// - Unlit
// - Standard LightMap with Fade out.

Shader "TempleRun2/Environment/Lightmap - With Fade out - Fog" {
Properties {
	_MainTex ("Base layer (RGB)", 2D) = "white" {}
	_DetailTex ("2nd layer (RGB)", 2D) = "grey" {}
	_AMultiplier ("Layer Multiplier", Float) = 0.5
	_FadeOutDistNear ("Near fadeout dist (View Space)", float) = 45	
	_FadeOutDistFar ("Far fadeout dist (View Space)", float) = 50
	//_Color("Color", Color) = (1,1,1,1)

}

SubShader {
	Tags { "Queue"="Transparent-100" "RenderType"="Transparent" "IgnoreProjector"="false"  }
	
	Lighting Off //Fog { Mode Off }
	ZWrite On
	Blend SrcAlpha OneMinusSrcAlpha
	LOD 100
	
		
	CGINCLUDE
// Upgrade NOTE: excluded shader from Xbox360; has structs without semantics (struct v2f members fade)
//#pragma exclude_renderers xbox360
	//#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
	#include "UnityCG.cginc"
	sampler2D _MainTex;
	sampler2D _DetailTex;
	
	float _AMultiplier;
	float _FadeOutDistNear;
	float _FadeOutDistFar;
	
	float4 _Color;

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		float2 uv2 : TEXCOORD1;
		//fixed4 color : TEXCOORD2;
		half fade;
	};

	
	v2f vert (appdata_full v)
	{
		float3	viewPos			= mul(UNITY_MATRIX_MV,v.vertex);
		float		dist		= length(viewPos);
		float		ffadeout	= 1 - saturate( (dist - _FadeOutDistNear) / (_FadeOutDistFar - _FadeOutDistNear));
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord.xy;
		o.uv2 = v.texcoord1.xy;
		
		o.fade = ffadeout ;//* _Color.a;
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
			fixed4 tex2 = tex2D (_DetailTex, i.uv2);
			
			o = (tex * tex2) * _AMultiplier;
			o.a = i.fade;// * tex.a;
			return o;
		}
		ENDCG 
	}	
}
}
