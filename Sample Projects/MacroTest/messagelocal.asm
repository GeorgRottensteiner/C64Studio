* = $c000

!source "messagelocal_inc.asm"

!zone library

.local = 2

!message "library.test = ", library.local

!message "(library.)test = ", .local