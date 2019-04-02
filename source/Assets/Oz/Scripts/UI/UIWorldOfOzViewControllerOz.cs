using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum WorldOfOzScreenName { Environments, Characters, sell , }


public class UIWorldOfOzViewControllerOz : UIViewControllerOz
{
	//public GameObject panel;
	
	public List<GameObject> worldOfOzPanelGOs = new List<GameObject>();
	public List<GameObject> tabGOs = new List<GameObject>();
	
	
	private WorldOfOzScreenName pageToLoad = WorldOfOzScreenName.Environments;
	
	private List<Vector3> tabGOSpriteScales = new List<Vector3>();	
		
	private TabSettings selectedTabSettings;
	private TabSettings deSelectedTabSettings;	
	private TabSettings sellTabSettings;
	
	private NotificationIcons notificationIcons;
	
	private SaleBanner saleBanner;
	
	protected override void Awake()
	{
		base.Awake();
		notificationIcons = gameObject.GetComponent<NotificationIcons>();
		
		selectedTabSettings = new TabSettings(tabGOs[0]);
		deSelectedTabSettings = new TabSettings(tabGOs[1]);
		sellTabSettings = new TabSettings(tabGOs[2]);
		
		// store initial icon sprite scale values, to pass into scaling function
		tabGOSpriteScales.Add(tabGOs[0].transform.Find("Sprite").localScale);
		tabGOSpriteScales.Add(tabGOs[1].transform.Find("Sprite").localScale);
		tabGOSpriteScales.Add(tabGOs[2].transform.Find("Sprite").localScale);
		
		
		tabGOs[0].transform.Find("Sprite").localScale *= TabSettings.tabScaleMultiplier;	// hack to make icon of selected tab scaled up on launch
	}
	
	public override void appear()
	{
		base.appear();		
		
		GameController.SharedInstance.MenusEntered();
		UIManagerOz.SharedInstance.PaperVC.SetCurrentPage(UIManagerOz.SharedInstance.worldOfOzVC);
		UIManagerOz.SharedInstance.PaperVC.SetPageName("Ttl_Woz", "");	
		UIManagerOz.SharedInstance.PaperVC.UpdateCurrency();
		
		SwitchToPanelOnEnter(pageToLoad);
		
		Refresh();
		
		//		SharingManagerBinding.SetCurrentScreenName( "world_of_oz" );
	}	
	
	public void Refresh()
	{
		// refresh appropriate panel prior to showing
		Services.Get<Store>().GetWeeklyDiscountManagerClass().ExpireDiscounts();
		
		switch (pageToLoad)
		{
			case WorldOfOzScreenName.Environments:
				worldOfOzPanelGOs[(int)WorldOfOzScreenName.Environments].GetComponent<UIWorldOfOzList>().Refresh();
				TurnOffSaleBanner();
				break;
			case WorldOfOzScreenName.Characters:
				worldOfOzPanelGOs[(int)WorldOfOzScreenName.Characters].GetComponent<UICharacterList>().Refresh();
				break;	
			case WorldOfOzScreenName.sell:
				worldOfOzPanelGOs[(int)WorldOfOzScreenName.sell].GetComponent<UISellList>().Refresh();
				break;
			
		}		
		
		Services.Get<NotificationSystem>().SetNotificationIconsForThisPage(UiScreenName.WORLDOFOZ);	
		
	//	panel.GetComponent<UIWorldOfOzList>().Refresh();
	}	
	
	public void OnButtonClick(GameObject button)
	{	
		switch (button.name)
		{
			case "Tab0":						// powerups button
			//				AnalyticsInterface.LogNavigationActionEvent( "Lands", "WorldOfOz", "Lands" );
			
				SwitchToPanel(WorldOfOzScreenName.Environments);
				TurnOffSaleBanner();
			//	UIManagerOz.SharedInstance.PaperVC.SetPageName("Ttl_Store", "Ttl_Sub_Powerups");				
				break;	
			
			case "Tab1":						// modifiers button
			//	AnalyticsInterface.LogNavigationActionEvent( "Characters", "WorldOfOz", "Characters" );
			
				Services.Get<NotificationSystem>().ClearCharactersNewFeatureNotification();
				SwitchToPanel(WorldOfOzScreenName.Characters);
			//	UIManagerOz.SharedInstance.PaperVC.SetPageName("Ttl_Store", "Ttl_Sub_Modifiers");
				break;
			
			case "Tab2":						// modifiers button
				//AnalyticsInterface.LogNavigationActionEvent( "Characters", "WorldOfOz", "Characters" );
			
				Services.Get<NotificationSystem>().ClearCharactersNewFeatureNotification();
				SwitchToPanel(WorldOfOzScreenName.sell);
			//	UIManagerOz.SharedInstance.PaperVC.SetPageName("Ttl_Store", "Ttl_Sub_Modifiers");
				break;
		}
	}

	
	private void SetActivePage()
	{
		foreach (GameObject go in worldOfOzPanelGOs)
			NGUITools.SetActive(go, false);
		
		NGUITools.SetActive(worldOfOzPanelGOs[(int)pageToLoad], true);		
		
		worldOfOzPanelGOs[(int)pageToLoad].GetComponent<UIDraggablePanel>().ResetPosition();
	}
	
	private void SwitchToPanel(WorldOfOzScreenName panelScreenName)	// activate panel upon button selection, passing in UpgradesScreenName
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
	
	public void SwitchToPanelOnEnter(WorldOfOzScreenName panelScreenName)	// activate panel upon button selection, passing in UpgradesScreenName
	{
		float tweenTime = 0.001f;
		
		selectedTabSettings.ScaleTab(tabGOs[(int)panelScreenName], selectedTabSettings, tweenTime, true,
			tabGOSpriteScales[(int)panelScreenName]);	// scale selected tab up
		
		for (int i=0; i<3; i++)
		{
			if (i != (int)panelScreenName)
				deSelectedTabSettings.ScaleTab(tabGOs[i], deSelectedTabSettings, tweenTime, false,
					tabGOSpriteScales[i]);	// scale other three tabs down
		}
		
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
	
	public void SetNotificationIcon(int buttonID, int iconValue)		// update actual icon onscreen
	{
		if(notificationIcons!=null)
			notificationIcons.SetNotification(buttonID, iconValue);
	}	
	
	public void OnNeedMoreCoinsNo() // use in main menu area only, goes to full store
	{
		DisconnectHandlers();
	}
	
	public void OnNeedMoreCoinsYes() // use in main menu area only, goes to full store
	{
		DisconnectHandlers();
		disappear();
		UIManagerOz.SharedInstance.inventoryVC.LoadThisPageNextTime(UpgradesScreenName.MoreCoins);
		UIManagerOz.SharedInstance.inventoryVC.appear();
	}	
	
	private void DisconnectHandlers()
	{
		UIConfirmDialogOz.onNegativeResponse -= OnNeedMoreCoinsNo;
		UIConfirmDialogOz.onPositiveResponse -= OnNeedMoreCoinsYes;	
	}
	
	public void LoadPageCharacters(WorldOfOzScreenName page)
	{
		pageToLoad = page;
	}
	
	private void TurnOffSaleBanner()
	{
		
		if ( gameObject.GetComponentsInChildren<SaleBanner>( true ) != null
			&& gameObject.GetComponentsInChildren<SaleBanner>( true ).Count() > 0
		) {
			if (saleBanner == null)
			{
				saleBanner = gameObject.GetComponentsInChildren<SaleBanner>(true).First();
			}
	
			// set status of sale banner (show only if sale is active)
			//saleBanner.SetSaleBannerStatus(gameObject.GetComponent<UIPanel>(), 
			//	transform.parent.gameObject, DiscountItemType.StoreItem, saleBanner);
			saleBanner.gameObject.active = false;
		}
	}	
}


