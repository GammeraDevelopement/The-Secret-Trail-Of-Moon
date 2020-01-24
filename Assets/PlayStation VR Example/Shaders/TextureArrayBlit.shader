Shader "PlayStationVR/TextureArrayBlit"
{
	Properties
	{
		_MainTex ("Texture", 2DArray) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
                float3 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
                float3 uv : TEXCOORD0;
			};

			v2f vert (appdata v, out uint sliceToRenderTo : SV_RenderTargetArrayIndex)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                sliceToRenderTo = v.uv.z;
				return o;
			}
			
            UNITY_DECLARE_TEX2DARRAY(_MainTex);			
            float _ArraySlice;

			float4 frag (v2f i) : SV_Target
			{
				float4 col = UNITY_SAMPLE_TEX2DARRAY_LOD(_MainTex, i.uv, 0);
				return col;
			}
			ENDCG
		}
	}
}
