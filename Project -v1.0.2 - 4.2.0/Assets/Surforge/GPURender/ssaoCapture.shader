Shader "Hidden/SsaoCapture" {
 Properties {
 		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
		_AO ("AO", 2D) = "white" {}
		_AOIntensity ("AO Intensity", Range (0.0, 2.0)) = 1.0

    }
   
    CGINCLUDE
        #define _GLOSSYENV 1
        #define UNITY_SETUP_BRDF_INPUT MetallicSetup
    ENDCG
   
    SubShader {
        Tags { "RenderType"="Opaque" "Floater" = "0" "Base" = "0"}
        LOD 200
       
        CGPROGRAM
        #include "UnityPBSLighting.cginc"
        #pragma surface surf Lambert addshadow alphatest:_Cutoff
        #pragma target 3.0
 
        fixed4 _Meta;
        
        sampler2D _MainTex;
        sampler2D _AO;
        
        half _AOIntensity;
 
        struct Input {
            fixed4 color : COLOR;
            float2 uv_MainTex;
            float2 uv_AO;
        };
 
        void surf (Input IN, inout SurfaceOutput o) {
        	half4 c = tex2D (_MainTex, IN.uv_MainTex);
        	
        	half4 ao = tex2D (_AO, IN.uv_AO);
        	
        	o.Emission = 1.0 * ao * _AOIntensity;
        	
        	o.Albedo = 0;
 
            o.Alpha = c.r;

        }
        ENDCG
    }
    
    
    FallBack "Diffuse"
}