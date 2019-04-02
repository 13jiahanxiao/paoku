using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObjectivesManagerEditor : EditorWindow 
{
	protected static Notify notify;
	Vector2 mScroll = Vector2.zero;
	bool mSaveNeeded = false;
	
	string search = "";

	[MenuItem("TR2/Editors/Objectives Editor")]
	static public void OpenEditor()
	{
		EditorWindow.GetWindow<ObjectivesManagerEditor>(false, "Objective Editor", true);
	}
	
	void Awake()
	{
		notify = new Notify(this.GetType().Name);	
	}
		
	void OnGUI()
	{
		//-- Load the file if its not yet loaded.
		if (ObjectivesManager.Objectives == null || ObjectivesManager.Objectives.Count == 0) 
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
			ObjectiveProtoData ob = new ObjectiveProtoData(new Dictionary<string, object>());	//"empty");
			ob._id = ObjectivesManager.GetNextID(ObjectivesManager.Objectives);
			ob._showFoldOut = true;
			ObjectivesManager.Objectives.Add(ob);
		}
		string saveString = "Save";
		if (mSaveNeeded == true)
		{
			saveString = "Save*";
			GUI.contentColor = Color.yellow;
		}
		
		if (GUILayout.Button(saveString) == true)
		{
			ObjectivesManager.SaveFile(ObjectivesManager.Objectives, "OZGameData/Objectives");
			mSaveNeeded = false;
			AssetDatabase.Refresh();
		}
		
		GUI.contentColor = Color.white;
		if (GUILayout.Button("Reload") == true)
		{
			AssetDatabase.Refresh();
			LoadFile();	//ObjectivesManager.LoadFile(ObjectivesManager.Objectives, "OZGameData/Objectives");
			mSaveNeeded = false;
		}
		
		GUILayout.EndHorizontal();
		
		
		NGUIEditorTools.DrawSeparator();
		
		GUILayout.BeginHorizontal();
		
		search = EditorGUILayout.TextField("Search",search);
		if(GUILayout.Button("X",GUILayout.Width(30)))
			search = "";
		
		GUILayout.EndHorizontal();
		
		NGUIEditorTools.DrawSeparator();
		

		//-- List Panel
		mScroll = EditorGUILayout.BeginScrollView(mScroll);
		
		int objCount = 0;
		
		foreach (ObjectiveProtoData data in ObjectivesManager.Objectives) 
		{
			string engTitle = "";
			string engDesc = "";
			if(Localization.SharedInstance!=null)
			{
				Localization.SharedInstance.SetLanguage("English");
				if(data._title!=null)
					engTitle = Localization.SharedInstance.Get(data._title);
				if(data._descriptionPreEarned!=null)
					engDesc = Localization.SharedInstance.Get(data._descriptionPreEarned);
			//	Debug.Log("Success... "+engTitle);
			}
			else {
				engTitle = data._title;
			}
			
			if(search!="" && !engTitle.ToLower().Contains(search.ToLower()) && !data._title.ToLower().Contains(search.ToLower()))
				continue;
			
			objCount++;
			
			EditorGUI.indentLevel = 0;
			GUILayout.BeginHorizontal();
			data._showFoldOut = EditorGUILayout.Foldout(data._showFoldOut,objCount.ToString() + ". " + data._title + ": " + engTitle/*data._internalTitle*/ + " (ID=" + data._id +")");
			GUILayout.FlexibleSpace();
			Color preBackColor = GUI.backgroundColor;
			
			if (data._showFoldOut == true)
			{
				GUI.backgroundColor = Color.red;
				if (GUILayout.Button("x")) 
				{
					bool delete = EditorUtility.DisplayDialog("Deleting Objective", "Are you sure you want to delete the "
						+ data._internalTitle + " objective?", "Delete", "Do Not Delete");
					if (delete) 
					{
						ObjectivesManager.Objectives.Remove(data);
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
							
			string tempString = (string)EditorGUILayout.TextField("Title Key", (data._title != null) ? data._title : "");
			if (tempString.CompareTo(data._title) != 0) 
			{
				data._title = tempString;
				mSaveNeeded = true;
				data._title.Replace("\n","");
				data._title.Replace(" ","");
				data._title.Replace("\t","");
			}
			
			tempString = (string)EditorGUILayout.TextField("Desc Key", (data._descriptionPreEarned != null) ? data._descriptionPreEarned : "");
			if (tempString.CompareTo(data._descriptionPreEarned) != 0) 
			{
				data._descriptionPreEarned = tempString;
				mSaveNeeded = true;
				data._descriptionPreEarned.Replace("\n","");
				data._descriptionPreEarned.Replace(" ","");
				data._descriptionPreEarned.Replace("\t","");
			}
			
			GUILayout.Label("Localized Description: "+engDesc);
			
			tempString = (string)EditorGUILayout.TextField("Icon", (data._iconNameEarned != null) ? data._iconNameEarned : "");
			if (tempString.CompareTo(data._iconNameEarned) != 0) 
			{
				data._iconNameEarned = tempString;
				mSaveNeeded = true;
			}			
			
			ObjectiveDifficulty objDiff = (ObjectiveDifficulty)EditorGUILayout.EnumPopup("Difficulty", data._difficulty);
			if (objDiff != data._difficulty)
			{
				data._difficulty = objDiff;
				mSaveNeeded = true;
			}
			
			ObjectiveCategory objCat = (ObjectiveCategory)EditorGUILayout.EnumPopup("Category", data._category);
			if (objCat != data._category) 
			{
				data._category = objCat;
				mSaveNeeded = true;
			}
			
			ObjectiveType objType = (ObjectiveType)EditorGUILayout.EnumPopup("Type", data._conditionList[0]._type);
			if (objType != data._conditionList[0]._type)
			{
				data._conditionList[0]._type = objType;
				mSaveNeeded = true;
			}
			
			ObjectiveTimeType objTimeType = (ObjectiveTimeType)EditorGUILayout.EnumPopup("Time Type", data._conditionList[0]._timeType);
			if (objTimeType != data._conditionList[0]._timeType)
			{
				data._conditionList[0]._timeType = objTimeType;
				mSaveNeeded = true;
			}
			
			//Filter type discontinued
		//	ObjectiveFilterType objFilterType = (ObjectiveFilterType)EditorGUILayout.EnumPopup("Filter Type", data._conditionList[0]._filterType);
		//	if (objFilterType != data._conditionList[0]._filterType)
		//	{
		//		data._conditionList[0]._filterType = objFilterType;
		//		mSaveNeeded = true;
		//	}
			bool useEnv = EditorGUILayout.Toggle("Use Env?",((int)data._environmentID)>=0);
			if (useEnv)
			{
				if(data._environmentID<0)	data._environmentID = 0;
			}
			if(!useEnv)
			{
				data._environmentID = -1;
			}
			
			if(!useEnv)	GUI.enabled = false;
			int env = (int)EditorGUILayout.IntField("Environment", (int)data._environmentID);
			if (env != data._environmentID)
			{
				data._environmentID = env;
				mSaveNeeded = true;
			}
			GUI.enabled = true;
			
			int statValue = (int)EditorGUILayout.IntField("Value", (int)data._conditionList[0]._statValue);
			if (statValue != data._conditionList[0]._statValue)
			{
				data._conditionList[0]._statValue = statValue;
				mSaveNeeded = true;
			}

			int _pointValue = (int)EditorGUILayout.IntField("Points", (int)data._pointValue);
			if (_pointValue != data._pointValue) 
			{
				data._pointValue = _pointValue;
				mSaveNeeded = true;
			}
	
			EditorGUILayout.Space();
		}
		
		EditorGUILayout.EndScrollView();
	}
	
	private void LoadFile()
	{
		bool success = ObjectivesManager.LoadFile(ObjectivesManager.Objectives, "OZGameData/Objectives");
		if (success == false)
		{
			GUILayout.Label("Failed to load objectives.txt");
			notify.Debug("Failed to load objectives.txt");
			return;
		}
		else
		{
			ObjectivesManager.Objectives.Sort((a1, a2) => a1._id.CompareTo(a2._id));
		}		
	}
}



//			
//			tempString = (string)EditorGUILayout.TextField("Int. Desc.", (data._descriptionEarned != null) ? data._descriptionEarned : "");
//			if (tempString.CompareTo(data._descriptionEarned) != 0)
//			{
//				data._descriptionEarned = tempString;
//				mSaveNeeded = true;
//			}


				
//			string tempString = (string)EditorGUILayout.TextField("Int. Title", (data._internalTitle != null) ? data._internalTitle : "");
//			if (tempString.CompareTo(data._internalTitle) != 0) 
//			{
//				data._internalTitle = tempString;
//				mSaveNeeded = true;
//			}	


//	UIGrid gridRoot = null;
//	GameObject mProtoCell = null;


//			tempString = (string)EditorGUILayout.TextField("Icon Pre", (data._iconNamePreEarned != null) ? data._iconNamePreEarned : "");
//			if (tempString.CompareTo(data._iconNamePreEarned) != 0)
//			{
//				data._iconNamePreEarned = tempString;
//				mSaveNeeded = true;
//			}
			
//			bool _multiEarn = (bool)EditorGUILayout.Toggle("MutliEarn", data._canEarnMoreThanOnce);
//			if (_multiEarn != data._canEarnMoreThanOnce) 
//			{
//				data._canEarnMoreThanOnce = _multiEarn;
//				mSaveNeeded = true;
//			}


	
	
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
//		//-- Load from Prefs
//		if (gridRoot == null) 
//		{
//			if(EditorPrefs.HasKey("ObjectivesManagerGridRoot") == true) 
//			{
//				GameObject go = GameObject.Find (EditorPrefs.GetString("ObjectivesManagerGridRoot"));
//				if (go) 
//				{
//					gridRoot = go.GetComponent<UIGrid>();
//				}
//			}
//		}
//		
//		if (mProtoCell == null) 
//		{
//			if (EditorPrefs.HasKey("ObjectivesManagerProtoCell") == true)
//			{
//				GameObject go = AssetDatabase.LoadAssetAtPath(EditorPrefs.GetString("ObjectivesManagerProtoCell"), typeof(GameObject)) as GameObject;
//				if (go) { mProtoCell = go; }
//			}
//		}	
	
	


		//-- Test process stuff.
//		if(GUILayout.Button("ITMS") == true) {
//			
//			ObjectivesManagerEditor.itms();
//		}
//		NGUIEditorTools.DrawSeparator();
		


//	public static void PopulateGridInEditor(UIGrid grid, GameObject protoCell)
//	{
//		//-- The selected Object must have a grid script.
//		if (grid == null) { grid = Selection.activeTransform.GetComponent<UIGrid>() as UIGrid;
//		if (grid == null) 
//			{
//				TR.LOG ("Selected Object ({0}) must have a UIGrid Component", Selection.activeTransform.gameObject);
//				return;
//			}
//		}
//		
//		if (grid == null) 
//		{
//			TR.LOG ("Must have a UIGrid Component");
//			return;
//		}
//		
//		GameObject cell = protoCell;
//		if (cell == null) 
//		{
//			cell = (GameObject)Resources.Load ("interface/ObjectivesManagerCell", typeof(GameObject));	
//			if(cell == null) {
//				TR.LOG ("Can't find interface/ObjectivesManagerCell in the Resources folder");
//				return;
//			}
//		}
//		
//		if(cell == null) {
//			TR.LOG ("Must have a ProtoCell Component");
//			return;
//		}
//		
//		//-- Kill existing cells.
//		while(grid.transform.GetChildCount() > 0)
//		{
//			UnityEngine.GameObject.DestroyImmediate(grid.transform.GetChild(0).gameObject);
//		}
//		
//		//-- Add cells for each type.
//		int totalCellCount = ObjectivesManager.Objectives.Count;
//		for(int t=0; t < totalCellCount; t++) {
//			
//			GameObject newCell = (GameObject)UnityEngine.Object.Instantiate(cell);
//			newCell.transform.parent = grid.transform;
//			newCell.transform.localScale = Vector3.one;
//			newCell.transform.rotation = grid.transform.rotation;
//			newCell.transform.position = Vector3.zero;
//			
//			//-- Populate the cell with data.
//			ObjectiveProtoData protoData = ObjectivesManager.Objectives[t];
//			GameObject go = HierarchyUtils.GetChildByName("Title", newCell);
//			if(go != null) {
//				UILabel titleLabel = go.GetComponent<UILabel>() as UILabel;
//				if(titleLabel != null) {
//					titleLabel.text = protoData._title;	
//				}
//			}
//			
//			go = HierarchyUtils.GetChildByName("Icon", newCell);
//			if(go != null) {
//				UISprite iconSprite = go.GetComponent<UISprite>() as UISprite;
//				if(iconSprite != null) {
//					iconSprite.spriteName = protoData._iconNamePreEarned;
//				}
//			}
//			
//			go = HierarchyUtils.GetChildByName("Description", newCell);
//			if(go != null) {
//				UILabel desc = go.GetComponent<UILabel>() as UILabel;
//				if(desc != null) {
//					desc.text = protoData._descriptionPreEarned;
//				}
//			}
//			
//			go = HierarchyUtils.GetChildByName("PointValueLabel", newCell);
//			if(go != null) {
//				UILabel pointValueLabel = go.GetComponent<UILabel>() as UILabel;
//				if(pointValueLabel != null) {
//					pointValueLabel.text = protoData._pointValue.ToString();
//				}
//			}
//			
//			go = HierarchyUtils.GetChildByName("ObjectiveCellContents", newCell);
//			if(go != null) {
//				
//				CellData cellData = go.GetComponent<CellData>() as CellData;
//				if(cellData == null) {
//					//-- Add the script.
//					cellData = go.AddComponent<CellData>() as CellData;
//				}
//				cellData.Data = t;
//			}
//		}
//		grid.Reposition();
//	}
//	


//	static private void itms() {
//		TR.LOG("itms start");
//		notify.Debug("foo");
//	}

	//		gridRoot = EditorGUILayout.ObjectField("Grid Root", gridRoot, typeof(UIGrid), true) as UIGrid;
//		mProtoCell = EditorGUILayout.ObjectField("ProtoCell", mProtoCell, typeof(GameObject), false) as GameObject;
		
//		GUILayout.BeginHorizontal();
//			EditorGUILayout.PrefixLabel("Total Points");
//			EditorGUILayout.LabelField(ObjectivesManager.getTotalPoints().ToString() + "/1000");
//		GUILayout.EndHorizontal();
		
//		EditorGUILayout.Space();
//		if(GUILayout.Button("Populate Grid") == true)
//		{
//			if(gridRoot != null && mProtoCell != null) {
//				//-- Save before we recreate all of the cells.
//				if(mSaveNeeded == true) {
//					mSaveNeeded = false;
//					ObjectivesManager.SaveFile();
//				}
//				ObjectivesManagerEditor.PopulateGridInEditor(gridRoot, mProtoCell);
//				ObjectivesManagerEditor.SaveString("ObjectivesManagerGridRoot", gridRoot.name);
//				ObjectivesManagerEditor.SaveString("ObjectivesManagerProtoCell", AssetDatabase.GetAssetPath(mProtoCell));
//				AssetDatabase.Refresh();
//			}
//			else {
//				if(gridRoot == null) {
//					EditorUtility.DisplayDialog("OOPS!", "You need to set the Grid Root before populating the grid.", "Cool");	
//				}
//				else if(mProtoCell == null) {
//					EditorUtility.DisplayDialog("BOOM HEADSHOT!", "You need to set the ProtoCell from Resources before populating the grid.", "Cool");	
//				}
//			}
//		}
