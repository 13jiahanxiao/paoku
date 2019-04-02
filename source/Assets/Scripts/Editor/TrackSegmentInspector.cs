using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Reflection;
using System.Collections.Generic;

[CustomEditor(typeof(TrackSegment))]
public class TrackSegmentInspector : Editor {

	TrackSegment mTrackSegment = null;
	TrackSegment.SegmentType selectedEnum = 0;
	
	public override void OnInspectorGUI ()
	{
		mTrackSegment = target as TrackSegment;
		
		//EditorGUIUtility.LookLikeControls();
		EditorGUIUtility.LookLikeInspector();
		EditorGUI.indentLevel = 0;
		
		TrackSegment.SegmentType newSelectedEnum = (TrackSegment.SegmentType)EditorGUILayout.EnumPopup("Populate from Enum", (Enum)selectedEnum);
		if(newSelectedEnum != selectedEnum)
		{
			if(mTrackSegment.Pieces == null)
			{
				mTrackSegment.Pieces = new List<TrackPiece.PieceType>();
			}
			mTrackSegment.Pieces.Clear();
			selectedEnum = newSelectedEnum;
		}
		
		if(newSelectedEnum != TrackSegment.SegmentType.NONE)
		{
			
			if(TrackSegment.SegmentTable.ContainsKey(newSelectedEnum) == true)
			{
				if(mTrackSegment.Pieces == null)
				{
					mTrackSegment.Pieces = new List<TrackPiece.PieceType>();
				}
				mTrackSegment.Pieces.Clear();
				
				List<TrackPiece.PieceType> list = TrackSegment.SegmentTable[newSelectedEnum];
				
				//- COPY THE LIST.
				mTrackSegment.Pieces.AddRange(list);
			}
		}
		//TrackPieceInspector.DrawSeparator();
		EditorGUI.indentLevel = 0;
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
}


