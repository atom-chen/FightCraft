// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "TDGame/Particles/Additive" {
	Properties {
		_MainTex ("Particle Texture", 2D) = "white" {}
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
	}
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha One
		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		
		Pass {
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			// user defined variables
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform fixed4 _TintColor;
			
			// base structs
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
			
			// vertex function
			vertexOutput vert(vertexInput v) {
				vertexOutput o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				// calc the scale and bias of _MainTex uv
				o.uv = TRANSFORM_TEX(v.texcoord,_MainTex);
				return o;
			}
			
			// fragment function
			fixed4 frag(vertexOutput i) : COLOR {
				return 2.0f * i.color * _TintColor * tex2D(_MainTex, i.uv);
			}
			
			ENDCG
		}
	}
	//Fallback "Mobile/Particles/Additive"
}