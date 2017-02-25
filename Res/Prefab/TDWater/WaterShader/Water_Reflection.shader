Shader "Water/Water_RefletionTex" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Normal("Normal", 2D) = "bump" {}
		_TexAtlasTiling("xy for TexAtlasTiling.xy/zw , zw for Direction.xy" , Vector) = (1,1,1,1)
		//_TexRotation("Tex Rotation" , Vector) = (0,0,0,0)
		//_DirectionUv("Tex DirectionUv" , Vector) = (2.0,2.0,0,1.0)
		_ReflectionAlpha("Reflection Alpha" , Range(0,1.0)) = 0.5
		_ReflectionBias("Reflection Bias" , float) = 0.02
	}
	
	CGINCLUDE
	
	#include "UnityCG.cginc"
	sampler2D _MainTex;
	sampler2D _Normal;
	
	float4 _TexAtlasTiling;
	//float4 _TexRotation;
	//float4 _DirectionUv;
	half _ReflectionAlpha;
	half _ReflectionBias;

	struct v2f_My
	{
		half4 pos : SV_POSITION;
		half4 uv1 : TEXCOORD0;
		//half4 uv2 : TEXCOORD1;
		half4 c : COLOR;
	};
	ENDCG
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent+2" }
		LOD 200
		
		Blend SrcAlpha OneMinusSrcAlpha
		ZTest LEqual
		ZWrite Off
		Cull Off		
		
		pass
		{
			CGPROGRAM
			v2f_My vert(appdata_full v)
			{
				v2f_My o;
				o.pos = mul(UNITY_MATRIX_MVP , v.vertex);
				o.uv1 = v.texcoord.xyxy * _TexAtlasTiling.xyxy + frac((_Time.xxxx) * _TexAtlasTiling.zwzw);
				o.uv1.zw = v.texcoord.xy;
				o.c = v.color;
				//o.uv2.y = o.uv1.y - _TexAtlasTiling.w;
				return o;			
				
			}
			
			fixed4 frag(v2f_My i) : COLOR
			{
				fixed4 normalMap_zw = tex2D(_Normal, i.uv1.xy);
				
				fixed4 rtReflNorm = (normalMap_zw - 0.5) * _ReflectionBias;
			//fixed4 rtRefl = tex2D(_ReflectionTex , (i.screen.xy / i.screen.w) + rtReflNorm.xy);
				fixed4 rtRefl = tex2D(_MainTex ,i.uv1.zw+ rtReflNorm.xy);
				fixed t_ShadowAlpha = rtRefl.a * _ReflectionAlpha;
				rtRefl.a = 1;
				
				fixed4 colorFinal = (rtRefl * t_ShadowAlpha);
				
				colorFinal.a *= i.c.a;
				
				return colorFinal;
			}
			
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			
			ENDCG  
		}
		
	} 
	FallBack "Diffuse"
}
