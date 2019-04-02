using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Services : MonoBehaviour
{
	private static Dictionary<string, MonoBehaviour> serviceDict = new Dictionary<string, MonoBehaviour>();		// single class-level lookup table for all services
	
	private static Services main;
	
	private static bool _servicesActive = false;
	
	// banderson -- Added a static check to see if the class had been loaded.
	public static bool ServicesActive
	{
		get
		{
			return _servicesActive;
		}
	}
	
	void Awake()
	{
		if (main!=null) 
			Destroy(gameObject);
		else 
			main = this;
		
		_servicesActive = true;

		// Add service MonoBehaviours. Eventually, any script derived from 'Service' should get automatically registered here in a different way using reflection.
		// 							   Might want to also spawn a separate GameObject parented under the 'Services' GO, to attach each service's MonoBehaviour to. 
		gameObject.AddComponent<Rand>();
		gameObject.AddComponent<ObjectivesManager>();
		gameObject.AddComponent<Store>();
		gameObject.AddComponent<NewsManager>();
		gameObject.AddComponent<TuningAnalyticsManager>();
		if ( Settings.GetBool("console-enabled", false))
		{
			gameObject.AddComponent<DebugConsoleOzGui>();
			gameObject.AddComponent<HUDFPSUnityGui>();
		}
		gameObject.AddComponent<ProfileManager>();
//		gameObject.AddComponent<ProfileLoader>();
		gameObject.AddComponent<DownloadManager>();		
		gameObject.AddComponent<DownloadManagerUI>();	
		gameObject.AddComponent<DownloadManagerLocalization>();	
		gameObject.AddComponent<ChallengeDataUpdater>();
		gameObject.AddComponent<LeaderboardManager>();
		gameObject.AddComponent<AppCounters>();
		gameObject.AddComponent<NotificationSystem>();	
		gameObject.AddComponent<MenuTutorials>();	
		gameObject.AddComponent<StorePurchaseHandler>();
		gameObject.AddComponent<ServerSettings>();
		gameObject.AddComponent<AmpBundleManager>();
		
		foreach(MonoBehaviour service in gameObject.transform.GetComponentsInChildren<MonoBehaviour>())
		{
			string name = service.ToString();
			serviceDict.Add(name, service);
		}
	}
	
	void Start() 
	{		
		DontDestroyOnLoad(gameObject);	// keeps services around when changing scenes
	}
	
	public static bool Register(string name, MonoBehaviour service)		// register whatever monobehaviour as a service
	{
		serviceDict["Services(Clone) (" + name + ")"] = service;
		return true;	
	}
	
	public static T Get<T>()			// return service MonoBehaviour script instance (component) based on type
	{
		return (T)(object)serviceDict["Services(Clone) (" + typeof(T).ToString() + ")"];
	}
}



//---------------------------------------------------------------------

	//private void Add<T>()
	//{
		//GameObject newService = new GameObject(typeof(T).Name);
		//newService.transform.parent = gameObject.transform;
		//newService.AddComponent<T>();
		
//		string s = someObject as string;
//		if (s != null)
		
//		if (typeof(T) is MonoBehaviour)
//		{
//			Component newService = gameObject.AddComponent<T>();
//			serviceDict.Add(typeof(T).Name, newService);
//		}
//	}

 //gameObject.transform.Find(typeof(T).Name).GetComponent<T>();

		
		// added by Alex, so Objectives Manager can have an instance (not just class level functionality)
		//GameObject objManGO = new GameObject("ObjectivesManager");	
		//objManGO.AddComponent<ObjectivesManager>();
		

