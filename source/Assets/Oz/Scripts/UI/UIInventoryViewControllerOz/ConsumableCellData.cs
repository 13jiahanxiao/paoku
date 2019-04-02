using UnityEngine;
using System.Collections;

public class ConsumableCellData : MonoBehaviour 
{
	public GameObject viewController;
	public GameObject scrollList;									// reference to the scroll list that this cell is parented under the grid/table of	
	public BaseConsumable _data;									// reference to consumable data
	
	public UISprite SaleBurstSprite;
	public UISprite SaleSleeveSprite;
	public UISprite SaleOldCoinSprite;
	public UISprite SaleSlashOutSprite;
	public UILabel SaleSlashOutSprite_alt;
	public UILabel DefaultBuyLabel;
	public UILabel SaleOldCostLabel;
	public UILabel SaleNewCostLabel;
	public UILabel DefaultCostLabel;
	
	private NotificationSystem notificationSystem;	
	private NotificationIcons notificationIcons;		
	
	protected static Notify notify;
	
	void Awake()
	{
		notify = new Notify("ConsumableCellData");
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
	
	public void SetData(BaseConsumable data)
	{
		_data = data;
		
		Refresh();													// populate fields	
	}
	
	private void _toggleSaleDisplay( bool active )
	{
		/*
		gameObject.transform.Find( "CellContents/GraphicsAnchor/sale_Burst" ).GetComponent<UISprite>().enabled = active;
		gameObject.transform.Find( "CellContents/GraphicsAnchor/sale_IconSleeve").GetComponent<UISprite>().enabled = active;
		gameObject.transform.Find( "CellContents/GraphicsAnchor/sale_OLD_CoinDisplayIcon").GetComponent<UISprite>().enabled = active;
		gameObject.transform.Find( "CellContents/FontAnchor/sale_OLD_LabelCost").GetComponent<UILabel>().enabled = active;
		gameObject.transform.Find( "CellContents/GraphicsAnchor/sale_SlashOut").GetComponent<UISprite>().enabled = active;
		*/

		// disable sale related elements if artifact is already maxed out
		if (GameProfile.SharedInstance.Player.IsConsumableMaxedOut(_data.PID))
		{
			active = false;	
		}
		
		SaleBurstSprite.enabled = active;
		SaleSleeveSprite.enabled = active;
		SaleOldCoinSprite.enabled = false;
		SaleSlashOutSprite.enabled = active;
		SaleSlashOutSprite_alt.enabled = active;
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
			gameObject.transform.Find("CellContents/GraphicsAnchor/ImageButton").GetComponent<UIButtonMessage>().target = scrollList;	// link up 'buy' button
			
			gameObject.transform.Find("CellContents").GetComponent<UIButtonMessage>().target = viewController;			
			//gameObject.transform.Find("CellContents/Title").GetComponent<UILabel>().text = _data._title;					
			gameObject.transform.Find("CellContents/FontAnchor/LabelTitle").GetComponent<UILocalize>().SetKey(_data.Title);
			//gameObject.transform.Find("CellContents/FontAnchor/LabelSubtitle").GetComponent<UILocalize>().SetKey(_data.Subtitle);
			gameObject.transform.Find("CellContents/IconAnchor/SpriteIcon").GetComponent<UISprite>().spriteName = _data.IconName;
			gameObject.transform.Find("CellContents/FontAnchor/LabelDescription").GetComponent<UILocalize>().SetKey(_data.Description);
			gameObject.transform.Find("CellContents/FontAnchor/LabelQuantity").GetComponent<UILabel>().text = GameProfile.SharedInstance.Player.consumablesPurchasedQuantity[_data.PID].ToString();
			SetNotificationIcon();	// show notification icon if can afford to purchase this item
			
			// set status and icon
			if (GameProfile.SharedInstance.Player.IsConsumableMaxedOut(_data.PID) == false) 	//-- Check if purchased
			{
				

				_toggleSaleDisplay( false );
					
				//gameObject.transform.Find("CellContents/FontAnchor/LabelCost").GetComponent<UILabel>().text = _data.ActualCost.ToString();	// show price & coin icon if not
				DefaultCostLabel.text = _data.ActualCost.ToString();
				
				
				if(_data.SortPriority==1)
				{
				_toggleSaleDisplay(true);
				SaleOldCostLabel.text = _data.Cost.ToString();
				SaleNewCostLabel.GetComponent<UILocalize>().SetKey("Super_Consumable_Price");
				gameObject.transform.Find("CellContents/FontAnchor/LabelQuantity").GetComponent<UILabel>().text = " ";
				gameObject.transform.Find("CellContents/IconAnchor/SlicedSprite (icon_corner)").GetComponent<UISprite>().enabled = false;
				//gameObject.transform.Find("CellContents/GraphicsAnchor/sale_NEW_LabelCost").GetComponent<UILabel>().text = "";
					
				}else
				{
				gameObject.transform.Find("CellContents/GraphicsAnchor/CoinDisplayIcon").GetComponent<UISprite>().enabled = true;
				}
				gameObject.transform.Find("CellContents/GraphicsAnchor/CoinDisplayIcon").GetComponent<UISprite>().spriteName = "currency_coin";
			}
			else 																			// maxed out
			{
				_toggleSaleDisplay( false );
				
				//gameObject.transform.Find("CellContents/Cost").GetComponent<UILabel>().text = "(Maxed Out at " + ConsumableStore.maxOfEachConsumable.ToString() + ")";
				//gameObject.transform.Find("CellContents/CoinDisplayIcon").GetComponent<UISprite>().spriteName = "empty1x1";
				gameObject.transform.Find("CellContents/FontAnchor/LabelCost").GetComponent<UILabel>().text = " ";
				gameObject.transform.Find("CellContents/GraphicsAnchor/CoinDisplayIcon").GetComponent<UISprite>().enabled = false;	//.spriteName = "tools_1x1_empty_sprite"; // "empty1x1";
			}
		}
		else
			notify.Warning("BaseConsumable (data) or UIInventoryViewControllerOz (viewController) is null in ConsumableCellData!");
	}

	public void SetNotificationIcon()
	{
		if (notificationSystem == null)
			notificationSystem = Services.Get<NotificationSystem>();		
		
		bool enable = notificationSystem.GetNotificationStatusForThisCell(NotificationType.Consumable, _data.PID);		
		notificationIcons.SetNotification(0, (enable) ? 0 : -1);
	}
}





				//SetButtonStatus(true);

				//SetButtonStatus(false);		// remove 'buy' button if maxed out...


	//void Update() {}

	
//	private void SetButtonStatus(bool active)
//	{
//		gameObject.transform.Find("CellContents/GraphicsAnchor/ImageButton/Background").GetComponent<UISprite>().enabled = active;
//		gameObject.transform.Find("CellContents/GraphicsAnchor/ImageButton/Highlight").GetComponent<UISprite>().enabled = active;
//		gameObject.transform.Find("CellContents/GraphicsAnchor/Frame").GetComponent<UISprite>().enabled = active;
//		gameObject.transform.Find("CellContents/FontAnchor/LabelBuy").GetComponent<UILabel>().enabled = active;
//		gameObject.transform.Find("CellContents/GraphicsAnchor/ImageButton").GetComponent<BoxCollider>().enabled = active;
//	}		
	


		
//		bool enable = GameProfile.SharedInstance.Player.CanAffordConsumable(_data.PID) &&	// we have enough currency to buy it, and...
//					!GameProfile.SharedInstance.Player.IsConsumableMaxedOut(_data.PID);		// it's not maxed out		
//		gameObject.transform.Find("CellContents/GraphicsAnchor/NotificationIconBg").GetComponent<UISprite>().enabled = enable;
//		gameObject.transform.Find("CellContents/IconAnchor/NotificationExclamation").GetComponent<UISprite>().enabled = enable;

			// populate fields from data
			//gameObject.transform.Find("CellContents").GetComponent<UIButtonMessage>().target = viewController;			
			//gameObject.transform.Find("CellContents/Title").GetComponent<UILabel>().text = data.Title;	
			//gameObject.transform.Find("CellContents/Title").GetComponent<UILocalize>().SetKey(data.Title);					
			//gameObject.transform.Find("CellContents/Description").GetComponent<UILabel>().text = data.Description;			
			//gameObject.transform.Find("CellContents/Description").GetComponent<UILocalize>().SetKey(data.Description);
			//gameObject.transform.Find("CellContents/Icon").GetComponent<UISprite>().spriteName = data.IconName;
			//gameObject.transform.Find("CellContents/Quantity").GetComponent<UILabel>().text = GameProfile.SharedInstance.Player.consumablesPurchasedQuantity[data.PID].ToString();
			




//
//			else if (GameProfile.SharedInstance.Player.IsArtifactGemmed(data.PID) == true)	//-- Check if gemmed also
//			{
//				gameObject.transform.Find("CellContents/Cost").GetComponent<UILabel>().text = "  (Gemmed)";
//				gameObject.transform.Find("CellContents/CoinDisplayIcon").GetComponent<UISprite>().spriteName = "line1x1"; // "empty1x1";
//			}	

			//gameObject.transform.Find("CellContents").GetComponent<CellData>().Data = data._id;
			//gameObject.transform.Find("CellContents").GetComponent<CellData>().cellParent = gameObject.transform;	

//		go = HierarchyUtils.GetChildByName("Cost", newCell);
//		if (go != null) 
//		{
//			GameObject buyButton = HierarchyUtils.GetChildByName("BuyButton", newCell);
//			GameObject CoinDisplayIcon = HierarchyUtils.GetChildByName("CoinDisplayIcon", newCell);
//			UILabel cost = go.GetComponent<UILabel>() as UILabel;
//			if (cost != null)
//			{
//				if (purchased == true) 
//				{
//					if(CoinDisplayIcon != null) { NGUITools.SetActive(CoinDisplayIcon, false); }
//					
//					if (equipped == true) 
//					{
//						cost.text = "equipped";
//						if (buyButton != null) { NGUITools.SetActive(buyButton, false); }
//					}
//					else 
//					{
//						cost.text = "available";	//equip";
//						if (buyButton != null) { NGUITools.SetActive(buyButton, true); }
//					}
//				}
//				else 
//				{
//					if (buyButton != null) { NGUITools.SetActive(buyButton, true); }
//					cost.text = coinValue;
//					if (CoinDisplayIcon != null) { NGUITools.SetActive(CoinDisplayIcon, true); }
//				}
//			}