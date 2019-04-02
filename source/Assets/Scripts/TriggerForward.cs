using UnityEngine;
using System.Collections;

public class TriggerForward : MonoBehaviour {
	
	public GameObject target;
	
	void Awake()
	{
		if(target==null)
			target = transform.parent.gameObject;
	}
	
	void OnTriggerEnter(Collider other)
	{
		target.SendMessage("OnTriggerEnter",other,SendMessageOptions.DontRequireReceiver);
	}
}
