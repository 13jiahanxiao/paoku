// - Unlit
// - Standard LightMap.

Shader "TempleRun2/Environment/Lightmap" {
Properties {
	_MainTex ("Base layer (RGB)", 2D) = "white" {}
	_DetailTex ("2nd layer (RGB)", 2D) = "white" {}
	//_AMultiplier ("Layer Multiplier", Float) = 0.5
	//_Color("Color", Color) = (1,1,1,1)

}

SubShader {
	Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="true" }
	
	Lighting Off Fog { Mode Off }
	ZWrite Off
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
	
	//float4 _Color;

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		float2 uv2 : TEXCOORD1;
		//fixed4 color : TEXCOORD2;
	};

	
	v2f vert (appdata_full v)
	{
		float3	viewPos			= mul(UNITY_MATRIX_MV,v.vertex);
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord.xy;
		o.uv2 = v.texcoord1.xy;
		
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
			o.a = tex.a;
			return o;
		}
		ENDCG 
	}	
}
}
