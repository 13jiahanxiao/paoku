using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InstaHypnosis : BasePower {
	
	public InstaHypnosis() {
		Title = "Instant Hypnosis";
		Description = "Instant Hypnosis when you fill the meter.";
		IconName = "storeItemHypnosis@2x";
		Duration = 15.0f;
		Cost = 1000;
		Active = false;
	}
	
	
	
	public override Dictionary<string, object> ToDict() {
		Dictionary<string, object> d = base.ToDict();
		if(!d.ContainsKey("Type"))
			d.Add ("Type", this.GetType());
		
		return d;
	}
	
	private Transform particle;
	
	public override void activate() {
		base.activate();
		//mDuration = ApplyBuff(mDuration);
		//Duration = ApplyGemmedUpgrade(Duration);
		AudioManager.Instance.PlayFX(AudioManager.Effects.oz_TimeClock_01);
		
		// Take into account any artifacts (do these exist for hypnosis?)
		Duration = GameProfile.SharedInstance.GetHypnosisDuration();
		
		GamePlayer.SharedInstance.Hypnotize((float)Duration);
		
		UIManagerOz.SharedInstance.inGameVC.coinMeter.AnimateCoinMeter((float)Duration*Time.timeScale);
		
		GamePlayer.SharedInstance.SetPowerUsed((float)Duration*Time.timeScale);	//Divide by two, since this time isnt adjusted by timescale
		
		//GamePlayer.SharedInstance.SpawnPlayerParticleEffect(PlayerParticleEffect.Hypnosis,(float)Duration,true,true);
		
		ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.PowerupUsedHypnosis,1);
		
		//UIManagerOz.SharedInstance.ActivePowerIcon();
		
		
		//Let bonus buttons know that this power was used (this will be moved)
	//	UIManagerOz.SharedInstance.inGameVC.bonusButtons.AddAbilityUsedFlag(AbilityUsed.Pow2);
	}
	
	
	
//	public override void deactivate() {
//		base.deactivate();
//	}
	
	public override bool update() {
		if(GamePlayer.SharedInstance == null)
			return false;
		if(GamePlayer.SharedInstance.IsDead == true || Time.timeScale==1f) {
			deactivate();
			return false;
		}
		
		return true;
	}
}
