!cpu m65

;oho no

* = $1000
          orq [$23]

          ;ora [$23],z
          
          ;these should not work
          ;lda ($23)
          ;lda [$23]