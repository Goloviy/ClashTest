Shader "Unlit/Unlit_Transparent_Tint_Shader"
{
     Properties {
         _MainTex ("Base (RGB) Trans (A)", 2D) = "w$$anonymous$$te" {}
         [PerRendererData] _Color6 ("Tint Color", Color) = (0, 0, 0, 1)
         //_State("State Tint", float) = 0.0
     }
 
     SubShader {
         Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
         LOD 100
 
         ZWrite Off
         Blend SrcAlpha OneMinusSrcAlpha
 
         Pass {
             CGPROGRAM
                 #pragma vertex vert
                 #pragma fragment frag
                 #pragma target 2.0
                #pragma multi_compile_instancing
                 #include "UnityCG.cginc"
 
                 struct appdata_t {
                     float4 vertex : POSITION;
                     fixed4 color    : COLOR;
                     float2 texcoord : TEXCOORD0;
                     UNITY_VERTEX_INPUT_INSTANCE_ID
                 };
 
                 struct v2f {
                     float4 vertex : SV_POSITION;
                     fixed4 color    : COLOR;
                     float2 texcoord : TEXCOORD0;
                     UNITY_VERTEX_OUTPUT_STEREO
                 };
 
                fixed4 _Color6;
                //float _State;
                sampler2D _MainTex;
                float4 _MainTex_ST;

                v2f vert(appdata_t IN)
                {
                    v2f OUT;
                    OUT.vertex = UnityObjectToClipPos(IN.vertex);
                    //if (_State < 0.1){
                        OUT.texcoord = TRANSFORM_TEX(IN.texcoord, _MainTex);
                        UNITY_TRANSFER_FOG(OUT,OUT.vertex);
                    //}
                    //else{
                    //    OUT.texcoord = IN.texcoord;
                    //    OUT.color = IN.color * _Color6;
                    //    #ifdef PIXELSNAP_ON
                    //    OUT.vertex = UnityPixelSnap (OUT.vertex);
                    //    #endif
                    //}

 
                    return OUT;
                }
  
                fixed4 frag(v2f IN) : SV_Target
                {  
				    fixed4 col = tex2D(_MainTex, IN.texcoord);
				    if (col.a > 0.5){
					    col += _Color6;
					    col.a = _Color6.a;
				    }
				    return col;
                }
             ENDCG
         }
     }
 
 }
