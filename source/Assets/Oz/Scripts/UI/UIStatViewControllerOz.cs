using UnityEngine;
using System.Collections;

public class UIStatViewControllerOz : UIViewControllerOz
{
	public GameObject SingleRunGroup;
	public GameObject LifetimeGroup;
	
	public UILabel multiplier;
	
	public override void appear()
	{		
		UIManagerOz.SharedInstance.PaperVC.SetPageName("Ttl_Stats", "");
		UIManagerOz.SharedInstance.PaperVC.SetCurrentPage(UIManagerOz.SharedInstance.statsVC);	
		Refresh();
		base.appear();
		
		//		SharingManagerBinding.SetCurrentScreenName( "stats" );
	}	

	public void Refresh()
	{		
		int weekly = GameProfile.SharedInstance.ChallengeScoreMultiplier;
		multiplier.text = (GameProfile.SharedInstance.GetPermanentScoreMultiplier() +  weekly).ToString() + "x";
	//	if(weekly>0)
	//		multiplier.text = GameProfile.SharedInstance.GetPermanentScoreMultiplier() + "x (+" + weekly + "x)";	//GetScoreMultiplier
	//	else
	//		multiplier.text = GameProfile.SharedInstance.GetPermanentScoreMultiplier() + "x";	//GetScoreMultiplier
		
		SingleRunGroup.transform.Find("LabelHighScore#").GetComponent<UILabel>().text = GameProfile.SharedInstance.Player.bestScore.ToString();	
		SingleRunGroup.transform.Find("LabelRun#").GetComponent<UILabel>().text = GameProfile.SharedInstance.Player.bestDistanceScore.ToString();		
		SingleRunGroup.transform.Find("LabelMostCoins#").GetComponent<UILabel>().text = GameProfile.SharedInstance.Player.bestCoinScore.ToString();	
		SingleRunGroup.transform.Find("LabelMostGems#").GetComponent<UILabel>().text = GameProfile.SharedInstance.Player.bestSpecialCurrencyScore.ToString();	

		LifetimeGroup.transform.Find("LabelGames#").GetComponent<UILabel>().text = GameProfile.SharedInstance.Player.lifetimePlays.ToString();	
		LifetimeGroup.transform.Find("LabelDistance#").GetComponent<UILabel>().text = GameProfile.SharedInstance.Player.lifetimeDistance.ToString();	
		LifetimeGroup.transform.Find("LabelTotalCoins#").GetComponent<UILabel>().text = ObjectivesDataUpdater.GetStatForLifetimeObjectiveType(ObjectiveType.CollectCoins,-1).ToString();	
		LifetimeGroup.transform.Find("LabelTotalGems#").GetComponent<UILabel>().text = GameProfile.SharedInstance.Player.lifetimeSpecialCurrency.ToString();		
	}	
}



	//void Update() { }

	//public GameObject RecentGroup; - has been removed N.N.

//		RecentGroup.transform.Find("LabelRecentDistance#").GetComponent<UILabel>().text = ((int)GameController.SharedInstance.DistanceTraveled).ToString();	
//		RecentGroup.transform.Find("LabelRecentScore#").GetComponent<UILabel>().text = GamePlayer.SharedInstance.Score.ToString();
		

