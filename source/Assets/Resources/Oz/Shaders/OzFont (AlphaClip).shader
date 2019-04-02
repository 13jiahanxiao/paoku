Shader "OZ/OzFont (AlphaClip)" {
	Properties {
		//_Color ("Text Color", Color) = (1,1,1,1)    
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "white" {}
		_BorderMidPoint("Border MidPoint", float)= 0.75
		_BorderRange("Border Range", float) = 2.59
		_AlphaRange("Alpha Range", float) = 0.15
	}
		SubShader {
			Tags
			{
				"Queue" = "Overlay"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
			}
			
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			AlphaTest Off
			Blend SrcAlpha OneMinusSrcAlpha
	
			
			Pass {
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
				
				
				//float4 _Color;
				sampler2D _MainTex;
				float4 _MainTex_ST;
				float _BorderMidPoint;
				float _BorderRange;
				float _AlphaRange;
				
				struct appdata_t
				{
					float4 vertex : POSITION;
					half4 color : COLOR;
					float2 texcoord : TEXCOORD0;
				};
				
				struct v2f {
					float4 pos : SV_POSITION;
					half4 color : COLOR;
					float4 uv : TEXCOORD0;
					float2 worldPos : TEXCOORD1;
				};
				
				v2f vert (appdata_t v)
				{
					v2f o;
					o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
					o.color = v.color;
					o.uv.xy = v.texcoord.xy;
					o.worldPos = TRANSFORM_TEX(v.vertex.xy, _MainTex);
					return o;
				}
				
				fixed4 frag (v2f i) : COLOR { 
					half4 tex = tex2D (_MainTex, i.uv.xy);
					half4 col = half4(1,1,1,1) * i.color;
					col.a = tex.a;
					
					
					float2 factor = abs(i.worldPos);
					float val = 1.0 - max(factor.x, factor.y);
					// Option 1: 'if' statement
					if (val < 0.0) col.a = 0.0;
					
					return col; 
				}
			ENDCG
		}
	
	} 
}