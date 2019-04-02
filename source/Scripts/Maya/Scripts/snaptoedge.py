import pymel.core as pm

def SnapToEdge(aimVector=[1.0,0.0,0.0], upVector = [0.0,0.0,1.0]):
	vertArray = pm.ls(sl=True, fl=True, type='float3')
	
	if(len(vertArray) != 2):
		raise pm.PopupError('Must select object then 2 verts')
	
	object = pm.group(name='dummy', empty=True)
	
	vert1 = vertArray[0]
	vert2 = vertArray[1]

	vector1 = pm.datatypes.Vector(vert1.getPosition(space='world'))
	vector1 = vector1*0.01
	vector2 = pm.datatypes.Vector(vert2.getPosition(space='world'))
	vector2 = vector2*0.01

	curve = pm.curve(d=1, p=[vector1*100, vector2*100], k=[0,1])

	constraint1 = pm.geometryConstraint(curve, object)
	constraint2 = pm.tangentConstraint(curve, object, aimVector=aimVector, upVector=upVector)

	pm.delete(constraint1)
	pm.delete(constraint2)

	edgeVector =  vector1 + vector2
	halfEdgeVector = edgeVector * 0.5

	newPos = vector2 + halfEdgeVector
	object.setTranslation(halfEdgeVector, worldSpace=True)
	
	pm.delete(curve)