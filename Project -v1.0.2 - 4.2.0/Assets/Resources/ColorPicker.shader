Shader "Custom/ColorPicker"
{
	
		Properties
		{
			_Color("Color", Color) = (1,1,1,1)
			_MainTex("Albedo (RGB)", 2D) = "white" {}
			_Glossiness("Smoothness", Range(0,1)) = 0.5
			_Metallic("Metallic", Range(0,1)) = 0.0
			_HueShift("HueShift", Float) = 0
			_Sat("Saturation", Float) = 1
			_Val("Value", Float) = 1
		}
			SubShader
			{
				Tags { "RenderType" = "Opaque" }
				LOD 200

				CGPROGRAM
				// Physically based Standard lighting model, and enable shadows on all light types
				#pragma surface surf Standard fullforwardshadows

				// Use shader model 3.0 target, to get nicer looking lighting
				#pragma target 3.0

				sampler2D _MainTex;

				struct Input
				{
					float2 uv_MainTex;
				};

				float _HueShift;
				float _Sat;
				float _Val;
				half _Glossiness;
				half _Metallic;
				fixed4 _Color;

				// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
				// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
				// #pragma instancing_options assumeuniformscaling
				UNITY_INSTANCING_BUFFER_START(Props)
					// put more per-instance properties here
				UNITY_INSTANCING_BUFFER_END(Props)


				float3 shift_col(float3 RGB, float3 shift)
				{
					float3 RESULT = float3(RGB);
					float VSU = shift.z*shift.y*cos(shift.x*3.14159265 / 180);
					float VSW = shift.z*shift.y*sin(shift.x*3.14159265 / 180);

					RESULT.x = (.299*shift.z + .701*VSU + .168*VSW)*RGB.x
						+ (.587*shift.z - .587*VSU + .330*VSW)*RGB.y
						+ (.114*shift.z - .114*VSU - .497*VSW)*RGB.z;

					RESULT.y = (.299*shift.z - .299*VSU - .328*VSW)*RGB.x
						+ (.587*shift.z + .413*VSU + .035*VSW)*RGB.y
						+ (.114*shift.z - .114*VSU + .292*VSW)*RGB.z;

					RESULT.z = (.299*shift.z - .3*VSU + 1.25*VSW)*RGB.x
						+ (.587*shift.z - .588*VSU - 1.05*VSW)*RGB.y
						+ (.114*shift.z + .886*VSU - .203*VSW)*RGB.z;

					return (RESULT);
				}


				void surf(Input IN, inout SurfaceOutputStandard o)
				{
					// Albedo comes from a texture tinted by color
					fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

					float3 shift = float3(_HueShift, _Sat, _Val);

					c = half4(half3(shift_col(c, shift)), c.a);

					o.Albedo = c.rgb;
					// Metallic and smoothness come from slider variables
					o.Metallic = _Metallic;
					o.Smoothness = _Glossiness;
					o.Alpha = c.a;
				}
				ENDCG
			}
				FallBack "Diffuse"
	}
