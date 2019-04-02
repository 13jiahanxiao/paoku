using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FTPSGraph : MonoBehaviour {
	protected static Notify notify;

	public List<float> Points = new List<float>(1000);

	float width;
	float height;

	public static FTPSGraph Instance = null;
	public int TargetFrameRate = 60;
	public float MaxDeltaTime = 1.0f/10.0f;
	
	void Awake()
	{
		notify = new Notify(this.GetType().Name);	
	}
	// Use this for initialization
	void Start () {
		Instance = this;
		CreateLineMaterial();
		width = Screen.width;
		height = Screen.height * 0.1f;
		notify.Debug("Before FRT: " + Application.targetFrameRate + " MDT: " + Time.maximumDeltaTime);
		if(!Application.isEditor)
		{
			Application.targetFrameRate = TargetFrameRate;
			Time.maximumDeltaTime = MaxDeltaTime;
		}

	}

	public void AddPoint(float p)
	{
		Points.Add(p);
	}

	Material lineMaterial;

	void CreateLineMaterial()
	{
		if (!lineMaterial) {
			lineMaterial = new Material("Shader \"Lines/Colored Blended\" {" +
					"SubShader {  Tags { \"Queue\" = \"Transparent+1000\" }  Pass { " +
					"    Blend SrcAlpha OneMinusSrcAlpha " +
					"    ZWrite Off Cull Off Fog { Mode Off } " +
					"    BindChannels {" +
					"      Bind \"vertex\", vertex Bind \"color\", color }" +
					"} } }");
			lineMaterial.hideFlags = HideFlags.HideAndDontSave;
			lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
		}
	}

	void Vert(float x, float y)
	{
		GL.Vertex3(x / Screen.width, y / Screen.height, 0);
	}

	void OnPostRender()
	{

		float max = 0;
		foreach(float p in Points) {
			if (p > max)
				max = p;
		}

		//notify.Debug("Points: " + Points.Count + " max: " + max);


		lineMaterial.SetPass(0);
		GL.PushMatrix();
		CreateLineMaterial();
		// set the current material
		lineMaterial.SetPass(0);
		GL.LoadOrtho();

		GL.Color(new Color(0, 0, 0, 0.5f));

		GL.Begin(GL.TRIANGLES);
		Vert(0, 0);
		Vert(0, height);
		Vert(width, 0);
		Vert(0, height);
		Vert(width, height);
		Vert(width, 0);
		GL.End();


		GL.Begin(GL.LINES);
		GL.Color(new Color(1, 0, 1, 0.5f));
		Vert(0, height+1);
		Vert(width, height+1);
		GL.End();

		GL.Begin(GL.LINES);
		GL.Color(Color.white);
		float x = width;
		int i = Points.Count - 1;
		for(;i >= 0; --i) {
			float p = Points[i];
			float y = (p / max) * height;
			Vert(x, y);
			x--;
			if (x < 0) {
				Points.RemoveAt(0);
				break;
			}
		}



		GL.End();
		GL.PopMatrix();

	}
}
