using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

public class GameProfile : MonoBehaviour
{
	public static GameProfile SharedInstance;
	protected static Notify notify;
	public string SaveFilePath;
	public string SaveFilePath_Backup;
	
	public OzModelRefs BalloonPrefab;
	
	public List<PlayerStats> Players = new List<PlayerStats>();
	public List<int> CharacterOrder = new List<int>();
	public List<CharacterStats> Characters = new List<CharacterStats>();	
	
	[Serializable]
	public class ProtoCharacterVisual
	{
		public string characterName;	//This is currently just used for clarity in the editor
		
		private GameObject	cachedPrefab;
		public string		prefabPath = "";
		
		public string 	opaquePath = "";
		public string 	simplePath = "";
		private Material cachedOpaque;
		private Material cachedSimple;
		
		public Material simple_finley;
		public Material normal_finley;
		
		public string		portraitSpriteName = null;
		
		
		private static ProtoCharacterVisual currentlyLoaded;
		
		
		private void Load()
		{
			if(currentlyLoaded!=null && currentlyLoaded == this)
				return;
			
			if(currentlyLoaded!=null)
			{
				currentlyLoaded.Unload();
			}
			
			currentlyLoaded = this;
			
			cachedPrefab = (GameObject)Resources.Load(prefabPath);
			cachedOpaque = (Material)Resources.Load(opaquePath);
			cachedSimple = (Material)Resources.Load(simplePath);
		}
		
		private void Unload()
		{
		/*	if(cachedPrefab!=null)
				Resources.UnloadAsset(cachedPrefab);*/
			if(cachedOpaque!=null)
				Resources.UnloadAsset(cachedOpaque);
			if(cachedSimple!=null)
				Resources.UnloadAsset(cachedSimple);
			
			cachedPrefab = null;
			cachedOpaque = null;
			cachedSimple = null;
			
			currentlyLoaded = null;
			
			Resources.UnloadUnusedAssets();
			System.GC.Collect();
		}
		
		// jonoble added (DMTRO-2063)
		public GameObject 	prefab
		{
			get
			{
				if(cachedPrefab==null)
				{
					Load();
				}
				return cachedPrefab;
			}
		}
		
		/* jonoble removed (DMTRO-2063)
		public Transform 	prefab
		{
			get
			{
				if(cachedPrefab==null)
				{
					Load();
				}
				return cachedPrefab.transform;
			}
		}
		*/
		
		public Material 	opaque
		{
			get
			{
				if(cachedOpaque==null)
				{
					Load();
				}
				return cachedOpaque;
			}
		}
		
		public Material 	simple
		{
			get
			{
				if(cachedSimple==null)
				{
					Load();
				}
				return cachedSimple;
			}
		}
		
	}
	
	//-- Do not save this data in the GameProfile Serialize.
	public List<ProtoCharacterVisual> ProtoCharacterVisuals = new List<ProtoCharacterVisual>();
	
	public bool IsBestScoreRecord;
	public bool IsBestCoinScoreRecord;
	public bool IsBestSpecialCurrencyScoreRecord;
	public bool IsBestDistanceScoreRecord;
	//public bool ShowTutorial = true;
	//public bool ShowTutorialEnv = true;
	//public bool ShowTutorialBalloon = true;
	public bool ShowFriendMarkers = false;
	//public float SoundVolume = 0.75f;
	//public float MusicVolume = 0.3f;
	//public float Sensitivity = 0.5f;
		
	//Default values (before modifiers)
	public float DefaultMaxSpeed = 18.0f;
	public float DefaultMinSpeed = 3.0f;	
	const float DefaultHeadStart = 0.0f;	//Used in Oz (Finley's Favor)
	public float DefaultBoostDistance = 250f;		
	const float DefaultCoinMultiplier = 1f;			//Used in Oz?
	const int DefaultAdditionalScoreMultplier = 0;
	public float DefaultMagnetDuration = 10f;		//Used in Oz
	public float DefaultHypnosisDuration = 15f;
	const float DefaultResurrectionDiscount = 0f;
	const float DefaultHeadStartDiscount = 0f;
	public float DefaultCoinMeterFillCount = 100f;
	public int ScoreBonusPickupValue = 2000;
	
	public float BalloonStartSpeed = 7.5f;
	public float BalloonMaxSpeed = 15f;
	
	public float DistBetweenCoins = 1f;
	public float DistBetweenBalloonCoins = 1f;
			
	public float Accel1 = 30f;				//0% speed to 50%
	public float Accel2 = 10f;				//50% speed to 65%
	public float Accel3 = 1.5f;				//65% speed to 85%
	public float Accel4 = 0.75f;			//85% speed to 100%
	public float BalloonAccel1 = 30f;		//0% speed to 50%
	public float BalloonAccel2 = 10f;		//50% speed to 65%
	public float BalloonAccel3 = 1.5f;		//65% speed to 85%
	public float BalloonAccel4 = 0.75f;		//85% speed to 100%
	
	//Oz
	const float DefaultDoubleCoinsDistance = Mathf.Infinity;
	const float DefaultTripleCoinsDistance = Mathf.Infinity;
	
	public float DefaultPoofDuration = 10f;
	public float DefaultGemChance = 0.1f;
	public float DefaultAcceleration = 1f;
	const float DefaultCoinMeterFillRate = 1f;
	public int DefaultMegaCoinValue = 25;
	public float DefaultLuck = 1f;
	
	public float BalloonGemSpawnChance = 0.1f;
	
	public float DistInEnvZeroBeforeSetChange = 300f;
	
		
	public float EnvSignDifficultyReduction = 500f;
	public float TransitionEndDifficultyReduction = 1000f;
	
	public int deathsPerSession = 0;
	private static bool localProfileLoaded = false;
		
	public static bool GetLocalProfileLoaded() { return localProfileLoaded; }
	
	/*
	public delegate void OnLocalProfileLoadedHandler();
	protected static event OnLocalProfileLoadedHandler onLocalProfileLoadedEvent = null;
	
	public void RegisterForLocalProfileLoaded(OnLocalProfileLoadedHandler delg) {
		onLocalProfileLoadedEvent += delg;
	}
	public void UnRegisterForLocalProfileLoaded(OnLocalProfileLoadedHandler delg) {
		onLocalProfileLoadedEvent -= delg;	
	}
	*/
	
	public int AdditionalScoreMultiplier
	{
		get; set;
	}
	
	//If a challenge was earned that had a multiplier reward, the WeeklyChallenges will update the below
	public int ChallengeScoreMultiplier = 0;
	
	public PlayerStats Player 
	{ get 
		{ 
			//    mo ren di yi ge me ? gai wei moren zzx
			return FindOrCreatePlayerStats(1); 
		} 
	}	
	
	public float GetMaxSpeed() { return GetStatValue(StatType.MaxSpeed, DefaultMaxSpeed); }
	
	public float GetMinSpeed() { return GetStatValue(StatType.MinSpeed, DefaultMinSpeed); }
	
	public float GetHeadStart() { return GetStatValue(StatType.HeadStart, DefaultHeadStart); }
	
	public float GetBoostDistance() { return GetStatValue(StatType.BoostDistance, DefaultBoostDistance); }
	
	//public int GetAdditionalScoreMultiplier() {return (int)GetStatValue(StatType.ScoreMultplier, DefaultAdditionalScoreMultplier) + AdditionalScoreMultiplier; }

	public float GetCoinMultiplier() { return GetStatValue(StatType.CoinMultiplier, DefaultCoinMultiplier); }
	
	public float GetCoinMultiplierBoost() { return GetCoinMultiplier(); }
	
	public float GetMagnetDuration() { return GetStatValue(StatType.GeneralPowerDuration, DefaultMagnetDuration); }
	
	public float GetHypnosisDuration() { return GetStatValue(StatType.GeneralPowerDuration, DefaultHypnosisDuration); }
	
	public float GetMagnetDurationBoost() { return GetMagnetDuration(); }
	
	public int GetResurrectionDiscount() { return (int)GetStatValue(StatType.ResurrectionDiscount, DefaultResurrectionDiscount); }
	
	public float GetHeadStartDiscount() { return 1f + GetStatValue(StatType.HeadStartDiscount, DefaultHeadStartDiscount); }
	
	public float GetMegaHeadStartDiscount() { return 1f + GetStatValue(StatType.HeadStartDiscount, DefaultHeadStartDiscount); }
	
	public float GetEnvHeadStartDiscount() { return 1f + GetStatValue(StatType.HeadStartDiscount, DefaultHeadStartDiscount); } //added fast travel so can get discount as well N.N.
	
	public float GetShieldDurationBoost() { return GetStatValue(StatType.ShieldDuration, 10.0f); }
	
	public float GetDistanceBetweenBonusItems() { return GetStatValue(StatType.DistanceBetweenBonusItems, 500.0f); }
	
	public float GetCoinMeterFillCount() { return GetStatValue(StatType.CoinMeterFillCount, DefaultCoinMeterFillCount); }
	
	//For Oz
	public float GetDoubleCoinsDistance() { return GetStatValue(StatType.DoubleCoinsDistance, DefaultDoubleCoinsDistance); }
	
	public float GetTripleCoinsDistance() { return GetStatValue(StatType.TripleCoinsDistance, DefaultTripleCoinsDistance); }
	
	public float GetPoofDuration() { return GetStatValue(StatType.GeneralPowerDuration, DefaultPoofDuration); }
	
	public float GetGemChance() { return GetStatValue(StatType.GemChance, DefaultGemChance); }
	
	public float GetHeadStartUpgradeDist() { return GetStatValue(StatType.HeadStartIncrease, 0f); }
	
	//Same as Gem Chance, times two (with a different default value
	public float GetMegaCoinBonusChance() { return GetStatValue(StatType.GemChance, 0f) * 2f; }
	
	public int GetGemValue() { return 1;}//(int)GetStatValue(StatType.GemValue, 1); }
	
	public float GetAcceleration() { return GetStatValue(StatType.Acceleration, DefaultAcceleration); }
	
	public float GetCoinMeterFillRate() { 
		int powerID = GetActiveCharacter().powerID;
		if(powerID < PowerStore.Powers.Count)
		{
			BasePower power = PowerStore.PowerFromID(GetActiveCharacter().powerID);	//.Powers[GetActiveCharacter().powerID];
			float multiFillRate = power!=null ? power.fillRateMultiplier : 1f;
			return multiFillRate * GetStatValue(StatType.CoinMeterFillRate, DefaultCoinMeterFillRate);
		}
		return DefaultCoinMeterFillRate;
	}
	
	public int GetMegaCoinValue() { return (int)GetStatValue(StatType.MegaCoinValue, DefaultMegaCoinValue); }
	
	public float GetLuckIncrease() { return GetStatValue(StatType.LuckIncrease, DefaultLuck); }	
	
	public int GetScoreBonusPickupValue() { return ScoreBonusPickupValue; }
	
	void OnEnable() { SharedInstance = this; }
	
	void Awake() 
	{ 	
		SharedInstance = this;
		notify = new Notify(this.GetType().Name);
		notify.Debug ("GameProfile.Awake()");
		
		SetupDefaultCharacters();			//-- Find or create a new player.
		Deserialize();						//-- Load the GameProfile from Disk.
	}
	
	void Start()
	{	
		notify.Debug ("GameProfile.Start()");
		//These must come AFTER WE LOAD THE GAME (for a specific update in ObjectivesManager's legendary objectives)
		ObjectivesManager.LoadFile(ObjectivesManager.Objectives, "OZGameData/Objectives");
		ObjectivesManager.LoadFile(ObjectivesManager.LegendaryObjectives, "OZGameData/ObjectivesLegendary");		

		// wxj, load Activity objectives
		ObjectivesManager.loadActivityObjectives();
		
		ObjectiveLevelManager.LoadFile();	// load objective levels
		Store.LoadFile();					// load the purchasable items into the store
		GameTipManager.LoadFile();         	// load Game Tips if no news is displayed.
		Player.RefillObjectives();			//-- Fill in the player's active objectives if they aren't full.
		
		//Update the legendary objectives' states
		for(int i=0;i<ObjectivesManager.LegendaryObjectives.Count;i++)
		{
			GameProfile.SharedInstance.Player.UpdateLegendaryObjectiveStat(i);
		}
		
		
		/*
		if (onLocalProfileLoadedEvent != null)
		{
			onLocalProfileLoadedEvent();
		}
		*/
		
		// Initialize Sort Priority on Characters to allow adjustment by WeeklyDiscountManager
		for ( int i = 0; i < GameProfile.SharedInstance.CharacterOrder.Count; i++ )
		{
			int characterIndex = GameProfile.SharedInstance.CharacterOrder[i];
			
			GameProfile.SharedInstance.Characters[ characterIndex ].SortPriority = i * 10;
		}
		
		localProfileLoaded = true;
	}
	
	public void SetupDefaultCharacters()
	{
		if (Characters != null && Characters.Count > 0) { return; }
		
		//-- Don't add new ones if someone added them in the editor.
		//-- EDIT: Heck, Why are we even using this function?
	/*	CharacterStats guyD = new CharacterStats(0);
		//guyD.displayName = "Guy Dangerous";
		guyD.displayName = "Oscar Diggs";
		guyD.unlocked = true;
		guyD.protoVisualIndex = 0;
		Characters.Add (guyD);*/
		
	/*	CharacterStats scarlett = new CharacterStats(1);
		scarlett.displayName = "Scarlett Fox";
		scarlett.unlocked = true;
		scarlett.protoVisualIndex = 1;
		Characters.Add (scarlett);
		
		CharacterStats barry = new CharacterStats(2);
		barry.displayName = "Barry Bones";
		barry.protoVisualIndex = 2;
		barry.unlockCost = 25000;
		Characters.Add (barry);
		
		CharacterStats karma = new CharacterStats(3);
		karma.displayName = "Karma Lee";
		karma.protoVisualIndex = 3;
		karma.unlockCost = 25000;
		Characters.Add (karma);
		
		CharacterStats smith = new CharacterStats(4);
		smith.displayName = "Montanna Smith";
		smith.protoVisualIndex = 4;
		smith.unlockCost = 25000;
		Characters.Add (smith);
		
		CharacterStats montoya = new CharacterStats(5);
		montoya.displayName = "Francisco Montoya";
		montoya.protoVisualIndex = 5;
		montoya.unlockCost = 25000;
		Characters.Add (montoya);
		
		CharacterStats wonder = new CharacterStats(6);
		wonder.displayName = "Zack Wonder";
		wonder.protoVisualIndex = 6;
		wonder.unlockCost = 25000;
		Characters.Add (wonder);*/
	}
	
	public void Reset()
	{
		foreach (CharacterStats item in Characters) { item.ResetGearAndArtifacts(); }
		foreach (PlayerStats item in Players) { item.Reset(); }
		//ShowTutorial = true;
		//ShowTutorialEnv = true;
		//ShowTutorialBalloon = true;
	}
		
	public CharacterStats GetActiveCharacter()
	{
		//-- Sanity check, return guyD if something really bad happened.
		if (Player.activePlayerCharacter < 0 || Player.activePlayerCharacter >= Characters.Count) 
		{ 
			return Characters[0]; 
		}
		else return Characters[Player.activePlayerCharacter];
	}
	
	
	public static string GetAreaCharacterString()
	{
		return EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.SetCode + "_" + GameProfile.SharedInstance.GetActiveCharacter().displayName;
	}
	
//	public bool IsArtifactEquipped(int artifactID, int activeCharacterID) {
//		foreach(CharacterStats c in Characters) {
//			if(c == null)
//				continue;
//			if(c.characterId != activeCharacterID)
//				continue;
//			if(c.isArtifactEquipped(artifactID) == true)
//				return true;
//		}
//		return false;
//	}
	
	public bool IsPowerEquipped(int powerID, int activeCharacterID)
	{
		foreach(CharacterStats c in Characters) 
		{
			if (c == null) { continue; }
			if (c.characterId != activeCharacterID) { continue; }
			if (c.powerID == powerID) { return true; }
		}
		return false;
	}
	
	public int GetResurrectionCost() 
	{
		if (GameController.SharedInstance == null) { return 1; }
		int cost = GameController.SharedInstance.ResurrectionCost + GetResurrectionDiscount();
		//TR.LOG("Resurrection Cost: {0} Discount: {1}", cost, GetResurrectionDiscount());
		if (cost < 1) { cost = 1; }
		return cost;
	}
	
	//Modified GetScoreMultipliers to add in Challen
	public int GetScoreMultiplier() 
	{
	//	int completedObjectiveCount = 0;
	//	if (Player.objectivesEarned != null) { completedObjectiveCount += Player.objectivesEarned.Count; }
		// Total multiplier is 10 + count up all the completed objectives and factor in the buffs	
		/*
		return (int)GetStatValue(StatType.ScoreMultplier, 10 + completedObjectiveCount + GetAdditionalScoreMultiplier());
		*/
		
		//Add ChallengeScoreMultiplier
	/*	return (int)GetStatValue(StatType.ScoreMultplier, 
			10 + completedObjectiveCount 
			+ ChallengeScoreMultiplier
			+ GetAdditionalScoreMultiplier()
		);*/
		return GetPermanentScoreMultiplier()
			+ GetTemporaryScoreMultiplier();
	}
	
	
	public int GetTemporaryScoreMultiplier()
	{
		return (int)GetStatValue(StatType.TemporaryScoreMultiplier,0)
			+ ChallengeScoreMultiplier
			+ AdditionalScoreMultiplier;
	}
	
	
	private int CachedMultiplierForLevelsAndLegendary = 0;
	public int GetPermanentScoreMultiplier()
	{
		//int completedObjectiveCount = 0;
		//if (Player.objectivesEarned != null) { completedObjectiveCount += Player.objectivesEarned.Count; } - Objectives shouldn't increase multiplier N.N.
		
		CachedMultiplierForLevelsAndLegendary = 0;	// for adding up all earned multiplier rewards
		
		for (int r=1; r < Player.GetCurrentRank(); r++)
		{
			if (GameProfile.SharedInstance.Player.GetRankRewardTypeForLevel(r) == RankRewardType.Multipliers)
				CachedMultiplierForLevelsAndLegendary += GameProfile.SharedInstance.Player.GetRankRewardQuanityOrItemForLevel(r);		
		}		
		if(ObjectivesManager.LegendaryObjectives!=null)
		{
			for (int r=0; r < ObjectivesManager.LegendaryObjectives.Count; r++)
			{
				if (ObjectivesManager.LegendaryObjectives[r]._rewardType == RankRewardType.Multipliers
					&& GameProfile.SharedInstance.Player.legendaryObjectivesEarned.Contains(ObjectivesManager.LegendaryObjectives[r]._id)
					&& !GameProfile.SharedInstance.Player.objectivesUnclaimed.Contains(ObjectivesManager.LegendaryObjectives[r]._id))
					CachedMultiplierForLevelsAndLegendary += ObjectivesManager.LegendaryObjectives[r]._rewardValue;
			}	
		}
		
		// Total multiplier is 10 + count up all the completed objectives and factor in the buffs
		/*
		return (int)GetStatValue(StatType.ScoreMultplier, 10 + completedObjectiveCount + (int)GetStatValue(StatType.ScoreMultplier, DefaultAdditionalScoreMultplier));
		*/
		//Add ChallengeScoreMultiplier
	/*	return (int)GetStatValue(StatType.ScoreMultplier, 
			10 + additionalMultiplierFromLevelsCompletedCount
			//+ completedObjectiveCount 
			//+ ChallengeScoreMultiplier
			+ (int)GetStatValue(StatType.ScoreMultplier, DefaultAdditionalScoreMultplier));*/
		return (int)GetStatValue(StatType.ScoreMultplier,10)
			+ CachedMultiplierForLevelsAndLegendary;
				
	}	
	
//	public int GetWeeklyScoreMultiplier()
//	{
	/*	int multi = 0;
		List<ObjectiveProtoData> weeklys = Services.Get<ObjectivesManager>().GetWeeklyObjectivesClass().GetWeeklyObjectives();
		
		foreach(ObjectiveProtoData ob in weeklys)
		{
			if(ob._conditionList[0]._earnedStatValue + ob._conditionList[0]._earnedNeighborValue >= ob._conditionList[0]._statValue)
			{
				if(ob._rewardType==RankRewardType.Multipliers)
					multi+= ob._rewardValue;
			}
		}
		
		return multi;*/
//	}
	
	public float GetStatValue(StatType statType, float defaultValue)
	{
		float newValue = defaultValue;
		int max = Player.artifactsPurchased.Count;
		
		for (int i = 0; i < max; i++)
		{
			ArtifactProtoData protoData = ArtifactStore.GetArtifactProtoData(Player.artifactsPurchased[i]);
			int level = Player.GetArtifactLevel(protoData._id);
			if(protoData==null)	continue;
			
			if (protoData._statType == statType)
			{
				if (protoData._statValueType == StatValueType.Percent) { newValue += (defaultValue*(float)protoData.GetStatValue(level)); }
				else if (protoData._statValueType == StatValueType.Relative) { newValue += (float)protoData.GetStatValue(level); }
				else if (protoData._statValueType == StatValueType.Absolute) { newValue = (float)protoData.GetStatValue(level); }
			}
			
			if (Player.IsArtifactGemmed(Player.artifactsPurchased[i]) && protoData._gemmedStatType == statType)	//Is gemmed?
			{
				if (protoData._gemmedValueType == StatValueType.Percent) { newValue += (defaultValue*(float)protoData._gemmedValue); }
				else if (protoData._gemmedValueType == StatValueType.Relative) { newValue += (float)protoData._gemmedValue; }
				else if (protoData._gemmedValueType == StatValueType.Absolute) { newValue = (float)protoData._gemmedValue; }
				else if (protoData._gemmedValueType == StatValueType.CopyStatValue) { newValue = (float)protoData.GetStatValue(level); }
			}
		}
		//TR.LOG ("Stat: {0} defValue: {1} -> {2}", statType, defaultValue, newValue);
		return newValue;
	}
	
	
	// wxj, check completed of activity objective 
	private void checkCompletedActivityObj(ObjectiveProtoData data)
	{
		ConditionProtoData cond = data._conditionList[0];
		// if the objective is complete
		if(cond._earnedStatValue >= cond._statValue)
		{
			
			// wxj, unclaimed list is mark the objective that need to be rewarded
			if(!GameProfile.SharedInstance.Player.objectivesUnclaimed.Contains(data._id))
					GameProfile.SharedInstance.Player.objectivesUnclaimed.Add(data._id);
			
			
			//			AnalyticsInterface.LogGameAction("challenges", "complete", data._title,GameProfile.GetAreaCharacterString(),0);
						
			Services.Get<NotificationSystem>().SendOneShotNotificationEvent(data._id, (int)OneShotNotificationType.ActivityChallengeCompleted);	
			//Show a pop-up
			// wxj, need to modify
			PopupNotification popupNotify = PopupNotification.PopupList[PopupNotificationType.Objective];
			popupNotify.setActivitySprite(true);
			popupNotify.Show(Localization.SharedInstance.Get("Msg_ActivityChallCompleted"));
		}		
		
	}
	
	public void UpdateStatsForSession(int score, int coins, int gems, int distance)
	{
		
		// wxj, deal with activity objectives when dead
		foreach(ObjectiveProtoData data in ObjectivesManager.ActivityObjectives)
		{
			ConditionProtoData cd = data._conditionList[0];
			if(cd._earnedStatValue < cd._statValue) 
			{
				switch(cd._type)
				{
				case ObjectiveType.Activity3:
					if(distance >= 1500)
					{
						cd._earnedStatValue++;
						checkCompletedActivityObj(data);
						UnityEngine.Debug.Log("wxj: Activity3 _earnedStatValue:"+cd._earnedStatValue);
					}
					break;
					
				case ObjectiveType.Activity4:
					// wxj, when break record of distance and rewardable
					if(distance >= 1000 && distance > Player.bestDistanceScore && cd._actiRewardedCount < 5)
					{
						if(distance >= 1000 && distance < 5000)
						{
							data._rewardType = RankRewardType.Coins;
							data._rewardValue = 500;
						}
						else if(distance >= 5000 && distance < 10000)
						{
							data._rewardType = RankRewardType.Coins;
							data._rewardValue = 1000;
						}
						else if(distance >= 10000)
						{
							data._rewardType = RankRewardType.Gems;
							data._rewardValue = 1;	
						}
						
						cd._earnedStatValue = 1;
						checkCompletedActivityObj(data);
						UnityEngine.Debug.Log("wxj: Activity4 _earnedStatValue:"+cd._earnedStatValue);
					}
					else
					{
						data._rewardType = RankRewardType.Coins;
						data._rewardValue = 500;
					}
					break;
				}
				
				
				// save objective condition
				ObjectivesManager.saveFileToPersistent(ObjectivesManager.AllActivityObjectives, "ObjectivesActivity");
			}
			
			
		}
		
		
		
		
		// Reset any flags
		IsBestScoreRecord = false;
		IsBestCoinScoreRecord = false;
		IsBestSpecialCurrencyScoreRecord = false;
		IsBestDistanceScoreRecord = false;
		
		//		PurchaseUtil.bIAnalysisWithParam("Player_Gems","Add_Gems|"+gems);
		
		PlayerStats r = Player;

		// Update lifetime stat s
		r.lifetimePlays++;
	//	r.lifetimeCoins += coins;
		r.lifetimeSpecialCurrency += gems;
		r.lifetimeDistance += distance;

		// Update High Score Stats
		if (score > r.bestScore) 
		{
			r.bestScore = score;
			IsBestScoreRecord = true;
		}

	//	if (coins > r.bestCoinScore) 
	//	{
	//		r.bestCoinScore = coins;
	//		IsBestCoinScoreRecord = true;
	//	}
		
		if (gems > r.bestSpecialCurrencyScore) 
		{
			r.bestSpecialCurrencyScore = gems;
			IsBestSpecialCurrencyScoreRecord = true;
		}

		if (distance > r.bestDistanceScore) 
		{
			r.bestDistanceScore = distance;
			IsBestDistanceScoreRecord = true;
		}
		
		//NOTE!! I've commented out all of the coins stuff because this is calculated inUpdateCoinsPostSession now.	-bc
		
		//r.coinCount += coins;				// Update active coin count
		
		r.specialCurrencyCount += gems;		// Update active gem count
		r.gameCenterNeedsUpdate = true;		// Make sure game center gets update
		
	
	}
	
	public int UpdateCoinsPostSession(int coins, bool updateLastRunCoinCount)
	{
		//		PurchaseUtil.bIAnalysisWithParam("Player_Coins","Add_Coins|"+coins);
		
		PlayerStats r = Player;
		
		r.coinCount += coins;
		
		if(coins>0)
			r.lifetimeCoins += coins;
		
		if(updateLastRunCoinCount)
			GamePlayer.SharedInstance.CoinCountTotal += coins;

		if (GamePlayer.SharedInstance.CoinCountTotal > r.bestCoinScore) 
		{
			r.bestCoinScore = GamePlayer.SharedInstance.CoinCountTotal;
			IsBestCoinScoreRecord = true;
		}
		
		return GamePlayer.SharedInstance.CoinCountTotal;
	}
	
	PlayerStats FindPlayerStats(int playerID)
	{
		if (Players == null) { return null; }
		
		int count = Players.Count;
		for (int i = 0; i<count; i++) 
		{
			if (Players[i] == null) { continue; }
			if (Players[i].playerId == playerID) { return Players[i]; }
		}
		return null;
	}
	
	public PlayerStats FindOrCreatePlayerStats(int playerId, bool allowDeserialize = true)
	{
		if (allowDeserialize == false)
		{
			notify.Debug("FindOrCreatePlayerStats start playerId={0} allowDeserialize={1}",playerId, allowDeserialize);
		}

		PlayerStats stats = FindPlayerStats(playerId);
		
		if(stats==null && allowDeserialize)
		{
			notify.Debug("stats is null, calling Deserialize");

			Deserialize();
			stats = FindPlayerStats(playerId);
		}
		if (stats == null) 
		{
			stats = new PlayerStats(playerId);
			Players.Add (stats);
			SetupDefaultCharacters();
		}
		return stats;
	}
	
	
	private string GetV7FullSaveFilePath() {
#if UNITY_IPHONE || UNITY_EDITOR
		return Application.persistentDataPath + Path.DirectorySeparatorChar + SaveFilePath;
#else
		return Application.persistentDataPath + Path.DirectorySeparatorChar + "stats.sav";
#endif
		
	}
	
	private string GetV7FullSaveFilePathBackup() {
#if UNITY_IPHONE || UNITY_EDITOR
		return Application.persistentDataPath + Path.DirectorySeparatorChar + SaveFilePath_Backup;
#else
		return Application.persistentDataPath + Path.DirectorySeparatorChar + "android_settings.oz";
#endif
	}
	
	private string GetFullSaveFilePath() {
		return Application.persistentDataPath + Path.DirectorySeparatorChar + "stats.sav";
		
	}
	
	private string GetFullSaveFilePathBackup() {
#if UNITY_IPHONE || UNITY_EDITOR
		return Application.persistentDataPath + Path.DirectorySeparatorChar + "ios_settings.oz";
#else
		return Application.persistentDataPath + Path.DirectorySeparatorChar + "android_settings.oz";
#endif
	}
	
	private static int Version = 8;
	
	private static bool hasAttemptedLoad = false;
	public void Serialize()
	{
		if(!hasAttemptedLoad)
			return;
		
		if(Application.isPlaying && Player!=null)
			Player.RecordLegendaryProgress();
		
		Dictionary<string, object> saveData = new Dictionary<string, object>();
		saveData.Add ("version", GameProfile.Version);
		
		List<object> convertedData = new List<object>();
		foreach (PlayerStats item in Players) { convertedData.Add (item.ToDict()); }
		saveData.Add ("Players", convertedData);
		
		List<object> convertedCharacterData = new List<object>();
		foreach (CharacterStats item in Characters) { convertedCharacterData.Add(item.ToDict()); }
		saveData.Add ("Characters", convertedCharacterData);
		
		//VERSION 8 ONLY! This is a secret "tag" that verifies that we are using a version 8 save file.
		//	If people just revert the save to version 7, they can change whatever they want! This data does
		//	not actually mean anything gameplay-wise.
		saveData.Add("Records",MiniJSON.Json.Serialize(Player.consumablesPurchasedQuantity));
		
		//saveData.Add ("ShowTutorial", ShowTutorial);
		//saveData.Add ("ShowTutorialEnv", ShowTutorialEnv);
		//saveData.Add ("ShowTutorialBalloon", ShowTutorialBalloon);
		saveData.Add ("ShowFriendMarkers", ShowFriendMarkers);
		//saveData.Add ("SoundVolume", SoundVolume);
		//saveData.Add ("MusicVolume", MusicVolume);
		//saveData.Add ("Sensitivity", Sensitivity);
		//Debug.Log ("saving SoundVolume " + SoundVolume);
		//Debug.Log ("saving MusicVolume " + MusicVolume);
		
		
		//-- Hash before we save.
		Dictionary<string, object> secureData = SaveLoad.Save(saveData);
		string jsonString = MiniJSON.Json.Serialize(secureData);
		
		string fileName = GetFullSaveFilePath();
		string fileNameBackup = GetFullSaveFilePathBackup();
		if(notify!=null)
			notify.Debug("Saving GameProfile to: " + fileName);
			
		bool failed = false;
		//Save two copies, in case one gets corrupted
		try 
		{
			using (StreamWriter fileWriter = File.CreateText(fileName)) 
			{
				fileWriter.WriteLine(jsonString);
				fileWriter.Close(); 
			}
			
			if(notify!=null)
				notify.Debug("Saving Backup GameProfile to: " + fileNameBackup);
			using (StreamWriter fileWriter = File.CreateText(fileNameBackup)) 
			{
				fileWriter.WriteLine(jsonString);
				fileWriter.Close(); 
			}
		}
		catch (Exception e) 
		{
			failed = true;
			Dictionary<string,string> d = new Dictionary<string, string>();
			d.Add("Exception",e.ToString());
			notify.Warning("Save Exception: " + e);
		}
		
#if UNITY_IPHONE && !UNITY_EDITOR
		//ONLY DO THIS ON IOS! Android V7 save location is the same as V8
		if(!failed)
		{
			//Remove V7 saves
			if(File.Exists(GetV7FullSaveFilePath()))
				File.Delete(GetV7FullSaveFilePath());
			if(File.Exists(GetV7FullSaveFilePathBackup()))
				File.Delete(GetV7FullSaveFilePathBackup());
		}
#endif
		
		if(notify!=null)
			notify.Debug("GameProfile Saved. failed=" + failed);
	}
	
	public bool Deserialize()
	{
		hasAttemptedLoad = true;
		
		bool isUsingV7 = false;
		
		string fileName = GetFullSaveFilePath();
		if (File.Exists(fileName) == false)
		{
			//NOTE: Don't notify.Warning the path name, we dont want people to see where we are saving
		//	notify.Warning("No GameProfile exists at: " + fileName);
			
			fileName = GetFullSaveFilePathBackup();
			
			if(File.Exists(fileName) == false)
			{
		//		notify.Warning("No Backup GameProfile exists at: " + fileName);
			
				isUsingV7 = true;
				fileName = GetV7FullSaveFilePath();
				
				if(File.Exists(fileName) == false)
				{
					fileName = GetV7FullSaveFilePathBackup();
				
					if(File.Exists(fileName) == false)
					{
						//notify.Warning("No V7 Backup GameProfile exists at: " + fileName);
						return false;
					}
				}
			}
			else
			{
			//	notify.Warning("Using backup file!");
			}
		}
		
		bool success = false;

		notify.Debug("Reading GameProfile from: " + fileName);
		//NOTE!!!! If we use "using" or "Dispose()", the game crashes if the device is in airplane mode!!!!! For now, we are not disposing of the StreamReader.
		//using (StreamReader reader = File.OpenText(fileName)) {
		
		StreamReader reader = File.OpenText(fileName);
		string jsonString = reader.ReadToEnd();
		
		//notify.Debug("GameProfile " + jsonString);
		Dictionary<string, object> loadedData = MiniJSON.Json.Deserialize(jsonString) as Dictionary<string, object>;

		if (SaveLoad.Load(loadedData))
		{
			
			//-- Security check
			Dictionary<string, object> secureData = loadedData["data"] as Dictionary<string, object>;
			long readInVersion = 0;
			if (secureData.ContainsKey("version"))
			{
	
				readInVersion = (long)secureData["version"];
				//bool success = false;
				
				switch(readInVersion) 
				{
				case 7:
					if(!isUsingV7)
						success = false;
					else
						success = DeseralizeVersion7(secureData);
					break;
				case 8:
					success = DeseralizeVersion7(secureData);
					break;
				default:
					success = false;
					break;
				}
			}
		}
			
		//-- Failed to load, try the backup 
		if (success == false) {
			reader.Close();
			
			fileName = isUsingV7 ? GetV7FullSaveFilePathBackup() : GetFullSaveFilePathBackup();
			
			if(File.Exists(fileName) == false)
			{
				//notify.Warning("No Backup GameProfile exists at: " + fileName);
				return false;
			}
			
			reader = File.OpenText(fileName);
			jsonString = reader.ReadToEnd();
			
			loadedData = MiniJSON.Json.Deserialize(jsonString) as Dictionary<string, object>;
			
			//Get the version of the file first, because version 7 saves might have hash problems.
			Dictionary<string, object> secureData = new Dictionary<string, object>();
			long readInVersion = 0;
			if (loadedData != null)
			{
				secureData = loadedData["data"] as Dictionary<string, object>;
				readInVersion = Version;
				if(secureData.ContainsKey("version"))
				{
					readInVersion = (long)secureData["version"];
				}
			}
			if(SaveLoad.Load(loadedData) || (readInVersion < 8 && !secureData.ContainsKey("Records")))
			{
				//success! hashes match.
				if(readInVersion < 8)
				{
					notify.Debug("Old version = "+readInVersion);
#if UNITY_ANDROID && !UNITY_EDITOR
					isUsingV7 = true;
#endif
				}
			}
			else
			{
				notify.Error("ERROR! HASHES ARE DIFFERENT!!! Not loading.");
			}
			
			//Do it anyway. The hashes have been giving us trouble.
			if (secureData.ContainsKey("version"))
			{
				//bool success = false;
				
				switch(readInVersion) 
				{
				case 7:
					if(!isUsingV7)
						success = false;
					else
						success = DeseralizeVersion7(secureData);
					break;
				case 8:
					success = DeseralizeVersion7(secureData);
					break;
				default:
					success = false;
					break;
				}
			}
			
		}
		
		if(success==false) 
		{
			notify.Debug("success was false in Deserialize line 935");
		}
		else 
		{
			/*
			Dictionary<string, string> data = new Dictionary<string, string>();
			data.Add("lifetimePlays", Player.lifetimePlays.ToString());
			data.Add("lifetimeCoins", Player.lifetimeCoins.ToString());
			data.Add("lifetimeGems", Player.lifetimeSpecialCurrency.ToString());
			data.Add("lifetimeDistance", Player.lifetimeDistance.ToString());
			data.Add("lifetimeBestScore", Player.bestScore.ToString());
			data.Add("lifetimeBestCoinsScore", Player.bestCoinScore.ToString());
			data.Add("lifetimeBestGemScore", Player.bestSpecialCurrencyScore.ToString());
			FlurryBinding.logEventWithParameters("LifeTimeStats", data, false);
			*/
		}
		notify.Debug("************* about to call reader.Dispose");
		reader.Dispose();
		notify.Debug("Done calling reader.Dispose");
		//}
		
		FixUpSavedEnvSetAfterUpdate();
		ForceHiResNotPresentAfterUpdate();
		
		notify.Debug("Deserialize returning success");
		return success;
	}
	
	private bool DeseralizeVersion7(Dictionary<string, object> loadedData)
	{
		notify.Debug("GameProfile.DeseralizeVersion7");
		bool success = true;
		
		if (loadedData.ContainsKey("Players") == true)
		{
			List<object> loadedPlayers = loadedData["Players"] as List<object>;
			if (loadedPlayers != null) 
			{
				Players.Clear();
				
				foreach (object item in loadedPlayers) 
				{
					Dictionary<string, object> data = item as Dictionary<string, object>;
					Players.Add(new PlayerStats(data));	
					
				}
				notify.Debug("Players.Count = " + Players.Count);
				
				if (ObjectivesManager.Objectives == null || ObjectivesManager.Objectives.Count == 0)
					ObjectivesManager.LoadFile(ObjectivesManager.Objectives, "OZGameData/Objectives");
				
				if (ObjectivesManager.LegendaryObjectives == null || ObjectivesManager.LegendaryObjectives.Count == 0)
					ObjectivesManager.LoadFile(ObjectivesManager.LegendaryObjectives, "OZGameData/ObjectivesLegendary");				
			}
			else { success = false; }
		} 
		else { success = false; }
		
		notify.Debug("calling FindOrCreatePlayerStats(1, allowDeserialize = false);");
		FindOrCreatePlayerStats(1, allowDeserialize: false);
		notify.Debug("done calling FindOrCreatePlayerStats(1, allowDeserialize = false);");
		
		
		
		//if (loadedData.ContainsKey("ShowTutorial")) { ShowTutorial = (bool)loadedData["ShowTutorial"]; }
		//if (loadedData.ContainsKey("ShowTutorialEnv")) { ShowTutorialEnv = (bool)loadedData["ShowTutorialEnv"]; }
		//if (loadedData.ContainsKey("ShowTutorialBalloon")) { ShowTutorialBalloon = (bool)loadedData["ShowTutorialBalloon"]; }
		
		if (loadedData.ContainsKey("ShowFriendMarkers")) { ShowFriendMarkers = (bool)loadedData["ShowFriendMarkers"]; }
		/*
		if (loadedData.ContainsKey("SoundVolume"))
		{
			SoundVolume = (float)JSONTools.ReadDouble(loadedData["SoundVolume"]);
			if (AudioManager.SharedInstance != null) { 
				AudioManager.SharedInstance.SoundVolume = SoundVolume;
			}
			//if (UIManagerOz.SharedInstance && UIManagerOz.SharedInstance.settingsVC){
			//	UIManagerOz.SharedInstance.settingsVC.soundSlider.sliderValue = SoundVolume;
			//}
		}
		
		if (loadedData.ContainsKey("MusicVolume")) 
		{
			//REDMOND FIX
			//MusicVolume = 0.15f;
			
			MusicVolume = (float)JSONTools.ReadDouble(loadedData["MusicVolume"]);
			if (AudioManager.SharedInstance != null) { 
				AudioManager.SharedInstance.MusicVolume = MusicVolume;
			}
			//if (UIManagerOz.SharedInstance && UIManagerOz.SharedInstance.settingsVC){
			//	UIManagerOz.SharedInstance.settingsVC.musicSlider.sliderValue = MusicVolume;
			//}
			
		}
		if (loadedData.ContainsKey("Sensitivity")) 
		{
			Sensitivity = (float)JSONTools.ReadDouble(loadedData["Sensitivity"]);
			if(TouchInput.Instance){
				TouchInput.Instance.Sensitivity = Sensitivity;
			}

		}
		*/
		if (loadedData.ContainsKey("Characters") == true) 
		{
			List<object> loadedChars = loadedData["Characters"] as List<object>;
			if(loadedChars != null) 
			{
			//	Characters.Clear();
				
				for(int i=0;i<loadedChars.Count;i++)// (object item in loadedChars)
				{
					object item = loadedChars[i];
					Dictionary<string, object> data = item as Dictionary<string, object>;
			//		Characters.Add ( new CharacterStats(data));		
					if(Characters.Count>i)
					{
						//Only update important values (unlocked/powerID/etc.)
						Characters[i].UpdateWithSerializedData(data);
					}
				}
			}
			else { success = false; }
		} 
		else { success = false; }
		
		return success;
	}
	
	/// <summary>
	/// Force you back to Whimsey Woods if you update your client
	/// </summary>
	private void FixUpSavedEnvSetAfterUpdate()
	{	
		notify.Debug("FixUpSavedEnvSetAfterUpdate start");
		// to avoid an infinite loop we need to do this check
		if (FindPlayerStats(1) != null)
		{
			notify.Debug("HasUpdateTakenPlace returned true");
#if UNITY_ANDROID
			if (GameController.IsDeviceLowEnd() || SystemInfo.deviceModel == "Amazon Kindle Fire")
			{
				PlayerPrefs.SetInt("UserSetQuality", 0);
				PlayerPrefs.Save();		
			}
#endif
			if (HasMajorOrMinorUpdateTakenPlace())			// check if an update has been completed since last launch
			{
				notify.Debug("HasUpdateTakenPlace returned true");
				Player.savedEnvSet = EnvironmentSetManager.WhimsyWoodsId; // reset to Whimsey Woods, due to update
				Serialize();								// store the change back to Whimsey Woods in the profile	
			}
		}
	}	
	
	private void ForceHiResNotPresentAfterUpdate()
	{	
		notify.Debug("ForceHiResNotPresentAfterUpdate start");
		
		// to avoid an infinite loop we need to do this check
		if (FindPlayerStats(1) != null)
		{
			if (HasMajorOrMinorUpdateTakenPlace() || FixNeededForStartupHiResBlackScreenIssue())
			{
				notify.Debug("(HasMajorOrMinorUpdateTakenPlace() || FixNeededForStartupHiResBlackScreenIssue()) returned true");
				ResourceManager.SharedInstance.downloadedAssetBundles.ForceHiResNotPresent();
				Serialize();
			}
		}
	}	
	
	/// <summary>
	/// Determines whether this instance has update taken place.
	/// </summary>
	/// <returns>
	/// <c>true</c> if this instance has update taken place; otherwise, <c>false</c>.
	/// </returns>
	private bool HasMajorOrMinorUpdateTakenPlace()	// check if an update has been completed since last launch
	{
		notify.Debug("HasMajorOrMinorUpdateTakenPlace start");
		string savedVersion = "1.0";		
		
		if (Player.majorVersion == -1 && Player.minorVersion == -1)
		{
			notify.Debug("savedVersion = 1.0 in HasMajorOrMinorUpdateTakenPlace, where both major & minor versions == -1");
			savedVersion = "1.0";	// older than version 1.2...
		}
		else
		{
			notify.Debug("savedVersion = " + Player.majorVersion.ToString() + '.' + Player.minorVersion.ToString());
			savedVersion = Player.majorVersion.ToString() + "." + Player.minorVersion.ToString();
		}
		
		float previousVersion = (float)Double.Parse(savedVersion);
		float currentVersion = (float)Double.Parse(ResourceManager.GetVersionString());
		
		return (currentVersion > previousVersion);
	}	
	
	private bool HasRevisionTakenPlace()
	{
		notify.Debug("HasRevisionTakenPlace start");
		
		int previousRevision = Player.revisionVersion;
		int currentRevision = ResourceManager.GetVersionNumberComponent(2);
			
		return (currentRevision > previousRevision);
	}
	
	private bool FixNeededForStartupHiResBlackScreenIssue()		// fix for DMTRO-2259, only when updating from 1.4.0 to 1.4.1
	{
		string previousVersion = Player.majorVersion.ToString() + "." + 
			Player.minorVersion.ToString() + "." + Player.revisionVersion.ToString();
		
		string currentVersion = BundleInfo.GetBundleVersion();
		
		return (currentVersion == "1.4.1" && previousVersion == "1.4.0");
	}
}

public static class SaveLoad 
{
#pragma warning disable 414
	static Notify notify = new Notify("SaveLoad");
#pragma warning restore 414
	//-- The names of these methods are INTENTIONALLY named incorrect for the prying eyes of users who care to look at the DLLs at runtime.
	private static string saveClass = "BonusItemProtoData";
	
	//-- Md5 the data and check against the read in hash.
	public static bool Load(Dictionary<string, object> data) 
	{
		bool success = false;
		if (data == null)
		{
			notify.Warning ("SaveLoad.Load(data)  data is null");
			return success;
		}
		
		string hashFromFile = null;
		if (data.ContainsKey("hash")) { hashFromFile = (string)data["hash"]; }
		
		if (data.ContainsKey("data") && hashFromFile != null)
		{
			//-- Security check
			object secureData = data["data"] as object;
			string hash = SaveLoad.CheckState(MiniJSON.Json.Serialize(secureData) + saveClass);
			if (hash.CompareTo(hashFromFile) == 0) { 
				success = true; }
			else { 
				notify.Warning ("hashed are diff. {0} != {1}", hash, hashFromFile); }
		}
		else 
		{
			if (hashFromFile == null) { 
				notify.Warning ("hashFromFile needs to be not null"); }
			if (data.ContainsKey("data") == false) { 
				notify.Warning ("no data node found.");	 }
		}
		return success;
	}
	
	//-- MD5 the data and return a new dictionary with that hash.
	public static Dictionary<string, object> Save(object data) 
	{
		string hash = SaveLoad.CheckState(MiniJSON.Json.Serialize(data) + saveClass);
		Dictionary<string, object> secureData = new Dictionary<string, object>();
		secureData.Add ("hash", hash);
		//TR.LOG ("Hash = {0}", hash);
		secureData.Add ("data", data);
		return secureData;
	}
	
	//-- Md5 the string and return a hash.
	//private static string CheckState(string strToEncrypt)
	//Making hashing go Public!
	public static string CheckState(string strToEncrypt)
	{
	    UTF8Encoding encoding = new System.Text.UTF8Encoding();
	    byte[] bytes = encoding.GetBytes(strToEncrypt);
	 
	    // encrypt bytes
	    MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
	    byte[] hashBytes = md5.ComputeHash(bytes);
	 
	    // Convert the encrypted bytes back to a string (base 16)
	    string hashString = "";
	 
	    for (long i = 0; i < hashBytes.Length; i++)
	    {
	        hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, "0"[0]);
	    }
	 
	    return hashString.PadLeft(32, "0"[0]);
	}
}




		
//		if (currentVersion > previousVersion)
//		{		
//			notify.Debug("currentVersion > previousVersion returning true");
//			return true;
//		}
//		else
//		{
//			notify.Debug("currentVersion > previousVersion returning false");
//			return false;
//		}
