// - Unlit
// - Flat color transparent shader with cubemap reflection.

Shader "TempleRun2/Environment/Transparent Reflective" {
Properties {
	//_MainTex ("Diffuse (RGB)", 2D) = "white" {}
	_Color("Base (RGB)", Color) = (1,1,1,1)
	_EnvTex ("Cubemap (RGB)", CUBE) = "black" {}
	_Transparency ("Transparency", Float) = 1.0	
	_Reflectivity ("Reflectivity", Float) = 1.0	
}

SubShader {
	Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="true" }
	
	Lighting Off Fog { Mode Off }
	ZWrite On
	Blend Off
	Blend SrcAlpha OneMinusSrcAlpha
	LOD 100
	
		
	CGINCLUDE
	//#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
	#include "UnityCG.cginc"
	//sampler2D _MainTex;
	float4 _Color;
	samplerCUBE _EnvTex;
	
	float _Transparency;
	float _Reflectivity;

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		float3 refl : TEXCOORD1;
	};

	
	v2f vert (appdata_full v)
	{
		v2f o;

		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord.xy;
		
		float3 worldNormal = mul((float3x3)_Object2World, v.normal);
		o.refl = reflect(-WorldSpaceViewDir(v.vertex), worldNormal);
		o.refl.x = -o.refl.x;		
	
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
			fixed4 c = _Color + texCUBE(_EnvTex,i.refl) * _Reflectivity;
			c.a = _Transparency;
			
			return c;
		}
		ENDCG 
	}	
}
}
