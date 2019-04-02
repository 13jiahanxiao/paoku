using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildVersion : MonoBehaviour 
{
	void Start() 
	{
		string buildVersion = "Build Label";
		TextAsset buildVersionData = Resources.Load("build_info") as TextAsset;
		
		if (buildVersionData != null)
			buildVersion = buildVersionData.text;
		
		if (Settings.GetBool("show-build-label", false))
		{
			gameObject.GetComponent<UILabel>().text = buildVersion;
		}
		else
		{
			gameObject.GetComponent<UILabel>().text = "";
		}
	}
	
	public static string GetBuildVersion()
	{
		string buildVersion = "Unity Editor Version";
		TextAsset buildVersionData = Resources.Load("build_info") as TextAsset;
		
		if (buildVersionData != null)
			buildVersion = buildVersionData.text;
		
		return buildVersion;
	}
}
