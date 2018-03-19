FIRST_LINE
          !for ROW = 0 to 2 
          lda #ROW
          sta 1024 + ROW
          !end
SECOND_LOOP          
          !for ROW = 0 to 2
          lda #ROW
          sta 1064 + ROW
          !end
END_OF_LOOP
          lda #5
          sta 53281
LAST_LINE         