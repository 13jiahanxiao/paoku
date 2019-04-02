using UnityEngine;
using System.Collections;

public class ResurrectMenu : MonoBehaviour 
{
	protected static Notify notify;
	public UIInGameViewControllerOz inGameVC;
	
	public Transform resurrectMenu;
	public Transform resurrectProgressBar;
	public UILabel resurrectionCostLabel;	
	public UILabel MessageLabel;
	public bool chooseToResurrect = false;
		
	//private bool continueButtonIsNowCancelButton = false;	// for canceling purchase of gems in mini store during resurrect menu
	
	void OnEnable()
	{
		// Listen to all events for illustration purposes
//		GoogleIABManager.notifyToResurrectByRMBEvent += notifyToResurrectByRMBEvent;
//		GoogleIABManager.notifyToResurrectByGEMEvent += notifyToResurrectByGEMEvent;
//		GoogleIABManager.notifyToReSumeGameEvent += notifyToReSumeGameEvent;
	}
	
	void OnDisable()
	{
		// Remove all event handlers
//		GoogleIABManager.notifyToResurrectByRMBEvent -= notifyToResurrectByRMBEvent;
//	
//		GoogleIABManager.notifyToResurrectByGEMEvent -= notifyToResurrectByGEMEvent;
//		GoogleIABManager.notifyToReSumeGameEvent -= notifyToReSumeGameEvent;
	}
	
	void notifyToResurrectByRMBEvent(){
		if (GamePlayer.SharedInstance.MovePlayerToNextSafeSpot() == true) 
		{
			//-- Slow down current speed
			//TR.LOG("DeathRunVelocity " + GamePlayer.SharedInstance.DeathRunVelocity + " getRunVelocity() " + GamePlayer.SharedInstance.getRunVelocity());
			//GamePlayer.SharedInstance.SetPlayerVelocity(GamePlayer.SharedInstance.DeathRunVelocity*0.5f);
			//GamePlayer.SharedInstance.SetPlayerVelocity( GamePlayer.SharedInstance.getRunVelocity() );
			//TR.LOG ("Resurrect speed set to {0}, Previous speed was {1}", GamePlayer.SharedInstance.PlayerMagnitude, GamePlayer.SharedInstance.DeathRunVelocity);
			
			if (GamePlayer.SharedInstance.PlayerMagnitude > GamePlayer.SharedInstance.getModfiedMaxRunVelocity()) 
				Debug.Break(); 
			
			GamePlayer.SharedInstance.Resurrect();	//-- give invulnerable for a few seconds.
			
			GameController.SharedInstance.IsHandlingEndGame = false;
			GameController.SharedInstance.IsGameOver = false;

			// reset coin meter
			inGameVC.coinMeter.FadePowerGlow();
			GamePlayer.SharedInstance.ResetCoinCountForBonus();
			
			// go back to game
			hideResurrectMenu();	//disableResurrectMenu();
			inGameVC.OnPause();
			inGameVC.OnUnPaused();
		}
		else
		{
			
			notify.Warning ("OnResurrect: Could not find safe spot, going to die");
			inGameVC.OnDiePostGame();
		}
		
	}
	
	void notifyToResurrectByGEMEvent(){
		OnResurrect();
		
	}
	
	void notifyToReSumeGameEvent(){
		notify.Debug("unity notifyToReSumeGameEvent call!!!");
		chooseToResurrect = false;
		inGameVC.OnDiePostGame();
		
	}
	
	
	void Awake() 
	{
		notify = new Notify(this.GetType().Name);
		notify.Debug("unity Awake call!!!");
		gameObject.AddComponent<UIPanelAlpha>();		
	}
	
	public void hideResurrectMenu()
	{
		//resurrectMenu.gameObject.AddComponent<FadeAlphaResetToZero>();
		NGUITools.SetActive(resurrectMenu.gameObject, false);
	}	

	public void enableResurrectMenu() 
	{
		//TR.LOG("enableResurrectMenu");
		
		chooseToResurrect = false;
		
		NGUITools.SetActive(resurrectMenu.gameObject, true);
		//resurrectMenu.gameObject.GetComponent<UIPanelAlpha>().alpha = 0f;
		//TweenAlpha.Begin(resurrectMenu.gameObject, 0.15f, 1f);
		
//		FadePanel fp = resurrectMenu.gameObject.AddComponent<FadePanel>();
//		fp.SetParameters(true, 0.15f);
		//ResetContinueButtonAlpha();
		
		//NGUITools.SetActive(statsRoot.gameObject, true); 
		//UIManagerOz.SharedInstance.inGameVC.statRoot.StartAnimSeq();
		resurrectionCostLabel.text = GameProfile.SharedInstance.GetResurrectionCost().ToString();
		
		inGameVC.HidePauseButton();			// hide everything else
		inGameVC.coinMeter.FadePowerGlow();
		
		SetContinueButtonStatus(true);	//ResetContinueButton();			
	}

	public void OnContinueButtonClick()	
	{
		if (!chooseToResurrect)
			OnResurrect();						// go ahead with resurrection process
	}
	
	public void SetContinueButtonStatus(bool active)
	{
		resurrectMenu.Find("Button").gameObject.active = active;
//		resurrectMenu.Find("Button").Find("MessageLabel").GetComponent<UILabel>().enabled = active;
//		resurrectMenu.Find("Button").Find("CostLabel").GetComponent<UILabel>().enabled = active;
//		resurrectMenu.Find("Button").Find("CoinDisplayIcon").GetComponent<UISprite>().enabled = active;
//		resurrectMenu.Find("Button").Find("Background").GetComponent<UISprite>().enabled = active;
//		resurrectMenu.Find("Button").Find("SlicedSprite (_bottombutton_highlight)").GetComponent<UISprite>().enabled = active;
//		resurrectMenu.Find("Button").Find("SlicedSprite (_bottombutton_frame)").GetComponent<UISprite>().enabled = active;		
//		resurrectMenu.Find("Button").GetComponent<BoxCollider>().enabled = active;
		
		resurrectMenu.Find("Progress Bar").Find("Background").GetComponent<UISprite>().enabled = active;
		resurrectMenu.Find("Progress Bar").Find("Foreground").GetComponent<UIFilledSprite>().enabled = active;	
	}		
	
	public void OnBackButtonClick()		
	{
		SetContinueButtonStatus(true);
		StartResurrectTimer();	//sliderValue);
	}		
	
	public void OnResurrect()
	{
		
		chooseToResurrect = true;
		//TR.LOG ("OnResurrect");
		//-- Can afford the res?
		
		//-- Turn off the countdown.
		TweenSlider ts = resurrectProgressBar.GetComponent<TweenSlider>();
		ts.enabled = false;	
		
		AudioManager.SharedInstance.StopFX(true);	
		
		if (GameProfile.SharedInstance.Player.CanAffordResurrect() == false) 
		{			
			//UIConfirmDialogOz.onNegativeResponse += OnNeedMoreGemsNoInResurrect;
			//UIConfirmDialogOz.onPositiveResponse += OnNeedMoreGemsYesInResurrect;
			//UIManagerOz.SharedInstance.confirmDialog.ShowConfirmDialog("Lbl_Dialogue_MoreGems_Prompt","Lbl_Dialogue_MoreGems_Confirm", "Btn_No", "Btn_Yes");
			//UIManagerOz.SharedInstance.confirmDialog.ShowConfirmDialog("Lbl_Dialogue_MoreGems_Prompt", "Btn_No", "Btn_Yes");
			SetContinueButtonStatus(false);	//TurnContinueButtonIntoCancelButton();
			UIManagerOz.SharedInstance.GoToMiniStore(ShopScreenName.Gems, true);	// "gems");	// send player to in-game mini store, gems page
		//	PurchaseUtil.notifyToShowResurrectDialog();
			return;	
		}
	
		// hide store, if open
		if (UIManagerOz.SharedInstance.IAPMiniStoreVC.gameObject.active)	// != null)
			UIManagerOz.SharedInstance.IAPMiniStoreVC.disappear();	
		
		//TR.LOG ("OnResurrect: spending gems");
		
		//If we don't have enough coins in the pool, make sure we take from the current-run gem count
		int curcost = GameProfile.SharedInstance.GetResurrectionCost();
		if(curcost > GameProfile.SharedInstance.Player.specialCurrencyCount)
		{
			curcost -= GameProfile.SharedInstance.Player.specialCurrencyCount;
			GameProfile.SharedInstance.Player.specialCurrencyCount = 0;
			GamePlayer.SharedInstance.AddGemsToScore(-curcost);
		}
		else
		{
			GameProfile.SharedInstance.Player.specialCurrencyCount -= curcost;
		}
//		PurchaseUtil.bIAnalysisWithParam("Player_Gems","Consume_Gems_Amount|"+curcost);
		// --- Analytics ------------------------------------------------------
//		AnalyticsInterface.LogInAppCurrencyActionEvent( CostType.Special, 0 - GameProfile.SharedInstance.GetResurrectionCost(), "save_me", "", 0, "", GameProfile.GetAreaCharacterString() );
		
		string causeOfDeathStr = "other";
			DeathTypes deathType = GamePlayer.SharedInstance.DeathType;
			switch ( deathType )
			{
				case DeathTypes.Fall:
					causeOfDeathStr = "fall";
					break;
				case DeathTypes.Baboon:
					causeOfDeathStr = "baboon";
					break;
				case DeathTypes.SceneryRock:
					causeOfDeathStr = "wall";
					break;
			}
		
//		AnalyticsInterface.LogGameAction( "save_me", "used", causeOfDeathStr, GameController.SharedInstance.DistanceTraveled.ToString(), 0 );
//		AnalyticsInterface.LogGameAction( "save_me", "used", GameProfile.GetAreaCharacterString(), GamePlayer.SharedInstance.Score.ToString(), 0);
//		// --------------------------------------------------------------------
		
		GameProfile.SharedInstance.Serialize();
		GameController.SharedInstance.ResurrectionCost *= 2;
		//TR.LOG ("OnResurrect: new res cost {0}", GameController.SharedInstance.ResurrectionCost);
		GameController.SharedInstance.IsResurrecting = true;
		
		
		
		//-- Find safe spot
		if (GamePlayer.SharedInstance.MovePlayerToNextSafeSpot() == true) 
		{
			//-- Slow down current speed
			//TR.LOG("DeathRunVelocity " + GamePlayer.SharedInstance.DeathRunVelocity + " getRunVelocity() " + GamePlayer.SharedInstance.getRunVelocity());
			//GamePlayer.SharedInstance.SetPlayerVelocity(GamePlayer.SharedInstance.DeathRunVelocity*0.5f);
			//GamePlayer.SharedInstance.SetPlayerVelocity( GamePlayer.SharedInstance.getRunVelocity() );
			//TR.LOG ("Resurrect speed set to {0}, Previous speed was {1}", GamePlayer.SharedInstance.PlayerMagnitude, GamePlayer.SharedInstance.DeathRunVelocity);
			
			if (GamePlayer.SharedInstance.PlayerMagnitude > GamePlayer.SharedInstance.getModfiedMaxRunVelocity()) 
				Debug.Break(); 
			
			GamePlayer.SharedInstance.Resurrect();	//-- give invulnerable for a few seconds.
			
			GameController.SharedInstance.IsHandlingEndGame = false;
			GameController.SharedInstance.IsGameOver = false;

			// reset coin meter
			inGameVC.coinMeter.FadePowerGlow();
			GamePlayer.SharedInstance.ResetCoinCountForBonus();
			
			// go back to game
			hideResurrectMenu();	//disableResurrectMenu();
			inGameVC.OnPause();
			inGameVC.OnUnPaused();		
		}
		else
		{
			notify.Warning ("OnResurrect: Could not find safe spot, going to die");
			inGameVC.OnDiePostGame();
		}
	}	
	

	
	public void StartResurrectTimer(float startTimerValue = 3.0f)
	{
		// wxj, debug when dead
		//notifyToReSumeGameEvent();
		//return;
		
		//TR.LOG ("StartResurrectTimer 1");
		if (resurrectProgressBar == null)
		{
			//TR.LOG ("StartResurrectTimer had some null params, going straight to post game");
			inGameVC.OnDiePostGame();
			return;
		}
		//TR.LOG ("StartResurrectTimer 2");
		chooseToResurrect = false;
		// ee temporarily disable hiding the ingame gui so we can see the resurrect panel.
		// TR2 has the resurrectPanel in postGameGui but we don't. so we can hide the rest of our inGameGui and just keep the resurrect on
		//hide();
		
		//AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_UI_gemMeter);
		
		enableResurrectMenu();
		//statsRoot.StartAnimSeq();
		//statsRoot.EnterStatsPage();
		
		int curcost = GameProfile.SharedInstance.GetResurrectionCost();
		if (GameProfile.SharedInstance.Player.CanAffordResurrect() == false) 
		{			
			curcost=0;
		}
		float currentDistance= GameController.SharedInstance.DifficultyDistanceTraveled;
		PlayerStats playerStats = GameProfile.SharedInstance.Player;
		float bestDistance = playerStats.bestDistanceScore;
		int difference = 0;
		int _difference = 0;
		if((bestDistance-currentDistance)<500)
		{
			difference = (int)(bestDistance-currentDistance);		
		
		}
		if(bestDistance<currentDistance)
		{
			_difference = (int)(currentDistance-bestDistance);		
		
		}
//		PurchaseUtil.notifyToShowResurrectDialog(difference,_difference,curcost);
		UnityEngine.Debug.Log("fuck");	
		
	
		//TR.LOG ("StartResurrectTimer 3");
		UISlider slider = resurrectProgressBar.GetComponent<UISlider>() as UISlider;
		slider.sliderValue = startTimerValue;	//1.0f;
		TweenSlider tweenSlider = TweenSlider.Begin(resurrectProgressBar.gameObject, 2.0f, 0.0f);
		tweenSlider.eventReceiver = inGameVC.gameObject;	//this.gameObject;
	//	tweenSlider.callWhenFinished = "OnDiePostGame";
		tweenSlider.method = UITweener.Method.Linear;
		     
		// wxj
//		notifyToReSumeGameEvent();
		
		return;
	}	
}

//	public void OnNeedMoreGemsNoInResurrect() 	// use in-game only, when choosing to not go to mini store.  Used on continue/resurrect screen.
//	{
//		//UIConfirmDialogOz.ClearEventHandlers();
//		UIConfirmDialogOz.onNegativeResponse -= OnNeedMoreGemsNoInResurrect;
//		UIConfirmDialogOz.onPositiveResponse -= OnNeedMoreGemsYesInResurrect;
//		
//		//if you don't want any gems, just die. no resurrection for you.
//		UIManagerOz.SharedInstance.inGameVC.resurrectMenu.chooseToResurrect = false;
//	 	UIManagerOz.SharedInstance.inGameVC.OnDiePostGame();
//	}
//	
//	public void OnNeedMoreGemsYesInResurrect() 	// use in-game only, goes to mini store.  Used on continue/resurrect screen.
//	{
//		//UIConfirmDialogOz.ClearEventHandlers();
//		UIConfirmDialogOz.onNegativeResponse -= OnNeedMoreGemsNoInResurrect;
//		UIConfirmDialogOz.onPositiveResponse -= OnNeedMoreGemsYesInResurrect;
//		
//		UIManagerOz.SharedInstance.GoToMiniStore(ShopScreenName.Gems, true);	// "gems");	// send player to in-game mini store, gems page
//	}		



	
//	public void disableResurrectMenu()
//	{
//		TweenAlpha.Begin(resurrectMenu.gameObject, 0.3f, 0f);
////		FadePanel fp = resurrectMenu.gameObject.AddComponent<FadePanel>();
////		fp.SetParameters(false, 0.3f);
//		//NGUITools.SetActive(resurrectMenu.gameObject, false);
//	}
	


//	public void TurnContinueButtonIntoCancelButton()
//	{
//		continueButtonIsNowCancelButton = true;
//		resurrectMenu.Find("Button").Find("MessageLabel").GetComponent<UILabel>().text = "Don't continue run...";
//		resurrectMenu.Find("Button").Find("CostLabel").GetComponent<UILabel>().enabled = false;
//		resurrectMenu.Find("Button").Find("CoinDisplayIcon").GetComponent<UISprite>().enabled = false;
//	}
//	
//	public void ResetContinueButton()
//	{
//		continueButtonIsNowCancelButton = false;
//		resurrectMenu.Find("Button").Find("MessageLabel").GetComponent<UILabel>().text = "Continue run?";
//		resurrectMenu.Find("Button").Find("CostLabel").GetComponent<UILabel>().enabled = true;
//		resurrectMenu.Find("Button").Find("CoinDisplayIcon").GetComponent<UISprite>().enabled = true;
//	}	
	
//	public void ResetContinueButtonAlpha()
//	{
//		resurrectMenu.Find("Button").Find("MessageLabel").GetComponent<UILabel>().color = Color.white;
//		resurrectMenu.Find("Button").Find("CostLabel").GetComponent<UILabel>().color = Color.white;
//		resurrectMenu.Find("Button").Find("CoinDisplayIcon").GetComponent<UISprite>().color = Color.white;
//		resurrectMenu.Find("Button").Find("Background").GetComponent<UISprite>().color = Color.white;
//	
//		resurrectMenu.Find("Progress Bar").Find("Background").GetComponent<UISprite>().color = Color.white;
//		resurrectMenu.Find("Progress Bar").Find("Foreground").GetComponent<UIFilledSprite>().color = Color.white;
//	}		
	
//	public void OnContinueButtonClick()	
//	{
//		if (continueButtonIsNowCancelButton)	// in mini store, gems screen, continue button becomes cancel button
//		{
//			//if you don't want any gems, just die. no resurrection for you.
//			UIManagerOz.SharedInstance.inGameVC.resurrectMenu.chooseToResurrect = false;
//	 		UIManagerOz.SharedInstance.inGameVC.OnDiePostGame();
//			
//			//inGameVC.paperViewController.IAPMiniStoreVC.GetCurrentStoreList().CancelContinue();		// cancel continue, killing the active store and confirm dialog if showing
//		}
//		else
//			OnResurrect();						// go ahead with resurrection process
//	}
	
//	public void OnBackButtonClick()		
//	{
		//SetContinueButtonStatus(true);
		//OnResurrect();	//StartResurrectTimer(sliderValue);
		
//		SetContinueButtonStatus(true);
//		StartResurrectTimer(1.0f);	//sliderValue);
		
		//if you don't want any gems, just die. no resurrection for you.
		//UIManagerOz.SharedInstance.inGameVC.resurrectMenu.chooseToResurrect = false;
	 	//UIManagerOz.SharedInstance.inGameVC.OnDiePostGame();
		//UIManagerOz.SharedInstance.PaperVC.ShowPaperVC();
//	}
	



			//return;
		
//inGameVC.ShowInGameUI();

		//-- Catch all for null parameters, go straight to post game.
		//TR.LOG ("StartResurrectTimer had some null params, going straight to post game");
		//inGameVC.OnDiePostGame();

		//TR.LOG("enableResurrectMenu with cost {0}", resurrectionCostLabel.text);
		//inGameVC.HidePauseMenu();

//		NGUITools.SetActive(pauseSprite.gameObject, false);
//		NGUITools.SetActive(pauseClickedSprite.gameObject, false);
//		NGUITools.SetActive(pauseLabel.gameObject, false);
//		NGUITools.SetActive(pauseClickedLabel.gameObject, false);
