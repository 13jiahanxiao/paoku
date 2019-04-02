using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BonusItemProtoData : MonoBehaviour {

	static public BonusItemProtoData SharedInstance = null;
	
	public bool AllowBonusItems = true;
	
	public float BonusItemPlacementHeight = 2.75f;
	public float VacuumRangeRadius2 = 1200.0f;
	public float VacuumStrength = 500.0f;
	public float VacuumCoinVelocityMax = 25.0f;
//	public float BoostRunDistance = 250.0f;		//now controllled in GameProfile
	public float BoostAcceleration = 150.0f;
//	public float BoostAlphaFadeInSpeed = 0.25f;
//	public float BoostAlphaFadeOutSpeed = 0.25f;
	public float BoostAlphaBlinkSpeed = 2.0f;
	public float BoostStartSlowDownDistance = 50.0f;
	public float BoostTextureOffsetSpeed = 2.0f;
	public float MaxTimeBetweenBonusItems = 15.0f;
	
	public int MaxCoinsPerRun = 25;
	public int MinCoinsPerRun = 10;
	public float MinDistanceBetweenCoinRuns = 50.0f;
	public float MinDistanceBetweenBonusItems = 500.0f;
	public float MinDistanceBetweenGems = 500.0f;
	public float MinDistanceBetweenTornadoTokens = 500.0f;
	public int MaxChanceTokensPerRun = 2;
	public float MinDistanceAfterTurn = 20.0f;
	//public int	MegaCoinValue = 25;	//Editable value now lives in GameProfile
	
	public float DistanceToChangeDoubleCoins = 500.0f;
	public float DistanceToChangeTripleCoins = 1000.0f;
	public bool AllowCoins = true;
	public bool AllowDoubleCoins = false;
	public bool AllowTripleCoins = false;
	public bool AllowCenterCoins = true;
	
	public float SwirlPlacementChance = 0.5f;
	public float ArcMaxHeight = 2.5f;
	public float ProbabilityBonusItem = 1f;
	public float ProbablityTornadoBonus = 0.1f;
	public float ProbabilityCoinBonus = 1f;
	public float ProbabilityVacuum = 1f;
	public float ProbabilityBoost = 1f;
//	public float ProbabilityShield = 1f;
	//public float ProbabilityGem = 1f;
	public float ProbabilityPoof = 1f;
	public float ProbabilityScoreBonus = 1f;
	
	
	void Awake()
	{
		SharedInstance = this;
	}
	
	// Use this for initialization
	void Start () 
	{
		SharedInstance = this;
	}
}

//[System.Serializable]
//public class TrackPieceTypes : System.Object
//{
//	public TrackPiece.PieceType TrackType = TrackPiece.PieceType.kTPMaxx;
//	public List<string> Types = new List<string>();
//}
