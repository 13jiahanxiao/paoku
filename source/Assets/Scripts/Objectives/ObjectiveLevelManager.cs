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

public class ObjectiveLevelManager
{
	protected static Notify notify = new Notify("ObjectiveLevelManager");
	public static List<ObjectiveLevel> objectiveLevelsList = new List<ObjectiveLevel>();	
	
	public static int GetDifficulty(int currentLevel, int objectiveNumber)	// completedObjectives)
	{
		notify.Debug("objectiveNumber = " + objectiveNumber.ToString());
		
		if (currentLevel+1 >= objectiveLevelsList.Count)
		{
			return -999;	// done with all objective levels, so not giving out any more objectives
		}
		
		int levelToPullFrom = currentLevel + 1;
		
		if (objectiveNumber == 0)
		{
			levelToPullFrom = currentLevel;	// if level just completed, we're actually refilling the third objective from the previous level
			objectiveNumber = 3;
		}
		
		List<int> difficulties = objectiveLevelsList[levelToPullFrom].difficulties;
		
		int total = 0;	//int diff = 1;
		
		for (int i=1; i<8; i++)
		{
			total += difficulties[i];
			
			if (objectiveNumber <= total)
			{
				notify.Debug("Returning objective difficulty: " + i.ToString() 
					+ " for objectiveNumber: " + objectiveNumber.ToString()
					+ " for levelToPullFrom: " + (levelToPullFrom).ToString());
				
				return i;	// return difficulty value
			}
		}
		
		return 0;	// should never get here
	}
	
	public static bool LoadFile()
	{
		objectiveLevelsList.Clear();
		
		TextAsset jsonText = Resources.Load("OZGameData/ObjectiveLevels") as TextAsset;
		//notify.Debug("ObjectiveLevelManager " + jsonText.text);
		Dictionary<string, object> loadedData = MiniJSON.Json.Deserialize(jsonText.text) as Dictionary<string, object>;	//-- Security check
		List<object> objectiveLevels = loadedData["data"] as List<object>;
		
		foreach (object dict in objectiveLevels)			
		{
			Dictionary<string, object> data = dict as Dictionary<string, object>;
			ObjectiveLevel newObjectiveLevel = new ObjectiveLevel();
			newObjectiveLevel.SetDataFromDictionary(data);
			objectiveLevelsList.Add(newObjectiveLevel);	
		}
		return true;
	}
	
	public static void SaveFile()
	{
		string fileName = Application.dataPath + Path.DirectorySeparatorChar + "Resources" + Path.DirectorySeparatorChar + "OZGameData/ObjectiveLevels.txt";
		List<object> list = new List<object>();
		foreach (ObjectiveLevel objectiveLevel in objectiveLevelsList) 
		{ 
			list.Add(objectiveLevel.ToDict()); 
		}
		
		//-- Hash before we save.
		Dictionary<string, object> secureData = SaveLoad.Save(list);
		string listString = MiniJSON.Json.Serialize(secureData);
		try 
		{
			using (StreamWriter fileWriter = File.CreateText(fileName)) { fileWriter.WriteLine(listString); fileWriter.Close(); }
		}
		catch (Exception e) 
		{
			Dictionary<string,string> d = new Dictionary<string, string>();
			d.Add("Exception",e.ToString());
			notify.Debug("Save Exception: " + e);
		}
	}
	
#if UNITY_EDITOR	
	public static int GetNextObjectiveLevelID() 
	{
		int nextID = 0;
		foreach (ObjectiveLevel objectiveLevel in objectiveLevelsList) { nextID = objectiveLevel.ID + 1; }
		return nextID;
	}
#endif
}




//	public static int GetDifficulty(int currentLevel)
//	{
//		ObjectiveLevel thisLevel = objectiveLevelsList[currentLevel];
//		List<int> difficulties = thisLevel.difficulties;			//new List<int>();
//		List<float> probabilities = new List<float>();
//		List<float> ranges = new List<float>();
//		int total = 0;
//		int diffLevels = 8;
//		
//		for (int i=0; i<diffLevels; i++)							// add up total of values for all difficulty levels
//			total += difficulties[i];
//		
//		for (int i=0; i<diffLevels; i++)							// normalize probabilities for each difficulty between 0.0f and 1.0f
//			probabilities.Add((float)difficulties[i] / (float)total);
//		
//		for (int i=0; i<diffLevels; i++)							// set up ranges for each difficulty, for random choice
//		{
//			if (i == 0) { ranges.Add(probabilities[i]); }
//			else { ranges.Add(probabilities[i] + probabilities[i-1]); }
//		}		
//		
//		float randVal = Rand.GetRandomFloat(0.0f, 1.0f);			// get random float between 0.0f and 1.0f (including 0.0f but not 1.0f)
//		
//		for (int i=0; i<diffLevels; i++)							// find difficulty level based on chosen random float value
//		{
//			if (randVal <= ranges[i])
//				return i;
//		}		
//		
//		return 1;													// choose easiest difficulty, for now
//	}


//	
//	static public void AddObjectiveLevel(ObjectiveLevel data) 
//	{
//		if (ObjectiveLevelManager.objectiveLevelsList == null) { return; }
//		ObjectiveLevelManager.objectiveLevelsList.Add(data);
//	}
//	
//	static public void RemoveObjectiveLevel(ObjectiveLevel data) 
//	{
//		if (ObjectiveLevelManager.objectiveLevelsList == null) { return; }
//		ObjectiveLevelManager.objectiveLevelsList.Remove(data);
//	}	
	
	//private static Rand rand = null;
		//if (rand == null) rand = Services.Get<Rand>();			// get reference to random number generator
			

		
		// ugly manual conversion, for now
//		difficulties.Add(thisLevel.Difficulty0);		
//		difficulties.Add(thisLevel.Difficulty1);
//		difficulties.Add(thisLevel.Difficulty2);
//		difficulties.Add(thisLevel.Difficulty3);
//		difficulties.Add(thisLevel.Difficulty4);
//		difficulties.Add(thisLevel.Difficulty5);
//		difficulties.Add(thisLevel.Difficulty6);
//		difficulties.Add(thisLevel.Difficulty7);