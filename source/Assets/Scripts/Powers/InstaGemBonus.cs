using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InstaGemBonus : BasePower {
	
	public InstaGemBonus() {
		Type = "InstaGemBonus";			
		Title = "Gem Bonus";
		Description = "Instant Gem Bonus when you fill the meter.";
		IconName = "storeItemBoost@2x";
		Duration = 0.0f;
		Cost = 1000;
		Active = false;
	}
	
	private const int addGems = 5;
	
	public override Dictionary<string, object> ToDict() {
		Dictionary<string, object> d = base.ToDict();
		//d.Add ("Type", this.GetType());
		
		return d;
	}
	
	public override void activate() {
		base.activate();
		
		//mDuration = ApplyBuff(mDuration);
		Duration = ApplyGemmedUpgrade(Duration);
		
		GamePlayer.SharedInstance.AddGemsToScore(addGems);
		
		//UIManagerOz.SharedInstance.ActivePowerIcon();
		
		AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_UI_Menu_levelUp);
		
		//GamePlayer.SharedInstance.SpawnPlayerParticleEffect(PlayerParticleEffect.ObjectGrab, 1.0f, true);
		GamePlayer.SharedInstance.playerFx.StartObjectGrab();
	}
	
	public override bool update() {
		// Auto deactivate since it doesn't have a duration of effect
		deactivate();
		return false;
	}
}

