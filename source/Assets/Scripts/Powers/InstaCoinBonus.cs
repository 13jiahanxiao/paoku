using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InstaCoinBonus : BasePower {
	
	public InstaCoinBonus() {
		Type = "InstaCoinBonus";		
		Title = "Coin Bonus";
		Description = "Instant Boost when you fill the meter.";
		IconName = "storeItemBoost@2x";
		Duration = 0.0f;
		Cost = 1000;
		Active = false;
	}
	
	//private const int AddScore = 500;
	
	public override Dictionary<string, object> ToDict() {
		Dictionary<string, object> d = base.ToDict();
		//d.Add ("Type", this.GetType());
		
		return d;
	}
	
	public override void activate() {
		base.activate();
		
		//mDuration = ApplyBuff(mDuration);
		//Duration = ApplyGemmedUpgrade(Duration);
		
		GamePlayer.SharedInstance.AddCoinsToScore((int)Duration);
		
		UIManagerOz.SharedInstance.inGameVC.coinMeter.AnimateCoinMeter(0.5f);
		
		//UIManagerOz.SharedInstance.ActivePowerIcon();
		
		//AudioManager.SharedInstance.PlayFX(AudioManager.Effects.cymbalCrash);
		
		//GamePlayer.SharedInstance.SpawnPlayerParticleEffect(PlayerParticleEffect.ObjectGrab, 1.0f, true);
		GamePlayer.SharedInstance.playerFx.StartObjectGrab();
		
		//Let bonus buttons know that this power was used (this will be moved)
		UIManagerOz.SharedInstance.inGameVC.bonusButtons.AddAbilityUsedFlag(AbilityUsed.Pow2);
	}
	
	public override bool update() {
		// Auto deactivate since it doesn't have a duration of effect
		deactivate();
		return false;
	}
}

