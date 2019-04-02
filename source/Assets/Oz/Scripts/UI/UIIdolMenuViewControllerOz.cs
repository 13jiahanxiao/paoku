using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIIdolMenuViewControllerOz : UIViewControllerOz
{
	public GameObject NewsFeed;
	
	public List<GameObject> topIdolGameObjects = new List<GameObject>();
	public List<GameObject> bottomIdolGameObjects = new List<GameObject>();
	public List<GameObject> backgroundGameObjects = new List<GameObject>();
	public GameObject play3d;
	private UILabel play3dlabel;
	public UISprite playEnglish;
	private bool mLogoIsVisible;
	private bool mMenuIsVisible;
	
	public GameObject defaultLogo;
	public GameObject localizedLogo;
	
	public UIPanelAlpha[] panelAlphas = new UIPanelAlpha[2];
	
	public static bool isQuitting = false;
	
	public float mQuitTimer = 0.0f;
	
	//private NotificationSystem notificationSystem;
	private NotificationIcons notificationIcons;
	
	bool ItemClicked = false;
	
	public bool DidLaunchViaPushNotification { get; set; } // For push notifications
	public string MenuButtonToPressAfterGameIntro { get; set; } // For push notifications and Burstly Notification Ads
	
	
	//Response can break into while running.  Bad.
	//On Bring in Bottom complete.

	public delegate void OnMainMenuAnimationCompleteHandler();
	protected static event OnMainMenuAnimationCompleteHandler onMainMenuAnimationCompleteEvent = null;
	public static void RegisterForOnIdolBottomPanelComplete( OnMainMenuAnimationCompleteHandler delg) {
		onMainMenuAnimationCompleteEvent += delg;
	}
	public static void UnregisterForOnMainMenuAnimationCompleteHandler( OnMainMenuAnimationCompleteHandler delg) {
		onMainMenuAnimationCompleteEvent -= delg;
	}
	
	protected override void Awake() 
	{ 
		base.Awake();
		notificationIcons = gameObject.GetComponent<NotificationIcons>();
		defaultLogo.GetComponent<UISprite>().enabled = false;
		localizedLogo.renderer.enabled = true;
		
	}		
	
	protected override void Start() 
	{ 
		base.Start();
		play3dlabel = play3d.GetComponent<UILabel>();
		//notificationSystem = Services.Get<NotificationSystem>();
	}

#if UNITY_ANDROID
	void Update()
	{
		if( mQuitTimer > 0.0f)
		{
			mQuitTimer -= Time.deltaTime;
			if( mQuitTimer <= 0.0f )
			//PurchaseUtil.exitGame();
				Application.Quit();
		}
	}
#endif
	
	public override void appear()
	{
		UIManagerOz.SharedInstance.MainGameCamera.enabled = true;
		UIManagerOz.SharedInstance.SetUICameraClearFlagToSolidColorBG(false);		
		UIManagerOz.SharedInstance.PaperVC.SetCurrentPage(UIManagerOz.SharedInstance.idolMenuVC);
		base.appear();
		
		ItemClicked = false;
		
		Services.Get<NotificationSystem>().SetNotificationIconsForThisPage(UiScreenName.IDOL);
		
		if(AudioManager.SharedInstance.IsMenuMusicPlaying())
			AudioManager.SharedInstance.FadeMusicMultiplier(0.2f,1f);
		
	//	if(Localization.SharedInstance.GetCurrentLanguageCode() == "en" ){
			play3d.SetActiveRecursively(false);
			play3d.GetComponent<UILabel>().enabled = false;
			playEnglish.gameObject.SetActiveRecursively(true);
			playEnglish.enabled = true;
//		}
//		else{
			//play3d.SetActiveRecursively(true);
	//		play3d.GetComponent<UILabel>().enabled = true;
	//		playEnglish.gameObject.SetActiveRecursively(false);
	//		playEnglish.enabled = false;
	//	}
		
		//		SharingManagerBinding.SetCurrentScreenName( "idol_screen" );
		
		if ( mMenuIsVisible == true )
		{
			// User is returning to the Idol screen from the main menu
			// Re-show the Burstly badge
			//SharingManagerBinding.ShowBurstlyBannerAd( "idol_badge", true );
		}
	}
	
	public void ChangeLogo(Texture logo)
	{
		if( logo && defaultLogo && localizedLogo)
		{
			defaultLogo.GetComponent<UISprite>().enabled = false;
			localizedLogo.renderer.enabled = true;
			localizedLogo.renderer.material.mainTexture = logo;
		}
//		defaultLogo.GetComponent<UISprite>()
	}
	
	public void ReadyForIntroAnimation()
	{	
		
//		gameObject.AddComponent<FadeAlphaResetToZero>();		
		
		// move idol menu offscreen, ready for animating in
		foreach (GameObject go in topIdolGameObjects)
			go.transform.localPosition = new Vector3(go.transform.localPosition.x, go.transform.localPosition.y + 1000.0f, go.transform.localPosition.z);		
		
		// move bottom panel objects offscreen, ready for animating in
		foreach (GameObject go in bottomIdolGameObjects)
			go.transform.localPosition = new Vector3(go.transform.localPosition.x, go.transform.localPosition.y - 200.0f, go.transform.localPosition.z);	
		
		//play3d.transform.localPosition = new Vector3( play3d.transform.localPosition.x, 1940f, play3d.transform.localPosition.z);		
		
		NGUITools.SetActive(gameObject, true);
		play3d.GetComponent<UILabel>().alpha = 0f;
	}
	
	public void BringInIdolMenu()
	{
		foreach (GameObject go in topIdolGameObjects)		// slide in logo
			iTween.MoveTo(go, iTween.Hash(
				"isLocal", true, 
				"position", new Vector3(go.transform.localPosition.x, go.transform.localPosition.y - 1000.0f,  go.transform.localPosition.z),
				"time", 1.0f, 
				"easetype", iTween.EaseType.easeOutSine,
				"oncomplete", "OnLogoSlideInComplete",
				"oncompletetarget", gameObject
				));

		float ratio = (float)Screen.width / (float)Screen.height;
		float ratioMin = 640f / 1136f; 
		float ratioMax = 768f / 1024f; 
		float current = (ratio - ratioMin) / (ratioMax - ratioMin);
		current = Mathf.Clamp01(current);
		
		
		
		play3dlabel.alpha = 0f;
		playEnglish.alpha = 0f;
		
		UIWidget play;
		
	//	if(Localization.SharedInstance.GetCurrentLanguageCode() == "en" ){
			play3d.SetActiveRecursively(false);
			play3d.GetComponent<UILabel>().enabled = false;
			playEnglish.gameObject.SetActiveRecursively(true);
			play = playEnglish;
	//	}
	//	else{
	//		play3d.SetActiveRecursively(true);
	//		playEnglish.gameObject.SetActiveRecursively(false);
	//		playEnglish.enabled = false;
	//		play = play3d.GetComponent<UILabel>();
	//	}
		
		TweenAlpha ta = TweenAlpha.Begin(play.gameObject,1f, 1f);
		ta.delay = 1f;

		
		ParticleSystem[] playParticles = play.transform.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0, imax = playParticles.Length; i < imax; ++i)
		{
			if(playParticles[i])
				playParticles[i].Play();
		}
		
		Invoke("StartMainMenuMusicOnStart",1.0f);
			
		Invoke("BringInBottomPanel", 2.0f);
		
		iTween.ValueTo(GameController.SharedInstance.gameObject, iTween.Hash(
			"time", 1f,
			"from", 4f,
			"to", 2f,
			"onupdate", "OnUpdateFade",
			"onupdatetarget", GameController.SharedInstance.gameObject
			));
		//paperViewController.DoIntroAnimation();
		
		if (onMainMenuAnimationCompleteEvent != null)
		{
			onMainMenuAnimationCompleteEvent();
			
		
			
		}
	}
	
	public void OnLogoSlideInComplete()
	{
		if ( mLogoIsVisible == false )
		{
			mLogoIsVisible = true;
		
			
		}
	}
	
	private void _showBurstlyNotificationAd()
	{
		if ( mLogoIsVisible == true && mMenuIsVisible == true )
		{
			/*
			// Show the Burstly badge
			if ( SharingManagerBinding.BustlyBannerAdIsCached( "idol_badge" ) == true )
			{
				// The ad is already loaded, so show it
				SharingManagerBinding.ShowBurstlyBannerAd( "idol_badge", true );
			}
			else
			{
				// Load the ad (and then ask SharingManagerBinding if it's OK to show it)
				SharingManagerBinding.LoadBurstlyBannerAd( "idol_badge" );
			}
			
			
			if ( DidLaunchViaPushNotification == false )
			{
				// If we're not in the middle of an Urban Airship deep-linking
				// then show the Burstly pre-game interstitial
				SharingManagerBinding.LoadBurstlyInterstitial( "pre_game" );
			}*/
		}
	}
	
	public void StartMainMenuMusicOnStart()
	{
		AudioManager.SharedInstance.SwitchToMainMenuMusic(0f);
		
	}

	public void BringInBottomPanel()
	{
		//PurchaseUtil.showBulletin();
		// wxj, get whether first open by java
		//int isFirstOpen = PlayerPrefs.GetInt("IS_FIRST_OPEN_REWARD",1);
		//UnityEngine.Debug.Log("isFirstOpen:"+isFirstOpen);
		
		//if(PurchaseUtil.isFirstOpen()){
		UIManagerOz.SharedInstance.okayDialog.ShowOkayDialog("Msg_Reward", "Btn_Ok");
		PlayerPrefs.SetInt("IS_FIRST_OPEN_REWARD",0);
		GameProfile.SharedInstance.Player.specialCurrencyCount += 3;	
		UIManagerOz.SharedInstance.PaperVC.UpdateCurrency();
		GameProfile.SharedInstance.Serialize();
		ObjectivesDataUpdater.ChangeGenericStat(ObjectiveType.CollectSpecialCurrency,3,false);
			
		//}
		
		foreach (GameObject go in bottomIdolGameObjects)	
			iTween.MoveTo(go, iTween.Hash(
				"isLocal", true,
				"position", new Vector3(go.transform.localPosition.x, go.transform.localPosition.y + 200.0f, go.transform.localPosition.z),
				"time", 0.5f,
				"easetype", iTween.EaseType.easeOutSine,
				"oncomplete", "OnMenuSlideInComplete",
				"oncompletetarget", gameObject
				));
		
		int isNeedReward = PlayerPrefs.GetInt("IS_REWARD",0);
		if(isNeedReward == 1)
		{
			string text = Localization.SharedInstance.Get ("Toast_Reward0");
			int coin = PlayerPrefs.GetInt("coin",100);
			int gem = PlayerPrefs.GetInt("gem",1);
			if(coin!=0 && gem!=0)
			{
				text=string.Format(Localization.SharedInstance.Get ("Toast_Reward3"), coin,gem);
				
			}else if(coin==0 && gem != 0)
			{
				
				text=string.Format(Localization.SharedInstance.Get ("Toast_Reward2"),gem);
				
			}else if(coin != 0 && gem == 0)
			{
				text=string.Format(Localization.SharedInstance.Get ("Toast_Reward1"),coin);
				
			}
			
			UIManagerOz.SharedInstance.okayDialog.ShowOkayDialog(text, "Btn_Ok");
			GameProfile.SharedInstance.Player.coinCount += coin;
			GameProfile.SharedInstance.Player.specialCurrencyCount += gem;	
			UIManagerOz.SharedInstance.PaperVC.UpdateCurrency();
			GameProfile.SharedInstance.Serialize();
		//	ObjectivesDataUpdater.ChangeGenericStat(ObjectiveType.CollectCoins,coin,false);
		//	ObjectivesDataUpdater.ChangeGenericStat(ObjectiveType.CollectSpecialCurrency,gem,false);
			//PurchaseUtil.notifyToMarkRewarded();
			
		}

		NewsFeed.GetComponent<HorizontalScrollingLabel>().StartScrollIfNeeded();	// reset news ticket just prior to showing it	
	}
	
	public void OnMenuSlideInComplete()
	{
		if ( mMenuIsVisible == false )
		{
			mMenuIsVisible = true;
					
		}
	
		
		
	}

	public void OnMenuClicked()
	{	
		//FadeOut(1.0f, UIManagerOz.SharedInstance.PaperVC.FadeIn);	// make main menu come in as soon as idol menu is done fading out
		//FadeIn(1.0f);
		
		if(ItemClicked) return;
		
		ItemClicked = true;

		//		AnalyticsInterface.LogNavigationActionEvent( "Menu", "Title Screen", "Main Menu" );
		
		// clear background with color similar to menu UI background, eliminate flashing between screens		
		UIManagerOz.SharedInstance.SetUICameraClearFlagToSolidColorBG(true);		
		
		UIManagerOz.SharedInstance.mainVC.MenuButtonToPress = MenuButtonToPressAfterGameIntro;
		MenuButtonToPressAfterGameIntro = null; // Clear this for next time
		UIManagerOz.SharedInstance.mainVC.appear();
		UIManagerOz.SharedInstance.PaperVC.appear();		
		disappear();
		
		// Hide the Burstly badge
		//		SharingManagerBinding.ShowBurstlyBannerAd( "idol_badge", false );
	}

	private void OnPlayClicked()
	{
//		if( DownloadManager.IsDownloadInProgress() )
//		{// don't allow playing if a download is in progress
//			// just bring up downloadng UI
//			UIManagerOz.SharedInstance.StartDownloadPrompts( true, false, true, gameObject);
//			return;
//		}
		
		if(ItemClicked) return;
		
		ItemClicked = true;
		
		if (GameController.SharedInstance.gameState != GameState.IN_RUN)	// only trigger this once per run
		{
			//			AnalyticsInterface.LogNavigationActionEvent( "Play", "Title Screen", "Game" );
			
			UIManagerOz.SharedInstance.OnPlayClicked();
			CancelInvoke("StartMainMenuMusicOnStart");
			
			StopPlayButtonParticles();
			FadeOutIdolMenu();
					
			// Hide the Burstly badge
			//SharingManagerBinding.ShowBurstlyBannerAd( "idol_badge", false );
		}
	}	
	
	private void StopPlayButtonParticles()
	{
		UIWidget play;
		
	//	if (Localization.SharedInstance.GetCurrentLanguageCode() == "en")
			play = playEnglish;
	//	else
	//		play = play3d.GetComponent<UILabel>();
		
		ParticleSystem[] playParticles = play.transform.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0, imax = playParticles.Length; i < imax; ++i)
		{
			if (playParticles[i])
			{
				//playParticles[i].Stop();
				playParticles[i].Clear();
			}
		}
	}
	
	float fadeOutTime = 1.5f;
	
	private void FadeOutIdolMenu()
	{
		iTween.ValueTo(gameObject, iTween.Hash(
				"from", 1f,
				"to", 0f,
				"time", fadeOutTime,
				"easetype", iTween.EaseType.easeOutCubic,	//easeInOutSine
				"onupdate", "FadeIdolMenuAlpha",
				"onupdatetarget", gameObject,
				"oncomplete", "HideIdolMenu",
				"oncompletetarget", gameObject,
				"ignoretimescale", true));
		
//		iTween.ValueTo(gameObject, iTween.Hash(
//				"from", 1f,
//				"to", 0f,
//				"time", fadeOutTime,
//				"easetype", iTween.EaseType.easeOutCubic,	//easeInOutSine
//				"onupdate", "FadeIdolMenuLocalizedLogo",
//				"onupdatetarget", gameObject,
//				"ignoretimescale", true));	
	}
	
	private void FadeIdolMenuAlpha(float val)
	{
		foreach (UIPanelAlpha pa in panelAlphas)
			pa.alpha = val;
		
		play3dlabel.alpha = val;
		localizedLogo.renderer.material.SetColor("_Color", new Color(1f,1f,1f,val));
	}
	
//	private void FadeIdolMenuLocalizedLogo(float val)
//	{
//		localizedLogo.renderer.material.SetColor("_Color", new Color(1f, 1f, 1f, val));
//	}		
	
	private void HideIdolMenu()
	{
		disappear();
		
		foreach (UIPanelAlpha pa in panelAlphas)
			pa.alpha = 1f;		// reset panel alphas	
		
		localizedLogo.renderer.material.SetColor("_Color", Color.white);
		play3dlabel.alpha = 1f;
		
	}
	
	public void SetNotificationIcon(int buttonID, int iconValue)		// update actual icon onscreen
	{
		notificationIcons.SetNotification(buttonID, iconValue);
	}
	
	public void OnEscapeButtonClicked()
	{
		if( UIManagerOz.escapeHandled ) return;
		UIManagerOz.escapeHandled = true;
		
		UIConfirmDialogOz.onNegativeResponse += CancelExit;
		UIConfirmDialogOz.onPositiveResponse += ExitGame;
		//UIManagerOz.SharedInstance.confirmDialog.ShowConfirmDialog("Msg_LeaveGame","", "Btn_No", "Btn_Yes");
		//		PurchaseUtil.exitGame();
		UIManagerOz.SharedInstance.confirmDialog.ShowConfirmDialog("Msg_LeaveGame", "Btn_No", "Btn_Yes");
	}
	
	public void CancelExit()
	{
		UIConfirmDialogOz.onNegativeResponse -= CancelExit;
		UIConfirmDialogOz.onPositiveResponse -= ExitGame;	
	}
	
	public void ExitGame()	
	{
		mQuitTimer = 0.2f;// wait o prevent screen glitch
		isQuitting = true;
	}	
	
}




		//Invoke("GoToMainMenu", 0.01f); // hack to allow main menu to format itself appropriately for iPad device, apparently
		//GoToMainMenu();
//	}
//	
//	private void GoToMainMenu()
//	{
//		UIManagerOz.SharedInstance.mainVC.appear();
//		UIManagerOz.SharedInstance.PaperVC.appear();		
//		disappear();	
//	}
	


	//ShowObject(gameObject, true, true);


	//if (menuClickCount > 0)	// if not first time clicked, need to remind main menu to reset itself
		//	UIManagerOz.SharedInstance.PaperVC.GoToMainMenu();
		
		//menuClickCount++;
	
//	public GameObject logoPanel;
//	public GameObject BottomPanel;		
//	public GameObject BackgroundPanel;		
	
	
//	public GameObject topBackgroundSprite;
//	public GameObject fullBackgroundSprite;
	
//	private bool logoSlideInCompleted = false;
	
//	private int menuClickCount = 0;
	

		
//        foreach (GameObject go in backgroundGameObjects)	// set alpha of background gray-out sprites to zero, ready for fade-in	
//        {
//			UIWidget w = go.GetComponent<UIWidget>();
//			
//            Color c = w.color;
//            c.a = 0.0f;
//            w.color = c;
//        }		

			
//		if (!logoSlideInCompleted)		// slide in logo, only the first time
//		{		
		
			//logoSlideInCompleted = true;
			



//			fullBackgroundSprite.GetComponent<TweenAlpha>().enabled = true;
//			topBackgroundSprite.GetComponent<TweenAlpha>().enabled = true;
			


		//else
		//	FadeInIdolMenu();
		//FadeOutIdolMenu();

//	private void GoToMainMenu()
//	{
//		if (menuClickCount > 0)	// if not first time clicked, need to remind main menu to reset itself
//			UIManagerOz.SharedInstance.PaperVC.GoToMainMenu();
//		
//		menuClickCount++;
//		mainVC.appear();
//		disappear();
//	}

//	public void FadeOutIdolMenu()
//	{
//		FadePanel fp1 = BackgroundPanel.AddComponent<FadePanel>();
//		FadePanel fp2 = logoPanel.AddComponent<FadePanel>();
//		FadePanel fp3 = BottomPanel.AddComponent<FadePanel>();	
//		
//		fp1.SetParameters(false, 1.0f);	
//		fp2.SetParameters(false, 1.0f);	
//		fp3.SetParameters(false, 1.0f);	
//		
//		fp3.AddCallback(GoToMainMenu);	
//	}
//	
//	public void FadeInIdolMenu()
//	{
//		FadePanel fp1 = BackgroundPanel.AddComponent<FadePanel>();
//		FadePanel fp2 = logoPanel.AddComponent<FadePanel>();
//		FadePanel fp3 = BottomPanel.AddComponent<FadePanel>();	
//		
//		fp1.SetParameters(true, 1.0f);	
//		fp2.SetParameters(true, 1.0f);	
//		fp3.SetParameters(true, 1.0f);			
//	}	





//		
//		if (!logoSlideInCompleted)	// slide in logo, only the first time
//		{
//			iTween.MoveFrom(logoPanel, iTween.Hash("isLocal", true, "position", new Vector3(0.0f, 600.0f, 0.0f),
//				"time", 1.0f, "easetype", iTween.EaseType.easeOutSine));
//			logoSlideInCompleted = true;
//			
//			//backgroundSprite.GetComponent<TweenAlpha>().enabled = true;
//		}



		//OzGameCamera.SharedInstance.reset();


//	public void OnMainMenuClicked() 
//	{
//		disappear();
//		
//		//-- Show the main menu and leave a bread crumb to this VC.
//		if(mainMenuVC != null) 
//		{
//			mainMenuVC.previousViewController = this;
//			mainMenuVC.appear();
//		}
//	}
//}


	
//	public UIInGameViewControllerOz inGameVC = null;
//	public static event voidClickedHandler onPlayClickedHandler = null;
//	
//	public void OnPlayClicked() 
//	{
//		if (MainGameCamera != null) { MainGameCamera.enabled = true; }
//		disappear();
//		if (inGameVC != null) { inGameVC.appear(); } //ShowObject(activePowerIcon.gameObject, false, false);
//		if (onPlayClickedHandler != null) { onPlayClickedHandler(); }	//-- Notify an object that is listening for this event.
//	}	

	//Some test fade-in code!
	/*public override void appear()
	{
		base.appear();
		
		StartCoroutine(FadeIn());
	}
	
	private IEnumerator FadeIn()
	{
		UIWidget[] ws = GetComponentsInChildren<UIWidget>();
		
		float alpha = 0f;
		
		while(alpha<1f)
		{
			alpha = Mathf.MoveTowards(alpha,1f,Time.deltaTime);
			
			foreach(UIWidget w in ws)
			{
				w.alpha = alpha;
			}
			yield return null;
		}
	}*/
