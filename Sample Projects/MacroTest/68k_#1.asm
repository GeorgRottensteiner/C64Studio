!cpu 68000

* = $00

  move.l #$0,d0
.loop:
  move.l d0,$dff180
  add.l #5,d0
  btst #6,$bfe001
  bne.s .loop
  rts



move.l #$0,d0
  lea $dff000,a0

.loop2:
  move.l d0,$180(a0)
  add.l #5,d0
  btst #6,$bfe001
  bne.s .loop2
  rts


  move.l #$0,d0
.loop3:
  move.w d0,$dff180
  add.b #5,d0         ; Cannot determine opcode
  add.w #5,d0         ; Cannot determine opcode
  add.l #5,d0          ; Allowed
  btst #6,$bfe001
  bne.s .loop3

  moveq #$0,d0
  lea $dff000,a0
.loop4:
  move.w d0,$180(a0)  ; Cannot determine opcode
  add.l #5,d0
  btst #6,$bfe001
  bne.s .loop4
  rts