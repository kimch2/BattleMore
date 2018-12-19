Shader "Hidden/Id3Floater" {
	Properties {
		_Color ("Main Color", Color) = (0.2,0.2,0.2,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}

	}
	SubShader {
		Tags { "Surforge"="Floater" "SurforgeMaterialId" = "3" "SurforgeId3" = "3" "Floater" = "1" "DisableBatching" = "True"}
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
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
