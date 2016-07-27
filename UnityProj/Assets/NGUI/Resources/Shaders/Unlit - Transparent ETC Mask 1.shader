
// by artsyli

Shader "Hidden/Unlit/Transparent ETC Mask 1"
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
				float2 worldPos : TEXCOORD1;
			};
	
			sampler2D _MainTex;
			sampler2D _MaskTex;
			float4 _MainTex_ST;
			float4 _ClipRange0 = float4(0.0, 0.0, 1.0, 1.0);
			float2 _ClipArgs0 = float2(1000.0, 1000.0);	
			VS_OUTPUT vert (VS_INPUT In)
			{
				VS_OUTPUT Out;
				Out.position = mul(UNITY_MATRIX_MVP, In.position);
				Out.uv = In.uv;
				Out.color = In.color;
				Out.worldPos = In.position.xy * _ClipRange0.zw + _ClipRange0.xy;
				return Out;
			}

			float4 frag (VS_OUTPUT In) : COLOR
			{
			    float2 factor = (float2(1.0, 1.0) - abs(In.worldPos)) * _ClipArgs0;
				float4 color = tex2D(_MainTex, In.uv);
				color.a = 1.0;
				color *= In.color;
				color.a *= tex2D(_MaskTex, In.uv).r;
				color.a *= clamp( min(factor.x, factor.y), 0.0, 1.0);
                float3 togray = float3(0.299, 0.587, 0.114);
                #ifdef GRAY_ON
                color.rgb = dot(color.rgb, togray);
				//color.r *= 0.5019607843137255;
				//color.g *= 0.4352941176470588;
				//color.b *= 0.4352941176470588;
                #endif // GRAY_ON

				return color;
			}
			ENDCG
		}
	}
}
