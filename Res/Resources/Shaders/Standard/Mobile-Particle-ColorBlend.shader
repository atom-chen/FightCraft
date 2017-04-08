Shader "TDGame/Particles/ColorBlend" {
	Properties {
		_MainTex ("Particle Texture", 2D) = "white" {}
		_BlendColor ("Blend Color", Color) = (1,1,1,1)
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
			
			// user defined variables
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform fixed4 _BlendColor;
			
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
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = v.color;
				// calc the scale and bias of _MainTex uv
				o.uv = TRANSFORM_TEX(v.texcoord,_MainTex);
				return o;
			}
			
			// fragment function
			fixed4 frag(vertexOutput i) : COLOR {
				float4 color2 = 2.0f * i.color * tex2D(_MainTex, i.uv);
				float alpha = color2.x*0.299 + color2.y*0.587 + color2.z*0.114;
				color2.w = alpha * color2.w;
				color2 = color2 * _BlendColor;
				return color2;
			}
			
			ENDCG
		}
	}
	//Fallback "Mobile/Particles/Additive"
}