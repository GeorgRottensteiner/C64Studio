!MACRO jmp_init_alert text_id, count, sfx      ; setup a hud alert macro 
      ldx #text_id 
      ldy #count 
      lda #sfx 
      jmp hud_alert_init 
      !END
      
      
* = $0801

!basic

@end    +jmp_init_alert 1, 2, 3
        rts
        
hud_alert_init
        rts