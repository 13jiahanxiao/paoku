using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum ConsumableType 
{
	BaseConsumable, 
	HeadStartConsumable, 
	ExtraMultiplierConsumable, 
	NoStumbleConsumable,
	ThirdEyeConsumable,
	FastTravelConsumable,
	//MegaHeadStartConsumable,	// removed as separate class, still exist as a separate level reward though
}

public class BaseConsumable
{
	public string Type { get; set; }	
	public string Title { get; set; }
	public string Subtitle { get; set; }
	public string Description { get; set; }
	public string IconName { get; set; }
	public int Cost { get; set; }
	public CostType CostType { get; set; }
	public float Value { get; set; }	
	public float Duration { get; set; }		
	public int SortPriority { get; set; }	
	public int PID { get; set; }
	
	public int ServerCost { get; set; }
	
	public int DefaultSortPriority { get; set; }
	
	protected bool Active { get; set; }	//-- Do not save.
	
// eyal edit	
//#if UNITY_EDITOR	
	//public bool _showFoldOut = false;
//#endif
	public bool _showFoldOut = false;
	
	//public BaseConsumable(Dictionary<string, object> data) { SetDataFromDictionary(data); }
	
	public BaseConsumable() 
	{
		Type = "BaseConsumable";
		Title = "";
		Subtitle = "";
		Description = "";	
		IconName = "";
		Cost = 0;
		CostType = CostType.Coin;
		Value = 0.0f;
		Duration = 0.0f;
		SortPriority = 0;
		PID = 0;		
		Active = false;
		
		ServerCost = 0;
	}
	
	public int DiscountCost
	{
		get
		{
			int cost = ServerCost;
			
			if (
				this.GetType() == typeof( HeadStartConsumable ) ||
				this.GetType() == typeof( FastTravelConsumable )
			) {
				cost = (int)( (float) ServerCost * GameProfile.SharedInstance.GetHeadStartDiscount() );
			}
			return cost;
		}
	}
	
	public int ActualCost
	{
		get
		{
			int cost = Cost;
			
			if (this.GetType()==typeof(HeadStartConsumable) || this.GetType()==typeof(FastTravelConsumable)) //added fast travel so can get discount as well N.N.
				cost = (int)((float)Cost * GameProfile.SharedInstance.GetHeadStartDiscount());
		
			return cost;
		}
	}
	
	public int GetFinalCost()
	{
		
		if ( ServerCost > 0 )
		{
			return DiscountCost;
		}
		else
		{
			return ActualCost;
		}
	}
	
	public virtual void Activate() 
	{
		ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.UseConsumable,1);
		Active = true;
	}
	
	public virtual void Deactivate() 
	{
		Active = false;
		//if (GameProfile.SharedInstance != null) { GameProfile.SharedInstance.Player.ConsumeBuffUse(BuffType.Powerup, PID); }
	}
	
	//-- This doesn't called until the user Activates the power, Double Tap for example.  Call deactivate and return false when the power is DONE being active.
	public virtual bool Update() { return false; }
	
	public void SetDataFromDictionary(Dictionary<string, object> data) 
	{
		if (data.ContainsKey("Type")) 
			Type = (string)data["Type"];	
			
		if (data.ContainsKey("Title"))
			Title = (string)data["Title"];
				
		if (data.ContainsKey("Subtitle"))
			Subtitle = (string)data["Subtitle"];		
		
		if (data.ContainsKey("Description"))
			Description = (string)data["Description"];
					
		if (data.ContainsKey("IconName"))
			IconName = (string)data["IconName"];
					
		if (data.ContainsKey("Cost"))
			Cost = Int32.Parse((string)data["Cost"]);
							
		if (data.ContainsKey("CostType"))
			CostType = (CostType)Enum.Parse(typeof(CostType), (string)data["CostType"]);
								
		if (data.ContainsKey("Value"))
			Value = Single.Parse((string)data["Value"]);
									
		if (data.ContainsKey("Duration"))
			Duration = Single.Parse((string)data["Duration"]);
										
		if (data.ContainsKey("SortPriority"))
			SortPriority = int.Parse((string)data["SortPriority"]);
		
		if (data.ContainsKey("PID"))
			PID = Int32.Parse((string)data["PID"]);	
	}
	
	public string ToJson()
	{
		Dictionary<string, object> d = this.ToDict();
		return MiniJSON.Json.Serialize(d);
	}
	
	public Dictionary<string, object> ToDict() 
	{
		Dictionary<string, object> d = new Dictionary<string, object>();
		d.Add ("Type", Type);		
		d.Add ("Title", Title);
		d.Add ("Subtitle", Subtitle);
		d.Add ("Description", Description);		
		d.Add ("IconName", IconName);
		d.Add ("Cost", Cost.ToString());
		d.Add ("CostType", CostType.ToString());
		d.Add ("Value", Value.ToString());		
		d.Add ("Duration", Duration.ToString());
		d.Add ("SortPriority", SortPriority.ToString());		
		d.Add ("PID", PID.ToString());
		return d;
	}
}
