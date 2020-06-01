;===============================================================================================
;C64Studio sample project - Assembly - Simple Assembly
;
;press "Build and Run" (Default Ctrl-F5) to assembly and run this code
;
;This sample consists of a simple code part. It loads the value 1 in the Accumulator register
;and stores it in the VICs border color register. Thus setting the border color to white.
;===============================================================================================


;set the start address to BASIC start $0801
* = $0801

;add a simple BASIC start line (0 SYS ...) 
!basic 

          ;set the border color to white
          lda #1
          sta $d020
          
          ;return to BASIC
          rts