using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ScaleModalDialog : MonoBehaviour
{
	public List<GameObject> buttons = new List<GameObject>();
	public List<GameObject> topCorners = new List<GameObject>();
	public List<GameObject> bottomCorners = new List<GameObject>();
	
	public UISprite background;
	public UILabel description;
	public UISysFontLabel descriptionUsingSysFont;
	
	private float textSize = 0f;		// equals number of lines of text
	
	public void SetScale(float scale = -1f)
	{
		if (descriptionUsingSysFont != null)
			textSize = Math.Max(description.relativeSize.y, descriptionUsingSysFont.relativeSize.y);
		else 
			textSize = description.relativeSize.y;
		
		if (scale != -1f)	// manual override for textSize parameter, corresponds to number of lines
		{
			textSize = scale;
		}
		
		background.transform.localScale = new Vector3(550.0f, ((textSize - 1.0f) * 50.0f) + 200.0f, 1.0f);
		
		description.transform.localPosition = new Vector3(description.transform.localPosition.x, 
			((textSize - 1.0f) * 25.0f) + 50.0f, description.transform.localPosition.z);
		
		foreach (GameObject corner in topCorners)
			corner.transform.localPosition = new Vector3(corner.transform.localPosition.x,
				((textSize - 1.0f) * 25.0f) + 100.0f, corner.transform.localPosition.z);

		foreach (GameObject corner in bottomCorners)
			corner.transform.localPosition = new Vector3(corner.transform.localPosition.x,
				((textSize - 1.0f) * -25.0f) - 100.0f, corner.transform.localPosition.z);
		
		foreach (GameObject button in buttons)
			button.transform.localPosition = new Vector3(button.transform.localPosition.x,
				((textSize - 1.0f) * -25.0f) - 35.0f, button.transform.localPosition.z);		
	}
}







//	public float GetTextSize()
//	{
//		return sysFontTextSize;	//textSize;
//	}
//	
//	public float GetTextTransform()
//	{
//		return sysFontTextTransform;
//	}	
