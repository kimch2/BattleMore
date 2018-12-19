Shader "Hidden/Id1" {
    Properties
    {
    	_Color ("Main Color", Color) = (0.35,0.35,0.35,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
        _BumpMap ("Normal(RGB) Alpha(A)", 2D) = "bump" {}
        _BumpIntensity ("Bump Intensity", Range (0.0, 2.0)) = 1.0
        _AO ("AO", 2D) = "white" {}
        _AOIntensity ("AO Intensity", Range (0.0, 2.0)) = 1.0
    }
 
    SubShader
    {
        Tags {"Surforge"="Opaque" "SurforgeMaterialId" = "1" "SurforgeId1" = "1" "Base" = "0" "Floater" = "0" "SurforgeEmissionId" = "3" "DisableBatching" = "True"}
        LOD 200
       
        CGPROGRAM
        #pragma surface surf Lambert alphatest:_Cutoff 
 
        sampler2D _MainTex;
        sampler2D _BumpMap;
        sampler2D _AO;
        fixed4 _Color;
        
        half _BumpIntensity;
        half _AOIntensity;
 
        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float2 uv_AO;
        };
 
        void surf (Input IN, inout SurfaceOutput o)
        {
            half4 c = tex2D (_MainTex, IN.uv_MainTex);
            half4 ao = tex2D (_AO, IN.uv_AO);
            
            o.Albedo = c.rgb * _Color * ao;
            o.Alpha = c.r;
            
            o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
        }
 
        ENDCG
    }
 
    Fallback "Transparent/Cutout/VertexLit"
}
