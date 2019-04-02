//Maya ASCII 2012 scene
//Name: oz_ec_catacombs_straight_over_anim_a.ma
//Last modified: Wed, Jun 26, 2013 04:20:16 PM
//Codeset: 1252
requires maya "2012";
requires "AutodeskPacketFile" "1.01";
requires "Mayatomr" "2012.0m - 3.9.1.36 ";
requires "stereoCamera" "10.0";
currentUnit -l meter -a degree -t ntsc;
fileInfo "application" "maya";
fileInfo "product" "Maya 2012";
fileInfo "version" "2012 x64";
fileInfo "cutIdentifier" "001200000000-796618";
fileInfo "osv" "Microsoft Windows 7 Enterprise Edition, 64-bit Windows 7 Service Pack 1 (Build 7601)\n";
createNode transform -n "path_default";
createNode transform -n "dummy_a" -p "path_default";
	setAttr ".t" -type "double3" 0 -8.0779356694631611e-030 0 ;
	setAttr ".r" -type "double3" -89.999999999999986 0 0 ;
	setAttr ".rp" -type "double3" 0 -1.1230151078670202e-014 5.6843418860800985e-016 ;
	setAttr ".rpt" -type "double3" 0 1.123015107867021e-014 -5.6843418860800946e-016 ;
	setAttr ".sp" -type "double3" 0 -1.1230151078670202e-014 5.6843418860800985e-016 ;
createNode transform -n "dummy_b" -p "path_default";
	setAttr ".t" -type "double3" 3.8146972656250001e-008 0 -19.94797119140625 ;
	setAttr ".r" -type "double3" -89.999999999999986 0 0 ;
createNode transform -s -n "persp";
	setAttr ".t" -type "double3" -0.043037317265975567 11.167928819108006 -1.3398515853917503 ;
	setAttr ".r" -type "double3" -47.738352763611857 -3.4000000004701669 -1.9913517977440158e-016 ;
createNode camera -s -n "perspShape" -p "persp";
	setAttr -k off ".v";
	setAttr ".pze" yes;
	setAttr ".fl" 34.999999999999986;
	setAttr ".ncp" 0.01;
	setAttr ".fcp" 1000;
	setAttr ".fd" 0.05;
	setAttr ".coi" 11.862391852524663;
	setAttr ".ow" 0.1;
	setAttr ".imn" -type "string" "persp";
	setAttr ".den" -type "string" "persp_depth";
	setAttr ".man" -type "string" "persp_mask";
	setAttr ".tp" -type "double3" 0 -5.6843418860800745e-014 -1.1230151078670204e-012 ;
	setAttr ".hc" -type "string" "viewSet -p %camera";
createNode transform -s -n "top";
	setAttr ".t" -type "double3" -1.1152393753917387 41.880916992187501 -6.8773094815823992 ;
	setAttr ".r" -type "double3" -89.999999999999986 0 0 ;
	setAttr ".s" -type "double3" 1 1.0000000000000004 1.0000000000000004 ;
createNode camera -s -n "topShape" -p "top";
	setAttr -k off ".v";
	setAttr ".rnd" no;
	setAttr ".ncp" 0.01;
	setAttr ".fcp" 1000;
	setAttr ".fd" 0.05;
	setAttr ".coi" 41.880916992187501;
	setAttr ".ow" 54.357310072697651;
	setAttr ".imn" -type "string" "top";
	setAttr ".den" -type "string" "top_depth";
	setAttr ".man" -type "string" "top_mask";
	setAttr ".hc" -type "string" "viewSet -t %camera";
	setAttr ".o" yes;
createNode transform -s -n "front";
	setAttr ".t" -type "double3" 6.3315615085895116 6.4527397192802187 41.880916992187501 ;
createNode camera -s -n "frontShape" -p "front";
	setAttr -k off ".v";
	setAttr ".rnd" no;
	setAttr ".ncp" 0.01;
	setAttr ".fcp" 1000;
	setAttr ".fd" 0.05;
	setAttr ".coi" 41.880916992187501;
	setAttr ".ow" 42.927381137183424;
	setAttr ".imn" -type "string" "front";
	setAttr ".den" -type "string" "front_depth";
	setAttr ".man" -type "string" "front_mask";
	setAttr ".hc" -type "string" "viewSet -f %camera";
	setAttr ".o" yes;
createNode transform -s -n "side";
	setAttr ".t" -type "double3" 43.032107095122882 -1.3482248347853014 -9.7281739041491821 ;
	setAttr ".r" -type "double3" 0 89.999999999999986 0 ;
	setAttr ".s" -type "double3" 1.0000000000000004 1 1.0000000000000004 ;
createNode camera -s -n "sideShape" -p "side";
	setAttr -k off ".v";
	setAttr ".rnd" no;
	setAttr ".ncp" 0.01;
	setAttr ".fcp" 1000;
	setAttr ".fd" 0.05;
	setAttr ".coi" 41.880916992187501;
	setAttr ".ow" 22.131312152040302;
	setAttr ".imn" -type "string" "side";
	setAttr ".den" -type "string" "side_depth";
	setAttr ".man" -type "string" "side_mask";
	setAttr ".hc" -type "string" "viewSet -s %camera";
	setAttr ".o" yes;
createNode transform -n "BridgePiece2";
	setAttr ".rp" -type "double3" -1.7572552490234372 0.015689353942871093 -6.3586090087890623 ;
	setAttr ".sp" -type "double3" -1.7572552490234372 0.015689353942871093 -6.3586090087890623 ;
createNode mesh -n "BridgePieceShape2" -p "BridgePiece2";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.50804638862609863 0.50502985715866089 ;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 19 ".uvst[0].uvsp[0:18]" -type "float2" 0.0011605734 0.39497763
		 0.12980653 0.39497763 0.12980653 0.49957269 0.0011605732 0.49957269 0.12980653 0.37514409
		 0.0011605737 0.25071549 0.12980653 0.25071549 0.15734079 0.34533381 0.013817549 0.34533376
		 0.013817549 0.29956979 0.15734068 0.29956979 0.049005814 0.25000122 0.049005747 0.36982638
		 9.9182129e-005 0.36982626 9.9211931e-005 0.25000131 0.049005814 0.25000122 0.049005747
		 0.36982638 9.9182129e-005 0.36982626 9.9211931e-005 0.25000131;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 17 ".uvst[1].uvsp[0:16]" -type "float2" 0.076664582 0.68961841
		 0.070001751 0.68973732 0.069883361 0.68447226 0.076617837 0.68433803 0.069686346
		 0.6756689 0.026159734 0.7485956 0.026159741 0.74549705 0.029457338 0.74549711 0.029457331
		 0.7485956 0.035304829 0.74847919 0.032276988 0.74847919 0.032276988 0.74563414 0.035304829
		 0.74563408 0.062157348 0.74471027 0.065959617 0.74471027 0.065959617 0.74828297 0.062157348
		 0.74828297;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 4 ".pt[5:8]" -type "float3"  0 -0.67423278 0 0 -0.67423278 
		0 0 -0.67423278 0 0 -0.67423278 0;
	setAttr -s 9 ".vt[0:8]"  -2.84461665 0.0015360245 -3.30782032 3.2237756e-016 0.001536028 -3.30782032
		 2.8711945e-016 0.002650033 -5.70610428 -1.0070804e-016 0.0044063334 -9.48805332 -2.84731865 0.0026500269 -5.70610428
		 -2.84461665 0.0015360245 -3.30782032 3.2237756e-016 0.001536028 -3.30782032 2.8711945e-016 0.002650033 -5.70610428
		 -1.0070804e-016 0.0044063334 -9.48805332;
	setAttr -s 13 ".ed[0:12]"  0 4 0 0 1 0 4 2 1 2 1 0 4 3 0 2 3 0 0 5 0
		 1 6 0 5 6 0 2 7 0 7 6 0 3 8 0 7 8 0;
	setAttr -s 5 ".fc[0:4]" -type "polyFaces" 
		f 4 1 -4 -3 -1
		mu 0 4 0 1 2 3
		mu 1 4 0 1 2 3
		f 3 -5 2 5
		mu 0 3 4 5 6
		mu 1 3 4 3 2
		f 4 -2 6 8 -8
		mu 0 4 7 8 9 10
		mu 1 4 5 6 7 8
		f 4 3 7 -11 -10
		mu 0 4 11 12 13 14
		mu 1 4 9 10 11 12
		f 4 -6 9 12 -12
		mu 0 4 15 16 17 18
		mu 1 4 13 14 15 16;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "SideBlock12" -p "BridgePiece2";
	setAttr ".rp" -type "double3" -3.1230328369140627 0.015689353942871093 -4.3045480810033245 ;
	setAttr ".sp" -type "double3" -3.1230328369140627 0.015689353942871093 -4.3045480810033245 ;
createNode mesh -n "SideBlockShape12" -p "SideBlock12";
	setAttr -k off ".v";
	setAttr ".iog[0].og[0].gcl" -type "componentList" 3 "f[0:2]" "f[1]" "f[2]";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 8 ".uvst[0].uvsp[0:7]" -type "float2" 0.060852632 0.17146137
		 0.2108359 0.17146137 0.21108857 0.24945144 0.061205044 0.24967495 0.2108359 0.17146137
		 0.060852632 0.17146137 0.060852632 0.17146137 0.061205044 0.24967495;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 8 ".uvst[1].uvsp[0:7]" -type "float2" 0.085920751 0.73488188
		 0.088442549 0.73488188 0.088442549 0.73593813 0.085920751 0.73593813 0.088442549
		 0.73937947 0.085920751 0.73937947 0.089717522 0.73593813 0.089717522 0.73937947;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 7 ".pt[0:6]" -type "float3"  0 0 8.9175911 0 0 8.9175911 
		0 0 8.9175911 0 0 8.9175911 0 0 8.9175911 0 0 8.9175911 0 0 8.9175911;
	setAttr -s 7 ".vt[0:6]"  -3.51451039 -0.26366088 -12.14675617 -2.73155522 -0.26366088 -12.14675617
		 -3.47764158 0.29503959 -12.24803543 -2.76842523 0.29503959 -12.24803543 -3.47764158 0.29503959 -14.19623947
		 -2.76842523 0.29503959 -14.19623947 -2.73155522 -0.26366088 -14.2975235;
	setAttr -s 9 ".ed[0:8]"  0 1 0 2 3 0 4 5 0 0 2 0 1 3 0 2 4 0 3 5 0
		 5 6 0 6 1 0;
	setAttr -s 3 ".fc[0:2]" -type "polyFaces" 
		f 4 0 4 -2 -4
		mu 0 4 0 1 2 3
		mu 1 4 0 1 2 3
		f 4 1 6 -3 -6
		mu 0 4 3 2 4 5
		mu 1 4 3 2 4 5
		f 4 -9 -8 -7 -5
		mu 0 4 1 6 7 2
		mu 1 4 6 7 4 2;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "BridgePiece1";
	setAttr ".rp" -type "double3" 1.7522106933593751 0.015689353942871093 -6.395331115722656 ;
	setAttr ".sp" -type "double3" 1.7522106933593751 0.015689353942871093 -6.395331115722656 ;
createNode mesh -n "BridgePieceShape1" -p "BridgePiece1";
	addAttr -ci true -sn "mso" -ln "miShadingSamplesOverride" -min 0 -max 1 -at "bool";
	addAttr -ci true -sn "msh" -ln "miShadingSamples" -min 0 -smx 8 -at "float";
	addAttr -ci true -sn "mdo" -ln "miMaxDisplaceOverride" -min 0 -max 1 -at "bool";
	addAttr -ci true -sn "mmd" -ln "miMaxDisplace" -min 0 -smx 1 -at "float";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.08669637143611908 0.31322908401489258 ;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 19 ".uvst[0].uvsp[0:18]" -type "float2" 0.24861985 0.39497763
		 0.24861985 0.49957269 0.12980653 0.49957269 0.12980653 0.39497763 0.24861985 0.25071549
		 0.12980653 0.37514409 0.12980653 0.25071549 0.14964208 0.32351196 0.026505768 0.32351196
		 0.026505649 0.28090626 0.14964204 0.28090626 0.1079992 0.25166088 0.10799921 0.37479725
		 0.065393567 0.37479734 0.065393552 0.25166094 0.1079992 0.25166088 0.10799921 0.37479725
		 0.065393567 0.37479734 0.065393552 0.25166094;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 17 ".uvst[1].uvsp[0:16]" -type "float2" 0.062103614 0.68228149
		 0.061922073 0.67700487 0.068638146 0.67686284 0.068756551 0.68212926 0.068441086
		 0.66805732 0.11674258 0.74832577 0.11674258 0.74523985 0.12002682 0.74523985 0.12002682
		 0.74832577 0.036276709 0.74568218 0.039294496 0.74568218 0.039294496 0.74851775 0.036276709
		 0.74851775 0.06709826 0.74494374 0.070887893 0.74494374 0.070887893 0.74850458 0.06709826
		 0.74850458;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 4 ".pt[5:8]" -type "float3"  0 -0.66975546 0 0 -0.66975546 
		0 0 -0.66975546 0 0 -0.66975546 0;
	setAttr -s 9 ".vt[0:8]"  -1.3419848e-017 0.002650033 -5.70610428 -1.4549674e-016 0.001536028 -3.30782032
		 2.84050679 0.001536028 -3.30782032 2.84022951 0.002650033 -5.70610428 -1.0078531e-016 0.0044063334 -9.48805332
		 -1.4549674e-016 0.001536028 -3.30782032 2.84050679 0.001536028 -3.30782032 -1.3419848e-017 0.002650033 -5.70610428
		 -1.0078531e-016 0.0044063334 -9.48805332;
	setAttr -s 13 ".ed[0:12]"  2 3 0 1 2 0 0 1 0 0 3 1 0 4 0 3 4 0 1 5 0
		 2 6 0 5 6 0 0 7 0 7 5 0 4 8 0 7 8 0;
	setAttr -s 5 ".fc[0:4]" -type "polyFaces" 
		f 4 0 -4 2 1
		mu 0 4 0 1 2 3
		mu 1 4 0 1 2 3
		f 3 5 -5 3
		mu 0 3 4 5 6
		mu 1 3 1 4 2
		f 4 -2 6 8 -8
		mu 0 4 7 8 9 10
		mu 1 4 5 6 7 8
		f 4 -3 9 10 -7
		mu 0 4 11 12 13 14
		mu 1 4 9 10 11 12
		f 4 4 11 -13 -10
		mu 0 4 15 16 17 18
		mu 1 4 13 14 15 16;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "SideBlock1" -p "BridgePiece1";
	setAttr ".rp" -type "double3" 3.1129437255859376 0.015689353942871093 -4.3779927509146912 ;
	setAttr ".sp" -type "double3" 3.1129437255859376 0.015689353942871093 -4.3779927509146912 ;
createNode mesh -n "SideBlockShape1" -p "SideBlock1";
	setAttr -k off ".v";
	setAttr ".iog[0].og[0].gcl" -type "componentList" 3 "f[0:2]" "f[1]" "f[2]";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 8 ".uvst[0].uvsp[0:7]" -type "float2" 0.060852632 0.17146137
		 0.2108359 0.17146137 0.21108857 0.24945144 0.061205044 0.24967495 0.2108359 0.17146137
		 0.060852632 0.17146137 0.2108359 0.17146137 0.21108857 0.24945144;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 8 ".uvst[1].uvsp[0:7]" -type "float2" 0.1003715 0.73764336
		 0.10289331 0.73764336 0.10289331 0.73869967 0.1003715 0.73869967 0.10289331 0.74214095
		 0.1003715 0.74214095 0.099096507 0.74214095 0.099096507 0.73869967;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 7 ".pt[0:6]" -type "float3"  0 0 8.8441477 0 0 8.8441477 
		0 0 8.8441477 0 0 8.8441477 0 0 8.8441477 0 0 8.8441477 0 0 8.8441477;
	setAttr -s 7 ".vt[0:6]"  2.72146606 -0.26366088 -12.14675617 3.50442147 -0.26366088 -12.14675617
		 2.75833607 0.29503959 -12.24803543 3.46755123 0.29503959 -12.24803543 2.75833607 0.29503959 -14.19623947
		 3.46755123 0.29503959 -14.19623947 2.72146606 -0.26366088 -14.2975235;
	setAttr -s 9 ".ed[0:8]"  0 1 0 2 3 0 4 5 0 0 2 0 1 3 0 2 4 0 3 5 0
		 4 6 0 6 0 0;
	setAttr -s 3 ".fc[0:2]" -type "polyFaces" 
		f 4 0 4 -2 -4
		mu 0 4 0 1 2 3
		mu 1 4 0 1 2 3
		f 4 1 6 -3 -6
		mu 0 4 3 2 4 5
		mu 1 4 3 2 4 5
		f 4 8 3 5 7
		mu 0 4 6 0 3 7
		mu 1 4 6 7 3 5;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "BridgePiece4";
	setAttr ".rp" -type "double3" -1.7572552490234374 0.015689353942871093 -13.006115112304688 ;
	setAttr ".sp" -type "double3" -1.7572552490234374 0.015689353942871093 -13.006115112304688 ;
createNode mesh -n "BridgePieceShape4" -p "BridgePiece4";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.073747258833110188 0.73487350344657898 ;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 20 ".uvst[0].uvsp[0:19]" -type "float2" 0.12980653 0.36880478
		 0.0011605734 0.36880478 0.0011605737 0.25071549 0.12980653 0.25071549 0.12980653
		 0.49957269 0.0011605732 0.49957269 0.0011605736 0.49957266 0.12980653 0.37514409
		 0.0050580651 0.29936963 0.16312188 0.29936963 0.16312188 0.34589839 0.0050580651
		 0.34589839 0.1085305 0.31995302 0.10853058 0.43705007 0.062001824 0.4370501 0.062001783
		 0.31995296 0.10911858 0.29722905 0.10911864 0.43272239 0.062589884 0.43272245 0.062589861
		 0.29722905;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 20 ".uvst[1].uvsp[0:19]" -type "float2" 0.080364883 0.7313177
		 0.080500767 0.7382313 0.073804773 0.73828733 0.073689744 0.73145968 0.096743688 0.72461575
		 0.10329331 0.72461575 0.10329331 0.72461575 0.096938863 0.73333693 0.085450187 0.74486983
		 0.085450172 0.74086022 0.089717403 0.74086022 0.089717396 0.74486983 0.073450848
		 0.74506742 0.076990232 0.74506742 0.076990232 0.74839306 0.073450848 0.74839306 0.097449526
		 0.74327189 0.10126515 0.74327189 0.10126515 0.74685717 0.097449526 0.74685717;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 4 ".pt[6:9]" -type "float3"  0 -0.67897874 0 0 -0.67897874 
		0 0 -0.67897874 0 0 -0.67897874 0;
	setAttr -s 10 ".vt[0:9]"  -2.84088993 0.0061626341 -13.27000237 -2.84405422 0.020455413 -16.52417755
		 -2.84088993 0.0061626332 -13.27000141 -4.2233677e-016 0.0061626341 -13.27000237 -9.395101e-016 0.020455418 -16.52417755
		 -1.0077986e-016 0.0044063334 -9.48805332 -1.0077986e-016 0.0044063334 -9.48805332
		 -2.84088993 0.0061626332 -13.27000141 -4.2233677e-016 0.0061626341 -13.27000237 -9.395101e-016 0.020455418 -16.52417755;
	setAttr -s 14 ".ed[0:13]"  0 1 0 1 4 0 0 2 0 3 0 0 5 2 0 3 4 0 3 5 0
		 5 6 0 2 7 0 6 7 0 3 8 0 4 9 0 8 9 0 8 6 0;
	setAttr -s 5 ".fc[0:4]" -type "polyFaces" 
		f 4 -2 -1 -4 5
		mu 0 4 0 1 2 3
		mu 1 4 0 1 2 3
		f 4 3 2 -5 -7
		mu 0 4 4 5 6 7
		mu 1 4 4 5 6 7
		f 4 4 8 -10 -8
		mu 0 4 8 9 10 11
		mu 1 4 8 9 10 11
		f 4 -6 10 12 -12
		mu 0 4 12 13 14 15
		mu 1 4 12 13 14 15
		f 4 6 7 -14 -11
		mu 0 4 16 17 18 19
		mu 1 4 16 17 18 19;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "SideBlock7" -p "BridgePiece4";
	setAttr ".rp" -type "double3" -3.120372314453125 0.015689353942871093 -15.423440551757812 ;
	setAttr ".sp" -type "double3" -3.120372314453125 0.015689353942871093 -15.423440551757812 ;
createNode mesh -n "SideBlockShape7" -p "SideBlock7";
	setAttr -k off ".v";
	setAttr ".iog[0].og[0].gcl" -type "componentList" 3 "f[0:2]" "f[1]" "f[2]";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 8 ".uvst[0].uvsp[0:7]" -type "float2" 0.08839187 0.0012279456
		 0.17260642 0.0012279456 0.17172465 0.091203481 0.08839187 0.091541991 0.17260642
		 0.0012279456 0.08839187 0.0012279456 0.08839187 0.0012279456 0.08839187 0.091541991;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 8 ".uvst[1].uvsp[0:7]" -type "float2" 0.1040374 0.7397393
		 0.10652912 0.7397393 0.10652912 0.74078304 0.1040374 0.74078304 0.10652912 0.74418324
		 0.1040374 0.74418324 0.10778895 0.74078304 0.10778895 0.74418324;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 7 ".vt[0:6]"  -3.5118506 -0.26366088 -14.34806061 -2.728894 -0.26366088 -14.34806061
		 -3.46744132 0.29503959 -14.47004318 -2.77330327 0.29503959 -14.47004318 -3.46744132 0.29503959 -16.37683487
		 -2.77330327 0.29503959 -16.37683487 -2.728894 -0.26366088 -16.49882126;
	setAttr -s 9 ".ed[0:8]"  0 1 0 2 3 0 4 5 0 0 2 0 1 3 0 2 4 0 3 5 0
		 5 6 0 6 1 0;
	setAttr -s 3 ".fc[0:2]" -type "polyFaces" 
		f 4 0 4 -2 -4
		mu 0 4 0 1 2 3
		mu 1 4 0 1 2 3
		f 4 1 6 -3 -6
		mu 0 4 3 2 4 5
		mu 1 4 3 2 4 5
		f 4 -9 -8 -7 -5
		mu 0 4 1 6 7 2
		mu 1 4 6 7 4 2;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "SideBlock8" -p "BridgePiece4";
	setAttr ".rp" -type "double3" -3.1230328369140627 0.015689353942871093 -13.222139892578125 ;
	setAttr ".sp" -type "double3" -3.1230328369140627 0.015689353942871093 -13.222139892578125 ;
createNode mesh -n "SideBlockShape8" -p "SideBlock8";
	setAttr -k off ".v";
	setAttr ".iog[0].og[0].gcl" -type "componentList" 3 "f[0:2]" "f[1]" "f[2]";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 8 ".uvst[0].uvsp[0:7]" -type "float2" 0.060852632 0.17146137
		 0.2108359 0.17146137 0.21108857 0.24945144 0.061205044 0.24967495 0.2108359 0.17146137
		 0.060852632 0.17146137 0.060852632 0.17146137 0.061205044 0.24967495;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 8 ".uvst[1].uvsp[0:7]" -type "float2" 0.05392256 0.73547548
		 0.056444328 0.73547548 0.056444328 0.73653179 0.05392256 0.73653179 0.056444328 0.73997313
		 0.05392256 0.73997313 0.05771932 0.73653179 0.05771932 0.73997313;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 7 ".vt[0:6]"  -3.51451039 -0.26366088 -12.14675617 -2.73155522 -0.26366088 -12.14675617
		 -3.47764158 0.29503959 -12.24803543 -2.76842523 0.29503959 -12.24803543 -3.47764158 0.29503959 -14.19623947
		 -2.76842523 0.29503959 -14.19623947 -2.73155522 -0.26366088 -14.2975235;
	setAttr -s 9 ".ed[0:8]"  0 1 0 2 3 0 4 5 0 0 2 0 1 3 0 2 4 0 3 5 0
		 5 6 0 6 1 0;
	setAttr -s 3 ".fc[0:2]" -type "polyFaces" 
		f 4 0 4 -2 -4
		mu 0 4 0 1 2 3
		mu 1 4 0 1 2 3
		f 4 1 6 -3 -6
		mu 0 4 3 2 4 5
		mu 1 4 3 2 4 5
		f 4 -9 -8 -7 -5
		mu 0 4 1 6 7 2
		mu 1 4 6 7 4 2;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "BridgePiece5";
	setAttr ".rp" -type "double3" 1.7535412597656248 0.015689353942871093 -13.006115112304688 ;
	setAttr ".sp" -type "double3" 1.7535412597656248 0.015689353942871093 -13.006115112304688 ;
createNode mesh -n "BridgePieceShape5" -p "BridgePiece5";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.18921319395303726 0.36825734520532993 ;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 15 ".uvst[0].uvsp[0:14]" -type "float2" 0.24861985 0.25071549
		 0.24861985 0.36825734 0.12980653 0.36825734 0.12980653 0.25071549 0.12980653 0.37514409
		 0.24861985 0.49957269 0.12980653 0.49957269 0.12131906 0.30066821 0.12131906 0.41292366
		 0.074549437 0.41292387 0.074549399 0.3006683 0.12131906 0.30066821 0.12131906 0.41292366
		 0.074549437 0.41292387 0.074549399 0.3006683;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 15 ".uvst[1].uvsp[0:14]" -type "float2" 0.056510624 0.72780079
		 0.063674346 0.72758317 0.063856401 0.73448724 0.056706548 0.73462927 0.11101875 0.73366404
		 0.10427269 0.72494435 0.11082359 0.72494435 0.10544908 0.74860388 0.10544908 0.74528843
		 0.10897759 0.74528843 0.10897759 0.74860388 0.078862324 0.74750972 0.078862309 0.74393553
		 0.082666203 0.74393553 0.082666211 0.74750972;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 3 ".pt[5:7]" -type "float3"  0 -0.67481232 0 0 -0.67481232 
		0 0 -0.67481232 0;
	setAttr -s 8 ".vt[0:7]"  -1.1709651e-016 0.0061626341 -13.27000237
		 2.84088993 0.0061626341 -13.27000237 -4.2596927e-016 0.020455418 -16.52417755 2.84056497 0.020455418 -16.52417755
		 4.132323e-017 0.0044063334 -9.48805332 -1.1709651e-016 0.0061626341 -13.27000237
		 -4.2596927e-016 0.020455418 -16.52417755 4.132323e-017 0.0044063334 -9.48805332;
	setAttr -s 11 ".ed[0:10]"  1 3 0 2 3 0 0 2 0 0 1 0 1 4 0 0 4 0 0 5 0
		 2 6 0 5 6 0 4 7 0 5 7 0;
	setAttr -s 4 ".fc[0:3]" -type "polyFaces" 
		f 4 0 -2 -3 3
		mu 0 4 0 1 2 3
		mu 1 4 0 1 2 3
		f 3 -5 -4 5
		mu 0 3 4 5 6
		mu 1 3 4 5 6
		f 4 2 7 -9 -7
		mu 0 4 7 8 9 10
		mu 1 4 7 8 9 10
		f 4 -6 6 10 -10
		mu 0 4 11 12 13 14
		mu 1 4 11 12 13 14;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "SideBlock5" -p "BridgePiece5";
	setAttr ".rp" -type "double3" 3.1129437255859376 0.015689353942871093 -13.222139892578125 ;
	setAttr ".sp" -type "double3" 3.1129437255859376 0.015689353942871093 -13.222139892578125 ;
createNode mesh -n "SideBlockShape5" -p "SideBlock5";
	setAttr -k off ".v";
	setAttr ".iog[0].og[0].gcl" -type "componentList" 3 "f[0:2]" "f[1]" "f[2]";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 8 ".uvst[0].uvsp[0:7]" -type "float2" 0.060852632 0.17146137
		 0.2108359 0.17146137 0.21108857 0.24945144 0.061205044 0.24967495 0.2108359 0.17146137
		 0.060852632 0.17146137 0.2108359 0.17146137 0.21108857 0.24945144;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 8 ".uvst[1].uvsp[0:7]" -type "float2" 0.060373694 0.73553467
		 0.062895499 0.73553467 0.062895499 0.73659098 0.060373694 0.73659098 0.062895499
		 0.74003232 0.060373694 0.74003232 0.059098694 0.74003232 0.059098694 0.73659098;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 7 ".vt[0:6]"  2.72146606 -0.26366088 -12.14675617 3.50442147 -0.26366088 -12.14675617
		 2.75833607 0.29503959 -12.24803543 3.46755123 0.29503959 -12.24803543 2.75833607 0.29503959 -14.19623947
		 3.46755123 0.29503959 -14.19623947 2.72146606 -0.26366088 -14.2975235;
	setAttr -s 9 ".ed[0:8]"  0 1 0 2 3 0 4 5 0 0 2 0 1 3 0 2 4 0 3 5 0
		 4 6 0 6 0 0;
	setAttr -s 3 ".fc[0:2]" -type "polyFaces" 
		f 4 0 4 -2 -4
		mu 0 4 0 1 2 3
		mu 1 4 0 1 2 3
		f 4 1 6 -3 -6
		mu 0 4 3 2 4 5
		mu 1 4 3 2 4 5
		f 4 8 3 5 7
		mu 0 4 6 0 3 7
		mu 1 4 6 7 3 5;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "SideBlock6" -p "BridgePiece5";
	setAttr ".rp" -type "double3" 3.1156048583984375 0.015689353942871093 -15.423440551757812 ;
	setAttr ".sp" -type "double3" 3.1156048583984375 0.015689353942871093 -15.423440551757812 ;
createNode mesh -n "SideBlockShape6" -p "SideBlock6";
	setAttr -k off ".v";
	setAttr ".iog[0].og[0].gcl" -type "componentList" 3 "f[0:2]" "f[1]" "f[2]";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 8 ".uvst[0].uvsp[0:7]" -type "float2" 0.08839187 0.0012279456
		 0.17260642 0.0012279456 0.17172465 0.091203481 0.08839187 0.091541991 0.17260642
		 0.0012279456 0.08839187 0.0012279456 0.17260648 0.0012279456 0.17172471 0.091203481;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 8 ".uvst[1].uvsp[0:7]" -type "float2" 0.081769109 0.73838347
		 0.084260888 0.73838347 0.084260888 0.73942727 0.081769109 0.73942727 0.084260888
		 0.74282759 0.081769109 0.74282759 0.080509283 0.74282759 0.080509283 0.73942727;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 7 ".vt[0:6]"  2.72412729 -0.26366088 -14.34806061 3.50708246 -0.26366088 -14.34806061
		 2.76853633 0.29503959 -14.47004318 3.46267343 0.29503959 -14.47004318 2.76853633 0.29503959 -16.37684441
		 3.46267343 0.29503959 -16.37684441 2.72412729 -0.26366088 -16.49882126;
	setAttr -s 9 ".ed[0:8]"  0 1 0 2 3 0 4 5 0 0 2 0 1 3 0 2 4 0 3 5 0
		 4 6 0 6 0 0;
	setAttr -s 3 ".fc[0:2]" -type "polyFaces" 
		f 4 0 4 -2 -4
		mu 0 4 0 1 2 3
		mu 1 4 0 1 2 3
		f 4 1 6 -3 -6
		mu 0 4 3 2 4 5
		mu 1 4 3 2 4 5
		f 4 8 3 5 7
		mu 0 4 6 0 3 7
		mu 1 4 6 7 3 5;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "BridgePiece6";
	setAttr ".rp" -type "double3" 1.753541259765625 0.015689353942871093 -9.3869577026367192 ;
	setAttr ".sp" -type "double3" 1.753541259765625 0.015689353942871093 -9.3869577026367192 ;
createNode mesh -n "BridgePieceShape6" -p "BridgePiece6";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.4817972828168422 0.90069857239723206 ;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 8 ".uvst[0].uvsp[0:7]" -type "float2" 0.24861985 0.49957269
		 0.12980653 0.37514409 0.24861985 0.37514409 0.24861985 0.25071549 0.0082671791 0.36760604
		 0.24655634 0.36760604 0.24655634 0.41170645 0.0082671791 0.41170645;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 8 ".uvst[1].uvsp[0:7]" -type "float2" 0.11650729 0.62801909
		 0.1232034 0.63667417 0.11665152 0.63674396 0.11679575 0.64546889 0.036982544 0.74051535
		 0.041287117 0.74051535 0.041287117 0.74456 0.036982544 0.74456;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 6 ".vt[0:5]"  2.84022951 0.002650033 -5.70610428 2.84055972 0.0044063334 -9.48805332
		 2.84088993 0.0061626341 -13.27000237 1.7299642e-016 0.0044063334 -9.48805332 2.84022951 -0.68832719 -5.70642519
		 1.0397056e-011 -0.68657088 -9.48837471;
	setAttr -s 8 ".ed[0:7]"  0 1 0 1 2 0 1 3 1 0 3 0 2 3 0 0 4 0 3 5 0
		 4 5 0;
	setAttr -s 3 ".fc[0:2]" -type "polyFaces" 
		f 3 4 -3 1
		mu 0 3 0 1 2
		mu 1 3 0 1 2
		f 3 -4 0 2
		mu 0 3 1 3 2
		mu 1 3 1 3 2
		f 4 3 6 -8 -6
		mu 0 4 4 5 6 7
		mu 1 4 4 5 6 7;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "SideBlock4" -p "BridgePiece6";
	setAttr ".rp" -type "double3" 3.1156048583984375 0.015689353942871093 -10.997359901252844 ;
	setAttr ".sp" -type "double3" 3.1156048583984375 0.015689353942871093 -10.997359901252844 ;
createNode mesh -n "SideBlockShape4" -p "SideBlock4";
	setAttr -k off ".v";
	setAttr ".iog[0].og[0].gcl" -type "componentList" 3 "f[0:2]" "f[1]" "f[2]";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 8 ".uvst[0].uvsp[0:7]" -type "float2" 0.08839187 0.0012279456
		 0.17260642 0.0012279456 0.17172465 0.091203481 0.08839187 0.091541991 0.17260642
		 0.0012279456 0.08839187 0.0012279456 0.17260648 0.0012279456 0.17172471 0.091203481;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 8 ".uvst[1].uvsp[0:7]" -type "float2" 0.070240349 0.73939091
		 0.072732106 0.73939091 0.072732106 0.74043465 0.070240349 0.74043465 0.072732106
		 0.74383503 0.070240349 0.74383503 0.0689805 0.74383503 0.0689805 0.74043465;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 7 ".pt[0:6]" -type "float3"  0 0 4.4260807 0 0 4.4260807 
		0 0 4.4260807 0 0 4.4260807 0 0 4.4260807 0 0 4.4260807 0 0 4.4260807;
	setAttr -s 7 ".vt[0:6]"  2.72412729 -0.26366088 -14.34806061 3.50708246 -0.26366088 -14.34806061
		 2.76853633 0.29503959 -14.47004318 3.46267343 0.29503959 -14.47004318 2.76853633 0.29503959 -16.37684441
		 3.46267343 0.29503959 -16.37684441 2.72412729 -0.26366088 -16.49882126;
	setAttr -s 9 ".ed[0:8]"  0 1 0 2 3 0 4 5 0 0 2 0 1 3 0 2 4 0 3 5 0
		 4 6 0 6 0 0;
	setAttr -s 3 ".fc[0:2]" -type "polyFaces" 
		f 4 0 4 -2 -4
		mu 0 4 0 1 2 3
		mu 1 4 0 1 2 3
		f 4 1 6 -3 -6
		mu 0 4 3 2 4 5
		mu 1 4 3 2 4 5
		f 4 8 3 5 7
		mu 0 4 6 0 3 7
		mu 1 4 6 7 3 5;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "SideBlock3" -p "BridgePiece6";
	setAttr ".rp" -type "double3" 3.1129437255859376 0.015689353942871093 -8.7960592420731558 ;
	setAttr ".sp" -type "double3" 3.1129437255859376 0.015689353942871093 -8.7960592420731558 ;
createNode mesh -n "SideBlockShape3" -p "SideBlock3";
	setAttr -k off ".v";
	setAttr ".iog[0].og[0].gcl" -type "componentList" 3 "f[0:2]" "f[1]" "f[2]";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 8 ".uvst[0].uvsp[0:7]" -type "float2" 0.060852632 0.17146137
		 0.2108359 0.17146137 0.21108857 0.24945144 0.061205044 0.24967495 0.2108359 0.17146137
		 0.060852632 0.17146137 0.2108359 0.17146137 0.21108857 0.24945144;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 8 ".uvst[1].uvsp[0:7]" -type "float2" 0.016611882 0.73610091
		 0.019133691 0.73610091 0.019133691 0.73715723 0.016611882 0.73715723 0.019133691
		 0.7405985 0.016611882 0.7405985 0.015336875 0.7405985 0.015336875 0.73715723;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 7 ".pt[0:6]" -type "float3"  0 0 4.4260807 0 0 4.4260807 
		0 0 4.4260807 0 0 4.4260807 0 0 4.4260807 0 0 4.4260807 0 0 4.4260807;
	setAttr -s 7 ".vt[0:6]"  2.72146606 -0.26366088 -12.14675617 3.50442147 -0.26366088 -12.14675617
		 2.75833607 0.29503959 -12.24803543 3.46755123 0.29503959 -12.24803543 2.75833607 0.29503959 -14.19623947
		 3.46755123 0.29503959 -14.19623947 2.72146606 -0.26366088 -14.2975235;
	setAttr -s 9 ".ed[0:8]"  0 1 0 2 3 0 4 5 0 0 2 0 1 3 0 2 4 0 3 5 0
		 4 6 0 6 0 0;
	setAttr -s 3 ".fc[0:2]" -type "polyFaces" 
		f 4 0 4 -2 -4
		mu 0 4 0 1 2 3
		mu 1 4 0 1 2 3
		f 4 1 6 -3 -6
		mu 0 4 3 2 4 5
		mu 1 4 3 2 4 5
		f 4 8 3 5 7
		mu 0 4 6 0 3 7
		mu 1 4 6 7 3 5;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "SideBlock2" -p "BridgePiece6";
	setAttr ".rp" -type "double3" 3.1156048583984375 0.015689353942871093 -6.5792934100943787 ;
	setAttr ".sp" -type "double3" 3.1156048583984375 0.015689353942871093 -6.5792934100943787 ;
createNode mesh -n "SideBlockShape2" -p "SideBlock2";
	setAttr -k off ".v";
	setAttr ".iog[0].og[0].gcl" -type "componentList" 3 "f[0:2]" "f[1]" "f[2]";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 8 ".uvst[0].uvsp[0:7]" -type "float2" 0.08839187 0.0012279456
		 0.17260642 0.0012279456 0.17172465 0.091203481 0.08839187 0.091541991 0.17260642
		 0.0012279456 0.08839187 0.0012279456 0.17260648 0.0012279456 0.17172471 0.091203481;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 8 ".uvst[1].uvsp[0:7]" -type "float2" 0.065299444 0.73921967
		 0.067791209 0.73921967 0.067791209 0.74026346 0.065299444 0.74026346 0.067791209
		 0.74366379 0.065299444 0.74366379 0.064039603 0.74366379 0.064039603 0.74026346;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 7 ".pt[0:6]" -type "float3"  0 0 8.8441477 0 0 8.8441477 
		0 0 8.8441477 0 0 8.8441477 0 0 8.8441477 0 0 8.8441477 0 0 8.8441477;
	setAttr -s 7 ".vt[0:6]"  2.72412729 -0.26366088 -14.34806061 3.50708246 -0.26366088 -14.34806061
		 2.76853633 0.29503959 -14.47004318 3.46267343 0.29503959 -14.47004318 2.76853633 0.29503959 -16.37684441
		 3.46267343 0.29503959 -16.37684441 2.72412729 -0.26366088 -16.49882126;
	setAttr -s 9 ".ed[0:8]"  0 1 0 2 3 0 4 5 0 0 2 0 1 3 0 2 4 0 3 5 0
		 4 6 0 6 0 0;
	setAttr -s 3 ".fc[0:2]" -type "polyFaces" 
		f 4 0 4 -2 -4
		mu 0 4 0 1 2 3
		mu 1 4 0 1 2 3
		f 4 1 6 -3 -6
		mu 0 4 3 2 4 5
		mu 1 4 3 2 4 5
		f 4 8 3 5 7
		mu 0 4 6 0 3 7
		mu 1 4 6 7 3 5;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "Lantern";
	setAttr ".rp" -type "double3" -0.0056027984619140625 16.563703002929689 -10.425055236816407 ;
	setAttr ".sp" -type "double3" -0.0056027984619140625 16.563703002929689 -10.425055236816407 ;
createNode mesh -n "LanternShape" -p "Lantern";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.86800384081030568 0.8971432993893711 ;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 24 ".uvst[0].uvsp[0:23]" -type "float2" 0.4188323 0.067327455
		 0.44867444 0.067327455 0.44867444 0.089960247 0.4188323 0.089960247 0.44867444 0.11259304
		 0.4188323 0.11259304 0.4188323 0.089960247 0.44867444 0.089960247 0.44867444 0.067327455
		 0.4188323 0.067327455 0.48475021 0.067327455 0.44867444 0.067327455 0.44867444 0.089960247
		 0.48475021 0.089960247 0.44867444 0.11259304 0.4188323 0.11259304 0.44867444 0.11259304
		 0.48475021 0.11259304 0.48475021 0.11259304 0.48475021 0.089960247 0.48475021 0.067327455
		 0.4188323 0.067327455 0.4188323 0.089960247 0.4188323 0.11259304;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 21 ".uvst[1].uvsp[0:20]" -type "float2" 0.11697786 0.71901768
		 0.12024532 0.71779287 0.12024532 0.71901768 0.11697786 0.72269201 0.11697786 0.72146726
		 0.11697786 0.72024244 0.11697786 0.72249806 0.11697786 0.71656811 0.11697786 0.7153433
		 0.12024532 0.7153433 0.12024532 0.71656811 0.12219177 0.71779287 0.12219177 0.71901768
		 0.12219177 0.7153433 0.12219177 0.71656811 0.12219177 0.71411854 0.12024532 0.71411854
		 0.11697786 0.71411854 0.11697786 0.71289378 0.11697786 0.71269983 0.11697786 0.71779287;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 19 ".vt[0:18]"  -0.35018134 14.053742409 -10.080477715 0.33897561 14.053742409 -10.080477715
		 -0.37786826 19.073663712 -10.052790642 0.36666244 19.073663712 -10.052790642 0.36666244 19.073663712 -10.79732132
		 -0.35018134 14.053742409 -10.76963425 0.33897561 14.053742409 -10.76963425 1.46845293 17.19960976 -8.95099926
		 -1.47965848 17.19960976 -8.95099926 1.46845293 17.19960976 -11.89911175 -0.0056028939 19.073663712 -10.052790642
		 -0.0056028939 14.053742409 -10.76963425 -0.0056028939 14.053742409 -10.080477715
		 -0.0056028939 17.19960976 -8.95099926 0.36666244 19.073663712 -10.42505646 -0.35018134 14.053742409 -10.42505646
		 -0.0056028939 14.053742409 -10.42505646 0.33897561 14.053742409 -10.42505646 1.46845293 17.19960976 -10.42505646;
	setAttr -s 30 ".ed[0:29]"  0 12 0 2 10 0 5 11 0 0 8 0 1 7 0 3 14 0 4 9 0
		 5 15 0 6 17 0 7 3 0 8 2 0 7 13 1 9 6 0 9 18 1 10 3 0 11 6 0 12 1 0 11 16 1 13 8 1
		 12 13 1 13 10 1 14 4 0 15 0 0 16 12 1 15 16 1 17 1 0 16 17 1 18 7 1 17 18 1 18 14 1;
	setAttr -s 12 ".fc[0:11]" -type "polyFaces" 
		f 4 0 19 18 -4
		mu 0 4 0 1 2 3
		mu 1 4 0 20 1 2
		f 4 24 23 -1 -23
		mu 0 4 6 7 8 9
		mu 1 4 5 6 20 0
		f 4 -26 28 27 -5
		mu 0 4 10 11 12 13
		mu 1 4 7 8 9 10
		f 4 -19 20 -2 -11
		mu 0 4 3 2 14 15
		mu 1 4 2 1 11 12
		f 4 -28 29 -6 -10
		mu 0 4 13 12 16 17
		mu 1 4 10 9 13 14
		f 4 -24 26 25 -17
		mu 0 4 8 7 19 20
		mu 1 4 20 19 8 7
		f 4 -20 16 4 11
		mu 0 4 2 1 10 13
		mu 1 4 1 20 7 10
		f 4 -21 -12 9 -15
		mu 0 4 14 2 13 17
		mu 1 4 11 1 10 14
		f 4 2 17 -25 -8
		mu 0 4 5 4 7 6
		mu 1 4 4 3 6 5
		f 4 -27 -18 15 8
		mu 0 4 19 7 4 18
		mu 1 4 8 19 18 17
		f 4 -29 -9 -13 13
		mu 0 4 12 11 21 22
		mu 1 4 9 8 17 16
		f 4 -30 -14 -7 -22
		mu 0 4 16 12 22 23
		mu 1 4 13 9 16 15;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "BridgePiece3";
	setAttr ".rp" -type "double3" -1.7572552490234374 0.015689353942871093 -9.3502349853515625 ;
	setAttr ".sp" -type "double3" -1.7572552490234374 0.015689353942871093 -9.3502349853515625 ;
createNode mesh -n "BridgePieceShape3" -p "BridgePiece3";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.13640999648749638 0.32293528318405151 ;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 8 ".uvst[0].uvsp[0:7]" -type "float2" 0.0011605737 0.25071549
		 0.12980653 0.37514409 0.0011605734 0.37514409 0.0011605736 0.49957266 0.024757432
		 0.30059952 0.24806255 0.30059952 0.24806255 0.34527105 0.024757432 0.34527105;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 8 ".uvst[1].uvsp[0:7]" -type "float2" 0.08643581 0.665968
		 0.079568148 0.65737867 0.086179584 0.65731227 0.085923366 0.64865649 0.052275613
		 0.74108142 0.056499332 0.74108142 0.056499332 0.74505013 0.052275613 0.74505013;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 2 ".pt[4:5]" -type "float3"  0 -0.66467088 0 0 -0.66467088 
		0;
	setAttr -s 6 ".vt[0:5]"  -2.84731865 0.0026500269 -5.70610428 -2.84410429 0.0044063306 -9.48805332
		 1.8305839e-016 0.0044063334 -9.48805332 -2.84088993 0.0061626332 -13.27000141 -2.84731865 0.0026500269 -5.70610428
		 1.8305839e-016 0.0044063334 -9.48805332;
	setAttr -s 8 ".ed[0:7]"  0 1 0 3 1 0 0 2 0 2 1 1 2 3 0 0 4 0 2 5 0
		 4 5 0;
	setAttr -s 3 ".fc[0:2]" -type "polyFaces" 
		f 3 2 3 -1
		mu 0 3 0 1 2
		mu 1 3 0 1 2
		f 3 4 1 -4
		mu 0 3 1 3 2
		mu 1 3 1 3 2
		f 4 -3 5 7 -7
		mu 0 4 4 5 6 7
		mu 1 4 4 5 6 7;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "SideBlock9" -p "BridgePiece3";
	setAttr ".rp" -type "double3" -3.120372314453125 0.015689353942871093 -10.946639926840053 ;
	setAttr ".sp" -type "double3" -3.120372314453125 0.015689353942871093 -10.946639926840053 ;
createNode mesh -n "SideBlockShape9" -p "SideBlock9";
	setAttr -k off ".v";
	setAttr ".iog[0].og[0].gcl" -type "componentList" 3 "f[0:2]" "f[1]" "f[2]";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 8 ".uvst[0].uvsp[0:7]" -type "float2" 0.08839187 0.0012279456
		 0.17260642 0.0012279456 0.17172465 0.091203481 0.08839187 0.091541991 0.17260642
		 0.0012279456 0.08839187 0.0012279456 0.08839187 0.0012279456 0.08839187 0.091541991;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 8 ".uvst[1].uvsp[0:7]" -type "float2" 0.047099493 0.74018276
		 0.049591213 0.74018276 0.049591213 0.74122643 0.047099493 0.74122643 0.049591213
		 0.7446267 0.047099493 0.7446267 0.05085104 0.74122643 0.05085104 0.7446267;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 7 ".pt[0:6]" -type "float3"  0 0 4.4768004 0 0 4.4768004 
		0 0 4.4768004 0 0 4.4768004 0 0 4.4768004 0 0 4.4768004 0 0 4.4768004;
	setAttr -s 7 ".vt[0:6]"  -3.5118506 -0.26366088 -14.34806061 -2.728894 -0.26366088 -14.34806061
		 -3.46744132 0.29503959 -14.47004318 -2.77330327 0.29503959 -14.47004318 -3.46744132 0.29503959 -16.37683487
		 -2.77330327 0.29503959 -16.37683487 -2.728894 -0.26366088 -16.49882126;
	setAttr -s 9 ".ed[0:8]"  0 1 0 2 3 0 4 5 0 0 2 0 1 3 0 2 4 0 3 5 0
		 5 6 0 6 1 0;
	setAttr -s 3 ".fc[0:2]" -type "polyFaces" 
		f 4 0 4 -2 -4
		mu 0 4 0 1 2 3
		mu 1 4 0 1 2 3
		f 4 1 6 -3 -6
		mu 0 4 3 2 4 5
		mu 1 4 3 2 4 5
		f 4 -9 -8 -7 -5
		mu 0 4 1 6 7 2
		mu 1 4 6 7 4 2;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "SideBlock10" -p "BridgePiece3";
	setAttr ".rp" -type "double3" -3.1230328369140627 0.015689353942871093 -8.7453392676603663 ;
	setAttr ".sp" -type "double3" -3.1230328369140627 0.015689353942871093 -8.7453392676603663 ;
createNode mesh -n "SideBlockShape10" -p "SideBlock10";
	setAttr -k off ".v";
	setAttr ".iog[0].og[0].gcl" -type "componentList" 3 "f[0:2]" "f[1]" "f[2]";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 8 ".uvst[0].uvsp[0:7]" -type "float2" 0.060852632 0.17146137
		 0.2108359 0.17146137 0.21108857 0.24945144 0.061205044 0.24967495 0.2108359 0.17146137
		 0.060852632 0.17146137 0.060852632 0.17146137 0.061205044 0.24967495;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 8 ".uvst[1].uvsp[0:7]" -type "float2" 0.020277739 0.73611611
		 0.022799522 0.73611611 0.022799522 0.73717237 0.020277739 0.73717237 0.022799522
		 0.7406137 0.020277739 0.7406137 0.024074499 0.73717237 0.024074499 0.7406137;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 7 ".pt[0:6]" -type "float3"  0 0 4.4768004 0 0 4.4768004 
		0 0 4.4768004 0 0 4.4768004 0 0 4.4768004 0 0 4.4768004 0 0 4.4768004;
	setAttr -s 7 ".vt[0:6]"  -3.51451039 -0.26366088 -12.14675617 -2.73155522 -0.26366088 -12.14675617
		 -3.47764158 0.29503959 -12.24803543 -2.76842523 0.29503959 -12.24803543 -3.47764158 0.29503959 -14.19623947
		 -2.76842523 0.29503959 -14.19623947 -2.73155522 -0.26366088 -14.2975235;
	setAttr -s 9 ".ed[0:8]"  0 1 0 2 3 0 4 5 0 0 2 0 1 3 0 2 4 0 3 5 0
		 5 6 0 6 1 0;
	setAttr -s 3 ".fc[0:2]" -type "polyFaces" 
		f 4 0 4 -2 -4
		mu 0 4 0 1 2 3
		mu 1 4 0 1 2 3
		f 4 1 6 -3 -6
		mu 0 4 3 2 4 5
		mu 1 4 3 2 4 5
		f 4 -9 -8 -7 -5
		mu 0 4 1 6 7 2
		mu 1 4 6 7 4 2;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "SideBlock11" -p "BridgePiece3";
	setAttr ".rp" -type "double3" -3.120372314453125 0.015689353942871093 -6.5058487401830121 ;
	setAttr ".sp" -type "double3" -3.120372314453125 0.015689353942871093 -6.5058487401830121 ;
createNode mesh -n "SideBlockShape11" -p "SideBlock11";
	setAttr -k off ".v";
	setAttr ".iog[0].og[0].gcl" -type "componentList" 3 "f[0:2]" "f[1]" "f[2]";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 8 ".uvst[0].uvsp[0:7]" -type "float2" 0.08839187 0.0012279456
		 0.17260642 0.0012279456 0.17172465 0.091203481 0.08839187 0.091541991 0.17260642
		 0.0012279456 0.08839187 0.0012279456 0.08839187 0.0012279456 0.08839187 0.091541991;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 8 ".uvst[1].uvsp[0:7]" -type "float2" 0.07392142 0.73929954
		 0.076413125 0.73929954 0.076413125 0.74034327 0.07392142 0.74034327 0.076413125 0.74374354
		 0.07392142 0.74374354 0.077672951 0.74034327 0.077672951 0.74374354;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 7 ".pt[0:6]" -type "float3"  0 0 8.9175911 0 0 8.9175911 
		0 0 8.9175911 0 0 8.9175911 0 0 8.9175911 0 0 8.9175911 0 0 8.9175911;
	setAttr -s 7 ".vt[0:6]"  -3.5118506 -0.26366088 -14.34806061 -2.728894 -0.26366088 -14.34806061
		 -3.46744132 0.29503959 -14.47004318 -2.77330327 0.29503959 -14.47004318 -3.46744132 0.29503959 -16.37683487
		 -2.77330327 0.29503959 -16.37683487 -2.728894 -0.26366088 -16.49882126;
	setAttr -s 9 ".ed[0:8]"  0 1 0 2 3 0 4 5 0 0 2 0 1 3 0 2 4 0 3 5 0
		 5 6 0 6 1 0;
	setAttr -s 3 ".fc[0:2]" -type "polyFaces" 
		f 4 0 4 -2 -4
		mu 0 4 0 1 2 3
		mu 1 4 0 1 2 3
		f 4 1 6 -3 -6
		mu 0 4 3 2 4 5
		mu 1 4 3 2 4 5
		f 4 -9 -8 -7 -5
		mu 0 4 1 6 7 2
		mu 1 4 6 7 4 2;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "decals";
	setAttr ".rp" -type "double3" -0.096756528202814426 18.284288330078127 -0.54054279327392585 ;
	setAttr ".sp" -type "double3" -0.096756528202814426 18.284288330078127 -0.54054279327392585 ;
createNode mesh -n "decalsShape" -p "decals";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.90099424123764038 0.53932127356529236 ;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 8 ".uvst[0].uvsp[0:7]" -type "float2" 0.37645119 0.1173339
		 0.38907367 0.1173339 0.38907367 0.0071263751 0.37645119 0.0071263751 0.38907367 0.1173339
		 0.37645119 0.1173339 0.37645119 0.04756945 0.38907367 0.04756945;
	setAttr ".uvst[2].uvsn" -type "string" "LightMap";
	setAttr -s 8 ".uvst[2].uvsp[0:7]" -type "float2" 0.061219245 0.64514732
		 0.064037785 0.6451472 0.064058676 0.65617806 0.061221235 0.65617889 0.061217342 0.63426179
		 0.064017221 0.63426238 0.06121622 0.62798536 0.064005345 0.62798637;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -0.52391499 18.28428841 -0.54054278 0.27729923 18.28428841 -0.54054278
		 0.27729923 12.19243526 -0.54054278 -0.52391499 12.19243526 -0.54054278 0.27729923 6.10058212 -0.54054278
		 -0.52391499 6.10058212 -0.54054278 -0.52391505 2.54311562 -0.54054296 0.27729923 2.54311562 -0.54054296;
	setAttr -s 10 ".ed[0:9]"  0 1 0 2 1 0 3 0 0 2 3 1 4 2 0 5 3 0 4 5 1
		 6 5 0 7 4 0 6 7 0;
	setAttr -s 3 ".fc[0:2]" -type "polyFaces" 
		f 4 -4 1 -1 -3
		mu 0 4 3 2 1 0
		mu 2 4 0 1 2 3
		f 4 -7 4 3 -6
		mu 0 4 5 4 2 3
		mu 2 4 4 5 1 0
		f 4 9 8 6 -8
		mu 0 4 6 7 4 5
		mu 2 4 6 7 5 4;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".ndt" 0;
createNode transform -n "oz_ec_catacombs_straight_over_anim_a";
createNode mesh -n "oz_ec_catacombs_straight_over_anim_aShape" -p "oz_ec_catacombs_straight_over_anim_a";
	addAttr -ci true -sn "mso" -ln "miShadingSamplesOverride" -min 0 -max 1 -at "bool";
	addAttr -ci true -sn "msh" -ln "miShadingSamples" -min 0 -smx 8 -at "float";
	addAttr -ci true -sn "mdo" -ln "miMaxDisplaceOverride" -min 0 -max 1 -at "bool";
	addAttr -ci true -sn "mmd" -ln "miMaxDisplace" -min 0 -smx 1 -at "float";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.46658390760421753 0.96937775611877441 ;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 1383 ".uvst[0].uvsp";
	setAttr ".uvst[0].uvsp[0:249]" -type "float2" 0.13753682 0.41311112 0.13753682
		 0.43508753 0.092664257 0.41311112 0.092457794 0.43508753 0.13753682 0.41311112 0.13753682
		 0.43508753 0.092664264 0.41311112 0.092457794 0.43508753 0.11240699 0.34601921 0.11240699
		 0.3682327 0.068513334 0.3682327 0.068719797 0.34601921 0.11240699 0.3682327 0.11240699
		 0.34601921 0.068513334 0.3682327 0.068719804 0.34601921 0.29342604 0.25101951 0.29986763
		 0.25101951 0.30036724 0.37307423 0.29380703 0.37307423 0.28009927 0.25101951 0.28024819
		 0.37307423 0.35027626 0.38998505 0.36501154 0.38998505 0.36439753 0.48394603 0.34996924
		 0.48394603 0.28247967 0.38967806 0.33861086 0.38967806 0.33861086 0.48363897 0.28247961
		 0.48363897 0.26006973 0.48394603 0.26006979 0.38998511 0.27633998 0.38998511 0.27633992
		 0.48394603 0.26103801 0.37307423 0.26257348 0.25101951 0.27666399 0.25101951 0.27540508
		 0.37307423 0.25462121 0.25101951 0.25295398 0.37307423 0.27012151 0.39249751 0.34948295
		 0.39249751 0.34948295 0.47917029 0.27012151 0.47917029 0.30991596 0.29204279 0.3495878
		 0.29204279 0.3495878 0.3308365 0.30991596 0.3308365 0.29342604 0.25101951 0.29986763
		 0.25101951 0.30036724 0.37307423 0.29380703 0.37307423 0.25462121 0.25101951 0.26257348
		 0.25101951 0.26103801 0.37307423 0.25295398 0.37307423 0.28024819 0.37307423 0.28009927
		 0.25101951 0.31041092 0.33002836 0.27335909 0.33002836 0.27335909 0.29149234 0.31041092
		 0.29149234 0.27666399 0.25101951 0.27540508 0.37307423 0.36954817 0.38895214 0.36898059
		 0.4847334 0.35479113 0.4847334 0.35479113 0.38895214 0.32522559 0.41767815 0.28817374
		 0.41767815 0.28817374 0.45646784 0.32522559 0.45646784 0.25289997 0.4847334 0.25375134
		 0.38895214 0.26708952 0.38895214 0.26708952 0.4847334 0.31041092 0.29166237 0.27335909
		 0.29166237 0.3495152 0.29183337 0.3492772 0.33107349 0.31026089 0.33091459 0.31026089
		 0.29192233 0.31026089 0.33091459 0.31026089 0.29192233 0.3495152 0.29183337 0.3492772
		 0.33107349 0.29342604 0.25101951 0.29986763 0.25101951 0.30036724 0.37307423 0.29380703
		 0.37307423 0.28009927 0.25101951 0.28024819 0.36241159 0.3519958 0.38220033 0.33622822
		 0.38220033 0.33558464 0.49298361 0.3532829 0.49298361 0.25536096 0.38107434 0.36743924
		 0.38107434 0.36743924 0.49185768 0.25536096 0.49185768 0.26215303 0.49076933 0.26247481
		 0.37998596 0.27663338 0.37998596 0.27663338 0.49076933 0.26103801 0.37307423 0.26257348
		 0.25101951 0.27666399 0.25101951 0.27540508 0.36241159 0.25462121 0.25101951 0.25295398
		 0.37307423 0.29342604 0.25101951 0.29986763 0.25101951 0.30036724 0.37307423 0.29380703
		 0.37307423 0.28009927 0.25101951 0.28024819 0.37307423 0.35027626 0.38998505 0.36501154
		 0.38998505 0.36439753 0.48394603 0.34996924 0.48394603 0.28247967 0.38967806 0.33861086
		 0.38967806 0.33861086 0.48363897 0.28247961 0.48363897 0.26006973 0.48394603 0.26006979
		 0.38998511 0.27633998 0.38998511 0.27633992 0.48394603 0.26103801 0.37307423 0.26257348
		 0.25101951 0.27666399 0.25101951 0.27540508 0.37307423 0.25462121 0.25101951 0.25295398
		 0.37307423 0.27012151 0.39249751 0.34948295 0.39249751 0.34948295 0.47917029 0.27012151
		 0.47917029 0.30991596 0.29204279 0.3495878 0.29204279 0.3495878 0.3308365 0.30991596
		 0.3308365 0.29342604 0.25101951 0.29986763 0.25101951 0.30036724 0.37307423 0.29380703
		 0.37307423 0.25462121 0.25101951 0.26257348 0.25101951 0.26103801 0.37307423 0.25295398
		 0.37307423 0.28024819 0.37307423 0.28009927 0.25101951 0.31041092 0.33002836 0.27335909
		 0.33002836 0.27335909 0.29149234 0.31041092 0.29149234 0.27666399 0.25101951 0.27540508
		 0.37307423 0.36954817 0.38895214 0.36898059 0.4847334 0.35479113 0.4847334 0.35479113
		 0.38895214 0.32522559 0.41767815 0.28817374 0.41767815 0.28817374 0.45646784 0.32522559
		 0.45646784 0.25289997 0.4847334 0.25375134 0.38895214 0.26708952 0.38895214 0.26708952
		 0.4847334 0.31041092 0.29166237 0.27335909 0.29166237 0.3495152 0.29183337 0.3492772
		 0.33107349 0.31026089 0.33091459 0.31026089 0.29192233 0.31026089 0.33091459 0.31026089
		 0.29192233 0.3495152 0.29183337 0.3492772 0.33107349 0.29342604 0.25101951 0.29986763
		 0.25101951 0.30036724 0.37307423 0.29380703 0.37307423 0.28009927 0.25101951 0.28024819
		 0.36241159 0.3519958 0.38220033 0.33622822 0.38220033 0.33558464 0.49298361 0.3532829
		 0.49298361 0.25536096 0.38107434 0.36743924 0.38107434 0.36743924 0.49185768 0.25536096
		 0.49185768 0.26215303 0.49076933 0.26247481 0.37998596 0.27663338 0.37998596 0.27663338
		 0.49076933 0.26103801 0.37307423 0.26257348 0.25101951 0.27666399 0.25101951 0.27540508
		 0.36241159 0.25462121 0.25101951 0.25295398 0.37307423 0.29342604 0.25101951 0.29986763
		 0.25101951 0.30036724 0.37307423 0.29380703 0.37307423 0.28009927 0.25101951 0.28024819
		 0.37307423 0.35027626 0.38998505 0.36501154 0.38998505 0.36439753 0.48394603 0.34996924
		 0.48394603 0.28247967 0.38967806 0.33861086 0.38967806 0.33861086 0.48363897 0.28247961
		 0.48363897 0.26006973 0.48394603 0.26006979 0.38998511 0.27633998 0.38998511 0.27633992
		 0.48394603 0.26103801 0.37307423 0.26257348 0.25101951 0.27666399 0.25101951 0.27540508
		 0.37307423 0.25462121 0.25101951 0.25295398 0.37307423 0.27012151 0.39249751 0.34948295
		 0.39249751 0.34948295 0.47917029 0.27012151 0.47917029 0.29342604 0.25101951 0.29986763
		 0.25101951 0.30036724 0.37307423 0.29380703 0.37307423 0.25462121 0.25101951 0.26257348
		 0.25101951 0.26103801 0.37307423 0.25295398 0.37307423 0.28024819 0.37307423 0.28009927
		 0.25101951 0.27666399 0.25101951 0.27540508 0.37307423 0.36954817 0.38895214 0.36898059
		 0.4847334 0.35479113 0.4847334 0.35479113 0.38895214 0.25289997 0.4847334 0.25375134
		 0.38895214;
	setAttr ".uvst[0].uvsp[250:499]" 0.26708952 0.38895214 0.26708952 0.4847334
		 0.29342604 0.25101951 0.29986763 0.25101951 0.30036724 0.37307423 0.29380703 0.37307423
		 0.28009927 0.25101951 0.28024819 0.37307423 0.35027626 0.38998505 0.36501154 0.38998505
		 0.36439753 0.48394603 0.34996924 0.48394603 0.28247967 0.38967806 0.33861086 0.38967806
		 0.33861086 0.48363897 0.28247961 0.48363897 0.26006973 0.48394603 0.26006979 0.38998511
		 0.27633998 0.38998511 0.27633992 0.48394603 0.26103801 0.37307423 0.26257348 0.25101951
		 0.27666399 0.25101951 0.27540508 0.37307423 0.25462121 0.25101951 0.25295398 0.37307423
		 0.27012151 0.39249751 0.34948295 0.39249751 0.34948295 0.47917029 0.27012151 0.47917029
		 0.29342604 0.25101951 0.29986763 0.25101951 0.30036724 0.37307423 0.29380703 0.37307423
		 0.25462121 0.25101951 0.26257348 0.25101951 0.26103801 0.37307423 0.25295398 0.37307423
		 0.28024819 0.37307423 0.28009927 0.25101951 0.27666399 0.25101951 0.27540508 0.37307423
		 0.36954817 0.38895214 0.36898059 0.4847334 0.35479113 0.4847334 0.35479113 0.38895214
		 0.25289997 0.4847334 0.25375134 0.38895214 0.26708952 0.38895214 0.26708952 0.4847334
		 0.29342604 0.25101951 0.29986763 0.25101951 0.30036724 0.37307423 0.29380703 0.37307423
		 0.28009927 0.25101951 0.28024819 0.37307423 0.35027626 0.38998505 0.36501154 0.38998505
		 0.36439753 0.48394603 0.34996924 0.48394603 0.28247967 0.38967806 0.33861086 0.38967806
		 0.33861086 0.48363897 0.28247961 0.48363897 0.26006973 0.48394603 0.26006979 0.38998511
		 0.27633998 0.38998511 0.27633992 0.48394603 0.26103801 0.37307423 0.26257348 0.25101951
		 0.27666399 0.25101951 0.27540508 0.37307423 0.25462121 0.25101951 0.25295398 0.37307423
		 0.27012151 0.39249751 0.34948295 0.39249751 0.34948295 0.47917029 0.27012151 0.47917029
		 0.30991596 0.29204279 0.3495878 0.29204279 0.3495878 0.3308365 0.30991596 0.3308365
		 0.29342604 0.25101951 0.29986763 0.25101951 0.30036724 0.37307423 0.29380703 0.37307423
		 0.25462121 0.25101951 0.26257348 0.25101951 0.26103801 0.37307423 0.25295398 0.37307423
		 0.28024819 0.37307423 0.28009927 0.25101951 0.31041092 0.33002836 0.27335909 0.33002836
		 0.27335909 0.29149234 0.31041092 0.29149234 0.27666399 0.25101951 0.27540508 0.37307423
		 0.36954817 0.38895214 0.36898059 0.4847334 0.35479113 0.4847334 0.35479113 0.38895214
		 0.32522559 0.41767815 0.28817374 0.41767815 0.28817374 0.45646784 0.32522559 0.45646784
		 0.25289997 0.4847334 0.25375134 0.38895214 0.26708952 0.38895214 0.26708952 0.4847334
		 0.31041092 0.29166237 0.27335909 0.29166237 0.3495152 0.29183337 0.3492772 0.33107349
		 0.31026089 0.33091459 0.31026089 0.29192233 0.31026089 0.33091459 0.31026089 0.29192233
		 0.3495152 0.29183337 0.3492772 0.33107349 0.29342604 0.25101951 0.29986763 0.25101951
		 0.30036724 0.37307423 0.29380703 0.37307423 0.28009927 0.25101951 0.28024819 0.36241159
		 0.3519958 0.38220033 0.33622822 0.38220033 0.33558464 0.49298361 0.3532829 0.49298361
		 0.25536096 0.38107434 0.36743924 0.38107434 0.36743924 0.49185768 0.25536096 0.49185768
		 0.26215303 0.49076933 0.26247481 0.37998596 0.27663338 0.37998596 0.27663338 0.49076933
		 0.26103801 0.37307423 0.26257348 0.25101951 0.27666399 0.25101951 0.27540508 0.36241159
		 0.25462121 0.25101951 0.25295398 0.37307423 0.29342604 0.25101951 0.29986763 0.25101951
		 0.30036724 0.37307423 0.29380703 0.37307423 0.28009927 0.25101951 0.28024819 0.37307423
		 0.35027626 0.38998505 0.36501154 0.38998505 0.36439753 0.48394603 0.34996924 0.48394603
		 0.28247967 0.38967806 0.33861086 0.38967806 0.33861086 0.48363897 0.28247961 0.48363897
		 0.26006973 0.48394603 0.26006979 0.38998511 0.27633998 0.38998511 0.27633992 0.48394603
		 0.26103801 0.37307423 0.26257348 0.25101951 0.27666399 0.25101951 0.27540508 0.37307423
		 0.25462121 0.25101951 0.25295398 0.37307423 0.27012151 0.39249751 0.34948295 0.39249751
		 0.34948295 0.47917029 0.27012151 0.47917029 0.29342604 0.25101951 0.29986763 0.25101951
		 0.30036724 0.37307423 0.29380703 0.37307423 0.25462121 0.25101951 0.26257348 0.25101951
		 0.26103801 0.37307423 0.25295398 0.37307423 0.28024819 0.37307423 0.28009927 0.25101951
		 0.27666399 0.25101951 0.27540508 0.37307423 0.36954817 0.38895214 0.36898059 0.4847334
		 0.35479113 0.4847334 0.35479113 0.38895214 0.25289997 0.4847334 0.25375134 0.38895214
		 0.26708952 0.38895214 0.26708952 0.4847334 0.29342604 0.25101951 0.29986763 0.25101951
		 0.30036724 0.37307423 0.29380703 0.37307423 0.28009927 0.25101951 0.28024819 0.37307423
		 0.35027626 0.38998505 0.36501154 0.38998505 0.36439753 0.48394603 0.34996924 0.48394603
		 0.28247967 0.38967806 0.33861086 0.38967806 0.33861086 0.48363897 0.28247961 0.48363897
		 0.26006973 0.48394603 0.26006979 0.38998511 0.27633998 0.38998511 0.27633992 0.48394603
		 0.26103801 0.37307423 0.26257348 0.25101951 0.27666399 0.25101951 0.27540508 0.37307423
		 0.25462121 0.25101951 0.25295398 0.37307423 0.27012151 0.39249751 0.34948295 0.39249751
		 0.34948295 0.47917029 0.27012151 0.47917029 0.29342604 0.25101951 0.29986763 0.25101951
		 0.30036724 0.37307423 0.29380703 0.37307423 0.25462121 0.25101951 0.26257348 0.25101951
		 0.26103801 0.37307423 0.25295398 0.37307423 0.28024819 0.37307423 0.28009927 0.25101951
		 0.27666399 0.25101951 0.27540508 0.37307423 0.36954817 0.38895214 0.36898059 0.4847334
		 0.35479113 0.4847334 0.35479113 0.38895214 0.25289997 0.4847334 0.25375134 0.38895214
		 0.26708952 0.38895214 0.26708952 0.4847334 0.29342604 0.25101951 0.29986763 0.25101951
		 0.30036724 0.37307423 0.29380703 0.37307423 0.28009927 0.25101951 0.28024819 0.37307423
		 0.35027626 0.38998505 0.36501154 0.38998505 0.36439753 0.48394603 0.34996924 0.48394603;
	setAttr ".uvst[0].uvsp[500:749]" 0.28247967 0.38967806 0.33861086 0.38967806
		 0.33861086 0.48363897 0.28247961 0.48363897 0.26006973 0.48394603 0.26006979 0.38998511
		 0.27633998 0.38998511 0.27633992 0.48394603 0.26103801 0.37307423 0.26257348 0.25101951
		 0.27666399 0.25101951 0.27540508 0.37307423 0.25462121 0.25101951 0.25295398 0.37307423
		 0.29342604 0.25101951 0.29986763 0.25101951 0.30036724 0.37307423 0.29380703 0.37307423
		 0.28009927 0.25101951 0.28024819 0.37307423 0.35027626 0.38998505 0.36501154 0.38998505
		 0.36439753 0.48394603 0.34996924 0.48394603 0.28247967 0.38967806 0.33861086 0.38967806
		 0.33861086 0.48363897 0.28247961 0.48363897 0.26006973 0.48394603 0.26006979 0.38998511
		 0.27633998 0.38998511 0.27633992 0.48394603 0.26103801 0.37307423 0.26257348 0.25101951
		 0.27666399 0.25101951 0.27540508 0.37307423 0.25462121 0.25101951 0.25295398 0.37307423
		 0.27012151 0.39249751 0.34948295 0.39249751 0.34948295 0.47917029 0.27012151 0.47917029
		 0.30991596 0.29204279 0.3495878 0.29204279 0.3495878 0.3308365 0.30991596 0.3308365
		 0.29342604 0.25101951 0.29986763 0.25101951 0.30036724 0.37307423 0.29380703 0.37307423
		 0.25462121 0.25101951 0.26257348 0.25101951 0.26103801 0.37307423 0.25295398 0.37307423
		 0.28024819 0.37307423 0.28009927 0.25101951 0.31041092 0.33002836 0.27335909 0.33002836
		 0.27335909 0.29149234 0.31041092 0.29149234 0.27666399 0.25101951 0.27540508 0.37307423
		 0.36954817 0.38895214 0.36898059 0.4847334 0.35479113 0.4847334 0.35479113 0.38895214
		 0.32522559 0.41767815 0.28817374 0.41767815 0.28817374 0.45646784 0.32522559 0.45646784
		 0.25289997 0.4847334 0.25375134 0.38895214 0.26708952 0.38895214 0.26708952 0.4847334
		 0.31041092 0.29166237 0.27335909 0.29166237 0.3495152 0.29183337 0.3492772 0.33107349
		 0.31026089 0.33091459 0.31026089 0.29192233 0.31026089 0.33091459 0.31026089 0.29192233
		 0.3495152 0.29183337 0.3492772 0.33107349 0.29342604 0.25101951 0.29986763 0.25101951
		 0.30036724 0.37307423 0.29380703 0.37307423 0.28009927 0.25101951 0.28024819 0.36241159
		 0.3519958 0.38220033 0.33622822 0.38220033 0.33558464 0.49298361 0.3532829 0.49298361
		 0.25536096 0.38107434 0.36743924 0.38107434 0.36743924 0.49185768 0.25536096 0.49185768
		 0.26215303 0.49076933 0.26247481 0.37998596 0.27663338 0.37998596 0.27663338 0.49076933
		 0.26103801 0.37307423 0.26257348 0.25101951 0.27666399 0.25101951 0.27540508 0.36241159
		 0.25462121 0.25101951 0.25295398 0.37307423 0.29342604 0.25101951 0.29986763 0.25101951
		 0.30036724 0.37307423 0.29380703 0.37307423 0.28009927 0.25101951 0.28024819 0.37307423
		 0.35027626 0.38998505 0.36501154 0.38998505 0.36439753 0.48394603 0.34996924 0.48394603
		 0.28247967 0.38967806 0.33861086 0.38967806 0.33861086 0.48363897 0.28247961 0.48363897
		 0.26006973 0.48394603 0.26006979 0.38998511 0.27633998 0.38998511 0.27633992 0.48394603
		 0.26103801 0.37307423 0.26257348 0.25101951 0.27666399 0.25101951 0.27540508 0.37307423
		 0.25462121 0.25101951 0.25295398 0.37307423 0.43708754 0.34285331 0.43761206 0.32932684
		 0.37797427 0.32891929 0.37940562 0.3424753 0.4386645 0.32800829 0.43856949 0.27952659
		 0.38278902 0.27963847 0.38201299 0.32811275 0.43848747 0.25849846 0.38241637 0.25861493
		 0.43585932 0.25455531 0.43593502 0.27651432 0.49328601 0.27635536 0.49327767 0.25439394
		 0.49573493 0.29346773 0.43692577 0.29319084 0.43674254 0.31990141 0.49527144 0.32015634
		 0.43624449 0.37160075 0.49389923 0.37145072 0.4947598 0.32497373 0.43605542 0.32513133
		 0.43584645 0.36615914 0.48919052 0.36635906 0.49216956 0.34684408 0.43621397 0.34662834
		 0.43789196 0.27698988 0.43772018 0.25783095 0.38040316 0.25735459 0.37899959 0.27652374
		 0.43792105 0.30246404 0.37787974 0.3020204 0.43674386 0.35506603 0.37981617 0.35471901
		 0.43614829 0.37047654 0.381832 0.37015158 0.43888566 0.36967945 0.38374332 0.36977339
		 0.4364211 0.33532941 0.49303818 0.33556461 0.49443209 0.26998588 0.43689924 0.26969725
		 0.49265212 0.25321299 0.43678695 0.25292039 0.38083971 0.25262737 0.37927932 0.26940814
		 0.37802559 0.29291353 0.3781237 0.319646 0.37971985 0.33509386 0.38017589 0.34641221
		 0.38242716 0.36595887 0.37845266 0.25471666 0.37859619 0.27667323 0.37736309 0.32528892
		 0.37860155 0.37175077 0.49404833 0.36958545 0.49533772 0.32790384 0.49437135 0.27941465
		 0.49457955 0.25838199 0.49034321 0.37080079 0.49353755 0.35541221 0.49463224 0.34323049
		 0.49710345 0.32973334 0.4978143 0.30290654 0.49664128 0.27745485 0.49490213 0.25830618
		 0.43708754 0.34285331 0.43761206 0.32932684 0.37797427 0.32891929 0.37940562 0.3424753
		 0.4386645 0.32800829 0.43856949 0.27952659 0.38278902 0.27963847 0.38201299 0.32811275
		 0.43848747 0.25849846 0.38241637 0.25861493 0.43585932 0.25455531 0.43593502 0.27651432
		 0.49328601 0.27635536 0.49327767 0.25439394 0.49573493 0.29346773 0.43692577 0.29319084
		 0.43674254 0.31990141 0.49527144 0.32015634 0.43624449 0.37160075 0.49389923 0.37145072
		 0.4947598 0.32497373 0.43605542 0.32513133 0.43584645 0.36615914 0.48919052 0.36635906
		 0.49216956 0.34684408 0.43621397 0.34662834 0.43789196 0.27698988 0.43772018 0.25783095
		 0.38040316 0.25735459 0.37899959 0.27652374 0.43792105 0.30246404 0.37787974 0.3020204
		 0.43674386 0.35506603 0.37981617 0.35471901 0.43614829 0.37047654 0.381832 0.37015158
		 0.43888566 0.36967945 0.38374332 0.36977339 0.4364211 0.33532941 0.49303818 0.33556461
		 0.49443209 0.26998588 0.43689924 0.26969725 0.49265212 0.25321299 0.43678695 0.25292039
		 0.24992572 0.22830391 0.37022519 0.22830391 0.37022519 0.21254127 0.24992572 0.21254127
		 0.37022519 0.16983248 0.24992572 0.16983248 0.37022519 0.15606827 0.24992575 0.15606827;
	setAttr ".uvst[0].uvsp[750:999]" 0.24998452 0.22830391 0.24998452 0.21254127
		 0.24998452 0.16983248 0.24998452 0.15606827 0.37298524 0.21207494 0.37298524 0.22779298
		 0.25268579 0.22779298 0.25268579 0.21207494 0.37298524 0.16948709 0.25268579 0.16948709
		 0.37298524 0.15576181 0.25268579 0.15576181 0.25274462 0.15576181 0.25274462 0.16948709
		 0.25274462 0.21207494 0.25274462 0.22779298 0.24992572 0.22830391 0.37022519 0.22830391
		 0.37022519 0.21254127 0.24992572 0.21254127 0.37022519 0.16983248 0.24992572 0.16983248
		 0.37022519 0.15606827 0.24992575 0.15606827 0.14188424 0.24840274 0.14188422 0.0024642944
		 0.19593176 0.0024642791 0.19593175 0.24840277 0.37298524 0.21207494 0.37298524 0.22779298
		 0.25268579 0.22779298 0.25268579 0.21207494 0.37298524 0.16948709 0.25268579 0.16948709
		 0.37298524 0.15576181 0.25268579 0.15576181 0.37511653 0.24843208 0.37511653 0.12605621
		 0.25094736 0.12605621 0.25094736 0.24843208 0.040781654 0.16846523 0.040781654 0.24968255
		 0.0024680346 0.24968253 0.0024680346 0.16846523 0.18594432 0.24840274 0.18594432
		 0.0024642819 0.24671398 0.0024642646 0.24671395 0.24840277 0.12094909 0.12923664
		 0.12094909 0.11491698 0.18222874 0.11491698 0.18222874 0.12923664 0.12980653 0.25071549
		 0.12980653 0.39092806 0.0011605734 0.39092806 0.0011605737 0.25071549 0.24861985
		 0.25071549 0.24861985 0.39092806 0.11126965 0.1607641 0.11126959 0.14615478 0.16589932
		 0.14615478 0.16589932 0.1607641 0.06313552 0.00023305416 0.063135616 0.24954787 0.0029916689
		 0.24954788 0.0029914901 0.00023305416 0.10177019 0.089948229 0.10177019 0.17254457
		 0.039589748 0.17254457 0.039589748 0.089948229 0.24392563 0.0060765147 0.24392566
		 0.24568893 0.010044701 0.24568892 0.010044686 0.0060765147 0.0034270138 0.0031477553
		 0.0034269691 0.24693367 0.24434425 0.24693367 0.24434426 0.0031477218 0.37409717
		 0.12657616 0.37409717 0.24937434 0.25740156 0.24937436 0.25740156 0.12657616 0.28579423
		 0.12657616 0.28579423 0.24937436 0.37409717 0.24937436 0.37409717 0.12657616 0.18572779
		 0.0031477301 0.18572776 0.24693367 0.0034269691 0.24693367 0.0034270138 0.0031477553
		 0.062545285 0.0060765222 0.0625453 0.24568892 0.24392566 0.24568893 0.24392563 0.0060765147
		 0.25740156 0.12657616 0.25740156 0.24937436 0.37409717 0.24937436 0.37409717 0.12657616
		 0.24434426 0.0031477218 0.24434425 0.24693367 0.0034269691 0.24693367 0.0034270138
		 0.0031477553 0.0042247921 0.0060765222 0.004224807 0.24568892 0.24392566 0.24568893
		 0.24392563 0.0060765147 0.25740156 0.12657616 0.34831977 0.12657616 0.34831977 0.24937436
		 0.25740156 0.24937436 0.24434426 0.0031477218 0.056644287 0.0031477478 0.05664425
		 0.24693367 0.24434425 0.24693367 0.1909771 0.0060765147 0.1909771 0.24568892 0.010044701
		 0.24568892 0.010044686 0.0060765147 0.24392563 0.0060765147 0.24392566 0.24568893
		 0.010044701 0.24568892 0.010044686 0.0060765147 0.0034270138 0.0031477553 0.0034269691
		 0.24693367 0.24434425 0.24693367 0.24434426 0.0031477218 0.37409717 0.12657616 0.37409717
		 0.24937434 0.25740156 0.24937436 0.25740156 0.12657616 0.18975773 0.0042658746 0.18975773
		 0.24531032 0.018865339 0.24531032 0.018865339 0.0042658746 0.018865339 0.0042658746
		 0.018865339 0.24531032 0.24470598 0.0042658746 0.24470598 0.24531032 0.018865339
		 0.24531032 0.018865339 0.0042658746 0.068752319 0.24531032 0.24470598 0.24531032
		 0.24470598 0.0042658746 0.068752319 0.0042658746 0.018865339 0.0042658746 0.018865339
		 0.24531032 0.24470598 0.24531032 0.24470598 0.0042658746 0.24470598 0.0042658746
		 0.24470598 0.24531032 0.0042247921 0.0060765222 0.004224807 0.24568892 0.24434426
		 0.0031477218 0.24434425 0.24693367 0.25740156 0.12657616 0.25740156 0.24937436 0.24392566
		 0.24568893 0.24392563 0.0060765147 0.0034269691 0.24693367 0.0034270138 0.0031477553
		 0.37409717 0.24937434 0.37409717 0.12657616 0.2447727 0.24620613 0.0084408969 0.24620602
		 0.0084409118 0.0047812238 0.2447727 0.0047812313 0.0034269989 0.24693371 0.2467888
		 0.24693373 0.2467888 0.0031477734 0.0034270063 0.0031478032 0.3683061 0.24937437
		 0.24889615 0.24937436 0.24889615 0.12657616 0.3683061 0.12657614 0.24889615 0.24937436
		 0.34944934 0.24937434 0.34944934 0.12657614 0.24889615 0.12657616 0.24678877 0.0031477213
		 0.24678876 0.2469337 0.041857705 0.24693365 0.041857731 0.0031477842 0.013314277
		 0.0047812089 0.013314292 0.24620616 0.2082217 0.24620612 0.2082217 0.0047812238 0.25740156
		 0.12657616 0.25740156 0.24937436 0.37409717 0.24937436 0.37409717 0.12657616 0.24434426
		 0.0031477218 0.24434425 0.24693367 0.0034269691 0.24693367 0.0034270138 0.0031477553
		 0.0042247921 0.0060765222 0.004224807 0.24568892 0.24392566 0.24568893 0.24392563
		 0.0060765147 0.20578393 0.0060765222 0.20578392 0.24568892 0.010044701 0.24568892
		 0.010044686 0.0060765147 0.042716254 0.0031477497 0.042716213 0.24693367 0.24434425
		 0.24693367 0.24434426 0.0031477218 0.35506621 0.12657616 0.35506621 0.24937434 0.25740156
		 0.24937436 0.25740156 0.12657616 0.23349854 0.24531038 0.0024048612 0.24531032 0.0024048612
		 0.0042658746 0.23349854 0.0042658448 0.0024048612 0.24531032 0.19700512 0.24531032
		 0.19700512 0.0042658448 0.0024048612 0.0042658746 0.19112697 0.24693367 0.19112699
		 0.0031477292 0.0034270138 0.0031477553 0.0034269691 0.24693367 0.25094736 0.12605621
		 0.34533939 0.12605621 0.3453393 0.19607802 0.25094736 0.19607802 0.24470598 0.0042658746
		 0.018865339 0.0042658746 0.018865339 0.24531032 0.24470598 0.24531032 0.24470598
		 0.0042658746 0.018865339 0.0042658746 0.018865339 0.24531032 0.24470598 0.24531032
		 0.018865339 0.0042658746 0.20787543 0.0042658746 0.20787543 0.24531032 0.018865339
		 0.24531032 0.0034270138 0.0031477553 0.24434426 0.0031477218 0.24434425 0.24693367
		 0.0034269691 0.24693367 0.24434425 0.24693367 0.0034269691 0.24693367 0.0034270138
		 0.0031477553 0.24434426 0.0031477218 0.24434426 0.0031477218 0.042716254 0.0031477497
		 0.042716213 0.24693367 0.24434425 0.24693367;
	setAttr ".uvst[0].uvsp[1000:1249]" 0.37511653 0.19607802 0.25094736 0.24843208
		 0.37511653 0.24843208 0.25094736 0.12605621 0.37511653 0.12605621 0.32108685 0.24943811
		 0.34838074 0.18001696 0.34838071 0.24943817 0.32108688 0.12678128 0.34838074 0.1267813
		 0.034484845 0.16846523 0.068560347 0.16846523 0.068560347 0.24968253 0.034484845
		 0.24968255 0.34533939 0.2484321 0.25094736 0.24843208 0.30288941 0.12678134 0.30288941
		 0.24943817 0.27559549 0.18030189 0.27559549 0.12678131 0.24434426 0.0031477218 0.24434425
		 0.24693367 0.0034269691 0.24693367 0.0034270138 0.0031477553 0.25094736 0.19607802
		 0.37511653 0.24843208 0.37511653 0.12605621 0.25094736 0.12605621 0.25094736 0.24843208
		 0.27559552 0.24943817 0.2467888 0.0031477734 0.0034270063 0.0031478032 0.0034269989
		 0.24693371 0.2467888 0.24693373 0.24678877 0.0031477213 0.24678876 0.2469337 0.041857705
		 0.24693365 0.041857731 0.0031477842 0.37293249 0.24909362 0.25050858 0.24909362 0.25050858
		 0.12579206 0.37293249 0.12579212 0.25050858 0.12579206 0.25050858 0.24909362 0.370408
		 0.24909362 0.370408 0.12579206 0.37293249 0.24909362 0.25050858 0.24909362 0.25050858
		 0.12579206 0.37293249 0.12579212 0.28063917 0.24943817 0.24995062 0.24943817 0.24995059
		 0.12678128 0.28063914 0.12678131 0.31064284 0.24937436 0.3683061 0.12657616 0.3683061
		 0.24937437 0.370408 0.12579206 0.32016474 0.24909362 0.370408 0.24909362 0.26944262
		 0.12579206 0.26944262 0.24909362 0.2467888 0.24693373 0.2467888 0.0031477734 0.0034270063
		 0.0031478032 0.0034269989 0.24693371 0.30766049 0.24937436 0.24889615 0.12657616
		 0.3683061 0.12657614 0.3683061 0.24937437 0.36830607 0.12657616 0.36830607 0.12657614
		 0.24889615 0.12657616 0.24889615 0.24937436 0.31027156 0.24937436 0.24678876 0.2469337
		 0.003426984 0.24693364 0.0034270138 0.0031477958 0.24678877 0.0031477213 0.2467888
		 0.24693373 0.2467888 0.0031477734 0.0034270063 0.0031478032 0.0034269989 0.24693371
		 0.31072193 0.24909362 0.25050858 0.24909362 0.25050858 0.12579206 0.37293249 0.12579212
		 0.37293249 0.24909362 0.24889615 0.24937436 0.36830607 0.24937433 0.24889615 0.12657616
		 0.3683061 0.12657614 0.24889615 0.24937436 0.0034269691 0.24693367 0.0034270138 0.0031477553
		 0.24434426 0.0031477218 0.24434425 0.24693367 0.37409717 0.12657617 0.31791005 0.24937437
		 0.37409717 0.24937436 0.31770858 0.24843208 0.27070543 0.24843208 0.27070543 0.12605621
		 0.37210172 0.12605621 0.37409717 0.12657616 0.25740156 0.12657616 0.25740156 0.24937436
		 0.25094736 0.24843208 0.37511653 0.24843208 0.37511653 0.12605621 0.25094736 0.12605621
		 0.37210172 0.24843208 0.25094736 0.24843208 0.25094736 0.12605621 0.37210172 0.12605621
		 0.37402558 0.24943823 0.34333709 0.24943817 0.34333712 0.1267813 0.37402564 0.12678131
		 0.25740156 0.12657616 0.31572688 0.24937434 0.37409717 0.24937436 0.37409717 0.12657616
		 0.24434425 0.24693367 0.0034269691 0.24693367 0.0034270138 0.0031477553 0.24434426
		 0.0031477218 0.0034269691 0.24693367 0.24434425 0.24693367 0.24434426 0.0031477218
		 0.0034270138 0.0031477553 0.37511653 0.12605621 0.316845 0.24843207 0.37511653 0.24843208
		 0.25094736 0.12605621 0.25094736 0.24843208 0.37210172 0.24843208 0.25740156 0.12657616
		 0.30946851 0.24937434 0.25740156 0.24937436 0.37409717 0.12657616 0.37409717 0.12657617
		 0.37409717 0.24937434 0.25740156 0.24937436 0.13102034 0.12923664 0.076519236 0.12923664
		 0.076519236 0.11491698 0.13102034 0.11491698 0.0011605734 0.36795506 0.12980653 0.36795506
		 0.12980653 0.49957269 0.0011605732 0.49957269 0.25094736 0.18658598 0.25094736 0.19607802
		 0.053250909 0.00023305416 0.10674202 0.00023305416 0.10674205 0.24954788 0.053251013
		 0.24954788 0.062043495 0.0031477474 0.062043454 0.24693367 0.24434425 0.24693367
		 0.24434426 0.0031477218 0.37210172 0.12605621 0.37210172 0.19607802 0.28042489 0.19607803
		 0.28042489 0.12605621 0.14685316 0.089948229 0.14685316 0.17254457 0.091550902 0.17254457
		 0.091550902 0.089948229 0.28042489 0.24843208 0.37210172 0.24843208 0.24861985 0.49957269
		 0.24861985 0.36795506 0.071661264 0.1607641 0.071661144 0.14615478 0.12024793 0.14615478
		 0.12024799 0.1607641 0.24434426 0.0031477218 0.24434425 0.24693367 0.37511653 0.18508762
		 0.37511653 0.19607802 0.0034269691 0.24693367 0.24434425 0.24693367 0.24434426 0.0031477218
		 0.0034270138 0.0031477553 0.0034269691 0.24693367 0.0034270138 0.0031477553 0.15318595
		 0.37228495 0.24765408 0.31628585 0.2476541 0.42862618 0.25094736 0.12605621 0.37511653
		 0.12605621 0.24765408 0.31628585 0.15318595 0.37228495 0.15200578 0.31628585 0.15318595
		 0.37228495 0.2476541 0.42862618 0.15437348 0.42862618 0.13753682 0.41311112 0.13753682
		 0.43508753 0.092457794 0.43508753 0.092664257 0.41311112 0.13753682 0.43508753 0.13753682
		 0.41311112 0.092457794 0.43508753 0.092664264 0.41311112 0.13753682 0.43508753 0.092457779
		 0.43508753 0.11240699 0.34601921 0.11240699 0.3682327 0.068513334 0.3682327 0.068719797
		 0.34601921 0.11240699 0.3682327 0.11240699 0.34601921 0.068513334 0.3682327 0.068719804
		 0.34601921 0.11240699 0.3682327 0.068513319 0.3682327 0.11240699 0.34601921 0.11240699
		 0.3682327 0.068513334 0.3682327 0.068719797 0.34601921 0.11240699 0.3682327 0.11240699
		 0.34601921 0.068513334 0.3682327 0.068719804 0.34601921 0.11240699 0.3682327 0.068513319
		 0.3682327 0.13753682 0.41311112 0.13753682 0.43508753 0.092457794 0.43508753 0.092664257
		 0.41311112 0.13753682 0.43508753 0.13753682 0.41311112 0.092457794 0.43508753 0.092664264
		 0.41311112 0.13753682 0.43508753 0.092457779 0.43508753 0.11240699 0.34601921 0.11240699
		 0.3682327 0.068513334 0.3682327 0.068719797 0.34601921 0.11240699 0.3682327 0.11240699
		 0.34601921 0.068513334 0.3682327 0.068719804 0.34601921 0.11240699 0.3682327 0.068513319
		 0.3682327 0.13753682 0.41311112;
	setAttr ".uvst[0].uvsp[1250:1382]" 0.13753682 0.43508753 0.092457794 0.43508753
		 0.092664257 0.41311112 0.13753682 0.43508753 0.13753682 0.41311112 0.092457794 0.43508753
		 0.092664264 0.41311112 0.13753682 0.43508753 0.092457779 0.43508753 0.15958469 0.34601921
		 0.15958469 0.3682327 0.11213493 0.3682327 0.11234139 0.34601921 0.15958469 0.3682327
		 0.15958469 0.34601921 0.11213493 0.3682327 0.1123414 0.34601921 0.15958469 0.3682327
		 0.11213491 0.3682327 0.026465556 0.41311112 0.026465556 0.43508753 -0.00010159612
		 0.43508753 0.00010486692 0.41311112 0.026465556 0.43508753 0.026465556 0.41311112
		 -0.00010159612 0.43508753 0.00010487437 0.41311112 0.026465556 0.43508753 -0.00010161102
		 0.43508753 0.026465556 0.41311112 0.026465556 0.43508753 -0.00010159612 0.43508753
		 0.00010486692 0.41311112 0.026465556 0.43508753 0.026465556 0.41311112 -0.00010159612
		 0.43508753 0.00010487437 0.41311112 0.026465556 0.43508753 -0.00010161102 0.43508753
		 0.026465556 0.43508753 0.026465556 0.41311112 -0.00010159612 0.41311112 0.00010486692
		 0.43508753 0.026465556 0.41311112 0.026465556 0.43508753 -0.00010159612 0.41311112
		 0.00010487437 0.43508753 0.026465556 0.41311112 -0.00010161102 0.41311112 0.37685823
		 0.12653056 0.41328317 0.12653056 0.41328317 0.24911717 0.37685823 0.24911723 0.37685823
		 0.12653056 0.41328317 0.12653056 0.41328317 0.24911717 0.37685823 0.24911723 0.37685823
		 0.12653056 0.41328317 0.12653056 0.41328317 0.24911717 0.37685823 0.24911723 0.49788016
		 0.12653056 0.4161883 0.12653062 0.4161883 0.24911717 0.49788016 0.24911723 0.49788016
		 0.24911723 0.49788016 0.12653056 0.4161883 0.12653062 0.4161883 0.24911717 0.49788016
		 0.24911723 0.49788016 0.12653056 0.4161883 0.12653062 0.4161883 0.24911717 0.41328317
		 0.12653056 0.41328317 0.24911717 0.41328317 0.24911717 0.41328317 0.12653056 0.41328317
		 0.24911717 0.41328317 0.12653056 0.4161883 0.12653062 0.37667233 0.12653056 0.37667233
		 0.24911723 0.4161883 0.24911717 0.4161883 0.24911717 0.4161883 0.12653062 0.37667233
		 0.12653056 0.37667233 0.24911723 0.4161883 0.24911717 0.4161883 0.12653062 0.37667233
		 0.12653056 0.37667233 0.24911723 0.37685823 0.12653056 0.41328317 0.12653056 0.41328317
		 0.24911717 0.37685823 0.24911723 0.37685823 0.12653056 0.41328317 0.12653056 0.41328317
		 0.24911717 0.37685823 0.24911723 0.37685823 0.12653056 0.41328317 0.12653056 0.41328317
		 0.24911717 0.37685823 0.24911723 0.49788016 0.12653056 0.4161883 0.12653062 0.4161883
		 0.24911717 0.49788016 0.24911723 0.49788016 0.24911723 0.49788016 0.12653056 0.4161883
		 0.12653062 0.4161883 0.24911717 0.49788016 0.24911723 0.49788016 0.12653056 0.4161883
		 0.12653062 0.4161883 0.24911717 0.41328317 0.12653056 0.41328317 0.24911717 0.41328317
		 0.24911717 0.41328317 0.12653056 0.41328317 0.24911717 0.41328317 0.12653056 0.4161883
		 0.12653062 0.37667233 0.12653056 0.37667233 0.24911723 0.4161883 0.24911717 0.4161883
		 0.24911717 0.4161883 0.12653062 0.37667233 0.12653056 0.37667233 0.24911723 0.4161883
		 0.24911717 0.4161883 0.12653062 0.37667233 0.12653056 0.37667233 0.24911723;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 904 ".uvst[1].uvsp";
	setAttr ".uvst[1].uvsp[0:249]" -type "float2" 0.0051459111 0.73385459 0.0051459111
		 0.7344901 0.003808219 0.7344901 0.0051459111 0.7365604 0.003808219 0.7365604 0.003808219
		 0.73719591 0.0051459111 0.73719591 0.003808219 0.73385459 0.0038082004 0.73845237
		 0.004970286 0.73845237 0.004970286 0.73900443 0.0038082004 0.73900443 0.004970286
		 0.740803 0.0038082004 0.740803 0.004970286 0.74135506 0.0038082004 0.74135506 0.075633503
		 0.72032368 0.07649108 0.7203179 0.076551557 0.73025012 0.075674668 0.73024553 0.074878335
		 0.72032684 0.074894689 0.73024237 0.074143738 0.72032893 0.074167922 0.73024106 0.071633473
		 0.72032636 0.07159999 0.73024017 0.070873097 0.73024082 0.070898727 0.72032297 0.070092984
		 0.73024338 0.07014332 0.72031862 0.069285445 0.72031182 0.069215789 0.73024726 0.02914688
		 0.62813634 0.031564973 0.62814236 0.031620614 0.6379227 0.029078573 0.63792014 0.028960895
		 0.659877 0.031715084 0.65987504 0.032106873 0.66049659 0.028569091 0.66049945 0.03302861
		 0.62813437 0.033876922 0.62813073 0.03399767 0.63791156 0.03312923 0.6379174 0.026831076
		 0.62811786 0.027680621 0.6281231 0.027569558 0.63791227 0.026700832 0.63790542 0.03236451
		 0.63792062 0.032306001 0.62813318 0.03293927 0.65899932 0.027738806 0.65900218 0.02849802
		 0.65798026 0.032181539 0.65797818 0.028404728 0.62812412 0.028334528 0.63791668 0.029104769
		 0.65712243 0.031575993 0.65712118 0.03383591 0.65909368 0.026841741 0.65909618 0.033170924
		 0.64796484 0.034058906 0.64796948 0.032381076 0.64796162 0.031645097 0.64796036 0.029044665
		 0.64795941 0.02830857 0.64796007 0.027518589 0.64796269 0.02663029 0.64796656 0.046320215
		 0.62813509 0.048736412 0.62814105 0.048792012 0.63791376 0.046251975 0.63791114 0.046134382
		 0.65985078 0.048886407 0.65984881 0.049277887 0.66046989 0.045742884 0.66047275 0.050198901
		 0.62813306 0.051046547 0.62812948 0.05116719 0.63790262 0.050299443 0.6379084 0.04400624
		 0.62811661 0.044855114 0.62812185 0.044744141 0.63790327 0.043876089 0.63789642 0.049535327
		 0.63791162 0.049476866 0.62813187 0.050109629 0.65897375 0.044913255 0.65897667 0.045671865
		 0.65795553 0.049352497 0.65795344 0.045578651 0.62812281 0.045508515 0.63790768 0.046278141
		 0.65709841 0.048747428 0.6570971 0.051005565 0.65906805 0.044016898 0.65907055 0.050341107
		 0.64794797 0.051228397 0.64795262 0.049551882 0.64794475 0.048816472 0.6479435 0.046218079
		 0.64794254 0.045482568 0.6479432 0.044693213 0.64794582 0.043805607 0.6479497 0.067740232
		 0.62814134 0.070165664 0.6281473 0.070221469 0.63795739 0.067671731 0.63795477 0.071633741
		 0.62813932 0.072484627 0.62813568 0.072605737 0.63794619 0.071734674 0.63795203 0.065417409
		 0.62812281 0.066269524 0.62812805 0.066158138 0.6379469 0.065286763 0.63793999 0.07096763
		 0.63795525 0.070908941 0.62813812 0.066995829 0.62812907 0.066925421 0.63795131 0.072667174
		 0.64803457 0.071776487 0.64802992 0.070984252 0.6480267 0.070246026 0.64802545 0.067637712
		 0.6480245 0.066899382 0.64802516 0.066107005 0.64802778 0.065216012 0.64803171 0.084671699
		 0.62813562 0.087088697 0.62814158 0.0871443 0.63791752 0.084603429 0.6379149 0.088551663
		 0.62813365 0.089399591 0.62813002 0.089520283 0.63790637 0.088652246 0.63791221 0.082356945
		 0.62811714 0.083206117 0.62812239 0.083095089 0.63790709 0.082226761 0.63790023 0.087887868
		 0.63791543 0.087829381 0.62813246 0.083929889 0.62812334 0.083859712 0.63791144 0.089581504
		 0.64795971 0.088693917 0.64795506 0.087904431 0.64795184 0.087168783 0.64795059 0.084569529
		 0.64794964 0.083833776 0.64795029 0.083044149 0.64795291 0.082156256 0.64795685 0.05502551
		 0.62813509 0.0574417 0.62814105 0.0574973 0.63791376 0.054957259 0.63791114 0.054839663
		 0.65985078 0.057591692 0.65984881 0.057983175 0.66046989 0.054448172 0.66047275 0.058904182
		 0.62813306 0.059751838 0.62812948 0.059872482 0.63790262 0.059004728 0.6379084 0.05271152
		 0.62811661 0.053560399 0.62812185 0.053449426 0.63790327 0.052581377 0.63789642 0.058240604
		 0.63791162 0.058182143 0.62813187 0.058814924 0.65897375 0.053618535 0.65897667 0.054377157
		 0.65795553 0.058057781 0.65795344 0.054283939 0.62812281 0.054213796 0.63790768 0.05498343
		 0.65709841 0.057452708 0.6570971 0.059710849 0.65906805 0.052722186 0.65907055 0.059046391
		 0.64794797 0.059933681 0.64795262 0.058257166 0.64794475 0.057521757 0.6479435 0.05492337
		 0.64794254 0.05418786 0.6479432 0.053398497 0.64794582 0.052510887 0.6479497 0.07621035
		 0.62814134 0.078635789 0.6281473 0.078691594 0.63795739 0.076141849 0.63795477 0.080103867
		 0.62813932 0.080954738 0.62813568 0.081075862 0.63794619 0.080204785 0.63795203 0.073887527
		 0.62812281 0.07473965 0.62812805 0.074628256 0.6379469 0.073756889 0.63793999 0.079437755
		 0.63795525 0.079379059 0.62813812 0.075465947 0.62812907 0.075395547 0.63795131 0.081137285
		 0.64803457 0.080246598 0.64802992 0.079454362 0.6480267 0.078716151 0.64802545 0.07610783
		 0.6480245 0.075369507 0.64802516 0.07457713 0.64802778 0.07368613 0.64803171 0.093141824
		 0.62813562 0.095558822 0.62814158 0.095614426 0.63791752 0.093073547 0.6379149 0.097021788
		 0.62813365 0.097869717 0.62813002 0.097990401 0.63790637 0.097122379 0.63791221 0.090827078
		 0.62811714 0.091676243 0.62812239 0.091565214 0.63790709 0.090696886 0.63790023 0.096358001
		 0.63791543 0.096299499 0.62813246 0.092399999 0.62812334 0.092329845 0.63791144 0.098051637
		 0.64795971 0.097164035 0.64795506 0.096374542 0.64795184 0.095638901 0.64795059 0.093039662
		 0.64794964 0.092303894 0.64795029 0.091514274 0.64795291 0.090626374 0.64795685 0.084103614
		 0.72032368 0.084961198 0.72031796 0.085021675 0.73025012 0.084144786 0.73024553 0.083348453
		 0.72032684 0.083364815 0.73024237 0.082613856 0.72032893 0.08263804 0.73024112 0.080103591
		 0.72032636 0.080070108 0.73024017 0.079343215 0.73024088 0.079368845 0.72032297 0.078563109
		 0.73024338 0.078613445 0.72031862;
	setAttr ".uvst[1].uvsp[250:499]" 0.07775557 0.72031188 0.0776859 0.73024726
		 0.037616916 0.62813634 0.040035009 0.62814236 0.04009065 0.6379227 0.037548609 0.63792014
		 0.037430927 0.659877 0.040185124 0.65987504 0.040576905 0.66049659 0.037039123 0.66049945
		 0.041498646 0.62813437 0.042346954 0.62813073 0.042467706 0.63791156 0.041599266
		 0.6379174 0.035301119 0.62811786 0.036150657 0.6281231 0.036039598 0.63791227 0.035170868
		 0.63790542 0.040834546 0.63792062 0.040776037 0.62813318 0.041409306 0.65899932 0.036208834
		 0.65900218 0.036968052 0.65798026 0.040651567 0.65797818 0.036874764 0.62812412 0.036804564
		 0.63791668 0.037574802 0.65712243 0.040046029 0.65712118 0.042305946 0.65909368 0.035311781
		 0.65909618 0.04164096 0.64796484 0.04252895 0.64796948 0.040851109 0.64796162 0.040115133
		 0.64796036 0.037514701 0.64795941 0.036778606 0.64796007 0.035988629 0.64796269 0.035100322
		 0.64796656 0.11700605 0.68931544 0.12079179 0.68931544 0.12079179 0.6962648 0.11700605
		 0.6962648 0.11021756 0.63684738 0.10521548 0.63684499 0.10521106 0.62793362 0.11021318
		 0.6279341 0.10098316 0.63684499 0.10097875 0.62793362 0.10098316 0.65597743 0.1052134
		 0.65597743 0.10520901 0.66488498 0.10097875 0.66488498 0.071122564 0.66674602 0.071126908
		 0.65793312 0.075041175 0.65793312 0.075036809 0.66674602 0.1154815 0.65597248 0.11547709
		 0.66488409 0.11035065 0.66488457 0.11035505 0.65597492 0.10524787 0.68965411 0.10524787
		 0.696612 0.099034294 0.696612 0.099034347 0.68965411 0.02754274 0.66994816 0.02663441
		 0.66994816 0.026630294 0.66169089 0.027538665 0.66169089 0.032746412 0.66994816 0.032742292
		 0.66169089 0.037366897 0.66994816 0.037362821 0.66169089 0.11460104 0.68931544 0.11460104
		 0.6962648 0.1078019 0.68931544 0.1078019 0.6962648 0.11548761 0.63684988 0.11548322
		 0.62793458 0.095880516 0.68965411 0.095880516 0.696612 0.092273347 0.696612 0.092273347
		 0.68965411 0.066241831 0.66674602 0.066246174 0.65793312 0.064980723 0.66674602 0.064985104
		 0.65793312 0.06498947 0.64912021 0.066250548 0.64912021 0.071131289 0.64912021 0.075045571
		 0.64912021 0.095880516 0.6826961 0.092273347 0.6826961 0.099034347 0.6826961 0.10524787
		 0.6826961 0.10098756 0.64706987 0.1052178 0.64706987 0.11035945 0.6470654 0.11548591
		 0.64706087 0.11549198 0.64576524 0.11022197 0.64576066 0.10521987 0.64575636 0.10098756
		 0.64575636 0.1078019 0.68236607 0.11460104 0.68236607 0.11700605 0.68236607 0.12079179
		 0.68236607 0.03737098 0.67820537 0.032750465 0.67820537 0.027546883 0.67820537 0.026638493
		 0.67820537 0.042393945 0.71217567 0.042393945 0.70862001 0.049786717 0.70862001 0.049786717
		 0.71217567 0.098462582 0.70284486 0.098460004 0.70754319 0.088979408 0.70754737 0.0889799
		 0.70284891 0.098460004 0.71151859 0.088979408 0.71152276 0.062397875 0.71506232 0.062397875
		 0.71108884 0.071874566 0.71109295 0.071874566 0.71506649 0.046828203 0.72578293 0.037453108
		 0.72577882 0.037453145 0.72210252 0.046828203 0.72210664 0.062392633 0.70144403 0.071873665
		 0.70144814 0.071874112 0.70626342 0.062395219 0.70625925 0.051099226 0.70873362 0.058500923
		 0.70873362 0.058500923 0.71456939 0.051099226 0.71456939 0.068353109 0.72550631 0.068353109
		 0.72635943 0.059569258 0.72636324 0.059569258 0.72551018 0.068353109 0.72061908 0.059569258
		 0.72062296 0.068353109 0.71627957 0.059569258 0.71628338 0.042393945 0.71443456 0.049786717
		 0.71443456 0.042393945 0.72082049 0.049786717 0.72082049 0.098465234 0.69789469 0.088980377
		 0.69789886 0.051099226 0.71753144 0.058500923 0.71753144 0.058500923 0.72091931 0.051099226
		 0.72091931 0.046828203 0.73036695 0.037453145 0.73036283 0.046828203 0.73155135 0.037453145
		 0.73154724 0.088615038 0.68165743 0.08861874 0.67414701 0.089489147 0.67414701 0.089485414
		 0.68165743 0.090922169 0.67414701 0.090918437 0.68165743 0.091479495 0.67414701 0.091475807
		 0.68165743 0.088622473 0.66663665 0.089492857 0.66663659 0.090925865 0.66663659 0.091483228
		 0.66663659 0.11160402 0.67376608 0.11120262 0.67376608 0.11119877 0.66597658 0.11160018
		 0.66597658 0.11269169 0.67376608 0.11268786 0.66597658 0.11304227 0.67376608 0.11303841
		 0.66597658 0.11304612 0.68155551 0.11269559 0.68155551 0.11160794 0.68155551 0.11120649
		 0.68155557 0.12215027 0.66889441 0.11415449 0.66889089 0.11415449 0.66807276 0.12215027
		 0.66807628 0.11415449 0.66672581 0.12215027 0.66672933 0.11415449 0.66620195 0.12215027
		 0.66620541 0.01293549 0.694157 0.013357401 0.71388745 0.0058924705 0.71417099 0.0054472573
		 0.69428885 0.034923617 0.68093657 0.034923617 0.68131387 0.026630297 0.68131745 0.026630297
		 0.68094015 0.034923617 0.67991412 0.026630297 0.67991775 0.034923617 0.67958462 0.026630297
		 0.67958826 0.034923617 0.68540078 0.026630297 0.68540436 0.024960913 0.70691586 0.025077086
		 0.7107904 0.017400071 0.7110768 0.017241124 0.70720917 0.024555419 0.68865514 0.01681155
		 0.68879151 0.024544146 0.68790877 0.016806308 0.68796921 0.016702559 0.68181199 0.024417866
		 0.6816588 0.016525846 0.67565447 0.024256162 0.67541963 0.024232838 0.67459828 0.01649607
		 0.67483205 0.023791987 0.65624022 0.016080059 0.65641278 0.023693256 0.65235877 0.015981007
		 0.65253741 0.11596612 0.71267712 0.11596612 0.71634787 0.10709606 0.71635109 0.10709606
		 0.71268046 0.11596612 0.72001857 0.10709606 0.72002178 0.11596612 0.72368926 0.10709606
		 0.72369254 0.043067843 0.68443847 0.038397476 0.68443847 0.038394213 0.67912143 0.043064736
		 0.67912143 0.047738235 0.68443847 0.047735084 0.67912143 0.05240858 0.68443847 0.052405428
		 0.67912143 0.043827228 0.66973197 0.038399018 0.66973197 0.038394213 0.66156518 0.043822389
		 0.66156518 0.07224682 0.73809731 0.067572951 0.73809731 0.067568816 0.73106545 0.072242662
		 0.73106545 0.10589795 0.73659372 0.10121818 0.73659372 0.10121402 0.72955298 0.10589379
		 0.72955298;
	setAttr ".uvst[1].uvsp[500:749]" 0.043823473 0.66336918 0.038395263 0.66336918
		 0.11203696 0.74058998 0.11204102 0.73372787 0.11789523 0.73372787 0.11789113 0.74058998
		 0.039809912 0.73243624 0.045676433 0.73243624 0.045672461 0.73931283 0.039805882
		 0.73931283 0.054688431 0.67789865 0.049260199 0.67789865 0.049255382 0.66973197 0.054683588
		 0.66973197 0.043832026 0.67789865 0.043827228 0.66973197 0.038403854 0.67789865 0.038399018
		 0.66973197 0.081480317 0.70581424 0.085848026 0.70581424 0.085850999 0.71146107 0.081483297
		 0.71146107 0.092042118 0.73424393 0.09806627 0.73424393 0.098062202 0.74110222 0.092038058
		 0.74110222 0.05265547 0.73896295 0.048044711 0.73896295 0.048040614 0.73202604 0.052651364
		 0.73202604 0.086161166 0.73382717 0.086156033 0.72517961 0.090863839 0.72517961 0.090868928
		 0.73382717 0.095727861 0.72032839 0.095727839 0.72399735 0.086861879 0.72400069 0.086861879
		 0.72033167 0.10601407 0.71997523 0.10601407 0.72364956 0.099096544 0.72365218 0.099096544
		 0.7199778 0.04925172 0.66336918 0.054679878 0.66336918 0.050633825 0.7307722 0.05062867
		 0.72201401 0.055310786 0.72201401 0.055315956 0.7307722 0.02663552 0.73124135 0.026630294
		 0.72249609 0.031305544 0.72249609 0.031310625 0.73124135 0.038402565 0.69317466 0.043072961
		 0.69317466 0.04106858 0.69928211 0.03323058 0.699278 0.03323058 0.69467837 0.04106858
		 0.69468248 0.04106858 0.70388162 0.03323058 0.70387763 0.04106858 0.70848125 0.03323058
		 0.7084772 0.026630294 0.7084738 0.026630353 0.70387423 0.026630294 0.69927448 0.026630353
		 0.69467485 0.11399259 0.70690322 0.11399259 0.71150267 0.10615493 0.71150672 0.10615493
		 0.70690727 0.11399259 0.70230383 0.10615493 0.70230788 0.11399259 0.69770437 0.10615493
		 0.69770843 0.12055212 0.69770098 0.12055216 0.70230037 0.12055212 0.70689982 0.12055212
		 0.71149921 0.061069269 0.70776069 0.053230174 0.70775664 0.053230174 0.70315635 0.061069269
		 0.70316046 0.046628941 0.70775318 0.046628989 0.70315289 0.099096544 0.71630347 0.10601407
		 0.71630085 0.099096492 0.71420103 0.10601407 0.71419853 0.081487149 0.71892357 0.085854843
		 0.71892357 0.073450848 0.69993174 0.08129327 0.69992775 0.08129327 0.70452994 0.073450848
		 0.70453399 0.08785671 0.69992435 0.08785671 0.70452648 0.077119507 0.71892357 0.077115618
		 0.71146107 0.073450848 0.69532967 0.08129327 0.69532561 0.08785671 0.69532216 0.074616447
		 0.71146107 0.072751798 0.71892357 0.072747871 0.71146107 0.011258457 0.62762922 0.0042085499
		 0.64318538 0.003808219 0.62799662 0.011973161 0.65466022 0.004515525 0.65483296 0.013449992
		 0.71810538 0.0060264133 0.71838236 0.099096492 0.71262914 0.10601404 0.71262664 0.013996579
		 0.74535567 0.0062595867 0.73011959 0.095727861 0.71665937 0.086861879 0.71666259
		 0.086861879 0.71456325 0.095727839 0.71299034 0.086861879 0.71299356 0.0065607838
		 0.74528146 0.053230174 0.69855613 0.061069269 0.69856012 0.046628941 0.69855261 0.084423013
		 0.68165743 0.084426723 0.67414701 0.084430434 0.66663659 0.12215027 0.67283463 0.11415449
		 0.67283118 0.025570653 0.73559046 0.017881032 0.73551375 0.11828908 0.68071353 0.12215026
		 0.67677498 0.12215027 0.6807152 0.04994522 0.69395411 0.053230174 0.69395584 0.046628989
		 0.69395238 0.080234662 0.67414701 0.080230966 0.68165743 0.076040819 0.67784309 0.076038949
		 0.68165743 0.080238424 0.66663659 0.080238424 0.66663665 0.076042622 0.67414701 0.076044545
		 0.67028677 0.11415449 0.67677146 0.12215027 0.67677498 0.057213653 0.69395787 0.061069269
		 0.69395983 0.07604637 0.66663659 0.11415449 0.68071175 0.026630297 0.68949127 0.034923617
		 0.6894877 0.026630294 0.68949127 0.030623406 0.69357646 0.026630297 0.69357818 0.084814176
		 0.69072157 0.08785671 0.69071996 0.034923617 0.69357461 0.10685483 0.67376608 0.10685098
		 0.66597658 0.10685868 0.68155551 0.015336875 0.62802303 0.023041449 0.62764311 0.10250704
		 0.67376608 0.098157302 0.66987282 0.098155372 0.66597658 0.10250318 0.66597658 0.10251088
		 0.68155557 0.077612877 0.69072533 0.08129327 0.69072342 0.073450848 0.69072747 0.098160937
		 0.67724156 0.098159216 0.67376608 0.10251088 0.68155557 0.098163098 0.68155557 0.005436074
		 0.6934638 0.012918472 0.69340533 0.0053028315 0.68668503 0.012763478 0.68653691 0.09986566
		 0.64605826 0.099096477 0.62809277 0.099855125 0.62809277 0.0046229474 0.65905195
		 0.012080263 0.65888512 0.012579173 0.67879683 0.0050978102 0.67902285 0.077112712
		 0.70581424 0.074613541 0.70581424 0.07274501 0.70581424 0.012599412 0.67968905 0.0051243454
		 0.67991608 0.032747537 0.73917699 0.032751612 0.73227781 0.038811758 0.73227781 0.038807683
		 0.73917699 0.059734087 0.66137975 0.058628131 0.67529446 0.058636326 0.66137975 0.026634257
		 0.73917919 0.026630294 0.7323938 0.03114032 0.7323938 0.031144354 0.73917919 0.11697786
		 0.72389174 0.12171328 0.72389174 0.1217184 0.73258996 0.11698298 0.73258996 0.10382537
		 0.70443135 0.099574789 0.71096247 0.099567056 0.69786018 0.11651746 0.66377497 0.11650728
		 0.64646351 0.12116876 0.64646351 0.12191003 0.66377497 0.053216711 0.69289756 0.054316197
		 0.67906356 0.060177915 0.67906356 0.060169704 0.69289756 0.10388239 0.71096259 0.10376804
		 0.69786018 0.020583589 0.7418648 0.022119213 0.7418648 0.022119213 0.74259436 0.020583589
		 0.74259436 0.022119213 0.74497098 0.020583589 0.74497098 0.022119213 0.74570048 0.020583589
		 0.74570048 0.019807182 0.74497098 0.019807182 0.74259436 0.0276464 0.74054193 0.029190764
		 0.74054193 0.029190764 0.74127555 0.0276464 0.74127555 0.029190764 0.74366575 0.0276464
		 0.74366575 0.029190764 0.74439943 0.0276464 0.74439943 0.026865564 0.74366575 0.026865564
		 0.74127555 0.034276336 0.74048042 0.035904065 0.74048042 0.035904065 0.74125367 0.034276336
		 0.74125367 0.035904065 0.74377286 0.034276336 0.74377286 0.035904065 0.74454618 0.034276336
		 0.74454618 0.033453375 0.74377286 0.033453375 0.74125367;
	setAttr ".uvst[1].uvsp[750:903]" 0.11422505 0.7418651 0.11576067 0.7418651
		 0.11576067 0.7425946 0.11422505 0.7425946 0.11576067 0.74497128 0.11422505 0.74497128
		 0.11576067 0.74570078 0.11422505 0.74570078 0.11344864 0.74497128 0.11344864 0.7425946
		 0.10306258 0.74530751 0.10439207 0.74530751 0.10439207 0.74593908 0.10306258 0.74593908
		 0.10439207 0.74799663 0.10306258 0.74799663 0.10439207 0.74862826 0.10306258 0.74862826
		 0.10239042 0.74799663 0.10239042 0.74593908 0.091638058 0.74230719 0.093173698 0.74230719
		 0.093173698 0.74303675 0.091638058 0.74303675 0.093173698 0.74541336 0.091638058
		 0.74541336 0.093173698 0.74614292 0.091638058 0.74614292 0.090861663 0.74541336 0.090861663
		 0.74303675 0.09488447 0.74228466 0.09632609 0.74228466 0.09632609 0.74296951 0.09488447
		 0.74296951 0.09632609 0.74520063 0.09488447 0.74520063 0.09632609 0.74588555 0.09488447
		 0.74588555 0.094155595 0.74520063 0.094155595 0.74296951 0.082708023 0.73122644 0.084730037
		 0.73122644 0.084730037 0.73218703 0.082708023 0.73218703 0.084730037 0.73531651 0.082708023
		 0.73531651 0.084730037 0.7362771 0.082708023 0.7362771 0.08168567 0.73531651 0.08168567
		 0.73218703 0.11988242 0.73397893 0.12190449 0.73397893 0.12190449 0.73493958 0.11988242
		 0.73493958 0.12190449 0.73806906 0.11988242 0.73806906 0.12190449 0.73902965 0.11988242
		 0.73902965 0.11886009 0.73806906 0.11886009 0.73493958 0.10811839 0.73459917 0.11014043
		 0.73459917 0.11014043 0.73555982 0.10811836 0.73555982 0.11014043 0.7386893 0.10811836
		 0.7386893 0.11014043 0.73964989 0.10811839 0.73964989 0.10709603 0.7386893 0.10709603
		 0.73555982 0.093685031 0.66570425 0.09384352 0.66327572 0.096982688 0.663275 0.097141117
		 0.66570425 0.062430982 0.69731903 0.062392633 0.69525009 0.065103941 0.69524914 0.065065682
		 0.69731897 0.065960862 0.70055878 0.065921858 0.69849259 0.06863448 0.69849163 0.068595544
		 0.70055872 0.063041918 0.69042057 0.06284634 0.68590397 0.06465032 0.68590474 0.064454727
		 0.69042057 0.067986161 0.69366783 0.066570193 0.69366777 0.066374734 0.68915486 0.068181671
		 0.68915558 0.09619832 0.65742767 0.094623595 0.65742767 0.094349474 0.65198302 0.096478716
		 0.65198088 0.062854625 0.69510669 0.06464196 0.69510752 0.068173081 0.69835085 0.066383228
		 0.69835001 0.096478097 0.66308129 0.09434998 0.66307724 0.062420499 0.68575495 0.06244532
		 0.6835196 0.065051287 0.6835196 0.065076157 0.68575418 0.068606719 0.68900615 0.065949656
		 0.68900692 0.065975063 0.68677431 0.068581268 0.68677431 0.097004443 0.65178776 0.093821794
		 0.65178907 0.093715712 0.64914674 0.097110398 0.64914674 0.060745664 0.67194438 0.060885258
		 0.66980529 0.06365025 0.66980463 0.0637898 0.67194438 0.027840503 0.72158021 0.027806688
		 0.71975559 0.03019781 0.71975476 0.030164063 0.72158015 0.033958338 0.72155362 0.033923931
		 0.71973145 0.036316223 0.71973062 0.03628188 0.72155362 0.028379288 0.71549642 0.028206803
		 0.71151316 0.029797755 0.71151388 0.029625252 0.71549642 0.035744477 0.71547645 0.034495715
		 0.71547645 0.034323331 0.71149647 0.035916876 0.71149707 0.062959388 0.66465425 0.061572358
		 0.66465425 0.061330903 0.65985864 0.06320636 0.65985674 0.028214123 0.71962911 0.029790375
		 0.71962988 0.035909329 0.71960646 0.034330837 0.71960574 0.063205816 0.66963404 0.061331343
		 0.66963047 0.027831256 0.71138179 0.027853146 0.70941043 0.03015136 0.70941037 0.030173298
		 0.71138108 0.036291752 0.71136528 0.033948459 0.711366 0.03397087 0.70939702 0.036269296
		 0.70939702 0.063669428 0.65968663 0.060866114 0.65968776 0.060772691 0.65736037 0.063762747
		 0.65736037;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 628 ".vt";
	setAttr ".vt[0:165]"  2.11065364 -0.24113637 -3.26772118 2.10891867 -0.24113637 -3.78397894
		 2.11065364 -0.024302199 -3.26772118 2.10891867 -0.024302199 -3.78397894 0.84702879 -0.024302199 -3.2634747
		 0.84529388 -0.024302199 -3.77973247 0.84702879 -0.24113637 -3.2634747 0.84529388 -0.24113637 -3.77973247
		 -0.4203254 -0.24113637 -3.26702189 -0.42206025 -0.24113637 -3.78327942 -0.4203254 -0.024302199 -3.26702189
		 -0.42206025 -0.024302199 -3.78327942 -1.26755643 -0.024302199 -3.26417446 -1.26929104 -0.024302199 -3.78043222
		 -1.26755643 -0.24113637 -3.26417446 -1.26929104 -0.24113637 -3.78043222 -11.86590958 5.048445702 -3.4703424
		 -12.16880512 5.048445702 -2.28303003 -12.69050884 5.048445702 -1.81029737 -12.99464226 5.048445702 -0.76760691
		 -14.95771122 5.048445702 -0.76760691 -15.26184464 5.048445702 -1.81029737 -15.78354836 5.048445702 -2.28303003
		 -16.086444855 5.048445702 -3.4703424 -11.86591721 -2.16400933 -3.47020936 -12.1688118 -2.16400933 -2.282897
		 -12.69051647 -2.16400933 -1.81016386 -12.99464989 -2.16400933 -0.76747394 -14.95771885 -2.16400933 -0.76747394
		 -15.26185226 -2.16400933 -1.81016386 -15.78355598 -2.16400933 -2.282897 -16.086450577 -2.16400933 -3.47020936
		 -15.78354168 27.40789604 -2.28316307 -12.16879749 27.40789604 -2.28316307 -16.086437225 27.40789604 -3.47047544
		 -11.86590195 27.40789604 -3.47047544 -12.69050217 26.68632889 -1.75555027 -15.26183701 26.68632889 -1.75555027
		 -12.99463463 26.095710754 -0.82261992 -14.95770359 26.095710754 -0.82261992 -14.95671463 28.17955017 -2.28316307
		 -12.99562359 28.17955017 -2.28316307 -12.83129311 28.17955017 -3.47047544 -15.12104607 28.17955017 -3.47047544
		 -11.86590195 19.50634956 -3.47047544 -12.16879749 19.50634956 -2.28316307 -12.69050217 19.50634956 -1.7829901
		 -12.99463463 19.50634956 -0.7951805 -14.95770359 19.50634956 -0.7951805 -15.26183701 19.50634956 -1.7829901
		 -15.78354168 19.50634956 -2.28316307 -16.086437225 19.50634956 -3.47047544 -11.86590195 12.26089954 -3.47047544
		 -12.16879749 12.26089954 -2.28316307 -12.69050217 12.26089954 -1.81043017 -12.99463463 12.26089954 -0.76774019
		 -14.95770359 12.26089954 -0.76774019 -15.26183701 12.26089954 -1.81043017 -15.78354168 12.26089954 -2.28316307
		 -16.086437225 12.26089954 -3.47047544 -13.35220051 5.56867075 -15.79333496 -9.73745728 5.56867075 -15.79333496
		 -13.35220051 27.89169121 -15.79333496 -9.73745728 27.89169121 -15.79333496 -13.65509605 27.89169121 -16.98064804
		 -9.43456173 27.89169121 -16.98064804 -13.65509605 5.56867075 -16.98064804 -9.43456173 5.56867075 -16.98064804
		 -12.83049679 5.56867075 -15.32060337 -10.259161 5.56867075 -15.32060337 -10.259161 27.17012405 -15.26572227
		 -12.83049679 27.17012405 -15.26572227 -12.52636337 5.56867075 -14.27791214 -10.56329441 5.56867075 -14.27791214
		 -10.56329441 26.57950592 -14.33279324 -12.52636337 26.57950592 -14.33279324 -12.52537441 28.66334534 -15.79333496
		 -10.56428432 28.66334534 -15.79333496 -10.39995193 28.66334534 -16.98064804 -12.68970585 28.66334534 -16.98064804
		 -9.43456173 19.99014664 -16.98064804 -9.73745728 19.99014664 -15.79333496 -10.259161 19.99014664 -15.2931633
		 -10.56329441 19.99014664 -14.30535316 -12.52636337 19.99014664 -14.30535316 -12.83049679 19.99014664 -15.2931633
		 -13.35220051 19.99014664 -15.79333496 -13.65509605 19.99014664 -16.98064804 -9.43456173 12.74469566 -16.98064804
		 -9.73745728 12.74469566 -15.79333496 -10.259161 12.74469566 -15.32060337 -10.56329441 12.74469566 -14.27791214
		 -12.52636337 12.74469566 -14.27791214 -12.83049679 12.74469566 -15.32060337 -13.35220051 12.74469566 -15.79333496
		 -13.65509605 12.74469566 -16.98064804 -15.33516598 -23.30341339 -15.98838139 -7.75449038 -23.30341339 -15.98838139
		 -15.9703846 -23.30341339 -18.47835922 -7.11927414 -23.30341339 -18.47835922 -14.24107265 -23.30341339 -15.054535866
		 -8.84858513 -23.30341339 -15.054535866 -13.60325813 -23.30341339 -12.75276089 -9.4864006 -23.30341339 -12.75276089
		 -8.11316109 -8.83265877 -16.98064804 -8.60572243 -8.83265877 -15.79333496 -9.45410538 -8.83265877 -15.32060337
		 -9.94868088 -8.83265877 -14.27791214 -13.14097786 -8.83265877 -14.27791214 -13.63555336 -8.83265877 -15.32060337
		 -14.48393631 -8.83265877 -15.79333496 -14.97649765 -8.83265877 -16.98064804 -7.038764477 -16.057962418 -16.98064804
		 -7.68553877 -16.057962418 -15.79333496 -8.79953671 -16.057962418 -15.32060337 -9.44895363 -16.057962418 -14.27791214
		 -13.64070415 -16.057962418 -14.27791214 -14.29012299 -16.057962418 -15.32060337 -15.40411949 -16.057962418 -15.79333496
		 -16.050893784 -16.057962418 -16.98064804 -9.43456173 -1.60735416 -16.98064804 -9.73745728 -1.60735416 -15.79333496
		 -10.259161 -1.60735416 -15.32060337 -10.56329441 -1.60735416 -14.27791214 -12.52636337 -1.60735416 -14.27791214
		 -12.83049679 -1.60735416 -15.32060337 -13.35220051 -1.60735416 -15.79333496 -13.65509605 -1.60735416 -16.98064804
		 9.75689411 5.56867075 -15.79333496 13.3716383 5.56867075 -15.79333496 9.75689411 27.89169121 -15.79333496
		 13.3716383 27.89169121 -15.79333496 9.45399857 27.89169121 -16.98064804 13.67453384 27.89169121 -16.98064804
		 9.45399857 5.56867075 -16.98064804 13.67453384 5.56867075 -16.98064804 10.27859879 5.56867075 -15.32060337
		 12.84993362 5.56867075 -15.32060337 12.84993362 27.17012405 -15.26572227 10.27859879 27.17012405 -15.26572227
		 10.5827322 5.56867075 -14.27791214 12.54580116 5.56867075 -14.27791214 12.54580116 26.57950592 -14.33279324
		 10.5827322 26.57950592 -14.33279324 10.58372116 28.66334534 -15.79333496 12.54481125 28.66334534 -15.79333496
		 12.70914268 28.66334534 -16.98064804 10.41938972 28.66334534 -16.98064804 13.67453384 19.99014664 -16.98064804
		 13.3716383 19.99014664 -15.79333496 12.84993362 19.99014664 -15.2931633 12.54580116 19.99014664 -14.30535316
		 10.5827322 19.99014664 -14.30535316 10.27859879 19.99014664 -15.2931633 9.75689411 19.99014664 -15.79333496
		 9.45399857 19.99014664 -16.98064804 13.67453384 12.74469566 -16.98064804 13.3716383 12.74469566 -15.79333496
		 12.84993362 12.74469566 -15.32060337 12.54580116 12.74469566 -14.27791214 10.5827322 12.74469566 -14.27791214
		 10.27859879 12.74469566 -15.32060337 9.75689411 12.74469566 -15.79333496 9.45399857 12.74469566 -16.98064804
		 8.086686134 -23.30341339 -15.95761871 15.041847229 -23.30341339 -15.95761871;
	setAttr ".vt[166:331]" 7.50388145 -23.30341339 -18.24213791 15.62465096 -23.30341339 -18.24213791
		 9.090502739 -23.30341339 -15.10082722 14.038029671 -23.30341339 -15.10082722 9.67568684 -23.30341339 -12.98898315
		 13.45284653 -23.30341339 -12.98898315 15.095463753 -8.83265877 -16.98064804 14.58861732 -8.83265877 -15.79333496
		 13.71562767 -8.83265877 -15.32060337 13.20670891 -8.83265877 -14.27791214 9.92182446 -8.83265877 -14.27791214
		 9.41290569 -8.83265877 -15.32060337 8.53991604 -8.83265877 -15.79333496 8.033067703 -8.83265877 -16.98064804
		 16.27574348 -16.057962418 -16.98064804 15.59948635 -16.057962418 -15.79333496 14.43470573 -16.057962418 -15.32060337
		 13.7556839 -16.057962418 -14.27791214 9.37284946 -16.057962418 -14.27791214 8.69382858 -16.057962418 -15.32060337
		 7.52904701 -16.057962418 -15.79333496 6.8527894 -16.057962418 -16.98064804 13.67453384 -1.60735416 -16.98064804
		 13.3716383 -1.60735416 -15.79333496 12.84993362 -1.60735416 -15.32060337 12.54580116 -1.60735416 -14.27791214
		 10.5827322 -1.60735416 -14.27791214 10.27859879 -1.60735416 -15.32060337 9.75689411 -1.60735416 -15.79333496
		 9.45399857 -1.60735416 -16.98064804 16.1672554 5.048445702 -3.4703424 15.8643589 5.048445702 -2.28303003
		 15.34265518 5.048445702 -1.81029737 15.038521767 5.048445702 -0.76760691 13.075452805 5.048445702 -0.76760691
		 12.77131939 5.048445702 -1.81029737 12.24961567 5.048445702 -2.28303003 11.94672012 5.048445702 -3.4703424
		 16.16724777 -2.16400933 -3.47020936 15.86435223 -2.16400933 -2.282897 15.34264755 -2.16400933 -1.81016386
		 15.038514137 -2.16400933 -0.76747394 13.075445175 -2.16400933 -0.76747394 12.77131271 -2.16400933 -1.81016386
		 12.24960804 -2.16400933 -2.282897 11.94671249 -2.16400933 -3.47020936 12.24962234 27.40789604 -2.28316307
		 15.86436653 27.40789604 -2.28316307 11.94672775 27.40789604 -3.47047544 16.16726112 27.40789604 -3.47047544
		 15.34266281 26.68632889 -1.75555027 12.77132702 26.68632889 -1.75555027 15.038529396 26.095710754 -0.82261992
		 13.075460434 26.095710754 -0.82261992 13.076449394 28.17955017 -2.28316307 15.037540436 28.17955017 -2.28316307
		 15.20187092 28.17955017 -3.47047544 12.91211796 28.17955017 -3.47047544 16.16726112 19.50634956 -3.47047544
		 15.86436653 19.50634956 -2.28316307 15.34266281 19.50634956 -1.7829901 15.038529396 19.50634956 -0.7951805
		 13.075460434 19.50634956 -0.7951805 12.77132702 19.50634956 -1.7829901 12.24962234 19.50634956 -2.28316307
		 11.94672775 19.50634956 -3.47047544 16.16726112 12.26089954 -3.47047544 15.86436653 12.26089954 -2.28316307
		 15.34266281 12.26089954 -1.81043017 15.038529396 12.26089954 -0.76774019 13.075460434 12.26089954 -0.76774019
		 12.77132702 12.26089954 -1.81043017 12.24962234 12.26089954 -2.28316307 11.94672775 12.26089954 -3.47047544
		 0 33.24891281 -13.27000237 0 33.24275208 0 22.62704659 23.10155296 -13.27000332 22.62704659 23.09539032 -1.2111664e-006
		 12.73503017 24.8136425 -13.27000237 10.23295498 26.24891663 -13.27000237 12.73503017 24.80747986 3.9510428e-007
		 10.23295498 26.24275208 -6.1035155e-007 -12.73503017 24.8136425 -13.27000237 -10.23295498 26.24891663 -13.27000237
		 -22.62704659 23.10155296 -13.27000332 -12.73503017 24.80747986 -6.1035155e-007 -10.23295498 26.24275208 -6.1035155e-007
		 -22.62704659 23.09539032 -1.2111664e-006 23.49632263 13.14732933 -8.2559882e-007
		 23.49632263 13.15349102 -13.27000332 24.48309326 16.57951736 -1.2111664e-006 24.48309326 16.58568192 -13.27000332
		 24.56236076 20.27772522 -1.2111664e-006 24.56236076 20.28388977 -13.27000332 19.29162979 23.49897194 -1.0710954e-006
		 19.29162979 23.50513649 -13.27000332 16.99830627 25.69343758 -8.3930792e-007 16.99830627 25.69960022 -13.27000332
		 6.19270992 31.35581589 -3.1996518e-007 6.19270992 31.36197662 -13.27000237 -6.19270992 31.35581589 -3.1996518e-007
		 -6.19270992 31.36197662 -13.27000237 -19.29162979 23.49897194 -1.0710954e-006 -19.29162979 23.50513649 -13.27000332
		 -16.99830627 25.69343758 -8.3930792e-007 -16.99830627 25.69960022 -13.27000332 -24.56236076 20.27772522 -1.2111664e-006
		 -24.56236076 20.28388977 -13.27000332 -24.48309326 16.57951736 -1.2111664e-006 -24.48309326 16.58568192 -13.27000332
		 -23.49632263 13.14732933 -1.1307746e-006 -23.49632263 13.15349102 -13.27000332 -23.49632263 13.1504097 -6.63500261
		 -24.48309326 16.58259964 -6.63500261 -24.56236076 20.2808075 -6.63500261 -22.62704659 23.098472595 -6.63500261
		 -19.29162979 23.50205612 -6.63500261 -16.99830627 25.69651794 -6.63500261 -12.73503017 24.81056213 -6.63500118
		 -10.23295498 26.24583435 -6.63500118 -6.19270992 28.42694283 -6.63500118 0 33.24583435 -6.63500118
		 6.19270992 28.16783142 -6.63500118 10.23295498 26.24583435 -6.63500118 12.73503017 24.81056213 -6.63500118
		 16.99830627 25.69651794 -6.63500261 19.29162979 23.50205612 -6.63500261 22.62704659 23.098472595 -6.63500261
		 24.56236076 20.2808075 -6.63500261 24.48309326 16.58259964 -6.63500261 23.49632263 13.1504097 -6.63500261
		 -23.49632263 13.15614223 -19.99748993 -24.48309326 16.58833122 -19.99748993 -24.56236076 20.28653908 -19.99748993
		 -22.62704659 23.10420418 -19.99748993 -19.29162979 23.5077858 -19.99748993 -16.99830627 25.70225143 -19.99748993
		 -12.73503017 24.81629372 -19.99748802 -10.23295498 26.25156403 -19.99748802 -6.19270992 31.36462975 -19.99748802
		 8.1001827e-017 33.25156403 -19.99748802 6.19270992 31.36462975 -19.99748802 10.23295498 26.25156403 -19.99748802
		 12.73503017 24.81629372 -19.99748802 16.99830627 25.70225143 -19.99748993 19.29162979 23.5077858 -19.99748993
		 22.62704659 23.10420418 -19.99748993 24.56236076 20.28653908 -19.99748993 24.48309326 16.58833122 -19.99748993
		 23.49632263 13.15614223 -19.99748993 -22.45281219 2.15429711 -1.2111664e-006 -22.46675873 2.16510534 -13.27975559
		 -22.45281219 14.36131096 -1.2111664e-006 -22.45281219 14.36747456 -13.27000332 -22.91495323 14.59024048 -1.2111664e-006
		 -22.91495323 14.59640408 -13.27000332 -22.97060394 15.28666878 -1.2111664e-006 -22.97060394 15.29283237 -13.27000332
		 -23.48714447 15.48258018 -1.2111664e-006 -23.48714447 15.48874378 -13.27000332 -22.45281219 10.2923069 -1.2111664e-006
		 -22.45281219 10.29846954 -13.27000332 -22.45281219 6.22330189 -1.2111664e-006 -22.45281219 6.22946405 -13.27000332
		 -22.46504974 2.16129494 -6.64924335 -22.45281219 6.22638321 -6.63500261;
	setAttr ".vt[332:497]" -22.45281219 10.29538822 -6.63500261 -22.45281219 14.36439228 -6.63500261
		 -22.91495323 14.59332275 -6.63500261 -22.97060394 15.28975105 -6.63500261 -23.48714447 15.48566151 -6.63500261
		 0 -4.5776366e-007 -1.4901161e-010 11.25939655 2.15429711 -6.1035155e-007 2.84088993 0.38051459 1.5770107e-016
		 2.84088993 -4.5776366e-007 1.5770107e-016 11.25939655 0.38051459 -6.1035155e-007
		 22.47334671 2.16602898 -13.27614594 22.45281219 2.15429711 -1.2111664e-006 -11.25939655 0.38051459 -6.1035155e-007
		 -11.25939655 2.15429711 -6.1035155e-007 -2.84088993 0.38051459 1.5770107e-016 -2.84088993 -4.5776366e-007 1.5770107e-016
		 22.45281219 14.36131096 -1.2111664e-006 22.45281219 14.36747456 -13.27000332 22.91495323 14.59024048 -1.2111664e-006
		 22.91495323 14.59640408 -13.27000332 22.97060394 15.28666878 -1.2111664e-006 22.97060394 15.29283237 -13.27000332
		 23.48714447 15.48258018 -1.2111664e-006 23.48714447 15.48874378 -13.27000332 22.4556179 6.22330189 0.00021457806
		 22.4542141 6.22946548 -13.2698946 22.4542141 10.2923069 0.00010668344 22.4535141 10.29846954 -13.26994991
		 23.48714447 15.48566151 -6.63500261 22.97060394 15.28975105 -6.63500261 22.91495323 14.59332275 -6.63500261
		 22.45281219 14.36439228 -6.63500261 22.4535141 10.29538822 -6.63494873 22.4542141 6.22638369 -6.63489437
		 22.46512604 2.16152763 -6.64114761 -22.45281219 2.16311049 -19.99748993 -22.45281219 6.23211527 -19.99748993
		 -22.45281219 10.30112076 -19.99748993 -22.45281219 14.37012482 -19.99748993 -22.91495323 14.59905434 -19.99748993
		 -22.97060394 15.29548264 -19.99748993 -23.48714447 15.49139404 -19.99748993 -11.25939655 2.16311049 -19.99748802
		 -11.25939655 0.38932797 -19.99748802 -2.84088993 0.38932797 -19.99748802 -2.84088993 0.008812896 -19.99748802
		 8.1001827e-017 0.008812896 -19.99748802 2.84088993 0.008812896 -19.99748802 2.84088993 0.38932797 -19.99748802
		 11.25939655 0.38932797 -19.99748802 11.25939655 2.16311049 -19.99748802 22.45281219 2.16311049 -19.99748993
		 23.48714447 15.49139404 -19.99748993 22.97060394 15.29548264 -19.99748993 22.91495323 14.59905434 -19.99748993
		 22.45281219 14.37012482 -19.99748993 22.45281219 10.30112076 -19.99748993 22.45281219 6.23211527 -19.99748993
		 -22.50885201 2.16819525 -16.49346352 -11.15404415 2.18283916 -16.48392868 -11.15404034 0.41948962 -16.48400116
		 -2.89093208 0.3443118 -16.52418137 -2.84405422 0.020455413 -16.52417755 -0.0069328928 0.020455413 -16.52417755
		 2.84056497 0.020455418 -16.52417755 2.92119884 0.37995964 -16.52418137 11.26037598 0.41682106 -16.48400307
		 11.26037025 2.18017054 -16.48392868 22.51413536 2.16829109 -16.48598099 -22.47999954 2.15858912 -3.34950066
		 -11.1895752 2.15581536 -3.3078208 -11.18957424 0.38460198 -3.3078208 -2.87997508 0.337798 -3.30782032
		 -2.84461665 0.0015360245 -3.30782032 4.6956645e-017 0.001536028 -3.30782032 2.84050679 0.001536028 -3.30782032
		 2.90361428 0.36564076 -3.30782032 11.26343155 0.38460198 -3.3078208 11.26343155 2.15581536 -3.3078208
		 22.47962189 2.1586206 -3.32656002 16.11775589 -22.19923401 -16.35943222 22.52383041 -22.21986771 -16.4312439
		 3.18222785 -22.19923401 -16.35941696 -3.27638674 -22.19923401 -16.35941696 -22.55785179 -22.21985817 -16.44838715
		 -16.20815849 -22.19923401 -16.35943222 16.12193871 -10.0097990036 -16.35941696 16.11778259 -18.15775299 -16.35977936
		 16.11772728 -14.088749886 -16.35906601 22.51579094 -10.020899773 -16.43141937 22.52378845 -14.081745148 -16.43106842
		 22.52387238 -18.15074921 -16.43141937 3.20976329 -10.0067176819 -16.35943222 3.18224621 -18.15775299 -16.35977936
		 3.18219852 -14.088749886 -16.35908508 -3.23017573 -10.00032043457 -16.35977936 -3.27641606 -14.088749886 -16.35908508
		 -3.2763598 -18.15775299 -16.35978317 -22.52721214 -10.016899109 -16.44838715 -22.55784416 -18.15074158 -16.44856262
		 -22.55786705 -14.081737518 -16.44821167 -16.13331223 -9.99621201 -16.35978317 -16.20818901 -14.088749886 -16.35908508
		 -16.20812988 -18.15775299 -16.35978317 22.51185036 -5.96285534 -16.43211746 22.51174355 -1.89385045 -16.43141937
		 16.13640785 2.18715429 -16.44129181 16.12609291 -1.86184204 -16.35906601 16.12615013 -5.93084717 -16.35977936
		 -3.012280941 -5.92160273 -16.35978317 -2.94944859 -1.85259831 -16.35908508 2.98324609 -1.85567999 -16.35906601
		 3.083349943 -5.92468452 -16.35978317 -16.10466957 2.19443154 -16.44417 -16.09589386 -5.91543961 -16.35978317
		 -16.095945358 -1.84643555 -16.35908508 -22.49658775 -1.88305831 -16.44821167 -22.49655724 -5.95206261 -16.44856262
		 -16.095975876 0.48179328 -16.35868073 16.12606049 0.46638641 -16.35868073 -11.075069427 -1.84883606 -16.35906601
		 -11.07502079 -5.91784048 -16.35978317 -11.11866188 -10.0019960403 -16.35950089 -11.1705761 -14.088749886 -16.35908508
		 -11.17052746 -18.15775299 -16.35977936 -11.17054653 -22.19923401 -16.35942841 11.24964333 -1.85951114 -16.35910034
		 11.24969482 -5.92851543 -16.35978317 11.23836422 -10.005736351 -16.35969543 11.2166748 -14.088749886 -16.35906601
		 11.21673203 -18.15775299 -16.35977936 11.21670437 -22.19923401 -16.35943222 -22.45281219 -22.24308014 -3.39928317
		 22.45386505 -22.24308014 -3.4179709 -22.45281219 -22.24307823 -10.20702553 22.45386505 -22.24307823 -9.67105103
		 -22.45281219 -10.035840034 -3.39928317 -22.45281219 -14.077319145 -3.39928317 -22.45281219 -18.14632416 -3.39928317
		 22.45386505 -10.035840034 -3.4179709 22.4542141 -18.14632416 -3.41794658 22.4535141 -14.077319145 -3.41799521
		 -22.45281219 -18.14632416 -10.20702553 -22.45281219 -14.077319145 -10.20702553 -22.45281219 -10.035840034 -10.20702553
		 22.45386505 -10.035840034 -9.67105103 22.4535141 -14.077319145 -9.6710701 22.4542141 -18.14632416 -9.67101669
		 -22.45281219 -1.9253571 -3.39928317 -22.45281219 -5.9943614 -3.39928317 22.4542141 -5.99436092 -3.41794658
		 22.4535141 -1.92535639 -3.41799521 -22.45281219 -5.9943614 -10.20702553 -22.45281219 -1.92535639 -10.20702553
		 -22.45936966 2.16727209 -10.086527824 22.46150589 2.16741323 -9.59066677 22.4535141 -1.92535639 -9.6710701
		 22.4542141 -5.99436092 -9.67101669 22.4542141 6.22946453 -13.2698946 -22.45281219 6.22946358 -13.27000332
		 -0.27349332 -0.35990715 -16.012609482 -0.27522814 -0.35990715 -16.52886772 -0.27349332 0.01642637 -16.012609482
		 -0.27522814 0.01642637 -16.52886772 -1.5371182 0.01642637 -16.0083637238 -1.53885281 0.01642637 -16.52462196;
	setAttr ".vt[498:627]" -1.5371182 -0.35990715 -16.0083637238 -1.53885281 -0.35990715 -16.52462196
		 0.87550211 -0.42483863 -16.061794281 0.87376726 -0.42483863 -16.57805252 0.87550211 0.012664552 -16.061794281
		 0.87376726 0.012664552 -16.57805252 -0.25899693 0.012664552 -16.057981491 -0.26073176 0.012664552 -16.57423782
		 -0.25899693 -0.42483863 -16.057981491 -0.26073176 -0.42483863 -16.57423782 -1.54851925 -0.42267326 -16.062093735
		 -1.55025411 -0.42267326 -16.57835197 -1.54851925 0.014829936 -16.062093735 -1.55025411 0.014829936 -16.57835197
		 -2.86130834 0.014829936 -16.057682037 -2.86304331 0.014829936 -16.57393837 -2.86130834 -0.42267326 -16.057682037
		 -2.86304331 -0.42267326 -16.57393837 2.15495491 -0.35990715 -16.012609482 2.15321994 -0.35990715 -16.52886772
		 2.15495491 0.01642637 -16.012609482 2.15321994 0.01642637 -16.52886772 0.89133012 0.01642637 -16.0083637238
		 0.88959527 0.01642637 -16.52462196 0.89133012 -0.35990715 -16.0083637238 0.88959527 -0.35990715 -16.52462196
		 2.88160944 -0.42483863 -16.061096191 2.87987447 -0.42483863 -16.57735252 2.88160944 0.012664552 -16.061096191
		 2.87987447 0.012664552 -16.57735252 2.16352272 0.012664552 -16.058679581 2.16178799 0.012664552 -16.57493591
		 2.16352272 -0.42483863 -16.058679581 2.16178799 -0.42483863 -16.57493591 0.40117273 -0.35990715 -15.45705795
		 0.39943787 -0.35990715 -15.97331524 0.40117273 0.01642637 -15.45705795 0.39943787 0.01642637 -15.97331524
		 -0.86245209 0.01642637 -15.45281029 -0.86418694 0.01642637 -15.96906853 -0.86245209 -0.35990715 -15.45281029
		 -0.86418694 -0.35990715 -15.96906853 2.54473734 -0.42267326 -15.52334023 2.54300261 -0.42267326 -16.039598465
		 2.54473734 0.014829936 -15.52334023 2.54300261 0.014829936 -16.039598465 1.61708379 0.014829936 -15.52022171
		 1.61534894 0.014829936 -16.03647995 1.61708379 -0.42267326 -15.52022171 1.61534894 -0.42267326 -16.03647995
		 -0.25522134 -1.84559512 -15.69867229 -0.2567434 -1.84559512 -16.51166534 -0.25522134 -1.21369731 -15.69867229
		 -0.2567434 -1.21369731 -16.51166534 -1.36385942 -1.21369731 -15.69198513 -1.36538136 -1.21369731 -16.50498009
		 -1.36385942 -1.84559512 -15.69198513 -1.36538136 -1.84559512 -16.50498009 -2.99211359 -1.97506809 -15.99746799
		 -2.99207973 -1.9735465 -16.81046295 -2.36037469 -1.98923278 -15.99746799 -2.3603406 -1.98771107 -16.81046295
		 -2.33552361 -0.88087326 -15.99077988 -2.33548927 -0.87935179 -16.80377769 -2.96726227 -0.8667087 -15.99077988
		 -2.96722865 -0.86518723 -16.80377769 3.065659285 -1.80590451 -16.15818787 3.06413722 -1.80591738 -16.97118378
		 3.060250044 -1.17402983 -16.15818787 3.058727741 -1.17404282 -16.97118378 1.95165253 -1.18352032 -16.1515007
		 1.95013058 -1.18353331 -16.96449661 1.95706177 -1.81539488 -16.1515007 1.95554018 -1.81540799 -16.96449661
		 -0.044620667 23.75889206 -2.67970562 -0.044620667 23.75889206 -3.4712379 -0.044620667 22.67481041 -3.4712379
		 -0.044620667 22.67481041 -2.67970562 -10.12856293 23.78648186 -2.67970586 -10.12856197 23.78648186 -3.4712379
		 -10.12856197 22.70240021 -3.4712379 -10.12856293 22.70240021 -2.67970586 9.71599674 23.78648186 -2.6797049
		 9.71599483 23.78648186 -3.4712379 9.71599483 22.70240021 -3.4712379 9.71599674 22.70240021 -2.6797049
		 -12.54810429 22.13485527 -2.52749515 -10.38909626 22.54400826 -2.52749515 -10.38909626 23.94487381 -2.52749515
		 -12.54810429 24.35402679 -2.52749515 -10.38909817 23.94487381 -3.62344837 -12.54810333 24.35402679 -3.6234479
		 -10.38909817 22.54400826 -3.62344837 -12.54810333 22.13485527 -3.6234479 10.08154583 23.96737862 -2.53910351
		 12.54810429 24.26938248 -2.53910303 12.54810429 24.26938248 -3.61183929 10.081547737 23.96737862 -3.61183953
		 10.081547737 22.52150345 -3.61183953 12.54810429 22.21949959 -3.61183929 10.08154583 22.52150345 -2.53910351
		 12.54810429 22.21949959 -2.53910303 -0.034559555 21.78037071 -16.33737755 -0.034559555 21.78037071 -17.12891006
		 -0.034559555 20.69628906 -17.12891006 -0.034559555 20.69628906 -16.33737755 -7.8447628 21.80796051 -16.33737755
		 -7.84476137 21.80796051 -17.12890816 -7.84476137 20.72387886 -17.12890816 -7.8447628 20.72387886 -16.33737755
		 7.52522087 21.80796051 -16.33737755 7.52522087 21.80796051 -17.12890816 7.52522087 20.72387886 -17.12890816
		 7.52522087 20.72387886 -16.33737755 -9.71874237 20.15633583 -16.18516731 -8.046550751 20.56548882 -16.18516731
		 -8.046550751 21.96635246 -16.18516731 -9.71874237 22.37550545 -16.18516731 -8.046550751 21.96635246 -17.2811203
		 -9.71874237 22.37550545 -17.2811203 -8.046550751 20.56548882 -17.2811203 -9.71874237 20.15633583 -17.2811203
		 7.80834723 21.98885727 -16.19677544 9.71874237 22.29086113 -16.19677544 9.71874237 22.29086113 -17.26951218
		 7.8083477 21.98885727 -17.26951218 7.8083477 20.54298401 -17.26951218 9.71874237 20.24097824 -17.26951218
		 7.80834723 20.54298401 -16.19677544 9.71874237 20.24097824 -16.19677544;
	setAttr -s 1065 ".ed";
	setAttr ".ed[0:165]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0 3 5 0 4 6 0
		 5 7 0 8 9 0 10 11 0 12 13 0 14 15 0 8 10 0 9 11 0 10 12 0 11 13 0 12 14 0 13 15 0
		 16 24 0 16 17 0 17 18 0 18 19 0 19 20 0 20 21 0 21 22 0 23 31 0 22 23 0 25 17 0 24 25 0
		 26 18 0 25 26 0 27 19 0 26 27 0 28 20 0 27 28 0 29 21 0 28 29 0 30 22 0 29 30 0 30 31 0
		 32 33 0 22 58 0 17 53 0 32 34 0 33 35 0 34 51 0 35 44 0 33 36 0 18 54 0 32 37 0 37 36 0
		 21 57 0 36 38 0 19 55 0 37 39 0 39 38 0 20 56 0 32 40 0 33 41 0 40 41 0 35 42 0 41 42 0
		 34 43 0 43 42 0 40 43 0 44 52 0 45 33 0 44 45 1 46 36 0 45 46 1 47 38 0 46 47 1 48 39 0
		 47 48 1 49 37 0 48 49 1 50 32 0 49 50 1 51 59 0 50 51 1 52 16 0 53 45 0 52 53 1 54 46 0
		 53 54 1 55 47 0 54 55 1 56 48 0 55 56 1 57 49 0 56 57 1 58 50 0 57 58 1 59 23 0 58 59 1
		 62 63 0 60 94 0 61 89 0 62 64 0 63 65 0 64 87 0 65 80 0 66 60 0 67 61 0 60 68 0 61 69 0
		 63 70 0 69 90 0 62 71 0 71 70 0 68 93 0 68 72 0 69 73 0 72 73 0 70 74 0 73 91 0 71 75 0
		 75 74 0 72 92 0 62 76 0 63 77 0 76 77 0 65 78 0 77 78 0 64 79 0 79 78 0 76 79 0 80 88 0
		 81 63 0 80 81 1 82 70 0 81 82 1 83 74 0 82 83 1 84 75 0 83 84 1 85 71 0 84 85 1 86 62 0
		 85 86 1 87 95 0 86 87 1 88 67 0 89 81 0 88 89 1 90 82 0 89 90 1 91 83 0 90 91 1 92 84 0
		 91 92 1 93 85 0 92 93 1 94 86 0 93 94 1 95 66 0 94 95 1 96 118 0 97 113 0 98 96 0
		 99 97 0 96 100 0 97 101 0 101 114 0;
	setAttr ".ed[166:331]" 100 117 0 100 102 0 101 103 0 102 103 0 103 115 0 102 116 0
		 104 112 0 104 105 0 105 106 0 106 107 0 107 108 0 108 109 0 109 110 0 111 119 0 110 111 0
		 112 99 0 113 105 0 112 113 1 114 106 0 113 114 1 115 107 0 114 115 1 116 108 0 115 116 1
		 117 109 0 116 117 1 118 110 0 117 118 1 119 98 0 118 119 1 110 126 0 105 121 0 106 122 0
		 109 125 0 107 123 0 108 124 0 67 120 0 66 127 0 120 104 0 121 61 0 120 121 1 122 69 0
		 121 122 1 123 73 0 122 123 1 124 72 0 123 124 1 125 68 0 124 125 1 126 60 0 125 126 1
		 127 111 0 126 127 1 130 131 0 128 162 0 129 157 0 130 132 0 131 133 0 132 155 0 133 148 0
		 134 128 0 135 129 0 128 136 0 129 137 0 131 138 0 137 158 0 130 139 0 139 138 0 136 161 0
		 136 140 0 137 141 0 140 141 0 138 142 0 141 159 0 139 143 0 143 142 0 140 160 0 130 144 0
		 131 145 0 144 145 0 133 146 0 145 146 0 132 147 0 147 146 0 144 147 0 148 156 0 149 131 0
		 148 149 1 150 138 0 149 150 1 151 142 0 150 151 1 152 143 0 151 152 1 153 139 0 152 153 1
		 154 130 0 153 154 1 155 163 0 154 155 1 156 135 0 157 149 0 156 157 1 158 150 0 157 158 1
		 159 151 0 158 159 1 160 152 0 159 160 1 161 153 0 160 161 1 162 154 0 161 162 1 163 134 0
		 162 163 1 164 186 0 165 181 0 166 164 0 167 165 0 164 168 0 165 169 0 169 182 0 168 185 0
		 168 170 0 169 171 0 170 171 0 171 183 0 170 184 0 172 180 0 172 173 0 173 174 0 174 175 0
		 175 176 0 176 177 0 177 178 0 179 187 0 178 179 0 180 167 0 181 173 0 180 181 1 182 174 0
		 181 182 1 183 175 0 182 183 1 184 176 0 183 184 1 185 177 0 184 185 1 186 178 0 185 186 1
		 187 166 0 186 187 1 178 194 0 173 189 0 174 190 0 177 193 0 175 191 0 176 192 0 135 188 0
		 134 195 0 188 172 0 189 129 0 188 189 1 190 137 0 189 190 1 191 141 0;
	setAttr ".ed[332:497]" 190 191 1 192 140 0 191 192 1 193 136 0 192 193 1 194 128 0
		 193 194 1 195 179 0 194 195 1 196 204 0 196 197 0 197 198 0 198 199 0 199 200 0 200 201 0
		 201 202 0 203 211 0 202 203 0 205 197 0 204 205 0 206 198 0 205 206 0 207 199 0 206 207 0
		 208 200 0 207 208 0 209 201 0 208 209 0 210 202 0 209 210 0 210 211 0 212 213 0 202 238 0
		 197 233 0 212 214 0 213 215 0 214 231 0 215 224 0 213 216 0 198 234 0 212 217 0 217 216 0
		 201 237 0 216 218 0 199 235 0 217 219 0 219 218 0 200 236 0 212 220 0 213 221 0 220 221 0
		 215 222 0 221 222 0 214 223 0 223 222 0 220 223 0 224 232 0 225 213 0 224 225 1 226 216 0
		 225 226 1 227 218 0 226 227 1 228 219 0 227 228 1 229 217 0 228 229 1 230 212 0 229 230 1
		 231 239 0 230 231 1 232 196 0 233 225 0 232 233 1 234 226 0 233 234 1 235 227 0 234 235 1
		 236 228 0 235 236 1 237 229 0 236 237 1 238 230 0 237 238 1 239 203 0 238 239 1 240 287 1
		 242 293 1 243 258 0 242 261 0 245 265 0 244 245 0 246 262 0 244 290 1 247 289 1 247 246 0
		 249 248 0 240 267 0 250 273 0 248 271 0 251 252 0 248 284 1 252 285 1 252 266 0 253 281 1
		 253 268 0 241 264 0 255 257 0 254 296 0 256 254 0 257 259 0 256 295 1 258 256 0 259 242 0
		 258 294 1 260 243 0 261 263 0 260 292 1 262 260 0 263 244 0 262 291 1 264 247 0 265 240 0
		 264 288 1 266 241 0 267 249 0 266 286 1 268 270 0 269 250 0 268 282 1 270 251 0 271 269 0
		 270 283 1 272 253 0 273 275 0 272 280 1 274 272 0 275 277 0 274 279 1 276 274 0 276 278 0
		 278 277 0 279 275 1 278 279 1 280 273 1 279 280 1 281 250 1 280 281 1 282 269 1 281 282 1
		 283 271 1 282 283 1 284 251 1 283 284 1 285 249 1 284 285 1 286 267 1 285 286 1 287 241 1
		 286 287 1 288 265 1 287 288 1 289 245 1 288 289 1 290 246 1 289 290 1;
	setAttr ".ed[498:663]" 291 263 1 290 291 1 292 261 1 291 292 1 293 243 1 292 293 1
		 294 259 1 293 294 1 295 257 1 294 295 1 296 255 0 295 296 1 245 308 1 249 304 1 250 300 1
		 255 315 0 257 314 1 259 313 1 261 311 1 263 310 1 265 307 1 267 305 1 269 301 1 271 302 1
		 273 299 1 275 298 1 277 297 0 297 298 0 298 299 0 299 300 0 300 301 0 301 302 0 303 248 1
		 302 303 0 303 304 0 304 305 0 306 240 1 305 306 0 306 307 0 307 308 0 309 244 1 308 309 0
		 309 310 0 310 311 0 312 242 1 311 312 0 312 313 0 313 314 0 314 315 0 316 328 0 318 320 0
		 318 333 1 320 322 0 321 319 0 320 334 1 322 324 0 323 321 0 322 335 1 325 323 0 324 336 0
		 326 318 0 328 326 0 333 319 1 334 321 1 333 334 1 335 323 1 334 335 1 336 325 0 335 336 1
		 337 347 0 338 341 0 337 406 0 339 340 0 340 407 1 340 337 0 341 409 1 341 339 0 343 411 0
		 343 338 0 344 345 0 345 402 1 345 316 0 346 344 0 347 405 1 347 346 0 348 358 0 349 351 0
		 348 363 1 350 348 0 351 353 0 350 362 1 352 350 0 353 355 0 352 361 1 354 352 0 354 360 0
		 356 343 0 358 356 0 359 349 0 360 355 0 361 353 1 360 361 1 362 351 1 361 362 1 363 349 1
		 362 363 1 319 370 1 321 371 1 323 372 1 325 373 0 367 368 0 368 369 0 369 370 0 370 371 0
		 371 372 0 372 373 0 367 374 0 375 392 1 374 375 0 376 393 1 375 376 0 376 377 0 377 378 0
		 378 379 0 379 380 0 380 381 0 381 382 0 382 383 0 349 387 1 351 386 1 353 385 1 355 384 0
		 359 388 1 384 385 0 385 386 0 386 387 0 387 388 0 388 389 0 389 383 0 392 393 0 394 395 0
		 395 396 0 401 316 0 401 402 0 403 344 1 402 403 0 404 346 1 403 404 0 404 405 0 405 406 0
		 406 407 0 408 339 1 407 408 0 408 409 0 410 338 1 409 410 0 410 411 0 413 412 0 412 463 0
		 414 415 0 415 457 0 417 416 0 412 419 0 419 420 0 420 418 0 421 418 0;
	setAttr ".ed[664:829]" 422 420 1 421 422 0 423 419 1 422 423 0 423 413 0 414 425 0
		 418 460 0 425 426 0 419 462 1 426 424 0 420 461 1 424 427 0 425 429 1 426 428 1 427 454 0
		 427 428 1 428 429 1 429 415 1 416 431 0 431 432 0 429 456 1 432 430 0 428 455 1 433 430 0
		 434 432 1 433 434 1 435 431 1 434 435 1 435 417 1 400 438 0 440 459 1 440 418 1 427 441 0
		 444 441 1 444 424 1 433 446 0 441 453 1 446 449 1 449 430 0 453 446 1 454 433 0 453 454 1
		 455 434 1 454 455 1 456 435 1 455 456 1 457 417 0 456 457 1 459 444 1 460 424 0 459 460 1
		 461 426 1 460 461 1 462 425 1 461 462 1 463 414 0 462 463 1 464 466 0 466 416 0 465 467 0
		 467 413 0 468 469 0 469 470 0 470 464 0 465 472 0 472 473 0 473 471 0 468 476 0 469 475 1
		 470 474 1 474 431 1 466 474 1 475 432 1 474 475 1 476 430 0 475 476 1 471 477 0 472 479 1
		 473 478 1 477 421 0 478 422 1 477 478 1 479 423 1 478 479 1 479 467 1 401 480 0 480 481 0
		 481 468 0 471 482 0 482 483 0 483 411 0 481 484 1 484 449 1 476 484 1 439 440 1 439 451 1
		 446 447 0 447 450 0 445 391 0 452 453 1 447 452 1 450 392 1 392 452 1 438 399 0 421 436 0
		 436 440 1 482 489 1 477 489 1 411 366 0 436 489 1 437 439 1 436 437 0 483 488 1 488 489 1
		 437 488 1 438 451 1 400 451 0 382 399 1 400 437 0 383 400 0 383 438 0 374 391 1 391 392 0
		 445 450 0 367 445 0 448 449 0 447 448 1 390 448 0 390 450 0 367 390 0 390 445 0 317 486 0
		 484 485 1 448 485 1 480 485 1 486 330 0 326 332 1 332 333 1 327 332 1 319 327 0 327 369 1
		 390 491 0 317 390 0 317 485 0 328 331 1 331 332 1 329 331 1 317 491 0 327 329 0 329 368 1
		 330 401 0 330 485 0 485 486 1 331 401 0 330 331 1 329 491 0 491 486 0 357 490 0 357 359 0
		 342 488 0 342 490 0 342 400 0 357 389 1 400 490 0 363 364 1 359 364 1;
	setAttr ".ed[830:995]" 356 365 1 365 366 1 358 364 1 364 365 1 357 365 1 366 487 0
		 487 488 1 366 488 0 365 487 0 342 487 0 365 411 0 393 394 0 377 394 1 378 395 0 392 394 0
		 380 397 1 397 398 0 439 458 1 458 459 1 398 458 1 381 398 1 398 399 0 398 451 1 379 396 1
		 396 397 0 443 444 1 396 398 0 441 442 0 442 452 1 442 443 1 396 443 1 443 458 1 442 395 0
		 394 442 0 443 395 0 492 493 0 494 495 0 496 497 0 498 499 0 492 494 0 493 495 0 494 496 0
		 495 497 0 496 498 0 497 499 0 498 492 0 500 501 0 502 503 0 504 505 0 506 507 0 500 502 0
		 501 503 0 502 504 0 503 505 0 504 506 0 505 507 0 506 500 0 508 509 0 510 511 0 512 513 0
		 514 515 0 508 510 0 509 511 0 510 512 0 511 513 0 512 514 0 513 515 0 514 508 0 516 517 0
		 518 519 0 520 521 0 522 523 0 516 518 0 517 519 0 518 520 0 519 521 0 520 522 0 521 523 0
		 522 516 0 524 525 0 526 527 0 528 529 0 530 531 0 524 526 0 525 527 0 526 528 0 527 529 0
		 528 530 0 529 531 0 530 524 0 532 533 0 534 535 0 536 537 0 538 539 0 532 534 0 533 535 0
		 534 536 0 535 537 0 536 538 0 537 539 0 538 532 0 540 541 0 542 543 0 544 545 0 546 547 0
		 540 542 0 541 543 0 542 544 0 543 545 0 544 546 0 545 547 0 546 540 0 548 549 0 550 551 0
		 552 553 0 554 555 0 548 550 0 549 551 0 550 552 0 551 553 0 552 554 0 553 555 0 554 548 0
		 556 557 0 558 559 0 560 561 0 562 563 0 556 558 0 557 559 0 558 560 0 559 561 0 560 562 0
		 561 563 0 562 556 0 564 565 0 566 567 0 568 569 0 570 571 0 564 566 0 565 567 0 566 568 0
		 567 569 0 568 570 0 569 571 0 570 564 0 572 580 0 573 581 0 572 573 1 574 582 0 575 583 0
		 574 575 1 575 572 1 576 572 0 577 573 0 576 577 0 578 574 0 579 575 0 578 579 0 579 576 0
		 580 581 0 582 583 0 583 580 0 579 585 1 584 585 0 576 586 1 585 586 0;
	setAttr ".ed[996:1064]" 587 586 0 584 587 0 577 588 0 586 588 0 589 588 0 587 589 0
		 578 590 0 591 590 0 590 585 0 591 584 0 580 592 0 592 593 0 593 594 0 581 595 0 595 594 0
		 592 595 0 582 596 0 596 597 0 583 598 0 596 598 0 597 599 0 598 599 0 598 592 0 599 593 0
		 600 608 0 601 609 0 600 601 1 602 610 0 603 611 0 602 603 1 603 600 1 604 600 0 605 601 0
		 604 605 0 606 602 0 607 603 0 606 607 0 607 604 0 608 609 0 610 611 0 611 608 0 607 613 1
		 612 613 0 604 614 1 613 614 0 615 614 0 612 615 0 605 616 0 614 616 0 617 616 0 615 617 0
		 606 618 0 619 618 0 618 613 0 619 612 0 608 620 0 620 621 0 621 622 0 609 623 0 623 622 0
		 620 623 0 610 624 0 624 625 0 611 626 0 624 626 0 625 627 0 626 627 0 626 620 0 627 621 0;
	setAttr -s 456 ".fc[0:455]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		mu 1 4 7 0 1 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		mu 1 4 2 1 3 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		mu 1 4 4 3 6 5
		f 4 10 15 -12 -15
		mu 0 4 8 9 10 11
		mu 1 4 8 9 10 11
		f 4 11 17 -13 -17
		mu 0 4 11 10 12 13
		mu 1 4 11 10 12 13
		f 4 12 19 -14 -19
		mu 0 4 13 12 14 15
		mu 1 4 13 12 14 15
		f 4 -31 -21 21 -30
		mu 0 4 16 17 18 19
		mu 1 4 16 17 18 19
		f 4 -33 29 22 -32
		mu 0 4 20 16 19 21
		mu 1 4 20 16 19 21
		f 4 -35 31 23 -34
		mu 0 4 22 23 24 25
		mu 1 4 22 20 21 23
		f 4 -37 33 24 -36
		mu 0 4 26 27 28 29
		mu 1 4 24 22 23 25
		f 4 -38 -39 35 25
		mu 0 4 30 31 32 33
		mu 1 4 26 27 24 25
		f 4 -40 -41 37 26
		mu 0 4 34 35 36 37
		mu 1 4 28 29 27 26
		f 4 -42 39 28 27
		mu 0 4 38 35 34 39
		mu 1 4 30 29 28 31
		f 4 -25 55 90 -59
		mu 0 4 40 41 42 43
		mu 1 4 32 33 34 35
		f 4 61 63 -66 -67
		mu 0 4 44 45 46 47
		mu 1 4 36 37 38 39
		f 4 -22 -83 84 -45
		mu 0 4 48 49 50 51
		mu 1 4 40 41 42 43
		f 4 -29 43 96 95
		mu 0 4 52 53 54 55
		mu 1 4 44 45 46 47
		f 4 44 86 -51 -23
		mu 0 4 48 51 56 57
		mu 1 4 40 43 48 49
		f 4 -43 51 52 -50
		mu 0 4 58 59 60 61
		mu 1 4 50 51 52 53
		f 4 -44 -27 53 94
		mu 0 4 54 53 62 63
		mu 1 4 46 45 54 55
		f 4 50 88 -56 -24
		mu 0 4 64 65 66 67
		mu 1 4 49 48 34 33
		f 4 -53 56 57 -55
		mu 0 4 68 69 70 71
		mu 1 4 53 52 56 57
		f 4 -54 -26 58 92
		mu 0 4 72 73 74 75
		mu 1 4 55 54 32 35
		f 4 42 60 -62 -60
		mu 0 4 59 58 76 77
		mu 1 4 51 50 37 36
		f 4 46 62 -64 -61
		mu 0 4 78 79 80 81
		mu 1 4 50 58 38 37
		f 4 -46 59 66 -65
		mu 0 4 82 83 84 85
		mu 1 4 59 51 36 39
		f 4 -70 -49 -47 -69
		mu 0 4 86 87 88 89
		mu 1 4 60 61 58 50
		f 4 -72 68 49 -71
		mu 0 4 90 86 89 91
		mu 1 4 62 60 50 53
		f 4 -74 70 54 -73
		mu 0 4 92 93 94 95
		mu 1 4 63 62 53 57
		f 4 -76 72 -58 -75
		mu 0 4 96 97 98 99
		mu 1 4 64 63 57 56
		f 4 -77 -78 74 -57
		mu 0 4 100 101 102 103
		mu 1 4 52 65 64 56
		f 4 -79 -80 76 -52
		mu 0 4 104 105 106 107
		mu 1 4 51 66 65 52
		f 4 -82 78 45 47
		mu 0 4 108 105 104 109
		mu 1 4 67 66 51 59
		f 4 -85 -68 69 -84
		mu 0 4 110 111 112 113
		mu 1 4 43 42 61 60
		f 4 -87 83 71 -86
		mu 0 4 114 110 113 115
		mu 1 4 48 43 60 62
		f 4 -89 85 73 -88
		mu 0 4 116 117 118 119
		mu 1 4 34 48 62 63
		f 4 -91 87 75 -90
		mu 0 4 120 121 122 123
		mu 1 4 35 34 63 64
		f 4 -92 -93 89 77
		mu 0 4 124 125 126 127
		mu 1 4 65 55 35 64
		f 4 -94 -95 91 79
		mu 0 4 128 129 130 131
		mu 1 4 66 46 55 65
		f 4 -97 93 81 80
		mu 0 4 132 129 128 133
		mu 1 4 47 46 66 67
		f 4 115 117 152 -121
		mu 0 4 134 135 136 137
		mu 1 4 68 69 70 71
		f 4 123 125 -128 -129
		mu 0 4 138 139 140 141
		mu 1 4 72 73 74 75
		f 4 -106 -145 146 -100
		mu 0 4 142 143 144 145
		mu 1 4 76 77 78 79
		f 4 104 98 158 157
		mu 0 4 146 147 148 149
		mu 1 4 80 81 82 83
		f 4 99 148 -110 -108
		mu 0 4 142 145 150 151
		mu 1 4 76 79 84 85
		f 4 -98 110 111 -109
		mu 0 4 152 153 154 155
		mu 1 4 86 87 88 89
		f 4 -99 106 112 156
		mu 0 4 148 147 156 157
		mu 1 4 82 81 90 91
		f 4 109 150 -118 -115
		mu 0 4 158 159 160 161
		mu 1 4 85 84 70 69
		f 4 -112 118 119 -117
		mu 0 4 162 163 164 165
		mu 1 4 89 88 92 93
		f 4 -113 113 120 154
		mu 0 4 166 167 168 169
		mu 1 4 91 90 68 71
		f 4 97 122 -124 -122
		mu 0 4 153 152 170 171
		mu 1 4 87 86 73 72
		f 4 101 124 -126 -123
		mu 0 4 172 173 174 175
		mu 1 4 86 94 74 73
		f 4 -101 121 128 -127
		mu 0 4 176 177 178 179
		mu 1 4 95 87 72 75
		f 4 -132 -104 -102 -131
		mu 0 4 180 181 182 183
		mu 1 4 96 97 94 86
		f 4 -134 130 108 -133
		mu 0 4 184 180 183 185
		mu 1 4 98 96 86 89
		f 4 -136 132 116 -135
		mu 0 4 186 187 188 189
		mu 1 4 99 98 89 93
		f 4 -138 134 -120 -137
		mu 0 4 190 191 192 193
		mu 1 4 100 99 93 92
		f 4 -139 -140 136 -119
		mu 0 4 194 195 196 197
		mu 1 4 88 101 100 92
		f 4 -141 -142 138 -111
		mu 0 4 198 199 200 201
		mu 1 4 87 102 101 88
		f 4 -144 140 100 102
		mu 0 4 202 199 198 203
		mu 1 4 103 102 87 95
		f 4 -147 -130 131 -146
		mu 0 4 204 205 206 207
		mu 1 4 79 78 97 96
		f 4 -149 145 133 -148
		mu 0 4 208 204 207 209
		mu 1 4 84 79 96 98
		f 4 -151 147 135 -150
		mu 0 4 210 211 212 213
		mu 1 4 70 84 98 99
		f 4 -153 149 137 -152
		mu 0 4 214 215 216 217
		mu 1 4 71 70 99 100
		f 4 -154 -155 151 139
		mu 0 4 218 219 220 221
		mu 1 4 101 91 71 100
		f 4 -156 -157 153 141
		mu 0 4 222 223 224 225
		mu 1 4 102 82 91 101
		f 4 -159 155 143 142
		mu 0 4 226 223 222 227
		mu 1 4 83 82 102 103
		f 4 169 170 189 -172
		mu 0 4 228 229 230 231
		mu 1 4 104 105 106 107
		f 4 -163 -182 183 -161
		mu 0 4 232 233 234 235
		mu 1 4 108 109 110 111
		f 4 161 159 195 194
		mu 0 4 236 237 238 239
		mu 1 4 112 113 114 115
		f 4 160 185 -166 -165
		mu 0 4 232 235 240 241
		mu 1 4 108 111 116 117
		f 4 -160 163 166 193
		mu 0 4 238 237 242 243
		mu 1 4 114 113 118 119
		f 4 165 187 -171 -169
		mu 0 4 244 245 246 247
		mu 1 4 117 116 106 105
		f 4 -167 167 171 191
		mu 0 4 248 249 250 251
		mu 1 4 119 118 104 107
		f 4 -184 -173 173 -183
		mu 0 4 252 253 254 255
		mu 1 4 111 110 120 121
		f 4 -186 182 174 -185
		mu 0 4 256 252 255 257
		mu 1 4 116 111 121 122
		f 4 -188 184 175 -187
		mu 0 4 258 259 260 261
		mu 1 4 106 116 122 123
		f 4 -190 186 176 -189
		mu 0 4 262 263 264 265
		mu 1 4 107 106 123 124
		f 4 -191 -192 188 177
		mu 0 4 266 267 268 269
		mu 1 4 125 119 107 124
		f 4 -193 -194 190 178
		mu 0 4 270 271 272 273
		mu 1 4 126 114 119 125
		f 4 -196 192 180 179
		mu 0 4 274 271 270 275
		mu 1 4 115 114 126 127
		f 4 -177 200 212 -202
		mu 0 4 276 277 278 279
		mu 1 4 128 129 130 131
		f 4 -174 -205 206 -198
		mu 0 4 280 281 282 283
		mu 1 4 132 133 134 135
		f 4 -181 196 218 217
		mu 0 4 284 285 286 287
		mu 1 4 136 137 138 139
		f 4 197 208 -199 -175
		mu 0 4 280 283 288 289
		mu 1 4 132 135 140 141
		f 4 -197 -179 199 216
		mu 0 4 286 285 290 291
		mu 1 4 138 137 142 143
		f 4 198 210 -201 -176
		mu 0 4 292 293 294 295
		mu 1 4 141 140 130 129
		f 4 -200 -178 201 214
		mu 0 4 296 297 298 299
		mu 1 4 143 142 128 131
		f 4 -207 -203 105 -206
		mu 0 4 300 301 302 303
		mu 1 4 135 134 144 145
		f 4 -209 205 107 -208
		mu 0 4 304 300 303 305
		mu 1 4 140 135 145 146
		f 4 -211 207 114 -210
		mu 0 4 306 307 308 309
		mu 1 4 130 140 146 147
		f 4 -213 209 -116 -212
		mu 0 4 310 311 312 313
		mu 1 4 131 130 147 148
		f 4 -214 -215 211 -114
		mu 0 4 314 315 316 317
		mu 1 4 149 143 131 148
		f 4 -216 -217 213 -107
		mu 0 4 318 319 320 321
		mu 1 4 150 138 143 149
		f 4 -219 215 -105 203
		mu 0 4 322 319 318 323
		mu 1 4 139 138 150 151
		f 4 237 239 274 -243
		mu 0 4 324 325 326 327
		mu 1 4 152 153 154 155
		f 4 245 247 -250 -251
		mu 0 4 328 329 330 331
		mu 1 4 156 157 158 159
		f 4 -228 -267 268 -222
		mu 0 4 332 333 334 335
		mu 1 4 160 161 162 163
		f 4 226 220 280 279
		mu 0 4 336 337 338 339
		mu 1 4 164 165 166 167
		f 4 221 270 -232 -230
		mu 0 4 332 335 340 341
		mu 1 4 160 163 168 169
		f 4 -220 232 233 -231
		mu 0 4 342 343 344 345
		mu 1 4 170 171 172 173
		f 4 -221 228 234 278
		mu 0 4 338 337 346 347
		mu 1 4 166 165 174 175
		f 4 231 272 -240 -237
		mu 0 4 348 349 350 351
		mu 1 4 169 168 154 153
		f 4 -234 240 241 -239
		mu 0 4 352 353 354 355
		mu 1 4 173 172 176 177
		f 4 -235 235 242 276
		mu 0 4 356 357 358 359
		mu 1 4 175 174 152 155
		f 4 219 244 -246 -244
		mu 0 4 343 342 360 361
		mu 1 4 171 170 157 156
		f 4 223 246 -248 -245
		mu 0 4 362 363 364 365
		mu 1 4 170 178 158 157
		f 4 -223 243 250 -249
		mu 0 4 366 367 368 369
		mu 1 4 179 171 156 159
		f 4 -254 -226 -224 -253
		mu 0 4 370 371 372 373
		mu 1 4 180 181 178 170
		f 4 -256 252 230 -255
		mu 0 4 374 370 373 375
		mu 1 4 182 180 170 173
		f 4 -258 254 238 -257
		mu 0 4 376 377 378 379
		mu 1 4 183 182 173 177
		f 4 -260 256 -242 -259
		mu 0 4 380 381 382 383
		mu 1 4 184 183 177 176
		f 4 -261 -262 258 -241
		mu 0 4 384 385 386 387
		mu 1 4 172 185 184 176
		f 4 -263 -264 260 -233
		mu 0 4 388 389 390 391
		mu 1 4 171 186 185 172
		f 4 -266 262 222 224
		mu 0 4 392 389 388 393
		mu 1 4 187 186 171 179
		f 4 -269 -252 253 -268
		mu 0 4 394 395 396 397
		mu 1 4 163 162 181 180
		f 4 -271 267 255 -270
		mu 0 4 398 394 397 399
		mu 1 4 168 163 180 182
		f 4 -273 269 257 -272
		mu 0 4 400 401 402 403
		mu 1 4 154 168 182 183
		f 4 -275 271 259 -274
		mu 0 4 404 405 406 407
		mu 1 4 155 154 183 184
		f 4 -276 -277 273 261
		mu 0 4 408 409 410 411
		mu 1 4 185 175 155 184
		f 4 -278 -279 275 263
		mu 0 4 412 413 414 415
		mu 1 4 186 166 175 185
		f 4 -281 277 265 264
		mu 0 4 416 413 412 417
		mu 1 4 167 166 186 187
		f 4 291 292 311 -294
		mu 0 4 418 419 420 421
		mu 1 4 188 189 190 191
		f 4 -285 -304 305 -283
		mu 0 4 422 423 424 425
		mu 1 4 192 193 194 195
		f 4 283 281 317 316
		mu 0 4 426 427 428 429
		mu 1 4 196 197 198 199
		f 4 282 307 -288 -287
		mu 0 4 422 425 430 431
		mu 1 4 192 195 200 201
		f 4 -282 285 288 315
		mu 0 4 428 427 432 433
		mu 1 4 198 197 202 203
		f 4 287 309 -293 -291
		mu 0 4 434 435 436 437
		mu 1 4 201 200 190 189
		f 4 -289 289 293 313
		mu 0 4 438 439 440 441
		mu 1 4 203 202 188 191
		f 4 -306 -295 295 -305
		mu 0 4 442 443 444 445
		mu 1 4 195 194 204 205
		f 4 -308 304 296 -307
		mu 0 4 446 442 445 447
		mu 1 4 200 195 205 206
		f 4 -310 306 297 -309
		mu 0 4 448 449 450 451
		mu 1 4 190 200 206 207
		f 4 -312 308 298 -311
		mu 0 4 452 453 454 455
		mu 1 4 191 190 207 208
		f 4 -313 -314 310 299
		mu 0 4 456 457 458 459
		mu 1 4 209 203 191 208
		f 4 -315 -316 312 300
		mu 0 4 460 461 462 463
		mu 1 4 210 198 203 209
		f 4 -318 314 302 301
		mu 0 4 464 461 460 465
		mu 1 4 199 198 210 211
		f 4 -299 322 334 -324
		mu 0 4 466 467 468 469
		mu 1 4 212 213 214 215
		f 4 -296 -327 328 -320
		mu 0 4 470 471 472 473
		mu 1 4 216 217 218 219
		f 4 -303 318 340 339
		mu 0 4 474 475 476 477
		mu 1 4 220 221 222 223
		f 4 319 330 -321 -297
		mu 0 4 470 473 478 479
		mu 1 4 216 219 224 225
		f 4 -319 -301 321 338
		mu 0 4 476 475 480 481
		mu 1 4 222 221 226 227
		f 4 320 332 -323 -298
		mu 0 4 482 483 484 485
		mu 1 4 225 224 214 213
		f 4 -322 -300 323 336
		mu 0 4 486 487 488 489
		mu 1 4 227 226 212 215
		f 4 -329 -325 227 -328
		mu 0 4 490 491 492 493
		mu 1 4 219 218 228 229
		f 4 -331 327 229 -330
		mu 0 4 494 490 493 495
		mu 1 4 224 219 229 230
		f 4 -333 329 236 -332
		mu 0 4 496 497 498 499
		mu 1 4 214 224 230 231
		f 4 -335 331 -238 -334
		mu 0 4 500 501 502 503
		mu 1 4 215 214 231 232
		f 4 -336 -337 333 -236
		mu 0 4 504 505 506 507
		mu 1 4 233 227 215 232
		f 4 -338 -339 335 -229
		mu 0 4 508 509 510 511
		mu 1 4 234 222 227 233
		f 4 -341 337 -227 325
		mu 0 4 512 509 508 513
		mu 1 4 223 222 234 235
		f 4 -352 -342 342 -351
		mu 0 4 514 515 516 517
		mu 1 4 236 237 238 239
		f 4 -354 350 343 -353
		mu 0 4 518 514 517 519
		mu 1 4 240 236 239 241
		f 4 -356 352 344 -355
		mu 0 4 520 521 522 523
		mu 1 4 242 240 241 243
		f 4 -358 354 345 -357
		mu 0 4 524 525 526 527
		mu 1 4 244 242 243 245
		f 4 -359 -360 356 346
		mu 0 4 528 529 530 531
		mu 1 4 246 247 244 245
		f 4 -361 -362 358 347
		mu 0 4 532 533 534 535
		mu 1 4 248 249 247 246
		f 4 -363 360 349 348
		mu 0 4 536 533 532 537
		mu 1 4 250 249 248 251
		f 4 -346 376 411 -380
		mu 0 4 538 539 540 541
		mu 1 4 252 253 254 255
		f 4 382 384 -387 -388
		mu 0 4 542 543 544 545
		mu 1 4 256 257 258 259
		f 4 -343 -404 405 -366
		mu 0 4 546 547 548 549
		mu 1 4 260 261 262 263
		f 4 -350 364 417 416
		mu 0 4 550 551 552 553
		mu 1 4 264 265 266 267
		f 4 365 407 -372 -344
		mu 0 4 546 549 554 555
		mu 1 4 260 263 268 269
		f 4 -364 372 373 -371
		mu 0 4 556 557 558 559
		mu 1 4 270 271 272 273
		f 4 -365 -348 374 415
		mu 0 4 552 551 560 561
		mu 1 4 266 265 274 275
		f 4 371 409 -377 -345
		mu 0 4 562 563 564 565
		mu 1 4 269 268 254 253
		f 4 -374 377 378 -376
		mu 0 4 566 567 568 569
		mu 1 4 273 272 276 277
		f 4 -375 -347 379 413
		mu 0 4 570 571 572 573
		mu 1 4 275 274 252 255
		f 4 363 381 -383 -381
		mu 0 4 557 556 574 575
		mu 1 4 271 270 257 256
		f 4 367 383 -385 -382
		mu 0 4 576 577 578 579
		mu 1 4 270 278 258 257
		f 4 -367 380 387 -386
		mu 0 4 580 581 582 583
		mu 1 4 279 271 256 259
		f 4 -391 -370 -368 -390
		mu 0 4 584 585 586 587
		mu 1 4 280 281 278 270
		f 4 -393 389 370 -392
		mu 0 4 588 584 587 589
		mu 1 4 282 280 270 273
		f 4 -395 391 375 -394
		mu 0 4 590 591 592 593
		mu 1 4 283 282 273 277
		f 4 -397 393 -379 -396
		mu 0 4 594 595 596 597
		mu 1 4 284 283 277 276
		f 4 -398 -399 395 -378
		mu 0 4 598 599 600 601
		mu 1 4 272 285 284 276
		f 4 -400 -401 397 -373
		mu 0 4 602 603 604 605
		mu 1 4 271 286 285 272
		f 4 -403 399 366 368
		mu 0 4 606 603 602 607
		mu 1 4 287 286 271 279
		f 4 -406 -389 390 -405
		mu 0 4 608 609 610 611
		mu 1 4 263 262 281 280
		f 4 -408 404 392 -407
		mu 0 4 612 608 611 613
		mu 1 4 268 263 280 282
		f 4 -410 406 394 -409
		mu 0 4 614 615 616 617
		mu 1 4 254 268 282 283
		f 4 -412 408 396 -411
		mu 0 4 618 619 620 621
		mu 1 4 255 254 283 284
		f 4 -413 -414 410 398
		mu 0 4 622 623 624 625
		mu 1 4 285 275 255 284
		f 4 -415 -416 412 400
		mu 0 4 626 627 628 629
		mu 1 4 286 266 275 285
		f 4 -418 414 402 401
		mu 0 4 630 627 626 631
		mu 1 4 267 266 286 287
		f 4 503 502 -448 449
		mu 0 4 632 633 634 635
		mu 1 4 288 289 290 291
		f 4 495 -427 -454 455
		mu 0 4 636 637 638 639
		mu 1 4 292 293 294 295
		f 4 496 -428 426 497
		mu 0 4 640 641 638 637
		mu 1 4 296 297 294 293
		f 4 487 -435 -433 -485
		mu 0 4 642 643 644 645
		mu 1 4 298 299 300 301
		f 4 467 479 -437 -466
		mu 0 4 646 647 648 649
		mu 1 4 302 303 304 305
		f 4 490 -457 458 491
		mu 0 4 650 651 652 653
		mu 1 4 306 307 308 309
		f 4 484 -463 464 485
		mu 0 4 654 655 656 657
		mu 1 4 310 311 312 313
		f 4 509 -441 -442 443
		mu 0 4 658 659 660 661
		mu 1 4 314 315 316 317
		f 4 507 -444 -445 446
		mu 0 4 662 658 661 663
		mu 1 4 318 314 317 319
		f 4 505 -447 -421 -503
		mu 0 4 633 662 663 634
		mu 1 4 320 318 319 321
		f 4 501 -450 -451 452
		mu 0 4 664 632 635 665
		mu 1 4 322 288 291 323
		f 4 499 -453 -425 -497
		mu 0 4 666 664 665 667
		mu 1 4 324 322 323 325
		f 4 493 -456 -439 -491
		mu 0 4 668 636 639 669
		mu 1 4 326 292 295 327
		f 4 -459 -436 434 489
		mu 0 4 653 652 644 643
		mu 1 4 309 308 300 299
		f 4 -462 -438 436 481
		mu 0 4 670 671 649 648
		mu 1 4 328 329 330 331
		f 4 -465 -460 461 483
		mu 0 4 657 656 671 670
		mu 1 4 313 312 329 328
		f 4 470 477 -468 -469
		mu 0 4 672 673 647 646
		mu 1 4 332 333 303 302
		f 4 472 475 -471 -472
		mu 0 4 674 675 673 672
		mu 1 4 334 335 333 332
		f 4 -476 473 -470 -475
		mu 0 4 673 675 676 677
		mu 1 4 333 335 336 337
		f 4 -478 474 -467 -477
		mu 0 4 647 673 677 678
		mu 1 4 303 333 337 338
		f 4 -480 476 -431 -479
		mu 0 4 648 647 678 679
		mu 1 4 304 303 338 339
		f 4 -481 -482 478 -461
		mu 0 4 680 670 648 679
		mu 1 4 340 328 331 341
		f 4 -483 -484 480 -464
		mu 0 4 681 657 670 680
		mu 1 4 342 313 328 340
		f 4 433 -486 482 -432
		mu 0 4 682 654 657 681
		mu 1 4 343 310 313 342
		f 4 -429 -487 -488 -434
		mu 0 4 683 684 643 642
		mu 1 4 344 345 299 298
		f 4 -489 -490 486 -458
		mu 0 4 685 653 643 684
		mu 1 4 346 309 299 345
		f 4 418 -492 488 -430
		mu 0 4 686 650 653 685
		mu 1 4 347 306 309 346
		f 4 -455 -493 -494 -419
		mu 0 4 687 688 636 668
		mu 1 4 348 349 292 326
		f 4 -423 -495 -496 492
		mu 0 4 688 689 637 636
		mu 1 4 349 350 293 292
		f 4 425 -498 494 -424
		mu 0 4 690 640 637 689
		mu 1 4 351 296 293 350
		f 4 -452 -499 -500 -426
		mu 0 4 691 692 664 666
		mu 1 4 352 353 322 324
		f 4 -449 -501 -502 498
		mu 0 4 692 693 632 664
		mu 1 4 353 354 288 322
		f 4 -422 419 -504 500
		mu 0 4 693 694 633 632
		mu 1 4 354 355 289 288
		f 4 -446 -505 -506 -420
		mu 0 4 694 695 662 633
		mu 1 4 356 357 318 320
		f 4 -443 -507 -508 504
		mu 0 4 695 696 658 662
		mu 1 4 357 358 314 318
		f 4 -440 -509 -510 506
		mu 0 4 696 697 659 658
		mu 1 4 358 359 315 314
		f 4 543 542 421 516
		mu 0 4 698 699 700 701
		mu 1 4 360 361 362 363
		f 4 537 -511 422 518
		mu 0 4 702 703 704 705
		mu 1 4 364 365 366 367
		f 4 538 423 510 539
		mu 0 4 706 707 704 703
		mu 1 4 368 369 366 365
		f 4 532 -512 428 -531
		mu 0 4 708 709 710 711
		mu 1 4 370 371 372 373
		f 4 522 527 -513 430
		mu 0 4 712 713 714 715
		mu 1 4 374 375 376 377
		f 4 534 429 519 535
		mu 0 4 716 717 718 719
		mu 1 4 378 379 380 381
		f 4 530 431 521 531
		mu 0 4 720 721 722 723
		mu 1 4 382 383 384 385
		f 4 546 -514 439 514
		mu 0 4 724 725 726 727
		mu 1 4 386 387 388 389
		f 4 545 -515 442 515
		mu 0 4 728 724 727 729
		mu 1 4 390 386 389 391
		f 4 544 -516 445 -543
		mu 0 4 699 728 729 700
		mu 1 4 392 390 391 393
		f 4 541 -517 448 517
		mu 0 4 730 698 701 731
		mu 1 4 394 360 363 395
		f 4 540 -518 451 -539
		mu 0 4 732 730 731 733
		mu 1 4 396 394 395 397
		f 4 536 -519 454 -535
		mu 0 4 734 702 705 735
		mu 1 4 398 364 367 399
		f 4 -520 457 511 533
		mu 0 4 719 718 710 709
		mu 1 4 381 380 372 371
		f 4 -521 460 512 528
		mu 0 4 736 737 715 714
		mu 1 4 400 401 402 403
		f 4 -522 463 520 529
		mu 0 4 723 722 737 736
		mu 1 4 385 384 401 400
		f 4 523 526 -523 466
		mu 0 4 738 739 713 712
		mu 1 4 404 405 375 374
		f 4 524 525 -524 469
		mu 0 4 740 741 739 738
		mu 1 4 406 407 405 404
		f 4 549 562 -553 -549
		mu 0 4 742 743 744 745
		mu 1 4 408 409 410 411
		f 4 552 564 -556 -551
		mu 0 4 745 744 746 747
		mu 1 4 411 410 412 413
		f 4 555 566 -558 -554
		mu 0 4 747 746 748 749
		mu 1 4 413 412 414 415
		f 4 -563 560 -552 -562
		mu 0 4 744 743 750 751
		mu 1 4 410 409 416 417
		f 4 -565 561 -555 -564
		mu 0 4 746 744 751 752
		mu 1 4 412 410 417 418
		f 4 -567 563 -557 -566
		mu 0 4 748 746 752 753
		mu 1 4 414 412 418 419
		f 4 603 -586 -587 588
		mu 0 4 754 755 756 757
		mu 1 4 420 421 422 423
		f 4 601 -589 -590 591
		mu 0 4 758 754 757 759
		mu 1 4 424 420 423 425
		f 4 599 -592 -593 593
		mu 0 4 760 758 759 761
		mu 1 4 426 424 425 427
		f 4 -591 -599 -600 597
		mu 0 4 762 763 758 760
		mu 1 4 428 429 424 426
		f 4 -588 -601 -602 598
		mu 0 4 763 764 754 758
		mu 1 4 429 430 420 424
		f 4 -585 -603 -604 600
		mu 0 4 764 765 755 754
		mu 1 4 430 431 421 420
		f 4 604 611 -606 551
		mu 0 4 766 767 768 769
		mu 1 4 432 433 434 435
		f 4 605 612 -607 554
		mu 0 4 769 768 770 771
		mu 1 4 435 434 436 437
		f 4 606 613 -608 556
		mu 0 4 771 770 772 773
		mu 1 4 437 436 438 439
		f 4 -619 615 637 -618
		mu 0 4 774 775 776 777
		mu 1 4 440 441 442 443
		f 4 633 -627 584 627
		mu 0 4 778 779 780 781
		mu 1 4 444 445 446 447
		f 4 632 -628 587 628
		mu 0 4 782 778 781 783
		mu 1 4 448 444 447 449
		f 4 631 -629 590 629
		mu 0 4 784 782 783 785
		mu 1 4 450 448 449 451
		f 4 634 -631 596 626
		mu 0 4 786 787 788 789
		mu 1 4 445 452 453 446
		f 4 -644 -579 -578 -643
		mu 0 4 790 791 792 793
		mu 1 4 454 455 456 457
		f 4 -646 642 -581 -645
		mu 0 4 794 795 796 797
		mu 1 4 458 454 457 459
		f 4 -647 644 -583 581
		mu 0 4 798 799 800 801
		mu 1 4 460 458 459 461
		f 4 569 -648 -582 -568
		mu 0 4 802 803 804 805
		mu 1 4 462 463 460 461
		f 4 -573 571 -649 -570
		mu 0 4 802 806 807 803
		mu 1 4 462 464 465 463
		f 4 -651 -572 -571 -650
		mu 0 4 808 809 810 811
		mu 1 4 466 465 464 467
		f 4 -652 649 -575 573
		mu 0 4 812 813 814 815
		mu 1 4 468 466 467 469
		f 4 -654 -574 -569 -653
		mu 0 4 816 817 818 819
		mu 1 4 470 468 469 471
		f 4 -663 -665 -666 663
		mu 0 4 820 821 822 823
		mu 1 4 472 473 474 475
		f 4 -662 -667 -668 664
		mu 0 4 824 825 826 827
		mu 1 4 473 476 477 474
		f 4 -661 -656 -669 666
		mu 0 4 828 829 830 831
		mu 1 4 476 478 479 477
		f 4 720 -657 660 672
		mu 0 4 832 833 834 835
		mu 1 4 480 481 482 483
		f 4 718 -673 661 674
		mu 0 4 836 837 838 839
		mu 1 4 484 480 483 485
		f 4 716 -675 662 670
		mu 0 4 840 841 842 843
		mu 1 4 486 484 485 487
		f 4 681 -658 669 676
		mu 0 4 844 845 846 847
		mu 1 4 488 489 490 491
		f 4 680 -677 671 677
		mu 0 4 848 849 850 851
		mu 1 4 492 493 494 495
		f 4 679 -678 673 675
		mu 0 4 852 853 854 855
		mu 1 4 496 497 498 499
		f 4 684 711 -659 -682
		mu 0 4 856 857 858 859
		mu 1 4 488 500 501 489
		f 4 686 709 -685 -681
		mu 0 4 860 861 862 863
		mu 1 4 502 503 504 505
		f 4 707 -687 -680 678
		mu 0 4 864 865 866 867
		mu 1 4 506 507 508 509
		f 4 -686 -689 -690 687
		mu 0 4 868 869 870 871
		mu 1 4 510 511 512 513
		f 4 -684 -691 -692 688
		mu 0 4 872 873 874 875
		mu 1 4 511 514 515 512
		f 4 -683 -660 -693 690
		mu 0 4 876 877 878 879
		mu 1 4 514 516 517 515
		f 4 714 -671 -696 694
		mu 0 4 880 881 882 883
		mu 1 4 518 519 520 521
		f 4 698 -714 -715 712
		mu 0 4 884 885 881 880
		mu 1 4 522 523 524 525
		f 4 -697 -676 -699 697
		mu 0 4 886 887 888 889
		mu 1 4 526 527 528 529
		f 4 -679 696 700 705
		mu 0 4 890 891 892 893
		mu 1 4 530 531 532 533
		f 4 702 -688 699 701
		mu 0 4 894 895 896 897
		mu 1 4 534 535 536 537
		f 4 -700 -705 -706 703
		mu 0 4 898 899 890 893
		mu 1 4 538 539 540 541
		f 4 689 -707 -708 704
		mu 0 4 900 901 865 864
		mu 1 4 513 512 542 543
		f 4 691 -709 -710 706
		mu 0 4 902 903 862 861
		mu 1 4 512 515 500 542
		f 4 692 -711 -712 708
		mu 0 4 904 905 858 857
		mu 1 4 515 517 501 500
		f 4 -716 -717 713 -674
		mu 0 4 906 841 840 907
		mu 1 4 544 545 546 547
		f 4 -718 -719 715 -672
		mu 0 4 908 837 836 909
		mu 1 4 548 549 550 551
		f 4 -720 -721 717 -670
		mu 0 4 910 833 832 911
		mu 1 4 552 481 480 553
		f 4 732 739 -732 725
		mu 0 4 912 913 914 915
		mu 1 4 554 555 556 557
		f 4 733 737 -733 726
		mu 0 4 916 917 918 919
		mu 1 4 558 559 555 554
		f 4 721 735 -734 727
		mu 0 4 920 921 922 923
		mu 1 4 560 561 559 558
		f 4 722 682 -735 -736
		mu 0 4 924 925 926 927
		mu 1 4 561 562 563 559
		f 4 -738 734 683 -737
		mu 0 4 928 929 930 931
		mu 1 4 555 559 563 564
		f 4 -740 736 685 -739
		mu 0 4 932 933 934 935
		mu 1 4 556 555 564 565
		f 4 748 -724 728 741
		mu 0 4 936 937 938 939
		mu 1 4 566 567 568 569
		f 4 747 -742 729 742
		mu 0 4 940 941 942 943
		mu 1 4 570 566 569 571
		f 4 745 -743 730 740
		mu 0 4 944 945 946 947
		mu 1 4 572 570 571 573
		f 4 665 -745 -746 743
		mu 0 4 948 949 950 951
		mu 1 4 574 575 570 572
		f 4 667 -747 -748 744
		mu 0 4 952 953 954 955
		mu 1 4 575 576 566 570
		f 4 668 -725 -749 746
		mu 0 4 956 957 958 959
		mu 1 4 576 577 567 566
		f 4 731 757 -756 751
		mu 0 4 960 961 962 963
		mu 1 4 578 579 580 581
		f 4 738 -703 -757 -758
		mu 0 4 964 965 966 967
		mu 1 4 579 582 583 580
		f 4 -764 -765 -761 -704
		mu 0 4 968 969 970 971
		mu 1 4 541 584 585 538
		f 4 764 -767 -766 -762
		mu 0 4 972 973 974 975
		mu 1 4 585 584 586 587
		f 4 769 695 -664 768
		mu 0 4 976 977 978 979
		mu 1 4 588 521 520 589
		f 4 770 -772 -741 752
		mu 0 4 980 981 982 983
		mu 1 4 590 591 592 593
		f 4 -774 -769 -744 771
		mu 0 4 984 985 986 987
		mu 1 4 591 594 595 592
		f 4 774 758 -770 775
		mu 0 4 988 989 990 991
		mu 1 4 596 597 521 588
		f 4 -771 753 776 777
		mu 0 4 992 993 994 995
		mu 1 4 591 590 598 599
		f 4 -779 -776 773 -778
		mu 0 4 996 997 998 999
		mu 1 4 599 600 594 591
		f 3 -781 693 779
		mu 0 3 1000 1001 1002
		mu 1 3 601 602 603
		f 4 -783 780 -760 -775
		mu 0 4 1003 1001 1000 1004
		mu 1 4 596 602 601 597
		f 3 784 -694 -784
		mu 0 3 1005 1006 1007
		mu 1 3 604 605 606
		f 4 -785 -626 781 -768
		mu 0 4 1006 1005 1008 1009
		mu 1 4 605 604 607 608
		f 4 -616 -617 785 786
		mu 0 4 1010 1011 1012 1013
		mu 1 4 442 441 609 610
		f 4 765 -787 -763 787
		mu 0 4 975 974 1014 1015
		mu 1 4 587 586 611 612
		f 4 -615 788 762 -786
		mu 0 4 1016 1017 1018 1019
		mu 1 4 609 613 614 610
		f 4 789 -702 760 790
		mu 0 4 1020 1021 1022 1023
		mu 1 4 615 534 537 616
		f 4 -793 791 -791 761
		mu 0 4 1024 1025 1026 1027
		mu 1 4 617 618 615 616
		f 3 792 -788 -795
		mu 0 3 1025 1024 1028
		mu 1 3 618 617 619
		f 3 -789 793 794
		mu 0 3 1018 1017 1029
		mu 1 3 614 613 620
		f 4 -799 750 755 796
		mu 0 4 1030 1031 1032 1033
		mu 1 4 621 622 581 580
		f 4 -797 756 -790 797
		mu 0 4 1034 1035 1036 1037
		mu 1 4 621 580 583 623
		f 4 -550 -559 800 801
		mu 0 4 1038 1039 1040 1041
		mu 1 4 409 408 624 625
		f 4 -804 -561 -802 -803
		mu 0 4 1042 1043 1044 1045
		mu 1 4 626 416 409 625
		f 4 -605 803 804 610
		mu 0 4 1046 1047 1048 1049
		mu 1 4 433 432 627 628
		f 4 640 -580 578 -642
		mu 0 4 1050 1051 1052 1053
		mu 1 4 629 630 456 455
		f 3 805 -812 806
		mu 0 3 1054 1055 1056
		mu 1 3 631 632 633
		f 3 -808 795 -817
		mu 0 3 1057 1058 1059
		mu 1 3 621 634 635
		f 4 807 -798 -792 -807
		mu 0 4 1058 1057 1060 1061
		mu 1 4 634 621 623 636
		f 4 809 -801 -560 808
		mu 0 4 1062 1063 1064 1065
		mu 1 4 637 625 624 638
		f 4 -818 -809 -548 -641
		mu 0 4 1066 1067 1068 1069
		mu 1 4 639 637 638 640
		f 5 -820 810 -819 -800 -821
		mu 0 5 1070 1071 1072 1073 1074
		mu 1 5 641 642 637 643 644
		f 4 -811 -813 802 -810
		mu 0 4 1075 1076 1077 1078
		mu 1 4 637 642 626 625
		f 4 609 -805 812 813
		mu 0 4 1079 1080 1081 1082
		mu 1 4 645 628 627 646
		f 4 814 749 798 -816
		mu 0 4 1083 1084 1085 1086
		mu 1 4 647 648 622 621
		f 3 815 816 799
		mu 0 3 1083 1086 1087
		mu 1 3 647 621 635
		f 3 817 -815 818
		mu 0 3 1067 1066 1088
		mu 1 3 637 639 643
		f 3 820 -796 811
		mu 0 3 1070 1074 1089
		mu 1 3 641 644 649
		f 5 -814 819 -806 -794 608
		mu 0 5 1090 1091 1055 1054 1092
		mu 1 5 645 646 632 631 650
		f 4 822 630 635 -827
		mu 0 4 1093 1094 1095 1096
		mu 1 4 651 453 452 652
		f 3 -828 -826 824
		mu 0 3 1097 1098 1099
		mu 1 3 653 654 655
		f 4 825 782 778 -824
		mu 0 4 1100 1101 1102 1103
		mu 1 4 656 657 600 599
		f 5 -822 826 636 783 827
		mu 0 5 1097 1104 1105 1106 1098
		mu 1 5 653 651 652 658 654
		f 4 585 828 -833 -584
		mu 0 4 1107 1108 1109 1110
		mu 1 4 422 421 659 660
		f 4 602 -597 829 -829
		mu 0 4 1111 1112 1113 1114
		mu 1 4 421 431 661 659
		f 4 575 -655 652 -577
		mu 0 4 1115 1116 1117 1118
		mu 1 4 662 663 470 471
		f 4 840 -576 -595 830
		mu 0 4 1119 1120 1121 1122
		mu 1 4 664 665 666 667
		f 4 -831 -596 832 833
		mu 0 4 1123 1124 1125 1126
		mu 1 4 664 667 660 659
		f 4 834 -834 -830 -823
		mu 0 4 1127 1128 1129 1130
		mu 1 4 668 664 659 661
		f 3 -838 835 836
		mu 0 3 1131 1132 1133
		mu 1 3 599 669 670
		f 4 837 -777 754 772
		mu 0 4 1132 1131 1134 1135
		mu 1 4 669 599 598 671
		f 3 823 -837 -840
		mu 0 3 1100 1103 1136
		mu 1 3 656 599 670
		f 3 838 -836 -832
		mu 0 3 1137 1138 1139
		mu 1 3 664 672 673
		f 5 -839 -835 821 -825 839
		mu 0 5 1138 1137 1140 1141 1142
		mu 1 5 672 664 668 674 675
		f 3 -841 831 -773
		mu 0 3 1120 1119 1143
		mu 1 3 665 664 673
		f 4 -843 -620 617 841
		mu 0 4 1144 1145 1146 1147
		mu 1 4 676 677 440 443
		f 4 638 -844 -621 842
		mu 0 4 1148 1149 1150 1151
		mu 1 4 676 678 679 677
		f 3 844 -842 -638
		mu 0 3 974 1152 1153
		mu 1 3 680 681 682
		f 4 -851 -624 845 846
		mu 0 4 1154 1155 1156 1157
		mu 1 4 683 684 685 686
		f 4 848 -695 -759 847
		mu 0 4 1158 1159 1160 1161
		mu 1 4 687 518 521 597
		f 4 759 -853 849 -848
		mu 0 4 1162 1163 1164 1165
		mu 1 4 597 601 688 687
		f 4 -625 850 851 -782
		mu 0 4 1166 1167 1168 1169
		mu 1 4 607 684 683 608
		f 4 -852 852 -780 767
		mu 0 4 1170 1164 1163 1171
		mu 1 4 689 688 601 603
		f 4 -622 843 639 -854
		mu 0 4 1172 1150 1149 1173
		mu 1 4 690 679 678 691
		f 4 -623 853 854 -846
		mu 0 4 1174 1175 1176 1177
		mu 1 4 685 690 691 686
		f 4 -862 855 -713 -849
		mu 0 4 1158 1178 1179 1159
		mu 1 4 692 693 694 695
		f 3 856 -847 -855
		mu 0 3 1180 1164 1181
		mu 1 3 696 697 698
		f 4 -698 -856 -860 -858
		mu 0 4 1182 1183 1184 1185
		mu 1 4 699 700 701 702
		f 4 857 858 763 -701
		mu 0 4 1186 1187 969 968
		mu 1 4 703 704 705 706
		f 3 -863 859 864
		mu 0 3 1188 1189 1190
		mu 1 3 707 708 709
		f 4 -859 -864 -845 766
		mu 0 4 973 1191 1152 974
		mu 1 4 710 711 712 713
		f 4 -857 860 861 -850
		mu 0 4 1164 1180 1192 1165
		mu 1 4 714 715 716 717
		f 3 862 -639 863
		mu 0 3 1193 1194 1195
		mu 1 3 708 707 718
		f 3 -865 -861 -640
		mu 0 3 1196 1197 1198
		mu 1 3 707 709 719
		f 4 865 870 -867 -870
		mu 0 4 1199 1200 1201 1202
		mu 1 4 720 721 722 723
		f 4 866 872 -868 -872
		mu 0 4 1202 1201 1203 1204
		mu 1 4 723 722 724 725
		f 4 867 874 -869 -874
		mu 0 4 1204 1203 1205 1206
		mu 1 4 725 724 726 727
		f 4 875 869 871 873
		mu 0 4 1207 1199 1202 1208
		mu 1 4 728 729 723 725
		f 4 876 881 -878 -881
		mu 0 4 1209 1210 1211 1212
		mu 1 4 730 731 732 733
		f 4 877 883 -879 -883
		mu 0 4 1212 1211 1213 1214
		mu 1 4 733 732 734 735
		f 4 878 885 -880 -885
		mu 0 4 1214 1213 1215 1216
		mu 1 4 735 734 736 737
		f 4 886 880 882 884
		mu 0 4 1217 1209 1212 1218
		mu 1 4 738 739 733 735
		f 4 887 892 -889 -892
		mu 0 4 1219 1220 1221 1222
		mu 1 4 740 741 742 743
		f 4 888 894 -890 -894
		mu 0 4 1222 1221 1223 1224
		mu 1 4 743 742 744 745
		f 4 889 896 -891 -896
		mu 0 4 1224 1223 1225 1226
		mu 1 4 745 744 746 747
		f 4 897 891 893 895
		mu 0 4 1227 1219 1222 1228
		mu 1 4 748 749 743 745
		f 4 898 903 -900 -903
		mu 0 4 1229 1230 1231 1232
		mu 1 4 750 751 752 753
		f 4 899 905 -901 -905
		mu 0 4 1232 1231 1233 1234
		mu 1 4 753 752 754 755
		f 4 900 907 -902 -907
		mu 0 4 1234 1233 1235 1236
		mu 1 4 755 754 756 757
		f 4 908 902 904 906
		mu 0 4 1237 1229 1232 1238
		mu 1 4 758 759 753 755
		f 4 909 914 -911 -914
		mu 0 4 1239 1240 1241 1242
		mu 1 4 760 761 762 763
		f 4 910 916 -912 -916
		mu 0 4 1242 1241 1243 1244
		mu 1 4 763 762 764 765
		f 4 911 918 -913 -918
		mu 0 4 1244 1243 1245 1246
		mu 1 4 765 764 766 767
		f 4 919 913 915 917
		mu 0 4 1247 1239 1242 1248
		mu 1 4 768 769 763 765
		f 4 920 925 -922 -925
		mu 0 4 1249 1250 1251 1252
		mu 1 4 770 771 772 773
		f 4 921 927 -923 -927
		mu 0 4 1252 1251 1253 1254
		mu 1 4 773 772 774 775
		f 4 922 929 -924 -929
		mu 0 4 1254 1253 1255 1256
		mu 1 4 775 774 776 777
		f 4 930 924 926 928
		mu 0 4 1257 1249 1252 1258
		mu 1 4 778 779 773 775
		f 4 931 936 -933 -936
		mu 0 4 1259 1260 1261 1262
		mu 1 4 780 781 782 783
		f 4 932 938 -934 -938
		mu 0 4 1262 1261 1263 1264
		mu 1 4 783 782 784 785
		f 4 933 940 -935 -940
		mu 0 4 1264 1263 1265 1266
		mu 1 4 785 784 786 787
		f 4 941 935 937 939
		mu 0 4 1267 1259 1262 1268
		mu 1 4 788 789 783 785
		f 4 942 947 -944 -947
		mu 0 4 1269 1270 1271 1272
		mu 1 4 790 791 792 793
		f 4 943 949 -945 -949
		mu 0 4 1272 1271 1273 1274
		mu 1 4 793 792 794 795
		f 4 944 951 -946 -951
		mu 0 4 1274 1273 1275 1276
		mu 1 4 795 794 796 797
		f 4 952 946 948 950
		mu 0 4 1277 1269 1272 1278
		mu 1 4 798 799 793 795
		f 4 953 958 -955 -958
		mu 0 4 1279 1280 1281 1282
		mu 1 4 800 801 802 803
		f 4 954 960 -956 -960
		mu 0 4 1282 1281 1283 1284
		mu 1 4 803 802 804 805
		f 4 955 962 -957 -962
		mu 0 4 1284 1283 1285 1286
		mu 1 4 805 804 806 807
		f 4 963 957 959 961
		mu 0 4 1287 1279 1282 1288
		mu 1 4 808 809 803 805
		f 4 964 969 -966 -969
		mu 0 4 1289 1290 1291 1292
		mu 1 4 810 811 812 813
		f 4 965 971 -967 -971
		mu 0 4 1292 1291 1293 1294
		mu 1 4 813 812 814 815
		f 4 966 973 -968 -973
		mu 0 4 1294 1293 1295 1296
		mu 1 4 815 814 816 817
		f 4 974 968 970 972
		mu 0 4 1297 1289 1292 1298
		mu 1 4 818 819 813 815
		f 4 993 995 -997 -998
		mu 0 4 1299 1300 1301 1302
		mu 1 4 820 821 822 823
		f 4 996 999 -1001 -1002
		mu 0 4 1303 1304 1305 1306
		mu 1 4 824 825 826 827
		f 4 1003 1004 -994 -1006
		mu 0 4 1307 1308 1309 1310
		mu 1 4 828 829 830 831
		f 4 975 989 -977 -978
		mu 0 4 1311 1312 1313 1314
		mu 1 4 832 833 834 835
		f 4 -981 978 990 -980
		mu 0 4 1315 1316 1317 1318
		mu 1 4 836 837 838 839
		f 4 -982 979 991 -976
		mu 0 4 1319 1320 1321 1322
		mu 1 4 840 841 842 843
		f 4 982 977 -984 -985
		mu 0 4 1323 1311 1314 1324
		mu 1 4 844 832 835 845
		f 4 -988 985 980 -987
		mu 0 4 1325 1326 1316 1315
		mu 1 4 846 847 837 836
		f 4 -989 986 981 -983
		mu 0 4 1327 1328 1320 1319
		mu 1 4 848 849 841 840
		f 4 1007 1008 -1011 -1012
		mu 0 4 1329 1330 1331 1332
		mu 1 4 850 851 852 853
		f 4 -1016 1013 1016 -1018
		mu 0 4 1333 1334 1335 1336
		mu 1 4 854 855 856 857
		f 4 -1019 1017 1019 -1008
		mu 0 4 1337 1338 1339 1340
		mu 1 4 858 859 860 861
		f 4 988 994 -996 -993
		mu 0 4 1328 1327 1301 1300
		mu 1 4 849 848 822 821
		f 4 984 998 -1000 -995
		mu 0 4 1323 1324 1305 1304
		mu 1 4 844 845 826 825
		f 4 987 992 -1005 -1003
		mu 0 4 1326 1325 1309 1308
		mu 1 4 847 846 830 829
		f 4 -990 1006 1011 -1010
		mu 0 4 1313 1312 1329 1332
		mu 1 4 834 833 850 853
		f 4 -991 1012 1015 -1015
		mu 0 4 1318 1317 1334 1333
		mu 1 4 839 838 855 854
		f 4 -992 1014 1018 -1007
		mu 0 4 1322 1321 1338 1337
		mu 1 4 843 842 859 858
		f 4 1038 1040 -1042 -1043
		mu 0 4 1341 1342 1343 1344
		mu 1 4 862 863 864 865
		f 4 1041 1044 -1046 -1047
		mu 0 4 1345 1346 1347 1348
		mu 1 4 866 867 868 869
		f 4 1048 1049 -1039 -1051
		mu 0 4 1349 1350 1351 1352
		mu 1 4 870 871 872 873
		f 4 1020 1034 -1022 -1023
		mu 0 4 1353 1354 1355 1356
		mu 1 4 874 875 876 877
		f 4 -1026 1023 1035 -1025
		mu 0 4 1357 1358 1359 1360
		mu 1 4 878 879 880 881
		f 4 -1027 1024 1036 -1021
		mu 0 4 1361 1362 1363 1364
		mu 1 4 882 883 884 885
		f 4 1027 1022 -1029 -1030
		mu 0 4 1365 1353 1356 1366
		mu 1 4 886 874 877 887
		f 4 -1033 1030 1025 -1032
		mu 0 4 1367 1368 1358 1357
		mu 1 4 888 889 879 878
		f 4 -1034 1031 1026 -1028
		mu 0 4 1369 1370 1362 1361
		mu 1 4 890 891 883 882
		f 4 1052 1053 -1056 -1057
		mu 0 4 1371 1372 1373 1374
		mu 1 4 892 893 894 895
		f 4 -1061 1058 1061 -1063
		mu 0 4 1375 1376 1377 1378
		mu 1 4 896 897 898 899
		f 4 -1064 1062 1064 -1053
		mu 0 4 1379 1380 1381 1382
		mu 1 4 900 901 902 903
		f 4 1033 1039 -1041 -1038
		mu 0 4 1370 1369 1343 1342
		mu 1 4 891 890 864 863
		f 4 1029 1043 -1045 -1040
		mu 0 4 1365 1366 1347 1346
		mu 1 4 886 887 868 867
		f 4 1032 1037 -1050 -1048
		mu 0 4 1368 1367 1351 1350
		mu 1 4 889 888 872 871
		f 4 -1035 1051 1056 -1055
		mu 0 4 1355 1354 1371 1374
		mu 1 4 876 875 892 895
		f 4 -1036 1057 1060 -1060
		mu 0 4 1360 1359 1376 1375
		mu 1 4 881 880 897 896
		f 4 -1037 1059 1063 -1052
		mu 0 4 1364 1363 1380 1379
		mu 1 4 885 884 901 900;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".bw" 3;
createNode transform -n "pPlane1";
createNode mesh -n "pPlaneShape1" -p "pPlane1";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.072170124105911546 0.67096073356976949 ;
	setAttr -s 2 ".uvst";
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 12 ".uvst[0].uvsp[0:11]" -type "float2" 1 0.00079427799 1
		 0.99827093 2.9802322e-008 0.00079427799 2.9802322e-008 0.99827093 1 0.99827087 2.9802322e-008
		 0.0007942915 1 0.99827087 2.9802322e-008 0.0007942915 1 0.0007942915 2.9802322e-008
		 0.99827087 2.9802322e-008 0.99827087 1 0.0007942915;
	setAttr ".uvst[1].uvsn" -type "string" "LightMap";
	setAttr -s 8 ".uvst[1].uvsp[0:7]" -type "float2" 0.0744555 0.6680935
		 0.074366421 0.66985214 0.070062995 0.67031062 0.070152104 0.66855204 0.074277282
		 0.67161077 0.074188173 0.67336941 0.069884717 0.67382801 0.069973886 0.67206931;
	setAttr ".cuvs" -type "string" "LightMap";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  0 8.059166 0 0 8.059166 0 
		0 8.059166 0 0 8.059166 0 0 8.059166 0 0 8.059166 0 0 8.059166 0 0 8.059166 0;
	setAttr -s 8 ".vt[0:7]"  -23.71952629 -20.99999237 -6.1035155e-007
		 23.71952629 -20.99999237 -6.1035155e-007 -23.71952629 -20.86527061 -17.23232841 23.71952629 -20.86527061 -17.23232841
		 7.90650654 -20.99999237 -6.1035155e-007 7.90650654 -20.86527061 -17.23232841 -7.90650988 -20.99999237 -6.1035155e-007
		 -7.90650988 -20.86527061 -17.23232841;
	setAttr -s 10 ".ed[0:9]"  0 6 0 0 2 0 1 3 0 2 7 0 4 1 0 5 3 0 4 5 1
		 6 4 0 7 5 0 6 7 1;
	setAttr -s 3 ".fc[0:2]" -type "polyFaces" 
		f 4 0 9 -4 -2
		mu 0 4 0 6 9 2
		mu 1 4 0 1 2 3
		f 4 4 2 -6 -7
		mu 0 4 11 1 3 5
		mu 1 4 4 5 6 7
		f 4 7 6 -9 -10
		mu 0 4 8 4 10 7
		mu 1 4 1 4 7 2;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode lightLinker -s -n "lightLinker1";
	setAttr -s 4 ".lnk";
	setAttr -s 4 ".slnk";
createNode displayLayerManager -n "layerManager";
	setAttr -s 6 ".dli[1:5]"  1 2 3 4 5;
createNode displayLayer -n "defaultLayer";
createNode renderLayerManager -n "renderLayerManager";
createNode renderLayer -n "defaultRenderLayer";
	setAttr ".g" yes;
createNode script -n "uiConfigurationScriptNode";
	setAttr ".b" -type "string" (
		"// Maya Mel UI Configuration File.\n//\n//  This script is machine generated.  Edit at your own risk.\n//\n//\n\nglobal string $gMainPane;\nif (`paneLayout -exists $gMainPane`) {\n\n\tglobal int $gUseScenePanelConfig;\n\tint    $useSceneConfig = $gUseScenePanelConfig;\n\tint    $menusOkayInPanels = `optionVar -q allowMenusInPanels`;\tint    $nVisPanes = `paneLayout -q -nvp $gMainPane`;\n\tint    $nPanes = 0;\n\tstring $editorName;\n\tstring $panelName;\n\tstring $itemFilterName;\n\tstring $panelConfig;\n\n\t//\n\t//  get current state of the UI\n\t//\n\tsceneUIReplacement -update $gMainPane;\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"Stereo\" (localizedPanelLabel(\"Stereo\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"Stereo\" -l (localizedPanelLabel(\"Stereo\")) -mbv $menusOkayInPanels `;\nstring $editorName = ($panelName+\"Editor\");\n            stereoCameraView -e \n                -editorChanged \"updateModelPanelBar\" \n                -camera \"persp\" \n                -useInteractiveMode 0\n"
		+ "                -displayLights \"default\" \n                -displayAppearance \"wireframe\" \n                -activeOnly 0\n                -ignorePanZoom 0\n                -wireframeOnShaded 0\n                -headsUpDisplay 1\n                -selectionHiliteDisplay 1\n                -useDefaultMaterial 0\n                -bufferMode \"double\" \n                -twoSidedLighting 1\n                -backfaceCulling 0\n                -xray 0\n                -jointXray 0\n                -activeComponentsXray 0\n                -displayTextures 0\n                -smoothWireframe 0\n                -lineWidth 1\n                -textureAnisotropic 0\n                -textureHilight 1\n                -textureSampling 2\n                -textureDisplay \"modulate\" \n                -textureMaxSize 16384\n                -fogging 0\n                -fogSource \"fragment\" \n                -fogMode \"linear\" \n                -fogStart 0\n                -fogEnd 100\n                -fogDensity 0.1\n                -fogColor 0.5 0.5 0.5 1 \n                -maxConstantTransparency 1\n"
		+ "                -colorResolution 4 4 \n                -bumpResolution 4 4 \n                -textureCompression 0\n                -transparencyAlgorithm \"frontAndBackCull\" \n                -transpInShadows 0\n                -cullingOverride \"none\" \n                -lowQualityLighting 0\n                -maximumNumHardwareLights 0\n                -occlusionCulling 0\n                -shadingModel 0\n                -useBaseRenderer 0\n                -useReducedRenderer 0\n                -smallObjectCulling 0\n                -smallObjectThreshold -1 \n                -interactiveDisableShadows 0\n                -interactiveBackFaceCull 0\n                -sortTransparent 1\n                -nurbsCurves 1\n                -nurbsSurfaces 1\n                -polymeshes 1\n                -subdivSurfaces 1\n                -planes 1\n                -lights 1\n                -cameras 1\n                -controlVertices 1\n                -hulls 1\n                -grid 1\n                -joints 1\n                -ikHandles 1\n                -deformers 1\n"
		+ "                -dynamics 1\n                -fluids 1\n                -hairSystems 1\n                -follicles 1\n                -nCloths 1\n                -nParticles 1\n                -nRigids 1\n                -dynamicConstraints 1\n                -locators 1\n                -manipulators 1\n                -dimensions 1\n                -handles 1\n                -pivots 1\n                -textures 1\n                -strokes 1\n                -motionTrails 1\n                -shadows 0\n                -displayMode \"centerEye\" \n                -viewColor 0 0 0 1 \n                $editorName;\nstereoCameraView -e -viewSelected 0 $editorName;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Stereo\")) -mbv $menusOkayInPanels  $panelName;\nstring $editorName = ($panelName+\"Editor\");\n            stereoCameraView -e \n                -editorChanged \"updateModelPanelBar\" \n                -camera \"persp\" \n                -useInteractiveMode 0\n                -displayLights \"default\" \n"
		+ "                -displayAppearance \"wireframe\" \n                -activeOnly 0\n                -ignorePanZoom 0\n                -wireframeOnShaded 0\n                -headsUpDisplay 1\n                -selectionHiliteDisplay 1\n                -useDefaultMaterial 0\n                -bufferMode \"double\" \n                -twoSidedLighting 1\n                -backfaceCulling 0\n                -xray 0\n                -jointXray 0\n                -activeComponentsXray 0\n                -displayTextures 0\n                -smoothWireframe 0\n                -lineWidth 1\n                -textureAnisotropic 0\n                -textureHilight 1\n                -textureSampling 2\n                -textureDisplay \"modulate\" \n                -textureMaxSize 16384\n                -fogging 0\n                -fogSource \"fragment\" \n                -fogMode \"linear\" \n                -fogStart 0\n                -fogEnd 100\n                -fogDensity 0.1\n                -fogColor 0.5 0.5 0.5 1 \n                -maxConstantTransparency 1\n"
		+ "                -colorResolution 4 4 \n                -bumpResolution 4 4 \n                -textureCompression 0\n                -transparencyAlgorithm \"frontAndBackCull\" \n                -transpInShadows 0\n                -cullingOverride \"none\" \n                -lowQualityLighting 0\n                -maximumNumHardwareLights 0\n                -occlusionCulling 0\n                -shadingModel 0\n                -useBaseRenderer 0\n                -useReducedRenderer 0\n                -smallObjectCulling 0\n                -smallObjectThreshold -1 \n                -interactiveDisableShadows 0\n                -interactiveBackFaceCull 0\n                -sortTransparent 1\n                -nurbsCurves 1\n                -nurbsSurfaces 1\n                -polymeshes 1\n                -subdivSurfaces 1\n                -planes 1\n                -lights 1\n                -cameras 1\n                -controlVertices 1\n                -hulls 1\n                -grid 1\n                -joints 1\n                -ikHandles 1\n                -deformers 1\n"
		+ "                -dynamics 1\n                -fluids 1\n                -hairSystems 1\n                -follicles 1\n                -nCloths 1\n                -nParticles 1\n                -nRigids 1\n                -dynamicConstraints 1\n                -locators 1\n                -manipulators 1\n                -dimensions 1\n                -handles 1\n                -pivots 1\n                -textures 1\n                -strokes 1\n                -motionTrails 1\n                -shadows 0\n                -displayMode \"centerEye\" \n                -viewColor 0 0 0 1 \n                $editorName;\nstereoCameraView -e -viewSelected 0 $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"modelPanel\" (localizedPanelLabel(\"Top View\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `modelPanel -unParent -l (localizedPanelLabel(\"Top View\")) -mbv $menusOkayInPanels `;\n\t\t\t$editorName = $panelName;\n            modelEditor -e \n"
		+ "                -camera \"top\" \n                -useInteractiveMode 0\n                -displayLights \"default\" \n                -displayAppearance \"wireframe\" \n                -activeOnly 0\n                -ignorePanZoom 0\n                -wireframeOnShaded 0\n                -headsUpDisplay 1\n                -selectionHiliteDisplay 1\n                -useDefaultMaterial 0\n                -bufferMode \"double\" \n                -twoSidedLighting 1\n                -backfaceCulling 0\n                -xray 0\n                -jointXray 0\n                -activeComponentsXray 0\n                -displayTextures 0\n                -smoothWireframe 0\n                -lineWidth 1\n                -textureAnisotropic 0\n                -textureHilight 1\n                -textureSampling 2\n                -textureDisplay \"modulate\" \n                -textureMaxSize 16384\n                -fogging 0\n                -fogSource \"fragment\" \n                -fogMode \"linear\" \n                -fogStart 0\n                -fogEnd 100\n                -fogDensity 0.1\n"
		+ "                -fogColor 0.5 0.5 0.5 1 \n                -maxConstantTransparency 1\n                -rendererName \"base_OpenGL_Renderer\" \n                -colorResolution 256 256 \n                -bumpResolution 512 512 \n                -textureCompression 0\n                -transparencyAlgorithm \"frontAndBackCull\" \n                -transpInShadows 0\n                -cullingOverride \"none\" \n                -lowQualityLighting 0\n                -maximumNumHardwareLights 0\n                -occlusionCulling 0\n                -shadingModel 0\n                -useBaseRenderer 0\n                -useReducedRenderer 0\n                -smallObjectCulling 0\n                -smallObjectThreshold -1 \n                -interactiveDisableShadows 0\n                -interactiveBackFaceCull 0\n                -sortTransparent 1\n                -nurbsCurves 1\n                -nurbsSurfaces 1\n                -polymeshes 1\n                -subdivSurfaces 1\n                -planes 1\n                -lights 1\n                -cameras 1\n"
		+ "                -controlVertices 1\n                -hulls 1\n                -grid 1\n                -joints 1\n                -ikHandles 1\n                -deformers 1\n                -dynamics 1\n                -fluids 1\n                -hairSystems 1\n                -follicles 1\n                -nCloths 1\n                -nParticles 1\n                -nRigids 1\n                -dynamicConstraints 1\n                -locators 1\n                -manipulators 1\n                -dimensions 1\n                -handles 1\n                -pivots 1\n                -textures 1\n                -strokes 1\n                -motionTrails 1\n                -shadows 0\n                $editorName;\nmodelEditor -e -viewSelected 0 $editorName;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tmodelPanel -edit -l (localizedPanelLabel(\"Top View\")) -mbv $menusOkayInPanels  $panelName;\n\t\t$editorName = $panelName;\n        modelEditor -e \n            -camera \"top\" \n            -useInteractiveMode 0\n            -displayLights \"default\" \n"
		+ "            -displayAppearance \"wireframe\" \n            -activeOnly 0\n            -ignorePanZoom 0\n            -wireframeOnShaded 0\n            -headsUpDisplay 1\n            -selectionHiliteDisplay 1\n            -useDefaultMaterial 0\n            -bufferMode \"double\" \n            -twoSidedLighting 1\n            -backfaceCulling 0\n            -xray 0\n            -jointXray 0\n            -activeComponentsXray 0\n            -displayTextures 0\n            -smoothWireframe 0\n            -lineWidth 1\n            -textureAnisotropic 0\n            -textureHilight 1\n            -textureSampling 2\n            -textureDisplay \"modulate\" \n            -textureMaxSize 16384\n            -fogging 0\n            -fogSource \"fragment\" \n            -fogMode \"linear\" \n            -fogStart 0\n            -fogEnd 100\n            -fogDensity 0.1\n            -fogColor 0.5 0.5 0.5 1 \n            -maxConstantTransparency 1\n            -rendererName \"base_OpenGL_Renderer\" \n            -colorResolution 256 256 \n            -bumpResolution 512 512 \n"
		+ "            -textureCompression 0\n            -transparencyAlgorithm \"frontAndBackCull\" \n            -transpInShadows 0\n            -cullingOverride \"none\" \n            -lowQualityLighting 0\n            -maximumNumHardwareLights 0\n            -occlusionCulling 0\n            -shadingModel 0\n            -useBaseRenderer 0\n            -useReducedRenderer 0\n            -smallObjectCulling 0\n            -smallObjectThreshold -1 \n            -interactiveDisableShadows 0\n            -interactiveBackFaceCull 0\n            -sortTransparent 1\n            -nurbsCurves 1\n            -nurbsSurfaces 1\n            -polymeshes 1\n            -subdivSurfaces 1\n            -planes 1\n            -lights 1\n            -cameras 1\n            -controlVertices 1\n            -hulls 1\n            -grid 1\n            -joints 1\n            -ikHandles 1\n            -deformers 1\n            -dynamics 1\n            -fluids 1\n            -hairSystems 1\n            -follicles 1\n            -nCloths 1\n            -nParticles 1\n            -nRigids 1\n"
		+ "            -dynamicConstraints 1\n            -locators 1\n            -manipulators 1\n            -dimensions 1\n            -handles 1\n            -pivots 1\n            -textures 1\n            -strokes 1\n            -motionTrails 1\n            -shadows 0\n            $editorName;\nmodelEditor -e -viewSelected 0 $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"modelPanel\" (localizedPanelLabel(\"Side View\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `modelPanel -unParent -l (localizedPanelLabel(\"Side View\")) -mbv $menusOkayInPanels `;\n\t\t\t$editorName = $panelName;\n            modelEditor -e \n                -camera \"side\" \n                -useInteractiveMode 0\n                -displayLights \"default\" \n                -displayAppearance \"wireframe\" \n                -activeOnly 0\n                -ignorePanZoom 0\n                -wireframeOnShaded 0\n                -headsUpDisplay 1\n                -selectionHiliteDisplay 1\n"
		+ "                -useDefaultMaterial 0\n                -bufferMode \"double\" \n                -twoSidedLighting 1\n                -backfaceCulling 0\n                -xray 0\n                -jointXray 0\n                -activeComponentsXray 0\n                -displayTextures 0\n                -smoothWireframe 0\n                -lineWidth 1\n                -textureAnisotropic 0\n                -textureHilight 1\n                -textureSampling 2\n                -textureDisplay \"modulate\" \n                -textureMaxSize 16384\n                -fogging 0\n                -fogSource \"fragment\" \n                -fogMode \"linear\" \n                -fogStart 0\n                -fogEnd 100\n                -fogDensity 0.1\n                -fogColor 0.5 0.5 0.5 1 \n                -maxConstantTransparency 1\n                -rendererName \"base_OpenGL_Renderer\" \n                -colorResolution 256 256 \n                -bumpResolution 512 512 \n                -textureCompression 0\n                -transparencyAlgorithm \"frontAndBackCull\" \n"
		+ "                -transpInShadows 0\n                -cullingOverride \"none\" \n                -lowQualityLighting 0\n                -maximumNumHardwareLights 0\n                -occlusionCulling 0\n                -shadingModel 0\n                -useBaseRenderer 0\n                -useReducedRenderer 0\n                -smallObjectCulling 0\n                -smallObjectThreshold -1 \n                -interactiveDisableShadows 0\n                -interactiveBackFaceCull 0\n                -sortTransparent 1\n                -nurbsCurves 1\n                -nurbsSurfaces 1\n                -polymeshes 1\n                -subdivSurfaces 1\n                -planes 1\n                -lights 1\n                -cameras 1\n                -controlVertices 1\n                -hulls 1\n                -grid 1\n                -joints 1\n                -ikHandles 1\n                -deformers 1\n                -dynamics 1\n                -fluids 1\n                -hairSystems 1\n                -follicles 1\n                -nCloths 1\n                -nParticles 1\n"
		+ "                -nRigids 1\n                -dynamicConstraints 1\n                -locators 1\n                -manipulators 1\n                -dimensions 1\n                -handles 1\n                -pivots 1\n                -textures 1\n                -strokes 1\n                -motionTrails 1\n                -shadows 0\n                $editorName;\nmodelEditor -e -viewSelected 0 $editorName;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tmodelPanel -edit -l (localizedPanelLabel(\"Side View\")) -mbv $menusOkayInPanels  $panelName;\n\t\t$editorName = $panelName;\n        modelEditor -e \n            -camera \"side\" \n            -useInteractiveMode 0\n            -displayLights \"default\" \n            -displayAppearance \"wireframe\" \n            -activeOnly 0\n            -ignorePanZoom 0\n            -wireframeOnShaded 0\n            -headsUpDisplay 1\n            -selectionHiliteDisplay 1\n            -useDefaultMaterial 0\n            -bufferMode \"double\" \n            -twoSidedLighting 1\n            -backfaceCulling 0\n"
		+ "            -xray 0\n            -jointXray 0\n            -activeComponentsXray 0\n            -displayTextures 0\n            -smoothWireframe 0\n            -lineWidth 1\n            -textureAnisotropic 0\n            -textureHilight 1\n            -textureSampling 2\n            -textureDisplay \"modulate\" \n            -textureMaxSize 16384\n            -fogging 0\n            -fogSource \"fragment\" \n            -fogMode \"linear\" \n            -fogStart 0\n            -fogEnd 100\n            -fogDensity 0.1\n            -fogColor 0.5 0.5 0.5 1 \n            -maxConstantTransparency 1\n            -rendererName \"base_OpenGL_Renderer\" \n            -colorResolution 256 256 \n            -bumpResolution 512 512 \n            -textureCompression 0\n            -transparencyAlgorithm \"frontAndBackCull\" \n            -transpInShadows 0\n            -cullingOverride \"none\" \n            -lowQualityLighting 0\n            -maximumNumHardwareLights 0\n            -occlusionCulling 0\n            -shadingModel 0\n            -useBaseRenderer 0\n"
		+ "            -useReducedRenderer 0\n            -smallObjectCulling 0\n            -smallObjectThreshold -1 \n            -interactiveDisableShadows 0\n            -interactiveBackFaceCull 0\n            -sortTransparent 1\n            -nurbsCurves 1\n            -nurbsSurfaces 1\n            -polymeshes 1\n            -subdivSurfaces 1\n            -planes 1\n            -lights 1\n            -cameras 1\n            -controlVertices 1\n            -hulls 1\n            -grid 1\n            -joints 1\n            -ikHandles 1\n            -deformers 1\n            -dynamics 1\n            -fluids 1\n            -hairSystems 1\n            -follicles 1\n            -nCloths 1\n            -nParticles 1\n            -nRigids 1\n            -dynamicConstraints 1\n            -locators 1\n            -manipulators 1\n            -dimensions 1\n            -handles 1\n            -pivots 1\n            -textures 1\n            -strokes 1\n            -motionTrails 1\n            -shadows 0\n            $editorName;\nmodelEditor -e -viewSelected 0 $editorName;\n"
		+ "\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"modelPanel\" (localizedPanelLabel(\"Front View\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `modelPanel -unParent -l (localizedPanelLabel(\"Front View\")) -mbv $menusOkayInPanels `;\n\t\t\t$editorName = $panelName;\n            modelEditor -e \n                -camera \"front\" \n                -useInteractiveMode 0\n                -displayLights \"default\" \n                -displayAppearance \"wireframe\" \n                -activeOnly 0\n                -ignorePanZoom 0\n                -wireframeOnShaded 0\n                -headsUpDisplay 1\n                -selectionHiliteDisplay 1\n                -useDefaultMaterial 0\n                -bufferMode \"double\" \n                -twoSidedLighting 1\n                -backfaceCulling 0\n                -xray 0\n                -jointXray 0\n                -activeComponentsXray 0\n                -displayTextures 0\n                -smoothWireframe 0\n"
		+ "                -lineWidth 1\n                -textureAnisotropic 0\n                -textureHilight 1\n                -textureSampling 2\n                -textureDisplay \"modulate\" \n                -textureMaxSize 16384\n                -fogging 0\n                -fogSource \"fragment\" \n                -fogMode \"linear\" \n                -fogStart 0\n                -fogEnd 100\n                -fogDensity 0.1\n                -fogColor 0.5 0.5 0.5 1 \n                -maxConstantTransparency 1\n                -rendererName \"base_OpenGL_Renderer\" \n                -colorResolution 256 256 \n                -bumpResolution 512 512 \n                -textureCompression 0\n                -transparencyAlgorithm \"frontAndBackCull\" \n                -transpInShadows 0\n                -cullingOverride \"none\" \n                -lowQualityLighting 0\n                -maximumNumHardwareLights 0\n                -occlusionCulling 0\n                -shadingModel 0\n                -useBaseRenderer 0\n                -useReducedRenderer 0\n                -smallObjectCulling 0\n"
		+ "                -smallObjectThreshold -1 \n                -interactiveDisableShadows 0\n                -interactiveBackFaceCull 0\n                -sortTransparent 1\n                -nurbsCurves 1\n                -nurbsSurfaces 1\n                -polymeshes 1\n                -subdivSurfaces 1\n                -planes 1\n                -lights 1\n                -cameras 1\n                -controlVertices 1\n                -hulls 1\n                -grid 1\n                -joints 1\n                -ikHandles 1\n                -deformers 1\n                -dynamics 1\n                -fluids 1\n                -hairSystems 1\n                -follicles 1\n                -nCloths 1\n                -nParticles 1\n                -nRigids 1\n                -dynamicConstraints 1\n                -locators 1\n                -manipulators 1\n                -dimensions 1\n                -handles 1\n                -pivots 1\n                -textures 1\n                -strokes 1\n                -motionTrails 1\n                -shadows 0\n"
		+ "                $editorName;\nmodelEditor -e -viewSelected 0 $editorName;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tmodelPanel -edit -l (localizedPanelLabel(\"Front View\")) -mbv $menusOkayInPanels  $panelName;\n\t\t$editorName = $panelName;\n        modelEditor -e \n            -camera \"front\" \n            -useInteractiveMode 0\n            -displayLights \"default\" \n            -displayAppearance \"wireframe\" \n            -activeOnly 0\n            -ignorePanZoom 0\n            -wireframeOnShaded 0\n            -headsUpDisplay 1\n            -selectionHiliteDisplay 1\n            -useDefaultMaterial 0\n            -bufferMode \"double\" \n            -twoSidedLighting 1\n            -backfaceCulling 0\n            -xray 0\n            -jointXray 0\n            -activeComponentsXray 0\n            -displayTextures 0\n            -smoothWireframe 0\n            -lineWidth 1\n            -textureAnisotropic 0\n            -textureHilight 1\n            -textureSampling 2\n            -textureDisplay \"modulate\" \n            -textureMaxSize 16384\n"
		+ "            -fogging 0\n            -fogSource \"fragment\" \n            -fogMode \"linear\" \n            -fogStart 0\n            -fogEnd 100\n            -fogDensity 0.1\n            -fogColor 0.5 0.5 0.5 1 \n            -maxConstantTransparency 1\n            -rendererName \"base_OpenGL_Renderer\" \n            -colorResolution 256 256 \n            -bumpResolution 512 512 \n            -textureCompression 0\n            -transparencyAlgorithm \"frontAndBackCull\" \n            -transpInShadows 0\n            -cullingOverride \"none\" \n            -lowQualityLighting 0\n            -maximumNumHardwareLights 0\n            -occlusionCulling 0\n            -shadingModel 0\n            -useBaseRenderer 0\n            -useReducedRenderer 0\n            -smallObjectCulling 0\n            -smallObjectThreshold -1 \n            -interactiveDisableShadows 0\n            -interactiveBackFaceCull 0\n            -sortTransparent 1\n            -nurbsCurves 1\n            -nurbsSurfaces 1\n            -polymeshes 1\n            -subdivSurfaces 1\n            -planes 1\n"
		+ "            -lights 1\n            -cameras 1\n            -controlVertices 1\n            -hulls 1\n            -grid 1\n            -joints 1\n            -ikHandles 1\n            -deformers 1\n            -dynamics 1\n            -fluids 1\n            -hairSystems 1\n            -follicles 1\n            -nCloths 1\n            -nParticles 1\n            -nRigids 1\n            -dynamicConstraints 1\n            -locators 1\n            -manipulators 1\n            -dimensions 1\n            -handles 1\n            -pivots 1\n            -textures 1\n            -strokes 1\n            -motionTrails 1\n            -shadows 0\n            $editorName;\nmodelEditor -e -viewSelected 0 $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"modelPanel\" (localizedPanelLabel(\"Persp View\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `modelPanel -unParent -l (localizedPanelLabel(\"Persp View\")) -mbv $menusOkayInPanels `;\n\t\t\t$editorName = $panelName;\n"
		+ "            modelEditor -e \n                -camera \"persp\" \n                -useInteractiveMode 0\n                -displayLights \"default\" \n                -displayAppearance \"wireframe\" \n                -activeOnly 0\n                -ignorePanZoom 0\n                -wireframeOnShaded 0\n                -headsUpDisplay 1\n                -selectionHiliteDisplay 1\n                -useDefaultMaterial 0\n                -bufferMode \"double\" \n                -twoSidedLighting 1\n                -backfaceCulling 1\n                -xray 0\n                -jointXray 0\n                -activeComponentsXray 0\n                -displayTextures 1\n                -smoothWireframe 0\n                -lineWidth 1\n                -textureAnisotropic 0\n                -textureHilight 1\n                -textureSampling 2\n                -textureDisplay \"modulate\" \n                -textureMaxSize 16384\n                -fogging 0\n                -fogSource \"fragment\" \n                -fogMode \"linear\" \n                -fogStart 0\n                -fogEnd 100\n"
		+ "                -fogDensity 0.1\n                -fogColor 0.5 0.5 0.5 1 \n                -maxConstantTransparency 1\n                -rendererName \"base_OpenGL_Renderer\" \n                -colorResolution 256 256 \n                -bumpResolution 512 512 \n                -textureCompression 0\n                -transparencyAlgorithm \"perPolygonSort\" \n                -transpInShadows 0\n                -cullingOverride \"none\" \n                -lowQualityLighting 0\n                -maximumNumHardwareLights 1\n                -occlusionCulling 0\n                -shadingModel 0\n                -useBaseRenderer 0\n                -useReducedRenderer 0\n                -smallObjectCulling 0\n                -smallObjectThreshold -1 \n                -interactiveDisableShadows 0\n                -interactiveBackFaceCull 0\n                -sortTransparent 1\n                -nurbsCurves 1\n                -nurbsSurfaces 1\n                -polymeshes 1\n                -subdivSurfaces 1\n                -planes 1\n                -lights 1\n"
		+ "                -cameras 0\n                -controlVertices 1\n                -hulls 1\n                -grid 0\n                -joints 1\n                -ikHandles 1\n                -deformers 1\n                -dynamics 1\n                -fluids 1\n                -hairSystems 1\n                -follicles 1\n                -nCloths 1\n                -nParticles 1\n                -nRigids 1\n                -dynamicConstraints 1\n                -locators 1\n                -manipulators 1\n                -dimensions 1\n                -handles 1\n                -pivots 1\n                -textures 1\n                -strokes 1\n                -motionTrails 1\n                -shadows 0\n                $editorName;\nmodelEditor -e -viewSelected 0 $editorName;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tmodelPanel -edit -l (localizedPanelLabel(\"Persp View\")) -mbv $menusOkayInPanels  $panelName;\n\t\t$editorName = $panelName;\n        modelEditor -e \n            -camera \"persp\" \n            -useInteractiveMode 0\n"
		+ "            -displayLights \"default\" \n            -displayAppearance \"wireframe\" \n            -activeOnly 0\n            -ignorePanZoom 0\n            -wireframeOnShaded 0\n            -headsUpDisplay 1\n            -selectionHiliteDisplay 1\n            -useDefaultMaterial 0\n            -bufferMode \"double\" \n            -twoSidedLighting 1\n            -backfaceCulling 1\n            -xray 0\n            -jointXray 0\n            -activeComponentsXray 0\n            -displayTextures 1\n            -smoothWireframe 0\n            -lineWidth 1\n            -textureAnisotropic 0\n            -textureHilight 1\n            -textureSampling 2\n            -textureDisplay \"modulate\" \n            -textureMaxSize 16384\n            -fogging 0\n            -fogSource \"fragment\" \n            -fogMode \"linear\" \n            -fogStart 0\n            -fogEnd 100\n            -fogDensity 0.1\n            -fogColor 0.5 0.5 0.5 1 \n            -maxConstantTransparency 1\n            -rendererName \"base_OpenGL_Renderer\" \n            -colorResolution 256 256 \n"
		+ "            -bumpResolution 512 512 \n            -textureCompression 0\n            -transparencyAlgorithm \"perPolygonSort\" \n            -transpInShadows 0\n            -cullingOverride \"none\" \n            -lowQualityLighting 0\n            -maximumNumHardwareLights 1\n            -occlusionCulling 0\n            -shadingModel 0\n            -useBaseRenderer 0\n            -useReducedRenderer 0\n            -smallObjectCulling 0\n            -smallObjectThreshold -1 \n            -interactiveDisableShadows 0\n            -interactiveBackFaceCull 0\n            -sortTransparent 1\n            -nurbsCurves 1\n            -nurbsSurfaces 1\n            -polymeshes 1\n            -subdivSurfaces 1\n            -planes 1\n            -lights 1\n            -cameras 0\n            -controlVertices 1\n            -hulls 1\n            -grid 0\n            -joints 1\n            -ikHandles 1\n            -deformers 1\n            -dynamics 1\n            -fluids 1\n            -hairSystems 1\n            -follicles 1\n            -nCloths 1\n            -nParticles 1\n"
		+ "            -nRigids 1\n            -dynamicConstraints 1\n            -locators 1\n            -manipulators 1\n            -dimensions 1\n            -handles 1\n            -pivots 1\n            -textures 1\n            -strokes 1\n            -motionTrails 1\n            -shadows 0\n            $editorName;\nmodelEditor -e -viewSelected 0 $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"outlinerPanel\" (localizedPanelLabel(\"Outliner\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `outlinerPanel -unParent -l (localizedPanelLabel(\"Outliner\")) -mbv $menusOkayInPanels `;\n\t\t\t$editorName = $panelName;\n            outlinerEditor -e \n                -showShapes 0\n                -showAttributes 0\n                -showConnected 0\n                -showAnimCurvesOnly 0\n                -showMuteInfo 0\n                -organizeByLayer 1\n                -showAnimLayerWeight 1\n                -autoExpandLayers 1\n                -autoExpand 0\n"
		+ "                -showDagOnly 1\n                -showAssets 1\n                -showContainedOnly 1\n                -showPublishedAsConnected 0\n                -showContainerContents 1\n                -ignoreDagHierarchy 0\n                -expandConnections 0\n                -showUpstreamCurves 1\n                -showUnitlessCurves 1\n                -showCompounds 1\n                -showLeafs 1\n                -showNumericAttrsOnly 0\n                -highlightActive 1\n                -autoSelectNewObjects 0\n                -doNotSelectNewObjects 0\n                -dropIsParent 1\n                -transmitFilters 0\n                -setFilter \"defaultSetFilter\" \n                -showSetMembers 1\n                -allowMultiSelection 1\n                -alwaysToggleSelect 0\n                -directSelect 0\n                -displayMode \"DAG\" \n                -expandObjects 0\n                -setsIgnoreFilters 1\n                -containersIgnoreFilters 0\n                -editAttrName 0\n                -showAttrValues 0\n                -highlightSecondary 0\n"
		+ "                -showUVAttrsOnly 0\n                -showTextureNodesOnly 0\n                -attrAlphaOrder \"default\" \n                -animLayerFilterOptions \"allAffecting\" \n                -sortOrder \"none\" \n                -longNames 0\n                -niceNames 1\n                -showNamespace 1\n                -showPinIcons 0\n                -mapMotionTrails 0\n                $editorName;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\toutlinerPanel -edit -l (localizedPanelLabel(\"Outliner\")) -mbv $menusOkayInPanels  $panelName;\n\t\t$editorName = $panelName;\n        outlinerEditor -e \n            -showShapes 0\n            -showAttributes 0\n            -showConnected 0\n            -showAnimCurvesOnly 0\n            -showMuteInfo 0\n            -organizeByLayer 1\n            -showAnimLayerWeight 1\n            -autoExpandLayers 1\n            -autoExpand 0\n            -showDagOnly 1\n            -showAssets 1\n            -showContainedOnly 1\n            -showPublishedAsConnected 0\n            -showContainerContents 1\n"
		+ "            -ignoreDagHierarchy 0\n            -expandConnections 0\n            -showUpstreamCurves 1\n            -showUnitlessCurves 1\n            -showCompounds 1\n            -showLeafs 1\n            -showNumericAttrsOnly 0\n            -highlightActive 1\n            -autoSelectNewObjects 0\n            -doNotSelectNewObjects 0\n            -dropIsParent 1\n            -transmitFilters 0\n            -setFilter \"defaultSetFilter\" \n            -showSetMembers 1\n            -allowMultiSelection 1\n            -alwaysToggleSelect 0\n            -directSelect 0\n            -displayMode \"DAG\" \n            -expandObjects 0\n            -setsIgnoreFilters 1\n            -containersIgnoreFilters 0\n            -editAttrName 0\n            -showAttrValues 0\n            -highlightSecondary 0\n            -showUVAttrsOnly 0\n            -showTextureNodesOnly 0\n            -attrAlphaOrder \"default\" \n            -animLayerFilterOptions \"allAffecting\" \n            -sortOrder \"none\" \n            -longNames 0\n            -niceNames 1\n            -showNamespace 1\n"
		+ "            -showPinIcons 0\n            -mapMotionTrails 0\n            $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\tif ($useSceneConfig) {\n\t\toutlinerPanel -e -to $panelName;\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"graphEditor\" (localizedPanelLabel(\"Graph Editor\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"graphEditor\" -l (localizedPanelLabel(\"Graph Editor\")) -mbv $menusOkayInPanels `;\n\n\t\t\t$editorName = ($panelName+\"OutlineEd\");\n            outlinerEditor -e \n                -showShapes 1\n                -showAttributes 1\n                -showConnected 1\n                -showAnimCurvesOnly 1\n                -showMuteInfo 0\n                -organizeByLayer 1\n                -showAnimLayerWeight 1\n                -autoExpandLayers 1\n                -autoExpand 1\n                -showDagOnly 0\n                -showAssets 1\n                -showContainedOnly 0\n                -showPublishedAsConnected 0\n"
		+ "                -showContainerContents 0\n                -ignoreDagHierarchy 0\n                -expandConnections 1\n                -showUpstreamCurves 1\n                -showUnitlessCurves 1\n                -showCompounds 0\n                -showLeafs 1\n                -showNumericAttrsOnly 1\n                -highlightActive 0\n                -autoSelectNewObjects 1\n                -doNotSelectNewObjects 0\n                -dropIsParent 1\n                -transmitFilters 1\n                -setFilter \"0\" \n                -showSetMembers 0\n                -allowMultiSelection 1\n                -alwaysToggleSelect 0\n                -directSelect 0\n                -displayMode \"DAG\" \n                -expandObjects 0\n                -setsIgnoreFilters 1\n                -containersIgnoreFilters 0\n                -editAttrName 0\n                -showAttrValues 0\n                -highlightSecondary 0\n                -showUVAttrsOnly 0\n                -showTextureNodesOnly 0\n                -attrAlphaOrder \"default\" \n                -animLayerFilterOptions \"allAffecting\" \n"
		+ "                -sortOrder \"none\" \n                -longNames 0\n                -niceNames 1\n                -showNamespace 1\n                -showPinIcons 1\n                -mapMotionTrails 1\n                $editorName;\n\n\t\t\t$editorName = ($panelName+\"GraphEd\");\n            animCurveEditor -e \n                -displayKeys 1\n                -displayTangents 0\n                -displayActiveKeys 0\n                -displayActiveKeyTangents 1\n                -displayInfinities 0\n                -autoFit 0\n                -snapTime \"integer\" \n                -snapValue \"none\" \n                -showResults \"off\" \n                -showBufferCurves \"off\" \n                -smoothness \"fine\" \n                -resultSamples 1.25\n                -resultScreenSamples 0\n                -resultUpdate \"delayed\" \n                -showUpstreamCurves 1\n                -stackedCurves 0\n                -stackedCurvesMin -1\n                -stackedCurvesMax 1\n                -stackedCurvesSpace 0.2\n                -displayNormalized 0\n"
		+ "                -preSelectionHighlight 0\n                -constrainDrag 0\n                -classicMode 0\n                $editorName;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Graph Editor\")) -mbv $menusOkayInPanels  $panelName;\n\n\t\t\t$editorName = ($panelName+\"OutlineEd\");\n            outlinerEditor -e \n                -showShapes 1\n                -showAttributes 1\n                -showConnected 1\n                -showAnimCurvesOnly 1\n                -showMuteInfo 0\n                -organizeByLayer 1\n                -showAnimLayerWeight 1\n                -autoExpandLayers 1\n                -autoExpand 1\n                -showDagOnly 0\n                -showAssets 1\n                -showContainedOnly 0\n                -showPublishedAsConnected 0\n                -showContainerContents 0\n                -ignoreDagHierarchy 0\n                -expandConnections 1\n                -showUpstreamCurves 1\n                -showUnitlessCurves 1\n                -showCompounds 0\n"
		+ "                -showLeafs 1\n                -showNumericAttrsOnly 1\n                -highlightActive 0\n                -autoSelectNewObjects 1\n                -doNotSelectNewObjects 0\n                -dropIsParent 1\n                -transmitFilters 1\n                -setFilter \"0\" \n                -showSetMembers 0\n                -allowMultiSelection 1\n                -alwaysToggleSelect 0\n                -directSelect 0\n                -displayMode \"DAG\" \n                -expandObjects 0\n                -setsIgnoreFilters 1\n                -containersIgnoreFilters 0\n                -editAttrName 0\n                -showAttrValues 0\n                -highlightSecondary 0\n                -showUVAttrsOnly 0\n                -showTextureNodesOnly 0\n                -attrAlphaOrder \"default\" \n                -animLayerFilterOptions \"allAffecting\" \n                -sortOrder \"none\" \n                -longNames 0\n                -niceNames 1\n                -showNamespace 1\n                -showPinIcons 1\n                -mapMotionTrails 1\n"
		+ "                $editorName;\n\n\t\t\t$editorName = ($panelName+\"GraphEd\");\n            animCurveEditor -e \n                -displayKeys 1\n                -displayTangents 0\n                -displayActiveKeys 0\n                -displayActiveKeyTangents 1\n                -displayInfinities 0\n                -autoFit 0\n                -snapTime \"integer\" \n                -snapValue \"none\" \n                -showResults \"off\" \n                -showBufferCurves \"off\" \n                -smoothness \"fine\" \n                -resultSamples 1.25\n                -resultScreenSamples 0\n                -resultUpdate \"delayed\" \n                -showUpstreamCurves 1\n                -stackedCurves 0\n                -stackedCurvesMin -1\n                -stackedCurvesMax 1\n                -stackedCurvesSpace 0.2\n                -displayNormalized 0\n                -preSelectionHighlight 0\n                -constrainDrag 0\n                -classicMode 0\n                $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n"
		+ "\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"dopeSheetPanel\" (localizedPanelLabel(\"Dope Sheet\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"dopeSheetPanel\" -l (localizedPanelLabel(\"Dope Sheet\")) -mbv $menusOkayInPanels `;\n\n\t\t\t$editorName = ($panelName+\"OutlineEd\");\n            outlinerEditor -e \n                -showShapes 1\n                -showAttributes 1\n                -showConnected 1\n                -showAnimCurvesOnly 1\n                -showMuteInfo 0\n                -organizeByLayer 1\n                -showAnimLayerWeight 1\n                -autoExpandLayers 1\n                -autoExpand 0\n                -showDagOnly 0\n                -showAssets 1\n                -showContainedOnly 0\n                -showPublishedAsConnected 0\n                -showContainerContents 0\n                -ignoreDagHierarchy 0\n                -expandConnections 1\n                -showUpstreamCurves 1\n                -showUnitlessCurves 0\n                -showCompounds 1\n"
		+ "                -showLeafs 1\n                -showNumericAttrsOnly 1\n                -highlightActive 0\n                -autoSelectNewObjects 0\n                -doNotSelectNewObjects 1\n                -dropIsParent 1\n                -transmitFilters 0\n                -setFilter \"0\" \n                -showSetMembers 0\n                -allowMultiSelection 1\n                -alwaysToggleSelect 0\n                -directSelect 0\n                -displayMode \"DAG\" \n                -expandObjects 0\n                -setsIgnoreFilters 1\n                -containersIgnoreFilters 0\n                -editAttrName 0\n                -showAttrValues 0\n                -highlightSecondary 0\n                -showUVAttrsOnly 0\n                -showTextureNodesOnly 0\n                -attrAlphaOrder \"default\" \n                -animLayerFilterOptions \"allAffecting\" \n                -sortOrder \"none\" \n                -longNames 0\n                -niceNames 1\n                -showNamespace 1\n                -showPinIcons 0\n                -mapMotionTrails 1\n"
		+ "                $editorName;\n\n\t\t\t$editorName = ($panelName+\"DopeSheetEd\");\n            dopeSheetEditor -e \n                -displayKeys 1\n                -displayTangents 0\n                -displayActiveKeys 0\n                -displayActiveKeyTangents 0\n                -displayInfinities 0\n                -autoFit 0\n                -snapTime \"integer\" \n                -snapValue \"none\" \n                -outliner \"dopeSheetPanel1OutlineEd\" \n                -showSummary 1\n                -showScene 0\n                -hierarchyBelow 0\n                -showTicks 1\n                -selectionWindow 0 0 0 0 \n                $editorName;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Dope Sheet\")) -mbv $menusOkayInPanels  $panelName;\n\n\t\t\t$editorName = ($panelName+\"OutlineEd\");\n            outlinerEditor -e \n                -showShapes 1\n                -showAttributes 1\n                -showConnected 1\n                -showAnimCurvesOnly 1\n                -showMuteInfo 0\n"
		+ "                -organizeByLayer 1\n                -showAnimLayerWeight 1\n                -autoExpandLayers 1\n                -autoExpand 0\n                -showDagOnly 0\n                -showAssets 1\n                -showContainedOnly 0\n                -showPublishedAsConnected 0\n                -showContainerContents 0\n                -ignoreDagHierarchy 0\n                -expandConnections 1\n                -showUpstreamCurves 1\n                -showUnitlessCurves 0\n                -showCompounds 1\n                -showLeafs 1\n                -showNumericAttrsOnly 1\n                -highlightActive 0\n                -autoSelectNewObjects 0\n                -doNotSelectNewObjects 1\n                -dropIsParent 1\n                -transmitFilters 0\n                -setFilter \"0\" \n                -showSetMembers 0\n                -allowMultiSelection 1\n                -alwaysToggleSelect 0\n                -directSelect 0\n                -displayMode \"DAG\" \n                -expandObjects 0\n                -setsIgnoreFilters 1\n"
		+ "                -containersIgnoreFilters 0\n                -editAttrName 0\n                -showAttrValues 0\n                -highlightSecondary 0\n                -showUVAttrsOnly 0\n                -showTextureNodesOnly 0\n                -attrAlphaOrder \"default\" \n                -animLayerFilterOptions \"allAffecting\" \n                -sortOrder \"none\" \n                -longNames 0\n                -niceNames 1\n                -showNamespace 1\n                -showPinIcons 0\n                -mapMotionTrails 1\n                $editorName;\n\n\t\t\t$editorName = ($panelName+\"DopeSheetEd\");\n            dopeSheetEditor -e \n                -displayKeys 1\n                -displayTangents 0\n                -displayActiveKeys 0\n                -displayActiveKeyTangents 0\n                -displayInfinities 0\n                -autoFit 0\n                -snapTime \"integer\" \n                -snapValue \"none\" \n                -outliner \"dopeSheetPanel1OutlineEd\" \n                -showSummary 1\n                -showScene 0\n                -hierarchyBelow 0\n"
		+ "                -showTicks 1\n                -selectionWindow 0 0 0 0 \n                $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"clipEditorPanel\" (localizedPanelLabel(\"Trax Editor\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"clipEditorPanel\" -l (localizedPanelLabel(\"Trax Editor\")) -mbv $menusOkayInPanels `;\n\n\t\t\t$editorName = clipEditorNameFromPanel($panelName);\n            clipEditor -e \n                -displayKeys 0\n                -displayTangents 0\n                -displayActiveKeys 0\n                -displayActiveKeyTangents 0\n                -displayInfinities 0\n                -autoFit 0\n                -snapTime \"none\" \n                -snapValue \"none\" \n                -manageSequencer 0 \n                $editorName;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Trax Editor\")) -mbv $menusOkayInPanels  $panelName;\n"
		+ "\t\t\t$editorName = clipEditorNameFromPanel($panelName);\n            clipEditor -e \n                -displayKeys 0\n                -displayTangents 0\n                -displayActiveKeys 0\n                -displayActiveKeyTangents 0\n                -displayInfinities 0\n                -autoFit 0\n                -snapTime \"none\" \n                -snapValue \"none\" \n                -manageSequencer 0 \n                $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"sequenceEditorPanel\" (localizedPanelLabel(\"Camera Sequencer\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"sequenceEditorPanel\" -l (localizedPanelLabel(\"Camera Sequencer\")) -mbv $menusOkayInPanels `;\n\n\t\t\t$editorName = sequenceEditorNameFromPanel($panelName);\n            clipEditor -e \n                -displayKeys 0\n                -displayTangents 0\n                -displayActiveKeys 0\n                -displayActiveKeyTangents 0\n"
		+ "                -displayInfinities 0\n                -autoFit 0\n                -snapTime \"none\" \n                -snapValue \"none\" \n                -manageSequencer 1 \n                $editorName;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Camera Sequencer\")) -mbv $menusOkayInPanels  $panelName;\n\n\t\t\t$editorName = sequenceEditorNameFromPanel($panelName);\n            clipEditor -e \n                -displayKeys 0\n                -displayTangents 0\n                -displayActiveKeys 0\n                -displayActiveKeyTangents 0\n                -displayInfinities 0\n                -autoFit 0\n                -snapTime \"none\" \n                -snapValue \"none\" \n                -manageSequencer 1 \n                $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"hyperGraphPanel\" (localizedPanelLabel(\"Hypergraph Hierarchy\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n"
		+ "\t\t\t$panelName = `scriptedPanel -unParent  -type \"hyperGraphPanel\" -l (localizedPanelLabel(\"Hypergraph Hierarchy\")) -mbv $menusOkayInPanels `;\n\n\t\t\t$editorName = ($panelName+\"HyperGraphEd\");\n            hyperGraph -e \n                -graphLayoutStyle \"hierarchicalLayout\" \n                -orientation \"horiz\" \n                -mergeConnections 0\n                -zoom 1\n                -animateTransition 0\n                -showRelationships 1\n                -showShapes 0\n                -showDeformers 0\n                -showExpressions 0\n                -showConstraints 0\n                -showUnderworld 0\n                -showInvisible 0\n                -transitionFrames 1\n                -opaqueContainers 0\n                -freeform 0\n                -imagePosition 0 0 \n                -imageScale 1\n                -imageEnabled 0\n                -graphType \"DAG\" \n                -heatMapDisplay 0\n                -updateSelection 1\n                -updateNodeAdded 1\n                -useDrawOverrideColor 0\n                -limitGraphTraversal -1\n"
		+ "                -range 0 0 \n                -iconSize \"smallIcons\" \n                -showCachedConnections 0\n                $editorName;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Hypergraph Hierarchy\")) -mbv $menusOkayInPanels  $panelName;\n\n\t\t\t$editorName = ($panelName+\"HyperGraphEd\");\n            hyperGraph -e \n                -graphLayoutStyle \"hierarchicalLayout\" \n                -orientation \"horiz\" \n                -mergeConnections 0\n                -zoom 1\n                -animateTransition 0\n                -showRelationships 1\n                -showShapes 0\n                -showDeformers 0\n                -showExpressions 0\n                -showConstraints 0\n                -showUnderworld 0\n                -showInvisible 0\n                -transitionFrames 1\n                -opaqueContainers 0\n                -freeform 0\n                -imagePosition 0 0 \n                -imageScale 1\n                -imageEnabled 0\n                -graphType \"DAG\" \n"
		+ "                -heatMapDisplay 0\n                -updateSelection 1\n                -updateNodeAdded 1\n                -useDrawOverrideColor 0\n                -limitGraphTraversal -1\n                -range 0 0 \n                -iconSize \"smallIcons\" \n                -showCachedConnections 0\n                $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"hyperShadePanel\" (localizedPanelLabel(\"Hypershade\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"hyperShadePanel\" -l (localizedPanelLabel(\"Hypershade\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Hypershade\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\tif ($useSceneConfig) {\n\t\tscriptedPanel -e -to $panelName;\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"visorPanel\" (localizedPanelLabel(\"Visor\")) `;\n"
		+ "\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"visorPanel\" -l (localizedPanelLabel(\"Visor\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Visor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"createNodePanel\" (localizedPanelLabel(\"Create Node\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"createNodePanel\" -l (localizedPanelLabel(\"Create Node\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Create Node\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"polyTexturePlacementPanel\" (localizedPanelLabel(\"UV Texture Editor\")) `;\n"
		+ "\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"polyTexturePlacementPanel\" -l (localizedPanelLabel(\"UV Texture Editor\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"UV Texture Editor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\tif ($useSceneConfig) {\n\t\tscriptedPanel -e -to $panelName;\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"renderWindowPanel\" (localizedPanelLabel(\"Render View\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"renderWindowPanel\" -l (localizedPanelLabel(\"Render View\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Render View\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"blendShapePanel\" (localizedPanelLabel(\"Blend Shape\")) `;\n"
		+ "\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\tblendShapePanel -unParent -l (localizedPanelLabel(\"Blend Shape\")) -mbv $menusOkayInPanels ;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tblendShapePanel -edit -l (localizedPanelLabel(\"Blend Shape\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"dynRelEdPanel\" (localizedPanelLabel(\"Dynamic Relationships\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"dynRelEdPanel\" -l (localizedPanelLabel(\"Dynamic Relationships\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Dynamic Relationships\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"relationshipPanel\" (localizedPanelLabel(\"Relationship Editor\")) `;\n"
		+ "\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"relationshipPanel\" -l (localizedPanelLabel(\"Relationship Editor\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Relationship Editor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"referenceEditorPanel\" (localizedPanelLabel(\"Reference Editor\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"referenceEditorPanel\" -l (localizedPanelLabel(\"Reference Editor\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Reference Editor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"componentEditorPanel\" (localizedPanelLabel(\"Component Editor\")) `;\n"
		+ "\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"componentEditorPanel\" -l (localizedPanelLabel(\"Component Editor\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Component Editor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"dynPaintScriptedPanelType\" (localizedPanelLabel(\"Paint Effects\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"dynPaintScriptedPanelType\" -l (localizedPanelLabel(\"Paint Effects\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Paint Effects\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"scriptEditorPanel\" (localizedPanelLabel(\"Script Editor\")) `;\n"
		+ "\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"scriptEditorPanel\" -l (localizedPanelLabel(\"Script Editor\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Script Editor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\tif ($useSceneConfig) {\n        string $configName = `getPanel -cwl (localizedPanelLabel(\"Current Layout\"))`;\n        if (\"\" != $configName) {\n\t\t\tpanelConfiguration -edit -label (localizedPanelLabel(\"Current Layout\")) \n\t\t\t\t-defaultImage \"\"\n\t\t\t\t-image \"\"\n\t\t\t\t-sc false\n\t\t\t\t-configString \"global string $gMainPane; paneLayout -e -cn \\\"single\\\" -ps 1 100 100 $gMainPane;\"\n\t\t\t\t-removeAllPanels\n\t\t\t\t-ap false\n\t\t\t\t\t(localizedPanelLabel(\"Persp View\")) \n\t\t\t\t\t\"modelPanel\"\n"
		+ "\t\t\t\t\t\"$panelName = `modelPanel -unParent -l (localizedPanelLabel(\\\"Persp View\\\")) -mbv $menusOkayInPanels `;\\n$editorName = $panelName;\\nmodelEditor -e \\n    -cam `findStartUpCamera persp` \\n    -useInteractiveMode 0\\n    -displayLights \\\"default\\\" \\n    -displayAppearance \\\"wireframe\\\" \\n    -activeOnly 0\\n    -ignorePanZoom 0\\n    -wireframeOnShaded 0\\n    -headsUpDisplay 1\\n    -selectionHiliteDisplay 1\\n    -useDefaultMaterial 0\\n    -bufferMode \\\"double\\\" \\n    -twoSidedLighting 1\\n    -backfaceCulling 1\\n    -xray 0\\n    -jointXray 0\\n    -activeComponentsXray 0\\n    -displayTextures 1\\n    -smoothWireframe 0\\n    -lineWidth 1\\n    -textureAnisotropic 0\\n    -textureHilight 1\\n    -textureSampling 2\\n    -textureDisplay \\\"modulate\\\" \\n    -textureMaxSize 16384\\n    -fogging 0\\n    -fogSource \\\"fragment\\\" \\n    -fogMode \\\"linear\\\" \\n    -fogStart 0\\n    -fogEnd 100\\n    -fogDensity 0.1\\n    -fogColor 0.5 0.5 0.5 1 \\n    -maxConstantTransparency 1\\n    -rendererName \\\"base_OpenGL_Renderer\\\" \\n    -colorResolution 256 256 \\n    -bumpResolution 512 512 \\n    -textureCompression 0\\n    -transparencyAlgorithm \\\"perPolygonSort\\\" \\n    -transpInShadows 0\\n    -cullingOverride \\\"none\\\" \\n    -lowQualityLighting 0\\n    -maximumNumHardwareLights 1\\n    -occlusionCulling 0\\n    -shadingModel 0\\n    -useBaseRenderer 0\\n    -useReducedRenderer 0\\n    -smallObjectCulling 0\\n    -smallObjectThreshold -1 \\n    -interactiveDisableShadows 0\\n    -interactiveBackFaceCull 0\\n    -sortTransparent 1\\n    -nurbsCurves 1\\n    -nurbsSurfaces 1\\n    -polymeshes 1\\n    -subdivSurfaces 1\\n    -planes 1\\n    -lights 1\\n    -cameras 0\\n    -controlVertices 1\\n    -hulls 1\\n    -grid 0\\n    -joints 1\\n    -ikHandles 1\\n    -deformers 1\\n    -dynamics 1\\n    -fluids 1\\n    -hairSystems 1\\n    -follicles 1\\n    -nCloths 1\\n    -nParticles 1\\n    -nRigids 1\\n    -dynamicConstraints 1\\n    -locators 1\\n    -manipulators 1\\n    -dimensions 1\\n    -handles 1\\n    -pivots 1\\n    -textures 1\\n    -strokes 1\\n    -motionTrails 1\\n    -shadows 0\\n    $editorName;\\nmodelEditor -e -viewSelected 0 $editorName\"\n"
		+ "\t\t\t\t\t\"modelPanel -edit -l (localizedPanelLabel(\\\"Persp View\\\")) -mbv $menusOkayInPanels  $panelName;\\n$editorName = $panelName;\\nmodelEditor -e \\n    -cam `findStartUpCamera persp` \\n    -useInteractiveMode 0\\n    -displayLights \\\"default\\\" \\n    -displayAppearance \\\"wireframe\\\" \\n    -activeOnly 0\\n    -ignorePanZoom 0\\n    -wireframeOnShaded 0\\n    -headsUpDisplay 1\\n    -selectionHiliteDisplay 1\\n    -useDefaultMaterial 0\\n    -bufferMode \\\"double\\\" \\n    -twoSidedLighting 1\\n    -backfaceCulling 1\\n    -xray 0\\n    -jointXray 0\\n    -activeComponentsXray 0\\n    -displayTextures 1\\n    -smoothWireframe 0\\n    -lineWidth 1\\n    -textureAnisotropic 0\\n    -textureHilight 1\\n    -textureSampling 2\\n    -textureDisplay \\\"modulate\\\" \\n    -textureMaxSize 16384\\n    -fogging 0\\n    -fogSource \\\"fragment\\\" \\n    -fogMode \\\"linear\\\" \\n    -fogStart 0\\n    -fogEnd 100\\n    -fogDensity 0.1\\n    -fogColor 0.5 0.5 0.5 1 \\n    -maxConstantTransparency 1\\n    -rendererName \\\"base_OpenGL_Renderer\\\" \\n    -colorResolution 256 256 \\n    -bumpResolution 512 512 \\n    -textureCompression 0\\n    -transparencyAlgorithm \\\"perPolygonSort\\\" \\n    -transpInShadows 0\\n    -cullingOverride \\\"none\\\" \\n    -lowQualityLighting 0\\n    -maximumNumHardwareLights 1\\n    -occlusionCulling 0\\n    -shadingModel 0\\n    -useBaseRenderer 0\\n    -useReducedRenderer 0\\n    -smallObjectCulling 0\\n    -smallObjectThreshold -1 \\n    -interactiveDisableShadows 0\\n    -interactiveBackFaceCull 0\\n    -sortTransparent 1\\n    -nurbsCurves 1\\n    -nurbsSurfaces 1\\n    -polymeshes 1\\n    -subdivSurfaces 1\\n    -planes 1\\n    -lights 1\\n    -cameras 0\\n    -controlVertices 1\\n    -hulls 1\\n    -grid 0\\n    -joints 1\\n    -ikHandles 1\\n    -deformers 1\\n    -dynamics 1\\n    -fluids 1\\n    -hairSystems 1\\n    -follicles 1\\n    -nCloths 1\\n    -nParticles 1\\n    -nRigids 1\\n    -dynamicConstraints 1\\n    -locators 1\\n    -manipulators 1\\n    -dimensions 1\\n    -handles 1\\n    -pivots 1\\n    -textures 1\\n    -strokes 1\\n    -motionTrails 1\\n    -shadows 0\\n    $editorName;\\nmodelEditor -e -viewSelected 0 $editorName\"\n"
		+ "\t\t\t\t$configName;\n\n            setNamedPanelLayout (localizedPanelLabel(\"Current Layout\"));\n        }\n\n        panelHistory -e -clear mainPanelHistory;\n        setFocus `paneLayout -q -p1 $gMainPane`;\n        sceneUIReplacement -deleteRemaining;\n        sceneUIReplacement -clear;\n\t}\n\n\ngrid -spacing 5 -size 12 -divisions 5 -displayAxes yes -displayGridLines yes -displayDivisionLines yes -displayPerspectiveLabels no -displayOrthographicLabels no -displayAxesBold yes -perspectiveLabelPosition axis -orthographicLabelPosition edge;\nviewManip -drawCompass 0 -compassAngle 0 -frontParameters \"\" -homeParameters \"\" -selectionLockParameters \"\";\n}\n");
	setAttr ".st" 3;
createNode script -n "sceneConfigurationScriptNode";
	setAttr ".b" -type "string" "playbackOptions -min 1 -max 50 -ast 1 -aet 60 ";
	setAttr ".st" 6;
createNode mentalrayItemsList -s -n "mentalrayItemsList";
createNode mentalrayGlobals -s -n "mentalrayGlobals";
	setAttr ".rvb" 3;
	setAttr ".ivb" no;
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
createNode groupId -n "groupId2";
	setAttr ".ihi" 0;
createNode groupId -n "ww_tree_card_02_groupId2";
	setAttr ".ihi" 0;
createNode groupId -n "groupId53";
	setAttr ".ihi" 0;
createNode apfFileNode -n "apfFileNode1";
createNode groupId -n "oz_ec_catacombs_straight_over_b:groupId6";
	setAttr ".ihi" 0;
createNode groupId -n "oz_ec_catacombs_straight_over_b:junk:JUNK_reducedAlphaTreeLeaves:groupId38";
	setAttr ".ihi" 0;
createNode groupId -n "oz_ec_catacombs_straight_over_b:groupId2";
	setAttr ".ihi" 0;
createNode groupId -n "oz_ec_catacombs_straight_over_b:ww_tree_card_01_groupId2";
	setAttr ".ihi" 0;
createNode apfFileNode -n "oz_ec_catacombs_straight_over_b:apfFileNode1";
createNode groupId -n "oz_ec_catacombs_straight_over_b:oz_ec_catacombs_straight_a:groupId2";
	setAttr ".ihi" 0;
createNode groupId -n "oz_ec_catacombs_straight_over_b:oz_ec_catacombs_straight_a:ww_tree_card_02_groupId2";
	setAttr ".ihi" 0;
createNode apfFileNode -n "oz_ec_catacombs_straight_over_b:oz_ec_catacombs_straight_a:apfFileNode1";
createNode shadingEngine -n "oz_ec_catacombs_straight_a1:lambert2SG";
	setAttr ".ihi" 0;
	setAttr -s 21 ".dsm";
	setAttr ".ro" yes;
createNode materialInfo -n "oz_ec_catacombs_straight_a1:materialInfo1";
createNode lambert -n "oz_ec_master_opaque";
createNode file -n "file1";
	setAttr ".ftn" -type "string" "C:/Users/ros001/TempleRunOzNew/Assets/Oz/Textures/GameTextures/EmeraldCity/oz_ec_master_opaque.tga";
createNode place2dTexture -n "oz_ec_catacombs_straight_a1:place2dTexture1";
createNode animCurveTU -n "BridgePiece1_visibility";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  13 1;
createNode animCurveTL -n "BridgePiece1_translateX";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  13 0 22 0.36981343223577368 45 0.87354023845453532;
	setAttr -s 3 ".kit[1:2]"  9 18;
	setAttr -s 3 ".kot[1:2]"  9 18;
createNode animCurveTL -n "BridgePiece1_translateY";
	setAttr ".tan" 1;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  13 0 22 0.18987802236340784 45 -25.660776067663136;
	setAttr -s 3 ".kit[0:2]"  16 1 1;
	setAttr -s 3 ".kot[0:2]"  16 1 1;
	setAttr -s 3 ".kix[1:2]"  0.0051211598329246044 0.00013732856314163655;
	setAttr -s 3 ".kiy[1:2]"  -0.99998694658279419 -1;
	setAttr -s 3 ".kox[1:2]"  0.0051211626268923283 0.00011966706370003521;
	setAttr -s 3 ".koy[1:2]"  -0.99998694658279419 -1;
createNode animCurveTL -n "BridgePiece1_translateZ";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  13 0 22 -0.4131870753744773;
	setAttr -s 2 ".kit[1]"  18;
	setAttr -s 2 ".kot[1]"  18;
createNode animCurveTA -n "BridgePiece1_rotateX";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  13 0 22 -8.7678653831762734 45 -12.431409106159968;
	setAttr -s 3 ".kit[1:2]"  9 18;
	setAttr -s 3 ".kot[1:2]"  9 18;
createNode animCurveTA -n "BridgePiece1_rotateY";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  13 0 22 12.516494068455827 45 43.898364664790194;
	setAttr -s 3 ".kit[1:2]"  9 18;
	setAttr -s 3 ".kot[1:2]"  9 18;
createNode animCurveTA -n "BridgePiece1_rotateZ";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  13 0 22 3.2225453084946278 45 -2.1248061348177676;
	setAttr -s 3 ".kit[1:2]"  9 18;
	setAttr -s 3 ".kot[1:2]"  9 18;
createNode animCurveTU -n "BridgePiece1_scaleX";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  13 1;
createNode animCurveTU -n "BridgePiece1_scaleY";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  13 1;
createNode animCurveTU -n "BridgePiece1_scaleZ";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  13 1;
createNode animCurveTU -n "BridgePiece2_visibility";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  13 1;
createNode animCurveTL -n "BridgePiece2_translateX";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  13 0 45 -0.5591815101872214;
	setAttr -s 2 ".kit[1]"  18;
	setAttr -s 2 ".kot[1]"  18;
createNode animCurveTL -n "BridgePiece2_translateY";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  13 0 45 -25.80846445873755;
	setAttr -s 2 ".kit[1]"  1;
	setAttr -s 2 ".kot[1]"  1;
	setAttr -s 2 ".kix[1]"  0.00013467496319208294;
	setAttr -s 2 ".kiy[1]"  -1;
	setAttr -s 2 ".kox[1]"  0.00011149567581014708;
	setAttr -s 2 ".koy[1]"  -1;
createNode animCurveTL -n "BridgePiece2_translateZ";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  13 0 26 -0.53902054007509947;
	setAttr -s 2 ".kit[1]"  9;
	setAttr -s 2 ".kot[1]"  9;
createNode animCurveTA -n "BridgePiece2_rotateX";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  13 0 26 -9.0565375097696368;
	setAttr -s 2 ".kit[1]"  9;
	setAttr -s 2 ".kot[1]"  9;
createNode animCurveTA -n "BridgePiece2_rotateY";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  13 0 26 -8.8111935873004033;
	setAttr -s 2 ".kit[1]"  9;
	setAttr -s 2 ".kot[1]"  9;
createNode animCurveTA -n "BridgePiece2_rotateZ";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  13 0 26 -21.545882816059859;
	setAttr -s 2 ".kit[1]"  9;
	setAttr -s 2 ".kot[1]"  9;
createNode animCurveTU -n "BridgePiece2_scaleX";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  13 1;
createNode animCurveTU -n "BridgePiece2_scaleY";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  13 1;
createNode animCurveTU -n "BridgePiece2_scaleZ";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  13 1;
createNode animCurveTU -n "BridgePiece3_visibility";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  13 1;
createNode animCurveTL -n "BridgePiece3_translateX";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  13 0 22 -0.64754999462774721 45 -1.2647204216867105;
	setAttr -s 3 ".kit[1:2]"  9 18;
	setAttr -s 3 ".kot[1:2]"  9 18;
createNode animCurveTL -n "BridgePiece3_translateY";
	setAttr ".tan" 1;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  13 0 22 -2.1915182630667833 45 -25.559200865583254;
	setAttr -s 3 ".kit[0:2]"  16 1 1;
	setAttr -s 3 ".kot[0:2]"  16 1 1;
	setAttr -s 3 ".kix[1:2]"  0.0099217304959893227 0.00013783575559500605;
	setAttr -s 3 ".kiy[1:2]"  -0.9999508261680603 -1;
	setAttr -s 3 ".kox[1:2]"  0.0099217304959893227 0.00011547335452632979;
	setAttr -s 3 ".koy[1:2]"  -0.9999508261680603 -1;
createNode animCurveTL -n "BridgePiece3_translateZ";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  13 0;
createNode animCurveTA -n "BridgePiece3_rotateX";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 4 ".ktv[0:3]"  13 0 22 7.2125974471979761 30 -32.982997492188751
		 45 -12.386276421043508;
	setAttr -s 4 ".kit[0:3]"  16 9 10 10;
	setAttr -s 4 ".kot[0:3]"  16 9 10 10;
createNode animCurveTA -n "BridgePiece3_rotateY";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 4 ".ktv[0:3]"  13 0 22 -9.8119629389715257 30 -5.4842566169313791
		 45 -13.632907873813668;
	setAttr -s 4 ".kit[0:3]"  16 9 10 10;
	setAttr -s 4 ".kot[0:3]"  16 9 10 10;
createNode animCurveTA -n "BridgePiece3_rotateZ";
	setAttr ".tan" 10;
	setAttr ".wgt" no;
	setAttr -s 4 ".ktv[0:3]"  13 0 22 -14.430093125575459 30 3.0066654283578713
		 45 -7.3335512011354762;
	setAttr -s 4 ".kit[0:3]"  16 9 10 10;
	setAttr -s 4 ".kot[0:3]"  16 9 10 10;
createNode animCurveTU -n "BridgePiece3_scaleX";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  13 1;
createNode animCurveTU -n "BridgePiece3_scaleY";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  13 1;
createNode animCurveTU -n "BridgePiece3_scaleZ";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  13 1;
createNode animCurveTU -n "BridgePiece4_visibility";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  13 1 45 1;
	setAttr -s 2 ".kit[1]"  9;
	setAttr -s 2 ".kot[1]"  5;
createNode animCurveTL -n "BridgePiece4_translateX";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  13 0 45 -3.9802735080978171;
	setAttr -s 2 ".kit[1]"  18;
	setAttr -s 2 ".kot[1]"  18;
createNode animCurveTL -n "BridgePiece4_translateY";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  13 0 25 -1.9512781708435154 45 -26.80074430003225;
	setAttr -s 3 ".kit[1:2]"  9 1;
	setAttr -s 3 ".kot[1:2]"  9 1;
	setAttr -s 3 ".kix[2]"  0.00012687755224760622;
	setAttr -s 3 ".kiy[2]"  -1;
	setAttr -s 3 ".kox[2]"  0.00010962165833916515;
	setAttr -s 3 ".koy[2]"  -1;
createNode animCurveTL -n "BridgePiece4_translateZ";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  13 0 45 0;
	setAttr -s 2 ".kit[1]"  10;
	setAttr -s 2 ".kot[1]"  10;
createNode animCurveTA -n "BridgePiece4_rotateX";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  13 0 25 39.534592604560309 45 28.892328146478668;
	setAttr -s 3 ".kit[1:2]"  9 10;
	setAttr -s 3 ".kot[1:2]"  9 10;
createNode animCurveTA -n "BridgePiece4_rotateY";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  13 0 25 -7.2416150593238893 45 -14.414583979784757;
	setAttr -s 3 ".kit[1:2]"  9 10;
	setAttr -s 3 ".kot[1:2]"  9 10;
createNode animCurveTA -n "BridgePiece4_rotateZ";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  13 0 25 -15.353678275754476 45 7.5789714862224375;
	setAttr -s 3 ".kit[1:2]"  9 10;
	setAttr -s 3 ".kot[1:2]"  9 10;
createNode animCurveTU -n "BridgePiece4_scaleX";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  13 1 45 1;
	setAttr -s 2 ".kit[1]"  10;
	setAttr -s 2 ".kot[1]"  10;
createNode animCurveTU -n "BridgePiece4_scaleY";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  13 1 45 1;
	setAttr -s 2 ".kit[1]"  10;
	setAttr -s 2 ".kot[1]"  10;
createNode animCurveTU -n "BridgePiece4_scaleZ";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  13 1 45 1;
	setAttr -s 2 ".kit[1]"  10;
	setAttr -s 2 ".kot[1]"  10;
createNode animCurveTU -n "BridgePiece5_visibility";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  13 1 45 1;
	setAttr -s 2 ".kit[1]"  9;
	setAttr -s 2 ".kot[1]"  5;
createNode animCurveTL -n "BridgePiece5_translateX";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  13 0 45 4.8843504007706562;
	setAttr -s 2 ".kit[1]"  18;
	setAttr -s 2 ".kot[1]"  18;
createNode animCurveTL -n "BridgePiece5_translateY";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  13 0 25 -2.661836624928767 45 -28.072942746751174;
	setAttr -s 3 ".kit[1:2]"  10 1;
	setAttr -s 3 ".kot[1:2]"  10 1;
	setAttr -s 3 ".kix[2]"  0.00018567488586995751;
	setAttr -s 3 ".kiy[2]"  -1;
	setAttr -s 3 ".kox[2]"  0.00016645652067381889;
	setAttr -s 3 ".koy[2]"  -1;
createNode animCurveTL -n "BridgePiece5_translateZ";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  13 0 45 0;
	setAttr -s 2 ".kit[1]"  10;
	setAttr -s 2 ".kot[1]"  10;
createNode animCurveTA -n "BridgePiece5_rotateX";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  13 0 25 49.176153561414694 45 77.977849047042042;
	setAttr -s 3 ".kit[1:2]"  9 10;
	setAttr -s 3 ".kot[1:2]"  9 10;
createNode animCurveTA -n "BridgePiece5_rotateY";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  13 0 25 16.950123347727942 45 7.2073288899625068;
	setAttr -s 3 ".kit[1:2]"  9 10;
	setAttr -s 3 ".kot[1:2]"  9 10;
createNode animCurveTA -n "BridgePiece5_rotateZ";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  13 0 25 -2.730421233702637 45 9.2015028219273489;
	setAttr -s 3 ".kit[1:2]"  9 10;
	setAttr -s 3 ".kot[1:2]"  9 10;
createNode animCurveTU -n "BridgePiece5_scaleX";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  13 1 45 1;
	setAttr -s 2 ".kit[1]"  10;
	setAttr -s 2 ".kot[1]"  10;
createNode animCurveTU -n "BridgePiece5_scaleY";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  13 1 45 1;
	setAttr -s 2 ".kit[1]"  10;
	setAttr -s 2 ".kot[1]"  10;
createNode animCurveTU -n "BridgePiece5_scaleZ";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  13 1 45 1;
	setAttr -s 2 ".kit[1]"  10;
	setAttr -s 2 ".kot[1]"  10;
createNode animCurveTU -n "BridgePiece6_visibility";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  13 1;
createNode animCurveTL -n "BridgePiece6_translateX";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  13 0 25 0.75766403285398498 45 1.1704910545727578;
	setAttr -s 3 ".kit[1:2]"  9 18;
	setAttr -s 3 ".kot[1:2]"  9 18;
createNode animCurveTL -n "BridgePiece6_translateY";
	setAttr ".tan" 1;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  13 0 25 -3.2673762530065336 45 -26.011284743459466;
	setAttr -s 3 ".kit[0:2]"  16 1 1;
	setAttr -s 3 ".kot[0:2]"  16 1 1;
	setAttr -s 3 ".kix[1:2]"  0.00060206354828551412 0.00013370030501391739;
	setAttr -s 3 ".kiy[1:2]"  -0.99999988079071045 -1;
	setAttr -s 3 ".kox[1:2]"  0.00060206116177141666 0.00011196016566827893;
	setAttr -s 3 ".koy[1:2]"  -0.99999988079071045 -1;
createNode animCurveTL -n "BridgePiece6_translateZ";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  13 0 26 -0.10657932807613862;
	setAttr -s 2 ".kit[1]"  9;
	setAttr -s 2 ".kot[1]"  9;
createNode animCurveTA -n "BridgePiece6_rotateX";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  13 0 26 32.216682981317788;
	setAttr -s 2 ".kit[1]"  9;
	setAttr -s 2 ".kot[1]"  9;
createNode animCurveTA -n "BridgePiece6_rotateY";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  13 0 26 8.5534947169001825;
	setAttr -s 2 ".kit[1]"  9;
	setAttr -s 2 ".kot[1]"  9;
createNode animCurveTA -n "BridgePiece6_rotateZ";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  13 0 26 2.6278585018055551;
	setAttr -s 2 ".kit[1]"  9;
	setAttr -s 2 ".kot[1]"  9;
createNode animCurveTU -n "BridgePiece6_scaleX";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  13 1;
createNode animCurveTU -n "BridgePiece6_scaleY";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  13 1;
createNode animCurveTU -n "BridgePiece6_scaleZ";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  13 1;
createNode animCurveTL -n "SideBlock12_translateX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 24 -1.0285273698069206;
createNode animCurveTL -n "SideBlock12_translateY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 24 -0.43092193727729361;
createNode animCurveTL -n "SideBlock12_translateZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 24 0.09275555315174952;
createNode animCurveTU -n "SideBlock12_visibility";
	setAttr ".tan" 9;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
	setAttr ".kot[0]"  5;
createNode animCurveTA -n "SideBlock12_rotateX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 24 34.010917289363221;
createNode animCurveTA -n "SideBlock12_rotateY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 24 -1.5575350605509701;
createNode animCurveTA -n "SideBlock12_rotateZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 24 -4.4347857469630911;
createNode animCurveTU -n "SideBlock12_scaleX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTU -n "SideBlock12_scaleY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTU -n "SideBlock12_scaleZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTA -n "SideBlock1_rotateX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 24 39.216333403935913;
createNode animCurveTA -n "SideBlock1_rotateY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 24 15.046472951328729;
createNode animCurveTA -n "SideBlock1_rotateZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 24 -2.3914673221997225;
createNode animCurveTU -n "SideBlock1_visibility";
	setAttr ".tan" 9;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
	setAttr ".kot[0]"  5;
createNode animCurveTL -n "SideBlock1_translateX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 24 1.3011964963705795;
createNode animCurveTL -n "SideBlock1_translateY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 24 -2.0791614647883474;
createNode animCurveTL -n "SideBlock1_translateZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 24 -0.059384488243817549;
createNode animCurveTU -n "SideBlock1_scaleX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTU -n "SideBlock1_scaleY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTU -n "SideBlock1_scaleZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTA -n "SideBlock10_rotateX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 24 32.186239985992835;
createNode animCurveTA -n "SideBlock10_rotateY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 24 -27.084542256848319;
createNode animCurveTA -n "SideBlock10_rotateZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 24 -6.6004827415510965;
createNode animCurveTU -n "SideBlock10_visibility";
	setAttr ".tan" 9;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
	setAttr ".kot[0]"  5;
createNode animCurveTL -n "SideBlock10_translateX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 24 -1.5965665561829741;
createNode animCurveTL -n "SideBlock10_translateY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 24 0.13998266233553217;
createNode animCurveTL -n "SideBlock10_translateZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 24 -0.045082113211127386;
createNode animCurveTU -n "SideBlock10_scaleX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTU -n "SideBlock10_scaleY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTU -n "SideBlock10_scaleZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTL -n "SideBlock9_translateX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 26 -0.0075287543553695085;
createNode animCurveTL -n "SideBlock9_translateY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 26 -1.0860494899034279;
createNode animCurveTL -n "SideBlock9_translateZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 26 0.40616053281122144;
createNode animCurveTU -n "SideBlock9_visibility";
	setAttr ".tan" 9;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
	setAttr ".kot[0]"  5;
createNode animCurveTA -n "SideBlock9_rotateX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 26 25.720131647800361;
createNode animCurveTA -n "SideBlock9_rotateY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 26 -4.3628833869163177;
createNode animCurveTA -n "SideBlock9_rotateZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 26 -1.2635977544383787;
createNode animCurveTU -n "SideBlock9_scaleX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTU -n "SideBlock9_scaleY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTU -n "SideBlock9_scaleZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTL -n "SideBlock8_translateX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 26 -1.544638464173028;
createNode animCurveTL -n "SideBlock8_translateY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 26 0.23709385188375418;
createNode animCurveTL -n "SideBlock8_translateZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 26 -0.40936942051827296;
createNode animCurveTU -n "SideBlock8_visibility";
	setAttr ".tan" 9;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
	setAttr ".kot[0]"  5;
createNode animCurveTA -n "SideBlock8_rotateX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 26 4.6553786941361848;
createNode animCurveTA -n "SideBlock8_rotateY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 26 -11.414736782293723;
createNode animCurveTA -n "SideBlock8_rotateZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 26 12.049784061932339;
createNode animCurveTU -n "SideBlock8_scaleX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTU -n "SideBlock8_scaleY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTU -n "SideBlock8_scaleZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTL -n "SideBlock6_translateX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 0;
createNode animCurveTL -n "SideBlock6_translateY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 0;
createNode animCurveTL -n "SideBlock6_translateZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 0;
createNode animCurveTU -n "SideBlock6_visibility";
	setAttr ".tan" 9;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
	setAttr ".kot[0]"  5;
createNode animCurveTA -n "SideBlock6_rotateX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 0;
createNode animCurveTA -n "SideBlock6_rotateY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 0;
createNode animCurveTA -n "SideBlock6_rotateZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 0;
createNode animCurveTU -n "SideBlock6_scaleX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTU -n "SideBlock6_scaleY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTU -n "SideBlock6_scaleZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTL -n "SideBlock4_translateX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 26 0.8015761985325226;
createNode animCurveTL -n "SideBlock4_translateY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 26 -1.5518995082195193;
createNode animCurveTL -n "SideBlock4_translateZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 26 1.5887712451867959;
createNode animCurveTU -n "SideBlock4_visibility";
	setAttr ".tan" 9;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
	setAttr ".kot[0]"  5;
createNode animCurveTA -n "SideBlock4_rotateX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 0;
createNode animCurveTA -n "SideBlock4_rotateY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 0;
createNode animCurveTA -n "SideBlock4_rotateZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 0;
createNode animCurveTU -n "SideBlock4_scaleX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTU -n "SideBlock4_scaleY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTU -n "SideBlock4_scaleZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTL -n "SideBlock5_translateX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 26 1.8524869304434095;
createNode animCurveTL -n "SideBlock5_translateY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 26 0.83519387504976716;
createNode animCurveTL -n "SideBlock5_translateZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 26 0.40184999876311794;
createNode animCurveTU -n "SideBlock5_visibility";
	setAttr ".tan" 9;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
	setAttr ".kot[0]"  5;
createNode animCurveTA -n "SideBlock5_rotateX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 26 -42.150638257343182;
createNode animCurveTA -n "SideBlock5_rotateY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 26 -15.637469023624138;
createNode animCurveTA -n "SideBlock5_rotateZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 26 -25.18833927047276;
createNode animCurveTU -n "SideBlock5_scaleX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTU -n "SideBlock5_scaleY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTU -n "SideBlock5_scaleZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTL -n "SideBlock3_translateX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 22 1.1351354882028795;
createNode animCurveTL -n "SideBlock3_translateY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 22 -0.62467854861297145;
createNode animCurveTL -n "SideBlock3_translateZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 22 1.3254300894681579;
createNode animCurveTU -n "SideBlock3_visibility";
	setAttr ".tan" 9;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
	setAttr ".kot[0]"  5;
createNode animCurveTA -n "SideBlock3_rotateX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 22 13.937074599505255;
createNode animCurveTA -n "SideBlock3_rotateY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 22 -15.896058325849333;
createNode animCurveTA -n "SideBlock3_rotateZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 22 23.33079930640378;
createNode animCurveTU -n "SideBlock3_scaleX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTU -n "SideBlock3_scaleY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTU -n "SideBlock3_scaleZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTL -n "SideBlock2_translateX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 24 0.88740508454773226;
createNode animCurveTL -n "SideBlock2_translateY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 24 0.99487030023891632;
createNode animCurveTL -n "SideBlock2_translateZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 24 -0.99922180787379578;
createNode animCurveTU -n "SideBlock2_visibility";
	setAttr ".tan" 9;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
	setAttr ".kot[0]"  5;
createNode animCurveTA -n "SideBlock2_rotateX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 24 -14.43625107853194;
createNode animCurveTA -n "SideBlock2_rotateY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 24 1.6233329505830112;
createNode animCurveTA -n "SideBlock2_rotateZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 24 -13.795137340756023;
createNode animCurveTU -n "SideBlock2_scaleX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTU -n "SideBlock2_scaleY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTU -n "SideBlock2_scaleZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTL -n "SideBlock11_translateX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 24 -1.1705035382716382;
createNode animCurveTL -n "SideBlock11_translateY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 24 -0.45240309128596357;
createNode animCurveTL -n "SideBlock11_translateZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 24 0.45970370966553009;
createNode animCurveTU -n "SideBlock11_visibility";
	setAttr ".tan" 9;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
	setAttr ".kot[0]"  5;
createNode animCurveTA -n "SideBlock11_rotateX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 24 -11.627385239088266;
createNode animCurveTA -n "SideBlock11_rotateY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 24 -8.314589269862628;
createNode animCurveTA -n "SideBlock11_rotateZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  15 0 24 19.134805899582915;
createNode animCurveTU -n "SideBlock11_scaleX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTU -n "SideBlock11_scaleY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTU -n "SideBlock11_scaleZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr ".ktv[0]"  15 1;
createNode animCurveTL -n "polySurface15_translateX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  1 0 36 0;
createNode animCurveTL -n "polySurface15_translateY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  1 -0.58236992559758027 36 -43.444549139523303;
createNode animCurveTL -n "polySurface15_translateZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  1 0 36 0;
createNode animCurveTA -n "polySurface15_rotateX";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  1 0 36 0;
createNode animCurveTA -n "polySurface15_rotateY";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  1 -45 36 -45;
createNode animCurveTA -n "polySurface15_rotateZ";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  1 0 36 0;
createNode animCurveTL -n "chain_translateX";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  1 0.07824643816190964 45 0.07824643816190964;
createNode animCurveTL -n "chain_translateY";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 4 ".ktv[0:3]"  1 15.213780764203735 6 16.637469873066244
		 24 15.214 45 15.213780764203735;
	setAttr -s 4 ".kit[1:3]"  9 18 16;
	setAttr -s 4 ".kot[1:3]"  9 18 16;
createNode animCurveTL -n "chain_translateZ";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  1 -9.9863601303289347 45 -9.9863601303289347;
createNode animCurveTU -n "chain_visibility";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  1 1 45 1;
createNode animCurveTA -n "chain_rotateX";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  1 0 45 0;
createNode animCurveTA -n "chain_rotateY";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  1 0 45 0;
createNode animCurveTA -n "chain_rotateZ";
	setAttr ".tan" 9;
	setAttr ".wgt" no;
	setAttr -s 6 ".ktv[0:5]"  1 0 6 -2.0730629000921375 13 3.802466278236357
		 24 -3.84839327561287 33 1.4302707686775427 45 0;
	setAttr -s 6 ".kit[0:5]"  16 9 9 9 9 16;
	setAttr -s 6 ".kot[0:5]"  16 9 9 9 9 16;
createNode animCurveTU -n "chain_scaleX";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  1 1 45 1;
createNode animCurveTU -n "chain_scaleY";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  1 1 45 1;
createNode animCurveTU -n "chain_scaleZ";
	setAttr ".tan" 16;
	setAttr ".wgt" no;
	setAttr -s 2 ".ktv[0:1]"  1 1 45 1;
createNode lambert -n "MoltenSteel";
createNode shadingEngine -n "lambert2SG";
	setAttr ".ihi" 0;
	setAttr ".ro" yes;
createNode materialInfo -n "materialInfo1";
createNode file -n "file2";
	setAttr ".ftn" -type "string" "C:/Users/ros001/TempleRunOzNew/Assets/Oz/Textures/GameTextures/EmeraldCity/MoltenSteel.tga";
createNode place2dTexture -n "place2dTexture1";
select -ne :time1;
	setAttr -av -k on ".cch";
	setAttr -cb on ".ihi";
	setAttr -k on ".nds";
	setAttr -cb on ".bnm";
	setAttr ".o" 1;
	setAttr ".unw" 1;
select -ne :renderPartition;
	setAttr -k on ".cch";
	setAttr -cb on ".ihi";
	setAttr -k on ".nds";
	setAttr -cb on ".bnm";
	setAttr -s 4 ".st";
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
	setAttr -s 4 ".s";
select -ne :defaultTextureList1;
	setAttr -s 2 ".tx";
select -ne :postProcessList1;
	setAttr -k on ".cch";
	setAttr -cb on ".ihi";
	setAttr -k on ".nds";
	setAttr -cb on ".bnm";
	setAttr -s 2 ".p";
select -ne :defaultRenderUtilityList1;
	setAttr -k on ".cch";
	setAttr -k on ".nds";
	setAttr -s 2 ".u";
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
connectAttr "BridgePiece2_visibility.o" "BridgePiece2.v";
connectAttr "BridgePiece2_translateX.o" "BridgePiece2.tx";
connectAttr "BridgePiece2_translateY.o" "BridgePiece2.ty";
connectAttr "BridgePiece2_translateZ.o" "BridgePiece2.tz";
connectAttr "BridgePiece2_rotateX.o" "BridgePiece2.rx";
connectAttr "BridgePiece2_rotateY.o" "BridgePiece2.ry";
connectAttr "BridgePiece2_rotateZ.o" "BridgePiece2.rz";
connectAttr "BridgePiece2_scaleX.o" "BridgePiece2.sx";
connectAttr "BridgePiece2_scaleY.o" "BridgePiece2.sy";
connectAttr "BridgePiece2_scaleZ.o" "BridgePiece2.sz";
connectAttr "SideBlock12_translateX.o" "SideBlock12.tx";
connectAttr "SideBlock12_translateY.o" "SideBlock12.ty";
connectAttr "SideBlock12_translateZ.o" "SideBlock12.tz";
connectAttr "SideBlock12_visibility.o" "SideBlock12.v";
connectAttr "SideBlock12_rotateX.o" "SideBlock12.rx";
connectAttr "SideBlock12_rotateY.o" "SideBlock12.ry";
connectAttr "SideBlock12_rotateZ.o" "SideBlock12.rz";
connectAttr "SideBlock12_scaleX.o" "SideBlock12.sx";
connectAttr "SideBlock12_scaleY.o" "SideBlock12.sy";
connectAttr "SideBlock12_scaleZ.o" "SideBlock12.sz";
connectAttr "BridgePiece1_visibility.o" "BridgePiece1.v";
connectAttr "BridgePiece1_translateX.o" "BridgePiece1.tx";
connectAttr "BridgePiece1_translateY.o" "BridgePiece1.ty";
connectAttr "BridgePiece1_translateZ.o" "BridgePiece1.tz";
connectAttr "BridgePiece1_rotateX.o" "BridgePiece1.rx";
connectAttr "BridgePiece1_rotateY.o" "BridgePiece1.ry";
connectAttr "BridgePiece1_rotateZ.o" "BridgePiece1.rz";
connectAttr "BridgePiece1_scaleX.o" "BridgePiece1.sx";
connectAttr "BridgePiece1_scaleY.o" "BridgePiece1.sy";
connectAttr "BridgePiece1_scaleZ.o" "BridgePiece1.sz";
connectAttr "SideBlock1_rotateX.o" "SideBlock1.rx";
connectAttr "SideBlock1_rotateY.o" "SideBlock1.ry";
connectAttr "SideBlock1_rotateZ.o" "SideBlock1.rz";
connectAttr "SideBlock1_visibility.o" "SideBlock1.v";
connectAttr "SideBlock1_translateX.o" "SideBlock1.tx";
connectAttr "SideBlock1_translateY.o" "SideBlock1.ty";
connectAttr "SideBlock1_translateZ.o" "SideBlock1.tz";
connectAttr "SideBlock1_scaleX.o" "SideBlock1.sx";
connectAttr "SideBlock1_scaleY.o" "SideBlock1.sy";
connectAttr "SideBlock1_scaleZ.o" "SideBlock1.sz";
connectAttr "BridgePiece4_visibility.o" "BridgePiece4.v";
connectAttr "BridgePiece4_translateX.o" "BridgePiece4.tx";
connectAttr "BridgePiece4_translateY.o" "BridgePiece4.ty";
connectAttr "BridgePiece4_translateZ.o" "BridgePiece4.tz";
connectAttr "BridgePiece4_rotateX.o" "BridgePiece4.rx";
connectAttr "BridgePiece4_rotateY.o" "BridgePiece4.ry";
connectAttr "BridgePiece4_rotateZ.o" "BridgePiece4.rz";
connectAttr "BridgePiece4_scaleX.o" "BridgePiece4.sx";
connectAttr "BridgePiece4_scaleY.o" "BridgePiece4.sy";
connectAttr "BridgePiece4_scaleZ.o" "BridgePiece4.sz";
connectAttr "SideBlock8_translateX.o" "SideBlock8.tx";
connectAttr "SideBlock8_translateY.o" "SideBlock8.ty";
connectAttr "SideBlock8_translateZ.o" "SideBlock8.tz";
connectAttr "SideBlock8_visibility.o" "SideBlock8.v";
connectAttr "SideBlock8_rotateX.o" "SideBlock8.rx";
connectAttr "SideBlock8_rotateY.o" "SideBlock8.ry";
connectAttr "SideBlock8_rotateZ.o" "SideBlock8.rz";
connectAttr "SideBlock8_scaleX.o" "SideBlock8.sx";
connectAttr "SideBlock8_scaleY.o" "SideBlock8.sy";
connectAttr "SideBlock8_scaleZ.o" "SideBlock8.sz";
connectAttr "BridgePiece5_visibility.o" "BridgePiece5.v";
connectAttr "BridgePiece5_translateX.o" "BridgePiece5.tx";
connectAttr "BridgePiece5_translateY.o" "BridgePiece5.ty";
connectAttr "BridgePiece5_translateZ.o" "BridgePiece5.tz";
connectAttr "BridgePiece5_rotateX.o" "BridgePiece5.rx";
connectAttr "BridgePiece5_rotateY.o" "BridgePiece5.ry";
connectAttr "BridgePiece5_rotateZ.o" "BridgePiece5.rz";
connectAttr "BridgePiece5_scaleX.o" "BridgePiece5.sx";
connectAttr "BridgePiece5_scaleY.o" "BridgePiece5.sy";
connectAttr "BridgePiece5_scaleZ.o" "BridgePiece5.sz";
connectAttr "SideBlock5_translateX.o" "SideBlock5.tx";
connectAttr "SideBlock5_translateY.o" "SideBlock5.ty";
connectAttr "SideBlock5_translateZ.o" "SideBlock5.tz";
connectAttr "SideBlock5_visibility.o" "SideBlock5.v";
connectAttr "SideBlock5_rotateX.o" "SideBlock5.rx";
connectAttr "SideBlock5_rotateY.o" "SideBlock5.ry";
connectAttr "SideBlock5_rotateZ.o" "SideBlock5.rz";
connectAttr "SideBlock5_scaleX.o" "SideBlock5.sx";
connectAttr "SideBlock5_scaleY.o" "SideBlock5.sy";
connectAttr "SideBlock5_scaleZ.o" "SideBlock5.sz";
connectAttr "SideBlock6_translateX.o" "SideBlock6.tx";
connectAttr "SideBlock6_translateY.o" "SideBlock6.ty";
connectAttr "SideBlock6_translateZ.o" "SideBlock6.tz";
connectAttr "SideBlock6_visibility.o" "SideBlock6.v";
connectAttr "SideBlock6_rotateX.o" "SideBlock6.rx";
connectAttr "SideBlock6_rotateY.o" "SideBlock6.ry";
connectAttr "SideBlock6_rotateZ.o" "SideBlock6.rz";
connectAttr "SideBlock6_scaleX.o" "SideBlock6.sx";
connectAttr "SideBlock6_scaleY.o" "SideBlock6.sy";
connectAttr "SideBlock6_scaleZ.o" "SideBlock6.sz";
connectAttr "BridgePiece6_visibility.o" "BridgePiece6.v";
connectAttr "BridgePiece6_translateX.o" "BridgePiece6.tx";
connectAttr "BridgePiece6_translateY.o" "BridgePiece6.ty";
connectAttr "BridgePiece6_translateZ.o" "BridgePiece6.tz";
connectAttr "BridgePiece6_rotateX.o" "BridgePiece6.rx";
connectAttr "BridgePiece6_rotateY.o" "BridgePiece6.ry";
connectAttr "BridgePiece6_rotateZ.o" "BridgePiece6.rz";
connectAttr "BridgePiece6_scaleX.o" "BridgePiece6.sx";
connectAttr "BridgePiece6_scaleY.o" "BridgePiece6.sy";
connectAttr "BridgePiece6_scaleZ.o" "BridgePiece6.sz";
connectAttr "SideBlock4_translateX.o" "SideBlock4.tx";
connectAttr "SideBlock4_translateY.o" "SideBlock4.ty";
connectAttr "SideBlock4_translateZ.o" "SideBlock4.tz";
connectAttr "SideBlock4_visibility.o" "SideBlock4.v";
connectAttr "SideBlock4_rotateX.o" "SideBlock4.rx";
connectAttr "SideBlock4_rotateY.o" "SideBlock4.ry";
connectAttr "SideBlock4_rotateZ.o" "SideBlock4.rz";
connectAttr "SideBlock4_scaleX.o" "SideBlock4.sx";
connectAttr "SideBlock4_scaleY.o" "SideBlock4.sy";
connectAttr "SideBlock4_scaleZ.o" "SideBlock4.sz";
connectAttr "SideBlock3_translateX.o" "SideBlock3.tx";
connectAttr "SideBlock3_translateY.o" "SideBlock3.ty";
connectAttr "SideBlock3_translateZ.o" "SideBlock3.tz";
connectAttr "SideBlock3_visibility.o" "SideBlock3.v";
connectAttr "SideBlock3_rotateX.o" "SideBlock3.rx";
connectAttr "SideBlock3_rotateY.o" "SideBlock3.ry";
connectAttr "SideBlock3_rotateZ.o" "SideBlock3.rz";
connectAttr "SideBlock3_scaleX.o" "SideBlock3.sx";
connectAttr "SideBlock3_scaleY.o" "SideBlock3.sy";
connectAttr "SideBlock3_scaleZ.o" "SideBlock3.sz";
connectAttr "SideBlock2_translateX.o" "SideBlock2.tx";
connectAttr "SideBlock2_translateY.o" "SideBlock2.ty";
connectAttr "SideBlock2_translateZ.o" "SideBlock2.tz";
connectAttr "SideBlock2_visibility.o" "SideBlock2.v";
connectAttr "SideBlock2_rotateX.o" "SideBlock2.rx";
connectAttr "SideBlock2_rotateY.o" "SideBlock2.ry";
connectAttr "SideBlock2_rotateZ.o" "SideBlock2.rz";
connectAttr "SideBlock2_scaleX.o" "SideBlock2.sx";
connectAttr "SideBlock2_scaleY.o" "SideBlock2.sy";
connectAttr "SideBlock2_scaleZ.o" "SideBlock2.sz";
connectAttr "polySurface15_translateX.o" "Lantern.tx";
connectAttr "polySurface15_translateY.o" "Lantern.ty";
connectAttr "polySurface15_translateZ.o" "Lantern.tz";
connectAttr "polySurface15_rotateX.o" "Lantern.rx";
connectAttr "polySurface15_rotateY.o" "Lantern.ry";
connectAttr "polySurface15_rotateZ.o" "Lantern.rz";
connectAttr "BridgePiece3_visibility.o" "BridgePiece3.v";
connectAttr "BridgePiece3_translateX.o" "BridgePiece3.tx";
connectAttr "BridgePiece3_translateY.o" "BridgePiece3.ty";
connectAttr "BridgePiece3_translateZ.o" "BridgePiece3.tz";
connectAttr "BridgePiece3_rotateX.o" "BridgePiece3.rx";
connectAttr "BridgePiece3_rotateY.o" "BridgePiece3.ry";
connectAttr "BridgePiece3_rotateZ.o" "BridgePiece3.rz";
connectAttr "BridgePiece3_scaleX.o" "BridgePiece3.sx";
connectAttr "BridgePiece3_scaleY.o" "BridgePiece3.sy";
connectAttr "BridgePiece3_scaleZ.o" "BridgePiece3.sz";
connectAttr "SideBlock9_translateX.o" "SideBlock9.tx";
connectAttr "SideBlock9_translateY.o" "SideBlock9.ty";
connectAttr "SideBlock9_translateZ.o" "SideBlock9.tz";
connectAttr "SideBlock9_visibility.o" "SideBlock9.v";
connectAttr "SideBlock9_rotateX.o" "SideBlock9.rx";
connectAttr "SideBlock9_rotateY.o" "SideBlock9.ry";
connectAttr "SideBlock9_rotateZ.o" "SideBlock9.rz";
connectAttr "SideBlock9_scaleX.o" "SideBlock9.sx";
connectAttr "SideBlock9_scaleY.o" "SideBlock9.sy";
connectAttr "SideBlock9_scaleZ.o" "SideBlock9.sz";
connectAttr "SideBlock10_rotateX.o" "SideBlock10.rx";
connectAttr "SideBlock10_rotateY.o" "SideBlock10.ry";
connectAttr "SideBlock10_rotateZ.o" "SideBlock10.rz";
connectAttr "SideBlock10_visibility.o" "SideBlock10.v";
connectAttr "SideBlock10_translateX.o" "SideBlock10.tx";
connectAttr "SideBlock10_translateY.o" "SideBlock10.ty";
connectAttr "SideBlock10_translateZ.o" "SideBlock10.tz";
connectAttr "SideBlock10_scaleX.o" "SideBlock10.sx";
connectAttr "SideBlock10_scaleY.o" "SideBlock10.sy";
connectAttr "SideBlock10_scaleZ.o" "SideBlock10.sz";
connectAttr "SideBlock11_translateX.o" "SideBlock11.tx";
connectAttr "SideBlock11_translateY.o" "SideBlock11.ty";
connectAttr "SideBlock11_translateZ.o" "SideBlock11.tz";
connectAttr "SideBlock11_visibility.o" "SideBlock11.v";
connectAttr "SideBlock11_rotateX.o" "SideBlock11.rx";
connectAttr "SideBlock11_rotateY.o" "SideBlock11.ry";
connectAttr "SideBlock11_rotateZ.o" "SideBlock11.rz";
connectAttr "SideBlock11_scaleX.o" "SideBlock11.sx";
connectAttr "SideBlock11_scaleY.o" "SideBlock11.sy";
connectAttr "SideBlock11_scaleZ.o" "SideBlock11.sz";
connectAttr "chain_translateX.o" "decals.tx";
connectAttr "chain_translateY.o" "decals.ty";
connectAttr "chain_translateZ.o" "decals.tz";
connectAttr "chain_visibility.o" "decals.v";
connectAttr "chain_rotateX.o" "decals.rx";
connectAttr "chain_rotateY.o" "decals.ry";
connectAttr "chain_rotateZ.o" "decals.rz";
connectAttr "chain_scaleX.o" "decals.sx";
connectAttr "chain_scaleY.o" "decals.sy";
connectAttr "chain_scaleZ.o" "decals.sz";
relationship "link" ":lightLinker1" ":initialShadingGroup.message" ":defaultLightSet.message";
relationship "link" ":lightLinker1" ":initialParticleSE.message" ":defaultLightSet.message";
relationship "link" ":lightLinker1" "oz_ec_catacombs_straight_a1:lambert2SG.message" ":defaultLightSet.message";
relationship "link" ":lightLinker1" "lambert2SG.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" ":initialShadingGroup.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" ":initialParticleSE.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" "oz_ec_catacombs_straight_a1:lambert2SG.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" "lambert2SG.message" ":defaultLightSet.message";
connectAttr "layerManager.dli[0]" "defaultLayer.id";
connectAttr "renderLayerManager.rlmi[0]" "defaultRenderLayer.rlid";
connectAttr ":mentalrayGlobals.msg" ":mentalrayItemsList.glb";
connectAttr ":miDefaultOptions.msg" ":mentalrayItemsList.opt" -na;
connectAttr ":miDefaultFramebuffer.msg" ":mentalrayItemsList.fb" -na;
connectAttr ":miDefaultOptions.msg" ":mentalrayGlobals.opt";
connectAttr ":miDefaultFramebuffer.msg" ":mentalrayGlobals.fb";
connectAttr "oz_ec_master_opaque.oc" "oz_ec_catacombs_straight_a1:lambert2SG.ss"
		;
connectAttr "oz_ec_catacombs_straight_over_anim_aShape.iog" "oz_ec_catacombs_straight_a1:lambert2SG.dsm"
		 -na;
connectAttr "decalsShape.iog" "oz_ec_catacombs_straight_a1:lambert2SG.dsm" -na;
connectAttr "SideBlockShape11.iog" "oz_ec_catacombs_straight_a1:lambert2SG.dsm" 
		-na;
connectAttr "SideBlockShape10.iog" "oz_ec_catacombs_straight_a1:lambert2SG.dsm" 
		-na;
connectAttr "SideBlockShape9.iog" "oz_ec_catacombs_straight_a1:lambert2SG.dsm" -na
		;
connectAttr "BridgePieceShape3.iog" "oz_ec_catacombs_straight_a1:lambert2SG.dsm"
		 -na;
connectAttr "SideBlockShape2.iog" "oz_ec_catacombs_straight_a1:lambert2SG.dsm" -na
		;
connectAttr "SideBlockShape3.iog" "oz_ec_catacombs_straight_a1:lambert2SG.dsm" -na
		;
connectAttr "SideBlockShape4.iog" "oz_ec_catacombs_straight_a1:lambert2SG.dsm" -na
		;
connectAttr "BridgePieceShape6.iog" "oz_ec_catacombs_straight_a1:lambert2SG.dsm"
		 -na;
connectAttr "SideBlockShape6.iog" "oz_ec_catacombs_straight_a1:lambert2SG.dsm" -na
		;
connectAttr "SideBlockShape5.iog" "oz_ec_catacombs_straight_a1:lambert2SG.dsm" -na
		;
connectAttr "BridgePieceShape5.iog" "oz_ec_catacombs_straight_a1:lambert2SG.dsm"
		 -na;
connectAttr "SideBlockShape8.iog" "oz_ec_catacombs_straight_a1:lambert2SG.dsm" -na
		;
connectAttr "SideBlockShape7.iog" "oz_ec_catacombs_straight_a1:lambert2SG.dsm" -na
		;
connectAttr "BridgePieceShape4.iog" "oz_ec_catacombs_straight_a1:lambert2SG.dsm"
		 -na;
connectAttr "SideBlockShape1.iog" "oz_ec_catacombs_straight_a1:lambert2SG.dsm" -na
		;
connectAttr "BridgePieceShape1.iog" "oz_ec_catacombs_straight_a1:lambert2SG.dsm"
		 -na;
connectAttr "SideBlockShape12.iog" "oz_ec_catacombs_straight_a1:lambert2SG.dsm" 
		-na;
connectAttr "BridgePieceShape2.iog" "oz_ec_catacombs_straight_a1:lambert2SG.dsm"
		 -na;
connectAttr "LanternShape.iog" "oz_ec_catacombs_straight_a1:lambert2SG.dsm" -na;
connectAttr "oz_ec_catacombs_straight_a1:lambert2SG.msg" "oz_ec_catacombs_straight_a1:materialInfo1.sg"
		;
connectAttr "oz_ec_master_opaque.msg" "oz_ec_catacombs_straight_a1:materialInfo1.m"
		;
connectAttr "file1.msg" "oz_ec_catacombs_straight_a1:materialInfo1.t" -na;
connectAttr "file1.oc" "oz_ec_master_opaque.c";
connectAttr "file1.ot" "oz_ec_master_opaque.it";
connectAttr "oz_ec_catacombs_straight_a1:place2dTexture1.c" "file1.c";
connectAttr "oz_ec_catacombs_straight_a1:place2dTexture1.tf" "file1.tf";
connectAttr "oz_ec_catacombs_straight_a1:place2dTexture1.rf" "file1.rf";
connectAttr "oz_ec_catacombs_straight_a1:place2dTexture1.mu" "file1.mu";
connectAttr "oz_ec_catacombs_straight_a1:place2dTexture1.mv" "file1.mv";
connectAttr "oz_ec_catacombs_straight_a1:place2dTexture1.s" "file1.s";
connectAttr "oz_ec_catacombs_straight_a1:place2dTexture1.wu" "file1.wu";
connectAttr "oz_ec_catacombs_straight_a1:place2dTexture1.wv" "file1.wv";
connectAttr "oz_ec_catacombs_straight_a1:place2dTexture1.re" "file1.re";
connectAttr "oz_ec_catacombs_straight_a1:place2dTexture1.of" "file1.of";
connectAttr "oz_ec_catacombs_straight_a1:place2dTexture1.r" "file1.ro";
connectAttr "oz_ec_catacombs_straight_a1:place2dTexture1.n" "file1.n";
connectAttr "oz_ec_catacombs_straight_a1:place2dTexture1.vt1" "file1.vt1";
connectAttr "oz_ec_catacombs_straight_a1:place2dTexture1.vt2" "file1.vt2";
connectAttr "oz_ec_catacombs_straight_a1:place2dTexture1.vt3" "file1.vt3";
connectAttr "oz_ec_catacombs_straight_a1:place2dTexture1.vc1" "file1.vc1";
connectAttr "oz_ec_catacombs_straight_a1:place2dTexture1.o" "file1.uv";
connectAttr "oz_ec_catacombs_straight_a1:place2dTexture1.ofs" "file1.fs";
connectAttr "file2.oc" "MoltenSteel.c";
connectAttr "MoltenSteel.oc" "lambert2SG.ss";
connectAttr "pPlaneShape1.iog" "lambert2SG.dsm" -na;
connectAttr "lambert2SG.msg" "materialInfo1.sg";
connectAttr "MoltenSteel.msg" "materialInfo1.m";
connectAttr "file2.msg" "materialInfo1.t" -na;
connectAttr "place2dTexture1.c" "file2.c";
connectAttr "place2dTexture1.tf" "file2.tf";
connectAttr "place2dTexture1.rf" "file2.rf";
connectAttr "place2dTexture1.mu" "file2.mu";
connectAttr "place2dTexture1.mv" "file2.mv";
connectAttr "place2dTexture1.s" "file2.s";
connectAttr "place2dTexture1.wu" "file2.wu";
connectAttr "place2dTexture1.wv" "file2.wv";
connectAttr "place2dTexture1.re" "file2.re";
connectAttr "place2dTexture1.of" "file2.of";
connectAttr "place2dTexture1.r" "file2.ro";
connectAttr "place2dTexture1.n" "file2.n";
connectAttr "place2dTexture1.vt1" "file2.vt1";
connectAttr "place2dTexture1.vt2" "file2.vt2";
connectAttr "place2dTexture1.vt3" "file2.vt3";
connectAttr "place2dTexture1.vc1" "file2.vc1";
connectAttr "place2dTexture1.o" "file2.uv";
connectAttr "place2dTexture1.ofs" "file2.fs";
connectAttr "oz_ec_catacombs_straight_a1:lambert2SG.pa" ":renderPartition.st" -na
		;
connectAttr "lambert2SG.pa" ":renderPartition.st" -na;
connectAttr "oz_ec_master_opaque.msg" ":defaultShaderList1.s" -na;
connectAttr "MoltenSteel.msg" ":defaultShaderList1.s" -na;
connectAttr "file1.msg" ":defaultTextureList1.tx" -na;
connectAttr "file2.msg" ":defaultTextureList1.tx" -na;
connectAttr "oz_ec_catacombs_straight_a1:place2dTexture1.msg" ":defaultRenderUtilityList1.u"
		 -na;
connectAttr "place2dTexture1.msg" ":defaultRenderUtilityList1.u" -na;
connectAttr "defaultRenderLayer.msg" ":defaultRenderingList1.r" -na;
connectAttr ":perspShape.msg" ":defaultRenderGlobals.sc";
// End of oz_ec_catacombs_straight_over_anim_a.ma
