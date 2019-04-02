import pymel.core as pm

#The UI for this uses pymel's AutoLayout class along with "with" statements instead of the usual Maya formLayouts. Using this pymel class is much easier and 
#saves a lot of time spent scripting UI. 
from pymel.core.uitypes import AutoLayout

import mainmayautil
import lightbakeapi

class LightBakeSettingsUI(pm.uitypes.Window):
	winName = 'LightBakeSettingsUI'
   
	def __new__(cls):
		if pm.window(cls.winName, exists=True):
			pm.deleteUI(cls.winName)
		self = pm.window('LightBakeSettingsUI', title="Light Bake Settings")
		return pm.uitypes.Window.__new__(cls, self)

	def __init__(self, title='Lightmap Bake Settings'):				
		with AutoLayout(orientation=AutoLayout.VERTICAL, ratios=[1,1,1,1,1,1,1,3,1]):
			with AutoLayout(orientation=AutoLayout.HORIZONTAL, ratios=[1,3,1]):
				pm.text(label='Output Directory:', align='left')
				self.fileDirField = pm.textField()
				pm.button(label="...", command=self.DirBrowseButtonOnClick)
			with AutoLayout(orientation=AutoLayout.HORIZONTAL, ratios=[1,3,1]):
				pm.text(label='Light Rig File:', align='left')
				self.lightRigField = pm.textField()
				pm.button(label="...", command=self.LightRigBrowseButtonOnClick)
			with AutoLayout(orientation=AutoLayout.HORIZONTAL, ratios=[1,3,1]):
				pm.text(label='Lightmap Preview File:', align='left')
				self.lightmapPreviewField = pm.textField()
				pm.button(label="...", command=self.LightmapPreviewBrowseButtonOnClick)
			self.autoAtlasCB = pm.checkBox(label="Normalize UVs on bake", align='left', value=True)
			self.renderSettingsCB = pm.checkBox(label="Set render globals on setup", value=True)
			self.bakeSetCB = pm.checkBox(label="Set bake set settings on setup", value=True)			
			self.bakeSetAOCB = pm.checkBox(label="Set bake set settings/shader on AO bake", value=True)
			with AutoLayout(orientation=AutoLayout.HORIZONTAL, ratios=[1,1,3]):
				pm.text(label='Lightmap Atlas Dimensions:', align='left')
				self.dimensionsField = pm.intField(value=8)			
			pm.text(label='') #spacer
			with AutoLayout(orientation=AutoLayout.HORIZONTAL, ratios=[5,1,1]):
				pm.text(label='') #spacer
				pm.button(label='Save', command=self.SaveButtonOnClick)
				pm.button(label='Cancel', command=self.CancelButtonOnClick)
		
		self.show()
		self.setWidthHeight([500,300])
		
		self.SetDefaults()
	
	def SetDefaults(self):
		lightbakeapi.SetDefaultPreferences()
		
		outputDir = mainmayautil.LoadOptionVar('LightBakeTools_OutputDir')
		if(outputDir):
			self.fileDirField.setText(outputDir)
		
		lightRigPath = mainmayautil.LoadOptionVar('LightBakeTools_LightRigPath')
		if(lightRigPath):
			self.lightRigField.setText(lightRigPath)

		lightmapPreviewPath = mainmayautil.LoadOptionVar('LightBakeTools_LightmapPreviewPath')
		if(lightmapPreviewPath):
			self.lightmapPreviewField.setText(lightmapPreviewPath)
		
		autoAtlasUV = mainmayautil.LoadOptionVar('LightBakeTools_AutoAtlasUV')
		if(autoAtlasUV != None):
			self.autoAtlasCB.setValue(autoAtlasUV)
		
		dimensions = mainmayautil.LoadOptionVar('LightBakeTools_Dimensions')
		if(dimensions):
			self.dimensionsField.setValue(dimensions)
			
		renderSettings = mainmayautil.LoadOptionVar('LightBakeTools_RenderSettings')
		if(renderSettings != None):
			self.renderSettingsCB.setValue(renderSettings)

		bakeSet = mainmayautil.LoadOptionVar('LightBakeTools_BakeSetSetup')
		if(bakeSet != None):
			self.bakeSetCB.setValue(bakeSet)			

		bakeSetAO = mainmayautil.LoadOptionVar('LightBakeTools_BakeSetAOSetup')
		if(bakeSetAO != None):
			self.bakeSetAOCB.setValue(bakeSetAO)				
			
	def SaveButtonOnClick(self, default):
		mainmayautil.SaveOptionVar('LightBakeTools_OutputDir', self.fileDirField.getText())
		mainmayautil.SaveOptionVar('LightBakeTools_LightRigPath', self.lightRigField.getText())
		mainmayautil.SaveOptionVar('LightBakeTools_LightmapPreviewPath', self.lightmapPreviewField.getText())
		mainmayautil.SaveOptionVar('LightBakeTools_AutoAtlasUV', self.autoAtlasCB.getValue())
		mainmayautil.SaveOptionVar('LightBakeTools_Dimensions', self.dimensionsField.getValue())
		mainmayautil.SaveOptionVar('LightBakeTools_RenderSettings', self.renderSettingsCB.getValue())
		mainmayautil.SaveOptionVar('LightBakeTools_BakeSetSetup', self.bakeSetCB.getValue())		
		mainmayautil.SaveOptionVar('LightBakeTools_BakeSetAOSetup', self.bakeSetAOCB.getValue())	
		pm.deleteUI(self)
		
	def CancelButtonOnClick(self, default):
		pm.deleteUI(self)
		
	def DirBrowseButtonOnClick(self, default):
		dirPath = pm.fileDialog2(fileMode=2, dialogStyle=2)[0]
		self.fileDirField.setText(dirPath)
		
	def LightRigBrowseButtonOnClick(self, default):
		multipleFilters = "Maya Files (*.ma *.mb);;Maya ASCII (*.ma);;Maya Binary (*.mb);;All Files (*.*)"
		filePath = pm.fileDialog2(fileFilter=multipleFilters, fileMode=1, dialogStyle=2)[0]
		self.lightRigField.setText(filePath)		
		
	def LightmapPreviewBrowseButtonOnClick(self, default):
		multipleFilters = "Targa Files (*.tga);;All Files (*.*)"
		filePath = pm.fileDialog2(fileFilter=multipleFilters, fileMode=1, dialogStyle=2)[0]	
		self.lightmapPreviewField.setText(filePath)	