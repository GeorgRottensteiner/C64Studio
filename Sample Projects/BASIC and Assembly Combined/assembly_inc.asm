

* = 4096
MEIN_START
BASIC_SIZE = io.filesize( basic.prg )

!if ( $0801 + BASIC_SIZE ) > MEIN_START {

!warn "Achtung! BASIC überlappt ins Assembly!"

}

JUMP_ADDRESS
          inc $d020
          rts