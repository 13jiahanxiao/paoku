using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaySound : MonoBehaviour {
	
	
	public List<AudioManager.Effects> Effects;
	void Play()
	{
		if(Effects.Count>0)
			AudioManager.SharedInstance.PlayFX(Effects[Random.Range(0,Effects.Count)]);
	}
}
