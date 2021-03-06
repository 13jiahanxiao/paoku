Shader "OZ/OzEffectTransparentFog" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)		
	}
		SubShader {
			Tags
			{
				"Queue" = "Overlay"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
			}
			
			Blend SrcAlpha OneMinusSrcAlpha	
			Lighting Off 
			ZWrite Off 
			//Fog { Mode Off }
			
			Pass {
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
				
				sampler2D _MainTex;
				float4 _MainTex_ST;
				
				half4 _Color;
				
				struct appdata_t
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;					
					float2 texcoord : TEXCOORD0;
				};
				
				struct v2f {
					float4 pos : SV_POSITION;
					fixed4 color : COLOR;					
					float4 uv : TEXCOORD0;
				};
				
				v2f vert (appdata_t v)
				{
					v2f o;
					o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
					o.uv.xy = v.texcoord.xy;
					
					o.color = v.color;					
					
					return o;
				}
				
				fixed4 frag (v2f i) : COLOR { 
					fixed4 o = tex2D (_MainTex, i.uv.xy) * i.color;
					
					return o;
				}
			ENDCG
		}
	
	} 
}