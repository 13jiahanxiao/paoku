using UnityEngine;
using System.Collections;

public class CoinMeter : MonoBehaviour
{
	//public Transform activePowerIcon;
	public GameObject activePowerButton;
	public UISprite spritePowerIcon;	
	private UILabel labelPowerIcon;
	public UISprite glow;
	public UIFilledSprite progressBar;
	public Color fillColor;
	public Color coolDownColor;
	private ParticleSystem fx;
	
	
	private bool activePower = false;
	//private bool updateMeter = true;
	
	
	void Start(){
		fx = activePowerButton.transform.GetComponentInChildren<ParticleSystem>();
	}
	
	public void appear()
	{
		activePowerButton.GetComponent<ActivePowerButton>().RefreshIcon();	// set powerup icon
		//spritePowerIcon.enabled = false;	// at startup, ensure that the power button glow is hidden
		progressBar.color = fillColor;
	}
	
	public void SetPowerProgress(float progress) 
	{
	//	if(!updateMeter) return;
	
		iTween.Stop(progressBar.gameObject);
		progressBar.fillAmount = progress;
		progressBar.color = fillColor;
		//float p = 0.5f + 0.5f * progress;
		//Color c = new Color(p * fillColor.r, p * fillColor.g, p * fillColor.b, 1f);
		//progressBar.color = c;
		
	}	

	public void FadePowerGlow() 						// remove glow when powerup is used
	{
		activePower = false;
		RemoveBlink(gameObject);
		//spritePowerIcon.enabled = false;
	}
	
	public void ActivePowerIcon()						// make glow when power meter is full
	{	
		activePower = true;
		//spritePowerIcon.enabled = true;
		//Blink(spritePowerIcon.gameObject, 0.25f);	
		Blink(gameObject, 0.25f);
		if(fx){
			fx.Play(true);
		}
		AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_ScoreMultiplier_01);
	}
	
	public void Pause()
	{
		if (activePower) { RemoveBlink(gameObject); }
	}

	public void UnPause()
	{
		if (activePower) { ActivePowerIcon(); }	
	}
	
	
	private void Blink(GameObject root, float duration)
	{
		UIWidget[] widgets = root.GetComponentsInChildren<UIWidget>();
		TweenColor tc = null;
		
		for (int i = 0, imax = widgets.Length; i < imax; ++i)
		{
			UIWidget w = widgets[i];
			if (w == null) { continue; }
			w.color = new Color(w.color.r, w.color.g, w.color.b, 1f);//Color.clear;
			tc = root.GetComponentInChildren<TweenColor>();
			tc.method = UITweener.Method.EaseInOut;
			tc.style = UITweener.Style.PingPong;
			tc.Play (true);
			
		}
		
		TintGeoUI[] uiGeo = root.GetComponentsInChildren<TintGeoUI>();
		foreach(TintGeoUI item in uiGeo)
		{
			item.StartTintAnimation();
		}
	}
	
	private void RemoveBlink(GameObject root) 
	{
		UITweener[] tweens = root.GetComponentsInChildren<UITweener>();
		for (int i=0, imax = tweens.Length; i < imax; ++i)
		{
			UITweener tween = tweens[i];
			if (tween == null) { continue; }
			tween.enabled = false;
		}
		spritePowerIcon.color = new Color(spritePowerIcon.color.r, spritePowerIcon.color.g, spritePowerIcon.color.b, 1f);
		glow.color = new Color(glow.color.r, glow.color.g, glow.color.b, 0f);

		TintGeoUI uiGeo = root.GetComponentInChildren<TintGeoUI>();
		if(uiGeo)
			uiGeo.EndTintAnimation();		
	}	
	
	public void AnimateCoinMeter(float time = 0.5f, float endVal = 0f){
	//	if(!updateMeter) return;
		
		iTween.Stop(progressBar.gameObject);
		
		progressBar.color = coolDownColor;
		//updateMeter = false;
		float startVal = progressBar.fillAmount;
 		iTween.ValueTo(progressBar.gameObject, iTween.Hash(
		"from", startVal,
		"to", endVal,
		"onupdatetarget", gameObject,
		"onupdate", "EmptyCoinMeterUpdate",
		"oncompletetarget", gameObject,
		"oncomplete", "EmptyCoinMeterComplete",
		"time", time,
		"easetype", iTween.EaseType.linear));
	}
	public void EmptyCoinMeterUpdate(float val){
		progressBar.fillAmount = val;
	}
	public void EmptyCoinMeterComplete(){
		//updateMeter = true;
		progressBar.color = fillColor;
	}
	
	
}

/*

	
	//public GameObject coinMeterPulsing;	
	//public Transform coinMeterGlow;
	

		//ShowPowerIcon(true);
		//ShowGlow(false);	


		//ShowGlow(true);
		//if (coinMeterPulsing.gameObject.active == false) { return; }
		//Blink(coinMeterPulsing, 0.25f);


		//ShowGlow(false);
		//ShowPowerIconAndGlow(false);
		//coinMeterGlow.animation.Play();
		//RemoveBlink(coinMeterPulsing);
			//ShowPowerIcon(true);	

	
//	private void ShowPowerIcon(bool show)		// show just the power icon
//	{
//		NGUITools.SetActive(activePowerIcon.gameObject, show);
//	}
	
//	private void ShowGlow(bool show)				// show just the glow
//	{
//		NGUITools.SetActive(coinMeterPulsing, show);	
//	}
	


		//ShowPowerIconAndGlow(false);
		//ShowPowerIcon(true);
		//ShowGlow(false);	
			//RemoveBlink(coinMeterPulsing);	

	/*
	public void ShowPowerIconAndGlow(bool show, string iconname = null) 
	{
		NGUITools.SetActive(activePowerIcon.gameObject, show);
		if (show) 
		{
			spritePowerIcon = activePowerIcon.GetComponentInChildren<UISprite>();
			
			//-- set the correct sprite name.
			//spritePowerIcon.spriteName = iconname;
			CharacterStats activeCharacter = GameProfile.SharedInstance.GetActiveCharacter();
			BasePower activePower = PowerStore.PowerFromID(activeCharacter.powerID);
			labelPowerIcon = activePowerIcon.GetComponentInChildren<UILabel>();
			if (labelPowerIcon)
			{
				labelPowerIcon.text = activePower.Title;
			}
		}
		
		if(coinMeterGlow) 
		{
			NGUITools.SetActive(coinMeterGlow.gameObject, show);
			//coinMeterGlow.renderer.material.color = Color.white;
		}
	}
	*/