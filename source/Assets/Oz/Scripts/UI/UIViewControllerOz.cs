using UnityEngine;
using System.Collections;
using System.Reflection;

public class UIViewControllerOz : MonoBehaviour
{
	protected Notify notify;
	public Camera myCamera = null;
	private const float fadeTime = 1.0f;
//	private UIPanelAlpha fader;
	
	protected virtual void Awake()
	{
		SetupNotify();
		
		myCamera = UIManagerOz.SharedInstance.UICamera;	//.gameObject.GetComponent<Camera>();
		//myCamera = transform.GetComponentInChildren<Camera>();	// this used to be in Start()
//		fader = gameObject.AddComponent<UIPanelAlpha>();
	}
	
	protected virtual void Start() { }
	
	public void SetupNotify()
	{
		if(notify==null)
		{
			notify = new Notify(this.GetType().Name);
		}
	}
	
	public void Hide(GameObject go)	// used only during initialization and tab switching
	{	
		NGUITools.SetActive(go, false);
		
//		go.AddComponent<FadeAlphaResetToZero>();
		
//		if (fader == null)
//			fader = gameObject.AddComponent<UIPanelAlpha>();
		
//		fader.alpha = 0f;
	}

	public void Show(GameObject go)	// used only during tab switching
	{	
		NGUITools.SetActive(go, true);
		
//		go.AddComponent<FadeAlphaResetToOne>();
//		fader.alpha = 1f;
	}	
	
	public virtual void appear()
	{
		SetupNotify();
		
		NGUITools.SetActive(gameObject, true);

//		FadeIn(fadeTime);
		//fader.alpha = 0f;
		//gameObject.SetActiveRecursively(true);
		//TweenAlpha ta = TweenAlpha.Begin(gameObject, fadeTime, 1f);
			//ta.onFinished += OnShowCollectCoinsTutorial;		
	}
	
	public virtual void disappear()
	{
		NGUITools.SetActive(gameObject, false);

//		FadeOut(fadeTime);	
		//fader.alpha = 1f;
		//TweenAlpha ta = TweenAlpha.Begin(gameObject, fadeTime, 0f);		
			//ta.onFinished += OnShowCollectCoinsTutorial;			
	}
}







//	protected void ShowObject(GameObject theObject, bool show, bool recursive)
//	{
//		if (theObject == null) { return; }
//		
//		if (recursive == true) 
//			theObject.SetActiveRecursively(show); 
//		else
//			theObject.active = show;
//	}



//	private FadePanel DisappearFromFade(float duration = 0f, FadePanel.fadeDoneCallback callback = null)
//	{
//		ShowObject(gameObject, false, true);	//NGUITools.SetActive(gameObject, false);
//		return null;
//	}	
//	
//	protected FadePanel FadeIn(float duration = fadeTime, FadePanel.fadeDoneCallback callback = null)
//	{
//		//UIManagerOz.SharedInstance.PaperVC.UpdateCurrency();	// hack to make sure this gets updated prior to showing
//		
//		//go = (go) ? go : gameObject;	// if no specific gameobject passed in as parameter, use the viewcontroller's gameobject
//		
//		ShowObject(gameObject, true, true);
//		FadePanel fp = gameObject.AddComponent<FadePanel>();
//		fp.SetParameters(true, duration);
//		
//		return fp;
//	}
//	
//	private FadePanel FadeOut(float duration = fadeTime, FadePanel.fadeDoneCallback callback = null)
//	{
//		//go = (go) ? go : gameObject;	// if no specific gameobject passed in as parameter, use the viewcontroller's gameobject
//		
//		FadePanel fp = gameObject.AddComponent<FadePanel>();
//		fp.SetParameters(false, duration);	
//		fp.AddCallback(DisappearFromFade);
//		
//		if (callback != null)
//			fp.AddCallback(callback);	// for adding additional callbacks
//		
//		return fp;
//	}



		//linkupOtherViewControllers();


		//if (gameObject.name != "UIPostGameViewControllerOz(Clone)")
			// && gameObject.name != "UIGatchaViewControllerOz(Clone)")	// hack to not turn off post-run VC and gatchVC, for now

	
//	public void Show()
//	{
//		ShowObject(gameObject, true, true);
//	}	
	


//	protected void updateCurrency() 
//	{
//		PlayerStats player = GameProfile.SharedInstance.Player;
//		UIManagerOz.SharedInstance.PaperVC.SetCurrencyLabel(player.coinCount.ToString(), true, CostType.Coin);
//		UIManagerOz.SharedInstance.PaperVC.SetCurrencyLabel(player.specialCurrencyCount.ToString(), true, CostType.Special);
//	}
	


		//if (firstAppear)
		//{
		//	ShowObject(gameObject, true, true);		//NGUITools.SetActive(gameObject, true);
		//	firstAppear = false;
		//}
		//else
		
		//if (firstDisappear)
		//{
		//	ShowObject(gameObject, false, true);	//NGUITools.SetActive(gameObject, false);
		//	firstDisappear = false;
		//}
		//else		
		
	
	//private bool firstAppear = true;
	//private bool firstDisappear = true;
			
		

	
		//viewDidLoad();	
//	public virtual void viewDidLoad() { }
	


	//public bool wantPaperViewController = true;

//		
//		if (wantPaperViewController)
//		{
//			FadePanel fpPPV = UIManagerOz.SharedInstance.PaperVC.FadeOut(duration);
//			fpPPV.SetParameters(false, duration);
//		}


//	
//		if (wantPaperViewController)
//		{
//			FadePanel fpPPV = UIManagerOz.SharedInstance.PaperVC.FadeIn(duration);
//			fpPPV.SetParameters(true, duration);
//		}


		// paperVC does not inherit from UIViewControllerOz, hence needs to be its own callout.
		//paperViewController = UIManagerOz.SharedInstance.GetInstantiatedObject<UIPaperViewControllerOz>();	
		



		//MainGameCamera = Camera.main;



	//public UIInGameViewControllerOz inGameVC = null;	
	//public UIPaperViewControllerOz paperViewController = null;
	
	//public UIViewControllerOz previousViewController = null;
	//public string backButtonMessage = "OnBackButton";
	//public string titleText = "";
	
	//public bool showPlayButton = true;



//	public virtual void OnBackButton()
//	{
//		//-- Save our current and back for navigation.		
//		if (previousViewController != null) 
//		{
//			disappear();
//			previousViewController.appear();
//		}
//	}

		
		
//		notify.Debug ("UIViewController.appear {0}", this);
		
		//if (wantPaperViewController) 
		//	linkupPaperVC();	
		
//		if (paperViewController != null)	// && wantPaperViewController)
//		{
//			ShowObject(paperViewController.gameObject, true, true);
//			//paperViewController.SetPlayButtonCallback(this.gameObject, "OnPlayClicked");
//			updateCurrency();
//
//		}
		
//		ShowObject(gameObject, true, true);		//NGUITools.SetActive(gameObject, true);

		//if (MainGameCamera != null)
		//	MainGameCamera.enabled = !HideMainCamera;

			
			//NGUITools.SetActive(paperViewController.gameObject, true);
			//paperViewController.SetBackButtonCallback(gameObject, "OnBackButton");
			//paperViewController.SetTitle(titleText);
			//paperViewController.ShowPlayButton(showPlayButton);
			//paperViewController.ShowBackgroundCamera(!HideBackgroundCamera);			
			
			// set 'Play' button target to current view controller
			//if (paperViewController != null) { paperViewController.SetPlayButtonCallback(this.gameObject, "OnPlayClicked"); }



		
		//bool hidePaper = true) 	// from Imangi, added 'hidePaper' parameter
		
		//TR.LOG ("UIViewController.DISappear {0}", this);		
		
//		if (paperViewController != null) 
//		{
//			ShowObject(paperViewController.gameObject, !hidePaper, true);
//			//NGUITools.SetActive(paperViewController.gameObject, !hidePaper);
//		}


	//public Camera MainGameCamera = null;
	//public bool HideMainCamera = true;			// disabled now, main camera is always active
	//public bool HideBackgroundCamera = false;
	


			//if (paperViewController.idolVC.enabled)		// hide idol menu if active
			//	paperViewController.idolVC.disappear();
			


//	public delegate void voidClickedHandler();
//	public static event voidClickedHandler onPlayClickedHandler = null;

	
//	public void OnPlayClicked() 
//	{
//		//Debug.LogWarning("OnPlayClicked in UIObjectivesViewControllerOz!");
//		
//		if (MainGameCamera != null) { MainGameCamera.enabled = true; }
//		disappear();
//		if (inGameVC != null) { inGameVC.appear(); } 
//		if (onPlayClickedHandler != null) { onPlayClickedHandler(); }	//-- Notify an object that is listening for this event.
//	}	

	
//	public virtual void Start()
//	{
//		notify.Debug(this.GetType().Name + ".Start()");
//		linkupOtherViewControllers();
//		MainGameCamera = Camera.main;
//		viewDidLoad();
//	}

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
	

//	public virtual void disappear() 
//	{
//		TR.LOG ("UIViewControllerOz.DISappear {0}", this);
//		
//		ShowObject(gameObject, false, true);
//		//NGUITools.SetActive(gameObject, false);
//		
//		if(paperViewController != null) 
//		{
//			ShowObject(paperViewController.gameObject, false, true);
//			//NGUITools.SetActive(paperViewController.gameObject, false);
//		}
//	}
	
