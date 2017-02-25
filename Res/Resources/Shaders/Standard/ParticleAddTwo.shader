Shader "HxageShaders/Particles/ParticleAddTwo"
 {
 
    Properties 
 
    {
         
        _MainTex ("Base (RGB)", 2D) = "white" {}
		_Mask ("Mask", 2D) = "white" {}
	
    }
     
 
     
 
    SubShader 
     
    {

	Tags {"Queue" = "Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 200
     			ZWrite Off
				Blend One One
        Pass 
      
        {
 
     
            CGPROGRAM
   

            #pragma vertex Vert
 
            #pragma fragment Frag
     
            #include "UnityCG.cginc"
     
 
            sampler2D _MainTex;
         
            struct V2F
         
            {
         
                float4 pos:POSITION;
				float4 col:COLOR;
                float2 txr1:TEXCOORD0;
         
            };
               
         
            V2F Vert(appdata_full v)
     
            {
                 
                V2F output;
     
                output.pos = mul(UNITY_MATRIX_MVP,v.vertex);
				output.col = v.color;
                output.txr1 = v.texcoord;
             
                return output;
             
            }
             
         
            half4 Frag(V2F i):COLOR
         
            {
                 
                return tex2D(_MainTex,i.txr1) * tex2D(_Mask,i.txr1)* i.col;
     
            }
 
         
            ENDCG
       
       
     
        }
 
         
    }
     
     
FallBack "Diffuse"
 
}