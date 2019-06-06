;�����������������������������������������������������������������������������ͻ
;�Escape from New York by Cadaver for Cosine's crapgame contest                �
;�Done on 21st September 1999!                                                 �
;�����������������������������������������������������������������������������ͼ

SPRITES         = $2000
CHARS           = $3800
MUSIC           = $4000
MAP             = $4800
BLOCKS          = $5000
PICTURE         = $6000
RASTER0POS      = 242
RASTER1POS      = 20

ACT_NONE        = 0
ACT_PLISSKEN    = 1
ACT_MAN1        = 2
ACT_MAN2        = 3
ACT_MAN3        = 4
ACT_MOTORIST    = 5
ACT_EXPLOSION   = 6


DISPGAME        = 0
DISPTITLE       = 1
DISPTEXT        = 2
JOY_UP          = 1
JOY_DOWN        = 2
JOY_LEFT        = 4
JOY_RIGHT       = 8
JOY_FIRE        = 16

dispmode        = $02
scrollx         = $03
joystick        = $04
prevjoy         = $05
mapadrlo        = $06
mapadrhi        = $07
mapx            = $08
blockx          = $09
temp1           = $0a
temp2           = $0b
temp3           = $0c
temp4           = $0d
temp5           = $0e
rastercount     = $0f

                processor 6502
                org $0810

efnystart:      cld
                jsr initraster
                jsr initscreen
                lda #$00
                jsr MUSIC
title:          lda score+2
                cmp hiscore+2
                bcc title_nohiscore
                beq title_check2
                bcs title_hiscore
title_check2:   lda score+1
                cmp hiscore+1
                bcc title_nohiscore
                beq title_check3
                bcs title_hiscore
title_check3:   lda score
                cmp hiscore
                bcc title_nohiscore
title_hiscore:  lda score
                sta hiscore
                lda score+1
                sta hiscore+1
                lda score+2
                sta hiscore+2
title_nohiscore:jsr showpic
                jsr drawscores
titleloop:      jsr waitras
                jsr getjoystick
                lda joystick
                and #JOY_FIRE
                beq titleloop

gamestart:      lda #$00
                sta score
                sta score+1
                sta score+2
                lda #3
                sta lives
                lda #1
                sta level

initlevel:      lda #$00
                sta $d015
                jsr drawscores
                jsr initscroll
                lda #0
                sta killactive
                sta killmeter
                sta killmeterd
initlevel2:     jsr waitras
                lda #4
                jsr doscroll
                lda mapx
                cmp #10
                bcs initlevel3
                jmp initlevel2
initlevel3:     ldx #7
initlevel4:     lda #ACT_NONE
                sta actt,x
                dex
                bpl initlevel4
                ldx #15
initlevel5:     lda #$00
                sta bullett,x
                dex
                bpl initlevel5

initlife:       lda #$99
                sta time
                lda #$00
                sta timedl
                lda #128
                sta actxl
                lda #0
                sta actxh
                lda #20*8
                sta acty
                lda #0
                sta actd
                sta actf
                sta actj
                sta actsx
                sta actsy
                sta actyd
                sta firedelay
                lda #ACT_PLISSKEN
                sta actt
                lda #200
                sta actimm
                lda #1
                sta acthp

gameloop:       jsr getjoystick
                jsr moveactors
                jsr plrshoot
                jsr spawnenemies
                jsr waitras
                jsr erasebullets
                jsr movebullets
                jsr drawscores
                jsr killmeterr
                jsr waitras
                jsr checkscroll
                jsr doscroll
                jsr drawactors
                jsr drawbullets
                jsr dectime
                jsr checkdeath
                jsr checklevelend
                jmp gameloop

checklevelend:  lda killactive
                beq cle2
                lda killmeter
                cmp killlimit
                bcc cle2
                pla
                pla
countbonus:     lda time
                beq countbonus2
                sed
                lda time
                sec
                sbc #$01
                sta time
                lda score+1
                clc
                adc #$01
                sta score+1
                bcc noextra
                inc lives
noextra:        lda score+2
                adc #$00
                sta score+2
                cld
                jsr drawscores
                jsr waitras
                jsr waitras
                jsr waitras
                jsr waitras
                jmp countbonus
countbonus2:    lda level
                cmp #$03
                beq complete
                inc level
                jmp initlevel
cle2:           rts
complete:       jsr initscroll
                lda #DISPTEXT
                sta dispmode
                ldx #0
                stx $d015
complete1:      lda ctext1,x
                and #$3f
                sta $400+4*40+8,x
                lda ctext2,x
                and #$3f
                sta $400+6*40+8,x
                lda ctext3,x
                and #$3f
                sta $400+8*40+8,x
                lda ctext4,x
                and #$3f
                sta $400+10*40+8,x
                lda #$01
                sta $d800+4*40+8,x
                sta $d800+6*40+8,x
                sta $d800+8*40+8,x
                sta $d800+10*40+8,x
                inx
                cpx #24
                bne complete1
                lda #255
                sta temp1
complete2:      jsr waitras
                jsr waitras
                dec temp1
                bne complete2
                jmp title
ctext1:         dc.b "     WELL DONE SNAKE    "
ctext2:         dc.b "YOUR MISSION IS COMPLETE"
ctext3:         dc.b "AND YOU HAVE EARNED YOUR"
ctext4:         dc.b "        FREEDOM.        "

killmeterr:     lda killactive
                bne killmeter2
                lda mapx
                cmp #100
                bcc killmeter2
                inc killactive
                sta killactive
                lda #96
                sta $400+42
                lda #97
                sta $400+43
                lda #98
                sta $400+44
                lda #99
                sta $400+45
                ldx level
                dex
                lda killlimittbl,x
                sta killlimit
                tax
drawkillloop:   lda #100
                sta $400+45,x
                dex
                bne drawkillloop
killmeter2:     rts

killlimittbl:   dc.b 8,12,24

dectime:        lda time
                beq nodectime
                inc timedl
                lda timedl
                cmp #50
                bcc nodectime
                lda #$00
                sta timedl
                sed
                lda time
                sec
                sbc #$01
                sta time
                cld
                bne nodectime
                lda #0
                sta acthp
nodectime:      rts

checkdeath:     lda actf
                cmp #7
                bne cdeath_not
                lda acty
                cmp #240
                bcc cdeath_not
                pla
                pla
                dec lives
                lda lives
                beq cdeath_gameover
                jmp initlife
cdeath_gameover:jmp title
cdeath_not:     rts

plrshoot:       lda firedelay
                beq plrshootok
                dec firedelay
                rts
plrshootok:     lda actf
                cmp #7
                bne plrshootok2
plrshootnot:    rts
plrshootok2:    lda joystick
                and #JOY_FIRE
                beq plrshootnot
                lda prevjoy
                and #JOY_FIRE
                bne plrshootnot
                ldx #7
plrshootfind:   lda bullett,x
                beq plrshootfound
                dex
                bpl plrshootfind
                rts
plrshootfound:  lda actxl
                sta bulletxl,x
                lda actxh
                sta bulletxh,x
                lda acty
                sec
                sbc actshootymod
                sta bullety,x
                lda actd
                sta bulletd,x
                lda #1
                sta bullett,x
                lda #2
                sta firedelay
                rts

plr:            lda #42
                sta actsizey
                lda #20
                sta actshootymod
                lda acthp,x
                bne plr_nodeath
                lda actf,x
                cmp #7
                beq plr_nodinit
                lda #7
                sta actf,x
                lda #-5
                sta actsy,x
                lda #0
                sta actyd,x
plr_nodinit:    inc actyd,x
                lda actyd,x
                cmp #3
                bcc plr_d2
                lda #$00
                sta actyd,x
                inc actsy,x
plr_d2:         jsr moveactory
                rts
plr_nodeath:    lda actj,x
                beq plr_nofly
plr_fly:        lda #5
                sta actf,x
                inc actyd,x
                lda actyd,x
                cmp #5
                bcc plr_fly2
                lda #$00
                sta actyd,x
                inc actsy,x
plr_fly2:       jsr plisskenmovex
                jsr moveactory
                lda actsy,x
                beq plr_noground
                bmi plr_noground
                jsr checkground
                bcs plr_noground
                lda #0
                sta actsy,x
                sta actj,x
                lda acty,x
                and #$f8
                sta acty,x
plr_noground:   rts
plr_nofly:      jsr checkground
                bcc plr_nofall
                lda #1
                sta actj,x
                lda #0
                sta actsy,x
                sta actyd,x
                jmp plr_fly
plr_nofall:     lda joystick
                and #JOY_UP
                beq plr_nojump
                lda prevjoy
                and #JOY_UP
                bne plr_nojump
                lda #1
                sta actj,x
                lda #0
                sta actyd,x
                lda #-4
                sta actsy,x
                jmp plr_fly
plr_nojump:     lda #0
                sta actsx,x
                lda joystick
                and #JOY_LEFT
                beq plr_notleft
                lda #1
                sta actd,x
                lda #-2
                sta actsx,x
                jmp plr_walkanim
plr_notleft:    lda joystick
                and #JOY_RIGHT
                beq plr_notright
                lda #0
                sta actd,x
                lda #2
                sta actsx,x
                jmp plr_walkanim
plr_notright:   lda #0
                sta actf,x
                jmp plr_domove
plr_walkanim:   inc actfd,x
                lda actfd,x
                cmp #5
                bcc plr_walkanim2
                lda #$00
                sta actfd,x
                lda actf,x
                and #$03
                clc
                adc #$01
                sta actf,x
plr_walkanim2:
plr_domove:     lda joystick
                and #JOY_DOWN
                beq plr_noduck
                lda #6
                sta actf,x
                lda #18
                sta actsizey
                lda #10
                sta actshootymod
                rts
plr_noduck:     jsr plisskenmovex
                rts

plisskenmovex:  lda actsx,x
                bmi plr_moveleft
                lda actxh,x
                beq plr_moveok
                lda actxl,x
                cmp #(310-256)
                bcc plr_moveok
plr_nomove:     rts
plr_moveok:     jsr moveactorx
                rts
plr_moveleft:   lda actxh,x
                bne plr_moveok
                lda actxl,x
                cmp #11
                bcs plr_moveok
                rts

man:            lda acthp,x
                bne man_notdead
                lda actf,x
                cmp #3
                beq man_nodinit
                jsr addenemyscore
                lda #3
                sta actf,x
                lda #-5
                sta actsy,x
                lda #0
                sta actyd,x
man_nodinit:    inc actyd,x
                lda actyd,x
                cmp #2
                bcc man_d2
                lda #$00
                sta actyd,x
                inc actsy,x
man_d2:         jsr moveactory
                lda acty,x
                cmp #240
                bcc man_noremove
                lda #0
                sta actt,x
man_noremove:   rts
man_notdead:    lda actj,x
                beq man_nofly
man_fly:        lda #2
                sta actf,x
                inc actyd,x
                lda actyd,x
                cmp #5
                bcc man_fly2
                lda #$00
                sta actyd,x
                inc actsy,x
man_fly2:       jsr moveactorx
                jsr moveactory
                lda actsy,x
                beq man_noground
                bmi man_noground
                jsr checkground
                bcs man_noground
                lda #0
                sta actsy,x
                sta actj,x
                lda acty,x
                and #$f8
                sta acty,x
man_noground:   jmp man_shoot
                rts
man_nofly:      jsr checkground
                bcc man_nojump
                lda #1
                sta actj,x
                lda #-4
                sta actsy,x
                sta actyd,x
                jmp man_fly
man_nojump:     lda actd,x
                beq man_right
                ldy actt,x
                dey
                lda actmaxhp,y
                eor #$ff
                clc
                adc #$01
                sta actsx,x
                jmp man_walkanim
man_right:      ldy actt,x
                dey
                lda actmaxhp,y
                sta actsx,x
man_walkanim:   inc actfd,x
                lda actfd,x
                cmp #6
                bcc man_walkanim2
                lda #0
                sta actfd,x
                lda actf,x
                eor #$01
                and #$01
                sta actf,x
man_walkanim2:  jsr moveactorx
man_shoot:      jsr random
                sta temp1
                lda level
                cmp temp1
                bcc man_noshoot
                ldy #8
manshootfind:   lda bullett,y
                beq manshootfound
                iny
                cpy #$10
                bcc manshootfind
man_noshoot:    rts
manshootfound:  lda actxl,x
                sta bulletxl,y
                lda actxh,x
                sta bulletxh,y
                lda acty,x
                sec
                sbc #20
                sta bullety,y
                lda actd,x
                sta bulletd,y
                lda #1
                sta bullett,y
                rts

mc:             lda acthp,x
                bne mc_notdead
                jsr addenemyscore
                lda #0
                sta actf,x
                lda #ACT_EXPLOSION
                sta actt,x
                rts
mc_notdead:     lda actj,x
                beq mc_nofly
mc_fly:         lda #0
                sta actf,x
                inc actyd,x
                lda actyd,x
                cmp #5
                bcc mc_fly2
                lda #$00
                sta actyd,x
                inc actsy,x
mc_fly2:        jsr moveactorx
                jsr moveactory
                lda actsy,x
                beq mc_noground
                bmi mc_noground
                jsr checkground
                bcs mc_noground
                lda #0
                sta actsy,x
                sta actj,x
                lda acty,x
                and #$f8
                sta acty,x
mc_noground:    rts
mc_nofly:       jsr checkground
                bcc mc_nojump
                lda #1
                sta actj,x
                lda #-3
                sta actsy,x
                sta actyd,x
                jmp mc_fly
mc_nojump:      lda actd,x
                beq mc_right
                lda #-4
                sta actsx,x
                jmp mc_walkanim
mc_right:       lda #4
                sta actsx,x
mc_walkanim:    lda actf,x
                eor #$01
                and #$01
                sta actf,x
                jsr moveactorx
                ldy #0
                jsr actactcoll
                bcc mc_nocoll
                lda acthp,y
                sec
                sbc #$01
                sta acthp,y
mc_nocoll:      rts

expl:           inc actfd,x
                lda actfd,x
                cmp #6
                bcc expl2
                lda #0
                sta actfd,x
                inc actf,x
                lda actf,x
                cmp #4
                bcc expl2
                lda #0
                sta actt,x
expl2:          rts


addenemyscore:  ldy actt,x
                dey
                sed
                lda score
                clc
                adc actscorelo,y
                sta score
                lda score+1
                adc actscorehi,y
                sta score+1
                bcc noextra2
                inc lives
noextra2:       lda score+2
                adc #$00
                sta score+2
                cld
                lda killactive
                beq addenemyscore2
                inc killmeterd
                ldy killmeter
                lda #100
                clc
                adc killmeterd
                sta $400+46,y
                lda killmeterd
                cmp #4
                bcc addenemyscore2
                lda #$00
                sta killmeterd
                inc killmeter
addenemyscore2: rts

moveactorx:     clc
                lda actsx,x
                bmi negmovex
                adc actxl,x
                sta actxl,x
                lda actxh,x
                adc #$00
                sta actxh,x
                rts
negmovex:       adc actxl,x
                sta actxl,x
                lda actxh,x
                adc #$ff
                sta actxh,x
                rts

moveactory:     lda actsy,x
                clc
                adc acty,x
                sta acty,x
                rts

checkground:    lda acty,x
                lsr
                lsr
                lsr
                cmp #24
                bcc cg_notover1
                lda #23
cg_notover1:    tay
                lda rowtbllo,y
                sta temp1
                lda rowtblhi,y
                sta temp2
                lda actxh,x
                sta temp3
                lda actxl,x
                lsr temp3
                ror
                lsr temp3
                ror
                lsr temp3
                ror
                tay
                bpl cg_notoverleft
                ldy #0
                jmp cg_notoverright
cg_notoverleft: cpy #39
                bcc cg_notoverright
                ldy #39
cg_notoverright:lda (temp1),y
                cmp #32
                rts
rowtbllo:
N               SET 0
                REPEAT 24
                dc.b #<($400+N*40)
N               SET N+1
                REPEND
rowtblhi:
N               SET 0
                REPEAT 24
                dc.b #>($400+N*40)
N               SET N+1
                REPEND

random:         lda $b00
                adc randseed
                adc $d012
                sta randseed
                inc random+1
                rts
randseed:       dc.b $73

        ;X = bullet number (bullet must exist!)
        ;Y = actor number
        ;C=1 collision happened
bullactcoll:    lda actt,y
                beq bsc_nocoll
                lda actimm,y
                bne bsc_nocoll
                lda acthp,y
                bne bsc1
bsc_nocoll:     clc
                rts
bsc1:           sty temp1
                lda actt,y
                tay
                dey
                lda actsizex,y
                sta bsc_xcmp+1
                lda actsizey,y
                sta bsc_suby+1
                ldy temp1
                lda acty,y              ;Check against bottom of actor
                cmp bullety,x
                bcc bsc_nocoll
                sec
bsc_suby:       sbc #$00                ;Check against top of actor
                cmp bullety,x
                bcs bsc_nocoll
                lda actxl,y             ;Get X distance between bullet&actor
                sec
                sbc bulletxl,x
                sta temp1
                lda actxh,y
                sbc bulletxh,x
                sta temp2
                bpl bsc_posofs
                eor #$ff
                sta temp2
                lda temp1
                eor #$ff
                clc
                adc #$01
                sta temp1
                lda temp2
                adc #$00
                sta temp2
bsc_posofs:     lda temp2                       ;X distance must not be
                bne bsc_nocoll                  ;greater than X size
                lda temp1
bsc_xcmp:       cmp #$00
                bcs bsc_nocoll
                sec
                rts

        ;X = actor number (must exist)
        ;Y = actor number
        ;C=1 collision happened
actactcoll:     lda actt,y
                beq ssc_nocoll
                lda actimm,y
                bne ssc_nocoll
                lda acthp,y
                bne ssc1
ssc_nocoll:     clc
                rts
ssc1:           sty temp1
                stx temp2
                lda actt,y
                tay
                dey
                lda actt,x
                tax
                dex
                lda actsizex,y
                clc
                adc actsizex,x
                sta ssc_xcmp+1
                lda actsizey,y
                sta ssc_ycmp+1
                ldy temp1
                ldx temp2
                lda acty,y              ;Check against bottom of actor
                sec
                sbc acty,x
                bpl ssc_ypos
                eor #$ff
                clc
                adc #$01
ssc_ypos:
ssc_ycmp:       cmp #$00
                bcs ssc_nocoll
                lda actxl,y             ;Get X distance between actors
                sec
                sbc actxl,x
                sta temp1
                lda actxh,y
                sbc actxh,x
                sta temp2
                bpl ssc_posofs
                eor #$ff
                sta temp2
                lda temp1
                eor #$ff
                clc
                adc #$01
                sta temp1
                lda temp2
                adc #$00
                sta temp2
ssc_posofs:     lda temp2                       ;X distance must not be
                bne ssc_nocoll                  ;greater than X size
                lda temp1
ssc_xcmp:       cmp #$00
                bcs ssc_nocoll
                sec
                rts

spawnenemies:   lda level
                sec
                sbc #$01
                asl
                asl
                asl
                asl
                sta spawnadd+1
                jsr random
                and #$3f
                cmp #$10
                bcc spawnok1
                rts
spawnok1:       clc
spawnadd:       adc #$00
                tax
                lda levelspawntable,x
                bne spawnok2
                rts
spawnok2:       ldx #1
spawnsearch:    ldy actt,x
                beq spawnfound
                inx
                cpx #7
                bcc spawnsearch
                rts
spawnfound:     sta actt,x
                jsr random
                and #$01
                tay
                sta actd,x
                lda spawnxlo,y
                sta actxl,x
                lda spawnxhi,y
                sta actxh,x
                jsr random
                and #$03
                clc
                adc #$02
                asl
                asl
                asl
                asl
                asl
                sta acty,x
                jsr checkground
                bcc spawnground
                lda #0
                sta actt,x
                rts
spawnground:    lda #0
                sta actj,x
                sta actsx,x
                sta actsy,x
                sta actf,x
                sta actfd,x
                sta actyd,x
                sta actimm,x
                ldy actt,x
                dey
                lda actmaxhp,y
                sta acthp,x
                rts

                ;level1
levelspawntable:dc.b 0,2,0,2,0,3,0,2,0,2,0,3,0,2,0,2
                ;level2
                dc.b 0,2,2,3,0,2,3,4,0,3,4,0,2,3,2,5
                ;level3
                dc.b 2,2,3,3,2,3,4,4,3,3,4,4,3,2,5,5
spawnxlo:       dc.b 0,<320
spawnxhi:       dc.b 0,>320

checkscroll:    ldx #$00
                lda actxh
                cmp #1
                bcs needscroll
                lda actxl
                cmp #200
                bcc noneedscroll
needscroll:     ldx #$02
noneedscroll:   txa
                rts

drawbullets:    ldx #15
dbloop:         lda bullett,x
                beq dbnext
                lda bullety,x
                lsr
                lsr
                lsr
                cmp #23
                bcs dbnext
                tay
                lda rowtbllo,y
                sta temp1
                lda rowtblhi,y
                sta temp2
                lda bulletxh,x
                sta temp3
                lda bulletxl,x
                lsr temp3
                ror
                lsr temp3
                ror
                lsr temp3
                ror
                cmp #40
                bcs dbnext
                clc
                adc temp1
                sta temp1
                sta bulletlo,x
                lda temp2
                adc #$00
                sta temp2
                sta bullethi,x
                ldy #0
                lda (temp1),y
                sta bulletunder,x
                lda #255
                sta (temp1),y
                lda #2
                sta bullett,x
dbnext:         dex
                bpl dbloop
                rts

erasebullets:   ldx #0
ebloop:         lda bullett,x
                cmp #2
                bne ebnext
                ldy #0
                lda bulletlo,x
                sta temp1
                lda bullethi,x
                sta temp2
                lda bulletunder,x
                sta (temp1),y
                lda #1
                sta bullett,x
ebnext:         inx
                cpx #16
                bcc ebloop
                rts

movebullets:    ldx #15
mbloop:         lda bullett,x
                beq mbnext

                cpx #8
                bcs checkenemybull
                ldy #1
checkplayerbull:
                jsr bullactcoll
                bcc cpb_nocoll
                lda acthp,y
                sec
                sbc #$01
                sta acthp,y
                lda #$00
                sta bullett,x
                jmp mbnext
cpb_nocoll:     iny
                cpy #8
                bcc checkplayerbull
                jmp bullcheckdone

checkenemybull: ldy #0
                jsr bullactcoll
                bcc bullcheckdone
                lda acthp,y
                sec
                sbc #$01
                sta acthp,y
                lda #$00
                sta bullett,x
                jmp mbnext

bullcheckdone:  lda bulletd,x
                bne mbleft
mbright:        lda bulletxl,x
                clc
                adc #8
                sta bulletxl,x
                lda bulletxh,x
                adc #0
                sta bulletxh,x
                beq mbnext
                lda bulletxl,x
                cmp #(320-256)
                bcc mbnext
mberase:        lda #0
                sta bullett,x
                jmp mbnext
mbleft:         lda bulletxl,x
                sec
                sbc #8
                sta bulletxl,x
                lda bulletxh,x
                sbc #0
                sta bulletxh,x
                bmi mberase
mbnext:         dex
                bpl mbloop
                rts



moveactors:     ldx #7
mactloop:       lda actt,x
                beq mactnext
                lda actimm,x
                beq mact_noimm
                dec actimm,x
mact_noimm:     cpx #0
                beq mact_noremove
                lda actxh,x
                beq mact_noremove
                bmi mact_rleft
mact_rright:    lda actxl,x
                cmp #<(330)
                bcc mact_noremove
                lda #0
                sta actt,x
                jmp mactnext
mact_rleft:     lda actxl,x
                cmp #(256-10)
                bcs mact_noremove
                lda #0
                sta actt,x
                jmp mactnext
mact_noremove:  stx mact_restx+1
                lda actt,x
                tay
                dey
                lda actroutlo,y
                sta mactjsr+1
                lda actrouthi,y
                sta mactjsr+2
mactjsr:        jsr $0000
mact_restx:     ldx #$00
mactnext:       dex
                bpl mactloop
                rts

drawactors:     lda #$00
                sta virtd010
                sta virtd015
                sta virtd017
                sta virtd01d
                ldx #7
dactloop:       lda actt,x
                bne dactok
                jmp dactnext
dactok:         tay
                dey
                lda actimm,x
                and #$08
                beq dact_noflash
                jmp dactnext
dact_noflash:   lda virtd015
                ora bittable,x
                sta virtd015
                lda actxl,x
                sec
                sbc acthotx,y
                sta temp1
                lda actxh,x
                sbc #$00
                sta temp2
                lda acty,x
                sec
                sbc acthoty,y
                sta temp3
                lda temp3
                clc
                adc #50
                sta temp3
                lda temp1
                clc
                adc #24
                sta temp1
                lda temp2
                adc #0
                sta temp2
                lda actd,x
                beq dact_right
dact_left:      lda actbaseframel,y
                jmp dact_frame
dact_right:     lda actbaseframer,y
dact_frame:     clc
                adc actf,x
                sta 2040,x
                lda actcolor,y
                sta $d027,x
                lda actmagx,y
                beq dact_nomagx
                lda virtd01d
                ora bittable,x
                sta virtd01d
dact_nomagx:    lda actmagy,y
                beq dact_nomagy
                lda virtd017
                ora bittable,x
                sta virtd017
dact_nomagy:    txa
                asl
                tay
                lda temp1
                sta $d000,y
                lda temp3
                sta $d001,y
                lda temp2
                beq dactnext
                lda virtd010
                ora bittable,x
                sta virtd010
dactnext:       dex
                bmi dactdone
                jmp dactloop
dactdone:       lda virtd010
                sta $d010
                lda virtd015
                sta $d015
                lda virtd017
                sta $d017
                lda virtd01d
                sta $d01d
                rts

bittable:       dc.b 1,2,4,8,16,32,64,128
virtd010:       dc.b 0
virtd015:       dc.b 0
virtd017:       dc.b 0
virtd01d:       dc.b 0

waitras:        lda rastercount
                cmp #$01
                bcc waitras
                lda #$00
                sta rastercount
                rts

initscroll:
                ldx #39
iscr1:
N               SET 0
                REPEAT 24
                lda #$20
                sta $400+N*40,x
                lda #$09
                sta $d800+N*40,x
N               SET N+1
                REPEND
                dex
                bmi iscrdone1
                jmp iscr1
iscrdone1:      lda #$00
                sta mapx
                sta blockx
                lda #$07
                sta scrollx
                lda level
                sec
                sbc #$01
                asl
                tax
                lda levelmaptbl,x
                sta mapadrlo
                lda levelmaptbl+1,x
                sta mapadrhi
                lda #DISPGAME
                sta dispmode
                rts
levelmaptbl:    dc.w MAP, MAP+500, MAP+1000

doscroll:       ldx mapx
                cpx #100
                bcc doscrollok
                rts
doscrollok:     sta scrsub+1
                sta sprsub+1
                ldx #7
doscrollspr:    lda actxl,x
                sec
sprsub:         sbc #$00
                sta actxl,x
                lda actxh,x
                sbc #$00
                sta actxh,x
                dex
                bpl doscrollspr
                lda scrollx
                sec
scrsub:         sbc #$00
                bmi scrshift
                sta scrollx
                rts
scrshift:       and #$07
                sta scrollx
                ldx #$00
scrshiftloop1:
N               SET 4
                REPEAT 10
                lda $400+N*40+1,x
                sta $400+N*40,x
N               SET N+1
                REPEND
                inx
                cpx #39
                bne scrshiftloop1
                ldx #$00
scrshiftloop2:
N               SET 14
                REPEAT 10
                lda $400+N*40+1,x
                sta $400+N*40,x
N               SET N+1
                REPEND
                inx
                cpx #39
                bne scrshiftloop2
                lda mapadrlo
                clc
                adc mapx
                sta temp1
                lda mapadrhi
                adc #$00
                sta temp2
                lda #<($400+4*40+39)
                sta temp3
                lda #>($400+4*40+39)
                sta temp4
                lda #20
                sta temp5
                ldy #$00
scrblockloop:   ldx blockx
                lda (temp1),y
                tay
                lda blocktbllo,y
                sta scrblockget+1
                lda blocktblhi,y
                sta scrblockget+2
                lda temp1
                clc
                adc #100
                sta temp1
                lda temp2
                adc #$00
                sta temp2
                ldy #$00
scrblockget:    lda $1000,x
                sta (temp3),y
                lda temp3
                clc
                adc #40
                sta temp3
                lda temp4
                adc #0
                sta temp4
                txa
                adc #4
                tax
                dec temp5
                beq scrblockready
                cpx #$10
                bcc scrblockget
                jmp scrblockloop
scrblockready:  inc blockx
                lda blockx
                cmp #$04
                bcc scrblockready2
                lda #$00
                sta blockx
                inc mapx
scrblockready2: rts

blocktbllo:
N               SET 0
                REPEAT 128
                dc.b #<(BLOCKS+N*16)
N               SET N+1
                REPEND
blocktblhi:
N               SET 0
                REPEAT 128
                dc.b #>(BLOCKS+N*16)
N               SET N+1
                REPEND

getjoystick:    lda #$ff
                sta $dc00
                lda joystick
                sta prevjoy
                lda $dc00
                eor #$ff
                sta joystick
                rts

showpic:        ldx #$00
showpicloop:    lda $8000,x
                sta $5c00,x
                lda $8100,x
                sta $5d00,x
                lda $8200,x
                sta $5e00,x
                lda $8400,x
                sta $d800,x
                lda $8500,x
                sta $d900,x
                lda $8600,x
                sta $da00,x
                inx
                bne showpicloop
showpic2:       lda $8300,x
                sta $5f00,x
                lda $8700,x
                sta $db00,x
                inx
                cpx #192
                bne showpic2
                lda #DISPTITLE
                sta dispmode
                rts

initscreen:     lda #$00
                sta $d020
                sta $d021
                lda #$0e
                sta $d022
                lda #$06
                sta $d023
                lda #$ff
                sta $d01c
                lda #$00
                sta $d01b
                lda #$0a
                sta $d025
                lda #$00
                sta $d026
                ldx #39
ip_loop:        lda paneltext,x
                and #$3f
                sta $400+24*40,x
                lda #$07
                sta $d800+24*40,x
                dex
                bpl ip_loop
                rts
drawscores:     ldy #$02
                ldx #$02
ds1:            lda score,x
                lsr
                lsr
                lsr
                lsr
                clc
                adc #48
                sta $400+24*40,y
                iny
                lda score,x
                and #$0f
                clc
                adc #48
                sta $400+24*40,y
                iny
                dex
                bpl ds1
                ldx #$02
                ldy #34
ds2:            lda hiscore,x
                lsr
                lsr
                lsr
                lsr
                clc
                adc #48
                sta $400+24*40,y
                iny
                lda hiscore,x
                and #$0f
                clc
                adc #48
                sta $400+24*40,y
                iny
                dex
                bpl ds2
                lda lives
                clc
                adc #48
                sta $400+24*40+14
                lda time
                lsr
                lsr
                lsr
                lsr
                clc
                adc #48
                sta $400+24*40+20
                lda time
                and #$0f
                clc
                adc #48
                sta $400+24*40+21
                lda level
                clc
                adc #48
                sta $400+24*40+28
                rts
paneltext:      dc.b "SC         MEN    TI     LEV    HI      "




initraster:     sei
                lda #<raster0                   ;Set main IRQ vector
                sta $0314
                lda #>raster0
                sta $0315
                lda #$7f                        ;Set timer interrupt off
                sta $dc0d
                lda #$01                        ;Set raster interrupt on
                sta $d01a
                lda $d011
                and #$7f
                sta $d011
                lda #RASTER0POS                 ;Set low bits of position
                sta $d012                       ;for first raster interrupt
                lda $dc0d                       ;Acknowledge timer interrupt
                cli                             ;(for safety)
                rts

raster0:        cld
                lda #27
                sta $d011
                lda #$03
                sta $dd00
                lda #21
                sta $d018
                lda #8
                sta $d016
                inc $d019
                jsr MUSIC+3
                lda #<raster1
                sta $0314
                lda #>raster1
                sta $0315
                lda #RASTER1POS
                sta $d012
                inc rastercount
                jmp $ea81

raster1:        cld
                lda dispmode
                beq r1_gamemode
r1_titlemode:   cmp #DISPTEXT
                bne r1_pic
                lda #3
                sta $dd00
                lda #27
                sta $d011
                lda #$18
                sta $d016
                lda #21
                sta $d018
                jmp r1_end
r1_pic:         lda #2
                sta $dd00
                lda #59
                sta $d011
                lda #24
                sta $d016
                lda #$78
                sta $d018
                lda #$00
                sta $d015
                jmp r1_end
r1_gamemode:    lda #$03
                sta $dd00
                lda #27
                sta $d011
                lda #30
                sta $d018
                lda scrollx
                and #$07
                ora #$10
                sta $d016
r1_end:         inc $d019
                lda #<raster0
                sta $0314
                lda #>raster0
                sta $0315
                lda #RASTER0POS
                sta $d012
                jmp $ea81

score:          dc.b 0,0,0
hiscore:        dc.b 0,0,0
lives:          dc.b 3
level:          dc.b 1
time:           dc.b $99
timedl:         dc.b 0
firedelay:      dc.b 0
killactive:     dc.b 0
killmeter:      dc.b 0
killmeterd:     dc.b 0
killlimit:      dc.b 0

actxl:          ds.b 8,0
actxh:          ds.b 8,0
acty:           ds.b 8,0
actf:           ds.b 8,0
actfd:          ds.b 8,0
actd:           ds.b 8,0
actsx:          ds.b 8,0
actsy:          ds.b 8,0
actyd:          ds.b 8,0
actj:           ds.b 8,0
actt:           ds.b 8,0
actimm:         ds.b 8,0
acthp:          ds.b 8,0

acthotx:        dc.b 12,12,12,12,24,24
acthoty:        dc.b 40,40,40,40,40,40
actmagx:        dc.b 0,0,0,0,1,1
actmagy:        dc.b 1,1,1,1,1,1
actsizex:       dc.b 12,12,12,12,30,48
actsizey:       dc.b 42,42,42,42,32,42
actcolor:       dc.b 11,9,2,4,12,7
actbaseframer:  dc.b 128,144,144,144,152,156
actbaseframel:  dc.b 136,148,148,148,154,156
actroutlo:      dc.b <plr,<man,<man,<man,<mc,<expl
actrouthi:      dc.b >plr,>man,>man,>man,>mc,>expl
actshootymod:   dc.b 20,20,20,20,20,0
actmaxhp:       dc.b 1,1,2,3,5,0
actscorelo:     dc.b $00,$50,$50,$00,$00,$00
actscorehi:     dc.b $00,$02,$04,$06,$10,$00

bulletxl:       ds.b 16,0
bulletxh:       ds.b 16,0
bullety:        ds.b 16,0
bulletd:        ds.b 16,0
bullett:        ds.b 16,0
bulletlo:       ds.b 16,0
bullethi:       ds.b 16,0
bulletunder:     ds.b 16,0



                org SPRITES
                incbin efny.spr

                org CHARS-$100
                incbin efny.chr

                org MUSIC
                incbin music.bin

                org MAP-2

                incbin efny.map

                org BLOCKS-$80
blocks:         incbin efny.blk

                org PICTURE
                incbin plissken.pic
