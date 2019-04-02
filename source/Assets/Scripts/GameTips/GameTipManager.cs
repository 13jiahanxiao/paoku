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

public class GameTipManager : MonoBehaviour
{
	protected static Notify notify = new Notify("GameTipManager");
	public static List<GameTip> GameTips = new List<GameTip>();
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	public static bool LoadFile()
	{
		string fileName = "OZGameData/GameTips";
		TextAsset gameTipText = Resources.Load(fileName) as TextAsset;
		
		if (gameTipText == null)
		{
			notify.Warning("No GameTips.txt exists at: " + fileName);
			return false;
		}
		
		if (GameTips == null) { GameTips = new List<GameTip>(); }
		else { GameTips.Clear(); }
		
		Dictionary<string, object> loadedData 
			= MiniJSON.Json.Deserialize(gameTipText.text) as Dictionary<string, object>;
		
		if (SaveLoad.Load(loadedData) == false)
		{
#if !UNITY_EDITOR
			return false;
#endif
		}
		
		List<object> gameTips = loadedData["data"] as List<object>;
		if (gameTips == null) { return false; }
		
		foreach(object dict in gameTips)
		{
			Dictionary<string, object> data = dict as  Dictionary<string, object>;
			GameTip gt = new GameTip();
			gt.SetDataFromDictionary(data);
			GameTips.Add(gt);
		}
		return false;
	}
	
#if UNITY_EDITOR
	public static int GetNextGameTipID()
	{
		int nextID = 0;
		foreach (GameTip gt in GameTips)
		{
			if (gt == null) { continue; }
			nextID = gt.id + 1;
		}
		return nextID;
	}
#endif
	
	public static GameTip GameTipFromID(int gameTipID)
	{
		if (GameTipManager.GameTips == null) { return null; }
		
		foreach (GameTip gt in GameTipManager.GameTips)
		{
			if (gt == null || gt.id != gameTipID) { continue; }
			return gt;
		}
		return null;
	}
	
	public static GameTip GetRandomGameTip()
	{
		if (GameTipManager.GameTips == null || GameTipManager.GameTips.Count == 0 ) { 
			return null; }
		
		int randomIndex = UnityEngine.Random.Range(0, GameTipManager.GameTips.Count);
		return GameTips[randomIndex];
	}
	
	public static void SaveFile()
	{
		string fileName = Application.dataPath + Path.DirectorySeparatorChar 
			+ "Resources" + Path.DirectorySeparatorChar + "OZGameData/GameTips.txt";
		
		List<object> list = new List<object>();
		
		foreach (GameTip gt in GameTips)
		{
			list.Add(gt.ToDict());
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
			notify.Warning("GameTip Save Exception: " + e);
		}
	}
}

