!macro plus .x
!set dload_zp_base = dload_zp_base + .x
!end

* = $2000
dload_zp_base
          +plus 4
          