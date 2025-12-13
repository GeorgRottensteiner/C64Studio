* = $2000

if_level = 0

!macro IF var, condition, type, val {

  if_level = if_level + 1

  !set if_condition##if_level = condition
  !set test##if_level = 2

  !warn "if_condition##if_level>>", if_condition##if_level
  !warn "test##if_level>>", test##if_level

          }

          +IF gnu , 2 , hurz , value
          +IF gnu , 3 , hurz , value


