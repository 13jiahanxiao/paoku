/*
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIInventoryViewController : UIViewController
{

	public UICharacterSelectViewController characterSelectVC = null;
	public ArtifactSlotType equipInSlot = ArtifactSlotType.Total;
	public Transform 	artifactPanel = null;
	public Transform 	powerupPanel = null;
	public UIGrid		artifactGrid = null;
	public UIGrid		powerupGrid = null;
	
	public Transform	equioeedCellRoot = null;
	public UILabel		equippedCellGroupTitle = null;
	public UILabel		equippedCellTitle = null;
	public UILabel		equippedCellDesc = null;
	public UILabel		equippedCellBuff = null;
	public UILabel		equippedCellBuffCost = null;
	public UISprite		equippedCellIcon = null;
	public Transform	equippedCellEquipButton = null;
	public UISlider		equippedCellProgressbar= null;
	
	public Transform	protoInventoryCell = null;
	bool				currentFilterIsPowerups = false;
	
	
	int			equippedPower = -1;
	
	public override void Awake()
	{
		base.Awake();
	}
	
	public void ShowArtifacts() {
		currentFilterIsPowerups = false;
	}
	public void ShowPowerups() {
		currentFilterIsPowerups = true;
	}
	
	public void SetEquippedPower(int powerID) {
		equippedPower = powerID;
		updateEquippedCell("Pick a Powerup");
	}
	
	public override void appear() {
		
		base.appear();
		updateEquippedCell();
		if(paperViewController != null) {
			paperViewController.ShowBackButton(false);
		}
		NGUITools.SetActive(powerupPanel.gameObject, currentFilterIsPowerups);
		NGUITools.SetActive(artifactPanel.gameObject, !currentFilterIsPowerups);
		NGUITools.SetActive(equioeedCellRoot.gameObject, currentFilterIsPowerups);
		if(currentFilterIsPowerups == true) {
			updatePowerups();
		}
		else {
			updateArtifacts();
		}
		
	}
	
	public override void OnBackButton() {
		base.OnBackButton();
	}
	

	public void SizeForScreen() {
		//-- ONLY CALL THIS FOR 1136 for now.
//		powerupPanel.transform.localPosition = new Vector3(powerupPanel.transform.localPosition.x, powerupPanel.transform.localPosition.y-85, powerupPanel.transform.localPosition.z);
//		artifactPanel.transform.localPosition = new Vector3(artifactPanel.transform.localPosition.x, artifactPanel.transform.localPosition.y-85, artifactPanel.transform.localPosition.z);
		
		UIPanel artP = artifactPanel.GetComponent<UIPanel>() as UIPanel;
		Vector4 temp = Vector4.zero;
		if(artP != null) {
			temp = artP.clipRange;
			temp.w = 930;
			artP.clipRange = temp;
		}
		UIPanel powerP = powerupPanel.GetComponent<UIPanel>() as UIPanel;
		if(powerP != null) {
			temp = powerP.clipRange;
			temp.w = 800;
			powerP.clipRange = temp;
		}
		
		UIDraggablePanel dp = artifactPanel.GetComponent<UIDraggablePanel>() as UIDraggablePanel;
		if(dp) {
			dp.repositionClipping = true;
		}
		dp = powerupPanel.GetComponent<UIDraggablePanel>() as UIDraggablePanel;
		if(dp) {
			dp.repositionClipping = true;
		}
	}
		
	void updateArifactCellData(CellData cellData, PlayerStats player, CharacterStats activeCharacter) {
		if(cellData == null)
			return;
		
		GameObject newCell = cellData.gameObject;
		int artifactID = cellData.Data;
			
		ArtifactProtoData protoData = ArtifactStore.GetArtifactProtoData(artifactID);
		if(protoData == null) {
			notify.Error("protoData should NOT be null.  ID={0}", artifactID);
			return;
		}
		
		bool purchased = player.IsArtifactPurchased(artifactID);
		if(player.IsArtifactDiscovered(artifactID, false, protoData) == false) {
			updateCellData(newCell, "Undiscovered", "powerup_question_mark", "", "", protoData._costType, false, false, false, 0.0f);
		}
		else {
			//-- Figure out progress.
			List<int> progression = null;	//ArtifactStore.GetProgressionFromArtifact(artifactID);
			int jeff = 36;
			if(artifactID == jeff) {
				artifactID = jeff;
			}
			float progress = 1.0f;
			if(purchased == false && progression != null) {
				int max = progression.Count;
				for (int i = 0; i < max; i++) {
					if(progression[i] != artifactID)
						continue;
					progress = ((float)i) / (float)max;
					break;
				}	
			}
			else if(progression == null) {
				progress = -1.0f;
			}
			updateCellData(newCell, protoData._title, protoData._iconName, protoData._description, protoData._cost.ToString(), protoData._costType, purchased, false, true, progress);	
		}
	}
	
	void updateCellData(GameObject newCell, string title, string iconName, string description, string coinValue, 
		CostType costType, bool purchased, bool equipped, bool discovered, float progress = -1.0f) {
		
		GameObject go = HierarchyUtils.GetChildByName("Title", newCell);
		if(go != null) {
			UILabel titleLabel = go.GetComponent<UILabel>() as UILabel;
			if(titleLabel != null) {
				titleLabel.text = title;	
				//titleLabel.color = ArtifactStore.colorForRarity(protoData._rarity);
				titleLabel.color = Color.white;
			}
		}
		
		go = HierarchyUtils.GetChildByName("Icon", newCell);
		if(go != null) {
			UISprite iconSprite = go.GetComponent<UISprite>() as UISprite;
			if(iconSprite != null) {
				iconSprite.spriteName = iconName;
				if(discovered) {
					iconSprite.color = Color.white;
				}
				else {
					iconSprite.color = new Color(0.75f, 0.75f, 0.75f, 1.0f);
				}
				iconSprite.MakePixelPerfect();
			}
		}
		
		go = HierarchyUtils.GetChildByName("IconBackground", newCell);
		if(go != null) {
			UISprite background = go.GetComponent<UISprite>() as UISprite;
			if(background != null) {
				//background.color = backgroundColor;
				background.color = new Color(250.0f/255.0f, 220.0f/255.0f, 125.0f/255.0f);
			}
		}
		
		go = HierarchyUtils.GetChildByName("Description", newCell);
		if(go != null) {
			UILabel desc = go.GetComponent<UILabel>() as UILabel;
			if(desc != null) {
				if(purchased == true && progress > Mathf.Epsilon && currentFilterIsPowerups == false) {
					desc.text = "Fully Upgraded!";
				} else {
					desc.text = description;
				}
				
			}
		}
		
		GameObject CoinDisplayIcon = HierarchyUtils.GetChildByName("CoinDisplayIcon", newCell);
		if(CoinDisplayIcon != null) {
			UISprite costIcon = CoinDisplayIcon.GetComponent<UISprite>() as UISprite;
			NGUITools.SetActive(costIcon.gameObject, discovered);
			if(costIcon != null) {
				if(costType == CostType.Coin) {
					costIcon.spriteName = "coin";	
				}
				else {
					costIcon.spriteName = "gem";	
				}
			}
		}
		
		go = HierarchyUtils.GetChildByName("Cost", newCell);
		if(go != null) {
			GameObject buyButton = HierarchyUtils.GetChildByName("BuyButton", newCell);
			NGUITools.SetActive(buyButton, discovered);
			//GameObject CoinDisplayIcon = HierarchyUtils.GetChildByName("CoinDisplayIcon", newCell);
			UILabel cost = go.GetComponent<UILabel>() as UILabel;
			if(cost != null) {
				if(purchased == true) {
					if(currentFilterIsPowerups == true) {
						if(equipped == true) {
							cost.text = "IN USE";
							if(buyButton != null) {
								NGUITools.SetActive(buyButton, false);
							}
						}
						else {
							cost.text = "USE";
							if(buyButton != null) {
								NGUITools.SetActive(buyButton, true);
							}
						}	
					}
					else {
						if(buyButton != null) {
							NGUITools.SetActive(buyButton, false);
						}	
					}
					
					if(CoinDisplayIcon != null) {
						NGUITools.SetActive(CoinDisplayIcon, false);
					}
				}
				else {
					if(buyButton != null) {
						NGUITools.SetActive(buyButton, discovered);
					}
					cost.text = coinValue;
					
					if(CoinDisplayIcon != null) {
						NGUITools.SetActive(CoinDisplayIcon, discovered);
					}
				}
					
			}
		}
		
		go = HierarchyUtils.GetChildByName("Progress Bar", newCell);
		if(go != null) {
			if(progress < -Mathf.Epsilon || currentFilterIsPowerups == true) {
				NGUITools.SetActive(go, false);	
			}
			else {
				NGUITools.SetActive(go, true);
				UISlider slider = go.GetComponent<UISlider>() as UISlider;
				if(slider) {
					slider.sliderValue = progress;
				}
			}
		}
	}
	
	void updatePowerCellData(CellData cellData, PlayerStats player, CharacterStats activeCharacter) {
		if(cellData == null)
			return;
		
		GameObject newCell = cellData.gameObject;
			
		BasePower power = PowerStore.PowerFromID(cellData.Data);
		bool purchased = player.IsPowerPurchased(cellData.Data);
		bool equipped = (activeCharacter.powerID == cellData.Data);
		//Color backgroundColor = new Color(250.0f/255.0f, 220.0f/255.0f, 125.0f/255.0f);
		updateCellData(newCell, power.Title, power.IconName, power.Description, power.Cost.ToString(), power.CostType, purchased, equipped, true);
	}
	
	void updateArtifacts() {
		
		PlayerStats player = GameProfile.SharedInstance.Player;
		CharacterStats activeCharacter = GameProfile.SharedInstance.GetActiveCharacter();
		
		CellData[] cells = artifactGrid.GetComponentsInChildren<CellData>(true) as CellData[];
		foreach (CellData item in cells) {
			if(item == null)
				continue;
			if(player.IsArtifactPurchased(item.Data) == true) {
				List<int> prog = null;	//ArtifactStore.GetProgressionFromArtifact(item.Data);
				
				if(prog != null) {
					int max = prog.Count;
					bool found = false;
					for (int i = 0; i < max; i++) {
						if(prog[i] != item.Data && found == false)
							continue;
						found = true;
						if(player.IsArtifactPurchased(prog[i]))
							continue;
						item.Data = prog[i];
						break;
					}
				}
			}
			
			updateArifactCellData(item, player, activeCharacter);
		}
	}
	
	void updatePowerups() {
		
		
		PlayerStats player = GameProfile.SharedInstance.Player;
		CharacterStats activeCharacter = GameProfile.SharedInstance.GetActiveCharacter();
		
		CellData[] cells = powerupGrid.GetComponentsInChildren<CellData>(true) as CellData[];
		foreach (CellData item in cells) {
			if(item == null)
				continue;
			updatePowerCellData(item, player, activeCharacter);
		}
	}
	
	
	
	public void OnArtifactCellPressed(GameObject cell) {
		//-- Can we equip this?
		bool closeDialog = false;
		if(cell != null && equipInSlot != ArtifactSlotType.Total) { 
			//-- equip it
			
			CellData cellData = cell.GetComponent<CellData>() as CellData;
			if(cellData != null) {
				int artifactID = cellData.Data;
				CharacterStats activeCharacter = GameProfile.SharedInstance.GetActiveCharacter();
				PlayerStats playerStats = GameProfile.SharedInstance.Player;
				ArtifactProtoData artifact = ArtifactStore.GetArtifactProtoData(artifactID);
				
				if(GameProfile.SharedInstance.Player.IsArtifactDiscovered(artifactID, false, artifact) == false) {
					//-- Show error dialog
					UIConfirmDialog.onPositiveResponse += OnCantEquip;
					UIManager.SharedInstance.confirmDialog.ShowInfoDialog("Undiscovered!", "That Artifact has not been discovered.", "Btn_Ok");
					return;
				}
				else if(playerStats.IsArtifactPurchased(artifactID) == false) {
					//-- Can we afford it?
					if(playerStats.CanAffordArtifact(artifactID, artifact) == false) {
						if(artifact._costType == CostType.Coin) {
							UIConfirmDialog.onNegativeResponse += OnNeedMoreCoinsNo;
							UIConfirmDialog.onPositiveResponse += OnNeedMoreCoinsYes;	
							UIManager.SharedInstance.confirmDialog.ShowConfirmDialog("Lbl_Dialogue_MoreGems_Prompt","Lbl_Dialogue_MoreCoins_Confirm", "Btn_No", "Btn_Yes");
						}
						else {
							UIConfirmDialog.onNegativeResponse += OnNeedMoreGemsNo;
							UIConfirmDialog.onPositiveResponse += OnNeedMoreGemsYes;
							UIManager.SharedInstance.confirmDialog.ShowConfirmDialog("Lbl_Dialogue_MoreGems_Prompt","Lbl_Dialogue_MoreGems_Confirm", "Btn_No", "Btn_Yes");
						}
						return;
					}
					
					//-- Buy it if we can afford it.
					playerStats.PurchaseArtifact(artifactID);
					List<int> progression = null;	//ArtifactStore.GetProgressionFromArtifact(artifactID);
					if(progression != null)
					{
						//-- get next id if we are in a progression.
						int max = progression.Count;
						for (int i = 0; i < (max-1); i++) {
							if(progression[i] != artifactID)
								continue;
							cellData.Data = progression[i+1];
							break;
						}
					}	
					updateArifactCellData(cellData, playerStats, activeCharacter);
					updateCurrency();
				}
//				else {
//					//-- Can't equip this because its already equipped.
//					//-- Show error dialog
//					UIConfirmDialog.onPositiveResponse += OnCantEquip;
//					UIManager.SharedInstance.confirmDialog.ShowInfoDialog("Oops!", "That Artifact is already in use.", "Ok");
//					return;
//				}	
			}
		}	
		
		if(closeDialog == true) {
			OnBackButton();	
		}
	}
	
	public void OnCantEquip() {
		UIConfirmDialog.onPositiveResponse -= OnCantEquip;
	}
	
	public void OnGemItemPressed() {
		int itemID = -1;
		Buff newBuff = null;
		
		itemID = equippedPower;
		if(itemID == -1)
			return;
		
		BasePower basePower = PowerStore.PowerFromID(itemID);
		if(basePower == null)
			return;
		newBuff = new Buff(basePower.ProtoBuff.ToDict());
		newBuff.itemID = itemID;
		
		int gemcost = GameProfile.SharedInstance.Player.GetBuffCost(BuffType.Powerup, itemID, newBuff);
		if(gemcost < 0)
			gemcost = 1;
		
		if(GameProfile.SharedInstance.Player.specialCurrencyCount < gemcost) {
			UIConfirmDialog.onNegativeResponse += OnNeedMoreGemsNo;
			UIConfirmDialog.onPositiveResponse += OnNeedMoreGemsYes;
			UIManager.SharedInstance.confirmDialog.ShowConfirmDialog("Lbl_Dialogue_MoreGems_Prompt","Lbl_Dialogue_MoreGems_Confirm", "Btn_No", "Btn_Yes");
			return;
		}
		
		//-- Augment the power.
		if(GameProfile.SharedInstance.Player.CreateBuff(BuffType.Powerup, itemID, basePower.ProtoBuff) == true) {
			GameProfile.SharedInstance.Player.specialCurrencyCount -= gemcost;
			if(GameProfile.SharedInstance.Player.specialCurrencyCount < 0)
				GameProfile.SharedInstance.Player.specialCurrencyCount = 0;	
			
			GameProfile.SharedInstance.Serialize();
		}
		
		updateEquippedCell(null);
		updateCurrency();
	}
	
	public void OnPowerItemPressed(GameObject cell) {
		//-- Can we equip this?
		bool closeDialog = false;
		if(cell != null) { 
			//-- equip it
			
			CellData cellData = cell.GetComponent<CellData>() as CellData;
			if(cellData != null) {
				int powerID = cellData.Data;
				CharacterStats activeCharacter = GameProfile.SharedInstance.GetActiveCharacter();
				PlayerStats playerStats = GameProfile.SharedInstance.Player;
				BasePower bp = PowerStore.PowerFromID(powerID);
				//-- If locked {
				if(playerStats.IsPowerPurchased(powerID) == false) {
					//-- Can we afford it?
					if(playerStats.CanAffordPower(powerID) == false) {
						if(bp.CostType == CostType.Coin) {
							UIConfirmDialog.onNegativeResponse += OnNeedMoreCoinsNo;
							UIConfirmDialog.onPositiveResponse += OnNeedMoreCoinsYes;	
							UIManager.SharedInstance.confirmDialog.ShowConfirmDialog("Lbl_Dialogue_MoreCoins_Prompt","Lbl_Dialogue_MoreCoins_Confirm", "Btn_No", "Btn_Yes");
						}
						else {
							UIConfirmDialog.onNegativeResponse += OnNeedMoreGemsNo;
							UIConfirmDialog.onPositiveResponse += OnNeedMoreGemsYes;
							UIManager.SharedInstance.confirmDialog.ShowConfirmDialog("Lbl_Dialogue_MoreGems_Prompt","Lbl_Dialogue_MoreGems_Confirm", "Btn_No", "Btn_Yes");
						}
						return;
					}
				
					//-- Buy it if we can afford it.
					playerStats.PurchasePower(powerID);
					updatePowerCellData(cellData, playerStats, activeCharacter);
					updateCurrency();
				}
				else if(GameProfile.SharedInstance.IsPowerEquipped(powerID, activeCharacter.characterId) == false) {
					activeCharacter.powerID = powerID;
					if(characterSelectVC != null) {
						characterSelectVC.updateCharacterCard(activeCharacter);	
					}
						
					equippedPower = powerID;
					updateEquippedCell();
					GameProfile.SharedInstance.Serialize();
					updatePowerups();
				}
				else {
					//-- Can't equip this because its already equipped.
					//-- Show error dialog
					UIConfirmDialog.onPositiveResponse += OnCantEquip;
					UIManager.SharedInstance.confirmDialog.ShowInfoDialog("Oops!", "That PowerUp is already in use.", "Btn_Ok");
					return;
				}	
			}
		}	
		
		if(closeDialog == true) {
			OnBackButton();	
		}
	}
	
	void updateEquippedCell(string optionalTitle = null) {
		if(currentFilterIsPowerups == false)
			return;
		
		if(equippedPower != -1) {
			if(equippedPower >= PowerStore.Powers.Count) 
				return;
			
			BasePower power = PowerStore.PowerFromID(equippedPower);
			if(power == null)
				return;
			
			if(equippedCellTitle) {
				equippedCellTitle.text = power.Title;
			}
			if(equippedCellDesc) {
				equippedCellDesc.text = power.Description;
			}
			
			if(equippedCellBuff) {
				NGUITools.SetActive(equippedCellBuff.gameObject, true);
				equippedCellBuff.text = power.BuffDescription;
			}
			
			if(equippedCellBuffCost) {
				equippedCellBuffCost.text = GameProfile.SharedInstance.Player.GetBuffCost(BuffType.Powerup, equippedPower, power.ProtoBuff).ToString();
			}
			
			if(equippedCellProgressbar) {
				NGUITools.SetActive(equippedCellProgressbar.gameObject, true);
				equippedCellProgressbar.sliderValue = GameProfile.SharedInstance.Player.GetBuffProgress(BuffType.Powerup, equippedPower, power.ProtoBuff);
			}
			
			if(equippedCellIcon) {
				equippedCellIcon.spriteName = power.IconName;
				equippedCellIcon.MakePixelPerfect();
			}
			
			if(equippedCellEquipButton) {
				NGUITools.SetActive(equippedCellEquipButton.gameObject, true);
			}
			
			
			//updatePowerups();
		}
		else {
			//-- Nothing equipped.
			if(equippedCellTitle) {
				if(optionalTitle != null) {
					equippedCellTitle.text = optionalTitle;
				}
			}
			if(equippedCellDesc) {
				equippedCellDesc.text = "";
			}
			
			if(equippedCellBuff) {
				NGUITools.SetActive(equippedCellBuff.gameObject, false);
			}
			
			if(equippedCellIcon) {
				equippedCellIcon.spriteName = "unknown";
				equippedCellIcon.MakePixelPerfect();
			}
			
			if(equippedCellProgressbar) {
				NGUITools.SetActive(equippedCellProgressbar.gameObject, false);
			}
		}
	}
}

 */