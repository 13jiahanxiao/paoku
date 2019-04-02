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

public class ConsumableStore
{
	protected static Notify notify = new Notify("ConsumableStore");
	public static int maxOfEachConsumable = Int32.MaxValue;	//999;
	public static List<BaseConsumable> consumablesList = new List<BaseConsumable>();
	
	public static bool LoadFile()
	{
		consumablesList.Clear();
		
		TextAsset jsonText = Resources.Load("OZGameData/Consumables") as TextAsset;
		//notify.Debug("consumableStore " + jsonText.text);
		Dictionary<string, object> loadedData = MiniJSON.Json.Deserialize(jsonText.text) as Dictionary<string, object>;	//-- Security check
		List<object> store = loadedData["data"] as List<object>;
		
		foreach(object dict in store)			
		{
			Dictionary<string, object> data = dict as Dictionary<string, object>;
			string typeString = (string)data["Type"];
			BaseConsumable newConsumable = ConsumableStore.ConsumableFromString(typeString);
			newConsumable.SetDataFromDictionary(data);
			consumablesList.Add(newConsumable);	
		}
		return true;
	}
	
	public static void SaveFile()
	{
		string fileName = Application.dataPath + Path.DirectorySeparatorChar + "Resources" + 
			Path.DirectorySeparatorChar + "OZGameData/Consumables.txt";
		List<object> list = new List<object>();
		foreach(BaseConsumable consumable in consumablesList) 
		{ 
			//consumable.Type = consumable.GetType().ToString();
			list.Add(consumable.ToDict()); 
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
	
#if UNITY_EDITOR	
	public static int GetNextConsumableID() 
	{
		int nextID = 0;
		foreach(BaseConsumable consumable in consumablesList)
			nextID = consumable.PID + 1;
		return nextID;
	}
#endif
	
	static public BaseConsumable ConsumableFromID(int consumableID) 
	{
		foreach(BaseConsumable consumable in ConsumableStore.consumablesList) 
		{
			if (consumable.PID == consumableID)
				return consumable;
		}
		return null;
	}
	
	public static BaseConsumable ConsumableFromString(string typeString) 
	{
		BaseConsumable newConsumable = new BaseConsumable();
		if (typeString == "HeadStartConsumable")
			newConsumable = new HeadStartConsumable();
		else if(typeString == "ExtraMultiplierConsumable")
			newConsumable = new ExtraMultiplierConsumable();
		else if(typeString == "NoStumbleConsumable")
			newConsumable = new NoStumbleConsumable();
		else if(typeString == "ThirdEyeConsumable")
			newConsumable = new ThirdEyeConsumable();
		else if(typeString == "FastTravelConsumable")
			newConsumable = new FastTravelConsumable();		
		//else if (typeString == "MegaHeadStartConsumable")
		//	newConsumable = new MegaHeadStartConsumable();		
		
		//else if(typeString == typeof(MegaHeadStart).ToString())
		//	newConsumable = new MegaHeadStart();
		//else if(typeString == typeof(MegaHeadStart).ToString())
		//	newConsumable = new MegaHeadStart();
		return newConsumable;
	}
}



//
//#if UNITY_EDITOR
//	static public void AddConsumable(BaseConsumable data) 
//	{
//		if (ConsumableStore.consumablesList == null) { return; }
//		ConsumableStore.consumablesList.Add(data);
//	}
//	
//	static public void RemoveConsumable(BaseConsumable data) 
//	{
//		if (ConsumableStore.consumablesList == null) { return; }
//		ConsumableStore.consumablesList.Remove(data);
//	}
//#endif	
//}





//public static BasePower PowerFromPowerType(System.Type powerType) {
//		BasePower newPower = null;
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
//		return newPower;
//	}
//	
