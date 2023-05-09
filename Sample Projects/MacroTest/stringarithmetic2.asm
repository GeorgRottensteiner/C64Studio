;test case 19, comparing strings
;tested in 7.4.1b11 20230423

*=$c000

n = "a"
m = n + 1
!message "m='", m, "'"
;'a1/$1'
o = 1
!message "o='", o, "'"
;'1/$1'
