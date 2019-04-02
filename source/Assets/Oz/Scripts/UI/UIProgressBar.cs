using UnityEngine;
using System.Collections;

public class UIProgressBar : MonoBehaviour 
{
	public static UIProgressBar SharedInstance = null;
	protected static Notify notify;
	
	public UISlider progressBar;
	
	void Awake() 
	{
		SharedInstance = this;
		notify = new Notify(this.GetType().Name);
	}
	
	public static void ShowdProgresBar( bool t) 
	{
//		NGUITools.SetActive(SharedInstance.gameObject, t);
//		SetProgress(0.0f);
	}
	
	public static void SetProgress(float p)
	{
		if( SharedInstance )
		{
//			Debug.LogError("P=" + p);
			SharedInstance.progressBar.sliderValue = p;
		}
	}
	
}
