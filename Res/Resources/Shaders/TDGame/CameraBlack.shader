// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "TDGame/CameraBlack" {
	Properties {
		_MainTex ("Base (ARGB)", 2D) = "white" {}
		_Alpha("Alpha", Range(0, 1)) = 1
		_Factor("Factor", Range(0,1)) = 1
		_NoEffRange1("NoEffRange", Float) = -1
		_NoEffPos1X("NoEffPos1X", Float) = -1
		_NoEffPos1Y("NoEffPos1Y", Float) = -1
		_NoEffPos2X("NoEffPos2X", Float) = -1
		_NoEffPos2Y("NoEffPos2Y", Float) = -1
	}
	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			ColorMask RGB

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			
			sampler2D _MainTex;
			float _Alpha;
			float _Factor;

			float _NoEffRange;
			float _NoEffPos1X;
			float _NoEffPos1Y;
			float _NoEffPos2X;
			float _NoEffPos2Y;
			uniform float4 _MainTex_TexelSize;

			// vertex input: position, UV
			struct appdata {
			    float4 vertex : POSITION;
				float4 color : COLOR;
			    float4 texcoord : TEXCOORD0;
			};
			struct v2f {
			    float4 pos : SV_POSITION;
				float4 color : COLOR;
			    float4 uv : TEXCOORD0;
				float4 pos1 : TEXCOORD1;
			};

			v2f vert (appdata v) {
			    v2f o;
			    o.pos = UnityObjectToClipPos( v.vertex );
			    o.uv = float4( v.texcoord.xy, 0, 0 );
				o.color = v.color;
				o.pos1 = v.vertex;
				
			    return o;
			}

			half4 frag( v2f i ) : COLOR 
			{
				float4 c = tex2D (_MainTex, i.uv.xy);

				//计算不受影响的区域
				if(_NoEffRange > 0)
				{
					float dis1 = distance(float3(i.uv.xy * _MainTex_TexelSize.zw, 0), float3(_NoEffPos1X,_NoEffPos1Y,0));
					float dis2 = distance(float3(i.uv.xy * _MainTex_TexelSize.zw, 0), float3(_NoEffPos2X,_NoEffPos2Y,0));
					if(dis1 < _NoEffRange)
					{
						return c;
					}
					else if(dis2 < _NoEffRange)
					{
						return c;
					}
					else
					{
						float4 ret = c * _Factor;
						ret.a = _Alpha * i.color.a;
						return ret;
					}
				}

				float4 ret = c * _Factor;
				ret.a = _Alpha * i.color.a;
				return ret;
				
			}
			ENDCG
		}
	}
	
	FallBack "Diffuse"
}
