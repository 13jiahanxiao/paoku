/*
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UICharacterSelectViewController : UIViewController
{
	public static event voidClickedHandler 	onPlayClickedHandler = null;
	
	public UIViewController inGameVC = null;
	public UICenterOnChild	centeredCharacter = null;
	public UIInventoryViewController inventoryVC = null;
	
	private List<Transform> CharacterCards = new List<Transform>();	
	public override void viewDidLoad() {
		//-- Cache stuff here.
		CharacterCards.Clear();
		CharacterCard[] cards = GetComponentsInChildren<CharacterCard>();
		foreach (CharacterCard item in cards) {
			CharacterCards.Add (item.transform);
		}
	}
	
	public override void appear() {
		
		base.appear();
		
		if(paperViewController != null) {
			paperViewController.SetPlayButtonCallback(this.gameObject, "OnPlayClicked");
		}
		
		foreach(CharacterStats character in GameProfile.SharedInstance.Characters) {
			updateCharacterCard(character);
		}
		
		RecenterOnSelectedCharacter();
	}
	
	public override void OnBackButton() {
		setActiveCharacter();
		base.OnBackButton();
	}
	
	private bool setActiveCharacter() {
		PlayerStats player = GameProfile.SharedInstance.Player;
		
		//-- Choose the current hero if its unlocked.
		CharacterStats selectedHero = getSelectedHero();
		if(selectedHero != null && selectedHero.unlocked == true) {
			player.activePlayerCharacter = selectedHero.characterId;
			return true;
		}
		return false;
	}
	
	public void OnPlayClicked() {
		
		if(setActiveCharacter() == true) {
			if(GameController.SharedInstance != null && GameController.SharedInstance.Player != null) {
				GameController.SharedInstance.Player.doSetupCharacter();	
			}
		} else {
			//-- If we can't set teh active player, prevent PLAYING.
			return;
		}
		
		if(MainGameCamera != null) {
			MainGameCamera.enabled = true;
			if(GamePlayer.SharedInstance != null && GamePlayer.SharedInstance.ShadowCamera != null && QualitySettings.GetQualityLevel() >= 1) {
				GamePlayer.SharedInstance.ShadowCamera.enabled = true;	
			}
		}
		
		disappear(true);
		
		//if(inGameVC != null) {
		//	inGameVC.appear();
			//ShowObject(activePowerIcon.gameObject, false, false);
		//}
		
		//-- Notify an object that is listening for this event.
		if(onPlayClickedHandler != null) {
			onPlayClickedHandler();
		}	
	}
	
	private CharacterCard findCardFromCardList(int characterID) {
		if(CharacterCards == null || characterID < 0 || characterID >= CharacterCards.Count)
			return null;
		int max = CharacterCards.Count;
		for (int i = 0; i < max; i++) {
			Transform cardXform = CharacterCards[i];
			if(cardXform == null)
				continue;
			CharacterCard card = cardXform.GetComponent<CharacterCard>() as CharacterCard;
			if(card == null)
				continue;
			if(card.CharacterID == characterID)
				return card;
		}
		return null;
	}
	
	public void updateCharacterCard(CharacterStats characterStat) {
		
		CharacterCard card = findCardFromCardList(characterStat.characterId);
		if(card == null)
			return;
		
		card.updateUI(characterStat);
	}
	
	public void Update() {
		CharacterStats selectedHero = getSelectedHero();
		if(selectedHero != null) {
			paperViewController.ShowPlayButton(selectedHero.unlocked);
		}
	}
	
	private CharacterStats getSelectedHero() {
		CharacterCard characterCard = null;
		if(centeredCharacter != null && centeredCharacter.centeredObject != null && paperViewController != null) {
			characterCard = centeredCharacter.centeredObject.GetComponent<CharacterCard>() as CharacterCard;
			if( characterCard != null && 
				characterCard.CharacterID >=0 && 
				characterCard.CharacterID < GameProfile.SharedInstance.Characters.Count) 
			{
				//return GameProfile.SharedInstance.Characters[characterCard.CharacterID];
				return GameProfile.SharedInstance.Characters[0];
			}
		}
		return null;
	}
	
	public void OnUnlockCharacter()
	{
		//-- Get the selectedHero.
		CharacterStats selectedHero = getSelectedHero();
		if(selectedHero.unlocked == true) {
			return;
		}
		
		//-- TODO: Propmpt to buy, can afford
		if(GameProfile.SharedInstance.Player.CanAffordHero(selectedHero.characterId) == false) {
			UIConfirmDialog.onNegativeResponse += OnNeedMoreCoinsNo;
			UIConfirmDialog.onPositiveResponse += OnNeedMoreCoinsYes;
			UIManager.SharedInstance.confirmDialog.ShowConfirmDialog("Lbl_Dialogue_MoreCoins_Prompt","Lbl_Dialogue_MoreCoins_Confirm", "Btn_No", "Btn_Yes");
			return;
		}
		BuyCharacter(selectedHero);
	}
	
	private void BuyCharacter(CharacterStats hero) {
		PlayerStats player = GameProfile.SharedInstance.Player;
		if(player == null || hero == null)
			return;
		player.PurchaseHero(hero.characterId);
		if(player.IsHeroPurchased(hero.characterId) == false)
			return;
		
		updateCurrency();
		updateCharacterCard(hero);
	}
	
	UIDraggablePanel mDrag;
	private void RecenterOnSelectedCharacter()
	{
		if (mDrag == null && CharacterCards != null && CharacterCards.Count >= 1)
		{
			mDrag = NGUITools.FindInParents<UIDraggablePanel>(CharacterCards[0].gameObject);

			if (mDrag == null)
			{
				return;
			}
		}
		if (mDrag == null || mDrag.panel == null) 
			return;
		
		PlayerStats player = GameProfile.SharedInstance.Player;
		if(player == null || CharacterCards == null) 
			return;
		
		CharacterCard card = findCardFromCardList(player.activePlayerCharacter);
		if(card == null)
			return;
		
		Transform closest = card.transform;
		if(closest == null) 
			return;
		
		// Calculate the panel's center in world coordinates
		Vector4 clip = mDrag.panel.clipRange;
		Transform dt = mDrag.panel.cachedTransform;
		Vector3 center = dt.localPosition;
		center.x += clip.x;
		center.y += clip.y;
		center = dt.parent.TransformPoint(center);
		mDrag.currentMomentum = Vector3.zero;
		
		if (closest != null)
		{
			// Figure out the difference between the chosen child and the panel's center in local coordinates
			Vector3 cp = dt.InverseTransformPoint(closest.position);
			Vector3 cc = dt.InverseTransformPoint(center);
			Vector3 offset = cp - cc;

			// Offset shouldn't occur if blocked by a zeroed-out scale
			if (mDrag.scale.x == 0f) offset.x = 0f;
			if (mDrag.scale.y == 0f) offset.y = 0f;
			if (mDrag.scale.z == 0f) offset.z = 0f;

			// Spring the panel to this calculated position
			//mDrag.MoveRelative(offset);
			SpringPanel.Begin(mDrag.gameObject, dt.localPosition - offset, 8f);
		}
	}
	
	public void OnCharacterEquipSlotOnePressed(GameObject sender) {
		characterEquipSlotPressed(sender, ArtifactSlotType.One);
	}
	public void OnCharacterEquipSlotTwoPressed(GameObject sender) {
		characterEquipSlotPressed(sender, ArtifactSlotType.Two);
	}
	public void OnCharacterEquipSlotThreePressed(GameObject sender) {
		characterEquipSlotPressed(sender, ArtifactSlotType.Three);
	}
	
	private void characterEquipSlotPressed(GameObject sender, ArtifactSlotType slotIndex) {
		setActiveCharacter();
		
		if(inventoryVC == null){
			return;
		}
		
		inventoryVC.equipInSlot = slotIndex;
		
		disappear(false);
		inventoryVC.titleText = "Abilities";
		inventoryVC.ShowArtifacts();
		inventoryVC.appear();
	}
	
	public void OnSpecialPowerEquipClicked() {
		setActiveCharacter();
		if(inventoryVC == null){
			return;
		}
		
		disappear(false);
		CharacterStats activeCharacter = GameProfile.SharedInstance.GetActiveCharacter();
		inventoryVC.titleText = "Powerups";
		inventoryVC.equippedCellGroupTitle.text = "Powerups";
		inventoryVC.SetEquippedPower(activeCharacter.powerID);
		inventoryVC.ShowPowerups();
		inventoryVC.appear();
	}
}

*/