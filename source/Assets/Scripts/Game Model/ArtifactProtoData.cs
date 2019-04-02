using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum StatType
{
	MaxSpeed,
	HeadStart,				//Used in Oz (Finley's favor)
	BoostDistance,			
	MinSpeed,
	CoinMultiplier,			//Used in Oz?
	ScoreMultplier,
	TemporaryScoreMultiplier,
	MagnetDuration,			//Used in Oz
	ResurrectionDiscount,
	HeadStartDiscount,
	MegaHeadStartDiscount,
	ShieldDuration,
	//Oz
	DoubleCoinsDistance,
	TripleCoinsDistance,
	
	PoofDuration,
	GemChance,
	GemValue,
	Acceleration,
	CoinMeterFillRate,
	MegaCoinValue,
	LuckIncrease,
	
	GeneralPowerDuration,
	
	// new Imangi values
	DistanceBetweenBonusItems,
	CoinMeterFillCount,
	
	HeadStartIncrease,
	
	None,

	Total,
}

public enum StatValueType
{
	Percent,
	Absolute,
	Relative,
	CopyStatValue, //<--for gemmedValueType only!
}

[Serializable]
public class ArtifactProtoData
{
	// wxj, id of oneKey
	public string _onekey_iap_id;
	public int _onekey_coins;
	public string _onekey_msg_key;
	
	public string _iconName;
	public string _title;
	public string _gemmedTitle;
	public string _description;
	public string _buffDescription;
	public int _cost;
	public int _cost_lv1 { get { return _cost; } }
	public int _cost_lv2;
	public int _cost_lv3;
	public int _cost_lv4;
	public int _cost_lv5;
	public StatType _statType;
	public StatValueType _statValueType;
	public double _statValue;
	public double _statValue_lv2;
	public double _statValue_lv3;
	public double _statValue_lv4;
	public double _statValue_lv5;
	public StatType _gemmedStatType;
	public StatValueType _gemmedValueType;
	public double _gemmedValue;
	public CostType _costType;
	public int _id;
	public int _requiredRank;
	public int _progressionID;
	public int _nextArtifactIDInProgression;
	public int _sortPriority;
	
	public int ServerCost_Lvl1 = 0;
	public int ServerCost_Lvl2 = 0;
	public int ServerCost_Lvl3 = 0;
	public int ServerCost_Lvl4 = 0;
	public int ServerCost_Lvl5 = 0;
	
	public int DefaultSortPriority { get; set; }
	
// eyal edit	
//#if UNITY_EDITOR
	public bool _showFoldOut = false;
//#endif
		
	// wxj 
	public ArtifactProtoData(string icon, string title, string desc, string buffdesc, int cost, int cost2, int cost3, int cost4, int cost5, 
		StatType statType, StatValueType valueType, float statVal, float statVal2, float statVal3, float statVal4, float statVal5,
		StatType gemmedStatType, StatValueType gemmendValueType, float gemmedVal, CostType costType, string onekeyIapId, int onekeyCoins) 
	{
		_iconName = icon;
		_title = title;
		_gemmedTitle = "";
		_description = desc;
		_buffDescription = buffdesc;
		_cost = cost;
		_cost_lv2 = cost2;
		_cost_lv3 = cost3;
		_cost_lv4 = cost4;
		_cost_lv5 = cost5;
		_statType = statType;
		_statValueType = valueType;
		_statValue = statVal;
		_statValue_lv2 = statVal2;
		_statValue_lv3 = statVal3;
		_statValue_lv4 = statVal4;
		_statValue_lv5 = statVal5;
		_gemmedStatType = gemmedStatType;
		_gemmedValueType = gemmendValueType;
		_gemmedValue = gemmedVal;
		_costType = costType;
		_requiredRank = 0;
		_progressionID = -1;
		_sortPriority = 0;

		// wxj
		_onekey_iap_id = onekeyIapId;
		_onekey_coins = onekeyCoins;
		_onekey_msg_key = "";

#if UNITY_EDITOR
		_showFoldOut = false;
		_id = ArtifactStore.GetNextArtifactID();
#endif
	}
	
	public ArtifactProtoData(Dictionary<string, object> dict) 
	{
#if UNITY_EDITOR
		_showFoldOut = false;	//true;
#endif
		
		// wxj
		_onekey_iap_id = "";
		if(dict.ContainsKey("OneKey_id"))
			_onekey_iap_id = (string)dict["OneKey_id"];
		_onekey_coins = 0;
		if(dict.ContainsKey("OneKey_coins_cost"))
			_onekey_coins = (int)Math.Abs((long)dict["OneKey_coins_cost"]);
		_onekey_msg_key = "";
		if(dict.ContainsKey("OneKey_msg_key"))
			_onekey_msg_key = (string)dict["OneKey_msg_key"];
		
		
		_title = "Empty";
		if (dict.ContainsKey("Title")) 
			_title = (string)dict["Title"];
		
		_gemmedTitle = "Empty";
		if (dict.ContainsKey("GemmedTitle")) 
			_gemmedTitle = (string)dict["GemmedTitle"];
		
		_iconName = null;
		if (dict.ContainsKey("IconName")) 
			_iconName = (string)dict["IconName"];
		
		_description = "none";
		if (dict.ContainsKey("Description")) 
			_description = (string)dict["Description"];
		
		_buffDescription = "Empty";
		if (dict.ContainsKey("BuffDescription")) 
			_buffDescription = (string)dict["BuffDescription"];
		
		_cost = 10000;
		if (dict.ContainsKey("Cost")) 
			_cost = (int)Math.Abs((long)dict["Cost"]);
		_cost_lv2 = 10000;
		if (dict.ContainsKey("Cost_lv2")) 
			_cost_lv2 = (int)Math.Abs((long)dict["Cost_lv2"]);
		_cost_lv3 = 10000;
		if (dict.ContainsKey("Cost_lv3")) 
			_cost_lv3 = (int)Math.Abs((long)dict["Cost_lv3"]);
		_cost_lv4 = 10000;
		if (dict.ContainsKey("Cost_lv4")) 
			_cost_lv4 = (int)Math.Abs((long)dict["Cost_lv4"]);
		_cost_lv5 = 10000;
		if (dict.ContainsKey("Cost_lv5")) 
			_cost_lv5 = (int)Math.Abs((long)dict["Cost_lv5"]);
		
		_progressionID = -1;
		if (dict.ContainsKey("Prog")) 
			_progressionID = JSONTools.ReadInt(dict["Prog"]);
		
		_nextArtifactIDInProgression = -1;		
		if (dict.ContainsKey("NextProg")) 
			_nextArtifactIDInProgression = JSONTools.ReadInt(dict["NextProg"]);
		
		_sortPriority = 0;
		if (dict.ContainsKey("SortPriority"))
			_sortPriority = int.Parse((string)dict["SortPriority"]);	
		
		if (dict.ContainsKey("PID")) 
		{
			_id = JSONTools.ReadInt(dict["PID"]);
		}
		else 
		{
//#if UNITY_EDITOR			
//			_id = ArtifactStore.GetNextArtifactID();
//#endif
		}
		
		_requiredRank = 0;
		if (dict.ContainsKey("rank"))
			_requiredRank = JSONTools.ReadInt(dict["rank"]);
		
		_statType = StatType.Total;
		if (dict.ContainsKey("StatType")) 
		{
			if (Enum.IsDefined(typeof(StatType), dict["StatType"]))
				_statType = (StatType)Enum.Parse(typeof(StatType), (string)dict["StatType"]);
		}
		
		_statValueType = StatValueType.Percent;
		if (dict.ContainsKey("StatValueType")) 
		{
			if (Enum.IsDefined(typeof(StatValueType), dict["StatValueType"]))
				_statValueType = (StatValueType)Enum.Parse(typeof(StatValueType), (string)dict["StatValueType"]);
		}
		
		_statValue = 0.0f;
		_statValue_lv2 = 0.0f;
		_statValue_lv3 = 0.0f;
		_statValue_lv4 = 0.0f;
		_statValue_lv5 = 0.0f;
		if (dict.ContainsKey("StatValue")) 
			_statValue = ParseObjectToDouble(dict["StatValue"]);
		if (dict.ContainsKey("StatValue_lv2")) 
			_statValue_lv2 = ParseObjectToDouble(dict["StatValue_lv2"]);
		if (dict.ContainsKey("StatValue_lv3")) 
			_statValue_lv3 = ParseObjectToDouble(dict["StatValue_lv3"]);
		if (dict.ContainsKey("StatValue_lv4")) 
			_statValue_lv4 = ParseObjectToDouble(dict["StatValue_lv4"]);
		if (dict.ContainsKey("StatValue_lv5")) 
			_statValue_lv5 = ParseObjectToDouble(dict["StatValue_lv5"]);
		
		_gemmedStatType = StatType.Total;
		if (dict.ContainsKey("GemmedStatType")) 
		{
			if (Enum.IsDefined(typeof(StatType), dict["GemmedStatType"])) 
				_gemmedStatType = (StatType)Enum.Parse(typeof(StatType), (string)dict["GemmedStatType"]);
		}
		
		_gemmedValueType = StatValueType.Percent;
		if (dict.ContainsKey("GemmedValueType"))
		{
			if (Enum.IsDefined(typeof(StatValueType), dict["GemmedValueType"])) 
				_gemmedValueType = (StatValueType)Enum.Parse(typeof(StatValueType), (string)dict["GemmedValueType"]);
		}
		
		_gemmedValue = 0.0f;
		if (dict.ContainsKey("GemmedValue"))
		{
			object obj = dict["GemmedValue"];
			if (obj.GetType() == typeof(Int64))
				_gemmedValue = (double)((long) obj);
			else if (obj.GetType() == typeof(int))
				_gemmedValue = (int)(obj);
			else if (obj.GetType() == typeof(Double))
				_gemmedValue = (double)(obj);
		}
		
		//-- Default to coin type.
		_costType = CostType.Coin;
		if (dict.ContainsKey("CostType"))
		{
			if (Enum.IsDefined(typeof(CostType), dict["CostType"]))
				_costType = (CostType)Enum.Parse(typeof(CostType), (string)dict["CostType"]);
		}
	}
	
	public double ParseObjectToDouble(object obj)
	{
		if (obj.GetType() == typeof(Int64))
			return (double)((long) obj);
		else if (obj.GetType() == typeof(int))
			return  (double)((int)(obj));
		else if (obj.GetType() == typeof(Double))
			return (double)obj;
		return 0.0; 
	}
	
	public string ToJson() 
	{
		Dictionary<string, object> d = ToDict();
		return MiniJSON.Json.Serialize(d);
	}	
	
	public int GetCost(int level)
	{
		if(level==1)	return _cost;
		if(level==2)	return _cost_lv2;
		if(level==3)	return _cost_lv3;
		if(level==4)	return _cost_lv4;
		if(level==5)	return _cost_lv5;
		return 0;
	}
	
	public int GetDiscountCost( int level )
	{
		if ( level == 1 ) return ServerCost_Lvl1;
		if ( level == 2 ) return ServerCost_Lvl2;
		if ( level == 3 ) return ServerCost_Lvl3;
		if ( level == 4 ) return ServerCost_Lvl4;
		if ( level == 5 ) return ServerCost_Lvl5;
		return 0;
	}
	
	public int GetFinalCost( int level )
	{
		//If a value was assigned to ServerCost, use the Server Cost Levels
		if ( ServerCost_Lvl1 > 0 )
		{
			switch ( level )
			{
				case 1:
					return ServerCost_Lvl1;
				case 2:
					return ServerCost_Lvl2;
				case 3:
					return ServerCost_Lvl3;
				case 4:
					return ServerCost_Lvl4;
				case 5:
					return ServerCost_Lvl5;
				default:
					return 0;
			}
		}
		else
		{
			switch ( level )
			{
				case 1:
					return _cost_lv1;
				case 2:
					return _cost_lv2;
				case 3:
					return _cost_lv3;
				case 4:
					return _cost_lv4;
				case 5:
					return _cost_lv5;
				default:
					return 0;
			}
			
		}
	}
	
	public double GetStatValue(int level)
	{
		if(level==1)	return _statValue;
		if(level==2)	return _statValue_lv2;
		if(level==3)	return _statValue_lv3;
		if(level==4)	return _statValue_lv4;
		if(level==5)	return _statValue_lv5;
		return 0;
	}
	
	public Dictionary<string, object> ToDict() 
	{
		Dictionary<string, object> d = new Dictionary<string, object>();
		d.Add ("Title", _title);
		d.Add ("GemmedTitle", _gemmedTitle);
		d.Add ("IconName", _iconName);
		d.Add ("Description", _description);
		d.Add ("BuffDescription", _buffDescription);
		d.Add ("Cost", _cost);
		d.Add ("Cost_lv2", _cost_lv2);
		d.Add ("Cost_lv3", _cost_lv3);
		d.Add ("Cost_lv4", _cost_lv4);
		d.Add ("Cost_lv5", _cost_lv5);
		d.Add ("StatType", _statType.ToString());
		d.Add ("StatValueType", _statValueType.ToString());
		d.Add ("StatValue", _statValue);
		d.Add ("StatValue_lv2", _statValue_lv2);
		d.Add ("StatValue_lv3", _statValue_lv3);
		d.Add ("StatValue_lv4", _statValue_lv4);
		d.Add ("StatValue_lv5", _statValue_lv5);
		d.Add ("GemmedStatType", _gemmedStatType.ToString());
		d.Add ("GemmedValueType", _gemmedValueType.ToString());
		d.Add ("GemmedValue", _gemmedValue);
		d.Add ("CostType", _costType.ToString());
		d.Add ("Prog", _progressionID);
		d.Add ("NextProg", _nextArtifactIDInProgression);
		d.Add ("SortPriority", _sortPriority.ToString());		
		d.Add ("PID", _id);
		d.Add ("rank", _requiredRank);
		
		// wxj
		d.Add("OneKey_id", _onekey_iap_id);
		d.Add("OneKey_coins_cost", _onekey_coins);
		d.Add("OneKey_msg_key", _onekey_msg_key);
		return d;
	}	
}