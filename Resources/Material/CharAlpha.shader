// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "TYImage/CharAlpha" {
   Properties {
      _MainTex ("RGBA Texture Image", 2D) = "white" {} 
	  _MainColor ("Albedo", Color) = (0,0,0,0.5)
   }
   SubShader {
      Tags {"Queue" = "Transparent" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}  
 
         
      Pass {	
         Cull off 
         ZWrite On 
         Blend SrcAlpha OneMinusSrcAlpha 
         // blend based on the fragment's alpha value
 
         CGPROGRAM
 
         #pragma vertex vert  
         #pragma fragment frag 
 
         uniform sampler2D _MainTex;    
	     uniform float4 _MainColor;
 
         struct vertexInput {
            float4 vertex : POSITION;
            float4 texcoord : TEXCOORD0;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 tex : TEXCOORD0;
         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
 
            output.tex = input.texcoord;
            output.pos = UnityObjectToClipPos(input.vertex);
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR
         {
         	float2 uv = float2(input.tex.xy);
         	float4 clr  = tex2D(_MainTex,  uv);
        	
        	return (float4(clr.r * _MainColor.r, clr.g * _MainColor.g, clr.b * _MainColor.b, clr.r*_MainColor.a));
         }
 
         ENDCG
      }
   }
   // The definition of a fallback shader should be commented out 
   // during development:
   Fallback "Mobile/Diffuse"
}