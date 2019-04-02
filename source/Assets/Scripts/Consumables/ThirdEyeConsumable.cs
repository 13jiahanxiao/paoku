using UnityEngine;
using System.Collections;

public class ThirdEyeConsumable : BaseConsumable
{
	
	public ThirdEyeConsumable()
	{
		Type = "ThirdEyeConsumable";	
	}
	
	public override void Activate()
	{
		base.Activate();
		
		BonusItem.ShowAllPowerUps(true);
		
		ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.UseConsumableThirdEye,1);
	}
	
	public override void Deactivate()
	{
		base.Deactivate();
		
		BonusItem.ShowAllPowerUps(false);
	}
}
