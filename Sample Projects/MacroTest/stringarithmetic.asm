;test case 19, comparing strings
;tested in 7.4.1b11 20230423

*=$c000

a = "a"
!if a = "a" {
  !message "pass, a='a'"
} else {
  !message "sorry, the test case failed :("
}
;expected: pass
;result: fail. E1001: Could not evaluate expression. Argument taken as a number.
