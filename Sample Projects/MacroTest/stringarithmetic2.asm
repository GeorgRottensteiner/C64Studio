* = $2000
lda #("A" + $80)  ; lda #$41 (??)


;test case 19, comparing strings
;tested in 7.4.1b11 20230423

n1 = "a"
m1 = n1 + 1
!message "m1='", m1, "'"

n2 = "abc"
m2 = n2 + 1
!message "m2='", m2, "'"

;'a1/$1'
o = 1
!message "o='", o, "'"
;'1/$1'
