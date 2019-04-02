using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameTip
{
	public string title = "";
	public string tip = "";
	public int id = 0;
	
	public bool _showFoldOut = false;
	
	public void SetDataFromDictionary(Dictionary<string, object> data)
	{
		if (data.ContainsKey("Title")) { title = (string)data["Title"]; }
		
		if (data.ContainsKey("Tip")) { tip = (string)data["Tip"]; }
		
		if (data.ContainsKey("ID")) { id = JSONTools.ReadInt(data["ID"]); }
	}
	
	public string ToJson()
	{
		Dictionary<string, object> d = this.ToDict();
		return MiniJSON.Json.Serialize(d);
	}
	
	public Dictionary<string, object> ToDict()
	{
		Dictionary<string,object> d = new Dictionary<string, object>();
		d.Add("Title", title);
		d.Add("Tip", tip);
		d.Add("ID", id);
		return d;
	}
}

