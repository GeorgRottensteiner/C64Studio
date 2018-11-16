10 PRINT"";
11 POKE53280,0:POKE53281,0
20 PRINT"            MERRY CHRISTMAS             ";
21 PRINT"                                        ";
22 PRINT"                                        ";
23 PRINT"                                        ";
24 PRINT"                                        ";
25 PRINT"                                        ";
26 PRINT"                                        ";
27 PRINT"                                        ";
28 PRINT"                                        ";
29 PRINT"                                        ";
30 PRINT"                                        ";
31 PRINT"                                        ";
32 PRINT"                                        ";
33 PRINT"                                        ";
34 PRINT"                                        ";
35 PRINT"                                        ";
36 PRINT"                                        ";
37 PRINT"                                        ";
38 PRINT"                                        ";
39 PRINT"                                        ";
40 PRINT"                                        ";
41 PRINT"                                        ";
42 PRINT"                                        ";
43 PRINT"                                        ";
50GETA$:IFA$<>""THENEND
52X=INT(RND(1)*1000)
53O=PEEK(1024+X):C=PEEK(55296+X)
54POKE1024+X,42:POKE55296+X,1:FORI=1TO100:NEXT:POKE55296+X,C:POKE1024+X,O:GOTO50