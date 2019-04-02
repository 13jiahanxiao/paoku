using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;
using System.IO;

public class EnvironmentSetDataEditor : EditorWindow{
	
	bool saveNeeded = false;
	protected static Notify notify;
	EnvironmentSetData curData = null;
	
	[MenuItem("TR2/Editors/EnvironmentSetDataEditor Editor")]
	static public void OpenEnvironmentSetDataEditor() 
	{ 
		if (notify == null) {
			notify = new Notify("EnvironmentSetDataEditor");
		}
		EditorWindow.GetWindow<EnvironmentSetDataEditor>(false, "Environment Set Bootstrap Editor", true); 
	}
	
	/// <summary>
	/// Draw our GUI
	/// </summary>
	void OnGUI()
	{
		EditorGUIUtility.LookLikeControls(120f);

		//-- Top Panel
		GUI.contentColor = Color.white;
		
		GUILayout.BeginHorizontal();
		
		if (GUILayout.Button("New") == true)
		{
			curData = new EnvironmentSetData();
		}
		
		GUI.contentColor = saveNeeded ? Color.yellow: Color.white;
		if (GUILayout.Button(saveNeeded ? "Save*" : "Save") == true)
		{
			doSave();
		}
		
		GUI.contentColor = Color.white;
		if(GUILayout.Button("Load") == true)
		{
			doLoad ();
		}
		GUILayout.EndHorizontal();
		NGUIEditorTools.DrawSeparator();		
		
		EnvironmentSetDataGui(curData);
	}
	
	/// <summary>
	/// prompt the user for the env set data file to open, and load it
	/// </summary>
	void doLoad()
	{
		string openPath = Application.dataPath + Path.DirectorySeparatorChar + "Resources" + Path.DirectorySeparatorChar + 
			EnvironmentSetBootstrap.EnvironmentSetResourcesDirectory ;
 		string fullPath = EditorUtility.OpenFilePanel("Load Environment Set Data",openPath,"txt");	
		string baseName = Path.GetFileNameWithoutExtension(fullPath);
		EnvironmentSetData envSetData;
		bool loaded = EnvironmentSetData.LoadFile (baseName, out envSetData, true );
		if (loaded)
		{
			curData = envSetData;
			UnityEditor.EditorUtility.DisplayDialog("Success", "Environment Set data file loaded", "Ok");
		}
		else
		{
			curData = envSetData;
			UnityEditor.EditorUtility.DisplayDialog("Error", "There was an error loading the data file", "Ok");
		}
	}
	
	/// <summary>
	/// triggered when he clicked on save button, try to save data
	/// </summary>
	void doSave()
	{
		if (curData != null)
		{
			bool valid = curData.ValidateValues();
			if (valid)
			{
				bool saved = curData.SaveFile();
				if (saved)
				{
					UnityEditor.EditorUtility.DisplayDialog("Success", "Data Saved", "Ok");
					AssetDatabase.Refresh();
					saveNeeded = false;
				}
				else
				{
					UnityEditor.EditorUtility.DisplayDialog("Error", "Data is valid but there was an error saving the files", "Ok");
				}
			}
			else
			{
				// some of the files got moved out from the resources directory, so it may now fail, but it will resolve correctly
				// in the asset bundle
				UnityEditor.EditorUtility.DisplayDialog("Error", "Some data may be invalid, see the console log. Forcing a save anyway", "Ok");
				bool saved = curData.SaveFile();
				if (saved)
				{
					UnityEditor.EditorUtility.DisplayDialog("Success", "Data Saved", "Ok");
					AssetDatabase.Refresh();
					saveNeeded = false;
				}
				else
				{
					UnityEditor.EditorUtility.DisplayDialog("Error", "Data is valid but there was an error saving the files", "Ok");
				}
			}
		}
		else
		{
			UnityEditor.EditorUtility.DisplayDialog("Error", "Nothing to save", "Ok");
		}
	}

	
	/// <summary>
	/// draw the environment set data gui controls
	/// </summary>
	void EnvironmentSetDataGui(System.Object obj)
	{
		if (obj != null)
		{
			System.Type myType = obj.GetType();
			FieldInfo[] myFieldInfo;
	        myFieldInfo = myType.GetFields(BindingFlags.Public | BindingFlags.Instance);
			
			foreach ( FieldInfo fieldInfo in myFieldInfo)
	        {
				//FieldInfo fieldInfo = myFieldInfo[i];
				System.Type fieldType = fieldInfo.FieldType;
				string memberName = fieldInfo.Name;
				notify.Info ("memberName = " + memberName + " fieldType = " + fieldType);
				
				// unfortunately we can't do a switch on fieldType
				if (fieldType == typeof(string))
				{
					string curValue = (string) fieldInfo.GetValue(obj);
					string tempString = (string)EditorGUILayout.TextField(memberName, curValue);
					if (tempString != curValue)
					{
						fieldInfo.SetValue(curData, tempString);
						saveNeeded = true;	
					}
				}
				else if ( fieldType == typeof(int))
				{
					int curValue = (int) fieldInfo.GetValue(curData);
					int tempInt = (int)EditorGUILayout.IntField(memberName, curValue);
					if (tempInt != curValue)
					{
						fieldInfo.SetValue(curData, tempInt);
						saveNeeded = true;	
					}				
				}
				else if ( fieldType == typeof(float))
				{
					float curValue = (float) fieldInfo.GetValue(curData);
					float tempFloat = (float)EditorGUILayout.FloatField(memberName, curValue);
					if (tempFloat != curValue)
					{
						fieldInfo.SetValue(curData, tempFloat);
						saveNeeded = true;	
					}				
				}
				else
				{
					notify.Warning("don't know how to display " + memberName + " of type " + fieldType);
				}
	        }			
		}
	}
}
