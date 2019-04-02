//Maya ASCII 2012 scene
//Name: brazier1.ma
//Last modified: Thu, Apr 18, 2013 10:53:59 AM
//Codeset: 1252
requires maya "2012";
requires "stereoCamera" "10.0";
currentUnit -l meter -a degree -t film;
fileInfo "application" "maya";
fileInfo "product" "Maya 2012";
fileInfo "version" "2012 x64";
fileInfo "cutIdentifier" "001200000000-796618";
fileInfo "osv" "Microsoft Windows 7 Enterprise Edition, 64-bit Windows 7 Service Pack 1 (Build 7601)\n";
createNode transform -n "polySurface94";
createNode mesh -n "polySurfaceShape96" -p "polySurface94";
	setAttr -k off ".v";
	setAttr ".iog[0].og[0].gcl" -type "componentList" 1 "f[0:84]";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 340 ".uvst[0].uvsp";
	setAttr ".uvst[0].uvsp[0:249]" -type "float2" 0.0019546449 -4.1723251e-007
		 1 0 1 1 0 1 0.1427038 -4.7683716e-007 1.14270377 -4.7683716e-007 1.14270377 0.99999952
		 0.14270383 0.99999952 0 0 1 0 1 1 -5.9604645e-008 0.9999997 0 0 1.0019547939 0.000626266
		 1 1 0 1 0 -4.7683716e-007 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0.66073447
		 0.0019541979 1.66073442 0.0019541979 1.66073442 1.0019541979 0.66073447 1.0019541979
		 0 0 1 0 1 1 -5.9604645e-008 0.9999997 0 0 1 0 1.0019545555 0.99999958 0 1 0 0 1 0
		 1.0019546747 0.99999958 0.0019545853 0.99999958 0 0 1 0 1 0.99999952 5.9604645e-008
		 0.99999952 -5.9604645e-008 -2.9802322e-007 0.99999994 -2.9802322e-007 1 1 0 1 0 0
		 1 0 0.99999994 0.9999997 -5.9604645e-008 0.9999997 0.0019547343 0.000626266 1 0 1
		 1 0.0019547343 1.00062632561 0.0019546449 -4.1723251e-007 1 0 1 1 0.0019546151 0.99999958
		 -0.058645546 0.0039090514 0.94135445 0.003909111 0.94135451 1.003909111 -0.058645546
		 1.003909111 -0.13292968 0.0058637857 0.86707032 0.0058637857 0.86707032 1.0058636665
		 -0.13292968 1.0058637857 0 0 1 0 1.54735196 0.99999958 0.54735196 0.99999958 0.0019547343
		 0.00062632561 1 0 1 1 0 1 0 0 1 0 1 0.99999952 0 1 -5.9604645e-008 -4.7683716e-007
		 1 0 1 1 5.9604645e-008 0.99999952 0.68028212 0.00079256296 1.68028212 0.00079256296
		 1.68028212 1.00079250336 0.68028212 1.00079250336 0.0019546449 -7.1525574e-007 1.0019546747
		 -7.1525574e-007 1.0019546747 0.99999928 0.0019546449 0.99999928 0.54735196 -4.1723251e-007
		 1.54735196 -4.1723251e-007 1 1 0 1 -0.35773563 -0.001955092 0.64226437 -0.0019551516
		 0.64226437 0.99804491 -0.3577356 0.99804491 0.29909176 0.00062638521 1.29909182 0.00062644482
		 1.29909182 1.00062644482 0.29909188 1.00062632561 0 0 1 0 1.0019547939 1.00062632561
		 0.0019547343 1.00062632561 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 -0.2658571
		 0.00062656403 0.7341429 0.00062656403 0.7341429 1.00062656403 -0.26585713 1.00062656403
		 -0.50825673 -0.0034594536 0.49174333 -0.0034594536 0.49174333 0.99654055 -0.50825667
		 0.99654055 -0.25217229 -0.0019552708 0.74782771 -0.0019552708 0.74782771 0.99804473
		 -0.25217223 0.99804473 0.0019546747 -4.7683716e-007 1.0019546747 -5.364418e-007 1.0019546747
		 0.99999946 0.0019546747 0.99999952 0.54735196 -4.1723251e-007 1.54735196 -4.1723251e-007
		 1 1 0 1 0 0 1 0 1.54735196 0.99999958 0.54735196 0.99999958 0 0 1 0 1 1 0 1 0.0019547045
		 -7.7486038e-007 1 0 1 1 0 1 -0.0039097667 -5.9604645e-007 1 0 1 1 0 1 0 0 1 0 0.99609023
		 0.9999994 0 1 0 0 1.0019546747 -7.1525574e-007 1.0019546747 0.99999928 0 1 0 0 1
		 0 0.58948404 0.99804503 -0.41051593 0.99804491 0 0 0.99609023 -5.9604645e-007 1 1
		 0 1 -0.41051596 -0.0019550323 1 0 1 1 -0.41051593 0.99804497 0 0 1 0 1.61381984 1.0037617683
		 0.61381996 1.0037617683 0 0 1.0019546747 -7.1525574e-007 1.0019546747 0.99999928
		 0 1 0.61381996 0.0037618279 1 0 1 1 0.61381996 1.0037617683 0 0 1 0 1 1 0 1 0 0 1
		 0 1 1 -0.0039097667 0.99999934 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0.0019547045
		 0.99999923 0 0 1 0 1 1 0 1 0 0 1 0 1 1 -0.0039097741 0.9999994 0 0 1 0 1 1 0 1 0
		 0 1 0 1 1 -0.0039097741 0.9999994 0 0 1 0 1 1 0 1 -5.9604645e-008 -2.9802322e-007
		 1 0 1 1 0 1 0.0058642477 -5.364418e-007 1 0 1 1 0 1 0 0 1 0 1.0058642626 0.99999952
		 0 1 0.34796304 0.00062638521 1 0 1 1 0.34796304 1.00062644482 0 0 1.34796286 0.00062644482;
	setAttr ".uvst[0].uvsp[250:339]" 1.34796309 1.00062644482 0 1 0 0 1 0 1 1 0.0058642477
		 0.99999952 0 0 1.34796298 0.00062644482 1 1 0 1 0 0 0.33731061 -0.0019548535 1 1
		 0 1 0 0 0.33731055 -0.0019547939 0.33731058 0.99804515 0 1 -0.66268945 -0.0019548535
		 0.33731061 -0.0019548535 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1.0058642626 -4.7683716e-007
		 1 1 0 1 0 0 1 0 1 1 0 1 0 0 0.99999988 -2.9802322e-007 1 1 0 1 0 0 0.99999994 -3.5762787e-007
		 1 1 0 1 0 0 0.99999988 -2.9802322e-007 1 1 0 1 0 0 1.0058642626 -5.364418e-007 1
		 1 0 1 0 0 1 0 1 1 0 1 0 0 1.0058642626 -5.364418e-007 1 1 0 1 0 0 1 0 1 1 0 1 0 0
		 1.0019546747 -2.9802322e-007 1.0019546747 0.99999964 0 1 0.0019547194 -4.1723251e-007
		 1 0 1 1 0.0019547269 0.99999958 0 0 1.0019546747 -4.1723251e-007 1.0019547939 0.99999958
		 0 1 0.0019546822 -3.5762787e-007 1 0 1 1 0.0019546747 0.99999964 0 0 1 0 1 1 0 1
		 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 94 ".vt[0:93]"  -0.58866209 -1.13966382 0.58792299 0.58718753 -1.13966382 0.58792299
		 -0.39317381 0.68524259 0.39244139 0.39169922 0.68524259 0.39244139 -0.39317381 0.68524259 -0.39244139
		 0.39169922 0.68524259 -0.39244139 -0.58866209 -1.13966382 -0.58792233 0.58718753 -1.13966382 -0.58792233
		 -0.14904296 0.68524259 0.39244139 -0.14904296 0.68524259 -0.39244139 -0.22291993 -1.13966382 -0.58792233
		 -0.22291993 -1.13966382 0.58792299 0.12740235 0.68524259 0.39244139 0.12740235 0.68524259 -0.39244139
		 0.19124024 -1.13966382 -0.58792233 0.19124024 -1.13966382 0.58792299 -0.17342773 0.082830049 0.45697021
		 -0.45770508 0.082830049 0.45697021 -0.45770508 0.082830049 -0.45697266 -0.17342773 0.082830049 -0.45697266
		 0.14847656 0.082830049 -0.45697266 0.45623046 0.082830049 -0.45697266 0.45623046 0.082830049 0.45697021
		 0.14847656 0.082830049 0.45697021 0.45623046 0.082830049 0.15375976 0.58718753 -1.13966382 0.19782105
		 -0.58866209 -1.13966382 0.19782105 -0.45770508 0.082830049 0.15375976 -0.39317381 0.68524259 0.13204528
		 -0.14904296 0.68524259 0.13204528 -0.17342773 0.082830049 0.15375976 0.14847656 0.082830049 0.15375976
		 0.12740235 0.68524259 0.13204528 0.39169922 0.68524259 0.13204528 0.45623046 0.082830049 -0.18112794
		 0.58718753 -1.13966382 -0.23303223 -0.58866209 -1.13966382 -0.23303223 -0.45770508 0.082830049 -0.18112794
		 -0.39317381 0.68524259 -0.15554932 -0.14904296 0.68524259 -0.15554932 -0.17342773 0.082830049 -0.18112794
		 0.14847656 0.082830049 -0.18112794 0.12740235 0.68524259 -0.15554932 0.39169922 0.68524259 -0.15554932
		 -0.39481446 0.75631988 0.39481691 0.39481446 0.75631988 0.39481691 -0.6715039 1.13966405 0.6715039
		 0.6715039 1.13966405 0.6715039 -0.6715039 1.13966405 -0.6715039 0.6715039 1.13966405 -0.6715039
		 -0.39481446 0.75631988 -0.39481691 0.39481446 0.75631988 -0.39481691 0.53133792 0.70688415 0
		 -0.53134763 0.70688415 0 0.58061522 0.8939786 -0.58061278 -0.58061522 0.8939786 -0.58061278
		 0.89708984 1.13966405 0 -0.89708984 1.13966405 0 0.58061522 0.8939786 0.58061218
		 -0.58061522 0.8939786 0.58061218 0 0.70688415 -0.53134519 0 1.13966405 -0.89709228
		 0 1.13966405 0.89708984 0 0.70688415 0.531344 -0.77469724 0.89182222 0 0.77469724 0.89182222 0
		 0 0.63707888 0 0 0.89182222 -0.77469724 0 0.89182222 0.77469724 -0.40760744 0.71800536 0.40759888
		 0 0.66772491 0.54750979 0 0.86458862 0.80714846 -0.60363281 0.86687011 0.60363281
		 -0.69580078 1.12169647 -0.69579834 0 1.12169647 -0.93345946 0 0.86458862 -0.8071509
		 -0.60363281 0.86687011 -0.60363525 -0.40760744 0.71800536 -0.4076001 0 0.66772491 -0.54751223
		 0 0.59471345 0 -0.54750979 0.66772491 0 0.54750979 0.66772491 0 0.40759766 0.71800536 0.40759888
		 0.80714846 0.86458862 0 0.60363281 0.86687011 0.60363281 -0.80714846 0.86458862 0
		 -0.93345702 1.12169647 0 -0.69580078 1.12169647 0.69579649 0.93345702 1.12169647 0
		 0.60363281 0.86687011 -0.60363525 0.69579101 1.12169647 -0.69579834 0.40759766 0.71800536 -0.4076001
		 0.69579101 1.12169647 0.69579649 0 1.12169647 0.93345886;
	setAttr -s 176 ".ed";
	setAttr ".ed[0:165]"  0 11 0 2 8 0 4 9 0 6 10 0 0 17 0 1 22 0 2 28 0 3 33 0
		 4 18 0 5 21 0 6 36 0 7 35 0 8 29 1 10 14 0 9 19 1 11 15 0 11 16 1 12 3 0 13 5 0 12 32 1
		 14 7 0 13 20 1 15 1 0 15 23 1 16 8 1 17 2 0 16 17 1 18 6 0 17 27 1 19 10 1 18 19 1
		 20 14 1 19 20 1 21 7 0 20 21 1 22 3 0 21 34 1 23 12 1 22 23 1 23 16 1 16 30 0 20 41 0
		 24 22 1 25 1 0 24 25 1 26 0 0 27 37 1 26 27 1 27 28 1 28 29 1 30 40 0 29 30 1 31 23 0
		 30 31 1 31 32 1 32 33 1 33 24 1 34 24 1 35 25 0 34 35 1 36 26 0 37 18 1 36 37 1 38 4 0
		 37 38 1 39 9 1 38 39 1 40 19 0 39 40 1 41 31 0 40 41 1 42 13 1 41 42 1 43 5 0 42 43 1
		 43 34 1 37 40 0 41 34 0 30 27 0 24 31 0 44 63 1 46 62 0 48 61 0 50 60 1 44 59 1 45 58 1
		 46 57 0 47 56 0 48 55 1 49 54 1 50 53 1 51 52 1 52 45 1 53 44 1 54 51 1 55 50 1 56 49 0
		 57 48 0 58 47 1 59 46 1 60 51 1 61 49 0 62 47 0 63 45 1 53 64 1 64 57 1 64 59 1 64 55 1
		 52 65 1 65 56 1 65 54 1 65 58 1 60 66 1 66 63 1 66 52 1 66 53 1 61 67 1 67 60 1 67 54 1
		 67 55 1 63 68 1 68 62 1 68 58 1 68 59 1 69 70 1 70 71 1 71 72 1 69 72 1 48 73 1 61 74 1
		 73 74 0 74 75 1 75 76 1 73 76 1 77 78 1 78 79 1 79 80 1 77 80 1 81 82 1 81 83 1 83 84 1
		 82 84 1 80 85 1 85 76 1 76 77 1 57 86 1 85 86 1 85 72 1 46 87 1 72 87 1 87 86 0 80 69 1
		 86 73 0 56 88 1 83 88 1 83 89 1 49 90 1 90 89 1 88 90 0 91 81 1 89 91 1 47 92 1 92 88 0
		 84 92 1 79 70 1 79 81 1;
	setAttr ".ed[166:175]" 70 82 1 78 91 1 75 78 1 75 89 1 74 90 0 62 93 1 71 93 1
		 71 84 1 93 92 0 87 93 0;
	setAttr -s 85 ".fc[0:84]" -type "polyFaces" 
		f 4 0 16 26 -5
		mu 0 4 0 1 2 3
		f 4 66 65 -3 -64
		mu 0 4 4 5 6 7
		f 4 30 29 -4 -28
		mu 0 4 8 9 10 11
		f 4 -12 -34 36 59
		mu 0 4 12 13 14 15
		f 4 10 62 61 27
		mu 0 4 16 17 18 19
		f 4 -30 32 31 -14
		mu 0 4 20 21 22 23
		f 4 39 -17 15 23
		mu 0 4 24 25 26 27
		f 4 74 73 -19 -72
		mu 0 4 28 29 30 31
		f 4 -32 34 33 -21
		mu 0 4 32 33 34 35
		f 4 -24 22 5 38
		mu 0 4 36 37 38 39
		f 4 -27 24 -2 -26
		mu 0 4 40 41 42 43
		f 4 -62 64 63 8
		mu 0 4 44 45 46 47
		f 4 2 14 -31 -9
		mu 0 4 48 49 50 51
		f 4 -35 -22 18 9
		mu 0 4 52 53 54 55
		f 4 75 -37 -10 -74
		mu 0 4 56 57 58 59
		f 4 -38 -39 35 -18
		mu 0 4 60 61 62 63
		f 4 68 67 -15 -66
		mu 0 4 64 65 66 67
		f 4 72 71 21 41
		mu 0 4 68 69 70 71
		f 4 70 -42 -33 -68
		mu 0 4 72 73 74 75
		f 4 -44 -45 42 -6
		mu 0 4 76 77 78 79
		f 4 -48 45 4 28
		mu 0 4 80 81 82 83
		f 4 -49 -29 25 6
		mu 0 4 84 85 86 87
		f 4 1 12 -50 -7
		mu 0 4 88 89 90 91
		f 4 -25 40 -52 -13
		mu 0 4 92 93 94 95
		f 4 -40 -53 -54 -41
		mu 0 4 96 97 98 99
		f 4 37 19 -55 52
		mu 0 4 100 101 102 103
		f 4 17 7 -56 -20
		mu 0 4 104 105 106 107
		f 4 -43 -57 -8 -36
		mu 0 4 108 109 110 111
		f 4 -59 -60 57 44
		mu 0 4 112 113 114 115
		f 4 -63 60 47 46
		mu 0 4 116 117 118 119
		f 4 53 -70 -71 -51
		mu 0 4 120 121 122 123
		f 4 -65 76 -69 -67
		mu 0 4 124 125 126 127
		f 4 -73 77 -76 -75
		mu 0 4 128 129 130 131
		f 4 48 49 51 78
		mu 0 4 132 133 134 135
		f 4 54 55 56 79
		mu 0 4 136 137 138 139
		f 4 -47 -79 50 -77
		mu 0 4 140 141 142 143
		f 4 69 -80 -58 -78
		mu 0 4 144 145 146 147
		f 4 124 125 126 -128
		mu 0 4 148 149 150 151
		f 4 130 131 132 -134
		mu 0 4 152 153 154 155
		f 4 134 135 136 -138
		mu 0 4 156 157 158 159
		f 4 -139 139 140 -142
		mu 0 4 160 161 162 163
		f 4 137 142 143 144
		mu 0 4 164 165 166 167
		f 4 -147 147 149 150
		mu 0 4 168 169 170 171
		f 4 -148 -143 151 127
		mu 0 4 172 173 174 175
		f 4 -144 146 152 133
		mu 0 4 176 177 178 179
		f 4 -155 155 -158 -159
		mu 0 4 180 181 182 183
		f 4 -156 -140 -160 -161
		mu 0 4 184 185 186 187
		f 4 -141 154 -163 -164
		mu 0 4 188 189 190 191
		f 4 -165 165 138 -167
		mu 0 4 192 193 194 195
		f 4 -166 -136 167 159
		mu 0 4 196 197 198 199
		f 4 -137 164 -125 -152
		mu 0 4 200 201 202 203
		f 4 -169 169 160 -168
		mu 0 4 204 205 206 207
		f 4 -170 -132 170 157
		mu 0 4 208 209 210 211
		f 4 -133 168 -135 -145
		mu 0 4 212 213 214 215
		f 4 -173 173 163 -175
		mu 0 4 216 217 218 219
		f 4 -174 -126 166 141
		mu 0 4 220 221 222 223
		f 4 -127 172 -176 -150
		mu 0 4 224 225 226 227
		f 4 84 -124 -121 -81
		mu 0 4 228 229 230 231
		f 4 88 -120 -117 -83
		mu 0 4 232 233 234 235
		f 4 90 -116 -113 -84
		mu 0 4 236 237 238 239
		f 4 85 -112 -109 92
		mu 0 4 240 241 242 243
		f 4 -96 -108 -105 -91
		mu 0 4 244 245 246 247
		f 4 -87 -100 -107 105
		mu 0 4 248 249 250 251
		f 4 -85 -94 104 106
		mu 0 4 252 253 254 255
		f 4 -89 -98 -106 107
		mu 0 4 256 257 258 259
		f 4 96 89 -111 109
		mu 0 4 260 261 262 263
		f 4 94 91 108 110
		mu 0 4 264 265 266 267
		f 4 98 87 -110 111
		mu 0 4 268 269 270 271
		f 4 103 -93 -115 113
		mu 0 4 272 273 274 275
		f 4 -92 -101 112 114
		mu 0 4 276 277 278 279
		f 4 93 80 -114 115
		mu 0 4 280 281 282 283
		f 4 100 -95 -119 117
		mu 0 4 284 285 286 287
		f 4 -90 -102 116 118
		mu 0 4 288 289 290 291
		f 4 95 83 -118 119
		mu 0 4 292 293 294 295
		f 4 102 -99 -123 121
		mu 0 4 296 297 298 299
		f 4 -86 -104 120 122
		mu 0 4 300 301 302 303
		f 4 99 81 -122 123
		mu 0 4 304 305 306 307
		f 4 82 129 -131 -129
		mu 0 4 308 309 310 311
		f 4 86 145 -151 -149
		mu 0 4 312 313 314 315
		f 4 97 128 -153 -146
		mu 0 4 316 317 318 319
		f 4 -97 153 158 -157
		mu 0 4 320 321 322 323
		f 4 -88 161 162 -154
		mu 0 4 324 325 326 327
		f 4 101 156 -171 -130
		mu 0 4 328 329 330 331
		f 4 -103 171 174 -162
		mu 0 4 332 333 334 335
		f 4 -82 148 175 -172
		mu 0 4 336 337 338 339;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".ndt" 0;
createNode groupId -n "groupId184";
	setAttr ".ihi" 0;
createNode shadingEngine -n "oz_ec_catacombs_straight_a:lambert2SG";
	setAttr ".ihi" 0;
	setAttr -s 11 ".dsm";
	setAttr ".ro" yes;
	setAttr -s 11 ".gn";
createNode materialInfo -n "oz_ec_catacombs_straight_a:materialInfo1";
createNode lambert -n "oz_ec_catacombs_straight_a:block";
createNode file -n "oz_ec_catacombs_straight_a:file1";
	setAttr ".ftn" -type "string" "C:/Users/whitj304/TempleRunOzNew/Assets/Oz/Textures/GameTextures/EmeraldCity/Blockout.tga";
createNode place2dTexture -n "oz_ec_catacombs_straight_a:place2dTexture1";
createNode lightLinker -s -n "lightLinker1";
	setAttr -s 6 ".lnk";
	setAttr -s 6 ".slnk";
select -ne :time1;
	setAttr ".o" 0.8;
	setAttr ".unw" 0.8;
select -ne :renderPartition;
	setAttr -s 6 ".st";
select -ne :initialShadingGroup;
	setAttr ".ro" yes;
select -ne :initialParticleSE;
	setAttr ".ro" yes;
select -ne :defaultShaderList1;
	setAttr -s 6 ".s";
select -ne :defaultTextureList1;
	setAttr -s 4 ".tx";
select -ne :postProcessList1;
	setAttr -s 2 ".p";
select -ne :defaultRenderUtilityList1;
	setAttr -s 2 ".u";
select -ne :defaultRenderingList1;
select -ne :renderGlobalsList1;
select -ne :defaultRenderGlobals;
	setAttr ".ren" -type "string" "mentalRay";
select -ne :defaultResolution;
	setAttr ".pa" 1;
select -ne :hardwareRenderGlobals;
	setAttr ".ctrs" 256;
	setAttr ".btrs" 512;
select -ne :defaultHardwareRenderGlobals;
	setAttr ".fn" -type "string" "im";
	setAttr ".res" -type "string" "ntsc_4d 646 485 1.333";
select -ne :ikSystem;
	setAttr -s 4 ".sol";
connectAttr "groupId184.id" "polySurfaceShape96.iog.og[0].gid";
connectAttr "oz_ec_catacombs_straight_a:lambert2SG.mwc" "polySurfaceShape96.iog.og[0].gco"
		;
connectAttr "oz_ec_catacombs_straight_a:block.oc" "oz_ec_catacombs_straight_a:lambert2SG.ss"
		;
connectAttr "polySurfaceShape96.iog.og[0]" "oz_ec_catacombs_straight_a:lambert2SG.dsm"
		 -na;
connectAttr "groupId184.msg" "oz_ec_catacombs_straight_a:lambert2SG.gn" -na;
connectAttr "oz_ec_catacombs_straight_a:lambert2SG.msg" "oz_ec_catacombs_straight_a:materialInfo1.sg"
		;
connectAttr "oz_ec_catacombs_straight_a:block.msg" "oz_ec_catacombs_straight_a:materialInfo1.m"
		;
connectAttr "oz_ec_catacombs_straight_a:file1.msg" "oz_ec_catacombs_straight_a:materialInfo1.t"
		 -na;
connectAttr "oz_ec_catacombs_straight_a:file1.oc" "oz_ec_catacombs_straight_a:block.c"
		;
connectAttr "oz_ec_catacombs_straight_a:place2dTexture1.c" "oz_ec_catacombs_straight_a:file1.c"
		;
connectAttr "oz_ec_catacombs_straight_a:place2dTexture1.tf" "oz_ec_catacombs_straight_a:file1.tf"
		;
connectAttr "oz_ec_catacombs_straight_a:place2dTexture1.rf" "oz_ec_catacombs_straight_a:file1.rf"
		;
connectAttr "oz_ec_catacombs_straight_a:place2dTexture1.mu" "oz_ec_catacombs_straight_a:file1.mu"
		;
connectAttr "oz_ec_catacombs_straight_a:place2dTexture1.mv" "oz_ec_catacombs_straight_a:file1.mv"
		;
connectAttr "oz_ec_catacombs_straight_a:place2dTexture1.s" "oz_ec_catacombs_straight_a:file1.s"
		;
connectAttr "oz_ec_catacombs_straight_a:place2dTexture1.wu" "oz_ec_catacombs_straight_a:file1.wu"
		;
connectAttr "oz_ec_catacombs_straight_a:place2dTexture1.wv" "oz_ec_catacombs_straight_a:file1.wv"
		;
connectAttr "oz_ec_catacombs_straight_a:place2dTexture1.re" "oz_ec_catacombs_straight_a:file1.re"
		;
connectAttr "oz_ec_catacombs_straight_a:place2dTexture1.of" "oz_ec_catacombs_straight_a:file1.of"
		;
connectAttr "oz_ec_catacombs_straight_a:place2dTexture1.r" "oz_ec_catacombs_straight_a:file1.ro"
		;
connectAttr "oz_ec_catacombs_straight_a:place2dTexture1.n" "oz_ec_catacombs_straight_a:file1.n"
		;
connectAttr "oz_ec_catacombs_straight_a:place2dTexture1.vt1" "oz_ec_catacombs_straight_a:file1.vt1"
		;
connectAttr "oz_ec_catacombs_straight_a:place2dTexture1.vt2" "oz_ec_catacombs_straight_a:file1.vt2"
		;
connectAttr "oz_ec_catacombs_straight_a:place2dTexture1.vt3" "oz_ec_catacombs_straight_a:file1.vt3"
		;
connectAttr "oz_ec_catacombs_straight_a:place2dTexture1.vc1" "oz_ec_catacombs_straight_a:file1.vc1"
		;
connectAttr "oz_ec_catacombs_straight_a:place2dTexture1.o" "oz_ec_catacombs_straight_a:file1.uv"
		;
connectAttr "oz_ec_catacombs_straight_a:place2dTexture1.ofs" "oz_ec_catacombs_straight_a:file1.fs"
		;
relationship "link" ":lightLinker1" ":initialShadingGroup.message" ":defaultLightSet.message";
relationship "link" ":lightLinker1" ":initialParticleSE.message" ":defaultLightSet.message";
relationship "link" ":lightLinker1" "oz_ec_catacombs_straight_a:lambert2SG.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" ":initialShadingGroup.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" ":initialParticleSE.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" "oz_ec_catacombs_straight_a:lambert2SG.message" ":defaultLightSet.message";
connectAttr "oz_ec_catacombs_straight_a:lambert2SG.pa" ":renderPartition.st" -na;
connectAttr "oz_ec_catacombs_straight_a:block.msg" ":defaultShaderList1.s" -na;
connectAttr "oz_ec_catacombs_straight_a:file1.msg" ":defaultTextureList1.tx" -na
		;
connectAttr "oz_ec_catacombs_straight_a:place2dTexture1.msg" ":defaultRenderUtilityList1.u"
		 -na;
// End of brazier1.ma
