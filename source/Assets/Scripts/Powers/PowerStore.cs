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

public enum PowerType 
{
	InstaScoreBonus, 
	InstaCoinBonus, 
	InstaGemBonus,
	InstaBoostPower,
	InstaMagnetPower,
	InstaPoofPower,
	InstaShieldPower,
	InstaHypnosis,
	InstaMagicWandPower,
}
	
public class PowerStore
{
	protected static Notify notify = new Notify("PowerStore");
	public static List<BasePower> Powers = new List<BasePower>();
	
	public static bool LoadFile()
	{
		string fileName = "OZGameData/Powers";
		TextAsset powersText = Resources.Load(fileName) as TextAsset;
		if (powersText == null)
		{
			notify.Warning("No Powers.txt exists at: " + fileName);	
			return false;
		}
		
		if (Powers == null) { Powers = new List<BasePower>(); }
		else { Powers.Clear(); }
		
		//-- Security check
		//notify.Debug("PowerStore " + artText.text);
		Dictionary<string, object> loadedData = MiniJSON.Json.Deserialize(powersText.text) as Dictionary<string, object>;
		
		if (SaveLoad.Load(loadedData) == false)
		{
			//-- IGNORE hash in editor. allowing for editing outside of editor.
#if !UNITY_EDITOR
			return false;
#endif
		}
		
		List<object> store = loadedData["data"] as List<object>;
		if (store == null) { return false; }
		
		foreach(object dict in store) 
		{
			Dictionary<string, object> data = dict as Dictionary<string, object>;
			if (data.ContainsKey("Type")) 
			{
				string typeString = (string)data["Type"];
				BasePower p = PowerStore.PowerFromPowerString(typeString);
				if (p != null)
				{
					p.SetDataFromDictionary(data);
					Powers.Add(p);		
				}
			}
		}
		return true;
	}
	
#if UNITY_EDITOR	
	public static int GetNextPowerID() 
	{
		int nextID = 0;
		foreach (BasePower p in Powers) 
		{
			if (p == null) { continue; }
			nextID = p.PowerID + 1;
		}
		return nextID;
	}
#endif
	
	static public BasePower PowerFromID(int powerID) 
	{
		if (PowerStore.Powers == null) { return null; }
		
		foreach (BasePower bp in PowerStore.Powers)
		{
			if (bp == null || bp.PowerID != powerID) { continue; }
			return bp;
		}
		return null;
	}
	
	public static BasePower PowerFromPowerString(string powerType)
	{
		BasePower newPower = null;
		
		if (powerType == typeof(InstaScoreBonus).ToString()) 
			newPower = new InstaScoreBonus();
		else if(powerType == typeof(InstaCoinBonus).ToString())
			newPower = new InstaCoinBonus();
		else if(powerType == typeof(InstaGemBonus).ToString())
			newPower = new InstaGemBonus(); 
		else if(powerType == typeof(InstaBoostPower).ToString())
			newPower = new InstaBoostPower();
		else if(powerType == typeof(InstaMagnetPower).ToString())
			newPower = new InstaMagnetPower();
		else if(powerType == typeof(InstaPoofPower).ToString())
			newPower = new InstaPoofPower();
		else if(powerType == typeof(InstaShieldPower).ToString())
			newPower = new InstaShieldPower();
		else if(powerType == typeof(InstaHypnosis).ToString())
			newPower = new InstaHypnosis();
		else if(powerType == typeof(InstaMagicWandPower).ToString())
			newPower = new InstaMagicWandPower();
		
		return newPower;
	}
	
	public static void SaveFile()
	{
		string fileName = Application.dataPath + Path.DirectorySeparatorChar + "Resources" + Path.DirectorySeparatorChar + "OZGameData/Powers.txt";
		List<object> list = new List<object>();
		
		foreach (BasePower power in Powers) 
		{
			list.Add(power.ToDict());
		}
		
		//-- Hash before we save.
		Dictionary<string, object> secureData = SaveLoad.Save(list);
		string listString = MiniJSON.Json.Serialize(secureData);
		
		try 
		{
			using (StreamWriter fileWriter = File.CreateText(fileName))
			{
				fileWriter.WriteLine(listString);
				fileWriter.Close(); 
			}
		}
		catch (Exception e) 
		{
			Dictionary<string,string> d = new Dictionary<string, string>();
			d.Add("Exception",e.ToString());
			notify.Warning("Save Exception: " + e);
		}
	}
}





//#if UNITY_EDITOR
//	static public void AddPowerup(BasePower data) 
//	{
//		if (PowerStore.Powers == null) { return; }
//		PowerStore.Powers.Add(data);
//	}
//	
//	static public void RemovePowerup(BasePower data) 
//	{
//		if (PowerStore.Powers == null) { return; }
//		PowerStore.Powers.Remove(data);
//	}
//#endif	


//	
//	public static BasePower PowerFromPowerType(System.Type powerType) {
//		BasePower newPower = null;
//		
//		if(powerType == typeof(InstaScoreBonus)) {
//			newPower = new InstaScoreBonus();
//		}
//		else if(powerType == typeof(InstaCoinBonus)) {
//			newPower = new InstaCoinBonus();
//		}
//		else if(powerType == typeof(InstaGemBonus)) {
//			newPower = new InstaGemBonus();
//		}
//		else if(powerType == typeof(InstaBoostPower)) {
//			newPower = new InstaBoostPower();
//		}
//		else if(powerType == typeof(InstaMagnetPower)) {
//			newPower = new InstaMagnetPower();
//		}
//		else if(powerType == typeof(InstaPoofPower)) {
//			newPower = new InstaPoofPower();
//		}
//		else if(powerType == typeof(InstaShieldPower)) {
//			newPower = new InstaShieldPower();
//		}
//		else if(powerType == typeof(InstaHypnosis)) {
//			newPower = new InstaHypnosis();
//		}
//		else if(powerType == typeof(InstaMagicWandPower)) {
//			newPower = new InstaMagicWandPower();
//		}
//		return newPower;
//	}			
