using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObjectivesLegendaryEditor : EditorWindow 
{
	protected static Notify notify;
	Vector2 mScroll = Vector2.zero;
	bool mSaveNeeded = false;

	[MenuItem("TR2/Editors/Legendary Objectives Editor")]
	static public void OpenEditor()
	{
		EditorWindow.GetWindow<ObjectivesLegendaryEditor>(false, "Legendary Objectives Editor", true);
	}
	
	void Awake()
	{
		notify = new Notify(this.GetType().Name);	
	}
	
	void OnGUI()
	{
		//-- Load the file if its not yet loaded.
		if (ObjectivesManager.LegendaryObjectives == null || ObjectivesManager.LegendaryObjectives.Count == 0) 
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
			ob._id = ObjectivesManager.GetNextID(ObjectivesManager.LegendaryObjectives);
			ob._showFoldOut = true;
			ObjectivesManager.LegendaryObjectives.Add(ob);
		}
		string saveString = "Save";
		if (mSaveNeeded == true)
		{
			saveString = "Save*";
			GUI.contentColor = Color.yellow;
		}
		
		if (GUILayout.Button(saveString) == true)
		{
			ObjectivesManager.SaveFile(ObjectivesManager.LegendaryObjectives, "OZGameData/ObjectivesLegendary");
			mSaveNeeded = false;
			AssetDatabase.Refresh();
		}
		
		GUI.contentColor = Color.white;
		if (GUILayout.Button("Reload") == true)
		{
			AssetDatabase.Refresh();
			LoadFile();	//ObjectivesManager.LoadFile(ObjectivesManager.LegendaryObjectives, "OZGameData/ObjectivesLegendary");
			mSaveNeeded = false;
		}
		
		GUILayout.EndHorizontal();
		
		NGUIEditorTools.DrawSeparator();

		//-- List Panel
		mScroll = EditorGUILayout.BeginScrollView(mScroll);
		
		foreach (ObjectiveProtoData data in ObjectivesManager.LegendaryObjectives) 
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
			
			EditorGUI.indentLevel = 0;
			GUILayout.BeginHorizontal();
			data._showFoldOut = EditorGUILayout.Foldout(data._showFoldOut,data._title + ": " + engTitle/*data._internalTitle*/ + " (ID=" + data._id +")");
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
						ObjectivesManager.LegendaryObjectives.Remove(data);
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
			
		//	ObjectiveFilterType objFilterType = (ObjectiveFilterType)EditorGUILayout.EnumPopup("Filter Type", data._conditionList[0]._filterType);
		//	if (objFilterType != data._conditionList[0]._filterType)
		//	{
		//		data._conditionList[0]._filterType = objFilterType;
		//		mSaveNeeded = true;
		//	}
			
			RankRewardType rewType = (RankRewardType)EditorGUILayout.EnumPopup("Reward Type", data._rewardType);
			if (rewType != data._rewardType)
			{
				data._rewardType = rewType;
				mSaveNeeded = true;
			}
			
			int rewValue = (int)EditorGUILayout.IntField("Reward Value", (int)data._rewardValue);
			if (rewValue != data._rewardValue)
			{
				data._rewardValue = rewValue;
				mSaveNeeded = true;
			}
			
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
			int env = (int)EditorGUILayout.IntField("Environment ID", (int)data._environmentID);
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
		bool success = ObjectivesManager.LoadFile(ObjectivesManager.LegendaryObjectives, "OZGameData/ObjectivesLegendary");
		if(success == false)
		{
			GUILayout.Label("Failed to load ObjectivesLegendary.txt");
			notify.Debug("Failed to load ObjectivesLegendary.txt");
			return;
		}
		else
		{
			ObjectivesManager.LegendaryObjectives.Sort((a1, a2) => a1._id.CompareTo(a2._id));
		}		
	}
}


			
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


	//UIGrid gridRoot = null;
	//GameObject mProtoCell = null;
	

		//-- Load from Prefs
//		if (gridRoot == null) 
//		{
//			if (EditorPrefs.HasKey("ObjectivesManagerGridRoot") == true) 
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