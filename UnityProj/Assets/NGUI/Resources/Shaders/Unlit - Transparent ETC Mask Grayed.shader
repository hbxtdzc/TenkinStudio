
// by artsyli

Shader "Unlit/Transparent ETC Mask Grayed"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "black" {}
		_MaskTex ("Mask Texture (A)", 2D) = "white" {}
	}
	
	SubShader
	{
		LOD 100

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Offset -1, -1
		Blend SrcAlpha OneMinusSrcAlpha
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

            #pragma multi_compile GRAY_OFF GRAY_ON

			#include "UnityCG.cginc"
	
			struct VS_INPUT
			{
				float4 position : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			struct VS_OUTPUT
			{
				float4 position : SV_POSITION;
				float4 color : COLOR;
				float2 uv : TEXCOORD0;
			};
	
			sampler2D _MainTex;
			sampler2D _MaskTex;
			float4 _MainTex_ST;
				
			VS_OUTPUT vert (VS_INPUT In)
			{
				VS_OUTPUT Out;
				Out.position = mul(UNITY_MATRIX_MVP, In.position);
				Out.uv = In.uv;
				Out.color = In.color;
				
				return Out;
			}

			float4 frag (VS_OUTPUT In) : COLOR
			{
				float4 color = tex2D(_MainTex, In.uv);
				color.a = 1.0;
				color *= In.color;
				color.a *= tex2D(_MaskTex, In.uv).r;

                color.rgb = dot(color.rgb, fixed3(.299,.587,.114));
				color.rgb = color.rgb* fixed3(.690,.655,.580)*1.6;

				return color;
			}
			ENDCG
		}
	}
}
