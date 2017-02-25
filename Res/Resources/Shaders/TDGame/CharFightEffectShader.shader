// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "TDGame/CharFightEffectShader" {
	 Properties {
      _MainTex ("RGBA Texture Image", 2D) = "white" {} 
      _AlphaTex ("ALPHA Texture Image", 2D) = "white" {} 
      _Cutoff ("Cutoff", Range(0,1)) = 0.5
      _Alpha ("Alpha", Range(0,1)) = 1
      _BlendColor ("Blend Color", Color) = (1,1,1,1)
      _BlendValue ("BlendValue", Range(0,1)) = 0
	  _OutlineColor ("Outline Color", Color) = (1,1,1,1)
	  _Outline ("Outline width", Range (0, 0.03)) = 0

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
                 uniform float _Cutoff;
                 uniform float _Alpha;
                 uniform float4 _BlendColor;
                 uniform float _BlendValue;
         
 
                 struct vertexInput {
                    float4 vertex : POSITION;
                    float4 color : COLOR;
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
                    vertexOutput output; 
                    output.tex = input.texcoord;
                    output.pos = mul(UNITY_MATRIX_MVP, input.vertex);  
                    if(_BlendValue > 0.01f)
                    {
                        float4 posWorld = mul(unity_ObjectToWorld,input.vertex);
                        float3 N = normalize(mul(float4(input.normal,0.0),unity_WorldToObject).xyz);
                        float3 L = normalize(_WorldSpaceLightPos0.xyz - posWorld.xyz);
                        float3 V = normalize( _WorldSpaceCameraPos.xyz - posWorld.xyz );
                        float3 R = normalize(reflect(-L,N));// 2 * saturate(dot(N,L)) * N - L;                    
                        //获得高亮混合颜色
                        output.color.rgb = _BlendColor * (1- pow(saturate(dot(V,R)),0.01) * saturate(dot(N,L))) ;
                        output.color.a = _BlendColor.a;                        
                    }
                    else
                    {
                        output.color = float4(0,0,0,0);
                    }

                    return output;
                 }
 
                 float4 frag(vertexOutput input) : COLOR
                 {   
                    float2 uv = float2(input.tex.xy);      	        
         	        float4 clr2 = tex2D(_AlphaTex, uv);         	       
                    //cutoff ,这里clip被替换掉了
         	        //clip(clr2.r - _Cutoff);    
                    float4 ret = clr2;
                    float a = clr2.r;
                    if(clr2.r > _Cutoff)
                    {                        
         	            float4 clr  = tex2D(_MainTex,  uv);  
                        //blend
                        if(_BlendValue > 0.01f)
                        {                            
                            clr = clr+input.color*_BlendValue; //lerp(clr,input.color,_BlendValue);                
                        }       	
                        ret = clr+clr*float4(UNITY_LIGHTMODEL_AMBIENT.xyz, 0)*0.85 ;
                        ret = float4(ret.rgb,clr2.r * _Alpha);                        
                    }
                    else
                    {
                        ret = float4(clr2.rgb,0);
                    }
                    //alpha     	
                    return ret;
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
                        o.pos = mul(UNITY_MATRIX_MVP, input.vertex); 
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
                 uniform float _Cutoff;
                 uniform float _Alpha;
                 uniform float4 _BlendColor;
                 uniform float _BlendValue;
         
 
                 struct vertexInput {
                    float4 vertex : POSITION;
                    float4 color : COLOR;
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
                    vertexOutput output; 
                    output.tex = input.texcoord;
                    output.pos = mul(UNITY_MATRIX_MVP, input.vertex);  
                    if(_BlendValue > 0.01f)
                    {
                        float4 posWorld = mul(unity_ObjectToWorld,input.vertex);
                        float3 N = normalize(mul(float4(input.normal,0.0),unity_WorldToObject).xyz);
                        float3 L = normalize(_WorldSpaceLightPos0.xyz - posWorld.xyz);
                        float3 V = normalize( _WorldSpaceCameraPos.xyz - posWorld.xyz );
                        float3 R = normalize(reflect(-L,N));// 2 * saturate(dot(N,L)) * N - L;                    
                        //获得高亮混合颜色
                        output.color.rgb = _BlendColor * (1- pow(saturate(dot(V,R)),0.01) * saturate(dot(N,L))) ;
                        output.color.a = _BlendColor.a;                        
                    }
                    else
                    {
                        output.color = float4(0,0,0,0);
                    }

                    return output;
                 }
 
                 float4 frag(vertexOutput input) : COLOR
                 {   
                    float2 uv = float2(input.tex.xy);      	        
         	        float4 clr2 = tex2D(_AlphaTex, uv);         	       
                    //cutoff ,这里clip被替换掉了
         	        //clip(clr2.r - _Cutoff);    
                    float4 ret = clr2;
                    float a = clr2.r;
                    if(clr2.r > _Cutoff)
                    {                        
         	            float4 clr  = tex2D(_MainTex,  uv);  
                        //blend
                        if(_BlendValue > 0.01f)
                        {                            
                            clr = clr+input.color*_BlendValue;  
                        }       	
                        ret = clr+clr*float4(UNITY_LIGHTMODEL_AMBIENT.xyz, 0)*0.85 ;
                        ret = float4(ret.rgb,clr2.r * _Alpha);                        
                    }
                    else
                    {
                        ret = float4(clr2.rgb,0);
                    }
                    //alpha     	
                    return ret;
                 }
             ENDCG
         }   
   }
   
   // The definition of a fallback shader should be commented out 
   // during development:
   Fallback "Mobile/Diffuse"
}
