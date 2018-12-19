Shader "Hidden/FloatersMaskCapture" {
 Properties {
    }
   
    CGINCLUDE
        #define _GLOSSYENV 1
        #define UNITY_SETUP_BRDF_INPUT MetallicSetup
    ENDCG
   
    SubShader {
        Tags { "RenderType"="Opaque" "Floater" = "1" }
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
    SubShader {
        Tags { "RenderType"="Opaque" "Floater" = "0"}
    	  CGPROGRAM
    	  #pragma surface surf Lambert  noshadow
     	 struct Input {
     	     float4 color : COLOR;
     	 };
     	 void surf (Input IN, inout SurfaceOutput o) {
     	     o.Albedo = 0;
     	     o.Emission = 0;
    	  }
        ENDCG
    }
    
    FallBack "Diffuse"
}