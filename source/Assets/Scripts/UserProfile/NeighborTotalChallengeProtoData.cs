using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class NeighborTotalChallengeProtoData
{
	public int _totalBestScore = 0;
	public int _totalBestMeters = 0;
	public int _totalBestCoins = 0;
	public int _totalBestGems = 0;
	
	public int _totalCurrentScore = 0;
	public int _totalCurrentMeters = 0;
	public int _totalCurrentCoins = 0;
	public int _totalCurrentGems = 0;
	
	public NeighborTotalChallengeProtoData (Dictionary<string, object> dict)
	{
		if (dict.ContainsKey("bestScore")) { _totalBestScore = int.Parse(dict["bestScore"].ToString()); }
		
		if (dict.ContainsKey("bestMeters")) { _totalBestMeters = int.Parse(dict["bestMeters"].ToString()); }
		
		if (dict.ContainsKey("bestCoins")) { _totalBestCoins = int.Parse(dict["bestCoins"].ToString()); }
		
		if (dict.ContainsKey("bestGems")) { _totalBestGems = int.Parse(dict["bestGems"].ToString()); }
		
		if (dict.ContainsKey("currentScore")) { _totalCurrentScore = int.Parse(dict["currentScore"].ToString()); }
		
		if (dict.ContainsKey("currentMeters")) { _totalCurrentMeters = int.Parse(dict["currentMeters"].ToString()); }
		
		if (dict.ContainsKey("currentCoints")) { _totalCurrentCoins = int.Parse(dict["currentCoins"].ToString()); }
		
		if (dict.ContainsKey("currentGems")) { _totalCurrentGems = int.Parse(dict["currentGems"].ToString()); }
	}
	
	public string ToJson () { return MiniJSON.Json.Serialize(this.ToDict()); }
	
	public Dictionary<string,object> ToDict()
	{
		Dictionary<string,object> d = new Dictionary<string, object>();
		d.Add("bestScore", _totalBestScore);
		d.Add("bestMeters", _totalBestMeters);
		d.Add("bestCoins", _totalBestCoins);
		d.Add("bestGems", _totalBestGems);
		
		d.Add("currentScore", _totalCurrentScore);
		d.Add("currentMeters", _totalCurrentMeters);
		d.Add("currentCoins", _totalCurrentCoins);
		d.Add("currentGems", _totalCurrentGems);
		
		return d;
	}
}

