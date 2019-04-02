/*
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIPostGameViewController : UIViewController
{
	public enum State {
		ObjectiveFlyin,
		ObjectiveReward,
		Stats,
	}
	
	public const float cTransitionTime = 0.5f;
	
	public State CurrentState = State.ObjectiveFlyin;
	public static event voidClickedHandler 	onPlayClickedHandler = null;
	public UIViewController mainMenuVC = null;
	public UIViewController inGameVC = null;
	public UICharacterSelectViewController inCharacterSelectVC = null;
	public UILabel DeathMessageLabel = null;
	public UISprite DeathPortrait = null;
	
	public UILabel DistanceLabel = null;
	public UILabel CoinLabel = null;
	public UILabel GemLabel = null;
	public UILabel ScoreLabel = null;
	public UILabel RankLabel = null;
	public UILabel ShareLabel = null;
	public UISlider RankProgress = null;
	
	public Transform portraitRoot = null;
	public Transform objectivesRoot = null;
	public Transform statsRoot = null;
	public Transform objectivesTitle = null;
	public Transform objectiveOne = null;
	public Transform objectiveTwo = null;
	public Transform objectiveThree = null;
	public Transform backButton = null;
	public Transform upgradesButton = null;
	public Transform shareButton = null;
	
	public UISprite objecttiveOneCheckBox = null;
	public UISprite objecttiveTwoCheckBox = null;
	public UISprite objecttiveThreeCheckBox = null;
	
//	public List<Transform> objectiveOneContents = null;
//	public List<Transform> objectiveTwoContents = null;
//	public List<Transform> objectiveThreeContents = null;
	
	private Vector3 portraitStartPosition = Vector3.zero;
	private Vector3 objectivesRootStartPosition = Vector3.zero;
	private Vector3 statsRootStartPosition = Vector3.zero;
	
	private Vector3 objectiveOneStartPosition = Vector3.zero;
	private Vector3 objectiveTwoStartPosition = Vector3.zero;
	private Vector3 objectiveThreeStartPosition = Vector3.zero;	
	private Vector3 objectivesTitleStartPostion = Vector3.zero;
	private Vector3 rankLabelStartPosition = Vector3.zero;
	private Vector3 rankProgressStartPosition = Vector3.zero;
	
	
	
	
	public new void Start() {
		if(portraitRoot) portraitStartPosition = portraitRoot.localPosition;
		if(objectivesRoot) objectivesRootStartPosition = objectivesRoot.localPosition;
		if(statsRoot) statsRootStartPosition = statsRoot.localPosition;
		
		if(objectiveOne) objectiveOneStartPosition = objectiveOne.localPosition;
		if(objectiveTwo) objectiveTwoStartPosition = objectiveTwo.localPosition;
		if(objectiveThree) objectiveThreeStartPosition = objectiveThree.localPosition;
		
		if(objectivesTitle) objectivesTitleStartPostion = objectivesTitle.localPosition;
		
		if(RankLabel) rankLabelStartPosition = RankLabel.transform.localPosition;
		if(RankProgress) rankProgressStartPosition = RankProgress.transform.localPosition;
	}
	
	public override void appear() {
		
		base.appear();
		
		//-- DEBUG Set this on death. otherwise we will always restart the flow.
		//CurrentState = State.ObjectiveFlyin;
		
		if(CurrentState != State.Stats) {
			
			portraitRoot.localPosition = new Vector3(portraitStartPosition.x, Screen.height, portraitStartPosition.z);
			statsRoot.localPosition = new Vector3(Screen.width*3.0f, statsRootStartPosition.y, statsRootStartPosition.z);
			//objectivesRoot.localPosition = new Vector3(-Screen.width*3.0f, objectivesRootStartPosition.y, objectivesRootStartPosition.z);
			objectivesRoot.localPosition = objectivesRootStartPosition;
			
			TweenPosition tp = TweenPosition.Begin(portraitRoot.gameObject, cTransitionTime, portraitStartPosition);
			if(tp) {
				tp.method = UITweener.Method.EaseInOut;
			}
			
			objectivesTitle.localPosition = new Vector3(objectivesTitleStartPostion.x, Screen.height, objectivesTitleStartPostion.z);
			TweenPosition.Begin(objectivesTitle.gameObject, cTransitionTime, objectivesTitleStartPostion);
			
			if(RankLabel) {
				RankLabel.transform.localPosition = new Vector3(rankLabelStartPosition.x, Screen.height, rankLabelStartPosition.z);
				TweenPosition.Begin(RankLabel.gameObject, cTransitionTime, rankLabelStartPosition);	
			}
			
			if(RankProgress) {
				RankProgress.transform.localPosition = new Vector3(rankProgressStartPosition.x, Screen.height, rankProgressStartPosition.z);
				TweenPosition.Begin(RankProgress.gameObject, cTransitionTime, rankProgressStartPosition);	
			}
			
			NGUITools.SetActive(objecttiveOneCheckBox.gameObject, false);
			NGUITools.SetActive(objecttiveTwoCheckBox.gameObject, false);
			NGUITools.SetActive(objecttiveThreeCheckBox.gameObject, false);
			
			//-- Walk in reverse order, since the 3rd objectives animates in last.
			//-- We need to set at least one FLYIN IS OVER callback or else we need to call the callback directly.
			bool hasActive = false;
			//ObjectiveProtoData ob = null;
			if(oldObjectiveIds.Count > 2) {
				if(oldObjectiveIds[2] != -1) {
					objectiveThree.localPosition = new Vector3(-Screen.width*3.0f, objectiveThreeStartPosition.y, objectiveThreeStartPosition.z);
					tp = TweenPosition.Begin(objectiveThree.gameObject, cTransitionTime*1.75f, objectiveThreeStartPosition);
					if(tp) {
						tp.method = UITweener.Method.EaseInOut;
						tp.onFinished += OnObjectiveFlyinFinished;
						hasActive = true;
					}	
				}
				else {
					NGUITools.SetActive(objectiveThree.gameObject, false);
				}	
			}
			else {
				NGUITools.SetActive(objectiveThree.gameObject, false);
			}
			
			
			if(oldObjectiveIds.Count > 1) {
				if(oldObjectiveIds[1] != -1) {
					objectiveTwo.localPosition = new Vector3(-Screen.width*3.0f, objectiveTwoStartPosition.y, objectiveTwoStartPosition.z);
					tp = TweenPosition.Begin(objectiveTwo.gameObject, cTransitionTime*1.5f, objectiveTwoStartPosition);
					if(tp) {
						tp.method = UITweener.Method.EaseInOut;
						if(hasActive == false) {
							tp.method = UITweener.Method.EaseInOut;
							tp.onFinished += OnObjectiveFlyinFinished;
							hasActive = true;
						}
					}	
				}
				else {
					NGUITools.SetActive(objectiveTwo.gameObject, false);
				}	
			}
			else {
				NGUITools.SetActive(objectiveTwo.gameObject, false);
			}
			
			if(oldObjectiveIds.Count > 0) {
				if(oldObjectiveIds[0] != -1) {
					objectiveOne.localPosition = new Vector3(-Screen.width*3.0f, objectiveOneStartPosition.y, objectiveOneStartPosition.z);
					tp = TweenPosition.Begin(objectiveOne.gameObject, cTransitionTime*1.25f, objectiveOneStartPosition);
					if(tp) {
						tp.method = UITweener.Method.EaseInOut;
						if(hasActive == false) {
							tp.method = UITweener.Method.EaseInOut;
							tp.onFinished += OnObjectiveFlyinFinished;
							hasActive = true;
						}
					}	
				}
				else {
					NGUITools.SetActive(objectiveOne.gameObject, false);
				}
			}
			else {
				NGUITools.SetActive(objectiveOne.gameObject, false);
			}
			
			NGUITools.SetActive(statsRoot.gameObject, false);
			if(backButton != null) {
				NGUITools.SetActive(backButton.gameObject, false);	
			}
			if(upgradesButton != null) {
				NGUITools.SetActive(upgradesButton.gameObject, false);
			}
			if(shareButton != null) {
				NGUITools.SetActive(shareButton.gameObject, true);
				UIButtonMessage bm = shareButton.GetComponent<UIButtonMessage>() as UIButtonMessage;
				if(bm) {
					bm.functionName = "OnMoveToStatPage";
				}
				if(ShareLabel != null) {
					ShareLabel.text = "NEXT";
				}
			}
			
			//-- NO objectives left to earn, skip the the flyin.
			if(hasActive == false || completedIndices == null || completedIndices.Count == 0) {
				ShowStatsPage(true);
				//OnObjectiveFlyinFinished(null);
			}
		}
		else {
			ShowStatsPage(false);
		}
		
		if(paperViewController != null) {
			paperViewController.ShowPlayButton(false);
			paperViewController.ShowBackButton(false);
		}
	}
	
	private bool movingToStatPage = false;
	public void OnMoveToStatPage() {
		movingToStatPage = true;
		//-- Give rewards if we haven't done so yet.
		if(HaveGivenLevelRewards == false) {
			for(int i=0; i<(animatedRanks.Count-1); i++) {
				int oldrank = animatedRanks[i];
				int newrank = animatedRanks[i+1];	
				
				if(oldrank != newrank) {
					currentRewardType = GameProfile.SharedInstance.Player.GetRankRewardTypeForLevel(oldrank);
					currentRewardItemID = GameProfile.SharedInstance.Player.GetRankRewardQuanityOrItemForLevel(oldrank);	//, currentRewardType);
					GiveLevelRewards(currentRewardType, currentRewardItemID);	
				}
			}	
		}
		
		ShowStatsPage(true);
	}
	
	private void GiveLevelRewards(RankRewardType rewardType, int QtyOrItemID) {
		if(rewardType == RankRewardType.Coins) {
			GameProfile.SharedInstance.Player.coinCount += QtyOrItemID;
			this.updateCurrency();
		}
		else if(rewardType == RankRewardType.Gems) {
			GameProfile.SharedInstance.Player.specialCurrencyCount += QtyOrItemID;
			this.updateCurrency();
		}
		GameProfile.SharedInstance.Serialize();
		HaveGivenLevelRewards = true;
	}
	
	private void ShowStatsPage(bool animate) {
		CurrentState = State.Stats;
		
		NGUITools.SetActive(statsRoot.gameObject, true);
		portraitRoot.localPosition = portraitStartPosition;
		
		if(animate) {
			statsRoot.localPosition = new Vector3(Screen.width*3.0f, statsRootStartPosition.y, statsRootStartPosition.z);	
			TweenPosition.Begin(statsRoot.gameObject, cTransitionTime, statsRootStartPosition);
			TweenPosition.Begin(objectivesRoot.gameObject, cTransitionTime, new Vector3(-Screen.width*3.0f, objectivesRootStartPosition.y, objectivesRootStartPosition.z));
			TweenPosition.Begin(objectivesTitle.gameObject, cTransitionTime, new Vector3(-Screen.width*3.0f, objectivesTitleStartPostion.y, objectivesTitleStartPostion.z));
			if(RankLabel) {
				RankLabel.transform.localPosition = rankLabelStartPosition;
				TweenPosition.Begin(RankLabel.gameObject, cTransitionTime, new Vector3(-Screen.width*3.0f, rankLabelStartPosition.y, rankLabelStartPosition.z));	
			}
			if(RankProgress) {
				RankProgress.transform.localPosition = rankProgressStartPosition;
				TweenPosition.Begin(RankProgress.gameObject, cTransitionTime, new Vector3(-Screen.width*3.0f, rankProgressStartPosition.y, rankProgressStartPosition.z));	
			}
		}
		else {
			NGUITools.SetActive(objectivesRoot.gameObject, false);	
			statsRoot.localPosition = statsRootStartPosition;
		}
		
		if(backButton != null) {
			NGUITools.SetActive(backButton.gameObject, true);	
		}
		if(upgradesButton != null) {
			NGUITools.SetActive(upgradesButton.gameObject, true);
		}
		if(shareButton) {
//			 jonoble
//			if(TwitterBinding.isTweetSheetSupported() == true || 
//				FacebookBinding.isFacebookComposerSupported() == true) 
//			{
//				NGUITools.SetActive(shareButton.gameObject, true);	
//				UIButtonMessage bm = shareButton.GetComponent<UIButtonMessage>() as UIButtonMessage;
//				if(bm) {
//					bm.functionName = "OnShareButton";
//				}
//				
//				if(ShareLabel != null) {
//					ShareLabel.text = "SHARE";
//				}
//			}
//			else {
//				NGUITools.SetActive(shareButton.gameObject, false);
//			}
			
		}
		
		//-- Take screenshot
		StartCoroutine(TakeDelayedScreenShot());
	}
	
	IEnumerator TakeDelayedScreenShot ()
	{
		yield return new WaitForSeconds(cTransitionTime);
		yield return new WaitForEndOfFrame();
		notify.Debug ("Taking screenshot at "+Application.persistentDataPath + "/lastRun.png");
		Application.CaptureScreenshot("lastRun.png");
	}

	private Transform getObjectiveRootFromIndex(int index) {
		if(index < 0 || index >=3)
			return null;
		if(index == 0)
			return objectiveOne;
		else if(index == 1)
			return objectiveTwo;
		else if(index == 2)
			return objectiveThree;
		return null;
	}
	
	private Vector3 getStartingPositionForObjectiveRootFromIndex(int index) {
		if(index < 0 || index >=3)
			return Vector3.zero;
		if(index == 0)
			return objectiveOneStartPosition;
		else if(index == 1)
			return objectiveTwoStartPosition;
		else if(index == 2)
			return objectiveThreeStartPosition;
		return Vector3.zero;
	}
	
	private UISprite getCheckBoxForObjectiveRootFromIndex(int index) {
		if(index < 0 || index >=3)
			return null;
		if(index == 0)
			return objecttiveOneCheckBox;
		else if(index == 1)
			return objecttiveTwoCheckBox;
		else if(index == 2)
			return objecttiveThreeCheckBox;
		return null;
	}
	
	private int currentAwardIndex = 0;
	private List<int> animatedRanks = null;
	private List<float> animatedRankProgress = null;
	private List<int> oldObjectiveIds = null;
	private List<int> completedIndices = null;
	public bool DidComputeObjectives = false;
	private bool HaveGivenLevelRewards = false;
	public void ComputeCompletedObjectives() {
		//-- Only do this per Death.
		if(DidComputeObjectives == true)
			return;
		
		HaveGivenLevelRewards = false;
		movingToStatPage = false;
			
		int oldRank = GameProfile.SharedInstance.Player.GetCurrentRank();
		float oldProgress = GameProfile.SharedInstance.Player.GetCurrentRankProgress();
		if(animatedRanks == null) {
			animatedRanks = new List<int>();
		}
		animatedRanks.Clear();
		animatedRanks.Add (oldRank);
		
		if(animatedRankProgress == null) {
			animatedRankProgress = new List<float>();
		}
		animatedRankProgress.Clear ();
		animatedRankProgress.Add (oldProgress);
		
		if(oldObjectiveIds == null){
			oldObjectiveIds = new List<int>();
		}
		oldObjectiveIds.Clear ();
		
		if(completedIndices == null) {
			completedIndices = new List<int>();
		}
		completedIndices.Clear();
		
		int count = GameProfile.SharedInstance.Player.objectivesActive.Count;
		for(int i=0; i<count; i++) {
			ObjectiveProtoData ob = GameProfile.SharedInstance.Player.objectivesActive[i];
			if(ob == null) {
				notify.Error("We should never have a null here.");
				continue;
			}
			if(ob._title == null || ob._title.Length == 0)
			{
				oldObjectiveIds.Add (-1);	
			}
			else {
				oldObjectiveIds.Add (ob._id);	
			}
			
			//-- Completed
			if(ob._conditionList[0]._earnedStatValue >= ob._conditionList[0]._statValue) {
				if(GameProfile.SharedInstance.Player.objectivesEarned.Contains(ob._id) == false) {
					GameProfile.SharedInstance.Player.objectivesEarned.Add (ob._id);	
				}
				GameProfile.SharedInstance.Player.RefillObjectiveForIndex(i);	//, ob._conditionList[0]._statValue);	
				//-- OB has changed.
				completedIndices.Add (i);
			}
			
			int newRank = GameProfile.SharedInstance.Player.GetCurrentRank();
			float newProgress = GameProfile.SharedInstance.Player.GetCurrentRankProgress();
			animatedRanks.Add (newRank);
			if(oldRank != newRank) {
				animatedRankProgress.Add (1.0f);
				animatedRankProgress.Add (0.0f);
			}
			else {
				animatedRankProgress.Add (newProgress);
				animatedRankProgress.Add (newProgress);
			}
			oldRank = newRank;
		}
		DidComputeObjectives = true;
		currentAwardIndex = 0;
		GameProfile.SharedInstance.Serialize();
		
		CurrentState = State.ObjectiveFlyin;
//		if(completedIndices.Count == 0)
//			CurrentState = State.Stats;
	}
	
	//-- We have shown the objectives one first appearance OR
	//-- We have completed a cycle of CheckBox, move left, move in from right.
	//-- walk down the list of objectives until we have animated them all.
	public void OnObjectiveFlyinFinished (UITweener tween) {
		notify.Debug ("OnObjectiveFlyinFinished");
		if(tween != null) {
			tween.onFinished -= OnObjectiveFlyinFinished;	
		}
		CurrentState = State.ObjectiveReward;
		
		//-- Play back the changes rank.progress.objectives changes.
	 	for(int i=1; i<animatedRanks.Count; i++) {
			if(currentAwardIndex >= GameProfile.SharedInstance.Player.objectivesActive.Count)
				break;
			
			ObjectiveProtoData currentOb = GameProfile.SharedInstance.Player.objectivesActive[currentAwardIndex];
			int currentObID = -1;
			if(currentOb != null) {
				currentObID = currentOb._id;
			}
			
			//-- Move to the next objective if they haven't changes OR the current one is nil or empty.
			if(currentObID == oldObjectiveIds[currentAwardIndex] || oldObjectiveIds[currentAwardIndex] == -1) {
				currentAwardIndex++;
				continue;
			}
				
			//-- Animate the checkbox.
			UISprite checkBox = getCheckBoxForObjectiveRootFromIndex(currentAwardIndex);
			if(checkBox) {
				NGUITools.SetActive(checkBox.gameObject, true);
				TweenColor tc = TweenColor.Begin(checkBox.gameObject, 0.33f, new Color(255,255,255,0));
				tc.style = UITweener.Style.PingPong;
			}
			
			if(currentAwardIndex > 0) {
				checkBox = getCheckBoxForObjectiveRootFromIndex(currentAwardIndex-1);
				if(checkBox) {
					TweenColor tc = checkBox.GetComponent<TweenColor>() as TweenColor;
					if(tc) {
						tc.enabled = false;
					}
					checkBox.color = Color.white;
				}
			}
			
			AudioManager.SharedInstance.PlayFX(AudioManager.Effects.bonusMeterFull);
			
			//RankProgress.sliderValue = animatedRankProgress[(currentAwardIndex*2)];
			TweenSlider ts = null;
			//ts = TweenSlider.Begin(RankProgress.gameObject, 1.0f, animatedRankProgress[(currentAwardIndex*2)+1]);
			ts = TweenSlider.Begin(RankProgress.gameObject, 0.66f, RankProgress.sliderValue);
			ts.onFinished += OnObjectiveFlyinFinished;
			currentAwardIndex++;
			return;
		}
		
		//-- If we get here, time to animate the progressbar because we have checked off the completed objectives.
		notify.Debug ("READY TO PROGRESS");
		if(currentAwardIndex > 0) {
			UISprite checkBox = getCheckBoxForObjectiveRootFromIndex(currentAwardIndex-1);
			if(checkBox) {
				TweenColor tc = checkBox.GetComponent<TweenColor>() as TweenColor;
				if(tc) {
					tc.enabled = false;
				}
				checkBox.color = Color.white;
			}
		}
		
		//-- Now start ticking up the progressbar.
		currentAwardIndex = 0;
		RankProgress.sliderValue = animatedRankProgress[(currentAwardIndex*2)];
		TweenSlider tweenSlider = null;
		tweenSlider = TweenSlider.Begin(RankProgress.gameObject, 0.5f, animatedRankProgress[(currentAwardIndex*2)+1]);
		tweenSlider.onFinished += OnDoneAwardObjectiveProgress;
		tweenSlider.method = UITweener.Method.Linear;
	}
	
	//-- We have clicked "get reward", now move the completed objectives off to the left.
	//-- then move to repopulated the objectives data.
	public void OnGetRankReward() {
		//notify.Debug ("OnGetRankReward");
		UIConfirmDialog.onPositiveResponse -= OnGetRankReward;	
		Transform currentObectiveRoot = getObjectiveRootFromIndex(currentAwardIndex);
		//-- Animate offscreen, start the reward loop again.
		Vector3 offscreenLeft = new Vector3(-Screen.width*3.0f, currentObectiveRoot.localPosition.y, currentObectiveRoot.localPosition.z);
		TweenPosition tp = TweenPosition.Begin(currentObectiveRoot.gameObject, cTransitionTime*1.25f, offscreenLeft);
		if(tp) {
			tp.method = UITweener.Method.EaseInOut;
			tp.onFinished += OnDoneAwardObjectiveProgress;
		}
		SetRankProgress(0);
		AudioManager.SharedInstance.PlayFX(AudioManager.Effects.cashRegister);
		currentAwardIndex++;
		
		GiveLevelRewards(currentRewardType, currentRewardItemID);
	}
	
	//-- Repop the data for the new objectives and move in from the right.
	public void OnObjectiveAwardedFinished(UITweener tween) {
		//notify.Debug ("OnObjectiveAwardedFinished");
		if(movingToStatPage == true)
			return;
		Transform currentObectiveRoot = tween.gameObject.transform;//getObjectiveRootFromIndex(currentAwardIndex);
		int currentIndex = 0;
		if(currentObectiveRoot == objectiveTwo) {
			currentIndex = 1;
		}
		else if(currentObectiveRoot == objectiveThree) {
			currentIndex = 2;
		}
		
		Vector3 offscreenRight = new Vector3(Screen.width*3.0f, currentObectiveRoot.localPosition.y, currentObectiveRoot.localPosition.z);
		currentObectiveRoot.localPosition = offscreenRight;
		
		if(FillInObjectiveDataForIndex(currentIndex, GameProfile.SharedInstance.Player.objectivesActive[currentIndex]) == false) {
			NGUITools.SetActive(currentObectiveRoot.gameObject, false);
			return;
		}
		
		//-- Start fly in from right.
		Vector3 onscreen = getStartingPositionForObjectiveRootFromIndex(currentIndex);
		TweenPosition tp = TweenPosition.Begin(currentObectiveRoot.gameObject, cTransitionTime*1.25f, onscreen);
		if(tp) {
			tp.method = UITweener.Method.EaseInOut;
		}
	}
	
	//-- We have moved the completed objective to the left.
	//-- now see if we Leveled, pop the level reward if so.
	//-- other move onto pulling in the new objective.
	private RankRewardType 	currentRewardType = RankRewardType.Coins;
	private int				currentRewardItemID = -1;
	public void OnDoneAwardObjectiveProgress(UITweener tween) {
		//notify.Debug ("OnDoneAwardObjectiveProgress");
		if(tween) {
			tween.onFinished -= OnDoneAwardObjectiveProgress;	
		}
		
		if(movingToStatPage == true)
			return;
		
		if(currentAwardIndex < 3) {
			//Transform currentObectiveRoot = getObjectiveRootFromIndex(currentAwardIndex);
			
			int oldrank = animatedRanks[currentAwardIndex];
			int newrank = oldrank;
			if(animatedRanks.Count > (currentAwardIndex+1)) {
				newrank = animatedRanks[currentAwardIndex+1];
			}
			
			if(oldrank != newrank) {
				//-- LEVELED!
				//-- WE set this data so that if the user hits "next" to speed through the animations.
				//-- we don't double reward because when they hit next, we will walk this list again,
				//-- looking for a change in rank so that we can award the rank change.
				animatedRanks[currentAwardIndex] = newrank;
				
				currentRewardType = GameProfile.SharedInstance.Player.GetRankRewardTypeForLevel(oldrank);
				currentRewardItemID = GameProfile.SharedInstance.Player.GetRankRewardQuanityOrItemForLevel(oldrank);	//, currentRewardType);
	
				//notify.Debug ("LevelReward {0} = {1}", currentRewardType, currentRewardItemID);
				
				UIConfirmDialog.onPositiveResponse += OnGetRankReward;	
				//UIManagerOz.SharedInstance.okayDialog.ShowRewardDialog(rewardText, iconName);	//"On Reaching Level "+newrank, iconName);
				SetRank(newrank);
				AudioManager.SharedInstance.PlayFX(AudioManager.Effects.angelWings);
				return;
			}
			else {
				currentAwardIndex++;
				if(currentAwardIndex < 3) {
					float duration = 0.5f;
					
					if(animatedRankProgress.Count > (currentAwardIndex*2)) {
						RankProgress.sliderValue = animatedRankProgress[(currentAwardIndex*2)];	
					}
					float end = RankProgress.sliderValue;
					if(animatedRankProgress.Count > ((currentAwardIndex*2)+1)) {
						end = animatedRankProgress[(currentAwardIndex*2)+1];
					}
					
					TweenSlider ts = null;
					if(Mathf.Abs(end-RankProgress.sliderValue) < Mathf.Epsilon) {
						duration = 0.01f;
					}
					ts = TweenSlider.Begin(RankProgress.gameObject, duration, end);
					ts.onFinished += OnDoneAwardObjectiveProgress;
					ts.method = UITweener.Method.Linear;
					return;
				}
			}	
		}
		
		int count = 1;
		foreach(int index in completedIndices) {
			Transform currentObectiveRoot = getObjectiveRootFromIndex(index);
			if(currentObectiveRoot) {
				//-- Animate offscreen, start the reward loop again.
				Vector3 offscreenLeft = new Vector3(-Screen.width*3.0f, currentObectiveRoot.localPosition.y, currentObectiveRoot.localPosition.z);
				TweenPosition tp = TweenPosition.Begin(currentObectiveRoot.gameObject, cTransitionTime*(1.25f*count), offscreenLeft);
				if(tp) {
					tp.method = UITweener.Method.EaseInOut;
					tp.onFinished += OnObjectiveAwardedFinished;
				}
				count++;
			}
		}
	}
	
	public bool FillInObjectiveDataForIndex (int index, ObjectiveProtoData data) {
		Transform currentObectiveRoot = getObjectiveRootFromIndex(index);
		if(currentObectiveRoot == null) {
			notify.Error("We should never have a null here.");
			return false;
		}
		if(data._title == null || data._title.Length == 0)
			return false;
		
		NGUITools.SetActive(currentObectiveRoot.gameObject, true);
			
		GameObject go = HierarchyUtils.GetChildByName("ObjectiveTitle", currentObectiveRoot.gameObject);
		if(go == null) {
			notify.Error("We should never have a null here.");
			return false;
		}
		
		UILabel label = go.GetComponent<UILabel>() as UILabel;
		if(label == null) {
			notify.Error("We should never have a null here.");
			return false;
		}
		
		label.text = data._title;
		
		go = HierarchyUtils.GetChildByName("ObjectiveDesc", currentObectiveRoot.gameObject);
		if(go == null) {
			notify.Error("We should never have a null here.");
			return false;
		}
		
		label = go.GetComponent<UILabel>() as UILabel;
		if(label == null) {
			notify.Error("We should never have a null here.");
			return false;
		}	
		
		label.text = data._descriptionPreEarned;
		
		UISprite checkBox = getCheckBoxForObjectiveRootFromIndex(index);
		if(checkBox) {
			checkBox.color = Color.white;
			TweenColor tc = checkBox.gameObject.GetComponent<TweenColor>() as TweenColor;
			if(tc) {
				tc.enabled = false;
			}
			NGUITools.SetActive(checkBox.gameObject, false);
		}
		
		return true;
	}
	
	public void FillInObjectiveData () {
		if( GameProfile.SharedInstance == null ||
			GameProfile.SharedInstance.Player == null ||
			GameProfile.SharedInstance.Player.objectivesActive == null)
			return;
		
		int index = 0;
		foreach(ObjectiveProtoData ob in GameProfile.SharedInstance.Player.objectivesActive) {
			if(ob == null) {
				notify.Error("We should never have a null here.");
				continue;
			}
			
			if(FillInObjectiveDataForIndex(index, ob) == false) {
				Transform currentObectiveRoot = getObjectiveRootFromIndex(index);
				if(currentObectiveRoot != null) {
					NGUITools.SetActive(currentObectiveRoot.gameObject, false);
				}
			}
			index++;
		}
	}
	
	public void OnPostGameJournalClicked() {
		//-- Show the main menu and leave a bread crumb to this VC.
		if(mainMenuVC != null) {
			disappear(false);
			//TODO CurrentState = State.Stats;
			mainMenuVC.appear();
			mainMenuVC.previousViewController = this;
		}
	}
	
	public void OnStoreButton() {
		if(inCharacterSelectVC != null) {
			disappear(false);
			
			//TODO CurrentState = State.Stats;
			inCharacterSelectVC.previousViewController = this;
			inCharacterSelectVC.appear();
		}
	}
	
	public void OnShareButton() {
//		 jonoble
//		string pathToImage = Application.persistentDataPath + "/lastRun.png";
//		string Message = string.Format("I got {0} points while escaping from the demon monkey in Temple Run 2. Beat that! http://bit.ly/TempleRunGame", GamePlayer.SharedInstance.Score);
//		if(TwitterBinding.isTweetSheetSupported() == true) {
//			TwitterBinding.showTweetComposer(Message, pathToImage);
//		}
//		else if(FacebookBinding.isFacebookComposerSupported() == true) {
//			FacebookBinding.showFacebookComposer(Message, pathToImage, null);
//		}
		
	}
	
	public void OnPlayClicked() {
		
		if(MainGameCamera != null) {
			MainGameCamera.enabled = true;
			if(GamePlayer.SharedInstance != null && GamePlayer.SharedInstance.ShadowCamera != null && QualitySettings.GetQualityLevel() >= 1) {
				GamePlayer.SharedInstance.ShadowCamera.enabled = true;	
			}
		}
		
		disappear(true);
		
		//if(inGameVC != null) {
		//	inGameVC.appear();
			//ShowObject(activePowerIcon.gameObject, false, false);
		//}
		
		//-- Notify an object that is listening for this event.
		if(onPlayClickedHandler != null) {
			onPlayClickedHandler();
		}	
	}
	
	public void SetDistance(int distance) {
		if(DistanceLabel == null)
			return;
		DistanceLabel.text = distance.ToString();
	}
	
	public void SetScore(int score) {
		if(ScoreLabel == null)
			return;
		ScoreLabel.text = score.ToString();
	}
	
	public void SetCoinScore(int score) {
		if(CoinLabel == null)
			return;
		CoinLabel.text = score.ToString();
	}
	
	public void SetScoreMultiplier(int score) {
		if(GemLabel == null)
			return;
		GemLabel.text = score.ToString();
	}
	
	public void SetRank(int rank) {
		if(RankLabel == null)
			return;
		RankLabel.text = "Level " + rank.ToString();
	}
	
	public void SetRankProgress(float progress) {
		if(RankProgress == null)
			return;
		
		//-- Just to be safe, lets clamp from 0 to 1.
		progress = Mathf.Clamp01(progress);
		
		RankProgress.sliderValue = progress;
	}
}

*/