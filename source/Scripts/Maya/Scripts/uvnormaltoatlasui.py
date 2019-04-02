import pymel.core as pm

#The UI for this uses pymel's AutoLayout class along with "with" statements instead of the usual Maya formLayouts. Using this pymel class is much easier and 
#saves a lot of time spent scripting UI. 
from pymel.core.uitypes import AutoLayout

import mainmayautil
import lightbakeapi

class UVNormalToAtlasUI(pm.uitypes.Window):
	winName = 'UVNormalToAtlasUI'
   
	def __new__(cls):
		if pm.window(cls.winName, exists=True):
			pm.deleteUI(cls.winName)
		self = pm.window('UVNormalToAtlasUI', title="Normal To Atlas")
		return pm.uitypes.Window.__new__(cls, self)

	def __init__(self, title='Lightmap Bake Settings'):
		dimensions = mainmayautil.LoadOptionVar('LightBakeTools_Dimensions')
		
		if(not dimensions):
			dimensions = 8	
	
		with AutoLayout(orientation=AutoLayout.VERTICAL, ratios=[1,1,3,1]):
			pm.text(label='Quadrant:', align='left')
			with AutoLayout(orientation=AutoLayout.HORIZONTAL, ratios=[1,1,1,1,5]):
				pm.text(label='U')
				self.uField = pm.intField(value=1, minValue=1, maxValue=dimensions)
				pm.text(label='V')
				self.vField = pm.intField(value=1, minValue=1, maxValue=dimensions)
				pm.text(label='') #spacer
			pm.text(label='') #spacer
			with AutoLayout(orientation=AutoLayout.HORIZONTAL, ratios=[5,1,1]):
				pm.text(label='') #spacer
				pm.button(label='Apply', command=self.ApplyButtonOnClick)
				pm.button(label='Cancel', command=self.CancelButtonOnClick)	
		self.show()
		self.setWidthHeight([300,200])		
				
	def ApplyButtonOnClick(self, default):
		uValue = self.uField.getValue()
		vValue = self.vField.getValue()
		
		lightbakeapi.UVNormalToAtlasSelected([uValue - 1,vValue - 1])
		
		pm.deleteUI(self)
		
	def CancelButtonOnClick(self, default):
		pm.deleteUI(self)