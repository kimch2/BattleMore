Shader "Hidden/NormalsCapture" {
   Properties {
      _MainTex ("Texture", 2D) = "white" {}
      _BumpMap ("Normal(RGB) Alpha(A)", 2D) = "bump" {}
      _BumpIntensity ("Bump Intensity", Range (0.0, 2.0)) = 1.0
    }
    SubShader {
      Tags { "RenderType" = "Opaque" "Surforge"="Opaque" }
      CGPROGRAM
      #pragma surface surf Lambert vertex:vert
      struct Input {
          float2 uv_MainTex;
          float2 uv_BumpMap;
          float4 customColor;
          float3 worldPos;
          float3 viewDir;
          float3 screenPos;
          float3 worldRefl; 
      };
      
      void vert (inout appdata_full v, out Input o) {
          UNITY_INITIALIZE_OUTPUT(Input,o);

          o.customColor.xyz = v.normal * 0.5 + 0.5;  
  
          o.customColor.w = 1.0;
      }
      
      sampler2D _MainTex;
      sampler2D _BumpMap;
      
      half _BumpIntensity;

      
      void surf (Input IN, inout SurfaceOutput o) {

      		float3 nBase = float3(o.Normal.x, o.Normal.z, o.Normal.y );    
			half3 nDetail = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
			
			_BumpIntensity = _BumpIntensity + 0.00001;
			
			nDetail.z = nDetail.z / _BumpIntensity;
			nDetail = normalize(nDetail);

			o.Normal = nBase;

     		
     		//RNM blending
     		
     		float a = 1/(1 + o.Normal.z);
			float b = -o.Normal.x*o.Normal.y*a;

			float3 b1 = float3(1 - o.Normal.x*o.Normal.x*a, b, -o.Normal.x);
			float3 b2 = float3(b, 1 - o.Normal.y*o.Normal.y*a, -o.Normal.y);
			float3 b3 = o.Normal;

			if (o.Normal.z < -0.9999999) {
    			b1 = float3( 0, -1, 0); 
   				b2 = float3(-1,  0, 0);
			}

			float3 r = nDetail.x*b1 + nDetail.y*b2 + nDetail.z*b3 ;
     		o.Emission = r*0.5 + 0.5;
     		
     		
     		//UDN blending
     		
     		//float3 result = normalize(float3(o.Normal.xy + nDetail.xy, o.Normal.z));
     		//o.Emission = (result + 1) * 0.5;
 
      }
      ENDCG
    } 
    
    SubShader {
      Tags { "RenderType" = "Opaque" "Surforge"="Floater" }
      CGPROGRAM
      #pragma surface surf Lambert vertex:vert
      struct Input {
          float2 uv_MainTex;
          float2 uv_BumpMap;
          float4 customColor;
          float3 worldPos;
          float3 viewDir;
          float3 screenPos;
          float3 worldRefl; 
      };
      
      void vert (inout appdata_full v, out Input o) {
          UNITY_INITIALIZE_OUTPUT(Input,o);

          o.customColor.xyz = v.normal * 0.5 + 0.5;  
  
          o.customColor.w = 1.0;
      }
      
      sampler2D _MainTex;
      sampler2D _BumpMap;
      
      half _BumpIntensity;

      
      void surf (Input IN, inout SurfaceOutput o) {

      		float3 nBase = float3(o.Normal.x, o.Normal.z, o.Normal.y );    
			half3 nDetail = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
			
			_BumpIntensity = _BumpIntensity + 0.00001;
			
			nDetail.z = nDetail.z / _BumpIntensity;
			nDetail = normalize(nDetail);

			o.Normal = nBase;

     		
     		//RNM blending
     		
     		float a = 1/(1 + o.Normal.z);
			float b = -o.Normal.x*o.Normal.y*a;

			float3 b1 = float3(1 - o.Normal.x*o.Normal.x*a, b, -o.Normal.x);
			float3 b2 = float3(b, 1 - o.Normal.y*o.Normal.y*a, -o.Normal.y);
			float3 b3 = o.Normal;

			if (o.Normal.z < -0.9999999) {
    			b1 = float3( 0, -1, 0); 
   				b2 = float3(-1,  0, 0);
			}

			float3 r = nDetail.x*b1 + nDetail.y*b2 + nDetail.z*b3 ;
     		o.Emission = r*0.5 + 0.5;
     		
     		
     		//UDN blending
     		
     		//float3 result = normalize(float3(o.Normal.xy + nDetail.xy, o.Normal.z));
     		//o.Emission = (result + 1) * 0.5;
 
      }
      ENDCG
    } 
    Fallback "Diffuse"
  }