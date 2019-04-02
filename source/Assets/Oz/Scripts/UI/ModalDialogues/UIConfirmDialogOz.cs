using UnityEngine;
using System;
using System.Collections;

public class UIConfirmDialogOz : UIModalDialogOz	//MonoBehaviour
{
	public delegate void voidClickedHandler();
	public static event voidClickedHandler onNegativeResponse;
	public static event voidClickedHandler onPositiveResponse;
	
	public Transform LeftButton;
	public Transform RightButton;
	public Transform LeftButtonText;
	public Transform RightButtonText;
	public Transform DescriptionText;
	public Transform DescriptionSysFont;
	public Transform BackgroundSprite;

	public UILabel levelLabel;
	public UISprite itemIcon;
	
	private ScaleModalDialog scaler;
	
	protected override void Awake()
	{
		base.Awake();
		scaler = gameObject.GetComponent<ScaleModalDialog>();
	}
	
	public void ShowConfirmDialog(string description, string negativeButtonText, string positiveButtonText)	//, string price = null)  
	{
		LeftButtonText.gameObject.GetComponent<UILocalize>().SetKey(negativeButtonText);	// localize the left button text
		RightButtonText.gameObject.GetComponent<UILocalize>().SetKey(positiveButtonText);	// localize the right button text		
		DescriptionText.gameObject.GetComponent<UILocalize>().SetKey(description);			// localize the description	
		DescriptionSysFont.gameObject.GetComponent<UISysFontLabel>().enabled = false;
		
		if (scaler == null)
			scaler = gameObject.GetComponent<ScaleModalDialog>();
		
		scaler.SetScale();	// scale the dialog box to the right size, to accomodate the full description text size
		
		NGUITools.SetActive(this.gameObject, true);
	}
	
	public void OnLeftButtonPress()
	{
		if (onNegativeResponse != null)
			onNegativeResponse();
		NGUITools.SetActive(this.gameObject, false);
	}
	
	public void OnRightButtonPress() 
	{
		if (onPositiveResponse != null)
			onPositiveResponse();
		NGUITools.SetActive(this.gameObject, false);
	}
	
	public void OnEscapeButtonClickedModel()
	{
		if( UIManagerOz.escapeHandled ) return;
		UIManagerOz.escapeHandled = true;

		
		OnLeftButtonPress();// left always??!!
	}	
}




	//public UILabel rewardLabel = null;

	//public void ShowConfirmDialog(string title, string description, string negativeButtonText, string positiveButtonText)	//, string price = null) 

	//public Transform TitleText = null;
	//public Transform CostText = null;	

		//TitleText.gameObject.GetComponent<UILocalize>().SetKey(title);						// localize the title	
		//if (price != null)
		//	CostText.gameObject.GetComponent<UILabel>().text = price;						// show the localized price	

	
	//public void ShowConfirmPurchaseDialog(string title, string description, string negativeButtonText, string positiveButtonText, Decimal cost) 
//	public void ShowConfirmPurchaseDialog(string title, string description, string negativeButtonText, string positiveButtonText, string price) 
//	{
//		LeftButtonText.gameObject.GetComponent<UILocalize>().SetKey(negativeButtonText);	// localize the left button text
//		RightButtonText.gameObject.GetComponent<UILocalize>().SetKey(positiveButtonText);	// localize the right button text	
//		DescriptionText.gameObject.GetComponent<UILocalize>().SetKey(description);			// localize the purchase prompt
//		TitleText.gameObject.GetComponent<UILocalize>().SetKey(title);						// localize the title
//		//CostText.gameObject.GetComponent<UILocalize>().SetMoney(cost);						// localize the price
//		CostText.gameObject.GetComponent<UILabel>().text = price;							// show the localized price
//		NGUITools.SetActive(this.gameObject, true);
//	}	


//	public static void ClearEventHandlers() 
//	{
//		onNegativeResponse = null;
//		onPositiveResponse = null;
//	}




	
//	public void ShowInfoDialog(string title, string description, string positiveButtonText) 
//	{
//		NGUITools.SetActive(this.gameObject, true);
//		NGUITools.SetActive(LeftButton.gameObject, false);
//		
//		if (RightButton != null)
//			RightButton.transform.position = new Vector3(0, RightButton.transform.position.y, RightButton.transform.position.z);
//		
//		if (RightButtonText) 
//		{
//			UILabel label = RightButtonText.GetComponent<UILabel>() as UILabel;
//			if (label)
//				label.text = positiveButtonText;
//		}
//		if(DescriptionText) 
//		{
//			UILabel label = DescriptionText.GetComponent<UILabel>() as UILabel;
//			if (label)
//				label.text = description;
//		}
//		if (TitleText) 
//		{
//			UILabel label = TitleText.GetComponent<UILabel>() as UILabel;
//			if (label)
//				label.text = title;
//		}
//	}

	
//	public void ShowRewardDialog(string rewardText, string levelText, string itemIconName)
//	{
//		NGUITools.SetActive(this.gameObject, true);
//		
//		if (RightButton != null)
//			RightButton.transform.position = new Vector3(0, RightButton.transform.position.y, RightButton.transform.position.z);
//		
//		if (rewardLabel != null && rewardText!= null)
//			rewardLabel.text = rewardText;
//		
//		if (levelLabel != null && levelText!= null) 
//			levelLabel.text = levelText;
//		
//		if (itemIcon != null && itemIconName!= null) 
//		{
//			itemIcon.spriteName = itemIconName;
//			itemIcon.MakePixelPerfect();
//		}
//	}



//		if (LeftButtonText) 
//		{
//			UILabel label = LeftButtonText.GetComponent<UILabel>() as UILabel;
//			if (label)
//				label.text = negativeButtonText;
//		}
//				
//		if (RightButtonText)
//		{
//			UILabel label = RightButtonText.GetComponent<UILabel>() as UILabel;
//			if(label)
//				label.text = positiveButtonText;
//		}
		
//		if (DescriptionText)
//		{
//			UILabel label = DescriptionText.GetComponent<UILabel>() as UILabel;
//			if (label)
//				label.text = description;
//		}		
		
//		if (TitleText) 
//		{ 
//			UILabel label = TitleText.GetComponent<UILabel>() as UILabel;
//			if (label) { label.text = title; }
//		}

	
	//private Vector3 rightButtonCachedPosition = new Vector3(160, -76, 0);
	
//	void Awake() 
//	{
//	//	if(RightButton != null) {
//	//		rightButtonCachedPosition = RightButton.transform.localPosition;
//	//	}
//	}
//	void Start() {
//		
//	}


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
;
//	
//	void Start() {
//		
//	}
//	
//	public void ShowConfirmDialog(string title, string description, string negativeButtonText, string positiveButtonText) {
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
