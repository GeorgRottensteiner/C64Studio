* = $2000

!macro runme v1,v2 {

  !ifdefparam v2 {
    ;only inserted if v2 is provided in call
    clc
    lda #v2

      !if v2>0 {
        nop
      }

    }

    ldx #v1

}

+runme 0   ; wirft den Error
;+runme 0,1 ; funktioniert