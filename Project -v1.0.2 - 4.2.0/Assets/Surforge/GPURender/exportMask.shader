// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/ExportMask" {
	Properties {
 		_MainTex ("", 2D) = "white" {}
 		
 		_Masks ("Masks", 2D) = "white" {}
 		_Masks2 ("Masks2", 2D) = "white" {}
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
  			
  			sampler2D _Masks; 
  			sampler2D _Masks2; 

 
  			fixed4 frag (v2f i) : COLOR{
				fixed4 masks = tex2D(_Masks, i.uv );
				fixed4 masks2 = tex2D(_Masks2, i.uv );
				
				fixed4 c1 = fixed4 (0, 1, 0.5, 1);
				fixed4 c2 = fixed4 (0, 1, 1, 1);
				fixed4 c3 = fixed4 (0, 0.5, 0.5, 1);
				fixed4 c4 = fixed4 (0, 0.5, 1, 1);
				
				fixed4 c5 = fixed4 (1, 0.5, 0, 1);
				fixed4 c6 = fixed4 (0.5, 0, 1, 1);
				fixed4 c7 = fixed4 (1, 1, 0.5, 1);
				fixed4 c8 = fixed4 (0, 0, 0, 1);
				
				
				//fixed4 resultColor = c1 * masks.r + c2 * masks.g + c3 * masks.b + c4 * masks.a + c5 * masks2.r + c6 * masks2.g + c7 * masks2.b + c8 * masks2.a;
				
				fixed4 resultColor = c1 * masks.r;
				resultColor = resultColor * (1 - masks.g) + c2 * masks.g;
				resultColor = resultColor * (1 - masks.b) + c3 * masks.b;
				resultColor = resultColor * (1 - masks.a) + c4 * masks.a;
				
				resultColor = resultColor * (1 - masks2.r) + c5 * masks2.r;
				resultColor = resultColor * (1 - masks2.g) + c6 * masks2.g;
				resultColor = resultColor * (1 - masks2.b) + c7 * masks2.b;
				resultColor = resultColor * (1 - masks2.a) + c8 * masks2.a; 
				
				
	
   				return clamp(resultColor, 0, 1);
  			}
  			ENDCG
 		}
	} 
 	FallBack "Diffuse"
}