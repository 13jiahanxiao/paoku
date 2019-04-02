using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PopupNotificationType
{
	Generic,
	Objective,
}

public class PopupNotification : MonoBehaviour 
{
	protected static Notify notify;
	public Transform bgXform;
	public Transform otherSprite;
	//wxj
	public Transform actiSprite;
	
	public PopupNotificationType type = PopupNotificationType.Generic;
	private Vector3 UpPosition, DownPosition;
	private Hashtable onHash, offHash;
	private UILabel popupLabel;
	private float timer = 0.0f;
	private const float timeOut = 2.0f;
	//private float yDistanceDiff = 0.0f;
	
	private int MessageBoardLastDistance = 0;
	private const int EarlyMessageBoardSpan = 250;
	private const int LateMessageBoardSpan = 500;
	private const int EarlyToLateDistance = 1249;
	
	public static Dictionary<PopupNotificationType,PopupNotification> PopupList = new Dictionary<PopupNotificationType,PopupNotification>();
	
	// Prevent Check Neighbors from quashing other popups
	private static bool showingPopup = false;
	
	public void OnLanguageChanged(string language)
	{
		SetCachedStrings();
	}
	
	private string meterAb;
	private string entering;
	private string welcometo;
	private string turn;
	private string directionRight;
	private string directionLeft;
	
	private void SetCachedStrings()
	{//Localization
		meterAb = Localization.SharedInstance.Get ("Lbl_MeterAb");
		entering = Localization.SharedInstance.Get ("Lbl_Entering");
		welcometo = Localization.SharedInstance.Get ("Lbl_WelcomeTo");
		turn = Localization.SharedInstance.Get ("Loc_Turn");
		directionRight = Localization.SharedInstance.Get ("Loc_Right");
		directionLeft = Localization.SharedInstance.Get ("Loc_Left");
	}
	
	void Awake()
	{
		notify = new Notify(this.GetType().Name);
		Services.Register("PopupNotification", this);
		
		if(popupLabel==null)
			popupLabel = gameObject.transform.Find("PopupLabel").GetComponent<UILabel>();
		
	}
	
	void Start()
	{
		SetCachedStrings();
		Localization.RegisterForOnLanguageChanged(OnLanguageChanged);
		if(gameObject.GetComponent<UIPanel>() == null)
		{
			notify.Warning("No UIPanel on popup " + gameObject.name);
		}
		
		if(popupLabel==null)
			popupLabel = gameObject.transform.Find("PopupLabel").GetComponent<UILabel>();	// get reference to text label	
		
		if(!PopupList.ContainsKey(type))
			PopupList.Add(type,this);
		
	//	if(type==PopupNotificationType.Generic)
	//	{
			DownPosition = new Vector3(0.0f, 0, 0.0f);					// set 'down' Y position
			UpPosition = new Vector3(0.0f, 140f, 0.0f);		// set 'up' Y position
			
	//	}
	//	else if(type==PopupNotificationType.Objective)
	//	{
	//		DownPosition = new Vector3(0.0f, 0, 0.0f);					// set 'down' Y position
	//		UpPosition = new Vector3(0.0f, 140f, 0.0f);		// set 'up' Y position
	//	}
		
		onHash = iTween.Hash("position", DownPosition, "isLocal", true, "time", 0.25f);		
		offHash = iTween.Hash("position", UpPosition, 
			"isLocal", true, 
			"time", 0.25f,
			"oncomplete", "HideComplete",
			"oncompletetarget", gameObject
			);
		/*
		if(Screen.width > 768) {// ipad retina
			DownPosition *= 0.5f;
			UpPosition *= 0.5f;
		}
		if(Screen.width <=320) {// iphone 3GS
			DownPosition *= 2f;
			UpPosition *= 2f;
		}
		*/
		//Hide the popup
		transform.localPosition = UpPosition;							// move popup off screen
		//yDistanceDiff = (float)Screen.height - 375.0f;
		SetBGandLabelActiveRecursively(false);
		
		
		//GameController.SharedInstance.RegisterForOnPlayerTurn(PlayerTurned);
		//BonusItem.RegisterForOnBonusItemPickup(BonusItemPickedUp);
		if(type==PopupNotificationType.Generic)
		{
			EnvironmentSetSwitcher.SharedInstance.RegisterForOnEnvironmentStateChange(EnvironmentStateChanged);	
			GamePlayer.SharedInstance.RegisterForOnTrackPieceChange(ChangedOnTrackPiece);	
		}
	}
	
	void Update()
	{
		float diff = Time.time - timer;
		if(diff > 0.5f)
		{
			if(type==PopupNotificationType.Generic) //&& !GameController.SharedInstance.debugUsed)
				CheckDistance();
			
			CheckNeighbors(); // eyal disabled since we are doing now in the UIingameViewControllerOz
			timer = Time.time;
		}

	}
	
	bool isFadedOut = false;
	private int fadeCount = 0;
	public void FadeOut()
	{
		fadeCount++;
	//	Debug.Log("FADE OUT! New fade count: "+fadeCount);
		isFadedOut = true;
		TweenAlpha.Begin(gameObject, 0.15f, 0f);
	}
	public void FadeIn()
	{
		fadeCount--;
	//	Debug.Log("FADE IN! New fade count: "+fadeCount);
		if(fadeCount<=0)
		{
			isFadedOut = false;
			TweenAlpha.Begin(gameObject, 0.15f, 1f);
		}
	}
	
	public void ResetDistance()
	{
		MessageBoardLastDistance = 0;
	}
	
	public void CheckDistance()
	{
		int dist = (int)GameController.SharedInstance.DistanceTraveled;
		
		if (dist < EarlyToLateDistance && dist >= MessageBoardLastDistance + EarlyMessageBoardSpan) 
		{
			Show((MessageBoardLastDistance + EarlyMessageBoardSpan).ToString() + meterAb,true);
			MessageBoardLastDistance = MessageBoardLastDistance + EarlyMessageBoardSpan;
		}
		else if (dist >= MessageBoardLastDistance + LateMessageBoardSpan) 
		{
			Show((MessageBoardLastDistance + LateMessageBoardSpan).ToString() + meterAb,true);
			MessageBoardLastDistance = MessageBoardLastDistance + LateMessageBoardSpan;
		}
		else
		if(!showingPopup && gameObject.active)
		{
			SetBGandLabelActiveRecursively(false);
		}
	}
	
	private void SetBGandLabelActiveRecursively(bool active)
	{
		popupLabel.gameObject.SetActiveRecursively(active);
		bgXform.gameObject.SetActiveRecursively(active);
		if(otherSprite)	otherSprite.gameObject.SetActiveRecursively(active);
		//wxj
		if(actiSprite)	actiSprite.gameObject.SetActiveRecursively(active);
	}

	//Check your friends best distance, and display that you passed them.
	public void CheckNeighbors()
	{
		if(GameController.SharedInstance.IsTutorialMode)
			return;
		if(!Initializer.SharedInstance.GetDisplayLeaderboard())
			return;
		
		int distance = (int) GameController.SharedInstance.DistanceTraveled;
		
		//ProfileManager userProfileManager = Services.Get<ProfileManager>();
		
		
		
		// If no other popup is being displayed.
//		if (!showingPopup && userProfileManager.userServerData!=null && userProfileManager.userServerData._neighborList.Count > 0) 

		if (!showingPopup
			&& ProfileManager.SharedInstance != null
			&& ProfileManager.SharedInstance.userServerData != null
			&& ProfileManager.SharedInstance.userServerData._neighborList.Count >0
		) {
			//Cycle through neighbors.
//			foreach(NeighborProtoData neighborData in userProfileManager.userServerData._neighborList)
			foreach(NeighborProtoData neighborData in ProfileManager.SharedInstance.userServerData._neighborList)
			{
				//Don't display neighbors that you have already passed for this bootup.
				// Neighbors with best meters of 0 are set to be automatically passed.
				
				
				//TO DO, persist that neighbors have been passed, unless they've beaten their previous
				//record
				if (!neighborData._passedNeighbor && distance > neighborData._bestMeters)
				{
					notify.Debug("Passed neighbor: " + neighborData._dbId);
					
					
					
					/* 02-09-13 Deactivate saving of passed friends */
					/* 03-18-13 Disabling this feature */
					neighborData._passedNeighbor = true;
					
					/*
					userProfileManager.userServerData.AddPassedNeighbor(neighborData._dbId, neighborData._bestMeters);
					*/
					
					//TO DO: Localize Passed.  Remove Passed ID.
					if (neighborData._name != "")
					{
						//Show ("Passed: " + neighborData._name);
						UIManagerOz.SharedInstance.ShowFriendScoreLabel(neighborData._name);
					}
//					else
//					{
//						Show ("Passed id: " + neighborData._dbId.Substring(1, 5));
//					}
					break;
				}
			}
		}
	}
	
	public void Show(string textToShow, bool isDistance = false)
	{
		SetBGandLabelActiveRecursively(true);
		StartCoroutine(ShowQueued(textToShow,isDistance));
	}
	
	public static void EnableNotifications(bool on)
	{
		notificationsEnabled = on;
	}
	private static bool notificationsEnabled = true;
	
	int distance_id = 0;
	private IEnumerator ShowQueued(string textToShow, bool isDistance = false)
	{		
		notify.Debug("Showing: "+ textToShow + " " +type + " " + notificationsEnabled + " " + showingPopup + " " + isFadedOut + " " + BonusButtons.isDisplaying);
		
		int my_id = 0;
		if(isDistance)
			my_id = ++distance_id;
		
		if(!notificationsEnabled)	yield break;
		
		if(isDistance && UIManagerOz.SharedInstance.inGameVC.showingEnvProgress)
		{
			yield break;
		}
		
		if(showingPopup)
		{
			//Debug.Log(textToShow + " " + showingPopup + " " + isFadedOut + " " + BonusButtons.isDisplaying);
			while(showingPopup || isFadedOut || BonusButtons.isDisplaying)	
			{
				if(isDistance && my_id!=distance_id) {
					yield break;
				}
				yield return null;
			}
			
			if(isDistance && UIManagerOz.SharedInstance.inGameVC.showingEnvProgress)
			{
				yield break;
			}
			
			//Set this back to true, in case there are other notifications waiting for this
			showingPopup = true;
			yield return new WaitForSeconds(0.25f);
		}
		
		//Finally, if another distance popup got called, skip this one.
		if(isDistance && my_id!=distance_id) {
			showingPopup = false;
			yield break;
		}
		
		showingPopup = true;
		
	//	if(UIManagerOz.SharedInstance.inGameVC.showingEnvProgress)	
	//	{
		UIManagerOz.SharedInstance.inGameVC.FadeOutEnvProgress();	//We want to do this even if its not showing, in case it trys to show up when this popup is still down
		if(UIManagerOz.SharedInstance.inGameVC.showingEnvProgress)
			yield return new WaitForSeconds(0.15f);		//...but we only have to wait for it to fade out if it is currently on screen.
	//	}
		if(isDistance && my_id!=distance_id) {
			showingPopup = false;
			yield break;
		}
		
		///// Now we can start to show the popup!
		
		SetBGandLabelActiveRecursively(true);
		
		
		if(AudioManager.SharedInstance!=null)
		{
			if(type==PopupNotificationType.Objective)
				AudioManager.Instance.PlayFX(AudioManager.Effects.oz_UI_WeeklyChallenge_01);	// play sound	
			else
				AudioManager.Instance.PlayFX(AudioManager.Effects.oz_UI_Menu_back);	// play sound	
		}
		
		SetBGandLabelActiveRecursively(true);
		
		
		string adjustedstring = textToShow.Replace("\\n","\n");
		string[] stringparts = adjustedstring.Split('\n');
		int maxLineSize = 0;
		for(int i=0;i<stringparts.Length;i++)
		{
			if(stringparts[i].Length > maxLineSize)
				maxLineSize = stringparts[i].Length;
		}
	//	Debug.Log("Text: "+adjustedstring+" Lines: "+stringparts.Length + " Total: "+adjustedstring.Length+" Max: "+maxLineSize);
		
		
		float textSize = isDistance ? 70f : 40f;
		popupLabel.transform.localScale = new Vector3(textSize,textSize,1);
		popupLabel.text = string.Format("{0}", textToShow);			// update text field
		
		float scaleX = (7 + maxLineSize) * textSize/3f;
		
		string lang = Localization.SharedInstance.GetLoadedLanguage();
		if(lang.Contains("Korean") || lang.Contains("Japanese") || lang.Contains("Chinese"))
			popupLabel.transform.localScale*=0.7f;
		
		if(otherSprite!=null)	scaleX += otherSprite.transform.localScale.x+60f;
		bgXform.localScale = new Vector3(scaleX, bgXform.localScale.y, 1f);
		
		if(otherSprite!=null)
		{
			Vector3 temp = otherSprite.transform.localPosition;
			temp.x = scaleX/2f - otherSprite.transform.localScale.x/2f - 10f;
			otherSprite.transform.localPosition = temp;
		}
		
		// wxj
		if(actiSprite!=null)
		{
			Vector3 temp = actiSprite.transform.localPosition;
			temp.x = scaleX/2f - actiSprite.transform.localScale.x/2f - 20f;
			actiSprite.transform.localPosition = temp;
		}
		
		
		iTween.MoveTo(gameObject, onHash);	//Move down!
		
		yield return new WaitForSeconds(2f);
			
		iTween.MoveTo(gameObject, offHash);	//Move up!
		
		yield return new WaitForSeconds(0.15f);
		
		UIManagerOz.SharedInstance.inGameVC.FadeInEnvProgress();
	}
	
	public void SetOtherSprite(string spriteName)
	{
		// wxj
		setActivitySprite(false);
		
		if(otherSprite!=null)
		{
			otherSprite.GetComponent<UISprite>().spriteName = spriteName;
		}
	}
	
	//wxj, call when activity objectives completed
	public void setActivitySprite(bool active)
	{
		if(otherSprite != null)
			otherSprite.GetComponent<UISprite>().enabled = !active;
		if(actiSprite != null)
			actiSprite.GetComponent<UISprite>().enabled = active;
	}
	
	private void HideComplete()
	{
		SetBGandLabelActiveRecursively(false);
		showingPopup = false;
	}


	private void EnvironmentStateChanged(EnvironmentSetSwitcher.SwitchState newState, int destinationEnvironmentId)
	{
		if (newState == EnvironmentSetSwitcher.SwitchState.waitingToBeAbleToDeletePools)	// popup notification for entering tunnel
		{
			//Show("Entering " + EnvironmentSetManager.SharedInstance.LocalDict[destinationEnvironmentId].GetLocalizedTitle());
			string loc = EnvironmentSetManager.SharedInstance.LocalDict[destinationEnvironmentId].GetLocalizedTitle();
			string final = string.Format(entering,loc);
			Show(final);
		}
		else if (newState == EnvironmentSetSwitcher.SwitchState.finished)	// popup notification for exiting tunnel and entering new environment
		{
			//Show("Welcome to " + EnvironmentSetManager.SharedInstance.LocalDict[destinationEnvironmentId].GetLocalizedTitle());
			string loc = EnvironmentSetManager.SharedInstance.LocalDict[destinationEnvironmentId].GetLocalizedTitle();
			string final = string.Format(welcometo,loc);
			Show(final);
		}			
	}
	
	private void ChangedOnTrackPiece(TrackPiece oldTrackPiece, TrackPiece newTrackPiece)
	{
		if(newTrackPiece == null)
			return;
		if (newTrackPiece.NextTrackPiece != null)
		{
			if (newTrackPiece.NextTrackPiece.NextTrackPiece != null)
			{
				if (newTrackPiece.NextTrackPiece.NextTrackPiece.TrackType == TrackPiece.PieceType.kTPEnvSetJunction)
				{
					TrackPiece junctionPiece = newTrackPiece.NextTrackPiece.NextTrackPiece;
					
					// GetComponentInChildren can fail if the junction piece is inactive, which happens in dark forest
					GameObject deciderGo = HierarchyUtils.GetChildByName("TransitionSignDecider", junctionPiece.gameObject);
					if (deciderGo == null)
					{
						notify.Error("could not find TransitionSignDecider game object");
						return;
					}
					TransitionSignDecider decider = deciderGo.GetComponent<TransitionSignDecider>();
					if (decider == null)
					{
						notify.Error("no TransitionSignDecider script in the envset junction");
						return;
					}
					if(EnvironmentSetManager.SharedInstance.LocalDict.ContainsKey(decider.DestinationId))
					{
						string target = EnvironmentSetManager.SharedInstance.LocalDict[decider.DestinationId].GetLocalizedTitle();
						string direction = (decider.MainLeftGoesToTransitionTunnel)?directionLeft:directionRight;
						Show(string.Format(turn, direction, target));
					}
				}
			}
		}
	}
}


	
//	public void PlayerTurned(bool isLeft) // not used in oz
//	{
//		string direction = (isLeft) ? "left" : "right";
//		Show("You turned " + direction + "!");
//	}
	
//	public void BonusItemPickedUp(BonusItem.BonusItemType bonusItemType)
//	{
		//Not used anymore.
		// popup notification for all bonus item pickups except coins
	/*	if (bonusItemType != BonusItem.BonusItemType.Coin && 
			bonusItemType != BonusItem.BonusItemType.CoinDouble &&
			bonusItemType != BonusItem.BonusItemType.CoinTriple)
		{
			Show(bonusItemType.ToString());
		}*/
//	}
	


//	public void ShowDistanceMeterWithDistance(int distance) 
//	{
//		if (isDistanceMeterOnScreen == true) { return; }	// || DistanceLabel == null || distanceMeter == null)
//
//		UpPosition.y = Screen.height;
//		DownPosition.y = 375;//Screen.height * 0.5f;
//		
//		NGUITools.SetActive(distanceMeter.gameObject, true);
//		distanceMeter.localPosition = UpPosition;
//		DistanceLabel.text = string.Format("{0}m", distance);
//		StartCoroutine(AnimateMessageBoard());
//	}
//	
//	public IEnumerator AnimateMessageBoard()
//	{
//		AudioManager.Instance.PlayFX(AudioManager.Effects.oz_UI_Menu_back);
//		distanceMeter.localPosition = UpPosition;
//		isDistanceMeterOnScreen = true;
//		TweenPosition.Begin(distanceMeter.gameObject, 0.25f, DownPosition);
//		yield return new WaitForSeconds(2);
//		
//		TweenPosition outTp = TweenPosition.Begin(distanceMeter.gameObject, 0.25f, UpPosition);
//		outTp.eventReceiver = this.gameObject;
//		outTp.callWhenFinished = "DistanceMeterFinishHideAnimation";
//	}
//	
//	public void DistanceMeterFinishHideAnimation() 
//	{
//		isDistanceMeterOnScreen = false;
//		NGUITools.SetActive(distanceMeter.gameObject, false);
//	}		
//}
//
//		//if (distanceMeter == null) { return; }
//

		//bool isPopupOnScreen = false;
	//private GameObject popupGO;	//public Transform distanceMeter = null;
			//distanceMeter = gameObject.transform;
	
