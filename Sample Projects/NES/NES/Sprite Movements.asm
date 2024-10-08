!to "Sprite Movements.nes",cartnes,$02,$00,$00,$01
;!to "Sprite Movements.nes",plain
  ;.inesprg $02   ; 2x 16KB PRG code
  ;.ineschr $00   ; 0x  8KB CHR data
  ;.inesmap $00   ;
  ;.inesmir $01   ; background mirroring

;header
;0-3 Constant $4E $45 $53 $1A (ASCII "NES" followed by MS-DOS end-of-file)
;4 Size of PRG ROM in 16 KB units
;5 Size of CHR ROM in 8 KB units (value 0 means the board uses CHR RAM)
;6 Flags 6 – Mapper, mirroring, battery, trainer
;7 Flags 7 – Mapper, VS/playchoice, NES 2.0
;8 Flags 8 – PRG-RAM size (rarely used extension)
;9 Flags 9 – TV system (rarely used extension)
;10  Flags 10 – TV system, PRG-RAM presence (unofficial, rarely used extension)
;11-15 Unused padding (should be filled with zero, but some rippers put their name across bytes 7-15)


!src "VariablesandConstants.asm"



;.bank 0
* = $8000


RESET
          sei
          cld

          ;disable APU frame IRQ
          ldx #$40
          stx $4017
          ldx #$FF
          txs          ; Set up stack
          inx          ; now X = 0
          stx $2000    ; disable NMI
          stx $2001    ; disable rendering
          stx $4010    ; disable DMC IRQs

vblankwait
          bit $2002
          bpl vblankwait

!lzone clrmem
          lda #$00
          sta $0000, x
          sta $0100, x
          sta $0300, x
          sta $0400, x
          sta $0500, x
          sta $0600, x
          sta $0700, x
          lda #$FE
          sta $0200, x
          inx
          bne clrmem

vblankwait2
          bit $2002
          bpl vblankwait2

          lda #$80                         ;here we set up any start points for enemies
          sta sprite_RAM                   ;in practice, these locations would be determined by your background
          sta sprite_RAM + 3

          lda #$90
          sta sprite_RAM + 16
          sta sprite_RAM + 19

          lda #$A0
          sta sprite_RAM + 32
          sta sprite_RAM + 35

          lda #$B0
          sta sprite_RAM + 48
          sta sprite_RAM + 51

          lda #<crewman_graphics       ;here we set up the pointer data for the different enemy types
          sta enemy_pointer                ;in practice, you wouldn't hard code these, they would be part of the loading routines
          lda #>( crewman_graphics + 1 )
          sta enemy_pointer + 1

          lda #<Punisher_graphics
          sta enemy_pointer + 2
          lda #>( Punisher_graphics + 1 )
          sta enemy_pointer + 3

          lda #<McBoobins_graphics
          sta enemy_pointer + 4
          lda #>( McBoobins_graphics + 1 )
          sta enemy_pointer + 5

          lda #<ArseFace_graphics
          sta enemy_pointer + 6
          lda #>( ArseFace_graphics + 1 )
          sta enemy_pointer + 7

;load palettes for sprites, background isn't used, so we don't need to populate it
LoadSpritePalettes
          lda $2002
          lda #$3F
          sta $2006
          lda #$10
          sta $2006

          ldx #$00
LoadSpritePalettesLoop
          lda spritepalette, x
          sta $2007
          inx
          cpx #$10
          bne LoadSpritePalettesLoop

          ;set the low byte (00) of the RAM address
          lda #$00
          sta $2003

          ;load the graphics into this "CHR-RAM" program
          ldx #$00
          jsr LoadcompleteBank

          ;enable NMI, sprites from Pattern Table 0, background from Pattern Table 1
          lda #%10010000
          sta $2000


;-----------------------START MAIN PROGRAM-----------------------------

!lzone Forever
          inc sleeping                     ;wait for NMI

.loop
          lda sleeping
          bne .loop                        ;wait for NMI to clear out the sleeping flag

          lda #$01
          sta updating_background          ;this is for when you are changing rooms or something, not really needed here
                                           ;it will skip the NMI updates so as not to mess with your room loading routines


          inc random_direction2            ;counter for random sprite directions

          inc random_direction1            ;this counter goes from 0-3 (each of the four directions)
          lda random_direction1            ;it is only accessed when a sprite switches direction
          cmp #$03
          bne .next
          lda #$00
          sta random_direction1
.next

!lzone UpdateENEMIES
          lda #$00                         ;this loop updates the enemies one at a time in a loop
          sta enemy_number                 ;start with enemy zero
          sta enemy_ptrnumber
.loop
          jsr enemy_update                 ;move the sprites based on which direction they are headed
          jsr Enemys_Animation             ;find out which frame the enemy animation is on
          jsr Enemys_Sprite_Loading        ;update the enemy meta tile graphics
          jsr update_enemy_sprites         ;update position

          inc enemy_number                 ;incriment the enemy number for the next trip through the loop
          inc enemy_ptrnumber              ;these are addresses for the graphics data, so we need to keep it at 2x the enemy number
          inc enemy_ptrnumber
          lda enemy_number
          cmp #$04                         ;if it is 4, we have updated enemies 0,1,2,3 so we are done
          bne .loop
UpdateENEMIESdone

          lda #$00                         ;tell NMI that we are done with graphics updates and it can do it's thing.
          sta updating_background

          jmp Forever                      ;jump back to Forever, and go back to sleep



NMI
          pha                              ;protect the registers
          txa
          pha
          tya
          pha

nmi_start

          lda updating_background          ;check to be sure that the main program isn't busy
          bne skip_graphics_updates

          lda #$02
          sta $4014                        ;set the high byte (02) of the RAM address, start the transfer

          lda #$00                         ;tell the ppu there is no background scrolling
          sta $2005
          sta $2005
          sta $2006
          sta $2006

          lda #%00011110                   ;enable sprites, enable background, no clipping on left side
          sta $2001

          lda #$00
          sta sleeping                     ;wake up the main program
          sta updating_background + 1

skip_graphics_updates

          pla
          tay
          pla
          tax
          pla

          rti



spritepalette
          !media "sprite-tiles.charsetproject",palette,0,4



;load in the sprite graphics
!lzone LoadcompleteBank
          ;this is all CHR-RAM stuff, not really covered here
          lda #<Sprite_Data
          sta tile_loader_ptr
          ;again, you wouldn't hard code this in practice
          lda #>( Sprite_Data + 1 )
          sta tile_loader_ptr + 1

          LDY #$00
          lda $2002
          lda ( tile_loader_ptr ),y
          sta $2006
          inc tile_loader_ptr
          lda ( tile_loader_ptr ),y
          sta $2006
          inc tile_loader_ptr
          ldx #$00
          ldy #$00
.LoadBank:
          lda ( tile_loader_ptr ),y
          sta $2007
          iny
          cpy #$00
          bne .LoadBank
          inc tile_loader_ptr + 1
          inx
          cpx #$10
          bne .LoadBank

          rts


;.bank 1
* = $A000


;constants for the use of the sprite_RAM constant
updateconstants
          ;4 sprites for each meta sprite, so add $10 for each meta sprite we process
          !byte $00,$10,$20,$30



;------Enemy Graphics Updates------------------------------------------------------------------------------

;these are random numbers used with the random_direction2 to make enemies switch direction
randomenemydirection
          !byte $57,$CD,$AF,$05,$BC



;-------------------------

!lzone enemy_update
          ldx enemy_number
          inc Enemy_Animation,x          ;incriment the counter for the animation routine

          lda random_direction2          ;check random_direction2 with the values stored in randomenemydirection
          cmp randomenemydirection,x     ;this is in place of the routine you would have in collision detection
          beq .down
          cmp randomenemydirection + 1,x
          bne .done
.down

          lda random_direction1          ;if the values match, switch direction the counter random_direction1
          sta enemy_direction,x

.done
          lda enemy_direction,x          ;for the various directions, move the sprites around the background
          bne .next
          jmp enemy_up                   ;note jmp not JSR
.next
          cmp #$01
          bne .next1
          jmp enemy_down
.next1
          cmp #$02
          bne .next2
          jmp enemy_right
.next2
          jmp enemy_left



enemy_up:
          ldx enemy_number                ;move the sprite up
          ldy updateconstants,x
          lda sprite_RAM,y
          sec
          sbc #enemy_speed
          sta sprite_RAM,y

          rts



enemy_down
          ;move the sprite down
          ldx enemy_number
          ldy updateconstants,x
          lda sprite_RAM,y
          clc
          adc #enemy_speed
          sta sprite_RAM,y

          rts



enemy_right
          ;move the sprite right
          ldx enemy_number
          ldy updateconstants,x
          lda sprite_RAM+3,y
          clc
          adc #enemy_speed
          sta sprite_RAM+3,y

          rts


enemy_left
          ;move the sprite left
          ldx enemy_number
          ldy updateconstants,x
          lda sprite_RAM+3,y
          sec
          sbc #enemy_speed
          sta sprite_RAM+3,y

          rts


;this routine updates the frame that is displayed for each enemy 1,2,1,or 3
Enemys_Animation

          ldx enemy_number
          lda Enemy_Animation,x           ;compare to constants
          cmp #enemyFrames1               ;you can change these around to make the animation faster or slower on the constants page
          beq .Animation1
          cmp #enemyFrames2
          beq .Animation2
          cmp #enemyFrames3
          beq .Animation1
          cmp #enemyFrames4
          beq .Animation3
          jmp .AnimationDone



          ;load the various frames
.Animation1
          lda #$00
          sta Enemy_Frame,x
          jmp .AnimationDone

.Animation2
          lda #$01
          sta Enemy_Frame,x
          jmp .AnimationDone

.Animation3
          lda #$02
          sta Enemy_Frame,x

.AnimationDone
          ;reset the counter when it gets to the end
          lda Enemy_Animation,x
          cmp #enemyFrames4
          bne .AnimationFinished
          sec
          sbc #enemyFrames4
          sta Enemy_Animation,x
.AnimationFinished
          rts



;the indexes to the various frames in the spritegraphics file
NM_up
          !byte $00,$08,$10
NM_down
          !byte $18,$20,$28
NM_right
          !byte $30,$38,$40
NM_left
          !byte $48,$50,$58



;this routine updates the sprite graphics every frame based on data set by the other routines
!lzone Enemys_Sprite_Loading

          ldx enemy_number
          lda enemy_direction,x
          beq .next  ;up                 ;find out which direction it is going
          cmp #$01
          beq .next1 ;down
          cmp #$02
          beq .next2 ;right
          jmp .next3 ;left

.next                            ;UP
          lda Enemy_Frame,x              ;load the spritegraphics index based on the frame number set in the animation routine
          tax                            ;some of this is redundant because I removed some of the more complex code here
          lda NM_up,x
          tay
          jmp enemyspritesupdate         ;update graphics

.next1                           ;DOWN
          lda Enemy_Frame,x
          tax
          lda NM_down,x
          tay
          jmp enemyspritesupdate

.next2                           ;RIGHT
          lda Enemy_Frame,x
          tax
          lda NM_right,x
          tay
          jmp enemyspritesupdate

.next3                           ;LEFT
          lda Enemy_Frame,x
          tax
          lda NM_left,x
          tay
          jmp enemyspritesupdate



;this routine updates the tiles and attributes for the enemies
enemyspritesupdate

          ldx enemy_ptrnumber             ;load in the pointer for the graphics data
          lda enemy_pointer,x
          sta enemygraphicspointer
          inx
          lda enemy_pointer,x
          sta enemygraphicspointer+1


;we put this here incase we want to have some sort of special update, like shooting graphics, it is not used here
subenemyspritesupdate
          ldx enemy_number
          lda updateconstants,x
          tax

          ;read the tile from the "spritegraphics" sub file and store it in memory
          lda [enemygraphicspointer],y
          sta sprite_RAM+1, x
          iny
          lda [enemygraphicspointer],y
          sta sprite_RAM+5, x
          iny
          lda [enemygraphicspointer],y
          sta sprite_RAM+9, x
          iny
          lda [enemygraphicspointer],y
          sta sprite_RAM+13, x
          iny
          lda [enemygraphicspointer],y
          sta sprite_RAM+2, x
          iny
          lda [enemygraphicspointer],y
          sta sprite_RAM+6, x
          iny
          lda [enemygraphicspointer],y
          sta sprite_RAM+10, x
          iny
          lda [enemygraphicspointer],y
          sta sprite_RAM+14, x

          rts



;this routine updates the position of the meta sprites relative to each other
update_enemy_sprites
          ldx enemy_number
          lda updateconstants,x
          tax

          lda sprite_RAM,x                ;vertical updates
          sta sprite_RAM+4,x
          clc
          adc #$08
          sta sprite_RAM+8,x
          sta sprite_RAM+12,x

          lda sprite_RAM+3,x              ;horizontal updates
          sta sprite_RAM+11,x
          clc
          adc #$08
          sta sprite_RAM+7,x
          sta sprite_RAM+15,x

          rts



;.bank 2
* = $C000
          !src "spritegraphics.asm"



;.bank 3
* = $E000


Sprite_Data
          ;sprite address in the PPU
          !byte $00,$00

          !media "sprite-tiles.charsetproject",CHAR



;first of the three vectors starts here
* = $FFFA
          !word NMI        ;when an NMI happens (once per frame if enabled) the
                           ;processor will jump to the label NMI:
          !word RESET      ;when the processor first turns on or is reset, it will jump
                           ;to the label RESET:
          !word 0          ;external interrupt IRQ is not used in this tutorial





