//
//
// replaces Debug.Log, Debug.LogWarning, and Debug.LogError
// meant so we can have more than 3 levels of messages
// and we can control which debug output we want per class
// and lastly do a compile time stripping of notify.Debug on relase builds
// 
// @author Redmond Urbino
//

//#define SHOW_TRACE
//#define SHOW_DEBUG
#define SHOW_INFO
#define SHOW_WARNING
#define SHOW_ERROR
//#define USE_NSLOG

using System.Runtime.InteropServices;
using System;
using UnityEngine;
using System.Diagnostics;

public class Notify
{
	// our notifies in order of increasing severity, default level is usually Info
	public enum NotifyLevel
	{
		Trace,
		Debug,
		Info,
		Warning,
		Error,
	}
	
	// we really want debug messages to be turned off by default
	private const NotifyLevel defaultDefaultLevel = NotifyLevel.Info;
	private static NotifyLevel curDefaultLevel = defaultDefaultLevel; 
	private const string defaultKey = "default-notify-level";
	
	private string identifier;
	NotifyLevel curLevel;
	public NotifyLevel CurLevel 
	{ 
		get 
		{
			return curLevel;
		}
	}
	
	// before anything else, lets figure out the current default
	static Notify()
	{
		curDefaultLevel = GetDefaultNotifyLevel();	
	}
	
	// the identifier is typically just the class name, 
	// but you could make a notify object that is used across multiple classes, if you really wanted to
	public Notify(string identifier)
	{
		this.identifier = identifier;
		string key = "notify-level-"+identifier;
		string value = Settings.GetString(key, curDefaultLevel.ToString());
		curLevel = parseNotifyLevel(value);
	}
	
	/// will return defaultLevel if not parsed properly,
	public static NotifyLevel parseNotifyLevel(string level)
	{
		NotifyLevel result = curDefaultLevel;
		try
		{
			result = (NotifyLevel) Enum.Parse(typeof(NotifyLevel), level, true);
		}
		catch (Exception e)
		{
			UnityEngine.Debug.Log("parseNotifyLevel, error parsing " + level + " " + e.Message );
		}
		
		return result;		
	}
	
	// if not specifically set, what level are we defaulting to
	public static NotifyLevel GetDefaultNotifyLevel()
	{
		string overrideDefaultLevel = Settings.GetString(defaultKey, defaultDefaultLevel.ToString());	
		NotifyLevel result = parseNotifyLevel( overrideDefaultLevel);
		return result;
	}
	
	// typically, which class are we associated with
	public string Identifier 
	{
		get
		{
			return identifier;	
		}
	}
	
	// entry point, to where we could also spit out the identifer, the stacktrace, the time, etc
	string formatMsg(string msg)
	{
		// as needs arise, add options to show identifier, stacktrice, time, etc.
		return msg;	
	}
	
	[Conditional("SHOW_TRACE")]
	public void Trace(string msg)
	{
		if (curLevel <= NotifyLevel.Trace)
		{
			UnityEngine.Debug.Log( formatMsg(msg));
			consoleLog(formatMsg(msg));
		}
	}
	
	[Conditional("SHOW_TRACE")]
	/// <summary>
	/// Print a trace statement with multiple parameters
	/// </summary>
	/// <param name='format'>
	/// the format string see string.Format
	/// </param>
	/// <param name='paramList'>
	/// the variable length parameter list.
	/// </param>
	public void Trace(string format, params object[] paramList)
	{
		this.Trace( string.Format(format, paramList)) ;
	}
	
	[Conditional("SHOW_DEBUG")]
	public void Debug(object obj)
	{
		if (curLevel <= NotifyLevel.Debug)
		{
			UnityEngine.Debug.Log( formatMsg(obj.ToString()));
			consoleLog(formatMsg(obj.ToString()));
		}		
	}
	
#if UNITY_EDITOR
	private static  void consoleLog(string text) {}
#elif UNITY_IPHONE
	#if USE_NSLOG
		[DllImport("__Internal")]
		private static extern void consoleLog(string text);
	#else
		private static  void consoleLog(string text) {}
	#endif
#elif UNITY_ANDROID
	private static  void consoleLog(string text) {}
#else
	private static  void consoleLog(string text) {}
#endif	
	
	[Conditional("SHOW_DEBUG")]
	public void Debug( string msg)
	{
		if (curLevel <= NotifyLevel.Debug)
		{
			UnityEngine.Debug.Log( formatMsg(msg));	
			consoleLog(formatMsg(msg));		
		}		
	}

	[Conditional("SHOW_DEBUG")]
	public void Debug(string format, params object[] paramList)
	{
		this.Debug( string.Format(format, paramList)) ;
	}

	
	[Conditional("SHOW_INFO")]
	public void Info( string msg)
	{
		if (curLevel <= NotifyLevel.Info)
		{
			UnityEngine.Debug.Log( formatMsg(msg));	
			consoleLog(formatMsg(msg));
		}		
	}
	
	[Conditional("SHOW_INFO")]
	public void Info(string format, params object[] paramList)
	{
		this.Info( string.Format(format, paramList));
	}	
	
	[Conditional("SHOW_WARNING")]
	public void Warning( string msg)
	{
		if (curLevel <= NotifyLevel.Warning)
		{
			UnityEngine.Debug.LogWarning( formatMsg(msg));	
			consoleLog(formatMsg(msg));
		}		
	}
	[Conditional("SHOW_WARNING")]
	public void Warning(string format, params object[] paramList)
	{
		this.Warning( string.Format(format, paramList));
	}
	
	[Conditional("SHOW_ERROR")]
	public void Error( string msg)
	{
		if (curLevel <= NotifyLevel.Error)
		{
			UnityEngine.Debug.LogError( formatMsg(msg));	
			consoleLog(formatMsg(msg));
		}		
	}
	
	[Conditional("SHOW_ERROR")]
	public void Error(string format, params object[] paramList)
	{
		this.Error( string.Format(format, paramList)); 
	}
	
	[Conditional("SHOW_DEBUG")]
	public void Assert(bool condition, string assertString, bool pauseOnFail=true)
	{
		if( condition == false )
		{
			this.Debug("assert failed: " + assertString);
			if( pauseOnFail == true )
			{
				UnityEngine.Debug.Break();
			}
		}
	}
}