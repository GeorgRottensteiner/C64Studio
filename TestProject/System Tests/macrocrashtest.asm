;Charset to sprite test 
;by Richard Bayliss 

;This program will transfer a single 
;char to a sprite position. For this 
;test only one sprite will be used. 

!to "charsprite.prg",cbm 

;Vars 
chr = $3000 ;char at $3000 
spr = $2000 ;sprite at $2000 

         *=$0801 ;Prompt basic SYS 
;!basic 2064 

         *=$0810 ;Main code start 
          
;Make a solid sprite at $2000 

         ldx #$00 
mksolid 
         lda #$ff 
         sta $2000,x 
         inx 
         cpx #$40 
         bne mksolid 
          
;Setup and enable the sprite 

         lda #$80 
         sta $07f8 
          
         lda #$01 
         sta $d015 
          
;A basic position for the sprite 

         lda #$80 
         sta $d000 
         sta $d001 
         lda #$00 
         sta $d010 
;Make it white 

         lda #$01 
         sta $d027 
          
;Also make the border/background colour red+black 

         lda #$02 
         ldx #$00 
         sta $d020 
         stx $d021 
          
;Time for some fun, generate a macro to build a 
;char into the sprite          
    
          
          
!macro GenChrSpr .char, .sprite { 
         lda .char 
         sta .sprite 
         lda .char+1 
         sta .sprite+3 
         lda .char+2 
         sta .sprite+6 
         lda .char+3 
         sta .sprite+9 
         lda .char+4 
         sta .sprite+12 
         lda .char+5 
         sta .sprite+15 
         lda .char+6 
         sta .sprite+18 
         lda .char+7 
         sta .sprite+21 
} 
            +GenChrSpr chr,spr 
         rts 
CHARS 
!byte $00,$7e,$42,$42,$42,$42,$7e,$00 
!byte $00,$10,$10,$10,$10,$10,$10,$00 
!byte $00,$7e,$02,$7e,$40,$40,$7e,$00 
!byte $00,$7e,$02,$1e,$02,$02,$7e,$00 
!byte $00,$42,$42,$42,$7e,$02,$02,$00 
!byte $00,$7e,$40,$7e,$02,$02,$7e,$00 
!byte $00,$40,$40,$7e,$42,$42,$7e,$00 
!byte $00,$7e,$02,$02,$02,$02,$02,$00 