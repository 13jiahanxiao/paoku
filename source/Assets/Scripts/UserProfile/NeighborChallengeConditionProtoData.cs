using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class NeighborChallengeConditionProtoData
{
	public int _conditionIndex = 0;
	
	public int _currentValue = 0;
	
	public int _neighborTotal = 0;
	
	public NeighborChallengeConditionProtoData()
	{
		
	}
	
	public NeighborChallengeConditionProtoData(Dictionary<string, object> dict) 
	{
		if (dict.ContainsKey("conditionIndex"))
		{
			_conditionIndex = int.Parse(dict["conditionIndex"].ToString());
		}
		
		if (dict.ContainsKey("currentValue"))
		{
			_currentValue = int.Parse(dict["currentValue"].ToString());
		}
		else if (dict.ContainsKey("EarnedSv"))
		{
			_currentValue = JSONTools.ReadInt(dict["EarnedSv"]);
		}
		
		if (dict.ContainsKey("neighborTotal"))
		{
			_neighborTotal = int.Parse(dict["neighborTotal"].ToString());
		}
	}
	
	public string ToJson() { return MiniJSON.Json.Serialize(this.ToDict()); }
	
	//Do not include Neighbor total in jSon
	public Dictionary<string,object> ToDict()
	{
		Dictionary<string,object> d = new Dictionary<string, object>();
		
		d.Add("conditionIndex", _conditionIndex);
		d.Add("currentValue", _currentValue);
		
		return d;
	}
}

