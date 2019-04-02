//Maya ASCII 2012 scene
//Name: oz_ec_narrows_straight_over_anim_a.ma
//Last modified: Mon, Jun 24, 2013 10:02:38 AM
//Codeset: 1252
requires maya "2012";
requires "Mayatomr" "2012.0m - 3.9.1.36 ";
requires "stereoCamera" "10.0";
currentUnit -l meter -a degree -t ntsc;
fileInfo "application" "maya";
fileInfo "product" "Maya 2012";
fileInfo "version" "2012 x64";
fileInfo "cutIdentifier" "001200000000-796618";
fileInfo "osv" "Microsoft Windows 7 Enterprise Edition, 64-bit Windows 7 Service Pack 1 (Build 7601)\n";
createNode transform -s -n "persp";
	setAttr ".t" -type "double3" -0.52665026319220942 9.2663157402184719 10.796804914277887 ;
	setAttr ".r" -type "double3" -23.738352703041375 -0.1999999999991017 -6.9575057532074188e-016 ;
createNode camera -s -n "perspShape" -p "persp";
	setAttr -k off ".v";
	setAttr ".fl" 34.999999999999986;
	setAttr ".ncp" 1;
	setAttr ".fcp" 100000;
	setAttr ".coi" 20.264978907154866;
	setAttr ".imn" -type "string" "persp";
	setAttr ".den" -type "string" "persp_depth";
	setAttr ".man" -type "string" "persp_mask";
	setAttr ".tp" -type "double3" -134.235595703125 -27.727432250976563 -861.9255909530857 ;
	setAttr ".hc" -type "string" "viewSet -p %camera";
createNode transform -s -n "top";
	setAttr ".t" -type "double3" -0.70688806713012187 100.10000000000001 -9.6652435528028189 ;
	setAttr ".r" -type "double3" -89.999999999999986 0 0 ;
createNode camera -s -n "topShape" -p "top";
	setAttr -k off ".v";
	setAttr ".rnd" no;
	setAttr ".ncp" 1;
	setAttr ".fcp" 100000;
	setAttr ".coi" 100.10000000000001;
	setAttr ".ow" 34.063303012593025;
	setAttr ".imn" -type "string" "top";
	setAttr ".den" -type "string" "top_depth";
	setAttr ".man" -type "string" "top_mask";
	setAttr ".hc" -type "string" "viewSet -t %camera";
	setAttr ".o" yes;
createNode transform -s -n "front";
	setAttr ".t" -type "double3" 3.4941957447660825 -15.960548203530598 100.10000000000001 ;
createNode camera -s -n "frontShape" -p "front";
	setAttr -k off ".v";
	setAttr ".rnd" no;
	setAttr ".ncp" 1;
	setAttr ".fcp" 100000;
	setAttr ".coi" 100.10000000000001;
	setAttr ".ow" 14.531123236256439;
	setAttr ".imn" -type "string" "front";
	setAttr ".den" -type "string" "front_depth";
	setAttr ".man" -type "string" "front_mask";
	setAttr ".hc" -type "string" "viewSet -f %camera";
	setAttr ".o" yes;
createNode transform -s -n "side";
	setAttr ".t" -type "double3" 100.10000000000001 9.5951073013692447 -0.37561117077672918 ;
	setAttr ".r" -type "double3" 0 89.999999999999986 0 ;
createNode camera -s -n "sideShape" -p "side";
	setAttr -k off ".v";
	setAttr ".rnd" no;
	setAttr ".ncp" 1;
	setAttr ".fcp" 100000;
	setAttr ".coi" 100.10000000000001;
	setAttr ".ow" 47.505072278405912;
	setAttr ".imn" -type "string" "side";
	setAttr ".den" -type "string" "side_depth";
	setAttr ".man" -type "string" "side_mask";
	setAttr ".hc" -type "string" "viewSet -s %camera";
	setAttr ".o" yes;
createNode transform -n "path_default";
createNode transform -n "dummy_a" -p "path_default";
	setAttr ".r" -type "double3" -89.999999999999986 0 0 ;
createNode transform -n "dummy_b" -p "path_default";
	setAttr ".t" -type "double3" 0 8.8817841970012525e-018 -19.008305664062505 ;
	setAttr ".r" -type "double3" -89.999999999999986 0 0 ;
	setAttr ".rp" -type "double3" 0 0.020439453124993179 -9.4414341796078518e-015 ;
	setAttr ".rpt" -type "double3" 0 -0.020439453124993189 -0.02043945312498829 ;
	setAttr ".sp" -type "double3" 0 0.020439453124993179 -9.4414341796078518e-015 ;
createNode transform -n "custom_lights";
createNode transform -n "pointLight1" -p "custom_lights";
	setAttr ".t" -type "double3" 1.7849347319670339 1.3326337273554922 -8.8975189003520523 ;
createNode pointLight -n "pointLightShape1" -p "pointLight1";
	setAttr -k off ".v";
	setAttr ".in" 5000;
	setAttr ".de" 2;
createNode transform -n "pointLight2" -p "custom_lights";
	setAttr ".t" -type "double3" -1.5361814170492778 1.3326337273554922 -8.8975189003520505 ;
createNode pointLight -n "pointLightShape2" -p "pointLight2";
	setAttr -k off ".v";
	setAttr ".in" 5000;
	setAttr ".de" 2;
createNode transform -n "Tile1";
	setAttr ".t" -type "double3" -0.018541862961230619 -0.10756223920558683 0.39610675484824909 ;
	setAttr ".s" -type "double3" 1 3.1239342550965419 1 ;
	setAttr ".rp" -type "double3" -0.47853600255613382 -0.10446868111448673 -9.3756119860209672 ;
	setAttr ".sp" -type "double3" -0.47853600255613382 -0.10446868111448673 -9.3756119860209672 ;
createNode mesh -n "Tile1Shape" -p "Tile1";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.77882093191146851 0.38828185200691223 ;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 18 ".uvst[0].uvsp[0:17]" -type "float2" 0.078245603 0.61554378
		 0.013414219 0.95398885 0.078245595 0.54693478 0.046465263 0.54693478 0.078245603
		 0.61554378 0.04646527 0.61554378 0.078245595 0.54693478 0.046465263 0.54693478 0.078245603
		 0.61554378 0.078245595 0.54693478 0.04646527 0.61554378 0.046465263 0.54693478 0.04646527
		 0.61554378 0.013414253 0.999816 0.046465263 0.54693478 0.10784604 0.95398885 0.10784598
		 0.999816 0.078245595 0.54693478;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 12 ".uvst[1].uvsp[0:11]" -type "float2" 0.728894 0.91117823
		 0.74253696 0.90681303 0.74253696 0.91117823 0.73347545 0.91117823 0.74253696 0.92539918
		 0.73347545 0.92539918 0.74711841 0.92539918 0.73347545 0.92976439 0.74711841 0.91117823
		 0.728894 0.92539918 0.74253696 0.92976439 0.73347545 0.90681303;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 4 ".pt";
	setAttr ".pt[1]" -type "float3" 0.035616547 0.00046838605 -0.0016684234 ;
	setAttr ".pt[3]" -type "float3" 0.035616547 0.00046838605 -0.0016684234 ;
	setAttr ".pt[5]" -type "float3" 0.035616547 0.00046838605 -0.0016684234 ;
	setAttr ".pt[7]" -type "float3" 0.035616547 0.00046838605 -0.0016684234 ;
	setAttr -s 8 ".vt[0:7]"  -0.96647418 -0.16091801 -8.50894451 0.0094021484 -0.16091801 -8.50894451
		 -0.96647418 -0.048019361 -8.50894451 0.0094021484 -0.048019361 -8.50894451 -0.96647418 -0.048019361 -9.50437164
		 0.0094021484 -0.048019361 -9.50437164 -0.96647418 -0.16091801 -9.50437164 0.0094021484 -0.16091801 -9.50437164;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 5 ".fc[0:4]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 15 1 13 16
		mu 1 4 11 1 2 3
		f 4 1 7 -3 -7
		mu 0 4 2 14 5 4
		mu 1 4 3 2 4 5
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		mu 1 4 5 4 10 7
		f 4 -12 -10 -8 -6
		mu 0 4 12 8 9 3
		mu 1 4 8 6 4 2
		f 4 10 4 6 8
		mu 0 4 10 0 17 11
		mu 1 4 9 0 3 5;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".bw" 3;
createNode transform -n "Tile2";
	setAttr ".s" -type "double3" 1 3.4671854910374926 1 ;
	setAttr ".rp" -type "double3" 0.49657380696009035 -0.10446868111448673 -9.4182005959771793 ;
	setAttr ".sp" -type "double3" 0.49657380696009035 -0.10446868111448673 -9.4182005959771793 ;
createNode mesh -n "Tile2Shape" -p "Tile2";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.060553139499682662 0.96918219420193896 ;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 18 ".uvst[0].uvsp[0:17]" -type "float2" 0.078245603 0.61554378
		 0.0057035945 0.93884271 0.078245595 0.54693478 0.046465263 0.54693478 0.078245603
		 0.61554378 0.04646527 0.61554378 0.078245595 0.54693478 0.046465263 0.54693478 0.078245603
		 0.61554378 0.078245595 0.54693478 0.04646527 0.61554378 0.046465263 0.54693478 0.04646527
		 0.61554378 0.0057035685 0.99952167 0.046465263 0.54693478 0.11540271 0.93884271 0.1154027
		 0.99952167 0.078245595 0.54693478;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 12 ".uvst[1].uvsp[0:11]" -type "float2" 0.73157918 0.93599081
		 0.74350142 0.93217611 0.74350142 0.93599081 0.73558283 0.93599081 0.74350142 0.94841808
		 0.73558283 0.94841808 0.74750513 0.94841808 0.73558283 0.95223272 0.74750513 0.93599081
		 0.73157918 0.94841808 0.74350142 0.95223272 0.73558283 0.93217611;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 4 ".pt";
	setAttr ".pt[1]" -type "float3" -0.15400399 0.0012759709 -0.010641179 ;
	setAttr ".pt[3]" -type "float3" -0.15400399 0.0012759709 -0.010641179 ;
	setAttr ".pt[5]" -type "float3" -0.15400399 0.0012759709 -0.010641179 ;
	setAttr ".pt[7]" -type "float3" -0.15400399 0.0012759709 -0.010641179 ;
	setAttr -s 8 ".vt[0:7]"  0.0086356355 -0.16091801 -8.90351105 0.98451197 -0.16091801 -8.90351105
		 0.0086356355 -0.048019361 -8.90351105 0.98451197 -0.048019361 -8.90351105 0.0086356355 -0.048019361 -9.49466705
		 0.98451197 -0.048019361 -9.49466705 0.0086356355 -0.16091801 -9.49466705 0.98451197 -0.16091801 -9.49466705;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 5 ".fc[0:4]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 15 1 13 16
		mu 1 4 11 1 2 3
		f 4 1 7 -3 -7
		mu 0 4 2 14 5 4
		mu 1 4 3 2 4 5
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		mu 1 4 5 4 10 7
		f 4 -12 -10 -8 -6
		mu 0 4 12 8 9 3
		mu 1 4 8 6 4 2
		f 4 10 4 6 8
		mu 0 4 10 0 17 11
		mu 1 4 9 0 3 5;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".bw" 3;
createNode transform -n "firework";
	setAttr ".rp" -type "double3" 2.7576962280273416 -15.807085571289063 -9.4301367187500009 ;
	setAttr ".sp" -type "double3" 2.7576962280273416 -15.807085571289063 -9.4301367187500009 ;
	setAttr ".smd" 7;
createNode mesh -n "fireworkShape" -p "firework";
	addAttr -ci true -sn "mso" -ln "miShadingSamplesOverride" -min 0 -max 1 -at "bool";
	addAttr -ci true -sn "msh" -ln "miShadingSamples" -min 0 -smx 8 -at "float";
	addAttr -ci true -sn "mdo" -ln "miMaxDisplaceOverride" -min 0 -max 1 -at "bool";
	addAttr -ci true -sn "mmd" -ln "miMaxDisplace" -min 0 -smx 1 -at "float";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.72535573650797747 0.18297255445190996 ;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 18 ".uvst[0].uvsp[0:17]" -type "float2" 0.99476498 0.72869557
		 0.9933002 0.72958738 0.99383104 0.75062627 0.9924612 0.68937731 0.9926554 0.73277706
		 0.990982 0.7331273 0.98893094 0.73203349 0.98917598 0.68863386 0.99178785 0.73131949
		 0.9964996 0.73042768 0.99832511 0.73131949 0.99546838 0.73203349 0.99751925 0.7331273
		 0.99919271 0.73277706 0.9939822 0.73168319 0.99571341 0.68863386 0.99385107 0.68828362
		 0.99105853 0.68972766;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 21 ".uvst[1].uvsp[0:20]" -type "float2" 0.72385377 0.91992527
		 0.70787245 0.92197907 0.72292626 0.93567318 0.72751045 0.93288225 0.71772987 0.93413913
		 0.69904137 0.89790547 0.71890998 0.88442564 0.71851039 0.89786595 0.71298677 0.89787716
		 0.71835214 0.90318948 0.71282858 0.90320069 0.73229754 0.90316111 0.71354467 0.87911332
		 0.71795285 0.91662973 0.71906829 0.87910217 0.73245579 0.89783758 0.71242923 0.916641
		 0.69888318 0.903229 0.71338642 0.88443691 0.72214615 0.93958706 0.72834677 0.9387902;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 13 ".vt[0:12]"  2.72409534 -15.16744137 -10.45436764 1.69992554 -15.17778587 -9.43014622
		 2.72409534 -15.16744137 -8.40592194 3.74826789 -15.15708733 -9.43014526 2.71374631 -14.14326859 -9.43014622
		 2.31774902 -17.20838928 -9.0031938553 3.1716125 -17.19976234 -9.0031938553 2.29596066 -15.052648544 -9.0031929016
		 3.14982414 -15.044027328 -9.0031929016 2.29596066 -15.052648544 -9.85709858 3.14982414 -15.044027328 -9.85709953
		 2.31774783 -17.20838928 -9.85709953 3.1716125 -17.19976234 -9.85709953;
	setAttr -s 20 ".ed[0:19]"  0 1 0 1 2 0 2 3 0 3 0 0 0 4 0 1 4 0 2 4 0
		 3 4 0 5 6 0 7 8 0 9 10 0 11 12 0 5 7 0 6 8 0 7 9 0 8 10 0 9 11 0 10 12 0 11 5 0 12 6 0;
	setAttr -s 11 ".fc[0:10]" -type "polyFaces" 
		f 4 -4 -3 -2 -1
		mu 0 4 0 1 8 9
		mu 1 4 20 19 2 3
		f 3 0 5 -5
		mu 0 3 0 9 2
		mu 1 3 0 3 4
		f 3 1 6 -6
		mu 0 3 9 10 2
		mu 1 3 3 2 4
		f 3 2 7 -7
		mu 0 3 8 1 2
		mu 1 3 2 19 4
		f 3 3 4 -8
		mu 0 3 1 0 2
		mu 1 3 1 0 4
		f 4 8 13 -10 -13
		mu 0 4 17 3 4 5
		mu 1 4 18 6 7 8
		f 4 9 15 -11 -15
		mu 0 4 12 13 14 11
		mu 1 4 8 7 9 10
		f 4 10 17 -12 -17
		mu 0 4 11 14 16 15
		mu 1 4 10 9 13 16
		f 4 11 19 -9 -19
		mu 0 4 15 16 3 17
		mu 1 4 12 14 6 18
		f 4 -20 -18 -16 -14
		mu 0 4 3 16 14 4
		mu 1 4 15 11 9 7
		f 4 18 12 14 16
		mu 0 4 7 17 5 6
		mu 1 4 17 5 8 10;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".bw" 3;
	setAttr ".ndt" 0;
createNode transform -n "curve1";
	setAttr ".v" no;
	setAttr ".t" -type "double3" 0 0 -11.941560728426616 ;
createNode nurbsCurve -n "curveShape1" -p "curve1";
	setAttr -k off ".v";
	setAttr ".cc" -type "nurbsCurve" 
		3 7 0 no 3
		12 0 0 0 1 2 3 4 5 6 7 7 7
		10
		0 -17.999999999990422 0
		6.4472689797236162 -11.807626245277284 0
		16.047051039742826 6.0867975427099852 0.79204252201964553
		9.4778254496435466 7.0967644812616584 0.47211340998665741
		6.7076883825689677 3.0653874633787264 0.56376607508905263
		9.4417209887659386 1.3116041735753425 0.51708452671237359
		11.639558351183492 3.1705030918059256 1.8898867610058974
		8.9966757566106512 6.5583763124779066 1.3670004499009094
		4.4120556554910548 2.4082223583769444 2.2681317553562717
		1.086558540303042 0.23788077570100122 2.2213187910400722
		;
createNode transform -n "positionMarker1" -p "curveShape1";
createNode positionMarker -n "positionMarkerShape1" -p "positionMarker1";
	setAttr -k off ".v";
	setAttr ".uwo" yes;
	setAttr ".t" 1;
createNode transform -n "positionMarker2" -p "curveShape1";
createNode positionMarker -n "positionMarkerShape2" -p "positionMarker2";
	setAttr -k off ".v";
	setAttr ".uwo" yes;
	setAttr ".lp" -type "double3" 0.01 0 0 ;
	setAttr ".t" 30;
createNode transform -n "Tile3";
	setAttr ".t" -type "double3" -1.0632656012331971 -0.10438704420723129 0.026813030290426271 ;
	setAttr ".s" -type "double3" 1 3.4671854910374926 1 ;
	setAttr ".rp" -type "double3" 0.49657380696009035 -0.10446868111448673 -9.4182005959771793 ;
	setAttr ".sp" -type "double3" 0.49657380696009035 -0.10446868111448673 -9.4182005959771793 ;
createNode mesh -n "Tile3Shape" -p "Tile3";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.057193181918703928 0.95464209709374148 ;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 18 ".uvst[0].uvsp[0:17]" -type "float2" 0.078245603 0.61554378
		 0.0057035945 0.93884271 0.078245595 0.54693478 0.041303013 0.92033756 0.078245603
		 0.61554378 0.04646527 0.61554378 0.078245595 0.54693478 0.046465263 0.54693478 0.073083349
		 0.98894656 0.073083341 0.92033756 0.04646527 0.61554378 0.046465263 0.54693478 0.04130302
		 0.98894656 0.0057035685 0.99952167 0.046465263 0.54693478 0.11540271 0.93884271 0.1154027
		 0.99952167 0.078245595 0.54693478;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 12 ".uvst[1].uvsp[0:11]" -type "float2" 0.73130208 0.88224834
		 0.74322432 0.8784337 0.74322432 0.88224834 0.73530567 0.88224834 0.74322432 0.89467567
		 0.73530567 0.89467567 0.74722791 0.89467567 0.73530567 0.89849031 0.74722791 0.88224834
		 0.73130208 0.89467567 0.74322432 0.89849031 0.73530567 0.8784337;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  0.0086356355 -0.16091801 -8.90351105 0.98451197 -0.16091801 -8.90351105
		 0.0086356355 -0.048019361 -8.90351105 0.98451197 -0.048019361 -8.90351105 0.0086356355 -0.048019361 -9.49466705
		 0.98451197 -0.048019361 -9.49466705 0.0086356355 -0.16091801 -9.49466705 0.98451197 -0.16091801 -9.49466705;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 5 ".fc[0:4]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 15 1 13 16
		mu 1 4 11 1 2 3
		f 4 1 7 -3 -7
		mu 0 4 2 14 5 4
		mu 1 4 3 2 4 5
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		mu 1 4 5 4 10 7
		f 4 -12 -10 -8 -6
		mu 0 4 12 8 9 3
		mu 1 4 8 6 4 2
		f 4 10 4 6 8
		mu 0 4 10 0 17 11
		mu 1 4 9 0 3 5;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".bw" 3;
createNode transform -n "Tile4";
	setAttr ".t" -type "double3" 0.51450142439057656 -0.28233709161771353 0 ;
	setAttr ".s" -type "double3" 0.41916143137286926 3.4671854910374926 1 ;
	setAttr ".rp" -type "double3" 0.49657380696009035 -0.10446868111448673 -9.4182005959771793 ;
	setAttr ".sp" -type "double3" 0.49657380696009035 -0.10446868111448673 -9.4182005959771793 ;
createNode mesh -n "Tile4Shape" -p "Tile4";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.038822401314973831 0.58123928308486938 ;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 20 ".uvst[0].uvsp[0:19]" -type "float2" 0.078245603 0.61554378
		 0.0057035945 0.93884271 0.046208482 0.54693478 0.046465263 0.54693478 0.078245603
		 0.61554378 0.031436309 0.61554378 0.078245595 0.54693478 0.046465263 0.54693478 0.078245603
		 0.61554378 0.078245595 0.54693478 0.04646527 0.61554378 0.046465263 0.54693478 0.04646527
		 0.61554378 0.0057035685 0.99952167 0.031436309 0.54693478 0.11540271 0.93884271 0.1154027
		 0.99952167 0.078245595 0.54693478 0.04646527 0.61554378 0.046208493 0.61554378;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 12 ".uvst[1].uvsp[0:11]" -type "float2" 0.6988216 0.88201195
		 0.70780838 0.8791365 0.70780838 0.88201195 0.70183945 0.88201195 0.70780838 0.89137948
		 0.70183945 0.89137948 0.71082634 0.89137948 0.70183945 0.89425492 0.71082634 0.88201195
		 0.6988216 0.89137948 0.70780838 0.89425492 0.70183945 0.8791365;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  0.0086356355 -0.16091801 -8.90351105 0.98451197 -0.16091801 -8.90351105
		 0.0086356355 -0.048019361 -8.90351105 0.98451197 -0.048019361 -8.90351105 0.0086356355 -0.048019361 -9.49466705
		 0.98451197 -0.048019361 -9.49466705 0.0086356355 -0.16091801 -9.49466705 0.98451197 -0.16091801 -9.49466705;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 5 ".fc[0:4]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 15 1 13 16
		mu 1 4 11 1 2 3
		f 4 1 7 -3 -7
		mu 0 4 2 14 5 19
		mu 1 4 3 2 4 5
		f 4 2 9 -4 -9
		mu 0 4 4 18 7 6
		mu 1 4 5 4 10 7
		f 4 -12 -10 -8 -6
		mu 0 4 12 8 9 3
		mu 1 4 8 6 4 2
		f 4 10 4 6 8
		mu 0 4 10 0 17 11
		mu 1 4 9 0 3 5;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".bw" 3;
createNode transform -n "Tile5";
	setAttr ".t" -type "double3" 0.9134060628291506 -0.15095050822579956 0 ;
	setAttr ".s" -type "double3" 0.41916143137286926 3.4671854910374926 0.52083326915750161 ;
	setAttr ".rp" -type "double3" 0.49657380696009035 -0.10446868111448673 -9.4182005959771793 ;
	setAttr ".sp" -type "double3" 0.49657380696009035 -0.10446868111448673 -9.4182005959771793 ;
createNode mesh -n "Tile5Shape" -p "Tile5";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.022600601698892817 0.58123928308486938 ;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 20 ".uvst[0].uvsp[0:19]" -type "float2" 0.078245603 0.61554378
		 0.0057035945 0.93884271 0.029986683 0.54693478 0.046465263 0.54693478 0.078245603
		 0.61554378 0.01521451 0.61554378 0.078245595 0.54693478 0.046465263 0.54693478 0.078245603
		 0.61554378 0.078245595 0.54693478 0.04646527 0.61554378 0.046465263 0.54693478 0.04646527
		 0.61554378 0.0057035685 0.99952167 0.01521451 0.54693478 0.11540271 0.93884271 0.1154027
		 0.99952167 0.078245595 0.54693478 0.04646527 0.61554378 0.029986694 0.61554378;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 12 ".uvst[1].uvsp[0:11]" -type "float2" 0.6983763 0.92803967
		 0.70573545 0.92568505 0.70573545 0.92803967 0.70084757 0.92803967 0.70573545 0.93571055
		 0.70084757 0.93571055 0.70820665 0.93571055 0.70084757 0.93806517 0.70820665 0.92803967
		 0.6983763 0.93571055 0.70573545 0.93806517 0.70084757 0.92568505;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  0.0086356355 -0.16091801 -8.90351105 0.98451197 -0.16091801 -8.90351105
		 0.0086356355 -0.048019361 -8.90351105 0.98451197 -0.048019361 -8.90351105 0.0086356355 -0.048019361 -9.49466705
		 0.98451197 -0.048019361 -9.49466705 0.0086356355 -0.16091801 -9.49466705 0.98451197 -0.16091801 -9.49466705;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 5 ".fc[0:4]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 15 1 13 16
		mu 1 4 11 1 2 3
		f 4 1 7 -3 -7
		mu 0 4 2 14 5 19
		mu 1 4 3 2 4 5
		f 4 2 9 -4 -9
		mu 0 4 4 18 7 6
		mu 1 4 5 4 10 7
		f 4 -12 -10 -8 -6
		mu 0 4 12 8 9 3
		mu 1 4 8 6 4 2
		f 4 10 4 6 8
		mu 0 4 10 0 17 11
		mu 1 4 9 0 3 5;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".bw" 3;
createNode mesh -n "polySurfaceShape19" -p "Tile5";
	setAttr -k off ".v";
	setAttr ".io" yes;
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.062355432659387589 0.58123928308486938 ;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 18 ".uvst[0].uvsp[0:17]" -type "float2" 0.078245603 0.61554378
		 0.0057035945 0.93884271 0.078245595 0.54693478 0.046465263 0.54693478 0.078245603
		 0.61554378 0.04646527 0.61554378 0.078245595 0.54693478 0.046465263 0.54693478 0.078245603
		 0.61554378 0.078245595 0.54693478 0.04646527 0.61554378 0.046465263 0.54693478 0.04646527
		 0.61554378 0.0057035685 0.99952167 0.046465263 0.54693478 0.11540271 0.93884271 0.1154027
		 0.99952167 0.078245595 0.54693478;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 12 ".uvst[1].uvsp[0:11]" -type "float2" 0.70025212 0.037587702
		 0.80502731 0.34959358 0.70025218 0.34959358 0.70025212 0.14236271 0.35891798 0.3495937
		 0.35891798 0.14236271 0.35891798 0.45436883 0.25414303 0.14236277 0.70025218 0.45436871
		 0.35891798 0.037587643 0.25414303 0.3495937 0.80502731 0.14236277;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  0.0086356355 -0.16091801 -8.90351105 0.98451197 -0.16091801 -8.90351105
		 0.0086356355 -0.048019361 -8.90351105 0.98451197 -0.048019361 -8.90351105 0.0086356355 -0.048019361 -9.49466705
		 0.98451197 -0.048019361 -9.49466705 0.0086356355 -0.16091801 -9.49466705 0.98451197 -0.16091801 -9.49466705;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 5 ".fc[0:4]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 15 1 13 16
		mu 1 4 11 1 2 3
		f 4 1 7 -3 -7
		mu 0 4 2 14 5 4
		mu 1 4 3 2 4 5
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		mu 1 4 5 4 10 7
		f 4 -12 -10 -8 -6
		mu 0 4 12 8 9 3
		mu 1 4 8 6 4 2
		f 4 10 4 6 8
		mu 0 4 10 0 17 11
		mu 1 4 9 0 3 5;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".bw" 3;
createNode transform -n "Tile6";
	setAttr ".rp" -type "double3" 0.34941823998767296 -0.18697775068957342 -9.0448414095714664 ;
	setAttr ".sp" -type "double3" 0.34941823998767296 -0.18697775068957342 -9.0448414095714664 ;
createNode mesh -n "Tile6Shape" -p "Tile6";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 1.4593489456214663 0.39766551337374978 ;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 16 ".uvst[0].uvsp[0:15]" -type "float2" 0.062355436 0.61554378
		 0.060553156 0.93884271 0.046208482 0.54693478 0.0099949539 0.99538028 0.078245603
		 0.61554378 0.031436309 0.61554378 0.062355429 0.54693478 0.062355455 0.92677128 0.1147159
		 0.99538028 0.046465263 0.54693478 0.0057035685 0.99952167 0.031436309 0.54693478
		 0.1154027 0.99952167 0.078245595 0.54693478 0.04646527 0.61554378 0.046208493 0.61554378;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 8 ".uvst[1].uvsp[0:7]" -type "float2" 0.73698044 0.97483796
		 0.74256784 0.96780121 0.74534607 0.97047794 0.73978966 0.97047794 0.74534607 0.97919798
		 0.73978966 0.97919798 0.74815542 0.97483796 0.74256784 0.9818747;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 5 ".pt[0:4]" -type "float3"  -0.13667275 -0.14226909 0.16319849 
		0.23095922 -0.017038135 0.040210284 -0.50315452 0.00234328 0.008698307 0.18787774 
		-0.047841396 0.28189552 -0.54623598 -0.028459981 0.25038356;
	setAttr -s 5 ".vt[0:4]"  0.49657381 -0.16091801 -9.19908905 0.0086356355 -0.048019361 -8.90351105
		 0.98451197 -0.048019361 -8.90351105 0.0086356355 -0.048019361 -9.49466705 0.98451197 -0.048019361 -9.49466705;
	setAttr -s 8 ".ed[0:7]"  1 2 0 3 4 0 0 1 0 0 2 0 1 3 0 2 4 0 3 0 0
		 4 0 0;
	setAttr -s 5 ".fc[0:4]" -type "polyFaces" 
		f 3 3 -1 -3
		mu 0 3 1 10 12
		mu 1 3 1 2 3
		f 4 0 5 -2 -5
		mu 0 4 2 11 5 15
		mu 1 4 3 2 4 5
		f 3 1 7 -7
		mu 0 3 4 14 6
		mu 1 3 5 4 7
		f 3 -8 -6 -4
		mu 0 3 7 8 3
		mu 1 3 6 4 2
		f 3 2 4 6
		mu 0 3 0 13 9
		mu 1 3 0 3 5;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".bw" 3;
createNode mesh -n "polySurfaceShape19" -p "Tile6";
	setAttr -k off ".v";
	setAttr ".io" yes;
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.062355432659387589 0.58123928308486938 ;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 18 ".uvst[0].uvsp[0:17]" -type "float2" 0.078245603 0.61554378
		 0.0057035945 0.93884271 0.078245595 0.54693478 0.046465263 0.54693478 0.078245603
		 0.61554378 0.04646527 0.61554378 0.078245595 0.54693478 0.046465263 0.54693478 0.078245603
		 0.61554378 0.078245595 0.54693478 0.04646527 0.61554378 0.046465263 0.54693478 0.04646527
		 0.61554378 0.0057035685 0.99952167 0.046465263 0.54693478 0.11540271 0.93884271 0.1154027
		 0.99952167 0.078245595 0.54693478;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 12 ".uvst[1].uvsp[0:11]" -type "float2" 0.70025212 0.037587702
		 0.80502731 0.34959358 0.70025218 0.34959358 0.70025212 0.14236271 0.35891798 0.3495937
		 0.35891798 0.14236271 0.35891798 0.45436883 0.25414303 0.14236277 0.70025218 0.45436871
		 0.35891798 0.037587643 0.25414303 0.3495937 0.80502731 0.14236277;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  0.0086356355 -0.16091801 -8.90351105 0.98451197 -0.16091801 -8.90351105
		 0.0086356355 -0.048019361 -8.90351105 0.98451197 -0.048019361 -8.90351105 0.0086356355 -0.048019361 -9.49466705
		 0.98451197 -0.048019361 -9.49466705 0.0086356355 -0.16091801 -9.49466705 0.98451197 -0.16091801 -9.49466705;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 5 ".fc[0:4]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 15 1 13 16
		mu 1 4 11 1 2 3
		f 4 1 7 -3 -7
		mu 0 4 2 14 5 4
		mu 1 4 3 2 4 5
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		mu 1 4 5 4 10 7
		f 4 -12 -10 -8 -6
		mu 0 4 12 8 9 3
		mu 1 4 8 6 4 2
		f 4 10 4 6 8
		mu 0 4 10 0 17 11
		mu 1 4 9 0 3 5;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".bw" 3;
createNode transform -n "Tile7";
	setAttr ".rp" -type "double3" -0.63874953531698486 -0.39405664497675419 -9.8760810474720184 ;
	setAttr ".sp" -type "double3" -0.63874953531698486 -0.39405664497675419 -9.8760810474720184 ;
createNode mesh -n "Tile7Shape" -p "Tile7";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.058933477848768234 0.96335703263710881 ;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 16 ".uvst[0].uvsp[0:15]" -type "float2" 0.062355436 0.61554378
		 0.060553156 0.93884271 0.046208482 0.54693478 0.007360138 0.99766159 0.078245603
		 0.61554378 0.031436309 0.61554378 0.062355429 0.54693478 0.058933511 0.92905247 0.11050682
		 0.99766159 0.046465263 0.54693478 0.0057035685 0.99952167 0.031436309 0.54693478
		 0.1154027 0.99952167 0.078245595 0.54693478 0.04646527 0.61554378 0.046208493 0.61554378;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 8 ".uvst[1].uvsp[0:7]" -type "float2" 0.69703376 0.91405529
		 0.70378566 0.90555215 0.70714283 0.90878665 0.70042855 0.90878665 0.70714283 0.91932398
		 0.70042855 0.91932398 0.71053755 0.91405529 0.70378566 0.92255849;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 5 ".vt[0:4]"  -0.62526739 -0.54351538 -9.86456966 -0.77999514 -0.23725304 -9.64259911
		 -0.46906045 -0.21232632 -9.6831274 -0.83540291 -0.27686962 -10.092058182 -0.52446818 -0.25194287 -10.13258648;
	setAttr -s 8 ".ed[0:7]"  1 2 0 3 4 0 0 1 0 0 2 0 1 3 0 2 4 0 3 0 0
		 4 0 0;
	setAttr -s 5 ".fc[0:4]" -type "polyFaces" 
		f 3 3 -1 -3
		mu 0 3 1 10 12
		mu 1 3 1 2 3
		f 4 0 5 -2 -5
		mu 0 4 2 11 5 15
		mu 1 4 3 2 4 5
		f 3 1 7 -7
		mu 0 3 4 14 6
		mu 1 3 5 4 7
		f 3 -8 -6 -4
		mu 0 3 7 8 3
		mu 1 3 6 4 2
		f 3 2 4 6
		mu 0 3 0 13 9
		mu 1 3 0 3 5;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".bw" 3;
createNode mesh -n "polySurfaceShape19" -p "Tile7";
	setAttr -k off ".v";
	setAttr ".io" yes;
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.062355432659387589 0.58123928308486938 ;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 18 ".uvst[0].uvsp[0:17]" -type "float2" 0.078245603 0.61554378
		 0.0057035945 0.93884271 0.078245595 0.54693478 0.046465263 0.54693478 0.078245603
		 0.61554378 0.04646527 0.61554378 0.078245595 0.54693478 0.046465263 0.54693478 0.078245603
		 0.61554378 0.078245595 0.54693478 0.04646527 0.61554378 0.046465263 0.54693478 0.04646527
		 0.61554378 0.0057035685 0.99952167 0.046465263 0.54693478 0.11540271 0.93884271 0.1154027
		 0.99952167 0.078245595 0.54693478;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 12 ".uvst[1].uvsp[0:11]" -type "float2" 0.70025212 0.037587702
		 0.80502731 0.34959358 0.70025218 0.34959358 0.70025212 0.14236271 0.35891798 0.3495937
		 0.35891798 0.14236271 0.35891798 0.45436883 0.25414303 0.14236277 0.70025218 0.45436871
		 0.35891798 0.037587643 0.25414303 0.3495937 0.80502731 0.14236277;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  0.0086356355 -0.16091801 -8.90351105 0.98451197 -0.16091801 -8.90351105
		 0.0086356355 -0.048019361 -8.90351105 0.98451197 -0.048019361 -8.90351105 0.0086356355 -0.048019361 -9.49466705
		 0.98451197 -0.048019361 -9.49466705 0.0086356355 -0.16091801 -9.49466705 0.98451197 -0.16091801 -9.49466705;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 5 ".fc[0:4]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 15 1 13 16
		mu 1 4 11 1 2 3
		f 4 1 7 -3 -7
		mu 0 4 2 14 5 4
		mu 1 4 3 2 4 5
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		mu 1 4 5 4 10 7
		f 4 -12 -10 -8 -6
		mu 0 4 12 8 9 3
		mu 1 4 8 6 4 2
		f 4 10 4 6 8
		mu 0 4 10 0 17 11
		mu 1 4 9 0 3 5;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".bw" 3;
createNode mesh -n "polySurfaceShape21" -p "Tile7";
	setAttr -k off ".v";
	setAttr ".io" yes;
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.038822401314973831 0.58123928308486938 ;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 20 ".uvst[0].uvsp[0:19]" -type "float2" 0.078245603 0.61554378
		 0.0057035945 0.93884271 0.046208482 0.54693478 0.046465263 0.54693478 0.078245603
		 0.61554378 0.031436309 0.61554378 0.078245595 0.54693478 0.046465263 0.54693478 0.078245603
		 0.61554378 0.078245595 0.54693478 0.04646527 0.61554378 0.046465263 0.54693478 0.04646527
		 0.61554378 0.0057035685 0.99952167 0.031436309 0.54693478 0.11540271 0.93884271 0.1154027
		 0.99952167 0.078245595 0.54693478 0.04646527 0.61554378 0.046208493 0.61554378;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 12 ".uvst[1].uvsp[0:11]" -type "float2" 0.70025212 0.037587702
		 0.80502731 0.34959358 0.70025218 0.34959358 0.70025212 0.14236271 0.35891798 0.3495937
		 0.35891798 0.14236271 0.35891798 0.45436883 0.25414303 0.14236277 0.70025218 0.45436871
		 0.35891798 0.037587643 0.25414303 0.3495937 0.80502731 0.14236277;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 5 ".pt";
	setAttr ".pt[0:2]" -type "float3" 0.48793817 0 -0.295578  -0.48793817 
		0 -0.295578  0 0 0 ;
	setAttr ".pt[6:7]" -type "float3" 0.48793817 0 0.29557741  -0.48793817 
		0 0.29557741 ;
	setAttr -s 8 ".vt[0:7]"  0.0086356355 -0.16091801 -8.90351105 0.98451197 -0.16091801 -8.90351105
		 0.0086356355 -0.048019361 -8.90351105 0.98451197 -0.048019361 -8.90351105 0.0086356355 -0.048019361 -9.49466705
		 0.98451197 -0.048019361 -9.49466705 0.0086356355 -0.16091801 -9.49466705 0.98451197 -0.16091801 -9.49466705;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 5 ".fc[0:4]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 15 1 13 16
		mu 1 4 11 1 2 3
		f 4 1 7 -3 -7
		mu 0 4 2 14 5 19
		mu 1 4 3 2 4 5
		f 4 2 9 -4 -9
		mu 0 4 4 18 7 6
		mu 1 4 5 4 10 7
		f 4 -12 -10 -8 -6
		mu 0 4 12 8 9 3
		mu 1 4 8 6 4 2
		f 4 10 4 6 8
		mu 0 4 10 0 17 11
		mu 1 4 9 0 3 5;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".bw" 3;
createNode transform -n "Tile8";
	setAttr ".rp" -type "double3" -0.43441250585344149 -0.19694088239469282 -9.4184977576680939 ;
	setAttr ".sp" -type "double3" -0.43441250585344138 -0.19694088239469282 -9.4184977576680939 ;
createNode mesh -n "Tile8Shape" -p "Tile8";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.060553133487701416 0.7732282280921936 ;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 16 ".uvst[0].uvsp[0:15]" -type "float2" 0.062355436 0.61554378
		 0.060553156 0.93884271 0.046208482 0.54693478 0.0075608306 0.99513167 0.078245603
		 0.61554378 0.031436309 0.61554378 0.062355429 0.54693478 0.059396576 0.92652267 0.11123227
		 0.99513167 0.046465263 0.54693478 0.0057035685 0.99952167 0.031436309 0.54693478
		 0.1154027 0.99952167 0.078245595 0.54693478 0.04646527 0.61554378 0.046208493 0.61554378;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 8 ".uvst[1].uvsp[0:7]" -type "float2" 0.72494304 0.97117919
		 0.73034549 0.96437538 0.73303175 0.96696347 0.72765929 0.96696347 0.73303175 0.9753949
		 0.72765929 0.9753949 0.73574805 0.97117919 0.73034549 0.977983;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 5 ".vt[0:4]"  -0.42427665 -0.30930358 -9.40984344 -0.5406006 -0.079056308 -9.24296665
		 -0.3068406 -0.06031644 -9.27343559 -0.58225608 -0.10883995 -9.58086872 -0.34849602 -0.09010008 -9.61133766;
	setAttr -s 8 ".ed[0:7]"  1 2 0 3 4 0 0 1 0 0 2 0 1 3 0 2 4 0 3 0 0
		 4 0 0;
	setAttr -s 5 ".fc[0:4]" -type "polyFaces" 
		f 3 3 -1 -3
		mu 0 3 1 10 12
		mu 1 3 1 2 3
		f 4 0 5 -2 -5
		mu 0 4 2 11 5 15
		mu 1 4 3 2 4 5
		f 3 1 7 -7
		mu 0 3 4 14 6
		mu 1 3 5 4 7
		f 3 -8 -6 -4
		mu 0 3 7 8 3
		mu 1 3 6 4 2
		f 3 2 4 6
		mu 0 3 0 13 9
		mu 1 3 0 3 5;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".bw" 3;
createNode mesh -n "polySurfaceShape19" -p "Tile8";
	setAttr -k off ".v";
	setAttr ".io" yes;
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.062355432659387589 0.58123928308486938 ;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 18 ".uvst[0].uvsp[0:17]" -type "float2" 0.078245603 0.61554378
		 0.0057035945 0.93884271 0.078245595 0.54693478 0.046465263 0.54693478 0.078245603
		 0.61554378 0.04646527 0.61554378 0.078245595 0.54693478 0.046465263 0.54693478 0.078245603
		 0.61554378 0.078245595 0.54693478 0.04646527 0.61554378 0.046465263 0.54693478 0.04646527
		 0.61554378 0.0057035685 0.99952167 0.046465263 0.54693478 0.11540271 0.93884271 0.1154027
		 0.99952167 0.078245595 0.54693478;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 12 ".uvst[1].uvsp[0:11]" -type "float2" 0.70025212 0.037587702
		 0.80502731 0.34959358 0.70025218 0.34959358 0.70025212 0.14236271 0.35891798 0.3495937
		 0.35891798 0.14236271 0.35891798 0.45436883 0.25414303 0.14236277 0.70025218 0.45436871
		 0.35891798 0.037587643 0.25414303 0.3495937 0.80502731 0.14236277;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  0.0086356355 -0.16091801 -8.90351105 0.98451197 -0.16091801 -8.90351105
		 0.0086356355 -0.048019361 -8.90351105 0.98451197 -0.048019361 -8.90351105 0.0086356355 -0.048019361 -9.49466705
		 0.98451197 -0.048019361 -9.49466705 0.0086356355 -0.16091801 -9.49466705 0.98451197 -0.16091801 -9.49466705;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 5 ".fc[0:4]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 15 1 13 16
		mu 1 4 11 1 2 3
		f 4 1 7 -3 -7
		mu 0 4 2 14 5 4
		mu 1 4 3 2 4 5
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		mu 1 4 5 4 10 7
		f 4 -12 -10 -8 -6
		mu 0 4 12 8 9 3
		mu 1 4 8 6 4 2
		f 4 10 4 6 8
		mu 0 4 10 0 17 11
		mu 1 4 9 0 3 5;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".bw" 3;
createNode mesh -n "polySurfaceShape21" -p "Tile8";
	setAttr -k off ".v";
	setAttr ".io" yes;
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.038822401314973831 0.58123928308486938 ;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 20 ".uvst[0].uvsp[0:19]" -type "float2" 0.078245603 0.61554378
		 0.0057035945 0.93884271 0.046208482 0.54693478 0.046465263 0.54693478 0.078245603
		 0.61554378 0.031436309 0.61554378 0.078245595 0.54693478 0.046465263 0.54693478 0.078245603
		 0.61554378 0.078245595 0.54693478 0.04646527 0.61554378 0.046465263 0.54693478 0.04646527
		 0.61554378 0.0057035685 0.99952167 0.031436309 0.54693478 0.11540271 0.93884271 0.1154027
		 0.99952167 0.078245595 0.54693478 0.04646527 0.61554378 0.046208493 0.61554378;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 12 ".uvst[1].uvsp[0:11]" -type "float2" 0.70025212 0.037587702
		 0.80502731 0.34959358 0.70025218 0.34959358 0.70025212 0.14236271 0.35891798 0.3495937
		 0.35891798 0.14236271 0.35891798 0.45436883 0.25414303 0.14236277 0.70025218 0.45436871
		 0.35891798 0.037587643 0.25414303 0.3495937 0.80502731 0.14236277;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 5 ".pt";
	setAttr ".pt[0:2]" -type "float3" 0.48793817 0 -0.295578  -0.48793817 
		0 -0.295578  0 0 0 ;
	setAttr ".pt[6:7]" -type "float3" 0.48793817 0 0.29557741  -0.48793817 
		0 0.29557741 ;
	setAttr -s 8 ".vt[0:7]"  0.0086356355 -0.16091801 -8.90351105 0.98451197 -0.16091801 -8.90351105
		 0.0086356355 -0.048019361 -8.90351105 0.98451197 -0.048019361 -8.90351105 0.0086356355 -0.048019361 -9.49466705
		 0.98451197 -0.048019361 -9.49466705 0.0086356355 -0.16091801 -9.49466705 0.98451197 -0.16091801 -9.49466705;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 5 ".fc[0:4]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 15 1 13 16
		mu 1 4 11 1 2 3
		f 4 1 7 -3 -7
		mu 0 4 2 14 5 19
		mu 1 4 3 2 4 5
		f 4 2 9 -4 -9
		mu 0 4 4 18 7 6
		mu 1 4 5 4 10 7
		f 4 -12 -10 -8 -6
		mu 0 4 12 8 9 3
		mu 1 4 8 6 4 2
		f 4 10 4 6 8
		mu 0 4 10 0 17 11
		mu 1 4 9 0 3 5;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".bw" 3;
createNode transform -n "Tile9";
	setAttr ".rp" -type "double3" 0.36494086653251828 -0.042912070153391824 -9.9327001998163524 ;
	setAttr ".sp" -type "double3" 0.36494086653251828 -0.042912070153391824 -9.9327001998163524 ;
createNode mesh -n "Tile9Shape" -p "Tile9";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.06235542893409729 0.95879447460174561 ;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 16 ".uvst[0].uvsp[0:15]" -type "float2" 0.062355436 0.61554378
		 0.060553156 0.93884271 0.046208482 0.54693478 0.0139306 0.99309897 0.078245603 0.61554378
		 0.031436309 0.61554378 0.062355429 0.54693478 0.062355451 0.92448997 0.11078025 0.99309897
		 0.046465263 0.54693478 0.0057035685 0.99952167 0.031436309 0.54693478 0.1154027 0.99952167
		 0.078245595 0.54693478 0.04646527 0.61554378 0.046208493 0.61554378;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 8 ".uvst[1].uvsp[0:7]" -type "float2" 0.7055068 0.99228531
		 0.70972896 0.98696798 0.71182847 0.9889906 0.70762968 0.9889906 0.71182847 0.99558002
		 0.70762968 0.99558002 0.71395129 0.99228531 0.70972896 0.9976027;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 5 ".pt[0:4]" -type "float3"  -0.11389664 -0.0407627 -0.72739929 
		0.22768417 0.010751889 -1.1188989 -0.73502731 0.025789147 -0.93516725 0.49390969 
		0.019254753 -0.54751498 -0.4688018 0.034292039 -0.36378327;
	setAttr -s 5 ".vt[0:4]"  0.49657381 -0.16091801 -9.19908905 0.0086356355 -0.048019361 -8.90351105
		 0.98451197 -0.048019361 -8.90351105 0.0086356355 -0.048019361 -9.49466705 0.98451197 -0.048019361 -9.49466705;
	setAttr -s 8 ".ed[0:7]"  1 2 0 3 4 0 0 1 0 0 2 0 1 3 0 2 4 0 3 0 0
		 4 0 0;
	setAttr -s 5 ".fc[0:4]" -type "polyFaces" 
		f 3 3 -1 -3
		mu 0 3 1 10 12
		mu 1 3 1 2 3
		f 4 0 5 -2 -5
		mu 0 4 2 11 5 15
		mu 1 4 3 2 4 5
		f 3 1 7 -7
		mu 0 3 4 14 6
		mu 1 3 5 4 7
		f 3 -8 -6 -4
		mu 0 3 7 8 3
		mu 1 3 6 4 2
		f 3 2 4 6
		mu 0 3 0 13 9
		mu 1 3 0 3 5;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".bw" 3;
createNode mesh -n "polySurfaceShape19" -p "Tile9";
	setAttr -k off ".v";
	setAttr ".io" yes;
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.062355432659387589 0.58123928308486938 ;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 18 ".uvst[0].uvsp[0:17]" -type "float2" 0.078245603 0.61554378
		 0.0057035945 0.93884271 0.078245595 0.54693478 0.046465263 0.54693478 0.078245603
		 0.61554378 0.04646527 0.61554378 0.078245595 0.54693478 0.046465263 0.54693478 0.078245603
		 0.61554378 0.078245595 0.54693478 0.04646527 0.61554378 0.046465263 0.54693478 0.04646527
		 0.61554378 0.0057035685 0.99952167 0.046465263 0.54693478 0.11540271 0.93884271 0.1154027
		 0.99952167 0.078245595 0.54693478;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 12 ".uvst[1].uvsp[0:11]" -type "float2" 0.70025212 0.037587702
		 0.80502731 0.34959358 0.70025218 0.34959358 0.70025212 0.14236271 0.35891798 0.3495937
		 0.35891798 0.14236271 0.35891798 0.45436883 0.25414303 0.14236277 0.70025218 0.45436871
		 0.35891798 0.037587643 0.25414303 0.3495937 0.80502731 0.14236277;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  0.0086356355 -0.16091801 -8.90351105 0.98451197 -0.16091801 -8.90351105
		 0.0086356355 -0.048019361 -8.90351105 0.98451197 -0.048019361 -8.90351105 0.0086356355 -0.048019361 -9.49466705
		 0.98451197 -0.048019361 -9.49466705 0.0086356355 -0.16091801 -9.49466705 0.98451197 -0.16091801 -9.49466705;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 5 ".fc[0:4]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 15 1 13 16
		mu 1 4 11 1 2 3
		f 4 1 7 -3 -7
		mu 0 4 2 14 5 4
		mu 1 4 3 2 4 5
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		mu 1 4 5 4 10 7
		f 4 -12 -10 -8 -6
		mu 0 4 12 8 9 3
		mu 1 4 8 6 4 2
		f 4 10 4 6 8
		mu 0 4 10 0 17 11
		mu 1 4 9 0 3 5;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".bw" 3;
createNode mesh -n "polySurfaceShape21" -p "Tile9";
	setAttr -k off ".v";
	setAttr ".io" yes;
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.038822401314973831 0.58123928308486938 ;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 20 ".uvst[0].uvsp[0:19]" -type "float2" 0.078245603 0.61554378
		 0.0057035945 0.93884271 0.046208482 0.54693478 0.046465263 0.54693478 0.078245603
		 0.61554378 0.031436309 0.61554378 0.078245595 0.54693478 0.046465263 0.54693478 0.078245603
		 0.61554378 0.078245595 0.54693478 0.04646527 0.61554378 0.046465263 0.54693478 0.04646527
		 0.61554378 0.0057035685 0.99952167 0.031436309 0.54693478 0.11540271 0.93884271 0.1154027
		 0.99952167 0.078245595 0.54693478 0.04646527 0.61554378 0.046208493 0.61554378;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 12 ".uvst[1].uvsp[0:11]" -type "float2" 0.70025212 0.037587702
		 0.80502731 0.34959358 0.70025218 0.34959358 0.70025212 0.14236271 0.35891798 0.3495937
		 0.35891798 0.14236271 0.35891798 0.45436883 0.25414303 0.14236277 0.70025218 0.45436871
		 0.35891798 0.037587643 0.25414303 0.3495937 0.80502731 0.14236277;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 5 ".pt";
	setAttr ".pt[0:2]" -type "float3" 0.48793817 0 -0.295578  -0.48793817 
		0 -0.295578  0 0 0 ;
	setAttr ".pt[6:7]" -type "float3" 0.48793817 0 0.29557741  -0.48793817 
		0 0.29557741 ;
	setAttr -s 8 ".vt[0:7]"  0.0086356355 -0.16091801 -8.90351105 0.98451197 -0.16091801 -8.90351105
		 0.0086356355 -0.048019361 -8.90351105 0.98451197 -0.048019361 -8.90351105 0.0086356355 -0.048019361 -9.49466705
		 0.98451197 -0.048019361 -9.49466705 0.0086356355 -0.16091801 -9.49466705 0.98451197 -0.16091801 -9.49466705;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 5 ".fc[0:4]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 15 1 13 16
		mu 1 4 11 1 2 3
		f 4 1 7 -3 -7
		mu 0 4 2 14 5 19
		mu 1 4 3 2 4 5
		f 4 2 9 -4 -9
		mu 0 4 4 18 7 6
		mu 1 4 5 4 10 7
		f 4 -12 -10 -8 -6
		mu 0 4 12 8 9 3
		mu 1 4 8 6 4 2
		f 4 10 4 6 8
		mu 0 4 10 0 17 11
		mu 1 4 9 0 3 5;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".bw" 3;
createNode transform -n "oz_ec_narrows_straight_over_anim_a";
createNode mesh -n "oz_ec_narrows_straight_over_anim_aShape" -p "oz_ec_narrows_straight_over_anim_a";
	setAttr -k off ".v";
	setAttr ".iog[0].og[0].gcl" -type "componentList" 1 "f[0:219]";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.67232988102352176 0.98603875912010852 ;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 600 ".uvst[0].uvsp";
	setAttr ".uvst[0].uvsp[0:249]" -type "float2" 0.20345408 0.83969557 0.23536199
		 0.83951503 0.23514533 0.80162114 0.20345408 0.80162114 0.20345408 0.83969557 0.23536199
		 0.83951503 0.23514533 0.80162114 0.20345408 0.80162114 0.20345408 0.83969557 0.23536199
		 0.83951503 0.23514533 0.80162114 0.20345408 0.80162114 0.078749768 0.546417 0.078749768
		 0.75338376 0.00077822432 0.75338376 0.00077822432 0.546417 0.079004765 0.50057942
		 0.079004765 0.61592925 0.00081756897 0.61592925 0.00080335326 0.50057942 0.078749768
		 0.546417 0.078749768 0.75338334 0.00068993308 0.75338334 0.17872077 0.77761114 0.079374336
		 0.61592919 0.079374336 0.50057948 0.00082508102 0.50057948 0.00082508102 0.61592919
		 0.17872074 0.77761126 0.0011463463 0.7776112 0.0011463463 0.79120326 0.17872077 0.79120326
		 0.17872074 0.77761126 0.0011463463 0.77761114 0.0011463463 0.79120326 0.17872077
		 0.79120326 0.0011463463 0.7912032 0.18074788 0.8117637 0.0011463463 0.7776112 0.17872074
		 0.77761126 0.17872077 0.79120326 0.0011463463 0.79120326 0.18074788 0.8117637 0.0035595745
		 0.8117637 0.0035595745 0.80754775 0.18074791 0.80754781 0.18074788 0.81176341 0.0035595745
		 0.81176353 0.0035595745 0.80754757 0.18074788 0.80754751 0.0035595745 0.80754775
		 0.17872074 0.79120314 0.0035595745 0.81176376 0.18074788 0.8117637 0.18074791 0.80754781
		 0.0035595745 0.80754793 0.17872074 0.79120314 0.0011463463 0.79120314 0.0011463463
		 0.7776112 0.17872077 0.77761102 0.17872074 0.79120314 0.0011463463 0.79120314 0.0011463463
		 0.7776112 0.17872077 0.77761102 0.0011463463 0.7776112 0.17872077 0.77761102 0.0011463463
		 0.79120314 0.17872074 0.79120314 0.17872077 0.77761102 0.0011463463 0.77761126 0.078866825
		 0.546417 0.00077822432 0.546417 0.00077822432 0.75338376 0.078866825 0.75338376 0.079004765
		 0.50057942 0.00060352311 0.50057942 0.00061776862 0.61592925 0.079004765 0.61592925
		 0.078866825 0.546417 0.00068993121 0.546417 0.00068993121 0.75338334 0.078866825
		 0.75338334 0.079491384 0.61592919 0.00082508102 0.61592919 0.00082508102 0.50057948
		 0.079491384 0.50057948 0.17872074 0.77761126 0.17872077 0.79120326 0.0011463463 0.79120326
		 0.0011463463 0.7776112 0.17872074 0.77761126 0.17872077 0.79120326 0.0011463463 0.79120326
		 0.0011463463 0.77761114 0.17872074 0.77761126 0.17872077 0.79120326 0.0011463463
		 0.79120326 0.0011463463 0.7776112 0.0011463463 0.7776112 0.0011463463 0.79120326
		 0.17872077 0.79120326 0.17872074 0.77761126 0.18074788 0.8117637 0.18074791 0.80754781
		 0.0035595745 0.80754769 0.0035595745 0.8117637 0.18074788 0.8117637 0.18074788 0.80754781
		 0.0035595745 0.80754793 0.0035595745 0.81176376 0.18074788 0.8117637 0.18074791 0.80754781
		 0.0035595745 0.80754769 0.0035595745 0.8117637 0.0035595745 0.81176353 0.0035595745
		 0.80754769 0.18074791 0.80754751 0.18074788 0.81176341 0.17872074 0.79120314 0.17872077
		 0.77761114 0.0011463463 0.7776112 0.0011463463 0.7912032 0.17872074 0.79120314 0.17872077
		 0.77761102 0.0011463463 0.77761114 0.0011463463 0.79120314 0.17872074 0.79120314
		 0.17872077 0.77761114 0.0011463463 0.7776112 0.0011463463 0.7912032 0.0011463463
		 0.79120314 0.0011463463 0.77761114 0.17872077 0.77761102 0.17872074 0.79120314 0.20345408
		 0.83969557 0.23536199 0.83951503 0.23514533 0.80162114 0.20345408 0.80162114 0.20345408
		 0.83969557 0.23536199 0.83951503 0.23514533 0.80162114 0.20345408 0.80162114 0.20345408
		 0.83969557 0.23536199 0.83951503 0.23514533 0.80162114 0.20345408 0.80162114 0.20345408
		 0.83969557 0.23536199 0.83951503 0.23514533 0.80162114 0.20345408 0.80162114 0.20345408
		 0.83969557 0.23536199 0.83951503 0.23514533 0.80162114 0.20345408 0.80162114 0.42565176
		 0.64673084 0.44144982 0.64673084 0.44144982 0.5005278 0.42565176 0.5005278 0.43170506
		 0.64673084 0.41369635 0.64673084 0.41369635 0.5005278 0.43170506 0.5005278 0.42565176
		 0.64673084 0.39179674 0.64673084 0.39184952 0.5005278 0.42565176 0.5005278 0.44144982
		 0.64673084 0.42565176 0.64673084 0.42565176 0.5005278 0.44144982 0.5005278 0.41369635
		 0.64673084 0.39568797 0.64673084 0.39568797 0.5005278 0.41369635 0.5005278 0.44367859
		 0.64673084 0.44365469 0.5005278 0.41484985 0.5005278 0.41488266 0.64673084 0.43378019
		 0.64673084 0.41376007 0.64673084 0.41376007 0.5005278 0.43378019 0.5005278 0.42529088
		 0.64673084 0.44454738 0.64673084 0.44454738 0.5005278 0.42529088 0.5005278 0.41492105
		 0.64673084 0.44697604 0.64673084 0.44694355 0.5005278 0.41487655 0.5005278 0.42529088
		 0.64673084 0.3843022 0.64673084 0.38438064 0.5005278 0.42529088 0.5005278 0.44454738
		 0.64673084 0.42529088 0.64673084 0.42529088 0.5005278 0.44454738 0.5005278 0.41376007
		 0.64673084 0.39374012 0.64673084 0.39374012 0.5005278 0.41376007 0.5005278 0.43186766
		 0.64673084 0.41379344 0.64673084 0.41379344 0.5005278 0.43186766 0.5005278 0.42407084
		 0.64673084 0.44155207 0.64673084 0.44155207 0.5005278 0.42407084 0.5005278 0.41491961
		 0.64673084 0.44383761 0.64673084 0.44380948 0.5005278 0.41488102 0.5005278 0.42407084
		 0.64673084 0.38672274 0.64673084 0.38679105 0.5005278 0.42407084 0.5005278 0.44155207
		 0.64673084 0.42407084 0.64673084 0.42407084 0.5005278 0.44155207 0.5005278 0.41379344
		 0.64673084 0.39571947 0.64673084 0.39571947 0.5005278 0.41379344 0.5005278 0.43378019
		 0.64673084 0.41376007 0.64673084 0.41376007 0.5005278 0.43378019 0.5005278 0.42529088
		 0.64673084 0.44454738 0.64673084 0.44454738 0.5005278 0.42529088 0.5005278 0.41483042
		 0.64673084 0.44690987 0.64673084 0.44687641 0.5005278 0.41478455 0.5005278 0.42529088
		 0.64673084 0.38446188 0.64673084 0.38454255 0.5005278 0.42529088 0.5005278 0.44454738
		 0.64673084 0.42529088 0.64673084 0.42529088 0.5005278 0.44454738 0.5005278 0.41376007
		 0.64673084 0.39374012 0.64673084 0.39374012 0.5005278 0.41376007 0.5005278;
	setAttr ".uvst[0].uvsp[250:499]" 0.20345408 0.83969557 0.21841386 0.82146931
		 0.23536199 0.83951503 0.21841386 0.82146931 0.23514533 0.80162114 0.23536199 0.83951503
		 0.21841386 0.82146931 0.20345408 0.83969557 0.23536199 0.83951503 0.21841386 0.82146931
		 0.23514533 0.80162114 0.23536199 0.83951503 0.20345408 0.83969557 0.21841386 0.82146931
		 0.23536199 0.83951503 0.21841386 0.82146931 0.23514533 0.80162114 0.23536199 0.83951503
		 0.20345408 0.83969557 0.21841386 0.82146931 0.23536199 0.83951503 0.21841386 0.82146931
		 0.23514533 0.80162114 0.23536199 0.83951503 0.21841386 0.82146931 0.20345408 0.83969557
		 0.23536199 0.83951503 0.21841386 0.82146931 0.23514533 0.80162114 0.23536199 0.83951503
		 0.20345408 0.83969557 0.21841386 0.82146931 0.23536199 0.83951503 0.21841386 0.82146931
		 0.23514533 0.80162114 0.23536199 0.83951503 0.23514527 0.80162114 0.20345408 0.83969557
		 0.20345408 0.80162114 0.20345408 0.83969557 0.23514527 0.80162114 0.20345408 0.80162114
		 0.23514533 0.80162114 0.20345408 0.83969557 0.20345408 0.80162114 0.20345408 0.83969557
		 0.23514533 0.80162114 0.20345408 0.80162114 0.23514533 0.80162114 0.20345408 0.83969557
		 0.20345408 0.80162114 0.23514527 0.80162114 0.20345408 0.83969557 0.20345408 0.80162114
		 0.20345408 0.83969557 0.23514527 0.80162114 0.20345408 0.80162114 0.23514533 0.80162114
		 0.20345408 0.83969557 0.20345408 0.80162114 0.20345408 0.83969557 0.23514533 0.80162114
		 0.20345408 0.80162114 0.23514527 0.80162114 0.20345408 0.83969557 0.20345408 0.80162114
		 0.41369635 0.64673084 0.39568797 0.64673084 0.39568797 0.5005278 0.41369635 0.5005278
		 0.43170506 0.64673084 0.41369635 0.64673084 0.41369635 0.5005278 0.43170506 0.5005278
		 0.42565176 0.64673084 0.44144982 0.64673084 0.44144982 0.5005278 0.42565176 0.5005278
		 0.41484985 0.64673084 0.44365469 0.64673084 0.44363198 0.5005278 0.41481853 0.5005278
		 0.42565176 0.64673084 0.39184952 0.64673084 0.39189965 0.5005278 0.42565176 0.5005278
		 0.44144982 0.64673084 0.42565176 0.64673084 0.42565176 0.5005278 0.44144982 0.5005278
		 0.41379344 0.64673084 0.39571947 0.64673084 0.39571947 0.5005278 0.41379344 0.5005278
		 0.43186766 0.64673084 0.41379344 0.64673084 0.41379344 0.5005278 0.43186766 0.5005278
		 0.42407084 0.64673084 0.44155207 0.64673084 0.44155207 0.5005278 0.42407084 0.5005278
		 0.41488102 0.64673084 0.44380948 0.64673084 0.44378078 0.5005278 0.41484162 0.5005278
		 0.42407084 0.64673084 0.38679105 0.64673084 0.38686103 0.5005278 0.42407084 0.5005278
		 0.44155207 0.64673084 0.42407084 0.64673084 0.42407084 0.5005278 0.44155207 0.5005278
		 0.43378019 0.64673084 0.41376007 0.64673084 0.41376007 0.5005278 0.43378019 0.5005278
		 0.42529088 0.64673084 0.44454738 0.64673084 0.44454738 0.5005278 0.42529088 0.5005278
		 0.41487655 0.64673084 0.44694355 0.64673084 0.44690987 0.5005278 0.41483042 0.5005278
		 0.42529088 0.64673084 0.38438064 0.64673084 0.38446188 0.5005278 0.42529088 0.5005278
		 0.44454738 0.64673084 0.42529088 0.64673084 0.42529088 0.5005278 0.44454738 0.5005278
		 0.41376007 0.64673084 0.39374012 0.64673084 0.39374012 0.5005278 0.41376007 0.5005278
		 0.092223823 0.99973059 0.058650315 0.99973059 0.058650315 0.88149524 0.092223823
		 0.88149524 0.077987373 0.99973059 0.11028025 0.99973059 0.11028025 0.88149524 0.077987373
		 0.88149524 0.060368389 0.99973059 0.11418599 0.99973059 0.11413035 0.88149524 0.060292065
		 0.88149524 0.077987373 0.99973059 0.0096529424 0.99973059 0.0097874999 0.88149524
		 0.077987373 0.88149524 0.11028025 0.99973059 0.077987373 0.99973059 0.077987373 0.88149524
		 0.11028025 0.88149524 0.058650315 0.99973059 0.025077134 0.99973059 0.025077134 0.88149524
		 0.058650315 0.88149524 0.0032688081 0.99973059 0.0031706989 0.88149524 0.38079467
		 0.64673084 0.38073564 0.5005278 0.38085401 0.64673084 0.38079467 0.5005278 0.38091135
		 0.64673084 0.38085401 0.5005278 0.23514533 0.80162114 0.3841885 0.64673084 0.38413769
		 0.5005278 0.38423821 0.64673084 0.3841885 0.5005278 0.23514533 0.80162114 0.3842884
		 0.64673084 0.38424802 0.5005278 0.3842884 0.5005278 0.38433075 0.64673084 0.23514527
		 0.80162114 0.20345408 0.83969557 0.20345408 0.80162114 0.20345408 0.83969557 0.20345408
		 0.80162114 0.23514527 0.80162114 0.20345408 0.83969557 0.20345408 0.80162114 0.20345408
		 0.83969557 0.20345408 0.80162114 0.24209398 0.84586161 0.26665029 0.84586161 0.26665029
		 0.86600816 0.24209398 0.86600816 0.26500842 0.81111383 0.26500842 0.82537508 0.24375062
		 0.82537508 0.24375062 0.81111383 0.23503509 0.84007019 0.27370924 0.84007019 0.27049395
		 0.84270811 0.23825027 0.84270811 0.27370924 0.87179923 0.27049407 0.86916155 0.23503509
		 0.87179923 0.23825027 0.86916131 0.27045897 0.84273702 0.2382853 0.84273702 0.27045897
		 0.86913276 0.2382853 0.86913258 0.20339243 0.84014034 0.19644615 0.81603026 0.20337097
		 0.80149502 0.27370924 0.87874436 0.23503517 0.87874436 0.27366793 0.80142528 0.30534545
		 0.80142528 0.3053239 0.84005857 0.27364445 0.84005857 0.2736181 0.80148441 0.2736181
		 0.76298994 0.23512134 0.76298994 0.23512134 0.80148441 0.23505799 0.80149502 0.23508137
		 0.84014034 0.31226796 0.81595618 0.23948829 0.8437236 0.23948829 0.86814582 0.26925614
		 0.86814582 0.26925614 0.8437236 0.27369398 0.80146307 0.27369398 0.84009188 0.27132589
		 0.83772391 0.27132589 0.80383104 0.235065 0.84009188 0.23743297 0.83772391 0.235065
		 0.80146307 0.23743297 0.80383104 0.27132589 0.83772391 0.27132589 0.80383104 0.23743297
		 0.83772391 0.23743297 0.80383104 0.2686646 0.82878041 0.2686646 0.80770844 0.24009453
		 0.82878041 0.24009453 0.80770844 0.26585811 0.82671076 0.26585811 0.80977845 0.24290106
		 0.82671076 0.24290106 0.80977845;
	setAttr ".uvst[0].uvsp[500:599]" 0.24209398 0.84586161 0.24209398 0.86600816
		 0.26665029 0.86600816 0.26665029 0.84586161 0.26500842 0.81111383 0.24375062 0.81111383
		 0.24375062 0.82537508 0.26500842 0.82537508 0.23503509 0.84007019 0.23825027 0.84270811
		 0.27049395 0.84270811 0.27370924 0.84007019 0.27049407 0.86916155 0.27370924 0.87179923
		 0.23825027 0.86916131 0.23503509 0.87179923 0.2382853 0.84273702 0.27045897 0.84273702
		 0.27045897 0.86913276 0.2382853 0.86913258 0.20339243 0.84014034 0.20337097 0.80149502
		 0.19644615 0.81603026 0.23503517 0.87874436 0.27370924 0.87874436 0.27366793 0.80142528
		 0.27364445 0.84005857 0.3053239 0.84005857 0.30534545 0.80142528 0.2736181 0.80148441
		 0.23512134 0.80148441 0.23512134 0.76298994 0.2736181 0.76298994 0.23508137 0.84014034
		 0.23505799 0.80149502 0.31226796 0.81595618 0.23948829 0.86814582 0.23948829 0.8437236
		 0.26925614 0.86814582 0.26925614 0.8437236 0.27369398 0.80146307 0.27132589 0.80383104
		 0.27132589 0.83772391 0.27369398 0.84009188 0.23743297 0.83772391 0.235065 0.84009188
		 0.23743297 0.80383104 0.235065 0.80146307 0.27132589 0.80383104 0.27132589 0.83772391
		 0.23743297 0.83772391 0.23743297 0.80383104 0.2686646 0.80770844 0.2686646 0.82878041
		 0.24009453 0.82878041 0.24009453 0.80770844 0.26585811 0.80977845 0.26585811 0.82671076
		 0.24290106 0.82671076 0.24290106 0.80977845 0.18074788 0.80754781 0.0011463463 0.79120314
		 0.17872074 0.79120314 0.0035595745 0.8117637 0.00068993308 0.546417 0.0011463463
		 0.77761114 0.011290036 0.83020085 0.17976665 0.83020085 0.17976665 0.85021383 0.011290036
		 0.85021383 0.011290036 0.83020085 0.17976665 0.83020085 0.17976665 0.85021383 0.011290036
		 0.85021383 0.011290036 0.83020085 0.17976665 0.83020085 0.17976665 0.85021383 0.011290036
		 0.85021383 0.011290036 0.83020085 0.17976665 0.83020085 0.17976665 0.85021383 0.011290036
		 0.85021383 0.011290036 0.83020085 0.17976665 0.83020085 0.17976665 0.85021383 0.011290036
		 0.85021383 0.011290036 0.83020085 0.17976665 0.83020085 0.17976665 0.85021383 0.011290036
		 0.85021383 0.011290036 0.83020085 0.17976665 0.83020085 0.17976665 0.85021383 0.011290036
		 0.85021383 0.011290036 0.83020085 0.17976665 0.83020085 0.17976665 0.85021383 0.011290036
		 0.85021383 0.060292065 0.88149524 0.060368389 0.99973059;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 422 ".uvst[1].uvsp";
	setAttr ".uvst[1].uvsp[0:249]" -type "float2" 0.7191636 0.94229156 0.7191636
		 0.95043463 0.7102952 0.95043463 0.7102952 0.94229156 0.70358104 0.99710381 0.6953088
		 0.99710381 0.70358104 0.98863482 0.6953088 0.98863482 0.7191636 0.95857775 0.7102952
		 0.95857775 0.66452467 0.95551586 0.66452467 0.98141384 0.65142405 0.98141384 0.65142179
		 0.95551586 0.66452467 0.90372002 0.66452467 0.92961794 0.65141892 0.92961794 0.65141702
		 0.90372002 0.66452467 0.8778221 0.65141475 0.8778221 0.65099156 0.98141384 0.65098917
		 0.95551586 0.65098637 0.92961794 0.65098453 0.90372002 0.6509822 0.8778221 0.64868695
		 0.98141384 0.64868456 0.95551586 0.64868176 0.92961794 0.64867985 0.90372002 0.64867753
		 0.8778221 0.63460892 0.96379608 0.63460636 0.99252766 0.6321345 0.99252766 0.63213706
		 0.96379608 0.63461411 0.90633202 0.63461196 0.9350636 0.63214052 0.9350636 0.63214225
		 0.90633202 0.63461673 0.87759995 0.63214481 0.87759995 0.67762762 0.95551586 0.6776253
		 0.98141384 0.67763239 0.90372002 0.67763042 0.92961794 0.67763472 0.8778221 0.67806023
		 0.95551586 0.67805791 0.98141384 0.67806494 0.90372002 0.67806304 0.92961794 0.67806721
		 0.8778221 0.68036479 0.95551586 0.68036252 0.98141384 0.6803695 0.90372002 0.68036759
		 0.92961794 0.68037188 0.8778221 0.6356765 0.96386665 0.63814616 0.96386665 0.63814878
		 0.99257404 0.63567907 0.99257404 0.6356709 0.90645242 0.63814098 0.90645236 0.63814318
		 0.93515933 0.63567346 0.93515933 0.63566869 0.87774503 0.63813841 0.87774503 0.66629368
		 0.9878003 0.66629368 0.98434895 0.66966468 0.98434895 0.66966468 0.9878003 0.72513103
		 0.98214579 0.72513103 0.99033612 0.71621096 0.99033612 0.71621096 0.98214579 0.70740533
		 0.95148802 0.70740533 0.96369964 0.69856423 0.96369964 0.69856423 0.95148802 0.70740533
		 0.94243658 0.69856423 0.94243658 0.72513103 0.99852681 0.71621096 0.99852681 0.63054168
		 0.99768269 0.62887228 0.99768269 0.62887228 0.98274481 0.63054168 0.98274481 0.68599993
		 0.97799826 0.68474782 0.97799826 0.68474782 0.9655062 0.68599993 0.9655062 0.69311655
		 0.97171509 0.69031286 0.97171509 0.69031721 0.96000755 0.69311655 0.96000755 0.69442499
		 0.97171509 0.69442499 0.96000755 0.68349576 0.97799826 0.68349576 0.9655062 0.64068472
		 0.98476696 0.64068651 0.97141236 0.64282745 0.97141236 0.64282495 0.98476696 0.68683964
		 0.92514718 0.68474782 0.92514718 0.68474782 0.913683 0.68683964 0.913683 0.63005501
		 0.93448406 0.62726599 0.93448406 0.62726599 0.92077535 0.63005501 0.92077535 0.64278346
		 0.92826724 0.63920289 0.92826724 0.63920653 0.91601163 0.64278841 0.91601163 0.69349796
		 0.92218369 0.68968171 0.92218369 0.68969065 0.9114396 0.69349796 0.9114396 0.69568384
		 0.92218369 0.69568384 0.9114396 0.68265599 0.92514718 0.68265599 0.913683 0.68638331
		 0.95152849 0.68474782 0.95152849 0.68474782 0.93987495 0.68638331 0.93987495 0.6303184
		 0.96603054 0.62813771 0.96603054 0.62813771 0.95209533 0.6303184 0.95209533 0.64280415
		 0.95646977 0.64000678 0.95646977 0.64000952 0.94401175 0.64280796 0.94401175 0.6932916
		 0.94690788 0.69013774 0.94690788 0.69014448 0.93598634 0.6932916 0.93598634 0.69500065
		 0.94690788 0.69500065 0.93598634 0.68311238 0.95152849 0.68311238 0.93987495 0.68683964
		 0.90194666 0.68474782 0.90194666 0.68474782 0.88966525 0.68683964 0.88966525 0.63005501
		 0.90674126 0.62726599 0.90674126 0.62726599 0.89205527 0.63005501 0.89205527 0.6427936
		 0.90346509 0.63921034 0.90346509 0.63921404 0.89033586 0.64279866 0.89033586 0.69349796
		 0.90044045 0.68969983 0.90044045 0.68970907 0.8889305 0.69349796 0.8889305 0.69568384
		 0.90044045 0.69568384 0.8889305 0.68265599 0.90194666 0.68265599 0.88966525 0.68599993
		 0.9536528 0.68474782 0.9536528 0.63054168 0.96857077 0.62887228 0.96857077 0.64282966
		 0.95874077 0.64068818 0.95874077 0.69311655 0.94889879 0.69032133 0.94889879 0.69442499
		 0.94889879 0.68349576 0.9536528 0.68638331 0.92794198 0.68474782 0.92794198 0.6303184
		 0.93782616 0.62813771 0.93782616 0.64281178 0.93125504 0.64001232 0.93125504 0.6932916
		 0.92480296 0.69015127 0.92480296 0.69500065 0.92480296 0.68311238 0.92794198 0.68474782
		 0.87771875 0.68683964 0.87771875 0.62726599 0.87776995 0.63005501 0.87776995 0.63921773
		 0.87756467 0.64280379 0.87756467 0.68971813 0.87773448 0.69349796 0.87773448 0.69568384
		 0.87773448 0.68265599 0.87771875 0.64635676 0.89033586 0.64636332 0.87756467 0.6463502
		 0.90346509 0.64634365 0.91601163 0.64633721 0.92826724 0.64553672 0.93125504 0.64553183
		 0.94401175 0.64552701 0.95646977 0.64510185 0.95874077 0.64509887 0.97141236 0.64509571
		 0.98476696 0.69921106 0.97966868 0.70496148 0.97966868 0.70496148 0.98542261 0.69921106
		 0.98542261 0.7091068 0.98301244 0.7091068 0.98039401 0.71300751 0.98039401 0.71300751
		 0.98301244 0.6807645 0.9846738 0.69214869 0.9846738 0.69120234 0.98553252 0.68171102
		 0.98553252 0.69214869 0.99500239 0.69120234 0.99414372 0.6807645 0.99500239 0.68171102
		 0.99414372 0.72036397 0.91593492 0.72036397 0.90672088 0.72235107 0.90673095 0.72235107
		 0.91592491 0.72369432 0.94177371 0.72369432 0.94826937 0.72070944 0.94826227 0.72070944
		 0.94178081 0.72510296 0.88404906 0.72510296 0.87822044 0.72861749 0.87822682 0.72861749
		 0.88404274 0.72656745 0.8957625 0.72656745 0.89032048 0.72901547 0.89032638 0.72901547
		 0.89575654 0.7470085 0.96354866 0.74004704 0.9655633 0.7353614 0.96355492 0.69214869
		 0.99726325 0.6807645 0.99726325 0.70754266 0.96613955 0.71000677 0.97533208 0.6983552
		 0.97532582 0.69835514 0.96613264 0.67070955 0.98432416 0.67415208 0.98432416 0.67415208
		 0.98776907 0.67070955 0.98776907 0.73782438 0.95436603 0.74700844 0.95435929 0.70531899
		 0.97734106;
	setAttr ".uvst[1].uvsp[250:421]" 0.69831914 0.98631507 0.6983192 0.97877634
		 0.69860083 0.97905815 0.69860089 0.98603332 0.70585334 0.98631507 0.70557165 0.98603332
		 0.7058534 0.97877634 0.70557165 0.97905815 0.72675914 0.99757969 0.72675914 0.99043256
		 0.72719711 0.99087065 0.72719711 0.99714154 0.73390192 0.99043256 0.733464 0.99087071
		 0.73390192 0.99757969 0.733464 0.99714154 0.72510266 0.88846266 0.72512817 0.88548559
		 0.72846377 0.88548559 0.72843897 0.88846266 0.72106582 0.89476055 0.72106582 0.89121437
		 0.72448105 0.89121437 0.72448105 0.89476055 0.72436583 0.91743463 0.724352 0.91280097
		 0.72620773 0.91280097 0.7262221 0.91743463 0.72453696 0.91018862 0.72453696 0.90565157
		 0.7263543 0.90565157 0.7263543 0.91018862 0.70794755 0.98434967 0.70794755 0.97812659
		 0.70843589 0.97976869 0.70843589 0.98363781 0.71416688 0.97812659 0.71367848 0.97976869
		 0.71416688 0.98434967 0.71367848 0.98363781 0.70895088 0.98014873 0.70895088 0.98325765
		 0.7131635 0.98014873 0.7131635 0.98325765 0.74124569 0.90156972 0.74125969 0.90500879
		 0.7396661 0.90473759 0.73965335 0.90184104 0.71064878 0.90590954 0.71063519 0.90927619
		 0.70907557 0.90901059 0.70908844 0.9061752 0.72772431 0.98158395 0.72772431 0.98737615
		 0.73351288 0.98737615 0.73351288 0.98158401 0.71886879 0.97938746 0.7227906 0.97938746
		 0.7227906 0.97675484 0.71886879 0.97675478 0.73714489 0.98546165 0.73809886 0.98632699
		 0.74766415 0.98632699 0.74861801 0.98546165 0.74766415 0.99500549 0.74861801 0.99587089
		 0.73809886 0.99500549 0.73714489 0.99587089 0.74493635 0.90796638 0.74715686 0.90795517
		 0.74715686 0.89767599 0.74493635 0.89766473 0.71432614 0.9331156 0.71070623 0.93312341
		 0.71070623 0.94024998 0.71432614 0.94025779 0.74500632 0.93330669 0.74719226 0.93330085
		 0.74719226 0.92796016 0.74500632 0.92795438 0.72106582 0.8901298 0.723454 0.8901248
		 0.723454 0.88552618 0.72106582 0.88552117 0.71140504 0.97006762 0.7230894 0.97007406
		 0.71838868 0.97208869 0.73714489 0.99814939 0.74861801 0.99814939 0.73108995 0.95030946
		 0.7218731 0.95030248 0.7218731 0.95952511 0.73356187 0.95953143 0.67490953 0.98427719
		 0.67490959 0.98773605 0.67836612 0.98773605 0.67836612 0.98427719 0.7114051 0.96084875
		 0.72061837 0.96085554 0.72885931 0.96154684 0.72682655 0.98827451 0.72711015 0.98799092
		 0.72711015 0.98096937 0.72682655 0.98068577 0.73441064 0.98827451 0.73412728 0.98799092
		 0.7344107 0.98068577 0.73412728 0.98096937 0.68885481 0.98150027 0.68930072 0.98105431
		 0.68930066 0.97467041 0.68885481 0.97422433 0.69568062 0.97467041 0.69612628 0.97422433
		 0.69568062 0.98105431 0.69612628 0.98150027 0.73004168 0.93326086 0.73200631 0.93326086
		 0.73202085 0.92835587 0.73005664 0.92835587 0.7210288 0.88333982 0.7233696 0.88333982
		 0.7233696 0.87749493 0.7210288 0.87749493 0.71648306 0.97544569 0.71648306 0.97348768
		 0.71159524 0.97350305 0.71159524 0.97546005 0.73576319 0.98380232 0.73767984 0.98380232
		 0.73767984 0.97901642 0.73576319 0.97901642 0.71770316 0.98073196 0.71819425 0.98001623
		 0.71819425 0.97612613 0.71770316 0.97447509 0.72346526 0.97612613 0.72395623 0.97447515
		 0.72346526 0.98001623 0.72395623 0.98073196 0.71871197 0.97963405 0.71871197 0.97650826
		 0.72294748 0.9765082 0.72294748 0.97963405 0.72783178 0.94381696 0.72585958 0.94415283
		 0.72587502 0.9477396 0.72784889 0.94807541 0.73684502 0.90095294 0.73493332 0.90127826
		 0.73491782 0.90475106 0.73682779 0.90507621 0.68766671 0.93983042 0.68767226 0.92963243
		 0.68915057 0.92963767 0.68916279 0.93983269 0.68765533 0.96097702 0.68766111 0.95027566
		 0.68917531 0.95027471 0.68918806 0.96097279 0.68764943 0.97194409 0.68920118 0.97193646
		 0.69583529 0.97214848 0.69584376 0.96116596 0.69737762 0.96117026 0.69738919 0.97215658
		 0.69585204 0.95046252 0.69586009 0.94002765 0.69735545 0.94002467 0.69736648 0.95046312
		 0.69586784 0.92985117 0.69734478 0.92984492;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 130 ".pt";
	setAttr ".pt[0:7]" -type "float3" 0 0 0.26454917  0 0 0.26454917  0 0 
		0.26454917  0 0 0.26454917  0.68827403 -1.0329856 0.60278517  0.29708087 -1.0314621 
		0.90685797  -0.64112455 -1.028067 0.57214594  -0.28845218 -1.0314621 0.89040351 ;
	setAttr ".pt[53:62]" -type "float3" 0 0 0.26454917  0 0 0.26454917  0 0 
		0.26454917  0 0 0.26454917  -0.68827403 -1.0314621 -0.070396051  -0.2970809 -1.0314621 
		-0.3777594  0.68356687 -1.0314621 -0.062882096  0.28845218 -1.0314621 -0.36130509  
		0 0 -0.53086615  0 0 -0.25855032 ;
	setAttr ".pt[68:69]" -type "float3" 0 0 -0.25855032  0 0 -0.53086615 ;
	setAttr ".pt[74:75]" -type "float3" 0 0 -0.25855032  0 0 -0.53086615 ;
	setAttr ".pt[80:81]" -type "float3" 0 0 -0.25855032  0 0 -0.53086615 ;
	setAttr ".pt[86]" -type "float3" 0 0 -0.25855032 ;
	setAttr ".pt[88:90]" -type "float3" 0 0 -0.25855032  0 0 -0.53086615  0 
		0 -0.53086615 ;
	setAttr ".pt[98:99]" -type "float3" 0 0 -0.25855032  0 0 -0.53086615 ;
	setAttr ".pt[105:106]" -type "float3" 0 0 -0.25855032  0 0 -0.53086615 ;
	setAttr ".pt[111:112]" -type "float3" 0 0 -0.25855032  0 0 -0.53086615 ;
	setAttr ".pt[116:117]" -type "float3" 0 0 -0.25855032  0 0 -0.53086615 ;
	setAttr ".pt[122:123]" -type "float3" 0 0 -0.25855032  0 0 -0.53086615 ;
	setAttr ".pt[138:229]" -type "float3" 0 0 0.39976907  0 0 0.39976907  0 0 
		0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  
		0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 
		0 0.39976907  0 0.30872083 0.39976907  0 0.30872077 0.39976907  0 0 0.39976907  0 
		0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 
		0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  
		0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 
		0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 
		0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  
		0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 
		0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 
		0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  
		0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 
		0 0.39976907  0 0 0.39976907  0 0.30872083 0.39976907  0 0 0.39976907  0 0 0.39976907  
		0 0.30872077 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  
		0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 
		0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 
		0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  0 0 0.39976907  
		0 0 0.39976907 ;
	setAttr -s 240 ".vt";
	setAttr ".vt[0:165]"  -2.22464776 -0.26290458 -11.32077026 -1.012269855 -0.26290458 -12.53388023
		 1.89539766 -0.26290458 -11.11377716 0.80240232 -0.26290458 -12.46406841 -2.22464776 -2.42834258 -11.24377155
		 -1.012269855 -2.43749166 -12.53388023 1.89539766 -2.45788264 -11.11377716 0.80240232 -2.43749166 -12.46406841
		 -2.0061473846 -1.3699003e-016 -19.028753281 -2.0061473846 -1.5543122e-017 -7.3242186e-006
		 0 0 0 0 0 -19.028745651 -2.0061473846 -7.438601e-017 -9.5143795 0 0 -9.51437283 -2.0061473846 -7.3183984e-017 -4.75719357
		 0 0 -4.75718641 -2.0061473846 -8.1101949e-017 -14.2715683 0 0 -14.27156162 -2.16886711 0.11543948 -19.028753281
		 -2.16886711 0.1154389 -14.2715683 -2.16886711 0.11543951 -9.5143795 -2.16886711 0.11543951 -4.75719357
		 -2.16886711 0.1154389 -7.3242186e-006 -2.36199212 0.11673862 -19.028749466 -2.36199212 0.11673866 -14.27156639
		 -2.36199212 0.11673866 -9.51437759 -2.36199212 0.11673851 -4.75719118 -2.36199212 0.11673862 -4.8828124e-006
		 -2.68471193 -0.36500016 -19.028753281 -2.68471193 -0.36500037 -14.2715683 -2.68471193 -0.36500046 -9.5143795
		 -2.68471193 -0.36500022 -4.75719357 -2.68471193 -0.36499953 -7.3242186e-006 2.0054931641 -4.3965076e-018 -19.02897644
		 2.0054931641 -1.5543122e-017 -0.00023071289 2.0054931641 5.9959907e-017 -9.51460361
		 2.0054931641 6.0625536e-017 -4.7574172 2.0054931641 -1.4536968e-017 -14.27179241
		 2.16748047 0.1154389 -19.0291996 2.16748047 0.11543944 -14.27201557 2.16748047 0.1154389 -9.51482677
		 2.16748071 0.11543951 -4.75764036 2.16748047 0.11543951 -0.00045410157 2.36058116 0.11673805 -19.0291996
		 2.36058116 0.11673866 -14.27201557 2.36058116 0.11673862 -9.51482677 2.36058116 0.11673851 -4.75764036
		 2.36058116 0.11673862 -0.00045410157 2.68334484 -0.36500022 -19.0291996 2.68334484 -0.36500016 -14.27201557
		 2.68334484 -0.36499953 -9.51482677 2.68334484 -0.36500046 -4.75764036 2.68334484 -0.36499953 -0.00045410157
		 2.041522264 -0.26290458 -8.38762283 0.8291443 -0.26290458 -7.083552361 -2.21005988 -0.26290458 -8.41950226
		 -0.98552793 -0.26290458 -7.15336466 2.041522264 -2.43749166 -8.38762283 0.8291443 -2.43749166 -7.083552361
		 -2.21005988 -2.43749166 -8.41950226 -0.98552793 -2.43749166 -7.15336466 1.47260618 -1.62117314 -9.98010635
		 1.21980953 -1.66934299 -8.26271629 0.39616454 -1.66934299 -7.64425373 -1.65177 -1.62117314 -10.48743534
		 -1.40184879 -1.66934299 -8.46883869 -0.60655701 -1.66934299 -7.64652729 0.27392945 -16.44520569 -7.54863691
		 1.21980953 -16.44520569 -8.37387371 1.47205687 -16.44520569 -9.97687054 -1.65453494 -16.44520569 -10.48067093
		 -1.40184879 -16.44520569 -8.46883869 -0.60655701 -16.44520569 -7.64652729 0.35959107 -34.76734161 -7.062709808
		 1.5486182 -34.76734161 -8.1243124 1.87724245 -34.76734161 -10.2294693 -1.94796085 -34.76734161 -10.55407429
		 -1.82939816 -34.76734161 -8.26472569 -0.79054564 -34.76734161 -7.19057751 0.35959107 -17.95902443 -7.062709808
		 1.54861939 -17.95902443 -7.97765064 1.87811518 -17.95902443 -10.23459625 -1.94358027 -17.95902443 -10.56479263
		 -1.82939816 -17.95902443 -8.26472569 -0.79054564 -17.95902443 -7.19057751 0.46130127 -36.75896454 -6.48575258
		 1.93969727 -36.75896454 -7.64790392 0.46130127 -53.29224777 -6.48575258 1.93969727 -53.29224777 -7.91223621
		 2.36001229 -36.75896454 -10.53610325 2.35884023 -53.29224777 -10.52923012 -2.50769234 -36.75896454 -10.80082321
		 -2.51356387 -53.29224777 -10.78645039 -2.33846307 -36.75896454 -8.023155212 -2.33846307 -53.29224777 -8.023155212
		 -1.0097491741 -36.75896454 -6.64929771 -1.0097491741 -53.29224777 -6.64929771 0.66869754 -70.55757904 -5.30926991
		 2.66511345 -70.55757904 -6.9922142 3.24630499 -70.55757904 -10.92221355 -3.47616506 -70.55757904 -11.26938725
		 -3.22445488 -70.55757904 -7.44707155 -1.37683964 -70.55757904 -5.53668356 -0.60655701 -9.67154884 -7.64652729
		 0.39616454 -9.67154884 -7.64425373 1.21980953 -9.67154884 -8.26271629 1.47232425 -9.67154884 -9.97844505
		 -1.65318906 -9.67154884 -10.48396492 -1.40184879 -9.67154884 -8.46883869 -0.79054564 -26.2636261 -7.19057751
		 0.35959107 -26.2636261 -7.062709808 1.5486182 -26.2636261 -8.028534889 1.87768316 -26.2636261 -10.23206329
		 -1.94574404 -26.2636261 -10.55949688 -1.82939816 -26.2636261 -8.26472569 0.46130127 -44.92868805 -6.48575258
		 1.93969727 -44.92868805 -7.91223621 2.35943604 -44.92868805 -10.53272629 -2.51057744 -44.92868805 -10.79375935
		 -2.33846307 -44.92868805 -8.023155212 -1.0097491741 -44.92868805 -6.64929771 0.46130127 -62.044261932 -6.48575258
		 1.93969727 -62.044261932 -7.91223621 2.35824943 -62.044261932 -10.52575684 -2.51653075 -62.044261932 -10.77919197
		 -2.33846307 -62.044261932 -8.023155212 -1.0097491741 -62.044261932 -6.64929771 -0.46093202 -70.55757904 -13.7485342
		 -0.27820802 -62.044261932 -12.57439613 -0.2761609 -53.29224777 -12.58149338 -0.27409789 -44.92868805 -12.58864021
		 -0.2721045 -36.75896454 -12.59554482 -0.18809082 -34.76734161 -12.015468597 -0.18656006 -26.2636261 -12.020769119
		 -0.18506594 -17.95902443 -12.025945663 -0.11537964 -16.44520569 -11.53720188 -0.11445069 -9.67154884 -11.54042244
		 -0.11347046 -1.62117314 -11.54381561 3.85973334 -2.58958888 -9.013165474 3.18200994 -1.22900212 -9.013165474
		 1.03872478 -1.22900212 -9.013165474 3.18200994 -1.22900212 -10.85236168 1.03872478 -1.22900212 -10.85236168
		 3.85973334 -2.58958888 -10.85236168 3.10507083 -1.15336835 -9.16606522 3.10507083 -1.15336835 -10.69946289
		 2.33345556 -1.15336835 -10.69946289 2.33345556 -1.15336835 -9.16606522 2.88379693 -0.14805557 -9.60579395
		 2.88379693 -0.14805557 -10.25973225 2.55472898 -0.14805557 -10.25973225 2.55472898 -0.14805557 -9.60579395
		 0.93128234 -4.11949396 -9.013165474 0.93128234 -4.11949396 -10.85236168 0.94257569 -1.44185293 -9.013165474
		 0.94257569 -1.44185293 -10.85236168 3.10423136 -0.96314883 -9.16773224 2.3342948 -0.96314883 -9.16773224
		 2.3342948 -0.96314883 -10.69779587 3.10423136 -0.96314883 -10.69779587 2.36308146 -0.88916934 -9.22493935
		 2.36308146 -0.88916934 -10.64058876 3.075444937 -0.88916934 -10.64058876 3.075444937 -0.88916934 -9.22493935
		 3.24899817 -1.29302311 -10.73961926 3.84700656 -2.51185632 -10.73961926;
	setAttr ".vt[166:239]" 3.24899817 -1.29302311 -9.12590981 3.84700656 -2.51185632 -9.12590981
		 3.35499644 -1.34874058 -10.67303371 3.91548038 -2.46777868 -10.67303371 3.35499644 -1.34874058 -9.19249535
		 3.91548038 -2.46777868 -9.19249535 3.46088004 -1.43837631 -10.55677414 3.8875196 -2.29699779 -10.55677414
		 3.46088004 -1.43837631 -9.30875587 3.8875196 -2.29699779 -9.30875587 3.44501472 -1.49418414 -10.43417645
		 3.80269837 -2.22918916 -10.43417645 3.44501472 -1.49418414 -9.43135166 3.80269837 -2.22918916 -9.43135166
		 3.36607933 -1.55969989 -10.39706707 3.68473506 -2.22524619 -10.39706707 3.36607933 -1.55969989 -9.46846294
		 3.68473506 -2.22524619 -9.46846294 -2.86208749 -0.14805557 -9.60579395 -2.86208749 -0.14805557 -10.25973225
		 -2.5330193 -0.14805557 -10.25973225 -2.5330193 -0.14805557 -9.60579395 -3.66302562 -2.22524619 -10.39706707
		 -3.34436989 -1.55969989 -10.39706707 -3.34436989 -1.55969989 -9.46846294 -3.66302562 -2.22524619 -9.46846294
		 -3.16030025 -1.22900212 -9.013165474 -3.16030025 -1.22900212 -10.85236168 -3.083361149 -1.15336835 -10.69946289
		 -3.083361149 -1.15336835 -9.16606522 -1.017015696 -1.22900212 -10.85236168 -2.31174612 -1.15336835 -10.69946289
		 -1.017015696 -1.22900212 -9.013165474 -2.31174612 -1.15336835 -9.16606522 -3.082521677 -0.96314883 -10.69779587
		 -3.082521677 -0.96314883 -9.16773224 -2.31258488 -0.96314883 -10.69779587 -2.31258488 -0.96314883 -9.16773224
		 -0.92086655 -1.44185293 -9.013165474 -0.9095729 -4.11949396 -9.013165474 -0.92086655 -1.44185293 -10.85236168
		 -3.83802366 -2.58958888 -10.85236168 -0.9095729 -4.11949396 -10.85236168 -3.83802366 -2.58958888 -9.013165474
		 -3.053735256 -0.88916934 -9.22493935 -2.34137225 -0.88916934 -9.22493935 -2.34137225 -0.88916934 -10.64058876
		 -3.053735256 -0.88916934 -10.64058876 -3.22728848 -1.29302311 -10.73961926 -3.82529688 -2.51185632 -10.73961926
		 -3.22728848 -1.29302311 -9.12590981 -3.82529688 -2.51185632 -9.12590981 -3.33328676 -1.34874058 -10.67303371
		 -3.89377069 -2.46777868 -10.67303371 -3.33328676 -1.34874058 -9.19249535 -3.89377069 -2.46777868 -9.19249535
		 -3.4391706 -1.43837631 -10.55677414 -3.86580992 -2.29699779 -10.55677414 -3.4391706 -1.43837631 -9.30875587
		 -3.86580992 -2.29699779 -9.30875587 -3.42330503 -1.49418414 -10.43417645 -3.78098869 -2.22918916 -10.43417645
		 -3.42330503 -1.49418414 -9.43135166 -3.78098869 -2.22918916 -9.43135166 -2.084052801 -0.70097733 -19.028753281
		 -2.026558876 -0.65283126 -14.32948971 -2.65406156 -0.66738504 -9.69433689 -2.083701134 -0.70097733 -5.0019469261
		 -2.084052801 -0.70097733 -7.3242186e-006 2.11926746 -0.68962258 -19.028064728 2.012746572 -0.73388565 -14.32793617
		 2.33879805 -0.68568969 -9.91490746 2.083032131 -0.70097733 -4.99875736 2.1192677 -0.68962258 0.0006793213;
	setAttr -s 457 ".ed";
	setAttr ".ed[0:165]"  0 1 0 2 3 0 3 1 0 0 4 0 1 5 0 4 5 0 2 6 0 3 7 0 6 7 0
		 7 5 0 8 16 1 9 10 0 11 8 0 10 15 1 12 14 1 13 17 1 12 13 1 14 9 1 15 13 1 14 15 1
		 16 12 1 17 11 1 16 17 1 8 18 0 16 19 1 18 19 1 12 20 1 14 21 1 20 21 1 9 22 0 21 22 1
		 19 20 1 18 23 0 19 24 1 23 24 1 20 25 1 21 26 1 25 26 1 22 27 0 26 27 1 24 25 1 23 28 0
		 24 29 1 28 29 0 25 30 1 26 31 1 30 31 0 27 32 0 31 32 0 29 30 0 33 37 1 34 10 0 11 33 0
		 35 36 1 35 13 1 36 34 1 36 15 1 37 35 1 37 17 1 33 38 0 37 39 1 38 39 1 35 40 1 36 41 1
		 40 41 1 34 42 0 41 42 1 39 40 1 38 43 0 39 44 1 43 44 1 40 45 1 41 46 1 45 46 1 42 47 0
		 46 47 1 44 45 1 43 48 0 44 49 1 48 49 0 45 50 1 46 51 1 50 51 0 47 52 0 51 52 0 49 50 0
		 2 53 0 53 54 0 0 55 0 55 56 0 56 54 0 53 57 0 6 57 0 54 58 0 57 58 0 55 59 0 4 59 0
		 56 60 0 59 60 0 60 58 0 61 62 0 62 63 0 64 65 0 65 66 0 66 63 0 61 106 0 62 105 0
		 63 104 0 64 107 0 65 108 0 66 103 0 64 137 0 67 68 0 68 69 0 69 135 0 70 71 0 71 72 0
		 72 67 0 73 74 0 74 75 0 75 132 0 76 77 0 77 78 0 78 73 0 67 79 0 68 80 0 79 80 0
		 79 110 0 80 111 0 69 81 0 80 81 0 81 112 0 70 82 0 81 134 0 82 113 0 71 83 0 82 83 0
		 83 114 0 72 84 0 83 84 0 84 109 0 84 79 0 73 85 0 74 86 0 85 86 0 85 115 0 87 88 1
		 86 116 0 75 89 0 86 89 0 88 90 1 89 117 0 76 91 0 89 131 0 90 129 1 91 118 0 77 93 0
		 91 93 0 92 94 1 93 119 0 78 95 0 93 95 0 94 96 1 95 120 0 95 85 0 96 87 1;
	setAttr ".ed[166:331]" 87 121 0 98 97 0 88 122 0 99 98 0 90 123 0 99 127 0
		 92 124 0 100 101 0 94 125 0 101 102 0 96 126 0 102 97 0 70 83 0 83 72 0 72 79 0 79 68 0
		 68 81 0 76 93 0 93 78 0 78 85 0 85 74 0 74 89 0 103 72 0 104 67 0 103 104 1 105 68 0
		 104 105 1 106 69 0 105 106 1 107 70 0 106 136 1 108 71 0 107 108 1 108 103 1 109 78 0
		 110 73 0 109 110 1 111 74 0 110 111 1 112 75 0 111 112 1 113 76 0 112 133 1 114 77 0
		 113 114 1 114 109 1 115 87 0 116 88 0 115 116 1 117 90 0 116 117 1 118 92 0 117 130 1
		 119 94 0 118 119 1 120 96 0 119 120 1 120 115 1 121 97 0 122 98 0 121 122 1 123 99 0
		 122 123 1 124 100 0 123 128 1 125 101 0 124 125 1 126 102 0 125 126 1 126 121 1 127 100 0
		 128 124 1 127 128 1 129 92 1 128 129 1 130 118 1 129 130 1 131 91 0 130 131 1 132 76 0
		 131 132 1 133 113 1 132 133 1 134 82 0 133 134 1 135 70 0 134 135 1 136 107 1 135 136 1
		 137 61 0 136 137 1 69 134 0 134 70 0 75 131 0 131 76 0 139 140 1 141 142 1 138 139 1
		 139 141 1 140 142 1 141 143 1 143 138 1 139 144 1 141 145 1 144 145 1 142 146 1 145 146 1
		 140 147 1 147 146 1 144 147 1 144 156 1 145 159 1 148 149 1 146 158 1 149 150 1 147 157 1
		 151 150 1 148 151 1 138 152 1 140 154 1 142 155 1 143 153 1 152 154 0 153 155 0 152 153 0
		 152 140 1 153 142 1 155 154 0 156 163 1 157 160 1 156 157 1 158 161 1 157 158 1 159 162 1
		 158 159 1 159 156 1 160 151 1 161 150 1 160 161 1 162 149 1 161 162 1 163 148 1 162 163 1
		 163 160 1 141 164 1 143 165 1 164 165 1 139 166 1 166 164 1 138 167 1 167 166 1 165 167 1
		 164 168 1 165 169 1 168 169 1 166 170 1 170 168 1 167 171 1 171 170 1 169 171 1 168 172 1
		 169 173 1 172 173 1 170 174 1 174 172 1 171 175 1;
	setAttr ".ed[332:456]" 175 174 1 173 175 1 172 176 1 173 177 1 176 177 1 174 178 1
		 178 176 1 175 179 1 179 178 1 177 179 1 176 180 1 177 181 1 180 181 1 178 182 1 182 180 1
		 179 183 1 183 182 1 181 183 1 184 185 1 185 186 1 187 186 1 184 187 1 189 188 1 190 189 1
		 191 190 1 188 191 1 192 193 1 193 194 1 195 194 1 192 195 1 193 196 1 196 197 1 194 197 1
		 198 196 1 198 199 1 199 197 1 192 198 1 195 199 1 194 200 1 200 201 1 195 201 1 197 202 1
		 202 200 1 199 203 1 203 202 1 201 203 1 198 204 1 205 204 0 205 198 1 196 206 1 206 204 0
		 207 208 1 208 196 1 193 207 1 209 205 1 205 208 0 207 209 1 208 206 0 209 192 1 201 210 1
		 210 211 1 203 211 1 211 212 1 202 212 1 212 213 1 200 213 1 213 210 1 211 187 1 212 186 1
		 213 185 1 210 184 1 193 214 1 214 215 1 207 215 1 192 216 1 216 214 1 209 217 1 217 216 1
		 215 217 1 214 218 1 218 219 1 215 219 1 216 220 1 220 218 1 217 221 1 221 220 1 219 221 1
		 218 222 1 222 223 1 219 223 1 220 224 1 224 222 1 221 225 1 225 224 1 223 225 1 222 226 1
		 226 227 1 223 227 1 224 228 1 228 226 1 225 229 1 229 228 1 227 229 1 226 189 1 227 188 1
		 228 190 1 229 191 1 28 230 0 29 231 0 230 231 0 30 232 0 31 233 0 232 233 0 32 234 0
		 233 234 0 231 232 0 48 235 0 49 236 0 235 236 0 50 237 0 51 238 0 237 238 0 52 239 0
		 238 239 0 236 237 0;
	setAttr -s 220 ".fc[0:219]" -type "polyFaces" 
		f 4 0 4 -6 -4
		mu 0 4 0 1 2 3
		mu 1 4 0 1 2 3
		f 4 -2 6 8 -8
		mu 0 4 4 5 6 7
		mu 1 4 6 4 5 7
		f 4 -3 7 9 -5
		mu 0 4 8 9 10 11
		mu 1 4 1 8 9 2
		f 4 21 12 10 22
		mu 0 4 12 13 14 15
		mu 1 4 10 11 12 13
		f 4 18 -17 14 19
		mu 0 4 16 17 18 19
		mu 1 4 14 15 16 17
		f 4 13 -20 17 11
		mu 0 4 20 21 22 564
		mu 1 4 18 14 17 19
		f 4 15 -23 20 16
		mu 0 4 24 25 26 27
		mu 1 4 15 10 13 16
		f 4 -11 23 25 -25
		mu 0 4 28 29 30 31
		mu 1 4 13 12 20 21
		f 4 -15 26 28 -28
		mu 0 4 32 33 34 35
		mu 1 4 17 16 22 23
		f 4 -18 27 30 -30
		mu 0 4 23 565 36 562
		mu 1 4 19 17 23 24
		f 4 -21 24 31 -27
		mu 0 4 38 39 40 41
		mu 1 4 16 13 21 22
		f 4 -26 32 34 -34
		mu 0 4 42 43 44 45
		mu 1 4 21 20 25 26
		f 4 -29 35 37 -37
		mu 0 4 46 47 48 49
		mu 1 4 23 22 27 28
		f 4 -31 36 39 -39
		mu 0 4 37 563 50 560
		mu 1 4 24 23 28 29
		f 4 -32 33 40 -36
		mu 0 4 52 53 54 55
		mu 1 4 22 21 26 27
		f 4 -35 41 43 -43
		mu 0 4 56 57 58 59
		mu 1 4 30 31 32 33
		f 4 -38 44 46 -46
		mu 0 4 60 61 62 63
		mu 1 4 34 35 36 37
		f 4 -40 45 48 -48
		mu 0 4 51 561 64 65
		mu 1 4 38 34 37 39
		f 4 -41 42 49 -45
		mu 0 4 66 67 68 69
		mu 1 4 35 30 33 36
		f 4 -59 -51 -53 -22
		mu 0 4 70 71 72 73
		mu 1 4 10 40 41 11
		f 4 -57 -54 54 -19
		mu 0 4 74 75 76 77
		mu 1 4 14 42 43 15
		f 4 -52 -56 56 -14
		mu 0 4 78 79 80 81
		mu 1 4 18 44 42 14
		f 4 -55 -58 58 -16
		mu 0 4 82 83 84 85
		mu 1 4 15 43 40 10
		f 4 60 -62 -60 50
		mu 0 4 86 87 88 89
		mu 1 4 40 45 46 41
		f 4 63 -65 -63 53
		mu 0 4 90 91 92 93
		mu 1 4 42 47 48 43
		f 4 65 -67 -64 55
		mu 0 4 94 95 96 97
		mu 1 4 44 49 47 42
		f 4 62 -68 -61 57
		mu 0 4 98 99 100 101
		mu 1 4 43 48 45 40
		f 4 69 -71 -69 61
		mu 0 4 102 103 104 105
		mu 1 4 45 50 51 46
		f 4 72 -74 -72 64
		mu 0 4 106 107 108 109
		mu 1 4 47 52 53 48
		f 4 74 -76 -73 66
		mu 0 4 110 111 112 113
		mu 1 4 49 54 52 47
		f 4 71 -77 -70 67
		mu 0 4 114 115 116 117
		mu 1 4 48 53 50 45
		f 4 78 -80 -78 70
		mu 0 4 118 119 120 121
		mu 1 4 55 56 57 58
		f 4 81 -83 -81 73
		mu 0 4 122 123 124 125
		mu 1 4 59 60 61 62
		f 4 83 -85 -82 75
		mu 0 4 126 127 128 129
		mu 1 4 63 64 60 59
		f 4 80 -86 -79 76
		mu 0 4 130 131 132 133
		mu 1 4 62 61 56 55
		f 4 86 91 -93 -7
		mu 0 4 134 135 136 137
		mu 1 4 65 66 67 68
		f 4 87 93 -95 -92
		mu 0 4 138 139 140 141
		mu 1 4 69 70 71 72
		f 4 -89 3 96 -96
		mu 0 4 142 143 144 145
		mu 1 4 73 74 75 76
		f 4 -90 95 98 -98
		mu 0 4 146 147 148 149
		mu 1 4 77 73 76 78
		f 4 -91 97 99 -94
		mu 0 4 150 151 152 153
		mu 1 4 70 79 80 71
		f 4 100 106 194 -106
		mu 0 4 154 155 156 157
		mu 1 4 81 82 83 84
		f 4 101 107 192 -107
		mu 0 4 158 159 160 161
		mu 1 4 85 86 87 88
		f 4 -103 108 198 -110
		mu 0 4 162 163 164 165
		mu 1 4 89 90 91 92
		f 4 -104 109 199 -111
		mu 0 4 166 167 168 169
		mu 1 4 93 89 92 94
		f 4 -105 110 190 -108
		mu 0 4 170 171 172 173
		mu 1 4 86 95 96 87
		f 4 105 196 256 255
		mu 0 4 174 175 176 177
		mu 1 4 97 98 99 100
		f 4 -145 145 214 -148
		mu 0 4 178 179 180 181
		mu 1 4 101 102 103 104
		f 4 -150 147 216 -152
		mu 0 4 182 183 184 185
		mu 1 4 105 106 107 108
		f 4 -154 151 218 244
		mu 0 4 186 187 188 189
		mu 1 4 109 110 111 112
		f 4 -158 155 220 -160
		mu 0 4 190 191 192 193
		mu 1 4 113 114 115 116
		f 4 -162 159 222 -164
		mu 0 4 194 195 196 197
		mu 1 4 117 113 116 118
		f 4 -165 163 223 -146
		mu 0 4 198 199 200 201
		mu 1 4 102 119 120 103
		f 4 -127 127 204 -129
		mu 0 4 202 203 204 205
		mu 1 4 121 122 123 124
		f 4 -131 128 206 -132
		mu 0 4 206 207 208 209
		mu 1 4 125 126 127 128
		f 4 -134 131 208 250
		mu 0 4 210 211 212 213
		mu 1 4 129 130 131 132
		f 4 -137 134 210 -138
		mu 0 4 214 215 216 217
		mu 1 4 133 134 135 136
		f 4 -140 137 211 -141
		mu 0 4 218 219 220 221
		mu 1 4 137 133 136 138
		f 4 -142 140 202 -128
		mu 0 4 222 223 224 225
		mu 1 4 122 139 140 123
		f 4 -147 166 226 -169
		mu 0 4 226 227 228 229
		mu 1 4 141 142 143 144
		f 4 -151 168 228 -171
		mu 0 4 230 231 232 233
		mu 1 4 145 146 147 148
		f 4 -155 170 230 240
		mu 0 4 234 235 236 237
		mu 1 4 149 150 151 152
		f 4 -159 172 232 -175
		mu 0 4 238 239 240 241
		mu 1 4 153 154 155 156
		f 4 -163 174 234 -177
		mu 0 4 242 243 244 245
		mu 1 4 157 153 156 158
		f 4 -166 176 235 -167
		mu 0 4 246 247 248 249
		mu 1 4 142 159 160 143
		f 3 -113 124 181
		mu 0 3 250 251 252
		mu 1 3 161 162 122
		f 3 -114 182 -130
		mu 0 3 253 254 255
		mu 1 3 163 164 125
		f 3 -115 257 252
		mu 0 3 256 257 258
		mu 1 3 165 166 129
		f 3 -116 178 -136
		mu 0 3 259 260 261
		mu 1 3 167 168 133
		f 3 -117 135 179
		mu 0 3 262 263 264
		mu 1 3 169 167 133
		f 3 -118 180 -125
		mu 0 3 265 266 267
		mu 1 3 162 170 122
		f 3 -119 142 186
		mu 0 3 268 269 270
		mu 1 3 171 172 102
		f 3 -120 187 -149
		mu 0 3 271 272 273
		mu 1 3 173 174 105
		f 3 -121 259 246
		mu 0 3 274 275 276
		mu 1 3 175 176 109
		f 3 -122 183 -157
		mu 0 3 277 278 279
		mu 1 3 177 178 113
		f 3 -123 156 184
		mu 0 3 280 281 282
		mu 1 3 179 177 113
		f 3 -124 185 -143
		mu 0 3 283 284 285
		mu 1 3 172 180 102
		f 3 -179 132 136
		mu 0 3 286 287 288
		mu 1 3 133 168 134
		f 3 -180 139 -139
		mu 0 3 289 290 291
		mu 1 3 169 133 137
		f 3 -181 138 141
		mu 0 3 292 293 294
		mu 1 3 122 170 139
		f 3 -182 126 -126
		mu 0 3 295 296 297
		mu 1 3 161 122 121
		f 3 -183 125 130
		mu 0 3 298 299 300
		mu 1 3 125 164 126
		f 3 -184 152 157
		mu 0 3 301 302 303
		mu 1 3 113 178 114
		f 3 -185 161 -161
		mu 0 3 304 305 306
		mu 1 3 179 113 117
		f 3 -186 160 164
		mu 0 3 307 308 309
		mu 1 3 102 180 119
		f 3 -187 144 -144
		mu 0 3 310 311 312
		mu 1 3 171 102 101
		f 3 -188 143 149
		mu 0 3 313 314 315
		mu 1 3 105 174 106
		f 4 -191 188 117 -190
		mu 0 4 316 317 318 319
		mu 1 4 87 96 170 162
		f 4 -193 189 112 -192
		mu 0 4 320 321 322 323
		mu 1 4 88 87 162 161
		f 4 -195 191 113 -194
		mu 0 4 324 325 326 327
		mu 1 4 84 83 164 163
		f 4 -197 193 114 254
		mu 0 4 328 329 330 331
		mu 1 4 99 98 166 165
		f 4 -199 195 115 -198
		mu 0 4 332 333 334 335
		mu 1 4 92 91 168 167
		f 4 -200 197 116 -189
		mu 0 4 336 337 338 339
		mu 1 4 94 92 167 169
		f 4 -203 200 123 -202
		mu 0 4 340 341 342 343
		mu 1 4 123 140 180 172
		f 4 -205 201 118 -204
		mu 0 4 344 345 346 347
		mu 1 4 124 123 172 171
		f 4 -207 203 119 -206
		mu 0 4 348 349 350 351
		mu 1 4 128 127 174 173
		f 4 -209 205 120 248
		mu 0 4 352 353 354 355
		mu 1 4 132 131 176 175
		f 4 -211 207 121 -210
		mu 0 4 356 357 358 359
		mu 1 4 136 135 178 177
		f 4 -212 209 122 -201
		mu 0 4 360 361 362 363
		mu 1 4 138 136 177 179
		f 4 -215 212 146 -214
		mu 0 4 364 365 366 367
		mu 1 4 104 103 142 141
		f 4 -217 213 150 -216
		mu 0 4 368 369 370 371
		mu 1 4 108 107 146 145
		f 4 -219 215 154 242
		mu 0 4 372 373 374 375
		mu 1 4 112 111 150 149
		f 4 -221 217 158 -220
		mu 0 4 376 377 378 379
		mu 1 4 116 115 154 153
		f 4 -223 219 162 -222
		mu 0 4 380 381 382 383
		mu 1 4 118 116 153 157
		f 4 -224 221 165 -213
		mu 0 4 384 385 386 387
		mu 1 4 103 120 159 142
		f 4 -227 224 -168 -226
		mu 0 4 388 389 390 391
		mu 1 4 144 143 181 182
		f 4 -229 225 -170 -228
		mu 0 4 392 393 394 395
		mu 1 4 148 147 183 184
		f 4 -231 227 171 238
		mu 0 4 599 397 398 399
		mu 1 4 152 151 185 186
		f 4 -233 229 173 -232
		mu 0 4 400 401 402 403
		mu 1 4 156 155 187 188
		f 4 -235 231 175 -234
		mu 0 4 404 405 406 407
		mu 1 4 158 156 188 189
		f 4 -236 233 177 -225
		mu 0 4 408 409 410 411
		mu 1 4 143 160 190 181
		f 4 -238 -239 236 -230
		mu 0 4 412 396 598 413
		mu 1 4 191 152 186 192
		f 4 -240 -241 237 -173
		mu 0 4 414 234 237 415
		mu 1 4 193 149 152 191
		f 4 -242 -243 239 -218
		mu 0 4 416 372 375 417
		mu 1 4 194 112 149 193
		f 4 -244 -245 241 -156
		mu 0 4 418 186 189 419
		mu 1 4 195 109 112 194
		f 3 -246 -247 260
		mu 0 3 420 274 276
		mu 1 3 196 175 109
		f 4 -248 -249 245 -208
		mu 0 4 421 352 355 422
		mu 1 4 197 132 175 196
		f 4 -250 -251 247 -135
		mu 0 4 423 210 213 424
		mu 1 4 198 129 132 197
		f 3 -252 -253 258
		mu 0 3 425 256 258
		mu 1 3 199 165 129
		f 4 -254 -255 251 -196
		mu 0 4 426 328 331 427
		mu 1 4 200 99 165 199
		f 4 -257 253 -109 111
		mu 0 4 177 176 428 429
		mu 1 4 100 99 200 201
		f 3 -258 129 133
		mu 0 3 430 431 432
		mu 1 3 129 166 130
		f 3 -259 249 -133
		mu 0 3 433 430 434
		mu 1 3 199 129 198
		f 3 -260 148 153
		mu 0 3 435 436 437
		mu 1 3 109 176 110
		f 3 -261 243 -153
		mu 0 3 438 435 439
		mu 1 3 196 109 195
		f 4 278 280 -283 -284
		mu 0 4 440 441 442 443
		mu 1 4 202 203 204 205
		f 4 -345 -347 -349 -350
		mu 0 4 444 445 446 447
		mu 1 4 206 207 208 209
		f 4 264 269 -271 -269
		mu 0 4 448 449 450 451
		mu 1 4 210 211 212 213
		f 4 262 271 -273 -270
		mu 0 4 449 452 453 450
		mu 1 4 211 214 215 212
		f 4 -266 273 274 -272
		mu 0 4 452 454 455 453
		mu 1 4 214 216 217 215
		f 4 -262 268 275 -274
		mu 0 4 454 448 451 455
		mu 1 4 216 210 213 217
		f 4 270 277 301 -277
		mu 0 4 451 450 456 457
		mu 1 4 218 219 220 221
		f 4 272 279 300 -278
		mu 0 4 450 453 458 456
		mu 1 4 222 223 224 225
		f 4 -275 281 298 -280
		mu 0 4 453 455 459 458
		mu 1 4 226 227 228 229
		f 4 -276 276 296 -282
		mu 0 4 455 451 457 459
		mu 1 4 230 231 232 233
		f 3 285 -289 291
		mu 0 3 460 461 462
		mu 1 3 234 235 236
		f 4 265 286 293 -286
		mu 0 4 454 452 463 464
		mu 1 4 216 214 237 238
		f 4 287 292 -263 266
		mu 0 4 465 466 467 468
		mu 1 4 239 240 241 242
		f 4 284 290 -288 267
		mu 0 4 469 470 471 472
		mu 1 4 243 244 245 246
		f 4 -292 -285 263 261
		mu 0 4 460 462 473 474
		mu 1 4 234 236 247 248
		f 3 -293 289 -287
		mu 0 3 467 466 475
		mu 1 3 241 240 249
		f 4 -297 294 309 -296
		mu 0 4 459 457 476 477
		mu 1 4 250 251 252 253
		f 4 -299 295 304 -298
		mu 0 4 458 459 477 478
		mu 1 4 254 250 253 255
		f 4 -301 297 306 -300
		mu 0 4 456 458 478 479
		mu 1 4 256 254 255 257
		f 4 -302 299 308 -295
		mu 0 4 457 456 479 476
		mu 1 4 251 256 257 252
		f 4 -305 302 282 -304
		mu 0 4 478 477 443 442
		mu 1 4 255 253 205 204
		f 4 -307 303 -281 -306
		mu 0 4 479 478 442 441
		mu 1 4 257 255 204 203
		f 4 -309 305 -279 -308
		mu 0 4 476 479 441 440
		mu 1 4 252 257 203 202
		f 4 -310 307 283 -303
		mu 0 4 477 476 440 443
		mu 1 4 253 252 202 205
		f 4 -267 310 312 -312
		mu 0 4 480 481 482 483
		mu 1 4 258 259 260 261
		f 4 -265 313 314 -311
		mu 0 4 481 484 485 482
		mu 1 4 259 262 263 260
		f 4 -264 315 316 -314
		mu 0 4 484 486 487 485
		mu 1 4 262 264 265 263
		f 4 -268 311 317 -316
		mu 0 4 486 480 483 487
		mu 1 4 264 258 261 265
		f 4 -313 318 320 -320
		mu 0 4 483 482 488 489
		mu 1 4 266 267 268 269
		f 4 -315 321 322 -319
		mu 0 4 482 485 490 488
		mu 1 4 270 271 272 273
		f 4 -317 323 324 -322
		mu 0 4 485 487 491 490
		mu 1 4 274 275 276 277
		f 4 -318 319 325 -324
		mu 0 4 487 483 489 491
		mu 1 4 278 279 280 281
		f 4 -321 326 328 -328
		mu 0 4 489 488 492 493
		mu 1 4 282 283 284 285
		f 4 -323 329 330 -327
		mu 0 4 488 490 494 492
		mu 1 4 283 286 287 284
		f 4 -325 331 332 -330
		mu 0 4 490 491 495 494
		mu 1 4 286 288 289 287
		f 4 -326 327 333 -332
		mu 0 4 491 489 493 495
		mu 1 4 288 282 285 289
		f 4 -329 334 336 -336
		mu 0 4 493 492 496 497
		mu 1 4 285 284 290 291
		f 4 -331 337 338 -335
		mu 0 4 492 494 498 496
		mu 1 4 284 287 292 290
		f 4 -333 339 340 -338
		mu 0 4 494 495 499 498
		mu 1 4 287 289 293 292
		f 4 -334 335 341 -340
		mu 0 4 495 493 497 499
		mu 1 4 289 285 291 293
		f 4 -337 342 344 -344
		mu 0 4 497 496 445 444
		mu 1 4 294 295 296 297
		f 4 -339 345 346 -343
		mu 0 4 496 498 446 445
		mu 1 4 290 292 208 207
		f 4 -341 347 348 -346
		mu 0 4 498 499 447 446
		mu 1 4 298 299 300 301
		f 4 -342 343 349 -348
		mu 0 4 499 497 444 447
		mu 1 4 293 291 206 209
		f 4 353 352 -352 -351
		mu 0 4 500 501 502 503
		mu 1 4 302 303 304 305
		f 4 357 356 355 354
		mu 0 4 504 505 506 507
		mu 1 4 306 307 308 309
		f 4 361 360 -360 -359
		mu 0 4 508 509 510 511
		mu 1 4 310 311 312 313
		f 4 359 364 -364 -363
		mu 0 4 511 510 512 513
		mu 1 4 313 312 314 315
		f 4 363 -368 -367 365
		mu 0 4 513 512 514 515
		mu 1 4 315 314 316 317
		f 4 366 -370 -362 368
		mu 0 4 515 514 509 508
		mu 1 4 317 316 311 310
		f 4 372 -372 -371 -361
		mu 0 4 509 516 517 510
		mu 1 4 318 319 320 321
		f 4 370 -375 -374 -365
		mu 0 4 510 517 518 512
		mu 1 4 322 323 324 325
		f 4 373 -377 -376 367
		mu 0 4 512 518 519 514
		mu 1 4 326 327 328 329
		f 4 375 -378 -373 369
		mu 0 4 514 519 516 509
		mu 1 4 330 331 332 333
		f 3 -381 379 -379
		mu 0 3 520 521 522
		mu 1 3 334 335 336
		f 4 378 -383 -382 -366
		mu 0 4 515 523 524 513
		mu 1 4 317 337 338 315
		f 4 -386 362 -385 -384
		mu 0 4 525 526 527 528
		mu 1 4 339 340 341 342
		f 4 -389 383 -388 -387
		mu 0 4 529 530 531 532
		mu 1 4 343 344 345 346
		f 4 -369 -391 386 380
		mu 0 4 520 533 534 521
		mu 1 4 334 347 348 335
		f 3 381 -390 384
		mu 0 3 527 535 528
		mu 1 3 341 349 342
		f 4 393 -393 -392 377
		mu 0 4 519 536 537 516
		mu 1 4 350 351 352 353
		f 4 395 -395 -394 376
		mu 0 4 518 538 536 519
		mu 1 4 354 355 351 350
		f 4 397 -397 -396 374
		mu 0 4 517 539 538 518
		mu 1 4 356 357 355 354
		f 4 391 -399 -398 371
		mu 0 4 516 537 539 517
		mu 1 4 353 352 357 356
		f 4 400 -353 -400 394
		mu 0 4 538 502 501 536
		mu 1 4 355 304 303 351
		f 4 401 351 -401 396
		mu 0 4 539 503 502 538
		mu 1 4 357 305 304 355
		f 4 402 350 -402 398
		mu 0 4 537 500 503 539
		mu 1 4 352 302 305 357
		f 4 399 -354 -403 392
		mu 0 4 536 501 500 537
		mu 1 4 351 303 302 352
		f 4 405 -405 -404 385
		mu 0 4 540 541 542 543
		mu 1 4 358 359 360 361
		f 4 403 -408 -407 358
		mu 0 4 543 542 544 545
		mu 1 4 361 360 362 363
		f 4 406 -410 -409 390
		mu 0 4 545 544 546 547
		mu 1 4 363 362 364 365
		f 4 408 -411 -406 388
		mu 0 4 547 546 541 540
		mu 1 4 365 364 359 358
		f 4 413 -413 -412 404
		mu 0 4 541 548 549 542
		mu 1 4 366 367 368 369
		f 4 411 -416 -415 407
		mu 0 4 542 549 550 544
		mu 1 4 370 371 372 373
		f 4 414 -418 -417 409
		mu 0 4 544 550 551 546
		mu 1 4 374 375 376 377
		f 4 416 -419 -414 410
		mu 0 4 546 551 548 541
		mu 1 4 378 379 380 381
		f 4 421 -421 -420 412
		mu 0 4 548 552 553 549
		mu 1 4 382 383 384 385
		f 4 419 -424 -423 415
		mu 0 4 549 553 554 550
		mu 1 4 385 384 386 387
		f 4 422 -426 -425 417
		mu 0 4 550 554 555 551
		mu 1 4 387 386 388 389
		f 4 424 -427 -422 418
		mu 0 4 551 555 552 548
		mu 1 4 389 388 383 382
		f 4 429 -429 -428 420
		mu 0 4 552 556 557 553
		mu 1 4 383 390 391 384
		f 4 427 -432 -431 423
		mu 0 4 553 557 558 554
		mu 1 4 384 391 392 386
		f 4 430 -434 -433 425
		mu 0 4 554 558 559 555
		mu 1 4 386 392 393 388
		f 4 432 -435 -430 426
		mu 0 4 555 559 556 552
		mu 1 4 388 393 390 383
		f 4 436 -355 -436 428
		mu 0 4 556 504 507 557
		mu 1 4 394 395 396 397
		f 4 435 -356 -438 431
		mu 0 4 557 507 506 558
		mu 1 4 391 309 308 392
		f 4 437 -357 -439 433
		mu 0 4 558 506 505 559
		mu 1 4 398 399 400 401
		f 4 438 -358 -437 434
		mu 0 4 559 505 504 556
		mu 1 4 393 307 306 390
		f 4 -44 439 441 -441
		mu 0 4 566 567 568 569
		mu 1 4 402 403 404 405
		f 4 -47 442 444 -444
		mu 0 4 570 571 572 573
		mu 1 4 406 407 408 409
		f 4 -49 443 446 -446
		mu 0 4 574 575 576 577
		mu 1 4 410 406 409 411
		f 4 -50 440 447 -443
		mu 0 4 578 579 580 581
		mu 1 4 407 402 405 408
		f 4 79 449 -451 -449
		mu 0 4 582 583 584 585
		mu 1 4 412 413 414 415
		f 4 82 452 -454 -452
		mu 0 4 586 587 588 589
		mu 1 4 416 417 418 419
		f 4 84 454 -456 -453
		mu 0 4 590 591 592 593
		mu 1 4 417 420 421 418
		f 4 85 451 -457 -450
		mu 0 4 594 595 596 597
		mu 1 4 413 416 419 414;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "Tile10";
	setAttr ".rp" -type "double3" -1.34235595703125 -0.2917981386007284 -9.5716700896562266 ;
	setAttr ".sp" -type "double3" -1.34235595703125 -0.2917981386007284 -9.5716700896562266 ;
createNode mesh -n "Tile10Shape" -p "Tile10";
	addAttr -ci true -sn "mso" -ln "miShadingSamplesOverride" -min 0 -max 1 -at "bool";
	addAttr -ci true -sn "msh" -ln "miShadingSamples" -min 0 -smx 8 -at "float";
	addAttr -ci true -sn "mdo" -ln "miMaxDisplaceOverride" -min 0 -max 1 -at "bool";
	addAttr -ci true -sn "mmd" -ln "miMaxDisplace" -min 0 -smx 1 -at "float";
	setAttr -k off ".v";
	setAttr ".iog[0].og[0].gcl" -type "componentList" 1 "f[0:16]";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.67556339978160818 0.99132020407047794 ;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 65 ".uvst[0].uvsp[0:64]" -type "float2" 0.00081756897 0.61592925
		 0.079374336 0.61592919 0.00082508102 0.61592919 0.0011463463 0.77761114 0.0011463463
		 0.79120326 0.0011463463 0.7776112 0.0011463463 0.79120326 0.0035595745 0.81176353
		 0.0035595745 0.80754757 0.0035595745 0.81176376 0.0035595745 0.80754793 0.0011463463
		 0.79120314 0.0011463463 0.7776112 0.0011463463 0.79120314 0.0011463463 0.77761126
		 0.17976665 0.83020085 0.17976665 0.85021383 0.011290036 0.83020085 0.011290036 0.85021383
		 0.028715147 0.85021383 0.019512421 0.77761126 0.028715147 0.83020085 0.021885715
		 0.80754793 0.019512417 0.79120314 0.019512421 0.79120326 0.021885714 0.81176376 0.00082508102
		 0.60399884 0.019512417 0.77761126 0.079374336 0.60399884 0.16019545 0.85021389 0.021774396
		 0.7776112 0.16019545 0.83020091 0.024142772 0.80754757 0.021774393 0.7912032 0.021774396
		 0.79120326 0.024142772 0.81176353 0.00081591762 0.60252953 0.021774393 0.7776112
		 0.045660321 0.60252953 0.04587619 0.61592919 0.045661025 0.61592925 0.04587619 0.60399884
		 0.13204406 0.88641733 0.13204402 0.99658269 0.021878779 0.99658275 0.021878734 0.88641739
		 0.13204406 0.88641733 0.13204402 0.99658269 0.021878779 0.94150007 0.13204406 0.88641733
		 0.13204402 0.99658269 0.021878779 0.99658275 0.021878734 0.88641739 0.13204406 0.88641733
		 0.13204402 0.99658269 0.021878779 0.99658275 0.021878734 0.88641739 0.13204406 0.88641733
		 0.13204402 0.99658269 0.021878779 0.99658275 0.021878734 0.88641739 0.13204406 0.88641733
		 0.13204402 0.99658269 0.021878779 0.99658275 0.021878734 0.88641739;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 39 ".uvst[1].uvsp[0:38]" -type "float2" 0.67811763 0.99478257
		 0.665012 0.99478257 0.66457951 0.99478257 0.66227472 0.99478257 0.65988851 0.99489617
		 0.65741706 0.99489617 0.674806 0.99123889 0.6763202 0.99123794 0.676319 0.99015814
		 0.6574167 0.99786806 0.67480648 0.99015862 0.6622752 0.99746084 0.65988815 0.99786806
		 0.66457975 0.99746084 0.66501224 0.99746084 0.67811763 0.99746084 0.67632163 0.99248081
		 0.6574173 0.99155855 0.67480528 0.99248224 0.66227472 0.99177396 0.65988874 0.99155855
		 0.66457927 0.99177396 0.66501176 0.99177396 0.67252862 0.99177396 0.67252862 0.99478257
		 0.67252886 0.99746084 0.64074892 0.99853671 0.63881713 0.99614275 0.64031893 0.98923188
		 0.64126438 0.98958981 0.64171308 0.99828434 0.64243525 0.99736965 0.64235401 0.98978168
		 0.64883566 0.99568832 0.64847374 0.98863131 0.65533465 0.99464095 0.65019673 0.99595332
		 0.64970398 0.99060136 0.65466088 0.9890154;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 25 ".pt[0:24]" -type "float3"  0 5.9604643e-010 0 4.7683715e-009 
		0 0 0 0 0 -4.7683715e-009 -3.5762786e-009 0 -4.7683715e-009 0 0 -0.1069738 -0.49415004 
		-0.037285279 -0.10697382 -0.49415004 -0.037285279 -4.7683715e-009 -2.3841857e-009 
		0 0 2.3841857e-009 0 2.3841857e-009 7.1525572e-009 0 0 -2.9802323e-009 0 0 1.1920929e-009 
		0 4.7683715e-009 -1.1920929e-009 0 -4.7683715e-009 -5.9604643e-010 0 -2.3841857e-009 
		-7.1525572e-009 0 0 -5.3644182e-009 0 0 0 0 2.3841857e-009 -1.1920929e-009 0 2.3841857e-009 
		-5.9604643e-010 0 0 -1.1920929e-009 0 0 1.1920929e-009 0 0 5.3644182e-009 0 2.3841857e-009 
		-2.9802323e-009 0 2.3841857e-009 -3.2782554e-009 0 -4.7683715e-009 -3.5762786e-009 
		0;
	setAttr -s 25 ".vt[0:24]"  -2.0061473846 -0.014523621 -9.51830292 1.5258789e-007 -0.014523697 -9.5182972
		 -2.16886663 0.10091591 -9.51830482 -2.36199212 0.10221504 -9.51830101 -2.68471169 -0.37952408 -9.51830292
		 -2.65406132 -0.68190873 -9.69826126 -2.58916068 -0.68040347 -10.1776638 -2.68471193 -0.37952408 -10.010328293
		 -2.36199212 0.10221504 -10.010326385 -2.16886663 0.10091583 -10.010328293 -2.0061469078 -0.014523544 -10.010328293
		 0 -0.014523773 -10.01031971 -2.29549599 -1.57068145 -9.15316677 -2.68471193 -0.37952408 -8.96568108
		 -2.36199212 0.10221495 -8.96568012 -2.16886687 0.10091598 -8.96568203 -2.0061473846 -0.014523697 -8.96568108
		 -0.8555423 -0.014523697 -8.96567917 -0.85554212 -0.014523621 -9.5182991 -0.85554212 -0.014523621 -10.010323524
		 -1.9970926 -1.36300099 -8.96568108 -1.79203081 -1.2620796 -8.96568108 -0.68279928 -1.09648478 -8.96567917
		 -0.74675715 -0.90197158 -9.51829815 0.10878525 -0.90197164 -9.51829624;
	setAttr -s 41 ".ed[0:40]"  0 16 1 1 11 0 0 18 1 0 2 1 2 15 1 2 3 1 3 14 1
		 3 4 1 4 13 0 4 5 0 5 12 0 6 5 0 7 4 0 6 7 0 8 3 1 7 8 0 9 2 1 8 9 0 10 0 1 9 10 0
		 10 19 0 12 13 0 13 14 0 14 15 0 15 16 0 16 17 0 18 1 0 17 18 0 19 11 0 18 19 1 14 20 0
		 12 20 0 15 20 0 16 21 0 20 21 0 17 22 0 21 22 0 18 23 0 1 24 0 23 24 0 22 23 0;
	setAttr -s 17 ".fc[0:16]" -type "polyFaces" 
		f 4 27 -3 0 25
		mu 0 4 38 40 0 36
		mu 1 4 23 24 1 22
		f 4 -1 3 4 24
		mu 0 4 37 3 4 34
		mu 1 4 22 1 2 21
		f 4 -5 5 6 23
		mu 0 4 35 7 8 32
		mu 1 4 21 2 3 19
		f 4 -7 7 8 22
		mu 0 4 33 11 12 30
		mu 1 4 20 4 5 17
		f 4 -9 9 10 21
		mu 0 4 31 15 16 29
		mu 1 4 18 6 7 16
		f 4 -13 -14 11 -10
		mu 0 4 17 21 19 18
		mu 1 4 6 10 8 7
		f 4 -15 -16 12 -8
		mu 0 4 13 23 20 14
		mu 1 4 4 12 9 5
		f 4 -17 -18 14 -6
		mu 0 4 9 25 22 10
		mu 1 4 2 13 11 3
		f 4 -19 -20 16 -4
		mu 0 4 5 27 24 6
		mu 1 4 1 14 13 2
		f 4 29 -21 18 2
		mu 0 4 39 41 26 2
		mu 1 4 24 25 14 1
		f 4 1 -29 -30 26
		mu 0 4 1 28 41 39
		mu 1 4 0 15 25 24
		f 4 -23 -22 31 -31
		mu 0 4 42 43 44 45
		mu 1 4 26 27 28 29
		f 3 -24 30 -33
		mu 0 3 46 47 48
		mu 1 3 30 26 29
		f 4 -25 32 34 -34
		mu 0 4 49 50 51 52
		mu 1 4 31 30 29 32
		f 4 -26 33 36 -36
		mu 0 4 53 54 55 56
		mu 1 4 33 31 32 34
		f 4 -27 37 39 -39
		mu 0 4 57 58 59 60
		mu 1 4 35 36 37 38
		f 4 -28 35 40 -38
		mu 0 4 61 62 63 64
		mu 1 4 36 33 34 37;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "Tile11";
	setAttr ".rp" -type "double3" 1.381450788850425 -0.86291230557023924 -10.146809639772115 ;
	setAttr ".sp" -type "double3" 1.381450788850425 -0.86291230557023924 -10.146809639772115 ;
createNode mesh -n "Tile11Shape" -p "Tile11";
	setAttr -k off ".v";
	setAttr ".iog[0].og[1].gcl" -type "componentList" 1 "f[0:13]";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.66080546379089355 0.98659539222717285 ;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 55 ".uvst[0].uvsp[0:54]" -type "float2" 0.045660321 0.60252953
		 0.045661025 0.61592925 0.00081756897 0.61592925 0.00081591762 0.60252953 0.021774393
		 0.7776112 0.0011463463 0.77761114 0.0011463463 0.79120326 0.021774396 0.79120326
		 0.024142772 0.81176353 0.0035595745 0.81176353 0.0035595745 0.80754757 0.024142772
		 0.80754757 0.021774393 0.7912032 0.0011463463 0.79120314 0.0011463463 0.7776112 0.021774396
		 0.7776112 0.16019545 0.83020091 0.17976665 0.83020085 0.17976665 0.85021383 0.16019545
		 0.85021389 0.011290036 0.83020085 0.028715147 0.83020085 0.028715147 0.85021383 0.011290036
		 0.85021383 0.0011463463 0.79120314 0.019512417 0.79120314 0.019512421 0.77761126
		 0.0011463463 0.77761126 0.0035595745 0.81176376 0.021885714 0.81176376 0.021885715
		 0.80754793 0.0035595745 0.80754793 0.0011463463 0.7776112 0.019512417 0.77761126
		 0.019512421 0.79120326 0.0011463463 0.79120326 0.04587619 0.61592919 0.04587619 0.60399884
		 0.00082508102 0.60399884 0.00082508102 0.61592919 0.13204406 0.88641733 0.13204402
		 0.99658269 0.021878779 0.99658275 0.021878734 0.88641739 0.13204406 0.88641733 0.13204402
		 0.99658269 0.021878779 0.94150007 0.13204406 0.88641733 0.13204402 0.99658269 0.021878779
		 0.99658275 0.021878734 0.88641739 0.13204406 0.88641733 0.13204402 0.99658269 0.021878779
		 0.99658275 0.021878734 0.88641739;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 33 ".uvst[1].uvsp[0:32]" -type "float2" 0.66474712 0.98375952
		 0.66474712 0.98676008 0.65896809 0.98676008 0.65896791 0.98375952 0.65863562 0.98676008
		 0.65863544 0.98375952 0.65686357 0.98676008 0.65686357 0.98375952 0.67264444 0.99121481
		 0.66930681 0.99121457 0.66930687 0.98874313 0.67264444 0.98874336 0.67757314 0.99252939
		 0.67757386 0.99128604 0.67908806 0.99128509 0.67908949 0.99252796 0.67757434 0.99020576
		 0.67908686 0.99020529 0.66633493 0.9912141 0.66633493 0.98874265 0.6586358 0.98943126
		 0.65686399 0.98943126 0.65896827 0.98943126 0.66474736 0.98943126 0.65020841 0.98919153
		 0.64897835 0.9876672 0.64993459 0.98326683 0.6505366 0.98349464 0.65082234 0.98903084
		 0.65128213 0.98844838 0.65123045 0.98361695 0.65535754 0.98737788 0.65512711 0.98288441;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 21 ".vt[0:20]"  2.0086712837 -0.13357659 -10.034215927 2.17243528 -0.019922256 -10.025974274
		 2.36488271 -0.020576019 -10.0097675323 2.67963266 -0.50498879 -9.95979691 2.65987253 -0.81539273 -10.12718868
		 2.635324 -0.83640456 -10.60988522 2.72078586 -0.52877009 -10.44952106 2.40603542 -0.0443573 -10.49949265
		 2.21358776 -0.043703537 -10.51569843 2.049823046 -0.15735778 -10.52394009 2.24445105 -1.67311168 -9.57278919
		 2.63341188 -0.47827867 -9.40975952 2.31866217 0.0061338805 -9.4597311 2.1262145 0.0067879488 -9.47593689
		 1.96244991 -0.10686661 -9.48417759 0.81599182 -0.095239639 -9.58108044 0.86221194 -0.1219497 -10.13111591
		 0.9033649 -0.14573075 -10.62083817 1.93437624 -1.45360732 -9.42113876 1.73147953 -1.35073698 -9.4431839
		 0.62858522 -1.17413533 -9.54443645;
	setAttr -s 34 ".ed[0:33]"  0 14 1 0 16 1 0 1 1 1 13 1 1 2 1 2 12 1 2 3 1
		 3 11 0 3 4 0 4 10 0 5 4 0 6 3 0 5 6 0 7 2 1 6 7 0 8 1 1 7 8 0 9 0 1 8 9 0 9 17 0
		 10 11 0 11 12 0 12 13 0 13 14 0 14 15 0 15 16 0 16 17 0 12 18 0 10 18 0 13 18 0 14 19 0
		 18 19 0 15 20 0 19 20 0;
	setAttr -s 14 ".fc[0:13]" -type "polyFaces" 
		f 4 -25 -1 1 -26
		mu 0 4 0 3 2 1
		mu 1 4 0 3 2 1
		f 4 -24 -4 -3 0
		mu 0 4 4 7 6 5
		mu 1 4 3 5 4 2
		f 4 -23 -6 -5 3
		mu 0 4 8 11 10 9
		mu 1 4 5 7 6 4
		f 4 -22 -8 -7 5
		mu 0 4 12 15 14 13
		mu 1 4 8 11 10 9
		f 4 -21 -10 -9 7
		mu 0 4 16 19 18 17
		mu 1 4 12 15 14 13
		f 4 8 -11 12 11
		mu 0 4 20 23 22 21
		mu 1 4 13 14 17 16
		f 4 6 -12 14 13
		mu 0 4 24 27 26 25
		mu 1 4 9 10 19 18
		f 4 4 -14 16 15
		mu 0 4 28 31 30 29
		mu 1 4 4 6 21 20
		f 4 2 -16 18 17
		mu 0 4 32 35 34 33
		mu 1 4 2 4 20 22
		f 4 -2 -18 19 -27
		mu 0 4 36 39 38 37
		mu 1 4 1 2 22 23
		f 4 27 -29 20 21
		mu 0 4 40 43 42 41
		mu 1 4 24 27 26 25
		f 3 29 -28 22
		mu 0 3 44 46 45
		mu 1 3 28 27 24
		f 4 30 -32 -30 23
		mu 0 4 47 50 49 48
		mu 1 4 29 30 27 28
		f 4 32 -34 -31 24
		mu 0 4 51 54 53 52
		mu 1 4 31 32 30 29;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "Tile12";
	setAttr ".rp" -type "double3" -1.34235595703125 -0.2917981386007284 -9.5716700896562266 ;
	setAttr ".sp" -type "double3" -1.34235595703125 -0.2917981386007284 -9.5716700896562266 ;
createNode mesh -n "Tile12Shape" -p "Tile12";
	addAttr -ci true -sn "mso" -ln "miShadingSamplesOverride" -min 0 -max 1 -at "bool";
	addAttr -ci true -sn "msh" -ln "miShadingSamples" -min 0 -smx 8 -at "float";
	addAttr -ci true -sn "mdo" -ln "miMaxDisplaceOverride" -min 0 -max 1 -at "bool";
	addAttr -ci true -sn "mmd" -ln "miMaxDisplace" -min 0 -smx 1 -at "float";
	setAttr -k off ".v";
	setAttr ".iog[0].og[0].gcl" -type "componentList" 1 "f[0:9]";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.70940337904418516 0.96373615704546023 ;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 39 ".uvst[0].uvsp[0:38]" -type "float2" 0.00081756897 0.61592925
		 0.0011463463 0.77761114 0.0011463463 0.79120326 0.0035595745 0.81176353 0.0035595745
		 0.80754757 0.0011463463 0.79120314 0.0011463463 0.7776112 0.17976665 0.83020085 0.17976665
		 0.85021383 0.16019545 0.85021389 0.021774396 0.7776112 0.16019545 0.83020091 0.024142772
		 0.80754757 0.021774393 0.7912032 0.021774396 0.79120326 0.024142772 0.81176353 0.00081591762
		 0.60252953 0.021774393 0.7776112 0.045660321 0.60252953 0.045661025 0.61592925 0.13204406
		 0.88641733 0.13204402 0.99658269 0.021878779 0.99658275 0.021878734 0.88641739 0.13204406
		 0.88641733 0.13204402 0.99658269 0.021878779 0.94150007 0.13204406 0.88641733 0.13204402
		 0.99658269 0.021878779 0.99658275 0.021878734 0.88641739 0.13204406 0.88641733 0.13204402
		 0.99658269 0.021878779 0.99658275 0.021878734 0.88641739 0.13204406 0.88641733 0.13204402
		 0.99658269 0.021878779 0.99658275 0.021878734 0.88641739;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 27 ".uvst[1].uvsp[0:26]" -type "float2" 0.70847601 0.96226299
		 0.70847601 0.96199632 0.70847607 0.9605754 0.68454027 0.98284626 0.68206882 0.98284626
		 0.6856631 0.98061115 0.6871773 0.98061019 0.68717873 0.98185307 0.68206906 0.97950864
		 0.68566239 0.9818545 0.71033072 0.9605754 0.68454051 0.97950864 0.71033078 0.96199614
		 0.71033078 0.96226281 0.71033072 0.96689695 0.70847607 0.96689695 0.63302112 0.99884439
		 0.63201433 0.99759674 0.632797 0.99399519 0.63328975 0.99418163 0.63352358 0.9987129
		 0.63389993 0.99823618 0.63385761 0.99428165 0.63723552 0.99735987 0.63704693 0.99368215
		 0.63794488 0.99749804 0.6376881 0.9947089;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 16 ".vt[0:15]"  -1.96339643 -0.18636198 -9.51756382 -2.11104107 -0.13151529 -9.51730442
		 -2.2993958 -0.14939249 -9.51721764 -2.66092777 -0.47512341 -9.51789188 -2.81233454 -0.96985871 -9.73643398
		 -2.39638805 -1.16514409 -9.15491199 -2.66115808 -0.47419903 -8.96527386 -2.29962587 -0.14846811 -8.96460056
		 -2.11127138 -0.13059083 -8.96468544 -1.96362638 -0.18543756 -8.96494579 -0.84069663 -0.074196324 -8.96542263
		 -0.84046644 -0.075120665 -9.51803875 -2.085161209 -1.0090068579 -8.96720505 -1.87527454 -0.92747909 -8.96712112
		 -0.77671325 -0.71899492 -8.96730423 -0.82009715 -0.60717958 -9.51956749;
	setAttr -s 25 ".ed[0:24]"  0 9 1 0 11 0 0 1 0 1 8 1 1 2 0 2 7 1 2 3 0
		 3 6 0 3 4 0 4 5 0 5 6 0 6 7 0 7 8 0 8 9 0 9 10 0 10 11 0 7 12 0 5 12 0 8 12 0 9 13 0
		 12 13 0 10 14 0 13 14 0 11 15 0 14 15 0;
	setAttr -s 10 ".fc[0:9]" -type "polyFaces" 
		f 4 15 -2 0 14
		mu 0 4 18 19 0 16
		mu 1 4 14 15 0 13
		f 4 -1 2 3 13
		mu 0 4 17 1 2 14
		mu 1 4 13 0 1 12
		f 4 -4 4 5 12
		mu 0 4 15 3 4 12
		mu 1 4 12 1 2 10
		f 4 -6 6 7 11
		mu 0 4 13 5 6 10
		mu 1 4 11 3 4 8
		f 4 -8 8 9 10
		mu 0 4 11 7 8 9
		mu 1 4 9 5 6 7
		f 4 -12 -11 17 -17
		mu 0 4 20 21 22 23
		mu 1 4 16 17 18 19
		f 3 -13 16 -19
		mu 0 3 24 25 26
		mu 1 3 20 16 19
		f 4 -14 18 20 -20
		mu 0 4 27 28 29 30
		mu 1 4 21 20 19 22
		f 4 -15 19 22 -22
		mu 0 4 31 32 33 34
		mu 1 4 23 21 22 24
		f 4 -16 21 24 -24
		mu 0 4 35 36 37 38
		mu 1 4 25 23 24 26;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode lightLinker -s -n "lightLinker1";
	setAttr -s 3 ".lnk";
	setAttr -s 3 ".slnk";
createNode displayLayerManager -n "layerManager";
createNode displayLayer -n "defaultLayer";
createNode renderLayerManager -n "renderLayerManager";
createNode renderLayer -n "defaultRenderLayer";
	setAttr ".g" yes;
createNode script -n "uiConfigurationScriptNode";
	setAttr ".b" -type "string" (
		"// Maya Mel UI Configuration File.\n//\n//  This script is machine generated.  Edit at your own risk.\n//\n//\n\nglobal string $gMainPane;\nif (`paneLayout -exists $gMainPane`) {\n\n\tglobal int $gUseScenePanelConfig;\n\tint    $useSceneConfig = $gUseScenePanelConfig;\n\tint    $menusOkayInPanels = `optionVar -q allowMenusInPanels`;\tint    $nVisPanes = `paneLayout -q -nvp $gMainPane`;\n\tint    $nPanes = 0;\n\tstring $editorName;\n\tstring $panelName;\n\tstring $itemFilterName;\n\tstring $panelConfig;\n\n\t//\n\t//  get current state of the UI\n\t//\n\tsceneUIReplacement -update $gMainPane;\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"modelPanel\" (localizedPanelLabel(\"Top View\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `modelPanel -unParent -l (localizedPanelLabel(\"Top View\")) -mbv $menusOkayInPanels `;\n\t\t\t$editorName = $panelName;\n            modelEditor -e \n                -camera \"top\" \n                -useInteractiveMode 0\n                -displayLights \"default\" \n                -displayAppearance \"wireframe\" \n"
		+ "                -activeOnly 0\n                -ignorePanZoom 0\n                -wireframeOnShaded 0\n                -headsUpDisplay 1\n                -selectionHiliteDisplay 1\n                -useDefaultMaterial 0\n                -bufferMode \"double\" \n                -twoSidedLighting 1\n                -backfaceCulling 0\n                -xray 0\n                -jointXray 0\n                -activeComponentsXray 0\n                -displayTextures 0\n                -smoothWireframe 0\n                -lineWidth 1\n                -textureAnisotropic 0\n                -textureHilight 1\n                -textureSampling 2\n                -textureDisplay \"modulate\" \n                -textureMaxSize 16384\n                -fogging 0\n                -fogSource \"fragment\" \n                -fogMode \"linear\" \n                -fogStart 0\n                -fogEnd 100\n                -fogDensity 0.1\n                -fogColor 0.5 0.5 0.5 1 \n                -maxConstantTransparency 1\n                -rendererName \"base_OpenGL_Renderer\" \n"
		+ "                -colorResolution 256 256 \n                -bumpResolution 512 512 \n                -textureCompression 0\n                -transparencyAlgorithm \"frontAndBackCull\" \n                -transpInShadows 0\n                -cullingOverride \"none\" \n                -lowQualityLighting 0\n                -maximumNumHardwareLights 0\n                -occlusionCulling 0\n                -shadingModel 0\n                -useBaseRenderer 0\n                -useReducedRenderer 0\n                -smallObjectCulling 0\n                -smallObjectThreshold -1 \n                -interactiveDisableShadows 0\n                -interactiveBackFaceCull 0\n                -sortTransparent 1\n                -nurbsCurves 1\n                -nurbsSurfaces 1\n                -polymeshes 1\n                -subdivSurfaces 1\n                -planes 1\n                -lights 1\n                -cameras 1\n                -controlVertices 1\n                -hulls 1\n                -grid 1\n                -joints 1\n                -ikHandles 1\n"
		+ "                -deformers 1\n                -dynamics 1\n                -fluids 1\n                -hairSystems 1\n                -follicles 1\n                -nCloths 1\n                -nParticles 1\n                -nRigids 1\n                -dynamicConstraints 1\n                -locators 1\n                -manipulators 1\n                -dimensions 1\n                -handles 1\n                -pivots 1\n                -textures 1\n                -strokes 1\n                -motionTrails 1\n                -shadows 0\n                $editorName;\nmodelEditor -e -viewSelected 0 $editorName;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tmodelPanel -edit -l (localizedPanelLabel(\"Top View\")) -mbv $menusOkayInPanels  $panelName;\n\t\t$editorName = $panelName;\n        modelEditor -e \n            -camera \"top\" \n            -useInteractiveMode 0\n            -displayLights \"default\" \n            -displayAppearance \"wireframe\" \n            -activeOnly 0\n            -ignorePanZoom 0\n            -wireframeOnShaded 0\n"
		+ "            -headsUpDisplay 1\n            -selectionHiliteDisplay 1\n            -useDefaultMaterial 0\n            -bufferMode \"double\" \n            -twoSidedLighting 1\n            -backfaceCulling 0\n            -xray 0\n            -jointXray 0\n            -activeComponentsXray 0\n            -displayTextures 0\n            -smoothWireframe 0\n            -lineWidth 1\n            -textureAnisotropic 0\n            -textureHilight 1\n            -textureSampling 2\n            -textureDisplay \"modulate\" \n            -textureMaxSize 16384\n            -fogging 0\n            -fogSource \"fragment\" \n            -fogMode \"linear\" \n            -fogStart 0\n            -fogEnd 100\n            -fogDensity 0.1\n            -fogColor 0.5 0.5 0.5 1 \n            -maxConstantTransparency 1\n            -rendererName \"base_OpenGL_Renderer\" \n            -colorResolution 256 256 \n            -bumpResolution 512 512 \n            -textureCompression 0\n            -transparencyAlgorithm \"frontAndBackCull\" \n            -transpInShadows 0\n            -cullingOverride \"none\" \n"
		+ "            -lowQualityLighting 0\n            -maximumNumHardwareLights 0\n            -occlusionCulling 0\n            -shadingModel 0\n            -useBaseRenderer 0\n            -useReducedRenderer 0\n            -smallObjectCulling 0\n            -smallObjectThreshold -1 \n            -interactiveDisableShadows 0\n            -interactiveBackFaceCull 0\n            -sortTransparent 1\n            -nurbsCurves 1\n            -nurbsSurfaces 1\n            -polymeshes 1\n            -subdivSurfaces 1\n            -planes 1\n            -lights 1\n            -cameras 1\n            -controlVertices 1\n            -hulls 1\n            -grid 1\n            -joints 1\n            -ikHandles 1\n            -deformers 1\n            -dynamics 1\n            -fluids 1\n            -hairSystems 1\n            -follicles 1\n            -nCloths 1\n            -nParticles 1\n            -nRigids 1\n            -dynamicConstraints 1\n            -locators 1\n            -manipulators 1\n            -dimensions 1\n            -handles 1\n            -pivots 1\n"
		+ "            -textures 1\n            -strokes 1\n            -motionTrails 1\n            -shadows 0\n            $editorName;\nmodelEditor -e -viewSelected 0 $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"modelPanel\" (localizedPanelLabel(\"Side View\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `modelPanel -unParent -l (localizedPanelLabel(\"Side View\")) -mbv $menusOkayInPanels `;\n\t\t\t$editorName = $panelName;\n            modelEditor -e \n                -camera \"side\" \n                -useInteractiveMode 0\n                -displayLights \"default\" \n                -displayAppearance \"wireframe\" \n                -activeOnly 0\n                -ignorePanZoom 0\n                -wireframeOnShaded 0\n                -headsUpDisplay 1\n                -selectionHiliteDisplay 1\n                -useDefaultMaterial 0\n                -bufferMode \"double\" \n                -twoSidedLighting 1\n                -backfaceCulling 0\n"
		+ "                -xray 0\n                -jointXray 0\n                -activeComponentsXray 0\n                -displayTextures 0\n                -smoothWireframe 0\n                -lineWidth 1\n                -textureAnisotropic 0\n                -textureHilight 1\n                -textureSampling 2\n                -textureDisplay \"modulate\" \n                -textureMaxSize 16384\n                -fogging 0\n                -fogSource \"fragment\" \n                -fogMode \"linear\" \n                -fogStart 0\n                -fogEnd 100\n                -fogDensity 0.1\n                -fogColor 0.5 0.5 0.5 1 \n                -maxConstantTransparency 1\n                -rendererName \"base_OpenGL_Renderer\" \n                -colorResolution 256 256 \n                -bumpResolution 512 512 \n                -textureCompression 0\n                -transparencyAlgorithm \"frontAndBackCull\" \n                -transpInShadows 0\n                -cullingOverride \"none\" \n                -lowQualityLighting 0\n                -maximumNumHardwareLights 0\n"
		+ "                -occlusionCulling 0\n                -shadingModel 0\n                -useBaseRenderer 0\n                -useReducedRenderer 0\n                -smallObjectCulling 0\n                -smallObjectThreshold -1 \n                -interactiveDisableShadows 0\n                -interactiveBackFaceCull 0\n                -sortTransparent 1\n                -nurbsCurves 1\n                -nurbsSurfaces 1\n                -polymeshes 1\n                -subdivSurfaces 1\n                -planes 1\n                -lights 1\n                -cameras 1\n                -controlVertices 1\n                -hulls 1\n                -grid 1\n                -joints 1\n                -ikHandles 1\n                -deformers 1\n                -dynamics 1\n                -fluids 1\n                -hairSystems 1\n                -follicles 1\n                -nCloths 1\n                -nParticles 1\n                -nRigids 1\n                -dynamicConstraints 1\n                -locators 1\n                -manipulators 1\n                -dimensions 1\n"
		+ "                -handles 1\n                -pivots 1\n                -textures 1\n                -strokes 1\n                -motionTrails 1\n                -shadows 0\n                $editorName;\nmodelEditor -e -viewSelected 0 $editorName;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tmodelPanel -edit -l (localizedPanelLabel(\"Side View\")) -mbv $menusOkayInPanels  $panelName;\n\t\t$editorName = $panelName;\n        modelEditor -e \n            -camera \"side\" \n            -useInteractiveMode 0\n            -displayLights \"default\" \n            -displayAppearance \"wireframe\" \n            -activeOnly 0\n            -ignorePanZoom 0\n            -wireframeOnShaded 0\n            -headsUpDisplay 1\n            -selectionHiliteDisplay 1\n            -useDefaultMaterial 0\n            -bufferMode \"double\" \n            -twoSidedLighting 1\n            -backfaceCulling 0\n            -xray 0\n            -jointXray 0\n            -activeComponentsXray 0\n            -displayTextures 0\n            -smoothWireframe 0\n            -lineWidth 1\n"
		+ "            -textureAnisotropic 0\n            -textureHilight 1\n            -textureSampling 2\n            -textureDisplay \"modulate\" \n            -textureMaxSize 16384\n            -fogging 0\n            -fogSource \"fragment\" \n            -fogMode \"linear\" \n            -fogStart 0\n            -fogEnd 100\n            -fogDensity 0.1\n            -fogColor 0.5 0.5 0.5 1 \n            -maxConstantTransparency 1\n            -rendererName \"base_OpenGL_Renderer\" \n            -colorResolution 256 256 \n            -bumpResolution 512 512 \n            -textureCompression 0\n            -transparencyAlgorithm \"frontAndBackCull\" \n            -transpInShadows 0\n            -cullingOverride \"none\" \n            -lowQualityLighting 0\n            -maximumNumHardwareLights 0\n            -occlusionCulling 0\n            -shadingModel 0\n            -useBaseRenderer 0\n            -useReducedRenderer 0\n            -smallObjectCulling 0\n            -smallObjectThreshold -1 \n            -interactiveDisableShadows 0\n            -interactiveBackFaceCull 0\n"
		+ "            -sortTransparent 1\n            -nurbsCurves 1\n            -nurbsSurfaces 1\n            -polymeshes 1\n            -subdivSurfaces 1\n            -planes 1\n            -lights 1\n            -cameras 1\n            -controlVertices 1\n            -hulls 1\n            -grid 1\n            -joints 1\n            -ikHandles 1\n            -deformers 1\n            -dynamics 1\n            -fluids 1\n            -hairSystems 1\n            -follicles 1\n            -nCloths 1\n            -nParticles 1\n            -nRigids 1\n            -dynamicConstraints 1\n            -locators 1\n            -manipulators 1\n            -dimensions 1\n            -handles 1\n            -pivots 1\n            -textures 1\n            -strokes 1\n            -motionTrails 1\n            -shadows 0\n            $editorName;\nmodelEditor -e -viewSelected 0 $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"modelPanel\" (localizedPanelLabel(\"Front View\")) `;\n\tif (\"\" == $panelName) {\n"
		+ "\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `modelPanel -unParent -l (localizedPanelLabel(\"Front View\")) -mbv $menusOkayInPanels `;\n\t\t\t$editorName = $panelName;\n            modelEditor -e \n                -camera \"front\" \n                -useInteractiveMode 0\n                -displayLights \"default\" \n                -displayAppearance \"wireframe\" \n                -activeOnly 0\n                -ignorePanZoom 0\n                -wireframeOnShaded 0\n                -headsUpDisplay 1\n                -selectionHiliteDisplay 1\n                -useDefaultMaterial 0\n                -bufferMode \"double\" \n                -twoSidedLighting 1\n                -backfaceCulling 0\n                -xray 0\n                -jointXray 0\n                -activeComponentsXray 0\n                -displayTextures 0\n                -smoothWireframe 0\n                -lineWidth 1\n                -textureAnisotropic 0\n                -textureHilight 1\n                -textureSampling 2\n                -textureDisplay \"modulate\" \n                -textureMaxSize 16384\n"
		+ "                -fogging 0\n                -fogSource \"fragment\" \n                -fogMode \"linear\" \n                -fogStart 0\n                -fogEnd 100\n                -fogDensity 0.1\n                -fogColor 0.5 0.5 0.5 1 \n                -maxConstantTransparency 1\n                -rendererName \"base_OpenGL_Renderer\" \n                -colorResolution 256 256 \n                -bumpResolution 512 512 \n                -textureCompression 0\n                -transparencyAlgorithm \"frontAndBackCull\" \n                -transpInShadows 0\n                -cullingOverride \"none\" \n                -lowQualityLighting 0\n                -maximumNumHardwareLights 0\n                -occlusionCulling 0\n                -shadingModel 0\n                -useBaseRenderer 0\n                -useReducedRenderer 0\n                -smallObjectCulling 0\n                -smallObjectThreshold -1 \n                -interactiveDisableShadows 0\n                -interactiveBackFaceCull 0\n                -sortTransparent 1\n                -nurbsCurves 1\n"
		+ "                -nurbsSurfaces 1\n                -polymeshes 1\n                -subdivSurfaces 1\n                -planes 1\n                -lights 1\n                -cameras 1\n                -controlVertices 1\n                -hulls 1\n                -grid 1\n                -joints 1\n                -ikHandles 1\n                -deformers 1\n                -dynamics 1\n                -fluids 1\n                -hairSystems 1\n                -follicles 1\n                -nCloths 1\n                -nParticles 1\n                -nRigids 1\n                -dynamicConstraints 1\n                -locators 1\n                -manipulators 1\n                -dimensions 1\n                -handles 1\n                -pivots 1\n                -textures 1\n                -strokes 1\n                -motionTrails 1\n                -shadows 0\n                $editorName;\nmodelEditor -e -viewSelected 0 $editorName;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tmodelPanel -edit -l (localizedPanelLabel(\"Front View\")) -mbv $menusOkayInPanels  $panelName;\n"
		+ "\t\t$editorName = $panelName;\n        modelEditor -e \n            -camera \"front\" \n            -useInteractiveMode 0\n            -displayLights \"default\" \n            -displayAppearance \"wireframe\" \n            -activeOnly 0\n            -ignorePanZoom 0\n            -wireframeOnShaded 0\n            -headsUpDisplay 1\n            -selectionHiliteDisplay 1\n            -useDefaultMaterial 0\n            -bufferMode \"double\" \n            -twoSidedLighting 1\n            -backfaceCulling 0\n            -xray 0\n            -jointXray 0\n            -activeComponentsXray 0\n            -displayTextures 0\n            -smoothWireframe 0\n            -lineWidth 1\n            -textureAnisotropic 0\n            -textureHilight 1\n            -textureSampling 2\n            -textureDisplay \"modulate\" \n            -textureMaxSize 16384\n            -fogging 0\n            -fogSource \"fragment\" \n            -fogMode \"linear\" \n            -fogStart 0\n            -fogEnd 100\n            -fogDensity 0.1\n            -fogColor 0.5 0.5 0.5 1 \n            -maxConstantTransparency 1\n"
		+ "            -rendererName \"base_OpenGL_Renderer\" \n            -colorResolution 256 256 \n            -bumpResolution 512 512 \n            -textureCompression 0\n            -transparencyAlgorithm \"frontAndBackCull\" \n            -transpInShadows 0\n            -cullingOverride \"none\" \n            -lowQualityLighting 0\n            -maximumNumHardwareLights 0\n            -occlusionCulling 0\n            -shadingModel 0\n            -useBaseRenderer 0\n            -useReducedRenderer 0\n            -smallObjectCulling 0\n            -smallObjectThreshold -1 \n            -interactiveDisableShadows 0\n            -interactiveBackFaceCull 0\n            -sortTransparent 1\n            -nurbsCurves 1\n            -nurbsSurfaces 1\n            -polymeshes 1\n            -subdivSurfaces 1\n            -planes 1\n            -lights 1\n            -cameras 1\n            -controlVertices 1\n            -hulls 1\n            -grid 1\n            -joints 1\n            -ikHandles 1\n            -deformers 1\n            -dynamics 1\n            -fluids 1\n"
		+ "            -hairSystems 1\n            -follicles 1\n            -nCloths 1\n            -nParticles 1\n            -nRigids 1\n            -dynamicConstraints 1\n            -locators 1\n            -manipulators 1\n            -dimensions 1\n            -handles 1\n            -pivots 1\n            -textures 1\n            -strokes 1\n            -motionTrails 1\n            -shadows 0\n            $editorName;\nmodelEditor -e -viewSelected 0 $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"modelPanel\" (localizedPanelLabel(\"Persp View\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `modelPanel -unParent -l (localizedPanelLabel(\"Persp View\")) -mbv $menusOkayInPanels `;\n\t\t\t$editorName = $panelName;\n            modelEditor -e \n                -camera \"persp\" \n                -useInteractiveMode 0\n                -displayLights \"default\" \n                -displayAppearance \"smoothShaded\" \n                -activeOnly 0\n                -ignorePanZoom 0\n"
		+ "                -wireframeOnShaded 0\n                -headsUpDisplay 1\n                -selectionHiliteDisplay 1\n                -useDefaultMaterial 0\n                -bufferMode \"double\" \n                -twoSidedLighting 0\n                -backfaceCulling 0\n                -xray 0\n                -jointXray 0\n                -activeComponentsXray 0\n                -displayTextures 1\n                -smoothWireframe 0\n                -lineWidth 1\n                -textureAnisotropic 0\n                -textureHilight 1\n                -textureSampling 2\n                -textureDisplay \"modulate\" \n                -textureMaxSize 16384\n                -fogging 0\n                -fogSource \"fragment\" \n                -fogMode \"linear\" \n                -fogStart 0\n                -fogEnd 100\n                -fogDensity 0.1\n                -fogColor 0.5 0.5 0.5 1 \n                -maxConstantTransparency 1\n                -rendererName \"base_OpenGL_Renderer\" \n                -colorResolution 256 256 \n                -bumpResolution 512 512 \n"
		+ "                -textureCompression 0\n                -transparencyAlgorithm \"perPolygonSort\" \n                -transpInShadows 0\n                -cullingOverride \"none\" \n                -lowQualityLighting 0\n                -maximumNumHardwareLights 1\n                -occlusionCulling 0\n                -shadingModel 0\n                -useBaseRenderer 0\n                -useReducedRenderer 0\n                -smallObjectCulling 0\n                -smallObjectThreshold -1 \n                -interactiveDisableShadows 0\n                -interactiveBackFaceCull 0\n                -sortTransparent 1\n                -nurbsCurves 1\n                -nurbsSurfaces 1\n                -polymeshes 1\n                -subdivSurfaces 1\n                -planes 1\n                -lights 0\n                -cameras 0\n                -controlVertices 1\n                -hulls 1\n                -grid 0\n                -joints 1\n                -ikHandles 1\n                -deformers 1\n                -dynamics 1\n                -fluids 1\n"
		+ "                -hairSystems 1\n                -follicles 1\n                -nCloths 1\n                -nParticles 1\n                -nRigids 1\n                -dynamicConstraints 1\n                -locators 1\n                -manipulators 1\n                -dimensions 1\n                -handles 1\n                -pivots 1\n                -textures 1\n                -strokes 1\n                -motionTrails 1\n                -shadows 0\n                $editorName;\nmodelEditor -e -viewSelected 0 $editorName;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tmodelPanel -edit -l (localizedPanelLabel(\"Persp View\")) -mbv $menusOkayInPanels  $panelName;\n\t\t$editorName = $panelName;\n        modelEditor -e \n            -camera \"persp\" \n            -useInteractiveMode 0\n            -displayLights \"default\" \n            -displayAppearance \"smoothShaded\" \n            -activeOnly 0\n            -ignorePanZoom 0\n            -wireframeOnShaded 0\n            -headsUpDisplay 1\n            -selectionHiliteDisplay 1\n            -useDefaultMaterial 0\n"
		+ "            -bufferMode \"double\" \n            -twoSidedLighting 0\n            -backfaceCulling 0\n            -xray 0\n            -jointXray 0\n            -activeComponentsXray 0\n            -displayTextures 1\n            -smoothWireframe 0\n            -lineWidth 1\n            -textureAnisotropic 0\n            -textureHilight 1\n            -textureSampling 2\n            -textureDisplay \"modulate\" \n            -textureMaxSize 16384\n            -fogging 0\n            -fogSource \"fragment\" \n            -fogMode \"linear\" \n            -fogStart 0\n            -fogEnd 100\n            -fogDensity 0.1\n            -fogColor 0.5 0.5 0.5 1 \n            -maxConstantTransparency 1\n            -rendererName \"base_OpenGL_Renderer\" \n            -colorResolution 256 256 \n            -bumpResolution 512 512 \n            -textureCompression 0\n            -transparencyAlgorithm \"perPolygonSort\" \n            -transpInShadows 0\n            -cullingOverride \"none\" \n            -lowQualityLighting 0\n            -maximumNumHardwareLights 1\n"
		+ "            -occlusionCulling 0\n            -shadingModel 0\n            -useBaseRenderer 0\n            -useReducedRenderer 0\n            -smallObjectCulling 0\n            -smallObjectThreshold -1 \n            -interactiveDisableShadows 0\n            -interactiveBackFaceCull 0\n            -sortTransparent 1\n            -nurbsCurves 1\n            -nurbsSurfaces 1\n            -polymeshes 1\n            -subdivSurfaces 1\n            -planes 1\n            -lights 0\n            -cameras 0\n            -controlVertices 1\n            -hulls 1\n            -grid 0\n            -joints 1\n            -ikHandles 1\n            -deformers 1\n            -dynamics 1\n            -fluids 1\n            -hairSystems 1\n            -follicles 1\n            -nCloths 1\n            -nParticles 1\n            -nRigids 1\n            -dynamicConstraints 1\n            -locators 1\n            -manipulators 1\n            -dimensions 1\n            -handles 1\n            -pivots 1\n            -textures 1\n            -strokes 1\n            -motionTrails 1\n"
		+ "            -shadows 0\n            $editorName;\nmodelEditor -e -viewSelected 0 $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"outlinerPanel\" (localizedPanelLabel(\"Outliner\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `outlinerPanel -unParent -l (localizedPanelLabel(\"Outliner\")) -mbv $menusOkayInPanels `;\n\t\t\t$editorName = $panelName;\n            outlinerEditor -e \n                -showShapes 0\n                -showAttributes 0\n                -showConnected 0\n                -showAnimCurvesOnly 0\n                -showMuteInfo 0\n                -organizeByLayer 1\n                -showAnimLayerWeight 1\n                -autoExpandLayers 1\n                -autoExpand 0\n                -showDagOnly 1\n                -showAssets 1\n                -showContainedOnly 1\n                -showPublishedAsConnected 0\n                -showContainerContents 1\n                -ignoreDagHierarchy 0\n                -expandConnections 0\n"
		+ "                -showUpstreamCurves 1\n                -showUnitlessCurves 1\n                -showCompounds 1\n                -showLeafs 1\n                -showNumericAttrsOnly 0\n                -highlightActive 1\n                -autoSelectNewObjects 0\n                -doNotSelectNewObjects 0\n                -dropIsParent 1\n                -transmitFilters 0\n                -setFilter \"defaultSetFilter\" \n                -showSetMembers 1\n                -allowMultiSelection 1\n                -alwaysToggleSelect 0\n                -directSelect 0\n                -displayMode \"DAG\" \n                -expandObjects 0\n                -setsIgnoreFilters 1\n                -containersIgnoreFilters 0\n                -editAttrName 0\n                -showAttrValues 0\n                -highlightSecondary 0\n                -showUVAttrsOnly 0\n                -showTextureNodesOnly 0\n                -attrAlphaOrder \"default\" \n                -animLayerFilterOptions \"allAffecting\" \n                -sortOrder \"none\" \n                -longNames 0\n"
		+ "                -niceNames 1\n                -showNamespace 1\n                -showPinIcons 0\n                -mapMotionTrails 0\n                $editorName;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\toutlinerPanel -edit -l (localizedPanelLabel(\"Outliner\")) -mbv $menusOkayInPanels  $panelName;\n\t\t$editorName = $panelName;\n        outlinerEditor -e \n            -showShapes 0\n            -showAttributes 0\n            -showConnected 0\n            -showAnimCurvesOnly 0\n            -showMuteInfo 0\n            -organizeByLayer 1\n            -showAnimLayerWeight 1\n            -autoExpandLayers 1\n            -autoExpand 0\n            -showDagOnly 1\n            -showAssets 1\n            -showContainedOnly 1\n            -showPublishedAsConnected 0\n            -showContainerContents 1\n            -ignoreDagHierarchy 0\n            -expandConnections 0\n            -showUpstreamCurves 1\n            -showUnitlessCurves 1\n            -showCompounds 1\n            -showLeafs 1\n            -showNumericAttrsOnly 0\n            -highlightActive 1\n"
		+ "            -autoSelectNewObjects 0\n            -doNotSelectNewObjects 0\n            -dropIsParent 1\n            -transmitFilters 0\n            -setFilter \"defaultSetFilter\" \n            -showSetMembers 1\n            -allowMultiSelection 1\n            -alwaysToggleSelect 0\n            -directSelect 0\n            -displayMode \"DAG\" \n            -expandObjects 0\n            -setsIgnoreFilters 1\n            -containersIgnoreFilters 0\n            -editAttrName 0\n            -showAttrValues 0\n            -highlightSecondary 0\n            -showUVAttrsOnly 0\n            -showTextureNodesOnly 0\n            -attrAlphaOrder \"default\" \n            -animLayerFilterOptions \"allAffecting\" \n            -sortOrder \"none\" \n            -longNames 0\n            -niceNames 1\n            -showNamespace 1\n            -showPinIcons 0\n            -mapMotionTrails 0\n            $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\tif ($useSceneConfig) {\n\t\toutlinerPanel -e -to $panelName;\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"graphEditor\" (localizedPanelLabel(\"Graph Editor\")) `;\n"
		+ "\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"graphEditor\" -l (localizedPanelLabel(\"Graph Editor\")) -mbv $menusOkayInPanels `;\n\n\t\t\t$editorName = ($panelName+\"OutlineEd\");\n            outlinerEditor -e \n                -showShapes 1\n                -showAttributes 1\n                -showConnected 1\n                -showAnimCurvesOnly 1\n                -showMuteInfo 0\n                -organizeByLayer 1\n                -showAnimLayerWeight 1\n                -autoExpandLayers 1\n                -autoExpand 1\n                -showDagOnly 0\n                -showAssets 1\n                -showContainedOnly 0\n                -showPublishedAsConnected 0\n                -showContainerContents 0\n                -ignoreDagHierarchy 0\n                -expandConnections 1\n                -showUpstreamCurves 1\n                -showUnitlessCurves 1\n                -showCompounds 0\n                -showLeafs 1\n                -showNumericAttrsOnly 1\n                -highlightActive 0\n"
		+ "                -autoSelectNewObjects 1\n                -doNotSelectNewObjects 0\n                -dropIsParent 1\n                -transmitFilters 1\n                -setFilter \"0\" \n                -showSetMembers 0\n                -allowMultiSelection 1\n                -alwaysToggleSelect 0\n                -directSelect 0\n                -displayMode \"DAG\" \n                -expandObjects 0\n                -setsIgnoreFilters 1\n                -containersIgnoreFilters 0\n                -editAttrName 0\n                -showAttrValues 0\n                -highlightSecondary 0\n                -showUVAttrsOnly 0\n                -showTextureNodesOnly 0\n                -attrAlphaOrder \"default\" \n                -animLayerFilterOptions \"allAffecting\" \n                -sortOrder \"none\" \n                -longNames 0\n                -niceNames 1\n                -showNamespace 1\n                -showPinIcons 1\n                -mapMotionTrails 1\n                $editorName;\n\n\t\t\t$editorName = ($panelName+\"GraphEd\");\n            animCurveEditor -e \n"
		+ "                -displayKeys 1\n                -displayTangents 0\n                -displayActiveKeys 0\n                -displayActiveKeyTangents 1\n                -displayInfinities 0\n                -autoFit 0\n                -snapTime \"integer\" \n                -snapValue \"none\" \n                -showResults \"off\" \n                -showBufferCurves \"off\" \n                -smoothness \"fine\" \n                -resultSamples 1.25\n                -resultScreenSamples 0\n                -resultUpdate \"delayed\" \n                -showUpstreamCurves 1\n                -clipTime \"on\" \n                -stackedCurves 0\n                -stackedCurvesMin -1\n                -stackedCurvesMax 1\n                -stackedCurvesSpace 0.2\n                -displayNormalized 0\n                -preSelectionHighlight 0\n                -constrainDrag 0\n                -classicMode 0\n                $editorName;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Graph Editor\")) -mbv $menusOkayInPanels  $panelName;\n"
		+ "\t\t\t$editorName = ($panelName+\"OutlineEd\");\n            outlinerEditor -e \n                -showShapes 1\n                -showAttributes 1\n                -showConnected 1\n                -showAnimCurvesOnly 1\n                -showMuteInfo 0\n                -organizeByLayer 1\n                -showAnimLayerWeight 1\n                -autoExpandLayers 1\n                -autoExpand 1\n                -showDagOnly 0\n                -showAssets 1\n                -showContainedOnly 0\n                -showPublishedAsConnected 0\n                -showContainerContents 0\n                -ignoreDagHierarchy 0\n                -expandConnections 1\n                -showUpstreamCurves 1\n                -showUnitlessCurves 1\n                -showCompounds 0\n                -showLeafs 1\n                -showNumericAttrsOnly 1\n                -highlightActive 0\n                -autoSelectNewObjects 1\n                -doNotSelectNewObjects 0\n                -dropIsParent 1\n                -transmitFilters 1\n                -setFilter \"0\" \n"
		+ "                -showSetMembers 0\n                -allowMultiSelection 1\n                -alwaysToggleSelect 0\n                -directSelect 0\n                -displayMode \"DAG\" \n                -expandObjects 0\n                -setsIgnoreFilters 1\n                -containersIgnoreFilters 0\n                -editAttrName 0\n                -showAttrValues 0\n                -highlightSecondary 0\n                -showUVAttrsOnly 0\n                -showTextureNodesOnly 0\n                -attrAlphaOrder \"default\" \n                -animLayerFilterOptions \"allAffecting\" \n                -sortOrder \"none\" \n                -longNames 0\n                -niceNames 1\n                -showNamespace 1\n                -showPinIcons 1\n                -mapMotionTrails 1\n                $editorName;\n\n\t\t\t$editorName = ($panelName+\"GraphEd\");\n            animCurveEditor -e \n                -displayKeys 1\n                -displayTangents 0\n                -displayActiveKeys 0\n                -displayActiveKeyTangents 1\n                -displayInfinities 0\n"
		+ "                -autoFit 0\n                -snapTime \"integer\" \n                -snapValue \"none\" \n                -showResults \"off\" \n                -showBufferCurves \"off\" \n                -smoothness \"fine\" \n                -resultSamples 1.25\n                -resultScreenSamples 0\n                -resultUpdate \"delayed\" \n                -showUpstreamCurves 1\n                -clipTime \"on\" \n                -stackedCurves 0\n                -stackedCurvesMin -1\n                -stackedCurvesMax 1\n                -stackedCurvesSpace 0.2\n                -displayNormalized 0\n                -preSelectionHighlight 0\n                -constrainDrag 0\n                -classicMode 0\n                $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\tif ($useSceneConfig) {\n\t\tscriptedPanel -e -to $panelName;\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"dopeSheetPanel\" (localizedPanelLabel(\"Dope Sheet\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"dopeSheetPanel\" -l (localizedPanelLabel(\"Dope Sheet\")) -mbv $menusOkayInPanels `;\n"
		+ "\t\t\t$editorName = ($panelName+\"OutlineEd\");\n            outlinerEditor -e \n                -showShapes 1\n                -showAttributes 1\n                -showConnected 1\n                -showAnimCurvesOnly 1\n                -showMuteInfo 0\n                -organizeByLayer 1\n                -showAnimLayerWeight 1\n                -autoExpandLayers 1\n                -autoExpand 0\n                -showDagOnly 0\n                -showAssets 1\n                -showContainedOnly 0\n                -showPublishedAsConnected 0\n                -showContainerContents 0\n                -ignoreDagHierarchy 0\n                -expandConnections 1\n                -showUpstreamCurves 1\n                -showUnitlessCurves 0\n                -showCompounds 1\n                -showLeafs 1\n                -showNumericAttrsOnly 1\n                -highlightActive 0\n                -autoSelectNewObjects 0\n                -doNotSelectNewObjects 1\n                -dropIsParent 1\n                -transmitFilters 0\n                -setFilter \"0\" \n"
		+ "                -showSetMembers 0\n                -allowMultiSelection 1\n                -alwaysToggleSelect 0\n                -directSelect 0\n                -displayMode \"DAG\" \n                -expandObjects 0\n                -setsIgnoreFilters 1\n                -containersIgnoreFilters 0\n                -editAttrName 0\n                -showAttrValues 0\n                -highlightSecondary 0\n                -showUVAttrsOnly 0\n                -showTextureNodesOnly 0\n                -attrAlphaOrder \"default\" \n                -animLayerFilterOptions \"allAffecting\" \n                -sortOrder \"none\" \n                -longNames 0\n                -niceNames 1\n                -showNamespace 1\n                -showPinIcons 0\n                -mapMotionTrails 1\n                $editorName;\n\n\t\t\t$editorName = ($panelName+\"DopeSheetEd\");\n            dopeSheetEditor -e \n                -displayKeys 1\n                -displayTangents 0\n                -displayActiveKeys 0\n                -displayActiveKeyTangents 0\n                -displayInfinities 0\n"
		+ "                -autoFit 0\n                -snapTime \"integer\" \n                -snapValue \"none\" \n                -outliner \"dopeSheetPanel1OutlineEd\" \n                -showSummary 1\n                -showScene 0\n                -hierarchyBelow 0\n                -showTicks 1\n                -selectionWindow 0 0 0 0 \n                $editorName;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Dope Sheet\")) -mbv $menusOkayInPanels  $panelName;\n\n\t\t\t$editorName = ($panelName+\"OutlineEd\");\n            outlinerEditor -e \n                -showShapes 1\n                -showAttributes 1\n                -showConnected 1\n                -showAnimCurvesOnly 1\n                -showMuteInfo 0\n                -organizeByLayer 1\n                -showAnimLayerWeight 1\n                -autoExpandLayers 1\n                -autoExpand 0\n                -showDagOnly 0\n                -showAssets 1\n                -showContainedOnly 0\n                -showPublishedAsConnected 0\n                -showContainerContents 0\n"
		+ "                -ignoreDagHierarchy 0\n                -expandConnections 1\n                -showUpstreamCurves 1\n                -showUnitlessCurves 0\n                -showCompounds 1\n                -showLeafs 1\n                -showNumericAttrsOnly 1\n                -highlightActive 0\n                -autoSelectNewObjects 0\n                -doNotSelectNewObjects 1\n                -dropIsParent 1\n                -transmitFilters 0\n                -setFilter \"0\" \n                -showSetMembers 0\n                -allowMultiSelection 1\n                -alwaysToggleSelect 0\n                -directSelect 0\n                -displayMode \"DAG\" \n                -expandObjects 0\n                -setsIgnoreFilters 1\n                -containersIgnoreFilters 0\n                -editAttrName 0\n                -showAttrValues 0\n                -highlightSecondary 0\n                -showUVAttrsOnly 0\n                -showTextureNodesOnly 0\n                -attrAlphaOrder \"default\" \n                -animLayerFilterOptions \"allAffecting\" \n"
		+ "                -sortOrder \"none\" \n                -longNames 0\n                -niceNames 1\n                -showNamespace 1\n                -showPinIcons 0\n                -mapMotionTrails 1\n                $editorName;\n\n\t\t\t$editorName = ($panelName+\"DopeSheetEd\");\n            dopeSheetEditor -e \n                -displayKeys 1\n                -displayTangents 0\n                -displayActiveKeys 0\n                -displayActiveKeyTangents 0\n                -displayInfinities 0\n                -autoFit 0\n                -snapTime \"integer\" \n                -snapValue \"none\" \n                -outliner \"dopeSheetPanel1OutlineEd\" \n                -showSummary 1\n                -showScene 0\n                -hierarchyBelow 0\n                -showTicks 1\n                -selectionWindow 0 0 0 0 \n                $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"clipEditorPanel\" (localizedPanelLabel(\"Trax Editor\")) `;\n\tif (\"\" == $panelName) {\n"
		+ "\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"clipEditorPanel\" -l (localizedPanelLabel(\"Trax Editor\")) -mbv $menusOkayInPanels `;\n\n\t\t\t$editorName = clipEditorNameFromPanel($panelName);\n            clipEditor -e \n                -displayKeys 0\n                -displayTangents 0\n                -displayActiveKeys 0\n                -displayActiveKeyTangents 0\n                -displayInfinities 0\n                -autoFit 0\n                -snapTime \"none\" \n                -snapValue \"none\" \n                -manageSequencer 0 \n                $editorName;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Trax Editor\")) -mbv $menusOkayInPanels  $panelName;\n\n\t\t\t$editorName = clipEditorNameFromPanel($panelName);\n            clipEditor -e \n                -displayKeys 0\n                -displayTangents 0\n                -displayActiveKeys 0\n                -displayActiveKeyTangents 0\n                -displayInfinities 0\n                -autoFit 0\n"
		+ "                -snapTime \"none\" \n                -snapValue \"none\" \n                -manageSequencer 0 \n                $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"sequenceEditorPanel\" (localizedPanelLabel(\"Camera Sequencer\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"sequenceEditorPanel\" -l (localizedPanelLabel(\"Camera Sequencer\")) -mbv $menusOkayInPanels `;\n\n\t\t\t$editorName = sequenceEditorNameFromPanel($panelName);\n            clipEditor -e \n                -displayKeys 0\n                -displayTangents 0\n                -displayActiveKeys 0\n                -displayActiveKeyTangents 0\n                -displayInfinities 0\n                -autoFit 0\n                -snapTime \"none\" \n                -snapValue \"none\" \n                -manageSequencer 1 \n                $editorName;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Camera Sequencer\")) -mbv $menusOkayInPanels  $panelName;\n"
		+ "\t\t\t$editorName = sequenceEditorNameFromPanel($panelName);\n            clipEditor -e \n                -displayKeys 0\n                -displayTangents 0\n                -displayActiveKeys 0\n                -displayActiveKeyTangents 0\n                -displayInfinities 0\n                -autoFit 0\n                -snapTime \"none\" \n                -snapValue \"none\" \n                -manageSequencer 1 \n                $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"hyperGraphPanel\" (localizedPanelLabel(\"Hypergraph Hierarchy\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"hyperGraphPanel\" -l (localizedPanelLabel(\"Hypergraph Hierarchy\")) -mbv $menusOkayInPanels `;\n\n\t\t\t$editorName = ($panelName+\"HyperGraphEd\");\n            hyperGraph -e \n                -graphLayoutStyle \"hierarchicalLayout\" \n                -orientation \"horiz\" \n                -mergeConnections 0\n                -zoom 1\n"
		+ "                -animateTransition 0\n                -showRelationships 1\n                -showShapes 0\n                -showDeformers 0\n                -showExpressions 0\n                -showConstraints 0\n                -showUnderworld 0\n                -showInvisible 0\n                -transitionFrames 1\n                -opaqueContainers 0\n                -freeform 0\n                -imagePosition 0 0 \n                -imageScale 1\n                -imageEnabled 0\n                -graphType \"DAG\" \n                -heatMapDisplay 0\n                -updateSelection 1\n                -updateNodeAdded 1\n                -useDrawOverrideColor 0\n                -limitGraphTraversal -1\n                -range 0 0 \n                -iconSize \"smallIcons\" \n                -showCachedConnections 0\n                $editorName;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Hypergraph Hierarchy\")) -mbv $menusOkayInPanels  $panelName;\n\n\t\t\t$editorName = ($panelName+\"HyperGraphEd\");\n"
		+ "            hyperGraph -e \n                -graphLayoutStyle \"hierarchicalLayout\" \n                -orientation \"horiz\" \n                -mergeConnections 0\n                -zoom 1\n                -animateTransition 0\n                -showRelationships 1\n                -showShapes 0\n                -showDeformers 0\n                -showExpressions 0\n                -showConstraints 0\n                -showUnderworld 0\n                -showInvisible 0\n                -transitionFrames 1\n                -opaqueContainers 0\n                -freeform 0\n                -imagePosition 0 0 \n                -imageScale 1\n                -imageEnabled 0\n                -graphType \"DAG\" \n                -heatMapDisplay 0\n                -updateSelection 1\n                -updateNodeAdded 1\n                -useDrawOverrideColor 0\n                -limitGraphTraversal -1\n                -range 0 0 \n                -iconSize \"smallIcons\" \n                -showCachedConnections 0\n                $editorName;\n\t\tif (!$useSceneConfig) {\n"
		+ "\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"hyperShadePanel\" (localizedPanelLabel(\"Hypershade\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"hyperShadePanel\" -l (localizedPanelLabel(\"Hypershade\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Hypershade\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\tif ($useSceneConfig) {\n\t\tscriptedPanel -e -to $panelName;\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"visorPanel\" (localizedPanelLabel(\"Visor\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"visorPanel\" -l (localizedPanelLabel(\"Visor\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Visor\")) -mbv $menusOkayInPanels  $panelName;\n"
		+ "\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"createNodePanel\" (localizedPanelLabel(\"Create Node\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"createNodePanel\" -l (localizedPanelLabel(\"Create Node\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Create Node\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"polyTexturePlacementPanel\" (localizedPanelLabel(\"UV Texture Editor\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"polyTexturePlacementPanel\" -l (localizedPanelLabel(\"UV Texture Editor\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"UV Texture Editor\")) -mbv $menusOkayInPanels  $panelName;\n"
		+ "\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\tif ($useSceneConfig) {\n\t\tscriptedPanel -e -to $panelName;\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"renderWindowPanel\" (localizedPanelLabel(\"Render View\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"renderWindowPanel\" -l (localizedPanelLabel(\"Render View\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Render View\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"blendShapePanel\" (localizedPanelLabel(\"Blend Shape\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\tblendShapePanel -unParent -l (localizedPanelLabel(\"Blend Shape\")) -mbv $menusOkayInPanels ;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tblendShapePanel -edit -l (localizedPanelLabel(\"Blend Shape\")) -mbv $menusOkayInPanels  $panelName;\n"
		+ "\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"dynRelEdPanel\" (localizedPanelLabel(\"Dynamic Relationships\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"dynRelEdPanel\" -l (localizedPanelLabel(\"Dynamic Relationships\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Dynamic Relationships\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"relationshipPanel\" (localizedPanelLabel(\"Relationship Editor\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"relationshipPanel\" -l (localizedPanelLabel(\"Relationship Editor\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Relationship Editor\")) -mbv $menusOkayInPanels  $panelName;\n"
		+ "\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"referenceEditorPanel\" (localizedPanelLabel(\"Reference Editor\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"referenceEditorPanel\" -l (localizedPanelLabel(\"Reference Editor\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Reference Editor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"componentEditorPanel\" (localizedPanelLabel(\"Component Editor\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"componentEditorPanel\" -l (localizedPanelLabel(\"Component Editor\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Component Editor\")) -mbv $menusOkayInPanels  $panelName;\n"
		+ "\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"dynPaintScriptedPanelType\" (localizedPanelLabel(\"Paint Effects\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"dynPaintScriptedPanelType\" -l (localizedPanelLabel(\"Paint Effects\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Paint Effects\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"scriptEditorPanel\" (localizedPanelLabel(\"Script Editor\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"scriptEditorPanel\" -l (localizedPanelLabel(\"Script Editor\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Script Editor\")) -mbv $menusOkayInPanels  $panelName;\n"
		+ "\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"Stereo\" (localizedPanelLabel(\"Stereo\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"Stereo\" -l (localizedPanelLabel(\"Stereo\")) -mbv $menusOkayInPanels `;\nstring $editorName = ($panelName+\"Editor\");\n            stereoCameraView -e \n                -editorChanged \"updateModelPanelBar\" \n                -camera \"persp\" \n                -useInteractiveMode 0\n                -displayLights \"default\" \n                -displayAppearance \"wireframe\" \n                -activeOnly 0\n                -ignorePanZoom 0\n                -wireframeOnShaded 0\n                -headsUpDisplay 1\n                -selectionHiliteDisplay 1\n                -useDefaultMaterial 0\n                -bufferMode \"double\" \n                -twoSidedLighting 1\n                -backfaceCulling 0\n                -xray 0\n                -jointXray 0\n                -activeComponentsXray 0\n"
		+ "                -displayTextures 0\n                -smoothWireframe 0\n                -lineWidth 1\n                -textureAnisotropic 0\n                -textureHilight 1\n                -textureSampling 2\n                -textureDisplay \"modulate\" \n                -textureMaxSize 16384\n                -fogging 0\n                -fogSource \"fragment\" \n                -fogMode \"linear\" \n                -fogStart 0\n                -fogEnd 100\n                -fogDensity 0.1\n                -fogColor 0.5 0.5 0.5 1 \n                -maxConstantTransparency 1\n                -colorResolution 4 4 \n                -bumpResolution 4 4 \n                -textureCompression 0\n                -transparencyAlgorithm \"frontAndBackCull\" \n                -transpInShadows 0\n                -cullingOverride \"none\" \n                -lowQualityLighting 0\n                -maximumNumHardwareLights 0\n                -occlusionCulling 0\n                -shadingModel 0\n                -useBaseRenderer 0\n                -useReducedRenderer 0\n"
		+ "                -smallObjectCulling 0\n                -smallObjectThreshold -1 \n                -interactiveDisableShadows 0\n                -interactiveBackFaceCull 0\n                -sortTransparent 1\n                -nurbsCurves 1\n                -nurbsSurfaces 1\n                -polymeshes 1\n                -subdivSurfaces 1\n                -planes 1\n                -lights 1\n                -cameras 1\n                -controlVertices 1\n                -hulls 1\n                -grid 1\n                -joints 1\n                -ikHandles 1\n                -deformers 1\n                -dynamics 1\n                -fluids 1\n                -hairSystems 1\n                -follicles 1\n                -nCloths 1\n                -nParticles 1\n                -nRigids 1\n                -dynamicConstraints 1\n                -locators 1\n                -manipulators 1\n                -dimensions 1\n                -handles 1\n                -pivots 1\n                -textures 1\n                -strokes 1\n                -motionTrails 1\n"
		+ "                -shadows 0\n                -displayMode \"centerEye\" \n                -viewColor 0 0 0 1 \n                $editorName;\nstereoCameraView -e -viewSelected 0 $editorName;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Stereo\")) -mbv $menusOkayInPanels  $panelName;\nstring $editorName = ($panelName+\"Editor\");\n            stereoCameraView -e \n                -editorChanged \"updateModelPanelBar\" \n                -camera \"persp\" \n                -useInteractiveMode 0\n                -displayLights \"default\" \n                -displayAppearance \"wireframe\" \n                -activeOnly 0\n                -ignorePanZoom 0\n                -wireframeOnShaded 0\n                -headsUpDisplay 1\n                -selectionHiliteDisplay 1\n                -useDefaultMaterial 0\n                -bufferMode \"double\" \n                -twoSidedLighting 1\n                -backfaceCulling 0\n                -xray 0\n                -jointXray 0\n                -activeComponentsXray 0\n"
		+ "                -displayTextures 0\n                -smoothWireframe 0\n                -lineWidth 1\n                -textureAnisotropic 0\n                -textureHilight 1\n                -textureSampling 2\n                -textureDisplay \"modulate\" \n                -textureMaxSize 16384\n                -fogging 0\n                -fogSource \"fragment\" \n                -fogMode \"linear\" \n                -fogStart 0\n                -fogEnd 100\n                -fogDensity 0.1\n                -fogColor 0.5 0.5 0.5 1 \n                -maxConstantTransparency 1\n                -colorResolution 4 4 \n                -bumpResolution 4 4 \n                -textureCompression 0\n                -transparencyAlgorithm \"frontAndBackCull\" \n                -transpInShadows 0\n                -cullingOverride \"none\" \n                -lowQualityLighting 0\n                -maximumNumHardwareLights 0\n                -occlusionCulling 0\n                -shadingModel 0\n                -useBaseRenderer 0\n                -useReducedRenderer 0\n"
		+ "                -smallObjectCulling 0\n                -smallObjectThreshold -1 \n                -interactiveDisableShadows 0\n                -interactiveBackFaceCull 0\n                -sortTransparent 1\n                -nurbsCurves 1\n                -nurbsSurfaces 1\n                -polymeshes 1\n                -subdivSurfaces 1\n                -planes 1\n                -lights 1\n                -cameras 1\n                -controlVertices 1\n                -hulls 1\n                -grid 1\n                -joints 1\n                -ikHandles 1\n                -deformers 1\n                -dynamics 1\n                -fluids 1\n                -hairSystems 1\n                -follicles 1\n                -nCloths 1\n                -nParticles 1\n                -nRigids 1\n                -dynamicConstraints 1\n                -locators 1\n                -manipulators 1\n                -dimensions 1\n                -handles 1\n                -pivots 1\n                -textures 1\n                -strokes 1\n                -motionTrails 1\n"
		+ "                -shadows 0\n                -displayMode \"centerEye\" \n                -viewColor 0 0 0 1 \n                $editorName;\nstereoCameraView -e -viewSelected 0 $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\tif ($useSceneConfig) {\n        string $configName = `getPanel -cwl (localizedPanelLabel(\"Current Layout\"))`;\n        if (\"\" != $configName) {\n\t\t\tpanelConfiguration -edit -label (localizedPanelLabel(\"Current Layout\")) \n\t\t\t\t-defaultImage \"\"\n\t\t\t\t-image \"\"\n\t\t\t\t-sc false\n\t\t\t\t-configString \"global string $gMainPane; paneLayout -e -cn \\\"single\\\" -ps 1 100 100 $gMainPane;\"\n\t\t\t\t-removeAllPanels\n\t\t\t\t-ap false\n\t\t\t\t\t(localizedPanelLabel(\"Persp View\")) \n\t\t\t\t\t\"modelPanel\"\n"
		+ "\t\t\t\t\t\"$panelName = `modelPanel -unParent -l (localizedPanelLabel(\\\"Persp View\\\")) -mbv $menusOkayInPanels `;\\n$editorName = $panelName;\\nmodelEditor -e \\n    -cam `findStartUpCamera persp` \\n    -useInteractiveMode 0\\n    -displayLights \\\"default\\\" \\n    -displayAppearance \\\"smoothShaded\\\" \\n    -activeOnly 0\\n    -ignorePanZoom 0\\n    -wireframeOnShaded 0\\n    -headsUpDisplay 1\\n    -selectionHiliteDisplay 1\\n    -useDefaultMaterial 0\\n    -bufferMode \\\"double\\\" \\n    -twoSidedLighting 0\\n    -backfaceCulling 0\\n    -xray 0\\n    -jointXray 0\\n    -activeComponentsXray 0\\n    -displayTextures 1\\n    -smoothWireframe 0\\n    -lineWidth 1\\n    -textureAnisotropic 0\\n    -textureHilight 1\\n    -textureSampling 2\\n    -textureDisplay \\\"modulate\\\" \\n    -textureMaxSize 16384\\n    -fogging 0\\n    -fogSource \\\"fragment\\\" \\n    -fogMode \\\"linear\\\" \\n    -fogStart 0\\n    -fogEnd 100\\n    -fogDensity 0.1\\n    -fogColor 0.5 0.5 0.5 1 \\n    -maxConstantTransparency 1\\n    -rendererName \\\"base_OpenGL_Renderer\\\" \\n    -colorResolution 256 256 \\n    -bumpResolution 512 512 \\n    -textureCompression 0\\n    -transparencyAlgorithm \\\"perPolygonSort\\\" \\n    -transpInShadows 0\\n    -cullingOverride \\\"none\\\" \\n    -lowQualityLighting 0\\n    -maximumNumHardwareLights 1\\n    -occlusionCulling 0\\n    -shadingModel 0\\n    -useBaseRenderer 0\\n    -useReducedRenderer 0\\n    -smallObjectCulling 0\\n    -smallObjectThreshold -1 \\n    -interactiveDisableShadows 0\\n    -interactiveBackFaceCull 0\\n    -sortTransparent 1\\n    -nurbsCurves 1\\n    -nurbsSurfaces 1\\n    -polymeshes 1\\n    -subdivSurfaces 1\\n    -planes 1\\n    -lights 0\\n    -cameras 0\\n    -controlVertices 1\\n    -hulls 1\\n    -grid 0\\n    -joints 1\\n    -ikHandles 1\\n    -deformers 1\\n    -dynamics 1\\n    -fluids 1\\n    -hairSystems 1\\n    -follicles 1\\n    -nCloths 1\\n    -nParticles 1\\n    -nRigids 1\\n    -dynamicConstraints 1\\n    -locators 1\\n    -manipulators 1\\n    -dimensions 1\\n    -handles 1\\n    -pivots 1\\n    -textures 1\\n    -strokes 1\\n    -motionTrails 1\\n    -shadows 0\\n    $editorName;\\nmodelEditor -e -viewSelected 0 $editorName\"\n"
		+ "\t\t\t\t\t\"modelPanel -edit -l (localizedPanelLabel(\\\"Persp View\\\")) -mbv $menusOkayInPanels  $panelName;\\n$editorName = $panelName;\\nmodelEditor -e \\n    -cam `findStartUpCamera persp` \\n    -useInteractiveMode 0\\n    -displayLights \\\"default\\\" \\n    -displayAppearance \\\"smoothShaded\\\" \\n    -activeOnly 0\\n    -ignorePanZoom 0\\n    -wireframeOnShaded 0\\n    -headsUpDisplay 1\\n    -selectionHiliteDisplay 1\\n    -useDefaultMaterial 0\\n    -bufferMode \\\"double\\\" \\n    -twoSidedLighting 0\\n    -backfaceCulling 0\\n    -xray 0\\n    -jointXray 0\\n    -activeComponentsXray 0\\n    -displayTextures 1\\n    -smoothWireframe 0\\n    -lineWidth 1\\n    -textureAnisotropic 0\\n    -textureHilight 1\\n    -textureSampling 2\\n    -textureDisplay \\\"modulate\\\" \\n    -textureMaxSize 16384\\n    -fogging 0\\n    -fogSource \\\"fragment\\\" \\n    -fogMode \\\"linear\\\" \\n    -fogStart 0\\n    -fogEnd 100\\n    -fogDensity 0.1\\n    -fogColor 0.5 0.5 0.5 1 \\n    -maxConstantTransparency 1\\n    -rendererName \\\"base_OpenGL_Renderer\\\" \\n    -colorResolution 256 256 \\n    -bumpResolution 512 512 \\n    -textureCompression 0\\n    -transparencyAlgorithm \\\"perPolygonSort\\\" \\n    -transpInShadows 0\\n    -cullingOverride \\\"none\\\" \\n    -lowQualityLighting 0\\n    -maximumNumHardwareLights 1\\n    -occlusionCulling 0\\n    -shadingModel 0\\n    -useBaseRenderer 0\\n    -useReducedRenderer 0\\n    -smallObjectCulling 0\\n    -smallObjectThreshold -1 \\n    -interactiveDisableShadows 0\\n    -interactiveBackFaceCull 0\\n    -sortTransparent 1\\n    -nurbsCurves 1\\n    -nurbsSurfaces 1\\n    -polymeshes 1\\n    -subdivSurfaces 1\\n    -planes 1\\n    -lights 0\\n    -cameras 0\\n    -controlVertices 1\\n    -hulls 1\\n    -grid 0\\n    -joints 1\\n    -ikHandles 1\\n    -deformers 1\\n    -dynamics 1\\n    -fluids 1\\n    -hairSystems 1\\n    -follicles 1\\n    -nCloths 1\\n    -nParticles 1\\n    -nRigids 1\\n    -dynamicConstraints 1\\n    -locators 1\\n    -manipulators 1\\n    -dimensions 1\\n    -handles 1\\n    -pivots 1\\n    -textures 1\\n    -strokes 1\\n    -motionTrails 1\\n    -shadows 0\\n    $editorName;\\nmodelEditor -e -viewSelected 0 $editorName\"\n"
		+ "\t\t\t\t$configName;\n\n            setNamedPanelLayout (localizedPanelLabel(\"Current Layout\"));\n        }\n\n        panelHistory -e -clear mainPanelHistory;\n        setFocus `paneLayout -q -p1 $gMainPane`;\n        sceneUIReplacement -deleteRemaining;\n        sceneUIReplacement -clear;\n\t}\n\n\ngrid -spacing 5 -size 12 -divisions 5 -displayAxes yes -displayGridLines yes -displayDivisionLines yes -displayPerspectiveLabels no -displayOrthographicLabels yes -displayAxesBold yes -perspectiveLabelPosition axis -orthographicLabelPosition axis;\nviewManip -drawCompass 0 -compassAngle 0 -frontParameters \"\" -homeParameters \"\" -selectionLockParameters \"\";\n}\n");
	setAttr ".st" 3;
createNode script -n "sceneConfigurationScriptNode";
	setAttr ".b" -type "string" "playbackOptions -min 1 -max 60 -ast 1 -aet 60 ";
	setAttr ".st" 6;
createNode mentalrayItemsList -s -n "mentalrayItemsList";
createNode mentalrayGlobals -s -n "mentalrayGlobals";
createNode mentalrayOptions -s -n "miDefaultOptions";
	addAttr -ci true -m -sn "stringOptions" -ln "stringOptions" -at "compound" -nc 
		3;
	addAttr -ci true -sn "name" -ln "name" -dt "string" -p "stringOptions";
	addAttr -ci true -sn "value" -ln "value" -dt "string" -p "stringOptions";
	addAttr -ci true -sn "type" -ln "type" -dt "string" -p "stringOptions";
	setAttr ".maxr" 2;
	setAttr ".fgpd" 5;
	setAttr ".fgtf" 1;
	setAttr ".fgbs" -type "float3" 1.5 1.5 1.5 ;
	setAttr -s 28 ".stringOptions";
	setAttr ".stringOptions[0].name" -type "string" "rast motion factor";
	setAttr ".stringOptions[0].value" -type "string" "1.0";
	setAttr ".stringOptions[0].type" -type "string" "scalar";
	setAttr ".stringOptions[1].name" -type "string" "rast transparency depth";
	setAttr ".stringOptions[1].value" -type "string" "8";
	setAttr ".stringOptions[1].type" -type "string" "integer";
	setAttr ".stringOptions[2].name" -type "string" "rast useopacity";
	setAttr ".stringOptions[2].value" -type "string" "true";
	setAttr ".stringOptions[2].type" -type "string" "boolean";
	setAttr ".stringOptions[3].name" -type "string" "importon";
	setAttr ".stringOptions[3].value" -type "string" "false";
	setAttr ".stringOptions[3].type" -type "string" "boolean";
	setAttr ".stringOptions[4].name" -type "string" "importon density";
	setAttr ".stringOptions[4].value" -type "string" "1.0";
	setAttr ".stringOptions[4].type" -type "string" "scalar";
	setAttr ".stringOptions[5].name" -type "string" "importon merge";
	setAttr ".stringOptions[5].value" -type "string" "0.0";
	setAttr ".stringOptions[5].type" -type "string" "scalar";
	setAttr ".stringOptions[6].name" -type "string" "importon trace depth";
	setAttr ".stringOptions[6].value" -type "string" "0";
	setAttr ".stringOptions[6].type" -type "string" "integer";
	setAttr ".stringOptions[7].name" -type "string" "importon traverse";
	setAttr ".stringOptions[7].value" -type "string" "true";
	setAttr ".stringOptions[7].type" -type "string" "boolean";
	setAttr ".stringOptions[8].name" -type "string" "shadowmap pixel samples";
	setAttr ".stringOptions[8].value" -type "string" "3";
	setAttr ".stringOptions[8].type" -type "string" "integer";
	setAttr ".stringOptions[9].name" -type "string" "ambient occlusion";
	setAttr ".stringOptions[9].value" -type "string" "false";
	setAttr ".stringOptions[9].type" -type "string" "boolean";
	setAttr ".stringOptions[10].name" -type "string" "ambient occlusion rays";
	setAttr ".stringOptions[10].value" -type "string" "256";
	setAttr ".stringOptions[10].type" -type "string" "integer";
	setAttr ".stringOptions[11].name" -type "string" "ambient occlusion cache";
	setAttr ".stringOptions[11].value" -type "string" "false";
	setAttr ".stringOptions[11].type" -type "string" "boolean";
	setAttr ".stringOptions[12].name" -type "string" "ambient occlusion cache density";
	setAttr ".stringOptions[12].value" -type "string" "1.0";
	setAttr ".stringOptions[12].type" -type "string" "scalar";
	setAttr ".stringOptions[13].name" -type "string" "ambient occlusion cache points";
	setAttr ".stringOptions[13].value" -type "string" "64";
	setAttr ".stringOptions[13].type" -type "string" "integer";
	setAttr ".stringOptions[14].name" -type "string" "irradiance particles";
	setAttr ".stringOptions[14].value" -type "string" "false";
	setAttr ".stringOptions[14].type" -type "string" "boolean";
	setAttr ".stringOptions[15].name" -type "string" "irradiance particles rays";
	setAttr ".stringOptions[15].value" -type "string" "256";
	setAttr ".stringOptions[15].type" -type "string" "integer";
	setAttr ".stringOptions[16].name" -type "string" "irradiance particles interpolate";
	setAttr ".stringOptions[16].value" -type "string" "1";
	setAttr ".stringOptions[16].type" -type "string" "integer";
	setAttr ".stringOptions[17].name" -type "string" "irradiance particles interppoints";
	setAttr ".stringOptions[17].value" -type "string" "64";
	setAttr ".stringOptions[17].type" -type "string" "integer";
	setAttr ".stringOptions[18].name" -type "string" "irradiance particles indirect passes";
	setAttr ".stringOptions[18].value" -type "string" "0";
	setAttr ".stringOptions[18].type" -type "string" "integer";
	setAttr ".stringOptions[19].name" -type "string" "irradiance particles scale";
	setAttr ".stringOptions[19].value" -type "string" "1.0";
	setAttr ".stringOptions[19].type" -type "string" "scalar";
	setAttr ".stringOptions[20].name" -type "string" "irradiance particles env";
	setAttr ".stringOptions[20].value" -type "string" "true";
	setAttr ".stringOptions[20].type" -type "string" "boolean";
	setAttr ".stringOptions[21].name" -type "string" "irradiance particles env rays";
	setAttr ".stringOptions[21].value" -type "string" "256";
	setAttr ".stringOptions[21].type" -type "string" "integer";
	setAttr ".stringOptions[22].name" -type "string" "irradiance particles env scale";
	setAttr ".stringOptions[22].value" -type "string" "1";
	setAttr ".stringOptions[22].type" -type "string" "integer";
	setAttr ".stringOptions[23].name" -type "string" "irradiance particles rebuild";
	setAttr ".stringOptions[23].value" -type "string" "true";
	setAttr ".stringOptions[23].type" -type "string" "boolean";
	setAttr ".stringOptions[24].name" -type "string" "irradiance particles file";
	setAttr ".stringOptions[24].value" -type "string" "";
	setAttr ".stringOptions[24].type" -type "string" "string";
	setAttr ".stringOptions[25].name" -type "string" "geom displace motion factor";
	setAttr ".stringOptions[25].value" -type "string" "1.0";
	setAttr ".stringOptions[25].type" -type "string" "scalar";
	setAttr ".stringOptions[26].name" -type "string" "contrast all buffers";
	setAttr ".stringOptions[26].value" -type "string" "true";
	setAttr ".stringOptions[26].type" -type "string" "boolean";
	setAttr ".stringOptions[27].name" -type "string" "finalgather normal tolerance";
	setAttr ".stringOptions[27].value" -type "string" "25.842";
	setAttr ".stringOptions[27].type" -type "string" "scalar";
createNode mentalrayFramebuffer -s -n "miDefaultFramebuffer";
createNode mentalrayOptions -s -n "miContourPreset";
createNode mentalrayOptions -s -n "Draft";
	setAttr ".maxr" 2;
createNode mentalrayOptions -s -n "DraftMotionBlur";
	setAttr ".maxr" 2;
	setAttr ".mb" 1;
	setAttr ".tconr" 1;
	setAttr ".tcong" 1;
	setAttr ".tconb" 1;
	setAttr ".tcona" 1;
createNode mentalrayOptions -s -n "DraftRapidMotion";
	setAttr ".scan" 3;
	setAttr ".rapc" 1;
	setAttr ".raps" 0.25;
	setAttr ".maxr" 2;
	setAttr ".mb" 1;
	setAttr ".tconr" 1;
	setAttr ".tcong" 1;
	setAttr ".tconb" 1;
	setAttr ".tcona" 1;
createNode mentalrayOptions -s -n "Preview";
	setAttr ".minsp" -1;
	setAttr ".maxsp" 1;
	setAttr ".fil" 1;
	setAttr ".rflr" 2;
	setAttr ".rfrr" 2;
	setAttr ".maxr" 4;
createNode mentalrayOptions -s -n "PreviewMotionblur";
	setAttr ".minsp" -1;
	setAttr ".maxsp" 1;
	setAttr ".fil" 1;
	setAttr ".rflr" 2;
	setAttr ".rfrr" 2;
	setAttr ".maxr" 4;
	setAttr ".mb" 1;
	setAttr ".tconr" 0.5;
	setAttr ".tcong" 0.5;
	setAttr ".tconb" 0.5;
	setAttr ".tcona" 0.5;
createNode mentalrayOptions -s -n "PreviewRapidMotion";
	setAttr ".minsp" -1;
	setAttr ".maxsp" 1;
	setAttr ".fil" 1;
	setAttr ".scan" 3;
	setAttr ".rapc" 3;
	setAttr ".rflr" 2;
	setAttr ".rfrr" 2;
	setAttr ".maxr" 4;
	setAttr ".mb" 1;
	setAttr ".tconr" 0.5;
	setAttr ".tcong" 0.5;
	setAttr ".tconb" 0.5;
	setAttr ".tcona" 0.5;
createNode mentalrayOptions -s -n "PreviewCaustics";
	setAttr ".minsp" -1;
	setAttr ".maxsp" 1;
	setAttr ".fil" 1;
	setAttr ".rflr" 2;
	setAttr ".rfrr" 2;
	setAttr ".maxr" 4;
	setAttr ".ca" yes;
	setAttr ".cc" 1;
	setAttr ".cr" 1;
createNode mentalrayOptions -s -n "PreviewGlobalIllum";
	setAttr ".minsp" -1;
	setAttr ".maxsp" 1;
	setAttr ".fil" 1;
	setAttr ".rflr" 2;
	setAttr ".rfrr" 2;
	setAttr ".maxr" 4;
	setAttr ".gi" yes;
	setAttr ".gc" 1;
	setAttr ".gr" 1;
createNode mentalrayOptions -s -n "PreviewFinalGather";
	setAttr ".minsp" -1;
	setAttr ".maxsp" 1;
	setAttr ".fil" 1;
	setAttr ".rflr" 2;
	setAttr ".rfrr" 2;
	setAttr ".maxr" 4;
	setAttr ".fg" yes;
createNode mentalrayOptions -s -n "Production";
	setAttr ".minsp" 0;
	setAttr ".maxsp" 2;
	setAttr ".fil" 2;
	setAttr ".rflr" 10;
	setAttr ".rfrr" 10;
	setAttr ".maxr" 20;
createNode mentalrayOptions -s -n "ProductionMotionblur";
	setAttr ".minsp" 0;
	setAttr ".maxsp" 2;
	setAttr ".fil" 2;
	setAttr ".rflr" 10;
	setAttr ".rfrr" 10;
	setAttr ".maxr" 20;
	setAttr ".mb" 2;
createNode mentalrayOptions -s -n "ProductionRapidMotion";
	setAttr ".minsp" 0;
	setAttr ".maxsp" 2;
	setAttr ".fil" 2;
	setAttr ".scan" 3;
	setAttr ".rapc" 8;
	setAttr ".raps" 2;
	setAttr ".rflr" 10;
	setAttr ".rfrr" 10;
	setAttr ".maxr" 20;
	setAttr ".mb" 2;
createNode mentalrayOptions -s -n "ProductionFineTrace";
	setAttr ".conr" 0.019999999552965164;
	setAttr ".cong" 0.019999999552965164;
	setAttr ".conb" 0.019999999552965164;
	setAttr ".minsp" 1;
	setAttr ".maxsp" 2;
	setAttr ".fil" 1;
	setAttr ".filw" 0.75;
	setAttr ".filh" 0.75;
	setAttr ".jit" yes;
	setAttr ".scan" 0;
createNode mentalrayOptions -s -n "ProductionRapidFur";
	setAttr ".conr" 0.039999999105930328;
	setAttr ".cong" 0.029999999329447746;
	setAttr ".conb" 0.070000000298023224;
	setAttr ".minsp" 0;
	setAttr ".maxsp" 2;
	setAttr ".fil" 1;
	setAttr ".filw" 1.1449999809265137;
	setAttr ".filh" 1.1449999809265137;
	setAttr ".jit" yes;
	setAttr ".scan" 3;
	setAttr ".rapc" 3;
	setAttr ".raps" 0.25;
	setAttr ".ray" no;
	setAttr ".shmth" 3;
	setAttr ".shmap" 3;
	setAttr ".mbsm" no;
	setAttr ".bism" 0.019999999552965164;
createNode mentalrayOptions -s -n "ProductionRapidHair";
	setAttr ".conr" 0.039999999105930328;
	setAttr ".cong" 0.029999999329447746;
	setAttr ".conb" 0.070000000298023224;
	setAttr ".minsp" 0;
	setAttr ".maxsp" 2;
	setAttr ".fil" 1;
	setAttr ".filw" 1.1449999809265137;
	setAttr ".filh" 1.1449999809265137;
	setAttr ".jit" yes;
	setAttr ".scan" 3;
	setAttr ".rapc" 6;
	setAttr ".ray" no;
	setAttr ".shmth" 3;
	setAttr ".shmap" 3;
	setAttr ".mbsm" no;
	setAttr ".bism" 0.019999999552965164;
createNode materialInfo -n "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_materialInfo1";
createNode shadingEngine -n "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG";
	setAttr ".ihi" 0;
	setAttr -s 14 ".dsm";
	setAttr ".ro" yes;
	setAttr -s 4 ".gn";
createNode file -n "file1";
	setAttr ".ftn" -type "string" "C:/Users/vasqm058.DISID/TempleRunOzNew/Assets/Oz/Textures/GameTextures/EmeraldCity/oz_ec_master_opaque.tga";
createNode animLayer -s -n "BaseAnimation";
	setAttr ".pref" yes;
	setAttr ".slct" yes;
	setAttr ".ovrd" yes;
createNode lambert -n "oz_ec_master_opaque";
createNode textureBakeSet -n "troz_texture_bake_set";
	setAttr ".clm" 1;
	setAttr ".format" 6;
	setAttr ".overrideuv" yes;
	setAttr ".set" -type "string" "LightMap";
	setAttr ".bmode" 1;
	setAttr ".bgc" -type "float3" 0.25 0.25 0.25 ;
	setAttr ".fillseams" 5;
	setAttr ".fgq" 3;
	setAttr ".one" yes;
	setAttr ".nsp" 2;
createNode partition -n "textureBakePartition";
createNode animCurveTL -n "greyBlock2_translateY";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  37 -0.11070942842870789 40 -0.033266762193989219
		 46 -0.0069843366958880814;
createNode animCurveTA -n "greyBlock2_rotateX";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  37 0 40 -5.67260453480253 46 -1.4680012583941926;
createNode animCurveTA -n "greyBlock2_rotateY";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  37 0 40 3.9933412504707362;
createNode animCurveTA -n "greyBlock2_rotateZ";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  37 0 40 1.5398366347581272;
createNode animCurveTL -n "greyBlock1_translateY";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  36 -0.10756223920558683 40 0.063787048048309536
		 47 0.069966452416335576;
createNode animCurveTA -n "greyBlock1_rotateX";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  36 0 40 -3.6634503483280327 47 -0.78393216115701647;
createNode animCurveTA -n "greyBlock1_rotateY";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  36 0 40 -2.7138885387729488;
createNode animCurveTA -n "greyBlock1_rotateZ";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  36 0 40 -2.3130405545638468;
createNode groupId -n "groupId80";
	setAttr ".ihi" 0;
createNode motionPath -n "motionPath1";
	setAttr -s 2 ".pmt";
	setAttr -s 2 ".pmt";
	setAttr ".fm" yes;
createNode animCurveTL -n "motionPath1_uValue";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  1 0 30 0.01;
createNode animCurveTL -n "firework_translateX";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 20 ".ktv[0:19]"  1 -2.6160771388047785 12 6.7730538419947877
		 15 8.5873790947054971 20 10.155010368745822 21 9.2635143339704857 22 7.5266489308501878
		 24 5.9705927819025399 25 4.9696104162760015 26 5.3252372194191766 27 6.9470084988916954
		 29 8.0594747547942163 30 7.4372344509258381 31 5.7643816145315769 32 4.1560683298497372
		 34 2.6866077047163879 35 1.2488859159178765 36 -0.1978028613542773 38 -1.9776754002934376
		 44 -1.630051591813048 50 -1.630051591813048;
	setAttr -s 20 ".kit[0:19]"  1 10 1 10 10 10 10 10 
		10 10 10 10 10 10 10 10 10 10 10 10;
	setAttr -s 20 ".kot[0:19]"  1 10 1 10 10 10 10 10 
		10 10 10 10 10 10 10 10 10 10 10 10;
	setAttr -s 20 ".kix[0:19]"  1 0.00041653806692920625 0.00042441918049007654 
		0.0029579752590507269 0.00025364343309774995 0.00030368176521733403 0.00039107722113840282 
		0.0010330217191949487 0.00033714331220835447 0.00036573249963112175 0.0020398721098899841 
		0.0002904747670982033 0.00020317996677476913 0.00032491021556779742 0.00034397531999275088 
		0.00023112770577427 0.00030992753454484046 0.0018618706380948424 1 1;
	setAttr -s 20 ".kiy[0:19]"  0 0.99999988079071045 0.99999988079071045 
		0.9999956488609314 -1 -1 -0.99999988079071045 -0.9999995231628418 0.99999994039535522 
		0.99999994039535522 0.99999797344207764 -1 -1 -0.99999994039535522 -1 -1 -0.99999994039535522 
		-0.99999827146530151 0 0;
	setAttr -s 20 ".kox[0:19]"  1 0.00041653806692920625 0.00042441918049007654 
		0.0029579752590507269 0.00025364343309774995 0.00030368176521733403 0.00039107722113840282 
		0.0010330217191949487 0.00033714331220835447 0.00036573249963112175 0.0020398721098899841 
		0.0002904747670982033 0.00020317996677476913 0.00032491021556779742 0.00034397531999275088 
		0.00023112770577427 0.00030992753454484046 0.0018618706380948424 1 1;
	setAttr -s 20 ".koy[0:19]"  0 0.99999988079071045 0.99999988079071045 
		0.9999956488609314 -1 -1 -0.99999988079071045 -0.9999995231628418 0.99999994039535522 
		0.99999994039535522 0.99999797344207764 -1 -1 -0.99999994039535522 -1 -1 -0.99999994039535522 
		-0.99999827146530151 0 0;
createNode animCurveTL -n "firework_translateY";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 23 ".ktv[0:22]"  1 -0.23317790941261884 12 10.594395459168195
		 15 13.84775083752112 16 15.415091225281042 17 17.089945634217933 19 18.81372336382368
		 20 20.583832236962213 21 22.041495804640551 22 22.115394878856492 24 21.284317297207021
		 25 19.838798420134371 26 18.200874102444072 27 17.765964533078854 29 18.873779929403995
		 30 20.449331138734507 31 20.822089280030543 32 20.10158975074458 34 19.114517252251161
		 35 18.074646511001983 36 17.041855273407108 38 16.995553582988126 44 16.392472299895683
		 50 16.392472299895683;
	setAttr -s 23 ".kit[0:22]"  1 10 1 10 10 10 10 10 
		10 10 10 10 10 10 10 10 10 10 10 10 10 10 10;
	setAttr -s 23 ".kot[0:22]"  1 10 1 10 10 10 10 10 
		10 10 10 10 10 10 10 10 10 10 10 10 10 10 10;
	setAttr -s 23 ".kix[0:22]"  1 0.00033141751191578805 0.00034932480775751173 
		0.00020562201098073274 0.00029423591331578791 0.00028621425735764205 0.00020654076070059091 
		0.0004352852120064199 0.0013206916628405452 0.00043925200589001179 0.00021620850020553917 
		0.00032162084244191647 0.0014860898954793811 0.00037266625440679491 0.00034217699430882931 
		0.0019171315943822265 0.00058562686899676919 0.00049335317453369498 0.0003216477925889194 
		0.00092670408776029944 0.004106427077203989 1 1;
	setAttr -s 23 ".kiy[0:22]"  0 0.99999994039535522 0.99999994039535522 
		1 1 0.99999994039535522 1 0.99999988079071045 -0.99999910593032837 -0.99999988079071045 
		-1 -0.99999994039535522 0.99999892711639404 0.99999988079071045 1 -0.99999815225601196 
		-0.99999988079071045 -0.99999988079071045 -0.99999994039535522 -0.9999995231628418 
		-0.99999153614044189 0 0;
	setAttr -s 23 ".kox[0:22]"  1 0.00033141751191578805 0.00034932480775751173 
		0.00020562201098073274 0.00029423591331578791 0.00028621425735764205 0.00020654076070059091 
		0.0004352852120064199 0.0013206916628405452 0.00043925200589001179 0.00021620850020553917 
		0.00032162084244191647 0.0014860898954793811 0.00037266625440679491 0.00034217699430882931 
		0.0019171315943822265 0.00058562686899676919 0.00049335317453369498 0.0003216477925889194 
		0.00092670408776029944 0.004106427077203989 1 1;
	setAttr -s 23 ".koy[0:22]"  0 0.99999994039535522 0.99999994039535522 
		1 1 0.99999994039535522 1 0.99999988079071045 -0.99999910593032837 -0.99999988079071045 
		-1 -0.99999994039535522 0.99999892711639404 0.99999988079071045 1 -0.99999815225601196 
		-0.99999988079071045 -0.99999988079071045 -0.99999994039535522 -0.9999995231628418 
		-0.99999153614044189 0 0;
createNode animCurveTL -n "firework_translateZ";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 21 ".ktv[0:20]"  1 -1.0372792609011634 12 -2.2168925690300272
		 15 -2.4761576625270063 16 -2.0441350214133958 17 -1.9866624025138355 19 -1.9356002945370085
		 20 -1.9008946220980385 21 -1.9125300597471404 22 -1.9662739041477824 24 -1.9893738604718294
		 25 -1.9769076317052952 26 -1.9509135049132136 27 -1.6474462414683775 29 -1.0160605693420246
		 30 -0.88207948834265493 31 -0.85033306382876728 32 -0.67626549936068958 34 -0.50261932875148319
		 35 -0.37383124892853403 36 -0.30117672146787416 50 -0.30117672146787416;
	setAttr -s 21 ".kit[0:20]"  1 10 1 10 10 10 10 10 
		10 10 10 10 10 10 10 10 10 10 10 10 10;
	setAttr -s 21 ".kot[0:20]"  1 10 1 10 10 10 10 10 
		10 10 10 10 10 10 10 10 10 10 10 10 10;
	setAttr -s 21 ".kix[0:20]"  1 0.0032432498410344124 0.003856458468362689 
		0.0013619458768516779 0.0092132464051246643 0.011658600531518459 0.028885204344987869 
		0.010196379385888577 0.013012313283979893 0.093627281486988068 0.017331261187791824 
		0.0020235003903508186 0.001069685909897089 0.0013065623352304101 0.0040226345881819725 
		0.0032391564454883337 0.002875917823985219 0.0033064826857298613 0.003309446619823575 
		1 1;
	setAttr -s 21 ".kiy[0:20]"  0 -0.99999475479125977 -0.99999260902404785 
		0.99999910593032837 0.99995756149291992 0.99993205070495605 0.99958270788192749 -0.99994802474975586 
		-0.99991530179977417 -0.99560731649398804 0.99984979629516602 0.99999797344207764 
		0.99999940395355225 0.99999910593032837 0.99999189376831055 0.99999475479125977 0.9999958872795105 
		0.99999457597732544 0.99999457597732544 0 0;
	setAttr -s 21 ".kox[0:20]"  1 0.0032432498410344124 0.003856458468362689 
		0.0013619458768516779 0.0092132464051246643 0.011658600531518459 0.028885204344987869 
		0.010196379385888577 0.013012313283979893 0.093627281486988068 0.017331261187791824 
		0.0020235003903508186 0.001069685909897089 0.0013065623352304101 0.0040226345881819725 
		0.0032391564454883337 0.002875917823985219 0.0033064826857298613 0.003309446619823575 
		1 1;
	setAttr -s 21 ".koy[0:20]"  0 -0.99999475479125977 -0.99999260902404785 
		0.99999910593032837 0.99995756149291992 0.99993205070495605 0.99958270788192749 -0.99994802474975586 
		-0.99991530179977417 -0.99560731649398804 0.99984979629516602 0.99999797344207764 
		0.99999940395355225 0.99999910593032837 0.99999189376831055 0.99999475479125977 0.9999958872795105 
		0.99999457597732544 0.99999457597732544 0 0;
createNode animCurveTA -n "firework_rotateX";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 25 ".ktv[0:24]"  1 138.35686708500455 12 151.49703037723609
		 15 171.32815299829321 16 183.57394928058898 17 184.77817054315258 19 197.46429555950704
		 20 141.43670732889257 21 -86.918234425112701 22 -136.60427818476469 24 -186.08008386390287
		 25 -36.73079523830819 26 62.799819700306394 27 132.40741789171136 28 178.78841088634169
		 29 174.94470291128567 30 -49.41160258527426 31 63.383728832554318 32 34.301168651732624
		 33 77.890629998892891 34 50.696062142909007 35 54.126860379751527 36 55.077291733026449
		 38 33.23161026626606 44 66.793860235776023 50 66.793860235776023;
	setAttr -s 25 ".kit[0:24]"  1 10 1 10 10 10 10 10 
		10 10 10 10 10 10 10 10 10 10 10 10 10 10 10 9 10;
	setAttr -s 25 ".kot[0:24]"  1 10 1 10 10 10 10 10 
		10 10 10 10 10 10 10 10 10 10 10 10 10 10 10 9 10;
	setAttr -s 25 ".kix[0:24]"  1 0.62986671924591064 0.70045965909957886 
		1 1 0.13105607032775879 0.013430409133434296 0.013736673630774021 0.057683870196342468 
		0.057274170219898224 0.015345829539000988 0.022577656432986259 0.032914005219936371 
		0.089437082409858704 0.016736123710870743 0.034218788146972656 0.045581478625535965 
		0.25462523102760315 0.22690509259700775 0.15869987010955811 1 1 0.7935369610786438 
		0.56392389535903931 1;
	setAttr -s 25 ".kiy[0:24]"  0 0.77670329809188843 -0.71369200944900513 
		0 0 -0.99137496948242188 -0.99990981817245483 -0.99990570545196533 -0.99833488464355469 
		0.99835854768753052 0.99988222122192383 0.99974507093429565 0.9994581937789917 0.99599248170852661 
		-0.99985992908477783 -0.99941438436508179 0.99896061420440674 0.96703976392745972 
		0.97391688823699951 -0.98732686042785645 0 0 0.60852199792861938 0.82582676410675049 
		0;
	setAttr -s 25 ".kox[0:24]"  1 0.62986671924591064 0.70045965909957886 
		1 1 0.13105607032775879 0.013430409133434296 0.013736673630774021 0.057683870196342468 
		0.057274170219898224 0.015345829539000988 0.022577656432986259 0.032914005219936371 
		0.089437082409858704 0.016736123710870743 0.034218788146972656 0.045581478625535965 
		0.25462523102760315 0.22690509259700775 0.15869987010955811 1 1 0.7935369610786438 
		0.56392389535903931 1;
	setAttr -s 25 ".koy[0:24]"  0 0.77670329809188843 -0.71369200944900513 
		0 0 -0.99137496948242188 -0.99990981817245483 -0.99990570545196533 -0.99833488464355469 
		0.99835854768753052 0.99988222122192383 0.99974507093429565 0.9994581937789917 0.99599248170852661 
		-0.99985992908477783 -0.99941438436508179 0.99896061420440674 0.96703976392745972 
		0.97391688823699951 -0.98732686042785645 0 0 0.60852199792861938 0.82582676410675049 
		0;
createNode animCurveTA -n "firework_rotateY";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 23 ".ktv[0:22]"  1 -86.308232361315078 12 -86.308232361315078
		 15 -86.308232361315078 16 -84.850719513595195 17 -84.039536205059377 19 -81.574426534932357
		 20 85.470941151857986 21 88.297142563128943 22 88.470912017283254 24 89.925351872889181
		 25 87.858906809367241 26 -86.343181783272271 27 -70.372642462208319 28 17.066955697100614
		 29 -17.733547806770961 30 88.829169798135709 31 85.901078076310242 32 82.907043960191658
		 33 -78.329243562284731 34 83.909166209159054 35 85.947259666853327 36 88.328257290550667
		 50 88.328257290550667;
	setAttr -s 23 ".kit[0:22]"  1 10 1 10 10 10 10 10 
		10 10 10 10 10 10 10 10 10 10 10 10 10 10 10;
	setAttr -s 23 ".kot[0:22]"  1 10 1 10 10 10 10 10 
		10 10 10 10 10 10 10 10 10 10 10 10 10 10 10;
	setAttr -s 23 ".kix[0:22]"  1 1 1 1 1 1 1 1 1 1 1 0.024133022874593735 
		0.036912392824888229 0.07237398624420166 0.053152192384004593 0.036832541227340698 
		0.54202622175216675 0.023252034559845924 0.96726536750793457 1 1 1 1;
	setAttr -s 23 ".kiy[0:22]"  0 0 0 0 0 0 0 0 0 0 0 -0.99970871210098267 
		0.99931854009628296 0.99737751483917236 0.99858635663986206 0.99932146072387695 -0.84036159515380859 
		-0.99972963333129883 0.25376713275909424 0 0 0 0;
	setAttr -s 23 ".kox[0:22]"  1 1 1 1 1 1 1 1 1 1 1 0.024133022874593735 
		0.036912392824888229 0.07237398624420166 0.053152192384004593 0.036832541227340698 
		0.54202622175216675 0.023252034559845924 0.96726536750793457 1 1 1 1;
	setAttr -s 23 ".koy[0:22]"  0 0 0 0 0 0 0 0 0 0 0 -0.99970871210098267 
		0.99931854009628296 0.99737751483917236 0.99858635663986206 0.99932146072387695 -0.84036159515380859 
		-0.99972963333129883 0.25376713275909424 0 0 0 0;
createNode animCurveTA -n "firework_rotateZ";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 23 ".ktv[0:22]"  1 -180 12 -180 15 -180 16 -180 17 180 19 180
		 20 -180 21 -4.7584082190556367e-015 22 0 24 0 25 180 26 180 27 -180 28 -185.13181868362128
		 29 203.09396730411791 30 0 31 -180 32 180 33 57.222654590969555 34 -180 35 180 36 180
		 50 180;
	setAttr -s 23 ".kit[0:22]"  1 10 1 10 10 10 10 10 
		10 10 10 10 10 10 10 10 10 10 10 10 10 10 10;
	setAttr -s 23 ".kot[0:22]"  1 10 1 10 10 10 10 10 
		10 10 10 10 10 10 10 10 10 10 10 10 10 10 10;
	setAttr -s 23 ".kix[0:22]"  1 1 1 1 1 1 0.021215880289673805 1 1 1 
		1 1 0.010460630990564823 0.0099702142179012299 0.020628035068511963 0.0099702142179012299 
		0.021215900778770447 0.016099754720926285 0.010609722696244717 0.03109586238861084 
		1 1 1;
	setAttr -s 23 ".kiy[0:22]"  0 0 0 0 0 0 -0.99977493286132813 0 0 0 
		0 0 -0.99994522333145142 0.9999503493309021 0.99978721141815186 -0.9999503493309021 
		0.99977493286132813 0.99987035989761353 -0.99994373321533203 0.99951636791229248 
		0 0 0;
	setAttr -s 23 ".kox[0:22]"  1 1 1 1 1 1 0.021215880289673805 1 1 1 
		1 1 0.010460630990564823 0.0099702142179012299 0.020628035068511963 0.0099702142179012299 
		0.021215900778770447 0.016099754720926285 0.010609722696244717 0.03109586238861084 
		1 1 1;
	setAttr -s 23 ".koy[0:22]"  0 0 0 0 0 0 -0.99977493286132813 0 0 0 
		0 0 -0.99994522333145142 0.9999503493309021 0.99978721141815186 -0.9999503493309021 
		0.99977493286132813 0.99987035989761353 -0.99994373321533203 0.99951636791229248 
		0 0 0;
createNode animCurveTL -n "Tile3_translateY";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  38 -0.10438704420723129 43 0.33831533009011389
		 50 0.27969609181055899;
createNode animCurveTA -n "Tile3_rotateX";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  38 5.8138954960920399 43 10.684296950796709
		 50 0.26052023740810509;
	setAttr ".roti" 4;
createNode animCurveTA -n "Tile3_rotateY";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  38 -1.8860927757050434 43 -33.506979591313033
		 50 -31.317248921193372;
	setAttr ".roti" 4;
createNode animCurveTA -n "Tile3_rotateZ";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  38 0.64624227299330961 43 -7.381465215881807
		 50 3.4716353837672744;
	setAttr ".roti" 4;
createNode animCurveTL -n "Tile4_translateY";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  36 -0.28233709161771353 38 -0.058305145067569501
		 42 -0.075135748930943491;
createNode animCurveTL -n "Tile4_translateZ";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  38 0;
createNode animCurveTA -n "Tile4_rotateX";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  38 -8.4882053383042582 42 -4.4235649114716526;
	setAttr ".roti" 4;
createNode animCurveTA -n "Tile4_rotateY";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  38 9.5952009029010821 42 7.4027332276426314;
	setAttr ".roti" 4;
createNode animCurveTA -n "Tile4_rotateZ";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  38 9.328646600210373 42 4.5834367396733686;
	setAttr ".roti" 4;
createNode animCurveTL -n "Tile5_translateY";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  36 -0.15095050822579956 38 -0.013337813681581392
		 43 -0.06515713301104166;
createNode animCurveTL -n "Tile5_translateZ";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  36 0;
createNode animCurveTA -n "Tile5_rotateX";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  36 -10.331920679071143 38 -11.312123192603988
		 43 -8.8286056126149912;
	setAttr ".roti" 4;
createNode animCurveTA -n "Tile5_rotateY";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  36 7.5707580733515165 38 -1.0999330663513154
		 43 0.10242500792745743;
	setAttr ".roti" 4;
createNode animCurveTA -n "Tile5_rotateZ";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  36 -2.9768821999444453 38 -13.468564832238563
		 43 -3.2508904423095899;
	setAttr ".roti" 4;
createNode animCurveTL -n "Tile7_translateX";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  38 0 43 -1.7667881314348888 50 -2.958583445049177;
createNode animCurveTL -n "Tile7_translateY";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  38 0 43 0.92489967868283018 50 -2.1741730517999591;
createNode animCurveTL -n "Tile7_translateZ";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  38 0 50 -1.7687185032751931;
createNode animCurveTA -n "Tile7_rotateX";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  38 0 50 -4.8660122048495031;
	setAttr ".roti" 4;
createNode animCurveTA -n "Tile7_rotateY";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  38 0 50 17.176411628777934;
	setAttr ".roti" 4;
createNode animCurveTA -n "Tile7_rotateZ";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  38 0 50 158.38154356397612;
	setAttr ".roti" 4;
createNode animCurveTL -n "Tile8_translateX";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  38 0 45 -1.1099599797460862;
createNode animCurveTL -n "Tile8_translateY";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 4 ".ktv[0:3]"  38 0 43 0.91821968488880901 45 0.30538296852436353
		 50 0.21897374182966572;
createNode animCurveTL -n "Tile8_translateZ";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  38 0 45 -1.0461868262919491;
createNode animCurveTA -n "Tile8_rotateX";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  38 0 45 28.473649359316493 50 -4.5013362668228636;
	setAttr ".roti" 4;
createNode animCurveTA -n "Tile8_rotateY";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  38 0 45 28.369173272278065 50 -39.11682620658231;
	setAttr ".roti" 4;
createNode animCurveTA -n "Tile8_rotateZ";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  38 0 45 -82.781180701899459 50 55.552454784441053;
	setAttr ".roti" 4;
createNode animCurveTL -n "Tile6_translateX";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  37 0 41 -0.16008960947000511 50 -0.2094628873193139;
createNode animCurveTL -n "Tile6_translateY";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 4 ".ktv[0:3]"  37 0 41 0.50531942825231957 46 0.30898591613204912
		 50 0.30898591613204912;
createNode animCurveTL -n "Tile6_translateZ";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  37 0 46 0.84474239093889769 50 1.1073032679888843;
createNode animCurveTA -n "Tile6_rotateX";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  37 0 46 -165.51566282906796 50 -82.565017017084074;
	setAttr ".roti" 4;
createNode animCurveTA -n "Tile6_rotateY";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  37 0 46 -69.460504859120036 50 -94.456291008433553;
	setAttr ".roti" 4;
createNode animCurveTA -n "Tile6_rotateZ";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  37 0 46 -4.8921263318571588 50 -92.68063774515835;
	setAttr ".roti" 4;
createNode animCurveTU -n "firework_scaleX";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  1 9.9999999999999998e-013 6 1;
createNode animCurveTU -n "firework_scaleY";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  1 9.9999999999999998e-013 6 1;
createNode animCurveTU -n "firework_scaleZ";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  1 9.9999999999999998e-013 6 1;
createNode animCurveTU -n "Tile7_scaleX";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  46 1.4670500473850558 50 9.9999999999999998e-013;
createNode animCurveTU -n "Tile7_scaleY";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  46 1.4670500473850558 50 9.9999999999999998e-013;
createNode animCurveTU -n "Tile7_scaleZ";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  46 1.4670500473850558 50 9.9999999999999998e-013;
createNode groupId -n "groupId81";
	setAttr ".ihi" 0;
createNode animCurveTL -n "oz_ec_narrows_straight_over_anim_a1_translateX";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  38 0 44 -0.3040705718845077;
createNode animCurveTL -n "oz_ec_narrows_straight_over_anim_a1_translateY";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  38 0 44 0.98890334423725845 50 0.93814551334857965;
createNode animCurveTL -n "oz_ec_narrows_straight_over_anim_a1_translateZ";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  38 0;
createNode animCurveTA -n "oz_ec_narrows_straight_over_anim_a1_rotateX";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  38 0 50 -2.0712457550762191;
createNode animCurveTA -n "oz_ec_narrows_straight_over_anim_a1_rotateY";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  38 0 50 10.279686720613981;
createNode animCurveTA -n "oz_ec_narrows_straight_over_anim_a1_rotateZ";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  38 0 44 -14.028159356058978 50 -11.45654124799208;
createNode animCurveTL -n "polySurface1_translateX";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  36 0;
createNode animCurveTL -n "polySurface1_translateY";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  36 0 42 0.29680219046675993;
createNode animCurveTL -n "polySurface1_translateZ";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  36 0;
createNode animCurveTA -n "polySurface1_rotateX";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  36 0 42 -2.4162048473335029;
createNode animCurveTA -n "polySurface1_rotateY";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  36 0 42 -5.5372154492299819;
createNode animCurveTA -n "polySurface1_rotateZ";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  36 0 42 23.619596712225725 50 5.7902282375575131;
createNode groupId -n "groupId83";
	setAttr ".ihi" 0;
createNode groupId -n "groupId84";
	setAttr ".ihi" 0;
createNode groupId -n "groupId85";
	setAttr ".ihi" 0;
createNode animCurveTL -n "Tile12_translateX";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  37 0.10004483719658121 46 -0.11323120211848225;
createNode animCurveTL -n "Tile12_translateY";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  37 -0.10170457100029229 46 0.73514001750453095
		 50 0.64058280378123078;
createNode animCurveTL -n "Tile12_translateZ";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  37 0.682967952817084 46 0.9021921174662435;
createNode animCurveTA -n "Tile12_rotateX";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  37 -8.2837865245587619 46 19.333170973465293
		 50 17.01839412162969;
createNode animCurveTA -n "Tile12_rotateY";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  37 0.74615547180241193 46 -6.3331463654624924
		 50 -1.5991801503888519;
createNode animCurveTA -n "Tile12_rotateZ";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  37 -8.5752620908507673 46 -18.818214273949554
		 50 -18.059751528155196;
createNode animCurveTL -n "Tile9_translateX";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  37 -0.35060839727855869 42 -1.0067009821752799
		 50 -1.3135609145117977;
createNode animCurveTL -n "Tile9_translateY";
	setAttr ".tan" 1;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  37 0 42 0.60005181609050506 46 0.084363989189585958;
	setAttr -s 3 ".kit[0:2]"  10 1 1;
	setAttr -s 3 ".kot[0:2]"  10 1 1;
	setAttr -s 3 ".kix[1:2]"  0.0023615050595253706 0.00059873738791793585;
	setAttr -s 3 ".kiy[1:2]"  0.99999719858169556 -0.99999982118606567;
	setAttr -s 3 ".kox[1:2]"  0.002361504128202796 0.00046680818195454776;
	setAttr -s 3 ".koy[1:2]"  0.99999725818634033 -0.99999988079071045;
createNode animCurveTL -n "Tile9_translateZ";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 4 ".ktv[0:3]"  37 0 42 1.0315257314092505 46 2.0473865477863149
		 50 3.5470472794481815;
	setAttr -s 4 ".kit[3]"  16;
	setAttr -s 4 ".kot[3]"  16;
createNode animCurveTA -n "Tile9_rotateX";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 4 ".ktv[0:3]"  37 0 42 37.068903123470712 46 193.71354367371598
		 50 280.60445763340653;
createNode animCurveTA -n "Tile9_rotateY";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 4 ".ktv[0:3]"  37 0 42 -46.086648821617061 46 8.2460151350676885
		 50 6.5189395359379576;
createNode animCurveTA -n "Tile9_rotateZ";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 4 ".ktv[0:3]"  37 0 42 -7.2424239426617456 46 2.2198363750671239
		 50 -19.350332010034663;
select -ne :time1;
	setAttr -av -k on ".cch";
	setAttr -cb on ".ihi";
	setAttr -k on ".nds";
	setAttr -cb on ".bnm";
	setAttr ".o" 5;
	setAttr ".unw" 5;
select -ne :renderPartition;
	setAttr -k on ".cch";
	setAttr -cb on ".ihi";
	setAttr -k on ".nds";
	setAttr -cb on ".bnm";
	setAttr -s 3 ".st";
	setAttr -cb on ".an";
	setAttr -cb on ".pt";
select -ne :initialShadingGroup;
	setAttr -k on ".cch";
	setAttr -cb on ".ihi";
	setAttr -av -k on ".nds";
	setAttr -cb on ".bnm";
	setAttr -k on ".mwc";
	setAttr -cb on ".an";
	setAttr -cb on ".il";
	setAttr -cb on ".vo";
	setAttr -cb on ".eo";
	setAttr -cb on ".fo";
	setAttr -cb on ".epo";
	setAttr ".ro" yes;
	setAttr -cb on ".mimt";
	setAttr -cb on ".miop";
	setAttr -cb on ".mise";
	setAttr -cb on ".mism";
	setAttr -cb on ".mice";
	setAttr -av -cb on ".micc";
	setAttr -cb on ".mica";
	setAttr -cb on ".micw";
	setAttr -cb on ".mirw";
select -ne :initialParticleSE;
	setAttr -k on ".cch";
	setAttr -cb on ".ihi";
	setAttr -k on ".nds";
	setAttr -cb on ".bnm";
	setAttr -k on ".mwc";
	setAttr -cb on ".an";
	setAttr -cb on ".il";
	setAttr -cb on ".vo";
	setAttr -cb on ".eo";
	setAttr -cb on ".fo";
	setAttr -cb on ".epo";
	setAttr ".ro" yes;
	setAttr -cb on ".mimt";
	setAttr -cb on ".miop";
	setAttr -cb on ".mise";
	setAttr -cb on ".mism";
	setAttr -cb on ".mice";
	setAttr -cb on ".micc";
	setAttr -cb on ".mica";
	setAttr -cb on ".micw";
	setAttr -cb on ".mirw";
select -ne :defaultShaderList1;
	setAttr -k on ".cch";
	setAttr -cb on ".ihi";
	setAttr -k on ".nds";
	setAttr -cb on ".bnm";
	setAttr -s 3 ".s";
select -ne :defaultTextureList1;
select -ne :lightList1;
	setAttr -s 2 ".l";
select -ne :postProcessList1;
	setAttr -k on ".cch";
	setAttr -cb on ".ihi";
	setAttr -k on ".nds";
	setAttr -cb on ".bnm";
	setAttr -s 2 ".p";
select -ne :defaultRenderingList1;
select -ne :renderGlobalsList1;
	setAttr -k on ".cch";
	setAttr -cb on ".ihi";
	setAttr -k on ".nds";
	setAttr -cb on ".bnm";
select -ne :defaultRenderGlobals;
	setAttr ".mcfr" 30;
	setAttr ".ren" -type "string" "mentalRay";
select -ne :defaultResolution;
	setAttr -k on ".cch";
	setAttr -k on ".nds";
	setAttr -av ".w";
	setAttr -av ".h";
	setAttr ".pa" 1;
	setAttr -k on ".al";
	setAttr -av ".dar";
	setAttr -k on ".ldar";
	setAttr -k on ".off";
	setAttr -k on ".fld";
	setAttr -k on ".zsl";
select -ne :defaultLightSet;
	setAttr -k on ".cch";
	setAttr -k on ".ihi";
	setAttr -k on ".nds";
	setAttr -k on ".bnm";
	setAttr -s 2 ".dsm";
	setAttr -k on ".mwc";
	setAttr -k on ".an";
	setAttr -k on ".il";
	setAttr -k on ".vo";
	setAttr -k on ".eo";
	setAttr -k on ".fo";
	setAttr -k on ".epo";
	setAttr ".ro" yes;
select -ne :defaultObjectSet;
	setAttr ".ro" yes;
select -ne :hardwareRenderGlobals;
	setAttr -k on ".cch";
	setAttr -cb on ".ihi";
	setAttr -k on ".nds";
	setAttr -cb on ".bnm";
	setAttr ".ctrs" 256;
	setAttr ".btrs" 512;
	setAttr -k off ".fbfm";
	setAttr -k off -cb on ".ehql";
	setAttr -k off -cb on ".eams";
	setAttr -k off -cb on ".eeaa";
	setAttr -k off -cb on ".engm";
	setAttr -k off -cb on ".mes";
	setAttr -k off -cb on ".emb";
	setAttr -av -k off -cb on ".mbbf";
	setAttr -k off -cb on ".mbs";
	setAttr -k off -cb on ".trm";
	setAttr -k off -cb on ".tshc";
	setAttr -k off ".enpt";
	setAttr -k off -cb on ".clmt";
	setAttr -k off -cb on ".tcov";
	setAttr -k off -cb on ".lith";
	setAttr -k off -cb on ".sobc";
	setAttr -k off -cb on ".cuth";
	setAttr -k off -cb on ".hgcd";
	setAttr -k off -cb on ".hgci";
	setAttr -k off -cb on ".mgcs";
	setAttr -k off -cb on ".twa";
	setAttr -k off -cb on ".twz";
	setAttr -cb on ".hwcc";
	setAttr -cb on ".hwdp";
	setAttr -cb on ".hwql";
	setAttr ".hwfr" 30;
select -ne :defaultHardwareRenderGlobals;
	setAttr -k on ".cch";
	setAttr -cb on ".ihi";
	setAttr -k on ".nds";
	setAttr -cb on ".bnm";
	setAttr -k on ".rp";
	setAttr -k on ".cai";
	setAttr -k on ".coi";
	setAttr -cb on ".bc";
	setAttr -av -k on ".bcb";
	setAttr -av -k on ".bcg";
	setAttr -av -k on ".bcr";
	setAttr -k on ".ei";
	setAttr -k on ".ex";
	setAttr -av -k on ".es";
	setAttr -av -k on ".ef";
	setAttr -av -cb on ".bf";
	setAttr -k on ".fii";
	setAttr -av -cb on ".sf";
	setAttr -k on ".gr";
	setAttr -k on ".li";
	setAttr -k on ".ls";
	setAttr -k on ".mb";
	setAttr -k on ".ti";
	setAttr -k on ".txt";
	setAttr -k on ".mpr";
	setAttr -k on ".wzd";
	setAttr ".fn" -type "string" "im";
	setAttr -k on ".if";
	setAttr ".res" -type "string" "ntsc_4d 646 485 1.333";
	setAttr -k on ".as";
	setAttr -k on ".ds";
	setAttr -k on ".lm";
	setAttr -k on ".fir";
	setAttr -k on ".aap";
	setAttr -k on ".gh";
	setAttr -cb on ".sd";
connectAttr "greyBlock1_translateY.o" "Tile1.ty";
connectAttr "greyBlock1_rotateX.o" "Tile1.rx";
connectAttr "greyBlock1_rotateY.o" "Tile1.ry";
connectAttr "greyBlock1_rotateZ.o" "Tile1.rz";
connectAttr "greyBlock2_translateY.o" "Tile2.ty";
connectAttr "greyBlock2_rotateX.o" "Tile2.rx";
connectAttr "greyBlock2_rotateY.o" "Tile2.ry";
connectAttr "greyBlock2_rotateZ.o" "Tile2.rz";
connectAttr "firework_translateX.o" "firework.tx";
connectAttr "firework_translateY.o" "firework.ty";
connectAttr "firework_translateZ.o" "firework.tz";
connectAttr "firework_rotateX.o" "firework.rx";
connectAttr "firework_rotateY.o" "firework.ry";
connectAttr "firework_rotateZ.o" "firework.rz";
connectAttr "motionPath1.msg" "firework.sml";
connectAttr "motionPath1.ro" "firework.ro";
connectAttr "firework_scaleX.o" "firework.sx";
connectAttr "firework_scaleY.o" "firework.sy";
connectAttr "firework_scaleZ.o" "firework.sz";
connectAttr "Tile3_translateY.o" "Tile3.ty";
connectAttr "Tile3_rotateX.o" "Tile3.rx";
connectAttr "Tile3_rotateY.o" "Tile3.ry";
connectAttr "Tile3_rotateZ.o" "Tile3.rz";
connectAttr "Tile4_translateY.o" "Tile4.ty";
connectAttr "Tile4_translateZ.o" "Tile4.tz";
connectAttr "Tile4_rotateX.o" "Tile4.rx";
connectAttr "Tile4_rotateY.o" "Tile4.ry";
connectAttr "Tile4_rotateZ.o" "Tile4.rz";
connectAttr "Tile5_translateY.o" "Tile5.ty";
connectAttr "Tile5_translateZ.o" "Tile5.tz";
connectAttr "Tile5_rotateX.o" "Tile5.rx";
connectAttr "Tile5_rotateY.o" "Tile5.ry";
connectAttr "Tile5_rotateZ.o" "Tile5.rz";
connectAttr "Tile6_translateY.o" "Tile6.ty";
connectAttr "Tile6_translateX.o" "Tile6.tx";
connectAttr "Tile6_translateZ.o" "Tile6.tz";
connectAttr "Tile6_rotateX.o" "Tile6.rx";
connectAttr "Tile6_rotateY.o" "Tile6.ry";
connectAttr "Tile6_rotateZ.o" "Tile6.rz";
connectAttr "Tile7_translateY.o" "Tile7.ty";
connectAttr "Tile7_translateX.o" "Tile7.tx";
connectAttr "Tile7_translateZ.o" "Tile7.tz";
connectAttr "Tile7_rotateX.o" "Tile7.rx";
connectAttr "Tile7_rotateY.o" "Tile7.ry";
connectAttr "Tile7_rotateZ.o" "Tile7.rz";
connectAttr "Tile7_scaleX.o" "Tile7.sx";
connectAttr "Tile7_scaleY.o" "Tile7.sy";
connectAttr "Tile7_scaleZ.o" "Tile7.sz";
connectAttr "Tile8_translateY.o" "Tile8.ty";
connectAttr "Tile8_translateX.o" "Tile8.tx";
connectAttr "Tile8_translateZ.o" "Tile8.tz";
connectAttr "Tile8_rotateX.o" "Tile8.rx";
connectAttr "Tile8_rotateY.o" "Tile8.ry";
connectAttr "Tile8_rotateZ.o" "Tile8.rz";
connectAttr "Tile9_translateY.o" "Tile9.ty";
connectAttr "Tile9_translateX.o" "Tile9.tx";
connectAttr "Tile9_translateZ.o" "Tile9.tz";
connectAttr "Tile9_rotateX.o" "Tile9.rx";
connectAttr "Tile9_rotateY.o" "Tile9.ry";
connectAttr "Tile9_rotateZ.o" "Tile9.rz";
connectAttr "groupId81.id" "oz_ec_narrows_straight_over_anim_aShape.iog.og[0].gid"
		;
connectAttr "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.mwc" "oz_ec_narrows_straight_over_anim_aShape.iog.og[0].gco"
		;
connectAttr "oz_ec_narrows_straight_over_anim_a1_translateX.o" "Tile10.tx";
connectAttr "oz_ec_narrows_straight_over_anim_a1_translateY.o" "Tile10.ty";
connectAttr "oz_ec_narrows_straight_over_anim_a1_translateZ.o" "Tile10.tz";
connectAttr "oz_ec_narrows_straight_over_anim_a1_rotateX.o" "Tile10.rx";
connectAttr "oz_ec_narrows_straight_over_anim_a1_rotateY.o" "Tile10.ry";
connectAttr "oz_ec_narrows_straight_over_anim_a1_rotateZ.o" "Tile10.rz";
connectAttr "groupId84.id" "Tile10Shape.iog.og[0].gid";
connectAttr "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.mwc" "Tile10Shape.iog.og[0].gco"
		;
connectAttr "polySurface1_translateX.o" "Tile11.tx";
connectAttr "polySurface1_translateY.o" "Tile11.ty";
connectAttr "polySurface1_translateZ.o" "Tile11.tz";
connectAttr "polySurface1_rotateX.o" "Tile11.rx";
connectAttr "polySurface1_rotateY.o" "Tile11.ry";
connectAttr "polySurface1_rotateZ.o" "Tile11.rz";
connectAttr "groupId83.id" "Tile11Shape.iog.og[1].gid";
connectAttr "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.mwc" "Tile11Shape.iog.og[1].gco"
		;
connectAttr "Tile12_translateX.o" "Tile12.tx";
connectAttr "Tile12_translateY.o" "Tile12.ty";
connectAttr "Tile12_translateZ.o" "Tile12.tz";
connectAttr "Tile12_rotateX.o" "Tile12.rx";
connectAttr "Tile12_rotateY.o" "Tile12.ry";
connectAttr "Tile12_rotateZ.o" "Tile12.rz";
connectAttr "groupId85.id" "Tile12Shape.iog.og[0].gid";
connectAttr "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.mwc" "Tile12Shape.iog.og[0].gco"
		;
relationship "link" ":lightLinker1" ":initialShadingGroup.message" ":defaultLightSet.message";
relationship "link" ":lightLinker1" ":initialParticleSE.message" ":defaultLightSet.message";
relationship "link" ":lightLinker1" "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" ":initialShadingGroup.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" ":initialParticleSE.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.message" ":defaultLightSet.message";
connectAttr "layerManager.dli[0]" "defaultLayer.id";
connectAttr "renderLayerManager.rlmi[0]" "defaultRenderLayer.rlid";
connectAttr ":mentalrayGlobals.msg" ":mentalrayItemsList.glb";
connectAttr ":miDefaultOptions.msg" ":mentalrayItemsList.opt" -na;
connectAttr ":miDefaultFramebuffer.msg" ":mentalrayItemsList.fb" -na;
connectAttr ":miDefaultOptions.msg" ":mentalrayGlobals.opt";
connectAttr ":miDefaultFramebuffer.msg" ":mentalrayGlobals.fb";
connectAttr "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.msg" "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_materialInfo1.sg"
		;
connectAttr "oz_ec_master_opaque.msg" "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_materialInfo1.m"
		;
connectAttr "file1.msg" "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_materialInfo1.t"
		 -na;
connectAttr "oz_ec_master_opaque.oc" "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.ss"
		;
connectAttr "fireworkShape.iog" "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.dsm"
		 -na;
connectAttr "Tile2Shape.iog" "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.dsm"
		 -na;
connectAttr "Tile1Shape.iog" "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.dsm"
		 -na;
connectAttr "Tile3Shape.iog" "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.dsm"
		 -na;
connectAttr "Tile4Shape.iog" "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.dsm"
		 -na;
connectAttr "Tile5Shape.iog" "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.dsm"
		 -na;
connectAttr "Tile6Shape.iog" "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.dsm"
		 -na;
connectAttr "Tile7Shape.iog" "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.dsm"
		 -na;
connectAttr "Tile8Shape.iog" "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.dsm"
		 -na;
connectAttr "Tile9Shape.iog" "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.dsm"
		 -na;
connectAttr "oz_ec_narrows_straight_over_anim_aShape.iog.og[0]" "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.dsm"
		 -na;
connectAttr "Tile11Shape.iog.og[1]" "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.dsm"
		 -na;
connectAttr "Tile10Shape.iog.og[0]" "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.dsm"
		 -na;
connectAttr "Tile12Shape.iog.og[0]" "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.dsm"
		 -na;
connectAttr "groupId81.msg" "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.gn"
		 -na;
connectAttr "groupId83.msg" "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.gn"
		 -na;
connectAttr "groupId84.msg" "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.gn"
		 -na;
connectAttr "groupId85.msg" "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.gn"
		 -na;
connectAttr "file1.oc" "oz_ec_master_opaque.c";
connectAttr "file1.ot" "oz_ec_master_opaque.it";
connectAttr "troz_texture_bake_set.pa" "textureBakePartition.st" -na;
connectAttr "motionPath1_uValue.o" "motionPath1.u";
connectAttr "curveShape1.ws" "motionPath1.gp";
connectAttr "positionMarkerShape1.t" "motionPath1.pmt[0]";
connectAttr "positionMarkerShape2.t" "motionPath1.pmt[1]";
connectAttr "oz_ec_narrows_straight_a:oz_ec_narrows_straight_a3:roots_01_lambert2SG.pa" ":renderPartition.st"
		 -na;
connectAttr "oz_ec_master_opaque.msg" ":defaultShaderList1.s" -na;
connectAttr "file1.msg" ":defaultTextureList1.tx" -na;
connectAttr "pointLightShape1.ltd" ":lightList1.l" -na;
connectAttr "pointLightShape2.ltd" ":lightList1.l" -na;
connectAttr "defaultRenderLayer.msg" ":defaultRenderingList1.r" -na;
connectAttr ":perspShape.msg" ":defaultRenderGlobals.sc";
connectAttr "pointLight1.iog" ":defaultLightSet.dsm" -na;
connectAttr "pointLight2.iog" ":defaultLightSet.dsm" -na;
// End of oz_ec_narrows_straight_over_anim_a.ma
