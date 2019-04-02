import pymel.core as pm
import maya.mel as mm
import mainmayautil

def DecalVertOrderFix():
	tempList = pm.ls(sl=True)
	if(not tempList):
		pm.PopupError('Must select an object first')
	
	object = tempList[0]
	objectName = object.nodeName()
	
	decalList = pm.polySeparate()
	decalList.pop()
	
	pm.delete(decalList, constructionHistory=True)
	
	decalDataList = []
	for decalObject in decalList:
		decalDataDict = {}
		
		boundingBox = pm.polyEvaluate(decalObject, boundingBox=True)
		
		center = mainmayautil.GetBoundingBoxCenter(boundingBox)
		
		decalDataDict['object'] = decalObject
		decalDataDict['center'] = center

		decalDataList.append(decalDataDict)
	
	orderedList = []
	while(decalDataList):
		min = decalDataList[0]
		for i in range(1, len(decalDataList)):
			if(decalDataList[i]['center'][2] < min['center'][2]):
				min = decalDataList[i]
		decalDataList.remove(min)
		orderedList.append(min['object'])
		
	newObject = pm.polyUnite(orderedList, constructionHistory=True)[0]
	pm.delete(newObject, constructionHistory=True)
	newObject.rename(objectName)
		
		
				
			
		
		
		
	
	

