using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class UserProtoData
{
	public string _dbId = "";
	public string _gcId = "";
	public string _fbId = "";
	public string _deviceId = "";
	public string _name = "";
	
	public int _totalScore = 0;
	public int _totalMeters = 0;
	public int _totalCoins = 0;
	public int _totalGems = 0;
	
	public int _bestScore = 0;
	public int _bestMeters = 0;
	public int _bestCoins = 0;
	public int _bestGems = 0;
	public int _rank = 1;
	public int _currentCoins = 0;
	public int _currentGems = 0;
	public int _abilitiesUsed = 0;
	
	
	public List<int> _artifactLevels = new List<int>();
	public List<int> _artifactsGemmed = new List<int>();
	public List<int> _artifactsDiscovered = new List<int>();
	
	public List<int> _objectivesEarned = new List<int>();
	public List<int> _legendaryObjectivesEarned = new List<int>();
	public List<int> _environmentsVisited = new List<int>();
	
	public List<int> _powersPurchased = new List<int>();
	public List<int> _powersGemmed = new List<int>();
	
	public List<int> _consumablesQuantity = new List<int>();
	
	public int AbilityTutorialPlayed = 0;
	public int UtilityTutorialPlayed = 0;
	public int GatchaTutorial = 0;
	
	public List<int> _earnedTeamObjectives = new List<int>();
	
	
	//public Dictionary<int, float[]> _lifetimeStats = new Dictionary<int, float[]>();
	
	public Dictionary<int, LifetimeStatEnvironmentProtoData> _lifetimeStats 
		= new Dictionary<int, LifetimeStatEnvironmentProtoData>();
	
	public Dictionary<int, float> _legendaryProgress 
		= new Dictionary<int, float>();
	
	public int _numberResurectsUsed = 0;
	
	public int _numberOfPlays = 0;
	
	public bool _coinRedeemFB = false;
	
	public List<int> _objectivesUnclaimed = new List<int>();
	
	public List<int> _characterUnlock = new List<int>();
	
#pragma warning disable 414
	public Notify notify;
#pragma warning restore 414

	public List<string> VersionGemRedeemList = new List<string>();
	
	public List<GuildChallengeProtoData> _guildChallengeList = new List<GuildChallengeProtoData>();
	
	public List<NeighborProtoData> _neighborList = new List<NeighborProtoData>();

	private Dictionary<string, int> passedNeighborDict = new Dictionary<string, int>();

	
	public UserProtoData (Dictionary<string,object> dict)
	{
		notify = new Notify("UserProtoData");
		
		notify.Debug("Dictionary passed: " + MiniJSON.Json.Serialize(dict));
		
		if (dict.ContainsKey("_id"))         { _dbId = (string) dict["_id"]; }
		if (dict.ContainsKey("_gcId"))       { _gcId = (string) dict["_gcId"]; }
		else if (dict.ContainsKey("gcId"))   { _gcId = (string) dict["gcId"]; }
		
		if (dict.ContainsKey("_fbId"))       { _fbId = (string) dict["_fbId"]; }
		else if (dict.ContainsKey("fbId"))   { _fbId = (string) dict["fbId"]; }
		
		if (dict.ContainsKey("deviceId"))    { _deviceId = (string) dict["deviceId"]; }
		
		if (dict.ContainsKey("name"))         { _name = (string) dict["name"]; }
		
		if (dict.ContainsKey("totalScore"))  { _totalScore = int.Parse(dict["totalScore"].ToString()); }
		if (dict.ContainsKey("totalMeters")) { _totalMeters = int.Parse(dict["totalMeters"].ToString()); }
		if (dict.ContainsKey("totalCoins"))  { _totalCoins = int.Parse(dict["totalCoins"].ToString()); }
		if (dict.ContainsKey("totalGems"))   { _totalGems = int.Parse(dict["totalGems"].ToString()); }
		
		if (dict.ContainsKey("bestScore"))   { _bestScore = int.Parse(dict["bestScore"].ToString()); }
		if (dict.ContainsKey("bestMeters"))  { _bestMeters = int.Parse(dict["bestMeters"].ToString()); }
		if (dict.ContainsKey("bestCoins"))   { _bestCoins = int.Parse(dict["bestCoins"].ToString()); }
		if (dict.ContainsKey("bestGems"))    { _bestGems = int.Parse(dict["bestGems"].ToString()); }
		if (dict.ContainsKey("currCoins"))   { _currentCoins = int.Parse(dict["currCoins"].ToString()); }
		if (dict.ContainsKey("currGems"))    { _currentGems = int.Parse(dict["currGems"].ToString()); }
		
		if (dict.ContainsKey("abilityUse"))  { _abilitiesUsed = int.Parse(dict["abilityUse"].ToString()); }
		
		if (dict.ContainsKey("numPlay")) 	 { _numberOfPlays = int.Parse(dict["numPlay"].ToString()); }
		
		if (dict.ContainsKey("fbRedeem")) 	 { _coinRedeemFB = bool.Parse(dict["fbRedeem"].ToString()); }
		
		if(dict.ContainsKey("lt")) {
			if (_lifetimeStats == null)
			{
				_lifetimeStats = new Dictionary<int, LifetimeStatEnvironmentProtoData>();
			}
			_lifetimeStats.Clear();
			
			IDictionary tempDict = dict["lt"] as IDictionary;
			
			if(tempDict != null) {
				foreach (var item in tempDict)
				{
					if(item != null)
					{
						DictionaryEntry kvp = (DictionaryEntry)item;
						
						int env = int.Parse((string)kvp.Key);///NOTE: for some reason, JSONTools.ParseInt() wasnt working here...
						Dictionary<string, object> ltDict = kvp.Value as Dictionary<string, object>;

						if (!_lifetimeStats.ContainsKey(env))
						{
							_lifetimeStats.Add(env, new LifetimeStatEnvironmentProtoData(ltDict));
						}
					}
				}	
			}
		}
			
		if (dict.ContainsKey("legProg")) {
			_legendaryProgress = new Dictionary<int,float>();
			_legendaryProgress.Clear();
			
			IDictionary tempDict = dict["legProg"] as IDictionary;
			
			if(tempDict != null) {
				foreach (var item in tempDict) {
					if(item != null) {
						DictionaryEntry kvp = (DictionaryEntry)item;
					
						int k = int.Parse((string)kvp.Key);
						float v = (float)JSONTools.ReadDouble(kvp.Value);
						
						_legendaryProgress.Add(k,v);
					}
				}
			}
		}
		
		if (dict.ContainsKey("artifactLevel"))
		{
			List <object> tempObjList = dict["artifactLevel"] as List<object>;
			
			if (tempObjList != null)
			{
				_artifactLevels.Clear();
				foreach(object artifactObject in tempObjList)
				{
					_artifactLevels.Add(int.Parse(artifactObject.ToString()));
				}
			}
		}
		
		if (dict.ContainsKey("artGem"))
		{
			List<object> tempObjList = dict["artGem"] as List<object>;
			
			if (tempObjList != null)
			{
				_artifactsGemmed.Clear();
				foreach(object artifactObject in tempObjList)
				{
					_artifactsGemmed.Add(int.Parse(artifactObject.ToString()));
				}
			}
		}
		
		if ( dict.ContainsKey( "earnTeam" ) )
		{
			List<object> tempObjList = dict["earnTeam"] as List<object>;
			
			if ( tempObjList != null )
			{
				_earnedTeamObjectives.Clear();
				foreach( object earnTeamObject in tempObjList )
				{
					_earnedTeamObjectives.Add( int.Parse( earnTeamObject.ToString() ) );
				}
			}
		}
		
		if (dict.ContainsKey("artDisc"))
		{
			List<object> tempObjList = dict["artDisc"] as List<object>;
			
			if (tempObjList != null)
			{
				_artifactsDiscovered.Clear();
				foreach(object artifactObject in tempObjList)
				{
					_artifactsDiscovered.Add(int.Parse (artifactObject.ToString()));
				}
			}
		}
		
		if (dict.ContainsKey("powGem"))
		{
			List<object> tempObjList = dict["powGem"] as List<object>;
			
			if (tempObjList != null)
			{
				_powersGemmed.Clear();
				foreach(object powerObject in tempObjList)
				{
					_powersGemmed.Add(int.Parse(powerObject.ToString()));
				}
			}
		}
		
		if (dict.ContainsKey("powerPurchase"))
		{
			List<object> tempObjList = dict["powerPurchase"] as List<object>;
			
			if (tempObjList != null)
			{
				_powersPurchased.Clear();
				foreach (object powerObj in tempObjList)
				{
					_powersPurchased.Add(int.Parse(powerObj.ToString()));
				}
			}
		}
		
		if (dict.ContainsKey("rezUsed")) { _numberResurectsUsed = int.Parse(dict["rezUsed"].ToString()); }
		
		if (dict.ContainsKey("consumableQty"))
		{
			List<object> tempObjList = dict["consumableQty"] as List<object>;
			
			if (tempObjList != null)
			{
				_consumablesQuantity.Clear();
				foreach (object consumableObj in tempObjList)
				{
					_consumablesQuantity.Add(int.Parse(consumableObj.ToString()));
				}
			}
		}
		
		if (dict.ContainsKey("rank")) 		 { _rank = int.Parse(dict["rank"].ToString()); }
		
		if (dict.ContainsKey("objEarned")) {
			List<object> tempObjList = dict["objEarned"] as List<object>;
			
			if (tempObjList != null) {
				_objectivesEarned.Clear();
				
				foreach (object objectiveEarned in tempObjList)
				{
					_objectivesEarned.Add(int.Parse(objectiveEarned.ToString()));
				}
			}
		}
		
		if (dict.ContainsKey("legObjEarned")) 
		{
			List<object> tempObjList = dict["legObjEarned"] as List<object>;
			
			foreach (object legObjEarned in tempObjList)
			{
				_legendaryObjectivesEarned.Add(int.Parse(legObjEarned.ToString()));
			}
		}
		
			
		notify.Debug("Best Coins: " + _bestCoins);
		
		if (dict.ContainsKey("guildChallenge"))
		{
			List<object> challengeObjList = dict["guildChallenge"] as List<object>;
			
			if (challengeObjList != null)
			{
				_guildChallengeList.Clear();
				
				foreach (object challengeObj in challengeObjList)
				{
					Dictionary<string, object> challengeDict = challengeObj as Dictionary<string,object>;
					GuildChallengeProtoData tempChallengeData = new GuildChallengeProtoData(challengeDict);
					
					_guildChallengeList.Add(tempChallengeData);
				}
			}
		}
		
		if ( dict.ContainsKey( "objUncList" ) )
		{
			List<object> objectivesUnclaimed = dict["objUncList"] as List<object>;
			
			if ( objectivesUnclaimed != null )
			{
				_objectivesUnclaimed.Clear();
				
				foreach (object unclaimedObjective in objectivesUnclaimed )
				{
					_objectivesUnclaimed.Add( int.Parse( unclaimedObjective.ToString() ) );
				}
			}
		}
		
		if ( dict.ContainsKey( "envVisited" ) )
		{
			List<object> environmentsVisited = dict["envVisited"] as List<object>;
			
			if ( environmentsVisited != null )
			{
				_environmentsVisited.Clear();
				
				foreach (object environmentVisited in environmentsVisited )
				{
					_environmentsVisited.Add( int.Parse( environmentVisited.ToString() ) );
				}
			}
		}		
		
		if ( dict.ContainsKey( "charUnlockList" ) )
		{
			List<object> charactersUnlockedList = dict["charUnlockList"] as List<object>;
			
			if ( charactersUnlockedList != null )
			{
				_characterUnlock.Clear();
				
				foreach (object unlockedCharacter in charactersUnlockedList )
				{
					_characterUnlock.Add( int.Parse( unlockedCharacter.ToString() ) );
				}
			}
		}
		
		if ( dict.ContainsKey( "vGemRedeem" ) )
		{
			List<object> versionGemRedeemList = dict["vGemRedeem"] as List<object>;
			
			if ( VersionGemRedeemList != null )
			{
				VersionGemRedeemList.Clear();
				
				foreach ( object versionNumber in versionGemRedeemList )
				{
					VersionGemRedeemList.Add( (string) versionNumber );
				}
			}
		}
		
		if ( dict.ContainsKey( "abTut" ) ) 
		{
			AbilityTutorialPlayed = int.Parse( dict["abTut"].ToString() );
		}
		
		if ( dict.ContainsKey( "utTut" ) )
		{
			UtilityTutorialPlayed = int.Parse( dict["utTut"].ToString() );
		}
		
		if ( dict.ContainsKey( "gatTut" ) )
		{
			GatchaTutorial = int.Parse( dict["gatTut"].ToString() );
		}
		
		//load passed neighbor Dict
		LoadPassedNeighbors();
		
		if (dict.ContainsKey("neighbors"))
		{
			List<object> neighborObjList = dict["neighbors"] as List<object>;
			
			if (neighborObjList != null)
			{
				_neighborList.Clear();
				
				foreach (object neighborObj in neighborObjList)
				{
					Dictionary<string, object> neighborDict = neighborObj as Dictionary<string, object>;
					NeighborProtoData tempNeighborData = new NeighborProtoData(neighborDict);
					
					//If the temp neighbor object has better distance than the stored distance for
					//	that neighbor, remove it from
					if (passedNeighborDict.ContainsKey(tempNeighborData._dbId)
						&& (tempNeighborData._bestMeters > (int) passedNeighborDict[tempNeighborData._dbId])
					) {
						passedNeighborDict.Remove(tempNeighborData._dbId);
					}
					
					//If neighbors best meters is 0 and not currently stored, store in passed neighbors list.
					if (tempNeighborData._bestMeters == 0 
						&& !passedNeighborDict.ContainsKey(tempNeighborData._dbId)
					) {

						passedNeighborDict.Add(tempNeighborData._dbId, tempNeighborData._bestMeters);
					}

					_neighborList.Add(tempNeighborData);
					
					//If temp neighbor object is in the stored list, set its passed status to true.
					if (passedNeighborDict.ContainsKey(tempNeighborData._dbId)) 
					{
						tempNeighborData._passedNeighbor = true;
					}
				}
			}
		}
		SavePassedNeighbors();
	}
	
	public void SavePassedNeighbors()
	{
		/*
		using (MemoryStream stream = new MemoryStream())
		{
			string fileName = Application.persistentDataPath + Path.DirectorySeparatorChar +
				"passedneighbor.txt";
			
			string passedNeighborString = MiniJSON.Json.Serialize(passedNeighborDict);
			
			try 
			{
				using (StreamWriter fileWriter = File.CreateText(fileName))
				{
					fileWriter.WriteLine(passedNeighborString);
					fileWriter.Close();
				}
			} 
			catch (System.Exception ex)
			{
				notify.Warning("Passed Neighbors Save Exception: " + ex.Message);
			}
		}
		*/
	}
	
	
	public void AddPassedNeighbor(string _id, int distance)
	{
		passedNeighborDict.Add(_id, distance);
		SavePassedNeighbors();
	}
	
	public void LoadPassedNeighbors()
	{
		/*
		string fileName = Application.persistentDataPath + Path.DirectorySeparatorChar +
			"passedneighbor.txt";
		
		if (File.Exists(fileName) == false)
		{
			notify.Warning("No passedneighbor.txt file exists");
		}
		else 
		{
			StreamReader reader = File.OpenText(fileName);
			string jsonString = reader.ReadToEnd();
			reader.Close();
									
			Dictionary<string, object> loadedDict = MiniJSON.Json.Deserialize(jsonString) as Dictionary<string, object>;
						
			if (loadedDict != null) {
				foreach (KeyValuePair<string, object> keyValuePair in loadedDict)
				{
					if (!passedNeighborDict.ContainsKey(keyValuePair.Key)) {
						passedNeighborDict.Add(keyValuePair.Key, int.Parse(keyValuePair.Value.ToString()));
					}
				}
			}	
		}
		*/
	}
	
	public string ToJson() { return MiniJSON.Json.Serialize(this.ToDict()); }
	
	public Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> d = new Dictionary<string, object>();
		
		d.Add("_id", _dbId);
		d.Add("gcId", _gcId);
		d.Add("fbId", _fbId);
		d.Add("deviceId", _deviceId);
		d.Add("name", _name);
		

		d.Add("totalMeters", _totalMeters);
		d.Add("totalCoins", _totalCoins);
		d.Add("totalGems", _totalGems);

		d.Add("bestScore", _bestScore);
		d.Add("bestMeters", _bestMeters);
		d.Add("bestCoins", _bestCoins);
		d.Add("bestGems", _bestGems);
		d.Add("rank", _rank);
		d.Add("objEarned", _objectivesEarned);
		d.Add("legObjEarned", _legendaryObjectivesEarned);
		d.Add("envVisited", _environmentsVisited);
		d.Add("currCoins", _currentCoins);
		d.Add("currGems", _currentGems);
		d.Add("artifactLevel", _artifactLevels);
		d.Add("powerPurchase", _powersPurchased);
		d.Add("consumableQty", _consumablesQuantity);
		d.Add("abilityUse", _abilitiesUsed);
		d.Add("fbRedeem", _coinRedeemFB);
		
		List<object> tempChallengeList = new List<object>();
		foreach (GuildChallengeProtoData challenge in _guildChallengeList)
		{
			tempChallengeList.Add(challenge.ToDict());
		}
		d.Add("guildChallenge", tempChallengeList);

		List<object> tempNeighborList = new List<object>();
		foreach (NeighborProtoData neighborObj in _neighborList)
		{
			tempNeighborList.Add(neighborObj.ToDict());
		}
		d.Add("neighbors", tempNeighborList);
		
		if (_objectivesEarned != null && _objectivesEarned.Count > 0)
		{
			d.Add("objEarned", _objectivesEarned);
		}
		
		if ( _earnedTeamObjectives != null && _earnedTeamObjectives.Count > 0 )
		{
			d.Add( "earnTeam", _earnedTeamObjectives );
		}
		
		if (_legendaryObjectivesEarned != null && _legendaryObjectivesEarned.Count > 0)
		{
			d.Add("legObjEarned", _legendaryObjectivesEarned);
		}
		
		if (_lifetimeStats != null && _lifetimeStats.Count > 0)
		{

			
			Dictionary<int, object> lifetimeStats = new Dictionary<int, object>();
			
			foreach(int envId in _lifetimeStats.Keys)
			{
				lifetimeStats.Add(envId, _lifetimeStats[envId].ToDict());
			}
			d.Add("lt", lifetimeStats);
		}
		
		if (_legendaryProgress != null && _legendaryProgress.Count > 0)
		{
			d.Add("legProg", _legendaryProgress);
		}
		
		if ( _environmentsVisited != null && _environmentsVisited.Count > 0 )
		{
			d.Add( "envVisited", _environmentsVisited );
		}				
		
		if ( _objectivesUnclaimed != null && _objectivesUnclaimed.Count > 0 )
		{
			d.Add( "objUncList", _objectivesUnclaimed );
		}
		
		if ( _characterUnlock != null && _characterUnlock.Count > 0 )
		{
			d.Add( "charUnlockList", _characterUnlock );
		}
		
		if ( VersionGemRedeemList != null && VersionGemRedeemList.Count > 0 )
		{
			d.Add( "vGemRedeem", VersionGemRedeemList );
		}
		
		d.Add( "abTut", AbilityTutorialPlayed );
		d.Add( "utTut", UtilityTutorialPlayed );
		d.Add( "gatTut", GatchaTutorial );
		
		return d;
	}
	
	public String UserToJson () { return MiniJSON.Json.Serialize(this.UserToDict()); }
		
	// Limit the output to only user information, and do not include neighbor data.
	public Dictionary<string, object> UserToDict()
	{
		Dictionary<string, object> d = new Dictionary<string, object>();
		d.Add("_id", _dbId);
		if (_gcId != "") {
			d.Add("gcId", _gcId);
		}
		if (_fbId != "") {
			d.Add("fbId", _fbId);
		}
		d.Add("deviceId", _deviceId);
		if (_name != "") {
			d.Add("name", _name);
		}
		

		d.Add("totalMeters", _totalMeters);
		d.Add("totalCoins", _totalCoins);
		d.Add("totalGems", _totalGems);

		d.Add("bestScore", _bestScore);
		d.Add("bestMeters", _bestMeters);
		d.Add("bestCoins", _bestCoins);
		d.Add("bestGems", _bestGems);
		d.Add("currCoins", _currentCoins);
		d.Add("currGems", _currentGems);
		d.Add("abilityUse", _abilitiesUsed);
		d.Add("rank", _rank);
		d.Add("fbRedeem", _coinRedeemFB);
		
		d.Add("numPlay", _numberOfPlays);

		if (_artifactLevels != null && _artifactLevels.Count > 0)
		{
			d.Add("artifactLevel", _artifactLevels);
		}
		else
		{
			d.Add("artifactLevel", null);
		}
		
		if (_artifactsGemmed != null && _artifactsGemmed.Count > 0)
		{
			d.Add("artGem", _artifactsGemmed);
		}
		else
		{
			d.Add("artGem", null);
		}
		
		if (_artifactsDiscovered != null && _artifactsDiscovered.Count > 0)
		{
			d.Add("artDisc", _artifactsDiscovered);
		}
		else 
		{
			d.Add("artDisc", null);
		}
		
		if (_powersPurchased != null && _powersPurchased.Count > 0)
		{
			d.Add("powerPurchase", _powersPurchased);
		}
		else
		{
			d.Add("powerPurchase", null);
		}
		
		if (_powersGemmed != null && _powersGemmed.Count > 0)
		{
			d.Add("powGem", _powersGemmed);
		}
		else
		{
			d.Add("powGem", null);
		}
		
		d.Add("rezUsed", _numberResurectsUsed);
		
		if(_consumablesQuantity != null && _consumablesQuantity.Count >0)
		{
			d.Add("consumableQty", _consumablesQuantity);
		}
		else
		{
			d.Add("consumableQty", null);
		}
		
		if (_objectivesEarned != null && _objectivesEarned.Count > 0)
		{
			d.Add("objEarned", _objectivesEarned);
		}
		else
		{
			d.Add("objEarned", null);
		}
		
		if (_legendaryObjectivesEarned != null && _legendaryObjectivesEarned.Count > 0)
		{
			d.Add("legObjEarned", _legendaryObjectivesEarned);
		}
		else 
		{
			d.Add("legObjEarned", null);
		}
		
		if (_lifetimeStats != null && _lifetimeStats.Count > 0) 
		{
			Dictionary<int, object> lifetimeStats = new Dictionary<int, object>();
			foreach(int envid in _lifetimeStats.Keys)
			{
				lifetimeStats.Add(envid, _lifetimeStats[envid].ToDict());
			}
			d.Add("lt", lifetimeStats);
		}
		
		if (_legendaryProgress != null && _legendaryProgress.Count > 0)
		{
			d.Add("legProg", _legendaryProgress);
		}
		
		// If objectives unclaimed's count is zero, still add it to the dictionary.
		if ( _objectivesUnclaimed != null ) // && _objectivesUnclaimed.Count > 0 )
		{
			d.Add( "objUncList", _objectivesUnclaimed );
		}
		
		if ( _environmentsVisited != null ) // && _environmentsVisited.Count > 0 )
		{
			d.Add( "envVisited", _environmentsVisited );
		}		
		
		if ( _characterUnlock != null && _characterUnlock.Count > 0 )
		{
			d.Add( "charUnlockList", _characterUnlock );
		}
		
		if ( VersionGemRedeemList != null && VersionGemRedeemList.Count > 0 )
		{
			d.Add( "vGemRedeem", VersionGemRedeemList );
		}

		
		if ( _earnedTeamObjectives != null && _earnedTeamObjectives.Count > 0 )
		{
			d.Add( "earnTeam", _earnedTeamObjectives );
		}
		
		//Cycle through weekly objectives for guildChallenges
		List<object> tempChallengeList = new List<object>();
		foreach (GuildChallengeProtoData challenge in _guildChallengeList)
		{
			tempChallengeList.Add(challenge.ToChallengeDict());
		}
		d.Add("guildChallenge", tempChallengeList);
		
		d.Add( "abTut", AbilityTutorialPlayed );
		d.Add( "utTut", UtilityTutorialPlayed );
		d.Add( "gatTut", GatchaTutorial );
		
		return d;
	}
}

