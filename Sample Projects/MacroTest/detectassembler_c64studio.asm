; **** C64HLA 0.1 (c)2021 PureTom - High Level Assembly Preprocessor ****
; Pass 1 start 22:50:38 2021/04/05
; Compiling: Asm tests\t0.txt ... ok
*=49152
; Pass 1 end 22:50:38 2021/04/05
;*******************************************************************************
; Constants
;*******************************************************************************
_mymodxule0_character = 1 ; L:6, Asm tests\t0.txt
_mymodxule0_screen_address = 1024 ; L:7, Asm tests\t0.txt
;*******************************************************************************
; Variables at fixed memory locations
;*******************************************************************************
;*******************************************************************************
; Code
;*******************************************************************************
lda #_mymodxule0_character
sta _mymodxule0_screen_address
rts
;*******************************************************************************
; Initialized Variables, Data Sections
;*******************************************************************************