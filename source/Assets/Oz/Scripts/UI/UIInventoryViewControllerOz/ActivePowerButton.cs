using UnityEngine;
using System.Collections;

public class ActivePowerButton : MonoBehaviour 			// change powerup icon in corner
{	
	void Start() {}
	
	public void RefreshIcon()
	{
		//int id = GameProfile.SharedInstance.GetActiveCharacter().powerID;
		//if(id>=0 && id<PowerStore.Powers.Count)
		//{
			//BasePower data = PowerStore.Powers[GameProfile.SharedInstance.GetActiveCharacter().powerID];
			BasePower data = PowerStore.PowerFromID(GameProfile.SharedInstance.GetActiveCharacter().powerID);
			gameObject.transform.Find("activePowerIcon").GetComponent<UISprite>().spriteName = data.IconName + "_noborder";
		//}
	}
	
	void Update() {}
}
