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

[Serializable]
public class Buff
{
	public int cost;
	public BuffType buffType;
	public int itemID;
	public int usesLeft;
	public StatValueType statValueType;
	public double statValue;
	
	static public int BaseCost = 120;
	static public int BaseUses = 10;
	static public int Version = 1;
	
	public Buff() 
	{
		cost = BaseCost;
		buffType = BuffType.Powerup;
		itemID = -1;
		usesLeft = BaseUses;
		statValueType = StatValueType.Percent;
		statValue = 1.0f;
	}
	
	public Dictionary<string, object> ToDict() 
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add ("v", Buff.Version);
		data.Add ("c", cost);
		data.Add ("t", buffType.ToString());
		data.Add ("id", itemID);
		data.Add ("u", usesLeft);
		data.Add ("StatValueType", statValueType.ToString());
		data.Add ("StatValue", statValue);
		
		return data;
	}
	
	public Buff(Dictionary<string, object> data)
	{
		int v = 0;
		
		if(data.ContainsKey("v")) 
		{
			object obj = data["v"];
			v = JSONTools.ReadInt(obj);
		}
		
		if (v >= 1) 
		{
			if (data.ContainsKey("c")) {cost = JSONTools.ReadInt(data["c"]); }
			if (data.ContainsKey("id")) {itemID = JSONTools.ReadInt(data["id"]); }
			if (data.ContainsKey("u")) {usesLeft = JSONTools.ReadInt(data["u"]); }
			buffType = BuffType.Powerup;
			if (data.ContainsKey("t")) 
			{
				if (Enum.IsDefined(typeof(BuffType), data["t"])) 
				{
					buffType = (BuffType)Enum.Parse(typeof(BuffType), (string)data["t"]);
				}
			}
			
			statValueType = StatValueType.Percent;
			if (data.ContainsKey("StatValueType"))
			{
				if (Enum.IsDefined(typeof(StatValueType), data["StatValueType"]))
				{
					statValueType = (StatValueType)Enum.Parse(typeof(StatValueType), (string)data["StatValueType"]);
				}
			}
		
			statValue = 0.0f;
			if (data.ContainsKey("StatValue")) 
			{
				object obj = data["StatValue"];
				if (obj.GetType() == typeof(Int64)) { statValue = (double)((long) obj); }
				else if (obj.GetType() == typeof(int)) { statValue = (int)obj; }
				else if (obj.GetType() == typeof(Double)) { statValue = (double)(obj); }
			}
		}
	}
}
