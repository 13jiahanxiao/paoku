using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackBuilder : MonoBehaviour {
	protected static Notify notify;
	static public TrackBuilder SharedInstance = null;
		
	public TrackSegment DebugTrackSegment = null;
	public List<TrackPiece.PieceType> PieceHistory = new List<TrackPiece.PieceType>();
	public List<TrackPiece.PieceType> QueuedPiecesToAdd = new List<TrackPiece.PieceType>();
	
	private Dictionary<TrackPiece.PieceType,TrackPieceTypeDefinition> PieceTypes = null;
	private List<TrackPieceTypeDefinition> pieceTypesLookupArray = null;
	private TrackPieceTypeDefinition[] AvailablePieceTypes = null;
	
	
	public int ActiveJunctions = 0;
	public int MaxActiveJunctions = 0;
	public int MaxJunctions = 1;
	
	public float MinDistanceBetweenTurns = 30.0f;
	public float MaxDistanceBetweenTurns = 60.0f;
	public float MinDistanceBetweenObstacles = 20.0f;
	public float MaxDistanceBetweenObstacles = 40.0f;
	public float MinDistanceAfterTurnForObstacle = 5.0f;
	public float MinDistanceBetweenBalloons = 900f;
	
	/// <summary>
	/// After this many meters has passed, we try to put the the envset junction piece, 
	/// it will be close but will always be over this number
	/// </summary>
	public float MinDistanceBetweenEnvSetJunctions = 900.0f;
	
	public float DistanceToTurnSection = 1000.0f;
	public float DistanceToTurnSectionEnd = 100.0f;
	
	private float TrackSegmentChance = 0.00f;
	public float MinDistanceBetweenTrackPieces = 250.0f;
	
	public bool AllowTurns = false;
	//private int LastTurnType; // -1 is Right, 1 is Left, 0 is no turns have happened yet
	public bool AllowObstacles = false;
	public int MaxTrackPieceDifficulty = 0;

	//public float MinDistanceBetweenDefaultEnvironmentChange = 350.0f;

	public bool IsFastTurnSection = false;
	public bool AllowTurnAfterObstacle = false;
	public int MaxBackToBackObstacles = 2;
	public float DoubleObstaclePercent = 0.0f;
	
	public float MaxZiplineForce = 2000.0f;
	public float ZiplinePerSegmentForce = 100.0f;
	
	// which environment set are we currently playing on
	public int CurrentEnvironmentSetId
	{
		get 
		{
			if (EnvironmentSetManager.SharedInstance)
			{
				return EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.SetId;
			}
			else
			{
				notify.Debug( "[TrackBuilder] CurrentEnvironmentSetId.get GameProfile.SharedInstance.Player.savedEnvSet" );
				
				int savedEnvSet = GameProfile.SharedInstance.Player.savedEnvSet;
				return Settings.GetInt("starting-envset", savedEnvSet);
			}
			
		}
	}
	
	public const int TunnelTransitionEnvironmentSetId = 10;
	
	void Awake()
	{
		SharedInstance = this;
		notify = new Notify(this.GetType().Name);
		PopulatePieceTypes();
	}
	
	private void PopulatePieceTypes()
	{
		// Set up the PieceTypes dictionary.  
		if(PieceTypes != null)
		{
			PieceTypes = null;
		}
		
		PieceTypes = new  Dictionary<TrackPiece.PieceType, TrackPieceTypeDefinition>();	

		if (Settings.GetBool("allow-environment-set-transition", true))
		{
			PopulateTunnelTransitionPieceTypes();	
		}
		
		if (Settings.GetBool("balloon-enabled", true))
		{
			PopulateBalloonPieceTypes();	
		}
				
		PopulateEnvironmentSetPieces(CurrentEnvironmentSetId);
		
		TrackSegment.PopulateSegmentDictionary();
	}
	
	public void PopulateEnvironmentSetPieces( int envset)
	{	
	//	notify.Debug (envset);
		switch (envset)
		{
		case 0:
			PopulateMachuPieceTypes();
			break;
		case 1:
			PopulateWWPieceTypes();
			break;
		case 2:
			PopulateDFPieceTypes();
			break;	
		case 3:
			PopulateYBRPieceTypes();
			break;	
		case 4:
			PopulateECPieceTypes();
			break;		
		}

		// Create scratch pad list of available piece types.  Max Size is the total number of TrackPieceTypes
		if (AvailablePieceTypes != null)
		{
			AvailablePieceTypes = null;
		}
		
		AvailablePieceTypes = new TrackPieceTypeDefinition[PieceTypes.Count];		
		
		System.Array ptvalues = System.Enum.GetValues(typeof(TrackPiece.PieceType));
		pieceTypesLookupArray = new List<TrackPieceTypeDefinition>(ptvalues.Length);
		for(int i=0; i<ptvalues.Length; i++) {
			pieceTypesLookupArray.Add (null);
		}
		//-- turn on/off the bitaray so we don't have to call .Contains on the PieceTypes array.
		foreach (var item in PieceTypes.Keys) {
			// TODO rebuild this when we switch environment set
			pieceTypesLookupArray[(int)item] = PieceTypes[item];	
		}		
		
	}
	
	
	/// <summary>
	/// Fixup internal fields in preparation for transition tunnel exit
	/// </summary>
	public void FixupForTransitionExit()
	{
		if (AvailablePieceTypes != null)
		{
			AvailablePieceTypes = null;
		}
		
		AvailablePieceTypes = new TrackPieceTypeDefinition[PieceTypes.Count];			
		
	}
	
	private void PopulateBalloonPieceTypes()
	{
		TrackPieceTypeDefinition trackType = new TrackPieceTypeDefinition();
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPBalloonEntrance;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionStart;
		trackType.EnvironmentSet = CurrentEnvironmentSetId;
		trackType.Environment = 9;
		trackType.DifficultyLevel = 9;
		trackType.IsBalloon = true;
		trackType.SelectionOdds = 0f;  //don't appear unless called specifically
		trackType.AddVariation ("oz_bc_entrance_a");
		trackType.AllowInRegularRotation = false;
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPBalloonExit;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionEnd;
		trackType.EnvironmentSet = CurrentEnvironmentSetId;
		trackType.Environment = 9;
		trackType.DifficultyLevel = 0;
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPBalloonFall);
		trackType.IsBalloon = true;
		trackType.AddVariation ("oz_bc_exit_a");
//		trackType.PrePieces.Add(TrackPiece.PieceType.kTPBalloonStraightShort);
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPBalloonStraightShort);
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPBalloonStraightShort);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPBalloonStraight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.AlternateAfterJunctionPiece = TrackPiece.PieceType.kTPBalloonStraightShort;
		trackType.AfterJunctionPiece = TrackPiece.PieceType.kTPBalloonFall;
		trackType.IsCloudsJunction = true;
		trackType.EnvironmentSet = CurrentEnvironmentSetId;
		trackType.Environment = 9;
		trackType.DifficultyLevel = 0;
		trackType.IsBalloon = true;
		trackType.AddVariation ("oz_bc_balloon_straight_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPBalloonStraightShort;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.IsCloudsJunction = false;
		trackType.EnvironmentSet = CurrentEnvironmentSetId;
		trackType.Environment = 9;
		trackType.DifficultyLevel = 0;
		trackType.IsBalloon = true;
		trackType.AddVariation ("oz_bc_balloon_straight_short_a");
		PieceTypes.Add(trackType.TrackType,trackType);
	}
	
	
	private void PopulateTunnelTransitionPieceTypes()
	{
		TrackPieceTypeDefinition trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPTransitionTunnelEntrance;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentSetTransitionStart;
		trackType.EnvironmentSet = CurrentEnvironmentSetId;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.IsTransitionTunnel = true;
		trackType.AddVariation ("oz_tt_entrance_a");
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPBeforeTunnelTransitionEntrance);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPTransitionTunnelMiddle);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPTransitionTunnelMiddle);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPTransitionTunnelMiddle);
		PieceTypes.Add(trackType.TrackType,trackType);	
		
		// EnvironmentSet is deliberately set to -1, once we place it, we fix it to match the new environment set,
		// and then once he steps off it, we bring it back to -1
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPTransitionTunnelExit;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentSetTransitionEnd;
		trackType.EnvironmentSet = -1;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.IsTransitionTunnel = true;
		trackType.AllowInRegularRotation = false;
		trackType.AddVariation ("oz_tt_exit_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPAfterTunnelTransitionExit);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlatIntro);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlatIntro);
		//trackType.DifficultyDistanceReduction = 2000;
		PieceTypes.Add(trackType.TrackType,trackType);
		
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPTransitionTunnelMiddle;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = TunnelTransitionEnvironmentSetId;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.IsTransitionTunnel = true;
		//trackType.AddVariation ("oz_tt_slightright_up_a");
		trackType.AddVariation ("oz_tt_slightleft_down_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPTransitionTunnelMiddle2;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = TunnelTransitionEnvironmentSetId;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.IsTransitionTunnel = true;
		trackType.AddVariation ("oz_tt_slightright_up_a");
		//trackType.AddVariation ("oz_tt_slightleft_down_a");
		PieceTypes.Add(trackType.TrackType,trackType);
	}
	
	// the bare minimum to get the game actually playing,
	private void PopulateSimplifiedWWPieceTypes()
	{	
		TrackPieceTypeDefinition trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPStraightFlat;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		//trackType.IsTurnLeft = true;
		trackType.AddVariation ("oz_ww_cliffs_straight_a");
		//trackType.AddVariation ("oz_ww_cliffs_shortstraight_a");
		PieceTypes.Add(trackType.TrackType,trackType);
	}
	
	////////////////////////////
	//WHIMSY WOODS ENVIRONMENT//
	////////////////////////////
	private void PopulateWWPieceTypes()
	{
		TrackPieceTypeDefinition trackType = new TrackPieceTypeDefinition();
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPStraightFlatIntro;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 0;
		trackType.AllowInRegularRotation = false;
		trackType.AddVariation ("oz_ww_woods_straight_a");
		PieceTypes.Add(trackType.TrackType,trackType);		
		
//		//Fast Travel Piece
//		trackType = new TrackPieceTypeDefinition();
//		trackType.TrackType = TrackPiece.PieceType.kTPFastTravel;
//		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
//		trackType.EnvironmentSet = 1;
//		trackType.Environment = 1;
//		trackType.DifficultyLevel = 0;
//		trackType.SelectionOdds = 0f; //Must be specifically called
//		trackType.AddVariation ("oz_ww_woods_straight_a");
//		trackType.AddVariation ("oz_ww_woods_shortstraight_a");
//		trackType.AddVariation ("oz_ww_woods_slightright_a");
//		trackType.AddVariation ("oz_ww_woods_slightleft_a");
//		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestSlights);
//		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
//		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestSlights);
//		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
//		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestSlights);
//		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestExit); //Now into the cliffs
//		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
//		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
//		trackType.PostPieces.Add(TrackPiece.PieceType.kTPCurveRight);
//		trackType.PostPieces.Add(TrackPiece.PieceType.kTPCurveLeft);
//		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
//		PieceTypes.Add(trackType.TrackType,trackType);
		
		/////////////////
		//CLIFFS TILES //
		/////////////////
		
		//SLIDE MECHANIC TEST
		//Cliffs Down
		/*
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPSlideEnter;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceStart;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.CompoundPieceMiddleType = TrackPiece.PieceType.kTPSlideMid;
		trackType.CompoundPieceEndType = TrackPiece.PieceType.kTPSlideExit;
		trackType.CompoundPieceMinMiddleCount = 12;
		trackType.CompoundPieceMaxMiddleCount = 17;
		trackType.SelectionOdds = 0f; //Use Debug to see the segment - Needs to speed you up when you enter still
		trackType.DifficultyLevel = 1;
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.AddVariation ("oz_ww_cliffs_slide_start_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPSlideMid;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceMiddle;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 1;
		trackType.AddVariation ("oz_ww_cliffs_slide_mid_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPSlideExit;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceEnd;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 1;
		trackType.AddVariation ("oz_ww_cliffs_slide_exit_a");
		PieceTypes.Add(trackType.TrackType,trackType);		
		*/
		
		//Custom Segments
		
		//Fast Turns
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFastTurnLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = .5f;
		trackType.IsTurnLeft = true;
		trackType.AddVariation ("oz_ww_cliffs_hardleft_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFastTurnLeftAlt;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = .5f;
		trackType.IsTurnLeft = true;
		trackType.AddVariation ("oz_ww_cliffs_hardleft_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFastTurnRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = .5f;
		trackType.IsTurnRight = true;
		trackType.AddVariation ("oz_ww_cliffs_hardright_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFastTurnRightAlt;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = .5f;
		trackType.IsTurnRight = true;
		trackType.AddVariation ("oz_ww_cliffs_hardright_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
				
		//Collapsing Cliffs Run
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAnimatedGauntlet1;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 3;
		trackType.SelectionOdds = .5f;
		trackType.IsLedgeLeft = true;
		trackType.IsSlightLeft =true;
		trackType.AddVariation ("oz_ww_cliffs_slightleft_leftcollapse_anim_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPAnimatedRightLedge);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPAnimatedLeftLedge);
		PieceTypes.Add(trackType.TrackType,trackType);		
		
		//Collapsing Cliffs Run
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAnimatedGauntlet2;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 3;
		trackType.SelectionOdds = .5f;
		trackType.IsLedgeRight = true;
		trackType.IsSlightRight =true;
		trackType.AddVariation ("oz_ww_cliffs_slightright_rightcollapse_anim_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPAnimatedLeftLedge);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPAnimatedRightLedge);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Gap into collapsing cliff
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAnimatedGauntlet3;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = .5f;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("oz_ww_cliffs_shortstraight_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPAnimatedRightLedge);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Gap into collapsing cliff
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAnimatedGauntlet4;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = .5f;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("oz_ww_cliffs_shortstraight_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPAnimatedLeftLedge);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Monkey into gap
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAnimatedGauntlet5;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 5;
		trackType.SelectionOdds = 2f;
		trackType.IsEnemy = true;
		trackType.IsAttackingBaboon = true;
		trackType.AddVariation ("oz_ww_cliffs_straight_monkey");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		
		//End Custom Segments
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPCurveLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 1f;
		trackType.IsSlightLeft =true;
		trackType.AddVariation ("oz_ww_cliffs_slightleft_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPCurveRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 1f;
		trackType.IsSlightRight =true;
		trackType.AddVariation ("oz_ww_cliffs_slightright_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPStraightFlat;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 1f;
		//trackType.AddVariation ("oz_ww_cliffs_slightright_a");
		trackType.AddVariation ("oz_ww_cliffs_straight_b");
		trackType.AddVariation ("oz_ww_cliffs_straight_a");	
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPStraight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 1f;
		//trackType.AddVariation ("oz_ww_cliffs_straight_a");
		//trackType.AddVariation ("oz_ww_cliffs_straight_b");
		trackType.AddVariation ("oz_ww_cliffs_shortstraight_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		/*
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPWWcliffsCurveRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("oz_ww_cliffs_slightright_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		*/
		
		//For Spencer to make new functions (IsLedgeLeft and IsLedgeRight as obstacles)
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAnimatedRightLedge;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 2;
		trackType.IsAnimated = true;
		trackType.IsLedgeRight = true;
		trackType.SelectionOdds = 0.5f;
		trackType.AddVariation ("oz_ww_cliffs_straight_rightcollapse_anim_a");
		trackType.AddVariation ("oz_ww_cliffs_slightleft_rightcollapse_anim_a");
		trackType.AddVariation ("oz_ww_cliffs_slightright_rightcollapse_anim_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//For Spencer to make new functions (IsLedgeLeft and IsLedgeRight as obstacles)
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAnimatedLeftLedge;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 2;
		trackType.IsAnimated = true;
		trackType.IsLedgeLeft = true;
		trackType.SelectionOdds = 0.5f;
		trackType.AddVariation ("oz_ww_cliffs_straight_leftcollapse_anim_a");
		trackType.AddVariation ("oz_ww_cliffs_slightleft_leftcollapse_anim_a");
		trackType.AddVariation ("oz_ww_cliffs_slightright_leftcollapse_anim_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPJumpOver;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		//trackType.IsObstacle = true;
		trackType.IsJumpOver = true;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_ww_cliffs_straight_over_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPJumpOverSlightLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		//trackType.IsObstacle = true;
		trackType.IsJumpOver = true;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 1f;
		trackType.IsSlightLeft =true;
		trackType.AddVariation ("oz_ww_cliffs_slightleft_over_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPJumpOverSlightRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		//trackType.IsObstacle = true;
		trackType.IsJumpOver = true;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 1f;
		trackType.IsSlightRight =true;
		trackType.AddVariation ("oz_ww_cliffs_slightright_over_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPTurnLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.IsTurnLeft = true;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("oz_ww_cliffs_hardleft_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPTJunction;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.IsTurnLeft = true;
		trackType.IsTurnRight = true;
		trackType.IsJunction = true;
		trackType.DifficultyLevel = 1;
		trackType.SelectionOdds = 1f;
		trackType.AfterJunctionPiece = TrackPiece.PieceType.kTPStraight;
		trackType.AddVariation ("oz_ww_cliffs_junctionLR_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPEnvSetJunction;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.IsTurnLeft = true;
		trackType.IsTurnRight = true;
		trackType.IsJunction = true;
		trackType.DifficultyLevel = 1;
		trackType.AfterJunctionPiece = TrackPiece.PieceType.kTPStraight;
		trackType.AllowWhenFastTravelling = false;
		trackType.AddVariation ("oz_ww_cliffs_envset_junctionLR_a");
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPStraight);
		//trackType.DifficultyDistanceReduction = 1000;
		PieceTypes.Add(trackType.TrackType,trackType);	
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPTurnRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.IsTurnRight = true;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("oz_ww_cliffs_hardright_a");
		PieceTypes.Add(trackType.TrackType,trackType);	
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPSlideUnder;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		//trackType.IsObstacle = true;
		trackType.IsSlideUnder = true;
		trackType.DifficultyLevel = 2;
		trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_ww_cliffs_straight_under_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPSlideUnderSlightLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		//trackType.IsObstacle = true;
		trackType.IsSlideUnder = true;
		trackType.DifficultyLevel = 2;
		trackType.SelectionOdds = 1f;
		trackType.IsSlightLeft =true;
		trackType.AddVariation ("oz_ww_cliffs_slightleft_under_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPSlideUnderSlightRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		//trackType.IsObstacle = true;
		trackType.IsSlideUnder = true;
		trackType.DifficultyLevel = 2;
		trackType.SelectionOdds = 1f;
		trackType.IsSlightRight =true;
		trackType.AddVariation ("oz_ww_cliffs_slightright_under_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPJumpOrSlide;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		//trackType.IsObstacle = true;
		trackType.IsSlideUnder = true;
		trackType.IsJumpOver = true;
		trackType.DifficultyLevel = 3;
		trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_ww_cliffs_straight_overunder_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGaps;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		//trackType.IsObstacle = true;
		trackType.IsGap = true;
		trackType.IsJumpOver = true;
		trackType.DifficultyLevel = 2;
		trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_ww_cliffs_straight_biggap_a");
		trackType.AddVariation ("oz_ww_cliffs_straight_smallgap_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		/*
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAnimatedJumpOrSlide;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		//trackType.IsObstacle = true;
		trackType.IsSlideUnder = true;
		trackType.IsJumpOver = true;
		trackType.IsAnimated = true;
		trackType.DifficultyLevel = 2;
		trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_ww_cliffs_straight_overunder_anim_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		*/
	
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPEnemyMonkey;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		//trackType.IsObstacle = true;
		trackType.IsEnemy = true;
		trackType.IsAnimated = true;
		trackType.IsAttackingBaboon = true;
		trackType.DifficultyLevel = 3;
		trackType.SelectionOdds = 2f;
		trackType.AddVariation ("oz_ww_cliffs_straight_monkey");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPEnemySnapDragon;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.IsStumble = true;
		trackType.IsEnemy = true;
		trackType.IsJumpOver = true;
		trackType.IsAnimated = true;
		trackType.DifficultyLevel = 1;
		trackType.SelectionOdds = 1.5f; 
		trackType.AddVariation ("oz_ww_cliffs_straight_snapdragonspawn");
		PieceTypes.Add(trackType.TrackType,trackType);

		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPLedgeLeftStart;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceStart;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.CompoundPieceMiddleType = TrackPiece.PieceType.kTPLedgeLeftMiddle;
		trackType.CompoundPieceEndType = TrackPiece.PieceType.kTPLedgeLeftEnd;
		trackType.CompoundPieceMinMiddleCount = 3;
		trackType.CompoundPieceMaxMiddleCount = 6;
		//trackType.IsObstacle = true;
		trackType.IsLedgeLeft = true;
		trackType.DifficultyLevel = 2;
		trackType.AddVariation ("oz_ww_cliffs_straight_leftstart_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPLedgeLeftMiddle;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceMiddle;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		//trackType.IsObstacle = true;
		trackType.IsLedgeLeft = true;
		trackType.DifficultyLevel = 2;
		trackType.AddVariation ("oz_ww_cliffs_straight_leftmiddle_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPLedgeLeftEnd;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceEnd;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		//trackType.IsObstacle = true;
		trackType.IsLedgeLeft = true;
		trackType.DifficultyLevel = 2;
		trackType.AddVariation ("oz_ww_cliffs_straight_leftend_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPLedgeRightStart;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceStart;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.CompoundPieceMiddleType = TrackPiece.PieceType.kTPLedgeRightMiddle;
		trackType.CompoundPieceEndType = TrackPiece.PieceType.kTPLedgeRightEnd;
		trackType.CompoundPieceMinMiddleCount = 3;
		trackType.CompoundPieceMaxMiddleCount = 6;
		//trackType.IsObstacle = true;
		trackType.IsLedgeRight = true;
		trackType.DifficultyLevel = 2;
		trackType.AddVariation ("oz_ww_cliffs_straight_rightstart_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPLedgeRightMiddle;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceMiddle;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		//trackType.IsObstacle = true;
		trackType.IsLedgeRight = true;
		trackType.DifficultyLevel = 2;
		trackType.AddVariation ("oz_ww_cliffs_straight_rightmiddle_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPLedgeRightEnd;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceEnd;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		//trackType.IsObstacle = true;
		trackType.IsLedgeRight = true;
		trackType.DifficultyLevel = 2;
		trackType.AddVariation ("oz_ww_cliffs_straight_rightend_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Cliffs Down
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPUndulateDownEnter;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceStart;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.CompoundPieceMiddleType = TrackPiece.PieceType.kTPUndulateDownMid;
		trackType.CompoundPieceEndType = TrackPiece.PieceType.kTPUndulateDownExit;
		trackType.CompoundPieceMinMiddleCount = 1;
		trackType.CompoundPieceMaxMiddleCount = 2;
		trackType.SelectionOdds = 0.7f;
		trackType.DifficultyLevel = 1;
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.AddVariation ("oz_ww_cliffs_arch_flatdown_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPUndulateDownMid;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceMiddle;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 1;
		trackType.AddVariation ("oz_ww_cliffs_arch_down_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPUndulateDownExit;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceEnd;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 1;
		trackType.AddVariation ("oz_ww_cliffs_arch_downflat_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Cliffs Up
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPUndulateUpEnter;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceStart;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		//The UP portion is marked as stairs so pickups do not spawn on them
		trackType.IsStairs = true;
		trackType.CompoundPieceMiddleType = TrackPiece.PieceType.kTPUndulateUpMid;
		trackType.CompoundPieceEndType = TrackPiece.PieceType.kTPUndulateUpExit;
		trackType.CompoundPieceMinMiddleCount = 1;
		trackType.CompoundPieceMaxMiddleCount = 2;
		trackType.SelectionOdds = 0.7f;
		trackType.DifficultyLevel = 1;
		trackType.AddVariation ("oz_ww_cliffs_arch_flatup_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPUndulateUpMid;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceMiddle;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 1;
		//The UP portion is marked as stairs so pickups do not spawn on them
		trackType.IsStairs = true;
		trackType.AddVariation ("oz_ww_cliffs_arch_up_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPUndulateUpExit;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceEnd;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 1;
		//The UP portion is marked as stairs so pickups do not spawn on them
		trackType.IsStairs = true;
		trackType.AddVariation ("oz_ww_cliffs_arch_upflat_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		////////////////
		//WOODS TILES //
		////////////////
		
		//Custom Pieces
		
		//Over Over Under
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPOverOverUnder;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = 1f;
		trackType.IsJumpOver = true;
		trackType.AddVariation ("oz_ww_woods_straight_over_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestSlideUnder);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Under Under Over
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPUnderUnderOver;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("oz_ww_woods_straight_under_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestSlideUnder);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestGaps);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Under Over Animated
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPUnderOverAnimated;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 5;
		trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("oz_ww_woods_straight_under_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestJumpOver);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestAnimatedJumpOrSlide);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Over Under Animated
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPOverUnderAnimated;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = 1f;
		trackType.IsGap = true;
		trackType.IsJumpOver = true;
		trackType.AddVariation ("oz_ww_woods_straight_smallgap_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestSlideUnder);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestAnimatedJumpOrSlide);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
		PieceTypes.Add(trackType.TrackType,trackType);
				
		//Gap, Gap
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestGapGap;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 5;
		trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("oz_ww_woods_shortstraight_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestGaps);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Gap, Gap.. GAP
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestGapGapGap;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 6;
		trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("oz_ww_woods_shortstraight_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestSmallGap);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestSmallGap);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestSmallGap);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestGaps);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestGaps);
		PieceTypes.Add(trackType.TrackType,trackType);

		//Gap, Slide, Gap
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestGapSlideGap;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 6;
		trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("oz_ww_woods_shortstraight_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestAnimatedJumpOrSlide);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestGaps);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Gap, Falling tree
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGapAnimated;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 5;
		trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("oz_ww_woods_shortstraight_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestAnimatedJumpOrSlide);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestGaps);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//End Custom Segments
		
		
			//notify.Debug ("Non-alpha");
			
			trackType = new TrackPieceTypeDefinition();
			trackType.TrackType = TrackPiece.PieceType.kTPForestEntrance;
			trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionStart;
			trackType.EnvironmentSet = 1;
			trackType.Environment = 1;
			trackType.DifficultyLevel = 0;
			trackType.PrePieces.Add (TrackPiece.PieceType.kTPStraight);	
			trackType.PrePieces.Add (TrackPiece.PieceType.kTPStraightFlat);
			trackType.PrePieces.Add (TrackPiece.PieceType.kTPStraight);
			trackType.AddVariation ("oz_ww_woods_transition_entrance_a");
			PieceTypes.Add(trackType.TrackType,trackType);
			
			trackType = new TrackPieceTypeDefinition();
			trackType.TrackType = TrackPiece.PieceType.kTPForestExit;
			trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionEnd;
			trackType.EnvironmentSet = 1;
			trackType.Environment = 1;
			trackType.DifficultyLevel = 0;
			trackType.AddVariation ("oz_ww_woods_transition_exit_a");
			trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
			trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
			trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
			PieceTypes.Add(trackType.TrackType,trackType);
	
			trackType = new TrackPieceTypeDefinition();
			trackType.TrackType = TrackPiece.PieceType.kTPForestJunction;
			trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
			trackType.EnvironmentSet = 1;
			trackType.Environment = 1;
			trackType.IsTurnRight = true;
			trackType.IsTurnLeft = true;
			trackType.IsJunction = true;
			trackType.DifficultyLevel = 0;
			trackType.AfterJunctionPiece = TrackPiece.PieceType.kTPForestStraight;
			trackType.AddVariation ("oz_ww_woods_junctionLR_a");
			PieceTypes.Add(trackType.TrackType,trackType);
			
			trackType = new TrackPieceTypeDefinition();
			trackType.TrackType = TrackPiece.PieceType.kTPForestTurnRight;
			trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
			trackType.EnvironmentSet = 1;
			trackType.Environment = 1;
			trackType.IsTurnRight = true;
			trackType.DifficultyLevel = 0;
			trackType.AddVariation ("oz_ww_woods_hardright_a");
			PieceTypes.Add(trackType.TrackType,trackType);
			
			trackType = new TrackPieceTypeDefinition();
			trackType.TrackType = TrackPiece.PieceType.kTPForestTurnLeft;
			trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
			trackType.EnvironmentSet = 1;
			trackType.Environment = 1;
			trackType.IsTurnLeft = true;
			trackType.DifficultyLevel = 0;
			trackType.AddVariation ("oz_ww_woods_hardleft_a");
			PieceTypes.Add(trackType.TrackType,trackType);
			
			trackType = new TrackPieceTypeDefinition();
			trackType.TrackType = TrackPiece.PieceType.kTPForestSlideUnder;
			trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
			trackType.EnvironmentSet = 1;
			trackType.Environment = 1;
			//trackType.IsObstacle = true;
			trackType.IsSlideUnder = true;
			trackType.DifficultyLevel = 1;
			trackType.SelectionOdds = 1f;
			trackType.AddVariation ("oz_ww_woods_straight_under_a");
			PieceTypes.Add(trackType.TrackType,trackType);
			
			trackType = new TrackPieceTypeDefinition();
			trackType.TrackType = TrackPiece.PieceType.kTPForestSlideUnderSlightLeft;
			trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
			trackType.EnvironmentSet = 1;
			trackType.Environment = 1;
			//trackType.IsObstacle = true;
			trackType.IsSlideUnder = true;
			trackType.DifficultyLevel = 2;
			trackType.SelectionOdds = 1f;
			trackType.IsSlightLeft =true;
			trackType.AddVariation ("oz_ww_woods_slightleft_under_a");
			PieceTypes.Add(trackType.TrackType,trackType);
		
			trackType = new TrackPieceTypeDefinition();
			trackType.TrackType = TrackPiece.PieceType.kTPForestSlideUnderSlightRight;
			trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
			trackType.EnvironmentSet = 1;
			trackType.Environment = 1;
			//trackType.IsObstacle = true;
			trackType.IsSlideUnder = true;
			trackType.DifficultyLevel = 2;
			trackType.SelectionOdds = 1f;
			trackType.IsSlightRight =true;
			trackType.AddVariation ("oz_ww_woods_slightright_under_a");
			PieceTypes.Add(trackType.TrackType,trackType);
		
			trackType = new TrackPieceTypeDefinition();
			trackType.TrackType = TrackPiece.PieceType.kTPForestJumpOver;
			trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
			trackType.EnvironmentSet = 1;
			trackType.Environment = 1;
			//trackType.IsObstacle = true;
			trackType.IsJumpOver = true;
			trackType.DifficultyLevel = 1;
			trackType.SelectionOdds = 1f;
			trackType.AddVariation ("oz_ww_woods_straight_over_a");
			PieceTypes.Add(trackType.TrackType,trackType);
		
			//Phil Note - Do these still feel weird? - Yes
//			trackType = new TrackPieceTypeDefinition();
//			trackType.TrackType = TrackPiece.PieceType.kTPForestJumpOverSlights;
//			trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
//			trackType.EnvironmentSet = 1;
//			trackType.Environment = 1;
//			//trackType.IsObstacle = true;
//			trackType.IsJumpOver = true;
//			trackType.DifficultyLevel = 1;
//			trackType.SelectionOdds = 1f;
//			trackType.AddVariation ("oz_ww_woods_slightleft_over_a");
//			trackType.AddVariation ("oz_ww_woods_slightright_over_a");
//			PieceTypes.Add(trackType.TrackType,trackType);
			
			trackType = new TrackPieceTypeDefinition();
			trackType.TrackType = TrackPiece.PieceType.kTPForestGaps;
			trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
			trackType.EnvironmentSet = 1;
			trackType.Environment = 1;
			//trackType.IsObstacle = true;
			trackType.IsGap = true;
			trackType.IsJumpOver = true;
			trackType.DifficultyLevel = 2;
			trackType.SelectionOdds = 1f;
			trackType.AddVariation ("oz_ww_woods_straight_smallgap_a");
			trackType.AddVariation ("oz_ww_woods_straight_largegap_a");
			PieceTypes.Add(trackType.TrackType,trackType);
				
			trackType = new TrackPieceTypeDefinition();
			trackType.TrackType = TrackPiece.PieceType.kTPForestAnimatedJumpOrSlide;
			trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
			trackType.EnvironmentSet = 1;
			trackType.Environment = 1;
			//trackType.IsObstacle = true;
			trackType.IsJumpOver = true;
			trackType.IsSlideUnder = true;
			trackType.IsAnimated = true;
			trackType.DifficultyLevel = 3;
			trackType.SelectionOdds = .75f;
			trackType.AddVariation ("oz_ww_woods_straight_overunder_anim_a");
			trackType.AddVariation ("oz_ww_woods_straight_overunder_anim_left_a");
			PieceTypes.Add(trackType.TrackType,trackType);
		
			trackType = new TrackPieceTypeDefinition();
			trackType.TrackType = TrackPiece.PieceType.kTPForestStraight;
			trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
			trackType.EnvironmentSet = 1;
			trackType.Environment = 1;
			trackType.DifficultyLevel = 0;
			trackType.SelectionOdds = 1f;
			trackType.AddVariation ("oz_ww_woods_straight_a");
			PieceTypes.Add(trackType.TrackType,trackType);
		
			trackType = new TrackPieceTypeDefinition();
			trackType.TrackType = TrackPiece.PieceType.kTPForestSlightLeft;
			trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
			trackType.EnvironmentSet = 1;
			trackType.Environment = 1;
			trackType.DifficultyLevel = 0;
			trackType.SelectionOdds = 1f;
			trackType.IsSlightLeft =true;
			trackType.AddVariation ("oz_ww_woods_slightleft_a");
			PieceTypes.Add(trackType.TrackType,trackType);
		
			trackType = new TrackPieceTypeDefinition();
			trackType.TrackType = TrackPiece.PieceType.kTPForestSlightRight;
			trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
			trackType.EnvironmentSet = 1;
			trackType.Environment = 1;
			trackType.DifficultyLevel = 0;
			trackType.SelectionOdds = 1f;
			trackType.IsSlightRight =true;
			trackType.AddVariation ("oz_ww_woods_slightright_a");
			PieceTypes.Add(trackType.TrackType,trackType);
			
			//Forest Short Track
			trackType = new TrackPieceTypeDefinition();
			trackType.TrackType = TrackPiece.PieceType.kTPForestStraightShort;
			trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
			trackType.EnvironmentSet = 1;
			trackType.Environment = 1;
			trackType.DifficultyLevel = 0;
			trackType.SelectionOdds = 1f;
			trackType.AddVariation ("oz_ww_woods_shortstraight_a");
			PieceTypes.Add(trackType.TrackType,trackType);
		
			trackType = new TrackPieceTypeDefinition();
			trackType.TrackType = TrackPiece.PieceType.kTPForestEnemySnapDragon;
			trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
			trackType.EnvironmentSet = 1;
			trackType.Environment = 1;
			trackType.DifficultyLevel = 0;
			trackType.SelectionOdds = 1.5f;
			trackType.IsStumble = true;
			trackType.IsEnemy = true;
			trackType.IsJumpOver = true;
			trackType.IsAnimated = true;
			trackType.AddVariation ("oz_ww_woods_shortstraight_b");
			PieceTypes.Add(trackType.TrackType,trackType);
			
			//Forest Down
			trackType = new TrackPieceTypeDefinition();
			trackType.TrackType = TrackPiece.PieceType.kTPForestUndulateDownEnter;
			trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
			trackType.EnvironmentSet = 1;
			trackType.Environment = 1;
			trackType.DifficultyLevel = 0;
			trackType.SelectionOdds = .1f;
			trackType.AddVariation ("oz_ww_woods_arch_downhill_flat_a");
			PieceTypes.Add(trackType.TrackType,trackType);
					
			//Forest Up
			trackType = new TrackPieceTypeDefinition();
			trackType.TrackType = TrackPiece.PieceType.kTPForestUndulateUpEnter;
			trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
			trackType.EnvironmentSet = 1;
			trackType.Environment = 1;
			trackType.DifficultyLevel = 0;
			trackType.SelectionOdds = .1f;
			//The UP portion is marked as stairs so pickups do not spawn on them
			trackType.IsStairs = true;
			trackType.AddVariation ("oz_ww_woods_arch_uphill_flat_a");
			PieceTypes.Add(trackType.TrackType,trackType);
		
		/////
		//Tutorial Specific Pieces
		////
		
			trackType = new TrackPieceTypeDefinition();
			trackType.TrackType = TrackPiece.PieceType.kTPForestSmallGap;
			trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
			trackType.EnvironmentSet = 1;
			trackType.Environment = 1;
			//trackType.IsObstacle = true;
			trackType.IsGap = true;
			trackType.IsJumpOver = true;
			trackType.DifficultyLevel = 1;
			trackType.SelectionOdds = 1f;
			trackType.AddVariation ("oz_ww_woods_straight_smallgap_a");
			//trackType.AddVariation ("oz_ww_woods_straight_largegap_a");
			PieceTypes.Add(trackType.TrackType,trackType);
		
			trackType = new TrackPieceTypeDefinition();
			trackType.TrackType = TrackPiece.PieceType.kTPForestLargeGap;
			trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
			trackType.EnvironmentSet = 1;
			trackType.Environment = 1;
			//trackType.IsObstacle = true;
			trackType.IsGap = true;
			trackType.IsJumpOver = true;
			trackType.DifficultyLevel = 2;
			trackType.SelectionOdds = 1f;
			//trackType.AddVariation ("oz_ww_woods_straight_smallgap_a");
			trackType.AddVariation ("oz_ww_woods_straight_largegap_a");
			PieceTypes.Add(trackType.TrackType,trackType);
		
		//////
		//End Tutorial Pieces
		/////
		
		
		//////////////////////
		//  Narrrows Tiles  //
		//////////////////////
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPNarrowsEntrance;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionStart;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 1;
		trackType.IsNarrow = true;
		trackType.PrePieces.Add (TrackPiece.PieceType.kTPStraight);	
		trackType.PrePieces.Add (TrackPiece.PieceType.kTPStraightFlat);
		trackType.AddVariation ("oz_ww_cliffs_straightnarrow_start_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPNarrowsExit;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionEnd;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 0;
		trackType.IsNarrow = true;
		trackType.AddVariation ("oz_ww_cliffs_straightnarrow_end_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPNarrowsJunction;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 2;
		trackType.IsTurnRight = true;
		trackType.IsTurnLeft = true;
		trackType.IsJunction = true;
		trackType.DifficultyLevel = 0;
		trackType.AfterJunctionPiece = TrackPiece.PieceType.kTPNarrowsStraight;
		trackType.IsNarrow = true;
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.AddVariation ("oz_ww_cliffs_straightnarrow_junctionLR_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Custom Fast Turn Sections
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPNarrowFastTurnLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 3;
		trackType.SelectionOdds = 1f;
		trackType.IsTurnLeft = true;
		trackType.IsNarrow = true;
		trackType.PrePieces.Add (TrackPiece.PieceType.kTPNarrowsEnemySnapDragon);
		trackType.AddVariation ("oz_ww_cliffs_narrow_hardleft_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPNarrowFastTurnLeftAlt;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = 1f;
		trackType.IsTurnLeft = true;
		trackType.IsNarrow = true;
		trackType.AddVariation ("oz_ww_cliffs_narrow_hardleft_a");	
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsEnemySnapDragon);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPNarrowFastTurnRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 3;
		trackType.SelectionOdds = 1f;
		trackType.IsTurnRight = true;
		trackType.IsNarrow = true;
		trackType.PrePieces.Add (TrackPiece.PieceType.kTPNarrowsEnemySnapDragon);
		trackType.AddVariation ("oz_ww_cliffs_narrow_hardright_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPNarrowFastTurnRightAlt;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = 1f;
		trackType.IsTurnRight = true;
		trackType.IsNarrow = true;
		trackType.AddVariation ("oz_ww_cliffs_narrow_hardright_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsEnemySnapDragon);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPNarrowsTurnRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 2;
		trackType.IsTurnRight = true;
		trackType.DifficultyLevel = 0;
		trackType.IsNarrow = true;
		trackType.AddVariation ("oz_ww_cliffs_narrow_hardright_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPNarrowsTurnLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 2;
		trackType.IsTurnLeft = true;
		trackType.DifficultyLevel = 0;
		trackType.IsNarrow = true;
		trackType.AddVariation ("oz_ww_cliffs_narrow_hardleft_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPNarrowsStraight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 0;
		trackType.IsNarrow = true;
		trackType.AddVariation ("oz_ww_cliffs_straightnarrow_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPNarrowsEnemySnapDragon;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 0;
		trackType.IsNarrow = true;
		trackType.IsStumble = true;
		trackType.IsEnemy = true;
		trackType.IsJumpOver = true;
		trackType.IsAnimated = true;
		trackType.SelectionOdds = .1f;
		trackType.AddVariation ("oz_ww_cliffs_straightnarrow_b");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		
		//  Narrrows End  //
		
		
		//Balloon Pieces		
		if (Settings.GetBool("balloon-enabled", true))
		{
			trackType = new TrackPieceTypeDefinition();
			trackType.TrackType = TrackPiece.PieceType.kTPBalloonJunction;
			trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
			trackType.EnvironmentSet = 1;
			trackType.Environment = 0;
			trackType.IsTurnLeft = true;
			trackType.IsTurnRight = true;
			trackType.IsJunction = true;
			trackType.IsBalloonJunction = true;
			trackType.AfterJunctionPiece = TrackPiece.PieceType.kTPStraightFlat;
			trackType.AlternateAfterJunctionPiece = TrackPiece.PieceType.kTPPreBalloon;
			trackType.PrePieces.Add(TrackPiece.PieceType.kTPStraight);
			trackType.DifficultyLevel = 2;
			trackType.SelectionOdds = Settings.GetFloat("balloon-odds-ww", 8f);
			trackType.AllowWhenFastTravelling = false;
			trackType.AddVariation ("oz_ww_cliffs_balloonjunctionLR_a");
			PieceTypes.Add(trackType.TrackType,trackType);
		}		

		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPBalloonFall;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionEnd;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 9;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 0f;	//Only spawn this directly
		//trackType.IsBalloon = true;
		trackType.AllowInRegularRotation = false;
		trackType.AddVariation ("oz_ww_transition_balloon_fall_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPPreBalloon;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionEnd;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		//trackType.SelectionOdds = 0f;
		trackType.AllowInRegularRotation = false;
		//trackType.IsBalloon = true;
		trackType.AddVariation ("oz_ww_cliffs_preballoon_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPBalloonEntrance);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPBalloonStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPBalloonStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPBalloonStraightShort);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		

		// Pieces that lead into and lead out of tunnel transition
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPBeforeTunnelTransitionEntrance;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.AllowInRegularRotation = false;
		trackType.AddVariation ("oz_ww_tt_exit_cliffs_a");
		PieceTypes.Add(trackType.TrackType,trackType);	
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAfterTunnelTransitionExit;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 0;
		trackType.AllowInRegularRotation = false;
		trackType.AddVariation ("oz_ww_tt_entrance_woods_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
	}
	
	///////////////////////////
	//DARK FOREST ENVIRONMENT//
	///////////////////////////
	private void PopulateDFPieceTypes()
	{
		TrackPieceTypeDefinition trackType = new TrackPieceTypeDefinition();
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPStraightFlatIntro;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 0;
		trackType.AllowInRegularRotation = false;
		trackType.AddVariation ("oz_df_forest_straight_b");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Balloon Pieces		
		if (Settings.GetBool("balloon-enabled", true))
		{
			trackType = new TrackPieceTypeDefinition();
			trackType.TrackType = TrackPiece.PieceType.kTPBalloonJunction;
			trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
			trackType.EnvironmentSet = 2;
			trackType.Environment = 0;
			trackType.IsTurnLeft = true;
			trackType.IsTurnRight = true;
			trackType.IsJunction = true;
			trackType.IsBalloonJunction = true;
			trackType.AfterJunctionPiece = TrackPiece.PieceType.kTPCemetaryStraight;
			trackType.AlternateAfterJunctionPiece = TrackPiece.PieceType.kTPPreBalloon;
			trackType.DifficultyLevel = 2;
			trackType.SelectionOdds = Settings.GetFloat("balloon-odds-df", 16f);
			trackType.AllowWhenFastTravelling = false;
			trackType.AddVariation ("oz_df_grave_balloonjunctionLR_a");
			PieceTypes.Add(trackType.TrackType,trackType);
		}		
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPBalloonFall;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionEnd;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 9;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 0f;	//Only spawn this directly
		//trackType.IsBalloon = true;
		trackType.AllowInRegularRotation = false;
		trackType.AddVariation ("oz_df_transition_balloon_fall_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPPreBalloon;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionEnd;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		//trackType.SelectionOdds = 0f;
		trackType.AllowInRegularRotation = false;
		//trackType.IsBalloon = true;
		trackType.AddVariation ("oz_df_grave_preballoon_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPBalloonEntrance);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPBalloonStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPBalloonStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPBalloonStraightShort);
		PieceTypes.Add(trackType.TrackType,trackType);

		/////////////////
		//FOREST TILES //
		/////////////////
		
		//Custom Segments
		
		//OverUnder, Over, Under - animated only
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAnimatedGauntlet1;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("oz_df_forest_straight_overunder_anim_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestAnimatedUnder);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestAnimatedOver);
		PieceTypes.Add(trackType.TrackType,trackType);		
		
		//Over Over Under
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPOverOverUnder;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 3;
		trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("oz_df_forest_straight_short_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestJumpOver);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestSlideUnder);
		PieceTypes.Add(trackType.TrackType,trackType);

		//Fast Turns
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFastTurnLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 3;
		trackType.SelectionOdds = .5f;
		trackType.IsTurnLeft = true;
		trackType.AddVariation ("oz_df_forest_hardleft_a");
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestTurnRight);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestTurnLeft);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFastTurnLeftAlt;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 5;
		trackType.SelectionOdds = .5f;
		trackType.IsTurnLeft = true;
		trackType.AddVariation ("oz_df_forest_hardleft_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestTurnLeft);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestStraight);
		PieceTypes.Add(trackType.TrackType,trackType);

		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFastTurnRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 3;
		trackType.SelectionOdds = .5f;
		trackType.IsTurnRight = true;
		trackType.AddVariation ("oz_df_forest_hardright_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestTurnRight);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFastTurnRightAlt;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 5;
		trackType.SelectionOdds = .5f;
		trackType.IsTurnRight = true;
		trackType.AddVariation ("oz_df_forest_hardright_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestTurnRight);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Gap, Gap
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestGapGap;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 5;
		trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("oz_df_forest_straight_short_c");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestGaps);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Gap, Slide, Gap
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestGapSlideGap;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 6;
		trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("oz_df_forest_straight_short_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestAnimatedOverUnder);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestGaps);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Gap, Under animated
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestGapAnimated;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 5;
		trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("oz_df_forest_straight_short_c");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestAnimatedUnder);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestGaps);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Over Under Anim
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestOverUnderAnimated;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = 1f;
		trackType.IsJumpOver = true;
		trackType.AddVariation ("oz_df_forest_straight_over_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestSlideUnder);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestAnimatedOver);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Under Under Over
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPUnderUnderOver;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("oz_df_forest_straight_under_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestSlideUnder);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestJumpOver);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Under Over Anim
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPUnderOverAnimated;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("oz_df_forest_straight_under_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestJumpOver);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestAnimatedOverUnder);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Over, Ani-Under, Ani-OverUnder
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAnimatedGauntlet5;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = 1f;
		trackType.IsJumpOver = true;
		trackType.AddVariation ("oz_df_forest_straight_over_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestAnimatedUnder);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestAnimatedOverUnder);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Gap Gap Gap
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestGapGapGap;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 6;
		trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("oz_df_forest_straight_short_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestGaps);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//End Custom Segments
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPStraightFlat;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_df_forest_straight_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPDarkForestEntrance;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionStart;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("oz_df_forest_transition_entrance_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPDarkForestExit;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionEnd;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("oz_df_forest_transition_exit_a");
		PieceTypes.Add(trackType.TrackType,trackType);
			
		//Turns
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPDarkForestJunction;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.IsTurnRight = true;
		trackType.IsTurnLeft = true;
		trackType.IsJunction = true;
		trackType.DifficultyLevel = 0;
		//trackType.AfterJunctionPiece = TrackPiece.PieceType.kTPDarkForestStraight;
		trackType.AddVariation ("oz_df_forest_junction_LR_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPDarkForestTurnRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.IsTurnRight = true;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("oz_df_forest_hardright_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPDarkForestTurnLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.IsTurnLeft = true;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("oz_df_forest_hardleft_a");
		PieceTypes.Add(trackType.TrackType,trackType);
			
		//Straights
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPDarkForestStraight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_df_forest_straight_b");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPDarkForestSlightLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 1f;
		trackType.IsSlightLeft =true;
		trackType.AddVariation ("oz_df_forest_slightleft_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPDarkForestSlightRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 1f;
		trackType.IsSlightRight =true;
		trackType.AddVariation ("oz_df_forest_slightright_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPDarkForestStraightShort;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 0;
		//trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_df_forest_straight_short_a");
		//trackType.AddVariation ("oz_df_forest_straight_short_b");
		trackType.AddVariation ("oz_df_forest_straight_short_c");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestEnemySnapDragon;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 0;
		trackType.IsStumble = true;
		trackType.IsEnemy = true;
		trackType.IsJumpOver = true;
		trackType.IsAnimated = true;
		trackType.SelectionOdds = 1.5f;
		trackType.AddVariation ("oz_df_forest_straight_short_b");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Jump Over
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPDarkForestJumpOver;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 1;
		//trackType.SelectionOdds = 1f;
		trackType.IsJumpOver = true;
		trackType.AddVariation ("oz_df_forest_straight_over_a");
		//trackType.AddVariation ("oz_df_forest_slightleft_over_a");
		//trackType.AddVariation ("oz_df_forest_slightright_over_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Slide Under
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPDarkForestSlideUnder;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 1;
		//trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("oz_df_forest_straight_under_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPDarkForestSlideUnderLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 1;
		//trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.IsSlightLeft =true;
		trackType.AddVariation ("oz_df_forest_slightleft_under_a");
		PieceTypes.Add(trackType.TrackType,trackType);

		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPDarkForestSlideUnderRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 1;
		//trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.IsSlightRight =true;
		trackType.AddVariation ("oz_df_forest_slightright_under_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Over Under
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPDarkForestOverUnder;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 3;
		//trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.IsJumpOver = true;
		trackType.AddVariation ("oz_df_forest_straight_overunder_a");
		//trackType.AddVariation ("oz_df_forest_slightleft_overunder_a");
		//trackType.AddVariation ("oz_df_forest_slightright_overunder_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Is Animated Obstacles
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPDarkForestAnimatedOverUnder; //Just under actually
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 3;
		trackType.SelectionOdds = 1f;
		trackType.IsAnimated = true;
		//trackType.IsJumpOver = true;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("oz_df_forest_straight_overunder_anim_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPDarkForestAnimatedUnder;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 2;
		trackType.SelectionOdds = 1f;
		trackType.IsAnimated = true;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("oz_df_forest_straight_under_anim_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPDarkForestAnimatedOver;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 1;
		trackType.SelectionOdds = 1f;
		trackType.IsJumpOver = true;
		trackType.IsAnimated = true;
		trackType.AddVariation ("oz_df_forest_straight_over_anim_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Path Obstacles
//		trackType = new TrackPieceTypeDefinition();
//		trackType.TrackType = TrackPiece.PieceType.kTPDarkForestLeftObstacle;
//		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
//		trackType.EnvironmentSet = 2;
//		trackType.Environment = 1;
//		trackType.DifficultyLevel = 1;
//		trackType.SelectionOdds = .5f;
//		trackType.IsLedgeRight = true;
//		trackType.AddVariation ("oz_df_forest_straight_leftobstacle_a");
//		PieceTypes.Add(trackType.TrackType,trackType);
//		
//		trackType = new TrackPieceTypeDefinition();
//		trackType.TrackType = TrackPiece.PieceType.kTPDarkForestRightObstacle;
//		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
//		trackType.EnvironmentSet = 2;
//		trackType.Environment = 1;
//		trackType.DifficultyLevel = 1;
//		trackType.SelectionOdds = .5f;
//		trackType.IsLedgeLeft = true;
//		trackType.AddVariation ("oz_df_forest_straight_rightobstacle_a");
//		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Gaps
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPDarkForestGaps;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 2;
		//trackType.SelectionOdds = 1f;
		trackType.IsGap = true;
		trackType.IsJumpOver = true;
		trackType.AddVariation ("oz_df_forest_straight_smallgap_a");
		trackType.AddVariation ("oz_df_forest_straight_largegap_a");
		PieceTypes.Add(trackType.TrackType,trackType);

		////////////////////
		//GRAVEYARD TILES //
		////////////////////
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPEnvSetJunction;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 0;
		trackType.IsTurnLeft = true;
		trackType.IsTurnRight = true;
		trackType.IsJunction = true;
		trackType.DifficultyLevel = 0;
		trackType.AfterJunctionPiece = TrackPiece.PieceType.kTPStraight;
		trackType.AllowWhenFastTravelling = false;
		trackType.AddVariation ("oz_df_grave_envset_junctionLR_a");
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPStraight);
		PieceTypes.Add(trackType.TrackType,trackType);	
		
		//Enterance and Exit
		/*
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPCemetaryEntrance;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionStart;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("oz_df_grave_transition_entrance_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPCemetaryExit;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionEnd;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("oz_df_grave_transition_exit_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		*/
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPCemetaryStraight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 0;
		trackType.IsStumble = true;
		trackType.IsStairs = true;
		trackType.DifficultyLevel = 0;
		//trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_df_grave_arch_a");
		//trackType.AddVariation ("oz_df_grave_slightrightarch_a");
		//trackType.AddVariation ("oz_df_grave_slightleftarch_a");
		//trackType.AddVariation ("oz_df_grave_arch_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPCemetarySlight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 0;
		trackType.IsStumble = true;
		trackType.IsStairs = true;
		trackType.DifficultyLevel = 0;
		//trackType.SelectionOdds = 1f;
		//trackType.AddVariation ("oz_df_grave_arch_a");
		trackType.AddVariation ("oz_df_grave_slightrightarch_a");
		trackType.AddVariation ("oz_df_grave_slightleftarch_a");
		//trackType.AddVariation ("oz_df_grave_arch_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPStraight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		//trackType.SelectionOdds = 1f;
		trackType.IsStairs = true;
		trackType.AddVariation ("oz_df_grave_arch_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPCemetaryTurnLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.IsTurnLeft = true;
		//trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_df_grave_hardleft_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPCemetaryTurnRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.IsTurnRight = true;
		//trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_df_grave_hardright_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPCemetarytJunction;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 0;
		trackType.IsTurnRight = true;
		trackType.IsTurnLeft = true;
		trackType.IsJunction = true;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 1f;
		trackType.AfterJunctionPiece = TrackPiece.PieceType.kTPStraight;
		trackType.AddVariation ("oz_df_grave_junction_LR_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		/////////////////
		//RUINS TILES //
		////////////////
		
		//Custom Tiles
		
		//Custom Fast Turn Sections
//		trackType = new TrackPieceTypeDefinition();
//		trackType.TrackType = TrackPiece.PieceType.kTPNarrowFastTurnLeft;
//		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
//		trackType.EnvironmentSet = 2;
//		trackType.Environment = 2;
//		trackType.DifficultyLevel = 3;
//		trackType.SelectionOdds = 1f;
//		trackType.IsTurnLeft = true;
//		trackType.IsNarrow = true;
//		trackType.AddVariation ("oz_df_ruins_hardleft_a");
//		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsTurnRight);
//		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsTurnLeft);
//		PieceTypes.Add(trackType.TrackType,trackType);
		
//		trackType = new TrackPieceTypeDefinition();
//		trackType.TrackType = TrackPiece.PieceType.kTPNarrowFastTurnLeftAlt;
//		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
//		trackType.EnvironmentSet = 2;
//		trackType.Environment = 2;
//		trackType.DifficultyLevel = 3;
//		trackType.SelectionOdds = 1f;
//		trackType.IsTurnLeft = true;
//		trackType.IsNarrow = true;
//		trackType.AddVariation ("oz_df_ruins_hardleft_a");
//		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsTurnRight);
//		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsTurnRight);
//		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsTurnLeft);
//		PieceTypes.Add(trackType.TrackType,trackType);
		
//		trackType = new TrackPieceTypeDefinition();
//		trackType.TrackType = TrackPiece.PieceType.kTPNarrowFastTurnRight;
//		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
//		trackType.EnvironmentSet = 2;
//		trackType.Environment = 2;
//		trackType.DifficultyLevel = 3;
//		trackType.SelectionOdds = 1f;
//		trackType.IsTurnRight = true;
//		trackType.IsNarrow = true;
//		trackType.AddVariation ("oz_df_ruins_hardright_a");
//		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsTurnLeft);
//		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsTurnRight);
//		PieceTypes.Add(trackType.TrackType,trackType);
		
//		trackType = new TrackPieceTypeDefinition();
//		trackType.TrackType = TrackPiece.PieceType.kTPNarrowFastTurnRightAlt;
//		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
//		trackType.EnvironmentSet = 2;
//		trackType.Environment = 2;
//		trackType.DifficultyLevel = 3;
//		trackType.SelectionOdds = 1f;
//		trackType.IsTurnRight = true;
//		trackType.IsNarrow = true;
//		trackType.AddVariation ("oz_df_ruins_hardright_a");
//		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsTurnLeft);
//		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsTurnLeft);
//		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsTurnRight);
//		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Ruins Animated Run
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAnimatedGauntlet2;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 5;
		trackType.SelectionOdds = 1f;
		trackType.IsJumpOver = true;
		trackType.IsNarrow = true;
		trackType.AddVariation ("oz_df_ruins_over_anim_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsShortStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsUnderAnim);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsShortStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsSmallGapAnim);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Over into animated gap
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAnimatedGauntlet3;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 3;
		trackType.SelectionOdds = 1f;
		trackType.IsGap = true;
		trackType.IsJumpOver = true;
		trackType.IsNarrow = true;
		trackType.AddVariation ("oz_df_ruins_largegap_anim_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsShortStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsSlideUnder);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Double collapsing gaps
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAnimatedGauntlet4;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 5;
		trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.IsNarrow = true;
		trackType.AddVariation ("oz_df_ruins_straight_narrow_b");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsSmallGapAnim);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsSmallGapAnim);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Gap then slide... but animated!
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGapAnimated;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.IsNarrow = true;
		trackType.AddVariation ("oz_df_ruins_straight_narrow_b");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsOverUnder);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsShortStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsUnderAnim);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Gap then Gap
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGapGap;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.IsNarrow = true;
		trackType.AddVariation ("oz_df_ruins_straight_narrow_b");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsGaps);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Gap, OverUnder, Gap
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGapSlideGap;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 6;
		trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.IsNarrow = true;
		trackType.AddVariation ("oz_df_ruins_straight_narrow_b");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsSmallGapAnim);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsOverUnder);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsGaps);
		PieceTypes.Add(trackType.TrackType,trackType);

		//Over Under Animated
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPOverUnderAnimated;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 5;
		trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.IsNarrow = true;
		trackType.AddVariation ("oz_df_ruins_under_anim_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsShortStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsOverUnder);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsShortStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsOverAnim);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Gap Gap Gap
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGapGapGap;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 6;
		trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("oz_df_ruins_straight_narrow_b");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsSmallGapAnim);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRuinsSmallGapAnim);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//End Custom Segments
		
		//Enterance and Exit
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPRuinsEntrance;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionStart;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("oz_df_ruins_transition_entrance_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPRuinsExit;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionEnd;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("oz_df_ruins_transition_exit_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Turns
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPRuinsJunction;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 2;
		trackType.IsTurnRight = true;
		trackType.IsTurnLeft = true;
		trackType.IsJunction = true;
		trackType.DifficultyLevel = 0;
		trackType.AfterJunctionPiece = TrackPiece.PieceType.kTPRuinsStraight;
		trackType.AddVariation ("oz_df_ruins_junctionLR_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPRuinsTurnRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 2;
		trackType.IsTurnRight = true;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("oz_df_ruins_hardright_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPRuinsTurnLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 2;
		trackType.IsTurnLeft = true;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("oz_df_ruins_hardleft_a");
		PieceTypes.Add(trackType.TrackType,trackType);
			
		//Straights
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPRuinsStraight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 0;
		//trackType.SelectionOdds = 0.4f;
		trackType.IsNarrow = true;
		trackType.AddVariation ("oz_df_ruins_straight_narrow_a");
		//trackType.AddVariation ("oz_df_ruins_straight_narrow_b");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPRuinsShortStraight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 0;
		//trackType.SelectionOdds = 0.4f;
		trackType.IsNarrow = true;
		//trackType.AddVariation ("oz_df_ruins_straight_narrow_a");
		trackType.AddVariation ("oz_df_ruins_straight_narrow_b");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPNarrowsEnemySnapDragon;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 0;
		trackType.IsStumble = true;
		trackType.IsNarrow = true;
		trackType.AddVariation ("oz_df_ruins_straight_narrow_c");
		PieceTypes.Add(trackType.TrackType,trackType);
		
//		trackType = new TrackPieceTypeDefinition();
//		trackType.TrackType = TrackPiece.PieceType.kTPDarkForestStraightShort;
//		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
//		trackType.EnvironmentSet = 2;
//		trackType.Environment = 0;
//		trackType.DifficultyLevel = 0;
//		trackType.SelectionOdds = 1f;
//		trackType.AddVariation ("oz_df_forest_straight_a");
//		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Jump Over
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPRuinsJumpOver;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 1;
		//trackType.SelectionOdds = 1f;
		trackType.IsJumpOver = true;
		trackType.IsNarrow = true;
		trackType.AddVariation ("oz_df_ruins_straight_narrow_over_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Slide Under
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPRuinsSlideUnder;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 2;
		//trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.IsNarrow = true;
		trackType.AddVariation ("oz_df_ruins_straight_narrow_under_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Over Under
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPRuinsOverUnder;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 3;
		//trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.IsJumpOver = true;
		trackType.IsNarrow = true;
		trackType.AddVariation ("oz_df_ruins_straight_narrow_overunder_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Gaps
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPRuinsGaps;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 1;
		//trackType.SelectionOdds = 1f;
		trackType.IsGap = true;
		trackType.IsJumpOver = true;
		trackType.IsNarrow = true;
		trackType.AddVariation ("oz_df_ruins_straight_narrow_smallgap_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Animated Tiles
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPRuinsUnderAnim;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 3;
		//trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.IsNarrow = true;
		trackType.AddVariation ("oz_df_ruins_under_anim_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPRuinsOverAnim;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 3;
		//trackType.SelectionOdds = 1f;
		trackType.IsJumpOver = true;
		trackType.IsNarrow = true;
		trackType.AddVariation ("oz_df_ruins_over_anim_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPRuinsSmallGapAnim;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 3;
		//trackType.SelectionOdds = 1f;
		trackType.IsGap = true;
		trackType.IsJumpOver = true;
		trackType.IsNarrow = true;
		trackType.AddVariation ("oz_df_ruins_largegap_anim_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		
		// Pieces that lead into and lead out of tunnel transition
		// note that graveyard is before the entrance and darkforest forest is after the exit
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPBeforeTunnelTransitionEntrance;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.AllowInRegularRotation = false;
		trackType.AddVariation ("oz_df_tt_exit_grave_a");
		PieceTypes.Add(trackType.TrackType,trackType);	
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAfterTunnelTransitionExit;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 2;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 0;
		trackType.AllowInRegularRotation = false;
		trackType.AddVariation ("oz_df_tt_entrance_forest_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPDarkForestStraightShort);
		PieceTypes.Add(trackType.TrackType,trackType);

	}
	
	//YBR Tiles
	private void PopulateYBRPieceTypes()
	{
		TrackPieceTypeDefinition trackType = new TrackPieceTypeDefinition();
		
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPStraightFlatIntro;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.AllowInRegularRotation = false;
		trackType.AddVariation ("oz_ybr_fields_straight_true_c");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Balloon Pieces		
		if (Settings.GetBool("balloon-enabled", true))
		{
			trackType = new TrackPieceTypeDefinition();
			trackType.TrackType = TrackPiece.PieceType.kTPBalloonJunction;
			trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
			trackType.EnvironmentSet = 3;
			trackType.Environment = 0;
			trackType.IsTurnLeft = true;
			trackType.IsTurnRight = true;
			trackType.IsJunction = true;
			trackType.IsBalloonJunction = true;
			trackType.AfterJunctionPiece = TrackPiece.PieceType.kTPFieldsStraight;
			trackType.AlternateAfterJunctionPiece = TrackPiece.PieceType.kTPPreBalloon;
			trackType.DifficultyLevel = 2;
			trackType.SelectionOdds = Settings.GetFloat("balloon-odds", 8f);
			trackType.AllowWhenFastTravelling = false;
			trackType.AddVariation ("oz_ybr_fields_balloonjunctionLR_a");
			PieceTypes.Add(trackType.TrackType,trackType);
		}		
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPBalloonFall;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionEnd;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 9;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 0f;	//Only spawn this directly
		//trackType.IsBalloon = true;
		trackType.AllowInRegularRotation = false;
		trackType.AddVariation ("oz_ybr_transition_balloon_fall_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPPreBalloon;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionEnd;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		//trackType.SelectionOdds = 0f;
		trackType.AllowInRegularRotation = false;
		//trackType.IsBalloon = true;
		trackType.AddVariation ("oz_ybr_fields_preballoon_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPBalloonEntrance);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		
		//REMOVED FOR LOW-END DEVICES
		if(!GameController.SharedInstance.LessTrackPieces())
		{
			trackType = new TrackPieceTypeDefinition();
			trackType.TrackType = TrackPiece.PieceType.kTPFieldsStraightFlatLong;
			trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
			trackType.EnvironmentSet = 3;
			trackType.Environment = 0;
			trackType.DifficultyLevel = 0;
			trackType.IsSlightLeft = true;
			//trackType.SelectionOdds = 0.4f;
			trackType.AddVariation ("oz_ybr_fields_straight_a");
			PieceTypes.Add(trackType.TrackType,trackType);		
			
			trackType = new TrackPieceTypeDefinition();
			trackType.TrackType = TrackPiece.PieceType.kTPEnemySnapDragon;
			trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
			trackType.EnvironmentSet = 3;
			trackType.Environment = 0;
			trackType.IsStumble = true;
			trackType.IsEnemy = true;
			trackType.IsJumpOver = true;
			trackType.IsAnimated = true;
			trackType.IsSlightLeft = true;
			trackType.DifficultyLevel = 1;
			trackType.SelectionOdds = 1.5f; 
			trackType.AddVariation ("oz_ybr_fields_straight_snapdragon_a");
			PieceTypes.Add(trackType.TrackType,trackType);
	
			trackType = new TrackPieceTypeDefinition();
			trackType.TrackType = TrackPiece.PieceType.kTPFieldsMonkey;
			trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
			trackType.EnvironmentSet = 3;
			trackType.Environment = 0;
			trackType.IsJumpOver = true;
			trackType.IsSlideUnder = true;
			trackType.IsAttackingBaboon = true;
			trackType.IsEnemy = true;
			trackType.DifficultyLevel = 3;
			//trackType.SelectionOdds = 0.4f;
			trackType.AddVariation ("oz_ybr_fields_monkeyswoop_a");
			PieceTypes.Add(trackType.TrackType,trackType);			
				
			trackType = new TrackPieceTypeDefinition();
			trackType.TrackType = TrackPiece.PieceType.kTPFieldsSlightLeft;
			trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
			trackType.EnvironmentSet = 3;
			trackType.Environment = 0;
			trackType.DifficultyLevel = 0;
			trackType.IsSlightLeft =true;
			trackType.SelectionOdds = 0.8f;
			trackType.AddVariation ("oz_ybr_fields_slightleft_a");
			PieceTypes.Add(trackType.TrackType,trackType);
			
			trackType = new TrackPieceTypeDefinition();
			trackType.TrackType = TrackPiece.PieceType.kTPFieldsSlightRight;
			trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
			trackType.EnvironmentSet = 3;
			trackType.Environment = 0;
			trackType.DifficultyLevel = 0;
			trackType.IsSlightRight =true;
			trackType.SelectionOdds = 0.8f;
			trackType.AddVariation ("oz_ybr_fields_slightright_a");
			PieceTypes.Add(trackType.TrackType,trackType);			
			
			//Up Up and Away//
			trackType = new TrackPieceTypeDefinition();
			trackType.TrackType = TrackPiece.PieceType.kTPFieldsUpUpAndAway;
			trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
			trackType.EnvironmentSet = 3;
			trackType.Environment = 0;
			trackType.DifficultyLevel = 3;
			trackType.SelectionOdds = .5f;
			//trackType.IsSlideUnder = true;
			trackType.IsSlightLeft = true;
			trackType.AddVariation ("oz_ybr_fields_slightleft_a");
			trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsUndulateDown);
			trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsJumpOverSlightRight);
			trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsUndulateUp);
			trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsSlideUnderSlightLeft);
			trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsUndulateDown);
			trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsOverUnderSlightRight);
			PieceTypes.Add(trackType.TrackType,trackType);			
			
		}
		
		
		////////////////
		//FARM TILES //
		//////////////
		
		//CUSTOM FARMS
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPUnderOverAnimated;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 5;
		trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("oz_ybr_farms_straight_under_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsAnimated);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsSlideUnder);
		PieceTypes.Add(trackType.TrackType,trackType);		
		
		//Fast Turns Easy Left//
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFarmsFastTurnLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 3;
		trackType.SelectionOdds = 1f;
		trackType.IsTurnLeft = true;
		trackType.AddVariation ("oz_ybr_farms_hardleft_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsStraightShort);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Fast Turns Easy Right//
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFarmsFastTurnRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 3;
		trackType.SelectionOdds = 1f;
		trackType.IsTurnRight = true;
		trackType.AddVariation ("oz_ybr_farms_hardright_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsStraightShort);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Fast Turns Medium Left//
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFarmsFastTurnLeftAlt;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = 1f;
		trackType.IsTurnLeft = true;
		trackType.AddVariation ("oz_ybr_farms_hardleft_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsStraightShort);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Fast Turns Medium Right//
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFarmsFastTurnRightAlt;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = 1f;
		trackType.IsTurnRight = true;
		trackType.AddVariation ("oz_ybr_farms_hardright_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsStraightShort);
		PieceTypes.Add(trackType.TrackType,trackType);

		//Fast Turns Hard Left//
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFarmsReallyFastTurnLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 5;
		trackType.SelectionOdds = 1.25f;
		trackType.IsTurnLeft = true;
		trackType.AddVariation ("oz_ybr_farms_hardleft_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsStraightShort);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Fast Turns Hard Right//
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFarmsReallyFastTurnRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 5;
		trackType.SelectionOdds = 1.25f;
		trackType.IsTurnRight = true;
		trackType.AddVariation ("oz_ybr_farms_hardright_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsStraightShort);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Fast Turns Very Hard Left//
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFarmsReallyFastTurnLeftAlt;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 6;
		trackType.SelectionOdds = 1.25f;
		trackType.IsTurnLeft = true;
		trackType.AddVariation ("oz_ybr_farms_hardleft_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsStraightShort);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Fast Turns Very Hard Right//
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFarmsReallyFastTurnRightAlt;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 6;
		trackType.SelectionOdds = 1.25f;
		trackType.IsTurnRight = true;
		trackType.AddVariation ("oz_ybr_farms_hardright_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsStraightShort);
		PieceTypes.Add(trackType.TrackType,trackType);		
			
		//Over Over Under
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPOverOverUnder;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 3;
		trackType.SelectionOdds = 1f;
		trackType.IsJumpOver = true;
		trackType.AddVariation ("oz_ybr_farms_straight_over_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsOverUnder);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsSlideUnder);
		PieceTypes.Add(trackType.TrackType,trackType);		
		
		//END CUSTOM FARMS
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPStraight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_ybr_farms_straight_a");	
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFarmsEntrance;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionStart;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("oz_ybr_farms_transition_entrance_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFarmsStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFarmsExit;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionEnd;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 0;
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPFarmsStraight);
		trackType.AddVariation ("oz_ybr_farms_transition_exit_a");
		PieceTypes.Add(trackType.TrackType,trackType);
			
		//Turns
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFarmsJunction;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 1;
		trackType.IsTurnRight = true;
		trackType.IsTurnLeft = true;
		trackType.IsJunction = true;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 1f;
		trackType.AfterJunctionPiece = TrackPiece.PieceType.kTPFarmsStraight;
		trackType.AddVariation ("oz_ybr_farms_junctionLR_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFarmsTurnRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 1;
		trackType.IsTurnRight = true;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("oz_ybr_farms_hardright_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFarmsTurnLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 1;
		trackType.IsTurnLeft = true;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("oz_ybr_farms_hardleft_a");
		PieceTypes.Add(trackType.TrackType,trackType);
			
		//Straights
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFarmsStraight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_ybr_farms_straight_b");	
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFarmsStraightShort;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_ybr_farms_straight_c");	
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//added to other stumbles as a variation
//		trackType = new TrackPieceTypeDefinition();
//		trackType.TrackType = TrackPiece.PieceType.kTPFarmsStraightShortStumble;
//		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
//		trackType.EnvironmentSet = 3;
//		trackType.Environment = 1;
//		trackType.DifficultyLevel = 4;
//		trackType.IsStumble = true;
//		//trackType.SelectionOdds = 1f;
//		trackType.AddVariation ("oz_ybr_farms_straight_stumble_a");	
//		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Jump Over
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFarmsJumpOver;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 1;
		//trackType.SelectionOdds = 1f;
		trackType.IsJumpOver = true;
		trackType.AddVariation ("oz_ybr_farms_straight_over_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		/*
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFarmsHardJump;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 4;
		trackType.IsJumpOver = true;
		trackType.AddVariation ("oz_ybr_farms_straight_overleft_a");
		trackType.AddVariation ("oz_ybr_farms_straight_overright_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		*/
		
		//Slide Under
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFarmsSlideUnder;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 3;
		//trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("oz_ybr_farms_straight_under_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Over Under
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFarmsOverUnder;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 2;
		//trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.IsJumpOver = true;
		trackType.AddVariation ("oz_ybr_farms_straight_overunder_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFarmsStumbles;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 1;
		trackType.SelectionOdds = 1f;
		trackType.IsStumble = true;
		trackType.AddVariation ("oz_ybr_farms_straight_leftpath_a");
		trackType.AddVariation ("oz_ybr_farms_straight_rightpath_a");
		PieceTypes.Add(trackType.TrackType,trackType);
				
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFarmsStumblesEnemy;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 3;
		trackType.SelectionOdds = 1f;
		trackType.IsStumble = true;
		trackType.AddVariation ("oz_ybr_farms_straight_stumble_a");	
		PieceTypes.Add(trackType.TrackType,trackType);
				
		//Animated
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFarmsAnimated;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 3;
		//trackType.SelectionOdds = 1f;
		trackType.IsJumpOver = true;
		trackType.IsAttackingBaboon = true;
		trackType.IsEnemy = true;
		//trackType.IsAnimated = true;
		trackType.AddVariation ("oz_ybr_farms_straight_enemy_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		
		//////////////////
		//FIELDS TILES //
		////////////////

		//CUSTOM FIELDS
		
		//Turn Turn
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFieldsLeftRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 2;
		trackType.SelectionOdds = 1f;
		trackType.IsTurnLeft = true;
		trackType.AddVariation ("oz_ybr_fields_hardleft_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsTurnRight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Turn Turn
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFieldsRightLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 2;
		trackType.SelectionOdds = 1f;
		trackType.IsTurnRight = true;
		trackType.AddVariation ("oz_ybr_fields_hardright_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsTurnLeft);
		PieceTypes.Add(trackType.TrackType,trackType);

		//Fast Turns Fields//
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFieldsFastTurnRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = 1f;
		trackType.IsTurnRight = true;
		trackType.AddVariation ("oz_ybr_fields_hardright_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsTurnLeft);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Fast Turns Fields//
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFieldsFastTurnLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = 1f;
		trackType.IsTurnLeft = true;
		trackType.AddVariation ("oz_ybr_fields_hardleft_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsTurnRight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Over Under Over
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFieldsOverUnderOver;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = 1f;
		trackType.IsJumpOver = true;
		//trackType.IsSlightRight = true;
		trackType.AddVariation ("oz_ybr_fields_straight_over_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsSlideUnderSlightRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsJumpOverSlightLeft);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Under Over Under
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFieldsUnderOverUnder;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 5;
		trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		//trackType.IsSlightRight = true;
		trackType.AddVariation ("oz_ybr_fields_slightright_under_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsJumpOverSlightLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsOverUnder);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Rolling Hills//
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFieldsRollingHills;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 3;
		trackType.SelectionOdds = 1f;
		trackType.IsSlightRight = true;
		trackType.AddVariation ("oz_ybr_fields_slightup_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsJumpOverSlightLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsUndulateDown);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsUndulateUp);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsSlideUnderSlightLeft);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Fields Flood//
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFieldsFlood;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = .1f;
		trackType.IsSlightLeft = true;
		trackType.AddVariation ("oz_ybr_fields_slightdown_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsUndulateUp);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsUndulateUp);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAnimatedGauntlet5;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = .75f;
		//trackType.IsLedgeLeft = true;
		trackType.IsSlightRight =true;
		trackType.IsJumpOver = true;
		trackType.AddVariation ("oz_ybr_fields_slightright_over_anim_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsJumpOver);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsEnemyUnder);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Gap then Gap
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGapGap;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("oz_ybr_fields_straight_true_c");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsGaps);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Gap then Gap... THEN GAP!
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGapGapGap;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 6;
		trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("oz_ybr_fields_straight_true_c");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsGaps);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Gap, Slide, Gap
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGapSlideGap;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 6;
		trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("oz_ybr_fields_straight_true_c");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsSlideUnder);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsGaps);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//END CUSTOM FIELDS
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPEnvSetJunction;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.IsTurnLeft = true;
		trackType.IsTurnRight = true;
		trackType.IsJunction = true;
		trackType.DifficultyLevel = 0;
		trackType.AfterJunctionPiece = TrackPiece.PieceType.kTPFieldsStraight;
		trackType.AllowWhenFastTravelling = false;
		trackType.AddVariation ("oz_ybr_fields_envset_junctionLR_a");
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPFieldsStraightFlat);
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPFieldsStraight);
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPFieldsStraightFlat);
		PieceTypes.Add(trackType.TrackType,trackType);	
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFieldsStraightFlat;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.IsSlightLeft = true;
		//trackType.SelectionOdds = 0.4f;
		trackType.AddVariation ("oz_ybr_fields_straight_b");
		PieceTypes.Add(trackType.TrackType,trackType);	
		
		//Turns
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFieldsJunction;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.IsTurnRight = true;
		trackType.IsTurnLeft = true;
		trackType.IsJunction = true;
		trackType.DifficultyLevel = 0;
		trackType.AfterJunctionPiece = TrackPiece.PieceType.kTPFieldsStraightFlat;
		trackType.AddVariation ("oz_ybr_fields_junctionLR_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFieldsTurnRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.IsTurnRight = true;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("oz_ybr_fields_hardright_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFieldsTurnLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.IsTurnLeft = true;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("oz_ybr_fields_hardleft_a");
		PieceTypes.Add(trackType.TrackType,trackType);
			
		//Straights
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFieldsStraight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_ybr_fields_straight_true_c");
		PieceTypes.Add(trackType.TrackType,trackType);	
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFieldsUndulateDown;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 1.2f;
		trackType.IsSlightLeft = true;
		trackType.AddVariation ("oz_ybr_fields_slightdown_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFieldsUndulateUp;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 1.2f;
		trackType.IsSlightRight = true;
		trackType.AddVariation ("oz_ybr_fields_slightup_a");
		PieceTypes.Add(trackType.TrackType,trackType);		
		
		//Jump Over
		//obstacle being removed will become a variation to other straight_a - N.N.
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFieldsJumpOver;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 1;
		trackType.SelectionOdds = 1f;
		trackType.IsJumpOver = true;
		trackType.AddVariation ("oz_ybr_fields_straight_over_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//obstacle being removed will become a variation to other slightleft - N.N.
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFieldsJumpOverSlightLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 1;
		//trackType.SelectionOdds = 1f;
		trackType.IsJumpOver = true;
		trackType.IsSlightLeft =true;
		trackType.AddVariation ("oz_ybr_fields_slightleft_over_a");
		PieceTypes.Add(trackType.TrackType,trackType);

		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFieldsJumpOverSlightRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = .25f;
		trackType.IsJumpOver = true;
		trackType.IsSlightRight =true;
		trackType.AddVariation ("oz_ybr_fields_slightright_over_anim_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Slide Under
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFieldsSlideUnder;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 1;
		//trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("oz_ybr_fields_straight_under_a");
		PieceTypes.Add(trackType.TrackType,trackType);

		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFieldsSlideUnderSlightLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 1;
		//trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.IsSlightLeft =true;
		trackType.AddVariation ("oz_ybr_fields_slightleft_under_a");
		PieceTypes.Add(trackType.TrackType,trackType);

		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFieldsSlideUnderSlightRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 1;
		//trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.IsSlightRight =true;
		trackType.AddVariation ("oz_ybr_fields_slightright_under_a");
		PieceTypes.Add(trackType.TrackType,trackType);		
		
		//Over Under
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFieldsOverUnder;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 2;
		//trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.IsJumpOver = true;
		trackType.AddVariation ("oz_ybr_fields_straight_overunder_a");
		PieceTypes.Add(trackType.TrackType,trackType);

		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFieldsOverUnderSlightLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 2;
		//trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.IsJumpOver = true;
		trackType.IsSlightLeft =true;
		trackType.AddVariation ("oz_ybr_fields_slightleft_overunder_a");
		PieceTypes.Add(trackType.TrackType,trackType);

		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFieldsOverUnderSlightRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 2;
		//trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.IsJumpOver = true;
		trackType.IsSlightRight =true;
		trackType.AddVariation ("oz_ybr_fields_slightright_overunder_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Animated Over
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFieldsEnemyOver;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 2;
		trackType.SelectionOdds = 1.25f;
		trackType.IsJumpOver = true;
		trackType.AddVariation ("oz_ybr_fields_straight_enemyover_anim_a");
		PieceTypes.Add(trackType.TrackType,trackType);
						
		//Animated Under
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFieldsEnemyUnder;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 3;
		trackType.SelectionOdds = 1.25f;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("oz_ybr_fields_straight_enemyunder_anim_a");
		PieceTypes.Add(trackType.TrackType,trackType);		
		
		//Gaps
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFieldsGaps;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 2;
		trackType.SelectionOdds = 0f; // Must be called manually
		trackType.IsGap = true;
		trackType.IsJumpOver = true;
		trackType.AddVariation ("oz_ybr_fields_straight_smallgap_a");
		trackType.AddVariation ("oz_ybr_fields_straight_biggap_a");
		PieceTypes.Add(trackType.TrackType,trackType);

		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFieldsSmallGap;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 1;
		trackType.SelectionOdds = 1f;
		trackType.IsGap = true;
		trackType.IsJumpOver = true;
		trackType.IsSlightLeft = true;
		trackType.AddVariation ("oz_ybr_fields_straight_smallgap_a");
		PieceTypes.Add(trackType.TrackType,trackType);		

		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFieldsLargeGap;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 1;
		trackType.SelectionOdds = 1f;
		trackType.IsGap = true;
		trackType.IsJumpOver = true;
		trackType.IsSlightRight = true;
		trackType.AddVariation ("oz_ybr_fields_straight_biggap_a");
		PieceTypes.Add(trackType.TrackType,trackType);			
		
		//////////////////
		//GROVES TILES //
		////////////////
		
		//CUSTOM GROVES
		
		//Triple Collapse Left//
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAnimatedGauntlet1;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = .5f;
		trackType.IsLedgeLeft = true;
		//trackType.IsSlightLeft =true;
		trackType.AddVariation ("oz_ybr_groves_slightleft_collapse_anim_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPGrovesCollapseRightLedge);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPGrovesCollapseLeftLedge);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Triple Collapse Right//
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAnimatedGauntlet2;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = .5f;
		trackType.IsLedgeRight = true;
		//trackType.IsSlightLeft =true;
		trackType.AddVariation ("oz_ybr_groves_slightright_collapse_anim_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPGrovesCollapseLeftLedge);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPGrovesCollapseRightLedge);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Quarter Turn Left
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAnimatedGauntlet3;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = 1f;
		//trackType.IsLedgeLeft = true;
		trackType.IsSlightLeft =true;
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPGrovesShortStraight);
		trackType.AddVariation ("oz_ybr_groves_straight_stumble_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPGrovesShortStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPGrovesUnderLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPGrovesShortStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPGrovesOverLeft);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Quarter Turn Right
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAnimatedGauntlet4;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = 1f;
		//trackType.IsLedgeLeft = true;
	    trackType.IsSlightRight = true;
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPGrovesShortStraight);
		trackType.AddVariation ("oz_ybr_groves_straight_stumble_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPGrovesShortStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPGrovesUnderRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPGrovesShortStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPGrovesOverRight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Left UnderOver Right
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGroveTiltUnderLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 5;
		trackType.SelectionOdds = .5f;
		trackType.IsLedgeLeft = true;
		//trackType.IsSlightLeft =true;
		trackType.AddVariation ("oz_ybr_groves_slightleft_collapse_anim_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPGrovesUnderRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPGrovesOverLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPGrovesCollapseRightLedge);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Right UnderOver Left
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGroveTiltUnderRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 5;
		trackType.SelectionOdds = .5f;
		trackType.IsLedgeRight = true;
		//trackType.IsSlightRight =true;
		trackType.AddVariation ("oz_ybr_groves_slightright_collapse_anim_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPGrovesUnderLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPGrovesOverRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPGrovesCollapseLeftLedge);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//CUSTOM EASY PIECES
		
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGrovesEasyOverLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 1;
		trackType.IsJumpOver = true;
		//trackType.SelectionOdds = 0.4f;
		trackType.IsSlightLeft =true;
		trackType.AddVariation ("oz_ybr_groves_slightleft_over_anim_a");
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPGrovesShortStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
	
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGrovesEasyOverRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 1;
		trackType.IsJumpOver = true;
		//trackType.SelectionOdds = 0.4f;
		trackType.IsSlightRight =true;
		trackType.AddVariation ("oz_ybr_groves_slightright_over_anim_a");
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPGrovesShortStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGrovesEasyUnderLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 2;
		trackType.IsSlideUnder = true;
		//trackType.SelectionOdds = 0.4f;
		trackType.IsSlightLeft =true;
		trackType.AddVariation ("oz_ybr_groves_slightleft_under_anim_a");
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPGrovesShortStraight);
		PieceTypes.Add(trackType.TrackType,trackType);

		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGrovesEasyUnderRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 2;
		trackType.IsSlideUnder = true;
		//trackType.SelectionOdds = 0.4f;
		trackType.IsSlightRight =true;
		trackType.AddVariation ("oz_ybr_groves_slightright_under_anim_a");
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPGrovesShortStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGrovesEasyStraightShortStumble;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 1;
		trackType.IsStumble = true;
		//trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_ybr_groves_straight_stumble_a");
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPGrovesShortStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGrovesEasyCollapseRightLedge;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.IsLedgeRight = true;
		trackType.IsSlightRight = true;
		//trackType.SelectionOdds = 0.05f;
		trackType.DifficultyLevel = 2;
		trackType.AddVariation ("oz_ybr_groves_slightright_collapse_anim_a");
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPGrovesShortStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGrovesEasyCollapseLeftLedge;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.IsLedgeLeft = true;
		trackType.IsSlightLeft = true;
		//trackType.SelectionOdds = 0.05f;
		trackType.DifficultyLevel = 2;
		trackType.AddVariation ("oz_ybr_groves_slightleft_collapse_anim_a");
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPGrovesShortStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGrovesEasyLedgeLeftEnter;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceStart;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.CompoundPieceMiddleType = TrackPiece.PieceType.kTPGrovesLedgeLeftMid;
		trackType.CompoundPieceEndType = TrackPiece.PieceType.kTPGrovesLedgeLeftExit;
		trackType.CompoundPieceMinMiddleCount = 1;
		trackType.CompoundPieceMaxMiddleCount = 2;
		trackType.IsLedgeLeft = true;
		trackType.IsSlightLeft = true;
		//trackType.SelectionOdds = 0.05f;
		trackType.DifficultyLevel = 1;
		trackType.AddVariation ("oz_ybr_groves_leftledge_start_a");
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPGrovesShortStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGrovesEasyLedgeRightEnter;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceStart;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.CompoundPieceMiddleType = TrackPiece.PieceType.kTPGrovesLedgeRightMid;
		trackType.CompoundPieceEndType = TrackPiece.PieceType.kTPGrovesLedgeRightExit;
		trackType.CompoundPieceMinMiddleCount = 1;
		trackType.CompoundPieceMaxMiddleCount = 2;
		trackType.IsLedgeRight = true;
		trackType.IsSlightRight = true;
		//trackType.SelectionOdds = 0.05f;
		trackType.DifficultyLevel = 1;
		trackType.AddVariation ("oz_ybr_groves_rightledge_start_a");
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPGrovesShortStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		
		//END CUSTOM GROVES
		
		//Enterance and Exit
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGrovesEntrance;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionStart;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("oz_ybr_groves_transition_entrance_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGrovesExit;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionEnd;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("oz_ybr_groves_transition_exit_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Turns
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGrovesJunction;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.IsTurnRight = true;
		trackType.IsTurnLeft = true;
		trackType.IsJunction = true;
		trackType.DifficultyLevel = 0;
		trackType.AfterJunctionPiece = TrackPiece.PieceType.kTPGrovesStraight;
		trackType.AddVariation ("oz_ybr_groves_junctionLR_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGrovesTurnRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.IsTurnRight = true;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("oz_ybr_groves_hardright_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGrovesTurnLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.IsTurnLeft = true;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("oz_ybr_groves_hardleft_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGrovesSlightLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 0;
		//trackType.SelectionOdds = 0.4f;
		trackType.IsSlightLeft =true;
		trackType.AddVariation ("oz_ybr_groves_slightleft_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGrovesSlightRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 0;
		//trackType.SelectionOdds = 0.4f;
		trackType.IsSlightRight =true;
		trackType.AddVariation ("oz_ybr_groves_slightright_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGrovesStraight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 0;
		//trackType.SelectionOdds = 0.4f;
		trackType.AddVariation ("oz_ybr_groves_straight_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGrovesShortStraight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 0;
		//trackType.SelectionOdds = 0.4f;
		trackType.AddVariation ("oz_ybr_groves_straight_b");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Obstacles
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGrovesOverLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 4;
		trackType.IsJumpOver = true;
		//trackType.SelectionOdds = 0.4f;
		trackType.IsSlightLeft =true;
		trackType.AddVariation ("oz_ybr_groves_slightleft_over_anim_a");
		PieceTypes.Add(trackType.TrackType,trackType);
	
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGrovesOverRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 4;
		trackType.IsJumpOver = true;
		//trackType.SelectionOdds = 0.4f;
		trackType.IsSlightRight =true;
		trackType.AddVariation ("oz_ybr_groves_slightright_over_anim_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGrovesUnderLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 5;
		trackType.IsSlideUnder = true;
		//trackType.SelectionOdds = 0.4f;
		trackType.IsSlightLeft =true;
		trackType.AddVariation ("oz_ybr_groves_slightleft_under_anim_a");
		PieceTypes.Add(trackType.TrackType,trackType);

		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGrovesUnderRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 5;
		trackType.IsSlideUnder = true;
		//trackType.SelectionOdds = 0.4f;
		trackType.IsSlightRight =true;
		trackType.AddVariation ("oz_ybr_groves_slightright_under_anim_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGrovesStraightShortStumble;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 4;
		trackType.IsStumble = true;
		//trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_ybr_groves_straight_stumble_a");	
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGrovesCollapseRightLedge;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.IsLedgeRight = true;
		trackType.IsSlightRight = true;
		//trackType.SelectionOdds = 0.05f;
		trackType.DifficultyLevel = 4;
		trackType.AddVariation ("oz_ybr_groves_slightright_collapse_anim_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGrovesCollapseLeftLedge;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.IsLedgeLeft = true;
		trackType.IsSlightLeft = true;
		//trackType.SelectionOdds = 0.05f;
		trackType.DifficultyLevel = 4;
		trackType.AddVariation ("oz_ybr_groves_slightleft_collapse_anim_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Ledges
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGrovesLedgeLeftEnter;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceStart;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.CompoundPieceMiddleType = TrackPiece.PieceType.kTPGrovesLedgeLeftMid;
		trackType.CompoundPieceEndType = TrackPiece.PieceType.kTPGrovesLedgeLeftExit;
		trackType.CompoundPieceMinMiddleCount = 1;
		trackType.CompoundPieceMaxMiddleCount = 2;
		trackType.IsLedgeLeft = true;
		trackType.IsSlightLeft = true;
		//trackType.SelectionOdds = 0.05f;
		trackType.DifficultyLevel = 3;
		trackType.AddVariation ("oz_ybr_groves_leftledge_start_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGrovesLedgeLeftMid;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceMiddle;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.IsLedgeLeft = true;
		//trackType.DifficultyLevel = 3;
		trackType.AddVariation ("oz_ybr_groves_leftledge_middle_a");
		trackType.AddVariation ("oz_ybr_groves_leftledge_middle_b");
		//trackType.AddVariation ("oz_ybr_groves_leftledge_middle_c");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGrovesLedgeLeftExit;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceEnd;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.IsLedgeLeft = true;
		//trackType.DifficultyLevel = 3;
		trackType.AddVariation ("oz_ybr_groves_leftledge_end_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
	
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGrovesLedgeRightEnter;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceStart;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.CompoundPieceMiddleType = TrackPiece.PieceType.kTPGrovesLedgeRightMid;
		trackType.CompoundPieceEndType = TrackPiece.PieceType.kTPGrovesLedgeRightExit;
		trackType.CompoundPieceMinMiddleCount = 1;
		trackType.CompoundPieceMaxMiddleCount = 2;
		trackType.IsLedgeRight = true;
		trackType.IsSlightRight = true;
		//trackType.SelectionOdds = 0.05f;
		trackType.DifficultyLevel = 3;
		trackType.AddVariation ("oz_ybr_groves_rightledge_start_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGrovesLedgeRightMid;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceMiddle;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.IsLedgeRight = true;
		//trackType.DifficultyLevel = 3;
		trackType.AddVariation ("oz_ybr_groves_rightledge_middle_a");
		trackType.AddVariation ("oz_ybr_groves_rightledge_middle_b");
		//trackType.AddVariation ("oz_ybr_groves_rightledge_middle_c");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGrovesLedgeRightExit;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceEnd;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 2;
		trackType.IsLedgeRight = true;
		//trackType.DifficultyLevel = 3;
		trackType.AddVariation ("oz_ybr_groves_rightledge_end_a");
		PieceTypes.Add(trackType.TrackType,trackType);
//		
		// Pieces that lead into and lead out of tunnel transition
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPBeforeTunnelTransitionEntrance;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.AllowInRegularRotation = false;
		trackType.AddVariation ("oz_ybr_tt_exit_fields_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPFieldsStraightFlat);
		PieceTypes.Add(trackType.TrackType,trackType);	
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAfterTunnelTransitionExit;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 3;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.AllowInRegularRotation = false;
		trackType.AddVariation ("oz_ybr_tt_entrance_fields_a");
		
		PieceTypes.Add(trackType.TrackType,trackType);

	}
	

	///////////////////////////
	//EMERALD CITY ENVIRONMENT//
	///////////////////////////	
	private void PopulateECPieceTypes()
	{
		TrackPieceTypeDefinition trackType = new TrackPieceTypeDefinition();
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPStraightFlatIntro;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.AllowInRegularRotation = false;
		trackType.AddVariation ("oz_ec_ramparts_straight_a");
		PieceTypes.Add(trackType.TrackType,trackType);		
		
		/////////////////
		//RAMPART TILES//
		/////////////////
		#region Ramparts
		//Custom Segments
			
		//Fast Turns
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFastTurnLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 3;
		trackType.SelectionOdds = .25f;
		trackType.IsTurnLeft = true;
		trackType.AddVariation ("oz_ec_ramparts_hardleft_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFastTurnLeftAlt;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = .25f;
		trackType.IsTurnLeft = true;
		trackType.AddVariation ("oz_ec_ramparts_hardleft_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
	
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFastTurnLeftAltAlt;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = .25f;
		trackType.IsTurnLeft = true;
		trackType.AddVariation ("oz_ec_ramparts_hardleft_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPJumpOrSlide);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		PieceTypes.Add(trackType.TrackType,trackType);		
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFastTurnRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 3;
		trackType.SelectionOdds = .25f;
		trackType.IsTurnRight = true;
		trackType.AddVariation ("oz_ec_ramparts_hardright_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFastTurnRightAlt;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = .25f;
		trackType.IsTurnRight = true;
		trackType.AddVariation ("oz_ec_ramparts_hardright_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPFastTurnRightAltAlt;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = .25f;
		trackType.IsTurnRight = true;
		trackType.AddVariation ("oz_ec_ramparts_hardright_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPJumpOrSlide);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		PieceTypes.Add(trackType.TrackType,trackType);		
		
		//Winkie Guards
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAnimatedGauntlet1;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 3;
		trackType.SelectionOdds = .25f;
		trackType.IsStumble = true;
		trackType.AddVariation ("oz_ec_ramparts_straight_stumble_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPEnemySnapDragon);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPSlideUnderSlightLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPCurveRight);

		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAnimatedGauntlet1;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 3;
		trackType.SelectionOdds = .25f;
		trackType.IsStumble = true;
		trackType.AddVariation ("oz_ec_ramparts_straight_stumble_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPEnemySnapDragon);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPSlideUnderSlightRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPCurveLeft);	
		PieceTypes.Add(trackType.TrackType,trackType);		

		//Gap to Flame
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAnimatedGauntlet3;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = 1.25f;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("oz_ec_ramparts_straight_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRampartsGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPJumpOrSlide);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Gap Under Flame
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAnimatedGauntlet4;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_ec_ramparts_straight_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRampartsGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPSlideUnderSlightRight);	
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPJumpOrSlide);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Flame Gap Flame
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAnimatedGauntlet5;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 5;
		trackType.SelectionOdds = 2f;
		trackType.AddVariation ("oz_ec_ramparts_straight_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPJumpOrSlide);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPRampartsGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPJumpOrSlide);
		PieceTypes.Add(trackType.TrackType,trackType);		
		
		/*TODO add equivelant tile or make a new custom segment TODO
		

		*/
		
		//End Custom Segments
		
		/* TODO add equivalent TODO
		//For Spencer to make new functions (IsLedgeLeft and IsLedgeRight as obstacles)
	
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPEnemyMonkey;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		//trackType.IsObstacle = true;
		trackType.IsEnemy = true;
		trackType.IsAnimated = true;
		trackType.IsAttackingBaboon = true;
		trackType.DifficultyLevel = 3;
		trackType.SelectionOdds = 2f;
		trackType.AddVariation ("oz_ww_cliffs_straight_monkey");
		PieceTypes.Add(trackType.TrackType,trackType);		
		 
	 

//Cliffs Down
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPUndulateDownEnter;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceStart;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.CompoundPieceMiddleType = TrackPiece.PieceType.kTPUndulateDownMid;
		trackType.CompoundPieceEndType = TrackPiece.PieceType.kTPUndulateDownExit;
		trackType.CompoundPieceMinMiddleCount = 1;
		trackType.CompoundPieceMaxMiddleCount = 2;
		trackType.SelectionOdds = 0.7f;
		trackType.DifficultyLevel = 1;
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.AddVariation ("oz_ww_cliffs_arch_flatdown_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPUndulateDownMid;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceMiddle;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 1;
		trackType.AddVariation ("oz_ww_cliffs_arch_down_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPUndulateDownExit;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceEnd;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 1;
		trackType.AddVariation ("oz_ww_cliffs_arch_downflat_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Cliffs Up
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPUndulateUpEnter;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceStart;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		//The UP portion is marked as stairs so pickups do not spawn on them
		trackType.IsStairs = true;
		trackType.CompoundPieceMiddleType = TrackPiece.PieceType.kTPUndulateUpMid;
		trackType.CompoundPieceEndType = TrackPiece.PieceType.kTPUndulateUpExit;
		trackType.CompoundPieceMinMiddleCount = 1;
		trackType.CompoundPieceMaxMiddleCount = 2;
		trackType.SelectionOdds = 0.7f;
		trackType.DifficultyLevel = 1;
		trackType.AddVariation ("oz_ww_cliffs_arch_flatup_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPUndulateUpMid;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceMiddle;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 1;
		//The UP portion is marked as stairs so pickups do not spawn on them
		trackType.IsStairs = true;
		trackType.AddVariation ("oz_ww_cliffs_arch_up_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPUndulateUpExit;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceEnd;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 1;
		//The UP portion is marked as stairs so pickups do not spawn on them
		trackType.IsStairs = true;
		trackType.AddVariation ("oz_ww_cliffs_arch_upflat_a");
		PieceTypes.Add(trackType.TrackType,trackType);												
																																				
		*/			
		
		//All Ramparts
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPECRamparts;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 1;
		trackType.SelectionOdds = 0f; //Must call this manually
		trackType.AllowInRegularRotation = false;
		trackType.AddVariation ("oz_ec_ramparts_straight_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestUndulateDownEnter);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestUndulateUpEnter);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPCurveLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPCurveRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPJumpOverSlightLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPJumpOverSlightRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPSlideUnderSlightLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPSlideUnderSlightRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPJumpOrSlide);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPJumpOrSlide);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPJumpOrSlide);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);	
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPEnemySnapDragon);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);			
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPTJunction);		
		PieceTypes.Add(trackType.TrackType,trackType);			
		
		//Down
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestUndulateDownEnter;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = .1f;
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.AddVariation ("oz_ec_ramparts_straight_down_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
				
		//Up
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestUndulateUpEnter;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = .1f;
		//The UP portion is marked as stairs so pickups do not spawn on them
		trackType.IsStairs = true;
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.AddVariation ("oz_ec_ramparts_straight_up_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		PieceTypes.Add(trackType.TrackType,trackType);		
		
		//Slights
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPCurveLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 1f;
		trackType.IsSlightLeft =true;
		trackType.AddVariation ("oz_ec_ramparts_slight_left_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPCurveRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 1f;
		trackType.IsSlightRight =true;
		trackType.AddVariation ("oz_ec_ramparts_slight_right_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPStraightFlat;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_ec_ramparts_straight_a");	
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPStraight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_ec_ramparts_straight_b");
		//trackType.AddVariation ("oz_ec_rampartsnarrows_straight_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		/*
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPJumpOverSlightLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		//trackType.IsObstacle = true;
		trackType.IsJumpOver = true;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 1f;
		trackType.IsSlightLeft =true;
		trackType.AddVariation ("oz_ec_ramparts_slight_left_over_a");
		PieceTypes.Add(trackType.TrackType,trackType);

		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPJumpOverSlightRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		//trackType.IsObstacle = true;
		trackType.IsJumpOver = true;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 1f;
		trackType.IsSlightRight =true;
		trackType.AddVariation ("oz_ec_ramparts_slight_right_over_anim_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		*/
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPTurnLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		trackType.IsTurnLeft = true;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("oz_ec_ramparts_hardleft_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPTJunction;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		trackType.IsTurnLeft = true;
		trackType.IsTurnRight = true;
		trackType.IsJunction = true;
		trackType.DifficultyLevel = 1;
		trackType.SelectionOdds = 1f;
		trackType.AfterJunctionPiece = TrackPiece.PieceType.kTPStraight;
		//trackType.PrePieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.AddVariation ("oz_ec_ramparts_junctionLR_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPEnvSetJunction;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		trackType.IsTurnLeft = true;
		trackType.IsTurnRight = true;
		trackType.IsJunction = true;
		trackType.DifficultyLevel = 1;
		trackType.AfterJunctionPiece = TrackPiece.PieceType.kTPStraight;
		trackType.AllowWhenFastTravelling = false;
		trackType.AddVariation ("oz_ec_ramparts_envset_junctionLR_a");
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPStraight);
		//trackType.DifficultyDistanceReduction = 1000;
		PieceTypes.Add(trackType.TrackType,trackType);	
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPTurnRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		trackType.IsTurnRight = true;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("oz_ec_ramparts_hardright_a");
		PieceTypes.Add(trackType.TrackType,trackType);	
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPSlideUnderSlightLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		//trackType.IsObstacle = true;
		trackType.IsSlideUnder = true;
		trackType.DifficultyLevel = 2;
		trackType.SelectionOdds = 1f;
		trackType.IsSlightLeft =true;
		trackType.AddVariation ("oz_ec_ramparts_slight_left_under_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPSlideUnderSlightRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		//trackType.IsObstacle = true;
		trackType.IsSlideUnder = true;
		trackType.DifficultyLevel = 2;
		trackType.SelectionOdds = 1f;
		trackType.IsSlightRight =true;
		trackType.AddVariation ("oz_ec_ramparts_slight_right_under_anim_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPJumpOrSlide;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		//trackType.IsObstacle = true;
		trackType.IsSlideUnder = true;
		trackType.IsJumpOver = true;
		trackType.DifficultyLevel = 1;
		trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_ec_ramparts_straight_overunder_a");
		trackType.AddVariation ("oz_ec_ramparts_straight_overunder_b");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPEnemySnapDragon;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		trackType.IsStumble = true;
		trackType.IsEnemy = true;
		trackType.IsJumpOver = true;
		trackType.IsAnimated = true;
		trackType.DifficultyLevel = 1;
		trackType.SelectionOdds = 1.5f; 
		trackType.AddVariation ("oz_ec_ramparts_straight_stumble_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPRampartsGaps;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		trackType.IsJumpOver = true;
		trackType.IsGap = true;
		trackType.DifficultyLevel = 3;
		//trackType.SelectionOdds = 1.5f; 
		trackType.AddVariation ("oz_ec_ramparts_straight_smallgap_anim_a");
		trackType.AddVariation ("oz_ec_ramparts_straight_biggap_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPRampartsGapSmall;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		trackType.IsJumpOver = true;
		trackType.IsGap = true;
		trackType.DifficultyLevel = 1;
		//trackType.SelectionOdds = 1.5f; 
		trackType.AddVariation ("oz_ec_ramparts_straight_smallgap_anim_a");
		PieceTypes.Add(trackType.TrackType,trackType);		
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPRampartsGapBig;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		trackType.IsJumpOver = true;
		trackType.IsGap = true;
		trackType.DifficultyLevel = 2;
		//trackType.SelectionOdds = 1.5f; 
		trackType.AddVariation ("oz_ec_ramparts_straight_biggap_a");
		PieceTypes.Add(trackType.TrackType,trackType);	
		
		
		#endregion
		
		///////////////////
		//CATACOMBS TILES//
		///////////////////
		#region Catacombs
		//Custom Pieces

		//Monkey Run
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAnimatedGauntlet2;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = 1f;
		trackType.IsGap = true;
		trackType.AddVariation ("oz_ec_catacombs_straight_over_b");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPCatacombsSwoopMonkey);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPCatacombsPounceMonkey);
		PieceTypes.Add(trackType.TrackType,trackType);		
		
		//Over Over Under
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPOverOverUnder;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = 1f;
		trackType.IsGap = true;
		trackType.AddVariation ("oz_ec_catacombs_straight_over_anim_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestSlideUnder);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Under to a gap... enough time?
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPUnderUnderOver;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 5;
		trackType.SelectionOdds = 1f;
		trackType.IsSlideUnder = true;
		trackType.IsAttackingBaboon = true;
		trackType.AddVariation ("oz_ec_catacombs_straight_under_anim_b");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestAnimatedJumpOrSlide);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestGaps);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Under Over Animated
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPUnderOverAnimated;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 5;
		trackType.SelectionOdds = 1.5f;
		trackType.IsSlideUnder = true;
		trackType.IsAttackingBaboon = true;
		trackType.AddVariation ("oz_ec_catacombs_straight_under_anim_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestJumpOver);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestAnimatedJumpOrSlide);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Over Under Animated
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPOverUnderAnimated;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = 1f;
		trackType.IsGap = true;
		trackType.IsJumpOver = true;
		trackType.AddVariation ("oz_ec_catacombs_straight_over_b");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestSlideUnder);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestAnimatedJumpOrSlide);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
		PieceTypes.Add(trackType.TrackType,trackType);
				
		//Gap, Gap
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestGapGap;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = 1.5f;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("oz_ec_catacombs_straight_b");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestGaps);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Gap, Gap.. GAP
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestGapGapGap;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 6;
		trackType.SelectionOdds = 3f;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("oz_ec_catacombs_straight_b");
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestSmallGap);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestSmallGap);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestSmallGap);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestGaps);
		PieceTypes.Add(trackType.TrackType,trackType);

		//Gap, Slide, Gap
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestGapSlideGap;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 6;
		trackType.SelectionOdds = 2f;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("oz_ec_catacombs_straight_b");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestAnimatedJumpOrSlide);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestGaps);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Gap, Falling tree
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGapAnimated;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 5;
		trackType.SelectionOdds = 1.5f;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("oz_ec_catacombs_straight_b");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestAnimatedJumpOrSlide);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestGaps);
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		PieceTypes.Add(trackType.TrackType,trackType);

		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAnimatedRightLedgeGap;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 3;
		trackType.IsAnimated = true;
		trackType.IsLedgeLeft = true;
		trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_ec_catacombs_straight_rightcollapse_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestGaps);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAnimatedLeftLedgeGap;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 3;
		trackType.IsAnimated = true;
		trackType.IsLedgeRight = true;
		trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_ec_catacombs_straight_leftcollapse_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestGaps);
		PieceTypes.Add(trackType.TrackType,trackType);		
		
//		//StairMaster
//		trackType = new TrackPieceTypeDefinition();
//		trackType.TrackType = TrackPiece.PieceType.kTPStairMaster;
//		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
//		trackType.EnvironmentSet = 4;
//		trackType.Environment = 1;
//		trackType.DifficultyLevel = 3;
//		trackType.SelectionOdds = .5f;
//		//trackType.IsTurnRight = true;
//		trackType.AddVariation ("oz_ec_catacombs_straight_up_a");
//		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestTurnRight);
//		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStairsUp);
//		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestGaps);
//		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStairsUp);
//		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestTurnLeft);
//		PieceTypes.Add(trackType.TrackType,trackType);
//		
//		//Descent
//		trackType = new TrackPieceTypeDefinition();
//		trackType.TrackType = TrackPiece.PieceType.kTPDescent;
//		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
//		trackType.EnvironmentSet = 4;
//		trackType.Environment = 1;
//		trackType.DifficultyLevel = 3;
//		trackType.SelectionOdds = .5f;
//		trackType.AddVariation ("oz_ec_catacombs_straight_down_a");
//		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestJumpOver);
//		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStairsDown);
//		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestSlideUnder);
//		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStairsDown);
//		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
//		PieceTypes.Add(trackType.TrackType,trackType);
		
		//End Custom Segments
			
		/* TODO find equivalent TODO
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestSlideUnderSlightLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 1;
		//trackType.IsObstacle = true;
		trackType.IsSlideUnder = true;
		trackType.DifficultyLevel = 2;
		trackType.SelectionOdds = 1f;
		trackType.IsSlightLeft =true;
		trackType.AddVariation ("oz_ww_woods_slightleft_under_a");
		PieceTypes.Add(trackType.TrackType,trackType);
	
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestSlideUnderSlightRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 1;
		//trackType.IsObstacle = true;
		trackType.IsSlideUnder = true;
		trackType.DifficultyLevel = 2;
		trackType.SelectionOdds = 1f;
		trackType.IsSlightRight =true;
		trackType.AddVariation ("oz_ww_woods_slightright_under_a");
		PieceTypes.Add(trackType.TrackType,trackType);

		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestSlightLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 1f;
		trackType.IsSlightLeft =true;
		trackType.AddVariation ("oz_ww_woods_slightleft_a");
		PieceTypes.Add(trackType.TrackType,trackType);
	
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestSlightRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 1f;
		trackType.IsSlightRight =true;
		trackType.AddVariation ("oz_ww_woods_slightright_a");
		PieceTypes.Add(trackType.TrackType,trackType);		

		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestEnemySnapDragon;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 1;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 1.5f;
		trackType.IsStumble = true;
		trackType.IsEnemy = true;
		trackType.IsJumpOver = true;
		trackType.IsAnimated = true;
		trackType.AddVariation ("oz_ww_woods_shortstraight_b");
		PieceTypes.Add(trackType.TrackType,trackType);																							
																																																						
		*/
		
		//All Catacombs
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPECCatacombs;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 1;
		trackType.SelectionOdds = 0f; //Must call this manually
		trackType.AllowInRegularRotation = false;
		trackType.AddVariation ("oz_ec_catacombs_straight_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);	
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestTurnLeft);	
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestSlideUnder);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestSlideUnder);	
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestSlideUnder);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestJumpOver);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestGaps);	
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestAnimatedJumpOrSlide);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestAnimatedJumpOrSlide);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestAnimatedJumpOrSlide);	
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStairsUp);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);	
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStairsDown);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);			
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestJunction);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPAnimatedLeftLedge);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPAnimatedRightLedge);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestEntrance;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionStart;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 0;
		trackType.PrePieces.Add (TrackPiece.PieceType.kTPStraight);	
		trackType.PrePieces.Add (TrackPiece.PieceType.kTPStraightFlat);
		//trackType.PrePieces.Add (TrackPiece.PieceType.kTPStraight);
		trackType.AddVariation ("oz_ec_catacombs_transition_entrance_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestExit;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionEnd;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 0;
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
		trackType.AddVariation ("oz_ec_catacombs_transition_exit_a");		
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		PieceTypes.Add(trackType.TrackType,trackType);

		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestJunction;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 1;
		trackType.IsTurnRight = true;
		trackType.IsTurnLeft = true;
		trackType.IsJunction = true;
		trackType.DifficultyLevel = 0;
		trackType.AfterJunctionPiece = TrackPiece.PieceType.kTPForestStraight;
		trackType.AddVariation ("oz_ec_catacombs_junctionLR_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestTurnRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 1;
		trackType.IsTurnRight = true;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("oz_ec_catacombs_hardright_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestTurnLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 1;
		trackType.IsTurnLeft = true;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("oz_ec_catacombs_hardleft_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestSlideUnder;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 1;
		//trackType.IsObstacle = true;
		trackType.IsSlideUnder = true;
		trackType.IsAttackingBaboon = true;
		trackType.DifficultyLevel = 2;
		trackType.SelectionOdds = 1f;
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
		trackType.AddVariation ("oz_ec_catacombs_straight_under_anim_a");
		trackType.AddVariation ("oz_ec_catacombs_straight_under_anim_b");
		//trackType.AddVariation ("oz_ec_catacombs_straight_under_anim_c");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Swooping monkey
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPCatacombsSwoopMonkey;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 1;
		//trackType.IsObstacle = true;
		trackType.IsSlideUnder = true;
		trackType.IsAttackingBaboon = true;
		trackType.DifficultyLevel = 1;
		trackType.SelectionOdds = 1f; //Must call manually
		trackType.AllowInRegularRotation = false;
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
		trackType.AddVariation ("oz_ec_catacombs_straight_under_anim_a");
		PieceTypes.Add(trackType.TrackType,trackType);		
		
		//Drop down monkey
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPCatacombsPounceMonkey;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 1;
		//trackType.IsObstacle = true;
		trackType.IsSlideUnder = true;
		trackType.IsAttackingBaboon = true;
		trackType.DifficultyLevel = 1;
		trackType.SelectionOdds = 1f; //Must call manually
		trackType.AllowInRegularRotation = false;
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
		trackType.AddVariation ("oz_ec_catacombs_straight_under_anim_b");
		PieceTypes.Add(trackType.TrackType,trackType);			
	
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestJumpOver;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 1;
		//trackType.IsObstacle = true;
		trackType.IsJumpOver = true;
		trackType.DifficultyLevel = 1;
		trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_ec_catacombs_straight_over_anim_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestGaps;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 1;
		//trackType.IsObstacle = true;
		trackType.IsGap = true;
		trackType.IsJumpOver = true;
		trackType.DifficultyLevel = 2;
		trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_ec_catacombs_straight_over_b");
		PieceTypes.Add(trackType.TrackType,trackType);
			
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestAnimatedJumpOrSlide;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 1;
		//trackType.IsObstacle = true;
		trackType.IsJumpOver = true;
		trackType.IsSlideUnder = true;
		trackType.IsAnimated = true;
		trackType.DifficultyLevel = 1;
		trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_ec_catacombs_straight_overunder_a");
		trackType.AddVariation ("oz_ec_catacombs_straight_overunder_b");
		PieceTypes.Add(trackType.TrackType,trackType);
	
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestStraight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_ec_catacombs_straight_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Forest Short Track
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestStraightShort;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_ec_catacombs_straight_b");
		PieceTypes.Add(trackType.TrackType,trackType);	
				
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAnimatedLeftLedge;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 2;
		trackType.IsAnimated = true;
		trackType.IsLedgeRight = true;
		trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_ec_catacombs_straight_leftcollapse_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAnimatedRightLedge;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 2;
		trackType.IsAnimated = true;
		trackType.IsLedgeLeft = true;
		trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_ec_catacombs_straight_rightcollapse_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//Stairs
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPStairsUp;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = .05f;
		trackType.IsStairs = true;
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
		trackType.AddVariation ("oz_ec_catacombs_straight_up_a");
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
		PieceTypes.Add(trackType.TrackType,trackType);		
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPStairsDown;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = .05f;
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
		trackType.AddVariation ("oz_ec_catacombs_straight_down_a");
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraightShort);
		PieceTypes.Add(trackType.TrackType,trackType);			
		
		#endregion
		
		////////////////////////
		//RAMPART NARROW TILES//
		////////////////////////
		#region Rampart Narrows
		
		//Gap Gap
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGapGapNarrow;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 2;
		//trackType.IsObstacle = true;
		trackType.IsGap = true;
		trackType.IsJumpOver = true;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = 1.5f;
		trackType.AddVariation ("oz_ec_narrows_biggap_anim_a");
		trackType.AddVariation ("oz_ec_narrows_smallgap_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPGaps);
		PieceTypes.Add(trackType.TrackType,trackType);	
		
		//Gap Gap Gap
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGapGapGapNarrow;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 2;
		//trackType.IsObstacle = true;
		trackType.IsGap = true;
		trackType.IsJumpOver = true;
		trackType.DifficultyLevel = 6;
		trackType.SelectionOdds = 2f;
		trackType.AddVariation ("oz_ec_narrows_biggap_anim_a");
		trackType.AddVariation ("oz_ec_narrows_smallgap_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPGaps);
		PieceTypes.Add(trackType.TrackType,trackType);	
		
		//Over Under Gap
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPOverUnderAnimatedNarrow;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 2;
		//trackType.IsObstacle = true;
		trackType.IsGap = true;
		trackType.IsJumpOver = true;
		trackType.DifficultyLevel = 5;
		trackType.SelectionOdds = 1.5f;
		trackType.AddVariation ("oz_ec_narrows_straight_over_anim_a");
		trackType.AddVariation ("oz_ec_narrows_straight_over_anim_b");
		trackType.AddVariation ("oz_ec_narrows_straight_over_c");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPSlideUnder);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPGaps);
		PieceTypes.Add(trackType.TrackType,trackType);		

		//OverUnder Gap Under
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPUnderOverAnimatedNarrow;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 2;
		//trackType.IsObstacle = true;
		trackType.IsGap = true;
		trackType.IsJumpOver = true;
		trackType.DifficultyLevel = 5;
		trackType.SelectionOdds = 1.5f;
		trackType.AddVariation ("oz_ec_narrows_straight_overunder_a");
		trackType.AddVariation ("oz_ec_narrows_straight_overunder_b");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPSlideUnder);
		PieceTypes.Add(trackType.TrackType,trackType);			
		
/*		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPNarrowsLedgesLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 0;
		trackType.IsNarrow = true;
		trackType.AddVariation ("oz_ec_narrows_backtonarrow_a");
		PieceTypes.Add(trackType.TrackType,trackType);			
		
		
		//Custom Fast Turn Sections
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPNarrowFastTurnLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 3;
		trackType.SelectionOdds = 1f;
		trackType.IsTurnLeft = true;
		trackType.IsNarrow = true;
		//trackType.PrePieces.Add (TrackPiece.PieceType.kTPNarrowsEnemySnapDragon);
		trackType.AddVariation ("oz_ec_narrows_hardleft_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPNarrowFastTurnLeftAlt;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = 1f;
		trackType.IsTurnLeft = true;
		trackType.IsNarrow = true;
		trackType.AddVariation ("oz_ec_narrows_hardleft_a");	
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPNarrowFastTurnRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 3;
		trackType.SelectionOdds = 1f;
		trackType.IsTurnRight = true;
		trackType.IsNarrow = true;
		//trackType.PrePieces.Add (TrackPiece.PieceType.kTPNarrowsEnemySnapDragon);
		trackType.AddVariation ("oz_ec_narrows_hardright_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPNarrowFastTurnRightAlt;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 4;
		trackType.SelectionOdds = 1f;
		trackType.IsTurnRight = true;
		trackType.IsNarrow = true;
		trackType.AddVariation ("oz_ec_narrows_hardright_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		PieceTypes.Add(trackType.TrackType,trackType);			
*/
		
		//All Narrows
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPECNarrows;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 1;
		trackType.SelectionOdds = 0f; //Must call this manually
		trackType.AllowInRegularRotation = false;
		trackType.AddVariation ("oz_ec_narrows_straight_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);	
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsTurnRight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);	
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsTurnLeft);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsEnemySnapDragon);	
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPJumpOver);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);	
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPJumpOver);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);	
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPJumpOver);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);	
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPSlideUnder);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);	
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPSlideUnder);		
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);	
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPSlideUnder);		
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);	
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);	
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPGaps);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);	
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPLedgeLeftStart);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPLedgeLeftMiddle);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPLedgeLeftMiddle);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPLedgeLeftMiddle);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPLedgeLeftEnd);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);	
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPLedgeRightStart);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPLedgeRightMiddle);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPLedgeRightMiddle);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPLedgeRightMiddle);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPLedgeRightEnd);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);	
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPAnimatedJumpOrSlide);		
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);	
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPAnimatedJumpOrSlide);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);	
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPAnimatedJumpOrSlide);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);	
		//trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsFromTower);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsJunction);	
		PieceTypes.Add(trackType.TrackType,trackType);
		
		//End custom segments
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPNarrowsEntrance;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionStart;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 1;
		trackType.IsNarrow = true;	
		trackType.IsCoinBlock = true;
		//trackType.IsJumpOver = true;
		trackType.PrePieces.Add (TrackPiece.PieceType.kTPStraight);	
		trackType.PrePieces.Add (TrackPiece.PieceType.kTPStraightFlat);
		trackType.AddVariation ("oz_ec_narrows_transition_entrance_b");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		//trackType.PrePieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPNarrowsExit;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionEnd;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 0;
		trackType.IsNarrow = true;
		trackType.PrePieces.Add (TrackPiece.PieceType.kTPNarrowsStraight);	
		trackType.PrePieces.Add (TrackPiece.PieceType.kTPNarrowsStraight);		
		trackType.AddVariation ("oz_ec_narrows_transition_exit_a");
		trackType.PostPieces.Add (TrackPiece.PieceType.kTPStraight);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPNarrowsJunction;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 2;
		trackType.IsTurnRight = true;
		trackType.IsTurnLeft = true;
		trackType.IsJunction = true;
		trackType.DifficultyLevel = 0;
		trackType.AfterJunctionPiece = TrackPiece.PieceType.kTPNarrowsStraight;
		trackType.IsNarrow = true;
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.AddVariation ("oz_ec_narrows_junctionLR_a");
		PieceTypes.Add(trackType.TrackType,trackType);		
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPNarrowsTurnRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 2;
		trackType.IsTurnRight = true;
		trackType.DifficultyLevel = 0;
		trackType.IsNarrow = true;
		trackType.AddVariation ("oz_ec_narrows_hardright_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPNarrowsTurnLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 2;
		trackType.IsTurnLeft = true;
		trackType.DifficultyLevel = 0;
		trackType.IsNarrow = true;
		trackType.AddVariation ("oz_ec_narrows_hardleft_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPNarrowsStraight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 0;
		trackType.IsNarrow = true;
		trackType.AddVariation ("oz_ec_narrows_straight_a");
		trackType.AddVariation ("oz_ec_narrows_straight_b");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPNarrowsEnemySnapDragon;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 2;
		trackType.DifficultyLevel = 0;
		trackType.IsNarrow = true;
		trackType.IsStumble = true;
		trackType.IsEnemy = true;
		trackType.IsJumpOver = true;
		trackType.IsAnimated = true;
		trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_ec_narrows_straight_stumble_anim_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPJumpOver;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 2;
		//trackType.IsObstacle = true;
		trackType.IsJumpOver = true;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_ec_narrows_straight_over_anim_a");
		trackType.AddVariation ("oz_ec_narrows_straight_over_anim_b");
		trackType.AddVariation ("oz_ec_narrows_straight_over_c");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPSlideUnder;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 2;
		//trackType.IsObstacle = true;
		trackType.IsSlideUnder = true;
		trackType.DifficultyLevel = 2;
		trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_ec_narrows_straight_under_anim_a");
		trackType.AddVariation ("oz_ec_narrows_straight_under_b");
		PieceTypes.Add(trackType.TrackType,trackType);		

		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPGaps;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 2;
		//trackType.IsObstacle = true;
		trackType.IsGap = true;
		trackType.IsJumpOver = true;
		trackType.DifficultyLevel = 2;
		trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_ec_narrows_biggap_anim_a");
		trackType.AddVariation ("oz_ec_narrows_smallgap_a");
		PieceTypes.Add(trackType.TrackType,trackType);

		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPLedgeLeftStart;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceStart;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 2;
		trackType.CompoundPieceMiddleType = TrackPiece.PieceType.kTPLedgeLeftMiddle;
		trackType.CompoundPieceEndType = TrackPiece.PieceType.kTPLedgeLeftEnd;
		trackType.CompoundPieceMinMiddleCount = 3;
		trackType.CompoundPieceMaxMiddleCount = 6;
		//trackType.IsObstacle = true;
		trackType.IsLedgeLeft = true;
		trackType.DifficultyLevel = 4;
		trackType.AddVariation ("oz_ec_narrows_leftledge_start_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPLedgeLeftMiddle;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceMiddle;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 2;
		//trackType.IsObstacle = true;
		trackType.IsLedgeLeft = true;
		trackType.DifficultyLevel = 2;
		trackType.AddVariation ("oz_ec_narrows_leftledge_middle_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPLedgeLeftEnd;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceEnd;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 2;
		//trackType.IsObstacle = true;
		trackType.IsCoinBlock = true;
		trackType.IsLedgeLeft = true;
		trackType.DifficultyLevel = 2;
		trackType.AddVariation ("oz_ec_narrows_leftledge_end_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPLedgeRightStart;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceStart;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 2;
		trackType.CompoundPieceMiddleType = TrackPiece.PieceType.kTPLedgeRightMiddle;
		trackType.CompoundPieceEndType = TrackPiece.PieceType.kTPLedgeRightEnd;
		trackType.CompoundPieceMinMiddleCount = 3;
		trackType.CompoundPieceMaxMiddleCount = 6;
		//trackType.IsObstacle = true;
		trackType.IsLedgeRight = true;
		trackType.DifficultyLevel = 4;
		trackType.AddVariation ("oz_ec_narrows_rightledge_start_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPLedgeRightMiddle;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceMiddle;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 2;
		//trackType.IsObstacle = true;
		trackType.IsLedgeRight = true;
		trackType.DifficultyLevel = 2;
		trackType.AddVariation ("oz_ec_narrows_rightledge_middle_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPLedgeRightEnd;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceEnd;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 2;
		//trackType.IsObstacle = true;
		trackType.IsLedgeRight = true;
		trackType.IsCoinBlock = true;
		trackType.DifficultyLevel = 2;
		trackType.AddVariation ("oz_ec_narrows_rightledge_end_a");
		PieceTypes.Add(trackType.TrackType,trackType);			

		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAnimatedJumpOrSlide;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 2;
		//trackType.IsObstacle = true;
		trackType.IsSlideUnder = true;
		trackType.IsJumpOver = true;
		trackType.IsAnimated = true;
		trackType.DifficultyLevel = 1;
		trackType.SelectionOdds = 1f;
		trackType.AddVariation ("oz_ec_narrows_straight_overunder_a");
		trackType.AddVariation ("oz_ec_narrows_straight_overunder_b");
		PieceTypes.Add(trackType.TrackType,trackType);		
		
		
		//  Narrrows End  //
		#endregion			

		//All Transitions
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPECSetChange;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 1;
		trackType.SelectionOdds = 0f; //Must call this manually
		trackType.AllowInRegularRotation = false;
		trackType.AddVariation ("oz_ec_ramparts_straight_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);		
		
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestEntrance);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);	
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestExit);
		
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsEntrance);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPNarrowsExit);			
		PieceTypes.Add(trackType.TrackType,trackType);		
		
		//Balloon Pieces		
		if (Settings.GetBool("balloon-enabled", true))
		{
			trackType = new TrackPieceTypeDefinition();
			trackType.TrackType = TrackPiece.PieceType.kTPBalloonJunction;
			trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
			trackType.EnvironmentSet = 4;
			trackType.Environment = 0;
			trackType.IsTurnLeft = true;
			trackType.IsTurnRight = true;
			trackType.IsJunction = true;
			trackType.IsBalloonJunction = true;
			trackType.AfterJunctionPiece = TrackPiece.PieceType.kTPStraightFlat;
			trackType.AlternateAfterJunctionPiece = TrackPiece.PieceType.kTPPreBalloon;
			trackType.PrePieces.Add(TrackPiece.PieceType.kTPStraight);
			trackType.DifficultyLevel = 2;
			trackType.SelectionOdds = Settings.GetFloat("balloon-odds-ww", 8f);
			trackType.AllowWhenFastTravelling = false;
			trackType.AddVariation ("oz_ec_ramparts_balloon_junctionLR_a");
			PieceTypes.Add(trackType.TrackType,trackType);
		}		

		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPBalloonFall;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionEnd;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 9;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 0f;	//Only spawn this directly
		//trackType.IsBalloon = true;
		trackType.AllowInRegularRotation = false;
		trackType.AddVariation ("oz_ec_ramparts_balloon_fail_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPPreBalloon;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionEnd;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		//trackType.SelectionOdds = 0f;
		trackType.AllowInRegularRotation = false;
		//trackType.IsBalloon = true;
		trackType.AddVariation ("oz_ec_ramparts_preballoon_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPBalloonEntrance);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPBalloonStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPBalloonStraightShort);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPBalloonStraightShort);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		

		// Pieces that lead into and lead out of tunnel transition
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPBeforeTunnelTransitionEntrance;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.AllowInRegularRotation = false;
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.AddVariation ("oz_ec_tt_exit_ramparts_a");
		PieceTypes.Add(trackType.TrackType,trackType);	
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPAfterTunnelTransitionExit;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 4;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.AllowInRegularRotation = false;
		trackType.AddVariation ("oz_ec_tt_entrance_ramparts_a");
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		PieceTypes.Add(trackType.TrackType,trackType);
		
	}
	
	
	private void PopulateMachuPieceTypes()
	{
		TrackPieceTypeDefinition trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPStraightFlat;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("straight_a");
		trackType.AddVariation ("straight_b");
		trackType.AddVariation ("straight_d");
		trackType.AddVariation ("straight_organic_c");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPStraightFlatIntro;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.AllowInRegularRotation = false;
		trackType.AddVariation ("straight_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPStraight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("straight_organic_a");
		trackType.AddVariation ("straight_organic_b");
		trackType.AddVariation ("hill_a");
		trackType.AddVariation ("hill_b");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPCurveLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 0;
		trackType.IsCurve = true;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("curve_a");
		trackType.AddVariation ("curve_c");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPCurveRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 0;
		trackType.IsCurve = true;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("curve_b");
		trackType.AddVariation ("curve_d");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPBridge;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 0;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 0.1f;
		trackType.AddVariation ("bridge_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPJumpOver;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 0;
		trackType.IsJumpOver = true;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("jump_over_a");
		trackType.AddVariation ("gap_small_a");
		trackType.AddVariation ("bridge_gap_a");
		trackType.AddVariation ("curve_water_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPJumpOverLong;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 0;
		trackType.IsJumpOver = true;
		trackType.DifficultyLevel = 2;
		trackType.AddVariation ("gap_large_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPSlideUnder;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 0;
		trackType.IsSlideUnder = true;
		trackType.DifficultyLevel = 1;
		trackType.SelectionOdds = 0.75f;
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.AddVariation ("slide_under_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPJumpOrSlide;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 0;
		trackType.IsJumpOver = true;
		trackType.IsSlideUnder = true;
		trackType.DifficultyLevel = 2;
		trackType.AddVariation ("jump_or_slide_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPStumble;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 0;
		trackType.IsStumble = true;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("stumble_a");
		trackType.AddVariation ("stumble_b");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPTurnLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 0;
		trackType.IsTurnLeft = true;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("turn_left_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPTurnRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 0;
		trackType.IsTurnRight = true;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("turn_right_a");
		PieceTypes.Add(trackType.TrackType,trackType);	
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPTJunction;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 0;
		trackType.IsTurnLeft = true;
		trackType.IsTurnRight = true;
		trackType.IsJunction = true;
		trackType.AfterJunctionPiece = TrackPiece.PieceType.kTPStraightFlat;
		trackType.DifficultyLevel = 1;
		trackType.SelectionOdds = 0.75f;
		trackType.AddVariation ("junction_a");
		trackType.AddVariation ("junction_b");
		PieceTypes.Add(trackType.TrackType,trackType);	
	
		// env set junction piece, so we can go back to whimsywoods
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPEnvSetJunction;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 0;
		trackType.IsTurnLeft = true;
		trackType.IsTurnRight = true;
		trackType.IsJunction = true;
		trackType.AfterJunctionPiece = TrackPiece.PieceType.kTPStraightFlat;
		trackType.DifficultyLevel = 1;
		trackType.AllowWhenFastTravelling = false;
		trackType.AddVariation ("envset_junction_a");
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPFieldsStraightFlat);
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPFieldsStraight);
		PieceTypes.Add(trackType.TrackType,trackType);

		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPUpStairsStart;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceStart;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 0;
		trackType.CompoundPieceMiddleType = TrackPiece.PieceType.kTPUpStairsMiddle;
		trackType.CompoundPieceEndType = TrackPiece.PieceType.kTPUpStairsEnd;
		trackType.CompoundPieceMinMiddleCount = 1;
		trackType.CompoundPieceMaxMiddleCount = 3;
		trackType.IsStairs = true;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 0.3f;
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.AddVariation ("stairs_up_start_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPUpStairsMiddle;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceMiddle;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 0;
		trackType.IsStairs = true;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("stairs_up_middle_a");
		trackType.AddVariation ("stairs_up_middle_b");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPUpStairsEnd;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceEnd;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 0;
		trackType.IsStairs = true;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("stairs_up_end_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPZipLine;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 0;
		trackType.IsZipLine = true;
		trackType.DifficultyLevel = 3;
		trackType.SelectionOdds = 0.5f;
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.AddVariation ("zipline_a");
		trackType.AddVariation ("zipline_b");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPLedgeLeftStart;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceStart;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 0;
		trackType.CompoundPieceMiddleType = TrackPiece.PieceType.kTPLedgeLeftMiddle;
		trackType.CompoundPieceEndType = TrackPiece.PieceType.kTPLedgeLeftEnd;
		trackType.CompoundPieceMinMiddleCount = 4;
		trackType.CompoundPieceMaxMiddleCount = 10;
		trackType.IsLedgeLeft = true;
		trackType.DifficultyLevel = 3;
		trackType.AddVariation ("ledge_start_left_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPLedgeLeftMiddle;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceMiddle;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 0;
		trackType.IsLedgeLeft = true;
		trackType.DifficultyLevel = 3;
		trackType.AddVariation ("ledge_left_a");
		trackType.AddVariation ("ledge_left_b");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPLedgeLeftEnd;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceEnd;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 0;
		trackType.IsLedgeLeft = true;
		trackType.DifficultyLevel = 3;
		trackType.AddVariation ("ledge_end_left_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPLedgeRightStart;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceStart;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 0;
		trackType.CompoundPieceMiddleType = TrackPiece.PieceType.kTPLedgeRightMiddle;
		trackType.CompoundPieceEndType = TrackPiece.PieceType.kTPLedgeRightEnd;
		trackType.CompoundPieceMinMiddleCount = 4;
		trackType.CompoundPieceMaxMiddleCount = 10;
		trackType.IsLedgeRight = true;
		trackType.DifficultyLevel = 3;
		trackType.AddVariation ("ledge_start_right_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPLedgeRightMiddle;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceMiddle;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 0;
		trackType.IsLedgeRight = true;
		trackType.DifficultyLevel = 3;
		trackType.AddVariation ("ledge_right_a");
		trackType.AddVariation ("ledge_right_b");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPLedgeRightEnd;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceEnd;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 0;
		trackType.IsLedgeRight = true;
		trackType.DifficultyLevel = 3;
		trackType.AddVariation ("ledge_end_right_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPMineEntrance;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionStart;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 2;
		trackType.IsMine = true;
		trackType.DifficultyLevel = 3;
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.AddVariation ("mine_entrance_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPMineStraight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 2;
		trackType.IsMine = true;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("mine_straight_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPMineExit;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionEnd;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 2;
		trackType.IsMine = true;
		trackType.DifficultyLevel = 0;
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPMineStraight);
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPMineStraight);
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPMineStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPStraightFlat);
		trackType.AddVariation ("mine_exit_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPMineCurve;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 2;
		trackType.IsCurve = true;
		trackType.IsMine = true;
		trackType.DifficultyLevel = 0;
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPMineStraight);
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPMineStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPMineStraight);
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPMineStraight);
		trackType.SelectionOdds = 0.2f;
		trackType.AddVariation ("mine_curve_a");
		//trackType.AddVariation ("mine_curve_b");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPMineCurveLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 2;
		trackType.IsCurve = true;
		trackType.IsMine = true;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 0.4f;
		trackType.AddVariation ("mine_curve_d");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPMineCurveRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 2;
		trackType.IsCurve = true;
		trackType.IsMine = true;
		trackType.DifficultyLevel = 0;
		trackType.SelectionOdds = 0.4f;
		trackType.AddVariation ("mine_curve_c");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPMineJunction;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 2;
		trackType.IsTurnLeft = true;
		trackType.IsTurnRight = true;
		trackType.IsJunction = true;
		trackType.AfterJunctionPiece = TrackPiece.PieceType.kTPMineStraight;
		trackType.DifficultyLevel = 1;
		trackType.SelectionOdds = 0.75f;
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPMineStraight);
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPMineStraight);
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPMineStraight);
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPMineStraight);
		trackType.SelectionOdds = 0.4f;
		trackType.AddVariation ("mine_junction_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPMineSlideUnder;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 2;
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPMineStraight);
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPMineStraight);
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPMineStraight);
		trackType.IsMine = true;
		trackType.DifficultyLevel = 2;
		trackType.SelectionOdds = 0.75f;
		trackType.IsSlideUnder = true;
		trackType.AddVariation ("mine_duck_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPMineLedgeLeftStart;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceStart;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 2;
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPMineStraight);
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPMineStraight);
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPMineStraight);
		trackType.CompoundPieceMiddleType = TrackPiece.PieceType.kTPMineLedgeLeftMiddle;
		trackType.CompoundPieceEndType = TrackPiece.PieceType.kTPMineLedgeLeftEnd;
		trackType.CompoundPieceMinMiddleCount = 4;
		trackType.CompoundPieceMaxMiddleCount = 12;
		trackType.IsLedgeLeft = true;
		trackType.IsMine = true;
		trackType.DifficultyLevel = 3;
		trackType.AddVariation ("mine_ledge_left_start_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPMineLedgeLeftMiddle;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceMiddle;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 2;
		trackType.IsLedgeLeft = true;
		trackType.IsMine = true;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("mine_ledge_left_a", 0.65f);
		trackType.AddVariation ("mine_ledge_left_b", 0.45f);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPMineLedgeLeftEnd;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceEnd;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 2;
		trackType.IsLedgeLeft = true;
		trackType.IsMine = true;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("mine_ledge_left_end_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPMineLedgeRightStart;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceStart;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 2;
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPMineStraight);
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPMineStraight);
		trackType.PrePieces.Add(TrackPiece.PieceType.kTPMineStraight);
		trackType.CompoundPieceMiddleType = TrackPiece.PieceType.kTPMineLedgeRightMiddle;
		trackType.CompoundPieceEndType = TrackPiece.PieceType.kTPMineLedgeRightEnd;
		trackType.CompoundPieceMinMiddleCount = 4;
		trackType.CompoundPieceMaxMiddleCount = 12;
		trackType.IsLedgeRight = true;
		trackType.IsMine = true;
		trackType.DifficultyLevel = 3;
		trackType.AddVariation ("mine_ledge_right_start_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPMineLedgeRightMiddle;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceMiddle;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 2;
		trackType.IsLedgeRight = true;
		trackType.IsMine = true;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("mine_ledge_right_a", 0.65f);
		trackType.AddVariation ("mine_ledge_right_b", 0.35f);
		//trackType.AddVariation ("mine_ledge_right_c", 0.1f);
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPMineLedgeRightEnd;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceEnd;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 2;
		trackType.IsLedgeRight = true;
		trackType.IsMine = true;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("mine_ledge_right_end_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestEntrance;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionStart;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("forest_entrance_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestExit;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionEnd;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("forest_exit_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestStraight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 1;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("forest_straight_a");
		trackType.AddVariation ("forest_straight_b");
		trackType.AddVariation ("forest_hill_a");
		
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestTurnLeft;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 1;
		trackType.IsTurnLeft = true;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("forest_turn_left_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestTurnRight;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 1;
		trackType.IsTurnRight = true;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("forest_turn_right_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestJunction;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 1;
		trackType.IsTurnLeft = true;
		trackType.IsTurnRight = true;
		trackType.IsJunction = true;
		trackType.AfterJunctionPiece = TrackPiece.PieceType.kTPForestStraight;
		trackType.DifficultyLevel = 1;
		trackType.SelectionOdds = 0.75f;
		trackType.AddVariation ("forest_junction_a");
		trackType.AddVariation ("forest_junction_b");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestSlideUnder;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 1;
		trackType.PostPieces.Add(TrackPiece.PieceType.kTPForestStraight);
		trackType.IsSlideUnder = true;
		trackType.DifficultyLevel = 2;
		trackType.AddVariation ("forest_slide_under_a");
		PieceTypes.Add(trackType.TrackType,trackType);
		
		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestJumpOver;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 1;
		trackType.IsJumpOver = true;
		trackType.DifficultyLevel = 1;
		trackType.AddVariation ("forest_jump_over_a");
		trackType.AddVariation ("forest_river_a");
		PieceTypes.Add(trackType.TrackType,trackType);

		trackType = new TrackPieceTypeDefinition();
		trackType.TrackType = TrackPiece.PieceType.kTPForestStumble;
		trackType.TrackCategory = TrackPieceTypeCategory.kTrackPieceCategoryNormal;
		trackType.EnvironmentSet = 0;
		trackType.Environment = 1;
		trackType.IsStumble = true;
		trackType.DifficultyLevel = 0;
		trackType.AddVariation ("forest_stumble_a");
		trackType.AddVariation ("forest_stumble_b");
		PieceTypes.Add(trackType.TrackType,trackType);
		
	}
	
	// Use this for initialization
	void Start () 
	{
		SharedInstance = this;
	}
	
	public TrackPieceTypeDefinition GetTypesFromTrackType(TrackPiece.PieceType trackType)
	{
		// we must use a dictionary because that's what gets modified as we are in the tunnel transition
		if (PieceTypes.ContainsKey(trackType)) {
			return PieceTypes[trackType];
		}
		return null;
		//return pieceTypesLookupArray[(int)trackType];
	}
	
	public List<TrackPiece.PieceType> GetPieceTypesKeys()
	{
		return new List<TrackPiece.PieceType>(PieceTypes.Keys);
	}
	
	// Delete everything in the PieceTypes Dictionary that matches an entry in the list
	public void DeletePieceTypes( List<TrackPiece.PieceType> deletedKeys)
	{
		foreach(  TrackPiece.PieceType onePiece in deletedKeys)
		{
			if (PieceTypes.ContainsKey( onePiece))
			{
				PieceTypes.Remove(onePiece);	
			}
			
		}	
		
		// Create scratch pad list of available piece types.  Max Size is the total number of TrackPieceTypes
		if (AvailablePieceTypes != null)
		{
			AvailablePieceTypes = null;
		}
		
		AvailablePieceTypes = new TrackPieceTypeDefinition[PieceTypes.Count];		
		
		System.Array ptvalues = System.Enum.GetValues(typeof(TrackPiece.PieceType));
		pieceTypesLookupArray = new List<TrackPieceTypeDefinition>(ptvalues.Length);
		for(int i=0; i<ptvalues.Length; i++) {
			pieceTypesLookupArray.Add (null);
		}
		//-- turn on/off the bitaray so we don't have to call .Contains on the PieceTypes array.
		foreach (var item in PieceTypes.Keys) {
			// TODO rebuild this when we switch environment set
			pieceTypesLookupArray[(int)item] = PieceTypes[item];	
		}		
			
	}

	
//	public string GetPrefabNameFromTrackType(TrackPiece.PieceType trackType)
//	{
//		TrackPieceTypeDefinition types = GetTypesFromTrackType(trackType);
//		return GetPrefabNameFromTrackTypeDef(types);
//	}
	
	public static string GetPrefabNameFromTrackTypeDef(TrackPieceTypeDefinition types)
	{
		if (types == null) {
			return null;	
		}
		
		// If there are no variations this is bad
		if (types.Variations.Count == 0) {
			return null;	
		}
	
		//-- Choose between the list of choices per track type.			
		int choice = 0;
		string choiceString = "";
		int prevChoice = types.LastChosenVariation;
		choice = types.GetVariation(prevChoice, ref choiceString);
			
		// Update the last chosen variation
		types.LastChosenVariation = choice;
		return choiceString;
	}
	
	public TrackPiece.PieceType ChooseNextTrackPiece(TrackPiece previousTrackPiece, List<TrackPiece.PieceType> queueList, int turn)
	{
		TrackPiece.PieceType prevTrackType = previousTrackPiece.TrackType;
		TrackPiece.PieceType newTrackType = TrackPiece.PieceType.kTPStraightFlat;
		
		//-- Pull a piece from the queue if there are any.
		if(queueList != null && queueList.Count > 0)
		{
			newTrackType = queueList[0];
			queueList.RemoveAt(0);
			
			//-- Record History.
			PieceHistory.Add (newTrackType);
		//	notify.Debug ("CHOSEN!!! "+newTrackType);
			return newTrackType;
		}
//#if UNITY_EDITOR		
		else if(DebugTrackSegment != null && DebugTrackSegment.Pieces != null && DebugTrackSegment.Pieces.Count > 0)
		{
			// If we are using a demo track and we've reached the end of the demo track this will re-add the demo track to the list so it loops.
			if(queueList != null)
			{
				queueList.AddRange(DebugTrackSegment.Pieces);
				if(queueList.Count > 0)
				{
					newTrackType = queueList[0];
					queueList.RemoveAt(0);
					
					//-- Record History.
					PieceHistory.Add (newTrackType);
		//	notify.Debug ("CHOSEN!!! "+newTrackType);
					return newTrackType;
				}	
			}
		}
//#endif
		
		// Get the TrackPieceType for the Previous piece
		TrackPieceTypeDefinition previousPieceType = previousTrackPiece.trackPieceDefinition;
		
		//Adjusted for Track Overlap
		// If there is nothing in the queue, choose a piece at random from the list of available piece
		bool doAllowTurns = AllowTurns;
		bool doAllowLeftTurns = true;
		bool doAllowRightTurns = true;
		bool doAllowObstacles = AllowObstacles;
		bool doAllowSlideUnder = true;
		bool doAllowEnvironmentStart = false;
		bool doAllowEnvironmentEnd = false;
		
		if(turn > 2)
		{
			doAllowLeftTurns = false;
		}
//		else
//		if(turn == 1)
//		{
//			doAllowLeftTurns = (Random.Range(0,2)==1);
//		}
		else if(turn < -2)
		{
			doAllowRightTurns = false;
		}
//		else
//		if(turn == -1)
//		{
//			doAllowRightTurns = (Random.Range(0,2)==1);
//		}
		
		//Debug.Log("Turn Value in Track Builder: " + turn);
		
		// Don't allow onstacles right after a turn
		if(previousTrackPiece.DistanceSinceLastTurn < MinDistanceAfterTurnForObstacle)
		{
			doAllowObstacles = false;
		}
		
		if (previousTrackPiece.DistanceSinceLastTurn < MinDistanceBetweenTurns)
		{
			doAllowTurns = false;
		}
		
		if(previousTrackPiece.BackToBackObstacleCount >= MaxBackToBackObstacles)
		{
			doAllowObstacles = false;
		}
		

		if(	(previousTrackPiece.DistanceSinceLastObstacle < MinDistanceBetweenObstacles) &&
			(Random.Range(1,1000) > (1000 * DoubleObstaclePercent))
		  )
		{
			doAllowObstacles = false;
		}
		
		if(IsTurnType(previousPieceType) == true)
		{
			doAllowObstacles = false;
		}
		
		if(IsObstacleType(previousPieceType) == true)
		{
			if(AllowTurnAfterObstacle == false)
			{
				doAllowTurns = false;
			}
		}
		
		if(IsSlightType(previousPieceType) == true && CurrentEnvironmentSetId != 2
			&& previousTrackPiece.DistanceSinceLastTurn < MaxDistanceBetweenTurns)
		{
			doAllowTurns = false;	
		}
		
		// Don't allow obstacles during fast turn sections
//		if (IsFastTurnSection) {
//			doAllowObstacles = false;
//		//	notify.Debug ("Fast Turn is ACTIVE!!!");
//		}
		
		if (previousPieceType.Environment == 0)
		{
			doAllowEnvironmentStart = previousTrackPiece.DistanceSinceLastEnvironmentChange > EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.Env0MinLength;
		//	notify.Debug("Env0?");
		}
	
		// if we are close to the fast travel distance, don't allow new environments 
		// note that since we know fast travel has no envset junctions,  using DistanceSinceLastEnvsetJunction
		// is a proxy for DistanceSinceTheBeginning
		bool withinTunnelEntranceBufferDistance = false;
		
		if ( GamePlayer.SharedInstance.HasFastTravel)
		{
			withinTunnelEntranceBufferDistance = true;//EnvironmentSetSwitcher.SharedInstance.WantNewEnvironmentSet &&
						//previousTrackPiece.DistanceSinceLastEnvsetJunction > GamePlayer.SharedInstance.FastTravelDistance - EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.TunnelBufferDistance;
		}
		else if (previousTrackPiece.LeadingToTransitionTunnel)
		{
			withinTunnelEntranceBufferDistance = EnvironmentSetSwitcher.SharedInstance.WantNewEnvironmentSet &&
						previousTrackPiece.DistanceSinceLastEnvsetJunction > EnvironmentSetSwitcher.SharedInstance.DistanceToTunnel - EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.TunnelBufferDistance;	
		
			notify.Debug ("withinTunnelBuffer is: " + withinTunnelEntranceBufferDistance);
		}
		
			// TODO RED refactor to switch
		if (previousPieceType.Environment == 0 &&
			!IsFastTurnSection &&
			previousTrackPiece.DistanceSinceLastEnvironmentChange > EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.Env0MinLength &&
			! withinTunnelEntranceBufferDistance)
		{
			doAllowEnvironmentStart = true;
			notify.Debug("Env0?");
		}
		
		// similarly if the previous track pieces says it leads to the transition tunnel, don't allow starting a new environment
//		if  (previousTrackPiece.LeadingToTransitionTunnel)
//		{
//			doAllowEnvironmentStart = false;
//		}
		
		if (previousPieceType.Environment == 1 && 
			previousTrackPiece.DistanceSinceLastEnvironmentChange > EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.Env1MinLength)
		{
			doAllowEnvironmentEnd = true;
		}
		
		if (previousPieceType.Environment == 2 && 
			previousTrackPiece.DistanceSinceLastEnvironmentChange > EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.Env2MinLength)
		{
			doAllowEnvironmentEnd = true;
		}
	
//		// Don't allow slide under obstacles immediately following jump over or stumble obstacles
		//Phil - not needed anymore
//		if ((IsJumpOverType(prevTrackType) || IsStumbleType(prevTrackType)) && doAllowObstacles) {
//			doAllowSlideUnder = false;	
//		}
		
		if (previousPieceType.Environment == 9 && 
			previousTrackPiece.DistanceSinceLastEnvironmentChange > EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.BalloonMinLength)
		{
			notify.Debug("Env9");
			notify.Debug(previousTrackPiece.DistanceSinceLastEnvironmentChange);
			doAllowEnvironmentEnd = true;
		}
		
//		if(GamePlayer.SharedInstance.HasSuperBoost) {
//			doAllowEnvironmentStart = false;
//			doAllowEnvironmentEnd = false;
//		}
		
		
		// In certain cases we want to force a turn or obstacle
		// *NOTE* Don't try to force more than scenario at a time or badness might happen.
		bool forceEnvSetJunction = false;
		bool forceTurn = false;
		bool forceObstacle = false;
		bool forceEnvironmentSetChange = false;
		bool forceEnvironmentEnd = false;
		
		bool needsEnvironmentChangeForFastTravel = false;
		
		if (EnvironmentSetSwitcher.SharedInstance!= null 
			&& EnvironmentSetSwitcher.SharedInstance.WantNewEnvironmentSet 
			&& !GamePlayer.SharedInstance.HasSuperBoost)
		{
			if (EnvironmentSetSwitcher.IsInactive())
			{
				if (previousTrackPiece.DistanceSinceLastEnvsetJunction >= EnvironmentSetSwitcher.SharedInstance.DistanceToTunnel)
				{
					// we can only allow a env set transition if his env is zero
					if(previousPieceType.Environment == 0) {
						if(previousTrackPiece.DistanceSinceLastEnvironmentChange > GameProfile.SharedInstance.DistInEnvZeroBeforeSetChange)
							forceEnvironmentSetChange = true;
					} else { 
						doAllowEnvironmentEnd = true;
						needsEnvironmentChangeForFastTravel = true;
					}
				} else if(GamePlayer.SharedInstance.HasFastTravel) { 
					doAllowEnvironmentEnd = true;
					needsEnvironmentChangeForFastTravel = true;
				}
			}
			notify.Debug ("forceEnvironmentSetChange= " + forceEnvironmentSetChange);
		}
		
		//notify.Debug("Env End: "+doAllowEnvironmentEnd);
		//notify.Debug("Env Str: "+doAllowEnvironmentStart);
		// do not force a turn or obstacle if we are in the middle of an environment set transition
		if (EnvironmentSetSwitcher.IsInactive())
		{
			if (previousPieceType.Environment == 0 && !doAllowEnvironmentStart) {
				if (previousTrackPiece.DistanceSinceLastTurn >= MaxDistanceBetweenTurns && doAllowTurns)
				{
					forceTurn = true;
				}
				
				if (previousTrackPiece.DistanceSinceLastObstacle >= MaxDistanceBetweenObstacles && doAllowObstacles)
				{
					forceObstacle = true;
				}
				
				if (previousTrackPiece.DistanceSinceLastEnvsetJunction >= MinDistanceBetweenEnvSetJunctions && 
					previousTrackPiece.DistanceSinceLastTurn > MinDistanceBetweenTurns &&
					! GamePlayer.SharedInstance.HasFastTravel &&
					! GamePlayer.SharedInstance.HasSuperBoost &&
					! previousTrackPiece.LeadingToTransitionTunnel &&
					EnvironmentSetManager.SharedInstance.LocallyAvailableCount() > 1 &&
					Settings.GetBool("envset-junction", true) )
				{
					// envset junction trumps forceTurn and forceObstacle
			//		notify.Debug (Time.frameCount + " " + previousTrackPiece.DistanceSinceLastEnvsetJunction + " forcing forceEnvSetJunction");
					forceEnvSetJunction = true;	
					forceTurn =false;
					forceObstacle = false;
					doAllowTurns = true;
					doAllowRightTurns = true;
					doAllowLeftTurns = true;
				}
				else
				{
			//		notify.Debug (Time.frameCount + " " + previousTrackPiece.DistanceSinceLastEnvsetJunction + " forceEnvSetJunction=false " +
			//			(! GamePlayer.SharedInstance.HasFastTravel) + (! previousTrackPiece.LeadingToTransitionTunnel));
				}
	
				// Can't force both so randomly pick one.
				if (forceTurn && forceObstacle) {
					if (Random.Range(0, 2) == 0) {
						forceTurn = false;	
					} else {
						forceObstacle = false;
					}
				}
			}
			//	notify.Debug("Force Obst: "+forceObstacle);
			//	notify.Debug("Force Turn: "+forceTurn);
			//Debug.Log ("Current ENV: " + CurrentEnvironmentSetId);
			
			
			if (queueList.Count == 0) {
				//Make sure we are NOT IN THE CLOUDS!!! TODO: WE NEED A BETTER WAY TO FIX THIS THAN HARD CODING!!!
				if(AllowTurns == true && AllowObstacles == true && previousPieceType.Environment!=9 && GamePlayer.SharedInstance.OnTrackPiece != null){	
					if(Random.value < TrackSegmentChance){
						//	notify.Debug ("queue up segment " + TrackSegmentChance);
						TrackPiece currPiece = GamePlayer.SharedInstance.OnTrackPiece;
						TrackPieceTypeDefinition def = TrackBuilder.SharedInstance.GetTypesFromTrackType(currPiece.TrackType);
						//notify.Debug ("curr environment set = " + CurrentEnvironmentSetId);
						//notify.Debug ("curr environment " + def.Environment);
						TrackSegment.QueueSegmentWithEnvInfo(CurrentEnvironmentSetId, def.Environment, MaxTrackPieceDifficulty, queueList);
						//notify.Debug ("Queuecount = " + queueList.Count);
						if(queueList.Count > 0)
						{
							newTrackType = queueList[0];
							queueList.RemoveAt (0);
							
							PieceHistory.Add (newTrackType);
					//		notify.Debug("CHOSEN: "+newTrackType);
							return newTrackType;
						}
					}
				}
			}
		}
		
		// check if we have to force environment end
		if ( doAllowEnvironmentEnd && ! forceEnvironmentSetChange && ! forceEnvSetJunction && ! forceObstacle && ! forceTurn)
		{
			if (previousPieceType.Environment == 0 && 
				previousTrackPiece.DistanceSinceLastEnvironmentChange > EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.Env0MaxLength)
			{
				forceEnvironmentEnd = true;	
			}
			else if (previousPieceType.Environment == 1 && 
				previousTrackPiece.DistanceSinceLastEnvironmentChange > EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.Env1MaxLength)
			{
				forceEnvironmentEnd = true;	
			}
			else if ( previousPieceType.Environment == 2 && 
				previousTrackPiece.DistanceSinceLastEnvironmentChange > EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.Env2MaxLength)
			{
				forceEnvironmentEnd = true;	
			}
			else if(previousPieceType.Environment!=0 &&
				needsEnvironmentChangeForFastTravel &&
				previousPieceType.TrackCategory != TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionEnd)
			{
				forceEnvironmentEnd = true;
			}
		}
		
		/*if(forceEnvironmentEnd || forceEnvironmentSetChange || forceTurn || forceEnvSetJunction)
		{
			Debug.Log(previousTrackPiece.gameObject.name + " " + forceEnvironmentEnd + " " + forceEnvironmentSetChange + " " + forceTurn + " " +forceEnvSetJunction);
		}*/
		// Build the list of available pieces based on the criteria selected above.
		int availablePieceCount = 0;
//		int max = pieceTypesLookupArray.Count;
//		for (int i = 0; i < max-1; i++) {
//			TrackPieceTypeDefinition piece = pieceTypesLookupArray[i];
//			if(piece.TrackType == TrackPiece.PieceType.kTPStraightFlatIntro)
//				continue;
//		}
		//	notify.Debug (prevTrackType,previousTrackPiece.gameObject);
			

		float additiveChance = 0f;
		
		TrackPieceTypeDefinition piece;
		
		if (previousTrackPiece.TrackType == TrackPiece.PieceType.kTPCemetarySlight)
		{
			notify.Debug("cem slight");	
		}
		foreach (KeyValuePair<TrackPiece.PieceType, TrackPieceTypeDefinition> pair in PieceTypes)
		{	
			piece = pair.Value;
			
			if(piece.AllowInRegularRotation == false) 
			{
				continue; 
			}
			
			if (piece.AllowWhenFastTravelling  == false && (GamePlayer.SharedInstance.HasFastTravel || GamePlayer.SharedInstance.HasSuperBoost))
			{
				continue;	
			}
			
			// Only allow piece selection from the current environment set
			if (previousPieceType.EnvironmentSet != piece.EnvironmentSet) {
				continue;	
			}
			
			//Don't allow junctions after envswitch junction
			if (previousTrackPiece.LeadingToTransitionTunnel && piece.IsJunction)
			{
				continue;
			}
			
			if(!piece.IsBalloonJunction 
				&& piece.TrackType != TrackPiece.PieceType.kTPEnvSetJunction 
				&& piece.IsJunction 
				&& TrackBuilder.SharedInstance.ActiveJunctions >= TrackBuilder.SharedInstance.MaxJunctions)
			{
				continue;
			}
			//Phil - Don't spawn tunnel entrance until you reach the minimum distance to tunnel
//			if(previousTrackPiece.LeadingToTransitionTunnel && 
//				previousTrackPiece.DistanceSinceLastEnvsetJunction < EnvironmentSetSwitcher.SharedInstance.distanceToTunnel && 
//				piece.IsTransitionTunnel)
//			{
//				//notify.Debug("Don't Spawn Tunnel Entrance");
//				//forceEnvironmentSetChange = false;
//				continue;	
//			}
			
			// Only allow piece selection from the current environment
			if (previousPieceType.Environment != piece.Environment)
			{	
				// *NOTE* The use of continue in these checks bails out of any more checks for this piece since we know we 
				// don't want to add it and don't want to perform any more checks.
				
				// Only allow environment start change pieces if they are currently allowed
				
				if ((piece.TrackCategory == TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionStart && doAllowEnvironmentStart) ||
					(previousPieceType.TrackCategory == TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionEnd && piece.Environment == 0))
				{
					// Allow this piece
				} else {
					continue;	
				}
			}
			else {
				// Don't allow another of the same environment after the end piece for that environement
				if (previousPieceType.TrackCategory == TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionEnd) {
					continue;
				}
			}
			
			//Make sure we are not spawning another balloon junction too early. or if we are boosting
			//if(previousTrackPiece.DistanceSinceLastBalloon < 1000f && piece.TrackType == TrackPiece.PieceType.kTPBalloonJunction)
			if((previousTrackPiece.DistanceSinceLastBalloon < MinDistanceBetweenBalloons || GamePlayer.SharedInstance.HasBoost) && piece.TrackType == TrackPiece.PieceType.kTPBalloonJunction) 
			{
				continue;
			}
			
			// Only allow environment change pieces if they are currently allowed
			if ((!doAllowEnvironmentStart && piece.TrackCategory == TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionStart)
				|| (!doAllowEnvironmentEnd && piece.TrackCategory == TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionEnd))
			{
				continue;
			}
			
			// if we are forcing environment end, only allow that piece
			if (forceEnvironmentEnd && piece.TrackCategory != TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionEnd)
			{
				continue;	
			}
				
			// Don't allow compound pieces that are in the middle or end (these must always be placed by the start first)
			if (piece.TrackCategory == TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceMiddle 
				|| piece.TrackCategory == TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceEnd)
			{	
				continue;
			}
			
			// Only allow obstacles that are of the correct diffulty or lower
			if (piece.DifficultyLevel > MaxTrackPieceDifficulty)
			{
				continue;
			}
			
			// Only allow piece if it meels selection odds criteria
			/*if (piece.SelectionOdds < 1.0f && Random.Range(0,1000) >= (1000 * piece.SelectionOdds))
			{
				continue;
			}*/
			
			// Only allow turn piece if they are currently allowed

			if (!doAllowTurns && IsTurnType(piece))
			{
				continue;
			}
			
			if ((!doAllowLeftTurns) && (IsTurnLeftType(piece) || IsSlightLeftType(piece)))
			{
				continue;
			}
			
			if ((!doAllowRightTurns) && (IsTurnRightType(piece) || IsSlightRightType(piece)))
			{
				continue;
			}
			
			if(doAllowRightTurns && previousTrackPiece.lastTurnType == 1 && piece.IsTurnLeft && !piece.IsJunction) //If the last turn type was a left, the next one can't be a left
			{
				//Debug.Log ("Last Turn was Left - This piece is: " + piece);
				continue;	
			}

			if(doAllowLeftTurns && previousTrackPiece.lastTurnType == -1 && piece.IsTurnRight && !piece.IsJunction) //If the last turn type was a right, the next one can't be a right
			{
				//Debug.Log ("Last Turn was Right - This piece is: " + piece);
				continue;	
			}
			
//			// REMOVE THIS
//			if(IsTurnType(piece))
//			{
//				string wasTurn = "";
//				
//				if(lastTurnType == 1)
//					wasTurn = "Left";
//				
//				else if(lastTurnType == -1)
//					wasTurn = "Right";
//				
//				else
//					wasTurn = "None";
//					
//				Debug.Log ("Last Turn was " + wasTurn + " : This Piece is " + piece.TrackType);	
//			}
			
			// Only allow obstacles piece if they are currently allowed
			if ((!doAllowObstacles) && IsObstacleType(piece))
			{
				continue;
			}
			
			// Only allow slide under obstacles if they are currently allowed
			if (!doAllowSlideUnder && IsSlideUnderType(piece)) {
				continue;	
			}
					
			// if we are forcing an environment set transition don't allow anything else
			if (forceEnvironmentSetChange && !GamePlayer.SharedInstance.HasSuperBoost)	//Super boost is sort of a hack...
			{
				if (piece.TrackCategory != TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentSetTransitionStart)
				{
					continue;
				}
				else
				{
					// we've added the tunnel entrance
					notify.Debug("choosing piece = " + piece.TrackType);	
				}
			}
			else
			{
				// we don't want it env set transition otherwise	
				if (piece.TrackCategory == TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentSetTransitionStart)
				{
					continue;
				}
			}
			
			if ( ! forceEnvironmentSetChange)
			{
				// If we are forcing this piece to be a turn don't allow other pieces
				if (doAllowTurns && forceTurn && !IsTurnType(piece))
				{
					continue;
				}
				
				// If we are forcing this piece to be an obstacle don't allow other pieces
				if (doAllowObstacles && forceObstacle && !IsObstacleType(piece))
				{
					continue;
				}
				
				
				if ( forceEnvSetJunction && doAllowTurns && doAllowLeftTurns && doAllowRightTurns && (piece.TrackType != TrackPiece.PieceType.kTPEnvSetJunction))
				{
					//Debug.Log ("Throw it all out");
					continue;	
				}
				else 
					if(forceEnvSetJunction && (piece.TrackType == TrackPiece.PieceType.kTPEnvSetJunction))	
				{
		//			notify.Debug("Cannot force Set Switch: " + Time.frameCount);	
				}
			}
			
			if (piece.TrackType == TrackPiece.PieceType.kTPEnvSetJunction )
			{
				if ( ! forceEnvSetJunction )
				{
					// we don't want another envset junction, when he still hasn't reached the new one
					//notify.Debug("forceEnvSetJunction is false, skipping kTPEnvSetJunction");
					continue;	
				}
				
				if (Settings.GetBool("envset-junction", true) == false)
				{
					// we don't even want this piece to be placed
					continue;
				}
			}
			
			//TODO why is this getting through
			if (piece.TrackCategory == TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentSetTransitionStart)
			{
				if ( ! forceEnvironmentSetChange)
				{
					notify.Error("this should not happen, GET REDMOND");
				}
			}
			
			// If we made it this far add the piece to the available piece list
			AvailablePieceTypes[availablePieceCount] = piece;
			availablePieceCount++;
			additiveChance += piece.SelectionOdds;
		}
		
		if(availablePieceCount==0)
		{
			//Debug.LogWarning("No available pieces!");
			notify.Warning("Zero availablePieceCount {0} doAllowTurns={1} doAllowObstacles={2} doAllowSlideUnder={3} doAllowEnvironmentStart={4} doAllowEnvironmentend={5} " +
				"forceEnvSetJunction = {6} forceTurn = {7} forceObstacle = {8} forceEnvironmentSetChange = {9} forceEnvironmentEnd = {10} DiffultyLevel={11}" ,
				previousTrackPiece.gameObject.name, doAllowTurns, doAllowObstacles, doAllowSlideUnder, doAllowEnvironmentStart, doAllowEnvironmentEnd,
				forceEnvSetJunction, forceTurn, forceObstacle, forceEnvironmentSetChange, forceEnvironmentEnd, MaxTrackPieceDifficulty);

		}
		
		// Choose a random piece from the list of allowed pieces
		int pieceChoice = 0;// = Random.Range(0, availablePieceCount);
		//Take into account the SelectionOdds
		float floatChoice = Random.Range(0f,additiveChance);
		for(int i=0;i<availablePieceCount;i++)
		{
			float odds = AvailablePieceTypes[i].SelectionOdds;
			if(floatChoice < odds)
			{
				pieceChoice = i;
				break;
			}
			else
			{
				floatChoice -= odds;
			}
		}
		
		//if (AvailablePieceTypes[pieceChoice] == null)
//		bool done = false;
//		while (!done)
//		{
			string pieces = "";
#if UNITY_EDITOR
			pieces += previousTrackPiece;
			for (int i =0; i < availablePieceCount; i++)
			{
				pieces += AvailablePieceTypes[i].TrackType + " " + AvailablePieceTypes[i].SelectionOdds + " ";
			}
			
#endif
			//if(availablePieceCount==0)
				notify.Debug("pieceChoice = " + pieceChoice + " availablePieceCount= " + availablePieceCount + " " + pieces);
		    // BRYANT we need this,  A TrackPiece might have multiple variations so it will still looks different even if it's the same as the previous track piece
			if (availablePieceCount  > 1)
			{
				if ( AvailablePieceTypes[pieceChoice].TrackType == prevTrackType)
				{
					//TODO fixme Red later
					TrackPieceTypeDefinition last = AvailablePieceTypes[availablePieceCount-1];
					if (last.TrackType == prevTrackType)
					{
						availablePieceCount -= 1;	
					}
					else
					{
						AvailablePieceTypes[pieceChoice] = last;
						availablePieceCount -= 1;
					}
					pieceChoice = Random.Range(0, availablePieceCount);
					notify.Debug ("A duplicate piece was available");
				}
			}
		
		if(availablePieceCount==0)
		{
			newTrackType = prevTrackType;	
		}
		else {
			newTrackType = AvailablePieceTypes[pieceChoice].TrackType;
		}
		
			//notify.Debug ("CHOSEN!!! "+newTrackType);
		//-- Record History.
		PieceHistory.Add (newTrackType);
		
		notify.Debug("CHOSEN: "+newTrackType );
		return newTrackType;
	}
	
//	public bool IsTurnType(TrackPiece.PieceType type)
//	{
//		return IsTurnType(GetTypesFromTrackType(type));
//	}
	
	public static bool IsTurnType(TrackPieceTypeDefinition def)
	{
		if (def == null) {
			return false;
		}
		
		return (def.IsTurnLeft || def.IsTurnRight || def.IsJunction);
	}

	
//	public bool IsTurnLeftType(TrackPiece.PieceType type)
//	{
//		return IsTurnLeftType(GetTypesFromTrackType(type));
//	}
	
	public static bool IsTurnLeftType(TrackPieceTypeDefinition def)
	{
		if (def == null) {
			return false;
		}
		
		return (def.IsTurnLeft || def.IsJunction);
	}
	
//	public bool IsTurnRightType(TrackPiece.PieceType type)
//	{
//		return IsTurnRightType(GetTypesFromTrackType(type));
//	}
	
	public static bool IsTurnRightType(TrackPieceTypeDefinition def)
	{
		if (def == null) {
			return false;
		}
		
		return (def.IsTurnRight || def.IsJunction);
	}
		
	public static bool IsSlightType(TrackPieceTypeDefinition def)
	{
		if (def == null)
			{
				return false;	
			}
		
		return (def.IsSlightLeft || def.IsSlightRight);
	}
		
//	public bool IsSlightRightType(TrackPiece.PieceType type)
//	{
//		return IsSlightRightType(GetTypesFromTrackType(type));
//	}
	
	public static bool IsSlightRightType(TrackPieceTypeDefinition def)
	{
		if (def == null) {
			return false;
		}
		
		return def.IsSlightRight;
	}
	
//	public bool IsSlightLeftType(TrackPiece.PieceType type)
//	{
//		return IsSlightLeftType(GetTypesFromTrackType(type));
//	}
	
	public static bool IsSlightLeftType(TrackPieceTypeDefinition def)
	{
		if (def == null) {
			return false;
		}
		
		return def.IsSlightLeft;
	}
	
	
//	public bool IsJunctionType(TrackPiece.PieceType type)
//	{
//		return IsJunctionType(GetTypesFromTrackType(type));
//	}
	
	public static bool IsJunctionType(TrackPieceTypeDefinition def)
	{
		if (def == null) {
			return false;
		}
		
		return def.IsJunction;
	}
	
//	public bool IsObstacleType(TrackPiece.PieceType type)
//	{
//		return IsObstacleType(GetTypesFromTrackType(type));
//	}
	
	public static bool IsObstacleType(TrackPieceTypeDefinition def)
	{
		if (def == null) {
			return false;
		}
		
		return (def.IsJumpOver || def.IsSlideUnder || def.IsStumble || def.IsLedgeLeft || def.IsLedgeRight 
			|| def.IsZipLine || def.IsEnemy || def.IsAnimated || def.IsGap || def.IsAttackingBaboon);
	}
	
//	public bool IsJumpOverType(TrackPiece.PieceType type)
//	{
//		return IsJumpOverType(GetTypesFromTrackType(type));
//	}
	
	public static bool IsJumpOverType(TrackPieceTypeDefinition def)
	{
		if (def == null) {
			return false;
		}
		
		return def.IsJumpOver;
	}

	//Attacking Baboon
//	public bool IsAttackingBaboonType(TrackPiece.PieceType type)
//	{
//		return IsAttackingBaboonType(GetTypesFromTrackType(type));
//	}
	
	public static bool IsAttackingBaboonType(TrackPieceTypeDefinition def)
	{
		if (def == null) {
			return false;
		}
		
		return def.IsAttackingBaboon;
	}
	
	
	//Gaps
//	public bool IsGapType(TrackPiece.PieceType type)
//	{
//		return IsGapType(GetTypesFromTrackType(type));
//	}
	
	public static bool IsGapType(TrackPieceTypeDefinition def)
	{
		if (def == null) {
			return false;
		}
		
		return def.IsGap;
	}
	
//	public bool IsSlideUnderType(TrackPiece.PieceType type)
//	{
//		return IsSlideUnderType(GetTypesFromTrackType(type));
//	}
	
	public static bool IsSlideUnderType(TrackPieceTypeDefinition def)
	{
		if (def == null) {
			return false;
		}
		
		return def.IsSlideUnder;
	}

//	public bool IsTransitionTunnelType(TrackPiece.PieceType type)
//	{
//		return IsTransitionTunnelType(GetTypesFromTrackType(type));
//	}
	
	public static bool IsTransitionTunnelType(TrackPieceTypeDefinition def)
	{
		if (def == null) {
			return false;
		}
		
		return def.IsTransitionTunnel;
	}	
	
//	public bool IsStumbleType(TrackPiece.PieceType type)
//	{
//		return IsStumbleType(GetTypesFromTrackType(type));
//	}
	
	public static bool IsStumbleType(TrackPieceTypeDefinition def)
	{
		if (def == null) {
			return false;
		}
		
		return def.IsStumble;
	}
	
//	public bool IsLedgeType(TrackPiece.PieceType type)
//	{
//		return IsLedgeType(GetTypesFromTrackType(type));
//	}
	
	public static bool IsLedgeType(TrackPieceTypeDefinition def)
	{
		if (def == null) {
			return false;
		}
		
		return (def.IsLedgeLeft || def.IsLedgeRight);
	}
	
//	public bool IsNarrowType(TrackPiece.PieceType type)
//	{
//		return IsNarrowType(GetTypesFromTrackType(type));
//	}
	
	public static bool IsNarrowType(TrackPieceTypeDefinition def)
	{
		if (def == null) {
			return false;
		}
		
		return def.IsNarrow;
	}
	
//	public bool IsAnyLeftLedgeType(TrackPiece.PieceType type)
//	{
//		return IsAnyLeftLedgeType(GetTypesFromTrackType(type));
//	}
	
	public static bool IsAnyLeftLedgeType(TrackPieceTypeDefinition def)
	{
		if (def == null) {
			return false;
		}
		
		return def.IsLedgeLeft;
	}
	
//	public bool IsAnyRightLedgeType(TrackPiece.PieceType type)
//	{
//		return IsAnyRightLedgeType(GetTypesFromTrackType(type));
//	}
	
	public static bool IsAnyRightLedgeType(TrackPieceTypeDefinition def)
	{
		if (def == null) {
			return false;
		}
		
		return def.IsLedgeRight;
	}
	
//	public bool IsCurveType(TrackPiece.PieceType type)
//	{
//		return IsCurveType(GetTypesFromTrackType(type));
//	}
	
	public static bool IsCurveType(TrackPieceTypeDefinition def)
	{
		if (def == null) {
			return false;
		}
		
		return def.IsCurve;
	}
	
//	public bool IsMineType(TrackPiece.PieceType type)
//	{
//		return IsMineType(GetTypesFromTrackType(type));
//	}
	
	public static bool IsMineType(TrackPieceTypeDefinition def)
	{
		if (def == null) {
			return false;
		}
		
		return def.IsMine;
	}
	
//	public bool IsStairType(TrackPiece.PieceType type)
//	{
//		return IsStairType(GetTypesFromTrackType(type));
//	}
	
	public static bool IsStairType(TrackPieceTypeDefinition def)
	{
		if (def == null) {
			return false;
		}
		
		return def.IsStairs;
	}
	
//	public bool IsAnimatedType(TrackPiece.PieceType type)
//	{
//		return IsAnimatedType(GetTypesFromTrackType(type));
//	}
	
	public static bool IsAnimatedType(TrackPieceTypeDefinition def)
	{
		if (def == null) { 
			return false;
		}
		return def.IsAnimated;
	}

//	public bool IsEnemyType(TrackPiece.PieceType type)
//	{
//		return IsEnemyType(GetTypesFromTrackType(type));
//	}

	public static bool IsEnemyType(TrackPieceTypeDefinition def)
	{
		if (def == null) {
			return false;
		}
		
		return def.IsEnemy;
	}

	public static bool IsBalloonType(TrackPieceTypeDefinition def )
	{
		if(def == null) {
			return false;
		}
		
		return def.IsBalloon;
	}
	
	public static bool IsBalloonJunctionType(TrackPieceTypeDefinition def)
	{
		if(def == null) {
			return false;
		}
		
		return def.IsBalloonJunction;
	}
	
	public static bool IsCloudsJunctionType(TrackPieceTypeDefinition def)
	{
		if(def == null) {
			return false;
		}
		
		return def.IsCloudsJunction;
	}
	
	/*public int lastTurnType
	{
		get { return LastTurnType; }
		set { LastTurnType = value; }
	}*/
}
