using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OzGameCamera : GameCamera	
{
	public static OzGameCamera OzSharedInstance;

	public bool didPlayCinematicPullback = true;// we remove it false;
	Vector3 TempMoveNumber;
	
	public Transform pointObj;

	void Awake()
	{
		SharedInstance = this;
		OzSharedInstance = this;
		//cameraState = CameraState.cinematicPullback; // remove it
		cameraState = CameraState.cinematicOpening;
		
		LateUpdate();
	}

	void Start()
	{
		
		SharedInstance = this;
		m_FocusHeight = 2.1f;
		m_FollowHeight = 2.0f;
		m_FollowDistance = 2.46f;
		
		m_FocusXOffset = 0f;
		
		/*Register for when the track piece changes.  
		 *Getting the changed piece allows us to make certain assumptions about camera position.
		 */
			
		
		//reset();
		
		
		//GameController.SharedInstance.SetMainMenuAnimation(); // this is instead of cinematicPullback
		
		
	}
	
	public void reset()
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
		
	//	Debug.Log("Reset! " + Time.time);
		
		holdT = 0f;
		
		stopped = false;
		
		setFocusTargetPosition();
		
		//-- Do this after setFocusTargetPosition but not inside.
		m_CurrentDistance = m_TargetDistance;
		
		m_CurrentPitch = m_TargetPitch;
		m_CurrentYaw = m_TargetYaw;
//		notify.Warning("Reset!");
		m_FocusXOffset = 0f;
	}
	
	public void setFocusTargetPosition()
	{
		if(PlayerTarget && FocusTarget && cameraState == CameraState.gamplay)
		{
			if(PlayerTarget.IsHangingFromWire == true || PlayerTarget.IsOnBalloon)
			{
				m_TargetCameraLocation = PlayerTarget.CurrentPosition;
			}
			else{
				m_TargetCameraLocation = PlayerTarget.transform.position;
			}
			
			if(PlayerTarget.OnTrackPiece != null)
			{
				m_TargetCameraLocation.y = PlayerTarget.CurrentPosition.y;
			}
			
			//m_TargetCameraLocation.y = PlayerTarget.currentPosition.y;
			FocusTarget.position = m_TargetCameraLocation;
			
			FocusTarget.rotation = PlayerTarget.transform.rotation;
			
			//-- Do Logic to set Local member variables according to Player state ( run, jump, slide, zipline, etc)
			computeCameraOffsets();
			
			FocusTarget.Translate(Vector3.up * m_FocusHeight, Space.World);
			FocusTarget.Translate(FocusTarget.right * m_FocusXOffset/0.9f, Space.World);
		}
	}
	
	void computeCameraOffsets()
	{
		/*if(PlayerTarget.IsJumping == true)
		{
			//Debug.Log ("test jump");
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
		}*/
		//Not in Oz! Simplifying by removing this.
		/*else if(PlayerTarget.IsOnAZipline() == true && PlayerTarget.IsHangingFromWire == true)
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
		}*/

		
		/*else*/ if(PlayerTarget.IsOnBalloon == true)
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
		
		//else if(PlayerTarget.OnTrackPiece != null )
		//{	
		/*	if(PlayerTarget.OnTrackPiece.TrackType == TrackPiece.PieceType.kTPForestUndulateUpEnter ||
				PlayerTarget.OnTrackPiece.TrackType == TrackPiece.PieceType.kTPUndulateUpEnter ||
				PlayerTarget.OnTrackPiece.TrackType == TrackPiece.PieceType.kTPUndulateUpMid ||
				PlayerTarget.OnTrackPiece.TrackType == TrackPiece.PieceType.kTPUndulateUpExit )
			{
				m_FocusHeight = UpHillFocusHeight;
				m_FocusDistance = UpHillFocusDistance;
				m_FollowDistance = UpHillFollowDistance;
				m_FollowHeight = UpHillFollowHeight;
				SmoothZoomSpeed = UpHillSmoothZoomSpeed;
			}
			else {*/
		//		m_FocusHeight = RunFocusHeight;
		//		m_FocusDistance = RunFocusDistance;
		//		m_FollowDistance = RunFollowDistance;
		//		m_FollowHeight = RunFollowHeight;
		//	m_FocusXOffset = 0f;
		//	BalloonXOffset = 0f;
			//}
		//}
		
		else
		{
			m_FocusHeight = RunFocusHeight;
			m_FocusDistance = RunFocusDistance;
			m_FollowDistance = RunFollowDistance;
			m_FollowHeight = RunFollowHeight;	
			m_FocusXOffset = 0f;
			BalloonXOffset = 0f;
		}
		UpdatePosDirOffsets();
		
		//After doing whatever voodoo above, add offsets for particular tiles.
		//	m_FocusHeight += 0;//currentPositionOffset.y;//TileFocusHeightOffset;
	//		m_FocusDistance += currentPositionOffset.y;//TileFocusDistanceOffset;
			m_FollowDistance += currentPositionOffset.z;//TileFollowDistanceOffset;
			m_FollowHeight += currentPositionOffset.y;//TileFollowHeightOffset;	=
		
		//-- Calc a new target Pitch
		m_TargetPitch = Mathf.Atan2(m_FollowHeight, m_FocusDistance) * 57.29578f;
		m_TargetDistance = (new Vector2(m_FollowDistance,m_FollowHeight)).magnitude;
		
		//m_TargetDistance = Mathf.Sqrt((m_FollowDistance*m_FollowDistance)+(m_FollowHeight*m_FollowHeight));
	}
	
	private bool stopped = false;
	private float internal_vel = 0f;
	public void Stop()
	{
		stopped = true;
		internal_vel = PlayerTarget.GetPlayerVelocity().magnitude;
	}
	
	public void Unstop()
	{
		StartCoroutine(Unstop_internal());
	}
	IEnumerator Unstop_internal()
	{
		yield return new WaitForSeconds(0.1f);
		stopped = false;
	}
	
	private bool holding = false;
	public bool Holding { get { return holding; } }
	
	private float holdT = 0f;
	//private Vector3 startoffset = Vector3.zero;
	private Vector3 startpos = Vector3.zero;
	private bool isset = false;
	
	//private Vector3 initCamLook = Vector3.zero;
	// do updating in LateUpdate to make sure all other objects have moved around.
	public new void  LateUpdate()
	{
		SharedInstance = this;
		
		
		if(PlayerTarget == null || cameraState != CameraState.gamplay)
			return;
		
		if(stopped)	//A hcak to make the camera slow down
		{
			internal_vel = Mathf.MoveTowards(internal_vel,0f,35f*Time.deltaTime);
			Vector3 frw = transform.forward;
			frw.y = 0f;
			frw.Normalize();
			transform.position += frw * internal_vel*Time.deltaTime;
		}
		
		else if (PlayerTarget.IsDead == false) 
		{
			
			if(PlayerTarget.OnTrackPiece==null || PlayerTarget.OnTrackPiece.CurrentTrackPieceData==null || PlayerTarget.OnTrackPiece.CurrentTrackPieceData.splineStart==null)
			{
				// eyal since we timeScale to 0 on pause the camera doesn't update which results in camera pointing sideways on character after resurrect.			
				float dt = Time.deltaTime;
				if(dt == 0f){
					dt = 0.02f;
				}
	
				
				//-- Move the FocusTarget to the player
				setFocusTargetPosition();
				
				m_CurrentDistance = Mathf.Lerp (m_CurrentDistance, m_TargetDistance, dt * SmoothZoomSpeed);
				
				//-- Calculate offset vector and a target Yaw
				m_TargetCameraLocation.x = m_TargetCameraLocation.y = 0.0f;
	    		m_TargetCameraLocation.z = -m_CurrentDistance;
			
			    if(PlayerTarget.Hold == false)
				{
					m_TargetYaw = SignedAngle(m_TargetCameraLocation.normalized, -PlayerTarget.transform.forward, Vector3.up);
		            
		        	//-- Clamp targetYaw to -180, 180
		        	m_TargetYaw = Mathf.Repeat(m_TargetYaw + 180f, 360f) - 180f;
		
		            //-- Clamp smooth currentYaw to targetYaw and clamp it to -180, 180
		            m_CurrentYaw = Mathf.LerpAngle(m_CurrentYaw, m_TargetYaw, dt * SmoothRotationSpeed);
		            m_CurrentYaw = Mathf.Repeat(m_CurrentYaw + 180f, 360f) - 180f;	
				}
	
	            //-- Smooth pitch
	            m_CurrentPitch = Mathf.LerpAngle(m_CurrentPitch, m_TargetPitch, dt * SmoothPitchSpeed);
				
				// Rotate offset vector
	        	m_TargetCameraLocation = Quaternion.Euler(m_CurrentPitch, m_CurrentYaw, 0f) * m_TargetCameraLocation;
	       
			//	float distFromStartOfPiece = 0;
			//	if(GamePlayer.SharedInstance.OnTrackPiece!=null)
			//		distFromStartOfPiece = (GamePlayer.SharedInstance.CurrentPosition-GamePlayer.SharedInstance.OnTrackPiece.GeneratedPath[0]).magnitude;
	        	
				// Position camera holder correctly
				holding = false;
				if((/*PlayerTarget.Hold == true ||*/ (/*GameController.SharedInstance.DistanceTraveled < 50f && GamePlayer.SharedInstance.transform.position.z>-25f && */GameController.SharedInstance.TimeSinceGameStart < 5f)))  // (FocusTarget.position.z > -12.0f && GameController.SharedInstance.TimeSinceGameStart < 3.5f))
				{
					//transform.position = EndLocation;
					if(GameController.SharedInstance.TimeSinceGameStart > Mathf.Epsilon)
					{
						bool reset = holdT<=0f && isset;
						
						float lerpspeed = GamePlayer.SharedInstance.HasBoost ? 0.35f : 0.21f;
						holdT = Mathf.MoveTowards(holdT,1f,Time.deltaTime*lerpspeed);
						
						float cosholdT = -Mathf.Cos((holdT)*Mathf.PI)/2f + 0.5f;
						
						//Vector3 outoffset = new Vector3(11f,7f,-20f);
						Vector3 outoffsetA = new Vector3(3f,3f,-13f);
						Vector3 outoffsetB = new Vector3(29f,9f,-9f);
						Vector3 outoffset = Vector3.Lerp(outoffsetA,outoffsetB,holdT*holdT);
						
						Vector3 outpos = startpos + outoffset;
						Vector3 endpos = FocusTarget.position + m_TargetCameraLocation;
						
						Vector3 inttarget = Vector3.Lerp(outpos,endpos,cosholdT);
						
						transform.position = Vector3.Lerp(startpos,inttarget,cosholdT);
						
						transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(FocusTarget.position-transform.position,Vector3.up),(holdT*holdT/1.35f )* Time.deltaTime*60f);
					//	transform.LookAt(FocusTarget.position);
						
						if(reset)
						{
							transform.LookAt(FocusTarget.position);
						}
						isset=true;
						
			/*			holdT = Mathf.MoveTowards(holdT,1f,Time.deltaTime*0.05f);
						
						float holdTsqr = holdT * holdT;
						
						float offlerp = Mathf.Clamp01(1-holdTsqr);
						
						Vector3 endoffset = m_TargetCameraLocation+Vector3.right*offlerp*10f;
						Vector3 curoffset = transform.position - FocusTarget.position;
						
						Vector3 intoffset = Vector3.Slerp(curoffset,endoffset,Time.deltaTime*0.25f/(1f-holdTsqr));
					//	intoffset = Vector3.MoveTowards(intoffset,FocusTarget.position,Time.deltaTime*5f);
						
						transform.position = FocusTarget.position + intoffset;
						*/
						
				//		holdT = Mathf.MoveTowards(holdT,1f,Time.deltaTime/50f);
					
				//	Debug.Log(holdT);
					
						
				//		transform.position = Vector3.Lerp(transform.position, FocusTarget.position+m_TargetCameraLocation, holdT);

						//TempMoveNumber = Vector3.Lerp(new Vector3 (0f,0f,17.5f),FocusTarget.position, GameController.SharedInstance.TimeSinceGameStart / 2.5f);
						
					//	transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(FocusTarget.position-transform.position,Vector3.up),holdTsqr);
						//transform.LookAt (TempMoveNumber);
						
					}
					else if(!isset)
					{
					//	startoffset = transform.position - FocusTarget.position;
						startpos = transform.position;
					}
					holding = true;

		        	// And then have the camera look at our target
		        	
				}
				/*else if(GamePlayer.SharedInstance.OnTrackPiece.TrackType==TrackPiece.PieceType.kTPBalloonEntrance)
				{
					if(distFromStartOfPiece > 8f && distFromStartOfPiece<=18f)
						transform.position = transform.position;
					else if(distFromStartOfPiece>18f)
						transform.position = GamePlayer.SharedInstance.OnTrackPiece.GeneratedPath[GamePlayer.SharedInstance.OnTrackPiece.GeneratedPath.Count-1] + m_TargetCameraLocation;
					else {
						transform.position = FocusTarget.position + m_TargetCameraLocation;
		        		transform.LookAt(FocusTarget.position);
					}
					//transform.rotation = Quaternion.LookRotation(-GamePlayer.SharedInstance.OnTrackPiece.transform.forward);
				}*/
				else
				{
					transform.position = FocusTarget.position + m_TargetCameraLocation;
					//transform.position = FocusTarget.TransformPoint(m_TargetCameraLocation);

		        	// And then have the camera look at our target
		        	transform.LookAt(FocusTarget.position);
				}
				
				if(holding == true)
				{
				//	m_CurrentDistance = Vector3.Distance (FocusTarget.position, transform.position);
				//	m_CurrentYaw = transform.rotation.eulerAngles.y;
				}
				else if (GameController.SharedInstance.TimeSinceGameStart > Mathf.Epsilon)
				{
					holdT = 0f;
				}
			}
			else
			{
				SplineNode start = PlayerTarget.OnTrackPiece.CurrentTrackPieceData.splineStart;
				List<Transform> pathlocations = PlayerTarget.OnTrackPiece.CurrentTrackPieceData.PathLocations;
				float distToLast = (pathlocations[0].position - PlayerTarget.transform.position).magnitude;
				float distToNext = (pathlocations[pathlocations.Count-1].position - PlayerTarget.transform.position).magnitude;
				float percent = distToLast/(distToLast+distToNext);
				
				int count = 0;
				SplineNode cur = start;
				while(cur.next!=null)
				{
					cur = cur.next;
					count++;
				}
				
				percent *= (float)count;
				
				SplineNode useNode = start;
				while(percent>=1f)
				{
					useNode = useNode.next;
					percent-=1f;
				}
				
				if(percent<1f)
				{
					transform.position = useNode.Bezier(percent);
				}

	        	// And then have the camera look at our target
	        	transform.LookAt(PlayerTarget.transform.position);
			}
			
			if (IsCameraShaking) {
				TimeSinceCameraShakeStart -= Time.smoothDeltaTime;
				CameraShakeMagnitude -= (Time.smoothDeltaTime * CameraShakeDamperRate);
				if (CameraShakeMagnitude <= 0.0f || TimeSinceCameraShakeStart > CameraShakeDuration)
					IsCameraShaking = false;
				else {
					float cameraShakeOffsetX = Mathf.Sin(TimeSinceCameraShakeStart * 35.0f * CameraShakeFrequencyMultiplier) * CameraShakeMagnitude;
					float cameraShakeOffsetY = Mathf.Sin(TimeSinceCameraShakeStart * 50.0f * CameraShakeFrequencyMultiplier) * CameraShakeMagnitude;
					transform.Translate(cameraShakeOffsetX, cameraShakeOffsetY, 0);
			//		Debug.Log(transform.position);
				}
			}
		}
		
		CachedPosition = transform.position;
	}
	
	
	//private Vector3 currentDirectionOverride = Vector3.zero;
	private Vector3 currentPositionOffset = Vector3.zero;
	
	private void UpdatePosDirOffsets()
	{
		//NOTE: We have a mysterious crash in this function, so there is  a lot of error checking... Hopefully some of the Debug.Logs catch something
		if(GamePlayer.SharedInstance==null || GamePlayer.SharedInstance.OnTrackPiece==null)
			return;
		GamePlayer player = GamePlayer.SharedInstance;
		TrackPieceData data = player.OnTrackPiece.CurrentTrackPieceData;
		
		//Find the two closest points on the track
		int splineIndex = 0;
		float minmag = 99999f;
		float nextmag = 99998f;
		int minind = 0;
		int nextind = 0;
		
		//NOTE: keep an eye on this, it was crashing on rare instances so I put in a null check...
		if(data==null || data.PathLocations==null || player.CachedTransform==null)	return;
		
		for(int i=0;i<data.PathLocations.Count;i++)
		{
			float newmag = (data.PathLocations[i].position-player.CachedTransform.position).sqrMagnitude;
			if(newmag<minmag)
			{
				nextmag = minmag;
				minmag = newmag;
				nextind = minind;
				minind = i;
			}
			else if(newmag<nextmag)
			{
				nextmag = newmag;
				nextind = i;
			}
		}
		if(minind<nextind)	splineIndex = minind;
		else 				splineIndex = minind-1;
		
		float totaldist;
		float lerpval;
		if(splineIndex>=0 && splineIndex+1<data.PathLocations.Count 
			&& data.PathLocations[splineIndex]!=null && data.PathLocations[splineIndex+1]!=null)
		{
			totaldist = (data.PathLocations[splineIndex].position - data.PathLocations[splineIndex+1].position).magnitude;
			if(totaldist>Mathf.Epsilon)
			{
				lerpval = (player.CachedTransform.position-data.PathLocations[splineIndex].position).magnitude/totaldist;
			}
			else
			{
				lerpval = 1f;
				notify.Error("Problem in OzGameCamera...");
			}
		}
		else
		{
			//CrittercismIOS.LeaveBreadcrumb("PROBLEM IN OZGAMECAMERA!!!! Get Bryant or Redmond. UpdatePosDirOffsets had a potential crash.");
			totaldist = 1f;
			lerpval = 1f;
			notify.Error("PROBLEM IN OZGAMECAMERA!!!! Get Bryant or Redmond. UpdatePosDirOffsets had a potential crash.");
		}
		
		Vector3 pos1 = (data.CameraPositionOffsets!=null && data.CameraPositionOffsets.Count>splineIndex) ? 
			data.CameraPositionOffsets[splineIndex] : Vector3.zero;
		Vector3 pos2 = (data.CameraPositionOffsets!=null && data.CameraPositionOffsets.Count>splineIndex+1) ?
			data.CameraPositionOffsets[splineIndex+1] : Vector3.zero;
		currentPositionOffset = Vector3.Lerp(pos1,pos2,lerpval);
		
		//Vector3 dir1 = data.CameraDirectionVectors.Count>splineIndex ? data.CameraDirectionVectors[splineIndex] : new Vector3(0f,-0.2f,-1f);
		//Vector3 dir2 = data.CameraDirectionVectors.Count>splineIndex+1 ? data.CameraDirectionVectors[splineIndex+1] : new Vector3(0f,-0.2f,-1f);
		//currentDirectionOverride = Vector3.Lerp(dir1,dir2,lerpval);
		
	}
	
	
	//Used to move the camera based on the track piece.
	//bool testToggle = false;
	//bool adjustCameraOffsetOnTile = false;
	float TileFocusHeightOffset;
	float TileFocusDistanceOffset;
	float TileFollowDistanceOffset;
	float TileFollowHeightOffset;
	
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(FocusTarget.position, 0.5f);
		Gizmos.DrawLine(transform.position, FocusTarget.position);
	}
	
	public void OnCinematicPullBackComplete(){
		GameController.SharedInstance.isPlayingCinematicPullback = false;
		cameraState = CameraState.cinematicOpening;
		GameController.SharedInstance.SetMainMenuAnimation();
	}
	
}
