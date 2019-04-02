Shader "Custom/Vertex Textured"
{
	Properties
	{
		_TextureR ("Texture", 2D) = "black" {}
		_TextureG ("Texture", 2D) = "white" {}
		_TextureB ("Texture", 2D) = "white" {}
		_Scale ("Scale", Vector) = (1,1,1,1)
	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _TextureR;
		sampler2D _TextureG;
		sampler2D _TextureB;
		float2 _Scale;

		struct Input
		{
			float3 worldPos;
			float4 color : COLOR;
		};

		void surf (Input IN, inout SurfaceOutput o)
		{
			half4 colr = tex2D(_TextureR,IN.worldPos.xy / _Scale) * IN.color.r;
			half4 colg = tex2D(_TextureG,IN.worldPos.xy / _Scale) * IN.color.g;
			half4 colb = tex2D(_TextureB,IN.worldPos.xy / _Scale) * IN.color.b;
			o.Emission = colr + colg + colb;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
