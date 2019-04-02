using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


public class WeeklyDiscountProtoData 
{
	public string _saleTitle = ""; // Default to blank "saleTitle";
	public string _saleBody = ""; // Default to blank "saleBody";
	public int _saleIndex = 0;
	public DateTime _endDate;
	public DiscountItemType _itemType = DiscountItemType.Artifact;
	public int _maxDiscount = 0;
	public List<DiscountItemProtoData> _itemList = null;
	
	public WeeklyDiscountProtoData(Dictionary<string, object> dict)
	{
		if (dict.ContainsKey("saleIndex")) {
			_saleIndex = JSONTools.ReadInt(dict["saleIndex"]);	
		}
		
		if (dict.ContainsKey("saleTitle")) {
			_saleTitle = (string) dict["saleTitle"];
		}
		
		if (dict.ContainsKey("saleBody")) {
			_saleBody = (string) dict["saleBody"];	
		}
		
		if (dict.ContainsKey("endDate")) {
			_endDate = DateTime.Parse((string) dict["endDate"]);	
		}
		
		if ( dict.ContainsKey( "itemTypeName" ) )
		{
			if ( Enum.IsDefined( typeof( DiscountItemType ), dict[ "itemTypeName"] ) )
			{
				_itemType = (DiscountItemType) Enum.Parse( typeof( DiscountItemType ), (string)dict["itemTypeName"] );
			}
		}
		
		if ( dict.ContainsKey( "maxDiscount" ) )
		{
			_maxDiscount = JSONTools.ReadInt( dict["maxDiscount"] );
		}
		
		_itemList = new List<DiscountItemProtoData>();
		if (dict.ContainsKey("Items")) {
			List<object> itemObjList = dict["Items"] as List<object>;
			
			if (itemObjList != null)
			{
				_itemList.Clear();
				
				foreach (object itemObj in itemObjList)
				{
					Dictionary<string,object> itemDict = itemObj as Dictionary<string,object>;
					DiscountItemProtoData tempDiscountData = new DiscountItemProtoData(itemDict);
					
					_itemList.Add(tempDiscountData);
				}
			}
		}
	}
	
	public string ToJson() { return MiniJSON.Json.Serialize(this.ToDict()); }
	
	public Dictionary<string,object> ToDict()
	{
		Dictionary<string, object> d = new Dictionary<string, object>();
		d.Add("saleIndex", _saleIndex);
		d.Add("saleTitle", _saleTitle);
		d.Add("saleBody", _saleBody);
		d.Add("endDate", _endDate);

		List<object> itemList = new List<object>();
		foreach (DiscountItemProtoData item in _itemList)
		{
			itemList.Add(item.ToDict());	
		}
		d.Add("Items", itemList);
		
		return d;
	}
}

