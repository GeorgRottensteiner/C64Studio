* = $2000

tmp = $02           ;tmp is defined as $02
     .byte tmp      ;assembles as $02
sub                 ;our subroutine label name, 'sub'
     .block        
tmp = $ff           ;tmp is defined as $ff but only applies in the block
     .byte tmp      ;assembles as $ff, not $02!
     .bend
     .byte tmp      ;assembles as  $02 again...