Shader "TDGame/WuQiAlpha" 
{
	Properties {
      _MainTex ("RGBA Texture Image", 2D) = "white" {} 
      _AlphaTex ("ALPHA Texture Image", 2D) = "white" {} 
	  _Alpha ("Alpha", Range(0,1)) = 1
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
         uniform sampler2D _AlphaTex;    
		 uniform float _Alpha;
 
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
            output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR
         {
         	float2 uv = float2(input.tex.xy);
         	float4 clr  = tex2D(_MainTex,  uv);
         	float4 clr2 = tex2D(_AlphaTex, uv);
         	float4 clr3 = clr+clr*float4(UNITY_LIGHTMODEL_AMBIENT.xyz, 0)*0.85 ;
        	
        	return (float4(clr3.r, clr3.g, clr3.b, clr2.r*_Alpha));
         }
 
         ENDCG
      }
   }
   // The definition of a fallback shader should be commented out 
   // during development:
   Fallback "Mobile/Diffuse"
}