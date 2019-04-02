using UnityEngine;
using System.Collections;

public class ChallengeDataUpdater : MonoBehaviour
{
	//void Start () { }
	//void Update () { }
	
	public void SetChallengeData()
	{
		/*int totalPowerups = GameController.SharedInstance.collectedBonusItemPerRun[(int)BonusItem.BonusItemType.Boost]
							+ GameController.SharedInstance.collectedBonusItemPerRun[(int)BonusItem.BonusItemType.Vacuum];*/
		
		/*if (totalPowerups == 0) 
		{ 
			SetChallengeDataWithFilter(ObjectiveFilterType.WithoutPowerups); 
		}
		
		if (GameController.SharedInstance.StumblesThisRun == 0) 
		{ 
			SetChallengeDataWithFilter(ObjectiveFilterType.WithoutStumble); 
		}
		
		if (GamePlayer.SharedInstance.CoinCountTotal == 0) 
		{ 
			SetChallengeDataWithFilter(ObjectiveFilterType.WithoutCoins); 
		}*/
		
		SetChallengeDataWithFilter(ObjectiveFilterType.None);
	}

	private void SetChallengeDataWithFilter(ObjectiveFilterType filter) 
	{
		ObjectivesManager objManager = Services.Get<ObjectivesManager>();
		
	/*	int totalPowerups = GameController.SharedInstance.collectedBonusItemPerRun[(int)BonusItem.BonusItemType.Boost];
		totalPowerups += GameController.SharedInstance.collectedBonusItemPerRun[(int)BonusItem.BonusItemType.Vacuum];
		
		objManager.AddChallengeStat(ObjectiveType.CollectCoins, ObjectiveTimeType.PerRun, filter, GamePlayer.SharedInstance.CoinCountTotal);
		objManager.AddChallengeStat(ObjectiveType.CollectCoins, ObjectiveTimeType.OverTime, filter, GamePlayer.SharedInstance.CoinCountTotal);	
		objManager.AddChallengeStat(ObjectiveType.CollectSpecialCurrency, ObjectiveTimeType.PerRun, filter, GamePlayer.SharedInstance.GemCountTotal);
		objManager.AddChallengeStat(ObjectiveType.CollectSpecialCurrency, ObjectiveTimeType.OverTime, filter, GamePlayer.SharedInstance.GemCountTotal);
		objManager.AddChallengeStat(ObjectiveType.Distance, ObjectiveTimeType.PerRun, filter, (int)GameController.SharedInstance.DistanceTraveled);
		objManager.AddChallengeStat(ObjectiveType.Distance, ObjectiveTimeType.OverTime, filter, (int)GameController.SharedInstance.DistanceTraveled);
		objManager.AddChallengeStat(ObjectiveType.Score, ObjectiveTimeType.PerRun, filter, GamePlayer.SharedInstance.Score);
		objManager.AddChallengeStat(ObjectiveType.Score, ObjectiveTimeType.OverTime, filter, GamePlayer.SharedInstance.Score);
		objManager.AddChallengeStat(ObjectiveType.CollectPowerups, ObjectiveTimeType.PerRun, filter, totalPowerups);
		objManager.AddChallengeStat(ObjectiveType.CollectPowerups, ObjectiveTimeType.OverTime, filter, totalPowerups);
		objManager.AddChallengeStat(ObjectiveType.Resurrects, ObjectiveTimeType.PerRun, filter, GameController.SharedInstance.ResurrectsThisRun);
		objManager.AddChallengeStat(ObjectiveType.Resurrects, ObjectiveTimeType.OverTime, filter, GameController.SharedInstance.ResurrectsThisRun);
		//objManager.AddChallengeStat(ObjectiveType.HeadStarts, ObjectiveTimeType.PerRun, filter, GameController.SharedInstance.HeadStartsThisRun);
		//objManager.AddChallengeStat(ObjectiveType.HeadStarts, ObjectiveTimeType.OverTime, filter, GameController.SharedInstance.HeadStartsThisRun);
		*/
		/*	need to add:
			UnlockPowerups, 
			UnlockArtifacts, 
			UnlockCharacters, 
			UnlockConsumables,
			EnvironmentSwitch,
			UseAbility,
			EmbueModifier,
			PassAnObstacle,
			PassTheWitch,
			PassTheBaboon,
			PassTheSnapDragons,
			PassTheTombstones,
			PassTheWinkies,
			PassTheCornfields,
			ReachLocation,
			ExitLocation,
		*/	
		
		objManager.CompleteChallenges();
		objManager.SaveWeeklyChallenges();
	}		
}
