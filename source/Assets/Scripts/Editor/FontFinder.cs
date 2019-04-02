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

public class FontFinder : EditorWindow
{
	protected static Notify notify;
	
	[MenuItem("TR2/Font Finder")]
	static public void OpenFontFinder ()
	{
		EditorWindow.GetWindow<FontFinder>(false, "Font Finder", true);
	}
	
	static private GameObject GetChildByName(string name, GameObject go)
    {
        GameObject result = null;
        Transform t = go.transform.FindChild(name);

        if (t != null)
            return t.gameObject;

        for(int i=0; i<go.transform.childCount; i++)
		{
			t = go.transform.GetChild(i);
			result = GetChildByName(name, t.gameObject);
			if(result != null)
				return result;
		}
		return null;
    }
	
	void Awake()
	{
		notify = new Notify(this.GetType().Name);	
	}
	
	Vector2 mScroll = Vector2.zero;
	
	void OnGUI ()
	{
		//-- Load the file if its not yet loaded.
		if(PowerStore.Powers == null || PowerStore.Powers.Count == 0) {
			bool success = PowerStore.LoadFile();
			if(success == false) {
				GUILayout.Label("Failed to load powers.txt");
				notify.Warning("Failed to load powers.txt");
				return;
			}
		}
		
		//-- Load from Prefs
		EditorGUIUtility.LookLikeControls(80f);
		GUILayout.Space(6f);
		
		GUI.contentColor = Color.white;
		
		//-- List Panel
		mScroll = EditorGUILayout.BeginScrollView(mScroll);
		
		Dictionary<string, int> table = new Dictionary<string, int> ();
		object[] obj = GameObject.FindObjectsOfType(typeof(UILabel));
		foreach (object item in obj) {
			UILabel label = (UILabel)item;
			if(label.font == null)
				continue;
			
			if(table.ContainsKey(label.font.name)) {
				int count = table[label.font.name] + 1;
				table[label.font.name] = count;
			}
			else {
				table.Add (label.font.name, 1);
			}
		}
		
		foreach(KeyValuePair<string, int> item in table) {
			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(item.Value.ToString(), item.Key);
			GUILayout.EndHorizontal();
		}
//  foreach (object o in obj)
//  {
//       GameObject g = (GameObject) o;
//       notify.Debug(g.name);
//  }
//			
////			EditorGUILayout.PrefixLabel("Buff Desc.");
////			tempString = EditorGUILayout.TextArea(data.BuffDescription);
////			
////			if(tempString.CompareTo(data.BuffDescription) != 0) {
////				data.BuffDescription = tempString;
////				mSaveNeeded = true;
////			}
//			
//			EditorGUILayout.Space();
//		}
		
		EditorGUILayout.EndScrollView();
	}
}

