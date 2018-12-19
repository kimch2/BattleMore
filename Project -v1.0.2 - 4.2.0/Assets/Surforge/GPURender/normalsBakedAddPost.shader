// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/NormalsBakedAddPost" {
 	Properties {
 		_MainTex ("", 2D) = "white" {}
 		
 		_BumpMap ("Normal(RGB) Alpha(A)", 2D) = "bump" {}
 		_SurforgeNormal ("Surforge Normal", 2D) = "bump" {}
	}
 
	SubShader {
 
		ZTest Always Cull Off ZWrite Off Fog { Mode Off } 
 
 		Pass{
  			CGPROGRAM
  			#pragma vertex vert
  			#pragma fragment frag
  			#include "UnityCG.cginc" 
  			#pragma target 3.0


			struct float25 {
  				float mat[25];
  			};

    
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
  			sampler2D _BumpMap;
  			sampler2D _SurforgeNormal;


  			float25 GetData(in int channel, v2f vi) {
   				float dxtex = 1.0 / 1024.0;  
  				float dytex = 1.0 / 1024.0;
  				
  				struct float25 array;
   				int k = -1;
   				[unroll]
  				for (int i=-2; i<3; i++) {  
  					[unroll] 
     				for(int j=-2; j<3; j++) {    
        				k++;    
        				
        				float4 col = tex2D(_BumpMap, vi.uv + float2((i)*dxtex, (j)*dytex));
  
       					if (channel == 0) array.mat[k] = col.r;
       					if (channel == 1) array.mat[k] = col.g;
       					if (channel == 2) array.mat[k] = col.b;
      				}
  				}
   				return array;
			}
			
			float GetDeviation(in float matr[25], in float matg[25], in float matb[25]) { 
				struct float25 array;
				
				float dr = 0;
				float dg = 0;
				float db = 0;
				
				[unroll]
   				for (int i=0; i<25; i++) {
					
					//curvature	   				
      				//dr = dr + abs(matr[0] - matr[i]);
      				//dg = dg + abs(matg[0] - matg[i]);
      				//db = db + abs(matb[0] - matb[i]);
      				
      				dr = dr + abs(0.5 - matr[i]);
      				dg = dg + abs(0.5 - matg[i]);
      				db = db + abs(1.0 - matb[i]);
   				}
   				//dr = dr / 25.0;
   				//dg = dg / 25.0;
   				//db = db / 25.0;
   				
   				float dfinal = (dr + dg+ db) - 1.0;
   				
   				//float dfinal = (dr + dg+ db);  //curvature
   				
   				//float dfinal = (dr + dg+ db);
   				 
   				float dresult = lerp(1, 0, dfinal);
   				
   				//float result = 0;
   				//if (dfinal < 0.0001) result = 1.0;
   				//else result = 0;
   				
   				//return result;
   				//return dfinal;
   				return dresult;
			}

 
  			fixed4 frag (v2f i) : COLOR{

  				fixed4 n1 = tex2D (_BumpMap, i.uv);
  				
  				struct float25 matr = GetData(0, i);	
     			struct float25 matg = GetData(1, i);	
     			struct float25 matb = GetData(2, i);
  				
  				float d = GetDeviation(matr, matg, matb);
  				fixed4 delta = fixed4 (d, d, d, 1);
  				delta = clamp (delta, 0, 1);


  				//float4 n1 =lerp(ns, fixed4 (0.5, 0.5, 1, 1), delta);
  				
  				fixed4 n2 = tex2D (_SurforgeNormal, i.uv);
  				
  				//RNM blending   		
     		 	float3 t = n1.xyz*float3( 2,  2, 2) + float3(-1, -1,  0);
    		 	float3 u = n2.xyz*float3(-2, -2, 2) + float3( 1,  1, -1);
    		 	float3 r = t*dot(t, u) - u*t.z;
    		 	float3 final = normalize(r);
    		 	
    		 	final = (final + 1) * 0.5;
   				return clamp(fixed4 (final.x, final.y, final.z, 1), 0, 1);
   				
   				//return delta;
  			}
  			ENDCG
 		}
	} 
 	FallBack "Diffuse"
}