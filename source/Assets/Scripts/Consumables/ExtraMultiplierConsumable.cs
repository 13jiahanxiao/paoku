using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExtraMultiplierConsumable : BaseConsumable
{
	public ExtraMultiplierConsumable()
	{
		Type = "ExtraMultiplierConsumable";	
	}	
		
	public override void Activate() 
	{
		base.Activate();
		
		GameProfile.SharedInstance.AdditionalScoreMultiplier += (int)Value;
		
		//AudioManager.SharedInstance.PlayFX(AudioManager.Effects.cymbalCrash);
		
		ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.UseConsumableMultiplier,1);
	}
	
	public override void Deactivate() { base.Deactivate(); }
	
	public override bool Update() 
	{
		// Auto deactivate since it doesn't have a duration of effect
		Deactivate();
		return false;
	}
}
