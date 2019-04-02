function ReplaceAll()
{
	atlasDoc = app.activeDocument;
	allLayers = atlasDoc.layers;
	
	RecursiveReplace(allLayers)
}

function RecursiveReplace(currentLayers)
{
	for(var i=0; i<currentLayers.length; i++)
	{
		if(currentLayers[i].typename == 'ArtLayer')
		{
			if (currentLayers[i].kind == "LayerKind.SMARTOBJECT")
			{
				atlasDoc.activeLayer = currentLayers[i];
				ReplaceSmartObject(currentLayers[i]);
			}
		}
		else if(currentLayers[i].typename == 'LayerSet')
		{
			RecursiveReplace(currentLayers[i].layers)
		}
	}
}	
		
function ReplaceSmartObject(artLayer)
{
	soLayerName = artLayer.name;
	
	var userPath = GetUserPath()	
	
	var filePath = userPath + "/TempleRunOzNew/Assets/Oz/Textures/GameTextures/WimsyWoods/LightMapBakeOutput/" + soLayerName + ".tga";
	
	if(!File(filePath).exists)
		alert(filePath + " does not exist. Make sure the lightmap tga file matches the layer named " + soLayerName + ". Skipping replacement.");
	else
		ReplaceContents(filePath);
}

function GetUserPath()
{
	var userPath = "";
	if ($.os.search(/windows/i) != -1)
	{
		userPath = $.getenv("USERPROFILE").replace(/\\/g, "/");
	}	
	
	return userPath
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

ReplaceAll()