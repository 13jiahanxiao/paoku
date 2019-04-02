//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Simple script that lets you localize a UIWidget.
/// </summary>

[RequireComponent(typeof(UIWidget))]
[AddComponentMenu("NGUI/UI/Localize")]
public class UILocalize : MonoBehaviour
{
	/// <summary>
	/// Localization key.
	/// </summary>

	public string key;
	public bool useSysFont = false;
	
	
	public float EnglishSize = 0f;
	public float FrenchSize = 0f;
	public float GermanSize = 0f;
	public float ItalianSize = 0f;
	public float SpanishSize = 0f;
	public float DutchSize = 0f;
	public float RussianSize = 0f;
	public float Chinese_TraditionalSize = 0f;
	public float Chinese_SimplifiedSize = 0f;
	public float KoreanSize = 0f;
	public float PortugueseSize = 0f;
	
	
	
	private UISysFontLabel sflbl = null;
	
	bool isMoney = false;		// when true, this is a money field
	Decimal money = 0M;			// monetary value of this field, when isMoney is true
	
	//bool isEmpty = false;
	
	string mLanguage;
	bool mStarted = false;
	
	//public bool localizeNow;
	
	private Dictionary<string,string> cachedText = new Dictionary<string,string>();	// cached localized strings, for performance optimization
	
	/// <summary>
	/// This function is called by the Localization manager via a broadcast SendMessage.
	/// </summary>

	void OnLocalize(Localization loc) { if (mLanguage != loc.currentLanguage) Localize(); }

	/// <summary>
	/// Localize the widget on enable, but only if it has been started already.
	/// </summary>

	void OnEnable() { if (mStarted && Localization.instance != null) Localize(); }

	/// <summary>
	/// Localize the widget on start.
	/// </summary>

	void Start()
	{
		tag = "UILocalize";
		mStarted = true;
#if !UNITY_EDITOR
		if(useSysFont){
			//UILabel w = GetComponent<UILabel>();
			UIWidget w = GetComponent<UIWidget>();
			w.enabled = false;
			GameObject go = new GameObject();
			go.transform.parent = this.transform.parent;
			go.transform.localPosition = this.transform.localPosition - Vector3.forward * 4f;
			go.transform.localRotation = this.transform.localRotation;
			go.transform.localScale = this.transform.localScale;
			go.layer = gameObject.layer;
			go.name = gameObject.name + "_sysFont";
			sflbl = go.AddComponent<UISysFontLabel>();
			sflbl.color = w.color;
			sflbl.pivot = w.pivot;
			sflbl.FontSize = 50;
			
			UILocalize uiloc = sflbl.gameObject.AddComponent<UILocalize>();
			uiloc.key = key;
			if (Localization.instance != null) uiloc.Localize();
		}
#endif
		if (Localization.instance != null) Localize();
		
	}
	
//	void Update()
//	{
//		if (localizeNow)
//		{
//			localizeNow = false;
//			Localize();
//		}
//	}	
	
	public void SetKey(string keyName)
	{
		isMoney = false;
		key = keyName;
		Localize();
	}
	
	public void SetMoney(Decimal moneyVal)
	{
		isMoney = true;
		money = moneyVal;
		Localize();
	}	
	
	/// <summary>
	/// Force-localize the widget.
	/// </summary>

	public void Localize()
	{
		Localization loc = Localization.instance;
		UIWidget w = GetComponent<UIWidget>();
		UILabel lbl = w as UILabel;
		UISprite sp = w as UISprite;
		//UISysFontLabel sflbl = w as UISysFontLabel;
		
		// If no localization key has been specified, use the label's text as the key
		//if (string.IsNullOrEmpty(mLanguage) && string.IsNullOrEmpty(key) && lbl != null) key = lbl.text;

		// If we still don't have a key, use the widget's name
		//string val = string.IsNullOrEmpty(key) ? loc.Get(w.name) : loc.Get(key);
		
		string val = "";
		
		if (isMoney)
			val = string.Format(loc.GetCultureInfo(), "{0:C}", money);	// If this is a money field, format it correctly
		else if (cachedText.ContainsKey(key))							
			val = cachedText[key];										// if key is found in local cache, return its value
		else
		{
			//val = (key == loc.Get(key)) ? ("*" + key) : loc.Get(key);	// If key is not found, display key with star in front of it (for internal testing)
			val = (key == loc.Get(key)) ? key : loc.Get(key);			// If key is not found, the message must be already localized

			//if (val != ("*" + key))									// if the value is a real localized string,
				cachedText.Add(key, val);								// store the requested key/val for future use
		}
			
		if (lbl != null)
		{
			lbl.text = val;
		}
		else if (sp != null)
		{
			sp.spriteName = val;
			sp.MakePixelPerfect();
		}
		if (sflbl) 
		{
			sflbl.Text = val;
			sflbl.AppleFontName = loc.currentIosFont;
			sflbl.AndroidFontName = loc.currentAndroidFont;
		}
		
		mLanguage = loc.currentLanguage;
		
		// quick and dirty fix for localization
		if(EnglishSize > 0 && mLanguage == "English"){
			transform.localScale = new Vector3(EnglishSize, EnglishSize, 1f);
		}
		if(FrenchSize > 0 && mLanguage == "French"){
			transform.localScale = new Vector3(FrenchSize, FrenchSize, 1f);
		}
		if(GermanSize > 0 && mLanguage == "German"){
			transform.localScale = new Vector3(GermanSize, GermanSize, 1f);
		}
		if(ItalianSize > 0 && mLanguage == "Italian"){
			transform.localScale = new Vector3(ItalianSize, ItalianSize, 1f);
		}
		if(SpanishSize > 0 && mLanguage == "Spanish"){
			transform.localScale = new Vector3(SpanishSize, SpanishSize, 1f);
		}
		if(DutchSize > 0 && mLanguage == "Dutch"){
			transform.localScale = new Vector3(DutchSize, DutchSize, 1f);
		}
		if(RussianSize > 0 && mLanguage == "Russian"){
			transform.localScale = new Vector3(RussianSize, RussianSize, 1f);
		}
		if(Chinese_TraditionalSize > 0 && mLanguage == "Chinese_Traditional"){
			transform.localScale = new Vector3(Chinese_TraditionalSize, Chinese_TraditionalSize, 1f);
		}
		if(Chinese_SimplifiedSize > 0 && mLanguage == "Chinese_Simplified"){
			transform.localScale = new Vector3(Chinese_SimplifiedSize, Chinese_SimplifiedSize, 1f);
		}
		if(KoreanSize > 0 && mLanguage == "Korean"){
			transform.localScale = new Vector3(KoreanSize, KoreanSize, 1f);
		}
		if(PortugueseSize > 0 && mLanguage == "Portuguese"){
			transform.localScale = new Vector3(PortugueseSize, PortugueseSize, 1f);
		}
		
	}
}