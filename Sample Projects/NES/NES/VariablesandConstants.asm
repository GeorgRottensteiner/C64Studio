* = $0000

;for the chr rom data
tile_loader_ptr
          !word ?

;main program flag
sleeping
          !byte ?

;0 = nothing, 1 = main program is updating the room
updating_background
          !word ?

;Animation Counters
Enemy_Animation
          !dword ?

;Animation Frame Number
Enemy_Frame
          !dword ?

;random direction counter
random_direction1
          !byte ?

;random direction counter
random_direction2
          !byte ?

;direction for enemys; 0=up,1=down,2=right,3=left
enemy_direction
          !dword ?

;pointer for the graphics data for Enemys
enemy_pointer
          !dword ?
          !dword ?

;pointer for the graphics updates
enemygraphicspointer
          !word ?

;enemy number for direction routine 0=Crewman, 1=punisher, 2=McBoobins, 3=ArseFace
enemy_number
          !byte ?

;enemy pointer number (i.e. 2x the enemy number, 0=Crewman, 2=punisher, 4=McBoobins, 6=ArseFace
enemy_ptrnumber
          !byte ?


;enemy's counter resets
enemyFrames1  = $0C
enemyFrames2  = $18
enemyFrames3  = $24
enemyFrames4  = $30

;constant for sprite updates
sprite_RAM    = $0200

enemy_speed   = $01