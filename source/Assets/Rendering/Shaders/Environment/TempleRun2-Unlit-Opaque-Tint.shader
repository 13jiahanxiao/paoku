// - Unlit
// - Unlit textured shader with a lerp tint.

Shader "TempleRun2/Environment/Unlit - Opaque - Tint" {
Properties {
	_MainTex ("Base layer (RGB)", 2D) = "white" {}
	_Color("Tint Color", Color) = (1,1,1,1)
	_TintValue ("Tint Value", Float) = 0.0		
}

SubShader {
	Tags { "Queue"="Geometry-55" "RenderType"="Opaque" "IgnoreProjector"="True" }
	
	Lighting Off Fog { Mode Off }
	ZWrite On
	Blend Off
	//Blend SrcAlpha OneMinusSrcAlpha
	//LOD 100
	
		
	CGINCLUDE
// Upgrade NOTE: excluded shader from Xbox360; has structs without semantics (struct v2f members fade)
//#pragma exclude_renderers xbox360
//	#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
	#include "UnityCG.cginc"
	sampler2D _MainTex;
	
	float _TintValue;
	
	float4 _Color;

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
	};

	
	v2f vert (appdata_full v)
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord.xy;
		
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
			fixed4 o = lerp(tex2D (_MainTex, i.uv), _Color, _TintValue);
			return o;
		}
		ENDCG 
	}	
}
}
