//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;


/// <summary>
/// Localization manager is able to parse localization information from text assets.
/// Although a singleton, you will generally not access this class as such. Instead
/// you should implement "void Localize (Localization loc)" functions in your classes.
/// Take a look at UILocalize to see how it's used.
/// </summary>
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Internal/Localization")]
public class Localization : MonoBehaviour
{
	
	public delegate void OnLanguageChangedHandler(string language);
	private static event OnLanguageChangedHandler onLanguageChangedHandler = null;
	public static void RegisterForOnLanguageChanged(OnLanguageChangedHandler delg) { 
		onLanguageChangedHandler += delg; }
	public static void UnRegisterForOnLanguageChanged(OnLanguageChangedHandler delg) { 
		onLanguageChangedHandler -= delg; }		
	
	private bool mainFontUpdated = false;
	protected static Notify notify;
	private static Localization sharedInstance = null;
	public static Localization SharedInstance
	{
		get
		{
			if(sharedInstance==null)
			{
				sharedInstance = (Localization)GameObject.FindObjectOfType(typeof(Localization));
				
				if(sharedInstance==null)
				{
					GameObject go = new GameObject();
					sharedInstance = go.AddComponent<Localization>();
					notify.Warning("No Localization found in the scene! Add one or enable it.");
				}
			}
			return sharedInstance;
		}
	}
	static Localization mInst;
	
	/// <summary>
	/// The instance of the localization class. Will create it if one isn't already around.
	/// </summary>

	static public Localization instance
	{
		get
		{
			if (mInst == null)
			{
				mInst = Object.FindObjectOfType(typeof(Localization)) as Localization;

				if (mInst == null)
				{
					GameObject go = new GameObject("_Localization");
					DontDestroyOnLoad(go);
					mInst = go.AddComponent<Localization>();
				}
			}
			return mInst;
		}
	}

	/// <summary>
	/// Language the localization manager will start with.
	/// </summary>

	public string startingLanguage;
	
	public bool ForceLanguage = false;
	
	/// <summary>
	/// Available list of languages.
	/// </summary>

	public TextAsset[] languages;

	Dictionary<string, string> mDictionary = new Dictionary<string, string>();
	TextAsset loadedDictionaryAsset = null;
	string mLanguage = "";
	string loadedLanguage = "";

	public UIFont fontMainRefPrefab;
	public UIFont fontMainRefPrefab2;
	
	public string standardFontPath;
	public string chineseSimpleFontPath;
	public string chineseTradFontPath;
	public string japaneseFontPath;
	public string koreanFontPath;
	
	public string standardFontPathHD;
	public string chineseSimpleFontPathHD;
	public string chineseTradFontPathHD;
	public string japaneseFontPathHD;
	public string koreanFontPathHD;
	
	
	public string standardFontPath2;
	public string chineseSimpleFontPath2;
	public string chineseTradFontPath2;
	public string japaneseFontPath2;
	public string koreanFontPath2;
	
	public string standardFontPathHD2;
	public string chineseSimpleFontPathHD2;
	public string chineseTradFontPathHD2;
	public string japaneseFontPathHD2;
	public string koreanFontPathHD2;
	
	
	public string standardFontIos = null;
	public string chineseSimpleFontIos = null;
	public string chineseTradFontIos = null;
	public string japaneseFontIos = null;
	public string koreanFontIos = null;
	
	
	public string standardFontAndroid = null;
	public string chineseSimpleFontAndroid = null;
	public string chineseTradFontAndroid = null;
	public string japaneseFontAndroid = null;
	public string koreanFontAndroid = null;
	

	[HideInInspector]
	public string currentIosFont;
	[HideInInspector]
	public string currentAndroidFont;
	
	private UIFont standardFont = null;
	private UIFont chineseSimpleFont = null;
	private UIFont chineseTradFont = null;
	private UIFont japaneseFont = null;
	private	UIFont koreanFont = null;
	
	
	private UIFont standardFont2 = null;
	private UIFont chineseSimpleFont2 = null;
	private UIFont chineseTradFont2 = null;
	private UIFont japaneseFont2 = null;
	private	UIFont koreanFont2 = null;
	
	private int currentLanguageId = 0;
	
	
	// eyal comment out
	//[SerializeField]
	//protected AmpsManager AmpsManager = null;
	
	public readonly string MainFontAssetName = "MainFont";
	
	public void SetLanguage()
	{// refresh lanuage font
		SetLanguage(currentLanguage);
	}
	
	public string GetLoadedLanguage()
	{
		return loadedLanguage;
	}
	
	/// <summary>
	/// Returns true if we have loaded the main font prefab
	/// </summary>
	/// <returns>
	/// <c>true</c> if we have loaded the main font prefab; otherwise, <c>false</c>.
	/// </returns>
	public static bool HasMainFontBeenUpdated()
	{
		bool result = false;
		if (sharedInstance != null && sharedInstance.mainFontUpdated == true)
		{
			
			result = true;
		}
		return result;
	}
	
	public void SetLanguage(string lang)
	{
		if( lang == loadedLanguage)
		{
			return ;
		}
		
		if(notify!=null)
		{	
			notify.Debug ("SetLanguage " + lang);
		}
		
		currentLanguage = lang; // this will update font map and dictionary
		
//		UpdateMainFont();  is called already by  currentLanguage set
		
		GameObject[] uis = GameObject.FindGameObjectsWithTag("UILocalize");
		foreach(GameObject go in uis){
			notify.Debug ("uis " + go.name);
			UILocalize ui = go.GetComponent<UILocalize>();
			ui.Localize();
		}

		if(onLanguageChangedHandler!=null)
		{
			onLanguageChangedHandler(lang);
		}		
	}
	
	public bool IsValueExistInList(object[] objectList, object objectToFound)
	{
		foreach(object obj in objectList)
		{
			if (objectToFound.Equals(obj))
			{
				return true;
			}
		}
		return false;
	}
	
	public string GetISO1LanguageCode()
	{
		return "zh_CN";
	}

	public string GetCurrentLanguageCode()
	{
		return "zh";
	}
	
	public void CycleLanguages(){
		currentLanguageId++;
		if(currentLanguageId > 10) currentLanguageId = 0;
		switch (currentLanguageId){
			case 0: SetLanguage("English");
				break;
			case 1: SetLanguage("French");
				break;
			case 2: SetLanguage("Italian");
				break;
			case 3: SetLanguage("German");
				break;
			case 4: SetLanguage("Spanish");
				break;
			case 5: SetLanguage("Dutch");
				break;
			case 6: SetLanguage("Russian");
				break;
			case 7: SetLanguage("Chinese_Traditional");
				break;
			case 8: SetLanguage("Chinese_Simplified");
				break;
			case 9: SetLanguage("Korean");
				break;
			case 10: SetLanguage("Portuguese");
				break;
		}
	}
	
	public string GetUnityLanguage()
	{
		string lang = Application.systemLanguage.ToString();
		switch(lang)
		{
			case "Chinese":
				return "Chinese_Traditional";
			case "English":
			case "French":
			case "Dutch":
			case "German":
			case "Italian":
			case "Spanish":
			case "Russian":
			case "Japanese":
			case "Korean":
				if (Debug.isDebugBuild)
					notify.Debug("Lang detection finished :" + lang);
				return lang;

			case "Portuguese":
				return "Portuguese";
			
			default:
				if (Debug.isDebugBuild) 
					notify.Debug("Lang detection not found : English"); 
				return "English";
		}	
	}
	
	#if UNITY_ANDROID
	
	//[System.NonSerialized]
	public static AndroidJavaClass LocaleClass = null;	
	//[System.NonSerialized]
	public static AndroidJavaObject DefaultLocaleObject = null;

	public bool InitializeJavaObject()
	{
		if (LocaleClass == null)
		{
			LocaleClass = new AndroidJavaClass("java.util.Locale");

			if (LocaleClass == null)
			{
				notify.Error("Error LocaleClass is null - Force English");
				return false;
			}
		}

		if (DefaultLocaleObject == null)
		{
			DefaultLocaleObject = LocaleClass.CallStatic<AndroidJavaObject>("getDefault");

			if (DefaultLocaleObject == null)
			{
				notify.Error("Error DefaultLocaleObject is null - Force English");
				return false;
			}
		}

		return true;
	}

	public string GetAndroidLanguage()
	{	
		string lang = Application.systemLanguage.ToString();
		
		switch(lang)
		{
			case "Chinese":
			{
				if (Debug.isDebugBuild) 
					notify.Debug("Chinese Language Detection"); 
				// Best Way to get Language in unity plugin documentation
	    		// Traditional Chinese in Taiwan
				// Simplified Chinese in other chinese country
				
				/*
				if (DefaultLocaleObject == null)
				{

					//return "Chinese_Simplified";
					
					// if (!InitializeJavaObject())
					{
 
					}
				}
			
				 isoCountry = DefaultLocaleObject.Call<string>("getCountry");


				if (Debug.isDebugBuild) 
					notify.Debug("getCountry in unity = " + isoCountry); 
				*/

				string isoCountry="CH";
				switch (isoCountry)
				{
				case "TW":
					return "Chinese_Traditional";
				case "CH":
				default:
					return "Chinese_Simplified";
				}
				
				/*
				// if failed use the iso3 country and specify what to do
				string iso3Country = DefaultLocaleObject.Call<string>("getISO3Country");
				
				if (Debug.isDebugBuild) 
					notify.Debug("getISO3Language in unity = " + iso3Country); 
				switch (iso3Country)
				{
				case "HKG": // Hong-Kong
				case "TWN":	// Taiwan
					// Change for traditional chinese
					return "Chinese_Traditional";
					break;
					
				case "MAC": // Macao (= Macau ?)
				case "SGP": // Singapore
				case "CHN": // Chinese
				default:
					// nothing to do
					return "Chinese_Simplified";
				}*/
			}
#if CHINESE_BUILD
			default:
				if (Debug.isDebugBuild) 
					notify.Debug("Lang detection finished :" + lang + " But chinese build : Chinese forced");
				return "Chinese_Simplified";
#else
			case "English":
			case "French":
			case "Dutch":
			case "German":
			case "Italian":
			case "Spanish":
			case "Russian":
//			case "Japanese":
			case "Korean":
				if (Debug.isDebugBuild)
					notify.Debug("Lang detection finished :" + lang);
				return lang;

			case "Portuguese":
				return "Portuguese";
			
			default:
				if (Debug.isDebugBuild) 
					notify.Debug("Lang detection not found : English"); 
				return "English";
#endif

		}
	}
				
#elif UNITY_IPHONE
	public string GetIOSLanguage()
	{
		// eyal edit
		string lang = EtceteraBinding.getCurrentLanguage();
		//string lang = "en";
//		Debug.LogError ("LANGUAGE ===== " + lang);
		notify.Debug("Language code :" + lang);
		switch (lang)
        {
			case "zh-Hant":
				lang = "Chinese_Traditional";
				break;
			case "zh-Hans":
			case "zh":
				lang = "Chinese_Simplified";
                break;
#if CHINESE_BUILD
			default:
				if (Debug.isDebugBuild) 
					notify.Debug("Lang detection finished :" + lang + " But chinese build : Chinese forced");
				lang = "Chinese_Simplified";
				break;
#else
            case "en":
                lang = "English";
                break;
            case "fr":
                lang = "French";
                break;
            case "it":
                lang = "Italian";
                break;
            case "de":
                lang = "German";
                break;
            case "es":
                lang = "Spanish";
                break;
            case "nl":
                lang = "Dutch";
                break;
            case "ru":
                lang = "Russian";
                break;

//			case "ja":
 //               lang = "Japanese";
 //               break;
			
			case "kr":
			case "ko":
				lang = "Korean";
                break;
			
			case "pt-PT":
			case "pt":
				lang = "Portuguese";
                break;
			
			default:
				lang = "English";
				break;
#endif
        }
		
		return lang;
	}
#endif
	
	public string GetLangBySystem()
	{		
		string lang;
#if CHINESE_BUILD
		if (Debug.isDebugBuild) 
			notify.Debug("Localization - CHINESE BUILD DETECTED");
#endif
#if UNITY_IPHONE
		if (Debug.isDebugBuild) 
			notify.Debug("Localization - Getting Language from IOS");
		if( Application.isEditor )
			lang = GetUnityLanguage();
		else
	        lang = GetIOSLanguage();
		if (Debug.isDebugBuild) 
			notify.Debug("Language choosed by IOS system : " + lang);
//		Debug.LogError ("IOS LANGUAGE1 ========= " + lang);
#elif UNITY_ANDROID
		if (Debug.isDebugBuild) 
			notify.Debug("Localization - Getting Language Java Object");

		lang = GetAndroidLanguage();
//		Debug.LogError ("ANDROID LANGUAGE1 ========= " + lang);
		if (Debug.isDebugBuild) 
			notify.Debug("Language choosed by Android system : " + lang);
#elif CHINESE_BUILD
		lang = "Chinese_Simplified";
#else
		lang = Application.systemLanguage.ToString();
#endif
//		Debug.LogError ("LANGUAGE1 ========= " + lang);
//		lang = "Chinese_Traditional";
//		lang = "Chinese_Simplified";
//		lang = "Korean";
//		lang = "Russian";
//		lang = "French";
//		lang = "Portuguese";
//		lang = "Italian";
//		lang = "Dutch";
//		lang = "Spanish";
		return lang;
	}
	
	public Dictionary<string, string> GetDictionnary()
	{
		return mDictionary;
	}
	
	/// <summary>
	/// Name of the currently active language.
	/// </summary>
	
	public string currentLanguage
	{
		get
		{
			if (string.IsNullOrEmpty(mLanguage))
			{
				if( string.IsNullOrEmpty(startingLanguage))
				{
					startingLanguage = languages[0].name;
				}
				
				if (ForceLanguage == true)
				{
					mLanguage = startingLanguage;
				}
				else
				{
					mLanguage = GetLangBySystem();
					if (string.IsNullOrEmpty(mLanguage))
					{
						mLanguage = startingLanguage;
					}
				}
//				SetLanguage(language); // postponded
			}
			return mLanguage;
		}
		private set
		{
			if (languages != null && loadedLanguage != value)
			{
				mLanguage = value;
//			Debug.LogError("SetLanguage " + value);
				if (ForceDictionary(value) == false)
				{
//					Debug.LogError("Failed!  Try english");
					mLanguage = "English";
					ForceDictionary(mLanguage);
					return;
				}
				/*startingLanguage = value;

				if (!string.IsNullOrEmpty(value))
				{
					// Check the referenced assets first
					if (languages != null)
					{
						for (int i = 0, imax = languages.Length; i < imax; ++i)
						{
							TextAsset asset = languages[i];

							if (asset != null && asset.name == value)
							{
								Load(asset); // if load failed, this will load english as default
								return;
							}
						}
					}

					// Not a referenced asset -- try to load it dynamically
					TextAsset txt = Resources.Load(value, typeof(TextAsset)) as TextAsset;

					if (txt != null)
					{
						Load(txt);
						return;
					}
				}

				// Either the language is null, or it wasn't found
				mDictionary.Clear();
				loadedDictionaryAsset = null;
				PlayerPrefs.DeleteKey("Language");*/
			}
		}
	}
	
	
	bool ForceDictionary(string lang, bool editorMode = false)
	{
		if (string.IsNullOrEmpty(lang))
		{
			mDictionary.Clear();
			loadedDictionaryAsset = null;
		}
		
		string name = lang.ToLower();
		
		foreach (TextAsset asset in languages)
		{
			//if (Debug.isDebugBuild) 
			//	notify.Debug(string.Format("compare languages {0}, {1}, {2}, {3}", asset ? asset.name : "null", name, val, asset == null));
			if (asset != null && asset.name.ToLower() == name)
			{
				return Load(asset, editorMode);
			}
		}
		return false;
	}
	
	/// <summary>
	/// Determine the starting language.
	/// </summary>

	void Awake () 
	{ 
		Localization.sharedInstance = this;	
		SetupNotify();
		if (mInst == null) 
		{ 
			mInst = this; 
			DontDestroyOnLoad(gameObject); 
		} 
		else 
		{
			Destroy(gameObject); 
		}
	}

	/// <summary>
	/// Oddly enough... sometimes if there is no OnEnable function in Localization, it can get the Awake call after UILocalize's OnEnable.
	/// </summary>

	void OnEnable () { if (mInst == null) mInst = this; }

	/// <summary>
	/// Remove the instance reference.
	/// </summary>

	void OnDestroy () { 
		if (mInst && mInst == this) mInst = null;
		if(fontMainRefPrefab) fontMainRefPrefab.replacement = null; 
		if(fontMainRefPrefab2) fontMainRefPrefab2.replacement = null; 
	}

	/// <summary>
	/// Load the specified asset and activate the localization.
	/// </summary>
	
	public void LoadDictionarOnly()
	{
		LoadDictionarOnly(GetLangBySystem());
	}
	
	public void LoadDictionarOnly(string language)
	{
		string name = language.ToLower();
		
		foreach (TextAsset asset in languages)
		{
			//if (Debug.isDebugBuild) 
			//	notify.Debug(string.Format("compare languages {0}, {1}, {2}, {3}", asset ? asset.name : "null", name, val, asset == null));
			if (asset != null && asset.name.ToLower() == name)
			{
				if( loadedDictionaryAsset != asset)
				{
					ByteReader reader = new ByteReader(asset);
					mDictionary = reader.ReadDictionary();
					loadedDictionaryAsset = asset;
				}
			}
		}
		return ;
	}

	/// <summary>
	/// Setups the notify instance.  When running in editor sometimes Awake() doesn't get called ahead of time
	/// </summary>
	protected void SetupNotify()
	{
		if(notify==null)
		{
			notify = new Notify(this.GetType().Name);
		}
	}
	
	
/// <summary>
	/// Load the specified asset and activate the localization.
	/// </summary>

	bool Load(TextAsset asset, bool bEditMode = false)
	{
		SetupNotify();
		notify.Debug("Localization.Load " + asset.ToString() + " " + bEditMode);
		InitializeCultureInfo();
		// eyal comment
		//InitLocale();
			
		if( loadedDictionaryAsset != asset)
		{
			//PlayerPrefs.SetString("Language", mLanguage);
			ByteReader reader = new ByteReader(asset);
			mDictionary = reader.ReadDictionary();
			loadedDictionaryAsset = asset;
		}
		
		if (!bEditMode)
		{
			NGUITools.Broadcast("OnLocalize", this);
		}

		// Update the main font reference
		if( UpdateMainFont() == false )// load failed, need to switch to default font
		{
			return false;
//			currentLanguage = "English";
		}
		
		return true;
		
		/* eyal comment out
		if (AmpsManager)
		{
			// Use downloaded font texture asset
			Texture2D fontTexture = AmpsManager.LoadAssetTexture(MainFontAssetName);
			if (fontTexture != null)
			{
			    // Replace the font image with the downloaded one
			    fontMainRefPrefab.replacement.material.mainTexture = fontTexture;
			}
			else
			{
				currentLanguage = "English";
			}
		}
		*/
	}
	
	public string GetAssetBundleName(string language)
	{
		string languagenamelower = language.ToLower();
		switch (languagenamelower)
		{
		case "chinese_traditional":
			return "localchinesetraditional";
			
		case "chinese_simplified":
			return "localchinesesimplified" ;
			
		case "japanese":
			return "";
			
		case "korean":
			return "localkorean";

		case "russian":
			return "localrussian";

		}
		
		return "";
	}

	public string GetLogoName(string language)
	{
		string languagenamelower = language.ToLower();
		switch (languagenamelower)
		{
		case "chinese_traditional":
			return "logoOzChineseTraditional_HIGH";
			
		case "chinese_simplified":
			return "logoOzChineseSimplified_HIGH" ;
			
		case "japanese":
			return "";
			
		case "korean":
			return "logoOzKorean_HIGH";
			
		case "russian":
			return "logoOzRussian_HIGH";
		}
		
		return "";
	}
	
	void LoadLogo()
	{
		string lname = GetLogoName(mLanguage);
		if( lname != "" )
		{
//			Debug.Log("Logo name = " + lname);
			Texture logo = ResourceManager.Load(lname, typeof(Texture)) as Texture;
			if( logo == null )
			{
				logo = Resources.Load(lname, typeof(Texture)) as Texture;
			}
			
			if( logo)
			{
				UIManagerOz.SharedInstance.idolMenuVC.ChangeLogo(logo);
			}
			else
			{
//				Debug.Log ("logo load failed");
			}
		}
	}
	
	private bool UpdateMainFont()
	{
		if (Debug.isDebugBuild && notify!=null) 
			notify.Debug(string.Format("Localization - Load (lang): {0}, {1}", mLanguage, mLanguage.ToLower()));
//		Debug.LogError ("Language is " + mLanguage);

		bool useLowResFont = UIManagerOz.UseLowResUI();
		string bundlename = GetAssetBundleName(mLanguage);
		string languagenamelower = mLanguage.ToLower();
		notify.Debug(string.Format("Localization.UpdateMainFont bundlename={0} mLanguage={1}", bundlename, mLanguage));
		switch (languagenamelower)
		{
		case "chinese_traditional":
			if (chineseTradFont == null)
			{
				if( bundlename != "" && ResourceManager.SharedInstance.IsAssetBundleAvailable(bundlename) )
				{
//					Debug.LogError ("Geeting font from asset bundle");
					if(useLowResFont)
					{
						chineseTradFont = ResourceManager.Load(chineseTradFontPath, typeof(UIFont)) as UIFont;
						chineseTradFont2 = ResourceManager.Load(chineseTradFontPath2, typeof(UIFont)) as UIFont;
					}
					else
					{
						chineseTradFont = ResourceManager.Load(chineseTradFontPathHD, typeof(UIFont)) as UIFont;
						chineseTradFont2 = ResourceManager.Load(chineseTradFontPathHD2, typeof(UIFont)) as UIFont;
					}
				}
				else
				{
//					Debug.LogError ("No Chinese asset bundle");
					if(useLowResFont)
					{
						chineseTradFont = Resources.Load(chineseTradFontPath, typeof(UIFont)) as UIFont;
						chineseTradFont2 = Resources.Load(chineseTradFontPath2, typeof(UIFont)) as UIFont;
					}
					else{
						chineseTradFont = Resources.Load(chineseTradFontPathHD, typeof(UIFont)) as UIFont;
						chineseTradFont2 = Resources.Load(chineseTradFontPathHD2, typeof(UIFont)) as UIFont;
					}
				}
				
				if( chineseTradFont == null || chineseTradFont2 == null )
				{
					return false;
				}
				
				currentIosFont = chineseTradFontIos;
				currentAndroidFont = chineseTradFontAndroid;
			}
			fontMainRefPrefab.replacement = chineseTradFont;
			fontMainRefPrefab2.replacement = chineseTradFont2;
			break;

		case "chinese_simplified":
			if (chineseSimpleFont == null)
			{
				if( bundlename != "" && ResourceManager.SharedInstance.IsAssetBundleAvailable(bundlename) )
				{
//					Debug.LogError ("Geeting font from asset bundle");
					if(useLowResFont)
					{
						chineseSimpleFont = ResourceManager.Load(chineseSimpleFontPath, typeof(UIFont)) as UIFont;
						chineseSimpleFont2 = ResourceManager.Load(chineseSimpleFontPath2, typeof(UIFont)) as UIFont;
					}
					else
					{
						chineseSimpleFont = ResourceManager.Load(chineseSimpleFontPathHD, typeof(UIFont)) as UIFont;
						chineseSimpleFont2 = ResourceManager.Load(chineseSimpleFontPathHD2, typeof(UIFont)) as UIFont;
					}
				}
				else
				{
//					Debug.LogError ("No Chinese asset bundle");
					if(useLowResFont)
					{
						chineseSimpleFont = Resources.Load(chineseSimpleFontPath, typeof(UIFont)) as UIFont;
						chineseSimpleFont2 = Resources.Load(chineseSimpleFontPath2, typeof(UIFont)) as UIFont;
					}
					else{
						chineseSimpleFont = Resources.Load(chineseSimpleFontPathHD, typeof(UIFont)) as UIFont;
						chineseSimpleFont2 = Resources.Load(chineseSimpleFontPathHD2, typeof(UIFont)) as UIFont;
					}
				}
				
				if( chineseSimpleFont == null || chineseSimpleFont2 == null )
				{
					return false;
				}
				
				currentIosFont = chineseSimpleFontIos;
				currentAndroidFont = chineseSimpleFontAndroid;
			}
			fontMainRefPrefab.replacement = chineseSimpleFont;
			fontMainRefPrefab2.replacement = chineseSimpleFont2;
			break;
			
		case "japanese":
			if (japaneseFont == null)
			{
				if(useLowResFont) {
					japaneseFont = Resources.Load(japaneseFontPath, typeof(UIFont)) as UIFont;
					japaneseFont2 = Resources.Load(japaneseFontPath2, typeof(UIFont)) as UIFont;
				}
				else {
					japaneseFont = Resources.Load(japaneseFontPathHD, typeof(UIFont)) as UIFont;
					japaneseFont2 = Resources.Load(japaneseFontPathHD2, typeof(UIFont)) as UIFont;
				}
				currentIosFont = japaneseFontIos;
				currentAndroidFont = japaneseFontAndroid;
			}
			
			fontMainRefPrefab.replacement = japaneseFont;
			fontMainRefPrefab2.replacement = japaneseFont2;
			break;
			
		case "korean":
			if (koreanFont == null)
			{
				if( bundlename != "" && ResourceManager.SharedInstance.IsAssetBundleAvailable(bundlename) )
				{
//					Debug.LogError ("Geeting font from asset bundle");
					if(useLowResFont)
					{
						koreanFont = ResourceManager.Load(koreanFontPath, typeof(UIFont)) as UIFont;
						koreanFont2 = ResourceManager.Load(koreanFontPath2, typeof(UIFont)) as UIFont;
					}
					else
					{
						koreanFont = ResourceManager.Load(koreanFontPathHD, typeof(UIFont)) as UIFont;
						koreanFont2 = ResourceManager.Load(koreanFontPathHD2, typeof(UIFont)) as UIFont;
					}
				}
				else
				{
//					Debug.LogError ("No Chinese asset bundle");
					if(useLowResFont)
					{
						koreanFont = Resources.Load(koreanFontPath, typeof(UIFont)) as UIFont;
						koreanFont2 = Resources.Load(koreanFontPath2, typeof(UIFont)) as UIFont;
					}
					else{
						koreanFont = Resources.Load(koreanFontPathHD, typeof(UIFont)) as UIFont;
						koreanFont2 = Resources.Load(koreanFontPathHD2, typeof(UIFont)) as UIFont;
					}
				}
				currentIosFont = koreanFontIos;
				currentAndroidFont = koreanFontAndroid;
			}

			if( koreanFont == null || koreanFont == null )
			{
				return false;
			}
			
			fontMainRefPrefab.replacement = koreanFont;
			fontMainRefPrefab2.replacement = koreanFont2;
			break;
		// others one...
		default:
			if (standardFont == null)
			{
				if(useLowResFont){
					standardFont = Resources.Load(standardFontPath, typeof(UIFont)) as UIFont;
					standardFont2 = Resources.Load(standardFontPath2, typeof(UIFont)) as UIFont;
				}
				else{
					standardFont = Resources.Load(standardFontPathHD, typeof(UIFont)) as UIFont;
					standardFont2 = Resources.Load(standardFontPathHD2, typeof(UIFont)) as UIFont;
				}
				currentIosFont = standardFontIos;
				currentAndroidFont = standardFontAndroid;
			}
			fontMainRefPrefab.replacement = standardFont;
			fontMainRefPrefab2.replacement = standardFont2;
			break;
		}
		UnloadUnusedFont();
		
		loadedLanguage = mLanguage; // load successfully
		notify.Debug("loadedLanguage is now" + loadedLanguage.ToString());
		LoadLogo();
		mainFontUpdated = true;
		return true;
	}

	void UnloadUnusedFont()
	{
		if (standardFont && fontMainRefPrefab.replacement != standardFont)
		{
			standardFont = null;
			standardFont2 = null;
		}
		
		if (japaneseFont && fontMainRefPrefab.replacement != japaneseFont)
		{
			japaneseFont = null;
			japaneseFont2 = null;
		}
		
		if (chineseSimpleFont && fontMainRefPrefab.replacement != chineseSimpleFont)
		{
			chineseSimpleFont = null;
			chineseSimpleFont2 = null;
		}
		
		if (chineseTradFont && fontMainRefPrefab.replacement != chineseTradFont)
		{
			chineseTradFont = null;
			chineseTradFont2 = null;
		}
		
		if (koreanFont && fontMainRefPrefab.replacement != koreanFont)
		{
			koreanFont = null;
			koreanFont2 = null;
		}
		//TODO: we probably should not do this...at least not here
		Resources.UnloadUnusedAssets();
	}

	/// <summary>
	/// Localize the specified value.
	/// </summary>

	public string Get (string key)
	{
		string val;
		if (mDictionary.TryGetValue(key, out val))
		{
			//notify.Debug("Localization.Get key={0} returning {1}", key, val);
			return val;
		}
		else
		{
			//notify.Debug("Localization.Get key={0} NOT found, returning {1}", key, val);
			return key;
		}
	}
	
	private NumberFormatInfo mNumberFormatInfo;
	
	public NumberFormatInfo GetIntLabelFormat()
	{
		return mNumberFormatInfo;
	}
	
	private string mLocaleCountry = "";
	public void InitLocale()
	{
#if UNITY_IPHONE
		mLocaleCountry = A2MGetCountry();
#elif UNITY_ANDROID	
	/*
		using (AndroidJavaClass cls = new AndroidJavaClass("java.util.Locale")) 
		{
			using(AndroidJavaObject locale = cls.CallStatic<AndroidJavaObject>("getDefault")) 
			{
				mLocaleCountry =  locale.Call<string>("getCountry");
			}
		}
	*/
#endif
	}
	
	[DllImport("__Internal")]
    static extern string A2MGetCountry();
	
	public string GetLocaleCountry()
	{
		return mLocaleCountry;
	}
	
	private CultureInfo mCultureInfo;
	public CultureInfo GetCultureInfo()
	{
		return mCultureInfo;
	}
	
	private void InitializeCultureInfo()
	{
#if CHINESE_BUILD
		mNumberFormatInfo = CultureInfo.GetCultureInfo("zh-CN").NumberFormat;
		return;
#else
		switch(mLanguage)
		{
		case "English":
			mCultureInfo = CultureInfo.GetCultureInfo("en-US");
			return;
		
		case "French":
			mCultureInfo = CultureInfo.GetCultureInfo("fr-FR");
			return;
		
		case "Italian":
			mCultureInfo = CultureInfo.GetCultureInfo("it-IT");
			return;
		
		case "German":
			mCultureInfo = CultureInfo.GetCultureInfo("de-DE");
			return;
			
		case "Spanish":
			mCultureInfo = CultureInfo.GetCultureInfo("es-ES");
			return;
			
		case "Dutch":
			mCultureInfo = CultureInfo.GetCultureInfo("nl-NL");
			return;
			
		case "Russian":
			mCultureInfo = CultureInfo.GetCultureInfo("ru-RU");
			return;
			
		case "Chinese_Traditional":
		case "Chinese_Simplified":
			mCultureInfo = CultureInfo.GetCultureInfo("zh-CN");
			return;
		
		case "Japanese":
			mCultureInfo = CultureInfo.GetCultureInfo("ja-JP");
			return;
			
		case "Portuguese":
			mCultureInfo = CultureInfo.GetCultureInfo("pt-PT");
			return;
			
		case "Korean":
			mCultureInfo = CultureInfo.GetCultureInfo("ko-KR");
			return;
		default:
			break;
		}
		//mCultureInfo = CultureInfo.GetCultureInfo("en-US");		// bypass the above for now
		
		mNumberFormatInfo = mCultureInfo.NumberFormat;
#endif
	}

#if UNITY_EDITOR

	public bool InitInEditor()
	{
		if (languages.Length > 0 && languages[0] != null)
		{
			ForceDictionary(languages[0].name, true);
			return true;
		}
		
		return false;
	}

	// We can't to remove file permission.... :(
	public void WriteLocalisationFile(TextAsset asset, Dictionary<string, string> dic)
	{
		string filepath = Application.dataPath + "/Text/" + asset.name + ".txt";
		if (!File.Exists(filepath))
    	{
			notify.Error(string.Format("Localisation file {0} is not in Asset/Text/Folder ({1})...", asset.name, filepath));
			return;
		}
		
		// create new file
		string filecontent = "";
		
		foreach (string cKey in dic.Keys)
		{
			string s = string.Format ("{0} = {1}\n", cKey, dic[cKey]);
			filecontent += s;
		}
		
		File.WriteAllText(filepath, filecontent);
	}
	
	public TextAsset[] EditorAddNewField(string newField, string desc)
	{
		Dictionary<string, string> tmpDic = new Dictionary<string, string> ();
		foreach (TextAsset asset in languages)
		{
			if (asset == null)
			{
				continue;
			}
			
			// read file
			ByteReader reader = new ByteReader(asset);
			
			// erase dictionary
			tmpDic.Clear();
			
			// get dictionary
			tmpDic = reader.ReadDictionary();
			
			// add field if not exist
			if (!tmpDic.ContainsKey(newField))
			{
				tmpDic.Add(newField, desc == null ? "ToDo" : desc);
				WriteLocalisationFile(asset, tmpDic);
			}
		}
		
		return languages;
	}
#endif
}