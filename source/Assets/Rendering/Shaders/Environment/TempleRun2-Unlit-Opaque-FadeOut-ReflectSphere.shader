// - Unlit
// - Standard LightMap with Fade out, Reflection for objects with no normals.

Shader "TempleRun2/Environment/Lightmap - FadeOut - Reflection" {
Properties {
	_MainTex ("Base layer (RGB)", 2D) = "white" {}
	_DetailTex ("2nd layer (RGB)", 2D) = "grey" {}
	_EnvTex ("Sphere Map (RGB)", 2D) = "black" {}	
	_AMultiplier ("Layer Multiplier", Float) = 0.5	
	_Reflectivity ("Reflectivity", Float) = 1.0
	_Color("Reflection Color", Color) = (1,1,1,1)
	_SphereMapScale("Sphere Map Scale", float) = 20			
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
	sampler2D _DetailTex;
	sampler2D _EnvTex;	
	
	float _AMultiplier;
	float _Reflectivity;	
	float4 _Color;
	float _SphereMapScale;	
	
	float _FadeOutDistNear;
	float _FadeOutDistFar;
	

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		float2 uv2 : TEXCOORD1;
		float2 reflectuv : TEXCOORD2;				
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
		
		o.reflectuv.x = viewPos.x/_SphereMapScale + 0.5;
		o.reflectuv.y = viewPos.y/_SphereMapScale + 0.5;		
		
		o.fade = ffadeout;
		
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
			fixed4 c = lerp(tex2D(_MainTex, i.uv), tex2D(_EnvTex, i.reflectuv) * _Color, _Reflectivity) * tex2D(_DetailTex, i.uv2) * _AMultiplier;
			c.a = i.fade;// * tex.a;
			return c;
		}
		ENDCG 
	}	
}
}
