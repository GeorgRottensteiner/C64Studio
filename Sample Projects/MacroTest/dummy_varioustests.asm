*=$0801


* = $40000
            !basic

            inc $d020

            .theValue2 = -2147483647; This works.  :)
            .theValue3 = -2147483648; Was broken in 5.8 build of 2018 - 09 - 08, but fixed in build of 2018 - 09 - 15(is a valid 32 - bit integer )
            .theValue4 = -65536; This works.  :)
            .theValue5 = -256; This works.  :)
            .theValue6 = 256; This works.  :)
            .theValue7 = 65535; This works.  :)
            .theValue8 = 65536; This works.  :)
            .theValue9 = -2147483647; This works.  :)

            .theValue = 0
            .intMinVal = -2147483648

            !if ( 0 <= -2147483648 ) {
            }

            ; ***ERROR * **
            !if ( -2147483648 >= 0 ) {
            }

            ; ***ERROR * **
            !if (.theValue <= -2147483648 ) {
            }

            ; ***ERROR * **
            !if ( -2147483648 >= .theValue ) {
            }

            ; ***ERROR * **
            !if (.theValue <= .intMinVal ) {
            }

            ; OK in 2018 - 09 - 15 build( was broken in 2018 - 09 - 08 build )
            !if (.intMinVal >= .theValue ) {
            }

            ; OK in 2018 - 09 - 15 build( was broken in 2018 - 09 - 08 build )
            !if (.theValue <> 0 ) {
            }

            ; OK in 2018 - 09 - 15 build( was broken in 2018 - 09 - 08 build )
            !if (.theValue = 0 ) {
            }

            ; OK in 2018 - 09 - 15 build: Previous E1001 now fixed in 2018 - 09 - 15 build( when.theValue = -2147483648 )
            inc $d020

            rts