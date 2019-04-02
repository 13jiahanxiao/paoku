using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class GuildChallengeProtoData
{
	public int _challengeIndex = 0;
	
	public int _conditionValue = 0;
	
	public int _neighborValueTotal = 0;

	
	public GuildChallengeProtoData()
	{
		
	}
	
	public GuildChallengeProtoData(Dictionary<string, object> dict)
	{
		if (dict.ContainsKey("challengeIndex")) 
		{
			_challengeIndex = JSONTools.ReadInt(dict["challengeIndex"]);
		}
		
		if ( dict.ContainsKey( "conditionValue" ) )
		{
			_conditionValue = JSONTools.ReadInt( dict["conditionValue"] );
		}
		
		if ( dict.ContainsKey( "neighborValueTotal" ) )
		{
			_neighborValueTotal = JSONTools.ReadInt( dict["neighborValueTotal"] );
		}
	}
	
	public string ToJson() { return MiniJSON.Json.Serialize(this.ToDict()); }
	
	public Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> d = new Dictionary<string, object>();
		
		d.Add("challengeIndex", _challengeIndex);
		d.Add( "conditionValue", _conditionValue );
		d.Add( "neighborValueTotal", _neighborValueTotal );
		

		return d;
	}
	
	public string ToChallengeJson() { return MiniJSON.Json.Serialize(this.ToChallengeDict()); }
	
 	// When updating the profile on the server, only send the index and the conditionValue.
	public Dictionary<string, object> ToChallengeDict()
	{
		Dictionary<string, object> d = new Dictionary<string, object>();
		
		d.Add( "challengeIndex", _challengeIndex );
		d.Add( "conditionValue", _conditionValue );

		
		return d;
	}
	
	
}

	
	/*
	public int _bestScore = 0;
	public int _bestMeters = 0;
	public int _bestCoins = 0;
	public int _bestGems = 0;
	
	public int _currentScore = 0;
	public int _currentMeters = 0;
	public int _currentCoins = 0;
	public int _currentGems = 0;

	public List<NeighborChallengeConditionProtoData> _conditions = 
		new List<NeighborChallengeConditionProtoData>();
	
	public NeighborTotalChallengeProtoData _neighborTotals;
	*/

	/*
	d.Add("bestScore", _bestScore);
	d.Add("bestMeters", _bestMeters);
	d.Add("bestCoins", _bestCoins);
	d.Add("bestGems", _bestGems);
	
	d.Add("currentScore", _currentScore);
	d.Add("currentMeters", _currentMeters);
	d.Add("currentCoins", _currentCoins);
	d.Add("currentGems", _currentGems);
	
	List<object> conditionList = new List<object>();
	foreach (NeighborChallengeConditionProtoData neighborCondition in _conditions)
	{
		conditionList.Add(neighborCondition.ToDict());
	}
	d.Add("conditions", conditionList);
	
	//d.Add("neighborTotals", _neighborTotals.ToDict());
	*/
		
		/*
		List<object> conditionList = new List<object>();
		foreach (NeighborChallengeConditionProtoData neighborCondition in _conditions)
		{
			conditionList.Add(neighborCondition.ToDict());
		}
		d.Add("conditions", conditionList);
		*/
/*		
		else if (dict.ContainsKey("PID")) { _challengeIndex = JSONTools.ReadInt(dict["PID"]); }

		if (dict.ContainsKey("bestScore")) { _bestScore = int.Parse(dict["bestScore"].ToString()); }
		if (dict.ContainsKey("bestMeters")) { _bestMeters = int.Parse(dict["bestMeters"].ToString()); }
		if (dict.ContainsKey("bestCoins")) { _bestCoins = int.Parse(dict["bestCoins"].ToString ()); }
		if (dict.ContainsKey("bestGems")) { _bestGems = int.Parse(dict["bestGems"].ToString()); }
		
		if (dict.ContainsKey("currentScore")) { _currentScore = int.Parse(dict["currentScore"].ToString ()); }
		if (dict.ContainsKey("currentMeters")) { _currentMeters = int.Parse(dict["currentMeters"].ToString ()); }
		if (dict.ContainsKey("currentCoins")) { _currentCoins = int.Parse(dict["currentCoins"].ToString()); }
		if (dict.ContainsKey("currentGems")) { _currentGems = int.Parse(dict["currentGems"].ToString()); };
		
		if (dict.ContainsKey("neighborTotals")) 
		{
			Dictionary<string, object> neighborDict 
				= dict["neighborTotals"] as Dictionary<string,object>;
			
			NeighborTotalChallengeProtoData tempNeighborTotal 
				= new NeighborTotalChallengeProtoData(neighborDict);
			
			if (tempNeighborTotal != null)
			{
				_neighborTotals = tempNeighborTotal;
			} 
			else
			{
				_neighborTotals 
					= new NeighborTotalChallengeProtoData(new Dictionary<string, object>());
			}
		}
		
		if (dict.ContainsKey("conditions"))
		{
			List<object> tempConditionList = dict["conditions"] as List<object>;
			
			if (tempConditionList != null)
			{
				_conditions.Clear();
				
				foreach (object conditionObject in tempConditionList)
				{
					Dictionary<string, object> conditionDict 
						= conditionObject as Dictionary<string, object>;
					NeighborChallengeConditionProtoData tempCondition
						= new NeighborChallengeConditionProtoData(conditionDict);
					_conditions.Add(tempCondition);
				}
			}
		}
		
*/