ORG $2000

DATA

HITABLE
    CT    EQU    0
    DO    20
    DH    DATA+(32*CT)
    CT    EQU    CT+1
    LOOP


DH 312
DL 312
DB 23,45+52-FRED,"This is a test message",13,10,"!"+128