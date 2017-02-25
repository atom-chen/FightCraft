Shader "HxageShaders/FogOfWarShader" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "black" {}
	}
	SubShader {
		BindChannels {
		   Bind "Vertex", vertex
		   Bind "Color", color   
		   Bind "texcoord", texcoord
		} 
        Tags { "Queue" = "Overlay" }
        Pass {
			ZTest Off
			Blend SrcAlpha OneMinusSrcAlpha
            //SetTexture [_MainTex] { combine texture * primary }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			sampler2D _MainTex;

			float2 TexSize;

			struct vIn 
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};
			struct v2f 
			{
				float4 position : POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert(vIn _in)
			{
				v2f output;
				output.position = mul(UNITY_MATRIX_MVP,_in.vertex);
				output.uv = _in.texcoord;
				return output;
			}

			float4 frag(v2f _in) : COLOR
			{
				float4 color = tex2D(_MainTex,_in.uv);
				color += tex2D(_MainTex,_in.uv + float2(0.0f / TexSize.x, 1.0f / TexSize.y));
				color += tex2D(_MainTex,_in.uv + float2(0.0f / TexSize.x, -1.0f / TexSize.y));
				color += tex2D(_MainTex,_in.uv + float2(1.0f / TexSize.x, 0.0f / TexSize.y));
				color += tex2D(_MainTex,_in.uv + float2(-1.0f / TexSize.x, 0.0f / TexSize.y));

				color /= 5;

				return color;
			}

			ENDCG
        }
    }

	FallBack "Diffuse"
}
