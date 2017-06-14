// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

  Shader "TDGame/Particles/AlphaSpecial"
 {
 
    Properties
    {
        _CW ("Color Weight", Range (0, 1)) = 1
        _AW ("Alpha Weight", Range (0, 1)) = 1
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _AlphaTex ("ALPHA Texture Image", 2D) = "white" {}
    }
    
	SubShader
	{
	        Tags{"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
	    LOD 200
	    Blend SrcAlpha OneMinusSrcAlpha
	    ZWrite Off
		Cull Off
		Lighting Off 		
		Fog {  Color (0,0,0,0) }

		BindChannels {
			Bind "Color", color
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
		}
	        Pass 
	        {
				CGPROGRAM
				#pragma vertex vert
                #pragma fragment frag
			
						#include "UnityCG.cginc"
			
                 uniform sampler2D _MainTex;
				 uniform float4 _MainTex_ST;
				 uniform sampler2D _AlphaTex;    
                 uniform fixed _CW;
                 uniform fixed _AW;

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
                o.pos = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }
			
			fixed4 frag(vertexOutput i) : COLOR {
         		float4 clra = 2.0f * i.color * float4(_AW,_AW,_AW,_AW) * tex2D(_AlphaTex, i.uv);         	
         		float4 clr  = 2.0f * i.color * float4(_CW,_CW,_CW,_AW) * tex2D(_MainTex, i.uv);
         	
         		return (float4(clr.r, clr.g, clr.b, clra.r));
            }

					ENDCG
				}
	   }
	   Fallback "Mobile/Particles/Alpha Blended"
	
}