// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/EdgesCapturePost" {
	Properties {
 		_MainTex ("", 2D) = "white" {}
 		_NormalMap("", 2D) = "white" {}
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
  			sampler2D _NormalMap; 
   
    		
    		
    		
    		float4 get_pixel(in float2 coords, in float dx, in float dy) { 
   				return tex2D(_NormalMap, coords + float2(dx, dy));
			}
			
			float Convolve(in float kernel[9], in float matrixx[9], in float denom, in float offset) {
   				float res = 0.0;
   				for (int i=0; i<9; i++) {
     				res += kernel[i]*matrixx[i];
   				}
   				return clamp(res/denom + offset, 0.0, 1.0);
			}
			
			struct float9 {
  				float mat[9];
  			};
			
			
			float9 GetData(in int channel, v2f vi, int m) {
   				float dxtex = 1.0 / 512.0;  
  				float dytex = 1.0 / 512.0;
  				
  				struct float9 array;
   				int k = -1;
   				[unroll]
  				for (int i=-1; i<2; i++) {   
  					[unroll]
     				for(int j=-1; j<2; j++) {    
        				k++;    
        				
        				float4 col;
        				if (m == 0) col = tex2D(_NormalMap, vi.uv + float2((i)*dxtex, (j)*dytex));
        				if (m == 1) col = tex2D(_NormalMap, vi.uv + float2((i)*dxtex, (j)*dytex)).r;
        				if (m == 2) col = tex2D(_NormalMap, vi.uv + float2((i)*dxtex, (j)*dytex)).g;
        
       					if (channel == 0) array.mat[k] = col.r;
       					if (channel == 1) array.mat[k] = col.g;
       					if (channel == 2) array.mat[k] = col.b;
      				}
  				}
   				return array;
			}
			
			
			float9 GetMean(in float matr[9], in float matg[9], in float matb[9]) {
				struct float9 array;
				[unroll]
   				for (int i=0; i<9; i++) {
      				array.mat[i] = (matr[i]+matg[i]+matb[i])/3.0;
   				}
   				return array;
			}
    		
    		float4 _NormalMap_ST;
 
  			fixed4 frag (v2f i) : COLOR{

     			float kerEmboss_g[9] = { 2.0,  0,     0,
     								   0,    -1.0,  0,
     								   0,    0,    -1.0 };
     								   
     			float kerEmboss_r[9] = { 0,  0,     2.0,
     								   0,    -1.0,  0,
     								   -1.0,    0,    0 };
     								   
     								   
     			float kerEdgeDetect[9] = {-1.0 / 8.0, -1.0/8.0, -1./8.0,
                                     -1.0/8.0, 1.0, -1.0/8.0,
                                     -1.0/8.0, -1.0/8.0, -1.0/8.0};
                                     
                float kerGausBlur[9] = {1.0,2.0,1.0,
                                    2.0, 4.0, 2.0,
                                    1.0, 2.0, 1.0};
                                     
     			
     			struct float9 matr_r = GetData(0, i, 1);	
     			struct float9 matg_r = GetData(1, i, 1);	
     			struct float9 matb_r = GetData(2, i, 1);	
     			struct float9 mata_r = GetMean(matr_r.mat, matg_r.mat, matb_r.mat);   
				
				// Gaussian blur kernel
   				//fixed4 resultColor = fixed4(Convolve(kerGausBlur,matr.mat,16.0, 0),
   				//                    		Convolve(kerGausBlur,matg.mat,16.0, 0),
   				//                    		Convolve(kerGausBlur,matb.mat,16.0, 0),1.0);
							
				// Emboss kernel
                fixed4 resultColor_r = fixed4(Convolve(kerEmboss_r,mata_r.mat, 1.0, 1.0/2.0),
                      						 Convolve(kerEmboss_r,mata_r.mat, 1.0, 1.0/2.0),
                    						 Convolve(kerEmboss_r,mata_r.mat, 1.0, 1.0/2.0),1.0);                           
                
                // Edge Detection kernel 	
				//fixed4 resultColor = fixed4(Convolve(kerEdgeDetect, mata.mat, 0.1, 0),
                //      					    Convolve(kerEdgeDetect, mata.mat, 0.1, 0),
                //     		                Convolve(kerEdgeDetect, mata.mat, 0.1, 0), 1.0);
     			
     			struct float9 matr_g = GetData(0, i, 2);	
     			struct float9 matg_g = GetData(1, i, 2);	
     			struct float9 matb_g = GetData(2, i, 2);	
     			struct float9 mata_g = GetMean(matr_g.mat, matg_g.mat, matb_g.mat);
     			
     			fixed4 resultColor_g = fixed4(Convolve(kerEmboss_g, mata_g.mat, 1.0, 1.0/2.0),
                      						  Convolve(kerEmboss_g, mata_g.mat, 1.0, 1.0/2.0),
                    						  Convolve(kerEmboss_g, mata_g.mat, 1.0, 1.0/2.0),1.0); 
     			
     			
     			resultColor_r = (1- resultColor_r);
     			resultColor_g = (1- resultColor_g);
     			
     			resultColor_r = clamp(resultColor_r, 0.5, 1.0);
     			resultColor_g = clamp(resultColor_g, 0.5, 1.0);
     			
     			fixed4 resultColor = 1 - (1 -resultColor_r) * (1 - resultColor_g);
     			
     			resultColor = pow(resultColor, 12);
     			
     			resultColor = 1 - (1- resultColor) * 1.05;
     			
     			resultColor = clamp(resultColor, 0, 1);
     			
   				return resultColor;
  			}
  			ENDCG
 		}
	} 
 	FallBack "Diffuse"
}