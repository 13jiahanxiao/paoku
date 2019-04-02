using UnityEngine;
using System.Collections;

public class GatchaCard : MonoBehaviour {
	
	public Vector3 origLocalPos;
	public Vector3 origScale;
	public bool isFlipped = false;
	public GatchaDataSet data;
	public UILabel label;
	public UISprite icon;
	public UISprite musicBox;
	public ParticleSystem fx;
	public ParticleSystem fxOpenBox;
	public ParticleSystem fxEmpty;
	[HideInInspector]
	public Vector3 iconOrigScale;
	
	void Awake () {
		origLocalPos = transform.localPosition;
		origScale = musicBox.transform.localScale;
		//label = transform.GetComponentInChildren<UILabel>();
		//fx = transform.GetComponentInChildren<ParticleSystem>();
		//iconOrigScale = icon.transform.localScale;
		iconOrigScale = new Vector3(95,91,1);
	}
	void Start(){
	}
	
	
	
}
