Shader "TDGame/Particles/MaskAlphaBlended" {
	Properties {
		_MainTex ("Particle Texture (RGB)", 2D) = "white" {}
        _MainTex_Alpha ("Particle Alpha Texture (RGB)", 2D) = "white" {}
        _MaskTex ("Mask Texture (RGB)", 2D) = "white" {}
        _MaskTex_Alpha ("Mask Alpha Texture (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off 
        Lighting Off 
        ZWrite Off 
        Fog { Color (0,0,0,0) }
		Pass 
	        {
				CGPROGRAM
				    #pragma vertex vert
                    #pragma fragment frag
			
				    #include "UnityCG.cginc"
			
                     uniform sampler2D _MainTex;
                     uniform sampler2D _MainTex_Alpha;    
				     uniform float4 _MainTex_ST;

                     uniform sampler2D _MaskTex;
                     uniform sampler2D _MaskTex_Alpha;    
				     uniform float4 _MaskTex_ST;
				     

                     struct vertexInput {
                        float4 vertex : POSITION;
                        fixed4 color : COLOR;
                        float2 texcoord : TEXCOORD0;
                        float2 texcoord1 : TEXCOORD1;
                    };

                    struct vertexOutput {
                        float4 pos : POSITION;
                        fixed4 color : COLOR;
                        float2 uv : TEXCOORD0;
                        float2 uv1 : TEXCOORD1;
                    };

                    vertexOutput vert(vertexInput v) {
                        vertexOutput o;
                        o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                        o.color = v.color;
                        o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                        o.uv1 = TRANSFORM_TEX(v.texcoord1, _MaskTex);
                        return o;
                    }
			
			        fixed4 frag(vertexOutput i) : COLOR {
                        float4 mainClr = tex2D(_MainTex, i.uv);
                        float4 mainClrA = tex2D(_MainTex_Alpha, i.uv);                        

                        float4 maskClr = tex2D(_MaskTex, i.uv1);
                        float4 maskClrA = tex2D(_MaskTex_Alpha, i.uv1);
                        float4 clr = i.color * maskClr * mainClr;
         	
         		        return float4(clr.rgb,maskClrA.r * mainClrA.r);
                    }
			   ENDCG
		   }
	} 
	FallBack "Diffuse"
}
