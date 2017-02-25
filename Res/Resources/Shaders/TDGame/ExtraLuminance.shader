Shader "TDGame/ExtraLuminance" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
		_Luminance ("Luminance", float) = 0
		_TintMask ("Tint Mask", Color) = (1,1,1,1)
	}
	SubShader {
        Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf TintMask

		uniform sampler2D _MainTex;
        uniform fixed _Luminance;
        uniform half4 _TintMask;
        struct Input {
            float2 uv_MainTex;
        };

        fixed4 LightingTintMask(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
            fixed diff = max (0, dot (s.Normal, lightDir));
            fixed4 c;
            c.rgb = s.Albedo * _LightColor0.rgb * (diff * atten * 2) + _TintMask;
            c.a = s.Alpha;
            return c;
        }

		void surf (Input IN, inout SurfaceOutput o) {
            half4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb * (1 + _Luminance);
            o.Alpha = c.a;
        }
		
		ENDCG
	}
	Fallback "Diffuse"
}