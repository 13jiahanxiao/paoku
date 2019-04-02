/*
using UnityEngine;
using System.Collections;

public class UIStatViewController : UIViewController
{
	public UILabel singleScore = null;
	public UILabel singleDistance = null;
	public UILabel singleCoins = null;
	public UILabel lifeTimeGames = null;
	public UILabel lifeTimeCoins = null;
	public UILabel lifeTimeDistance = null;
	public UILabel version = null;
	public UILabel mutliplier = null;
	
	public override void appear ()
	{
		base.appear ();
		
		if(singleCoins != null) {
			singleCoins.text = string.Format("{0:#,###0}", GameProfile.SharedInstance.Player.bestCoinScore);
		}
		if(singleDistance != null) {
			singleDistance.text = string.Format("{0:#,###0}", (int)GameProfile.SharedInstance.Player.bestDistanceScore);
		}
		if(singleScore != null) {
			singleScore.text = string.Format("{0:#,###0}", GameProfile.SharedInstance.Player.bestScore);
		}
		if(mutliplier != null) {
			mutliplier.text = "Multiplier: "+GameProfile.SharedInstance.GetScoreMultiplier().ToString()+"x";
		}
		if(lifeTimeCoins != null) {
			lifeTimeCoins.text = string.Format("{0:#,###0}", GameProfile.SharedInstance.Player.lifetimeCoins);
		}
		if(lifeTimeDistance != null) {
			lifeTimeDistance.text = string.Format("{0:#,###0}", GameProfile.SharedInstance.Player.lifetimeDistance);
		}
		if(lifeTimeGames != null) {
			lifeTimeGames.text = string.Format("{0:#,###0}", GameProfile.SharedInstance.Player.lifetimePlays);
		}
		if(version != null) {
			version.text = "v1.0b (r4940)";
		}
	}
	
}

 */