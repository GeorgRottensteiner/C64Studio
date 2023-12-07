* = $2000

;ACME has <, > unary operator precedence lower, so they affect the whole line

* = $2000

          ;with ACME this ends up as A9 C0
          lda #>$c000 + 1