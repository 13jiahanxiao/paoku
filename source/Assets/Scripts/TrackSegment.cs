using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
public class TrackSegment : ScriptableObject{
	protected static Notify notify;

	//-- Dynamic Pieces set in Editor
	public List<TrackPiece.PieceType> Pieces = null;
	
	//-- Static Data for Segments.
	//-- STEP 1: Add a new ENUM.
	public enum SegmentType
	{
		NONE					= 0 ,
		Intro						,
		RightLedgeGapLeftLedge		,
		DEMO						,
		QueuedOne					,
		MineDuck					,
		WWIntro						,
		DFIntro						,
		YBRIntro					,
		ECIntro						,
		TutorialJump				,
		TutorialFinley				,
		TutorialTurn				,
		TutorialSlide				,
		TutorialTilt				,
		TutorialEnemy				,
		TutorialMeter				,
		TutorialBalloon				,
		Total						
	}
	
	public enum SegmentDifficulty
	{
		Tutorial,
		Easy,
		Medium,
		Hard,
		Impossible
	}
	
	private enum SegmentEnvSet
	{
		Machu = 0,
		WhimsieWoods =1,
		NONE =2 //Always put this as last enum.
	}
	
	public enum SegmentEnv
	{
		env0 = 0,
		env1,
		env2
	}
	
	private void OnEnable()
	{
		if ( notify  != null)
		{
			notify = new Notify( this.GetType().Name);	
		}
	}
	

	//-- STEP 2: Add a method to fill out a list of pieces.

	private static void WWIntroTrack()
	{
		List<TrackPiece.PieceType> list = new List<TrackPiece.PieceType>();
		list.Add (TrackPiece.PieceType.kTPForestStraight);
		list.Add (TrackPiece.PieceType.kTPForestStraight);
		list.Add (TrackPiece.PieceType.kTPForestStraight);
		list.Add (TrackPiece.PieceType.kTPForestSlightLeft);
		list.Add (TrackPiece.PieceType.kTPForestSlightRight);
		list.Add (TrackPiece.PieceType.kTPForestStraight);
		//list.Add (TrackPiece.PieceType.kTPZipLine);
		
		mSegmentTable.Add (SegmentType.WWIntro, list);
		mDifficultyBySegmentType[(int)SegmentType.WWIntro] = SegmentDifficulty.Tutorial;
		mEnvSetBySegmentType[(int)SegmentType.WWIntro] = SegmentEnvSet.NONE;
		mEnvBySegmentType[(int)SegmentType.WWIntro] = SegmentEnv.env0;
	}
	
	private static void DFIntroTrack()
	{
		List<TrackPiece.PieceType> list = new List<TrackPiece.PieceType>();
		list.Add (TrackPiece.PieceType.kTPDarkForestStraight);
		list.Add (TrackPiece.PieceType.kTPStraightFlat);
		list.Add (TrackPiece.PieceType.kTPDarkForestSlightLeft);
		list.Add (TrackPiece.PieceType.kTPStraightFlat);
		list.Add (TrackPiece.PieceType.kTPDarkForestSlightRight);
		list.Add (TrackPiece.PieceType.kTPStraightFlat);
		list.Add (TrackPiece.PieceType.kTPDarkForestStraight);
		list.Add (TrackPiece.PieceType.kTPStraightFlat);
		//list.Add (TrackPiece.PieceType.kTPZipLine);
		
		mSegmentTable.Add (SegmentType.DFIntro, list);
		mDifficultyBySegmentType[(int)SegmentType.DFIntro] = SegmentDifficulty.Tutorial;
		mEnvSetBySegmentType[(int)SegmentType.DFIntro] = SegmentEnvSet.NONE;
		mEnvBySegmentType[(int)SegmentType.DFIntro] = SegmentEnv.env0;
	}
	
	private static void YBRIntroTrack()
	{
		List<TrackPiece.PieceType> list = new List<TrackPiece.PieceType>();
		list.Add (TrackPiece.PieceType.kTPFieldsStraight);	//Making this Straight instead of StraightFlat, so Oz doesn't clip through the intro piece
		list.Add (TrackPiece.PieceType.kTPFieldsStraight);
		list.Add (TrackPiece.PieceType.kTPFieldsStraightFlat);
		list.Add (TrackPiece.PieceType.kTPFieldsUndulateUp);
		list.Add (TrackPiece.PieceType.kTPFieldsSlightLeft);
		list.Add (TrackPiece.PieceType.kTPFieldsSlightRight);
		//list.Add (TrackPiece.PieceType.kTPZipLine);
		
		mSegmentTable.Add (SegmentType.YBRIntro, list);
		mDifficultyBySegmentType[(int)SegmentType.YBRIntro] = SegmentDifficulty.Tutorial;
		mEnvSetBySegmentType[(int)SegmentType.YBRIntro] = SegmentEnvSet.NONE;
		mEnvBySegmentType[(int)SegmentType.YBRIntro] = SegmentEnv.env0;
	}

	private static void ECIntroTrack()
	{
		List<TrackPiece.PieceType> list = new List<TrackPiece.PieceType>();
		list.Add (TrackPiece.PieceType.kTPStraight);
		list.Add (TrackPiece.PieceType.kTPStraight);
		list.Add (TrackPiece.PieceType.kTPStraight);
		list.Add (TrackPiece.PieceType.kTPCurveLeft);
		list.Add (TrackPiece.PieceType.kTPStraight);
		list.Add (TrackPiece.PieceType.kTPCurveRight);
		//list.Add (TrackPiece.PieceType.kTPZipLine);
		
		mSegmentTable.Add (SegmentType.ECIntro, list);
		mDifficultyBySegmentType[(int)SegmentType.ECIntro] = SegmentDifficulty.Tutorial;
		mEnvSetBySegmentType[(int)SegmentType.ECIntro] = SegmentEnvSet.NONE;
		mEnvBySegmentType[(int)SegmentType.ECIntro] = SegmentEnv.env0;
	}	
	
	private static void IntroTrack()
	{
		List<TrackPiece.PieceType> list = new List<TrackPiece.PieceType>();
		list.Add (TrackPiece.PieceType.kTPStraightFlat);
		list.Add (TrackPiece.PieceType.kTPStraightFlat);
		list.Add (TrackPiece.PieceType.kTPStraightFlat);
		//list.Add (TrackPiece.PieceType.kTPZipLine);
		
		mSegmentTable.Add (SegmentType.Intro, list);
		mDifficultyBySegmentType[(int)SegmentType.Intro] = SegmentDifficulty.Tutorial;
		mEnvSetBySegmentType[(int)SegmentType.Intro] = SegmentEnvSet.NONE;
		mEnvBySegmentType[(int)SegmentType.Intro] = SegmentEnv.env0;
	}
	
	
	//Queued segments
	
	private static void TutorialJump()
	{
		List<TrackPiece.PieceType> list = new List<TrackPiece.PieceType>();
		list.Add (TrackPiece.PieceType.kTPForestStraightShort);
		list.Add (TrackPiece.PieceType.kTPForestSlightLeft);
		list.Add (TrackPiece.PieceType.kTPForestStraight);
		list.Add (TrackPiece.PieceType.kTPForestStraightShort);
		list.Add (TrackPiece.PieceType.kTPForestSlightRight);
		list.Add (TrackPiece.PieceType.kTPForestStraight);
		list.Add (TrackPiece.PieceType.kTPForestSlightLeft);
		list.Add (TrackPiece.PieceType.kTPForestStraight);
		list.Add (TrackPiece.PieceType.kTPForestJumpOver);
		
//		list.Add (TrackPiece.PieceType.kTPForestStraight);
//		list.Add (TrackPiece.PieceType.kTPForestStraightShort);
//		list.Add (TrackPiece.PieceType.kTPForestStraight);
//		list.Add (TrackPiece.PieceType.kTPForestJumpOver);
		
		list.Add (TrackPiece.PieceType.kTPForestSlightLeft);
		list.Add (TrackPiece.PieceType.kTPForestStraightShort);
		list.Add (TrackPiece.PieceType.kTPForestSmallGap);
		list.Add (TrackPiece.PieceType.kTPForestSlightRight);
		

		mSegmentTable.Add (SegmentType.TutorialJump, list);
		mDifficultyBySegmentType[(int)SegmentType.TutorialJump] = SegmentDifficulty.Tutorial;
		mEnvSetBySegmentType[(int)SegmentType.TutorialJump] = SegmentEnvSet.NONE;
		mEnvBySegmentType[(int)SegmentType.TutorialJump] = SegmentEnv.env0;
	}

	
	private static void TutorialFinley()
	{
		List<TrackPiece.PieceType> list = new List<TrackPiece.PieceType>();
		list.Add (TrackPiece.PieceType.kTPForestStraight);
		list.Add (TrackPiece.PieceType.kTPForestSlightRight);
		list.Add (TrackPiece.PieceType.kTPForestStraight);
		list.Add (TrackPiece.PieceType.kTPForestSlightLeft);
		list.Add (TrackPiece.PieceType.kTPForestStraight);
		//list.Add (TrackPiece.PieceType.kTPForestSlights);
		list.Add (TrackPiece.PieceType.kTPForestStraightShort);
		list.Add (TrackPiece.PieceType.kTPForestSmallGap);
		list.Add (TrackPiece.PieceType.kTPForestLargeGap);
		list.Add (TrackPiece.PieceType.kTPForestSlightLeft);
		list.Add (TrackPiece.PieceType.kTPForestTurnRight);
		list.Add (TrackPiece.PieceType.kTPForestStraight);
		list.Add (TrackPiece.PieceType.kTPForestSlightRight);
		list.Add (TrackPiece.PieceType.kTPForestTurnLeft);
		list.Add (TrackPiece.PieceType.kTPForestSlightRight);
		list.Add (TrackPiece.PieceType.kTPForestStraight);
		list.Add (TrackPiece.PieceType.kTPForestSlightLeft);
		list.Add (TrackPiece.PieceType.kTPForestStraight);
		list.Add (TrackPiece.PieceType.kTPForestSlightRight);
		list.Add (TrackPiece.PieceType.kTPForestStraight);
		
		

		mSegmentTable.Add (SegmentType.TutorialFinley, list);
		mDifficultyBySegmentType[(int)SegmentType.TutorialFinley] = SegmentDifficulty.Tutorial;
		mEnvSetBySegmentType[(int)SegmentType.TutorialFinley] = SegmentEnvSet.NONE;
		mEnvBySegmentType[(int)SegmentType.TutorialFinley] = SegmentEnv.env0;
	}
	
	private static void TutorialTurn()
	{
		List<TrackPiece.PieceType> list = new List<TrackPiece.PieceType>();
		list.Add (TrackPiece.PieceType.kTPForestSlightRight);
		list.Add (TrackPiece.PieceType.kTPForestStraight);
		list.Add (TrackPiece.PieceType.kTPForestStraightShort);
		list.Add (TrackPiece.PieceType.kTPForestSlightLeft);
		list.Add (TrackPiece.PieceType.kTPForestStraight);
		//list.Add (TrackPiece.PieceType.kTPForestStraight);
		list.Add (TrackPiece.PieceType.kTPForestStraightShort);
		list.Add (TrackPiece.PieceType.kTPForestTurnLeft);
		
		list.Add (TrackPiece.PieceType.kTPForestStraightShort);
		list.Add (TrackPiece.PieceType.kTPForestSlightLeft);
		list.Add (TrackPiece.PieceType.kTPForestStraight);
		list.Add (TrackPiece.PieceType.kTPForestTurnRight);
		
		
//		list.Add (TrackPiece.PieceType.kTPForestStraight);
//		list.Add (TrackPiece.PieceType.kTPForestSlights);
//		list.Add (TrackPiece.PieceType.kTPForestTurnLeft);
		
		
		list.Add (TrackPiece.PieceType.kTPForestStraight);
		//list.Add (TrackPiece.PieceType.kTPForestSlights);
		

		mSegmentTable.Add (SegmentType.TutorialTurn, list);
		mDifficultyBySegmentType[(int)SegmentType.TutorialTurn] = SegmentDifficulty.Tutorial;
		mEnvSetBySegmentType[(int)SegmentType.TutorialTurn] = SegmentEnvSet.NONE;
		mEnvBySegmentType[(int)SegmentType.TutorialTurn] = SegmentEnv.env0;
	}

	private static void TutorialSlide()
	{
		List<TrackPiece.PieceType> list = new List<TrackPiece.PieceType>();
		list.Add (TrackPiece.PieceType.kTPForestSlightRight);
		list.Add (TrackPiece.PieceType.kTPForestStraightShort);
		list.Add (TrackPiece.PieceType.kTPForestSlightLeft);
		list.Add (TrackPiece.PieceType.kTPForestStraight);
		list.Add (TrackPiece.PieceType.kTPForestSlightLeft);	
		list.Add (TrackPiece.PieceType.kTPForestStraightShort);
		list.Add (TrackPiece.PieceType.kTPForestSlideUnder);
		
//		list.Add (TrackPiece.PieceType.kTPForestStraight);
//		list.Add (TrackPiece.PieceType.kTPForestStraightShort);
//		list.Add (TrackPiece.PieceType.kTPForestStraight);
//		list.Add (TrackPiece.PieceType.kTPForestSlideUnderSlights);
		
		list.Add (TrackPiece.PieceType.kTPForestStraight);
		list.Add (TrackPiece.PieceType.kTPForestSlightRight);
		list.Add (TrackPiece.PieceType.kTPForestStraightShort);
		list.Add (TrackPiece.PieceType.kTPForestAnimatedJumpOrSlide);	
		list.Add (TrackPiece.PieceType.kTPForestSlightLeft);

		mSegmentTable.Add (SegmentType.TutorialSlide, list);
		mDifficultyBySegmentType[(int)SegmentType.TutorialSlide] = SegmentDifficulty.Tutorial;
		mEnvSetBySegmentType[(int)SegmentType.TutorialSlide] = SegmentEnvSet.NONE;
		mEnvBySegmentType[(int)SegmentType.TutorialSlide] = SegmentEnv.env0;
	}

	private static void TutorialEnemy()
	{
		List<TrackPiece.PieceType> list = new List<TrackPiece.PieceType>();
		
		list.Add (TrackPiece.PieceType.kTPForestExit);
		list.Add (TrackPiece.PieceType.kTPCurveLeft);
		list.Add (TrackPiece.PieceType.kTPStraight);
		//list.Add (TrackPiece.PieceType.kTPCurveRight);
		
//		list.Add (TrackPiece.PieceType.kTPCurveRight);
//		list.Add (TrackPiece.PieceType.kTPStraight);
//		list.Add (TrackPiece.PieceType.kTPCurveRight);
//		list.Add (TrackPiece.PieceType.kTPStraight);
		list.Add (TrackPiece.PieceType.kTPEnemyMonkey);
		
		list.Add (TrackPiece.PieceType.kTPStraight);
		
		

		mSegmentTable.Add (SegmentType.TutorialEnemy, list);
		mDifficultyBySegmentType[(int)SegmentType.TutorialEnemy] = SegmentDifficulty.Tutorial;
		mEnvSetBySegmentType[(int)SegmentType.TutorialEnemy] = SegmentEnvSet.NONE;
		mEnvBySegmentType[(int)SegmentType.TutorialEnemy] = SegmentEnv.env0;
	}
	
	
	private static void TutorialTilt()
	{
		List<TrackPiece.PieceType> list = new List<TrackPiece.PieceType>();
		
		
		list.Add (TrackPiece.PieceType.kTPCurveRight);
		list.Add (TrackPiece.PieceType.kTPStraight);
		list.Add (TrackPiece.PieceType.kTPCurveRight);
		list.Add (TrackPiece.PieceType.kTPStraight);
		list.Add (TrackPiece.PieceType.kTPCurveRight);

				
		list.Add (TrackPiece.PieceType.kTPCurveLeft);
		list.Add (TrackPiece.PieceType.kTPStraight);
		list.Add (TrackPiece.PieceType.kTPCurveRight);
		list.Add (TrackPiece.PieceType.kTPAnimatedLeftLedge);
		
		
		list.Add (TrackPiece.PieceType.kTPCurveRight);
		list.Add (TrackPiece.PieceType.kTPCurveRight);
		list.Add (TrackPiece.PieceType.kTPCurveRight);
		list.Add (TrackPiece.PieceType.kTPStraightFlat);
		list.Add (TrackPiece.PieceType.kTPCurveLeft);
		list.Add (TrackPiece.PieceType.kTPCurveLeft);
		list.Add (TrackPiece.PieceType.kTPCurveLeft);
		list.Add (TrackPiece.PieceType.kTPStraight);
		list.Add (TrackPiece.PieceType.kTPStraight);
		list.Add (TrackPiece.PieceType.kTPCurveRight);
		list.Add (TrackPiece.PieceType.kTPCurveRight);
		list.Add (TrackPiece.PieceType.kTPCurveRight);
		list.Add (TrackPiece.PieceType.kTPStraight);
		
		
		list.Add (TrackPiece.PieceType.kTPForestEntrance);
		
		mSegmentTable.Add (SegmentType.TutorialTilt, list);
		mDifficultyBySegmentType[(int)SegmentType.TutorialTilt] = SegmentDifficulty.Tutorial;
		mEnvSetBySegmentType[(int)SegmentType.TutorialTilt] = SegmentEnvSet.NONE;
		mEnvBySegmentType[(int)SegmentType.TutorialTilt] = SegmentEnv.env0;
	}
	

	private static void TutorialMeter()
	{
		List<TrackPiece.PieceType> list = new List<TrackPiece.PieceType>();
		list.Add (TrackPiece.PieceType.kTPForestSlightLeft);
		list.Add (TrackPiece.PieceType.kTPForestStraight);
		list.Add (TrackPiece.PieceType.kTPForestSlightRight);
		list.Add (TrackPiece.PieceType.kTPForestStraight);
		list.Add (TrackPiece.PieceType.kTPForestSlightRight);
		list.Add (TrackPiece.PieceType.kTPForestStraightShort);
		list.Add (TrackPiece.PieceType.kTPForestStraight);
		list.Add (TrackPiece.PieceType.kTPForestSlightLeft);
		list.Add (TrackPiece.PieceType.kTPForestStraight);
		list.Add (TrackPiece.PieceType.kTPForestSlightLeft);
		list.Add (TrackPiece.PieceType.kTPForestStraightShort);
		list.Add (TrackPiece.PieceType.kTPForestStraight);
		
		list.Add (TrackPiece.PieceType.kTPForestExit);
		list.Add (TrackPiece.PieceType.kTPStraight);
		
		mSegmentTable.Add (SegmentType.TutorialMeter, list);
		mDifficultyBySegmentType[(int)SegmentType.TutorialMeter] = SegmentDifficulty.Tutorial;
		mEnvSetBySegmentType[(int)SegmentType.TutorialMeter] = SegmentEnvSet.NONE;
		mEnvBySegmentType[(int)SegmentType.TutorialMeter] = SegmentEnv.env0;
	}
	
	private static void TutorialBalloon()
	{
		List<TrackPiece.PieceType> list = new List<TrackPiece.PieceType>();
		
		list.Add (TrackPiece.PieceType.kTPCurveLeft);
		list.Add (TrackPiece.PieceType.kTPStraight);
		list.Add (TrackPiece.PieceType.kTPCurveRight);
		list.Add (TrackPiece.PieceType.kTPCurveLeft);
		list.Add (TrackPiece.PieceType.kTPStraight);
		list.Add (TrackPiece.PieceType.kTPCurveRight);
		list.Add (TrackPiece.PieceType.kTPCurveLeft);
		list.Add (TrackPiece.PieceType.kTPStraight);
		list.Add (TrackPiece.PieceType.kTPCurveRight);
		list.Add (TrackPiece.PieceType.kTPStraight);
		list.Add (TrackPiece.PieceType.kTPBalloonJunction);

		

		mSegmentTable.Add (SegmentType.TutorialBalloon, list);
		mDifficultyBySegmentType[(int)SegmentType.TutorialBalloon] = SegmentDifficulty.Tutorial;
		mEnvSetBySegmentType[(int)SegmentType.TutorialBalloon] = SegmentEnvSet.NONE;
		mEnvBySegmentType[(int)SegmentType.TutorialBalloon] = SegmentEnv.env0;
	}
	
	
	private static Dictionary<SegmentType, List<TrackPiece.PieceType>> mSegmentTable = null;
	private static List<SegmentDifficulty> mDifficultyBySegmentType = null;
	private static List<SegmentEnvSet> mEnvSetBySegmentType = null;
	private static List <SegmentEnv> mEnvBySegmentType = null;
	public static Dictionary<SegmentType, List<TrackPiece.PieceType>> SegmentTable
	{
		get{
			if(mSegmentTable == null)
			{
				PopulateSegmentDictionary();
			}
			return mSegmentTable;
		}
	}
	
	//-- STEP 3: Associate the new list to an ENUM for fast lookup later
	public static void PopulateSegmentDictionary()
	{
		mSegmentTable = new Dictionary<SegmentType, List<TrackPiece.PieceType>>();
		mDifficultyBySegmentType = new List<SegmentDifficulty>((int)SegmentType.Total);
		mEnvSetBySegmentType = new List<SegmentEnvSet>((int)SegmentType.Total);

		mEnvBySegmentType = new List<SegmentEnv>((int)SegmentType.Total);
			
		for(int i=0; i<(int)SegmentType.Total; i++)
		{
			mDifficultyBySegmentType.Add (SegmentDifficulty.Impossible);
			mEnvSetBySegmentType.Add (SegmentEnvSet.NONE);
			mEnvBySegmentType.Add (SegmentEnv.env0);
		}

		IntroTrack();
		WWIntroTrack();
		DFIntroTrack();
		YBRIntroTrack();
		ECIntroTrack();
		TutorialJump();
		TutorialFinley();
		TutorialTurn();
		TutorialSlide();
		TutorialTilt();
		TutorialEnemy();
		TutorialMeter();
		TutorialBalloon();
	}
	
	//-- STEP 4: In Code call this method with your ENUM and the last piece placed on the track.
	public static void QueueSegmentWithType(SegmentType segmentType, List<TrackPiece.PieceType> queueList)
	{
		// If there is no segment don't do anything
		if(TrackSegment.SegmentTable.ContainsKey(segmentType) == false)
			return;
		
		// If there are no pieces in the segment don't do anything
		List<TrackPiece.PieceType> list = TrackSegment.SegmentTable[segmentType];
		if(list == null)
			return;
		
		// Add all the pieces in the segment to the queue
		queueList.AddRange(list);
	}
	
	public static void QueueSegmentWithDifficulty(SegmentDifficulty difficulty, List<TrackPiece.PieceType> queueList)
	{
		List<int> segmentsWithDifficulty = new List<int>();
		for(int i=0; i<(int)SegmentType.Total; i++)
		{
			if(mDifficultyBySegmentType[i] == difficulty)
			{
				segmentsWithDifficulty.Add (i);
			}
		}
		
		// If there are none of this difficulty don't do anything
		if(segmentsWithDifficulty.Count == 0)
			return;
		
		// Randomly pick one of the segments
		SegmentType segmentType = (SegmentType)UnityEngine.Random.Range(0, segmentsWithDifficulty.Count);
		
		// Add it to the queue
		QueueSegmentWithType(segmentType, queueList);
	}	
	//Get a queue based on segment difficulty and ensure its the right environment and environment set
	public static void QueueSegmentWithEnvInfo(int envSet, int env, int difficulty, List<TrackPiece.PieceType> queueList)
	{
	//notify.DebugError("Set: " + envSet + ", Env: " + env);
		List<int> segmentsWithEnvSet = new List<int>();

		for(int i=0; i<(int)SegmentType.Total; i++)
		{
			if((int)mEnvSetBySegmentType [i] == envSet && (int)mEnvBySegmentType[i] == env  && (int)mDifficultyBySegmentType[i] <= difficulty)
			{
					segmentsWithEnvSet.Add (i);
					notify.Debug("segment type" + (SegmentType)i);
			}
		}
		
		foreach (int seg in segmentsWithEnvSet){
			notify.Debug ((SegmentType)(seg));
		}

		// If there are none of this difficulty don't do anything
		if(segmentsWithEnvSet.Count == 0)
			return;
		
		// Randomly pick one of the segments
		int RdmSeg = UnityEngine.Random.Range(0, segmentsWithEnvSet.Count);
		SegmentType selectedSegment = (SegmentType)(segmentsWithEnvSet[RdmSeg]);

		// Add it to the queue
		QueueSegmentWithType(selectedSegment, queueList);
	}	
}
