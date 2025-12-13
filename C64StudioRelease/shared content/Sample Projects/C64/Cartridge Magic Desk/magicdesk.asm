!to "magicdesk.crt" , magicdeskcrt

!src <c64.asm>

          ;this macro is used to insert the same code in every bank
          ;only variation is the bank number which is inserted via argument
!macro HandleBank bank
!bank bank , $2000

;generates a dynamic zone name, concattenating BANK with the current bank number (BANK0 up to BANK7)
!zone BANK##bank

          ;make all code appear to assemble at $8000
!pseudopc $8000
          ;RESET vector (cold start)
          !word launcher
          ;NMI vector   (warm start, RESTORE)
          !word launcher
          ;magic number (must be "CBM80")
          !pet "CBM80"

launcher
          sei
          stx VIC.CONTROL_2   ;activate VIC
          jsr $fda3           ;init CIA
          jsr $fd50           ;check RAM
          jsr $fd15           ;init kernal vectors
          jsr $ff5b           ;init VIC
          cli                 ;enable interrupts

-
          ;display BANK<number>
          lda #'B' - 64
          sta $0400
          lda #'A' - 64
          sta $0401
          lda #'N' - 64
          sta $0402
          lda #'K' - 64
          sta $0403

          lda #bank
          clc
          adc #$30
          sta $0405

          ;check if joystick is pushed up
          lda #$01
          bit CIA1.DATA_PORT_A
          bne .NotUpPressed

          ;check if joystick up is released
.WaitForRelease
          lda #$01
          bit CIA1.DATA_PORT_A
          beq .WaitForRelease

          ;switch cartridge to next bank
          ;the moment $de00 is written to the relevant bank is active from $8000 to $9fff
          ;since we place the same code in all banks the CPU can continue
          lda #( bank + 1 ) % 8
          sta $de00
.NotUpPressed

          jmp -
!end



* = $8000

          +HandleBank 0
          +HandleBank 1
          +HandleBank 2
          +HandleBank 3
          +HandleBank 4
          +HandleBank 5
          +HandleBank 6
          +HandleBank 7

