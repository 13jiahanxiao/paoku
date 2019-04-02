// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Unlit/Transparent - ColorOnlyOverlay" {
Properties {
    _Color ("Main Color", Color) = (1,1,1,0.5)
}

SubShader {
	Tags {"Queue"="Overlay" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 100
	Blend SrcAlpha OneMinusSrcAlpha 
	Pass {
	
	CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	
	#include "UnityCG.cginc"
	
	float4 _Color;
	
	struct v2f {
	    float4  pos : SV_POSITION;
	};
	
	v2f vert (appdata_base v)
	{
	    v2f o;
	    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
	    return o;
	}
	
	half4 frag (v2f i) : COLOR
	{
	    return  _Color;
	} 
	ENDCG
	}
}

}
