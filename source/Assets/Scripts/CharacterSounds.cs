using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class CharacterSounds : MonoBehaviour
{
	
	public List<AudioClip> footsteps_ww = new List<AudioClip>();
	
	public List<AudioClip> footsteps_df = new List<AudioClip>();
	
	public List<AudioClip> footsteps_ybr = new List<AudioClip>();
	
	public List<AudioClip> footsteps_ec = new List<AudioClip>();
	
	public AudioClip jump;
	
	public AudioClip land;
	
	public AudioClip slide;
	
	public AudioClip turn_right;
	
	public AudioClip turn_left;
	
	public AudioClip fail_tree;
	public AudioClip fail_rock;
	public AudioClip fail_fall;
	
	public AudioClip activate_finley;
	public AudioClip enter_balloon;
	public AudioClip baboon_grab;
	
	
	
	
	/*public List<string> paths = new List<string>();
	
	private static Dictionary<string,AudioClip> loadedClips = new Dictionary<string, AudioClip>();
	
	
	public void Load()
	{	
		foreach(string key in loadedClips.Keys)
		{
			Resources.UnloadAsset(loadedClips[key]);
			loadedClips.Remove(key);
		}
		
		for(int i=0;i<paths.Count;i++)
		{
			AudioClip clip = (AudioClip)Resources.Load(paths[i]);
			
		}
		
	}
	
	
	public static AudioClip GetClip(string name)
	{
		if(loadedClips.ContainsKey(name))
		{
			return loadedClips[name];
		}
		
		return null;
	}*/
}
