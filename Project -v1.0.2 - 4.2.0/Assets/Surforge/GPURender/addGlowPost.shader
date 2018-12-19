// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/AddGlowPost" {
	Properties {
		_MainTex ("", 2D) = "white" {}
	
 		_Emission ("Emission", 2D) = "white" {}
 		_Glow ("Glow", 2D) = "white" {}
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
  			
  			sampler2D _Emission; 
  			sampler2D _Glow; 
   
 
  			fixed4 frag (v2f i) : COLOR{
				fixed4 emissionTex = tex2D(_Emission, i.uv );
				fixed4 glowTex = tex2D(_Glow, i.uv );

				
				fixed4 resultColor = 1;
				
				resultColor = emissionTex + glowTex * 0.5;
				//resultColor = emissionTex;

				resultColor = clamp(resultColor, 0, 1);
     			
   				return resultColor;
  			}
  			ENDCG
 		}
	} 
 	FallBack "Diffuse"
}