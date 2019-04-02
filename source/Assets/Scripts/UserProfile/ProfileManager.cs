using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SocialPlatforms;

public class ProfileManager : MonoBehaviour
{
	public UserProtoData userServerData { get; private set; }
	
	public UserProtoData lastServerData { get; private set; }
	
	private bool serverAllowProfile = false;
	//private bool startCheckProfile = false;
	
	public GameObject messageObject = null;
	
	public static ProfileManager SharedInstance;
	
	private static bool _gotGemReward = false;
	
	public static bool GotGemReward
	{
		get
		{
			return _gotGemReward;
		}
		set
		{
			_gotGemReward = value;
		}
	}
	
	// Changing from User Declined Restore to User Decision Made.
	// Will switch to true when the user makes a decision for either accepting or rejecting
	// the restore
	private bool userDecisionMade = false;
	
	public bool UserDecisionMade
	{ 
		get
		{
			return userDecisionMade;
		}
	}
	
	/*
	public delegate void OnlineProfileInstatiatedHandler();
	protected static event OnlineProfileInstatiatedHandler onOnlineProfileInstantiatedEvent = null;
	public static void RegisterForOnlineProfileLoaded( OnlineProfileInstatiatedHandler delg) {
		onOnlineProfileInstantiatedEvent += delg;
	}
	public static void UnregisterForOnlineProfileLoaded( OnlineProfileInstatiatedHandler delg) {
		onOnlineProfileInstantiatedEvent -= delg;
	}
	*/

	protected static Notify notify;
	
	void Awake()
	{
		if (SharedInstance ==  null)
		{
			SharedInstance = this;
		}
		else
		{
			notify.Warning("Profile Manager: multiple instances of Profile Manager have  been instatiated.");
		}
		notify = new Notify(this.GetType().Name);
		
		notify.Debug("Profile Manager Awake");
	}
	
	void Start()
	{	
		UIIdolMenuViewControllerOz.RegisterForOnIdolBottomPanelComplete( ProfileManager.SharedInstance.CheckForProfile );

		notify.Debug("Profile Manager Started");
		
		/*
		if (onOnlineProfileInstantiatedEvent != null)
		{
			onOnlineProfileInstantiatedEvent();
		}
		*/
	}
	
	/*
	void Update()
	{
		//notify.Debug("Profile Manager Update loop");
		
		if (startCheckProfile)
		{
			if (Initializer.GetInitDownloadSuccess() && GameProfile.GetLocalProfileLoaded())
			{
				startCheckProfile = false;
				restoreProfile();
			}
			else if (Initializer.GetInitDownloadFailure() && GameProfile.GetLocalProfileLoaded())
			{
				startCheckProfile = false;
				SendMessageToUIManager();
			}
		}
	}
	*/
	
	/*
	public void StartCheckProfile(GameObject msgObject)
	{
		messageObject = msgObject;
		
		startCheckProfile = true;
	}
	*/
	
	public void SendMessageToUIManager()
	{
		//messageObject.SendMessage("OnProfileLoadCheckDone");
	}
	
	private void GetProfile()
	{
/*
		//Build Headers
		Hashtable headers = new Hashtable();
		headers.Add("X-Token", SharingManagerBinding.GetTrozUniqueIdentifier());
		headers.Add("Accept-Language", "en_US");
		headers.Add("Authentication", "");
		WWWForm profileForm = new WWWForm();
		
		Dictionary<string, string> deviceIdDict = new Dictionary<string, string>();

		deviceIdDict.Add("deviceId", SharingManagerBinding.GetTrozUniqueIdentifier());
		string jsonString = MiniJSON.Json.Serialize(deviceIdDict);
		
		profileForm.AddField("data", jsonString);		
		NetAgent.Submit(new NetRequest("/api/user/get-profile?deviceId=" + SharingManagerBinding.GetTrozUniqueIdentifier(), profileForm, GotProfile, headers));

		string deviceId = SharingManagerBinding.GetTrozUniqueIdentifier();
		NetAgent.Submit(new NetRequest("/v1/user/get-profile?deviceId=" + deviceId, GotProfile));
*/
	}
	
	private bool GotProfile(WWW www, bool noErrors, object results)
	{
		notify.Debug("Got Profile noErrors=" + noErrors + " www.error=" + www.error + " text= " + www.text);
		bool result = false;
		
		if (noErrors)
		{
			if (results == null)
				notify.Error("No results!  Must not be connected... " + www.text);
			
			Dictionary<string, object> rootDict = results as Dictionary<string, object>;
			
			int responseCode = int.Parse(rootDict["responseCode"].ToString());
			
			if (responseCode == 200) 
			{
				notify.Debug(MiniJSON.Json.Serialize(rootDict));
				
				Dictionary<string,object> accountDict = 
						rootDict["account"] as Dictionary<string,object>;
				notify.Debug(MiniJSON.Json.Serialize(accountDict));
				
				ProfileManager.SharedInstance.userServerData = DecodeUserJsonObject(accountDict);
				
				// Apply the neighbor values to any Team Objectives.
				if ( WeeklyObjectives.TeamObjectiveCount > 0 
					&& ProfileManager.SharedInstance.userServerData._guildChallengeList.Count > 0 
				) {
					List<ObjectiveProtoData> teamObjectiveList 
						= Services.Get<ObjectivesManager>().GetWeeklyObjectivesClass().TeamObjectiveList;
					
					foreach ( GuildChallengeProtoData gcProtoData in ProfileManager.SharedInstance.userServerData._guildChallengeList )
					{
						foreach ( ObjectiveProtoData teamObjective in teamObjectiveList )
						{
							if (gcProtoData._challengeIndex == teamObjective._id )
							{
								teamObjective._conditionList[0]._earnedNeighborValue = gcProtoData._neighborValueTotal;
							}
						}
					}
				}
				
				notify.Debug("Decoded User Object: " + userServerData.UserToJson());
				
				result = true;
			}
			
		}
		return result;
	}
	
	public bool ApplyProfileFromInit(List<object> profileList, int responseCode, bool serverProfile)
	{
		bool result = false;
		

		
		if (responseCode == 200 && profileList != null)
		{
			if (profileList.Count == 1)
			{			

				ProfileManager.SharedInstance.userServerData = DecodeUserJsonObject(profileList[0]);
				
				/*
				if (ProfileManager.SharedInstance.lastServerData != null) 
				{
					if ((ProfileManager.SharedInstance.userServerData._rank >= ProfileManager.SharedInstance.lastServerData._rank)
						&& (ProfileManager.SharedInstance.userServerData._totalMeters >	ProfileManager.SharedInstance.lastServerData._totalMeters))
					{
						ProfileManager.SharedInstance.lastServerData = DecodeUserJsonObject(profileList[0]);
					}
				}
				else 
				{
					ProfileManager.SharedInstance.lastServerData = DecodeUserJsonObject(profileList[0]);
				}
				*/
				ProfileManager.SharedInstance.lastServerData = DecodeUserJsonObject(profileList[0]);
				
				notify.Debug("ApplyProfileFromInit");
				
				notify.Debug( "[ProfileManager] - ApplyProfileFromInit Total Neighbor Challenge Progress" );
				
				TeamChallengeTotaler.RefreshData( ProfileManager.SharedInstance.userServerData._neighborList );

				result = true;
				
				notify.Debug("Server Data: " + lastServerData.UserToJson());
				
				Initializer.SharedInstance.SetDMOId( userServerData._dbId );
				
				//PlayerStats inGameStats = GameProfile.SharedInstance.Player;
				
				//Determine load based upon presence of boolean
				
				//bool allowProfile = Settings.GetBool("allow-profile-restore", false);
				
				serverAllowProfile = serverProfile;
				
			 
				/* Moved to a delegate
				if (allowProfile && serverProfile)
				{
					if (userServerData != null && (userServerData._rank >= inGameStats.GetCurrentRank())
						&& (userServerData._totalMeters > inGameStats.lifetimeDistance))
					{
						notify.Debug("Profile Loader - Server data higher than local.");
					
						UIConfirmDialogOz.onNegativeResponse += onNotLoadProfile;
						UIConfirmDialogOz.onPositiveResponse += onLoadProfile;
				
						UIManagerOz.SharedInstance.confirmDialog.ShowConfirmDialog("Msg_ProfileStored_Title",
							"Msg_ProfileStored_Desc",
							"Btn_No", "Btn_Yes");
					}
				}
				*/
				
				
			}
		}
		
		
		/*
		if (onOnlineProfileLoadedEvent != null)
		{
			notify.Debug("Profile Manager call delegate");
			onOnlineProfileLoadedEvent();
		}
		*/		
		
		return result;
	}
	
	public void restoreProfile(GameObject msgObject)
	{
		
	}
	
	/*
	public void TriggerEvent()
	{
		//Call the delegate if it exists
		if (onOnlineProfileLoadedEvent != null)
		{
			notify.Debug("Profile Manager call delegate");
			onOnlineProfileLoadedEvent();
		}
		
	}
	*/

	private UserProtoData DecodeUserJsonObject(object _accountDict)
	{
		Dictionary<string, object> dict = _accountDict as Dictionary<string, object>;
		
		UserProtoData user = new UserProtoData(dict);
		
		return user;
	}
	
	public void onNotLoadProfile()
	{
		UIConfirmDialogOz.onNegativeResponse -= onNotLoadProfile;
		UIConfirmDialogOz.onPositiveResponse -= onLoadProfile;
		
		userDecisionMade = true;
		
		//lastServerData = null;
		
		GameProfile.SharedInstance.Serialize();
		
		SendMessageToUIManager();
	}
	
	public void onLoadProfile()
	{
		UIConfirmDialogOz.onNegativeResponse -= onNotLoadProfile;
		UIConfirmDialogOz.onPositiveResponse -= onLoadProfile;
		
		userDecisionMade = true;
		
		//PlayerStats inGameStats = GameProfile.SharedInstance.Player;
		//UserProtoData userServerData = ProfileManager.SharedInstance.userServerData;

		GameProfile.SharedInstance.Player.bestCoinScore = ProfileManager.SharedInstance.lastServerData._bestCoins;
		GameProfile.SharedInstance.Player.bestDistanceScore = ProfileManager.SharedInstance.lastServerData._bestMeters;
		GameProfile.SharedInstance.Player.bestSpecialCurrencyScore = ProfileManager.SharedInstance.lastServerData._bestGems;
		GameProfile.SharedInstance.Player.bestScore = ProfileManager.SharedInstance.lastServerData._bestScore;
					
		GameProfile.SharedInstance.Player.lifetimeCoins = ProfileManager.SharedInstance.lastServerData._totalCoins;
		GameProfile.SharedInstance.Player.lifetimeDistance = ProfileManager.SharedInstance.lastServerData._totalMeters;
		GameProfile.SharedInstance.Player.lifetimeSpecialCurrency = ProfileManager.SharedInstance.lastServerData._totalGems;
		GameProfile.SharedInstance.Player.abilitiesUsed = ProfileManager.SharedInstance.lastServerData._abilitiesUsed;
			
		GameProfile.SharedInstance.Player.coinCount = ProfileManager.SharedInstance.lastServerData._currentCoins;
		GameProfile.SharedInstance.Player.specialCurrencyCount = ProfileManager.SharedInstance.lastServerData._currentGems;
		
		GameProfile.SharedInstance.Player.numberResurectsUsed = ProfileManager.SharedInstance.lastServerData._numberResurectsUsed;
		
		GameProfile.SharedInstance.Player.lifetimePlays = ProfileManager.SharedInstance.lastServerData._numberOfPlays;
		
		GameProfile.SharedInstance.Player.facebookLoginCoinsAwarded = ProfileManager.SharedInstance.lastServerData._coinRedeemFB;
		
		if (ProfileManager.SharedInstance.lastServerData._artifactLevels != null 
			&& ProfileManager.SharedInstance.lastServerData._artifactLevels.Count > 0) 
		{
			GameProfile.SharedInstance.Player.artifactLevels.Clear();
			foreach (int artifact in ProfileManager.SharedInstance.lastServerData._artifactLevels)
			{
				GameProfile.SharedInstance.Player.artifactLevels.Add(artifact);
			}
		}
		
		if (ProfileManager.SharedInstance.lastServerData._artifactsGemmed != null 
			&& ProfileManager.SharedInstance.lastServerData._artifactsGemmed.Count > 0)
		{
			GameProfile.SharedInstance.Player.artifactsGemmed.Clear();
			foreach (int artifact in ProfileManager.SharedInstance.lastServerData._artifactsDiscovered)
			{
				GameProfile.SharedInstance.Player.artifactsGemmed.Add(artifact);
			}
		}
		
		if (ProfileManager.SharedInstance.lastServerData._artifactsDiscovered != null 
			&& ProfileManager.SharedInstance.lastServerData._artifactsDiscovered.Count > 0)
		{
			GameProfile.SharedInstance.Player.artifactsDiscovered.Clear();
			foreach (int artifact in ProfileManager.SharedInstance.lastServerData._artifactsDiscovered)
			{
				GameProfile.SharedInstance.Player.artifactsDiscovered.Add(artifact);
			}
		}
		
		
		if (ProfileManager.SharedInstance.lastServerData._objectivesEarned != null 
			&& ProfileManager.SharedInstance.lastServerData._objectivesEarned.Count > 0)
		{
			GameProfile.SharedInstance.Player.objectivesEarned.Clear();
			foreach (int objective in ProfileManager.SharedInstance.lastServerData._objectivesEarned)
			{
				GameProfile.SharedInstance.Player.objectivesEarned.Add(objective);
			}
		}
		
		if (ProfileManager.SharedInstance.lastServerData._legendaryObjectivesEarned != null 
			&& ProfileManager.SharedInstance.lastServerData._legendaryObjectivesEarned.Count > 0
		) {
			GameProfile.SharedInstance.Player.legendaryObjectivesEarned.Clear();
			foreach (int legObjective in ProfileManager.SharedInstance.lastServerData._legendaryObjectivesEarned)
			{
				GameProfile.SharedInstance.Player.legendaryObjectivesEarned.Add(legObjective);
			}
		}
		
		if (ProfileManager.SharedInstance.lastServerData._powersPurchased != null 
			&& ProfileManager.SharedInstance.lastServerData._powersPurchased.Count > 0)
		{
			GameProfile.SharedInstance.Player.powersPurchased.Clear();
			foreach (int power in ProfileManager.SharedInstance.lastServerData._powersPurchased)
			{
				GameProfile.SharedInstance.Player.powersPurchased.Add(power);
			}
		}
		
		if (ProfileManager.SharedInstance.lastServerData._powersGemmed != null 
			&& ProfileManager.SharedInstance.lastServerData._powersGemmed.Count > 0)
		{
			GameProfile.SharedInstance.Player.powersGemmed.Clear();
			foreach (int power in ProfileManager.SharedInstance.lastServerData._powersGemmed)
			{
				GameProfile.SharedInstance.Player.powersGemmed.Add(power);
			}
		}
		
		if (ProfileManager.SharedInstance.lastServerData._consumablesQuantity != null 
			&& ProfileManager.SharedInstance.lastServerData._consumablesQuantity.Count > 0)
		{
			GameProfile.SharedInstance.Player.consumablesPurchasedQuantity.Clear();
			foreach (int consumable in lastServerData._consumablesQuantity)
			{
				GameProfile.SharedInstance.Player.consumablesPurchasedQuantity.Add(consumable);
			}
		}
		
		if (ProfileManager.SharedInstance.lastServerData._lifetimeStats != null
			&& ProfileManager.SharedInstance.lastServerData._lifetimeStats.Count > 0
		) {
			GameProfile.SharedInstance.Player.LifetimeStats.Clear();
			
			foreach (int envId in ProfileManager.SharedInstance.lastServerData._lifetimeStats.Keys)
			{
				GameProfile.SharedInstance.Player.LifetimeStats.Add(
					envId, 
					ProfileManager.SharedInstance.lastServerData._lifetimeStats[envId].ToFloatArray()
				);
			}
		}
		
		if (ProfileManager.SharedInstance.lastServerData._legendaryProgress != null
			&& ProfileManager.SharedInstance.lastServerData._legendaryProgress.Count > 0
		) {
			GameProfile.SharedInstance.Player.legendaryProgress.Clear();
			
			foreach (KeyValuePair<int,float> kvp in ProfileManager.SharedInstance.lastServerData._legendaryProgress)
			{
				GameProfile.SharedInstance.Player.legendaryProgress.Add(kvp.Key,(int)kvp.Value);
			}
			Services.Get<ObjectivesManager>().WebInitializeLegendaryProgress(GameProfile.SharedInstance.Player);
		}
		
		if ( ProfileManager.SharedInstance.lastServerData._objectivesUnclaimed != null
			&& ProfileManager.SharedInstance.lastServerData._objectivesUnclaimed.Count > 0
		) {
			GameProfile.SharedInstance.Player.objectivesUnclaimed.Clear();
			
			foreach ( int objectiveUnclaimed in ProfileManager.SharedInstance.lastServerData._objectivesUnclaimed )
			{
				GameProfile.SharedInstance.Player.objectivesUnclaimed.Add( objectiveUnclaimed );
			}
		}
		
		if ( ProfileManager.SharedInstance.lastServerData._environmentsVisited != null
			&& ProfileManager.SharedInstance.lastServerData._environmentsVisited.Count > 0
		) {
			GameProfile.SharedInstance.Player.environmentsVisited.Clear();
			
			foreach ( int objectiveUnclaimed in ProfileManager.SharedInstance.lastServerData._environmentsVisited )
			{
				GameProfile.SharedInstance.Player.environmentsVisited.Add( objectiveUnclaimed );
			}
		}		
		
		if ( ProfileManager.SharedInstance.lastServerData._characterUnlock != null
			&& ProfileManager.SharedInstance.lastServerData._characterUnlock.Count > 0
		) {
			foreach ( CharacterStats character in GameProfile.SharedInstance.Characters )
			{
				if ( ProfileManager.SharedInstance.lastServerData._characterUnlock.Contains( character.characterId ) )
				{
					character.unlocked = true;
				}
			}
		}
		
		if ( ProfileManager.SharedInstance.lastServerData._earnedTeamObjectives != null 
			&& ProfileManager.SharedInstance.lastServerData._earnedTeamObjectives.Count > 0
		) {
		
			WeeklyObjectives weeklyObjClass = Services.Get<ObjectivesManager>().GetWeeklyObjectivesClass();
			
			foreach ( int earnedTeamObjectiveIndex in ProfileManager.SharedInstance.lastServerData._earnedTeamObjectives )
			{
				weeklyObjClass.EarnedTeamObjectiveList.Add( earnedTeamObjectiveIndex );
			}
		}
		
		PlayerPrefs.SetInt( "abilityTutorialPlayedInt", ProfileManager.SharedInstance.lastServerData.AbilityTutorialPlayed );
		PlayerPrefs.SetInt( "utilityTutorialPlayedInt", ProfileManager.SharedInstance.lastServerData.UtilityTutorialPlayed );
		PlayerPrefs.SetInt( "gatchaCounter", ProfileManager.SharedInstance.lastServerData.GatchaTutorial );
		PlayerPrefs.Save();
		
		// Reapply the settings for the GameController and the Gatcha
		GameController.SharedInstance.SetTutorialsByPlayerPrefs();
		UIManagerOz.SharedInstance.gatchVC.SetGatchCounterFromPrefs();
		
		notify.Debug( String.Format( "Gatcha Counter PlayedInt: {0}", PlayerPrefs.GetInt( "gatchaCounter" ) ) );
		
		if ( ProfileManager.SharedInstance.lastServerData.VersionGemRedeemList != null 
			&& ProfileManager.SharedInstance.lastServerData.VersionGemRedeemList.Count > 0
			&& ProfileManager.SharedInstance.lastServerData.VersionGemRedeemList.Contains( BundleInfo.GetBundleVersion() )
		) {
			_gotGemReward = true;
		}
		
		GameProfile.SharedInstance.Player.objectivesActive.Clear();
		GameProfile.SharedInstance.Player.RefillObjectives();
		
		UIManagerOz.SharedInstance.PaperVC.UpdateCurrency();

		notify.Debug("Loaded Profile with rank: " + ProfileManager.SharedInstance.lastServerData._rank + " and total meters: " 
			+ GameProfile.SharedInstance.Player.lifetimeDistance);
		
		GameProfile.SharedInstance.Serialize();
		
		SendMessageToUIManager();
	
	}
	
	public void ResetNeighborPassed()
	{
		if (ProfileManager.SharedInstance.userServerData != null) {
			if (ProfileManager.SharedInstance.userServerData._neighborList != null
				&& ProfileManager.SharedInstance.userServerData._neighborList.Count > 0
			) {
			
				int neighborCount = ProfileManager.SharedInstance.userServerData._neighborList.Count;
				
				for (int i = 0; i < neighborCount; i++)
				{
					if (ProfileManager.SharedInstance.userServerData._neighborList[i]._bestMeters > 0)
					{
						notify.Debug("Reset passed neighbor");
						ProfileManager.SharedInstance.userServerData._neighborList[i]._passedNeighbor = false;
					}
				}
			}
		}
	}
	
	public void UpdateProfile()
	{
		notify.Debug("Updating Profile");
		
		//Grab an the instance of the PlayersStats
		//PlayerStats inGameStats = GameProfile.SharedInstance.Player;
		
		if (userServerData != null) {
			
			//Protect DB corruption to odd meters bug
			if (GameProfile.SharedInstance.Player.bestScore >= 0)
			{
				ProfileManager.SharedInstance.userServerData._bestScore = GameProfile.SharedInstance.Player.bestScore;
			}
			if (GameProfile.SharedInstance.Player.bestCoinScore >= 0)
			{
				ProfileManager.SharedInstance.userServerData._bestCoins = GameProfile.SharedInstance.Player.bestCoinScore;
			}
			if (GameProfile.SharedInstance.Player.bestDistanceScore >= 0)
			{
				ProfileManager.SharedInstance.userServerData._bestMeters = GameProfile.SharedInstance.Player.bestDistanceScore;
			}
			
			if (GameProfile.SharedInstance.Player.bestSpecialCurrencyScore >= 0)
			{
				ProfileManager.SharedInstance.userServerData._bestGems = GameProfile.SharedInstance.Player.bestSpecialCurrencyScore;
			}
			
			if (GameProfile.SharedInstance.Player.lifetimeDistance >= 0)
			{
				ProfileManager.SharedInstance.userServerData._totalMeters = GameProfile.SharedInstance.Player.lifetimeDistance;
			}
			
			if (GameProfile.SharedInstance.Player.lifetimeCoins >= 0)
			{
				ProfileManager.SharedInstance.userServerData._totalCoins = GameProfile.SharedInstance.Player.lifetimeCoins;
			}
			
			if (GameProfile.SharedInstance.Player.lifetimeSpecialCurrency >= 0)
			{
				ProfileManager.SharedInstance.userServerData._totalGems = GameProfile.SharedInstance.Player.lifetimeSpecialCurrency;
			}
			
			if (GameProfile.SharedInstance.Player.coinCount >= 0)
			{
				ProfileManager.SharedInstance.userServerData._currentCoins = GameProfile.SharedInstance.Player.coinCount;
			}
			
			if (GameProfile.SharedInstance.Player.specialCurrencyCount >= 0)
			{
				ProfileManager.SharedInstance.userServerData._currentGems = GameProfile.SharedInstance.Player.specialCurrencyCount;
			}
			
			if (GameProfile.SharedInstance.Player.lifetimePlays >= 0)
			{
				ProfileManager.SharedInstance.userServerData._numberOfPlays = GameProfile.SharedInstance.Player.lifetimePlays;
			}
			
			if (GameProfile.SharedInstance.Player.numberResurectsUsed >= 0)
			{
				ProfileManager.SharedInstance.userServerData._numberResurectsUsed = GameProfile.SharedInstance.Player.numberResurectsUsed;
			}
			
			if (GameProfile.SharedInstance.Player.facebookLoginCoinsAwarded == true)
			{
				ProfileManager.SharedInstance.userServerData._coinRedeemFB =GameProfile.SharedInstance.Player.facebookLoginCoinsAwarded;
			}
			
			// BWA 2013-04-22 In order to allow the zero rank profile, we need to reactivate 
			// the full posting of profile data.
			
//			bool loadProfile = Settings.GetBool("allow-profile-restore", false);
			
//			if (loadProfile)
//			{
			ProfileManager.SharedInstance.userServerData._rank = GameProfile.SharedInstance.Player.GetCurrentRank();

			ProfileManager.SharedInstance.userServerData._abilitiesUsed = GameProfile.SharedInstance.Player.abilitiesUsed;
			
			if (GameProfile.SharedInstance.Player.artifactLevels != null 
				&& GameProfile.SharedInstance.Player.artifactLevels.Count > 0
			) {
				ProfileManager.SharedInstance.userServerData._artifactLevels.Clear();
				foreach (int artifact in GameProfile.SharedInstance.Player.artifactLevels) 
				{	
					ProfileManager.SharedInstance.userServerData._artifactLevels.Add(artifact);
				}
			}
			
			if (GameProfile.SharedInstance.Player.artifactsDiscovered != null
				&& GameProfile.SharedInstance.Player.artifactsDiscovered.Count > 0
			) {
				ProfileManager.SharedInstance.userServerData._artifactsDiscovered.Clear();
				foreach (int artifact in GameProfile.SharedInstance.Player.artifactsDiscovered)
				{
					ProfileManager.SharedInstance.userServerData._artifactsDiscovered.Add(artifact);
				}
			}
			
			if (GameProfile.SharedInstance.Player.artifactsGemmed != null
				&& GameProfile.SharedInstance.Player.artifactsGemmed.Count > 0
			) {
				ProfileManager.SharedInstance.userServerData._artifactsGemmed.Clear();
				foreach (int artifact in GameProfile.SharedInstance.Player.artifactsGemmed)
				{
					ProfileManager.SharedInstance.userServerData._artifactsGemmed.Add(artifact);
				}
			}
			
			if (GameProfile.SharedInstance.Player.powersPurchased != null 
				&& GameProfile.SharedInstance.Player.powersPurchased.Count > 0
			) {
				ProfileManager.SharedInstance.userServerData._powersPurchased.Clear();
				foreach (int power in GameProfile.SharedInstance.Player.powersPurchased) 
				{	
					ProfileManager.SharedInstance.userServerData._powersPurchased.Add(power);
				}
			}
			
			notify.Debug("Powers Purchased count: " + GameProfile.SharedInstance.Player.powersPurchased.Count);

			if (GameProfile.SharedInstance.Player.powersGemmed != null
				&& GameProfile.SharedInstance.Player.powersGemmed.Count > 0
			) {
				ProfileManager.SharedInstance.userServerData._powersGemmed.Clear();
				foreach (int power in GameProfile.SharedInstance.Player.powersGemmed)
				{
					ProfileManager.SharedInstance.userServerData._powersGemmed.Add(power);
				}
			}
			
			if (GameProfile.SharedInstance.Player.consumablesPurchasedQuantity != null 
				&& GameProfile.SharedInstance.Player.consumablesPurchasedQuantity.Count > 0
			) {
				ProfileManager.SharedInstance.userServerData._consumablesQuantity.Clear();
				
				foreach (int consumable in GameProfile.SharedInstance.Player.consumablesPurchasedQuantity) 
				{	
					ProfileManager.SharedInstance.userServerData._consumablesQuantity.Add(consumable);
				}
			}
			
			notify.Debug("Consumable Count: " + GameProfile.SharedInstance.Player.consumablesPurchasedQuantity.Count);
			
			if (GameProfile.SharedInstance.Player.objectivesEarned != null 
				&& GameProfile.SharedInstance.Player.objectivesEarned.Count > 0
			) {
				ProfileManager.SharedInstance.userServerData._objectivesEarned.Clear();
				foreach (int objective in GameProfile.SharedInstance.Player.objectivesEarned) 
				{	
					ProfileManager.SharedInstance.userServerData._objectivesEarned.Add(objective);
				}
			}
			
			notify.Debug("GameProfile objective earned: " + GameProfile.SharedInstance.Player.objectivesEarned.Count);
			
			if (GameProfile.SharedInstance.Player.legendaryObjectivesEarned != null 
				&& GameProfile.SharedInstance.Player.legendaryObjectivesEarned.Count > 0
			) {
				ProfileManager.SharedInstance.userServerData._legendaryObjectivesEarned.Clear();
				foreach (int objective in GameProfile.SharedInstance.Player.legendaryObjectivesEarned) 
				{	
					ProfileManager.SharedInstance.userServerData._legendaryObjectivesEarned.Add(objective);
				}
			}
			
			if (GameProfile.SharedInstance.Player.LifetimeStats != null
				&& GameProfile.SharedInstance.Player.LifetimeStats.Count > 0
			){
				ProfileManager.SharedInstance.userServerData._lifetimeStats.Clear();
				
				
				foreach (int envId in GameProfile.SharedInstance.Player.LifetimeStats.Keys)
				{
					ProfileManager.SharedInstance.userServerData._lifetimeStats.Add(envId,
						new LifetimeStatEnvironmentProtoData(GameProfile.SharedInstance.Player.LifetimeStats[envId])
					);
				}
			}
			
			if (GameProfile.SharedInstance.Player.legendaryProgress != null
				&& GameProfile.SharedInstance.Player.legendaryProgress.Count > 0
			) {
				ProfileManager.SharedInstance.userServerData._legendaryProgress.Clear();
				
				foreach (KeyValuePair<int, int> kvp in GameProfile.SharedInstance.Player.legendaryProgress)
				{
					ProfileManager.SharedInstance.userServerData._legendaryProgress.Add(kvp.Key, kvp.Value);
				}
			}
		
			if ( GameProfile.SharedInstance.Player.objectivesUnclaimed != null
				&& GameProfile.SharedInstance.Player.objectivesUnclaimed.Count > 0
			) {
			
				ProfileManager.SharedInstance.userServerData._objectivesUnclaimed.Clear();
				
				foreach ( int unclaimedObjective in GameProfile.SharedInstance.Player.objectivesUnclaimed )
				{
					ProfileManager.SharedInstance.userServerData._objectivesUnclaimed.Add( unclaimedObjective );
				}
			}
			// if there are no unclaimed objectives on the game profile, ensuring the corresponding field 
			//   on userServerData is cleared.
			else
			{
				ProfileManager.SharedInstance.userServerData._objectivesUnclaimed.Clear();
			}
			
			
			if ( GameProfile.SharedInstance.Characters != null )
			{
				ProfileManager.SharedInstance.userServerData._characterUnlock.Clear();
				
				foreach ( CharacterStats character in GameProfile.SharedInstance.Characters )
				{
					if ( character.unlocked )
					{
						ProfileManager.SharedInstance.userServerData._characterUnlock.Add( character.characterId );
					}
				}
					
			}
			
			List<int> earnedTeamObjectivesIndices = Services.Get<ObjectivesManager>().GetWeeklyObjectivesClass().EarnedTeamObjectiveList;
			
			if ( earnedTeamObjectivesIndices != null && earnedTeamObjectivesIndices.Count > 0 )
			{
				ProfileManager.SharedInstance.userServerData._earnedTeamObjectives.Clear();
				
				foreach ( int earnedTeamObjectiveIndex in earnedTeamObjectivesIndices )
				{
					ProfileManager.SharedInstance.userServerData._earnedTeamObjectives.Add( earnedTeamObjectiveIndex );
				}
			}
			
//			}
#if UNITY_IPHONE		
		if ( GameCenterBinding.isGameCenterAvailable() == true )
		{
			if ( GameCenterBinding.isPlayerAuthenticated() == true )
			{
				ProfileManager.SharedInstance.userServerData._gcId = GameCenterBinding.playerIdentifier();

			}
		}
#endif
/*			
			//Get the challenges.
			ObjectivesManager objManager = Services.Get<ObjectivesManager>();
			
			List<ObjectiveProtoData> weeklyObjectiveList = objManager.GetWeeklyObjectives();
			
			//Clear the stored _guildChallengeList
//			ProfileManager.SharedInstance.userServerData._guildChallengeList.Clear();
			
			//Add any earned statvalue
			foreach(ObjectiveProtoData weeklyObj in weeklyObjectiveList)
			{
				notify.Debug ("Cycling through Objectives");
				
				notify.Debug("WeeklyObj ToDict(): " +MiniJSON.Json.Serialize(weeklyObj.ToWebObjectiveDict()));
				if (weeklyObj._guildChallenge == true)
				{
//					GuildChallengeProtoData gcProtoData = new GuildChallengeProtoData(weeklyObj.ToWebObjectiveDict());
					
					//Prevent invalid conditions from being passed to the 
					if (gcProtoData != null 
						&& gcProtoData._conditions != null
						&& gcProtoData._conditions.Count > 0
						&& gcProtoData._conditions[0]._conditionIndex != 0) {
						ProfileManager.SharedInstance.userServerData._guildChallengeList.Add(gcProtoData);
					}

					foreach (GuildChallengeProtoData guildChallenge in userServerData._guildChallengeList)
					{
						if (weeklyObj._id == guildChallenge._challengeIndex) 
						{

							//Cycle through conditions
							foreach(ConditionProtoData weeklyCondition in  weeklyObj._conditionList)
							{
								foreach (NeighborChallengeConditionProtoData guildCondition in guildChallenge._conditions)
								{
									if (guildCondition._conditionIndex == weeklyCondition._conditionIndex)
									{
										guildCondition._currentValue = weeklyCondition._earnedStatValue;
									}
								}
							}
						}
					}
					
					

				}	
			}
*/
				
			List<ObjectiveProtoData> teamObjectives 
				= Services.Get<ObjectivesManager>().GetWeeklyObjectivesClass().TeamObjectiveList;
			
			if ( teamObjectives.Count > 0 )
			{
				userServerData._guildChallengeList.Clear();
				
				foreach ( ObjectiveProtoData teamObj in teamObjectives )
				{
					GuildChallengeProtoData gcProtoData = new GuildChallengeProtoData();
					gcProtoData._challengeIndex = teamObj._id;
					gcProtoData._conditionValue = teamObj._conditionList[0]._earnedStatValue;
					
					userServerData._guildChallengeList.Add( gcProtoData );
				}
			}
			
			// Store the player prefs for tutorials.
			ProfileManager.SharedInstance.userServerData.AbilityTutorialPlayed = PlayerPrefs.GetInt( "abilityTutorialPlayedInt" );
			ProfileManager.SharedInstance.userServerData.UtilityTutorialPlayed = PlayerPrefs.GetInt( "utilityTutorialPlayedInt" );
			ProfileManager.SharedInstance.userServerData.GatchaTutorial        = PlayerPrefs.GetInt( "gatchaCounter" );
			
			// If Player was rewarded for downloading DF & WC, save the bundle version
			if ( GotGemReward )
			{
				if ( !ProfileManager.SharedInstance.userServerData.VersionGemRedeemList.Contains( BundleInfo.GetBundleVersion() ) )
				{
					ProfileManager.SharedInstance.userServerData.VersionGemRedeemList.Add( BundleInfo.GetBundleVersion() );
				}
			}
			
			notify.Debug("User Data: " + userServerData.UserToJson());
			SendProfile();
		}
	}
	
	public void CheckForProfile()
	{
		if ( GameController.SharedInstance.IsSafeToLaunchDownloadDialog()
			&& !ProfileManager.SharedInstance.UserDecisionMade
		) {
			bool profileRestore = Settings.GetBool("allow-profile-restore", false);
		
			bool rankOneProfileRestore = Settings.GetBool( "allow-rank-one-profile-restore", false );
		
			notify.Debug("[ProfileManager] CheckForProfile: Profile Restore State");
			
			if (profileRestore 
				&& ProfileManager.SharedInstance != null
				&& ProfileManager.SharedInstance.lastServerData != null
			) {
				notify.Debug("[ProfileManager] - CheckForProfile: Profile Restore: ");
			
				ProfileManager.SharedInstance.restoreProfile(gameObject);
			}
			else if (rankOneProfileRestore 
				&& ProfileManager.SharedInstance != null
				&& ProfileManager.SharedInstance.lastServerData != null
			) {
				notify.Debug(" [ProfileManager] - CheckForProfile: Rank One Profile Restore called" );
			
				ProfileManager.SharedInstance.restoreProfile( gameObject );
			}	
		}	
	}
	
	private void SendProfile ()
	{
		string jsonString = userServerData.UserToJson();
		string hashSalt = Settings.GetString("hash-salt", "");
		
		if (hashSalt != "")
		{
			string hashMessage = SaveLoad.CheckState(jsonString + hashSalt);
		
			notify.Debug("Json hash: " + hashMessage);
			
			//Build Headers
			Hashtable hashHeader = new Hashtable();
			hashHeader.Add("X_AUTHENTICATION", hashMessage);
			WWWForm profileForm = new WWWForm();
		
			profileForm.AddField("data", userServerData.UserToJson());
		
			NetAgent.Submit(new NetRequest("/user/update-profile", profileForm, SentProfile, hashHeader));
		}
	}
			
	private bool SentProfile(WWW www, bool noErrors, object webResult)
	{
		notify.Debug("Profile Sent, www.text: " + www.text + " noErrors: " + noErrors + " wwwError: " + www.error);
		
		return true;
	}
}

