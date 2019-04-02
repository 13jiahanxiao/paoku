using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(GameProfile))]
public class GameProfileInspector : Editor
{
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
};


