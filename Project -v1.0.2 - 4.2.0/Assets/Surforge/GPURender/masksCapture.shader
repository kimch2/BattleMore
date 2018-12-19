Shader "Hidden/MasksCapture" {
 Properties {
 		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
   
    CGINCLUDE
        #define _GLOSSYENV 1
        #define UNITY_SETUP_BRDF_INPUT MetallicSetup
    ENDCG
    
     SubShader {
        Tags { "RenderType"="Opaque" "SurforgeMaterialId" = "0" "SurforgeId0" = "0"}
        LOD 200
       
        CGPROGRAM
       	#pragma surface surf Lambert alphatest:_Cutoff
 
        fixed4 _Meta;
        sampler2D _MainTex;
 
        struct Input {
            fixed4 color : COLOR;
            float2 uv_MainTex;
        };
 
        void surf (Input IN, inout SurfaceOutput  o) {

            o.Emission = fixed4(1, 0, 0, 1);
            
            half4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Alpha = c.r;
        }
        ENDCG
    }
    
     SubShader {
        Tags { "RenderType"="TransparentCutout" "SurforgeMaterialId" = "1" "SurforgeId1" = "1"}
        LOD 200
       
        CGPROGRAM
        #pragma surface surf Lambert alphatest:_Cutoff
 
        fixed4 _Meta;
        sampler2D _MainTex;
 
        struct Input {
            fixed4 color : COLOR;
            float2 uv_MainTex;
        };
 
        void surf (Input IN, inout SurfaceOutput  o) {
  
            o.Emission = fixed4(0, 1, 0, 1);
            
            half4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Alpha = c.r;
        }
        ENDCG
    }
    
     SubShader {
        Tags { "RenderType"="TransparentCutout" "SurforgeMaterialId" = "2" "SurforgeId2" = "2"}
        LOD 200
       
        CGPROGRAM
      	 #pragma surface surf Lambert alphatest:_Cutoff
 
        fixed4 _Meta;
        sampler2D _MainTex;
 
        struct Input {
            fixed4 color : COLOR;
            float2 uv_MainTex;
        };
 
        void surf (Input IN, inout SurfaceOutput  o) {
   
            o.Emission = fixed4(0, 0, 1, 1);
            
            half4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Alpha = c.r;
        }
        ENDCG
    }
    
     SubShader {
        Tags { "RenderType"="TransparentCutout" "SurforgeMaterialId" = "3" "SurforgeId3" = "3"}
        LOD 200
       
        CGPROGRAM
       	#pragma surface surf Lambert alphatest:_Cutoff
 
        fixed4 _Meta;
        sampler2D _MainTex;
 
        struct Input {
            fixed4 color : COLOR;
            float2 uv_MainTex;
        };
 
        void surf (Input IN, inout SurfaceOutput  o) {
       
            o.Emission = fixed4(1, 1, 0, 1);
            
            half4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Alpha = c.r;
        }
        ENDCG
    }
   
    SubShader {
        Tags { "RenderType"="Opaque" "SurforgeMaterialId" = "4" "SurforgeId4" = "4"}
        LOD 200
       
        CGPROGRAM
       	#pragma surface surf Lambert alphatest:_Cutoff
 
        fixed4 _Meta;
        sampler2D _MainTex;
 
        struct Input {
            fixed4 color : COLOR;
            float2 uv_MainTex;
        };
 
        void surf (Input IN, inout SurfaceOutput  o) {

            o.Emission = fixed4(1, 0, 1, 1);
            
            half4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Alpha = c.r;
        }
        ENDCG
    }
    
     SubShader {
        Tags { "RenderType"="TransparentCutout" "SurforgeMaterialId" = "5" "SurforgeId5" = "5"}
        LOD 200
       
        CGPROGRAM
        #pragma surface surf Lambert alphatest:_Cutoff
 
        fixed4 _Meta;
        sampler2D _MainTex;
 
        struct Input {
            fixed4 color : COLOR;
            float2 uv_MainTex;
        };
 
        void surf (Input IN, inout SurfaceOutput  o) {
  
            o.Emission = fixed4(0, 1, 1, 1);
            
            half4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Alpha = c.r;
        }
        ENDCG
    }
    
     SubShader {
        Tags { "RenderType"="TransparentCutout" "SurforgeMaterialId" = "6" "SurforgeId6" = "6"}
        LOD 200
       
        CGPROGRAM
      	 #pragma surface surf Lambert alphatest:_Cutoff
 
        fixed4 _Meta;
        sampler2D _MainTex;
 
        struct Input {
            fixed4 color : COLOR;
            float2 uv_MainTex;
        };
 
        void surf (Input IN, inout SurfaceOutput  o) {
   
            o.Emission = fixed4(1, 1, 1, 1);
            
            half4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Alpha = c.r;
        }
        ENDCG
    }
    
     SubShader {
        Tags { "RenderType"="TransparentCutout" "SurforgeMaterialId" = "7" "SurforgeId7" = "7"}
        LOD 200
       
        CGPROGRAM
       	#pragma surface surf Lambert alphatest:_Cutoff
 
        fixed4 _Meta;
        sampler2D _MainTex;
 
        struct Input {
            fixed4 color : COLOR;
            float2 uv_MainTex;
        };
 
        void surf (Input IN, inout SurfaceOutput  o) {
       
            o.Emission = fixed4(0, 0, 0, 1);
            
            half4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Alpha = c.r;
        }
        ENDCG
    }
    FallBack "Diffuse"
}