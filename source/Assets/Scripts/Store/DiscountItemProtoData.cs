using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public enum DiscountIcon 
{ 
	None = 0, 
	Sale = 1,
	LimitedTimeOffer = 2,
	MostPopular = 3
}

public enum DiscountItemType
{
	Artifact = 0,
	Consumable = 1,
	Powerup = 2,
	StoreItem = 3,
	Character = 4
}

public class DiscountItemProtoData
{
	protected static Notify notify;
	
	public int _id = 0;
	public int _salePriority = 0;
	public int _costValue = 0;
	public CostType _costType = CostType.Coin;
	public DiscountIcon _discountIcon = DiscountIcon.None;
	private string _shortCode = "";
	public string ShortCode 
	{
		get
		{
			return _shortCode;
		}
		
		set
		{
			_shortCode = value;
		}
	}
	
	private DiscountItemType _itemType = DiscountItemType.Consumable;
	public DiscountItemType ItemType
	{
		get
		{
			return _itemType;
		}
		set
		{
			_itemType = value;
		}
	}
	
	public DiscountItemProtoData(Dictionary<string, object> dict)
	{
		notify = new Notify(this.GetType().Name);
		
		if (dict.ContainsKey("ID")) {
			_id = JSONTools.ReadInt(dict["ID"]);
		}
		else if (dict.ContainsKey("PID")) {
			_id = JSONTools.ReadInt(dict["PID"]);
		}

		if (dict.ContainsKey("salePriority")) {
			_salePriority = JSONTools.ReadInt(dict["salePriority"]);	
		}
		
		if (dict.ContainsKey("CostValue")) {
			_costValue = JSONTools.ReadInt(dict["CostValue"]);	
		}
		
		if (dict.ContainsKey("CostType")) {
			if (Enum.IsDefined(typeof(CostType), dict["CostType"])) {
				_costType = (CostType)Enum.Parse(typeof(CostType), (string)dict["CostType"]);	
			}
		}
		
		if (dict.ContainsKey("shortCode")) {
			ShortCode = dict["shortCode"].ToString();
		}
		
		if (dict.ContainsKey("DiscountIcon")) {
			if (Enum.IsDefined(typeof(DiscountIcon), dict["DiscountIcon"])) {
				_discountIcon = (DiscountIcon)Enum.Parse(typeof(DiscountIcon), (string)dict["DiscountIcon"]);
			}
		} else if (dict.ContainsKey("discountIconIndex")) {
			switch (JSONTools.ReadInt(dict["discountIconIndex"]))
			{
				case 0:
					_discountIcon = DiscountIcon.None;
					break;
				case 1:
					_discountIcon = DiscountIcon.Sale;
					break;
				case 2:
					_discountIcon = DiscountIcon.LimitedTimeOffer;
					break;
				case 3:
					_discountIcon = DiscountIcon.MostPopular;
					break;
			}
		}
		
		if (dict.ContainsKey("ItemType")) 
		{
			ItemType = (DiscountItemType)Enum.Parse(typeof(DiscountItemType), dict["ItemType"].ToString());
		}
	}
	
	public string ToJson() { return MiniJSON.Json.Serialize(this.ToDict()); }
	
	public Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> d = new Dictionary<string, object>();
		
		d.Add("ID", _id);
		d.Add("salePriority", _salePriority);
		d.Add("CostValue", _costValue);
		d.Add("CostType", _costType);
		d.Add("DiscountIcon", _discountIcon);
		d.Add("shortCode", ShortCode);
		d.Add("ItemType", ItemType);
		
		return d;
	}
}

