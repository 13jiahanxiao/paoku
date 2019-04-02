using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class GameProfileEditor : MonoBehaviour {

//	[MenuItem("TR2/Game Profile/Reset")]
//	public static void reset()
//	{
//		GameProfile gp = FindObjectOfType(typeof(GameProfile)) as GameProfile;
//		if(gp) {
//			GameProfile.SharedInstance = gp;
//		}
//		else {
//			TR.ERROR ("ERROR: Couldn't find the GameProfile Object in the Scene");
//			return;
//		}
//		
//		if(GameProfile.SharedInstance.Characters != null) {
//			GameProfile.SharedInstance.Characters.Clear ();	
//			GameProfile.SharedInstance.SetupDefaultCharacters();
//		}
//		
//		GameProfile.SharedInstance.Reset();
//	}
	protected static Notify notify;
	
	public void Awake()
	{
		notify = new Notify(this.GetType().Name);
	}
	
	[MenuItem("TR2/Game Profile/Reset Everything")]
	public static void resetEverything()
	{	
		if(Application.isPlaying == true) {
			notify.Error("GAME SHOULD NOT BE RUNNING DURING RESET EVERYTHING!");
		}
		
		PlayerPrefs.DeleteAll();
		System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo(Application.persistentDataPath);
		
		foreach (FileInfo file in downloadedMessageInfo.GetFiles())
		{
		    file.Delete(); 
		}
		foreach (DirectoryInfo dir in downloadedMessageInfo.GetDirectories())
		{
		    dir.Delete(true); 
		}
		
		
		// wxj ,reset activity objecitves
		List<ObjectiveProtoData> dataList = new List<ObjectiveProtoData>();
		ObjectivesManager.LoadFile(dataList, "OZGameData/ObjectivesActivity");
		foreach(ObjectiveProtoData data in dataList)
		{
			ObjectivesManager.resetActivityObjective(data);
		}
		ObjectivesManager.SaveFile(dataList, "OZGameData/ObjectivesActivity");
		
	}
	
	//[MenuItem("TR2/Game Profile/Reset and Save")]
	// no longer a menu item, as Bryant's backup player profile code kicks and and you don't really reset
	public static void resetAndSave()
	{
		// game is probably not running, hence using Debug.Log
		Debug.Log ("Application.persistentDataPath = " + Application.persistentDataPath);	
		if(Application.isPlaying == true) {
			notify.Error("GAME SHOULD NOT BE RUNNING DURING RESET AND SAVE, KEITH!");
		}
		
		PlayerPrefs.DeleteAll();
		
		GameProfile gp = FindObjectOfType(typeof(GameProfile)) as GameProfile;
		if(gp) {
			GameProfile.SharedInstance = gp;
		}
		else {
			notify.Warning ("ERROR: Couldn't find the GameProfile Object in the Scene");
			return;
		}
		
		if(GameProfile.SharedInstance.Characters != null) {
			GameProfile.SharedInstance.Characters.Clear ();	
			GameProfile.SharedInstance.SetupDefaultCharacters();
		}
		
		GameProfile.SharedInstance.Reset();
		GameProfile.SharedInstance.Serialize();
		
		AppCounters.ResetCountersInPlayerPrefs();
		NotificationSystem.ResetClearedNotificationsInPlayerPrefs();
		MenuTutorials.ResetMenuTutorialsInPlayerPrefs();
		
		//Destory weekly objectives stored file
		string fileName = Application.persistentDataPath + Path.DirectorySeparatorChar + 
			"weeklyobject.txt";
		
		FileUtil.DeleteFileOrDirectory(fileName);
		
	}
}
