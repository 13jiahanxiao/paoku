// - Unlit
// - Standard LightMap with Fade out.

Shader "TempleRun2/Unlit Colored" {
Properties {
	_MainTex ("Base layer (RGB)", 2D) = "white" {}
	//_AMultiplier ("Layer Multiplier", Float) = 0.5
	_Color("Color", Color) = (1,1,1,1)

}

SubShader {
	Tags { "Queue"="Geometry-55" "RenderType"="Opaque" }
	
	Lighting Off Fog { Mode Off }
	ZWrite On
	Blend Off
	//Blend SrcAlpha OneMinusSrcAlpha
	LOD 100
	
		
	CGINCLUDE
	//#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
	#include "UnityCG.cginc"
	sampler2D _MainTex;
	
	float _AMultiplier;
	
	float4 _Color;

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		fixed4 color : TEXCOORD2;
	};

	
	v2f vert (appdata_full v)
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord.xy;
		
		o.color = _Color * _AMultiplier;
		return o;
	}
	ENDCG


	Pass {
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest		
		fixed4 frag (v2f i) : COLOR
		{
			fixed4 o;
			fixed4 tex = tex2D (_MainTex, i.uv);
			
			o = (tex) * i.color;
			//o.a = i.color.a;
			return o;
		}
		ENDCG 
	}	
}
}
