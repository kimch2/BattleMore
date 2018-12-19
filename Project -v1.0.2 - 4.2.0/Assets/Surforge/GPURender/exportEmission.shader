// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/ExportEmission" {
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
		_ObjectMasks ("Object Masks", 2D) = "black" {}
		_ObjectMasks2 ("Object Masks2", 2D) = "black" {}
		
		_Tint0 ("0 Tint", Color) = (1,1,1,1)
		_SpecularTint0 ("0 Specular Tint", Color) = (1,1,1,1)
		
		_SpecularIntensity0 ("0 Specular Intensity", Range (0.0, 1.0)) = 0.2
		_0SpecularContrast ("0 Specular Contrast", Range (0, 1.0)) = 0.1
		_0SpecularBrightness ("0 Specular Brightness", Range (0, 1.0)) = 0.5
		
		_Glossiness0 ("0 Glossiness", Range (0.0, 1.0)) = 0.4
		_0GlossinessContrast ("0 Glossiness Contrast", Range (0.0, 1.0)) = 0.2
		_0GlossinessBrightness ("0 Glossiness Brightness", Range (0.0, 1.0)) = 0.1
		
		_0Paint1Intensity ("0 Paint 1 Intensity", Range (0.0, 1.0)) = 1.0
		_0Paint2Intensity ("0 Paint 2 Intensity", Range (0.0, 1.0)) = 0.0
		_0WornEdgesNoiseMix ("0 Worn Edges Noise Mix", Vector) = (0,0,0,1)
		_0WornEdgesAmount ("0 Worn Edges Amount", Range (0.0, 100.0)) = 8.0
		_0WornEdgesOpacity ("0 Worn Edges Opacity", Range (0.0, 1.0)) = 0.0
		_0WornEdgesContrast ("0 Worn Edges Contrast", Range (1.0, 10.0)) = 2.0
		_0WornEdgesBorder ("0 Worn Edges Border", Range (0.0, 10.0)) = 0.5
		_0WornEdgesBorderTint("0 Worn Edges Border Tint", Color) = (0.5,0.5,0.5,1)
		_0UnderlyingDiffuseTint ("0 Underlying Diffuse Tint", Color) = (1,1,1,1)
		_0UnderlyingSpecularTint ("0 Underlying Specular Tint", Color) = (1,1,1,1)
		_0UnderlyingDiffuse ("0 Underlying Diffuse", Range (0.0, 1.0)) = 1.0
		_0UnderlyingSpecular ("0 Underlying Specular", Range (0.0, 1.0)) = 0.8
		_0UnderlyingGlossiness ("0 Underlying Glossiness", Range (0.0, 1.0)) = 0.5
		_NormalsStrength0 ("0 Normals Strength", Range (-1.0, 1)) = 0.1
		_Texture0 ("0 Texture", 2D) = "white" {}
		
		_EmissionMap0("0 Emission Map", 2D) = "black" {}
		_EmissionMapTint0 ("0 Emission Map Tint", Color) = (1,1,1,1)
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
		
		_0MaterialRotation ("0 Material Rotation", Range (-360.0, 360.0)) = 0.0
		
		
		_Tint1 ("1 Tint", Color) = (1,1,1,1)
		_SpecularTint1 ("1 Specular Tint", Color) = (1,1,1,1)
		
		_SpecularIntensity1 ("1 Specular Intensity", Range (0.0, 1.0)) = 0.2
		_1SpecularContrast ("1 Specular Contrast", Range (0, 1.0)) = 0.1
		_1SpecularBrightness ("1 Specular Brightness", Range (0, 1.0)) = 0.5
		
		_Glossiness1 ("1 Glossiness", Range (0.0, 1.0)) = 0.4
		_1GlossinessContrast ("1 Glossiness Contrast", Range (0.0, 1.0)) = 0.2
		_1GlossinessBrightness ("1 Glossiness Brightness", Range (0.0, 1.0)) = 0.1
		
		_1Paint1Intensity ("1 Paint 1 Intensity", Range (0.0, 1.0)) = 0.0
		_1Paint2Intensity ("1 Paint 2 Intensity", Range (0.0, 1.0)) = 0.0
		_1WornEdgesNoiseMix ("1 Worn Edges Noise Mix", Vector) = (0,0,0,1)
		_1WornEdgesAmount ("1 Worn Edges Amount", Range (0.0, 100.0)) = 8.0
		_1WornEdgesOpacity ("1 Worn Edges Opacity", Range (0.0, 1.0)) = 0.0
		_1WornEdgesContrast ("1 Worn Edges Contrast", Range (1.0, 10.0)) = 2.0
		_1WornEdgesBorder ("1 Worn Edges Border", Range (0.0, 10.0)) = 0.5
		_1WornEdgesBorderTint("1 Worn Edges Border Tint", Color) = (0.5,0.5,0.5,1)
		_1UnderlyingDiffuseTint ("1 Underlying Diffuse Tint", Color) = (1,1,1,1)
		_1UnderlyingSpecularTint ("1 Underlying Specular Tint", Color) = (1,1,1,1)
		_1UnderlyingDiffuse ("1 Underlying Diffuse", Range (0.0, 1.0)) = 1.0
		_1UnderlyingSpecular ("1 Underlying Specular", Range (0.0, 1.0)) = 0.8
		_1UnderlyingGlossiness ("1 Underlying Glossiness", Range (0.0, 1.0)) = 0.5
		_NormalsStrength1 ("1 Normals Strength", Range (-1.0, 1)) = 0.1
		_Texture1 ("1 Texture", 2D) = "white" {}
		
		_EmissionMap1("1 Emission Map", 2D) = "black" {}
		_EmissionMapTint1 ("1 Emission Map Tint", Color) = (1,1,1,1)
		_EmissionMapIntensity1 ("1 Emission Map Intensity", Range (0.0, 10.0)) = 1.0
		
		_1Paint1Specular ("1 Paint 1 Specular", Range (0.0, 1.0)) = 0.0
		_1Paint1Glossiness ("1 Paint 1 Glossiness", Range (0.0, 1.0)) = 0.0
		_1Paint1Color ("1 Paint 1 Color", Color) = (0.5,0.5,0.5,1)
		_1Paint1NoiseMix ("1 Paint 1 Noise Mix", Vector) = (0,0,0,0)
		_1Paint1MaskTex ("1 Paint 1 Mask", 2D) = "white" {}
		
		_1Paint2Specular ("1 Paint 2 Specular", Range (0.0, 1.0)) = 0.0
		_1Paint2Glossiness ("1 Paint 2 Glossiness", Range (0.0, 1.0)) = 0.0
		_1Paint2Color ("1 Paint 2 Color", Color) = (0.5,0.5,0.5,1)
		_1Paint2NoiseMix ("1 Paint 2 Noise Mix", Vector) = (0,0,0,0)
		_1Paint2MaskTex ("1 Paint 2 Mask", 2D) = "white" {}
		
		_1MaterialRotation ("1 Material Rotation", Range (-360.0, 360.0)) = 0.0
		
		
		_Tint2 ("2 Tint", Color) = (1,1,1,1)
		_SpecularTint2 ("2 Specular Tint", Color) = (1,1,1,1)
		
		_SpecularIntensity2 ("2 Specular Intensity", Range (0.0, 1.0)) = 0.2
		_2SpecularContrast ("2 Specular Contrast", Range (0, 1.0)) = 0.1
		_2SpecularBrightness ("2 Specular Brightness", Range (0, 1.0)) = 0.5
		
		_Glossiness2 ("2 Glossiness", Range (0.0, 1.0)) = 0.4
		_2GlossinessContrast ("2 Glossiness Contrast", Range (0.0, 1.0)) = 0.2
		_2GlossinessBrightness ("2 Glossiness Brightness", Range (0.0, 1.0)) = 0.1
		
		_2Paint1Intensity ("2 Paint 1 Intensity", Range (0.0, 1.0)) = 0.0
		_2Paint2Intensity ("2 Paint 2 Intensity", Range (0.0, 1.0)) = 0.0
		_2WornEdgesNoiseMix ("2 Worn Edges Noise Mix", Vector) = (0,0,0,1)
		_2WornEdgesAmount ("2 Worn Edges Amount", Range (0.0, 100.0)) = 8.0
		_2WornEdgesOpacity ("2 Worn Edges Opacity", Range (0.0, 1.0)) = 0.0
		_2WornEdgesContrast ("2 Worn Edges Contrast", Range (1.0, 10.0)) = 2.0
		_2WornEdgesBorder ("2 Worn Edges Border", Range (0.0, 10.0)) = 0.5
		_2WornEdgesBorderTint("2 Worn Edges Border Tint", Color) = (0.5,0.5,0.5,1)
		_2UnderlyingDiffuseTint ("2 Underlying Diffuse Tint", Color) = (1,1,1,1)
		_2UnderlyingSpecularTint ("2 Underlying Specular Tint", Color) = (1,1,1,1)
		_2UnderlyingDiffuse ("2 Underlying Diffuse", Range (0.0, 1.0)) = 1.0
		_2UnderlyingSpecular ("2 Underlying Specular", Range (0.0, 1.0)) = 0.8
		_2UnderlyingGlossiness ("2 Underlying Glossiness", Range (0.0, 1.0)) = 0.5
		_NormalsStrength2 ("2 Normals Strength", Range (-1.0, 1)) = 0.1
		_Texture2 ("2 Texture", 2D) = "white" {}
		
		_EmissionMap2("2 Emission Map", 2D) = "black" {}
		_EmissionMapTint2 ("2 Emission Map Tint", Color) = (1,1,1,1)
		_EmissionMapIntensity2 ("2 Emission Map Intensity", Range (0.0, 10.0)) = 1.0
		
		_2Paint1Specular ("2 Paint 1 Specular", Range (0.0, 1.0)) = 0.0
		_2Paint1Glossiness ("2 Paint 1 Glossiness", Range (0.0, 1.0)) = 0.0
		_2Paint1Color ("2 Paint 1 Color", Color) = (0.5,0.5,0.5,1)
		_2Paint1NoiseMix ("2 Paint 1 Noise Mix", Vector) = (0,0,0,0)
		_2Paint1MaskTex ("2 Paint 1 Mask", 2D) = "white" {}
		
		_2Paint2Specular ("2 Paint 2 Specular", Range (0.0, 1.0)) = 0.0
		_2Paint2Glossiness ("2 Paint 2 Glossiness", Range (0.0, 1.0)) = 0.0
		_2Paint2Color ("2 Paint 2 Color", Color) = (0.5,0.5,0.5,1)
		_2Paint2NoiseMix ("2 Paint 2 Noise Mix", Vector) = (0,0,0,0)
		_2Paint2MaskTex ("2 Paint 2 Mask", 2D) = "white" {}
		
		_2MaterialRotation ("2 Material Rotation", Range (-360.0, 360.0)) = 0.0
		
		
		_Tint3 ("3 Tint", Color) = (1,1,1,1)
		_SpecularTint3 ("3 Specular Tint", Color) = (1,1,1,1)
		
		_SpecularIntensity3 ("3 Specular Intensity", Range (0.0, 1.0)) = 0.2
		_3SpecularContrast ("3 Specular Contrast", Range (0, 1.0)) = 0.1
		_3SpecularBrightness ("3 Specular Brightness", Range (0, 1.0)) = 0.5
		
		_Glossiness3 ("3 Glossiness", Range (0.0, 1.0)) = 0.4
		_3GlossinessContrast ("3 Glossiness Contrast", Range (0.0, 1.0)) = 0.2
		_3GlossinessBrightness ("3 Glossiness Brightness", Range (0.0, 1.0)) = 0.1
		
		_3Paint1Intensity ("3 Paint 1 Intensity", Range (0.0, 1.0)) = 0.0
		_3Paint2Intensity ("3 Paint 2 Intensity", Range (0.0, 1.0)) = 0.0
		_3WornEdgesNoiseMix ("3 Worn Edges Noise Mix", Vector) = (0,0,0,1)
		_3WornEdgesAmount ("3 Worn Edges Amount", Range (0.0, 100.0)) = 8.0
		_3WornEdgesOpacity ("3 Worn Edges Opacity", Range (0.0, 1.0)) = 0.0
		_3WornEdgesContrast ("3 Worn Edges Contrast", Range (1.0, 10.0)) = 2.0
		_3WornEdgesBorder ("3 Worn Edges Border", Range (0.0, 10.0)) = 0.5
		_3WornEdgesBorderTint("3 Worn Edges Border Tint", Color) = (0.5,0.5,0.5,1)
		_3UnderlyingDiffuseTint ("3 Underlying Diffuse Tint", Color) = (1,1,1,1)
		_3UnderlyingSpecularTint ("3 Underlying Specular Tint", Color) = (1,1,1,1)
		_3UnderlyingDiffuse ("3 Underlying Diffuse", Range (0.0, 1.0)) = 1.0
		_3UnderlyingSpecular ("3 Underlying Specular", Range (0.0, 1.0)) = 0.8
		_3UnderlyingGlossiness ("3 Underlying Glossiness", Range (0.0, 1.0)) = 0.5
		_NormalsStrength3 ("3 Normals Strength", Range (-1.0, 1)) = 0.1
		_Texture3 ("3 Texture", 2D) = "white" {}
		
		_EmissionMap3("3 Emission Map", 2D) = "black" {}
		_EmissionMapTint3 ("3 Emission Map Tint", Color) = (1,1,1,1)
		_EmissionMapIntensity3 ("3 Emission Map Intensity", Range (0.0, 10.0)) = 1.0
		
		_3Paint1Specular ("3 Paint 1 Specular", Range (0.0, 1.0)) = 0.0
		_3Paint1Glossiness ("3 Paint 1 Glossiness", Range (0.0, 1.0)) = 0.0
		_3Paint1Color ("3 Paint 1 Color", Color) = (0.5,0.5,0.5,1)
		_3Paint1NoiseMix ("3 Paint 1 Noise Mix", Vector) = (0,0,0,0)
		_3Paint1MaskTex ("3 Paint 1 Mask", 2D) = "white" {}
		
		_3Paint2Specular ("3 Paint 2 Specular", Range (0.0, 1.0)) = 0.0
		_3Paint2Glossiness ("3 Paint 2 Glossiness", Range (0.0, 1.0)) = 0.0
		_3Paint2Color ("3 Paint 2 Color", Color) = (0.5,0.5,0.5,1)
		_3Paint2NoiseMix ("3 Paint 2 Noise Mix", Vector) = (0,0,0,0)
		_3Paint2MaskTex ("3 Paint 2 Mask", 2D) = "white" {}
		
		_3MaterialRotation ("3 Material Rotation", Range (-360.0, 360.0)) = 0.0
		
		
		_Tint4 ("4 Tint", Color) = (1,1,1,1)
		_SpecularTint4 ("4 Specular Tint", Color) = (1,1,1,1)
		
		_SpecularIntensity4 ("4 Specular Intensity", Range (0.0, 1.0)) = 0.2
		_4SpecularContrast ("4 Specular Contrast", Range (0, 1.0)) = 0.1
		_4SpecularBrightness ("4 Specular Brightness", Range (0, 1.0)) = 0.5
		
		_Glossiness4 ("4 Glossiness", Range (0.0, 1.0)) = 0.4
		_4GlossinessContrast ("4 Glossiness Contrast", Range (0.0, 1.0)) = 0.2
		_4GlossinessBrightness ("4 Glossiness Brightness", Range (0.0, 1.0)) = 0.1
		
		_4Paint1Intensity ("4 Paint 1 Intensity", Range (0.0, 1.0)) = 1.0
		_4Paint2Intensity ("4 Paint 2 Intensity", Range (0.0, 1.0)) = 0.0
		_4WornEdgesNoiseMix ("4 Worn Edges Noise Mix", Vector) = (0,0,0,1)
		_4WornEdgesAmount ("4 Worn Edges Amount", Range (0.0, 100.0)) = 8.0
		_4WornEdgesOpacity ("4 Worn Edges Opacity", Range (0.0, 1.0)) = 0.0
		_4WornEdgesContrast ("4 Worn Edges Contrast", Range (1.0, 10.0)) = 2.0
		_4WornEdgesBorder ("4 Worn Edges Border", Range (0.0, 10.0)) = 0.5
		_4WornEdgesBorderTint("4 Worn Edges Border Tint", Color) = (0.5,0.5,0.5,1)
		_4UnderlyingDiffuseTint ("4 Underlying Diffuse Tint", Color) = (1,1,1,1)
		_4UnderlyingSpecularTint ("4 Underlying Specular Tint", Color) = (1,1,1,1)
		_4UnderlyingDiffuse ("4 Underlying Diffuse", Range (0.0, 1.0)) = 1.0
		_4UnderlyingSpecular ("4 Underlying Specular", Range (0.0, 1.0)) = 0.8
		_4UnderlyingGlossiness ("4 Underlying Glossiness", Range (0.0, 1.0)) = 0.5
		_NormalsStrength4 ("4 Normals Strength", Range (-1.0, 1)) = 0.1
		_Texture4 ("4 Texture", 2D) = "white" {}
		
		_EmissionMap4("4 Emission Map", 2D) = "black" {}
		_EmissionMapTint4 ("4 Emission Map Tint", Color) = (1,1,1,1)
		_EmissionMapIntensity4 ("4 Emission Map Intensity", Range (0.0, 10.0)) = 1.0
		
		_4Paint1Specular ("4 Paint 1 Specular", Range (0.0, 1.0)) = 0.0
		_4Paint1Glossiness ("4 Paint 1 Glossiness", Range (0.0, 1.0)) = 0.0
		_4Paint1Color ("4 Paint 1 Color", Color) = (0.5,0.5,0.5,1)
		_4Paint1NoiseMix ("4 Paint 1 Noise Mix", Vector) = (0,0,0,0)
		_4Paint1MaskTex ("4 Paint 1 Mask", 2D) = "white" {}
		
		_4Paint2Specular ("4 Paint 2 Specular", Range (0.0, 1.0)) = 0.0
		_4Paint2Glossiness ("4 Paint 2 Glossiness", Range (0.0, 1.0)) = 0.0
		_4Paint2Color ("4 Paint 2 Color", Color) = (0.5,0.5,0.5,1)
		_4Paint2NoiseMix ("4 Paint 2 Noise Mix", Vector) = (0,0,0,0)
		_4Paint2MaskTex ("4 Paint 2 Mask", 2D) = "white" {}
		
		_4MaterialRotation ("4 Material Rotation", Range (-360.0, 360.0)) = 0.0
		
		
		_Tint5 ("5 Tint", Color) = (1,1,1,1)
		_SpecularTint5 ("5 Specular Tint", Color) = (1,1,1,1)
		
		_SpecularIntensity5 ("5 Specular Intensity", Range (0.0, 1.0)) = 0.2
		_5SpecularContrast ("5 Specular Contrast", Range (0, 1.0)) = 0.1
		_5SpecularBrightness ("5 Specular Brightness", Range (0, 1.0)) = 0.5
		
		_Glossiness5 ("5 Glossiness", Range (0.0, 1.0)) = 0.4
		_5GlossinessContrast ("5 Glossiness Contrast", Range (0.0, 1.0)) = 0.2
		_5GlossinessBrightness ("5 Glossiness Brightness", Range (0.0, 1.0)) = 0.1
		
		_5Paint1Intensity ("5 Paint 1 Intensity", Range (0.0, 1.0)) = 1.0
		_5Paint2Intensity ("5 Paint 2 Intensity", Range (0.0, 1.0)) = 0.0
		_5WornEdgesNoiseMix ("5 Worn Edges Noise Mix", Vector) = (0,0,0,1)
		_5WornEdgesAmount ("5 Worn Edges Amount", Range (0.0, 100.0)) = 8.0
		_5WornEdgesOpacity ("5 Worn Edges Opacity", Range (0.0, 1.0)) = 0.0
		_5WornEdgesContrast ("5 Worn Edges Contrast", Range (1.0, 10.0)) = 2.0
		_5WornEdgesBorder ("5 Worn Edges Border", Range (0.0, 10.0)) = 0.5
		_5WornEdgesBorderTint("5 Worn Edges Border Tint", Color) = (0.5,0.5,0.5,1)
		_5UnderlyingDiffuseTint ("5 Underlying Diffuse Tint", Color) = (1,1,1,1)
		_5UnderlyingSpecularTint ("5 Underlying Specular Tint", Color) = (1,1,1,1)
		_5UnderlyingDiffuse ("5 Underlying Diffuse", Range (0.0, 1.0)) = 1.0
		_5UnderlyingSpecular ("5 Underlying Specular", Range (0.0, 1.0)) = 0.8
		_5UnderlyingGlossiness ("5 Underlying Glossiness", Range (0.0, 1.0)) = 0.5
		_NormalsStrength5 ("5 Normals Strength", Range (-1.0, 1)) = 0.1
		_Texture5 ("5 Texture", 2D) = "white" {}
		
		_EmissionMap5("5 Emission Map", 2D) = "black" {}
		_EmissionMapTint5 ("5 Emission Map Tint", Color) = (1,1,1,1)
		_EmissionMapIntensity5 ("5 Emission Map Intensity", Range (0.0, 10.0)) = 1.0
		
		_5Paint1Specular ("5 Paint 1 Specular", Range (0.0, 1.0)) = 0.0
		_5Paint1Glossiness ("5 Paint 1 Glossiness", Range (0.0, 1.0)) = 0.0
		_5Paint1Color ("5 Paint 1 Color", Color) = (0.5,0.5,0.5,1)
		_5Paint1NoiseMix ("5 Paint 1 Noise Mix", Vector) = (0,0,0,0)
		_5Paint1MaskTex ("5 Paint 1 Mask", 2D) = "white" {}
		
		_5Paint2Specular ("5 Paint 2 Specular", Range (0.0, 1.0)) = 0.0
		_5Paint2Glossiness ("5 Paint 2 Glossiness", Range (0.0, 1.0)) = 0.0
		_5Paint2Color ("5 Paint 2 Color", Color) = (0.5,0.5,0.5,1)
		_5Paint2NoiseMix ("5 Paint 2 Noise Mix", Vector) = (0,0,0,0)
		_5Paint2MaskTex ("5 Paint 2 Mask", 2D) = "white" {}
		
		_5MaterialRotation ("5 Material Rotation", Range (-360.0, 360.0)) = 0.0
		
		
		_Tint6 ("6 Tint", Color) = (1,1,1,1)
		_SpecularTint6 ("6 Specular Tint", Color) = (1,1,1,1)
		
		_SpecularIntensity6 ("6 Specular Intensity", Range (0.0, 1.0)) = 0.2
		_6SpecularContrast ("6 Specular Contrast", Range (0, 1.0)) = 0.1
		_6SpecularBrightness ("6 Specular Brightness", Range (0, 1.0)) = 0.5
		
		_Glossiness6 ("6 Glossiness", Range (0.0, 1.0)) = 0.4
		_6GlossinessContrast ("6 Glossiness Contrast", Range (0.0, 1.0)) = 0.2
		_6GlossinessBrightness ("6 Glossiness Brightness", Range (0.0, 1.0)) = 0.1
		
		_6Paint1Intensity ("6 Paint 1 Intensity", Range (0.0, 1.0)) = 1.0
		_6Paint2Intensity ("6 Paint 2 Intensity", Range (0.0, 1.0)) = 0.0
		_6WornEdgesNoiseMix ("6 Worn Edges Noise Mix", Vector) = (0,0,0,1)
		_6WornEdgesAmount ("6 Worn Edges Amount", Range (0.0, 100.0)) = 8.0
		_6WornEdgesOpacity ("6 Worn Edges Opacity", Range (0.0, 1.0)) = 0.0
		_6WornEdgesContrast ("6 Worn Edges Contrast", Range (1.0, 10.0)) = 2.0
		_6WornEdgesBorder ("6 Worn Edges Border", Range (0.0, 10.0)) = 0.5
		_6WornEdgesBorderTint("6 Worn Edges Border Tint", Color) = (0.5,0.5,0.5,1)
		_6UnderlyingDiffuseTint ("6 Underlying Diffuse Tint", Color) = (1,1,1,1)
		_6UnderlyingSpecularTint ("6 Underlying Specular Tint", Color) = (1,1,1,1)
		_6UnderlyingDiffuse ("6 Underlying Diffuse", Range (0.0, 1.0)) = 1.0
		_6UnderlyingSpecular ("6 Underlying Specular", Range (0.0, 1.0)) = 0.8
		_6UnderlyingGlossiness ("6 Underlying Glossiness", Range (0.0, 1.0)) = 0.5
		_NormalsStrength6 ("6 Normals Strength", Range (-1.0, 1)) = 0.1
		_Texture6 ("6 Texture", 2D) = "white" {}
		
		_EmissionMap6("6 Emission Map", 2D) = "black" {}
		_EmissionMapTint6 ("6 Emission Map Tint", Color) = (1,1,1,1)
		_EmissionMapIntensity6 ("6 Emission Map Intensity", Range (0.0, 10.0)) = 1.0
		
		_6Paint1Specular ("6 Paint 1 Specular", Range (0.0, 1.0)) = 0.0
		_6Paint1Glossiness ("6 Paint 1 Glossiness", Range (0.0, 1.0)) = 0.0
		_6Paint1Color ("6 Paint 1 Color", Color) = (0.5,0.5,0.5,1)
		_6Paint1NoiseMix ("6 Paint 1 Noise Mix", Vector) = (0,0,0,0)
		_6Paint1MaskTex ("6 Paint 1 Mask", 2D) = "white" {}
		
		_6Paint2Specular ("6 Paint 2 Specular", Range (0.0, 1.0)) = 0.0
		_6Paint2Glossiness ("6 Paint 2 Glossiness", Range (0.0, 1.0)) = 0.0
		_6Paint2Color ("6 Paint 2 Color", Color) = (0.5,0.5,0.5,1)
		_6Paint2NoiseMix ("6 Paint 2 Noise Mix", Vector) = (0,0,0,0)
		_6Paint2MaskTex ("6 Paint 2 Mask", 2D) = "white" {}
		
		_6MaterialRotation ("6 Material Rotation", Range (-360.0, 360.0)) = 0.0
		
		
		_Tint7 ("7 Tint", Color) = (1,1,1,1)
		_SpecularTint7 ("7 Specular Tint", Color) = (1,1,1,1)
		
		_SpecularIntensity7 ("7 Specular Intensity", Range (0.0, 1.0)) = 0.2
		_7SpecularContrast ("7 Specular Contrast", Range (0, 1.0)) = 0.1
		_7SpecularBrightness ("7 Specular Brightness", Range (0, 1.0)) = 0.5
		
		_Glossiness7 ("7 Glossiness", Range (0.0, 1.0)) = 0.4
		_7GlossinessContrast ("7 Glossiness Contrast", Range (0.0, 1.0)) = 0.2
		_7GlossinessBrightness ("7 Glossiness Brightness", Range (0.0, 1.0)) = 0.1
		
		_7Paint1Intensity ("7 Paint 1 Intensity", Range (0.0, 1.0)) = 1.0
		_7Paint2Intensity ("7 Paint 2 Intensity", Range (0.0, 1.0)) = 0.0
		_7WornEdgesNoiseMix ("7 Worn Edges Noise Mix", Vector) = (0,0,0,1)
		_7WornEdgesAmount ("7 Worn Edges Amount", Range (0.0, 100.0)) = 8.0
		_7WornEdgesOpacity ("7 Worn Edges Opacity", Range (0.0, 1.0)) = 0.0
		_7WornEdgesContrast ("7 Worn Edges Contrast", Range (1.0, 10.0)) = 2.0
		_7WornEdgesBorder ("7 Worn Edges Border", Range (0.0, 10.0)) = 0.5
		_7WornEdgesBorderTint("7 Worn Edges Border Tint", Color) = (0.5,0.5,0.5,1)
		_7UnderlyingDiffuseTint ("7 Underlying Diffuse Tint", Color) = (1,1,1,1)
		_7UnderlyingSpecularTint ("7 Underlying Specular Tint", Color) = (1,1,1,1)
		_7UnderlyingDiffuse ("7 Underlying Diffuse", Range (0.0, 1.0)) = 1.0
		_7UnderlyingSpecular ("7 Underlying Specular", Range (0.0, 1.0)) = 0.8
		_7UnderlyingGlossiness ("7 Underlying Glossiness", Range (0.0, 1.0)) = 0.5
		_NormalsStrength7 ("7 Normals Strength", Range (-1.0, 1)) = 0.1
		_Texture7 ("7 Texture", 2D) = "white" {}
		
		_EmissionMap7("7 Emission Map", 2D) = "black" {}
		_EmissionMapTint7 ("7 Emission Map Tint", Color) = (1,1,1,1)
		_EmissionMapIntensity7 ("7 Emission Map Intensity", Range (0.0, 10.0)) = 1.0
		
		_7Paint1Specular ("7 Paint 1 Specular", Range (0.0, 1.0)) = 0.0
		_7Paint1Glossiness ("7 Paint 1 Glossiness", Range (0.0, 1.0)) = 0.0
		_7Paint1Color ("7 Paint 1 Color", Color) = (0.5,0.5,0.5,1)
		_7Paint1NoiseMix ("7 Paint 1 Noise Mix", Vector) = (0,0,0,0)
		_7Paint1MaskTex ("7 Paint 1 Mask", 2D) = "white" {}
		
		_7Paint2Specular ("7 Paint 2 Specular", Range (0.0, 1.0)) = 0.0
		_7Paint2Glossiness ("7 Paint 2 Glossiness", Range (0.0, 1.0)) = 0.0
		_7Paint2Color ("7 Paint 2 Color", Color) = (0.5,0.5,0.5,1)
		_7Paint2NoiseMix ("7 Paint 2 Noise Mix", Vector) = (0,0,0,0)
		_7Paint2MaskTex ("7 Paint 2 Mask", 2D) = "white" {}
		
		_7MaterialRotation ("7 Material Rotation", Range (-360.0, 360.0)) = 0.0
		
		
		//dirt
		_Dirt1Tint ("Dirt 1 Tint", Color) = (1, 1, 1, 1)
		_DirtNoise1Mix ("Dirt 1 Noise Mix", Vector) = (1,0.5,0,0)
		_Dirt1Amount ("Dirt 1 Amount", Range (0.0, 20.0)) = 4.0
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
   				
   				half2 texcoord_EmissionMap0 : TEXCOORD1;
  			};
   			
   			float _0MaterialRotation;
   			
   			float4 _EmissionMap0_ST;

  			v2f vert (appdata_img v){
   				v2f o;
  				o.pos = UnityObjectToClipPos (v.vertex);
   				o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
   	
   				float rad = radians(_0MaterialRotation);
				float s = sin (rad);
				float c = cos (rad);
				float2x2 rotationMatrix = float2x2( c, -s, s, c);
				
   				o.texcoord_EmissionMap0 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _EmissionMap0) + 0.5;
   				
   				return o; 
  			}
    
  			sampler2D _Texture0;
  			sampler2D _BumpMap0;
  			sampler2D _OcclusionMap0;	
			fixed4 _Tint0;
			
			sampler2D _EmissionMap0;
			fixed4 _EmissionMapTint0;
			float _EmissionMapIntensity0;
		
			fixed4 _SpecularTint0;
			half _SpecularIntensity0;
			float _0SpecularContrast;
			float _0SpecularBrightness;

			half _Glossiness0;	
			float _0GlossinessContrast;			
			float _0GlossinessBrightness;
				
			float _NormalsStrength0;
			float _BumpMapStrength0;
			float _OcclusionMapStrength0;
		
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
		
		
			float4 _NormalMap_ST;
			float4 _Texture0_ST;
			float4 _BumpMap0_ST;	
			float4 _0Paint1MaskTex_ST;
			float4 _0Paint2MaskTex_ST;
			
			half _0GlobalTransparency;
			half _0AlbedoTransparency;
			half _0Paint1Transparency;
			half _0Paint2Transparency;
   
 
  			fixed4 frag (v2f i) : COLOR{
				
				fixed4 objectMasks = tex2D(_ObjectMasks, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw);
				
  				//Emission
				fixed4 emissionMask = tex2D(_EmissionMask, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw);
				fixed4 emissionMap = tex2D(_EmissionMap0, i.texcoord_EmissionMap0 / _GlobalScale ) * objectMasks.r;
				fixed4 finalEmission = GetEmission(emissionMask, _0EmissionTint, _1EmissionTint, _0EmissionIntensity, _1EmissionIntensity, emissionMap, _EmissionMapTint0, _EmissionMapIntensity0);

				fixed4 resultColor = applyHSBEffect(finalEmission, float4(_Hue, _Brightness, _Contrast, _Saturation));

   				return fixed4(resultColor.r, resultColor.g, resultColor.b, 1);
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
   				
   				half2 texcoord_EmissionMap1 : TEXCOORD1;
  			};
   			
   			float _1MaterialRotation;
   			
   			float4 _EmissionMap1_ST;

  			v2f vert (appdata_img v){
   				v2f o;
  				o.pos = UnityObjectToClipPos (v.vertex);
   				o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
   	
   				float rad = radians(_1MaterialRotation);
				float s = sin (rad);
				float c = cos (rad);
				float2x2 rotationMatrix = float2x2( c, -s, s, c);
				
   				o.texcoord_EmissionMap1 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _EmissionMap1) + 0.5;
   				
   				return o; 
  			}
    
  			sampler2D _Texture1;
  			sampler2D _BumpMap1;
  			sampler2D _OcclusionMap1;	
			fixed4 _Tint1;
			
			sampler2D _EmissionMap1;
			fixed4 _EmissionMapTint1;
			float _EmissionMapIntensity1;
		
			fixed4 _SpecularTint1;
			half _SpecularIntensity1;
			float _1SpecularContrast;
			float _1SpecularBrightness;

			half _Glossiness1;	
			float _1GlossinessContrast;			
			float _1GlossinessBrightness;
				
			float _NormalsStrength1;
			float _BumpMapStrength1;
			float _OcclusionMapStrength1;
		
			half _1WornEdgesContrast;
			half _1WornEdgesAmount;
			half _1WornEdgesBorder;
			half _1WornEdgesOpacity;
			fixed4 _1WornEdgesBorderTint;
			fixed4 _1WornEdgesNoiseMix;
			fixed4 _1UnderlyingDiffuseTint;
			fixed4 _1UnderlyingSpecularTint;
			half _1UnderlyingDiffuse;
			half _1UnderlyingSpecular;
			half _1UnderlyingGlossiness;
		
			half _1Paint1Intensity;
			half _1Paint2Intensity;
			
			half _1Paint1Specular;
			half _1Paint1Glossiness;
			fixed4 _1Paint1Color;
			sampler2D _1Paint1MaskTex;
			fixed4 _1Paint1NoiseMix;
				
			half _1Paint2Specular;
			half _1Paint2Glossiness;
			fixed4 _1Paint2Color;
			sampler2D _1Paint2MaskTex;
			fixed4 _1Paint2NoiseMix;
		
		
			float4 _NormalMap_ST;
			float4 _Texture1_ST;
			float4 _BumpMap1_ST;	
			float4 _1Paint1MaskTex_ST;
			float4 _1Paint2MaskTex_ST;
			
			half _1GlobalTransparency;
			half _1AlbedoTransparency;
			half _1Paint1Transparency;
			half _1Paint2Transparency;
   
 
  			fixed4 frag (v2f i) : COLOR{

				fixed4 objectMasks = tex2D(_ObjectMasks, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw);
				
  				//Emission
				fixed4 emissionMask = tex2D(_EmissionMask, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw);
				fixed4 emissionMap = tex2D(_EmissionMap1, i.texcoord_EmissionMap1 / _GlobalScale ) * objectMasks.g;
				fixed4 finalEmission = GetEmission(emissionMask, _0EmissionTint, _1EmissionTint, _0EmissionIntensity, _1EmissionIntensity, emissionMap, _EmissionMapTint1, _EmissionMapIntensity1);

				fixed4 resultColor = applyHSBEffect(finalEmission, float4(_Hue, _Brightness, _Contrast, _Saturation));

   				return fixed4(resultColor.r, resultColor.g, resultColor.b, 1) * objectMasks.g;
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
   				
   				half2 texcoord_EmissionMap2 : TEXCOORD1;
  			};
   			
   			float _2MaterialRotation;
   			
   			float4 _EmissionMap2_ST;

  			v2f vert (appdata_img v){
   				v2f o;
  				o.pos = UnityObjectToClipPos (v.vertex);
   				o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
   	
   				float rad = radians(_2MaterialRotation);
				float s = sin (rad);
				float c = cos (rad);
				float2x2 rotationMatrix = float2x2( c, -s, s, c);
				
   				o.texcoord_EmissionMap2 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _EmissionMap2) + 0.5;
   				
   				return o; 
  			}
    
  			sampler2D _Texture2;
  			sampler2D _BumpMap2;	
  			sampler2D _OcclusionMap2;
			fixed4 _Tint2;
			
			sampler2D _EmissionMap2;
			fixed4 _EmissionMapTint2;
			float _EmissionMapIntensity2;
		
			fixed4 _SpecularTint2;
			half _SpecularIntensity2;
			float _2SpecularContrast;
			float _2SpecularBrightness;

			half _Glossiness2;	
			float _2GlossinessContrast;			
			float _2GlossinessBrightness;
				
			float _NormalsStrength2;
			float _BumpMapStrength2;
			float _OcclusionMapStrength2;
		
			half _2WornEdgesContrast;
			half _2WornEdgesAmount;
			half _2WornEdgesBorder;
			half _2WornEdgesOpacity;
			fixed4 _2WornEdgesBorderTint;
			fixed4 _2WornEdgesNoiseMix;
			fixed4 _2UnderlyingDiffuseTint;
			fixed4 _2UnderlyingSpecularTint;
			half _2UnderlyingDiffuse;
			half _2UnderlyingSpecular;
			half _2UnderlyingGlossiness;
		
			half _2Paint1Intensity;
			half _2Paint2Intensity;
		
			half _2Paint1Specular;
			half _2Paint1Glossiness;
			fixed4 _2Paint1Color;
			sampler2D _2Paint1MaskTex;
			fixed4 _2Paint1NoiseMix;
				
			half _2Paint2Specular;
			half _2Paint2Glossiness;
			fixed4 _2Paint2Color;
			sampler2D _2Paint2MaskTex;
			fixed4 _2Paint2NoiseMix;
		
			float4 _NormalMap_ST;
			float4 _Texture2_ST;
			float4 _BumpMap2_ST;	
			float4 _2Paint1MaskTex_ST;
			float4 _2Paint2MaskTex_ST;
			
			half _2GlobalTransparency;
			half _2AlbedoTransparency;
			half _2Paint1Transparency;
			half _2Paint2Transparency;
   
 
  			fixed4 frag (v2f i) : COLOR{

				fixed4 objectMasks = tex2D(_ObjectMasks, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw);
				
  				//Emission
				fixed4 emissionMask = tex2D(_EmissionMask, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw);
				fixed4 emissionMap = tex2D(_EmissionMap2, i.texcoord_EmissionMap2 / _GlobalScale ) * objectMasks.b;
				fixed4 finalEmission = GetEmission(emissionMask, _0EmissionTint, _1EmissionTint, _0EmissionIntensity, _1EmissionIntensity, emissionMap, _EmissionMapTint2, _EmissionMapIntensity2);

				fixed4 resultColor = applyHSBEffect(finalEmission, float4(_Hue, _Brightness, _Contrast, _Saturation));

   				return fixed4(resultColor.r, resultColor.g, resultColor.b, 1) * objectMasks.b;
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
   				
   				half2 texcoord_EmissionMap3 : TEXCOORD1;
  			};
   			
   			float _3MaterialRotation;
   			
   			float4 _EmissionMap3_ST;

  			v2f vert (appdata_img v){
   				v2f o;
  				o.pos = UnityObjectToClipPos (v.vertex);
   				o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
   	
   				float rad = radians(_3MaterialRotation);
				float s = sin (rad);
				float c = cos (rad);
				float2x2 rotationMatrix = float2x2( c, -s, s, c);
				
   				o.texcoord_EmissionMap3 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _EmissionMap3) + 0.5;
   				
   				return o; 
  			}
    
  			sampler2D _Texture3;
  			sampler2D _BumpMap3;
  			sampler2D _OcclusionMap3;	
			fixed4 _Tint3;
			
			sampler2D _EmissionMap3;
			fixed4 _EmissionMapTint3;
			float _EmissionMapIntensity3;
		
			fixed4 _SpecularTint3;
			half _SpecularIntensity3;
			float _3SpecularContrast;
			float _3SpecularBrightness;

			half _Glossiness3;	
			float _3GlossinessContrast;			
			float _3GlossinessBrightness;
				
			float _NormalsStrength3;
			float _BumpMapStrength3;
			float _OcclusionMapStrength3;
		
			half _3WornEdgesContrast;
			half _3WornEdgesAmount;
			half _3WornEdgesBorder;
			half _3WornEdgesOpacity;
			fixed4 _3WornEdgesBorderTint;
			fixed4 _3WornEdgesNoiseMix;
			fixed4 _3UnderlyingDiffuseTint;
			fixed4 _3UnderlyingSpecularTint;
			half _3UnderlyingDiffuse;
			half _3UnderlyingSpecular;
			half _3UnderlyingGlossiness;
		
			half _3Paint1Intensity;
			half _3Paint2Intensity;
			
			half _3Paint1Specular;
			half _3Paint1Glossiness;
			fixed4 _3Paint1Color;
			sampler2D _3Paint1MaskTex;
			fixed4 _3Paint1NoiseMix;
				
			half _3Paint2Specular;
			half _3Paint2Glossiness;
			fixed4 _3Paint2Color;
			sampler2D _3Paint2MaskTex;
			fixed4 _3Paint2NoiseMix;
		
			float4 _NormalMap_ST;
			float4 _Texture3_ST;
			float4 _BumpMap3_ST;	
			float4 _3Paint1MaskTex_ST;
			float4 _3Paint2MaskTex_ST;
	
   			half _3GlobalTransparency;
			half _3AlbedoTransparency;
			half _3Paint1Transparency;
			half _3Paint2Transparency;
			
 
  			fixed4 frag (v2f i) : COLOR{

				fixed4 objectMasks = tex2D(_ObjectMasks, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw);
				
  				//Emission
				fixed4 emissionMask = tex2D(_EmissionMask, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw);
				fixed4 emissionMap = tex2D(_EmissionMap3, i.texcoord_EmissionMap3 / _GlobalScale ) * objectMasks.a;
				fixed4 finalEmission = GetEmission(emissionMask, _0EmissionTint, _1EmissionTint, _0EmissionIntensity, _1EmissionIntensity, emissionMap, _EmissionMapTint3, _EmissionMapIntensity3);

				fixed4 resultColor = applyHSBEffect(finalEmission, float4(_Hue, _Brightness, _Contrast, _Saturation));

   				return fixed4(resultColor.r, resultColor.g, resultColor.b, 1) * objectMasks.a;
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
   				
   				half2 texcoord_EmissionMap4 : TEXCOORD1;
  			};
   			
   			float _4MaterialRotation;
   			
   			float4 _EmissionMap4_ST;

  			v2f vert (appdata_img v){
   				v2f o;
  				o.pos = UnityObjectToClipPos (v.vertex);
   				o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
   	
   				float rad = radians(_4MaterialRotation);
				float s = sin (rad);
				float c = cos (rad);
				float2x2 rotationMatrix = float2x2( c, -s, s, c);
				
   				o.texcoord_EmissionMap4 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _EmissionMap4) + 0.5;
   				
   				return o; 
  			}
    
  			sampler2D _Texture4;
  			sampler2D _BumpMap4;
  			sampler2D _OcclusionMap4;	
			fixed4 _Tint4;
			
			sampler2D _EmissionMap4;
			fixed4 _EmissionMapTint4;
			float _EmissionMapIntensity4;
		
			fixed4 _SpecularTint4;
			half _SpecularIntensity4;
			float _4SpecularContrast;
			float _4SpecularBrightness;

			half _Glossiness4;	
			float _4GlossinessContrast;			
			float _4GlossinessBrightness;
				
			float _NormalsStrength4;
			float _BumpMapStrength4;
			float _OcclusionMapStrength4;
		
			half _4WornEdgesContrast;
			half _4WornEdgesAmount;
			half _4WornEdgesBorder;
			half _4WornEdgesOpacity;
			fixed4 _4WornEdgesBorderTint;
			fixed4 _4WornEdgesNoiseMix;
			fixed4 _4UnderlyingDiffuseTint;
			fixed4 _4UnderlyingSpecularTint;
			half _4UnderlyingDiffuse;
			half _4UnderlyingSpecular;
			half _4UnderlyingGlossiness;
		
			half _4Paint1Intensity;
			half _4Paint2Intensity;
			
			half _4Paint1Specular;
			half _4Paint1Glossiness;
			fixed4 _4Paint1Color;
			sampler2D _4Paint1MaskTex;
			fixed4 _4Paint1NoiseMix;
				
			half _4Paint2Specular;
			half _4Paint2Glossiness;
			fixed4 _4Paint2Color;
			sampler2D _4Paint2MaskTex;
			fixed4 _4Paint2NoiseMix;
		
			float4 _NormalMap_ST;
			float4 _Texture4_ST;
			float4 _BumpMap4_ST;	
			float4 _4Paint1MaskTex_ST;
			float4 _4Paint2MaskTex_ST;
	
   			half _4GlobalTransparency;
			half _4AlbedoTransparency;
			half _4Paint1Transparency;
			half _4Paint2Transparency;
   			
 
  			fixed4 frag (v2f i) : COLOR{

				fixed4 objectMasks = tex2D(_ObjectMasks2, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw);
				
  				//Emission
				fixed4 emissionMask = tex2D(_EmissionMask, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw);
				fixed4 emissionMap = tex2D(_EmissionMap4, i.texcoord_EmissionMap4 / _GlobalScale ) * objectMasks.r;
				fixed4 finalEmission = GetEmission(emissionMask, _0EmissionTint, _1EmissionTint, _0EmissionIntensity, _1EmissionIntensity, emissionMap, _EmissionMapTint4, _EmissionMapIntensity4);

				fixed4 resultColor = applyHSBEffect(finalEmission, float4(_Hue, _Brightness, _Contrast, _Saturation));

   				return fixed4(resultColor.r, resultColor.g, resultColor.b, 1) * objectMasks.r;
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
   				
   				half2 texcoord_EmissionMap5 : TEXCOORD1;
  			};
   			
   			float _5MaterialRotation;
   			
   			float4 _EmissionMap5_ST;

  			v2f vert (appdata_img v){
   				v2f o;
  				o.pos = UnityObjectToClipPos (v.vertex);
   				o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
   	
   				float rad = radians(_5MaterialRotation);
				float s = sin (rad);
				float c = cos (rad);
				float2x2 rotationMatrix = float2x2( c, -s, s, c);
				
   				o.texcoord_EmissionMap5 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _EmissionMap5) + 0.5;
   				
   				return o; 
  			}
    
  			sampler2D _Texture5;
  			sampler2D _BumpMap5;
  			sampler2D _OcclusionMap5;	
			fixed4 _Tint5;
			
			sampler2D _EmissionMap5;
			fixed4 _EmissionMapTint5;
			float _EmissionMapIntensity5;
		
			fixed4 _SpecularTint5;
			half _SpecularIntensity5;
			float _5SpecularContrast;
			float _5SpecularBrightness;

			half _Glossiness5;	
			float _5GlossinessContrast;			
			float _5GlossinessBrightness;
				
			float _NormalsStrength5;
			float _BumpMapStrength5;
			float _OcclusionMapStrength5;
		
			half _5WornEdgesContrast;
			half _5WornEdgesAmount;
			half _5WornEdgesBorder;
			half _5WornEdgesOpacity;
			fixed4 _5WornEdgesBorderTint;
			fixed4 _5WornEdgesNoiseMix;
			fixed4 _5UnderlyingDiffuseTint;
			fixed4 _5UnderlyingSpecularTint;
			half _5UnderlyingDiffuse;
			half _5UnderlyingSpecular;
			half _5UnderlyingGlossiness;
		
			half _5Paint1Intensity;
			half _5Paint2Intensity;
			
			half _5Paint1Specular;
			half _5Paint1Glossiness;
			fixed4 _5Paint1Color;
			sampler2D _5Paint1MaskTex;
			fixed4 _5Paint1NoiseMix;
				
			half _5Paint2Specular;
			half _5Paint2Glossiness;
			fixed4 _5Paint2Color;
			sampler2D _5Paint2MaskTex;
			fixed4 _5Paint2NoiseMix;
			
		
			float4 _NormalMap_ST;
			float4 _Texture5_ST;
			float4 _BumpMap5_ST;	
			float4 _5Paint1MaskTex_ST;
			float4 _5Paint2MaskTex_ST;
			
			half _5GlobalTransparency;
			half _5AlbedoTransparency;
			half _5Paint1Transparency;
			half _5Paint2Transparency;
   
 
  			fixed4 frag (v2f i) : COLOR{

				fixed4 objectMasks = tex2D(_ObjectMasks2, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw);
				
  				//Emission
				fixed4 emissionMask = tex2D(_EmissionMask, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw);
				fixed4 emissionMap = tex2D(_EmissionMap5, i.texcoord_EmissionMap5 / _GlobalScale ) * objectMasks.g;
				fixed4 finalEmission = GetEmission(emissionMask, _0EmissionTint, _1EmissionTint, _0EmissionIntensity, _1EmissionIntensity, emissionMap, _EmissionMapTint5, _EmissionMapIntensity5);

				fixed4 resultColor = applyHSBEffect(finalEmission, float4(_Hue, _Brightness, _Contrast, _Saturation));

   				return fixed4(resultColor.r, resultColor.g, resultColor.b, 1) * objectMasks.g;
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
   				
   				half2 texcoord_EmissionMap6 : TEXCOORD1;
  			};
   			
   			float _6MaterialRotation;
   			
   			float4 _EmissionMap6_ST;

  			v2f vert (appdata_img v){
   				v2f o;
  				o.pos = UnityObjectToClipPos (v.vertex);
   				o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
   	
   				float rad = radians(_6MaterialRotation);
				float s = sin (rad);
				float c = cos (rad);
				float2x2 rotationMatrix = float2x2( c, -s, s, c);
				
   				o.texcoord_EmissionMap6 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _EmissionMap6) + 0.5;
   				
   				return o; 
  			}
    
  			sampler2D _Texture6;
  			sampler2D _BumpMap6;
  			sampler2D _OcclusionMap6;	
			fixed4 _Tint6;
			
			sampler2D _EmissionMap6;
			fixed4 _EmissionMapTint6;
			float _EmissionMapIntensity6;
		
			fixed4 _SpecularTint6;
			half _SpecularIntensity6;
			float _6SpecularContrast;
			float _6SpecularBrightness;

			half _Glossiness6;	
			float _6GlossinessContrast;			
			float _6GlossinessBrightness;
				
			float _NormalsStrength6;
			float _BumpMapStrength6;
			float _OcclusionMapStrength6;
		
			half _6WornEdgesContrast;
			half _6WornEdgesAmount;
			half _6WornEdgesBorder;
			half _6WornEdgesOpacity;
			fixed4 _6WornEdgesBorderTint;
			fixed4 _6WornEdgesNoiseMix;
			fixed4 _6UnderlyingDiffuseTint;
			fixed4 _6UnderlyingSpecularTint;
			half _6UnderlyingDiffuse;
			half _6UnderlyingSpecular;
			half _6UnderlyingGlossiness;
		
			half _6Paint1Intensity;
			half _6Paint2Intensity;
			
			half _6Paint1Specular;
			half _6Paint1Glossiness;
			fixed4 _6Paint1Color;
			sampler2D _6Paint1MaskTex;
			fixed4 _6Paint1NoiseMix;
				
			half _6Paint2Specular;
			half _6Paint2Glossiness;
			fixed4 _6Paint2Color;
			sampler2D _6Paint2MaskTex;
			fixed4 _6Paint2NoiseMix;
		
		
			float4 _NormalMap_ST;
			float4 _Texture6_ST;
			float4 _BumpMap6_ST;	
			float4 _6Paint1MaskTex_ST;
			float4 _6Paint2MaskTex_ST;
			
			half _6GlobalTransparency;
			half _6AlbedoTransparency;
			half _6Paint1Transparency;
			half _6Paint2Transparency;
   
 
  			fixed4 frag (v2f i) : COLOR{

				fixed4 objectMasks = tex2D(_ObjectMasks2, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw);
				
  				//Emission
				fixed4 emissionMask = tex2D(_EmissionMask, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw);
				fixed4 emissionMap = tex2D(_EmissionMap6, i.texcoord_EmissionMap6 / _GlobalScale ) * objectMasks.b;
				fixed4 finalEmission = GetEmission(emissionMask, _0EmissionTint, _1EmissionTint, _0EmissionIntensity, _1EmissionIntensity, emissionMap, _EmissionMapTint6, _EmissionMapIntensity6);

				fixed4 resultColor = applyHSBEffect(finalEmission, float4(_Hue, _Brightness, _Contrast, _Saturation));

   				return fixed4(resultColor.r, resultColor.g, resultColor.b, 1) * objectMasks.b;
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
   				
   				half2 texcoord_EmissionMap7 : TEXCOORD1;
  			};
   			
   			float _7MaterialRotation;
   			
   			float4 _EmissionMap7_ST;

  			v2f vert (appdata_img v){
   				v2f o;
  				o.pos = UnityObjectToClipPos (v.vertex);
   				o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
   	
   				float rad = radians(_7MaterialRotation);
				float s = sin (rad);
				float c = cos (rad);
				float2x2 rotationMatrix = float2x2( c, -s, s, c);
				
   				o.texcoord_EmissionMap7 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _EmissionMap7) + 0.5;
   				
   				return o; 
  			}
    
  			sampler2D _Texture7;
  			sampler2D _BumpMap7;
  			sampler2D _OcclusionMap7;	
			fixed4 _Tint7;
			
			sampler2D _EmissionMap7;
			fixed4 _EmissionMapTint7;
			float _EmissionMapIntensity7;
		
			fixed4 _SpecularTint7;
			half _SpecularIntensity7;
			float _7SpecularContrast;
			float _7SpecularBrightness;

			half _Glossiness7;	
			float _7GlossinessContrast;			
			float _7GlossinessBrightness;
				
			float _NormalsStrength7;
			float _BumpMapStrength7;
			float _OcclusionMapStrength7;
		
			half _7WornEdgesContrast;
			half _7WornEdgesAmount;
			half _7WornEdgesBorder;
			half _7WornEdgesOpacity;
			fixed4 _7WornEdgesBorderTint;
			fixed4 _7WornEdgesNoiseMix;
			fixed4 _7UnderlyingDiffuseTint;
			fixed4 _7UnderlyingSpecularTint;
			half _7UnderlyingDiffuse;
			half _7UnderlyingSpecular;
			half _7UnderlyingGlossiness;
		
			half _7Paint1Intensity;
			half _7Paint2Intensity;
			
			half _7Paint1Specular;
			half _7Paint1Glossiness;
			fixed4 _7Paint1Color;
			sampler2D _7Paint1MaskTex;
			fixed4 _7Paint1NoiseMix;
				
			half _7Paint2Specular;
			half _7Paint2Glossiness;
			fixed4 _7Paint2Color;
			sampler2D _7Paint2MaskTex;
			fixed4 _7Paint2NoiseMix;
		
		
			float4 _NormalMap_ST;
			float4 _Texture7_ST;
			float4 _BumpMap7_ST;
			float4 _7Paint1MaskTex_ST;
			float4 _7Paint2MaskTex_ST;
			
			half _7GlobalTransparency;
			half _7AlbedoTransparency;
			half _7Paint1Transparency;
			half _7Paint2Transparency;
   
 
  			fixed4 frag (v2f i) : COLOR{

				fixed4 objectMasks = tex2D(_ObjectMasks2, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw);
				
  				//Emission
				fixed4 emissionMask = tex2D(_EmissionMask, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw);
				fixed4 emissionMap = tex2D(_EmissionMap7, i.texcoord_EmissionMap7 / _GlobalScale ) * objectMasks.a;
				fixed4 finalEmission = GetEmission(emissionMask, _0EmissionTint, _1EmissionTint, _0EmissionIntensity, _1EmissionIntensity, emissionMap, _EmissionMapTint7, _EmissionMapIntensity7);

				fixed4 resultColor = applyHSBEffect(finalEmission, float4(_Hue, _Brightness, _Contrast, _Saturation));

   				return fixed4(resultColor.r, resultColor.g, resultColor.b, 1) * objectMasks.a;
  			}
  			ENDCG
 		}
	}   
	   
 
 	FallBack "Diffuse"
}