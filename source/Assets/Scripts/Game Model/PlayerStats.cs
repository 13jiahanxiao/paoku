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
using System.Linq;

[Serializable]
public class PlayerStats
{
	protected static Notify notify = new Notify("PlayerStats");
	public static int Version = 1;
	public int playerId;
	public int bestScore;
	public int bestCoinScore;
	public int bestSpecialCurrencyScore;
	public int bestDistanceScore;
	public int lifetimePlays;
	public int lifetimeCoins;
	public int lifetimeSpecialCurrency;
	public int lifetimeDistance;
	public int coinCount = defaultCoinCount;				// total coins the player has, not 'per run'
	public int specialCurrencyCount;	// total gems the player has, not 'per run'
	public int activePlayerCharacter;
	public bool gameCenterNeedsUpdate;
	public bool facebookLoginCoinsAwarded; // Whether or not the user has been awarded 500 coins for logging into Facebook
	
	public int abilitiesUsed;
	
	public int previousScoreRank = -1;
	public int previousDistanceRank = -1;

	public List<int> artifactsPurchased	// for backward compatibility with Imangi's list keeping track only of IDs of purchased artifacts
	{									// we needed to also keep track of how many purchases were made (up to 5), for the 'levels' of the artifacts
		get
		{
			List<int> purchasedArtifactIDs = new List<int>();
			for (int i=0; i < artifactLevels.Count; i++)
			{
				if (artifactLevels[i] > 0)
					purchasedArtifactIDs.Add(i);
			}
				
			return purchasedArtifactIDs;
		}
	}
	
	public List<int> artifactLevels;
	public List<int> artifactsGemmed;
	public List<int> artifactsDiscovered;
	public List<int> objectivesEarned;
	public List<int> legendaryObjectivesEarned;
	public List<int> objectivesUnclaimed = new List<int>();
	public List<int> environmentsVisited = new List<int>();
	//public List<int> objectivesEarnedStatValues;
	
	[System.NonSerialized]
	public List<ObjectiveProtoData> objectivesActive = new List<ObjectiveProtoData>();
	public List<string> objectivesEarnedDuringRun;
	public List<int> powersPurchased;
	public List<int> powersGemmed;
	public List<int> consumablesPurchasedQuantity;	
	
	public int savedEnvSet = EnvironmentSetManager.WhimsyWoodsId;
	
	public int majorVersion = -1;
	public int minorVersion = -1;
	public int revisionVersion = 0;
	
	//public int gotGemReward = 0;
	
	//NOTE: Is important that this stays an integer dictionary so that the save file will pass the hash check.
	[System.NonSerialized]
	public Dictionary<int,int> legendaryProgress = new Dictionary<int, int>();
	
	private Dictionary<int, float[]> lifetimeStats = new Dictionary<int, float[]>();
	public Dictionary<int, float[]> LifetimeStats
	{
		get {
			if(lifetimeStats==null)	lifetimeStats = new Dictionary<int, float[]>();
				return lifetimeStats;
		}
		set
		{
			lifetimeStats = value;
		}
	}
	
	
	public List<Buff> buffs;
	public int randomSeed;
	public int numberResurectsUsed;
	public int numberChanceTokens;
	public int numberChanceTokensThisRun;
	
	
	private const int defaultCoinCount = 1500;
	
	//private Rand rand = null;	
	
	public Dictionary<string, object> ToDict() {
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add ("v", PlayerStats.Version);
		data.Add ("pid", playerId);
		data.Add ("bestScore", bestScore);
		data.Add ("bestCoinScore", bestCoinScore);
		data.Add ("bestSpecialCurrencyScore", bestSpecialCurrencyScore);
		data.Add ("bestDistanceScore", bestDistanceScore);
		data.Add ("lifetimePlays", lifetimePlays);
		data.Add ("lifetimeCoins", lifetimeCoins);
		data.Add ("lifetimeSpecialCurrency", lifetimeSpecialCurrency);
		data.Add ("lifetimeDistance", lifetimeDistance);
		data.Add ("coinCount", coinCount);
		data.Add ("specialCurrencyCount", specialCurrencyCount);
		data.Add ("activePlayerCharacter", activePlayerCharacter);
		data.Add ("gameCenterNeedsUpdate", gameCenterNeedsUpdate);
		data.Add ("facebookLoginCoinsAwarded", facebookLoginCoinsAwarded);
		data.Add ("abilitiesUsed", abilitiesUsed);
		//data.Add ("artifactsPurchased", artifactsPurchased);	// if artifactLevels[index] > 0, it means it's purchased
		data.Add ("artifactLevels", artifactLevels);
		data.Add ("artifactsGemmed", artifactsGemmed);
		data.Add ("artifactsDiscovered", artifactsDiscovered);
		data.Add ("objectives", objectivesEarned);
		data.Add ("legendaryObjectives", legendaryObjectivesEarned);
		data.Add ("objectivesUnclaimed", objectivesUnclaimed);
		data.Add ("environmentsVisited", environmentsVisited);
		//data.Add ("objectivesEarnedStatValues", objectivesEarnedStatValues);		
		data.Add ("powersPurchased", powersPurchased);
		data.Add ("powersGemmed", powersGemmed);
		data.Add ("consumablesPurchasedQuantity", consumablesPurchasedQuantity);		
		data.Add ("randomSeed", randomSeed);
		data.Add ("numberResurectsUsed", numberResurectsUsed);
		data.Add ("numberChanceTokens", numberChanceTokens);
		data.Add ("legendaryProgressData", legendaryProgress);
		//data.Add ("gotGemReward", gotGemReward);
		
		data.Add( "previousScoreRank", previousScoreRank );
		data.Add( "previousDistanceRank", previousDistanceRank );

		if(EnvironmentSetManager.SharedInstance!=null && EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet != null)
			savedEnvSet = EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.SetId;
		data.Add ("savedEnvSet", savedEnvSet);

		data.Add ("majorVersion", ResourceManager.GetVersionNumberComponent(0));	//false));	// get version number before saving
		data.Add ("minorVersion", ResourceManager.GetVersionNumberComponent(1));	//true));	
		data.Add ("revisionVersion", ResourceManager.GetVersionNumberComponent(2));	
		
		//convert lifetime stats to enum based dictionary
		//IMPORTANT: The actual dictionary is using floats, but we are using ints in the save file so that it can pass the hash check
		Dictionary<int, Dictionary<ObjectiveType,int>> lifetimeStats = new Dictionary<int, Dictionary<ObjectiveType, int>>();
		foreach(int envid in LifetimeStats.Keys)
		{
			float[] source = LifetimeStats[envid];
			Dictionary<ObjectiveType, int> dest = new Dictionary<ObjectiveType, int>();
			lifetimeStats.Add(envid, dest);
			for(ObjectiveType objective = (ObjectiveType)0; objective < ObjectiveType.LifetimeObjectivesCount; ++objective)
			{
				dest.Add(objective, (int)source[(int)objective]);
			}
		}
		
		data.Add ("LifetimeStats",lifetimeStats);
		
		List<object> convertedData = new List<object>();
		foreach (Buff item in buffs) {
			convertedData.Add ( item.ToDict() );
		}
		data.Add ("Buffs", convertedData);
		
		List<object> objectivesActiveData = new List<object>();
		foreach (ObjectiveProtoData item in objectivesActive) {
			if(item == null)
			{
				objectivesActiveData.Add(null);
			}
				//continue;
			else
				objectivesActiveData.Add(item.ToDict());
		}
		data.Add ("objectivesActiveData", objectivesActiveData);
		
		
		return data;
	}
	
	public PlayerStats(Dictionary<string, object> data)
	{
		Reset();
		
//		TR.LOG ("GetCurrentRank()={0}", GetCurrentRank());
//		TR.LOG ("GetMaxRank()={0}", GetMaxRank());
//		TR.LOG ("GetCurrentRankProgress()={0}", GetCurrentRankProgress());
//		TR.LOG ("ObjectivesManager.Objectives.Count={0}", ObjectivesManager.Objectives.Count);
		
		int version = 0;
		if(data.ContainsKey("v")) {
			object obj = data["v"];
			version = JSONTools.ReadInt(obj);
		}
		
		if(version >= 1) {
			if(data.ContainsKey("pid")) {
				playerId=JSONTools.ReadInt(data["pid"]);
			}
			if(data.ContainsKey("bestScore")) {
				bestScore=JSONTools.ReadInt(data["bestScore"]);
			}
			if(data.ContainsKey("bestCoinScore")) {
				bestCoinScore=JSONTools.ReadInt(data["bestCoinScore"]);
			}
			if(data.ContainsKey("bestSpecialCurrencyScore")) {
				bestSpecialCurrencyScore=JSONTools.ReadInt(data["bestSpecialCurrencyScore"]);
			}
			if(data.ContainsKey("bestDistanceScore")) {
				bestDistanceScore=JSONTools.ReadInt(data["bestDistanceScore"]);
			}
			if(data.ContainsKey("lifetimePlays")) {
				lifetimePlays=JSONTools.ReadInt(data["lifetimePlays"]);
			}
			if(data.ContainsKey("lifetimeDistance")) {
				lifetimeDistance=JSONTools.ReadInt(data["lifetimeDistance"]);
			}
			if(data.ContainsKey("lifetimeCoins")) {
				lifetimeCoins=JSONTools.ReadInt(data["lifetimeCoins"]);
			}
			if(data.ContainsKey("lifetimeSpecialCurrency")) {
				lifetimeSpecialCurrency=JSONTools.ReadInt(data["lifetimeSpecialCurrency"]);
			}
			coinCount = defaultCoinCount;
			if(data.ContainsKey("coinCount")) {
				coinCount=JSONTools.ReadInt(data["coinCount"]);
			}
			if(data.ContainsKey("specialCurrencyCount")) {
				specialCurrencyCount=JSONTools.ReadInt(data["specialCurrencyCount"]);
			}
			if(data.ContainsKey("activePlayerCharacter")) {
				activePlayerCharacter=JSONTools.ReadInt(data["activePlayerCharacter"]);
			}
			if(data.ContainsKey("numberResurectsUsed")) {
				numberResurectsUsed=JSONTools.ReadInt(data["numberResurectsUsed"]);
			}
			if(data.ContainsKey("randomSeed")) {
				randomSeed=JSONTools.ReadInt(data["randomSeed"]);
			}
			if(data.ContainsKey("gameCenterNeedsUpdate")) {
				gameCenterNeedsUpdate = (bool)data["gameCenterNeedsUpdate"];
			}
			if(data.ContainsKey("facebookLoginCoinsAwarded")) {
				facebookLoginCoinsAwarded = (bool)data["facebookLoginCoinsAwarded"];
			}
			if(data.ContainsKey("abilitiesUsed")) {
				abilitiesUsed=JSONTools.ReadInt(data["abilitiesUsed"]);
			}
			if(data.ContainsKey("savedEnvSet")) {
				savedEnvSet=JSONTools.ReadInt(data["savedEnvSet"]);
			}
			if(data.ContainsKey("majorVersion")) {
				majorVersion=JSONTools.ReadInt(data["majorVersion"]);
			}			
			if(data.ContainsKey("minorVersion")) {
				minorVersion=JSONTools.ReadInt(data["minorVersion"]);
			}
			if(data.ContainsKey("revisionVersion")) {
				revisionVersion=JSONTools.ReadInt(data["revisionVersion"]);
			}
			
			if ( data.ContainsKey( "previousScoreRank" ) )
			{
				previousScoreRank = JSONTools.ReadInt( data["previousScoreRank"] );
			}
			
			if ( data.ContainsKey( "previousDistanceRank" ) )
			{
				previousDistanceRank = JSONTools.ReadInt( data["previousDistanceRank"] );
			}
			
//			if(data.ContainsKey("artifactsPurchased")) {
//				
//				//-- If we added new artifacts since the last save, auto set them to not bought.
//				artifactsPurchased = new List<int>();
//				artifactsPurchased.Clear();
//				
//				IList tempList = data["artifactsPurchased"] as IList;
//				int index = 0;
//				foreach (var item in tempList) {
//					int id = JSONTools.ReadInt(item);
//					if(artifactsPurchased.Contains(id) == false) {
//						artifactsPurchased.Add (id);	
//					}
//					index++;
//				}
//			}
			if(data.ContainsKey("artifactLevels")) {
				
				//-- If we added new artifacts since the last save, auto set them to not bought.
				//artifactLevels = new List<int>();
				//artifactLevels.Clear();	// don't clear anymore, gets reset to all 0's at right size in Reset()
				//artifactLevels.Add (0);
				
				IList tempList = data["artifactLevels"] as IList;
				int index = 0;
				
				foreach (var item in tempList) {
					int lvl = JSONTools.ReadInt(item);
					artifactLevels[index] = lvl;	//artifactLevels.Add (lvl);	
					index++;
				}
				
				//Make sure we have enough slots in our inventory (this gets done in Reset now(), prior to getting here)
				//while(index<ArtifactStore.Artifacts.Count)
				//{
				//	artifactLevels.Add (0);
				//	index++;
				//}			
			}
			if(data.ContainsKey("artifactsGemmed")) {
				
				//-- If we added new artifacts since the last save, auto set them to not bought.
				artifactsGemmed = new List<int>();
				artifactsGemmed.Clear();
				
				IList tempList = data["artifactsGemmed"] as IList;
				int index = 0;
				foreach (var item in tempList) {
					int id = JSONTools.ReadInt(item);
					if(artifactsGemmed.Contains(id) == false) {
						artifactsGemmed.Add (id);	
					}
					index++;
				}
			}
			if(data.ContainsKey("artifactsDiscovered")) {
				
				//-- If we added new artifacts since the last save, auto set them to not bought.
				artifactsDiscovered = new List<int>();
				artifactsDiscovered.Clear();
				
				IList tempList = data["artifactsDiscovered"] as IList;
				foreach (var item in tempList) {
					int id = JSONTools.ReadInt(item);
					if(artifactsDiscovered.Contains(id) == false) {
						artifactsDiscovered.Add (id);	
					}
				}
			}
			
			if(data.ContainsKey("objectives")) {
				objectivesEarned = new List<int>();
				IList tempList = data["objectives"] as IList;
				int index = 0;
				foreach (var item in tempList) {
					int id = JSONTools.ReadInt(item);
					if(objectivesEarned.Contains(id) == false) {
						objectivesEarned.Add (id);	
					}
					index++;
				}
			}
			
			if(data.ContainsKey("legendaryObjectives")) {
				legendaryObjectivesEarned = new List<int>();
				IList tempList = data["legendaryObjectives"] as IList;
				int index = 0;
				foreach (var item in tempList) {
					int id = JSONTools.ReadInt(item);
					if(legendaryObjectivesEarned.Contains(id) == false) {
						legendaryObjectivesEarned.Add (id);	
					}
					index++;
				}
			}
			
			if(data.ContainsKey("objectivesUnclaimed")) {
				objectivesUnclaimed = new List<int>();
				IList tempList = data["objectivesUnclaimed"] as IList;
				int index = 0;
				foreach (var item in tempList) {
					int id = JSONTools.ReadInt(item);
					if(objectivesUnclaimed.Contains(id) == false) {
						objectivesUnclaimed.Add (id);	
					}
					index++;
				}
			}
			
			if(data.ContainsKey("environmentsVisited")) {
				environmentsVisited = new List<int>();
				IList tempList = data["environmentsVisited"] as IList;
				int index = 0;
				foreach (var item in tempList) {
					int id = JSONTools.ReadInt(item);
					if(environmentsVisited.Contains(id) == false) {
						environmentsVisited.Add (id);	
					}
					index++;
				}				
			}
			
			if(data.ContainsKey("LifetimeStats")) {
				LifetimeStats = new Dictionary<int, float[]>();
				LifetimeStats.Clear();
				
				IDictionary tempDict = data["LifetimeStats"] as IDictionary;
				
				if(tempDict != null) {
					foreach (var item in tempDict)
					{
						if(item != null)
						{
							DictionaryEntry kvp = (DictionaryEntry)item;
							
							int env = int.Parse((string)kvp.Key);///NOTE: for some reason, JSONTools.ParseInt() wasnt working here...
							object dict = kvp.Value;
								
							if(!LifetimeStats.ContainsKey(env))
							{
								LifetimeStats.Add(env, new float[(int)ObjectiveType.LifetimeObjectivesCount]);
							}
							IDictionary perEnvDict = dict as IDictionary;
							
							foreach(var dictItem in perEnvDict)
							{
								if(dictItem!=null)
								{
									//KeyValuePair<ObjectiveType,float> kvp2 = (KeyValuePair<ObjectiveType,float>)dictItem;
									DictionaryEntry kvp2 = (DictionaryEntry)dictItem;
									
									ObjectiveType t = (ObjectiveType)Enum.Parse(typeof(ObjectiveType),(string)kvp2.Key);
									float amt = (float)JSONTools.ReadDouble(kvp2.Value);
									if(t < ObjectiveType.LifetimeObjectivesCount)
									{
										LifetimeStats[env][(int)t] = amt;
									}
								//	Debug.Log(env.ToString() + " " + t.ToString() + " " + amt.ToString());
								}
							}
						}
					}	
				}
			}
			
//			if(data.ContainsKey("objectivesEarnedStatValues")) 
//			{
//				objectivesEarnedStatValues = new List<int>();
//				objectivesEarnedStatValues.Clear();
//				//objectivesEarnedStatValues.Add (0);
//				
//				IList tempList = data["objectivesEarnedStatValues"] as IList;
//				int index = 0;
//				
//				foreach (var item in tempList) 
//				{
//					int qty = JSONTools.ReadInt(item);
//					objectivesEarnedStatValues.Add(qty);
//					index++;
//				}
//				
//				//Make sure we have enough slots in our inventory
//				while (index < objectivesEarnedStatValues.Count)
//				{
//					objectivesEarnedStatValues.Add(0);
//					index++;
//				}
//			}			
			
			if(data.ContainsKey("objectivesActiveData")) {
				objectivesActive = new List<ObjectiveProtoData>(3);
				objectivesActive.Clear();
				
				IList tempList = data["objectivesActiveData"] as IList;
				if(tempList != null) {
					foreach (var item in tempList) {
						if(item != null) {
							Dictionary<string, object> obDict = item as Dictionary<string, object>;
							if(obDict != null) {
								ObjectiveProtoData ob = new ObjectiveProtoData(obDict);
								
								if(ob._id>0 && ObjectivesManager.FindObjectiveByID(ob._id) != null) {
									objectivesActive.Add (ob);	
								}	
								else
								{
									objectivesActive.Add(null);
								}
							}
						}
						else
						{
							objectivesActive.Add(null);
						}
					}	
				}
			}
			
			if(data.ContainsKey("legendaryProgressData")) {
				legendaryProgress = new Dictionary<int,int>();
				legendaryProgress.Clear();
				
				IDictionary tempDict = data["legendaryProgressData"] as IDictionary;
				
				if(tempDict != null) {
					foreach (var item in tempDict) {
						if(item != null) {
							DictionaryEntry kvp = (DictionaryEntry)item;
							
					//		if(kvp != null) {
								int k = int.Parse((string)kvp.Key);
								int v = (int)JSONTools.ReadDouble(kvp.Value);
							//We should be reading this in as an int; an old version wrote it as a float, this is to make sure
							//	if(v<=0)
							//		v = int.Parse((string)kvp.Value);
								
								legendaryProgress.Add(k,v);
					//		}
						}
					}	
				}
			}
			
			//Set default powerup
			if(data.ContainsKey("powersPurchased")) {
				powersPurchased = new List<int>();
				powersPurchased.Clear();
				//Default powerup
				powersPurchased.Add (1);
				
				IList tempList = data["powersPurchased"] as IList;
				int index = 0;
				foreach (var item in tempList) {
					int id = JSONTools.ReadInt(item);
					if(powersPurchased.Contains(id) == false) {
						powersPurchased.Add (id);	
					}
					index++;
				}
			}
			
			if(data.ContainsKey("powersGemmed")) {
				powersGemmed = new List<int>();
				powersGemmed.Clear();
				powersGemmed.Add (0);
				
				IList tempList = data["powersGemmed"] as IList;
				int index = 0;
				foreach (var item in tempList) {
					int id = JSONTools.ReadInt(item);
					if(powersGemmed.Contains(id) == false) {
						powersGemmed.Add (id);	
					}
					index++;
				}
			}
			
			if(data.ContainsKey("consumablesPurchasedQuantity")) 
			{
				//consumablesPurchasedQuantity = new List<int>();
				//consumablesPurchasedQuantity.Clear();	// don't clear anymore, gets reset to all 0's at right size in Reset()
				//consumablesPurchasedQuantity.Add (0);
				
				IList tempList = data["consumablesPurchasedQuantity"] as IList;
				int index = 0;
				
				foreach (var item in tempList) 
				{
					int qty = JSONTools.ReadInt(item);
					consumablesPurchasedQuantity[index] = qty;	//consumablesPurchasedQuantity.Add (qty);
					index++;
				}
				
				//Make sure we have enough slots in our inventory (this gets done in Reset() now, prior to getting here)
				//while(index<ConsumableStore.consumablesList.Count)
				//{
				//	consumablesPurchasedQuantity.Add (0);
				//	index++;
				//}
			}
			
			if(data.ContainsKey("Buffs")) {
				
				//-- If we added new artifacts since the last save, auto set them to not bought.
				buffs = new List<Buff>();
				
				IList tempList = data["Buffs"] as IList;
				foreach (var item in tempList) {
					Dictionary<string, object> buffdata = item as Dictionary<string, object>;
					buffs.Add ( new Buff(buffdata));		
				}
			}
			
			
			if(data.ContainsKey("numberChanceTokens")) {
				numberChanceTokens = JSONTools.ReadInt(data["numberChanceTokens"]);
			}
			
			/*
			if(data.ContainsKey("gotGemReward")) {
				gotGemReward=JSONTools.ReadInt(data["gotGemReward"]);
			}
			*/			
		}
	}
	
	public PlayerStats(int newPlayerId)
	{
		Reset();
		playerId = newPlayerId;
	}
	
//	public PlayerStats()
//	{
//		playerId = 0;
//		Reset();
//	}

	public void Reset()
	{
		
//			flags[(int)RecordFlagType.kRecordFlagVideoAdOfferAvailable] = 1;
//			flags[(int)RecordFlagType.kRecordFlagVideoAdReward] = 250;
//			flags[(int)RecordFlagType.kRecordFlagVideoAdPercentFlurry] = 100;
//			flags[(int)RecordFlagType.kRecordFlagFullScreenAdOfferAvailable] = 1;
//			flags[(int)RecordFlagType.kRecordFlagFullScreenAdReward] = 5000;
//			flags[(int)RecordFlagType.kRecordFlagFullScreenAdDisableCost] = 2500;
//			flags[(int)RecordFlagType.kRecordFlagFullScreenAdPercentFaad] = 50;
//			flags[(int)RecordFlagType.kRecordFlagFacebookOfferAvailable] = 1;
//			flags[(int)RecordFlagType.kRecordFlagTwitterOfferAvailable] = 1;
//			flags[(int)RecordFlagType.kRecordFlagFacebookOfferReward] = 250;
//			flags[(int)RecordFlagType.kRecordFlagFullScreenAdIncentivizedPercentage] = 0;
//			flags[(int)RecordFlagType.kRecordFlagFullScreenAdFrequency] = 100;
		bestScore = bestCoinScore = bestSpecialCurrencyScore = bestDistanceScore = 0;
		lifetimePlays = lifetimeCoins = lifetimeSpecialCurrency = lifetimeDistance = 0;
		coinCount = defaultCoinCount;
		specialCurrencyCount = 0;
		activePlayerCharacter = 0;
		gameCenterNeedsUpdate = true;
		facebookLoginCoinsAwarded = false;
		randomSeed = 1;
		numberResurectsUsed = 0;
		
		ArtifactStore.LoadFile();
		
		if (DeathMessage.Messages == null || DeathMessage.Messages.Count == 0)
			DeathMessage.LoadFile();
		
		//artifactsPurchased = new List<int>();
		//artifactsPurchased.Clear();
		
		artifactsGemmed = new List<int>();
		artifactsGemmed.Clear();
		
		artifactsDiscovered = new List<int>();
		artifactsDiscovered.Clear();
		
		//if (ObjectivesManager.Objectives == null || ObjectivesManager.Objectives.Count == 0)
		//	ObjectivesManager.LoadFile(ObjectivesManager.Objectives, "OZGameData/Objectives",this);
		//
		//if (ObjectivesManager.LegendaryObjectives == null || ObjectivesManager.LegendaryObjectives.Count == 0)
		//	ObjectivesManager.LoadFile(ObjectivesManager.LegendaryObjectives, "OZGameData/ObjectivesLegendary",this);
		
		objectivesEarned = new List<int>();
		objectivesEarned.Clear();
		
		legendaryObjectivesEarned = new List<int>();
		legendaryObjectivesEarned.Clear();
		
		legendaryProgress = new Dictionary<int, int>();
		
		if (PowerStore.Powers == null || PowerStore.Powers.Count == 0)
			PowerStore.LoadFile();
		
		powersPurchased = new List<int>();
		powersPurchased.Clear();
		powersPurchased.Add (2);
		
		powersGemmed = new List<int>();
		powersGemmed.Clear();
		powersGemmed.Add (0);
	
		if (ConsumableStore.consumablesList == null || ConsumableStore.consumablesList.Count == 0)
			ConsumableStore.LoadFile();
		
		SetUpSlotsForArtifactsAndConsumables();		// populate lists with appropriate number of entries
		
		buffs = new List<Buff>();
		savedEnvSet = EnvironmentSetManager.WhimsyWoodsId;
	}
	
	private void SetUpSlotsForArtifactsAndConsumables()
	{
		// set up slots for all consumables purchased quantities
		consumablesPurchasedQuantity = new List<int>();
		consumablesPurchasedQuantity.Clear();
		
		List<BaseConsumable> consumablesList = ConsumableStore.consumablesList.ToList();
		consumablesList = consumablesList.OrderBy(x => x.PID).ToList(); 
		
		for (int i=0; i<=consumablesList.Last().PID; i++)
			consumablesPurchasedQuantity.Add(0);
		
		// set up slots for all artifact levels
		artifactLevels = new List<int>();
		artifactLevels.Clear();
		
		List<ArtifactProtoData> artifactsList = ArtifactStore.Artifacts.ToList();
		artifactsList = artifactsList.OrderBy(x => x._id).ToList(); 	
		
		for (int i=0; i<=artifactsList.Last()._id; i++)
			artifactLevels.Add(0);
	}
	
	public bool IsArtifactDiscovered(int artifactID, bool checkRank, ArtifactProtoData protoData = null) {
		if(artifactID < 0)
			return false;
		
		if(artifactsDiscovered.Contains(artifactID))
			return true;
		
		//-- If we are skipping rank check, then consider this discovered.
		if(checkRank == false)
			return true;
		
		//-- Check rank.
		if(protoData == null) {
			protoData = ArtifactStore.GetArtifactProtoData(artifactID);	
		}
		
		if(protoData != null) {
			if(protoData._requiredRank <= GameProfile.SharedInstance.Player.GetCurrentRank())
				return true;
		}
		return false;
	}
	
	public bool IsArtifactPurchased(int artifactID) {
		if(artifactID < 0)
			return false;
		
		return artifactsPurchased.Contains(artifactID);
	}
	
	public bool IsArtifactGemmed(int artifactID) {
		if(artifactID < 0)
			return false;
		
		return artifactsGemmed.Contains(artifactID);
	}
	
	
	
	public bool CanAffordArtifact(int artifactID, ArtifactProtoData protoData=null)
	{
		//-- Its already bought, so the price is effectively infinte.
		//if(IsArtifactPurchased(artifactID) == true)
		//	return false;
		
		if (protoData == null)
			protoData = ArtifactStore.GetArtifactProtoData(artifactID);
		
		if (protoData == null)
			return false;
		
		// GetCostForLevel will now call the ArtifactProtoData's GetFinalCost( int level )

		int cost = GetCostForLevel(protoData);
		
		//-- Cheaters.
		if (cost < 0)	//protoData._cost < 0)
			return false;
		
		if (protoData._costType == CostType.Coin) 
		{
			if (cost > coinCount)	//protoData._cost > coinCount)
				return false;	
		}
		else 
		{
			if (cost > specialCurrencyCount)	//protoData._cost > specialCurrencyCount)
				return false;	
		}
		
		return true;
	}
	
	public bool CanAffordArtifactGem(int artifactID, ArtifactProtoData protoData=null) {
		//-- Its already bought, so the price is effectively infinte.
		if(IsArtifactGemmed(artifactID) == true)
			return false;
		
		if(protoData == null)
			protoData = ArtifactStore.GetArtifactProtoData(artifactID);
		
		if(protoData == null)
			return false;
		
		if(!IsArtifactPurchased(artifactID))
			return false;
		
		//-- Just one gem.
		if(1 > specialCurrencyCount)
		{
			return false;	
		}
		
		
		return true;
	}
	
	
//	public bool CanAffordArtifactUpgrade(int artifactID, int currentLvl, ArtifactProtoData protoData=null) {
//		//-- Its already bought, so the price is effectively infinte.
//		if(IsArtifactPurchased(artifactID) == true)
//			return false;
//		
//		if(protoData == null)
//			protoData = ArtifactStore.GetArtifactProtoData(artifactID);
//		
//		if(protoData == null)
//			return false;
//		
//		//-- Cheaters.
//		if(protoData._cost < 0)
//			return false;
//		
//		if(protoData._costType == CostType.Coin) {
//			if(GetArtifactUpgradeCost(artifactID,currentLvl+1) > coinCount)
//				return false;	
//		}
//		else {
//			if(GetArtifactUpgradeCost(artifactID,currentLvl+1) > specialCurrencyCount)
//				return false;	
//		}
//		
//		
//		return true;
//	}
	
	// wxj
	//params:isOnekey, whether upgurate artifact by onekey
	//params:isRMB, purchase mode whether RMB
	public void PurchaseArtifact(int artifactID, bool isAll, ArtifactProtoData protoData = null) 
	{
		if (artifactID < 0) 
			return;
		
		if (protoData == null)
			protoData = ArtifactStore.GetArtifactProtoData(artifactID);
		
		if (protoData == null) 
			return;
		
		//-- Check canafford
		//if (CanAffordArtifact(artifactID, protoData) == false)
		// wxj
		if (CanAffordArtifact(artifactID, protoData) == false && !isAll)
			return;
		
		//Log a modifier purchase
		switch(protoData._statType)	
		{
		case StatType.DoubleCoinsDistance:
			ObjectivesDataUpdater.SetGenericStat(ObjectiveType.ModifierLevelDoubleCoins,artifactLevels[artifactID]);
			break;
		case StatType.GeneralPowerDuration:
			ObjectivesDataUpdater.SetGenericStat(ObjectiveType.ModifierLevelMagician,artifactLevels[artifactID]);
			break;
		case StatType.CoinMeterFillRate:
			ObjectivesDataUpdater.SetGenericStat(ObjectiveType.ModifierLevelEnchanter,artifactLevels[artifactID]);
			break;
		case StatType.HeadStartDiscount:
			ObjectivesDataUpdater.SetGenericStat(ObjectiveType.ModifierLevelBargainHunter,artifactLevels[artifactID]);
			break;
		case StatType.LuckIncrease:
			ObjectivesDataUpdater.SetGenericStat(ObjectiveType.ModifierLevelLuck,artifactLevels[artifactID]);
			break;
		}
	
		//-- Using ABS here to prevent cheaters setting negative values in data.
		int analyticsCost = 0;
		
		// GetCostForLevel now calls the ArtifactProtoData GetFinalCost( int level )
		// wxj, if purchase mode is coins
		int cost = isAll? 0 : GetCostForLevel(protoData);
		if (protoData._costType == CostType.Coin)
		{
			analyticsCost = Math.Abs( cost );
			coinCount -= Math.Abs(cost);	//protoData._cost);
			//			PurchaseUtil.bIAnalysisWithParam("Player_Coins","Consume_Coins_Amount|"+cost);
		}
		else 
		{
			analyticsCost = Math.Abs( cost );
			specialCurrencyCount -= Math.Abs(cost);	//protoData._cost); // NO IF here. any bad data coming through here will cost speical currency.
		}
		
		
		//-- Ok mark it bought.
		//if (artifactsPurchased.Contains(artifactID) == false) 
		//	artifactsPurchased.Add(artifactID);
		
		//int index = artifactsPurchased.IndexOf(artifactID);
		// wxj
		if(isAll)
		{
			artifactLevels[artifactID] = 5;
		}
		else
		{
			artifactLevels[artifactID]++;	// add 1 to level.  If the quantity is > 0, it means that it's been 'purchased' according to Imangi's legacy terminology.
		}
		
		
		//Test to see if we have completed ALL artifact upgrades
		//NOTE! THIS IS ASSUMING 5 LEVELS
		int upgradedCount = 0;
		for (int i=0;i<artifactLevels.Count;i++)
		{
			if (artifactLevels[i]>=5)
				upgradedCount++;
		}
	//	Debug.Log("Fully upgraded artifacts: " + upgradedCount);
		ObjectivesDataUpdater.SetGenericStat(ObjectiveType.ModifiersMaxed,upgradedCount);
		
		int obtainedCount = 0;
		for (int i=0;i<artifactLevels.Count;i++)
		{
			if (artifactLevels[i]>=1)
				obtainedCount++;
		}
	//	Debug.Log("Obtained artifacts: " + obtainedCount);
		ObjectivesDataUpdater.SetGenericStat(ObjectiveType.UnlockArtifacts,obtainedCount);
		
		
	//	AddObjectiveStat(ObjectiveType.UnlockArtifacts, ObjectiveTimeType.OverTime, ObjectiveFilterType.None, 1);
		
		GameProfile.SharedInstance.Serialize();
		/*
		Dictionary<string,string> dict = new Dictionary<string,string>();
		dict.Add("Title", protoData._title );
		FlurryBinding.logEventWithParameters("purchaseArtifact", dict, false );
		*/

		//Debug.LogWarning("prevCoinCount = " + prevCoinCount.ToString() + ", coinCount = " + coinCount);
		
		//string artifactTitle = Localization.SharedInstance.Get( protoData._title );
		//		AnalyticsInterface.LogInAppCurrencyActionEvent( CostType.Coin, 0 - analyticsCost, "ability", protoData._title, artifactLevels[artifactID], "store", "" );
	}
	
	private int GetCostForLevel(ArtifactProtoData data)
	{	
		int level = GameProfile.SharedInstance.Player.GetArtifactLevel(data._id);
		/*
		switch (level)
		{
		case 0:
			return data._cost_lv1;
		case 1:
			return data._cost_lv2;
		case 2:
			return data._cost_lv3;
		case 3:
			return data._cost_lv4;
		case 4:
			return data._cost_lv5;
		default:
			return data._cost_lv5;
		}
		*/
		// Add one to the artifact's current rank to get the next rank's cost.
		return data.GetFinalCost( level + 1 );
	}	
	
	public int GetArtifactUpgradeCost(int artifactID, int level)
	{
		if(artifactID < 0)
			return 0;
		
		ArtifactProtoData protoData = ArtifactStore.GetArtifactProtoData(artifactID);	
		
		if(protoData == null)
			return 0;
		
		return Mathf.Abs(protoData.GetCost(level));
	}
	
	public string GetArtifactLocalizeKey(int artifactID)
	{
		if(artifactID < 0)
			return "";
		
		ArtifactProtoData protoData = ArtifactStore.GetArtifactProtoData(artifactID);	
		
		if(protoData == null)
			return "";
		
		return protoData._title;
	}
	
//	public int GetCurrentArtifactLevel(int artifactID)
//	{
//		if(IsArtifactGemmed(artifactID) == true)
//			return 0;
//		
//		ArtifactProtoData protoData = ArtifactStore.GetArtifactProtoData(artifactID);
//		
//		if(protoData == null)
//			return 0;
//		
//		if(!IsArtifactPurchased(artifactID))
//			return 0;
//		
//		int index = artifactsPurchased.IndexOf(artifactID);
//		
//		return artifactLevels[index];
//	}
	
	/*public void UpgradeArtifact(int artifactID, ArtifactProtoData protoData = null)
	{if(artifactID < 0)
			return;
		
		if(protoData == null)
			protoData = ArtifactStore.GetArtifactProtoData(artifactID);
		
		if(protoData == null)
			return;
		
		//-- Check canafford
		if(CanAffordArtifact(artifactID, protoData) == false)
			return;
		
		if(artifactsPurchased.Contains(artifactID) == false)
			artifactsPurchased.Add (artifactID);
			
		int index = artifactsPurchased.IndexOf(artifactID);
		
		artifactLevels[index]++;	//artifactLevels.Add(1);
		
		//-- Using ABS here to prevent cheaters setting negative values in data.
		if(protoData._costType == CostType.Coin) {
			coinCount -= Math.Abs(GetArtifactUpgradeCost(artifactID,artifactLevels[index]));	
		}
		else { // NO IF here. any bad data coming through here will cost speical currency.
			specialCurrencyCount -= Math.Abs(protoData._cost);	
		}
		
		//AddObjectiveStat(ObjectiveType.UnlockArtifacts, ObjectiveTimeType.OverTime, ObjectiveFilterType.None, 1);
		
		GameProfile.SharedInstance.Serialize();
		
	}*/
	
	public void GemArtifact(int artifactID, ArtifactProtoData protoData = null) {
		if(artifactID < 0)
			return;
		
		if(protoData == null)
			protoData = ArtifactStore.GetArtifactProtoData(artifactID);
		
		if(protoData == null)
			return;
		
		//-- Check canafford
		if(CanAffordArtifactGem(artifactID, protoData) == false)
			return;
		
		//-- Ok mark it bought.
		if(artifactsGemmed.Contains(artifactID) == false) {
			artifactsGemmed.Add (artifactID);
			//			AnalyticsInterface.LogGameAction("ability_gem","applied",protoData._statType.ToString(),GameProfile.GetAreaCharacterString(),0);
		}
		
		ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.EmbueModifier,1);
		
		//-- Remove a single gem
		specialCurrencyCount -= 1;	
		
		//AddObjectiveStat(ObjectiveType.UnlockArtifacts, ObjectiveTimeType.OverTime, ObjectiveFilterType.None, 1);
		
		//NOTE: This doesnt happen in the menu anymore, so we shouldn't save it here
		//GameProfile.SharedInstance.Serialize();
	}
	
	public void UnGemAllArtifacts()
	{
		artifactsGemmed.Clear();
		
		GameProfile.SharedInstance.Serialize();
	}
	
	public void UnGemArtifact(int artifactID, ArtifactProtoData protoData = null) {
		if(artifactID < 0)
			return;
		
		if(protoData == null)
			protoData = ArtifactStore.GetArtifactProtoData(artifactID);
		
		if(protoData == null)
			return;
		
		//-- Check canafford
		if(CanAffordArtifactGem(artifactID, protoData) == false)
			return;
		
		//-- Ok mark it bought.
		if(artifactsGemmed.Contains(artifactID) == true) {
			artifactsGemmed.Remove (artifactID);
		}
		
		//AddObjectiveStat(ObjectiveType.UnlockArtifacts, ObjectiveTimeType.OverTime, ObjectiveFilterType.None, 1);
		
		GameProfile.SharedInstance.Serialize();
	}
	
	public bool IsHeroPurchased(int characterID) {
		if(characterID < 0 || characterID > GameProfile.SharedInstance.Characters.Count)
			return false;
		
		CharacterStats character = GameProfile.SharedInstance.Characters[characterID];
		if(character == null)
			return false;
		
		return character.unlocked;
	}
	
	public bool CanAffordHero(int characterID) {
		if(characterID < 0 || characterID > GameProfile.SharedInstance.Characters.Count)
			return false;
		
		CharacterStats character = GameProfile.SharedInstance.Characters[characterID];
		if(character == null)
			return false;
		
		//-- Its already bought, so the price is effectively infinte.
		if(IsHeroPurchased(characterID) == true)
			return false;
		
		//-- Cheaters.
		if ( character.GetFinalCost() < 0 )
		{
			return false;
		}
		
		if(character.unlockCostType == CostType.Coin) {
			if ( character.GetFinalCost() > coinCount )
			{
				return false;	
			}
		}
		else {
			if ( character.GetFinalCost() > specialCurrencyCount )
			{
				return false;	
			}
		}
		return true;
	}
	
	public void PurchaseHero(int characterID) 
	{
		if(characterID < 0 || characterID > GameProfile.SharedInstance.Characters.Count)
			return;
		if(CanAffordHero(characterID) == false)
			return;
		CharacterStats character = GameProfile.SharedInstance.Characters[characterID];
		
		character.unlocked = true;
		if(character.unlockCostType == CostType.Coin) {
			//coinCount -= Math.Abs(character.unlockCost);
			coinCount -= Math.Abs( character.GetFinalCost() );
		}
		else { // NO IF here. any bad data coming through here will cost speical currency.
			//specialCurrencyCount -= Math.Abs(character.unlockCost);	
			specialCurrencyCount -= Math.Abs( character.GetFinalCost() );
		}
		
	//	AddObjectiveStat(ObjectiveType.UnlockCharacters, ObjectiveTimeType.OverTime, ObjectiveFilterType.None, 1);
		
		GameProfile.SharedInstance.Serialize();

		//		AnalyticsInterface.LogInAppCurrencyActionEvent( CostType.Coin, 0 - Math.Abs( character.GetFinalCost() ), "character", character.displayName, 0, "store", "" );
	}
	
	public void PurchasePower(int powerID)
	{
		notify.Debug ("PurchasePower {0}", powerID);
		if(powerID < 0)
			return;
		
		//-- Check canafford
		if(CanAffordPower(powerID) == false)
			return;
		
		//-- Ok mark it bought.
		if(powersPurchased.Contains(powerID) == false) {
			powersPurchased.Add (powerID);
		}
		
		BasePower power = PowerStore.PowerFromID(powerID);
		//-- Using ABS here to prevent cheaters setting negative values in data.
		if(power.CostType == CostType.Coin) {
			//coinCount -= Math.Abs(power.Cost);
			coinCount -= Math.Abs( power.GetFinalCost() );
		}
		else { // NO IF here. any bad data coming through here will cost speical currency.
			//specialCurrencyCount -= Math.Abs(power.Cost);	
			specialCurrencyCount -= Math.Abs( power.GetFinalCost() );
		}
		//		PurchaseUtil.bIAnalysisWithParam("Player_Coins","Consume_Coins_Amount|"+power.GetFinalCost());
	//	AddObjectiveStat(ObjectiveType.UnlockPowerups, ObjectiveTimeType.OverTime, ObjectiveFilterType.None, 1);
		
		GameProfile.SharedInstance.Serialize();

		//		AnalyticsInterface.LogInAppCurrencyActionEvent( CostType.Coin, 0 - Math.Abs( power.GetFinalCost() ), "powerup", power.Title, 0, "store", "" );
	}
		
	public void PurchaseConsumable(int consumableID) 
	{
		notify.Debug ("PurchaseConsumable {0}", consumableID);
		if(consumableID < 0) { return; }
		
		if (CanAffordConsumable(consumableID) == false) { return; }	//-- Check canafford
		//if (consumablesPurchasedQuantity.Contains(consumableID) == false) { consumablesPurchasedQuantity.Add (consumableID); }
		consumablesPurchasedQuantity[consumableID]++;	// add 1 to purchased quantity
		
		BaseConsumable consumable = ConsumableStore.ConsumableFromID(consumableID);
		//if (consumable.CostType == CostType.Coin) { coinCount -= Math.Abs(consumable.ActualCost); }	//-- Using ABS here to prevent cheaters setting negative values in data.
		//else { specialCurrencyCount -= Math.Abs(consumable.ActualCost); }	// NO IF here. any bad data coming through here will cost speical currency.
		
		if ( consumable.CostType == CostType.Coin )
		{
			coinCount -= Math.Abs( consumable.GetFinalCost() );
			//			PurchaseUtil.bIAnalysisWithParam("Player_Coins","Consume_Coins_Amount|"+consumable.GetFinalCost() );
		}
		else
		{
			specialCurrencyCount -= Math.Abs( consumable.GetFinalCost() );
		}
		
	//	AddObjectiveStat(ObjectiveType.UnlockConsumables, ObjectiveTimeType.OverTime, ObjectiveFilterType.None, 1);
		GameProfile.SharedInstance.Serialize();
		
		//		AnalyticsInterface.LogInAppCurrencyActionEvent( CostType.Coin, 0 - Math.Abs( consumable.GetFinalCost() ), "utility", consumable.Title, 0, "store", "" );
	}	
	
	public void EarnConsumable(int consumableID, int quantity) 
	{
		notify.Debug ("PurchaseConsumable {0}", consumableID);
	//	if (consumableID < 0) { return; }
		UnityEngine.Debug.LogError("EarnConsumable call");
		consumablesPurchasedQuantity[consumableID] += quantity;		// add to purchased quantity
		
	//	AddObjectiveStat(ObjectiveType.UnlockConsumables, ObjectiveTimeType.OverTime, ObjectiveFilterType.None, 1);
		GameProfile.SharedInstance.Serialize();
	}		
	
	public void PurchaseStoreItem(int storeItemID) 
	{
		Debug.Log ("PurchaseStoreItem"); // jonoble
		
		notify.Debug ("PurchaseStoreItem {0}", storeItemID);
		if(storeItemID < 0) { return; }
		
		StoreItem storeItem = Store.StoreItems[storeItemID];
		if (storeItem.costType == CostType.RealMoney) 
		{ 
			if (storeItem.itemType == StoreItemType.CoinBundle) 		
			{
				coinCount += storeItem.itemQuantity;
			}
			else if (storeItem.itemType == StoreItemType.GemBundle) 		
			{
				specialCurrencyCount += storeItem.itemQuantity;
			}			
		}
		
		GameProfile.SharedInstance.Serialize();
	}		
	
	public void GemPower(int powerID) {
		notify.Debug("GemPower {0}", powerID);
		if(powerID < 0)
			return;
		
		//-- Check canafford
		if(CanAffordPowerGem(powerID) == false)
			return;
		
		//-- Ok mark it bought.
		if(powersGemmed.Contains(powerID) == false) {
			powersGemmed.Add (powerID);
			notify.Debug("PlayerStats - Power Gem");
			//			AnalyticsInterface.LogGameAction("ability_gem","applied",((PowerType)powerID).ToString(),GameProfile.GetAreaCharacterString(),0);
		}
		
	//	BasePower power = PowerStore.PowerFromID(powerID);
		
		//Remove a single gem
		specialCurrencyCount -= 1;	
		
		
	//	AddObjectiveStat(ObjectiveType.UnlockPowerups, ObjectiveTimeType.OverTime, ObjectiveFilterType.None, 1);
		
		GameProfile.SharedInstance.Serialize();
	}
	
	public bool IsPowerPurchased(int powerID) {
		if(powerID < 0)
			return false;
		
		return powersPurchased.Contains(powerID);
	}
	
	public bool IsPowerGemmed(int powerID) {
		if(powerID < 0)
			return false;
		
		return powersGemmed.Contains(powerID);
	}
	
	public bool CanAffordPower(int powerID) {
		BasePower power = PowerStore.PowerFromID(powerID);
		
		//-- Its already bought, so the price is effectively infinte.
		if(IsPowerPurchased(powerID) == true)
			return false;
		
		//-- Cheaters.
		//if(power.Cost < 0)
		if ( power.GetFinalCost() < 0 )
		{
			return false;
		}
		if(power.CostType == CostType.Coin) {
			if ( power.GetFinalCost() > coinCount )
			{
				return false;	
			}
		}
		else {
			if ( power.GetFinalCost() > specialCurrencyCount )
			{
				return false;	
			}
		}
		
		
		return true;
	}
	
	public bool CanAffordPowerGem(int powerID)
	{
		//BasePower power = PowerStore.PowerFromID(powerID);
		
		//-- Its already bought, so the price is effectively infinte.
		if(IsPowerGemmed(powerID) == true || !IsPowerPurchased(powerID))
			return false;
		
		if(1 > specialCurrencyCount)
			return false;	
		
		return true;
	}
	

	
	public bool IsArtifactMaxedOut(int artifactID)
	{		
		if (artifactID < 0) { return false; }
			
		if (artifactLevels[artifactID] >= ArtifactStore.maxOfEachArtifact) { return true; }
		else { return false; }
	}	
	
	public bool IsConsumableMaxedOut(int consumableID) 
	{		
		if (consumableID < 0) { return false; }
			
		if (consumablesPurchasedQuantity[consumableID] >= ConsumableStore.maxOfEachConsumable) { return true; }
		else { return false; }
	}
	
	public bool CanAffordConsumable(int consumableID) 
	{
		BaseConsumable consumable = ConsumableStore.ConsumableFromID(consumableID);
		
		//-- Its already bought, so the price is effectively infinte.
		//if(IsConsumablePurchased(consumableID) == true) { return false; }
		
		//-- Cheaters.
		if (consumable.ActualCost < 0) { return false; }
		
		if (consumable.CostType == CostType.Coin) 
		{
			//if (consumable.ActualCost > coinCount) { return false; }
			if ( consumable.GetFinalCost() > coinCount )
			{
				return false;
			}
		}
		else 
		{
			//if (consumable.ActualCost > specialCurrencyCount) { return false; }
			if ( consumable.GetFinalCost() > specialCurrencyCount )
			{
				return false;
			}
		}
		return true;
	}
	
	public string GetConsumableLocalizeString(int consumableID) 
	{
		BaseConsumable consumable = ConsumableStore.ConsumableFromID(consumableID);
		
		//-- Its already bought, so the price is effectively infinte.
		//if(IsConsumablePurchased(consumableID) == true) { return false; }
		
		if(consumable==null)	return "";
		
		string locTitle = Localization.SharedInstance.Get(consumable.Title);
		
		return locTitle;
	}
	
	public bool PopConsumable(int id)
	{
		if(id>=0 && id<consumablesPurchasedQuantity.Count)
		{
			if(consumablesPurchasedQuantity[id] > 0)
			{
				consumablesPurchasedQuantity[id]--;
				return true;
			}
		}
		return false;
	}
	
	public int GetConsumableCount(int id)
	{
		if(id>=0 && id<consumablesPurchasedQuantity.Count)
		{
			return consumablesPurchasedQuantity[id];
		}
		return 0;
	}
	
	public void UseUpOneOfEachConsumable()
	{
		for (int i=0; i< consumablesPurchasedQuantity.Count; i++)
		{
			if (consumablesPurchasedQuantity[i] >= 1) { consumablesPurchasedQuantity[i]--; }
		}
	}	
	
	public bool CanAffordResurrect() {
		if(GameProfile.SharedInstance == null)
			return false;
		
		if(specialCurrencyCount + GamePlayer.SharedInstance.GemCountTotal >= GameProfile.SharedInstance.GetResurrectionCost())
			return true;
		
		return false;
	}
	
	public int GetGemCount()
	{
		return specialCurrencyCount;
	}
	
	public bool CreateBuff(BuffType bufftype, int itemID, Buff protoBuff) {
		//-- Assumes already paid for.
		Buff activeBuff = findActiveBuff(bufftype, itemID);
		if(activeBuff == null) {
			activeBuff = new Buff(protoBuff.ToDict());
			activeBuff.itemID = itemID;
			activeBuff.buffType = bufftype;
			activeBuff.usesLeft=1;
			buffs.Add(activeBuff);
			return true;
		}
		else {
			//-- add a use.
			activeBuff.usesLeft++;
			if(activeBuff.usesLeft > protoBuff.usesLeft) {
				activeBuff.usesLeft = protoBuff.usesLeft;
				//-- Return false here because we can't charge, the power is already full.
				return false;
			}
		}
		return true;
	}
	
	public void ConsumeBuffUse(BuffType bufftype, int itemID) {
		Buff activeBuff = findActiveBuff(bufftype, itemID);
		if(activeBuff == null)
			return;
		
		activeBuff.usesLeft--;
		if(activeBuff.usesLeft == 0) {
			buffs.Remove(activeBuff);
		}
	}
	
	public Buff findActiveBuff(BuffType bufftype, int itemID) {
		foreach(Buff b in buffs) {
			if(b == null)
				continue;
			if(b.buffType == bufftype && b.itemID == itemID)
				return b;
		}
		return null;
	}
	
	public int GetBuffCost(BuffType bufftype, int itemID, Buff protoBuff) {
		
		//-- Find a live buff
		Buff activeBuff = findActiveBuff(bufftype, itemID);
		if(activeBuff == null) {
			//-- No buff given? use the base cost.
			if(protoBuff == null)
				return Buff.BaseCost;
			
			return protoBuff.cost;
		}
		if(protoBuff == null)
			return (int)Buff.BaseCost;
		
		return (int)protoBuff.cost;
	}
	
	public float GetBuffProgress(BuffType bufftype, int itemID, Buff protoBuff){
		Buff activeBuff = findActiveBuff(bufftype, itemID);
		if(activeBuff == null)
			return 0.0f;
		
		return (float)activeBuff.usesLeft / (float)protoBuff.usesLeft;
	}
	
	
	public int GetNumberChanceTokens()
	{
		return numberChanceTokens;
	}
	
	public int GetNumberChanceTokensThisRun()
	{
		return numberChanceTokensThisRun;
	}
	
	public void AddChanceToken()
	{
		numberChanceTokens++;
		numberChanceTokensThisRun++;
	}
	
	public bool PopChanceToken()
	{
		if(numberChanceTokens>0) {
			numberChanceTokens--;
			return true;
		}
		return false;
	}
	
	
	
	//-------------------------------------------------------------------------------- 
	//-- Objectives
	//-------------------------------------------------------------------------------- 
	public int GetMaxRank() 
	{
		if (ObjectivesManager.Objectives == null) { return 1; }
		return LevelFromCount(ObjectivesManager.Objectives.Count);
	}
	
	public int GetCurrentRank() 
	{
		return LevelFromCount(objectivesEarned.Count);
	}
	
	private int objNeeded = 3;	// # objectives required to complete a level
	
	private int LevelFromCount(int earned) 
	{
		int level = 1;
		int needed = objNeeded;	//3;	//2;
		while(needed <= earned) 
		{
			level+=1;
			needed+=objNeeded;	//3;	//(level+1);
		}
		return level;	
	}	
	
	public float GetCurrentRankProgress() 
	{
		int earned = objectivesEarned.Count;
		
		int level = 1;
		int needed = objNeeded;	//3;	//2;
		while(needed <= earned) 
		{
			level+=1;
			needed+=objNeeded;	//3;	//(level+1);
		}
		//return 1.0f - ((float)(needed - earned) / (float)(level+1));
		//return (3.0f - (float)(needed - earned)) / 3.0f;
		return ((float)objNeeded - (float)(needed - earned)) / (float)objNeeded;
	}		

	public int GetArtifactLevel(int id)
	{
		return artifactLevels[id];
	}
	
//	public int GetCurrentRank() 
//	{
//		if(objectivesEarned == null || objectivesEarned.Count < 2)
//			return 1;
//		
//		return LevelFromCount(objectivesEarned.Count);
//	}
//
//	private int LevelFromCount(int count) 
//	{
//		int earned = count;
//		int level = 1;
//		int needed = 2;
//		while(needed <= earned) 
//		{
//			level+=1;
//			needed+=(level+1);
//		}
//		return level;	
//	}
//	
//	public float GetCurrentRankProgress() 
//	{
//		int earned = objectivesEarned.Count;
//		int level = 1;
//		int needed = 2;
//		while(needed <= earned) 
//		{
//			level+=1;
//			needed+=(level+1);
//		}
//		return 1.0f - ((float)(needed - earned) / (float)(level+1));
//	}	
	
	
	
	public void RecordLegendaryProgress()
	{
		foreach(ObjectiveProtoData data in ObjectivesManager.LegendaryObjectives)
		{
			if(!legendaryProgress.ContainsKey(data._id))
				legendaryProgress.Add(data._id,0);
			legendaryProgress[data._id] = (int)data._conditionList[0]._earnedStatValue;
		}
	}
	
	
	//An alternative to the below function
	public void UpdateAllObjectiveStats()
	{
		for (int i = 0; i < objectivesActive.Count; i++)
		{
			UpdateObjectiveStat(i);
		}
	}
	
	public void UpdateObjectiveStat(int ind)
	{
		if(ind<0 || ind>=objectivesActive.Count)	return;
		
		UpdateObjectiveStat(objectivesActive[ind]);
	}
	
	public int GetObjectiveProgressDuringRun(int objectiveIndex)
	{
		return GetObjectiveProgressDuringRun(GetActiveObjectiveFromIndex(objectiveIndex));
	}
	
	public int GetObjectiveProgressDuringRun(ObjectiveProtoData ob, bool isWeekly = false)
	{
		if (ob == null) return 0;

		int earnedProgress = 0;
		
		ConditionProtoData cd = ob._conditionList[0];
		
		if (!isWeekly && cd._timeType == ObjectiveTimeType.LifeTime)
		{
			int lifetimeStat = ObjectivesDataUpdater.GetStatForLifetimeObjectiveType(cd._type, ob._environmentID);
			earnedProgress = Mathf.Clamp(lifetimeStat, cd._earnedStatValue, cd._statValue);
		}
		else
		{
			int earnedStat = ObjectivesDataUpdater.GetStatForObjectiveType(cd._type,ob._environmentID);
		
			if (cd._timeType == ObjectiveTimeType.PerRun)
				earnedProgress = Mathf.Clamp(earnedStat, 0, cd._statValue);
			else if (cd._timeType == ObjectiveTimeType.OverTime)
				earnedProgress = Mathf.Clamp(cd._earnedStatValue + earnedStat, cd._earnedStatValue, cd._statValue);
			else if (isWeekly && cd._timeType == ObjectiveTimeType.LifeTime)
				earnedProgress = Mathf.Clamp(earnedStat, 0, cd._statValue);
		}
		
		return earnedProgress;
	}	
	
	Dictionary<int,int> recordedOverTimeStats = new Dictionary<int, int>();
	public void RecordOverTimeStats()
	{
		ObjectiveProtoData data = null;
		
		for(int i=0;i<ObjectivesManager.LegendaryObjectives.Count;i++)
		{
			data = ObjectivesManager.LegendaryObjectives[i];
			if(data==null)	continue;
			if(data._conditionList[0]._timeType==ObjectiveTimeType.OverTime)
			{
				if(!recordedOverTimeStats.ContainsKey(data._id))
					recordedOverTimeStats.Add(data._id,0);
				recordedOverTimeStats[data._id] = data._conditionList[0]._earnedStatValue;
			}
		}
		
		List<ObjectiveProtoData> weekly = Services.Get<ObjectivesManager>().GetWeeklyObjectives();
		if(weekly==null)	return;
		
		for(int i=0;i<weekly.Count;i++)
		{
			data = weekly[i];
			if(data==null)	continue;
			if(data._conditionList[0]._timeType==ObjectiveTimeType.OverTime)
			{
				if(!recordedOverTimeStats.ContainsKey(data._id))
					recordedOverTimeStats.Add(data._id,0);
				recordedOverTimeStats[data._id] = data._conditionList[0]._earnedStatValue;
			}
		}
		
		// Need Team stats
		List<ObjectiveProtoData> teamObjList = Services.Get<ObjectivesManager>().GetWeeklyObjectivesClass().TeamObjectiveList;
		if ( teamObjList == null )
		{
			return;
		}
		
		for ( int i = 0; i < teamObjList.Count; i++ )
		{
			data = teamObjList[i];
			if ( data == null )
			{
				continue;
			}
			if ( data._conditionList[0]._timeType == ObjectiveTimeType.OverTime )
			{
				if ( !recordedOverTimeStats.ContainsKey( data._id ) )
				{
					recordedOverTimeStats.Add( data._id, 0 );	
				}
				recordedOverTimeStats[data._id] = data._conditionList[0]._earnedStatValue;
			}
		}
		
		for(int i=0;i<objectivesActive.Count;i++)
		{
			data = objectivesActive[i];
			if(data==null)	continue;
			if(data._conditionList[0]._timeType==ObjectiveTimeType.OverTime)
			{
				if(!recordedOverTimeStats.ContainsKey(data._id))
					recordedOverTimeStats.Add(data._id,0);
				recordedOverTimeStats[data._id] = data._conditionList[0]._earnedStatValue;
			}
		}
	}
	
	public int GetRecordedOverTimeStat(int id)
	{
		if(!recordedOverTimeStats.ContainsKey(id))
			recordedOverTimeStats.Add(id,0);
		return recordedOverTimeStats[id];
	}
	

	// wxj, add param ObjectiveTpye, cause activity objective need other object type's data
	public void UpdateObjectiveStat(ObjectiveProtoData ob, bool isWeekly = false, ObjectiveType type = ObjectiveType.Distance)
	{
		if(ob==null)	return;
		
		ConditionProtoData cd = ob._conditionList[0];
		
		
		// wxj, activity objectives
		switch(cd._type)
		{
		case ObjectiveType.Activity1:
			if(type != ObjectiveType.Distance) return;
			cd._earnedStatValue = 0;
			// if more than 2500 distance in per run
			foreach(float value in cd._actiEarnedStatForEnvs.Values)
			{
				if((int)value >= 1500)
				{
					cd._earnedStatValue++;	
				}
			}
			//UnityEngine.Debug.Log("wxj: Activity1 _earnedStatValue:"+cd._earnedStatValue);
			
			return;
			
		case ObjectiveType.Activity2:
			if(type != ObjectiveType.JumpOverPassed) return;
			cd._earnedStatValue = 0;
			// wxj, if jump over obstacle more than 50 in per run
			foreach(int value in cd._actiEarnedStatForEnvs.Values)
			{
				if(value >= 15)
				{
					cd._earnedStatValue++;
				}
			}
			UnityEngine.Debug.Log("wxj: Activity2 _earnedStatValue:"+cd._earnedStatValue);
			return;
		}
		
		
		
		if(!isWeekly && cd._timeType==ObjectiveTimeType.LifeTime)
		{
			int lifetimeStat = ObjectivesDataUpdater.GetStatForLifetimeObjectiveType(cd._type,ob._environmentID);
			
		//	if(lifetimeStat>=0)
		//		earnedStat = lifetimeStat;
		//	else
		//		notify.Debug("WARNING! Objective Type '"+cd._type+"' is not tracked as a lifetime stat. Using the current stat instead.");
			
			cd._earnedStatValue = Mathf.Clamp(lifetimeStat,cd._earnedStatValue,cd._statValue);
		}
		else
		{
			int earnedStat = ObjectivesDataUpdater.GetStatForObjectiveType(cd._type,ob._environmentID);
		
			if(cd._timeType==ObjectiveTimeType.PerRun)
			{
				cd._earnedStatValue = Mathf.Clamp(earnedStat,cd._earnedStatValue,cd._statValue);
			}
			else if(cd._timeType==ObjectiveTimeType.OverTime)
			{
				cd._earnedStatValue = Mathf.Clamp(GetRecordedOverTimeStat(ob._id) + earnedStat,cd._earnedStatValue,cd._statValue);
			}
			else if (isWeekly && cd._timeType == ObjectiveTimeType.LifeTime)
			{
				cd._earnedStatValue = Mathf.Clamp(earnedStat, cd._earnedStatValue, cd._statValue);
			}
		}

	//	if(cd._earnedStatValue >= cd._statValue)
	//	{
	//		//Achieved! Could do something neat here... or elsewhere.
	//	}
	}
	
	/*
	// wxj, update activity objectives stat
	private void updateActivityObjectiveStat()
	{
		foreach(ObjectiveProtoData data in ObjectivesManager.ActivityObjectives)
		{
			UpdateObjectiveStat(data);
		}
	}
	*/
	
	public ObjectiveProtoData UpdateLegendaryObjectiveStat(ObjectiveProtoData ob)
	{
		UpdateObjectiveStat(ob);
		
		//This is slow, putting at the end of a run
	///	if(!legendaryProgress.ContainsKey(ob._id))
	//		legendaryProgress.Add(ob._id,0f);
	//	legendaryProgress[ob._id] = ob._conditionList[0]._earnedStatValue;
		
		//if(data._conditionList[0]._type == ObjectiveType.CollectCoins)
		//	Debug.Log(data._title + " " +data._conditionList[0]._type + " " +data._conditionList[0]._earnedStatValue + " " +data._conditionList[0]._statValue);
		
		return ob;
	}
	
	public ObjectiveProtoData UpdateLegendaryObjectiveStat(int ind)
	{
		if(ind<0 || ind>=ObjectivesManager.LegendaryObjectives.Count)	return null;
		
		ObjectiveProtoData data = ObjectivesManager.LegendaryObjectives[ind];
		
		UpdateLegendaryObjectiveStat(data);
		//if(data._conditionList[0]._type == ObjectiveType.CollectCoins)
		//	Debug.Log(data._title + " " +data._conditionList[0]._type + " " +data._conditionList[0]._earnedStatValue + " " +data._conditionList[0]._statValue);
		
		return data;
	}
	
	public ObjectiveProtoData UpdateWeeklyObjectiveStat(int ind)
	{
		if(ind<0 || ind>=Services.Get<WeeklyObjectives>().GetWeeklyObjectives().Count)	return null;
		
		ObjectiveProtoData data = Services.Get<WeeklyObjectives>().GetWeeklyObjectives()[ind];
		
		UpdateObjectiveStat(data);
		
		//if(data._conditionList[0]._type == ObjectiveType.CollectCoins)
		//	Debug.Log(data._title + " " +data._conditionList[0]._type + " " +data._conditionList[0]._earnedStatValue + " " +data._conditionList[0]._statValue);
		
		return data;
	}
	
	public void ClearObjectivesEarnedDuringRun()
	{
		objectivesEarnedDuringRun = new List<string>();
		objectivesEarnedDuringRun.Clear();
	}
	
	//-- Called during gameplay to track objectives.
//	public void AddObjectiveStat(ObjectiveType objType, ObjectiveTimeType timeType, ObjectiveFilterType filterType, int incrementValue) {
		
	
		
		/*if(timeType == ObjectiveTimeType.LifeTime) {
			//-- TODO Funnel this into something else
		}
		else {
			//-- find the objType in the active list.
			ObjectiveProtoData foundOb = null;
			int max = objectivesActive.Count;
			for (int i = 0; i < max; i++) {
				ObjectiveProtoData ob = objectivesActive[i];
				if(ob == null)
					continue;
				if(ob._conditionList[0]._type != objType || ob._conditionList[0]._timeType != timeType || ob._conditionList[0]._filterType != filterType)
					continue;
				foundOb = ob;
				//-- Breaking here mean we will only find the first one to match, but the design complies with this because we
				//-- should only have one of each type in the list.
				break;
			}
			
			if(foundOb == null)
				return;
			
			//-- Found active objective, lets progress on it.
			if(timeType == ObjectiveTimeType.OverTime) {
				foundOb._conditionList[0]._earnedStatValue += incrementValue;	
			}
			else if(timeType == ObjectiveTimeType.PerRun) {
				if(incrementValue >= foundOb._conditionList[0]._earnedStatValue) {	//_statValue) {		// '_statValue' replaced by '_earnedStatValue' by Alex,
					foundOb._conditionList[0]._earnedStatValue = incrementValue;							// 	to keep track of progress toward objective
				}
				
				if(foundOb._conditionList[0]._earnedStatValue > foundOb._conditionList[0]._statValue) {						// added by Alex, to cap progess value at objective's value
					foundOb._conditionList[0]._earnedStatValue = foundOb._conditionList[0]._statValue;
				}			
			}
			
			//-- Did we earn it?
			if(foundOb._conditionList[0]._earnedStatValue >= foundOb._conditionList[0]._statValue) {
				//-- TODO SHOW BANNER
			}
		}*/
//	}
	
	public ObjectiveProtoData GetActiveObjectiveFromIndex(int objectiveIndex) {
		if(objectivesActive == null)
			return null;
		if(objectiveIndex >= objectivesActive.Count)
			return null;
		
		return  objectivesActive[objectiveIndex];
	}
	
	public bool IsObjectiveComplete(int objectiveIndex) {
		ObjectiveProtoData foundOb = GetActiveObjectiveFromIndex(objectiveIndex);
		if(foundOb == null)
			return false;
		
		if(foundOb._conditionList[0]._earnedStatValue >= foundOb._conditionList[0]._statValue)
			return true;
		
		return false;
	}
	
	public float  GetObjectiveProgress(int objectiveIndex) {
		ObjectiveProtoData foundOb = GetActiveObjectiveFromIndex(objectiveIndex);
		if(foundOb == null)
			return 0.0f;
		
		return (float)foundOb._conditionList[0]._earnedStatValue / (float)foundOb._conditionList[0]._statValue;
	}
	
	public void RefillObjectives() {
		if(ObjectivesManager.Objectives == null || ObjectivesManager.Objectives.Count == 0)
			return;
		if(objectivesActive == null) {
			objectivesActive = new List<ObjectiveProtoData>();
		}
		
		for(int i=0; i<3; i++) {
			if(i >= objectivesActive.Count || objectivesActive[i] == null) {
				//-- Add new ob.	
				RefillObjectiveForIndex(i);	//, 0);
			}
			else {
				//-- This is done, add a new one.
				if(objectivesActive[i]._conditionList[0]._earnedStatValue >= objectivesActive[i]._conditionList[0]._statValue) {
					//-- Add old ob to earned list.
					if(objectivesEarned.Contains(objectivesActive[i]._id) == false) {
						objectivesEarned.Add (objectivesActive[i]._id);
					}
					//-- Find a new ob
					RefillObjectiveForIndex(i);	//, objectivesActive[i]._conditionList[0]._statValue);
				}	
			}
			if(i<objectivesActive.Count && objectivesActive[i] != null) {
			//	TR.LOG ("Objective:{0} is {1}", i, objectivesActive[i]._title);	
			}
			else
			{
				notify.Warning ("Objective:{0} is null", i);
			}
		}	
	}

	/*
	public ObjectiveProtoData RefillObjectiveForIndex(int slot, int statValue) 
	{
		ObjectiveType choice = ObjectiveType.Distance;
		List<ObjectiveType> currentTypes = new List<ObjectiveType>();
		for (int i=0; i<objectivesActive.Count; i++)
		{
			if (objectivesActive[i] != null && i == slot) { choice = objectivesActive[i]._type; }
			if (i == slot || objectivesActive[i] == null) { continue; }
			currentTypes.Add(objectivesActive[i]._type);
		}
		
		Array values = Enum.GetValues(typeof(ObjectiveType));		//-- Pick a type first.
		
		ObjectiveProtoData bestOb = null;
		while (bestOb == null) 
		{
			choice = (ObjectiveType)values.GetValue(0);	//(ObjectiveType)UnityEngine.Random.Range(0, (int)values.GetValue(values.Length-1)+1);
			for (int c=0; c<values.Length; c++)
			{
				if (currentTypes.Contains((ObjectiveType)values.GetValue(c)) == true) { continue; }
				choice = (ObjectiveType)values.GetValue(c);
				break;
			}
			
			//TR.LOG ("Looking for objectives of type "+choice);
			
			int lowestStatValue = int.MaxValue;
			foreach (ObjectiveProtoData ob in ObjectivesManager.Objectives) 
			{
				if (ob == null) { continue; }
				//TR.LOG ("({0},{1})", slot, ob._title);
				//-- Mark down that we have looked at this objective.
				
				if (ob._type != choice) { continue; }	//TR.LOG ("wrong choice {0} != {1}", ob._type, choice);
					
				if (objectivesEarned.Contains(ob._id) == true) { continue; }	//TR.LOG ("already earned");
				
				statValue = ob._statValue;
				if (ob._timeType == ObjectiveTimeType.OverTime) { statValue *= 10; }
				else if (ob._timeType == ObjectiveTimeType.LifeTime) { statValue *= 100; }
				
				if (ob._filterType == ObjectiveFilterType.WithoutCoins) { statValue *= 10; }
				else if (ob._filterType == ObjectiveFilterType.WithoutPowerups) { statValue *= 2; }
				else if (ob._filterType == ObjectiveFilterType.WithoutStumble) { statValue *= 5; }
				
				if (statValue > lowestStatValue) { continue; }	//TR.LOG ("statValue({0}) > lowestStatValue({1})", statValue, lowestStatValue);
				
				//TR.LOG ("Possible best {0}", ob._title);
				lowestStatValue = statValue;
				bestOb = ob;
			}
			
			//-- If we have looked at each objective, STOP LOOKING. 
			currentTypes.Add (choice);
			if (bestOb == null) { if(currentTypes.Count >= values.Length) { break; } }
		}
		
		if (bestOb != null)
		{
			if (slot>=objectivesActive.Count) { objectivesActive.Add (bestOb); }
			else { objectivesActive[slot] = bestOb;	}	
		}
		else if (slot < objectivesActive.Count) { objectivesActive[slot] = bestOb; }
		return bestOb;
	}
*/
	
	public ObjectiveProtoData RefillObjectiveForIndex(int slot)	//, int statValue)	// our new 'objective picking' method
	{
		//int failCount = 0;
		
		// use the difficulty ratings to determine which difficulty pool to pick from
		// don't pick objective from same category as the existing two
		// Use ObjectivesManager.GetObjectivesByDifficultyWithThreeCategoriesRemoved().
		
		//if (rand == null) { rand = Services.Get<Rand>(); }	// get reference to random number generator
		
		//int objectiveNumber = (GameProfile.SharedInstance.Player.objectivesEarned.Count - 1) % 3;
		//int difficulty = ObjectiveLevelManager.GetDifficulty(GameProfile.SharedInstance.Player.GetCurrentRank(), objectiveNumber);
		int difficulty = 1;
		int objectiveNumber = GameProfile.SharedInstance.Player.objectivesEarned.Count % 3;		// returns 1, 2, 0 -> convert the 0 to a 3 when getting difficulty
		
		//if (objectiveNumber == 0)
		//	objectiveNumber = 3;	// get third objective at the level
		
		
		if (GameProfile.SharedInstance.Player.objectivesEarned.Count > 0)
			difficulty = ObjectiveLevelManager.GetDifficulty(GameProfile.SharedInstance.Player.GetCurrentRank(), objectiveNumber);
		
		//Debug.Log(difficulty);
		
		//NOTE: Commented out, in case we can't find any new levels.
	/*	if (difficulty <= -1) // no more objectives to pull
		{
			if (slot >= objectivesActive.Count || slot < 0) 
				objectivesActive.Add(null); 
			else
				objectivesActive[slot] = null;	
			return null;
		}*/
		
		if(difficulty<0)	difficulty = 0;
			
		ObjectiveDifficulty diffEnumVal = (ObjectiveDifficulty)(difficulty);	// - 1);
		
		ObjectiveProtoData obj1 = (objectivesActive.Count > 0) ? objectivesActive[0] : null;
		ObjectiveProtoData obj2 = (objectivesActive.Count > 1) ? objectivesActive[1] : null;
		ObjectiveProtoData obj3 = (objectivesActive.Count > 2) ? objectivesActive[2] : null;		
		
		// get new objective, taking into consideration the chosen difficulty level and choosing from different categories as the current objectives
		List<ObjectiveProtoData> newObjPossibilities = ObjectivesManager.GetObjectivesByDifficultyWithThreeCategoriesRemoved(diffEnumVal, obj1, obj2, obj3);
		
		// stop categories from being eliminated when choosing new objectives, so all objectives end up getting used up
		if (newObjPossibilities.Count == 0)
			 newObjPossibilities = ObjectivesManager.GetObjectivesByDifficultyWithThreeCategoriesRemoved(diffEnumVal, obj1, obj2, null);
	
		if (newObjPossibilities.Count == 0)
			 newObjPossibilities = ObjectivesManager.GetObjectivesByDifficultyWithThreeCategoriesRemoved(diffEnumVal, obj1, null, null);
		
		if (newObjPossibilities.Count == 0)
			 newObjPossibilities = ObjectivesManager.GetObjectivesByDifficultyWithThreeCategoriesRemoved(diffEnumVal, null, null, null);
		
		if (newObjPossibilities.Count == 0)
		{
		//	notify.Warning("Objective not found for target difficulty, forced to find one from a previous difficulty! "+ diffEnumVal + " Slot:" + slot);
			newObjPossibilities = ObjectivesManager.GetObjectivesByDifficultyWithThreeCategoriesRemoved(diffEnumVal, null, null, null, false);
		}
		while(newObjPossibilities.Count==0 && (int)diffEnumVal<System.Enum.GetNames(typeof(ObjectiveDifficulty)).Length)
		{
			diffEnumVal++;
	//		notify.Warning("Objective not found for target difficulty or lower, forced to find one from a HIGHER difficulty! "+ diffEnumVal + " Slot:" + slot);
			newObjPossibilities = ObjectivesManager.GetObjectivesByDifficultyWithThreeCategoriesRemoved(diffEnumVal, null, null, null, false);
		}
		
	//	Debug.Log(newObjPossibilities.Count);
		
		//int objVal = Rand.GetRandomInt(0, newObjPossibilities.Count);			// bottom value inclusive, top value not inclusive
		int objVal = UnityEngine.Random.Range(0, newObjPossibilities.Count);	// bottom value inclusive, top value not inclusive
		
	//	Debug.Log(slot + " " +objectivesActive.Count + " " +objVal + " " + newObjPossibilities.Count);
	
		if(newObjPossibilities.Count>0)
		{
			if (slot >= objectivesActive.Count || slot < 0) 
				objectivesActive.Add(newObjPossibilities[objVal]); 
			else
				objectivesActive[slot] = newObjPossibilities[objVal];	
			
			return newObjPossibilities[objVal];
		}
		else
		{
			if (slot >= objectivesActive.Count || slot < 0) 
				objectivesActive.Add(null); 
			else
				objectivesActive[slot] = null;	
		}
			
		return null;
		
	}	
	
	public RankRewardType GetRankRewardTypeForLevel(int level)
	{
		if(ObjectiveLevelManager.objectiveLevelsList.Count>level)
			return ObjectiveLevelManager.objectiveLevelsList[level].rewardType;
		else return RankRewardType.Coins;
		//if (level % 2 == 1) { return RankRewardType.Coins; }
		//else { return RankRewardType.Gems; } 
	}
	
	public int GetRankRewardQuanityOrItemForLevel(int level)	//, RankRewardType reward) 
	{
		if (level < ObjectiveLevelManager.objectiveLevelsList.Count)			// check if within range of all objective levels
			return ObjectiveLevelManager.objectiveLevelsList[level].rewardValue;	
		else return 0;
		
		//if (reward == RankRewardType.Coins) { return level*1000; }
		//else if (reward == RankRewardType.Gems) { return level*2; }
		//return 0;
	}
	
	public string GetRewardTextFor(int level)	//(RankRewardType reward, int QtyOrItemID) 
	{
		int quantity = GetRankRewardQuanityOrItemForLevel(level);
		string type = GetRankRewardTypeForLevel(level).ToString();

		
		string str1 = "";
		RankRewardType rewardType = GetRankRewardTypeForLevel(level);
		switch(rewardType){
			case RankRewardType.Multipliers:
				str1 = Localization.SharedInstance.Get("Lbl_LevelReward_1");
				break;
			case RankRewardType.Coins:
				str1 = Localization.SharedInstance.Get("Lbl_LevelReward_2");
				break;
			case RankRewardType.Gems:
				str1 = Localization.SharedInstance.Get("Lbl_LevelReward_3");
				if(quantity == 1)
					str1 = Localization.SharedInstance.Get("Lbl_LevelReward_4");
				break;
			case RankRewardType.HeadStartConsumables:
				str1 = Localization.SharedInstance.Get("Lbl_LevelReward_5");
				break;
			case RankRewardType.MegaHeadStartConsumables:
				str1 = Localization.SharedInstance.Get("Lbl_LevelReward_6");
				break;
			case RankRewardType.NoStumbleConsumables:
				str1 = Localization.SharedInstance.Get("Lbl_LevelReward_7");
				break;
			case RankRewardType.ThirdEyeConsumables:
				str1 = Localization.SharedInstance.Get("Lbl_LevelReward_8");
				break;
				
		}
		
		notify.Debug("GetRewardTextFor " + level + " quantity " + quantity + " type " + type);
		string loc = Localization.SharedInstance.Get("Lbl_LevelRewardAssemble");
		string reward = string.Format(str1, quantity);
		string text = string.Format(loc,reward, (level + 1).ToString());

		return text;
	}
	
	public string GetRewardIconFor(int level)	//(RankRewardType reward, int QtyOrItemID) 
	{
		if (level <= ObjectiveLevelManager.objectiveLevelsList.Count - 1){			// check if within range of all objective levels
			//return ObjectiveLevelManager.objectiveLevelsList[level].iconImage;	
			string icon = "";
			switch(ObjectiveLevelManager.objectiveLevelsList[level].rewardType){
				case RankRewardType.Coins: icon = "coinbundle_01";
					break;
				case RankRewardType.Gems: icon = "gembundle_01";
					break;
				case RankRewardType.Multipliers: icon = "icon_multiplier2";
					break;
			}
	//		Debug.Log ("level " + level + " reward " + ObjectiveLevelManager.objectiveLevelsList[level].rewardType + " icon " + icon);
			return icon;
		}
		else return "";	// return empty sprite if over range
		
		//if (reward == RankRewardType.Coins) { return "powerup_coin_bonus"; }
		//if (reward == RankRewardType.Gems) { return "powerup_gem_bonus"; }
		//return "";
	}
	
	private static int HeadStartCost = 2500;
	private static int MegaHeadStartCost = 5000;
	private static int EnvHeadStartCost = 7500;
	
	public int GetHeadStartCost() {
		return (int)(HeadStartCost * GameProfile.SharedInstance.GetHeadStartDiscount());
	}
	
	public int GetMegaHeadStartCost() {
		return (int)(MegaHeadStartCost * GameProfile.SharedInstance.GetMegaHeadStartDiscount());
	}
	
	public int GetEnvHeadStartCost() {
		return (int)(EnvHeadStartCost * GameProfile.SharedInstance.GetEnvHeadStartDiscount());
	} //added fast travel so can get discount as well N.N.
		
	[Obsolete ("CanAffordHeadStart is deprecated.  Environment Head Starts are now a consumable", true) ]
	public bool CanAffordHeadStart(bool mega) 
	{ 
		if(mega == false) 
		{
			if(this.GetType()==typeof(FastTravelConsumable))
			{
				return(coinCount >= GetEnvHeadStartCost());
			}
			return(coinCount >= GetHeadStartCost());
		}
		return (coinCount >= GetMegaHeadStartCost());
	}
};


		
		//{
			//return ObjectivesManager.FindObjectiveByID(355);
			
//			Dictionary<string, object> d = new Dictionary<string, object>();
//			d.Add ("Title", "Empty objective");
//			d.Add ("InternalTitle", "");		
//			d.Add ("IconNamePre", "icon_map_fasttravel");
//			d.Add ("IconName", "icon_map_fasttravel");
//			d.Add ("DescriptionPre", "Empty objective!!!");
//			d.Add ("Description", "Empty objective!!!");
//			d.Add ("Points", 9999999999);
//			d.Add ("Hidden", false);
//			d.Add ("CanEarnMoreThanOnce", false);
//			d.Add ("EnvironmentID", 1);
//			
//			List<object> conditionList = new List<object>();
//			d.Add ("conditions", conditionList);
//			d.Add ("Category", "");
//			d.Add ("Difficulty", "1");
//			d.Add ("rewardType", "");
//			d.Add ("rewardValue", "");
//			
//			d.Add ("PID", 999999999);
			
//			return new ObjectiveProtoData((d));
//		}


		
//		if (newObjPossibilities.Count == 0)
//			 newObjPossibilities = ObjectivesManager.GetObjectivesByDifficultyWithThreeCategoriesRemoved(diffEnumVal, obj1, obj2, null, true);	// disregard difficulty	
//		
//		if (newObjPossibilities.Count == 0)
//			 newObjPossibilities = ObjectivesManager.GetObjectivesByDifficultyWithThreeCategoriesRemoved(diffEnumVal, obj1, null, null, true); // disregard difficulty	
//
//		if (newObjPossibilities.Count == 0)
//			 newObjPossibilities = ObjectivesManager.GetObjectivesByDifficultyWithThreeCategoriesRemoved(diffEnumVal, null, null, null, true);	// disregard difficulty
//			


		//return ObjectiveLevelManager.objectiveLevelsList[level].description;
//		if (reward == RankRewardType.Coins) 
//		{
//			if (QtyOrItemID == 1) { return String.Format("{0} Coin", QtyOrItemID); }
//			return String.Format("{0} Coins", QtyOrItemID);
//		}
//		if (reward == RankRewardType.Gems)
//		{
//			if(QtyOrItemID == 1) { return String.Format("{0} Gem", QtyOrItemID); }
//			return String.Format("{0} Gems", QtyOrItemID);
//		}
//		return "";

			//Debug.LogWarning("objVal = " + objVal.ToString() + ", " + "newObjPossibilities.Count = " + newObjPossibilities.Count.ToString());
			
			//if (newObjPossibilities.Count == 0 || objVal >= newObjPossibilities.Count-1)
			//{
			//	failCount++;
			//	Debug.LogWarning("newObjPossibilities.Count = " + newObjPossibilities.Count.ToString() + ", failure #: " + failCount.ToString());
				//RefillObjectiveForIndex(slot, statValue);	// try the whole thing again, since got nothing back...
			//}
			//else


			//diffEnumVal, objectivesActive[0], objectivesActive[1], objectivesActive[2]);	

//	public ObjectiveProtoData RefillObjectiveForIndex(int slot, int statValue) {
//		
//		ObjectiveType choice = ObjectiveType.Distance;
//		List<ObjectiveType> currentTypes = new List<ObjectiveType>();
//		for(int i=0; i<objectivesActive.Count; i++) {
//			if(objectivesActive[i] != null && i == slot)
//				choice = objectivesActive[i]._type;
//			if(i == slot || objectivesActive[i] == null) {
//				continue;
//			}
//			currentTypes.Add(objectivesActive[i]._type);
//			
//		}
//		
//		//-- Pick a type first.
//		Array values = Enum.GetValues(typeof(ObjectiveType));
//		
//		ObjectiveProtoData bestOb = null;
//		while(bestOb == null) {
//			
//			choice = (ObjectiveType)values.GetValue(0);//(ObjectiveType)UnityEngine.Random.Range(0, (int)values.GetValue(values.Length-1)+1);
//			for(int c=0; c<values.Length; c++) {
//				if(currentTypes.Contains((ObjectiveType)values.GetValue(c)) == true)
//					continue;
//				choice = (ObjectiveType)values.GetValue(c);
//				break;
//			}
//			
//			//TR.LOG ("Looking for objectives of type "+choice);
//			
//			int lowestStatValue = int.MaxValue;
//			foreach(ObjectiveProtoData ob in ObjectivesManager.Objectives) {
//				
//				if(ob == null)
//					continue;
//				//TR.LOG ("({0},{1})", slot, ob._title);
//				//-- Mark down that we have looked at this objective.
//				
//				
//				if(ob._type != choice)
//				{
//					//TR.LOG ("wrong choice {0} != {1}", ob._type, choice);
//					continue;
//				}
//					
//				if(objectivesEarned.Contains(ob._id) == true) {
//					//TR.LOG ("already earned");
//					continue;
//				}
//				
//				statValue = ob._statValue;
//				if(ob._timeType == ObjectiveTimeType.OverTime)
//					statValue *= 10;
//				else if(ob._timeType == ObjectiveTimeType.LifeTime)
//					statValue *= 100;
//				
//				if(ob._filterType == ObjectiveFilterType.WithoutCoins)
//					statValue *= 10;
//				else if(ob._filterType == ObjectiveFilterType.WithoutPowerups)
//					statValue *= 2;
//				else if(ob._filterType == ObjectiveFilterType.WithoutStumble)
//					statValue *= 5;
//				
//				
//				if(statValue > lowestStatValue)
//				{
//					//TR.LOG ("statValue({0}) > lowestStatValue({1})", statValue, lowestStatValue);
//					continue;
//				}
//				//TR.LOG ("Possible best {0}", ob._title);
//				lowestStatValue = statValue;
//				bestOb = ob;
//			}
//			
//			//-- If we have looked at each objective, STOP LOOKING. 
//			currentTypes.Add (choice);
//			if(bestOb == null) {
//				if(currentTypes.Count >= values.Length)
//					break;
//			}
//		}
//		
//		if(bestOb != null) {
//			if(slot>=objectivesActive.Count) {
//				objectivesActive.Add (bestOb);
//			}
//			else {
//				objectivesActive[slot] = bestOb;	
//			}	
//		}
//		else if(slot < objectivesActive.Count){
//			objectivesActive[slot] = bestOb;
//		}
//		return bestOb;
//	}
	
