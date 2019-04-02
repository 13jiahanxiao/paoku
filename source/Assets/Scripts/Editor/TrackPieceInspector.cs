using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor(typeof(TrackPiece))]
public class TrackPieceInspector : Editor
{
	//TrackPiece 	mTrackPiece = null;
	
	static Texture2D mWhiteTex;
//	static Texture2D mBackdropTex;
//	static Texture2D mContrastTex;
	static Texture2D mGradientTex;
	
	static Texture2D CreateDummyTex ()
	{
		Texture2D tex = new Texture2D(1, 1);
		tex.name = "[Generated] Dummy Texture";
		tex.SetPixel(0, 0, Color.white);
		tex.Apply();
		tex.filterMode = FilterMode.Point;
		return tex;
	}
	
	/// <summary>
	/// Returns a blank usable 1x1 white texture.
	/// </summary>

	static public Texture2D blankTexture
	{
		get
		{
			if (mWhiteTex == null) mWhiteTex = CreateDummyTex();
			return mWhiteTex;
		}
	}
	
	static public Texture2D gradientTexture
	{
		get
		{
			if (mGradientTex == null) mGradientTex = CreateGradientTex();
			return mGradientTex;
		}
	}
	
	static Texture2D CreateGradientTex ()
	{
		Texture2D tex = new Texture2D(1, 16);
		tex.name = "[Generated] Gradient Texture";

		Color c0 = new Color(1f, 1f, 1f, 0f);
		Color c1 = new Color(1f, 1f, 1f, 0.4f);

		for (int i = 0; i < 16; ++i)
		{
			float f = Mathf.Abs((i / 15f) * 2f - 1f);
			f *= f;
			tex.SetPixel(0, i, Color.Lerp(c0, c1, f));
		}

		tex.Apply();
		tex.filterMode = FilterMode.Bilinear;
		return tex;
	}
	
	static public void DrawSeparator ()
	{
		GUILayout.Space(12f);

		if (Event.current.type == EventType.Repaint)
		{
			Texture2D tex = blankTexture;
			Rect rect = GUILayoutUtility.GetLastRect();
			GUI.color = new Color(0f, 0f, 0f, 0.25f);
			GUI.DrawTexture(new Rect(0f, rect.yMin + 6f, Screen.width, 4f), tex);
			GUI.DrawTexture(new Rect(0f, rect.yMin + 6f, Screen.width, 1f), tex);
			GUI.DrawTexture(new Rect(0f, rect.yMin + 9f, Screen.width, 1f), tex);
			GUI.color = Color.white;
		}
	}
	
	static public Rect DrawHeader (string text)
	{
		GUILayout.Space(28f);
		Rect rect = GUILayoutUtility.GetLastRect();
		rect.yMin += 5f;
		rect.yMax -= 4f;
		rect.width = Screen.width;

		if (Event.current.type == EventType.Repaint)
		{
			GUI.color = Color.black;
			GUI.DrawTexture(new Rect(0f, rect.yMin, Screen.width, rect.yMax - rect.yMin), gradientTexture);
			GUI.color = new Color(0f, 0f, 0f, 0.25f);
			GUI.DrawTexture(new Rect(0f, rect.yMin, Screen.width, 1f), blankTexture);
			GUI.DrawTexture(new Rect(0f, rect.yMax - 1, Screen.width, 1f), blankTexture);
			GUI.color = Color.white;
			GUI.Label(new Rect(rect.x + 4f, rect.y, rect.width - 4, rect.height), text, EditorStyles.boldLabel);
		}
		return rect;
	}
	
	
	public override void OnInspectorGUI ()
	{
		//mTrackPiece = target as TrackPiece;
		
		//EditorGUIUtility.LookLikeControls();
		EditorGUIUtility.LookLikeInspector();
			
//		EditorGUI.indentLevel = 0;
//		TrackPieceInspector.DrawHeader("Static Data");
//		EditorGUI.indentLevel = 1;
//		EditorGUILayout.LabelField("Max Coins Per Run", TrackPiece.sMaxCoinsPerRun.ToString());
//		TrackPiece.sMinSpacesBetweenCoinRuns = EditorGUILayout.IntField("Min Spaces Between Coin Runs", TrackPiece.sMinSpacesBetweenCoinRuns);
//		TrackPieceInspector.DrawSeparator();
//		EditorGUI.indentLevel = 0;
		DrawDefaultInspector ();
		
		//EditorGUILayout.LabelField ("Some help", "Some other text");
        //target.speed = EditorGUILayout.Slider ("Speed", target.speed, 0, 100);
        // Show default inspector property editor
        
	}
	
	public void OnSceneGUI()
	{
		TrackPiece tp = target as TrackPiece;
		GUI.skin.label.normal.textColor = Color.red;
		int BICount = 0;
		if(tp.BonusItems != null)
		{
			BICount = tp.BonusItems.Count;	
		}
		
		float distSqr = 0.0f;
		float testDistSqr = 0.0f;
		float testDistSqrHalf = 0.0f;
		if(GameController.SharedInstance && GameController.SharedInstance.Player)
		{
			distSqr = Vector3.SqrMagnitude(GameController.SharedInstance.Player.transform.position-tp.transform.position);
			testDistSqr = GameController.SharedInstance.AlphaCullDistance*GameController.SharedInstance.AlphaCullDistance;	
			testDistSqrHalf = testDistSqr*0.5f;
		}
		
		
		Handles.Label(tp.transform.position + (Vector3.up * 8), 
			"CountSinceLastTurn = " + tp.DistanceSinceLastTurn +
			"\nCountSinceLastObstacle = " + tp.DistanceSinceLastObstacle +
			"\nCountSinceLastEnvironmentChange = " + tp.DistanceSinceLastEnvironmentChange +
			"\nCountSinceLastBonusItem = " + tp.DistanceSinceLastBonusItem + 
			"\nCountSinceLastGem = " + tp.DistanceSinceLastGem + 
			"\nCountSinceLastTornadoToken = " + tp.DistanceSinceLastTornadoToken + 
			"\nCountSinceLastCoinRun = " + tp.DistanceSinceLastCoinRun + 
			"\nDistanceSinceLastBalloon = " + tp.DistanceSinceLastBalloon +
			"\nBonusItemCount = " + BICount +
			"\nDist = " + distSqr + " < " + testDistSqr +
			"\nHalfDist = " + distSqr + " < " + testDistSqrHalf);
		
		GUI.skin.label.normal.textColor = Color.white;
		
		
		
		//Handles.BeginGUI();
//		GUILayout.BeginArea(backgroundRect);
//		GUILayout.Label("LastBonusItemCount = " + tp.DebugCountSinceLastBonusItem);
//		GUILayout.EndArea();
		
//		Rect backgroundRect = new Rect(Screen.width-100, Screen.height-80, 90, 50);
//		Handles.BeginGUI();
//		GUI.backgroundColor = Color.blue;
//		GUI.Box(backgroundRect,"FOOOO");
//        Handles.EndGUI();
		
		//Handles.EndGUI();
	}
}
