// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/BucketApplyPadding" {
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
   			
   			float4 _MainTex_ST;
			
			
  			sampler2D _MainTex; 

			
  			v2f vert (appdata_img v){
   				v2f o;
  				o.pos = UnityObjectToClipPos (v.vertex);

   				float2 boffset = float2 (0.1, 0.1);
  
   				o.uv =  v.texcoord.xy * _MainTex_ST.xy * 0.8 + _MainTex_ST.zw + boffset;
   				
   				return o; 
  			}
    
 
  			fixed4 frag (v2f i) : COLOR{
				fixed4 c = clamp(tex2D(_MainTex, i.uv ), 0, 1);
				fixed4 resultColor = fixed4 (c.r, c.g, c.b, c.a);

   				return clamp(resultColor, 0, 1);
  			}
  			ENDCG
 		}
	} 
 	FallBack "Diffuse"
}