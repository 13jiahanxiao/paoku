// Get some settings information, and be able to override it locally in the editor
// and be able to override it from the device
//
// Redmond Urbino
//
// some usage examples
// int startingLife = Settings.GetInt("starting-life", 25)
//   checks Resources/Settings.txt if starting-life is defined, if not set, it defaults to 25
//   if Resources/localSettings.txt exists and has starting-life defined, it would return that value
//   if on the device, deviceSettings.txt exists in the Documents folder, and has starting-life defined, it would return that value
//   if starting-life is defined in multiple files, priority would go in order from
//       1. deviceSettings.txt - this would override any other value defined in #2 or #3
//       2. Resources/localSettings.txt
//       3. Resources/Settings.txt
// note that Resources/localSettings.txt should NOT be committed into git.  it should be included in the .gitignore file
//

// in c# code, it's best that the default values are the same, otherwise if it's not defined
// in any of the settings files, the value that will be picked up will be the first one
// that's evaluated
#define CHECK_FOR_DIFFERENT_DEFAULT_VALUES

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Settings  
{	
	const string appSettingsFilename = "settings.txt";
	const string localSettingsFilename = "local-settings.txt";
	const string deviceSettingsFilename = "device-settings.txt";
	
	/// <summary>
	/// In increasing priority, which source trumps the lower one
	/// </summary>
	public enum SourcePriority
	{
		CodeDefault, // default value from the code
		AppSettingsTextFile, // Resources/settings.txt
		GameServer, // settings coming from the game server
		LocalSettingsTextFile, // Resources/local-settings.txt
		DeviceSettingsTextFile, // Application.dataPath/device-settings.txt
		RuntimeOverride, // changed at runtime, e.g. console
	}
	
	// Parameterized Generic Setting	
	struct BaseSetting<T> 
	{
		public T curValue;
		public T defaultValue;
		public SourcePriority sourcePriority;
	}	
	
	/// <summary>
	/// Keep track of the raw string and where it came from
	/// </summary>
	struct RawValue
	{
		public string rawString;
		public SourcePriority sourcePriority; 
		
		public RawValue( string rawValue, SourcePriority sourcePriority)
		{
			this.rawString = rawValue;
			this.sourcePriority = sourcePriority;
		}
	}
	
	// raw values for our keys
	static Dictionary<string, RawValue> key2RawValues; 
	static Dictionary<string, BaseSetting<string>> key2String;
	static Dictionary<string, BaseSetting<int>> key2Int;
	static Dictionary<string, BaseSetting<float>> key2Float;
	static Dictionary<string, BaseSetting<bool>> key2Bool;

	static StringTryParse stringParser = new StringTryParse();
	static IntTryParse intParser = new IntTryParse();
	static FloatTryParse floatParser = new FloatTryParse();	
	static BoolTryParse boolParser = new BoolTryParse();
	
	
	// the first Settings.Getxxx call will trigger this
	static Settings()
	{
		key2RawValues = new Dictionary<string, RawValue>();
		key2String = new Dictionary<string, BaseSetting<string>>();
		key2Int = new Dictionary<string, BaseSetting<int>>();
		key2Float = new Dictionary<string, BaseSetting<float>>();
		key2Bool = new Dictionary<string, BaseSetting<bool>>();
		parseFiles();
	}
	
	// parse the files in reverse order of priority, so the later files will override the earlier ones
	static void parseFiles()
	{

		TextAsset settingsContents = Resources.Load( Path.GetFileNameWithoutExtension (appSettingsFilename), 
			typeof(TextAsset)) as TextAsset;
		if (settingsContents != null)
		{

			parseOneFile( settingsContents.text, appSettingsFilename, SourcePriority.AppSettingsTextFile );
		}
		else
		{
			Debug.LogWarning("could not load Resources/" + appSettingsFilename);	
		}
		
		TextAsset localSettings = Resources.Load( Path.GetFileNameWithoutExtension(localSettingsFilename)) as TextAsset;
		if (localSettings != null)
		{
			// it is ok  NOT to have localSettings, 	
			parseOneFile(localSettings.text, localSettingsFilename, SourcePriority.LocalSettingsTextFile);
		}
		
		string devicePath = "";
#if UNITY_IPHONE			
		// on iphone it goes to /var/mobile/Applications/aLongStringOfHexDigits/Documents/device-settings.txt
		string fileNameBase = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/'));
		devicePath = fileNameBase.Substring(0, fileNameBase.LastIndexOf('/')) + "/Documents/" + deviceSettingsFilename;
#elif UNITY_ANDROID
		// on the kindle fire, it goes to /mnt/sdcard/Android/data/com.disney.gamename/files/device-settings.txt
		devicePath = Application.persistentDataPath + "/" + deviceSettingsFilename ;
#else
		//on mac it goes to /Users/username/Library/Caches/disney/gamename/device-settings.txt
		devicePath = Application.dataPath + "/" + deviceSettingsFilename;
#endif
		//Debug.Log ("devicePath = " + devicePath);
		try
		{
		    using (StreamReader sr = new StreamReader(devicePath))
		    {
		        string contents = sr.ReadToEnd();
				parseOneFile(contents, deviceSettingsFilename, SourcePriority.DeviceSettingsTextFile); 
		    }
		}
		catch (System.Exception )
		{
			// it is also ok NOT to have device-settings.txt
		    //Debug.Log("The file could not be read:");
		    //Debug.Log(e.Message);
		}
	}
	
	// parse the settings in one file, spitting out appropriate warnings if needed
	static void parseOneFile(string contents, string filename, SourcePriority sourcePriority)
	{
		string [] lines = contents.Split('\n');
		HashSet<string> keysInThisFile = new HashSet<string>();
		foreach (string oneLine in lines)
		{
			string trimmedLine = oneLine.Trim();
			// this is where we can change the format if we need something like json or xml
			
			// lines that begin with # are comments 
			if (trimmedLine.Length > 0 &&  ! trimmedLine.StartsWith("#"))
			{
				string [] splits = trimmedLine.Split('=');
				if (splits.Length == 1)
				{
					Debug.LogWarning(string.Format ("{0}: did not find = in line {1}" , filename , trimmedLine));
					continue;
				}
				
				string rightSide = trimmedLine.Remove(0, splits[0].Length + 1);
				rightSide = rightSide.Trim();
				if (rightSide.Length < 1)
				{
					Debug.LogWarning(string.Format ("{0}: did not find value for {0}, ignoring the line", filename, splits[0]));
					continue;
				}
				
				// its okay to overwrite an existing key across different files, but probably an error if its 
				// done in the same file
				string key = splits[0].Trim();
				if (keysInThisFile.Contains(key))
				{
					Debug.LogWarning(string.Format("{0}: key {1} defined twice, using {2}", filename, key, rightSide));
				}
				keysInThisFile.Add(key);
				
				key2RawValues[key] = new RawValue(rightSide, sourcePriority);
			}
		}
	}
	
	// generic parser from a string to a given type
	public interface ITryParse<T>
	{
		bool TryParse(string rawValue, out T result);	
	}
	

	
	// Generic method for getting a setting
	private static T getSetting<T>(string key, T defaultValue, Dictionary<string, BaseSetting<T>> settings,  ITryParse<T> tryParser)
	{
		T result;
		BaseSetting<T> oneSetting;
		if (settings.TryGetValue(key, out oneSetting))
		{
			result = oneSetting.curValue;
#if CHECK_FOR_DIFFERENT_DEFAULT_VALUES
       		Comparer<T> defComp = Comparer<T>.Default;
			if ( defComp.Compare(defaultValue,oneSetting.defaultValue) != 0)
			{
				Debug.LogWarning(string.Format("For setting {0} we have different default values of {1} and {2}", 
					key, defaultValue, oneSetting.defaultValue));	
			}
#endif
		}		
		else
		{
			RawValue rawValue;
			// not currently parsed as a setting, try raw values
			if (key2RawValues.TryGetValue(key, out rawValue))
			{
				// it was defined in one of the settings files
				oneSetting.defaultValue = defaultValue;
				T valueFromFile;
				if (tryParser.TryParse(rawValue.rawString, out valueFromFile))
				{
					oneSetting.curValue = valueFromFile;
					oneSetting.sourcePriority = rawValue.sourcePriority;
					result = valueFromFile;
				}
				else
				{
					Debug.LogWarning(string.Format("for key {0} couldn't parse {1}, using default of {2}", key, rawValue, defaultValue));
					result = defaultValue;
				}
				settings[key] = oneSetting;	
			}
			else
			{
				// not defined in the files either, save it
				oneSetting.defaultValue = defaultValue;
				oneSetting.curValue = defaultValue;
				oneSetting.sourcePriority = SourcePriority.CodeDefault;
				settings[key] = oneSetting;
				result = defaultValue;
			}			
		}
		return result;
	}
	
	// Generic method for setting a Setting,  typically called in the middle of the run, not on initialization
	// code that uses this method should constantly check the setting, instead of reading it once
	// during Start() or Awake()
	// priority passed in must be greater than or equal to the current priority for the set to take hold
	private static void setSetting<T>(string key, T newValue, Dictionary<string, BaseSetting<T>> settings,  ITryParse<T> tryParser, SourcePriority priority = SourcePriority.RuntimeOverride)
	{
		BaseSetting<T> oneSetting;
		if (settings.TryGetValue(key, out oneSetting))
		{
			if ( priority >= oneSetting.sourcePriority)
			{
				oneSetting.curValue = newValue; 
				settings[key] = oneSetting;
			}
			else
			{
				Debug.LogWarning(string.Format ("NOT changing {0} to {1} from {2} as current priority {3} is greater than {4}",
					key, newValue, oneSetting.curValue, oneSetting.sourcePriority, priority));
			}
		}
		else
		{
			RawValue rawValue;
			// not currently parsed as a setting, try raw values
			if (key2RawValues.TryGetValue(key, out rawValue))
			{
				// it was defined in one of the settings files
				T valueFromFile;
				if (tryParser.TryParse(rawValue.rawString, out valueFromFile))
				{
					// we would get this case if a GetSetting wasn't previously called, but now what do we do with defaultValue?
					oneSetting.defaultValue = valueFromFile;
				}
				else
				{
					Debug.LogWarning(string.Format("for key {0} couldn't parse {1}, using   {2}", key, rawValue.rawString, newValue));
					oneSetting.defaultValue = newValue;
				}
				if ( priority >= rawValue.sourcePriority)
				{
					oneSetting.curValue = newValue;
				}
				else
				{
					oneSetting.curValue = oneSetting.defaultValue;
					Debug.LogWarning(string.Format ("NOT changing {0} to {1} from {2} as current priority {3} is greater than {4}",
						key, newValue, oneSetting.defaultValue, rawValue.sourcePriority, priority));
				}

				settings[key] = oneSetting;		
			}
			else
			{
				// not defined in the files either, save it
				Debug.LogWarning("Settings.SetString key not found, possible misspelling for " + key);
				oneSetting.defaultValue = newValue;
				oneSetting.curValue = newValue;
				oneSetting.sourcePriority = priority;
				settings[key] = oneSetting;
			}			
		}
	}
	
	// trivial string parser
	public class StringTryParse: ITryParse<string>
	{
		public bool TryParse(string rawValue, out string result)
		{
			result = rawValue;
			return true;
		}
	}
	
	// trivial int parser
	public class IntTryParse: ITryParse<int>
	{
		public bool TryParse(string rawValue, out int result)
		{
			return int.TryParse(rawValue, out result);
		}
	}
	
	// trivial float parser
	public class FloatTryParse: ITryParse<float>
	{
		public bool TryParse(string rawValue, out float result)
		{
			return float.TryParse(rawValue, out result);
		}
	}
	
	// trivial bool parser
	public class BoolTryParse: ITryParse<bool>
	{
		public bool TryParse(string rawValue, out bool result)
		{
			bool parsed = true;
			string lowered = rawValue.ToLower();
			if ( lowered == "true")
			{
				result = true;
			}
			else if (lowered == "false")
			{
				result = false;
			}
			else
			{
				result = false;
				parsed = false;
			}
			return parsed;
		}
	}

	// get a string setting
	public static string GetString(string key, string defaultValue)
	{
		string result = getSetting<string>(key, defaultValue, key2String,  stringParser );
		return result;	
	}
	
	// set a string setting
	public static void SetString(string key, string newValue, SourcePriority priority = SourcePriority.RuntimeOverride)
	{
		setSetting<string>(key, newValue, key2String, stringParser, priority);
	}

	// get a int setting
	public static int GetInt(string key, int defaultValue)
	{
		int result = getSetting<int>(key, defaultValue, key2Int, intParser );
		return result;	
	}
	
	// set a int setting
	public static void SetInt(string key, int newValue, SourcePriority priority = SourcePriority.RuntimeOverride)
	{
		setSetting<int>(key, newValue, key2Int, intParser, priority);
	}
	
	// get a float setting
	public static float GetFloat(string key, float defaultValue)
	{
		float result = getSetting<float>(key, defaultValue, key2Float, floatParser );
		return result;	
	}
	
	// set a float setting
	public static void SetFloat(string key, float newValue, SourcePriority priority = SourcePriority.RuntimeOverride)
	{
		setSetting<float>(key, newValue, key2Float, floatParser, priority);
	}
	
	
	// get a bool setting
	public static bool GetBool(string key, bool defaultValue)
	{
		bool result = getSetting<bool>(key, defaultValue, key2Bool, boolParser );
		return result;	
	}
	
	// set a bool setting
	public static void SetBool(string key, bool newValue, SourcePriority priority = SourcePriority.RuntimeOverride)
	{
		setSetting<bool>(key, newValue, key2Bool, boolParser, priority);
	}
}
