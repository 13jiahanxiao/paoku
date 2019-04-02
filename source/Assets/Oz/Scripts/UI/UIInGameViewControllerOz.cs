using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum tutorialMessageType
{
	SUCESS,
	FAIL,
	MSG
}	

public class UIInGameViewControllerOz : UIViewControllerOz
{
	public UILabel debugDistanceLabel;
	//public UISprite countDownSprite;
	public UILabel countDownLabel;
	public GameObject pauseButton;
	
 
	//private PopupNotification popupNotification;
	public HeadStart headStart, megaHeadStart;	
	public CoinMeter coinMeter;
	//public Transform activePowerupButton;
	public ScoreUI scoreUI;
	public PauseMenu pauseMenu;
	public ResurrectMenu resurrectMenu;	
	public BonusButtons bonusButtons;
	
	public UIPanelAlpha	envChangeHud = null;
	public GameObject	envChangeHudArrow = null;
	public UILabel		envChangeHudLabel = null;
	
	public UISprite     wandVignette = null;
	
	//-- Tutorial
	public GameObject	tutorialArrowLabelPanel = null;
	public Transform 	tutorialArrow = null;
	public UILabel	 	tutorialLabel = null;
	public Transform	tutorialRoot = null;
	public Transform	tutorialTurn = null;
	public Transform	tutorialJump = null;
	public Transform	tutorialSlide = null;
	public Transform	tutorialTilt = null;
	public Transform	tutorialTileIcon;
	public Transform	tutorialMeter = null;
	public Transform	tutorialAvoid = null;
	public Transform	tutorialEnv = null;
	public Transform	tutorialBalloon = null;
	public Transform	tutorialFinley = null;
	
	public UIPanelAlpha tutorialAbility;
	public UILabel		tutorialAbilityLabel;
	private int			tutorialAbilityCounter = 0;
	public Transform	tutorialAbilityButton;
	private Vector3		tutorialAbilityButtonPos;
	public UISprite		tutorialFinger;
	private Vector3		tutorialFingerScale;
	public GameObject	tutorialAbilityRing;
	public UISprite		tutorialAbilityRingSprite;
	//public Material		tutorialRingMaterial;
	private int			tutorialAbilityRingCount;
	public UISprite		tutorialAbilityGem;
	
	public UILabel		tutorialMeterLabel = null;
	public UILabel		tutorialSlideLabel = null;
	public UILabel		tutorialTiltLabel = null;
	public UILabel		tutorialAvoidLabel = null;
	public UILabel		tutorialEnvLabel = null;
	public UILabel		tutorialBalloonLabel = null;
	
	public UIPanelAlpha tutorialCollectCoins = null;
	//-- Tutorial
	//-- Fast Travel
	public FastTravelButton[]	fastTravel = null;
	public Transform envProgressMove = null;
	public UISlider envProgressSlider = null;
	//private float envProgressDistance;
	public UISprite envProgressSprite;
	public UILabel envProgressLabel;
	
	private bool isPowerMeterFinger = false;
	
	public bool ShowDebugDistance = true;
	public ParticleSystem fx_coin;
		
	public delegate void RetriggeringGemmingOfArtifact();
	public RetriggeringGemmingOfArtifact sourceArtifactMethod;
	
	protected override void Start()
	{
		base.Start();
		
		//popupNotification = Services.Get<PopupNotification>();
		 
		tutorialAbilityButtonPos = tutorialAbilityButton.transform.localPosition;
		tutorialFingerScale = tutorialFinger.transform.localScale;
		//bonusButtons = GetComponent<BonusButtons>();
		//foreach(FastTravelButton ftb in fastTravel){
		//	ftb.ingameVc = this;
		//}
		//resurrectMenu.statsRoot = statsRoot;
		
		Material mat = tutorialFinger.material;
		Shader shader = mat.shader;
		tutorialFinger.material = new Material(shader);
		tutorialFinger.material.renderQueue += 30;
		debugDistanceLabel.text = "";
		
	}
	
/*	public void OnGUI()
	{
		if (GUI.Button(new Rect(0,50,100,50), "Kill GUI"))
			NGUITools.SetActive(this.gameObject, false);
			//ShowInGameGUI(false);
	}*/
	
//	public void ShowInGameGUI(bool state)
//	{
//		NGUITools.SetActive(this.gameObject, false);
//	}
	
	public override void appear()
	{
		debugDistanceLabel.text = "";
		NGUITools.SetActive(gameObject, true);
		ResetInGameGUI();		
		UIManagerOz.SharedInstance.PaperVC.SetCurrentPage(UIManagerOz.SharedInstance.inGameVC);
		fx_coin.renderer.enabled = true;
		//base.appear();				// don't fade in alpha
	}
	
	public override void disappear()	//bool hidePaper = false) 
	{
		pauseMenu.HidePauseMenu();		//NGUITools.SetActive(UIPauseMenu, false);
		scoreUI.ResetCurrencyBars();	//lastScoreXOffset = 0.0f;
		NGUITools.SetActive(gameObject, false);
		//base.disappear();				// don't fade out alpha
	}
	
	public void ResetInGameGUI() 	
	{
		// set up head start icons that flash on run start
		//headStart.appear();
		//megaHeadStart.appear();
		
		// other stuff
		//countDownSprite.enabled = false;
		countDownLabel.gameObject.SetActiveRecursively(false);
		//wandVignette.enabled = false;
		resurrectMenu.hideResurrectMenu();	//.disableResurrectMenu();
		scoreUI.ResetCurrencyBars();	//lastScoreXOffset = 0.0f;
		coinMeter.appear();
		pauseMenu.HidePauseMenu();
		HideTutorial();	
		ResetEnvProgress();
		
		ExpandPeripheralUIelementsBackToNormalAfterPause();
		
		foreach(FastTravelButton ftb in fastTravel)
		{
			ftb.Hide();
		}
	}
	
	public void SetDistanceTotal(int distanceTotal) 
	{//expensive on 3GS
		if (ShowDebugDistance && Settings.GetBool("console-enabled", false))
		{
			debugDistanceLabel.text = "Dist: " + distanceTotal.ToString() + "  Spd: " + GameController.SharedInstance.velocityMagnitude.ToString();
		}
	}
	
	public void StopShowingDebugDistance()
	{
		ShowDebugDistance = false;
		if(Settings.GetBool("console-enabled", false))
		{
			debugDistanceLabel.text = "";
		}
	}
	
	public void SetCountDownNumber(int number) 
	{
		if (number < 0 || number >= 4)
		{
			//countDownSprite.enabled = false;
			countDownLabel.gameObject.SetActiveRecursively(false);
			coinMeter.UnPause();
			return;
		}
		//countDownSprite.enabled = true;
		//countDownSprite.spriteName = number.ToString();	
		countDownLabel.gameObject.SetActiveRecursively(true);
		countDownLabel.text = number.ToString();	
		coinMeter.Pause();
	}
	
	public void HidePauseButton()
	{
		NGUITools.SetActive(pauseButton, false);		
	}
	public void ShowPauseButton()
	{
		NGUITools.SetActive(pauseButton, true);		
	}
	
	public void OnUnPaused()
	{
		notify.Debug ("OnUnPaused");
		
		fx_coin.renderer.enabled = true;
		
		coinMeter.UnPause();
		pauseMenu.HidePauseMenu();
	//	NGUITools.SetActive(pauseButton, true);	
		
		UIManagerOz.SharedInstance.Unpause();	//.onUnPauseClicked();									//-- Notify an object that is listening for this event.
	}
	
	public void OnEscapeButtonClicked()
	{
		if( UIManagerOz.escapeHandled ) return;
		UIManagerOz.escapeHandled = true;

		if( pauseButton.active && Time.timeScale != 0f)	//!isAbilityTutorialOn)
			OnPause();
		else
		if( pauseMenu.homeButton.active )
			OnUnPaused();			
//			OnHomeButtonClicked();
	}
	
	public void OnPause()
	{
		notify.Debug ("OnPause");
		
		if (GamePlayer.SharedInstance.Dying || GamePlayer.SharedInstance.IsDead || GameController.SharedInstance.IsPaused == true) { return; }
	
		NGUITools.SetActive(pauseButton, false);			// disable in-game UI elements
		coinMeter.Pause();
		pauseMenu.ShowPauseMenu();	
		
		fx_coin.renderer.enabled = false;
		
		ShrinkPeripheralUIelementsDuringPause();	// shrink tutorial to get it out of the way, if tutorial is active
		
		UIManagerOz.SharedInstance.Pause();	//.onPauseClicked();									//-- Notify an object that is listening for this event.
		AudioManager.SharedInstance.StopFX();
		AudioManager.SharedInstance.StopStumbleProof();
		
		if (!GamePlayer.SharedInstance.HasGlindasBubble)	// don't clean up memory if resurrecting (in the bubble...)
		{
			StartCoroutine("CleanUpMemory");	// take this fine opportunity to clean up memory in-game, since we've just paused
		}
	}
	
	private IEnumerator CleanUpMemory()
	{
		yield return null;	// wait until next frame, so pause menu gets drawn first
		Resources.UnloadUnusedAssets();
		System.GC.Collect();			
	}
	
	public void OnOutOfGemsPause(RetriggeringGemmingOfArtifact source)	// triggered when trying to gem a utility without enough gems
	{
		notify.Debug ("OnOutOfGemsPause");
	
		sourceArtifactMethod = source;
		
		NGUITools.SetActive(pauseButton, false);			// disable in-game UI elements
		coinMeter.Pause();
		//pauseMenu.ShowPauseMenu();	
		
		ShrinkPeripheralUIelementsDuringPause();	// shrink tutorial to get it out of the way, if tutorial is active
		
		UIManagerOz.SharedInstance.Pause();	//.onPauseClicked();									//-- Notify an object that is listening for this event.
		
		UIConfirmDialogOz.onNegativeResponse += OnNeedMoreGemsNoInGame;
		UIConfirmDialogOz.onPositiveResponse += OnNeedMoreGemsYesInGame;
		
		UIManagerOz.SharedInstance.confirmDialog.ShowConfirmDialog("Lbl_Dialogue_MoreGems_Prompt", "Btn_No", "Btn_Yes");
	}
	
	public void OnNeedMoreGemsNoInGame() 	// use in-game only, when choosing to not go to mini store.  Used on continue/resurrect screen.
	{
		UIConfirmDialogOz.onNegativeResponse -= OnNeedMoreGemsNoInGame;
		UIConfirmDialogOz.onPositiveResponse -= OnNeedMoreGemsYesInGame;
		
		if (GameProfile.SharedInstance.Player.GetGemCount() <= 0)					
				UIManagerOz.SharedInstance.inGameVC.sourceArtifactMethod = null;	// kill attempt to gem the ability, since didn't buy any gems
						
		//if you don't want any gems, just continue with run.
	 	OnUnPaused();
	}
	
	public void OnNeedMoreGemsYesInGame() 	// use in-game only, goes to mini store.  Used on continue/resurrect screen.
	{
		UIConfirmDialogOz.onNegativeResponse -= OnNeedMoreGemsNoInGame;
		UIConfirmDialogOz.onPositiveResponse -= OnNeedMoreGemsYesInGame;
		
		UIManagerOz.SharedInstance.GoToMiniStore(ShopScreenName.Gems, false);	// "gems");	// send player to in-game mini store, gems page
	}		
	
	public void OnHomeButtonClicked()
	{
		if (Settings.GetBool("activate-quit-game-button", true))
		{
			UIConfirmDialogOz.onNegativeResponse += DisconnectHandlers;
			UIConfirmDialogOz.onPositiveResponse += ExitGame;
			//UIManagerOz.SharedInstance.confirmDialog.ShowConfirmDialog("Msg_LeaveGame","", "Btn_No", "Btn_Yes");
			UIManagerOz.SharedInstance.confirmDialog.ShowConfirmDialog("Msg_QuitGame", "Btn_No", "Btn_Yes");
		}
	}
	
	// --- jonoble: Temporary fix ---------------------------------------------
	public void OnButtonClick()
	{
		// TODO: Edit "InGameEdit.unity" and change the UIButton's function name
		// from "OnButtonClick()" to "OnShareButtonClicked()"
		OnShareButtonClicked();
	}
	// ------------------------------------------------------------------------
	
	public void OnShareButtonClicked()	
	{
		 
	}
	
	public void DisconnectHandlers()
	{
		UIConfirmDialogOz.onNegativeResponse -= DisconnectHandlers;
		UIConfirmDialogOz.onPositiveResponse -= ExitGame;	
	}
	
	public void ExitGame()	
	{
		//AnalyticsInterface.LogNavigationActionEvent( "Menu", "Pause Menu", "Main Menu" );
		
		DisconnectHandlers();
		
		// clear background with color similar to menu UI background, eliminate flashing between screens		
		UIManagerOz.SharedInstance.SetUICameraClearFlagToSolidColorBG(true);		
		
		GameController.SharedInstance.MoveCameraToTemple();		// do some stuff here to reset everything
		GameController.SharedInstance.Player.Reset();
		//GameController.SharedInstance.ResetLevelInformation();
		//GameController.SharedInstance.ShowStartingTemple();	
		
		Time.timeScale = 1.0f;		// so menus can animate correctly
		
		UIManagerOz.SharedInstance.PaperVC.goBackToIdolMenu = true;
		UIManagerOz.SharedInstance.mainVC.appear();
		UIManagerOz.SharedInstance.PaperVC.appear();
		disappear();
	}
	
	public void OnDiePostGame()
	{
		UnityEngine.Debug.Log("OnDiePostGame");
		notify.Debug ("OnDiePostGame1");
		//		PurchaseUtil.bIAnalysisWithParam("Game_Distance","Best_Distance|"+GameProfile.SharedInstance.Player.bestDistanceScore);
		//PurchaseUtil.bIAnalysisWithParam("Player_Coins","Total_Coins|"+GameProfile.SharedInstance.Player.coinCount);
		//PurchaseUtil.bIAnalysisWithParam("Player_Gems","Total_Gems|"+GameProfile.SharedInstance.Player.specialCurrencyCount);
		
		if (resurrectMenu.chooseToResurrect == true) { return; }
	
		//Moving this to the BEGINNING of the game, rather than the end (has to happen after the DistanceTraveled is reset)
		//popupNotification.ResetDistance();					//MessageBoardLastDistance = 0;

		ObjectivesRoot.playAnimations = true;				// animate in objectives in post-run screen
		
		AudioManager.SharedInstance.StopFX(true);
		
		AudioManager.SharedInstance.FadeMusicMultiplier(0.0f,0.0f);
		//AudioManager.SharedInstance.StartMainMenuMusic(0.2f);

		//Update challenges and user profile to reflect full run.
		ChallengeDataUpdater challengeUpdater = Services.Get<ChallengeDataUpdater>();
		challengeUpdater.SetChallengeData();
		
		/*
		ProfileManager userProfile = Services.Get<ProfileManager>();
		userProfile.UpdateProfile();
		*/
		
		coinMeter.AnimateCoinMeter(0.01f,0f);
		
		//ProfileManager.SharedInstance.UpdateProfile();
				
		/* eyal moving this to statsRoot so it is called after Gatcha score is added
#if UNITY_IPHONE
		GameCenterBinding.reportScore((System.Int64)GamePlayer.SharedInstance.Score, GameController.Leaderboard_HighScores);
		GameCenterBinding.reportScore((System.Int64)((int)GameController.SharedInstance.DistanceTraveled), GameController.Leaderboard_DistanceRun);
#elif UNITY_ANDROID
		//TODO add GameCircle here
#endif
		
		*/
		GameProfile.SharedInstance.UpdateCoinsPostSession(GamePlayer.SharedInstance.CoinCountTotal,false);
		
		if(GameProfile.SharedInstance.Player.GetNumberChanceTokens() > 0) // if we get chance tokens lets show the gatcha first
		{
			//NGUITools.SetActive(resurrectMenu.statsRoot.gameObject, false);
			//NGUITools.SetActive(resurrectMenu.gameObject, false);
			
			NGUITools.SetActive(this.gameObject, false);
			
			UIManagerOz.SharedInstance.gatchVC.availableCards = GameProfile.SharedInstance.Player.GetNumberChanceTokens();
			UIManagerOz.SharedInstance.gatchVC.appear();

		}
		else
		{
			UIManagerOz.SharedInstance.postGameVC.appear();
			
			 notify.Debug( string.Format( "[UIInGameViewControllerOz] - OnDiePostGame.  Weekly Objective count: {0}", WeeklyObjectives.WeeklyObjectiveCount  ) );
			
			bool testWeeklyDisplay = Settings.GetBool( "always-display-weekly-postrun", false );
			
			//if (true && WeeklyObjectives.WeeklyObjectiveCount > 0 ) // && ObjectivesDataUpdater.AreAnyWeeklyChallengesCompleted()) // TODO Remove the true, and the WeeklyObjectiveCount.  AreAnyWeeklyChallengesCompleted will do the check for if any challenges exist.
			if ( testWeeklyDisplay || ObjectivesDataUpdater.AreAnyWeeklyChallengesCompleted() )
			{
				UIManagerOz.SharedInstance.postGameVC.ShowWeeklyChallengesPage();	
			}
			else
			{
				UIManagerOz.SharedInstance.postGameVC.ShowObjectivesPage();	
			}
			
			// clear background with color similar to menu UI background, eliminate flashing between screens			
			UIManagerOz.SharedInstance.SetUICameraClearFlagToSolidColorBG(true);			
			
			disappear();	//true);
		}
	}	
	
	public void ShowMagicWandVignette() {
		notify.Warning("Calling show magic wand");
		notify.Warning(wandVignette.gameObject.name);
		
		wandVignette.enabled = true;
		//wandVignette.localPosition = new Vector3(0, 0, 0);
		//wandVignette.localRotation = Quaternion.Euler(new Vector3(0,0,90));
		//TweenPosition.Begin(wandVignette.gameObject, 1.0f, new Vector3(-Screen.width, wandVignette.localPosition.y, wandVignette.localPosition.z));
		//if(tutorialTC != null) {
			//tutorialTC.color = Color.white;	
		//}
		//tutorialTC = TweenColor.Begin (tutorialArrow.gameObject, 0.5f, new Color(1,1,1,0));
		//tutorialTC.delay = 0.5f;
		
		//AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_UI_Menu_back);
	}	
	
	public void ExpandPeripheralUIelementsBackToNormalAfterPause() 	// make them reappear
	{
		tutorialRoot.localScale = Vector3.one;
		bonusButtons.gameObject.transform.localScale = Vector3.one;
		
		foreach (FastTravelButton button in fastTravel)
			button.gameObject.transform.localScale = Vector3.one;
		
		//UIManagerOz.SharedInstance.popupNotification.transform.localScale = Vector3.one;
		//UIManagerOz.SharedInstance.popupNotificationObjectives.transform.localScale = Vector3.one;
	}
	
	public void ShrinkPeripheralUIelementsDuringPause()				// make them disappear
	{
		tutorialRoot.localScale = Vector3.zero;
		bonusButtons.gameObject.transform.localScale = Vector3.zero;
		
		foreach (FastTravelButton button in fastTravel)
			button.gameObject.transform.localScale = Vector3.zero;
		
		//UIManagerOz.SharedInstance.popupNotification.transform.localScale = Vector3.zero;
		//UIManagerOz.SharedInstance.popupNotificationObjectives.transform.localScale = Vector3.zero;		
	}	
	
	public void HideTutorial() {
		NGUITools.SetActive(tutorialArrow.gameObject, false);
		NGUITools.SetActive(tutorialRoot.gameObject, false);
	}
	
	public UIPanelAlpha ShowFinleyInstruction() {
		NGUITools.SetActive(tutorialRoot.gameObject, false);
		NGUITools.SetActive(tutorialFinley.gameObject, true);
		return tutorialFinley.GetComponent<UIPanelAlpha>();
	}
	public UIPanelAlpha ShowJumpInstruction() {
		//Debug.Log("ShowJumpInstruction");
		NGUITools.SetActive(tutorialRoot.gameObject, false);
		//NGUITools.SetActive(tutorialJump.gameObject, true);
		return tutorialJump.GetComponent<UIPanelAlpha>();
	}
	public UIPanelAlpha ShowTurnInstruction() {
		NGUITools.SetActive(tutorialRoot.gameObject, false);
		NGUITools.SetActive(tutorialTurn.gameObject, true);
		return tutorialTurn.GetComponent<UIPanelAlpha>();
	}
	public UIPanelAlpha ShowSlideInstruction() {
		NGUITools.SetActive(tutorialRoot.gameObject, false);
		NGUITools.SetActive(tutorialSlide.gameObject, true);
		return tutorialSlide.GetComponent<UIPanelAlpha>();
	}
	public UIPanelAlpha ShowAvoidInstruction() {
		NGUITools.SetActive(tutorialRoot.gameObject, false);
		NGUITools.SetActive(tutorialAvoid.gameObject, true);
		return tutorialAvoid.GetComponent<UIPanelAlpha>();
	}
	public UIPanelAlpha ShowTiltInstruction(bool balloon=false) {
		NGUITools.SetActive(tutorialRoot.gameObject, false);
		NGUITools.SetActive(tutorialTilt.gameObject, true);
		if(balloon) {
			tutorialTiltLabel.text = Localization.SharedInstance.Get("Tut_AvoidCrystals");
		}else {
			tutorialTiltLabel.text = Localization.SharedInstance.Get("Tut_TiltToCollect");
		}
		tutorialTileIcon.localRotation = Quaternion.Euler(0f,0f,-20f);
		iTween.RotateTo(tutorialTileIcon.gameObject, iTween.Hash(
			"z", 20f,
			"islocal", true,
			"time", 0.5f,
			"easetype", iTween.EaseType.easeInOutCirc,
			"looptype", iTween.LoopType.pingPong
			));
		return tutorialTilt.GetComponent<UIPanelAlpha>();
	}
	
	public UIPanelAlpha ShowEnvInstruction() {
		notify.Debug ("ShowEnvInstruction");
		NGUITools.SetActive(tutorialRoot.gameObject, false);
		NGUITools.SetActive(tutorialEnv.gameObject, true);
		return tutorialEnv.GetComponent<UIPanelAlpha>();
	}
	
	
	public UIPanelAlpha ShowMeterInstruction() {
		NGUITools.SetActive(tutorialRoot.gameObject, false);
		NGUITools.SetActive(tutorialMeter.gameObject, true);
		SetPowerMeterMessage();
		return tutorialMeter.GetComponent<UIPanelAlpha>();
	}
	
	public void SetPowerMeterMessage() {
		if(tutorialMeterLabel == null)
			return;
		
		NGUITools.SetActive(tutorialMeterLabel.gameObject, true);
		TweenScale ts = TweenScale.Begin(tutorialMeter.gameObject, 0.25f, tutorialMeter.transform.localScale);
		if(ts) {
			ts.onFinished += OnPart1PowerMeterLabel;
		}
		
		StartPowerMeterFinger();
	}
	
	public void StartPowerMeterFinger(){
		//Debug.Log ("StartPowerMeterFinger");
		// this bit will be the finger tapping
		if(isPowerMeterFinger)
			return;
		isPowerMeterFinger = true;
		tutorialAbility.gameObject.SetActiveRecursively(false);
		tutorialAbility.gameObject.active = true;
		tutorialAbility.transform.localPosition = Vector3.right * 2000f;
		
		tutorialFinger.gameObject.active = true;
		tutorialFinger.transform.localRotation = Quaternion.Euler(0f,0f,50f);
		tutorialFinger.transform.position = new Vector3(0.1f,-0.1f,0.1f);
		//tutorialFinger.transform.localPosition = new Vector3(0f,-150f,0f);
		//tutorialAbilityRing.renderer.material = tutorialRingMaterial;
		//tutorialAbilityRing.renderer.material.SetColor("_Color", new Color(1f,0f,0f,0f));
		//tutorialAbilityRing.renderer.enabled = false;
		tutorialAbilityRing.gameObject.active = true;
		tutorialAbilityRingSprite.alpha = 0f;
		tutorialAbilityRing.transform.position = tutorialFinger.transform.position + Vector3.forward * 0.1f;
		tutorialAbility.gameObject.active = true;
		TweenAlpha ta = TweenAlpha.Begin(tutorialAbility.gameObject, 0.6f, 1f);
		ta.onFinished += StartPowerMeterFinger;
	}
	
	
	public void StartPowerMeterFinger(UITweener tweener){
		ShowFingerFx();
	}
	
	
	void OnPart1PowerMeterLabel(UITweener tween) {
		GamePlayer.SharedInstance.AddPointsToPowerMeter(1000);
		tween.onFinished -= OnPart1PowerMeterLabel;
	}
	
	
	public void ShowCollectCoinsTutorial(){
		notify.Debug("ShowCollectCoinsTutorial");
		tutorialCollectCoins.alpha = 0f;
		tutorialCollectCoins.gameObject.SetActiveRecursively(true);
		TweenAlpha ta = TweenAlpha.Begin(tutorialCollectCoins.gameObject, 0.5f, 1f);
		ta.onFinished += OnShowCollectCoinsTutorial;
	}
	
	public void OnShowCollectCoinsTutorial(UITweener tween){
		tween.onFinished -= OnShowCollectCoinsTutorial;
		StartCoroutine(HideCollectCoinsTutorial());
		
	}
	private IEnumerator HideCollectCoinsTutorial(){
		yield return new WaitForSeconds(2f);
		TweenAlpha.Begin(tutorialCollectCoins.gameObject, 0.5f, 0f);
	}
	
	public UIPanelAlpha ShowBalloonInstruction() {
		notify.Debug("ShowBalloonInstruction");
		NGUITools.SetActive(tutorialRoot.gameObject, false);
		NGUITools.SetActive( tutorialBalloon.gameObject, true);
		return tutorialBalloon.GetComponent<UIPanelAlpha>();
	}
	
	public void ShowTutorialMessage(string message,  tutorialMessageType type = tutorialMessageType.SUCESS) {
		if(tutorialLabel == null)
			return;
		tutorialArrowLabelPanel.active = true;
		NGUITools.SetActive(tutorialLabel.gameObject, true);
		
		tutorialLabel.text = message;
		//tutorialLabel.MakePixelPerfect();
		
		Vector3 normalScale = new Vector3(40f,40f,40f);//tutorialLabel.transform.localScale;
		//tutorialLabel.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
		tutorialLabel.transform.localScale =  normalScale * 0.5f;
		normalScale *= 2f;
		//normalScale = new Vector3(2f, 2f, 1.0f);
		float time = 1f;

		//TweenScale.Begin(tutorialLabel.gameObject, time, normalScale);
		iTween.ScaleTo(tutorialLabel.gameObject, iTween.Hash(
			"scale", normalScale,
			"time", time
			));
			
		if(type == tutorialMessageType.FAIL){
			tutorialLabel.transform.localPosition = new Vector3(0, 0, 0);
			//TweenPosition.Begin(tutorialLabel.gameObject, time, new Vector3(0, 0, 0));
			iTween.MoveTo(tutorialLabel.gameObject, iTween.Hash(
				"position", new Vector3(0, 0, 0),
				"time", time,
				"isLocal" , true
				));
		}
		else if(type == tutorialMessageType.SUCESS){
			/*
			float y = 960f;
			if(Screen.height <= 480){
				y = 480;
			}
			*/
			tutorialLabel.transform.localPosition = new Vector3(0, 400, 0);
			iTween.MoveTo(tutorialLabel.gameObject, iTween.Hash(
				"position", new Vector3(0, 250, 0),
				"time", time,
				"isLocal" , true
				));
		}
		else if(type == tutorialMessageType.MSG){
			time = 2f;
			tutorialLabel.transform.localPosition = new Vector3(0, 400, 0);
			iTween.MoveTo(tutorialLabel.gameObject, iTween.Hash(
				"position", new Vector3(0, 250f, 0),
				"time", time,
				"isLocal" , true
				));
		}
		
		
		//tutorialLabel.color = Color.white;
		tutorialLabel.alpha = 1f;
//		TweenColor tc =  TweenColor.Begin(tutorialLabel.gameObject, time, new Color(1f,1f,1f,0f));
//		tc.delay = time;
//		tc.onFinished += OnHideTutorialMessage;
		iTween.ValueTo(tutorialLabel.gameObject, iTween.Hash(
			"from", 1f,
			"to", 0f,
			"time", time,
			"delay", time,
			"onupdate", "OnTutorialMessageUpdate",
			"onupdatetarget", gameObject
			));
		
		
		
		AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_Apprentice_01);
	}
	
	public void OnTutorialMessageUpdate(float val){
		//tutorialLabel.alpha = val;
		if(val <= 0f){
			NGUITools.SetActive(tutorialLabel.gameObject, false);
			GameController.SharedInstance.canShowNextTutorialStep = true;
			if(!GameController.SharedInstance.IsTutorialMode){
				UIManagerOz.SharedInstance.inGameVC.tutorialRoot.gameObject.SetActiveRecursively(false);
			}
		}
	}
	
	/*
	void OnHideTutorialMessage(UITweener tween) {
		tween.onFinished -= OnHideTutorialMessage;
		NGUITools.SetActive(tutorialLabel.gameObject, false);
		GameController.SharedInstance.canShowNextTutorialStep = true;
	}
	*/
	public void ShowSwipeLeft() {
		if(tutorialTP != null) {
			if(tutorialTP.enabled == true)
				return;
		}
		
		tutorialArrowLabelPanel.active = true;
		NGUITools.SetActive(tutorialArrow.gameObject, true);
		tutorialArrow.localPosition = new Vector3(0, 0, 0);
		tutorialArrow.localRotation = Quaternion.Euler(new Vector3(0,0,90));
		//tutorialTP = TweenPosition.Begin(tutorialArrow.gameObject, 1.0f, new Vector3(-Screen.width, tutorialArrow.localPosition.y, tutorialArrow.localPosition.z));
		tutorialTP = TweenPosition.Begin(tutorialArrow.gameObject, 1.0f, new Vector3(-900, tutorialArrow.localPosition.y, tutorialArrow.localPosition.z));
		if(tutorialTC != null) {
			tutorialTC.color = Color.white;	
		}
		tutorialTC = TweenColor.Begin (tutorialArrow.gameObject, 0.5f, new Color(1,1,1,0));
		tutorialTC.delay = 0.5f;
		
		//AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_turnleft_ww01);
		AudioManager.SharedInstance.PlayClip(GamePlayer.SharedInstance.characterSounds.turn_left);
	}
	
	public void ShowSwipeRight() {
		if(tutorialTP != null) {
			if(tutorialTP.enabled == true)
				return;
		}
		
		tutorialArrowLabelPanel.active = true;
		NGUITools.SetActive(tutorialArrow.gameObject, true);
		tutorialArrow.localPosition = new Vector3(0, 0, 0);
		tutorialArrow.localRotation = Quaternion.Euler(new Vector3(0,0,-90));
		//tutorialTP = TweenPosition.Begin(tutorialArrow.gameObject, 1.0f, new Vector3(Screen.width, tutorialArrow.localPosition.y, tutorialArrow.localPosition.z));
		tutorialTP = TweenPosition.Begin(tutorialArrow.gameObject, 1.0f, new Vector3(900, tutorialArrow.localPosition.y, tutorialArrow.localPosition.z));
		if(tutorialTC != null) {
			tutorialTC.color = Color.white;	
		}
		tutorialTC = TweenColor.Begin (tutorialArrow.gameObject, 0.5f, new Color(1,1,1,0));
		tutorialTC.delay = 0.5f;
		
		//AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_turnright_ww01);
		AudioManager.SharedInstance.PlayClip(GamePlayer.SharedInstance.characterSounds.turn_right);
	}
	
	TweenPosition tutorialTP = null;
	TweenColor tutorialTC = null;
	public void ShowSwipeUp() {
		if(tutorialTP != null) {
			if(tutorialTP.enabled == true)
				return;
		}
		
		tutorialArrowLabelPanel.active = true;
		NGUITools.SetActive(tutorialArrow.gameObject, true);
		tutorialArrow.localPosition = new Vector3(0, 0, 0);
		tutorialArrow.localRotation = Quaternion.Euler(new Vector3(0,0,0));
		//tutorialTP = TweenPosition.Begin(tutorialArrow.gameObject, 1.0f, new Vector3(tutorialArrow.localPosition.x, Screen.height, tutorialArrow.localPosition.z));
		tutorialTP = TweenPosition.Begin(tutorialArrow.gameObject, 1.0f, new Vector3(tutorialArrow.localPosition.x, 900, tutorialArrow.localPosition.z));
		if(tutorialTC != null) {
			tutorialTC.color = Color.white;	
		}
		tutorialTC = TweenColor.Begin (tutorialArrow.gameObject, 0.5f, new Color(1,1,1,0));
		tutorialTC.delay = 0.5f;
		
		//AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_turnleft_ww01);
		AudioManager.SharedInstance.PlayClip(GamePlayer.SharedInstance.characterSounds.turn_right);
	}
	
	public void ShowSwipeDown() {
		if(tutorialTP != null) {
			if(tutorialTP.enabled == true)
				return;
		}
		
		tutorialArrowLabelPanel.active = true;
		NGUITools.SetActive(tutorialArrow.gameObject, true);
		tutorialArrow.localPosition = new Vector3(0, 100, 0);
		tutorialArrow.localRotation = Quaternion.Euler(new Vector3(0,0,180));
		//tutorialTP = TweenPosition.Begin(tutorialArrow.gameObject, 1.0f, new Vector3(tutorialArrow.localPosition.x, -Screen.height, tutorialArrow.localPosition.z));
		tutorialTP = TweenPosition.Begin(tutorialArrow.gameObject, 1.0f, new Vector3(tutorialArrow.localPosition.x, -900, tutorialArrow.localPosition.z));
		if(tutorialTC != null) {
			tutorialTC.color = Color.white;	
		}
		tutorialTC = TweenColor.Begin (tutorialArrow.gameObject, 0.5f, new Color(1,1,1,0));
		tutorialTC.delay = 0.5f;
		
		//AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_turnright_ww01);
		AudioManager.SharedInstance.PlayClip(GamePlayer.SharedInstance.characterSounds.turn_right);
	}
	
	public void OnPowerButton()		// act as if he did a double tap and use the power
	{
		GameController.SharedInstance.UsePower();
		if(GameController.SharedInstance.IsTutorialMode){
			tutorialMeter.GetComponent<UIPanelAlpha>().alpha = 0f;
			GameController.SharedInstance.TimeSinceTutorialEnded = 0.0f;
			GameController.SharedInstance.canShowNextTutorialStep = true;
		}
	}
	
	
	public void ShowEnvChangeHud(string message) {
		if(envChangeHudLabel == null)
			return;
		
		NGUITools.SetActive(envChangeHud.gameObject, true);
		NGUITools.SetActive(envChangeHudLabel.gameObject, true);
		NGUITools.SetActive(envChangeHudArrow, true);
		
		envChangeHudLabel.text = message;
		//envChangeHudLabel.MakePixelPerfect();

		envChangeHud.alpha = 0f;
		TweenAlpha ta =  envChangeHud.GetComponent<TweenAlpha>();
		if(ta){
			ta.delay = 0f;
		}
		ta =  TweenAlpha.Begin(envChangeHud.gameObject, 1f, 1f);
		ta.onFinished += OnCompleteEnvChangeHud;
		

		
		AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_UI_scoreTally_fireworks_01, 0.3f);
	}
	

	void OnCompleteEnvChangeHud(UITweener tween) {
		tween.onFinished -= OnCompleteEnvChangeHud;
		TweenAlpha ta =  TweenAlpha.Begin(envChangeHud.gameObject, 2f, 0.98f);
		ta.onFinished += OnHideEnvAllChangeHud;

	}
	
	
	void OnHideEnvAllChangeHud(UITweener tween) {
		tween.onFinished -= OnCompleteEnvChangeHud;
		TweenAlpha ta =  envChangeHud.GetComponent<TweenAlpha>();
		if(ta){
			ta.delay = 3f;
			
		}
		ta =  TweenAlpha.Begin(envChangeHud.gameObject, 1f, 0f);
		ta.onFinished += OnHideEnvChangeHud;

	}
	
	void OnHideEnvChangeHud(UITweener tween) {
		tween.onFinished -= OnHideEnvChangeHud;
		//envChangeHudLabel.MakePixelPerfect();
		NGUITools.SetActive(envChangeHud.gameObject, false);
		NGUITools.SetActive(envChangeHudLabel.gameObject, false);
		NGUITools.SetActive(envChangeHudArrow, false);
	}
	
	void OnClickButton()
	{
		this.OnShareButtonClicked();	
	}
	
	
	public void EmitCoin()
	{
		fx_coin.Emit(1);
	}
	
	public void ShowFastTravel()
	{
		ShowFastTravelNow();	//Invoke("ShowFastTravelNow", 3f);
	}
	
	private void ShowFastTravelNow()
	{
		if (!GamePlayer.SharedInstance.IsDead && GamePlayer.SharedInstance.HasFastTravel || true)
		{
			List<FastTravelButton> buttonsToShow = new List<FastTravelButton>();
			
			foreach (FastTravelButton ftb in fastTravel)
			{
				if (ftb.IsSetDownloaded() && EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.SetId != ftb.environmentSetId &&
					!GamePlayer.SharedInstance.HasBoost && !GamePlayer.SharedInstance.HasFastTravel &&
					GameProfile.SharedInstance.Player.GetConsumableCount(ftb.GetFastTravelConsumableID(ftb.environmentSetId)) > 0)
						buttonsToShow.Add(ftb);   //ftb.Show();
			}
			
			if (buttonsToShow.Count == 1)		// if one ftb to show, center it
				buttonsToShow[0].Show(0);
			else if (buttonsToShow.Count == 2)	// if two ftb's to show, show them side by side
			{
				buttonsToShow[0].Show(-70);			
				buttonsToShow[1].Show(70);		
			}
			else if (buttonsToShow.Count == 3)	// if three ftb's to show, show them side by side
			{
				buttonsToShow[0].Show(-140);			
				buttonsToShow[1].Show(0);
				buttonsToShow[2].Show(140);
			}
		}
	}

	public void HideFastTravel()
	{
		if(!GamePlayer.SharedInstance.IsDead && GamePlayer.SharedInstance.HasFastTravel || true)
		{
			foreach(FastTravelButton ftb in fastTravel)
			{
				ftb.Hide();
			}

		}
	}
	
	private void ResetEnvProgress(){
		envProgressSlider.sliderValue = 0;
		envProgressMove.transform.localPosition = new Vector3(0f, 120f, -200f);
		envProgressMove.gameObject.SetActiveRecursively(false);
	}
	
	private int envprogfadecount = 0;
	public void FadeInEnvProgress()
	{
		envprogfadecount--;
		if(envprogfadecount<=0)
			TweenAlpha.Begin(envProgressMove.gameObject,0.25f,1f);
	}
	
	public void FadeOutEnvProgress()
	{
		envprogfadecount++;
		TweenAlpha.Begin(envProgressMove.gameObject,0.25f,0f);
	}
	
	public bool showingEnvProgress = false;
	public void ShowEnvProgress(int setId){
		showingEnvProgress = true;
	 	envProgressMove.gameObject.SetActiveRecursively(true);
		switch(setId){
			case 1: envProgressSprite.spriteName = "icon_map_whimsiewoods";
				break;
			case 2: envProgressSprite.spriteName = "icon_map_darkforest";
				break;
			case 3: envProgressSprite.spriteName = "icon_map_winkiecountry";
				break;
			case 4: envProgressSprite.spriteName = "icon_location_emeraldcity";
				break;
		}
		envProgressLabel.text = Localization.SharedInstance.Get(EnvironmentSetManager.SharedInstance.LocalDict[setId].Title);
		iTween.MoveTo(envProgressMove.gameObject, iTween.Hash(
			"y", 0f,
			"islocal", true,
			"time", 0.5f
			));
		//envProgressDistance = GameController.SharedInstance.RealDistanceTraveled;
		lastDistTravelled = GameController.SharedInstance.RealDistanceTraveled;
		curDistanceRatio = 0f;
		estimatedDistLeft = 1000f;
	}
		
	public void HideEnvProgress(){
		showingEnvProgress = false;
		iTween.MoveTo(envProgressMove.gameObject, iTween.Hash(
			"y", 120f,
			"islocal", true,
			"time", 0.5f,
			"oncomplete", "HideEnvProgressComplete",
			"oncompletetarget", gameObject
			));
	}
	public void HideEnvProgressComplete(){
		envProgressMove.gameObject.SetActiveRecursively(false);
	}
	
	public bool isAbilityTutorialOn = false;
	private bool isUtilityTutorial = false;
	private Vector3 playerSpeedAbilityTutorial;
	public void ShowAbilityTutorial(bool isUtility = false){
		Enemy.main.StopAudio();
		
		playerSpeedAbilityTutorial = GamePlayer.SharedInstance.GetPlayerVelocity();
		UIManagerOz.SharedInstance.inGameVC.bonusButtons.CancelHideConsumableAndModifierButtons();
		UIManagerOz.SharedInstance.inGameVC.bonusButtons.EnableAllButtons(false);
		if(isUtility){
			isUtilityTutorial = true;
		}
		else{
			isUtilityTutorial = false;
		}
		
		notify.Debug("ShowAbilityTutorial " + isUtility);
		
		isAbilityTutorialOn = true;
		
		iTween.ValueTo(gameObject, iTween.Hash(
			"from", 1f,
			"to", 0f,
			"time", 0.5f,
			"onupdate", "OnUpdateTimeScale",
			"onupdatetarget", gameObject,
			"ignoretimescale", true
			));
		
		bonusButtons.StopCoroutine("BlinkIcons");
		
		tutorialAbilityCounter = 0;
		tutorialAbility.transform.localPosition = Vector3.zero;
		tutorialAbility.alpha = 0f;
		tutorialAbility.gameObject.SetActiveRecursively(true);
		tutorialAbilityButton.transform.localPosition = tutorialAbilityButtonPos;
		tutorialAbilityButton.collider.enabled = true;
		//tutorialAbilityRing.renderer.enabled = false;
		//tutorialAbilityRing.renderer.material = tutorialRingMaterial;
		tutorialAbilityRingSprite.alpha = 0f;
		tutorialAbilityGem.enabled = false;
		Transform ab;
		if(isUtilityTutorial){
			tutorialAbilityLabel.text = Localization.SharedInstance.Get("Tut_HUD_Cons");
			UIManagerOz.SharedInstance.inGameVC.bonusButtons.tutorialButtonAbility = null;
			ab = UIManagerOz.SharedInstance.inGameVC.bonusButtons.tutorialButtonUtility.transform;
			ab.localPosition = new Vector3( ab.localPosition.x, ab.localPosition.y,  -150f);
			tutorialFinger.transform.localPosition = new Vector3(-300f,-1000f,-200f);
			tutorialFinger.transform.localRotation = Quaternion.Euler(0f,0f,-130f);
			Vector3 pos = ab.position - Vector3.forward * 1.2f;
			tutorialAbilityButton.transform.localPosition = new Vector3(1000f, 0f, -200f);
			tutorialAbilityRing.transform.position = ab.position - Vector3.forward * 0.2f;
			iTween.MoveTo(tutorialFinger.gameObject, iTween.Hash(
				"position", pos,
				"time", 1.6f,
				"oncomplete", "ShowFingerFx",
				"oncompletetarget", gameObject,
				"ignoretimescale", true
				));
		}
		else{
			string loc = Localization.SharedInstance.Get("Tut_InGame_ModifierPrompt_1");
			string powerupName = Localization.SharedInstance.Get(UIManagerOz.SharedInstance.inGameVC.bonusButtons.tutorialButtonAbility.GetComponent<BonusButton>().bonusName);
			string text = string.Format(loc, powerupName);

			tutorialAbilityLabel.text = text;
			tutorialFinger.transform.localPosition = new Vector3(300f,-1000f,-200f);
			tutorialFinger.transform.localRotation = Quaternion.Euler(0f,0f,130f);
			UIManagerOz.SharedInstance.inGameVC.bonusButtons.tutorialButtonUtility = null;
			ab = UIManagerOz.SharedInstance.inGameVC.bonusButtons.tutorialButtonAbility.transform;
			ab.localPosition = new Vector3( ab.localPosition.x, ab.localPosition.y,  -150f);
			tutorialAbilityButton.transform.localPosition = new Vector3(tutorialAbilityButton.transform.localPosition.x, tutorialAbilityButton.transform.localPosition.y, -200f);
		}
		tutorialAbility.GetComponent<ScaleModalDialog>().SetScale();
		TweenAlpha.Begin(tutorialAbility.gameObject, 0.5f, 1f);
	}
	
	public void HideAbilityTutorial(){
		if(!isAbilityTutorialOn)
			return;
		isAbilityTutorialOn = false;
		notify.Debug("HideAbilityTutorial " + isUtilityTutorial);
		GamePlayer.SharedInstance.SetPlayerVelocity(playerSpeedAbilityTutorial.magnitude);
		BonusButtons bb = UIManagerOz.SharedInstance.inGameVC.bonusButtons;
		
		if(isUtilityTutorial){
			PlayerPrefs.SetInt("utilityTutorialPlayedInt",1);
			GameController.SharedInstance.utilityTutorialPlayed = true;
			GameProfile.SharedInstance.Player.EarnConsumable(bb.tutorialButtonUtility.GetComponent<BonusButton>().artifactId,1);
			//UIManagerOz.SharedInstance.inGameVC.bonusButtons.EnableAllButtonsUtility(true);
		}
		else{
			PlayerPrefs.SetInt("abilityTutorialPlayedInt",1);
			GameController.SharedInstance.abilityTutorialPlayed = true;
			//UIManagerOz.SharedInstance.inGameVC.bonusButtons.EnableAllButtonsAbility(true);
		}
		
		//bb.EnableAllButtons(true);
		
		iTween.ValueTo(gameObject, iTween.Hash(
			"from", 0f,
			"to", 1f,
			"time", 0.5f,
			"onupdate", "OnUpdateTimeScale",
			"onupdatetarget", gameObject,
			"ignoretimescale", true
			));
		TweenAlpha ta =  TweenAlpha.Begin(tutorialAbility.gameObject, 0.4f, 0f);
		ta.onFinished += OnHideAbilityTutorial;
		bb.HideConsumableAndModifierButtonsNow();
		if(isUtilityTutorial){
			//Transform t =  bb.tutorialButtonUtility.transform;
			//t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, 0f);
			bb.tutorialButtonUtility = null;
			// this is if we use both tutorials together
			//if(GameController.SharedInstance.abilityTutorialPlayed || !bb.CanShowModifiers()){
			//	bb.HideConsumableAndModifierButtons();
			//}
			
		}
		else{
			//Transform t =  UIManagerOz.SharedInstance.inGameVC.bonusButtons.tutorialButtonAbility.transform;
			//t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, 0f);
			bb.tutorialButtonUtility = null;
			
			//if(GameController.SharedInstance.utilityTutorialPlayed || !bb.CanShowConsumables()){
			//	bb.HideConsumableAndModifierButtons();
			//}

		}
		
	}
	
	public void OnHideAbilityTutorial(UITweener tweener){
		notify.Debug ("OnHideAbilityTutorial " + GameController.SharedInstance.abilityTutorialPlayed + " " + GameController.SharedInstance.utilityTutorialPlayed);
		tweener.onFinished -= OnHideAbilityTutorial;
		tutorialAbility.gameObject.SetActiveRecursively(false);		
		//UIManagerOz.SharedInstance.inGameVC.bonusButtons.HideConsumableAndModifierButtons();
	}
	
	public void OnAbilityOkClicked(){
		if(tutorialAbilityCounter == 0){
			tutorialAbilityLabel.text = Localization.SharedInstance.Get("Tut_InGame_ModifierPrompt_2");
			tutorialAbility.GetComponent<ScaleModalDialog>().SetScale();
		}
		if(tutorialAbilityCounter == 1){
			BonusButton bb = UIManagerOz.SharedInstance.inGameVC.bonusButtons.tutorialButtonAbility.GetComponent<BonusButton>();
			string powerupName =  Localization.SharedInstance.Get(bb.bonusName);
			string powerupDesc =  Localization.SharedInstance.Get(bb.desc);
			string loc3 = Localization.SharedInstance.Get("Tut_InGame_ModifierPrompt_3");
			string loc4 = Localization.SharedInstance.Get("Tut_InGame_ModifierPrompt_4");
			string text = loc3 + "\n" + string.Format(loc4,powerupName, powerupDesc);
			tutorialAbilityLabel.text = text;
			tutorialAbility.GetComponent<ScaleModalDialog>().SetScale();
			tutorialAbilityButton.gameObject.SetActiveRecursively(false);
			Vector3 pos;

			UIManagerOz.SharedInstance.inGameVC.bonusButtons.tutorialButtonAbility.transform.localPosition -= Vector3.forward * 90f;
			tutorialAbilityRing.transform.position = UIManagerOz.SharedInstance.inGameVC.bonusButtons.tutorialButtonAbility.transform.position - Vector3.forward * 0.2f;
			pos = UIManagerOz.SharedInstance.inGameVC.bonusButtons.tutorialButtonAbility.transform.position - Vector3.forward * 1.2f;
		
			iTween.MoveTo(tutorialFinger.gameObject, iTween.Hash(
				"position", pos,
				"time", 1.6f,
				"oncomplete", "ShowFingerFx",
				"oncompletetarget", gameObject,
				"ignoretimescale", true
				));
			tutorialAbilityGem.enabled = true;
			//give the player a gem
			//GamePlayer.SharedInstance.AddGemsToScore(1);
			GameProfile.SharedInstance.Player.specialCurrencyCount ++;
		}
		
		tutorialAbilityCounter++;
		
	}
	
	public void ShowFingerFx(){
		
		if(isAbilityTutorialOn){
			if(isUtilityTutorial){
				notify.Debug("ShowFingerFx " + UIManagerOz.SharedInstance.inGameVC.bonusButtons.tutorialButtonUtility.name);
				UIManagerOz.SharedInstance.inGameVC.bonusButtons.tutorialButtonUtility.collider.enabled = true;
			}
			else{
				notify.Debug("ShowFingerFx " + UIManagerOz.SharedInstance.inGameVC.bonusButtons.tutorialButtonAbility.name);
				UIManagerOz.SharedInstance.inGameVC.bonusButtons.tutorialButtonAbility.collider.enabled = true;
			}
		}
		
		tutorialAbilityRing.transform.localScale = Vector3.one;
		//tutorialAbilityRing.renderer.enabled = true;
		//tutorialAbilityRing.renderer.enabled = false;
		tutorialAbilityRingCount = 0;
		EmitRing();
		
	}
	
	private void EmitRing(){
		tutorialAbilityRingCount++;
		if(tutorialAbilityRingCount > 2 && !isAbilityTutorialOn){
			OnRingComplete();
			return;
		}

		iTween.ScaleTo(tutorialFinger.gameObject, iTween.Hash(
			"scale", tutorialFingerScale * 1.4f,
			"time", 0.05f,
			"ignoretimescale", true,
			"easetype", iTween.EaseType.easeOutCirc,
			"oncomplete", "OnFingerScaleComplete",
			"oncompletetarget", gameObject
			));
	}
	
	public void OnFingerScaleComplete(){
		iTween.ScaleTo(tutorialFinger.gameObject, iTween.Hash(
			"scale", tutorialFingerScale / 1.4f,
			"time", 0.05f,
			"ignoretimescale", true,
			"easetype", iTween.EaseType.easeInCirc,
			"oncomplete", "StartRingAnim",
			"oncompletetarget", gameObject
			));
	}
	public void StartRingAnim(){
		iTween.ValueTo(tutorialAbilityRing, iTween.Hash(
			"from", 0f,
			"to", 1f,
			"time", 0.3f,
			"ignoretimescale", true,
			"onupdate", "OnUpdateRing",
			"onupdatetarget", gameObject
			));
	}
	
	public void OnUpdateRing(float val){
		tutorialAbilityRing.transform.localScale = Vector3.one * (10f +150f * val );
		//tutorialAbilityRing.renderer.material.SetColor("_Color",new Color(1f,0f,0f,0.8f - val * 0.8f));
		tutorialAbilityRingSprite.alpha = 1f - val ;
		if(val >= 1)
			EmitRing();
	}
	
	private void OnRingComplete(){
		if(GameController.SharedInstance.IsTutorialMode){
			isPowerMeterFinger = false;
			tutorialAbilityRing.active = false;
			TweenAlpha.Begin(tutorialAbility.gameObject, 0.6f, 0f);
		}
	}
	
	public void OnUpdateTimeScale(float val){
		Time.timeScale = val;
		if(isAbilityTutorialOn){
			UIManagerOz.SharedInstance.inGameVC.bonusButtons.GetComponent<UIPanelAlpha>().alpha = 1 - val;
		}
		if(Time.timeScale == 0f){
			UIManagerOz.SharedInstance.inGameVC.bonusButtons.MakeButtonsStatic(false);
		}
	}
	
	public void SetEstimatedDistanceLeft(float dist)
	{
		estimatedDistLeft = dist;
	}
	
	private float curDistanceRatio = 0f;
	private float lastDistTravelled = 0f;
	private float estimatedDistLeft = 0f;
	void Update(){
		if(envProgressSlider.gameObject.active){
			
			float delta = GameController.SharedInstance.RealDistanceTraveled - lastDistTravelled;
			float ratioToAdd = delta/estimatedDistLeft;
			
			curDistanceRatio += (1-curDistanceRatio)*ratioToAdd;
			
		//	float ratio = (GameController.SharedInstance.DistanceTraveled - envProgressDistance) / 1100f;
			envProgressSlider.sliderValue = curDistanceRatio;
			
			estimatedDistLeft -= delta;
			lastDistTravelled = GameController.SharedInstance.RealDistanceTraveled;
		}
		
	}
}

