using UnityEngine;
using System.Collections;

public class ShadowQuality : MonoBehaviour {

	public Projector ShadowProjector = null;
	public Camera ShadowCamera = null;
	public Transform Blob = null;
	// Use this for initialization
	private bool alwaysBlob = false;
	private bool lowQuality = false;
	private bool enableShadow = true;
	
	public void EnableShadow(bool shadowOn)
	{
		if(shadowOn == enableShadow)
		{
			return;
		}
		enableShadow = shadowOn;
		if(enableShadow)
		{
			SetQuality(lowQuality);
		}
		else
		{
			if(ShadowCamera != null)
			{
				ShadowCamera.gameObject.SetActiveRecursively(false);
			}
			if(ShadowProjector != null)
			{
				ShadowProjector.gameObject.SetActiveRecursively(false);
			}
			if(Blob != null)
			{
				GameObject blobObject = Blob.gameObject;
				blobObject.SetActiveRecursively(false);
			}
		}
	}
	
	public void UpdateBlobShadowPosition(Vector3 newPosition)
	{
		if(lowQuality)
		{
			Blob.position = newPosition;
		}
	}
	
	public void UpdateBlobShadowScale(float newScale)
	{
		if(lowQuality)
		{
			Blob.localScale = newScale * new Vector3(0.65f, 1f, 1f);
		}
	}
	
	
	/// <summary>
	/// returns true if we forces the BLOB shadow.
	/// </summary>
	bool ForceBlobShadow()
	{
#if UNITY_IPHONE
		GameController.DeviceGeneration device = GameController.SharedInstance.GetDeviceGeneration();

		switch( device )
		{
			case GameController.DeviceGeneration.Unsupported :
			case GameController.DeviceGeneration.iPodTouch3 :
			case GameController.DeviceGeneration.iPodTouch4 :
			case GameController.DeviceGeneration.iPhone3GS :
			case GameController.DeviceGeneration.iPhone4 :
			case GameController.DeviceGeneration.iPad3 :
				return true;
		}
#endif
		
#if UNITY_ANDROID
		
		if ( GameController.userSelectedQuality == -1 )
		{
			if( SystemInfo.systemMemorySize < 300 )
			{
				return true;
			}
		}
#endif
		return false;
	}
	
	void SetQuality(bool low)
	{
		if( ForceBlobShadow() )
		{//always low if no memory
			alwaysBlob = true;
		}
		if(low || alwaysBlob)
		{
			low = true;
			if(ShadowCamera)
			{
				GameObject camObject = ShadowCamera.gameObject;
				if(alwaysBlob)
				{
					ShadowCamera = null;
					Destroy(camObject);
				}
				else
				{
					camObject.SetActiveRecursively(false);
				}
			}
			if(ShadowProjector)
			{
				GameObject projectorObject = ShadowProjector.gameObject;
				if(alwaysBlob)
				{
					ShadowProjector = null;
					Destroy(projectorObject);
				}
				else
				{
					projectorObject.SetActiveRecursively(false);
				}
			}
			if(Blob)
			{
				GameObject blobObject = Blob.gameObject;
				blobObject.SetActiveRecursively(true);
			}
		}
		else
		{
			if(ShadowProjector && ShadowCamera)
			{
				Texture cookieTexture = null;
				cookieTexture = Resources.Load("projectedShadow", typeof(Texture)) as Texture;
				//projector.orthoGraphicSize = 3.0f;
				ShadowProjector.material.SetTexture("_ShadowTex", cookieTexture);
				ShadowProjector.gameObject.SetActiveRecursively(true);
				ShadowCamera.gameObject.SetActiveRecursively(true);
			}
			if(Blob)
			{
				GameObject blobObject = Blob.gameObject;
				blobObject.SetActiveRecursively(false);
			}
		}
		lowQuality = low;
	}
	
	void Start () 
	{
		enableShadow = true;

		lowQuality = ((QualitySettings.GetQualityLevel() == 0)
				||(GameController.SharedInstance.GetDeviceGeneration() == GameController.DeviceGeneration.iPad3));
		SetQuality (lowQuality);
	}
	
	public void Update()
	{//check if shadow quality has changed
		if(alwaysBlob)
		{
			return;
		}
		
		bool newQuality = ((QualitySettings.GetQualityLevel() == 0) || GameController.SharedInstance.GetDeviceGeneration() == GameController.DeviceGeneration.MedEnd);
		if(newQuality != lowQuality)
		{
			Debug.Log ("ShadowQuality change " + newQuality );
			lowQuality = newQuality;
			if(enableShadow)
			{
				SetQuality(lowQuality);
			}
		}
			
	}
}
