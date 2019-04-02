// - Unlit
// - Standard LightMap with Fade out.

Shader "OZ/OZ_Font_Ramp" {
Properties {
	_MainTex ("Base layer (RGB)", 2D) = "white" {}
	_DetailTex ("2nd layer (RGB)", 2D) = "white" {}
	_Color0 ("Ramp Color Up (RGB)", Color) = (1,1,1,1)
	_Color1 ("Ramp Color Down (RGB)", Color) = (1,1,1,1)
	//_AMultiplier ("Layer Multiplier", Float) = 0.5
}

SubShader {
	Tags { 
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			}
	
	Lighting Off// Fog { Mode Off }
	ZWrite On
	//Blend Off
	Blend SrcAlpha OneMinusSrcAlpha
	//LOD 100
	
		
	CGINCLUDE
	//#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
	#include "UnityCG.cginc"
	sampler2D _MainTex;
	sampler2D _DetailTex;
	float4 _Color0;
	float4 _Color1;
	
	struct v2f {
		float4 pos : SV_POSITION;
		float4 uv : TEXCOORD0;
		float2 lPos : TEXCOORD1;
	};

	
	v2f vert (appdata_full v)
	{
		v2f o; 
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv.xy = v.texcoord.xy;
		o.uv.zw = v.texcoord1.xy;
		o.lPos = mul(UNITY_MATRIX_MV, v.vertex);
		
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
			fixed4 tex = tex2D (_MainTex, i.uv.xy);
			fixed4 tex2 = tex2D (_DetailTex, i.uv.zw);
			//fixed4 ramp = lerp(_Color0, _Color1, i.lPos.x * 100  );
			//o = ramp ;
			o = tex2;
			o.a = tex.a;
			return o;
		}
		ENDCG 
	}	
}
}
