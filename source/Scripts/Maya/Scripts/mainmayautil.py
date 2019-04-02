import pymel.core as pm
import shutil
import os

USERPROFILE = os.environ['USERPROFILE'].replace('\\','/')

def SaveOptionVar(name, value):
	pm.optionVar[name] = value
	
def LoadOptionVar(name):
	optionVarValue = None
	if name in pm.optionVar:
		optionVarValue = pm.optionVar[name]

	return optionVarValue
	
def GetUVBoundingBox(uvList):
    uMin = uvList[0]
    uMax = uvList[0]
    vMin = uvList[1]
    vMax = uvList[1]
    for i in range(0, len(uvList)):
		if(not i % 2): # even
			if(uvList[i] < uMin):
				uMin = uvList[i]
			if(uvList[i] > uMax):
				uMax = uvList[i]
		else: # odd
			if(uvList[i] < vMin):
				vMin = uvList[i]
			if(uvList[i] > vMax):
				vMax = uvList[i]
	
    return [uMin, uMax, vMin, vMax]
	
def ForceCopyFile(src, dest):
	if(os.path.exists(dest)):
		os.remove(dest)
	# try:
		# output = shutil.copyfile(src, dest)
		# print output
	# except:
		# raise pm.PopupError('File copy failed.\n%s\n%s' % (src, dest))
	output = shutil.copy2(src, dest)
	print output
	
def GetBoundingBoxCenter(boundingBox):
	xVal = (boundingBox[0][0] + boundingBox[0][1])/2
	yVal = (boundingBox[1][0] + boundingBox[1][1])/2
	zVal = (boundingBox[2][0] + boundingBox[2][1])/2

	return (xVal, yVal, zVal)
	
def FixTextureUserPaths():
	fileList = pm.ls(type='file')
	for file in fileList:
		texturePath = file.fileTextureName.get()
		texturePath = texturePath[texturePath.find('/TempleRunOzNew/'):]
		texturePath = USERPROFILE + texturePath
		file.fileTextureName.set(texturePath)
		
def FixClipPlanes():
	cameraList = pm.ls(cameras=True)
	
	for camera in cameraList:
		camera.nearClipPlane.set(1.0)
		camera.farClipPlane.set(100000)	
		
def CreateNewTextureWithPlace2d(textureName):
	texture = pm.shadingNode('file', asTexture=True, name=textureName)
	place2d = pm.shadingNode('place2dTexture', asUtility=True)
	
	place2d.coverage >> texture.coverage
	place2d.outUvFilterSize >> texture.uvFilterSize
	place2d.outUV >> texture.uvCoord	
	place2d.vertexCameraOne >> texture.vertexCameraOne	
	place2d.vertexUvThree >> texture.vertexUvThree
	place2d.vertexUvTwo >> texture.vertexUvTwo
	place2d.vertexUvOne >> texture.vertexUvOne
	place2d.noiseUV >> texture.noiseUV
	place2d.rotateUV >> texture.rotateUV	
	place2d.offset >> texture.offset
	place2d.repeatUV >> texture.repeatUV		
	place2d.wrapV >> texture.wrapV
	place2d.wrapU >> texture.wrapU
	place2d.stagger >> texture.stagger
	place2d.mirrorV >> texture.mirrorV	
	place2d.mirrorU >> texture.mirrorU	
	place2d.rotateFrame >> texture.rotateFrame
	place2d.translateFrame >> texture.translateFrame	

	return texture, place2d