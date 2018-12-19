Shader "Hidden/WireframeFromVertexColor" {
Properties 
    {
		_Color("Main Color (RGB)", color) = (0.5, 0.5, 0.5, 1)
		_MainTex("Base (RGB)", 2D) = "white"{}
		
		_WColor("Wire Color", color) = (1, 1, 1, 1)	
		_Thickness("Wire Thickness", Range(0, 2)) = 0.5
		
    }

    CGINCLUDE
        #define _GLOSSYENV 1
        #define UNITY_SETUP_BRDF_INPUT SpecularSetup
    ENDCG
   
    SubShader {
        Tags { "RenderType"="Opaque" "Floater" = "0" "Base" = "0"}
        LOD 200
       
        CGPROGRAM
        #include "UnityPBSLighting.cginc"
        #pragma surface surf Standard addshadow alphatest:_Cutoff
        #pragma target 3.0
		
		fixed4 _Color;
        sampler2D _MainTex;
        fixed4 _WColor;
		half _Thickness;
 
        struct Input {
            fixed4 color : COLOR;
            float2 uv_MainTex;
            float2 uv_AO;
        };
 
        void surf (Input IN, inout SurfaceOutputStandard o) {
        	half4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
        	
        	half3 d = abs(ddx(IN.color)) + abs(ddy(IN.color));
        	
        	half3 fragColor = smoothstep(half3(0, 0, 0), d * _Thickness, IN.color);		
	  		half value = min(min(fragColor.x, fragColor.y), fragColor.z);	
			
			half4 result = lerp(lerp(c, _WColor, _WColor.a), c, value); 
			
            o.Albedo = result; 

        }
        ENDCG
    }
    FallBack "Diffuse"
}