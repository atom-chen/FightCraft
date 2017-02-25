//这个脚本和脚本相关的shader就是个杯具
//现在我已经看不懂shader和脚本了
//任何修改的企图都是徒劳的
//如果他跑的好好的，那就不用管它
//如果他跑歪了，那就毫不犹豫的换掉！

Shader "TDGame/SelectCircle1" {
	Properties {
		_Range ("Range", FLOAT) = 0.2
		_Angle ("Angle", FLOAT) = 45
		_Circle1Color ("Circle1 Color", COLOR)  = ( 1 , 0 , 0 , 0)
		_HaloDis ("HaloDis", FLOAT) = 0
		_AngleTan ("AngleTan", FLOAT) = 0
		_LineDisParamRecip ("LineDisParam", FLOAT) = 1
	}

	SubShader
	{
		Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
		pass
		{
			ZWrite Off
			ZTest Always
			Blend SrcAlpha OneMinusSrcAlpha 

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			uniform half _Range;
			uniform half _Angle;
			uniform half4 _Circle1Color;
			uniform half _HaloDis;
			uniform half _AngleTan;
			uniform half _LineDisParamRecip;

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
				if(disToOutLine < 0.57)
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
					return circle_alpha(_HaloDis - dis);
				else
					return 0;
			}

			fragmentInput vert(vertexInput i)
			{
                fragmentInput o;
                o.position = mul (UNITY_MATRIX_MVP, i.vertex);
				o.modelPos = i.vertex;
                return o;
            }

            half4 frag(fragmentInput i) : COLOR 
			{
				//求点到原点的距离，超过范围的直接不考虑

				half dis = distance(half3(i.modelPos.x, 0, i.modelPos.z), half3(0,0,0));


				half angleX = i.modelPos.z * _AngleTan;
				half absPosX = abs(i.modelPos.x);

				half alpha1 = 0;
				half alphaTemp = 0;
					
				//最外一层半径颜色
				if(dis < _Range && absPosX < angleX)
				{
					//if((_Angle < 90 && absPosX < angleX)
					//	|| (_Angle >= 90 && absPosX > angleX))
					{
						alpha1 = circle_alpha(_Range - dis);
					}

					//圆切面两边颜色
					//if((_Angle < 90 && absPosX <= angleX)
					//	|| (_Angle >= 90 && absPosX >= angleX && _Angle < 180))
					{
						alphaTemp = circle_alpha(abs((angleX - absPosX)) * _LineDisParamRecip);
						if(alphaTemp > alpha1)
							alpha1 = alphaTemp;
					}

					//光环动画颜色
					//if((_Angle < 90 && absPosX < angleX)
					//	|| (_Angle >= 90 && absPosX > angleX))
					{
						alphaTemp = haloByInput(dis);
						if(alphaTemp > alpha1)
							alpha1 = alphaTemp;
					}
				}

				//
				return half4(_Circle1Color.x, _Circle1Color.y, _Circle1Color.z, alpha1);
            }

			ENDCG
		}
	}
}