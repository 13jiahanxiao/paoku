using UnityEngine;
using System.Collections;

public class PCInput : GameInput 
{
	public static PCInput Instance;
	
	public float 	SimulatedTiltAmount = 0;
	public float 	SimulatedTiltRecovery = 100.0f;
	public float 	SimulatedTiltSpeed = 1.0f;
	
	// Use this for initialization
	public override void init (GameController gameController) 
	{
		//TR.LOG("PCInput: init");
		Instance = this;
		
#if (UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER || UNITY_FLASH)
		enabled = true;
		TheGameController = gameController;
#else
		enabled = false;
		JoystickMode = false;
#endif
	}
	
	// Update is called once per frame
	public override float update (bool isIntroScene) 
	{
		float xOffset = base.update(isIntroScene);
		
		/*
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
		*/
		
		
		if (Input.GetKeyDown(KeyCode.Space)) 
		{
			Debug.Break();
		}

		if (JoystickMode) 
		{
			if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) 
			{
				ShouldJump = true;
				TimeSinceShouldJump = 0;
			}

			if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) 
			{
				ShouldSlide = true;
				TimeSinceShouldSlide = 0;
			}

			if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) 
			{
				TrackPiece currentTrackPiece = GamePlayer.SharedInstance.OnTrackPiece;
				if(currentTrackPiece != null) {
					ShouldTurnRight = true;
					TimeSinceShouldTurn = 0.0f;
					if (TheGameController.Player.canIgnoreStumble() == false &&
						currentTrackPiece.CloseEnoughToTurn(GamePlayer.SharedInstance.CachedTransform.position) == false) {
						TheGameController.Player.Stumble(0.05f, true);
						if(TheGameController.Player.StumbleKillTimer > TheGameController.Player.DeathByMonkeyTime)
							xOffset = 1.0f;
					}	
				}
					
			
				if (JoystickPosition < JoystickPositionType.kRight)
					++JoystickPosition;
			}

			if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) 
			{
				TrackPiece currentTrackPiece = GamePlayer.SharedInstance.OnTrackPiece;
				if(currentTrackPiece != null) {
					ShouldTurnLeft = true;
					TimeSinceShouldTurn = 0.0f;
					if (TheGameController.Player.canIgnoreStumble() == false &&
						currentTrackPiece.CloseEnoughToTurn(GamePlayer.SharedInstance.CachedTransform.position) == false) {
						TheGameController.Player.Stumble(0.05f, true);
						if(TheGameController.Player.StumbleKillTimer > TheGameController.Player.DeathByMonkeyTime)
							xOffset = -1.0f;
					}
				}
			
				if (JoystickPosition > JoystickPositionType.kLeft)
					--JoystickPosition;
			}

			if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
			{
				JoystickPosition = JoystickPositionType.kMiddle;
			}
		}
		else 
		{
			if(!isIntroScene)
			{
				simulateTilt();
				if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) 
				{
					ShouldJump = true;
					TimeSinceShouldJump = 0;
				}
	
				if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) 
				{
					ShouldSlide = true;
					TimeSinceShouldSlide = 0;
				}
				
				if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) 
				{
					TrackPiece currentTrackPiece = GamePlayer.SharedInstance.OnTrackPiece;
					if(currentTrackPiece != null) {
						ShouldTurnRight = true;
						TimeSinceShouldTurn = 0;	
						if (TheGameController.Player.canIgnoreStumble() == false &&
							currentTrackPiece.CloseEnoughToTurn(GamePlayer.SharedInstance.CachedTransform.position) == false) {
							TheGameController.Player.Stumble(0.05f, true);
							if(TheGameController.Player.StumbleKillTimer > TheGameController.Player.DeathByMonkeyTime)
								xOffset = 1.0f;
						}	
					}
				}
			
				if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) 
				{
					TrackPiece currentTrackPiece = GamePlayer.SharedInstance.OnTrackPiece;
					if(currentTrackPiece != null) {
						ShouldTurnLeft = true;
						TimeSinceShouldTurn = 0;	
						if (TheGameController.Player.canIgnoreStumble() == false &&
							currentTrackPiece.CloseEnoughToTurn(GamePlayer.SharedInstance.CachedTransform.position) == false) {
							TheGameController.Player.Stumble(0.05f, true);
							if(TheGameController.Player.StumbleKillTimer > TheGameController.Player.DeathByMonkeyTime)
								xOffset = -1.0f;
						}	
					}
				}
				
				if (Input.GetKeyDown(KeyCode.P)) {
					GameController.SharedInstance.UsePower();
				}
			}
			if (Input.GetKeyDown(KeyCode.Keypad1)) {
				Time.timeScale = 1f;
				notify.Debug("timeScale " + Time.timeScale);
			}
			if (Input.GetKeyDown(KeyCode.Keypad2)) {
				Time.timeScale = 2f;
				notify.Debug("timeScale " + Time.timeScale);
			}
			if (Input.GetKeyDown(KeyCode.Keypad3)) {
				Time.timeScale = 3f;
				notify.Debug("timeScale " + Time.timeScale);
			}
			if (Input.GetKeyDown(KeyCode.E)) {
//			UIManagerOz.SharedInstance.inGameVC.scoreUI.SetGemCount(GamePlayer.SharedInstance.GemCountTotal);
				GamePlayer.SharedInstance.AddGemsToScore(3);
				//UIManagerOz.SharedInstance.inGameVC.ShowEnvChangeHud("Get ready to turn\ninto a new location");
				//UIManagerOz.SharedInstance.inGameVC.coinMeter.AnimateCoinMeter();
			}
			if (Input.GetKeyDown(KeyCode.C)) {
				GamePlayer.SharedInstance.AddCoinsInstantly(200);
				GamePlayer.SharedInstance.AddPointsToPowerMeter(200);
			}
			if (Input.GetKeyDown(KeyCode.F)) {
				int rand = (int)(Random.value * 15);
				string name = "";
				for(int i=0; i < rand; i++)
					name += "a";
				UIManagerOz.SharedInstance.ShowFriendScoreLabel(name);
			}
			//if (Input.GetKeyDown(KeyCode.S)) {
			//	UIManagerOz.SharedInstance.postGameVC.OnSpeedUpButton();
			//}
			if (Input.GetKeyDown(KeyCode.S)) {
				UIManagerOz.SharedInstance.inGameVC.SetPowerMeterMessage();
			}
			if (Input.GetKeyDown(KeyCode.H)) {
				UIManagerOz.SharedInstance.inGameVC.ShowEnvChangeHud("Get ready to turn\nto a new location");
			}
			if (Input.GetKeyDown(KeyCode.G)) {
				GameProfile.SharedInstance.Player.AddChanceToken();
				/*
				if(UIManagerOz.SharedInstance.gatchVC.visible){
					UIManagerOz.SharedInstance.gatchVC.disappear();
				}
				else{
					UIManagerOz.SharedInstance.gatchVC.appear();
				}
				*/
			}
			/*
			if (Input.GetKeyDown(KeyCode.U)) {
				notify.Debug ("fadeIn");
				AudioManager.SharedInstance.FadeInMusic();
			}
			if (Input.GetKeyDown(KeyCode.Y)) {
				notify.Debug ("fadeOut");
				AudioManager.SharedInstance.FadeOutMusic();
			}
			if (Input.GetKeyDown(KeyCode.T)) {
				notify.Debug ("SwitchMusic");
				AudioClip nextMusicClip = Resources.Load(EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.MusicFile) as AudioClip;
				if(nextMusicClip) {
					notify.Debug("We loaded a new Music Clip");
					AudioManager.SharedInstance.SwitchMusic(nextMusicClip);
				}
			}
			*/
			if (Input.GetKeyDown(KeyCode.L)) {
				notify.Debug ("cycle language");
				Localization.SharedInstance.CycleLanguages();
			}
#if UNITY_EDITOR //cheat codes
			if (Input.GetKeyDown(KeyCode.B)) {
				GamePlayer.SharedInstance.StartBoost(GameProfile.SharedInstance.GetBoostDistance());
			}
			if (Input.GetKeyDown(KeyCode.M)) {
				GamePlayer.SharedInstance.StartVacuum(GameProfile.SharedInstance.GetMagnetDuration());
			}
			if (Input.GetKeyDown(KeyCode.N)) {
				GamePlayer.SharedInstance.StartPoof(GameProfile.SharedInstance.GetPoofDuration());
				//GamePlayer.SharedInstance.ShieldDuration = GameProfile.SharedInstance.GetShieldDurationBoost();
				//GamePlayer.SharedInstance.StartShield();
			}
#endif
		}
		
		HandleBow();
		
		return xOffset;
	}
	
	private void HandleBow()
	{
		//TODO add logic to determine if we should allow the player to shoot
		if (Input.GetMouseButtonDown(0))
		{
			if (Attack.SharedInstance != null)
			{
				//Attack.SharedInstance.ShootArrow(Input.mousePosition);
			}
		}
	}

	// Keyboard simualtion of tilt
	private void simulateTilt()
	{
		bool tiltRecover = true;
		if (Input.GetKey(KeyCode.X) || (Input.GetKey(KeyCode.RightArrow) && TheGameController.Player.OnTrackPiece != null && TheGameController.Player.OnTrackPiece.IsAZipLine )) 
		{
			tiltRecover = false;
			SimulatedTiltAmount -= (SimulatedTiltSpeed * Time.deltaTime);
			if (SimulatedTiltAmount < -1)
				SimulatedTiltAmount = -1;
		}
		
		if (Input.GetKey(KeyCode.Z) || (Input.GetKey(KeyCode.LeftArrow) && TheGameController.Player.OnTrackPiece != null && TheGameController.Player.OnTrackPiece.IsAZipLine )) 
		{
			tiltRecover = false;
			SimulatedTiltAmount += (SimulatedTiltSpeed * Time.deltaTime);
			if (SimulatedTiltAmount > 1)
				SimulatedTiltAmount = 1;
		}
		
		if (tiltRecover) 
		{
			if (SimulatedTiltAmount < 0) 
			{
				SimulatedTiltAmount += (SimulatedTiltRecovery * Time.deltaTime);
				if (SimulatedTiltAmount > 0)
					SimulatedTiltAmount = 0;
			}
			if (SimulatedTiltAmount > 0) 
			{
				SimulatedTiltAmount -= (SimulatedTiltRecovery * Time.deltaTime);
				if (SimulatedTiltAmount < 0)
					SimulatedTiltAmount = 0;
			}
		}
	}

	public override float getAccelerometerForceX()
	{
		return SimulatedTiltAmount;
	}
}
