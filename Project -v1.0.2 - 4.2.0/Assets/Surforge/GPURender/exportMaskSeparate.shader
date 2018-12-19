// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/ExportMaskSeparate" {
	Properties {
 		_MainTex ("", 2D) = "white" {}
 		
 		_MaskID ("MaskID", Range (0.0, 7.0)) = 0
 		
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
  			
  			float _MaskID;
  			
  			sampler2D _Masks; 
  			sampler2D _Masks2; 

 
  			fixed4 frag (v2f i) : COLOR{
				fixed4 masks = tex2D(_Masks, i.uv );
				fixed4 masks2 = tex2D(_Masks2, i.uv );
				
				
				fixed4 c = fixed4 (0,0,0,1);
				fixed4 w = fixed4 (1,1,1,1);
				
				if (_MaskID == 0) c = w * masks.r;
				if (_MaskID == 1) c = w * masks.g;
				if (_MaskID == 2) c = w * masks.b;
				if (_MaskID == 3) c = w * masks.a;
				
				if (_MaskID == 4) c = w * masks2.r;
				if (_MaskID == 5) c = w * masks2.g;
				if (_MaskID == 6) c = w * masks2.b;
				if (_MaskID == 7) c = w * masks2.a;
				
				fixed4 resultColor = fixed4 (c.r, c.g, c.b, 1);
				
   				return clamp(resultColor, 0, 1);
  			}
  			ENDCG
 		}
	} 
 	FallBack "Diffuse"
}