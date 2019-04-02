using UnityEngine;
using System.Collections;

public class NoStumbleConsumable : BaseConsumable
{
	
	public NoStumbleConsumable()
	{
		Type = "NoStumbleConsumable";	
	}
	
	public override void Activate()
	{
		base.Activate();
		GamePlayer.SharedInstance.NoStumble = true;
		//GamePlayer.SharedInstance.NoStumbleDistLeft = Value;
		GamePlayer.SharedInstance.playerFx.StartStumbleProof();
		AudioManager.SharedInstance.StartStumbleProof();
		
		ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.UseConsumableStumbleProof,1);
	}
}
