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

public class StoreEditor : EditorWindow
{
	protected static Notify notify;
	Vector2 mScroll = Vector2.zero;
	bool mSaveNeeded = false;
	
	[MenuItem("TR2/Editors/Store Editor")]
	static public void OpenStoreEditor()
	{
		EditorWindow.GetWindow<StoreEditor>(false, "Store Editor", true);
	}
	
	void Awake()
	{
		notify = new Notify(this.GetType().Name);	
	}
	
	void OnGUI()
	{
		//-- Load the file if its not yet loaded.
		if (Store.StoreItems == null || Store.StoreItems.Count == 0) 
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
			StoreItem si = new StoreItem();
			si.id = Store.GetNextStoreItemID();
			si._showFoldOut = true;
			Store.StoreItems.Add(si);
		}
		
		string saveString = "Save";
		if (mSaveNeeded == true) 
		{
			saveString = "Save*";
			GUI.contentColor = Color.yellow;
		}
		
		if (GUILayout.Button(saveString) == true)
		{
			Store.SaveFile();
			mSaveNeeded = false;
			AssetDatabase.Refresh();
		}
		
		GUI.contentColor = Color.white;
		if (GUILayout.Button("Reload") == true)
		{
			AssetDatabase.Refresh();
			LoadFile();	//Store.LoadFile();
			mSaveNeeded = false;
		}
		
		GUILayout.EndHorizontal();
		
		NGUIEditorTools.DrawSeparator();
	
		DisplayStoreItems();
	}

	private void DisplayStoreItems() 		
	{
		//-- List Panel
		mScroll = EditorGUILayout.BeginScrollView(mScroll);
		
		foreach (StoreItem data in Store.StoreItems) 
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
					bool delete = EditorUtility.DisplayDialog("Deleting Store Item", "Are you sure you want to delete the "
						+ data.title + " store item?", "Delete", "Do Not Delete");
					if (delete) 
					{
						Store.StoreItems.Remove(data);
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

			tempString = (string)EditorGUILayout.TextField("Desc Key", (data.description != null) ? data.description : "");
			if (tempString.CompareTo(data.description) != 0) 
			{
				data.description = tempString;
				mSaveNeeded = true;
			}			
			
			tempString = (string)EditorGUILayout.TextField("Icon", (data.icon != null) ? data.icon : "");
			if (tempString.CompareTo(data.icon) != 0) 
			{
				data.icon = tempString;
				mSaveNeeded = true;
			}

			CostType tempCostType = (CostType)EditorGUILayout.EnumPopup("Cost Type", data.costType);
			if (tempCostType != data.costType && tempCostType != CostType.Total) 
			{
				data.costType = tempCostType;
				mSaveNeeded = true;
			}	
			
			int tempInt = (int)EditorGUILayout.IntField("Cost", (int)data.cost);
			if (tempInt != data.cost) 
			{
				data.cost = tempInt;
				mSaveNeeded = true;
			}
			
			tempInt = (int)EditorGUILayout.IntField("MrkDn Cost", (int)data.markDownCost);
			if (tempInt != data.markDownCost) 
			{
				data.markDownCost = tempInt;
				mSaveNeeded = true;
			}					
		
			StoreItemType tempStoreItemType = (StoreItemType)EditorGUILayout.EnumPopup("Item Type", data.itemType);
			if (tempStoreItemType != data.itemType) 
			{
				data.itemType = tempStoreItemType;
				mSaveNeeded = true;
			}					
			
			tempInt = (int)EditorGUILayout.IntField("Item Quantity", (int)data.itemQuantity);
			if (tempInt != data.itemQuantity) 
			{
				data.itemQuantity = tempInt;
				mSaveNeeded = true;
			}					
			
			tempInt = (int)EditorGUILayout.IntField("Sort Priority", (int)data.sortPriority);
			if (tempInt != data.sortPriority) 
			{
				data.sortPriority = tempInt;
				mSaveNeeded = true;
			}
			
			EditorGUILayout.Space();
		}
		
		EditorGUILayout.EndScrollView();
	}
			
	private void LoadFile()
	{
		bool success = Store.LoadFile();
		if (success == false) 
		{
			GUILayout.Label("Failed to load StoreItems.txt");
			notify.Debug("Failed to load StoreItems.txt");
			return;
		}
		else
		{
			Store.StoreItems.Sort((a1, a2) => a1.id.CompareTo(a2.id));
		}			
	}
}



				
//			string tempString = (string)EditorGUILayout.TextField("Int. Title", (data.internalTitle != null) ? data.internalTitle : "");
//			if (tempString.CompareTo(data.internalTitle) != 0) 
//			{
//				data.internalTitle = tempString;
//				mSaveNeeded = true;
//			}					
			