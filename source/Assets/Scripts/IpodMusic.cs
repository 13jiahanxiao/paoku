using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class IpodMusic
{
	
	[DllImport("__Internal")]
	public static extern bool _getIsIpodMusicPlaying();
	
	public static bool IsIpodMusicPlaying()
	{
#if UNITY_IPHONE && !UNITY_EDITOR
		return _getIsIpodMusicPlaying();
#else
		return false;
#endif
	}
}
