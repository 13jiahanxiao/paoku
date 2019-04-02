using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class LifetimeStatEnvironmentProtoData 	
{
	public float _collectCoins = 0;
	public float _distance = 0;
	public float _score = 0;
	public float _collectPowerups = 0;
	public float _collectSpecialCurrency = 0;
	public float _resurrects = 0;
	
	public float _coinMeterFills = 0;
	public float _modifierLevelDoubleCoins = 0;
	public float _modifierLevelMagician = 0;
	public float _modifierLevelEnchanter = 0;
	public float _modifierLevelBargainHunter = 0;
	public float _modifierLevelLuck = 0;
	public float _modifiersMaxed = 0;
	
	public float _useConsumableHeadStart = 0;
	public float _useConsumableMegaHeadStart = 0;
	public float _useConsumableStumbleProof = 0;
	public float _useConsumableThirdEye = 0;
	public float _useConsumableMultiplier = 0;
	
	public float _paidDestinyCard = 0;
	
	public float _fastTraveled = 0;
	
	public float _unlockArtifacts = 0;
	public float _useAllInGamePowerups = 0;
	public float _useEverything = 0;
	
#pragma warning disable 414
	public Notify notify;
#pragma warning restore 414
	
	public LifetimeStatEnvironmentProtoData(Dictionary<string, object>  dict)
	{
		notify = new Notify("LifetimeStatEnvironmentProtoData");
		
		if (dict.ContainsKey("cc"))   { _collectCoins = (float) JSONTools.ReadDouble(dict["cc"]);}
		if (dict.ContainsKey("di"))   { _distance = (float) JSONTools.ReadDouble(dict["di"]); }
		if (dict.ContainsKey("sc"))   { _score = (float) JSONTools.ReadDouble(dict["sc"]);  }
		if (dict.ContainsKey("cp"))   { _collectPowerups = (float) JSONTools.ReadDouble(dict["cp"]); }
		if (dict.ContainsKey("csc"))  { _collectSpecialCurrency = (float) JSONTools.ReadDouble(dict["csc"]); }
		if (dict.ContainsKey("rez"))  { _resurrects = (float) JSONTools.ReadDouble(dict["rez"]); }
		if (dict.ContainsKey("cmf"))  { _coinMeterFills = (float) JSONTools.ReadDouble(dict["cmf"]); }
		if (dict.ContainsKey("mdc"))  { _modifierLevelDoubleCoins = (float) JSONTools.ReadDouble(dict["mdc"]); }
		if (dict.ContainsKey("mm"))   { _modifierLevelMagician = (float) JSONTools.ReadDouble(dict["mm"]); }
		if (dict.ContainsKey("me"))   { _modifierLevelEnchanter = (float) JSONTools.ReadDouble(dict["me"]); }
		if (dict.ContainsKey("mbh"))  { _modifierLevelBargainHunter = (float) JSONTools.ReadDouble(dict["mbh"]); }
		if (dict.ContainsKey("ml"))   { _modifierLevelLuck = (float) JSONTools.ReadDouble(dict["ml"]); }
		if (dict.ContainsKey("mmx"))  { _modifiersMaxed = (float) JSONTools.ReadDouble(dict["mmx"]); }
		if (dict.ContainsKey("uch"))  { _useConsumableHeadStart = (float) JSONTools.ReadDouble(dict["uch"]); }
		if (dict.ContainsKey("ucmh")) { _useConsumableMegaHeadStart = (float) JSONTools.ReadDouble(dict["ucmh"]); }
		if (dict.ContainsKey("ucsp")) { _useConsumableStumbleProof = (float) JSONTools.ReadDouble(dict["ucsp"]); }
		if (dict.ContainsKey("ucte")) { _useConsumableThirdEye = (float) JSONTools.ReadDouble(dict["ucte"]); }
		if (dict.ContainsKey("ucm"))  { _useConsumableMultiplier = (float) JSONTools.ReadDouble(dict["ucm"]); }
		if (dict.ContainsKey("pdd"))  { _paidDestinyCard = (float) JSONTools.ReadDouble(dict["pdd"]); }
		if (dict.ContainsKey("ft"))   { _fastTraveled = (float) JSONTools.ReadDouble(dict["ft"]); }
		if (dict.ContainsKey("ua"))   { _unlockArtifacts = (float) JSONTools.ReadDouble(dict["ua"]); }
		if (dict.ContainsKey("uagp")) { _useAllInGamePowerups = (float) JSONTools.ReadDouble(dict["uagp"]); }
		if (dict.ContainsKey("uAll")) { _useEverything = (float) JSONTools.ReadDouble(dict["uAll"]); }
		
	}
	
	public LifetimeStatEnvironmentProtoData(float[] statArray)
	{
		notify = new Notify("LifetimeStatEnvironmentProtoData");
		
    	if ((int)ObjectiveType.CollectCoins < statArray.Length)
			_collectCoins = statArray[(int)ObjectiveType.CollectCoins];
		
		if ((int)ObjectiveType.Distance < statArray.Length)
			_distance = statArray[(int)ObjectiveType.Distance];
		
		if ((int)ObjectiveType.Score < statArray.Length) {
			_score = statArray[(int)ObjectiveType.Score];
		}
		
		if ((int)ObjectiveType.CollectPowerups < statArray.Length) {
			_collectPowerups = statArray[(int)ObjectiveType.CollectPowerups];
		}
		
		if ((int)ObjectiveType.CollectSpecialCurrency < statArray.Length) {
			_collectSpecialCurrency = statArray[(int)ObjectiveType.CollectSpecialCurrency];
		}
		
		if ((int)ObjectiveType.Resurrects < statArray.Length) {
			_resurrects = statArray[(int)ObjectiveType.Resurrects];
		}
		
		if ((int)ObjectiveType.CoinMeterFills < statArray.Length) {
			_coinMeterFills = statArray[(int)ObjectiveType.CoinMeterFills];
		}
		
		if ((int)ObjectiveType.ModifierLevelDoubleCoins < statArray.Length) {
			_modifierLevelDoubleCoins = statArray[(int)ObjectiveType.ModifierLevelDoubleCoins];
		}

		if ((int)ObjectiveType.ModifierLevelMagician < statArray.Length) {
			_modifierLevelMagician = statArray[(int)ObjectiveType.ModifierLevelMagician];
		}
		
		if ((int)ObjectiveType.ModifierLevelEnchanter <statArray.Length) {
			_modifierLevelEnchanter = statArray[(int)ObjectiveType.ModifierLevelEnchanter];
		}
		
		if ((int)ObjectiveType.ModifierLevelBargainHunter < statArray.Length) {
			_modifierLevelBargainHunter = statArray[(int)ObjectiveType.ModifierLevelBargainHunter];
		}
		
		if ((int)ObjectiveType.ModifierLevelLuck < statArray.Length) {
			_modifierLevelLuck = statArray[(int)ObjectiveType.ModifierLevelLuck];
		}
		
		if ((int)ObjectiveType.ModifiersMaxed < statArray.Length) {
			_modifiersMaxed = statArray[(int)ObjectiveType.ModifiersMaxed];
		}
		
		if ((int)ObjectiveType.UseConsumableHeadStart < statArray.Length) {
			_useConsumableHeadStart = statArray[(int)ObjectiveType.UseConsumableHeadStart];
		}
		
		if ((int)ObjectiveType.UseConsumableMegaHeadStart < statArray.Length) {
			_useConsumableMegaHeadStart = statArray[(int)ObjectiveType.UseConsumableMegaHeadStart];
		}
		
		if ((int)ObjectiveType.UseConsumableStumbleProof < statArray.Length) {
			_useConsumableStumbleProof = statArray[(int)ObjectiveType.UseConsumableStumbleProof];
		}
		
		if ((int)ObjectiveType.UseConsumableThirdEye < statArray.Length) {
			_useConsumableThirdEye = statArray[(int)ObjectiveType.UseConsumableThirdEye];
		}
		
		if ((int)ObjectiveType.UseConsumableMultiplier < statArray.Length) {
			_useConsumableMultiplier = statArray[(int)ObjectiveType.UseConsumableMultiplier];
		}
		
		if ((int)ObjectiveType.PaidDestinyCard < statArray.Length) {
			_paidDestinyCard = statArray[(int)ObjectiveType.PaidDestinyCard];
		}
		
		if ((int)ObjectiveType.FastTraveled < statArray.Length) {
			_fastTraveled = statArray[(int)ObjectiveType.FastTraveled];
		}
		
		if ((int)ObjectiveType.UnlockArtifacts < statArray.Length) {
			_unlockArtifacts = statArray[(int)ObjectiveType.UnlockArtifacts];
		}
		
		if ((int)ObjectiveType.UseAllInGamePowerups < statArray.Length) {
			_useAllInGamePowerups = statArray[(int)ObjectiveType.UseAllInGamePowerups];
		}
		
		if ((int)ObjectiveType.UseEverything < statArray.Length) {
			_useEverything = statArray[(int)ObjectiveType.UseEverything];
		}
				
	}
			
	public LifetimeStatEnvironmentProtoData(Dictionary<ObjectiveType, float> dict)
	{
		notify = new Notify("LifetimeStatEnvironmentProtoData");
		
		if (dict.ContainsKey(ObjectiveType.CollectCoins)) {
			_collectCoins = (float) JSONTools.ReadDouble(dict[ObjectiveType.CollectCoins]);
		}
		
		if (dict.ContainsKey(ObjectiveType.Distance)) {
			_distance = (float) JSONTools.ReadDouble(dict[ObjectiveType.Distance]);
		}
		
		if (dict.ContainsKey(ObjectiveType.Score)) {
			_score = (float) JSONTools.ReadDouble(dict[ObjectiveType.Score]);
		}
		
		if (dict.ContainsKey(ObjectiveType.CollectPowerups)) {
			_collectPowerups = (float) JSONTools.ReadDouble(dict[ObjectiveType.CollectPowerups]);
		}
		
		if (dict.ContainsKey(ObjectiveType.CollectSpecialCurrency)) {
			_collectSpecialCurrency = (float) JSONTools.ReadDouble(dict[ObjectiveType.CollectSpecialCurrency]);
		}
		
		if (dict.ContainsKey(ObjectiveType.Resurrects)) {
			_resurrects = (float) JSONTools.ReadDouble(dict[ObjectiveType.Resurrects]);
		}
		
		if (dict.ContainsKey(ObjectiveType.CoinMeterFills)) {
			_coinMeterFills = (float) JSONTools.ReadDouble(dict[ObjectiveType.CoinMeterFills]);
		}
		
		if (dict.ContainsKey(ObjectiveType.ModifierLevelDoubleCoins)) {
			_modifierLevelDoubleCoins = (float) JSONTools.ReadDouble(dict[ObjectiveType.ModifierLevelDoubleCoins]);
		}
		
		if (dict.ContainsKey(ObjectiveType.ModifierLevelMagician)) {
			_modifierLevelMagician = (float) JSONTools.ReadDouble(dict[ObjectiveType.ModifierLevelMagician]);
		}
		
		if (dict.ContainsKey(ObjectiveType.ModifierLevelEnchanter)) {
			_modifierLevelEnchanter = (float) JSONTools.ReadDouble(dict[ObjectiveType.ModifierLevelEnchanter]);
		}
		
		if (dict.ContainsKey(ObjectiveType.ModifierLevelBargainHunter)) {
			_modifierLevelBargainHunter = (float) JSONTools.ReadDouble(dict[ObjectiveType.ModifierLevelBargainHunter]);
		}
		
		if (dict.ContainsKey(ObjectiveType.ModifierLevelLuck)) {
			_modifierLevelLuck = (float) JSONTools.ReadDouble(dict[ObjectiveType.ModifierLevelLuck]);
		}
		
		if (dict.ContainsKey(ObjectiveType.ModifiersMaxed)) {
			_modifiersMaxed = (float) JSONTools.ReadDouble(dict[ObjectiveType.ModifiersMaxed]);
		}
		
		if (dict.ContainsKey(ObjectiveType.UseConsumableHeadStart)) {
			_useConsumableHeadStart = (float) JSONTools.ReadDouble(dict[ObjectiveType.UseConsumableHeadStart]);
		}
		
		if (dict.ContainsKey(ObjectiveType.UseConsumableMegaHeadStart)) {
			_useConsumableMegaHeadStart = (float) JSONTools.ReadDouble(dict[ObjectiveType.UseConsumableMegaHeadStart]);
		}
		
		if (dict.ContainsKey(ObjectiveType.UseConsumableStumbleProof)) {
			_useConsumableStumbleProof = (float) JSONTools.ReadDouble(dict[ObjectiveType.UseConsumableStumbleProof]);
		}
		
		if (dict.ContainsKey(ObjectiveType.UseConsumableThirdEye)) {
			_useConsumableThirdEye = (float) JSONTools.ReadDouble(dict[ObjectiveType.UseConsumableThirdEye]);
		}
		
		if (dict.ContainsKey(ObjectiveType.UseConsumableMultiplier)) {
			_useConsumableMultiplier = (float) JSONTools.ReadDouble(dict[ObjectiveType.UseConsumableMultiplier]);
		}
		
		if (dict.ContainsKey(ObjectiveType.PaidDestinyCard)) {
			_paidDestinyCard = (float) JSONTools.ReadDouble(dict[ObjectiveType.PaidDestinyCard]);
		}
		
		if (dict.ContainsKey(ObjectiveType.FastTraveled)) {
			_fastTraveled = (float) JSONTools.ReadDouble(dict[ObjectiveType.FastTraveled]);
		}
		
		if (dict.ContainsKey(ObjectiveType.UnlockArtifacts)) {
			_unlockArtifacts = (float) JSONTools.ReadDouble(dict[ObjectiveType.UnlockArtifacts]);
		}
		
		if (dict.ContainsKey(ObjectiveType.UseAllInGamePowerups)) {
			_useAllInGamePowerups = (float) JSONTools.ReadDouble(dict[ObjectiveType.UseAllInGamePowerups]);
		}
		
		if (dict.ContainsKey(ObjectiveType.UseEverything)) {
			_useEverything = (float) JSONTools.ReadDouble(dict[ObjectiveType.UseEverything]);
		}
	}
	
	public string ToJson() { return MiniJSON.Json.Serialize(this.ToDict()); }
	
	public float[] ToFloatArray()
	{
		float[] fl = new float[(int)ObjectiveType.LifetimeObjectivesCount];
		
		try {
			fl[(int)ObjectiveType.CollectCoins] = _collectCoins;
			fl[(int)ObjectiveType.Distance] = _distance;
			fl[(int)ObjectiveType.Score] = _score;
			fl[(int)ObjectiveType.CollectPowerups] = _collectPowerups;
			fl[(int)ObjectiveType.CollectSpecialCurrency] = _collectSpecialCurrency;
			fl[(int)ObjectiveType.Resurrects] = _resurrects;
			fl[(int)ObjectiveType.CoinMeterFills] = _coinMeterFills;
			fl[(int)ObjectiveType.ModifierLevelDoubleCoins] = _modifierLevelDoubleCoins;
			fl[(int)ObjectiveType.ModifierLevelMagician] = _modifierLevelMagician;
			fl[(int)ObjectiveType.ModifierLevelEnchanter] = _modifierLevelEnchanter;
			fl[(int)ObjectiveType.ModifierLevelBargainHunter] = _modifierLevelBargainHunter;
			fl[(int)ObjectiveType.ModifierLevelLuck] = _modifierLevelLuck;
			fl[(int)ObjectiveType.ModifiersMaxed] = _modifiersMaxed;
			fl[(int)ObjectiveType.UseConsumableHeadStart] = _useConsumableHeadStart;
			fl[(int)ObjectiveType.UseConsumableMegaHeadStart] = _useConsumableMegaHeadStart;
			fl[(int)ObjectiveType.UseConsumableStumbleProof] = _useConsumableStumbleProof;
			fl[(int)ObjectiveType.UseConsumableThirdEye] = _useConsumableThirdEye;
			fl[(int)ObjectiveType.UseConsumableMultiplier] = _useConsumableMultiplier;
			fl[(int)ObjectiveType.PaidDestinyCard] = _paidDestinyCard;
			fl[(int)ObjectiveType.UnlockArtifacts] = _unlockArtifacts;
			fl[(int)ObjectiveType.UseAllInGamePowerups] = _useAllInGamePowerups;
			fl[(int)ObjectiveType.UseEverything] = _useEverything;
		} catch (Exception ex) {
			notify.Warning("LifetimeStatEnvironmentProtoData - Unable to convert to float array " + ex);
		}
		
		return fl;
	}
	
	public Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> d = new Dictionary<string, object>();
		d.Add("cc", _collectCoins);
		d.Add("di", _distance);
		d.Add("sc", _score);
		d.Add("cp", _collectPowerups);
		d.Add("csc", _collectSpecialCurrency);
		d.Add("rez", _resurrects);
		d.Add("cmf", _coinMeterFills);
		d.Add("mdc", _modifierLevelDoubleCoins);
		d.Add("mm", _modifierLevelMagician);
		d.Add("me", _modifierLevelEnchanter);
		d.Add("mbh", _modifierLevelBargainHunter);
		d.Add("ml", _modifierLevelLuck);
		d.Add("mmx", _modifiersMaxed);
		d.Add("uch", _useConsumableHeadStart);
		d.Add("ucmh", _useConsumableMegaHeadStart);
		d.Add("ucsp", _useConsumableStumbleProof);
		d.Add("ucte", _useConsumableThirdEye);
		d.Add("ucm", _useConsumableMultiplier);
		d.Add("pdd", _paidDestinyCard);
		d.Add("ft", _fastTraveled);
		d.Add("ua", _unlockArtifacts);
		d.Add("uagp", _useAllInGamePowerups);
		d.Add("uAll", _useEverything);
		
		return d;
	}
}

