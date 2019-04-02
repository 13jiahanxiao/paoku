// - Unlit
// - Flat color transparent shader with cubemap reflection.

Shader "TempleRun2/Environment/Spheremap" {
Properties {
	_MainTex ("Diffuse (RGB)", 2D) = "white" {}
	_Color("Base (RGB)", Color) = (1,1,1,1)
	_EnvTex ("Sphere Map (RGB)", 2D) = "black" {}
	_SpecTex ("Specular Sphere Map (RGB)", 2D) = "black" {}
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
	sampler2D _MainTex;
	float4 _Color;
	sampler2D _EnvTex;
	sampler2D _SpecTex;
		
	float _Transparency;
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
		
		//float3 worldNormal = mul((float3x3)_Object2World, v.normal);
		//o.refl = reflect(-WorldSpaceViewDir(v.vertex), worldNormal);
		//o.refl.x = -o.refl.x;		
		
		//float3 u = normalize(mul(UNITY_MATRIX_MV, v.vertex));
		//float3 worldNormal = mul((float3x3)_Object2World, v.normal);
		//float3 r = reflect(-WorldSpaceViewDir(v.vertex), worldNormal);
		
		//float3 worldNormal = mul((float3x3)_Object2World, v.normal);
		float3 modelViewNormal = normalize(mul((float3x3)UNITY_MATRIX_MV, v.normal));
		//float3 r = reflect(-WorldSpaceViewDir(v.vertex), worldNormal);
		//r.x = -r.x;		
	
		//float3 modelViewReflect = normalize(mul((float3x3)UNITY_MATRIX_MV, r));		
				
		//o.reflectuv.x = (worldNormal.x/2 + 0.5)/5.0;
		//o.reflectuv.y = (worldNormal.y/2 + 0.5)/5.0;
				
		o.reflectuv.x = modelViewNormal.x/2 + 0.5;
		o.reflectuv.y = modelViewNormal.y/2 + 0.5;
				
		//float m = 2.0 * sqrt( r.x*r.x + r.y*r.y + (r.z+1.0)*(r.z+1.0) );
		
		//o.reflectuv.x = r.x/m + 0.5;
		//o.reflectuv.y = r.y/m + 0.5;
		
		//float3 u = normalize((float3)((float3x3)UNITY_MATRIX_MV * v.vertex));
		//vec3 n = normalize( gl_NormalMatrix * gl_Normal );
		//vec3 r = reflect( u, n );
		//float m = 2.0 * sqrt( r.x*r.x + r.y*r.y + (r.z+1.0)*(r.z+1.0) );
		//gl_TexCoord[1].s = r.x/m + 0.5;
		//gl_TexCoord[1].t = r.y/m + 0.5;
	
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
			//fixed4 c = _Color + texCUBE(_EnvTex,i.refl) * _Reflectivity;
			fixed4 c = tex2D (_MainTex, i.uv);
			fixed4 r = tex2D (_EnvTex, i.reflectuv);
			fixed4 s = tex2D (_SpecTex, i.reflectuv);
						
			c = lerp(_Color, r, _Reflectivity) + s;
			
			c.a = _Transparency;
			
			return c;
		}
		ENDCG 
	}	
}
}
