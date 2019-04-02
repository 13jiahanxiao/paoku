using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public enum AbilityUsed
{
	None = 0,
	Mod1 = 1,
	Mod2 = 2,
	Mod3 = 4,
	Mod4 = 8,
	Mod5 = 16,
	Cons1 = 32,
	Cons2 = 64,
	Cons3 = 128,
	Cons4 = 256,
	Cons5 = 512,
	AllInGame = 1023,
	Pow1 = 1024,
	Pow2 = 2048,
	Pow3 = 4096,
	Pow4 = 8192,
	Pow5 = 16384,
	Pow6 = 32768,
	All = 65535,
//	All = 131071,
}




public enum BonusButtonType
{
	Consumable,
	Modifier,
	Pickup,
	Power,
}


public class BonusButtons : MonoBehaviour
{
	protected static Notify notify;
	
	//Modifier ID's
	public int DoubleCoinsID = 0;
	public int DiscountID = 0;
	public int PowerDurationID = 0;
	public int CoinMeterSpeedID = 0;
	public int LuckID = 0;
	
	//Modifier Button Objects
	public GameObject DoubleCoinsButton;
	public GameObject DiscountButton;
	public GameObject PowerDurationButton;
	public GameObject CoinMeterSpeedButton;
	public GameObject LuckButton;
	
	
	//Modifier ID's
	public int HeadStartID = 0;
	public int BigHeadStartID = 0;
	public int BonusMultiplierID = 0;
	public int StumbleProofID = 0;
	public int ThirdEyeID = 0;
	
	//Modifier Button Objects
	public GameObject HeadStartButton;
	public GameObject BigHeadStartButton;
	public GameObject BonusMultiplierButton;
	public GameObject StumbleProofButton;
	public GameObject ThirdEyeButton;
	
	public GameObject GemCountLabel;
	public Transform gemTransform;
	
	//Pickup Image Objects
	public GameObject BoostImage;
	public GameObject PoofImage;
	public GameObject MagnetImage;
	public GameObject MegaCoinImage;
	public GameObject TornadoTokenImage;
	public GameObject GemImage;
	public GameObject ScoreBonusImage;
	
	
	public Transform displayLocator;
	
	public ParticleSystemRenderer glow1;
	public ParticleSystemRenderer glow2;
	
	//public UILocalize DescriptionLabelLocalization;
	public UILabel DescriptionLabel;
	
	
	private List<GameObject> purchasedModifiers = new List<GameObject>();
	private List<GameObject> purchasedConsumables = new List<GameObject>();
	
	
	
	private const float PIXELS_TO_OFFSCREEN = 150f;
	private const float ANIMATION_BOUNCINESS = 1.5f;
	private const float ANIMATION_SPEED = 3f;
	
	[HideInInspector]
	public GameObject tutorialButtonAbility;
	[HideInInspector]
	public GameObject tutorialButtonUtility;
	
	private int UsedAbilityFlags = 0;
	
	
	
	public static BonusButtons main
	{
		get; private set;
	}
	
	
	
	
	void Awake()
	{
		notify = new Notify(this.GetType().Name);	
		main = this;
		
		UIManagerOz.onPauseClicked += OnPause;
	}
	
	
	void OnPause()
	{
		//HideConsumableAndModifierButtons();
	}
	
	
	void Start()
	{
		if(GameProfile.SharedInstance==null)
		{
			ShowAllButtons();
		}
		DeactivatePickupButtons();
		//DescriptionLabelLocalization.GetComponent<UILabel>().text = "";
		DescriptionLabel.text = "";
		if(gemTransform!=null)	gemTransform.renderer.enabled = false;
	}
	
	void OnEnable()
	{
		//DescriptionLabelLocalization.GetComponent<UILabel>().text = "";
		DescriptionLabel.text = "";
		DeactivatePickupButtons();
		
	//	TweenAlpha.Begin(DescriptionLabel.gameObject,0f,0f);
		DescriptionLabel.transform.localPosition = new Vector3(0,400,-5);	//The -5 is so that it shows up in front of any other In-Game UI (overlap problems in Russian)
	}
	
	public void EnableAllButtons(bool on)
	{
		notify.Debug ("EnableAllButtons " + on);
		DoubleCoinsButton.collider.enabled = on;
		DiscountButton.collider.enabled = on;
		PowerDurationButton.collider.enabled = on;
		CoinMeterSpeedButton.collider.enabled = on;
		LuckButton.collider.enabled = on;
		HeadStartButton.collider.enabled = on;
		BigHeadStartButton.collider.enabled = on;
		BonusMultiplierButton.collider.enabled = on;
		StumbleProofButton.collider.enabled = on;
		ThirdEyeButton.collider.enabled = on;
	}
	
	public void EnableAllButtonsAbility(bool on)
	{
		notify.Debug ("EnableAllButtonsAbility " + on);
		DoubleCoinsButton.collider.enabled = on;
		DiscountButton.collider.enabled = on;
		PowerDurationButton.collider.enabled = on;
		CoinMeterSpeedButton.collider.enabled = on;
		LuckButton.collider.enabled = on;
	}
	public void EnableAllButtonsUtility(bool on)
	{
		notify.Debug ("EnableAllButtonsUtility " + on);
		HeadStartButton.collider.enabled = on;
		BigHeadStartButton.collider.enabled = on;
		BonusMultiplierButton.collider.enabled = on;
		StumbleProofButton.collider.enabled = on;
		ThirdEyeButton.collider.enabled = on;
	}

/*	void DeactivateAllButtons()
	{
		DeactivatePickupButtons();
		GemCountLabel.SetActiveRecursively(false);
		DoubleCoinsButton.SetActiveRecursively(false);
		DiscountButton.SetActiveRecursively(false);
		PowerDurationButton.SetActiveRecursively(false);
		CoinMeterSpeedButton.SetActiveRecursively(false);
		LuckButton.SetActiveRecursively(false);
		HeadStartButton.SetActiveRecursively(false);
		BigHeadStartButton.SetActiveRecursively(false);
		BonusMultiplierButton.SetActiveRecursively(false);
		StumbleProofButton.SetActiveRecursively(false);
		ThirdEyeButton.SetActiveRecursively(false);
	}*/
	
	public void DeactivatePickupButtons()
	{
		BoostImage.SetActiveRecursively(false);
		PoofImage.SetActiveRecursively(false);
		MagnetImage.SetActiveRecursively(false);
		MegaCoinImage.SetActiveRecursively(false);
		TornadoTokenImage.SetActiveRecursively(false);
		GemImage.SetActiveRecursively(false);
		ScoreBonusImage.SetActiveRecursively(false);
	}
	
	public void ShowAllButtons()
	{
		
		EmergePurchasedModifierButtons(false);
		EmergePurchasedConsumableButtons(false);
		
		currentSprite = null;
			
		//if(purchasedModifiers.Count>0)
		if(GameProfile.SharedInstance.Player.artifactsPurchased.Count > 0)
		{
			ShowButton(GemCountLabel,true,false,GameProfile.SharedInstance.Player.GetGemCount());
			UpdateGemCount();
		}
		else
		{
			GemCountLabel.SetActiveRecursively(false);
		}
		
		EmergePurchasedModifierButtons(true);
		EmergePurchasedConsumableButtons(true);
		DeactivatePickupButtons();
		
		StopCoroutine("BlinkIcons");
		
		StartCoroutine("BlinkIcons");
		
		//Hide buttons in 7 seconds
		HideConsumableAndModifierButtons();
	}

/*	public void HideAllButtons()
	{
		DeactivateAllButtons();
	}
	*/
	
	public void HideConsumableAndModifierButtons()
	{
		CancelHideConsumableAndModifierButtons();	
		
		StartCoroutine("HideConsumableAndModifierButtons_internal");
	}
	
	private IEnumerator HideConsumableAndModifierButtons_internal()
	{
		yield return new WaitForSeconds(7f);
		
		HideConsumableAndModifierButtonsNow();
	}
	
	public void HideConsumableAndModifierButtonsNow()
	{
		if(purchasedModifiers.Contains(GemCountLabel))
			ShowButton(GemCountLabel,false,false,GameProfile.SharedInstance.Player.GetGemCount());
		
		EmergePurchasedModifierButtons(false);
		EmergePurchasedConsumableButtons(false);
		
		CancelHideConsumableAndModifierButtons();
	}
	
	
	public void CancelHideConsumableAndModifierButtons(){
		StopCoroutine("HideConsumableAndModifierButtons_internal");
	}
	
	
	public bool CanShowModifiers()
	{
		if(GameProfile.SharedInstance!=null)
		{
			if(GameProfile.SharedInstance.Player.IsArtifactPurchased(DoubleCoinsID))
				return true;
			//This one doesnt count; it's an addition to the boost. If someone only buys this, it's just going to have to be an exception.
			//if(GameProfile.SharedInstance.Player.IsArtifactPurchased(DiscountID))
			//	return true;
			if(GameProfile.SharedInstance.Player.IsArtifactPurchased(PowerDurationID))
				return true;
			if(GameProfile.SharedInstance.Player.IsArtifactPurchased(CoinMeterSpeedID))
				return true;
			if(GameProfile.SharedInstance.Player.IsArtifactPurchased(LuckID))
				return true;
		}
		return false;
	}
	
	
	public bool CanShowConsumables()
	{
		if(GameProfile.SharedInstance!=null)
		{
			if(GameProfile.SharedInstance.Player.GetConsumableCount(HeadStartID) > 0)
				return true;
			if(GameProfile.SharedInstance.Player.GetConsumableCount(BigHeadStartID) > 0)
				return true;
			if(GameProfile.SharedInstance.Player.GetConsumableCount(BonusMultiplierID) > 0)
				return true;
			if(GameProfile.SharedInstance.Player.GetConsumableCount(StumbleProofID) > 0)
				return true;
			if(GameProfile.SharedInstance.Player.GetConsumableCount(ThirdEyeID) > 0)
				return true;
		}
		return false;
	}
	
	
	
	//TODO: Consider moving the root object instead of each button (would require turning off all other buttons)
	private void EmergePurchasedModifierButtons(bool show)
	{
		if(!GameController.SharedInstance.abilityTutorialOn)
			tutorialButtonAbility = null;
		
		if(show)
			StopCoroutine("BlinkIcons");
		if(GameProfile.SharedInstance!=null)
		{
			if(GameProfile.SharedInstance.Player.IsArtifactPurchased(DoubleCoinsID))
				ShowButton(DoubleCoinsButton,show,false);
			else
				DoubleCoinsButton.SetActiveRecursively(false);
			
			if(GameProfile.SharedInstance.Player.IsArtifactPurchased(DiscountID) && !show)
				ShowButton(DiscountButton,show,false);
			else
				DiscountButton.SetActiveRecursively(false);
			
			if(GameProfile.SharedInstance.Player.IsArtifactPurchased(PowerDurationID))
				ShowButton(PowerDurationButton,show,false);
			else
				PowerDurationButton.SetActiveRecursively(false);
			
			if(GameProfile.SharedInstance.Player.IsArtifactPurchased(CoinMeterSpeedID))
				ShowButton(CoinMeterSpeedButton,show,false);
			else
				CoinMeterSpeedButton.SetActiveRecursively(false);
			
			if(GameProfile.SharedInstance.Player.IsArtifactPurchased(LuckID))
				ShowButton(LuckButton,show,false);
			else
				LuckButton.SetActiveRecursively(false);
		}
		if(!show)
			purchasedModifiers.Clear();
	}
	
	private void EmergePurchasedConsumableButtons(bool show)
	{
		if(!GameController.SharedInstance.utilityTutorialOn)
			tutorialButtonUtility = null;
		
		if(show)
			StopCoroutine("BlinkIcons");
		if(GameProfile.SharedInstance!=null)
		{
			//NOTE: These calls will automatically short-circuit if we pass in '0' for the consumable count.
			ShowButton(HeadStartButton,show,true,GameProfile.SharedInstance.Player.GetConsumableCount(HeadStartID));
			ShowButton(BigHeadStartButton,show,true,GameProfile.SharedInstance.Player.GetConsumableCount(BigHeadStartID));
			ShowButton(BonusMultiplierButton,show,true,GameProfile.SharedInstance.Player.GetConsumableCount(BonusMultiplierID));
			ShowButton(StumbleProofButton,show,true,GameProfile.SharedInstance.Player.GetConsumableCount(StumbleProofID));
			ShowButton(ThirdEyeButton,show,true,GameProfile.SharedInstance.Player.GetConsumableCount(ThirdEyeID));
		}
		if(!show)
			purchasedConsumables.Clear();
	}
	
	
	
	float Bump(float total, float t, float bumpiness)
	{
		return t*total + t*total*bumpiness*(1f-t);
	}
	
	
	public void EnableAllCountLabels(bool enable)
	{
		EnableCount(HeadStartButton,enable);
		EnableCount(BigHeadStartButton,enable);
		EnableCount(BonusMultiplierButton,enable);
		EnableCount(StumbleProofButton,enable);
		EnableCount(ThirdEyeButton,enable);
	}
	
	public void EnableCount(GameObject go, bool enable)
	{
		BonusButton bb = go.GetComponent<BonusButton>();
		if(bb!=null)
		{
			UISprite spr = bb.amtSprite;
			UILabel lbl = bb.amtLabel;
			if(spr!=null)	spr.enabled = enable;
			if(lbl!=null)	lbl.enabled = enable;
		}
	}
	
	
	/*void Bounce(Transform tr)
	{
		StartCoroutine(Bounce_internal(tr));
	}
	
	IEnumerator Bounce_internal(Transform tr)
	{
		const float speed = 3f;
		const float maxScale = -0.25f;
		
		Vector3 defScale = tr.localScale;
		float t = 0f;
		float scaleFactor = 1f;
		
		while(t<1f)
		{
			t = Mathf.MoveTowards(t,1f,speed*Realtime.deltaTime);
			scaleFactor = 1 + maxScale*4f*(t-t*t);
			tr.localScale = defScale*scaleFactor;
			
			yield return null;
		}
	}
	*/
	
	public void ShowButton(GameObject go, bool show, bool isConsumable, int count = 1)
	{
		if(!show && (go == tutorialButtonAbility || go == tutorialButtonUtility))
			return; // don't hide the button if it's a tutorial button, since we need to display it
		
		EnableCount(go,true);
		
		if(show && !go.active)
		{
			go.SetActiveRecursively(true);
		}
		if(go.active)
		{
			StartCoroutine(ShowButton_internal(go,show,isConsumable,count));
		}
		
		// reset back z since we move it on tutorial to be infront of fade
		go.transform.localPosition = new Vector3(go.transform.localPosition.x, go.transform.localPosition.y, 0f);
		
		if(show && isConsumable && count > 0 && !GameController.SharedInstance.utilityTutorialPlayed){
			tutorialButtonUtility = go; // this is the last utility button activated
		}
		if(show && !isConsumable && go != GemCountLabel && !GameController.SharedInstance.abilityTutorialPlayed){
			tutorialButtonAbility = go; // this is the last ability button activated
		}
	}
	
	
	
	private IEnumerator ShowButton_internal(GameObject go, bool show, bool isConsumable, int count = 1)
	{
		//Short-circuit if this is a consumable and we dont have any, and we are trying to SHOW this button
		if(show && count<=0 && isConsumable)
		{
			go.SetActiveRecursively(false);
			yield break;
		}
		
		
		UIPanelAlpha panel = GetComponent<UIPanelAlpha>();
		
		panel.alpha = 1f;
		
		List<GameObject> buttonList = isConsumable ? purchasedConsumables : purchasedModifiers;
		
		if(buttonList.Contains(go)==show && go!=GemCountLabel)
		{
			//Shouldnt need this. If we are calling show_button(false) and this GO isnt in the array, we will disable it later in Display_button
		//	if(!show)
		//	{
		//		go.SetActiveRecursively(false);
		//	}
			yield break;
		}
		
		
		//Add or remove this from our "currently out" list
		if(show)	buttonList.Add(go);
		else 		buttonList.Remove(go);
		
		//If it has a label, set the label to be the number that was passed in
		if(show)
		{
			UILabel label = go.GetComponentInChildren<UILabel>();
			if(label!=null)	label.text = count.ToString();
		}
		
		//Find a Y pos based on how many buttons are in this list already
		float yPos = show ? -600 + buttonList.Count*100f : go.transform.localPosition.y;
		if(!isConsumable && show)	yPos -= 100;
		
		//Turn the button attatched to it on or off
		if(go.collider)
			go.collider.enabled = show;
		
		//Set up some initial values based on if we are showing or not, and whether its a consumable or modifier
		float curT = show ? 0f : 1f;
		float target = show ? 1f : 0f;
		float offscreenOffset = isConsumable ? PIXELS_TO_OFFSCREEN : -PIXELS_TO_OFFSCREEN;
		
		Vector3 tempVec3 = Vector3.zero;
		
		//Move the button on or off screen
		while(curT!=target && buttonList.Contains(go)==show)
		{
			curT = Mathf.MoveTowards(curT,target,Realtime.deltaTime*ANIMATION_SPEED);

			tempVec3 = go.transform.localPosition;
			tempVec3.x = Bump(-offscreenOffset,curT,ANIMATION_BOUNCINESS)+offscreenOffset;
			tempVec3.y = yPos;
			go.transform.localPosition = tempVec3;
			
			yield return null;
		}
		if(!show)
		{
			yield return new WaitForSeconds(6f);	//Wait, so that its not deactivated while its being shown
			go.SetActiveRecursively(false);
		}
		yield break;
	}
	
	private int cur_glow_id = 0;
	IEnumerator ShowGlow(bool show, Color baseColor, float delay = 0f)
	{
#if !UNITY_EDITOR
		if(GameController.SharedInstance.GetDeviceTier() < 3)
		{
			yield break;
		}
#endif
		
//		Debug.Log("Glow! "+show+" "+Realtime.time);
		
		int myId = ++cur_glow_id;
		
		yield return StartCoroutine(Realtime.WaitForSeconds(delay));
		
		if(myId!=cur_glow_id)	yield break;
		
		if(show)
		{
			glow1.particleSystem.Play();
			glow2.particleSystem.Play();
		}
		else
		{
			glow1.particleSystem.Stop();
			glow2.particleSystem.Stop();
		}
		
		float target = show ? 1f : 0f;
		float start = show ? 0f : 1f;
		float speed = show ? 2f : 4f;
		
		Color col = baseColor;
		
		float t = start;
		while(t!=target && myId==cur_glow_id)
		{
			t = Mathf.MoveTowards(t,target,Realtime.deltaTime*speed);
			
			col.a = Mathf.Sqrt(t)/4f;
			
			glow1.material.SetColor("_TintColor",col);
			glow2.material.SetColor("_TintColor",col);
			
			yield return null;
		}
	}
	
	public IEnumerator ShiftButton(GameObject go)
	{
		float t = 0f;
		
		while (t < 100f)
		{
			float moveDist = ((100f-t)+20f)*Realtime.deltaTime*5f;
			
			go.transform.localPosition -= Vector3.up*moveDist;
			t+=moveDist;
			
			yield return null;
		}
	}
	
	
	static int move_id = 0;
	private IEnumerator ShowText(bool on)
	{
		int id = ++move_id;
		Vector3 target = on? new Vector3(0,100,-5) : new Vector3(0,300,-5);	//-5 is for overlap issues, so it looks right in Russian
		while(id==move_id && DescriptionLabel.transform.localPosition!=target)
		{
			DescriptionLabel.transform.localPosition = Vector3.Lerp(DescriptionLabel.transform.localPosition,target,Realtime.deltaTime*10f);
			yield return null;
		}
	}
	
	
	public void DisplayButton(GameObject go, BonusButtonType type, string translateKey = "")
	{
		//DescriptionLabel.alpha = 1f;
		if(!go.active)
		{
			go.SetActiveRecursively(true);
		}
		// set button back on z , since we may have moved it forward in tutorial
		go.transform.localPosition = new Vector3(go.transform.localPosition.x, go.transform.localPosition.y, 0f);
		//go.GetComponent<UIPanel>().widgetsAreStatic = false;
		
		EnableCount(go,false);
		
		StartCoroutine(DisplayButton_internal(go,type,translateKey));
	}
	
	void LateUpdate()
	{
		if(currentSprite!=null)
			currentSprite.alpha = 1f;
	}
	
	
	public static bool isDisplaying = false;
	public static int cur_show_id = 0;
	private UISprite currentSprite;
	public IEnumerator DisplayButton_internal(GameObject go, BonusButtonType type, string translateKey = "")
	{
		int my_id = ++cur_show_id;
		
		currentSprite = go.GetComponentInChildren<UISprite>();
		
		isDisplaying = true;
		PopupNotification.PopupList[PopupNotificationType.Generic].FadeOut();
		PopupNotification.PopupList[PopupNotificationType.Objective].FadeOut();
		FastTravelButton.FadeOut();
		UIManagerOz.SharedInstance.inGameVC.FadeOutEnvProgress();
		
//		TweenAlpha.Begin(DescriptionLabel.gameObject, 0.3f, 1f);
		StartCoroutine(ShowText(true));
		int id = move_id;
		
		Vector3 origPos = go.transform.localPosition;
		
		List<GameObject> buttonList = null;
		if(type==BonusButtonType.Consumable)
			buttonList = purchasedConsumables;
		else if(type==BonusButtonType.Modifier)
			buttonList = purchasedModifiers;
		
		if(buttonList!=null)
		{
			for(int i=buttonList.Count-1;i>=0 && go!=buttonList[i];i--)
			{
				StartCoroutine(ShiftButton(buttonList[i]));
			}
		}
		
		if(buttonList!=null)	buttonList.Remove(go);
		
		if(go.collider)
			go.collider.enabled = false;
		
		int glowId = cur_glow_id+1;
		StartCoroutine(ShowGlow(true, type==BonusButtonType.Modifier ? Color.green : Color.white, 0.25f));
		
		
		//TODO: Clean this up.
		float t = 0f;
		Vector3 defScale = go.transform.localScale;
		float scalespeed = 3f;
		float speed = type==BonusButtonType.Pickup ? 7f : 3.75f;
		//float parabolicMax = -0.25f;
		float maxScale = 4.5f;
		float scaleFactor = 0f;
		Transform tr = go.transform;
		
//		string int_key = translateKey;
//		DescriptionLabelLocalization.GetComponent<UILabel>().enabled = true;
//		DescriptionLabelLocalization.key = translateKey;
//		DescriptionLabelLocalization.Localize();
		DescriptionLabel.text = Localization.SharedInstance.Get(translateKey);
		
		if(type==BonusButtonType.Modifier)
		{
			gemTransform.position = GemCountLabel.transform.position;
		}
		
		Vector3 offset = new Vector3(-.1f,-.1f);
		
		while(t<maxScale && !GameController.SharedInstance.IsPaused && !GamePlayer.SharedInstance.IsDead && !GamePlayer.SharedInstance.Dying && (cur_show_id==my_id || tr.position!=displayLocator.position))
		{
			t = Mathf.MoveTowards(t,maxScale,scalespeed*Realtime.deltaTime);
			if(t<1)
				scaleFactor = 1 + (t*t-t);
			else
				scaleFactor = 1 + Mathf.Sqrt(t)/4f - 0.25f;//+ parabolicMax*4f*(t-t*t);
			tr.localScale = defScale*scaleFactor;
			Vector3 targ = displayLocator.position;
			tr.position = Vector3.MoveTowards(tr.position,targ,Realtime.deltaTime*speed);
			if(type==BonusButtonType.Modifier && t>1f)	
			{
				gemTransform.renderer.enabled = true;
				gemTransform.position = Vector3.MoveTowards(gemTransform.position,displayLocator.position+offset,Realtime.deltaTime*speed);
				gemTransform.RotateAround(Vector3.up,Realtime.deltaTime*5f);
			}
			
			if(t>1.5f && glowId==cur_glow_id)
				StartCoroutine(ShowGlow(false,type==BonusButtonType.Modifier ? Color.green : Color.white));
			
			
			yield return null;
		}
		//If we didn't already, turn off the glow
		if(t<=1.5f && glowId==cur_glow_id)
			StartCoroutine(ShowGlow(false,type==BonusButtonType.Modifier ? Color.green : Color.white));
		
//		if(glowId==cur_glow_id)
//			StartCoroutine(ShowGlow(false,type==BonusButtonType.Modifier ? Color.green : Color.white));
		
		t=1f;
		speed = 0f;
		Vector3 defPos = tr.position;
		while(t>0f)
		{
			float amt = t + (t-t*t)*2f;
			tr.position = defPos + Vector3.up * (1f - amt * 1f);
			if(type==BonusButtonType.Modifier)
			{
				gemTransform.renderer.enabled = true;
				gemTransform.position = tr.position+offset;
				gemTransform.RotateAround(Vector3.up,Realtime.deltaTime*5f);
			}
			
			t-=Realtime.deltaTime*5f;
			
			yield return null;
		}
		
		gemTransform.renderer.enabled = false;
		
		/* comment out for now
		if(int_key == DescriptionLabelLocalization.key)
		{
			DescriptionLabelLocalization.GetComponent<UILabel>().enabled = false;
		}
		*/
		if(id==move_id)
			StartCoroutine(ShowText(false));
		
		tr.localScale = defScale;
		
		if(type==BonusButtonType.Consumable)	origPos.x += PIXELS_TO_OFFSCREEN;
		else if(type==BonusButtonType.Modifier)	origPos.x -= PIXELS_TO_OFFSCREEN;
		go.transform.localPosition = origPos;
		
	//	TweenAlpha.Begin(DescriptionLabel.gameObject, 0.3f, 0f);
		//DescriptionLabel.alpha = 0f;
		go.SetActiveRecursively(false);
		
		PopupNotification.PopupList[PopupNotificationType.Generic].FadeIn();
		PopupNotification.PopupList[PopupNotificationType.Objective].FadeIn();
		FastTravelButton.FadeIn();
		UIManagerOz.SharedInstance.inGameVC.FadeInEnvProgress();
		
		isDisplaying = false;
		
		if(my_id==cur_show_id)
			DeactivatePickupButtons();
		
	//	Debug.Log("fin");
		
		yield break;
	}
	
	void UpdateGemCount()
	{
		UILabel label = GemCountLabel.GetComponentInChildren<UILabel>();
		int gemCount = GameProfile.SharedInstance.Player.GetGemCount();
		if(label!=null)	label.text = gemCount.ToString();
		
		if(gemCount<=0)
		{
			//EmergePurchasedModifierButtons(false);	// don't hide anymore if no gems, go to mini store instead if clicked
			StartCoroutine(BlinkGemCountLabel());
		}
	}
	
	IEnumerator BlinkGemCountLabel()
	{
		float t=0f;
		Color col = Color.gray;
		
		UISprite gemsprite = GemCountLabel.GetComponentInChildren<UISprite>();
		
		//Blink for ten seconds (so that it blinks until it's hidden)
		while(t<10f)
		{
			col.a = Mathf.PingPong(t,0.5f)+0.5f;
			gemsprite.color = col;
			t+=Realtime.deltaTime;
			yield return null;
		}
	}
	
	
	IEnumerator BlinkIcons()
	{
		float t=0f;
		float alpha = 1f;
		
		UIPanelAlpha panel = GetComponent<UIPanelAlpha>();
		
		panel.alpha = 1f;
	
		yield return new WaitForSeconds(5f);
		
		//Blink for ten seconds (so that it blinks until it's hidden)
		while(t<2f)
		{
			alpha = Mathf.PingPong(t*5f + 0.85f,0.85f)+0.15f;
			panel.alpha = alpha;
			t+=Realtime.deltaTime;
			yield return null;
		}
	}
	
	
	public void MakeButtonsStatic(bool doStatic){
		StartCoroutine(MakeButtonsStaticLoop(doStatic));
	}
	
	IEnumerator MakeButtonsStaticLoop(bool doStatic){
		yield return new WaitForSeconds( 0.1f);
		DoubleCoinsButton.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		yield return new WaitForSeconds( 0.1f);
		DiscountButton.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		yield return new WaitForSeconds( 0.1f);
		PowerDurationButton.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		yield return new WaitForSeconds( 0.1f);
		CoinMeterSpeedButton.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		yield return new WaitForSeconds( 0.1f);
		LuckButton.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		
		
		yield return new WaitForSeconds( 0.1f);
		HeadStartButton.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		yield return new WaitForSeconds( 0.1f);
		BigHeadStartButton.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		yield return new WaitForSeconds( 0.1f);
		BonusMultiplierButton.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		yield return new WaitForSeconds( 0.1f);
		StumbleProofButton.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		yield return new WaitForSeconds( 0.1f);
		ThirdEyeButton.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		
		
		yield return new WaitForSeconds( 0.1f);
		BoostImage.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		yield return new WaitForSeconds( 0.1f);
		PoofImage.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		yield return new WaitForSeconds( 0.1f);
		MagnetImage.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		yield return new WaitForSeconds( 0.1f);
		MegaCoinImage.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		yield return new WaitForSeconds( 0.1f);
		TornadoTokenImage.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		yield return new WaitForSeconds( 0.1f);
		GemImage.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		yield return new WaitForSeconds( 0.1f);
		ScoreBonusImage.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		
		
		doStatic = !doStatic;
		
		yield return new WaitForSeconds( 0.1f);
		DoubleCoinsButton.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		yield return new WaitForSeconds( 0.1f);
		DiscountButton.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		yield return new WaitForSeconds( 0.1f);
		PowerDurationButton.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		yield return new WaitForSeconds( 0.1f);
		CoinMeterSpeedButton.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		yield return new WaitForSeconds( 0.1f);
		LuckButton.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		
		
		yield return new WaitForSeconds( 0.1f);
		HeadStartButton.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		yield return new WaitForSeconds( 0.1f);
		BigHeadStartButton.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		yield return new WaitForSeconds( 0.1f);
		BonusMultiplierButton.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		yield return new WaitForSeconds( 0.1f);
		StumbleProofButton.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		yield return new WaitForSeconds( 0.1f);
		ThirdEyeButton.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		
		
		yield return new WaitForSeconds( 0.1f);
		BoostImage.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		yield return new WaitForSeconds( 0.1f);
		PoofImage.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		yield return new WaitForSeconds( 0.1f);
		MagnetImage.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		yield return new WaitForSeconds( 0.1f);
		MegaCoinImage.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		yield return new WaitForSeconds( 0.1f);
		TornadoTokenImage.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		yield return new WaitForSeconds( 0.1f);
		GemImage.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
		yield return new WaitForSeconds( 0.1f);
		ScoreBonusImage.GetComponent<UIPanel>().widgetsAreStatic = doStatic;
	}

	//TODO: Consolidate most of this functionality into a smaller function
	// Modifier Gemming events
	void TriggerCoinMeterSpeed()
	{
		if (!GameController.SharedInstance.IsPaused)
		{
			if (GameProfile.SharedInstance.Player.CanAffordArtifactGem(CoinMeterSpeedID))
			{
				if(GameProfile.SharedInstance.Player.artifactsGemmed.Count==0)
					ObjectivesDataUpdater.SetGenericStat(ObjectiveType.DistanceWithoutPowerups,GameController.SharedInstance.DistanceTraveled);
				AudioManager.SharedInstance.PlayFX(AudioManager.Effects.MusicBox);
				GameProfile.SharedInstance.Player.GemArtifact(CoinMeterSpeedID);
				DisplayButton(CoinMeterSpeedButton,BonusButtonType.Modifier,ArtifactStore.GetArtifactProtoData(CoinMeterSpeedID)._gemmedTitle);
				UpdateGemCount();
				//Bounce(CoinMeterSpeedButton.transform);
				AddAbilityUsedFlag(AbilityUsed.Mod1);
				UIManagerOz.SharedInstance.inGameVC.HideAbilityTutorial();
			}
			else
				UIManagerOz.SharedInstance.inGameVC.OnOutOfGemsPause(TriggerCoinMeterSpeed);
		}
	}
	
	void TriggerTripleCoins()
	{
		if (!GameController.SharedInstance.IsPaused)
		{
			if (GameProfile.SharedInstance.Player.CanAffordArtifactGem(DoubleCoinsID))
			{
				if(GameProfile.SharedInstance.Player.artifactsGemmed.Count==0)
					ObjectivesDataUpdater.SetGenericStat(ObjectiveType.DistanceWithoutPowerups,GameController.SharedInstance.DistanceTraveled);
				AudioManager.SharedInstance.PlayFX(AudioManager.Effects.MusicBox);
				GameProfile.SharedInstance.Player.GemArtifact(DoubleCoinsID);
				GameController.SharedInstance.SetUpUpgradeData();
				DisplayButton(DoubleCoinsButton,BonusButtonType.Modifier,ArtifactStore.GetArtifactProtoData(DoubleCoinsID)._gemmedTitle);
				UpdateGemCount();
				//Bounce(DoubleCoinsButton.transform);
				AddAbilityUsedFlag(AbilityUsed.Mod2);
				UIManagerOz.SharedInstance.inGameVC.HideAbilityTutorial();
			}
			else
				UIManagerOz.SharedInstance.inGameVC.OnOutOfGemsPause(TriggerTripleCoins);
		}		
	}
	
	void TriggerLuck()
	{
		if (!GameController.SharedInstance.IsPaused)
		{
			if (GameProfile.SharedInstance.Player.CanAffordArtifactGem(LuckID))
			{
				if(GameProfile.SharedInstance.Player.artifactsGemmed.Count==0)
					ObjectivesDataUpdater.SetGenericStat(ObjectiveType.DistanceWithoutPowerups,GameController.SharedInstance.DistanceTraveled);
				UIManagerOz.SharedInstance.inGameVC.HideAbilityTutorial();
				AudioManager.SharedInstance.PlayFX(AudioManager.Effects.MusicBox);
				GameProfile.SharedInstance.Player.GemArtifact(LuckID);
				DisplayButton(LuckButton,BonusButtonType.Modifier,ArtifactStore.GetArtifactProtoData(LuckID)._gemmedTitle);
				UpdateGemCount();
				//Bounce(LuckButton.transform);
				AddAbilityUsedFlag(AbilityUsed.Mod3);
			}
			else
				UIManagerOz.SharedInstance.inGameVC.OnOutOfGemsPause(TriggerLuck);
		}			
	}
	
	void TriggerDiscount()
	{
		if (!GameController.SharedInstance.IsPaused)
		{
			if (GameProfile.SharedInstance.Player.CanAffordArtifactGem(DiscountID))
			{
				if(GameProfile.SharedInstance.Player.artifactsGemmed.Count==0)
					ObjectivesDataUpdater.SetGenericStat(ObjectiveType.DistanceWithoutPowerups,GameController.SharedInstance.DistanceTraveled);
				UIManagerOz.SharedInstance.inGameVC.HideAbilityTutorial();
				AudioManager.SharedInstance.PlayFX(AudioManager.Effects.MusicBox);
				GameProfile.SharedInstance.Player.GemArtifact(DiscountID);
				DisplayButton(DiscountButton,BonusButtonType.Modifier,ArtifactStore.GetArtifactProtoData(DiscountID)._gemmedTitle);
				UpdateGemCount();
				//Bounce(DiscountButton.transform);
				AddAbilityUsedFlag(AbilityUsed.Mod4);
			}
			else
				UIManagerOz.SharedInstance.inGameVC.OnOutOfGemsPause(TriggerDiscount);
		}			
	}
	
	void TriggerPowerDuration()
	{
		if (!GameController.SharedInstance.IsPaused)
		{
			if (GameProfile.SharedInstance.Player.CanAffordArtifactGem(PowerDurationID))
			{
				if(GameProfile.SharedInstance.Player.artifactsGemmed.Count==0)
					ObjectivesDataUpdater.SetGenericStat(ObjectiveType.DistanceWithoutPowerups,GameController.SharedInstance.DistanceTraveled);
				UIManagerOz.SharedInstance.inGameVC.HideAbilityTutorial();
				GameProfile.SharedInstance.Player.GemArtifact(PowerDurationID);
				DisplayButton(PowerDurationButton,BonusButtonType.Modifier,ArtifactStore.GetArtifactProtoData(PowerDurationID)._gemmedTitle);
				UpdateGemCount();
				//Bounce(PowerDurationButton.transform);
				AddAbilityUsedFlag(AbilityUsed.Mod5);
			}
			else
				UIManagerOz.SharedInstance.inGameVC.OnOutOfGemsPause(TriggerPowerDuration);
		}			
	}
	
	public static void HideHeadStarts()
	{
		main.ShowButton(main.BigHeadStartButton,false,true);
		main.ShowButton(main.HeadStartButton,false,true);
	}
	
	void HideDiscountButton()
	{
		ShowButton(DiscountButton,false,false);
	}
	
	//Consumable events
	void UseHeadStart()
	{
		if(!GameController.SharedInstance.IsPaused && GameProfile.SharedInstance.Player.PopConsumable(HeadStartID))
		{
			UIManagerOz.SharedInstance.inGameVC.HideAbilityTutorial();
			AudioManager.SharedInstance.PlayFX(AudioManager.Effects.MusicBox);
			ConsumableStore.ConsumableFromID(HeadStartID).Activate();
			ShowButton(BigHeadStartButton,false,true);
			DisplayButton(HeadStartButton,BonusButtonType.Consumable,GameProfile.SharedInstance.Player.GetConsumableLocalizeString(HeadStartID));
			//Bounce(HeadStartButton.transform);
			AddAbilityUsedFlag(AbilityUsed.Cons1);	//For both head starts
			AddAbilityUsedFlag(AbilityUsed.Cons2);
			
			/* jonoble: Commented out to see if it improves performance
			BaseConsumable consumable = ConsumableStore.ConsumableFromID(HeadStartID);
			
			AnalyticsInterface.LogGameAction(
				"run",
				"consumable_used", 
				consumable.Title,
				EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.SetCode,
				0
			);
			*/
			//			AnalyticsInterface.UseConsumable( AnalyticsInterface.Consumable.HeadStart, 
			//EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.SetCode );
			
			if(GameProfile.SharedInstance.Player.IsArtifactPurchased(DiscountID)) {
				ShowButton(DiscountButton,true,false);
			//	Invoke("HideDiscountButton",5f);
			}
			
			FastTravelButton.HideAll();
		}
	}
	
	void UseBigHeadStart()
	{
		if(!GameController.SharedInstance.IsPaused && GameProfile.SharedInstance.Player.PopConsumable(BigHeadStartID))
		{
			UIManagerOz.SharedInstance.inGameVC.HideAbilityTutorial();
			ConsumableStore.ConsumableFromID(BigHeadStartID).Activate();
			ShowButton(HeadStartButton,false,true);
			DisplayButton(BigHeadStartButton,BonusButtonType.Consumable,GameProfile.SharedInstance.Player.GetConsumableLocalizeString(BigHeadStartID));
			//Bounce(BigHeadStartButton.transform);
			AddAbilityUsedFlag(AbilityUsed.Cons2);	//For both head starts
			AddAbilityUsedFlag(AbilityUsed.Cons1);
			
			/* jonoble: Commented out to see if it improves performance
			BaseConsumable consumable = ConsumableStore.ConsumableFromID(BigHeadStartID);
			
			AnalyticsInterface.LogGameAction(
				"run",
				"consumable_used",
				consumable.Title,
				EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.SetCode,
				0
			);		
			*/
			//			AnalyticsInterface.UseConsumable( AnalyticsInterface.Consumable.BigHeadStart, EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.SetCode );
			
			
			if(GameProfile.SharedInstance.Player.IsArtifactPurchased(DiscountID)) {
				ShowButton(DiscountButton,true,false);
		//		Invoke("HideDiscountButton",5f);
			}
			
			FastTravelButton.HideAll();
		}
	}
	
	void UseBonusMultiplier()
	{
		if(!GameController.SharedInstance.IsPaused && GameProfile.SharedInstance.Player.PopConsumable(BonusMultiplierID))
		{
			UIManagerOz.SharedInstance.inGameVC.HideAbilityTutorial();
			AudioManager.SharedInstance.PlayFX(AudioManager.Effects.MusicBox);
			ConsumableStore.ConsumableFromID(BonusMultiplierID).Activate();
			DisplayButton(BonusMultiplierButton,BonusButtonType.Consumable,GameProfile.SharedInstance.Player.GetConsumableLocalizeString(BonusMultiplierID));
			//Bounce(BonusMultiplierButton.transform);
			AddAbilityUsedFlag(AbilityUsed.Cons3);
			
			/* jonoble: Commented out to see if it improves performance
			BaseConsumable consumable = ConsumableStore.ConsumableFromID(BonusMultiplierID);
			
			AnalyticsInterface.LogGameAction(
				"run",
				"consumable_used",
				consumable.Title,
				EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.SetCode,
				0
			);
			*/
			//			AnalyticsInterface.UseConsumable( AnalyticsInterface.Consumable.Multiplier, EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.SetCode );
		}
	}
	
	void UseStumbleProof()
	{
		if(!GameController.SharedInstance.IsPaused && GameProfile.SharedInstance.Player.PopConsumable(StumbleProofID))
		{
			UIManagerOz.SharedInstance.inGameVC.HideAbilityTutorial();
			AudioManager.SharedInstance.PlayFX(AudioManager.Effects.MusicBox);
			ConsumableStore.ConsumableFromID(StumbleProofID).Activate();
			DisplayButton(StumbleProofButton,BonusButtonType.Consumable,GameProfile.SharedInstance.Player.GetConsumableLocalizeString(StumbleProofID));
			//Bounce(StumbleProofButton.transform);
			AddAbilityUsedFlag(AbilityUsed.Cons4);
			
			/* jonoble: Commented out to see if it improves performance
			BaseConsumable consumable = ConsumableStore.ConsumableFromID(StumbleProofID);
			
			AnalyticsInterface.LogGameAction(
				"run",
				"consumable_used",
				consumable.Title,
				EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.SetCode,
				0
			);
			*/
			//			AnalyticsInterface.UseConsumable( AnalyticsInterface.Consumable.StumbleProof, EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.SetCode );
		}
	}
	
	void UseThirdEye()
	{
		if(!GameController.SharedInstance.IsPaused && GameProfile.SharedInstance.Player.PopConsumable(ThirdEyeID))
		{
			UIManagerOz.SharedInstance.inGameVC.HideAbilityTutorial();
			AudioManager.SharedInstance.PlayFX(AudioManager.Effects.MusicBox);
			ConsumableStore.ConsumableFromID(ThirdEyeID).Activate();
			DisplayButton(ThirdEyeButton,BonusButtonType.Consumable,GameProfile.SharedInstance.Player.GetConsumableLocalizeString(ThirdEyeID));
			//Bounce(ThirdEyeButton.transform);
			AddAbilityUsedFlag(AbilityUsed.Cons5);
			
			/* jonoble: Commented out to see if it improves performance
			BaseConsumable consumable = ConsumableStore.ConsumableFromID(ThirdEyeID);
			
			AnalyticsInterface.LogGameAction(
				"run",
				"consumable_used",
				consumable.Title,
				EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.SetCode,
				0
			);
			*/
			//			AnalyticsInterface.UseConsumable( AnalyticsInterface.Consumable.ThirdEye, EnvironmentSetManager.SharedInstance.CurrentEnvironmentSet.SetCode );
		}
	}
	
	public void ResetAbilityUsedFlags()
	{
		UsedAbilityFlags = 0;
	}
	
	public void AddAbilityUsedFlag(AbilityUsed abilityFlag)
	{
		//If we haven't used this ability this run, add to our stat
		if(((int)abilityFlag & UsedAbilityFlags) == 0 && abilityFlag < AbilityUsed.AllInGame)
			ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.UseAllInGamePowerups,1);
		//If we've never used this ability before, add this to the "All" stat
		if(((int)abilityFlag & GameProfile.SharedInstance.Player.abilitiesUsed) == 0)
			ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.UseEverything,1);
		
	//	Debug.Log(ObjectivesDataUpdater.GetStatForObjectiveType(ObjectiveType.UseAllInGamePowerups,-1));
	//	Debug.Log(ObjectivesDataUpdater.GetStatForLifetimeObjectiveType(ObjectiveType.UseEverything,-1));
		
		
		UsedAbilityFlags |= (int)abilityFlag;
		
		GameProfile.SharedInstance.Player.abilitiesUsed |= (int)abilityFlag;
		
	//	Debug.Log(abilityFlag + " " + UsedAbilityFlags + " " + (UsedAbilityFlags & (int)AbilityUsed.AllInGame) + " " + (int)AbilityUsed.AllInGame);
		
	//	Debug.Log(abilityFlag + " " + (int)GameProfile.SharedInstance.Player.abilitiesUsed + " " + (GameProfile.SharedInstance.Player.abilitiesUsed & (int)AbilityUsed.All) + " " + (int)AbilityUsed.All);
		
		
/*		int everythingBits = 0;
		
		for(int i=0;i<(int)AbilityUsed.AllInGame;i++)
		{
			if(((int)Mathf.Pow(2,i) & (int)UsedAbilityFlags) > 0)
			{
				everythingBits++;
			}
		}
		ObjectivesDataUpdater.SetGenericStat(ObjectiveType.UseAllInGamePowerups,everythingBits);
		
		everythingBits = 0;
		for(int i=0;i<(int)AbilityUsed.AllInGame;i++)
		{
			if(((int)Mathf.Pow(2,i) & (int)GameProfile.SharedInstance.Player.abilitiesUsed) > 0)
			{
				everythingBits++;
			}
		}
		ObjectivesDataUpdater.SetGenericStat(ObjectiveType.UseEverything,everythingBits);*/
		
		//if((UsedAbilityFlags & (int)AbilityUsed.AllInGame) == (int)AbilityUsed.AllInGame)
		//	ObjectivesDataUpdater.SetGenericStat(ObjectiveType.UseEverything,1);
		//if((GameProfile.SharedInstance.Player.abilitiesUsed & (int)AbilityUsed.All) == (int)AbilityUsed.All)
		//	ObjectivesDataUpdater.SetGenericStat(ObjectiveType.UseEverything,2);
	}
	
	
	//Pickup events
	public void OnBoostPickedUp(Vector3 worldPos)
	{
		DisplayButton(BoostImage,BonusButtonType.Pickup,"Upg_Powerup_3_Title");
		Vector3 screenPos = GameCamera.SharedInstance.camera.WorldToScreenPoint(worldPos);
		BoostImage.transform.position = UICamera.mainCamera.camera.ScreenToWorldPoint(new Vector3(screenPos.x,screenPos.y,0f));
	}
	public void OnPoofPickedUp(Vector3 worldPos)
	{
		DisplayButton(PoofImage,BonusButtonType.Pickup,"Upg_Powerup_2_Title");
		Vector3 screenPos = GameCamera.SharedInstance.camera.WorldToScreenPoint(worldPos);
		PoofImage.transform.position = UICamera.mainCamera.camera.ScreenToWorldPoint(new Vector3(screenPos.x,screenPos.y,0f));
	}
	public void OnMagnetPickedUp(Vector3 worldPos)
	{
		DisplayButton(MagnetImage,BonusButtonType.Pickup,"Upg_Powerup_4_Title");
		Vector3 screenPos = GameCamera.SharedInstance.camera.WorldToScreenPoint(worldPos);
		MagnetImage.transform.position = UICamera.mainCamera.camera.ScreenToWorldPoint(new Vector3(screenPos.x,screenPos.y,0f));
	}
	public void OnMegaCoinPickedUp(Vector3 worldPos)
	{
		DisplayButton(MegaCoinImage,BonusButtonType.Pickup,"Pkp_MegaCoin");
		Vector3 screenPos = GameCamera.SharedInstance.camera.WorldToScreenPoint(worldPos);
		MegaCoinImage.transform.position = UICamera.mainCamera.camera.ScreenToWorldPoint(new Vector3(screenPos.x,screenPos.y,0f));
	}
	public void OnTornadoTokenPickedUp(Vector3 worldPos)
	{
		DisplayButton(TornadoTokenImage,BonusButtonType.Pickup,"Pkp_DestinyCard");
		Vector3 screenPos = GameCamera.SharedInstance.camera.WorldToScreenPoint(worldPos);
		TornadoTokenImage.transform.position = UICamera.mainCamera.camera.ScreenToWorldPoint(new Vector3(screenPos.x,screenPos.y,0f));
	}
	public void OnGemPickedUp(Vector3 worldPos)
	{
		DisplayButton(GemImage,BonusButtonType.Pickup,"Pkp_Gem");
		Vector3 screenPos = GameCamera.SharedInstance.camera.WorldToScreenPoint(worldPos);
		GemImage.transform.position = UICamera.mainCamera.camera.ScreenToWorldPoint(new Vector3(screenPos.x,screenPos.y,0f));
	}
	public void OnScoreBonusPickedUp(Vector3 worldPos)
	{
		DisplayButton(ScoreBonusImage,BonusButtonType.Pickup,"Pkp_ScoreBonus");
		Vector3 screenPos = GameCamera.SharedInstance.camera.WorldToScreenPoint(worldPos);
		ScoreBonusImage.transform.position = UICamera.mainCamera.camera.ScreenToWorldPoint(new Vector3(screenPos.x,screenPos.y,0f));
	}
	
}
