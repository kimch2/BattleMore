// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/BlurPost" {
	Properties {
 		_MainTex ("", 2D) = "white" {} 
	}
 
	SubShader {
 
		ZTest Always Cull Off ZWrite Off Fog { Mode Off } 
 
 		Pass{
  			CGPROGRAM
  			#pragma target 3.0
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
   
    		
    		
    		
    		float4 get_pixel(in float2 coords, in float dx, in float dy) { 
   				return tex2D(_MainTex, coords + float2(dx, dy));
			}
			
			float Convolve(in float kernel[25], in float matrixx[25], in float denom, in float offset) {
   				float res = 0.0;
   				for (int i=0; i<25; i++) {
     				res += kernel[i]*matrixx[i];
   				}
   				return clamp(res/denom + offset, 0.0, 1.0);
			}
			
			struct float25 {
  				float mat[25];
  			};
			
			
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
        				
        				float4 col = tex2D(_MainTex, vi.uv + float2((i)*dxtex, (j)*dytex));
  
       					if (channel == 0) array.mat[k] = col.r;
       					if (channel == 1) array.mat[k] = col.g;
       					if (channel == 2) array.mat[k] = col.b;
      				}
  				}
   				return array;
			}
			
			
			float25 GetMean(in float matr[25], in float matg[25], in float matb[25]) { 
				struct float25 array;
				[unroll]
   				for (int i=0; i<25; i++) {
      				array.mat[i] = (matr[i]+ matg[i]+ matb[i])/3.0;
   				}
   				return array;
			}
    		
    		
 
  			fixed4 frag (v2f i) : COLOR{

                                     
                float kerGausBlur[25] =  { { 2, 4, 5, 4, 2, }, 
                       					   { 4, 9, 12, 9, 4, }, 
                       					   { 5, 12, 15, 12, 5, }, 
                       					   { 4, 9, 12, 9, 4, }, 
                        				   { 2, 4, 05, 4, 2, }, };
         
                                     
     			
     			struct float25 matr = GetData(0, i);	
     			struct float25 matg = GetData(1, i);	
     			struct float25 matb = GetData(2, i);	
     			struct float25 mata = GetMean(matr.mat, matg.mat, matb.mat);   
				
				// Gaussian blur kernel
   				fixed4 resultColor = fixed4(Convolve(kerGausBlur,matr.mat,160.0, 0),
   				                    		Convolve(kerGausBlur,matg.mat,160.0, 0),
   				                    		Convolve(kerGausBlur,matb.mat,160.0, 0),1.0);

   
   				return resultColor + resultColor * 0.1;
  			}
  			ENDCG
 		}
	} 
 	FallBack "Diffuse"
}