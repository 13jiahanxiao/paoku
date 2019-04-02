using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.IO;

public class WeeklyFileEditor : MonoBehaviour {

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
	
	[MenuItem("TR2/Game Profile/Destroy Weekly Challenge Cache")]
	public static void resetAndSave()
	{

		
		//Destory weekly objectives stored file
		string fileName = Application.persistentDataPath + Path.DirectorySeparatorChar + 
			"weeklyobject.txt";
		FileUtil.DeleteFileOrDirectory(fileName);
		
		fileName = Application.persistentDataPath + Path.DirectorySeparatorChar + 
			"weeklydiscount.txt";
		FileUtil.DeleteFileOrDirectory(fileName);
		
		fileName = Application.persistentDataPath + Path.DirectorySeparatorChar +
			"passedneighbor.txt";
		FileUtil.DeleteFileOrDirectory(fileName);
		
	}
	
	
}
