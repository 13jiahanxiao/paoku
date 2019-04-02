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

public class ArtifactStore
{
	public static List<ArtifactProtoData> Artifacts = new List<ArtifactProtoData>();
	protected static Notify notify = new Notify ("ArtifactStore");
	public static int maxOfEachArtifact = 5;

	public static bool LoadFile()
	{
		string fileName = "OZGameData/Artifacts";
		TextAsset artText = Resources.Load(fileName) as TextAsset;
		if (artText == null)
		{
			notify.Debug("No Artifacts.txt exists at: " + fileName);	
			return false;
		}
		
		if (Artifacts == null) { Artifacts = new List<ArtifactProtoData>(); }
		else { Artifacts.Clear(); }
		
		//-- Security check
		//notify.Debug("ArtifactStore  " + artText.text);
		Dictionary<string, object> loadedData = MiniJSON.Json.Deserialize(artText.text) as Dictionary<string, object>;
		if (SaveLoad.Load(loadedData) == false)
		{
			//-- IGNORE hash in editor. allowing for editing outside of editor.
			notify.Warning ("ArtifactStore, hashes are different.");
#if !UNITY_EDITOR
			return false;
#endif
		}
		
		List<object> store = loadedData["data"] as List<object>;
		
		if (store == null) 
		{
			Dictionary<string, object> fullStore = loadedData["data"] as Dictionary<string, object>;
			if (fullStore == null) 
			{
				notify.Warning("Artifacts.txt failed from jsonD: " + fileName);	
				return false;	
			}	
			
			object storeObject = null;
			if (fullStore.TryGetValue("list", out storeObject) == false)
			{
				notify.Warning("Artifacts.txt failed to find list from jsonD: " + fileName);	
				return false;	
			}
			store = storeObject as List<object>;
		}
			
		foreach(object dict in store)
		{
			Dictionary<string, object> data = dict as Dictionary<string, object>;
			Artifacts.Add ( new ArtifactProtoData(data) );
		}
		return true;
	}
	
	public static void SaveFile()
	{
#if UNITY_EDITOR		
		using (MemoryStream stream = new MemoryStream()) 
		{
			string fileName = Application.dataPath + Path.DirectorySeparatorChar + "Resources" + Path.DirectorySeparatorChar + "OZGameData/Artifacts.txt";
			List<object> store = new List<object>();
			
			foreach (ArtifactProtoData data in Artifacts) 
			{
				store.Add(data.ToDict());
			}
			
			Dictionary<string, object> fullStore = new Dictionary<string, object>();
			fullStore.Add("list", store);
			
			//-- Hash before we save.
			Dictionary<string, object> secureData = SaveLoad.Save(fullStore);
			string storeString = MiniJSON.Json.Serialize(secureData);
			
			try 
			{
				using (StreamWriter fileWriter = File.CreateText(fileName)) 
				{
					fileWriter.WriteLine(storeString);
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
#endif
	}
	
#if UNITY_EDITOR	
	public static int GetNextArtifactID()
	{
		int nextID = 0;
		foreach(ArtifactProtoData p in Artifacts) 
		{
			if (p == null) { continue; }
			nextID = p._id + 1;
		}
		return nextID;
	}
#endif

	static public int GetNumberOfArtifacts() 
	{
		if (ArtifactStore.Artifacts == null) { return 0; }
		return ArtifactStore.Artifacts.Count;
	}
	
	public static ArtifactProtoData GetArtifactProtoData(int artifactID) 
	{
		if (artifactID < 0 || ArtifactStore.Artifacts == null) { return null; }
		
		int max = ArtifactStore.Artifacts.Count;
		for (int i = 0; i < max; i++)
		{
			ArtifactProtoData data = ArtifactStore.Artifacts[i];
			if (data == null) { continue; }
			if (data._id == artifactID) { return data; }
		}
		return null;
	}
}




//#if UNITY_EDITOR
//	static public void AddArtifact(ArtifactProtoData data) 
//	{
//		if (ArtifactStore.Artifacts == null)  { return; }
//		ArtifactStore.Artifacts.Add(data);
//	}
//	
//	static public void RemoveArtifact(ArtifactProtoData data) 
//	{
//		if (ArtifactStore.Artifacts == null) { return; }
//		ArtifactStore.Artifacts.Remove(data);
//	}
//#endif


	//{ ArtifactStore.Artifacts = new List<ArtifactProtoData>(); }
	
//	
//	static public void ComputeProgressions() 
//	{
//		if (Progressions == null) 
//		{
//			Progressions = new Dictionary<int, List<int>>();
//		}
//		else { Progressions.Clear(); }		
//		
//		foreach(ArtifactProtoData data in Artifacts) 
//		{
//			if(data._progressionID == -1)
//				continue;
//			List<int> progression = null;
//			if(Progressions.TryGetValue(data._progressionID, out progression) == true)
//			{
//				if(progression.Contains(data._id) == false) {
//					progression.Add (data._id);
//				}
//			}
//			else {
//				progression = new List<int>();
//				progression.Add (data._id);
//				Progressions.Add (data._progressionID, progression);
//			}
//		}
//		
//		//-- Now sort the progression by Rank, then cost.
//		foreach (KeyValuePair<int, List<int>> prog in Progressions) 
//		{
//			prog.Value.Sort (delegate(int a, int b) 
//			{
//				ArtifactProtoData aData = GetArtifactProtoData(a);
//				ArtifactProtoData bData = GetArtifactProtoData(b);
//				if (aData._requiredRank == bData._requiredRank) 
//				{
//					if (aData._cost == bData._cost)
//					{
//						return aData._statValue.CompareTo(bData._statValue);
//					}
//					return aData._cost.CompareTo(bData._cost);
//				}
//				return aData._requiredRank.CompareTo(bData._requiredRank);
//			});
//			
//			//-- Now set the next artifact ID in each Aritfact.
//			int max = prog.Value.Count;
//			for (int i = 0; i < max; i++) 
//			{
//				int artifactID = prog.Value[i];
//				int nextID = (i == (max-1)) ? artifactID : prog.Value[i+1];
//				
//				ArtifactProtoData data = GetArtifactProtoData(artifactID);
//				if (data == null) { continue; }
//				data._nextArtifactIDInProgression = nextID;
//			}
//		}
//	}
	
	//static public Dictionary<int, List<int>> getProgression()  { return Progressions; }

		
//		//-- Add to the progression list if one is specified.
//		if (data._progressionID == -1) { return; }
//		
//		List<int> progression = null;
//		if (Progressions.TryGetValue(data._progressionID, out progression) == true)
//		{
//			if (progression.Contains(data._id) == false) 
//			{
//				progression.Add (data._id);
//			}
//		}



	
//	static public List<int> GetProgressionFromArtifact(int artifactID) 
//	{
//		ArtifactProtoData data = GetArtifactProtoData(artifactID);
//		if(data == null)
//			return null;
//		if(data._progressionID == -1)
//			return null;
//		List<int> progression = null;
//		
//		Progressions.TryGetValue(data._progressionID, out progression);
//		return progression;
//	}
	

	
//	public static Color colorForRarity(ItemRarity rarity) {
//		switch(rarity) {
//		case ItemRarity.Common:
//			return Color.gray;
//		case ItemRarity.Uncommon:
//			return new Color(155.0f/255.0f, 235.0f/255.0f, 140.0f/255.0f);
//		case ItemRarity.Rare:
//			return new Color(0.0f/255.0f, 122.0f/255.0f, 255.0f/255.0f);
//		case ItemRarity.Epic:
//			return new Color(224.0f/255.0f, 95.0f/255.0f, 255.0f/255.0f);
//		case ItemRarity.Legendary:
//			return new Color(1.00f, 0.51f, 0.07f);
//		default:
//			return Color.gray;
//		}
//	}
	


	//fullStore.Add ("progressions", Progressions);	


	
	//public static Dictionary<int, List<int>> Progressions = new Dictionary<int, List<int>>();
	//private static List< List<int> > Progressions = new List<List<int>>();
	



			
		//if (Progressions == null) { Progressions = new Dictionary<int, List<int>>(); }
		//else { Progressions.Clear(); }		
			
			
//			object progressionObject = null;
//			if(fullStore.TryGetValue("progressions", out progressionObject) == true) 
//			{
//				Dictionary<string, object> readInProgressions = progressionObject as Dictionary<string, object>;
//				if(readInProgressions != null) 
//				{
//					foreach (KeyValuePair<string, object> item in readInProgressions) 
//					{
//						List<int> ids = new List<int>();
//						foreach(object o in item.Value as List<object>) 
//						{
//							ids.Add(Convert.ToInt32(o));
//						}
//						Progressions.Add(Int32.Parse(item.Key), ids);
//					}
//				}
//			}