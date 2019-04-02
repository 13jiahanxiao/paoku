import pymel.core as pm
import maya.mel as mm

import math
import shutil
import os

import mainmayautil

USERPROFILE = os.environ['userprofile'].replace('\\','/')

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
	
gMentalRaySettings = {
	'finalGather' : 1,
	'rayTracing' : 1,
	'finalGatherTraceDiffuse' : 1,
	'finalGatherPresampleDensity' : 5,
	'finalGatherBounceScale' : [1.5,1.5,1.5] #This increases the radiosity or secondary bounce light. 
	}	
	
gAOSettings = {
	'samples' : 128
	}
	
def ReferenceRigAndBakeSelected():
	tempArray = pm.ls(sl=True)
	if(not tempArray):
		raise pm.PopupError('Must select an object first')

	object = tempArray[0]
	bakeSetNode = GetObjectsBakeSet(object)
	
	if(not bakeSetNode):
		raise pm.PopupError('Bake set node not found for object %s. Run the bake setup script first.' % object.nodeName())
	
	referenceNode = ReferenceLightRig()	
	
	PerformBake(object, bakeSetNode)
	
	PerformCleanup(referenceNode)
	
def SetupSelectedForBake():
	SetDefaultPreferences()

	tempArray = pm.ls(sl=True)
	if(not tempArray):
		raise pm.PopupError('Must select an object first')

	object = tempArray[0]
	objectShape = object.getShape()

	bakeSetNode = GetObjectsBakeSet(object)
	bakeSetNameStr = 'lm_%s_textureBakeSet' % object.nodeName()
	
	if(not bakeSetNode):
		bakeSetNode = CreateAndAssignBakeSet('textureBakeSet', object, 'lm_%s_textureBakeSet' % object.nodeName())
		#bakeSetNode.addAttr('lightmapPath', dataType='string')
	
	SetBakeDefaultSettings(bakeSetNode)
	
	SetMentalRaySettings()
	
	#Make sure that bake set has the proper name
	if(bakeSetNode.nodeName() != bakeSetNameStr):
		#Need to pop up warning for now. Not sure why but something doesn't get updated before calling name change command. This causes errors to happen after the script is finished.
		pm.PopupError('WARNING: Name of node is inconsistant with name of bake set. Please rename bake set.')

	return bakeSetNode
	
def ReferenceLightRig():
	selectedNodes = pm.ls(sl=True)

	lightRigPath = mainmayautil.LoadOptionVar('LightBakeTools_LightRigPath')
	referenceNode = pm.createReference(lightRigPath)
	
	pm.select(selectedNodes)
	
	return referenceNode
	
def BakeSelected():
	tempArray = pm.ls(sl=True)
	if(not tempArray):
		raise pm.PopupError('Must select an object first')

	object = tempArray[0]
	bakeSetNode = GetObjectsBakeSet(object)
	
	if(not bakeSetNode):
		raise pm.PopupError('Bake set node not found for object %s. Run the bake setup script first.' % object.nodeName())

	PerformBake(object, bakeSetNode)

def AOBakeSelected():
	tempArray = pm.ls(sl=True)
	if(not tempArray):
		raise pm.PopupError('Must select an object first')

	object = tempArray[0]
	bakeSetNode = GetObjectsBakeSet(object)
	
	if(not bakeSetNode):
		raise pm.PopupError('Bake set node not found for object %s. Run the bake setup script first.' % object.nodeName())

	PerformAOBake(object, bakeSetNode)
	
def PerformBake(object, bakeSetNode):
	#lightmapPath = bakeSetNode.lightmapPath.get()
	
	outputDir = mainmayautil.LoadOptionVar('LightBakeTools_OutputDir')
	
	lightmapPath = '%s/lm_%s.tga' % (outputDir, object.nodeName())	
	
	autoAtlasUV = mainmayautil.LoadOptionVar('LightBakeTools_AutoAtlasUV')	
	if(autoAtlasUV):
		quadrant = UVAtlasToNormal(object)
	
	#These 2 lines are needed to set mental ray up properly for baking. Something with creating initial bake sets.
	pm.select(clear=True);
	mm.eval('mrBakeToTexture false;')
	
	pm.select(object)
	
	try:
		outputPath = pm.convertLightmapSetup(bakeAll=False, camera='persp', shadows=True, keepOrgSG=True, showcpv=True)[0]
	except:
		UVNormalToAtlas(object, quadrant)
		raise pm.PopupError('Lightmap bake failed. Check script editor')
		
	if(autoAtlasUV):	
		UVNormalToAtlas(object, quadrant)

	mainmayautil.ForceCopyFile(outputPath, lightmapPath)	
	
def PerformAOBake(object, bakeSetNode):
	outputDir = mainmayautil.LoadOptionVar('LightBakeTools_OutputDir')
	
	aoPath = '%s/lm_%s_ao.tga' % (outputDir, object.nodeName())	
	
	autoAtlasUV = mainmayautil.LoadOptionVar('LightBakeTools_AutoAtlasUV')	
	if(autoAtlasUV):
		quadrant = UVAtlasToNormal(object)

	#These 2 lines are needed to set mental ray up properly for baking. Something with creating initial bake sets.
	pm.select(clear=True);
	mm.eval('mrBakeToTexture false;')
	
	surfaceShader, mibAONode = SetAOBakeSettings(bakeSetNode)

	pm.select(object)

	try:
		outputPath = pm.convertLightmapSetup(bakeAll=False, camera='persp', shadows=True, keepOrgSG=True, showcpv=True)[0]
	except:
		UVNormalToAtlas(object, quadrant)
		raise pm.PopupError('Lightmap bake failed. Check script editor')	
		
	if(autoAtlasUV):	
		UVNormalToAtlas(object, quadrant)
		
	bakeSetNode.colorMode.set(gBakeSettings['colorMode'])
	pm.delete(surfaceShader)
	pm.delete(mibAONode)

	mainmayautil.ForceCopyFile(outputPath, aoPath)			
	
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
	
	UVNormalToAtlas(object, quadrant)
	
def UVAtlasToNormalSelected():
	tempArray = pm.ls(sl=True)
	if(not tempArray):
		raise pm.PopupError('Must select an object first')

	object = tempArray[0]
	
	UVAtlasToNormal(object)	
	
def UVAtlasToNormal(object):
	quadrant = FindAtlasQuadrant(object)
	
	object.setCurrentUVSetName('LightMap')
	uv = pm.polyListComponentConversion(object, toUV=True)[0]
	pm.polyEditUV(uv, uValue=-1*quadrant[0]*0.125, vValue=-1*quadrant[1]*0.125)
	pm.polyEditUV(uv, scaleU=8.0, scaleV=8.0, pivotU=0.0, pivotV=0.0)
	
	return quadrant
	
def UVNormalToAtlas(object, quadrant):
	object.setCurrentUVSetName('LightMap')
	uv = pm.polyListComponentConversion(object, toUV=True)[0]
	pm.polyEditUV(uv, scaleU=0.125, scaleV=0.125, pivotU=0.0, pivotV=0.0)	
	pm.polyEditUV(uv, uValue=quadrant[0]*0.125, vValue=quadrant[1]*0.125)	

def FindAtlasQuadrant(object):
	uv = pm.polyListComponentConversion(object, toUV=True)[0]
	uvList = pm.polyEditUV(uv, q=True, uValue=True, vValue=True)
		
	boundingBox = mainmayautil.GetUVBoundingBox(uvList)
	print boundingBox

	bbCenter = [(boundingBox[0] + boundingBox[1]) / 2, (boundingBox[2] + boundingBox[3]) / 2]
	print bbCenter	
	
	uQuadrant = math.floor(bbCenter[0]/0.125)
	print uQuadrant
	vQuadrant = math.floor(bbCenter[1]/0.125)
	print vQuadrant
	
	return [uQuadrant, vQuadrant]
	
def CreateUVSets(object):
	objectShape = object.getShape()
	objectShape.createUVSet('LightMap')
	objectShape.createUVSet('LightMapSource')
	
def CheckUVSets(object):
	objectShape = object.getShape()
	
	uvSets = objectShape.getUVSetNames()
	
	try:
		if(uvSets[1] != 'LightMap'):
			pm.PopupError('WARNING: %s second uv set is not named LightMap' % object.nodeName())
	except:
		pm.PopupError('WARNING: %s object has no LightMap uv set' % object.nodeName())

	try:
		if(uvSets[2] != 'LightMapSource'):
			pm.PopupError('WARNING: %s second uv set is not named LightMapSource' % object.nodeName())
	except:
		pm.PopupError('WARNING: %s object has no LightMapSource uv set' % object.nodeName())
		
	if(len(uvSets) > 3):
		pm.PopupError('WARNING: %s has more than 3 uv sets' % object.nodeName())
		
def SetBakeDefaultSettings(bakeSetNode):
	for attr, value in gBakeSettings.iteritems():
		bakeSetNode.setAttr(attr, value)

def SetAOBakeSettings(bakeSetNode):
	bakeSetNode.colorMode.set(4) # Custom Shader
	
	surfaceShader = pm.shadingNode('surfaceShader', asShader=True)
	nodeName = mm.eval('mrCreateCustomNode -asTexture "" mib_amb_occlusion;')
	mibAONode = pm.ls(nodeName)[0]
	
	mibAONode.outValue >> surfaceShader.outColor
	surfaceShader.outColor >> bakeSetNode.customShader
	
	for attr, value in gAOSettings.iteritems():
		mibAONode.setAttr(attr, value)
		
	return surfaceShader, mibAONode
		
def SetMentalRaySettings():
	mm.eval('setCurrentRenderer mentalRay;')
	
	tempArray = pm.ls('miDefaultOptions', type='mentalrayOptions')
	
	if(not tempArray):
		raise pm.PopupError('Mental ray settings node not found. Open the Maya render globals (settings) then try again.') 
	
	miDefaultOptions = tempArray[0]

	for attr, value in gMentalRaySettings.iteritems():
		miDefaultOptions.setAttr(attr, value)
		
def CreateAndAssignBakeSet(type, item, name):
	bakeSetName = mm.eval('createBakeSet( "%s", "%s" );' % (name,type))
	mm.eval('assignBakeSet( "%s", "%s" );' % (bakeSetName, item.longName()))
	mm.eval('showBakeSetAE( "%s" );' % bakeSetName)
	
	bakeSetNode = pm.ls(bakeSetName)[0]
	
	return bakeSetNode
	
def GetObjectsBakeSet(object):
	objectShape = object.getShape()

	bakeSetArray = pm.ls(type='textureBakeSet')
	for bakeSetNode in bakeSetArray:
		bakeSetMembers = bakeSetNode.members()
		for member in bakeSetMembers:
			if(member == objectShape):
				return bakeSetNode
	return None
	
	
			
	