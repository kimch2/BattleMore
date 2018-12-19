Shader "Hidden/SurforgeSingle" {
	Properties {
		_LabelTint ("Label Tint", Color) = (1,1,1,1)
		_LabelTexture ("Label Texture", 2D) = "black" {}
		_LabelSpecularIntensity ("Label Specular Intensity", Range (0.0, 1.0)) = 0.136
		_LabelGlossiness ("Label Glossiness", Range (0.0, 1.0)) = 0.843
		
		
		_NormalMap ("Normal(RGB) Alpha(A)", 2D) = "bump" {}
		
		_AoEdgesDirtDepth ("Ao(R) Edges(G) Dirt(B) Depth(A)", 2D) = "white" {}
			
		//noise
		_RGBAnoise ("RGBA Noise", 2D) = "black" {}
		
		//masks
		_ObjectMasks ("Object Masks", 2D) = "white" {}
		_ObjectMasks2 ("Object Masks2", 2D) = "black" {}
		
		_Tint0 ("0 Tint", Color) = (1,1,1,1)
		_SpecularTint0 ("0 Specular Tint", Color) = (1,1,1,1)
		
		_SpecularIntensity0 ("0 Specular Intensity", Range (0.0, 1.0)) = 0.2
		_0SpecularContrast ("0 Specular Contrast", Range (0, 1.0)) = 0.1
		_0SpecularBrightness ("0 Specular Brightness", Range (0, 1.0)) = 0.5
		
		_Glossiness0 ("0 Glossiness", Range (0.0, 1.0)) = 0.4
		_GlossinessIntensity0 ("0 Glossiness Intensity", Range (0.0, 1.0)) = 0.2
		_0GlossinessContrast ("0 Glossiness Contrast", Range (0.0, 1.0)) = 0.2
		_0GlossinessBrightness ("0 Glossiness Brightness", Range (0.0, 1.0)) = 0.5
		
		_0Paint1Intensity ("0 Paint 1 Intensity", Range (0.0, 1.0)) = 0.0
		_0Paint2Intensity ("0 Paint 2 Intensity", Range (0.0, 1.0)) = 0.0
		_0WornEdgesNoiseMix ("0 Worn Edges Noise Mix", Vector) = (0,0,0,1)
		_0WornEdgesAmount ("0 Worn Edges Amount", Range (0.0, 100.0)) = 1.0
		_0WornEdgesOpacity ("0 Worn Edges Opacity", Range (0.0, 1.0)) = 0.0
		_0WornEdgesContrast ("0 Worn Edges Contrast", Range (1.0, 10.0)) = 2.0
		_0WornEdgesBorder ("0 Worn Edges Border", Range (0.0, 10.0)) = 0.0
		_0WornEdgesBorderTint("0 Worn Edges Border Tint", Color) = (0.5,0.5,0.5,1)
		_0UnderlyingDiffuseTint ("0 Underlying Diffuse Tint", Color) = (1,1,1,1)
		_0UnderlyingSpecularTint ("0 Underlying Specular Tint", Color) = (1,1,1,1)
		_0UnderlyingDiffuse ("0 Underlying Diffuse", Range (0.0, 1.0)) = 1.0
		_0UnderlyingSpecular ("0 Underlying Specular", Range (0.0, 1.0)) = 0.8
		_0UnderlyingGlossiness ("0 Underlying Glossiness", Range (0.0, 1.0)) = 0.5
		_NormalsStrength0 ("0 Normals Strength", Range (-1.0, 1)) = 0.1
		_Texture0 ("0 Texture", 2D) = "white" {}
		_BumpMap0("0 Normal Map", 2D) = "bump" {}
		_BumpMapStrength0 ("0 Normal Map Strength", Range (-2.0, 2)) = 1.0
		
		_OcclusionMap0("0 Occlusion Map", 2D) = "white" {}
		_OcclusionMapStrength0 ("0 Occlusion Map Strength", Range (0.0, 2)) = 1.0
		
		_SpecularMap0("0 Specular Map", 2D) = "black" {}
		_UseSpecularMap0 ("Use Specular Map 0", Range (0.0, 1)) = 0.0
		_GlossinessFromAlpha0 ("Glossiness From Alpha 0", Range (0.0, 1)) = 1.0
		
		_EmissionMap0("0 Emission Map", 2D) = "white" {}
		_EmissionMapTint0 ("0 Emission Map Tint", Color) = (0,0,0,1)
		_EmissionMapIntensity0 ("0 Emission Map Intensity", Range (0.0, 10.0)) = 1.0
		
		
		_0Paint1Specular ("0 Paint 1 Specular", Range (0.0, 1.0)) = 0.0
		_0Paint1Glossiness ("0 Paint 1 Glossiness", Range (0.0, 1.0)) = 0.0
		_0Paint1Color ("0 Paint 1 Color", Color) = (0.5,0.5,0.5,1)
		_0Paint1NoiseMix ("0 Paint 1 Noise Mix", Vector) = (0,0,0,0)
		_0Paint1MaskTex ("0 Paint 1 Mask", 2D) = "white" {}
		
		_0Paint2Specular ("0 Paint 2 Specular", Range (0.0, 1.0)) = 0.0
		_0Paint2Glossiness ("0 Paint 2 Glossiness", Range (0.0, 1.0)) = 0.0
		_0Paint2Color ("0 Paint 2 Color", Color) = (0.5,0.5,0.5,1)
		_0Paint2NoiseMix ("0 Paint 2 Noise Mix", Vector) = (0,0,0,0)
		_0Paint2MaskTex ("0 Paint 2 Mask", 2D) = "white" {}
		
		_0GlobalTransparency ("0 Global Transparency", Range (0.0, 1.0)) = 0.0
		_0AlbedoTransparency ("0 Albedo Transparency", Range (0.0, 1.0)) = 0.0
		_0Paint1Transparency ("0 Paint 1 Transparency", Range (0.0, 1.0)) = 0.0
		_0Paint2Transparency ("0 Paint 2 Transparency", Range (0.0, 1.0)) = 0.0
		
		_0MaterialRotation ("0 Material Rotation", Range (-360.0, 360.0)) = 0.0
		
		
		//dirt
		_Dirt1Tint ("Dirt 1 Tint", Color) = (1, 1, 1, 1)
		_DirtNoise1Mix ("Dirt 1 Noise Mix", Vector) = (1,0.5,0,0)
		_Dirt1Amount ("Dirt 1 Amount", Range (0.0, 20.0)) = 0.0
		_Dirt1Contrast ("Dirt 1 Contrast", Range (1.0, 8.0)) = 2.0
		_Dirt1Opacity ("Dirt 1 Opacity", Range (0.0, 1.0)) = 1.0
		_DirtTexture1 ("Dirt 1 Texture", 2D) = "white" {}
		
		_Dirt2Tint ("Dirt 2 Tint", Color) = (1, 1, 1, 1)
		_DirtNoise2Mix ("Dirt 2 Noise Mix", Vector) = (1,0.5,0,0)
		_Dirt2Amount ("Dirt 2 Amount", Range (0.0, 20.0)) = 0
		_Dirt2Contrast ("Dirt 2 Contrast", Range (1.0, 8.0)) = 2.0
		_Dirt2Opacity ("Dirt 2 Opacity", Range (0.0, 1.0)) = 1.0
		_DirtTexture2 ("Dirt 2 Texture", 2D) = "white" {}
	
		
		//emission
		_0EmissionTint ("Emission 0 Tint", Color) = (1,1,1,1)
		_0EmissionIntensity ("Emission 0 Intensity", Range (0.0, 20.0)) = 1.0
		_1EmissionTint ("Emission 1 Tint", Color) = (1,1,1,1)
		_1EmissionIntensity ("Emission 1 Intensity", Range (0.0, 20.0)) = 1.0
		_2EmissionTint ("Emission 2 Tint", Color) = (1,1,1,1)
		_2EmissionIntensity ("Emission 2 Intensity", Range (0.0, 20.0)) = 1.0

		_EmissionMask ("Emission Masks", 2D) = "black" {}
		
		
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.0
		
		//specular and glossiness mapping
		_specMin ("Specular Min", Range (0.0, 1.0)) = 0
		_specMax ("Specular Max", Range (0.0, 1.0)) = 1
		
		_glossMin ("Glossiness Min", Range (0.0, 1.0)) = 0
		_glossMax ("Glossiness Max", Range (0.0, 1.0)) = 1
		
		//levels
		_gamma ("Gamma", Range (0.0, 10.0)) = 1.0
		_minInput ("Min Input", Range (0.0, 1.0)) = 0
		_maxInput ("Max Input", Range (0.0, 1.0)) = 1
		_minOutput ("Min Output", Range (0.0, 1.0)) = 0
		_maxOutput ("Max Output", Range (0.0, 1.0)) = 1
		
		
		//HSBC final albedo
		_Hue ("Hue", Range (0.0, 1.0)) = 0
		_Saturation ("Saturation", Range (0.0, 1.0)) = 0.5
		_Brightness ("Brightness", Range (0.0, 1.0)) = 0.5
		_Contrast ("Contrast", Range (0.0, 1.0)) = 0.5
		
		_GlobalScale ("GlobalScale", Range (0.1, 2.0)) = 1.0
		
		_LinearColorSpace ("LinearColorSpace", Int) = 0 
	}
	
	CGINCLUDE
        #define _GLOSSYENV 1
        #define UNITY_SETUP_BRDF_INPUT SpecularSetup
    ENDCG
	
	
	SubShader { 
		Tags { "RenderType"="Opaque" "RenderMaterialIcon"="1"}
		LOD 300
		
		// Mask 0
		CGPROGRAM
		#pragma target 3.0
		#include "UnityPBSLighting.cginc"
		#include "Surforge.cginc"
		#pragma surface surf StandardSpecular  fullforwardshadows addshadow nolightmap vertex:vert
		
		sampler2D _Texture0;	
		sampler2D _BumpMap0;
		sampler2D _OcclusionMap0;
		fixed4 _Tint0;
		
		fixed4 _SpecularTint0;
		half _SpecularIntensity0;
		float _0SpecularContrast;
		float _0SpecularBrightness;

		half _Glossiness0;	
		half _GlossinessIntensity0;
		float _0GlossinessContrast;			
		float _0GlossinessBrightness;
				
		float _NormalsStrength0;
		float _BumpMapStrength0;
		float _OcclusionMapStrength0;
		
		sampler2D _SpecularMap0;
		float _UseSpecularMap0;
		float _GlossinessFromAlpha0;
		
		sampler2D _EmissionMap0;
		fixed4 _EmissionMapTint0;
		float _EmissionMapIntensity0;
		
		half _0WornEdgesContrast;
		half _0WornEdgesAmount;
		half _0WornEdgesBorder;
		half _0WornEdgesOpacity;
		fixed4 _0WornEdgesBorderTint;
		fixed4 _0WornEdgesNoiseMix;
		fixed4 _0UnderlyingDiffuseTint;
		fixed4 _0UnderlyingSpecularTint;
		half _0UnderlyingDiffuse;
		half _0UnderlyingSpecular;
		half _0UnderlyingGlossiness;
		
		half _0Paint1Intensity;
		half _0Paint2Intensity;	
		
		half _0Paint1Specular;
		half _0Paint1Glossiness;
		fixed4 _0Paint1Color;
		sampler2D _0Paint1MaskTex;
		fixed4 _0Paint1NoiseMix;
				
		half _0Paint2Specular;
		half _0Paint2Glossiness;
		fixed4 _0Paint2Color;
		sampler2D _0Paint2MaskTex;
		fixed4 _0Paint2NoiseMix;		
		
		half _0GlobalTransparency;
		half _0AlbedoTransparency;
		half _0Paint1Transparency;
		half _0Paint2Transparency;

		float _0MaterialRotation;
		
		float4 _Texture0_ST;
		float4 _BumpMap0_ST;
		float4 _EmissionMap0_ST;
		float4 _0Paint1MaskTex_ST;
		float4 _0Paint2MaskTex_ST;
		
		float4 _EmissionMask_ST;	


		struct Input {
			float2 uv_NormalMap;
			
			float2 texcoord_Texture0;
			float2 texcoord_BumpMap0;
			float2 texcoord_EmissionMap0;
			float2 texcoord_0Paint1MaskTex;
			float2 texcoord_0Paint2MaskTex;
			float2 texroord_Noise;
			
			float4 screenPos;
		};
		
		
		void vert (inout appdata_full v, out Input o) { 
			UNITY_INITIALIZE_OUTPUT(Input,o);
			
            float rad = radians(_0MaterialRotation);
			float s = sin (rad);
			float c = cos (rad);
			float2x2 rotationMatrix = float2x2( c, -s, s, c);
			
			o.texcoord_Texture0 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _Texture0) + 0.5;
			o.texcoord_BumpMap0 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _BumpMap0) + 0.5;
			o.texcoord_EmissionMap0 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _EmissionMap0) + 0.5;
			o.texcoord_0Paint1MaskTex = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _0Paint1MaskTex) + 0.5;
			o.texcoord_0Paint2MaskTex = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _0Paint2MaskTex) + 0.5;
			o.texroord_Noise = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _EmissionMask) + 0.5;
        }


		void surf (Input IN, inout SurfaceOutputStandardSpecular o) {
			fixed4 objectLabel = tex2D(_LabelTexture, IN.uv_NormalMap) * _LabelTint;
			fixed4 objectLabelAlpha = tex2D(_LabelTexture, IN.uv_NormalMap).a;

			fixed ao = GetResultAoMap(tex2D(_AoEdgesDirtDepth, IN.uv_NormalMap).r);
            fixed4 objectMasks = tex2D(_ObjectMasks, IN.uv_NormalMap); 
            fixed4 objectTexture0 = tex2D(_Texture0, IN.texcoord_Texture0);
            objectTexture0.a = 1;
            fixed edgeMask = tex2D(_AoEdgesDirtDepth, IN.uv_NormalMap).g; 
            fixed dirtMask = tex2D(_AoEdgesDirtDepth, IN.uv_NormalMap).b;

     
            fixed noiseR = tex2D(_EmissionMask, IN.texroord_Noise / _GlobalScale).b; 	
            fixed noiseB = tex2D(_EmissionMask, IN.texroord_Noise / _GlobalScale).a;     
            fixed noiseG1 = tex2D(_EmissionMask, IN.texroord_Noise / _GlobalScale + 0.5).b; 	
            fixed noiseG2 = tex2D(_EmissionMask, 1 - IN.texroord_Noise / _GlobalScale + 0.5).a;
            fixed noiseG = clamp( (1 - noiseG1) * 0.5 + (1 - noiseG2) * 0.5, 0, 1);
            
            fixed4 rgbaNoise = fixed4(noiseR, noiseG, noiseB, 1);
            
            //worn edges
            fixed edgeNoise = GetEdgeNoise(rgbaNoise, _0WornEdgesNoiseMix);
        	fixed wornEdges = GetWornEdges(ao, edgeMask, edgeNoise, _0WornEdgesContrast, _0WornEdgesAmount, _0WornEdgesOpacity); 
            fixed wornEdgesCenter = GetWornEdgesCenter(ao, edgeMask, edgeNoise, _0WornEdgesContrast, _0WornEdgesAmount, _0WornEdgesOpacity, _0WornEdgesBorder); 

            
            //paint
			//1
			fixed4 basePaint1 = tex2D(_0Paint1MaskTex, IN.texcoord_0Paint1MaskTex);
			fixed4 basePaint1Mask = tex2D(_0Paint1MaskTex, IN.texcoord_0Paint1MaskTex).a;	
			
			fixed maskNoiseBorder1 = GetMaskNoiseBorder(rgbaNoise, _0Paint1NoiseMix);			
			fixed4 pMask1 = GetPMask(basePaint1Mask, objectMasks.r, _0Paint1Intensity, maskNoiseBorder1);							  		   
           	fixed4 paintCol1 = GetPaintCol(pMask1, basePaint1, _0Paint1Color);
			fixed paintMask1 =  pMask1.a;
			
			
			//2
			fixed4 basePaint2 = tex2D(_0Paint2MaskTex, IN.texcoord_0Paint2MaskTex);
			fixed4 basePaint2Mask = tex2D(_0Paint2MaskTex, IN.texcoord_0Paint2MaskTex).a;	
				
			fixed maskNoiseBorder2 = GetMaskNoiseBorder(rgbaNoise, _0Paint2NoiseMix);			
			fixed4 pMask2 = GetPMask(basePaint2Mask, objectMasks.r, _0Paint2Intensity, maskNoiseBorder2);							  		   
           	fixed4 paintCol2 = GetPaintCol(pMask2, basePaint2, _0Paint2Color);
			fixed paintMask2 =  pMask2.a;
			
			
			//dirt
			fixed baseDirt = GetBaseDirt(dirtMask);
			
			//dirts1
			fixed dirtNoise1 = GetDirtNoise(rgbaNoise, _DirtNoise1Mix);		
			fixed dirts1Mask = GetDirtMask(dirtNoise1, dirtMask, _Dirt1Amount, _Dirt1Contrast, _Dirt1Opacity); 						
			fixed4 dirts1 = GetDirt(dirts1Mask, _Dirt1Tint);
					
			//dirts2
			fixed dirtNoise2 = GetDirtNoise(rgbaNoise, _DirtNoise2Mix);		
			fixed dirts2Mask = GetDirtMask(dirtNoise2, dirtMask, _Dirt2Amount, _Dirt2Contrast, _Dirt2Opacity); 						
			fixed4 dirts2 = GetDirt(dirts2Mask, _Dirt2Tint);
            			
			
			//Specular/Glossiness
			fixed4 resultSpecularMap = 0;
			fixed resultGlossinessMap = 0;
			fixed objectTexture0gloss = 0;
			
			if (_UseSpecularMap0 > 0.5) {
				fixed4 objectTexture0specRGB = GetSpecMap(tex2D(_SpecularMap0, IN.texcoord_BumpMap0 / _GlobalScale), _0SpecularContrast, _0SpecularBrightness); 
				resultSpecularMap = GetResultSpecularMapRGB(objectTexture0specRGB, _SpecularIntensity0, _SpecularTint0, paintMask1, paintMask2, _0Paint1Specular, _0Paint2Specular, objectLabelAlpha, _LabelSpecularIntensity, wornEdges, _0UnderlyingSpecular, _0UnderlyingSpecularTint, dirts1Mask, dirts2Mask, _specMin, _specMax, _0GlobalTransparency);
				
				if (_GlossinessFromAlpha0 > 0.5) objectTexture0gloss = GetGlossMap(tex2D(_SpecularMap0, IN.texcoord_BumpMap0 / _GlobalScale).a, _0GlossinessContrast, _0GlossinessBrightness);
				else {
					if (_LinearColorSpace != 0) objectTexture0gloss = GetGlossMap(pow (tex2D(_SpecularMap0, IN.texcoord_BumpMap0 / _GlobalScale), 0.454545), _0GlossinessContrast, _0GlossinessBrightness);
					else objectTexture0gloss = GetGlossMap(tex2D(_SpecularMap0, IN.texcoord_BumpMap0 / _GlobalScale), _0GlossinessContrast, _0GlossinessBrightness);
				}	
				resultGlossinessMap = GetResultGlossinessMapFromAlpha(objectTexture0gloss, _GlossinessIntensity0, _Glossiness0, paintMask1, paintMask2, _0Paint1Glossiness, _0Paint2Glossiness, objectLabelAlpha, _LabelGlossiness, wornEdges, _0UnderlyingGlossiness, _glossMin, _glossMax, _0GlobalTransparency);
			}
			else {
			 	fixed objectTexture0spec = GetSpecMap(tex2D(_Texture0, IN.texcoord_Texture0 / _GlobalScale), _0SpecularContrast, _0SpecularBrightness); 
				resultSpecularMap = GetResultSpecularMap(objectTexture0spec, _SpecularIntensity0, _SpecularTint0, paintMask1, paintMask2, _0Paint1Specular, _0Paint2Specular, objectLabelAlpha, _LabelSpecularIntensity, wornEdges, _0UnderlyingSpecular, _0UnderlyingSpecularTint, dirts1Mask, dirts2Mask, _specMin, _specMax, _0GlobalTransparency);	
				
				if (_LinearColorSpace != 0) objectTexture0gloss = GetGlossMap(pow (tex2D(_Texture0, IN.texcoord_Texture0 / _GlobalScale), 0.454545), _0GlossinessContrast, _0GlossinessBrightness);
				else objectTexture0gloss = GetGlossMap(tex2D(_Texture0, IN.texcoord_Texture0 / _GlobalScale), _0GlossinessContrast, _0GlossinessBrightness);

				
				resultGlossinessMap = GetResultGlossinessMap(objectTexture0gloss, _GlossinessIntensity0, _Glossiness0, paintMask1, paintMask2, _0Paint1Glossiness, _0Paint2Glossiness, objectLabelAlpha, _LabelGlossiness, wornEdges, _0UnderlyingGlossiness, _glossMin, _glossMax, _0GlobalTransparency);
			} 
			
			
			
			//Diffuse
			fixed4 diffuse = GetDiffuse(objectTexture0, _Tint0, paintMask1, paintMask2, _0Paint1Color, _0Paint2Color, paintCol1, paintCol2, objectLabelAlpha, objectLabel, dirts1Mask, dirts2Mask, dirts1, dirts2, _0UnderlyingDiffuseTint, wornEdges, _0WornEdgesBorderTint, _0UnderlyingDiffuse, wornEdgesCenter);

						
			//Emission
			fixed4 emissionMask = tex2D(_EmissionMask, IN.uv_NormalMap);
			fixed4 emissionMap = tex2D(_EmissionMap0, IN.texcoord_EmissionMap0 / _GlobalScale);
			fixed4 finalEmission = GetEmission(emissionMask, _0EmissionTint, _1EmissionTint, _0EmissionIntensity, _1EmissionIntensity, emissionMap, _EmissionMapTint0, _EmissionMapIntensity0);
			
			
			//Material AO
			fixed materialAO = GetMaterialAO(tex2D(_OcclusionMap0, IN.texcoord_BumpMap0 / _GlobalScale).r, _OcclusionMapStrength0);
			

			//levels 
			fixed4 finalAlbedo = diffuse * ao * materialAO;
	
			float3 finalAlbedoLevels = LevelsControl(float3 (finalAlbedo.r, finalAlbedo.g, finalAlbedo.b), _minInput, _gamma, _maxInput, _minOutput, _maxOutput);
			
			float4 finalColor = fixed4(finalAlbedoLevels.r, finalAlbedoLevels.g, finalAlbedoLevels.b, 1);

				
			//Normal detailing
			float me = tex2D(_Texture0,IN.texcoord_Texture0).x;
			float n = tex2D(_Texture0,float2(IN.texcoord_Texture0.x,IN.texcoord_Texture0.y+1.0/2048)).x;
			float s = tex2D(_Texture0,float2(IN.texcoord_Texture0.x,IN.texcoord_Texture0.y-1.0/2048)).x;
			float e = tex2D(_Texture0,float2(IN.texcoord_Texture0.x-1.0/2048,IN.texcoord_Texture0.y)).x;
			float w = tex2D(_Texture0,float2(IN.texcoord_Texture0.x+1.0/2048,IN.texcoord_Texture0.y)).x;
			
			float3 finalNormal = GetFinalNormal(tex2D(_NormalMap, IN.uv_NormalMap), me, n, s, e, w, objectMasks.r, _NormalsStrength0, tex2D(_BumpMap0, IN.texcoord_BumpMap0), _BumpMapStrength0, _0MaterialRotation); 


			o.Albedo = applyHSBEffect(finalColor, float4(_Hue, _Brightness, _Contrast, _Saturation));
			o.Occlusion = ao * materialAO;
			o.Specular = resultSpecularMap;	
			o.Smoothness = resultGlossinessMap;
			o.Normal = finalNormal * 2-1;
			o.Emission = applyHSBEffect(finalEmission, float4(_Hue, _Brightness, _Contrast, _Saturation));
		}
		ENDCG
	
		
		
		
		
	}

	//FallBack "Specular"
	CustomEditor "SurforgeSingleShaderGUI"
}