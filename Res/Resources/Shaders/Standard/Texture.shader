Shader "HxageShaders/Texture" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 100
		
		Pass {
			Lighting Off
			SetTexture [_MainTex] { combine texture } 
		}
	} 
}

