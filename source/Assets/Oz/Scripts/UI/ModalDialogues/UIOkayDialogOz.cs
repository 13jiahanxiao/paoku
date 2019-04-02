using UnityEngine;
using System.Collections;

public class UIOkayDialogOz : UIModalDialogOz	//MonoBehaviour
{
	public delegate void voidClickedHandler();
//	public static event voidClickedHandler onNegativeResponse;
	public static event voidClickedHandler onPositiveResponse;
	
	public Transform CenterButton;
	public Transform CenterButtonText;
	public Transform CenterButtonTextSysFont;
	public Transform DescriptionText;
	public Transform DescriptionSysFont;
	public Transform BackgroundSprite;

	public UISprite itemIcon;
	
	private ScaleModalDialog scaler;
	
	private GameObject messageObject;
	
	protected override void Awake()
	{
		base.Awake();
		scaler = gameObject.GetComponent<ScaleModalDialog>();
	}
	
	public void ShowOkayDialog(string description, string positiveButtonText, bool useSysFont = false, GameObject msgObject = null) 
	{		
		SetupNotify();	//Sometimes, this is called before 'notify' is set up
		notify.Debug("ShowOkayDialog {0} {1} {2} {3}" , description, positiveButtonText, useSysFont, msgObject);	
		messageObject = msgObject;

		if (Localization.HasMainFontBeenUpdated() == false)
		{
			// force SysFont in this case, we are so early we haven't loaded the main font texture
			notify.Debug("ShowOkayDialog forcing sysfont");
			useSysFont = true;
		}
		
		if (useSysFont)
		{
			description = Localization.SharedInstance.Get(description);
			description = description.Replace("\\n", " "); 			
			DescriptionSysFont.gameObject.GetComponent<UISysFontLabel>().enabled = true;
			DescriptionSysFont.gameObject.GetComponent<UISysFontLabel>().Text = description;	// localize the description
			DescriptionText.gameObject.GetComponent<UILabel>().enabled = false;
			
			positiveButtonText = Localization.SharedInstance.Get(positiveButtonText);			// localize the center button text
			CenterButtonTextSysFont.gameObject.GetComponent<UISysFontLabel>().Text = positiveButtonText;	
			CenterButtonTextSysFont.gameObject.GetComponent<UISysFontLabel>().enabled = true;
			CenterButtonText.gameObject.GetComponent<UILabel>().enabled = false;		
		}
		else	
		{
			DescriptionText.gameObject.GetComponent<UILabel>().enabled = true;
			DescriptionText.gameObject.GetComponent<UILocalize>().SetKey(description);			// localize the description
			DescriptionSysFont.gameObject.GetComponent<UISysFontLabel>().enabled = false;
			
			CenterButtonTextSysFont.gameObject.GetComponent<UISysFontLabel>().enabled = false;
			CenterButtonText.gameObject.GetComponent<UILocalize>().SetKey(positiveButtonText);	// localize the center button text
			CenterButtonText.gameObject.GetComponent<UILabel>().enabled = true;
		}
		
		if (scaler == null)
			scaler = gameObject.GetComponent<ScaleModalDialog>();		
		
		if (useSysFont)
			scaler.SetScale(3f);	// force dialog box scale to large size, since auto text height detection doesn't work for sysfont
		else
			scaler.SetScale();		// scale automatically based on text size
		
		NGUITools.SetActive(this.gameObject, true);
		
		if (itemIcon != null) 
			NGUITools.SetActive(itemIcon.gameObject, false);
		
		// for debugging why scaling isn't working in some cases
		//CenterButtonTextSysFont.gameObject.GetComponent<UISysFontLabel>().Text = 
		//	CenterButtonTextSysFont.gameObject.GetComponent<UISysFontLabel>().Text + ":" + scaler.GetTextSize().ToString() + ":" + scaler.GetTextTransform().ToString();
	}
	
	public void OnCenterButtonPress()
	{
		if (onPositiveResponse != null)
			onPositiveResponse();

		if (messageObject)
			messageObject.SendMessage("OnOkayDialogClosed");
		
		NGUITools.SetActive(this.gameObject, false);		
	}
	
	public void OnEscapeButtonClickedModel()
	{
		if( UIManagerOz.escapeHandled ) return;
		UIManagerOz.escapeHandled = true;
		
		OnCenterButtonPress();// left always??!!
	}	
}




	//public Transform Quantity;

		//Quantity.gameObject.GetComponent<UILabel>().enabled = false;	//.text = "";		// hide the quantity


	//public Transform TitleText = null;

	//public void ShowOkayDialog(string title, string description, string positiveButtonText, GameObject msgObject = null) 

		//TitleText.gameObject.GetComponent<UILocalize>().SetKey(title);						// localize the title

	
	//public UILabel rewardLabel = null;
	//public UILabel levelLabel = null;



//	public void ShowRewardDialog(string title, string description, int quantity, string itemIconName) //(string rewardText, string itemIconName) 	// string levelText, 
//	{
//		NGUITools.SetActive(this.gameObject, true);
//	
//		CenterButtonText.gameObject.GetComponent<UILocalize>().SetKey("Btn_Ok");			// center button text
//		DescriptionText.gameObject.GetComponent<UILocalize>().SetKey(description);			// localize the description
//		TitleText.gameObject.GetComponent<UILocalize>().SetKey(title);						// localize the title		
//		Quantity.gameObject.GetComponent<UILabel>().text = quantity.ToString();				// show the quantity
//	
//		if (itemIcon != null && itemIconName!= null) 
//		{
//			itemIcon.spriteName = itemIconName;
//			itemIcon.MakePixelPerfect();
//		}	
//		
////		if (rewardLabel != null && rewardText!= null)
////			rewardLabel.text = rewardText;
////		
////		if (levelLabel != null)	// && levelText!= null)
////			levelLabel.text = "";	//levelText;
//
//	}

		
//		if (CenterButtonText) 
//		{
//			UILabel label = CenterButtonText.GetComponent<UILabel>() as UILabel;
//			if (label)
//				label.text = positiveButtonText;
//		}
//		
//		if (DescriptionText) 
//		{
//			UILabel label = DescriptionText.GetComponent<UILabel>() as UILabel;
//			if (label) 
//				label.text = description;
//		}
//		
//		if (TitleText) 
//		{
//			UILabel label = TitleText.GetComponent<UILabel>() as UILabel;
//			if (label) 
//				label.text = title;
//		}


	
//		if (CenterButton != null)
//			CenterButton.transform.position = new Vector3(0, CenterButton.transform.position.y, CenterButton.transform.position.z);


//		NGUITools.SetActive(DescriptionText.gameObject, false);
//		NGUITools.SetActive(TitleText.gameObject, false);
//		NGUITools.SetActive(BackgroundSprite.gameObject, false);
	
	//private Vector3 rightButtonCachedPosition = new Vector3(160, -76, 0);
	
	//void Awake() 
	//{
	//	if (RightButton != null) { rightButtonCachedPosition = RightButton.transform.localPosition; }
	//}
	
	//void Start() { }



//using UnityEngine;
//using System.Collections;
//
//public class UIConfirmDialogOz : MonoBehaviour
//{
//	public delegate void voidClickedHandler();
//	public static event voidClickedHandler	onNegativeResponse = null;
//	public static event voidClickedHandler	onPositiveResponse = null;
//	
//	
//	public Transform LeftButton = null;
//	public Transform RightButton = null;
//	public Transform LeftButtonText = null;
//	public Transform RightButtonText = null;
//	public Transform DescriptionText = null;
//	public Transform TitleText = null;
//	public Transform BackgroundSprite = null;

//	
//	void Start() {
//		
//				
//			}
//		}
//	}
//	
//	public void ShowOkayDialog(string title, string description, string negativeButtonText, string positiveButtonText) {
//		if(LeftButtonText) {
//			UILabel label = LeftButtonText.GetComponent<UILabel>() as UILabel;
//			if(label) {
//				label.text = negativeButtonText;
//			}
//		}
//		if(RightButtonText) {
//			UILabel label = RightButtonText.GetComponent<UILabel>() as UILabel;
//			if(label) {
//				label.text = positiveButtonText;
//			}
//		}
//		if(DescriptionText) {
//			UILabel label = DescriptionText.GetComponent<UILabel>() as UILabel;
//			if(label) {
//				label.text = description;
//			}
//		}
//		if(TitleText) {
//			UILabel label = TitleText.GetComponent<UILabel>() as UILabel;
//			if(label) {
//				label.text = title;
//			}
//		}
//		NGUITools.SetActive(this.gameObject, true);
//	}
//	public void OnLeftButtonPress() {
//		if(onNegativeResponse != null)
//		{
//			onNegativeResponse();
//		}
//		NGUITools.SetActive(this.gameObject, false);
//	}
//	public void OnRightButtonPress() {
//		if(onPositiveResponse != null)
//		{
//			onPositiveResponse();
//		}
//		NGUITools.SetActive(this.gameObject, false);
//	}
//}
//
