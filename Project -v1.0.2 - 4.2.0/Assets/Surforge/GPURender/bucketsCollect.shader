// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/BucketsCollect" {
	Properties {
 		_MainTex ("", 2D) = "white" {}
 		_Result ("", 2D) = "white" {}
 		_Bucket ("", 2D) = "white" {}
 		
 		_IsFirstBucket ("Is First Bucket", Range (0.0, 1.0)) = 0
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
  			sampler2D _Result; 
  			sampler2D _Bucket; 
  			
  			float _IsFirstBucket;

 
  			fixed4 frag (v2f i) : COLOR{
				fixed4 result = clamp(tex2D(_Result, i.uv ), 0, 1);
				fixed4 bucket = clamp(tex2D(_Bucket, i.uv ), 0, 1);
				fixed4 resultColor = result + bucket;
				
				if (_IsFirstBucket == 1) {
					resultColor = bucket;
				}
				
   				return clamp(resultColor, 0, 1);
  			}
  			ENDCG
 		}
	} 
 	FallBack "Diffuse"
}