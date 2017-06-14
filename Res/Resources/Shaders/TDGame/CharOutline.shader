// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "TYImage/CharOutline" {
	Properties {
		_AlphaTex ("ALPHA Texture Image", 2D) = "white" {} 
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_Outline ("Outline width", Range (.002, 0.03)) = .003	
        _SubTex ("Sub RGB Texture Image", 2D) = "red" {}       	  
        _BlendValue ("BlendValue", Range(0,1)) = 0	
	}
 
CGINCLUDE
#include "UnityCG.cginc"
 
	struct vertexInput {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
        float4 texcoord : TEXCOORD0;
		};
 
	struct vertexOutput {
		float4 pos : POSITION;
		float4 color : COLOR;
        float4 tex : TEXCOORD0;
	};
 
	uniform float _Outline;
	uniform float4 _OutlineColor;
 
	vertexOutput vert(vertexInput input) 
	{
		// just make a copy of incoming vertex data but scaled according to normal direction
		vertexOutput o;
        o.tex = input.texcoord;
		o.pos = UnityObjectToClipPos(input.vertex);
 
		float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, input.normal);
		float2 offset = TransformViewToProjection(norm.xy);
 
		o.pos.xy += offset * o.pos.z * _Outline;
		o.color = _OutlineColor;
		return o;
	}
	ENDCG
 
	SubShader 
	{
		//Tags {"Queue" = "Geometry+100" }
		
 
		// note that a vertex shader is specified here but its using the one above
		Pass {
			Name "OUTLINE"
			Tags { "LightMode" = "Always" }
			Cull Front
			ZWrite On
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
			//Offset 50,50
 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
            uniform sampler2D _AlphaTex;   
			half4 frag(vertexOutput i) :COLOR 
            { 
                float2 uv = float2(i.tex.xy);
                float4 clr  = tex2D(_AlphaTex,  uv);
                return float4( i.color.rgb,clr.r); 
            }
			ENDCG
		}
	}
 
	
	Fallback "Mobile/Diffuse"
}
