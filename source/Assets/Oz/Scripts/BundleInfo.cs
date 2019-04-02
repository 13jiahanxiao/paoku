using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

/// <summary>
/// Retrieves information specific to the bundle.
/// </summary>
public static class BundleInfo 
{
#if !UNITY_EDITOR && UNITY_IPHONE
	[DllImport("__Internal")]
	public static extern string GetBundleId();
#else
	public static string GetBundleId()
	{
		string bundleId = "com.disney.TempleRunOz";
 
		return bundleId;
	}
#endif

	public static string GetBundleVersion()
	{
		string bundleVersion = "";
 
		bundleVersion = ResourceManager.GetVersionString() + ".0";
		return bundleVersion;
	}
}
