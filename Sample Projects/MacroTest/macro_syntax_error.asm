SCREEN_CHAR = $0400
BACKUP_COLUMN = $c000

!macro first_to_backup_column .startrow, .endrow {
    !for .i, .startrow, .endrow {
        lda SCREEN_CHAR + (.i * 40)
        sta BACKUP_COLUMN + .i
    }
}

* = $1000
+first_to_backup_column 0,19