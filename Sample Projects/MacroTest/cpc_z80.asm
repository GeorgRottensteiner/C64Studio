!cpu z80

* = $00

;; A procedure to display a 8-bit number in hex form
;;
.txt_output = $bb5a

;;----------------------------
;; DISPLAY A BYTE IN HEX FORM
;;
;; Enter:
;; A = 8-bit value
;;
;; Exit:
;; AF corrupt, All other registers preserved

.print_hex_byte
push af         ;store original byte value

;; move upper nibble into lower nibble
rrca
rrca
rrca
rrca

;; display nibble as ASCII
call .print_byte_digit
pop af          ;retrieve original byte value

;;-------------------------------------------
;; display number in lower nibble (bits 3..0)
.print_byte_digit
and %00001111   ;isolate lower nibble. (This contains the digit value 0...15)
add a,"0"       ;add ASCII for 0. Digits 0...9 become "0"..."9", digits 10..15
                ;become ":"...."A"
;cp "9"+1
cp '9'+1
jr c,.number     ;if number is in digit range 0...9, display digit

add a,'A'-'9'-1 ;modify ASCII value so that digits in the range 10...15
                ;;become "A"...."F"
.number
jp .txt_output   ;display digit