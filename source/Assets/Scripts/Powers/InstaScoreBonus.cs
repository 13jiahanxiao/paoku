using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InstaScoreBonus : BasePower {
	
	public InstaScoreBonus() {
		Type = "InstaScoreBonus";	
		Title = "Score Bonus";
		Description = "Instant Boost when you fill the meter.";
		IconName = "storeItemBoost@2x";
		Duration = 0.0f;
		Cost = 1000;
		Active = false;
	}
	
	private const int addScore = 1000;
	
	public override Dictionary<string, object> ToDict() {
		Dictionary<string, object> d = base.ToDict();
		//d.Add ("Type", this.GetType());
		
		return d;
	}
	
	public override void activate() {
		base.activate();
		
		//mDuration = ApplyBuff(mDuration);
		//Duration = ApplyGemmedUpgrade((float)addScore);
		
		GamePlayer.SharedInstance.AddScore(addScore);
		
		UIManagerOz.SharedInstance.inGameVC.coinMeter.AnimateCoinMeter(0.5f);
		
		//UIManagerOz.SharedInstance.ActivePowerIcon();
		
		AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_ScoreMultiplier_01);
		
		GamePlayer.SharedInstance.playerFx.StartScoreBonusEffects();
		UIManagerOz.SharedInstance.inGameVC.scoreUI.ScoreBonusEffects();		
		
		ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.PowerupUsedApprentice,1);
		
		
		//Let bonus buttons know that this power was used (this will be moved)
		UIManagerOz.SharedInstance.inGameVC.bonusButtons.AddAbilityUsedFlag(AbilityUsed.Pow1);
	}
	
	public override bool update() {
		// Auto deactivate since it doesn't have a duration of effect
		deactivate();
		return false;
	}
}
