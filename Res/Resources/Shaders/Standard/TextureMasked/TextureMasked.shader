Shader "HxageShaders/TextureMasked"
{
   Properties
   {
      _MainTex ("Base (RGB)", 2D) = "white" {}
      _Mask ("Culling Mask", 2D) = "white" {}
      _AlphaMask ("Alpha Mask", 2D) = "white" {}
      _Cutoff ("Alpha cutoff", Range (0,1)) = 1
   }
   SubShader
   {
   	  Tags {"Queue"="Transparent"}   
      Pass
      {
      	SetTexture [_MainTex] {combine texture}      
      }
      
      Pass
      {  
      	 Blend SrcAlpha OneMinusSrcAlpha
         AlphaTest GEqual [_Cutoff]         
         SetTexture [_Mask] {combine texture}
         SetTexture [_AlphaMask] {combine texture lerp(texture) previous}
      }
     
      
   }
}