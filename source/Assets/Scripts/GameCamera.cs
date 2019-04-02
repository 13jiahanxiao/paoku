using UnityEngine;
using System.Collections;

public enum CameraState{
		cinematicPullback,
		cinematicOpening,
		gamplay
	}

public class GameCamera : MonoBehaviour	
{
	protected static Notify notify;
	public CameraState cameraState;
	
	protected float m_TargetYaw;
    protected float m_TargetPitch;
    public float m_TargetDistance;
	protected float m_CurrentYaw = 25.0f;
    protected float m_CurrentPitch;
	protected float m_CurrentDistance;
	
	protected float m_FollowHeight;
	protected float m_FollowDistance;
	protected float m_FocusHeight;
	protected float m_FocusDistance;
	protected float m_FocusXOffset;
	
	public static GameCamera SharedInstance;
	
	protected  Vector3 m_TargetCameraLocation = new Vector3(0,0,0);
	
	public float 		SmoothRotationSpeed = 8;
	public float		SmoothZoomSpeed = 3;
	public float		SmoothPitchSpeed = 3;
	public Transform 	FocusTarget = null;
	public GamePlayer 	PlayerTarget = null;
	public float 		RunFollowHeight;
	public float 		RunFollowDistance;
	public float 		RunFocusHeight;
	public float 		RunFocusDistance;
	public float 		JumpFollowHeight;
	public float 		JumpFollowDistance;
	public float 		JumpFocusHeight;
	public float 		JumpFocusDistance;
	public float 		SlideFollowHeight;
	public float 		SlideFollowDistance;
	public float 		SlideFocusHeight;
	public float 		SlideFocusDistance;
	public float 		ZipFollowDistance;
	public float 		UpHillFollowDistance;
	public float		UpHillFollowHeight;
	public float		UpHillSmoothZoomSpeed;
	public float		UpHillFocusHeight;
	public float		UpHillFocusDistance;
	
	
	
	public float 		MineFocusHeight;
	public float 		MineFocusDistance;
	public float 		MineFollowDistance;
	public float 		MineFollowHeight;	
	
	public float 		BalloonFocusHeight;
	public float 		BalloonFocusDistance;
	public float 		BalloonFollowDistance;
	public float 		BalloonFollowHeight;
	public float 		BalloonXOffset;
	public float		BalloonSmoothZoomSpeed;
	
	//Balloon Tilt Vars
	float				tempTiltAmount;
	float				currentTiltAmount;
	
	// this is used to set targetPos and targetRot in GameController so that the camera knows where to go  to after CameraOpening animation
	[HideInInspector]
	public Vector3 targetPos;
	[HideInInspector] 
	public Quaternion targetRot;
	
	public Vector3 		StartLocation = new Vector3(90,110,150);
	public Vector3 		EndLocation = new Vector3(0,10,86);
	
	public bool			FlyMode {get{return isFlyMode;}}
	
	protected bool		isFlyMode = false;
	private Transform	CacheTransform = null;
	public Transform CachedTransform 
	{
		get
		{
			if(CacheTransform == null)
			{
				CacheTransform = transform;
			}
			return CacheTransform;
		}
	}
	public Vector3 CachedPosition
	{
		get; protected set;
	}
	//-- MOVE THIS TO A UTIL CLASS
	public static float SignedAngle(Vector3 v1, Vector3 v2, Vector3 n)
    {
        return Mathf.Atan2(Vector3.Dot(n, Vector3.Cross(v1, v2)), Vector3.Dot(v1, v2)) * 57.29578f;
    }

	void Awake()
	{
		SharedInstance = this;
		notify = new Notify(this.GetType().Name);
		CacheTransform = transform;
		LateUpdate();
	}

	void Start(){
		/*
		animation["CameraOpening"].speed = 0.4f;
		animation.Play("CameraOpening");
		enabled = false;
		*/
		
		SharedInstance = this;
		m_FocusHeight = 2.1f;
		m_FollowHeight = 2.0f;
		m_FollowDistance = 2.46f;
		
		m_FocusXOffset = 0f;

		reset();
		
		
	}
	
	void reset()
	{
		m_FocusHeight = RunFocusHeight;
		m_FollowHeight = RunFollowHeight;
		m_FollowDistance = RunFollowDistance;
		
		IsCameraShaking = false;
		ShakeAfterDelay = false;
		TimeSinceCameraShakeStart = 0.0f;
		CameraShakeDamperRate = 0.0f;
		CameraShakeMagnitude = 0.0f;
		CameraShakeDuration = 0.0f;
		CameraShakeFrequencyMultiplier = 1.0f;
		
		
		setFocusTargetPosition();
		
		//-- Do this after setFocusTargetPosition but not inside.
		m_CurrentDistance = m_TargetDistance;
		m_FocusXOffset = 0f;
	}
	
	public void StartMenuAniamtion(bool snapToEnd) {
		notify.Debug ("StartMenuAniamtion "+snapToEnd.ToString());
		if(snapToEnd == true) {
			StopFlyIN();
		}
		else {
			Animation anim = this.animation;
			if(anim == null)
			{
				isFlyMode = false;
				CacheTransform.position = EndLocation;
				return;
			}
			anim.Play();
		}
	}
	
	void StopFlyIN() {
		//TR.LOG ("STOPFLYIN");
		isFlyMode = false;
		this.animation.Stop();
		CacheTransform.position = EndLocation;
//		if(UIManager.SharedInstance.idolMenuVC != null) {
//			UIManager.SharedInstance.idolMenuVC.previousViewController = null;
//			UIManager.SharedInstance.idolMenuVC.appear();
//		}
	}
	
	public void SpeedUpCinematicPullBack(){
		animation["CameraPullBack"].speed = 3.5f;
	}
	
	void setFocusTargetPosition()
	{
		if(PlayerTarget && FocusTarget)
		{
			if(PlayerTarget.IsHangingFromWire == true)// || PlayerTarget.IsOnBalloon)
			{
				m_TargetCameraLocation = PlayerTarget.CurrentPosition;
			}
			else{
				m_TargetCameraLocation = PlayerTarget.CachedTransform.position;
			}
			
			if(PlayerTarget.OnTrackPiece != null)
			{
				m_TargetCameraLocation.y = PlayerTarget.CurrentPosition.y;
			}
			
			//m_TargetCameraLocation.y = PlayerTarget.currentPosition.y;
			FocusTarget.position = m_TargetCameraLocation;
			
			FocusTarget.rotation = PlayerTarget.CachedTransform.rotation;
			
			//-- Do Logic to set Local member variables according to Player state ( run, jump, slide, zipline, etc)
			computeCameraOffsets();
			
			
			FocusTarget.Translate(Vector3.up * m_FocusHeight, Space.World);
			FocusTarget.Translate(Vector3.right * m_FocusXOffset, Space.World);
		}
	}
	
	void computeCameraOffsets()
	{
		if(PlayerTarget.IsJumping == true)
		{
			//notify.Debug ("test jump");
			m_FocusHeight = JumpFocusHeight;
			m_FocusDistance = JumpFocusDistance;
			m_FollowDistance = JumpFollowDistance;
			m_FollowHeight = JumpFollowHeight;
		}
		else if(PlayerTarget.IsSliding == true)
		{
			m_FocusHeight = SlideFocusHeight;
			m_FocusDistance = SlideFocusDistance;
			m_FollowDistance = SlideFollowDistance;
			m_FollowHeight = SlideFollowHeight;
		}
		else if(PlayerTarget.IsOnAZipline() == true && PlayerTarget.IsHangingFromWire == true)
		{
			float high = 2.0f;
			float low = 0.5f;
			float start = 1.0f;
			float end = 0.25f;
			if(PlayerTarget.CurrentSegment < 6)
			{
				m_FocusHeight = RunFocusHeight;
				m_FocusDistance = RunFocusDistance;
				m_FollowDistance = ZipFollowDistance;
				m_FollowHeight = RunFollowHeight*Mathf.Lerp (start, high, (float)PlayerTarget.CurrentSegment / 6.0f);
			}
			else if(PlayerTarget.CurrentSegment > 20)
			{
				m_FocusHeight = RunFocusHeight;
				m_FocusDistance = RunFocusDistance;
				m_FollowDistance = ZipFollowDistance;//*Mathf.Lerp (1.0f, 2.0f, (float)PlayerTarget.currentSegment / 34.0f);
				m_FollowHeight = RunFollowHeight*Mathf.Lerp (low, end, (float)(PlayerTarget.CurrentSegment-20) / 14.0f);
			}
			else
			{
				m_FocusHeight = RunFocusHeight;
				m_FocusDistance = RunFocusDistance;
				m_FollowDistance = ZipFollowDistance;
				m_FollowHeight = RunFollowHeight*Mathf.Lerp (high, low, (float)(PlayerTarget.CurrentSegment-6) / 14.0f);		
			}
			
		}
		else if(PlayerTarget.IsOnMineCart == true)
		{
			m_FocusHeight = MineFocusHeight;
			m_FocusDistance = MineFocusDistance;
			m_FollowDistance = MineFollowDistance;
			m_FollowHeight = MineFollowHeight;
		}
		

		
		else if(PlayerTarget.IsOnBalloon == true)
		{
			/*
			float positiveTiltAmount = 0;
			float tiltAmount = GamePlayer.SharedInstance.PlayerXOffset;
			
			//Always make tilt float positive
			if (tiltAmount < 0){
				positiveTiltAmount = tiltAmount*-1f;
			}
			else{
				positiveTiltAmount = tiltAmount;	
			}
			*/
			
			//m_FocusHeight = BalloonFocusHeight + tempTiltAmount;
			m_FocusHeight = BalloonFocusHeight;
			m_FocusDistance = BalloonFocusDistance;
			m_FollowDistance = BalloonFollowDistance;
			//m_FollowHeight = BalloonFollowHeight + tempTiltAmount;
			m_FollowHeight = BalloonFollowHeight;
			m_FocusXOffset = BalloonXOffset;
			SmoothZoomSpeed = BalloonSmoothZoomSpeed;
			
			//tempTiltAmount = Mathf.SmoothStep(currentTiltAmount, positiveTiltAmount * 0.2f, Time.deltaTime * 10f);
			//currentTiltAmount = tempTiltAmount;
		}
		
		else
		{
			m_FocusHeight = RunFocusHeight;
			m_FocusDistance = RunFocusDistance;
			m_FollowDistance = RunFollowDistance;
			m_FollowHeight = RunFollowHeight;	
			m_FocusXOffset = 0f;
			BalloonXOffset = 0f;
		}
		
		
		
		//-- Calc a new target Pitch
		m_TargetPitch = Mathf.Atan(m_FollowHeight / m_FocusDistance) * 57.29578f;
		m_TargetDistance = Mathf.Sqrt((m_FollowDistance*m_FollowDistance)+(m_FollowHeight*m_FollowHeight));
	}

	// do updating in LateUpdate to make sure all other objects have moved around.
	public void LateUpdate()
	{
		SharedInstance = this;
		
		if(PlayerTarget == null || !GameController.SharedInstance.isPlayingCinematicPullback)
			return;
		
		if(isFlyMode == true) {
			
		}
		else if (PlayerTarget.IsDead == false) 
		{
			
			//-- Move the FocusTarget to the player
			setFocusTargetPosition();
			
			m_CurrentDistance = Mathf.Lerp (m_CurrentDistance, m_TargetDistance, Time.deltaTime * SmoothZoomSpeed);
			
			//-- Calculate offset vector and a target Yaw
			m_TargetCameraLocation.x = m_TargetCameraLocation.y = 0.0f;
    		m_TargetCameraLocation.z = -m_CurrentDistance;
		
		    if(PlayerTarget.Hold == false)
			{
				m_TargetYaw = SignedAngle(m_TargetCameraLocation.normalized, -PlayerTarget.CachedTransform.forward, Vector3.up);
	            
	        	//-- Clamp targetYaw to -180, 180
	        	m_TargetYaw = Mathf.Repeat(m_TargetYaw + 180f, 360f) - 180f;
	
	            //-- Clamp smooth currentYaw to targetYaw and clamp it to -180, 180
	            m_CurrentYaw = Mathf.LerpAngle(m_CurrentYaw, m_TargetYaw, Time.deltaTime * SmoothRotationSpeed);
	            m_CurrentYaw = Mathf.Repeat(m_CurrentYaw + 180f, 360f) - 180f;	
			}

            //-- Smooth pitch
            m_CurrentPitch = Mathf.LerpAngle(m_CurrentPitch, m_TargetPitch, Time.deltaTime * SmoothPitchSpeed);
			
			// Rotate offset vector
        	m_TargetCameraLocation = Quaternion.Euler(m_CurrentPitch, m_CurrentYaw, 0f) * m_TargetCameraLocation;
        
        	// Position camera holder correctly
			bool holding = false;
			notify.Debug(GameController.SharedInstance.DistanceTraveled);
			if(PlayerTarget.Hold == true || GameController.SharedInstance.DistanceTraveled < 50f)  //(FocusTarget.position.z > -12.0f && GameController.SharedInstance.TimeSinceGameStart < 10.0f))
			{
				//CacheTransform.position = EndLocation;
				if(GameController.SharedInstance.TimeSinceGameStart > Mathf.Epsilon)
				{
					CacheTransform.position = Vector3.Lerp(EndLocation, new Vector3(1.75f,4.79f,-10.0f), GameController.SharedInstance.TimeSinceGameStart / 2.0f);
					
				}
				holding = true;
			}
			else
			{
				CacheTransform.position = FocusTarget.position + m_TargetCameraLocation;
			}

        	// And then have the camera look at our target
        	CacheTransform.LookAt(FocusTarget.position);
			
			if(holding == true)
			{
				m_CurrentDistance = Vector3.Distance (FocusTarget.position, CacheTransform.position);
				m_CurrentYaw = CacheTransform.rotation.eulerAngles.y;
			}
		}
		if (IsCameraShaking) {
			TimeSinceCameraShakeStart -= Time.smoothDeltaTime;
			CameraShakeMagnitude -= (Time.smoothDeltaTime * CameraShakeDamperRate);
			if (CameraShakeMagnitude <= 0.0f || TimeSinceCameraShakeStart > CameraShakeDuration)
				IsCameraShaking = false;
			else {
				float cameraShakeOffsetX = Mathf.Sin(TimeSinceCameraShakeStart * 35.0f * CameraShakeFrequencyMultiplier) * CameraShakeMagnitude;
				float cameraShakeOffsetY = Mathf.Sin(TimeSinceCameraShakeStart * 50.0f * CameraShakeFrequencyMultiplier) * CameraShakeMagnitude;
				CacheTransform.Translate(cameraShakeOffsetX, cameraShakeOffsetY, 0);
			}
		}
		
		CachedPosition = transform.position;
	}
	
	
	// Camera shake info
	public bool IsCameraShaking;
	public bool ShakeAfterDelay;
	public float ShakeDelay;
	public float ShakeMagnitudeAfterDelay;
	public float ShakeDurationAfterDelay;
	public float ShakeFrequencyMultiplierAfterDelay;
	public float TimeSinceCameraShakeStart;
	public float CameraShakeDamperRate;
	public float CameraShakeMagnitude;
	public float CameraShakeDuration;
	public float CameraShakeFrequencyMultiplier;
	
	public void Shake(float magnitude, float duration, float freqMult, float delay = 0)
	{
		if (duration > 0 && magnitude > 0) {
			if (delay > 0.0f) {
				ShakeMagnitudeAfterDelay = magnitude;
				ShakeDurationAfterDelay = duration;
				ShakeFrequencyMultiplierAfterDelay = freqMult;
				ShakeAfterDelay = true;
				ShakeDelay = delay;
				return;
			}

			if (!IsCameraShaking) {
				IsCameraShaking = true;
			}

			ShakeAfterDelay = false;
			TimeSinceCameraShakeStart = 0.0f;
			CameraShakeDamperRate = (magnitude / duration);
			CameraShakeMagnitude = magnitude;
			CameraShakeDuration = duration;
			CameraShakeFrequencyMultiplier = freqMult;
		}		
		
	}
	
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(FocusTarget.position, 0.5f);
		Gizmos.DrawLine(transform.position, FocusTarget.position);
	}
}
