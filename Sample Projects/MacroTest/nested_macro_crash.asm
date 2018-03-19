;**** Start Source Code

!macro print_hex4_acc {
clc
cmp #10
bpl +

adc #$30
jmp ++

+ 
adc #$36
++ 
jsr $ffd2
}

!macro print_hex8_acc {
tax
clc
lsr
lsr
lsr
lsr
+print_hex4_acc
txa
and #%00001111
+print_hex4_acc
}

!macro print_hex16_adr .value{ 
lda .value+1 
+print_hex8_acc
lda .value
+print_hex8_acc

}

!macro ausgabe{ 
+print_hex16_adr $fb
}

*=$801
!basic main
main
  +ausgabe  

rts
test
!byte 0

;**** End Source Code