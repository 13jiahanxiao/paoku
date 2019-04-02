using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BonusItem : MonoBehaviour 
{
	protected static Notify notify;
	public enum BonusItemType
	{
		None = -1,
		Coin = 0,
		CoinDouble = 1,
		CoinTriple = 2,
		Vacuum = 3,
		Boost = 4,
		Gem = 5,
		MegaCoin = 6,
		Poof = 7,
		Shield = 8,
		ScoreBonus = 9,
		TornadoToken = 10,
	}

	public BonusItemType 	BonusType = BonusItemType.None;
	public int 				Value = 0;
	public bool 			IsMagnetized = false;
	public bool				FaceCamera = false;
	public bool 			effectsPlaying = false;
	public Vector3 			RotationAxis = new Vector3(0,1,0);
//	private Vector3			AngularVelocityVector = new Vector3(0,1,0);
	public float 			AngularVelocity = 0.0f;
	public Transform		Shadow = null;
	public Transform		Coin = null;
//	private bool			_ShadowEnabled = true;
	
	static public 			LayerMask defaultLayerMask = -1;
	static public 			LayerMask uiLayerMask = -1;
	
	Renderer[] 				CachedRenderers = null;
	private Transform 		CachedTransform = null;
	//private Transform 		CachedTransform_Parent = null;

	public ParticleSystem defaultEffect;	
	public ParticleSystem thirdEyeEffect;
	

	static private int sCount;
//	static private Dictionary<string, GameObject> _prefabs = new Dictionary<string, GameObject>();	
	public Renderer powerUpRend;
//	public Renderer musicBoxRend;
	
	public List<Renderer> shuffleRenderers = new List<Renderer>();

	static SpawnPool sSpawnPool = null;
	
	void Awake() {	
		if (notify != null)
		{
			notify = new Notify(this.GetType().Name);	
		}
		CachedTransform = transform;
		//CachedTransform_Parent = transform.parent;
		
		if(CachedRenderers == null)
		{
			CachedRenderers = CachedTransform.GetComponentsInChildren<Renderer>();
		}
		
		if(BonusItem.defaultLayerMask == -1) {
			BonusItem.defaultLayerMask = LayerMask.NameToLayer("Coins");
		}
		if(BonusItem.uiLayerMask == -1) {
			BonusItem.uiLayerMask = LayerMask.NameToLayer("InGameUI");
		}
	}
	
	void OnEnable()
	{	
		if(!IsACoin())		
			StopEffects();	
			PlayEffects();			
			
		if(shuffleRenderers.Count!=0)
		{
			StartCoroutine(BonusShuffle());
		}
	}
	
	void OnDisable()
	{	
		if(!IsACoin())		
			StopEffects();	
	}		
		
	// Use this for initialization
	void Start () 
	{				
		//Add a kinematic rigidbody, otherwise the time it takes to rotate/move a lot of these objects is enormous (.1 -.2 ms)
//		if(rigidbody==null)
//			gameObject.AddComponent<Rigidbody>();
//		rigidbody.isKinematic = true;
		
		if(renderer!=null)
		{
			enabled = renderer.isVisible;
			if(collider!=null)	collider.enabled = renderer.isVisible;
		}
		//InvokeRepeating("AnimationUpdate", 1.0f, 0.033f);
	}
	
	private int shuffleProgress = 0;
	IEnumerator BonusShuffle()
	{
		if(bShowAll) {
			for(int i=0;i<shuffleRenderers.Count;i++)
			{
				shuffleRenderers[i].enabled = false;
				yield return null;
			}
			if(powerUpRend!=null)	powerUpRend.enabled = true;
			
			yield break;
		}
	
		
		while(enabled && shuffleRenderers.Count>0)
		{
			shuffleRenderers[shuffleProgress].enabled = false;
			shuffleProgress = (shuffleProgress+1)%shuffleRenderers.Count;
			shuffleRenderers[shuffleProgress].enabled = true;
			yield return new WaitForSeconds(0.15f);
		}	
	}
	
	//Only rotate when the object is visible, otherwise the time cost is very high.
	void OnBecameVisible()
	{
		enabled = true;
		if(collider!=null)	collider.enabled = true;	
	}
	
	void OnBecameInvisible()
	{
		enabled = false;
		if(collider!=null)	collider.enabled = false;			
	}
	
	private static bool bShowAll = false;
	public static void ShowAllPowerUps(bool show)
	{
		bShowAll = show;
	}
	
	/*public void ShowPowerUp(bool show)
	{
		if(musicBoxRend!=null && powerUpRend!=null)
		{
			musicBoxRend.enabled = !show;
			powerUpRend.enabled = show;
		}
	}*/
	
	
	public void AnimationUpdate() {
		if(enabled == false || GameController.SharedInstance == null || GameController.SharedInstance.IsPaused == true)
			return;
		
		
		if(HasBeenCollected == true) {
			if(AnimatingPostCollection == false)
				return;
		}
		
		
		if (AngularVelocity != 0 && IsACoin() == true)
		{

			//If its a coin, use renderObj so the collider doesnt move (more expensive)
			if(powerUpRend!=null && powerUpRend.enabled)
				powerUpRend.transform.RotateAround(Vector3.up, AngularVelocity * Mathf.Deg2Rad * Time.deltaTime);
		//	if(musicBoxRend!=null && musicBoxRend.enabled)
		//		musicBoxRend.transform.RotateAround(Vector3.up, AngularVelocity * Mathf.Deg2Rad * Time.deltaTime);
		//	else
		//		CachedTransform.RotateAround(Vector3.up, AngularVelocity * Mathf.Deg2Rad * Time.deltaTime);

			/* TODO see which one is faster 
			AngularVelocityVector = RotationAxis * AngularVelocity;
			
			Quaternion deltaRotation = Quaternion.Euler(AngularVelocityVector * Time.deltaTime);
        	CachedTransform.rotation = this.CachedTransform.rotation * deltaRotation;
        	*/

		}
		
		if(FaceCamera == true && Camera.mainCamera != null)
		{
			CachedTransform.LookAt(Camera.mainCamera.transform.position, Camera.mainCamera.transform.up);
		}
	}
	
	public void Update()
	{
		AnimationUpdate();
	}
	
	public void FixedUpdate () 
	{
		if(GameController.SharedInstance == null || GameController.SharedInstance.IsPaused == true)
			return;
		
//		if (AngularVelocity != 0 && IsACoin() == true)
//		{
//			AngularVelocityVector = RotationAxis * AngularVelocity;
//			
//			Quaternion deltaRotation = Quaternion.Euler(AngularVelocityVector * Time.deltaTime);
//        	CachedTransform.rotation = this.CachedTransform.rotation * deltaRotation; 
//		}
		
//		if(FaceCamera == true && Camera.mainCamera != null)
//		{
//			CachedTransform.LookAt(Camera.mainCamera.transform.position, Camera.mainCamera.transform.up);
//		}
		
		if(GamePlayer.SharedInstance.IsDead == true && AnimatingPostCollection == true) {
			AnimatingPostCollection = false;
			transform.parent = BonusItem.sSpawnPool.transform;
			transform.localScale = Vector3.one;
			transform.localRotation = Quaternion.identity;
			transform.position = Vector3.zero;
			if(Coin != null) {
				Coin.localScale = Vector3.one;
			}
			SetLayer(defaultLayerMask);
			SetVisibility(false);
			return;
		}
		
		if(GamePlayer.SharedInstance.IsDead == false && HasBeenCollected == false)
		{
			//GamePlayer.SharedInstance.transform.up;
		/*	float upOffset = 0.5f;
			
			if (GamePlayer.SharedInstance.IsOnMineCart) {
				upOffset = 0.85f;
			} else if (GamePlayer.SharedInstance.IsOnAZipline()) {
				upOffset = 0.25f;	

			}*/
			
			//NOTE: uses the center of the collider, because the balloon moves the offset of the collider
			Vector3 towardsPlayer = (GamePlayer.SharedInstance.CharacterModel.transform.position+Vector3.up/2f) - CachedTransform.position;
			//Vector3 towardsPlayer = (GamePlayer.SharedInstance.collider.bounds.center) - CachedTransform.position;


			float distanceSqr = Vector3.SqrMagnitude(towardsPlayer);
			
			if(IsACoin() && (GamePlayer.SharedInstance.HasBonusEffect(BonusItem.BonusItemType.Vacuum) == true || IsMagnetized == true))
			{
				if(distanceSqr <= BonusItemProtoData.SharedInstance.VacuumRangeRadius2 ) 
				{
					towardsPlayer.Normalize();
					//float towardsPlayerMag = ((1.0f / distanceSqr) * BonusItemProtoData.SharedInstance.VacuumStrength * Time.fixedDeltaTime ); 
					// eyal modify to increase the radius in which we suck in coins when magnet is on, similar to tr2
					float towardsPlayerMag = ((5.0f / distanceSqr) * BonusItemProtoData.SharedInstance.VacuumStrength * Time.fixedDeltaTime ); 
					float towardsPlayerMagMax = BonusItemProtoData.SharedInstance.VacuumCoinVelocityMax * Time.fixedDeltaTime;
					
					if (towardsPlayerMag > towardsPlayerMagMax) {
						towardsPlayerMag = towardsPlayerMagMax;	
					}
					
					towardsPlayer *= towardsPlayerMag;
					
					CachedTransform.Translate(towardsPlayer, Space.World);
					IsMagnetized = true;
				}
			}
			
					
			//-- If we are really close to the player, credit a coin and hide self
			float CollisionRadius2 = 0.5f;
			
			if (!IsACoin()) {
				// Pickups use larger radius
				CollisionRadius2 = 1.5f;	
			} else if (IsMagnetized) {
				//Dont expand the radius here, instead, we move the coin TOWARDs the player in the above code
		//		CollisionRadius2 = 2.5f;	
			} 
			
			if(GamePlayer.SharedInstance.IsOnBalloon){
				CollisionRadius2 *= 2.5f;
			}
			
			if(distanceSqr <= CollisionRadius2)
			{
				
				// fire off the bonus item pickup event
				if(onBonusItemPickupEvent!=null)	onBonusItemPickupEvent(BonusType);
				
				//-- Turn off renderering and make the object tree inactive.
				if(gameObject.renderer != null)
				{
					gameObject.renderer.enabled = false;
				}
				
				
				// Add the score for this coin
				if(!GameController.SharedInstance.IsTutorialMode)
					GamePlayer.SharedInstance.AddScore(Value);
				
				bool flyToLeftMeter = false;
				
				if(BonusType == BonusItemType.Coin)
				{
					//-- Collect single coin.
					if(GamePlayer.SharedInstance.CoinCountTotal==0) {
						ObjectivesDataUpdater.SetGenericStat(ObjectiveType.DistanceWithoutCoins,GameController.SharedInstance.DistanceTraveled);
					}
					GamePlayer.SharedInstance.AddCoinsToScore(1);
					//GamePlayer.SharedInstance.AddPointsToPowerMeter(GameProfile.SharedInstance.GetCoinMeterFillRate());
					//flyToLeftMeter = true;
					EmitCoin();
					GamePlayer.SharedInstance.StartCoroutine(IncreaseMeterInSeconds(0.75f));
				}
				else if(BonusType == BonusItemType.CoinDouble)
				{
					//-- Collect double coin.
					if(GamePlayer.SharedInstance.CoinCountTotal==0) {
						ObjectivesDataUpdater.SetGenericStat(ObjectiveType.DistanceWithoutCoins,GameController.SharedInstance.DistanceTraveled);
					}
					GamePlayer.SharedInstance.AddCoinsToScore(2);
					//GamePlayer.SharedInstance.AddPointsToPowerMeter(GameProfile.SharedInstance.GetCoinMeterFillRate());
					//flyToLeftMeter = true;
					EmitCoin();
					GamePlayer.SharedInstance.StartCoroutine(IncreaseMeterInSeconds(0.75f));
				}
				else if(BonusType == BonusItemType.CoinTriple)
				{
					//-- Collect triple coin.
					if(GamePlayer.SharedInstance.CoinCountTotal==0) {
						ObjectivesDataUpdater.SetGenericStat(ObjectiveType.DistanceWithoutCoins,GameController.SharedInstance.DistanceTraveled);
					}
					GamePlayer.SharedInstance.AddCoinsToScore(3);
					//GamePlayer.SharedInstance.AddPointsToPowerMeter(GameProfile.SharedInstance.GetCoinMeterFillRate());
					//flyToLeftMeter = true;
					EmitCoin();
					GamePlayer.SharedInstance.StartCoroutine(IncreaseMeterInSeconds(0.75f));
				} 
				else if (BonusType == BonusItemType.Boost)
				{	
					//-- Turn on boost if not already on.
					GamePlayer.SharedInstance.StartBoost(GameProfile.SharedInstance.GetBoostDistance());
					if(AudioManager.SharedInstance!=null)	AudioManager.SharedInstance.PlayFX(AudioManager.Effects.boostLoop);
					BonusButtons.main.OnBoostPickedUp(transform.position);
				}
				else if (BonusType == BonusItemType.Vacuum)
				{
					//-- Turn on vacuum if not already on.
					GamePlayer.SharedInstance.StartVacuum(GameProfile.SharedInstance.GetMagnetDuration());
					if(AudioManager.SharedInstance!=null)	AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_MagicMagnet_01);
					BonusButtons.main.OnMagnetPickedUp(transform.position);
				}
				else if (BonusType == BonusItemType.Gem)
				{
					//-- Collect a gem! (or more, if GetGemValue() returns an upgraded number)
					GamePlayer.SharedInstance.AddGemsToScore(GameProfile.SharedInstance.GetGemValue());
					//GamePlayer.SharedInstance.SpawnPlayerParticleEffect(PlayerParticleEffect.ObjectGrab,1.0f,true);
					GamePlayer.SharedInstance.playerFx.StartObjectGrab ();
					if(AudioManager.SharedInstance!=null)	AudioManager.SharedInstance.PlayFX(AudioManager.Effects.Gem);
					BonusButtons.main.OnGemPickedUp(transform.position);
			
					ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.CollectSpecialCurrency,GameProfile.SharedInstance.GetGemValue());
					
					//TODO: Make this an Emitter as well?
					flyToLeftMeter = true;
					
					if(!GamePlayer.SharedInstance.IsOnBalloon)
					{
					//Gem rate decreases with each gem collected
						if(EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.SetId == 1)
							TrackPiece.gemReductionValue += 650f;
						
						else if(EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.SetId == 2)
							TrackPiece.gemReductionValue += 500f;
						
						else
							TrackPiece.gemReductionValue += 575f;
					}
				}
				
				else if (BonusType == BonusItemType.MegaCoin)
				{
					//-- Collect a megacoins!
					int value = GameProfile.SharedInstance.GetMegaCoinValue();
					value *= Random.value < GameProfile.SharedInstance.GetMegaCoinBonusChance() ? 2 : 1;
					
					GamePlayer.SharedInstance.AddPointsToPowerMeter(value);
					GamePlayer.SharedInstance.AddCoinsToScore(value);
					//GamePlayer.SharedInstance.SpawnPlayerParticleEffect(PlayerParticleEffect.ObjectGrab,1.0f,true);
					GamePlayer.SharedInstance.playerFx.StartObjectGrab();
					if(AudioManager.SharedInstance!=null)	AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_MegaCoin_01);
					BonusButtons.main.OnMegaCoinPickedUp(transform.position);
				}
				else if(BonusType == BonusItemType.Poof)
				{
					//-- Turn on poof if not already on.
					GamePlayer.SharedInstance.StartPoof(GameProfile.SharedInstance.GetPoofDuration());
			//		if(AudioManager.SharedInstance!=null)	AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_Poof_activate);
					BonusButtons.main.OnPoofPickedUp(transform.position);
				}
				else if(BonusType == BonusItemType.ScoreBonus)
				{
					//-- Turn on poof if not already on.
					GamePlayer.SharedInstance.AddScore(GameProfile.SharedInstance.GetScoreBonusPickupValue());
					GamePlayer.SharedInstance.playerFx.StartScoreBonusEffects();
					UIManagerOz.SharedInstance.inGameVC.scoreUI.ScoreBonusEffects();
					AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_ScoreMultiplier_01);
					BonusButtons.main.OnScoreBonusPickedUp(transform.position);
				}
				else if(BonusType == BonusItemType.TornadoToken)
				{
					//-- Turn on poof if not already on.
					GameProfile.SharedInstance.Player.AddChanceToken();
					//GamePlayer.SharedInstance.SpawnPlayerParticleEffect(PlayerParticleEffect.ObjectGrab,1.0f,true);
					GamePlayer.SharedInstance.playerFx.StartObjectGrab();
					if(AudioManager.SharedInstance!=null)	AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_Gatcha_01);
					BonusButtons.main.OnTornadoTokenPickedUp(transform.position);
				}
				/* jonoble: Temporarily removing these analytics events because they may be causing a lag
				if (BonusType != BonusItemType.Coin 
					&& BonusType != BonusItemType.CoinDouble 
					&& BonusType != BonusItemType.CoinTriple)
				{
					AnalyticsInterface.LogGameAction(
						"run", 
						"item_received", 
						BonusType.ToString(), 
						GameController.SharedInstance.DistanceTraveled.ToString(),
						0
					);
					
					AnalyticsInterface.LogGameAction(
						"run", 
						"item_received", 
						BonusType.ToString(), 
						GameProfile.GetAreaCharacterString(),
						0
					);
				}
				*/
				
				GameController.SharedInstance.collectedBonusItemPerRun[(int)BonusType] += 1;
				if(BonusType!=BonusItemType.Coin && BonusType!=BonusItemType.CoinDouble && BonusType!=BonusItemType.CoinTriple)
					ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.CollectPowerups,1);

				HasBeenCollected = true;
				
				if(flyToLeftMeter == true) {
					// we need to keep this code since we use it for gem pickups
					
					Vector3 v = UIManagerOz.SharedInstance.MainGameCamera.WorldToViewportPoint(this.transform.position);
					transform.parent = UIManagerOz.SharedInstance.inGameVC.myCamera.transform;
					SetLayer(uiLayerMask);
					AnimatingPostCollection = true;

					SetVisibility(true);
					
					v = UIManagerOz.SharedInstance.inGameVC.myCamera.ViewportToWorldPoint(new Vector3(v.x,v.y,1f));
					transform.localScale = new Vector3(100,100,100);
					transform.position = new Vector3(v.x - 0.16f, v.y, 1);
					Transform b = UIManagerOz.SharedInstance.inGameVC.scoreUI.gemsprite;

					float time = 1.1f;
					ParticleSystem[] pss = transform.GetComponentsInChildren<ParticleSystem>() ;
					foreach(ParticleSystem ps in pss){
						ps.gameObject.active = false;
					}
					
					
					iTween.MoveTo(gameObject, iTween.Hash(
						"time", time,
						"position", b.position,
						"oncomplete", "OnGemArrived",
						"oncompletetarget", gameObject
						));
					

				}
				else{
					gameObject.SetActiveRecursively(false);
				}
				
			}
			
			if(!IsACoin()) {
				if(distanceSqr <= 10 && effectsPlaying) {
					StopEffects();
				}
			}
		}
	}
	
	private void OnGemArrived(){
		AnimatingPostCollection = false;
		transform.parent = BonusItem.sSpawnPool.transform;
		transform.localScale = Vector3.one;
		transform.localRotation = Quaternion.identity;
		transform.position = Vector3.zero;
		SetLayer(defaultLayerMask);
		SetVisibility(false);
	}
	
	
	private void EmitCoin(){
		//Vector3 wp = GamePlayer.SharedInstance.transform.position;
		Vector3 wp = GamePlayer.SharedInstance.CharacterModel.transform.position;
		if(GamePlayer.SharedInstance.IsOnBalloon){
			wp += Vector3.up * 13f;
		}
		Vector3 vp = OzGameCamera.SharedInstance.camera.WorldToViewportPoint(wp) + new Vector3(-0.03f,0.08f,0f);
		Vector3 lp = UIManagerOz.SharedInstance.inGameVC.myCamera.ViewportToWorldPoint(new Vector3(vp.x, vp.y, 0.5f) );
		
		Vector3 rot = new Vector3 (0,0,lp.x*30f);
		UIManagerOz.SharedInstance.inGameVC.fx_coin.startLifetime = 0.9f + lp.x/4f;
		
		UIManagerOz.SharedInstance.inGameVC.fx_coin.transform.position = new Vector3(lp.x,UIManagerOz.SharedInstance.inGameVC.fx_coin.transform.position.y, lp.z) ;
		UIManagerOz.SharedInstance.inGameVC.fx_coin.transform.eulerAngles = rot;
		UIManagerOz.SharedInstance.inGameVC.EmitCoin();
	}
	
	private IEnumerator IncreaseMeterInSeconds(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		
		HitLeftMeter();
	}
	
	// event for when bonus item is picked up
	public delegate void OnBonusItemPickupHandler(BonusItemType bonusItemType);
	private static event OnBonusItemPickupHandler onBonusItemPickupEvent = null;
	public static void RegisterForOnBonusItemPickup(OnBonusItemPickupHandler delg) { onBonusItemPickupEvent += delg; }
	public static void UnRegisterForOnBonusItemPickupEvent(OnBonusItemPickupHandler delg) { onBonusItemPickupEvent -= delg; }			
	
	
	
	
	private void HitLeftMeter(/*UITweener tween*/) {
	//	Debug.Log("Hit");
		//-- 1 pt for each type coin.  THIS IS BY DESIGN.  Do not change it to 1, 2, 3.
		if(BonusType == BonusItemType.Coin || BonusType == BonusItemType.CoinDouble || BonusType == BonusItemType.CoinTriple)
		{
			//-- Collect single coin.
			GamePlayer.SharedInstance.AddPointsToPowerMeter(GameProfile.SharedInstance.GetCoinMeterFillRate());
		}
		//tween.onFinished -= HitLeftMeter;
		
		AnimatingPostCollection = false;
		
	/*	transform.parent = BonusItem.sSpawnPool.transform;
		transform.localScale = Vector3.one;
		transform.localRotation = Quaternion.identity;
		transform.position = Vector3.zero;
		if(Coin != null) {
			Coin.localScale = Vector3.one;
		}
		SetLayer(defaultLayerMask);
		SetVisibility(false);*/
	}
	

	public bool IsACoin()
	{
		return (BonusType > BonusItemType.None && BonusType <= BonusItemType.CoinTriple);
	}
	
	public static void DeSpawnAll()
	{
		if(BonusItem.sSpawnPool == null) {
			BonusItem.sSpawnPool = PoolManager.Pools["BonusItems"];
		}
		
		if(BonusItem.sSpawnPool != null)
		{
			BonusItem.sSpawnPool.DespawnAll();
		}
	}
	
	public static BonusItem  Create(BonusItemType type)
	{
		//-- Get the BI pool
		if(BonusItem.sSpawnPool == null) {
			BonusItem.sSpawnPool = PoolManager.Pools["BonusItems"];
		}
		
		//-- We set the BI pool in the inspector to have preloaded items.
		if(BonusItem.sSpawnPool._perPrefabPoolOptions.Count <= (int)type || 
			BonusItem.sSpawnPool._perPrefabPoolOptions[(int)type].prefab == null) {
			notify.Warning("MISSING PRE LOADED Bonus Item Prefab for {0}", type);
			return null;
		}
		
		//-- Get a new game from the pool.
		GameObject go = BonusItem.sSpawnPool.Spawn(BonusItem.sSpawnPool._perPrefabPoolOptions[(int)type].prefab, true).gameObject;
		
		BonusItem bonusItem = go.GetComponent<BonusItem>();
		if(bonusItem == null) {
			notify.Warning ("BonusItem Prefab doesn't have a BonusItemScript {0}", go);
			BonusItem.sSpawnPool.Despawn(go.transform, BonusItem.sSpawnPool._perPrefabPoolOptions[(int)type]);
			return null;
		}
		bonusItem.Reset();
		//bonusItem.ShowPowerUp(bShowAll);
		
		//Set value, and angular velocity
		//and update StatTracker
		switch (type) 
		{
		case BonusItemType.Coin:
			bonusItem.Value = 5;
			bonusItem.AngularVelocity = -300.0f;
			StatTracker.CoinLv1Spawned();
			break;
		case BonusItemType.CoinDouble:
			bonusItem.Value = 10;
			bonusItem.AngularVelocity = -300.0f;
			StatTracker.CoinLv2Spawned();
			break;
		case BonusItemType.CoinTriple:
			bonusItem.Value = 15;
			bonusItem.AngularVelocity = -300.0f;
			StatTracker.CoinLv3Spawned();
			break;
		default:
			bonusItem.Value = 500;
			StatTracker.BonusItemSpawned(type);
			break;
		}
		//clear rotation for platforms that cannot afford it
		switch(GameController.SharedInstance.GetDeviceGeneration())
		{
		case GameController.DeviceGeneration.Unsupported:
		case GameController.DeviceGeneration.iPhone3GS:
		case GameController.DeviceGeneration.iPodTouch3:
		case GameController.DeviceGeneration.iPodTouch4:
		case GameController.DeviceGeneration.iPhone4:
			bonusItem.AngularVelocity = 0.0f;
			break;
		default:
			break;
		}
		//Update StatTracker
		bonusItem.enabled = true;
		//bonusItem.SetVisibility(true);
		bonusItem.BonusType = type;
		//bonusItem.transform.localScale = Vector3.one;		
		
		return bonusItem;
	}
	
	private void Reset()
	{
		BonusType = BonusItemType.None;
		Value = 0;
		IsMagnetized = false;
		RotationAxis = Vector3.up;
		AngularVelocity = 0.0f;
		CachedTransform.position = Vector3.zero;
		//CachedRenderers = null;
		HasBeenCollected = false;
//		_ShadowEnabled = true; 
		AnimatingPostCollection =false;
		StopEffects();
	}
	
	private void SetLayer(LayerMask mask) {
		int max = CachedRenderers.Length;
		gameObject.layer = mask;
		for(int i=0; i<max; i++) {
			CachedRenderers[i].gameObject.layer = mask;
		}
	}	
	
	public void PlayEffects() 
	{
		effectsPlaying = true;
		if(bShowAll && thirdEyeEffect && thirdEyeEffect.gameObject.active)
		{
			thirdEyeEffect.Play(true);
		}
		else if(defaultEffect && defaultEffect.gameObject.active)
		{		
			defaultEffect.Play(true);
		}
	}

	public void StopEffects() 
	{
		effectsPlaying = false;
		if(thirdEyeEffect && thirdEyeEffect.gameObject.active && thirdEyeEffect.isPlaying) {
			thirdEyeEffect.Stop(true);
			thirdEyeEffect.Clear (true);		
		}
		if(defaultEffect && defaultEffect.gameObject.active && defaultEffect.isPlaying) {
			defaultEffect.Stop(true);
			defaultEffect.Clear (true);	
		}
	}	
	
	public void setShadowEnabled(bool enable)
	{
		if(GamePlayer.SharedInstance.IsOnBalloon == true)
			enable = false;
		
		if(HasBeenCollected == true)
			enable = false;
		
		if(Shadow == null)
			return;
		
		if(Shadow!=null && Shadow.renderer!=null)
			Shadow.renderer.enabled = enable;
	//	_ShadowEnabled = enable;
	}
	
	public void setShadowLocalPosition(Vector3 position)
	{
		if(Shadow == null)
			return;
		Shadow.localPosition = position;
	}
	
//	private GameObject shadowGO = null;
	
	public void SetVisibility(bool visible)
	{				
		if(HasBeenCollected == true) {
			visible = AnimatingPostCollection;
//			_ShadowEnabled = !visible;
		}
		
		if(visible != gameObject.active)
			this.gameObject.SetActiveRecursively(visible);
		
		

		//Why do we need all this? We are setting ActiveRecursively anyway... commented out. -bc
		//if(CachedRenderers == null)
		//{
		//	CachedRenderers = transform.GetComponentsInChildren<Renderer>();
		//}

		//if(CachedRenderers != null && visible == true) {
		//	int max = CachedRenderers.Length;
		//	for (int i = 0; i < max; i++) {
		//		if(CachedRenderers[i] == null)
		//			continue;
		//		CachedRenderers[i].enabled = visible;
		//	}	
		//}
		
		//if(Shadow != null)
		//{
		//	if(_ShadowEnabled == true)
		//		Shadow.gameObject.SetActiveRecursively(visible);
		//	else
		//		Shadow.gameObject.SetActiveRecursively(false);
		//}
	}
	
	public void OnSpawned()
	{
		Reset();
	}
	
	public void OnDespawned()
	{
		//Reset();
	}

	public void DestroySelf()
	{			
		if (!IsACoin())
			StopEffects();		
		
		if(BonusItem.sSpawnPool == null) {
			BonusItem.sSpawnPool = PoolManager.Pools["BonusItems"];
		}
		
		if(BonusItem.sSpawnPool != null) {
			//CachedTransform_Parent = BonusItem.sSpawnPool.transform;
			BonusItem.sSpawnPool.Despawn(transform, BonusItem.sSpawnPool._perPrefabPoolOptions[(int)this.BonusType]);	
		}
	}
	
	private bool HasBeenCollected = false;
	private bool AnimatingPostCollection = false;
	
	public void Hide()
	{
		if(gameObject.renderer != null)
		{
			gameObject.renderer.enabled = false;
		}
		HasBeenCollected = true;
		this.StartCoroutine(HideSelf(this));		
	}
	
	private IEnumerator HideSelf(BonusItem bonusItem)
    {		
		yield return null;
        yield return new WaitForEndOfFrame();
		
		//-- Turn off renderering and make the object tree inactive.
		bonusItem.gameObject.SetActiveRecursively(false);
		yield return null;
	}
	
}




