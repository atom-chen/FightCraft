// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "TDGame/WuQiFightEffectShader" {
	 Properties {
      _MainTex ("RGBA Texture Image", 2D) = "white" {} 
      _AlphaTex ("ALPHA Texture Image", 2D) = "white" {} 
      _SpeculerTex("Speculer Texture Image",2D) = "white"{}
      _Cutoff ("Cutoff", Range(0,1)) = 0.5
      _Alpha ("Alpha", Range(0,1)) = 1
      _BlendColor ("Blend Color", Color) = (1,1,1,1)
      _BlendValue ("BlendValue", Range(0,1)) = 0
	  _OutlineColor ("Outline Color", Color) = (1,1,1,1)
	  _Outline ("Outline width", Range (0, 0.03)) = 0
      _Shininess ("Shininess", Range (1, 512)) = 255
      _ShininessColor("Shininess Color",Color) = (1,1,1,1)  
      _ShininessAtten("Shiness Atten",Range(1,40)) = 1
      _DiffuseColor("Diffuse Color",Color) = (1,1,1,1) 
      _DiffuseAtten("Diffuse Atten",Range(0.01,2)) = 1

   }
SubShader {
      Tags {"Queue" = "Transparent" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}   
      LOD 210
      Pass {	
              Cull off 
              ZWrite On 
              Blend SrcAlpha OneMinusSrcAlpha 
              CGPROGRAM
 
                 #pragma vertex vert  
                 #pragma fragment frag    
                 #include "UnityCG.cginc"       
 
                 uniform sampler2D _MainTex;    
                 uniform sampler2D _AlphaTex;    
                 uniform sampler2D _SpeculerTex; 
                 uniform float _Cutoff;
                 uniform float _Alpha;
                 uniform float4 _BlendColor;
                 uniform float _BlendValue;                                  
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
                         //漫反射
                        float3 diffuseReflection  = saturate(dot( N, L)) * _DiffuseColor.xyz * _DiffuseAtten ;//
                        //高亮反射
                        float3 specularReflection = pow( saturate(dot( R, V )), _Shininess) * _ShininessColor.xyz  * _ShininessAtten;

                        float3 finalColor = clr.xyz  + clr.xyz * (UNITY_LIGHTMODEL_AMBIENT.xyz  + diffuseReflection  + specularReflection ) * saturate(clr_s.x - 0.1);                                       

                        if(_BlendValue > 0.01f)
                        {
                            float3 blendColor =  _BlendColor.xyz * _BlendValue * (1 - pow( saturate(dot( R, V )), _Shininess) * saturate(dot( N, L)));
                            finalColor = finalColor +  blendColor;
                        }
         	            return float4(finalColor ,clr2.r * _Alpha);
                    }
                    else
                    {
                        return float4(clr2.rgb,0);
                    }                   
                 }
             ENDCG
         }   
    Pass {
			Name "OUTLINE"
			Tags { "LightMode" = "Always" }
			Cull Front
			//ZWrite On			
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			    #pragma vertex vert
			    #pragma fragment frag
                #include "UnityCG.cginc" 

                uniform sampler2D _AlphaTex;  
                uniform float _Alpha;
                uniform float _Outline;
	            uniform float4 _OutlineColor; 

                struct vertexInput {
                    float4 vertex : POSITION;                
                    float3 normal : NORMAL;
                    float4 texcoord : TEXCOORD0;
                 };
                 struct vertexOutput {
                    float4 pos : SV_POSITION;
                    float4 color : COLOR;
                    float4 tex : TEXCOORD0;
                 };

                vertexOutput vert(vertexInput input) 
	            {		        
		            vertexOutput o;
                    o.tex = input.texcoord;		                            
                    if(_Outline > 0.0001)
                    {                    
                        o.pos = UnityObjectToClipPos(input.vertex); 
		                float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, input.normal);
		                float2 offset = TransformViewToProjection(norm.xy); 
		                o.pos.xy += offset * o.pos.z * _Outline;
                    }
                    else
                    {
                        o.pos = float4(0,0,0,0);
                    }
		            o.color = _OutlineColor;
		            return o;
	            }
			    half4 frag(vertexOutput i) :COLOR 
                {   
                    if(_Outline > 0.0001)
                    {
                        float2 uv = float2(i.tex.xy);
                        float4 clr  = tex2D(_AlphaTex,  uv);
                        return float4( i.color.rgb,clr.r * _Alpha); 
                    }
                    else
                    {
                        return i.color; 
                    }
                }
			ENDCG
		}
    }
SubShader {
      Tags {"Queue" = "Transparent" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}   
      LOD 200
      Pass {	
              Cull off 
              ZWrite On 
              Blend SrcAlpha OneMinusSrcAlpha 
              CGPROGRAM
 
                 #pragma vertex vert  
                 #pragma fragment frag    
                 #include "UnityCG.cginc"       
 
                 uniform sampler2D _MainTex;    
                 uniform sampler2D _AlphaTex;    
                 uniform sampler2D _SpeculerTex; 
                 uniform float _Cutoff;
                 uniform float _Alpha;
                 uniform float4 _BlendColor;
                 uniform float _BlendValue;                                  
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
                         //漫反射
                        float3 diffuseReflection  = saturate(dot( N, L)) * _DiffuseColor.xyz * _DiffuseAtten ;//
                        //高亮反射
                        float3 specularReflection = pow( saturate(dot( R, V )), _Shininess) * _ShininessColor.xyz  * _ShininessAtten;

                        float3 finalColor = clr.xyz  + clr.xyz * (UNITY_LIGHTMODEL_AMBIENT.xyz  + diffuseReflection  + specularReflection ) * saturate(clr_s.x - 0.1);                                       

                        if(_BlendValue > 0.01f)
                        {
                            float3 blendColor =  _BlendColor.xyz * _BlendValue * (1 - pow( saturate(dot( R, V )), _Shininess) * saturate(dot( N, L)));
                            finalColor = finalColor +  blendColor;
                        }
         	            return float4(finalColor ,clr2.r * _Alpha);
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
