using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class ObjectiveProtoData
{
	public string _title  = null;					// flavored title text, which is actually displayed
	public string _internalTitle  = null;			// internal title, not displayed
	public string _iconNamePreEarned  = null;
	public string _iconNameEarned = null;
	public string _descriptionPreEarned = null;		// flavored description, which is actually displayed
	public string _descriptionEarned = null;		// internal description, not displayed
	public int _pointValue = 0;
	public bool _hidden = false;
	public bool _canEarnMoreThanOnce = true;
	public int _id = -1;
	
	public ObjectiveCategory _category = ObjectiveCategory.Coin;
	public ObjectiveDifficulty _difficulty = ObjectiveDifficulty.Difficulty1;	

	public List<ConditionProtoData> _conditionList = new List<ConditionProtoData>();
	
	//public int _rewardType = 1;
	public RankRewardType _rewardType = RankRewardType.Coins;
	
	public bool _guildChallenge = false;
	public int _rewardValue = 0;
	
	public int _environmentID = -1;
	
	public DateTime _endDate;
	public DateTime _startDate;
	
	/*
	public ObjectiveType _type = ObjectiveType.Distance;
	public ObjectiveTimeType _timeType = ObjectiveTimeType.PerRun;
	public ObjectiveFilterType _filterType = ObjectiveFilterType.None;
	public int _statValue = -1;
	public int _earnedStatValue = 0;
	*/
	
#if UNITY_EDITOR
	public bool _showFoldOut = false;
#endif
	
//	public ObjectiveProtoData(string title) 
//	{
//		_title = title;
//#if UNITY_EDITOR
//		_showFoldOut = true;
//#endif
//	}
	

	
	public string ToJson( bool serializeNeighborProgress = false ) 
	{ 
		return MiniJSON.Json.Serialize( this.ToDict( serializeNeighborProgress ) );
	}
	
	public Dictionary<string, object> ToDict( bool serializeNeighborProgress = false ) 
	{
		Dictionary<string, object> d = new Dictionary<string, object>();
		d.Add ("Title", _title);
		d.Add ("InternalTitle", _internalTitle);		
		d.Add ("IconNamePre", _iconNamePreEarned);
		d.Add ("IconName", _iconNameEarned);
		d.Add ("DescriptionPre", _descriptionPreEarned);
		d.Add ("Description", _descriptionEarned);
		d.Add ("Points", _pointValue);
		d.Add ("Hidden", _hidden);
		d.Add ("CanEarnMoreThanOnce", _canEarnMoreThanOnce);
		d.Add ("EnvironmentID", _environmentID);

//		d.Add ("T", _type.ToString());
//		d.Add ("TT", _timeType.ToString());
//		d.Add ("FT", _filterType.ToString());
//		d.Add ("SV", _statValue);
//		d.Add ("EarnedSV", _earnedStatValue);		
		
		List<object> conditionList = new List<object>();
		if(_conditionList!=null)
		{
			foreach (ConditionProtoData condition in _conditionList)
			{
				if(condition!=null)
					conditionList.Add(condition.ToDict());	
			}
		}
		
		d.Add ("conditions", conditionList);
		d.Add ("Category", _category);
		d.Add ("Difficulty", _difficulty);
		//Added reward type, reward value, and guild challenge to reflect web.
		d.Add ("rewardType", _rewardType);
		d.Add ("rewardValue", _rewardValue);
		d.Add ("guildChallenge", _guildChallenge);
		d.Add ("endDate", _endDate);
		d.Add("startDate", _startDate);
		
		d.Add ("PID", _id);
		return d;
	}
	
	public Dictionary<string, object> ToWebObjectiveDict( bool serializeNeighborProgress = false ) 
	{
		Dictionary<string, object> d = new Dictionary<string, object>();
		d.Add ("Title", _title);
		d.Add ("InternalTitle", _internalTitle);		
		d.Add ("IconNamePre", _iconNamePreEarned);
		d.Add ("IconName", _iconNameEarned);
		d.Add ("DescriptionPre", _descriptionPreEarned);
		d.Add ("Description", _descriptionEarned);
		d.Add ("Points", _pointValue);
		d.Add ("Hidden", _hidden);
		d.Add ("CanEarnMoreThanOnce", _canEarnMoreThanOnce);
		d.Add ("EnvironmentID", _environmentID);

//		d.Add ("T", _type.ToString());
//		d.Add ("TT", _timeType.ToString());
//		d.Add ("FT", _filterType.ToString());
//		d.Add ("SV", _statValue);
//		d.Add ("EarnedSV", _earnedStatValue);		
		
		List<object> conditionList = new List<object>();
		foreach (ConditionProtoData condition in _conditionList)
		{
			conditionList.Add(condition.ToWebCondDict( serializeNeighborProgress ));	
		}
		
		d.Add ("conditions", conditionList);
		d.Add ("Category", _category);
		d.Add ("Difficulty", _difficulty);
		//Added reward type, reward value, and guild challenge to reflect web.
		d.Add ("rewardType", _rewardType);
		d.Add ("rewardValue", _rewardValue);
		d.Add ("guildChallenge", _guildChallenge);
		d.Add ("endDate", _endDate);
		d.Add ("startDate", _startDate);
		
		d.Add ("PID", _id);
		return d;
	}
	
	
	// wxj
	public ObjectiveProtoData(int id)
	{
		_id = id;
	}
	
	public ObjectiveProtoData(Dictionary<string, object> dict) 
	{
#if UNITY_EDITOR
		_showFoldOut = false;
#endif
		
		_title = "Empty";
		if (dict.ContainsKey("Title"))
			_title = (string)dict["Title"];
		else if (dict.ContainsKey("challengeTitle"))
			_title = (string)dict["challengeTitle"];
		
		_internalTitle = "Empty";
		if (dict.ContainsKey("InternalTitle"))
			_internalTitle = (string)dict["InternalTitle"];
		else if (dict.ContainsKey("Title"))
			_internalTitle = (string)dict["Title"];		// fallback if no internal title found
		else if (dict.ContainsKey("challengeTitle"))
			_internalTitle = (string)dict["challengeTitle"];
		
		_iconNameEarned = null;
		if (dict.ContainsKey("IconName"))
			_iconNameEarned = (string)dict["IconName"];
		
		_iconNamePreEarned = null;
		if (dict.ContainsKey("IconNamePre"))
			_iconNamePreEarned = (string)dict["IconNamePre"];
		
		_descriptionEarned = null;
		if (dict.ContainsKey("Description"))
			_descriptionEarned = (string)dict["Description"];
		//else if (dict.ContainsKey("challengeBody"))
		//	_descriptionEarned = (string)dict["challengeBody"];	
		
		_descriptionPreEarned = null;
		if (dict.ContainsKey("DescriptionPre"))
			_descriptionPreEarned = (string)dict["DescriptionPre"];
		else if (dict.ContainsKey("challengeBody"))
			_descriptionPreEarned = (string)dict["challengeBody"];
		
		_pointValue = 0;
		if (dict.ContainsKey("Points"))
			_pointValue = (int)Math.Abs((long)dict["Points"]);
		
		_environmentID = -1;
		if (dict.ContainsKey("EnvironmentID"))
			_environmentID = (int)((long)dict["EnvironmentID"]);
		
		_hidden = false;
		if (dict.ContainsKey("Hidden"))
			_hidden = (bool)dict["Hidden"];
		
		_canEarnMoreThanOnce = false;
		if (dict.ContainsKey("CanEarnMoreThanOnce"))
			_canEarnMoreThanOnce = (bool)dict["CanEarnMoreThanOnce"];
		
		_id = -1;
		if (dict.ContainsKey("PID"))
			_id = JSONTools.ReadInt(dict["PID"]);
		else if (dict.ContainsKey("challengeIndex"))
			_id = JSONTools.ReadInt(dict["challengeIndex"]);
		
		
		_category = ObjectiveCategory.Coin;
		if (dict.ContainsKey("Category"))
		{
			if (Enum.IsDefined(typeof(ObjectiveCategory), dict["Category"]))
				_category = (ObjectiveCategory)Enum.Parse(typeof(ObjectiveCategory), (string)dict["Category"]);
		}

		_conditionList = new List<ConditionProtoData>();
		if (dict.ContainsKey("conditions"))
		{
			List<object> conditionObjList = dict["conditions"] as List<object>;
		
			if (conditionObjList != null)
			{
				//_conditionList = new List<ConditionProtoData>();
				_conditionList.Clear();
				
				//int totalEarnedSv = 0;
				//int totalSv = -1;
				
				int i = 0;
				foreach (object condition in conditionObjList)
				{
					Dictionary<string,object> condDict = condition as Dictionary<string,object>;
					ConditionProtoData tempCondData = new ConditionProtoData(condDict);
					
					if (i == 0 && condDict.ContainsKey("conditionType"))
					{
						switch (int.Parse(condDict["conditionType"].ToString()))
						{
							case 1:
								_category = ObjectiveCategory.Distance;
								break;
							case 2:
								_category = ObjectiveCategory.Coin;
								break;
							case 3:
								_category = ObjectiveCategory.Collection;
								break;
							case 4:
								_category = ObjectiveCategory.Score;
								break;
							default:
								_category = ObjectiveCategory.Coin;
								break;
						}
					}
					
					//Read in "region" to _environmentID, since the server saved them differently
					if(i==0 && condDict.ContainsKey("region"))
					{
						switch (JSONTools.ReadInt(condDict["region"]))
						{
							case 1:
								_environmentID = -1;//ConditionRegionType.Any;
								break;
								
							case 2:
								_environmentID = 1;//ConditionRegionType.WhimsieWoods;
								break;
								
							case 3:
								_environmentID = 3;//ConditionRegionType.YellowBrickRoad;
								break;
								
							case 4:
								_environmentID = 2;//ConditionRegionType.DarkForest;
								break;
								
							default:
								_environmentID = JSONTools.ReadInt(condDict["region"]) - 1;
								break;
						}
					}
					//totalEarnedSv += tempCondData._earnedStatValue;
					//totalSv += tempCondData._statValue;
					
					_conditionList.Add(tempCondData);
					i++;
				}
				//_earnedStatValue = totalEarnedSv;
				//_statValue = totalSv;
			} 
		}
		#if UNITY_EDITOR
		else { _conditionList.Add(new ConditionProtoData()); }	// if in the editor, need to give the new objective a single condition to fill values of
		#endif				
		
//		else
//		{
//			//_conditionList = new List<ConditionProtoData>();
//			ConditionProtoData tempCond = new ConditionProtoData();
//			
//			tempCond._type = ObjectiveType.Distance;
//			if (dict.ContainsKey("T")) 
//			{
//				if (Enum.IsDefined(typeof(ObjectiveType), dict["T"])) 
//				{
//					tempCond._type = (ObjectiveType)Enum.Parse(typeof(ObjectiveType), (string)dict["T"]);
//				}
//			}
//
//			tempCond._timeType = ObjectiveTimeType.PerRun;
//			if (dict.ContainsKey("TT")) 
//			{
//				if(Enum.IsDefined(typeof(ObjectiveTimeType), dict["TT"])) 
//				{
//					tempCond._timeType = (ObjectiveTimeType)Enum.Parse(typeof(ObjectiveTimeType), (string)dict["TT"]);
//				}
//			}
//			
//			tempCond._filterType = ObjectiveFilterType.None;
//			if (dict.ContainsKey("FT"))
//			{
//				if(Enum.IsDefined(typeof(ObjectiveFilterType), dict["FT"]))
//				{
//					tempCond._filterType = (ObjectiveFilterType)Enum.Parse(typeof(ObjectiveFilterType), (string)dict["FT"]);
//				}
//			}
//			
//			tempCond._earnedStatValue = 0;
//			if (dict.ContainsKey("EarnedSV"))
//			{
//				tempCond._earnedStatValue = JSONTools.ReadInt(dict["EarnedSV"]);	
//			}
//			
//			tempCond._statValue = -1;
//			if (dict.ContainsKey("SV"))
//			{
//				tempCond._statValue = JSONTools.ReadInt(dict["SV"]);	
//			}
//			
//			_conditionList.Add(tempCond);
//		}


		_difficulty = ObjectiveDifficulty.Difficulty1;
		if (dict.ContainsKey("Difficulty"))
		{
			if (Enum.IsDefined(typeof(ObjectiveDifficulty), dict["Difficulty"]))
				_difficulty = (ObjectiveDifficulty)Enum.Parse(typeof(ObjectiveDifficulty), (string)dict["Difficulty"]);
		}
		else if (dict.ContainsKey("diff"))
		{
			switch (int.Parse(dict["diff"].ToString()))
			{
				case 2:
					_difficulty = ObjectiveDifficulty.Difficulty4;
					break;
				case 3:
					_difficulty = ObjectiveDifficulty.Difficulty7;
					break;
				default:
					_difficulty = ObjectiveDifficulty.Difficulty1;
					break;
			}
		}
		
		if (dict.ContainsKey("rewardType")) 
		{
//			_rewardType = JSONTools.ReadInt(dict["rewardType"]); 
			if (dict["rewardType"].GetType() == typeof(Int64)
				|| dict["rewardType"].GetType() == typeof(int)
			) {
				switch (JSONTools.ReadInt(dict["rewardType"]))
				{
				case 1:
					_rewardType = RankRewardType.Multipliers;
					break;
				case 2:
					_rewardType = RankRewardType.Coins;
					break;
				case 3:
					_rewardType = RankRewardType.Gems;
					break;
				default:
					_rewardType = RankRewardType.Coins;
					break;
				}
			} 
			else if (Enum.IsDefined(typeof(RankRewardType), dict["rewardType"])) 
				_rewardType = (RankRewardType)Enum.Parse(typeof(RankRewardType), (string)dict["rewardType"]);
		}

		/* wsl 08-07
		if (dict.ContainsKey("rewardValue"))
			_rewardValue = JSONTools.ReadInt(dict["rewardValue"]);
		
		if (dict.ContainsKey("guildChallenge"))
			_guildChallenge = (bool) dict["guildChallenge"];
		
		if (dict.ContainsKey("endDate"))
			_endDate = DateTime.Parse(dict["endDate"].ToString());
		
		if (dict.ContainsKey("startDate"))
			_startDate = DateTime.Parse(dict["startDate"].ToString());
			*/
	}
}
		

		
		/*
		if (dict.ContainsKey("SV")) { _statValue = JSONTools.ReadInt(dict["SV"]); }
		
		if (dict.ContainsKey("EarnedSV")) { _earnedStatValue = JSONTools.ReadInt(dict["EarnedSV"]); }
		
		_type = ObjectiveType.Distance;
		if (dict.ContainsKey("T")) 
		{
			if (Enum.IsDefined(typeof(ObjectiveType), dict["T"])) 
			{
				_type = (ObjectiveType)Enum.Parse(typeof(ObjectiveType), (string)dict["T"]);
			}
		}
		
		_timeType = ObjectiveTimeType.PerRun;
		if (dict.ContainsKey("TT")) 
		{
			if(Enum.IsDefined(typeof(ObjectiveTimeType), dict["TT"])) 
			{
				_timeType = (ObjectiveTimeType)Enum.Parse(typeof(ObjectiveTimeType), (string)dict["TT"]);
			}
		}
		
		_filterType = ObjectiveFilterType.None;
		if (dict.ContainsKey("FT"))
		{
			if(Enum.IsDefined(typeof(ObjectiveFilterType), dict["FT"]))
			{
				_filterType = (ObjectiveFilterType)Enum.Parse(typeof(ObjectiveFilterType), (string)dict["FT"]);
			}
		}
		*/