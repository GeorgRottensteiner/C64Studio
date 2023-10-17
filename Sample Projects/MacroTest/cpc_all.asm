!cpu z80

* = $00

;LD r,r'

          ld b,b    ;40
          ld b,c    ;41
          ld b,d
          ld b,e
          ld b,h
          ld b,l    ;45
          ld b,a    ;47

          ld c,b    ;48
          ld c,c
          ld c,d
          ld c,e
          ld c,h
          ld c,l
          ld c,a

          ld d,b
          ld d,c
          ld d,d
          ld d,e
          ld d,h
          ld d,l
          ld d,a

          ld e,b
          ld e,c
          ld e,d
          ld e,e
          ld e,h
          ld e,l
          ld e,a

          ld h,b
          ld h,c
          ld h,d
          ld h,e
          ld h,h
          ld h,l
          ld h,a

          ld l,b
          ld l,c
          ld l,d
          ld l,e
          ld l,h
          ld l,l
          ld l,a

          ld a,b
          ld a,c
          ld a,d
          ld a,e
          ld a,h
          ld a,l    ;7d
          ld a,a    ;7f

;LD r,n

          ld b,0    ;06 00
          ld c,1    ;0e 01
          ld d,2    ;16 02
          ld e,3    ;1e 03
          ld h,4    ;26 04
          ld l,5    ;2e 05
          ld a,7    ;3e 07

;LD r,(HL)
          ld b,(HL) ;46
          ld c,(HL) ;4e
          ld d,(HL) ;56
          ld e,(HL) ;5e
          ld h,(HL) ;66
          ld l,(HL) ;6e
          ld a,(HL) ;7e

;LD r,(IX+d)
          ld b,(IX + 0)
          ld c,(IX + 1)
          ld d,(IX + 2)
          ld e,(IX + 3)
          ld h,(IX + 4)
          ld l,(IX + 5)
          ld a,(IX + 6)