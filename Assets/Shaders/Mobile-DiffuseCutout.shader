// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Simplified Diffuse shader. Differences from regular Diffuse one:
// - no Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "Mobile/DiffuseCutOut" {
Properties {
	_Tint("Color",Color) = (1,1,1,1)
    _MainTex ("Base (RGB)", 2D) = "white" {}
	_CutOut("CutOut",Range(0,1)) = 0.5
}
SubShader{
	Tags { "RenderType" = "Opaque" }
	LOD 150

	Cull Off

CGPROGRAM
#pragma surface surf Lambert alphatest:_CutOut noforwardadd addshadow

fixed4 _Tint;
sampler2D _MainTex;
//half _CutOut;

struct Input {
    float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
    fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
    o.Albedo = c.rgb * _Tint.rgb;
    o.Alpha = c.a;
	//clip(c.a - _CutOut);
}
ENDCG
}

Fallback "Mobile/VertexLit"
}
