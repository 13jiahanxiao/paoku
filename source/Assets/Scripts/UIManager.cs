/*
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//-- This purpose of this class is to manager the UI, show/hide/animate.  
//-- It should have no knowledge of the Game Sim. Just show, move, animate, and notify anyone about events.

public class UIManager : MonoBehaviour {
	protected static Notify notify;
	public static UIManager SharedInstance = null;
	
	public UIPaperViewController PaperVC = null;
	public UIViewController characterSelectVC;
	public UIViewController journalVC;
	public UIViewController idolMenuVC;
	public UIViewController freeStoreVC;
	public UIPostGameViewController postGameVC;
	public UIInGameViewController inGameVC;
	public UIInventoryViewController invenotryVC;
	public UIConfirmDialog	confirmDialog;
	public UIConfirmDialog	rewardDialog;
	public UIIAPViewController		IAPStoreVC = null;
	public UIGIftsViewController	GiftsVC = null;
	public UIObjectivesViewController ObjectivesVC = null;
	public UIMoreGamesViewController moreGamesVC = null;
	public UISettingsViewController settingsVC = null;
	public UIStatViewController		statsVC = null;
	public UILeaderboardViewController	leaderboardVC = null;
	//-- These should each be a ViewController.
	
	
	//public GameObject UIObjectivesMenu;
	//-- These should each be a ViewController.
	
	
	public List<UILabel> CoinLabels = null;
	public List<UILabel> SpecialCoinLabels = null;
	
	//public Transform tiltObject = null;
	
	public Camera MainGameCamera = null;
	
	public List<Transform> CharacterCards = null;
	
//	private System.Collections.Generic.Stack<GameObject> panelStack = new System.Collections.Generic.Stack<GameObject>();
//	
//	public void PushOnStack(GameObject obj) {
//		TR.LOG ("PushOnStack");
//		panelStack.Push(obj);
//		foreach(GameObject o in panelStack) {
//			TR.LOG ("ps: {0}", o);
//		}
//	}
//	
//	public GameObject PopOffStack() {
//		TR.LOG ("PopOffStack");
//		GameObject obj = panelStack.Pop();
//		foreach(GameObject o in panelStack) {
//			TR.LOG ("ps: {0}", o);
//		}
//		return obj;
//	}
//	
//	public GameObject PeekAtStack() {
//		TR.LOG ("PeekAtStack");
//		GameObject obj = panelStack.Peek();
//		foreach(GameObject o in panelStack) {
//			TR.LOG ("ps: {0}", o);
//		}
//		return obj;
//	}
//	
//	public int StackCount() {
//		return panelStack.Count;
//	}
	
	
//	private UIPanel _transitionOut = null;
//	private UIPanel _transitionIn = null;
	// Use this for initialization
	public void Start () {
		notify = new Notify(this.GetType().Name);
		
		
		//if (UIPauseMenu != null)
		//{
		//	NGUITools.SetActive(UIPauseMenu, false);
		//}
		if (PaperVC != null)
		{
			NGUITools.SetActive(PaperVC.gameObject, false);
		}
		
		
		if(leaderboardVC) {
			leaderboardVC.disappear(true);
		}
		if(ObjectivesVC) {
			ObjectivesVC.disappear(true);
		}
		if(moreGamesVC) {
			moreGamesVC.disappear(true);
		}
		if(settingsVC) {
			settingsVC.disappear(true);
		}
		if(statsVC) {
			statsVC.disappear(true);
		}
		
		if(postGameVC) {
			postGameVC.disappear(true);
		}
		
		//if(inGameVC) {
		//	inGameVC.disappear(true);
		//	inGameVC.disableResurrectMenu();
		//}
		
		if(IAPStoreVC) {
			IAPStoreVC.disappear(true);
		}
		if(freeStoreVC) {
			freeStoreVC.disappear(true);
		}
		if(GiftsVC) {
			GiftsVC.disappear(true);
		}
		
		if(characterSelectVC) {
			characterSelectVC.disappear(true);	
		}
		if(journalVC) {
			journalVC.disappear(true);	
		}
		if(invenotryVC) {
			invenotryVC.disappear(true);
		}
		if(confirmDialog) {
			NGUITools.SetActive(confirmDialog.gameObject, false);
		}
		if(rewardDialog) {
			NGUITools.SetActive(rewardDialog.gameObject, false);
		}
		
		ChooseAtlasBasedOnScreenResolution();
		
		
		ResizeUIRoots();
		
		if(idolMenuVC != null) {
			idolMenuVC.previousViewController = null;
			idolMenuVC.disappear(true);
		}
	}
	
	void Awake () {
		UIManager.SharedInstance = this;
	}
	
	public enum UIResolutionType
	{
		kResolution480 = 0,
		kResolution960 = 1,
		kResolution1024 = 2,
		kResolution1136 = 3,
		kResolution2048 = 4,
		kResolutionCount
	}
	
	public UIAtlas InterfaceMaster;
	public List<string> InterfaceMasterReplacements = new List<string>();
	
	public UIAtlas DeathMaster;
	public List<string> DeathMasterReplacements = new List<string>();
	
	void ChooseAtlasBasedOnScreenResolution() {
		
		if(InterfaceMaster != null) {
			UIResolutionType chosenType = UIResolutionType.kResolution960;
				switch(Screen.height) {
					case 480:
						chosenType = UIResolutionType.kResolution480;
						break;
					case 960:
						if(GameController.SharedInstance.GetDeviceGeneration() == GameController.DeviceGeneration.iPhone4)
						{
							chosenType = UIResolutionType.kResolution480;
						}
						else
						{
							chosenType = UIResolutionType.kResolution960;
						}
						break;
					case 1024:
						chosenType = UIResolutionType.kResolution1024;
						break;
					case 1136:
						chosenType = UIResolutionType.kResolution1136;
						break;
					case 2048:
						chosenType = UIResolutionType.kResolution2048;
						break;
					default:
					if(Screen.height > 2048) {
						chosenType = UIResolutionType.kResolution2048;
					} 
					break;
					
				}
			
			if(InterfaceMasterReplacements != null && InterfaceMasterReplacements.Count > 0) {
				string fileName = InterfaceMasterReplacements[(int)chosenType];
				notify.Debug ("filename = " + fileName);
				GameObject go = Resources.Load(fileName, typeof(GameObject)) as GameObject;
				notify.Debug ("go = " + go);
				InterfaceMaster.replacement = go.GetComponent<UIAtlas>();
			}
			
			if(DeathMasterReplacements != null && DeathMasterReplacements.Count > 0) {
				string fileName = DeathMasterReplacements[(int)chosenType];
				GameObject go = Resources.Load(fileName, typeof(GameObject)) as GameObject;
				DeathMaster.replacement = go.GetComponent<UIAtlas>();
			}
		}
	}
	
	void ResizeUIRoots() {
		notify.Debug ("ResizeUIRoots screen.height={0}", Screen.height);
		if(Screen.height == 1136) {
			foreach(UIRoot root in UIRoot.list) {
				if(root == null)
					continue;
				notify.Debug ("Setting root height to {0} for {1}", Screen.height, root);
				root.manualHeight = Screen.height;
			}
			
			if(invenotryVC != null) {
				invenotryVC.SizeForScreen();
			}
			if(IAPStoreVC != null) {
				IAPStoreVC.SizeForScreen();
			}
//			if(freeStoreVC != null) {
//				freeStoreVC.SizeForScreen();
//			}
		}
	}
	
	public void EndGame() {
		if(inGameVC) {
			inGameVC.StartResurrectTimer();
		}
	}
	
	public void SetPowerIcon(string iconname) {
		if(inGameVC == null)
			return;
		inGameVC.SetPowerIcon(iconname);
	}
	
	public void ShowPowerIconAndGlow(bool show, string iconname = null) {
		if(inGameVC == null)
			return;
		
		inGameVC.ShowPowerIconAndGlow(show, iconname);
	}
	
	public void ActivePowerIcon() {
		if(inGameVC == null)
			return;
		
		inGameVC.ActivePowerIcon();
	}
	
	public void FadePowerGlow() {
		if(inGameVC == null)
			return;
		
		inGameVC.FadePowerGlow();
	}
	
	public void SetPowerProgress(float progress) {
		if(inGameVC == null)
			return;
		
		inGameVC.SetPowerProgress(progress);
	}
	
	public void SetScore(int score) {
		if(inGameVC == null)
			return;
		
		inGameVC.SetScore(score);
	}
	
	public void SetCoinCount(int coins) {
		if(inGameVC == null)
			return;
		
		inGameVC.SetCoinCount(coins);
	}
	
	public void SetCountDownNumber(int number) {
		if(inGameVC == null)
			return;
		
		inGameVC.SetCountDownNumber(number);
	}
	
	
	
	//-----------------------------------------------------------------------------------------------
	//-- Message Callbacks:  They don't have params coming form NGUI, so we make specific methods
	//-----------------------------------------------------------------------------------------------
	
	
	
	//-----------------------------------------------------------------------------------------------
	//-- Specific Transitions. Its Data, Its Code, Its Both.
	//-----------------------------------------------------------------------------------------------
//	private void TranstionMainMenuFromDownToUP() {
//		GameObject scrollRoot = HierarchyUtils.GetChildByName("ScrollRoot", _mainMenuPanel.gameObject);
//		if(scrollRoot == null) {
//			disableMainMenu();
//			return;
//		}
//		
//		TweenPosition tp = TweenPosition.Begin(scrollRoot, 0.5f, new Vector3(0,Screen.currentResolution.height,0));
//		tp.eventReceiver = this.gameObject;
//		tp.callWhenFinished = "disableMainMenu";
//		tp.method = UITweener.Method.EaseInOut;
//		
//		GameObject fadeRoot = HierarchyUtils.GetChildByName("FadeRoot", _mainMenuPanel.gameObject);
//		if(fadeRoot != null) {
//			FadeTreeOUT(fadeRoot, 0.5f, null);
//		}
//	}
//	
//	private void TranstionMainMenuFromUPToDown() {
//		enableMainMenu();
//		GameObject scrollRoot = HierarchyUtils.GetChildByName("ScrollRoot", _mainMenuPanel.gameObject);
//		if(scrollRoot == null)
//			return;
//		
//		TweenPosition tp = TweenPosition.Begin(scrollRoot, 0.75f, Vector3.zero);
//		tp.method = UITweener.Method.EaseInOut;
//		
//		GameObject fadeRoot = HierarchyUtils.GetChildByName("FadeRoot", _mainMenuPanel.gameObject);
//		if(fadeRoot != null) {
//			FadeTreeIN(fadeRoot, 0.5f, null);
//		}
//	}
	
//	private void FadeJournalIN() {
//		GameObject fadeRoot = HierarchyUtils.GetChildByName("FadeRoot", _journalPanel.gameObject);
//		if(fadeRoot != null) {
//			enableJournalMenu();
//			FadeTreeIN(fadeRoot, 0.5f, null);
//		}
//		else {
//			FadeTreeIN(_journalPanel.gameObject, 0.5f, UIJournalMenu);	
//		}
//		
//		
//	}
//	
//	private void FadeJournalOUT() {
//		GameObject fadeRoot = HierarchyUtils.GetChildByName("FadeRoot", _journalPanel.gameObject);
//		if(fadeRoot != null) {
//			FadeTreeOUT(fadeRoot, 0.5f, "disableJournalMenu");
//		}
//		else {
//			FadeTreeOUT(_journalPanel.gameObject, 0.25f, "disableJournalMenu");	
//		}
//	}
	
	
//	private void FadeCharacterMenuIN() {
//		enableCharacterMenu();
//		FadeTreeIN(_characterPanel.gameObject, 0.5f, null);
//	}
//	
//	private void FadeCharacterMenuOUT() {
//		FadeTreeOUT(_characterPanel.gameObject, 0.25f, "disableCharacterMenu");
//	}
	
	
	//-----------------------------------------------------------------------------------------------
	//-- generic transitions
	//-----------------------------------------------------------------------------------------------
	
	
	
//	private void FadeTreeIN(GameObject root, float duration, GameObject enableObject) {
//		ShowObject(enableObject, true, true);
//		
//		UIWidget[] widgets = root.GetComponentsInChildren<UIWidget>();
//		TweenColor tc = null;
//		for (int i = 0, imax = widgets.Length; i < imax; ++i)
//		{
//			UIWidget w = widgets[i];
//			if(w == null)
//				continue;
//			
//			w.color = Color.clear;
//			tc = TweenColor.Begin(w.gameObject, duration, Color.white);
//			tc.method = UITweener.Method.EaseInOut;
//		}
//		
//		Renderer[] models = root.GetComponentsInChildren<Renderer>();
//		for(int i=0, imax = models.Length; i<imax; ++i)
//		{
//			Renderer r = models[i];
//			if(r == null)
//				continue;
//			
//			r.material.color = Color.clear;
//			tc = TweenColor.Begin(r.gameObject, duration, Color.white);
//			tc.method = UITweener.Method.EaseInOut;
//		}
//	}
//	
//	private void FadeTreeOUT(GameObject root, float duration, string callWhenFinished) {
//		UIWidget[] widgets = root.GetComponentsInChildren<UIWidget>();
//		TweenColor tc = null;
//		for (int i = 0, imax = widgets.Length; i < imax; ++i)
//		{
//			UIWidget w = widgets[i];
//			if(w == null)
//				continue;
//			
//			w.color = Color.white;
//			tc = TweenColor.Begin(w.gameObject, duration, Color.clear);
//			tc.method = UITweener.Method.EaseInOut;
//			
//			if(i == 0) {
//				tc.eventReceiver = this.gameObject;
//				tc.callWhenFinished = callWhenFinished;
//			}
//		}
//		
//		Renderer[] models = root.GetComponentsInChildren<Renderer>();
//		for(int i=0, imax = models.Length; i<imax; ++i)
//		{
//			Renderer r = models[i];
//			if(r == null)
//				continue;
//			
//			r.material.color = Color.white;
//			tc = TweenColor.Begin(r.gameObject, duration, Color.clear);
//			tc.method = UITweener.Method.EaseInOut;
//		}
//	}
	
	
	public void ShowConfirmDialog(string title, string description, string negativeButtonText, string positiveButtonText) {
		if(confirmDialog == null)
			return;
		
		confirmDialog.ShowConfirmDialog(title, description, negativeButtonText, positiveButtonText);
	}
//	public void TiltDevice(float tilt) {
//		if(tiltObject == null)
//			return;
//		
//		//-- Tilt is between -1 and 1
//		float alpha = (tilt + 1) / 2.0f;
//		float yRotation = Mathf.Lerp(-40.0f, 40.0f, alpha);
//		tiltObject.rotation = Quaternion.Euler(0, yRotation, 0);
//	}
	
	//-- Event Delegate Definitions
	public delegate void voidClickedHandler();
	
	//-- The Handlers for the event.
	
	//public static event voidClickedHandler 	onShowAchievementsClicked = null;
}
 
*/