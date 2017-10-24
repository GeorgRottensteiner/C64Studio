C64Ass - C64Studio Command Line assembler
=========================================
A very simple command line implementation of the C64Studio assembler for direct use.
It uses the same code but does not require any GUI.

The following arguments are supported. Note that the input file is always the last argument!

Call with C64Ass [options] [file]

-H, --HELP                 - Display this help
-O, --OUTFILE [Filename]   - Determine output file name
-L, --LABELDUMP [Filename] - Write a label file
-F, --FORMAT [PLAIN/CBM]   - Sets the output file format
                             PLAIN is a raw binary, CBM a .prg file
-AUTOTRUNCATELITERALS      - Clamps literal values to bytes/words without error