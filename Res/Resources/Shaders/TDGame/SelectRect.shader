// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "TDGame/SelectRect" {
	Properties {
		_Width ("Width", FlOAT) = 0.2
		_Length1 ("Length1", FlOAT) = 1
		_Circle1Color ("Circle1 Color", COLOR)  = ( 1 , 0 , 0 , 0)
		_HaloDis ("HaloDis", FlOAT) = 0
	}

	SubShader
	{
		Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
		pass
		{
			ZWrite Off
			ZTest Always
			//AlphaTest NotEqual 0
			Blend SrcAlpha OneMinusSrcAlpha 

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#include "UnityCG.cginc"

			uniform half4 _Color;
			uniform half _Width;
			uniform half _Length1;
			uniform half4 _Circle1Color;
			uniform half _HaloDis;

			struct vertexInput {
                half4 vertex : POSITION;
                half4 texcoord0 : TEXCOORD0;
            };

            struct fragmentInput{
                half4 position : SV_POSITION;
				half4 modelPos : TEXCOORD0; 
            };

			//根据与边框的距离 计算颜色
			half circle_alpha(half disToOutLine)
			{
				if(disToOutLine > 0  && disToOutLine < 0.57)
				{
					return disToOutLine * (-1.67) + 0.95;
				}
				else 
				{
					return 0;
				}
			}

			//光环
			half haloByInput(half dis)
			{
				if(dis < _HaloDis)
				{
					return circle_alpha(_HaloDis - dis);
				}
				else
					return 0;
			}

			fragmentInput vert(vertexInput i){
                fragmentInput o;
                o.position = UnityObjectToClipPos (i.vertex);
				o.modelPos = i.vertex;
                return o;
            }

            half4 frag(fragmentInput i) : COLOR 
			{
				half alpha1 = 0;
				half tempAlpha = 0;

				if((i.modelPos.x <= _Width && i.modelPos.x >= - _Width) 
				&& (i.modelPos.z <= _Length1 && i.modelPos.z >= 0))
				{
					//前后边
					alpha1 = circle_alpha(_Length1 - abs(i.modelPos.z));

					//左右边
					tempAlpha = circle_alpha(_Width - abs(i.modelPos.x));
					if(tempAlpha > alpha1)
						alpha1 = tempAlpha;

					//动画
					half dis = abs(i.modelPos.x) + abs(i.modelPos.z) / 1.414;
					tempAlpha = haloByInput(dis);
					if(tempAlpha > alpha1)
						alpha1 = tempAlpha;					
				}

				return half4(_Circle1Color.x, _Circle1Color.y, _Circle1Color.z, alpha1);
            }

			ENDCG
		}
	}
}