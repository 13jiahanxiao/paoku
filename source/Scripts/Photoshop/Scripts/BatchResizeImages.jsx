gSourceDir = "C:/work/testbatchresize";
gSizeMultiplier = 1.0;

function BatchResizeImages()
{
	alert("Resizing PNGs in directory " +  gSourceDir + " by " + gSizeMultiplier);
	var processFolder = Folder(gSourceDir);
	// Use folder object get files function with mask 'a reg ex'
	var fileList = processFolder.getFiles(/\.(png)$/i);
	// Loop through files
	for (var i = 0; i < fileList.length; i++) 
	{
		app.open(fileList[i]);
		
		activeDoc = app.activeDocument;
		
		currentWidth = activeDoc.width;
		var newWidth = currentWidth * gSizeMultiplier;

		currentHeight = activeDoc.height;
		var newHeight = currentHeight * gSizeMultiplier;		
		
		activeDoc.resizeImage(newWidth, newHeight, null, ResampleMethod.BICUBIC);
		
		activeDoc.save();
		activeDoc.close();
	}
}
BatchResizeImages();