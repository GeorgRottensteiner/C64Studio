DEBUG       = 0
SONG_SPEED  = $05

    !source "player_defs.asm"
    !source "player_macros.asm"


  ; Zero Page allocations
  ;*=$10
  !source "player_zeropage.asm"

  ; basic start
  *=$0801
  
  !basic 2020,initialize

initialize:
  sei 
  lda #$00 
  sta $d020 
  sta $d021 

  ; reset the player
  lda #$00  ; song ID
  jsr Player_Reset

  ; clear screen
  ldx #$00 
  lda #$20
_clear
  sta $0400,x
  sta $0500,x 
  dex 
  bne _clear  

  ; simple wait for raster 
  ; call update
mainloop:

  lda $d012 
  cmp #$80
  bne mainloop
  inc $d020 
  inc $d021

  jsr Player_Update

  dec $d021
  dec $d020 

!if DEBUG=1  {
  ldx #$00 
_dloop
  lda TRACK0,x
  sta $0400,x 
; lda PATTERN0,x
; sta $0428,x 
  inx 
  cpx #3*7
  bne _dloop
}

  jmp mainloop

  *=$0880
Player_Reset:
  jmp player_Reset_func
Player_Update:
  jmp player_Update_func

; NOTE aligned to 256 bytes
; simple tune from X Men 1st class
Voice0:
P0:
  !byte BEGIN_REPT,4
  !byte G4
  !byte C5
  !byte G4
  !byte F4
  !byte END_REPT
P1: 
  !byte BEGIN_REPT,4
  !byte G4
  !byte As4
  !byte G4
  !byte F4
  !byte END_REPT
P2: 
  !byte BEGIN_REPT,4
  !byte G4
  !byte A4
  !byte G4
  !byte F4
  !byte END_REPT
P3: 
  !byte BEGIN_REPT,4
  !byte G4
  !byte Gs4
  !byte G4
  !byte F4
  !byte END_REPT
  ; loop back 
  !byte JUMP_TRACK
  !word Voice0

  ; wait for a bit then do some base 
WaitV1:
  !byte INSTRUMENT,1
  !byte BEGIN_REPT,4
  !byte BLANK_NOTE,BLANK_NOTE
  !byte END_REPT
  ;loop point 
Voice1:
  !byte INSTRUMENT,2
  !byte C3
  !byte Ds3
  !byte F3
  !byte Gs3
  !byte JUMP_TRACK
  !word Voice1

  ; wait for a bit then do some notes
WaitV2:
  !byte INSTRUMENT,1
  !byte BEGIN_REPT,8
  !byte BLANK_NOTE,BLANK_NOTE
  !byte END_REPT
V2Loop:
  !byte INSTRUMENT,2
  !byte C5
  !byte C4
  !byte C5
  !byte C4
  !byte C5  
  !byte JUMP_TRACK
  !word V2Loop

  ; blank track
Voice2: 
  !byte BLANK_NOTE,BLANK_NOTE
  !byte JUMP_TRACK
  !word Voice2

;  drums lol
Kitt0:
  !byte INSTRUMENT,$6
  !byte C4
  !byte BLANK_NOTE
  !byte C4
  !byte BLANK_NOTE
  !byte C4
  !byte C4
  !byte C4
  !byte BLANK_NOTE
  !byte JUMP_TRACK
  !word Kitt0

; riff from Micro Mages 
Mages:
  !byte INSTRUMENT,4
  !byte B4  
  !byte As4 
  !byte Fs4 
  !byte Cs4
  
  !byte BEGIN_REPT,3
  !byte B3
  !byte As4 
  !byte Fs4 
  !byte Cs4
  !byte END_REPT  
  
  !byte As4 
  !byte Fs4 
  !byte Cs4
  !byte B3  
  
  !byte JUMP_TRACK
  !word Mages

Song0:
  !word Voice0,WaitV1,WaitV2
Song1:
  !word Mages,Voice2,Kitt0

Songs:
  !word Song0,Song1 

;                       0       1                     2         3         4         5     6 
AttackDelayTable: !byte $1b,    $1b,                  $cd,      $2b,      $1a,      $00, $16
ControlTable:     !byte PULSE,  TRIANGLE,             SAWTOOTH, SAWTOOTH, SAWTOOTH, $00, NOISE
DurationTable:    !byte SONG_SPEED*2, SONG_SPEED*16,  SONG_SPEED*32, SONG_SPEED*2, SONG_SPEED*1, SONG_SPEED*8, SONG_SPEED*1

  !source "player.asm"

!if DEBUG = 1 {
OUTHEX:
  sty $fb
  stx $fc
  ldy #$00
  pha
  lsr
  lsr
  lsr
  lsr
  tax
  lda tab,x
  sta ($fb),y
  iny
  pla
  and #$0f
  tax
  lda tab,x
  sta ($fb),y
  rts

tab:
      .text "0123456789"
  !byte 1,2,3,4,5,6,7,8

}