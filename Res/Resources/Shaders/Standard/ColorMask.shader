Shader "HxageShaders/ColorMask"
{
	Properties
	{
		_ColorDiffuse ("Main Color", Color) = (1,1,1,1)
		_ColorMask ("Mask Color", Color) = (1,1,1,1)
		_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Shininess ("Shininess", Range (0.01, 1)) = 0.078125
		_MainTex ("Diffuse (RGB), Specular (A)", 2D) = "white" {}
		_MaskMap ("Mask map", 2D) = "white" {}
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	}

	// Simple quality -- drop the normal map
	SubShader
	{
		Tags {
			"Queue" = "Transparent-99"
			"IgnoreProjector"="True"
			"RenderType" = "TreeTransparentCutout"
		}
		Cull Off

		//Pass
		//{
			Alphatest Greater [_Cutoff]

			CGPROGRAM
			#pragma surface surf PPL

			sampler2D _MainTex;
			sampler2D _MaskMap;
			fixed4 _ColorDiffuse;
			fixed4 _ColorMask;
			float _Shininess;

			struct Input
			{
				float2 uv_MainTex;
			};

			// Forward lighting
			half4 LightingPPL (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
			{
				half3 nNormal = normalize(s.Normal);
				half shininess = s.Gloss * 250.0 + 4.0;

			#ifndef USING_DIRECTIONAL_LIGHT
				lightDir = normalize(lightDir);
			#endif

				// Phong shading model
				half reflectiveFactor = max(0.0, dot(-viewDir, reflect(lightDir, nNormal)));

				// Blinn-Phong shading model
				//half reflectiveFactor = max(0.0, dot(nNormal, normalize(lightDir + viewDir)));

				half diffuseFactor = max(0.0, dot(nNormal, lightDir));
				half specularFactor = pow(reflectiveFactor, shininess) * s.Specular;

				half4 c;
				c.rgb = (s.Albedo * diffuseFactor + _SpecColor.rgb * specularFactor) * _LightColor0.rgb;
				c.rgb *= (atten * 2.0);
				c.a = s.Alpha;
				return c;
			}

			void surf (Input IN, inout SurfaceOutput o)
			{
				half4 col 	= tex2D(_MainTex, IN.uv_MainTex);
				half4 col1  = tex2D(_MaskMap, IN.uv_MainTex);

				o.Albedo 	= col1.rgb * _ColorMask.rgb * col1.a + col.rgb * _ColorDiffuse.rgb * (1 - col1.a);
				o.Alpha 	= col.a;
				o.Gloss 	= _Shininess;
				o.Specular 	= 100.0f;
			}
			ENDCG
		//}
	}
	Fallback "Diffuse"
}