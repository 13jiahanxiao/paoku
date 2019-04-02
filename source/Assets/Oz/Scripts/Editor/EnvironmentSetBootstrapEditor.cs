using UnityEngine;
using System.Collections;
using UnityEditor;

public class EnvironmentSetBootstrapEditor : EditorWindow {
	
	bool saveNeeded = false;
	
	/// <summary>
	/// Opens the environment set bootstrap editor.
	/// </summary>
	[MenuItem("TR2/Editors/EnvironmentSetBootstrap Editor")]
	static public void OpenEnvironmentSetBootstrapEditor() 
	{ 
		EditorWindow.GetWindow<EnvironmentSetBootstrapEditor>(false, "BootstrapEditor", true); 
	}

	/// <summary>
	/// Draw our GUI
	/// </summary>
	void OnGUI()
	{
		EditorGUIUtility.LookLikeControls(80f);

		//-- Top Panel
		GUI.contentColor = Color.white;
		
		GUILayout.BeginHorizontal();
		
		if (GUILayout.Button("Add") == true)
		{
			EnvironmentSetBootstrapData data = new EnvironmentSetBootstrapData ("ChangeMe", "ChangeMe", true , false);
			EnvironmentSetBootstrap.BootstrapList.Add(data);		
		}
		
		GUI.contentColor = saveNeeded ? Color.yellow: Color.white;
		if (GUILayout.Button(saveNeeded ? "Save*" : "Save") == true)
		{
			bool valid = EnvironmentSetBootstrap.ValidateValues();
			if (valid)
			{
				bool saved = EnvironmentSetBootstrap.SaveFile();
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
				UnityEditor.EditorUtility.DisplayDialog("Error", "You can not have duplicate values", "Ok");
			}
		}
		
		GUI.contentColor = Color.white;
		if(GUILayout.Button("Reload") == true)
		{
			AssetDatabase.Refresh();
			bool result = EnvironmentSetBootstrap.LoadFile();
			if (result)
			{
				UnityEditor.EditorUtility.DisplayDialog("Success", "Data Reloaded", "Ok");
			}
			else
			{
				UnityEditor.EditorUtility.DisplayDialog("Error", "Error loading the file", "Ok");
			}
		}
		GUILayout.EndHorizontal();
		NGUIEditorTools.DrawSeparator();		
		
		// now display the bootstrap info		
		foreach (EnvironmentSetBootstrapData data in EnvironmentSetBootstrap.BootstrapList) 
		{
			string tempString = (string)EditorGUILayout.TextField(EnvironmentSetBootstrapData.CodeKey, (data.SetCode != null) ? data.SetCode : "");
			if (tempString.CompareTo(data.SetCode) != 0) 
			{
				data.SetCode = tempString;
				saveNeeded = true;
			}
			string tempName = (string) EditorGUILayout.TextField(EnvironmentSetBootstrapData.AssetBundleNameKey, (data.AssetBundleName != null) ? data.AssetBundleName : "");
			if (tempName.CompareTo(data.AssetBundleName) != 0)
			{
				data.AssetBundleName = tempName;
				saveNeeded = true;
			}
			bool tempEmbedded = (bool) EditorGUILayout.Toggle(EnvironmentSetBootstrapData.EmbeddedKey, data.Embedded);
			if (tempEmbedded != data.Embedded)
			{
				data.Embedded = tempEmbedded;
				saveNeeded = true;
			}
			bool tempEmbeddedAssetBundle = (bool) EditorGUILayout.Toggle(EnvironmentSetBootstrapData.EmbeddedAssetBundleKey, data.EmbeddedAssetBundle);
			if (tempEmbeddedAssetBundle != data.EmbeddedAssetBundle)
			{
				data.EmbeddedAssetBundle = tempEmbeddedAssetBundle;
				saveNeeded = true;
			}
			EditorGUILayout.Space();
		}
		
	}	
}
 