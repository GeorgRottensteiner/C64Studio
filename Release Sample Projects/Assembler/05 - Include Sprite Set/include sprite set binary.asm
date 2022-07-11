;===============================================================================================
;C64Studio sample project - Assembly - Include Sprite Set Project
;
;press "Build and Run" (Default Ctrl-F5) to assembly and run this code
;
;This sample includes a sprite set from a binary file, and configures the VIC to display
;it. To update the sprite set just modify the "sprites.spriteproject". The resulting
;data is automatically included
;===============================================================================================

;include the library file
!source <kernal.asm>


;set the start address to BASIC start $0801
* = $0801

;add a simple BASIC start line (0 SYS ...) 
!basic 
          ;set background color
          lda #0
          sta VIC.BACKGROUND_COLOR

          ;set up sprite
          lda #200
          sta VIC.SPRITE_X_POS
          sta VIC.SPRITE_Y_POS
          
          lda #1
          sta VIC.SPRITE_MULTICOLOR
          lda #6
          sta VIC.SPRITE_MULTICOLOR_1
          lda #2
          sta VIC.SPRITE_MULTICOLOR_2
          
          ;set sprite pointer to $2000
          lda #( $2000 / 64 )
          sta $0400 + 1024 - 8
          
          lda #1
          sta VIC.SPRITE_ENABLE
          
          rts
          

;place sprite data at $2000          
* = $2000
          ;include data for 1 sprite from offset 0 (the first sprite)
          !bin "sprites.spr"