//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//
//public class MegaHeadStartConsumable : BaseConsumable
//{
//	public MegaHeadStartConsumable()
//	{
//		Type = "MegaHeadStartConsumable";	
//	}
//	
//	public override void Activate() 
//	{
//		base.Activate();
//		
//		GamePlayer.SharedInstance.StartBoost();
//		GamePlayer.SharedInstance.BoostDistanceLeft += Value;
//		GamePlayer.SharedInstance.HasSuperBoost = true;
//	}
//	
//	public override void Deactivate() { base.Deactivate(); }
//	
//	public override bool Update() 
//	{
//		//if(GamePlayer.SharedInstance == null) { return false; }
//		//if(GamePlayer.SharedInstance.IsDead == true || GamePlayer.SharedInstance.HasBoost == false) 
//		//{
//		//	Deactivate();
//		//	return false;
//		//}
//		return true;
//	}
//}
