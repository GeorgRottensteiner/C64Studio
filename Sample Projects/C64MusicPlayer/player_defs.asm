  ;------------------------------------------------------------------------------------
  ; 
  ; Please note this was mostly to learn the process 
  ;   also note O'Dog of Laxity you and your group bunch of wankers
  ; 
  ;------------------------------------------------------------------------------------

  ;------------------------------------------------------------------------------------
  ; Notes + Octave as an Index into the frequency table
  ;------------------------------------------------------------------------------------

  C1  = 0 + (0*12) 
  Cs1   = 1 + (0*12)
  D1  = 2 + (0*12)
  Ds1 = 3 + (0*12)
  E1  = 4 + (0*12)
  F1  = 5 + (0*12)
  Fs1   = 6 + (0*12)
  G1  = 7 + (0*12)
  Gs1   = 8 + (0*12)
  A1  = 9 + (0*12)
  As1   = 10 + (0*12)
  B1  = 11 + (0*12)

  C2  = 0 + (1*12)
  Cs2   = 1 + (1*12)
  D2  = 2 + (1*12)
  Ds2 = 3 + (1*12)
  E2  = 4 + (1*12)
  F2  = 5 + (1*12)
  Fs2   = 6 + (1*12)
  G2  = 7 + (1*12)
  Gs2   = 8 + (1*12)
  A2  = 9 + (1*12)
  As2   = 10 + (1*12)
  B2  = 11 + (1*12)

  C3  = 0 + (2*12)
  Cs3   = 1 + (2*12)
  D3  = 2 + (2*12)
  Ds3 = 3 + (2*12)
  E3  = 4 + (2*12)
  F3  = 5 + (2*12)
  Fs3   = 6 + (2*12)
  G3  = 7 + (2*12)
  Gs3   = 8 + (2*12)
  A3  = 9 + (2*12)
  As3   = 10 + (2*12)
  B3  = 11 + (2*12)

  C4  = 0 + (3*12)
  Cs4   = 1 + (3*12)
  D4  = 2 + (3*12)
  Ds4 = 3 + (3*12)
  E4  = 4 + (3*12)
  F4  = 5 + (3*12)
  Fs4   = 6 + (3*12)
  G4  = 7 + (3*12)
  Gs4   = 8 + (3*12)
  A4  = 9 + (3*12)
  As4   = 10 + (3*12)
  B4  = 11 + (3*12)

  C5  = 0 + (4*12)
  Cs5   = 1 + (4*12)
  D5  = 2 + (4*12)
  Ds5 = 3 + (4*12)
  E5  = 4 + (4*12)
  F5  = 5 + (4*12)
  Fs5   = 6 + (4*12)
  G5  = 7 + (4*12)
  Gs5   = 8 + (4*12)
  A5  = 9 + (4*12)
  As5   = 10 + (4*12)
  B5  = 11 + (4*12)

  ;------------------------------------------------------------------------------------
  ; Sound types for instrument definitions  
  ;------------------------------------------------------------------------------------

  NOISE   = %10000001
  PULSE   = %01000001
  SAWTOOTH  = %00100001
  TRIANGLE  = %00010001

  ;------------------------------------------------------------------------------------
  ; anything > $80 is a command byte 

  ;------------------------------------------------------------------------------------
  ;   don't play anything
  BLANK_NOTE  = $80
  ;------------------------------------------------------------------------------------

  ;------------------------------------------------------------------------------------
  ;   begin repeat 
  ;   byte  $81
  ;   byte  repeat count
  BEGIN_REPT  = $81
  ;------------------------------------------------------------------------------------

  ;------------------------------------------------------------------------------------
  ;   end repeat
  ;   if repeat count > 0 then jump back to the point JUST after the begin repeat block 
  END_REPT  = $82
  ;------------------------------------------------------------------------------------

  ;------------------------------------------------------------------------------------
  ;   set the next instrument 
  ;   byte $83    ; command
  ;   byte instrument   ; index 
  INSTRUMENT  = $83
  ;------------------------------------------------------------------------------------

  ;------------------------------------------------------------------------------------
  ;   jump the track to a new pointer
  ;   byte $84 
  ;   word address
  JUMP_TRACK  = $84
  ;------------------------------------------------------------------------------------

  ;------------------------------------------------------------------------------------
  ;   describe the per track data structure 
  ;   7 bytes available
  ;------------------------------------------------------------------------------------
;Track .struct         
;  Clock   !byte ? ; 0 
;  Address   !word ? ; 1-2
;  Instrument  !byte ? ; 3
;  RepeatAddress !word ? ; 4-5
;  RepeatCounter !byte ? ; 6
;.ends 
  ;------------------------------------------------------------------------------------
  ;   pattern struct
  ;   unused
  ;------------------------------------------------------------------------------------
;Pattern .struct 
;  !byte ? ; 0
;  !byte ? ; 1
;  !byte ? ; 2
;  !byte ? ; 3
;  !byte ? ; 4
;  !byte ? ; 5
;  !byte ? ; 6
;.ends
  ;------------------------------------------------------------------------------------