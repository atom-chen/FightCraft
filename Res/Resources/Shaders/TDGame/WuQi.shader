// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "TDGame/WuQi" 
{
	Properties {
      _MainTex ("RGBA Texture Image", 2D) = "white" {} 
      _AlphaTex ("ALPHA Texture Image", 2D) = "white" {} 
      _SpeculerTex("Speculer Texture Image",2D) = "white"{} 
      _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
      _Shininess ("Shininess", Range (1, 512)) = 255
      _ShininessColor("Shininess Color",Color) = (1,1,1,1)  
      _ShininessAtten("Shiness Atten",Range(1,40)) = 1
      _DiffuseColor("Diffuse Color",Color) = (1,1,1,1) 
      _DiffuseAtten("Diffuse Atten",Range(0.01,2)) = 1

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
         uniform sampler2D _SpeculerTex; 
         uniform float _Cutoff;
         uniform float _Shininess;
         uniform float4 _ShininessColor;
         uniform float _ShininessAtten;
         uniform float4 _DiffuseColor;
         uniform float _DiffuseAtten;
 
        struct vertexInput {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float4 texcoord : TEXCOORD0;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;                        
            float4 tex : TEXCOORD0;
            float4 worldpos: TEXCOORD1;                   
            float3 normal: TEXCOORD2;         
         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
 
            output.tex = input.texcoord;
            output.pos = UnityObjectToClipPos(input.vertex);
            output.normal = input.normal;
            output.worldpos =  mul(unity_ObjectToWorld,input.vertex);
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR
         {
         	float2 uv = float2(input.tex.xy);
         	float4 clr2 = tex2D(_AlphaTex, uv);
            if(clr2.r > _Cutoff)
            {   
                float4 clr  = tex2D(_MainTex,  uv) ;
                float4 clr_s = tex2D (_SpeculerTex,uv);            
                float3 N = normalize(mul(float4(input.normal,0.0),unity_WorldToObject).xyz);
                float3 L = normalize(_WorldSpaceCameraPos.xyz - input.worldpos.xyz);
                float3 V = normalize( _WorldSpaceCameraPos.xyz - input.worldpos.xyz );
                float3 R = normalize(reflect(-L,N));        
                 //Âþ·´Éä
                float3 diffuseReflection  = saturate(dot( N, L)) * _DiffuseColor.xyz * _DiffuseAtten ;//
                //¸ßÁÁ·´Éä
                float3 specularReflection = pow( saturate(dot( R, V )), _Shininess) * _ShininessColor.xyz  * _ShininessAtten;

                float3 lightFinal = (  UNITY_LIGHTMODEL_AMBIENT.xyz  + diffuseReflection  + specularReflection ) * saturate(clr_s.x - 0.1)  ;                                       
                
         	    return float4(clr.xyz  + clr.xyz * lightFinal ,clr2.r);
            }
            else
            {
                return float4(clr2.rgb,0);
            }
         }
 
         ENDCG
      }
   }
   // The definition of a fallback shader should be commented out 
   // during development:
   Fallback "Mobile/Diffuse"
}