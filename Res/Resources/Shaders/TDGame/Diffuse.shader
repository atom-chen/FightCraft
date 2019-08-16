Shader "TDGame/Diffuse" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_Emission ("Emission", Range(0,1)) = 0.5
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
}

SubShader {
	Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
	LOD 200
	Cull off
CGPROGRAM

#pragma surface surf SimpleLambert alphatest:_Cutoff

half4 LightingSimpleLambert(SurfaceOutput s, half3 lightDir, half atten) {
		half NdotL = dot(s.Normal, lightDir);
		half4 c;
		c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten) + s.Albedo * s.Emission;
		c.a = s.Alpha;
		return c;
		//return half4(s.Albedo, s.Alpha);

	}

sampler2D _MainTex;
sampler2D _AlphaTex;
fixed4 _Color;
fixed _Emission;

struct Input {
	float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
	//c = c * _Color + c * _Emission;
	o.Albedo = c.rgb * _Color;
	o.Alpha = c.a;
	o.Emission = c.rgb *_Emission;
}
ENDCG

}

	Fallback "Transparent/Cutout/Diffuse"
}
