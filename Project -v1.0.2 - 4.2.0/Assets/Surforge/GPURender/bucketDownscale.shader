// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/BucketDownscale" {
	Properties {
 		_MainTex ("", 2D) = "white" {}
 		_bucketX ("Bucket X", Range (0.0, 4.0)) = 0
 		_bucketZ ("Bucket Z", Range (0.0, 4.0)) = 0
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
   			
   			float4 _MainTex_ST;
			
			
  			sampler2D _MainTex; 
			float _ssMode;
			float _bucketX;
			float _bucketZ;
			
  			v2f vert (appdata_img v){
   				v2f o;
  				o.pos = UnityObjectToClipPos (v.vertex);

   				float2 boffset = float2 (_bucketX, _bucketZ);

   				o.uv =  v.texcoord.xy * _MainTex_ST.xy * 2  + _MainTex_ST.zw - boffset;

   				return o; 
  			}
    
 
  			fixed4 frag (v2f i) : COLOR{
				fixed4 c = clamp(tex2D(_MainTex, i.uv ), 0, 1);
				fixed4 resultColor = fixed4 (c.r, c.g, c.b, c.a);
				
				//fixed storeAlpha = resultColor.a; //
				
				resultColor.a = 1;

                resultColor.a *= (i.uv.x >= 0);
                resultColor.a *= (i.uv.x <= 1);
                resultColor.a *= (i.uv.y >= 0);
                resultColor.a *= (i.uv.y <= 1);
               
               
               
                resultColor.rgb *= resultColor.a;
                
               // storeAlpha *= resultColor.a; //
               // resultColor.a = storeAlpha;  //
	
   				return clamp(resultColor, 0, 1);
  			}
  			ENDCG
 		}
	} 
 	FallBack "Diffuse"
}