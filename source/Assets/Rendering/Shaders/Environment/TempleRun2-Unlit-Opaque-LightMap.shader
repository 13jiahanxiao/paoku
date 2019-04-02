// - Unlit
// - Standard LightMap with Fade out.

Shader "TempleRun2/Environment/Lightmap - Opaque" {
Properties {
	_MainTex ("Base layer (RGB)", 2D) = "white" {}
	_DetailTex ("2nd layer (RGB)", 2D) = "white" {}
	//_AMultiplier ("Layer Multiplier", Float) = 0.5
}

SubShader {
	Tags { "Queue"="Geometry-55" "RenderType"="Opaque" }
	
	Lighting Off Fog { Mode Off }
	ZWrite On
	Blend Off
	//Blend SrcAlpha OneMinusSrcAlpha
	//LOD 100
	
		
	CGINCLUDE
	//#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
	#include "UnityCG.cginc"
	sampler2D _MainTex;
	sampler2D _DetailTex;
	
	float _AMultiplier;

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		float2 uv2 : TEXCOORD1;
		//fixed4 color : TEXCOORD2;
	};

	
	v2f vert (appdata_full v)
	{
		//float3	viewPos			= mul(UNITY_MATRIX_MV,v.vertex);
		//float		dist		= length(viewPos);
		//float		nfadeout	= saturate( (dist - _FadeInDistNear) / (_FadeInDistNear));
		//float		ffadeout	= 1 - saturate( (dist - _FadeOutDistNear) / (_FadeOutDistFar - _FadeOutDistNear));
		//float		ffadeout	= 1 - (dist - _FadeOutDistNear) / (_FadeOutDistFar - _FadeOutDistNear);
		v2f o;
		//v.vertex.x+=(_SinTime*1.0*v.vertex.z); 
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord.xy;
		o.uv2 = v.texcoord1.xy;
		
		//o.color = _Color * _AMultiplier;
		//o.color.a = ffadeout * _Color.a;
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
			
			o = (tex * tex2) * _AMultiplier;//* i.color;
			//o.a = i.color.a;
			return o;
		}
		ENDCG 
	}	
}
}
