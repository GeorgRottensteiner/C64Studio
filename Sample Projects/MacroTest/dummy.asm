* = $2000

REGISTER = 1
A_REG = 2
Y_REG = 3
X_REG = 4



IF_7_31_.error=1
!if ╚REGISTER╝=REGISTER & ╚REGISTER╝=REGISTER {
!if ╚A_REG╝=A_REG& Y_REG =X_REG {
IF_7_31_.error=0
stx IF_7_31_.tmp+1
IF_7_31_.tmp:
cmp #$ff
}

!if ╚A_REG╝=A_REG& Y_REG =Y_REG {
IF_7_31_.error=0 ; <========================= HIER
}


}

!if IF_7_31_.error=1 {
!error "Wrong Parameters in 'IF'"
}

rts
