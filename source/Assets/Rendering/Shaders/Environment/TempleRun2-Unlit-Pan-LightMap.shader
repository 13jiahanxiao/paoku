// - Unlit
// - Standard LightMap with Fade out.
// - Pan
Shader "TempleRun2/Environment/Lightmap - Pan" {
Properties {
	_MainTex ("Base layer (RGB)", 2D) = "white" {}
	_DetailTex ("2nd layer (RGB)", 2D) = "white" {}
	_AMultiplier ("Layer Multiplier", Float) = 0.5
	_ScrollX ("Base layer Scroll speed X", Float) = 1.0
	_ScrollY ("Base layer Scroll speed Y", Float) = 0.0
}

SubShader {
	Tags { "Queue"="Transparent-100" "RenderType"="Transparent" "IgnoreProjector"="true" }
	
	Lighting Off Fog { Mode Off }
	ZWrite On
	Blend SrcAlpha OneMinusSrcAlpha
	LOD 100
	
		
	CGINCLUDE
	//#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
	#include "UnityCG.cginc"
	sampler2D _MainTex;
	sampler2D _DetailTex;
	
	float _AMultiplier;
	float _FadeOutDistNear;
	float _FadeOutDistFar;
	float _ScrollX;
	float _ScrollY;
	
	float4 _MainTex_ST;
	
	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		float2 uv2 : TEXCOORD1;
	};

	
	v2f vertA (appdata_full v)
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = TRANSFORM_TEX(v.texcoord.xy, _MainTex) + frac(float2(_ScrollX, _ScrollY) * _Time);
		o.uv2 = v.texcoord1.xy;
		
		return o;
	}
	ENDCG


	Pass {
		CGPROGRAM
		#pragma vertex vertA
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest		
		fixed4 frag (v2f i) : COLOR
		{
			fixed4 o;
			fixed4 tex = tex2D (_MainTex, i.uv);
			fixed4 tex2 = tex2D (_DetailTex, i.uv2);
			
			o = (tex * tex2) * _AMultiplier;
			o.a = tex.a;			
			return o;
		}
		ENDCG 
	}	
}
}
