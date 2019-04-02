using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum UpgradesScreenName { PowerUps, Artifacts, Consumables, MoreCoins, }	//Stats, }

public class UIInventoryViewControllerOz : UIViewControllerOz
{
	public List<GameObject> inventoryPanelGOs = new List<GameObject>();	
	//public List<UIPanelAlpha> faders = new List<UIPanelAlpha>();		
	//public List<GameObject> graphicsGOs = new List<GameObject>();
	//public List<GameObject> labelGOs = new List<GameObject>();	
	public List<GameObject> tabGOs = new List<GameObject>();		
	
	// wxj
	public GameObject storeScroll;
	
	private TabSettings selectedTabSettings;
	private TabSettings deSelectedTabSettings;	
	
	private List<Vector3> tabGOSpriteScales = new List<Vector3>();		
	
	private UpgradesScreenName pageToLoad = UpgradesScreenName.PowerUps;	
	
	//private NotificationSystem notificationSystem;
	private NotificationIcons notificationIcons;	
	
	//private SaleBanner saleBanner;
	
	
	//private bool initDone = false;

	protected override void Awake() 
	{
		base.Awake();
		notificationIcons = gameObject.GetComponent<NotificationIcons>();
		
		selectedTabSettings = new TabSettings(tabGOs[0]);
		deSelectedTabSettings = new TabSettings(tabGOs[1]);
		
		// store initial icon sprite scale values, to pass into scaling function
		tabGOSpriteScales.Add(tabGOs[0].transform.Find("Sprite").localScale);
		tabGOSpriteScales.Add(tabGOs[1].transform.Find("Sprite").localScale);
		tabGOSpriteScales.Add(tabGOs[2].transform.Find("Sprite").localScale);
		tabGOSpriteScales.Add(tabGOs[3].transform.Find("Sprite").localScale);

		tabGOs[0].transform.Find("Sprite").localScale *= TabSettings.tabScaleMultiplier;	// hack to make icon of selected tab scaled up on launch
	}	
	
	protected override void Start() 
	{ 
		base.Start();
		//notificationSystem = Services.Get<NotificationSystem>();
	}	

	public override void appear() 
	{
		base.appear();
		
		GameController.SharedInstance.MenusEntered();
		UIManagerOz.SharedInstance.PaperVC.SetPageName("Ttl_Store", "");	
		UIManagerOz.SharedInstance.PaperVC.SetCurrentPage(UIManagerOz.SharedInstance.inventoryVC);
		UIManagerOz.SharedInstance.PaperVC.UpdateCurrency();

		//if (initDone)
		ResetScrollListsToTop();		
		SwitchToPanelOnEnter(pageToLoad);
		SetTabColliders();
		//initDone = true;
		//Services.Get<NotificationSystem>().SetNotificationIconsForThisPage(UiScreenName.UPGRADES);	

		//SharingManagerBinding.ShowBurstlyBannerAd( "store", true );
		//		SharingManagerBinding.SetCurrentScreenName( "store" );
	}
	
	public void OnEnable()
	{
	//	notificationSystem.SetNotificationIconsToThisPage(UiScreenName.UPGRADES);		
//		ResetScrollListPanelAlphasToZero();
//		SwitchToPanelOnEnter(pageToLoad);
		//SwitchToPanel(pageToLoad);
	}
	
	public void OnDisable()
	{
		//SharingManagerBinding.ShowBurstlyBannerAd( "store", false );
	}
	
	private void SetTabColliders()	// only use this on initial page entry, afterwards the scaling takes care of collider state
	{
		foreach (GameObject tabGO in tabGOs)
			tabGO.GetComponent<BoxCollider>().enabled = true;
		
		tabGOs[(int)pageToLoad].GetComponent<BoxCollider>().enabled = false;
	}			
	
	public void LoadThisPageNextTime(UpgradesScreenName page)
	{
		pageToLoad = page;
		//SwitchToPanel(pageToLoad);
	}
	
	public void Refresh() 	
	{
		// refresh appropriate panel prior to showing
		Services.Get<Store>().GetWeeklyDiscountManagerClass().ExpireDiscounts();
		
		switch (pageToLoad)
		{
			case UpgradesScreenName.PowerUps:
				inventoryPanelGOs[(int)UpgradesScreenName.PowerUps].GetComponent<UIPowersList>().Refresh();
				break;
			case UpgradesScreenName.Artifacts:
				inventoryPanelGOs[(int)UpgradesScreenName.Artifacts].GetComponent<UIArtifactsList>().Refresh();
				break;			
			case UpgradesScreenName.Consumables:
				inventoryPanelGOs[(int)UpgradesScreenName.Consumables].GetComponent<UIConsumablesList>().Refresh();
				break;
			case UpgradesScreenName.MoreCoins:
				if (UIStoreList.storeLoaded == false)					// request product list from store
					inventoryPanelGOs[(int)UpgradesScreenName.MoreCoins].GetComponent<UIStoreList>().RequestStoreList();	//Refresh();
				else if (UIStoreList.fullStoreScrollListGenerated == false)	// generate scroll list
					inventoryPanelGOs[(int)UpgradesScreenName.MoreCoins].GetComponent<UIStoreList>().GenerateScrollList();
				else
					inventoryPanelGOs[(int)UpgradesScreenName.MoreCoins].GetComponent<UIStoreList>().Refresh();	
				TurnOffSaleBanner();
				break;			
		}		
		
		Services.Get<NotificationSystem>().SetNotificationIconsForThisPage(UiScreenName.UPGRADES);	
		//paperViewController.ResetTabs((int)pageToLoad);			// reset highlighted tab to the one actually chosen		
	}

	private void SetActivePage()
	{
//		foreach (GameObject go in inventoryPanelGOs)
//		{
//			TweenAlpha ta = TweenAlpha.Begin(go, 0.1f, 0f);
//			
//			if (go == inventoryPanelGOs[0])
//				ta.onFinished += FadeInNextPanel;	
//		}
		
		foreach (GameObject go in inventoryPanelGOs)
			NGUITools.SetActive(go, false);
		
		NGUITools.SetActive(inventoryPanelGOs[(int)pageToLoad], true);	
		
		inventoryPanelGOs[(int)pageToLoad].GetComponent<UIDraggablePanel>().ResetPosition();
		
	// set only appropriate panel active, make others inactive
		//for (int i=0; i < inventoryPanelGOs.Count; i++)		
		//	faders[i].alpha = 0f;	
		
		//inventoryPanelGOs[(int)pageToLoad].GetComponent<UIPanelAlpha>().alpha = 1f;
		//TweenAlpha.Begin(inventoryPanelGOs[(int)pageToLoad], 0.3f, 1f);
		
		if ((int)pageToLoad <= 2)
			Services.Get<MenuTutorials>().SendEvent((int)pageToLoad);	// for menu tutorials
		
		
		// wxj
		if(pageToLoad == UpgradesScreenName.MoreCoins)
			NGUITools.SetActive(storeScroll, true);
		else
			NGUITools.SetActive(storeScroll, false);
	}
	
	public void ResetScrollListsToTop()
	{
		foreach (GameObject go in inventoryPanelGOs)
			go.GetComponent<UIDraggablePanel>().ResetPosition();
	}

	public void SwitchToPanelOnEnter(UpgradesScreenName panelScreenName)	// activate panel upon button selection, passing in UpgradesScreenName
	{
		float tweenTime = 0.001f;
		
		selectedTabSettings.ScaleTab(tabGOs[(int)panelScreenName], selectedTabSettings, tweenTime, true,
			tabGOSpriteScales[(int)panelScreenName]);	// scale selected tab up
		
		for (int i=0; i<=3; i++)
		{
			if (i != (int)panelScreenName)
				deSelectedTabSettings.ScaleTab(tabGOs[i], deSelectedTabSettings, tweenTime, false,
					tabGOSpriteScales[i]);	// scale other three tabs down
		}
		
		SetActivePage();
		Refresh();
	}		
	
	private void SwitchToPanel(UpgradesScreenName panelScreenName)	// activate panel upon button selection, passing in UpgradesScreenName
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
			case "Tab0":						// powerups button
			//				AnalyticsInterface.LogNavigationActionEvent( "Powerups", "Store", "Store-Powerups" );
			
				SwitchToPanel(UpgradesScreenName.PowerUps);
				UIManagerOz.SharedInstance.PaperVC.SetPageName("Ttl_Store", "Ttl_Sub_Powerups");				
				break;	
			
			case "Tab1":						// modifiers button
			//	AnalyticsInterface.LogNavigationActionEvent( "Abilities", "Store", "Store-Abilities" );
			
				SwitchToPanel(UpgradesScreenName.Artifacts);
				UIManagerOz.SharedInstance.PaperVC.SetPageName("Ttl_Store", "Ttl_Sub_Modifiers");
				break;				
			case "Tab2":						// consumables button
			//AnalyticsInterface.LogNavigationActionEvent( "Utilities", "Store", "Store-Utilities" );
			
				SwitchToPanel(UpgradesScreenName.Consumables);
				UIManagerOz.SharedInstance.PaperVC.SetPageName("Ttl_Store", "Ttl_Sub_Consumables");				
				break;	
			
			case "Tab3":						// more coins button
			
			//AnalyticsInterface.LogNavigationActionEvent( "More Coins", "Store", "Store-More Coins" );
			
				SwitchToPanel(UpgradesScreenName.MoreCoins);
				UIManagerOz.SharedInstance.PaperVC.SetPageName("Ttl_Store", "Lbl_MoreCoins");		
				break;
		}
	}	
	
	public void OnNeedMoreCoinsNo() // use in main menu area only, goes to full store
	{
		DisconnectHandlers();
	}
	
	public void OnNeedMoreCoinsYes() // use in main menu area only, goes to full store
	{
		DisconnectHandlers();
		SwitchToPanel(UpgradesScreenName.MoreCoins);	// send player to store
	}	
	
	private void DisconnectHandlers()
	{
		UIConfirmDialogOz.onNegativeResponse -= OnNeedMoreCoinsNo;
		UIConfirmDialogOz.onPositiveResponse -= OnNeedMoreCoinsYes;	
	}
	
	public void SetNotificationIcon(int buttonID, int iconValue)		// update actual icon onscreen
	{
		if(notificationIcons!=null)
			notificationIcons.SetNotification(buttonID, iconValue);
	}	
	
	private void TurnOffSaleBanner()
	{
		/*
		if (saleBanner == null)
		{
			saleBanner = gameObject.GetComponentsInChildren<SaleBanner>(true).First();
		}

		// set status of sale banner (show only if sale is active)
		//saleBanner.SetSaleBannerStatus(gameObject.GetComponent<UIPanel>(), 
		//	transform.parent.gameObject, DiscountItemType.StoreItem, saleBanner);
		saleBanner.gameObject.active = false;
		
		*/
	}
}




	
//	private void ResetScrollListPanelAlphasToZero()
//	{
//		foreach (GameObject go in inventoryPanelGOs)
//			go.GetComponent<UIPanelAlpha>().alpha = 0f;
//	}			
//	
//	private void FadeInNextPanel(UITweener ta)
//	{
//		TweenAlpha.Begin(inventoryPanelGOs[(int)pageToLoad], 0.2f, 1f);
//		ta.onFinished -= FadeInNextPanel;
//	}
	

		
		//UIConfirmDialogOz.ClearEventHandlers();		
		//notify.Info ("FAKE BUY IAP FOR 1000 coins");
		//GameProfile.SharedInstance.Player.coinCount += 1000;
		//updateCurrency();
				//UIManagerOz.SharedInstance.PaperVC.GoToStore(ShopScreenName.Coins);

		//UIConfirmDialogOz.ClearEventHandlers();
		//UIConfirmDialogOz.onNegativeResponse -= OnNeedMoreCoinsNo;
		//UIConfirmDialogOz.onPositiveResponse -= OnNeedMoreCoinsYes;

		
		//SetActivePage();
		//Refresh();

//		// set only appropriate panel active, make others inactive
//		foreach (GameObject go in inventoryPanelGOs)
//			Hide(go);	//NGUITools.SetActive(go, false);
//		
//		Show(inventoryPanelGOs[(int)pageToLoad]);	//NGUITools.SetActive(inventoryPanelGOs[(int)pageToLoad], true);	
		

			//SwitchToPanel(pageToLoad);
		//SetActivePage();
		//Refresh();
	//	initDone = true;	
		
		//if (pageToLoad == UpgradesScreenName.MoreCoins)
		//	storePanelGOs[(int)pageToLoad].GetComponent<UIStoreList>().RequestStoreList();


		//for (int i=0; i < inventoryPanelGOs.Count; i++)
		//	faders[i] = inventoryPanelGOs[i].AddComponent<UIPanelAlpha>();
		
//		foreach (GameObject go in inventoryPanelGOs)
//			go.AddComponent<UIPanelAlpha>();
		

		
//		Vector3 dest = (bigger) ? Vector3.zero : new Vector3(0.0f, -6.862183f, 0.0f);
		
//		iTween.MoveTo(tabGOs[index].transform.Find("BackgroundFrame").gameObject, iTween.Hash(
//			"position", dest,
//			"islocal", true,
//			"time", 0.3f,
//			"easetype", iTween.EaseType.easeOutBack));	
//			
//			iTween.MoveTo(tabGOs[index].transform.Find("BackgroundSprite").gameObject, iTween.Hash(
//			"position", dest,
//			"islocal", true,
//			"time", 0.3f,
//			"easetype", iTween.EaseType.easeOutBack));	


//	private void ResetScrollListsToTop()
//	{
		//if (inventoryPanelGOs[0].transform.Find("powerGrid").gameObject.GetComponentsInChildren<UIWidget>().Length != 0)
		//{
//			foreach (GameObject go in inventoryPanelGOs)
//				go.GetComponent<UIDraggablePanel>().ResetPosition();
				//go.GetComponent<UIDraggablePanel>().SetDragAmount(0.0f, 0.0f, false);
		//}
//	}


	//public UICharacterSelectViewControllerOz characterSelectVC = null;

		//	nextViewController = inventoryVC;
		//}
		
		//UIManagerOz.SharedInstance.PaperVC.OnButtonClick(button);	
		
		//if (currentScreen == UiScreenName.UPGRADES)	// inventory screen buttons	
		//{

	//Ttl_Upgrades", "Ttl_Sub_Stats");		

				//inventoryPanelGOs[(int)UpgradesScreenName.Stats].GetComponent<UIStatsList>().Refresh();			//.Stats:

//	public void UpdateCurrency()
//	{
//		this.updateCurrency();
//	}	
//}
	

//	public void OnClosePanel()
//	{
//		ShowObject(this.gameObject, false, true);	
//	}




		//if (paperViewController != null) { paperViewController.SetPlayButtonCallback(this.gameObject, "OnPlayClicked"); }
		
	
//	public override void OnPlayClicked() 
//	{
//		if (characterSelectVC != null) { characterSelectVC.disappear(); } 
//		base.OnPlayClicked();		
//	}


//		NGUITools.SetActive(artifactPanel, (pageToLoad == UpgradesScreenName.Artifacts) ? true : false);	
//		NGUITools.SetActive(powerupPanel, (pageToLoad == UpgradesScreenName.PowerUps) ? true : false);
//		NGUITools.SetActive(statsPanel, (pageToLoad == UpgradesScreenName.Stats) ? true : false);
//		NGUITools.SetActive(consumablePanel, (pageToLoad == UpgradesScreenName.Consumables) ? true : false);	
		
	
//	public GameObject artifactPanel = null;
//	public GameObject powerupPanel = null;
//	public GameObject statsPanel = null;
//	public GameObject consumablePanel = null;

	
//	public override void OnBackButton() 
//	{
//		base.OnBackButton();
//		ShowObject(this.gameObject, false, true);
//		characterSelectVC.previousViewController = UIManagerOz.SharedInstance.mainVC;
//		characterSelectVC.SetButtons(true);
//		paperViewController.SetBackButtonCallback(characterSelectVC.gameObject, "OnBackButton");
//	}
	
	
	//public UIInGameViewControllerOz inGameVC = null;
	//public static event voidClickedHandler onPlayClickedHandler = null;
//		
//	public void OnPlayClicked() 		
//	{
//		Debug.LogWarning("OnPlayClicked in UIInventoryViewControllerOz!");
//		
//		if (MainGameCamera != null) { MainGameCamera.enabled = true; }
//		if (characterSelectVC != null) { characterSelectVC.disappear(); } 
//		disappear();
//		if (inGameVC != null) { inGameVC.appear(); } 
//		if (onPlayClickedHandler != null) { onPlayClickedHandler(); }	//-- Notify an object that is listening for this event.
//	}	
	
//	public Transform artifactPanel = null;
//	public Transform powerupPanel = null;
//	public Transform statsPanel = null;
//	public UIGrid artifactGrid = null;
//	public UIGrid powerupGrid = null;

//	
//	private void UpdateCellData(GameObject newCell, string title, string iconName, string description, string coinValue, bool purchased, bool equipped)
//	{
//		GameObject go = HierarchyUtils.GetChildByName("Title", newCell);
//		if (go != null) 
//		{
//			UILabel titleLabel = go.GetComponent<UILabel>() as UILabel;
//			if (titleLabel != null) { titleLabel.text = title; }//titleLabel.color = ArtifactStore.colorForRarity(protoData._rarity);
//		}
//		
//		go = HierarchyUtils.GetChildByName("Icon", newCell);
//		if (go != null) 
//		{
//			UISprite iconSprite = go.GetComponent<UISprite>() as UISprite;
//			if (iconSprite != null) { iconSprite.spriteName = iconName; }
//		}
//		
//		go = HierarchyUtils.GetChildByName("Description", newCell);
//		if (go != null) 
//		{
//			UILabel desc = go.GetComponent<UILabel>() as UILabel;
//			if (desc != null) { desc.text = description; }
//		}
//		
//		go = HierarchyUtils.GetChildByName("Cost", newCell);
//		if (go != null) 
//		{
//			GameObject buyButton = HierarchyUtils.GetChildByName("BuyButton", newCell);
//			GameObject CoinDisplayIcon = HierarchyUtils.GetChildByName("CoinDisplayIcon", newCell);
//			UILabel cost = go.GetComponent<UILabel>() as UILabel;
//			if (cost != null)
//			{
//				if (purchased == true) 
//				{
//					if(CoinDisplayIcon != null) { NGUITools.SetActive(CoinDisplayIcon, false); }
//					
//					if (equipped == true) 
//					{
//						cost.text = "equipped";
//						if (buyButton != null) { NGUITools.SetActive(buyButton, false); }
//					}
//					else 
//					{
//						cost.text = "available";	//equip";
//						if (buyButton != null) { NGUITools.SetActive(buyButton, true); }
//					}
//				}
//				else 
//				{
//					if (buyButton != null) { NGUITools.SetActive(buyButton, true); }
//					cost.text = coinValue;
//					if (CoinDisplayIcon != null) { NGUITools.SetActive(CoinDisplayIcon, true); }
//				}
//			}
//		}
//	}



	//private bool currentFilterIsPowerups = false;
	//private bool currentFilterIsArtifacts = false;
	//private bool currentFilterIsStats = false;
	
//	private int equippedArtifact = -1;
//	private int equippedPower = -1;
//	
//	public void SetEquippedArtifact(int artifactID) 
//	{
//		TR.LOG ("SetEquippedArtifact {0}", artifactID);
//		equippedArtifact = artifactID;
//		equippedPower = -1;
//		UpdateEquippedCell("Pick an Artifact");
//	}
//	
//	public void SetEquippedPower(int powerID) 
//	{
//		equippedPower = -1;
//		equippedPower = powerID;
//		UpdateEquippedCell("Pick a Powerup");
//	}	
//	


		//if (currentFilterIsArtifacts || currentFilterIsPowerups) { UpdateEquippedCell(); }		
		//if (currentFilterIsPowerups == true) { UpdatePowerups(); }
		//else if (currentFilterIsArtifacts == true) { UpdateArtifacts(); }

		
		//currentFilterIsArtifacts = (_name == ScreenName.Artifacts) ? true : false;
		//currentFilterIsPowerups = (_name == ScreenName.PowerUps) ? true : false;		
		//currentFilterIsStats = (_name == ScreenName.Stats) ? true : false;

		//public ArtifactSlotType equipInSlot = ArtifactSlotType.Total;

	//public Transform protoInventoryCell = null;
	
		
		//if(paperViewController != null) { paperViewController.ShowBackButton(false); }		
		//NGUITools.SetActive(artifactPanel.gameObject, !currentFilterIsPowerups);
		//NGUITools.SetActive(statsPanel.gameObject, false);
		

//	public void OnNeedMoreCoinsNo() {
//		UIConfirmDialogOz.onNegativeResponse -= OnNeedMoreCoinsNo;
//		UIConfirmDialogOz.onPositiveResponse -= OnNeedMoreCoinsYes;
//	}
//	
//	public void OnNeedMoreCoinsYes() {
//		UIConfirmDialogOz.onNegativeResponse -= OnNeedMoreCoinsNo;
//		UIConfirmDialogOz.onPositiveResponse -= OnNeedMoreCoinsYes;
//		TR.LOG ("FAKE BUY IAP FOR 10 gems");
//		GameProfile.SharedInstance.Player.specialCurrencyCount += 10;
//		updateCurrency();
//	}
	

		//disappear();
		//characterSelectVC.previousViewController = UIManagerOz.SharedInstance.mainVC;
		//paperViewController.SetBackButtonCallback(characterSelectVC.gameObject, "OnBackButton");
		//characterSelectVC.wantPaperViewController = true;
		//characterSelectVC.disappear();
		//disappear();
		//UIManagerOz.SharedInstance.mainVC.appear();

	
//	public void ShowArtifacts() 
//	{
//		currentFilterIsPowerups = false;
//		currentFilterIsArtifacts = true;
//		currentFilterIsStats = false;
//	}
//	public void ShowPowerups() 
//	{
//		currentFilterIsPowerups = true;
//		currentFilterIsArtifacts = false;
//		currentFilterIsStats = false;
//	}
//	public void ShowStats() 
//	{
//		currentFilterIsPowerups = false;
//		currentFilterIsArtifacts = false;
//		currentFilterIsStats = true;
//	}
