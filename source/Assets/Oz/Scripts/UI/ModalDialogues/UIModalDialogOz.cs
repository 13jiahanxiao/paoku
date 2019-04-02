using UnityEngine;
using System;
using System.Collections;

public class UIModalDialogOz : MonoBehaviour 
{
	//private BackgroundColliderDisabler bgColliderDisabler;
	
	private GameObject alphaBGforModalDialogs;
	
	protected static Notify notify;
	
	protected virtual void Awake()
	{
		SetupNotify();
	}
	
	protected void SetupNotify()
	{
		if(notify==null)
		{
			notify = new Notify(this.GetType().Name);
		}
	}
	
	
	void Start()
	{
		alphaBGforModalDialogs = (GameObject)Instantiate(Resources.Load("Oz/Prefabs/AlphaBGforModalDialogs"));
		alphaBGforModalDialogs.transform.parent = gameObject.transform.Find("Camera/CenterAnchor");
		alphaBGforModalDialogs.transform.localPosition = new Vector3(0.0f, 0.0f, 3.0f);
		alphaBGforModalDialogs.transform.localScale = new Vector3(5000.0f, 5000.0f, 1.0f);		
	}
	
	void OnEnable()		// disable background colliders
	{
		UIManagerOz.SharedInstance.SetUICameraLayerMask(true);		// only modal dialog layer should receive events
		UIManagerOz.SharedInstance.AddToActiveList(this as UIModalDialogOz);
		
//		if (bgColliderDisabler == null)
//			bgColliderDisabler = gameObject.AddComponent<BackgroundColliderDisabler>();		// add script instance
		
//		bgColliderDisabler.DisableAllBackgroundColliders(gameObject, UIManagerOz.SharedInstance.gameObject);
	}
	
	void OnDisable()	// re-enable all background colliders
	{
		UIManagerOz.SharedInstance.SetUICameraLayerMask(false);		// all layers will now receive events again
		UIManagerOz.SharedInstance.RemoveFromActiveList(this as UIModalDialogOz);
		
//		bgColliderDisabler.EnableAllBackgroundColliders();
	}
}
