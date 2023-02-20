base_register=3

!macro lda_ora rotated,i,j,offset_1,offset_2
   ldy #offset_1
   lda+1 (base_register+(rotated*64)+i+j*8),y
   tay
   lda+1 ($f0+(rotated*4)),y
   ldy #offset_2
   lda+1 (base_register+i+j*8),y
   tay
   ora+1 ($f2+(rotated*4)),y

!end



* = $2000
          +lda_ora 1,2,3,4,5
          rts