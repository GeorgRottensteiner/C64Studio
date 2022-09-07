*= $0801
BasicUpstart(main)

*= $0810
main:
.break
    inc $d021
l1:
    inc cntlo
    bne l1
    inc cnthi
    bne l1
    jmp main
cntlo:  .by 0
cnthi:  .by 0