using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class RampColor {
	public Color color;
	public float position;
	public RampColor(Color color, float position) {
		this.color = color;
		this.position = position;
	}
}


public class Ramp : ScriptableObject {
	public int size = 256;
	public bool blend = true;
	public List<RampColor> colors = null;
	
	//private Texture2D texture;
	
	void OnEnable() {
		if(colors == null) {
			colors = new List<RampColor>();
			colors.Add(new RampColor(Color.white, 0));
			colors.Add(new RampColor(Color.black, 1));
		}
		//if(texture == null) {
			UpdateRamp();	
		//}
	}
	
//	public Texture2D Texture {
//		get {
//			if(texture == null) UpdateRamp();
//			return texture;
//		}
//	}
	
	public void UpdateRamp() {
		colors.Sort(delegate(RampColor A, RampColor B) {
			return A.position.CompareTo(B.position);	
		});
//		if(texture == null)
//			texture = new Texture2D(size, 16, TextureFormat.RGB24, false);
//		if(texture.width != size) {
//			texture.Resize(size, 16);	
//		}
//		for(var x=0; x<size; x++) {
//			var c = Sample(x/(size*1f));
//			for(var y=0; y<size; y++) {
//				texture.SetPixel(x, y, c);
//			}
//		}
//		texture.Apply();
	}
	
	public Color Sample(float p) {
		var baseC = colors[0];
		var nextC = colors[1];
		for(var i=0; i<colors.Count; i++) {
			if(colors[i].position > p) {
				nextC = colors[i];
				break;
			} else {
				baseC = colors[i];
			}
		}
		var F = Mathf.InverseLerp(baseC.position, nextC.position, p);
		if(!blend) F = p>0.5f?1:0;
		var c = Color.Lerp(baseC.color, nextC.color, F);
		c.a = baseC.color.a + (nextC.color.a * F);
		return c;
	}
	
}
