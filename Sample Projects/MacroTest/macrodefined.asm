* = $2000

TRUE = 1=1
FALSE = 1=0

!macro defined .x {
  !ifdef .x {
    mathlib.defined = TRUE
    !message "defined as",.x
  } else {
    mathlib.defined = FALSE
    !message "not defined"
  }
}


!macro test .x, .y {
  +defined .x
  !message "test: .x is defined ",mathlib.defined
  !if mathlib.defined {
    !message "test: x=", .x
  }

  +defined .y
  !message "test: .y is defined ",mathlib.defined
  !if mathlib.defined {
    !message "test: y=", .y
  }
}


j=1
k=2
+test j,k