// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "TDGame/ImageCutCircle" {
	Properties {        
       _MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
	}
    CGINCLUDE
        #include "UnityCG.cginc"
        uniform sampler2D _MainTex;

        struct v2f 
        {      
		    float4 pos:SV_POSITION;      
		    float2 uv : TEXCOORD0;     
		};    

        v2f vert(appdata_base v)
        {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
            o.uv = v.texcoord.xy;
            return o;
        }

        fixed4 frag(v2f i):COLOR
        {            
            fixed4 col = tex2D(_MainTex,i.uv);            
            fixed alpha = 1 - step(1, length(i.uv * 2 - 1)); 
            return fixed4(col.xyz,alpha * col.a);        
        }
    ENDCG
	SubShader {
		Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
		LOD 200
            
            Pass{
                Blend SrcAlpha OneMinusSrcAlpha     
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma fragmentoption ARB_precision_hint_fastest
                ENDCG

            }       
		
	} 
	FallBack "Diffuse"
}
