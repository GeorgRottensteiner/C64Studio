*=3000
RND=$de1b
lda #111:ldx #222:ldy #33      ; This works.  :)
lda 111:ldx 222:ldy 33         ; This works.  :)
lda $de1b:ldx $de1b:ldy $de1b  ; This works.  :)
lda RND:ldx RND:ldy RND        ; *** But this FAILS. ***  :(
lda RND :ldx RND :ldy RND      ; Oddly, this again works.
lda RND : ldx RND : ldy RND    ; This also works.
lda RND: ldx RND: ldy RND      ; *** But this again FAILS. ***
