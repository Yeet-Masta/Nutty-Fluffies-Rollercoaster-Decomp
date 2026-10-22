Shader "SimpleTexture" {
Properties {
 _MainTex ("Texture", 2D) = "" {}
}
SubShader { 
 Tags { "Queue"="Background" "IgnoreProjector"="True" "RenderType"="Opaque" }
 Pass {
  Tags { "Queue"="Background" "IgnoreProjector"="True" "RenderType"="Opaque" }
  ZTest Always
  ZWrite Off
  Cull Off
  Fog { Mode Off }
  SetTexture [_MainTex] { combine texture }
 }
}
}