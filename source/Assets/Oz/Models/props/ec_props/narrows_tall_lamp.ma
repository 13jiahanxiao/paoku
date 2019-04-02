//Maya ASCII 2012 scene
//Name: narrows_tall_lamp.ma
//Last modified: Thu, Apr 18, 2013 10:52:16 AM
//Codeset: 1252
requires maya "2012";
requires "stereoCamera" "10.0";
currentUnit -l meter -a degree -t film;
fileInfo "application" "maya";
fileInfo "product" "Maya 2012";
fileInfo "version" "2012 x64";
fileInfo "cutIdentifier" "001200000000-796618";
fileInfo "osv" "Microsoft Windows 7 Enterprise Edition, 64-bit Windows 7 Service Pack 1 (Build 7601)\n";
createNode transform -n "polySurface86";
createNode mesh -n "polySurfaceShape89" -p "polySurface86";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 368 ".uvst[0].uvsp";
	setAttr ".uvst[0].uvsp[0:249]" -type "float2" 0 0 1 0 1 1 0 1 0 0 1 0 1 1
		 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0
		 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1
		 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0
		 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0
		 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1
		 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0
		 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1
		 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1
		 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0
		 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1
		 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0
		 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0;
	setAttr ".uvst[0].uvsp[250:367]" 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0
		 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0
		 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1
		 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0
		 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1
		 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1 0 1 0 0 1 0 1 1
		 0 1 0 0 1 0 1 1 0 1;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 97 ".pt[0:96]" -type "float3"  -3.5209424 -1.6344038 7.8956838 
		-3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 
		7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 
		-1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 
		-3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 
		7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 
		-1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 
		-3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 
		7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 
		-1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 
		-3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 
		7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 
		-1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 
		-3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 
		7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 
		-1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 
		-3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 
		7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 
		-1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 
		-3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 
		7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 
		-1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 
		-3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 
		7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 
		-1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 
		-3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 
		7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 
		-1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 
		-3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 
		7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 
		-1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 
		-3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 
		7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 
		-1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 
		-3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 
		7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 
		-1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838 -3.5209424 -1.6344038 7.8956838;
	setAttr -s 97 ".vt[0:96]"  3.88764167 -0.14549537 -7.52898121 3.15423822 -0.14549537 -7.52898121
		 3.84761715 2.5612061 -7.56900978 3.19426751 2.5612061 -7.56900978 3.84761715 2.5612061 -8.22235584
		 3.19426751 2.5612061 -8.22235584 3.88764167 -0.14549537 -8.26238441 3.15423822 -0.14549537 -8.26238441
		 3.52094245 -0.14549537 -7.40674973 3.52094245 2.57276058 -7.4546771 3.52094245 2.57276058 -8.33668804
		 3.52094245 -0.14549537 -8.38461685 3.96194816 2.57276058 -7.89568233 3.079931736 2.57276058 -7.89568233
		 4.0098781586 -0.14549537 -7.89568233 3.032006741 -0.14549537 -7.89568233 3.52094245 -0.017694779 -7.4087038
		 3.88617682 -0.017694779 -7.53044796 4.0079197884 -0.017694779 -7.89568233 3.88617682 -0.017694779 -8.26091671
		 3.52094245 -0.017694779 -8.38266087 3.15570807 -0.017694779 -8.26091671 3.033964872 -0.017694779 -7.89568233
		 3.15570807 -0.017694779 -7.53044796 3.52094245 0.16298805 -7.68351173 3.68006825 0.16298805 -7.73655415
		 3.7331152 0.16298805 -7.89568233 3.68006825 0.16298805 -8.054810524 3.52094245 0.16298805 -8.10785294
		 3.36181641 0.16298805 -8.054810524 3.30876946 0.16298805 -7.89568233 3.36181641 0.16298805 -7.73655415
		 3.52094245 0.35627374 -7.68450546 3.67932606 0.35627374 -7.73729849 3.73211908 0.35627374 -7.89568233
		 3.67932606 0.35627374 -8.054065704 3.52094245 0.35627374 -8.10686016 3.3625586 0.35627374 -8.054065704
		 3.30976558 0.35627374 -7.89568233 3.3625586 0.35627374 -7.73729849 3.52094245 0.4567495 -7.41511583
		 3.88136721 0.4567495 -7.53525686 4.0015087128 0.4567495 -7.89568233 3.88136721 0.4567495 -8.25610733
		 3.52094245 0.4567495 -8.37624836 3.16051769 0.4567495 -8.25610733 3.040375948 0.4567495 -7.89568233
		 3.16051769 0.4567495 -7.53525686 3.52094245 0.86074507 -7.42129707 3.87672853 0.86074507 -7.53989553
		 3.99532723 0.86074507 -7.89568233 3.87672853 0.86074507 -8.25147057 3.52094245 0.86074507 -8.3700676
		 3.16515136 0.86074507 -8.25147057 3.046557665 0.86074507 -7.89568233 3.16515136 0.86074507 -7.53989553
		 3.52094245 2.39432716 -7.45317364 3.19271493 2.3839817 -7.56745911 3.07843256 2.39432716 -7.89568233
		 3.19271493 2.3839817 -8.22390747 3.52094245 2.39432716 -8.33819008 3.84916496 2.3839817 -8.22390747
		 3.96344733 2.39432716 -7.89568233 3.84916496 2.3839817 -7.56745911 3.9934864 2.5726428 -7.89568233
		 3.87097645 2.56026173 -8.24571609 3.52094245 2.5726428 -8.36822891 3.17090821 2.56026173 -8.24571609
		 3.048398495 2.5726428 -7.89568233 3.17090821 2.56026173 -7.5456481 3.52094245 2.5726428 -7.42313862
		 3.87097645 2.56026173 -7.5456481 3.9934864 2.8366797 -7.89568233 3.87097645 2.82429814 -8.24571609
		 3.52094245 2.8366797 -8.36822891 3.17090821 2.82429814 -8.24571609 3.048398495 2.8366797 -7.89568233
		 3.17090821 2.82429814 -7.5456481 3.52094245 2.8366797 -7.42313862 3.87097645 2.82429814 -7.5456481
		 3.56689453 3.29392886 -7.89568233 3.55498052 3.29272532 -7.9297204 3.52094245 3.29392886 -7.94163465
		 3.48690438 3.29272532 -7.9297204 3.47499013 3.29392886 -7.89568233 3.48690438 3.29272532 -7.86164665
		 3.52094245 3.29392886 -7.84973288 3.55498052 3.29272532 -7.86164665 3.56689453 3.41275454 -7.89568233
		 3.55498052 3.411551 -7.9297204 3.52094245 3.41275454 -7.94163465 3.52094245 3.41430306 -7.89568233
		 3.48690438 3.411551 -7.9297204 3.47499013 3.41275454 -7.89568233 3.48690438 3.411551 -7.86164665
		 3.52094245 3.41275454 -7.84973288 3.55498052 3.411551 -7.86164665;
	setAttr -s 188 ".ed";
	setAttr ".ed[0:165]"  0 8 0 8 1 0 2 9 1 9 3 1 4 10 1 10 5 1 6 11 0 11 7 0
		 0 17 1 1 23 1 2 12 1 12 4 1 3 13 1 13 5 1 4 61 1 5 59 1 6 14 0 14 0 0 7 15 0 15 1 0
		 56 48 1 52 60 1 58 54 1 62 50 1 16 8 1 17 25 1 16 17 1 18 14 1 17 18 1 19 6 1 18 19 1
		 20 11 1 19 20 1 21 7 1 20 21 1 22 15 1 21 22 1 23 31 1 22 23 1 23 16 1 24 16 1 25 33 1
		 24 25 1 26 18 1 25 26 1 27 19 1 26 27 1 28 20 1 27 28 1 29 21 1 28 29 1 30 22 1 29 30 1
		 31 39 1 30 31 1 31 24 1 32 24 1 33 41 1 32 33 1 34 26 1 33 34 1 35 27 1 34 35 1 36 28 1
		 35 36 1 37 29 1 36 37 1 38 30 1 37 38 1 39 47 1 38 39 1 39 32 1 40 32 1 41 49 1 40 41 1
		 42 34 1 41 42 1 43 35 1 42 43 1 44 36 1 43 44 1 45 37 1 44 45 1 46 38 1 45 46 1 47 55 1
		 46 47 1 47 40 1 48 40 1 49 63 1 48 49 1 50 42 1 49 50 1 51 43 1 50 51 1 52 44 1 51 52 1
		 53 45 1 52 53 1 54 46 1 53 54 1 55 57 1 54 55 1 55 48 1 56 9 1 57 3 1 56 57 1 58 13 1
		 57 58 1 59 53 1 58 59 1 60 10 1 59 60 1 61 51 1 60 61 1 62 12 1 61 62 1 63 2 1 62 63 1
		 63 56 1 12 64 1 4 65 1 64 65 1 10 66 1 65 66 1 5 67 1 66 67 1 13 68 1 68 67 1 3 69 1
		 69 68 1 9 70 1 70 69 1 2 71 1 71 70 1 71 64 1 64 72 1 65 73 1 72 73 1 66 74 1 73 74 1
		 67 75 1 74 75 1 68 76 1 76 75 1 69 77 1 77 76 1 70 78 1 78 77 1 71 79 1 79 78 1 79 72 1
		 72 80 1 73 81 1 80 81 1 74 82 1 81 82 1 75 83 1 82 83 1 76 84 1 84 83 1 77 85 1 85 84 1
		 78 86 1 86 85 1 79 87 1;
	setAttr ".ed[166:187]" 87 86 1 87 80 1 80 88 1 81 89 1 88 89 1 82 90 1 89 90 1
		 91 90 1 91 88 1 83 92 1 90 92 1 84 93 1 93 92 1 91 93 1 85 94 1 94 93 1 86 95 1 95 94 1
		 91 95 1 87 96 1 96 95 1 96 88 1;
	setAttr -s 92 ".fc[0:91]" -type "polyFaces" 
		f 4 119 20 90 89
		mu 0 4 0 1 2 3
		f 4 -102 103 -21 106
		mu 0 4 4 5 6 7
		f 4 170 172 -174 174
		mu 0 4 8 9 10 11
		f 4 176 -179 -180 173
		mu 0 4 12 13 14 15
		f 4 -182 -184 -185 179
		mu 0 4 16 17 18 19
		f 4 -187 187 -175 184
		mu 0 4 20 21 22 23
		f 4 96 21 114 113
		mu 0 4 24 25 26 27
		f 4 -110 112 -22 98
		mu 0 4 28 29 30 31
		f 4 108 22 102 101
		mu 0 4 32 33 34 35
		f 4 100 -23 110 109
		mu 0 4 36 37 38 39
		f 4 -114 116 23 94
		mu 0 4 40 41 42 43
		f 4 -90 92 -24 118
		mu 0 4 44 45 46 47
		f 4 -1 8 -27 24
		mu 0 4 48 49 50 51
		f 4 -29 -9 -18 -28
		mu 0 4 52 53 54 55
		f 4 -17 -30 -31 27
		mu 0 4 56 57 58 59
		f 4 -33 29 6 -32
		mu 0 4 60 61 62 63
		f 4 7 -34 -35 31
		mu 0 4 64 65 66 67
		f 4 -37 33 18 -36
		mu 0 4 68 69 70 71
		f 4 19 9 -39 35
		mu 0 4 72 73 74 75
		f 4 -40 -10 -2 -25
		mu 0 4 76 77 78 79
		f 4 26 25 -43 40
		mu 0 4 80 81 82 83
		f 4 -45 -26 28 -44
		mu 0 4 84 85 86 87
		f 4 30 -46 -47 43
		mu 0 4 88 89 90 91
		f 4 -49 45 32 -48
		mu 0 4 92 93 94 95
		f 4 34 -50 -51 47
		mu 0 4 96 97 98 99
		f 4 -53 49 36 -52
		mu 0 4 100 101 102 103
		f 4 38 37 -55 51
		mu 0 4 104 105 106 107
		f 4 -56 -38 39 -41
		mu 0 4 108 109 110 111
		f 4 42 41 -59 56
		mu 0 4 112 113 114 115
		f 4 -61 -42 44 -60
		mu 0 4 116 117 118 119
		f 4 46 -62 -63 59
		mu 0 4 120 121 122 123
		f 4 -65 61 48 -64
		mu 0 4 124 125 126 127
		f 4 50 -66 -67 63
		mu 0 4 128 129 130 131
		f 4 -69 65 52 -68
		mu 0 4 132 133 134 135
		f 4 54 53 -71 67
		mu 0 4 136 137 138 139
		f 4 -72 -54 55 -57
		mu 0 4 140 141 142 143
		f 4 58 57 -75 72
		mu 0 4 144 145 146 147
		f 4 -77 -58 60 -76
		mu 0 4 148 149 150 151
		f 4 62 -78 -79 75
		mu 0 4 152 153 154 155
		f 4 -81 77 64 -80
		mu 0 4 156 157 158 159
		f 4 66 -82 -83 79
		mu 0 4 160 161 162 163
		f 4 -85 81 68 -84
		mu 0 4 164 165 166 167
		f 4 70 69 -87 83
		mu 0 4 168 169 170 171
		f 4 -88 -70 71 -73
		mu 0 4 172 173 174 175
		f 4 74 73 -91 88
		mu 0 4 176 177 178 179
		f 4 -93 -74 76 -92
		mu 0 4 180 181 182 183
		f 4 78 -94 -95 91
		mu 0 4 184 185 186 187
		f 4 -97 93 80 -96
		mu 0 4 188 189 190 191
		f 4 82 -98 -99 95
		mu 0 4 192 193 194 195
		f 4 -101 97 84 -100
		mu 0 4 196 197 198 199
		f 4 86 85 -103 99
		mu 0 4 200 201 202 203
		f 4 -104 -86 87 -89
		mu 0 4 204 205 206 207
		f 4 3 -106 -107 104
		mu 0 4 208 209 210 211
		f 4 -109 105 12 -108
		mu 0 4 212 213 214 215
		f 4 13 15 -111 107
		mu 0 4 216 217 218 219
		f 4 -113 -16 -6 -112
		mu 0 4 220 221 222 223
		f 4 -5 14 -115 111
		mu 0 4 224 225 226 227
		f 4 -117 -15 -12 -116
		mu 0 4 228 229 230 231
		f 4 -11 -118 -119 115
		mu 0 4 232 233 234 235
		f 4 -120 117 2 -105
		mu 0 4 236 237 238 239
		f 4 11 121 -123 -121
		mu 0 4 240 241 242 243
		f 4 4 123 -125 -122
		mu 0 4 244 245 246 247
		f 4 5 125 -127 -124
		mu 0 4 248 249 250 251
		f 4 -14 127 128 -126
		mu 0 4 252 253 254 255
		f 4 -13 129 130 -128
		mu 0 4 256 257 258 259
		f 4 -4 131 132 -130
		mu 0 4 260 261 262 263
		f 4 -3 133 134 -132
		mu 0 4 264 265 266 267
		f 4 10 120 -136 -134
		mu 0 4 268 269 270 271
		f 4 122 137 -139 -137
		mu 0 4 272 273 274 275
		f 4 124 139 -141 -138
		mu 0 4 276 277 278 279
		f 4 126 141 -143 -140
		mu 0 4 280 281 282 283
		f 4 -129 143 144 -142
		mu 0 4 284 285 286 287
		f 4 -131 145 146 -144
		mu 0 4 288 289 290 291
		f 4 -133 147 148 -146
		mu 0 4 292 293 294 295
		f 4 -135 149 150 -148
		mu 0 4 296 297 298 299
		f 4 135 136 -152 -150
		mu 0 4 300 301 302 303
		f 4 138 153 -155 -153
		mu 0 4 304 305 306 307
		f 4 140 155 -157 -154
		mu 0 4 308 309 310 311
		f 4 142 157 -159 -156
		mu 0 4 312 313 314 315
		f 4 -145 159 160 -158
		mu 0 4 316 317 318 319
		f 4 -147 161 162 -160
		mu 0 4 320 321 322 323
		f 4 -149 163 164 -162
		mu 0 4 324 325 326 327
		f 4 -151 165 166 -164
		mu 0 4 328 329 330 331
		f 4 151 152 -168 -166
		mu 0 4 332 333 334 335
		f 4 154 169 -171 -169
		mu 0 4 336 337 338 339
		f 4 156 171 -173 -170
		mu 0 4 340 341 342 343
		f 4 158 175 -177 -172
		mu 0 4 344 345 346 347
		f 4 -161 177 178 -176
		mu 0 4 348 349 350 351
		f 4 -163 180 181 -178
		mu 0 4 352 353 354 355
		f 4 -165 182 183 -181
		mu 0 4 356 357 358 359
		f 4 -167 185 186 -183
		mu 0 4 360 361 362 363
		f 4 167 168 -188 -186
		mu 0 4 364 365 366 367;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".ndt" 0;
createNode shadingEngine -n "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG";
	setAttr ".ihi" 0;
	setAttr ".ro" yes;
createNode materialInfo -n "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_materialInfo1";
createNode lambert -n "oz_ec_narrows_straight_a:block";
createNode file -n "oz_ec_narrows_straight_a:file1";
	setAttr ".ftn" -type "string" "C:/Users/whitj304/TempleRunOzNew/Assets/Oz/Textures/GameTextures/EmeraldCity/Blockout.tga";
createNode place2dTexture -n "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_place2dTexture1";
createNode materialInfo -n "materialInfo2";
createNode shadingEngine -n "lambert3SG";
	setAttr ".ihi" 0;
	setAttr -s 2 ".dsm";
	setAttr ".ro" yes;
createNode lambert -n "block";
createNode file -n "file2";
	setAttr ".ftn" -type "string" "C:/Users/whitj304/TempleRunOzNew/Assets/Oz/Textures/GameTextures/EmeraldCity/Blockout.tga";
createNode lightLinker -s -n "lightLinker1";
	setAttr -s 5 ".lnk";
	setAttr -s 5 ".slnk";
select -ne :time1;
	setAttr ".o" 0.8;
	setAttr ".unw" 0.8;
select -ne :renderPartition;
	setAttr -s 5 ".st";
select -ne :initialShadingGroup;
	setAttr ".ro" yes;
select -ne :initialParticleSE;
	setAttr ".ro" yes;
select -ne :defaultShaderList1;
	setAttr -s 5 ".s";
select -ne :defaultTextureList1;
	setAttr -s 3 ".tx";
select -ne :postProcessList1;
	setAttr -s 2 ".p";
select -ne :defaultRenderUtilityList1;
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
connectAttr "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.mwc" "polySurfaceShape89.iog.og[0].gco"
		;
connectAttr "oz_ec_narrows_straight_a:block.oc" "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.ss"
		;
connectAttr "polySurfaceShape89.iog.og[0]" "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.dsm"
		 -na;
connectAttr "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.msg" "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_materialInfo1.sg"
		;
connectAttr "oz_ec_narrows_straight_a:block.msg" "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_materialInfo1.m"
		;
connectAttr "oz_ec_narrows_straight_a:file1.msg" "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_materialInfo1.t"
		 -na;
connectAttr "oz_ec_narrows_straight_a:file1.oc" "oz_ec_narrows_straight_a:block.c"
		;
connectAttr "oz_ec_narrows_straight_a:file1.ot" "oz_ec_narrows_straight_a:block.it"
		;
connectAttr "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_place2dTexture1.c" "oz_ec_narrows_straight_a:file1.c"
		;
connectAttr "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_place2dTexture1.tf" "oz_ec_narrows_straight_a:file1.tf"
		;
connectAttr "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_place2dTexture1.rf" "oz_ec_narrows_straight_a:file1.rf"
		;
connectAttr "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_place2dTexture1.mu" "oz_ec_narrows_straight_a:file1.mu"
		;
connectAttr "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_place2dTexture1.mv" "oz_ec_narrows_straight_a:file1.mv"
		;
connectAttr "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_place2dTexture1.s" "oz_ec_narrows_straight_a:file1.s"
		;
connectAttr "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_place2dTexture1.wu" "oz_ec_narrows_straight_a:file1.wu"
		;
connectAttr "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_place2dTexture1.wv" "oz_ec_narrows_straight_a:file1.wv"
		;
connectAttr "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_place2dTexture1.re" "oz_ec_narrows_straight_a:file1.re"
		;
connectAttr "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_place2dTexture1.of" "oz_ec_narrows_straight_a:file1.of"
		;
connectAttr "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_place2dTexture1.r" "oz_ec_narrows_straight_a:file1.ro"
		;
connectAttr "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_place2dTexture1.n" "oz_ec_narrows_straight_a:file1.n"
		;
connectAttr "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_place2dTexture1.vt1" "oz_ec_narrows_straight_a:file1.vt1"
		;
connectAttr "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_place2dTexture1.vt2" "oz_ec_narrows_straight_a:file1.vt2"
		;
connectAttr "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_place2dTexture1.vt3" "oz_ec_narrows_straight_a:file1.vt3"
		;
connectAttr "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_place2dTexture1.vc1" "oz_ec_narrows_straight_a:file1.vc1"
		;
connectAttr "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_place2dTexture1.o" "oz_ec_narrows_straight_a:file1.uv"
		;
connectAttr "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_place2dTexture1.ofs" "oz_ec_narrows_straight_a:file1.fs"
		;
connectAttr "lambert3SG.msg" "materialInfo2.sg";
connectAttr "block.msg" "materialInfo2.m";
connectAttr "file2.msg" "materialInfo2.t" -na;
connectAttr "block.oc" "lambert3SG.ss";
connectAttr "polySurfaceShape89.iog" "lambert3SG.dsm" -na;
connectAttr "file2.oc" "block.c";
relationship "link" ":lightLinker1" ":initialShadingGroup.message" ":defaultLightSet.message";
relationship "link" ":lightLinker1" ":initialParticleSE.message" ":defaultLightSet.message";
relationship "link" ":lightLinker1" "lambert3SG.message" ":defaultLightSet.message";
relationship "link" ":lightLinker1" "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" ":initialShadingGroup.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" ":initialParticleSE.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" "lambert3SG.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.message" ":defaultLightSet.message";
connectAttr "lambert3SG.pa" ":renderPartition.st" -na;
connectAttr "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.pa" ":renderPartition.st"
		 -na;
connectAttr "block.msg" ":defaultShaderList1.s" -na;
connectAttr "oz_ec_narrows_straight_a:block.msg" ":defaultShaderList1.s" -na;
connectAttr "file2.msg" ":defaultTextureList1.tx" -na;
connectAttr "oz_ec_narrows_straight_a:file1.msg" ":defaultTextureList1.tx" -na;
connectAttr "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_place2dTexture1.msg" ":defaultRenderUtilityList1.u"
		 -na;
// End of narrows_tall_lamp.ma
