// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/ExportAo" {
	Properties {
		_MainTex ("", 2D) = "white" {}
		_NormalMap ("Normal(RGB) Alpha(A)", 2D) = "bump" {}
		_AoEdgesDirtDepth ("Ao(R) Edges(G) Dirt(B) Depth(A)", 2D) = "white" {}
		
		//masks
		_ObjectMasks ("Object Masks", 2D) = "black" {}
		_ObjectMasks2 ("Object Masks2", 2D) = "black" {}
		


		_BumpMap0("0 Normal Map", 2D) = "bump" {}
		_BumpMapStrength0 ("0 Normal Map Strength", Range (-2.0, 2)) = 0.0
		_OcclusionMap0("0 Occlusion Map", 2D) = "white" {}
		_OcclusionMapStrength0 ("0 Occlusion Map Strength", Range (0.0, 2)) = 1.0
		_0MaterialRotation ("0 Material Rotation", Range (-360.0, 360.0)) = 0.0

		_BumpMap1("1 Normal Map", 2D) = "bump" {}
		_BumpMapStrength1 ("1 Normal Map Strength", Range (-2.0, 2)) = 0.0
		_OcclusionMap1("1 Occlusion Map", 2D) = "white" {}
		_OcclusionMapStrength1 ("1 Occlusion Map Strength", Range (0.0, 2)) = 1.0
		_1MaterialRotation ("1 Material Rotation", Range (-360.0, 360.0)) = 0.0

		_BumpMap2("2 Normal Map", 2D) = "bump" {}
		_BumpMapStrength2 ("2 Normal Map Strength", Range (-2.0, 2)) = 0.0
		_OcclusionMap2("2 Occlusion Map", 2D) = "white" {}
		_OcclusionMapStrength2 ("2 Occlusion Map Strength", Range (0.0, 2)) = 1.0
		_2MaterialRotation ("2 Material Rotation", Range (-360.0, 360.0)) = 0.0
		
		_BumpMap3("3 Normal Map", 2D) = "bump" {}
		_BumpMapStrength3 ("3 Normal Map Strength", Range (-2.0, 2)) = 0.0	
		_OcclusionMap3("3 Occlusion Map", 2D) = "white" {}
		_OcclusionMapStrength3 ("3 Occlusion Map Strength", Range (0.0, 2)) = 1.0
		_3MaterialRotation ("3 Material Rotation", Range (-360.0, 360.0)) = 0.0

		_BumpMap4("4 Normal Map", 2D) = "bump" {}
		_BumpMapStrength4 ("4 Normal Map Strength", Range (-2.0, 2)) = 0.0
		_OcclusionMap4("4 Occlusion Map", 2D) = "white" {}
		_OcclusionMapStrength4 ("4 Occlusion Map Strength", Range (0.0, 2)) = 1.0
		_4MaterialRotation ("4 Material Rotation", Range (-360.0, 360.0)) = 0.0

		_BumpMap5("5 Normal Map", 2D) = "bump" {}
		_BumpMapStrength5 ("5 Normal Map Strength", Range (-2.0, 2)) = 0.0
		_OcclusionMap5("5 Occlusion Map", 2D) = "white" {}
		_OcclusionMapStrength5 ("5 Occlusion Map Strength", Range (0.0, 2)) = 1.0
		_5MaterialRotation ("5 Material Rotation", Range (-360.0, 360.0)) = 0.0

		_BumpMap6("6 Normal Map", 2D) = "bump" {}
		_BumpMapStrength6 ("6 Normal Map Strength", Range (-2.0, 2)) = 0.0
		_OcclusionMap6("6 Occlusion Map", 2D) = "white" {}
		_OcclusionMapStrength6 ("6 Occlusion Map Strength", Range (0.0, 2)) = 1.0
		_6MaterialRotation ("6 Material Rotation", Range (-360.0, 360.0)) = 0.0

		_BumpMap7("7 Normal Map", 2D) = "bump" {}
		_BumpMapStrength7 ("7 Normal Map Strength", Range (-2.0, 2)) = 0.0
		_OcclusionMap7("7 Occlusion Map", 2D) = "white" {}
		_OcclusionMapStrength7 ("7 Occlusion Map Strength", Range (0.0, 2)) = 1.0
		_7MaterialRotation ("7 Material Rotation", Range (-360.0, 360.0)) = 0.0

		_GlobalScale ("GlobalScale", Range (0.1, 2.0)) = 1.0
		
		_LinearColorSpace ("LinearColorSpace", Int) = 0
	}
 
	SubShader {
 
		ZTest Always Cull Off ZWrite Off Fog { Mode Off } 
 		
 		// Mask0
 		Pass{
  			CGPROGRAM
  			#pragma target 3.0
  			#pragma vertex vert
  			#pragma fragment frag
  			#include "UnityCG.cginc" 
  			#include "Surforge.cginc"

    
 			struct v2f {
   				float4 pos : POSITION;
   				half2 uv : TEXCOORD0;
   				
   				half2 texcoord_BumpMap0 : TEXCOORD1;
  			};
   			
   			float _0MaterialRotation;
   			
   			float4 _NormalMap_ST;
			float4 _BumpMap0_ST;
			

  			v2f vert (appdata_img v){
   				v2f o;
  				o.pos = UnityObjectToClipPos (v.vertex);
   				o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
   	
   				float rad = radians(_0MaterialRotation);
				float s = sin (rad);
				float c = cos (rad);
				float2x2 rotationMatrix = float2x2( c, -s, s, c);
				
				o.texcoord_BumpMap0 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _BumpMap0) + 0.5;
   				
   				return o; 
  			}
    
  			sampler2D _BumpMap0;
  			sampler2D _OcclusionMap0;	

			float _BumpMapStrength0;
			float _OcclusionMapStrength0;	
   
 
  			fixed4 frag (v2f i) : COLOR{

            	fixed4 objectMasks = tex2D(_ObjectMasks, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw); 
  	
				//Material AO
				fixed materialAO = GetMaterialAO(tex2D(_OcclusionMap0, i.texcoord_BumpMap0 / _GlobalScale).r, _OcclusionMapStrength0);
   				
   				fixed ao = GetResultAoMap(tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).r) * materialAO;
   				return fixed4(ao, ao, ao, 1);
  			}
  			ENDCG
 		}
 		
 		
 		// Mask1
 		Pass{
 		Blend SrcAlpha OneMinusSrcAlpha
 		
  			CGPROGRAM
  			#pragma target 3.0
  			#pragma vertex vert
  			#pragma fragment frag
  			#include "UnityCG.cginc" 
  			#include "Surforge.cginc"

    
 			struct v2f {
   				float4 pos : POSITION;
   				half2 uv : TEXCOORD0;
   				
   				half2 texcoord_BumpMap1 : TEXCOORD1;
  			};
   			
   			float _1MaterialRotation;
   			
   			float4 _NormalMap_ST;
			float4 _BumpMap1_ST;
			

  			v2f vert (appdata_img v){
   				v2f o;
  				o.pos = UnityObjectToClipPos (v.vertex);
   				o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
   	
   				float rad = radians(_1MaterialRotation);
				float s = sin (rad);
				float c = cos (rad);
				float2x2 rotationMatrix = float2x2( c, -s, s, c);
				
				o.texcoord_BumpMap1 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _BumpMap1) + 0.5;
   				
   				return o; 
  			}
    
  			sampler2D _BumpMap1;
  			sampler2D _OcclusionMap1;	

			float _BumpMapStrength1;
			float _OcclusionMapStrength1;		

   
 
  			fixed4 frag (v2f i) : COLOR{

            	fixed4 objectMasks = tex2D(_ObjectMasks, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw);

				//Material AO
				fixed materialAO = GetMaterialAO(tex2D(_OcclusionMap1, i.texcoord_BumpMap1 / _GlobalScale).r, _OcclusionMapStrength1);

   				fixed ao = GetResultAoMap(tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).r) * materialAO;
   				return fixed4(ao, ao, ao, 1) * objectMasks.g;
  			}
  			ENDCG
 		}
 		
 		// Mask2
 		Pass{
 		Blend SrcAlpha OneMinusSrcAlpha
 		
  			CGPROGRAM
  			#pragma target 3.0
  			#pragma vertex vert
  			#pragma fragment frag
  			#include "UnityCG.cginc" 
  			#include "Surforge.cginc"

    
 			struct v2f {
   				float4 pos : POSITION;
   				half2 uv : TEXCOORD0;
   				
   				half2 texcoord_BumpMap2 : TEXCOORD1;
  			};
   			
   			float _2MaterialRotation;
   			
   			float4 _NormalMap_ST;
			float4 _BumpMap2_ST;
			

  			v2f vert (appdata_img v){
   				v2f o;
  				o.pos = UnityObjectToClipPos (v.vertex);
   				o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
   	
   				float rad = radians(_2MaterialRotation);
				float s = sin (rad);
				float c = cos (rad);
				float2x2 rotationMatrix = float2x2( c, -s, s, c);
				
				o.texcoord_BumpMap2 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _BumpMap2) + 0.5;
   				
   				return o; 
  			}
    
  			sampler2D _BumpMap2;
  			sampler2D _OcclusionMap2;	

			float _BumpMapStrength2;
			float _OcclusionMapStrength2;		

   
 
  			fixed4 frag (v2f i) : COLOR{

            	fixed4 objectMasks = tex2D(_ObjectMasks, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw); 
				
				//Material AO
				fixed materialAO = GetMaterialAO(tex2D(_OcclusionMap2, i.texcoord_BumpMap2 / _GlobalScale).r, _OcclusionMapStrength2);

   				fixed ao = GetResultAoMap(tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).r) * materialAO;
   				return fixed4(ao, ao, ao, 1) * objectMasks.b;
  			}
  			ENDCG
 		}
 		
 		// Mask3
 		Pass{
 		Blend SrcAlpha OneMinusSrcAlpha
  			CGPROGRAM
  			#pragma target 3.0
  			#pragma vertex vert
  			#pragma fragment frag
  			#include "UnityCG.cginc" 
  			#include "Surforge.cginc"

    
 			struct v2f {
   				float4 pos : POSITION;
   				half2 uv : TEXCOORD0;
   				
   				half2 texcoord_BumpMap3 : TEXCOORD1;
  			};
   			
   			float _3MaterialRotation;
   			
   			float4 _NormalMap_ST;
			float4 _BumpMap3_ST;
			

  			v2f vert (appdata_img v){
   				v2f o;
  				o.pos = UnityObjectToClipPos (v.vertex);
   				o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
   	
   				float rad = radians(_3MaterialRotation);
				float s = sin (rad);
				float c = cos (rad);
				float2x2 rotationMatrix = float2x2( c, -s, s, c);
				
				o.texcoord_BumpMap3 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _BumpMap3) + 0.5;
   				
   				return o; 
  			}
    
  			sampler2D _BumpMap3;
  			sampler2D _OcclusionMap3;	

			float _BumpMapStrength3;
			float _OcclusionMapStrength3;		

 
  			fixed4 frag (v2f i) : COLOR{

            	fixed4 objectMasks = tex2D(_ObjectMasks, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw); 

				//Material AO
				fixed materialAO = GetMaterialAO(tex2D(_OcclusionMap3, i.texcoord_BumpMap3 / _GlobalScale).r, _OcclusionMapStrength3);

   				fixed ao = GetResultAoMap(tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).r) * materialAO;
   				return fixed4(ao, ao, ao, 1) * objectMasks.a;
  			}
  			ENDCG
 		}
 		
 		
 		// Mask4
 		Pass{
 		Blend SrcAlpha OneMinusSrcAlpha
  			CGPROGRAM
  			#pragma target 3.0
  			#pragma vertex vert
  			#pragma fragment frag
  			#include "UnityCG.cginc" 
  			#include "Surforge.cginc"

    
 			struct v2f {
   				float4 pos : POSITION;
   				half2 uv : TEXCOORD0;
   				
   				half2 texcoord_BumpMap4 : TEXCOORD1;
  			};
   			
   			float _4MaterialRotation;
   			
   			float4 _NormalMap_ST;
			float4 _BumpMap4_ST;
			

  			v2f vert (appdata_img v){
   				v2f o;
  				o.pos = UnityObjectToClipPos (v.vertex);
   				o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
   	
   				float rad = radians(_4MaterialRotation);
				float s = sin (rad);
				float c = cos (rad);
				float2x2 rotationMatrix = float2x2( c, -s, s, c);
				
				o.texcoord_BumpMap4 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _BumpMap4) + 0.5;
   				
   				return o; 
  			}
    
  			sampler2D _BumpMap4;
  			sampler2D _OcclusionMap4;	

			float _BumpMapStrength4;
			float _OcclusionMapStrength4;		
   			
 
  			fixed4 frag (v2f i) : COLOR{

            	fixed4 objectMasks = tex2D(_ObjectMasks2, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw); 

				//Material AO
				fixed materialAO = GetMaterialAO(tex2D(_OcclusionMap4, i.texcoord_BumpMap4 / _GlobalScale).r, _OcclusionMapStrength4);
   				
   				fixed ao = GetResultAoMap(tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).r) * materialAO;
   				return fixed4(ao, ao, ao, 1) * objectMasks.r;
  			}
  			ENDCG
 		}
 		
 		
 		// Mask5
 		Pass{
 		Blend SrcAlpha OneMinusSrcAlpha
  			CGPROGRAM
  			#pragma target 3.0
  			#pragma vertex vert
  			#pragma fragment frag
  			#include "UnityCG.cginc" 
  			#include "Surforge.cginc"

    
 			struct v2f {
   				float4 pos : POSITION;
   				half2 uv : TEXCOORD0;
   				
   				half2 texcoord_BumpMap5 : TEXCOORD1;
  			};
   			
   			float _5MaterialRotation;
   			
   			float4 _NormalMap_ST;
			float4 _BumpMap5_ST;
			

  			v2f vert (appdata_img v){
   				v2f o;
  				o.pos = UnityObjectToClipPos (v.vertex);
   				o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
   	
   				float rad = radians(_5MaterialRotation);
				float s = sin (rad);
				float c = cos (rad);
				float2x2 rotationMatrix = float2x2( c, -s, s, c);
				
				o.texcoord_BumpMap5 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _BumpMap5) + 0.5;
   				
   				return o; 
  			}
    
  			sampler2D _BumpMap5;
  			sampler2D _OcclusionMap5;	

			float _BumpMapStrength5;
			float _OcclusionMapStrength5;		

   
 
  			fixed4 frag (v2f i) : COLOR{

            	fixed4 objectMasks = tex2D(_ObjectMasks2, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw); 

				//Material AO
				fixed materialAO = GetMaterialAO(tex2D(_OcclusionMap5, i.texcoord_BumpMap5 / _GlobalScale).r, _OcclusionMapStrength5);

   				fixed ao = GetResultAoMap(tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).r) * materialAO;
   				return fixed4(ao, ao, ao, 1) * objectMasks.g;
  			}
  			ENDCG
 		}
 		
 		
 		// Mask6
 		Pass{
 		Blend SrcAlpha OneMinusSrcAlpha
  			CGPROGRAM
  			#pragma target 3.0
  			#pragma vertex vert
  			#pragma fragment frag
  			#include "UnityCG.cginc" 
  			#include "Surforge.cginc"

    
 			struct v2f {
   				float4 pos : POSITION;
   				half2 uv : TEXCOORD0;
   				
   				half2 texcoord_BumpMap6 : TEXCOORD1;
  			};
   			
   			float _6MaterialRotation;
   			
   			float4 _NormalMap_ST;
			float4 _BumpMap6_ST;
			

  			v2f vert (appdata_img v){
   				v2f o;
  				o.pos = UnityObjectToClipPos (v.vertex);
   				o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
   	
   				float rad = radians(_6MaterialRotation);
				float s = sin (rad);
				float c = cos (rad);
				float2x2 rotationMatrix = float2x2( c, -s, s, c);
				
				o.texcoord_BumpMap6 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _BumpMap6) + 0.5;
   				
   				return o; 
  			}
    
  			sampler2D _BumpMap6;
  			sampler2D _OcclusionMap6;	

			float _BumpMapStrength6;
			float _OcclusionMapStrength6;		
   
 
  			fixed4 frag (v2f i) : COLOR{

            	fixed4 objectMasks = tex2D(_ObjectMasks2, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw); 
				
				//Material AO
				fixed materialAO = GetMaterialAO(tex2D(_OcclusionMap6, i.texcoord_BumpMap6 / _GlobalScale).r, _OcclusionMapStrength6);

   				fixed ao = GetResultAoMap(tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).r) * materialAO;
   				return fixed4(ao, ao, ao, 1) * objectMasks.b;
  			}
  			ENDCG
 		}
 		
 		
 		// Mask7
 		Pass{
 		Blend SrcAlpha OneMinusSrcAlpha
  			CGPROGRAM
  			#pragma target 3.0
  			#pragma vertex vert
  			#pragma fragment frag
  			#include "UnityCG.cginc" 
  			#include "Surforge.cginc"

    
 			struct v2f {
   				float4 pos : POSITION;
   				half2 uv : TEXCOORD0;
   				
   				half2 texcoord_BumpMap7 : TEXCOORD1;
  			};
   			
   			float _7MaterialRotation;
   			
   			float4 _NormalMap_ST;
			float4 _BumpMap7_ST;
			

  			v2f vert (appdata_img v){
   				v2f o;
  				o.pos = UnityObjectToClipPos (v.vertex);
   				o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
   	
   				float rad = radians(_7MaterialRotation);
				float s = sin (rad);
				float c = cos (rad);
				float2x2 rotationMatrix = float2x2( c, -s, s, c);
				
				o.texcoord_BumpMap7 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _BumpMap7) + 0.5;
   				
   				return o; 
  			}
    
  			sampler2D _BumpMap7;
  			sampler2D _OcclusionMap7;	

			float _BumpMapStrength7;
			float _OcclusionMapStrength7;		

   
 
  			fixed4 frag (v2f i) : COLOR{

            	fixed4 objectMasks = tex2D(_ObjectMasks2, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw); 

				//Material AO
				fixed materialAO = GetMaterialAO(tex2D(_OcclusionMap7, i.texcoord_BumpMap7 / _GlobalScale).r, _OcclusionMapStrength7);

   				fixed ao = GetResultAoMap(tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).r) * materialAO;
   				return fixed4(ao, ao, ao, 1) * objectMasks.a;
  			}
  			ENDCG
 		}
	} 
 	FallBack "Diffuse"
}