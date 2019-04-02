/*

using UnityEngine;
using System.Collections;

public class UISettingsViewController : UIViewController
{
	public UISlider musicSlider = null;
	public UISlider soundSlider = null;
	public UISlider showTutorialSlider = null;
	public UISlider showFriendMarkerSlider = null;
	
	public override void appear ()
	{
		base.appear ();
		if(musicSlider != null) {
			//musicSlider.sliderValue = GameProfile.SharedInstance.MusicVolume;
			//AudioManager.SharedInstance.MusicVolume = GameProfile.SharedInstance.MusicVolume;
		}
		if(soundSlider != null) {
			//soundSlider.sliderValue = GameProfile.SharedInstance.SoundVolume;
			//AudioManager.SharedInstance.SoundVolume = GameProfile.SharedInstance.SoundVolume;
		}
		//if(showTutorialSlider != null) {
		//	showTutorialSlider.sliderValue = GameProfile.SharedInstance.ShowTutorial ? 0.0f : 1.0f;
		//  }
		if(showFriendMarkerSlider != null) {
			showFriendMarkerSlider.sliderValue = GameProfile.SharedInstance.ShowFriendMarkers ? 0.0f : 1.0f;
		}
	}
	
	public override void OnBackButton ()
	{
		GameProfile.SharedInstance.Serialize();
		base.OnBackButton ();
	}
	
	void OnMusicSliderChange(float val) {
		if(AudioManager.SharedInstance != null) {
			//AudioManager.SharedInstance.MusicVolume = GameProfile.SharedInstance.MusicVolume = val;
		}
		notify.Debug("MUSIC"+val);
	}
	
	void OnShowFriendsSliderChange(float val) {
		GameProfile.SharedInstance.ShowFriendMarkers = (val > 0.5f) ? false : true;
	}
	
	//void OnShowTutorialSliderChange(float val) {
	//	GameProfile.SharedInstance.ShowTutorial = (val > 0.5f) ? false : true;
	//}
	
	void OnSoundSliderChange(float val) {
		if(AudioManager.SharedInstance != null) {
			//AudioManager.SharedInstance.SoundVolume = GameProfile.SharedInstance.SoundVolume = val;
		}
	}
}

 */