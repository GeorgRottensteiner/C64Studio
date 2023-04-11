* = $2000

library.page_high = 2

!zone library
.page_high = $c8

!zone main

        lda library.page_high
!zone multilib


library.page_high = 3

.TRUE = -1
.common_loaded = .TRUE


x0 = $fb


.sqr_lo = $c000

abc = 45