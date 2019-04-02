/*

using UnityEngine;
using System.Collections;

public class UIViewController : MonoBehaviour
{
	protected static Notify notify;
	//-- Event Delegate Definitions
	public delegate void voidClickedHandler();
	
	public UIPaperViewController 	paperViewController = null;
	public UIViewController			previousViewController = null;
	//public string					backButtonMessage = "OnBackButton";
	public string					titleText = "";
	public bool						showPlayButton = true;
	public Camera					MainGameCamera = null;
	public bool						HideMainCamera = true;
	
	public virtual void Awake()
	{
		notify = new Notify(this.GetType().Name);	
	}
	
	// Use this for initialization
	public virtual void Start ()
	{
		viewDidLoad();
	}
	
	public virtual void viewDidLoad() {
	}

	public virtual void disappear(bool hidePaper) {
		//TR.LOG ("UIViewController.DISappear {0}", this);
		NGUITools.SetActive(gameObject, false);
		if(paperViewController != null) {
			NGUITools.SetActive(paperViewController.gameObject, !hidePaper);
		}
	}
	
	
	public virtual void OnNeedMoreCoinsNo() {
		UIConfirmDialog.onNegativeResponse -= OnNeedMoreCoinsNo;
		UIConfirmDialog.onPositiveResponse -= OnNeedMoreCoinsYes;
	}
	
	public virtual void OnNeedMoreCoinsYes() {
		UIConfirmDialog.onNegativeResponse -= OnNeedMoreCoinsNo;
		UIConfirmDialog.onPositiveResponse -= OnNeedMoreCoinsYes;
		
//		TR.LOG ("FAKE BUY IAP FOR 1000 coins");
//		GameProfile.SharedInstance.Player.coinCount += 1000;
//		updateCurrency();
		OnShowIAPStore();
	}
	
	public virtual void OnNeedMoreGemsNo() {
		UIConfirmDialog.onNegativeResponse -= OnNeedMoreGemsNo;
		UIConfirmDialog.onPositiveResponse -= OnNeedMoreGemsYes;
	}
	
	public virtual void OnNeedMoreGemsYes() {
		UIConfirmDialog.onNegativeResponse -= OnNeedMoreGemsNo;
		UIConfirmDialog.onPositiveResponse -= OnNeedMoreGemsYes;
//		TR.LOG ("FAKE BUY IAP FOR 100 Gems");
//		GameProfile.SharedInstance.Player.specialCurrencyCount += 100;
//		updateCurrency();
		OnShowIAPStore();
	}
	
	public void OnShowIAPStore() {
		if(UIManager.SharedInstance.IAPStoreVC != null) {
			disappear(true);
			
			UIManager.SharedInstance.IAPStoreVC.previousViewController = this;
			UIManager.SharedInstance.IAPStoreVC.appear();
		}
	}
	
	protected void updateCurrency() {
		if(paperViewController != null) {
			PlayerStats player = GameProfile.SharedInstance.Player;
			paperViewController.SetCurrencyLabel(string.Format("{0:#,###0}", player.coinCount), true, CostType.Coin);
			paperViewController.SetCurrencyLabel(string.Format("{0:#,###0}", player.specialCurrencyCount), true, CostType.Special);
		}
	}
	
	public virtual void appear() {
		//TR.LOG ("UIViewController.appear {0}", this);
		QualitySettings.antiAliasing = 0;
		//FlurryBinding.logEvent("appear_"+gameObject.name, false);
		//AnalyticsInterface.LogAnalyticsEvent( "appear_" + gameObject.name );
		
		if(paperViewController != null) {
			NGUITools.SetActive(paperViewController.gameObject, true);
			paperViewController.SetBackButtonCallback(gameObject, "OnBackButton");
			paperViewController.SetTitle(titleText);
			paperViewController.ShowPlayButton(showPlayButton);
			updateCurrency();
		}
		
		NGUITools.SetActive(gameObject, true);
		
		if(MainGameCamera != null && MainGameCamera.enabled == HideMainCamera) {
			MainGameCamera.enabled = !HideMainCamera;
			
			if(GamePlayer.SharedInstance != null && GamePlayer.SharedInstance.ShadowCamera != null && QualitySettings.GetQualityLevel() >= 1) {
				GamePlayer.SharedInstance.ShadowCamera.enabled = !HideMainCamera;	
			}
		}
	}
	
	public virtual void OnBackButton() {
		
		//-- Save our current and back for navigation.
		if( previousViewController != null) 
		{
			disappear(false);
			previousViewController.appear();
		}
	}
//	public virtual void OnBackButton() {
//		
//		TR.LOG ("UIViewController::OnBackButton");
//		
//		//-- Save our current and back for navigation.
//		GameObject currentPanel = null;
//		GameObject previousPanel = null;
//		if(UIManager.SharedInstance.StackCount() >= 1)
//			currentPanel = UIManager.SharedInstance.PopOffStack();
//		
//		if(UIManager.SharedInstance.StackCount() >= 1)
//			previousPanel = UIManager.SharedInstance.PeekAtStack();
//		
//		TR.LOG ("UIViewController::OnBackButton prev={0} cur={1}", previousPanel, currentPanel);
//		
//		if(currentPanel != null) {
//			UIViewController vc = currentPanel.GetComponent<UIViewController>() as UIViewController;
//			if(vc != null) {
//				vc.disappear();
//			}
//			else {
//				//-- hide object tree
//				ShowObject(currentPanel, false, true);		
//			}
//		}
//		
//		if(previousPanel != null) {
//			UIViewController vc = previousPanel.GetComponent<UIViewController>() as UIViewController;
//			if(vc != null) {
//				vc.appear();
//			}
//			else {
//				//-- hide object tree
//				ShowObject(previousPanel, true, true);		
//			}
//		}
//	}
	

}

 */