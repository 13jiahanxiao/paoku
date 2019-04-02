using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InstaShieldPower : BasePower {
	
	public InstaShieldPower() {
		Type = "InstaShieldPower";			
		Title = "Shield";
		Description = "Helps protect you from obstacles";
		IconName = "storeItemVacuum@2x";
		Duration = 5.0f;
		Cost = 1000;
		Active = false;
	}
	
	
	public override Dictionary<string, object> ToDict() {
		Dictionary<string, object> d = base.ToDict();
		//d.Add ("Type", this.GetType());
		
		return d;
	}
	
	public override void activate() {
		base.activate();
		GamePlayer.SharedInstance.StartShield();
		
		Duration = ApplyBuff(Duration);
		
		// Take into account any artifacts
		Duration = GameProfile.SharedInstance.GetStatValue(StatType.ShieldDuration, (float)Duration);
		
		GamePlayer.SharedInstance.ShieldDuration = (float)Duration;
		//UIManagerOz.SharedInstance.ActivePowerIcon();
	}
	
//	public override void deactivate() {
//		base.deactivate();
//	}
	
	public override bool update() {
		if(GamePlayer.SharedInstance == null)
			return false;
		if(GamePlayer.SharedInstance.IsDead == true || GamePlayer.SharedInstance.HasShield == false) {
			deactivate();
			return false;
		}
		
		return true;
	}
}

