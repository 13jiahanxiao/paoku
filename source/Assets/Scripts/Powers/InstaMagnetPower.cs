using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InstaMagnetPower : BasePower {
	
	public InstaMagnetPower() {
		Type = "InstaMagnetPower";
		Title = "Instant Magnet";
		Description = "Instant Magnet when you fill the meter.";
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
		
		//mDuration = ApplyBuff(mDuration);
		//Duration = ApplyGemmedUpgrade(Duration);
		
		// Take into account any artifacts
		Duration = GameProfile.SharedInstance.GetMagnetDuration();
		
		UIManagerOz.SharedInstance.inGameVC.coinMeter.AnimateCoinMeter((float)Duration);
		
		GamePlayer.SharedInstance.StartVacuum((float)Duration);
		
		GamePlayer.SharedInstance.SetPowerUsed((float)Duration);
		
		//GamePlayer.SharedInstance.VacuumDuration = (float)Duration;
		//UIManagerOz.SharedInstance.ActivePowerIcon();
		
		ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.PowerupUsedMagnet,1);
		
		//Let bonus buttons know that this power was used (this will be moved)
		UIManagerOz.SharedInstance.inGameVC.bonusButtons.AddAbilityUsedFlag(AbilityUsed.Pow5);
	}
	
//	public override void deactivate() {
//		base.deactivate();
//	}
	
	public override bool update() {
		if(GamePlayer.SharedInstance == null)
			return false;
		if(GamePlayer.SharedInstance.IsDead == true || GamePlayer.SharedInstance.HasVacuum == false) {
			deactivate();
			return false;
		}
		
		return true;
	}
}
