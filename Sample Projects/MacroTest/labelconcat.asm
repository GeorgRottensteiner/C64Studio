* = $1000

          !for xx = 0 to 5
          CHR_##xx = xx
          !end

yy = 2
CHR_##yy = yy

hurz = 3


label##hurz



a=1

msg="A=" + a ;msg="A=1/$1"

a="hello"
b="there"
msg=a + " " + b ;msg = "hello there"
!message msg

a = "the value is "
b = 1
msg = a + b ;msg = "the value is 1/$1"


!message a + b ;"the value is 1/$1"

!message "CHR_1=",CHR_1