/*

using UnityEngine;
using System.Collections;

public class UIIdolMenuViewController : UIViewController
{
	public UIViewController mainMenuVC = null;
	public UIViewController inGameVC = null;
	
	public UITexture jefftest = null;
	
	public Transform scrollRoot = null;
	public UIPanelAlpha fadeRoot = null;
	
	private Vector3 scrollRootStartPosition = Vector3.zero;
	public new void Start() {
		if(fadeRoot != null) {
			fadeRoot.alpha = 0.0f;
		}
	}
	
	public void OnMainMenuClicked() {
		
		disappear(true);
		
		//-- Show the main menu and leave a bread crumb to this VC.
		if(mainMenuVC != null) {
			mainMenuVC.previousViewController = this;
			mainMenuVC.appear();
		}
	}
	
	public static event voidClickedHandler 	onPlayClickedHandler = null;
	public void OnPlayClicked() {
		
		if(MainGameCamera != null) {
			MainGameCamera.enabled = true;
			if(GamePlayer.SharedInstance != null && GamePlayer.SharedInstance.ShadowCamera != null && QualitySettings.GetQualityLevel() >= 1) {
				GamePlayer.SharedInstance.ShadowCamera.enabled = true;	
			}
		}
		
		disappear(true);
		
		//if(inGameVC != null) {
		//	inGameVC.appear();
			//ShowObject(activePowerIcon.gameObject, false, false);
		//}
		
		//-- Notify an object that is listening for this event.
		if(onPlayClickedHandler != null) {
			onPlayClickedHandler();
		}	
	}
	
	public override void appear() {
		base.appear();
		GameController.SharedInstance.SetMSAAByDevice();
		
		scrollRoot.localPosition = new Vector3(scrollRootStartPosition.x, Screen.height, scrollRootStartPosition.z);
		TweenPosition tp = TweenPosition.Begin(scrollRoot.gameObject, 0.25f, scrollRootStartPosition);
		if(tp) {
			tp.method = UITweener.Method.EaseInOut;
		}
		fade = true;
		if(fadeRoot != null) {
			fadeRoot.alpha = 0.0f;
			fadeRoot.enabled = true;
		}
		
		if(jefftest != null) {
//			iOSInfo.MakesNameTexture("jefftest");
//			jefftest.mainTexture = iOSInfo.rttString;
		}
	}
	
	private bool fade = false;
	public void Update() {
		if(fade == false || fadeRoot == false)
			return;
		fadeRoot.alpha += Time.deltaTime;
	}
}

 */
