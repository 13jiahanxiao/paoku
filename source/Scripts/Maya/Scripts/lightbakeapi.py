import pymel.core as pm
import maya.mel as mm

import math
import shutil
import os

import mainmayautil

import xml.etree.ElementTree as ET

USERPROFILE = os.environ['userprofile'].replace('\\','/')

#Settings stored in the bake set during lightmap setup. More info on these settings can be found in the maya docs.
#http://download.autodesk.com/global/docs/maya2012/en_us/Nodes/textureBakeSet.html
gBakeSettings = {
	'colorMode' : 1, # Light Only
	'xResolution' : 512,
	'yResolution' : 512,
	'fileFormat' : 6, #tga
	'bakeToOneMap' : 1,
	'samples' : 2,
	'finalGatherQuality' : 3.0,
	'backgroundMode' : 1,
	'backgroundColor' : [0.25,0.25,0.25],
	'fillTextureSeams' : 5.0,
	'overrideUvSet' : 1,
	'uvSetName' : 'LightMap'
	}

gBakeAOSettings = {
	'colorMode' : 4, # Custom Shader
	'backgroundColor' : [1.0,1.0,1.0]
	}
	
#Render settings for mental ray. You can find these settings in the render globals. 
gMentalRaySettings = {
	'finalGather' : 0, 
	'rayTracing' : 1,
	'finalGatherTraceDiffuse' : 1,
	'finalGatherPresampleDensity' : 5,
	'finalGatherBounceScale' : [1.5,1.5,1.5] #This increases the radiosity or secondary bounce light. 
	}	
	
#Settings for ambient occlusion. These get set on the mib_amb_occlusion node that is temporarily created durring the ao bake process.
gAOSettings = {
	'samples' : 128,
	'max_distance' : 3000
	}

#Loops through maya files either baking lightmaps or aomaps or both
def BatchBakeMaps(sourceDir, lightRig, inclusionFilters='', exclusionFilters='', check=True, setup=True, lightmaps=True, aomaps=True, scrollField=None):
	if(not pm.pluginInfo('Mayatomr.mll', q=True, loaded=True)):
		raise pm.PopupError('Mental Ray plugin is not loaded. Go to plugin manager and load the mayatomr.mll plugin')

	#Need to temporarily set the light rig option var
	originalLightRig = mainmayautil.LoadOptionVar('LightBakeTools_LightRigPath')
	mainmayautil.SaveOptionVar('LightBakeTools_LightRigPath', lightRig)
	
	print sourceDir
	allFiles = os.listdir(sourceDir)
	print 'All files'
	print allFiles
	
	mbFiles = []
	inclusionFiles = []
	filesToBake = []
	
	for fileName in allFiles:
		if(fileName.endswith('.mb') or fileName.endswith('.ma')):
			mbFiles.append(fileName)
	
	print 'All Maya Files'
	print mbFiles
	
	if(not inclusionFilters):
		inclusionFiles = mbFiles
	else:
		for fileName in mbFiles:
			filterFound = False
			for filter in inclusionFilters.split(','):
				filter = filter.strip()
				if(fileName.find(filter) != -1):
					filterFound = True
					break
			if(filterFound):
				inclusionFiles.append(fileName)
	
	print 'Files after inclusion filter applied'
	print inclusionFiles
	if(not exclusionFilters):
		filesToBake = inclusionFiles
	else:
		for fileName in inclusionFiles:
			filterFound = False
			for filter in exclusionFilters.split(','):
				filter = filter.strip()
				if(fileName.find(filter) != -1):
					filterFound = True
					break
			if(not filterFound):
				filesToBake.append(fileName)
	
	print 'Files after exclusion filter applied'
	print filesToBake
	
	i=0
	for fileName in filesToBake:
		#print i
		#if(i > 50):					
		pm.openFile('%s/%s' % (sourceDir, fileName), force=True)
		
		#Fix the paths for textures
		mainmayautil.FixTextureUserPaths()
	
		scrollField.setText('%s\n%s\n' % (scrollField.getText(), fileName))
		#name = os.path.splitext(fileName)[0]
	
		mainmayautil.FixTextureUserPaths()
		if(check):
			ConsistencyCheck(scrollField)
		if(setup):
			result = AutoSetup()
			scrollField.setText('%s%s\n' % (scrollField.getText(), result))
		if(lightmaps):
			result = AutoBakeLightmap()
			scrollField.setText('%s%s\n' % (scrollField.getText(), result))						
		if(aomaps):
			result = AutoBakeAO()
			scrollField.setText('%s%s\n' % (scrollField.getText(), result))
		i+=1
		#if(i > 5):
			#break

	#Restore light rig after batching
	mainmayautil.SaveOptionVar('LightBakeTools_LightRigPath', originalLightRig)
				
	scrollField.setText('%s\nBatch Bake Finished.\n' % scrollField.getText())	

	pm.newFile(force=True)
	
# def BatchBakeMaps(sourceDir, lightRig, setup=True, lightmaps=True, aomaps=True, scrollField=None):
	#Need to temporarily set the light rig option var
	# originalLightRig = mainmayautil.LoadOptionVar('LightBakeTools_LightRigPath')
	# mainmayautil.SaveOptionVar('LightBakeTools_LightRigPath', lightRig)
	
	# for fileName in os.listdir(sourceDir):
		# if(fileName.endswith('.mb')):
			# if(fileName.find('_forest_') != -1): #temp filter for forest pieces.
				# print fileName
				# print fileName.find('_anim_')
				# if(fileName.find('_anim_') == -1):	#don't batch animation pieces they need custom setup	
					# pm.openFile('%s/%s' % (sourceDir, fileName), force=True)
					# name = os.path.splitext(fileName)[0]
					
					# meshes = pm.ls(geometry=True)
					# foundMeshes = []
					# for mesh in meshes:
						# if(mesh.nodeName().startswith(name)):
							# foundMeshes.append(mesh)
							
					# if(not foundMeshes):
						# scrollField.setText('%sFAILED: File %s has no valid meshes. Check naming.\n' % (scrollField.getText(), fileName))
					# elif(len(foundMeshes) > 1):
						# scrollField.setText('%sFAILED: File %s has more than one valid mesh. Check naming.\n' % (scrollField.getText(), fileName))
					# else:
						# nodesToBake = []
						# nodesToBake.append(foundMeshes[0].getParent())
						# tempArray = pm.ls('decals')
						# if(tempArray):
							# nodesToBake.append(tempArray[0])
						# tempArray = pm.ls('decals_overlays')
						# if(tempArray):
							# nodesToBake.append(tempArray[0])
							
						# pm.hide(allObjects=True)
						# pm.showHidden(nodesToBake)
						
						# if(setup):
							# pm.select(clear=True)
							# pm.select(nodesToBake)
							# SetupSelectedForBake()
						# if(lightmaps):
							# pm.select(clear=True)
							# pm.select(nodesToBake)
							# ReferenceRigAndBakeSelected()
						# if(aomaps):
							# pm.select(clear=True)
							# pm.select(nodesToBake)
							# AOBakeSelected()
						# scrollField.setText('%sSUCCESS: File %s baked successfully.\n' % (scrollField.getText(), fileName))

	#Restore light rig after batching
	# mainmayautil.SaveOptionVar('LightBakeTools_LightRigPath', originalLightRig)
				
	# scrollField.setText('\nBatch Bake Finished.\n')

#Automatically setting up a scene for lightmap or ao map baking. This will read through nodes in a scene setting them up instead of setting up whats selected. Used by the batch process.
def AutoSetup():
	name = GetNameFromSceneName()
	mainMeshNode = FindMainMeshNode(name)
	
	if(not mainMeshNode):
		return 'SETUP FAILED: Setup failed no mesh named %s found.' % name
		
	pm.delete(mainMeshNode, constructionHistory=True)
	
	lightmapModels = []
	tempTransforms = pm.ls(transforms=True)
	for transform in tempTransforms:
			if(transform.getShape()):
				if(pm.nodeType(transform.getShape()) == 'mesh'):
					uvSets = transform.getShape().getUVSetNames()
					if(len(uvSets) == 2):
						if(uvSets[1] == 'LightMap'):	
							lightmapModels.append(transform)
							
	decalsOverlaysModels = []
	decalsModels = []
	otherModels = []
			
	for transform in lightmapModels:
		if(transform.nodeName().find('decals_overlays') != -1):
			decalsOverlaysModels.append(transform)
		elif(transform.nodeName().find('decals') != -1):
			#Don't include objects tagged with decals_lo these are meant to replace lightmap objects for lower end devices. 
			if(transform.nodeName().find('decals_lo') == -1):
				decalsModels.append(transform)
		else:
			#ignore the main mesh and backdrop meshes
			if(transform != mainMeshNode and transform.nodeName != 'backdrop'):
				otherModels.append(transform)
	
	decalsOverlaysCombined = None
	decalsCombined = None
	otherModelsCombined = None
	modelsToBake = []
	
	modelsToBake.append(mainMeshNode)
	
	if(decalsOverlaysModels):
		#Combine the decals overlays models to one mesh if there is more than one.
		if(len(decalsOverlaysModels) > 1):
			decalsOverlaysCombined = pm.polyUnite(decalsOverlaysModels, constructionHistory=True, mergeUVSets=True, name='decals_overlays')[0]
		else:
			decalsOverlaysCombined = decalsOverlaysModels[0]
		#Turn the shadows off for the new combined decals_overlays
		decalsOverlaysCombined.getShape().castsShadows.set(0)
		pm.delete(decalsOverlaysCombined, constructionHistory=True)
		modelsToBake.append(decalsOverlaysCombined)
		
	if(decalsModels):
		if(len(decalsModels) > 1):
			decalsCombined = pm.polyUnite(decalsModels, constructionHistory=True, mergeUVSets=True, name='decals')[0]
		else:
			decalsCombined = decalsModels[0]
		pm.delete(decalsCombined, constructionHistory=True)
		modelsToBake.append(decalsCombined)
		
	if(otherModels):
		if(len(otherModels) > 1):
			otherModelsCombined = pm.polyUnite(otherModels, constructionHistory=True, mergeUVSets=True, name='other_models')[0]
		else:
			otherModelsCombined = otherModels[0]
		pm.delete(otherModelsCombined, constructionHistory=True)	
		modelsToBake.append(otherModelsCombined)		
		
	childNodes = mainMeshNode.getChildren()
	for child in childNodes:
		if(child != mainMeshNode.getShape()):
			print 'deleting '
			print child
			pm.delete(child)
		
	pm.hide(allObjects=True)
	pm.showHidden(modelsToBake)

	msg = ''
	
	SetupForBake(modelsToBake)

	msg += 'SETUP SUCCESSFUL'
	
	return msg
	
#Automatically bake lightmap for nodes in a scene. Used by the batch process.
def AutoBakeLightmap():
	name = GetNameFromSceneName()
	mainMeshNode = FindMainMeshNode(name)
	
	print 'NODE NAME IS %s' % mainMeshNode
	if(not mainMeshNode):
		return 'LM BAKE FAILED: Setup failed no mesh named %s found.' % name
	
	bakeSetNode = GetBakeSetNode()
	if(not bakeSetNode):
		return 'LM BAKE FAILED: Bake set node not found or misnamed. Open the file and run setup first.'	
	
	#Gets the objects from the bake set. 
	lightmapShapes = bakeSetNode.members(flatten=True)
	lightmapModels = [] #list of transforms
	for shape in lightmapShapes:
		if(pm.nodeType(shape) == 'mesh'):
			lightmapModels.append(shape.getParent())	
		
	if(IsLightRigReferenced()):
		return 'LM BAKE FAILED: Rig is already referenced. Open the file and clear the reference out in the Reference Editor.'
	
	referenceNode = ReferenceLightRig()	
	
	msg = ''
	if(len(lightmapModels) > 5):
		msg = 'WARNING: Attempting to bake more than 5 models. This may cause issues. Merge objects if the lightmap comes out wrong.' 	
	
	tempArray = pm.ls('custom_lights', transforms=True)
	customLightsNode = None
	if(tempArray):
		customLightsNode = tempArray[0]
		
	
	pm.hide(allObjects=True)
	pm.showHidden(lightmapModels)
	if(referenceNode.nodes()):
		pm.showHidden(referenceNode.nodes())
	if(customLightsNode and customLightsNode.getChildren()):
		pm.showHidden(customLightsNode)
		childNodes = pm.listRelatives(customLightsNode, allDescendents=True)
		for child in childNodes:
			pm.showHidden(child)
	
	PerformBake(mainMeshNode, lightmapModels, bakeSetNode)
	
	msg += 'LM BAKE SUCCESSFUL'
	
	return msg
	
#Automatically bake ao for nodes in a scene. Used by the batch process.
def AutoBakeAO():
	name = GetNameFromSceneName()
	mainMeshNode = FindMainMeshNode(name)
	
	print 'NODE NAME IS %s' % mainMeshNode
	if(not mainMeshNode):
		return 'AO BAKE FAILED: No mesh named %s found.' % name
	
	bakeSetNode = GetBakeSetNode()
	if(not bakeSetNode):
		return 'AO BAKE FAILED: Bake set node not found or misnamed. Open the file and run setup first.'	
	
	lightmapShapes = bakeSetNode.members(flatten=True)
	lightmapModels = [] #list of transforms	
	for shape in lightmapShapes:
		if(pm.nodeType(shape) == 'mesh'):
			if(shape.getParent().nodeName().find('decals_overlays') == -1):
				lightmapModels.append(shape.getParent())

	msg = ''
	if(len(lightmapModels) > 5):
		msg = 'WARNING: Attempting to bake more than 5 models. This may cause issues. Merge objects if the lightmap comes out wrong.' 		
	
	pm.hide(allObjects=True)
	pm.showHidden(lightmapModels)
	
	PerformAOBake(mainMeshNode, lightmapModels, bakeSetNode)
	
	msg += 'AO BAKE SUCCESSFUL'
	
	return msg

#Gets the current Maya scene filename from the scene path. 
def GetNameFromSceneName():
	sceneName = pm.sceneName()
	if(not sceneName):
		pm.PopupError('Scene must be saved first')
		
	name = os.path.splitext(os.path.basename(sceneName))[0]
	return name
	
#Find all mesh nodes that start with input name
def FindMainMeshNode(name):
	transformList = pm.ls(transforms=True)

	resultList = []
	for transform in transformList:
		if(transform.nodeName().startswith(name)):
			shape = transform.getShape()
			if(shape):
				if(pm.nodeType(shape) == 'mesh'):
					resultList.append(transform)

	if(not resultList):
		return None
	
	return resultList[0]


def FindUniqueTransformNodes(name):
	matches = pm.ls(name, transforms=True)

	return matches
	
#Reference the light rig into the scene then bake the selected nodes lightmap
def ReferenceRigAndBakeSelected():
	MentalRayCheck()

	objectList = pm.ls(sl=True)
	if(not objectList):
		raise pm.PopupError('Must select an object first')
	if(objectList[0].nodeName() == 'decals'):
		raise pm.PopupError('Select the main object first')

	mainObject = objectList[0]
		
	bakeSetNode = GetObjectsBakeSet(objectList[0])
	
	if(not bakeSetNode):
		raise pm.PopupError('Bake set node not found for object %s. Run the bake setup script first.' % objectList[0].nodeName())
	
	if(IsLightRigReferenced()):
		pm.PopupError('Rig is already referenced. Use the bake command instead or clear the reference out in the Reference Editor.')
	
	referenceNode = ReferenceLightRig()	
	
	PerformBake(mainObject, objectList, bakeSetNode)
	
	PerformCleanup(referenceNode)
	
#Check to see if the light rig is already referenced in the scene.
def IsLightRigReferenced():
	references = pm.getReferences()
	for key, value in references.iteritems():
		if(key.find('_lightrig') != -1):
			return True
		if(value.path.find('/LightRig/') != -1):
			return True
	return False
	
#Check to see if mental ray plugin is loaded and that the options node is there.
def MentalRayCheck():
	if(not pm.pluginInfo('Mayatomr.mll', q=True, loaded=True)):
		raise pm.PopupError('Mental Ray plugin is not loaded. Go to plugin manager and load the mayatomr.mll plugin')
		
	tempArray = pm.ls('miDefaultOptions', type='mentalrayOptions')
	
	if(not tempArray):
		raise pm.PopupError('Mental ray settings node not found. Open the Maya render globals (settings) then try again.') 		
	
#Setup selected objects for bake. 
def SetupSelectedForBake():
	MentalRayCheck()

	SetDefaultPreferences()

	objectList = pm.ls(sl=True)
	if(not objectList):
		raise pm.PopupError('Must select an object first')

	SetupForBake(objectList)
	
#Setup objects for bake. Setup involves creating a bake set and assigning objects as well as setting render settings.
def SetupForBake(objectList):
	bakeSetNode = None
	
	#Eradicate all bake sets named troz_texture_bake_set
	tempArray = pm.ls('*troz_texture_bake_set*', r=True)
	for object in tempArray:
		pm.delete(object)
	
	bakeSetNode = CreateAndAssignBakeSet('textureBakeSet', objectList, 'troz_texture_bake_set')
	
	bakeSetSetup = mainmayautil.LoadOptionVar('LightBakeTools_BakeSetSetup')
	if(bakeSetSetup == None or bakeSetSetup == True):	
		SetBakeDefaultSettings(bakeSetNode)
	
	renderSettings = mainmayautil.LoadOptionVar('LightBakeTools_RenderSettings')
	if(renderSettings == None or renderSettings == True):	
		SetMentalRaySettings()	
	
#Reference the light rig into the scene.
def ReferenceLightRig():
	selectedNodes = pm.ls(sl=True)

	lightRigPath = mainmayautil.LoadOptionVar('LightBakeTools_LightRigPath')
	referenceNode = pm.createReference(lightRigPath)
	
	pm.select(selectedNodes)
	
	return referenceNode
	
#Bake selected objects lightmap without referencing rig.
def BakeSelected():
	MentalRayCheck()

	objectList = pm.ls(sl=True)
	if(not objectList):
		raise pm.PopupError('Must select an object first')
	if(objectList[0].nodeName() == 'decals'):
		raise pm.PopupError('Select the main object first')
		
	mainObject = objectList[0]

	bakeSetNode = GetObjectsBakeSet(objectList[0])
	
	if(not bakeSetNode):
		raise pm.PopupError('Bake set node not found for object %s. Run the bake setup script first.' % objectList[0].nodeName())

	PerformBake(mainObject, objectList, bakeSetNode)

#Bake selected objects ao map
def AOBakeSelected():
	MentalRayCheck()

	objectList = pm.ls(sl=True)
	if(not objectList):
		raise pm.PopupError('Must select an object first')
	if(objectList[0].nodeName() == 'decals'):
		raise pm.PopupError('Select the main object first')

	mainMeshNode = objectList[0]	
		
	bakeSetNode = GetObjectsBakeSet(objectList[0])
	
	if(not bakeSetNode):
		raise pm.PopupError('Bake set node not found for object %s. Run the bake setup script first.' % objectList[0].nodeName())

	PerformAOBake(mainMeshNode, objectList, bakeSetNode)
	
def PerformBake(mainObject, objectList, bakeSetNode):
	print 'performbake object list'
	print objectList
	#lightmapPath = bakeSetNode.lightmapPath.get()
	
	outputDir = mainmayautil.LoadOptionVar('LightBakeTools_OutputDir')
	
	if(not os.path.exists(outputDir)):
		 os.makedirs(outputDir)
	
	lightmapPath = '%s/lm_%s.tga' % (outputDir, mainObject.nodeName())	
	
	autoAtlasUV = mainmayautil.LoadOptionVar('LightBakeTools_AutoAtlasUV')	
	quadrant = None
	if(autoAtlasUV):
		quadrant, size = UVAtlasToNormal(objectList[0])
		if(len(objectList) > 1):
			for i in range(1, len(objectList)):
				UVAtlasToNormal(objectList[i])
		#if(size > 0.126 or size < 0.124):
			#pm.PopupError('WARNING: Uvs for object %s reach further than their atlas quadrant bounds. Check the uv editor' % object.nodeName())
	
	#These 2 lines are needed to set mental ray up properly for baking. Something with creating initial bake sets.
	pm.select(clear=True);
	mm.eval('mrBakeToTexture false;')
	
	pm.select(objectList)
	
	try:
		print 'baking'
		outputPath = pm.convertLightmapSetup(bakeAll=False, camera='persp', shadows=True, keepOrgSG=True, showcpv=True)[0]
	except:
		for object in objectList:
			UVNormalToAtlas(object, quadrant)
		raise pm.PopupError('Lightmap bake failed. Check script editor')
		
	if(autoAtlasUV):
		for object in objectList:
			UVNormalToAtlas(object, quadrant)

	print 'Copying to: %s' % lightmapPath
	mainmayautil.ForceCopyFile(outputPath, lightmapPath)	
	
	dimensions = mainmayautil.LoadOptionVar('LightBakeTools_Dimensions')
	if(not dimensions):
		dimensions = 8
	
	if(quadrant):
		WriteAtlasRefFile(outputDir, mainObject.nodeName(), quadrant, dimensions)
	
def PerformAOBake(mainObject, objectList, bakeSetNode):
	outputDir = mainmayautil.LoadOptionVar('LightBakeTools_OutputDir')

	if(not os.path.exists(outputDir)):
		 os.makedirs(outputDir)
	
	aoPath = '%s/lm_%s_ao.tga' % (outputDir, mainObject.nodeName())	
	
	autoAtlasUV = mainmayautil.LoadOptionVar('LightBakeTools_AutoAtlasUV')	
	if(autoAtlasUV):
		quadrant, size = UVAtlasToNormal(objectList[0])
		if(len(objectList) > 1):
			for i in range(1, len(objectList)):
				UVAtlasToNormal(objectList[i])
		#if(size > 0.126 or size < 0.124):
			#pm.PopupError('WARNING: Uvs for object %s reach further than their atlas quadrant bounds. Check the uv editor' % object.nodeName())
		
	#These 2 lines are needed to set mental ray up properly for baking. Something with creating initial bake sets.
	pm.select(clear=True);
	mm.eval('mrBakeToTexture false;')
	
	surfaceShader = None
	mibAONode = None
	changedSettings = {}
	
	bakeSetAOSetup = mainmayautil.LoadOptionVar('LightBakeTools_BakeSetAOSetup')
	if(bakeSetAOSetup == None or bakeSetAOSetup == True):	
		surfaceShader, mibAONode, changedSettings = SetAOBakeSettings(bakeSetNode)

	pm.select(objectList)
	
	try:
		print 'baking'
		outputPath = pm.convertLightmapSetup(bakeAll=False, camera='persp', shadows=True, keepOrgSG=True, showcpv=True)[0]
	except:
		for object in objectList:
			UVNormalToAtlas(object, quadrant)
		raise pm.PopupError('Lightmap bake failed. Check script editor')	
		
	if(autoAtlasUV):	
		for object in objectList:
			UVNormalToAtlas(object, quadrant)
	
	#Revert bake set back to previous settings when finished baking.
	for attr, value in changedSettings.iteritems():
		bakeSetNode.setAttr(attr, value)
		
	#bakeSetNode.colorMode.set(gBakeSettings['colorMode'])
	#bakeSetNode.backgroundColor.set(gBakeSettings['backgroundColor'])
	
	if(surfaceShader):
		pm.delete(surfaceShader)
	if(mibAONode):
		pm.delete(mibAONode)

	mainmayautil.ForceCopyFile(outputPath, aoPath)		

def Lightmap3dPaintSelected():
	objectList = pm.ls(sl=True)
	if(not objectList):
		raise pm.PopupError('Must select an object(s) first')
	
	object = objectList[0]
	
	uvSets = object.getShape().getUVSetNames()
	if(len(uvSets) == 2):
		if(uvSets[1] == 'LightMap'):	
			object.setCurrentUVSetName('LightMap')	
			if(CheckForMissingUVs(object)):
				raise pm.PopupError('Object has faces without uvs on the LightMap uv set')
		else:
			raise pm.PopupError('Object contains no uv set named LightMap or Lightmap is not the second uv set')
	else:
		raise pm.PopupError('Only one uv set on object')
		
	lightmapPath = mainmayautil.LoadOptionVar('LightBakeTools_LightmapPreviewPath')
	if(not lightmapPath):
		pm.PopupError('No lightmap path specified. Set a lightmap preview file in the lightmap tool settings')
	
	lightmapName = os.path.splitext(os.path.basename(lightmapPath))[0]
	
	pm.select(clear=True)
	pm.select(object)
	pm.hyperShade(shaderNetworksSelectMaterialNodes=True)
	materialList = pm.ls(sl=True)
	
	pm.select(clear=True)
	pm.select(object)

	if(materialList[0].name().startswith('lightmapPaintMaterial')):
		lightmapMaterial = materialList[0]
		lightmapTexture = lightmapMaterial.color.inputs()[0]
		lightmapTexture.fileTextureName.set(lightmapPath)
	else:
		#Connect all the shape nodes to the uvChooser	
		lightmapMaterial = pm.shadingNode('lambert', asShader=True, name='lightmapPaintMaterial')
		
		pm.select(object)		
		pm.hyperShade(assign=lightmapMaterial.name())		
		
		lightmapTexture, lightmapPlace2d = mainmayautil.CreateNewTextureWithPlace2d(lightmapName)	
		
		lightmapTexture.outColor >> lightmapMaterial.color
		
		lightmapTexture.fileTextureName.set(lightmapPath)
		
		uvChooser = pm.shadingNode('uvChooser', asUtility=True)		
		
		#Connect the uv chooser to the place2d
		uvChooser.outVertexCameraOne >> lightmapPlace2d.vertexCameraOne
		uvChooser.outVertexUvThree >> lightmapPlace2d.vertexUvThree	
		uvChooser.outVertexUvTwo >> lightmapPlace2d.vertexUvTwo	
		uvChooser.outVertexUvOne >> lightmapPlace2d.vertexUvOne	
		uvChooser.outUv >> lightmapPlace2d.uvCoord
		
		shapeNode = object.getShape()
		
		found = False
		for uvSet in shapeNode.uvSet:
			if(uvSet.uvSetName.get() == "LightMap"):
				uvSet.uvSetName >> uvChooser.uvSets[0]
				found = True					
				
		if(not found):
			pm.PopupError("Object does not have a uv set called Lightmap")
		
	pm.select(object)		
	mm.eval('Art3dPaintToolOptions')	
		
def LightmapSavePaint3d():
	projectDir = pm.workspace(q=True, rd=True)	
	
	lightmapPath = mainmayautil.LoadOptionVar('LightBakeTools_LightmapPreviewPath')
	lightmapName = os.path.splitext(os.path.basename(lightmapPath))[0]
	
	objectShapeNameList = pm.art3dPaintCtx('art3dPaintContext', q=True, shapenames=True)
	if(len(objectShapeNameList) > 1):
		raise pm.PopupError('ERROR: More than one object is selected to paint. This save operation is meant to work with one object. Use the default "Save Textures" button instead')
	
	sceneName = GetNameFromSceneName()
	outputDir = '%ssourceimages/3dPaintTextures/%s' % (projectDir, sceneName)
	
	if(not os.path.exists(outputDir)):
		os.makedirs(outputDir)
	
	pm.art3dPaintCtx('art3dPaintContext', e=True, expandfilename=False, savetexture=True)
	
	fileList = os.listdir(outputDir)
	if(len(fileList) > 1):
		pm.PopupError('WARNING: More than one paint texture file found in the following directory. This happened because the directory wasnt clear before starting. You may have to move your textures over by hand. %s' % outputDir)
	
	outputPath = '%s/%s' % (outputDir, fileList[0])
	
	mainmayautil.ForceCopyFile(outputPath, lightmapPath)
	
	os.remove(outputPath)

def ConsistencyCheck(scrollField):
	tempArray = pm.ls('miDefaultOptions', type='mentalrayOptions')
	
	if(not tempArray):
		SetScrollFieldText(scrollField, '(REQUIRED): Mental ray settings node not found. Must set the renderer in render globals to mental ray then save the scene.')

	name = GetNameFromSceneName()
	meshList = pm.ls(type = 'mesh')

	mainMeshNodes = []
	decalsNodes = pm.ls('decals')
	decalsOverlaysNodes = pm.ls('decals_overlays')
	backdropNodes = pm.ls('backdrop')
	# otherMeshes = []
	
	allMeshes = []		
	for transform in pm.ls(transforms=True):
		if(transform.getShape()):
			if(pm.nodeType(transform.getShape()) == 'mesh'):
				if(transform.nodeName().startswith(name)):
					mainMeshNodes.append(transform)
				# else:
					# otherMeshes.append(transform)
	
	litMeshes = mainMeshNodes + decalsNodes + decalsOverlaysNodes	
	
	# otherMeshes = list(set(otherMeshes) - set(decalsNodes))
	# otherMeshes = list(set(otherMeshes) - set(decalsOverlaysNodes))
	# otherMeshes = list(set(otherMeshes) - set(backdropNodes))
	
	customLightsGroupNodes = pm.ls('custom_lights', transforms=True)
	
	if(not mainMeshNodes):
		SetScrollFieldText(scrollField, '(REQUIRED): Main mesh not found or not named right.')
	if(len(mainMeshNodes) > 1):
		SetScrollFieldText(scrollField, '(REQUIRED): More than one main mesh node found.')
		for mesh in mainMeshNodes:
			SetScrollFieldText(scrollField, mesh.longName())
	# if(not decalsNodes):
		# SetScrollFieldText(scrollField, '(OPTIONAL): decals mesh not found or not named right.')
	# if(len(decalsNodes) > 1):
		# SetScrollFieldText(scrollField, '(OPTIONAL): More than one decals mesh node found or more than one shape')
		# for mesh in decalsNodes:
			# SetScrollFieldText(scrollField, mesh.longName())
	# if(not decalsOverlaysNodes):
		# SetScrollFieldText(scrollField, '(OPTIONAL): decals_overlays mesh not found or not named right.')
	# elif(len(decalsOverlaysNodes) > 1):
		# SetScrollFieldText(scrollField, '(OPTIONAL): More than one decals_overlays mesh node found.')
		# for mesh in decalsOverlaysNodes:
			# SetScrollFieldText(scrollField, mesh.longName())
	else:
		if(decalsOverlaysNodes):
			if(decalsOverlaysNodes[0].getShape().castsShadows.get() == 1):
				SetScrollFieldText(scrollField, '(REQUIRED): decals_overlays is set to cast shadows. On the shape node go to Attribute editor->Render Stats->Cast Shadows and uncheck it.')
	# if(not backdropNodes):
		# SetScrollFieldText(scrollField, '(OPTIONAL): backdrop mesh not found or not named right.')
	# else:
	if(backdropNodes):
		for backdropNode in backdropNodes:
			if(not CheckBackdropUVs(backdropNode)):
				SetScrollFieldText(scrollField, '(REQUIRED): Some backdrop uvs are outside of the designated space on the atlas. Make sure all backdrop uvs are placed in the lower left corner of the atlas.')
			
	if(IsLightRigReferenced()):
		SetScrollFieldText(scrollField, '(OPTIONAL): Light rig referenced in scene. Clear out before sumbitting.')
	
	lightList = pm.ls(lights=True)
	if(lightList):
		for light in lightList:
			lightTransform = light.getParent()
			if(lightTransform):
				lightParent = lightTransform.getParent()
				if(lightParent):
					if(lightParent.nodeName() != 'custom_lights'):
						SetScrollFieldText(scrollField, '(OPTIONAL): Light %s is not parented under a "custom_lights" node.' % light.name())
			
	# if(otherMeshes):
		# SetScrollFieldText(scrollField, '(OPTIONAL): There are meshes in the scene that are unknown. Make sure these extra meshes are there for a reason')
		# for mesh in otherMeshes:	
			# SetScrollFieldText(scrollField, mesh.longName())
	
	overlappingShellList = []
	for transform in allMeshes:
		shape = transform.getShape()
		
		pm.select(clear=True)
		pm.select(transform)
		pm.hyperShade(shaderNetworksSelectMaterialNodes=True) 
		materialList = pm.ls(sl=True)
		if(not materialList):
			SetScrollFieldText(scrollField, '(REQUIRED): No materials assigned to mesh %s. Make sure everything is assigned to the main material and clear out all the rest.' % transform.nodeName())
		elif(len(materialList) > 1):
			SetScrollFieldText(scrollField, '(REQUIRED): More than one material assigned to mesh %s. Make sure everything is assigned to the main material and clear out all the rest.' % transform.nodeName())
		elif(materialList[0].name().startswith('lambert')):
			SetScrollFieldText(scrollField, '(REQUIRED): Mesh %s assigned to material not named right or assigned to lambert1.' % transform.nodeName())

		uvSets = transform.getShape().getUVSetNames()
		if(len(uvSets) < 2):
			SetScrollFieldText(scrollField, '(REQUIRED): Only on uv set assigned to mesh %s. Make sure mesh has LightMap uv set' % transform.nodeName())
		elif(uvSets[1] != 'LightMap'):
			SetScrollFieldText(scrollField, '(REQUIRED): Mesh node %s second uv set is named %s. Should be named "LightMap".' % (transform.nodeName(), uvSets[1]))
	
	for transform in litMeshes:
		shape = transform.getShape()
		
		resultList = pm.polyNormalPerVertex(shape.vtx, q=True, freezeNormal=True)
		for result in resultList:
			if(result == True):
				SetScrollFieldText(scrollField, '(REQUIRED): Mesh %s has normals that are locked. Turn on vertex normals and check for yellow normals. Fix with Normals->Unlock Normals.' % transform.nodeName())
				break	
	
		uvSets = transform.getShape().getUVSetNames()
		if(len(uvSets) == 2):
			if(uvSets[1] == 'LightMap'):	
				transform.setCurrentUVSetName('LightMap')
				if(not CheckUVSize(transform)):
					SetScrollFieldText(scrollField, '(REQUIRED): Uvs for object %s reach outside of the quadrant space. Check the UV editor.' % transform.nodeName())
				if(CheckForMissingUVs(transform)):
					SetScrollFieldText(scrollField, '(REQUIRED): There are faces on mesh %s that do not have UVs in the LightMap uvset.' % transform.nodeName())
	
	SetScrollFieldText(scrollField, 'CHECK FINISHED\n')
	
def CheckBackdropUVs(backdrop):
	uv = pm.polyListComponentConversion(backdrop, toUV=True)[0]
	uvList = pm.polyEditUV(uv, q=True, uValue=True, vValue=True)
	
	for i in range(0, len(uvList)):
		#both u and v must be within this range
		if(uvList[i] < 0 or uvList[i] > 0.0078125):
			return False
	
	return True
			
def CheckUVSize(object):
	uv = pm.polyListComponentConversion(object, toUV=True)[0]
	uvList = pm.polyEditUV(uv, q=True, uValue=True, vValue=True)
		
	boundingBox = mainmayautil.GetUVBoundingBox(uvList)
	bbSize = [boundingBox[1] - boundingBox[0], boundingBox[3] - boundingBox[2]]	
	
	if(bbSize[0] > 0.125 or bbSize[1] > 0.125):
		return False
	
	return True
	
def CheckForMissingUVs(object):
	uv = pm.polyListComponentConversion(object, toUV=True)[0]
	if(not uv):
		return True
	
	uvFaces = pm.polyListComponentConversion(uv, fromUV=True, toFace=True)[0]
	faces = pm.polyListComponentConversion(object, toFace=True)[0]

	if(len(pm.ls(uvFaces, flatten=True)) < len(pm.ls(faces, flatten=True))):
		return True
	
	return False
	
def WriteAtlasRefFile(dir, name, quad, dimensions='8', size='256'):
	xmlPath = '%s/lm_%s.xml' % (dir, name)
	
	root = ET.Element('atlasref')
	dimensionsElement = ET.Element('dimensions')
	quadx = ET.Element('quadx')
	quady = ET.Element('quady')

	root.append(dimensionsElement)
	root.append(quadx)
	root.append(quady)
	
	dimensionsElement.text = str(dimensions)
	quadx.text = str(quad[0])
	quady.text = str(quad[1])

	file = open(xmlPath, 'w')

	ET.ElementTree(root).write(file)

	file.close()
	
def PerformCleanup(referenceNode):
	referenceNode.remove()
	
#Set default option var preferences if they don't exist already
def SetDefaultPreferences():
	outputDir = mainmayautil.LoadOptionVar('LightBakeTools_OutputDir')
	if(not outputDir):
		#if the setting wasn't set set it to a default value
		outputDir = '%s/TempleRunOzNew/Assets/Oz/Textures/GameTextures/WimsyWoods/LightMapBakeOutput' % USERPROFILE
		mainmayautil.SaveOptionVar('LightBakeTools_OutputDir', outputDir)
		
	lightRigPath = mainmayautil.LoadOptionVar('LightBakeTools_LightRigPath')
	if(not lightRigPath):
		#if the setting wasn't set set it to a default value
		lightRigPath = '%s/TempleRunOzNew/Assets/Oz/Models/LightRig/whimsywoods_lightrig.mb' % USERPROFILE
		mainmayautil.SaveOptionVar('LightBakeTools_LightRigPath', lightRigPath)	
		
	autoAtlasUV = mainmayautil.LoadOptionVar('LightBakeTools_AutoAtlasUV')
	if(autoAtlasUV == None):
		mainmayautil.SaveOptionVar('LightBakeTools_AutoAtlasUV', 1)
	
def UVNormalToAtlasSelected(quadrant):
	tempArray = pm.ls(sl=True)
	if(not tempArray):
		raise pm.PopupError('Must select an object first')

	object = tempArray[0]
	
	for object in tempArray:
		if(pm.nodeType(object.getShape()) == 'mesh'):	
			UVNormalToAtlas(object, quadrant)
		else:
			pm.PopupError('Warning: One of the objects selected is not a mesh object')
	
def UVAtlasToNormalSelected():
	tempArray = pm.ls(sl=True)
	if(not tempArray):
		raise pm.PopupError('Must select a mesh object(s) first')

	object = tempArray
	
	for object in tempArray:
		if(pm.nodeType(object.getShape()) == 'mesh'):
			UVAtlasToNormal(object)	
		else:
			pm.PopupError('Warning: One of the objects selected is not a mesh object')
			
def UVAtlasToNormal(object):
	object.setCurrentUVSetName('LightMap')
	
	dimensions = mainmayautil.LoadOptionVar('LightBakeTools_Dimensions')
	
	if(not dimensions):
		raise pm.PopupError('Dimensions option var not found or set to 0. Check Lightmap Tool Settings.')	
	
	quadrant, size = FindAtlasQuadrantSize(object, dimensions)	
	
	scaleValue = 1.0/dimensions
	
	uv = pm.polyListComponentConversion(object, toUV=True)[0]
	pm.polyEditUV(uv, uValue=-1*quadrant[0]*scaleValue, vValue=-1*quadrant[1]*scaleValue)
	pm.polyEditUV(uv, scaleU=dimensions, scaleV=dimensions, pivotU=0.0, pivotV=0.0)
	
	return quadrant, size
	
def UVNormalToAtlas(object, quadrant):
	dimensions = mainmayautil.LoadOptionVar('LightBakeTools_Dimensions')
	
	if(not dimensions):
		raise pm.PopupError('Dimensions option var not found or set to 0. Check Lightmap Tool Settings.')
	
	scaleAmount = 1.0/dimensions
	if(quadrant[0] > (dimensions - 1)):
		raise pm.PopupError('quadrant value is more than the dimensions')
	if(quadrant[1] > (dimensions - 1)):
		raise pm.PopupError('quadrant value is more than the dimensions')
		
	object.setCurrentUVSetName('LightMap')
	uv = pm.polyListComponentConversion(object, toUV=True)[0]
	pm.polyEditUV(uv, scaleU=scaleAmount, scaleV=scaleAmount, pivotU=0.0, pivotV=0.0)	
	pm.polyEditUV(uv, uValue=quadrant[0]*scaleAmount, vValue=quadrant[1]*scaleAmount)	

def FindAtlasQuadrantSize(object, dimensions):
	print 'object is'
	print object
	uv = pm.polyListComponentConversion(object, toUV=True)[0]
	uvList = pm.polyEditUV(uv, q=True, uValue=True, vValue=True)
		
	boundingBox = mainmayautil.GetUVBoundingBox(uvList)
	print boundingBox

	bbCenter = [(boundingBox[0] + boundingBox[1]) / 2, (boundingBox[2] + boundingBox[3]) / 2]
	bbSize = [boundingBox[1] - boundingBox[0], boundingBox[3] - boundingBox[2]]
	
	print bbCenter	
	
	scaleValue = 1.0/dimensions
	
	uQuadrant = math.floor(bbCenter[0]/scaleValue)
	print uQuadrant
	vQuadrant = math.floor(bbCenter[1]/scaleValue)
	print vQuadrant
	
	return [uQuadrant, vQuadrant], bbSize
	
def CreateUVSets(object):
	objectShape = object.getShape()
	objectShape.createUVSet('LightMap')
	objectShape.createUVSet('LightMapSource')
	
def CheckUVSets(object):
	objectShape = object.getShape()
	
	uvSets = objectShape.getUVSetNames()
	
	try:
		if(uvSets[1] != 'LightMap'):
			pm.PopupError('(OPTIONAL): %s second uv set is not named LightMap' % object.nodeName())
	except:
		pm.PopupError('(OPTIONAL): %s object has no LightMap uv set' % object.nodeName())

	try:
		if(uvSets[2] != 'LightMapSource'):
			pm.PopupError('(OPTIONAL): %s second uv set is not named LightMapSource' % object.nodeName())
	except:
		pm.PopupError('(OPTIONAL): %s object has no LightMapSource uv set' % object.nodeName())
		
	if(len(uvSets) > 3):
		pm.PopupError('(OPTIONAL): %s has more than 3 uv sets' % object.nodeName())
		
def SetBakeDefaultSettings(bakeSetNode):
	for attr, value in gBakeSettings.iteritems():
		bakeSetNode.setAttr(attr, value)

def SetAOBakeSettings(bakeSetNode):
	#bakeSetNode.colorMode.set(4) # Custom Shader
	#bakeSetNode.backgroundColor.set([1.0,1.0,1.0])
	
	changedSettings = {}
	
	for attr, value in gBakeAOSettings.iteritems():
		changedSettings[attr] = bakeSetNode.getAttr(attr)
		bakeSetNode.setAttr(attr, value)	
	
	surfaceShader = pm.shadingNode('surfaceShader', asShader=True)
	nodeName = mm.eval('mrCreateCustomNode -asTexture "" mib_amb_occlusion;')
	mibAONode = pm.ls(nodeName)[0]
	
	mibAONode.outValue >> surfaceShader.outColor
	surfaceShader.outColor >> bakeSetNode.customShader
	
	for attr, value in gAOSettings.iteritems():
		mibAONode.setAttr(attr, value)
		
	return surfaceShader, mibAONode, changedSettings
		
def SetMentalRaySettings():
	mm.eval('setCurrentRenderer mentalRay;')
	
	tempArray = pm.ls('miDefaultOptions', type='mentalrayOptions')
	
	if(not tempArray):
		print 'Error: Mental ray settings node not found. Must open render globals and set renderer to mental ray'
		return
	
	miDefaultOptions = tempArray[0]

	for attr, value in gMentalRaySettings.iteritems():
		miDefaultOptions.setAttr(attr, value)
		
def CreateAndAssignBakeSet(type, items, name):
	bakeSetName = mm.eval('createBakeSet( "%s", "%s" );' % (name,type))
	for item in items:
		mm.eval('assignBakeSet( "%s", "%s" );' % (bakeSetName, item.longName()))
	mm.eval('showBakeSetAE( "%s" );' % bakeSetName)
	
	bakeSetNode = pm.ls(bakeSetName)[0]
	
	return bakeSetNode

def GetBakeSetNode():
	nodeList = pm.ls('troz_texture_bake_set', type='textureBakeSet')
	if(not nodeList):
		return None
		
	return nodeList[0]
	
def GetObjectsBakeSet(object):
	objectShape = object.getShape()

	bakeSetArray = pm.ls(type='textureBakeSet')
	for bakeSetNode in bakeSetArray:
		bakeSetMembers = bakeSetNode.members()
		for member in bakeSetMembers:
			if(member == objectShape):
				return bakeSetNode
	return None
	
def SetScrollFieldText(scrollField, text):
	scrollField.setText('%s%s\n' % (scrollField.getText(), text))