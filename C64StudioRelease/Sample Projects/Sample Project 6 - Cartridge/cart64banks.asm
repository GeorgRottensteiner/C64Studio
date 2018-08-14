!to "cart64banks.crt.bin", magicdeskcrt

;bank 0
* = $8000

!zone BANK0
!bank 0,$2000

!word launcher
!word launcher
	!byte $c3 ;c
	!byte $c2 ;b
	!byte $cd ;m
	!byte $38 ;8
	!byte $30 ;0

launcher
	sei
	stx $d016
	jsr $fda3 ;prepare irq
	jsr $fd50 ;init memory
	jsr $fd15 ;init i/o
	jsr $ff5b ;init video
	cli


-
          lda #0
          sta 1024

          lda #$01
          bit $dc00
          bne .NotUpPressed

.WaitForRelease
          lda #$01
          bit $dc00
          beq .WaitForRelease

          lda #1
          sta $de00
.NotUpPressed

          jmp -


;bank 1
!bank 1,$2000
!zone BANK1

!pseudopc $8000

!word .launcher
!word .launcher
	!byte $c3 ;c
	!byte $c2 ;b
	!byte $cd ;m
	!byte $38 ;8
	!byte $30 ;0

.launcher
	sei
	stx $d016
	jsr $fda3 ;prepare irq
	jsr $fd50 ;init memory
	jsr $fd15 ;init i/o
	jsr $ff5b ;init video
	cli


-
          lda #1
          sta 1024

          lda #$01
          bit $dc00
          bne .NotUpPressed

.WaitForRelease
          lda #$01
          bit $dc00
          beq .WaitForRelease

          lda #2
          sta $de00
.NotUpPressed

          jmp -


;bank 2
!bank 2,$2000
!pseudopc $8000
!zone BANK2

!word .launcher
!word .launcher
	!byte $c3 ;c
	!byte $c2 ;b
	!byte $cd ;m
	!byte $38 ;8
	!byte $30 ;0

.launcher
	sei
	stx $d016
	jsr $fda3 ;prepare irq
	jsr $fd50 ;init memory
	jsr $fd15 ;init i/o
	jsr $ff5b ;init video
	cli


-
          lda #2
          sta 1024

          lda #$01
          bit $dc00
          bne .NotUpPressed

.WaitForRelease
          lda #$01
          bit $dc00
          beq .WaitForRelease

          lda #3
          sta $de00
.NotUpPressed

          jmp -


;bank 3
!bank 3,$2000
!zone BANK3
!pseudopc $8000

!word .launcher
!word .launcher
	!byte $c3 ;c
	!byte $c2 ;b
	!byte $cd ;m
	!byte $38 ;8
	!byte $30 ;0

.launcher
	sei
	stx $d016
	jsr $fda3 ;prepare irq
	jsr $fd50 ;init memory
	jsr $fd15 ;init i/o
	jsr $ff5b ;init video
	cli


-
          lda #3
          sta 1024

          lda #$01
          bit $dc00
          bne .NotUpPressed

.WaitForRelease
          lda #$01
          bit $dc00
          beq .WaitForRelease

          lda #4
          sta $de00
.NotUpPressed

          jmp -


;bank 4
!bank 4,$2000
!zone BANK4
!pseudopc $8000

!word .launcher
!word .launcher
	!byte $c3 ;c
	!byte $c2 ;b
	!byte $cd ;m
	!byte $38 ;8
	!byte $30 ;0

.launcher
	sei
	stx $d016
	jsr $fda3 ;prepare irq
	jsr $fd50 ;init memory
	jsr $fd15 ;init i/o
	jsr $ff5b ;init video
	cli


-
          lda #4
          sta 1024

          lda #$01
          bit $dc00
          bne .NotUpPressed

.WaitForRelease
          lda #$01
          bit $dc00
          beq .WaitForRelease

          lda #5
          sta $de00
.NotUpPressed

          jmp -


;bank 5
!bank 5,$2000
!zone BANK5
!pseudopc $8000

!word .launcher
!word .launcher
	!byte $c3 ;c
	!byte $c2 ;b
	!byte $cd ;m
	!byte $38 ;8
	!byte $30 ;0

.launcher
	sei
	stx $d016
	jsr $fda3 ;prepare irq
	jsr $fd50 ;init memory
	jsr $fd15 ;init i/o
	jsr $ff5b ;init video
	cli


-
          lda #5
          sta 1024

          lda #$01
          bit $dc00
          bne .NotUpPressed

.WaitForRelease
          lda #$01
          bit $dc00
          beq .WaitForRelease

          lda #6
          sta $de00
.NotUpPressed

          jmp -


;bank 6
!bank 6,$2000
!zone BANK6
!pseudopc $8000

!word .launcher
!word .launcher
	!byte $c3 ;c
	!byte $c2 ;b
	!byte $cd ;m
	!byte $38 ;8
	!byte $30 ;0

.launcher
	sei
	stx $d016
	jsr $fda3 ;prepare irq
	jsr $fd50 ;init memory
	jsr $fd15 ;init i/o
	jsr $ff5b ;init video
	cli


-
          lda #6
          sta 1024

          lda #$01
          bit $dc00
          bne .NotUpPressed

.WaitForRelease
          lda #$01
          bit $dc00
          beq .WaitForRelease

          lda #7
          sta $de00
.NotUpPressed

          jmp -


;bank 7
!bank 7,$2000
!zone BANK7
!pseudopc $8000

!word .launcher
!word .launcher
	!byte $c3 ;c
	!byte $c2 ;b
	!byte $cd ;m
	!byte $38 ;8
	!byte $30 ;0

.launcher
	sei
	stx $d016
	jsr $fda3 ;prepare irq
	jsr $fd50 ;init memory
	jsr $fd15 ;init i/o
	jsr $ff5b ;init video
	cli


-
          lda #7
          sta 1024

          lda #$01
          bit $dc00
          bne .NotUpPressed

.WaitForRelease
          lda #$01
          bit $dc00
          beq .WaitForRelease

          lda #0
          sta $de00
.NotUpPressed

          jmp -

