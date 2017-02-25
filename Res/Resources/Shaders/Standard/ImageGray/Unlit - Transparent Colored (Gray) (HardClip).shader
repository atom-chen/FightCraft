Shader "Unlit/Transparent Colored (Gray) (HardClip)" {
	Properties {
		_MainTex ("Base (ARGB)", 2D) = "black" {}
		_Alpha("Alpha", Range(0, 1)) = 1
		_Factor("Factor", Range(0,1)) = 1
	}
	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			ColorMask RGB
			AlphaTest Greater .01
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse


			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Alpha;
			float _Factor;

			// vertex input: position, UV
			struct appdata {
			    float4 vertex : POSITION;
				float4 color : COLOR;
			    float4 texcoord : TEXCOORD0;
			};
			struct v2f {
			    float4 pos : SV_POSITION;
				float4 color : COLOR;
			    float2 texcoord : TEXCOORD0;
			    float2 worldPos : TEXCOORD1;
			};
			v2f vert (appdata v) {
			    v2f o;
			    o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
			    o.texcoord = float4( v.texcoord.xy, 0, 0 );
				o.color = v.color;
				o.worldPos = TRANSFORM_TEX(v.vertex.xy, _MainTex);
			    return o;
			}
			half4 frag( v2f i ) : COLOR 
			{
				// Softness factor
				float2 factor = (float2(1.0, 1.0) - abs(i.worldPos)) * float2(200.0, 200.0) ;
			
				// Sample the texture
				float4 c = tex2D (_MainTex, i.texcoord);
				c.a *= clamp( min(factor.x, factor.y), 0.0, 1.0);
				float4 ret = (c.r + c.g + c.b) / 3 * _Factor + c * (1.0 - _Factor);
				ret.a = c.a * _Alpha * i.color.a;
			  return ret;
			}
			ENDCG
		}
	}	
	FallBack "Diffuse"
	
}
