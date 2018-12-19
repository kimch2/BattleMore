Shader "Hidden/EmissionCapture" {
 Properties {
    }
   
    CGINCLUDE
        #define _GLOSSYENV 1
        #define UNITY_SETUP_BRDF_INPUT MetallicSetup
    ENDCG
   
    SubShader {
        Tags { "RenderType"="Opaque" "SurforgeEmissionId" = "0" "SurforgeId0" = "0"}
        LOD 200
       
        CGPROGRAM
       	#pragma surface surf Lambert
 
        fixed4 _Meta;
 
        struct Input {
            fixed4 color : COLOR;
        };
 
        void surf (Input IN, inout SurfaceOutput  o) {

            o.Emission = fixed4(1, 0, 0, 0);
        }
        ENDCG
    }
    
     SubShader {
        Tags { "RenderType"="Opaque" "SurforgeEmissionId" = "1" "SurforgeId1" = "1"}
        LOD 200
       
        CGPROGRAM
        #pragma surface surf Lambert
 
        fixed4 _Meta;
 
        struct Input {
            fixed4 color : COLOR;
        };
 
        void surf (Input IN, inout SurfaceOutput  o) {
  
            o.Emission = fixed4(0, 1, 0, 0);
        }
        ENDCG
    }
    
     SubShader {
        Tags { "RenderType"="Opaque" "SurforgeEmissionId" = "2" "SurforgeId2" = "2"}
        LOD 200
       
        CGPROGRAM
      	 #pragma surface surf Lambert
 
        fixed4 _Meta;
 
        struct Input {
            fixed4 color : COLOR;
        };
 
        void surf (Input IN, inout SurfaceOutput  o) {
   
            o.Emission = fixed4(0, 0, 1, 0);
        }
        ENDCG
    }
    
     SubShader {
        Tags { "RenderType"="Opaque" "SurforgeEmissionId" = "3" "SurforgeId3" = "3"}
        LOD 200
       
        CGPROGRAM
       	#pragma surface surf Lambert
 
        fixed4 _Meta;
 
        struct Input {
            fixed4 color : COLOR;
        };
 
        void surf (Input IN, inout SurfaceOutput  o) {
       
            o.Emission = fixed4(0, 0, 0, 1);
        }
        ENDCG
    }
    FallBack "Diffuse"
}