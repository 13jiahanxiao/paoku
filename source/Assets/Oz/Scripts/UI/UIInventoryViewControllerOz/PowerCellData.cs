using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerCellData : MonoBehaviour 
{
	public GameObject viewController;
	public GameObject scrollList;									// reference to the scroll list that this cell is parented under the grid/table of		
	//public GameObject darkener;
	public GameObject checkmark;
	
	public UISprite background;
	
	public List<GameObject> buttons = new List<GameObject>();
	
	public UISprite SaleBurstSprite;
	public UISprite SaleSleeveSprite;
	public UISprite SaleOldCoinSprite;
	public UISprite SaleSlashOutSprite;
	public UILabel SaleSlashOutSprite_alt;
	public UILabel SaleOldCostLabel;
	
	public UILabel DefaultCostLabel;
	public UILabel SaleNewCostLabel;
	public UILabel DefaultBuyLabel;
	
	public BasePower _data;											// reference to powerup data	
	
	private NotificationSystem notificationSystem;
	private NotificationIcons notificationIcons;		
	
	protected static Notify notify;
	
	void Awake()
	{
		if (notify != null)
			notify = new Notify("PowerCellData");
		
		notificationIcons = gameObject.GetComponent<NotificationIcons>();		
	}
	
	void Start()
	{
		Destroy(gameObject.GetComponent<UIPanel>());				// kill auto-attached UIPanel component
		
		notificationSystem = Services.Get<NotificationSystem>();
		
		_toggleSaleDisplay( false );
		
		if (_data != null && viewController != null)
		{
			Refresh();												// populate fields
		}
	}

	public void SetData(BasePower data)
	{
		_data = data;
		Refresh();													// populate fields	
	}
	
	private void _toggleSaleDisplay( bool active )
	{
		// Default behaivor, turn off sale related elements.
		/*
		gameObject.transform.Find( "CellContents/GraphicsAnchor/sale_Burst" ).GetComponent<UISprite>().enabled = active;
		gameObject.transform.Find( "CellContents/GraphicsAnchor/sale_IconSleeve").GetComponent<UISprite>().enabled = active;
		gameObject.transform.Find( "CellContents/GraphicsAnchor/sale_OLD_CoinDisplayIcon").GetComponent<UISprite>().enabled = active;
		gameObject.transform.Find( "CellContents/GraphicsAnchor/sale_OLD_LabelCost").GetComponent<UILabel>().enabled = active;
		gameObject.transform.Find( "CellContents/GraphicsAnchor/sale_SlashOut").GetComponent<UISprite>().enabled = active;		
		*/

		// disable sale related elements if artifact is already maxed out
		if (GameProfile.SharedInstance.Player.IsPowerPurchased(_data.PowerID))
		{
			active = false;	
		}
		
		SaleBurstSprite.enabled = active;
		SaleOldCoinSprite.enabled = active;
		SaleSlashOutSprite.enabled = active;
		SaleSlashOutSprite_alt.enabled = active;
		SaleSleeveSprite.enabled = active;
		SaleOldCostLabel.enabled = active;
		SaleNewCostLabel.enabled = active;
		
		DefaultBuyLabel.enabled = !active;
		DefaultCostLabel.enabled = !active;
	}
	
	public void Refresh()
	{
		if (_data != null && viewController != null)
		{	
			// populate fields from data
			//gameObject.transform.Find("CellContents/GraphicsAnchor/ImageButton").GetComponent<UIButtonMessage>().target = scrollList;	// link up 'buy' button
			foreach (GameObject go in buttons)
				go.GetComponent<UIButtonMessage>().target = scrollList;	// link up 'buy' button
			
			gameObject.transform.Find("CellContents").GetComponent<UIButtonMessage>().target = viewController;			
			gameObject.transform.Find("CellContents/FontAnchor/LabelTitle").GetComponent<UILocalize>().SetKey(_data.Title);
			gameObject.transform.Find("CellContents/FontAnchor/LabelDescription").GetComponent<UILocalize>().SetKey(_data.Description);

			
			SetNotificationIcon();	// show notification icon if can afford to purchase this item
			
			// set status and icon
			if (GameProfile.SharedInstance.Player.IsPowerPurchased(_data.PowerID) == false) 	//-- Check if purchased
			{
				gameObject.transform.Find("CellContents/FontAnchor/LabelBuy").GetComponent<UILabel>().enabled = true;
				gameObject.transform.Find("CellContents/FontAnchor/LabelBuy").GetComponent<UILocalize>().SetKey("Lbl_Buy");	// set localized 'buy' button text
				gameObject.transform.Find("CellContents/FontAnchor/LabelEquipped").GetComponent<UILabel>().enabled = false;	//.text = "";	//.SetKey("");
				
				if ( _data.DiscountCost > 0 )
				{
					//gameObject.transform.Find("CellContents/FontAnchor/LabelCost").GetComponent<UILabel>().text = _data.DiscountCost.ToString();	// show price & coin icon if not
					
					// "Discounts" could be use to increase cost. If it is, do not highlight increase.
					if ( _data.DiscountCost < _data.Cost )
					{
						_toggleSaleDisplay( true );	
						//gameObject.transform.Find( "CellContents/GraphicsAnchor/sale_OLD_LabelCost").GetComponent<UILabel>().text = _data.Cost.ToString();
						SaleOldCostLabel.text = _data.Cost.ToString();
						SaleNewCostLabel.text = _data.DiscountCost.ToString();

					}
					else
					{
						DefaultCostLabel.text = _data.DiscountCost.ToString();
						_toggleSaleDisplay( false );
					}
				}
				else
				{	
					_toggleSaleDisplay( false );
					DefaultCostLabel.text = _data.Cost.ToString();
					//gameObject.transform.Find("CellContents/FontAnchor/LabelCost").GetComponent<UILabel>().text = _data.Cost.ToString();	// show price & coin icon if not
				}
				gameObject.transform.Find("CellContents/IconAnchor/SpriteIcon").GetComponent<UISprite>().spriteName = _data.IconName;
				gameObject.transform.Find("CellContents/GraphicsAnchor/CoinDisplayIcon").GetComponent<UISprite>().enabled = true;
				
				EnableButton(0);
				
				SetBackgroundDarkening(false);
			}
			else if (GameProfile.SharedInstance.IsPowerEquipped(_data.PowerID, GameProfile.SharedInstance.GetActiveCharacter().characterId) == true)	//-- Check if equipped also
			{
				_toggleSaleDisplay( false );
				
				gameObject.transform.Find("CellContents/FontAnchor/LabelBuy").GetComponent<UILabel>().enabled = false;	//.SetKey("Lbl_Equipped"); // set localized 'active' button text
				gameObject.transform.Find("CellContents/FontAnchor/LabelEquipped").GetComponent<UILabel>().enabled = true;
				gameObject.transform.Find("CellContents/FontAnchor/LabelEquipped").GetComponent<UILocalize>().SetKey("Lbl_Purchased");	//Lbl_Equipped");
				gameObject.transform.Find("CellContents/FontAnchor/LabelCost").GetComponent<UILabel>().text = " ";
				gameObject.transform.Find("CellContents/IconAnchor/SpriteIcon").GetComponent<UISprite>().spriteName = _data.IconName;
				gameObject.transform.Find("CellContents/GraphicsAnchor/CoinDisplayIcon").GetComponent<UISprite>().enabled = false;	//.spriteName = "tools_1x1_empty_sprite";	//checkbox_checked";
				gameObject.transform.Find("CellContents/FontAnchor/LabelDescription").GetComponent<UILabel>().color = new Color(52f/255f, 48f/255f, 45f/255f, 1f);
				
				EnableButton(2);
				
				SetBackgroundDarkening(true);	// darken background when equipped
			}
			else 																		// purchased, but not equipped
			{
				_toggleSaleDisplay( false );
				
				gameObject.transform.Find("CellContents/FontAnchor/LabelBuy").GetComponent<UILabel>().enabled = true;
				gameObject.transform.Find("CellContents/FontAnchor/LabelBuy").GetComponent<UILocalize>().SetKey("Lbl_Equip");	// set localized 'equip' button text
				gameObject.transform.Find("CellContents/FontAnchor/LabelEquipped").GetComponent<UILabel>().enabled = true;
				gameObject.transform.Find("CellContents/FontAnchor/LabelEquipped").GetComponent<UILocalize>().SetKey("Lbl_Purchased");
				gameObject.transform.Find("CellContents/FontAnchor/LabelCost").GetComponent<UILabel>().text = " ";
				gameObject.transform.Find("CellContents/IconAnchor/SpriteIcon").GetComponent<UISprite>().spriteName = _data.IconName;
				gameObject.transform.Find("CellContents/GraphicsAnchor/CoinDisplayIcon").GetComponent<UISprite>().enabled = false;	//.spriteName = "tools_1x1_empty_sprite";	//checkbox_unchecked";
				gameObject.transform.Find("CellContents/FontAnchor/LabelDescription").GetComponent<UILabel>().color = new Color(87f/255f, 78f/255f, 69f/255f, 1f);
				
				EnableButton(1);
				
				SetBackgroundDarkening(false);
			}			
		}
		else
			notify.Warning("BasePower (data) or UIInventoryViewControllerOz (viewController) is null in PowerCellData!");
	}
	
	public void SetNotificationIcon()
	{
		if (notificationSystem == null)
			notificationSystem = Services.Get<NotificationSystem>();
		
		bool enable = notificationSystem.GetNotificationStatusForThisCell(NotificationType.Powerup, _data.PowerID);
		notificationIcons.SetNotification(0, (enable) ? 0 : -1);
	}	
	
	private void EnableButton(int id)
	{
		foreach (GameObject go in buttons)
			NGUITools.SetActive(go, false);
		
		NGUITools.SetActive(buttons[id], true);
	}
	
	private void SetBackgroundDarkening(bool darker)
	{
		Color targetColor = (darker) ? new Color(206f/255f, 179f/255f, 137f/255f, 1f) : Color.white;
		background.color = targetColor;
	}
}
	



	//void Update() {}


//	private void SetBackgroundDarkening(bool darker)
//	{
//		float targetAlpha = (darker) ? 1f : 0f;
//
//		TweenAlpha.Begin(darkener, 0.1f, targetAlpha);
//	}
	
				
				//gameObject.transform.Find("CellContents/GraphicsAnchor/ImageButton/Background").GetComponent<UISprite>().color = new Color(56f/255f, 165f/255f, 0f, 1f); // green		
				
				//gameObject.transform.Find("CellContents/GraphicsAnchor/ImageButton/Background").GetComponent<UISprite>().color = new Color(50f/255f, 50f/255f, 50f/255f, 1f); // gray	
				//gameObject.transform.Find("CellContents/GraphicsAnchor/ImageButton").GetComponent<UIButton>().enabled = false;
				
				//gameObject.transform.Find("CellContents/GraphicsAnchor/ImageButton/Background").GetComponent<UISprite>().color = new Color(240f/255f, 140f/255f, 0f, 1f); // orange
				
				


			//gameObject.transform.Find("CellContents/Title").GetComponent<UILabel>().text = data._title;	
			//gameObject.transform.Find("CellContents/Icon").GetComponent<UISprite>().spriteName = data.IconName;
			//gameObject.transform.Find("CellContents/Quantity").GetComponent<UILabel>().text = GameProfile.SharedInstance.Player.consumablesPurchasedQuantity[data.PID].ToString();			
			


		
		//float targetAlpha = (darker) ? 1f : 0f;
		
		//foreach (GameObject go in darkeners)
		//	TweenAlpha.Begin(go, 0.2f, targetAlpha); 
			//go.GetComponent<UISprite>().enabled = darker;
	
				//SetButtonStatus(true);	// show button
	
	
//	private void SetButtonStatus(bool active)
//	{
//		gameObject.transform.Find("CellContents/GraphicsAnchor/ImageButton/Background").GetComponent<UISprite>().enabled = active;
//		gameObject.transform.Find("CellContents/GraphicsAnchor/ImageButton/Highlight").GetComponent<UISprite>().enabled = active;		
//		gameObject.transform.Find("CellContents/GraphicsAnchor/Frame").GetComponent<UISprite>().enabled = active;
//		gameObject.transform.Find("CellContents/FontAnchor/LabelBuy").GetComponent<UILabel>().enabled = active;
//		gameObject.transform.Find("CellContents/GraphicsAnchor/ImageButton").GetComponent<BoxCollider>().enabled = active;
//	}



		
		//bool enable = GameProfile.SharedInstance.Player.CanAffordPower(data.PowerID) &&	// we have enough currency to buy it, and...
//			!GameProfile.SharedInstance.Player.IsPowerPurchased(data.PowerID);	// it's not already purchased
		
//		gameObject.transform.Find("CellContents/GraphicsAnchor/NotificationIconBg").GetComponent<UISprite>().enabled = enable;
//		gameObject.transform.Find("CellContents/IconAnchor/NotificationExclamation").GetComponent<UISprite>().enabled = enable;

	

//			else if (GameProfile.SharedInstance.Player.IsPowerGemmed(data.PowerID) == true)	//-- Check if gemmed also
//			{
//				gameObject.transform.Find("CellContents/Cost").GetComponent<UILabel>().text = "Gemmed";
//				gameObject.transform.Find("CellContents/Icon").GetComponent<UISprite>().spriteName = data.IconName;
//			}	


			
			// populate fields from data
			//gameObject.transform.Find("CellContents").GetComponent<UIButtonMessage>().target = viewController;			
			//gameObject.transform.Find("CellContents/Title").GetComponent<UILabel>().text = data.Title;		
			//gameObject.transform.Find("CellContents/Title").GetComponent<UILocalize>().SetKey(data.Title);		
			//gameObject.transform.Find("CellContents/Description").GetComponent<UILabel>().text = data.Description;
			//gameObject.transform.Find("CellContents/Description").GetComponent<UILocalize>().SetKey(data.Description);

	
			//gameObject.transform.Find("CellContents").GetComponent<CellData>().Data = data.PowerID;
			//gameObject.transform.Find("CellContents").GetComponent<CellData>().cellParent = gameObject.transform;		

			//gameObject.transform.Find("CellContents/Cost").GetComponent<UILabel>().text = data.Cost.ToString();
			//gameObject.transform.Find("CellContents/Icon").GetComponent<UISprite>().spriteName = data.IconName;	
