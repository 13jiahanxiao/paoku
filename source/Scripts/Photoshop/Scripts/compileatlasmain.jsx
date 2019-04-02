function CompileAtlas(type)
{
	atlasDoc = app.activeDocument;
	var allLayers = atlasDoc.layerSets;
	
	var unitssetting = app.preferences.rulerUnits;
	app.preferences.rulerUnits = Units.PIXELS;		
	
	var tempArray = SetupFramework(allLayers);
	var lmLayerSet = tempArray[0]
	var aoLayerSet = tempArray[1]
	
	var docDir = atlasDoc.path.fsName.toString();
	//docDir = docPath.substring(0,docPath.lastIndexOf("/")+1);
	var lightmapDir = docDir + "\\LightMapBakeOutput\\";
	
	AssembleAtlas(lmLayerSet, aoLayerSet, lightmapDir, type);
	
	app.preferences.rulerUnits = unitssetting;	
}

function CompileSingleMap()
{
	app.activeDocument.selection.deselect()
	atlasDoc = app.activeDocument;
	var allLayers = atlasDoc.layerSets;
	
	var unitssetting = app.preferences.rulerUnits;
	app.preferences.rulerUnits = Units.PIXELS;		
	
	var tempArray = SetupFramework(allLayers);
	var lmLayerSet = tempArray[0]
	var aoLayerSet = tempArray[1]
	
	var docDir = atlasDoc.path.fsName.toString();
	//docDir = docPath.substring(0,docPath.lastIndexOf("/")+1);
	var lightmapDir = docDir + "\\LightMapBakeOutput\\";
	
	var filePath = File(openDialog()).fsName.toString()
	var nameExt = filePath.substring(filePath.lastIndexOf("\\")+1);
	var name = nameExt.substring(0, nameExt.length-4);	
	
	if(filePath.substring(filePath.length-4) != ".tga")
	{
		alert("ERROR: No file selected or file is not a tga file")
		return
	}
	
	var xmlPath = null
	if(name.substring(name.length-3) == "_ao")
	{
		xmlPath = filePath.substring(0, filePath.length-7) + '.xml';
		CreateMapLayer(aoLayerSet, filePath, xmlPath)
	}
	else
	{
		xmlPath = filePath.substring(0, filePath.length-4) + '.xml';
		CreateMapLayer(lmLayerSet, filePath, xmlPath)
	}
	
	app.preferences.rulerUnits = unitssetting;
}

function SetupFramework(currentLayers)
{
	var lmLayerSet = null;
	var aoLayerSet = null;

	for(var i=0; i<currentLayers.length; i++)
	{
		if(currentLayers[i].name == "Lightmap")
			lmLayerSet = currentLayers[i];
		if(currentLayers[i].name == "Ambient Occlusion")
			aoLayerSet = currentLayers[i];
	}
	
	if(lmLayerSet == null)
	{
		lmLayerSet = currentLayers.add();
		lmLayerSet.name = "Lightmap";
	}
	
	if(aoLayerSet == null)
	{
		aoLayerSet = currentLayers.add();
		aoLayerSet.name = "Ambient Occlusion";
	}

	tempArray = [lmLayerSet, aoLayerSet]
	
	return tempArray
}

function AssembleAtlas(lmLayerSet, aoLayerSet, sourceDir, type)
{
	var processFolder = Folder(sourceDir);
	// Use folder object get files function with mask 'a reg ex'
	var fileList = processFolder.getFiles(/\.(tga)$/i);
	// Loop through files
	for (var i = 0; i < fileList.length; i++) 
	{
		var filePath = fileList[i].fsName.toString();
		var nameExt = filePath.substring(filePath.lastIndexOf("\\")+1);
		var name = nameExt.substring(0, nameExt.length-4);
		
		if(name.substring(0,3) == "lm_")
		{
			if(name.substring(name.length-3) != "_ao")
			{
				if(type === "lm")
				{
					//alert("found lightmap")
					var xmlPath = filePath.substring(0, filePath.length-4) + '.xml';
					CreateMapLayer(lmLayerSet, filePath, xmlPath)
				}
			}
			else
			{
				if(type == "ao")
				{
					//alert("found ao")
					var xmlPath = filePath.substring(0, filePath.length-7) + '.xml';
					CreateMapLayer(aoLayerSet, filePath, xmlPath)
				}
			}
		}
	}
}

function CreateMapLayer(layerSet, filePath, xmlPath)
{
	var nameExt = filePath.substring(filePath.lastIndexOf("\\")+1);
	var name = nameExt.substring(0, nameExt.length-4);

	var resolution = atlasDoc.width;
	
	var lmArtLayers = layerSet.artLayers;
	var currentArtLayer = null;
	for (var j = 0; j < lmArtLayers.length; j++) 
	{
		if(lmArtLayers[j].name == name)
			currentArtLayer = lmArtLayers[j]
	}
	if(currentArtLayer != null)
	{
		atlasDoc.activeLayer = currentArtLayer;
		ReplaceContents(filePath)
	}
	else
	{
		if(!File(xmlPath).exists)
			alert(xmlPath + " does not exist.")
		else
		{
			dimensions = GetDimensionsFromFile(xmlPath);
			
			//alert(String(resolution))
			//alert(String(dimensions))			
			quadrantRes = resolution.value/dimensions
		
			lmArtLayers.add()
			CreateSmartObject(atlasDoc.activeLayer)
			ReplaceContents(filePath)
			
			var bounds = atlasDoc.activeLayer.bounds
			var layerRes = bounds[2].value - bounds[0].value
			
			//alert("resizing")
			//alert(String(quadrantRes))
			//alert(String(layerRes))
			atlasDoc.activeLayer.resize(Number(quadrantRes/layerRes*100),Number(quadrantRes/layerRes*100))
			//atlasDoc.activeLayer.translate(-(resolution.value/2) + (quadrantRes/2), -(resolution.value/2) + (quadrantRes/2))
			//alert(String(pos))

			var quadrant = GetQuadrantFromFile(xmlPath)
			
			var tx = quadrant[0] * quadrantRes
			var ty = quadrant[1] * quadrantRes
			atlasDoc.activeLayer.translate((-(resolution.value/2) + (quadrantRes/2)) + tx, ((resolution.value/2) - (quadrantRes/2)) - ty)
			//alert(String(bounds))
		}
	}
}
	
function GetDimensionsFromFile(xmlPath)
{
	var xmlFile = new File(xmlPath);
	xmlFile.open("r");
	var xml = new XML(xmlFile.read());
		
	var dimensions = xml.dimensions
	
	if(dimensions == "")
	{
		dimensions = 8
	}
	
	xmlFile.close()
	
	return dimensions
}	
	
function GetQuadrantFromFile(xmlPath)
{	
	var xmlFile = new File(xmlPath);
	xmlFile.open("r");
	var xml = new XML(xmlFile.read());
		
	var quadrant = [xml.quadx, xml.quady]
	
	xmlFile.close()
	
	return quadrant
}

function CreateSmartObject(layer)
{
   var doc = app.activeDocument;
   var layer = layer != undefined ? layer : doc.activeLayer;
   
   if(doc.activeLayer != layer) doc.activeLayer = layer;
   
   try
   {
      var idnewPlacedLayer = stringIDToTypeID( "newPlacedLayer" );
      executeAction( idnewPlacedLayer, undefined, DialogModes.NO );
      return doc.activeLayer;
   }
   catch(e)
   {
      return undefined;
   }
}

function ReplaceContents (newFile) {
// =======================================================
	var idplacedLayerReplaceContents = stringIDToTypeID( "placedLayerReplaceContents" );
    var desc3 = new ActionDescriptor();
    var idnull = charIDToTypeID( "null" );
    desc3.putPath( idnull, new File( newFile ) );
    var idPgNm = charIDToTypeID( "PgNm" );
    desc3.putInteger( idPgNm, 1 );
	executeAction( idplacedLayerReplaceContents, desc3, DialogModes.NO );
	return app.activeDocument.activeLayer
}
