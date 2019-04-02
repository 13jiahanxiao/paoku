using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterCard : MonoBehaviour {
	protected static Notify notify;
	public enum DisplayItem
	{
		Icon,
		Background
	}
	
	public List<Transform> 	PowerSlot = new List<Transform>();
	public UIGrid			AbilitiesGrid = null;
	public UILabel			DisplayName = null;
	public int				CharacterID = -1;
	public UISlider			PowerGemSlider = null;
	
	public Transform		unlockButton = null;
	public UILabel			unlockPriceLabel = null;
	public UISprite			unlockPriceIcon = null;
	
	// Use this for initialization
	void Awake () {
		notify = new Notify(this.GetType().Name);
	}
	
	public void updateUI(CharacterStats characterStat) {
		CharacterID = characterStat.characterId;
		
		//-- Update coins
		if(AbilitiesGrid != null) {
			notify.Debug ("repositioning ablilities Grid for {0}", characterStat.displayName);
			AbilitiesGrid.repositionNow = true;
		}
		
		if(DisplayName != null) {
			DisplayName.text = characterStat.displayName;
		}
		
		//-- Update Wardrobe
		
		//-- Update SpecialPower
		updatePowerDisplay(characterStat);
		
		if(unlockButton != null) {
			NGUITools.SetActive(unlockButton.gameObject, !characterStat.unlocked);
			if(unlockPriceLabel != null && characterStat.unlocked == false) {
				unlockPriceLabel.text = characterStat.unlockCost.ToString();
			}
		}
		
		if(AbilitiesGrid != null) {
			NGUITools.SetActive(AbilitiesGrid.gameObject, characterStat.unlocked);	
		}
	}
	
	private void updatePowerDisplay(CharacterStats characterStat) {
		BasePower power = null;
		
		if(PowerStore.Powers != null && characterStat.powerID >=0) {
			power = PowerStore.PowerFromID(characterStat.powerID);	
		}
		
		if(PowerSlot[(int)DisplayItem.Background] != null) {
			NGUITools.SetActive(PowerSlot[(int)DisplayItem.Background].gameObject, characterStat.unlocked);
		}
		
		if(PowerSlot[(int)DisplayItem.Icon] != null) {
			if(characterStat.unlocked == true) { 
				UISprite icon = PowerSlot[(int)DisplayItem.Icon].GetComponent<UISprite>() as UISprite;
				if(icon != null) {
					if(power != null) {
						icon.spriteName = power.IconName;
					}
					else{
						icon.spriteName = "coin";	
					}
					icon.MakePixelPerfect();
				}	
			}
		}
		
		if(PowerGemSlider != null) {
			NGUITools.SetActive(PowerGemSlider.gameObject, false);
			if(power != null) {
				float progress = GameProfile.SharedInstance.Player.GetBuffProgress(BuffType.Powerup, characterStat.powerID, power.ProtoBuff);
				if(progress > Mathf.Epsilon && characterStat.unlocked) {
					NGUITools.SetActive(PowerGemSlider.gameObject, true);
					PowerGemSlider.sliderValue = progress;
				}
			}
		}
	}
}
