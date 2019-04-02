Shader "Custom/Vertex Colored - Diffuse"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;

		struct Input
		{
			float2 uv_MainTex;
			float4 color : COLOR;
		};

		void surf (Input IN, inout SurfaceOutput o)
		{
			half4 col = tex2D(_MainTex,IN.uv_MainTex) * IN.color;
			o.Albedo = col.rgb;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
