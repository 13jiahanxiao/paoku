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

public class GameTipEditor : EditorWindow
{
	protected static Notify notify;
	Vector2 mScroll = Vector2.zero;
	bool mSaveNeeded = false;
	
	[MenuItem("TR2/Editors/GameTip Editor")]
	static public void OpenGameTipEditor()
	{
		EditorWindow.GetWindow<GameTipEditor>(false, "GameTip Editor", true);
	}
	
	void Awake()
	{
		notify = new Notify(this.GetType().Name);	
	}

	void OnGUI()
	{
		//-- Load the file if its not yet loaded.
		if (GameTipManager.GameTips == null || GameTipManager.GameTips.Count == 0) 
		{
			bool success =  GameTipManager.LoadFile();
			if (success == false) 
			{
				GUILayout.Label("Failed to load GameTips.txt");
				notify.Debug("Failed to load GameTips.txt");
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
			GameTip gt = new GameTip();
			gt.id = GameTipManager.GetNextGameTipID();
			gt._showFoldOut = true;
			GameTipManager.GameTips.Add(gt);
		}
		
		string saveString = "Save";
		if (mSaveNeeded == true) 
		{
			saveString = "Save*";
			GUI.contentColor = Color.yellow;
		}
		
		if (GUILayout.Button(saveString) == true)
		{
			GameTipManager.SaveFile();
			mSaveNeeded = false;
			AssetDatabase.Refresh();
		}
		
		GUI.contentColor = Color.white;
		if (GUILayout.Button("Reload") == true)
		{
			AssetDatabase.Refresh();
			GameTipManager.LoadFile();
			mSaveNeeded = false;
		}
		
		GUILayout.EndHorizontal();
		
		NGUIEditorTools.DrawSeparator();
	
		DisplayGameTips();
	}
		
	private void DisplayGameTips() 		
	{
		//-- List Panel
		mScroll = EditorGUILayout.BeginScrollView(mScroll);
		
		foreach (GameTip data in GameTipManager.GameTips)
		{
			EditorGUI.indentLevel = 0;
			GUILayout.BeginHorizontal();
			data._showFoldOut = EditorGUILayout.Foldout(data._showFoldOut, data.title + " (ID=" + data.id + ")");
			GUILayout.FlexibleSpace();
			Color preBackColor = GUI.backgroundColor;
						
			if (data._showFoldOut == true)
			{			
				GUI.backgroundColor = Color.red;
				if (GUILayout.Button("x")) 
				{
					bool delete = EditorUtility.DisplayDialog("Deleting GameTip", "Are you sure you want to delete the "
						+ data.title + " GameTip", "Delete", "Do Not Delete");
					if (delete) 
					{
						GameTipManager.GameTips.Remove(data);
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

			string tempString = (string)EditorGUILayout.TextField("Title Key", (data.title != null) ? data.title : "");
			if (tempString.CompareTo(data.title) != 0) 
			{
				data.title = tempString;
				mSaveNeeded = true;
			}			

			tempString = (string)EditorGUILayout.TextField("Tip Key", (data.tip != null) ? data.tip : "");
			if (tempString.CompareTo(data.tip) != 0) 
			{
				data.tip = tempString;
				mSaveNeeded = true;
			}			
			EditorGUILayout.Space();
		}
		
		EditorGUILayout.EndScrollView();
	}
}