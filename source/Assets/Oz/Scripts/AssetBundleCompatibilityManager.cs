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

public class AssetBundleCompatibilityManager : MonoBehaviour
{
	protected static Notify notify = new Notify("AssetBundleCompatibilityManager");
	public static List<AssetBundleCompatibilityEntry> Entries = new List<AssetBundleCompatibilityEntry>();
	
	public static bool LoadFile()
	{
		string fileName = "OZGameData/AssetBundleCompatibility";
		TextAsset entryText = Resources.Load(fileName) as TextAsset;
		
		if (entryText == null)
		{
			notify.Warning("No AssetBundleCompatibility.txt exists at: " + fileName);
			return false;
		}
		
		if (Entries == null)
		{
			Entries = new List<AssetBundleCompatibilityEntry>();
		}
		else
		{
			Entries.Clear();
		}
		
		Dictionary<string, object> loadedData = MiniJSON.Json.Deserialize(entryText.text) as Dictionary<string, object>;
		
		if (SaveLoad.Load(loadedData) == false)
		{
#if !UNITY_EDITOR
			return false;
#endif
		}
		
		List<object> entries = loadedData["data"] as List<object>;
		if (entries == null) { return false; }
		
		foreach (object dict in entries)
		{
			Dictionary<string, object> data = dict as Dictionary<string, object>;
			AssetBundleCompatibilityEntry entry = new AssetBundleCompatibilityEntry();
			entry.SetDataFromDictionary(data);
			Entries.Add(entry);
		}
		return false;
	}

	public static void SaveFile()
	{
		string fileName = Application.dataPath + Path.DirectorySeparatorChar 
			+ "Resources" + Path.DirectorySeparatorChar + "OZGameData/AssetBundleCompatibility.txt";
		
		List<object> list = new List<object>();
		
		foreach (AssetBundleCompatibilityEntry entry in Entries)
		{
			list.Add(entry.ToDict());
		}
		
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
			notify.Warning("AssetBundleCompatibilityEntry Save Exception: " + e);
		}
	}
	
	public static AssetBundleCompatibilityEntry GetAssetBundleCompatibilityEntry(string clientVersion, string assetBundleName)
	{
		if (AssetBundleCompatibilityManager.Entries == null) { return null; }
		
		foreach (AssetBundleCompatibilityEntry entry in AssetBundleCompatibilityManager.Entries)
		{
			if (entry == null) { continue; }
			
			if (entry.clientVersion == clientVersion && entry.assetBundleName == assetBundleName) 
			{ 
				return entry; 
			}
		}
		return null;
	}	
}






//#if UNITY_EDITOR
//	public static int GetNextAssetBundleCompatibilityEntryID()
//	{
//		int nextID = 0;
//		foreach (AssetBundleCompatibilityEntry entry in Entries)
//		{
//			if (entry == null) { continue; }
//			nextID = entry.id + 1;
//		}
//		return nextID;
//	}
//#endif



//	public static AssetBundleCompatibilityEntry AssetBundleCompatibilityEntryFromID(int gameTipID)
//	{
//		if (AssetBundleCompatibilityEntryManager.Entries == null) { return null; }
//		
//		foreach (AssetBundleCompatibilityEntry entry in AssetBundleCompatibilityEntryManager.Entries)
//		{
//			if (entry == null || entry.id != gameTipID) { continue; }
//			return entry;
//		}
//		return null;
//	}
	
//	public static AssetBundleCompatibilityEntry GetRandomAssetBundleCompatibilityEntry()
//	{
//		if (AssetBundleCompatibilityEntryManager.Entries == null || AssetBundleCompatibilityEntryManager.Entries.Count == 0 ) { 
//			return null; }
//		
//		int randomIndex = UnityEngine.Random.Range(0, AssetBundleCompatibilityEntryManager.Entries.Count);
//		return Entries[randomIndex];
//	}
	