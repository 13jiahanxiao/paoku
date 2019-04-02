/*
using UnityEngine;
using System.Collections;

public class UIMainMenuViewController : UIViewController
{
	public UIViewController 		characterSelectVC = null;
	public UIViewController 		freeStoreVC = null;
	public UIViewController 		inGameVC = null;
	public UIIAPViewController		IAPStoreVC = null;
	public UIGIftsViewController	GiftsVC = null;
	public UIObjectivesViewController ObjectivesVC = null;
	public UIMoreGamesViewController moreGamesVC = null;
	public UISettingsViewController settingsVC = null;
	public UIStatViewController		statsVC = null;
	public UILeaderboardViewController leaderboardVC = null;
	public UILabel			currentCharacterLabel = null;
	public UISprite			currentCharacterSprite = null;
	
	public void OnCharacterSelectClicked() {
		
		//-- show characters.
		if(characterSelectVC != null) {
			disappear(false);
			characterSelectVC.previousViewController = this;
			characterSelectVC.appear();
		}
	}
	
	public override void OnBackButton() {
		
		//-- Save our current and back for navigation.
		if( previousViewController != null) 
		{
			
			disappear(previousViewController == UIManager.SharedInstance.idolMenuVC);
			previousViewController.appear();
		}
	}
	
	
	
	public override void appear() {
		
		//-- Make sure out current char portrait and name are set.
		CharacterStats currentCharacter = GameProfile.SharedInstance.GetActiveCharacter();
		if(currentCharacter != null) {
			if(currentCharacterLabel != null) {
				currentCharacterLabel.text = currentCharacter.displayName;
			}
			if(currentCharacterSprite != null) {
				GameProfile.ProtoCharacterVisual protoVisual = GameProfile.SharedInstance.ProtoCharacterVisuals[currentCharacter.protoVisualIndex];
				if(protoVisual != null && protoVisual.portraitSpriteName != null) {
					currentCharacterSprite.spriteName = protoVisual.portraitSpriteName;
				}
			}
		}
		
		if(paperViewController != null) {
			paperViewController.SetPlayButtonCallback(this.gameObject, "OnPlayClicked");
		}
		base.appear();
	}
	
	public static event voidClickedHandler 	onPlayClickedHandler = null;
	public void OnPlayClicked() {
		
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
	
	public void OnIAPStore() {
		if(IAPStoreVC != null) {
			disappear(false);
			
			IAPStoreVC.previousViewController = this;
			IAPStoreVC.appear();
		}
	}
	
	public void OnFreeStore() {
		if(freeStoreVC != null) {
			disappear(false);
			
			freeStoreVC.previousViewController = this;
			freeStoreVC.appear();
		}
	}
	
	public void OnGiftStore() {
		if(GiftsVC != null) {
			disappear(false);
			
			GiftsVC.previousViewController = this;
			GiftsVC.appear();
		}
	}
	
	public void OnObjectives() {
		if(ObjectivesVC != null) {
			disappear(false);
			
			ObjectivesVC.previousViewController = this;
			ObjectivesVC.appear();
		}
	}
	
	public void OnMoreGames() {
		if(moreGamesVC != null) {
			disappear(false);
			
			moreGamesVC.previousViewController = this;
			moreGamesVC.appear();
		}
	}
	
	public void OnSettings() {
		if(settingsVC != null) {
			disappear(false);
			
			settingsVC.previousViewController = this;
			settingsVC.appear();
		}
	}
	
	public void OnShowStats() {
		if(statsVC != null) {
			disappear(false);
			
			statsVC.previousViewController = this;
			statsVC.appear();
		}
	}
	
	public void OnShowLeaderboards() {
		if( Application.platform == RuntimePlatform.IPhonePlayer ) {
#if UNITY_IPHONE			
			GameCenterBinding.showLeaderboardWithTimeScope(GameCenterLeaderboardTimeScope.AllTime);
#endif
		}
		else {
			if(leaderboardVC != null) {
				disappear(false);
				
				leaderboardVC.previousViewController = this;
				leaderboardVC.appear();
			}	
		}
	}
}

 */