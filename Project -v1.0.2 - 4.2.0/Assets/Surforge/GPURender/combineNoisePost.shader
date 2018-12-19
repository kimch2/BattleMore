// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/CombineNoisePost" {
	Properties {
		_MainTex ("", 2D) = "white" {}
	
 		_EmissionMap ("EmissionMap", 2D) = "white" {}
 		_Noise ("Noise", 2D) = "white" {}
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
  			
  			sampler2D _EmissionMap; 
  			sampler2D _Noise; 
   
 
  			fixed4 frag (v2f i) : COLOR{
				fixed4 emissionMap = tex2D(_EmissionMap, i.uv );
				fixed4 noise  = tex2D(_Noise, i.uv );

				
				fixed4 resultColor = fixed4 (emissionMap.r, emissionMap.g, noise.r, noise.b);

				resultColor = clamp(resultColor, 0, 1);
     			
   				return resultColor;
  			}
  			ENDCG
 		}
	} 
 	FallBack "Diffuse"
}