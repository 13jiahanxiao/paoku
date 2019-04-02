#if UNITY_EDITOR

#define DEBUG_LEVEL_LOG
#define DEBUG_LEVEL_WARN
#define DEBUG_LEVEL_ERROR

#else

//#define DEBUG_LEVEL_LOG
//#define DEBUG_LEVEL_WARN
//#define DEBUG_LEVEL_ERROR

#endif

using UnityEngine;
using System.Collections;

public class TR
{
	[System.Diagnostics.Conditional( "DEBUG_LEVEL_LOG" )]
	[System.Obsolete("TR.LOG is deprecated, please use notify.Debug instead.", true)]
	public static void LOG(string format, params object[] paramList)
	{
		string newFormat = string.Format ("[{0}] {1}", Time.time, string.Format(format, paramList));
		Debug.Log ( newFormat );
	}
	
	[System.Diagnostics.Conditional( "DEBUG_LEVEL_LOG" )]
	[System.Diagnostics.Conditional( "DEBUG_LEVEL_WARN" )]
	[System.Obsolete("TR.WARN is deprecated, please use notify.Warning instead.", true)]
	public static void WARN(string format, params object[] paramList)
	{
		string newFormat = string.Format ("[{0}] {1}", Time.time, string.Format(format, paramList));
		Debug.LogWarning ( newFormat );

	}
	
	[System.Diagnostics.Conditional( "DEBUG_LEVEL_LOG" )]
	[System.Diagnostics.Conditional( "DEBUG_LEVEL_WARN" )]
	[System.Diagnostics.Conditional( "DEBUG_LEVEL_ERROR" )]
	[System.Obsolete("TR.ERROR is deprecated, please use notify.Error instead.", true)]
	public static void ERROR(string format, params object[] paramList)
	{
		string newFormat = string.Format ("[{0}] {1}", Time.time, string.Format(format, paramList));
		Debug.LogError ( newFormat );
	}
	
	[System.Diagnostics.Conditional( "UNITY_EDITOR" )]
	[System.Diagnostics.Conditional( "DEBUG_LEVEL_LOG" )]
	public static void ASSERT(bool condition)
	{
		ASSERT (condition, string.Empty, true);
	}
	
	[System.Diagnostics.Conditional( "UNITY_EDITOR" )]
	[System.Diagnostics.Conditional( "DEBUG_LEVEL_LOG" )]
	public static void ASSERT(bool condition, string assertString)
	{
		ASSERT (condition, assertString, true);
	}
	
	[System.Diagnostics.Conditional( "UNITY_EDITOR" )]
	[System.Diagnostics.Conditional( "DEBUG_LEVEL_LOG" )]
	public static void ASSERT(bool condition, string assertString, bool pauseOnFail)
	{
		if( condition == false )
		{
			Debug.Log("["+Time.time+"] assert failed: " + assertString);
			if( pauseOnFail == true )
			{
				Debug.Break();
			}
		}
	}
};
