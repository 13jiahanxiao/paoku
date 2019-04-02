using UnityEngine;
using System.Collections;

public class UpdateVolume : MonoBehaviour {
	
	private float origVolume;
	
	// Use this for initialization
	void Awake () {
		if(audio!=null)
		{
			origVolume = audio.volume;
		}
		Update();
	}
	
	// Update is called once per frame
	void Update () {
		if(audio!=null && UIManagerOz.SharedInstance!=null && AudioManager.SharedInstance!=null && UIManagerOz.SharedInstance.inGameVC!=null &&
			UIManagerOz.SharedInstance.idolMenuVC!=null)
		{
			if(!UIManagerOz.SharedInstance.inGameVC.gameObject.active && !UIManagerOz.SharedInstance.idolMenuVC.gameObject.active)
				audio.volume = 0f;
			else
				audio.volume = origVolume * AudioManager.SharedInstance.SoundVolume;
		}
	}
}
