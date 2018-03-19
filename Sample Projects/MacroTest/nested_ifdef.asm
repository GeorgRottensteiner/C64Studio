!to "nested_ifdef.prg",cbm


TEST_DEFINE1
;TEST_DEFINE2

* = $2000 


!ifdef TEST_DEFINE1 {
 
    !byte 1 
    !message "path 1"
  
  
} else {

    !byte 2
    !message "path 2"
}