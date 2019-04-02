using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GatchaType{
	EMPTY,
	COINS,
	SCORE_BONUS,
	//SCORE_MULTIPLIER,
	HeadStart,
	BigHeadStart,
	//BonusMultiplier,
	StumbleProof,
	ThirdEye
}

[System.Serializable]
public class GatchaDataSet{
	public GatchaType type;
	public int amount;
	public int randomWeight = 1;
	public bool active = true;
}

public class GatchaData : MonoBehaviour {
	public List<GatchaDataSet> gatchaList;
}
