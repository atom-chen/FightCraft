Shader "TDGame/GuideBackgroup" {
	Properties {
        _MainColor("Main Color",Color) = (1,1,1,1)
        _Rect ("Rect",Vector) = (0,0,0,0)
	}
    CGINCLUDE
        #include "UnityCG.cginc"

        uniform fixed4 _MainColor;
        uniform float4 _Rect;

        struct v2f 
        {      
		    float4 pos:SV_POSITION;      
		    float2 uv : TEXCOORD0;     
		};    

        v2f vert(appdata_base v)
        {
            v2f o;
            o.pos = mul(UNITY_MATRIX_MVP,v.vertex);
            o.uv = v.texcoord.xy;
            return o;
        }

        fixed4 frag(v2f i):COLOR
        {            
            return fixed4(_MainColor.xyz,_MainColor.w * (1-
                step((_Rect.x - _Rect.z / 2),i.uv.x)*
                step((_Rect.y -_Rect.w /2),i.uv.y) * 
                step(i.uv.x,(_Rect.x + _Rect.z/2))*
                step(i.uv.y,(_Rect.y + _Rect.w/2))
                ));        
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
