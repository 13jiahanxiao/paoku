using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;

public class ObjectiveLevelEditor : EditorWindow
{
	protected static Notify notify;
	Vector2 mScroll = Vector2.zero;
	bool mSaveNeeded = false;

	[MenuItem("TR2/Editors/Objective Level Editor")]
	static public void OpenObjectiveLevelEditor() 
	{ 
		EditorWindow.GetWindow<ObjectiveLevelEditor>(false, "Objective Level Editor", true); 
	}
	
	static void SaveString (string field, string val)
	{
		if (string.IsNullOrEmpty(val)) { EditorPrefs.DeleteKey(field); }
		else { EditorPrefs.SetString(field, val); }
	}
	
	void Awake()
	{
		notify = new Notify(this.GetType().Name);
	}
	
	void OnGUI()
	{
		//-- Load the file if its not yet loaded.
		if (ObjectiveLevelManager.objectiveLevelsList == null || ObjectiveLevelManager.objectiveLevelsList.Count == 0) 
		{
			LoadFile();
		}		
		
		EditorGUIUtility.LookLikeControls(80f);
		GUILayout.Space(6f);
		
		//-- Top Panel
		GUI.contentColor = Color.white;
		
		GUILayout.BeginHorizontal();
		
		if (GUILayout.Button("Add") == true)
		{
			ObjectiveLevel objLevel = new ObjectiveLevel();
			objLevel.ID = ObjectiveLevelManager.GetNextObjectiveLevelID();
			objLevel._showFoldOut = true;
			ObjectiveLevelManager.objectiveLevelsList.Add(objLevel);
			mSaveNeeded = true;
		}
		
		GUI.contentColor = mSaveNeeded ? Color.yellow: Color.white;
		if (GUILayout.Button(mSaveNeeded ? "Save*" : "Save") == true)
		{
			ObjectiveLevelManager.SaveFile();
			mSaveNeeded = false;
			AssetDatabase.Refresh();
		}
		
		GUI.contentColor = Color.white;
		if(GUILayout.Button("Reload") == true)
		{
			AssetDatabase.Refresh();
			LoadFile();	//ObjectiveLevelManager.LoadFile();
			mSaveNeeded = false;
		}
		
		GUILayout.EndHorizontal();
		NGUIEditorTools.DrawSeparator();
		
		DisplayObjectiveLevels();
	}

	private void DisplayObjectiveLevels() 	
	{		
		//-- List Panel
		mScroll = EditorGUILayout.BeginScrollView(mScroll);
		
		foreach (ObjectiveLevel data in ObjectiveLevelManager.objectiveLevelsList) 
		{
			if (data.ID == 0) { continue; }		// don't show data for level 0, even though it's there for simplicity's sake
			
			EditorGUI.indentLevel = 0;
			GUILayout.BeginHorizontal();
			data._showFoldOut = EditorGUILayout.Foldout(data._showFoldOut, " (Objective Level ID = " + data.ID + ")");
			GUILayout.FlexibleSpace();
			Color preBackColor = GUI.backgroundColor;
			
			if (data._showFoldOut == true)
			{			
				GUI.backgroundColor = Color.red;
				if (GUILayout.Button("x")) 
				{
					bool delete = EditorUtility.DisplayDialog("Deleting Objective Level", "Are you sure you want to delete objective level "
						+ data.ID + "?", "Delete", "Do Not Delete");
					if (delete) 
					{
						ObjectiveLevelManager.objectiveLevelsList.Remove(data);
						GUILayout.EndHorizontal();
						return;	
					}
				}
				GUI.backgroundColor = preBackColor;
			}
			
			GUILayout.EndHorizontal();
			
			if (data._showFoldOut == false) { continue; }
			
			EditorGUI.indentLevel = 0;
			GUI.backgroundColor = preBackColor;
			GUI.contentColor = Color.white;
			
			EditorGUI.indentLevel = 0;
			int tempInt = (int)EditorGUILayout.IntField("Objs Req'd", (int)data.ObjectivesRequired);
			if (tempInt != data.ObjectivesRequired) 
			{ 
				data.ObjectivesRequired = tempInt;
				mSaveNeeded = true; 
			}	
	
			RankRewardType tempRewardType = (RankRewardType)EditorGUILayout.EnumPopup("Reward Type", data.rewardType);
			if (tempRewardType != data.rewardType)
			{
				data.rewardType = tempRewardType;
				mSaveNeeded = true;
			}		
		
			tempInt = (int)EditorGUILayout.IntField("Reward Value", (int)data.rewardValue);
			if (tempInt != data.rewardValue) 
			{
				data.rewardValue = tempInt; 
				mSaveNeeded = true; 
			}			
			
			string tempString = (string)EditorGUILayout.TextField("Icon", (data.iconImage != null) ? data.iconImage : "");
			if (tempString.CompareTo(data.iconImage) != 0) 
			{
				data.iconImage = tempString;
				mSaveNeeded = true;
			}
				
			tempInt = (int)EditorGUILayout.IntField("Difficulty 1", (int)data.difficulties[1]);
			if (tempInt != data.difficulties[1]) 
			{
				data.difficulties[1] = tempInt; 
				mSaveNeeded = true; 
			}
			
			tempInt = (int)EditorGUILayout.IntField("Difficulty 2", (int)data.difficulties[2]);
			if (tempInt != data.difficulties[2]) 
			{ 
				data.difficulties[2] = tempInt; 
				mSaveNeeded = true; 
			}
			
			tempInt = (int)EditorGUILayout.IntField("Difficulty 3", (int)data.difficulties[3]);
			if (tempInt != data.difficulties[3]) 
			{ 
				data.difficulties[3] = tempInt;
				mSaveNeeded = true; 
			}
			
			tempInt = (int)EditorGUILayout.IntField("Difficulty 4", (int)data.difficulties[4]);
			if (tempInt != data.difficulties[4]) 
			{ 
				data.difficulties[4] = tempInt; 
				mSaveNeeded = true;
			}

			tempInt = (int)EditorGUILayout.IntField("Difficulty 5", (int)data.difficulties[5]);
			if (tempInt != data.difficulties[5]) 
			{ 
				data.difficulties[5] = tempInt;
				mSaveNeeded = true; 
			}			
			
			tempInt = (int)EditorGUILayout.IntField("Difficulty 6", (int)data.difficulties[6]);
			if (tempInt != data.difficulties[6]) 
			{ 
				data.difficulties[6] = tempInt;
				mSaveNeeded = true; 
			}			
			
			tempInt = (int)EditorGUILayout.IntField("Difficulty 7", (int)data.difficulties[7]);
			if (tempInt != data.difficulties[7]) 
			{ 
				data.difficulties[7] = tempInt;
				mSaveNeeded = true; 
			}			
						
			EditorGUILayout.Space();
		}
		
		EditorGUILayout.EndScrollView();
	}
		
	private void LoadFile()
	{	
		bool success = ObjectiveLevelManager.LoadFile();
		if (success == false)
		{
			GUILayout.Label("Failed to load ObjectiveLevels.txt");
			notify.Warning("Failed to load ObjectiveLevels.txt");
			return;
		}
		else
		{
			ObjectiveLevelManager.objectiveLevelsList.Sort((a1, a2) => a1.ID.CompareTo(a2.ID));
		}
	}
}



//			
//			tempString = (string)EditorGUILayout.TextField("Description", (data.description != null) ? data.description : "");
//			if (tempString.CompareTo(data.description) != 0) 
//			{
//				data.description = tempString;
//				mSaveNeeded = true;
//			}	

			
//			tempInt = (int)EditorGUILayout.IntField("Difficulty 0", (int)data.difficulties[0]);
//			if (tempInt != data.difficulties[0]) 
//			{
//				data.difficulties[0] = tempInt; 
//				mSaveNeeded = true; 
//			}		

//	
//    static void Init() 
//	{
//		//bool success = ObjectiveLevelManager.LoadFile();	
//		//if (success == false) { TR.LOG("Failed to load ObjectiveLevels.txt"); }
//    }	
