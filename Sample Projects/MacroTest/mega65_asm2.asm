; ZPs
  !zone ZP
  * = $02
  .Screen:
    !word ?
    !word ?
    
  ; CONSTs
  ;SCREEN_RAM = $10000
  SCREEN_RAM = $10000


!cpu m65

*=$2001
   !basic 2020,"",entry
   
entry:
  jsr SystemSetup
  
  jsr SetupVectors
  
  jsr Clear80Screen
  jsr WriteHelloWorld
  
  lda #$02
  ldz #$00
  sta [ZP.Screen],z
  
  ldz #$00
  stz $d020
  stz $d021
  jmp *
  

!zone SetupVectors
SetupVectors:
  lda #<(SCREEN_RAM)
  sta ZP.Screen + 0
  lda #>(SCREEN_RAM)
  sta ZP.Screen + 1
  lda #(SCREEN_RAM >> 16)
  sta ZP.Screen + 2
  lda #$00
  sta ZP.Screen + 3

  ; Move Default screen location
  ; $D060-$D063
  lda ZP.Screen + 0
  sta $d060
  lda ZP.Screen + 1
  sta $d061
  lda ZP.Screen + 2
  sta $d062
  lda ZP.Screen + 3
  sta $d063

  rts

!zone Clear80Screen
Clear80Screen:
  lda #$20
  ldz #$00
-
  ;sta SCREEN_RAM,x
  sta [ZP.Screen],z
  ;sta SCREEN_RAM + 1 * $100,x
  ;sta SCREEN_RAM + 2 * $100,x
  ;sta SCREEN_RAM + 3 * $100,x
  ;sta SCREEN_RAM + 4 * $100,x
  ;sta SCREEN_RAM + 5 * $100,x
  ;sta SCREEN_RAM + 6 * $100,x
  ;sta SCREEN_RAM + 7 * $100,x
  inz 
  bne -
  rts

!zone WriteHelloWorld
WriteHelloWorld:
  ldx #$00
-
  lda hellotxt,x
  beq +
  sta $0800,x
  lda #$01
  sta $d800,x
  inx
  jmp -
+  
  rts

!zone SystemSetup
SystemSetup:
  sei
  
  ; Set memory
  lda #$35
  sta $01

  ; Enable 40Mhz
  lda #$41
  sta $00

  ; Enable VIC4 registers
  lda #$00
  tax
  tay
  taz
  map
  eom
  
  ; Turn off CiA
  lda #$7f
  sta $dc0d
  sta $dd0d
  
  ; Disable D65 rom write protection
  lda #$70
  sta $d640
  eom
  
  
  ; Turn off raster interrupts
  lda #$00
  sta $d01a
  cli

  rts
  
hellotxt:
  !scr "hello world",0