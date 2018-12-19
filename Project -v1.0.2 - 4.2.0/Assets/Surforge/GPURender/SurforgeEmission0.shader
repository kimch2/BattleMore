Shader "Hidden/EmissionId0" {
	Properties {
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}

	}
	SubShader {
		Tags { "SurforgeMaterialId" = "0" "SurforgeId0" = "0" "SurforgeEmissionId" = "0" "DisableBatching" = "True"}
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
			o.Emission = float4 (1,1,1,1);
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
