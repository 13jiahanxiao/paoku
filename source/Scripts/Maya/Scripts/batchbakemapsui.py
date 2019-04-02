import pymel.core as pm

#The UI for this uses pymel's AutoLayout class along with "with" statements instead of the usual Maya formLayouts. Using this pymel class is much easier and 
#saves a lot of time spent scripting UI. 
from pymel.core.uitypes import AutoLayout

import mainmayautil
import lightbakeapi

class BatchBakeMapsUI(pm.uitypes.Window):
	winName = 'BatchBakeMapsUI'
   
	def __new__(cls):
		if pm.window(cls.winName, exists=True):
			pm.deleteUI(cls.winName)
		self = pm.window('BatchBakeMapsUI', title="Batch Bake Maps")
		return pm.uitypes.Window.__new__(cls, self)

	def __init__(self, title='Batch Bake Maps'):				
		with AutoLayout(orientation=AutoLayout.VERTICAL, ratios=[1,1,1,1,1,1,1,1,1,1,7,1]):
			with AutoLayout(orientation=AutoLayout.HORIZONTAL, ratios=[1,3,1]):
				pm.text(label='Source Directory:', align='left')
				self.sourceDirField = pm.textField()
				pm.button(label="...", command=self.SourceDirBrowseButtonOnClick)
			with AutoLayout(orientation=AutoLayout.HORIZONTAL, ratios=[1,3,1]):
				pm.text(label='Output Directory:', align='left')
				self.outputDirField = pm.textField()
				pm.button(label="...", command=self.OutputDirBrowseButtonOnClick)				
			with AutoLayout(orientation=AutoLayout.HORIZONTAL, ratios=[1,3,1]):
				pm.text(label='Light Rig File:', align='left')
				self.lightRigField = pm.textField()
				pm.button(label="...", command=self.LightRigBrowseButtonOnClick)
			with AutoLayout(orientation=AutoLayout.HORIZONTAL, ratios=[1,3]):
				pm.text(label='Inclusion Filters:', align='left')
				self.inclusionField = pm.textField()
			with AutoLayout(orientation=AutoLayout.HORIZONTAL, ratios=[1,3]):
				pm.text(label='Exclusion Filters:', align='left')
				self.exclusionField = pm.textField()
			self.checkCB = pm.checkBox(label="Consistancy Check", value=True)
			self.setupCB = pm.checkBox(label="Run Setup", value=True)
			self.lightmapsCB = pm.checkBox(label="Bake Lightmaps", value=True)
			self.aoCB = pm.checkBox(label="Bake Ambient Occlusion", value=True)
			pm.text(label='Output:', align='left')
			self.outputField = pm.scrollField(editable=False)
			with AutoLayout(orientation=AutoLayout.HORIZONTAL, ratios=[5,1,1]):
				pm.text(label='') #spacer
				pm.button(label='Bake', command=self.BakeButtonOnClick)
				pm.button(label='Close', command=self.CancelButtonOnClick)
		
		self.show()
		self.setWidthHeight([500,500])
		
		self.SetDefaults()
		
	def SetDefaults(self):
		sourceDir = mainmayautil.LoadOptionVar('LightBakeTools_BatchBakeMaps_SourceDir')
		if(sourceDir):
			self.sourceDirField.setText(sourceDir)

		outputDir = mainmayautil.LoadOptionVar('LightBakeTools_OutputDir')
		if(outputDir):
			self.outputDirField.setText(outputDir)
			
		lightRigPath = mainmayautil.LoadOptionVar('LightBakeTools_BatchBakeMaps_LightRigPath')
		if(lightRigPath):
			self.lightRigField.setText(lightRigPath)

		inclusionFilters = mainmayautil.LoadOptionVar('LightBakeTools_BatchBakeMaps_InclusionFilters')
		if(inclusionFilters):
			self.inclusionField.setText(inclusionFilters)

		exclusionFilters = mainmayautil.LoadOptionVar('LightBakeTools_BatchBakeMaps_ExclusionFilters')
		if(exclusionFilters):
			self.exclusionField.setText(exclusionFilters)			
			
		check = mainmayautil.LoadOptionVar('LightBakeTools_BatchBakeMaps_Check')
		if(check != None):
			self.checkCB.setValue(check)		
			
		setup = mainmayautil.LoadOptionVar('LightBakeTools_BatchBakeMaps_Setup')
		if(setup != None):
			self.setupCB.setValue(setup)		

		lightmaps = mainmayautil.LoadOptionVar('LightBakeTools_BatchBakeMaps_Lightmaps')
		if(lightmaps != None):
			self.lightmapsCB.setValue(lightmaps)		
		
		ao = mainmayautil.LoadOptionVar('LightBakeTools_BatchBakeMaps_AO')
		if(ao != None):
			self.aoCB.setValue(ao)				
		
	def BakeButtonOnClick(self, default):
		self.outputField.clear()
		
		sourceDir = self.sourceDirField.getText()
		outputDir = self.outputDirField.getText()
		lightRig = self.lightRigField.getText()
		inclusionFilters = self.inclusionField.getText()
		exclusionFilters = self.exclusionField.getText()
		check = self.checkCB.getValue()
		setup = self.setupCB.getValue()
		lightmaps = self.lightmapsCB.getValue()
		ao = self.aoCB.getValue()
		
		self.SaveSettings()
		
		lightbakeapi.BatchBakeMaps(sourceDir, lightRig, inclusionFilters, exclusionFilters, check, setup, lightmaps, ao, self.outputField)
		
	def SaveSettings(self):
		mainmayautil.SaveOptionVar('LightBakeTools_BatchBakeMaps_SourceDir', self.sourceDirField.getText())
		mainmayautil.SaveOptionVar('LightBakeTools_OutputDir', self.outputDirField.getText())
		mainmayautil.SaveOptionVar('LightBakeTools_BatchBakeMaps_LightRigPath', self.lightRigField.getText())
		mainmayautil.SaveOptionVar('LightBakeTools_BatchBakeMaps_InclusionFilters', self.inclusionField.getText())
		mainmayautil.SaveOptionVar('LightBakeTools_BatchBakeMaps_ExclusionFilters', self.exclusionField.getText())
		mainmayautil.SaveOptionVar('LightBakeTools_BatchBakeMaps_Check', self.checkCB.getValue())	
		mainmayautil.SaveOptionVar('LightBakeTools_BatchBakeMaps_Setup', self.setupCB.getValue())	
		mainmayautil.SaveOptionVar('LightBakeTools_BatchBakeMaps_Lightmaps', self.lightmapsCB.getValue())
		mainmayautil.SaveOptionVar('LightBakeTools_BatchBakeMaps_AO', self.aoCB.getValue())			
		
	def CancelButtonOnClick(self, default):
		self.SaveSettings()
		pm.deleteUI(self)
		
	def SourceDirBrowseButtonOnClick(self, default):
		tempArray = pm.fileDialog2(fileMode=2, dialogStyle=2)
		if(not tempArray):
			return
		dirPath = tempArray[0]
		self.sourceDirField.setText(dirPath)

	def OutputDirBrowseButtonOnClick(self, default):
		tempArray = pm.fileDialog2(fileMode=2, dialogStyle=2)
		if(not tempArray):
			return
		dirPath = tempArray[0]
		self.outputDirField.setText(dirPath)
		
	def LightRigBrowseButtonOnClick(self, default):
		multipleFilters = "Maya Files (*.ma *.mb);;Maya ASCII (*.ma);;Maya Binary (*.mb);;All Files (*.*)"
		filePath = pm.fileDialog2(fileFilter=multipleFilters, fileMode=1, dialogStyle=2)[0]
		self.lightRigField.setText(filePath)			