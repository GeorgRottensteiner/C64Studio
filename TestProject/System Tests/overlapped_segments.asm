!to "overlapped_segments.prg",cbm

* = $2000
          !byte $11,$22,$33,$44
          
          !fill $1000 - 4
          
* = $3000
          !byte $55,$66,$77,$88
          
* = $1000
          !byte $AA,$BB,$CC,$DD
          
             