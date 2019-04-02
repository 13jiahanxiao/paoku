using UnityEngine;
using System.Collections;

public class AnimTest : MonoBehaviour {
	
	
	public string animString = "";
	
	void OnGUI()
	{
		GUILayout.BeginVertical(GUILayout.Width(200));
		
		if(GUILayout.Button("Play"))
			animation.Play(animString);
		if(GUILayout.Button("Stop"))
			animation.Stop();
		if(GUILayout.Button("Rewind"))
		{
			animation.Play(animString);
			animation[animString].enabled = true;
			animation[animString].time = 0f;
			animation.Sample();
			animation[animString].enabled = false;
		}
		
		GUILayout.EndVertical();
	}
}
