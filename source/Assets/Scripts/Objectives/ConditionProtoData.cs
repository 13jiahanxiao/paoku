using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public enum ConditionRegionType {Any, WhimsieWoods, YellowBrickRoad, DarkForest }

public class ConditionProtoData
{
	public ObjectiveType _type = ObjectiveType.Distance;
	
	public ObjectiveTimeType _timeType = ObjectiveTimeType.PerRun;
	
	public ObjectiveFilterType _filterType = ObjectiveFilterType.None;
	
	public ConditionRegionType _regionType = ConditionRegionType.Any;
	
	public int _statValue = -1;
	
	public int _earnedStatValue = 0;

	//Track for conditions
	public int _conditionIndex = 0;
	public int _earnedNeighborValue = 0;
		
	// wxj, activity1,2, record count for finish target in a environment
	public Dictionary<int, float> _actiEarnedStatForEnvs = new Dictionary<int, float>();
	// wxj, activity4, rewarded count
	public int _actiRewardedCount = 0;
		
	public ConditionProtoData()
	{

	}
	
	public ConditionProtoData(Dictionary<string, object> dict) : this()
	{	
		// wxj activity objective fields
		if(dict.ContainsKey("ActiEarnedStatForEnvs")) {
			_actiEarnedStatForEnvs = new Dictionary<int,float>();
			_actiEarnedStatForEnvs.Clear();
			
			IDictionary tempDict = dict["ActiEarnedStatForEnvs"] as IDictionary;
			
			if(tempDict != null) {
				foreach (var item in tempDict) {
					if(item != null) {
						DictionaryEntry kvp = (DictionaryEntry)item;
						int k = int.Parse((string)kvp.Key);
						float v = float.Parse(kvp.Value.ToString());
						_actiEarnedStatForEnvs.Add(k,v);
					}
				}	
			}
		} 
		
		if (dict.ContainsKey("ActiRewardedCount")) {
			_actiRewardedCount = JSONTools.ReadInt(dict["ActiRewardedCount"]);
		}
		
		
		
		
		
		//_statValue = 0;
		if (dict.ContainsKey("SV")) {
			_statValue = JSONTools.ReadInt(dict["SV"]);
		} else if (dict.ContainsKey("conditionValue")) {
			_statValue = JSONTools.ReadInt(dict["conditionValue"]);
		}
		
		//_earnedStatValue = 0;
		if (dict.ContainsKey("EarnedSv")) {
				_earnedStatValue = JSONTools.ReadInt(dict["EarnedSv"]);
		}
		
		
		if ( dict.ContainsKey( "NeighborEarnedSV" ) )
		{
			_earnedNeighborValue = JSONTools.ReadInt( dict["NeighborEarnedSV"] );
		}
		
		//_filterType = ObjectiveFilterType.None;
		if (dict.ContainsKey("FT"))
		{
			if (Enum.IsDefined(typeof(ObjectiveFilterType), dict["FT"]))
			{
				_filterType = (ObjectiveFilterType) Enum.Parse(typeof(ObjectiveFilterType), (string)dict["FT"]);
			}
		}
		else if (dict.ContainsKey("filterIndex"))
		{
			switch (JSONTools.ReadInt(dict["filterIndex"]))
			{
				case 1:
					_filterType = ObjectiveFilterType.None;
				break;
				
				case 2:
					_filterType = ObjectiveFilterType.WithoutCoins;
					break;
					
				case 3:
					_filterType = ObjectiveFilterType.WithoutPowerups;
					break;
				
				case 4:
					_filterType = ObjectiveFilterType.WithoutArtifacts;
					break;
				case 5:
					_filterType = ObjectiveFilterType.WithoutTransition;
				break;
				
				case 6:
					
					_filterType = ObjectiveFilterType.PenniesFromHeavenOnly;
					break;
				case 7:
					_filterType = ObjectiveFilterType.MagicMagnetOnly;
					break;
					
				case 8:
					_filterType = ObjectiveFilterType.PoofOnly;
					break;
					
				case 9:
					_filterType = ObjectiveFilterType.FinleysFavorOnly;
					break;
					
				case 10:
					
					_filterType = ObjectiveFilterType.ScoreMultiplierOnly;
					break;
					
				case 11:
					_filterType = ObjectiveFilterType.JumpOverOnly;
					break;
					
				case 12:
					_filterType = ObjectiveFilterType.SlideUnderOnly;
					break;
				case 13:
					_filterType = ObjectiveFilterType.DodgeOnly;
					break;
				case 14:
					_filterType = ObjectiveFilterType.WithoutStumble;
					break;

				default:
					_filterType = ObjectiveFilterType.None;
					break;					
			}
			
		}
		
		
		//_type = ObjectiveType.Distance;
		if (dict.ContainsKey("T"))
		{
			if (Enum.IsDefined(typeof(ObjectiveType), dict["T"]))
			{
				_type = (ObjectiveType)Enum.Parse(typeof(ObjectiveType), (string)dict["T"]);	
				
				if(_type==ObjectiveType.Distance)
				{
					if(_filterType==ObjectiveFilterType.WithoutCoins)
						_type = ObjectiveType.DistanceWithoutCoins;
					else if(_filterType==ObjectiveFilterType.WithoutPowerups)
						_type = ObjectiveType.DistanceWithoutPowerups;
					else if (_filterType==ObjectiveFilterType.WithoutStumble)
						_type = ObjectiveType.DistanceWithoutStumble;
					else if (_filterType==ObjectiveFilterType.WithoutTransition)
						_type = ObjectiveType.DistanceWithoutTransition;
					else if(_filterType==ObjectiveFilterType.WithoutArtifacts)
						_type = ObjectiveType.DistanceWithoutArtifacts;
				}
			}
		} 
		else if (dict.ContainsKey("conditionType")) 
		{
			switch (JSONTools.ReadInt(dict["conditionType"]))
			{
			case 1:
				_type = ObjectiveType.Distance;
				break;
			case 2:
				_type = ObjectiveType.CollectCoins;
				break;
			case 3:
				_type = ObjectiveType.CollectSpecialCurrency;
				break;
			case 4:
				_type = ObjectiveType.Score;
				break;
			default:
				_type = ObjectiveType.Distance;
				break;
			}
			if(_type==ObjectiveType.Distance)
			{
				if(_filterType==ObjectiveFilterType.WithoutCoins)
					_type = ObjectiveType.DistanceWithoutCoins;
				else if(_filterType==ObjectiveFilterType.WithoutPowerups)
					_type = ObjectiveType.DistanceWithoutPowerups;
				else if (_filterType==ObjectiveFilterType.WithoutStumble)
					_type = ObjectiveType.DistanceWithoutStumble;
				else if (_filterType==ObjectiveFilterType.WithoutTransition)
					_type = ObjectiveType.DistanceWithoutTransition;
				else if(_filterType==ObjectiveFilterType.WithoutArtifacts)
					_type = ObjectiveType.DistanceWithoutArtifacts;
			}
		}
		
		//_timeType = ObjectiveTimeType.PerRun;
		if (dict.ContainsKey("TT"))
		{
			if (Enum.IsDefined(typeof(ObjectiveTimeType), dict["TT"]))
			{
				_timeType = (ObjectiveTimeType) Enum.Parse(typeof(ObjectiveTimeType), (string)dict["TT"]);	
			}
		}
		else if (dict.ContainsKey("oneRun"))
		{	
			bool oneRun = (bool)dict["oneRun"];
			
			if (oneRun) {
				_timeType = ObjectiveTimeType.PerRun;	
			} else {
				_timeType = ObjectiveTimeType.OverTime;
//				_timeType = ObjectiveTimeType.LifeTime;
			}
		}
		
		if (dict.ContainsKey("conditionIndex")) 
		{	
			_conditionIndex = JSONTools.ReadInt(dict["conditionIndex"]);
		}
		if (dict.ContainsKey("region"))
		{
			switch (JSONTools.ReadInt(dict["region"]))
			{
				case 1:
					_regionType = ConditionRegionType.Any;
					break;
					
				case 2:
					_regionType = ConditionRegionType.WhimsieWoods;
					break;
					
				case 3:
					_regionType = ConditionRegionType.YellowBrickRoad;
					break;
					
				case 4:
					_regionType = ConditionRegionType.DarkForest;
					break;
					
				default:
					_regionType = ConditionRegionType.Any;
					break;
			}
		}
	}
	
	public string ToJson() { return MiniJSON.Json.Serialize(this.ToDict()); }
	
	public Dictionary<string, object> ToDict()
	{
		Dictionary<string,object> d = new Dictionary<string, object>();
		
		d.Add("T", _type);
		d.Add("TT", _timeType);
		d.Add("FT", _filterType);
		d.Add("SV", _statValue);
		d.Add("EarnedSv", _earnedStatValue);
		// wxj
		d.Add("ActiRewardedCount", _actiRewardedCount);
		d.Add("ActiEarnedStatForEnvs", _actiEarnedStatForEnvs);
		return d;
	}
	
	public string ToWebCondJson() { return MiniJSON.Json.Serialize(this.ToWebCondDict()); }
	
	public Dictionary<string, object> ToWebCondDict( bool serializeNeighborProgress = false )
	{
		Dictionary<string, object> d = new Dictionary<string, object>();
		
		d.Add("conditionIndex", _conditionIndex);
		d.Add("T", _type);
		d.Add("TT", _timeType);
		d.Add("FT", _filterType);
		d.Add("SV", _statValue);
		d.Add("EarnedSv", _earnedStatValue);
		// wxj
		d.Add("ActiRewardedCount", _actiRewardedCount);
		d.Add("ActiEarnedStatForEnvs", _actiEarnedStatForEnvs);
		
		if ( serializeNeighborProgress )
		{
			d.Add ( "NeighborEarnedSV", _earnedNeighborValue );
		}
		
		return d;
	}
}

