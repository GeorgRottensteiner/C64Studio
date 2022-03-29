* = $2000

      lda #<TEXT
      sta $fe
      lda #>TEXT
      sta $ff

      lda TEXT,y
      rts



TEXT
      !text "hurz"