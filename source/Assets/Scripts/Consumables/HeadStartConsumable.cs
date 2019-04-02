using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeadStartConsumable : BaseConsumable
{
	public HeadStartConsumable()
	{
		Type = "HeadStartConsumable";	
	}
	
	public override void Activate() 
	{
		base.Activate();
		
		GamePlayer.SharedInstance.StartBoost(Value);
		//GamePlayer.SharedInstance.BoostDistanceLeft += Value;
		GamePlayer.SharedInstance.HasSuperBoost = true;
		
		if(Value<=1000)
			ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.UseConsumableHeadStart,1);
		else
			ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.UseConsumableMegaHeadStart,1);
	}
	
	public override void Deactivate() { base.Deactivate(); }
	
	public override bool Update() 
	{
		//if(GamePlayer.SharedInstance == null) { return false; }
		//if(GamePlayer.SharedInstance.IsDead == true || GamePlayer.SharedInstance.HasBoost == false) 
		//{
		//	Deactivate();
		//	return false;
		//}
		return true;
	}
}
