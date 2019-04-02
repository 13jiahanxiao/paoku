using UnityEngine;
using System.Collections;

public class CharacterSelectFOV : MonoBehaviour
{
	public float 	defaultFOV = 75.0f;
	public float 	FOVForHeight1136 = 86.0f;
	public bool 	OverrideFOV = true;
	
	private Camera thisCam = null;
	// Use this for initialization
	void Start ()
	{
		thisCam = this.GetComponent<Camera>() as Camera;
	}
	
	void Update() {
		if(OverrideFOV && thisCam != null) {
			if(Screen.height == 1136) {
				thisCam.fieldOfView = FOVForHeight1136;
			}
			else {
				thisCam.fieldOfView = defaultFOV;
			}	
		}
	}
	
	
}

