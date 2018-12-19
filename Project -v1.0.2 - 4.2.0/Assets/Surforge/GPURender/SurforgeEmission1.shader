Shader "Hidden/EmissionId1" {
	Properties {
		_Color ("Main Color", Color) = (0.35,0.35,0.35,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}

	}
	SubShader {
		Tags { "SurforgeMaterialId" = "1" "SurforgeId1" = "1" "SurforgeEmissionId" = "1" "DisableBatching" = "True"}
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		fixed4 _Color;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb * _Color;
			o.Emission = float4 (0.4,1,1,1);
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
