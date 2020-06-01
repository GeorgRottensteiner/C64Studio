;===============================================================================================
;C64Studio sample project - Assembly - Include Library Files
;
;press "Build and Run" (Default Ctrl-F5) to assembly and run this code
;
;This sample shows the usage of library files. Library files can be included with a 
;!source directive using less/greater than brackets.
;C64Studio comes with a library files containing the C64s Kernal entry points and 
;definitions for the CIA/VIC/SID registers.
;
;Library files are searched from the configured paths. For this specific sample to work
;make sure a library path is set to the local folder containing "kernal.asm"
;===============================================================================================


;include the library file
!source <kernal.asm>


;set the start address to BASIC start $0801
* = $0801

;add a simple BASIC start line (0 SYS ...) 
!basic 

          ;set the border color to white
          lda #1
          sta VIC.BORDER_COLOR
          
          ;set the background color to black
          lda #0
          sta VIC.BACKGROUND_COLOR
          
          ;return to BASIC
          rts