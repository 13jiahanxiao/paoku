using UnityEngine;
using System.Collections;

public class RotateWorld : MonoBehaviour {
	
	public Vector3 RotationRate = new Vector3(0,0,1);
	Vector3 localRotation = new Vector3(0,0,1);
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		localRotation = RotationRate;
		localRotation *= Time.deltaTime;
		transform.Rotate (localRotation);
	}
}
