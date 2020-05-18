  ;------------------------------------------------------------------------------------
  ; 
  ; Please note this was mostly to learn the process 
  ;   also note O'Dog of Laxity you and your group FUCKING SUCK
  ; 
  ;------------------------------------------------------------------------------------
  
ZEROPAGE_ADDRESS = $10  

VPEEK  = ZEROPAGE_ADDRESS ;!word ?
              ; storage for the track 
              ; it's important that there are 3 directly next to each other 
;TRACK0  .dstruct Track
;TRACK1  .dstruct Track
;TRACK2  .dstruct Track

!zone TRACK0
TRACK0  = ZEROPAGE_ADDRESS + 2
  .Clock          = TRACK0 + 0     ;!byte ? ; 0 
  .Address        = TRACK0 + 1  ;!word ? ; 1-2
  .Instrument     = TRACK0 + 3  ;!byte ? ; 3
  .RepeatAddress  = TRACK0 + 4 ;!word ? ; 4-5
  .RepeatCounter  = TRACK0 + 6 ;!byte ? ; 6

!zone TRACK1
TRACK1  = ZEROPAGE_ADDRESS + 7
  .Clock          = TRACK1 + 0     ;!byte ? ; 0 
  .Address        = TRACK1 + 1  ;!word ? ; 1-2
  .Instrument     = TRACK1 + 3  ;!byte ? ; 3
  .RepeatAddress  = TRACK1 + 4 ;!word ? ; 4-5
  .RepeatCounter  = TRACK1 + 6 ;!byte ? ; 6
  
!zone TRACK2
TRACK2  = ZEROPAGE_ADDRESS + 14
  .Clock          = TRACK2 + 0     ;!byte ? ; 0 
  .Address        = TRACK2 + 1  ;!word ? ; 1-2
  .Instrument     = TRACK2 + 3  ;!byte ? ; 3
  .RepeatAddress  = TRACK2 + 4 ;!word ? ; 4-5
  .RepeatCounter  = TRACK2 + 6 ;!byte ? ; 6
  

JmpFunc = ZEROPAGE_ADDRESS + 21 ; !word ?
TempX   = ZEROPAGE_ADDRESS + 23 ;!byte ?

;PATTERN0 .dstruct Pattern
;PATTERN1 .dstruct Pattern
;PATTERN2 .dstruct Pattern