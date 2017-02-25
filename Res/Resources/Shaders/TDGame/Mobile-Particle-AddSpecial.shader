Shader "TDGame/Particles/AdditiveSpecial" {
	Properties {
		_MainTex ("Particle Texture", 2D) = "white" {}
		_CW ("Color Weight", Range (0, 1)) = 1
		_AW ("Alpha Weight", Range (0, 1)) = 1
	}
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }

		Blend SrcAlpha One
		
		ZWrite Off
		Cull Off
		Lighting Off 		
		Fog { Color (0,0,0,0) }
		
		BindChannels {
			Bind "Color", color
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
		}

		Pass {
			SetTexture[_MainTex]
			{			
				Combine texture * primary
			}
			SetTexture[_MainTex]
	        {	  
	        	ConstantColor ([_CW],[_CW],[_CW],[_AW])          
	            combine previous * constant DOUBLE
	        }
		}
	}
	Fallback "Mobile/Particles/Additive"
}