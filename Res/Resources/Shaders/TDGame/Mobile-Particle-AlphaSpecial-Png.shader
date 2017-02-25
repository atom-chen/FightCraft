  Shader "TDGame/Particles/AlphaSpecial_PNG"
 {
 
    Properties
    {
        _CW ("Color Weight", Range (0, 1)) = 1
		_AW ("Alpha Weight", Range (0, 1)) = 1
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    Category {
	   

		Tags{"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
	    LOD 200
	    Blend SrcAlpha OneMinusSrcAlpha
	    ZWrite Off
		Cull Off
		Lighting Off 		
		Fog {  Color (0,0,0,0) }

		BindChannels {
			Bind "Color", color
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
		}

	    SubShader
	    {
	        Pass
	        {
		        SetTexture[_MainTex]
		        {
		        	combine texture * primary
		        }

		        SetTexture[_MainTex]
		        {	  
		        	ConstantColor ([_CW],[_CW],[_CW],[_AW])          
		            combine previous * constant DOUBLE
		        }
		        
	        }
	    }
	    Fallback "Mobile/Particles/Alpha Blended"
	}
}