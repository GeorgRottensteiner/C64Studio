;===============================================================================================
;C64Studio sample project - Assembly - Simple Assembly
;
;press "Build and Run" (Default Ctrl-F5) to assembly and run this code
;
;This sample includes a character set from a charset project file, and configures the VIC to 
;display it. To update the character set just modify the "sprites.spriteproject". The 
;resulting data is automatically included
;===============================================================================================

;include the library file
!source <kernal.asm>


;set the start address to BASIC start $0801
* = $0801

;add a simple BASIC start line (0 SYS ...) 
!basic 
          ;$18 
          ;  $1x                => screen at $0400 (offset from VIC bank 3 at $0000)
          ;  $x8 = %xxxx 100x   => character data at $2000 (offset %100 * 2048 = from VIC bank 3 at $0000)
          lda #$18
          sta VIC.MEMORY_CONTROL
          
          ;fill top line of screen with custom character (0)
          lda #0
          ldx #0
-          
          sta $0400,x
          inx
          cpx #40
          bne -
          
          rts
          

;place character set at $2000          
* = $2000          
          !media "sprites.spriteproject",SPRITE