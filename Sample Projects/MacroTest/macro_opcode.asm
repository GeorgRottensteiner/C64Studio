text_ram = $0400

* = $2000

!macro print s,z,adr
ldx #0
lda adr,x
sta text_ram+(z*40)+s,x
!end


+print 2,4,$3000+5

rts


* = $3000
!text "lsmf"