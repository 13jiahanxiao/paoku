using UnityEngine;
using System.Collections;

public class ForceToBack : MonoBehaviour
{
	private bool firstUpdate = true;
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(firstUpdate == true) {
			if(UIRoot.list == null || UIRoot.list.Count == 0)
				return;
			
			//-- Soooo why use the UIRoot Manual height here...
			//-- NGUI will scale the UI based on manualheight and we just want this widget to be on the back plane of the ortho camera.
			//-- There is some math formula that says an ortho camera with size 1, far plane of 2 = manualheight set in editor.
			//-- Ok, if you are reading this far, it just works. 
			Vector3 newP = new Vector3(0,0, UIRoot.list[0].manualHeight-1);
			transform.localPosition = newP;
			firstUpdate = false;
		}
	}
}

