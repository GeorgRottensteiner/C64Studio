* = $0801

MODUL = 2
!basic
            lda #$02
            sta $0d02

            jsr wic64_dont_disable_irqs
            rts

            !pseudopc $2000

            DATA
            !byte 123
            !word wic64_timeout_handler_stackpointer

            !realpc

daten
!ifDEF MODUL {
    !pseudopc $C000 {
            wic64_data_section_start: ; EXPORT
            ;---------------------------------------------------------
            ; Globals
            ;---------------------------------------------------------
            wic64_timeout:              !byte $0f    ; EXPORT
            wic64_dont_disable_irqs:    !byte $00    ; EXPORT
            wic64_request_header_size:  !byte $04
            wic64_response_header_size: !byte $03
            wic64_request:           !word $0000     ; EXPORT
            wic64_response:          !word $0000     ; EXPORT
            wic64_transfer_size:     !word $0000     ; EXPORT
            wic64_bytes_to_transfer: !word $0000     ; EXPORT
            .response_header:
            wic64_status:               !byte $00           ; EXPORT
            wic64_response_size:        !word $0000, $0000  ; EXPORT
            ; these label should be local, but unfortunately acmes
            ; limited scoping requires these labels to be defined
            ; as global labels:
            wic64_configured_timeout: !byte $0f
            wic64_timeout_handler: !word $0000
            wic64_timeout_handler_stackpointer: !byte $00
            wic64_error_handler: !word $0000
            wic64_error_handler_stackpointer: !byte $00
            wic64_handlers_suspended: !byte $01
            wic64_counters: !byte $00, $00, $00, $00
            wic64_nop_instruction: !byte $ea, $ea, $ea
            wic64_auto_discard_response: !byte $01
            ;---------------------------------------------------------
            ; Locals
            ;---------------------------------------------------------
            .protocol: !byte $00
            .user_irq_flag: !byte $00
            .request_header: !fill 6, 0
    }
}


            stx wic64_timeout
