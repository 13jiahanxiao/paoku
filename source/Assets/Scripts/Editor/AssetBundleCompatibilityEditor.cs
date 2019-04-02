using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;

public class AssetBundleCompatibilityEditor : EditorWindow
{
	protected static Notify notify;
	Vector2 mScroll = Vector2.zero;
	bool mSaveNeeded = false;
	
	[MenuItem("TR2/Editors/Asset Bundle Compatibility Editor")]
	static public void OpenAssetBundleCompatibilityEditor()
	{
		EditorWindow.GetWindow<AssetBundleCompatibilityEditor>(false, "Asset Bundle Compatibility Editor", true);
	}
	
	void Awake()
	{
		notify = new Notify(this.GetType().Name);	
	}

	void OnGUI()
	{
		//-- Load the file if its not yet loaded.
		if (AssetBundleCompatibilityManager.Entries == null || AssetBundleCompatibilityManager.Entries.Count == 0) 
		{
			bool success =  AssetBundleCompatibilityManager.LoadFile();
			if (success == false) 
			{
				GUILayout.Label("Failed to load AssetBundleCompatibility.txt");
				notify.Debug("Failed to load AssetBundleCompatibility.txt");
				return;
			}
		}
				
		EditorGUIUtility.LookLikeControls(80f);
		GUILayout.Space(6f);
		
		//-- Top Panel
		GUI.contentColor = Color.white;
		
		GUILayout.BeginHorizontal();
		
		if (GUILayout.Button("Add") == true)
		{
			AssetBundleCompatibilityEntry entry = new AssetBundleCompatibilityEntry();
			entry._showFoldOut = true;
			AssetBundleCompatibilityManager.Entries.Add(entry);
		}
		
		string saveString = "Save";
		if (mSaveNeeded == true) 
		{
			saveString = "Save*";
			GUI.contentColor = Color.yellow;
		}
		
		if (GUILayout.Button(saveString) == true)
		{
			AssetBundleCompatibilityManager.SaveFile();
			mSaveNeeded = false;
			AssetDatabase.Refresh();
		}
		
		GUI.contentColor = Color.white;
		if (GUILayout.Button("Reload") == true)
		{
			AssetDatabase.Refresh();
			AssetBundleCompatibilityManager.LoadFile();
			mSaveNeeded = false;
		}
		
		GUILayout.EndHorizontal();
		
		NGUIEditorTools.DrawSeparator();
	
		DisplayAssetBundleCompatibility();
	}
		
	private void DisplayAssetBundleCompatibility() 		
	{
		//-- List Panel
		mScroll = EditorGUILayout.BeginScrollView(mScroll);
		
		foreach (AssetBundleCompatibilityEntry data in AssetBundleCompatibilityManager.Entries)
		{
			EditorGUI.indentLevel = 0;
			GUILayout.BeginHorizontal();
			
			//string environmentName = EnvironmentSetManager.SharedInstance.AllDict[Int32.Parse(data.environmentID)].Title;
			data._showFoldOut = EditorGUILayout.Foldout(data._showFoldOut, 
				"(client version: " + data.clientVersion + ",   asset bundle name: " + data.assetBundleName + ")");	//environmentName + ")");
			GUILayout.FlexibleSpace();
			Color preBackColor = GUI.backgroundColor;
						
			if (data._showFoldOut == true)
			{			
				GUI.backgroundColor = Color.red;
				if (GUILayout.Button("x")) 
				{
					bool delete = EditorUtility.DisplayDialog("Deleting Asset Bundle Compatibility Entry", 
						"Are you sure you want to delete this Asset Bundle Compatibility Entry?", "Delete", "Do Not Delete");
					if (delete) 
					{
						AssetBundleCompatibilityManager.Entries.Remove(data);
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

			string tempString = (string)EditorGUILayout.TextField("Client Version", (data.clientVersion != null) ? data.clientVersion : "");
			if (tempString.CompareTo(data.clientVersion) != 0) 
			{
				data.clientVersion = tempString;
				mSaveNeeded = true;
			}			

			tempString = (string)EditorGUILayout.TextField("Bundle Name", (data.assetBundleName != null) ? data.assetBundleName : "");
			if (tempString.CompareTo(data.assetBundleName) != 0) 
			{
				data.assetBundleName = tempString;
				mSaveNeeded = true;
			}	
			
			tempString = (string)EditorGUILayout.TextField("Ver. To Use", (data.assetBundleVersionToUse != null) ? data.assetBundleVersionToUse : "");
			if (tempString.CompareTo(data.assetBundleVersionToUse) != 0) 
			{
				data.assetBundleVersionToUse = tempString;
				mSaveNeeded = true;
			}			
			
			EditorGUILayout.Space();
		}
		
		EditorGUILayout.EndScrollView();
	}
}



			//gt.id = GameTipManager.GetNextGameTipID();
