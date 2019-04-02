using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

public class UIManagerOz : MonoBehaviour
{
	public static Notify notify;

	public static UIManagerOz SharedInstance;
	
	public bool InstantiateDialogs = true;

	//-- These should each be a ViewController.	
	public UIPaperViewControllerOz PaperVC;	
	public UIMainMenuViewControllerOz mainVC;
	public UIIdolMenuViewControllerOz idolMenuVC;
	public UIPostGameViewControllerOz postGameVC;
	public UIInGameViewControllerOz inGameVC;
	public UIInventoryViewControllerOz inventoryVC;
	public UIIAPMiniViewControllerOz IAPMiniStoreVC;	
	public UIObjectivesViewControllerOz ObjectivesVC;
	public UISettingsViewControllerOz settingsVC;
	public UIStatViewControllerOz statsVC;
	public UILeaderboardViewControllerOz leaderboardVC;
	public UIGatchaViewControllerOz gatchVC;
	public UIWorldOfOzViewControllerOz worldOfOzVC;

	// single button dialog variations, non-viewcontroller
	public UIConfirmDialogOz confirmDialog;
	public UIOkayDialogOz okayDialog;
	public UIRewardDialogOz rewardDialog;	
	public UIDownloadDialogOz downloadDialog;
	public UIDownloadDialogUI downloadUIDialog;
	public UIDownloadDialogLocal downloadLocalDialog;
	public UIFacebookDialogOz facebookLoginDialog;
	public UIUpdateDialogOz updateDialog;
	public UIUpdateNoDialogOz updateNoDialog;
	public UINewLocDialogOz newLocDialog;	
	
	// wxj
	public UIConfirmDialogOz onekeyPurchaseDialog;
	
	//public ProfileManager profileManager;
	
	public UIProgressBar progressBar;
	
	public GameObject UIResurrectMenu;
	
	public List<UILabel> CoinLabels;
	public List<UILabel> SpecialCoinLabels;
	
	public Camera MainGameCamera;
	public Transform resurrectProgressBar;
	
	public PassFriend passFriend;
	
	private static bool _hasShownOfferWall = false;
	public static bool HasShownOfferWall
	{
		get
		{
			return _hasShownOfferWall;
		}
		set
		{
			_hasShownOfferWall = value;
		}
	}
	
	public Camera UICamera;	

	//-- Event Delegate Definitions
	public delegate void voidClickedHandler();	
	public static event voidClickedHandler onPlayClickedHandler;		
	public static event voidClickedHandler onUnPauseClicked;
	public static event voidClickedHandler onPauseClicked;
	
	private int promptID = -1;
	private bool isStarting = true;
	
	private int showCounter = 0;
	
	public string _Test = "";
	
	private int hiPromptInterval = int.MaxValue;
	public static int lastPromptedLevel = -1;
	
	private bool forceDFDownloadPrompt = false;
	private bool forceUIDownloadPrompt = false;
	private bool noDownloadOKPrompt = false;

	private GameObject downloadDoneMsgObject = null;

	public static bool escapeHandled = false;
	
//	public static bool isUsingHiResUI = false;
	
	private List<UIModalDialogOz> activeModals = new List<UIModalDialogOz>();
	
	private LayerMask normalEventLayerMask, modalEventLayerMask;
	private UICamera UICameraScript;
	
	public UIResolutionType chosenUIResType = UIResolutionType.kResolution960;

	private bool firstPassOnUpdateDialogCompleted = false;
	
	public bool deviceSupportsHiRes = true;
	
	private AmpBundleManager ampBundleManager;
	
	public void SetUICameraLayerMask(bool toModal)	// pass in 'true' for modal dialogs, false otherwise
	{
		UICameraScript.eventReceiverMask = (toModal) ? modalEventLayerMask : normalEventLayerMask;
	}	
	
	public void Pause()
	{
		onPauseClicked();	
	}		
	
	public void Unpause()
	{
		onUnPauseClicked();	
	}
	
	/// <summary>
	/// For rapid lookup, have a way to go from the type of the view controller directly to an instantiated game object
	/// </summary>
	private Dictionary<System.Type,  Object> type2InstantiatedObject = new Dictionary<System.Type,Object>();	

	public void Awake()
	{
		SharedInstance = this;
		notify = new Notify(this.GetType().Name);
		GameObject progressBarGO = Resources.Load("Oz/Prefabs/UIProgressBar", typeof(GameObject)) as GameObject;
		progressBar = progressBarGO.GetComponent<UIProgressBar>();
		
		UICameraScript = UICamera.gameObject.GetComponent<UICamera>();
		normalEventLayerMask = UICameraScript.eventReceiverMask;
		modalEventLayerMask = 1048576;	//LayerMask.NameToLayer("dialogBoxUI");
		
		//UICamera = gameObject.GetComponent<Camera>();
		/* eyal - this bit of code is to create a new clone of an empty atlas which we then populate with the right atlas resolution,
		 		We do this to prevent the issue where anyone who saves the ozGame Scene overwrite the default atlas resolution
		GameObject at = Resources.Load("Oz/interface/Atlases/interfaceMasterOz") as GameObject;
		notify.Debug("atlas " + at.name);
		GameObject atGo= GameObject.Instantiate(at) as GameObject;
		InterfaceMaster = atGo.GetComponent<UIAtlas>();
		notify.Debug("InterfaceMaster " + InterfaceMaster);
		ChooseAtlasBasedOnScreenResolution();
		*/
	}
	
	public void Start() 
	{
		ampBundleManager = Services.Get<AmpBundleManager>();
		
//		RsesetAllPrefs();  // denug only
		
		showCounter = PlayerPrefs.GetInt("DownloadPromptCounter",-1);
		
//		showCounter = 0;//
		
		lastPromptedLevel = PlayerPrefs.GetInt("LastPromptedLevel",-1);
//		Debug.LogError ("promptCounter is " + showCounter);
//		Debug.LogError ("lastPromptedLevel is " + lastPromptedLevel);
	
		MainGameCamera = Camera.main;	// this should be the only reference to the main camera in all of the GUI
		
		//TODO: 4s
		if (InstantiateDialogs) 
			InstantiateViewControllers();
		disableResurrectMenu();

		if (PaperVC) { PaperVC.Hide(PaperVC.gameObject); }	//.disappear(); }		
		if (mainVC) { mainVC.Hide(mainVC.gameObject); }	//.disappear(); }

		if (leaderboardVC) { leaderboardVC.Hide(leaderboardVC.gameObject); }	//.disappear(); }
		if (settingsVC) { settingsVC.Hide(settingsVC.gameObject); }	//.disappear(); }
		if (statsVC) { statsVC.Hide(statsVC.gameObject); }	//.disappear(); }
		if (postGameVC) { postGameVC.Hide(postGameVC.gameObject); }	//.disappear(); }
		if (IAPMiniStoreVC) { IAPMiniStoreVC.Hide(IAPMiniStoreVC.gameObject); }	//.disappear(); }		
		if (idolMenuVC) { idolMenuVC.Hide(idolMenuVC.gameObject); }	//.disappear();}	
		if (gatchVC) { gatchVC.Hide(gatchVC.gameObject); }	//.disappear();}	
		if (inventoryVC) { inventoryVC.Hide(inventoryVC.gameObject); }	//.disappear();}		
		if (ObjectivesVC) { ObjectivesVC.Hide(ObjectivesVC.gameObject); }	//.disappear(); }	
		if (worldOfOzVC) { worldOfOzVC.Hide(worldOfOzVC.gameObject); }	//.disappear(); }	
		
		// just turn this one off, don't mess with its alpha
		if (inGameVC) { NGUITools.SetActive(inGameVC.gameObject, false); }	//.disappear();}		
		
		// non-viewcontrollers:
		if (confirmDialog) { NGUITools.SetActive(confirmDialog.gameObject, false); }
		if (okayDialog) { NGUITools.SetActive(okayDialog.gameObject, false); }
		if (rewardDialog) { NGUITools.SetActive(rewardDialog.gameObject, false); }
		if (downloadDialog) { NGUITools.SetActive(downloadDialog.gameObject, false); }
		if (downloadUIDialog) { NGUITools.SetActive(downloadUIDialog.gameObject, false); }
		if (downloadLocalDialog) { NGUITools.SetActive(downloadLocalDialog.gameObject, false); }
		if (facebookLoginDialog) { NGUITools.SetActive(facebookLoginDialog.gameObject, false); }
		if (updateDialog) { NGUITools.SetActive(updateDialog.gameObject, false); }
		if (updateNoDialog) { NGUITools.SetActive(updateNoDialog.gameObject, false); }
		if (newLocDialog) { NGUITools.SetActive(newLocDialog.gameObject, false); }
		// wxj
		if (onekeyPurchaseDialog) { NGUITools.SetActive(onekeyPurchaseDialog.gameObject, false); }
		
		ChooseAtlasBasedOnScreenResolution(); // later on this may be moved to Awake()
		ResizeUIRoots();

		//TODO: 0.5s
		StartDownloadPrompts(false);
		
		promptForRateMyApp();
	}
	
	void Update()
	{
		if( Application.platform != RuntimePlatform.IPhonePlayer )
		{
			if( Input.GetKeyUp(KeyCode.Escape) )
			{
				escapeHandled = false;
				gameObject.BroadcastMessage("OnEscapeButtonClickedModel", SendMessageOptions.DontRequireReceiver);

				if( escapeHandled == false )
					gameObject.BroadcastMessage("OnEscapeButtonClicked", SendMessageOptions.DontRequireReceiver);
/*
				if( UIManagerOz.SharedInstance.NoModalDialogsCurrentlyShowing() )		// check if any other modal dialogs are active
				{
//						Debug.LogError("1");
					gameObject.BroadcastMessage("OnEscapeButtonClicked");
				}
				else
				{
//						Debug.LogError("2");
					gameObject.BroadcastMessage("OnEscapeButtonClickedModel");
				}
*/				
			}
		}
	}
		
	void OnEnvDownloadCheckDone(bool success)// called when the prompt for env download is done
	{
		NextPrompt();
	}

	void OnUIDownloadCheckDone(bool success)// called when the prompt for HiRes UI download is done
	{
		NextPrompt();
	}
	
	void OnLocalizationDownloadCheckDone(bool success)
	{// the localization budle should be ready now, or download failed?
		Localization.SharedInstance.SetLanguage();
		NextPrompt();
	}
	
	void RsesetAllPrefs()
	{
		PlayerPrefs.SetInt("DownloadPromptCounter", -1);
		PlayerPrefs.SetInt("DownloadDFCancelCtr", 0);
		PlayerPrefs.SetInt("DownloadUICancelCtr", 0);
		PlayerPrefs.SetInt("LastPromptedLevel", -1);
		PlayerPrefs.Save();
		
	}
	
	public void OnApplicationPause(bool pause)	
	{
//		notify.Debug("Paused = " + pause);
//		Debug.LogError ("Paused = " + pause);
		if( pause )
			return;

		if( IAPWrapper.isIAPCalled )
		{
//			notify.Debug("Burstly handled");œ
//			Debug.LogError("Burstly handled");
			IAPWrapper.isIAPCalled = false;
			return;
		}
		
	 
			
		if( isStarting )
			return;
		
		promptForRateMyApp();
		
		if( GameController.SharedInstance.IsSafeToLaunchDownloadDialog() )
		{
//			Debug.LogError ("Focus gained");
			StartDownloadPrompts(false);
		}
	}
	
	/// <summary>
	/// Prompts  the user to rate the app, with a time counter and launch count
	/// </summary>
	public void promptForRateMyApp()
	{
		if ( Application.internetReachability != NetworkReachability.NotReachable )
		{
			string title = Localization.SharedInstance.Get ("RATE_TITLE");
			string msg = Localization.SharedInstance.Get ("RATE_PROMPT");
			int hoursBetweenPrompts = Settings.GetInt("rate-app-hours-between-prompts",24);
			int launchMinimum = Settings.GetInt("rate-app-launch-minimum", 5);
 
		}		
	}
	
	public static bool IsPromptSequenceInProgress()
	{
		return SharedInstance.promptID != -1 ;
	}
	
	public void StartDownloadPrompts(bool forceDF = false, bool forceUI = false, bool noOKPrompt = false, GameObject msgobj = null)
	{
		if( promptID == -1 ) // check if not already in prompt sequence
		{
			downloadDoneMsgObject = msgobj;
				
			forceUIDownloadPrompt = forceUI;
			forceDFDownloadPrompt = forceDF;
			noDownloadOKPrompt = noOKPrompt;
			
			ampBundleManager.bundleVersioncheck = true;
			if( forceUIDownloadPrompt || forceDFDownloadPrompt)
				ampBundleManager.bundleVersioncheck = false;

			
			showCounter++;
//			Debug.LogError ("promptCounter is " + showCounter);
			PlayerPrefs.SetInt("DownloadPromptCounter", showCounter);
			PlayerPrefs.Save();
			
			NextPrompt();// start the sequence
		}
	}
	
	void OnProfileLoadCheckDone()
	{
		NextPrompt();
	}
		
	private bool HasDarkForestBeenPreviouslyDownloaded(string filename)
	{
		string[] flist = Directory.GetFiles(Application.persistentDataPath);
		
		if (flist == null)
		{
			notify.Debug("Bad Folder!!!!");
			return false;
		}
		
		foreach (string f in flist)
		{
			if (f != null && f.Contains(filename))
				return true;
		}
		
		return false;
	}
	
// this is the prompt sequencer, will be called when the focus is back or some special events
	void NextPrompt()
	{
		promptID++;
//			Debug.LogError ("promptCounter is " + showCounter);
//			Debug.LogError ("Prompting step " + promptID);
		
		notify.Debug("Prompt Step: " + promptID);
		
		switch(promptID)
		{
			case 0: // force init call on awake, but not if the user forced a UI or DF download
			
				notify.Debug( string.Format( "[UIManagerOz] Current Prompt: {0}.  HasShownOfferWall: {1}", promptID, _hasShownOfferWall ) );
			
				if (Initializer.SharedInstance != null && !Initializer.GetInitIsDownloading()
//					&& !forceUIDownloadPrompt && !forceDFDownloadPrompt
					&&Initializer.SharedInstance.IsTimeToGetInit()
				) {
					
					Initializer.SharedInstance.GetInitFromServer();
				
					notify.Warning("UIManager - Call Initializer");
				}
				NextPrompt();
			break;
			
			case 1 :// load font
				notify.Debug( string.Format( "[UIManagerOz] Current Prompt: {0}.  HasShownOfferWall: {1}", promptID, _hasShownOfferWall ) );
			
				string  bundleName = Localization.SharedInstance.GetAssetBundleName(Localization.SharedInstance.GetLangBySystem());
				if( bundleName != "")// need to download
				{
					if( ResourceManager.SharedInstance.IsAssetBundleDownloadedLastestVersion( bundleName ) )// latest downloaded
						ampBundleManager.DoBundleVersionCheck();// this is asyn call and may return any time, the prompt seq won't wait for it, so the new ver number may only take effect next time the prompt seq starts
					if( UIDownloadDialogLocal.isCheckDone == false)
						downloadLocalDialog.StartPrompt(gameObject, bundleName);
					else
						OnLocalizationDownloadCheckDone(false);// just update font
				}
				else
				{
					OnLocalizationDownloadCheckDone(true);// just update font
				}
			break;
			
			case 2:
				notify.Debug( string.Format( "[UIManagerOz] Current Prompt: {0}.  HasShownOfferWall: {1}", promptID, _hasShownOfferWall ) );
			
				if ( Application.internetReachability != NetworkReachability.NotReachable && !_hasShownOfferWall )
				{
					if( DownloadManagerUI.NeedHiresUI() && !forceDFDownloadPrompt)
					{
						if (ResourceManager.SharedInstance.IsAssetBundleDownloaded( DownloadManagerUI.bundleName ) )
						{// only check if already downloaded
							ampBundleManager.DoBundleVersionCheck();// this is asyn call and may return any time, the prompt seq won't wait for it, so the new ver number may only take effect next time the prompt seq starts
							downloadUIDialog.StartPrompt(gameObject, true, noDownloadOKPrompt);// just load it
						}else
						if( forceUIDownloadPrompt ) // force it!!
						{
							downloadUIDialog.StartPrompt(gameObject, true, noDownloadOKPrompt);
						}else
						if( (showCounter % hiPromptInterval == 0 ) )
						{
							downloadUIDialog.StartPrompt(gameObject, false, noDownloadOKPrompt);
						}else
						{
							NextPrompt();
						}
					}
					else
					{
						NextPrompt();
					}
				}
				else
				{
					if (forceUIDownloadPrompt)	// only show this dialog window if it was forced from 'World of Oz' page
					{
						UIManagerOz.SharedInstance.okayDialog.ShowOkayDialog("Msg_NeedConnect", "Btn_Ok", false);	
					}
				
					NextPrompt();
				}				
			break;
			
			case 3 : 
				notify.Debug( string.Format( "[UIManagerOz] Current Prompt: {0}.  HasShownOfferWall: {1}", promptID, _hasShownOfferWall ) );
			
				if (HasDarkForestBeenPreviouslyDownloaded("darkforest") && 
					!DownloadManager.HaveAllLocationsBeenDownloaded() && 
					!firstPassOnUpdateDialogCompleted)
				{
					if (Application.internetReachability != NetworkReachability.NotReachable)
					{
						updateDialog.StartPrompt(gameObject, true, true);
					}
					else
					{
						updateNoDialog.StartPrompt(gameObject);
					}
				}
				else
				{
					if (Application.internetReachability != NetworkReachability.NotReachable)
					{
						if( DownloadManager.HaveAllLocationsBeenDownloaded() )
						{	
							ampBundleManager.DoBundleVersionCheck();// this is asyn call and may return any time, the prompt seq won't wait for it, so the new ver number may only take effect next time the prompt seq starts
						}
						if( showCounter != 0 && !_hasShownOfferWall ) // skip the first time
						{
							if( !DownloadManager.HaveAllLocationsBeenDownloaded() && !forceUIDownloadPrompt )
							{
								if(forceDFDownloadPrompt)
								{
									downloadDialog.StartPrompt(gameObject, true, noDownloadOKPrompt);
								}else if( showCounter == 1 )// always show when the second time prompt sequence
								{
									downloadDialog.StartPrompt(gameObject, false, noDownloadOKPrompt);
								}
								else if( showCounter % hiPromptInterval == 0 )
								{
									downloadDialog.StartPrompt(gameObject, false, noDownloadOKPrompt);
								}
								else
								{
									NextPrompt();
								}
							}
							else
							{
								NextPrompt();
							}
						}
						else
						{
							NextPrompt();
						}
					}
					else
					{
						if (forceDFDownloadPrompt)	// only show this dialog window if it was forced from 'World of Oz' page
						{
							UIManagerOz.SharedInstance.okayDialog.ShowOkayDialog("Msg_NeedConnect", "Btn_Ok", false);	
						}
						NextPrompt();
					}					
				}
			break;

			case 4 :
				notify.Debug( string.Format( "[UIManagerOz] Current Prompt: {0}.  HasShownOfferWall: {1}", promptID, _hasShownOfferWall ) );
			
			/*
				bool profileRestore = Settings.GetBool("allow-profile-restore", false);
			
				bool rankOneProfileRestore = Settings.GetBool( "allow-rank-one-profile-restore", false );
			
				notify.Debug("UI Manager: Profile Restore State");
				
				if (profileRestore 
					&& ProfileManager.SharedInstance != null
					&& ProfileManager.SharedInstance.lastServerData != null
				) {
					notify.Debug("UI Manager Profile Restore: ");
				
					ProfileManager.SharedInstance.restoreProfile(gameObject);
				}
				else if (rankOneProfileRestore 
					&& ProfileManager.SharedInstance != null
					&& ProfileManager.SharedInstance.lastServerData != null
				) {
					notify.Debug(" [UIManagerOz] - NextPrompt() Rank One Profile Restore called" );
				
					ProfileManager.SharedInstance.restoreProfile( gameObject );
				}
				else
				{
					NextPrompt();
				}
			*/
				NextPrompt();
			break;	
			case 5 :
				notify.Debug( string.Format( "[UIManagerOz] Current Prompt: {0}.  HasShownOfferWall: {1}", promptID, _hasShownOfferWall ) );
			
				promptID = -1;// mark the end of sequence
				if( isStarting )
				{
//					if( bundleVersioncheck )
			//			DoBundleVersionCheck();
					GameController.SharedInstance.SetMainMenuAnimation();
					isStarting = false;
//					Debug.LogError ("111111111");
 		
				}
			
				_hasShownOfferWall = false;
			
				if( downloadDoneMsgObject )
				{
					downloadDoneMsgObject.SendMessage("OnPromptSequenceDone");
					downloadDoneMsgObject = null;
				}
			
				firstPassOnUpdateDialogCompleted = true;	// this is so update dialog never comes up during gameplay, only during initial launch
			break;
		}
	}
	
	private void InstantiateViewControllers()
	{
		// paper vc is used by just about everything else, so it comes here first
		this.PaperVC = GetInstantiatedObject<UIPaperViewControllerOz>();
		this.mainVC = GetInstantiatedObject<UIMainMenuViewControllerOz>();
		this.idolMenuVC = GetInstantiatedObject<UIIdolMenuViewControllerOz>();
		this.postGameVC = GetInstantiatedObject<UIPostGameViewControllerOz>();
		this.inGameVC = GetInstantiatedObject<UIInGameViewControllerOz>();
		this.inventoryVC = GetInstantiatedObject<UIInventoryViewControllerOz>();
		this.IAPMiniStoreVC = GetInstantiatedObject<UIIAPMiniViewControllerOz>();		
		this.ObjectivesVC = GetInstantiatedObject<UIObjectivesViewControllerOz>();
		this.settingsVC = GetInstantiatedObject<UISettingsViewControllerOz>();
		this.statsVC = GetInstantiatedObject<UIStatViewControllerOz>();
		this.leaderboardVC = GetInstantiatedObject<UILeaderboardViewControllerOz>();

		this.gatchVC = GetInstantiatedObject<UIGatchaViewControllerOz>();
		this.worldOfOzVC = GetInstantiatedObject<UIWorldOfOzViewControllerOz>();
		
		// non view-controllers (modal dialogs)
		this.confirmDialog = GetInstantiatedObject<UIConfirmDialogOz>();
		this.okayDialog = GetInstantiatedObject<UIOkayDialogOz>();
		this.rewardDialog = GetInstantiatedObject<UIRewardDialogOz>();	
		this.downloadDialog = GetInstantiatedObject<UIDownloadDialogOz>();
		this.downloadUIDialog = GetInstantiatedObject<UIDownloadDialogUI>(); 
		this.downloadLocalDialog = GetInstantiatedObject<UIDownloadDialogLocal>(); 
		this.facebookLoginDialog = GetInstantiatedObject<UIFacebookDialogOz>(); 
		this.updateDialog = GetInstantiatedObject<UIUpdateDialogOz>(); 
		this.updateNoDialog = GetInstantiatedObject<UIUpdateNoDialogOz>(); 
		this.newLocDialog = GetInstantiatedObject<UINewLocDialogOz>();
		
		// wxj
		this.onekeyPurchaseDialog = GetInstantiatedObject<UIConfirmDialogOz>();
	}
	
	/// <summary>
	/// Gets the field info, for the specified type, may return null if it is null
	/// </summary>
	/// <returns>
	/// The field info, may be null if not there, undefined if we have two fields that have the same type, you can get either one
	/// </returns>
	/// <typeparam name='T'>
	/// This will be typically types like UIStoreViewControllerOz, or UIPaperViewControllerOz
	/// </typeparam>
	private FieldInfo GetFieldInfo<T>()
	{
		FieldInfo result = null;
       // try
        {
            // Get the type handle of a specified class.
            System.Type myType = this.GetType();
			FieldInfo[] myFieldInfo;
	         myFieldInfo = myType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
	        //notify.Debug("\nThe fields of " + "FieldInfoClass are \n");
	        // Display the field information of FieldInfoClass. 
			
	        for(int i = 0; i < myFieldInfo.Length; i++)
	        {
	            //notify.Debug("\nName            : " + myFieldInfo[i].Name);
	            //notify.Debug("Declaring Type  : " + myFieldInfo[i].DeclaringType);
	            //notify.Debug("IsPublic        : " + myFieldInfo[i].IsPublic);
	            //notify.Debug("MemberType      : " + myFieldInfo[i].MemberType);
	            //notify.Debug("FieldType       : " + myFieldInfo[i].FieldType);
	            //notify.Debug("IsFamily        : " + myFieldInfo[i].IsFamily);
				//notify.Debug("CurValue        : " + myFieldInfo[i].GetValue(this));
				
				if (myFieldInfo[i].FieldType == typeof(T))
				{
					// wxj
					//if(myFieldInfo[i].FieldType == typeof(UIArtifactDialogOz))
					//{
					//	break;
					//}
					result = myFieldInfo[i];
					break;
				}
	        }
        }
        //catch(System.Exception e)
        //{
        //    notify.Debug("Exception : " + e.Message);
        //}
		return result;
	}
	
	/// <summary>
	/// Returns an instantiated game object of the given type, might return null
	/// </summary>
	/// <returns>
	/// if non null, an instantiated game object, if null then there wasn't a matching prefab
	/// </returns>
	/// <typeparam name='T'>
	/// The 1st type parameter.
	/// </typeparam>
	public T GetInstantiatedObject<T>() where T : UnityEngine.Component
	{
		T result = default(T);
		Object tempResult;
		if (type2InstantiatedObject.TryGetValue(typeof(T), out tempResult))
		{
			result = tempResult as T;
			return result;
		}
		
		// not in our quick lookup dictionary, see if the field was linked up manually in the scene
		FieldInfo fi = GetFieldInfo<T>();
		
		if (fi != null)
		{
			Object obj = fi.GetValue(this) as Object;
			
			if (obj != null)			// if connected through editor
			{
				// add it to our lookup dict
				result = obj as T;
				type2InstantiatedObject[typeof(T)] = result;
			
				ConnectToMainUIRootAndCamera(result.gameObject);
			}
			else 
			{
				T[] scripts = gameObject.GetComponentsInChildren<T>(true);
				
				if (scripts.Length > 0)	// if child in scene
				{
					// add it to our lookup dict
					result = scripts[0] as T;
					type2InstantiatedObject[typeof(T)] = result;
				
					ConnectToMainUIRootAndCamera(result.gameObject);
				}
				else				
				{
					// not in the dict, not hooked up in the scene, and not found through search, load it from resources
					string fullPath = "Oz/Prefabs/" + typeof(T).Name;
					
					
					//notify.Debug("prefab path " + fullPath);
					GameObject prefab = Resources.Load( fullPath, typeof(GameObject)) as GameObject;
					
					if (prefab != null)
					{
						GameObject instantiated = GameObject.Instantiate(prefab) as GameObject;	
						UnityEngine.Debug.Log("~~~!#######"+instantiated.ToString());
						result = instantiated.GetComponent<T>();
						
						type2InstantiatedObject[typeof(T)] = result;
						fi.SetValue(this, result);
						
						if (result != null)
							result.transform.parent = this.transform;	
						
						ConnectToMainUIRootAndCamera(instantiated);
					}
					else
						notify.Error("couldn't load prefab " + fullPath);	
				}
			}
		}
		else
			notify.Error ("UIManagerOz " + "doesn't have a field of type " + typeof(T).Name);	
			
		return result;
	}
	
	private void ConnectToMainUIRootAndCamera(GameObject viewControllerGO)
	{
		//if (viewControllerGO.transform.Find("Camera") != null)
		//{
			viewControllerGO.GetComponent<UIRoot>().enabled = false;
			viewControllerGO.transform.Find("Camera").gameObject.GetComponent<UICamera>().enabled = false;
			viewControllerGO.transform.Find("Camera").gameObject.GetComponent<Camera>().enabled = false;
			viewControllerGO.transform.localPosition = Vector3.zero;
			viewControllerGO.transform.localEulerAngles = Vector3.zero;
			viewControllerGO.transform.localScale = Vector3.one;
		//}
	}
	
	public enum UIResolutionType
	{
		kResolution480 = 0,
		kResolution960 = 1,
		kResolution1024 = 2,
		kResolution1136 = 3,
		kResolution2048 = 4,
		kResolutionCount
	}
	
	public UIAtlas InterfaceMaster;
	public UIAtlas InterfaceMasterOpaque;
	public UIAtlas InterfaceMaster2;
	public List<string> InterfaceMasterReplacements = new List<string>();
	public List<string> InterfaceMasterReplacementsOpaque = new List<string>();
	public List<string> InterfaceMasterReplacements2 = new List<string>();
	
	public static bool UseLowResUI()
	{
		if( Screen.height <= 480)
		{
			return true;
		}
		
#if UNITY_ANDROID
		if(SystemInfo.systemMemorySize < 300)
		{
			return true;
		}
		
		if ( GameController.IsDeviceLowEnd() == true && GetAndroidDisplayDensityDPI() > 160 )
		{
			// Low-end, high-DPI device
			return true;
		}
#endif 


#if UNITY_IPHONE
		if (GameController.SharedInstance != null)
		{
			GameController.DeviceGeneration device = GameController.SharedInstance.GetDeviceGeneration();
	
			switch( device )
			{
				case GameController.DeviceGeneration.Unsupported :
				case GameController.DeviceGeneration.iPodTouch3 :
				case GameController.DeviceGeneration.iPhone3GS :
				case GameController.DeviceGeneration.iPodTouch4 :
					return true;
			}
		}
#endif 

		return false;
	}

	public void ChooseAtlasBasedOnScreenResolution() 
	{
		notify.Debug("ChooseAtlasBasedOnScreenResolution " );
		/*GameObject prefab = Resources.Load ("Oz/interface/InterfaceMasterOz", typeof(GameObject)) as GameObject;
		GameObject.Instantiate(prefab);
		
		InterfaceMaster = prefab.GetComponent<UIAtlas>();
		
		InterfaceMasterReplacements.Add("Oz/interface/InterfaceMasterOz_512");
		InterfaceMasterReplacements.Add("Oz/interface/InterfaceMasterOz_1024");
		InterfaceMasterReplacements.Add("Oz/interface/InterfaceMasterOz_1024");
		InterfaceMasterReplacements.Add("Oz/interface/InterfaceMasterOz_2048");
		InterfaceMasterReplacements.Add("Oz/interface/InterfaceMasterOz_2048");*/
		
		if (InterfaceMaster != null) 
		{
			UIResolutionType chosenType = UIResolutionType.kResolution960;
			if(UseLowResUI ())
			{//force lo res on low memory devices
				chosenType = UIResolutionType.kResolution480;
			}
			else
			{
				switch(Screen.height) 
				{
				case 480:
					chosenType = UIResolutionType.kResolution480;
					break;
				case 960:
//					if(GameController.SharedInstance.GetDeviceGeneration() == GameController.DeviceGeneration.iPhone4)
//					{//down res textures for iPhone 4 because it is fill bound
//						chosenType = UIResolutionType.kResolution480;
//					}
//					else
					{
						chosenType = UIResolutionType.kResolution960;
					}
					break;
				case 1024:
					chosenType = UIResolutionType.kResolution1024;
					break;
				case 1136:
					chosenType = UIResolutionType.kResolution1136;
					break;
				case 2048:
					chosenType = UIResolutionType.kResolution2048;
					break;
				default:
					if(Screen.height > 2048) { 
						chosenType = UIResolutionType.kResolution2048; 
					} 
					else if (Screen.height >= 1136)
					{
						// this bit makes the samsung s3 get the hi res assets
						chosenType = UIResolutionType.kResolution1136;
					}
					break;
				}
			}
			
			// at dave's request force high rez ui for ipad2 
			if (GameController.SharedInstance.GetDeviceGeneration() == GameController.DeviceGeneration.iPad2 )
			{
				chosenType = UIResolutionType.kResolution2048;
			}
			
			//hiresresources/InterfaceMasterOz_HIGH_Prefab
//			chosenType = UIResolutionType.kResolution2048;  //for testing hi res only

// check if overriden by user's quality setting
			deviceSupportsHiRes = (chosenType == UIResolutionType.kResolution2048 || chosenType == UIResolutionType.kResolution1136);

			if( GameController.userSelectedQuality == 0 )// low quality selected
			{
				if( deviceSupportsHiRes )
				{// just use medium
					chosenType = UIResolutionType.kResolution1024;
					notify.Debug("Force to medium res");
				}
			}
			
			chosenUIResType = chosenType;// save it
			
			int choosenTypeIdx = (int)chosenType;
			if (InterfaceMasterReplacements != null && InterfaceMasterReplacements.Count > choosenTypeIdx && InterfaceMasterReplacements2.Count > choosenTypeIdx) 
			{
				string fileName = InterfaceMasterReplacements[choosenTypeIdx];
				string fileNameOp = InterfaceMasterReplacementsOpaque[choosenTypeIdx];
				string fileName2 = InterfaceMasterReplacements2[choosenTypeIdx];
				notify.Debug ("filename = " + fileName + " filename 2 = " + fileName2);
				
				GameObject go;
				GameObject go2;
				GameObject goOpaque;

				if( fileName.Contains(DownloadManagerUI.bundleName) && 
					ResourceManager.SharedInstance.downloadedAssetBundles.IsAssetBundleDownloaded( DownloadManagerUI.bundleName ))
				{
					go = ResourceManager.Load(fileName, typeof(GameObject)) as GameObject;
					goOpaque = ResourceManager.Load(fileNameOp, typeof(GameObject)) as GameObject;
					go2 = ResourceManager.Load(fileName2, typeof(GameObject)) as GameObject;
					notify.Debug("Load straight from hires bundle");
				}
				else
				{
					go = Resources.Load(fileName, typeof(GameObject)) as GameObject;
					goOpaque = Resources.Load(fileNameOp, typeof(GameObject)) as GameObject;
					go2 = Resources.Load(fileName2, typeof(GameObject)) as GameObject;
				}
				
				if( go != null && go2 != null && goOpaque != null)
				{// found in the resouce
					notify.Debug ("UI res load from resource = " + go);
					ChangeUISet(go, goOpaque, go2, choosenTypeIdx);
					deviceSupportsHiRes = false;
				}
				else
				{
					notify.Debug ("UI res load from asset bundle = " + fileName);

					deviceSupportsHiRes = true;
					DownloadManagerUI.loadUIName = System.IO.Path.GetFileName(fileName);
					DownloadManagerUI.loadUINameOpaque = System.IO.Path.GetFileName(fileNameOp);
					DownloadManagerUI.loadUIName2 = System.IO.Path.GetFileName(fileName2);
					
					// go get asset bundle
					
					DownloadManagerUI.needUIBundle = true; // set a flag so when prompted will trigger the download

					choosenTypeIdx--;
					
					// find first local avaiable first
					while( choosenTypeIdx >= 0 )
					{
						fileName = InterfaceMasterReplacements[choosenTypeIdx];
						fileNameOp = InterfaceMasterReplacementsOpaque[choosenTypeIdx];
						fileName2 = InterfaceMasterReplacements2[choosenTypeIdx];
						go = Resources.Load(fileName, typeof(GameObject)) as GameObject;
						goOpaque = Resources.Load(fileNameOp, typeof(GameObject)) as GameObject;
						go2 = Resources.Load(fileName2, typeof(GameObject)) as GameObject;
						if( go != null && go2 != null && goOpaque != null)
						{// found in the resouce
							notify.Debug ("go = " + go);
							ChangeUISet(go, goOpaque, go2, choosenTypeIdx);
							break;
						}
						choosenTypeIdx--;
					}
				}
			}
		}
	}
	
	public static int GetAndroidDisplayDensityDPI()
	{
		// Returns one of the following values:
		// -1 (error)
		// "120" (low density)
		// "160" (medium density)
		// "213" (TV density) 
		// "240" (high density)
		// "320" (extra-high density)
		// "480" (extra-extra-high density)
		
		int densityDPI = -1;
#if UNITY_ANDROID
		AndroidJavaClass cls_TempleRunOz = new AndroidJavaClass( "com.disney.troz.TempleRunOzActivity" );
		if ( cls_TempleRunOz != null )
		{
			AndroidJavaObject objActivity = cls_TempleRunOz.CallStatic<AndroidJavaObject>( "getInstance" );
			if ( objActivity != null )
			{
				AndroidJavaObject metricsInstance = new AndroidJavaObject( "android.util.DisplayMetrics" );
				if ( metricsInstance != null )
				{
					AndroidJavaObject windowManagerInstance = objActivity.Call<AndroidJavaObject>( "getWindowManager" );
					if ( windowManagerInstance != null )
					{
          				AndroidJavaObject displayInstance = windowManagerInstance.Call<AndroidJavaObject>( "getDefaultDisplay" );
						if ( displayInstance != null )
						{
							displayInstance.Call( "getMetrics", metricsInstance );
							densityDPI = metricsInstance.Get<int>( "densityDpi" );
						}
					}
				}
			}
		}
#endif
		return densityDPI;
	}
	
	public void ChangeUISet(GameObject master, GameObject masteropaque, GameObject master2, int level = -1)
	{
//		Debug.LogError("UI set to " + master.name);
		InterfaceMaster.replacement = master.GetComponent<UIAtlas>();
		InterfaceMasterOpaque.replacement = masteropaque.GetComponent<UIAtlas>();
		InterfaceMaster2.replacement = master2.GetComponent<UIAtlas>();
		
		if (level != -1)
			chosenUIResType = (UIResolutionType)level;	// update UI resolution level when setting atlases
	}
	
	void ResizeUIRoots()
	{
		notify.Debug ("ResizeUIRoots screen.height={0}", Screen.height);
		if (Screen.height == 1136)
		{
			foreach(UIRoot root in UIRoot.list) 
			{
				if (root == null) { continue; }
				notify.Debug("Setting root height to 1136 for {0}", root);
				root.manualHeight = Screen.height;
			}
		}
	}

	public void EndGame() 
	{
		if (inGameVC) 
			inGameVC.resurrectMenu.StartResurrectTimer(); 
	}
	
	public void SetPowerProgress(float progress) 
	{
		if (inGameVC == null) { return; }
		inGameVC.coinMeter.SetPowerProgress(progress);	//inGameVC.SetPowerProgress(progress);
	}

	public void SetDistanceTotal(int distanceTotal)
	{
		if (inGameVC == null) { return; }
		inGameVC.SetDistanceTotal(distanceTotal);
	}

	// Message Callbacks:  They don't have params coming form NGUI, so we make specific methods
	
	public void disableResurrectMenu() 
	{
		if (UIResurrectMenu != null)
			NGUITools.SetActive(UIResurrectMenu, false);
	}
	
	public void enableResurrectMenu()
	{
		if (UIResurrectMenu != null)
			NGUITools.SetActive(UIResurrectMenu, true);
	}

	public void OnPlayClicked()
	{		
		if (!inGameVC.gameObject.active)		
		{			
			// run garbage collection if 'play' clicked from menus, but not on idol screen (unless low end iOS device)
//			if (!UIManagerOz.SharedInstance.idolMenuVC.enabled || GameController.SharedInstance.Is256MBiOSDevice())
//			{
//				Resources.UnloadUnusedAssets();		
//				System.GC.Collect();
//			}
			
			UIManagerOz.SharedInstance.PaperVC.goBackToIdolMenu = false;// main menu back button will go back to post-run from now on, not idol menu
			
			//if (MainGameCamera == null || Camera.main == null) 
			//	GameCamera.SharedInstance.GetComponent<Camera>();		//MainGameCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
			
			//MainGameCamera.enabled = true;							// turn on main camera
			
			if(GamePlayer.SharedInstance.Sunlight!=null)
			{
				GamePlayer.SharedInstance.Sunlight.EnableShadow(true);
			}
			/*if(GamePlayer.SharedInstance.ShadowCamera != null)
			{
				if (QualitySettings.GetQualityLevel() >= 1)				// turn shadow camera on for higher quality settings
				{
					GamePlayer.SharedInstance.ShadowCamera.enabled = true;
				}
				else
				{
					GameObject camObject = GamePlayer.SharedInstance.ShadowCamera.gameObject;
					GamePlayer.SharedInstance.ShadowCamera = null;
					Destroy(camObject);
				}
			}*/

			Services.Get<ObjectivesManager>().BackUpWeeklyChallengeProgressForAnimationsNextTime();
			
			leaderboardVC.CancelProfilePhotoDownloads();	// cancel any in-progress profile photo downloads
			
			HideAllViewControllersExceptInGameAndIdol();							// hide this view controller
			UIManagerOz.SharedInstance.SetUICameraClearFlagToSolidColorBG(false);	// switch UI camera background color off			
			inGameVC.appear();		 												// bring in in-game view controller
			
			if (onPlayClickedHandler != null)
				onPlayClickedHandler();												//-- Notify an object that is listening for this event.
			
			//GameController.SharedInstance.HideStartingTemple();
			MainGameCamera.enabled = true;
		}
	}			
	
	public void GoToMiniStore(ShopScreenName pageToLoad, bool comingFromResurrectMenu)	// used only for going directly to coins/gems pages in ways other than clicking on the store button
	{
		//if (storeItemList == "gems")	// coming from resurrect menu
		//	IAPMiniStoreVC.isResurrectMenu = true;
		
		IAPMiniStoreVC.pageToLoadIfMoreSpecificNeeded = pageToLoad;	//storeItemList;
		IAPMiniStoreVC.comingFromResurrectMenu = comingFromResurrectMenu;
		IAPMiniStoreVC.appear();
	}
	
	private void HideAllViewControllersExceptInGameAndIdol()
	{
		HideIfShowing(PaperVC);
		HideIfShowing(mainVC);
		//HideIfShowing(idolMenuVC);
		HideIfShowing(postGameVC);		
		HideIfShowing(inventoryVC);		
		HideIfShowing(IAPMiniStoreVC);		
		HideIfShowing(ObjectivesVC);		
		HideIfShowing(settingsVC);		
		HideIfShowing(statsVC);		
		HideIfShowing(leaderboardVC);		
		HideIfShowing(gatchVC);		
		HideIfShowing(worldOfOzVC);	
	}
	
	private void HideIfShowing(UIViewControllerOz vc)
	{
		//if (vc && vc.gameObject.GetComponent<UIPanelAlpha>().alpha == 1f)
		if (vc && vc.gameObject.active)
			vc.disappear();		
	}
	
	public void ShowFriendScoreLabel(string name){
		float dist = 0f;
		TrackPiece tp = GamePlayer.SharedInstance.OnTrackPiece;
		bool marching = true;
		while(marching){ // keep going forward until we get to the right spot
			TrackPiece ntp = tp.NextTrackPiece;
			if(ntp){
				dist += ntp.GeneratedPathLength;
				if(dist > 30f){
					marching = false;
				}
				tp = ntp;
			}
			else{
				marching = false;
			}
		}
		
		TrackPiece prevTrack = tp.PreviousTrackPiece;
		
		Vector3 pos = prevTrack.GeneratedPath[prevTrack.GeneratedPath.Count-1];
		Vector3 dir = (prevTrack.GeneratedPath[prevTrack.GeneratedPath.Count-2] - pos).normalized;
		
		
		passFriend.gameObject.SetActiveRecursively(true);
		passFriend.SetText(name);
		StartCoroutine( passFriend.SetPosition(pos, dir));
	}
	
	public bool NoModalDialogsCurrentlyShowing()
	{
		if (activeModals.Count > 0)
		{
/*			foreach(UIModalDialogOz mui in  activeModals)
			{
				Debug.LogError (mui.name);
			}
*/			
			return false;
		}
		else 
			return true;
	}
	
	public void AddToActiveList(UIModalDialogOz modalDialog)
	{
//		Debug.Log("+++++++Adding" + modalDialog.name);
		if (!activeModals.Contains(modalDialog))
			activeModals.Add(modalDialog);
	}
	
	public void RemoveFromActiveList(UIModalDialogOz modalDialog)
	{
//		Debug.Log("---------REmoving" + modalDialog.name);
		if (activeModals.Contains(modalDialog))
			activeModals.Remove(modalDialog);		
	}
	
	// quick and dirty way to hide a none moving UI item, make sure don't hide it twice
	public static void HideUIItem(GameObject item, bool t)
	{
		item.SetActiveRecursively(!t);
/*		
		ItemHider hider = item.GetComponent<ItemHider>();
		if( hider == null )
		{
			hider = (ItemHider) (item.AddComponent("ItemHider"));
		}
		
		hider.Hide(t);
		*/
	}
	
	public void SetUICameraClearFlagToSolidColorBG(bool solid)
	{
		if (solid)
		{
			UICamera.clearFlags = CameraClearFlags.SolidColor;
			UICamera.backgroundColor = new Color(120f/255f, 100f/255f, 60f/255f, 1f);
		}
		else
			UICamera.clearFlags = CameraClearFlags.Depth;
	}	
	
	/// <summary>
	/// Gets the user interface size value.
	/// </summary>
	/// <returns>
	/// The user interface size value.  1 = high res, 2 = med res, 3 = low res
	/// </returns>
	public int GetUISizeVal()
	{
		if (InterfaceMasterReplacements[(int)chosenUIResType].Contains("HIGH"))
		{
			return 1;	
		}
		if (InterfaceMasterReplacements[(int)chosenUIResType].Contains("MED"))
		{
			return 2;	
		}
		if (InterfaceMasterReplacements[(int)chosenUIResType].Contains("SMALL"))
		{
			return 3;	
		}		
	
		return 0;	// not found, error
	}
}



/*	
	public UIResolutionType GetChosenTypeFromQualitySelection(GameController.DeviceGeneration dg )
	{
		UIResolutionType chosenType = UIResolutionType.kResolution960;
		
		switch(dg)
		{
			case GameController.DeviceGeneration.LowEnd:
			{
				chosenType = UIResolutionType.kResolution480;
				// set to the lowest
			}
			break;
			case GameController.DeviceGeneration.MedEnd:
			{
				chosenType = UIResolutionType.kResolution1024;
			}
			break;
			case GameController.DeviceGeneration.HighEnd:
			{
				if( deviceSupportsHiRes )
					chosenType = UIResolutionType.kResolution2048;
				else
					chosenType = UIResolutionType.kResolution1024;
			}
			break;
		}
		return chosenType;
	}
	
	public void ChooseAtlasBasedOnDevice( GameController.DeviceGeneration dg )
	{
		UIResolutionType chosenType = GetChosenTypeFromQualitySelection(dg);

		int choosenTypeIdx = (int)chosenType;
		
		
		if (InterfaceMasterReplacements != null && InterfaceMasterReplacements.Count > choosenTypeIdx && InterfaceMasterReplacements2.Count > choosenTypeIdx) 
		{
			
			if( InterfaceMasterReplacements[(int)chosenUIResType] == InterfaceMasterReplacements[choosenTypeIdx] ) return ;// no change
			
			chosenUIResType = chosenType;
		
			string fileName = InterfaceMasterReplacements[choosenTypeIdx];
			string fileNameOp = InterfaceMasterReplacementsOpaque[choosenTypeIdx];
			string fileName2 = InterfaceMasterReplacements2[choosenTypeIdx];
			
			notify.Debug ("filename = " + fileName + " filename 2 = " + fileName2);
			
			GameObject go = ResourceManager.Load(fileName, typeof(GameObject)) as GameObject;
			GameObject goOpaque = ResourceManager.Load(fileNameOp, typeof(GameObject)) as GameObject;
			GameObject go2 = ResourceManager.Load(fileName2, typeof(GameObject)) as GameObject;
			
			if( go != null && go2 != null && goOpaque != null )
			{
					UIManagerOz.SharedInstance.ChangeUISet(go, goOpaque, go2);
			}
		}		
	}
*/	
	
			//HideIfShowing(mapVC);
		//HideIfShowing(aboutVC);		
		//HideIfShowing(characterSelectVC);	
		//HideIfShowing(IAPStoreVC);		
			//HideIfShowing(moreGamesVC);		
	
		
	
	//public UICharacterSelectViewControllerOz characterSelectVC;
	//public UIMapViewControllerOz mapVC;
	//public UIAboutViewControllerOz aboutVC;	
	//public UIIAPViewControllerOz IAPStoreVC;
	//public UIMoreGamesViewControllerOz moreGamesVC;	
	//public UINatViewControllerOz natVC;
	//public UIResurrectMenuOz UIResurrectMenuVC;
			
		//if (mapVC) { mapVC.Hide(mapVC.gameObject); }	//.disappear(); }
		//if (aboutVC) { aboutVC.Hide(aboutVC.gameObject); }	//.disappear(); }		
		//if (moreGamesVC) { moreGamesVC.Hide(moreGamesVC.gameObject); }	//.disappear(); }
		//if (IAPStoreVC) { IAPStoreVC.Hide(IAPStoreVC.gameObject); }	//.disappear(); }
		//if (characterSelectVC) { characterSelectVC.Hide(characterSelectVC.gameObject); }	//.disappear();	}
		//if (natVC) { natVC.Hide(natVC.gameObject); }	//.disappear();}		
		//if (rewardDialog) { NGUITools.SetActive(rewardDialog.gameObject, false); }
		
		
			//this.mapVC = GetInstantiatedObject<UIMapViewControllerOz>();
		//this.aboutVC = GetInstantiatedObject<UIAboutViewControllerOz>();
		//this.characterSelectVC = GetInstantiatedObject<UICharacterSelectViewControllerOz>();
		//this.storeVC = GetInstantiatedObject<UIStoreViewControllerOz>();		
		//this.IAPStoreVC = GetInstantiatedObject<UIIAPViewControllerOz>();
			//this.moreGamesVC = GetInstantiatedObject<UIMoreGamesViewControllerOz>();
			//this.UIResurrectMenuVC = GetInstantiatedObject<UIResurrectMenuOz>();
		//this.UIResurrectMenu = UIResurrectMenuVC.gameObject;
			//this.natVC = GetInstantiatedObject<UINatViewControllerOz>();



//		if (confirmDialog.gameObject.active)
//			return false;
//		
//		if (okayDialog.gameObject.active)
//			return false;
//		
//		if (rewardDialog.gameObject.active)
//			return false;

	
	//START of old code that from when this was still inheriting from the Imangi UIManager
//	public void ShowObject(GameObject theObject, bool show, bool recursive)
//	{
//		if (theObject == null) { return; }
//		if (recursive == true)
//			theObject.SetActiveRecursively(show);
//		else
//			theObject.active = show;
//	}
	//END of old code that from when this was still inheriting from the Imangi UIManager


		
//		if (PaperVC && PaperVC.gameObject.active) { PaperVC.disappear(); }
//		if (mainVC && mainVC.gameObject.active) { mainVC.disappear(); }
//		if (mapVC && mapVC.gameObject.active) { mapVC.disappear(); }
//		if (aboutVC && aboutVC.gameObject.active) { aboutVC.disappear(); }
//		if (characterSelectVC && characterSelectVC.gameObject.active) { characterSelectVC.disappear(); }
//		if (idolMenuVC && idolMenuVC.gameObject.active) { idolMenuVC.disappear(); }	
//		if (postGameVC && postGameVC.gameObject.active) { postGameVC.disappear(); }
//		if (inventoryVC && inventoryVC.gameObject.active) { inventoryVC.disappear(); }
//		if (IAPStoreVC && IAPStoreVC.gameObject.active) { IAPStoreVC.disappear(); }
//		if (IAPMiniStoreVC && IAPMiniStoreVC.gameObject.active) { IAPMiniStoreVC.disappear(); }
//		if (ObjectivesVC && ObjectivesVC.gameObject.active) { ObjectivesVC.disappear(); }
//		if (moreGamesVC && moreGamesVC.gameObject.active) { moreGamesVC.disappear(); }
//		if (settingsVC && settingsVC.gameObject.active) { settingsVC.disappear(); }
//		if (statsVC && statsVC.gameObject.active) { statsVC.disappear(); }
//		if (leaderboardVC && leaderboardVC.gameObject.active) { leaderboardVC.disappear(); }
//		if (gatchVC && gatchVC.gameObject.active) { gatchVC.disappear(); }

	
//	public void ShowConfirmDialog(string title, string description, string negativeButtonText, string positiveButtonText) 
//	{
//		if (confirmDialog == null) { return; }
//		confirmDialog.ShowConfirmDialog(title, description, negativeButtonText, positiveButtonText);
//	}
//	
//	public void ShowOkayDialog(string title, string description, string positiveButtonText) 
//	{
//		if (okayDialog == null) { return; }
//		okayDialog.ShowOkayDialog(title, description, positiveButtonText);
//	}
	

		//if (PaperVC)
		//	NGUITools.SetActive(PaperVC.gameObject, false);
		

	
//	public void ShowIdolMenu()
//	{
//		if (idolMenuVC)
//			idolMenuVC.appear();
//	}
	
//	public void BringInIdolMenu()
//	{
//		if (idolMenuVC)
//			idolMenuVC.BringInIdolMenu();
//	}	



//			if (idolMenuVC)		// hide idol menu if active
//				idolMenuVC.disappear();			
//		
//			if (PaperVC)			// hide paperview controller if active
//				PaperVC.HidePaperVC();				
//			

		
		//if (UIPauseMenu != null) { NGUITools.SetActive(UIPauseMenu, false); }
		


			//inGameVC.statRoot.EnterStatsPage();
			//inGameVC.statRoot.StartAnimSeq();
			//inGameVC.pauseButton.SetActiveRecursively(false);
			//AudioManager.SharedInstance.StopFX();

	
//	public void SetCountDownNumber(int number)
//	{
//		if (inGameVC == null) { return; }
//		inGameVC.SetCountDownNumber(number);
//	}
	



//-- This purpose of this class is to manager the UI, show/hide/animate.  
//-- It should have no knowledge of the Game Sim. Just show, move, animate, and notify anyone about events.


	
//		if (idolMenuVC != null) 
//		{
//			//idolMenuVC.previousViewController = null;
//			idolMenuVC.disappear();
//		}


	//public UIStoreViewControllerOz storeVC;
	//public UIGIftsViewControllerOz GiftsVC = null;
	
	//public GameObject UIObjectivesMenu;	
	//public Transform tiltObject = null;	
	//public List<Transform> CharacterCards = null;	
	


		//if (downloadDialogVC) { downloadDialogVC.disappear(); }
		
	//	notify.Debug ("countAppLaunch "  + countAppLaunch );
		//if ((downloadDialogVC && (countAppLaunch % 3 > 0)) || HaveAllLocationsBeenDownloaded())
		//	downloadDialogVC.disappear();
		//if (gatchVC) { gatchVC.disappear();}
//		if (GiftsVC) { GiftsVC.disappear(); }
		//if (storeVC) { storeVC.disappear(); }		
		


//	public void ShowPaperVC()
//	{
//		NGUITools.SetActive(PaperVC.gameObject, true);
//	}	
	


//	public void ActivePowerIcon() 
//	{
//		if (inGameVC == null) { return; }
//		inGameVC.coinMeter.ActivePowerIcon();	//inGameVC.ActivePowerIcon();
//	}
//	
//	public void FadePowerGlow() 
//	{
//		if (inGameVC == null) { return; }
//		inGameVC.coinMeter.FadePowerGlow();	//inGameVC.FadePowerGlow();
//	}

	
//	public void ShowPowerIconAndGlow(bool show, string iconname = null) 
//	{
//		if (inGameVC == null) { return; }
//		inGameVC.ShowPowerIcon(true); // in oz we always want the power icon
//		inGameVC.ShowGlow(show);
//	}
	
//	public void SetScore(int score) 
//	{
//		if (inGameVC == null) { return; }
//		inGameVC.SetScore(score);
//	}
	
//	public void SetCoinCount(int coins) 
//	{
//		if (inGameVC == null) { return; }
//		inGameVC.SetCoinCount(coins);
//	}
	
//	public void SetGemCount(int gems) 
//	{
//		if (inGameVC == null) { return; }
//		inGameVC.SetGemCount(gems);
//	}
	
	
	//-- The Handlers for the event.
	//public static event voidClickedHandler 	onShowAchievementsClicked = null;

	
//	private System.Collections.Generic.Stack<GameObject> panelStack = new System.Collections.Generic.Stack<GameObject>();
//	
//	public void PushOnStack(GameObject obj) {
//		TR.LOG ("PushOnStack");
//		panelStack.Push(obj);
//		foreach(GameObject o in panelStack) {
//			TR.LOG ("ps: {0}", o);
//		}
//	}
//	
//	public GameObject PopOffStack() {
//		TR.LOG ("PopOffStack");
//		GameObject obj = panelStack.Pop();
//		foreach(GameObject o in panelStack) {
//			TR.LOG ("ps: {0}", o);
//		}
//		return obj;
//	}
//	
//	public GameObject PeekAtStack() {
//		TR.LOG ("PeekAtStack");
//		GameObject obj = panelStack.Peek();
//		foreach(GameObject o in panelStack) {
//			TR.LOG ("ps: {0}", o);
//		}
//		return obj;
//	}
//	
//	public int StackCount() {
//		return panelStack.Count;
//	}
	
	
//	private UIPanel _transitionOut = null;
//	private UIPanel _transitionIn = null;
	
	
//	public void TiltDevice(float tilt) {
//		if(tiltObject == null)
//			return;
//		
//		//-- Tilt is between -1 and 1
//		float alpha = (tilt + 1) / 2.0f;
//		float yRotation = Mathf.Lerp(-40.0f, 40.0f, alpha);
//		tiltObject.rotation = Quaternion.Euler(0, yRotation, 0);
//	}

	//-----------------------------------------------------------------------------------------------
	//-- Specific Transitions. Its Data, Its Code, Its Both.
	//-----------------------------------------------------------------------------------------------
//	private void TranstionMainMenuFromDownToUP() {
//		GameObject scrollRoot = HierarchyUtils.GetChildByName("ScrollRoot", _mainMenuPanel.gameObject);
//		if(scrollRoot == null) {
//			disableMainMenu();
//			return;
//		}
//		
//		TweenPosition tp = TweenPosition.Begin(scrollRoot, 0.5f, new Vector3(0,Screen.currentResolution.height,0));
//		tp.eventReceiver = this.gameObject;
//		tp.callWhenFinished = "disableMainMenu";
//		tp.method = UITweener.Method.EaseInOut;
//		
//		GameObject fadeRoot = HierarchyUtils.GetChildByName("FadeRoot", _mainMenuPanel.gameObject);
//		if(fadeRoot != null) {
//			FadeTreeOUT(fadeRoot, 0.5f, null);
//		}
//	}
//	
//	private void TranstionMainMenuFromUPToDown() {
//		enableMainMenu();
//		GameObject scrollRoot = HierarchyUtils.GetChildByName("ScrollRoot", _mainMenuPanel.gameObject);
//		if(scrollRoot == null)
//			return;
//		
//		TweenPosition tp = TweenPosition.Begin(scrollRoot, 0.75f, Vector3.zero);
//		tp.method = UITweener.Method.EaseInOut;
//		
//		GameObject fadeRoot = HierarchyUtils.GetChildByName("FadeRoot", _mainMenuPanel.gameObject);
//		if(fadeRoot != null) {
//			FadeTreeIN(fadeRoot, 0.5f, null);
//		}
//	}
	
//	private void FadeJournalIN() {
//		GameObject fadeRoot = HierarchyUtils.GetChildByName("FadeRoot", _journalPanel.gameObject);
//		if(fadeRoot != null) {
//			enableJournalMenu();
//			FadeTreeIN(fadeRoot, 0.5f, null);
//		}
//		else {
//			FadeTreeIN(_journalPanel.gameObject, 0.5f, UIJournalMenu);	
//		}
//		
//		
//	}
//	
//	private void FadeJournalOUT() {
//		GameObject fadeRoot = HierarchyUtils.GetChildByName("FadeRoot", _journalPanel.gameObject);
//		if(fadeRoot != null) {
//			FadeTreeOUT(fadeRoot, 0.5f, "disableJournalMenu");
//		}
//		else {
//			FadeTreeOUT(_journalPanel.gameObject, 0.25f, "disableJournalMenu");	
//		}
//	}
	
	
//	private void FadeCharacterMenuIN() {
//		enableCharacterMenu();
//		FadeTreeIN(_characterPanel.gameObject, 0.5f, null);
//	}
//	
//	private void FadeCharacterMenuOUT() {
//		FadeTreeOUT(_characterPanel.gameObject, 0.25f, "disableCharacterMenu");
//	}
	
	
	//-----------------------------------------------------------------------------------------------
	//-- generic transitions
	//-----------------------------------------------------------------------------------------------
	
	
	
//	private void FadeTreeIN(GameObject root, float duration, GameObject enableObject) {
//		ShowObject(enableObject, true, true);
//		
//		UIWidget[] widgets = root.GetComponentsInChildren<UIWidget>();
//		TweenColor tc = null;
//		for (int i = 0, imax = widgets.Length; i < imax; ++i)
//		{
//			UIWidget w = widgets[i];
//			if(w == null)
//				continue;
//			
//			w.color = Color.clear;
//			tc = TweenColor.Begin(w.gameObject, duration, Color.white);
//			tc.method = UITweener.Method.EaseInOut;
//		}
//		
//		Renderer[] models = root.GetComponentsInChildren<Renderer>();
//		for(int i=0, imax = models.Length; i<imax; ++i)
//		{
//			Renderer r = models[i];
//			if(r == null)
//				continue;
//			
//			r.material.color = Color.clear;
//			tc = TweenColor.Begin(r.gameObject, duration, Color.white);
//			tc.method = UITweener.Method.EaseInOut;
//		}
//	}
//	
//	private void FadeTreeOUT(GameObject root, float duration, string callWhenFinished) {
//		UIWidget[] widgets = root.GetComponentsInChildren<UIWidget>();
//		TweenColor tc = null;
//		for (int i = 0, imax = widgets.Length; i < imax; ++i)
//		{
//			UIWidget w = widgets[i];
//			if(w == null)
//				continue;
//			
//			w.color = Color.white;
//			tc = TweenColor.Begin(w.gameObject, duration, Color.clear);
//			tc.method = UITweener.Method.EaseInOut;
//			
//			if(i == 0) {
//				tc.eventReceiver = this.gameObject;
//				tc.callWhenFinished = callWhenFinished;
//			}
//		}
//		
//		Renderer[] models = root.GetComponentsInChildren<Renderer>();
//		for(int i=0, imax = models.Length; i<imax; ++i)
//		{
//			Renderer r = models[i];
//			if(r == null)
//				continue;
//			
//			r.material.color = Color.white;
//			tc = TweenColor.Begin(r.gameObject, duration, Color.clear);
//			tc.method = UITweener.Method.EaseInOut;
//		}
//	}

