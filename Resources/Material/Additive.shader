
Shader "TYImage/Particles/Additive" {
Properties {
 _MainTex ("Particle Texture", 2D) = "white" {}
}
SubShader { 
 Tags { "Queue" = "Opaque + 10" "RenderType" = "Opaque + 10" }
 Pass {
  Tags { "Queue" = "Opaque + 10" "RenderType" = "Opaque + 10" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "color", Color
   Bind "texcoord", TexCoord
  }
  ZWrite Off
  Cull Off
  ZTest Always
  Fog {
   Color (0,0,0,0)
  }
  Blend OneMinusSrcAlpha One
  SetTexture [_MainTex] { combine texture * primary }
 }
}
}