using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Ramp))]
public class RampEditor : Editor
{
	
	public override void OnInspectorGUI ()
	{
		EditorGUIUtility.LookLikeInspector ();
		bool update = false;
		var t = target as Ramp;
		if (Event.current.isMouse && Event.current.type == EventType.MouseUp) {
			update = true;
		}
		GUILayout.Space (10);
		GUILayout.BeginHorizontal ();
//		GUILayout.Label ("Texture Size");
//		var pS = t.size;
//		t.size = Mathf.Clamp (EditorGUILayout.IntField (t.size, "TextField"), 16, 1024);
//		
//		if (pS != t.size)
//			update = true;
//		var pB = t.blend;
//		t.blend = GUILayout.Toggle(t.blend, "Blend");
//		if (pB != t.blend)
//			update = true;
//		GUILayout.EndHorizontal ();
//		GUILayout.BeginHorizontal ();
//		GUILayout.Label (t.Texture, GUILayout.ExpandWidth (true));
//		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal ();
		
		RampColor kill = null;
		foreach (var c in t.colors) {
			GUILayout.BeginHorizontal ();
			var pC = c.color;
			c.color = EditorGUILayout.ColorField (c.color, GUILayout.MaxWidth (64));
			if (c.color != pC)
				update = true;
			c.position = EditorGUILayout.Slider (c.position, 0, 1, GUILayout.ExpandWidth (true));
			if (t.colors.Count > 2) {
				if (GUILayout.Button ("-", GUILayout.Width (16), GUILayout.Height (12))) {
					kill = c;
					update = true;
				}
			} else {
				GUILayout.Space (16);
			}
			GUILayout.EndHorizontal ();
		}
		if (kill != null)
			t.colors.Remove (kill);
		if (GUILayout.Button ("Add Color")) {
			var p = t.colors[t.colors.Count - 1];
			t.colors.Add (new RampColor (t.colors[0].color, Mathf.Lerp (p.position, 1, 0.5f)));
			update = true;
		}
		
		if (update) {
			t.UpdateRamp ();
			EditorUtility.SetDirty(t);
		}
		
		GUILayout.Space(8);
		GUILayout.BeginHorizontal();
//		if(GUILayout.Button("Bake New Texture")) {
//			BakeTexture(true);
//		}
//		if(GUILayout.Button("Replace Texture")) {
//			BakeTexture(false);
//		}
		
		GUILayout.EndHorizontal();
	}
	
//	void BakeTexture(bool unique) {
//		var t = target as Ramp;
//		var root = AssetDatabase.GetAssetPath (t);
//		var filename = System.IO.Path.GetFileNameWithoutExtension(root) + ".png";
//		root = System.IO.Path.GetDirectoryName(root);
//		string path;
//		if(unique) 
//			path = AssetDatabase.GenerateUniqueAssetPath (root + "/" + filename);
//		else
//			path = root + "/" + filename;
//		var bytes = t.Texture.EncodeToPNG();
//		System.IO.File.WriteAllBytes(path, bytes);
//		AssetDatabase.Refresh();
//	}


	[MenuItem("Assets/Create/Ramp")]
	public static void CreateRamp ()
	{
		var root = "Assets";
		if (Selection.activeObject != null) {
			root = AssetDatabase.GetAssetPath (Selection.activeObject);
			if (!System.IO.Directory.Exists (root))
				root = System.IO.Path.GetDirectoryName (root);
		}
		var path = AssetDatabase.GenerateUniqueAssetPath (root + "/ramp.asset");
		var asset = ScriptableObject.CreateInstance<Ramp> ();
		AssetDatabase.CreateAsset (asset, path);
		
	}
}
