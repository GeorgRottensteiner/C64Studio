!cpu m65

* = $2000
!ifdef INCLUDE_SD {

!macro SD_LOAD_CHIPRAM addr, fname
          bra +

.FileName
          !text fname
          !byte $00

+
          lda #>.FileName
          ldx #<.FileName
          jsr SDIO.CopyFileName

          ldx #<addr
          ldy #>addr
          ldz #( addr & $ff0000 ) >> 16

          lda #HVC_SD_TO_CHIPRAM
          sta Mega65.HTRAP00
          eom
!end



!macro SD_LOAD_ATTICRAM addr, fname
          bra +

.FileName
          !text fname
          !byte $00

+
          lda #>.FileName
          ldx #<.FileName
          jsr SDIO.CopyFileName

          ldx #<addr
          ldy #>addr
          ldz #( addr & $ff0000 ) >> 16

          lda #HVC_SD_TO_ATTICRAM
          sta Mega65.HTRAP00
          eom
!end


!zone SDIO
CopyFileName
          sta .FileName + 1
          stx .FileName + 0

          ldx #$00
-

.FileName = * + 1
          lda $BEEF, x
          sta SDFILENAME, x
          inx
          bne -

          ;Make hypervisor call to set filename to load
          ldx #<SDFILENAME
          ldy #>SDFILENAME
          lda #$2e
          sta Mega65.HTRAP00
          eom
          rts
}


!zone FLOPPYIO

!realign $10 ;Keep these vars bytes from crossing a page

BASEPAGE = >*
.FileNamePtr      !word $0000
.BufferPtr        !word $0000
.NextTrack        !byte $00
.NextSector       !byte $00
.PotentialTrack   !byte $00
.PotentialSector  !byte $00
.SectorHalf       !byte $00
