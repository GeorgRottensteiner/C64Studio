!to "text-modes.prg",cbm
* = $2000
  byte_value = $1234
        !text "ABCabc123"
        !text <byte_value,>byte_value,
        !text <byte_value + 1,>byte_value + 1
        !text ( 12 * 34 ) + 1