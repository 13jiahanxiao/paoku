using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InstaMagicWandPower : BasePower {
	
	public InstaMagicWandPower() {
		Type = "InstaMagicWandPower";
		Title = "Magic Wand";
		Description = "Magic Wand that destroys baboons.";
		IconName = "storeItemBoost@2x";
		Duration = 0.0f;
		Cost = 1000;
		Active = false;
	}
	
	private const float ShootDist = 35f;
	
	public override Dictionary<string, object> ToDict() {
		Dictionary<string, object> d = base.ToDict();
		//d.Add ("Type", this.GetType());
		
		return d;
	}
	
	public override void activate() {
		base.activate();
		AudioManager.Instance.PlayFX(AudioManager.Effects.oz_MagicWand_01);
		
		//GamePlayer.SharedInstance.SpawnPlayerParticleEffect(PlayerParticleEffect.Wand,1.0f,true);
		
		//mDuration = ApplyBuff(mDuration);
		//mDuration = ApplyGemmedUpgrade(mDuration);
		
		UIManagerOz.SharedInstance.inGameVC.coinMeter.AnimateCoinMeter(0.5f);
		
		
		GamePlayer.SharedInstance.StartCoroutine(
			GamePlayer.SharedInstance.playerFx.ShootWandParticle(
				GamePlayer.SharedInstance.transform.position + GamePlayer.SharedInstance.transform.forward*15f+Vector3.up*5) );
		
		// Take into account any artifacts
		//mDuration = GameProfile.SharedInstance.GetStatValue(StatType.BoostDistance, (float)mDuration);
		//The first command kills attatched objects, the second kills monkeys out of the sky.
		GamePlayer.SharedInstance.StartCoroutine(Shoot());
	//	Monkey.KillOne();
		
		ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.PowerupUsedMagicWand,1);
		
		//Let bonus buttons know that this power was used (this will be moved)
		UIManagerOz.SharedInstance.inGameVC.bonusButtons.AddAbilityUsedFlag(AbilityUsed.Pow6);
		
	}
	
	IEnumerator Shoot()
	{
		TrackPiece tp = GamePlayer.SharedInstance.OnTrackPiece;
		float dist = 0f;
		while(tp!=null && dist < ShootDist)
		{
			tp.gameObject.BroadcastMessage("Kill",SendMessageOptions.DontRequireReceiver);
			dist+=tp.EstimatedPathLength;
			tp = tp.NextTrackPiece;
			yield return null;
		}
	}
	
//	public override void deactivate() {
//		base.deactivate();
//	}
	
	public override bool update() {
		return false;
	}
}
