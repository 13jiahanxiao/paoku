// - Unlit
// - Flat color transparent shader with cubemap reflection.

Shader "TempleRun2/Environment/Opaque-ReflectSphere" {
Properties {
	_MainTex ("Diffuse (RGB)", 2D) = "white" {}
	_EnvTex ("Sphere Map (RGB)", 2D) = "black" {}
	_Reflectivity ("Reflectivity", Float) = 1.0	
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
	sampler2D _MainTex;;
	sampler2D _EnvTex;
		
	float _Reflectivity;

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		float2 reflectuv : TEXCOORD1;
	};

	
	v2f vert (appdata_full v)
	{
		v2f o;

		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord.xy;
		
		float3 modelViewNormal = normalize(mul((float3x3)UNITY_MATRIX_MV, v.normal));
				
		o.reflectuv.x = modelViewNormal.x/2 + 0.5;
		o.reflectuv.y = modelViewNormal.y/2 + 0.5;
	
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
			fixed4 c = tex2D(_MainTex, i.uv) + tex2D(_EnvTex, i.reflectuv) * _Reflectivity;
			
			return c;
		}
		ENDCG 
	}	
}
}
