Shader "Hidden/LabelsCapture" {
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
        Tags { "RenderType"="Transparent" "SurforgeLabel"="Transparent"}    
        LOD 200
       
        CGPROGRAM
       	#pragma surface surf Lambert alpha:fade
 
        fixed4 _Meta;
        sampler2D _MainTex;
        fixed4 _Color;
 
        struct Input {
            fixed4 color : COLOR;
            float2 uv_MainTex;
        };
 
        void surf (Input IN, inout SurfaceOutput  o) {
        	half4 tint = half4 (_Color.r, _Color.g, _Color.b, 1);
            half4 c = tex2D (_MainTex, IN.uv_MainTex) * tint;
            o.Emission = c; 
            o.Alpha = c.a;
        }
        ENDCG
    }
    
   SubShader {
		Tags { "RenderType"="Transparent" "SurforgeLabel"="Text"}
		Lighting Off Cull Off ZWrite Off Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha
		Pass {
			Color [_Color]
			SetTexture [_MainTex] {
				combine primary, texture * primary
			}
		}
	}
    
  }