/*

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIInGameViewController : UIViewController
{
	public static event voidClickedHandler 	onUnPauseClicked = null;
	public static event voidClickedHandler 	onPauseClicked = null;
	
	public Transform 	PowerMeter;
	public UILabel 		ScoreLabel;
	public UILabel 		CoinLabel;
	public UISprite 	CountDownSprite = null;
	public Transform 	activePowerIcon = null;
	public GameObject 	UIPauseMenu;
	public Transform 	pauseClickedSprite;
	public Transform 	pauseSprite;
	public Transform 	scoreBar;
	public Transform 	coinBar;
	public Transform 	coinGlow = null;
	public Transform	coinMeterGlow = null;
	public Transform 	distanceMeter = null;
	public UILabel		DistanceLabel = null;
	public Transform 	resurrectMenu;
	public Transform 	resurrectProgressBar = null;
	public UILabel		resurrectionCostLabel = null;
	
	public UILabel		headStartCostLabel = null;
	public UILabel		megaheadStartCostLabel = null;
	
	public Transform	headStartRoot = null;
	public Transform	megaHeadStartRoot = null;
	
	private Vector3		startingHeadStartPosition = Vector3.zero;
	
	public UIPostGameViewController	postGameVC = null;
	
	private bool firstUpdate = true;
	private bool blinking = false;
	
	public override void Awake()
	{
		base.Awake();	
	}
	
	new void Start() {
		if(headStartRoot != null) {
			startingHeadStartPosition = headStartRoot.localPosition;
		}
	}
	
	public override void appear() {
		base.appear();
		//why is this done here?
		GameController.SharedInstance.SetMSAAByDevice();
		NGUITools.SetActive(UIPauseMenu, false);
		firstUpdate = true;
		blinking = false;
		
		if(CountDownSprite) {
			CountDownSprite.enabled = false;
		}
		
		if(pauseClickedSprite != null) {
			NGUITools.SetActiveSelf(pauseClickedSprite.gameObject, false);
		}
		
		if(coinGlow != null) {
			NGUITools.SetActive(coinGlow.gameObject, false);
		}
		if(coinMeterGlow != null) {
			NGUITools.SetActive(coinMeterGlow.gameObject, false);
		}
		if(activePowerIcon != null) {
			NGUITools.SetActive(activePowerIcon.gameObject, false);
			
			CharacterStats activeCharacter = GameProfile.SharedInstance.GetActiveCharacter();
			if(activeCharacter != null) {
				BasePower power = PowerStore.PowerFromID(activeCharacter.powerID);
				if(power != null) {
					NGUITools.SetActive(activePowerIcon.gameObject, true);
					SetPowerIcon(power.IconName);
				}
			}
		}
		
		if(headStartRoot != null) {
			NGUITools.SetActive(headStartRoot.gameObject, false);
		}
			
		if(megaHeadStartRoot != null) {
			NGUITools.SetActive(megaHeadStartRoot.gameObject, false);
		}
		
		if(headStartCostLabel != null) {
			headStartCostLabel.text = GameProfile.SharedInstance.Player.GetHeadStartCost().ToString();
		}
		if(megaheadStartCostLabel != null) {
			megaheadStartCostLabel.text = GameProfile.SharedInstance.Player.GetMegaHeadStartCost().ToString();
		}
		
		disableResurrectMenu();
		
		lastScoreXOffset = 0.0f;
		DistanceMeterFinishHideAnimation();
	}
	
	public override void disappear(bool hidepaper) {
		base.disappear(hidepaper);
		NGUITools.SetActive(UIPauseMenu, false);
		lastScoreXOffset = 0.0f;
		
	}
	
	public void disableResurrectMenu() {
		if(resurrectMenu != null) {
			NGUITools.SetActive(resurrectMenu.gameObject, false);
		}
	}
	
	public void enableResurrectMenu() {
		notify.Debug("enableResurrectMenu");
		if(resurrectMenu != null) {
			NGUITools.SetActive(resurrectMenu.gameObject, true);
			if(resurrectionCostLabel != null && GameController.SharedInstance != null) {
				resurrectionCostLabel.text = GameProfile.SharedInstance.GetResurrectionCost().ToString();
				notify.Debug("enableResurrectMenu with cost {0}", resurrectionCostLabel.text);
			}
		}
	}
	
	public void SetPowerIcon(string iconname) {
		if(activePowerIcon == null)
			return;
		
		if(spritePowerIcon == null) {
			spritePowerIcon = activePowerIcon.GetComponent<UISprite>();
		}
		
		//-- set the correct sprite name.
		if(spritePowerIcon) {
			spritePowerIcon.spriteName = iconname + "_meter";
			spritePowerIcon.MakePixelPerfect();
		}
	}
	
	private UISprite spritePowerIcon = null;
	public void ShowPowerIconAndGlow(bool show, string iconname = null) {
		if(activePowerIcon == null)
			return;
		
		activePowerIcon.gameObject.active = show;
		if(show && iconname != null) {
			
			if(spritePowerIcon == null) {
				spritePowerIcon = activePowerIcon.GetComponent<UISprite>();
			}
			
			//-- set the correct sprite name.
			spritePowerIcon.spriteName = iconname + "_meter";
			spritePowerIcon.MakePixelPerfect();
		}
		
		if(coinMeterGlow) {
			NGUITools.SetActive(coinMeterGlow.gameObject, show);
			coinMeterGlow.renderer.material.color = Color.white;
		}
	}
	
	public void FadePowerGlow() {
		if(coinMeterGlow == null)
			return;
		if(coinMeterGlow.gameObject.active == false)
			return;
		
		coinMeterGlow.animation.Play();
	}
	
	public void ActivePowerIcon() {
		if(activePowerIcon == null)
			return;
		if(activePowerIcon.gameObject.active == false)
			return;
	}
	
	bool firstTime = true;
	float VDelta = 0.0f;
	public void SetPowerProgress(float progress) {
		if(PowerMeter != null) {
			PowerMeter.localScale = new Vector3(100,100,100.0f*progress);
			MeshFilter mf = PowerMeter.GetComponent<MeshFilter>() as MeshFilter;
			if(mf != null) {
				Vector2[] uvs = mf.mesh.uv;
				if(firstTime == true) {
					firstTime = false;
					VDelta = uvs[0].y - uvs[1].y;
				}
				uvs[0].y = uvs[1].y + progress*VDelta;
				uvs[2].y = uvs[1].y + progress*VDelta;
				mf.mesh.uv = uvs;
			}
		}
	}
	
	private int _lastScore = 0;
	private int _score = 0;
	public void SetScore(int score) {
		if(ScoreLabel == null)
			return;
		_score = score;
	}
	
	private int _lastCoinCount = 0;
	private int _coinCount = 0;
	public void SetCoinCount(int coins) {
		if(CoinLabel == null)
			return;
		_coinCount = coins;		
	}
	
	
	public void SetCountDownNumber(int number) {
		if(CountDownSprite == null)
			return;
		
		if(number < 0 || number >= 4) {
			CountDownSprite.enabled = false;
			return;
		}
		CountDownSprite.enabled = true;
		if(number == 1)
			CountDownSprite.spriteName = "countdown_1";		
		else if(number == 2)
			CountDownSprite.spriteName = "countdown_2";		
		else if(number == 3)
			CountDownSprite.spriteName = "countdown_3";
		else
			CountDownSprite.spriteName = "";	
	}
	
	public void OnUnPaused()
	{
		appear();
		if (UIPauseMenu != null)
		{
			NGUITools.SetActive(UIPauseMenu, false);
		}
		NGUITools.SetActive(activePowerIcon.gameObject, true);
		//-- Notify an object that is listening for this event.
		if(onUnPauseClicked != null) {
			onUnPauseClicked();
		}
		//FlurryBinding.logEvent("ingame_unpause", false);
		//AnalyticsInterface.LogAnalyticsEvent( "ingame_unpause" );
	}
	
	public void OnPause()
	{
		if(GameController.SharedInstance.IsPaused == true)
			return;
		
		NGUITools.SetActive(pauseSprite.gameObject, false);
		NGUITools.SetActive(pauseClickedSprite.gameObject, false);
		NGUITools.SetActive(UIPauseMenu, true);
		NGUITools.SetActive(headStartRoot.gameObject, false);
		NGUITools.SetActive(megaHeadStartRoot.gameObject, false);
		turnOffHeadStartBlink();
		
		//-- Notify an object that is listening for this event.
		if(onPauseClicked != null) {
			onPauseClicked();
		}
		//FlurryBinding.logEvent("ingame_pause", false);
		//AnalyticsInterface.LogAnalyticsEvent( "ingame_pause" );
	}
	
	public void OnPausePress() {
		if(pauseClickedSprite != null) {
			NGUITools.SetActiveSelf(pauseClickedSprite.gameObject, true);
		}
	}
	public void OnPauseRelease() {
		if(pauseClickedSprite != null) {
			NGUITools.SetActiveSelf(pauseClickedSprite.gameObject, false);
		}
	}
	
	float lastScoreXOffset = 0.0f;
	float lastCoinXOffset = 0.0f;
	void respositionCurrencyBars()
	{
		if(scoreBar != null) {
			float x = ScoreLabel.relativeSize.x * ScoreLabel.transform.localScale.x * 0.5f;
			if(x > lastScoreXOffset) {
				scoreBar.transform.localPosition = new Vector3(340.0f-x, scoreBar.transform.localPosition.y, scoreBar.transform.localPosition.z);	
				lastScoreXOffset = x;
			}
		}
		
		if(coinBar != null) {
			float x = CoinLabel.relativeSize.x * CoinLabel.transform.localScale.x * 0.5f;
			if( x > lastCoinXOffset) {
				coinBar.transform.localPosition = new Vector3(165.0f-x, coinBar.transform.localPosition.y, coinBar.transform.localPosition.z);	
				lastCoinXOffset = x;
			}
		}
	}
	
	bool isDistanceMeterOnScreen = false;
	private Vector3 UpPosition = new Vector3(0, 0, 0);
	private Vector3 DownPosition = new Vector3(0, 0, 0);
	public int MessageBoardLastDistance = 0;
	public int EarlyMessageBoardSpan = 250;
	public int LateMessageBoardSpan = 500;
	public int EarlyToLateDistance = 1249;
	
	public void ShowDistanceMeterWithDistance(int distance) {
		if(isDistanceMeterOnScreen == true || DistanceLabel == null || distanceMeter == null)
			return;

		UpPosition.y = Screen.height;
		DownPosition.y = 375;//Screen.height * 0.5f;
		
		NGUITools.SetActive(distanceMeter.gameObject, true);
		distanceMeter.localPosition = UpPosition;
		DistanceLabel.text = string.Format("{0}m", distance);
		StartCoroutine(AnimateMessageBoard());
	}
	
	public IEnumerator AnimateMessageBoard()
	{
		AudioManager.Instance.PlayFX(AudioManager.Effects.oz_UI_Menu_back);
		distanceMeter.localPosition = UpPosition;
		isDistanceMeterOnScreen = true;
		TweenPosition.Begin(distanceMeter.gameObject, 0.25f, DownPosition);
		yield return new WaitForSeconds(2);
		
		TweenPosition outTp = TweenPosition.Begin(distanceMeter.gameObject, 0.25f, UpPosition);
		outTp.eventReceiver = this.gameObject;
		outTp.callWhenFinished = "DistanceMeterFinishHideAnimation";
	}
	
	public void DistanceMeterFinishHideAnimation() {
		if(distanceMeter == null)
			return;
		
		isDistanceMeterOnScreen = false;
		NGUITools.SetActive(distanceMeter.gameObject, false);
	}
	
	public bool chooseToResurrect = false;
	public void OnResurrect() {
		chooseToResurrect = true;
		//-- Can afford the res?
		if(GameProfile.SharedInstance.Player.CanAffordResurrect() == false) {
			UIConfirmDialog.onNegativeResponse += OnNeedMoreGemsNo;
			UIConfirmDialog.onPositiveResponse += OnNeedMoreGemsYes;
			UIManager.SharedInstance.confirmDialog.ShowConfirmDialog("Lbl_Dialogue_MoreGems_Prompt","Lbl_Dialogue_MoreGems_Confirm", "Btn_No", "Btn_Yes");
			return;	
		}
		
		//-- Turn off the countdown.
		if(resurrectProgressBar != null) {
			TweenSlider ts = resurrectProgressBar.GetComponent<TweenSlider>();
			if(ts != null) {
				ts.enabled = false;
			}
		}
		
//		Dictionary<string, string> flurryData = new Dictionary<string, string>();
//		flurryData.Add("cost", GameController.SharedInstance.ResurrectionCost.ToString());
//		flurryData.Add("currenteDistance", ((int)GameController.SharedInstance.DistanceTraveled).ToString());
//		
//		string analyticsParams = "cost," + GameController.SharedInstance.ResurrectionCost.ToString();
//		analyticsParams += ",currentDistance," + ((int)GameController.SharedInstance.DistanceTraveled).ToString();
		
		GameProfile.SharedInstance.Player.specialCurrencyCount -= GameProfile.SharedInstance.GetResurrectionCost();
		GameProfile.SharedInstance.Serialize();
		GameController.SharedInstance.ResurrectionCost *= 2;
		GameController.SharedInstance.ResurrectsThisRun+=1;
		GameController.SharedInstance.IsResurrecting = true;
		
//		flurryData.Add("count", GameController.SharedInstance.ResurrectsThisRun.ToString());
//		FlurryBinding.logEventWithParameters("resurrect", flurryData, false);
	
		//-- Find safe spot
		if(GamePlayer.SharedInstance.MovePlayerToNextSafeSpot() == true) {
			//-- give invulnerable for a few seconds.
			GamePlayer.SharedInstance.Resurrect();
			
			GameController.SharedInstance.IsHandlingEndGame = false;
			GameController.SharedInstance.IsGameOver = false;
			
			
			show();
			OnPause();
			OnUnPaused();
			disableResurrectMenu();
			GamePlayer.SharedInstance.ResetCoinCountForBonus();
			return;
		}
		
		notify.Debug ("OnResurrect: Could not find safe spot, going to die");
		OnDiePostGame();
		
	}
	
	public void StartResurrectTimer() {
		notify.Debug ("StartResurrectTimer 1");
		if(resurrectProgressBar == null)
			return;
		notify.Debug ("StartResurrectTimer 2");
		chooseToResurrect = false;
		hide();
		
		enableResurrectMenu();
		if(resurrectProgressBar != null) {
			notify.Debug ("StartResurrectTimer 3");
			UISlider slider = resurrectProgressBar.GetComponent<UISlider>() as UISlider;
			if(slider != null) {
				notify.Debug( "ResurrectTimer is not null");
				slider.sliderValue = 1.0f;
				TweenSlider tweenSlider = TweenSlider.Begin(resurrectProgressBar.gameObject, 2.0f, 0.0f);
				tweenSlider.eventReceiver = this.gameObject;
				tweenSlider.callWhenFinished = "OnDiePostGame";
				tweenSlider.method = UITweener.Method.Linear;
				return;
			}
		}
		
		//-- Catch all for null parameters, go straight to post game.
		notify.Debug ("StartResurrectTimer had some null params, going straight to post game");
		OnDiePostGame();
	}
	
	private void hide() {
		Camera cam = GetComponentInChildren<Camera>();
		if(cam != null) {
			cam.enabled = false;	
		}
	}
	
	private void show() {
		Camera cam = GetComponentInChildren<Camera>();
		if(cam != null) {
			cam.enabled = true;	
		}
	}
	
//	static public void SetObjectiveDataWithFilter(ObjectiveFilterType filter) {
//		
//		int totalPowerups = GameController.SharedInstance.collectedBonusItemPerRun[(int)BonusItem.BonusItemType.Boost];
//		totalPowerups += GameController.SharedInstance.collectedBonusItemPerRun[(int)BonusItem.BonusItemType.Vacuum];
//		
//		GameProfile.SharedInstance.Player.AddObjectiveStat(ObjectiveType.CollectCoins, ObjectiveTimeType.PerRun, filter, GamePlayer.SharedInstance.CoinCountTotal);
//		GameProfile.SharedInstance.Player.AddObjectiveStat(ObjectiveType.CollectCoins, ObjectiveTimeType.OverTime, filter, GamePlayer.SharedInstance.CoinCountTotal);	
//		GameProfile.SharedInstance.Player.AddObjectiveStat(ObjectiveType.CollectSpecialCurrency, ObjectiveTimeType.PerRun, filter, GamePlayer.SharedInstance.GemCountTotal);
//		GameProfile.SharedInstance.Player.AddObjectiveStat(ObjectiveType.CollectSpecialCurrency, ObjectiveTimeType.OverTime, filter, GamePlayer.SharedInstance.GemCountTotal);
//		GameProfile.SharedInstance.Player.AddObjectiveStat(ObjectiveType.Distance, ObjectiveTimeType.PerRun, filter, (int)GameController.SharedInstance.DistanceTraveled);
//		GameProfile.SharedInstance.Player.AddObjectiveStat(ObjectiveType.Distance, ObjectiveTimeType.OverTime, filter, (int)GameController.SharedInstance.DistanceTraveled);
//		GameProfile.SharedInstance.Player.AddObjectiveStat(ObjectiveType.Score, ObjectiveTimeType.PerRun, filter, GamePlayer.SharedInstance.Score);
//		GameProfile.SharedInstance.Player.AddObjectiveStat(ObjectiveType.Score, ObjectiveTimeType.OverTime, filter, GamePlayer.SharedInstance.Score);
//		GameProfile.SharedInstance.Player.AddObjectiveStat(ObjectiveType.CollectPowerups, ObjectiveTimeType.PerRun, filter, totalPowerups);
//		GameProfile.SharedInstance.Player.AddObjectiveStat(ObjectiveType.CollectPowerups, ObjectiveTimeType.OverTime, filter, totalPowerups);
//		GameProfile.SharedInstance.Player.AddObjectiveStat(ObjectiveType.Resurrects, ObjectiveTimeType.PerRun, filter, GameController.SharedInstance.ResurrectsThisRun);
//		GameProfile.SharedInstance.Player.AddObjectiveStat(ObjectiveType.Resurrects, ObjectiveTimeType.OverTime, filter, GameController.SharedInstance.ResurrectsThisRun);
//		//GameProfile.SharedInstance.Player.AddObjectiveStat(ObjectiveType.HeadStarts, ObjectiveTimeType.PerRun, filter, GameController.SharedInstance.HeadStartsThisRun);
//		//GameProfile.SharedInstance.Player.AddObjectiveStat(ObjectiveType.HeadStarts, ObjectiveTimeType.OverTime, filter, GameController.SharedInstance.HeadStartsThisRun);
//	}
	
	public void OnDiePostGame() {
		notify.Debug ("OnDiePostGame1");
		if(chooseToResurrect == true)
			return;
		
		//FlurryBinding.endTimedEvent("Run");
		// TODO: Send a "Run ended" event with elapsed time

		
		
		disableResurrectMenu();
		
		if(postGameVC != null) {
			show();
			disappear(true);

			//-- Capture the current rank
			int preRunRank = GameProfile.SharedInstance.Player.GetCurrentRank();
			float preRunRankProgress = GameProfile.SharedInstance.Player.GetCurrentRankProgress();
			postGameVC.SetRank(preRunRank);
			postGameVC.SetRankProgress(preRunRankProgress);
			
			//All of this is calculated elsewhere now.
//			ObjectiveFilterType filter = ObjectiveFilterType.None;
//			if(GamePlayer.SharedInstance.CoinCountTotal == 0) {
//				filter = ObjectiveFilterType.WithoutCoins;
//				SetObjectiveDataWithFilter(filter);
//			}
//			
//			int totalPowerups = GameController.SharedInstance.collectedBonusItemPerRun[(int)BonusItem.BonusItemType.Boost];
//			totalPowerups += GameController.SharedInstance.collectedBonusItemPerRun[(int)BonusItem.BonusItemType.Vacuum];
//			
//			if(totalPowerups == 0) {
//				filter = ObjectiveFilterType.WithoutPowerups;
//				SetObjectiveDataWithFilter(filter);
//			}
//			
//			if(GameController.SharedInstance.StumblesThisRun == 0) {
//				filter = ObjectiveFilterType.WithoutStumble;
//				SetObjectiveDataWithFilter(filter);
//			}
//			
//			SetObjectiveDataWithFilter(ObjectiveFilterType.None);
			
			postGameVC.FillInObjectiveData();
			postGameVC.DidComputeObjectives = false;
			//-- This will save the player state to disk.
			postGameVC.ComputeCompletedObjectives();
			
			postGameVC.SetDistance((int)GameController.SharedInstance.DistanceTraveled);
			postGameVC.SetScore(GamePlayer.SharedInstance.Score);
			postGameVC.SetCoinScore(GamePlayer.SharedInstance.CoinCountTotal);
			int scoreMultiplier = GameProfile.SharedInstance.GetScoreMultiplier();
			postGameVC.SetScoreMultiplier(scoreMultiplier);
			
			postGameVC.DeathMessageLabel.text = "When you don't succeed, try, try again!";
			postGameVC.DeathPortrait.spriteName = "Eaten";
			int activeCharacterID = GameProfile.SharedInstance.GetActiveCharacter().characterId;
			
			ProtoDeathMessage dm = DeathMessage.GetRandomMessageForDeathType(GamePlayer.SharedInstance.DeathType);
			if(dm != null && dm.messageChoices != null && dm.messageChoices.Count > 0) {
				//-- pick a random death message
				if(postGameVC.DeathPortrait.atlas.GetSprite(dm.spriteName) != null) {
					postGameVC.DeathMessageLabel.text = dm.messageChoices[UnityEngine.Random.Range(0, dm.messageChoices.Count)];	
					postGameVC.DeathPortrait.spriteName = dm.spriteName;
				}
				else {
					string characterSpriteName = dm.getCharacterSpriteName(activeCharacterID);
					if(postGameVC.DeathPortrait.atlas.GetSprite(characterSpriteName) != null) {
						postGameVC.DeathMessageLabel.text = dm.getRandomCharacterMessage(activeCharacterID);	
						postGameVC.DeathPortrait.spriteName = characterSpriteName;
					}
				}
			}
			
#if UNITY_IPHONE
			GameCenterBinding.reportScore((System.Int64)GamePlayer.SharedInstance.Score, GameController.Leaderboard_HighScores);
			GameCenterBinding.reportScore((System.Int64)((int)GameController.SharedInstance.DistanceTraveled), GameController.Leaderboard_DistanceRun);

#elif UNITY_ANDROID
			//-- game circle
 
#endif			
			
			
//			Dictionary<string,string> dict = new Dictionary<string,string>();
//			int temp = GamePlayer.SharedInstance.Score;
//			temp = temp % 100;
//			dict.Add( "score", temp.ToString() );
//			temp = GamePlayer.SharedInstance.CoinCountTotal;
//			temp %= 10;
//			dict.Add( "coinsPerRun", temp.ToString() );
//			dict.Add( "gemsPerRun", GamePlayer.SharedInstance.GemCountTotal.ToString() );
//			dict.Add( "Multiplier", scoreMultiplier.ToString() );
//			temp = (int)GameController.SharedInstance.DistanceTraveled;
//			temp %= 100;
//			dict.Add( "DistancePerRun", temp.ToString() );
//			dict.Add( "DeathType", GamePlayer.SharedInstance.DeathType.ToString() );
//			dict.Add( "Level", GameProfile.SharedInstance.Player.GetCurrentRank().ToString() );
//			dict.Add( "Toon", GameProfile.SharedInstance.GetActiveCharacter().displayName );
//			
//			if(GameProfile.SharedInstance.GetActiveCharacter().powerID >= 0) {
//				dict.Add( "EquippedPower", PowerStore.PowerFromID(GameProfile.SharedInstance.GetActiveCharacter().powerID).Title );
//			}
//			if(GameProfile.SharedInstance.Player.objectivesEarned != null) {
//				dict.Add( "ObjectivesEarned", GameProfile.SharedInstance.Player.objectivesEarned.Count.ToString() );	
//			}
//			
//			FlurryBinding.logEventWithParameters( "EndOfRun", dict, false );

			
			
			//postGameVC.appear();
		}
	}
	
	
	public override void OnNeedMoreGemsNo() {
		base.OnNeedMoreGemsNo();
		chooseToResurrect = false;
		OnDiePostGame();
	}
	
	public override void OnNeedMoreGemsYes() {
		UIConfirmDialog.onNegativeResponse -= OnNeedMoreGemsNo;
		UIConfirmDialog.onPositiveResponse -= OnNeedMoreGemsYes;
//		notify.Debug ("FAKE BUY IAP FOR 100 Gems");
//		GameProfile.SharedInstance.Player.specialCurrencyCount += 100;
//		updateCurrency();
		if(UIManager.SharedInstance.IAPStoreVC != null) {
			UIManager.SharedInstance.IAPStoreVC.previousViewController = this;
			disappear(true);
			UIManager.SharedInstance.IAPStoreVC.appear();
		}
		else {
			chooseToResurrect = false;
			OnDiePostGame();
		}
	}
	
	public void OnMegaHeadStart() { 
		notify.Debug ("MEGAHEADSTART");
		//-- Charge the player
		GameProfile.SharedInstance.Player.coinCount -= GameProfile.SharedInstance.Player.GetMegaHeadStartCost();
		
		GamePlayer.SharedInstance.StartBoost(2500.0f);
		GameController.SharedInstance.HeadStartsThisRun++;
		
		if(headStartRoot != null) {
			NGUITools.SetActive(headStartRoot.gameObject, false);	
		}
		if(megaHeadStartRoot != null) {
			NGUITools.SetActive(megaHeadStartRoot.gameObject, false);	
		}
	}
	
	public void OnHeadStart() {
		notify.Debug ("HEADSTART");
		
		//-- Charge the player
		GameProfile.SharedInstance.Player.coinCount -= GameProfile.SharedInstance.Player.GetHeadStartCost();
		GamePlayer.SharedInstance.StartBoost(1000.0f);
		GameController.SharedInstance.HeadStartsThisRun++;
		
		if(headStartRoot != null) {
			NGUITools.SetActive(headStartRoot.gameObject, false);	
		}
		if(megaHeadStartRoot != null) {
			NGUITools.SetActive(megaHeadStartRoot.gameObject, false);	
		}
	}
	
	private void turnOffHeadStartBlink() {
		if(megaHeadStartWidgets != null) {
			int max = megaHeadStartWidgets.Length;
			for (int i = 0; i < max; i++) {
				UIWidget widget = megaHeadStartWidgets[i];
				if(widget == null)
					continue;
				
				TweenColor tc = widget.GetComponent<TweenColor>();
				if(tc != null) {
					tc.enabled = false;
				}
				widget.color = Color.white;
			}
		}
		if(headStartWidgets != null) {
			int max = headStartWidgets.Length;
			for (int i = 0; i < max; i++) {
				UIWidget widget = headStartWidgets[i];
				if(widget == null)
					continue;
				
				TweenColor tc = widget.GetComponent<TweenColor>();
				if(tc != null) {
					tc.enabled = false;
				}
				widget.color = Color.white;
			}
		}
	}
	
	// Update is called once per frame
	private UIWidget[] headStartWidgets = null;
	private UIWidget[] megaHeadStartWidgets = null;
	void Update () {
		
		switch(Time.frameCount%3)
		{
		case 0:
			if(_score != _lastScore)
			{
				ScoreLabel.text = _score.ToString();
				if((_score == 0)||((int)Math.Floor(Math.Log10(_score)) != (int)Math.Floor(Math.Log10(_lastScore))))
				{
					respositionCurrencyBars();
				}
				_lastScore = _score;
			}
			break;
		case 1:
			if(_coinCount != _lastCoinCount)
			{
				CoinLabel.text = _coinCount.ToString();
				_lastCoinCount = _coinCount;
			}
			break;
		case 2:
			break;
		}
		
		int dist = (int)GameController.SharedInstance.DistanceTraveled;
		if (dist < EarlyToLateDistance && dist >= MessageBoardLastDistance + EarlyMessageBoardSpan) {
			ShowDistanceMeterWithDistance(MessageBoardLastDistance + EarlyMessageBoardSpan);
			MessageBoardLastDistance = MessageBoardLastDistance + EarlyMessageBoardSpan;
		}
		else if (dist >= MessageBoardLastDistance + LateMessageBoardSpan) {
			ShowDistanceMeterWithDistance(MessageBoardLastDistance + LateMessageBoardSpan);
			MessageBoardLastDistance = MessageBoardLastDistance + LateMessageBoardSpan;
		}
		
		//-- Headstart show/hide
		if(GameController.SharedInstance.IsIntroScene == true || GameController.SharedInstance.TimeSinceGameStart > 10.0f || GameController.SharedInstance.IsInCountdown == true)
		{
			NGUITools.SetActive(headStartRoot.gameObject, false);
			NGUITools.SetActive(megaHeadStartRoot.gameObject, false);
			return;
		}
			
		if(firstUpdate == true) {
			//-- If we have enough money for headstart and megaheadstart, show and blink them.
			firstUpdate = false;
			blinking = true;
			if(GamePlayer.SharedInstance.HasBoost == false) {
				blinking = false;
				bool canAffordHeadStart = GameProfile.SharedInstance.Player.CanAffordHeadStart(false);
				bool canAffordMegaHeadStart = GameProfile.SharedInstance.Player.CanAffordHeadStart(true);
				
				if(headStartRoot != null) {
					NGUITools.SetActive(headStartRoot.gameObject, canAffordHeadStart);
					
					if(canAffordMegaHeadStart) {
						headStartRoot.localPosition = startingHeadStartPosition;
					}
					else {
						headStartRoot.localPosition = new Vector3(0, startingHeadStartPosition.y, startingHeadStartPosition.z);
					}
				}
				if(megaHeadStartRoot != null) {
					NGUITools.SetActive(megaHeadStartRoot.gameObject, canAffordMegaHeadStart);
				}
				turnOffHeadStartBlink();	
			}
			
		}
		else if(GameController.SharedInstance.TimeSinceGameStart > 6.0f && blinking == false) {
			blinking = true;
			if(NGUITools.GetActive(headStartRoot.gameObject)) {
				if(headStartWidgets == null) {
					headStartWidgets = headStartRoot.GetComponentsInChildren<UIWidget>();
				}
				if(headStartWidgets != null) {
					int max = headStartWidgets.Length;
					for (int i = 0; i < max; i++) {
						UIWidget widget = headStartWidgets[i];
						if(widget == null)
							continue;
						TweenColor tc = TweenColor.Begin(widget.gameObject, 0.25f, new Color(255,255,255,0));
						if(tc != null) {
							tc.style = UITweener.Style.PingPong;
						}
					}
				}
			}
			
			if(NGUITools.GetActive(megaHeadStartRoot.gameObject)) {
				if(megaHeadStartWidgets == null) {
					megaHeadStartWidgets = megaHeadStartRoot.GetComponentsInChildren<UIWidget>();
				}
				if(megaHeadStartWidgets != null) {
					int max = megaHeadStartWidgets.Length;
					for (int i = 0; i < max; i++) {
						UIWidget widget = megaHeadStartWidgets[i];
						if(widget == null)
							continue;
						TweenColor tc = TweenColor.Begin(widget.gameObject, 0.25f, new Color(255,255,255,0));
						if(tc != null) {
							tc.style = UITweener.Style.PingPong;
						}
					}
				}
			}
		}
	}
}

*/
