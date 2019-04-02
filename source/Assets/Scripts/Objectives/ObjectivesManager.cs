using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class ObjectivesManager : MonoBehaviour
{
	protected static Notify notify;
	public static List<ObjectiveProtoData> Objectives = new List<ObjectiveProtoData>();
	public static List<ObjectiveProtoData> LegendaryObjectives = new List<ObjectiveProtoData>();
	
	//Quick access to protodata based on objective type, so we dont have to search through them all each time
//	public static Dictionary<ObjectiveType,List<ObjectiveProtoData>> QuickAccessLegendaryObjectives = new Dictionary<ObjectiveType, List<ObjectiveProtoData>>();
	public static List<ObjectiveProtoData>[] QuickAccessLegendaryObjectives;
	
	private WeeklyObjectives weeklyObjs;
	public List<ObjectiveProtoData> GetWeeklyObjectives() { return weeklyObjs.GetWeeklyObjectives(); }
	
	public WeeklyObjectives GetWeeklyObjectivesClass() { return weeklyObjs; }
	
	public Dictionary<int,int> PreviousWeeklyObjectiveStats { get; private set; }
	

	
	// wxj
	public static List<ObjectiveProtoData> ActivityObjectives = new List<ObjectiveProtoData>();
	public static List<ObjectiveProtoData> AllActivityObjectives = new List<ObjectiveProtoData>();
	// wxj
	public static void loadActivityObjectives()
	{
		int itemId = 1002 + getActivityItemId();
		//itemId = 1005;
		ActivityObjectives.Clear();
		
		// if ObjectivesActivity is not exists, load game resource OZGameData/ObjectivesActivity
		if(!loadFileFromPersistent(AllActivityObjectives, "ObjectivesActivity"))
		{
			LoadFile(AllActivityObjectives, "OZGameData/ObjectivesActivity");
			saveFileToPersistent(AllActivityObjectives, "ObjectivesActivity");
		}
		else
		{
			notify.Info("wxj: loadActivityObjectives() loadFileFromPersistent successs");
		}
		
		UnityEngine.Debug.Log("wxj: AllActivityObjectives.count:"+AllActivityObjectives.Count);
	
		foreach(ObjectiveProtoData data in AllActivityObjectives)
		{
			if(data._id == itemId)
			{
				// if activity is overdue, reset data
				if(data._endDate != null && DateTime.Compare(data._endDate, DateTime.Now) <= 0)
				{
					resetActivityObjective(data);
				}
				// set endDate is next day 00:00
				DateTime temp = DateTime.Now.AddDays(1);
				data._endDate = new DateTime(temp.Year, temp.Month, temp.Day);
				//data._endDate = DateTime.Now.AddSeconds(20);
				ActivityObjectives.Add(data);
				//data._conditionList[0]._earnedStatValue = 4;
				//GameProfile.SharedInstance.Player.objectivesUnclaimed.Add(data._id);
				//UnityEngine.Debug.Log("wxj: load activity objective _actiEarnedStatForEnvs:"+data._conditionList[0]._actiEarnedStatForEnvs[1]);
				UnityEngine.Debug.Log("wxj: data._endDate:"+data._endDate);
				UnityEngine.Debug.Log("wxj: load activity objective earned value:"+data._conditionList[0]._earnedStatValue);
				UnityEngine.Debug.Log("wxj: load activity objective id:"+data._id);
				//resetActivityObjective(data);
			}
		}	
		
		
	}
	
	// wxj, update activity to next day 
	public static DateTime updateActiForNextDay()
	{
		foreach(ObjectiveProtoData data in ActivityObjectives)
		{
			// wxj, reset
			data._endDate = data._endDate.AddDays(1);
			resetActivityObjective(data);
			return data._endDate;
		}
		return DateTime.Now;
	}
	
	// wxj, reset data
	public static void resetActivityObjective(ObjectiveProtoData data)
	{
		data._conditionList[0]._earnedStatValue = 0;
		data._conditionList[0]._earnedNeighborValue = 0;
		data._conditionList[0]._actiEarnedStatForEnvs = new Dictionary<int, float>();
		data._conditionList[0]._actiRewardedCount = 0;
	}
	
	
	// wxj, get file content from files that in Application.persistentDataPath
	private static string getFileContent(string fileName)
	{
		string filePath = Application.persistentDataPath + Path.DirectorySeparatorChar + fileName;
		
		if(!File.Exists(filePath))
		{
			return null;
		}
		
		try
		{
			StreamReader reader = File.OpenText(filePath);
			StringBuilder strBuilder = new StringBuilder();
			string buff = null;
			while((buff = reader.ReadLine()) != null)
			{
				strBuilder.Append(buff);
			}
			
			reader.Close();
			return strBuilder.ToString();
		}
		catch(Exception err)
		{
			notify.Warning("wxj: open file failed, "+filePath);
		}
		return null;	
	}
	
	// wxj, get activity item which get by key from xml file
	private static int getActivityItemId()
	{
		int item = 1;
		item = PlayerPrefs.GetInt("activity_item", 1);	
		item = Mathf.Clamp(item, 1, 4);
			
			/*
			 * 
			 * 
			 *string jsonStr = getFileContent("game_settings.txt");
			if(jsonStr == null)
			{
				return item;
			}
			Dictionary<string, object> json = MiniJSON.Json.Deserialize(jsonStr) as Dictionary<string, object>;
			item = int.Parse(json["activity_item"].ToString());
		
		
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(filePath);
			XmlNodeList nodes = xmlDoc.SelectSingleNode("map").ChildNodes;
			foreach(XmlNode node in nodes)
			{
				if(((XmlElement)node).GetAttribute("name").Equals("activity_item"))
				{
					item = Int32.Parse(((XmlElement)node).GetAttribute("value"));
					return item;
				}
			}
			*/
		return item;
	}

	// wxj, save file in persistentDataPath
	public static bool saveFileToPersistent(List<ObjectiveProtoData> objectivesList, string fileName)
	{
		if(objectivesList == null)
		{
			return false;
		}
		
		string filePath = Application.persistentDataPath + Path.DirectorySeparatorChar + fileName + ".txt";
		List<object> list = new List<object>();
			
		foreach (ObjectiveProtoData data in objectivesList)	
		{ 
			//data._conditionList[0]._earnedStatValue = 0;
			list.Add(data.ToDict());
		}
		//-- Hash before we save.
		Dictionary<string, object> secureData = SaveLoad.Save(list);
		string listString = MiniJSON.Json.Serialize(secureData);
		
		try 
		{
			using (StreamWriter fileWriter = File.CreateText(filePath))	
			{
				fileWriter.WriteLine(listString);
				fileWriter.Close(); 
			}
			notify.Info("wxj: saveFileToPersistents succeed, filePath: " + filePath);
			return true;
		}
		catch (Exception e) 
		{
			Dictionary<string,string> d = new Dictionary<string, string>();
			d.Add("Exception",e.ToString());
			notify.Warning("wxj: saveFileToPersistents error: " + e);
		}
		return false;
	}
	
	// wxj, load file in persistentDataPath
	public static bool loadFileFromPersistent(List<ObjectiveProtoData> objectivesList, string fileName)
	{		
		string jsonStr = getFileContent(fileName + ".txt");
		if(jsonStr == null)
		{
			return false;
		}
		
		Dictionary<string, object> loadedData = MiniJSON.Json.Deserialize(jsonStr) as Dictionary<string, object>;
		if (SaveLoad.Load(loadedData) == false)
		{
#if !UNITY_EDITOR			
			return false;
#endif
		}
		
		List<object> store = loadedData["data"] as List<object>;
		if (store == null) { return false; }
		
		foreach (object dict in store) 
		{
			Dictionary<string, object> data = dict as Dictionary<string, object>;
			ObjectiveProtoData ob = new ObjectiveProtoData(data);
#if UNITY_EDITOR			
			if (ob._id == -1) 
				ob._id = ObjectivesManager.GetNextID(objectivesList);
#endif
			objectivesList.Add(ob);	//Objectives.Add(ob);
		} 
		
		return true;
			
	}
	
	
	
	
	void Awake()
	{
		notify = new Notify(this.GetType().Name);	

		weeklyObjs = gameObject.AddComponent<WeeklyObjectives>();
	}
	
	public static bool LoadFile(List<ObjectiveProtoData> objectivesList, string fileName)
	{		
		if (notify == null)
		{
			notify = new Notify("ObjectivesManager");
		}
		objectivesList.Clear();
		
		TextAsset text = Resources.Load(fileName) as TextAsset;
		if (text == null)
		{
			UnityEngine.Debug.Log("wxj: No Objectives file exists at: " + fileName);
			notify.Warning("No Objectives file exists at: " + fileName);	
			return false;
		}

		//-- Security check
		//notify.Debug("ObjectivesManager " + text.text);
		Dictionary<string, object> loadedData = MiniJSON.Json.Deserialize(text.text) as Dictionary<string, object>;
		if (SaveLoad.Load(loadedData) == false)
		{
#if !UNITY_EDITOR			
			return false;
#endif
		}
		
		List<object> store = loadedData["data"] as List<object>;
		if (store == null) { return false; }
		
		foreach (object dict in store) 
		{
			Dictionary<string, object> data = dict as Dictionary<string, object>;
			ObjectiveProtoData ob = new ObjectiveProtoData(data);
#if UNITY_EDITOR			
			if (ob._id == -1) 
				ob._id = ObjectivesManager.GetNextID(objectivesList);
#endif
			objectivesList.Add(ob);	//Objectives.Add(ob);
		} 
		
		if (GameProfile.SharedInstance != null)
		{
			PlayerStats playerToLoadTo = GameProfile.SharedInstance.Player;
			
			//Set up the quick-access legendary obejctives
			ObjectivesManager.SetUpQuickAccess(playerToLoadTo);
			
			ObjectivesManager.InitializeLegendaryProgress(playerToLoadTo);
		}
		
		return true;
	}
	
	public static void InitializeLegendaryProgress(PlayerStats loadToPlayer)
	{
		for(int i=0;i<LegendaryObjectives.Count;i++)
		{
			ObjectiveProtoData opd = LegendaryObjectives[i];
			
			if(loadToPlayer.legendaryProgress.ContainsKey(opd._id))
				opd._conditionList[0]._earnedStatValue = (int)loadToPlayer.legendaryProgress[opd._id];
		}
	}
	
	public void WebInitializeLegendaryProgress(PlayerStats loadToPlayer)
	{
		for (int i=0; i < LegendaryObjectives.Count; i++)
		{
			ObjectiveProtoData opd = LegendaryObjectives[i];
			
			if (loadToPlayer.legendaryProgress.ContainsKey(opd._id))
				opd._conditionList[0]._earnedStatValue = (int)loadToPlayer.legendaryProgress[opd._id];
		}
	}
	
	public static void SetUpQuickAccess(PlayerStats loadToPlayer)
	{
		QuickAccessLegendaryObjectives = new List<ObjectiveProtoData>[System.Enum.GetValues(typeof(ObjectiveType)).Length];
		for(int i=0;i<LegendaryObjectives.Count;i++)
		{
			ObjectiveType type = LegendaryObjectives[i]._conditionList[0]._type;
			
			if(QuickAccessLegendaryObjectives[(int)type] == null)
				QuickAccessLegendaryObjectives[(int)type] = new List<ObjectiveProtoData>();
			
			//Only add it to the quick-access if we have not completed this objective. This is for optimzation during a run.
			if(!loadToPlayer.legendaryObjectivesEarned.Contains(LegendaryObjectives[i]._id))
				QuickAccessLegendaryObjectives[(int)type].Add(LegendaryObjectives[i]);
		}
	}
	
	public static void SaveFile(List<ObjectiveProtoData> objectivesList, string fileName)
	{
		using (MemoryStream stream = new MemoryStream()) 
		{
			//string fileName = Application.dataPath + Path.DirectorySeparatorChar + "Resources" + Path.DirectorySeparatorChar + "OZGameData/Objectives.txt";
			string fileNameWithPath = Application.dataPath + Path.DirectorySeparatorChar + "Resources" + Path.DirectorySeparatorChar + fileName + ".txt";
			List<object> list = new List<object>();
			
			foreach (ObjectiveProtoData data in objectivesList)	//Objectives) 
			{ 
				//data._conditionList[0]._earnedStatValue = 0;
				list.Add(data.ToDict());
			}
			//-- Hash before we save.
			Dictionary<string, object> secureData = SaveLoad.Save(list);
			string listString = MiniJSON.Json.Serialize(secureData);
			
			try 
			{
				using (StreamWriter fileWriter = File.CreateText(fileNameWithPath))	//fileName)) 
				{
					fileWriter.WriteLine(listString);
					fileWriter.Close(); 
				}
			}
			catch (Exception e) 
			{
				Dictionary<string,string> d = new Dictionary<string, string>();
				d.Add("Exception",e.ToString());
				notify.Warning("Save Exception: " + e);
			}
		}
	}
	
#if UNITY_EDITOR	
	public static int GetNextID(List<ObjectiveProtoData> list) 
	{
		int nextID = 0;
		
		foreach (ObjectiveProtoData p in list)
		{
			if (p == null) { continue; }
			nextID = p._id + 1;
		}
		return nextID;
	}
	
	public static int getTotalPoints() 
	{
		if (Objectives == null) { return 0; }
		int total = 0;
		foreach (ObjectiveProtoData data in Objectives) { total += data._pointValue; }
		return total;
	}
#endif
	
	public static ObjectiveProtoData FindObjectiveByID(int searchID) 
	{
		if (Objectives == null) { return null; }
		
		foreach (ObjectiveProtoData p in Objectives) 
		{
			if (p == null) { continue; }
			if (p._id == searchID) { return p; }
		}
		return null;
	}
	
	public static string GetIconName(int id)
	{			
		string[] iconNames = new string[9] 
		{ 
			"icon_obj_class_coin", 			// Coin
			"icon_obj_class_collection", 	// Collection
			"icon_obj_class_discovery",		// Discovery
			"icon_obj_class_distance",		// Distance
			"icon_obj_class_legendary",		// Lifetime (Legendary?)
			"icon_obj_class_obstacle",		// Obstacles
			"icon_obj_class_legendary",		// Purchases (Weekly?)
			"icon_obj_class_score",			// Score
			"icon_obj_class_skill",			// Skill
		};
		
		return iconNames[id];
	}
	
	public static string GetRewardIconSpriteName( int id )
	{
		string[] rewardIconSpriteName = new string[8]
		{
			"currency_coin",				//"coinbundle_01", 				// Coins
			"currency_gem",					//"gembundle_01", 				// Gems
			"icon_stats_multiplier",		//"icon_multiplier1",				// Multipliers
			"icon_reward_mega_head_start",				// HeadStartConsumables
			"multiplier3",					// ExtraMultiplierConsumables
			"icon_stumbleproof",			// NoStumbleConsumables
			"icon_headstart",				// ThirdEyeConsumables
			"icon_reward_mega_head_start",			// MegaHeadStartConsumables
		};
		
		return rewardIconSpriteName[ id ];
	}
	
	public static string GetRewardIconName(int id)
	{
		string[] rewardIconNames = new string[8]
		{ 
			"coinbundle_01", 				// Coins
			"gembundle_01", 				// Gems
			"icon_multiplier1",				// Multipliers
			"icon_thirdeye",				// HeadStartConsumables
			"multiplier3",					// ExtraMultiplierConsumables
			"icon_stumbleproof",			// NoStumbleConsumables
			"icon_headstart",				// ThirdEyeConsumables
			"icon_megaheadstart",			// MegaHeadStartConsumables
		};
		
		return rewardIconNames[id];
	}
	
	public static void RewardLegendaryObjective(int objIndex)
	{
		//notify.Warning("Weekly Objective reward during gameplay");
		
		foreach (ObjectiveProtoData obj in LegendaryObjectives)
		{
			if (objIndex == obj._id)
			{
				switch (obj._rewardType)
				{
					//For now, multipliers are calculated elsewhere (we should consolidate this some time...)
					//case RankRewardType.Multipliers:
					//	GameProfile.SharedInstance.ChallengeScoreMultiplier += obj._rewardValue;
					//	break;
					case RankRewardType.Coins:
						GamePlayer.SharedInstance.AddCoinsToScore(obj._rewardValue);
						break;
					case RankRewardType.Gems:
						GamePlayer.SharedInstance.AddGemsToScore(obj._rewardValue);
						break;
				}
			}
		}
	}
	
	
	// wxj, call when pressed collect button
	public static void RewardActivityObjective(int objIndex)
	{
		foreach (ObjectiveProtoData obj in ActivityObjectives)
		{
			if (objIndex == obj._id)
			{
				switch (obj._rewardType)
				{
					case RankRewardType.Coins:
						GameProfile.SharedInstance.Player.coinCount += obj._rewardValue;
						break;
					case RankRewardType.Gems:
						GameProfile.SharedInstance.Player.specialCurrencyCount += obj._rewardValue;
						break;
					case RankRewardType.MegaHeadStartConsumables:
						GameProfile.SharedInstance.Player.consumablesPurchasedQuantity[2] += obj._rewardValue;	// add 1 to purchased quantity
						break;
				}
				
				int tempCount = ++obj._conditionList[0]._actiRewardedCount;
				UnityEngine.Debug.Log("wxj: _actiRewardedCount:"+obj._conditionList[0]._actiRewardedCount);
				
				// wxj, if activity4 not out of reward count, reset objectives data
				if(obj._conditionList[0]._type == ObjectiveType.Activity4 && obj._conditionList[0]._actiRewardedCount < 5)
				{
					ObjectivesManager.resetActivityObjective(obj);
					obj._conditionList[0]._actiRewardedCount = tempCount;
				}
				
				// save objective condition
				ObjectivesManager.SaveFile(ObjectivesManager.AllActivityObjectives, "OZGameData/ObjectivesActivity");
					
			}
		}
	}
	
	public static List<ObjectiveProtoData> GetObjectivesByDifficultyWithThreeCategoriesRemoved
		(ObjectiveDifficulty diff, ObjectiveProtoData objective1, ObjectiveProtoData objective2, ObjectiveProtoData objective3, bool exactDifficulty = true)	//, bool disregardDifficulty = false)
	{
		List<ObjectiveProtoData> list = new List<ObjectiveProtoData>();
		
		foreach (ObjectiveProtoData ob in ObjectivesManager.Objectives)
		{
			if (!GameProfile.SharedInstance.Player.objectivesEarned.Contains(ob._id) &&	// exclude completed objectives	
				!GameProfile.SharedInstance.Player.objectivesActive.Contains(ob))		// exclude active objectives (NOTE: This will not work. instead, I am comparing ID's in the below "if" statement
			{
				int obCat = (int)ob._category;											// convert enum to int
				int cat1 = (objective1 != null) ? (int)objective1._category : -2;		// check if nulls passed into function. 
				int cat2 = (objective2 != null) ? (int)objective2._category : -2;		// (will occur on first launch when no objectives chosen yet)
				int cat3 = (objective3 != null) ? (int)objective3._category : -2;	
				
				//Make sure we don't already have this objective active
				if(GameProfile.SharedInstance.Player.objectivesActive.Count>=3)
				{
					if(GameProfile.SharedInstance.Player.objectivesActive[0]!=null && GameProfile.SharedInstance.Player.objectivesActive[0]._id==ob._id)
						continue;
					if(GameProfile.SharedInstance.Player.objectivesActive[1]!=null && GameProfile.SharedInstance.Player.objectivesActive[1]._id==ob._id)
						continue;
					if(GameProfile.SharedInstance.Player.objectivesActive[2]!=null && GameProfile.SharedInstance.Player.objectivesActive[2]._id==ob._id)
						continue;
				}
				
				if (obCat != cat1 && obCat != cat2 && obCat != cat3)
				{
					if(exactDifficulty)
					{
						if (ob._difficulty == diff && ob._environmentID<=EnvironmentSetBootstrap.BootstrapList.Count)
							list.Add(ob);
					}
					else
					{
						if (ob._difficulty <= diff && ob._environmentID<=EnvironmentSetBootstrap.BootstrapList.Count)
							list.Add(ob);
					}
				}
			}
		}
	
		if(notify!=null)
		{
			if (list.Count == 0)
			{
				notify.Debug("Failure at difficulty = " + diff.ToString() + ", returning 0 objectives");
				//list = GetObjectivesByDifficultyWithThreeCategoriesRemoved(diff, objective1, objective2, objective3, true);	// try again recursively
			}
			else
				notify.Debug("Success at difficulty = " + diff.ToString() + ", returning " + list.Count + " objectives");
		}
		return list;
	}	
	
	public void AddChallengeStat(ObjectiveType objType, ObjectiveTimeType timeType, 
		ObjectiveFilterType filterType, int incrementValue)
	{
		weeklyObjs.AddChallengeStat(objType, timeType, filterType, incrementValue);
	}
	
	public void SaveWeeklyChallenges()
	{
		weeklyObjs.SaveChallenges();	
	}
	
	public void CompleteChallenges()
	{
		weeklyObjs.CompleteChallenges();	
	}
	
	public void ApplyWeeklyChallenge(List<object> challengeList, int responseCode)
	{
		weeklyObjs.ApplyChallengesFromInit(challengeList, responseCode);
	}
	
	[Obsolete( "Add Neighbor stats is obsolete", true )]
	public void AddNeighborStats()
	{
		// weeklyObjs.UpdateGuildChallenges();
	}
	
	public void LoadLocalWeeklyChallenge()
	{
		weeklyObjs.LoadChallenges();
	}
	
	public void RemoveExpiredWeeklyChallenges()
	{
		weeklyObjs.RemoveExpiredChallenges();
	}
	
	public List<ObjectiveProtoData> SortGridItemsByPriority(List<ObjectiveProtoData> list)
	{
		notify.Debug("SortGridItemsByPriority called in UIObjectivesList");			
		
		//List<ObjectiveProtoData> listToSort = unsortedList.ToList();
		//listToSort = listToSort.OrderBy(x => x._difficulty).ToList(); 
		//listToSort.Sort((x,y) => x._difficulty.CompareTo(y._difficulty));
		//listToSort.Sort((ObjectiveProtoData data1, ObjectiveProtoData data2) => { return data1._difficulty.CompareTo(data2._difficulty); } );
   		//listToSort = listToSort.OrderBy(data => data._difficulty).ToList();
  		//pG.aL = pG.aL.OrderBy(a => a.number).ToList();
		
		list.Sort((a1, a2) => a1._difficulty.CompareTo(a2._difficulty));
		return list;	//listToSort;
	}		

	public void BackUpWeeklyChallengeProgressForAnimationsNextTime()
	{
		PreviousWeeklyObjectiveStats = new Dictionary<int,int>();
		
		foreach (ObjectiveProtoData data in Services.Get<ObjectivesManager>().GetWeeklyObjectives())
		{
			PreviousWeeklyObjectiveStats.Add(data._id, data._conditionList[0]._earnedStatValue);
		}
	}
}



	//}
	
	//void Start()
	//{

				//if (disregardDifficulty)	// bypass difficulty for alpha
					//	list.Add(ob);
					//else 


	
//	public static List<ObjectiveProtoData> GetListOfAvailableObjectivesDisregardingDifficulty()
//	{
//		List<ObjectiveProtoData> list = new List<ObjectiveProtoData>();
//		
//		foreach (ObjectiveProtoData ob in ObjectivesManager.Objectives)
//		{
//			if (!GameProfile.SharedInstance.Player.objectivesEarned.Contains(ob._id) &&	// exclude completed objectives	
//				!GameProfile.SharedInstance.Player.objectivesActive.Contains(ob))		// exclude active objectives	
//			{
//				int obCat = (int)ob._category;											// convert enum to int
//				int cat1 = (objective1 != null) ? (int)objective1._category : -1;		// check if nulls passed into function. 
//				int cat2 = (objective2 != null) ? (int)objective2._category : -1;		// (will occur on first launch when no objectives chosen yet)
//				int cat3 = (objective3 != null) ? (int)objective3._category : -1;			
//				
//				if (obCat != cat1 && obCat != cat2 && obCat != cat3)
//				{
//					if (ob._difficulty == diff)
//						list.Add(ob);
//				}
//			}
//		}	
//	}


		
		//if (list.Count == 0 && (int)diff > 1)	//0)	// no objectives found at this difficulty level and not in categories already active
//		{
//			Debug.LogWarning("Failure at difficulty = " + diff.ToString());
//			
//			diff--;
//			list = GetObjectivesByDifficultyWithThreeCategoriesRemoved(diff, objective1, objective2, objective3);	// try again recursively
//		}
//		else
//			Debug.LogWarning("Success at difficulty = " + diff.ToString());
//		
//		return list;


//
//	
//	static public void AddObjective(ObjectiveProtoData data) 
//	{
//		if (ObjectivesManager.Objectives == null) { return; }
//		ObjectivesManager.Objectives.Add(data);
//	}
//	
//	static public void RemoveObjective(ObjectiveProtoData data) 
//	{
//		if (ObjectivesManager.Objectives == null) { return; }
//		ObjectivesManager.Objectives.Remove(data);
//	}
//
//	static public void AddLegendaryObjective(ObjectiveProtoData data) 
//	{
//		if (ObjectivesManager.LegendaryObjectives == null) { return; }
//		ObjectivesManager.LegendaryObjectives.Add(data);
//	}
//	
//	static public void RemoveLegendaryObjective(ObjectiveProtoData data) 
//	{
//		if (ObjectivesManager.LegendaryObjectives == null) { return; }
//		ObjectivesManager.LegendaryObjectives.Remove(data);
//	}	

		
		//if (Objectives == null) { Objectives = new List<ObjectiveProtoData>(); }
		//else { Objectives.Clear(); }
		
		

	//private GameObject weeklyObjsGO;

	//private List<ObjectiveProtoData> weeklyObjectives;
	//public List<ObjectiveProtoData> GetWeeklyObjectives() { return weeklyObjectives; } 		
		
		//ObjectiveLevelManager.LoadFile();
		
		//weeklyObjsGO = new GameObject("WeeklyObjectives");
		//weeklyObjsGO.transform.parent = transform;
		//weeklyObjs = weeklyObjsGO.AddComponent<WeeklyObjectives>();	
		//weeklyObjectives = weeklyObjs.GetWeeklyObjectives();

//	public static List<ObjectiveProtoData> GetObjectivesByCategory(ObjectiveCategory category)
//	{
//		List<ObjectiveProtoData> list = new List<ObjectiveProtoData>();
//		
//		foreach (ObjectiveProtoData ob in ObjectivesManager.Objectives)
//		{
//			if (ob._category == category)
//			{
//				list.Add(ob);
//			}
//		}
//		
//		return list;
//	}
//	
//	public static List<ObjectiveProtoData> GetObjectivesByDifficulty(ObjectiveDifficulty difficulty)
//	{
//		List<ObjectiveProtoData> list = new List<ObjectiveProtoData>();
//		
//		foreach (ObjectiveProtoData ob in ObjectivesManager.Objectives)
//		{
//			if (ob._difficulty == difficulty)
//			{
//				list.Add(ob);
//			}
//		}
//		
//		return list;
//	}