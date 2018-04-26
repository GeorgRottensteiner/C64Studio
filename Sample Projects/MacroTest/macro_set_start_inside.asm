!macro ENDIF {
  
curr_addr = *
* = STK_LVL_1 - 1
!byte ( curr_addr - STK_LVL_1 ) & 0xff
  
  }

* = 49152

STK_LVL_1 = 500

+ENDIF

rts 