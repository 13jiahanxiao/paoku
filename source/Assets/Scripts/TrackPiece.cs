using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

public class TrackPiece : DynamicElement
{	
	protected static Notify notify;
	public enum PieceType
	{
		kTPStraight 	 	= 0,
		kTPECRamparts		,
		kTPECNarrows		,
		kTPECCatacombs		,
		kTPECSetChange		,
		kTPCurveLeft	 	,
		kTPCurveRight		,
		kTPTurnLeft 		,
		kTPZipLine    	 	,	
		kTPStraightFlat		,
		kTPStraightFlatIntro,
		kTPTurnRight 		,
		kTPUpStairsStart	,
		kTPBridge			,
		kTPTJunction		,
		kTPJumpOver			,
		kTPJumpOverLong		,
		kTPSlideUnder		,
		kTPJumpOrSlide		,
		kTPStumble			,
		kTPLedgeLeftStart	,
		kTPLedgeRightStart	,
		kTPUpStairsMiddle	,
		kTPUpStairsEnd		,
		kTPLedgeLeftMiddle	,
		kTPLedgeLeftEnd		,
		kTPLedgeRightMiddle	,
		kTPLedgeRightEnd	,
		kTPMineEntrance		,
		kTPMineStraight		,
		kTPMineCurve		,
		kTPMineCurveLeft	,
		kTPMineCurveRight	,
		kTPMineJunction		,
		kTPMineSlideUnder	,
		kTPMineExit			,
		kTPMineLedgeLeftStart	,
		kTPMineLedgeRightStart	,
		kTPMineLedgeLeftMiddle	,
		kTPMineLedgeLeftEnd		,
		kTPMineLedgeRightMiddle	,
		kTPMineLedgeRightEnd	,
		kTPForestEntrance		,
		kTPForestStraight		,
		kTPForestTurnLeft		,
		kTPForestTurnRight		,
		kTPForestSlideUnder 	,
		kTPForestStumble		,
		kTPForestJunction		,
		kTPForestJumpOver		,
		kTPEnemyMonkey			,
		kTPEnemySnapDragon		,
		kTPForestEnemySnapDragon		,
		kTPNarrowsEnemySnapDragon		,
		kTPForestExit			,
		kTPEnvSetJunction , // Junction piece that says "left turn for yellow brick road"
	
		//Oz Tile Set WW//
		
		//Slide Mechanic Test
		
		//Added by Spencer
		kTPNarrowsEntrance		,
		kTPNarrowsStraight		,
		kTPNarrowsTurnLeft		,
		kTPNarrowsTurnRight		,
		kTPNarrowsJunction		,
		kTPNarrowsExit			,
		kTPForestAnimatedJumpOrSlide	,
		kTPAnimatedLeftLedge	,
		kTPAnimatedRightLedge	,
		kTPAnimatedLeftLedgeGap	,
		kTPAnimatedRightLedgeGap	,		
		kTPAnimatedJumpOrSlide	,
		kTPBalloonJunction		,
		kTPBalloonEntrance		,
		kTPBalloonExit			,
		kTPBalloonFall			,
		kTPBalloonStraight		,
		kTPBalloonStraightShort	,
		kTPPreBalloon			,
		kTPUndulateUpEnter		,
		kTPUndulateUpMid		,
		kTPUndulateUpExit		,
		kTPUndulateDownEnter	,
		kTPUndulateDownMid		,
		kTPUndulateDownExit		,
		kTPForestStraightShort	,
		kTPForestUndulateUpEnter,
		kTPForestUndulateDownEnter	,
		kTPForestGaps			,
		kTPForestSmallGap		,
		kTPForestLargeGap		,
		kTPForestSlideUnderSlightLeft,
		kTPForestSlideUnderSlightRight,
		kTPForestJumpOverSlightLeft,
		kTPForestJumpOverSlightRight,
		kTPForestSlightLeft,
		kTPForestSlightRight,
		kTPGaps,
		kTPSlideUnderSlightLeft,
		kTPSlideUnderSlightRight,
		kTPJumpOverSlightLeft,
		kTPJumpOverSlightRight,
		
		//Custom Segments
		kTPFastTurnLeft			,
		kTPFastTurnRight		,
		kTPFastTurnLeftAlt		,
		kTPFastTurnRightAlt		,
		kTPFastTurnLeftAltAlt	,
		kTPFastTurnRightAltAlt	,
		kTPNarrowFastTurnLeft	,
		kTPNarrowFastTurnRight	,
		kTPNarrowFastTurnLeftAlt,
		kTPNarrowFastTurnRightAlt,	
		kTPAnimatedGauntlet1	,
		kTPAnimatedGauntlet2	,
		kTPAnimatedGauntlet3	,
		kTPAnimatedGauntlet4	,
		kTPAnimatedGauntlet5	,
		kTPOverOverUnder		,
		kTPUnderUnderOver		,
		kTPUnderOverAnimated	,
		kTPOverUnderAnimated	,
		kTPUnderOverAnimatedNarrow	,
		kTPOverUnderAnimatedNarrow	,		
		kTPGapGap				,
		kTPGapGapGap			,
		kTPGapGapNarrow			,		
		kTPGapGapGapNarrow		,		
		kTPGapSlideGap			,
		kTPGapAnimated			,
		kTPForestGapGap			,
		kTPForestGapGapGap		,
		kTPForestGapSlideGap	,
		kTPForestGapAnimated	,
		kTPForestOverUnderAnimated	,
		kTPFastTravel,
		
		//EC Exclusive Tiles
		kTPStairsDown,
		kTPStairsUp,
		kTPNarrowsLedgesLeft,
		kTPNarrowsLedgesRight,
		kTPStairMaster,
		kTPDescent,
		
		//EC Exclusive Custome Segments
		
		
		//Dark Forest tile set
		kTPDarkForestStraightFlat,
		kTPDarkForestStraight,
		kTPDarkForestSlightLeft,
		kTPDarkForestSlightRight,
		kTPDarkForestStraightShort,
		kTPDarkForestJumpOver,
		kTPDarkForestSlideUnder,
		kTPDarkForestSlideUnderLeft,
		kTPDarkForestSlideUnderRight,
		kTPDarkForestOverUnder,
		kTPDarkForestTurnLeft,
		kTPDarkForestTurnRight,
		kTPDarkForestJunction,
		kTPDarkForestLeftObstacle,
		kTPDarkForestRightObstacle,
		kTPDarkForestEnemy,
		kTPDarkForestGaps,
		kTPDarkForestStumble,
		kTPDarkForestEntrance,
		kTPDarkForestExit,
		kTPDarkForestAnimatedOverUnder,
		kTPDarkForestAnimatedUnder,
		kTPDarkForestAnimatedOver,
		
		
		kTPCemetaryStraight,
		kTPCemetarySlight,
		kTPCemetaryStraightShort,
		kTPCemetaryTurnLeft,
		kTPCemetaryTurnRight,
		kTPCemetarytJunction,
		kTPCemetaryEntrance,
		kTPCemetaryExit,

		
		kTPRuinsStraight,
		kTPRuinsShortStraight,
		kTPRuinsJumpOver,
		kTPRuinsSlideUnder,
		kTPRuinsOverUnder,
		kTPRuinsGaps,
		kTPRuinsTurnLeft,
		kTPRuinsTurnRight,
		kTPRuinsJunction,
		kTPRuinsUnderAnim,
		kTPRuinsOverAnim,
		kTPRuinsSmallGapAnim,
		kTPRuinsEntrance,
		kTPRuinsExit,
		
		//Yellow Brick Road Tile Set
		kTPFarmsStraightFlat,
		kTPFarmsStraight,
		kTPFarmsStraightShort,
		kTPFarmsJumpOver,
		kTPFarmsSlideUnder,
		kTPFarmsOverUnder,
		kTPFarmsTurnLeft,
		kTPFarmsTurnRight,
		kTPFarmsJunction,
		kTPFarmsEntrance,
		kTPFarmsExit,
		kTPFarmsStumbles,
		kTPFarmsStumblesEnemy,
		kTPFarmsStraightShortStumble,
		kTPFarmsFastTurnLeft,
		kTPFarmsFastTurnRight,
		kTPFarmsFastTurnLeftAlt,
		kTPFarmsFastTurnRightAlt,
		kTPFarmsAnimated,
		
		kTPFieldsStraightFlat,
		kTPFieldsStraightFlatLong,
		kTPFieldsStraight,
		kTPFieldsSlightLeft,
		kTPFieldsSlightRight,
		kTPFieldsJumpOverSlightLeft,
		kTPFieldsJumpOverSlightRight,
		kTPFieldsSlideUnder,
		kTPFieldsSlideUnderSlightLeft,
		kTPFieldsSlideUnderSlightRight,
		kTPFieldsOverUnder,
		kTPFieldsOverUnderSlightLeft,
		kTPFieldsOverUnderSlightRight,
		kTPFieldsTurnLeft,
		kTPFieldsTurnRight,
		kTPFieldsJunction,
		kTPFieldsMonkey,
		kTPFieldsEnemy,
		kTPFieldsGaps,
		kTPFieldsEntrance,
		kTPFieldsExit,	
		kTPFieldsUndulateUp,
		kTPFieldsUndulateDown,
		kTPFieldsEnemyOver,
		kTPFieldsEnemyUnder,
		kTPFieldsSmallGap,
		kTPFieldsLargeGap,		
		kTPFieldsFastTurnRight,
		kTPFieldsFastTurnLeft,
		kTPFieldsRollingHills,
		kTPFieldsFlood,
		kTPFieldsOverUnderOver,
		kTPFieldsUnderOverUnder,
		kTPFieldsRightLeft,
		kTPFieldsLeftRight,
		kTPFieldsUpUpAndAway,
		kTPFieldsJumpOver,
		
		kTPFarmsReallyFastTurnLeft,
		kTPFarmsReallyFastTurnRight,
		kTPFarmsReallyFastTurnLeftAlt,
		kTPFarmsReallyFastTurnRightAlt,
		
		
		kTPGrovesStraightFlat,
		kTPGrovesStraight,
		kTPGrovesShortStraight,
		kTPGrovesSlightLeft,
		kTPGrovesSlightRight,
		kTPGrovesOverLeft,
		kTPGrovesOverRight,
		kTPGrovesUnderLeft,
		kTPGrovesUnderRight,
		kTPGrovesTurnLeft,
		kTPGrovesTurnRight,
		kTPGrovesJunction,
		kTPGrovesEntrance,
		kTPGrovesExit,
		kTPGrovesLedgeLeftEnter,
		kTPGrovesLedgeLeftMid,
		kTPGrovesLedgeLeftExit,
		kTPGrovesLedgeRightEnter,
		kTPGrovesLedgeRightMid,
		kTPGrovesLedgeRightExit,
		kTPGrovesStraightShortStumble,
		kTPGrovesCollapseRightLedge,
		kTPGrovesCollapseLeftLedge,
		kTPGrovesEasyOverLeft,
		kTPGrovesEasyOverRight,
		kTPGrovesEasyUnderLeft,
		kTPGrovesEasyUnderRight,
		kTPGrovesEasyStraightShortStumble,
		kTPGrovesEasyCollapseRightLedge,
		kTPGrovesEasyCollapseLeftLedge,
		kTPGrovesEasyLedgeRightEnter,
		kTPGrovesEasyLedgeLeftEnter,
		
		kTPRampartsGaps,
		kTPRampartsGapSmall,
		kTPRampartsGapBig,
		kTPCatacombsSwoopMonkey,
		kTPCatacombsPounceMonkey,
		
		//Custom grove
		kTPGroveTiltUnderLeft,
		kTPGroveTiltUnderRight,
		
		
		// tunnel transition tile set
		kTPTransitionTunnelEntrance	,
		kTPTransitionTunnelExit	,	
		kTPTransitionTunnelMiddle	,
		kTPTransitionTunnelMiddle2	,
		// end of tunnel transition tile set
		
		// these are related to tunnel transition but are part of the definition in all environment sets
		kTPBeforeTunnelTransitionEntrance,
		kTPAfterTunnelTransitionExit,
		
		//Oz Tile Set OS//
		kTPOzOSStraightA		,
		kTPOzOSStraightOverA	,
		kTPMaxx 				
	}
	
	
	class PrefabPath
	{
		public string path;
		public GameObject go;
	}
	
	private static List<PrefabPath> PrefabsToWarm = new List<PrefabPath>();

	// some naming convention definitions for our prefabs here
	public const string			MACHU_CODE = "";
	public const string	        WHIMSY_WOODS_CODE = "ww";
	public const string	        DARK_FOREST_CODE = "df";
	public const string         TUNNEL_TRANSITION_CODE = "tt";
	public const string         BALLOON_CODE = "bc";
	public const string         TUNNEL_TRANSITION_PREFIX = "oz_" + TUNNEL_TRANSITION_CODE;
	public const string			WHIMSY_WOODS_PREFIX = "oz_" + WHIMSY_WOODS_CODE;
	public const string			DARK_FOREST_PREFIX = "oz_" + DARK_FOREST_CODE;
	public const string			BALLOON_PREFIX = "oz_" + BALLOON_CODE;	

	public enum CoinPlacement {
		Left = 0,
		Center = 1,
		Right = 2,
		LeftAndCenter = 3,
		RightAndCenter = 4,
		LeftRightAndCenter = 5,
		LeftAndRight=6,
		Total = 7
	}

	
	//-- statics
	public const string			PREFAB_PATH = "Prefabs/Temple/environments/template_pieces/{0}_prefab";
	public const string			MACHU_PREFAB_PATH = "Prefabs/Temple/environments/machu/{0}_prefab";
	public const string         WHIMSY_WOODS_PREFAB_PATH = "Prefabs/Temple/environments/whimsywoods/{0}_prefab";
	public const string         DARK_FOREST_PREFAB_PATH = "Prefabs/Temple/environments/darkforest/{0}_prefab";
	public const string         YELLOW_BRICK_ROAD_PREFAB_PATH = "Prefabs/Temple/environments/yellowbrickroad/{0}_prefab";
	public const string         EMERALD_CITY_PREFAB_PATH = "Prefabs/Temple/environments/emeraldcity/{0}_prefab";
	public const string			TUNNEL_TRANSITION_PREFAB_PATH = "Prefabs/Temple/environments/tunneltransition/{0}_prefab";
	public const string			BALLOON_PREFAB_PATH = "Prefabs/Temple/environments/balloon/{0}_prefab";
	public static string        CURRENT_ENVIRONMENT_PREFAB_PATH = WHIMSY_WOODS_PREFAB_PATH;
	public static string []     SEARCH_PATHS = { MACHU_PREFAB_PATH, WHIMSY_WOODS_PREFAB_PATH, BALLOON_PREFAB_PATH, TUNNEL_TRANSITION_PREFAB_PATH, DARK_FOREST_PREFAB_PATH, YELLOW_BRICK_ROAD_PREFAB_PATH, EMERALD_CITY_PREFAB_PATH};
	//public static string      CURRENT_ENVIRONMENT_PREFAB_PATH = "Prefabs/Temple/environments/whimsywoods/cliffs/MB" +"/{0}_prefab";

	public static int 			COUNT = 0;
	
	//Gem reduction rate
	public static float 		gemReductionValue = 0f;
	
	private static Transform 	TRACK_PIECE_PREFAB;
	private static CRSpline 	SplinePath =  null;
	
	//-- Instance Data
	public PieceType 			TrackType = PieceType.kTPMaxx;
	public int 					PieceIndex = -1;
	public bool					IsAZipLine = false;
	public TrackPiece			PreviousTrackPiece = null;
	public TrackPiece			NextTrackPiece = null;
	public TrackPiece			Alternate_NextTrackPiece = null;
	public bool					Cleared = true;
	public bool					IsTutorialPiece = false;
	
	public List<BonusItem> 		BonusItems = new List<BonusItem>();
	
	public List<Vector3>		GeneratedPath = null;
	public List<Transform>		PathLocations = null;
	public float				GeneratedPathLength = 0;
	public float				EstimatedPathLength = 0;
	
//	public List<Vector3>		GeneratedPathNormals = null;
//	public List<float>			GeneratedPathSegmentDistances = null;
	
	public bool					UseAlternatePath = false;
	public List<Vector3>		Alternate_GeneratedPath = null;
	private List<Transform>		Alternate_PathLocations = null;
	private float				Alternate_GeneratedPathLength = 0;
//	public List<Vector3>		Alternate_GeneratedPathNormals = null;
//	public List<float>			Alternate_GeneratedPathSegmentDistances = null;
	
	//-- Runtime data holders.
	private List<Renderer>		OpaqueRenderers = null;
	private List<Renderer>		DecalRenderers = null;
	private List<Renderer>[]	ExtraRenderers = null;
	private List<Renderer>		OpaqueForceFadeRenderers = null;
	private List<Renderer>		DecalForceFadeRenderers = null;
//	private List<Renderer>		OpaqueTornadoRenderers = null;
//	private List<Renderer>		DecalTornadoRenderers = null;
	private TrackPieceData		trackPieceData = null;
	public TrackPieceTypeDefinition trackPieceDefinition = null;
	//private Renderer[] 			MyRenderers;
	
	private static LayerMask shadowLayer;
	private static LayerMask defaultLayer;	
	
	public int lastTurnType = 0;
	
	public static void CacheLayerInfo()
	{
		shadowLayer = LayerMask.NameToLayer("ReceiveShadow");
		defaultLayer = LayerMask.NameToLayer("Default");
	}
	
	
	public void ReceiveShadow(bool on)
	{
		_ReceiveShadow(on);
		if(on)
		{
			if(NextTrackPiece)
			{
				NextTrackPiece._ReceiveShadow(on);
			}
			if(Alternate_NextTrackPiece)
			{
				Alternate_NextTrackPiece._ReceiveShadow(on);
			}
		}
	}
	
	private void _ReceiveShadow(bool on)
	{
		if(TrackBuilder.IsBalloonType(trackPieceDefinition))
		{
			return;
		}
		LayerMask layer = defaultLayer;
		if(on)
		{
			layer = shadowLayer;
		}

		if(TrackBuilder.IsTransitionTunnelType(trackPieceDefinition))
		{//transition tunnel uses extra 1 + 2 for track
			if(ExtraRenderers != null)
			{
				for(int i = 0; i < 2; ++i)
				{
					if(ExtraRenderers[i] != null)
					{
						foreach(Renderer r in ExtraRenderers[i])
						{
							if(r != null)
							{
								GameObject obj = r.gameObject;
								if(obj != null)
								{
									obj.layer = layer;
								}
							}
						}
					}
				}
			}
		}
		
		if(OpaqueRenderers != null)
		{
			foreach(Renderer r in OpaqueRenderers)
			{
				if(r != null)
				{
					GameObject obj = r.gameObject;
					if(obj != null)
					{
						obj.layer = layer;
					}
				}
			}
		}
	}
	
	public int isHardSurface;
	
	public List<PieceType> PostPieces = null;//new List<PieceType>();	// Pieces to always post-append to this piece
	
	public TrackPieceData CurrentTrackPieceData
	{
		get { 
			return trackPieceData;
		}
	}
	
	//-- Runtime data
	public enum RenderState
	{
		NotSet,
		Enabled,
		Disabled
	};
	
	private bool RendererIsVisible(List<Renderer> renderers)
	{
		if(renderers == null)
		{
			return false;
		}
		foreach(Renderer renderer in renderers)
		{
			if(renderer.isVisible) 
			{
				return true;
			}
		}
		return false;	
	}
	public bool isVisible()
	{
		return RendererIsVisible(OpaqueRenderers)||RendererIsVisible(DecalRenderers)||
			RendererIsVisible(OpaqueForceFadeRenderers)||RendererIsVisible(DecalForceFadeRenderers)||
			RendererIsVisible(ExtraRenderers[0])||RendererIsVisible(ExtraRenderers[1]);
	}

	public RenderState 	LastRenderState = RenderState.NotSet;
	public RenderState	LastInvRenderState = RenderState.NotSet;

	private static Dictionary< string, string> prefabNameToFullPath = new Dictionary<string, string>();

	//-- Runtime Counts
	public float DistanceSinceLastTurn = 0;
	public float DistanceSinceLastObstacle = 0;
	public float DistanceSinceLastEnvironmentChange = 0;
	public float DistanceSinceLastBonusItem = 0;
	public float DistanceSinceLastGem = 0;
	public float DistanceSinceLastTornadoToken = 0;
	public float DistanceSinceLastCoinRun = 0;
	public float DistanceSinceLastBalloon = 0;
	public int BackToBackObstacleCount = 0;
	public int CoinRunCoinCount = 0;
	public float LastCoinPlacementHeight = 0.5f;
	public CoinPlacement LastCoinPlacement = CoinPlacement.Center;
	/// <summary>
	/// The distance since last envset junction he passed
	/// </summary>
	public float DistanceSinceLastEnvsetJunction = 0;
	/// <summary>
	/// if not 0 this piece is going towards the transition tunnel, and the number indicates the envset id
	/// </summary>
	public int TransitionTunnelDestinationId = -1;
	public bool LeadingToTransitionTunnel
	{
		get { 
			return (TransitionTunnelDestinationId != -1);
		}
	}
	
	private SpawnEnemyFromPiece[] cachedSpawners;
	private SpawnEnemyFromPiece[] CurrentSpawners
	{
		get { 
			if(cachedSpawners==null)
				cachedSpawners = GetComponentsInChildren<SpawnEnemyFromPiece>(true);
			return cachedSpawners;
		}
	}
	
	public Transform CachedTransform = null;
	private Vector3 CachedPosition;//asumes track piece never moves
	public void CachePosition()
	{
		CachedPosition = CachedTransform.position;
	}
//	public TrackPieceTypeDefinition TrackDef;
	
	private static float distSubtractedPerPieceAdded = 2.0f;
	public static float DistanceSubtracedPerPieceAdded
	{
		get 	
		{
			return 	distSubtractedPerPieceAdded;
		}
		
		set
		{
			distSubtractedPerPieceAdded = value;
		}
	}
	
	private void Awake()
	{
		// avoid putting more stuff in this function,  use OnSpawned and OnDespawned
		if (notify == null)
		{
			notify = new Notify( this.GetType().Name);	
		}
		if (CachedTransform == null)
		{
			CachedTransform = gameObject.transform;	
		}
	}
	
	void OnEnable()
	{
	}
	
	void OnDisable()
	{
		trackPieceData = null;
		cachedSpawners = null;
	}
	
	public void OnSpawned()
	{
		//notify.Debug ("Spawn TrackPiece with name {0}", this.name);
		//-- Pool objects get reused, so lets reset all internal non static data.
		TrackType = PieceType.kTPMaxx;
		PieceIndex = -1;
		PreviousTrackPiece = null;
		NextTrackPiece = null;
		Alternate_NextTrackPiece = null;
		IsAZipLine = false;
		LastRenderState = RenderState.NotSet;
		LastInvRenderState = RenderState.NotSet;
		
		//-- Reset Runtime Counts
		DistanceSinceLastTurn = 0;
		DistanceSinceLastObstacle = 0;
		DistanceSinceLastEnvironmentChange = 0;
		DistanceSinceLastBonusItem = 0;
		DistanceSinceLastGem = 0;
		DistanceSinceLastTornadoToken = 0;
		DistanceSinceLastCoinRun = 0;
		DistanceSinceLastBalloon = 0;
		BackToBackObstacleCount = 0;
		CoinRunCoinCount = 0;
		LastCoinPlacementHeight = 0.5f;
		LastCoinPlacement = CoinPlacement.Center;
		DistanceSinceLastEnvsetJunction = 0;
		TransitionTunnelDestinationId = -1;
		PostPieces = null;
		cachedSpawners = null;
		
		UseAlternatePath = false;
		
		ResetGeneratedData();
		
		if(BonusItems == null)
		{
			BonusItems = new List<BonusItem>();
		}
			
		if(PathLocations == null)
		{
			PathLocations = new List<Transform>();
		}
		if(GeneratedPath == null)
		{
			GeneratedPath = new List<Vector3>();
		}
//		if(GeneratedPathNormals == null)
//		{
//			GeneratedPathNormals = new List<Vector3>();
//		}
//		if(GeneratedPathSegmentDistances == null)
//		{
//			GeneratedPathSegmentDistances = new List<float>();
//		}
		
		if(Alternate_PathLocations == null)
		{
			Alternate_PathLocations = new List<Transform>();
		}
		if(Alternate_GeneratedPath == null)
		{
			Alternate_GeneratedPath = new List<Vector3>();
		}
//		if(Alternate_GeneratedPathNormals == null)
//		{
//			Alternate_GeneratedPathNormals = new List<Vector3>();
//		}
//		if(Alternate_GeneratedPathSegmentDistances == null)
//		{
//			Alternate_GeneratedPathSegmentDistances = new List<float>();
//		}
		
		SplinePath = null;	
	}

	public void OnDespawned()
	{
		//notify.Debug ("DeSpawn TrackPiece with name {0}", this.name);
		//-- Prep object for reuse.
		PreviousTrackPiece = null;
		
		//-- Before wiping the NextTrackPiece, if this is the current Track Root, move that to next.
		if(GameController.SharedInstance.trackRoot == this )
		{
			GameController.SharedInstance.trackRoot = NextTrackPiece;
		}
		PostPieces = null;
		NextTrackPiece = null;
		Alternate_NextTrackPiece = null;
		SplinePath = null;
		
		LastRenderState = RenderState.NotSet;
		LastInvRenderState = RenderState.NotSet;
		
		UseAlternatePath = false;
		
		IsAZipLine = false;

		if(BonusItems != null)
		{
			RemoveBonusItems();
		}
		
		ResetGeneratedData();
		Pool = null;
	}
	
	/// <summary>
	/// Returns the full path from just the base name of the prefab, may return null
	/// </summary>
	/// <param name='prefabName'>
	/// this should end with _prefab
	/// </param>
	public static string GetFullPathOfPrefab(string prefabName)
	{
		string result = null;
		prefabNameToFullPath.TryGetValue(prefabName, out result);
		return result;
	}
	
	private void ResetGeneratedData()
	{
		GeneratedPathLength = 0;
		Alternate_GeneratedPathLength = 0;
		if(GeneratedPath != null)
		{
			//notify.Debug ("ResetGeneratedData on {0} clearing generatedPath", this.name);
			//temporarily make this Info
			//notify.Debug(string.Format("{0} {1} ResetGeneratedData clearing GeneratedPath {2}", 	Time.frameCount, this.GetInstanceID(), this.name));
			GeneratedPath.Clear();
			GeneratedPath = null;
		}

		if(Alternate_GeneratedPath != null)
		{
			//notify.Debug ("ResetGeneratedData on {0} clearing alternatePath", this.name);
			Alternate_GeneratedPath.Clear();
			Alternate_GeneratedPath = null;
		}

		Cleared = true;
		//notify.Debug ("ResetGeneratedData on {0} CLEARED", this.name);
	}
	
	public static GameObject Instantiate(PieceType trackType)
	{
		//-- Load the TrackPiece Prefab from disk.
		if (TRACK_PIECE_PREFAB == null)
			TRACK_PIECE_PREFAB = ((GameObject)ResourceManager.Load("Prefabs/TrackPiece")).transform;
		
		//-- Create or Find a Pool of TrackPieces.
		SpawnPool pool = PoolManager.Pools["TrackPiece"];
		
		////Profiler.BeginSample("TP_Spawn");
		GameObject go = pool.Spawn(TRACK_PIECE_PREFAB, true).gameObject;
		////Profiler.EndSample();
		
		
		TrackPiece script = go.GetComponent<TrackPiece>();
		script.OnSpawned();
		script.SetTrackTypeAndMesh(trackType);
		script.Pool = pool;
		script.prefabPool = pool._perPrefabPoolOptions[0];
		script.CachedTransform = go.transform;
		script.PostPieces = null;
		script.ReceiveShadow(false);
		
		//Profiler.BeginSample("TP_SetTrackTypeAndMesh");
			//script.SetTrackTypeAndMesh(trackType);
		//Profiler.EndSample();
		script.trackPieceDefinition = TrackBuilder.SharedInstance.GetTypesFromTrackType(trackType);
		script.UpdateTrackPieceRenderers();
		
		script.PieceIndex = ++TrackPiece.COUNT;
		string newName = "TP-" + script.PieceIndex;
		go.name = newName;
		return go;
	}
	
	public static TrackPiece InstantiateTrackPiece(PieceType trackType)
	{
		return Instantiate(trackType).GetComponent<TrackPiece>();
	}
	
	private void ResetRendererLists()
	{
		//Set all track piece renderers to opaque OR transparent
		if(OpaqueRenderers!=null)	
		{
			OpaqueRenderers.Clear();
		}
		else
		{
			OpaqueRenderers = new List<Renderer>();
		}
		if(DecalRenderers!=null)	
		{
			DecalRenderers.Clear();
		}
		else
		{
			DecalRenderers = new List<Renderer>();
		}
		
		if(ExtraRenderers!=null)	
		{
			for(int i = 0 ; i < ExtraRenderers.Length; ++i)
			{
				if(ExtraRenderers[i] != null)
				{
					ExtraRenderers[i].Clear();
				}
				else
				{
					ExtraRenderers[i] = new List<Renderer>();
				}
			}
		}
		else
		{
			ExtraRenderers = new List<Renderer>[GameController.EnvironmentMaterials.ExtraCount];
			for(int i = 0; i<GameController.EnvironmentMaterials.ExtraCount; ++i)
			{
				ExtraRenderers[i] = new List<Renderer>();
			}
		}
		if(OpaqueForceFadeRenderers!=null)	
		{
			OpaqueForceFadeRenderers.Clear();
		}
		else
		{
			OpaqueForceFadeRenderers = new List<Renderer>();
		}
		if(DecalForceFadeRenderers!=null)	
		{
			DecalForceFadeRenderers.Clear();
		}
		else
		{
			DecalForceFadeRenderers = new List<Renderer>();
		}			
	}
	
	public static void RemoveUnwantedMeshes(GameObject rendererParent)
	{
		GameController.DeviceGeneration device = GameController.SharedInstance.GetDeviceGeneration();
		foreach(MeshRenderer r in rendererParent.GetComponentsInChildren<MeshRenderer>(true))
		{
			bool isLo = r.gameObject.name.StartsWith("decals_lo");
			bool isHi = r.gameObject.name.StartsWith("decals_hi");
			bool isForce = r.gameObject.name.Contains("force");
			//remove detail from low end devices
			if(device <= GameController.DeviceGeneration.iPhone4)
			{
				bool isBest = r.gameObject.name.Contains("best");
				if(isBest || isForce)
				{
					Destroy(r);
					continue;
				}
			}
			//remove alpha materials from fill bound devices
			if((device == GameController.DeviceGeneration.iPhone4)||(device == GameController.DeviceGeneration.iPodTouch4)||(device == GameController.DeviceGeneration.LowEnd))
			{
				if(isHi)
				{
					Destroy(r);
					continue;
				}
			}
			else
			if(isLo)
			{
				Destroy(r);
				continue;
			}
		}	
	}
		
	//NOTE: This was in UpdateTrackPieceRenderer, but it will be faster if it's cached out here
	string[] extraTags = { "extra1", "extra2", "extra3", "extra4", "extra5" };

	private void UpdateTrackPieceRenderer(Renderer r, GameController.EnvironmentMaterials fadeMaterials)
	{
		
		GameController.DeviceGeneration device = GameController.SharedInstance.GetDeviceGeneration();

		int isExtra = -1;
		if(r.gameObject.name.Contains("extra"))
		{
			for(int i = 0; i < fadeMaterials.Extra.Length; ++i)
			{
				if((fadeMaterials.Extra[i]!= null) && (r.gameObject.name.Contains(extraTags[i])))
				{
					isExtra = i;
					break;
				}
			}
		}
		bool isDecal = r.gameObject.name.Contains("decals");
		bool isLo = isDecal && r.gameObject.name.Contains("decals_lo");
		bool isHi = isDecal && r.gameObject.name.Contains("decals_hi");
		bool isForce = r.gameObject.name.Contains("force");
		

		//remove detail from low end devices
		if(device <= GameController.DeviceGeneration.iPhone4)
		{
			bool isBest = r.gameObject.name.Contains("best");
			if(isBest || isForce)
			{
				Destroy(r);
				return;
			}
		}
		//remove alpha materials from fill bound devices
		if((device == GameController.DeviceGeneration.iPhone4)||(device == GameController.DeviceGeneration.iPodTouch4)||(device == GameController.DeviceGeneration.LowEnd))
		{
			if(isHi)
			{
				Destroy(r);
				return;
			}
			bool isLoMesh = r.gameObject.name.Contains("decals_lomesh");
			if(isLoMesh)
			{
				isDecal = false;
			}
		}
		else
		if(isLo)
		{
			Destroy(r);
			return;
		}
		
/*			if((device == GameController.DeviceGeneration.iPhone4)||(device == GameController.DeviceGeneration.iPodTouch4))
		{
			bool isLoMesh = r.gameObject.name.StartsWith("decals_lomesh");
			if(isLoMesh)
			{
				isDecal = false;
			}
		}*/
		//Ensure that any decals are on the "decal" layer (ignores shadow)
		if(isDecal)	
		{
			r.gameObject.layer = LayerMask.NameToLayer("decals");
		}
		if(r.gameObject.tag != "DontChangeMat")
		{
			if(isExtra>=0)
			{
				ExtraRenderers[isExtra].Add(r);
			}
			else
			if(isDecal)
			{
				if(isForce)
				{
					DecalForceFadeRenderers.Add(r);
					r.material = fadeMaterials.Decal;//we should never need to set this again
				}
				else
				{
					DecalRenderers.Add(r);
				}
			}
			else
			{
				if(isForce)
				{
					OpaqueForceFadeRenderers.Add(r);
					r.material = fadeMaterials.Opaque;//we should never need to set this again
				}
				else
				{
					OpaqueRenderers.Add(r);
				}
			}				
		}
	}
	public void UpdateTrackPieceRenderers()
	{
		ResetRendererLists();
		GameController.EnvironmentMaterials fadeMaterials = GameController.SharedInstance.TrackFadeMaterials;
		if((trackPieceDefinition != null) && (trackPieceDefinition.IsTransitionTunnel))
		{
			fadeMaterials = GameController.SharedInstance.TunnelFadeMaterials;
		}
		else
		if((trackPieceDefinition != null) && (trackPieceDefinition.IsBalloon))
		{
			fadeMaterials = GameController.SharedInstance.BalloonFadeMaterials;
		}
		
		bUsingOpaque = false;
//		if(GameController.SharedInstance.trackRoot==this)
//		{
//			bUsingOpaque = true;
//		}
		
		GameObject rendererParent = gameObject;
		
		foreach(Renderer r in rendererParent.GetComponentsInChildren<MeshRenderer>(true))
		{
			UpdateTrackPieceRenderer (r, fadeMaterials);
		}
		foreach(Renderer r in rendererParent.GetComponentsInChildren<SkinnedMeshRenderer>(true))
		{
			UpdateTrackPieceRenderer (r, fadeMaterials);
		}

		//now we have the lists of renderers set them to the appropriate state
		UpdateRendererMaterials(bUsingOpaque);
	}

	
	public void CreateSpline()
	{
		//-- TODO: JER Do this on IMPORT.
		//TR.ASSERT(Cleared == true, "object not properly despawned " + this.name);
		//- prev piece <--> this piece <--> nextPiece

		//-- If nextPiece is null, return.
		GeneratedPath.Clear();
		if(NextTrackPiece == null)
			return;
			
		//notify.Debug(	"CreateSpline: type " + this.TrackType + " def " + this.trackPieceData + " def " + this.TrackDef );
		//notify.Debug("CreateSpline: "+this+" Prev:"+this.PreviousTrackPiece+" Next:"+NextTrackPiece+" Alt:"+Alternate_NextTrackPiece);
		//-- create spline for this piece
		if((PathLocations != null) && (PathLocations.Count >= 2))
		{
			//GeneratedPath.Clear();
			
			if(!IsBalloon() && !IsTurn() && /*!IsStairs() &&*/(PathLocations.Count > 2) && (Alternate_PathLocations.Count < 2))
			{
				//-- Spline has the pathlocations from this piece, and 2 end cap control points.
				//-- prevPiece LastPt Minus One, this piece Path Locations, nextPiece FirstPt Plus One
				//-- We use LastPt Minus One and FirstPt Plus One because the LastPt Minus Zero and FirstPt Plus Zero are the same as this
				//-- pieces first and last point.
				Vector3[] tempPath = new Vector3[PathLocations.Count+2];
				for(int i=1; i<= PathLocations.Count; i++)
					tempPath[i] = PathLocations[i-1].position;
				if(PreviousTrackPiece != null && PreviousTrackPiece.PathLocations != null && PreviousTrackPiece.PathLocations.Count >= 2)
				{
					tempPath[0] = PreviousTrackPiece.PathLocations[PreviousTrackPiece.PathLocations.Count-2].position;
				}
				else
				{
					//-- Add the first point 2 twice.
					tempPath[0] = tempPath[1];
				}
				//Cap the endpoints distance from the end to 2 (if it gets too far, the path can get jumbled)
				if((tempPath[0]-tempPath[1]).magnitude > 2f)
					tempPath[0] = tempPath[1] + (tempPath[0]-tempPath[1]).normalized * 2f;
				
				//-- See comment above
				if(NextTrackPiece.PathLocations != null && NextTrackPiece.PathLocations.Count >= 2)
				{
					tempPath[PathLocations.Count+1] = NextTrackPiece.PathLocations[1].position;
				}
				else
				{
					tempPath[PathLocations.Count+1] = tempPath[PathLocations.Count];
				}
				//Cap the endpoints distance from the end to 2 (if it gets too far, the path can get jumbled)
				if((tempPath[PathLocations.Count+1]-tempPath[PathLocations.Count]).magnitude > 2f)
					tempPath[PathLocations.Count+1] = tempPath[PathLocations.Count] + (tempPath[PathLocations.Count+1]-tempPath[PathLocations.Count]).normalized * 2f;
				
				//-- Add points in the tempPath to ensure a constant Velocity?
				
				//-- tempPath now contains our PathLocations + 2 points.
				//-- prevLast .. a .. b .. c .. nextSecond
				SplinePath = null;
				SplinePath = new CRSpline(tempPath);
				int numSections = SplinePath.pts.Length-3;
				int SmoothAmount = numSections * 3;
				if(TrackType == PieceType.kTPMineCurve || 
					TrackType == PieceType.kTPMineCurveLeft ||
					TrackType == PieceType.kTPMineCurveRight)
				{
					SmoothAmount *= 5;
				}
				for (int i = 0; i <= SmoothAmount; i++) 
				{
					float t = (float) i / SmoothAmount;
					GeneratedPath.Add(SplinePath.Interp(t));
				}
				SplinePath = null;
			}
			else
			{
				for (int i = 0; i < PathLocations.Count; i++) 
				{
					GeneratedPath.Add(PathLocations[i].position);
				}
				if(Alternate_PathLocations != null && Alternate_PathLocations.Count > 0)
				{
					Alternate_GeneratedPath.Clear ();
					for (int i = 0; i < Alternate_PathLocations.Count; i++) 
					{
						Alternate_GeneratedPath.Add(Alternate_PathLocations[i].position);
					}
				}
			}
			
			//-- Normalize segments
			//-- Calc Path Length.
			GeneratedPathLength = 0.0f;
			float length = 0.0f;
			Vector3 segment = new Vector3(0,0,0);
			segment = GeneratedPath[0];
			for(int i = 1; i< GeneratedPath.Count; i++)
			{
				segment = GeneratedPath[i] - segment;
				
				//-- Add up length.
				length = segment.magnitude;
				GeneratedPathLength += length;
				
				//-- Normalize and save.
				segment /= length;
//				GeneratedPathNormals.Add (segment);
//				GeneratedPathSegmentDistances.Add (length);
				//-- Move to next point in the list.
				segment = GeneratedPath[i];
			}
			
			if(Alternate_GeneratedPath != null && Alternate_GeneratedPath.Count > 0)
			{
				Alternate_GeneratedPathLength = 0.0f;
				length = 0.0f;
				segment = new Vector3(0,0,0);
				segment = Alternate_GeneratedPath[0];
				for(int i = 1; i< Alternate_GeneratedPath.Count; i++)
				{
					segment = Alternate_GeneratedPath[i] - segment;
					
					//-- Add up length.
					length = segment.magnitude;
					Alternate_GeneratedPathLength += length;
					
					//-- Normalize and save.
					segment /= length;
//					Alternate_GeneratedPathNormals.Add (segment);
//					Alternate_GeneratedPathSegmentDistances.Add (length);
					//-- Move to next point in the list.
					segment = Alternate_GeneratedPath[i];
				}
			}
		}
		
		if (GeneratedPath.Count == 0)
		{
			notify.Error(string.Format("{0} {1} CreateSpline clearing GeneratedPath.Count=0 {2}", 
				Time.frameCount, this.GetInstanceID(), this.name));
		}
		Cleared = false;
		//notify.Debug ("CreateSpline: "+this+" GP: "+this.GeneratedPath.Count);
	}
	
	public TrackPiece AttachPiece(PieceType newTrackType, bool alternateRoute = false)
	{
		//notify.Debug ("AttachPiece={0} to {1} using alt={2}", newTrackType, this, alternateRoute);
		TrackPiece newTrackPiece = TrackPiece.InstantiateTrackPiece(newTrackType);
		
	//	if(newTrackPiece.gameObject.GetComponentInChildren<SpawnEnemyFromPiece>()!=null){
			//newTrackPiece.gameObject.GetComponentInChildren<SpawnEnemyFromPiece>().OnSpawned();
	//	}
		//
		if(newTrackPiece == null)
			return null;
		
		//-- Save a reference to the new track piece.
		if(alternateRoute == true)
		{
			Alternate_NextTrackPiece = newTrackPiece;
		}
		else
		{
			NextTrackPiece = newTrackPiece;	
		}
		
		newTrackPiece.lastTurnType = lastTurnType;
		
		//Record the last turn direction if this is a junction
		if(IsJunction())
		{
			if(NextTrackPiece!=null)
				NextTrackPiece.lastTurnType = 1;
			if(Alternate_NextTrackPiece!=null)
				Alternate_NextTrackPiece.lastTurnType = -1;
		}
		
		
		newTrackPiece.PreviousTrackPiece = this;
		
		Vector3 positionDelta = Vector3.zero;
		
		//-- Get the offset of the first path dummy to the new piece's root.
		if(newTrackPiece.PathLocations != null && newTrackPiece.PathLocations.Count != 0)
		{
			positionDelta = newTrackPiece.PathLocations[0].position;
		}
	
		if(alternateRoute == true)
		{
			//-- Place the new piece at the end of the THIS one.
			if(Alternate_PathLocations != null && Alternate_PathLocations.Count > 0)
			{
				Vector3 endAttachPoint = Alternate_PathLocations[Alternate_PathLocations.Count-1].position;
				positionDelta = endAttachPoint - positionDelta;
				newTrackPiece.transform.position += positionDelta;
				
				//-- Rotate the new piece about the last path dummy.
				float yRot = Alternate_PathLocations[Alternate_PathLocations.Count-1].eulerAngles.y;
				newTrackPiece.transform.RotateAround(endAttachPoint, Vector3.up, yRot);
			}
		}
		else
		{
			//-- Place the new piece at the end of the THIS one.
			if(PathLocations != null && PathLocations.Count > 0)
			{
				Vector3 endAttachPoint;
				endAttachPoint = PathLocations[PathLocations.Count-1].position;
				positionDelta = endAttachPoint - positionDelta;
				newTrackPiece.transform.position += positionDelta;
				
				//-- Rotate the new piece about the last path dummy.
				float yRot = PathLocations[PathLocations.Count-1].eulerAngles.y;
				newTrackPiece.transform.RotateAround(endAttachPoint, Vector3.up, yRot);
			}

		}
		newTrackPiece.CachePosition();//assumes track piece is never moved
		
		//-- Update the tracking numbers (these get used to help control pacing of various elements)
		TrackPieceTypeDefinition newTrackPieceTypes = TrackBuilder.SharedInstance.GetTypesFromTrackType(newTrackType);
		
		
		//-- Set distance since last enset to 0 on entering the tunnel
		if(newTrackPiece.TrackType == TrackPiece.PieceType.kTPTransitionTunnelEntrance)
		{
			UIManagerOz.SharedInstance.inGameVC.SetEstimatedDistanceLeft(DistanceSinceLastEnvsetJunction - GamePlayer.SharedInstance.OnTrackPiece.DistanceSinceLastEnvsetJunction);
			newTrackPiece.DistanceSinceLastEnvsetJunction = 0;
		}
		
		//-- Keep track of the number of pieces since the last turn
		if(!newTrackPiece.IsTurn()) {
			newTrackPiece.DistanceSinceLastTurn = this.DistanceSinceLastTurn + newTrackPiece.EstimatedPathLength;
		} else {
			newTrackPiece.DistanceSinceLastTurn = 0;
		}
		
		//-- Keep track of the number of pieces since the last obstacle
		if(!newTrackPiece.IsObstacle()) {
			newTrackPiece.DistanceSinceLastObstacle = this.DistanceSinceLastObstacle + newTrackPiece.EstimatedPathLength;
		} else {
			newTrackPiece.DistanceSinceLastObstacle = 0;
			newTrackPiece.BackToBackObstacleCount = 0;
		}
		
		//-- Keep track of the number of piece since the last environment change
		if (newTrackPieceTypes.TrackCategory == TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionStart 
			|| newTrackPieceTypes.TrackCategory == TrackPieceTypeCategory.kTrackPieceCategoryEnvironmentTransitionEnd) {
			newTrackPiece.DistanceSinceLastEnvironmentChange = 0;
		} else {
			newTrackPiece.DistanceSinceLastEnvironmentChange = this.DistanceSinceLastEnvironmentChange + newTrackPiece.EstimatedPathLength;	
		}
		
		if ( newTrackPiece.TrackType == TrackPiece.PieceType.kTPEnvSetJunction) {
			newTrackPiece.DistanceSinceLastEnvsetJunction = 0;
			// pop the envChangeHud
			GameController.SharedInstance.envChangeSignAvailable = true;
			//UIManagerOz.SharedInstance.inGameVC.ShowEnvChangeHud("Get ready to turn\nto a new location");
			// flag this as a tutorialEnvChange
			bool ShowTutorialEnv = true; // by default env tutorial is on
			if(PlayerPrefs.GetInt("ShowTutorialEnv") == 2 || TrackBuilder.SharedInstance.CurrentEnvironmentSetId  != 1){
				ShowTutorialEnv = false;
			}
			notify.Debug("AttachePiece kTPEnvSetJunction ShowTutorialEnv " + ShowTutorialEnv);
			if( ShowTutorialEnv || GameController.SharedInstance.forceTutorialEnvOn) {
				GameController.SharedInstance.tutorialEnvOn = true;
			}
		}
		else 
		{
			//Don't count distance to new ENVSET sign while going through transition tunnel
			if(GamePlayer.SharedInstance.OnTrackPiece == null
				|| (PreviousTrackPiece != null 
				&& GamePlayer.SharedInstance.OnTrackPiece.TrackType != TrackPiece.PieceType.kTPTransitionTunnelMiddle 
				&& GamePlayer.SharedInstance.OnTrackPiece.TrackType != TrackPiece.PieceType.kTPTransitionTunnelMiddle2))
			{
				newTrackPiece.DistanceSinceLastEnvsetJunction = this.DistanceSinceLastEnvsetJunction + newTrackPiece.EstimatedPathLength;
			}
		}
			
		if (newTrackPiece.TrackType == TrackPiece.PieceType.kTPBalloonJunction)
		{
			GameController.SharedInstance.SetBalloonDifficulty();
		}
		
		if ( newTrackPiece.TrackType == TrackPiece.PieceType.kTPPreBalloon) { // turn off balloon on this piece immidiately, we will turn it on when we step on ballonjunction
			BalloonPrePiece bpp = newTrackPiece.GetComponentInChildren<BalloonPrePiece>();
			if(bpp!=null)
				bpp.HideBalloon();
			else
				notify.Warning("Piece of type 'kTPPreBalloon' does not have a BalloonPrePiece behaviour attached!",newTrackPiece);
		}
		
		propagateTunnelTransitionDestinationId(newTrackType, alternateRoute, ref newTrackPiece);
		
		if(newTrackPieceTypes.TrackType == PieceType.kTPBalloonExit ||
			newTrackPieceTypes.TrackType == PieceType.kTPBalloonFall || 
			newTrackPieceTypes.TrackType == PieceType.kTPBalloonJunction) 
		{
			newTrackPiece.DistanceSinceLastBalloon = 0;
		} 
		
		else 
		{
			newTrackPiece.DistanceSinceLastBalloon = DistanceSinceLastBalloon + newTrackPiece.EstimatedPathLength;
		}
		
		//-- Keep track of the back to back obstacle count
		if (newTrackPiece.IsObstacle() && this.IsObstacle()) {
			newTrackPiece.BackToBackObstacleCount = this.BackToBackObstacleCount + 1;	
		}
		
		switch(newTrackPiece.trackPieceDefinition.Environment){
			case 0:	newTrackPiece.isHardSurface = EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.hardSurfaceEnv0;
			break;
			case 1:	newTrackPiece.isHardSurface = EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.hardSurfaceEnv1;
			break;
			case 2:	newTrackPiece.isHardSurface = EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.hardSurfaceEnv2;
			break;
		}
		// Handle creating the spline between pieces, adding coins, and bonus items. 
		// NOTE: Spline is generated on the previous track piece because we need to use the current track piece as the end of the control points for the spline.
		//       this is also why coins / bonus items don't get added to the new track piece, they need the spline in place first.
		if (PreviousTrackPiece != null && !alternateRoute) {
			
			try
			{
				CreateSpline();
			}
			catch (System.Exception e)
			{
				notify.Error("Exception calling CreateSpline() , previousTrackPiece=" + PreviousTrackPiece.TrackType + " exception = " + e);
				throw;
			}
			
			//-- Keep track of the number of pieces since the last bonus item or coin run was placed
			//TrackPiece PrevPrevTrackPiece = PreviousTrackPiece.PreviousTrackPiece;
				
			//if (PrevPrevTrackPiece != null) 
			//{
			
			//Don't count distance since last if in tutorial or transition tunnel
			if( GamePlayer.SharedInstance.OnTrackPiece == null
				|| (!GameController.SharedInstance.IsTutorialMode
				&& GamePlayer.SharedInstance.OnTrackPiece.TrackType != TrackPiece.PieceType.kTPTransitionTunnelMiddle 
				&& GamePlayer.SharedInstance.OnTrackPiece.TrackType != TrackPiece.PieceType.kTPTransitionTunnelMiddle2))
			{
				DistanceSinceLastBonusItem 		= PreviousTrackPiece.DistanceSinceLastBonusItem + PreviousTrackPiece.EstimatedPathLength;
				DistanceSinceLastGem 			= PreviousTrackPiece.DistanceSinceLastGem + PreviousTrackPiece.EstimatedPathLength;
				DistanceSinceLastTornadoToken 	= PreviousTrackPiece.DistanceSinceLastTornadoToken + PreviousTrackPiece.EstimatedPathLength;
			}
			
			//Continue to allow coins in TT
			DistanceSinceLastCoinRun 		= PreviousTrackPiece.DistanceSinceLastCoinRun + PreviousTrackPiece.EstimatedPathLength;
			
			//}
/*			else
			{
				PreviousTrackPiece.DistanceSinceLastBonusItem = 0;
				PreviousTrackPiece.DistanceSinceLastCoinRun = 0;
				PreviousTrackPiece.DistanceSinceLastGem = 0;
				PreviousTrackPiece.DistanceSinceLastTornadoToken = 0;
			}*/
			
			//-- Attempt to add a bonus item
			//This was a bug but I left it in to reduce load of spawned bonus items at any one time
			//This is required to prevent bonus items from spawning in positions that kill the player
			AddBonusItems();
			
			//-- If a bonus item was placed set the count to zero
			//!!! Moved this to AddBonusItems
			//if (bonusItemAdded) {
			//	PreviousTrackPiece.DistanceSinceLastBonusItem = 0;
			//}
			
			bool coinRunAdded = false;
			SpawnEnemyFromPiece[] spawners = CurrentSpawners;
			for(int i = 0; i < spawners.Length; ++i)
			{
				if(!spawners[i].WaitToEnable)
				{
					spawners[i].SpawnEnemies(true);
				}
			}

			//This was a bug but I left it in to reduce load of spawned bonus items at any one time
			if(!GameController.SharedInstance.IsTutorialMode){
				coinRunAdded = AddCoinsToTrack();
			}
		
			if (coinRunAdded && CoinRunCoinCount < BonusItemProtoData.SharedInstance.MaxCoinsPerRun) {
				NextTrackPiece.CoinRunCoinCount = CoinRunCoinCount;
				NextTrackPiece.LastCoinPlacement = LastCoinPlacement;
				NextTrackPiece.LastCoinPlacementHeight = LastCoinPlacementHeight;
				NextTrackPiece.LastArcT = LastArcT;
			}
			
			// This bit makes sure a coin run gets ended if it gets cut off early by a piece you can't put coins on.
			if (!coinRunAdded && CoinRunCoinCount > 0) 
			{
				PreviousTrackPiece.DistanceSinceLastCoinRun = 0;
				DistanceSinceLastCoinRun = PreviousTrackPiece.EstimatedPathLength;
				CoinRunCoinCount = 0;
				LastCoinPlacement = CoinPlacement.Center;
				LastCoinPlacementHeight = 0.5f;
			}
		}
		
		
		// this will force a gem if you made it to the end of a cloud run unless you are in the tutorial
		int gemsTutorial = PlayerPrefs.GetInt("GemsTutorial");
		if(newTrackPiece.TrackType == PieceType.kTPBalloonExit )
		{
			if(gemsTutorial > 0  &&  GameController.SharedInstance.DistanceTraveled < BonusItemProtoData.SharedInstance.MinDistanceBetweenGems){
				// if you are in tutorial and already collected a gem, don't spawn a new one
			}
			//DESIGN NO LONGER WANTED A GEM AT THE END OF A BALLOON RUN
			/*
			else{
				//-- Create the actual bonus item
				BonusItem item = BonusItem.Create(BonusItem.BonusItemType.Gem);
	
				float heightAboveGround = IsBalloon() ? 0f : BonusItemProtoData.SharedInstance.BonusItemPlacementHeight;
				int middleIndex = PreviousTrackPiece.GeneratedPath.Count / 2;
				Vector3 location = PreviousTrackPiece.GeneratedPath[middleIndex];		// Stick it in the middle of the piece
				//Vector3 location = GeneratedPath[0];				//Spawn at the beginning 
				location.y += heightAboveGround;
				item.transform.position = location;
				
				item.setShadowEnabled(true);
				location.x = 0.0f;
				location.z = 0.0f;
				location.y = -heightAboveGround + .15f; 
				item.setShadowLocalPosition(location);
				
				int startIndex = middleIndex - 1;
				if(startIndex < 0) 
					startIndex = 0;
				
				if(startIndex != middleIndex) {
					location = PreviousTrackPiece.GeneratedPath[startIndex];
					location -= PreviousTrackPiece.GeneratedPath[middleIndex];
					location.Normalize();
					item.transform.forward = location;
				}
				item.SetVisibility(true);
				if (BonusItems == null) {
					BonusItems = new List<BonusItem>();
				}
				
				BonusItems.Add(item);
			}
			*/
					
		}
		
		
		
		
		
		
		newTrackPiece.IsTutorialPiece = false;
		
		if(GameController.SharedInstance.IsTutorialMode == true)
		{
			GameController.SharedInstance.TutorialTrackPieceCounter++;
			if(TrackBuilder.SharedInstance.QueuedPiecesToAdd != null && TrackBuilder.SharedInstance.QueuedPiecesToAdd.Count > 0) {
				newTrackPiece.IsTutorialPiece = true;
				bool coinsAdded = false;
				if(GameController.SharedInstance.TutorialSegmentID == 5 && PreviousTrackPiece!=null && GameController.SharedInstance.TutorialTrackPieceCounter > 9) {
					BonusItemProtoData.SharedInstance.MinDistanceBetweenCoinRuns = 60.0f;
					if(PreviousTrackPiece.TrackType == PieceType.kTPCurveRight){
						BonusItemProtoData.SharedInstance.AllowCoins = true;
						coinsAdded = PreviousTrackPiece.AddCoinsToTrack(CoinPlacement.Left);
					}
					if(PreviousTrackPiece.TrackType == PieceType.kTPCurveLeft){
						BonusItemProtoData.SharedInstance.AllowCoins = true;
						coinsAdded = PreviousTrackPiece.AddCoinsToTrack(CoinPlacement.Right);
					}
					if(PreviousTrackPiece.TrackType == PieceType.kTPLedgeLeftStart){
						BonusItemProtoData.SharedInstance.AllowCoins = true;
						coinsAdded = PreviousTrackPiece.AddCoinsToTrack(CoinPlacement.Left);
						
					}
					if(PreviousTrackPiece.TrackType == PieceType.kTPLedgeRightStart){
						BonusItemProtoData.SharedInstance.AllowCoins = true;
						coinsAdded = PreviousTrackPiece.AddCoinsToTrack(CoinPlacement.Right);
					}
					if(PreviousTrackPiece.TrackType == PieceType.kTPAnimatedLeftLedge){
						BonusItemProtoData.SharedInstance.AllowCoins = true;
						coinsAdded = PreviousTrackPiece.AddCoinsToTrack(CoinPlacement.Left);
					}
				}
				notify.Debug("coinsAdded={0}", coinsAdded);
				notify.Debug( GameController.SharedInstance.TutorialSegmentID + " adding track " + newTrackPiece.TrackType);
				if(GameController.SharedInstance.TutorialSegmentID == 1){
					if(newTrackPiece.TrackType == PieceType.kTPForestLargeGap){
						//-- Create the actual bonus item
						BonusItem item = BonusItem.Create(BonusItem.BonusItemType.Boost);

						float heightAboveGround = IsBalloon() ? 0f : BonusItemProtoData.SharedInstance.BonusItemPlacementHeight;
						int middleIndex = PreviousTrackPiece.GeneratedPath.Count / 2;
						Vector3 location = PreviousTrackPiece.GeneratedPath[middleIndex];		// Stick it in the middle of the piece
						//Vector3 location = GeneratedPath[0];				//Spawn at the beginning 
						location.y += heightAboveGround;
						item.transform.position = location;
						
						item.setShadowEnabled(true);
						location.x = 0.0f;
						location.z = 0.0f;
						location.y = -heightAboveGround + .15f; 
						item.setShadowLocalPosition(location);
						
						int startIndex = middleIndex - 1;
						if(startIndex < 0) 
							startIndex = 0;
						
						if(startIndex != middleIndex) {
							location = PreviousTrackPiece.GeneratedPath[startIndex];
							location -= PreviousTrackPiece.GeneratedPath[middleIndex];
							location.Normalize();
							item.transform.forward = location;
						}
						item.SetVisibility(true);
						if (BonusItems == null) {
							BonusItems = new List<BonusItem>();
						}
						BonusItems.Add(item);
						
					}
				}
			}
			else {
				switch(GameController.SharedInstance.TutorialSegmentID) {
					case 0:
						TrackSegment.QueueSegmentWithType(TrackSegment.SegmentType.TutorialFinley, TrackBuilder.SharedInstance.QueuedPiecesToAdd);
						GameController.SharedInstance.TutorialSegmentID++;
						GameController.SharedInstance.TutorialTrackPieceCounter = 0;
						notify.Debug("no more pieces to spawn, next trackSegment is " + GameController.SharedInstance.TutorialSegmentID);
						break;
					case 1:
						TrackSegment.QueueSegmentWithType(TrackSegment.SegmentType.TutorialTurn, TrackBuilder.SharedInstance.QueuedPiecesToAdd);
						GameController.SharedInstance.TutorialSegmentID++;
						GameController.SharedInstance.TutorialTrackPieceCounter = 0;
						break;
					case 2:
						TrackSegment.QueueSegmentWithType(TrackSegment.SegmentType.TutorialSlide, TrackBuilder.SharedInstance.QueuedPiecesToAdd);
						GameController.SharedInstance.TutorialSegmentID++;
						GameController.SharedInstance.TutorialTrackPieceCounter = 0;
						break;
					case 3:
						TrackSegment.QueueSegmentWithType(TrackSegment.SegmentType.TutorialEnemy, TrackBuilder.SharedInstance.QueuedPiecesToAdd);
						GameController.SharedInstance.TutorialSegmentID++;
						GameController.SharedInstance.TutorialTrackPieceCounter = 0;
						break;
					case 4:
						TrackSegment.QueueSegmentWithType(TrackSegment.SegmentType.TutorialTilt, TrackBuilder.SharedInstance.QueuedPiecesToAdd);
						GameController.SharedInstance.TutorialSegmentID++;
						GameController.SharedInstance.TutorialTrackPieceCounter = 0;
						break;
					case 5:
						TrackSegment.QueueSegmentWithType(TrackSegment.SegmentType.TutorialMeter, TrackBuilder.SharedInstance.QueuedPiecesToAdd);
						GameController.SharedInstance.TutorialSegmentID++;
						GameController.SharedInstance.TutorialTrackPieceCounter = 0;
						break;
					
					case 6:
						TrackSegment.QueueSegmentWithType(TrackSegment.SegmentType.TutorialBalloon, TrackBuilder.SharedInstance.QueuedPiecesToAdd);
						GameController.SharedInstance.TutorialSegmentID++;
						GameController.SharedInstance.TutorialTrackPieceCounter = 0;
						break;
					 
				}
			}
		}

		
	//	Debug.Log(newTrackPiece.gameObject.name,gameObject);
	//	Debug.Log(newTrackPieceTypes.TrackCategory,gameObject);
	//	Debug.Log(newTrackPieceTypes.TrackType,gameObject);
	//	Debug.Log(newTrackPieceTypes.Environment);
		
		return newTrackPiece;
	}
	
	/// <summary>
	/// Propagates the tunnel transition destination identifier if needed, resets it to -1 otherwise
	/// </summary>
	private void propagateTunnelTransitionDestinationId(PieceType newTrackType, bool alternateRoute, ref TrackPiece  newTrackPiece)
	{
		//TODO check TransitionSignDecider
		if (this.TrackType == TrackPiece.PieceType.kTPEnvSetJunction)
		{
			TransitionSignDecider decider = GetComponentInChildren<TransitionSignDecider>();
			bool mainLeftToTunnel = true;
			int destEnvSetId = -1;
			if (decider == null)
			{
				notify.Warning("TransitionSignDecider not found in " + this.TrackType + " choosing left to go to transition tunnel");
				mainLeftToTunnel = true;
			}
			else
			{
				mainLeftToTunnel = decider.MainLeftGoesToTransitionTunnel;
				destEnvSetId = decider.DestinationId;
			}
			
			// yeah you could write this as one boolean expression, but this is easier to understand
			if (mainLeftToTunnel)
			{
				if (alternateRoute)
				{
					newTrackPiece.TransitionTunnelDestinationId = -1;
				}
				else
				{
					newTrackPiece.TransitionTunnelDestinationId = destEnvSetId;
				}
			}
			else
			{
				if (alternateRoute)
				{
					newTrackPiece.TransitionTunnelDestinationId = destEnvSetId;
				}
				else
				{
					newTrackPiece.TransitionTunnelDestinationId = -1;
				}
			}
		}
		else
		{
			newTrackPiece.TransitionTunnelDestinationId = this.TransitionTunnelDestinationId;
			// of course once we're in the tunnel we need to set this boolean to off
			if (newTrackPiece.TrackType == PieceType.kTPTransitionTunnelEntrance)
			{
				newTrackPiece.TransitionTunnelDestinationId = -1;	
			}
		}
		
		if (newTrackPiece.LeadingToTransitionTunnel)
		{
			notify.Debug (newTrackPiece.TrackType + " leads to transition tunnel " +  newTrackPiece.transform.parent.name);	
		}		
		
	}
	
	//Adjusted for Track Overlap
	public void AttachRandomPiece(bool alternateRoute, List<PieceType> queueList = null, int turn = 0)
	{
		//-- Base Case
		if(TrackBuilder.SharedInstance == null)
		{
			return;
		}
		
		if(TrackBuilder.SharedInstance.DebugTrackSegment!=null && alternateRoute)
			return;

		GameController.SharedInstance.needSetTrackVisibility = true;
		PieceType newTrackType = PieceType.kTPStraightFlat;
		
		//-- Ask the TrackBuilder for a piece.
		TrackPiece newTrackPiece = this;
		
		if(newTrackPiece.PostPieces != null)
		{
			if(newTrackPiece.PostPieces.Count == 0)
			{
				newTrackPiece.PostPieces = null;
				notify.Warning(newTrackPiece.name + " ended up with an empty post pieces list");
			}
			else
			{
				List<PieceType> postPieces = newTrackPiece.PostPieces;
				newTrackPiece.PostPieces = null;
				
				newTrackPiece = newTrackPiece.AttachPiece(postPieces[0]);
				if(postPieces.Count > 1)
				{
					postPieces.RemoveAt(0);
					newTrackPiece.PostPieces = postPieces;
				}
				return;
			}
		}
		
		newTrackType = TrackBuilder.SharedInstance.ChooseNextTrackPiece(this, queueList, turn);
		
		//notify.Debug (newTrackType);
		
		// Get the TrackPieceType info
		TrackPieceTypeDefinition trackPieceInfo = TrackBuilder.SharedInstance.GetTypesFromTrackType(newTrackType);
			
		if (trackPieceInfo == null) {
			Debug.LogError ("ERROR!!! '"+newTrackType+ "' is NOT in the current pool! Expect game crash!");
			return;
		}
		
		// Attach any Pre-Pieces
		if (trackPieceInfo.PrePieces != null) {
			for (int i = 0; i < trackPieceInfo.PrePieces.Count; i++) {
				newTrackPiece = newTrackPiece.AttachPiece(trackPieceInfo.PrePieces[i]);	
			}
		}
		
		//-- Create the actual object
		//notify.Debug ("Choosing {0}", newTrackType);
		
		newTrackPiece = newTrackPiece.AttachPiece(newTrackType, alternateRoute);
		if(newTrackPiece == null)
		{
			Debug.LogError ("ERROR!!! '"+newTrackType+ "' does NOT have any prefabs! Expect game crash!");
			return;
		}
		
		
		//-- Some pieces are special and trigger a block of other pieces to get spawned immediately instead of queued.
		if(trackPieceInfo.TrackCategory	== TrackPieceTypeCategory.kTrackPieceCategoryCompoundPieceStart)
		{
			//-- Fill out all the other pieces of this compound piece	
			int numberOfMiddlePieces = Random.Range(trackPieceInfo.CompoundPieceMinMiddleCount, trackPieceInfo.CompoundPieceMaxMiddleCount + 1);
			
			newTrackPiece.PostPieces = new List<PieceType>(numberOfMiddlePieces + 1);
			for(int i=0; i<numberOfMiddlePieces; i++)
			{
				newTrackPiece.PostPieces.Add(trackPieceInfo.CompoundPieceMiddleType);
			}
			newTrackPiece.PostPieces.Add(trackPieceInfo.CompoundPieceEndType);
			return;
		}
		
//		else if(TrackBuilder.SharedInstance.IsCloudsJunctionType(newTrackType) && TrackBuilder.SharedInstance.DebugTrackSegment==null)
		else if(newTrackPiece.trackPieceDefinition.IsCloudsJunction && TrackBuilder.SharedInstance.DebugTrackSegment==null)
		{				
			//notify.Debug ("newtype is junction, newTrackPiece is {0}",newTrackPiece);
			if(trackPieceInfo.AlternateAfterJunctionPiece!=TrackPiece.PieceType.kTPBalloonEntrance)
			{
				newTrackPiece.AttachPiece(trackPieceInfo.AlternateAfterJunctionPiece, false);
			}
			
			//notify.Debug ("Alternate_newTrackPiece newTrackPiece {0}",newTrackPiece);
			//if(trackPieceInfo.AfterJunctionPiece!=TrackPiece.PieceType.kTPStraightFlat)
			//{
			return;
		}
		
		
//		else if(TrackBuilder.SharedInstance.IsBalloonJunctionType(newTrackType))
		else if(newTrackPiece.trackPieceDefinition.IsBalloonJunction)
		{	
			TrackPiece currentTrackPiece = newTrackPiece.AttachPiece(trackPieceInfo.AlternateAfterJunctionPiece, false);
			TrackPieceTypeDefinition preBalloonTrackPieceInfo = TrackBuilder.SharedInstance.GetTypesFromTrackType(
				trackPieceInfo.AlternateAfterJunctionPiece);
			// since we do an immediate return, we never get a chance to add the postpieces unless we do it here
			if ((preBalloonTrackPieceInfo.PostPieces != null)&&(preBalloonTrackPieceInfo.PostPieces.Count > 0))
			{//queue the pieces...
				currentTrackPiece.PostPieces = new List<PieceType>(preBalloonTrackPieceInfo.PostPieces);
			}
			
			newTrackPiece.AttachPiece(trackPieceInfo.AfterJunctionPiece, true);
			return;
		}
		
//		else if(TrackBuilder.SharedInstance.IsJunctionType(newTrackType))
		else if(newTrackPiece.trackPieceDefinition.IsJunction)
		{	
			newTrackPiece.AttachPiece(trackPieceInfo.AfterJunctionPiece, false);
			newTrackPiece.AttachPiece(trackPieceInfo.AfterJunctionPiece, true);
			return;
		}
		
		// Attach any Post-Pieces
		if ((trackPieceInfo.PostPieces != null)&&(trackPieceInfo.PostPieces.Count > 0))
		{//queue the pieces...
			newTrackPiece.PostPieces = new List<PieceType>(trackPieceInfo.PostPieces);
		}
		
	}
	
	private void SetTrackTypeAndMesh(PieceType trackType)
	{
		TrackType = trackType;
		trackPieceDefinition = TrackBuilder.SharedInstance.GetTypesFromTrackType(TrackType);	//Get this now. so we don't have to get it every frame
		
//		if(TrackDef == null)
//		{
//			Debug.Log ("Null trackType: " + trackType, gameObject);	
//		}
		
		if(TrackBuilder.IsJunctionType(trackPieceDefinition))
		{
			++TrackBuilder.SharedInstance.ActiveJunctions;
			if(TrackBuilder.SharedInstance.ActiveJunctions > TrackBuilder.SharedInstance.MaxActiveJunctions)
			{
				TrackBuilder.SharedInstance.MaxActiveJunctions = TrackBuilder.SharedInstance.ActiveJunctions;
			}
		}
		if(TrackBuilder.IsObstacleType(trackPieceDefinition))
		{
			if(TrackBuilder.IsJumpOverType(trackPieceDefinition))	StatTracker.ObstacleSpawned(ObstacleType.kJump);
			if(TrackBuilder.IsSlideUnderType(trackPieceDefinition))	StatTracker.ObstacleSpawned(ObstacleType.kDuck);
			if(TrackBuilder.IsStumbleType(trackPieceDefinition))		StatTracker.ObstacleSpawned(ObstacleType.kStumble);
			if(TrackBuilder.IsLedgeType(trackPieceDefinition))		StatTracker.ObstacleSpawned(ObstacleType.kLedge);
		}
		else if(TrackBuilder.IsTurnType(trackPieceDefinition))
			StatTracker.TurnSpawned();
		
		ClearMeshPieces();
		
		string prefabName = TrackBuilder.GetPrefabNameFromTrackTypeDef(trackPieceDefinition);
		if (prefabName == null || prefabName.Length == 0)
		{
			notify.Error("prefabName is empty given TrackType {0}", trackType);
			return;
		}
			
		if (prefabName == null)
		{
			notify.Error("prefabName is null given TrackType {0}", trackType);
			return;			
			
		}
		GameObject go = null;

		string fullPath = "";
		if (prefabNameToFullPath.TryGetValue(prefabName, out fullPath))
		{
		}
		else
		{
			fullPath = string.Format(TrackPiece.CURRENT_ENVIRONMENT_PREFAB_PATH, prefabName);
		}

		//string fullPath = string.Format(TrackPiece.MACHU_PREFAB_PATH, prefabName);
		go = SetTrackMesh(fullPath);

//		Debug.LogError("go===name "+go.name);
		if(go == null)
		{
			Debug.LogError ("SetDataModelReference returned null from "+ fullPath);
			return;
		}
		
		IsAZipLine = false;
		if( TrackType == PieceType.kTPZipLine)
		{
			//notify.Debug("making this a zipline: "+this);
			IsAZipLine = true;
		}
		
////Profiler.BeginSample("STTAM_SETUP");
		//PathLocations.Clear();
		//TrackPieceData data = go.GetComponentInChildren<TrackPieceData>();    
		TrackPieceData data = go.GetComponent<TrackPieceData>();
		if(data != null)
		{
			PathLocations = data.PathLocations;
			Alternate_PathLocations = data.AltPathLocations;
			//Now dynamically finding these in UpdateTrackPieceRenderers
			//NOTE: Commented out because we find these dynamically with GetComponentInChildren() elsewhere.
			//TrackPieceRenderers = data.TrackPieceRenderers;
			EstimatedPathLength = data.EstimatedPathLength;
			trackPieceData = data;
		}
		else
		{
			//TrackPieceData[] datas = go.GetComponentsInChildren<TrackPieceData>(true);
			TrackPieceData[] datas = go.GetComponentsInChildren<TrackPieceData>();
			if(datas.Length>0)
			{
				EstimatedPathLength = data.EstimatedPathLength;
				trackPieceData = datas[0];
			}
			else
			{
				trackPieceData = null;
				notify.Warning("No TrackPieceData on " + gameObject.name);	
			}
		}

		if(PathLocations.Count < 2)
		{
			notify.Error("Not Enough PathLocations in object {0}", go);
			return;
		}
		
		//UpdateCachedComponents();
////Profiler.EndSample();

	}
	
	public void ClearMeshPieces()
	{
		//Profiler.BeginSample("ClearMeshPieces");
		RemoveAllSubObjects();
		ClearAllChildren();
		PreviousTrackPiece = null;
		//Profiler.EndSample();
	}
	
	private void RemoveBonusItems()
	{
		//notify.Debug("RemoveBonusItems for {0}", this.name);
		if(BonusItems == null)
			return;
		
		int max = BonusItems.Count;
		for (int i = 0; i < max; i++) {
			if(BonusItems[i] == null)
				continue;
			BonusItems[i].DestroySelf();
		}
		
		BonusItems.Clear();
		BonusItems = null;
	}
	
	private void ActivateBonusItems(bool active)
	{
		if(BonusItems == null)
			return;
		
		int max = BonusItems.Count;
		for (int i = 0; i < max; i++) {
			if(BonusItems[i] == null)
				continue;
			//BonusItems[i].SetVisibility(active);
		}
	}
	
	private void RemoveAllSubObjects()
	{
		//Profiler.BeginSample("RemoveAllSubObjects");
		RemoveBonusItems();
		//Profiler.EndSample();
	}
	
	public void UpdateCachedComponents()
	{
		SetRendering(true);
	}
	
	// tries to guess where a prefab is located and load it, then uses SEARCH_PATHS, may return null
	// fullPath if not blank will contain the full path of the loaded prefab
	public static GameObject LoadPrefab( string prefabName, out string  fullPath)
	{
		GameObject result = null;
		fullPath = "";
		
		if (prefabName.StartsWith( WHIMSY_WOODS_PREFIX))
		{
			string guessedPath = string.Format( WHIMSY_WOODS_PREFAB_PATH, prefabName);
			result = ResourceManager.Load	(guessedPath, typeof(GameObject)) as GameObject;
			if (result)
			{
				fullPath = guessedPath;	
			}
		}
		else if (prefabName.StartsWith( TUNNEL_TRANSITION_PREFIX))
		{
			string guessedPath = string.Format( TUNNEL_TRANSITION_PREFAB_PATH, prefabName);
			result = ResourceManager.Load	(guessedPath, typeof(GameObject)) as GameObject;
			if (result)
			{
				fullPath = guessedPath;	
			}			
		}
		
		if (result == null)
		{
			// probably a machu piece, it's first in the SEARCH_PATHS
			foreach ( string guessedPath in SEARCH_PATHS)
			{
				string actualGuessedPath = string.Format( guessedPath, prefabName);
				result = ResourceManager.Load	(actualGuessedPath, typeof(GameObject)) as GameObject;
				if (result)
				{
					fullPath = actualGuessedPath;	
					break;
				}				
			}	
		}
		
		return result;
	}
	
	/// <summary>
	/// given the prefab name, what is our best guess for it's path, 
	/// </summary>
	/// <returns>
	/// The best guessed path, will return prefab name if we don't know
	/// </returns>
	/// <param name='prefabName'>
	/// Prefab name.
	/// </param>
	public static string BestGuessPath(string prefabName)
	{
		string format = "{0}";
		if (prefabName.StartsWith( WHIMSY_WOODS_PREFIX))
		{
			format =  WHIMSY_WOODS_PREFAB_PATH;
		}
		else if (prefabName.StartsWith( TUNNEL_TRANSITION_PREFIX))
		{
			format = TUNNEL_TRANSITION_PREFAB_PATH;
		}
		else if (prefabName.StartsWith( DARK_FOREST_PREFIX))
		{
			format = DARK_FOREST_PREFAB_PATH;	
		}
		else if (prefabName.StartsWith( BALLOON_PREFIX))
		{
			format = BALLOON_PREFAB_PATH;
		}
		
		string guessedPath = string.Format(format,prefabName);
		return guessedPath;
	}
/*	
	public static IEnumerator WarmPrefabsAsync()
	{
		yield return null;
		
		notify.Debug("WARMING POOLS START @ " + Time.realtimeSinceStartup);
		PrefabsToWarm.Clear();//should already be clear
		for (PieceType t = PieceType.kTPStraight; t < PieceType.kTPMaxx; ++t) 
		{
			TrackPieceTypeDefinition types = TrackBuilder.SharedInstance.GetTypesFromTrackType(t);
			
			if (types == null) {
				continue;	
			}
			
			foreach(KeyValuePair<string, float> variation in types.Variations)
			{
				string prefabName = variation.Key;
				if (prefabName != null && prefabName != "") 
				{
					string fullPath = "";
					GameObject go = LoadPrefab( prefabName, out fullPath);
					notify.Debug("loading " + fullPath + " gameobj = " + go + " " + prefabName);
					Debug.LogError ("loading " + fullPath + " gameobj = " + go + " " + prefabName);
					
					if(go!=null)
					{
						PrefabPath pp = new PrefabPath();
						pp.go = go;
						pp.path = fullPath;
						PrefabsToWarm.Add (pp);

						//TODO see if there's a better way to do this, seems wasteful
						if (prefabNameToFullPath.ContainsKey(prefabName) == false)
							prefabNameToFullPath.Add ( prefabName, fullPath);
					}
					
					yield return new WaitForSeconds(0.01f);
				}
			}
		}
	
		
#if !UNITY_EDITOR
		foreach(PrefabPath pp in PrefabsToWarm)
		{
			foreach(Renderer r in pp.go.GetComponentsInChildren<MeshRenderer>(true))
			{
				Material material = r.material;
				if(material != null)
				{
					if(material.mainTexture != null)
					{
						Texture t = material.mainTexture;
						material.mainTexture = null;
						Resources.UnloadAsset(t);
					}
					Texture lightmap = material.GetTexture("_DetailTex");
					if(lightmap != null)
					{
						material.SetTexture("_DetailTex", null);
						Resources.UnloadAsset(lightmap);
					}
					r.material = null;
				}
			}
		}
#endif	
	}
*/
	
	public static void WarmPrefabs()
	{
		notify.Debug("WARMING POOLS START @ " + Time.realtimeSinceStartup);
		PrefabsToWarm.Clear();//should already be clear
		for (PieceType t = PieceType.kTPStraight; t < PieceType.kTPMaxx; ++t) 
		{
			TrackPieceTypeDefinition types = TrackBuilder.SharedInstance.GetTypesFromTrackType(t);
			
			if (types == null) {
				continue;	
			}
			
			foreach(KeyValuePair<string, float> variation in types.Variations)
			{
				string prefabName = variation.Key;
				if (prefabName != null && prefabName != "") 
				{
					string fullPath = "";
					GameObject go = LoadPrefab( prefabName, out fullPath);
					notify.Debug("loading " + fullPath + " gameobj = " + go + " " + prefabName);
					
					if(go!=null)
					{
						PrefabPath pp = new PrefabPath();
						pp.go = go;
						pp.path = fullPath;
						PrefabsToWarm.Add (pp);

						//TODO see if there's a better way to do this, seems wasteful
						if (prefabNameToFullPath.ContainsKey(prefabName) == false)
							prefabNameToFullPath.Add ( prefabName, fullPath);
					}
				}
			}
		}
	}	
	
	public static void WarmPools()
	{
		notify.Debug("WARMING POOLS START");
		SpawnPool p = PoolManager.Pools["TrackMesh"];
		if(!p)
		{
			notify.Error("No TrackMesh pool!");
			return;
		}
		
		foreach (PrefabPath pp in PrefabsToWarm)
		{
			GameObject go = pp.go;
			
			PrefabPool existingPrefabPool;
			try
			{
				existingPrefabPool = p.GetPrefabPool(go.transform);
			}
			catch (System.Exception theException)
			{
				notify.Error("error pooling {0} {1}", go.name, theException.Message );
				throw;
			}
			if (existingPrefabPool != null)
			{
				// probably tunnel transition, has already been warmed up
				continue;
			}
			//notify.Debug("Creating PoolDef for {0} ", fullPath);
			PrefabPool meshPoolDef = new PrefabPool(go.transform);
			TrackPieceData tpData = go.GetComponent<TrackPieceData>();
			if(tpData != null)
			{
				meshPoolDef.preloadAmount = tpData.preloadAmount;
				tpData.preFabPool = meshPoolDef;
			}
			List<Transform> prefabs = new List<Transform>();
			prefabs = p.CreatePrefabPool(meshPoolDef);

			foreach(Transform transform in prefabs)
			{
				RemoveUnwantedMeshes(transform.gameObject);
			}

			//-- Save this GameObject for fast lookup later in SetModelDataReference.
			if (loaded_prefabs.ContainsKey(pp.path) == false)
				loaded_prefabs.Add(pp.path, meshPoolDef);
		}
		PrefabsToWarm.Clear();

	}
	
	
	// Warning, any changes to this, please update WarmResourcesCoroutine
/*	public static void WarmResources()
	{
		
		notify.Debug("WARMING START");
		int c = 0;
		SpawnPool p = PoolManager.Pools["TrackMesh"];
		
		for (PieceType t = PieceType.kTPStraight; t < PieceType.kTPMaxx; ++t) 
		{
			TrackPieceTypeDefinition types = TrackBuilder.SharedInstance.GetTypesFromTrackType(t);
			
			if (types == null) {
				continue;	
			}
			
			foreach(KeyValuePair<string, float> variation in types.Variations)
			{
				string prefabName = variation.Key;
				if (prefabName != null && prefabName != "") 
				{
					++c;
					
					
					string fullPath = "";
					GameObject go = LoadPrefab( prefabName, out fullPath);
					notify.Debug("loading " + fullPath + " gameobj = " + go + " " + prefabName);
					
					PrefabPool existingPrefabPool;
					try
					{
						existingPrefabPool = p.GetPrefabPool(go.transform);
					}
					catch (System.Exception theException)
					{
						notify.Error("error loading {0} {1}", prefabName, theException.Message );
						throw;
					}
					if (existingPrefabPool != null)
					{
						// probably tunnel transition, has already been warmed up
						continue;
					}
//#if !UNITY_EDITOR
//					RemoveLoOrHiMesh(go);
//#endif
					if(p)
					{
						//notify.Debug("Creating PoolDef for {0} ", fullPath);
						PrefabPool meshPoolDef = new PrefabPool(go.transform);
						TrackPieceData tpData = go.GetComponent<TrackPieceData>();
						if(tpData != null)
						{
							meshPoolDef.preloadAmount = tpData.preloadAmount;
							tpData.preFabPool = meshPoolDef;
						}
//#if UNITY_EDITOR
						List<Transform> prefabs = new List<Transform>();
						prefabs =
//#endif
						p.CreatePrefabPool(meshPoolDef);

//#if UNITY_EDITOR
						foreach(Transform transform in prefabs)
						{
							RemoveUnwantedMeshes(transform.gameObject);
						}
//#endif

						//-- Save this GameObject for fast lookup later in SetModelDataReference.
						if (loaded_prefabs.ContainsKey(fullPath) == false)
							loaded_prefabs.Add(fullPath, meshPoolDef);
					}

					
					//TODO see if there's a better way to do this, seems wasteful
					if (prefabNameToFullPath.ContainsKey(prefabName) == false)
						prefabNameToFullPath.Add ( prefabName, fullPath);
				}
			}
		}
	}*/
	
	// mimics WarmupResources, but does it in a coroutine to spread out the 4.5 seconds freeze on an iPhone 3GS
	public static IEnumerator WarmPrefabsCoroutine()
	{
		notify.Debug("WARMING PREFABS COROUTINE START @ " + Time.realtimeSinceStartup);
		PrefabsToWarm.Clear();//should already be clear
		
		for (PieceType t = PieceType.kTPStraight; t < PieceType.kTPMaxx; ++t) 
		{
			
			TrackPieceTypeDefinition types = TrackBuilder.SharedInstance.GetTypesFromTrackType(t);
//			yield return null;
			
			if (types == null) 
			{
				continue;	
			}
			foreach(KeyValuePair<string, float> variation in types.Variations)
			{
				string prefabName = variation.Key;
				if (prefabName != null && prefabName != "") 
				{
					string fullPath = "";
					GameObject go = null;
					string guessedPath = BestGuessPath(prefabName);
					notify.Debug ("{0} calling lLoadAsyncFromAssetBundleprefabName", Time.realtimeSinceStartup);
					AssetBundleRequest loadReq  = ResourceManager.SharedInstance.LoadAsyncFromAssetBundle(guessedPath, typeof(GameObject));
					if (loadReq != null)
					{
						notify.Debug ("{0} about to yield on loadReq", Time.realtimeSinceStartup);
						yield return loadReq;
						notify.Debug ("{0} done yielding on loadReq", Time.realtimeSinceStartup);
						fullPath = guessedPath;
						go = loadReq.asset as GameObject;
					}
					else
					{
						notify.Debug("Could not async load " + prefabName);
						go = LoadPrefab( prefabName, out fullPath);
					}

					if (fullPath == "")
					{
						notify.Error("error loading " + prefabName);
					}

					if(go != null)
					{
						PrefabPath pp = new PrefabPath();
						pp.go = go;
						pp.path = fullPath;
						PrefabsToWarm.Add (pp);
						//TODO see if there's a better way to do this, seems wasteful
						if (prefabNameToFullPath.ContainsKey(prefabName) == false)
							prefabNameToFullPath.Add ( prefabName, fullPath);
						yield return null;
					}
				}
			}
		}
	}

	public static IEnumerator WarmPoolsCoroutine()
	{
		notify.Debug("WarmPoolsCoroutine start @ " + Time.realtimeSinceStartup);
		SpawnPool p = PoolManager.Pools["TrackMesh"];
		
		if(p == null)
		{
			Debug.Log("No TrackMesh pool");
			yield break;
		}
		
		foreach (PrefabPath pp in PrefabsToWarm)
		{
			GameObject go = pp.go;
			PrefabPool existingPrefabPool = p.GetPrefabPool(go.transform);
			if (existingPrefabPool != null)
			{
				// probably tunnel transition, has already been warmed up
				continue;
			}
			notify.Debug("Creating PoolDef for {0} ", pp.path);
			PrefabPool meshPoolDef = new PrefabPool(go.transform);
			//meshPoolDef.preloadAmount = 5;
			TrackPieceData tpData = go.GetComponent<TrackPieceData>();
			if(tpData != null)
			{
				meshPoolDef.preloadAmount = tpData.preloadAmount;
				tpData.preFabPool = meshPoolDef;
			}
			List<Transform> prefabs = new List<Transform>();
			yield return p.StartCoroutine(p.CreatePrefabPoolCoroutine(meshPoolDef, prefabs));

			foreach(Transform transform in prefabs)
			{
				RemoveUnwantedMeshes(transform.gameObject);
				yield return null;
			}
			prefabs = null;
			
			//-- Save this GameObject for fast lookup later in SetModelDataReference.
			if (loaded_prefabs.ContainsKey(pp.path) == false)
				loaded_prefabs.Add(pp.path, meshPoolDef);
		}
	}
/*	
	public static IEnumerator WarmupResourcesCoroutine()
	{
		int c = 0;
		SpawnPool p = PoolManager.Pools["TrackMesh"];
		
		for (PieceType t = PieceType.kTPStraight; t < PieceType.kTPMaxx; ++t) 
		{
			
			TrackPieceTypeDefinition types = TrackBuilder.SharedInstance.GetTypesFromTrackType(t);
//			yield return null;
			
			if (types == null) {
				continue;	
			}
			foreach(KeyValuePair<string, float> variation in types.Variations)
			{
				string prefabName = variation.Key;
				if (prefabName != null && prefabName != "") 
				{
					++c;

					string fullPath = "";
					GameObject go = null;
					string guessedPath = BestGuessPath(prefabName);
					notify.Debug ("{0} calling lLoadAsyncFromAssetBundleprefabName", Time.realtimeSinceStartup);
					AssetBundleRequest loadReq  = ResourceManager.SharedInstance.LoadAsyncFromAssetBundle(guessedPath, typeof(GameObject));
					if (loadReq != null)
					{
						notify.Debug ("{0} about to yield on loadReq", Time.realtimeSinceStartup);
						yield return loadReq;
						notify.Debug ("{0} done yielding on loadReq", Time.realtimeSinceStartup);
						fullPath = guessedPath;
						go = loadReq.asset as GameObject;
					}
					else
					{
						notify.Debug("Could not async load " + prefabName);
						go = LoadPrefab( prefabName, out fullPath);
					}

					if (fullPath == "")
					{
						notify.Error("error loading " + prefabName);
					}
					
					PrefabPool existingPrefabPool = p.GetPrefabPool(go.transform);
					if (existingPrefabPool != null)
					{
						// probably tunnel transition, has already been warmed up
						continue;
					}
//					yield return null;
						
//#if !UNITY_EDITOR
//					RemoveLoOrHiMesh(go);
//#endif
					if(p)
					{
			//			notify.Debug("Creating PoolDef for {0} ", fullPath);
						PrefabPool meshPoolDef = new PrefabPool(go.transform);
						meshPoolDef.preloadAmount = 5;
						TrackPieceData tpData = go.GetComponent<TrackPieceData>();
						if(tpData != null)
						{
							tpData.preFabPool = meshPoolDef;
						}
//						yield return null;
//#if UNITY_EDITOR
						List<Transform> prefabs = new List<Transform>();
						prefabs =
//#endif
							p.CreatePrefabPoolCoroutine(meshPoolDef);
//						yield return null;

//#if UNITY_EDITOR
						foreach(Transform transform in prefabs)
						{
							yield return null;
							RemoveUnwantedMeshes(transform.gameObject);
						}
//#endif
				
						
						//-- Save this GameObject for fast lookup later in SetModelDataReference.
						if (loaded_prefabs.ContainsKey(fullPath) == false)
							loaded_prefabs.Add(fullPath, meshPoolDef);
//						yield return null;

					}
									
					//TODO see if there's a better way to do this, seems wasteful
					if (prefabNameToFullPath.ContainsKey(prefabName) == false)
						prefabNameToFullPath.Add ( prefabName, fullPath);
					yield return null;
				}
				//yield return new WaitForSeconds(Settings.GetFloat("piece-wait", 0.05f));
			}
		}
	}
*/	
	public void SetCurrentSegmentAndSkipLogic(int segment)
	{
		mCurrentSegment = segment;
	}
	
	public int CurrentSegment
	{
		get {return mCurrentSegment;}
		set
		{
			mCurrentSegment = value;
			doSegmentLogic();
		}
	}
	private int mCurrentSegment = 0;
	
	//-- Event Delegate Definitions
	public delegate void SegmentChangedHandler(TrackPiece trackPiece, int segmentIndex);
	public delegate void DoTurnHandler(TrackPiece trackPiece, int segmentIndex);
	public delegate void DoZipLineGrabHandler(TrackPiece trackPiece, int segmentIndex);
	public delegate void DoZipLineHangHandler(TrackPiece trackPiece, int segmentIndex);
	public delegate void DoZipLineLetGoHandler(TrackPiece trackPiece, int segmentIndex);
	public delegate void DoZipLineClearBaseHandler(TrackPiece trackPiece, int segmentIndex);
	
	//-- The Handlers for the event.
	public static event SegmentChangedHandler 	onNextSegment = null;
	public static event DoTurnHandler			onDoTurn = null;
	public static event DoZipLineGrabHandler	onDoZipLineGrab = null;
	public static event DoZipLineHangHandler	onDoZipLineHang = null;
	public static event DoZipLineLetGoHandler	onDoZipLineLetGo = null;
	public static event DoZipLineClearBaseHandler onDoZipLineClearBase = null;
	
	
	//-- All of the segment Logic can probably be data drived a bit better.
	//-- Seeing all these magic numbers makes me squirm.
	private void doSegmentLogic()
	{
		if(IsTurn() == true)
		{
			if(mCurrentSegment == 1)
			{
				//-- Turn if we are on the 2nd segment of a turn becaus turns have 3 PathLocations.
				if(onDoTurn != null)
				{
					onDoTurn(this, mCurrentSegment);
				}
			}
		}
		
		if(IsAZipLine == true)
		{
			if(mCurrentSegment == 2)
			{
				if(onDoZipLineGrab != null)
				{
					onDoZipLineGrab(this, mCurrentSegment);
				}
			}
			else if(mCurrentSegment == 3)
			{
				if(onDoZipLineHang != null)
				{
					onDoZipLineHang(this, mCurrentSegment);
				}
			}
			else if(mCurrentSegment == 8)
			{
				if(onDoZipLineClearBase != null)
				{
					onDoZipLineClearBase(this, mCurrentSegment);
				}
			}
			else if(mCurrentSegment == 33)
			{
				if(onDoZipLineLetGo != null)
				{
					onDoZipLineLetGo(this, mCurrentSegment);
				}
			}
		}
		
		//-- Generic catch all.
		if(onNextSegment != null)
		{
			onNextSegment(this, mCurrentSegment);
		}
	}

#if UNITY_EDITOR
	Color turnColor = new Color(0.0f, 0.0f, 1.0f, 0.1f);
	void OnDrawGizmos()
	{
		//if(!bUsingOpaque)	{Gizmos.color = new Color(1,0,0,0.25f);Gizmos.DrawSphere(transform.position,3f);}
		
		if(GeneratedPath != null && GeneratedPath.Count > 1)
		{
			if(TrackBuilder.IsTurnType(trackPieceDefinition)) {
				Gizmos.color = turnColor;
				Gizmos.DrawSphere(GeneratedPath[1], GameController.SharedInstance.MaxTurnDistance);
				Gizmos.color = Color.white;
			}
			for(int i=0; i<GeneratedPath.Count-1; i++)
			{
				float scaler = 1.0f;
				if(IsAZipLine == true)
				{
					scaler = 2.0f;
					Gizmos.color = Color.blue;
				}
				else
				{
					switch(i%3)
					{
					case 0:
						Gizmos.color = Color.cyan;
						break;
					case 1:
						Gizmos.color = Color.yellow;
						break;
					case 2:
						Gizmos.color = Color.magenta;
						break;
					}
				}
				
				if(CurrentSegment == i)
				{
					Gizmos.color = Color.red;
				}
				
					
				Gizmos.DrawLine(GeneratedPath[i] + (Vector3.up*scaler), GeneratedPath[i+1] + (Vector3.up*scaler));
			}
		}
		
//		if(ShowBonusItemGizmo == true)
//		{
//			Gizmos.DrawIcon(transform.position, "tr_Icon", false);		
//		}
		//
	}
#endif
	/*
	// if we are a tunnel transition piece, use the correct alpha material
	private Material chooseAlphaMaterial()
	{
		Material result = GameController.SharedInstance.TrackPieceAlphaMaterial;
		//TrackPieceTypeDefinition def = TrackBuilder.SharedInstance.GetTypesFromTrackType( this.TrackType);	//Using TrackDef instead
		if (TrackDef != null)
		{
			if (TrackDef.IsTransitionTunnel)
			{
				result = GameController.SharedInstance.TunnelTransitionAlphaMaterial;	
				
			}
			else if (TrackDef.IsBalloon)
			{
				result = GameController.SharedInstance.BalloonTileAlphaMaterial;	
			}
		}
		return result;
	}
	
	// if we are a tunnel transition piece, use the correct opaque material
	private Material chooseOpaqueMaterial()
	{
		Material result = GameController.SharedInstance.TrackMaterials.OpaqueMaterial;
		//TrackPieceTypeDefinition def = TrackBuilder.SharedInstance.GetTypesFromTrackType( this.TrackType);	//Using TrackDef instead
		if (TrackDef != null)
		{
			if (TrackDef.IsTransitionTunnel)
			{
				result = GameController.SharedInstance.TunnelTransitionOpaqueMaterial;	
				
			}
			else if (TrackDef.IsBalloon)
			{
				result = GameController.SharedInstance.BalloonTileOpaqueMaterial;	
			}
		
		}
		return result;
	}
	private Material chooseDecalMaterial()
	{
		Material result = GameController.SharedInstance.TrackPieceDecalMaterial;
		//TrackPieceTypeDefinition def = TrackBuilder.SharedInstance.GetTypesFromTrackType( this.TrackType);	//Using TrackDef instead
		if (TrackDef != null)
		{
			if (TrackDef.IsTransitionTunnel)
			{
				result = GameController.SharedInstance.TunnelTransitionDecalMaterial;

			}
			else if (TrackDef.IsBalloon)
			{
				result = GameController.SharedInstance.BalloonDecalOpaqueMaterial;	
			}
		
		}
		return result;
	}
	private Material chooseDecalFadeMaterial()
	{
		Material result = GameController.SharedInstance.TrackPieceDecalFadeMaterial;
		//TrackPieceTypeDefinition def = TrackBuilder.SharedInstance.GetTypesFromTrackType( this.TrackType);	//Using TrackDef instead
		if (TrackDef != null)
		{
			if (TrackDef.IsTransitionTunnel)
			{
				result = GameController.SharedInstance.TunnelTransitionFadeDecalMaterial;	
				
			}
			else if (TrackDef.IsBalloon)
			{
				result = GameController.SharedInstance.BalloonDecalAlphaMaterial;	
			}
		
		}
		return result;
	}
	*/
	private bool bUsingOpaque = false;
	public void LateUpdate()
	{
		//-- Get distance from Camera. change materials if need to be blended.
		if((GameController.SharedInstance == null) || (GameController.SharedInstance.Player == null) || (GameCamera.SharedInstance == null) || (GameCamera.SharedInstance.CachedTransform == null))
			return;
		
		//if(GeneratedPath != null && GeneratedPath.Count > 1)
		//{
		//	mScratchPadVector3 = GeneratedPath[GeneratedPath.Count-1];	
		//}
		//else
		//{
		//	mScratchPadVector3 = CachedTransform.position;//transform.position;
		//}
		
		bool useOpaque = false;
		
		float boundingRadius = 0f;
		GameController.DeviceGeneration device = GameController.SharedInstance.GetDeviceGeneration();
		if((CurrentTrackPieceData != null)&&
			(device != GameController.DeviceGeneration.iPhone4) &&
			(device != GameController.DeviceGeneration.iPodTouch4))
		{
			boundingRadius = CurrentTrackPieceData.EstimatedPathLength;
		}
		
		if(GameController.SharedInstance.Player.OnTrackPiece != this)
		{
			if(boundingRadius < GameController.SharedInstance.AlphaCullDistance)
			{
				float distSqr = Vector3.SqrMagnitude(GameCamera.SharedInstance.CachedPosition-CachedPosition);
				float testDistSqr = GameController.SharedInstance.AlphaCullDistance - boundingRadius;
				testDistSqr *= testDistSqr;
				useOpaque = (distSqr < testDistSqr);
			}
			else
			{
				useOpaque = false;
			}
		}
		else
		{
			useOpaque = true;
		}
		
		if(bUsingOpaque!=useOpaque)
		{
			UpdateRendererMaterials(useOpaque);
		}
	}

	public static void UpdateRendererMaterials(GameObject rendererParent, bool useOpaque)
	{
		GameController.EnvironmentMaterials materials = null;
		if(useOpaque)
		{
			materials = GameController.SharedInstance.TrackMaterials;
		}
		else
		{
			materials = GameController.SharedInstance.TrackFadeMaterials;
		}
		
		string[] extraTags = {
			"extra1", "extra2", "extra3", "extra4", "extra5"
		};
				
		GameController.DeviceGeneration device = GameController.SharedInstance.GetDeviceGeneration();
		foreach(Renderer r in rendererParent.GetComponentsInChildren<MeshRenderer>())
		{
			int isExtra = -1;
			for(int i = 0; i < materials.Extra.Length; ++i)
			{
				if((materials.Extra[i]!= null) && (r.gameObject.name.Contains(extraTags[i])))
				{
					isExtra = i;
					break;
				}
			}
			bool isDecal = r.gameObject.name.Contains("decals");
			//bool isForce = r.gameObject.name.Contains("force");
			//bool isTornado = r.gameObject.name.Contains("tornado");
			//Ensure that any decals are on the "decal" layer (ignores shadow)
			if(r.gameObject.tag != "DontChangeMat")
			//if(!(r.gameObject.name.Equals("water")||r.gameObject.name.Equals("waterfall")))
			{
				if((device == GameController.DeviceGeneration.iPhone4)||(device == GameController.DeviceGeneration.iPodTouch4))
				{
					bool isLoMesh = r.gameObject.name.StartsWith("decals_lomesh");
					if(isLoMesh)
					{
						isDecal = false;
					}
				}
				if(isExtra>=0)
				{
					r.material = materials.Extra[isExtra];
				}
				else
				if(isDecal)
				{
					r.material = materials.Decal;
				}
				else
				{
					r.material = materials.Opaque;
					r.gameObject.layer = shadowLayer;
				}				
			}
		}
	}
	
	private void UpdateRendererMaterials(bool useOpaque)
	{	
		GameController.EnvironmentMaterials materials = null;
		if((trackPieceDefinition != null) && (trackPieceDefinition.IsTransitionTunnel))
		{
			if(useOpaque)
			{
				materials = GameController.SharedInstance.TunnelMaterials;
			}
			else
			{
				materials = GameController.SharedInstance.TunnelFadeMaterials;
			}
		}
		else
		if((trackPieceDefinition != null) && (trackPieceDefinition.IsBalloon))
		{
			
			if(useOpaque)
			{				
				materials = GameController.SharedInstance.BalloonMaterials;
			}
			else
			{
				materials = GameController.SharedInstance.BalloonFadeMaterials;
			}
		}		
		else
		if(useOpaque)
		{
			materials = GameController.SharedInstance.TrackMaterials;
		}
		else
		{
			materials = GameController.SharedInstance.TrackFadeMaterials;
		}
		if(OpaqueRenderers != null && OpaqueRenderers.Count > 0)
		{
			for(int i=0;i<OpaqueRenderers.Count;i++)
			{
				Renderer r = OpaqueRenderers[i];
				
				if(r == null)
					continue;
				
				r.material = materials.Opaque;
			}
		}
		if(DecalRenderers != null && DecalRenderers.Count > 0)
		{
			for(int i=0;i<DecalRenderers.Count;i++)
			{
				Renderer r = DecalRenderers[i];
				
				if(r == null)
					continue;
				
				r.material = materials.Decal;
			}
		}
		if(ExtraRenderers != null)
		{
			for(int ri = 0; ri<ExtraRenderers.Length; ++ri)
			{
				if(materials.Extra[ri] != null && ExtraRenderers[ri] != null && ExtraRenderers[ri].Count > 0)
				{
					for(int i=0;i<ExtraRenderers[ri].Count;i++)
					{
						Renderer r = ExtraRenderers[ri][i];
						
						if(r == null)
							continue;
						
						r.material = materials.Extra[ri];
					}
				}
			}		
		}
		bUsingOpaque = useOpaque;
	}
		
	public bool IsTurn()
	{
		return TrackBuilder.IsTurnType(trackPieceDefinition);
	}
	
	public bool IsTurnLeft()
	{
		return TrackBuilder.IsTurnLeftType(trackPieceDefinition);
	}
	
	public bool IsTurnRight()
	{
		return TrackBuilder.IsTurnRightType(trackPieceDefinition);
	}
	
	public bool IsSlightLeft()
	{
		return TrackBuilder.IsSlightLeftType(trackPieceDefinition);
	}
	
	public bool IsSlightRight()
	{
		return TrackBuilder.IsSlightRightType(trackPieceDefinition);
	}
	
	public bool IsJunction()
	{
		return TrackBuilder.IsJunctionType(trackPieceDefinition);
	}
	
	public bool IsStairs()
	{
		return TrackBuilder.IsStairType(trackPieceDefinition);
	}
	
	public bool IsMine()
	{
		return TrackBuilder.IsMineType(trackPieceDefinition);
	}
	
	public bool IsObstacle()
	{
		return TrackBuilder.IsObstacleType(trackPieceDefinition);
	}
	
	public bool IsJumpOver()
	{
		return TrackBuilder.IsJumpOverType(trackPieceDefinition);
	}
	
	public bool IsAttackingBaboon()
	{
		return TrackBuilder.IsAttackingBaboonType(trackPieceDefinition);
	}
	
	//Gap
	public bool IsGap()
	{
		return TrackBuilder.IsGapType(trackPieceDefinition);
	}

	public bool IsBalloon()
	{
		return TrackBuilder.IsBalloonType(trackPieceDefinition);	//Only function that doesn't support TrackPieceTypeDefinition
	}
	
	public bool IsStumble()
	{
		return TrackBuilder.IsStumbleType(trackPieceDefinition);
	}
	
	public bool IsTransitionTunnel()
	{
		return TrackBuilder.IsTransitionTunnelType(trackPieceDefinition);
	}
	
	public bool IsSlideUnder()
	{
		return TrackBuilder.IsSlideUnderType(trackPieceDefinition);
	}
	
	public bool IsLedge()
	{
		return TrackBuilder.IsLedgeType(trackPieceDefinition);
	}
	
	public bool IsNarrow()
	{
		return TrackBuilder.IsNarrowType(trackPieceDefinition);
	}
	
	
	public void SetRendering(bool enable, bool forceBonusItemsOff = false)
	{
/*		if(BonusItems != null)
		{
			int count = BonusItems.Count;
			for (int i = 0; i < count; i++) {
				if(BonusItems[i] == null)
					continue;
				
				//BonusItems[i].SetVisibility(enable);
			}
		}*/
		
		bool showInvincible = (GamePlayer.SharedInstance.IsInvicible() == true && enable == true) ? true : false;
		bool changeState = true;
		
		//-- if the state didn't change, do nothing.
		if (enable == (LastRenderState == RenderState.Enabled))
		{
			changeState = false;
		}

		if(showInvincible != (LastInvRenderState == RenderState.Enabled))
		{
			changeState = true;
		}
		
		if(changeState == false)
			return;
		
		
		/*if(BonusItems != null)
		{
			foreach(BonusItem bi in BonusItems)
			{
				//bi.SetVisibility(enable);
			}
		}*/
		
		
		
		/*if(TrackPieceRenderers != null && TrackPieceRenderers.Count > 0  && bUsingOpaque)
		{
			//Renderer r = null;
			int max = TrackPieceRenderers.Count;
			for (int i = 0; i < max; i++) {
				cachedRenderer = TrackPieceRenderers[i];
				if(cachedRenderer == null)
					continue;
				if(enable == true)
				{
					cachedRenderer.material = GameController.SharedInstance.TrackPieceAlphaMaterial;
				}
				//cachedRenderer.enabled = enable;
			}
			}
			if(enable)
				bUsingOpaque = false;
		}*/
		
		gameObject.SetActiveRecursively(enable);
			
		//ActivateBonusItems(enable);
		
		
		if(GamePlayer.SharedInstance != null) {
			
			if(trackPieceData != null) {
				if(trackPieceData.InvincibleObjects != null) {
					int max = trackPieceData.InvincibleObjects.Count;
					for (int i = 0; i < max; i++) {
						if(trackPieceData.InvincibleObjects[i] == null)
							continue;
						trackPieceData.InvincibleObjects[i].gameObject.active = showInvincible;
					}
				}
				//-- Force turn off for low end ?
	//			if(QualitySettings.GetQualityLevel() <= 0) {
	//				showInvincible = false;
	//			}
				if(trackPieceData.InvincibleEffects != null) {
					int max = trackPieceData.InvincibleEffects.Count;
					for (int i = 0; i < max; i++) {
						if(trackPieceData.InvincibleEffects[i] == null)
							continue;
						trackPieceData.InvincibleEffects[i].gameObject.active = showInvincible;
					}
				}
			}	
		}
		
		
			
		//if (enable == true) 
		{
			
			
//			ParticleSystem[] childParticleSystems = GetComponentsInChildren<ParticleSystem>();
//			foreach(ParticleSystem ps in childParticleSystems)
//			{
//				notify.Debug ("ps {0}", ps);
//				if(enable == true)
//				{
//					ps.Play();
//				}
//				else{
//					ps.Stop();
//				}
//			}
		}

		LastRenderState = enable ? RenderState.Enabled : RenderState.Disabled;
		LastInvRenderState = showInvincible ? RenderState.Enabled : RenderState.Disabled;
//Profiler.EndSample();
	}
	
	public void RecursiveVisibility(bool activeFlag, bool isRootNode, int turnCount = 0, bool forceOff = false, int depth = 0)
	{
		//bool forceBonusItemsOff = false;
		if(this == GamePlayer.SharedInstance.OnTrackPiece)
		{
			activeFlag = true;
		}
		else if(forceOff == true)
		{
			activeFlag = false;
		}
		else
		{
			Transform transform = GameCamera.SharedInstance.CachedTransform;
			Vector3 camPosition = transform.position;
			float distSqr = Vector3.SqrMagnitude(camPosition-CachedPosition);
			float testSqr = GameController.SharedInstance.showCullDistance + CurrentTrackPieceData.GetTrackPieceBounds();//*GameController.SharedInstance.ShowCullDistance;
			testSqr *= testSqr;
			//			if(GamePlayer.SharedInstance.IsOnMineCart == true)
//			{
//				testSqr = Mathf.Infinity;
//			}
			if(distSqr > testSqr)
			{
				forceOff = true;
				activeFlag = false;
			}
			else
			{
//				if(distSqr > (testSqr*0.5f))
//				{
//					forceBonusItemsOff = true;
//				}
				
				if (activeFlag == true) 
				{
					if( IsTurn() ||// TrackType == PieceType.kTPMineCurve ||// TrackType == PieceType.kTPMineCurveRight ||
						(	depth > 3 &&
							PreviousTrackPiece != null && 
							TrackBuilder.IsCurveType(trackPieceDefinition) && 
							TrackBuilder.IsCurveType(PreviousTrackPiece.trackPieceDefinition)
						)
					 )
					{
						//NOTE: If this piece is a junction, these statements should cancel each other out. This is correct, because we handle this below.
						if(IsTurnLeft() && !IsJunction() && !isRootNode)
						{
							turnCount++;
						}
						if(IsTurnRight() && !IsJunction() && !isRootNode)
						{
							turnCount--;
						}
						
					}
				}	
			}
			
			//-- If turnCount is now 2, then turn off everything behind it.
			if (Mathf.Abs(turnCount) >= 2)
			{
				//activeFlag = false;
				forceOff = true;
				if(GamePlayer.SharedInstance.DEBUG_TRACKPIECEVIS == true)
				{
					notify.Debug ("Forcing off because of more of then 2 turns. {0}", this);
				}
			}
		}
		
		if(GamePlayer.SharedInstance.DEBUG_TRACKPIECEVIS == true)
		{
			//notify.Debug("RecursiveVisibility({0}) - a({1}) - t({2}) - ltf - {3} - f({4})", this, activeFlag, turnCount, lastTurnWasLeft, forceOff);	
		}
		
		if(activeFlag == true)
		{
			//Minecart: Not in Oz
			/*if(TrackType == PieceType.kTPMineExit)
			{
				//notify.Debug ("{0} ON I'm turning the lights on", this);
				GameController.SharedInstance.SetDaylight(1.0f);
			}
			else if(TrackBuilder.SharedInstance.IsMineType(TrackDef) == true && GameController.SharedInstance.Player.IsOnMineCart == true)
			{
				//notify.Debug ("{0} OFF I'm turning the lights OFF", this);
				GameController.SharedInstance.SetDaylight(0.0f);
			}
			else */if(turnCount == 0 && GameController.SharedInstance.Player.IsFalling == false)
			{
				//notify.Debug ("{0} ON I'm turning the lights on", this);
				GameController.SharedInstance.SetDaylight(1.0f);
			}
		}
		
		if(!gameObject.active)
			SetRendering(activeFlag, forceOff);
		
		if(NextTrackPiece != null)
		{
			if(IsJunction() && !isRootNode)
				NextTrackPiece.RecursiveVisibility(activeFlag, false, turnCount+1, forceOff, depth+1);
			else
				NextTrackPiece.RecursiveVisibility(activeFlag, false, turnCount, forceOff, depth+1);
		}
		if(Alternate_NextTrackPiece != null)
		{
			if(IsJunction() && !isRootNode)
				Alternate_NextTrackPiece.RecursiveVisibility(activeFlag, false, turnCount-1, forceOff, depth+1);
			else
				Alternate_NextTrackPiece.RecursiveVisibility(activeFlag, false, turnCount, forceOff, depth+1);
		}
	}
	
	public void RecursiveRemove()
	{
		//notify.Debug ("RR {0}", this.name);
		//if((NextTrackPiece != null) || (Alternate_NextTrackPiece != null))
		{
			if(NextTrackPiece != null)
			{
				//notify.Debug (" RR NTP {0}", NextTrackPiece.name);
				NextTrackPiece.RecursiveRemove();
				NextTrackPiece = null;
			}
			if(Alternate_NextTrackPiece != null)
			{
				//notify.Debug (" RR ALT_NTP {0}", Alternate_NextTrackPiece.name);
				Alternate_NextTrackPiece.RecursiveRemove();
				Alternate_NextTrackPiece = null;
			}
		}
		//name += "[dead]";
		if((trackPieceDefinition != null)&&(TrackBuilder.IsJunctionType(trackPieceDefinition)))
		{
			--TrackBuilder.SharedInstance.ActiveJunctions;
		}
		
		ClearMeshPieces();
		DestroySelf();
		
		//notify.Debug ("RR {0} ClearMeshPieces_ResetGeneratedData", this.name);
		ResetGeneratedData();
	}
	
	public bool CanTurnLeft()
	{
		return TrackBuilder.IsTurnLeftType(trackPieceDefinition);
	}
	
	public bool CanTurnRight()
	{
		return TrackBuilder.IsTurnRightType(trackPieceDefinition);
	}
	
	public bool CloseEnoughToTurn(Vector3 position) {
		TrackPiece currentPiece = this;
		while(TrackBuilder.IsTurnType(currentPiece.trackPieceDefinition) == false) {
			currentPiece = currentPiece.NextTrackPiece;
			if(currentPiece == null)
				break;
		}
		
		if(currentPiece == null)
		{
			//notify.Debug ("CloseEnoughToTurn on {0}, returning null.", this);
			return false;
		}
			
		
		float dSqr = GameController.SharedInstance.MaxTurnDistance*GameController.SharedInstance.MaxTurnDistance;
		position -= currentPiece.getTurnCenter();
		float d = position.sqrMagnitude;
		if(d < dSqr)
		{
			//notify.Debug ("CloseEnoughToTurn on {0}, d({1}) < dSqr({2}).", this, d, dSqr);
			return true;
		}
		
		//notify.Debug ("NOT CloseEnoughToTurn on {0}, d({1}) < dSqr({2}).", this, d, dSqr);
		return false;
	}
	
	private Vector3 getTurnCenter()
	{
		if(GeneratedPath != null && GeneratedPath.Count == 3) {
			return GeneratedPath[1];
		}
		//-- return the object center if this isn't a turn. this is the error value.
		return CachedTransform.position;
	}
	
	public bool AddBonusItems()
	{
		// Make sure the bonus item list isn't null
		if (BonusItems == null) {
			BonusItems = new List<BonusItem>();
		}
		
		// Don't allow bonus items for a variety of reasons
		if(GeneratedPath == null 
			|| GeneratedPath.Count == 0 
			|| DistanceSinceLastTurn < BonusItemProtoData.SharedInstance.MinDistanceAfterTurn
			|| IsTurn()
			|| IsStairs ()
			|| NextTrackPiece.IsSlideUnder()
			|| NextTrackPiece.IsJumpOver()
			|| (!GameController.SharedInstance.IsTutorialMode && NextTrackPiece.IsGap())
			|| IsAttackingBaboon()
			|| IsBalloon()
			|| IsSlideUnder()
			|| GamePlayer.SharedInstance.HasBoost
			|| PreviousTrackPiece.IsSlideUnder()
			|| BonusItems.Count != 0
			|| BonusItemProtoData.SharedInstance.AllowBonusItems == false
			|| BonusItemProtoData.SharedInstance.ProbabilityBonusItem <= Mathf.Epsilon) {

			return false;
		}
		
		// Create a list of possible bonus items allowed
		BonusItem.BonusItemType itemType = BonusItem.BonusItemType.None;

		int availableItemCount = 0;
		BonusItem.BonusItemType[] availableItemTypes = new BonusItem.BonusItemType[6];
		
		
		//Made this simpler by taking out the probabilities that arent used.
		if (BonusItemProtoData.SharedInstance.ProbabilityCoinBonus > Random.value) {
			availableItemTypes[availableItemCount] = BonusItem.BonusItemType.MegaCoin;
			availableItemCount++;
		}
		
		
		if (BonusItemProtoData.SharedInstance.ProbabilityVacuum > Random.value) {
			availableItemTypes[availableItemCount] = BonusItem.BonusItemType.Vacuum;
			availableItemCount++;
		}
		
		if (GamePlayer.SharedInstance.HasFastTravel == false)
		{
			// don't show the boost or poof power up when you are fast travelling, 
			if (BonusItemProtoData.SharedInstance.ProbabilityBoost > Random.value) {
				availableItemTypes[availableItemCount] = BonusItem.BonusItemType.Boost;
				availableItemCount++;
			}
			
			if (BonusItemProtoData.SharedInstance.ProbabilityPoof > Random.value) {
			availableItemTypes[availableItemCount] = BonusItem.BonusItemType.Poof;
			availableItemCount++;
			}
		}
		
		if (BonusItemProtoData.SharedInstance.ProbabilityScoreBonus > Random.value) {
			availableItemTypes[availableItemCount] = BonusItem.BonusItemType.ScoreBonus;
			availableItemCount++;
		}

		//--Chance to bonus even dropping.
		//float chanceToPlaceBonusItem = Random.value;
		
		//Calculate chance for Gem seperately (or if we are in a balloon section
		if(Random.value < GameProfile.SharedInstance.GetGemChance()
				&& DistanceSinceLastGem > BonusItemProtoData.SharedInstance.MinDistanceBetweenGems + gemReductionValue
				&& !GameController.SharedInstance.IsTutorialMode 
				&& !IsBalloon()
				&& GamePlayer.SharedInstance.OnTrackPiece.TrackType != TrackPiece.PieceType.kTPTransitionTunnelMiddle
				&& GamePlayer.SharedInstance.OnTrackPiece.TrackType != TrackPiece.PieceType.kTPTransitionTunnelMiddle2) 
		{
			itemType = BonusItem.BonusItemType.Gem;		
			DistanceSinceLastGem = 0;

		}
		else if(Random.value < BonusItemProtoData.SharedInstance.ProbablityTornadoBonus
				&& DistanceSinceLastTornadoToken > BonusItemProtoData.SharedInstance.MinDistanceBetweenTornadoTokens
				&& GameProfile.SharedInstance.Player.numberChanceTokensThisRun < BonusItemProtoData.SharedInstance.MaxChanceTokensPerRun
				&& GameProfile.SharedInstance.Player.GetNumberChanceTokens() < 2
				&& !GameController.SharedInstance.IsTutorialMode 
				&& !IsBalloon()
				&& GamePlayer.SharedInstance.OnTrackPiece.TrackType != TrackPiece.PieceType.kTPTransitionTunnelMiddle
				&& GamePlayer.SharedInstance.OnTrackPiece.TrackType != TrackPiece.PieceType.kTPTransitionTunnelMiddle2)
		{
			itemType = BonusItem.BonusItemType.TornadoToken;
			DistanceSinceLastTornadoToken = 0;
		}
		else if (Random.value < BonusItemProtoData.SharedInstance.ProbabilityBonusItem
				&& availableItemCount>0 
				&& !IsBalloon()
				&& DistanceSinceLastBonusItem > BonusItemProtoData.SharedInstance.MinDistanceBetweenBonusItems
				&& GamePlayer.SharedInstance.OnTrackPiece.TrackType != TrackPiece.PieceType.kTPTransitionTunnelMiddle
				&& GamePlayer.SharedInstance.OnTrackPiece.TrackType != TrackPiece.PieceType.kTPTransitionTunnelMiddle2) 
		{
			itemType = availableItemTypes[Random.Range(0, availableItemCount)];
			DistanceSinceLastBonusItem = 0;
		}
		else
			return false;	

		//-- Create the actual bonus item
		BonusItem item = BonusItem.Create(itemType);
		if(item == null)
		{
			notify.Warning("Item Type is null!");
			return false;
		}
		
		//Pickup Spawn location
		float heightAboveGround = IsBalloon() ? 100f : BonusItemProtoData.SharedInstance.BonusItemPlacementHeight;
		int middleIndex = GeneratedPath.Count / 2;
		Vector3 location = GeneratedPath[middleIndex];		// Stick it in the middle of the piece
		//Vector3 location = GeneratedPath[0];				//Spawn at the beginning 
		location.y += heightAboveGround;
		item.transform.position = location;
		
		item.setShadowEnabled(true);
		location.x = 0.0f;
		location.z = 0.0f;
		location.y = -heightAboveGround + .15f; 
		item.setShadowLocalPosition(location);
		
		int startIndex = middleIndex - 1;
		if(startIndex < 0) 
			startIndex = 0;
		
		if(startIndex != middleIndex) {
			location = GeneratedPath[startIndex];
			location -= GeneratedPath[middleIndex];
			location.Normalize();
			item.transform.forward = location;
		}
		//item.SetVisibility(true);
		
		BonusItems.Add(item);
		
		return true;
	}
	
	
	public BonusItem.BonusItemType CoinTypeA
	{
		get { if(BonusItemProtoData.SharedInstance.AllowTripleCoins)
				return BonusItem.BonusItemType.CoinTriple;
			  return BonusItem.BonusItemType.Coin;
		}
	}
	public BonusItem.BonusItemType CoinTypeB
	{
		get { if(BonusItemProtoData.SharedInstance.AllowDoubleCoins)
				return BonusItem.BonusItemType.CoinDouble;
			  if(BonusItemProtoData.SharedInstance.AllowTripleCoins)
			  	return BonusItem.BonusItemType.CoinTriple;
			  return BonusItem.BonusItemType.Coin;
		}
	}
	public BonusItem.BonusItemType CoinTypeC
	{
		get {
			return BonusItem.BonusItemType.Coin;
		}
	}
	
	
	public bool AddCoinsToTrack()
	{
		CoinPlacement placementChoice = CoinPlacement.Center;
		
		//-- Choose coin horiz placement if this is a new run.
		if(CoinRunCoinCount == 0) {
			if (TrackBuilder.IsAnyLeftLedgeType(trackPieceDefinition))
				placementChoice = CoinPlacement.Left;
			else if (TrackBuilder.IsAnyRightLedgeType(trackPieceDefinition))
				placementChoice = CoinPlacement.Right;
			else
				placementChoice = (CoinPlacement)Random.Range(0, (int)CoinPlacement.Right + 1);
			
		} else {
			placementChoice = LastCoinPlacement;
		}
		
		//If there is an obstacle spawner, make sure it's ok with the coin placement
		SpawnEnemyFromPiece spawner = GetComponentInChildren<SpawnEnemyFromPiece>();
		if(spawner!=null)
		{
			CoinLanePermission permission = spawner.CurrentCoinPermission;
			
			if(permission==CoinLanePermission.None)	return false;
			
			switch(permission)
			{
			case CoinLanePermission.LeftOnly: placementChoice = CoinPlacement.Left; break;
			case CoinLanePermission.RightOnly: placementChoice = CoinPlacement.Right; break;
			}
		}
		
		return AddCoinsToTrack(placementChoice);
	}
	
	private float LastArcT = -1f;
	
	public bool AddCoinsToTrack(CoinPlacement placementChoice)
	{			
		// Make sure the bonus item list isn't null
		if (BonusItems == null) {
			BonusItems = new List<BonusItem>();
		}
		
		if (Settings.GetBool("allow-coins", true) == false)
		{
			// coins affect performance, especially if there's a lot of them
			return false;
		}
		
		if (trackPieceDefinition.IsLedgeLeft && placementChoice != CoinPlacement.Left)
			return false;
		if (trackPieceDefinition.IsLedgeRight && placementChoice != CoinPlacement.Right)
			return false;
		
		if(trackPieceDefinition.IsTurnRight && placementChoice==CoinPlacement.Right)
			return false;
		if(trackPieceDefinition.IsTurnLeft && placementChoice==CoinPlacement.Left)
			return false;
		
		if(trackPieceDefinition.IsCoinBlock)
			return false;
		
		if(CoinRunCoinCount>0 && placementChoice!=LastCoinPlacement)
			return false;
		
		if (GeneratedPath == null 
			//|| GeneratedPath.Count == 0
			//|| IsTurn()
			//|| IsJumpOver()
			//|| IsStumble()
			|| (BonusItems.Count != 0 && !IsBalloon())
			//|| BonusItemProtoData.SharedInstance.ProbabilityCoinBonus <= Mathf.Epsilon
			|| BonusItemProtoData.SharedInstance.AllowCoins == false
			|| GeneratedPath.Count < 2) {
			return false;
		}
		
		if(DistanceSinceLastCoinRun < BonusItemProtoData.SharedInstance.MinDistanceBetweenCoinRuns && !IsBalloon())
		{
			return false;
		}
		
		//Get spline, if any, and include inactive objects (only works with "plural" component getter)
		SplineNode[] splines = GetComponentsInChildren<SplineNode>(true);
		SplineNode spline = null;
		if(splines.Length>0)
			spline = splines[0];
		
		//-- Divide by one more than the number of coins we are putting down.
		float distanceBetweenCoins = IsBalloon() ? GameProfile.SharedInstance.DistBetweenBalloonCoins :
												   GameProfile.SharedInstance.DistBetweenCoins;

		//-- Declare variables before the loops.
		float traveledDistance = 0.0f;
		float currentSegmentLength = 0.0f;
		float nextCoinDistance = distanceBetweenCoins*0.75f;	//INITIAL value. Keeps changing to define the distance from the start of the track
		Vector3 currentSegmentDirection = new Vector3(0,0,0);
		Vector3 currentSegmentRight = new Vector3(1,0,0);
		Vector3 coinLocation = new Vector3(0,0,0);
		float maxHorzOffset = 0.85f;
	//	float heightAboveGround = LastCoinPlacementHeight;
		
		//float horizOffset = 0.0f;
		//float LeftCenterRightChance = Random.value;
		//float left = 0.0f;
		//float leftChance = 0.33f;
		
		int Lane = 0;
		
		//Get lane placement of coins
		if(placementChoice == CoinPlacement.Left)
			Lane = 0;
		else if(placementChoice == CoinPlacement.Center)
			Lane = 1;
		else if(placementChoice == CoinPlacement.Right)
			Lane = 2;
		
		// If some how we ended up with a placement that doesn't make sense, lets bail
		//Shouldn't need this; we decide this in the function above
		/*if ((TrackBuilder.SharedInstance.IsAnyLeftLedgeType(TrackType) && placementChoice != CoinPlacement.Left)
			|| (TrackBuilder.SharedInstance.IsAnyRightLedgeType(TrackType) && placementChoice != CoinPlacement.Right)
			|| (TrackBuilder.SharedInstance.IsMineType(TrackType) && placementChoice == CoinPlacement.Center))
		{
			return false;
		}*/
		
		//notify.Debug ("AddCoins FILTERD placementChoice={0} Min({1}), Max({2})", placementChoice, MinLane, MaxLane);
		LastCoinPlacement = placementChoice;
		
		//horizOffset = 0.75f*left;
		
		float arcT = LastArcT;
		bool arcUp = LastArcT >= 0f;
		
	//	if(TrackType == PieceType.kTPZipLine)
	//	{
		//	heightAboveGround = 3.2f;
	//		maxHorzOffset = 0.8f;
	//	}
	//	if(IsMine())
	//	{
	//		heightAboveGround = 1.0f;
	//		maxHorzOffset = 0.7f;
	//	}
		if(IsJumpOver() || IsStumble() || IsGap())
		{
			arcUp =  true;	
			arcT = -1;
			LastArcT = arcT;
	//		heightAboveGround = BonusItemProtoData.SharedInstance.ArcMaxHeight;
		}
		
		List<float> coinArcs = new List<float>();
		coinArcs.Add(trackPieceData.arcStartDist);
		SpawnEnemyFromPiece spawner = GetComponentInChildren<SpawnEnemyFromPiece>();
		if(spawner!=null)
		{
			coinArcs = spawner.CurrentCoinArcs;
			arcUp = coinArcs.Count>0;
			arcT = arcUp ? -1 : arcT;
			LastArcT = arcT;
		}
			
		
	//	bool isFirstCoinOnPiece = true;
	//	float firstCoinYLocation = 0;
		
		int targetCoinCount = Random.Range(BonusItemProtoData.SharedInstance.MinCoinsPerRun,BonusItemProtoData.SharedInstance.MaxCoinsPerRun);
		
	//	Debug.Log("Target coin count: "+targetCoinCount);
		
		if(spline==null && !IsBalloon())
		{
			TR.ASSERT(GeneratedPath != null, "GeneratedPath is null");
			TR.ASSERT(GeneratedPath.Count >= 2, "GeneratedPath.Count is less than 2");
			//notify.Debug ("PLACING Coins: {0}, CRC={1}", this, CoinRunCount);
			int i = 0;
			//-- Don't start placing coins on ziplines until well into the piece.
			//-- TODO: 8 is a hack here.  Data drive this.
			/*if(IsAZipLine)
			{
				i = 8;
			}*/
			//Predeclare floats for arcing
			float w = 4f;
			float h = 2f;
			float height = 0f;
			int curArcDistIndex = 0;
			float curArcDist = coinArcs.Count>0 ? coinArcs[0] : 0;
			if(LastArcT>=0f)
				curArcDist = -LastArcT;
		
	//	Debug.Log(gameObject.name + " " + arcUp + " " + curArcDist);
			
			for(; i < (GeneratedPath.Count-1); i++)
			{
				currentSegmentDirection = GeneratedPath[i+1] - GeneratedPath[i];
				currentSegmentLength = currentSegmentDirection.magnitude;
				currentSegmentDirection /= currentSegmentLength;
				
				if((traveledDistance+currentSegmentLength) < nextCoinDistance )
				{
					traveledDistance+=currentSegmentLength;
					continue;
				}
				
				currentSegmentRight = Vector3.Cross(Vector3.up, currentSegmentDirection);
				
				while(nextCoinDistance < (traveledDistance+currentSegmentLength))
				{
					float step = -1.0f;
					if(Lane == 1) step = 0.0f;
					if(Lane == 2) step = 1.0f;
					//for(int Lane=MinLane; Lane<MaxLane; Lane++)
					//{
						//-- Skip the middle lane
					/*	if(placementChoice == CoinPlacement.LeftAndRight && Lane == 1) {
							step += 1.0f;
							continue;
						}*/
						
					coinLocation = currentSegmentDirection * (nextCoinDistance-traveledDistance);
					coinLocation += (currentSegmentRight*step*maxHorzOffset);
					coinLocation += GeneratedPath[i];
					step += 1.0f;
					
					//BonusItem coin = null;
					
					//-- Handle Arc
					if(arcUp && arcT<0f && nextCoinDistance>=curArcDist) {
						arcT = 0f;
						if(targetCoinCount<12)
							targetCoinCount = 12;	//Make sure we hav an arc otherwise it never happens!
					}
				
					//Determine how high this coin should be based on arc shape
					if(arcT>=0f)
					{
						arcT = nextCoinDistance - curArcDist;
						
						if(arcT < w*2f)
						{
							height = -h*((arcT - w)/w)*((arcT - w)/w) + h;
							coinLocation.y += height + 0.5f;
						}
						else
						{
							curArcDistIndex++;
							if(coinArcs.Count <= curArcDistIndex)
							{
								arcT = -1f;
								arcUp = false;
							}
							else
							{
								curArcDist = coinArcs[curArcDistIndex];
								arcT = -1f;
							}
							coinLocation.y += 0.5f;
						}
					}
					else
						coinLocation.y += 0.5f;
					
					LastArcT = arcT;
					
					/////SPAWN THE COIN!/////
					SpawnACoin(coinLocation,height+0.45f,arcT<0f);
						
					//}
					if(CoinRunCoinCount >= targetCoinCount)
					{
						break;
					}
					//-- Move to next coin location.
					nextCoinDistance+=distanceBetweenCoins;	
				}
				
				if(CoinRunCoinCount >= targetCoinCount)
				{
					DistanceSinceLastCoinRun = 0;
					LastArcT = -1f;
					return true;
				}
				traveledDistance+=currentSegmentLength;
			}
			
		}
		else if (spline!=null)
		{
			//A hack to get the coins at the right height for pickup
			for(int i=0;i<splines.Length;i++)
			{
				Vector3 temp = splines[i].transform.position;
				temp.y = PathLocations[0].position.y + 0.5f;
				splines[i].transform.position = temp;
			}
			
			SplineNode mySp = spline;
			
			float t=0.01f;
			int index = 1;
			while(t<1f)
			{
				Vector3 coinLoc = mySp.Bezier(t);
				
				SpawnACoin(coinLoc,coinLoc.y-transform.position.y);
				
				t += 1f/mySp.BezierTangent(t).magnitude;
				
				if(t>=1f && mySp.next!=null && mySp.next.next!=null)
				{
					index++;
					t -= 1f;
					mySp = mySp.next;
					if(mySp == spline)	break;	//Just in case there is a circular spline
				}
			}
		//	Debug.Log(index);
			
			if(CoinRunCoinCount >= targetCoinCount)
			{
				DistanceSinceLastCoinRun = 0;
				LastArcT = -1f;
				return true;
			}
			traveledDistance+=currentSegmentLength;
		}
		
		//Debug.Log("Stopped at: "+CoinRunCoinCount +" / "+targetCoinCount);
		
		//notify.Debug("{0} CoinRunCount={1}, CountSinceLastRun={2}", this, CoinRunCount, TrackBuilder.SharedInstance.CountSinceLastCoinRun);
		
		return true;
	}
	
	//private static int coinsPlaced = 0;
	public GameObject SpawnACoin(Vector3 pos,float height,bool showShadow = true)
	{
		//Determine coin type
		int coinColor = CoinRunCoinCount % 5;
		BonusItem.BonusItemType coinType = BonusItem.BonusItemType.Coin;
		if(coinColor == 2 || coinColor == 3) {
			// Double Value
			coinType = CoinTypeB;
		} else if(coinColor == 4) {
			//Triple Value
			coinType = CoinTypeC;
		} else {
			// Single Value
			coinType = CoinTypeA;
		}
		
		//Create a coin!
		BonusItem coin = BonusItem.Create(coinType);
		
		//Set position and rotation
		coin.transform.eulerAngles = new Vector3(0, -90+(CoinRunCoinCount*36), 0);
		coin.transform.position = pos;
		
						
		
		//Set shadow position
		coin.setShadowEnabled(showShadow);
		pos.x = 0.0f;
		pos.z = 0.0f;
		pos.y = !IsBalloon() ? -height : -100f;
		coin.setShadowLocalPosition(pos);
		
		//coin.SetVisibility(true);
		BonusItems.Add(coin);
		
		CoinRunCoinCount++;
		
		return coin.gameObject;
	}
	
	
}
