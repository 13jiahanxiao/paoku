using UnityEngine;
using UnityEditor;
using System.Collections;

public class ScreenshotEditor : MonoBehaviour
{
#if UNITY_EDITOR
	protected static Notify notify;
	
	void Awake()
	{
		notify = new Notify(this.GetType().Name);	
	}

	static private int superSize = 1;
	
	[MenuItem("TR2/SupaScreenShot/1x")]
	public static void ScreenShot1()
	{
		superSize = 1;
		ScreenShot("");		
	}
	
	[MenuItem("TR2/SupaScreenShot/2x")]
	public static void ScreenShot2()
	{
		superSize = 2;
		ScreenShot("2x");		
	}
	
	[MenuItem("TR2/SupaScreenShot/4x")]
	public static void ScreenShot4()
	{
		superSize = 4;
		ScreenShot("4x");		
		
	}
	
	[MenuItem("TR2/SupaScreenShot/Insane")]
	public static void ScreenShotInsane()
	{
		superSize = 12;
		ScreenShot("12x");		
		
	}
	
	private static void ScreenShot(string ext)
	{
		if(EditorApplication.isPlaying == true) {
			EditorApplication.isPaused = true;	
		}
		
		string savedPath = EditorUtility.SaveFilePanel("Save SupaScreenshot", "", "supaScreenshot"+ext, "png");
		Application.CaptureScreenshot(savedPath, superSize);
		notify.Debug ("Capturing screenshot at {0} size and saving to : {1}", superSize, savedPath);
	}
#endif
}

