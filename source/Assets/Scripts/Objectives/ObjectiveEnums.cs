using UnityEngine;
using System;

public enum RankRewardType
{ 
	Coins=0, 
	Gems,
	Multipliers,
	HeadStartConsumables, 
	ExtraMultiplierConsumables, 
	NoStumbleConsumables,
	ThirdEyeConsumables,
	MegaHeadStartConsumables, 
}

public enum ObjectiveTimeType 
{ 
	PerRun, 
	OverTime, 
	LifeTime, 
}

public enum ObjectiveFilterType 
{ 
	None, 
	WithoutCoins, 
	WithoutPowerups, 
	WithoutStumble, 
	WithoutArtifacts,
	WithoutTransition,
	PenniesFromHeavenOnly,
	MagicMagnetOnly,
	PoofOnly,
	FinleysFavorOnly,
	ScoreMultiplierOnly,
	JumpOverOnly,
	SlideUnderOnly,
	DodgeOnly,
	WhimsyWoods,
	YellowBrickRoad,
	DarkForest,
	Global,
	BalloonMode,
}

public enum ObjectiveType 
{ 

	CollectCoins=0, 
	Distance, 
	Score, 
	CollectPowerups, 
	CollectSpecialCurrency,
	Resurrects,

	CoinMeterFills, 

	ModifierLevelDoubleCoins,
	ModifierLevelMagician,
	ModifierLevelEnchanter,
	ModifierLevelBargainHunter,
	ModifierLevelLuck,
	ModifiersMaxed,
	
	UseConsumableHeadStart,
	UseConsumableMegaHeadStart,
	UseConsumableStumbleProof,
	UseConsumableThirdEye,
	UseConsumableMultiplier,
	
	PaidDestinyCard,
	
	FastTraveled,
	
	UnlockArtifacts, 
	UseAllInGamePowerups,
	UseEverything,
	
	EnvironmentsVisitedAllTime,
	EnvironmentsVisitedSingleRun,
	
	//All lifetime objectives go before this...
	LifetimeObjectivesCount, //this leaves an empty slot in objectives - do we care?
	

	UnlockPowerups, 
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
	
	
	DistanceWithoutCoins, 
	DistanceWithoutPowerups, 
	DistanceWithoutStumble, 
	DistanceWithoutArtifacts,
	DistanceWithoutTransition,
	
	PickupCollectedPenniesFromHeaven,
	PickupCollectedMagicMagnet,
	PickupCollectedPoof,
	PickupCollectedFinleysFavor,
	PickupCollectedScoreMultiplier,
	
	JumpOverPassed,
	SlideUnderPassed,
	
	BalloonModeEntered,
	
	
	UsedDestinyCard,
	
	
	PowerupUsedPoof,
	PowerupUsedMagnet,
	PowerupUsedBoost,
	PowerupUsedMagicWand,
	PowerupUsedHypnosis,
	PowerupUsedApprentice,
	
	BalloonModeFinished,
	
	PassGap,
	UsePowerupOtherThanShield,
	UseConsumable,
	
	TravelFromEnv1,
	TravelFromEnv2,
	TravelFromEnv3,
	TravelFromEnv4,
	TravelFromEnv5,
	
	// wxj, activity type
	Activity1,
	Activity2,
	Activity3,
	Activity4,
	
	TotalObjectivesCount		// this needs to be last!
	
}

public enum ObjectiveCategory 
{ 
	Coin,
	Collection,
	Discovery, 
	Distance, 
	Lifetime, 
	Obstacles, 
	Purchases, 
	Score, 
	Skill, 
}

public enum ObjectiveDifficulty
{
	//Difficulty0, 
	Difficulty1 = 1, // start with difficulty 1 now
	Difficulty2, 
	Difficulty3, 
	Difficulty4, 
	Difficulty5, 
	Difficulty6,
	Difficulty7, 
}

public enum ObjectiveLocation
{
	WhimsyWoods,
	YellowBrickRoad,
	DarkForest,
	Global,
	BalloonMode,	
}
