// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/CombineLabelsAlphaPost" {
	Properties {
		_MainTex ("", 2D) = "white" {}
	
 		_Labels ("Labels", 2D) = "white" {}
 		_LabelsAlpha ("LabelsAlpha", 2D) = "white" {}
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
  			
  			sampler2D _Labels; 
  			sampler2D _LabelsAlpha; 
   
 
  			fixed4 frag (v2f i) : COLOR{
				fixed4 labels = tex2D(_Labels, i.uv );
				fixed4 labelsAlpha = tex2D(_LabelsAlpha, i.uv );

				
				fixed4 resultColor = fixed4 (labels.r, labels.g, labels.b, labelsAlpha.r);

				resultColor = clamp(resultColor, 0, 1);
     			
   				return resultColor;
  			}
  			ENDCG
 		}
	} 
 	FallBack "Diffuse"
}