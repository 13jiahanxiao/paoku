using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class ObjectivesDataUpdater 
{
	public static List<string> objectivesQueue;
	
	static ObjectivesDataUpdater()
	{
		//Pre-warm objectives dictionary
		GenericStatsPerRun = new float[(int)ObjectiveType.TotalObjectivesCount];
		EnvironmentSetStatsPerRun = new float[EnvironmentSetBootstrap.BootstrapList.Count+1, (int)ObjectiveType.TotalObjectivesCount];
		
		Dictionary<int,float[]> LifetimeStats = GameProfile.SharedInstance.Player.LifetimeStats;
		for(int i=-1;i<=EnvironmentSetBootstrap.BootstrapList.Count;i++)
		{
			if(!LifetimeStats.ContainsKey(i))
			{
				float[] dict = new float[(int)ObjectiveType.LifetimeObjectivesCount];
				LifetimeStats.Add(i,dict);
			}
		}
		
		objectivesQueue = new List<string>();
		
	}
	private static NotificationSystem notifications;
	
	public static float[]	GenericStatsPerRun;
	public static float[,] EnvironmentSetStatsPerRun;
	
	private static WeeklyObjectives cachedWeeklyObjectives;
	private static WeeklyObjectives CachedWeeklyObjectives {
		get {
			if(cachedWeeklyObjectives==null)
				cachedWeeklyObjectives = Services.Get<ObjectivesManager>().GetWeeklyObjectivesClass();
			return cachedWeeklyObjectives;
		}
	}
	
	private static PopupNotification cachedPopupNotify;
	private static PopupNotification CachedPopupNotify {
		get {
			if(cachedPopupNotify==null)
				cachedPopupNotify = PopupNotification.PopupList[PopupNotificationType.Objective];//Services.Get<PopupNotification>();
			return cachedPopupNotify;
		}
	}

	public static int GetStatForLifetimeObjectiveType(ObjectiveType type, int env)
	{
		if(type < ObjectiveType.LifetimeObjectivesCount)
		{
			Dictionary<int,float[]> LifetimeStats = GameProfile.SharedInstance.Player.LifetimeStats;
			
			if(!LifetimeStats.ContainsKey(env))	return 0;
			
			return (int)LifetimeStats[env][(int)type];
		}
		return -1;
	}
	
	public static int GetStatForObjectiveType(ObjectiveType type, int env)
	{
		int outVal = 0;

		switch(type)
		{
		case ObjectiveType.UnlockPowerups:			outVal = GameProfile.SharedInstance.Player.powersPurchased.Count; break;
	//	case ObjectiveType.UnlockCharacters:			outVal = 1; break;
		case ObjectiveType.UnlockConsumables:		/*TODO - TEST*/	outVal = GameProfile.SharedInstance.Player.consumablesPurchasedQuantity.Count; break;
			
		case ObjectiveType.PassTheCornfields:		/*TODO*/outVal = 0; break;
	
	//	case ObjectiveType.DistanceWithoutArtifacts:	outVal = 1; break;
		case ObjectiveType.DistanceWithoutTransition:/*TODO*/outVal = 0; break;
			
		default:
			outVal = GetEnvironmentStat(type,env); break;
		}
		
		//Debug.Log(type.ToString() + " " +outVal);
		
		return outVal;
	}
	
	public static void AddToGenericStat(ObjectiveType s,float qty)
	{
		if(GameController.SharedInstance.IsTutorialMode && !GamePlayer.SharedInstance.IsDead) // if you are currently running and in tutorial don't add to your stats
			return;
		ChangeGenericStat(s,qty,false);
	}
	
	public static void SetGenericStat(ObjectiveType s,float qty)
	{
		ChangeGenericStat(s,qty,true);
	}
	
	private static void ChangeStatInArray(float[,] dict, ObjectiveType s, int environment, float qty, bool absolute = false)
	{
		if(absolute)
		{
			dict[environment,(int)s] = qty;
		}
		else
		{
			dict[environment,(int)s] += qty;
		}
	}	
	private static void ChangeStatInDictionary(Dictionary<int,float[]> dict,
												ObjectiveType s, int environment, float qty, bool absolute = false)
	{
		if(absolute)
		{
			dict[environment][(int)s] = qty;
		}
		else
		{
			dict[environment][(int)s] += qty; 
		}
	}
	
	public static void ChangeGenericStat(ObjectiveType s, float qty, bool absolute = false)
	{
		if(absolute)
		{
			GenericStatsPerRun[(int)s] = qty;
		}
		else
		{
			GenericStatsPerRun[(int)s] += qty;
		}
		
		int env = EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.SetId;
		
		//Change the per-run tracked stat based on provided info
		ChangeStatInArray(EnvironmentSetStatsPerRun,s,env,qty,absolute);
		
		
		// wxj,change per-run of stat in activity1 ,2 objective
		foreach(ObjectiveProtoData data in ObjectivesManager.ActivityObjectives)
		{
			ConditionProtoData cond = data._conditionList[0];
			if(cond._type == ObjectiveType.Activity1 && s == ObjectiveType.Distance)
			{
				if(!cond._actiEarnedStatForEnvs.ContainsKey(env))
				{
					cond._actiEarnedStatForEnvs.Add(env, 0);
				}
				cond._actiEarnedStatForEnvs[env] += qty;
				cond._actiEarnedStatForEnvs[env] = cond._actiEarnedStatForEnvs[env] <= 1500F ? cond._actiEarnedStatForEnvs[env] : 1500F;
			}
			else if(cond._type == ObjectiveType.Activity2 && s == ObjectiveType.JumpOverPassed)
			{
				if(!cond._actiEarnedStatForEnvs.ContainsKey(env))
				{
					cond._actiEarnedStatForEnvs.Add(env, 0);
				}
				cond._actiEarnedStatForEnvs[env] += (int)qty;
				cond._actiEarnedStatForEnvs[env] = Mathf.Clamp(cond._actiEarnedStatForEnvs[env], 0, 15);
				UnityEngine.Debug.Log("wxj: Activity2 env:"+env+"  earnedStatForCurEnv:"+cond._actiEarnedStatForEnvs[env]);
			}
		}
		
		
		if(s < ObjectiveType.LifetimeObjectivesCount)
		{
		//	if(!IsObjectiveTypeLifetimeTracked(s))
		//	{
		//		Debug.LogError("Bad lifetime objective! " + s.ToString());
		//	}
			//Change the per-run tracked stat based on provided info
			//--First for global stats...
			ChangeStatInDictionary(GameProfile.SharedInstance.Player.LifetimeStats,s,-1,qty,absolute);
			//--Then for environment-specific stats.
			ChangeStatInDictionary(GameProfile.SharedInstance.Player.LifetimeStats,s,env,qty,absolute);
			
		}
		
		//Now update
		CheckForCompletedObjectives(s);
		
	//	else
	//	if(IsObjectiveTypeLifetimeTracked(s))
	//	{
	//		Debug.LogError("Bad lifetime objective! " + s.ToString());
	//	}
	}

	public static void CheckForCompletedObjectives(ObjectiveType s)
	{
		ObjectiveProtoData data = null;
		ConditionProtoData cond = null;
		
		List<string> facebookAchievements = new List<string>();
		
		if(notifications==null)
			notifications = Services.Get<NotificationSystem>();
		
		PlayerStats cachedPlayer = GameProfile.SharedInstance.Player;
		
		
		// wxj, Check all activity objectives
		List<ObjectiveProtoData> actiObjs = ObjectivesManager.ActivityObjectives;
		if(actiObjs != null)
		{
			for(int i = 0; i < actiObjs.Count; i++)
			{
				data = actiObjs[i];
				cond = data._conditionList[0];
				
				// if the objective is completed before update, return
				if(cond._earnedStatValue >= cond._statValue)
				{
					return;
				}
				
				int oldValue = cond._earnedStatValue;
				cachedPlayer.UpdateObjectiveStat(data, false, s);
				// if earned value changed, save in file
				if(cond._earnedStatValue > oldValue)
				{
					ObjectivesManager.saveFileToPersistent(ObjectivesManager.AllActivityObjectives, "ObjectivesActivity");
				}
					
					
				// if the objective is complete
				if(cond._earnedStatValue >= cond._statValue)
				{
					// wxj, unclaimed list is mark the objective that need to be rewarded
					if(!GameProfile.SharedInstance.Player.objectivesUnclaimed.Contains(data._id))
								GameProfile.SharedInstance.Player.objectivesUnclaimed.Add(data._id);
					
					//					AnalyticsInterface.LogGameAction("challenges", "complete", data._title,GameProfile.GetAreaCharacterString(),0);
						
					Services.Get<NotificationSystem>().SendOneShotNotificationEvent(data._id, (int)OneShotNotificationType.ActivityChallengeCompleted);	
					//Show a pop-up
					// wxj, need to modify
					CachedPopupNotify.setActivitySprite(true);
					CachedPopupNotify.Show(Localization.SharedInstance.Get("Msg_ActivityChallCompleted"));
				}
				
			}
		}
		
		
		
		
		//First get the relevant legendary objectives from ObjectivesManager
		List<ObjectiveProtoData> relevantObjectives = ObjectivesManager.QuickAccessLegendaryObjectives[(int)s];
	
		//Check all Legendary objectives
		if(relevantObjectives!=null)
		{
			for(int i=0;i<relevantObjectives.Count;i++)
			{
				data = relevantObjectives[i];
				cond = data._conditionList[0];
				
				if(cond._type == s)	//Double-check that this is the correct type
				{
					if(!cachedPlayer.legendaryObjectivesEarned.Contains(data._id))
					{
						cachedPlayer.UpdateLegendaryObjectiveStat(data);
					
						//If the objective is complete...
						if(cond._earnedStatValue >= cond._statValue)
						{
							//Mark as complete (add it to the complete array)
							cachedPlayer.legendaryObjectivesEarned.Add(data._id);
							
							//Remove it from the quick-access array; we dont need it anymore
							relevantObjectives.RemoveAt(i);
							
							notifications.SendOneShotNotificationEvent(data._id, (int)OneShotNotificationType.LegendaryChallengeCompleted);
	
							
							if(!GameProfile.SharedInstance.Player.objectivesUnclaimed.Contains(data._id))
								GameProfile.SharedInstance.Player.objectivesUnclaimed.Add(data._id);
							
							// this is now being called later on
							if (data._pointValue > 0)	
							{
								string achievementId = BundleInfo.GetBundleId() + "." + data._title;
								objectivesQueue.Add(achievementId);
								Debug.Log ("complete objective " + achievementId);
								// this is now being done in statsRoot after run is over
								// but safe to do so if we are in the menus as network activity wont kill you
								if (GameController.SharedInstance.gameState == GameState.IN_MENUS)
								{
									//									GameCenterBinding.reportAchievement(achievementId, 100.0f);
									
									// Normally we publish all achievements after a run
									// But this will handle the achievements that you can earn
									// outside of a run ("Purchase all abilities", "Max out all abilities", etc.)
									facebookAchievements.Add( achievementId );
									
									//									AnalyticsInterface.LogGameAction( "objective", "achieved", achievementId, GameProfile.GetAreaCharacterString(), 0 );
								}
							}
							
							i--;
							
							//Show a pop-up
							//CachedPopupNotify.Show(Localization.SharedInstance.Get(data._title));
							CachedPopupNotify.SetOtherSprite("icon_objectives_legendary");
							CachedPopupNotify.Show(Localization.SharedInstance.Get("Msg_LegendaryCompleted"));
						}
					}
				}
				else
					Debug.LogWarning("Incorrect type! Objective '"+data._title+"' somehow got put in the wrong slot of QuickAccessLegendaryObjectives");
			}
			
			if ( facebookAchievements.Count > 0 )
			{
				//				SharingManagerBinding.PublishFacebookScoresAndAchievements( 0, facebookAchievements );
			}
		}
		
		
		//Check all Weekly objectives
		List<ObjectiveProtoData> weeklyObjs = Services.Get<ObjectivesManager>().GetWeeklyObjectives();
		for(int i=0;i<weeklyObjs.Count;i++)
		{
			data = weeklyObjs[i];
			cond = data._conditionList[0];
			
			if(cond._type == s)	//Double-check that this is the correct type
			{
				if(!CachedWeeklyObjectives.GetEarnedWeeklyObjectiveList().Contains(data._id))
				{
					cachedPlayer.UpdateObjectiveStat(data,true);
				
					//If the objective is complete...
					if(/*cond._earnedNeighborValue + */cond._earnedStatValue >= cond._statValue)
					{
				//		Debug.Log(data._title + " " + cond._statValue + " " + cond._type.ToString() + " " + cond._timeType.ToString());
						
						//Mark as complete (add it to the complete array)
						CachedWeeklyObjectives.GetEarnedWeeklyObjectiveList().Add(data._id);
						
						
						//We are now rewarding objectives in the objective UI
				//		CachedWeeklyObjectives.RewardObjective(data._id);
						
						if(!GameProfile.SharedInstance.Player.objectivesUnclaimed.Contains(data._id))
							GameProfile.SharedInstance.Player.objectivesUnclaimed.Add(data._id);
						
						//						AnalyticsInterface.LogGameAction("challenges", "complete", data._title,GameProfile.GetAreaCharacterString(),0);
						
						Services.Get<NotificationSystem>().SendOneShotNotificationEvent(data._id, (int)OneShotNotificationType.WeeklyChallengeCompleted);
						
						//Show a pop-up
						//CachedPopupNotify.Show(Localization.SharedInstance.Get(data._title));
						CachedPopupNotify.SetOtherSprite("icon_objectives_weekly");
						CachedPopupNotify.Show(Localization.SharedInstance.Get("Msg_WeeklyChallCompleted"));
					}
				}
			}
		}
		
		List<ObjectiveProtoData> teamObjectiveList = Services.Get<ObjectivesManager>().GetWeeklyObjectivesClass().TeamObjectiveList;
		
		List<int> earnedTeamObjectiveList = Services.Get<ObjectivesManager>().GetWeeklyObjectivesClass().EarnedTeamObjectiveList;
		
		for ( int i = 0; i < teamObjectiveList.Count; i++ )
		{
			data = teamObjectiveList[i];
			cond = data._conditionList[0];
			
			if ( cond._type == s )
			{
				
				// Update team objective stats regardless if they are completed or not.
				// Thereby a player can still help his friends out
				
				cachedPlayer.UpdateObjectiveStat( data, true );	
				
				if ( !earnedTeamObjectiveList.Contains( data._id ) )
				{

					
					if ( cond._earnedStatValue + cond._earnedNeighborValue >= cond._statValue )
					{
						earnedTeamObjectiveList.Add( data._id );
						
						// TO DO, have UI Hookup to reward challenge.
						// UI should be Hooked up
						// CachedWeeklyObjectives.RewardObjective( data._id );
						
						if ( !GameProfile.SharedInstance.Player.objectivesUnclaimed.Contains( data._id ) )
						{
							GameProfile.SharedInstance.Player.objectivesUnclaimed.Add( data._id );
						}
						
						//						AnalyticsInterface.LogGameAction("challenges", "complete", data._title,GameProfile.GetAreaCharacterString(),0);
						
						// TO DO. Have notification system update for team objective.
						Services.Get<NotificationSystem>().SendOneShotNotificationEvent( data._id, (int) OneShotNotificationType.TeamChallengeCompleted );
						
						CachedPopupNotify.SetOtherSprite("icon_objectives_weekly");
						// TO DO. Get Team challenge Icon from Art.
						CachedPopupNotify.Show( Localization.SharedInstance.Get( "Msg_TeamComplete" ) );
					}
				}
			}
		}
	}

	private static int GetGenericStat(ObjectiveType s)
	{
		return (int)GenericStatsPerRun[(int)s];
	}
	
	public static int GetEnvironmentStat(ObjectiveType s, int env)
	{
		if(env<0)
		{
			return GetGenericStat(s);
		}
		return (int)EnvironmentSetStatsPerRun[env,(int)s];
	}
	
	public static void Clear()
	{
	//	Debug.Log("Clear");
		GameProfile.SharedInstance.Player.RecordOverTimeStats();
		
		Services.Get<ObjectivesManager>().GetWeeklyObjectivesClass().SaveChallenges();
		
		//Reset all stat values to zero. DO NOT CLEAR DCITIONARIES
		for(ObjectiveType o = (ObjectiveType)0; o < ObjectiveType.TotalObjectivesCount; ++o)
		{
			GenericStatsPerRun[(int)o] = 0f;
		}
		
		for(int e=1;e<=EnvironmentSetBootstrap.BootstrapList.Count;e++)
		{
			for(ObjectiveType o = (ObjectiveType)0; o < ObjectiveType.TotalObjectivesCount; ++o)
			{
				EnvironmentSetStatsPerRun[e,(int)o] = 0f;
			}
		}
	}
	
	public static bool AreAnyWeeklyChallengesCompleted()
	{
		bool completed = false;
		List<ObjectiveProtoData> weeklyObjs = Services.Get<ObjectivesManager>().GetWeeklyObjectives();
		
		if ( weeklyObjs == null || weeklyObjs.Count == 0 )
		{
			return completed;
		}
		
		foreach (ObjectiveProtoData obj in weeklyObjs)
		{
			if (GameProfile.SharedInstance.Player.objectivesUnclaimed.Contains(obj._id))
			{
				completed = true;
			}
		}
		
		return completed;
	}
	
	public static void LogEnvironmentVisited(int destinationEnvironmentId)
	{
		//Debug.LogWarning("LogEnvironmentVisited for envID: " + destinationEnvironmentId);
		
		if (!GameProfile.SharedInstance.Player.environmentsVisited.Contains(destinationEnvironmentId))
		{
			GameProfile.SharedInstance.Player.environmentsVisited.Add(destinationEnvironmentId);
			AddToGenericStat(ObjectiveType.EnvironmentsVisitedAllTime, 1);
			//Debug.LogWarning("Logging new all time location visit for envID: " + destinationEnvironmentId);
			//Debug.LogWarning("GetGenericStat(ObjectiveType.EnvironmentsVisitedAllTime) = " + 
			//	GetGenericStat(ObjectiveType.EnvironmentsVisitedAllTime).ToString());
		}
		
		if (!GamePlayer.SharedInstance.EnvironmentsVisitedThisRun.Contains(destinationEnvironmentId))
		{
			GamePlayer.SharedInstance.EnvironmentsVisitedThisRun.Add(destinationEnvironmentId);
			AddToGenericStat(ObjectiveType.EnvironmentsVisitedSingleRun, 1);
			//Debug.LogWarning("Logging new per run location visit for envID: " + destinationEnvironmentId);
			//Debug.LogWarning("GetGenericStat(ObjectiveType.EnvironmentsVisitedSingleRun) = " + 
			//	GetGenericStat(ObjectiveType.EnvironmentsVisitedSingleRun).ToString());
		}	
	}
}







//	public static bool HaveAllEnvironmentsBeenVisited()
//	{
//		List<int> envVisited = GameProfile.SharedInstance.Player.environmentsVisited;
//		
//		return (envVisited.Contains(EnvironmentSetManager.WhimsyWoodsId) && 
//			envVisited.Contains(EnvironmentSetManager.DarkForestId) &&
//			envVisited.Contains(EnvironmentSetManager.YellowBrickRoadId) &&
//			envVisited.Contains(EnvironmentSetManager.EmeraldCityId));
//	}


	
/*	public static bool IsObjectiveTypeLifetimeTracked(ObjectiveType type)
	{
		if(type==ObjectiveType.Distance ||
			type==ObjectiveType.CollectCoins ||
			type==ObjectiveType.CollectSpecialCurrency ||
			type==ObjectiveType.Score ||
			type==ObjectiveType.CollectPowerups ||
			type==ObjectiveType.CoinMeterFills ||
			type==ObjectiveType.Resurrects ||
			type==ObjectiveType.ModifierLevelDoubleCoins ||
			type==ObjectiveType.ModifierLevelMagician ||
			type==ObjectiveType.ModifierLevelEnchanter ||
			type==ObjectiveType.ModifierLevelBargainHunter ||
			type==ObjectiveType.ModifierLevelLuck ||
			type==ObjectiveType.ModifiersMaxed ||
			type==ObjectiveType.UseConsumableHeadStart ||
			type==ObjectiveType.UseConsumableMegaHeadStart ||
			type==ObjectiveType.UseConsumableMultiplier ||
			type==ObjectiveType.UseConsumableStumbleProof ||
			type==ObjectiveType.UseConsumableThirdEye ||
			type==ObjectiveType.PaidDestinyCard ||
			type==ObjectiveType.FastTraveled ||
			type==ObjectiveType.UseEverything ||
			type==ObjectiveType.UseAllInGamePowerups 
			)
			return true;
		return false;
	}
	
	*/
	//		UIManagerOz.SharedInstance.okayDialog.ShowOkayDialog(locKey, "Loc_Downloading", "Btn_Ok");
	
	
	
	//NOTE!!! These two following functions take a good 0.04 ms (~0.7% of our frame time) for one call
	//		if the lifetime of the given stat is tracked... We need to pass in a boolean that tells whether or not 
	//		Lifetime data should be pushed to these arrays, and only set it to "true" occasionaly.
	
	
	


	
	/*public static void SetGenericStat(ObjectiveType s,float amt)
	{
		if(!GenericStatsPerRun.ContainsKey(s))
			GenericStatsPerRun.Add(s,amt);
		else
			GenericStatsPerRun[s] = amt;
		
		int env = EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.SetId;
		
		if(!EnvironmentSetStatsPerRun.ContainsKey(env))
			EnvironmentSetStatsPerRun.Add(env,new Dictionary<ObjectiveType,float>());
		
		if(!EnvironmentSetStatsPerRun[env].ContainsKey(s))
			EnvironmentSetStatsPerRun[env].Add(s,amt);
		else
			EnvironmentSetStatsPerRun[env][s] = amt;
		
		if(IsObjectiveTypeLifetimeTracked(s))
		{			
			//Debug.Log(env.ToString());
			Dictionary<int,Dictionary<ObjectiveType,float>> LifetimeStats = GameProfile.SharedInstance.Player.LifetimeStats;
			
			//For generics
			if(!LifetimeStats.ContainsKey(-1))
				LifetimeStats.Add(-1,new Dictionary<ObjectiveType,float>());
			
			if(!LifetimeStats[-1].ContainsKey(s))
				LifetimeStats[-1].Add(s,amt);
			else
				LifetimeStats[-1][s] = amt;
			
			if(!LifetimeStats.ContainsKey(env))
				LifetimeStats.Add(env,new Dictionary<ObjectiveType,float>());
			
			if(!LifetimeStats[env].ContainsKey(s))
				LifetimeStats[env].Add(s,amt);
			else
				LifetimeStats[env][s] = amt;
			
			//Check for completed objectives
			//TODO: Break this entire function into smaller ones! Same goes for the above function!
			ObjectiveProtoData data = null;
			ConditionProtoData cond = null;
			for(int i=0;i<ObjectivesManager.QuickAccessLegendaryObjectives.Count;i++)
			{
				data = ObjectivesManager.LegendaryObjectives[i];
				cond = data._conditionList[0];
				
				if(cond._type == s)
				{
					if(!GameProfile.SharedInstance.Player.legendaryObjectivesEarned.Contains(data._id))
					{
						GameProfile.SharedInstance.Player.UpdateObjectiveStat(data);
					
						if(cond._earnedStatValue >= cond._statValue)
						{
							GameProfile.SharedInstance.Player.legendaryObjectivesEarned.Add(data._id);
							
							CachedPopupNotify.Show(Localization.SharedInstance.Get(data._title));
						}
					}
				}
			}
			
			
			List<ObjectiveProtoData> weeklyObjs = Services.Get<ObjectivesManager>().GetWeeklyObjectives();
			for(int i=0;i<weeklyObjs.Count;i++)
			{
				data = weeklyObjs[i];
				cond = data._conditionList[0];
				
				if(cond._type == s)
				{
					if(!Services.Get<ObjectivesManager>().GetWeeklyObjectivesClass().GetEarnedWeeklyObjectiveList().Contains(data._id))
					{
						//UpdateObjectiveStat objective = data, isWeekly = true
						GameProfile.SharedInstance.Player.UpdateObjectiveStat(data, true);
					
						if(cond._earnedNeighborValue + cond._earnedStatValue >= cond._statValue)
						{
							if(!CachedWeeklyObjectives.GetEarnedWeeklyObjectiveList().Contains(data._id))
								CachedWeeklyObjectives.GetEarnedWeeklyObjectiveList().Add(data._id);
							
							CachedPopupNotify.Show(Localization.SharedInstance.Get(data._title));
						}
					}
				}
			}
		}
	}*/
	
	


/*
	
	// everything below is not used anymore...
 
 
//	List<int> oldObjectiveIds = new List<int>();	
//	List<int> completedIndices = new List<int>();
//	
//	int oldRank = 0;
//	int newRank = 0;
//	float oldProgress = 0.0f;
//	float newProgress = 0.0f;		

	//ComputeCompletedObjectives();
	
	private void ComputeCompletedObjectives()
	{
		Debug.LogWarning("Computing Completed Objectives!");
		
		oldRank = GameProfile.SharedInstance.Player.GetCurrentRank();
		oldProgress = GameProfile.SharedInstance.Player.GetCurrentRankProgress();		
		
		oldObjectiveIds.Clear();
		oldObjectiveIds = GetActiveObjectiveIDs();
		
		completedIndices.Clear();
		completedIndices = CheckIfActiveObjectivesCompleted();
		
		newRank = GameProfile.SharedInstance.Player.GetCurrentRank();
		newProgress = GameProfile.SharedInstance.Player.GetCurrentRankProgress();
			
		if (oldRank != newRank)
		{
			GiveLevelRewards();
		}

		GameProfile.SharedInstance.Serialize();
	}	
	
	private List<int> GetActiveObjectiveIDs()
	{
		List<int> objectiveIDs = new List<int>();
		
		foreach (ObjectiveProtoData ob in GameProfile.SharedInstance.Player.objectivesActive)
		{
			objectiveIDs.Add(ob._id);
		}
		
		return objectiveIDs;
	}
	
	private List<int> CheckIfActiveObjectivesCompleted()
	{
		List<int> completedIndices = new List<int>();
		int count = GameProfile.SharedInstance.Player.objectivesActive.Count;
		
		for (int i=0; i<count; i++) 
		{
			ObjectiveProtoData ob = GameProfile.SharedInstance.Player.objectivesActive[i];
			
			if (ob._earnedStatValue >= ob._statValue) 	//-- Completed
			{
				if (GameProfile.SharedInstance.Player.objectivesEarned.Contains(ob._id) == false)
				{
					GameProfile.SharedInstance.Player.objectivesEarned.Add(ob._id);	
				}
				
				GameProfile.SharedInstance.Player.RefillObjectiveForIndex(i, ob._statValue);	
				completedIndices.Add(i);	//-- OB has changed.
			}	
		}
		
		return completedIndices;
	}


// for rewards:
//	if (oldrank != newrank) 
//	{
//		currentRewardType = GameProfile.SharedInstance.Player.GetRankRewardTypeForLevel(oldrank);
//		currentRewardItemID = GameProfile.SharedInstance.Player.GetRankRewardQuanityOrItemForLevel(oldrank, currentRewardType);
//		GiveLevelRewards(currentRewardType, currentRewardItemID);	
//	}
	
	private void GiveLevelRewards()	//RankRewardType rewardType, int QtyOrItemID) 
	{
		//-- LEVELED!
		//-- WE set this data so that if the user hits "next" to speed through the animations.
		//-- we don't double reward because when they hit next, we will walk this list again,
		//-- looking for a change in rank so that we can award the rank change.
		//animatedRanks[currentAwardIndex] = newrank;
		
		RankRewardType currentRewardType = GameProfile.SharedInstance.Player.GetRankRewardTypeForLevel(oldRank);
		int currentRewardItemID = GameProfile.SharedInstance.Player.GetRankRewardQuanityOrItemForLevel(oldRank, currentRewardType);
		string rewardText = GameProfile.SharedInstance.Player.GetRewardTextFor(currentRewardType, currentRewardItemID);
		string iconName = GameProfile.SharedInstance.Player.GetRewardIconFor(currentRewardType, currentRewardItemID);
		//TR.LOG ("LevelReward {0} = {1}", currentRewardType, currentRewardItemID);
		
		//UIConfirmDialogOz.onPositiveResponse += OnGetRankReward;	
		UIManagerOz.SharedInstance.okayDialog.ShowRewardDialog(rewardText, "On Reaching Level " + newRank, iconName);
		
		if (currentRewardType == RankRewardType.Coins) 
		{
			GameProfile.SharedInstance.Player.coinCount += currentRewardItemID;	//QtyOrItemID;
			//postGameVC.UpdateCurrency();
		}
		else if (currentRewardType == RankRewardType.Gems) 
		{
			GameProfile.SharedInstance.Player.specialCurrencyCount += currentRewardItemID;	//QtyOrItemID;
			//postGameVC.UpdateCurrency();
		}
		
		//SetRank(newrank);		
		//AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_UI_Menu_levelMax);		
		GameProfile.SharedInstance.Serialize();
		//HaveGivenLevelRewards = true;
	}	

*/

		//int currentAwardIndex = 0;
		//List<int> animatedRanks = new List<int>();
		//List<float> animatedRankProgress = 0.0f;
		//List<int> oldObjectiveIds = new List<int>();
		//List<int> completedIndices = new List<int>();
		//bool DidComputeObjectives = false;
		//bool HaveGivenLevelRewards = false;			
		
		//if (DidComputeObjectives == true) { return; }	//-- Only do this once per Death.

		//DidComputeObjectives = true;
		//currentAwardIndex = 0;

//		int count = GameProfile.SharedInstance.Player.objectivesActive.Count;
//		for (int i=0; i<count; i++) 
//		{
//			ObjectiveProtoData ob = GameProfile.SharedInstance.Player.objectivesActive[i];
//			if (ob == null) { TR.ERROR("We should never have a null here."); continue; }
//			if (ob._title == null || ob._title.Length == 0) { oldObjectiveIds.Add (-1); }
//			else { oldObjectiveIds.Add(ob._id);	}
//			
//			//-- Completed
//			if (ob._earnedStatValue >= ob._statValue) 
//			{
//				if (GameProfile.SharedInstance.Player.objectivesEarned.Contains(ob._id) == false) 
//				{
//					GameProfile.SharedInstance.Player.objectivesEarned.Add(ob._id);	
//				}
//				GameProfile.SharedInstance.Player.RefillObjectiveForIndex(i, ob._statValue);	
//				//-- OB has changed.
//				completedIndices.Add(i);
//			}
//			
//			int newRank = GameProfile.SharedInstance.Player.GetCurrentRank();
//			float newProgress = GameProfile.SharedInstance.Player.GetCurrentRankProgress();
//			animatedRanks.Add(newRank);
//			if (oldRank != newRank) 
//			{
//				animatedRankProgress.Add(1.0f);
//				animatedRankProgress.Add(0.0f);
//			}
//			else 
//			{
//				animatedRankProgress.Add(newProgress);
//				animatedRankProgress.Add(newProgress);
//			}
//			oldRank = newRank;
//		}
	