;===============================================================================================
;C64Studio sample project - Assembly - Simple Assembly
;
;press "Build and Run" (Default Ctrl-F5) to assembly and run this code
;
;This sample includes binary data from a file, and copies the 10 bytes to the top left of the
;screen.
;===============================================================================================


;set the start address to BASIC start $0801
* = $0801

;add a simple BASIC start line (0 SYS ...) 
!basic 

          ;copy the binary data to the screen
          ldx #0
          
-          
          lda BINARY_DATA,x
          sta $0400,x
          
          inx
          cpx #10
          bne -
          
          ;return to BASIC
          rts
          
          
BINARY_DATA
          !bin "binary.bin"