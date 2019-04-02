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

public class Store: MonoBehaviour
{
	protected static Notify notify = new Notify("Store");
	public static List<StoreItem> StoreItems = new List<StoreItem>();
	
	private WeeklyDiscountManager weeklyDiscountManager;
	public List<WeeklyDiscountProtoData> GetWeeklyDiscounts() { return weeklyDiscountManager.GetWeeklyDiscounts(); }
	
	public WeeklyDiscountManager GetWeeklyDiscountManagerClass()
	{
		return weeklyDiscountManager;
	}

	void Start()
	{
		weeklyDiscountManager = gameObject.AddComponent<WeeklyDiscountManager>();
	}
	
	public static bool LoadFile()
	{
		string fileName = "OZGameData/StoreItems";
		TextAsset storeText = Resources.Load(fileName) as TextAsset;
		
		if (storeText == null)
		{
			notify.Warning("No StoreItems.txt exists at: " + fileName);	
			return false;
		}
		
		if (StoreItems == null) { StoreItems = new List<StoreItem>(); }
		else { StoreItems.Clear(); }
		
		//-- Security check
		//notify.Debug("PowerStore " + artText.text);
		Dictionary<string, object> loadedData = MiniJSON.Json.Deserialize(storeText.text) as Dictionary<string, object>;
		
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
			StoreItem si = new StoreItem();
			si.SetDataFromDictionary(data);
			StoreItems.Add(si);		
		}
		return true;
	}
	
#if UNITY_EDITOR	
	public static int GetNextStoreItemID() 
	{
		int nextID = 0;
		foreach (StoreItem si in StoreItems) 
		{
			if (si == null) { continue; }
			nextID = si.id + 1;
		}
		return nextID;
	}
#endif
	
	static public StoreItem StoreItemFromID(int storeItemID) 
	{
		if (Store.StoreItems == null) { return null; }
		
		foreach (StoreItem si in Store.StoreItems)
		{
			if (si == null || si.id != storeItemID) { continue; }
			return si;
		}
		return null;
	}
		
	public static void SaveFile()
	{
		string fileName = Application.dataPath + Path.DirectorySeparatorChar + "Resources" + Path.DirectorySeparatorChar + "OZGameData/StoreItems.txt";
		List<object> list = new List<object>();
		
		foreach (StoreItem storeItem in StoreItems) 
		{
			list.Add(storeItem.ToDict());
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
	
	public void ApplyDiscountFromInit(List<object> discountList, int responseCode)
	{
		weeklyDiscountManager.ApplySalesFromInit(discountList, responseCode);
	}
}
