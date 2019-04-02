using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InstaPoofPower : BasePower {
	
	public InstaPoofPower() {
		Type = "InstaPoofPower";			
		Title = "Instant Poof";
		Description = "Instant Poof when you fill the meter.";
		IconName = "storeItemPoof@2x";
		Duration = 10.0f;
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
	//	AudioManager.Instance.PlayFX(AudioManager.Effects.oz_Poof_activate);
		
		//mDuration = ApplyBuff(mDuration);
		//Duration = ApplyGemmedUpgrade(Duration);
		
		// Take into account any artifacts ... NOTE: Should we? Or just check for gem?
		Duration = GameProfile.SharedInstance.GetPoofDuration();
		
		UIManagerOz.SharedInstance.inGameVC.coinMeter.AnimateCoinMeter((float)Duration);
		
		GamePlayer.SharedInstance.StartPoof((float)Duration);
		
		//Make sure coins aren't added while we are using this
		GamePlayer.SharedInstance.SetPowerUsed((float)Duration);
		
		
		ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.PowerupUsedPoof,1);
		
		
		//UIManagerOz.SharedInstance.ActivePowerIcon();
		
		//Let bonus buttons know that this power was used (this will be moved)
		UIManagerOz.SharedInstance.inGameVC.bonusButtons.AddAbilityUsedFlag(AbilityUsed.Pow3);
	}
	
//	public override void deactivate() {
//		base.deactivate();
//	}
	
	public override bool update() {
		if(GamePlayer.SharedInstance == null)
			return false;
		if(GamePlayer.SharedInstance.IsDead == true || GamePlayer.SharedInstance.HasPoof == false) {
			deactivate();
			return false;
		}
		
		return true;
	}
}
