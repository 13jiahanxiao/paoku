using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// wxj, add activity
public enum ObjectivesScreenName { Activity, Weekly, Legendary, Team} //Current, Completed, }

public class UIObjectivesViewControllerOz : UIViewControllerOz
{
	private ObjectivesScreenName pageToLoad = ObjectivesScreenName.Activity;		
	
	public List<GameObject> objectivesPanelGOs = new List<GameObject>();	
	public List<GameObject> tabGOs = new List<GameObject>();	
	public List<UIObjectivesList> objectivesPanelUILists = new List<UIObjectivesList>();	
	
	private TabSettings selectedTabSettings;
	private TabSettings deSelectedTabSettings;
	
	private List<Vector3> tabGOSpriteScales = new List<Vector3>();
	
	private NotificationIcons notificationIcons;
	
	public UILabel ClientUpdate;
	
	public UILabel TeamClientUpdate;

	protected override void Awake()
	{
		base.Awake();
		notificationIcons = gameObject.GetComponent<NotificationIcons>();
		
//		foreach (GameObject go in objectivesPanelGOs)
//			go.AddComponent<UIPanelAlpha>();
		
		selectedTabSettings = new TabSettings(tabGOs[0]);
		deSelectedTabSettings = new TabSettings(tabGOs[1]);
		deSelectedTabSettings = new TabSettings(tabGOs[2]);
		deSelectedTabSettings = new TabSettings(tabGOs[3]);
		
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
		//SharingManagerBinding.ShowBurstlyBannerAd( "challenges", true );
		
		base.appear();		
		
		Services.Get<NotificationSystem>().SetNotificationIconsForThisPage(UiScreenName.OBJECTIVES);
		
		UIManagerOz.SharedInstance.PaperVC.SetPageName("Ttl_Objectives", "Ttl_Sub_WklyChallenges");
		UIManagerOz.SharedInstance.PaperVC.SetCurrentPage(UIManagerOz.SharedInstance.ObjectivesVC);
		//SwitchToPanel(ObjectivesScreenName.Weekly);	// jonoble replaced with "SwitchToPanelOnEnter()"
		SwitchToPanelOnEnter(pageToLoad); // jonoble added: Allow deep-linking to take us directly to any tab
		SetTabColliders();
		
		//SharingManagerBinding.SetCurrentScreenName( "challenges" );
	}

	public void OnDisable()
	{
		//SharingManagerBinding.ShowBurstlyBannerAd( "challenges", false );
	}
	
	public void RefreshWeeklyObjectivesList() 	
	{
		UIObjectivesList weeklyList = objectivesPanelGOs[(int)ObjectivesScreenName.Weekly].transform.Find("DraggablePanel").GetComponent<UIObjectivesList>();
		weeklyList.PopulateWeeklyObjectiveData();
		weeklyList.Refresh();
	}
	
	public void RefreshTeamObjectivesList()
	{
		UIObjectivesList teamList = objectivesPanelGOs[(int)ObjectivesScreenName.Team].transform.Find( "DraggablePanel").GetComponent<UIObjectivesList>();
		teamList.PopulateWeeklyObjectiveData();
		teamList.Refresh();
	}
	
	private void SetTabColliders()	// only use this on initial page entry, afterwards the scaling takes care of collider state
	{
		foreach (GameObject tabGO in tabGOs)
			tabGO.GetComponent<BoxCollider>().enabled = true;
		
		tabGOs[(int)pageToLoad].GetComponent<BoxCollider>().enabled = false;
	}			
	
	public void LoadThisPageNextTime(ObjectivesScreenName page)
	{
		pageToLoad = page;
		//SwitchToPanel(pageToLoad);
	}
	
	private void Refresh() 	
	{
		objectivesPanelGOs[(int)pageToLoad].transform.Find("DraggablePanel").GetComponent<UIObjectivesList>().Refresh();
		Services.Get<NotificationSystem>().SetNotificationIconsForThisPage(UiScreenName.OBJECTIVES);
	}	
	
	public void SwitchToPanelOnEnter(ObjectivesScreenName panelScreenName)	// activate panel upon button selection, passing in UpgradesScreenName
	{
		float tweenTime = 0.001f;
		
		selectedTabSettings.ScaleTab(tabGOs[(int)panelScreenName], selectedTabSettings, tweenTime, true,
			tabGOSpriteScales[(int)panelScreenName]);	// scale selected tab up
		
		// wxj
		for (int i=0; i<=3; i++)
		{
			if (i != (int)panelScreenName)
				deSelectedTabSettings.ScaleTab(tabGOs[i], deSelectedTabSettings, tweenTime, false,
					tabGOSpriteScales[i]);	// scale other two tabs down
		}
		
		if (panelScreenName == ObjectivesScreenName.Weekly)
		{
			if ( Initializer.IsBuildVersionPassThreshold() || 
				WeeklyObjectives.WeeklyObjectiveCount == 0)
//				Services.Get<ObjectivesManager>().GetWeeklyObjectives().Count == 0)
			{
				ClientUpdate.enabled = false;
			}
			else
			{
				ClientUpdate.enabled = true;
			}		
		}
		if ( panelScreenName == ObjectivesScreenName.Team )
		{
			TeamClientUpdate.enabled = false;
		}
		
		SetActivePage();
		Refresh();
	}
	
	private void SwitchToPanel(ObjectivesScreenName panelScreenName)	// activate panel upon button selection, passing in ObjectivesScreenName
	{
		if (panelScreenName != pageToLoad)
		{
			ScaleTab((int)pageToLoad, false);		
			ScaleTab((int)panelScreenName, true);
		}
		
		pageToLoad = panelScreenName;		
				
		SetActivePage();
		Refresh();
		
		if (pageToLoad == ObjectivesScreenName.Weekly)
		{
			if ( Initializer.IsBuildVersionPassThreshold() || 
				WeeklyObjectives.WeeklyObjectiveCount == 0)
//				Services.Get<ObjectivesManager>().GetWeeklyObjectives().Count == 0)
			{
				ClientUpdate.enabled = false;
			}
			else
			{
				ClientUpdate.enabled = true;
			}		
		}
		if ( pageToLoad == ObjectivesScreenName.Team )
		{
			if ( Initializer.IsBuildVersionPassThreshold() ||
				WeeklyObjectives.TeamObjectiveCount == 0
			) {
				TeamClientUpdate.enabled = false;
			}
			else
			{
				TeamClientUpdate.enabled = false;
			}
		}
		
		if (pageToLoad == ObjectivesScreenName.Weekly && objectivesPanelUILists[(int)pageToLoad].IsInitialized)
		{
			Services.Get<NotificationSystem>().ClearOneShotNotification(OneShotNotificationType.NewWeeklyChallenge);
		}
		
		if ( pageToLoad == ObjectivesScreenName.Team && objectivesPanelUILists[ (int) pageToLoad].IsInitialized )
		{
			Services.Get<NotificationSystem>().ClearOneShotNotification( OneShotNotificationType.NewTeamChallenge );
		}
	}
	
	private void SetActivePage()
	{
	// set only appropriate panel active, make others inactive
		foreach (GameObject go in objectivesPanelGOs)
			NGUITools.SetActive(go, false);
		
		NGUITools.SetActive(objectivesPanelGOs[(int)pageToLoad], true);	
			
		/*
		if (!Initializer.SharedInstance.IsBuildVersionPassThreshold() && displayOkayDialog
			&& pageToLoad == ObjectivesScreenName.Weekly)
		{
			UIManagerOz.SharedInstance.okayDialog.ShowOkayDialog("Msg_UpgradeClient", "Btn_Ok");
			displayOkayDialog = false;
		}
		*/
		
		if (pageToLoad == ObjectivesScreenName.Legendary)		
			objectivesPanelGOs[(int)pageToLoad].transform.Find("DraggablePanel").GetComponent<UIDraggablePanel>().ResetPosition();
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
			// wxj, activity
			case "Tab0":						
			//				AnalyticsInterface.LogNavigationActionEvent( "Activity", "Challenges", "Challenges-Activity" );
			
				SwitchToPanel(ObjectivesScreenName.Activity);
				UIManagerOz.SharedInstance.PaperVC.SetPageName("Ttl_Objectives", "Ttl_Sub_Activity");
				break;
			
			// wxj, weekly
			case "Tab1":						
			//	AnalyticsInterface.LogNavigationActionEvent( "Weekly", "Challenges", "Challenges-Weekly" );
			
				SwitchToPanel(ObjectivesScreenName.Weekly);
				UIManagerOz.SharedInstance.PaperVC.SetPageName("Ttl_Objectives", "Ttl_Sub_WklyChallenges");	
				break;
			
			// wxj, legendary
			case "Tab2":						
			//.LogNavigationActionEvent( "Legendary", "Challenges", "Challenges-Legendary" );
 
				SwitchToPanel(ObjectivesScreenName.Legendary);
				UIManagerOz.SharedInstance.PaperVC.SetPageName("Ttl_Objectives", "Ttl_Sub_Legendary");	
				break;
						
			// wxj, team
			case "Tab3":						
			//AnalyticsInterface.LogNavigationActionEvent( "Team", "Challenges", "Challenges-Team" );
			
				SwitchToPanel(ObjectivesScreenName.Team);
				UIManagerOz.SharedInstance.PaperVC.SetPageName("Ttl_Objectives", "Ttl_Sub_Team");	
				break;
			// TO DO Add third case for team challenges.  Loc String is "Ttl_Sub_Team"
		}
		
		//SetActivePage();
		//Refresh();
	}	
	
	public void SetNotificationIcon(int buttonID, int iconValue)		// update actual icon onscreen
	{
		notificationIcons.SetNotification(buttonID, iconValue);
	}
	
	public void OnInviteClick()
	{
		
		 
	}
	
	private void OnFacebookLoginNoButtonPressed()
	{
		UIFacebookDialogOz.onNegativeResponse -= OnFacebookLoginNoButtonPressed;
		UIFacebookDialogOz.onPositiveResponse -= OnFacebookLoginYesButtonPressed;
	}
	
	// "Log in to Facebook?" dialog box
	private void OnFacebookLoginYesButtonPressed()
	{
		UIFacebookDialogOz.onNegativeResponse -= OnFacebookLoginNoButtonPressed;
		UIFacebookDialogOz.onPositiveResponse -= OnFacebookLoginYesButtonPressed;
		
		//SharingManagerBinding.FacebookLogin();
	}
	
	public bool IsOnTeamPage()
	{
		if ( pageToLoad == ObjectivesScreenName.Team )
		{
			return true;
		}
		return false;
	}
}



		
		//if (initDone)
		//	ResetScrollListsToTop();
		//ResetScrollListPanelAlphasToZero();		
		//SetActivePage();
		//Refresh();
		//initDone = true;		


	//public List<GameObject> graphicsGOs = new List<GameObject>();
	//public List<GameObject> labelGOs = new List<GameObject>();
	
	
	//private NotificationSystem notificationSystem;	
	
	
	//private bool initDone = false;
	
	//public UILabel rankLabel; - remove no longer need N.N.
	//public UISlider rankProgress;	- remove no longer need N.N.
		
	


		
//		if (objectivesPanelUILists[(int)pageToLoad].IsInitialized)
//		{
//			if (pageToLoad == ObjectivesScreenName.Weekly)
//			{
//				Services.Get<NotificationSystem>().ClearOneShotNotification(OneShotNotificationType.NewWeeklyChallenge);
//				//Services.Get<NotificationSystem>().ClearOneShotNotification(OneShotNotificationType.WeeklyChallengeCompleted);
//			}
//			//else if (pageToLoad == ObjectivesScreenName.Legendary)
//				//Services.Get<NotificationSystem>().ClearOneShotNotification(OneShotNotificationType.LegendaryChallengeCompleted);
//		}



//	private void ResetScrollListPanelAlphasToZero()
//	{
//		foreach (GameObject go in objectivesPanelGOs)
//			go.GetComponent<UIPanelAlpha>().alpha = 0f;
//	}		
//	
//	private void FadeInNextPanel(UITweener ta)
//	{
//		TweenAlpha.Begin(objectivesPanelGOs[(int)pageToLoad], 0.2f, 1f);
//		ta.onFinished -= FadeInNextPanel;
//	}	
	

	
//	private void ResetScrollListsToTop()
//	{
//		foreach (GameObject go in objectivesPanelGOs)
//			go.transform.Find("DraggablePanel").GetComponent<UIDraggablePanel>().ResetPosition();
//	}		
	


		
		// refresh panels prior to showing
//		foreach (GameObject go in objectivesPanelGOs)
//		{
//			Show(go);	//NGUITools.SetActive(go, true);	// activate panel first, so refresh works correctly every time
//			go.GetComponent<UIObjectivesList>().Refresh();		
//		}		
//		
//		// set only appropriate panel active, make others inactive
//		foreach (GameObject go in objectivesPanelGOs)
//			Hide(go);	//NGUITools.SetActive(go, false);
//		
//		Show(objectivesPanelGOs[(int)pageToLoad]);	//    NGUITools.SetActive(objectivesPanelGOs[(int)pageToLoad], true);			
		
//		rankLabel.text = "Level " + GameProfile.SharedInstance.Player.GetCurrentRank().ToString(); - remove no longer need N.N.
//		rankProgress.sliderValue = Mathf.Clamp01(GameProfile.SharedInstance.Player.GetCurrentRankProgress());	//-- Just to be safe, clamp from 0 to 1.
		
		//paperViewController.ResetTabs((int)pageToLoad);		// reset highlighted tab to the one actually chosen	


		
	// set only appropriate panel active, make others inactive
//		foreach (GameObject go in objectivesPanelGOs)
//			NGUITools.SetActive(go, false);	//FadeOut(1f, null, go);
		//foreach (GameObject go in labelGOs)
		//	NGUITools.SetActive(go, false);	//FadeOut(1f, null, go);								
		
//		NGUITools.SetActive(objectivesPanelGOs[(int)pageToLoad], true);	//FadeIn(1f, null, graphicsGOs[(int)pageToLoad]);
		//NGUITools.SetActive(labelGOs[(int)pageToLoad], true);		//FadeIn(1f, null, labelGOs[(int)pageToLoad]);		
	
//		foreach (GameObject go in objectivesPanelGOs)
//		{
//			TweenAlpha ta = TweenAlpha.Begin(go, 0.1f, 0f);
//			
//			if (go == objectivesPanelGOs[0])
//				ta.onFinished += FadeInNextPanel;	
//		}
	
//	public void OnEnable()
//	{
//	//	notificationSystem.SetNotificationIconsToThisPage(UiScreenName.OBJECTIVES);		
//		UIManagerOz.SharedInstance.PaperVC.SetPageName("Ttl_Objectives", "Ttl_Sub_WklyChallenges");
//		UIManagerOz.SharedInstance.PaperVC.SetCurrentPage(UIManagerOz.SharedInstance.ObjectivesVC);
//		if (initDone)
//			ResetScrollListsToTop();
//		//ResetScrollListPanelAlphasToZero();
//		//SwitchToPanel(pageToLoad);
//		SetTabColliders();
//		//SetActivePage();
//		//Refresh();
//		initDone = true;	
//	}	
	

	
//	private void SetTabColliders()
//	{
//		foreach (GameObject tabGO in tabGOs)
//			tabGO.GetComponent<BoxCollider>().enabled = true;
//		
//		tabGOs[pageToLoad].GetComponent<BoxCollider>().enabled = false;
//	}		
	


		//UIManagerOz.SharedInstance.PaperVC.OnButtonClick(button);
		
		//if (currentScreen == UiScreenName.OBJECTIVES)// objectives screen buttons	
		//{


		//	nextViewController = ObjectivesVC;
		//}		
		
	
//	public override void Start()
//	{
//		base.Start();
//	}


	//		currentPanel.GetComponent<UIObjectivesList>().Refresh();
//		weeklyPanel.GetComponent<UIObjectivesList>().Refresh();
//		legendaryPanel.GetComponent<UIObjectivesList>().Refresh();		
//		completedPanel.GetComponent<UIObjectivesList>().Refresh();		
		
	

//		NGUITools.SetActive(currentPanel, true);
//		NGUITools.SetActive(weeklyPanel, true);
//		NGUITools.SetActive(legendaryPanel, true);		
//		NGUITools.SetActive(completedPanel, true);	
		

//	public GameObject currentPanel;		// current active objectives
//	public GameObject weeklyPanel;		// weekly challenges
//	public GameObject legendaryPanel;	// legendary objectives	
//	public GameObject completedPanel;	// completed objectives	
//	public GameObject activePanel;		// the panel that is currently selected (out of the four above)
		
		
		//activePanel = currentPanel;	//legendaryPanel;
		//ActivatePanel(activePanel);


		//activePanel = currentPanel;	// always show active objectives first when going back to this screen
		//ActivatePanel(activePanel);

		
//		currentCheckboxButton.isChecked = true;
//		weeklyCheckboxButton.isChecked = false;
//		legendaryCheckboxButton.isChecked = false;		
//		completedCheckboxButton.isChecked = false;	
	
//	public UICheckbox currentCheckboxButton;
//	public UICheckbox weeklyCheckboxButton;
//	public UICheckbox legendaryCheckboxButton;	
//	public UICheckbox completedCheckboxButton;		
	
//	public void ConnectCheckBoxes(List<UICheckbox> checkboxList)
//	{
//		currentCheckboxButton = checkboxList[0];
//		weeklyCheckboxButton = checkboxList[1];
//		legendaryCheckboxButton = checkboxList[2];
//		completedCheckboxButton = checkboxList[3];
//	}

//
//	private void ActivatePanel(GameObject _panel)
//	{
//		if (_panel == currentPanel)
//		{
//			NGUITools.SetActive(currentPanel, true);
//			NGUITools.SetActive(weeklyPanel, false);
//			NGUITools.SetActive(legendaryPanel, false);
//			NGUITools.SetActive(completedPanel, false);			
//		}
//		else if (_panel == weeklyPanel)
//		{
//			NGUITools.SetActive(currentPanel, false);
//			NGUITools.SetActive(weeklyPanel, true);	
//			NGUITools.SetActive(legendaryPanel, false);
//			NGUITools.SetActive(completedPanel, false);			
//		}
//		else if (_panel == legendaryPanel)
//		{
//			NGUITools.SetActive(currentPanel, false);
//			NGUITools.SetActive(weeklyPanel, false);
//			NGUITools.SetActive(legendaryPanel, true);
//			NGUITools.SetActive(completedPanel, false);	
//		}		
//		else if (_panel == completedPanel)
//		{
//			NGUITools.SetActive(currentPanel, false);
//			NGUITools.SetActive(weeklyPanel, false);
//			NGUITools.SetActive(legendaryPanel, false);
//			NGUITools.SetActive(completedPanel, true);	
//		}		
//		
//		activePanel = _panel;
//	}		
//
//	public void OnActivateLocal(bool _value)
//	{
//		NGUITools.SetActive(currentPanel, _value);
//		if (_value) { activePanel = currentPanel; }
//	}	
//	
//	public void OnActivateWeekly(bool _value)
//	{
//		NGUITools.SetActive(weeklyPanel, _value);
//		if (_value) { activePanel = weeklyPanel; }
//	}
//	
//	public void OnActivateLegendary(bool _value)
//	{
//		NGUITools.SetActive(legendaryPanel, _value);
//		if (_value) { activePanel = legendaryPanel; }
//	}	
//	
//	public void OnActivateCompleted(bool _value)
//	{
//		NGUITools.SetActive(completedPanel, _value);
//		if (_value) { activePanel = completedPanel; }
//	}	
//}

	
	//public UIInGameViewControllerOz inGameVC = null;	
	//public static event voidClickedHandler onPlayClickedHandler = null;
//	
//	public void OnPlayClicked() 
//	{
//		//Debug.LogWarning("OnPlayClicked in UIObjectivesViewControllerOz!");
//		
//		if (MainGameCamera != null) { MainGameCamera.enabled = true; }
//		disappear();
//		if (inGameVC != null) { inGameVC.appear(); } 
//		if (onPlayClickedHandler != null) { onPlayClickedHandler(); }	//-- Notify an object that is listening for this event.
//	}		
	


	//float tweenTime = 0.3f;

		//Vector3 scale = _value ? Vector3.one : Vector3.zero;
		//iTween.ScaleTo(generalPanel, scale, tweenTime);
	
	//void OnEnable() 
	//{
	//	ActivatePanel(activePanel);
	//}
	