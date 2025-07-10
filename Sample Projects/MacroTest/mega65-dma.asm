!src <mega65.asm>

!cpu m65

* = $2001
!basic
          jsr ClearGraphicLocation
          rts


GRAPHIC_LOCATION = $20000

!lzone ClearGraphicLocation
          +RunDMAJob Job
          rts

Job
          +DMAHeader $00, GRAPHIC_LOCATION >> 20
          +DMAFillJob $0, GRAPHIC_LOCATION, 128 * 256, $0

