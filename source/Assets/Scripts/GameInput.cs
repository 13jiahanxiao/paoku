using UnityEngine;
using System.Collections;

public class GameInput : MonoBehaviour
{
	protected static Notify notify;
	public enum JoystickPositionType
	{
		kMiddle 		= 0,
		kRight	 		= 1,
		kLeft	 		= -1
	}
	
	//-- Member Data
	public GameController 		TheGameController = null;
	
	protected float 				_Sensitivity = 0.5f;
	public float				AccelControlSensitivy = 1.0f;
	public JoystickPositionType	JoystickPosition = JoystickPositionType.kMiddle;
	public bool 				JoystickMode = false;
	
	[SerializeField]
	private float controlSensitivity_iPhone = 20f;
	[SerializeField]
#pragma warning disable 414
	private float controlSensitivity_iPad = 30f;
#pragma warning restore 414
	
	public float controlSensitivity
	{
		get
		{
#if !UNITY_EDITOR
			switch(GameController.SharedInstance.GetDeviceType())
			{
			case GameController.DeviceType.iPad:
				return controlSensitivity_iPad;
			default:
				return controlSensitivity_iPhone;
			}
#else
			return controlSensitivity_iPhone;
#endif
		}
	}
	
	
	public float Sensitivity {
		get {
			return _Sensitivity;
		}
		set {
			_Sensitivity = value;
			PlayerPrefs.SetFloat ("Sensitivity", value);
		}
	}
	public bool 	JustTurned;
	
	public bool 	shouldTurnLeft = false;
	public bool 	shouldTurnRight = false;
	public bool 	ShouldTurnLeft {
		get { return shouldTurnLeft; }
		set { shouldTurnLeft = value;
			  if(shouldTurnLeft)	GameController.SharedInstance.Player.OnTurnLeftInput();}
	}
	public bool 	ShouldTurnRight {
		get { return shouldTurnRight; }
		set { shouldTurnRight = value;
			  if(shouldTurnRight)	GameController.SharedInstance.Player.OnTurnRightInput();}
	}
	
	public float 	TimeSinceShouldTurn;
	public float 	TimeSinceLastTurn;
	public bool 	ShouldJump;
	public float 	TimeSinceShouldJump;
	public bool 	ShouldSlide;
	public float 	TimeSinceShouldSlide;
	
	//private bool isIntro = false;
					
	public void Awake()
	{
		notify = new Notify( this.GetType().Name);	
	}
	
	void Start(){
		Sensitivity = PlayerPrefs.GetFloat("Sensitivity",0.5f);
	//	Debug.Log ("GameInput Sensitivity " + Sensitivity);
	}
	
	private static float playerXOffsetUnclamped = 0f;
	public static float PlayerXOffsetUnclamped { get { return playerXOffsetUnclamped; } }
	
	//-- Methods
	public virtual void init					(GameController gameController) {notify.Warning("GameInput init: please override");}
	public virtual float update					(bool isIntroScene) 
	{
		float accelForce = getAccelerometerForceX();
		float xOffset = 0.0f;
		
		//isIntro = isIntroScene;
		
		// Ignore all input if this is the intro scene
		if (isIntroScene) {
			if(GameCamera.SharedInstance.cameraState == CameraState.cinematicPullback && Input.GetMouseButtonDown(0)){
				GameCamera.SharedInstance.SpeedUpCinematicPullBack();
			}
			if ((Input.GetKeyDown(KeyCode.G) || Input.GetMouseButtonDown(0)) && GameCamera.SharedInstance.FlyMode == true) 
			{
				GameCamera.SharedInstance.StartMenuAniamtion(true);
			}
			return xOffset;
		}
		
		
		if (isIntroScene == false) 
		{
			// Determine the square of the force (with sign)
			float sensMult = 0.5f + _Sensitivity;
			playerXOffsetUnclamped = -Mathf.Abs(accelForce) * accelForce * controlSensitivity * sensMult;
			float filteredForce = Mathf.Clamp(playerXOffsetUnclamped, -1f,1f);

			if (GamePlayer.SharedInstance.IsStumbling == false && GamePlayer.SharedInstance.StumbleAfterDelay == false) {
				xOffset = filteredForce;	
			}
			else {
				xOffset = GamePlayer.SharedInstance.PlayerXOffset;
			}
		}
		
		//Not in Oz
		/*
		if (GamePlayer.SharedInstance.IsOnMineCart) {
			if (GamePlayer.SharedInstance.PlayerXOffset < -0.4f) {
				ShouldTurnLeft = true;	
				TimeSinceShouldTurn = 0;
			} else if (GamePlayer.SharedInstance.PlayerXOffset > 0.4f) {
				ShouldTurnRight = true;	
				TimeSinceShouldTurn = 0;
			}
		}*/
		
		if (ShouldTurnRight || ShouldTurnLeft) {
			TimeSinceShouldTurn += Time.deltaTime;
			if (TimeSinceShouldTurn > 1.0f) {
				ShouldTurnLeft = ShouldTurnRight = false;
				TimeSinceShouldTurn = 0;
				//TheGameController.Player.AnimateObject.CrossFade("Run01",0.15f);
				//TR.LOG("Cancel Should Turn");
			}
		}

		if (ShouldJump) {
			TimeSinceShouldJump += Time.deltaTime;
			if (TimeSinceShouldJump > 0.5f) {
				ShouldJump = false;
				TimeSinceShouldJump = 0;
			}
		}

		if (ShouldSlide) {
			TimeSinceShouldSlide += Time.deltaTime;
			if (TimeSinceShouldSlide > 0.5f) {
				ShouldSlide = false;
				TimeSinceShouldSlide = 0;
			}
		}
		
		return xOffset;
	}

	public virtual float getAccelerometerForceX	() {
		return 0.0f;
	}
	
	public void Reset()
	{
		JustTurned = false;
		ShouldTurnLeft = false;
		ShouldTurnRight = false;
		TimeSinceShouldTurn = 0.0f;
		TimeSinceLastTurn = 0.0f;
		ShouldJump = false;
		TimeSinceShouldJump = 0.0f;
		ShouldSlide = false;
		TimeSinceShouldSlide = 0.0f;
	}
	
}
