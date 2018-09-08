* = $2000
  
!addr k_chrout = $ffd2  ; this is an address
      CLEAR = 147   ; but this is not
    !addr {
      ; these are addresses:
      sid_v1_control  = $d404
      sid_v2_control  = $d40b
      sid_v3_control  = $d412
    }
    ; these are not:
    sid_VOICECONTROL_NOISE    = %#.......
    sid_VOICECONTROL_RECTANGLE  = %.#......
    sid_VOICECONTROL_SAWTOOTH = %..#.....
    sid_VOICECONTROL_TRIANGLE = %...#....

  rts