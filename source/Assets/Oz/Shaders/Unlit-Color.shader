//Shader "Custom/Unlit-Color" {
//	Properties {
//		_MainTex ("Base (RGB)", 2D) = "white" {}
//	}
//	SubShader {
//		Tags { "RenderType"="Opaque" }
//		LOD 200
//		
//		CGPROGRAM
//		#pragma surface surf Lambert
//
//		sampler2D _MainTex;
//
//		struct Input {
//			float2 uv_MainTex;
//		};
//
//		void surf (Input IN, inout SurfaceOutput o) {
//			half4 c = tex2D (_MainTex, IN.uv_MainTex);
//			o.Albedo = c.rgb;
//			o.Alpha = c.a;
//		}
//		ENDCG
//	} 
//	FallBack "Diffuse"
//}
Shader "Unlit/Texture - Color" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	//_Color ("Color", Color) = (1,1,1,1)
}

SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 100
	Cull Off	
	Lighting Off
	Fog { Mode Off }	
	ColorMask RGB
	ColorMaterial AmbientAndDiffuse
	Pass {
		Lighting Off
		SetTexture [_MainTex] {
			//constantColor [_Color]
			combine texture * Primary
		} 
	}
}
}
