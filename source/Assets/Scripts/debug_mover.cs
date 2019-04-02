using UnityEngine;
using System.Collections;

public class debug_mover : MonoBehaviour {
	
	public float speed = 10.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.World
			);
	}
}
