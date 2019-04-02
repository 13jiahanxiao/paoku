import pymel.core as pm

#The UI for this uses pymel's AutoLayout class along with "with" statements instead of the usual Maya formLayouts. Using this pymel class is much easier and 
#saves a lot of time spent scripting UI. 
from pymel.core.uitypes import AutoLayout

import mainmayautil
import lightbakeapi

class ConsistencyCheckUI(pm.uitypes.Window):
	winName = 'ConsistencyCheckUI'
   
	def __new__(cls):
		if pm.window(cls.winName, exists=True):
			pm.deleteUI(cls.winName)
		self = pm.window('ConsistencyCheckUI', title="Consistency Check")
		return pm.uitypes.Window.__new__(cls, self)

	def __init__(self, title='Consistency Check'):				
		with AutoLayout(orientation=AutoLayout.VERTICAL, ratios=[1,15]):
			pm.text(label='Output:', align='left')
			self.outputField = pm.scrollField(editable=False)
			# with AutoLayout(orientation=AutoLayout.HORIZONTAL, ratios=[5,1,1]):
				# pm.text(label='') #spacer
				# pm.button(label='Bake', command=self.BakeButtonOnClick)
				# pm.button(label='Close', command=self.CancelButtonOnClick)
		
		self.show()
		self.setWidthHeight([500,500])
		
		self.RunCheck()
	
	def RunCheck(self):
		lightbakeapi.ConsistencyCheck(self.outputField)