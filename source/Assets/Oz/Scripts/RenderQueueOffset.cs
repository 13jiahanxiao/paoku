using UnityEngine;
using System.Collections;

public class RenderQueueOffset : MonoBehaviour {

	public int offset = 0;
	
	// Use this for initialization
	void Start () {
		SetRenderQueue(transform);
	}

	private void SetRenderQueue(Transform root) {
		
		for(int i = 0; i < root.childCount; i++) {
			Transform xform = root.GetChild(i);
			Renderer r = xform.renderer;
			if(r){
				r.material.renderQueue += offset;
				Debug.Log (xform.name + " " + xform.renderer.material.renderQueue);
			}
			UIWidget ui = xform.GetComponent<UIWidget>();
			if(ui){
				ui.material.renderQueue += offset;
				Debug.Log (xform.name + " " + ui.material.renderQueue);
			}
			SetRenderQueue(xform);
		}
	}
	
	

}
