SCREEN_COLOR  = $d800
VIDEO_MEM     = $0400

* = $2000
VideMemTable
          !for y=9 to 12
            !for i=11 to 15
               !word (VIDEO_MEM + 40 * y) + i
            !end
          !end

;ScreenColorTable
          !for y=9 to 17
            !for i=11 to 19
               !word (SCREEN_COLOR + 40 * y) + i
            !end
          !end
