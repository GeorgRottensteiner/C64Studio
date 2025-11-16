* = $0801

!macro testmacro v1,v2 {
  !ifdefparam v2 {
    !warn "v2 DEFINED"
  }
  !ifndefparam v2 {
    !warn "v2 NOT DEFINED"
  }
}

!macro testmacromitelse v1,v2 {
  !ifdefparam v2 {
    !warn "v2 DEFINED"
    tax
  } else {
    !warn "v2 NOT DEFINED"
    nop
  }
}


+testmacro b0
+testmacro b0 , w0

+testmacromitelse b0
+testmacromitelse b0, w0