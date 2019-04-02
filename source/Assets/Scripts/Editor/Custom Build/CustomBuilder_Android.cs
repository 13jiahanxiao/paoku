using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System;

public class CustomBuilder_Android : CustomBuilder {

	public int revCode;
	public bool w3iEnable;
	public string keyStore;
	public string keyStorePass;
	public string keyStoreKeyAlias;
	public string keyStoreKeyPassword;



	protected override void Setup()
	{
		base.Setup();
		revCode = PlayerSettings.Android.bundleVersionCode;
	
		w3iEnable = GetSavedBool("w3iEnable",false);
		
		keyStore = GetSavedString("KeyStore",PlayerSettings.Android.keystoreName);
		keyStorePass = GetSavedString("KeyStorePassword",PlayerSettings.Android.keystorePass);
		keyStoreKeyAlias = GetSavedString("KeyStoreKeyAlias",PlayerSettings.Android.keyaliasName);
		keyStoreKeyPassword = GetSavedString("KeyStoreKeyPassword",PlayerSettings.Android.keyaliasPass);

	}


	protected override bool PreBuild()
	{

		if (base.PreBuild() == false)
			return false;

		buildTarget = BuildTarget.Android;

		PlayerSettings.Android.bundleVersionCode = revCode;

		PlayerSettings.Android.keyaliasName = keyStore;
		PlayerSettings.Android.keystorePass = keyStorePass;
		PlayerSettings.Android.keyaliasName = keyStoreKeyAlias;
		PlayerSettings.Android.keyaliasPass = keyStoreKeyPassword;
		
		SetSavedBool("w3iEnable",w3iEnable);
		SetSavedString("KeyStore", keyStore);
		SetSavedString("KeyStorePassword", keyStorePass);
		SetSavedString("KeyStoreKeyAlias", keyStoreKeyAlias);
		SetSavedString("KeyStoreKeyPassword", keyStoreKeyPassword);
		
		
		if (GameController.SharedInstance.Mechanism != UIIAPViewControllerOz.AndroidIAPMechanism.GooglePlay) { 
			GameController.SharedInstance.Mechanism = UIIAPViewControllerOz.AndroidIAPMechanism.GooglePlay; 
			Note("Changing IAP Mechanism to Google Play "+GameController.SharedInstance.Mechanism); 
		}
//
//		GameObject gow = GetGameObject("External Promotion Manager");
//		PromotionManager pm = gow.GetComponent<PromotionManager>();
//		if (pm == null) {
//			IndicateProblem("External Promotion Manager object didn't have PromotionManager component??");
//			return false;
//		}
//		if (pm.W3IEnable != w3iEnable) { pm.W3IEnable = w3iEnable; Note("Changing External Promotion to w3i: " + ((w3iEnable == true) ? "Enabled" : "Disabled") ); }
		return true;
		
	}

	protected override string GetDefaultString(string key)
	{
		if (key.Equals("PromotionURL"))
			return "https://s3.amazonaws.com/com.imangi/android/promo/TRAndroid_1.txt";
		if(key.Equals("TwitterURL"))
			return "http://bit.ly/TempleRunOnAndroid";
		if(key.Equals("TwitterMessage"))
			return @"I got {0} points while escaping from demon monkeys in @TempleRun for Android. Beat that! {1}";
		if(key.Equals("ReviewBegEnabled"))
			return true.ToString();
		return base.GetDefaultString(key);
	}


}
