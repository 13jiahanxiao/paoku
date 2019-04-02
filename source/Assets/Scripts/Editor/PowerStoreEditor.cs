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

public class PowerStoreEditor : EditorWindow
{
	protected static Notify notify;
	Vector2 mScroll = Vector2.zero;
	bool mSaveNeeded = false;
	
	[MenuItem("TR2/Editors/Power Editor")]
	static public void OpenPowerStoreEditor()
	{
		EditorWindow.GetWindow<PowerStoreEditor>(false, "Power Editor", true);
	}
	
	void Awake()
	{
		notify = new Notify(this.GetType().Name);
	}

	void OnGUI()
	{
		//-- Load the file if its not yet loaded.
		if (PowerStore.Powers == null || PowerStore.Powers.Count == 0) 
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
			BasePower p = new BasePower();
			p.PowerID = PowerStore.GetNextPowerID();
			p._showFoldOut = true;
			PowerStore.Powers.Add(p);
		}
		
		string saveString = "Save";
		if (mSaveNeeded == true) 
		{
			saveString = "Save*";
			GUI.contentColor = Color.yellow;
		}
		
		if (GUILayout.Button(saveString) == true)
		{
			PowerStore.SaveFile();
			mSaveNeeded = false;
			AssetDatabase.Refresh();
		}
		
		GUI.contentColor = Color.white;
		if (GUILayout.Button("Reload") == true)
		{
			AssetDatabase.Refresh();
			LoadFile();	//PowerStore.LoadFile();
			mSaveNeeded = false;
		}
		
		GUILayout.EndHorizontal();
		
		NGUIEditorTools.DrawSeparator();
	
		DisplayPowerups();
	}
		
	private void DisplayPowerups() 		
	{
		//-- List Panel
		mScroll = EditorGUILayout.BeginScrollView(mScroll);
		
		foreach (BasePower data in PowerStore.Powers) 
		{
			EditorGUI.indentLevel = 0;
			GUILayout.BeginHorizontal();
			data._showFoldOut = EditorGUILayout.Foldout(data._showFoldOut, data.Title + " (ID=" + data.PowerID + ")");
			GUILayout.FlexibleSpace();
			Color preBackColor = GUI.backgroundColor;
						
			if (data._showFoldOut == true)
			{			
				GUI.backgroundColor = Color.red;
				if (GUILayout.Button("x")) 
				{
					bool delete = EditorUtility.DisplayDialog("Deleting Powerup", "Are you sure you want to delete the "
						+ data.Title + " powerup?", "Delete", "Do Not Delete");
					if (delete) 
					{
						PowerStore.Powers.Remove(data);
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
			
			string tempString = (string)EditorGUILayout.TextField("Title Key", (data.Title != null) ? data.Title : "");
			if (tempString.CompareTo(data.Title) != 0) 
			{
				data.Title = tempString;
				mSaveNeeded = true;
			}
			
			tempString = (string)EditorGUILayout.TextField("Desc Key", (data.Description != null) ? data.Description : "");
			if (tempString.CompareTo(data.Description) != 0)
			{
				data.Description = tempString;
				mSaveNeeded = true;
			}			
			
			tempString = (string)EditorGUILayout.TextField("Gem Desc Key", (data.BuffDescription != null) ? data.BuffDescription : "");
			if (tempString.CompareTo(data.BuffDescription) != 0)
			{
				data.BuffDescription = tempString;
				mSaveNeeded = true;
			}			
			
			tempString = (string)EditorGUILayout.TextField("Icon", (data.IconName != null) ? data.IconName : "");
			if (tempString.CompareTo(data.IconName) != 0) 
			{
				data.IconName = tempString;
				mSaveNeeded = true;
			}
		
			PowerType tempPowerType = (PowerType)EditorGUILayout.EnumPopup("Power Type", (PowerType)Enum.Parse(typeof(PowerType), data.Type));
			if (tempPowerType.ToString() != data.Type) 
			{
				data.Type = tempPowerType.ToString();
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
			
			double tempDouble = (double)EditorGUILayout.FloatField("Value", (float)data.Duration);
			if (tempDouble != data.Duration) 
			{
				data.Duration = tempDouble;
				mSaveNeeded = true;
			}
			
			float tempFloat = EditorGUILayout.FloatField("Coin Fill Rate Muliplier", (float)data.fillRateMultiplier);
			if (tempFloat != data.fillRateMultiplier) 
			{
				data.fillRateMultiplier = tempFloat;
				mSaveNeeded = true;
			}
			
			tempInt = (int)EditorGUILayout.IntField("Gem Cost", (int)data.ProtoBuff.cost);
			if (tempInt != data.ProtoBuff.cost) 
			{
				data.ProtoBuff.cost = tempInt;
				mSaveNeeded = true;
			}
			
			tempInt = (int)EditorGUILayout.IntField("Gem Uses", (int)data.ProtoBuff.usesLeft);
			if (tempInt != data.ProtoBuff.usesLeft)
			{
				data.ProtoBuff.usesLeft = tempInt;
				mSaveNeeded = true;
			}
			
			StatValueType tempSVType = (StatValueType)EditorGUILayout.EnumPopup("Gem Stat Type", data.ProtoBuff.statValueType);
			if (tempSVType != data.ProtoBuff.statValueType) 
			{
				data.ProtoBuff.statValueType = tempSVType;
				mSaveNeeded = true;
			}
			
			tempDouble = (double)EditorGUILayout.FloatField("Gem Value", (float)data.ProtoBuff.statValue);
			if (tempDouble != data.ProtoBuff.statValue) 
			{
				data.ProtoBuff.statValue = tempDouble;
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
		bool success = PowerStore.LoadFile();
		if (success == false) 
		{
			GUILayout.Label("Failed to load powers.txt");
			notify.Warning("Failed to load powers.txt");
			return;
		}
		else
		{
			PowerStore.Powers.Sort((a1, a2) => a1.PowerID.CompareTo(a2.PowerID));
		}			
	}
}




//			EditorGUI.indentLevel = 1;

			
			//EditorGUILayout.PrefixLabel("Description");
			//tempString = EditorGUILayout.TextArea(data.Description);
			

			//EditorGUILayout.PrefixLabel("Buff Desc.");
			//tempString = EditorGUILayout.TextArea(data.BuffDescription);



					//PowerStore.CreateFromCode();	
			//ArtifactStore.Artifacts.Add (new ArtifactProtoData(null, "Empty", "none", "Add Gems to increase effect", 0, 0, 0, 0, 0.0f, CostType.Coin));


//	
//	static void SaveString(string field, string val)
//	{
//		if (string.IsNullOrEmpty(val))
//		{
//			EditorPrefs.DeleteKey(field);
//		}
//		else
//		{
//			EditorPrefs.SetString(field, val);
//		}
//	}
//	


	//UIGrid gridRoot = null;
	//GameObject managerCallbackObject = null;
	//string managerCallbackName = "";
	//GameObject mProtoCell = null;
	


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
//	


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