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

public class ArtifactStoreEditor : EditorWindow
{
	protected static Notify notify;
	Vector2 mScroll = Vector2.zero;
	bool mSaveNeeded = false;

	[MenuItem("TR2/Editors/Artifact Editor")]
	static public void OpenArtifactStoreEditor()
	{
		EditorWindow.GetWindow<ArtifactStoreEditor>(false, "Artifact Editor", true);
	}
	
	void Awake()
	{
		notify = new Notify(this.GetType().Name);
	}
	
	void OnGUI()
	{
		//-- Load the file if its not yet loaded.
		if (ArtifactStore.Artifacts == null || ArtifactStore.Artifacts.Count == 0) 	
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
			ArtifactProtoData apData = new ArtifactProtoData(null, "Empty", "none", "Add Gems to increase effect",0,0,0,0, 0, 0, 0,0,0,0,0, 0.0f, 0, 0, 0.0f, CostType.Coin, "", 0);
			apData._showFoldOut = true;
			ArtifactStore.Artifacts.Add(apData);
		}
		
		string saveString = "Save";
		if (mSaveNeeded == true) 
		{
			saveString = "Save*";
			GUI.contentColor = Color.yellow;
		}
		
		if (GUILayout.Button(saveString) == true)
		{
			ArtifactStore.SaveFile();
			mSaveNeeded = false;
			AssetDatabase.Refresh();
		}
		
		GUI.contentColor = Color.white;
		
		if (GUILayout.Button("Reload") == true)
		{
			AssetDatabase.Refresh();
			LoadFile();	//ArtifactStore.LoadFile();
			mSaveNeeded = false;
		}
		GUILayout.EndHorizontal();

		NGUIEditorTools.DrawSeparator();
		
		DisplayArtifacts();
	}

	private void DisplayArtifacts() 
	{
		//-- List Panel
		mScroll = EditorGUILayout.BeginScrollView(mScroll);
	
		foreach (ArtifactProtoData data in ArtifactStore.Artifacts) 
		{
			EditorGUI.indentLevel = 0;
			GUILayout.BeginHorizontal();
			data._showFoldOut = EditorGUILayout.Foldout(data._showFoldOut, data._title + " (ID=" + data._id + ")");
			GUILayout.FlexibleSpace();
			Color preBackColor = GUI.backgroundColor;
			
			if (data._showFoldOut == true)
			{
				GUI.backgroundColor = Color.red;
				
				if (GUILayout.Button("x")) 
				{
					bool delete = EditorUtility.DisplayDialog("Deleting Artifact", "Are you sure you want to delete the "
						+ data._title + " artifact?", "Delete", "Do Not Delete");
					if (delete)
					{
						ArtifactStore.Artifacts.Remove(data);
						GUILayout.EndHorizontal();
						return;	
					}
				}
			}
			
			GUI.backgroundColor = preBackColor;
			GUILayout.EndHorizontal();
			
			if (data._showFoldOut == false) { continue; }
			
			EditorGUI.indentLevel = 0;
			GUI.backgroundColor = preBackColor;
			GUI.contentColor = Color.white;
			
			
			// wxj
			String tempString = (string)EditorGUILayout.TextField("OneKey Id", data._onekey_iap_id);
			if (tempString.CompareTo(data._onekey_iap_id) != 0) 
			{
				data._onekey_iap_id = tempString;
				mSaveNeeded = true;
			}
			
			int tempInt = (int)EditorGUILayout.IntField("OneKey Coins", data._onekey_coins);
			if (tempInt != data._onekey_coins) 
			{
				data._onekey_coins = tempInt;
				mSaveNeeded = true;
			}
			
			tempString = (string)EditorGUILayout.TextField("OK Msg key", data._onekey_msg_key);
			if (tempString.CompareTo(data._onekey_msg_key) != 0) 
			{
				data._onekey_msg_key = tempString;
				mSaveNeeded = true;
			}
			
			
			
			
			tempString = (string)EditorGUILayout.TextField("Title Key", (data._title != null) ? data._title : "");
			if (tempString.CompareTo(data._title) != 0) 
			{
				data._title = tempString;
				mSaveNeeded = true;
			}
			
			tempString = (string)EditorGUILayout.TextField("Gemmed Title Key", (data._gemmedTitle != null) ? data._gemmedTitle : "");
			if (tempString.CompareTo(data._gemmedTitle) != 0) 
			{
				data._gemmedTitle = tempString;
				mSaveNeeded = true;
			}
			
			tempString = (string)EditorGUILayout.TextField("Desc Key", (data._description != null) ? data._description : "");
			if (tempString.CompareTo(data._description) != 0)
			{
				data._description = tempString;
				mSaveNeeded = true;
			}			

			tempString = (string)EditorGUILayout.TextField("Gem Desc Key", (data._buffDescription != null) ? data._buffDescription : "");
			if (tempString.CompareTo(data._buffDescription) != 0) 
			{
				data._buffDescription = tempString;
				mSaveNeeded = true;
			}			
			
			tempString = (string)EditorGUILayout.TextField("Icon", (data._iconName != null) ? data._iconName : "");
			if (tempString.CompareTo(data._iconName) != 0) 
			{
				data._iconName = tempString;
				mSaveNeeded = true;
			}
			
			tempInt = (int)EditorGUILayout.IntField("Min Rank", (int)data._requiredRank);
			if (tempInt != data._requiredRank)
			{
				data._requiredRank = tempInt;
				mSaveNeeded = true;
			}
			
		/*	tempInt = (int)EditorGUILayout.IntField("Cost", (int)data._cost);
			if (tempInt != data._cost) 
			{
				data._cost = tempInt;
				mSaveNeeded = true;
			}*/
			
			CostType tempCostType = (CostType)EditorGUILayout.EnumPopup("Cost Type", data._costType);
			if (tempCostType != data._costType && tempCostType != CostType.Total) 
			{
				data._costType = tempCostType;
				mSaveNeeded = true;
			}
			
			StatType tempStatType = (StatType)EditorGUILayout.EnumPopup("Stat", data._statType);
			if (tempStatType != data._statType && tempStatType != StatType.Total)
			{
				data._statType = tempStatType;
				mSaveNeeded = true;
			}
			
			StatValueType tempStatValueType = (StatValueType)EditorGUILayout.EnumPopup("Stat Type", data._statValueType);
			if (tempStatValueType != data._statValueType) 
			{
				data._statValueType = tempStatValueType;
				mSaveNeeded = true;
			}
			
			/*double tempDouble = (double)EditorGUILayout.FloatField("Value", (float)data._statValue);
			if (tempDouble != data._statValue)
			{
				data._statValue = tempDouble;
				mSaveNeeded = true;
			}*/
			
			
			EditorGUILayout.BeginHorizontal();
			
			EditorGUILayout.BeginVertical();
			
			GUILayout.Label("Lv.1");
			data._statValue = (double)EditorGUILayout.FloatField("Value", (float)data._statValue);
			data._cost = EditorGUILayout.IntField("Cost", data._cost);
			
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical();
			
			GUILayout.Label("Lv.2");
			data._statValue_lv2 = (double)EditorGUILayout.FloatField("Value", (float)data._statValue_lv2);
			data._cost_lv2 = EditorGUILayout.IntField("Cost", data._cost_lv2);
			
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical();
			
			GUILayout.Label("Lv.3");
			data._statValue_lv3 = (double)EditorGUILayout.FloatField("Value", (float)data._statValue_lv3);
			data._cost_lv3 = EditorGUILayout.IntField("Cost", data._cost_lv3);
			
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical();
			
			GUILayout.Label("Lv.4");
			data._statValue_lv4 = (double)EditorGUILayout.FloatField("Value", (float)data._statValue_lv4);
			data._cost_lv4 = EditorGUILayout.IntField("Cost", data._cost_lv4);
			
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical();
			
			GUILayout.Label("Lv.5");
			data._statValue_lv5 = (double)EditorGUILayout.FloatField("Value", (float)data._statValue_lv5);
			data._cost_lv5 = EditorGUILayout.IntField("Cost", data._cost_lv5);
			
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.EndHorizontal();
			
			
			
			tempStatType = (StatType)EditorGUILayout.EnumPopup("Gem Stat", data._gemmedStatType);
			if (tempStatType != data._gemmedStatType && tempStatType != StatType.Total) 
			{
				data._gemmedStatType = tempStatType;
				mSaveNeeded = true;
			}
			
			tempStatValueType = (StatValueType)EditorGUILayout.EnumPopup("Gem St Type", data._gemmedValueType);
			if (tempStatValueType != data._gemmedValueType) 
			{
				data._gemmedValueType = tempStatValueType;
				mSaveNeeded = true; 
			}
			
			double tempDouble = (double)EditorGUILayout.FloatField("Gem Value", (float)data._gemmedValue);
			if (tempDouble != data._gemmedValue) 
			{
				data._gemmedValue = tempDouble;
				mSaveNeeded = true;
			}
		
			tempInt = (int)EditorGUILayout.IntField("Sort Priority", (int)data._sortPriority);
			if (tempInt != data._sortPriority) 
			{
				data._sortPriority = tempInt;
				mSaveNeeded = true;
			}	
			
			if(GUI.changed)	mSaveNeeded = true;
			
			
			EditorGUILayout.Space();
		}
		
		EditorGUILayout.EndScrollView();
	}
		
	private void LoadFile()
	{
		bool success = ArtifactStore.LoadFile();
		if (success == false)
		{
			GUILayout.Label("Failed to load artifacts.txt");
			notify.Warning("Failed to load artifacts.txt");
			return;
		}
		else
		{
			ArtifactStore.Artifacts.Sort((a1, a2) => a1._id.CompareTo(a2._id));
		}		
	}
}



	//ArtifactStore.RemoveArtifact(data);


	// ArtifactStore.AddArtifact(apData);


//			EditorGUI.indentLevel = 1;


		//if (ArtifactStore.GetNumberOfArtifacts() == 0) 
			
//			int tempProgression = (int)EditorGUILayout.IntField("Progression", (int)data._progressionID);
//			if (tempProgression != data._progressionID) 
//			{
//				data._progressionID = tempProgression;
//				mSaveNeeded = true;
//			}

			
			//EditorGUILayout.PrefixLabel("Gem Desc");
			//string tempString2 = EditorGUILayout.TextArea(data._buffDescription);

			//EditorGUILayout.PrefixLabel("Desc");
			//tempString = EditorGUILayout.TextArea(data._description);

	//int showArtifacts = 1;
	//Dictionary<int, bool> showProgressionFoldOut = new Dictionary<int, bool>();
	


//
//	Vector2 progressionScrollVector;
//	
//	private void DisplayProgressions()
//	{
//		ArtifactStore.ComputeProgressions();
//		GUILayout.BeginHorizontal(GUI.skin.box);
//    	progressionScrollVector = GUILayout.BeginScrollView(progressionScrollVector);
//		
//		Dictionary<int, List<int>> progressions = ArtifactStore.getProgression();
//		foreach (KeyValuePair<int, List<int>> progressionPair in progressions) 
//		{
//			EditorGUI.indentLevel = 0;
//			bool showFoldOut = true;
//			
//			if (showProgressionFoldOut.TryGetValue(progressionPair.Key, out showFoldOut) == false) 
//			{
//				showProgressionFoldOut.Add (progressionPair.Key, true);
//			}
//			
//			showProgressionFoldOut[progressionPair.Key] = EditorGUILayout.Foldout(showFoldOut, "Progression ID = "+progressionPair.Key);
//			if (showProgressionFoldOut[progressionPair.Key] == false) { continue; }
//			 
//			EditorGUI.indentLevel = 1;
//			
//			foreach (int artifactID in progressionPair.Value) 
//			{
//				ArtifactProtoData data = ArtifactStore.GetArtifactProtoData(artifactID);
//				if (data == null) { continue; }
//				
//				GUILayout.BeginHorizontal(GUI.skin.textField);
//				EditorGUILayout.LabelField(data._title);
//				EditorGUILayout.LabelField("ID: "+data._id.ToString());
//				EditorGUILayout.LabelField("Cost: "+data._cost+" "+data._costType.ToString()+"s");
//				
//				if (data._statValueType == StatValueType.Percent) 
//				{
//					EditorGUILayout.LabelField(data._statType+" *= "+data._statValue);	
//				}
//				else if (data._statValueType == StatValueType.Absolute) 
//				{
//					EditorGUILayout.LabelField(data._statType+" = "+data._statValue);	
//				}
//				else if (data._statValueType == StatValueType.Relative) 
//				{
//					EditorGUILayout.LabelField(data._statType+" += "+data._statValue);	
//				}
//				
//				GUILayout.EndHorizontal();
//			}
//		}
//
//	    GUILayout.EndScrollView();
//	    GUILayout.EndHorizontal();
//	}	


		
		//int max = ArtifactStore.GetNumberOfArtifacts();
		//for (int i = 0; i < max; i++) 
		//{
		//	ArtifactProtoData data = ArtifactStore.GetArtifactProtoData(i);
		//	if (data == null) { continue; }
		
		
		//NGUIEditorTools.DrawSeparator();
		//GUILayout.BeginHorizontal();
		//bool showAritacts = GUILayout.Button("Show Artifacts");
		//string[] texts = new string[2];
		//texts[0]= "Show Artifacts";
		//texts[1]= "Show Progressions";
		//showArtifacts = GUILayout.SelectionGrid(showArtifacts,texts,2,"button");
		//showArtifacts = 0;
		
		//if (showArtifacts == 0) { DisplayArtifacts(); }
		//else { DisplayProgressions(); }
				
		
		//GUILayout.EndHorizontal();

		
//		int max = ArtifactStore.GetNumberOfArtifacts();
//		for (int i = 0; i < max; i++)
//	    {
////	        GUILayout.Label(ArtifactStore.GetArtifactProtoData(i)._title);
////	        if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
////	        {
////            // Handle events here
////				TR.LOG ("mouse over "+ArtifactStore.GetArtifactProtoData(i)._title);
////	        }
//	    }



			//ArtifactStoreEditor.SaveString("theArtifactGridRoot", gridRoot.name);
			//ArtifactStoreEditor.SaveString("artifactUIManager", managerCallbackObject.name);
			//ArtifactStoreEditor.SaveString("artifactMgrCallbackName", managerCallbackName);
			//ArtifactStoreEditor.SaveString("artifactProtoCell", AssetDatabase.GetAssetPath(mProtoCell));

	//UIGrid gridRoot = null;
	//GameObject managerCallbackObject = null;
	//string managerCallbackName = "";
	//GameObject mProtoCell = null;


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
//				ArtifactStoreEditor.PopulateGridInEditor(gridRoot, mProtoCell, managerCallbackObject, managerCallbackName);
//				
//				ArtifactStoreEditor.SaveString("theArtifactGridRoot", gridRoot.name);
//				ArtifactStoreEditor.SaveString("artifactUIManager", managerCallbackObject.name);
//				ArtifactStoreEditor.SaveString("artifactMgrCallbackName", managerCallbackName);
//				ArtifactStoreEditor.SaveString("artifactProtoCell", AssetDatabase.GetAssetPath(mProtoCell));
//				AssetDatabase.Refresh();
//				return;	
//			}
//			else {
//				if(gridRoot == null) {
//					EditorUtility.DisplayDialog("OOPS!", "You need to set the Grid Root before populating the grid.", "Cool");	
//				}
//				else if(managerCallbackObject == null) {
//					EditorUtility.DisplayDialog("DOOH!", "You need to set the callback Object before populating the grid.", "Cool");	
//				}
//				else if(managerCallbackName == null || managerCallbackName.Length == 0) {
//					EditorUtility.DisplayDialog("DOOH!", "You need to set the callback method name before populating the grid.", "Cool");	
//				}
//				else if(mProtoCell == null) {
//					EditorUtility.DisplayDialog("BOOM HEADSHOT!", "You need to set the ProtoCell from Resources before populating the grid.", "Cool");	
//				}
//			}
//		}
		



//		
//		//-- Load from Prefs
//		if(gridRoot == null) {
//			if(EditorPrefs.HasKey("theArtifactGridRoot") == true) {
//				GameObject go = GameObject.Find (EditorPrefs.GetString("theArtifactGridRoot"));
//				if(go) {
//					gridRoot = go.GetComponent<UIGrid>();
//				}
//			}
//		}
//		
//		if(managerCallbackObject == null) {
//			if(EditorPrefs.HasKey("artifactUIManager") == true) {
//				GameObject go = GameObject.Find (EditorPrefs.GetString("artifactUIManager"));
//				if(go) {
//					managerCallbackObject = go;
//				}
//			}
//		}
//		
//		if(managerCallbackName == null || managerCallbackName.Length == 0) {
//			if(EditorPrefs.HasKey("artifactMgrCallbackName") == true) {
//				managerCallbackName = EditorPrefs.GetString("artifactMgrCallbackName");
//			}
//		}
//		
//		if(mProtoCell == null) {
//			if(EditorPrefs.HasKey("artifactProtoCell") == true) {
//				GameObject go = AssetDatabase.LoadAssetAtPath(EditorPrefs.GetString("artifactProtoCell"), typeof(GameObject)) as GameObject;//GameObject.Find (EditorPrefs.GetString("artifactProtoCell"));
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
//			cell = (GameObject)Resources.Load ("interface/ArtifactStoreCell", typeof(GameObject));	
//			if(cell == null) {
//				TR.LOG ("Can't find interface/ArtifactStoreCell in the Resources folder");
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
//		//-- Add cells for each Artifact type.
//		int totalCellCount = ArtifactStore.GetNumberOfArtifacts();
//		for(int t=0; t < totalCellCount; t++) {
//			
//			ArtifactProtoData protoData = ArtifactStore.GetArtifactProtoData(t);
//			if(protoData == null)
//				continue;
//			if(protoData._costType == CostType.RealMoney)
//				continue;
//			//-- SKip if we are in a progression and ARE NOT the first one in the list.
//			if(protoData._progressionID != -1) {
//				List<int> progression = ArtifactStore.getProgression()[protoData._progressionID];
//				if(protoData._id != progression[0])
//					continue;
//			}
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
//			//ArtifactProtoData protoData = ArtifactStore.Artifacts[t];
//			GameObject go = HierarchyUtils.GetChildByName("Title", newCell);
//
//			if(go != null) {
//				UILabel titleLabel = go.GetComponent<UILabel>() as UILabel;
//				if(titleLabel != null) {
//					titleLabel.text = protoData._title;	
//					//titleLabel.color = ArtifactStore.colorForRarity(protoData._rarity);
//				}
//			}
//			
//			go = HierarchyUtils.GetChildByName("Icon", newCell);
//			if(go != null) {
//				UISprite iconSprite = go.GetComponent<UISprite>() as UISprite;
//				if(iconSprite != null) {
//					iconSprite.spriteName = protoData._iconName;
//				}
//			}
//			
//			go = HierarchyUtils.GetChildByName("Description", newCell);
//			if(go != null) {
//				UILabel desc = go.GetComponent<UILabel>() as UILabel;
//				if(desc != null) {
//					desc.text = protoData._description;
//				}
//			}
//			
//			go = HierarchyUtils.GetChildByName("Cost", newCell);
//			if(go != null) {
//				UILabel cost = go.GetComponent<UILabel>() as UILabel;
//				if(cost != null) {
//					cost.text = protoData._cost.ToString();
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
//				//cellData.Data = protoData._id;
//				//cellData.cellParent = newCell.transform;
//				
////				go = HierarchyUtils.GetChildByName("BuyButton", go);
////				if(go != null) {
////					message = go.GetComponent<UIButtonMessage>() as UIButtonMessage;
////					if(message != null) {
////						message.target = callbackObject;
////						message.functionName = callbackName;
////						//message.messageVal = t;
////					}
////					
////					cellData = go.GetComponent<CellData>() as CellData;
////					if(cellData == null) {
////						//-- Add the script.
////						cellData = go.AddComponent<CellData>() as CellData;
////					}
////					cellData.Data = t;
////					cellData.cellParent = newCell.transform;
////				}
//			}
//		}
//		
//		
//		grid.Reposition();
//	}
//	
//	static void SaveString (string field, string val)
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
	
	
//	[MenuItem("TR2/Aritfact Store/Save Store")]
//	public static void SaveStore()
//	{
//		ArtifactStore.SaveFile();
//	}
	