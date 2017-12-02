
  
!macro SBAHN_CHECK_SPRITE_COLLISION sprite_x_reg, sprite_pointer_reg, .additional_x_bit_mask {
  lda sprite_pointer_reg
  cmp #$E9
  beq +                         ;kollision mit schienen interessiert nicht
  lda $D010
  and #%.additional_x_bit_mask
  beq +                         ;extended bit gesetzt? Dann kollision nicht möglich
  lda sprite_x_reg
  cmp $D000
  bcc +                         ;Sprite hinter figur? Dann kollision nicht möglich (schon forbei)
  sec
  sbc $D000
  cmp #23                       ;Abstand über 23? Dann für Kollision noch zu früh
  bcs +
  lda #%10000000
  sta STATUS
  rts
+ nop
}

