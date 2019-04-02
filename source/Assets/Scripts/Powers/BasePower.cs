using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BasePower 
{
	public double Duration { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public string Type { get; set; }
	public string BuffDescription { get; set; }
	public string IconName { get; set; }
	public bool Active { get; set; }
	public int Cost { get; set; }
	public CostType CostType { get; set; }
	public int SortPriority { get; set; }	
	public int PowerID { get; set; }	//-- Do not save.
	public float fillRateMultiplier { get; set; }
	
	public int ServerCost { get; set; }
	
	public int DefaultSortPriority { get; set; }
	
	public Buff ProtoBuff
	{
		get { return mProtoBuff; }
		set
		{
			Buff b = (Buff)value;
			mProtoBuff = new Buff(b.ToDict());
		}
	}
	protected Buff mProtoBuff;
	
	public virtual void activate() 
	{
		Active = true;
		//Dictionary<string, string> flurryData = new Dictionary<string, string>();
		//flurryData.Add("Title", this.Title);
		//FlurryBinding.logEventWithParameters("powerActivate", flurryData, false);

		ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.UseAbility,1);
		if(this.GetType()!=typeof(InstaPoofPower))
			ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.UsePowerupOtherThanShield,1);
	}
	
	public virtual void deactivate() 
	{
		Active = false;
		if (GameProfile.SharedInstance != null) 
		{
			GameProfile.SharedInstance.Player.ConsumeBuffUse(BuffType.Powerup, PowerID);
		}
	}
	
	// This doesn't called until the user Activates the power, Double Tap for example.
	// call deactivate and return false when the power is DONE being active.
	public virtual bool update()
	{
		return false;
	}

	public BasePower(Dictionary<string, object> data)
	{
		SetDataFromDictionary(data);	
	}
	
	public BasePower() 
	{
		Type = "InstaScoreBonus";
		Title = "Empty";
		Description = "Empty";
		BuffDescription = "Make better with Gems";
		IconName = null;
		Duration = 1.0f;	//0.0f;
		Cost = 1;
		CostType = CostType.Coin;
		Active = false;
		ProtoBuff = new Buff();
		SortPriority = 0;

		ServerCost = 0;
	}
	
// eyal edit
//#if UNITY_EDITOR	
	public bool _showFoldOut = false;
//#endif
	
	public int DiscountCost
	{
		get
		{
			return ServerCost;
		}
	}
	
	public int GetFinalCost()
	{
		if ( ServerCost > 0 )
		{
			return ServerCost;
		}
		else
		{
			return Cost;
		}
	}
	
	public void SetDataFromDictionary(Dictionary<string, object> data) 
	{
		if (data.ContainsKey("Title"))
			Title = (string)data["Title"];
		
		if (data.ContainsKey("Description"))
			Description = (string)data["Description"];
		
		if (data.ContainsKey("Type"))
			Type = (string)data["Type"];	
		
		if (data.ContainsKey("BuffDescription"))
			BuffDescription = (string)data["BuffDescription"];
		
		if (data.ContainsKey("IconName"))
			IconName = (string)data["IconName"];
	
		if (data.ContainsKey("PID"))
			PowerID = JSONTools.ReadInt(data["PID"]);
		
		if (data.ContainsKey("Value"))
		{
			object obj = data["Value"];
			if (obj.GetType() == typeof(Int64)) 
				Duration = (double)((long) obj);
			else if (obj.GetType() == typeof(int)) 
				Duration = (int)(obj);
			else if (obj.GetType() == typeof(Double)) 
				Duration = (double)(obj);
		}
		if (data.ContainsKey("Cost"))
		{
			object obj = data["Cost"];
			if (obj.GetType() == typeof(Int64))
				Cost = (int)((long) obj);
			else if (obj.GetType() == typeof(int))
				Cost = (int)((obj));
			else if (obj.GetType() == typeof(Double)) 
				Cost = (int)((double)(obj));
		}
		
		CostType = CostType.Coin;
		if (data.ContainsKey("CostType"))
		{
			if (Enum.IsDefined(typeof(CostType), data["CostType"])) 
				CostType = (CostType)Enum.Parse(typeof(CostType), (string)data["CostType"]);
		}
		
		mProtoBuff = new Buff();
		if (data.ContainsKey("ProtoBuff"))
		{
			Dictionary<string, object> buffData = data["ProtoBuff"] as Dictionary<string, object>;
			if (buffData != null) 
				mProtoBuff = new Buff(buffData);
		}
		
		SortPriority = 0;
		if (data.ContainsKey("SortPriority"))
			SortPriority = int.Parse((string)data["SortPriority"]);	
		
		fillRateMultiplier = 1f;
		if (data.ContainsKey("fillRateMultiplier"))
			fillRateMultiplier = float.Parse((string)data["fillRateMultiplier"]);	
		else
			fillRateMultiplier = 1f;
		
		Active = false;
	}
	
	public string ToJson() 
	{
		Dictionary<string, object> d = this.ToDict();
		return MiniJSON.Json.Serialize(d);
	}
	
	public virtual Dictionary<string, object> ToDict() 
	{
		Dictionary<string, object> d = new Dictionary<string, object>();
		d.Add ("Title", Title);
		d.Add ("IconName", IconName);
		d.Add ("Type", Type);
		d.Add ("Description", Description);
		d.Add ("BuffDescription", BuffDescription);
		d.Add ("Cost", Cost);
		d.Add ("Value", Duration);
		d.Add ("CostType", CostType.ToString());
		d.Add ("ProtoBuff", ProtoBuff.ToDict());
		d.Add ("SortPriority", SortPriority.ToString());
		d.Add ("PID", PowerID);
		d.Add ("fillRateMultiplier", fillRateMultiplier.ToString());
		
		return d;
	}
	
	public double ApplyBuff(double defaultValue) 
	{
		Buff activeBuff = GameProfile.SharedInstance.Player.findActiveBuff(BuffType.Powerup, PowerID);
		if (activeBuff != null) 
		{
			if (activeBuff.statValueType == StatValueType.Percent)
				defaultValue = defaultValue + (defaultValue*activeBuff.statValue);
			else if (activeBuff.statValueType == StatValueType.Relative)
				defaultValue = defaultValue + (activeBuff.statValue);
			else if (activeBuff.statValueType == StatValueType.Absolute)
				defaultValue = (float)activeBuff.statValue;
		}
		return defaultValue;
	}
	
	// Use this for Oz
	public double ApplyGemmedUpgrade(double defaultValue)
	{
		Buff activeBuff = mProtoBuff;
		if (activeBuff != null && GameProfile.SharedInstance.Player.powersGemmed.Contains(PowerID))
		{
			if (activeBuff.statValueType == StatValueType.Percent) 
				defaultValue = defaultValue + (defaultValue*activeBuff.statValue);
			else if (activeBuff.statValueType == StatValueType.Relative)
				defaultValue = defaultValue + (activeBuff.statValue);
			else if (activeBuff.statValueType == StatValueType.Absolute)
				defaultValue = (float)activeBuff.statValue;
		}
		return defaultValue;
	}
}




		//Redmond/Sean Hack probably need to fix this later.

//		if (data.ContainsKey("fillRateMultiplier"))
//		{
//			float tempResult;
//			if (float.TryParse(data["fillRateMultiplier"].ToString(), out tempResult))
//			{
//				fillRateMultiplier = tempResult;
//			}
//			else
//			{
//				Debug.LogWarning("fillRateMultiplier couldn't parse " +  data["fillRateMultiplier"].ToString() + " as a float");	
//			}
//		}


//		mType = "InstaScoreBonus";
//		mTitle = "Empty";
//		mDescription = "Empty";
//		mBuffDescription = "Make better with Gems";
//		mIconName = null;
//		mDuration = 1.0f;
//		mCost = 1;
//		mCostType = CostType.Coin;
//		mActive = false;
//		mProtoBuff = new Buff();


//	public double Duration 
//	{
//		get { return mDuration; }
//		set { mDuration = value; }
//	}
//	protected double mDuration = 0.0f; 
//	
//	public string Title 
//	{
//		get { return mTitle; }
//		set { mTitle = value; }
//	}
//	protected string mTitle;
//	
//	public string Description 
//	{
//		get { return mDescription; }
//		set { mDescription = value; }
//	}
//	protected string mDescription;
//	
//	public string Type
//	{
//		get { return mType; }
//		set { mType = value; }
//	}
//	protected string mType;	
//	
//	public string BuffDescription 
//	{
//		get { return mBuffDescription; }
//		set { mBuffDescription = value; }
//	}
//	protected string mBuffDescription;
//	
//	public string IconName 
//	{
//		get { return mIconName; }
//		set { mIconName = value; }
//	}
//	protected string mIconName;
//	
//	public bool Active 
//	{
//		get { return mActive; }
//		set { mActive = value; }
//	}
//	protected bool mActive;
//	
//	public int Cost 
//	{
//		get { return mCost; }
//		set { mCost = value; }
//	}
//	protected int mCost;
//	
//	public CostType CostType 
//	{
//		get { return mCostType; }
//		set { mCostType = value; }
//	}
//	protected CostType mCostType;
//	
//	public Buff ProtoBuff
//	{
//		get { return mProtoBuff; }
//		set
//		{
//			Buff b = (Buff)value;
//			mProtoBuff = new Buff(b.ToDict());
//		}
//	}
//	protected Buff mProtoBuff;
//	
//	//-- Do not save.
//	public int PowerID 
//	{
//		get { return mPowerID; }
//		set { mPowerID=value; }
//	}
//	protected int mPowerID;
//	//-- Do not save.
//	