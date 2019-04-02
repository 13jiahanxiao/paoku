Shader "TempleRun2/Multiply-Shadow" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	//	_Color("Color", Color) = (1,1,1,1)
		_Strength ("Strength",float) = 1
	}
	Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }

	Blend SrcAlpha OneMinusSrcAlpha
	Lighting Off //Fog { Color (0,0,0,0) }
	Offset -15, 0
	
//	BindChannels {
//		Bind "Color", color
//		Bind "Vertex", vertex
//		Bind "TexCoord", texcoord
//	}
	
	SubShader {
	CGINCLUDE
	//#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
	#include "UnityCG.cginc"
	sampler2D _MainTex;
	float _Strength;
	
	//float4 _Color;

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
	//	fixed4 color : TEXCOORD2;
	};

	
	v2f vert (appdata_full v)
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord.xy;
		
	//	o.color = _Color;
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
			
			o = _Strength*tex;// * i.color;
			//o.a = i.color.a;
			return o;
		}
		ENDCG 
	}	
	
//		Pass {
//			SetTexture [_MainTex] {
//				combine texture * texture
//			}
//		}
	}
}
}