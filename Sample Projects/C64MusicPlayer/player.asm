  ;------------------------------------------------------------------------------------
  ; 
  ; Please note this was mostly to learn the process 
  ;   also note O'Dog of Laxity you and your group can FUCK right off.
  ; Don't pre-release games for the 4kb compo which technically disqualified my entry
  ; And killed a lot of my passion for doing C64 titles 
  ; 
  ; Thanks to 
  ; Frantic/Hack'n'Trade player was a point of reference when re-educating myself with SID
  ; https://cadaver.github.io/rants/music.html this page was useful too 
  ; 
  ;------------------------------------------------------------------------------------

  ;------------------------------------------------------------------------------------
  ; Player Reset
  ; A = song ID
  ; X = trashed
  ; Y = trashed
  ;------------------------------------------------------------------------------------

player_Reset_func
  ; mul A * 2
  asl 
  tay

  ; clear the data we need 
  ; overkill 
  ldx #$00 
_clearloop
  lda #$20 
  sta TRACK0,x 
  lda #$00 
  sta $d400,x
  inx 
  cpx #3*7
  bne _clearloop

  ; get song pointers and set up VPEEK
  lda Songs,y
  sta VPEEK
  lda Songs+1,y
  sta VPEEK+1
  ; set the voice track pointers from the song def 
  ; 3 words
  ldy #$00
  lda (VPEEK),y 
  sta TRACK0.Address
  iny
  lda (VPEEK),y 
  sta TRACK0.Address+1 

  iny
  lda (VPEEK),y 
  sta TRACK1.Address
  iny
  lda (VPEEK),y 
  sta TRACK1.Address+1 

  iny 
  lda (VPEEK),y 
  sta TRACK2.Address
  iny
  lda (VPEEK),y 
  sta TRACK2.Address+1 

  ; set Clock to zero 
  lda #$01 
  sta TRACK0.Clock
  sta TRACK1.Clock
  sta TRACK2.Clock
  ; defaults
  lda #$03 
  sta TRACK0.Instrument
  lda #$02 
  sta TRACK1.Instrument
  lda #$03
  sta TRACK2.Instrument

  lda #%01111111  ;Hi-pass + Lo-pass filter on.
  sta $d418
  rts 

player_Update_func:
  
  ldx #$00

  ; Track 0 
  sec 
  dec TRACK0.Clock,x
  bne _noupdate0
  jsr InitialUpdate
_noupdate0

  ; Track 1
  ; X is 7 due to using SID regs,x
  ; so this is the voice 2   
  ldx #$07
  sec 
  dec TRACK0.Clock,x
  bne _noupdate1
  jsr InitialUpdate
_noupdate1

  ; Track 2
  ; X is 14 due to using SID regs,x
  ; so this is the voice 3   
  ldx #14
  sec 
  dec TRACK0.Clock,x
  bne _noupdate2
  jsr InitialUpdate
_noupdate2
  rts 
  ;------------------------------------------------------------------------------------
  ; BeginRepeat: 
  ; stored as command byte , count byte 
  ; copies the repeat amount and stores a pointer back to the beginning of the repeat ( skipping the header ) 
  ;------------------------------------------------------------------------------------
  
BeginRepeat:
  ; move ptr
  +NextCommand TRACK0.Address
  +PeekCommand TRACK0.Address
  sta TRACK0.RepeatCounter,x
  +NextCommand TRACK0.Address
  ; copy Address
  lda TRACK0.Address,x 
  sta TRACK0.RepeatAddress,x
  lda TRACK0.Address+1,x 
  sta TRACK0.RepeatAddress+1,x
  jmp updateV0

  ;------------------------------------------------------------------------------------
  ; EndRepeat: reset back to repeat point if saved repeat count = 0
  ; stored as command byte 
  ;------------------------------------------------------------------------------------
  
EndRepeat:
  dec TRACK0.RepeatCounter,x
  clc 
  lda TRACK0.RepeatCounter,x
  beq _SkipReptByte
  lda TRACK0.RepeatAddress,x 
  sta TRACK0.Address,x
  lda TRACK0.RepeatAddress+1,x 
  sta TRACK0.Address+1,x
  jmp updateV0
_SkipReptByte:
  +NextCommand TRACK0.Address
  jmp updateV0

  ;------------------------------------------------------------------------------------
  ; JumpTrack:
  ; unconditional jump the track pointer to the word after the command byte
  ;------------------------------------------------------------------------------------

JumpTrack:
  ; move ptr
  +NextCommand TRACK0.Address
  +PeekCommand TRACK0.Address
  sta JmpFunc
  +NextCommand TRACK0.Address
  +PeekCommand TRACK0.Address
  ; copy Address
  sta TRACK0.Address+1,x
  lda JmpFunc 
  sta TRACK0.Address,x
  jmp updateV0

  ;------------------------------------------------------------------------------------
  ; Set the track instrument to the next byte   
  ;------------------------------------------------------------------------------------

SetInstrument:
  ; next instruction
  +NextCommand TRACK0.Address
  +PeekCommand TRACK0.Address
  ; store that number into the current Instrument
  sta TRACK0.Instrument,x
  ; step to next instruction
  +NextCommand TRACK0.Address
  jmp updateV0

  ;------------------------------------------------------------------------------------
  ; Command Functions array 
  ;------------------------------------------------------------------------------------
PlayerFunctions:
  !word SkipNote,BeginRepeat,EndRepeat,SetInstrument,JumpTrack

  ;------------------------------------------------------------------------------------
  ; per track update function
  ;------------------------------------------------------------------------------------
InitialUpdate:
  
  ; note this looks like it's just TRACK0 
  ; but the macro's use 
  ; ARG,x
updateV0
  +PeekCommand TRACK0.Address
  tay
  ; is it a command ? 
  and #$80 
  ; no just go play the note
  beq _playnote
  ; it is 
  tya
  stx TempX
  ; <<1 to get word index
  and #$f
  asl 
  tax 
  ; copy function pointers
  lda PlayerFunctions,x 
  sta JmpFunc 
  lda PlayerFunctions+1,x 
  sta JmpFunc+1
  ; jump over 
  ldx TempX
  jmp (JmpFunc)
_playnote
  +PeekCommand TRACK0.Address
  tay 
  ; Set Frequency based on NOTE ID
  lda FrequencyLo,y
  sta $d400,x
  lda FrequencyHi,y
  sta $d401,x

  ; clear gate ?
  lda #$00
  sta $d404,x
  ; Set Instrument ADSR and Control values
  lda TRACK0.Instrument,x
  tay
  lda AttackDelayTable,y
  sta $d405,x
  lda ControlTable,y
  sta $d404,x
SkipNote
  ; no note is played from here on out
  lda TRACK0.Instrument,x
  tay
  ; reset the Clock 
  lda DurationTable,y
  sta TRACK0.Clock,x

  +NextCommand TRACK0.Address
  rts

  ;------------------------------------------------------------------------------------
  ; This is the PAL table
  ;------------------------------------------------------------------------------------

FrequencyHi:
  ;      C   C#  D   D#  E   F   F#  G   G#  A   A#  B
  !byte $01,$01,$01,$01,$01,$01,$01,$01,$01,$01,$01,$02  ; 1
  !byte $02,$02,$02,$02,$02,$02,$03,$03,$03,$03,$03,$04  ; 2
  !byte $04,$04,$04,$05,$05,$05,$06,$06,$06,$07,$07,$08  ; 3
  !byte $08,$09,$09,$0a,$0a,$0b,$0c,$0d,$0d,$0e,$0f,$10  ; 4
  !byte $11,$12,$13,$14,$15,$17,$18,$1a,$1b,$1d,$1f,$20  ; 5
  !byte $22,$24,$27,$29,$2b,$2e,$31,$34,$37,$3a,$3e,$41  ; 6
FrequencyLo:
  ;     C   C#  D   D#  E   F   F#  G   G#  A   A#  B
  !byte $17,$27,$39,$4b,$5f,$74,$8a,$a1,$ba,$d4,$f0,$0e  ; 1
  !byte $2d,$4e,$71,$96,$be,$e8,$14,$43,$74,$a9,$e1,$1c  ; 2
  !byte $5a,$9c,$e2,$2d,$7c,$cf,$28,$85,$e8,$52,$c1,$37  ; 3
  !byte $b4,$39,$c5,$5a,$f7,$9e,$4f,$0a,$d1,$a3,$82,$6e  ; 4
  !byte $68,$71,$8a,$b3,$ee,$3c,$9e,$15,$a2,$46,$04,$dc  ; 5
  !byte $d0,$e2,$14,$67,$dd,$79,$3c,$29,$44,$8d,$08,$b8  ; 6

  ;------------------------------------------------------------------------------------