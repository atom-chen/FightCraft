Shader "TDGame/ImageNormal" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
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
			ZWrite Off			
            CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			sampler2D _MainTex;

			struct appdata {
			    float4 vertex : POSITION;				
			    float4 texcoord : TEXCOORD0;
			};
			struct v2f {
			    float4 pos : SV_POSITION;				
			    float4 uv : TEXCOORD0;
			};

			v2f vert (appdata v) 
            {
			    v2f o;
			    o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
			    o.uv = float4( v.texcoord.xy, 0, 0 );				
			    return o;
			}
			half4 frag( v2f i ) : COLOR 
			{
				fixed4 c = tex2D (_MainTex, i.uv.xy);								
			    return fixed4(c.xyz,1);
			}
			ENDCG
		}
	}
	
	FallBack "Diffuse"
}
