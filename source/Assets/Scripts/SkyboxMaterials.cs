using UnityEngine;
using System.Collections;

public class SkyboxMaterials : MonoBehaviour {
	
	//public Material tintMaterial;
	public Material origMaterial;
	
	private float tintFadeInTime = 0.5f;
	private float tintFadeOutTime = 0.5f;
	
	private float fadeInMaxValue = 0.0f;
	private bool fadingIn = false;
	private bool fadingOut = false;
	
	private Color originalTintColor;
	protected static Notify notify;
	
	void Awake()
	{
		notify = new Notify(this.GetType().Name);
	}
	// Use this for initialization
	void Start () 
	{
		if(renderer.material.HasProperty("_Color"))
			originalTintColor = renderer.material.GetColor ("_Color");
	}
	
	// Update is called once per frame
	void Update () {
		if(fadingIn && !fadingOut)
		{
			SetTintValue(GetTintValue () + Time.deltaTime/tintFadeInTime);
			if(GetTintValue() > fadeInMaxValue)
			{
				SetTintValue (fadeInMaxValue);				
				fadingIn = false;
			}
		}
		
		if(fadingOut && !fadingIn)
		{
			SetTintValue(GetTintValue () - Time.deltaTime/tintFadeOutTime);
			if(GetTintValue() <= 0.0f)
			{
				SetTintValue (0.0f);
				fadingOut = false;
			}
		}		
	}
	
	public void ResetToOriginalTintColor()
	{
		if(renderer.material.HasProperty("_Color"))
			renderer.material.SetColor("_Color", originalTintColor);
	}
	
	public void SetTintColorToWhite()
	{
		if(renderer.material.HasProperty("_Color"))
			renderer.material.SetColor("_Color", new Color (1,1,1,1));
	}

	public void SetTintColorToBlack()
	{
		if(renderer.material.HasProperty("_Color"))
			renderer.material.SetColor("_Color", new Color (0,0,0,1));
	}	
	
	public void SetMaterial()
	{
		notify.Debug("renderer = " + renderer);
		if (renderer != null)
		{
			notify.Debug("renderer.material = " + renderer.material);
		}
		else
		{
			notify.Error("renderer should not be null");	
		}
		notify.Debug("origMaterial = " + origMaterial);
		renderer.material = origMaterial;
	}

	public float GetTintValue()
	{
		float outValue = 0.0f;
		if(renderer.material.HasProperty("_TintValue"))
			outValue = renderer.material.GetFloat("_TintValue");
				
		return outValue;
	}	
	
	public void SetTintValue(float tintValue)
	{
		if(renderer.material.HasProperty("_TintValue"))
			renderer.material.SetFloat("_TintValue", tintValue);
	}

	public void FadeInTint(float time, float maxValue)
	{
		tintFadeInTime = time;
		fadeInMaxValue = maxValue;
		SetTintValue (0.0f);
		fadingIn = true;
	}

	public void FadeOutTint(float time)
	{
		tintFadeOutTime = time;
		fadingOut = true;
	}	
	
}
