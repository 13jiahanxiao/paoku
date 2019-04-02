using UnityEngine;
using System;
using System.Collections;
using System.Globalization;

public class StoreCellData : MonoBehaviour 
{
	public GameObject viewController;								// make link to UIInventoryViewControllerOz in prefab, in Unity	
	public GameObject scrollList;
	//public StoreItem _data { get; private set; }	
	public IAP_DATA _data { get; private set; }	
	
	public UILabel titleLabel;
	public UILabel descLabel;
//	public UISysFontLabel costLabel;
	public UILabel costLabel;
	//public UILabel quantityLabel;
	public UISprite iconSprite;	
	
	private NotificationSystem notificationSystem;
	private NotificationIcons notificationIcons;	
	
	private string fuhao = null;
	void Awake()
	{
		notificationIcons = gameObject.GetComponent<NotificationIcons>();
		//add by lichuang
		fuhao =  descLabel.text;
		descLabel.text = "";
		//end
	}
	
	void Start()
	{
		Destroy(gameObject.GetComponent<UIPanel>());				// kill auto-attached UIPanel component
		//if (_data != null && viewController != null) { Refresh(); }	// populate fields
	
		notificationSystem = Services.Get<NotificationSystem>();		
		
		if (viewController != null)
			Refresh();	// populate fields
	}

	public void SetData(IAP_DATA data)	//StoreItem data)
	{
		//if (_data != null) { oldEarnedStatValue = _data._conditionList[0]._earnedStatValue; }		// back up old _earnedStatValue, if exists
		_data = data;
		Refresh();										// populate fields	
	}
	
	public void Refresh()								// populate fields
	{
		//if (_data != null && viewController != null)
		if (viewController != null)
		{		
			
			gameObject.transform.Find("CellContents").GetComponent<UIButtonMessage>().target = viewController;		
			gameObject.transform.Find("CellContents/GraphicsAnchor/Image Button").GetComponent<UIButtonMessage>().target = scrollList;	
			//by add lichuang
			// these text fields disabled for now
	//		gameObject.transform.Find("CellContents/FontAnchor/LabelDiscount").GetComponent<UISysFontLabel>().enabled = false;	
	//		gameObject.transform.Find("CellContents/FontAnchor/LabelTimer").GetComponent<UILabel>().enabled = false;	
			//end
			// populate fields from data		
			//gameObject.transform.Find("CellContents/Title").GetComponent<UILabel>().text = _data.Title;					
			//gameObject.transform.Find("CellContents/Description").GetComponent<UILabel>().text = _data.Description;
			//gameObject.transform.Find("CellContents/Icon").GetComponent<UISprite>().spriteName = _data.IconName;
			//gameObject.transform.Find("CellContents/Quantity").GetComponent<UILabel>().text = GameProfile.SharedInstance.Player.consumablesPurchasedQuantity[_data.PID].ToString();			
			
			SetNotificationIcon();	// show notification icon if the item this cell contains is related to an active notification 
			titleLabel.gameObject.GetComponent<UILocalize>().SetKey(_data.title);	
			//titleLabel.text = _data.title;	
			//descLabel.text = _data.description;	
			//titleLabel.gameObject.GetComponent<UILocalize>().SetKey(_data.title);	
			//descLabel.gameObject.GetComponent<UILocalize>().SetKey(_data.description);
			
			//titleLabel.gameObject.GetComponent<UILabel>().text = _data.title;	
		//	titleLabel.text = _data.title;	
	//		descLabel.text = _data.description;			
			
//			Decimal cost = _data.cost / 100.00M;
			//costLabel.text = string.Format(Localization.instance.GetCultureInfo(), "{0:C}", cost);		//costLabel.text = String.Format("{0:C}", cost);			
//			costLabel.gameObject.GetComponent<UILocalize>().SetMoney(cost);
				//by add lichuang
		//	costLabel.Text = _data.price;
			costLabel.text = fuhao +_data.price;
			//end
		 	//quantityLabel.text = string.Format("{0:n0}", _data.costValueOrID);	//.itemQuantity);	
			iconSprite.spriteName = _data.iconname;	//.icon;
		}
	}
	
	public void SetNotificationIcon()
	{
		if (notificationSystem == null)
			notificationSystem = Services.Get<NotificationSystem>();		
		
		bool enable = false;	//notificationSystem.GetNotificationStatusForThisCell(NotificationType.MoreCoins, _data.productID);
		notificationIcons.SetNotification(0, (enable) ? 0 : -1);
	}	
	
	//void Update() { }	
}



			
			//string cultureString = Localization.instance.GetCultureInfo();	//"en-GB";	// make this string pull from some global setting (haha, global...)
			//CultureInfo culture = new CultureInfo(cultureString);

		// hack to work around NGUI scroller collider situation, which was preventing 'back' and 'play' buttons from working on objectives screen
		//if (gameObject.collider != null)
		//{
		//	gameObject.collider.enabled = (gameObject.transform.position.y < -3.3f) ? false : true;
		//}
			
			//progressBarFill.transform.localScale = new Vector3(400.0f * ((float)_data._earnedStatValue / (float)_data._statValue),
			//	progressBarFill.transform.localScale.y, progressBarFill.transform.localScale.z);


		//progressBarFill.GetComponent<UISprite>().color = new Color(1.0f, 1.0f, 0.0f, 1.0f);

			//progressBarFill.GetComponent<UISprite>().color = new Color(23.0f/255.0f, 115.0f/255.0f, 98.0f/255.0f, 255.0f/255.0f);
			
	
//			if (gameObject.transform.position.y < -3.3f) 
//			{ 
//				gameObject.collider.enabled = false;
//			}
//			else { gameObject.collider.enabled = true; }