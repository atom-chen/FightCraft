// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "TDGame/RoleShadow" {
	Properties {
		_Range ("Range", FLOAT) = 0.2
		_RangeInner ("RangeInner", FLOAT) = 45
		_ShadowColor ("Shadow Color", COLOR)  = ( 1 , 0 , 0 , 0)
	}

	SubShader
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		pass
		{
			AlphaTest NotEqual 0
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			uniform half _Range;
			uniform half _RangeInner;
			uniform half4 _ShadowColor;

            struct fragmentInput{
                half4 position : SV_POSITION;
				half4 modelPos : TEXCOORD0; 
            };

            fragmentInput vert(float4 v:POSITION) 
			{
				fragmentInput o;
				o.position = UnityObjectToClipPos (v);
				o.modelPos = v;
                return o;
            }

            fixed4 frag(fragmentInput i) : COLOR 
			{
				half dis = distance(half3(i.modelPos.x, i.modelPos.y, i.modelPos.z), half3(0,0,0));
				if(dis < _RangeInner)
				{
					return _ShadowColor;
				}
				else if(dis > _Range)
				{
					return half4(0,0,0,0);
				}
				else
				{
					half alpha = 1 - (dis - _RangeInner) / (_Range - _RangeInner);
					return half4(_ShadowColor.x,_ShadowColor.y,_ShadowColor.z,alpha * _ShadowColor.w);
				}
            }

			ENDCG
		}
	}
}