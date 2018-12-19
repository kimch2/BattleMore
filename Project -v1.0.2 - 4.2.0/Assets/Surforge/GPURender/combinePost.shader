// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/CombinePost" {
	Properties {
 		_MainTex ("", 2D) = "white" {}
 		
 		_Ao ("Ao", 2D) = "white" {}
 		_AoFloaters ("AoFloaters", 2D) = "white" {}
 		_Edges ("Edges", 2D) = "white" {}
 		_Depth ("Depth", 2D) = "white" {}
 		
 		_ModelBakedAO ("ModelBakedAO", 2D) = "white" {}
 		_ModelBakedEdgeMap ("ModelBakedEdgeMap", 2D) = "white" {}
 		
 		_FloatersMask ("FloatersMask", 2D) = "white" {}
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
  			
  			sampler2D _Ao; 
  			sampler2D _AoFloaters; 
  			sampler2D _Edges; 
  			sampler2D _Depth;
  			sampler2D _ModelBakedAO;
  			sampler2D _ModelBakedEdgeMap;
  			
  			sampler2D _FloatersMask; 
   
 
  			fixed4 frag (v2f i) : COLOR{
				fixed4 ao = tex2D(_Ao, i.uv );
				
				fixed4 aoFloaters = tex2D(_AoFloaters, i.uv );
				
				fixed4 edges = tex2D(_Edges, i.uv );
				fixed4 modelBakedAO = tex2D(_ModelBakedAO, i.uv );
				fixed4 modelBakedEdgeMap = tex2D(_ModelBakedEdgeMap, i.uv );
				
				fixed4 heightmap = tex2D(_Depth, i.uv );
				
				fixed4 floatersMask = clamp(tex2D(_FloatersMask, i.uv ), 0, 1);
				
		
				aoFloaters = (1 - ((1 - aoFloaters) * 0.5)) ;
				aoFloaters = aoFloaters * aoFloaters;
				aoFloaters = clamp(aoFloaters, 0, 1);
				
				fixed4 resultColor = 1;
				
				
				resultColor.r = ao.r * (1 - floatersMask.r) + aoFloaters.r * floatersMask.r;
				
				
				resultColor.g = edges.r + (1 - modelBakedEdgeMap);
				
				resultColor.b = pow(resultColor.r * 1.05f, 4) * resultColor.r;
				
				resultColor.r = resultColor.r * modelBakedAO.r;
				
				resultColor.a = heightmap.r;

				resultColor = clamp(resultColor, 0, 1);
     			
   				return resultColor;
  			}
  			ENDCG
 		}
	} 
 	FallBack "Diffuse"
}