Shader "TYImage/WeaponLight"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		[ToggleOff] _PosX("_PosX", Float) = 1.0
		_LightPosXMin("_LightPosMin", float) = -1.65 
		_LightPosXMax("_LightPosMax", float) = -0.3
		_LightRange("LightRange", float) = 0.5
		_LightInterval("LightInterval", float) = 0.5
		_TimeEm("TimeEm", float) = 0.5
	}
	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			ZWrite On
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
					// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 objVertex: COLOR;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _PosX;
			float _LightPosXMin;
			float _LightPosXMax;
			float _LightRange;
			float _LightInterval;
			float _TimeEm;


			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.objVertex = v.vertex;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = half4(1, 1, 1,1);
				float delta = _LightPosXMin - _LightPosXMax;
				//float timePos = _Time.y / delta;
				float timePos = fmod(_Time.y, _LightInterval) / _LightInterval;

				if (_PosX == 1)
				{
					if (/*i.objVertex.x > _LightPosXMin && */i.objVertex.x + _LightPosXMin > timePos * delta - _LightRange
						&& /*i.objVertex.x < _LightPosXMax && */i.objVertex.x + _LightPosXMin < timePos * delta + _LightRange)
					{
						col = tex2D(_MainTex, i.uv);
						col.a *= ((_LightRange - abs(i.objVertex.x + _LightPosXMin - timePos * delta)) / _LightRange);
					}
					else
					{
						col.a = 0;
					}
				}
				else
				{
					if (/*i.objVertex.x > _LightPosXMin && */i.objVertex.y + _LightPosXMin > timePos * delta - _LightRange
						&& /*i.objVertex.x < _LightPosXMax && */i.objVertex.y + _LightPosXMin < timePos * delta + _LightRange)
					{
						col = tex2D(_MainTex, i.uv);
						col.a *= ((_LightRange - abs(i.objVertex.y + _LightPosXMin - timePos * delta)) / _LightRange);
					}
					else
					{
						col.a = 0;
					}
				}
				return col;
			}
			ENDCG
		}
	}
}