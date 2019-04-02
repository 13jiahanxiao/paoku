using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class NotificationIcons : MonoBehaviour 
{
	// these lists must all be the same size, as set up in the Inspector!
	public List<UISprite> iconBackgrounds = new List<UISprite>();
	public List<UILabel> iconLabels = new List<UILabel>();
	public List<UISprite> iconExclamations = new List<UISprite>();
	
	private int curIconValue = -1;
	public int IconValue
	{
		get { return curIconValue; }
	}
	
	/*
	Material bgMat, labelMat;
	void Start(){
		if(iconBackgrounds.Count > 0){
			bgMat = new Material(iconBackgrounds[0].material);
			bgMat.renderQueue += 11350;
			bgMat.mainTexture = iconBackgrounds[0].material.mainTexture;
		}
		if(iconLabels.Count > 0){
			labelMat = new Material(iconLabels[0].material);
			labelMat.renderQueue += 11480;
			labelMat.mainTexture = iconLabels[0].material.mainTexture;
		}
		
		foreach(UISprite bg in iconBackgrounds){
			bg.material = bgMat;
			Debug.Log (bg.name + " " + bg.material.renderQueue);
		}
		foreach(UILabel label in iconLabels){
			label.material = labelMat;
			Debug.Log (label.name + " " + label.material.renderQueue);
		}
		foreach(UISprite exc in iconExclamations){
			exc.material = labelMat;
			Debug.Log (exc.name + " " + exc.material.renderQueue);
		}
	}
		*/
	
	
	public void SetNotification(int buttonID, int iconValue)
	{
		curIconValue = iconValue;
		
		if (iconValue < 0)			// if value < 0, turn off notification
		{
			iconBackgrounds[buttonID].enabled = false;
			iconLabels[buttonID].enabled = false;
			iconExclamations[buttonID].enabled = false;
		}
		else if (iconValue == 0 || iconValue > 9)	// if value == 0 or value > 9, show exclamation point 
		{
			iconBackgrounds[buttonID].enabled = true;
			iconLabels[buttonID].enabled = false;	//true;
			//iconLabels[buttonID].text = "!";
			iconExclamations[buttonID].enabled = true;
		}
		else if (iconValue > 0)		// if value > 0, show actual number (capped at 9)
		{
			iconBackgrounds[buttonID].enabled = true;
			iconLabels[buttonID].enabled = true;
			iconLabels[buttonID].text = (Math.Min(iconValue, 9)).ToString();
			iconExclamations[buttonID].enabled = false;
		}
	}
}



	
//	void Start()
//	{
//		for (int i=0; i<iconBackgrounds.Count; i++)		// turn off all notification icons at launch
//			SetNotification(i,-1);
//	}

	//public List<GameObject> notificationIcons = new List<GameObject>();
//		iconSprites.Add(icon.transform.Find("Background").GetComponent<UISprite>());
//		iconLabels.Add(icon.transform.Find("Label").GetComponent<UILabel>());
//		iconExclamations.Add(icon.transform.Find("SpriteExclamation").GetComponent<UISprite>());

//public class IconParts
//{
//	GameObject iconGO;
//	UISprite sprite;
//	UILabel label;
//	UISprite exclamation;
//}


//	public void RegisterNotificationIcon(string name, UISprite sprite, UILabel label, UISprite exclamation)
//	{
//		//SetNotification(name,-1);
//	}

	
//	public List<GameObject> notificationIcons = new List<GameObject>();
//
//	private Dictionary<string,UISprite> iconSprites = new List<UISprite>();
//	private Dictionary<string,UILabel> iconLabels = new List<UILabel>();
//	private Dictionary<string,UISprite> iconExclamations = new List<UISprite>();
