Shader "MenuSprite" {
Properties {
 _MainTex ("Texture", 2D) = "" {}
 _MainColor ("Main Color", Color) = (0.5,0.5,0.5,0.5)
}
SubShader { 
 LOD 100
 Tags { "Queue"="Overlay" "IgnoreProjector"="True" "RenderType"="Transparent" }
 Pass {
  Tags { "Queue"="Overlay" "IgnoreProjector"="True" "RenderType"="Transparent" }
  BindChannels {
   Bind "color", Color
   Bind "texcoord", TexCoord
  }
  ZTest Always
  ZWrite Off
  Cull Off
  Fog { Mode Off }
  Blend SrcAlpha OneMinusSrcAlpha
  SetTexture [_MainTex] { ConstantColor [_MainColor] combine constant * primary }
  SetTexture [_MainTex] { combine texture * previous double }
 }
}
SubShader { 
 LOD 100
 Tags { "Queue"="Overlay" "IgnoreProjector"="True" "RenderType"="Transparent" }
 Pass {
  Tags { "Queue"="Overlay" "IgnoreProjector"="True" "RenderType"="Transparent" }
  BindChannels {
   Bind "color", Color
   Bind "texcoord", TexCoord
  }
  ZTest Always
  ZWrite Off
  Cull Off
  Fog { Mode Off }
  Blend SrcAlpha OneMinusSrcAlpha
  SetTexture [_MainTex] { combine texture * primary }
 }
}
}