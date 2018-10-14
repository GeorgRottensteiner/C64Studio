!to "all-bytes.prg",plain

* = $2000

!byte   0
!byte   1
!byte   2
!byte 255
;!byte 256   ;out of range
!byte -1
!byte -128  ;ok
;!byte -129  ;out of range

!word 0
!word 1
!word 2
!word 255
!word 65535
;!word 65536 ;out of range
!word -1
!word -32768
;!word -32769 ;out of range