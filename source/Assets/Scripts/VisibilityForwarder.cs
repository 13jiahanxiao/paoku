using UnityEngine;
using System.Collections;

public class VisibilityForwarder : MonoBehaviour {
	
	void OnEnable()
	{
		if(renderer.isVisible)
			transform.parent.SendMessage("OnBecameVisible",SendMessageOptions.DontRequireReceiver);
		else
			transform.parent.SendMessage("OnBecameInvisible",SendMessageOptions.DontRequireReceiver);
	}

	void OnBecameVisible()
	{
		transform.parent.SendMessage("OnBecameVisible",SendMessageOptions.DontRequireReceiver);
	}
	
	void OnBecameInvisible()
	{
		transform.parent.SendMessage("OnBecameInvisible",SendMessageOptions.DontRequireReceiver);
	}
}
