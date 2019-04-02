using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SaleBanner : MonoBehaviour 
{
	private DateTime _endDate = DateTime.MinValue;
	private int _previousSeconds = -1;
	
	protected Notify notify;
	
	private delegate void _refreshBannerOnExpireHandler();
	private event _refreshBannerOnExpireHandler onBannerExpiredEvent = null;
	private void _registerForOnBannerExpired( _refreshBannerOnExpireHandler delg )
	{
		onBannerExpiredEvent += delg;
	}
	private void _unregisterForOnBannerExpire( _refreshBannerOnExpireHandler delg )
	{
		onBannerExpiredEvent -= delg;
	}
	
	private void _expireBanner()
	{
		if ( UIManagerOz.SharedInstance.worldOfOzVC.gameObject.active )
		{
			UIManagerOz.SharedInstance.worldOfOzVC.Refresh();
		}
		else if ( UIManagerOz.SharedInstance.inventoryVC.gameObject.active )
		{
			UIManagerOz.SharedInstance.inventoryVC.Refresh();
		}
	}
	
	public void Start()
	{
		notify = new Notify( this.GetType().Name );
	}

	
	/// <summary>
	/// Sets the sale banner status.  For scaling scroll list clipping area down to accomodate sale banner.
	/// </summary>
	/// <param name='clippedPanel'>
	/// Clipped panel.
	/// </param>
	public void SetSaleBannerStatus(UIPanel clippedPanel, GameObject scrollListRoot, DiscountItemType itemType, SaleBanner saleBanner)
	{
		CancelInvoke( "UpdateTimer" );
		
		//List<WeeklyDiscountProtoData> specificDiscounts = Services.Get<Store>().GetComponent<WeeklyDiscountManager>().GetDiscountsByItemType( itemType	 );
		List<WeeklyDiscountProtoData> specificDiscounts = Services.Get<NotificationSystem>().GetSalesViewable(itemType);

		if (specificDiscounts.Count > 0 && specificDiscounts[0]._maxDiscount > 0 && specificDiscounts[0]._itemList.Count > 0)
		{
			scrollListRoot.transform.localPosition = new Vector3(scrollListRoot.transform.localPosition.x, 
				-37f, scrollListRoot.transform.localPosition.z);
			clippedPanel.clipRange = new Vector4(clippedPanel.clipRange.x, clippedPanel.clipRange.y, clippedPanel.clipRange.z, 607f);
			saleBanner.gameObject.active = true;
			
			saleBanner.transform.Find("LabelSale").GetComponent<UILabel>().enabled = true;
			saleBanner.transform.Find("LabelTimer").GetComponent<UILabel>().enabled = true;
			saleBanner.transform.Find("LabelOff").GetComponent<UILabel>().enabled = true;
			
			string saleText = "";
			
			if ( specificDiscounts[0]._itemList.Count == 1 )
			{
				string percentOff = Localization.SharedInstance.Get( "Lbl_SaleBanner_Off" );
				
				saleText = string.Format( percentOff, specificDiscounts[0]._maxDiscount );
			}
			else
			{
				string percentOff = Localization.SharedInstance.Get( "Lbl_SaleBanner_Upto" );
				
				saleText = string.Format( percentOff, specificDiscounts[0]._maxDiscount );
			}
			
			saleBanner.transform.Find( "LabelOff" ).GetComponent<UILabel>().text = saleText;
			
			// Set the text to the localized string if the saleBody is not equal to ""
			if ( specificDiscounts[0]._saleBody != "" )
			{
				saleBanner.transform.Find( "LabelSale" ).GetComponent<UILabel>().text = specificDiscounts[0]._saleBody;
			}
			else
			{
				saleBanner.transform.Find( "LabelSale" ).GetComponent<UILocalize>().SetKey( "Lbl_SaleBanner_Sale" );
			}
			string countDown = FormatTimeSpan( specificDiscounts[0]._endDate - System.DateTime.UtcNow );
			
			_endDate = specificDiscounts[0]._endDate;
			
			if ( _endDate > DateTime.UtcNow && onBannerExpiredEvent == null )
			{
				_registerForOnBannerExpired( _expireBanner );
			}
			
			saleBanner.transform.Find( "LabelTimer" ).GetComponent<UILabel>().text = countDown;
			
			Invoke( "UpdateTimer", 1.0f );
		}
		else
		{
			scrollListRoot.transform.localPosition = new Vector3(scrollListRoot.transform.localPosition.x, 
				0f, scrollListRoot.transform.localPosition.z);
			clippedPanel.clipRange = new Vector4(clippedPanel.clipRange.x, clippedPanel.clipRange.y, clippedPanel.clipRange.z, 681f);
			saleBanner.gameObject.active = false;

		}
	}
	
	void UpdateTimer()
	{	
		if ( _previousSeconds != DateTime.UtcNow.Second )
		{			
			string countDown = FormatTimeSpan( _endDate - System.DateTime.UtcNow );
			
			this.transform.Find( "LabelTimer" ).GetComponent<UILabel>().text = countDown;
			
			if ( _endDate <= DateTime.UtcNow && onBannerExpiredEvent != null )
			{
				onBannerExpiredEvent();
				_unregisterForOnBannerExpire( _expireBanner );
			}

		}
		Invoke( "UpdateTimer", 1.0f );
	}
	
   	private string FormatTimeSpan(TimeSpan span)
   	{
		if (span <= TimeSpan.Zero)
			return "00:00:00:00";
		else
		  	return span.Days.ToString("00") + ":" + 
				span.Hours.ToString("00") + ":" + 
				span.Minutes.ToString("00") + ":" + 
				span.Seconds.ToString("00");
   }	
}



		
		/*
		List<DiscountItemProtoData> specificDiscounts = new List<DiscountItemProtoData>();
		
		foreach (WeeklyDiscountProtoData discount in allDiscounts)
		{
			foreach (DiscountItemProtoData item in discount._itemList)
			{
				if (item.ItemType == itemType)
				{
					specificDiscounts.Add(item);
				}
			}
		}
		*/
		