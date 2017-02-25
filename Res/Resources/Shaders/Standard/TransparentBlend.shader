Shader "TDGame/TransparentBlend" 
{
    Properties 
	{
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	}

	SubShader 
	{
        Tags
		{
            "Queue"="Geometry"
			"IgnoreProjector"="True"
		}

		Pass 
		{
			Tags {"LightMode" = "ForwardBase"}   

            Blend SrcAlpha OneMinusSrcAlpha
			Cull Off

			CGPROGRAM
			
			#pragma fragment frag
			#pragma vertex vert
			#pragma exclude_renderers flash

			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			uniform sampler2D _MainTex;
            uniform float4 _MainTex_ST;

            struct vertexInput {
                float4 vertex : POSITION;
                float4 texcoord0 : TEXCOORD0;
            };

            struct vertexOutput {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            vertexOutput vert(vertexInput v) {
                vertexOutput o;
                o.uv = TRANSFORM_TEX(v.texcoord0, _MainTex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
			
			float4 frag(vertexOutput i) : COLOR {
                return tex2D(_MainTex, i.uv) + float4(UNITY_LIGHTMODEL_AMBIENT.xyz, 0);
            }
			
			ENDCG
		}
	}

	Fallback "Diffuse"
}