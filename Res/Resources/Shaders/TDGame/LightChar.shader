Shader "TDGame/LightChar" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _AlphaTex ("Alpha (RGB)", 2D) = "white" {}
        _HighLightColor ("High Light", Color) = (0.7,0.7,0.7,1)
        _PowValue("Pow Value" , float) = 0
        _Illum ("Illumin (A)", 2D) = "white" {}
        _LightColor("Light Color" , Color) = (1,1,1,1)
        _LightDir("Light Direction" , Vector) = (20,90,30,0)
        _Cutoff("Cut Alpha" , Range(0,1)) = 0.6
    }
    SubShader {	
        Tags { "Queue" = "Transparent-10" }
        Cull off

        CGPROGRAM            
            #pragma surface surf SimpleLambert noforwardadd alphatest:_Cutoff
            sampler2D _MainTex; 
            sampler2D _AlphaTex;
            sampler2D _Illum;
            fixed _Outline;
            fixed4 _OutlineColor;
            fixed4 _Color;	
            fixed4 _HighLightColor;
            half _PowValue;
            fixed4 _LightColor;
            half4 _LightDir;

            struct Input
            {
                half2 uv_MainTex;
            };


            half4 LightingSimpleLambert (SurfaceOutput s, half3 lightDir, half atten)
            {                
                half NdotL = s.Specular;
                half4 c;
                half3 temp = s.Albedo * (_HighLightColor.rgb + _LightColor.rgb) * (NdotL * pow(NdotL , _PowValue)  *2) + s.Albedo * min(_PowValue/10,0.3);
                c.rgb = temp;
                c.a = s.Alpha;
                return c;		
            }

            void surf (Input IN, inout SurfaceOutput o) 
            {
                fixed4 texColor = tex2D(_MainTex , IN.uv_MainTex) * _Color;                
                o.Albedo = texColor;
                o.Emission = texColor.rgb * tex2D(_Illum , IN.uv_MainTex).a;
                o.Alpha =  tex2D(_AlphaTex, IN.uv_MainTex).r * _Color.a ;
                o.Specular = max(dot(normalize(o.Normal) , normalize(_LightDir.xyz)),0.001);
            }
        ENDCG
    }
    //} 
    FallBack "Diffuse"
}

