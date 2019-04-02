// - Unlit
// - Standard LightMap with Fade out.

Shader "TempleRun2/Environment/Unlit - FadeOut" {
Properties {
	_MainTex ("Base layer (RGB)", 2D) = "white" {}
	_FadeOutDistNear ("Near fadeout dist (View Space)", float) = 45	
	_FadeOutDistFar ("Far fadeout dist (View Space)", float) = 50

}

SubShader {
	Tags { "Queue"="Transparent-100" "RenderType"="Transparent" "IgnoreProjector"="true"  }
	
	Lighting Off Fog { Mode Off }
	ZWrite On
	Blend SrcAlpha OneMinusSrcAlpha
	LOD 100
	
		
	CGINCLUDE
// Upgrade NOTE: excluded shader from Xbox360; has structs without semantics (struct v2f members fade)
//#pragma exclude_renderers xbox360
	//#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
	#include "UnityCG.cginc"
	sampler2D _MainTex;
	
	float _FadeOutDistNear;
	float _FadeOutDistFar;
	
	float4 _Color;

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
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
			fixed4 o = tex2D (_MainTex, i.uv);
			o.a = i.fade * o.a;
			return o;
		}
		ENDCG 
	}	
}
}
