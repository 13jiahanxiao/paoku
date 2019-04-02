using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaleBannerRoot : MonoBehaviour 
{
	void Awake()
	{
		GameObject saleBanner = (GameObject)Instantiate(Resources.Load("Oz/Prefabs/SaleBanner"));	
		saleBanner.transform.parent = transform;
		saleBanner.transform.localPosition = Vector3.zero;
		saleBanner.transform.localScale = Vector3.one;
	}
}
