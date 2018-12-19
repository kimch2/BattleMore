Shader "Hidden/AoCaptureFloater" {
 Properties {
    }
   
    CGINCLUDE
        #define _GLOSSYENV 1
        #define UNITY_SETUP_BRDF_INPUT MetallicSetup
    ENDCG
   
    SubShader {
        Tags { "RenderType"="Opaque" "Floater" = "1" }
        LOD 200
       
        CGPROGRAM
        #include "UnityPBSLighting.cginc"
        #pragma surface surf Lambert addshadow
        #pragma target 3.0
 
        fixed4 _Meta;
 
        struct Input {
            fixed4 color : COLOR;
        };
 
        void surf (Input IN, inout SurfaceOutput o) {
            o.Albedo = 1.0;
            
        }
        ENDCG
    }
    SubShader {
        Tags { "RenderType"="Opaque" "Floater" = "0"}
    	  CGPROGRAM
    	  #pragma surface surf Lambert  noshadow
     	 struct Input {
     	     float4 color : COLOR;
     	 };
     	 void surf (Input IN, inout SurfaceOutput o) {
     	     o.Albedo = 1;
     	     o.Emission = 1;
    	  }
        ENDCG
    }
    
    FallBack "Diffuse"
}