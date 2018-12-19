// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Hidden/HeightmapCapture" {
 Properties {
 		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		
		_MaxGeometryHeight ("Max geometry height", Range (0.0, 100.0)) = 1.0
    }
   
    CGINCLUDE
        #define _GLOSSYENV 1
        #define UNITY_SETUP_BRDF_INPUT MetallicSetup
    ENDCG
   
    SubShader {
    	Lighting Off
        Tags { "RenderType"="Opaque" "Floater" = "0" "Base" = "0" "Surforge"="Opaque"}
        LOD 200
       
        CGPROGRAM
        #include "UnityPBSLighting.cginc"
        #pragma vertex vert 
        #pragma surface surf Lambert alphatest:_Cutoff noshadow noambient 
        #pragma target 3.0
 
        fixed4 _Meta;
        
        sampler2D _MainTex;
		
		float _MaxGeometryHeight;
 
        struct Input {
            fixed4 color : COLOR;
            float2 uv_MainTex;
            float3 worldPos   : TEXCOORD1;
        };
        
         void vert (inout appdata_full v, out Input o) 
         {
             UNITY_INITIALIZE_OUTPUT(Input,o);
             o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
         }
 
        void surf (Input IN, inout SurfaceOutput o) {
			float d = 1 * (IN.worldPos.y + 0.26);
			float heihtValue = 0.5;
			if (d != 0) heihtValue = (_MaxGeometryHeight + 0.26) / d;

			float4 gradient = 1.0 / heihtValue;
        		
        	o.Albedo = 0;	
        	o.Emission = gradient;
            
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}