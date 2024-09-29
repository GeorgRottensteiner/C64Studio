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
;7 Flags 7 – Mapper, VS/Playchoice, NES 2.0
;8 Flags 8 – PRG-RAM size (rarely used extension)
;9 Flags 9 – TV system (rarely used extension)
;10  Flags 10 – TV system, PRG-RAM presence (unofficial, rarely used extension)
;11-15 Unused padding (should be filled with zero, but some rippers put their name across bytes 7-15)

* = $1000

!hex 4E 45 53 1A 02 00 01 00 00 00 00 00 00 00 00 00

;4E 45 53 1A 02 00 01 00 00 00 00 00 00 00 00 00
;!text "NES",$1a
;!byte $02   ; 2x 16KB PRG code - inesprg
;!byte $00   ; 0x  8KB CHR data - ineschr
;!byte $01   ; bg mirroring
;!byte $00   ; mapper
;!byte $00   ; mapper nes2.0
;!byte $00   ; prg-ram size
;!byte $00   ; TV system
;!byte $00   ; TV system PRG-RAM presence
;!fill 5,0

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;

  !src "VariablesandConstants.asm"

;;;;;;;;;;;;
;;;;;;;;;;;;

  ;.bank 0
* = $8000

;;;;;;;;;;;;
;;;;;;;;;;;;

RESET:
  SEI          ; disable IRQs
  CLD          ; disable decimal mode
  LDX #$40
  STX $4017    ; disable APU frame IRQ
  LDX #$FF
  TXS          ; Set up stack
  INX          ; now X = 0
  STX $2000    ; disable NMI
  STX $2001    ; disable rendering
  STX $4010    ; disable DMC IRQs

vblankwait
  BIT $2002
  BPL vblankwait

!lzone clrmem
  LDA #$00
  STA $0000, x
  STA $0100, x
  STA $0300, x
  STA $0400, x
  STA $0500, x
  STA $0600, x
  STA $0700, x
  LDA #$FE
  STA $0200, x
  INX
  BNE clrmem

vblankwait2
  BIT $2002
  BPL vblankwait2

  LDA #$80                         ;here we set up any start points for enemies
  STA sprite_RAM                   ;in practice, these locations would be determined by your background
  STA sprite_RAM+3

  LDA #$90
  STA sprite_RAM+16
  STA sprite_RAM+19

  LDA #$A0
  STA sprite_RAM+32
  STA sprite_RAM+35

  LDA #$B0
  STA sprite_RAM+48
  STA sprite_RAM+51

  LDA #<crewman_graphics       ;here we set up the pointer data for the different enemy types
  STA enemy_pointer                ;in practice, you wouldn't hard code these, they would be part of the loading routines
  LDA #>( crewman_graphics+1)
  STA enemy_pointer+1

  LDA #<(Punisher_graphics)
  STA enemy_pointer+2
  LDA #>(Punisher_graphics+1)
  STA enemy_pointer+3

  LDA #<(McBoobins_graphics)
  STA enemy_pointer+4
  LDA #>(McBoobins_graphics+1)
  STA enemy_pointer+5

  LDA #<(ArseFace_graphics)
  STA enemy_pointer+6
  LDA #>(ArseFace_graphics+1)
  STA enemy_pointer+7

LoadSpritePalettes:                ;load pallettes for sprites, background isn't used, so we don't need to populate it
  LDA $2002
  LDA #$3F
  STA $2006
  LDA #$10
  STA $2006
  LDX #$00
LoadSpritePalettesLoop:
  LDA spritepalette, x
  STA $2007
  INX
  CPX #$10
  BNE LoadSpritePalettesLoop

  LDA #$00
  STA $2003                         ; set the low byte (00) of the RAM address

  LDX #$00
  JSR LoadCompleteBank              ;load the graphics into this "CHR-RAM" program

  LDA #%10010000                    ; enable NMI, sprites from Pattern Table 0, background from Pattern Table 1
  STA $2000

;----------------------------------------------------------------------
;-----------------------START MAIN PROGRAM-----------------------------
;----------------------------------------------------------------------

!lzone Forever
  INC sleeping                     ;wait for NMI

.loop
  LDA sleeping
  BNE .loop                        ;wait for NMI to clear out the sleeping flag

  LDA #$01
  STA updating_background          ;this is for when you are changing rooms or something, not really needed here
                                   ;it will skip the NMI updates so as not to mess with your room loading routines


  INC random_direction2            ;counter for random sprite directions

  INC random_direction1            ;this counter goes from 0-3 (each of the four directions)
  LDA random_direction1            ;it is only accessed when a sprite switches direction
  CMP #$03
  BNE .next
  LDA #$00
  STA random_direction1
.next

!lzone UpdateENEMIES
  LDA #$00                         ;this loop updates the enemies one at a time in a loop
  STA enemy_number                 ;start with enemy zero
  STA enemy_ptrnumber
.loop
  JSR enemy_update                 ;move the sprites based on which direction they are headed
  JSR Enemys_Animation             ;find out which frame the enemy animation is on
  JSR Enemys_Sprite_Loading        ;update the enemy meta tile graphics
  JSR update_enemy_sprites         ;update position

  INC enemy_number                 ;incriment the enemy number for the next trip through the loop
  INC enemy_ptrnumber              ;these are addresses for the graphics data, so we need to keep it at 2x the enemy number
  INC enemy_ptrnumber
  LDA enemy_number
  CMP #$04                         ;if it is 4, we have updated enemies 0,1,2,3 so we are done
  BNE .loop
UpdateENEMIESdone:

  LDA #$00                         ;tell NMI that we are done with graphics updates and it can do it's thing.
  STA updating_background

  JMP Forever                      ;jump back to Forever, and go back to sleep



;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
;--------THE FUCKIN' NMI ROUTINE, RECOGNIZE BITCH!------------;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;

NMI:
  PHA                              ;protect the registers
  TXA
  PHA
  TYA
  PHA

nmi_start:

  LDA updating_background          ;check to be sure that the main program isn't busy
  BNE skip_graphics_updates

  LDA #$02
  STA $4014                        ;set the high byte (02) of the RAM address, start the transfer

  LDA #$00                         ;tell the ppu there is no background scrolling
  STA $2005
  STA $2005
  STA $2006
  STA $2006

  LDA #%00011110                   ;enable sprites, enable background, no clipping on left side
  STA $2001

  LDA #$00
  STA sleeping                     ;wake up the main program
  STA updating_background+1

skip_graphics_updates:

  PLA                              ;restore the registers
  TAY
  PLA
  TAX
  PLA

  RTI                              ;return from interrupt

;-----------------------------------------------------------------------------------------------------

spritepalette:
  !byte $0F,$04,$1C,$10, $0F,$05,$11,$08, $0F,$16,$29,$08, $0F,$28,$31,$0B   ;sprite palette

;-----------------------------------------------------------------------------------------------------

LoadCompleteBank:                 ;load in the sprite graphics

  LDA #<(Sprite_Data)           ;this is all CHR-RAM stuff, not really covered here
  STA tile_loader_ptr             ;again, you wouldn't hard code this in practice
  LDA #>(Sprite_Data+1)
  STA tile_loader_ptr+1

  LDY #$00
  LDA $2002
  LDA [tile_loader_ptr],y
  STA $2006
  INC tile_loader_ptr
  LDA [tile_loader_ptr],y
  STA $2006
  INC tile_loader_ptr
  LDX #$00
  LDY #$00
.LoadBank:
  LDA [tile_loader_ptr],y
  STA $2007
  INY
  CPY #$00
  BNE .LoadBank
  INC tile_loader_ptr+1
  INX
  CPX #$10
  BNE .LoadBank

  RTS

;;;;;;;;;;;;
;;;;;;;;;;;;

  ;.bank 1
  * = $A000

;;;;;;;;;;;;
;;;;;;;;;;;;

;-----------------------

updateconstants:                  ;constants for the use of the sprite_RAM constant
  !byte $00,$10,$20,$30             ;4 sprites for each meta sprite, so add $10 for each meta sprite we process

;----------------------------------------------------------------------------------------------------------
;------Enemy Graphics Updates------------------------------------------------------------------------------
;----------------------------------------------------------------------------------------------------------

randomenemydirection:            ;these are random numbers used with the random_direction2 to make enemies switch direction
  !byte $57,$CD,$AF,$05,$BC

;-------------------------

!lzone enemy_update
  LDX enemy_number
  INC Enemy_Animation,x          ;incriment the counter for the animation routine

  LDA random_direction2          ;check random_direction2 with the values stored in randomenemydirection
  CMP randomenemydirection,x     ;this is in place of the routine you would have in collision detection
  BEQ .down
  CMP randomenemydirection+1,x
  BNE .done
.down

  LDA random_direction1          ;if the values match, switch direction the counter random_direction1
  STA enemy_direction,x

.done
  LDA enemy_direction,x          ;for the various directions, move the sprites around the background
  BNE .next
  JMP enemy_up                   ;note JMP not JSR
.next
  CMP #$01
  BNE .next1
  JMP enemy_down
.next1
  CMP #$02
  BNE .next2
  JMP enemy_right
.next2
  JMP enemy_left

;------------------------------

enemy_up:

  LDX enemy_number                ;move the sprite up
  LDY updateconstants,x
  LDA sprite_RAM,y
  SEC
  SBC #enemy_speed
  STA sprite_RAM,y

  RTS

;------------------------------

enemy_down:

  LDX enemy_number                ;move the sprite down
  LDY updateconstants,x
  LDA sprite_RAM,y
  CLC
  ADC #enemy_speed
  STA sprite_RAM,y

  RTS

;------------------------------

enemy_right:

  LDX enemy_number                ;move the sprite right
  LDY updateconstants,x
  LDA sprite_RAM+3,y
  CLC
  ADC #enemy_speed
  STA sprite_RAM+3,y

  RTS

;------------------------------

enemy_left:

  LDX enemy_number                ;move the sprite left
  LDY updateconstants,x
  LDA sprite_RAM+3,y
  SEC
  SBC #enemy_speed
  STA sprite_RAM+3,y

  RTS

;------------------------------

Enemys_Animation:                 ;this routine updates the frame that is displayed for each enemy 1,2,1,or 3

  LDX enemy_number

  LDA Enemy_Animation,x           ;compare to constants
  CMP #enemyFrames1               ;you can change these around to make the animation faster or slower on the constants page
  BEQ .Animation1
  CMP #enemyFrames2
  BEQ .Animation2
  CMP #enemyFrames3
  BEQ .Animation1
  CMP #enemyFrames4
  BEQ .Animation3
  JMP .AnimationDone

.Animation1:                      ;load the various frames
  LDA #$00
  STA Enemy_Frame,x
  JMP .AnimationDone
.Animation2:
  LDA #$01
  STA Enemy_Frame,x
  JMP .AnimationDone
.Animation3:
  LDA #$02
  STA Enemy_Frame,x
.AnimationDone

  LDA Enemy_Animation,x           ;reset the counter when it gets to the end
  CMP #enemyFrames4
  BNE .AnimationFinished
  SEC
  SBC #enemyFrames4
  STA Enemy_Animation,x
.AnimationFinished
  RTS

;-----------------------------

NM_up:
  !byte $00,$08,$10                ;the indexes to the various frames in the spritegraphics file
NM_down:
  !byte $18,$20,$28
NM_right:
  !byte $30,$38,$40
NM_left:
  !byte $48,$50,$58

;-------------------------------

!lzone Enemys_Sprite_Loading           ;this routine updates the sprite graphics every frame based on data set by the other routines

  LDX enemy_number
  LDA enemy_direction,x
  BEQ .next  ;up                 ;find out which direction it is going
  CMP #$01
  BEQ .next1 ;down
  CMP #$02
  BEQ .next2 ;right
  JMP .next3 ;left

.next                            ;UP
  LDA Enemy_Frame,x              ;load the spritegraphics index based on the frame number set in the animation routine
  TAX                            ;some of this is redundant because I removed some of the more complex code here
  LDA NM_up,x
  TAY
  JMP enemyspritesupdate         ;update graphics

.next1                           ;DOWN
  LDA Enemy_Frame,x
  TAX
  LDA NM_down,x
  TAY
  JMP enemyspritesupdate

.next2                           ;RIGHT
  LDA Enemy_Frame,x
  TAX
  LDA NM_right,x
  TAY
  JMP enemyspritesupdate

.next3                           ;LEFT
  LDA Enemy_Frame,x
  TAX
  LDA NM_left,x
  TAY
  JMP enemyspritesupdate

;-------------------------------

enemyspritesupdate:               ;this routine updates the tiles and attributes for the enemies

  LDX enemy_ptrnumber             ;load in the pointer for the graphics data
  LDA enemy_pointer,x
  STA enemygraphicspointer
  INX
  LDA enemy_pointer,x
  STA enemygraphicspointer+1

subenemyspritesupdate:            ;we put this here incase we want to have some sort of special update, like shooting graphics, it is not used here
  LDX enemy_number
  LDA updateconstants,x
  TAX

  LDA [enemygraphicspointer],y    ;read the tile from the "spritegraphics" sub file and store it in memory
  STA sprite_RAM+1, x
  INY
  LDA [enemygraphicspointer],y
  STA sprite_RAM+5, x
  INY
  LDA [enemygraphicspointer],y
  STA sprite_RAM+9, x
  INY
  LDA [enemygraphicspointer],y
  STA sprite_RAM+13, x
  INY
  LDA [enemygraphicspointer],y
  STA sprite_RAM+2, x
  INY
  LDA [enemygraphicspointer],y
  STA sprite_RAM+6, x
  INY
  LDA [enemygraphicspointer],y
  STA sprite_RAM+10, x
  INY
  LDA [enemygraphicspointer],y
  STA sprite_RAM+14, x

  RTS

;-------------------------------------

update_enemy_sprites:             ;this routine updates the position of the meta sprites relative to each other
  LDX enemy_number
  LDA updateconstants,x
  TAX

  LDA sprite_RAM,x                ;vertical updates
  STA sprite_RAM+4,x
  CLC
  ADC #$08
  STA sprite_RAM+8,x
  STA sprite_RAM+12,x

  LDA sprite_RAM+3,x              ;horizontal updates
  STA sprite_RAM+11,x
  CLC
  ADC #$08
  STA sprite_RAM+7,x
  STA sprite_RAM+15,x

  RTS

;-------------------------------------

;;;;;;;;;;;;
;;;;;;;;;;;;

  ;.bank 2
  * = $C000

;;;;;;;;;;;;
;;;;;;;;;;;;

  !src "spritegraphics.asm"

;;;;;;;;;;;;
;;;;;;;;;;;;
;;;;;;;;;;;;

  ;.bank 3
  * = $E000

;;;;;;;;;;;;
;;;;;;;;;;;;

Sprite_Data:
  !byte $00,$00                   ;sprite address in the PPU

  !bin "SpriteMovement.chr"  ;include the sprite graphics data

;;;;;;;;;;;;

  * = $FFFA     ;first of the three vectors starts here
  !word NMI        ;when an NMI happens (once per frame if enabled) the
                   ;processor will jump to the label NMI:
  !word RESET      ;when the processor first turns on or is reset, it will jump
                   ;to the label RESET:
  !word 0          ;external interrupt IRQ is not used in this tutorial

;;;;;;;;;;;;
;;;;;;;;;;;;




