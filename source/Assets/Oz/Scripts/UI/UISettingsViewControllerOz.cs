using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SettingsScreenName { General, About, Credits, CustomerSupport, TermsOfUse, PrivacyPolicy, }

public class UISettingsViewControllerOz : UIViewControllerOz
{
	private const string kCustomerSupportURL = "http://g.10086.cn/";
	private const string kPrivacyPolicyURL = "http://corporate.disney.go.com/corporate/pp.html";
	private const string kTermsOfUseIOSURL = "http://corporate.disney.go.com/corporate/terms-appapp.html";
	private const string kTermsOfUseAndroidURL = "http://corporate.disney.go.com/corporate/terms-appgen.html";
	
	public UISlider musicSlider;
	public UISlider soundSlider;
	public UISlider sensitivitySlider;
	//public UIAboutViewControllerOz aboutVC = null;
	public UICheckbox tutorialCheckboxOn;
	public UICheckbox tutorialCheckboxOff;
	public UICheckbox autoPostFBcheckboxOn;
	public UICheckbox autoPostFBcheckboxOff;
	public UICheckbox graphicsQualityCheckboxHigh;
	public UICheckbox graphicsQualityCheckboxMed;	
	public UICheckbox graphicsQualityCheckboxLow;	
	
	public UILabel versionLabel;
	
	public List<GameObject> graphicsQualityGOs = new List<GameObject>();
	
	private bool firstTimeOnTutorial = true; // is this 1st time we turn on checkbox, always happens 1st time we go to settings so we need to ignore first callback
	private bool firstTimeOnAutoFBpost = true;	
	private bool firstTimeOnGraphicsQuality = true;	
	
	public List<GameObject> graphicsGOs = new List<GameObject>();
	public List<GameObject> labelGOs = new List<GameObject>();	
	public List<GameObject> tabGOs = new List<GameObject>();	

	private TabSettings selectedTabSettings;
	private TabSettings deSelectedTabSettings;	
	
	private List<Vector3> tabGOSpriteScales = new List<Vector3>();		
	
	private SettingsScreenName pageToLoad = SettingsScreenName.General;
	
	private bool hideQaulitySelections = false;
	
	protected override void Awake()
	{
		base.Awake();
		
		if(versionLabel!=null)
			versionLabel.text = "v" + BundleInfo.GetBundleVersion();
		
//		foreach (GameObject go in graphicsGOs)
//			go.AddComponent<UIPanelAlpha>();
//		
//		foreach (GameObject go in labelGOs)
//			go.AddComponent<UIPanelAlpha>();		
		
		selectedTabSettings = new TabSettings(tabGOs[0]);
		deSelectedTabSettings = new TabSettings(tabGOs[1]);
		
		// store initial icon sprite scale values, to pass into scaling function
		tabGOSpriteScales.Add(tabGOs[0].transform.Find("Sprite").localScale);
		tabGOSpriteScales.Add(tabGOs[1].transform.Find("Sprite").localScale);
		tabGOSpriteScales.Add(tabGOs[2].transform.Find("Sprite").localScale);
		
		tabGOs[0].transform.Find("Sprite").localScale *= TabSettings.tabScaleMultiplier;	// hack to make icon of selected tab scaled up on launch
		
		hideQaulitySelections = false;
		if( Application.platform == RuntimePlatform.IPhonePlayer || GameController.IsDeviceLowEnd() || SystemInfo.deviceModel == "Amazon Kindle Fire" )
		{
			hideQaulitySelections = true;
		}
			
		if( hideQaulitySelections )
		{
			// turn off 'graphics quality' settings option if not Android
			foreach (GameObject go in graphicsQualityGOs)
			{
				//notify.Debug("Destroying: " + go);	
				Destroy(go);
			}
		}
	}
	
	public override void appear()
	{		
		base.appear();	
		UIManagerOz.SharedInstance.PaperVC.SetPageName("Ttl_Settings", "Ttl_Sub_General");
		UIManagerOz.SharedInstance.PaperVC.SetCurrentPage(UIManagerOz.SharedInstance.settingsVC);
		//ResetScrollListPanelAlphasToZero();
		SwitchToPanel(SettingsScreenName.General);	//pageToLoad);
		SetTabColliders();
		
		//		SharingManagerBinding.SetCurrentScreenName( "settings" );
	}

	private void SetTabColliders()	// only use this on initial page entry, afterwards the scaling takes care of collider state
	{
		foreach (GameObject tabGO in tabGOs)
			tabGO.GetComponent<BoxCollider>().enabled = true;
		
		tabGOs[(int)pageToLoad].GetComponent<BoxCollider>().enabled = false;
	}			
	
	private void Refresh()
	{
		musicSlider.sliderValue = AudioManager.SharedInstance.MusicVolume;
		soundSlider.sliderValue = AudioManager.SharedInstance.SoundVolume;
		
		if (TouchInput.Instance) 
			sensitivitySlider.sliderValue = TouchInput.Instance.Sensitivity;
		else 
			sensitivitySlider.sliderValue = PlayerPrefs.GetFloat("Sensitivity", 0.5f);
		
		int tut = PlayerPrefs.GetInt("ShowTutorial", Settings.GetInt("default-tutorial-status",0));	// default to 'true'
		SetCheckMark(tutorialCheckboxOn, tutorialCheckboxOff, tut);

		int autoFB = PlayerPrefs.GetInt("AutoPostToFacebook", 1);	// 0 or 1 = true, 2 = false		// default to 'true'
		SetCheckMark(autoPostFBcheckboxOn, autoPostFBcheckboxOff, autoFB);
		
		if( !hideQaulitySelections )
		{
//		int userquality = ;	// 2= high, 1 = med, 0 = low	// default to 'low'
			SetCheckMarkThree(graphicsQualityCheckboxHigh, graphicsQualityCheckboxMed, graphicsQualityCheckboxLow, GameController.userSelectedQuality);		
		}
		//paperViewController.ResetTabs((int)pageToLoad);	// reset highlighted tab to the one actually chosen
	}
	
	private void SetCheckMark(UICheckbox onBox, UICheckbox offBox, int statusValue)
	{	
		switch (statusValue)
		{
			case 0:
			case 1:
				onBox.isChecked = true;
				offBox.isChecked = false;
				break;
			case 2:
				onBox.isChecked = false;
				offBox.isChecked = true;		
				break;
		}
	}
	
	private void SetCheckMarkThree(UICheckbox highBox, UICheckbox medBox, UICheckbox lowBox, int statusValue)
	{	
		switch (statusValue)
		{
			case 2:
				highBox.isChecked = true;
				medBox.isChecked = false;		
				lowBox.isChecked = false;		
				break;
			case 1:
				highBox.isChecked = false;		
				medBox.isChecked = true;
				lowBox.isChecked = false;
				break;
			case 0:
				highBox.isChecked = false;		
				medBox.isChecked = false;
				lowBox.isChecked = true;		
				break;
		}	
	}	
	
	private void SwitchToPanel(SettingsScreenName panelScreenName)	// activate panel upon button selection, passing in SettingsScreenName		//ID (in range of int 0-3)
	{
		if (panelScreenName != pageToLoad)
		{
			ScaleTab((int)pageToLoad, false);		
			ScaleTab((int)panelScreenName, true);
		}
		
		pageToLoad = panelScreenName;		
		
		SetActivePage();
		Refresh();
	}	
	
	private void SetActivePage()
	{
	// set only appropriate panel active, make others inactive
		foreach (GameObject go in graphicsGOs)
			NGUITools.SetActive(go, false);	//FadeOut(1f, null, go);
		foreach (GameObject go in labelGOs)
			NGUITools.SetActive(go, false);	//FadeOut(1f, null, go);								
		
		NGUITools.SetActive(graphicsGOs[(int)pageToLoad], true);	//FadeIn(1f, null, graphicsGOs[(int)pageToLoad]);
		NGUITools.SetActive(labelGOs[(int)pageToLoad], true);		//FadeIn(1f, null, labelGOs[(int)pageToLoad]);
	}
	
	private void ScaleTab(int index, bool bigger)
	{
		if (index >= tabGOs.Count)	// prevent nulls
			return;
		
		float tweenTime = 0.1f;
		TabSettings targetTabSettings = (bigger) ? selectedTabSettings : deSelectedTabSettings;
		
		targetTabSettings.ScaleTab(tabGOs[index], targetTabSettings, tweenTime, bigger, tabGOSpriteScales[index]);
	}
	
	public void OnButtonClick(GameObject button)
	{	
		switch (button.name)
		{
			case "Tab0":									// general settings button
			//				AnalyticsInterface.LogNavigationActionEvent( "General", "Settings", "Settings-General" );
			
				SwitchToPanel(SettingsScreenName.General);	
				//ScaleTab((int)SettingsScreenName.General, true);
				//ScaleTab((int)SettingsScreenName.About, false);
				UIManagerOz.SharedInstance.PaperVC.SetPageName("Ttl_Settings", "Ttl_Sub_General");
				break;
			
			case "Tab1":									// about
			//AnalyticsInterface.LogNavigationActionEvent( "About", "Settings", "Settings-About" );
			
				SwitchToPanel(SettingsScreenName.About);
				//ScaleTab((int)SettingsScreenName.About, true);
				//ScaleTab((int)SettingsScreenName.General, false);
				UIManagerOz.SharedInstance.PaperVC.SetPageName("Ttl_Settings", "Lbl_About_Link_Support");
				break;
			
			case "Tab2":									// credits button
			//AnalyticsInterface.LogNavigationActionEvent( "Credits", "Settings", "Settings-Credits" );
			
				//pageToLoad = SettingsScreenName.Credits;		//SwitchToPanel(SettingsScreenName.Credits);
				SwitchToPanel(SettingsScreenName.Credits);
				UIManagerOz.SharedInstance.PaperVC.SetPageName("Ttl_Settings", "Lbl_About_Link_Privacy");
				break;
		}
		
//		SetActivePage();
//		Refresh();
	}
	void CancelRecoveryData(){
		UIConfirmDialogOz.onNegativeResponse -= CancelRecoveryData;
		UIConfirmDialogOz.onPositiveResponse -= ConfirmRecoveryData;		
	}
	
	void ConfirmRecoveryData(){
		//PurchaseUtil.recoveryData();
	}
	
	public void OnAboutPageButtonClick(GameObject button)
	{	
		switch ( button.name )
		{
			case "ButtonCustomer":									// customer support button
			//	AnalyticsInterface.LogPageViewEvent( "Settings", kCustomerSupportURL );
			//	Application.OpenURL( kCustomerSupportURL );
		UIConfirmDialogOz.onNegativeResponse += CancelRecoveryData;
		UIConfirmDialogOz.onPositiveResponse += ConfirmRecoveryData;
		//UIManagerOz.SharedInstance.confirmDialog.ShowConfirmDialog("Msg_LeaveGame","", "Btn_No", "Btn_Yes");
		UIManagerOz.SharedInstance.confirmDialog.ShowConfirmDialog("Msg_RecoveryData", "Btn_No", "Btn_Yes");
			
				break;
			
			case "ButtonTerms":										// terms of use button
				string termsOfUseURL = kTermsOfUseIOSURL;	
#if UNITY_ANDROID
				termsOfUseURL = kTermsOfUseAndroidURL;
#endif
			//				AnalyticsInterface.LogPageViewEvent( "Settings", termsOfUseURL );
				Application.OpenURL( termsOfUseURL );
			
				break;
	
			case "ButtonPrivacy":									// privacy policy button
			//				AnalyticsInterface.LogPageViewEvent( "Settings", kPrivacyPolicyURL );
				Application.OpenURL( kPrivacyPolicyURL );
				break;	
		}
	}	
	
	public void OnLanguageClick()
	{
		//Debug.Log ("OnLanguageClick");
		Localization.SharedInstance.CycleLanguages();
	}

	public void OnMusicSliderChange()
	{
		if (musicSlider)
		{
			//Debug.Log ("musicSlider " + musicSlider.sliderValue);
			AudioManager.SharedInstance.MusicVolume = musicSlider.sliderValue;
			AudioManager.SharedInstance.UpdateMusicVolume();
		}
	}
	
	public void OnSoundSliderChange()
	{
		if (soundSlider)
			AudioManager.SharedInstance.SoundVolume = soundSlider.sliderValue;
	}
	
	public void OnSensitivitySliderChange()
	{
		if (sensitivitySlider)
		{
			if(TouchInput.Instance) TouchInput.Instance.Sensitivity = sensitivitySlider.sliderValue;
			PlayerPrefs.SetFloat("Sensitivity", sensitivitySlider.sliderValue);
			//Debug.Log ("sensitivitySlider " + sensitivitySlider.sliderValue);
		}
	}
	
	public void OnTutorialCheckboxOn()
	{
		if (firstTimeOnTutorial)
		{
			firstTimeOnTutorial = false;
			return;
		}
		notify.Debug("OnTutorialCheckboxOn" + tutorialCheckboxOn.isChecked);
		if (tutorialCheckboxOn.isChecked)
		{
			PlayerPrefs.SetInt("ShowTutorial",1);
		}
		else
			PlayerPrefs.SetInt("ShowTutorial",2);
		PlayerPrefs.Save();
		
		AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_UI_Menu_click);
	}
	
	public void OnAutoPostToFacebookOn()
	{
		notify.Debug("OnAutoPostToFacebookOn" + tutorialCheckboxOn.isChecked);
		if (firstTimeOnAutoFBpost)
		{
			firstTimeOnAutoFBpost = false;
			return;
		}		
		if (autoPostFBcheckboxOn.isChecked)
			PlayerPrefs.SetInt("AutoPostToFacebook",1);
		else
			PlayerPrefs.SetInt("AutoPostToFacebook",2);
		PlayerPrefs.Save();
		
		AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_UI_Menu_click);
	}

	public void OnSetGraphicsQualityHi()
	{
		if (graphicsQualityCheckboxHigh.isChecked)
			OnSetGraphicsQuality(2);
	}
	
	public void OnSetGraphicsQualityLo()
	{
		if (graphicsQualityCheckboxLow.isChecked)
			OnSetGraphicsQuality(0);
	}
	
	public void OnSetGraphicsQualityMed()
	{
		if (graphicsQualityCheckboxMed.isChecked)
			OnSetGraphicsQuality(1);
	}
	
	private int newGraphicsQualityID = -1;
	
	public void OnSetGraphicsQuality(int newSettingID)
	{
		if (firstTimeOnGraphicsQuality)
		{
			firstTimeOnGraphicsQuality = false;
			newGraphicsQualityID = -1;
			return;
		}	
		
		AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_UI_Menu_click);
		
		newGraphicsQualityID = newSettingID;	// store ID of new requested setting
		
		UIConfirmDialogOz.onNegativeResponse += RevertGraphicsQuality;
		UIConfirmDialogOz.onPositiveResponse += SetGraphicsQuality;
		UIManagerOz.SharedInstance.confirmDialog.ShowConfirmDialog("Msg_Restart", "Btn_No", "Btn_Yes");	//Msg_LeaveGame
	}

	private void RevertGraphicsQuality()
	{
		ClearHandlers();
		SetCheckMarkThree(graphicsQualityCheckboxHigh, graphicsQualityCheckboxMed, graphicsQualityCheckboxLow, GameController.userSelectedQuality);			
	}

	private void SetGraphicsQuality()
	{
		ClearHandlers();
		GameController.SetUserDevicegenerationFromSaveID(newGraphicsQualityID);
		UIManagerOz.SharedInstance.SetUICameraClearFlagToSolidColorBG(true);	// switch UI camera background color on			
		NGUITools.SetActive(UIManagerOz.SharedInstance.confirmDialog.gameObject, false);
		UIManagerOz.SharedInstance.settingsVC.disappear();
		UIManagerOz.SharedInstance.PaperVC.disappear();
		UIManagerOz.SharedInstance.UICamera.backgroundColor = Color.black;
		Invoke("QuitApp", 0.1f);
	}
	
	private void ClearHandlers()
	{
		UIConfirmDialogOz.onNegativeResponse -= ClearHandlers;
		UIConfirmDialogOz.onPositiveResponse -= SetGraphicsQuality;		
	}
	
	private void QuitApp()
	{
		//PurchaseUtil.exitGame();
		//Application.Quit();
	}
}







/*	
	public void SetGraphicsQuality()
	{
		
//		notify.Debug("OnSetGraphicsQuality" + graphicsQualityCheckboxHigh.isChecked);
		{
		Debug.LogError ("11");
//			PlayerPrefs.SetInt("GraphicsQuality",0);
		}
		{
		Debug.LogError ("22");
			GameController.SetUserDevicegenerationFromSaveID(1);
//			PlayerPrefs.SetInt("GraphicsQuality",1);
		}
		{
		Debug.LogError ("33");
			GameController.SetUserDevicegenerationFromSaveID(0);
//			PlayerPrefs.SetInt("GraphicsQuality",2);
		}
//		PlayerPrefs.Save();
	}	
*/	
//}


//	void OnEnable()
//	{
//	//	notificationSystem.SetNotificationIconsToThisPage(UiScreenName.SETTINGS);
//		UIManagerOz.SharedInstance.PaperVC.SetPageName("Ttl_Settings", "Ttl_Sub_General");
//		UIManagerOz.SharedInstance.PaperVC.SetCurrentPage(UIManagerOz.SharedInstance.settingsVC);
//		//ResetScrollListPanelAlphasToZero();
//		SwitchToPanel(pageToLoad);
//		SetTabColliders();
//		//SetActivePage();
//		//Refresh();
//	}


//		foreach (GameObject go in graphicsGOs)
//		{
//			TweenAlpha ta = TweenAlpha.Begin(go, 0.1f, 0f);
//			
//			if (go == graphicsGOs[0])
//				ta.onFinished += FadeInNextPanelGraphics;	
//		}
//		
//		foreach (GameObject go in labelGOs)
//		{
//			TweenAlpha ta = TweenAlpha.Begin(go, 0.1f, 0f);
//			
//			if (go == labelGOs[0])
//				ta.onFinished += FadeInNextPanelLabels;	
//		}		
//	}
	
//	private void ResetScrollListPanelAlphasToZero()
//	{
//		foreach (GameObject go in graphicsGOs)
//			go.GetComponent<UIPanelAlpha>().alpha = 0f;
//		
//		foreach (GameObject go in labelGOs)
//			go.GetComponent<UIPanelAlpha>().alpha = 0f;		
//	}	
	
//	private void FadeInNextPanelGraphics(UITweener ta)
//	{
//		TweenAlpha.Begin(graphicsGOs[(int)pageToLoad], 0.2f, 1f);
//		ta.onFinished -= FadeInNextPanelGraphics;
//	}	
//	
//	private void FadeInNextPanelLabels(UITweener ta)
//	{
//		TweenAlpha.Begin(labelGOs[(int)pageToLoad], 0.2f, 1f);
//		ta.onFinished -= FadeInNextPanelLabels;
//	}


//			case "Tab2":									// terms of use button
//				SwitchToPanel(SettingsScreenName.TermsOfUse);	//2);
//				UIManagerOz.SharedInstance.PaperVC.SetPageName("Ttl_Settings", "Lbl_About_Link_Terms");
//				break;	
//			case "Tab3":									// privacy policy button
//				SwitchToPanel(SettingsScreenName.PrivacyPolicy);		//3);
//				UIManagerOz.SharedInstance.PaperVC.SetPageName("Ttl_Settings", "Lbl_About_Link_Privacy");
//				break;
		

//	public void ScaleTab(int index, bool bigger)
//	{
//		if (index >= tabGOs.Count)	// for 'credits' page, that's not a tab
//			return;
//		
//		float tweenTime = 0.3f;
//		
//		Vector3 dest1 = (bigger) ? new Vector3(0.0f, 30.0f, 0.0f) : new Vector3(0.0f, 13.74316f, 0.0f);
//		
//		iTween.MoveTo(tabGOs[index].transform.Find("Sprite").gameObject, iTween.Hash(
//			"position", dest1,
//			"islocal", true,
//			"time", tweenTime,
//			"easetype", iTween.EaseType.easeInOutSine));	
//		
//		iTween.MoveTo(tabGOs[index].transform.Find("Background").gameObject, iTween.Hash(
//			"position", dest1,
//			"islocal", true,
//			"time", tweenTime,
//			"easetype", iTween.EaseType.easeInOutSine));			
//		
//		Vector3 dest2 = (bigger) ? new Vector3(0.0f, 6.0f, 0.0f) : new Vector3(0.0f, -10.0f, 0.0f);
//		
//		iTween.MoveTo(tabGOs[index].transform.Find("Frame").gameObject, iTween.Hash(
//			"position", dest2,
//			"islocal", true,
//			"time", tweenTime,
//			"easetype", iTween.EaseType.easeInOutSine));	//easeOutBack));	
//		
//		Vector3 dest3 = (bigger) ? new Vector3(0.0f, -13.34267f, 0.0f) : new Vector3(0.0f, -29.34267f, 0.0f);		
//		
//		iTween.MoveTo(tabGOs[index].transform.Find("Checkmark").gameObject, iTween.Hash(
//			"position", dest3,
//			"islocal", true,
//			"time", tweenTime,
//			"easetype", iTween.EaseType.easeInOutSine));	
//	}

		//foreach (GameObject go in settingsGOs)
		//	NGUITools.SetActive(go, false);
		
		//NGUITools.SetActive(settingsGOs[panelNumber], true);
		

	
//	public void OnShowCredits()
//	{
//		SwitchToPanel(SettingsScreenName.Credits);
//		disappear();
//		appear();
//	}
	
//	public void OnShowSettings()
//	{
//		SwitchToPanel(SettingsScreenName.General);
//		disappear();
//		appear();
//	}	


		//	nextViewController = settingsVC;
		//}			
		

		//UIManagerOz.SharedInstance.PaperVC.OnButtonClick(button);
			
		//if (currentScreen == UiScreenName.SETTINGS)		// settings screen buttons	
		//{


	
//	public void OnShowAbout()
//	{
//		if (aboutVC != null) 
//		{
//			disappear();
//			aboutVC.previousViewController = this;
//			aboutVC.appear();
//		}
//	}
	
	/*
	public void OnTutorialCheckboxOn(bool ev){
		//Debug.Log ("OnTutorialCheckboxOn " + ev);
	}
	public void OnTutorialCheckboxOff(bool ev){
		//Debug.Log ("OnTutorialCheckboxOff " + ev);
	}
	*/

	
	/*
	void Start()
	{
		//Debug.Log("tutorialCheckboxOff.startsChecked " + tutorialCheckboxOff.startsChecked);
		if (tutorialCheckboxOff.startsChecked)
			tutorialCheckboxOff.isChecked = true;
	}
   */