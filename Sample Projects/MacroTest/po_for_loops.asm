* = $1000

;ACME style
!for i, 0, 24 {
  !byte i
}


;C64Studio style
!for i = 0 TO 24
  !byte i
!end

;broken style #1
!for i = 0 TO 24
  !byte i
}

;broken style #2
!for i, 0, 24 {
  !byte i
!end

