
// by artsyli

Shader "Hidden/Unlit/Transparent ETC Mask Clip Grayed 3"
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
				float4 worldPos : TEXCOORD1;
				float2 worldPos2 : TEXCOORD2;
			};
	
			sampler2D _MainTex;
			sampler2D _MaskTex;
			float4 _MainTex_ST;
			float4 _ClipRange0 = float4(0.0, 0.0, 1.0, 1.0);
			float2 _ClipArgs0 = float2(1000.0, 1000.0);
			float4 _ClipRange1 = float4(0.0, 0.0, 1.0, 1.0);
			float4 _ClipArgs1 = float4(1000.0, 1000.0, 0.0, 1.0);
			float4 _ClipRange2 = float4(0.0, 0.0, 1.0, 1.0);
			float4 _ClipArgs2 = float4(1000.0, 1000.0, 0.0, 1.0);

			float2 Rotate (float2 v, float2 rot)
			{
				float2 ret;
				ret.x = v.x * rot.y - v.y * rot.x;
				ret.y = v.x * rot.x + v.y * rot.y;
				return ret;
			}	

			VS_OUTPUT vert (VS_INPUT In)
			{

				VS_OUTPUT Out;
				Out.position = mul(UNITY_MATRIX_MVP, In.position);
				Out.uv = In.uv;
				Out.color = In.color;
				Out.worldPos.xy = In.position.xy * _ClipRange0.zw + _ClipRange0.xy;
				Out.worldPos.zw = Rotate(In.position.xy, _ClipArgs1.zw) * _ClipRange1.zw + _ClipRange1.xy;
				Out.worldPos2 = Rotate(In.position.xy, _ClipArgs2.zw) * _ClipRange2.zw + _ClipRange2.xy;
				return Out;
			}

			float4 frag (VS_OUTPUT In) : COLOR
			{
			    // First clip region
			    float2 factor = (float2(1.0, 1.0) - abs(In.worldPos)) * _ClipArgs0;
				float f = min(factor.x, factor.y);

				// Second clip region
				factor = (float2(1.0, 1.0) - abs(In.worldPos.zw)) * _ClipArgs1.xy;
				f = min(f, min(factor.x, factor.y));

				// Third clip region
				factor = (float2(1.0, 1.0) - abs(In.worldPos2)) * _ClipArgs2.xy;
				f = min(f, min(factor.x, factor.y));

				float4 color = tex2D(_MainTex, In.uv);
				color.a = 1.0;
				color *= In.color;
				color.a *= tex2D(_MaskTex, In.uv).r;
				if(tex2D(_MaskTex, In.uv).g<0.5f)
				   color.a=0;
				color.a *= clamp( f, 0.0, 1.0);
                color.rgb = dot(color.rgb, fixed3(.299,.587,.114));
				color.rgb = color.rgb* fixed3(.690,.655,.580)*1.6;

				return color;
			}
			ENDCG
		}
	}
}
