using UnityEngine;
using System.Collections;

public class TouchInput : GameInput
{
	public static TouchInput Instance;
	
	public override void init (GameController gameController) 
	{
		notify.Debug("TouchInput: init");
		Instance = this;
#if ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR)
		enabled = true;
		TheGameController = gameController;
#else
		enabled = false;
#endif
	}

	bool IsSwiping = false;
	Vector2 LastSwipePoint = Vector2.zero;
	int tapCount = 0;
	float doubleTapTimeout = 0;
	float ignoreTapTimeout = 0;

	void DoubleTap ()
	{
		GameController.SharedInstance.UsePower();
	}
	
	public override float update (bool isIntroScene) 
	{
		float xOffset = base.update(isIntroScene);
		
		if (doubleTapTimeout > 0)
		{
			doubleTapTimeout -= Time.deltaTime;
		}
			
		if (doubleTapTimeout <= 0) 
		{
			tapCount = 0;
			doubleTapTimeout = 0;
		}

		if (ignoreTapTimeout > 0)
		{
			ignoreTapTimeout -= Time.deltaTime;
		}
		
		return xOffset;
	}

	public void TouchEnded (Vector2 touchPoint)
	{
	}

	public void TouchBegan (Vector2 touchPoint)
	{

		//notify.Debug("TC: " + tapCount + " fc: " + Time.frameCount);

		if (enabled == false)
			return;
		
		if (GameCamera.SharedInstance.FlyMode == true) 
		{
			GameCamera.SharedInstance.StartMenuAniamtion(true);
		}

		if (!TheGameController.IsPaused && !TheGameController.IsIntroScene) 
		{
			//TODO add logic to determine if the player should be allowed to shoot an arror
			//Attack.SharedInstance.ShootArrow(GetTouchPosition());
			
			IsSwiping = true;
			LastSwipePoint = touchPoint;

			if (ignoreTapTimeout <= 0)
			{
				if (tapCount == 0)
				{
					doubleTapTimeout = 0.25f;
				}
					
				tapCount++;
				if (tapCount == 2)
				{
					DoubleTap ();
					tapCount = 0;
				}
				ignoreTapTimeout = 0.1f;
			}
		}
	}

	public void TouchMoved (Vector2 touchPoint)
	{
		// Unity touches are pixel not point based.  Need to normalize for the screen dpi to make touches
		// feel consistant across devices.
		float dpiScaleFactor = 1.0f;
		
		if (Screen.dpi != 0.0f) {
			dpiScaleFactor = (Screen.dpi / 160.0f);
		}
		
		float swipeDistanceThreshold = 25.0f * dpiScaleFactor;

		if (enabled == false)
			return;

		if (TheGameController.IsGameOver || TheGameController.IsPaused || TheGameController.IsIntroScene || Time.timeScale == 0f)
			return;

		if (!IsSwiping)
			return;

		float distance = Vector2.Distance (touchPoint, LastSwipePoint);

		if (distance < swipeDistanceThreshold)
			return;

		// Calculate the angle of the touchVector with repect to the Y axis
		Vector2 touchVector = touchPoint - LastSwipePoint;
		touchVector.Normalize ();

		float angle = (Mathf.Atan2 (1.0f, 0.0f) - Mathf.Atan2 (touchVector.y, touchVector.x)) * (180.0f / Mathf.PI);
		if (angle < 0)
		{
			angle += 360;
		}

		float angleOffset = getAccelerometerForceX () * -2.0f;
		
		if (angle > (320.0f + angleOffset) || angle <= (40.0f + angleOffset)) 
		{
			//TR.LOG("Jump Command - Distance:{0} SwipeDistanceThreshold:{1} Angle:{2} AngleOffset:{3}", distance, swipeDistanceThreshold, angle, angleOffset);
			ShouldJump = true;
			TimeSinceShouldJump = 0;
			IsSwiping = false;
			tapCount = 0;
		} 
		else if (angle > (140.0f + angleOffset) && angle <= (220.0f + angleOffset)) 
		{
			//TR.LOG("Slide Command - Distance:{0} SwipeDistanceThreshold:{1} Angle:{2} AngleOffset:{3}", distance, swipeDistanceThreshold, angle, angleOffset);
			ShouldSlide = true;
			TimeSinceShouldSlide = 0;
			IsSwiping = false;
			tapCount = 0;
		} 
		else if (angle > (40.0f + angleOffset) && angle <= (140.0f + angleOffset)) 
		{
			TrackPiece currentTrackPiece = GamePlayer.SharedInstance.OnTrackPiece;
			if(currentTrackPiece != null) 
			{
				//if(!currentTrackPiece.IsTransitionTunnel() && !currentTrackPiece.IsTutorialPiece)
				//{
				
					bool closeEnoughToTurn = currentTrackPiece.CloseEnoughToTurn(GamePlayer.SharedInstance.CachedTransform.position);
					
					if (closeEnoughToTurn || (!closeEnoughToTurn && distance > (swipeDistanceThreshold * 2.0f))) {
						//TR.LOG("Turn Right - Distance:{0} SwipeDistanceThreshold:{1} Angle:{2} AngleOffset:{3} CloseToTurn:{4}", distance, swipeDistanceThreshold, angle, angleOffset, closeEnoughToTurn);
						ShouldTurnRight = true;
						TimeSinceShouldTurn = 0;	
						
						if (!GamePlayer.SharedInstance.canIgnoreStumble() && !closeEnoughToTurn) {
							GamePlayer.SharedInstance.Stumble(0.10f, true);
					//		GamePlayer.SharedInstance.PlayerXOffset = 1.0f;
						}
						
						IsSwiping = false;
						tapCount = 0;
					}
				//}
			}
		} 
		else if (angle > (220.0f + angleOffset) && angle <= (320.0f + angleOffset)) 
		{
			TrackPiece currentTrackPiece = GamePlayer.SharedInstance.OnTrackPiece;
			if(currentTrackPiece != null) 
			{
				//if(!currentTrackPiece.IsTransitionTunnel() && !currentTrackPiece.IsTutorialPiece)
				//{
					bool closeEnoughToTurn = currentTrackPiece.CloseEnoughToTurn(GamePlayer.SharedInstance.CachedTransform.position);
					
					if (closeEnoughToTurn || (!closeEnoughToTurn && distance > (swipeDistanceThreshold * 2.0f))) {
						//TR.LOG("Turn Left - Distance:{0} SwipeDistanceThreshold:{1} Angle:{2} AngleOffset:{3} CloseToTurn:{4}", distance, swipeDistanceThreshold, angle, angleOffset, closeEnoughToTurn);
						ShouldTurnLeft = true;
						TimeSinceShouldTurn = 0;	
						
						if (!GamePlayer.SharedInstance.canIgnoreStumble() && !closeEnoughToTurn) {
							GamePlayer.SharedInstance.Stumble(0.1f, true);
					//		GamePlayer.SharedInstance.PlayerXOffset = -1.0f;
						}
						
						IsSwiping = false;
						tapCount = 0;
					}
				//}
			}
		}
	}

	public Vector3 accelerometerNormalized = Vector3.zero;

	
	public override float getAccelerometerForceX()
	{
		accelerometerNormalized = Input.acceleration.normalized;
		#if (UNITY_IOS)
		return -accelerometerNormalized.x;
		#elif (UNITY_ANDROID)
		return accelerometerNormalized.y;
		#else 
		return 0;
		#endif
	}
	
	public Vector2 GetTouchPosition()
	{
		Vector2 position = Vector2.zero;

		if (Input.touchCount > 0)
		{
			/*#pragma warning disable 219
			foreach (Touch touch in Input.touches[])
				position += touch.position;
			#pragma warning restore 219
			
			position /= (float)Input.touchCount;*/
			
			position = Input.touches[0].position;
		}

		return position;
	}


}


