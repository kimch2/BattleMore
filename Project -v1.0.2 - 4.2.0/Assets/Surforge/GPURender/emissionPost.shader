// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/EmissionPost" {
	Properties {
 		_MainTex ("", 2D) = "white" {}
	}
 
	SubShader {
 
		ZTest Always Cull Off ZWrite Off Fog { Mode Off } 
 
 		Pass{
  			CGPROGRAM
  			#pragma vertex vert
  			#pragma fragment frag
  			#include "UnityCG.cginc" 

    
 			 struct v2f {
   				float4 pos : POSITION;
   				half2 uv : TEXCOORD0;
  			};
   

  			v2f vert (appdata_img v){
   				v2f o;
  				o.pos = UnityObjectToClipPos (v.vertex);
   				o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
   				return o; 
  			}
    
  			sampler2D _MainTex; 

 
  			fixed4 frag (v2f i) : COLOR{
				fixed4 c = tex2D(_MainTex, i.uv );
				fixed4 resultColor = 1;

				if ( (c.r > 0.5) || (c.g > 0.5) || (c.b > 0.5)) {
					resultColor.r = c.r;
					resultColor.g = c.g;
					resultColor.b = c.b;
					resultColor.a = 0;
				}
				else {
					resultColor = c;
				}

   				return clamp(resultColor, 0, 1);
  			}
  			ENDCG
 		}
	} 
 	FallBack "Diffuse"
}