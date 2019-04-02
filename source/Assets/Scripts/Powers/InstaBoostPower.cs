using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InstaBoostPower : BasePower {
	
	public InstaBoostPower() {
		Type = "InstaBoostPower";
		Title = "Instant Boost";
		Description = "Instant Boost when you fill the meter.";
		IconName = "storeItemBoost@2x";
		Duration = 5.0f;
		Cost = 1000;
		Active = false;
	}
	
	private const float boostDistance = 500f;
	
	public override Dictionary<string, object> ToDict() {
		Dictionary<string, object> d = base.ToDict();
		//d.Add ("Type", this.GetType());
		
		return d;
	}
	
	public override void activate() {
		
		if(GamePlayer.SharedInstance.IsOnBalloon)
			return;
		
		base.activate();
		//mDuration = ApplyBuff(mDuration);
		//Duration = ApplyGemmedUpgrade(Duration);
		
		// Take into account any artifacts
		Duration = GameProfile.SharedInstance.GetBoostDistance()/GamePlayer.SharedInstance.getModfiedMaxRunVelocity();
		
		UIManagerOz.SharedInstance.inGameVC.coinMeter.AnimateCoinMeter((float)Duration);
		
		GamePlayer.SharedInstance.StartBoost(GameProfile.SharedInstance.GetBoostDistance());
		
		
		//Make sure coins aren't added while we are using this
		GamePlayer.SharedInstance.SetPowerUsed((float)Duration);
		
		//GamePlayer.SharedInstance.BoostDistanceLeft = (float)Duration;
		
		ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.PowerupUsedBoost,1);
		//UIManagerOz.SharedInstance.ActivePowerIcon();
		
		//Let bonus buttons know that this power was used (this will be moved)
		UIManagerOz.SharedInstance.inGameVC.bonusButtons.AddAbilityUsedFlag(AbilityUsed.Pow4);
	}
	
//	public override void deactivate() {
//		base.deactivate();
//	}
	
	public override bool update() {
		if(GamePlayer.SharedInstance == null)
			return false;
		if(GamePlayer.SharedInstance.IsDead == true || GamePlayer.SharedInstance.HasBoost == false) {
			deactivate();
			return false;
		}
		
		return true;
	}
}
