using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//public enum StoreItemType { CoinBundle, GemBundle, Wallpaper, Character, MovieTickets, CoinOffers, }  // In 'Store' UI screens, 'Misc' page includes Wallpaper, Character and MovieTickets

public class StoreItem
{
	//public string internalTitle = "";	
	public string title = "";	
	public string description = "";
	public string icon = "";
	public CostType costType = CostType.Coin;
	public int cost = 1;	// cents, so 100 = $1.00
	public int markDownCost = 1;
	public StoreItemType itemType = StoreItemType.CoinBundle;
	public int itemQuantity = 1;	
	public int sortPriority = 0;	
	public int id = 0;
	
	public bool _showFoldOut = false;
	
	public void SetDataFromDictionary(Dictionary<string, object> data)
	{
		//if (data.ContainsKey("InternalTitle"))
		//	internalTitle = (string)data["InternalTitle"];
		
		if (data.ContainsKey("Title"))
			title = (string)data["Title"];
		
		if (data.ContainsKey("Description"))
			description = (string)data["Description"];
		
		if (data.ContainsKey("Icon"))
			icon = (string)data["Icon"];
		
		if (data.ContainsKey("CostType")) 
			costType = (CostType)Enum.Parse(typeof(CostType), (string)data["CostType"]);
		
		if (data.ContainsKey("Cost"))
			cost = int.Parse((string)data["Cost"]);
		
		if (data.ContainsKey("MarkDownCost"))
				markDownCost = int.Parse((string)data["MarkDownCost"]);
		
		if (data.ContainsKey("ItemType"))
			itemType = (StoreItemType)Enum.Parse(typeof(StoreItemType), (string)data["ItemType"]);
		
		if (data.ContainsKey("ItemQuantity"))
			itemQuantity = int.Parse((string)data["ItemQuantity"]);		
		
		if (data.ContainsKey("SortPriority"))
			sortPriority = int.Parse((string)data["SortPriority"]);
		
		if (data.ContainsKey("ID"))
			id = int.Parse((string)(data["ID"]));
	}
	
	public string ToJson() 
	{
		Dictionary<string, object> d = this.ToDict();
		return MiniJSON.Json.Serialize(d);
	}
	
	public Dictionary<string, object> ToDict() 
	{
		Dictionary<string, object> d = new Dictionary<string, object>();
		//d.Add ("InternalTitle", internalTitle);
		d.Add ("Title", title);
		d.Add ("Description", description);	
		d.Add ("Icon", icon);
		d.Add ("CostType", costType.ToString());		
		d.Add ("Cost", cost.ToString());
		d.Add ("MarkDownCost", markDownCost.ToString());
		d.Add ("ItemType", itemType.ToString());
		d.Add ("ItemQuantity", itemQuantity.ToString());		
		d.Add ("SortPriority", sortPriority.ToString());
		d.Add ("ID", id.ToString());
		return d;
	}
}
