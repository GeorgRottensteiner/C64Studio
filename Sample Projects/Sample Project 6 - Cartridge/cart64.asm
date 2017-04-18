!to "cart64.crt", magicdeskcrt

;bank 0
* = $8000

!zone BANK0

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

.endofbank
!fill 8139,1
.endofbankfill

;bank 1
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

.endofbank
!fill 8139,2
.endofbankfill

;bank 2
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

.endofbank
!fill 8139,3
.endofbankfill

;bank 3
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

.endofbank
!fill 8139,4
.endofbankfill

;bank 4
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

.endofbank
!fill 8139,5
.endofbankfill

;bank 5
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

.endofbank
!fill 8139,6
.endofbankfill



;bank 6
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

.endofbank
!fill 8139,7
.endofbankfill



;bank 7
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

.endofbank
!fill 8139,8
.endofbankfill
