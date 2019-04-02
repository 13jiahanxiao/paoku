using UnityEngine;
using System.Collections;

public class Footsteps : MonoBehaviour {
	
	public AudioClip[] footstepsSfx;
	
	private GamePlayer player;
	private GameController controller;
	//public AudioSource audiosource;
	
	void Start(){
		player = GamePlayer.SharedInstance;
		controller = GameController.SharedInstance;
		//if(!audiosource) audiosource = new AudioSource();
		/* not used anymore 
		if(AudioManager.SharedInstance!=null)
		{
			footstepsSfx[0] = AudioManager.SharedInstance.footstepsSfx[0];
			footstepsSfx[1] = AudioManager.SharedInstance.footstepsSfx[1];
			footstepsSfx[2] = AudioManager.SharedInstance.footstepsSfx[2];
			footstepsSfx[3] = AudioManager.SharedInstance.footstepsSfx[3];
			footstepsSfx[4] = AudioManager.SharedInstance.footstepsSfx[4];
		}
		*/
	}
	
	public void OnFootstep(){
		if(controller.IsPaused || controller.IsGameOver || player.Hold || player.IsDead || controller.IsInCountdown) return;
		AudioManager.SharedInstance.PlayFootsteps();
		//int rand = (int)Mathf.Floor(Random.value * 2f);
		//AudioManager.SharedInstance.PlayCharacterSound(AudioManager.SharedInstance.footstepsSfx[rand]);
		//notify.Debug ("OnFootstep " + rand);
		/*
		if(audio!=null && footstepsSfx.Length>rand)
		{
			if(AudioManager.SharedInstance!=null)	audio.volume = AudioManager.SharedInstance.SoundVolume;
			if(footstepsSfx[rand]) audio.clip = footstepsSfx[rand];
				audio.Play();
		}
		*/
	}
}
