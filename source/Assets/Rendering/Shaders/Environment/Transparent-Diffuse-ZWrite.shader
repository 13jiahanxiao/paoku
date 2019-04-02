// - Unlit
// - Standard LightMap with Fade out.

Shader "Transparent/Diffuse - ZWrite" {
Properties {
	_MainTex ("Base layer (RGB)", 2D) = "white" {}
	_Color("Color", Color) = (1,1,1,1)

}

SubShader {
	Tags { "Queue"="Transparent" "RenderType"="Transparent" }
	
	Fog { Mode Off }
	ZWrite On
	Blend SrcAlpha OneMinusSrcAlpha
	LOD 100
	
		
	CGPROGRAM
	#pragma surface surf Lambert alpha
	
	sampler2D _MainTex;
	float4 _Color;

	struct Input {
		float2 uv_MainTex;
	};
	
	void surf (Input IN, inout SurfaceOutput o)
	{
		fixed4 tex = tex2D (_MainTex, IN.uv_MainTex);
		
		o.Albedo = tex.rgb * _Color.rgb;
		o.Alpha = tex.a * _Color.a; 
	}
	ENDCG 
}
}
