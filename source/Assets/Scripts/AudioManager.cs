using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour 
{
	private static AudioManager sharedInstance = null;
	public static AudioManager SharedInstance
	{
		get
		{
			if(sharedInstance==null)
			{
				sharedInstance = (AudioManager)GameObject.FindObjectOfType(typeof(AudioManager));
				
				if(sharedInstance==null)
				{
					//GameObject go = new GameObject();
					//sharedInstance = go.AddComponent<AudioManager>();
					//notify.Warning("No AudioManager found in the scene! Add one or enable it.");
				}
			}
			return sharedInstance;
		}
	}
	protected static Notify notify;
	
//	private AudioSource MainMenuAS;
	private AudioSource Music;
	private AudioSource SoundEffects;
	private AudioSource Boost;
	private AudioSource Coins;
	private AudioSource StumbleProof;
	private AudioSource CharacterSound;
	private AudioSource AnimatedObjectSound;
		
	public AudioClip BaboonsFlyIn;
	public AudioClip LaunchMusic;
	public AudioClip MainMenuMusic;
	
	[System.NonSerialized]
	public AudioClip GameMusic;
	
	public AudioClip TunnelMusic;
	public AudioClip BalloonMusic;
	

	public GameController Controller;
	public GamePlayer Player;
	
	public AudioClip Swish;
	public AudioClip Recharged;
	public AudioClip ScoreBlast;
	public AudioClip AngelWings;
	
	//public AudioClip landing;
	//public AudioClip jumping;
	//public AudioClip sliding;
	//public AudioClip turnRight;
	//public AudioClip turnLeft;
	
	private float musicMultiplier = 0.85f;
	public bool DisableAudio = false;
	
	public bool SFXready = false;
	static public AudioManager Instance;

	float AllowScreamIn = 0;

	List<AudioClip> StartedThisFrame = new List<AudioClip>();

	float _MusicVolume = 0.8f;
	float _SoundVolume = 0.8f;

	public float MusicVolume
	{
		get
		{
			return _MusicVolume;
		}
		set
		{
			_MusicVolume = value;
			PlayerPrefs.SetFloat("MusicVolume", value);
			Music.volume = value;
		}
	}
	
	public float SoundVolume
	{
		get
		{
			return _SoundVolume;
		}
		set
		{
			_SoundVolume = value;
			PlayerPrefs.SetFloat("SoundVolume", value);
		
			SoundEffects.volume = value;
			Boost.volume = value;
			Coins.volume = value;
			StumbleProof.volume = value;
			CharacterSound.volume = value;
			NGUITools.soundVolume = value;
			AnimatedObjectSound.volume = value;

		}
	}
	
	
	public bool IsMusicPlaying
	{
		get { return Music.isPlaying; }
	}
	
	
	//public AudioClip[] footstepsSfx;
	
	public enum Effects
	{
		//angelWings, //This needs to go
		//bonusMeterFull,
		boostLoop,

		//buttonClick,
		//cashRegister,
		coin,
		//cymbalCrash,

		
	//	gruntJump,
	//	gruntJumpLand,
	//	oz_slide_ww01,

	//	gruntTrip,
	//	monkeyRoar,
		monkeyDie,

		monkeys,
		//recharged,
		//scoreBlast,
	//	scream,
		
	//	oz_impact_rock01,
	//	oz_impact_wood01,
		
	//	oz_land_ww01,

		oz_MagicMagnet_01,
		oz_Poof_activate,
		oz_Poof_deactivate,
		oz_Apprentice_01,
		oz_GlindasBubble_01,
		oz_GlindasBubble_off,
		oz_MagicWand_01,
		oz_MegaCoin_01,
		oz_ScoreMultiplier_01,
		oz_TimeClock_01,
		oz_CountDown_01,
	//	oz_BoostStart,
		
		oz_StumbleProof_01,
		oz_StumbleProof_deactivate,
				
		oz_CrystalHit_01,
		oz_CrystalHit_02,
		oz_CrystalHit_03,
		
		oz_turnBasket_01,
		oz_turnBasket_02,
	//	oz_ClimbInBasket_01,
		oz_BalloonBasketImpact_01,
		oz_BalloonMotorLoop,
		
		oz_UI_CoinMeterFull,
		oz_UI_gemMeter,
		oz_UI_levelMultiplier,
		oz_UI_Menu,
		oz_UI_Menu_back,
		oz_UI_Menu_click,
		oz_UI_Menu_dialogBox,
		oz_UI_Menu_levelMax,
		oz_UI_Menu_levelUp,
		oz_UI_Menu_play,
		oz_UI_Menu_purchase,
		oz_UI_Menu_tab,
		//oz_UI_Menu_tab_01,
		oz_UI_meterFull01,
		oz_UI_meterFull02,
		oz_UI_meterFull03,
		oz_UI_scoreTally_fireworks_01,
		oz_UI_scoreTally_fireworks_02,
		oz_UI_scoreTally_fireworks_03,
		oz_firework_big,
		oz_firework1,
		oz_UI_scoreTallyBar,
				
		oz_Gatcha_01,
		oz_Gatcha_Deal,
		
		oz_Passfreind,
		
		oz_FinleyWings_01,
		oz_FinleyWings_02,
		oz_FinleyWings_03,
		oz_FinleyWings_04,
		
		oz_WandImpact_01,
		
		BossBaboon,
		
	//	oz_turnleft_ww01,
	//	oz_turnright_ww01,
		
		oz_Poof_loop,
		
		oz_UI_WeeklyChallenge_01,
		
		WhiteOut01,
		
		Lightning01,
		
		//shimmer,
		//slide,

		//splash,
		//splat,
		//swish,

		//wooosh,
		
		Gem,
		MusicBox,
		
	//	oz_cg_BigRockHit_01,
	//	oz_cg_TreeHit_01,
	//	oz_cg_FallingWail_01,
		
		MAX
	}

	Dictionary<Effects, AudioClip> Sounds = new Dictionary<Effects, AudioClip>();
	
	private AudioClip NextMusicClip;

	void Start () 
	{
		Instance = this;
		PreloadSounds();
		//EnvironmentSetSwitcher.SharedInstance.RegisterForOnEnvironmentStateChange(EnvironmentStateChanged);
	}
	
	void Awake()
	{
		AudioManager.sharedInstance = this;		
		notify = new Notify(this.GetType().Name);
		
#if !UNITY_EDITOR
		//We don't want this to happen in the player, just the editor.
		DisableAudio = false;
#endif
		
		//TODO: Don't expose these as public, add the audio sources in code only. -bc
	//	if (MainMenuAS==null) MainMenuAS = gameObject.AddComponent<AudioSource>();
		if (Music==null) Music = gameObject.AddComponent<AudioSource>();
		if (SoundEffects==null)	SoundEffects = gameObject.AddComponent<AudioSource>();
		//if (FootSteps==null) FootSteps = gameObject.AddComponent<AudioSource>();
		if (Boost==null) Boost = gameObject.AddComponent<AudioSource>();
		if (Coins==null) Coins = gameObject.AddComponent<AudioSource>();
		//if (Magnet==null) Magnet = gameObject.AddComponent<AudioSource>();
		if (StumbleProof==null) StumbleProof = gameObject.AddComponent<AudioSource>();
		if (CharacterSound==null) CharacterSound = gameObject.AddComponent<AudioSource>();
		if (AnimatedObjectSound==null) AnimatedObjectSound = gameObject.AddComponent<AudioSource>();
		
		if (Controller==null) Controller = GameController.SharedInstance;
		if (Player==null) Player = GamePlayer.SharedInstance;
		
		SoundVolume = PlayerPrefs.GetFloat("SoundVolume", 0.8f);
		MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
		
	//	MainMenuAS.loop = true;
		Music.loop = true;
		
	}

	void PreloadSounds()
	{
		notify.Debug("PreloadSounds called!");
		
		for (int i = 0; i < (int)Effects.MAX; ++i) 
		{
			Effects fx = (Effects)i;
			string fileName = "Oz/TROzSound/" + fx.ToString();
			AudioClip ac = Resources.Load(fileName) as AudioClip;
			if (ac == null) 
			{
				notify.Warning("Could not load sound: " + fileName);
				//Debug.Log("Could not load sound: " + fileName);
			//	Debug.Break();
			}
			Sounds.Add(fx, ac);
		}
		
		StumbleProof.clip = Sounds[Effects.oz_StumbleProof_01];
		StumbleProof.loop = true;
		Coins.clip = Sounds[Effects.coin];
		Boost.clip = Sounds[Effects.boostLoop];
		
	}
	
	public void PlayBaboonsFlyIn()
	{
		SoundEffects.clip = BaboonsFlyIn;
		SoundEffects.Play();
	}
	
	
	
	
	public void StartLaunchMusic(){
		CharacterSound.clip = LaunchMusic;
		CharacterSound.Play();
		notify.Debug("StartLaunchMusic " + CharacterSound.clip + " " + CharacterSound.volume);
	}
	
	public void StopCharacterSound()
	{
		CharacterSound.Stop();
	}
	
	
	
	
	
	
	
	/*public void StartMainMenuMusicOnStart(){
		StartMainMenuMusic();
	}
	public void AdjustMainMenuMusic(){
		MainMenuAS.volume = _MusicVolume * mainMenuVolumeMult;
	}
	public void StartMainMenuMusic(float volumeMult = 0.6f)
	{
		//mainMenuVolumeMult = volumeMult;
		Music.Stop();
		MainMenuAS.clip = MainMenuMusic;
		MainMenuAS.loop = true;
		MainMenuAS.volume = _MusicVolume * mainMenuVolumeMult;
		if(!MainMenuAS.isPlaying)
			MainMenuAS.Play();
	}
	public void StopMainMenuMusic(){
		MainMenuAS.Stop();
	}
	
	public void SetDefaultMusic(){
		Music.clip = GameMusic;
	}
	public void StartGameMusic()
	{
		StopMainMenuMusic();
		EnvironmentSetSwitcher.SharedInstance.nextMusicClip = null; // clear since we didn't use autoswitch music
		//Resources.UnloadAsset(MainMenuMusic); // unload the main menu music since we didn't clear it on autoswitch
		//notify.Debug ("StartGameMusic " + EnvironmentSetManager.SharedInstance.LocalDict[MusicFile]);
		//Music.Stop(); 
		//Music.clip = GameMusic;
		
		Music.volume = _MusicVolume ;
		if(!Music.isPlaying)
			Music.Play();
	}*/

/*	public void StopMusic()
	{
		Music.Stop();
	}*/
	
	private float internal_music_multiplier = 1f;	//Used for fades
	
	public bool IsGameMusicPlaying()
	{
		return IsMusicPlaying && Music.clip==GameMusic;
	}
	
	public bool IsMenuMusicPlaying()
	{
		return IsMusicPlaying && Music.clip==MainMenuMusic;
	}
	
	public void SwitchToMainMenuMusic(float time = 0.5f)
	{
		SwitchMusic(MainMenuMusic,time);
	}
	
	public void SwitchToGameMusic(float time = 0.5f)
	{
		SwitchMusic(GameMusic,time);
	}
	
	public void SwitchMusic(AudioClip nextClip, float time = 0.5f){
		if(nextClip==Music.clip) return;
		
	//	Debug.Log("SwitchMusic " + nextClip.name );
		NextMusicClip = nextClip;
		
		float fadeInTime = time;
		if(Music.volume<=0f)	time = 0f;
		
		FadeOutMusic(time,0.01f,fadeInTime);
	}
	
	private void FadeInMusic(float time = 0.5f){
		//notify.Debug ("FadeInMusic");
		if(Music!=null)
		{
			iTween.ValueTo(gameObject, iTween.Hash("time",time,
													"from", internal_music_multiplier,
													"to", 1f ,
													"ignoretimescale", true,
													"onupdate", "ChangeMusicVolume"));
		}
	}
	
	private void FadeOutMusic(float time = 0.5f, float to = 0f, float fadeInTime = 0.5f){
		//notify.Debug ("FadeOutMusic");
		to = Mathf.Min(to, _MusicVolume);
		if(Music!=null)
		{
			iTween.ValueTo(gameObject, iTween.Hash("time",time,
													"from", internal_music_multiplier,
													"to", to,
													"ignoretimescale", true,
													"onupdate", "ChangeMusicVolume",
													"oncomplete", "OnFadeOutComplete",
													"oncompleteparams",fadeInTime));
		}
	}
	
	public void FadeMusicMultiplier(float time = 0.5f, float to = 0.5f)
	{
		//if(Music.clip!=null)
		//	Debug.Log("Fade Music: "+Music.clip.name+" "+time + " " + to);
		if(Music!=null)
		{
			iTween.ValueTo(gameObject, iTween.Hash("time",time,
													"from", musicMultiplier,
													"to", to,
													"ignoretimescale", true,
													"onupdate", "SetMusicMultiplier"));
		}
	}
	
	private void SetMusicMultiplier(float to)
	{
		musicMultiplier = to;
		UpdateMusicVolume();
		if(to<=0f) {
			Music.Stop();
		}
		else if(!Music.isPlaying)
			Music.Play();
	}
	
	
	private void ChangeMusicVolume(float volume){
		internal_music_multiplier = volume;
		UpdateMusicVolume();
		if(volume<=0f) {
			Music.Stop();
		}
		else if(!Music.isPlaying)
			Music.Play();
	}
	
	
	private void OnFadeOutComplete(float time = 0.5f){
		//notify.Debug ("OnFadeOutComplete");
		if(musicMultiplier == 0f || internal_music_multiplier == 0f){
			Music.Stop();
			musicMultiplier = 1f;
		}
		if(NextMusicClip){
		//	AudioClip current = Music.clip;
			Music.clip = NextMusicClip;
			NextMusicClip = null;
			Music.Play();
			FadeInMusic(time);
		}
	}
	
	
	public void UpdateMusicVolume()
	{
		if(Music!=null)
		{
			if(!iPodOn)
				Music.volume = _MusicVolume * musicMultiplier * internal_music_multiplier;
			else
				Music.volume = 0f;
		}
	}
	
	
	private bool iPodOn = false;
	public void UpdateIpodMusicIsOn()
	{
		iPodOn = IpodMusic.IsIpodMusicPlaying();
		
		UpdateMusicVolume();
	}
	
	
	public void PlayClip(AudioClip ac, float volumeScale = 1f, float pitch = 1f)
	{
		if (ac!=null) 
		{
			float v = volumeScale * SoundVolume;
			
			SoundEffects.pitch = pitch;
			
			if(SoundEffects.enabled)
				SoundEffects.PlayOneShot(ac,v);
		}
		else
		{
			Debug.Log("problem!");
		}
	}
	

	public void PlayFX(Effects effect, float volumeScale = 1.0f, float pitch = 1.0f)
	{	
		notify.Debug("Effect: "+effect);
		
		if(!Sounds.ContainsKey(effect))
		{
			return;
		}

		AudioClip ac = Sounds[effect];
		if (ac) 
		{
			if (StartedThisFrame.Contains(ac)) { return; }

			StartedThisFrame.Add(ac);
			float v = volumeScale * SoundVolume;
			//notify.Debug("Sound: " + effect + " VS: " + volumeScale + "  SV: " + _SoundVolume + "  FV: " + v);
			
			SoundEffects.pitch = pitch;
			//notify.Debug("playing: "+ac.name + " " + SoundEffects.enabled);
			if(SoundEffects.enabled)
				SoundEffects.PlayOneShot(ac,v);
		}
		else{
	//		notify.Debug("NoBueno " + effect);	
		}
	}

	public void StopFX(bool stopSfx = true )
	{
	//	notify.Debug("StopFX Sounds");
		if(stopSfx){
			SoundEffects.Stop();
		}
		Boost.Stop();
		StumbleProof.Stop();
		AnimatedObjectSound.Stop();
		if(Enemy.main.audio!=null)
			Enemy.main.audio.Stop();
		
		if(GameController.SharedInstance.openingTile){// stop waterfall sfx
			AudioSource audio = GameController.SharedInstance.openingTile.transform.GetComponentInChildren<AudioSource>();
			if(audio)
				audio.Stop();
		}
	}	
	
	public void PlayAnimatedSound(AudioClip clip){
		//AnimatedObjectSound.clip = clip;
		
		if (AnimatedObjectSound.isPlaying)
			return;
		
		AnimatedObjectSound.PlayOneShot(clip);
	}
	float TimeSinceLastCoin = 0;
	int CoinSequenceID = 0;

	public void PlayCoin(bool up = true)
	{
		if (TimeSinceLastCoin > 0.5f) 
		{
			CoinSequenceID = 0;
		}
		else 
		{
			CoinSequenceID++;
		}

		TimeSinceLastCoin = 0;

		float p = ((float)(CoinSequenceID % 50) / 10.0f) + 0.5f;
		if(!up){
			p = ((float)(50 - CoinSequenceID % 50) / 10.0f) + 0.5f;
		}
		Coins.pitch = p;
		//Coins.Play();
		Coins.PlayOneShot(Coins.clip);
//		notify.Debug("COIN: " + Coins.pitch + "  Vol: " + Coins.volume);
	}

	void Update()
	{
		
#if UNITY_EDITOR
		if(DisableAudio)
		{
			foreach(AudioSource a in GetComponents<AudioSource>())
			{
				a.Stop();
			}
		}
#endif
		bool noSound = (Controller.IsPaused || Controller.IsGameOver || Player.Hold || Player.IsDead || Controller.IsInCountdown);

		if (GamePlayer.SharedInstance.BoostDistanceLeft < BonusItemProtoData.SharedInstance.BoostStartSlowDownDistance)
		{
			float boostVolume = 0.4f * ((GamePlayer.SharedInstance.BoostDistanceLeft / BonusItemProtoData.SharedInstance.BoostStartSlowDownDistance)) + 0.1f;
			//float alpha = BoostEffect.Instance.GetAlpha();
			Boost.volume = boostVolume * SoundVolume;
		}
		else 
		{ 
			Boost.volume = 0.65f * SoundVolume;
		}

		bool playBoost = ((GamePlayer.SharedInstance.HasBoost || GamePlayer.SharedInstance.IsMegaBoost) && !noSound);
		if (Boost.isPlaying != playBoost)
		{
			if (playBoost) 
			{
				Boost.Play();
			}
			else 
			{
				Boost.Stop();
			}
		}

		StartedThisFrame.Clear();
		if (AllowScreamIn > 0)
		{
			AllowScreamIn -= Time.deltaTime;
		}
		TimeSinceLastCoin += Time.deltaTime;
	}
	
	void OnDisable()
	{
		if (Music.clip == null){
			Music.Stop(); 
		}
	}

	public void PlayCrystalSound(){
		int rand = (int)(Random.value * 3f);
		switch(rand){
			case 0: PlayFX(Effects.oz_CrystalHit_01);
				break;
			case 1: PlayFX(Effects.oz_CrystalHit_02);
				break;
			case 2: PlayFX(Effects.oz_CrystalHit_03);
				break;
		}
		
	}
	
	public void PlayBasketSound(){
		int rand = (int)(Random.value * 2f);
		switch(rand){
			case 0: PlayFX(Effects.oz_turnBasket_01);
				break;
			case 1: PlayFX(Effects.oz_turnBasket_02);
				break;
		}
		
	}
	
	public void PlayCharacterSound(AudioClip ac){
		CharacterSound.pitch = 1f;
		CharacterSound.clip = ac;
		CharacterSound.Play();
	}

	public void PlayFootsteps(){
		//Debug.Log ("hard surface  " + GamePlayer.SharedInstance.OnTrackPiece.isHardSurface);
		/*int rand = (int)Mathf.Floor(Random.value * 3f);
		CharacterSound.pitch = 1f + 0.2f * (0.5f - Random.value);
		CharacterSound.clip = footstepsSfx[rand + 3 * GamePlayer.SharedInstance.OnTrackPiece.isHardSurface];
		CharacterSound.Play();*/
		
		CharacterSounds sounds = GamePlayer.SharedInstance.characterSounds;
		
		List<AudioClip> footsteps = null;
		switch(EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.SetId)
		{
		case 1:	footsteps = sounds.footsteps_ww;	break;
		case 2:	footsteps = sounds.footsteps_df;	break;
		case 3:	footsteps = sounds.footsteps_ybr;	break;
		case 4:	footsteps = sounds.footsteps_ec;	break;
		}
		
		int rand = Random.Range(0,footsteps.Count);
		
		CharacterSound.pitch = 1f + 0.2f * (0.5f - Random.value);
		CharacterSound.clip = footsteps[rand];
		CharacterSound.Play();
	}
	
	public void StartStumbleProof(){
		StumbleProof.clip = Sounds[Effects.oz_StumbleProof_01];
		StumbleProof.Play();
	}
	
	public void StopStumbleProof(){
		StumbleProof.Stop();
	}
	
	public void StartBalloonMotor(){
		StumbleProof.clip = Sounds[Effects.oz_BalloonMotorLoop];
		StumbleProof.Play();
		iTween.ValueTo(gameObject, iTween.Hash("time",1f,
												"from", 0f,
												"to", _SoundVolume,
												"onupdate", "ChangeMotorVolume"));
	}
	
	public void PlayMagnet()
	{
		StumbleProof.clip = Sounds[Effects.oz_MagicMagnet_01];
		StumbleProof.Play();
		iTween.ValueTo(gameObject, iTween.Hash("time",1f,
												"from", 0f,
												"to", _SoundVolume,
												"onupdate", "ChangeStumbleProofVolume"));
	}
	
	public void StopMagnet()
	{
		if(!Sounds.ContainsKey(Effects.oz_MagicMagnet_01))	return;
		
		if(StumbleProof.clip == Sounds[Effects.oz_MagicMagnet_01])
		{
			iTween.ValueTo(gameObject, iTween.Hash("time",1f,
												"from", _SoundVolume,
												"to", 0f,
												"onupdate", "ChangeStumbleProofVolume"));
		}
	}
	
	public void PlayPoof()
	{
		StumbleProof.clip = Sounds[Effects.oz_Poof_loop];
		StumbleProof.Play();
		iTween.ValueTo(gameObject, iTween.Hash("time",1f,
												"from", 0f,
												"to", _SoundVolume,
												"onupdate", "ChangeStumbleProofVolume"));
	}
	
	public void StopPoof()
	{
		if(!Sounds.ContainsKey(Effects.oz_Poof_loop))	return;
		
		if(StumbleProof.clip == Sounds[Effects.oz_Poof_loop])
		{
			iTween.ValueTo(gameObject, iTween.Hash("time",1f,
												"from", _SoundVolume,
												"to", 0f,
												"onupdate", "ChangeStumbleProofVolume"));
		}
	}
	
	public void UpdateBalloonMotorPitch(float pitch)
	{
		if(StumbleProof.isPlaying && StumbleProof.clip == Sounds[Effects.oz_BalloonMotorLoop])
			StumbleProof.pitch = pitch;
	}
	
	public void ChangeMotorVolume(float val)
	{
		StumbleProof.volume = val;
		if(_SoundVolume>Mathf.Epsilon)
			StumbleProof.pitch = val/_SoundVolume;
	}
	
	public void ChangeStumbleProofVolume(float val)
	{
		StumbleProof.volume = val;
	}
	
	void OnBalloonFadeOutComplete()
	{
		StumbleProof.Stop();
		StumbleProof.pitch = 1f;
	}
	
	public void StopBalloonMotor(){
		
		iTween.ValueTo(gameObject, iTween.Hash("time",1f,
												"from", StumbleProof.volume,
												"to", 0.0,
												"onupdate", "ChangeMotorVolume",
												"oncomplete", "OnBalloonFadeOutComplete"));
	}
	
	public AudioSource GetSoundEffectsPlayer()
	{
		return SoundEffects;
	}
	

}
