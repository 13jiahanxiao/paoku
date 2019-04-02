using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System;


public class CustomBuilder : ScriptableWizard {
	protected static Notify notify = new Notify("CustomBuilder");
	public bool TurnOffDevFlags = true;
	public bool AutoRunBuild = false;
	public bool AllowDebugging = false;
	public string buildName;

	public string promotionURL;
	
	public string twitterMessage;
	public string twitterURL;
	
	public bool reviewBegEnable;

	public string version;


	const string DefaultBuildName = "TempleRun2";
	protected BuildOptions buildOptions = BuildOptions.None;


	public enum BuildTypes
	{
		iOS,
		Android,
		AndroidFire,
		AndroidToysRUS,
		Mac
	}

	static string[] BuildTypeName = new string[] {
		"iOS",
		"Android",
		"Android (Fire)",
		"Android (Toys R Us Device)",
		"Mac App"
	};


	static string[] BuildSettingsPrefix = new string[] {
		"IOS",
		"ANDROID",
		"FIRE",
		"TRU",
		"MACAS"
	};
	
	static string[] BuildSufix = new string[] {
	"",
	".apk",
	".apk",
	".apk",
	".app"
	};


	static char 		DirectorySeparatorChar = System.IO.Path.DirectorySeparatorChar;
	static BuildTypes 	CustomBuildType = BuildTypes.iOS;

	protected BuildTarget buildTarget = BuildTarget.WebPlayer; 
	
	[MenuItem("Build/iOS", false, 1)]
	public static void CustomBuildiOS()
	{
		CustomBuildType = BuildTypes.iOS;
		ShowWizard();
//		CustomBuilder_iOS cb = ScriptableWizard.DisplayWizard<CustomBuilder_iOS>("Custom Build", "Build");
//		cb.Setup();
	}
	
	[MenuItem("Build/Android", false, 1)]
	public static void CustomBuildAndroid()
	{
		CustomBuildType = BuildTypes.Android;
		CustomBuilder_Android cb = ScriptableWizard.DisplayWizard<CustomBuilder_Android>("Custom Build", "Build");
		cb.Setup();
	}

	[MenuItem("Build/Android (Kindle Fire)", false, 1)]
	public static void CustomBuildAndroidFire()
	{
		CustomBuildType = BuildTypes.AndroidFire;
		CustomBuilder_Fire cb = ScriptableWizard.DisplayWizard<CustomBuilder_Fire>("Custom Build", "Build");
		cb.Setup();
	}
	
	[MenuItem("Build/Android (Toy's R Us Device)", false, 1)]
	public static void CustomToysRUS()
	{
		CustomBuildType = BuildTypes.AndroidToysRUS;
		CustomBuilder_TRU cb = ScriptableWizard.DisplayWizard<CustomBuilder_TRU>("Custom Build", "Build");
		cb.Setup();
	}

	[MenuItem("Build/Mac App Store", false, 1)]
	public static void CustomBuildMac()
	{
		CustomBuildType = BuildTypes.Mac;
		ShowWizard();
	}

	static void ShowWizard()
	{
		CustomBuilder cb = ScriptableWizard.DisplayWizard<CustomBuilder>("Custom Build", "Build");
		cb.Setup();
	}

	protected virtual void Setup()
	{
		version = PlayerSettings.bundleVersion;
		buildName = BuildName;
		promotionURL = PromotionURL;
		twitterURL = TwitterURL;
		twitterMessage = TwitterMessage;
		reviewBegEnable = ReviewBegEnable;
	}


	protected void OnWizardUpdate()
	{
		helpString = "Build for " + BuildTypeName[(int)CustomBuildType];
	}


	protected void OnWizardCreate()
	{
		StoreDefaults();
		if (PreBuild() == true) {
			EditorApplication.SaveScene(EditorApplication.currentScene);
			Build();
		}
	}


	protected virtual bool PreBuild()
	{
		PlayerSettings.bundleVersion = version;
		if (TurnOffDevFlags)
			ConfigureForeRelease();

		//----------
		
//		GameObject vo = GetGameObject("Version Label");
//		if (vo == null) return false;
//		UILabel vl = vo.GetComponent<UILabel>();
//		if (vl == null) {
//			IndicateProblem("Version Label object didn't have UILabel component??");
//			return false;
//		}
//
//		if (vl.text != version) { vl.text = version; Note("Setting Version Label in Options to [" + vl.text + "]"); }

		//----------

//		GameObject pgo = GetGameObject("Promotion Manager");
//		Promotion p = pgo.GetComponent<Promotion>();
//		if (p == null) {
//			IndicateProblem("Promotion Manager object didn't have Promotion component??");
//			return false;
//		}
//
//		if(p.url != promotionURL) { p.url = promotionURL; Note("Setting Pomrotion URL to [" + promotionURL + "]"); }
				
				
		//----------
				
//		GameObject eggo = GetGameObject("End Game GUI");
//		EndGameUI e = eggo.GetComponent<EndGameUI>();
//		if (e == null) {
//			IndicateProblem("End Game GUI object didn't have EndGameGUI component??");
//			return false;
//		}
//
//		e.TwitterLink = twitterURL;
//		if(twitterURL != e.TwitterLink) { e.TwitterLink = twitterURL; Note("Setting Twitter URL to [" + e.TwitterLink + "]"); }
//		
//		e.TwitterMessage = twitterMessage;
//		if(twitterMessage != e.TwitterMessage) { e.TwitterMessage = twitterMessage; Note("Setting Twitter Message to [" + e.TwitterMessage + "]"); }
		
		//----------


//		GameObject go = GetGameObject("End Game GUI");
//		EndGameUI egg = go.GetComponent<EndGameUI>();
//		if (egg == null) {
//			IndicateProblem("End Game GUI object didn't have EndGameGUI component??");
//			return false;
//		}
//		string sss = GetDefaultString("ShowStatusButton");
//		bool should = false;
//		if (sss != "")
//			should = Boolean.Parse(sss);
//
//		if (egg.ShowStatsButton != should) { egg.ShowStatsButton = should; Note("Changing Show Stats Button to " + should); }



		//----------


		GameObject go = GetGameObject("GameCircleManager");
		AmazonGameServiceCustomManager mgr = go.GetComponent<AmazonGameServiceCustomManager>();
		if (mgr == null) {
			IndicateProblem("AmazonGameServicesManager object didn't have AmazonGameServiceCustomManager component??");
			return false;
		}
		string sss = GetDefaultString("DoAmazonGameServices");
		bool should = false;
		if (sss != "")
			should = Boolean.Parse(sss);

		if (mgr.DoAmazonGamerServices != should) { 
			mgr.DoAmazonGamerServices = should; 
			Note("Changing Do Amazon Game Services to " + should); 
		}

		return true;
	}



	protected virtual bool ConfigureForeRelease()
	{
		GameObject gco = GetGameObject("GameController");
		if (gco == null) return false;

		GameController gc = gco.GetComponent<GameController>();
		if (gc == null) {
			IndicateProblem("Game Controller object didn't have GameController component??");
			return false;
		}

//		if (gc.DebugHyperCoins) { gc.DebugHyperCoins = false; Note("Turning off Debug Hyper Coins"); }
//
//
//		GameObject pws = GetGameObject("Playback Window Sizer",false);
//		if (pws != null) { pws.SetActiveRecursively(false); Note("Turning off playback window sizer"); }
//
//		GameObject fps = GetGameObject("FPS Panel", false);
//		if (fps != null) { NGUITools.SetActive(fps, false); Note("Turning off FPS Panel"); }
//
//		GameObject pgo = GetGameObject("Promotion Manager");
//		Promotion p = pgo.GetComponent<Promotion>();
//		if (p == null) {
//			IndicateProblem("Promotion Manager object didn't have Promotion component??");
//			return false;
//		}
//		if (p.TestMode) { p.TestMode = false; Note("Turning off Promotion Test Mode"); }


		return true;
	}


	protected GameObject GetGameObject(string name,bool errorIfNotFound=true)
	{
		GameObject go = GameObject.Find(name);
		if (go == null && errorIfNotFound) {
			IndicateProblem("Could not find [" + name + "]");
			return null;
		}
		return go;
	}


	protected virtual void Build()
	{
		//Get the build directory - by default it's the project's root directory
		string buildPath = GetBuildDirectory().FullName + DirectorySeparatorChar + buildName + BuildSufix[(int)CustomBuildType];

		string[] levels = new string[] {
			@"Assets/Scenes/TheGame.unity"  // A mere one scene for this game
		};

		string err = "";
		BuildOptions options = BuildOptions.None;
		
		if(AutoRunBuild)
			options |= BuildOptions.AutoRunPlayer;
		if(AllowDebugging == true)
			options |= BuildOptions.AllowDebugging;
		
		err = BuildPipeline.BuildPlayer(levels, buildPath, buildTarget, options);
		
		if (err != string.Empty) {
			Note(err);
			EditorUtility.DisplayDialog("Custom Build","Build Error:\n" + err, "Dismiss");
		}
		else {
			PostProcessBuild(buildPath);
		}
	}


	protected virtual void PostProcessBuild(string buildPath)
	{
		// Do Nothing
	}


	protected void IndicateProblem(string prob)
	{
		EditorUtility.DisplayDialog("Custon Build Problem", prob + "  Aborting!", "Oh Noz!");
	}

	protected void Note(string note)
	{
		notify.Debug("CUSTOM BUILD: " + note);
	}


	void StoreDefaults()
	{
		BuildName = buildName;
		PromotionURL = promotionURL;
		TwitterURL = twitterURL;
		TwitterMessage = twitterMessage;
		ReviewBegEnable = reviewBegEnable;
	}

	protected string BuildName
	{
		get
		{
			string bn = GetSavedString("BuildName",GetDefaultString("BuildName"));
			return bn;
		}
		set {
			SetSavedString("BuildName", value);
		}
	}

	protected string PromotionURL
	{
		get
		{
			string bn = GetSavedString("PromotionURL", GetDefaultString("PromotionURL"));
			if (bn == string.Empty)
				bn = GetDefaultString("PromotionURL");
			return bn;
		}
		set
		{
			SetSavedString("PromotionURL", value);
		}
	}
	
	
	protected string TwitterURL
	{
		get
		{
			string bn = GetSavedString("TwitterURL", GetDefaultString("TwitterURL"));
			if (bn == string.Empty)
				bn = GetDefaultString("TwitterURL");
			return bn;
		}
		set
		{
			SetSavedString("TwitterURL", value);
		}
	}
	
	protected string TwitterMessage
	{
		get
		{
			string bn = GetSavedString("TwitterMessage", GetDefaultString("TwitterMessage"));
			if (bn == string.Empty)
				bn = GetDefaultString("TwitterMessage");
			return bn;
		}
		set
		{
			SetSavedString("TwitterMessage", value);
		}
	}
	
	protected bool ReviewBegEnable {
		get {
			string rbe = GetSavedString ("ReviewBegEnabled",GetDefaultString ("ReviewBegEnabled"));
			if(rbe == string.Empty)
				rbe = GetDefaultString("ReviewBegEnabled");
			return Boolean.Parse(rbe);
		}
		set
		{
			SetSavedString ("ReviewBegEnabled",value.ToString());
		}
	}


	protected virtual string GetDefaultString(string key)
	{
		if (key.Equals("BuildName"))
			return DefaultBuildName;
		if(key.Equals("ReviewBegEnabled"))
			return true.ToString();
		return "";
	}
	
	
	private string KeyName(string key)
	{
		return BuildSettingsPrefix[(int)CustomBuildType] + " " + key;
	}

	protected string GetSavedString(string key, string defaultValue)
	{
		return EditorPrefs.GetString(KeyName (key), defaultValue);
	}

	protected void SetSavedString(string key, string value)
	{
		EditorPrefs.SetString(KeyName(key), value);
	}

	
	protected bool GetSavedBool(string key, bool defaultValue)
	{
		return EditorPrefs.GetBool(KeyName (key), defaultValue);
	}

	protected void SetSavedBool(string key, bool value)
	{
		EditorPrefs.SetBool(KeyName(key), value);
	}


	static DirectoryInfo GetBuildDirectory()
	{
		DirectoryInfo assetsDirectory = new DirectoryInfo(Application.dataPath);
		DirectoryInfo projectDirectory = assetsDirectory.Parent;
		DirectoryInfo[] buildDirectories = projectDirectory.GetDirectories("Builds");
		return (buildDirectories == null || buildDirectories.Length == 0) ? projectDirectory : buildDirectories[0];
	}

	
}
