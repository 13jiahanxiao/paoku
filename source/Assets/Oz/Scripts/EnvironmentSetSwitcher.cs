	using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Main class responsible for switching the environment set
// interacts with TrackBuilder
public class EnvironmentSetSwitcher : MonoBehaviour 
{
	protected static Notify notify;
	public static EnvironmentSetSwitcher SharedInstance = null;
	
	private int destinationEnvironmentSetId; // which env set are we going to
	private int originEnvironmentSetId; // which env set are we coming from
	
	private float startDeleteTime;
	private float startWaitingToExitTunnelTime;

	private Dictionary<SwitchState, float> timeSpentPerState = new Dictionary<SwitchState, float>();
	
	// event for environment state change
	public delegate void OnEnvironmentStateChangeHandler(SwitchState newState, int destinationEnvironmentId);
	private static event OnEnvironmentStateChangeHandler onEnvironmentStateChangeEvent = null;
	public void RegisterForOnEnvironmentStateChange(OnEnvironmentStateChangeHandler delg) { 
		onEnvironmentStateChangeEvent += delg; }
	public void UnRegisterForOnEnvironmentSet(OnEnvironmentStateChangeHandler delg) { 
		onEnvironmentStateChangeEvent -= delg; }		
	
	private bool wantNewEnvSet;
	public AudioClip nextMusicClip;
	public bool WantNewEnvironmentSet
	{
		get
		{
			return wantNewEnvSet;	
		}
	}
	
	public int DestinationEnvironmentSet
	{
		get 
		{
			return destinationEnvironmentSetId;	
		}
		
		set
		{
			if (TransitionState != SwitchState.inactive)
			{
				notify.Error ("Destination Enviroment Set should not be called in the middle of a transition");
				return;
			}
			destinationEnvironmentSetId = value;
		}	
	}
	
	public enum SwitchState
	{
		inactive,
		waitingToBeAbleToDeletePools,
		deletingPools,
		loadingAssetBundle,
		addingNewPools,
		waitingToExitTunnel,
		finished,
	}
	
	//Phil - Distance between junction and tunnel entrance
	public float DistanceToTunnel { 
		get {
			if(GamePlayer.SharedInstance.HasFastTravel)
				return 0f;
			
			if(GamePlayer.SharedInstance.OnTrackPiece.TransitionTunnelDestinationId==1)
				return EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.DistanceToWW;
			if(GamePlayer.SharedInstance.OnTrackPiece.TransitionTunnelDestinationId==2)
				return EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.DistanceToDF;
			if(GamePlayer.SharedInstance.OnTrackPiece.TransitionTunnelDestinationId==3)
				return EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.DistanceToYBR;
			
			return 800.0f;
		}
	}
	
	private SwitchState _transitionState = SwitchState.inactive;
	public SwitchState TransitionState
	{
		get 
		{
			return _transitionState;	
		}
		
		private set
		{
			_transitionState = value;
			onEnvironmentStateChangeEvent(_transitionState, destinationEnvironmentSetId);
		}
	}
	
	/// <summary>
	/// Reset this instance when we want to run again.
	/// </summary> 
	public void Reset()
	{
		wantNewEnvSet = false;	
		TrackPiece.DistanceSubtracedPerPieceAdded = 2.0f;
		firstTransition = true;
	}
	
	void Awake()
	{
		SharedInstance = this;
		notify = new Notify(this.GetType().Name);
	}
	
	void Start()
	{
		GamePlayer.SharedInstance.RegisterForOnTrackPieceChange(ChangedOnTrackPiece);	
		
		TrackPiece.onDoTurn += PlayerTurned;
	//	GameController.SharedInstance.RegisterForOnPlayerTurn(PlayerTurned);
	}
	
	public static bool IsInactive()
	{
		bool result = EnvironmentSetSwitcher.SharedInstance == null ||
			EnvironmentSetSwitcher.SharedInstance.TransitionState == EnvironmentSetSwitcher.SwitchState.inactive;
		return result;
	}

	void ChangedOnTrackPiece(TrackPiece oldTrackPiece, TrackPiece newTrackPiece)
	{
		if(newTrackPiece == null)
			return;
		notify.Debug ("EnvironmentSetSwitcher.ChangedOnTrackPiece oldTrack" + oldTrackPiece + " newTrack" + newTrackPiece);
		if (newTrackPiece.TrackType == TrackPiece.PieceType.kTPTransitionTunnelEntrance)
		{
			startDeleteTime = Time.time;	

			if (TransitionState == SwitchState.inactive)
			{
				TransitionState = SwitchState.waitingToBeAbleToDeletePools;
			}

		}
		if (TransitionState == SwitchState.waitingToBeAbleToDeletePools)
		{
			if (IsEverythingPreviousTunnelTransition (newTrackPiece))
			//if (true)
			{
				StartCoroutine ( doEnvironmentSetTransition());
			}
		}
		
		if (oldTrackPiece != null && oldTrackPiece.TrackType == TrackPiece.PieceType.kTPTransitionTunnelExit)
		{
			finishTransition();	
			GameController.TextureReport("finishTransition");
			GameController.AudioReport ("finishTransition");
			GameController.MeshReport ("finishTransition");
		}
	}
	
	//Only allow difficulty reduction once per run
	private bool firstTransition = true;
	/// <summary>
	/// Finishes the transition. Makes sure the tunnel exit is fixed up
	/// </summary>
	void finishTransition()
	{
		TrackPieceTypeDefinition tunnelExit = TrackBuilder.SharedInstance.GetTypesFromTrackType( TrackPiece.PieceType.kTPTransitionTunnelExit);	
		tunnelExit.EnvironmentSet = -1;
		
		//Reduce the "DifficultyDistance" in GameController
		if(firstTransition)
		{
			GameController.SharedInstance.DifficultyDistanceTraveled -= GameProfile.SharedInstance.TransitionEndDifficultyReduction;
			firstTransition = false;
		}
		
		//dark forest temporarily needs a dynamic light
		//if (destinationEnvironmentSetId == 2)
		//{
		//	DebugConsole.DebugDirectionalLight(true);
		//}
		//else
		//{
		//	DebugConsole.DebugDirectionalLight(false);
		//}
		
		TrackPieceTypeDefinition tunnelEntrance = TrackBuilder.SharedInstance.GetTypesFromTrackType( TrackPiece.PieceType.kTPTransitionTunnelEntrance);	
		tunnelEntrance.EnvironmentSet = destinationEnvironmentSetId;
		TransitionState = SwitchState.inactive;
		
		timeSpentPerState[SwitchState.waitingToExitTunnel] = Time.time - startWaitingToExitTunnelTime;
		TrackPiece.DistanceSubtracedPerPieceAdded = 2.0f;
		
		TransitionState = SwitchState.finished;		// sends out 'finished' event to all listeners
		TransitionState = SwitchState.inactive;		
	}
	
	public string GetTunnelTimes()
	{
		string result = "";
		float oneTime;
		float totalTime = 0;
		if (timeSpentPerState.TryGetValue(SwitchState.deletingPools, out oneTime))
		{
			result += " deleting pools=" + oneTime;
			totalTime += oneTime;
		}
		if (timeSpentPerState.TryGetValue(SwitchState.addingNewPools, out oneTime))
		{
			result += " adding new pools=" + oneTime;
			totalTime += oneTime;
		}
		if (timeSpentPerState.TryGetValue(SwitchState.waitingToExitTunnel, out oneTime))
		{
			result += " waitingToExitTunnel=" + oneTime;
			totalTime += oneTime;
		}
		result += " total= " + totalTime;
		return result;
	}
	
	// checks all previous pieces, and returns true if they are all tunnel
	bool IsEverythingPreviousTunnelTransition(TrackPiece newTrackPiece)
	{
		bool result = true;
		TrackPiece curTrackPiece = newTrackPiece;
		while (curTrackPiece != null)
		{
			TrackPieceTypeDefinition def = TrackBuilder.SharedInstance.GetTypesFromTrackType( curTrackPiece.TrackType);
			if ( ! def.IsTransitionTunnel)
			{
				result = false;
				break;
			}
			curTrackPiece = curTrackPiece.PreviousTrackPiece;
		}
		return result;
	}
	
	// delete stuff related to the old environment set objects
	IEnumerator deleteOldObjects()
	{
		TransitionState = SwitchState.deletingPools;
		SpawnPool spawnPool;
		if (!PoolManager.Pools.TryGetValue("TrackMesh", out spawnPool))
		{
			notify.Debug("A 'TrackMesh' pool does NOT exist!");
			yield break;
		}
		
		notify.Debug("A 'TrackMesh' pool was found: " + spawnPool.group.name);
		
		// for memory reasons, we need to get rid of all the old pool objects
		List<TrackPiece.PieceType> pieceTypeKeys = TrackBuilder.SharedInstance.GetPieceTypesKeys();
		List<TrackPiece.PieceType> deletedKeys = new List<TrackPiece.PieceType>();
		int poolsDeleted = 0;
		
		foreach( TrackPiece.PieceType pieceType in pieceTypeKeys)
		{
			TrackPieceTypeDefinition def = TrackBuilder.SharedInstance.GetTypesFromTrackType(pieceType);
			if (!def.IsTransitionTunnel && !def.IsBalloon)
			{
				foreach ( string prefabName in def.Variations.Keys)
				{
					string withSuffix = prefabName + "_prefab";
					if (spawnPool.prefabs.ContainsKey(withSuffix))
					{
						Transform onePrefab = spawnPool.prefabs[withSuffix];
						notify.Debug("onePrefab = " + onePrefab);
						PrefabPool prefabPool = spawnPool.GetPrefabPool(onePrefab);
						notify.Debug("prefabPool = " + prefabPool);
						if (prefabPool != null)
						{
							prefabPool.SelfDestruct();
							/*	
							prefabPool.cullAbove = 0;
							prefabPool.cullDelay = 1;
							prefabPool.cullDespawned = true;
							prefabPool.cullMaxPerPass = 1;
							prefabPool._logMessages = true;
							prefabPool.StartCulling();
							*/
							notify.Debug("culling prefabPool = " + prefabPool);
						//	string fullPath = TrackPiece.GetFullPathOfPrefab(prefabName);
							spawnPool.DeletePrefabPool(prefabPool, withSuffix);
							poolsDeleted += 1;
						
						}
						
						string fullPath = TrackPiece.GetFullPathOfPrefab(prefabName);
						if (fullPath != null)
						{
							if ( DynamicElement.loaded_prefabs.ContainsKey(fullPath))
							{
								//notify.Debug("Removing " + fullPath + " from loaded prefabs");
								DynamicElement.loaded_prefabs.Remove(fullPath);
							}
						}
						else
						{
							//notify.Warning("could not get fullpath for  " + prefabName);
						}
						
					}
					//yield return new WaitForSeconds(0);
					yield return null;
					deletedKeys.Add(pieceType);
				}
			}
			yield return null;
		}
		notify.Debug ("deleting piece types");
		
		TrackBuilder.SharedInstance.DeletePieceTypes(deletedKeys);
		
		notify.Debug ("deleting skybox");
		GameController.SharedInstance.DestroyOZSkyBox();
		
		//stalls, moved to doEnvironmentSetTransition - hidden by whiteout
/*		notify.Debug ("Unloading asset bundle");
		string assetBundleName = EnvironmentSetManager.SharedInstance.GetAssetBundleName(originEnvironmentSetId);
		if (assetBundleName != null)
		{
			ResourceManager.SharedInstance.UnloadAssetBundle(assetBundleName);	
		}*/
			
		float endDeleteTime = Time.time;
		timeSpentPerState[SwitchState.deletingPools] = endDeleteTime - startDeleteTime;
	}
	
	/// <summary>
	/// Add objects related to the new environment set 
	/// </summary>
	IEnumerator addNewEnvironmentSetObjectsCoroutine()
	{
		TransitionState = SwitchState.addingNewPools; 
		float startAddingTime = Time.time;
		
		float startTime = Time.realtimeSinceStartup;
		notify.Debug("warming pools start " + startTime);
		//TrackPiece.WarmResources();
		yield return StartCoroutine( TrackPiece.WarmPoolsCoroutine());
		
		notify.Debug ("yielded to WarmPoolsCoroutine for " + destinationEnvironmentSetId);
		
		float endTime = Time.realtimeSinceStartup;
		float totalTime = endTime - startTime;
		notify.Debug (string.Format ("warming resources took  {0:0.000000} seconds" , totalTime));
		TrackBuilder.SharedInstance.FixupForTransitionExit();
		
		timeSpentPerState[SwitchState.addingNewPools] = Time.time - startAddingTime ;
	}
	

	void addNewEnvironmentSetObjects()
	{
		TransitionState = SwitchState.addingNewPools; 
		float startAddingTime = Time.time;
		
		float startTime = Time.realtimeSinceStartup;
		notify.Debug("warming pools start " + startTime);
		//TrackPiece.WarmResources();
		TrackPiece.WarmPools();
		
		notify.Debug ("yielded to WarmPoolsCoroutine for " + destinationEnvironmentSetId);
		
		float endTime = Time.realtimeSinceStartup;
		float totalTime = endTime - startTime;
		notify.Debug (string.Format ("warming resources took  {0:0.000000} seconds" , totalTime));
		TrackBuilder.SharedInstance.FixupForTransitionExit();
		
		timeSpentPerState[SwitchState.addingNewPools] = Time.time - startAddingTime ;
	}	
	
	// do the hard work of transitioning to a different environment set
	IEnumerator doEnvironmentSetTransition()
	{
		TransitionState = SwitchState.deletingPools;
		wantNewEnvSet = false;
//		yield return null;
		
		yield return StartCoroutine(deleteOldObjects());
		releaseEnvMaterials();
		
		GamePlayer.SharedInstance.WhiteoutFadeIn(0.2f, 1.0f);
		while(!GamePlayer.SharedInstance.WhiteoutComplete())
		{
			yield return null;
		}
		yield return null;//make sure we rendered the full whiteout frame
		notify.Debug ("Unloading asset bundle");
		string assetBundleName = EnvironmentSetManager.SharedInstance.GetAssetBundleName(originEnvironmentSetId);
		if (assetBundleName != null)
		{
			ResourceManager.SharedInstance.UnloadAssetBundle(assetBundleName);	
		}
		
		yield return Resources.UnloadUnusedAssets();
		System.GC.Collect();
		yield return null;
		
		TransitionState = SwitchState.loadingAssetBundle;
		notify.Debug("{0}  loading asset bundle for the new environment set", Time.realtimeSinceStartup);
		yield return StartCoroutine(ResourceManager.SharedInstance.LoadAssetBundleCoroutine( 
				EnvironmentSetManager.SharedInstance.GetAssetBundleName(DestinationEnvironmentSet), 
			false, -1, EnvironmentSetManager.SharedInstance.IsEmbedded(DestinationEnvironmentSet)
			));
		notify.Debug("{0}  done loading the asset bundle", Time.realtimeSinceStartup);
		
		ResourceManager.AllowLoad = true;
		
		notify.Debug ("adding new environment set pieces destinationEnvironmentSetId =" + destinationEnvironmentSetId);
		TrackBuilder.SharedInstance.PopulateEnvironmentSetPieces(destinationEnvironmentSetId);
		GamePlayer.SharedInstance.WhiteoutFadeOut(1.0f);


#if true
		yield return new WaitForSeconds(4f);
		GamePlayer.SharedInstance.WhiteoutFadeIn(0.2f, 1.0f);
		while(!GamePlayer.SharedInstance.WhiteoutComplete())
		{
			yield return null;
		}
		yield return null;//make sure we rendered the full whiteout frame
		notify.Debug ("starting WarmPrefabs for " + destinationEnvironmentSetId);
		TrackPiece.WarmPrefabs();
		//Debug.Log("UnloadUnusedAssets after WarmPrefabsCoroutine @ " + Time.realtimeSinceStartup);
		//yield return Resources.UnloadUnusedAssets();
		GameController.SharedInstance.NeededSkyBox = destinationEnvironmentSetId;
		notify.Debug ("starting SpawnOZSkyBox for " + destinationEnvironmentSetId);
		GameController.SharedInstance.SpawnOZSkyBox(1f);
		notify.Debug ("starting loadNewTrackMaterial for " + destinationEnvironmentSetId);
		loadNewTrackMaterial(destinationEnvironmentSetId);
		notify.Debug ("finished loadNewTrackMaterial for " + destinationEnvironmentSetId);
		AudioManager.SharedInstance.SFXready = false;
		AddNewMusicResource(DestinationEnvironmentSet);
		//AddNewSfxResource(DestinationEnvironmentSet);
		AudioManager.SharedInstance.SFXready = true;
		
		ResourceManager.AllowLoad = false;
		
		addNewEnvironmentSetObjects();

		GamePlayer.SharedInstance.WhiteoutFadeOut(1.0f);
#else		
//		GamePlayer.SharedInstance.WhiteoutFadeOut(0.5f);
		notify.Debug ("starting coroutine WarmPrefabsCoroutine for " + destinationEnvironmentSetId);
		yield return StartCoroutine( TrackPiece.WarmPrefabsCoroutine());
		//Debug.Log("UnloadUnusedAssets after WarmPrefabsCoroutine @ " + Time.realtimeSinceStartup);
		//yield return Resources.UnloadUnusedAssets();

		GameController.SharedInstance.NeededSkyBox = destinationEnvironmentSetId;
		notify.Debug ("starting coroutine SpawnOZSkyBoxCoroutine for " + destinationEnvironmentSetId);
		yield return StartCoroutine(GameController.SharedInstance.SpawnOZSkyBoxCoroutine(1f));
		notify.Debug ("starting coroutine loadNewTrackMaterialCoroutine for " + destinationEnvironmentSetId);
		yield return StartCoroutine( loadNewTrackMaterialCoroutine(destinationEnvironmentSetId));
		notify.Debug ("yielded to loadNewTrackMaterialCoroutine for " + destinationEnvironmentSetId);
		AudioManager.SharedInstance.SFXready = false;
		yield return StartCoroutine(AddNewMusicResourceCoroutine(DestinationEnvironmentSet));
		//yield return StartCoroutine(AddNewSfxResourceCoroutine(DestinationEnvironmentSet));
		AudioManager.SharedInstance.SFXready = true;
		
		ResourceManager.AllowLoad = false;
		
		yield return StartCoroutine(addNewEnvironmentSetObjectsCoroutine());

#endif		
		TrackPieceTypeDefinition tunnelExit = TrackBuilder.SharedInstance.GetTypesFromTrackType( TrackPiece.PieceType.kTPTransitionTunnelExit);
		if (tunnelExit == null)
		{
			notify.Error ("we are stuck forever in the tunnel as there is no tunnel exit");
			yield break;
		}
		
		startWaitingToExitTunnelTime = Time.time;
		tunnelExit.EnvironmentSet = destinationEnvironmentSetId;
		{
			TrackPiece piece = GamePlayer.SharedInstance.OnTrackPiece;
			while(piece.NextTrackPiece != null)
			{
				piece = piece.NextTrackPiece;
			}
			TrackBuilder.SharedInstance.QueuedPiecesToAdd.Add( piece.TrackType );
		}
		
		TrackBuilder.SharedInstance.QueuedPiecesToAdd.Add( TrackPiece.PieceType.kTPTransitionTunnelExit);
		
		TransitionState = SwitchState.waitingToExitTunnel;
		// notify.Error("waiting to exit tunnel");
		
		//now wait for the exit to be spawned and star the fog fade out
		bool readyToExit = false;
		while(!readyToExit)
		{
			yield return null;
			TrackPiece piece = GamePlayer.SharedInstance.OnTrackPiece;
			if((piece.TrackType == TrackPiece.PieceType.kTPTransitionTunnelExit)
				||((piece.TrackType == piece.NextTrackPiece.TrackType)))
			{
				readyToExit = true;
				break;
			}
		}		
		// now once we get informed that the player is leaving the TunnelExit
		// bring us back to inactive
		//fade out fog
		GamePlayer.SharedInstance.StartFogFadeOut();
	}
	
	
	public void ReloadEnvAudio(int envId)
	{// this is being called from GameController to load envAudio when we launch the game
		notify.Debug("ReloadEnvAudio " + envId);
		AudioManager.SharedInstance.SFXready = false;
		AddNewMusicResource(envId);
		//AddNewSfxResourceCoroutine(envId);
		AudioManager.SharedInstance.SFXready = true;
	}
	
	IEnumerator ReloadEnvAudioCoroutine(int envId){
		AudioManager.SharedInstance.SFXready = false;
		yield return StartCoroutine(AddNewMusicResourceCoroutine(envId));
		//yield return StartCoroutine(AddNewSfxResourceCoroutine(envId));
		AudioManager.SharedInstance.SFXready = true;
	}

	public void AddNewMusicResource(int envId )
	{
		notify.Debug("AddNewMusicResource----------------------- " + envId);
		
		nextMusicClip = ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].MusicFile) as AudioClip;
		AudioManager.SharedInstance.GameMusic = nextMusicClip;
		/*
		if(nextMusicClip) 
		{
			notify.Debug("We loaded a new Music Clip " + nextMusicClip.name);
			if(autoSwitch) AudioManager.SharedInstance.SwitchMusic(nextMusicClip);
		}
		*/
	}

	
	
	public IEnumerator AddNewMusicResourceCoroutine(int envId ){
		notify.Debug("AddNewMusicResource----------------------- " + envId);
		
		AssetBundleRequest abr = ResourceManager.SharedInstance.LoadAsyncFromAssetBundle(EnvironmentSetManager.SharedInstance.LocalDict[envId].MusicFile, typeof(AudioClip)); 
				//AssetBundleRequest abr = ResourceManager.SharedInstance.LoadAsyncFromAssetBundle(materialNames[i], typeof(Material));
		if(abr != null)
		{
			yield return abr;
			nextMusicClip = abr.asset as AudioClip;//ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].MusicFile) as AudioClip;
		}
		else
		{
			nextMusicClip = ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].MusicFile) as AudioClip;
		}
		if(nextMusicClip) 
		{
			notify.Debug("We loaded a new Music Clip " + nextMusicClip.name);
			//if(autoSwitch) AudioManager.SharedInstance.SwitchMusic(nextMusicClip);
			AudioClip oldMusicClip = AudioManager.SharedInstance.GameMusic;
			AudioManager.SharedInstance.GameMusic = nextMusicClip;
			Resources.UnloadAsset(oldMusicClip);
		}
		yield return null;//new WaitForSeconds(0.05f);
	}

	
	/*public void AddNewSfxResource(int envId )
	{
		notify.Debug("AddNewSfxResource----------------------- " + envId);
		AudioClip footstepNew = null;
		AudioClip tempClip = null;
		footstepNew = ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].FootstepFile1) as AudioClip;
		
		if(footstepNew) 
		{
			notify.Debug ("Found footstep1 " + footstepNew.name);
			if(GamePlayer.SharedInstance.footsteps) 
			{
				tempClip = GamePlayer.SharedInstance.footsteps.footstepsSfx[0];
				GamePlayer.SharedInstance.footsteps.footstepsSfx[0] = footstepNew;
			}
			if(AudioManager.SharedInstance!=null)	AudioManager.SharedInstance.footstepsSfx[0] = footstepNew; // we also referencing the audioclip in AudioManager since at application launch we don't have a GamePlayer instanst
			if(tempClip != null)
			{
				Resources.UnloadAsset(tempClip);
				tempClip = null;
			}
			footstepNew = null;
		}
		
		footstepNew = ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].FootstepFile2) as AudioClip;

		if(footstepNew != null) 
		{
			notify.Debug ("Found footstep2 " + footstepNew.name);
			if(GamePlayer.SharedInstance.footsteps) 
			{
				tempClip = GamePlayer.SharedInstance.footsteps.footstepsSfx[1];
				GamePlayer.SharedInstance.footsteps.footstepsSfx[1] = footstepNew;
			}
			if(AudioManager.SharedInstance!=null)
			{
				AudioManager.SharedInstance.footstepsSfx[1] = footstepNew;
			}
			if(tempClip != null)
			{
				Resources.UnloadAsset(tempClip);
				tempClip = null;
			}
			footstepNew = null;
		}
	
		footstepNew = ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].FootstepFile3) as AudioClip;
		if(footstepNew != null)  
		{
			notify.Debug ("Found footstep3 " + footstepNew.name);
			if(GamePlayer.SharedInstance.footsteps) 
			{
				tempClip = GamePlayer.SharedInstance.footsteps.footstepsSfx[2];
				GamePlayer.SharedInstance.footsteps.footstepsSfx[2] = footstepNew;
			}
			if(AudioManager.SharedInstance!=null)	AudioManager.SharedInstance.footstepsSfx[2] = footstepNew;
			if(tempClip != null)
			{
				Resources.UnloadAsset(tempClip);
				tempClip = null;
			}
			footstepNew = null;
		}
		footstepNew = ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].FootstepFile4) as AudioClip;
		if(footstepNew != null)  
		{
			notify.Debug ("Found footstep4 " + footstepNew.name);
			if(GamePlayer.SharedInstance.footsteps) 
			{
				tempClip = GamePlayer.SharedInstance.footsteps.footstepsSfx[3];
				GamePlayer.SharedInstance.footsteps.footstepsSfx[3] = footstepNew;
			}
			if(AudioManager.SharedInstance!=null)	AudioManager.SharedInstance.footstepsSfx[3] = footstepNew;
			if(tempClip != null)
			{
				Resources.UnloadAsset(tempClip);
				tempClip = null;
			}
			footstepNew = null;
		}
		footstepNew = ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].FootstepFile5) as AudioClip;
		if(footstepNew)  
		{
			notify.Debug ("Found footstep5 " + footstepNew.name);
			if(GamePlayer.SharedInstance.footsteps) 
			{
				tempClip = GamePlayer.SharedInstance.footsteps.footstepsSfx[4];
				GamePlayer.SharedInstance.footsteps.footstepsSfx[4] = footstepNew;
			}
			if(AudioManager.SharedInstance!=null)	AudioManager.SharedInstance.footstepsSfx[4] = footstepNew;
			if(tempClip != null)
			{
				Resources.UnloadAsset(tempClip);
				tempClip = null;
			}
			footstepNew = null;
		}
		// jumping
		footstepNew = ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].JumpingSoundEffect) as AudioClip;
		if(footstepNew)  
		{
			notify.Debug ("Found LandingSoundEffect " + footstepNew.name);
			if(AudioManager.SharedInstance!=null)	
			{
				tempClip = AudioManager.SharedInstance.jumping;
				AudioManager.SharedInstance.jumping = footstepNew; 
			}
			if(tempClip != null)
			{
				Resources.UnloadAsset(tempClip);
				tempClip = null;
			}
			footstepNew = null;
		}
		// landing
		footstepNew = ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].LandingSoundEffect) as AudioClip;
		if(footstepNew)  
		{
			notify.Debug ("Found LandingSoundEffect " + footstepNew.name);
			if(AudioManager.SharedInstance!=null)	
			{
				tempClip = AudioManager.SharedInstance.landing;
				AudioManager.SharedInstance.landing = footstepNew; 
			}
			if(tempClip != null)
			{
				Resources.UnloadAsset(tempClip);
				tempClip = null;
			}
			footstepNew = null;
		}
		// sliding
		footstepNew = ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].SlidingSoundEffect) as AudioClip;
		if(footstepNew)  
		{
			notify.Debug ("Found LandingSoundEffect " + footstepNew.name);
			if(AudioManager.SharedInstance!=null)	
			{
				tempClip = AudioManager.SharedInstance.sliding;
				AudioManager.SharedInstance.sliding = footstepNew; 
			}
			if(tempClip != null)
			{
				Resources.UnloadAsset(tempClip);
				tempClip = null;
			}
			footstepNew = null;
		}
		// turnLeft
		footstepNew = ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].TurnLeftSoundEffect) as AudioClip;
		if(footstepNew)  
		{
			notify.Debug ("Found LandingSoundEffect " + footstepNew.name);
			if(AudioManager.SharedInstance!=null)	
			{
				tempClip = AudioManager.SharedInstance.turnLeft;
				AudioManager.SharedInstance.turnLeft = footstepNew; 
			}
			Resources.UnloadAsset(tempClip);
			tempClip = null;
			footstepNew = null;
		}
		// landing
		footstepNew = ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].TurnRightSoundEffect) as AudioClip;
		if(footstepNew)  
		{
			notify.Debug ("Found LandingSoundEffect " + footstepNew.name);
			if(AudioManager.SharedInstance!=null)	
			{
				tempClip = AudioManager.SharedInstance.turnRight;
				AudioManager.SharedInstance.turnRight = footstepNew; 
			}
			if(tempClip != null)
			{
				Resources.UnloadAsset(tempClip);
				tempClip = null;
			}
			footstepNew = null;
		}
	}
	
	public IEnumerator AddNewSfxResourceCoroutine(int envId )
	{
		notify.Debug("AddNewSfxResource----------------------- " + envId);
		EnvironmentSetData data = EnvironmentSetManager.SharedInstance.LocalDict[envId];
		AudioClip footstepNew = null;
		AudioClip tempClip = null;
		AssetBundleRequest abr = ResourceManager.SharedInstance.LoadAsyncFromAssetBundle(data.FootstepFile1, typeof(AudioClip)); 
		if(abr != null)
		{
			yield return abr;
			footstepNew = abr.asset as AudioClip;//ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].FootstepFile1) as AudioClip;
		}
		else
		{
			footstepNew = ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].FootstepFile1) as AudioClip;
		}
		
		if(footstepNew) 
		{
			notify.Debug ("Found footstep1 " + footstepNew.name);
			if(GamePlayer.SharedInstance.footsteps) 
			{
				tempClip = GamePlayer.SharedInstance.footsteps.footstepsSfx[0];
				GamePlayer.SharedInstance.footsteps.footstepsSfx[0] = footstepNew;
			}
			if(AudioManager.SharedInstance!=null)	AudioManager.SharedInstance.footstepsSfx[0] = footstepNew; // we also referencing the audioclip in AudioManager since at application launch we don't have a GamePlayer instanst
			if(tempClip != null)
			{
				Resources.UnloadAsset(tempClip);
				tempClip = null;
			}
			footstepNew = null;
		}
		
		abr = ResourceManager.SharedInstance.LoadAsyncFromAssetBundle(data.FootstepFile2, typeof(AudioClip)); 
		if(abr != null)
		{
			yield return abr;
			footstepNew = abr.asset as AudioClip;//ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].FootstepFile2) as AudioClip;
		}
		else
		{
			footstepNew = ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].FootstepFile2) as AudioClip;
		}

		if(footstepNew != null) 
		{
			notify.Debug ("Found footstep2 " + footstepNew.name);
			if(GamePlayer.SharedInstance.footsteps) 
			{
				tempClip = GamePlayer.SharedInstance.footsteps.footstepsSfx[1];
				GamePlayer.SharedInstance.footsteps.footstepsSfx[1] = footstepNew;
			}
			if(AudioManager.SharedInstance!=null)
			{
				AudioManager.SharedInstance.footstepsSfx[1] = footstepNew;
			}
			if(tempClip != null)
			{
				Resources.UnloadAsset(tempClip);
				tempClip = null;
			}
			footstepNew = null;
		}
	
		abr = ResourceManager.SharedInstance.LoadAsyncFromAssetBundle(data.FootstepFile3, typeof(AudioClip)); 
		if(abr != null)
		{
			yield return abr;
			footstepNew = abr.asset as AudioClip;//ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].FootstepFile3) as AudioClip;
		}
		else
		{
			footstepNew = ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].FootstepFile3) as AudioClip;
		}
		if(footstepNew != null)  
		{
			notify.Debug ("Found footstep3 " + footstepNew.name);
			if(GamePlayer.SharedInstance.footsteps) 
			{
				tempClip = GamePlayer.SharedInstance.footsteps.footstepsSfx[2];
				GamePlayer.SharedInstance.footsteps.footstepsSfx[2] = footstepNew;
			}
			if(AudioManager.SharedInstance!=null)	AudioManager.SharedInstance.footstepsSfx[2] = footstepNew;
			if(tempClip != null)
			{
				Resources.UnloadAsset(tempClip);
				tempClip = null;
			}
			footstepNew = null;
		}
		abr = ResourceManager.SharedInstance.LoadAsyncFromAssetBundle(data.FootstepFile4, typeof(AudioClip)); 
		if(abr != null)
		{
			yield return abr;
			footstepNew = abr.asset as AudioClip;//ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].FootstepFile4) as AudioClip;
		}
		else
		{
			footstepNew = ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].FootstepFile4) as AudioClip;
		}
		if(footstepNew != null)  
		{
			notify.Debug ("Found footstep4 " + footstepNew.name);
			if(GamePlayer.SharedInstance.footsteps) 
			{
				tempClip = GamePlayer.SharedInstance.footsteps.footstepsSfx[3];
				GamePlayer.SharedInstance.footsteps.footstepsSfx[3] = footstepNew;
			}
			if(AudioManager.SharedInstance!=null)	AudioManager.SharedInstance.footstepsSfx[3] = footstepNew;
			if(tempClip != null)
			{
				Resources.UnloadAsset(tempClip);
				tempClip = null;
			}
			footstepNew = null;
		}
		abr = ResourceManager.SharedInstance.LoadAsyncFromAssetBundle(data.FootstepFile5, typeof(AudioClip)); 
		if(abr != null)
		{
			yield return abr;
			footstepNew = abr.asset as AudioClip;//ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].FootstepFile5) as AudioClip;
		}
		else
		{
			footstepNew = ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].FootstepFile5) as AudioClip;
		}
		if(footstepNew)  
		{
			notify.Debug ("Found footstep5 " + footstepNew.name);
			if(GamePlayer.SharedInstance.footsteps) 
			{
				tempClip = GamePlayer.SharedInstance.footsteps.footstepsSfx[4];
				GamePlayer.SharedInstance.footsteps.footstepsSfx[4] = footstepNew;
			}
			if(AudioManager.SharedInstance!=null)	AudioManager.SharedInstance.footstepsSfx[4] = footstepNew;
			if(tempClip != null)
			{
				Resources.UnloadAsset(tempClip);
				tempClip = null;
			}
			footstepNew = null;
		}
		// jumping
		abr = ResourceManager.SharedInstance.LoadAsyncFromAssetBundle(data.JumpingSoundEffect, typeof(AudioClip)); 
		if(abr != null)
		{
			yield return abr;
			footstepNew = abr.asset as AudioClip;//ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].JumpingSoundEffect) as AudioClip;
		}
		else
		{
			footstepNew = ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].JumpingSoundEffect) as AudioClip;
		}
		if(footstepNew)  
		{
			notify.Debug ("Found LandingSoundEffect " + footstepNew.name);
			if(AudioManager.SharedInstance!=null)	
			{
				tempClip = AudioManager.SharedInstance.jumping;
				AudioManager.SharedInstance.jumping = footstepNew; 
			}
			if(tempClip != null)
			{
				Resources.UnloadAsset(tempClip);
				tempClip = null;
			}
			footstepNew = null;
		}
		// landing
		abr = ResourceManager.SharedInstance.LoadAsyncFromAssetBundle(data.LandingSoundEffect, typeof(AudioClip)); 
		if(abr != null)
		{
			yield return abr;
			footstepNew = abr.asset as AudioClip;//ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].LandingSoundEffect) as AudioClip;
		}
		else
		{
			footstepNew = ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].LandingSoundEffect) as AudioClip;
		}
		if(footstepNew)  
		{
			notify.Debug ("Found LandingSoundEffect " + footstepNew.name);
			if(AudioManager.SharedInstance!=null)	
			{
				tempClip = AudioManager.SharedInstance.landing;
				AudioManager.SharedInstance.landing = footstepNew; 
			}
			if(tempClip != null)
			{
				Resources.UnloadAsset(tempClip);
				tempClip = null;
			}
			footstepNew = null;
		}
		// sliding
		abr = ResourceManager.SharedInstance.LoadAsyncFromAssetBundle(data.SlidingSoundEffect, typeof(AudioClip)); 
		if(abr != null)
		{
			yield return abr;
			footstepNew = abr.asset as AudioClip;//ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].SlidingSoundEffect) as AudioClip;
		}
		else
		{
			footstepNew = ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].SlidingSoundEffect) as AudioClip;
		}
		if(footstepNew)  
		{
			notify.Debug ("Found LandingSoundEffect " + footstepNew.name);
			if(AudioManager.SharedInstance!=null)	
			{
				tempClip = AudioManager.SharedInstance.sliding;
				AudioManager.SharedInstance.sliding = footstepNew; 
			}
			if(tempClip != null)
			{
				Resources.UnloadAsset(tempClip);
				tempClip = null;
			}
			footstepNew = null;
		}
		// turnLeft
		abr = ResourceManager.SharedInstance.LoadAsyncFromAssetBundle(data.TurnLeftSoundEffect, typeof(AudioClip)); 
		if(abr != null)
		{
			yield return abr;
			footstepNew = abr.asset as AudioClip;//ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].TurnLeftSoundEffect) as AudioClip;
		}
		else
		{
			footstepNew = ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].TurnLeftSoundEffect) as AudioClip;
		}
		if(footstepNew)  
		{
			notify.Debug ("Found LandingSoundEffect " + footstepNew.name);
			if(AudioManager.SharedInstance!=null)	
			{
				tempClip = AudioManager.SharedInstance.turnLeft;
				AudioManager.SharedInstance.turnLeft = footstepNew; 
			}
			Resources.UnloadAsset(tempClip);
			tempClip = null;
			footstepNew = null;
		}
		// landing
		abr = ResourceManager.SharedInstance.LoadAsyncFromAssetBundle(data.TurnRightSoundEffect, typeof(AudioClip)); 
		if(abr != null)
		{
			yield return abr;
			footstepNew = abr.asset as AudioClip;//ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].TurnRightSoundEffect) as AudioClip;
		}
		else
		{
			footstepNew = ResourceManager.Load(EnvironmentSetManager.SharedInstance.LocalDict[envId].TurnRightSoundEffect) as AudioClip;
		}
		if(footstepNew)  
		{
			notify.Debug ("Found LandingSoundEffect " + footstepNew.name);
			if(AudioManager.SharedInstance!=null)	
			{
				tempClip = AudioManager.SharedInstance.turnRight;
				AudioManager.SharedInstance.turnRight = footstepNew; 
			}
			if(tempClip != null)
			{
				Resources.UnloadAsset(tempClip);
				tempClip = null;
			}
			footstepNew = null;
		}
		
		yield return null;//new WaitForSeconds(0.05f);
	}*/
	
	private string AlternateEnvironmentMaterialSuffix(bool allowAlt)
	{
		if(allowAlt)
		{
			switch(GameController.SharedInstance.GetDeviceGeneration())
			{
			case GameController.DeviceGeneration.Unsupported:
			case GameController.DeviceGeneration.iPhone3GS:
			case GameController.DeviceGeneration.iPodTouch3:
			case GameController.DeviceGeneration.MedEnd:
				return "_lo";
			case GameController.DeviceGeneration.LowEnd:
			case GameController.DeviceGeneration.iPhone4:
			case GameController.DeviceGeneration.iPodTouch4:
				return "_opt";
			default:
				return "_hi";
			}
		}
		else
		{
			return "";
		}
	}
	
	private IEnumerator loadEnvMaterialsCoroutine(string[] materialNames, GameController.EnvironmentMaterials materialsOut, bool allowAlt)
	{
		string materialSuffix = AlternateEnvironmentMaterialSuffix(allowAlt);
	
		Material[] materials = new Material[materialNames.Length];
		for(int i = 0; i < materials.Length; ++i)
		{
			if((materialNames[i] != null) && (materialNames[i] != ""))
			{
				AssetBundleRequest abr = ResourceManager.SharedInstance.LoadAsyncFromAssetBundle(materialNames[i], typeof(Material));
				if(abr != null)
				{
					yield return abr;
					materials[i] = abr.asset as Material;//ResourceManager.Load(materialNames[0] + materialSuffix, typeof(Material)) as Material;
				}
				else
				{
					materials[i] = ResourceManager.Load(materialNames[i] + materialSuffix, typeof(Material)) as Material;
				}
				if(materials[i] == null)
				{//fall back to low res
					abr = ResourceManager.SharedInstance.LoadAsyncFromAssetBundle(materialNames[i] + "_lo", typeof(Material));
					if(abr != null)
					{
						yield return abr;
						materials[i] = abr.asset as Material;
					}
					else
					{
						materials[i] = ResourceManager.Load(materialNames[i], typeof(Material)) as Material;
					}
				}
				if(materials[i] != null)
				{
					materials[i].mainTexture.mipMapBias = GameController.SharedInstance.EnvironmentMipMapBias;
				}
				else
				{
					notify.Warning("Failed to load " + materialNames[i]);
				}
			}
		}

		materialsOut.Opaque = materials[0];
		materialsOut.Decal = materials[1];
		for(int i = 0; i < materialsOut.Extra.Length; ++i)
		{
			if((i+2 < materials.Length)&&(materials[i+2] != null))
			{
				materialsOut.Extra[i] = materials[i+2];
			}
		}
	}

	private void releaseEnvMaterials(GameController.EnvironmentMaterials materials)
	{
		materials.Opaque = null;
		materials.Decal = null;
		for(int i =0; i < materials.Extra.Length; ++i)
		{
			materials.Extra[i] = null;
		}
	}	

	public void releaseEnvMaterials()
	{
		releaseEnvMaterials(GameController.SharedInstance.TrackMaterials);
		releaseEnvMaterials(GameController.SharedInstance.TrackFadeMaterials);
	}	
	
	private void loadEnvMaterials(string[] materialNames, GameController.EnvironmentMaterials materialsOut, bool allowAlt)
	{
		string materialSuffix = AlternateEnvironmentMaterialSuffix(allowAlt);
	
		Material[] materials = new Material[materialNames.Length];
		for(int i = 0; i < materials.Length; ++i)
		{
			if((materialNames[i] != null) && (materialNames[i] != ""))
			{
				materials[i] = ResourceManager.Load(materialNames[i] + materialSuffix, typeof(Material)) as Material;
				if(materials[i] == null)
				{//fall back to low res
					materials[i] = ResourceManager.Load(materialNames[i] + "_lo", typeof(Material)) as Material;
				}
				if(materials[i] != null)
				{
					//materials[i].mainTexture.mipMapBias = GameController.SharedInstance.EnvironmentMipMapBias;
				}
				else
				{
					notify.Warning("Failed to load " + materialNames[i]);
				}
			}
		}

		materialsOut.Opaque = materials[0];
		materialsOut.Decal = materials[1];
		for(int i = 0; i < materialsOut.Extra.Length; ++i)
		{
			if((i+2 < materials.Length)&&(materials[i+2] != null))
			{
				materialsOut.Extra[i] = materials[i+2];
			}
		}
	}
	
	public IEnumerator loadNewTrackMaterialCoroutine(int newEnvironmentSet)
	{
		GameController.TextureReport("Before new track materials coroutine");
		if (EnvironmentSetManager.SharedInstance.IsLocallyAvailable(newEnvironmentSet))
		{
			if(GameController.SharedInstance.TunnelMaterials == null)
			{
				GameController.SharedInstance.TunnelMaterials = new GameController.EnvironmentMaterials();
				string[] tunnelMaterials = {
					"Oz/Materials/oz_tt_master_opaque",
					null,
					"Oz/Materials/oz_tt_master_fade_road_fog",
					"Oz/Materials/oz_tt_master_fade_bricks_fog",
 					"Oz/Materials/oz_tt_master_opaque_spinning_fog",
					"Oz/Materials/oz_tt_master_decals_spinning"
				};
				yield return StartCoroutine(loadEnvMaterialsCoroutine(tunnelMaterials, GameController.SharedInstance.TunnelMaterials,false));
			}
			if(GameController.SharedInstance.TunnelFadeMaterials == null)
			{
				GameController.SharedInstance.TunnelFadeMaterials = new GameController.EnvironmentMaterials();
				string[] tunnelFadeMaterials = {
					"Oz/Materials/oz_tt_master_alpha",
					null,
					"Oz/Materials/oz_tt_master_fade_road_fog",
					"Oz/Materials/oz_tt_master_fade_bricks_fog",
 					"Oz/Materials/oz_tt_master_opaquefade_spinning_fog",
					"Oz/Materials/oz_tt_master_decalsFade_spinning"
				};
				yield return StartCoroutine(loadEnvMaterialsCoroutine(tunnelFadeMaterials, GameController.SharedInstance.TunnelFadeMaterials,false));
			}

			if(GameController.SharedInstance.BalloonMaterials == null)
			{
				GameController.SharedInstance.BalloonMaterials = new GameController.EnvironmentMaterials();
				string[] balloonMaterials = {
					"Oz/Materials/oz_bc_master_opaque_fog",
					"Oz/Materials/oz_bc_master_decals_fog"
				};
				yield return StartCoroutine(loadEnvMaterialsCoroutine(balloonMaterials, GameController.SharedInstance.BalloonMaterials,false));
			}
			if(GameController.SharedInstance.BalloonFadeMaterials == null)
			{
				GameController.SharedInstance.BalloonFadeMaterials = new GameController.EnvironmentMaterials();
				string[] balloonFadeMaterials = {
					"Oz/Materials/oz_bc_master_alpha_fog",
					"Oz/Materials/oz_bc_master_alpha_decals_fog"
				};
				yield return StartCoroutine(loadEnvMaterialsCoroutine(balloonFadeMaterials, GameController.SharedInstance.BalloonFadeMaterials,false));
			}			
			
			EnvironmentSetData envSetData = EnvironmentSetManager.SharedInstance.LocalDict[newEnvironmentSet];
		
			string[] opaque = new string[4];
			string[] fade = new string[4];
			opaque[0] = envSetData.OpaqueMaterialPath;
			opaque[1] =	envSetData.DecalMaterialPath;
			opaque[2] = envSetData.Extra1MaterialPath;
			opaque[3] = envSetData.Extra2MaterialPath;
//			opaque[4] = envSetData.Extra3MaterialPath;
//			opaque[5] = envSetData.Extra4MaterialPath;

			//regular materials
			if(GameController.SharedInstance.TrackMaterials == null)
			{
				GameController.SharedInstance.TrackMaterials = new GameController.EnvironmentMaterials();
			}
			yield return StartCoroutine(loadEnvMaterialsCoroutine(opaque, GameController.SharedInstance.TrackMaterials,true));
			
			fade[0] = envSetData.FadeOutMaterialPath;
			fade[1] = envSetData.DecalFadeOutMaterialPath;
			fade[2] = envSetData.Extra1FadeMaterialPath;
			fade[3] = envSetData.Extra2FadeMaterialPath;
//			fade[4] = envSetData.Extra3FadeMaterialPath;
//			fade[5] = envSetData.Extra4FadeMaterialPath;

			if(GameController.SharedInstance.TrackFadeMaterials == null)
			{
				GameController.SharedInstance.TrackFadeMaterials = new GameController.EnvironmentMaterials();
			}
			yield return StartCoroutine(loadEnvMaterialsCoroutine(fade, GameController.SharedInstance.TrackFadeMaterials,true));
		}
		else
		{
			notify.Error("Don't know how to load the new track material for environment set " + newEnvironmentSet);
		}
		GameController.TextureReport("After new track materials coroutine");
		
	}
	
	/// <summary>
	/// Loads the new track material for the upcoming environment set
	/// </summary>
	/// <returns>
	/// True if the track material was loaded, false if it couldn't be found
	/// </returns>
	/// <param name='newEnvironmentSet'>
	/// 0 for machu, 1 for whimsey woods, 
	/// </param>
	public bool loadNewTrackMaterial(int newEnvironmentSet)
	{
		GameController.TextureReport("Before new track materials");
		bool result = true;
		
		if (EnvironmentSetManager.SharedInstance.IsLocallyAvailable(newEnvironmentSet))
		{
			if(GameController.SharedInstance.TunnelMaterials == null)
			{
				GameController.SharedInstance.TunnelMaterials = new GameController.EnvironmentMaterials();
				string[] tunnelMaterials = {
					"Oz/Materials/oz_tt_master_opaque",
					null,
					"Oz/Materials/oz_tt_master_fade_road_fog",
					"Oz/Materials/oz_tt_master_fade_bricks_fog",
 					"Oz/Materials/oz_tt_master_opaque_spinning_fog",
					"Oz/Materials/oz_tt_master_decals_spinning"
				};
				loadEnvMaterials(tunnelMaterials, GameController.SharedInstance.TunnelMaterials,false);
			}
			if(GameController.SharedInstance.TunnelFadeMaterials == null)
			{
				GameController.SharedInstance.TunnelFadeMaterials = new GameController.EnvironmentMaterials();
				string[] tunnelFadeMaterials = {
					"Oz/Materials/oz_tt_master_alpha",
					null,
					"Oz/Materials/oz_tt_master_fade_road_fog",
					"Oz/Materials/oz_tt_master_fade_bricks_fog",
 					"Oz/Materials/oz_tt_master_opaquefade_spinning_fog",
					"Oz/Materials/oz_tt_master_decalsFade_spinning"
				};
				loadEnvMaterials(tunnelFadeMaterials, GameController.SharedInstance.TunnelFadeMaterials,false);
			}

			if(GameController.SharedInstance.BalloonMaterials == null)
			{
				GameController.SharedInstance.BalloonMaterials = new GameController.EnvironmentMaterials();
				string[] balloonMaterials = {
					"Oz/Materials/oz_bc_master_opaque_fog",
					"Oz/Materials/oz_bc_master_decals_fog"
				};
				loadEnvMaterials(balloonMaterials, GameController.SharedInstance.BalloonMaterials,false);
			}
			if(GameController.SharedInstance.BalloonFadeMaterials == null)
			{
				GameController.SharedInstance.BalloonFadeMaterials = new GameController.EnvironmentMaterials();
				string[] balloonFadeMaterials = {
					"Oz/Materials/oz_bc_master_alpha_fog",
					"Oz/Materials/oz_bc_master_alpha_decals_fog"
				};
				loadEnvMaterials(balloonFadeMaterials, GameController.SharedInstance.BalloonFadeMaterials,false);
			}			
			
			EnvironmentSetData envSetData = EnvironmentSetManager.SharedInstance.LocalDict[newEnvironmentSet];
		
			string[] opaque = new string[4];
			string[] fade = new string[4];
			opaque[0] = envSetData.OpaqueMaterialPath;
			opaque[1] =	envSetData.DecalMaterialPath;
			opaque[2] = envSetData.Extra1MaterialPath;
			opaque[3] = envSetData.Extra2MaterialPath;
//			opaque[4] = envSetData.Extra3MaterialPath;
//			opaque[5] = envSetData.Extra4MaterialPath;

			//regular materials
			if(GameController.SharedInstance.TrackMaterials == null)
			{
				GameController.SharedInstance.TrackMaterials = new GameController.EnvironmentMaterials();
			}
			loadEnvMaterials(opaque, GameController.SharedInstance.TrackMaterials,true);
			
			fade[0] = envSetData.FadeOutMaterialPath;
			fade[1] = envSetData.DecalFadeOutMaterialPath;
			fade[2] = envSetData.Extra1FadeMaterialPath;
			fade[3] = envSetData.Extra2FadeMaterialPath;
//			fade[4] = envSetData.Extra3FadeMaterialPath;
//			fade[5] = envSetData.Extra4FadeMaterialPath;

			if(GameController.SharedInstance.TrackFadeMaterials == null)
			{
				GameController.SharedInstance.TrackFadeMaterials = new GameController.EnvironmentMaterials();
			}
			loadEnvMaterials(fade, GameController.SharedInstance.TrackFadeMaterials,true);
			

		}
		else
		{
			result = false;
			notify.Error("Don't know how to load the new track material for environment set " + newEnvironmentSet);
		}
		GameController.TextureReport("After new track materials");
		return result;
	}
	
	/// <summary>
	/// GameController wants an new environment set. Do the hard work.
	/// </summary> 
	public void RequestEnvironmentSetChange( int newDestId)
	{
		notify.Debug("RequestEnvironmentSetChange");
		this.originEnvironmentSetId = TrackBuilder.SharedInstance.CurrentEnvironmentSetId;
		this.destinationEnvironmentSetId = newDestId;
		wantNewEnvSet = true;
	
		// we need to do this to avoid tunnel entrance being placed twice
		TrackPiece.DistanceSubtracedPerPieceAdded = 3000.0f;  // yes it's humongously large
		
		// show envProgressHud
		UIManagerOz.SharedInstance.inGameVC.ShowEnvProgress(destinationEnvironmentSetId);
		
	}
	
	public void CancelEnvironmentSetChange()
	{
		wantNewEnvSet = false;
		if(UIManagerOz.SharedInstance.inGameVC!=null)
			UIManagerOz.SharedInstance.inGameVC.HideEnvProgress();
	}
	
	
	/// <summary>
	/// Gets the random destination environment set.
	/// </summary>
	/// <returns>
	/// 0 for machu, 1 for whimsy woods, others added as implemented
	/// </returns> 
	
	private int lastChoice = 0;
	public int GetRandomDestinationEnvironmentSet()
	{
		int result;
		
		int chance = Random.Range(0,2);

		if (TrackBuilder.SharedInstance.CurrentEnvironmentSetId == EnvironmentSetManager.WhimsyWoodsId) //Currently in WW
		{	
			if(lastChoice == EnvironmentSetManager.DarkForestId) //If last choice was DF
			{
				result = EnvironmentSetManager.YellowBrickRoadId;	
			}
			
			else if(lastChoice == EnvironmentSetManager.YellowBrickRoadId) //If WC was chosen last
			{
				result = EnvironmentSetManager.DarkForestId;
			}
			
			else //If neither location was chosen last, randomly choose one
			{
				if(chance == 0)
				{
					result = EnvironmentSetManager.DarkForestId;
				}
				
				else
				{
					result = EnvironmentSetManager.YellowBrickRoadId;				
				}
			}
		}
		
		else if (TrackBuilder.SharedInstance.CurrentEnvironmentSetId == EnvironmentSetManager.DarkForestId) //Currently in DF
		{	
			if(lastChoice == EnvironmentSetManager.WhimsyWoodsId) //If last choice was WW
			{
				result = EnvironmentSetManager.YellowBrickRoadId;	
			}
			
			else if(lastChoice == EnvironmentSetManager.YellowBrickRoadId) //If WC was chosen last
			{
				result = EnvironmentSetManager.WhimsyWoodsId;
			}
			
			else //If neither location was chosen last, randomly choose one
			{
				if(chance == 0)
				{
					result = EnvironmentSetManager.WhimsyWoodsId;
				}
				
				else
				{
					result = EnvironmentSetManager.YellowBrickRoadId;					
				}
			}
		}

		else //if (TrackBuilder.SharedInstance.CurrentEnvironmentSetId == 3) //Currently in WC
		{	
			if(lastChoice == EnvironmentSetManager.WhimsyWoodsId) //If last choice was WW
			{
				result = EnvironmentSetManager.DarkForestId;
			}
			
			else if(lastChoice == EnvironmentSetManager.DarkForestId) //If DF was chosen last
			{
				result = EnvironmentSetManager.WhimsyWoodsId;
			}
			
			else //If neither location was chosen last, randomly choose one
			{
				if(chance == 0)
				{
					result = EnvironmentSetManager.WhimsyWoodsId;
				}
				
				else
				{
					result = EnvironmentSetManager.DarkForestId;				
				}
			}
		}		
		
		notify.Debug("Dest env set = " + result);
		lastChoice = result;
		return result;
	}
	
	public void PlayerTurned(TrackPiece piece, int segment)
	{
		bool isLeft = !piece.UseAlternatePath;
		
		if (GamePlayer.SharedInstance.OnTrackPiece != null && GamePlayer.SharedInstance.OnTrackPiece.TrackType == TrackPiece.PieceType.kTPEnvSetJunction)
		{
			// okay so for now we say he's going to a different environment set if he turns towards the tunnel transitions
			if (!wantNewEnvSet)
			{
				TransitionSignDecider decider = GamePlayer.SharedInstance.OnTrackPiece.GetComponentInChildren<TransitionSignDecider>();
				if (decider == null)
				{
					notify.Error("couldn't find TransitionSignDecider in " + GamePlayer.SharedInstance.OnTrackPiece);
				}
				
				//Phil - Turn Towards Transition - Now how do we set the distance...
				if (decider.MainLeftGoesToTransitionTunnel)  
				{
					if (isLeft && (GamePlayer.SharedInstance.IsTurningLeft||GamePlayer.SharedInstance.JustTurned)) 
					{
						RequestEnvironmentSetChange(decider.DestinationId);	
						
						//Reduce the "DifficultyDistance" in GameController
						if(firstTransition)
							GameController.SharedInstance.DifficultyDistanceTraveled -= GameProfile.SharedInstance.EnvSignDifficultyReduction;
					}
				}
				else  {
					// so turning right goes to transition tunnel, and he did turn right
					if ( ! isLeft && (GamePlayer.SharedInstance.IsTurningRight||GamePlayer.SharedInstance.JustTurned)) 
					{
						RequestEnvironmentSetChange(decider.DestinationId);
						
						//Reduce the "DifficultyDistance" in GameController
						if(firstTransition)
							GameController.SharedInstance.DifficultyDistanceTraveled -= GameProfile.SharedInstance.EnvSignDifficultyReduction;
					}
				}
			}
		}
	}
}
