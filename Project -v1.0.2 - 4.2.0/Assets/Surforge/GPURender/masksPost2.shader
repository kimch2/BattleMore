// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/MasksPost2" {
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
				fixed4 c = clamp(tex2D(_MainTex, i.uv ), 0, 1);
				fixed4 resultColor = fixed4 (0,0,0,0);

		
				
				if ( (c.r > 0.5) && (c.g < 0.5) && (c.b > 0.5)) {
					resultColor = fixed4(1, 0, 0, 0);
				}
				if ( (c.r < 0.5) && (c.g > 0.5) && (c.b > 0.5)) {
					resultColor = fixed4(0, 1, 0, 0);
				}
				if ( (c.r > 0.5) && (c.g > 0.5) && (c.b > 0.5)) {
					resultColor = fixed4(0, 0, 1, 0);
				}
				if ( (c.r < 0.5) && (c.g < 0.5) && (c.b < 0.5)) {
					resultColor = fixed4(0, 0, 0, 1);
				}
				
				
				
	
   				return clamp(resultColor, 0, 1);
  			}
  			ENDCG
 		}
	} 
 	FallBack "Diffuse"
}