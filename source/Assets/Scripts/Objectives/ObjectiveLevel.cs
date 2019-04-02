using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ObjectiveLevel
{
	public int ObjectivesRequired { get; set; }
	public List<int> difficulties = new List<int>();
	public int ID { get; set; }
	
	public RankRewardType rewardType;
	public int rewardValue;
	public string iconImage;
	//public string description;	
	//private int diffLevels = 7;
	
// eyal edit
//#if UNITY_EDITOR	
	public bool _showFoldOut = false;
//#endif
	
	//public ObjectiveLevel(Dictionary<string, object> data) { SetDataFromDictionary(data); }
	
	public ObjectiveLevel() 
	{
		ObjectivesRequired = 3;
		difficulties.Add(0);	// don't use difficulties[0], but have it there anyway
		for (int i=1; i<=7; i++) { difficulties.Add(0); }	// populate all difficulty settings
		ID = 0;
		
		rewardType = RankRewardType.Coins;
		rewardValue = 0;
		iconImage = "";
		//description = "";		
	}
	
	public void SetDataFromDictionary(Dictionary<string, object> data) 
	{
		if (data.ContainsKey("ObjectivesRequired")) { ObjectivesRequired = Int32.Parse((string)data["ObjectivesRequired"]); }
		
		for (int i=1; i<=7; i++)
		{
			if (data.ContainsKey("Difficulty" + i.ToString())) 
			{ 
				difficulties[i] = Int32.Parse((string)data["Difficulty" + i.ToString()]); 
			}	
		}
		
		if (data.ContainsKey("ID")) { ID = Int32.Parse((string)data["ID"]); }
		
		if (data.ContainsKey("RewardType")) { rewardType = (RankRewardType)Enum.Parse(typeof(RankRewardType), (string)data["RewardType"]); }
		if (data.ContainsKey("RewardValue")) { rewardValue = Int32.Parse((string)data["RewardValue"]); }
		if (data.ContainsKey("Icon")) { iconImage = (string)data["Icon"]; }
		//if (data.ContainsKey("Description")) { description = (string)data["Description"]; }
	}
	
	public string ToJson()
	{
		Dictionary<string, object> d = this.ToDict();
		return MiniJSON.Json.Serialize(d);
	}
	
	public Dictionary<string, object> ToDict() 
	{
		Dictionary<string, object> d = new Dictionary<string, object>();
		d.Add ("ObjectivesRequired", ObjectivesRequired.ToString());
		
		for (int i=1; i<=7; i++)
		{
			d.Add ("Difficulty" + i.ToString(), difficulties[i].ToString());		
		}
		
		d.Add ("ID", ID.ToString());
		
		d.Add ("RewardType", rewardType.ToString());
		d.Add ("RewardValue", rewardValue.ToString());
		d.Add ("Icon", iconImage);
		//d.Add ("Description", description);
		
		return d;
	}
}



	
//		d.Add ("Difficulty0", Difficulty1.ToString());			
//		d.Add ("Difficulty1", Difficulty1.ToString());		
//		d.Add ("Difficulty2", Difficulty2.ToString());
//		d.Add ("Difficulty3", Difficulty3.ToString());		
//		d.Add ("Difficulty4", Difficulty4.ToString());
//		d.Add ("Difficulty5", Difficulty5.ToString());
//		d.Add ("Difficulty6", Difficulty6.ToString());
//		d.Add ("Difficulty7", Difficulty7.ToString());	

	
//		
//		
//		if (data.ContainsKey("Difficulty0")) { Difficulty0 = Int32.Parse((string)data["Difficulty0"]); }		
//		if (data.ContainsKey("Difficulty1")) { Difficulty1 = Int32.Parse((string)data["Difficulty1"]); }
//		if (data.ContainsKey("Difficulty2")) { Difficulty2 = Int32.Parse((string)data["Difficulty2"]); }
//		if (data.ContainsKey("Difficulty3")) { Difficulty3 = Int32.Parse((string)data["Difficulty3"]); }
//		if (data.ContainsKey("Difficulty4")) { Difficulty4 = Int32.Parse((string)data["Difficulty4"]); }
//		if (data.ContainsKey("Difficulty5")) { Difficulty5 = Int32.Parse((string)data["Difficulty5"]); }
//		if (data.ContainsKey("Difficulty6")) { Difficulty6 = Int32.Parse((string)data["Difficulty6"]); }
//		if (data.ContainsKey("Difficulty7")) { Difficulty7 = Int32.Parse((string)data["Difficulty7"]); }


//
//	public int Difficulty0 { get; set; }	
//	public int Difficulty1 { get; set; }	
//	public int Difficulty2 { get; set; }	
//	public int Difficulty3 { get; set; }	
//	public int Difficulty4 { get; set; }	
//	public int Difficulty5 { get; set; }
//	public int Difficulty6 { get; set; }		
//	public int Difficulty7 { get; set; }

//
//		Difficulty0 = 0;		
//		Difficulty1 = 0;
//		Difficulty2 = 0;
//		Difficulty3 = 0;
//		Difficulty4 = 0;
//		Difficulty5 = 0;
//		Difficulty6 = 0;
//		Difficulty7 = 0;

//	public int V_Easy { get; set; }	
//	public int Easy { get; set; }
//	public int Medium { get; set; }
//	public int Hard { get; set; }
//	public int V_Hard { get; set; }