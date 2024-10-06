;;;;;;;;;;;;  VARIABLES------------

* = $0000  ;start from location 0

tile_loader_ptr  !word ?        ;for the chr rom data

sleeping  !byte ?           ;main program flag
updating_background  !word ?    ;0 = nothing, 1 = main program is updating the room

Enemy_Animation  !dword ?        ;Animation Counters
Enemy_Frame  !dword ?            ;Animation Frame Number

random_direction1  !byte ?      ;random direction counter
random_direction2  !byte ?      ;random direction counter

enemy_direction  !dword ?        ;direction for enemys; 0=up,1=down,2=right,3=left

enemy_pointer  !dword ?        ;pointer for the graphics data for Enemys
!dword ?
enemygraphicspointer  !word ?  ;pointer for the graphics updates

enemy_number  !byte ?           ;enemy number for direction routine 0=Crewman, 1=punisher, 2=McBoobins, 3=ArseFace
enemy_ptrnumber  !byte ?        ;enemy pointer number (i.e. 2x the enemy number, 0=Crewman, 2=punisher, 4=McBoobins, 6=ArseFace

;;;;;;;;;;;;;;;;;;  CONSTANTS----------

enemyFrames1 = $0C  ;enemy's counter resets
enemyFrames2 = $18
enemyFrames3 = $24
enemyFrames4 = $30

sprite_RAM = $0200   ;constant for sprite updates

enemy_speed = $01