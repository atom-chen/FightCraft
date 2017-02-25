Shader "HxageShaders/CameraGray" {
	Properties {
		_MainTex ("Base (ARGB)", 2D) = "white" {}
		_Alpha("Alpha", Range(0, 1)) = 1
		_Factor("Factor", Range(0,1)) = 1
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
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			ColorMask RGB
			AlphaTest Greater .01
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse


			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			sampler2D _MainTex;
			float _Alpha;
			float _Factor;

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
			};
			v2f vert (appdata v) {
			    v2f o;
			    o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
			    o.uv = float4( v.texcoord.xy, 0, 0 );
				o.color = v.color;
			    return o;
			}
			half4 frag( v2f i ) : COLOR 
			{
				float4 c = tex2D (_MainTex, i.uv.xy);
				float4 ret = (c.r + c.g + c.b) / 3 * _Factor + c * (1.0 - _Factor);
				ret.a = _Alpha * i.color.a;
			    return ret;
			}
			ENDCG
		}
	}
	
	FallBack "Diffuse"
}
