using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System;

public class CustomBuilder_Fire : CustomBuilder
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

		GameObject go = GetGameObject("GameController");
		GameController vc = go.GetComponent<GameController>();
		if (vc == null) {
			IndicateProblem("UIIAPStore object didn't have UIIAPViewController component??");
			return false;
		}
		if (vc.Mechanism != UIIAPViewControllerOz.AndroidIAPMechanism.Amazon) { 
			vc.Mechanism = UIIAPViewControllerOz.AndroidIAPMechanism.Amazon; 
			Note("Changing IAP Mechanism to Amazon"); 
		}
//		GameObject go = GetGameObject("IAPShim");
//		IAPShim s = go.GetComponent<IAPShim>();
//		if (s == null) {
//			IndicateProblem("IAPShim object didn't have IAPShim component??");
//			return false;
//		}
//		if (s.Mechanism != IAPShim.IAPMechanism.Amazon) { s.Mechanism = IAPShim.IAPMechanism.Amazon; Note("Changing IAP Mechanism to Amazon"); }

		return true;

	}

	protected override string GetDefaultString(string key)
	{
		if(key.Equals("PromotionURL"))
			return "https://s3.amazonaws.com/com.imangi/android/promo/TRAndroidFire_1.txt";
		if(key.Equals("TwitterURL"))
			return "http://amzn.to/TempleRunAmazon";	
		if(key.Equals("TwitterMessage"))
			return @"I got {0} points while escaping from demon monkeys in @TempleRun. Beat that! {1}";
		if(key.Equals("ReviewBegEnabled"))
			return false.ToString();	
		if(key.Equals("ShowStatusButton"))
			return true.ToString();
		if (key.Equals("DoAmazonGameServices"))
			return true.ToString();

		return base.GetDefaultString(key);
	}

}
