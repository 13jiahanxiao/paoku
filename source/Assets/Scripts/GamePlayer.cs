using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class GamePlayer : MonoBehaviour
{
	protected static Notify notify;
	public static GamePlayer SharedInstance;
	
	public enum AnimType
	{
		kRun = 0,
		kJump,
		kSlide,
		kSlideIdle,
		kZiplineEnter,
		kZiplineIdle,
		kZiplineExit,
		kMinecartEnter,
		kMinecartIdle,
		kMinecartExit,
		kMinecartDuck,
		kStumble,

		// added bu oz
		kTurnLeft,
		kTurnRight,
		kWallFail,
		kBalloonEnter,
		kBalloonIdle,
		kBalloonExit,
		kLeftLedge,
		kRightLedge,

		// added by imangi
		kDeathSplat,
		kDeathFall,
		kDeathWaterfall,
		kDeathMineDuck,
		
		kPickedUp,
		kPutDown,
		kFlying,
		kFloating,
		kFloatingEnd,
		kFloatingJump,
		
		kBalloonStumble,
		kBalloonExitFail,
		kBalloonStumbleFail,
		kBalloonPanic,
		kBalloonLeanLeft,
		kBalloonLeanRight,
		
		kAnimTypeTotal
	}
	
	private Transform _cachedTransform = null;
	public Transform CachedTransform {
		get {return _cachedTransform;}
	}
	
	public bool 		DEBUG_FOCUSSCENEVIEW = false;
	public bool			DEBUG_TRACKPIECEVIS = false;
	
	public TrackPiece OnTrackPiece { get;  set; }
	
	public GameObject	BackDropObject = null;
	public float		BackDropOffset = -320.0f;
	public Color		ShadowColor = new Color(1,1,1,1);
	public float		ShadowFadeSpeed = 20;
	
	[System.NonSerialized]
	//public Camera		ShadowCamera = null;
	
	private bool 		HasForce 		= false;
	public bool			Hold { get; set;}
	public bool 		IsDead { get; set; }
	public bool 		IsOnGround { get; private set; }
	public bool 		IsOverGround { get; private set; }
	public bool 		IsOnMineCart { get; private set; }
	public bool 		IsOnBalloon { get; private set; }
	public bool			HasBoost 
	{ 
		get { 
			return mbBonusTypeBoostEffect;}
	}
	public bool			HasVacuum { get; private set; }
	public bool			HasShield { get; private set; }
	public bool			HasPoof { get; private set; }
	public bool			IsFalling { get; private set; }
	public bool			IsOnLedge { get; private set; }
	public bool			IsMegaBoost { get; private set; }
	public bool 		IsHangingFromWire { get; private set; }
	public bool			ClearedZipBase { get; private set; }
	public bool			DidBalloonFail { get; set; }
	public bool			WillBalloonFail { get; set; }
	public bool			NoStumble { get; set; }
	
	//public float 		NoStumbleDistLeft; // currently stumbleproof last forever
	
	public bool			HasFastTravel{ get; private set; }
	public float		FastTravelDistance { get; private set;}
	public int			FastTravelDestinationEnvironmentSetId{ get; private set;}
	
	public float		PlayerXOffset { get; set; }
	public float		PreviousPlayerXOffset {get; set; }
	//private float		SpecialCameraTargetHeight = 0.0f;
	
	[System.NonSerialized]
	public bool			HasSuperBoost = false;


	public float		PlayerGravity = 250.0f;
	public float		MaxJumpVelocity = 2.0f;
	public float		MinJumpVelocity = 1.0f;
	public float		SmoothFollow = 0.05f;
	
	private Vector3		playerVelocity;
	private Vector3		currentPosition;
	private Vector3		currentDirection;
	
	//public int currentSegment = 0;
	
	public float 		TimeSinceLastPowerup { get; private set; }
	public float 		TimeSinceDeath { get; private set; }
	public float		PlayerMagnitude { get; private set; }
	public float		PlayerXZMagnitude { get; private set; }
	public Vector3		PlayerVelocity { get { return playerVelocity; } }
	public Vector3		PlayerForce { get; private set; }
	public Vector3		CurrentPosition { get { return currentPosition; } }
	public Vector3		CurrentDirection { get { return currentDirection; } }
	public Vector3		PreviousLocation { get; private set; }
	public Vector3		StartingPosition { get; private set; }
	
	
	public float playerSpeedMinAfterSaveMe = 12f;
	public float playerSpeedMinAfterHeadstart = 14f;
	public float playerSpeedInTutorial = 16f;
	
	public void SetDirection(Vector3 forward)
	{
		currentDirection = forward;
	}
	
	public int			MinPiecesBeforeAdding = 5;
	public int			MinPieceHistoryToKeep = 3;
	public float		PieceDistanceToAdd = 20;
	private float       _pieceDistanceToAdd;
	public float		TunnelPieceDistanceToAdd = 45;
	public float		BonusItemActivateDistance = 30;
	
	public float 		CoinCountForBonus;
	public int 			CoinCountForBonusThreshold;
	public int 			balloonDifficulty = 1;
	public float		fogFadeInSpeed = 3.0f;
	public float		fogFadeOutSpeed = 3.0f;
	public float		fogDensityClamp = 0.06f;
	public float		whiteoutFadeInSpeed = 1;
	public float		whiteoutFadeOutSpeed = 1;
	
	// per run
	public int Score { get; private set; }
	public int CoinCountTotal { get; set; }
	public int GemCountTotal { get; private set; }
	public DeathTypes DeathType { get; private set; }
	
	private bool JumpAfterDelay;
	private float JumpDelay;
	private bool IsJumpingUpPhase;
	private float TimeSinceSlideStart;
	private float SlideDuration;
	private float SlideDurationFactor = 0.07f;
	private float StumbleDelay;
	private float TimeSinceStumbleStart;
	private float StumbleDuration;
	private float TimeSinceFallStart;
	private float InvincibilityTimeLeft = 0;
	
	public float DeathByMonkeyTime = 10f;
	
	public bool IsSliding { get; private set; }
	public bool IsDucking { get; private set; }
	public bool IsJumping { get; private set; }
	public bool IsStumbling { get; private set; }
	public bool StumbleAfterDelay { get; private set; }
	public float StumbleKillTimer { get; private set; }
	public float DeathRunVelocity { get; private set; }
	public bool HasInvincibility { get; private set; }
	public bool IsInCatacombs { get; set; }
	
	public bool IsTurningRight { get { return turnRightNextSegment; } }
	public bool IsTurningLeft { get { return turnLeftNextSegment; } }
	public bool JustTurned { get { return justTurned; } }
	
	public List<int> EnvironmentsVisitedThisRun = new List<int>();
	
	public Animation FinleyObject;
	public ShadowQuality Sunlight = null;
	private Renderer finleyRenderer;
	
	//public Projector shadowProjector;
	//public Camera shadowCamera;
	
	//public Transform blobShadow;
	
	public Material coinToMeterMaterial;
	
	//public delegate void FastTravelHandler();	//(bool enabled);
	//public static event FastTravelHandler OnFastTravel;
	
	public PlayerFx playerFx;
	
	//public ParticleSystem transitionTunnelLightning;

	//private Color PoofBlinkStartColor = new Color(1,1,1,1);
	//private Color PoofBlinkEndColor = new Color(1,1,1,1);
	
	public void Resurrect()
	{
		//TR.LOG("Resurrect DeathRunVelocity " + GamePlayer.SharedInstance.DeathRunVelocity);
		//GamePlayer.SharedInstance.SetPlayerVelocity(GamePlayer.SharedInstance.DeathRunVelocity*0.95f);
		
		float newSpeed;
		
		if(GamePlayer.SharedInstance.DeathRunVelocity < GamePlayer.SharedInstance.playerSpeedMinAfterSaveMe)
		{
			newSpeed = GamePlayer.SharedInstance.DeathRunVelocity;
		}
		
		else
		{
			newSpeed = Mathf.Max (GamePlayer.SharedInstance.playerSpeedMinAfterSaveMe,GamePlayer.SharedInstance.DeathRunVelocity*0.925f);
		}

		//float newSpeed = GamePlayer.SharedInstance.DeathRunVelocity * .95f;

		GamePlayer.SharedInstance.SetPlayerVelocity(newSpeed);
		
		//notify.Debug("Resurrect new speed " + newSpeed);
		
		SetPlayerMaterial(true);
		IsDead = false;
		IsOnBalloon = false;
		DidBalloonFail = false;
		dying = false;
		DeathByMonkeyTime = 10f;
		StumbleKillTimer = 7.0f;
		doSetVisibility(true);
		doResetForce();
		
		doSetupCharacter();
		StartGlindasBubble(4f);
		
		if(NoStumble){
			if(playerFx) playerFx.StopStumbleProof();
			AudioManager.SharedInstance.StopStumbleProof();
		}
		
		//Enemy.main.Reset(); // reset enemies
		GameController.SharedInstance.EnemyFollowDistance = 7.5f; // reset enemy distance
		
		ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.Resurrects,1);
		
		Enemy.main.StartChase();
		// set camera
		//GameCamera.SharedInstance.tr
		/*
		iTween.ValueTo(gameObject,iTween.Hash(
			"from", 0f,
			"to", 1f,
			"time", 1f,
			"onupdate", "SetCameraUpdate",
			"onupdatetarget", gameObject));
			*/
		
	}
	/*
	public void SetCameraUpdate(float val){
		notify.Debug("SetCameraUpdate");
		OzGameCamera.SharedInstance.LateUpdate();
	}
	*/
	public void MakeInvincible(float time)
	{
		HasInvincibility = true;
		InvincibilityTimeLeft = time;
	}
	public bool HasGlindasBubble = false;
	private void StartGlindasBubble(float time)
	{
		HasInvincibility = true;
		InvincibilityTimeLeft = time;
		HasGlindasBubble = true;
		if(playerFx) {
			playerFx.StartBubble();
			playerFx.StartBubbleInside();
		}
		doPlayAnimation(AnimType.kFloating,false,false,0f,true,PlayMode.StopAll);
		animateObject.Sample();
		AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_GlindasBubble_01);
		
	}	
	
	bool isEndingBubble = false;
	private void EndGlindasBubble()
	{
		if(isEndingBubble)	return;
		
		if(!IsOnBalloon)
		{
			if(playerFx) {
				playerFx.StopBubble();
				playerFx.StopBubbleInside();
				playerFx.StartBubblePop();
			}	
			
			doPlayAnimation(AnimType.kFloatingEnd);
			doPlayAnimation(AnimType.kRun,true);
			AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_GlindasBubble_off);
		}
		
		isEndingBubble = true;
		
		StartCoroutine(EndGlindasBubble_internal());
	}
	
	IEnumerator EndGlindasBubble_internal()
	{
		yield return new WaitForSeconds(2f);
		
		HasGlindasBubble = false;
		HasInvincibility = false;
				
		if(NoStumble){
			playerFx.StartStumbleProof();	
			AudioManager.SharedInstance.StartStumbleProof();
		}
		
		isEndingBubble = false;
		
		EndOzFlash();
	}
	
	private void ResetGlindasBubbleData()
	{
		if(playerFx) {
			playerFx.StopBubble();
			playerFx.StopBubbleInside();
		}		
		
		HasGlindasBubble = false;
		HasInvincibility = false;		
	}	
	
	public void Hypnotize(float duration)
	{
		notify.Debug("Hypnosis!!!!");
		Time.timeScale = 0.5f;
		//UIManagerOz.SharedInstance.inGameVC.ShowMagicWandVignette();
	}
	public void UnHypnotize()
	{
		notify.Debug("NO HYPNO!");
		Time.timeScale = 1f;
	}

	//hold ballon temp rotation values//
	private float currentBalloonRotation = 0;

	private int WaterColliderLayerMask;
	private int TurnColliderLayerMask;
	private int ObstacleColliderLayerMask;
	private int StumbleColliderLayerMask;
	private int FallSaverColliderLayerMask;
	private int ShadowBoxLayerMask;
	//private int CoinLayerMask;
	//private int MineEntranceMask;
	private int IdolMask;
	private int BalloonEntranceMask;
	private int BalloonFailMask;	
	private SphereCollider PlayerCollider;
	Vector3				oldPlayerColliderCenter;
	
	//private _cachedTransform idolRoot;

	private List<TrackPiece>					OldTrackPieces;
	private Dictionary<AnimType,List<string>>	AnimTypeMap = null;	
	
	
	
	// player is leaving oldTrackPiece, and just stepped on newTrackPiece, oldTrackPiece may be null
	public delegate void OnTrackPieceChangedHandler(TrackPiece oldTrackPiece,  TrackPiece newTrackPiece);
	private static event OnTrackPieceChangedHandler OnTrackPieceChangedEvent = null;
	
	
	//-- This gets seeded from the GameProfile. We should update this local value anytime we collide with a power up or
	//-- MaxSpeed StatType object it applied to the Character/Player.
	private float _MaxRunVelocity;
	
	// -- Some tracking stuff
	private TrackPiece LastTrackPiece = null;
	private float timeSinceLastCountRefesh = 0.0f;
	private int turnCount = 0;
	private int obstacleCount = 0;
	
	private float speedBeforeBalloon = 10f;
	[HideInInspector]
	public int isOnHardSurface = 0;
	
	public bool HasDeathRotation
	{
		get { return false;}
		set {}
	}
	
	public void ParentLightRootTo( Transform transform)
	{	
	}
	
	public void ClearOldPieces() {
		if(OldTrackPieces != null)
		{
			while(OldTrackPieces.Count > 0)
			{
				TrackPiece deadPiece = OldTrackPieces[0];
				OldTrackPieces.RemoveAt(0);
//				deadPiece.name += "[dead]";
				deadPiece.DestroySelf();				
			}
//			OldTrackPieces.Clear();
		}
	}
	
	public void SetMaxRunVelocity(float speed) {
		_MaxRunVelocity = speed;
	}
	
	void Awake()
	{
		SharedInstance = this;
		notify = new Notify(this.GetType().Name);
		//if(!Application.isEditor)
		FillInAnimTypesGuyDAnimsTypes();
		_cachedTransform = transform;
		
		//ShadowCamera = GetComponentInChildren<Camera>();
		
		WaterColliderLayerMask = LayerMask.NameToLayer("waterCollider");
		TurnColliderLayerMask = LayerMask.NameToLayer("turncolliders");
		ObstacleColliderLayerMask = LayerMask.NameToLayer("obstacleColliders");
		StumbleColliderLayerMask = LayerMask.NameToLayer("stumbleColliders");
		FallSaverColliderLayerMask = -1;
		ShadowBoxLayerMask = LayerMask.NameToLayer("Shadowbox");
		//CoinLayerMask = LayerMask.NameToLayer("Coins");
		//MineEntranceMask = LayerMask.NameToLayer("mineentrance");
		BalloonEntranceMask = LayerMask.NameToLayer("balloonentrance");
		BalloonFailMask = LayerMask.NameToLayer("balloonfail");
		
		PlayerCollider = GetComponent<SphereCollider>();
		oldPlayerColliderCenter = PlayerCollider.center;
		
		if(FinleyObject!=null)
			finleyRenderer = FinleyObject.GetComponentInChildren<Renderer>();
		
	}
	
	void FillInAnimTypesGuyDAnimsTypes() 
	{
		if(AnimTypeMap == null) {
			AnimTypeMap = new Dictionary<AnimType,List<string>>();
		}
		AddToAnimTypeMap(AnimType.kRun,"Run01");
		AddToAnimTypeMap(AnimType.kJump,"Jump01");
		AddToAnimTypeMap(AnimType.kJump,"Jump02");
		AddToAnimTypeMap(AnimType.kDeathFall,"Fall01");
		AddToAnimTypeMap(AnimType.kSlide,"SlideEnter01");
		AddToAnimTypeMap(AnimType.kSlide,"SlideEnter02");
		AddToAnimTypeMap(AnimType.kSlideIdle,"Slide01");
		AddToAnimTypeMap(AnimType.kSlideIdle,"Slide02");
		AddToAnimTypeMap(AnimType.kStumble,"RunStumble01");
		AddToAnimTypeMap(AnimType.kTurnRight,"RightTurnSlide01");
		AddToAnimTypeMap(AnimType.kTurnLeft,"LeftTurnSlide01");
		AddToAnimTypeMap(AnimType.kWallFail,"WallFail01");
	//	AddToAnimTypeMap(AnimType.kZiplineEnter,"ZiplineEnter01");
	//	AddToAnimTypeMap(AnimType.kZiplineIdle,"ZiplineIdle01");
	//	AddToAnimTypeMap(AnimType.kZiplineExit,"ZiplineExit01");
	//	AddToAnimTypeMap(AnimType.kMinecartEnter,"MineCartEnter01");
	//	AddToAnimTypeMap(AnimType.kMinecartIdle,"MineCartIdle01");
	//	AddToAnimTypeMap(AnimType.kMinecartExit,"MineCartExit01");
		AddToAnimTypeMap(AnimType.kBalloonEnter,"EnterBalloon01");
		AddToAnimTypeMap(AnimType.kBalloonIdle,"IdleBalloon01");
		AddToAnimTypeMap(AnimType.kBalloonExit,"ExitBalloon01");
		AddToAnimTypeMap(AnimType.kBalloonExitFail,"BalloonExitFail01");
		AddToAnimTypeMap(AnimType.kPickedUp,"Grab01");
		AddToAnimTypeMap(AnimType.kPutDown,"Release01");
		AddToAnimTypeMap(AnimType.kFlying,"Flying01");
		AddToAnimTypeMap(AnimType.kFloating,"Float");
		AddToAnimTypeMap(AnimType.kFloatingEnd,"FloatFall");
		AddToAnimTypeMap(AnimType.kBalloonStumble,"StumbleBalloon01");
		AddToAnimTypeMap(AnimType.kBalloonStumbleFail,"SlideBalloon01");
		AddToAnimTypeMap(AnimType.kBalloonPanic,"PanicBalloon01");
		AddToAnimTypeMap(AnimType.kBalloonLeanLeft,"LeanLeftIdle");
		AddToAnimTypeMap(AnimType.kBalloonLeanRight,"LeanRightIdle");
		AddToAnimTypeMap(AnimType.kLeftLedge,"LeanLeftLedge");
		AddToAnimTypeMap(AnimType.kRightLedge,"LeanRightLedge");
		AddToAnimTypeMap(AnimType.kFloatingJump,"BubbleJump");
	}
			
	void AddToAnimTypeMap(AnimType type, string anim)
	{
		if(AnimTypeMap == null) {
			AnimTypeMap = new Dictionary<AnimType,List<string>>();
		}
		
		if(!AnimTypeMap.ContainsKey(type))
			AnimTypeMap.Add(type,new List<string>());
		AnimTypeMap[type].Add(anim);
	}
	
	//Not in Oz
	/*
	void FillInAnimTypesTR1AnimsTypes() 
	{
		AddToAnimTypeMap(AnimType.kRun,"runEventsA");
		AddToAnimTypeMap(AnimType.kJump,"jumpA");
		AddToAnimTypeMap(AnimType.kSlide,"slidestartA");
		AddToAnimTypeMap(AnimType.kSlideIdle,"slideloopA");
		AddToAnimTypeMap(AnimType.kStumble,"stubmleA");
		AddToAnimTypeMap(AnimType.kZiplineEnter,"ziplinestartA");
		AddToAnimTypeMap(AnimType.kZiplineIdle,"ziplineloopA");
		AddToAnimTypeMap(AnimType.kZiplineExit,"ZiplineExit01");
		AddToAnimTypeMap(AnimType.kMinecartEnter,"jumpA");
		AddToAnimTypeMap(AnimType.kMinecartIdle,"minecart");
		AddToAnimTypeMap(AnimType.kMinecartExit,"jumpA");
	}*/
	
	void OnEnable()
	{
		//TrackPiece.onNextSegment += onNextSegment;
		TrackPiece.onDoTurn += OnTrackPieceTurnEvent;
	}
	
	void OnDisable()
	{
		//TrackPiece.onNextSegment -= onNextSegment;
		TrackPiece.onDoTurn -= OnTrackPieceTurnEvent;
	}
	
	
	bool justTurned = false;
	//-- Fired when we pass a turning point on the TrackPiece.
	void OnTrackPieceTurnEvent(TrackPiece trackPiece, int segmentIndex)
	{
		//TR.LOG ("OnTrackPieceTurnEvent: {0}-{1}", segmentIndex, CurrentSegment);
		if(IsFalling == true || IsOverGround == false)
			return;
		
		if(turnLeftNextSegment == true)
		{
			Vector3 start = OnTrackPiece.GeneratedPath[CurrentSegment];
			Vector3 end = OnTrackPiece.GeneratedPath[CurrentSegment+1];
			end -= start;
			end.Normalize();
			
			//_cachedTransform.Rotate(0, -90, 0, Space.Self);
			_cachedTransform.forward = end;
			currentDirection = end;//_cachedTransform.forward;
			
			float mag = playerVelocity.magnitude;
			playerVelocity = currentDirection;
			playerVelocity*= mag;

			
			doResetForce();
			turnLeftNextSegment = false;
			
			justTurned = true;
			//Debug.Break();
			
			if(!IsJumping && !dying && !HasBoost && (!HasGlindasBubble||isEndingBubble))
				doPlayAnimation(AnimType.kRun, false, false);	
		}
		else if(turnRightNextSegment == true)
		{
			Vector3 start, end;
			if(trackPiece.Alternate_GeneratedPath != null && trackPiece.Alternate_GeneratedPath.Count > 0)
			{
				trackPiece.UseAlternatePath = true;
				start = OnTrackPiece.Alternate_GeneratedPath[CurrentSegment];
				end = OnTrackPiece.Alternate_GeneratedPath[CurrentSegment+1];
			}
			else{
				start = OnTrackPiece.GeneratedPath[CurrentSegment];
				end = OnTrackPiece.GeneratedPath[CurrentSegment+1];
			}
			end -= start;
			end.Normalize();
			
			//_cachedTransform.Rotate(0, 90, 0, Space.Self);
			_cachedTransform.forward = end;
			currentDirection = end;//_cachedTransform.forward;
			
			float mag = playerVelocity.magnitude;
			playerVelocity = currentDirection;
			playerVelocity*= mag;
			
			doResetForce();
			turnRightNextSegment = false;
			
			justTurned = true;
			
			if(!IsJumping && !dying && !HasBoost && (!HasGlindasBubble||isEndingBubble))
				doPlayAnimation(AnimType.kRun, false, false);	
			
		}
		else if(autoTurn == false)
		{
			//-- Didn't turn. go back to previous segment
//			TR.LOG ("Didn't turn. go back to previous segment");
			CurrentSegment = segmentIndex - 1;
			trackPiece.SetCurrentSegmentAndSkipLogic(CurrentSegment);
			//trackPiece.CurrentSegment = segmentIndex - 1;
			//TR.LOG (" did not turn: {0}", trackPiece.CurrentSegment);
		}
	}
	
	public void UpdatePieceDistanceToAdd()
	{
		_pieceDistanceToAdd = PieceDistanceToAdd;
		
		switch(GameController.SharedInstance.GetDeviceGeneration())
		{
		case GameController.DeviceGeneration.Unsupported:
		case GameController.DeviceGeneration.iPhone3GS:
		case GameController.DeviceGeneration.iPhone4:
		case GameController.DeviceGeneration.iPodTouch4:
		case GameController.DeviceGeneration.LowEnd:
			break;
		case GameController.DeviceGeneration.iPodTouch5:
		case GameController.DeviceGeneration.iPhone5:
		case GameController.DeviceGeneration.iPad4:
		case GameController.DeviceGeneration.HighEnd:
			_pieceDistanceToAdd *= 2f;//1.25f;
			break;
		case GameController.DeviceGeneration.iPad2:
		case GameController.DeviceGeneration.iPadMini:
		case GameController.DeviceGeneration.MedEnd:
			_pieceDistanceToAdd *= 1.25f;
			break;
		default:
			_pieceDistanceToAdd *= 1.5f;
			break;
		}
	}
	
	// Use this for initialization
	void Start()
	{
		UpdatePieceDistanceToAdd();

		StartingPosition = _cachedTransform.position;
		DeathRunVelocity = 0;
		CoinCountTotal = 0;
		GemCountTotal = 0;
		CoinCountForBonusThreshold = Settings.GetInt("power-coin-threshold", 50);
		HasInvincibility = false;
		HasGlindasBubble = false;
		InvincibilityTimeLeft = 0.0f;
		balloonDifficulty = 1;
		EnvironmentSetSwitcher.SharedInstance.RegisterForOnEnvironmentStateChange(EnvironmentStateChanged);
	//	playerFx = GetComponent<PlayerFx>();
		
		Reset();
	}
	
	public void ResetCoinCountForBonus() {
		CoinCountForBonus = 0;
		//UIManagerOz.SharedInstance.SetPowerProgress(0.0f);
		if(UIManagerOz.SharedInstance.inGameVC && UIManagerOz.SharedInstance.inGameVC.coinMeter)
			UIManagerOz.SharedInstance.inGameVC.coinMeter.AnimateCoinMeter();
		CharacterStats activeCharacter = GameProfile.SharedInstance.GetActiveCharacter();
		
		//BasePower activePower = null;
		if(activeCharacter != null && PowerStore.Powers != null && activeCharacter.powerID >= 0) {
			PowerStore.PowerFromID(activeCharacter.powerID);	
		}
	}
	
	public void ResetScoreAndCoins(){
		Score = 0;
		CoinCountTotal = 0;
		UIManagerOz.SharedInstance.inGameVC.scoreUI.SetCoinCount(CoinCountTotal);
	}
	
	
	public void Reset()
	{
		if(OldTrackPieces != null)
		{
			while(OldTrackPieces.Count > 0)
			{
				TrackPiece deadPiece = OldTrackPieces[0];
				OldTrackPieces.RemoveAt(0);
//				deadPiece.name += "[dead]";
				deadPiece.DestroySelf();				
			}
			//OldTrackPieces.Clear();
		}
		else
		{
			OldTrackPieces = new List<TrackPiece>();
		}
		
		doSetupCharacter();
			
		
		//--Cache some stat Values that WONT change during a run.
		minSpeed = GameProfile.SharedInstance.GetMinSpeed();
		scoreMultiplier = GameProfile.SharedInstance.GetScoreMultiplier();
		coinMultiplierBoost = (int)GameProfile.SharedInstance.GetCoinMultiplierBoost();
		
			
		if(playerFx) playerFx.StopBoostTrail();
		FinleyRendOff();
		
		Hold = true;
		PlayerXOffset = 0.0f;
		OnTrackPiece = null;
		
		CurrentSegment = 0;
		
		GameProfile.SharedInstance.Player.numberChanceTokensThisRun = 0;
		
		FinleyObject.CrossFade("Release");
		FinleyRendOff();
		
		SetPlayerVelocity(1f);
		
		HasSuperBoost = false;
		boostEnding = false;
		
		
		timeInAir = 0f;
		IsDead = false;
		dying = false;
		TimeSinceDeath = 0;
		DeathType = DeathTypes.Fall;
		IsFalling = false;
		IsJumping = false;
		JumpAfterDelay = false;
		IsSliding = false;
		IsDucking = false;
		TimeSinceSlideStart = 0;
		IsOnGround = false;
		IsOverGround = true;
		IsStumbling = false;
		StumbleAfterDelay = false;
		IsOnMineCart =false;
		IsOnBalloon =false;
		HasFastTravel = false;
		ShowMineCart(IsOnMineCart);
		ResetCoinCountForBonus();
		ShowBalloon(IsOnBalloon,0f);
		notify.Debug("Reset WillBalloonFail " + WillBalloonFail);
		WillBalloonFail = false;
		IsInCatacombs = false;
		
		EnvironmentSetSwitcher.SharedInstance.CancelEnvironmentSetChange();
		
		PoofTimeLeft = 0f;
		InvincibilityTimeLeft = 0f;
		
		mbBonusTypeBoostEffect = false;
		
		VelocityBeforeBoost = getRunVelocity();
		
		NoStumble = false;
		if(playerFx) playerFx.StopStumbleProof();
		if(AudioManager.SharedInstance!=null)	AudioManager.SharedInstance.StopStumbleProof();
		
		balloonDifficulty = 1;
		Score = 0;
		CoinCountTotal = 0;
		GemCountTotal = 0;
		
		LastTrackPiece = null;
		turnCount = 0;
		obstacleCount = 0;
		timeSinceLastCountRefesh = 0.0f;
		
		if (UIManagerOz.SharedInstance != null) 
		{
			if (UIManagerOz.SharedInstance.inGameVC != null) 
			{
				//UIManagerOz.SharedInstance.SetCoinCount(CoinCountTotal);
				UIManagerOz.SharedInstance.inGameVC.scoreUI.SetCoinCount(CoinCountTotal);
			}
		}
		
		ResetBonusTypeEffect(BonusItem.BonusItemType.None);
		
//		IsGroundHeightChangeHigher = false;
//		GroundHeight = 0;

		HasInvincibility = false;
		HasGlindasBubble = false;
		InvincibilityTimeLeft = 0;
//		AngelWingsCount = 0;
//		HasAngelWings = false;
//		AngelWingsTimeLeft = 0;
//		AngelWingsRechargeTimeLeft = 0;
		
		TimeSinceLastPowerup = 0;
		TimeSinceStumbleStart = DeathByMonkeyTime;
		StumbleKillTimer = 7.0f;
		DeathRunVelocity = 0;

//		ApplyPlayerTexture();
		_cachedTransform.position = StartingPosition;
		_cachedTransform.forward = new Vector3(0,0,-1);
		PreviousLocation = _cachedTransform.position;
		currentPosition = StartingPosition;
		doResetForce();
		
		fogTransition = false;
		fogTransitionSign = 0;
		RenderSettings.fogMode = FogMode.ExponentialSquared;
		RenderSettings.fogDensity = 0f;
		RenderSettings.fogStartDistance = 0;
		RenderSettings.fogEndDistance = 0;
		
		SkyboxMaterials skyboxMaterials = GetSkyboxMaterials();
		if(skyboxMaterials) {
			skyboxMaterials.SetMaterial ();
			skyboxMaterials.SetTintValue(0.0f);
		}
		
		if(coinToMeterMaterial && coinToMeterMaterial.HasProperty("_TintValue")) 
			coinToMeterMaterial.SetFloat("_TintValue",0.0f);
				
		TintPlayer(Color.black,0f);
	}
	
	private int scoreMultiplier = 1;
	public void AddScore(int scr, bool ignoreMultiplier = false)
	{
		if (scr > 0) {
			if(ignoreMultiplier){
				Score += scr ;
				ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.Score,scr);
			}
			else{
				Score += (scr * scoreMultiplier);
				ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.Score,scr*scoreMultiplier);
			}
			UIManagerOz.SharedInstance.inGameVC.scoreUI.SetScore(Score);
		}
	}
	
	private int coinMultiplierBoost = 1;
	public void AddCoinsToScore(int count)
	{	
		if (count > 0) 
		{
			//-- Adjust coin count for boosts.
		//	int previousTotal = CoinCountTotal;
			count *= coinMultiplierBoost;
			CoinCountTotal += count;
			
			ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.CollectCoins,count);
			
			if (UIManagerOz.SharedInstance != null) 
			{
				//UIManagerOz.SharedInstance.SetCoinCount(CoinCountTotal);	
				UIManagerOz.SharedInstance.inGameVC.scoreUI.SetCoinCount(CoinCountTotal);
			}
			
			if(AudioManager.SharedInstance != null)
			{
				AudioManager.SharedInstance.PlayCoin();
			}
			
			//We do this in GameController
			//if(previousTotal == 0 && previousTotal != CoinCountTotal) {
				//UIInGameViewController.SetObjectiveDataWithFilter(ObjectiveFilterType.WithoutCoins);
			//}
		}
	}
	public void AddCoinsInstantly(int count){
		CoinCountTotal +=  count;
		
		if(count>0)
			ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.CollectCoins,count);
		
		CoinCountTotal = (int)Mathf.Max(0,CoinCountTotal);
		
		UIManagerOz.SharedInstance.inGameVC.scoreUI.SetCoinCount(CoinCountTotal);
	}
	
	public void SetPowerUsed(float time)
	{
		timePowerWillBeInUse = Time.time + time;
	}
	
	private float timePowerWillBeInUse = 0f;
	private bool IsPowerInUse { get { return Time.time < timePowerWillBeInUse; } }
	public void AddPointsToPowerMeter(float count) {
		//Debug.Log(count + "? "+ IsPowerInUse + " " +timePowerWillBeInUse);
		//if(GameController.SharedInstance.IsTutorialMode){ 
			// in tutorial mode triple the coins since we want to fill up the meter
		//	count *= 2;
		//}
		
		if (count > 0 && !IsPowerInUse) 
		{
		//	Debug.Log(count);
			float lastCoinCountForBonus = CoinCountForBonus; 
			CoinCountForBonus += count;
			
			if (CoinCountForBonus >= CoinCountForBonusThreshold) 
			{
				if(lastCoinCountForBonus < CoinCountForBonusThreshold) {
					//-- We hit the end, turn on power icon.
					GameController.SharedInstance.CreateOrShowPower();
					
					ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.CoinMeterFills,1);
					
					// Add a bunch of points!
					AddScore(500);
				}

				//CoinCountForBonus -= CoinCountForBonusThreshold;
			}
			else {
				//-- Update the progress meter.
				UIManagerOz.SharedInstance.SetPowerProgress((float)CoinCountForBonus / (float)CoinCountForBonusThreshold);
			}
		}
	}
	
	public void AddGemsToScore(int count)
	{
	//	if (count > 0)	Commented out because ResurrectMenu might want to TAKE coins from this pool
	//	{
			//-- Uncomment this once we have an artifact that boosts gems.
			//count *= (int)GameProfile.SharedInstance.GetSpecialCurrencyMultiplierBoost();
			if( GameController.SharedInstance.DistanceTraveled < BonusItemProtoData.SharedInstance.MinDistanceBetweenGems){
				// if you are in tutorial, set gemsTutorial to prevent from cheaters to only play the tutorial and get many gems
				PlayerPrefs.SetInt("GemsTutorial", 1);
			}
			GemCountTotal += count;
			if (UIManagerOz.SharedInstance != null) 
			{
				//UIManagerOz.SharedInstance.SetGemCount(count);	
				UIManagerOz.SharedInstance.inGameVC.scoreUI.SetGemCount(GemCountTotal);
			}
	//	}
	}
	
	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(CurrentPosition, 0.1f);
		Gizmos.DrawLine(CurrentPosition, CurrentPosition+(currentDirection));
		
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(CurrentPosition, CurrentPosition+(PlayerForce));
		
		Gizmos.color = new Color(0,0,1,0.5f);
		Gizmos.DrawSphere(transform.position + (transform.up * 0.25f), 0.25f);
		
	}
	
	CharacterStats activeCharacter = null;
	private int activeCharacterId = 0;
	private Animation animateObject;
	[System.NonSerialized]
	public GameObject CharacterModel;
	private OzModelRefs CharacterModelRefs;
	private Renderer RenderObject;
	//private Material origMaterial;
	//public Material poofMaterial;
	public Footsteps footsteps;
	//private MeshRenderer[] MineCartRenderers = null;
	//private MeshRenderer[] BalloonRenderers = null;
	private GameObject MineCartObject = null;
	public GameObject BalloonObject
	{
		get { return CharacterModelRefs.BalloonRef; }
	}
	//private _cachedTransform BalloonPivot = null;
	//private _cachedTransform MineWheelBackLeft = null;
	//private _cachedTransform MineWheelBackRight = null;
	
	public int ActiveCharacterId { get { return activeCharacterId; } set { activeCharacterId = value; } }
	public Animation AnimateObject { get { return animateObject; } }
	
	public CharacterSounds characterSounds { get; private set; }
	
	public Texture BoostTexture = null;
	//public Texture InvincibleTexture = null;
	
	public Color debugColor = new Color(1,1,1,1);
	public bool ColorDebugging = false;
	
	private float distanceAlongRight = 0f;
	
	
	private void ShowMineCart(bool show)
	{
		if(MineCartObject == null)
			return;
		
		MineCartObject.SetActiveRecursively(show);
	}
	

		private void ShowBalloon(bool show, float delay)
	{
		if(BalloonObject == null) {
			return;
		}
		
		StartCoroutine(Delayed_ShowBalloon(show,delay));
	}
	
	IEnumerator Delayed_ShowBalloon(bool show, float wait)
	{
		if(wait>0f)
			yield return new WaitForSeconds(wait);
		BalloonObject.SetActiveRecursively(show);
	}
	
	public void SetPlayerMaterial(bool transparent) {
		if(GameProfile.SharedInstance == null || RenderObject == null) {
			return;
		}
		
		activeCharacter = GameProfile.SharedInstance.GetActiveCharacter();
		GameProfile.ProtoCharacterVisual protoVisual = GameProfile.SharedInstance.ProtoCharacterVisuals[activeCharacter.protoVisualIndex];
		
		if(protoVisual.simple != null && QualitySettings.GetQualityLevel() == 0) 
		{
			RenderObject.material = protoVisual.simple;	
			finleyRenderer.material = protoVisual.simple_finley;
/*			if(Sunlight)
			{
				Destroy(Sunlight.gameObject);
				Sunlight = null;
			}*/
		}
		else if(protoVisual.opaque != null) {
			//We are not using transparent anymore
			//if(transparent == true && protoVisual.transparent != null) {
			//	RenderObject.material = protoVisual.transparent;	
		//	}else {
				RenderObject.material = protoVisual.opaque;	
		//	}
			finleyRenderer.material = protoVisual.normal_finley;
		}
	}
	
	public void doSetupCharacterDelayed(float time)
	{
		Invoke("doSetupCharacter",time);
	}

	public void doSetupCharacter()
	{

		if(GameProfile.SharedInstance == null) {
			notify.Warning ("GameProfile is null. Not setting up character.");
			return;
		}
		
		activeCharacter = GameProfile.SharedInstance.GetActiveCharacter();
		
		GetComponent<SphereCollider>().radius = activeCharacter.colliderRadius;
		GetComponent<SphereCollider>().center = new Vector3(0f,activeCharacter.colliderRadius + 0.16f,0f);
		
		Sunlight.UpdateBlobShadowScale( activeCharacter.colliderRadius / 0.44f );
		
		if(playerFx!=null)
			playerFx.transform.parent = null;
		
		if(CharacterModel != null) 
		{
			Destroy(CharacterModel);
			CharacterModel = null;
			CharacterModelRefs = null;
			animateObject = null;
			RenderObject = null;
	//		MineCartObject = null;
	//		MineCartRenderers = null;
		}
		//    gai yixia   

		//Debug.LogError("protoVisualIndex======"+activeCharacter.protoVisualIndex);
		//activeCharacter.protoVisualIndex=0;
		GameProfile.ProtoCharacterVisual protoVisual = GameProfile.SharedInstance.ProtoCharacterVisuals[activeCharacter.protoVisualIndex];
		 
	//	TR.LOG ("dsc: {0}", protoVisual.prefab);
		//GameObject prefab = protoVisual.prefab.gameObject; // jonoble removed (DMTRO-2063)
		GameObject prefab = protoVisual.prefab; // jonoble added (DMTRO-2063)
		
		OzModelRefs balloonPrefab = GameProfile.SharedInstance.BalloonPrefab;
		if (prefab != null) 
		{

			//string assetPath = UnityEditor.AssetDatabase.GetAssetPath(prefab);
			//TR.LOG ("Assetpath: {0}", assetPath);
			GameObject go = GameObject.Instantiate(prefab) as GameObject;
			go.transform.parent = _cachedTransform;
			go.transform.localPosition = new Vector3(0, 0.0f, 0);
			go.transform.localRotation = Quaternion.identity;
			CharacterModel = go;
			CharacterModelRefs = go.GetComponent<OzModelRefs>();
			characterSounds = go.GetComponent<CharacterSounds>();
			footsteps = go.transform.GetComponentInChildren<Footsteps>();
			
			
			Animation[] animsInGO = go.GetComponentsInChildren<Animation>();
			foreach (Animation item in animsInGO) {
				if(item == null)
					continue;
				if(item.GetClip("Run01") == null)
					continue;
				animateObject = item;
				break;
			}
			
			if(balloonPrefab!=null)
			{

				 
				OzModelRefs balloon = (OzModelRefs)Instantiate(balloonPrefab,go.transform.position,go.transform.rotation);
				balloon.transform.parent = go.transform;
				
				if(CharacterModelRefs!=null)
				{
					CharacterModelRefs.BalloonRef = balloon.BalloonRef;
					CharacterModelRefs.BalloonPropellorBaseL = balloon.BalloonPropellorBaseL;
					CharacterModelRefs.BalloonPropellorBaseR = balloon.BalloonPropellorBaseR;
					CharacterModelRefs.BalloonPropellorL = balloon.BalloonPropellorL;
					CharacterModelRefs.BalloonPropellorR = balloon.BalloonPropellorR;
				}
			} 
			
			
			playerFx.transform.parent = CharacterModel.transform;
			playerFx.transform.localPosition = Vector3.zero;
			playerFx.transform.localEulerAngles = new Vector3(0,180,0);
			
//			cachedState = null;
//			cachedState = new List<AnimationState>();
//			foreach(AnimationState animState in animateObject.animation) {
//				cachedState.Add (animState);
//			}
			
			RenderObject = CharacterModelRefs.OzRenderer;
			
//			Renderer[] renderObjects = animateObject.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
//			foreach(Renderer robj in renderObjects){	
//				if(robj.gameObject.active){
//					RenderObject = robj;
//					origMaterial = RenderObject.material;
//				}
//			}
			
			//Minecart: not in oz!
		//	MineCartObject = HierarchyUtils.GetChildByName("mine_cart_a_prefab", go);
		/*	if(MineCartObject != null)
			{
				MineCartRenderers = MineCartObject.GetComponentsInChildren<MeshRenderer>();
				GameObject wheelObj = HierarchyUtils.GetChildByName("wheel_back_left", MineCartObject);
				if(wheelObj) MineWheelBackLeft = wheelObj._cachedTransform;
				
				wheelObj = HierarchyUtils.GetChildByName("wheel_back_right", MineCartObject);
				if(wheelObj) MineWheelBackRight = wheelObj._cachedTransform;
			}*/
			//locator
			//BalloonObject = HierarchyUtils.GetChildByName("Balloon1:Oz_Animnations_Ruel_22_noCoat:HotAirBalloon1:locator1", go);
			/*if(BalloonObject != null)
			{
				//BalloonRenderers = BalloonObject.GetComponentsInChildren<MeshRenderer>();
				//GameObject wheelObj = HierarchyUtils.GetChildByName("wheel_back_left", MineCartObject);
				//if(BalloonObject) BalloonPivot = BalloonObject._cachedTransform;
				
				//wheelObj = HierarchyUtils.GetChildByName("wheel_back_right", MineCartObject);
				//if(wheelObj) MineWheelBackRight = wheelObj._cachedTransform;
			}*/
			
//			GameObject magnetGO = GameObject.FindWithTag("Magnet");
//			if(magnetGO)
//			{
//				VacuumEffectObject = magnetGO._cachedTransform;
//			}
//			GameObject boostGO = GameObject.FindWithTag("Boost");
//			if(boostGO)
//			{
//				BoostEffectObject = boostGO._cachedTransform;
//			}
			
			SetPlayerMaterial(false);
			
			FillInAnimTypesGuyDAnimsTypes();
			ShowMineCart(false);
			ShowBalloon(false,0f);
		}
	}
	
	public void doStopAllAnimation()
	{
//		TR.LOG("doStopAllAnimation");
		if(animateObject == null)
			return;
			
		animateObject.Stop();
	}
	
	public int doPlayBalloonAnimation(AnimType animType, bool queued=false, bool blend=true, float fadeTime = 0.2f, bool rewind = true, PlayMode playMode = PlayMode.StopAll)
	{
		string anim = AnimTypeMap[animType][0];
		
		//Also animate the balloon, if needed
		if(animType==AnimType.kBalloonEnter || animType==AnimType.kBalloonIdle || animType==AnimType.kBalloonExit ||
			(animType>=AnimType.kBalloonStumble && animType <= AnimType.kBalloonLeanRight) )
		{
			if(BalloonObject!=null && BalloonObject.animation!=null)
			{
				BalloonObject.animation.CrossFade(anim,fadeTime,playMode);
			}
		}
		
		return 0;
	}
	
	public int doPlayAnimation(AnimType animType, bool queued=false, bool blend=true, float fadeTime = 0.2f, bool rewind = true, PlayMode playMode = PlayMode.StopAll)
	{
	//	if(queued == false)
	//		TR.LOG("Play Animation:"+animType+Time.deltaTime);
	//	else
	//		TR.LOG("Play Queued Animation:"+animType+Time.deltaTime);
		if(animType >= AnimType.kAnimTypeTotal || AnimTypeMap == null)
			return 0;
		
		if(!AnimTypeMap.ContainsKey(animType))
		{
			notify.Warning("No animation of type " + animType + " registered on GamePlayer!");
			return 0;
		}
		
		int index = Random.Range(0,AnimTypeMap[animType].Count);
		
		return doPlayAnimation(animType,index,queued,blend,fadeTime,rewind,playMode);
	}
	
	public int doPlayAnimation(AnimType animType, int ind, bool queued=false, bool blend=true, float fadeTime=0.2f, bool rewind = true, PlayMode playMode = PlayMode.StopAll)
	{
//		if(queued == false)
//			TR.LOG("Play Animation:"+anim+Time.deltaTime);
//		else
//			TR.LOG("Play Queued Animation:"+anim+Time.deltaTime);
		if(animType >= AnimType.kAnimTypeTotal || AnimTypeMap == null)
		{
			return 0;
		}
		
		if(ind>=AnimTypeMap[animType].Count)
		{
			notify.Warning("No animation of type " + animType + " registered on GamePlayer!");
			return 0;
		}
			
		string anim = AnimTypeMap[animType][ind];
		
		if (animateObject.GetClip(anim) == null)
		{
			notify.Debug (anim + " animation not found in " + animateObject);	
		}
		
		//notify.Debug (animateObject  + " playing " + anim);
		
		if(animateObject != null)
		{
			if (queued)
			{
				if(blend == true)
				{
					animateObject.CrossFadeQueued(anim,fadeTime);
				}
				else
				{
					animateObject.PlayQueued(anim);
				}
				
			}
			else
			{
				if(blend == true)
				{
					animateObject.CrossFade(anim, fadeTime, playMode);
				}
				else
				{
					if(rewind && animateObject.IsPlaying(anim))
					{
						animateObject.Rewind(anim);
					}
					
					animateObject.Play(anim, playMode);	
				}
				
			}
		}
		else
			Debug.Log("no anim!");
		
		//Also animate the balloon, if needed
		if(animType==AnimType.kBalloonEnter || animType==AnimType.kBalloonIdle || animType==AnimType.kBalloonExit ||
			(animType>=AnimType.kBalloonStumble && animType <= AnimType.kBalloonLeanRight) )
		{
			if(BalloonObject!=null && BalloonObject.animation!=null)
			{
				BalloonObject.animation.CrossFade(anim,fadeTime,playMode);
			}
		}
		
		//return the index, so we can base other animations off of this if we want to
		return ind;
	}
	
	public float AnimationPlaybackRateMin = 0.75f;
	public float AnimationPlaybackRateMax = 1.0f;
	//private List<AnimationState> cachedState = null;
	public void doAdjustAnimationSpeed(AnimType type)
	{
		if(animateObject == null || animateObject.animation == null)
			return;
			
		//-- JeffR: Not getting the modified velocity here because we want Boosted players to animate really fast.
		float ratio = playerVelocity.magnitude / _MaxRunVelocity;
		
		ratio = Mathf.Lerp(AnimationPlaybackRateMin, AnimationPlaybackRateMax, ratio);
		
		
		foreach(string s in AnimTypeMap[type])
			animateObject[s].speed = ratio;
		
//		int count = animateObject.animation.Count;
//		for(int i=0; i<count; i++) {
//			if(cachedState[i].layer == (int)AnimType.kJump)
//			{
//				cachedState[i].speed = ratio * 0.5f;
//			}
//			else
//			{
//				cachedState[i].speed = ratio;
//			}
//		}
//		foreach(AnimationState state in animateObject.animation) 
//		{
			//if(state.name == AnimTypeMap[AnimType.kJump])
			//{
			//	state.speed = ratio * 0.5f;
			//}
			//else
//			{
//				state.speed = ratio;
//			}
//		}
	}
	
	public bool IsAnimPlaying(AnimType type)
	{
		if(animateObject==null)	return false;
		
		return animateObject.IsPlaying(AnimTypeMap[type][0]);
	}
	
	public bool IsBalloonAnimPlaying(AnimType type)
	{
		if(BalloonObject==null || BalloonObject.animation==null)	return false;
		
		return BalloonObject.animation.IsPlaying(AnimTypeMap[type][0]);
	}
	
	public AnimationState GetAnimState(AnimType type)
	{
		if(animateObject==null)
			return null;
		
		return animateObject[AnimTypeMap[type][0]];
	}
	
	public AnimationState GetBalloonAnimState(AnimType type)
	{
		if(BalloonObject==null || BalloonObject.animation==null)
			return null;
		
		return BalloonObject.animation[AnimTypeMap[type][0]];
	}
	
	public void doAdjustAnimationSpeed(AnimType type, float speed)
	{
		if(animateObject == null)
			return;
		
		foreach(string s in AnimTypeMap[type])
			animateObject[s].speed = speed;
	}
	
	public void doPauseAnimation(string animState)
	{
//		TR.LOG("doPauseAnimation");
		if(animateObject == null)
			return;
			
		if(animateObject[animState]!=null)
			animateObject[animState].speed = 0f;
	}
	
	public void doPauseAllAnimations()
	{
//		TR.LOG("doPauseAnimation");
		if(animateObject == null || animateObject.animation == null)
			return;
			
//		int count = cachedState.Count;
//		for(int i=0; i<count; i++) {
//			cachedState[i].speed = 0;
//		}
		foreach(AnimationState ans in animateObject.animation) 
		{
			ans.speed = 0;
		}
	}
	
	public void doUnPauseAllAnimations()
	{
//		TR.LOG("doPauseAnimation");
		if(animateObject == null || animateObject.animation == null)
			return;
			
		foreach(AnimationState ans in animateObject) 
		{
			ans.speed = 1f;
		}
		
		//-- TODO: STOP ANIMATION IN CHILDREN
	}
	
	public void doSetAnimationTime(float t)
	{
//		TR.LOG("doSetAnimationTime");
		if(animateObject == null || animateObject.animation == null)
			return;
		
		
//		int count = cachedState.Count;
//		for(int i=0; i<count; i++) {
//			cachedState[i].time = t;
//		}
		
		foreach(AnimationState ans in animateObject.animation) 
		{
			ans.time = t;
		}
	}

	public void doUnPauseAnimation()
	{
//		TR.LOG("doUnPauseAnimation");
		doAdjustAnimationSpeed(AnimType.kRun);
	}
	
	public void doSetVisibility(bool visible)
	{
		if(RenderObject != null)
		{
			RenderObject.enabled = visible;
		}
		
//		Renderer[] renderers = _cachedTransform.GetComponentsInChildren<Renderer>();
//		foreach (Renderer r in renderers) 
//		{
//			r.enabled = visible;
//		}
	}
	
	public bool IsInvicible() {
		return (HasInvincibility || HasBoost);
	}
	
	public float GetSoundFrequencyMult() {
	/*    if (ActiveCharacterId == 2) {
	        // Barry Bones is low
	        return 0.85f;
	    } else if (ActiveCharacterId == 3) {
	        // Karma Lee is high
	        return 1.2f;
	    } else if (ActiveCharacterId == 4) {
	        // Montana Smith is a little lower
	        return 0.95f;
	    } else if (ActiveCharacterId == 5) {
	        // Francisco Montoya is a higher lower
	        return 1.15f;
	    } else if (ActiveCharacterId == 6) {
	        // Zach Wonder is a little lower
	        return 0.9f;
	    }*/
	    
	    return 1.0f;
	}
	
	private void PlaySoundForDeathType(DeathTypes deathType)
	{
		if(AudioManager.SharedInstance == null)
			return;
		
		switch(deathType)
		{
			case DeathTypes.Fall:
			{
				AudioManager.SharedInstance.PlayClip(characterSounds.fail_fall, 1.0f, GetSoundFrequencyMult());
				//AudioManager.SharedInstance.PlayFX(GameProfile.SharedInstance.GetActiveCharacter().fallSound, 1.0f, GetSoundFrequencyMult());
				break;	
			}
			case DeathTypes.SceneryTree:
			{
				AudioManager.SharedInstance.PlayClip(characterSounds.fail_tree, 1.0f, GetSoundFrequencyMult());
				//AudioManager.SharedInstance.PlayFX(GameProfile.SharedInstance.GetActiveCharacter().woodImpactSound, 1.0f, GetSoundFrequencyMult());
				break;	
			}
			case DeathTypes.SceneryRock:
			{
				AudioManager.SharedInstance.PlayClip(characterSounds.fail_rock, 1.0f, GetSoundFrequencyMult());
				//AudioManager.SharedInstance.PlayFX(GameProfile.SharedInstance.GetActiveCharacter().rockImpactSound, 1.0f, GetSoundFrequencyMult());
				break;	
			}
			case DeathTypes.Ledge:
			{
				AudioManager.SharedInstance.PlayClip(characterSounds.fail_fall, 1.0f, GetSoundFrequencyMult());
				//AudioManager.SharedInstance.PlayFX(GameProfile.SharedInstance.GetActiveCharacter().fallSound, 1.0f, GetSoundFrequencyMult());
				break;	
			}
//			case DeathTypes.WaterFall:
//			{
//				AudioManager.SharedInstance.PlayFX(AudioManager.Effects.splash, 1.0f, GetSoundFrequencyMult());
//				break;
//			}
//			case DeathTypes.Wheel:
//			{
//				AudioManager.SharedInstance.PlayFX(AudioManager.Effects.splat, 1.0f, GetSoundFrequencyMult());	
//				break;
//			}
			//case DeathTypes.Fire:
			//{
			//	AudioManager.SharedInstance.PlayFX(AudioManager.Effects.sizzle, 1.0f, GetSoundFrequencyMult());	
			//	break;
			//}
		}
	}
	
	
	public void Kill(DeathTypes deathType, float delay = 0f)
	{
		UIManagerOz.SharedInstance.inGameVC.HidePauseButton();
		//notify.Debug ("Kill " + deathType);
		Time.timeScale = 1f; // tutorial may set timescale to slowmo
		UIManagerOz.SharedInstance.postGameVC.statsRoot.GetComponent<StatsRoot>().SetDeathPortrait(deathType);
		if(dying && deathType!=DeathTypes.Baboon) return;
		boostDistanceLeft = 0f;
		EndBoost();
		DeathRunVelocity = getRunVelocity();
		//Debug.Log ("Death Velocity = " + DeathRunVelocity);
		StartCoroutine(KillSequence(deathType, delay));
	}
	
	private bool dying = false;
	public bool Dying { get { return dying; } set { dying = value; } }
	private IEnumerator KillSequence(DeathTypes deathType, float delay = 0f)
	{
		//notify.Debug("Player Killed: " + deathType);
		if(Enemy.main!=null)
			Enemy.main.enabled = false;
		
		yield return new WaitForSeconds(delay);
		
		//enabled = false;
		GameCamera.SharedInstance.enabled = false;
		
		dying = true;
		
		CurrentJumpVelocity = 0f;
		
		
		ResetBonusTypeEffect(BonusItem.BonusItemType.None);
		if(playerFx) playerFx.StopScoreBonusEffects();

		PlaySoundForDeathType(deathType);
		
		//if( deathType==DeathTypes.Fall || deathType==DeathTypes.WaterFall)	//Why does this keep changing??? -bc
		if(deathType==DeathTypes.SceneryRock || deathType==DeathTypes.SceneryTree || deathType==DeathTypes.WaterFall)
			yield return StartCoroutine(Kill_Obstacle());
		else if(deathType==DeathTypes.Fall || deathType==DeathTypes.Ledge)
			yield return StartCoroutine(Kill_Fall());
		
		if(Enemy.main!=null)
			Enemy.main.enabled = true;
		
		//enabled = true;
		GameCamera.SharedInstance.enabled = true;
		

		IsDead = true;
		DeathType = deathType;
		//DeathRunVelocity = getRunVelocity();
		playerVelocity = Vector3.zero;
		//Audio.StopFX();
		if(AudioManager.SharedInstance!=null)
			AudioManager.SharedInstance.FadeMusicMultiplier(0.15f, 0.4f);
			//AudioManager.SharedInstance.FadeOutMusic(0.25f);
		//ResetTint();
		TrackBuilder.SharedInstance.QueuedPiecesToAdd.Clear();
		
		//NOTE: Uncomment this to re-enable death animations
		doSetVisibility(false);
		
		boostDistanceLeft = 0f;
		EndBoost();
		
		NoStumble = false;
		if(playerFx) playerFx.StopStumbleProof();
		if(AudioManager.SharedInstance!=null)	AudioManager.SharedInstance.StopStumbleProof();		
		
		//Resources.UnloadUnusedAssets();		
		System.GC.Collect();
	}
	
	private IEnumerator Kill_Obstacle()
	{
		dying = true;
		
		doPlayAnimation(AnimType.kWallFail,false,true,0.1f);
	//	yield return new WaitForSeconds(0.5f);
		yield break;
		
	}
	
	private IEnumerator Kill_Fall()
	{
		yield break;
	}
	
	Vector3 tempVec3 = new Vector3(0,0,0);
	public void doSetX(float x)
	{
		tempVec3 = _cachedTransform.position;
		tempVec3.x = x;
		_cachedTransform.position = tempVec3;
	}
	public void doSetZ(float z)
	{
		tempVec3 = _cachedTransform.position;
		tempVec3.z = z;
		_cachedTransform.position = tempVec3;
	}
	
	public float getRunVelocity()
	{
		return playerVelocity.magnitude;
	}

	public float getModfiedMaxRunVelocity()
	{
		if(IsOnBalloon)
		{
			return GameProfile.SharedInstance.BalloonMaxSpeed;
		}
		if (HasBoost) 
		{
			if (Settings.GetBool("invulnerable", false))
			{
				return _MaxRunVelocity;
			}
			else
			{
				return (IsMegaBoost) ? _MaxRunVelocity * 2.5f : _MaxRunVelocity * 2.0f;
			}
		}
		return _MaxRunVelocity;
	}
	
	
	
	
	//Running Speed Min
	private float minSpeed = 10.0f;
	public void SetPlayerVelocity(float newVelocity) {
		if(newVelocity < minSpeed) {
			notify.Debug("Trying to set a min speed lower than the base min speed. minSpeed={0} , trying to set speed={1}" , minSpeed, newVelocity);
			newVelocity = minSpeed;
		}
		PlayerMagnitude = newVelocity;
		
		playerVelocity.x = Mathf.Abs (currentDirection.x);
		playerVelocity.y = Mathf.Abs (currentDirection.y);
		playerVelocity.z = Mathf.Abs (currentDirection.z);
		playerVelocity.x *= PlayerMagnitude;
		playerVelocity.y *= PlayerMagnitude;
		playerVelocity.z *= PlayerMagnitude;
	}
	
	public void Stumble(float delay = 0, bool badTurn = false)
	{
		notify.Debug("STUMBLE called.  Delay: " + delay + "  kill timer: " + StumbleKillTimer + " IsOnBalloon " + IsOnBalloon);
		
		//Debug.Log ("Stumbled... mysteriously???");
		
	/*	if (delay > 0.0f && !badTurn) 
		{
			StumbleAfterDelay = true;
			StumbleDelay = delay;
			return;
		}*/
		
		if(dying || OnTrackPiece.IsTransitionTunnel() || GameController.SharedInstance.IsTutorialMode
			|| GamePlayer.SharedInstance.IsAnimPlaying(AnimType.kBalloonExit) || GamePlayer.SharedInstance.IsAnimPlaying(AnimType.kBalloonExitFail))	return;
		
		StumbleAfterDelay = false;
		
		if (NoStumble && !badTurn) 
		{
			if(playerFx)
			{
			//	playerFx.StartStumbleProofStatic();
			//	playerFx.StopStumbleProof();
			//	playerFx.StartPoofPop ();
			}
			
			//'NoStumble' lasts forever. commented out.
			//NoStumble = false;
			return;
		}
		
		
		if(HasPoof)
		{
			EndPoof();
			//return;
		}
		if (HasShield) {
			// Don't allow the stumble if you have a shield
			EndShield();
			return;
		}
		
		if (StumbleKillTimer < DeathByMonkeyTime) {
			//Kill(DeathTypes.Eaten); gonna try attacking baboons
			if(!OnTrackPiece.NextTrackPiece.IsBalloon())	//Revive will not work if the next piece is a balloon
			{
				Enemy.main.isAttacking = true;
				if(AudioManager.SharedInstance!=null)
				{
					//AudioManager.SharedInstance.PlayFX(AudioManager.Effects.monkeyRoar, 0.85f);
					AudioManager.SharedInstance.PlayClip(characterSounds.baboon_grab,0.85f);
				}
			}
		}
		else {
			if(dying || OnTrackPiece.IsTransitionTunnel() || GameController.SharedInstance.IsTutorialMode || IsFalling)
			{
				return;
			}
			
			GameCamera.SharedInstance.Shake(0.15f, 1.0f, 1.0f);
			
			if(GameController.SharedInstance.StumblesThisRun==0)
				ObjectivesDataUpdater.SetGenericStat(ObjectiveType.DistanceWithoutStumble,GameController.SharedInstance.DistanceTraveled);
			
			GameController.SharedInstance.StumblesThisRun++;
		//	notify.Debug(GameController.SharedInstance.StumblesThisRun);
			
			
			float newVelocity = getRunVelocity() * 0.925f;
			SetPlayerVelocity(newVelocity);
			
			
			IsStumbling = true;
			TimeSinceStumbleStart = 0;
			float ratio = playerVelocity.magnitude / _MaxRunVelocity;
			ratio = Mathf.Lerp(AnimationPlaybackRateMin, AnimationPlaybackRateMax, ratio);
			StumbleDuration = 0.5f;// * ratio;
			
			if(!HasBonusEffect(BonusItem.BonusItemType.Poof) && !HasBonusEffect(BonusItem.BonusItemType.Vacuum))
				GameController.SharedInstance.LosePower();	
			
			StumbleKillTimer = 0;
	
			if(AudioManager.SharedInstance!=null)
			{
				//AudioManager.SharedInstance.PlayClip(characterSounds.t, 1.0f, GetSoundFrequencyMult());
				//AudioManager.SharedInstance.PlayFX(AudioManager.Effects.gruntTrip, 1.0f, GetSoundFrequencyMult());
				//AudioManager.SharedInstance.PlayFX(AudioManager.Effects.monkeys);
			}

			//notify.Debug("Stumble Animation");
			if(!dying && !IsOnBalloon && !HasGlindasBubble) {
				doPlayAnimation(AnimType.kStumble, false, false);
				doPlayAnimation(AnimType.kRun, true, true);	
			}
		}
	}
	
	private float timeInAir = 0f;
	private float CurrentJumpVelocity = 0.0f;
	bool useBoostJump = false;
	public bool Jump(float withDelay = 0)
	{
		if(dying)	return false;
		
		if(OnTrackPiece==null || OnTrackPiece.TrackType == TrackPiece.PieceType.kTPBalloonExit)
			return false;
/*
		if (IsJumping)
			TR.LOG("CANT JUMP: Already Jumping");
		if (IsStumbling)
			TR.LOG("CANT JUMP: Stumbling");
		if (IsFalling)
			TR.LOG("CANT JUMP: Falling");
 */ 
		
		if(IsJumping == true)
			return false;
		if(IsStumbling == true)
			return false;
		if(IsFalling == true)
			return false;
		if(IsOnBalloon == true)
			return false;
		if(IsAnimPlaying(AnimType.kBalloonExit) || IsAnimPlaying(AnimType.kBalloonExitFail))
			return false;
		if(longSlide)
			return false;
		
		//Not in Oz...
		//if(IsHangingFromWire == true)
		//	return false;
		//if(IsOnMineCart == true)
		//	return false;
		
		
		if(IsSliding)
			StopSliding();
		
		if (withDelay > 0) 
		{
			JumpAfterDelay = true;
			JumpDelay = withDelay;
			return true;
		}
		
		//Flag whether or not this should be a boost-style jump (smoother)
		useBoostJump = HasBoost && !boostEnding;
			
		timeInAir = 0f;

		if(AudioManager.SharedInstance != null)
		{
			//if(!HasBoost)
			//	AudioManager.SharedInstance.PlayFX(AudioManager.Effects.gruntJump, 1.0f, GetSoundFrequencyMult());	
			
			//AudioManager.SharedInstance.PlayCharacterSound(AudioManager.SharedInstance.jumping);
			AudioManager.SharedInstance.PlayClip(characterSounds.jump, 1.0f, GetSoundFrequencyMult());
		}
		
		
		float jumpVelocity = getRunVelocity();
		CurrentJumpVelocity = Mathf.Clamp(jumpVelocity, MinJumpVelocity, MaxJumpVelocity);
		
		JumpAfterDelay = false;
		IsJumping = true;
		IsSliding = false;
		if(!useBoostJump && (!HasGlindasBubble||isEndingBubble))
			doPlayAnimation(AnimType.kJump, false, false);
		else if (HasGlindasBubble&&!isEndingBubble)
		{
			doPlayAnimation(AnimType.kFloatingJump);
			doPlayAnimation(AnimType.kFloating,true);
		}
		return true;
	}
	
	public bool IsOnAZipline()
	{
		if(OnTrackPiece == null)
			return false;
		return OnTrackPiece.IsAZipLine;
	}
	
	public bool Slide(bool longSlide = false)
	{
		if( dying ||
			IsAnimPlaying(AnimType.kBalloonExit) || 
			IsAnimPlaying(AnimType.kBalloonExitFail) ||
			IsSliding == true ||
			IsFalling == true ||
			IsJumping == true ||
			IsOnAZipline() == true ||
			IsOnMineCart == true ||
			IsOnBalloon == true ||
			OnTrackPiece==null ||
			OnTrackPiece.TrackType == TrackPiece.PieceType.kTPBalloonExit ||
			HasBoost ||
			(HasGlindasBubble&&!isEndingBubble) )
		{
			return false;
		}
		
		IsSliding = true;
		TimeSinceSlideStart = 0;
		
		//Debug.Log ("I'm sliding");
		
		/*if(IsOnMineCart == true) {
			SlideDuration = 1.0f;
			doPlayAnimation(AnimType.kMinecartDuck);
			IsDucking = true;
		}*/
		//else {
		if(!longSlide)
		{
			SlideDuration = getRunVelocity() * SlideDurationFactor;
			if (SlideDuration > 0.75f)
			{
				SlideDuration = 0.75f;
			}	
		}
		
		else
			SlideDuration = 15f;
		
		
			if(AudioManager.SharedInstance != null)
			{
				//AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_slide_ww01);
				AudioManager.SharedInstance.PlayClip(characterSounds.slide);
				//AudioManager.SharedInstance.PlayCharacterSound(AudioManager.SharedInstance.sliding);
			}
			doPlayAnimation(AnimType.kSlide);
			doPlayAnimation(AnimType.kSlideIdle,true);
			if(playerFx) playerFx.StartDustSlide();
		//}

		int index = doPlayAnimation(AnimType.kSlide);	//Save the index of the animation, because we have to play the matching "SlideIdle"
		doPlayAnimation(AnimType.kSlideIdle,index,true);
		if(PlayerCollider)
		{
			tempVec3 = PlayerCollider.center;
			tempVec3.y = 0.2f;
			PlayerCollider.center = tempVec3;
		}	
		
		Invoke("SetSlidePositions",0.1f);
		
		return true;
	}
	
	public void SetSlidePositions()
	{
		if(playerFx && playerFx.magicMagnet && playerFx.magicMagnet.isPlaying){
			playerFx.SetMagicMagnetSlideHeight();
		}			
		
		if(playerFx && playerFx.scoreBonusElectric && playerFx.scoreBonusElectric.isPlaying){
			playerFx.SetScoreBonusElectricSlideHeight();
		}		

		if(playerFx && HasPoof){
			playerFx.SetShieldEffectsSlidePosition(0.0f, 0.4f, 0.0f);
		}			
	}
	
	public void doApplyForce(Vector3 deltaForce)
	{
		PlayerForce += deltaForce;
//		PlayerForce.x = playerForce.x;
//		PlayerForce.z = playerForce.z;
		//TR.LOG ("PF {0}", PlayerForce);
		HasForce = true;
	}
	
	public void doApplyForcef(float deltaForce)
	{
		PlayerForce += Vector3.one*deltaForce;
		//TR.LOG ("PFf {0}", PlayerForce);
		HasForce = true;
	}
	
	public void doResetForce()
	{
		PlayerForce = Vector3.zero;
		HasForce = false;
	}
	
	private bool IsAffectedByGravity()
	{
		if((IsOnAZipline() == false && IsOnMineCart == false && IsOnBalloon == false) || 
			IsJumping == true || 
			//OnTrackPiece.IsDownStairs() == true ||
			IsOverGround == false)
		{
			return true;
		}
		return false;
	}
	
	private float balloonVelocity = 0f;
	
	private float currentBalloonOffset = 0f;
	
	// Update is called once per frame
	private float newPlayerY = 0.0f;
	//private Vector3 turnVelocity = Vector3.zero;
	//private float BlinkDirection = 1.0f;
	//private _cachedTransform landPrt;
	
//	private AnimType balloonIdleAnim = AnimType.kBalloonIdle;
	
	public bool fogTransition = false;
	public int fogTransitionSign = 0;

	public bool whiteoutTransition = false;
	private int whiteoutTransitionSign = 0;
	
	private bool whiteoutDelay = false;
	private bool whiteoutPreFlash = false;
	private float whiteoutDelayTime = 0.0f;
	
	public void Update()
	{
		//Debug.Log(CoinCountForBonus + " " +CoinCountForBonusThreshold + " ");
		if(GamePlayer.SharedInstance != null && GamePlayer.SharedInstance.DEBUG_FOCUSSCENEVIEW == true)
		{
			#if UNITY_EDITOR
				//UnityEditor.SceneView.lastActiveSceneView.FrameSelected();
			Vector3 camPos = _cachedTransform.position;
			//camPos.y += 10.0f;//GamePlayer.SharedInstance.DEBUG_CAMERAZOOM;
			
			UnityEditor.SceneView.lastActiveSceneView.LookAt(camPos);
				//UnityEditor.SceneView.lastActiveSceneView.camera._cachedTransform.Translate(Vector3.up * GamePlayer.SharedInstance.DEBUG_CAMERAZOOM, Space.World);
			#endif
		}
		
		//doTintPlayer();
		//GameController.SharedInstance.Enemies[0].mat.color = RenderObject.material.color;
		
		if(Hold == true || IsDead == true || GameController.SharedInstance.IsPaused == true || Time.timeScale == 0f) {
			//TR.LOG(" the game is on Hold " + Hold + " IsDead " + IsDead + " IsPaused " + GameController.SharedInstance.IsPaused);
			return;
		}
			
	
		
		//-- Special case checks for tilt on pieces.
		/* Eyal Not in Oz
		if(OnTrackPiece != null && !IsInvicible())
		{
			if(OnTrackPiece.TrackType == TrackPiece.PieceType.kTPMineLedgeLeftMiddle)
			{
				if(PlayerXOffset > -0.4f)
				{
					IsOverGround = false;
					IsOnMineCart = false;
				}	
			}
			else if(OnTrackPiece.TrackType == TrackPiece.PieceType.kTPMineLedgeRightMiddle)
			{
				if(PlayerXOffset < 0.4f)
				{
					IsOverGround = false;
					IsOnMineCart = false;
				}
			}
		}
		*/
		
		if (StumbleAfterDelay == true && !HasInvincibility) { // don't double stumble after resurrect
			StumbleDelay -= Time.deltaTime;
			if (StumbleDelay <= 0)
				notify.Debug ("Stumble after delay");
				Stumble();
		}
		
		if (JumpAfterDelay == true) 
		{
			JumpDelay -= Time.deltaTime;
			if (JumpDelay <= 0)
				Jump();
		}
		
		//-- Decrement the vacuum clock.
		if(HasBonusEffect(BonusItem.BonusItemType.Vacuum) == true)
		{
			if(dying)
			{
				VacuumDuration = 0f;
				ResetCoinCountForBonus();
				//ResetBonusTypeEffect(BonusItem.BonusItemType.Vacuum);
			}
			
			VacuumDuration -= Time.deltaTime;
			if(VacuumDuration <= 0.0f)
			{
				ResetBonusTypeEffect(BonusItem.BonusItemType.Vacuum);
			}
		}
		
		//Removed this, because invincibility takes care of this
		/* //-- Decrement the poof clock.
		if(HasBonusEffect(BonusItem.BonusItemType.Poof) == true)
		{
			PoofDuration -= Time.deltaTime;
			if(PoofDuration < 0.0f)
			{
				ResetBonusTypeEffect(BonusItem.BonusItemType.Poof);
			}
		}*/
		
		//-- Decrement the shield clock.
		if(HasBonusEffect(BonusItem.BonusItemType.Shield) == true)
		{
			if(dying)
			{
				ShieldDuration = 0f;
				ResetCoinCountForBonus();
			}
			
			ShieldDuration -= Time.deltaTime;
			if(ShieldDuration < 0.0f)
			{
				EndShield();
			}
		}

		if(HasBonusEffect(BonusItem.BonusItemType.Boost) == true)
		{
			//TR.LOG ("Boosted alpha={0}", alpha);
			// Make the alpha flash if it is running out
		//	if (BoostDistanceLeft < BonusItemProtoData.SharedInstance.BoostStartSlowDownDistance) 
		//	{
		//		float alphaDistanceToEnd = (BonusItemProtoData.SharedInstance.BoostStartSlowDownDistance-BoostDistanceLeft)/BonusItemProtoData.SharedInstance.BoostStartSlowDownDistance;
//				alpha -= Time.deltaTime * Mathf.Lerp(1.0f, BonusItemProtoData.SharedInstance.BoostAlphaBlinkSpeed, alphaDistanceToEnd) * BlinkDirection;
//				if (alpha <= 0.05f) 
//				{
//					BlinkDirection *= -1.0f;
//					alpha = 0.05f;
//				}
//				else if(alpha >= 1.0f) {
//					BlinkDirection *= -1.0f;
//					alpha = 1.0f;
//				}
			//Taking this out for now, it caused oz to turn black
			//	RenderObject.material.color = Color.Lerp(Color.black, Color.white, Mathf.Abs(Mathf.Cos(alphaDistanceToEnd*BonusItemProtoData.SharedInstance.BoostAlphaBlinkSpeed)));
		//	}'
			if(!dying)
			{
				if(BoostDistanceLeft < BonusItemProtoData.SharedInstance.BoostStartSlowDownDistance)
				{
					if(!flashing)
					{
						StartCoroutine(DoOzFlash());
					}
					SetPlayerVelocity(Mathf.MoveTowards(getRunVelocity(),Mathf.Max(VelocityBeforeBoost,playerSpeedMinAfterHeadstart),Time.deltaTime*5f));
				}
			}
			
			else
			{
				BoostDistanceLeft = 0f;
				ResetCoinCountForBonus();
			}

//			Vector2 to = RenderObject.material.mainTextureOffset;
//			to.x += BonusItemProtoData.SharedInstance.BoostTextureOffsetSpeed * Time.deltaTime; 
//			to.y += BonusItemProtoData.SharedInstance.BoostTextureOffsetSpeed * Time.deltaTime;
//			RenderObject.material.mainTextureOffset = to;
			
			//-- BoostDistanceLeft is decremented in GameController.
			if(BoostDistanceLeft <= 0.0f)
			{
				EndBoost();
			}
		}
			
		if(HasPoof)
		{
			if(dying)
			{
				PoofTimeLeft = 0f;
				ResetCoinCountForBonus();
			}
			
			PoofTimeLeft -= Time.deltaTime;
			
			//if(PoofTimeLeft < 2.0f)
				//TintPlayer(Color.Lerp(PoofBlinkStartColor, PoofBlinkEndColor, Mathf.Abs(Mathf.Cos(InvincibilityTimeLeft*BonusItemProtoData.SharedInstance.BoostAlphaBlinkSpeed))),0f);		
		
			if(PoofTimeLeft <=0f)
				EndPoof();
		}
		
		if(HasInvincibility == true) {
			InvincibilityTimeLeft -= Time.deltaTime;
			
			if(InvincibilityTimeLeft < 2.0f && HasGlindasBubble && !flashing)
				StartCoroutine(DoOzFlash());
				
			if(InvincibilityTimeLeft < 0.0f) {
				//if(HasGlindasBubble)	//I've moved some stuff to EndGlindasBubble, because im assuming glindas bubble is equal to "invicibility"
					EndGlindasBubble ();
			}
		}
		/* this is used to disable stumbleproof after a certain distance
		if(NoStumble){
			if(NoStumbleDistLeft <= 0.0f){
				NoStumble = false;
				playerFx.StopStumbleProof();
				AudioManager.SharedInstance.StopStumbleProof();
			}
		}
		*/
		
		//GameController.SharedInstance.Enemies[0].mat.color = RenderObject.material.color;
	
//		if (IsDead || 
//			Hold || 
//			GameController.SharedInstance.IsResurrecting || GameController.SharedInstance.IsInCountdown || GameController.SharedInstance.IsPaused)
//			return;

		// Update time since last bonus item
		TimeSinceLastPowerup += Time.deltaTime;

		// Gravity
		float jumpGravityForce = PlayerGravity;
		
		if (!IsOnGround) {
			jumpGravityForce *= 2.0f;
		}
				
		// Impulse force
		if (HasForce == true) 
		{
			playerVelocity += (PlayerForce * Time.deltaTime);
			//-- CAP PV.
			//Adjusted max boost speed?
			const float ABSOLUTE_MAX_SUPERBOOST_VELOCITY = 25f;
			float ABSOLUTE_MAX_VELOCITY = !HasSuperBoost ? 21.0f : ABSOLUTE_MAX_SUPERBOOST_VELOCITY;
			
			if(playerVelocity.sqrMagnitude > (ABSOLUTE_MAX_VELOCITY*ABSOLUTE_MAX_VELOCITY)) {
				playerVelocity.Normalize();
				playerVelocity *= ABSOLUTE_MAX_VELOCITY;
			}
			doResetForce();
		}
		
		// Gravity force on jump
		if(!dying && !useBoostJump)	CurrentJumpVelocity += (jumpGravityForce * Time.deltaTime);
		else if(dying)			CurrentJumpVelocity += (jumpGravityForce * Time.deltaTime / 5f);	//fall slower if we just hit a wall
		
		PreviousLocation = _cachedTransform.position;
		Vector3 prevCurrentPosition = CurrentPosition;
		newPlayerY = _cachedTransform.position.y;
		
		//-- Simulate what gravity will do this step and save in a temp variable.
		//NOTE: Put a condition on this because step downward inclines were causing shaking
		if(IsJumping || !IsOverGround || !IsOnGround)
		{
			if(!useBoostJump)
				newPlayerY += (CurrentJumpVelocity * Time.deltaTime);
			else
				newPlayerY = CurrentPosition.y + (-Mathf.Cos(Mathf.Clamp01(timeInAir)*Mathf.PI*2f) + 1f);
		}
		

		//-- If we don't have piece, start at the root.
		if (OnTrackPiece == null)
		{
			ChooseNextTrackPiece(GameController.SharedInstance.trackRoot);
		}
		
		Vector3 playerPosPlaceholder = _cachedTransform.position;
		
		//-- If we cross into the next segments, choose the next track piece ( which may extend the track ).
		if (OnTrackPiece != null) 
		{
			//-- get next position and direction.
			CalcPointAlongTrackPiece(OnTrackPiece, currentPosition.y, out currentPosition, out currentDirection);
			
			//TR.LOG("OnTrackPiece " + OnTrackPiece + " currentPosition " + currentPosition + " currentDirection " + currentDirection + " runVelocity " + getRunVelocity());
			
		//	Debug.DrawLine(_cachedTransform.position,_cachedTransform.position + currentDirection,Color.red);
			if(autoTurn == true)
			{
				if(currentDirection.sqrMagnitude > Mathf.Epsilon)
				{
					_cachedTransform.forward = Vector3.Slerp(_cachedTransform.forward, currentDirection, Time.deltaTime*15f);
					
//					if(OnTrackPiece.TrackType == TrackPiece.PieceType.kTPCurveLeft || 
//						OnTrackPiece.TrackType == TrackPiece.PieceType.kTPCurveRight ||
//						OnTrackPiece.TrackType == TrackPiece.PieceType.kTPStraight)
					{
						SetPlayerVelocity( getRunVelocity() );
					}
				}
				else
					notify.Warning("Small direction vector! problem with turning...");
			}
			
			tempVec3 = currentPosition;
		//	notify.Debug(CurrentPosition);
			
			if(IsAffectedByGravity())
			{
				tempVec3.y = newPlayerY;
				tempVec3.y += (currentPosition.y - prevCurrentPosition.y);
			}
			
			//HACK: move back a little bit if dying
			if(dying)	{
				tempVec3.x = _cachedTransform.position.x;
				tempVec3.z = _cachedTransform.position.z;
			}
			
			if (float.IsNaN(tempVec3.x) || float.IsNaN(tempVec3.y) || float.IsNaN(tempVec3.z))
			{
				notify.Error("how did we get Nan? get Redmond or Bryant "+tempVec3);	
			}
			playerPosPlaceholder = tempVec3;
			
			currentPosition.x = tempVec3.x;
			currentPosition.z = tempVec3.z;
		//	Debug.Log(newPlayerY);
			
			if(OnTrackPiece.TrackType == TrackPiece.PieceType.kTPTransitionTunnelEntrance && !fogTransition)
			{		
				//RenderSettings.fog = true;
				RenderSettings.fogDensity = 0f;
				fogTransition = true;
				fogTransitionSign = 1;
				//RenderSettings.fogMode = FogMode.ExponentialSquared;			
				RenderSettings.fogColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);				
				RenderSettings.fogStartDistance = 0f;
				RenderSettings.fogEndDistance = 300f;
				fogFadeInSpeed = 0.5f;
				fogDensityClamp = 0.06f;	
				
				SkyboxMaterials skyboxMaterials = GetSkyboxMaterials();
				if(skyboxMaterials) {
					skyboxMaterials.SetTintColorToWhite();
					skyboxMaterials.FadeInTint(0.5f, 1.0f);	
				}
			}
				
			if(!whiteoutTransition && fogTransition && fogTransitionSign != 0)
			{
				if(fogTransitionSign == 1){
			//		Debug.Log (RenderSettings.fogDensity);
					RenderSettings.fogDensity += (Time.deltaTime*fogDensityClamp)/fogFadeInSpeed;
					if(RenderSettings.fogDensity > fogDensityClamp){
						fogTransitionSign = 0;
					}
				}
				if(fogTransitionSign == -1){
					RenderSettings.fogDensity -= (Time.deltaTime*fogDensityClamp)/fogFadeOutSpeed;
					if(RenderSettings.fogDensity <= 0){
						fogTransitionSign = 0;
						fogTransition = false;
						RenderSettings.fogDensity = 0f;
						//RenderSettings.fog = false;
					}
				}
			}

			if(whiteoutTransition)
			{
				//Debug.Log ("starting transition");
				if(whiteoutDelay)
				{
					whiteoutDelayTime -= Time.deltaTime;
					
					if(whiteoutPreFlash && whiteoutDelayTime <= 0.5f)
					{
						if(OnTrackPiece.TrackType == TrackPiece.PieceType.kTPTransitionTunnelMiddle || 
							OnTrackPiece.TrackType == TrackPiece.PieceType.kTPTransitionTunnelMiddle2)
						{
							TTAnimatedTile ttAnimatedTile = OnTrackPiece.GetComponentInChildren<TTAnimatedTile>();
							ttAnimatedTile.LightningFlash(0.0f, 0.4f, 0.2f);
							
							//Play lightning effect along track
//							if(transitionTunnelLightning && transitionTunnelLightning.active)
//							{
//								Vector3 trackPosition;
//								Debug.Log (CurrentSegment + 10);
//								Debug.Log (OnTrackPiece.GeneratedPath.Count);
//								Debug.Log (OnTrackPiece.NextTrackPiece.GeneratedPath.Count);
//								if(CurrentSegment + 10 < OnTrackPiece.GeneratedPath.Count)
//									trackPosition = OnTrackPiece.GeneratedPath[CurrentSegment + 10];
//								else
//								{
//									int index = OnTrackPiece.GeneratedPath.Count - (CurrentSegment + 10);
//									trackPosition = OnTrackPiece.NextTrackPiece.GeneratedPath[index];
//								}
//									
//								Vector3 playerPosition = OnTrackPiece.GeneratedPath[CurrentSegment];
//								
//								Vector3 transVector = trackPosition - playerPosition;
//								Debug.Log (trackPosition);
//								Debug.Log (playerPosition);
//								Debug.Log (transVector);
//								
//								transVector.y = transVector.y + 1;
//								transitionTunnelLightning.transform.Translate (transVector, Space.World);

//								transitionTunnelLightning.Play ();
//								Debug.Break();
//							}
						}
						whiteoutPreFlash = false;
					}
					
					//Debug.Log (whiteoutDelayTime);
					if(whiteoutDelayTime <= 0f)
					{
						whiteoutDelayTime = 0.0f;
						whiteoutTransitionSign = 1;
						whiteoutDelay = false;
					}
				}
				if(fogTransitionSign == 0 && whiteoutTransitionSign == 1){
					
					RenderSettings.fogDensity += (Time.deltaTime/whiteoutFadeInSpeed);
					if(coinToMeterMaterial && coinToMeterMaterial.HasProperty("_TintValue")) 
						coinToMeterMaterial.SetFloat("_TintValue", coinToMeterMaterial.GetFloat ("_TintValue") + ((Time.deltaTime/whiteoutFadeOutSpeed) + 0.6f));
					if(RenderSettings.fogDensity >= 1.0f)
					{
						RenderSettings.fogDensity = 1.0f;
						whiteoutTransitionSign = 0;
						if(coinToMeterMaterial && coinToMeterMaterial.HasProperty("_TintValue")) 
							coinToMeterMaterial.SetFloat("_TintValue",1.0f);
					}
				}
				if(fogTransitionSign == 0 && whiteoutTransitionSign == -1){
					RenderSettings.fogDensity -= (Time.deltaTime/whiteoutFadeOutSpeed);
					if(coinToMeterMaterial && coinToMeterMaterial.HasProperty("_TintValue")) 
						coinToMeterMaterial.SetFloat("_TintValue", coinToMeterMaterial.GetFloat ("_TintValue") - ((Time.deltaTime/whiteoutFadeOutSpeed)));
					if(RenderSettings.fogDensity <= fogDensityClamp){
						whiteoutTransitionSign = 0;
						whiteoutTransition = false;
						RenderSettings.fogDensity = fogDensityClamp;
						if(coinToMeterMaterial && coinToMeterMaterial.HasProperty("_TintValue")) 
							coinToMeterMaterial.SetFloat("_TintValue",0.0f);						
						//RenderSettings.fog = false;
					}
				}
			}						
			
			
		}
		
		bool wasOnGround = IsOnGround;

		if (IsOverGround == true) 
		{
			// Stay above the ground
			if ((IsFalling == false && newPlayerY <= CurrentPosition.y && !useBoostJump) ||
				//Commenting this out may cause problems, but this needs to be simpler (i dont see what this is doing)
			//	(IsFalling == false && (IsJumping == false|| IsHangingFromWire==true ) && newPlayerY <= (CurrentPosition.y+3.0f)) ||
				(IsFalling == true && newPlayerY <= CurrentPosition.y && newPlayerY >= CurrentPosition.y - 6.0f) ||
				(useBoostJump == true && IsJumping==true && timeInAir >=1f) )
			{
				
				if (IsJumping == true /*|| (HasBoost && IsHangingFromWire) || (HasInvincibility && IsHangingFromWire)*/) 
				{
					//DustSprite.enabled = true;
					IsJumping = false;
					
					timeInAir = 0f;
					
					//Not in oz
				/*	if(IsHangingFromWire == true)
					{
						doPlayAnimation(AnimType.kZiplineIdle, false, false);
					}
					else if(IsOnMineCart == true)
					{
						if(IsSliding == true && IsDucking == false)
							StopSliding();
						
						doPlayAnimation(AnimType.kMinecartIdle, false);
						doPauseAnimation(AnimTypeMap[AnimType.kMinecartIdle][0]);
						doSetAnimationTime(0.5f);
					}
					else*/ if (IsOnBalloon == true)
					{
						if(IsSliding == true)
							StopSliding();
						
					//	doPlayAnimation(balloonIdleAnim, false);
					//	doPauseAnimation(AnimTypeMap[balloonIdleAnim][0]);
					//	doSetAnimationTime(0.5f);
					}
					else
					{
						if(!dying && (!HasBoost || boostEnding) && (!HasGlindasBubble||isEndingBubble))	doPlayAnimation(AnimType.kRun,false,true,0.15f);
						if(playerFx) playerFx.StartDustLand();
						lastCollidedLayer = 0;

						if(AudioManager.SharedInstance != null)
						{
							if(!useBoostJump)
							{
								AudioManager.SharedInstance.PlayClip(characterSounds.land,1f,GetSoundFrequencyMult());
								//AudioManager.SharedInstance.PlayFX(AudioManager.Effects.gruntJumpLand, 1.0f, GetSoundFrequencyMult());
							}
							
							//AudioManager.SharedInstance.PlayCharacterSound(AudioManager.SharedInstance.landing);
						}
					}
					useBoostJump = false;
				}
				
				
				
				// If they cut it close make them stumble
//				if (IsFalling == true && newPlayerY < CurrentPosition.y - 2.0f && HasInvincibility == false)
//				{
//					doStumble();
//				}
//
//				if (IsGroundHeightChangeHigher && y >= GroundHeight - 6.0f && y < GroundHeight - 2.0f && !HasInvincibility)
//					Stumble();
				
				// Put player back on the ground level
				if(dying)	{currentPosition.x=tempVec3.x; currentPosition.z = tempVec3.z;}
				playerPosPlaceholder = currentPosition;
				newPlayerY = currentPosition.y;
				CurrentJumpVelocity = 0.0f;
				IsOnGround = true;
			}
		}
		else 
		{
			IsOnGround = false;
		}
		
		//-- Tilt push left and right
		if(/*OnTrackPiece.IsAZipLine == false &&*/ IsFalling == false && /*IsOnMineCart == false &&*/ IsOnBalloon == false)
		{
			//Why did we need all this stuff...?
			
			//Vector3 playerRight = _cachedTransform.right;
			//tempVec3 = _cachedTransform.position;
			//tempVec3.y = CurrentPosition.y;
			//tempVec3 -= CurrentPosition;
			
			if(!dying)
			{
				//distanceAlongRight = Vector3.Dot (playerRight, tempVec3);
				distanceAlongRight = ((PlayerXOffset*0.85f));// - distanceAlongRight);
			//	_cachedTransform.Translate(Vector3.right*distanceAlongRight);
				playerPosPlaceholder += Vector3.Cross(Vector3.up,currentDirection.normalized) * distanceAlongRight;
			}
		}
		_cachedTransform.position = playerPosPlaceholder;
	
		
		if(IsOnBalloon == true)
		{
			const float maxspeed = 11f;
			const float maxoffset = 5.5f;
			const float maxangle = 30f;
			const float edgesoft = 5f;
			const float maxTilt = 4f;
			const float responsiveness = 250f;
			
			float xOffset = Mathf.Clamp(GameInput.PlayerXOffsetUnclamped,-maxTilt,maxTilt);
			if(Mathf.Sign(xOffset) != Mathf.Sign(PreviousPlayerXOffset)){
				//AudioManager.SharedInstance.PlayBasketSound();
			}
			
			float targetVel = Mathf.Pow(Mathf.Abs(xOffset),0.5f)/Mathf.Pow(maxTilt,0.5f) *maxspeed * Mathf.Sign(xOffset);
		//	if(Mathf.Abs(targetVel) > Mathf.Abs(balloonVelocity))
		//		balloonVelocity = targetVel;
		//	else
				balloonVelocity = Mathf.MoveTowards(balloonVelocity,targetVel,Time.deltaTime*responsiveness);//(xOffset/maxTilt)*maxspeed;//
			float minV = Mathf.Clamp((-maxoffset-currentBalloonOffset)*edgesoft,-maxspeed,maxspeed);
			float maxV = Mathf.Clamp((maxoffset-currentBalloonOffset)*edgesoft,-maxspeed,maxspeed);
			balloonVelocity = Mathf.Clamp(balloonVelocity,minV,maxV);// + (PlayerXOffset-(currentBalloonOffset/maxoffset))*maxspeed/10f;
				
			if(OnTrackPiece.TrackType==TrackPiece.PieceType.kTPBalloonExit
						|| OnTrackPiece.TrackType==TrackPiece.PieceType.kTPBalloonFall
						|| (OnTrackPiece.TrackType==TrackPiece.PieceType.kTPBalloonEntrance && (transform.position-OnTrackPiece.transform.position).sqrMagnitude < 1200f)
						|| WillBalloonFail)
				balloonVelocity = Mathf.MoveTowards(balloonVelocity,-currentBalloonOffset,Time.deltaTime*(responsiveness*1.5f));
			
			currentBalloonOffset = Mathf.Clamp(currentBalloonOffset + balloonVelocity*Time.deltaTime,-maxoffset,maxoffset);
			
			currentBalloonRotation = Mathf.Lerp(currentBalloonRotation,balloonVelocity*maxangle/maxspeed,Time.deltaTime*3f);
			
			Transform characterModelTransform = CharacterModel.transform;
			characterModelTransform.localPosition = Vector3.zero;
			characterModelTransform.localRotation = Quaternion.identity;
		//	characterModelTransform.LookAt(OnTrackPiece.GeneratedPath[CurrentSegment+1],Vector3.up);
			characterModelTransform.RotateAround(_cachedTransform.position, _cachedTransform.forward, currentBalloonRotation);
			
			Vector3 offset = Vector3.right * currentBalloonOffset;
			characterModelTransform.localPosition = offset;
			GetComponent<SphereCollider>().center = offset*0.9f + Vector3.up*0.6f;
			
			
			OzGameCamera.SharedInstance.BalloonXOffset = currentBalloonOffset / 1.5f;
			
			if(!IsAnimPlaying(AnimType.kBalloonEnter) && !WillBalloonFail)
			{
			//	if(OnTrackPiece.TrackType!=TrackPiece.PieceType.kTPBalloonEntrance) { 
					if(!IsAnimPlaying(AnimType.kBalloonIdle))			doPlayAnimation(AnimType.kBalloonIdle,false,false,0.5f,false,PlayMode.StopSameLayer);
					if(!IsAnimPlaying(AnimType.kBalloonLeanLeft))		doPlayAnimation(AnimType.kBalloonLeanLeft,false,false,0.5f,false,PlayMode.StopSameLayer);
					if(!IsAnimPlaying(AnimType.kBalloonLeanRight))		doPlayAnimation(AnimType.kBalloonLeanRight,false,false,0.5f,false,PlayMode.StopSameLayer);
					
					GetAnimState(AnimType.kBalloonLeanRight).weight = 	Mathf.Lerp(GetAnimState(AnimType.kBalloonLeanRight).weight,Mathf.Clamp01(PlayerXOffset),Time.deltaTime*1000f);
					GetAnimState(AnimType.kBalloonLeanLeft).weight = 	Mathf.Lerp(GetAnimState(AnimType.kBalloonLeanLeft).weight,Mathf.Clamp01(-PlayerXOffset),Time.deltaTime*1000f);
					GetAnimState(AnimType.kBalloonIdle).weight = 		Mathf.Lerp(GetAnimState(AnimType.kBalloonIdle).weight,Mathf.Clamp01(1f-Mathf.Abs(PlayerXOffset)),Time.deltaTime*1000f);
				
				if(BalloonObject!=null)
				{
					if(!IsBalloonAnimPlaying(AnimType.kBalloonIdle))			doPlayBalloonAnimation(AnimType.kBalloonIdle,false,false,0.5f,false,PlayMode.StopSameLayer);
					if(!IsBalloonAnimPlaying(AnimType.kBalloonLeanLeft))		doPlayBalloonAnimation(AnimType.kBalloonLeanLeft,false,false,0.5f,false,PlayMode.StopSameLayer);
					if(!IsBalloonAnimPlaying(AnimType.kBalloonLeanRight))		doPlayBalloonAnimation(AnimType.kBalloonLeanRight,false,false,0.5f,false,PlayMode.StopSameLayer);
					
					GetBalloonAnimState(AnimType.kBalloonLeanRight).weight = 	Mathf.Lerp(GetBalloonAnimState(AnimType.kBalloonLeanRight).weight,Mathf.Clamp01(PlayerXOffset),Time.deltaTime*1000f);
					GetBalloonAnimState(AnimType.kBalloonLeanLeft).weight = 	Mathf.Lerp(GetBalloonAnimState(AnimType.kBalloonLeanLeft).weight,Mathf.Clamp01(-PlayerXOffset),Time.deltaTime*1000f);
					GetBalloonAnimState(AnimType.kBalloonIdle).weight = 		Mathf.Lerp(GetBalloonAnimState(AnimType.kBalloonIdle).weight,Mathf.Clamp01(1f-Mathf.Abs(PlayerXOffset)),Time.deltaTime*1000f);
				}
			//	}
			}
			
			CharacterModelRefs.BalloonPropellorBaseR.localRotation = Quaternion.identity;
			CharacterModelRefs.BalloonPropellorBaseL.localRotation = Quaternion.identity;
			
			float rotationmax = Mathf.PI/3f;
			
			CharacterModelRefs.BalloonPropellorBaseR.RotateAroundLocal(Vector3.up,GetAnimState(AnimType.kBalloonLeanRight).weight*rotationmax*2f - rotationmax);
			CharacterModelRefs.BalloonPropellorBaseL.RotateAroundLocal(Vector3.up,GetAnimState(AnimType.kBalloonLeanLeft).weight*(-rotationmax*2f) + rotationmax);
			CharacterModelRefs.BalloonPropellorR.RotateAroundLocal(Vector3.forward,15f*Time.deltaTime + Mathf.Abs(xOffset)*Time.deltaTime*15f);
			CharacterModelRefs.BalloonPropellorL.RotateAroundLocal(Vector3.forward,15f*Time.deltaTime + Mathf.Abs(xOffset)*Time.deltaTime*15f);
			
			AudioManager.SharedInstance.UpdateBalloonMotorPitch(0.9f + 0.25f * Mathf.Abs(balloonVelocity) / maxspeed);
			

		}
	
		
		if(IsJumping == true || OnTrackPiece.IsBalloon() || OnTrackPiece.IsStairs())
		{
			tempVec3 = _cachedTransform.forward;
			tempVec3.y = 0.0f;
			_cachedTransform.LookAt(_cachedTransform.position+tempVec3, _cachedTransform.up);
		}
		
		if(IsJumping)
			timeInAir += Time.deltaTime;
		
		if (newPlayerY < CurrentPosition.y && IsOnMineCart == false && IsOnBalloon == false && HasBoost==false) 
		{
			TimeSinceFallStart += Time.deltaTime;
			if (IsFalling == false) 
			{
				TimeSinceFallStart = 0;
				IsFalling = true;
				UIManagerOz.SharedInstance.inGameVC.HidePauseButton();
			}

		/*	if (TimeSinceFallStart > 0.25f) 
			{
				if(AudioManager.SharedInstance != null)
				{
					AudioManager.SharedInstance.PlayFX(GameProfile.SharedInstance.GetActiveCharacter().fallSound, 1.0f, GetSoundFrequencyMult());
				}
			}*/
		}
		else 
		{
			if (IsFalling) 
			{
				// TODO Stop a scream sound
			}
			IsFalling = false;
			
			//TR.LOG("Is  Falling");
		}
		
		if (IsJumping) {
			// Keep track if we are in the rise of fall part of the jump. 
			// If our current position is greater than the previous position in Y, then we are still in the UpPhase
			if (newPlayerY > PreviousLocation.y) {
				IsJumpingUpPhase = true;
			} else {
				IsJumpingUpPhase = false;
			}
		}
		
		if (IsSliding == true) 
		{
			TimeSinceSlideStart += Time.deltaTime;
			//DustSprite.enabled = true;
			if (TimeSinceSlideStart > SlideDuration) 
			{
				StopSliding();

			}
		}

		if (IsOnGround == true && wasOnGround == false && IsStumbling == false && IsSliding == false && IsOnAZipline() == false && IsOnMineCart == false && IsOnBalloon == false && !HasBoost)
		{
			if(!dying && (!HasGlindasBubble||isEndingBubble))	doPlayAnimation(AnimType.kRun,false,true,0.05f);
		}

		if (IsStumbling == true) 
		{
			TimeSinceStumbleStart += Time.deltaTime;
			if (TimeSinceStumbleStart > StumbleDuration)
				IsStumbling = false;
		}
		
		

		StumbleKillTimer += Time.deltaTime;

		// If the player falls off the ground mark thema s dead
		if (IsFalling == true && TimeSinceFallStart > 0.5f) 
		{
			DeathTypes deathType = DeathTypes.Fall;
			if(OnTrackPiece)
			{
//				if(OnTrackPiece.IsLedge())
//				{
//					deathType = DeathTypes.Ledge;
//				}
//				else if(lastCollidedLayer == WaterColliderLayerMask) {
//					deathType = DeathTypes.WaterFall;
//				}
			}
			
			Kill(deathType);
		}
		
		if(Time.frameCount%60==0)	// We dont need to do this every frame
		{
			doAdjustAnimationSpeed(AnimType.kRun);
		//	doAdjustAnimationSpeed(AnimType.kJump,1.2f);
		}
		
		if(BackDropObject != null)
		{
			BackDropObject.transform.position = _cachedTransform.position + Vector3.up * BackDropOffset ;
		}
		
		// Tracking data for pacing
		timeSinceLastCountRefesh += Time.deltaTime;
		
		if (LastTrackPiece != OnTrackPiece) {
			LastTrackPiece = OnTrackPiece;
			if (OnTrackPiece.IsTurn()) {
				turnCount++;	
			}
			
			if (OnTrackPiece.IsObstacle()) {
				obstacleCount++;	
			}
		}
		
		if (timeSinceLastCountRefesh >= 30.0f) {
			notify.Debug("Pacing Data - Turns: {0} Obstacles {1}", turnCount, obstacleCount);
			turnCount = 0;
			obstacleCount = 0;
			timeSinceLastCountRefesh = 0.0f;
		}
		
		if(Sunlight!=null && !IsOnBalloon)
		{
			Sunlight.UpdateBlobShadowPosition( CurrentPosition + transform.right*PlayerXOffset*0.85f + Vector3.up*0.02f);
		}
/*		if(blobShadow!=null && !IsOnBalloon)
		{
			blobShadow.transform.position = CurrentPosition + transform.right*PlayerXOffset*0.85f + Vector3.up*0.02f;
		}*/
		//We now do this in gamecontroller
		//Keep recording the amount of distance, if we havent had any stumbles
		//if(GameController.SharedInstance.StumblesThisRun == 0) {
		//	UIInGameViewController.SetObjectiveDataWithFilter(ObjectiveFilterType.WithoutStumble);
		//}
		
	//	if(OnTrackPiece!=null)
	//		Debug.Log(OnTrackPiece.CurrentTrackPieceData.name + " " + CurrentSegment);
		
	}
	
	private void StopSliding()
	{
		IsSliding = false;
		if(PlayerCollider)
		{
			tempVec3 = PlayerCollider.center;
			tempVec3.y = 0.6f;
			PlayerCollider.center = tempVec3;
		}
		
		if(playerFx) playerFx.StopDustSlide();		

		if(playerFx && playerFx.magicMagnet && playerFx.magicMagnet.isPlaying){
			playerFx.ResetMagicMagnetHeight();
		}		
		
		if(playerFx && playerFx.scoreBonusElectric && playerFx.scoreBonusElectric.isPlaying){
			playerFx.ResetScoreBonusElectricHeight();
		}
		
		if(playerFx && HasPoof)
			playerFx.ResetShieldEffectsPosition();
		
		//if(PoofEffectObject != null) {
			//tempVec3 = PoofEffectObject._cachedTransform.localPosition;
			//tempVec3.y = 0.0f;
			//PoofEffectObject._cachedTransform.localPosition = tempVec3;
		//}
		
		if((!HasBoost||boostEnding) && !IsOnBalloon && !dying && (!HasGlindasBubble||isEndingBubble))
			doPlayAnimation(AnimType.kRun);
	/*	else if(IsDucking == true) {
			IsDucking = true;
			doPlayAnimation(AnimType.kMinecartIdle);
		}*/
	}
	
//	float lastD = 0;
	private void AddTrackPieceToEnd(TrackPiece currentPiece, float remain, int vertCount, int triCount, int length, int turn)
	{
	
		//notify.Debug (string.Format("AddTrackPieceToEnd {0}, {1}", currentPiece, remain));
		bool environmentSwitching = (EnvironmentSetSwitcher.SharedInstance != null && EnvironmentSetSwitcher.SharedInstance.WantNewEnvironmentSet == true);
		
		while((((remain > 0.0f)&&(vertCount>0)&&(triCount>0))||(length < 3)) && 
				(currentPiece!=null) )
		{
			if((currentPiece.TrackType == TrackPiece.PieceType.kTPBalloonExit)&&(currentPiece != GamePlayer.SharedInstance.OnTrackPiece))
			{
				if(length < 3)
				{
					remain = 0f;
				}
				else
				{
					break;
				}
			}
			if(environmentSwitching && (currentPiece.TrackType==TrackPiece.PieceType.kTPTransitionTunnelEntrance)&&(remain > 20f))
			{//cap the maximum track to 20m after a tunnel entrance until we are inside it
				remain = 20f;
			}
			
			//Adjusted for Track Overlap
			int oldturn = turn;
			if(currentPiece.IsTurnLeft())// && !currentPiece.IsJunction())// || currentPiece.IsSlightLeft())
			{
				//turn = 2;
				if(turn < 0)
				{
					turn = 2;
				}
				else
				{
					turn += 2;
				}
				if(!currentPiece.IsJunction())
					currentPiece.lastTurnType = 1;
			}
			
			else if (currentPiece.IsSlightLeft())// && EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.SetId != 3)
			{
				if(turn < 0)
				{
					turn = 1;
				}
				else
				{
					turn ++;
				}				
			}
			
			else if(currentPiece.IsTurnRight())// && !currentPiece.IsJunction())// || currentPiece.IsSlightRight())
			{
				//turn = -2;
				if(turn > 0)
				{
					turn = -2;
				}
				else
				{
					turn -= 2;
				}
				
				if(!currentPiece.IsJunction())
					currentPiece.lastTurnType = -1;				
			}
			
			else if(currentPiece.IsSlightRight())// && EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.SetId != 3)
			{
				if(turn > 0)
				{
					turn = -1;
				}
				else
				{
					turn --;
				}
			}
			
			//We still want the junction to count as a left turn,
			// since below we start another thread for the alternate path that counts it as a right turn
			if(currentPiece.IsJunction())
			{
				if(turn < 0)
				{
					turn = 2;
				}
				else
				{
					turn += 2;
				}
			}
			
			//Debug.Log("Turn Value in GamePlayer: " + turn);
			
			if(currentPiece.NextTrackPiece == null)
			{
				//notify.Debug(string.Format("AddTrackPieceToEnd Really adding {0}, {1}", currentPiece, remain));
				currentPiece.AttachRandomPiece(false, TrackBuilder.SharedInstance.QueuedPiecesToAdd, turn);
				if(currentPiece.NextTrackPiece == null)
				{
					notify.Warning("Could not add track piece");
				}
				if(length > 2)
					return;
			}
			++length;
			remain -= currentPiece.EstimatedPathLength;
			triCount -= currentPiece.CurrentTrackPieceData.TriCount;
			vertCount -= currentPiece.CurrentTrackPieceData.VertCount;
			TrackPiece.PieceType trackType = currentPiece.TrackType;
			//build out alternate path - but wait until it is chosen for balloons as there are a lot of junctions...

			//junctions are left and right but alternate is always right
			if((currentPiece.Alternate_NextTrackPiece != null) &&
				((trackType != TrackPiece.PieceType.kTPBalloonStraight)||(currentPiece.UseAlternatePath)))
			{
				vertCount/=2;
				triCount/=2;
				if(oldturn>0)
					oldturn = 2;
				else
					oldturn-=2;
				AddTrackPieceToEnd(currentPiece.Alternate_NextTrackPiece, remain, vertCount, triCount, length, oldturn);//(oldturn<0)?oldturn-1:-1);//-1 for the right turn, -1 for the left turn added earlier
			}
			currentPiece = currentPiece.NextTrackPiece;
		}

	}
	
	public void RegisterForOnTrackPieceChange( OnTrackPieceChangedHandler delg)
	{
		OnTrackPieceChangedEvent += delg;
	}
 
	public void UnregisterForOnTrackPieceChange( OnTrackPieceChangedHandler delg)
	{
		OnTrackPieceChangedEvent -= delg;
	}
	
	
	private void ChooseNextTrackPiece(TrackPiece rootPiece)
	{
		//notify.Debug("ChooseNextTrackPiece");
		//notify.Debug("rootPiece " + rootPiece );
		//notify.Debug(" type " + rootPiece.TrackType);
		//notify.Debug(" OnTrackPiece " + OnTrackPiece );
		//-- Move to the next piece.
		TrackPiece oldTrackPiece = OnTrackPiece;
		if(oldTrackPiece && oldTrackPiece.PreviousTrackPiece!=null)	//Don't take shadow off if this is the first track piece
		{
			oldTrackPiece.ReceiveShadow(false);
		}
		OnTrackPiece = rootPiece;
		OnTrackPiece.ReceiveShadow(true);
		if (OnTrackPieceChangedEvent != null)
		{
			OnTrackPieceChangedEvent(oldTrackPiece, OnTrackPiece);
		}
		
		justTurned = false;
		
		//if(HasBoost && (TrackBuilder.SharedInstance.IsObstacleType(OnTrackPiece.NextTrackPiece.TrackType) || TrackBuilder.SharedInstance.IsStumbleType(OnTrackPiece.NextTrackPiece.TrackType)) )
		//{
		//	Jump(0f);
		//	Enemy.main.Jump((Enemy.main._cachedTransform.position-Cached_cachedTransform.position).magnitude/PlayerVelocity.magnitude);
		//}
		
		if(WillBalloonFail)
		{
			DidBalloonFail = true;
			TrackPiece Alternate_newTrackPiece = OnTrackPiece.AttachPiece(TrackPiece.PieceType.kTPBalloonFall, true);
			Alternate_newTrackPiece.PostPieces = new List<TrackPiece.PieceType>(4);
			TrackPiece.PieceType fail = (TrackPiece.PieceType) System.Enum.Parse(typeof(TrackPiece.PieceType), EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.FailPostBalloonPiece);
			for (int i=0; i< EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.NumberOfPostBallonFailPieces; i++)
			{
				Alternate_newTrackPiece.PostPieces.Add(fail);
			}
		}
		
		if(OnTrackPiece != null)
		{
			if(OnTrackPiece.IsTurn() == true)
			{
				GameController.SharedInstance.MainCamera.SmoothRotationSpeed = 8;
			}
			else
			{
				GameController.SharedInstance.MainCamera.SmoothRotationSpeed = 3.5f;
			}
			
			//Used to determine if balloon difficulty should be turned off or kept on
			if(OnTrackPiece.PreviousTrackPiece == null
				|| (OnTrackPiece.TrackType != TrackPiece.PieceType.kTPPreBalloon
				&& OnTrackPiece.PreviousTrackPiece.TrackType == TrackPiece.PieceType.kTPBalloonJunction))
			{
				//wrong turn, so revert difficulty back to normal difficulty
				GameController.SharedInstance.ResetBalloonDifficulty();
			}
			
			if(OnTrackPiece.TrackType == TrackPiece.PieceType.kTPBalloonJunction)
			{
				
				//notify.Debug ("we are on balloon junction so let's turn on baloon on pre piece");
				TrackPiece next =  OnTrackPiece.NextTrackPiece;
				if(next){
					BalloonPrePiece bpp = next.GetComponentInChildren<BalloonPrePiece>();
					if(bpp){
						bpp.ShowBalloon();
					}
				}
			}
			
			if(OnTrackPiece.TrackType == TrackPiece.PieceType.kTPBalloonEntrance){
				AudioManager.SharedInstance.SwitchMusic(AudioManager.SharedInstance.BalloonMusic);
			}
			
			if(OnTrackPiece.TrackType == TrackPiece.PieceType.kTPBalloonExit ||
				OnTrackPiece.TrackType == TrackPiece.PieceType.kTPBalloonFall){
				AudioManager.SharedInstance.SwitchMusic(AudioManager.SharedInstance.GameMusic);
			}
			
			// display collect coin tutorial
			if(GameController.SharedInstance.IsTutorialMode && 
				OnTrackPiece.TrackType == TrackPiece.PieceType.kTPStraightFlat &&
				OnTrackPiece.IsTutorialPiece &&
				GameController.SharedInstance.TutorialID == 5){
				UIManagerOz.SharedInstance.inGameVC.ShowCollectCoinsTutorial();
			}
			// hide envProgressHud
			if(OnTrackPiece.TrackType == TrackPiece.PieceType.kTPTransitionTunnelEntrance){
				UIManagerOz.SharedInstance.inGameVC.HideEnvProgress();
				AudioManager.SharedInstance.SwitchMusic(AudioManager.SharedInstance.TunnelMusic);
			}
			if(OnTrackPiece.TrackType == TrackPiece.PieceType.kTPTransitionTunnelExit){
				AudioManager.SharedInstance.SwitchMusic(AudioManager.SharedInstance.GameMusic);
				AddScore(2000);
				//Debug.Log ("Score for Exiting TT");
			}
			
			// define wether it's a hard or soft surface
			
			//notify.Debug("isHard Surface " +  OnTrackPiece.isHardSurface + " Environment " + OnTrackPiece.TrackDef.Environment);
			isOnHardSurface = OnTrackPiece.isHardSurface;

			/*
			if((OnTrackPiece.IsLedge() || OnTrackPiece.IsNarrow()) && IsSliding == false){
				IsOnLedge = true;
				if(PlayerXOffset >= 0){
					if(!HasInvincibility && !HasBoost)	doPlayAnimation(AnimType.kLeftLedge,false,true,0.5f);
				}
				else {
					if(!HasInvincibility && !HasBoost)	doPlayAnimation(AnimType.kRightLedge);
				}
			}
			
			
			else if(!OnTrackPiece.IsLedge() && !OnTrackPiece.IsNarrow() && IsOnLedge){
				IsOnLedge = false;
				if(!HasInvincibility && !HasBoost)	doPlayAnimation(AnimType.kRun);
			}
			*/
			
		}
    
		//notify.Debug("Add old piece to queue");
		//-- Drop old pieces.
		TrackPiece oldp = oldTrackPiece;
		if(oldp != null)
		{
			//TR.LOG("ChooseNextTrackPiece: Adding to oldtrackpieces {0}", oldp);
			OldTrackPieces.Add(oldp);
			
			//-- Drop previous branch not taken
			//-- If the previous piece had a branch (Alternate_NextTrackPiece != null)
			if(oldp != null && oldp.Alternate_NextTrackPiece != null)
			{
				//-- If we are on the alternate branch, drop the other path.
				if(OnTrackPiece.PieceIndex == oldp.Alternate_NextTrackPiece.PieceIndex)
				{
					if(oldp.NextTrackPiece) {
						//TR.LOG("ChooseNextTrackPiece: starting RR {0}", oldp.NextTrackPiece);
						oldp.NextTrackPiece.RecursiveRemove();
					}
				}
				else
				{
					//TR.LOG("ChooseNextTrackPiece: starting RR {0}", oldp.Alternate_NextTrackPiece);
					oldp.Alternate_NextTrackPiece.RecursiveRemove();
				}
			}
			if((oldp.trackPieceDefinition != null)&&(TrackBuilder.IsJunctionType(oldp.trackPieceDefinition)))
			{
				--TrackBuilder.SharedInstance.ActiveJunctions;
			}
		}
		
		//-- Delete old pieces
		while((OldTrackPieces.Count > MinPieceHistoryToKeep)||(OldTrackPieces.Count > 0 && !OldTrackPieces[0].isVisible()))
		{
			TrackPiece deadPiece = OldTrackPieces[0];
			OldTrackPieces.RemoveAt(0);
			deadPiece.ClearMeshPieces();
			deadPiece.DestroySelf();
		}	
	}
	

	
//	private int _i = 0;
//	private Vector3 _last = Vector3.zero;
//	private Vector3 _current = Vector3.zero;
//	private Vector3 _calcPoint = Vector3.zero;
//	private float	_segmentAlpha = 0.0f;
//	private float  	_travelD = 0.0f;
//	private float	_pieceDist = 0.0f;
//	private float	_segmentDist = 0.0f;
	
	//private Rigidbody myRigidBody;
	
	public int CurrentSegment { get; private set; }
	
	private bool autoTurn = false;
	
	private void CalcPointAlongTrackPiece(TrackPiece track, float previousY, out Vector3 pointAlongTrack, out Vector3 forwardDirection)
	{
		pointAlongTrack = CurrentPosition;
		forwardDirection = currentDirection;
		
		
		
		if(OnTrackPiece == null)
			return;
		
		int maxVerts = GameController.SharedInstance.pieceMaxVerts;
		int maxTris = GameController.SharedInstance.pieceMaxTris;
		
		float distanceToAdd = (OnTrackPiece.IsTransitionTunnel()&&(EnvironmentSetSwitcher.SharedInstance.TransitionState != EnvironmentSetSwitcher.SwitchState.waitingToExitTunnel))?TunnelPieceDistanceToAdd:_pieceDistanceToAdd;

		AddTrackPieceToEnd(OnTrackPiece, distanceToAdd, maxVerts, maxTris, 0 , 0);
		
		List<Vector3> 	generatedPath = null;
		//List<Vector3> 	generatedPathNormals = null;
		//List<float>		generatedPathSegmentDistances = null;
		
		if(OnTrackPiece.UseAlternatePath == true)
		{
			//TR.LOG ("Choosing altpath1");
			generatedPath = OnTrackPiece.Alternate_GeneratedPath;
			//generatedPathNormals = OnTrackPiece.Alternate_GeneratedPathNormals;
			//generatedPathSegmentDistances = OnTrackPiece.Alternate_GeneratedPathSegmentDistances;
		}
		else
		{
			generatedPath = OnTrackPiece.GeneratedPath;
			
			if(generatedPath==null || generatedPath.Count<=1)
			{
				notify.Warning("WARNING! generated path not created for track piece '"+ OnTrackPiece.gameObject.name + "' (" + OnTrackPiece.TrackType + ") . Attempting to create it during the run.");
				OnTrackPiece.CreateSpline();
			
				generatedPath = OnTrackPiece.GeneratedPath;
			}
			//generatedPathNormals = OnTrackPiece.GeneratedPathNormals;
			//generatedPathSegmentDistances = OnTrackPiece.GeneratedPathSegmentDistances;
		}
		
		if(generatedPath == null || CurrentSegment+1 >= generatedPath.Count)
		{
			//TR.LOG ("Choosing next piece {0}, {1} >= {2}", generatedPath == null, CurrentSegment+1, generatedPath.Count);
			turnLeftNextSegment = false;
			turnRightNextSegment = false;
			if(OnTrackPiece.UseAlternatePath == true && OnTrackPiece.Alternate_NextTrackPiece != null)
			{
				ChooseNextTrackPiece(OnTrackPiece.Alternate_NextTrackPiece);
			}
			else{
				ChooseNextTrackPiece(OnTrackPiece.NextTrackPiece);
			}
			
			CurrentSegment = 0;
			if(OnTrackPiece == null)
				return;
			
			//-- Update our array ptrs
			if(OnTrackPiece.UseAlternatePath == true)
			{
				//TR.LOG ("Choosing altpath2");
				generatedPath = OnTrackPiece.Alternate_GeneratedPath;
				//generatedPathNormals = OnTrackPiece.Alternate_GeneratedPathNormals;
				//generatedPathSegmentDistances = OnTrackPiece.Alternate_GeneratedPathSegmentDistances;
			}
			else
			{
				generatedPath = OnTrackPiece.GeneratedPath;
				//generatedPathNormals = OnTrackPiece.GeneratedPathNormals;
				//generatedPathSegmentDistances = OnTrackPiece.GeneratedPathSegmentDistances;
			}
		}
		
		
		//-- Calc our XZ movement for this step.
		float dV = (Time.deltaTime*getRunVelocity());
		Vector3 movement = Vector3.zero;
		
		pointAlongTrack = _cachedTransform.position;
		pointAlongTrack.y = previousY;
		
		if(generatedPath.Count==0)
		{
			notify.Error("Track Piece "+OnTrackPiece.gameObject.name+"' does not have a generated path!",OnTrackPiece.gameObject);
			return;
		}
		
		//-- Calc the segment properties. Should be precalc'ed.
		Vector3 start = generatedPath[CurrentSegment];
		Vector3 end = generatedPath[CurrentSegment+1];
		
		Vector3 segmentNormal = end-start;
		float segmentDistance = segmentNormal.magnitude;
		segmentNormal/=segmentDistance;
		
		//-- Decide if we are autoTurning.
		autoTurn = !OnTrackPiece.IsTurn() || HasBoost || HasInvincibility;
		if(autoTurn == true)
		{
			//Debug.Break();
		}
//		if(IsJumping == true || IsSliding == true)
//			autoTurn = true;
		
		if(IsFalling == true)
		{
			autoTurn = false;
		}
		
		//-- Move forward, temporarily
		movement = dV*forwardDirection;
		pointAlongTrack += (movement);
		
		//-- Check if cross over into next segment,
		//-- if so, and we are autoturning, 
		//-- Project onto next segment, also checking if next segment is on the next piece.
		if(IsOverGround == true)
		{
			//-- Tie your self to the path unless you are falling.
			Vector3 pointAlongSegment = pointAlongTrack - start;
			float distanceFromStartOfSegment = Vector3.Dot(pointAlongSegment, segmentNormal);
			
			//-- Move forward segments if needed
			int localSegment = CurrentSegment;
			while(distanceFromStartOfSegment > segmentDistance)
			{
				distanceFromStartOfSegment-=segmentDistance;
				
				CurrentSegment++;
				if(CurrentSegment+1 >= generatedPath.Count)
				{
					turnLeftNextSegment = false;
					turnRightNextSegment = false;
					if(OnTrackPiece.UseAlternatePath == true && OnTrackPiece.Alternate_NextTrackPiece != null)
					{
						ChooseNextTrackPiece(OnTrackPiece.Alternate_NextTrackPiece);
					}
					else{
						ChooseNextTrackPiece(OnTrackPiece.NextTrackPiece);
					}
					
					//ChooseNextTrackPiece(OnTrackPiece.NextTrackPiece);
					CurrentSegment = 0;
					if(OnTrackPiece == null)
						return;
					
					if(OnTrackPiece.UseAlternatePath == true)
					{
						//TR.LOG ("Choosing altpathX");
						generatedPath = OnTrackPiece.Alternate_GeneratedPath;
						//generatedPathNormals = OnTrackPiece.Alternate_GeneratedPathNormals;
						//generatedPathSegmentDistances = OnTrackPiece.Alternate_GeneratedPathSegmentDistances;
					}
					else
					{
						generatedPath = OnTrackPiece.GeneratedPath;
						//generatedPathNormals = OnTrackPiece.GeneratedPathNormals;
						//generatedPathSegmentDistances = OnTrackPiece.GeneratedPathSegmentDistances;
					}
				}
				//-- Setting this will trigger events on the track pieces. Listeners will get notified.
				//TR.LOG ("pre changing segments {0}", OnTrackPiece);
				OnTrackPiece.CurrentSegment = CurrentSegment;
				//TR.LOG ("post changing segments {0}", OnTrackPiece);
				
				if(OnTrackPiece.UseAlternatePath == true)
				{
					generatedPath = OnTrackPiece.Alternate_GeneratedPath;
					//TR.LOG ("Choosing altpathX {0}", generatedPath);
					//generatedPathNormals = OnTrackPiece.Alternate_GeneratedPathNormals;
					//generatedPathSegmentDistances = OnTrackPiece.Alternate_GeneratedPathSegmentDistances;
				}
				else
				{
					generatedPath = OnTrackPiece.GeneratedPath;
					//TR.LOG ("Choosing pathX {0},{1}", OnTrackPiece, generatedPath);
					//generatedPathNormals = OnTrackPiece.GeneratedPathNormals;
					//generatedPathSegmentDistances = OnTrackPiece.GeneratedPathSegmentDistances;
				}
				
				//TR.ASSERT(generatedPath != null, "CRASH BOOM!");
				try
				{
					start = generatedPath[CurrentSegment];
				}
				catch (System.Exception e)
				{
					notify.Error("Exception " + e + " on track " + 	OnTrackPiece.TrackType + " CurrentSegment=" + CurrentSegment);
					throw;
				}
				try
				{
					end = generatedPath[CurrentSegment+1];
				}			
				catch (System.Exception e)
				{
					notify.Error("Exception " + e + " on track " + 	OnTrackPiece.TrackType  + " CurrentSegment=" + CurrentSegment);
					throw;
				}
				
				segmentNormal = end-start;
				segmentDistance = segmentNormal.magnitude;
				segmentNormal/=segmentDistance;
				
				pointAlongSegment = pointAlongTrack - start;
				distanceFromStartOfSegment = Vector3.Dot(pointAlongSegment, segmentNormal);
				//-- Break for same segment
				if(localSegment == CurrentSegment)
					break;
			}
			
			//if(autoTurn == true)
			{
				forwardDirection = segmentNormal;
				pointAlongSegment = segmentNormal * distanceFromStartOfSegment;
				pointAlongSegment += start;
				pointAlongTrack = pointAlongSegment;
				
				//-- Take care that our chosen currentDirection has a magnitude
				forwardDirection = segmentNormal;
				
				//-- Make the player stay upright through the jump, ignornig any pitch in the piece below.
				//if(IsJumping == true || OnTrackPiece.IsAZipLine == true)
//				if(IsOnMineCart == false)
//					forwardDirection.y = 0.0f;
			}
		}
	}
	
	
	private LayerMask lastCollidedLayer = 0;
	private bool longSlide = false;
	
	private float oldDistBetweenCoins;
	private int oldMaxCoins;
	
	void OnTriggerEnter(Collider other) 
	{
//		TR.LOG("Entering: {0}, layer={1}", other, other.gameObject.layer);
//		TR.LOG("CoinMask: {0}", CoinLayerMask.value);
//		TR.LOG("TurnColliderLayerMask: {0}", TurnColliderLayerMask.value);
//		TR.LOG("ObstacleColliderLayerMask: {0}", ObstacleColliderLayerMask.value);
//		TR.LOG("ShadowBoxLayerMask: {0}", ShadowBoxLayerMask.value);
//		TR.LOG("BalloonLayerMask: {0}", BalloonEntranceMask.value);
		if(other.gameObject != null)
		{
			//Slide Mechanic
//			if(other.tag == "SlideStart")
//			{
//				longSlide = true;
//				
//				oldDistBetweenCoins = BonusItemProtoData.SharedInstance.MinDistanceBetweenCoinRuns;
//				BonusItemProtoData.SharedInstance.MinDistanceBetweenCoinRuns = 0f;
//				
//				oldMaxCoins = BonusItemProtoData.SharedInstance.MaxCoinsPerRun;
//				BonusItemProtoData.SharedInstance.MaxCoinsPerRun = 5;
//				
//				
//				
//				Slide(true);
//			}		
//			
//			else if(other.tag == "SlideEnd")
//			{
//				StopSliding();
//				longSlide = false;
//				BonusItemProtoData.SharedInstance.MinDistanceBetweenCoinRuns = oldDistBetweenCoins;
//				BonusItemProtoData.SharedInstance.MaxCoinsPerRun = oldMaxCoins;
//			}

			
			//Monkey Pressure in Catacombs
			if(other.tag == "MonkeyPressureOn")
			{
				IsInCatacombs = true;	
			}
			
			else if(other.tag == "MonkeyPressureOff")
			{
				IsInCatacombs = false;	
			}	
			//
			
			int gameObjectLayer = other.gameObject.layer;
			
			if((gameObjectLayer == TurnColliderLayerMask || gameObjectLayer == WaterColliderLayerMask) 
				&& HasBoost == false && HasInvincibility == false)
			{
				if (!OnTrackPiece.IsTurn() && IsJumping && IsJumpingUpPhase) {
					notify.Debug("Ignored Collision with turn or water. Falling: {0} JumpingUp: {1}", IsFalling, IsJumpingUpPhase);
				} else {
					//Debug.Break ();
					IsOverGround = false;
					IsOnMineCart = false;
					doPlayAnimation(AnimType.kDeathFall);
					lastCollidedLayer = gameObjectLayer;
				}
			}
			else if(gameObjectLayer == ObstacleColliderLayerMask && HasBoost == false && HasInvincibility == false)
			{
				if (!OnTrackPiece.IsTurn() && !OnTrackPiece.IsJunction() && /*OnTrackPiece.IsObstacle() && */HasPoof/* HasShield*/) {
					//EndShield();
					EndPoof();
				} else {
					switch(other.tag){
					case "impact_Rock" :    Kill(DeathTypes.SceneryRock);
											break;
					case "impact_Tree" :	Kill(DeathTypes.SceneryTree);
											break;
					case "impact_Fall" :	Kill(DeathTypes.Fall);
											break;
					case "impact_Fence" :	Kill(DeathTypes.Fence);
											break;
					case "impact_Crystal" :	AudioManager.SharedInstance.PlayCrystalSound();
											break;
					default :				Kill(DeathTypes.Unknown);	
											break;
					}
					/* eyal edit to determine death based on tags instead of pieceType
					if(OnTrackPiece.TrackType == TrackPiece.PieceType.kTPForestJumpOver) {
						Kill(DeathTypes.SceneryRock);
					}
					else if(OnTrackPiece.TrackType == TrackPiece.PieceType.kTPForestSlideUnder) {
						Kill(DeathTypes.SceneryTree);
					}
					else if(OnTrackPiece.TrackType == TrackPiece.PieceType.kTPJumpOrSlide) {
						Kill(DeathTypes.Fire);
					}
					else {
						Kill(DeathTypes.SceneryRock);	
					}
					*/
					lastCollidedLayer = gameObjectLayer;
				}
			}
			else if(gameObjectLayer == StumbleColliderLayerMask && HasBoost == false && HasInvincibility == false)
			{
				//if (!HasShield) {
				if(!HasPoof) {
					Stumble();
					lastCollidedLayer = gameObjectLayer;
				} else {
					//EndShield();	
					if(!NoStumble)
						EndPoof();
				}
			}
			else if(gameObjectLayer == FallSaverColliderLayerMask && IsJumping && HasBoost == false && HasInvincibility == false)
			{
				notify.Debug("Hit Fall Saver Collider");
				IsFalling = false;
				IsOverGround = true;
				Stumble();
				lastCollidedLayer = gameObjectLayer;
			}
			else if(gameObjectLayer == ShadowBoxLayerMask)
			{
				//--Shadowbox
				//TintPlayer(ShadowColor, Time.deltaTime * ShadowFadeSpeed);
			}		
			
			else if(gameObjectLayer == BalloonEntranceMask)
			{				
				//Debug.Log("Balloon Difficulty: " + balloonDifficulty);
				
				//	notify.Debug("Balloon" + other.gameObject + " HasInvincibility " + HasInvincibility);
				if(IsOnBalloon == false)
				{
					if(HasBoost && Settings.GetBool("invulnerable", false) == false )
						EndBoost();
					
					if(HasPoof || (HasInvincibility && Settings.GetBool("invulnerable", false) == false) )
						EndPoof();
					
					if(HasGlindasBubble && Settings.GetBool("invulnerable", false) == false )
						EndGlindasBubble();
					
					if(NoStumble){
						playerFx.StopStumbleProof();
						AudioManager.SharedInstance.StopStumbleProof();
					}
					
					speedBeforeBalloon = getRunVelocity();
					
					ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.BalloonModeEntered,1);
					
					//-- get on balloon
					IsOnBalloon = true;
					DidBalloonFail = false;
					WillBalloonFail = false;
					doPlayAnimation(AnimType.kBalloonEnter, false);
					//AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_ClimbInBasket_01);
					AudioManager.SharedInstance.PlayClip(characterSounds.enter_balloon);
					AudioManager.SharedInstance.StartBalloonMotor();
					//doPlayAnimation(balloonIdleAnim, true);
					ShowBalloon(true,0f);
					SetPlayerVelocity(GameProfile.SharedInstance.BalloonStartSpeed);
					balloonHits = 0;
					currentBalloonOffset = 0f;
					currentBalloonRotation = 0f;
					balloonVelocity = 0f;
		
					GetAnimState(AnimType.kBalloonLeanRight).layer = 2;
					GetAnimState(AnimType.kBalloonLeanLeft).layer = 3;
					
					if(Sunlight != null)
					{
						Sunlight.EnableShadow(false);
					}
					/*if(shadowProjector!=null && shadowCamera!=null)
					{
						shadowProjector.enabled = false;
						shadowCamera.enabled = false;
					//	shadowProjector.orthographicSize = 10f;
					//	shadowCamera.orthographicSize = 10f;
					}
					
					if(blobShadow!=null)
					{
						blobShadow.renderer.enabled = false;
					}*/
					// we need to turn off the balloon that's on preBalloon piece
					//try this piece and the last piece, since the collider that triggers this is right in the middle
					BalloonPrePiece bpp = OnTrackPiece.PreviousTrackPiece.transform.GetComponentInChildren<BalloonPrePiece>();
					if(bpp){
						bpp.HideBalloon();
					}
					bpp = OnTrackPiece.transform.GetComponentInChildren<BalloonPrePiece>();
					if(bpp){
						bpp.HideBalloon();
					}
					
					// Analytics
					balloonCoinsStart = CoinCountTotal;
					balloonGemsStart = GemCountTotal;	
				}
				else //Balloon ending
				{
					doPlayAnimation(AnimType.kBalloonExitFail);
					AudioManager.SharedInstance.StopBalloonMotor();
					doPlayAnimation(AnimType.kRun, true);	
					
					if(!WillBalloonFail)
					{
						ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.BalloonModeFinished,1);
						
						//Balloon difficulty count increased if player is successful
						balloonDifficulty++;
						AddScore(1000);
						//Debug.Log ("Score for Balloon");
					}
						
				//	notify.Debug("go out of balloon");
					GameCamera.SharedInstance.BalloonXOffset = 0;
					currentBalloonRotation = 0f;
					balloonVelocity = 0f;
					IsOnBalloon = false;
					DidBalloonFail = false;
					WillBalloonFail = false;
					doSetAnimationTime(0f);
					ShowBalloon(false,1.5f);
					
					Transform characterModelCachedTransform = CharacterModel.transform;
					characterModelCachedTransform.localPosition = Vector3.zero;
					characterModelCachedTransform.localRotation = Quaternion.identity;
					
					GetComponent<SphereCollider>().center = Vector3.up*0.6f;
					
					//Set speed to 92.5% of when the player went into the balloon
					
					if(speedBeforeBalloon > 15f)
						SetPlayerVelocity(speedBeforeBalloon * 0.925f);
					
					else
						SetPlayerVelocity(speedBeforeBalloon);
					
					if(Sunlight != null)
					{
						Sunlight.EnableShadow(true);
					}
/*					if(shadowProjector!=null && shadowCamera!=null)
					{
						shadowProjector.enabled = true;
						shadowCamera.enabled = true;
						shadowProjector.orthographicSize = 1.5f;
						shadowCamera.orthographicSize = 1.5f;
					}
					
					
					if(blobShadow!=null)
					{
						blobShadow.renderer.enabled = true;
					}*/
					if(NoStumble){
						playerFx.StartStumbleProof();	
						AudioManager.SharedInstance.StartStumbleProof();
					}
					
					//Revert to normal difficulty
					GameController.SharedInstance.ResetBalloonDifficulty();
					
					/* jonoble: Commented out to see if it improves performance
					AnalyticsInterface.LogInAppCurrencyActionEvent(
						CostType.Coin, 
						CoinCountTotal - balloonCoinsStart,
						"balloon", 
						"",
						0,
						"",
						GameProfile.GetAreaCharacterString()
					);	
					AnalyticsInterface.LogInAppCurrencyActionEvent(
						CostType.Special, 
						GemCountTotal - balloonGemsStart,
						"balloon", 
						"",
						0,
						"",
						GameProfile.GetAreaCharacterString()
					);
					*/
					//	AnalyticsInterface.StoreBalloonSequenceStats( balloonCoinsStart, CoinCountTotal, balloonGemsStart, GemCountTotal );
				}
			}
			else if(gameObjectLayer == BalloonFailMask)
			{
				//notify.Debug("Fail?");
				if(IsOnBalloon == true && Time.time-lastBalloonHit>0.5f)
				{
					GameController.SharedInstance.ShowBalloonDamage();
					AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_BalloonBasketImpact_01);
					balloonHits++;
					lastBalloonHit = Time.time;
					GameCamera.SharedInstance.Shake(0.15f, 1.0f, 1.0f);
					if(WillBalloonFail)
					{
						doAdjustAnimationSpeed(AnimType.kBalloonStumbleFail,0.75f);
						doPlayAnimation(AnimType.kBalloonStumbleFail);
					}
					else
						doPlayAnimation(AnimType.kBalloonStumble);
					
					if(balloonHits>=2)
					{
					//	notify.Debug("Fail!");
						WillBalloonFail = true;
					//	balloonIdleAnim = AnimType.kBalloonPanic;
						doPlayAnimation(AnimType.kBalloonPanic,true);
						
						//Revert to normal difficulty
						GameController.SharedInstance.ResetBalloonDifficulty();
						
						//Debug.Log("Balloon Spawned turned off because of exit");
						
					if(speedBeforeBalloon > 15f)
						SetPlayerVelocity(speedBeforeBalloon * 0.925f);
					
					else
						SetPlayerVelocity(speedBeforeBalloon);
					}
				}
			}
			
			else if(other.tag == "balloonfogstart")
			{
				RenderSettings.fogDensity = 0.015f;				
				RenderSettings.fogStartDistance = 5;
				RenderSettings.fogEndDistance = 100;
				RenderSettings.fogColor = new Color(0.745f, 0.957f, 0.98f, 1.0f);
					
				SkyboxMaterials skyboxMaterials = GetSkyboxMaterials();
				if(skyboxMaterials) skyboxMaterials.SetTintValue(0.3f);
			}

			else if(other.tag == "balloonfogend")
			{
				RenderSettings.fogDensity = 0f;					
				RenderSettings.fogStartDistance = 99;
				RenderSettings.fogEndDistance = 100;
				SkyboxMaterials skyboxMaterials = GetSkyboxMaterials();
				if(skyboxMaterials) skyboxMaterials.SetTintValue(0.0f);				
			}	
			
			else if(other.tag == "CatacombsSkyboxStart")
			{
				SkyboxMaterials skyboxMaterials = GetSkyboxMaterials();
				if(skyboxMaterials) 
				{
					skyboxMaterials.SetTintColorToBlack();
					skyboxMaterials.FadeInTint(0.5f, 1.0f);
				}
			}
			
			else if(other.tag == "CatacombsSkyboxEnd")
			{	
				SkyboxMaterials skyboxMaterials = GetSkyboxMaterials();
				if(skyboxMaterials) 
				{
					skyboxMaterials.ResetToOriginalTintColor();
					skyboxMaterials.FadeOutTint(0.5f);
				}
			}				
			
		}
    }
	private float lastBalloonHit = 0f;
	
	private int balloonHits = 0;
	
	// Analytics
	private int balloonCoinsStart = 0;
	private int balloonGemsStart = 0;
	
	/*public void TurnOffIdol() {
		//if(idolRoot == null)
		//	return;
		
		//idolRoot.gameObject.SetActiveRecursively(false);
	}*/
	
	public void StartFogFadeOut()
	{
		fogTransitionSign = -1;
		fogFadeOutSpeed = 1.3f;		
				
		SkyboxMaterials skyboxMaterials = GetSkyboxMaterials();
		if(skyboxMaterials) 
		{
			skyboxMaterials.FadeOutTint(1.5f);
			skyboxMaterials.ResetToOriginalTintColor();
		}
		else
		{
			Debug.Log ("***No skybox material to fade");
		}
	}
	
	public SkyboxMaterials GetSkyboxMaterials()
	{
		SkyboxMaterials skyboxMaterials = GameController.SharedInstance.OzSkyBox.GetComponentInChildren<SkyboxMaterials>();
		return skyboxMaterials;
	}
	
	//private float tintLerpAmount = 0.0f;
	//private float lerpSpeed = 1.0f;
	//private Color tintToColor = Color.white;
	//-- Call this to setup a tint ramp.
	public void TintPlayer(Color tintColor, float time = 1.0f)
	{
		//Debug.Log("Tint: "+tintColor);
		//TR.LOG ("TintPlayer({0}, {1})", tintColor, lerpAmount);
		//tintLerpAmount = lerpAmount;
		//tintToColor = tintColor;
		//lerpSpeed = speed;
		StartCoroutine(TintPlayer_internal(tintColor,time));
	}
	
	private void SetPlayerColor(Color tint)
	{
		if (RenderObject != null && RenderObject.material != null &&  RenderObject.material.HasProperty("_Color"))
		{
			RenderObject.material.color = tint;
		//	Debug.Log(tint);
		}
		else
		{
			//notify.Debug("SetPlayerColor failed");
		//	Debug.LogWarning("No color!");
		}
	}
	
	private int int_col_id = 0;
	public IEnumerator TintPlayer_internal(Color tintColor, float time = 1f)
	{
		//Debug.Log("Tinting... "+tintColor+" " + time);
		if(time<=0.01f)	
		{ 
			SetPlayerColor(tintColor); yield break; 
		}
		
		int myId = ++int_col_id;
		
		Color start = Color.black;
		if (RenderObject != null && RenderObject.material != null && RenderObject.material.HasProperty("_Color"))
		{
			start = RenderObject.material.color;
		}
		float t = 0f;
		
		while(t!=1f && myId==int_col_id)
		{
			t = Mathf.MoveTowards(t, 1f, Time.deltaTime / time);
			SetPlayerColor(Color.Lerp(start,tintColor,t));
			yield return null;
		}
	}


    void OnTriggerExit(Collider other) 
	{
		
		if(RenderObject != null && other.gameObject != null)
		{
			int gameObjectLayer = 1 << other.gameObject.layer;
			
			if(gameObjectLayer == ShadowBoxLayerMask) {
				//TintPlayer(Color.white, Time.deltaTime * ShadowFadeSpeed);	
				
			} else {
				//TintPlayer(Color.white,  2f);	
			}
			
		}
    }
	
    
    private bool turnLeftNextSegment = false;
    private bool turnRightNextSegment = false;
	private bool fallNextSegment = false;
    public void doTurnLeft()
	{
	//	if(HasBoost) {
	//		BankLeft();
	//	}
		
		turnLeftNextSegment = true;
		turnRightNextSegment = false;
		
		if(justTurned && !OnTrackPiece.NextTrackPiece.IsTurn())	return;
		
		if (!GameController.SharedInstance.IsTutorialMode && !OnTrackPiece.IsTransitionTunnel() && !IsSliding && 
			!IsJumping && !IsOnBalloon && !dying && !HasBoost && (!HasGlindasBubble||isEndingBubble)
			&& !GamePlayer.SharedInstance.IsAnimPlaying(AnimType.kBalloonExit) && !GamePlayer.SharedInstance.IsAnimPlaying(AnimType.kBalloonExitFail))
		{
			doPlayAnimation(GamePlayer.AnimType.kTurnLeft);
			doPlayAnimation(GamePlayer.AnimType.kRun,true,false);
			
			if(AudioManager.SharedInstance != null)
			{
				//AudioManager.SharedInstance.PlayCharacterSound(AudioManager.SharedInstance.turnLeft);
				//AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_slide_ww01);
				AudioManager.SharedInstance.PlayClip(characterSounds.turn_left);
			}
		}
	}

	public void doTurnRight()
	{
	//	if(HasBoost) {
	//		BankRight();
	//	}
		
		turnLeftNextSegment = false;
		turnRightNextSegment = true;
		
		if(justTurned && !OnTrackPiece.NextTrackPiece.IsTurn())	return;
		
		if (!GameController.SharedInstance.IsTutorialMode && !OnTrackPiece.IsTransitionTunnel() && !IsSliding && 
			!IsJumping && !IsOnBalloon && !dying && !HasBoost && (!HasGlindasBubble||isEndingBubble)
			&& !GamePlayer.SharedInstance.IsAnimPlaying(AnimType.kBalloonExit) && !GamePlayer.SharedInstance.IsAnimPlaying(AnimType.kBalloonExitFail)) 
		{
			doPlayAnimation(GamePlayer.AnimType.kTurnRight);
			doPlayAnimation(GamePlayer.AnimType.kRun,true,false);
			
			if(AudioManager.SharedInstance != null)
			{
				//AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_slide_ww01);
				AudioManager.SharedInstance.PlayClip(characterSounds.turn_right);
				//AudioManager.SharedInstance.PlayCharacterSound(AudioManager.SharedInstance.turnRight);
			}
		}
	}
	
	public void balloonFall()
	{
		fallNextSegment = true;
		TrackPiece trackPiece = OnTrackPiece;
		if(IsOnBalloon)
		{
			if(fallNextSegment)
			{
				//fogTransitionSign = -1;
				Vector3 start, end;
				if(trackPiece.Alternate_GeneratedPath != null && trackPiece.Alternate_GeneratedPath.Count > 0)
				{
					//notify.Debug("Alternate");
					trackPiece.UseAlternatePath = true;
					start = OnTrackPiece.Alternate_GeneratedPath[CurrentSegment];
					end = OnTrackPiece.Alternate_GeneratedPath[CurrentSegment+1];
				}
				else{
					//notify.Debug("Main");
					start = OnTrackPiece.GeneratedPath[CurrentSegment];
					end = OnTrackPiece.GeneratedPath[CurrentSegment+1];
				}
				end -= start;
				end.Normalize();
				
				//_cachedTransform.Rotate(0, 90, 0, Space.Self);
				_cachedTransform.forward = end;
				currentDirection = _cachedTransform.forward;
				
				float mag = playerVelocity.magnitude;
				playerVelocity = currentDirection;
				playerVelocity*= mag;
				
				doResetForce();
				fallNextSegment = false;
			}
		}
	}
	
	public void OnTurnLeftInput()
	{
		//if(HasBoost) {
		//	BankLeft();
		//}
		
		if(GameController.SharedInstance.IsTutorialMode && !OnTrackPiece.IsTurnLeft() && !OnTrackPiece.NextTrackPiece.IsTurnLeft())
			return;
		
		if(justTurned && !OnTrackPiece.NextTrackPiece.IsTurn())	return;
		
		if(longSlide)
			return;
		
		//Do this only if we are NOT on a turning piece, otherwise, doTurnLeft will take care of the anim
		if(!OnTrackPiece.IsTransitionTunnel() && !dying && !IsJumping && !IsSliding 
			&& !IsOnBalloon && !OnTrackPiece.IsTurn() && !HasBoost && (!HasGlindasBubble||isEndingBubble)
			&& !GamePlayer.SharedInstance.IsAnimPlaying(AnimType.kBalloonExit) && !GamePlayer.SharedInstance.IsAnimPlaying(AnimType.kBalloonExitFail))
		{
			
			doPlayAnimation(GamePlayer.AnimType.kTurnLeft);
			doPlayAnimation(GamePlayer.AnimType.kRun,true,false);
		}
	}
	
	public void OnTurnRightInput()
	{
		//if(HasBoost) {
		//	BankRight();
		//}
		
		if(GameController.SharedInstance.IsTutorialMode && !OnTrackPiece.IsTurnRight() && !OnTrackPiece.NextTrackPiece.IsTurnRight())
			return;
		
		if(justTurned && !OnTrackPiece.NextTrackPiece.IsTurn())	return;
		
		if(!OnTrackPiece.IsTransitionTunnel() && !dying && !IsJumping && !IsSliding 
			&& !IsOnBalloon && !OnTrackPiece.IsTurn() && !HasBoost && (!HasGlindasBubble||isEndingBubble)
			&& !GamePlayer.SharedInstance.IsAnimPlaying(AnimType.kBalloonExit) && !GamePlayer.SharedInstance.IsAnimPlaying(AnimType.kBalloonExitFail))
		{
			doPlayAnimation(GamePlayer.AnimType.kTurnRight);
			doPlayAnimation(GamePlayer.AnimType.kRun,true,false);
		}
	}
	
	public void BankRight()
	{
		/*if(FinleyObject.IsPlaying("Release") || !HasBoost)	return;
		
		FinleyObject["BankRight"].speed = 0.5f;
		FinleyObject["BankRight"].layer = 2;
		FinleyObject.CrossFade("BankRight",0.4f);
		//FinleyObject.CrossFadeQueued("Flying");
		animateObject["BankRight01"].speed = 0.5f;
		animateObject["BankRight01"].layer = 2;
		animateObject.CrossFade("BankRight01",0.4f);
		
		StartCoroutine(StopBank());*/
	}
	
	public void BankLeft()
	{
		/*if(FinleyObject.IsPlaying("Release") || !HasBoost)	return;
		
		FinleyObject["BankLeft"].speed = 0.5f;
		FinleyObject["BankLeft"].layer = 2;
		FinleyObject.CrossFade("BankLeft",0.4f);
		//FinleyObject.CrossFadeQueued("Flying");
		animateObject["BankLeft01"].speed = 0.5f;
		animateObject["BankLeft01"].layer = 2;
		animateObject.CrossFade("BankLeft01",0.4f);
		
		StartCoroutine(StopBank());*/
	}
	
	public IEnumerator StopBank()
	{
		yield break;
		/*yield return new WaitForSeconds(0.5f);
		
		FinleyObject["NoBank"].layer = 2;
		FinleyObject.CrossFade("NoBank");
		animateObject["NoBank"].layer = 2;
		animateObject.CrossFade("NoBank");
		*/
		/*
		FinleyObject.Rewind("BankLeft");
		FinleyObject.Rewind("BankRight");
		animateObject.Rewind("BankLeft01");
		animateObject.Rewind("BankRight01");
		FinleyObject.Stop("BankLeft");
		FinleyObject.Stop("BankRight");
		animateObject.Stop("BankLeft01");
		animateObject.Stop("BankRight01");*/
	}
	
	public bool canIgnoreStumble()
	{
		return (IsJumping || IsSliding || HasBoost || IsHangingFromWire || IsOnMineCart || IsOnBalloon
			|| HasInvincibility || GameController.SharedInstance.IsTutorialMode || OnTrackPiece.IsTransitionTunnel());
	}
	
	private bool mbBonusTypeVacuumEffect = false;
	[System.NonSerialized]
	public float VacuumDuration = 0.0f;
	//public _cachedTransform VacuumEffectObject = null;
	
	private bool mbBonusTypeBoostEffect = false;
	//public float BoostDuration = 0.0f;
	[System.NonSerialized]
	public float boostDistanceLeft = 0f;
	public float BoostDistanceLeft
	{
		get {
			float headStartUpgrade = HasSuperBoost ? GameProfile.SharedInstance.GetHeadStartUpgradeDist() : 0f;
			return boostDistanceLeft + headStartUpgrade;
		}
		set
		{
			float headStartUpgrade = HasSuperBoost ? GameProfile.SharedInstance.GetHeadStartUpgradeDist() : 0f;
			boostDistanceLeft = value - headStartUpgrade;
		}
	}
	
	//public float BoostSlowdownDistance = 50.0f;
	[System.NonSerialized]
	public float VelocityBeforeBoost = 0;
	[System.NonSerialized]
	public float VelocityStartZipline = 0.0f;
	

	//private _cachedTransform PoofEffectObject = null;

	private bool mbBonusTypeShieldEffect = false;
	[System.NonSerialized]
	public float ShieldDuration = 0.0f;
	
	public Material FlashMaterial;

	
	public bool HasBonusEffect(BonusItem.BonusItemType bonusType)
	{
		if(bonusType == BonusItem.BonusItemType.Vacuum && mbBonusTypeVacuumEffect == true)
		{
			return true;
		}

		if(bonusType == BonusItem.BonusItemType.Boost && mbBonusTypeBoostEffect == true)
		{
			return true;
		}
		

		if(bonusType == BonusItem.BonusItemType.Poof && HasInvincibility)
		{
			return true;
		}

		if(bonusType == BonusItem.BonusItemType.Shield && mbBonusTypeShieldEffect == true)
		{
			return true;
		}
		
		return false;
	}
	
	private void ResetBonusTypeEffect(BonusItem.BonusItemType specificType)
	{
		if(specificType == BonusItem.BonusItemType.None)
		{
			//-- Special case to turn off ALL.
			ResetVacuumData();
			ResetBoostData();
			ResetShieldData();
			ResetPoofData();
			ResetGlindasBubbleData();
		}
		else if(specificType == BonusItem.BonusItemType.Vacuum)
		{
			ResetVacuumData();
		}
		else if(specificType == BonusItem.BonusItemType.Boost)
		{
			ResetBoostData();
		}
		else if(specificType == BonusItem.BonusItemType.Shield)
		{
			ResetShieldData();
		}
		else if(specificType == BonusItem.BonusItemType.Poof)
		{
			ResetPoofData();
		}
		//SetAlpha(1.0f);		
	}
	
	public void StartVacuum(float duration)
	{
		if(HasBonusEffect(BonusItem.BonusItemType.Vacuum) == true)
		{
			VacuumDuration = duration;
			if(GameController.SharedInstance.IsPowerActive()){
				UIManagerOz.SharedInstance.inGameVC.coinMeter.SetPowerProgress(1f);
				UIManagerOz.SharedInstance.inGameVC.coinMeter.AnimateCoinMeter((float)duration);
			}
			return;
		}
		
		//TR.LOG("Starting Vacuum");
		HasVacuum = true;
		mbBonusTypeVacuumEffect = true;
		VacuumDuration = duration;
		
		if(playerFx) playerFx.StartMagicMagnet();
		
		AudioManager.SharedInstance.PlayMagnet();
	}
	
	private void ResetVacuumData()
	{
		mbBonusTypeVacuumEffect = false;
		HasVacuum = false;
		
		SetPowerUsed(0f);
		
		AudioManager.SharedInstance.StopMagnet();
		
		if(playerFx) playerFx.StopMagicMagnet();
	}
	
	private float PoofTimeLeft = 0f;
	public void StartPoof(float duration)
	{
		if(HasBonusEffect(BonusItem.BonusItemType.Poof) == true)
		{
			PoofTimeLeft = duration;
			if(GameController.SharedInstance.IsPowerActive()){
				UIManagerOz.SharedInstance.inGameVC.coinMeter.SetPowerProgress(1f);
				UIManagerOz.SharedInstance.inGameVC.coinMeter.AnimateCoinMeter((float)duration);
			}
			return;
		}
		
		HasPoof = true;
		PoofTimeLeft = duration;
		
		if(playerFx) {
			playerFx.StartPoofSmoke();
			playerFx.StartShieldEffects();
		}
		//origMaterial = RenderObject.material;
		//RenderObject.material = poofMaterial;
		
		//Setting colors for blinking at the end of the powerup duration
		//PoofBlinkStartColor = RenderObject.material.GetColor("_Color");
		//PoofBlinkEndColor =  new Color(0.8f,0.8f,0.8f,1f);
		
		//TR.LOG("Starting Poof");
		//Not making Oz invisble anymore... commenting out.
		//MakeInvincible(time);
		
		AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_Poof_activate,1f);
		
		AudioManager.SharedInstance.PlayPoof();
		
		if(NoStumble){
			if(playerFx) playerFx.StopStumbleProof();
			AudioManager.SharedInstance.StopStumbleProof();
		}
	}
	
	public void EndPoof()
	{
		//RenderObject.material = origMaterial;
		HasPoof = false;
		PoofTimeLeft = 0f;
		GamePlayer.SharedInstance.SetPowerUsed(0f);
		if(playerFx){
			//playerFx.StopPoofTrail ();
			playerFx.StartShieldBreak();
			playerFx.StopShieldEffects();
		}
		
		AudioManager.SharedInstance.StopPoof();
		
		AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_Poof_deactivate,1f);
		
		if(NoStumble){
			playerFx.StartStumbleProof();	
			AudioManager.SharedInstance.StartStumbleProof();
		}		
	}
	
	public void ResetPoofData()
	{		
		if(GameController.SharedInstance.DistanceTraveled > 10 && HasPoof)
		{
			AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_Poof_deactivate);
			//Debug.Log ("Shield OFF!");
		}
		
		HasPoof = false;
		PoofTimeLeft = 0f;
		
		if(playerFx){
			playerFx.StopShieldEffects();
		}
	}
	
	public void StartBoost(float distance)
	{
		if(dying || IsDead)
			return;
		
		if(HasBonusEffect(BonusItem.BonusItemType.Boost))
		{
			//TODO: Should we add time to the duration here?
			return;
		}
		
		if(HasGlindasBubble)
			EndGlindasBubble();
		
		CancelInvoke("EndBoostPart2");
		
		SetBoostTexture();
	//	if(IsHangingFromWire == true) {
	//		VelocityBeforeBoost = VelocityStartZipline;
	//	}
	//	else {
			VelocityBeforeBoost = this.getRunVelocity();	
	//	}
		
		newPlayerY = 0f;
		
		timeInAir = 0f;
		
		finleyRenderer.enabled = true;
		FinleyObject.CrossFade("Grab",0.25f);
	//	FinleyObject.Rewind("Grab");
		FinleyObject.CrossFadeQueued("Flying");
		
		doPlayAnimation(GamePlayer.AnimType.kPickedUp,false,true,0.25f);
		doPlayAnimation(GamePlayer.AnimType.kFlying,true);
		
		BoostDistanceLeft = distance;
		
		if(GameController.SharedInstance.IsTutorialMode){ // finley tutorial overide
			BoostDistanceLeft = 110f;
			GameController.SharedInstance.finleyPowerUsed = true;
		}
	//	notify.Debug(BoostDistanceLeft);
		mbBonusTypeBoostEffect = true;
		if(playerFx) playerFx.StartBoostTrail();
		
		AudioManager.SharedInstance.PlayClip(characterSounds.activate_finley);
		
		if(NoStumble)
		{
			playerFx.StopStumbleProof();
			AudioManager.SharedInstance.StopStumbleProof();
		}
	}
	
	bool boostEnding = false;
	public void EndBoost() {
		if(boostEnding || (!HasBoost && !HasFastTravel && !HasSuperBoost))	return;
		
		boostEnding = true;
		FinleyObject.CrossFade("Release");
		Invoke("FinleyRendOff",1.75f);
		
		doPlayAnimation(GamePlayer.AnimType.kPutDown);
		doPlayAnimation(GamePlayer.AnimType.kRun,true);
			
		if(playerFx) playerFx.StopBoostTrail();		
		
		Invoke("EndBoostPart2",2f);
	}
	
	public void EndBoostPart2()
	{
		ResetBonusTypeEffect(BonusItem.BonusItemType.Boost);
	//	playerVelocity.Normalize();
		//playerVelocity*=VelocityBeforeBoost; // eyal - we change that so Phil can control what speed we start after a boost
		if(VelocityBeforeBoost < playerSpeedMinAfterHeadstart)
			SetPlayerVelocity(playerSpeedMinAfterHeadstart);
		
		else
			SetPlayerVelocity(VelocityBeforeBoost); 
		
		HasSuperBoost = false;
		
		boostEnding = false;
		
		if(NoStumble){
			playerFx.StartStumbleProof();	
			AudioManager.SharedInstance.StartStumbleProof();
		}
		
		CancelInvoke("EndBoostPart2");
		
		EndOzFlash();
	}
	
	bool flashing = false;
	IEnumerator DoOzFlash()
	{
		if(FlashMaterial==null)	yield break;
		
		Material[] ozMats = RenderObject.materials;
		
		if(ozMats.Length<2)
		{
			Material[] newMats = new Material[2];
			newMats[0] = ozMats[0];
			newMats[1] = FlashMaterial;
			RenderObject.materials = ozMats = newMats;
		}
		
		Material targMat = ozMats[1];
		
		flashing = true;
		float t = 0f;
		float cval = 0f;
		float speed = 2f;
		Color col = new Color(1,1,1,1);
		while(flashing && targMat!=null)
		{
			t+=Time.deltaTime*speed;
		//	speed = Mathf.MoveTowards(speed,6f,Time.deltaTime*3f);
			if(t>3.5f)	speed = 9f;
			
			cval = Mathf.PingPong(t,0.55f);
			
			col.r = cval;
			col.g = cval;
			col.b = cval;
			
			targMat.color = col;
			
			yield return null;
		}
		
		if(targMat!=null)
			targMat.color = Color.white;
	}
	
	void EndOzFlash()
	{
		flashing = false;
		
		StopCoroutine("DoOzFlash");
		
		Material[] ozMats = RenderObject.materials;
		if(ozMats.Length>=2)
		{
			Material[] newMats = new Material[1];
			newMats[0] = ozMats[0];
			RenderObject.materials = newMats;
		}
	}
	
	public IEnumerator BlendAnimTo(Animation animcomp, string anim, float target, float speed)
	{
		if(animcomp[anim]==null || speed==0)	yield break;
		
		float curBlend = animcomp[anim].weight;
		
		while(curBlend!=target)
		{
			curBlend = Mathf.MoveTowards(curBlend,target,speed*Time.deltaTime);
			animcomp[anim].weight = curBlend;
			yield return null;
		}
	}
	
	private void FinleyRendOff()
	{
		finleyRenderer.enabled = false;
	}
	
	private void ResetBoostData()
	{
		ApplyPlayerTexture(preBoostTexture);
		mbBonusTypeBoostEffect = false;
	}
	
	/// <summary>
	/// Starts us fast travelling
	/// </summary>
	/// <param name='destinationEnvSetId'>
	/// Destination env set identifier.
	/// where we want to go to
	/// <param name='distanceToTravel'>
	/// approximate distance it takes to get there
	/// </param>
	public void StartFastTravel(int destinationEnvSetId)
	{
		//float distanceToTravel = 1000f;
		ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.FastTraveled,1);
		HasFastTravel = true;
		FastTravelDestinationEnvironmentSetId = destinationEnvSetId;
		FastTravelDistance = 0f;//distanceToTravel;
		StartBoost(2000f); // hopefully a buffer of 1000 is enough
		//OnFastTravel();	//true);
	}
	
	/// <summary>
	/// Ends our fast travel, give a little time for boost to wear off
	/// </summary>
	public void EndFastTravel()
	{
		if (HasFastTravel)
		{
			HasFastTravel = false;
			EndBoost();
			//OnFastTravel();	//false);	
		}
	}
	
	/// <summary>
	/// Triggered when the environment set has changed
	/// </summary>
	/// <param name='newEnvSet'>
	/// unused
	/// </param>
	public void EnvironmentStateChanged(EnvironmentSetSwitcher.SwitchState newState, int newEnvSet)
	{
		if (newState >= EnvironmentSetSwitcher.SwitchState.waitingToBeAbleToDeletePools)
		{		
			EndFastTravel();
		}
	}
	
	public void StartShield()
	{
		if(HasBonusEffect(BonusItem.BonusItemType.Shield) == true)
			return;
		
		notify.Debug("Starting Shield with Duration: {0}", ShieldDuration);
		
		mbBonusTypeShieldEffect = true;
		ShieldDuration = GameProfile.SharedInstance.GetShieldDurationBoost();
	}
	
	public void EndShield()
	{
		notify.Debug("End Shield");
		ResetBonusTypeEffect(BonusItem.BonusItemType.Shield);
	}
	
	private void ResetShieldData()
	{
		mbBonusTypeShieldEffect = false;
	}
	
	public void WhiteoutFadeIn(float fadeInSpeed, float delay)
	{
		AudioManager.SharedInstance.PlayFX(AudioManager.Effects.WhiteOut01);
				
		//If there is a delay use it otherwise just start the fade
		if(delay > 0.0f)
		{
			whiteoutDelayTime = delay;
			whiteoutDelay = true;
			whiteoutPreFlash = true;
		}
		else
			whiteoutTransitionSign = 1;
		
		whiteoutFadeInSpeed = fadeInSpeed;		
		//Flag for the entire transition delay, start, end
		whiteoutTransition = true;
	}

	public bool WhiteoutComplete()
	{
		return (RenderSettings.fogDensity >= 1.0f);
	}
	
	public void WhiteoutFadeOut(float fadeOutSpeed)
	{
		whiteoutTransitionSign = -1;	
		whiteoutFadeOutSpeed = fadeOutSpeed;			
	}	
	
//	private void ResetInvincibleData()
//	{
//		ApplyPlayerTexture(preInvincibleTexture);
//	}
	
	public void ApplyPlayerTexture(Texture texture)
	{
//		if(texture != null) {
//			RenderObject.material.mainTexture = texture;	
//		}
//		RenderObject.material.mainTextureOffset = new Vector2(0, 0);
		SetPlayerMaterial(false);
	}
	
	private Texture preBoostTexture = null;
	public void SetBoostTexture()
	{
		if(BoostTexture != null && RenderObject != null)
		{
			SetPlayerMaterial(true);
//			preBoostTexture = RenderObject.material.mainTexture;
//			RenderObject.material.mainTexture = BoostTexture;
		}
	}
	
	/// <summary>
	/// Gets the player velocity. Needed by Arrow code
	/// </summary>
	/// <returns>
	/// The player velocity.
	/// </returns> 
	public Vector3 GetPlayerVelocity()
	{
		return playerVelocity;	
	}
	
//	private Texture preInvincibleTexture = null;
//	public void SetInvincibleTexture()
//	{
//		if(BoostTexture != null && RenderObject != null)
//		{
//			SetAlpha(0.5f);
//			SetPlayerMaterial();
//			
//			if(HasBoost == true) {
//				preInvincibleTexture = preBoostTexture;
//			} else {
//				preInvincibleTexture = RenderObject.material.mainTexture;	
//			}
//			RenderObject.material.mainTexture = InvincibleTexture;
//		}
//	}
	
	void ResetCharacterModelXform() {
		if(CharacterModel == null)
			return;
		Transform characterModelCachedTransform = CharacterModel.transform;
		characterModelCachedTransform.parent = gameObject.transform;
		characterModelCachedTransform.localPosition = Vector3.zero;
		characterModelCachedTransform.localRotation = Quaternion.identity;
		PlayerCollider.center = oldPlayerColliderCenter;
		//DeathRotationQuat = Quaternion.identity;
		if(RenderObject != null) {
			Transform renderObjectTransform = RenderObject.transform;
			renderObjectTransform.parent = characterModelCachedTransform;
			renderObjectTransform.localPosition = Vector3.zero;
			renderObjectTransform.localRotation = Quaternion.identity;	
		}
		PlayerXOffset = 0f;
		notify.Debug("ResetCharacterModelXform");
	}
		//-- This was written for Resurrect.
	public bool MovePlayerToNextSafeSpot() {
		bool safeSpotFound = false;
		TrackPiece currentTrackPiece = OnTrackPiece;
		while(safeSpotFound == false) {
			if(currentTrackPiece == null)
				break;
			
			
			notify.Debug ("MovePlayerToNextSafeSpot");
			notify.Debug ("currentTrackPiece " + currentTrackPiece);
			if(currentTrackPiece.NextTrackPiece){
				notify.Debug ("NextTrackPiece " + currentTrackPiece.NextTrackPiece);
			}
			else{
				notify.Error("no nextTrackPiece  :(  we need a safe track piece" );
				
			}
			
			if(currentTrackPiece.TrackType == TrackPiece.PieceType.kTPBalloonJunction)
			{
				ChooseNextTrackPiece(currentTrackPiece.Alternate_NextTrackPiece);
				currentTrackPiece = OnTrackPiece;
			}
			else
			{
				ChooseNextTrackPiece(currentTrackPiece.NextTrackPiece);
				currentTrackPiece = OnTrackPiece;
			}
			
			if(currentTrackPiece==null)	break;
		
			//if(currentTrackPiece.IsAZipLine || currentTrackPiece.IsObstacle() || currentTrackPiece.IsTurn() || currentTrackPiece.IsBalloon()) {
			//if( currentTrackPiece.IsTurn()  || currentTrackPiece.IsBalloon()) {
			if(  currentTrackPiece.IsBalloon() || currentTrackPiece.IsJunction()) { // eyal - it's safe enough to be on a turn, just not a junction
				//Took this part out, because we should let it loop back through
				//ChooseNextTrackPiece(currentTrackPiece.NextTrackPiece);
				//currentTrackPiece = OnTrackPiece;
				continue;
			}
			
			if((currentTrackPiece.GeneratedPath == null)||(currentTrackPiece.GeneratedPath.Count < 2))
			{
				continue;
			}
			
			notify.Debug("GeneratedPath " );
			//if(GameController.SharedInstance.IsTutorialMode)
				currentPosition = (currentTrackPiece.GeneratedPath[0] + currentTrackPiece.GeneratedPath[1])/2f;
			//else
			//	currentPosition = currentTrackPiece.GeneratedPath[0];
			currentDirection = currentTrackPiece.GeneratedPath[1] - currentTrackPiece.GeneratedPath[0];
			//currentPosition = currentTrackPiece.PathLocations[0]._cachedTransform.position;
			//currentDirection = currentTrackPiece.PathLocations[1]._cachedTransform.position - currentTrackPiece.PathLocations[0]._cachedTransform.position;
			currentDirection.Normalize();
			notify.Debug("currentTrackPiece " + currentTrackPiece.name );

			//-- MOVE
			HasDeathRotation = false;
			//DeathRotationQuat = Quaternion.identity;
			
			
			OnTrackPiece = currentTrackPiece;
			OnTrackPiece.ReceiveShadow(true);
			
			CurrentSegment=0;
			_cachedTransform.parent = null;
			ResetCharacterModelXform();
			
			_cachedTransform.position = currentPosition;
			_cachedTransform.forward = currentDirection;
			
			IsOnGround = true;
			IsOverGround = true;
			IsFalling = false;
			safeSpotFound = true;
				
			if(!HasGlindasBubble||isEndingBubble)
				doPlayAnimation(AnimType.kRun, false);
			
			break;
		}
		
		return safeSpotFound;
	}
	
	
	/*private Quaternion 	DeathRotationQuat = Quaternion.identity;
	
	private void StartDeathRotation() {
		if(HasDeathRotation == true)
			return;
		notify.Debug("StartDeathRotation");
		if(OnTrackPiece.IsLedge() == true) {
			if(CharacterModel != null) {
				if(TrackBuilder.SharedInstance.IsAnyLeftLedgeType(OnTrackPiece.TrackType) == true) {
					//DeathRotationQuat = _cachedTransform.rotation * Quaternion.AngleAxis(225.0f, Vector3.up) * Quaternion.AngleAxis(-90.0f, Vector3.right);
				}
				else {
					//DeathRotationQuat = _cachedTransform.rotation * Quaternion.AngleAxis(225.0f, Vector3.up) * Quaternion.AngleAxis(90.0f, Vector3.right);
				}
				
				HasDeathRotation = true;
			}
		}
	}*/
	
	
		
	//-- Doesn't work for junctions.
	public TrackPiece FindNearestPieceOfTypeWithInDistance(TrackPiece root, float distance, TrackPiece.PieceType trackType) {
		if(root == null)
			root = OnTrackPiece;
		if(root == null)
			return null;
		
		root = root.NextTrackPiece;
		float currentD = 0.0f;
		
		while(root != null) {
			if(root.TrackType == trackType)
				return root;
			
			currentD+=root.GeneratedPathLength;
			if(currentD > distance)
				return null;
			
			root = root.NextTrackPiece;
		}
		return null;
	}
	
}
