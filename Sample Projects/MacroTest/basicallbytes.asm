* = $0801
          !byte < LINE_2 , > LINE_2
          !word 10
          !byte $99,$22

          !fill 32,[ i ]
          !byte $22,0

LINE_2
          !byte < LINE_3 , > LINE_3
          !word 20
          !byte $99,$22

          !fill 32,[ 32 + i ]
          !byte $22,0

LINE_3
          !byte < LINE_4 , > LINE_4
          !word 30
          !byte $99,$22

          !fill 32,[ 2 * 32 + i ]
          !byte $22,0

LINE_4
          !byte < LINE_5 , > LINE_5
          !word 40
          !byte $99,$22

          !fill 32,[ 3 * 32 + i ]
          !byte $22,0

LINE_5
          !byte < LINE_6 , > LINE_6
          !word 50
          !byte $99,$22

          !fill 32,[ 4 * 32 + i ]
          !byte $22,0

LINE_6
          !byte < LINE_7 , > LINE_7
          !word 60
          !byte $99,$22

          !fill 32,[ 5 * 32 + i ]
          !byte $22,0

LINE_7
          !byte < LINE_8 , > LINE_8
          !word 70
          !byte $99,$22

          !fill 32,[ 6 * 32 + i ]
          !byte $22,0

LINE_8
          !byte < LINE_9 , > LINE_9
          !word 80
          !byte $99,$22

          !fill 32,[ 7 * 32 + i ]
          !byte $22,0

LINE_9
          !byte 0,0