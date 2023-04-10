;a proposed macro definition
!macro set_ifndef var, val, var_str {
  !ifndef var {
    !set var = val
    !message "mathlib: ", var_str, " defaulted to: ", var
  } else {
    !message "mathlib: ", var_str, " overridden to: ", var
  }
}

* = $0801
!basic
          +set_ifndef .page_high, $c2, ".page_high"


          +set_ifndef .page_high, $c2, ".page_high"
          rts