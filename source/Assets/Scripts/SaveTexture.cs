using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class SaveTexture : MonoBehaviour {
	protected static Notify notify;
	public string textureName = "foo.png";
	public bool takePicture = false;
	
	private Camera myCam = null;
	
	void Awake()
	{
		if (notify != null)
		{
			notify = new Notify(this.GetType().Name);	
		}
	}
	// Use this for initialization
	void Start () {
		myCam = GetComponent<Camera>() as Camera;
	}
	
	// Update is called once per frame
	void Update () {
		if(takePicture == true) {
			takePicture = false;
			StartCoroutine("saveCameraToDisk");
		}
	}
	
	public void TakeScreenShotNow() {
		StartCoroutine("saveCameraToDisk");
	}
	
	IEnumerator saveCameraToDisk() {
		if(myCam.targetTexture == null) {
			myCam.targetTexture = new RenderTexture((int)myCam.pixelWidth, (int)myCam.pixelHeight, 0, RenderTextureFormat.RGB565, RenderTextureReadWrite.Default);
		}
		
		if(myCam.targetTexture == null) {
			yield return null;
		}
		
		//-- Create a new texture that we can save to disk.
		Texture2D newTexture = new Texture2D((int)myCam.pixelWidth, (int)myCam.pixelHeight, TextureFormat.RGB24, false);
		if(newTexture == null) {
			Destroy(myCam.targetTexture);
			yield return null;
		}
		
		myCam.Render();
		
		yield return new WaitForEndOfFrame();
		
		RenderTexture.active = myCam.targetTexture;
		newTexture.ReadPixels(new Rect(0, 0, myCam.pixelWidth, myCam.pixelHeight), 0, 0);
		newTexture.Apply();
		byte[] png = newTexture.EncodeToPNG();
		
		
		using (MemoryStream stream = new MemoryStream()) 
		{
//#if UNITY_EDITOR			
//			string fileName = Application.dataPath + Path.DirectorySeparatorChar + "Resources" + Path.DirectorySeparatorChar + "TRGameData/" + textureName;
//#else
			string fileName = Application.persistentDataPath + Path.DirectorySeparatorChar + textureName;
//#endif
			try {
				using (FileStream fileWriter = System.IO.File.Create(fileName)) {
					fileWriter.Write(png, 0, png.Length);
					fileWriter.Close(); 
				}
			}
			catch (Exception e) {
				Dictionary<string,string> d = new Dictionary<string, string>();
				d.Add("Exception",e.ToString());
				notify.Warning("Save Exception: " + e);
			}
		}
		Destroy(myCam.targetTexture);
		Destroy(newTexture);
		yield return null;
	}
}
