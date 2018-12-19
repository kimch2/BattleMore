Shader "Hidden/LabelsAlphaCapture" {
    Properties {
 		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
		_RGBAnoise ("RGBA Noise", 2D) = "black" {}
		_NoiseMix ("Noise Mix", Vector) = (0,0,0,1)
    }
   
    CGINCLUDE
        #define _GLOSSYENV 1
        #define UNITY_SETUP_BRDF_INPUT MetallicSetup
    ENDCG
   
     SubShader {
        Tags { "RenderType"="Transparent" "SurforgeLabel"="Transparent" }
        LOD 200
       
        CGPROGRAM
        #include "UnityPBSLighting.cginc"
        #pragma surface surf Standard addshadow alpha:fade
        #pragma target 3.0
 
        fixed4 _Meta;
        sampler2D _MainTex;
        fixed4 _Color;
        sampler2D _RGBAnoise;
        fixed4 _NoiseMix;
 
        struct Input {
            fixed4 color : COLOR;
            float2 uv_MainTex;
        };
 
        void surf (Input IN, inout SurfaceOutputStandard  o) {
  
            half4 c = tex2D (_MainTex, IN.uv_MainTex);
            
            fixed4 rgbaNoise = tex2D(_RGBAnoise, IN.uv_MainTex);
            
            fixed noise = (rgbaNoise.r * _NoiseMix.r + 
            				  rgbaNoise.g * _NoiseMix.g + 
            				  rgbaNoise.b * _NoiseMix.b +
            				  rgbaNoise.a * _NoiseMix.a);
            
            o.Emission = 1 ;
            
            o.Alpha = c.a * _Color.a * noise;
        }
        ENDCG
    }
    
    SubShader {
        Tags { "RenderType"="Transparent" "SurforgeLabel"="Text" }
        LOD 200
       
        CGPROGRAM
        #include "UnityPBSLighting.cginc"
        #pragma surface surf Standard addshadow alpha:fade
        #pragma target 3.0
 
        fixed4 _Meta;
        sampler2D _MainTex;
        fixed4 _Color;
        sampler2D _RGBAnoise;
        fixed4 _NoiseMix;
 
        struct Input {
            fixed4 color : COLOR;
            float2 uv_MainTex;
        };
 
        void surf (Input IN, inout SurfaceOutputStandard  o) {
  
            half4 c = tex2D (_MainTex, IN.uv_MainTex);
            
            fixed4 rgbaNoise = tex2D(_RGBAnoise, IN.uv_MainTex);
            
            fixed noise = (rgbaNoise.r * _NoiseMix.r + 
            				  rgbaNoise.g * _NoiseMix.g + 
            				  rgbaNoise.b * _NoiseMix.b +
            				  rgbaNoise.a * _NoiseMix.a);
            
            o.Emission = 1 ;
            
            o.Alpha = c.a * _Color.a * noise;
        }
        ENDCG
    }
    
   
    
  }