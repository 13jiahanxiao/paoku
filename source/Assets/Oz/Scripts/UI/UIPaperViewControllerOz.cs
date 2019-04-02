using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class UIPaperViewControllerOz : UIViewControllerOz	//MonoBehaviour
{
	public GameObject PlayButtonRoot;	
	public GameObject PageNameTitle; 
	
	public UILabel CoinLabel;	
	public UILabel GemLabel; 
	
	public UISprite BurstlySprite;		
	public UILabel BurstlyLabel; 	
	public BoxCollider BurstlyCollider;
	
	public UIViewControllerOz previousViewController;
	public UIViewControllerOz currentViewController;
	
	#pragma warning disable
	private UIIdolMenuViewControllerOz idolVC;
	private UIMainMenuViewControllerOz mainVC;	
	private UIInventoryViewControllerOz inventoryVC;
	private UILeaderboardViewControllerOz leaderboardVC;
	private UIObjectivesViewControllerOz ObjectivesVC;	
	//private UIIAPViewControllerOz IAPStoreVC;
	//private UIIAPMiniViewControllerOz IAPMiniStoreVC;	
	//private UIMoreGamesViewControllerOz moreGamesVC;
	private UISettingsViewControllerOz settingsVC;
	private UIStatViewControllerOz statsVC;	
	private UIPostGameViewControllerOz postVC;
	private UIWorldOfOzViewControllerOz worldOfOzVC;
	#pragma warning restore
	
	private AppCounters appCounters;
//	private NotificationSystem notificationSystem;	
	
	public bool goBackToIdolMenu = true;
	private string analyticsCurrentPageName;
	private string analyticsPreviousPageName;
	
	
	void OnEnable(){
		
//		GoogleIABManager.notifyToPurchaseSuperConumbleEvent += purchaseSuperConsumableSuccess;
//		GoogleIABManager.notifyToPurchaseChinaGirlEvent += purchaseChinaGirlSuccess;
//		// wxj, exchange onekey product
//		GoogleIABManager.artifactPurchaseSucceedEvent += artifactPurchaseSuccess;artifactPurchaseSuccess
	}
	
	void OnDisable(){
		
		
//		GoogleIABManager.notifyToPurchaseSuperConumbleEvent -= purchaseSuperConsumableSuccess;
//		GoogleIABManager.notifyToPurchaseChinaGirlEvent -= purchaseChinaGirlSuccess;
//		// wxj, exchange onekey product
//		GoogleIABManager.artifactPurchaseSucceedEvent -= artifactPurchaseSuccess;
	}
	
	// wxj, call when exchange onekey artifact purchase succeed
	public void artifactPurchaseSuccess(string pId)
	{
		UIManagerOz.SharedInstance.okayDialog.ShowOkayDialog("Msg_ThankYouPurchase", "Btn_Ok");
		for(int i = 0; i < IAPWrapper.IAPS_ONEKEY_UPG.Length; i++)
		{
			if(IAPWrapper.IAPS_ONEKEY_UPG[i].Equals(pId))
			{
				GameProfile.SharedInstance.Player.PurchaseArtifact(i, true);
				inventoryVC.inventoryPanelGOs[1].GetComponent<UIArtifactsList>().Refresh();
			}
		}
	}
	
	
	public void purchaseChinaGirlSuccess(){
		UIManagerOz.SharedInstance.okayDialog.ShowOkayDialog("Msg_ThankYouPurchase", "Btn_Ok");
		CharacterStats character = GameProfile.SharedInstance.Characters[3];
		character.unlocked = true;
		GameProfile.SharedInstance.Serialize();
		UIManagerOz.SharedInstance.PaperVC.UpdateCurrency();
		worldOfOzVC.worldOfOzPanelGOs[1].GetComponent<UICharacterList>().Refresh();
		
	}
	
	public void purchaseSuperConsumableSuccess(){
		UnityEngine.Debug.LogError("purchaseSuperConsumableSuccess call");
//		PurchaseUtil.bIAnalysisWithParam("Purchase_Consumables","ConsumablesName|Super_Consumable,amount|1");
		PlayerStats playerStats = GameProfile.SharedInstance.Player;		
		playerStats.EarnConsumable(9,6);
		playerStats.EarnConsumable(4,6);
		playerStats.EarnConsumable(7,6);
		playerStats.EarnConsumable(8,6);
		UIManagerOz.SharedInstance.PaperVC.UpdateCurrency();
		UIManagerOz.SharedInstance.okayDialog.ShowOkayDialog("Msg_ThankYouPurchase", "Btn_Ok");
		inventoryVC.inventoryPanelGOs[2].GetComponent<UIConsumablesList>().Refresh();
		//consumableToPurchase.Refresh();
		
		
		
		
	}
	
	
	protected override void Start()	
	{
		base.Start();
		linkupOtherViewControllers();
		appCounters = Services.Get<AppCounters>();
	}
	
	public override void appear()
	{
		base.appear();
		UpdateCurrency();
	}

	public void UpdateCurrency()
	{
		PlayerStats player = GameProfile.SharedInstance.Player;
		CoinLabel.text = player.coinCount.ToString();
		GemLabel.text = player.specialCurrencyCount.ToString();
	}	
	
	public void SetBurstlyButton(bool visible)
	{
		BurstlySprite.enabled = visible;	
		BurstlyLabel.enabled = visible;
		BurstlyCollider.enabled = visible;
	}
	
	public void SetPageName(string title, string subTitle)
	{	
		PageNameTitle.GetComponent<UILocalize>().SetKey(title);
		//PageNameGroup.transform.Find("SubtitleLabel").GetComponent<UILocalize>().SetKey(subTitle); - remove don't need on this page N.N.	
	
		// Store the name of the previous page so we can include the "from" location
		// in the analytics when the "Back" button is pressed
		analyticsPreviousPageName = analyticsCurrentPageName;
		
		analyticsCurrentPageName = title;
		if ( subTitle != "" )
		{
			// Append the subtitle, if any
			analyticsCurrentPageName += "/" + subTitle;
		}
		
		if ( analyticsPreviousPageName == null )
		{
			// "analyticsPreviousPageName" hasn't been set yet, so just set it to the current page name
			analyticsPreviousPageName = analyticsCurrentPageName;
		}
	}
	
	private void OnBurstlyClicked()		// open Burstly panel
	{
//		PurchaseUtil.notifyToShowExChangeDialog();	
	}

	private void OnPlayClicked() 
	{
//		if( DownloadManager.IsDownloadInProgress() )
//		{// don't allow playing if a download is in progress
//			// just bring up downloadng UI
//			UIManagerOz.SharedInstance.StartDownloadPrompts( true, false, true, gameObject);
//			return;
//		}
		
		//		AnalyticsInterface.LogNavigationActionEvent( "Play", "Main Menu", "Game" );
		
		UIManagerOz.SharedInstance.OnPlayClicked();
	}	
	
	public void SetCurrentPage(UIViewControllerOz page)	
	{
		previousViewController = currentViewController;
		currentViewController = page;
	}
	
	public void ShowExitPrompt()
	{
//		PurchaseUtil.exitGame();
		UIConfirmDialogOz.onNegativeResponse += CancelExit;
		UIConfirmDialogOz.onPositiveResponse += ExitGame;
		UIManagerOz.SharedInstance.confirmDialog.ShowConfirmDialog("Msg_LeaveGame", "Btn_No", "Btn_Yes");		
	}	
	
	public void CancelExit()
	{
		UIConfirmDialogOz.onNegativeResponse -= CancelExit;
		UIConfirmDialogOz.onPositiveResponse -= ExitGame;	
		
	}
	
	public void ExitGame()	
	{
		Invoke("Quit", 0.2f);	// wait to prevent screen glitch
	}		
	
	private void Quit()
	{
//		PurchaseUtil.exitGame();
		Application.Quit();
	}
	
	public void OnEscapeButtonClicked()					// Android hardware back button is mapped to this
	{
		if( UIManagerOz.escapeHandled ) return;
		UIManagerOz.escapeHandled = true;
		
		if (currentViewController == mainVC)			// if on main menu screen			
			ShowExitPrompt();
		else
			OnHomeClicked(null);
	}	

	public void OnHomeClicked(GameObject button)
	{
		bool hidePaperVC = false;
		
		//		SharingManagerBinding.HideBusyIndicator();
		
		appCounters.UpdateSecondsSpentInApp();
		
		UIViewControllerOz nextViewController = mainVC;	//idolVC;

		if (currentViewController == mainVC)			// if on main menu screen			
		{
			if (goBackToIdolMenu)
				nextViewController = idolVC;			// go back to idol menu	
			else
				nextViewController = postVC;			// go back to post-run		
		
			hidePaperVC = true;
		}
		else if (currentViewController == inventoryVC)	// if on inventory screen			
		{
			if (previousViewController == postVC)
			{
				nextViewController = postVC;			// go back to post-run	
				hidePaperVC = true;
			}
			else
				nextViewController = mainVC;			// go back to main menu		
		}
		else
			nextViewController = mainVC;		
		
		currentViewController.disappear();	
		nextViewController.appear();
		
		// --- Analytics ------------------------------------------------------
		string previousPageName = analyticsPreviousPageName;
		
		string nextViewControllerAnalyticsName = "Main Menu";
		
		if ( nextViewController == idolVC )
		{
			nextViewControllerAnalyticsName = "Title Screen";
			previousPageName = analyticsCurrentPageName;
		}
		else if ( nextViewController == postVC )
		{
			nextViewControllerAnalyticsName = "Post Run";
		}
		
		//		AnalyticsInterface.LogNavigationActionEvent( "Back", previousPageName, nextViewControllerAnalyticsName );
		// ------------------------------------------------------------------
		
//		previousViewController = currentViewController;		// 'SetCurrentPage' now does this, from other pages
//		currentViewController = nextViewController;
		
		// fix for situation where main menu doesn't disappear when going into Challenges screen
		if (ObjectivesVC.gameObject.active && mainVC.gameObject.active)
			mainVC.disappear();
		
		if (hidePaperVC)
			disappear();
		
		// fix for extremely sporadic issue when 'Challenges' screen lingers for some reason
		//if (ObjectivesVC.gameObject.active == true && mainVC.gameObject.active == true)
		//	ObjectivesVC.disappear();
	}	
	
	public bool IsOneLevelDownFromMainMenu()			// Would hitting 'BACK' button bring us back to main menu?
	{
		return (currentViewController != idolVC && currentViewController != mainVC && previousViewController == mainVC);
	}
	
	public bool IsOnPostRunScreen()
	{
		return (currentViewController == postVC);
	}	
	
	void linkupOtherViewControllers()					// Links up other View Controllers, anything that inherits from UIViewControllerOz, but not exactly UIViewControllerOz
	{													
        System.Type myType = this.GetType();			// Get the type handle of a specified class.
		FieldInfo[] myFieldInfo;
        myFieldInfo = myType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
       
        for (int i = 0; i < myFieldInfo.Length; i++)	// Display the field information of FieldInfoClass. 
        {
			System.Type fieldType = myFieldInfo[i].FieldType;
			bool descendant = typeof(UIViewControllerOz).IsAssignableFrom(fieldType);
			if (fieldType != typeof(UIViewControllerOz) && descendant)
			{		
				MethodInfo method = typeof(UIManagerOz).GetMethod("GetInstantiatedObject", 
					BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public);
				MethodInfo generic = method.MakeGenericMethod(fieldType);
				System.Object result = generic.Invoke(UIManagerOz.SharedInstance, null);
				
				myFieldInfo[i].SetValue(this, result);
			}
        }		
	}		
}



		//previousViewController = idolVC;
		//currentViewController = mainVC;	

	
//	public void GoToStoreCoins()						// used for button in top right corner of UI
//	{
////		GoToUpgrades(UpgradesScreenName.MoreCoins);
//		//GoToStore(ShopScreenName.Coins);	
//	}
	
//	public void GoToStore(ShopScreenName screen)		// used only for going directly to coins/gems pages in ways other than clicking on the store button
//	{
////		notificationSystem.SetNotificationIconsToThisPage(UiScreenName.STORE);
//		//currentScreen = UiScreenName.STORE;
//		//SetPageName("Ttl_Store", GetStoreTitleKey(screen));
//		IAPStoreVC.SwitchToPanel(screen);
//		currentViewController.disappear();
//		IAPStoreVC.appear();
//		currentViewController = IAPStoreVC;		
//	}
		


		//CurrencyBar.transform.Find("CoinLabel").GetComponent<UILabel>().text = player.coinCount.ToString(); - had to comment out to get to build N.N.
		//CurrencyBar.transform.Find("GemLabel").GetComponent<UILabel>().text = player.specialCurrencyCount.ToString();	- had to comment out to get to build N.N.
	
	//public GameObject NewsFeed = null;	
	//public GameObject CurrencyBar = null; - remove don't need on this page N.N.
	


		//currentViewController = mainVC;

	
//	public void GoToMainMenu()
//	{
//		//notificationSystem.SetNotificationIconsToThisPage(UiScreenName.HOME);
//		//currentScreen = UiScreenName.HOME;
//		SetPageName("", "");	
//		currentViewController.disappear();	//.FadeOut(1.0f, mainVC.FadeIn);
//		mainVC.appear();
//		currentViewController = mainVC;	
//	}
	
//	public void GoToUpgrades(UpgradesScreenName screen)
//	{
//		//notificationSystem.SetNotificationIconsToThisPage(UiScreenName.UPGRADES);
//		//currentScreen = UiScreenName.UPGRADES;
//		SetPageName("Ttl_Upgrades", "Ttl_Sub_Powerups");				
//		inventoryVC.SwitchToPanel(screen);
//		currentViewController.disappear();
//		inventoryVC.appear();
//		currentViewController = inventoryVC;
//	}


//disappear();	//HidePaperVC();

	//public enum UiScreenName { HOME, SETTINGS, STORE, UPGRADES, OBJECTIVES, SOCIAL, MOREGAMES, STATS, }	// MAP
	//private UiScreenName currentScreen = UiScreenName.HOME; 
	


		//if (currentScreen == UiScreenName.HOME)					// if on home screen
//			switch (button.name)
//			{
//				case "HomeButton":								// Home button on home page		
//			}
				//currentScreen = UiScreenName.HOME;	
			
			
			//break;		

//		}
//		else if (button.name == "HomeButton")			// if not on HOME page, go home if HOME button clicked
//		{
//			notificationSystem.SetNotificationIconsToThisPage(UiScreenName.HOME);			
			//currentScreen = UiScreenName.HOME;
//			nextViewController = mainVC;	//idolVC;
//		}		
		

		

//		IAPMiniStoreVC.SwitchToPanel(screen);
		
		//currentViewController.FadeOut(1.0f, nextViewController.FadeIn);
		//if (currentViewController != nextViewController)
		//{		
		
		//}
		//else
		//	currentViewController.RefreshAndShow();
		


//		currentViewController = mainVC;		

		
//				case "SettingsButton":							// settings button
////					notificationSystem.SetNotificationIconsToThisPage(UiScreenName.SETTINGS);
//					currentScreen = UiScreenName.SETTINGS;
//					nextViewController = settingsVC;			// go to settings	
//					settingsVC.SwitchToPanel(SettingsScreenName.General);
//					break;
//				case "UpgradesButton":							// upgrades button
////					notificationSystem.SetNotificationIconsToThisPage(UiScreenName.UPGRADES);
//					currentScreen = UiScreenName.UPGRADES;
//					nextViewController = inventoryVC;
//					inventoryVC.SwitchToPanel(UpgradesScreenName.PowerUps);
//					break;
//				case "ChallengesButton":						// objectives button
////					notificationSystem.SetNotificationIconsToThisPage(UiScreenName.OBJECTIVES);
//					currentScreen = UiScreenName.OBJECTIVES;
//					nextViewController = ObjectivesVC;				
//					ObjectivesVC.SwitchToPanel(ObjectivesScreenName.Current);	//.Legendary);				
//					break;
//				case "LeaderboardsButton":						// social button
////					notificationSystem.SetNotificationIconsToThisPage(UiScreenName.SOCIAL);
//					currentScreen = UiScreenName.SOCIAL;
//					nextViewController = leaderboardVC;	
//					leaderboardVC.SwitchToPanel(SocialScreenName.SocialConnections, true); // Initial click = true				
//					break;		
//				case "MoreGamesButton":							// more games button
//					currentScreen = UiScreenName.MOREGAMES;
//					nextViewController = moreGamesVC;			// go to more games page
//					break;
//				case "StatsButton":								// stats button
//					currentScreen = UiScreenName.STATS;
//					nextViewController = statsVC;				// go to stats page
//					break;				


				

					//SetPageName("Ttl_Settings", "Ttl_Sub_General");					
					//SetPageName("Ttl_Upgrades", "Ttl_Sub_Powerups");				
					//SetPageName("Ttl_Objectives", "Ttl_Sub_CurObjs");
				//SetPageName("Ttl_Social", "Ttl_Sub_SocialConnct");			
					//SetPageName("", "");				
					//SetPageName("Ttl_Sub_Stats", "Ttl_Sub_Stats");						
							//SetPageName("", "");



//	private string GetStoreTitleKey(ShopScreenName screen)
//	{
//		string titleKey = "";	
//		
//		if (screen == ShopScreenName.Coins)
//			titleKey = "Ttl_Sub_Coins";
//		else if (screen == ShopScreenName.Gems)
//			titleKey = "Ttl_Sub_Gems";
//		else if (screen == ShopScreenName.Misc)
//			titleKey = "Ttl_Sub_Misc";
//		else if (screen == ShopScreenName.CoinOffers)
//			titleKey = "Ttl_Sub_Offers";	
//	
//		return titleKey;
//	}



				//ShowNewsFeed(false);

	
	//public UISprite CurrencyIcon = null;	
	//public UISprite SpecialCurrencyIcon = null;

	//public Transform BackButtonRoot = null;		// back button is no longer visible
	//public Camera BackgroundCamera = null;
	//public UISprite HomeButtonSprite = null;
	//public UISprite StoreButtonSprite = null;	
	//public GameObject CurrencyBar = null; 	

	//public UILabel CurrencyLabel = null;
	//public UILabel SpecialCurrencyLabel = null;	
	
//	public UIInGameViewControllerOz ingameVC = null;
//	public UIMapViewControllerOz mapVC = null;
	//public UICharacterSelectViewControllerOz characterSelectVC = null;	
		

//	private void ShowNewsFeed(bool state)
//	{
//		NewsFeed.transform.Find("LabelNewsFeed").GetComponent<UILabel>().enabled = state;		
//		//NewsFeed.transform.Find("SlicedSpriteBG").GetComponent<UISlicedSprite>().enabled = state;	//- gradient background - NN
//	}	
	
	


//		PlayerStats player = GameProfile.SharedInstance.Player;
//		SetCurrencyLabel(player.coinCount.ToString(), true, CostType.Coin);
//		SetCurrencyLabel(player.specialCurrencyCount.ToString(), true, CostType.Special)


//
//	public void SetCurrencyLabel(string currencyText, bool show = true, CostType costType = CostType.Coin) 
//	{
//		UILabel label = null;
//		
//		if (costType == CostType.Coin) 
//		{
//			label = CurrencyLabel;
//			
//			if (CurrencyLabel == null) { return; }
//			
//			if (show) 
//			{
//				CurrencyLabel.text = currencyText;
//				CurrencyLabel.enabled = true;
//			} 
//			else
//				CurrencyLabel.enabled = false;
//		}
//		else 
//		{
//			if (costType == CostType.Special)
//				label = SpecialCurrencyLabel;
//		
//			if (label == null) { return; }
//			
//			if (show) 
//			{
//				label.text = currencyText;
//				label.enabled = true;
//			} 
//			else
//				label.enabled = false;
//		}
//	}	


		//RespositionCurrencyWidgets();

//	private void RespositionCurrencyWidgets()
//	{
//		//-- These controls are children of a TopRight Anchor, so we lay out going negative X, starting at 0.
//		float currencySpacer = SpecialCurrencyIcon.transform.localScale.x*0.25f;
//		float currentX = 0;
//		
//		if (SpecialCurrencyIcon.enabled) 
//		{
//			SpecialCurrencyIcon.transform.localPosition = new Vector3(currentX, 0, 0);
//			currentX -= SpecialCurrencyIcon.transform.localScale.x;
//			
//			SpecialCurrencyLabel.transform.localPosition = new Vector3(currentX, SpecialCurrencyLabel.transform.localPosition.y, SpecialCurrencyLabel.transform.localPosition.z);
//			currentX -= (SpecialCurrencyLabel.relativeSize.x*SpecialCurrencyLabel.transform.localScale.x);
//			currentX -= currencySpacer;
//		}
//		
//		if (CurrencyIcon.enabled) 
//		{
//			CurrencyIcon.transform.localPosition = new Vector3(currentX, 0, 0);
//			currentX -= CurrencyIcon.transform.localScale.x;
//			CurrencyLabel.transform.localPosition = new Vector3(currentX, CurrencyLabel.transform.localPosition.y, CurrencyLabel.transform.localPosition.z);
//		}
//	}


			//ShowCurrencyBar(true);


//	public void ShowPageNamePanel(bool state, string title, string subTitle)
//	{	
//		PageNamePanel.transform.Find("TitleLabel").GetComponent<UILocalize>().SetKey(title);
//		PageNamePanel.transform.Find("SubtitleLabel").GetComponent<UILocalize>().SetKey(subTitle);	
//		
//		if (state)
//			iTween.MoveTo(PageNamePanel, iTween.Hash("isLocal", true, "position", new Vector3(65.0f, 0.0f, 0.0f),
//				"time", 0.1f, "easetype", iTween.EaseType.easeOutSine));
//		else
//			iTween.MoveTo(PageNamePanel, iTween.Hash("isLocal", true, "position", new Vector3(-225.0f, 0.0f, 0.0f),
//				"time", 0.1f, "easetype", iTween.EaseType.easeOutSine));
//	}	


		//linkupOtherViewControllers();		
		
//		notificationSystem = Services.Get<NotificationSystem>();
//		notificationSystem.notificationIcons = gameObject.GetComponent<NotificationIcons>();
					
		// bring in bottom menu
		//BottomPanel.transform.localPosition = new Vector3(0.0f, -450.0f, 0.0f);
		//Invoke("BringInBottomPanel", 2.0f);
		
		// set up buttons for main menu
		//ShowSettingsButton(true);		// HomeButtonSprite.spriteName = "icon_main_settings";		// show settings button icon
//		ShowCurrencyBar(true);	//false);
//		ShowPageNamePanel(true, "", "");	//false, "", "");
//		SetCheckboxStatus(false);
		//ShowMainMenuButtons(true);
//		ShowTabs(false);
		//ObjectivesVC.ConnectCheckBoxes(checkboxes)		
		

			
//	private void ShowCurrencyBar(bool state)
//	{
//		if (state)
//			iTween.MoveTo(CurrencyBar, iTween.Hash("isLocal", true, "position", Vector3.zero,
//				"time", 0.1f, "easetype", iTween.EaseType.easeOutSine));
//		else
//			iTween.MoveTo(CurrencyBar, iTween.Hash("isLocal", true, "position", new Vector3(350.0f, 0.0f, 0.0f),
//				"time", 0.1f, "easetype", iTween.EaseType.easeOutSine));
//	}	
	
		
		
		
		//SwapIcons(emptyIcons, emptyBG);
		
		//SetCheckboxStatus(false);		
		
		//ShowMainMenuButtons(true);
		//ShowTabs(false);	
	
		//if (currentViewController != null)
		//	currentViewController.disappear();

		
				//SwapIcons(upgradesIcons, tabBG);

				//SetCheckboxStatus(true);

		//ShowMainMenuButtons(false);
		//ShowTabs(true);		
		//ResetTabs((int)screen);		
	
		//if (currentViewController != null)


		//SwapIcons(storeIcons, tabBG);
		//nextViewController = IAPStoreVC;		
			//SetCheckboxStatus(true);	
			//ShowMainMenuButtons(false);
		//ShowTabs(true);
			//ResetTabs((int)screen);	// highlight the appropriate tab

		//if (currentViewController != null)	
		



	//PageNamePanel.transform.Find("TitleLabel").GetComponent<UILabel>().text = title;
		//PageNamePanel.transform.Find("SubtitleLabel").GetComponent<UILabel>().text = subTitle;		
		
		//ShowSettingsButton(false);	//HomeButtonSprite.spriteName = "icon_navigation_home";
					
		
		
							//SwapIcons(emptyIcons, emptyBG);		
					//SetCheckboxStatus(false);
					//ShowMainMenuButtons(false);
					//ShowTabs(false);
		
		
					
				
				//SwapIcons(settingsIcons, tabBG);
				
						//SetCheckboxStatus(true);					
					//ShowMainMenuButtons(false);
					//ShowTabs(true);
					//ResetTabs();				
						
		//SwapIcons(upgradesIcons, tabBG);				
					//SetCheckboxStatus(true);				
					//ShowMainMenuButtons(false);
					//ShowTabs(true);
					//ResetTabs();	
				
				
					//SwapIcons(objectivesIcons, tabBG);				
					//SetCheckboxStatus(true);				
					//ShowMainMenuButtons(false);	
					//ShowTabs(true);
					//ResetTabs();	//(int)ObjectivesScreenName.Legendary);					
					
		
				
					//SwapIcons(socialIcons, tabBG);				
					//SetCheckboxStatus(true);				
					//ShowMainMenuButtons(false);
					//ShowTabs(true);
				//ResetTabs();	//(int)SocialScreenName.Challenges);				
						
	
				//SwapIcons(emptyIcons, emptyBG);				
					//SetCheckboxStatus(false);
				//ShowMainMenuButtons(false);
					//ShowTabs(false);			
		
		
					//SetCheckboxStatus(false);
					//ShowMainMenuButtons(false);
					//ShowTabs(false);						
					//SwapIcons(emptyIcons, emptyBG);				
						
			
//			if (firstRun)
//				nextViewController = mainVC;			// go back to main menu	
//			else
//				nextViewController = postVC;			// go back to post-run		
			

			//ShowSettingsButton(true);	//HomeButtonSprite.spriteName = "icon_main_settings";
			//SetCheckboxStatus(false);			
			//SwapIcons(mainIcons, navigationBG);
						
			//ShowMainMenuButtons(true);
			//ShowTabs(false);		
		
		
//		if (currentViewController != null)
//			currentViewController.disappear();
//		//nextViewController.previousViewController = this;
//		nextViewController.appear();




//	public List<GameObject> tabGOs = new List<GameObject>();		// for tabs at top, while sub-pages are being shown
//	private List<UICheckbox> tabCheckboxes = new List<UICheckbox>();	
//	private List<UISprite> tabSprites = new List<UISprite>();
//	private List<UISlicedSprite> tabBGs = new List<UISlicedSprite>();
//	private List<UISprite> tabFrames = new List<UISprite>();
//	private List<UILabel> tabLabels = new List<UILabel>();		
//	private List<BoxCollider> tabColliders = new List<BoxCollider>();
//	
//	private string[] mainIcons = { "icon_main_upgrades", "icon_main_map", "icon_main_objectives", "icon_main_social", };
//	private string[] upgradesIcons = { "icon_upgrades_magictricks", "icon_upgrades_spells", "icon_upgrades_charms", "icon_store_coins", };
//	private string[] objectivesIcons = { "icon_objectives_current", "icon_objectives_weekly", "icon_objectives_legendary", "icon_objectives_complete", };
//	private string[] storeIcons = { "icon_store_gems", "icon_store_coins", "icon_store_misc", "icon_store_freecoins", };
//	private string[] socialIcons = { "icon_social_connections", "icon_social_topscore", "icon_social_topdistance", "icon_social_challenges", };	
//	private string[] settingsIcons = { "icon_settings_device", "icon_settings_support", "icon_settings_terms", "icon_settings_privacy", };		
//	private string[] emptyIcons = { "tools_1x1_empty_sprite", "tools_1x1_empty_sprite", "tools_1x1_empty_sprite", "tools_1x1_empty_sprite", };		
//	
//	private string navigationBG = "button_navigation";
//	private string tabBG = "button_tabs";
//	private string emptyBG = "tools_1x1_empty_sprite";



		// find icons and backgrounds for all tabs in top row (the ones that change from page to page)
//		foreach (GameObject tabGO in tabGOs)
//		{
//			tabCheckboxes.Add(tabGO.GetComponent<UICheckbox>());	
//			tabSprites.Add(tabGO.transform.Find("Sprite").GetComponent<UISprite>());	
//			tabBGs.Add(tabGO.transform.Find("Background").GetComponent<UISlicedSprite>());
//			tabFrames.Add(tabGO.transform.Find("Frame").GetComponent<UISprite>());
//			tabLabels.Add(tabGO.transform.Find("Label").GetComponent<UILabel>());
//			tabColliders.Add(tabGO.GetComponent<BoxCollider>());	
//		}	



		
		// set up buttons for main menu
		//ShowSettingsButton(true);		// HomeButtonSprite.spriteName = "icon_main_settings";		// show settings button icon
//		ShowCurrencyBar(true);	//false);
//		ShowPageNamePanel(true, "", "");	//false, "", "");
//		SetCheckboxStatus(false);
//		ShowMainMenuButtons(true);
//		ShowTabs(false);
		//ObjectivesVC.ConnectCheckBoxes(checkboxes);

	
//	private void SwapIcons(string[] iconNamesArray, string backgroundIconName)
//	{
//		for (int i=0; i<tabGOs.Count; i++)
//			tabSprites[i].spriteName = iconNamesArray[i];
//	
//		for (int i=0; i<tabBGs.Count; i++)
//			tabBGs[i].spriteName = backgroundIconName;
//		
//		// disable all colliders if buttons hidden, otherwise enable all colliders
//		foreach (BoxCollider collider in tabColliders)
//			collider.enabled = (backgroundIconName == emptyBG) ? false : true;
//				
//		// same for frames
//		foreach (UISprite frame in tabFrames)
//			frame.enabled = (backgroundIconName == emptyBG) ? false : true;
//		
//		// double-check hide all icons and backgrounds, including checkboxes, if empty
//		if (backgroundIconName == emptyBG)
//		{
//			foreach (GameObject tabGO in tabGOs)
//			{
//				tabGO.GetComponent<UIImageButton>().enabled = false;
//				tabGO.GetComponent<UICheckbox>().enabled = false;
//				tabGO.GetComponent<UICheckbox>().checkSprite.enabled = false;
//			}					
//		}
//	}
	
//	private void ShowTabs(bool show) 
//	{
//		for (int i=0; i<tabGOs.Count; i++)
//		{
//			tabCheckboxes[i].enabled = show;
//			tabSprites[i].enabled = show;
//			tabBGs[i].enabled = show;
//			tabColliders[i].enabled = show;
//			tabFrames[i].enabled = show;
//			tabLabels[i].enabled = show;
//		}
//	}	
	



//	private void SetCheckboxStatus(bool active)		// true to switch to 'checkboxes' for button, false to use basic image buttons
//	{
//		foreach (GameObject tabGO in tabGOs)
//		{
//			tabGO.GetComponent<UIImageButton>().enabled = !active;
//			tabGO.GetComponent<UICheckbox>().enabled = active;
//			if (!active)	// disable checkmark highlight if going back to button mode	
//				tabGO.GetComponent<UICheckbox>().checkSprite.enabled = false;
//			else 
//				tabGO.GetComponent<UICheckbox>().checkSprite.enabled = true;
//		}				
//	}
//
//	private void ResetTabs(int buttonIndex = 0)
//	{
//		//foreach (UICheckbox checkbox in checkboxes)	// deactivate all of them
//		//	checkbox.isChecked = false;
//		
//		tabCheckboxes[buttonIndex].isChecked = true;	// set one to active
//		currentTabIndex = buttonIndex;
//		Invoke("TabHack", 0.01f);		// hack to highlight the correct tab
//	}
//	
//	private int currentTabIndex = 0;	// hack to highlight the correct tab
//	
//	private void TabHack()				// hack to highlight the correct tab
//	{
//		tabCheckboxes[currentTabIndex].isChecked = true;
//	}
		




//		if (currentScreen == UiScreenName.UPGRADES)	// inventory screen buttons	
//		{
//			switch (button.name)
//			{
//				case "Tab0":						// powerups button
//					inventoryVC.SwitchToPanel(UpgradesScreenName.PowerUps);
//					ShowPageNamePanel(true, "Ttl_Upgrades", "Ttl_Sub_Powerups");				
//					break;	
//				case "Tab1":						// modifiers button
//					inventoryVC.SwitchToPanel(UpgradesScreenName.Artifacts);
//					ShowPageNamePanel(true, "Ttl_Upgrades", "Ttl_Sub_Modifiers");
//					break;				
//				case "Tab2":						// consumables button
//					inventoryVC.SwitchToPanel(UpgradesScreenName.Consumables);
//					ShowPageNamePanel(true, "Ttl_Upgrades", "Ttl_Sub_Consumables");				
//					break;	
//				case "Tab3":						// more coins button
//					inventoryVC.SwitchToPanel(UpgradesScreenName.MoreCoins);
//					ShowPageNamePanel(true, "", "");	//Ttl_Upgrades", "Ttl_Sub_Stats");				
//					break;
//			}
//			nextViewController = inventoryVC;
//		}
		
//		else if (currentScreen == UiScreenName.OBJECTIVES)// objectives screen buttons	
//		{
//			switch (button.name)
//			{
//				case "Tab0":						// current button
//					ObjectivesVC.SwitchToPanel(ObjectivesScreenName.Current);
//					ShowPageNamePanel(true, "Ttl_Objectives", "Ttl_Sub_CurObjs");				
//					break;
//				case "Tab1":						// weekly challenges button
//					ObjectivesVC.SwitchToPanel(ObjectivesScreenName.Weekly);
//					ShowPageNamePanel(true, "Ttl_Objectives", "Ttl_Sub_WklyChallenges");		
//					break;	
//				case "Tab2":						// legendary button
//					ObjectivesVC.SwitchToPanel(ObjectivesScreenName.Legendary);
//					ShowPageNamePanel(true, "Ttl_Objectives", "Ttl_Sub_Legendary");	
//					break;	
//				case "Tab3":						// completed button
//					ObjectivesVC.SwitchToPanel(ObjectivesScreenName.Completed);
//					ShowPageNamePanel(true, "Ttl_Objectives", "Ttl_Sub_Completed");	
//					break;
//			}
//			nextViewController = ObjectivesVC;
//		}			
		
//		if (currentScreen == UiScreenName.SOCIAL)	// social screen buttons	
//		{
//			switch (button.name)
//			{
//				case "Tab0":						// social connections button
//					leaderboardVC.SwitchToPanel(SocialScreenName.SocialConnections);
//					ShowPageNamePanel(true, "Ttl_Social", "Ttl_Sub_SocialConnct");	
//					break;
//				case "Tab1":						// top scores button
//					leaderboardVC.SwitchToPanel(SocialScreenName.TopScores);
//					ShowPageNamePanel(true, "Ttl_Social", "Ttl_Sub_TopScores");
//					break;	
//				case "Tab2":						// top distances button
//					leaderboardVC.SwitchToPanel(SocialScreenName.TopDistances);
//					ShowPageNamePanel(true, "Ttl_Social", "Ttl_Sub_TopDistance");
//					break;	
//				case "Tab3":						// challenges button
//					leaderboardVC.SwitchToPanel(SocialScreenName.Challenges);
//					ShowPageNamePanel(true, "Ttl_Social", "Ttl_Sub_FriendChallenges");
//					break;
//			}
//			nextViewController = leaderboardVC;
//		}		
		
//		else if (currentScreen == UiScreenName.STORE)	// store screen buttons	
//		{
//			switch (button.name)
//			{
//				case "Tab0":						// gems button
//					IAPStoreVC.SwitchToPanel(ShopScreenName.Gems);
//					ShowPageNamePanel(true, "Ttl_Store", GetStoreTitleKey(ShopScreenName.Gems));
//					break;
//				case "Tab1":						// coins button
//					IAPStoreVC.SwitchToPanel(ShopScreenName.Coins);
//					ShowPageNamePanel(true, "Ttl_Store", GetStoreTitleKey(ShopScreenName.Coins));
//					break;	
//				case "Tab2":						// misc button
//					IAPStoreVC.SwitchToPanel(ShopScreenName.Misc);
//					ShowPageNamePanel(true, "Ttl_Store", GetStoreTitleKey(ShopScreenName.Misc));
//					break;	
//				case "Tab3":						// free coins button
//					IAPStoreVC.SwitchToPanel(ShopScreenName.CoinOffers);
//					ShowPageNamePanel(true, "Ttl_Store", GetStoreTitleKey(ShopScreenName.CoinOffers));
//					break;
//			}
//			nextViewController = IAPStoreVC;
//		}		
		
//		else if (currentScreen == UiScreenName.SETTINGS)		// settings screen buttons	
//		{
//			switch (button.name)
//			{
//				case "Tab0":									// general settings button
//					settingsVC.SwitchToPanel(SettingsScreenName.General);		//0);
//					ShowPageNamePanel(true, "Ttl_Settings", "Ttl_Sub_General");
//					break;
//				case "Tab1":									// customer support button
//					settingsVC.SwitchToPanel(SettingsScreenName.CustomerSupport);	//1);
//					ShowPageNamePanel(true, "Ttl_Settings", "Lbl_About_Link_Support");
//					break;	
//				case "Tab2":									// terms of use button
//					settingsVC.SwitchToPanel(SettingsScreenName.TermsOfUse);	//2);
//					ShowPageNamePanel(true, "Ttl_Settings", "Lbl_About_Link_Terms");
//					break;	
//				case "Tab3":									// privacy policy button
//					settingsVC.SwitchToPanel(SettingsScreenName.PrivacyPolicy);		//3);
//					ShowPageNamePanel(true, "Ttl_Settings", "Lbl_About_Link_Privacy");
//					break;
//			}
//			nextViewController = settingsVC;
//		}		


	
//	public void ShowPaperVC()
//	{
//		NGUITools.SetActive(this.gameObject, true);
//		notificationSystem.SetNotificationIconsToThisPage(UiScreenName.HOME);
//	}
	
//	public void HidePaperVC()
//	{
//		NGUITools.SetActive(this.gameObject, false);
//	}		
	


//	public void SetPlayButtonCallback(GameObject target, string functionName) 
//	{
//		//if (BackButtonRoot == null) { return; }
//		if (PlayButtonRoot == null) { return; }		
//		UIButtonMessage message = PlayButtonRoot.GetComponent<UIButtonMessage>() as UIButtonMessage;
//		if (message == null) { return; }
//		message.target = target;
//		message.functionName = functionName;
//	}	
			

//	public FadePanel DisappearFromFade(float duration, FadePanel.fadeDoneCallback callback = null)
//	{
//		ShowObject(gameObject, false, true);	//NGUITools.SetActive(gameObject, false);
//		return dummyFadePanel;
//	}	
//	
//	public FadePanel FadeIn(float duration = 1f, FadePanel.fadeDoneCallback callback = null)
//	{
//		ShowObject(gameObject, true, true);	
//		FadePanel fp = gameObject.AddComponent<FadePanel>();
//		fp.SetParameters(true, duration);
//		return fp;
//	}
//	
//	public FadePanel FadeOut(float duration = 1f, FadePanel.fadeDoneCallback callback = null)
//	{
//		FadePanel fp = gameObject.AddComponent<FadePanel>();
//		fp.SetParameters(false, duration);	
//		fp.AddCallback(DisappearFromFade);
//		
//		if (callback != null)
//			fp.AddCallback(callback);	// for adding additional callbacks
//		
//		return fp;
//	}	
//	
//	public void ShowObject(GameObject theObject, bool show, bool recursive)
//	{
//		if (theObject == null) { return; }
//		
//		if (recursive == true) 
//			theObject.SetActiveRecursively(show); 
//		else
//			theObject.active = show;
//	}	


	// Linkups the other view Controllers, anything that inherits from UIViewControllerOz, but is not exactly UIViewControllerOz
//	void linkupOtherViewControllers()
//	{
//        // Get the type handle of a specified class.
//        System.Type myType = this.GetType();
//		FieldInfo[] myFieldInfo;
//        myFieldInfo = myType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
//		
//        // Display the field information of FieldInfoClass. 
//        for (int i = 0; i < myFieldInfo.Length; i++)
//        {
//			System.Type fieldType = myFieldInfo[i].FieldType;
//			bool descendant = typeof(UIViewControllerOz).IsAssignableFrom(fieldType);
//			if (fieldType != typeof(UIViewControllerOz) && descendant)
//			{		
//				MethodInfo method = typeof(UIManagerOz).GetMethod("GetInstantiatedObject", 
//					BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public);
//				MethodInfo generic = method.MakeGenericMethod(fieldType);
//				System.Object result = generic.Invoke(UIManagerOz.SharedInstance, null);
//				
//				myFieldInfo[i].SetValue(this, result);
//			}
//        }		
//	}		



	
//	public void SetTitle(string titleText) 
//	{
//		if (TitleLabel == null) { return; }
//		TitleLabel.text = titleText;
//	}
	



//	public void ShowPlayButton(bool show = true) 
//	{
//		if (PlayButtonRoot == null || PlayButtonRoot.gameObject == null) { return; }
//		PlayButtonRoot.gameObject.SetActiveRecursively(show);
//	}
//	
//	public void ShowBackButton(bool show = true) 
//	{
//		if (BackButtonRoot == null || BackButtonRoot.gameObject == null) { return; }
//		BackButtonRoot.gameObject.SetActiveRecursively(false);//show);		// never show back button anymore
//	}
//	
//	public void ShowBackgroundCamera(bool show = true)
//	{
//		if (BackgroundCamera == null || BackgroundCamera.gameObject == null) { return; }
//		BackgroundCamera.gameObject.SetActiveRecursively(show);
//	}	
	
//	public void SetPlayButtonCallback(GameObject target, string functionName) 
//	{
//		//if (BackButtonRoot == null) { return; }
//		if (PlayButtonRoot == null) { return; }		
//		UIButtonMessage message = PlayButtonRoot.GetComponent<UIButtonMessage>() as UIButtonMessage;
//		if (message == null) { return; }
//		message.target = target;
//		message.functionName = functionName;
//	}
//	
//	public void SetBackButtonCallback(GameObject target, string functionName)
//	{
//		if (BackButtonRoot == null) { return; }
//		UIButtonMessage message = BackButtonRoot.GetComponent<UIButtonMessage>() as UIButtonMessage;
//		if (message == null) { return; }
//		message.target = target;
//		message.functionName = functionName;
//	}
	

		
		
		

				//GameController.SharedInstance.ShowStartingTemple();	
		
		// kill Post-run view controller, if active
//		if (postVC != null)
//			postVC.disappear();		
		
//		// kill Main view controller, if active
//		if (nextViewController != mainVC)	//idolVC)
//			mainVC.disappear();	//idolVC.disappear();			
				
		
		
		// jonoble: Commented these out until they get connected
		//NGUITools.SetActive(DisneyButton, false); //-deactivating buttons N.N.
		//NGUITools.SetActive(TwitterButton, false);
		//NGUITools.SetActive(FBButton, false);
				
		
//		// kill Post-run view controller, if active
//		if (postVC != null)
//			postVC.disappear();		
//		
//		// kill Idol view controller, if active
//		if (currentViewController == mainVC)	//idolVC)
//		{
//			mainVC.disappear();	//idolVC.disappear();	
//			currentViewController = null;
//		}
		



					//ShowBottomPanelButton(false);
					//ShowScreenShotButton(false);
					//ShowMoreDisneyButton(false);
					//ResetTabs();					
				
	
	//public GameObject DisneyButton = null;
	//public GameObject FBButton = null;
	//public GameObject TwitterButton = null;
	
//	public GameObject BottomPanel = null;		
	//public GameObject BottomPanelButton = null;
	//public GameObject BottomPanelButtonWithoutArrow = null;	
	//private bool BottomPanelOpenState = false;
	
	
//	private ShareButton shareButton;
//		shareButton = gameObject.GetComponent<ShareButton>();		
			
	



//		switch (button.name)						// on any screen
//		{
//			case "ShopButton":
//				if (StoreButtonSprite.spriteName == "icon_navigation_store")	// open store		
//				{	
//					notificationSystem.SetNotificationIconsToThisPage(UiScreenName.STORE);
//					SwapIcons(storeIcons, tabBG);
//					currentScreen = UiScreenName.STORE;
//					nextViewController = IAPStoreVC;
//					ShowNewsFeed(false);
//					SetCheckboxStatus(true);
//					ShowCurrencyBar(true);
//					ShowPageNamePanel(true, "Ttl_Store", GetStoreTitleKey(ShopScreenName.Gems));
//					//ShowBottomPanelButton(false);
//					//ShowScreenShotButton(false);
//					//ShowMoreDisneyButton(true);
//					ShowMainMenuButtons(false);
//					ShowTabs(true);
//					IAPStoreVC.SwitchToPanel(ShopScreenName.Gems);
//					ResetTabs();
//				}
//				else if (StoreButtonSprite.spriteName == "icon_main_moredisney")	// go to 'more disney' page
//				{
//					// open 'more disney' page
//					//notificationSystem.SetNotificationIconsToThisPage(UiScreenName.SETTINGS);
//					SwapIcons(mainIcons, navigationBG);
//					currentScreen = UiScreenName.HOME;
//					nextViewController = moreGamesVC;	// go to settings	
//					SetCheckboxStatus(true);
//					ShowNewsFeed(false);
//					ShowCurrencyBar(false);
//					ShowPageNamePanel(false, "", "");
//					//ShowBottomPanelButton(false);
//					//ShowScreenShotButton(false);
//					//ShowMoreDisneyButton(false);
//					ShowMainMenuButtons(false);
//					ShowTabs(true);
//					ResetTabs();		
//				}			
//				else //if (StoreButtonSprite.spriteName == "icon_pause_snapshot")
//				{
//					// do sharing functionality
//					shareButton.DoShareFunction(null);		
//				}
//				break;
//		}		


//	public void OnEnable() // at the moment this needs to be set in 2 different locations so there won't be any bugs
//	{
//		if (BottomPanelOpenState == true)
//		{
//			// updated the code, dave wants to see this- N.N
//			BottomPanelButton.transform.Find("ButtonTwitter").GetComponent<UISprite>().enabled = false;
//			BottomPanelButton.transform.Find("ButtonFB").GetComponent<UISprite>().enabled = false;
//			BottomPanelButton.transform.Find("ButtonDisney").GetComponent<UISprite>().enabled = false;
//		}
////		else
////		{
////			// updated the code, dave wants to see this- N.N.
////			BottomPanelButton.transform.Find("ButtonTwitter").GetComponent<UISprite>().enabled = true; 
////			BottomPanelButton.transform.Find("ButtonFB").GetComponent<UISprite>().enabled = true;
////			BottomPanelButton.transform.Find("ButtonDisney").GetComponent<UISprite>().enabled = true;
////		}
//	}

	
//	public void DoIntroAnimation()
//	{
//		//Invoke("BringInBottomPanel", 2.0f);  // this will animate in the open state of the main menu and we don't want that N.N.
//		ShowBottomPanel(BottomPanelOpenState); // this only brings in the closed state of the main menus N.N.
//	}
	
//	public void BringInBottomPanel()
//	{
//		notificationSystem.SetNotificationIconsToThisPage(UiScreenName.HOME);
//		ShowNewsFeed(true);		
//		//ShowBottomPanelButton(true);
//		ShowBottomPanel(true, 0.5f);
//		//iTween.MoveTo(BottomPanel, iTween.Hash("isLocal", true, "position", Vector3.zero,
//		//	"time", 0.5f, "easetype", iTween.EaseType.easeOutSine));
//		//BottomPanelOpenState = true;		
//	}
	
//	public void OnBottomPanelButtonClick()	// toggle bottom panel open or closed
//	{
//		ShowBottomPanel(!BottomPanelOpenState);
//		
//		if (BottomPanelOpenState == true)
//		{
//			// updated the code, dave wants to see this- N.N
//			BottomPanelButton.transform.Find("ButtonTwitter").GetComponent<UISprite>().enabled = false;
//			BottomPanelButton.transform.Find("ButtonFB").GetComponent<UISprite>().enabled = false;
//			BottomPanelButton.transform.Find("ButtonDisney").GetComponent<UISprite>().enabled = false;
//		}
////		else
////		{
////			// updated the code, dave wants to see this- N.N.
////			BottomPanelButton.transform.Find("ButtonTwitter").GetComponent<UISprite>().enabled = true; 
////			BottomPanelButton.transform.Find("ButtonFB").GetComponent<UISprite>().enabled = true;
////			BottomPanelButton.transform.Find("ButtonDisney").GetComponent<UISprite>().enabled = true;
////		}
//	}
	
//	private void ShowBottomPanelButton(bool state, bool withoutArrow = false)
//	{
//		GameObject bottomPanelButtonStyle = BottomPanelButton;			// regular version, with arrow
//		
//		if (withoutArrow)
//			bottomPanelButtonStyle = BottomPanelButtonWithoutArrow;		// for post-run page, use version without arrow 
//		
//		// added more disney, fb, and twitter buttons to this for post screen - N.N.
//		// turn off everything
//		BottomPanelButton.transform.Find("Background").GetComponent<UISprite>().enabled = false;
//		BottomPanelButton.transform.Find("Sprite").GetComponent<UISprite>().enabled = false;
//		BottomPanelButton.transform.Find("SpriteB").GetComponent<UISprite>().enabled = false;	// empty sprite, so code doesn't break
//		BottomPanelButton.GetComponent<BoxCollider>().enabled = false;
//		BottomPanelButton.transform.Find("ButtonTwitter").GetComponent<UISprite>().enabled = false;
//		BottomPanelButton.transform.Find("ButtonFB").GetComponent<UISprite>().enabled = false;
//		BottomPanelButton.transform.Find("ButtonDisney").GetComponent<UISprite>().enabled = false;
//		BottomPanelButtonWithoutArrow.transform.Find("Background").GetComponent<UISprite>().enabled = false;
//		BottomPanelButtonWithoutArrow.transform.Find("Sprite").GetComponent<UISprite>().enabled = false;
//		BottomPanelButtonWithoutArrow.transform.Find("SpriteB").GetComponent<UISprite>().enabled = false;
//		BottomPanelButtonWithoutArrow.transform.Find("ButtonTwitter").GetComponent<UISprite>().enabled = false; // empty sprite, so code doesn't break
//		BottomPanelButtonWithoutArrow.transform.Find("ButtonFB").GetComponent<UISprite>().enabled = false; // empty sprite, so code doesn't break
//		BottomPanelButtonWithoutArrow.transform.Find("ButtonDisney").GetComponent<UISprite>().enabled = false; // empty sprite, so code doesn't break
//		
//		// turn on only the correct one, if any
//		bottomPanelButtonStyle.transform.Find("Background").GetComponent<UISprite>().enabled = state;
//		bottomPanelButtonStyle.transform.Find("Sprite").GetComponent<UISprite>().enabled = state;
//		bottomPanelButtonStyle.transform.Find("SpriteB").GetComponent<UISprite>().enabled = state;
//		bottomPanelButtonStyle.transform.Find("ButtonTwitter").GetComponent<UISprite>().enabled = state;
//		bottomPanelButtonStyle.transform.Find("ButtonFB").GetComponent<UISprite>().enabled = state;
//		bottomPanelButtonStyle.transform.Find("ButtonDisney").GetComponent<UISprite>().enabled = state;
//		bottomPanelButtonStyle.GetComponent<BoxCollider>().enabled = state;
//	}
	
//	private void ShowBottomPanel(bool state, float time = 0.2f)
//	{
//		Vector3 finalPos = Vector3.zero;
//		
//		if (state)
//			finalPos = Vector3.zero;
//		else
//			finalPos = new Vector3(0.0f, -313.0f, 0.0f);
//		
//		iTween.MoveTo(BottomPanel, iTween.Hash(
//			"isLocal", true, 
//			"position", finalPos,
//			//"oncompletetarget", gameObject,
//			//"oncomplete", "FlipArrow",
//			//"oncompleteparams", !state,
//			"time", time, 
//			"easetype", iTween.EaseType.easeOutSine));	
//		
//		UISprite arrow = BottomPanelButton.transform.Find("Sprite").GetComponent<UISprite>();
//		float mult = !state ? -1.0f : 1.0f;
//		
//		//arrow.transform.localScale = new Vector3(arrow.transform.localScale.x, 38.6f * mult, 1.0f);
//		iTween.ScaleTo(arrow.gameObject, new Vector3(arrow.transform.localScale.x, 38.6f * mult, 1.0f), time * 2.0f);	
//		
//		BottomPanelOpenState = state;
//	}		
	
	
//	private void ShowSettingsButton(bool state)
//	{
//		if (state)
//			HomeButtonSprite.spriteName = "icon_main_settings";		// settings button icon
//		else
//			HomeButtonSprite.spriteName = "icon_navigation_home";	// home button icon
//	}	
	
//	private void ShowScreenShotButton(bool state)
//	{
//		if (state)
//			StoreButtonSprite.spriteName = "icon_pause_snapshot";	// screenshot button icon
//		else
//			StoreButtonSprite.spriteName = "icon_navigation_store";	// store button icon
//	}		
	
//	private void ShowMoreDisneyButton(bool state)
//	{
//		if (state)
//			StoreButtonSprite.spriteName = "icon_main_moredisney";	// more disney button icon
//		else
//			StoreButtonSprite.spriteName = "icon_navigation_store";	// store button icon
//	}			
		
	
	



//				case "MoreGamesButton":				// more games button
//					SwapIcons(emptyIcons, emptyBG);
//					nextViewController = moreGamesVC;
//					ShowNewsFeed(false);
//					ShowCurrencyBar(true);
//					ShowPageNamePanel(true);
//					ShowBottomPanelButton(false);
//					break;				
				
//				case "MapButton":						// map button
//					notificationSystem.SetNotificationIconsToThisPage(UiScreenName.MAP);
//					SwapIcons(emptyIcons, emptyBG);		//SwapIcons(mainIcons, navigationBG);	
//					currentScreen = UiScreenName.MAP;
//					nextViewController = mapVC;
//					//SetCheckboxStatus(false);	//true);
//					ShowNewsFeed(false);
//					ShowCurrencyBar(true);
//					ShowPageNamePanel(true, "Ttl_Map", "Ttl_Sub_Map");
//					ShowBottomPanelButton(false);
//					ShowScreenShotButton(false);
//					ShowMoreDisneyButton(false);
//					ShowSettingsButton(false);
//					ShowMainMenuButtons(false);
//					ShowTabs(true);
//					break;


	
//	public void SetToPostRun()
//	{
//		notificationSystem.SetNotificationIconsToThisPage(UiScreenName.HOME);
//		SwapIcons(mainIcons, navigationBG);
//		currentScreen = UiScreenName.HOME;
//		ShowSettingsButton(true);	
//		SetCheckboxStatus(false);
//		ShowNewsFeed(true);
//		ShowCurrencyBar(true);
//		ShowPageNamePanel(false, "", "");
//		ShowBottomPanelButton(true, true);	
//		ShowScreenShotButton(false);
//		ShowMainMenuButtons(false);
//		
//		if (BottomPanelOpenState != true)	// force main menu state to open N.N.
//			ShowBottomPanel(!BottomPanelOpenState); 
//	}
	
//	public void SetToPostRunStats()
//	{
//		notificationSystem.SetNotificationIconsToThisPage(UiScreenName.HOME);
//		SwapIcons(mainIcons, navigationBG);
//		currentScreen = UiScreenName.HOME;
//		ShowSettingsButton(true);	
//		SetCheckboxStatus(false);
//		ShowNewsFeed(true);
//		ShowCurrencyBar(true);
//		ShowPageNamePanel(false, "", "");
//		ShowBottomPanelButton(false);
//		ShowScreenShotButton(true);
//	}
	


	
//	private void FlipArrow(bool flip)
//	{
//		UISprite arrow = BottomPanelButton.transform.Find("Sprite").GetComponent<UISprite>();
//		float mult = flip ? -1.0f : 1.0f;
//		
//		//arrow.transform.localScale = new Vector3(arrow.transform.localScale.x, 38.6f * mult, 1.0f);
//		iTween.ScaleTo(arrow.gameObject, new Vector3(arrow.transform.localScale.x, 38.6f * mult, 1.0f), 0.1f);
//		
//		BottomPanelOpenState = !flip;
//	}
	

	
	// mainIcons, upgradesIcons, objectivesIcons, storeIcons, socialIcons, emptyIcons
	// navigationBG, tabBG, emptyBG
	
//	public void OnActivateButton0(bool _value)	// checkbox clicks
//	{
//		switch (currentScreen)
//		{
//			case UiScreenName.OBJECTIVES:
//				ObjectivesVC.OnActivateLocal(_value);
//				break;
//			case UiScreenName.SOCIAL:
//				leaderboardVC.SwitchToPanel(SocialScreenName.SocialConnections);	//leaderboardVC.OnActivateSocialConnections(_value);
//				break;	
//		}
//	}
//	
//	public void OnActivateButton1(bool _value)
//	{
//		switch (currentScreen)
//		{
//			case UiScreenName.OBJECTIVES:
//				ObjectivesVC.OnActivateWeekly(_value);
//				break;
//			case UiScreenName.SOCIAL:
//				leaderboardVC.SwitchToPanel(SocialScreenName.TopScores);	//leaderboardVC.OnActivateTopScores(_value);
//				break;	
//		}		
//	}
//	
//	public void OnActivateButton2(bool _value)
//	{
//		switch (currentScreen)
//		{
//			case UiScreenName.OBJECTIVES:
//				ObjectivesVC.OnActivateLegendary(_value);
//				break;
//			case UiScreenName.SOCIAL:
//				leaderboardVC.SwitchToPanel(SocialScreenName.TopDistances);	//leaderboardVC.OnActivateTopDistances(_value);
//				break;	
//		}				
//	}	
//	
//	public void OnActivateButton3(bool _value)
//	{
//		switch (currentScreen)
//		{
//			case UiScreenName.OBJECTIVES:
//				ObjectivesVC.OnActivateCompleted(_value);
//				break;
//			case UiScreenName.SOCIAL:
//				leaderboardVC.SwitchToPanel(SocialScreenName.Challenges);	//leaderboardVC.OnActivateChallenges(_value);
//				break;	
//		}	
//	}		
	
//		checkboxes[0].isChecked = (buttonIndex == 0) ? true : false;
//		checkboxes[1].isChecked = (buttonIndex == 1) ? true : false;
//		checkboxes[2].isChecked = (buttonIndex == 2) ? true : false;
//		checkboxes[3].isChecked = (buttonIndex == 3) ? true : false;		
			
	
	//-- TODO Support the special currency.	
	

	
//	public GameObject Button0 = null;	
//	public GameObject Button1 = null;	
//	public GameObject Button2 = null;	
//	public GameObject Button3 = null;	

					//NGUITools.SetActive(ButtonsMainMenu, true);	

			//NGUITools.SetActive(ButtonsInventory, true);

		//NGUITools.SetActive(ButtonsInventory, false);
		//ButtonsInventory.gameObject.SetActiveRecursively(false);


		//case "SettingsButton": viewController = settingsVC; break;
		//case "StatsButton": viewController = statsVC; break;	

		//NGUITools.SetActive(ButtonsMainMenu, false);	
		//NGUITools.SetActive(ButtonsInventory, false);
		//ButtonsMainMenu.gameObject.SetActiveRecursively(false);
		//ButtonsInventory.gameObject.SetActiveRecursively(false);
		
	
	
	//public GameObject ButtonsMainMenu = null;	
	//public GameObject ButtonsInventory = null;		
	