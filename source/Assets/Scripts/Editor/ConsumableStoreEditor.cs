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

public class ConsumableStoreEditor : EditorWindow
{
	protected static Notify notify;
	Vector2 mScroll = Vector2.zero;
	bool mSaveNeeded = false;
	
	[MenuItem("TR2/Editors/Consumable Editor")]
	static public void OpenConsumableStoreEditor() 
	{ 
		EditorWindow.GetWindow<ConsumableStoreEditor>(false, "Consumable Editor", true); 
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
		if (ConsumableStore.consumablesList == null || ConsumableStore.consumablesList.Count == 0) 
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
			BaseConsumable consumable = new BaseConsumable();
			consumable.PID = ConsumableStore.GetNextConsumableID();
			consumable._showFoldOut = true;
			ConsumableStore.consumablesList.Add(consumable);
		}
		
		GUI.contentColor = mSaveNeeded ? Color.yellow: Color.white;
		if (GUILayout.Button(mSaveNeeded ? "Save*" : "Save") == true)
		{
			ConsumableStore.SaveFile();
			mSaveNeeded = false;
			AssetDatabase.Refresh();
		}
		
		GUI.contentColor = Color.white;
		if(GUILayout.Button("Reload") == true)
		{
			AssetDatabase.Refresh();
			LoadFile();	//ConsumableStore.LoadFile();
			mSaveNeeded = false;
		}
		
		GUILayout.EndHorizontal();
		NGUIEditorTools.DrawSeparator();
		
		DisplayConsumables();
	}

	private void DisplayConsumables() 	
	{
		//-- List Panel
		mScroll = EditorGUILayout.BeginScrollView(mScroll);
		
		foreach (BaseConsumable data in ConsumableStore.consumablesList) 
		{
			EditorGUI.indentLevel = 0;
			GUILayout.BeginHorizontal();
			data._showFoldOut = EditorGUILayout.Foldout(data._showFoldOut, data.Title + " (ID=" + data.PID + ")");
			GUILayout.FlexibleSpace();
			Color preBackColor = GUI.backgroundColor;
						
			if (data._showFoldOut == true)
			{			
				GUI.backgroundColor = Color.red;
				if (GUILayout.Button("x")) 
				{
					bool delete = EditorUtility.DisplayDialog("Deleting Consumable", "Are you sure you want to delete the "
						+ data.Title + " consumable?", "Delete", "Do Not Delete");
					if (delete) 
					{
						ConsumableStore.consumablesList.Remove(data);
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
			string tempString = (string)EditorGUILayout.TextField("Title Key", (data.Title != null) ? data.Title : "");
			if (tempString.CompareTo(data.Title) != 0) 
			{
				data.Title = tempString;
				mSaveNeeded = true;
			}
		
			tempString = (string)EditorGUILayout.TextField("Subtitle Key", (data.Subtitle != null) ? data.Subtitle : "");
			if (tempString.CompareTo(data.Subtitle) != 0) 
			{
				data.Subtitle = tempString;
				mSaveNeeded = true;
			}				
			
			tempString = (string)EditorGUILayout.TextField("Desc Key", (data.Description != null) ? data.Description : "");
			if (tempString.CompareTo(data.Description) != 0) 
			{
				data.Description = tempString;
				mSaveNeeded = true;
			}			
			
			tempString = (string)EditorGUILayout.TextField("Icon Name", (data.IconName != null) ? data.IconName : "");
			if (tempString.CompareTo(data.IconName) != 0) 
			{
				data.IconName = tempString;
				mSaveNeeded = true;
			}
			
			int tempInt = (int)EditorGUILayout.IntField("Cost", (int)data.Cost);
			if (tempInt != data.Cost) 
			{
				data.Cost = tempInt;
				mSaveNeeded = true;
			}
			
			CostType tempCostType = (CostType)EditorGUILayout.EnumPopup("Cost Type", data.CostType);
			if (tempCostType != data.CostType && tempCostType != CostType.Total) 
			{
				data.CostType = tempCostType;
				mSaveNeeded = true;
			}
			
			ConsumableType dataType = ConsumableType.BaseConsumable;
			dataType = (ConsumableType)Enum.Parse(dataType.GetType(), data.Type);
			ConsumableType tempConsumableType = (ConsumableType)EditorGUILayout.EnumPopup("Type", dataType);
			if (tempConsumableType != dataType && tempConsumableType != ConsumableType.BaseConsumable) 
			{
				data.Type = tempConsumableType.ToString();
				mSaveNeeded = true;
			}
			
			float tempFloat = (float)EditorGUILayout.FloatField("Value", (float)data.Value);
			if (tempFloat != data.Value) 
			{
				data.Value = tempFloat;
				mSaveNeeded = true;
			}

			tempFloat = (float)EditorGUILayout.FloatField("Duration", (float)data.Duration);
			if (tempFloat != data.Duration) 
			{
				data.Duration = tempFloat;
				mSaveNeeded = true;
			}			
			
			tempInt = (int)EditorGUILayout.IntField("Sort Priority", (int)data.SortPriority);
			if (tempInt != data.SortPriority) 
			{
				data.SortPriority = tempInt;
				mSaveNeeded = true;
			}						
			
			EditorGUILayout.Space();
		}
		
		EditorGUILayout.EndScrollView();
	}
	
	private void LoadFile()
	{
		bool success = ConsumableStore.LoadFile();
		if (success == false)
		{
			GUILayout.Label("Failed to load Consumables.txt");
			notify.Debug("Failed to load Consumables.txt");
			return;
		}
		else
		{
			ConsumableStore.consumablesList.Sort((a1, a2) => a1.PID.CompareTo(a2.PID));
		}	
	}	
}




//	
//    static void Init() 
//	{
//		//bool success = ConsumableStore.LoadFile();	
//		//if (success == false) { TR.LOG("Failed to load Consumables.txt"); }
//    }	
//	


		//if (ConsumableStore.consumablesList.Count == 0) 
		//{
		//	bool success = ConsumableStore.LoadFile();		//-- Load the data file
		//	if (success == false) 
		//	{
		//		TR.LOG("Failed to load Consumables.txt");
		//		return;
		//	}
		//}		

		//string saveString = mSaveNeeded ? "Save*" : "Save";
		
		
//		string saveString = "Save";
//		if (mSaveNeeded == true) 
//		{
//			saveString = "Save*";
//			GUI.contentColor = Color.yellow;
//		}

	//UIGrid gridRoot = null;
	//GameObject managerCallbackObject = null;
	//string managerCallbackName = "";
	//GameObject mProtoCell = null;


//	public static void PopulateGridInEditor(UIGrid grid, GameObject protoCell, GameObject callbackObject, string callbackName)
//	{
//		//-- The selected Object must have a grid script.
//		if(grid == null) {
//			grid = Selection.activeTransform.GetComponent<UIGrid>() as UIGrid;
//			if(grid == null) {
//				TR.LOG ("Selected Object ({0}) must have a UIGrid Component", Selection.activeTransform.gameObject);
//				return;
//			}
//		}
//		
//		if(grid == null) {
//			TR.LOG ("Must have a UIGrid Component");
//			return;
//		}
//		
//		GameObject cell = protoCell;
//		if(cell == null) {
//			cell = (GameObject)Resources.Load ("interface/PowerStoreCell", typeof(GameObject));	
//			if(cell == null) {
//				TR.LOG ("Can't find interface/PowerStoreCell in the Resources folder");
//				return;
//			}
//		}
//		
//		if(cell == null) {
//			TR.LOG ("Must have a ProtoCell Component");
//			return;
//		}
//		
//		while(grid.transform.GetChildCount() > 0)
//		{
//			UnityEngine.GameObject.DestroyImmediate(grid.transform.GetChild(0).gameObject);
//		}
//		
//		//-- Add cells for each power type.
//		int totalCellCount = PowerStore.Powers.Count;
//		for(int t=0; t < totalCellCount; t++) {
//			
//			GameObject newCell = (GameObject)UnityEngine.Object.Instantiate(cell);
//			DestroyImmediate(newCell.GetComponent<UIPanel>());
//			
//			newCell.transform.parent = grid.transform;
//			newCell.transform.localScale = Vector3.one;
//			newCell.transform.rotation = grid.transform.rotation;
//			newCell.transform.localPosition = Vector3.zero;
//			
//			//-- Populate the cell with data.
//			BasePower power = PowerStore.Powers[t];
//			GameObject go = HierarchyUtils.GetChildByName("Title", newCell);
//			if(go != null) {
//				UILabel titleLabel = go.GetComponent<UILabel>() as UILabel;
//				if(titleLabel != null) {
//					titleLabel.text = power.Title;	
//					//titleLabel.color = Color.white;
//				}
//			}
//			
//			go = HierarchyUtils.GetChildByName("Icon", newCell);
//			if(go != null) {
//				UISprite iconSprite = go.GetComponent<UISprite>() as UISprite;
//				if(iconSprite != null) {
//					iconSprite.spriteName = power.IconName;
//				}
//			}
//			
//			go = HierarchyUtils.GetChildByName("Description", newCell);
//			if(go != null) {
//				UILabel desc = go.GetComponent<UILabel>() as UILabel;
//				if(desc != null) {
//					desc.text = power.Description;
//				}
//			}
//			
//			go = HierarchyUtils.GetChildByName("Cost", newCell);
//			if(go != null) {
//				UILabel cost = go.GetComponent<UILabel>() as UILabel;
//				if(cost != null) {
//					cost.text = power.Cost.ToString();
//				}
//			}
//			
//			go = HierarchyUtils.GetChildByName("CellContents", newCell);
//			if(go != null) {
//				UIButtonMessage message = go.GetComponent<UIButtonMessage>() as UIButtonMessage;
//				if(message != null) {
//					message.target = callbackObject;
//					message.functionName = callbackName;
//					//message.messageVal = t;
//				}
//				
//				//CellData cellData = go.GetComponent<CellData>() as CellData;
//				//if(cellData == null) {
//				//	//-- Add the script.
//				//	cellData = go.AddComponent<CellData>() as CellData;
//				//}
//				//cellData.Data = power.PowerID;
//				//cellData.cellParent = newCell.transform;
//				
//				go = HierarchyUtils.GetChildByName("BuyButton", go);
//				if(go != null) {
//					message = go.GetComponent<UIButtonMessage>() as UIButtonMessage;
//					if(message != null) {
//						message.target = callbackObject;
//						message.functionName = callbackName;
//						//message.messageVal = t;
//					}
//					
//					//cellData = go.GetComponent<CellData>() as CellData;
//					//if(cellData == null) {
//					//	//-- Add the script.
//					//	cellData = go.AddComponent<CellData>() as CellData;
//					//}
//					//cellData.Data = power.PowerID;
//					//cellData.cellParent = newCell.transform;
//				}
//			}
//		}
//		
//		
//		grid.Reposition();
//	}

//		gridRoot = EditorGUILayout.ObjectField("Grid Root", gridRoot, typeof(UIGrid), true) as UIGrid;
//		managerCallbackObject = EditorGUILayout.ObjectField("Cell Callback", managerCallbackObject, typeof(GameObject), true) as GameObject;
//		managerCallbackName = EditorGUILayout.TextField("Callback Name", managerCallbackName);
//		mProtoCell = EditorGUILayout.ObjectField("ProtoCell", mProtoCell, typeof(GameObject), false) as GameObject;
//		if(GUILayout.Button("Populate Grid") == true)
//		{
//			if(gridRoot != null && managerCallbackObject != null && mProtoCell != null && managerCallbackName != null) {
//				//-- Save before we recreate all of the cells.
//				if(mSaveNeeded == true) {
//					mSaveNeeded = false;
//					ArtifactStore.SaveFile();
//				}
//				PowerStoreEditor.PopulateGridInEditor(gridRoot, mProtoCell, managerCallbackObject, managerCallbackName);
//				
//				PowerStoreEditor.SaveString("powerGridRoot", gridRoot.name);
//				PowerStoreEditor.SaveString("powerUIManager", managerCallbackObject.name);
//				PowerStoreEditor.SaveString("powerMgrCallbackName", managerCallbackName);
//				PowerStoreEditor.SaveString("powerProtoCell", AssetDatabase.GetAssetPath(mProtoCell));
//				AssetDatabase.Refresh();
//			}
//			else {
//				if(gridRoot == null) {
//					EditorUtility.DisplayDialog("OOPS!", "You need to set the Grid Root before populating the grid.", "Cool");	
//				}
//				else if(managerCallbackObject == null) {
//					EditorUtility.DisplayDialog("DOOH!", "You need to set the callback object before populating the grid.", "Cool");	
//				}
//				else if(managerCallbackName == null || managerCallbackName.Length == 0) {
//					EditorUtility.DisplayDialog("DOOH!", "You need to set the callback method name before populating the grid.", "Cool");	
//				}
//				else if(mProtoCell == null) {
//					EditorUtility.DisplayDialog("BOOM HEADSHOT!", "You need to set the ProtoCell from Resources before populating the grid.", "Cool");	
//				}
//			}
//		}

	
//		//-- Load from Prefs
//		if(gridRoot == null) {
//			if(EditorPrefs.HasKey("powerGridRoot") == true) {
//				GameObject go = GameObject.Find (EditorPrefs.GetString("powerGridRoot"));
//				if(go) {
//					gridRoot = go.GetComponent<UIGrid>();
//				}
//			}
//		}
//		
//		if(managerCallbackObject == null) {
//			if(EditorPrefs.HasKey("powerUIManager") == true) {
//				GameObject go = GameObject.Find (EditorPrefs.GetString("powerUIManager"));
//				if(go) {
//					managerCallbackObject = go;
//				}
//			}
//		}
//		
//		if(managerCallbackName == null || managerCallbackName.Length == 0) {
//			if(EditorPrefs.HasKey("powerMgrCallbackName") == true) {
//				managerCallbackName = EditorPrefs.GetString("powerMgrCallbackName");
//			}
//		}
//		
//		if(mProtoCell == null) {
//			if(EditorPrefs.HasKey("powerProtoCell") == true) {
//				GameObject go = AssetDatabase.LoadAssetAtPath(EditorPrefs.GetString("powerProtoCell"), typeof(GameObject)) as GameObject;//GameObject.Find (EditorPrefs.GetString("artifactProtoCell"));
//				if(go) {
//					mProtoCell = go;
//				}
//			}
//		}