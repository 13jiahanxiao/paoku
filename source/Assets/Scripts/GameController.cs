using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public enum GameState { PRE_RUN, IN_RUN, POST_RUN, IN_MENUS }
public class GameController : MonoBehaviour
{	
	public GameState gameState = GameState.PRE_RUN;
	
	public static GameController SharedInstance = null;
	protected static Notify notify;
	
	public static string Leaderboard_HighScores = BundleInfo.GetBundleId() + "." + "TrozHighScores";
	public static string Leaderboard_DistanceRun =  BundleInfo.GetBundleId() + "." + "TrozDistanceRun";
	
	public int	Seed = 1;
	
	//-- Game State
	public float TimeSinceGameStart { get; private set; }
	public float TimeSinceResurrectStart { get; private set; }
	public float TimeSinceLastPause { get; private set; }
	public float TimeSinceLastCoin { get; private set; }
	public float TimeSinceTutorialEnded = 0.0f;
	public float TimeSinceTutorialEndedDelay = 0.0f;
	private float TimeOfDeath = 0.0f;
	public float DistanceRemainder { get; private set; }
	public float DistanceTraveled { get; private set; }
	private float diffDistTraveled = 0;
	public float DifficultyDistanceTraveled {
		get{ return diffDistTraveled; }
		set{ diffDistTraveled = Mathf.Max(0f,value); }
	}
	public float DistanceTraveledPreviousFrame { get; private set; }
	public float DistanceTraveledSinceLastTurnSection { get; private set; }
	
	private float	EndGameDelayTime = 0.0f;
	//public float	MaxDistanceWithoutCoins { get; set; }
	//public float	MaxDistanceWithoutStumble { get; set; }
	//public float	MaxDistanceWithoutBonusItems { get; set; }
	//private int		CoinRunCount = 0;
	//private float	LastCoinPlacementHeight = 0.5f;
	//private TrackPiece.CoinPlacement 	LastCoinPlacement = TrackPiece.CoinPlacement.Center;
	
	public float	AlphaCullDistance = 80.0f;
	public float	ShowCullDistance = 120.0f;
	private float	_alphaCullDistance = 80.0f;
	private float	_showCullDistance = 120.0f;
	public float showCullDistance { get { return _showCullDistance; } }
	public int		PieceMaxVerts = 20000;
	public int		PieceMaxTris = 15000;
	private int		_pieceMaxVerts = 20000;
	private int		_pieceMaxTris = 15000;
	public int pieceMaxVerts { get { return _pieceMaxVerts; } }
	public int pieceMaxTris { get { return _pieceMaxTris; } }
	public float	EnvironmentMipMapBias = -0.5f;
	public float 	MaxTurnDistance = 10.0f;
	public float 	EnemyFollowDistance = 7.5f;
	public int		ResurrectionCost = 1;
	
	[System.NonSerialized]
	public List<int> collectedBonusItemPerRun = null;
	
	//Collected Stats
	public int StumblesThisRun { get; set; }
	public int ResurrectsThisRun { get; set; }
	public int HeadStartsThisRun { get; set; }
	
	public bool HasSpawnedGemInBalloon { get; set; }
		
	public int NeededSkyBox { get; set; }

	public bool IsGameStarted { get; set; }
	public bool IsGameOver { get ; set; }
	public bool IsResurrecting { get; set; }
	public bool IsPaused { get; set; }
	public bool IsIntroScene { get; set; }
	public bool IsTutorialMode { get; set; }
	public bool IsHandlingEndGame { get; set; }
	//public bool IsSafeToLaunchDownloadDialog { get; set; }
	
	[HideInInspector]
	public int	TutorialID = -1;
	[HideInInspector]
	public int	TutorialSegmentID = 0;
	[HideInInspector]
	public int	TutorialTrackPieceCounter = 0;
	private bool tutorialPowerUsed = false;
	[HideInInspector]
	public bool finleyPowerUsed = false;
	[HideInInspector]
	public bool tutorialEnvOn = false;
	[HideInInspector]
	public bool forceTutorialEnvOn = false;
	[HideInInspector]
	public bool forceTutorialBalloon = false;
	[HideInInspector]
	public bool tutorialBalloonOn = false;
	[HideInInspector]
	public bool didShowBalloonTilt = false;
	private float monkeyTrackDisntace = 0f;
	private bool monkeyTutorialFailed = false;
	
	private int showEnvHud;
	
	[HideInInspector]
	public bool abilityTutorialPlayed = false;
	[HideInInspector]
	public bool abilityTutorialOn = false;
	[HideInInspector]
	public bool utilityTutorialPlayed = false;
	[HideInInspector]
	public bool utilityTutorialOn = false;
	
	
	[HideInInspector]
	public bool envChangeSignAvailable = false;
	
	//Track if balloon is active
	private bool balloonSpawned = false;
	
	UIPanelAlpha tutorialObjectShowing = null;
	[HideInInspector]
	public bool AutoRestart = true;
	
	//-- Track Data
	public TrackPiece trackRoot { get; set; }
	public float TrackGrowCount = 30;
	private TrackPiece LastTurnPiece = null;
	public GameObject StartingSetPiece = null;
	private bool startingSetPieceOn = false;
	public GameObject OzSkyBox = null;
	public Material skyboxMaterial = null;
	public GameObject openingTile;
	
	//-- Player Motion Data
	public GamePlayer Player;
	public float TiltOffset = 1.0f;
	
	public class EnvironmentMaterials
	{
		public static int ExtraCount = 5;
		public Material Opaque;
		public Material Decal;
		public Material[] Extra = new Material[ExtraCount];
	}
	
	public EnvironmentMaterials TrackMaterials;
	public EnvironmentMaterials TrackFadeMaterials;
	public EnvironmentMaterials TunnelMaterials;
	public EnvironmentMaterials TunnelFadeMaterials;
	public EnvironmentMaterials BalloonMaterials;
	public EnvironmentMaterials BalloonFadeMaterials;
	
/*	public Material TrackPieceOpaqueMaterial { get; set; }
	public Material TrackPieceAlphaMaterial { get; set; }
	public Material TrackPieceDecalMaterial { get; set; }
	public Material TrackPieceDecalFadeMaterial { get; set; }
	public Material TrackPieceExtra1Material { get; set; }
	public Material TrackPieceExtra1FadeMaterial { get; set; }
	public Material TrackPieceExtra2Material { get; set; }
	public Material TrackPieceExtra2FadeMaterial { get; set; }
	
	public Material TunnelTransitionOpaqueMaterial = null;
	public Material TunnelTransitionAlphaMaterial = null;
	public Material TunnelTransitionDecalMaterial = null;
	public Material TunnelTransitionFadeDecalMaterial = null;
	public Material TunnelTransitionSpinOpaqueMaterial = null;
	public Material TunnelTransitionSpinDecalMaterial = null;
	public Material TunnelTransitionSpinOpaqueFadeInMaterial = null;
	public Material TunnelTransitionSpinDecalFadeInMaterial = null;
	
	public Material BalloonTileOpaqueMaterial = null;
	public Material BalloonTileAlphaMaterial = null;	
	public Material BalloonDecalOpaqueMaterial = null;
	public Material BalloonDecalAlphaMaterial = null;	
*/	
	public float BaseCenteringForce = 10.0f;
	
	//-- Input
	public GameInput InputController = null;
	public PCInput PCInputController = null;
	public TouchInput TouchInputController = null;
	
	public GameCamera MainCamera;
	public Enemy Monkey = null;
	
	//-- Sound
//	public int RunCycleLength;
//	public AudioManager Audio;

	//-- Count down stuff
	public bool 	IsInCountdown { get; private set; }
	private float 	TimeSinceCountdownStarted = 0;
	private bool 	IsStartOfCountDown = false;
	//private bool 	Has3SoundPlayed = false;
	private bool 	Has2SoundPlayed = false;
	private bool 	Has1SoundPlayed = false;
	
	[HideInInspector]
	public bool didStartCinematicPullback =  true;// we are removing cinematic pullback false;
	[HideInInspector]
	public bool isPlayingCinematicPullback = false;
	public GameObject cinematicStoryboard;
	[HideInInspector] 
	float fadeValue;
	
	public static int userSelectedQuality = -1;
	
	private string mMenuButtonToPressAfterGameIntro; // For push notifications and Burstly Notification Ads
	
	//private bool didShowUtilityTut = false;
	//private bool didShowAbilityTut = false;
	
	
// eyal edit
//#if UNITY_ANDROID || UNITY_EDITOR
	
	public UIIAPViewControllerOz.AndroidIAPMechanism Mechanism;
//#endif
	public enum DeviceType
	{
		Unknown,
		iPod,
		iPad,
		iPhone
	}
	
	public enum DeviceGeneration
	{
		Unsupported,
		iPodTouch3,
		iPhone3GS,
		iPodTouch4,
		LowEnd,
		iPhone4,
		iPhone4S,
		iPad2,
		iPadMini,
		MedEnd,
		iPad3,
		iPodTouch5,
		iPhone5,
		iPad4,
		HighEnd,
	}
	
	public DeviceGeneration FakeDevice = DeviceGeneration.HighEnd;
	private DeviceGeneration deviceGeneration = DeviceGeneration.Unsupported;
	private DeviceType deviceType = DeviceType.Unknown;

	public DeviceType GetDeviceType()
	{
		if(deviceType != DeviceType.Unknown)
			return deviceType;
		DeviceType device = DeviceType.Unknown;
#if UNITY_EDITOR
		device = DeviceType.Unknown;
#elif UNITY_IPHONE
		notify.Debug ("Mapping iphone device");
		switch(iPhone.generation)
		{
		default://should never happen
			device = DeviceType.Unknown;
			break;
		case iPhoneGeneration.iPodTouch3Gen:
		case iPhoneGeneration.iPodTouch4Gen:
		case iPhoneGeneration.iPodTouch5Gen:
		case iPhoneGeneration.iPodTouchUnknown:
			device = DeviceType.iPod;
			break;
		case iPhoneGeneration.iPhone3GS:
		case iPhoneGeneration.iPhone4:
		case iPhoneGeneration.iPhone4S:
		case iPhoneGeneration.iPhone5:
		case iPhoneGeneration.iPhoneUnknown:
			device = DeviceType.iPhone;
			break;
		case iPhoneGeneration.iPad2Gen:
		case iPhoneGeneration.iPad3Gen:
		case iPhoneGeneration.iPad4Gen:
		case iPhoneGeneration.iPadMini1Gen:
		case iPhoneGeneration.iPadUnknown:
			device = DeviceType.iPad;
			break;
		}
#else
		device = DeviceType.Unknown;
		// TODO figure out if this device is a high end device generation
#endif //UNITY_EDITOR/UNITY_IPHONE
		deviceType = device;
		return device;
	}	
	
	private static bool reportedDeviceGeneration = false;
	public DeviceGeneration GetDeviceGeneration()
	{
		if(deviceGeneration != DeviceGeneration.Unsupported)
			return deviceGeneration;
		DeviceGeneration device = DeviceGeneration.Unsupported;
#if UNITY_EDITOR
		device = FakeDevice;
		if(device == DeviceGeneration.Unsupported)
		{
			if (userSelectedQuality == -1 )
			{
				device = DeviceGeneration.HighEnd;
				userSelectedQuality = 2;					
			}
			else
			{
				device = GetDeviceGenerationFromSaveID(userSelectedQuality);
			}
		}
#elif UNITY_IPHONE
		switch(iPhone.generation)
		{
		default://should never happen
			notify.Debug("Unsupported iPhoneGeneration " + iPhone.generation.ToString());
			device = DeviceGeneration.Unsupported;
			break;
		case iPhoneGeneration.iPodTouch3Gen:
			device = DeviceGeneration.iPodTouch3;
			break;
		case iPhoneGeneration.iPhone3GS:
			device = DeviceGeneration.iPhone3GS;
			break;
		case iPhoneGeneration.iPodTouch4Gen:
			device = DeviceGeneration.iPodTouch4;
			break;
		case iPhoneGeneration.iPhone4:
			device = DeviceGeneration.iPhone4;
			break;
		case iPhoneGeneration.iPhone4S:
			device = DeviceGeneration.iPhone4S;
			break;
		case iPhoneGeneration.iPadMini1Gen:
			device = DeviceGeneration.iPadMini;
			break;
		case iPhoneGeneration.iPad2Gen:
			device = DeviceGeneration.iPad2;
			break;
		case iPhoneGeneration.iPad3Gen:
			device = DeviceGeneration.iPad3;
			break;
		case iPhoneGeneration.iPad4Gen:
			device = DeviceGeneration.iPad4;
			break;
		case iPhoneGeneration.iPodTouch5Gen:
			device = DeviceGeneration.iPodTouch5;
			break;
		case iPhoneGeneration.iPhone5:
			device = DeviceGeneration.iPhone5;
			break;
		case iPhoneGeneration.iPadUnknown:
			device = DeviceGeneration.HighEnd;
			break;
		case iPhoneGeneration.iPodTouchUnknown:
			device = DeviceGeneration.HighEnd;
			break;
		case iPhoneGeneration.iPhoneUnknown:
			device = DeviceGeneration.HighEnd;
			break;
		}
#elif UNITY_ANDROID
		
		if( userSelectedQuality == -1 )
		{
			// do the android device check here
			
			device = DeviceGeneration.Unsupported; 
			userSelectedQuality = 0;
			// check the list first
			
			string model = SystemInfo.deviceModel.ToLower();

			if( model.Contains("nexus 7"))
			{
				device = DeviceGeneration.MedEnd;
				userSelectedQuality = 1;
			}
			else
			if( SystemInfo.processorCount == 4)
			{
				device = DeviceGeneration.HighEnd;
				userSelectedQuality = 2;
			}
			else
			{
				if( SystemInfo.systemMemorySize >= 768 )// need 1G to be medium
				{
					device = DeviceGeneration.MedEnd;
					userSelectedQuality = 1;
				}
				else
				{
					device = DeviceGeneration.LowEnd; 
					userSelectedQuality = 0;
				}
			}
		}
		else
		{
			device = GetDeviceGenerationFromSaveID(userSelectedQuality);
		}

/*		
		Debug.Log("Device Name ============================================== " + SystemInfo.deviceName);
		Debug.Log("System Mem = " + SystemInfo.systemMemorySize);
		Debug.Log("processorCount = " + SystemInfo.processorCount);
		Debug.Log("Selection = " + device.ToString());
*/		
		
		notify.Debug("Device Name ============================================== " + SystemInfo.deviceName);
		notify.Debug("Device model " + SystemInfo.deviceModel);
		notify.Debug("System Mem = " + SystemInfo.systemMemorySize);
		notify.Debug("processorType = " + SystemInfo.processorType);
		notify.Debug("processorCount = " + SystemInfo.processorCount);
		notify.Debug("Selection = " + device.ToString());
#else
		device = DeviceGeneration.HighEnd; 
#endif //UNITY_EDITOR/UNITY_IPHONE/UNITY_ANDROID
		if (! reportedDeviceGeneration)
		{
			notify.Debug("Device : " + device.ToString());
			reportedDeviceGeneration = true;
		}
		deviceGeneration = device;
		return device;
	}
	
	public static bool IsDeviceLowEnd()
	{// quick implementation here
#if UNITY_ANDROID
			if( SystemInfo.processorCount == 4)
			{
				return false;
			}
			else
			{
				if( SystemInfo.systemMemorySize >= 768 )// need 1G to be medium
				{
					return false;
				}
			}
			return true;
#else
			return false;
#endif
	}	
	
	public static void SetUserDevicegenerationFromSaveID(int id)
	{
		userSelectedQuality = id;
		notify.Debug("Player set quality to " + id);
		PlayerPrefs.SetInt("UserSetQuality", userSelectedQuality );
		PlayerPrefs.Save();

		SharedInstance.deviceGeneration = GetDeviceGenerationFromSaveID(id);
		
		SetQualitySettings();// unity system call
		
		SharedInstance.UpdateTrackPieceParams();
		GamePlayer.SharedInstance.UpdatePieceDistanceToAdd();
		
		// do UI texture swapping if needed
		
		UIManagerOz.SharedInstance.ChooseAtlasBasedOnScreenResolution();

		
	}
	
	public static DeviceGeneration GetDeviceGenerationFromSaveID(int id)
	{
		DeviceGeneration device = DeviceGeneration.Unsupported;
		switch(id)
		{
			case 0:
				device = DeviceGeneration.LowEnd;
			break;
			case 1:
				device = DeviceGeneration.MedEnd;
			break;
			case 2:
				device = DeviceGeneration.HighEnd;
			break;
		}
		return device;
	}
	
	public int GetDeviceTier()
	{
		DeviceGeneration gen = GetDeviceGeneration();
		
		int output = 1;
		switch(gen)
		{
		case DeviceGeneration.iPhone3GS:
		case DeviceGeneration.iPodTouch3:
		case DeviceGeneration.iPodTouch4:
		case DeviceGeneration.Unsupported:
		case DeviceGeneration.LowEnd:
			output = 1;
			break;
			
		case DeviceGeneration.MedEnd:
		case DeviceGeneration.iPhone4:
			output = 2;
			break;
			
		default:
			output = 3;
			break;
		}
		return output;
	}
	
	static public void TextureReport(String name)
	{
		if(notify.CurLevel <= Notify.NotifyLevel.Debug)
		{
			{
				String report = name + " texture report:\n";
			
				UnityEngine.Object[] textures = Resources.FindObjectsOfTypeAll(typeof(Texture2D));
				String line = "Found " + textures.Length + " textures\nName,Size,Mip maps,Format\n";
				report = report + line;
				int i = 1;
				foreach (UnityEngine.Object textureObj in textures)
				{
					Texture2D texture = textureObj as Texture2D;
					line = i + "," + texture.name + "," + texture.width + " X " + texture.height + "," + texture.mipmapCount + "," + texture.format + "\n";
					report = report + line;
					++i;
				}
				
				notify.Debug(report);
			}
			{
				String report = name + " render texture report:\n";
	
				UnityEngine.Object[] textures = Resources.FindObjectsOfTypeAll(typeof(RenderTexture));
				String line = "Found " + textures.Length + " textures\nName,Size,Mip maps,Format\n";
				report = report + line;
				int i = 1;
				foreach (UnityEngine.Object textureObj in textures)
				{
					RenderTexture texture = textureObj as RenderTexture;
					line = i + "," + texture.name + "," + texture.width + " X " + texture.height + "," + texture.texelSize + "," + texture.format + "\n";
					report = report + line;
					++i;
				}

				notify.Debug(report);
			}			
		}
	}
	static public void AudioReport(String name)
	{
		if(notify.CurLevel <= Notify.NotifyLevel.Debug)
		{
			String report = name + " audio report:\n";
			
			UnityEngine.Object[] clips = Resources.FindObjectsOfTypeAll(typeof(AudioClip));
			String line = "Found " + clips.Length + " audio clips\nName,Length,Samples,Frequency\n";
			report = report + line;
			float totalTime = 0.0f;
			int totalSamples = 0;
			int i = 0;
			foreach (UnityEngine.Object clipObj in clips)
			{
				AudioClip clip = clipObj as AudioClip;
				totalTime += clip.length;
				totalSamples += clip.samples;
				line = i + "," + clip.name + "," + clip.length + "," + clip.samples + "," + clip.frequency + "\n";
				report = report + line;
				++i;
			}
			report += "Total:," + totalTime + "," + totalSamples;
			notify.Debug(report);
		}
	}

	static public void MeshReport(String name)
	{
		if(notify.CurLevel <= Notify.NotifyLevel.Debug)
		{
			String report = name + " mesh report:\n";
			
			UnityEngine.Object[] meshes = Resources.FindObjectsOfTypeAll(typeof(Mesh));
			String line = "Found " + meshes.Length + " meshes\nName,Length,Samples,Frequency\n";
			report = report + line;
			int totalTriangles = 0;
			int totalVerts =0;
			int totalNormals = 0;
			int i = 0;
			foreach (UnityEngine.Object meshObj in meshes)
			{
				Mesh mesh = meshObj as Mesh;
				totalTriangles += mesh.triangles.Length;
				totalVerts += mesh.vertexCount;
				totalNormals += mesh.normals.Length;
				line = i + "," + mesh.name + "," + mesh.triangles.Length + "," + mesh.vertexCount + "," + mesh.normals.Length + "\n";
				report = report + line;
				++i;
			}
			report += "Total:," + totalTriangles + "," + totalVerts + "," + totalNormals;
			notify.Debug(report);
		}
	}
	
	public void SetDaylight(float amount)
	{
		if(skyboxMaterial != null)
		{
			skyboxMaterial.SetFloat("_AMultiplier", amount);
		}
	}
	
	void OnEnable()
	{
		UIManagerOz.onPlayClickedHandler += OnRestartClickedUI;		
		UIManagerOz.onUnPauseClicked += OnUnPauseClickedUI;
		UIManagerOz.onPauseClicked += OnPauseClickedUI;		
		
		BonusItem.RegisterForOnBonusItemPickup(onBonusItemCollected);
	}
	
	void OnDisable()
	{
		UIManagerOz.onPlayClickedHandler -= OnRestartClickedUI;			
		UIManagerOz.onUnPauseClicked -= OnUnPauseClickedUI;
		UIManagerOz.onPauseClicked -= OnPauseClickedUI;
		
		BonusItem.UnRegisterForOnBonusItemPickupEvent(onBonusItemCollected);
	}
	
	
	
	public static void SetQualitySettings()
	{
		//-- set this to max so ANY NEW Device always gets the MAX.
		int qualityLevel = 2;
		
		switch(SharedInstance.GetDeviceGeneration())
		{
		case DeviceGeneration.HighEnd:
		case DeviceGeneration.iPhone5:
		case DeviceGeneration.iPad4:
		case DeviceGeneration.iPodTouch5:
			qualityLevel = 2;
			break;
		case DeviceGeneration.MedEnd:
		case DeviceGeneration.iPhone4S:
		case DeviceGeneration.iPad3:
		case DeviceGeneration.iPad2:
		case DeviceGeneration.iPadMini:
			qualityLevel = 1;
			break;
		default:
			qualityLevel = 0;
			break;
		}
		QualitySettings.SetQualityLevel(qualityLevel);
		notify.Debug("Setting QualitySettings={0}", QualitySettings.GetQualityLevel());
	}
	
	
	
	void Awake ()
	{
		//TR.LOG("GAME CONTROLLER: AWAKE");
		GameController.SharedInstance = this;
		notify = new Notify(this.GetType().Name);
		
// load user selected quality for android

	#if UNITY_ANDROID
		userSelectedQuality = PlayerPrefs.GetInt("UserSetQuality", -1 );
//		userSelectedQuality = -1;
		notify.Debug("Player set quality is " + userSelectedQuality);

	#endif
		
		
		Shader.SetGlobalFloat("_AMultiplier", 0f);

		
		{
			switch(GetDeviceGeneration())
			{
			case GameController.DeviceGeneration.Unsupported:
			case GameController.DeviceGeneration.iPodTouch3:
			case GameController.DeviceGeneration.iPhone3GS:
			case GameController.DeviceGeneration.LowEnd:
				Application.targetFrameRate = 30;
				QualitySettings.vSyncCount = 1;
				break;
			case GameController.DeviceGeneration.MedEnd:
			case GameController.DeviceGeneration.iPodTouch4:
			case GameController.DeviceGeneration.iPhone4:
				Application.targetFrameRate = 40;
				//QualitySettings.vSyncCount = 0;
				break;
//			case GameController.DeviceGeneration.iPad2:
//				Application.targetFrameRate = 45;
//				QualitySettings.vSyncCount = 0;
//				break;
			default:
				Application.targetFrameRate = 60;
				break;
			}
			notify.Debug("Set target frame rate to " + Application.targetFrameRate);
		}
		
		
		//ColorCard.FadeToBlack(0f);
		//MainCamera.camera.backgroundColor = Color.black;
		
		// Alex added the FPScounter, DebugConsole, and NetAgent functionality to the 'Services' system
		/*
		GameObject fpsPrefab = Resources.Load("Prefabs/FpsCounter", typeof(GameObject)) as GameObject;
		GameObject.Instantiate(fpsPrefab);

		GameObject consolePrefab = Resources.Load("Oz/Prefabs/DebugConsole", typeof(GameObject)) as GameObject;
		GameObject.Instantiate(consolePrefab);		
		
		// TODO make this part of the real game scene, fpsPrefab and consolePrefab is optional, this is not
		GameObject netAgentPrefab = Resources.Load("Oz/Prefabs/NetAgent", typeof(GameObject)) as GameObject;
		GameObject.Instantiate(netAgentPrefab);
		*/
		 
		//Check if UIManagerOz exists, if not instaniate it
//		if (GameObject.Find("UIManagerOz") == null && (GameObject.Find("UI") == null && Application.loadedLevelName != "TheGame")) {
//			GameObject uiManagerPrefab = Resources.Load ("Prefabs/UI/UIManagerOz", typeof(GameObject)) as GameObject;
//			GameObject.Instantiate(uiManagerPrefab);	
//		}
		
		SetQualitySettings();
		
		SetMSAAByDevice();
		
		//-- Imangi based TF ID.
		// jonoble: Removed TestFlight crash handling so we can use
		//	Crittercism crash handling instead
		//TestFlightUnity.TakeOff("76f52991b905db559b6090ade51fcb66_MTA3MjIyMDEyLTA3LTI0IDE2OjI3OjQ3LjI0MDk3Ng");
		
		//-- Begin analytics tracking
		//	AnalyticsInterface.Init();
	}
	
	public void SetMSAAByDevice() {
		switch(GetDeviceGeneration())
		{
		case DeviceGeneration.iPad2:
		case DeviceGeneration.iPadMini:
			//- Best
			QualitySettings.antiAliasing = 2;	
			break;	
		default:
			//-- Low
			QualitySettings.antiAliasing = 0;
			break;
		}
	}
	
	/// <summary>
	/// Returns true if we need to use less track pieces, which will be for low end devices
	/// </summary>
	/// <returns>
	/// false for high end devices (typically those with greater than 256 MB)
	/// </returns>
	public bool LessTrackPieces()
	{
		bool result;
		switch(GetDeviceGeneration())
		{
			case DeviceGeneration.Unsupported:
			case DeviceGeneration.iPhone3GS:
			case DeviceGeneration.iPodTouch4:
				result = true;
				break;	
			default:
				result = false;
				break;
		}		
		return result;
	}
	
	
	DateTime timePaused = new DateTime(0);
	
	void OnApplicationPause (bool paused)
	{
		//TR.LOG("APP PAUSE: " + paused);
		TimeSinceCountdownStarted = -3f;
		if (paused && IsGameStarted && !IsGameOver && !Player.Dying && !Player.IsFalling) 
		{
			if(UIManagerOz.SharedInstance.inGameVC.isAbilityTutorialOn){
				Time.timeScale = 0f;
			}
			else{
				UIManagerOz.SharedInstance.inGameVC.OnPause();
			}
//			SavedGame.SaveGame();
		}
			
		AudioManager.SharedInstance.UpdateIpodMusicIsOn();
		
		if (paused)
		{
			timePaused = System.DateTime.UtcNow;
			//Time.realtimeSinceStartup;
			//Send out the numbers of deaths for this play session.
			if (GameProfile.SharedInstance.deathsPerSession > 0)
			{
				//				AnalyticsInterface.LogGameAction("run","died",GameProfile.SharedInstance.deathsPerSession.ToString(), GameProfile.GetAreaCharacterString(),0);
				GameProfile.SharedInstance.deathsPerSession = 0;
			}
		}
		
		if (!paused)
		{
			if(timePaused != new DateTime(0))
			{
				//We shouldn't need to do this...
				//AudioManager.SharedInstance.FadeMusicMultiplier(0f,0f);
				//AudioManager.SharedInstance.FadeMusicMultiplier(0.5f,1f);
				
				//Make sure the app has been suspended for more than two minutes, in case it was a notification
				TimeSpan twominutes = new TimeSpan(0,2,0);
				if(DateTime.UtcNow-timePaused >= twominutes)
				{
					//Refresh the challenges
					//Services.Get<ObjectivesManager>().GetWeeklyObjectivesClass().GetChallenges();
					Initializer.SharedInstance.OnAppForegroundRefreshChallenges();
				}
			}
		}
		
//		if (paused)
//		{
//			// run a memory cleanup only when leaving the app.  Pause menu will do it when returning, if running.
//			Resources.UnloadUnusedAssets();		
//			System.GC.Collect();
//		}
		
		if (paused == false && IsGameStarted && !IsGameOver)
		{
//			SavedGame.DeleteSavedGame();
		}
	}
	
	public void UpdateIpodMusicIsOn(string dummy)
	{
		AudioManager.SharedInstance.UpdateIpodMusicIsOn();
	}
	
	void OnApplicationQuit()
	{
//#if UNITY_ANDROID
//		FlurryAndroid.onEndSession();
//#endif
//		SavedGame.SaveGame();
		SetDaylight(1.0f);
	}
	
	public void UpdateTrackPieceParams()
	{
		_alphaCullDistance = AlphaCullDistance;
		_showCullDistance = ShowCullDistance;
		_pieceMaxVerts = PieceMaxVerts;
		_pieceMaxTris = PieceMaxTris;
		
		switch(GetDeviceGeneration())
		{
		case GameController.DeviceGeneration.Unsupported:
		case DeviceGeneration.iPhone3GS:
		case DeviceGeneration.iPhone4:
		case DeviceGeneration.iPodTouch4:
		case DeviceGeneration.LowEnd:
			break;
		case DeviceGeneration.iPodTouch5:
		case DeviceGeneration.iPhone5:
		case DeviceGeneration.iPad4:
		case DeviceGeneration.HighEnd:
			_alphaCullDistance /= 2f;
			_showCullDistance *= 2f;//1.125f;
			_pieceMaxVerts *= 2;
			_pieceMaxTris *= 2;
			break;
		case DeviceGeneration.iPad2:
		case DeviceGeneration.iPadMini:
		case DeviceGeneration.MedEnd:
			_showCullDistance *= 1.25f;
			_pieceMaxVerts = (5 * _pieceMaxVerts) / 4;
			_pieceMaxTris = (5 * _pieceMaxTris) / 4;
			break;
		default:
			_showCullDistance *= 1.5f;
			_pieceMaxVerts = (3 * _pieceMaxVerts) / 2;
			_pieceMaxTris = (3 * _pieceMaxTris) / 2;
			break;
		}		
	}
	
	void Start ()
	{		
		//		_setMenuButtonToPressAfterGameIntro( SharingManagerBinding.GetPushNotificationDestinationMenuButton() );
		
		TrackPiece.CacheLayerInfo();
		
		UpdateTrackPieceParams();
		
		Instantiate(Resources.Load("Services"));
		
		showEnvHud = PlayerPrefs.GetInt("showEnvHud", 1);
		
		AudioManager.SharedInstance.UpdateIpodMusicIsOn();
		
		
		//GetNews();	// moved to 'Services' by Alex

		//Do opening animation w/ camera, then show the UI
		// postpone this untill cinematicPullback is done
		//StartCoroutine(DoOpeningScene());
		
		//DebugTrackType = TrackPiece.PieceType.kTPMax;
		/*if(TrackPieceAlphaMaterial != null)
		{
			TrackPieceAlphaMaterial.SetFloat("_FadeOutDistFar", CullDistance);
			TrackPieceAlphaMaterial.SetFloat("_FadeOutDistNear", CullDistance*0.9f);
		}*/
		//TR.LOG("GAME CONTROLLER: START");

		
		// Generate a good random seed.
		if( Input.acceleration.x != 0.0f)
    	{
			UnityEngine.Random.seed = (int)(Input.acceleration.x * int.MaxValue);
		}
		else
		{
        	string ticks =  System.DateTime.Now.Ticks.ToString();
        	UnityEngine.Random.seed = int.Parse(ticks.Substring(ticks.Length - 8, 8 ));
    	}
		
		GameProfile.SharedInstance.Player.randomSeed = UnityEngine.Random.seed;
		
		notify.Debug("Random Seed: " + GameProfile.SharedInstance.Player.randomSeed);
		
		
#if ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR)
		notify.Debug("Init TouchInput");
		InputController = TouchInputController;
		PCInputController.enabled = false;
		PCInputController.gameObject.SetActiveRecursively(false);
#else
		
		InputController = PCInputController;
		TouchInputController.gameObject.SetActiveRecursively(false);
#endif
		InputController.gameObject.SetActiveRecursively(true);
		InputController.init(this);
		
	 
		EnvironmentSetManager.SharedInstance.PrepareCurrentEnvironmentSet( OnCurrentEnvironmentReady );

		
		
		TextureReport ("GameController.Start");
		
		Player.TintPlayer(Color.black,0f);

		SetTutorialsByPlayerPrefs();
		
		// Initialize the default sort priorities.  
		/*
		for ( int i = 0; i < PowerStore.Powers.Count; i++ )
		{
			PowerStore.Powers[i].DefaultSortPriority = i * 10 + 10;
		}
		
		for ( int i = 0; i < ArtifactStore.Artifacts.Count; i++ )
		{
			ArtifactStore.Artifacts[i].DefaultSortPriority = i * 10 + 10;
		}
		
		for ( int i = 0; i < ConsumableStore.consumablesList.Count; i++ )
		{
			ConsumableStore.consumablesList[i].DefaultSortPriority = i * 10 + 10;
		}
		*/
		
		foreach ( BasePower power in PowerStore.Powers )
		{
			power.DefaultSortPriority = power.SortPriority;
		}
		
		foreach ( ArtifactProtoData artifact in ArtifactStore.Artifacts )
		{
			artifact.DefaultSortPriority = artifact._sortPriority;
		}
		
		foreach ( BaseConsumable consumable in ConsumableStore.consumablesList )
		{
			consumable.DefaultSortPriority = consumable.SortPriority;
		}
		
		for ( int i = 0; i < GameProfile.SharedInstance.CharacterOrder.Count; i++ )
		{
			int characterIndex = GameProfile.SharedInstance.CharacterOrder[i];
			
			GameProfile.SharedInstance.Characters[ characterIndex ].DefaultSortPriority = i * 10 + 10;
		}
		
	}
	
	public void SetTutorialsByPlayerPrefs()
	{
		int abilityTutorialPlayedInt = PlayerPrefs.GetInt("abilityTutorialPlayedInt");
		
		if ( abilityTutorialPlayedInt == 1 ) 
		{
			abilityTutorialPlayed = true;
		}
		else  
		{
			abilityTutorialPlayed = false;
		}
		
		int utilityTutorialPlayedInt = PlayerPrefs.GetInt("utilityTutorialPlayedInt");
		
		if ( utilityTutorialPlayedInt == 1 )
		{
			utilityTutorialPlayed = true;
		}
		else  
		{
			utilityTutorialPlayed = false;
		}
	}
		
	IEnumerator InitTracks()
	{
		ResourceManager.AllowLoad = true;
		
		yield return StartCoroutine(TrackPiece.WarmPrefabsCoroutine());	
		
		ResourceManager.AllowLoad = false;		
//		TrackPiece.WarmPrefabs();
		EnvironmentSetSwitcher.SharedInstance.loadNewTrackMaterial(TrackBuilder.SharedInstance.CurrentEnvironmentSetId);
		EnvironmentSetSwitcher.SharedInstance.ReloadEnvAudio(TrackBuilder.SharedInstance.CurrentEnvironmentSetId);
		GameController.SharedInstance.NeededSkyBox = TrackBuilder.SharedInstance.CurrentEnvironmentSetId;
		GameController.SharedInstance.SpawnOZSkyBox();
		
		//-- Start preloading assets.
		TrackPiece.WarmPools();
		StartCoroutine (WarmupResources());
		
		SetDaylight(1.0f);
		

		//UIManagerOz.SharedInstance.ShowPowerIconAndGlow(false);
		//EnvironmentSetSwitcher.SharedInstance.loadNewTrackMaterial(TrackBuilder.SharedInstance.CurrentEnvironmentSetId);

		//pick opening tile
		notify.Debug("OnCurrentEnvironmentReady  SpawnOpeningTile");
		SpawnOpeningTile ();
		
		TutorialID = -1;
		TutorialSegmentID = 0;
		//Pick opening skybox
//		NeededSkyBox = TrackBuilder.SharedInstance.CurrentEnvironmentSetId;
	//	StartCoroutine(SpawnOZSkyBoxCoroutine ());

		Player.RegisterForOnTrackPieceChange(OnTrackChange);
		

		ResetLevelInformation();
		CreateInitialTrack();

//TutorialSprite.enabled = false;
//TutorialTiltSprite.enabled = false;		
	}
	
	
	/// <summary>
	/// Startup everything else that is environment set specific
	/// </summary>
	public void OnCurrentEnvironmentReady()
	{	
		if ( Application.platform == RuntimePlatform.Android &&  EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.IsEmbeddedAssetBundle())
		{
			StartCoroutine(InitTracks());
		}
		else
		{
	
			TrackPiece.WarmPrefabs();
			EnvironmentSetSwitcher.SharedInstance.loadNewTrackMaterial(TrackBuilder.SharedInstance.CurrentEnvironmentSetId);
			EnvironmentSetSwitcher.SharedInstance.ReloadEnvAudio(TrackBuilder.SharedInstance.CurrentEnvironmentSetId);
			GameController.SharedInstance.NeededSkyBox = TrackBuilder.SharedInstance.CurrentEnvironmentSetId;
			GameController.SharedInstance.SpawnOZSkyBox();
			
			//-- Start preloading assets.
			TrackPiece.WarmPools();
			StartCoroutine (WarmupResources());
			
			SetDaylight(1.0f);
			
	
			//UIManagerOz.SharedInstance.ShowPowerIconAndGlow(false);
			//EnvironmentSetSwitcher.SharedInstance.loadNewTrackMaterial(TrackBuilder.SharedInstance.CurrentEnvironmentSetId);
	
			//pick opening tile
			notify.Debug("OnCurrentEnvironmentReady  SpawnOpeningTile");
			SpawnOpeningTile ();
			
			TutorialID = -1;
			TutorialSegmentID = 0;
			//Pick opening skybox
	//		NeededSkyBox = TrackBuilder.SharedInstance.CurrentEnvironmentSetId;
		//	StartCoroutine(SpawnOZSkyBoxCoroutine ());
	
			Player.RegisterForOnTrackPieceChange(OnTrackChange);
			
	
			ResetLevelInformation();
			CreateInitialTrack();
		}
	}

	IEnumerator WarmupResources ()
	{
#if UNITY_IOS	
		/* jonoble: Moved GameCenter initialization to Initializer.cs
		if(GameCenterBinding.isGameCenterAvailable() == true) {
			GameCenterBinding.authenticateLocalPlayer();
		}
		*/
#endif
		yield return new WaitForEndOfFrame();
		Shader.WarmupAllShaders();
		//TrackPiece.WarmResources ();
	}
	
	
	//bool isPlaying = false;
	public bool needSetTrackVisibility = false;

	public static bool isLoadingIconOn = false;
	public AudioClip BaboonSound;
	private IEnumerator DoOpeningScene()
	{
		notify.Debug("DoOpeningScene");
		GameCamera.SharedInstance.enabled = false;
		Animation animateRoot = GameCamera.SharedInstance.transform.parent.parent.animation;
		
		//Go to black over the hitches at the start
		while(!AudioManager.SharedInstance.SFXready)
		{
			yield return null;
		}
		yield return null;
		
		if( isLoadingIconOn )
		{
			isLoadingIconOn = false;
			//			SharingManagerBinding.HideBusyIndicator();
//			Debug.LogError("3333333333");
		}		
		
		if ( mMenuButtonToPressAfterGameIntro == null )
		{
			// Play the launch music if we're not launching from a push notification
			AudioManager.SharedInstance.StartLaunchMusic();
		}
		
		yield return new WaitForSeconds(0.1f);
		yield return null;
		
		iTween.ValueTo(gameObject, iTween.Hash(
			"time", 2f,
			"from", 0f,
			"to", 4f,
			"onupdate", "OnUpdateFade",
			"onupdatetarget", gameObject
			));
		
		if (AudioManager.SharedInstance!=null) AudioManager.SharedInstance.PlayAnimatedSound(BaboonSound);
		
		Enemy.main.Setup();
		//ColorCard.FadeToPicture(1f);
		MainCamera.camera.backgroundColor = Color.white;
		
		//UIManagerOz.SharedInstance.idolMenuVC.Init();	//.ShowIdolMenu();
		UIManagerOz.SharedInstance.idolMenuVC.ReadyForIntroAnimation();
			
		if (GameCamera.SharedInstance.animation != null)
		{
			animateRoot["CameraOpening"].speed = 1f;
			animateRoot.Play("CameraOpening");
		}
		
		while(GameCamera.SharedInstance.animation != null && animateRoot.isPlaying)
		{
			if ((Input.touchCount>0 && Input.touches[0].phase==TouchPhase.Began) 
				|| Input.GetMouseButtonDown(0) 
				|| mMenuButtonToPressAfterGameIntro != null )
			{
				if ( mMenuButtonToPressAfterGameIntro == null )
				{
					animateRoot["CameraOpening"].speed = 6f; // High-speed intro
				}
				else
				{
					animateRoot["CameraOpening"].speed = 12f; // Ultra high-speed intro (if we're launching from a push notification)
				}
			}
			yield return null;
		}
		
		GameCamera.SharedInstance.enabled = true;
		GameCamera.SharedInstance.cameraState = CameraState.gamplay;
		if (cinematicStoryboard) cinematicStoryboard.active = false;
		
		//((UIManagerOz)UIManagerOz.SharedInstance).ShowIdolMenu();	
		UIManagerOz.SharedInstance.idolMenuVC.appear();
		UIManagerOz.SharedInstance.idolMenuVC.BringInIdolMenu();
		
		if ( mMenuButtonToPressAfterGameIntro != null )
		{
			// Open the menu and press the specified button
			UIManagerOz.SharedInstance.idolMenuVC.MenuButtonToPressAfterGameIntro = String.Copy( mMenuButtonToPressAfterGameIntro );
			UIManagerOz.SharedInstance.idolMenuVC.DidLaunchViaPushNotification = true;
				
			mMenuButtonToPressAfterGameIntro = null; // Clear this for next time
			UIManagerOz.SharedInstance.idolMenuVC.OnMenuClicked();
		}
	}
	
	public void OnUpdateFade(float val){
		Shader.SetGlobalFloat("_AMultiplier", val);
		fadeValue = val;
	}
	
	public void SetMainMenuAnimation()
	{		
		TextureReport ("GameController.SetMainMenuAnimation");
		AudioReport ("GameController.SetMainMenuAnimation");
		MeshReport ("GameController.SetMainMenuAnimation");
		
		//GameInterface.HideAll();
		//MainCamera.SetMainMenuAniatmion();
		//Audio.StartMainMenuMusic();
		
		//-- Put Camera in flyin mode.
		//-- Play animation.
		if(AudioManager.SharedInstance!=null)
			//AudioManager.SharedInstance.StartMainMenuMusic();
			//GameCamera.SharedInstance.animation.clip = GameCamera.SharedInstance.animation["CameraOpening"].clip;
			StartCoroutine(DoOpeningScene());
		/*
		if( didStartCinematicPullback && !isPlayingCinematicPullback){
			AudioManager.SharedInstance.StartMainMenuMusic();
			GameCamera.SharedInstance.animation.clip = GameCamera.SharedInstance.animation["CameraOpening"].clip;
			StartCoroutine(DoOpeningScene());
			//GameCamera.SharedInstance.StartMenuAniamtion(false); // this used to be commented out for cinematic pullback
		}
		else{
			AudioManager.SharedInstance.PlayCinematicPullbackSound();
			didStartCinematicPullback = true;
			isPlayingCinematicPullback = true;
			transform.LookAt(OzGameCamera.SharedInstance.FocusTarget.position);

			GameCamera.SharedInstance.animation.clip = GameCamera.SharedInstance.animation["CameraPullBack"].clip;
			GameCamera.SharedInstance.animation["CameraPullBack"].speed = 0.5f;
			//GameCamera.SharedInstance.animation.Play();
			GameCamera.SharedInstance.animation.Play("CameraPullBack");
		}
		*/
	}
	
	public void LosePower() 
	{
		Player.ResetCoinCountForBonus();
		UIManagerOz.SharedInstance.inGameVC.coinMeter.FadePowerGlow();	
		activePower = null;
	}
	
	public void CreateOrShowPower() {
		if(activePower != null && activePower.Active == true)
			return;

		if(AudioManager.SharedInstance!=null)
			AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_UI_CoinMeterFull);
		
		CharacterStats activeCharacter = GameProfile.SharedInstance.GetActiveCharacter();
		if(PowerStore.Powers == null || activeCharacter.powerID == -1)
		{
			Player.ResetCoinCountForBonus();
			return;
		}
		
		activePower = PowerStore.PowerFromID(activeCharacter.powerID);
		if(activePower == null)
		{
			Player.ResetCoinCountForBonus();
			return;
		}
		//-- Double check that we aren't active until the user wants us to be active from a double tap.
		activePower.Active = false;
		activePower.PowerID = activeCharacter.powerID;
		//UIManagerOz.SharedInstance.ShowPowerIconAndGlow(true, activePower.IconName);
		UIManagerOz.SharedInstance.inGameVC.coinMeter.ActivePowerIcon();	//UIManagerOz.SharedInstance.ActivePowerIcon();
		UIManagerOz.SharedInstance.SetPowerProgress(1.0f);
	}
	
	private BasePower activePower = null;
	public void UsePower()
	{
		
		if(activePower == null || Player.Dying || Player.IsFalling)
			return;
		
		if((GamePlayer.SharedInstance.IsOnBalloon
			|| GamePlayer.SharedInstance.IsAnimPlaying(GamePlayer.AnimType.kBalloonExit)
			|| GamePlayer.SharedInstance.IsAnimPlaying(GamePlayer.AnimType.kBalloonExitFail))
			&& (activePower.PowerID == 2 // Poof
			|| activePower.PowerID == 3  // Boost
			|| activePower.PowerID == 5)) // Wand
		{
			//Don't use Boost, Poof, or Magic Wand in the balloon
			return;
		}
		
		if(activePower!=null && activePower.PowerID==3 && Player.HasBoost)
		{
			return;
		}
	
		else
		{
			//-- Can we use the power?
			if(Player.CoinCountForBonus < Player.CoinCountForBonusThreshold || Player.Dying)
				return;
			if(Player.HasSuperBoost || Player.HasBoost || Player.OnTrackPiece.IsTransitionTunnel())
			{
				if(activePower.PowerID!=0 && activePower.PowerID!=6)	//Score Bonus or Coin Bonus
					return;
			}
			notify.Debug("UsePower tutorialId " + TutorialID);
			tutorialPowerUsed = true;
			if(IsTutorialMode && TutorialID == 6){
				canShowNextTutorialStep = true;
				if(tutorialObjectShowing!=null)
				{
					tutorialObjectShowing.alpha = 0f;
					tutorialObjectShowing = null;
				}
				ShowNextTutorial();
			}
			if(activePower != null && activePower.Active == false) {
				activePower.activate();
			}
		}
	//	float duration = (float) PowerStore.Powers[GameProfile.SharedInstance.GetActiveCharacter().powerID].Duration; 
		//notify.Debug("powerId " + GameProfile.SharedInstance.GetActiveCharacter().powerID + " name " + PowerStore.Powers[GameProfile.SharedInstance.GetActiveCharacter().powerID].IconName);
	//	if(duration < 0.1f) duration = 0.4f;
		// eyal hack until I have time to look into what's wrong with the powerEditor
		// for some reason in tutorial we are getting duration = 1000 which we will never have a powerup that lasts 1000 seconds so...
	//	if(duration > 500){
	//		duration = 0.4f;
	//	}
		//notify.Debug("duration " + duration);
		
		//Moved this to Power classes
		//UIManagerOz.SharedInstance.inGameVC.coinMeter.AnimateCoinMeter(duration);
	}
	
	public bool IsPowerActive() {
		if(activePower == null)
			return false;
		
		return activePower.Active;
	}
	
	
	public void ResetLevelInformation()
	{
		//TR.LOG("LOAD LEVEL INFORMATION");
		DistanceRemainder = 0;
		DistanceTraveled = 0.0f;
		realDist = 0f;
		DistanceTraveledSinceLastTurnSection = 0.0f;
		DistanceTraveledPreviousFrame = 0.0f;
		DifficultyDistanceTraveled = 0f;
		StumblesThisRun = 0;
		ResurrectsThisRun = 0;
		HeadStartsThisRun = 0;
		ResetBalloonDifficulty();
		EndGameDelayTime = 0.0f;
		//debugUsed = false;
		
//		if(UIManager.SharedInstance != null && UIManager.SharedInstance.inGameVC != null) {
//			UIManager.SharedInstance.inGameVC.MessageBoardLastDistance = 0;
//		}
		
		collectedBonusItemPerRun = new List<int>();
		Array values = System.Enum.GetValues(typeof(BonusItem.BonusItemType));
		int count = values.Length;
		for(int i =0; i<count; i++) {
			collectedBonusItemPerRun.Add (0);
		}
		
		ResetEnemies();
		
		EnemyFollowDistance = 7.5f;	//huh???

		IsResurrecting = false;
		TimeSinceResurrectStart = 0.0f;

		IsIntroScene = true;
		ResurrectionCost = 1;
		TrackPiece.gemReductionValue = 0;
		
		
		//Reset Passed Neighbors
		ProfileManager.SharedInstance.ResetNeighborPassed();
		
		TrackBuilder.SharedInstance.IsFastTurnSection = false;
		
		// Set up the player
		if (Player != null) 
		{
			Player.Reset();
			Player.doSetVisibility(true);
			
			// Set the active player id
			PlayerStats playerStats = GameProfile.SharedInstance.Player;
			Player.ActiveCharacterId = playerStats.activePlayerCharacter;
			
			float minSpeed = GameProfile.SharedInstance.GetMinSpeed();
			Player.SetDirection(Vector3.forward);
			Player.SetPlayerVelocity(minSpeed);
			float maxSpeed = GameProfile.SharedInstance.GetMaxSpeed();
			Player.SetMaxRunVelocity(maxSpeed);
			
			// Set the coin threshold for the bonus
			Player.CoinCountForBonusThreshold = (int)GameProfile.SharedInstance.GetCoinMeterFillCount();
		}
		
		if (EnvironmentSetSwitcher.SharedInstance != null)
		{
			EnvironmentSetSwitcher.SharedInstance.Reset();
		}
		doHandlePacing();
		
	}
	
	public TrackPiece CreateTrackRoot(TrackPiece.PieceType trackType) {
		GameObject go = TrackPiece.Instantiate(trackType);
		if(go != null) {
			go.transform.position = new Vector3 (0, 0, 0);
			TrackPiece element = go.GetComponent<TrackPiece>();
			element.ReceiveShadow(true);
			element.CachePosition(); 
			return element;	
		}
		return null;
	}

	public void CreateInitialTrack()
	{
		notify.Debug("CreateInitialTrack TutorialID " + TutorialID + " TutorialSegmentID " + TutorialSegmentID);
		// Create the initial track for exiting the Temple
		if(trackRoot == null)
		{
			// First piece is always straight flat
			Debug.Log ("Track Root Set");
			trackRoot = CreateTrackRoot(TrackPiece.PieceType.kTPStraightFlatIntro);
	
			//GameObject go = TrackPiece.Instantiate(TrackPiece.PieceType.kTPStraightFlatIntro);
			//go.transform.position = new Vector3 (0, 0, 0);
			//TrackPiece element = go.GetComponent<TrackPiece>();
			//trackRoot = element;
			//trackRoot.UpdateTrackPieceRenderers();	//So that the first piece isnt alpha'd
			
			// Fill the queue to always start with the Intro Track Segment (unless we are using a DebugTrackSegment)
			if(TrackBuilder.SharedInstance.QueuedPiecesToAdd.Count == 0 && TrackBuilder.SharedInstance.DebugTrackSegment == null) {
				// this is a hack for the sam raimi build and we are pressed for time
				//TODO the best solution is to create a new track piece kTPIntro and define that in Trackbuilder.cs, that is upcoming in the TR2 merge 
				notify.Debug("track queue is 0 " + IsTutorialMode);
				if(IsTutorialMode == false) {
					if(TrackBuilder.SharedInstance.CurrentEnvironmentSetId == 1)
					{
						TrackSegment.QueueSegmentWithType(TrackSegment.SegmentType.WWIntro, TrackBuilder.SharedInstance.QueuedPiecesToAdd);
					}
					if(TrackBuilder.SharedInstance.CurrentEnvironmentSetId == 2)
					{
						TrackSegment.QueueSegmentWithType(TrackSegment.SegmentType.DFIntro, TrackBuilder.SharedInstance.QueuedPiecesToAdd);
					}
					if(TrackBuilder.SharedInstance.CurrentEnvironmentSetId == 3)
					{
						TrackSegment.QueueSegmentWithType(TrackSegment.SegmentType.YBRIntro, TrackBuilder.SharedInstance.QueuedPiecesToAdd);
					}
					if(TrackBuilder.SharedInstance.CurrentEnvironmentSetId == 4)
					{
						TrackSegment.QueueSegmentWithType(TrackSegment.SegmentType.ECIntro, TrackBuilder.SharedInstance.QueuedPiecesToAdd);
					}
				}
				else {
					ShowNextTutorial();
					// do we need to force CurrentEnvironmentSetId
					//EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet = 1;
				}
			}
			// this is new to fix the busted tutorial...
			// all of the code above is doesn't get executed
			//if(IsTutorialMode){
				//ShowNextTutorial();
			//}
			
			//TR.LOG ("TrackGrowCount={0}", TrackGrowCount);
			//trackRoot.AttachRandomPiece(false, TrackBuilder.SharedInstance.QueuedPiecesToAdd);

			int count = TrackBuilder.SharedInstance.QueuedPiecesToAdd.Count;
			TrackPiece current = trackRoot;
			for(;(current != null) && (count > 0); --count)
			{
				while(current.NextTrackPiece != null)
				{
					current = current.NextTrackPiece;
				}
				current.AttachRandomPiece( false, TrackBuilder.SharedInstance.QueuedPiecesToAdd, 0);	
			}
		
			if(IsTutorialMode) { // eyal edit to make sure this piece is tutorial
				notify.Debug("Set the root track to tutorialPiece");
				trackRoot.IsTutorialPiece = true;
			}
		}
	
	}
	
	public bool canShowNextTutorialStep = true;
	TrackPiece lastTutorialPiece = null;
	
	public void ShowNextTutorial() {
		// the next lines of code can be commented since we are ending the tutorial now when activating the powerMeter, need time to test it.
		if(TutorialID >= 9 && !UIManagerOz.SharedInstance.inGameVC.tutorialLabel.gameObject.active) {
			endTutorial();
			return;
		}
		
		if(canShowNextTutorialStep == false)
			return;
		
		
		//-- progress the tutorial
		TutorialID++;
		//0 = jump
		//1 = turn
		//2 = slide
		//3 = tilt
		//4 = enemy
		//5 = powerups
		
		//notify.Debug ("We switched to TuturialId " + TutorialID);
		
		if(TutorialID == 0) {
			TrackSegment.QueueSegmentWithType(TrackSegment.SegmentType.TutorialJump, TrackBuilder.SharedInstance.QueuedPiecesToAdd);
		}
//		else if(TutorialID == 1) {
//			if(TimeSinceGameStart > 4.0f) {
//				UIManagerOz.SharedInstance.inGameVC.ShowTutorialMessage("Nice Job!");
//			}
//			
//		}
		else if(TutorialID == 2) {
			if(Time.time - TimeOfDeath > 4.0f) {
				UIManagerOz.SharedInstance.inGameVC.ShowTutorialMessage(Localization.SharedInstance.Get("Tut_Success"));	
			}
		}
		else if(TutorialID == 3) {
			if(Time.time - TimeOfDeath > 4.0f) {
				UIManagerOz.SharedInstance.inGameVC.ShowTutorialMessage(Localization.SharedInstance.Get("Tut_Success"));	
			}
		}
		else if(TutorialID == 4) {
			if(Time.time - TimeOfDeath > 4.0f) {
				UIManagerOz.SharedInstance.inGameVC.ShowTutorialMessage(Localization.SharedInstance.Get("Tut_Success"));		
			}
		}
//		else if(TutorialID == 5) {
//			if(TimeSinceGameStart > 4.0f) {
//				UIManagerOz.SharedInstance.inGameVC.ShowTutorialMessage(Localization.SharedInstance.Get("Tut_Success"));		
//			}
//		}
		else if(TutorialID == 6) {
			if(Time.time - TimeOfDeath > 4.0f) {
				UIManagerOz.SharedInstance.inGameVC.ShowTutorialMessage(Localization.SharedInstance.Get("Tut_Success"));	
			}
		}
		else if(TutorialID == 7) {
			notify.Debug("Did player used the power ? " + tutorialPowerUsed);
			if(!tutorialPowerUsed){
				GamePlayer.SharedInstance.Kill(DeathTypes.Unknown);
				GamePlayer.SharedInstance.AddPointsToPowerMeter(200);
				return;
			}
			if(TimeSinceGameStart > 0.2f) {
				UIManagerOz.SharedInstance.inGameVC.ShowTutorialMessage(Localization.SharedInstance.Get("Tut_Success"));		
				TutorialID++; // we force tutorialId to finish the tutorial as soon as the showTuturialMessage is hidden
			}
		}
		tutorialObjectShowing = null;
		canShowNextTutorialStep = false;
	}

	
	public void endTutorial() {
		notify.Debug("endTutorial");
		IsTutorialMode = false;
		PopupNotification.EnableNotifications(true);

		if(UIManagerOz.SharedInstance.settingsVC.tutorialCheckboxOn) 
			UIManagerOz.SharedInstance.settingsVC.tutorialCheckboxOn.isChecked = false;	
		if(UIManagerOz.SharedInstance.settingsVC.tutorialCheckboxOff) 
			UIManagerOz.SharedInstance.settingsVC.tutorialCheckboxOff.isChecked = true;		
		
		tutorialObjectShowing = null;
		canShowNextTutorialStep = true;
		lastTutorialPiece = null;
		
		// Exit tutorial mode
		//GameProfile.SharedInstance.ShowTutorial = false;
		PlayerPrefs.SetInt("ShowTutorial",2);
		PlayerPrefs.Save();
		IsTutorialMode = false;
		DistanceTraveled = 29;	//This was causing issues since we hold the camera before 29M
		//DistanceTraveled = 0f;
		DistanceTraveledSinceLastTurnSection = 0;
		
		
		DifficultyDistanceTraveled = 0f;
		ResetBalloonDifficulty();
		GamePlayer.SharedInstance.ResetScoreAndCoins();
		//MaxDistanceWithoutCoins = 0;
		
		BonusItemProtoData.SharedInstance.AllowCoins = true;
		TrackBuilder.SharedInstance.AllowTurns = true;
		TrackBuilder.SharedInstance.AllowObstacles = true;
		
		if(TimeSinceGameStart > 3.0f) {
			UIManagerOz.SharedInstance.inGameVC.ShowTutorialMessage(Localization.SharedInstance.Get ("Tut_Ready"));	
		}
		
		StartCoroutine(StartBalloonTutorialWithDelay(2.5f));
		
		
	}
	
	public void OnTrackChange(TrackPiece prev, TrackPiece on)
	{
		needSetTrackVisibility = true;
		
	//	TrackPieceTypeDefinition def = TrackBuilder.SharedInstance.GetTypesFromTrackType(on.TrackType);
		
	/*	TrackPieceData prevData = prev.CurrentTrackPieceData;
		
		float reduction = prev.UseAlternatePath ? prevData.DifficultyDistanceReduction_Alternate : prevData.DifficultyDistanceReduction;
			
		if(reduction>0f)
			DifficultyDistanceTraveled = Mathf.Max(0f, DifficultyDistanceTraveled-reduction);*/
		UIManagerOz.SharedInstance.inGameVC.pauseMenu.ShowHomeButton = !on.IsTransitionTunnel() && on.TrackType!=TrackPiece.PieceType.kTPBalloonExit;
		
		if(prev!=null)
		{
			if(prev.IsObstacle())
				ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.PassAnObstacle,1);
			if(prev.IsJumpOver())
				ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.JumpOverPassed,1);
			if(prev.IsSlideUnder())
				ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.SlideUnderPassed,1);
			if(prev.IsGap())
				ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.PassGap,1);
			if(prev.TrackType==TrackPiece.PieceType.kTPEnemyMonkey
				|| prev.TrackType==TrackPiece.PieceType.kTPFieldsEnemy
				|| prev.TrackType==TrackPiece.PieceType.kTPFarmsAnimated
				|| prev.IsAttackingBaboon()
				|| prev.TrackType==TrackPiece.PieceType.kTPFieldsMonkey)
				ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.PassTheBaboon,1);
			if((prev.TrackType==TrackPiece.PieceType.kTPEnemySnapDragon
					|| prev.TrackType==TrackPiece.PieceType.kTPForestEnemySnapDragon
					|| prev.TrackType==TrackPiece.PieceType.kTPNarrowsEnemySnapDragon) && Player.IsStumbling==false)
				ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.PassTheSnapDragons,1);
			if(prev.TrackType==TrackPiece.PieceType.kTPCemetarySlight && Player.IsStumbling==false)
				ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.PassTheTombstones,1);
			if(prev.TrackType==TrackPiece.PieceType.kTPFieldsEnemyOver 
				|| prev.TrackType==TrackPiece.PieceType.kTPFieldsEnemyUnder
				|| prev.TrackType==TrackPiece.PieceType.kTPFieldsJumpOverSlightRight 
				|| prev.TrackType==TrackPiece.PieceType.kTPFarmsStumblesEnemy
				|| prev.TrackType==TrackPiece.PieceType.kTPGrovesUnderLeft)
				ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.PassTheWinkies,1);
//			if(prev.TrackType==TrackPiece.PieceType.kTPFarmsAnimated)
//				ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.PassTheWitch,1);
		}
	}
	
	public bool Is_256MB_iOS_Device()
	{
		DeviceGeneration gen = GameController.SharedInstance.GetDeviceGeneration();
		return (gen == DeviceGeneration.iPhone3GS || gen == DeviceGeneration.iPodTouch4);
	}
	
	public void doGameStart()
	{
		Time.timeScale = 1f;
		TutorialID = -1;
		
		cachedAccel = -1f;
		
		// turn on popup notification
		//PopupNotification
		PopupNotification.EnableNotifications(true);
		
		AudioManager.SharedInstance.UpdateIpodMusicIsOn();
		
		//abilityTutorialPlayed = false;
		//utilityTutorialPlayed = false;
		abilityTutorialOn = false;
		utilityTutorialOn = false;
		notify.Debug("abilityTutorialOn " + abilityTutorialOn + " utilityTutorialOn " + utilityTutorialOn);
		notify.Debug("abilityTutorialPlayed " + abilityTutorialPlayed + " utilityTutorialPlayed " + utilityTutorialPlayed);
		
		
		notify.Debug("doGameStart " + Time.timeScale);
		notify.Debug("doGameStart IsTutorialMode " + PlayerPrefs.GetInt("ShowTutorial",Settings.GetInt("default-tutorial-status",0)) );
		notify.Debug("ShowTutorialEnv " + PlayerPrefs.GetInt("ShowTutorialEnv") );
		notify.Debug("ShowTutorialBalloon " + PlayerPrefs.GetInt("ShowTutorialBalloon") );
		//FlurryBinding.logEvent("Run", true);
		//		AnalyticsInterface.SetCanUseNetwork( false ); // Pause network activity during the run
		//AnalyticsInterface.LogGameAction( "run", "start", "", "", 0 );
		//AnalyticsInterface.StartTimer( "run_duration" );
		
		if(fadeValue < 4f){
			iTween.ValueTo(gameObject, iTween.Hash(
				"time", 1f,
				"from", 1f,
				"to", 4f,
				"onupdate", "OnUpdateFade",
				"onupdatetarget", gameObject
				));
			
		}
		
		
		if (IsGameStarted == true)
		{
			notify.Warning("GAME CONTROLLER: doGameStart end bc IsGameStarted=true");
			return;
		}
		GameCamera.SharedInstance.enabled = true;
		GameCamera.SharedInstance.camera.enabled = true;
		GameCamera.SharedInstance.cameraState = CameraState.gamplay;
		
		AudioManager.SharedInstance.StopCharacterSound();
		
		ResetLevelInformation();
		IsHandlingEndGame = false;
		
		ObjectivesDataUpdater.Clear();
		
		// Remove the idol from the main menu
		//Player.TurnOffIdol();
		
		gameState = GameState.IN_RUN;
		UIManagerOz.SharedInstance.MainGameCamera.enabled = true;
		
		IsGameStarted = true;
		TimeSinceGameStart = 0;
		canShowNextTutorialStep = true;
		
		GamePlayer.SharedInstance.CharacterModel.transform.parent = GamePlayer.SharedInstance.transform;
		GamePlayer.SharedInstance.CharacterModel.transform.localPosition = Vector3.zero;
		GamePlayer.SharedInstance.CharacterModel.transform.localRotation = Quaternion.identity;
		
		Player.TintPlayer(Color.black,0f);
		Player.TintPlayer(Color.white,2f);
		
		//Remove all gems from modifiers, since they are only per run now
		GameProfile.SharedInstance.Player.UnGemAllArtifacts();
		
		// Clear the "objectives earned during the run" list
		GameProfile.SharedInstance.Player.ClearObjectivesEarnedDuringRun();
		
		// tutorial setup
		tutorialEnvOn = false;
		tutorialBalloonOn = false;
		didShowBalloonTilt = false;
		IsTutorialMode = true; // by default show tutorial in case it's the first time we load the game
		
		int IsTutorialModeInt = PlayerPrefs.GetInt("ShowTutorial", Settings.GetInt("default-tutorial-status",0));// GameProfile.SharedInstance.ShowTutorial;
		if(IsTutorialModeInt == 2) IsTutorialMode = false;
		if(!UIManagerOz.SharedInstance.settingsVC.tutorialCheckboxOff.isChecked ){
			notify.Debug("TutorialCheckboxOn is set to on so force tutorial");
			IsTutorialMode = true;
			forceTutorialEnvOn = true;
			forceTutorialBalloon = true;
		}
		if(TrackBuilder.SharedInstance.DebugTrackSegment != null) {
			IsTutorialMode=false;
		}
		
		if(TrackBuilder.SharedInstance.CurrentEnvironmentSetId  != 1){
			IsTutorialMode = false;
			forceTutorialEnvOn = false;
			tutorialBalloonOn = false;
		}
		
		if(IsTutorialMode){
			// make sure to reset timescale to 1 in case you die during tutorial slow down
			Time.timeScale = 1f;
			tutorialPowerUsed = false;
			finleyPowerUsed = false;
			monkeyTutorialFailed = false;
			PopupNotification.EnableNotifications(false);
		}
		//else
		//{  // we should always do that
			GamePlayer.SharedInstance.ClearOldPieces();
			GameController.SharedInstance.DeSpawnTrack();
			TrackBuilder.SharedInstance.QueuedPiecesToAdd.Clear();	
		//}
		
		notify.Debug("tutorialBalloonOn " + tutorialBalloonOn);
		
		TutorialSegmentID = 0;
		
		CreateInitialTrack();
		
		GameProfile.SharedInstance.AdditionalScoreMultiplier = 0;

		StatTracker.Reset();
		StatTracker.Begin();
		
		BonusItem.ShowAllPowerUps(false);

		if (Settings.GetBool("invulnerable", false))
		{
			GamePlayer.SharedInstance.StartBoost(1000000000f);
		}
		
//		if (Player.HasFastTravel)
//		{
//			Player.StartFastTravel(Player.FastTravelDestinationEnvironmentSetId);//, Player.FastTravelDistance);	
//		}
		
		if(AudioManager.SharedInstance!=null)
		{
			if(AudioManager.SharedInstance.IsGameMusicPlaying())
				AudioManager.SharedInstance.FadeMusicMultiplier(0.5f,1f);
			else
				AudioManager.SharedInstance.SwitchToGameMusic(0f);
		}
		
		//Resources.UnloadUnusedAssets();		
		System.GC.Collect();
		
		//Select opening piece
		SpawnOpeningTile ();

		if(NeededSkyBox != TrackBuilder.SharedInstance.CurrentEnvironmentSetId){
			NeededSkyBox = TrackBuilder.SharedInstance.CurrentEnvironmentSetId;
			//Pick opening skybox
			SpawnOZSkyBox ();
		}/*
		else{
			openingTile.transform.parent = StartingSetPiece.transform;	
			openingTile.transform.localPosition = Vector3.zero;
		}
		*/
		
		SetUpUpgradeData();
		if(!IsTutorialMode)
		{
			UIManagerOz.SharedInstance.inGameVC.bonusButtons.ShowAllButtons();
			UIManagerOz.SharedInstance.inGameVC.bonusButtons.GetComponent<UIPanelAlpha>().alpha = 1f;
			bool isTut = false;
			if(!abilityTutorialPlayed && UIManagerOz.SharedInstance.inGameVC.bonusButtons.CanShowModifiers() ){
				UIManagerOz.SharedInstance.inGameVC.bonusButtons.EnableAllButtons(false);
				isTut = true;
			}
			if(!utilityTutorialPlayed && UIManagerOz.SharedInstance.inGameVC.bonusButtons.CanShowConsumables()){
				UIManagerOz.SharedInstance.inGameVC.bonusButtons.EnableAllButtons(false);
				isTut = true;
			}
	
			if(isTut){
				UIManagerOz.SharedInstance.inGameVC.bonusButtons.GetComponent<UIPanelAlpha>().alpha = 0f;
			}
			else{
				UIManagerOz.SharedInstance.inGameVC.ShowFastTravel();
				//UIManagerOz.SharedInstance.inGameVC.bonusButtons.ShowAllButtons();
			}
		}
		else
		{
			UIManagerOz.SharedInstance.inGameVC.bonusButtons.HideConsumableAndModifierButtonsNow();
			UIManagerOz.SharedInstance.inGameVC.HideFastTravel();
		}
		
		UIManagerOz.SharedInstance.inGameVC.bonusButtons.DeactivatePickupButtons();
		
		// hide envHud in case it's visible from previous run
		//UIManagerOz.SharedInstance.inGameVC.envChangeHud.alpha = 0f;
		UIManagerOz.SharedInstance.inGameVC.envChangeHud.gameObject.SetActiveRecursively(false);
		//UIManagerOz.SharedInstance.inGameVC.tutorialBalloonLabel.alpha = 0f;
		UIManagerOz.SharedInstance.inGameVC.tutorialBalloon.gameObject.SetActiveRecursively(false);
		// get rid of coins from previous run
		UIManagerOz.SharedInstance.inGameVC.fx_coin.Clear();
/*
		String textureReport = "doGameStart texture report:\n";
		
		UnityEngine.Object[] textures = Resources.FindObjectsOfTypeAll(typeof(Texture2D));
		String line = "Found " + textures.Length + " textures\nName,Size,Mip maps,Format\n";
		textureReport = textureReport + line;
		foreach (UnityEngine.Object textureObj in textures)
		{
			Texture2D texture = textureObj as Texture2D;
			line = texture.name + "," + texture.width + " X " + texture.height + "," + texture.mipmapCount + "," + texture.format + "\n";
			textureReport = textureReport + line;
		}
		
		notify.Debug(textureReport);
		//Resources.UnloadUnusedAssets();
*/
		PopupNotification.PopupList[PopupNotificationType.Generic].ResetDistance();
		
		// reset environments visited and log current locaiton
		GamePlayer.SharedInstance.EnvironmentsVisitedThisRun.Clear();
		ObjectivesDataUpdater.LogEnvironmentVisited(EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.SetId);
		//Debug.LogWarning("SetId = " + EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.SetId);
	}
	
	private void DeSpawnTrack() 
	{
		if(trackRoot != null)
		{			
			SpawnPool pool = PoolManager.Pools["TrackPiece"];
			if(pool)
			{
				pool.DespawnAll();
				trackRoot = null;
			}
		}
		TrackBuilder.SharedInstance.ActiveJunctions = 0;
	}
	
	public void SetUpUpgradeData()
	{
		BonusItemProtoData.SharedInstance.DistanceToChangeDoubleCoins = GameProfile.SharedInstance.GetDoubleCoinsDistance();
		BonusItemProtoData.SharedInstance.DistanceToChangeTripleCoins = GameProfile.SharedInstance.GetTripleCoinsDistance();
		/*
		//Call all consumables
		PlayerStats stats = GameProfile.SharedInstance.Player;	
		for(int i=0;i<stats.consumablesPurchasedQuantity.Count;i++)
		{
			if(stats.PopConsumable(i))
			{
				ConsumableStore.consumablesList[i].Activate();
			}
		}*/
	}
	
	
	private void RestartGame()
	{
		//TR.LOG("GAME CONTROLLER: RestartGame begin");
		
		TimeSinceGameStart = 0;
		
		cachedAccel = -1f;
		
		GameProfile.SharedInstance.Player.UnGemAllArtifacts();
		
		UIManagerOz.SharedInstance.inGameVC.bonusButtons.ResetAbilityUsedFlags();
		
		//		AnalyticsInterface.ResetBalloonSequenceStats();
		//∂∂	AnalyticsInterface.ResetConsumables();
		
		
		//if(trackRoot != null)
		//{
			SpawnPool pool = PoolManager.Pools["TrackPiece"];
			if(pool)
			{
				pool.DespawnAll();
				trackRoot = null;
			}
		//}
		
		//GamePlayer.SharedInstance.Reset();
		
		ObjectivesDataUpdater.Clear();
		
		OzGameCamera.OzSharedInstance.reset();
		
		IsGameStarted = false;
		IsGameOver = false;
		AutoRestart = false;
		IsInCountdown = false;
		IsPaused = false;
		//TR.LOG("GAME CONTROLLER: RestartGame end");
		
		//Spawn opening tile
		
	}
	
	public void HandleResurrecting()
	{
//		TimeSinceResurrectStart += Time.deltaTime;
//
//		// Wait a second before resurrection
//		if (TimeSinceResurrectStart >= 1.0f) 
//		{
//			IsResurrecting = false;
//			doUnPause();	
//		}
	}
	
	public void doSetTrackPieceVisbility()
	{
		if(Player == null)
			return;
		if(Player.OnTrackPiece == null)
			return;
			
		Player.OnTrackPiece.RecursiveVisibility(true, true, 0, false, 0);
		
		
	}
	
#if UNITY_EDITOR
	[System.NonSerialized]
	public int Frame;
#endif
	
	//private float LastTime = 0f;
	public void LateUpdate()
	{
/*		if((EnvironmentSetSwitcher.SharedInstance.TransitionState >= EnvironmentSetSwitcher.SwitchState.waitingToBeAbleToDeletePools) &&
			(EnvironmentSetSwitcher.SharedInstance.TransitionState < EnvironmentSetSwitcher.SwitchState.finished) )
		{
			float curTime = Time.realtimeSinceStartup;
			float dt = curTime - LastTime;
			LastTime = curTime;
			if(dt > 0.3)
			{
				Debug.Log("===================Switch state " + EnvironmentSetSwitcher.SharedInstance.TransitionState.ToString() + " dt:" + dt + " @ " + Time.frameCount + " : " + Time.realtimeSinceStartup);
			}
			else
				Debug.Log("Switch state " + EnvironmentSetSwitcher.SharedInstance.TransitionState.ToString() + " dt:" + dt + " @ " + Time.frameCount + " : " + Time.realtimeSinceStartup);
		}*/
		if(needSetTrackVisibility == true)
		{
			doSetTrackPieceVisbility();
			needSetTrackVisibility = false;
		}
		
		if(!IsPaused && IsGameStarted)
			SimulateEnemies();
	}
	
	public void Update()
	{
#if UNITY_EDITOR
		Frame = Time.frameCount;
#endif
		
		foreach (KeyValuePair<string,SpawnPool> pool in PoolManager.Pools)
		{
			if(pool.Key == "BonusItems")
			{
				pool.Value.ActivateDeferred(GamePlayer.SharedInstance.CachedTransform.position, GamePlayer.SharedInstance.BonusItemActivateDistance*GamePlayer.SharedInstance.BonusItemActivateDistance);
			}
			else
			{
				pool.Value.ActivateDeferred();
			}
		}
		
		if (IsInCountdown == true) 
		{
			doHandleCountDown();
			needSetTrackVisibility = true;
		}
		else if (IsPaused == false) 
		{
			
			if (IsGameStarted == true) 
			{
				TimeSinceGameStart += Time.deltaTime;
				TimeSinceLastPause += Time.deltaTime;
				TimeSinceLastCoin += Time.deltaTime;
			}
			
			HandleControls();
			
			//-- Disable intro scene			
			//if (TimeSinceGameStart > 2.0f && IsIntroScene == true) 
			if(IsIntroScene && !OzGameCamera.OzSharedInstance.Holding && TimeSinceGameStart>.1f) //Added TimeSinceGameStart because there is a delay between game start and camera being locked down
			{
				//doEnableIntroScene(false);
				IsIntroScene = false;
			}	
			if (TimeSinceGameStart > 5.0f && startingSetPieceOn && !Player.IsDead) 
			{
				DestroyStartingPiece();
			}

			if (IsGameStarted == true) 
			{
				Player.Hold = false;
				
				if (InputController.JustTurned == true) 
				{
					InputController.TimeSinceLastTurn += Time.deltaTime;
					if (InputController.TimeSinceLastTurn > 0.25f)
						InputController.JustTurned = false;
				}

				
				doHandleDelayedJumping();
				doHandleDelayedSliding();
				doHandleDelayedTurning();

				if (Player.IsSliding == false)
				{
					//MainCamera.NeedsToDuckCamera = false;
				}

				if (Player.IsDead == false) 
				{
					if (Player.IsOnGround == true && Player.IsSliding == false && Player.HasBoost == false) 
					{
						// TODO footsetp sounds logic
					}

					doComputeDistanceTraveled();
					doHandlePacing();
					
				}
				//Moved this to LateUpdate, because the monkeys would shake after dying once. Still don't know why.
			//	SimulateEnemies();
			}

			// Check to see if we are dead
			if (Player.IsDead == true && IsGameOver == false) 
			{
				if(EndGameDelayTime > 0.0f) {
					EndGameDelayTime-=Time.deltaTime;
					if(EndGameDelayTime < 0.0f) {
						IsGameOver = true;
					}
				}
				//CHANGE THE 0.1's TO OTHER NUMBERS TO PUT BACK IN THE RESURRECT DELAY!!!!
				else if (IsTutorialMode == true) 
				{
					notify.Debug("DoEndGame TutorialID " + TutorialID);
					if(TutorialID ==1){ // finley tutorial
						doEndGame(0.1f, true);
					}
					else if(TutorialID ==7){ // powerMeter tutorial
						TutorialID--;
						doEndGame(0.1f, true);
					}
					else{
						doEndGame(1.1f, true);
					}
				}
				else
				{
					doEndGame(0.1f, false);
				}
			}
			
			if (IsGameOver == true && IsHandlingEndGame == false) 
			{
				doHandleEndGame();
			}
			
			if(activePower != null && activePower.Active == true) {
				if(activePower.update() == false) {
					LosePower();
				}
			}
			
			// Since we use a scene graph, this prunes the visibility of things in the same
			// way the original code did the selective batching for rendering.
			//NOTE: Moving this to a place that is not called every frame.
			//doSetTrackPieceVisbility();
			
		}
	}
	
	
	public void UpdateChallengeAndEndGame(){

		//Moved Challenge Updater and user profile calls to UIInGameViewController OnDiePostGame
	//	ChallengeDataUpdater challengeUpdater = Services.Get<ChallengeDataUpdater>();
	//	challengeUpdater.SetChallengeData();
				
		//ProfileManager userProfile = Services.Get<ProfileManager>();
		//userProfile.UpdateProfile();

		
		if(UIManagerOz.SharedInstance != null) {
			UIManagerOz.SharedInstance.EndGame();	
		}
	}
	
	public void doHandleEndGame()
	{
		gameState = GameState.POST_RUN;

		IsHandlingEndGame = true;
		
		//Restart the Analytics
		
		//		AnalyticsInterface.SetCanUseNetwork(true);
		
		//	AnalyticsInterface.LogTimingEvent( "run_duration" );
		
		
		if (IsTutorialMode == false)		{
			GameProfile.SharedInstance.deathsPerSession += 1;
			UpdateChallengeAndEndGame();
			// add this to make sure we hide buttons when dead
			
			// wxj, gameObject BonusButtons maybe inactive
			if(UIManagerOz.SharedInstance.inGameVC.bonusButtons.gameObject.active)
			UIManagerOz.SharedInstance.inGameVC.bonusButtons.HideConsumableAndModifierButtons();
			
			
			// --- Analytics --------------------------------------------------
			string causeOfDeathStr = "other";
			DeathTypes deathType = GamePlayer.SharedInstance.DeathType;
		//	Debug.Log ("cause of death = " + deathType );
			switch ( deathType )
			{
				case DeathTypes.Fall:
					causeOfDeathStr = "fall";
					break;
				case DeathTypes.Baboon:
					causeOfDeathStr = "baboon";
					break;
				case DeathTypes.SceneryRock:
					causeOfDeathStr = "wall";
					break;
			}
			//AnalyticsInterface.LogGameAction( "run", "died", causeOfDeathStr, DistanceTraveled.ToString(), 0 );
			//			AnalyticsInterface.LogGameAction( "run", "area", "", GameProfile.GetAreaCharacterString(), 0 );
			/*AnalyticsInterface.LogInAppCurrencyActionEvent(
				CostType.Coin, 
				GamePlayer.SharedInstance.CoinCountTotal,
				"run", 
				EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.SetCode,
				0,
				"",
				GameProfile.GetAreaCharacterString()
			);
			*/
			/*
			AnalyticsInterface.LogInAppCurrencyActionEvent(
				CostType.Special, 
				GamePlayer.SharedInstance.GemCountTotal,
				"run", 
				EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.SetCode,
				0,
				"",
				GameProfile.GetAreaCharacterString()
			);	
				*/
				
			
			// ----------------------------------------------------------------
		}
		else if(AutoRestart == true) {
			notify.Debug("doHandleEndGame autorestart"); 
			if(IsTutorialMode == true) {
				if(TutorialID == 4){ // monkey tutorial
					monkeyTutorialFailed = true;
				}
				TutorialID--;
				TutorialSegmentID = TutorialID;
			}
			GamePlayer.SharedInstance.ClearOldPieces();
			DeSpawnTrack();
			TrackBuilder.SharedInstance.QueuedPiecesToAdd.Clear();
			
			//-- 
			
			
			if(IsTutorialMode == true) {
				trackRoot = CreateTrackRoot(TrackPiece.PieceType.kTPForestStraight);
				
				if(TutorialID == -1) {
					TutorialID=0;
					tutorialObjectShowing = null;
					TrackSegment.QueueSegmentWithType(TrackSegment.SegmentType.TutorialJump, TrackBuilder.SharedInstance.QueuedPiecesToAdd);
					TutorialSegmentID = TutorialID;
				}
				else {
					TrackBuilder.SharedInstance.QueuedPiecesToAdd.Add(TrackPiece.PieceType.kTPForestStraight);
					TrackBuilder.SharedInstance.QueuedPiecesToAdd.Add(TrackPiece.PieceType.kTPForestStraight);	
				}
				// put it into a while per Gavin.
				int count = TrackBuilder.SharedInstance.QueuedPiecesToAdd.Count;
				TrackPiece current = trackRoot;
				for(;(current != null) && (count > 0); --count)
				{
					while(current.NextTrackPiece != null)
					{
						current = current.NextTrackPiece;
					}
					current.AttachRandomPiece( false, TrackBuilder.SharedInstance.QueuedPiecesToAdd, 0);	
				}
				
			}
			
			
			
			
			GamePlayer.SharedInstance.OnTrackPiece = trackRoot;
			
			//-- Find safe spot
			if(GamePlayer.SharedInstance.MovePlayerToNextSafeSpot() == true) {
				//-- Slow down current speed
				//GamePlayer.SharedInstance.SetPlayerVelocity(GamePlayer.SharedInstance.DeathRunVelocity*0.5f);
				if(GameController.SharedInstance.IsTutorialMode){
					GamePlayer.SharedInstance.SetPlayerVelocity(GamePlayer.SharedInstance.playerSpeedInTutorial);
				}
				//-- give invulnerable for a few seconds.
				GamePlayer.SharedInstance.IsDead = false;
				GamePlayer.SharedInstance.doSetVisibility(true);
				GamePlayer.SharedInstance.doResetForce();
				GamePlayer.SharedInstance.Dying = false;
			//	TimeSinceGameStart = 0f;
				//GamePlayer.SharedInstance.doSetupCharacter();
				//GamePlayer.SharedInstance.MakeInvincible(4f);
					
				GameController.SharedInstance.IsHandlingEndGame = false;
				GameController.SharedInstance.IsGameOver = false;
				GameController.SharedInstance.ResetEnemies();
				//UIManager.SharedInstance.inGameVC.show();
				//UIManagerOz.SharedInstance.inGameVC
			//	if(!AudioManager.SharedInstance.IsGameMusicPlaying()){
			//		AudioManager.SharedInstance.SwitchToGameMusic(0f);
			//	}
			//	else{
			//		AudioManager.SharedInstance.FadeMusicMultiplier(0.5f,1f);
			//	}
				
				
//				UIManager.SharedInstance.inGameVC.OnPause();
//				UIManager.SharedInstance.inGameVC.OnUnPaused();
				GamePlayer.SharedInstance.ResetCoinCountForBonus();
				
				
				//UIManagerOz.SharedInstance.inGameVC.ShowTutorialMessage("Try again!", tutorialMessageType.FAIL);		
			}
			
			OzGameCamera.OzSharedInstance.reset();
			
			UIManagerOz.SharedInstance.inGameVC.ShowPauseButton();
			
			AutoRestart = false;
		}
	}
	
	public float EvalForceMag;

	public float velocityMagnitude;
	private float maxRunVelocity;
	private float velocityPercent;
	
	public void HandleControls()
	{
		if(InputController == null || Player == null)
			return;
		
		// Handle Player's lateral movement
		float xOffset = InputController.update(IsIntroScene || GameCamera.SharedInstance.FlyMode);
		
	/*	if(InputController.JoystickMode == true)
		{
			if(InputController.JoystickPosition == GameInput.JoystickPositionType.kRight) {
				xOffset = TiltOffset;
			} else if(InputController.JoystickPosition == GameInput.JoystickPositionType.kLeft) {
				xOffset = -TiltOffset;
			} else {
				xOffset = 0.0f;
			}
		}*/
		
		// This smooths the players lateral movement so he tilts left / right smoothly
		if(GamePlayer.SharedInstance.IsStumbling == false && GamePlayer.SharedInstance.StumbleAfterDelay == false) {
		//	float distanceOff = xOffset - Player.PlayerXOffset;
			Player.PreviousPlayerXOffset = Player.PlayerXOffset;
		//	Player.PlayerXOffset = Player.PlayerXOffset + (distanceOff * 0.1f);	
			Player.PlayerXOffset = Mathf.Lerp(Player.PlayerXOffset,xOffset,Time.deltaTime*5f);
		}
		else {
			Player.PreviousPlayerXOffset = Player.PlayerXOffset;
			Player.PlayerXOffset = xOffset;
		}
		
		if(Mathf.Abs(Player.PreviousPlayerXOffset-Player.PlayerXOffset) > 0.2f)
			notify.Debug("Large change in PlayerXOffset");

		// Acceleration
		velocityMagnitude = Player.getRunVelocity();
		maxRunVelocity = Player.getModfiedMaxRunVelocity();
		velocityPercent = velocityMagnitude / maxRunVelocity;
		float TROneToNow = 2.0f / 20.0f;
		
		//-- If we are above max run velocity and we arent boosting or hanging, slow down 
		if(	velocityMagnitude > maxRunVelocity && 
			((Player.HasBoost == false || Settings.GetBool("invulnerable", false)) && 
			Player.OnTrackPiece != null && Player.OnTrackPiece.IsAZipLine == false))
		{
			if((velocityMagnitude-maxRunVelocity) < 0.1f)
			{
			}
			else
			{
				EvalForceMag = BonusItemProtoData.SharedInstance.BoostAcceleration*(-0.01f);	
				Player.doApplyForcef(EvalForceMag);
			}
		}
		//-- If we are jumping or sliding, don't speed up. <--Changed. Why do we need this?
		//-- Speed up if we aren't above maxRunVelocity OR we are boosting OR are hanging.
		else if (/*Player.IsJumping == false && Player.IsSliding == false &&*/
				(velocityMagnitude < maxRunVelocity || (Player.HasBoost == true) || (Player.IsHangingFromWire == true))
		   ) 
		{
			//-- Use the default curve above
			
			
			if(!Player.IsOnBalloon)
			{
				if (velocityPercent < 0.5f)
					EvalForceMag = GameProfile.SharedInstance.Accel1;
				else if (velocityPercent >= 0.5f && velocityPercent <= 0.65f)
					EvalForceMag = GameProfile.SharedInstance.Accel2;
				else if (velocityPercent >= 0.65f && velocityPercent <= 0.85f)
					EvalForceMag = GameProfile.SharedInstance.Accel3;
				else
					EvalForceMag = GameProfile.SharedInstance.Accel4;
			}
			else
			{
				if (velocityPercent < 0.5f)
					EvalForceMag = GameProfile.SharedInstance.BalloonAccel1;
				else if (velocityPercent >= 0.5f && velocityPercent <= 0.65f)
					EvalForceMag = GameProfile.SharedInstance.BalloonAccel2;
				else if (velocityPercent >= 0.65f && velocityPercent <= 0.85f)
					EvalForceMag = GameProfile.SharedInstance.BalloonAccel3;
				else
					EvalForceMag = GameProfile.SharedInstance.BalloonAccel4;
			}
			
			
			if(cachedAccel<0)
				cachedAccel = GameProfile.SharedInstance.GetAcceleration();
			
			EvalForceMag *= cachedAccel;
			
			// Extra boost velocity
			if (Player.HasBoost) 
			{
				if (Player.BoostDistanceLeft > BonusItemProtoData.SharedInstance.BoostStartSlowDownDistance) 
				{
					// Speed up like crazy during Boost
					EvalForceMag = BonusItemProtoData.SharedInstance.BoostAcceleration;
				}
				else if (velocityMagnitude > Player.VelocityBeforeBoost) 
				{
					// Put on the breaks at the end of the Boost
					EvalForceMag = BonusItemProtoData.SharedInstance.BoostAcceleration*(-0.01f);
				}
			}
			EvalForceMag *= TROneToNow;
			
			if(Player.HasSuperBoost)	
				EvalForceMag *= 10f;
			
			//A quick patch-up that keeps speed from increasing while paused or in a Bonus button tutorial
			if(Time.timeScale<=0.1f)
				EvalForceMag = 0;
			
			if(Player.IsHangingFromWire == true && Player.HasBoost == false && Player.OnTrackPiece != null)
			{
				float t = (float)Player.CurrentSegment / (float)Player.OnTrackPiece.GeneratedPath.Count;
				EvalForceMag = Mathf.Lerp(TrackBuilder.SharedInstance.ZiplinePerSegmentForce, TrackBuilder.SharedInstance.MaxZiplineForce, t);
			}
			
			// Limit veloicty in tutorial mode.  Otherwise full speed ahead
//			if (IsTutorialMode == false || (IsTutorialMode == true && velocityPercent < 0.65f)) 
			{
				Player.doApplyForcef(EvalForceMag);
			}
			
			
		}
		
	}
	private float cachedAccel = -1;
	
	private BalanceState currentBalanceState = null;
	
	void doHandlePacing()
	{
		//if(tutorialEnvOn){ not currently used
		//	doHandlePacingTutorialEnv();
		//}
		
		if(tutorialBalloonOn){
			doHandlePacingTutorialBalloon();
		}
		
		
		if(!abilityTutorialPlayed ){
			doHandlePacingTutorialAbility();
		}
		if(!utilityTutorialPlayed ){
			doHandlePacingTutorialUtility();
		}
		
		if (!IsTutorialMode) 
		{
			
			if(envChangeSignAvailable && !Player.IsOnBalloon && showEnvHud == 1 ){
				TrackPiece tp = GamePlayer.SharedInstance.FindNearestPieceOfTypeWithInDistance(lastTutorialPiece, 46.0f, TrackPiece.PieceType.kTPEnvSetJunction);
				if(tp){
					showEnvHud = 0;
					PlayerPrefs.SetInt("showEnvHud", 0);
					envChangeSignAvailable = false;
					TransitionSignDecider decider = tp.GetComponentInChildren<TransitionSignDecider>();
					if(decider){
						notify.Debug("sign direction is left " + decider.MainLeftGoesToTransitionTunnel);
						// currently we don't use the actual location and direction , just a generic msg
						/*
						string location = EnvironmentSetManager.SharedInstance.LocalDict[decider.DestinationId].GetLocalizedTitle();
						string format = Localization.SharedInstance.Get ("Tut_Turn");
						string dir = Localization.SharedInstance.Get ("Loc_Left");
						string finalText = String.Format(format, dir, location);
						notify.Debug("format " + format);
						notify.Debug("location " + location);
						notify.Debug("dir " + dir);
						notify.Debug("finalText "  + finalText);
						*/
						string finalText = Localization.SharedInstance.Get("Tut_NewLocation");
						if(decider.MainLeftGoesToTransitionTunnel){
							UIManagerOz.SharedInstance.inGameVC.envChangeHudArrow.transform.localEulerAngles = new Vector3(0f,0f,90f);
							UIManagerOz.SharedInstance.inGameVC.ShowEnvChangeHud(finalText);
						}
						else{
							//dir = Localization.SharedInstance.Get ("Loc_Right");
							//finalText = String.Format(format, dir, location);
							UIManagerOz.SharedInstance.inGameVC.envChangeHudArrow.transform.localEulerAngles = new Vector3(0f,0f,-90f);
							UIManagerOz.SharedInstance.inGameVC.ShowEnvChangeHud(finalText);
						}
					}
				}
			}
			
			
			
			//TutorialSprite.enabled = false;
			BonusItemProtoData.SharedInstance.AllowBonusItems = Player.TimeSinceLastPowerup >= BonusItemProtoData.SharedInstance.MaxTimeBetweenBonusItems;

			// Coin pacing
			BonusItemProtoData.SharedInstance.AllowDoubleCoins = (DistanceTraveled >= BonusItemProtoData.SharedInstance.DistanceToChangeDoubleCoins);
			BonusItemProtoData.SharedInstance.AllowTripleCoins = (DistanceTraveled >= BonusItemProtoData.SharedInstance.DistanceToChangeTripleCoins);
			
			
			if(BalanceData.Current.BalanceInfo.Count>1)
			{
			//	int stateIndex = 0;
				
				BalanceState state = null;
				
				//Find which balance state we should use for this distance
				for(int i=1;i<BalanceData.Current.BalanceInfo.Count;i++)
				{
					if(DifficultyDistanceTraveled < BalanceData.Current.BalanceInfo[i].Distance)
					{
						state = BalanceData.Current.BalanceInfo[i-1];
				// 		stateIndex = i-1;
						break;
					}
				}
				if(state==null)
					state = BalanceData.Current.BalanceInfo[BalanceData.Current.BalanceInfo.Count-1];
//				
//				if(balloonSpawned)
//				{
//					TrackBuilder.SharedInstance.MaxTrackPieceDifficulty = GamePlayer.SharedInstance.balloonDifficulty;
//					Debug.Log ("Balloon Difficulty Set");
//				}
//				
				if(state!=currentBalanceState)
				{
					currentBalanceState = state;
					
					//Set all of our values, based on our balance sheet data
					BonusItemProtoData.SharedInstance.AllowCoins = currentBalanceState["AllowCoins"].BoolValue;
					BonusItemProtoData.SharedInstance.MinDistanceBetweenCoinRuns = currentBalanceState["MinDistBetweenCoinRuns"].Value;
					BonusItemProtoData.SharedInstance.MinCoinsPerRun = (int)currentBalanceState["MinCoinsPerRun"].Value;
					BonusItemProtoData.SharedInstance.MaxCoinsPerRun = (int)currentBalanceState["MaxCoinsPerRun"].Value;
					TrackBuilder.SharedInstance.AllowTurns = currentBalanceState["AllowTurns"].BoolValue;
					TrackBuilder.SharedInstance.AllowObstacles = currentBalanceState["AllowObstacles"].BoolValue;
					TrackBuilder.SharedInstance.MinDistanceBetweenTurns = currentBalanceState["MinDistBetweenTurns"].Value;
					TrackBuilder.SharedInstance.MaxDistanceBetweenTurns = currentBalanceState["MaxDistBetweenTurns"].Value;
					
					//Difficulty split between running and balloon
					if(!balloonSpawned)
					{
						TrackBuilder.SharedInstance.MaxTrackPieceDifficulty = (int)currentBalanceState["MaxDifficulty"].Value;
					}
					
					TrackBuilder.SharedInstance.MinDistanceBetweenObstacles = currentBalanceState["MinDistBetweenObstacles"].Value;
					TrackBuilder.SharedInstance.MaxDistanceBetweenObstacles = currentBalanceState["MaxDistBetweenObstacles"].Value;
					TrackBuilder.SharedInstance.AllowTurnAfterObstacle = currentBalanceState["AllowTurnAfterObstacle"].BoolValue;
					TrackBuilder.SharedInstance.MinDistanceAfterTurnForObstacle = currentBalanceState["MinDistAfterTurnForObstacle"].Value;
					TrackBuilder.SharedInstance.DistanceToTurnSection = currentBalanceState["DistToTurnSection"].Value;
					TrackBuilder.SharedInstance.DoubleObstaclePercent = currentBalanceState["DoubleObstaclePercent"].Value;
				}
			
			}

			if (Player.HasFastTravel)
			{
				handleFastTravel();
			}

		}
		else{
			doHandlePacingTutorial();
		}
	}
	
	public void ResetBalloonDifficulty()
	{
		//Difficulty after exiting balloon
	//	if(currentBalanceState != null && (TrackBuilder.SharedInstance.MaxTrackPieceDifficulty != 0))
	//	{
	//		//Debug.Log ("Previous Difficulty (Balloon): " + TrackBuilder.SharedInstance.MaxTrackPieceDifficulty);
	//		TrackBuilder.SharedInstance.MaxTrackPieceDifficulty = (int)currentBalanceState["MaxDifficulty"].Value;
			//Debug.Log ("Current Difficulty (Normal): " + TrackBuilder.SharedInstance.MaxTrackPieceDifficulty);
	//	}
	//	balloonSpawned = false;
		
		//Debug.Log ("Current Difficulty (Normal): " + TrackBuilder.SharedInstance.MaxTrackPieceDifficulty);
	}
	
	public void SetBalloonDifficulty()
	{
		//Difficulty when balloon spawns
		//Debug.Log ("Previous Difficulty (Normal): " + TrackBuilder.SharedInstance.MaxTrackPieceDifficulty);
	//	TrackBuilder.SharedInstance.MaxTrackPieceDifficulty = GamePlayer.SharedInstance.balloonDifficulty;
	//	balloonSpawned = true;
		
		HasSpawnedGemInBalloon = false;
		//Debug.Log ("Current Difficulty (Balloon): " + TrackBuilder.SharedInstance.MaxTrackPieceDifficulty);
		
		//Debug.Log ("Current Difficulty (Balloon): " + TrackBuilder.SharedInstance.MaxTrackPieceDifficulty);
	}
	
	private void doHandlePacingTutorialAbility(){
		if(abilityTutorialOn || IsTutorialMode)
			return;
		if(!UIManagerOz.SharedInstance.inGameVC)
			return;
		if(!UIManagerOz.SharedInstance.inGameVC.bonusButtons.tutorialButtonAbility)
			return;
		if(!UIManagerOz.SharedInstance.inGameVC.bonusButtons.tutorialButtonAbility.active)
			return;
		if(!UIManagerOz.SharedInstance.inGameVC.bonusButtons.CanShowModifiers())
			return;
		if(!OzGameCamera.OzSharedInstance.Holding && DistanceTraveled>=5f){	//Wait until the camera swings around
			abilityTutorialOn = true;
			UIManagerOz.SharedInstance.inGameVC.bonusButtons.EnableAllCountLabels(false);
			UIManagerOz.SharedInstance.inGameVC.ShowAbilityTutorial();
		}
	}
	
	private void doHandlePacingTutorialUtility(){
		//if(utilityTutorialOn  || IsTutorialMode ) // both tutorials together
		if(utilityTutorialOn  || IsTutorialMode || abilityTutorialOn)
			return;
		if(!UIManagerOz.SharedInstance.inGameVC)
			return;
		if(!UIManagerOz.SharedInstance.inGameVC.bonusButtons.tutorialButtonUtility)
			return;
		if(!UIManagerOz.SharedInstance.inGameVC.bonusButtons.tutorialButtonUtility.active)
			return;
		if(!UIManagerOz.SharedInstance.inGameVC.bonusButtons.CanShowConsumables())
			return;
		if(!OzGameCamera.OzSharedInstance.Holding && DistanceTraveled>=5f){	//Wait until the camera swings around
			utilityTutorialOn = true;
			UIManagerOz.SharedInstance.inGameVC.bonusButtons.EnableAllCountLabels(false);
			UIManagerOz.SharedInstance.inGameVC.ShowAbilityTutorial(true);
		}
	}
	
	private void doHandlePacingTutorial(){
		if(IsIntroScene)  return;
		
		if(GamePlayer.SharedInstance.OnTrackPiece==null)	return;
			 
		/*
		BonusItemProtoData.SharedInstance.AllowCoins = false;
		BonusItemProtoData.SharedInstance.MaxCoinsPerRun = 25;
		BonusItemProtoData.SharedInstance.MinDistanceBetweenCoinRuns = 80.0f;
		
		BonusItemProtoData.SharedInstance.AllowDoubleCoins = false;
		BonusItemProtoData.SharedInstance.AllowTripleCoins = false;
		TrackBuilder.SharedInstance.AllowTurns = false;
		TrackBuilder.SharedInstance.AllowObstacles = false;
		TrackBuilder.SharedInstance.AllowTurnAfterObstacle = false;
		BonusItemProtoData.SharedInstance.AllowBonusItems = false;
		
		TrackBuilder.SharedInstance.MinDistanceBetweenTurns = 25.0f;
		TrackBuilder.SharedInstance.MaxDistanceBetweenTurns = 50.0f;
		TrackBuilder.SharedInstance.MaxTrackPieceDifficulty = 0;
		TrackBuilder.SharedInstance.MinDistanceBetweenObstacles = 10.0f;
		TrackBuilder.SharedInstance.MaxDistanceBetweenObstacles = 20.0f;
		TrackBuilder.SharedInstance.DistanceToTurnSection = 1000;
	*/

		if(GamePlayer.SharedInstance.OnTrackPiece.IsTutorialPiece == false) {
			//notify.Debug ("IsTutorialPiece is false so show next tutorial "  + TutorialID + " canShowNextTutorialStep " + canShowNextTutorialStep);

			// if (tutorialId != 7) - when tutorialId is 7, we use a special case and display the ShowNextTutorial when we press the power button (dobule tap)
			if(TutorialID == 7){ // powermeter tutorial
				//TutorialID++;
			}
			else{  // not powerMeter
				ShowNextTutorial();
			}
		}
		else if(UIManagerOz.SharedInstance.inGameVC==null || UIManagerOz.SharedInstance.inGameVC.tutorialLabel.gameObject.active){
			return; // wait till we complete the "Nice Job"
		}
		else if(tutorialObjectShowing == null){
			TimeSinceTutorialEndedDelay = 0f;
			switch(TutorialID) {
				case 0:
					TimeSinceTutorialEndedDelay = 2f;
					tutorialObjectShowing = UIManagerOz.SharedInstance.inGameVC.ShowJumpInstruction();
					break;
				case 1:
					tutorialObjectShowing = UIManagerOz.SharedInstance.inGameVC.ShowFinleyInstruction();
					break;
				case 2:
					tutorialObjectShowing = UIManagerOz.SharedInstance.inGameVC.ShowTurnInstruction();
					break;
				case 3:
					tutorialObjectShowing = UIManagerOz.SharedInstance.inGameVC.ShowSlideInstruction();
					break;
				case 4:
					monkeyTrackDisntace = 681f;//DistanceTraveled;
					tutorialObjectShowing = UIManagerOz.SharedInstance.inGameVC.ShowAvoidInstruction();
					break;
				case 5:
					BonusItemProtoData.SharedInstance.AllowCoins = true;
					tutorialObjectShowing = UIManagerOz.SharedInstance.inGameVC.ShowTiltInstruction();
					break;
				case 6:
					tutorialObjectShowing = UIManagerOz.SharedInstance.inGameVC.ShowMeterInstruction();
					break;
				}
					if(tutorialObjectShowing) tutorialObjectShowing.alpha = 0.0f;
					TimeSinceTutorialEnded = 0.0f;
					canShowNextTutorialStep = true;
				}
				else {
					if(NGUITools.GetActive(tutorialObjectShowing.gameObject) == false) {
						
						if(TutorialID == 0) {
							lastTutorialPiece = GamePlayer.SharedInstance.FindNearestPieceOfTypeWithInDistance(lastTutorialPiece, 5.0f, TrackPiece.PieceType.kTPForestJumpOver);
							if(lastTutorialPiece == null) {
								lastTutorialPiece = GamePlayer.SharedInstance.FindNearestPieceOfTypeWithInDistance(lastTutorialPiece, 5.0f, TrackPiece.PieceType.kTPForestJumpOverSlightLeft);
						}
						if(lastTutorialPiece == null) {
							lastTutorialPiece = GamePlayer.SharedInstance.FindNearestPieceOfTypeWithInDistance(lastTutorialPiece, 5.0f, TrackPiece.PieceType.kTPForestSmallGap);
						}
						if(lastTutorialPiece != null) {
							UIManagerOz.SharedInstance.inGameVC.ShowSwipeUp();	
						}
					}
					else if(TutorialID == 1) {
						lastTutorialPiece = GamePlayer.SharedInstance.FindNearestPieceOfTypeWithInDistance(lastTutorialPiece, 14.0f, TrackPiece.PieceType.kTPForestLargeGap);
						if(lastTutorialPiece != null){
							lastTutorialPiece = null;
							UIManagerOz.SharedInstance.inGameVC.ShowSwipeUp();
						}
					
						lastTutorialPiece = GamePlayer.SharedInstance.FindNearestPieceOfTypeWithInDistance(lastTutorialPiece, 15.0f, TrackPiece.PieceType.kTPForestTurnRight);
						if(lastTutorialPiece != null && !finleyPowerUsed){
							GamePlayer.SharedInstance.Kill(DeathTypes.Unknown);
						}
					}
					else if(TutorialID == 2) {
						lastTutorialPiece = GamePlayer.SharedInstance.FindNearestPieceOfTypeWithInDistance(lastTutorialPiece, 5.0f, TrackPiece.PieceType.kTPForestTurnRight);
						if(lastTutorialPiece == null) {
							lastTutorialPiece = GamePlayer.SharedInstance.FindNearestPieceOfTypeWithInDistance(lastTutorialPiece, 5.0f, TrackPiece.PieceType.kTPForestTurnLeft);
							if(lastTutorialPiece != null){
								UIManagerOz.SharedInstance.inGameVC.ShowSwipeLeft();
							}
						}
						else {
							UIManagerOz.SharedInstance.inGameVC.ShowSwipeRight();
						}
						}
						else if(TutorialID == 3) {
							lastTutorialPiece = GamePlayer.SharedInstance.FindNearestPieceOfTypeWithInDistance(lastTutorialPiece, 5.0f, TrackPiece.PieceType.kTPForestSlideUnder);
							if(lastTutorialPiece == null) {
								lastTutorialPiece = GamePlayer.SharedInstance.FindNearestPieceOfTypeWithInDistance(lastTutorialPiece, 5.0f, TrackPiece.PieceType.kTPForestStraight);
						}
						if(lastTutorialPiece == null) {
							lastTutorialPiece = GamePlayer.SharedInstance.FindNearestPieceOfTypeWithInDistance(lastTutorialPiece, 5.0f, TrackPiece.PieceType.kTPForestAnimatedJumpOrSlide);
						}
						
						if(lastTutorialPiece != null) {
							UIManagerOz.SharedInstance.inGameVC.ShowSwipeDown();
						}
					}
					else if(TutorialID == 4) {
						//float diff = DistanceTraveled - monkeyTrackDisntace;
						monkeyTrackDisntace += Player.PlayerVelocity.magnitude * Time.deltaTime;
						//Debug.Log ("Monkey Dist: " + monkeyTrackDisntace);
						//if(diff > 65f && diff < 75f) {
						float distMin = 698f;
						float distMax = 703f;
						if(monkeyTutorialFailed)
						{	
							distMin = 725f;
							distMax = 732f;
						}
						//notify.Debug("DOOOOOOOOOOOOOOOOOOOOO " + monkeyTrackDisntace + " monkeyTutorialFailed " + monkeyTutorialFailed);
						if(monkeyTrackDisntace > distMin && monkeyTrackDisntace < distMax) {
							UIManagerOz.SharedInstance.inGameVC.ShowSwipeDown();
						}
					}
				
					/*
					else if(TutorialID == 5) {
						lastTutorialPiece = GamePlayer.SharedInstance.FindNearestPieceOfTypeWithInDistance(lastTutorialPiece, 1.0f, TrackPiece.PieceType.kTPStraightFlat);
						if(lastTutorialPiece != null) {
							UIManagerOz.SharedInstance.inGameVC.ShowCollectCoinsTutorial();
						}
					}
					*/
					else if(TutorialID == 6) {
						lastTutorialPiece = GamePlayer.SharedInstance.FindNearestPieceOfTypeWithInDistance(lastTutorialPiece, 5.0f, TrackPiece.PieceType.kTPForestStraightShort);
						if(lastTutorialPiece != null) {
							//TimeSinceTutorialEnded = 0.0f;
							//TimeSinceTutorialEndedDelay = 0f;
							//tutorialObjectShowing = UIManagerOz.SharedInstance.inGameVC.ShowMeterInstruction();
							//if(tutorialObjectShowing) tutorialObjectShowing.alpha = 0.0f;
							UIManagerOz.SharedInstance.inGameVC.StartPowerMeterFinger();
						}
					}
				}
			}
	
	
	
	}
	
	
	
	private void doHandlePacingTutorialEnv(){
		// per Sean , lets disable the env tutorial for now, we are just going to show the sign which is in doHandlePacing
		return;
		
		
		//Commenting this out, because the above return; was generating warnings
		/*if(UIManagerOz.SharedInstance.inGameVC.tutorialLabel.gameObject.active || Player.IsOnBalloon){
				return; // wait till we complete the "Nice Job"
		}
		else {
			//notify.Debug("tutorialObjectShowing " + tutorialObjectShowing);
			if(tutorialObjectShowing == null){
				TimeSinceTutorialEndedDelay = 0f;
				tutorialObjectShowing = UIManagerOz.SharedInstance.inGameVC.ShowEnvInstruction();
				if(tutorialObjectShowing) tutorialObjectShowing.alpha = 0.0f;
				TimeSinceTutorialEnded = 0.0f;
				canShowNextTutorialStep = true;
			}
			else {
				//notify.Debug("NGUITools.GetActive(tutorialObjectShowing.gameObject) " + NGUITools.GetActive(tutorialObjectShowing.gameObject));
				if(NGUITools.GetActive(tutorialObjectShowing.gameObject) == false) {	
					lastTutorialPiece = GamePlayer.SharedInstance.FindNearestPieceOfTypeWithInDistance(lastTutorialPiece, 10.0f, TrackPiece.PieceType.kTPEnvSetJunction);							
					if(lastTutorialPiece != null && !didShowSignForYBR) {
						Time.timeScale = 0.2f;
						didShowSignForYBR = true;
						TransitionSignDecider decider = lastTutorialPiece.GetComponentInChildren<TransitionSignDecider>();
						string location = EnvironmentSetManager.SharedInstance.LocalDict[decider.DestinationId].GetLocalizedTitle();
						//UIManagerOz.SharedInstance.inGameVC.ShowTutorialMessage("\n\nFollow signs to\n" + location + "\n(1500M)", tutorialMessageType.MSG);	
						UIManagerOz.SharedInstance.inGameVC.ShowTutorialMessage("\n\n" + Localization.SharedInstance.Get ("Tut_FollowSigns") + "\n" + location + "\n(1500M)", tutorialMessageType.MSG);	
					
					}
					lastTutorialPiece = null;
				
				
					lastTutorialPiece = GamePlayer.SharedInstance.FindNearestPieceOfTypeWithInDistance(lastTutorialPiece, 6.0f, TrackPiece.PieceType.kTPEnvSetJunction);							
					if(lastTutorialPiece != null) {
						//notify.Debug("we are close to env change, trig the tutorial");
						Time.timeScale = 0.2f;
						TransitionSignDecider decider = lastTutorialPiece.GetComponentInChildren<TransitionSignDecider>();
						if(decider.MainLeftGoesToTransitionTunnel){
							//Debug.Log ("decider is right " + decider.MainLeftGoesToTransitionTunnel);
							UIManagerOz.SharedInstance.inGameVC.ShowSwipeLeft();
						}
						else {
							UIManagerOz.SharedInstance.inGameVC.ShowSwipeRight();
							//Debug.Log ("decider is left " + decider.MainLeftGoesToTransitionTunnel);
						}
					}
				}
			}
		}*/
	}
	
	
	IEnumerator StartBalloonTutorialWithDelay(float timer){
		yield return new WaitForSeconds(timer);
		StartBalloonTutorial();
	}
	
	public void StartBalloonTutorial(){
		notify.Debug("StartBalloonTutorial");
		//if(!GameProfile.SharedInstance.ShowTutorialBalloon) return;
		if(PlayerPrefs.GetInt("ShowTutorialBalloon")==2 ) return;
		if(tutorialBalloonOn) return;
		if(envChangeSignAvailable) return;
		if(tutorialObjectShowing) return;
		tutorialBalloonOn = true;
		TimeSinceTutorialEndedDelay = 0f;
		tutorialObjectShowing = UIManagerOz.SharedInstance.inGameVC.ShowBalloonInstruction();
		if(tutorialObjectShowing) tutorialObjectShowing.alpha = 0.0f;
		TimeSinceTutorialEnded = 0.0f;
		canShowNextTutorialStep = true;
	}
	
	public void ShowBallonInstructionsTilt(){
		//notify.Debug("ShowBallonInstructionsTilt");
		TimeSinceTutorialEndedDelay = 2f;
		tutorialObjectShowing = UIManagerOz.SharedInstance.inGameVC.ShowTiltInstruction(true);
		if(tutorialObjectShowing) tutorialObjectShowing.alpha = 0.0f;
		TimeSinceTutorialEnded = 0.0f;
		canShowNextTutorialStep = true;
	}
	
	public void ShowBalloonDamage(){
		if(!tutorialBalloonOn) return;
		tutorialBalloonOn = false;
		//GameProfile.SharedInstance.ShowTutorialBalloon = false;
		//GameProfile.SharedInstance.Serialize();
		PlayerPrefs.SetInt("ShowTutorialBalloon",2);
		PlayerPrefs.Save();
		UIManagerOz.SharedInstance.inGameVC.ShowTutorialMessage(Localization.SharedInstance.Get ("Tut_SinkBalloon"),tutorialMessageType.MSG);	
	}
	
	private void doHandlePacingTutorialBalloon(){
		
		
		lastTutorialPiece = GamePlayer.SharedInstance.FindNearestPieceOfTypeWithInDistance(lastTutorialPiece, 8.0f, TrackPiece.PieceType.kTPBalloonJunction);
		if(lastTutorialPiece != null){
			UIManagerOz.SharedInstance.inGameVC.ShowSwipeLeft();
		}
		
		//check to see if we just entered a balloon
		if(Player.OnTrackPiece != null && Player.OnTrackPiece.IsBalloon() && !didShowBalloonTilt){
			didShowBalloonTilt = true;
			ShowBallonInstructionsTilt();
		}
	}
	
	/*
	public void CompleteTutorialEnv(){
		notify.Debug("CompleteTutorialEnv");
		Time.timeScale = 1f;
		tutorialEnvOn = false;
		forceTutorialEnvOn = false;
		// per Dave.  get rid of that 
		//UIManagerOz.SharedInstance.inGameVC.ShowTutorialMessage(Localization.SharedInstance.Get("Tut_OnWay"), tutorialMessageType.MSG);	
		PlayerPrefs.SetInt("ShowTutorialEnv",2);
		PlayerPrefs.Save();
	}
	
	public void FailedTutorialEnv(){
		notify.Debug("FailedTutorialEnv");
		Time.timeScale = 1f;
		// per Dave.  get rid of that 
		//UIManagerOz.SharedInstance.inGameVC.ShowTutorialMessage(Localization.SharedInstance.Get("Tut_WrongTurn"), tutorialMessageType.FAIL);
		tutorialEnvOn = false;
		forceTutorialEnvOn = false;
		PlayerPrefs.SetInt("ShowTutorialEnv",2);
		PlayerPrefs.Save();
	}
	*/
	/// <summary>
	/// If the player is fast travelling, see if it's time to request it
	/// </summary> 
	void handleFastTravel()
	{
		if (DistanceTraveled > Player.FastTravelDistance - 	EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.TunnelBufferDistance)
		{
			if (EnvironmentSetSwitcher.SharedInstance.WantNewEnvironmentSet == false &&
				EnvironmentSetSwitcher.IsInactive())
			{
				EnvironmentSetSwitcher.SharedInstance.RequestEnvironmentSetChange(Player.FastTravelDestinationEnvironmentSetId);
			}
		}
	}
	
	public void OnPauseClickedUI()
	{
		IsPaused = true;
		//Player.doPauseAllAnimations();
		Time.timeScale = 0f;
		//Player.doPauseAnimation();
		/*foreach (Enemy enemy in Enemies) {
			if(enemy == null)
				continue;
			
			enemy.doPauseAnimation();
		}*/
		//Enemy.main.doPauseAnimation();
		
		UIManagerOz.SharedInstance.inGameVC.bonusButtons.EnableAllButtons(false);
		
		if(AudioManager.SharedInstance!=null){
			AudioManager.SharedInstance.FadeMusicMultiplier(0.15f, 0.4f);
			AudioManager.SharedInstance.StopFX();
		}
		//if(Enemy.main && Enemy.main.audio) Enemy.main.audio.Stop();
		
		
		// hide envHud in case it's visible from previous run
		//UIManagerOz.SharedInstance.inGameVC.envChangeHud.alpha = 0f;
		UIManagerOz.SharedInstance.inGameVC.envChangeHud.gameObject.SetActiveRecursively(false);
		//UIManagerOz.SharedInstance.inGameVC.tutorialBalloonLabel.alpha = 0f;
		UIManagerOz.SharedInstance.inGameVC.tutorialBalloon.gameObject.SetActiveRecursively(false);
		// get rid of coins from previous run
		UIManagerOz.SharedInstance.inGameVC.fx_coin.Clear();
		
	}
	
	public void OnUnPauseClickedUI()
	{
		doUnPause();
		//Player.doUnPauseAllAnimations();
	}
	
	//-- Start the countdown when going from Paused to UnPaused. This can come from the UI or an in game event like Resurrection.
	public void doUnPause()
	{
		notify.Debug("doUnPause");
		IsStartOfCountDown = true;
		IsInCountdown = true;
		TimeSinceCountdownStarted = 0.0f;
		//Has3SoundPlayed = false;
		Has2SoundPlayed = false;
		Has1SoundPlayed = false;
		
		Vector3 diff = ((OzGameCamera)OzGameCamera.SharedInstance).transform.position - ((OzGameCamera)OzGameCamera.SharedInstance).targetPos;
		float dist = new Vector3(diff.x, 0f, diff.z).magnitude;
		((OzGameCamera)OzGameCamera.SharedInstance).transform.position = ((OzGameCamera)OzGameCamera.SharedInstance).targetPos - ((OzGameCamera)OzGameCamera.SharedInstance).PlayerTarget.transform.forward * dist + Vector3.up * diff.y;
	}
	
	//doCountdown is turned off when we reset the game. if it is false, we skip the countdown.
	private bool doCountdown = true;
	public void doHandleCountDown()
	{
		//notify.Debug("GAME CONTROLLER: doHandleCountDown "  );
		//notify.Debug("IsStartOfCountDown " + IsStartOfCountDown + " doCountdown " + doCountdown);
		
		//notify.Debug("doHandleCountDown");
		
		if (IsStartOfCountDown == false) 
		{
			TimeSinceCountdownStarted += Realtime.deltaTime;
			//notify.Debug("IsStartOfCountDown==false TimeSinceCountdownStarted " + TimeSinceCountdownStarted);
		}
		else 
		{
			if(AudioManager.SharedInstance != null)
			{
				AudioManager.SharedInstance.StopFX();
				AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_CountDown_01);
			}
			Has1SoundPlayed = false;
			Has2SoundPlayed = false;
			//Has3SoundPlayed = true;
			IsStartOfCountDown = false;
//			GamePlayer.StopAllAnimation();
		}
		
		int countDownNumber = Mathf.FloorToInt(4.0f - TimeSinceCountdownStarted);
		//TR.LOG("countDownNumber " + countDownNumber);
		UIManagerOz.SharedInstance.inGameVC.SetCountDownNumber(countDownNumber);
		
		
		if(Player.Dying || Player.IsFalling) {
			TimeSinceCountdownStarted = 3f;
			Has2SoundPlayed = true;
			Has1SoundPlayed = true;
		}

		if (TimeSinceCountdownStarted > 1.0f && !Has2SoundPlayed) 
		{
			if(AudioManager.SharedInstance != null)
			{
				AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_CountDown_01);
			}
			
			Has2SoundPlayed = true;
		}
		else if (TimeSinceCountdownStarted > 2.0f && !Has1SoundPlayed) 
		{
			if(AudioManager.SharedInstance != null)
			{
				AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_CountDown_01);
			}
			Has1SoundPlayed = true;
		}

		if (TimeSinceCountdownStarted > 3.0f || !doCountdown) 
		{
			//notify.Debug("That's the end of the countdown so lets unpause the game");
			IsInCountdown = false;
			//Player.doUnPauseAnimation();
			/*foreach (Enemy enemy in Enemies) {
				if(enemy == null)
					continue;
				
				enemy.doUnPauseAnimation();
			}*/
			//Enemy.main.doUnPauseAnimation();
			//TR.LOG("Lets unpause the game for real");
			IsPaused = false;
			
			NGUITools.SetActive(UIManagerOz.SharedInstance.inGameVC.pauseButton, true);
		
			UIManagerOz.SharedInstance.inGameVC.bonusButtons.EnableAllButtons(true);

			UIManagerOz.SharedInstance.inGameVC.SetCountDownNumber(-1);
			Time.timeScale = 1f;
			if(AudioManager.SharedInstance!=null)	{
				if(!AudioManager.SharedInstance.IsGameMusicPlaying()){
					AudioManager.SharedInstance.SwitchToGameMusic(0.5f);
				}
				else{
					AudioManager.SharedInstance.FadeMusicMultiplier(0.5f,1f);
				}
				if(Player.HasGlindasBubble){
					AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_GlindasBubble_01);
				}
				if(Player.IsOnBalloon){
					AudioManager.SharedInstance.StartBalloonMotor();
				}
				if(Player.HasVacuum){
					AudioManager.SharedInstance.PlayMagnet();
				}
				if(Player.HasPoof){
					AudioManager.SharedInstance.PlayPoof();
				}
			}
			
			// expand tutorial back to normal size, if tutorial is active
			UIManagerOz.SharedInstance.inGameVC.ExpandPeripheralUIelementsBackToNormalAfterPause();
			
			// trigger gemming of ability that was attempted without enough gems, after buying more
			if (UIManagerOz.SharedInstance.inGameVC.sourceArtifactMethod != null)
			{
				UIManagerOz.SharedInstance.inGameVC.sourceArtifactMethod();
				UIManagerOz.SharedInstance.inGameVC.sourceArtifactMethod = null;
			}
			
			//if(UIManagerOz.SharedInstance.inGameVC.isAbilityTutorialOn){
			//	Time.timeScale = 0f;
			//}
			
//			if (GamePlayer.IsSliding) 
//			{
//				Audio.PlayFX(AudioManager.Effects.slide);
//			}
//			
//			if (GamePlayer.HasVacuum) 
//			{
//				Audio.PlayFX(AudioManager.Effects.magnet);
//			}
//			
//			if (GamePlayer.HasInvincibility && !GamePlayer.HasBoost) 
//			{
//				Audio.PlayFX(AudioManager.Effects.shimmer);
//			}
//			
			if (Player.HasBoost) 
			{
				AudioManager.SharedInstance.PlayFX(AudioManager.Effects.boostLoop);
			}
			
			doCountdown = true;
		}

		// Fade in the player on resurrection
//		if (GamePlayer.GetAlpha() <= 0.5f) 
//		{
//			float alpha = GamePlayer.GetAlpha();
//			alpha += Time.deltaTime * 0.5f;
//			if (alpha > 0.5f)
//				alpha = 0.5f;
//			GamePlayer.SetAlpha(alpha);
//		}
	}
	
/*	public void doEnableIntroScene(bool enble)
	{
		IsIntroScene = enble;
		// TODO Delete the extra monkeys used for intro scene
	}*/
	
	//Made this seperate from doEnableIntroScene because it was disappearing too early,
	// and we don't want the player input to kick in any later
	public void DestroyStartingPiece()
	{
		notify.Debug("DestroyStartingPiece " );
		if(StartingSetPiece != null)
		{
			startingSetPieceOn = false;
			Transform starttemp;
			starttemp = StartingSetPiece.transform;
			foreach(Transform children in starttemp)
				{
				    Destroy(children.gameObject);
				}
		}
	}
	
	private Vector2 tempVec2A = new Vector2(0,0);
	private Vector2 tempVec2B = new Vector2(0,0);
	private int lastSetScore = -1;
	
	private float realDist = 0f;
	public float RealDistanceTraveled { get { return realDist; } }
	
	void doComputeDistanceTraveled()
	{
		if (Player == null)
			return;
		
		//What does this do?
		DistanceTraveledPreviousFrame = DistanceTraveled;
		
		
		if (Player.IsFalling == false && !Player.Dying) 
		{
	
				tempVec2A.x = Player.transform.position.x;
				tempVec2A.y = Player.transform.position.z;
				tempVec2B.x = Player.PreviousLocation.x;
				tempVec2B.y = Player.PreviousLocation.z;
				
				
				float distProjected = Vector2.Distance(tempVec2A, tempVec2B);
				
				// This some voodoo magic to fix the fact that our first release was using dist2 instead of dist.
				// This magic keeps the distanceTravel curve looking the same on fast devices and fixes
				// the issue with slow devices accuring distance much much faster.  The only reason we still keep this around is to keep distance
				// traveled similar across TR1 and TR2
				
			//Make sure we don't divide by zero
			float distanceTraveled = 0f;
			if(Time.deltaTime>Mathf.Epsilon)
			{
				float distProjNormalized = (distProjected / Time.deltaTime) * (1.0f / 60.0f);
				distanceTraveled = ((distProjNormalized * distProjNormalized) * (Time.deltaTime / (1.0f / 60.0f))) * 4.5f;
			}
				
			//Only calculate distance if the player is NOT in tutorial or transition tunnel				
			if( GamePlayer.SharedInstance.OnTrackPiece == null
				|| (!IsTutorialMode
				&& GamePlayer.SharedInstance.OnTrackPiece.TrackType != TrackPiece.PieceType.kTPTransitionTunnelMiddle 
				&& GamePlayer.SharedInstance.OnTrackPiece.TrackType != TrackPiece.PieceType.kTPTransitionTunnelMiddle2))
			{
				if((GamePlayer.SharedInstance.HasSuperBoost && GamePlayer.SharedInstance.BoostDistanceLeft > 120f) || Player.HasFastTravel)
				{
					distanceTraveled *= 5f;
				}
			
				realDist += distProjected;
				DistanceTraveled += distanceTraveled;
				DifficultyDistanceTraveled += distanceTraveled;
				DistanceTraveledSinceLastTurnSection += distanceTraveled;
				
				ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.Distance, distanceTraveled);
				
				DistanceRemainder += distanceTraveled;
				int distanceScore = (int)DistanceRemainder;
				DistanceRemainder = DistanceRemainder - distanceScore;
				
				Player.AddScore(distanceScore * 2);
				//TODO: remove for SHIP - expensive on 3GS
				UIManagerOz.SharedInstance.SetDistanceTotal((int)DistanceTraveled);
				
				if(Player.Score != lastSetScore) 
				{
					//UIManagerOz.SharedInstance.SetScore(Player.Score);
					UIManagerOz.SharedInstance.inGameVC.scoreUI.SetScore(Player.Score);
					lastSetScore = Player.Score;	
				}
			}
			
			
			if (Player.HasBoost) 
			{
				// If player is boosting
				Player.BoostDistanceLeft -= distanceTraveled;
			}
			/* this is used to disable stumbleproof after a certain distance
			if (Player.NoStumble) 
			{
				// If player is stumbleProof
				Player.NoStumbleDistLeft -= distanceTraveled;
			}
			*/
			
			//We now do this elsewhere
			/*if (MaxDistanceWithoutCoins == 0 && Player.CoinCountTotal > 0) 
			{
				// keep track of the max distande they ran before collecting coins
				MaxDistanceWithoutCoins = DistanceTraveled;
			}*/
			
			
			//Tutorial Stuff
			if (!IsIntroScene && (IsTutorialMode || tutorialEnvOn || tutorialBalloonOn) && tutorialObjectShowing != null) {
				TimeSinceTutorialEnded += Time.smoothDeltaTime;
				if (TimeSinceTutorialEnded > (0.15f + TimeSinceTutorialEndedDelay) && TimeSinceTutorialEnded < (1.0f + TimeSinceTutorialEndedDelay)) {
					// Fade in next tutorial piece
					NGUITools.SetActive(tutorialObjectShowing.gameObject, true);
					float alpha = tutorialObjectShowing.alpha;
					alpha += (Time.smoothDeltaTime * 8.0f);
					if (alpha > 1.0f)
						alpha = 1.0f;
					tutorialObjectShowing.alpha = alpha;
				}
				else if (TimeSinceTutorialEnded >= (1.5f + TimeSinceTutorialEndedDelay)) {
					// Fade out the tutorial text
					float alpha = tutorialObjectShowing.alpha;
					alpha -= (Time.smoothDeltaTime * 4.0f);
					if (alpha < 0.0f) {
						NGUITools.SetActive(tutorialObjectShowing.gameObject, false);
						//tutorialObjectShowing = null;
//						//-- Tutorial done for now.
//						if (TutorialID >= 2) {
//							endTutorial();
//						}
					}
					else {
						tutorialObjectShowing.alpha = alpha;	
					}
				}
			}
					
		}
	}
	
	
	
	public void onBonusItemCollected(BonusItem.BonusItemType type)
	{
		FinalizeNegativeObjectiveStats();
		
		if(type==BonusItem.BonusItemType.MegaCoin)
			ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.PickupCollectedPenniesFromHeaven,1);
		if(type==BonusItem.BonusItemType.Vacuum)
			ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.PickupCollectedMagicMagnet,1);
		if(type==BonusItem.BonusItemType.Poof)
			ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.PickupCollectedPoof,1);
		if(type==BonusItem.BonusItemType.Boost)
			ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.PickupCollectedFinleysFavor,1);
		if(type==BonusItem.BonusItemType.ScoreBonus)
			ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.PickupCollectedScoreMultiplier,1);
	}
	
	
	
	public void FinalizeNegativeObjectiveStats()
	{
		if(StumblesThisRun==0)
			ObjectivesDataUpdater.SetGenericStat(ObjectiveType.DistanceWithoutStumble,DistanceTraveled);
		if(Player.CoinCountTotal==0)
			ObjectivesDataUpdater.SetGenericStat(ObjectiveType.DistanceWithoutCoins,DistanceTraveled);
		if(ObjectivesDataUpdater.GetStatForObjectiveType(ObjectiveType.DistanceWithoutTransition,-1) == 0)	//If we haven't had to update this...
			ObjectivesDataUpdater.SetGenericStat(ObjectiveType.DistanceWithoutTransition,DistanceTraveled);
		
		if(collectedBonusItemPerRun[(int)BonusItem.BonusItemType.Boost]==0 &&
				collectedBonusItemPerRun[(int)BonusItem.BonusItemType.Poof]==0 &&
				collectedBonusItemPerRun[(int)BonusItem.BonusItemType.Vacuum]==0 &&
				collectedBonusItemPerRun[(int)BonusItem.BonusItemType.ScoreBonus]==0 &&
				collectedBonusItemPerRun[(int)BonusItem.BonusItemType.MegaCoin]==0)
			ObjectivesDataUpdater.SetGenericStat(ObjectiveType.DistanceWithoutPowerups,DistanceTraveled);
	}
	
	
	void doEndGame(float delayTime, bool autoRestart)
	{
		notify.Debug("END GAME");
		TimeOfDeath = Time.time;
		
		if (IsGameOver == false) 
		{
			//IsGameOver = true;
			EndGameDelayTime = delayTime;
			AutoRestart = autoRestart;
			
			FinalizeNegativeObjectiveStats();
			
			
			//Shouldnt need this, we do it elsewhere
			//if (Player.CoinCountTotal == 0)
			//	MaxDistanceWithoutCoins = DistanceTraveled;

			// TODO reset touch state
			//IMPORTANT!!! This is comented out, because here, this is done during "SAVE ME!"
			//Moving the same function to another place
			//UpdateAndSaveRecords();

			// TODO stop background music and sounds
			AudioManager.SharedInstance.StopFX(false);
			
			
			StatTracker.Stop();
			
			/*
			TuningAnalyticsManager analyticsManager = 
				Services.Get<TuningAnalyticsManager>();
			
			if (!Settings.GetBool("invulnerable", false)) {
				analyticsManager.SendTuningAnalytics(
					BuildVersion.GetBuildVersion() + "," +
					MACAddress.ShowPrimaryNetworkInterface() + "," +
					StatTracker.GetStatsCommaDelimitedString() + "," +
					this.DistanceTraveled.ToString() + "," +
					Player.Score.ToString() + "," +
					Player.CoinCountTotal.ToString() + "," +
					Player.GemCountTotal.ToString() + "," +
					(Player.OnTrackPiece.PreviousTrackPiece==null ? "null" : Player.OnTrackPiece.PreviousTrackPiece.TrackType.ToString()) + "," +
					Player.OnTrackPiece.TrackType.ToString() + "," +
					Player.OnTrackPiece.NextTrackPiece.TrackType.ToString()
				);
			} */
		}
	}
	
	public void UpdateAndSaveRecords()
	{
		// Update Player Records
		GameProfile.SharedInstance.UpdateStatsForSession( 
			Player.Score, 
			Player.CoinCountTotal, 
			Player.GemCountTotal,
			(int)DistanceTraveled);

		// Udate Player Achievement REcords
		//UpdateAchievementRecords();

		// Save Everything
		GameProfile.SharedInstance.Serialize();
	}
	
	void doHandleDelayedJumping()
	{
		if (InputController.ShouldJump == true && Player.IsDead == false) 
		{
			bool jumpHandled = Player.Jump();
			if (jumpHandled) 
			{
				// If the current piece or next piece is an obstacle also make the monkey jump.
				if (Player.OnTrackPiece != null && (Player.OnTrackPiece.IsObstacle() || (Player.OnTrackPiece.NextTrackPiece != null && Player.OnTrackPiece.NextTrackPiece.IsObstacle())))  {
					//foreach (Enemy e in Enemies) 
					//{
						float dist = Vector2.Distance(GamePlayer.SharedInstance.CurrentPosition, Enemy.main.transform.position);
						float timeTilJump = dist / GamePlayer.SharedInstance.getRunVelocity();
						Enemy.main.Jump(timeTilJump);
					//}
				}
				
				InputController.ShouldJump = false;
				InputController.TimeSinceShouldJump = 0;
			}
		}
	}
	
	
	public void SimulateEnemies()
	{
		//notify.Debug ("SimulateEnemies " + EnemyFollowDistance);
		/*if (Enemies.Count == 0)
		{
			notify.Debug("HuhWha?");
			return;
		}*/
		if(Enemy.main==null)
		{
			notify.Debug("WHAT?!?!?!");
			return;
		}

		// Delete the extra monkeys used for the intro scene
//		if (IsIntroScene == false) {
//			if (Enemies.Count > 3) {
//				for (int ei = 3; ei < Enemies.Count; ++ei) {
//					GameObject.Destroy(Enemies[ei].gameObject);
//				}
//				Enemies.RemoveRange(3, Enemies.Count - 3);
//			}
//		}

		float stumbleKillPercent = GamePlayer.SharedInstance.StumbleKillTimer / (2.0f*GamePlayer.SharedInstance.DeathByMonkeyTime);
		float slope = 46.0f;
		float adjust = (slope*0.5f) - slope;
		float targetFollowDistance = slope * stumbleKillPercent + adjust;

		if (targetFollowDistance < 2.5f) {
			targetFollowDistance = 2.5f;
		}
		else if (targetFollowDistance > GamePlayer.SharedInstance.DeathByMonkeyTime) {
			targetFollowDistance = 10.0f;
		}

		float catchUpSpeed = 10.0f;
		
		//Catacombs
		if(GamePlayer.SharedInstance.IsInCatacombs)
		{
			targetFollowDistance = 2.5f;	
		}
		
		if (GamePlayer.SharedInstance.IsSliding) {
			targetFollowDistance += 1.0f;
		}
		else if (InputController.JustTurned) {
			targetFollowDistance += 5.0f;
		}
		
		else if(GamePlayer.SharedInstance.HasBoost || GamePlayer.SharedInstance.HasSuperBoost || GamePlayer.SharedInstance.IsOnBalloon)
			targetFollowDistance += 8f;
		

		if (GamePlayer.SharedInstance.IsDead && GamePlayer.SharedInstance.DeathType == DeathTypes.Eaten) {
			targetFollowDistance = 0.0f;
		}

		float targetFollowDifference = targetFollowDistance - EnemyFollowDistance;

		float direction = 1.0f;

		if (targetFollowDifference < 0.0f) {
			direction = -1.0f;
			catchUpSpeed = 10.0f;
		}

		float targetUpdateAmount = (direction * Time.deltaTime * catchUpSpeed);

		if (Mathf.Abs(targetUpdateAmount) > Mathf.Abs(targetFollowDifference)) {
			targetUpdateAmount = targetFollowDifference;
		}

		EnemyFollowDistance = EnemyFollowDistance + targetUpdateAmount;
		
		
		//Only follow after the monkeys would have passed the origin, so they can fly down from the cliffs...
		// First, wait until a little BEFORE that to start the animation. this is a little hackneyed.
		if(!Enemy.main.HasFlownIn && Player.transform.position.z < 12f-EnemyFollowDistance)
			Enemy.main.FlyIn();
		else if (GamePlayer.SharedInstance.IsFalling == false && (Player.transform.position.z < -EnemyFollowDistance || GameController.SharedInstance.TimeSinceGameStart>10f)) {		
			Enemy.main.doUpdate();
		}
	}
	
	public void ResetEnemies() {
		//int EnemyCount = Enemies.Count;
		//Enemy enemy = null;
		//for(int eID = 0; eID < EnemyCount; eID++) {
		//	enemy = Enemies[eID];
		//	if(enemy == null)
		//		continue;
		//	enemy.Reset();
		//}
		
		if(Enemy.main!=null)
			Enemy.main.Reset();
	}
	
	void doHandleDelayedSliding()
	{
		if (InputController.ShouldSlide && Player.IsDead == false) 
		{
			bool slideHandled = Player.Slide();
			if (slideHandled) 
			{
				InputController.ShouldSlide = false;
				InputController.TimeSinceShouldSlide = 0;
			}
		}
	}
	
	void doHandleDelayedTurning()
	{
		if(Player == null || Player.IsDead == true || Player.OnTrackPiece == null)
			return;
		
		//-- Check that we are on the turn piece.
		if(Player.OnTrackPiece.IsTurn() == false && !Player.IsOnBalloon)
		{
			return;
		}

		bool goLeft = false;
		bool goRight = false;
		bool fall = false;
		if((Player.HasBoost == true || Player.HasInvincibility == true) && LastTurnPiece != Player.OnTrackPiece)
		{
			if(TrackBuilder.IsJunctionType(Player.OnTrackPiece.trackPieceDefinition) == true)
			{
				if(UnityEngine.Random.Range(0,2) == 0)
				{
					goLeft = true;
				}
				else
				{
					goRight = true;
				}
			}
			else if(Player.OnTrackPiece.CanTurnLeft() == true)
			{
				goLeft = true;
			}
			else if(Player.OnTrackPiece.CanTurnRight() == true)
			{
				goRight = true;
			}
		}
		else if (InputController.ShouldTurnLeft && Player.OnTrackPiece.CanTurnLeft() 
			&& !GamePlayer.SharedInstance.IsAnimPlaying(GamePlayer.AnimType.kBalloonExit) 
			&& !GamePlayer.SharedInstance.IsAnimPlaying(GamePlayer.AnimType.kBalloonExitFail))
		{
			goLeft = true;
		}
		else if (InputController.ShouldTurnRight && Player.OnTrackPiece.CanTurnRight()
			&& !GamePlayer.SharedInstance.IsAnimPlaying(GamePlayer.AnimType.kBalloonExit)
			&& !GamePlayer.SharedInstance.IsAnimPlaying(GamePlayer.AnimType.kBalloonExitFail))
		{
			goRight = true;
		}
		else if (Player.DidBalloonFail && Player.IsOnBalloon)
		{
		//	notify.Debug("Falling...");
			fall = true;
			Player.DidBalloonFail = false;
		}
		
		if(goLeft == true)
		{
			Player.doTurnLeft();
			InputController.JustTurned = true;
			InputController.TimeSinceLastTurn = 0;
			InputController.ShouldTurnLeft = false;
			InputController.ShouldTurnRight = false;
			LastTurnPiece = Player.OnTrackPiece;
			if (onPlayerTurnEvent != null)
			{
				onPlayerTurnEvent(true);	
			}
		}
		else if(goRight == true)
		{
			Player.doTurnRight();
			InputController.JustTurned = true;
			InputController.TimeSinceLastTurn = 0;
			InputController.ShouldTurnLeft = false;
			InputController.ShouldTurnRight = false;
			LastTurnPiece = Player.OnTrackPiece;
			if (onPlayerTurnEvent != null)
			{
				onPlayerTurnEvent(false);	
			}
		}
		//TAKE OUT SCENE NAME CHECK AFTER BALLOON TESTING
		else if(fall == true && TrackBuilder.SharedInstance.DebugTrackSegment==null)
		{
			Player.balloonFall();
		}
	}
	
	public void OnPlayClickedUI()
	{
		notify.Debug("OnPlayClickedUI");
		doGameStart();
	}
	
	public void OnRestartClickedUI()
	{
		notify.Debug("OnRestartClickedUI");
		//-- Hide the main menu.
		RestartGame();
		doGameStart();
		//doCountdown = false;
	}
	
	// player turned left or right (correctly), isLeft is true if he went left, isLeft is false if he turns right
	public delegate void OnPlayerTurnHandler(bool isLeft);
	private static event OnPlayerTurnHandler onPlayerTurnEvent = null;
	
	public void RegisterForOnPlayerTurn( OnPlayerTurnHandler delg)
	{
		onPlayerTurnEvent += delg;
	}
 	
	public void UnegisterForOnPlayerTurn( OnPlayerTurnHandler delg)
	{
		onPlayerTurnEvent -= delg;
	}	

	public void SpawnOpeningTile()
	{
		notify.Debug("SpawnOpeningTile " + StartingSetPiece.transform.GetChildCount());
		if(StartingSetPiece.transform.GetChildCount() > 0)
			return;
		
		DestroyStartingPiece();
		
		string prefabPath = EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.OpeningTilePrefabPath;
		GameObject prefab = ResourceManager.Load(prefabPath) as GameObject;
		openingTile = Instantiate(prefab) as GameObject;
		TrackPiece.RemoveUnwantedMeshes(openingTile);
		openingTile.transform.parent = StartingSetPiece.transform;	
		TrackPiece.UpdateRendererMaterials(openingTile, true);
		startingSetPieceOn = true;
	}

	public void DestroyOZSkyBox () 
	{		
		notify.Debug("DestroyOZSkyBox");
		Transform skyboxtemp;
		skyboxtemp = OzSkyBox.transform;

		foreach(Transform children in skyboxtemp)
		{
		    Destroy(children.gameObject);
		}
		
	}
	
	public IEnumerator SpawnOZSkyBoxCoroutine (float tintValue) 
	{		
		DestroyOZSkyBox();

		notify.Debug("SpawnOZSkyBoxCoroutine");
		//Transform skyboxtemp;
		//skyboxtemp = OzSkyBox.transform;

		EnvironmentSetData envSetData = EnvironmentSetManager.SharedInstance.LocalDict[NeededSkyBox];
		string prefabPath = envSetData.SkyboxPrefabPath;
		AssetBundleRequest abr = ResourceManager.SharedInstance.LoadAsyncFromAssetBundle(prefabPath, typeof(GameObject));
		GameObject prefab = null;
		if(abr != null)
		{
			yield return abr;
			prefab = abr.asset as GameObject;
		}
		else
		{
			prefab = ResourceManager.Load(prefabPath) as GameObject;
		}
		GameObject newGO = Instantiate(prefab) as GameObject;
		newGO.transform.parent = OzSkyBox.transform;	
		newGO.transform.localPosition = Vector3.zero;
		SkyboxMaterials skyboxMaterials = OzSkyBox.GetComponentInChildren<SkyboxMaterials>();
		if(skyboxMaterials)
		{
			skyboxMaterials.SetMaterial();
			skyboxMaterials.SetTintValue(tintValue);
			skyboxMaterials.SetTintColorToWhite();
		}
	}
	
	public void SpawnOZSkyBox (float tintValue) 
	{		
		SpawnOZSkyBox();
		SkyboxMaterials skyboxMaterials = OzSkyBox.GetComponentInChildren<SkyboxMaterials>();
		if(skyboxMaterials)
		{
			skyboxMaterials.SetMaterial();
			skyboxMaterials.SetTintValue(tintValue);
			skyboxMaterials.SetTintColorToWhite();
		}		
	}

	public void SpawnOZSkyBox () 
	{		
		notify.Debug("SpawnOZSkyBox");
		EnvironmentSetData envSetData = EnvironmentSetManager.SharedInstance.LocalDict[NeededSkyBox];
		string prefabPath = envSetData.SkyboxPrefabPath;
		GameObject prefab = ResourceManager.Load(prefabPath) as GameObject;
		
		if(prefab!=null)
		{
			DestroyOZSkyBox();
			GameObject newGO = Instantiate(prefab) as GameObject;
			newGO.transform.parent = OzSkyBox.transform;	
			newGO.transform.localPosition = Vector3.zero;
		}
	}

	public void MoveCameraToTemple(){
		notify.Debug("MoveCameraToTemple");
		SpawnOpeningTile();
		GameCamera.SharedInstance.camera.enabled = true;
		GameCamera.SharedInstance.enabled = false;
		ResetEnemies();
		Animation animateRoot = GameCamera.SharedInstance.transform.parent.parent.animation;
		GameCamera.SharedInstance.cameraState = CameraState.cinematicOpening;
		if (GameCamera.SharedInstance.animation != null)
		{
			animateRoot.enabled = true;
			animateRoot["CameraOpening"].normalizedTime = 1f;
			animateRoot.Sample();
			animateRoot.enabled = false;
			GameCamera.SharedInstance.transform.localPosition = Vector3.zero;
			GameCamera.SharedInstance.transform.localRotation = Quaternion.identity;
			GameCamera.SharedInstance.transform.parent.localRotation = Quaternion.identity;
		}
	}
	
	public bool IsSafeToLaunchDownloadDialog()	// used by recurring download dialogs, to be launched periodically when app regains focus
	{
		if ((gameState == GameState.PRE_RUN || gameState == GameState.IN_MENUS)
			&& UIManagerOz.SharedInstance.NoModalDialogsCurrentlyShowing() && !UIManagerOz.IsPromptSequenceInProgress() // check if any other modal dialogs are active
			&& mMenuButtonToPressAfterGameIntro == null ) // check if we're launching as a result of a push notification
			return true;
		else
			return false;
	}	
	
	public bool IsSafeToUpdateWeeklyObjectives()
	{
		if (gameState == GameState.PRE_RUN || gameState == GameState.IN_MENUS)
			return true;
		else
			return false;
	}		
	
	public void MenusEntered()
	{
		gameState = GameState.IN_MENUS;		// called when entering menu state (not including post-run animations, intro, or in-game)
		UIManagerOz.SharedInstance.MainGameCamera.enabled = false;
	}
	
	//public bool debugUsed = false;
	public void ManipulateDistance(int amount)
	{

		//realDist += amount;	//Do we want to add to realDist when we use debug?
		DistanceTraveled += amount;
		DifficultyDistanceTraveled += amount;
		DistanceTraveledSinceLastTurnSection += amount;
		
		ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.Distance, amount);
		//debugUsed = true;
	}
	
	/// <summary>
	/// Sets a member variable that tells us where a deep-linking push notification should take us
	/// </summary>
	/// <param name='buttonName'>
	/// The name of the menu button that we should press once we get to the main menu
	/// Possible values are:
	/// 	AbilitiesButton
	/// 	ChallengesButton
	/// 	CharactersButton
	/// 	LeaderboardsButton
	/// 	LegendaryChallengesButton
	/// 	MoreCoins
	/// 	SettingsButton
	/// 	StatsButton
	/// 	TeamChallengesButton
	/// 	UpgradesButton
	/// 	UtilitiesButton
	/// 	WorldofOzButton
	/// </param>
	private void _setMenuButtonToPressAfterGameIntro( string buttonName )
	{
		notify.Debug( "_setMenuButtonToPressAfterGameIntro: " + buttonName );
		bool allowUrbanAirshipDeepLinking = Settings.GetBool( "urban-airship-deep-linking", true );
		if ( allowUrbanAirshipDeepLinking == true )
		{
			// Tell the idol screen to open the menu and press the specified menu button (once the intro is complete)
			mMenuButtonToPressAfterGameIntro = buttonName;
		}
	}
}






//	public bool IsSafeToLaunchDownloadDialog()	// used by recurring download dialogs, to be launched periodically when app regains focus
//	{
//		//if (!IsGameStarted && !IsGameOver)		// eliminate the possibility of showing popup before playing game once by disabling this
//		//	return true;
//		
//		if (IsGameOver && !IsHandlingEndGame)		//if (IsGameStarted && IsGameOver && !IsHandlingEndGame)
//			return true;
//		
//		return false;
//	}




//	public bool IsGameStarted { get; set; }
//	public bool IsGameOver { get ; set; }
//	public bool IsResurrecting { get; set; }
//	public bool IsPaused { get; set; }
//	public bool IsIntroScene { get; set; }
//	public bool IsTutorialMode { get; set; }
//	public bool IsHandlingEndGame { get; set; }
	


//	void ShowObject(GameObject theObject, bool show, bool recursive)
//	{
//		if(theObject == null)
//			return;
//			
//		if(recursive == true)
//		{
//			theObject.SetActiveRecursively(show);
//		}
//		else
//		{
//			theObject.active = show;
//		}
//	}
