Shader "TDGame/WuQiOutline" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" { }
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
		o.pos = mul(UNITY_MATRIX_MVP, input.vertex);
 
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
		CGPROGRAM
			#pragma surface surf Lambert
 
			uniform sampler2D _MainTex;
			uniform sampler2D _AlphaTex;    
            uniform sampler2D _SubTex;  
            uniform float _BlendValue;  
 
			struct Input 
			{
				float2 uv_MainTex;
			};
 
			void surf (Input IN, inout SurfaceOutput o) {
				float2 uv = IN.uv_MainTex;
         		float4 clr  = tex2D(_MainTex,  uv);
         		float4 clr2 = tex2D(_AlphaTex, uv);
         	     if(_BlendValue > 0.01f)
                {
                    float4 clr1  = tex2D(_SubTex,  uv);
                    clr = lerp(clr,clr1,_BlendValue);                
                }
	            half4 ret =float4(clr.r, clr.g, clr.b, clr2.r)  ;
     	       	o.Albedo = ret.rgb + ret.rgb * UNITY_LIGHTMODEL_AMBIENT.xyz*0.85;
				o.Emission = ret.rgb;
            	o.Alpha = ret.a;
			}
		ENDCG
 
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
                float2 uv = float2(i.tex);
                float4 clr  = tex2D(_AlphaTex,  uv);
                return float4( i.color.rgb,clr.r); 
            }
			ENDCG
		}
	}
 
	SubShader 
	{
		CGPROGRAM
			#pragma surface surf Lambert
 
			uniform sampler2D _MainTex;
			uniform sampler2D _AlphaTex;
            uniform sampler2D _SubTex;  
            uniform float _BlendValue;  
			struct Input {
				float2 uv_MainTex;
			};
 
			void surf (Input IN, inout SurfaceOutput o) 
			{
				float2 uv = IN.uv_MainTex;
         		float4 clr  = tex2D(_MainTex,  uv);
         		float4 clr2 = tex2D(_AlphaTex, uv);
         	    if(_BlendValue > 0.01f)
                {
                    float4 clr1  = tex2D(_SubTex,  uv);
                    clr = lerp(clr,clr1,_BlendValue);                
                }
	            half4 ret =float4(clr.r, clr.g, clr.b, clr2.r) ;
     	       	o.Albedo = ret.rgb + ret.rgb * UNITY_LIGHTMODEL_AMBIENT.xyz*0.85;
				o.Emission = ret.rgb;
            	o.Alpha = ret.a;
			}
		ENDCG
 
		Pass {
			Name "OUTLINE"
			Tags { "LightMode" = "Always" }
			Cull Front
			ZWrite On
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
 
			CGPROGRAM
			#pragma vertex vert
			#pragma exclude_renderers gles xbox360 ps3
			ENDCG
			SetTexture [_MainTex] { combine primary }
		}
	}
 
	Fallback "Mobile/Diffuse"
}
