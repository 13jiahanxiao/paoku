using UnityEngine;
using System.Collections;


public class TintGeoUI : MonoBehaviour {
	public float tintAmount = 2.0f;
	
	private float timeSinceTintStart = 0.0f;
	
	private bool animateTint = false;
	
	private TweenColor tc;
	
	// Use this for initialization
	void Start () {
		tc = transform.parent.GetComponentInChildren<TweenColor>();
	}
	
	// Update is called once per frame
	void Update () {
		if(animateTint)
		{
			timeSinceTintStart += Time.deltaTime;
			if(renderer.material.HasProperty("_TintValue"))
				renderer.material.SetFloat ("_TintValue", tc.color.a/tintAmount);
		}
	}
	
	public void StartTintAnimation()
	{
		animateTint = true;
		ResetTint();
	}

	public void EndTintAnimation()
	{
		animateTint = false;
		ResetTint();
	}
	
	public void ResetTint()
	{
		if(renderer.material.HasProperty("_TintValue"))
			renderer.material.SetFloat ("_TintValue",0.0f);
	}
	
}
