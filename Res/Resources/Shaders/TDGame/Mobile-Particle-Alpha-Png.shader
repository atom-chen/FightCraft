Shader "TDGame/Particles/Alpha Blended_PNG" {
    Properties {
        _MainTex ("Particle Texture", 2D) = "white" {}
		_TintColor ("Main Tint", Color) = (0.5,0.5,0.5,0.5)
	}
	
	SubShader {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		
		Pass {
			CGPROGRAM

			#pragma vertex vert
            #pragma fragment frag
			
			#include "UnityCG.cginc"
			
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
            uniform fixed4 _TintColor;

            struct vertexInput {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct vertexOutput {
                float4 pos : POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            vertexOutput vert(vertexInput v) {
                vertexOutput o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.color = v.color;
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }
			
			fixed4 frag(vertexOutput i) : COLOR {
                return 2.0f * i.color * _TintColor * tex2D(_MainTex, i.uv);
            }

			ENDCG
		}
	}
	Fallback "Mobile/Particles/Alpha Blended"
}