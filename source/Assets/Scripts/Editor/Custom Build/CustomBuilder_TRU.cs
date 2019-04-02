using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System;

public class CustomBuilder_TRU : CustomBuilder
{

	public int revCode;


	protected override void Setup()
	{
		base.Setup();
		revCode = PlayerSettings.Android.bundleVersionCode;
	}

	protected override bool PreBuild()
	{
		if (base.PreBuild() == false)
			return false;

		buildTarget = BuildTarget.Android;

		PlayerSettings.Android.bundleVersionCode = revCode;

		PlayerSettings.Android.keyaliasName = "";


//		GameObject go = GetGameObject("IAPShim");
//		IAPShim s = go.GetComponent<IAPShim>();
//		if (s == null) {
//			IndicateProblem("IAPShim object didn't have IAPShim component??");
//			return false;
//		}
//		if (s.Mechanism != IAPShim.IAPMechanism.None) { s.Mechanism = IAPShim.IAPMechanism.None; Note("Changing IAP Mechanism to None"); }

		return true;

	}

	protected override string GetDefaultString(string key)
	{
		if(key.Equals("PromotionURL"))
			return "https://s3.amazonaws.com/com.imangi/android/promo/TRAndroidTRU_1.txt";
		if(key.Equals("TwitterURL"))
			return "http://bit.ly/TempleRunOnAndroid";
		if(key.Equals("TwitterMessage"))
			return @"I got {0} points while escaping from demon monkeys in @TempleRun for Android. Beat that! {1}";
		if(key.Equals("ReviewBegEnabled"))
			return false.ToString();			
		return base.GetDefaultString(key);
	}

}
