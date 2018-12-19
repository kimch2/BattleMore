// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/ExportSpecular" {
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
		_GlossinessIntensity0 ("0 Glossiness Intensity", Range (0.0, 1.0)) = 0.2
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
		_BumpMap0("0 Normal Map", 2D) = "bump" {}
		_BumpMapStrength0 ("0 Normal Map Strength", Range (-2.0, 2)) = 1.0
		
		_OcclusionMap0("0 Occlusion Map", 2D) = "white" {}
		_OcclusionMapStrength0 ("0 Occlusion Map Strength", Range (0.0, 2)) = 1.0
		
		
		_SpecularMap0("0 Specular Map", 2D) = "black" {}
		_UseSpecularMap0 ("Use Specular Map 0", Range (0.0, 1)) = 0.0
		_GlossinessFromAlpha0 ("Glossiness From Alpha 0", Range (0.0, 1)) = 1.0
		
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
		
		
		_Tint1 ("1 Tint", Color) = (1,1,1,1)
		_SpecularTint1 ("1 Specular Tint", Color) = (1,1,1,1)
		
		_SpecularIntensity1 ("1 Specular Intensity", Range (0.0, 1.0)) = 0.2
		_1SpecularContrast ("1 Specular Contrast", Range (0, 1.0)) = 0.1
		_1SpecularBrightness ("1 Specular Brightness", Range (0, 1.0)) = 0.5
		
		_Glossiness1 ("1 Glossiness", Range (0.0, 1.0)) = 0.4
		_GlossinessIntensity1 ("1 Glossiness Intensity", Range (0.0, 1.0)) = 0.2
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
		_BumpMap1("1 Normal Map", 2D) = "bump" {}
		_BumpMapStrength1 ("1 Normal Map Strength", Range (-2.0, 2)) = 1.0
		
		_OcclusionMap1("1 Occlusion Map", 2D) = "white" {}
		_OcclusionMapStrength1 ("1 Occlusion Map Strength", Range (0.0, 2)) = 1.0
		
		
		_SpecularMap1("1 Specular Map", 2D) = "black" {}
		_UseSpecularMap1 ("Use Specular Map 1", Range (0.0, 1)) = 0.0
		_GlossinessFromAlpha1 ("Glossiness From Alpha 1", Range (0.0, 1)) = 1.0
		
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
		
		_1GlobalTransparency ("0 Global Transparency", Range (0.0, 1.0)) = 0.0
		_1AlbedoTransparency ("0 Albedo Transparency", Range (0.0, 1.0)) = 0.0
		_1Paint1Transparency ("0 Paint 1 Transparency", Range (0.0, 1.0)) = 0.0
		_1Paint2Transparency ("0 Paint 2 Transparency", Range (0.0, 1.0)) = 0.0
		
		_1MaterialRotation ("1 Material Rotation", Range (-360.0, 360.0)) = 0.0
		
		
		_Tint2 ("2 Tint", Color) = (1,1,1,1)
		_SpecularTint2 ("2 Specular Tint", Color) = (1,1,1,1)
		
		_SpecularIntensity2 ("2 Specular Intensity", Range (0.0, 1.0)) = 0.2
		_2SpecularContrast ("2 Specular Contrast", Range (0, 1.0)) = 0.1
		_2SpecularBrightness ("2 Specular Brightness", Range (0, 1.0)) = 0.5
		
		_Glossiness2 ("2 Glossiness", Range (0.0, 1.0)) = 0.4
		_GlossinessIntensity2 ("2 Glossiness Intensity", Range (0.0, 1.0)) = 0.2
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
		_BumpMap2("2 Normal Map", 2D) = "bump" {}
		_BumpMapStrength2 ("2 Normal Map Strength", Range (-2.0, 2)) = 1.0
		
		_OcclusionMap2("2 Occlusion Map", 2D) = "white" {}
		_OcclusionMapStrength2 ("2 Occlusion Map Strength", Range (0.0, 2)) = 1.0
		
		
		_SpecularMap2("2 Specular Map", 2D) = "black" {}
		_UseSpecularMap2 ("Use Specular Map 2", Range (0.0, 1)) = 0.0
		_GlossinessFromAlpha2 ("Glossiness From Alpha 2", Range (0.0, 1)) = 1.0
		
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
		
		_2GlobalTransparency ("0 Global Transparency", Range (0.0, 1.0)) = 0.0
		_2AlbedoTransparency ("0 Albedo Transparency", Range (0.0, 1.0)) = 0.0
		_2Paint1Transparency ("0 Paint 1 Transparency", Range (0.0, 1.0)) = 0.0
		_2Paint2Transparency ("0 Paint 2 Transparency", Range (0.0, 1.0)) = 0.0
		
		_2MaterialRotation ("2 Material Rotation", Range (-360.0, 360.0)) = 0.0
		
		
		_Tint3 ("3 Tint", Color) = (1,1,1,1)
		_SpecularTint3 ("3 Specular Tint", Color) = (1,1,1,1)
		
		_SpecularIntensity3 ("3 Specular Intensity", Range (0.0, 1.0)) = 0.2
		_3SpecularContrast ("3 Specular Contrast", Range (0, 1.0)) = 0.1
		_3SpecularBrightness ("3 Specular Brightness", Range (0, 1.0)) = 0.5
		
		_Glossiness3 ("3 Glossiness", Range (0.0, 1.0)) = 0.4
		_GlossinessIntensity3 ("3 Glossiness Intensity", Range (0.0, 1.0)) = 0.2
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
		_BumpMap3("3 Normal Map", 2D) = "bump" {}
		_BumpMapStrength3 ("3 Normal Map Strength", Range (-2.0, 2)) = 1.0
		
		_OcclusionMap3("3 Occlusion Map", 2D) = "white" {}
		_OcclusionMapStrength3 ("3 Occlusion Map Strength", Range (0.0, 2)) = 1.0
		
		
		_SpecularMap3("3 Specular Map", 2D) = "black" {}
		_UseSpecularMap3 ("Use Specular Map 3", Range (0.0, 1)) = 0.0
		_GlossinessFromAlpha3 ("Glossiness From Alpha 3", Range (0.0, 1)) = 1.0
		
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
		
		_3GlobalTransparency ("0 Global Transparency", Range (0.0, 1.0)) = 0.0
		_3AlbedoTransparency ("0 Albedo Transparency", Range (0.0, 1.0)) = 0.0
		_3Paint1Transparency ("0 Paint 1 Transparency", Range (0.0, 1.0)) = 0.0
		_3Paint2Transparency ("0 Paint 2 Transparency", Range (0.0, 1.0)) = 0.0
		
		_3MaterialRotation ("3 Material Rotation", Range (-360.0, 360.0)) = 0.0
		
		
		_Tint4 ("4 Tint", Color) = (1,1,1,1)
		_SpecularTint4 ("4 Specular Tint", Color) = (1,1,1,1)
		
		_SpecularIntensity4 ("4 Specular Intensity", Range (0.0, 1.0)) = 0.2
		_4SpecularContrast ("4 Specular Contrast", Range (0, 1.0)) = 0.1
		_4SpecularBrightness ("4 Specular Brightness", Range (0, 1.0)) = 0.5
		
		_Glossiness4 ("4 Glossiness", Range (0.0, 1.0)) = 0.4
		_GlossinessIntensity4 ("4 Glossiness Intensity", Range (0.0, 1.0)) = 0.2
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
		_BumpMap4("4 Normal Map", 2D) = "bump" {}
		_BumpMapStrength4 ("4 Normal Map Strength", Range (-2.0, 2)) = 1.0
		
		_OcclusionMap4("4 Occlusion Map", 2D) = "white" {}
		_OcclusionMapStrength4 ("4 Occlusion Map Strength", Range (0.0, 2)) = 1.0
		
		
		_SpecularMap4("4 Specular Map", 2D) = "black" {}
		_UseSpecularMap4 ("Use Specular Map 4", Range (0.0, 1)) = 0.0
		_GlossinessFromAlpha4 ("Glossiness From Alpha 4", Range (0.0, 1)) = 1.0
		
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
		
		_4GlobalTransparency ("0 Global Transparency", Range (0.0, 1.0)) = 0.0
		_4AlbedoTransparency ("0 Albedo Transparency", Range (0.0, 1.0)) = 0.0
		_4Paint1Transparency ("0 Paint 1 Transparency", Range (0.0, 1.0)) = 0.0
		_4Paint2Transparency ("0 Paint 2 Transparency", Range (0.0, 1.0)) = 0.0
		
		_4MaterialRotation ("4 Material Rotation", Range (-360.0, 360.0)) = 0.0
		
		
		_Tint5 ("5 Tint", Color) = (1,1,1,1)
		_SpecularTint5 ("5 Specular Tint", Color) = (1,1,1,1)
		
		_SpecularIntensity5 ("5 Specular Intensity", Range (0.0, 1.0)) = 0.2
		_5SpecularContrast ("5 Specular Contrast", Range (0, 1.0)) = 0.1
		_5SpecularBrightness ("5 Specular Brightness", Range (0, 1.0)) = 0.5
		
		_Glossiness5 ("5 Glossiness", Range (0.0, 1.0)) = 0.4
		_GlossinessIntensity5 ("5 Glossiness Intensity", Range (0.0, 1.0)) = 0.2
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
		_BumpMap5("5 Normal Map", 2D) = "bump" {}
		_BumpMapStrength5 ("5 Normal Map Strength", Range (-2.0, 2)) = 1.0
		
		_OcclusionMap5("5 Occlusion Map", 2D) = "white" {}
		_OcclusionMapStrength5 ("5 Occlusion Map Strength", Range (0.0, 2)) = 1.0
		
		
		_SpecularMap5("5 Specular Map", 2D) = "black" {}
		_UseSpecularMap5 ("Use Specular Map 5", Range (0.0, 1)) = 0.0
		_GlossinessFromAlpha5 ("Glossiness From Alpha 5", Range (0.0, 1)) = 1.0
		
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
		
		_5GlobalTransparency ("0 Global Transparency", Range (0.0, 1.0)) = 0.0
		_5AlbedoTransparency ("0 Albedo Transparency", Range (0.0, 1.0)) = 0.0
		_5Paint1Transparency ("0 Paint 1 Transparency", Range (0.0, 1.0)) = 0.0
		_5Paint2Transparency ("0 Paint 2 Transparency", Range (0.0, 1.0)) = 0.0
		
		_5MaterialRotation ("5 Material Rotation", Range (-360.0, 360.0)) = 0.0
		
		
		_Tint6 ("6 Tint", Color) = (1,1,1,1)
		_SpecularTint6 ("6 Specular Tint", Color) = (1,1,1,1)
		
		_SpecularIntensity6 ("6 Specular Intensity", Range (0.0, 1.0)) = 0.2
		_6SpecularContrast ("6 Specular Contrast", Range (0, 1.0)) = 0.1
		_6SpecularBrightness ("6 Specular Brightness", Range (0, 1.0)) = 0.5
		
		_Glossiness6 ("6 Glossiness", Range (0.0, 1.0)) = 0.4
		_GlossinessIntensity6 ("6 Glossiness Intensity", Range (0.0, 1.0)) = 0.2
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
		_BumpMap6("6 Normal Map", 2D) = "bump" {}
		_BumpMapStrength6 ("6 Normal Map Strength", Range (-2.0, 2)) = 1.0
		
		_OcclusionMap6("6 Occlusion Map", 2D) = "white" {}
		_OcclusionMapStrength6 ("6 Occlusion Map Strength", Range (0.0, 2)) = 1.0
		
		
		_SpecularMap6("6 Specular Map", 2D) = "black" {}
		_UseSpecularMap6 ("Use Specular Map 6", Range (0.0, 1)) = 0.0
		_GlossinessFromAlpha6 ("Glossiness From Alpha 6", Range (0.0, 1)) = 1.0
		
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
		
		_6GlobalTransparency ("0 Global Transparency", Range (0.0, 1.0)) = 0.0
		_6AlbedoTransparency ("0 Albedo Transparency", Range (0.0, 1.0)) = 0.0
		_6Paint1Transparency ("0 Paint 1 Transparency", Range (0.0, 1.0)) = 0.0
		_6Paint2Transparency ("0 Paint 2 Transparency", Range (0.0, 1.0)) = 0.0
		
		_6MaterialRotation ("6 Material Rotation", Range (-360.0, 360.0)) = 0.0
		
		
		_Tint7 ("7 Tint", Color) = (1,1,1,1)
		_SpecularTint7 ("7 Specular Tint", Color) = (1,1,1,1)
		
		_SpecularIntensity7 ("7 Specular Intensity", Range (0.0, 1.0)) = 0.2
		_7SpecularContrast ("7 Specular Contrast", Range (0, 1.0)) = 0.1
		_7SpecularBrightness ("7 Specular Brightness", Range (0, 1.0)) = 0.5
		
		_Glossiness7 ("7 Glossiness", Range (0.0, 1.0)) = 0.4
		_GlossinessIntensity7 ("7 Glossiness Intensity", Range (0.0, 1.0)) = 0.2
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
		_BumpMap7("7 Normal Map", 2D) = "bump" {}
		_BumpMapStrength7 ("7 Normal Map Strength", Range (-2.0, 2)) = 1.0
		
		_OcclusionMap7("7 Occlusion Map", 2D) = "white" {}
		_OcclusionMapStrength7 ("7 Occlusion Map Strength", Range (0.0, 2)) = 1.0
		
		
		_SpecularMap7("7 Specular Map", 2D) = "black" {}
		_UseSpecularMap7 ("Use Specular Map 7", Range (0.0, 1)) = 0.0
		_GlossinessFromAlpha7 ("Glossiness From Alpha 7", Range (0.0, 1)) = 1.0
		
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
		
		_7GlobalTransparency ("0 Global Transparency", Range (0.0, 1.0)) = 0.0
		_7AlbedoTransparency ("0 Albedo Transparency", Range (0.0, 1.0)) = 0.0
		_7Paint1Transparency ("0 Paint 1 Transparency", Range (0.0, 1.0)) = 0.0
		_7Paint2Transparency ("0 Paint 2 Transparency", Range (0.0, 1.0)) = 0.0
		
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
 
	SubShader {
 
		ZTest Always Cull Off ZWrite Off Fog { Mode Off } 
 		
 		//Mask 0
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
   				
   				half2 texcoord_Texture0 : TEXCOORD1;
   				half2 texcoord_BumpMap0 : TEXCOORD2;
   				half2 texcoord_0Paint1MaskTex : TEXCOORD3;
   				half2 texcoord_0Paint2MaskTex : TEXCOORD4; 
   				half2 texroord_Noise : TEXCOORD5; 
  			};
   			
   			float _0MaterialRotation;
   			
			float4 _NormalMap_ST;
			float4 _Texture0_ST;
			float4 _BumpMap0_ST;	
			float4 _0Paint1MaskTex_ST;
			float4 _0Paint2MaskTex_ST;
			float4 _EmissionMask_ST; 

  			v2f vert (appdata_img v){
   				v2f o;
  				o.pos = UnityObjectToClipPos (v.vertex);
   				o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
   	
   				float rad = radians(_0MaterialRotation);
				float s = sin (rad);
				float c = cos (rad);
				float2x2 rotationMatrix = float2x2( c, -s, s, c);
				
   				o.texcoord_Texture0 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _Texture0) + 0.5;
				o.texcoord_BumpMap0 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _BumpMap0) + 0.5;
				o.texcoord_0Paint1MaskTex = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _0Paint1MaskTex) + 0.5;
				o.texcoord_0Paint2MaskTex = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _0Paint2MaskTex) + 0.5;
				o.texroord_Noise = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _EmissionMask) + 0.5;
   				
   				return o; 
  			}
    
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
			sampler2D _SpecularMap0;
			float _UseSpecularMap0;
			float _GlossinessFromAlpha0;
		
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
 
  			fixed4 frag (v2f i) : COLOR{
  			
				fixed4 objectLabel = tex2D(_LabelTexture, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw) * _LabelTint; 
				fixed4 objectLabelAlpha = tex2D(_LabelTexture, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).a;
				

				fixed ao = GetResultAoMap(tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).r);
          		fixed4 objectMasks = tex2D(_ObjectMasks, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw); 
           		fixed4 objectTexture0 = tex2D(_Texture0, i.texcoord_Texture0 / _GlobalScale );
            	objectTexture0.a = 1;
            	fixed edgeMask = tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).g; 
            	fixed dirtMask = tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).b;

     
            	fixed noiseR = tex2D(_EmissionMask, i.texroord_Noise / _GlobalScale).b; 	
            	fixed noiseB = tex2D(_EmissionMask, i.texroord_Noise / _GlobalScale).a;     
            	fixed noiseG1 = tex2D(_EmissionMask, i.texroord_Noise / _GlobalScale + 0.5).b; 	
            	fixed noiseG2 = tex2D(_EmissionMask, 1 - i.texroord_Noise / _GlobalScale + 0.5).a;
            	fixed noiseG = clamp( (1 - noiseG1) * 0.5 + (1 - noiseG2) * 0.5, 0, 1);
            
            	fixed4 rgbaNoise = fixed4(noiseR, noiseG, noiseB, 1);
            	
            
            	//worn edges
            	fixed edgeNoise = GetEdgeNoise(rgbaNoise, _0WornEdgesNoiseMix);
        		fixed wornEdges = GetWornEdges(ao, edgeMask, edgeNoise, _0WornEdgesContrast, _0WornEdgesAmount, _0WornEdgesOpacity); 
            	fixed wornEdgesCenter = GetWornEdgesCenter(ao, edgeMask, edgeNoise, _0WornEdgesContrast, _0WornEdgesAmount, _0WornEdgesOpacity, _0WornEdgesBorder); 

            
      		    //paint
				//1
				fixed4 basePaint1 = tex2D(_0Paint1MaskTex, i.texcoord_0Paint1MaskTex);
				fixed4 basePaint1Mask = tex2D(_0Paint1MaskTex, i.texcoord_0Paint1MaskTex).a;	
			
				fixed maskNoiseBorder1 = GetMaskNoiseBorder(rgbaNoise, _0Paint1NoiseMix);			
				fixed4 pMask1 = GetPMask(basePaint1Mask, objectMasks.r, _0Paint1Intensity, maskNoiseBorder1);							  		   
           		fixed4 paintCol1 = GetPaintCol(pMask1, basePaint1, _0Paint1Color);
				fixed paintMask1 =  pMask1.a;
			
			
				//2
				fixed4 basePaint2 = tex2D(_0Paint2MaskTex, i.texcoord_0Paint2MaskTex);
				fixed4 basePaint2Mask = tex2D(_0Paint2MaskTex, i.texcoord_0Paint2MaskTex).a;	
				
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
            			
				
				//Specular
				fixed4 resultSpecularMap = 0;
			
				if (_UseSpecularMap0 > 0.5) {
					fixed4 objectTexture0specRGB = GetSpecMap(tex2D(_SpecularMap0, i.texcoord_BumpMap0 / _GlobalScale), _0SpecularContrast, _0SpecularBrightness); 
					resultSpecularMap = GetResultSpecularMapRGB(objectTexture0specRGB, _SpecularIntensity0, _SpecularTint0, paintMask1, paintMask2, _0Paint1Specular, _0Paint2Specular, objectLabelAlpha, _LabelSpecularIntensity, wornEdges, _0UnderlyingSpecular, _0UnderlyingSpecularTint, dirts1Mask, dirts2Mask, _specMin, _specMax, _0GlobalTransparency);
				}
				else {
				 	fixed objectTexture0spec = GetSpecMap(tex2D(_Texture0, i.texcoord_Texture0 / _GlobalScale), _0SpecularContrast, _0SpecularBrightness); 
					resultSpecularMap = GetResultSpecularMap(objectTexture0spec, _SpecularIntensity0, _SpecularTint0, paintMask1, paintMask2, _0Paint1Specular, _0Paint2Specular, objectLabelAlpha, _LabelSpecularIntensity, wornEdges, _0UnderlyingSpecular, _0UnderlyingSpecularTint, dirts1Mask, dirts2Mask, _specMin, _specMax, _0GlobalTransparency);	
				}
				
     			
   				return fixed4 (resultSpecularMap.r, resultSpecularMap.g, resultSpecularMap.b, 1);
  			}
  			ENDCG
 		}
 		
 		//Mask 1
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
   				
   				half2 texcoord_Texture1 : TEXCOORD1;
   				half2 texcoord_BumpMap1 : TEXCOORD2;
   				half2 texcoord_1Paint1MaskTex : TEXCOORD3;
   				half2 texcoord_1Paint2MaskTex : TEXCOORD4; 
   				half2 texroord_Noise : TEXCOORD5; 
  			};
   			
   			float _1MaterialRotation;
   			
			float4 _NormalMap_ST;
			float4 _Texture1_ST;
			float4 _BumpMap1_ST;	
			float4 _1Paint1MaskTex_ST;
			float4 _1Paint2MaskTex_ST;
			float4 _EmissionMask_ST; 

  			v2f vert (appdata_img v){
   				v2f o;
  				o.pos = UnityObjectToClipPos (v.vertex);
   				o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
   	
   				float rad = radians(_1MaterialRotation);
				float s = sin (rad);
				float c = cos (rad);
				float2x2 rotationMatrix = float2x2( c, -s, s, c);
				
   				o.texcoord_Texture1 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _Texture1) + 0.5;
				o.texcoord_BumpMap1 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _BumpMap1) + 0.5;
				o.texcoord_1Paint1MaskTex = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _1Paint1MaskTex) + 0.5;
				o.texcoord_1Paint2MaskTex = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _1Paint2MaskTex) + 0.5;
				o.texroord_Noise = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _EmissionMask) + 0.5;
   				
   				return o; 
  			}
    
  			sampler2D _Texture1;
  			sampler2D _BumpMap1;
			sampler2D _OcclusionMap1;	
			fixed4 _Tint1;
		
			fixed4 _SpecularTint1;
			half _SpecularIntensity1;
			float _1SpecularContrast;
			float _1SpecularBrightness;

			half _Glossiness1;	
			half _GlossinessIntensity1;
			float _1GlossinessContrast;			
			float _1GlossinessBrightness;
				
			float _NormalsStrength1;
			sampler2D _SpecularMap1;
			float _UseSpecularMap1;
			float _GlossinessFromAlpha1;
		
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
   			
   			half _1GlobalTransparency;
			half _1AlbedoTransparency;
			half _1Paint1Transparency;
			half _1Paint2Transparency;
 
  			fixed4 frag (v2f i) : COLOR{
  			
				fixed4 objectLabel = tex2D(_LabelTexture, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw) * _LabelTint;
				fixed4 objectLabelAlpha = tex2D(_LabelTexture, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).a;

				fixed ao = GetResultAoMap(tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).r);
          		fixed4 objectMasks = tex2D(_ObjectMasks, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw); 
           		fixed4 objectTexture1 = tex2D(_Texture1, i.texcoord_Texture1 / _GlobalScale );
            	objectTexture1.a = 1;
            	fixed edgeMask = tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).g; 
            	fixed dirtMask = tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).b;

     
            	fixed noiseR = tex2D(_EmissionMask, i.texroord_Noise / _GlobalScale).b; 	
            	fixed noiseB = tex2D(_EmissionMask, i.texroord_Noise / _GlobalScale).a;     
            	fixed noiseG1 = tex2D(_EmissionMask, i.texroord_Noise / _GlobalScale + 0.5).b; 	
            	fixed noiseG2 = tex2D(_EmissionMask, 1 - i.texroord_Noise / _GlobalScale + 0.5).a;
            	fixed noiseG = clamp( (1 - noiseG1) * 0.5 + (1 - noiseG2) * 0.5, 0, 1);
            
            	fixed4 rgbaNoise = fixed4(noiseR, noiseG, noiseB, 1);
            	
            
            	//worn edges
            	fixed edgeNoise = GetEdgeNoise(rgbaNoise, _1WornEdgesNoiseMix);
        		fixed wornEdges = GetWornEdges(ao, edgeMask, edgeNoise, _1WornEdgesContrast, _1WornEdgesAmount, _1WornEdgesOpacity); 
            	fixed wornEdgesCenter = GetWornEdgesCenter(ao, edgeMask, edgeNoise, _1WornEdgesContrast, _1WornEdgesAmount, _1WornEdgesOpacity, _1WornEdgesBorder); 

            
      		    //paint
				//1
				fixed4 basePaint1 = tex2D(_1Paint1MaskTex, i.texcoord_1Paint1MaskTex);
				fixed4 basePaint1Mask = tex2D(_1Paint1MaskTex, i.texcoord_1Paint1MaskTex).a;	
			
				fixed maskNoiseBorder1 = GetMaskNoiseBorder(rgbaNoise, _1Paint1NoiseMix);			
				fixed4 pMask1 = GetPMask(basePaint1Mask, objectMasks.g, _1Paint1Intensity, maskNoiseBorder1);							  		   
           		fixed4 paintCol1 = GetPaintCol(pMask1, basePaint1, _1Paint1Color);
				fixed paintMask1 =  pMask1.a;
			
			
				//2
				fixed4 basePaint2 = tex2D(_1Paint2MaskTex, i.texcoord_1Paint2MaskTex);
				fixed4 basePaint2Mask = tex2D(_1Paint2MaskTex, i.texcoord_1Paint2MaskTex).a;	
				
				fixed maskNoiseBorder2 = GetMaskNoiseBorder(rgbaNoise, _1Paint2NoiseMix);			
				fixed4 pMask2 = GetPMask(basePaint2Mask, objectMasks.g, _1Paint2Intensity, maskNoiseBorder2);							  		   
           		fixed4 paintCol2 = GetPaintCol(pMask2, basePaint2, _1Paint2Color);
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
            			
				//Specular
				fixed4 resultSpecularMap = 0;
			
				if (_UseSpecularMap1 > 0.5) {
					fixed4 objectTexture1specRGB = GetSpecMap(tex2D(_SpecularMap1, i.texcoord_BumpMap1 / _GlobalScale), _1SpecularContrast, _1SpecularBrightness); 
					resultSpecularMap = GetResultSpecularMapRGB(objectTexture1specRGB, _SpecularIntensity1, _SpecularTint1, paintMask1, paintMask2, _1Paint1Specular, _1Paint2Specular, objectLabelAlpha, _LabelSpecularIntensity, wornEdges, _1UnderlyingSpecular, _1UnderlyingSpecularTint, dirts1Mask, dirts2Mask, _specMin, _specMax, _1GlobalTransparency);
				}
				else {
				 	fixed objectTexture1spec = GetSpecMap(tex2D(_Texture1, i.texcoord_Texture1 / _GlobalScale), _1SpecularContrast, _1SpecularBrightness); 
					resultSpecularMap = GetResultSpecularMap(objectTexture1spec, _SpecularIntensity1, _SpecularTint1, paintMask1, paintMask2, _1Paint1Specular, _1Paint2Specular, objectLabelAlpha, _LabelSpecularIntensity, wornEdges, _1UnderlyingSpecular, _1UnderlyingSpecularTint, dirts1Mask, dirts2Mask, _specMin, _specMax, _1GlobalTransparency);	
				}
			

     			
   				return fixed4 (resultSpecularMap.r, resultSpecularMap.g, resultSpecularMap.b, 1) * objectMasks.g;
  			}
  			ENDCG
 		}
 		
 		
 		//Mask 2
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
   				
   				half2 texcoord_Texture2 : TEXCOORD1;
   				half2 texcoord_BumpMap2 : TEXCOORD2;
   				half2 texcoord_2Paint1MaskTex : TEXCOORD3;
   				half2 texcoord_2Paint2MaskTex : TEXCOORD4; 
   				half2 texroord_Noise : TEXCOORD5; 
  			};
   			
   			float _2MaterialRotation;
   			
			float4 _NormalMap_ST;
			float4 _Texture2_ST;
			float4 _BumpMap2_ST;	
			float4 _2Paint1MaskTex_ST;
			float4 _2Paint2MaskTex_ST;
			float4 _EmissionMask_ST; 

  			v2f vert (appdata_img v){
   				v2f o;
  				o.pos = UnityObjectToClipPos (v.vertex);
   				o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
   	
   				float rad = radians(_2MaterialRotation);
				float s = sin (rad);
				float c = cos (rad);
				float2x2 rotationMatrix = float2x2( c, -s, s, c);
				
   				o.texcoord_Texture2 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _Texture2) + 0.5;
				o.texcoord_BumpMap2 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _BumpMap2) + 0.5;
				o.texcoord_2Paint1MaskTex = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _2Paint1MaskTex) + 0.5;
				o.texcoord_2Paint2MaskTex = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _2Paint2MaskTex) + 0.5;
				o.texroord_Noise = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _EmissionMask) + 0.5;
   				
   				return o; 
  			}
    
  			sampler2D _Texture2;
  			sampler2D _BumpMap2;
			sampler2D _OcclusionMap2;	
			fixed4 _Tint2;
		
			fixed4 _SpecularTint2;
			half _SpecularIntensity2;
			float _2SpecularContrast;
			float _2SpecularBrightness;

			half _Glossiness2;	
			half _GlossinessIntensity2;
			float _2GlossinessContrast;			
			float _2GlossinessBrightness;
				
			float _NormalsStrength2;
			sampler2D _SpecularMap2;
			float _UseSpecularMap2;
			float _GlossinessFromAlpha2;
		
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
   			
   			half _2GlobalTransparency;
			half _2AlbedoTransparency;
			half _2Paint1Transparency;
			half _2Paint2Transparency;
 
  			fixed4 frag (v2f i) : COLOR{
  			
				fixed4 objectLabel = tex2D(_LabelTexture, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw) * _LabelTint;
				fixed4 objectLabelAlpha = tex2D(_LabelTexture, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).a;

				fixed ao = GetResultAoMap(tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).r);
          		fixed4 objectMasks = tex2D(_ObjectMasks, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw); 
           		fixed4 objectTexture2 = tex2D(_Texture2, i.texcoord_Texture2 / _GlobalScale );
            	objectTexture2.a = 1;
            	fixed edgeMask = tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).g; 
            	fixed dirtMask = tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).b;

     
            	fixed noiseR = tex2D(_EmissionMask, i.texroord_Noise / _GlobalScale).b; 	
            	fixed noiseB = tex2D(_EmissionMask, i.texroord_Noise / _GlobalScale).a;     
            	fixed noiseG1 = tex2D(_EmissionMask, i.texroord_Noise / _GlobalScale + 0.5).b; 	
            	fixed noiseG2 = tex2D(_EmissionMask, 1 - i.texroord_Noise / _GlobalScale + 0.5).a;
            	fixed noiseG = clamp( (1 - noiseG1) * 0.5 + (1 - noiseG2) * 0.5, 0, 1);
            
            	fixed4 rgbaNoise = fixed4(noiseR, noiseG, noiseB, 1);
            	
            
            	//worn edges
            	fixed edgeNoise = GetEdgeNoise(rgbaNoise, _2WornEdgesNoiseMix);
        		fixed wornEdges = GetWornEdges(ao, edgeMask, edgeNoise, _2WornEdgesContrast, _2WornEdgesAmount, _2WornEdgesOpacity); 
            	fixed wornEdgesCenter = GetWornEdgesCenter(ao, edgeMask, edgeNoise, _2WornEdgesContrast, _2WornEdgesAmount, _2WornEdgesOpacity, _2WornEdgesBorder); 

            
      		    //paint
				//1
				fixed4 basePaint1 = tex2D(_2Paint1MaskTex, i.texcoord_2Paint1MaskTex);
				fixed4 basePaint1Mask = tex2D(_2Paint1MaskTex, i.texcoord_2Paint1MaskTex).a;	
			
				fixed maskNoiseBorder1 = GetMaskNoiseBorder(rgbaNoise, _2Paint1NoiseMix);			
				fixed4 pMask1 = GetPMask(basePaint1Mask, objectMasks.b, _2Paint1Intensity, maskNoiseBorder1);							  		   
           		fixed4 paintCol1 = GetPaintCol(pMask1, basePaint1, _2Paint1Color);
				fixed paintMask1 =  pMask1.a;
			
			
				//2
				fixed4 basePaint2 = tex2D(_2Paint2MaskTex, i.texcoord_2Paint2MaskTex);
				fixed4 basePaint2Mask = tex2D(_2Paint2MaskTex, i.texcoord_2Paint2MaskTex).a;	
				
				fixed maskNoiseBorder2 = GetMaskNoiseBorder(rgbaNoise, _2Paint2NoiseMix);			
				fixed4 pMask2 = GetPMask(basePaint2Mask, objectMasks.b, _2Paint2Intensity, maskNoiseBorder2);							  		   
           		fixed4 paintCol2 = GetPaintCol(pMask2, basePaint2, _2Paint2Color);
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
            			
				//Specular
				fixed4 resultSpecularMap = 0;
			
				if (_UseSpecularMap2 > 0.5) {
					fixed4 objectTexture2specRGB = GetSpecMap(tex2D(_SpecularMap2, i.texcoord_BumpMap2 / _GlobalScale), _2SpecularContrast, _2SpecularBrightness); 
					resultSpecularMap = GetResultSpecularMapRGB(objectTexture2specRGB, _SpecularIntensity2, _SpecularTint2, paintMask1, paintMask2, _2Paint1Specular, _2Paint2Specular, objectLabelAlpha, _LabelSpecularIntensity, wornEdges, _2UnderlyingSpecular, _2UnderlyingSpecularTint, dirts1Mask, dirts2Mask, _specMin, _specMax, _2GlobalTransparency);
				}
				else {
				 	fixed objectTexture2spec = GetSpecMap(tex2D(_Texture2, i.texcoord_Texture2 / _GlobalScale), _2SpecularContrast, _2SpecularBrightness); 
					resultSpecularMap = GetResultSpecularMap(objectTexture2spec, _SpecularIntensity2, _SpecularTint2, paintMask1, paintMask2, _2Paint1Specular, _2Paint2Specular, objectLabelAlpha, _LabelSpecularIntensity, wornEdges, _2UnderlyingSpecular, _2UnderlyingSpecularTint, dirts1Mask, dirts2Mask, _specMin, _specMax, _2GlobalTransparency);		
				}
			

     			
   				return fixed4 (resultSpecularMap.r, resultSpecularMap.g, resultSpecularMap.b, 1) * objectMasks.b;
  			}
  			ENDCG
 		}
 		
 		
 		//Mask 3
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
   				
   				half2 texcoord_Texture3 : TEXCOORD1;
   				half2 texcoord_BumpMap3 : TEXCOORD2;
   				half2 texcoord_3Paint1MaskTex : TEXCOORD3;
   				half2 texcoord_3Paint2MaskTex : TEXCOORD4; 
   				half2 texroord_Noise : TEXCOORD5; 
  			};
   			
   			float _3MaterialRotation;
   			
			float4 _NormalMap_ST;
			float4 _Texture3_ST;
			float4 _BumpMap3_ST;	
			float4 _3Paint1MaskTex_ST;
			float4 _3Paint2MaskTex_ST;
			float4 _EmissionMask_ST; 

  			v2f vert (appdata_img v){
   				v2f o;
  				o.pos = UnityObjectToClipPos (v.vertex);
   				o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
   	
   				float rad = radians(_3MaterialRotation);
				float s = sin (rad);
				float c = cos (rad);
				float2x2 rotationMatrix = float2x2( c, -s, s, c);
				
   				o.texcoord_Texture3 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _Texture3) + 0.5;
				o.texcoord_BumpMap3 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _BumpMap3) + 0.5;
				o.texcoord_3Paint1MaskTex = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _3Paint1MaskTex) + 0.5;
				o.texcoord_3Paint2MaskTex = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _3Paint2MaskTex) + 0.5;
				o.texroord_Noise = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _EmissionMask) + 0.5;
   				
   				return o; 
  			}
    
  			sampler2D _Texture3;
  			sampler2D _BumpMap3;
			sampler2D _OcclusionMap3;	
			fixed4 _Tint3;
		
			fixed4 _SpecularTint3;
			half _SpecularIntensity3;
			float _3SpecularContrast;
			float _3SpecularBrightness;

			half _Glossiness3;	
			half _GlossinessIntensity3;
			float _3GlossinessContrast;			
			float _3GlossinessBrightness;
				
			float _NormalsStrength3;
			sampler2D _SpecularMap3;
			float _UseSpecularMap3;
			float _GlossinessFromAlpha3;
		
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
   
   			half _3GlobalTransparency;
			half _3AlbedoTransparency;
			half _3Paint1Transparency;
			half _3Paint2Transparency;
 
  			fixed4 frag (v2f i) : COLOR{
  			
				fixed4 objectLabel = tex2D(_LabelTexture, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw) * _LabelTint;
				fixed4 objectLabelAlpha = tex2D(_LabelTexture, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).a;

				fixed ao = GetResultAoMap(tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).r);
          		fixed4 objectMasks = tex2D(_ObjectMasks, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw); 
           		fixed4 objectTexture3 = tex2D(_Texture3, i.texcoord_Texture3 / _GlobalScale );
            	objectTexture3.a = 1;
            	fixed edgeMask = tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).g; 
            	fixed dirtMask = tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).b;

     
            	fixed noiseR = tex2D(_EmissionMask, i.texroord_Noise / _GlobalScale).b; 	
            	fixed noiseB = tex2D(_EmissionMask, i.texroord_Noise / _GlobalScale).a;     
            	fixed noiseG1 = tex2D(_EmissionMask, i.texroord_Noise / _GlobalScale + 0.5).b; 	
            	fixed noiseG2 = tex2D(_EmissionMask, 1 - i.texroord_Noise / _GlobalScale + 0.5).a;
            	fixed noiseG = clamp( (1 - noiseG1) * 0.5 + (1 - noiseG2) * 0.5, 0, 1);
            
            	fixed4 rgbaNoise = fixed4(noiseR, noiseG, noiseB, 1);
            	
            
            	//worn edges
            	fixed edgeNoise = GetEdgeNoise(rgbaNoise, _3WornEdgesNoiseMix);
        		fixed wornEdges = GetWornEdges(ao, edgeMask, edgeNoise, _3WornEdgesContrast, _3WornEdgesAmount, _3WornEdgesOpacity); 
            	fixed wornEdgesCenter = GetWornEdgesCenter(ao, edgeMask, edgeNoise, _3WornEdgesContrast, _3WornEdgesAmount, _3WornEdgesOpacity, _3WornEdgesBorder); 

            
      		    //paint
				//1
				fixed4 basePaint1 = tex2D(_3Paint1MaskTex, i.texcoord_3Paint1MaskTex);
				fixed4 basePaint1Mask = tex2D(_3Paint1MaskTex, i.texcoord_3Paint1MaskTex).a;	
			
				fixed maskNoiseBorder1 = GetMaskNoiseBorder(rgbaNoise, _3Paint1NoiseMix);			
				fixed4 pMask1 = GetPMask(basePaint1Mask, objectMasks.a, _3Paint1Intensity, maskNoiseBorder1);							  		   
           		fixed4 paintCol1 = GetPaintCol(pMask1, basePaint1, _3Paint1Color);
				fixed paintMask1 =  pMask1.a;
			
			
				//2
				fixed4 basePaint2 = tex2D(_3Paint2MaskTex, i.texcoord_3Paint2MaskTex);
				fixed4 basePaint2Mask = tex2D(_3Paint2MaskTex, i.texcoord_3Paint2MaskTex).a;	
				
				fixed maskNoiseBorder2 = GetMaskNoiseBorder(rgbaNoise, _3Paint2NoiseMix);			
				fixed4 pMask2 = GetPMask(basePaint2Mask, objectMasks.a, _3Paint2Intensity, maskNoiseBorder2);							  		   
           		fixed4 paintCol2 = GetPaintCol(pMask2, basePaint2, _3Paint2Color);
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
            			
				//Specular
				fixed4 resultSpecularMap = 0;
			
				if (_UseSpecularMap3 > 0.5) {
					fixed4 objectTexture3specRGB = GetSpecMap(tex2D(_SpecularMap3, i.texcoord_BumpMap3 / _GlobalScale), _3SpecularContrast, _3SpecularBrightness); 
					resultSpecularMap = GetResultSpecularMapRGB(objectTexture3specRGB, _SpecularIntensity3, _SpecularTint3, paintMask1, paintMask2, _3Paint1Specular, _3Paint2Specular, objectLabelAlpha, _LabelSpecularIntensity, wornEdges, _3UnderlyingSpecular, _3UnderlyingSpecularTint, dirts1Mask, dirts2Mask, _specMin, _specMax, _3GlobalTransparency);	
				}
				else {
				 	fixed objectTexture3spec = GetSpecMap(tex2D(_Texture3, i.texcoord_Texture3 / _GlobalScale), _3SpecularContrast, _3SpecularBrightness); 
					resultSpecularMap = GetResultSpecularMap(objectTexture3spec, _SpecularIntensity3, _SpecularTint3, paintMask1, paintMask2, _3Paint1Specular, _3Paint2Specular, objectLabelAlpha, _LabelSpecularIntensity, wornEdges, _3UnderlyingSpecular, _3UnderlyingSpecularTint, dirts1Mask, dirts2Mask, _specMin, _specMax, _3GlobalTransparency);	
				}
			

     			
   				return fixed4 (resultSpecularMap.r, resultSpecularMap.g, resultSpecularMap.b, 1) * objectMasks.a;
  			}
  			ENDCG
 		}
 		
 		
 		//Mask 4
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
   				
   				half2 texcoord_Texture4 : TEXCOORD1;
   				half2 texcoord_BumpMap4 : TEXCOORD2;
   				half2 texcoord_4Paint1MaskTex : TEXCOORD3;
   				half2 texcoord_4Paint2MaskTex : TEXCOORD4; 
   				half2 texroord_Noise : TEXCOORD5; 
  			};
   			
   			float _4MaterialRotation;
   			
			float4 _NormalMap_ST;
			float4 _Texture4_ST;
			float4 _BumpMap4_ST;	
			float4 _4Paint1MaskTex_ST;
			float4 _4Paint2MaskTex_ST;
			float4 _EmissionMask_ST; 

  			v2f vert (appdata_img v){
   				v2f o;
  				o.pos = UnityObjectToClipPos (v.vertex);
   				o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
   	
   				float rad = radians(_4MaterialRotation);
				float s = sin (rad);
				float c = cos (rad);
				float2x2 rotationMatrix = float2x2( c, -s, s, c);
				
   				o.texcoord_Texture4 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _Texture4) + 0.5;
				o.texcoord_BumpMap4 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _BumpMap4) + 0.5;
				o.texcoord_4Paint1MaskTex = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _4Paint1MaskTex) + 0.5;
				o.texcoord_4Paint2MaskTex = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _4Paint2MaskTex) + 0.5;
				o.texroord_Noise = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _EmissionMask) + 0.5;
   				
   				return o; 
  			}
    
  			sampler2D _Texture4;
  			sampler2D _BumpMap4;
			sampler2D _OcclusionMap4;	
			fixed4 _Tint4;
		
			fixed4 _SpecularTint4;
			half _SpecularIntensity4;
			float _4SpecularContrast;
			float _4SpecularBrightness;

			half _Glossiness4;	
			half _GlossinessIntensity4;
			float _4GlossinessContrast;			
			float _4GlossinessBrightness;
				
			float _NormalsStrength4;
			sampler2D _SpecularMap4;
			float _UseSpecularMap4;
			float _GlossinessFromAlpha4;
		
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
   
   			half _4GlobalTransparency;
			half _4AlbedoTransparency;
			half _4Paint1Transparency;
			half _4Paint2Transparency;
 
  			fixed4 frag (v2f i) : COLOR{
  			
				fixed4 objectLabel = tex2D(_LabelTexture, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw) * _LabelTint;
				fixed4 objectLabelAlpha = tex2D(_LabelTexture, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).a;

				fixed ao = GetResultAoMap(tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).r);
          		fixed4 objectMasks = tex2D(_ObjectMasks2, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw); 
           		fixed4 objectTexture4 = tex2D(_Texture4, i.texcoord_Texture4 / _GlobalScale );
            	objectTexture4.a = 1;
            	fixed edgeMask = tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).g; 
            	fixed dirtMask = tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).b;

     
            	fixed noiseR = tex2D(_EmissionMask, i.texroord_Noise / _GlobalScale).b; 	
            	fixed noiseB = tex2D(_EmissionMask, i.texroord_Noise / _GlobalScale).a;     
            	fixed noiseG1 = tex2D(_EmissionMask, i.texroord_Noise / _GlobalScale + 0.5).b; 	
            	fixed noiseG2 = tex2D(_EmissionMask, 1 - i.texroord_Noise / _GlobalScale + 0.5).a;
            	fixed noiseG = clamp( (1 - noiseG1) * 0.5 + (1 - noiseG2) * 0.5, 0, 1);
            
            	fixed4 rgbaNoise = fixed4(noiseR, noiseG, noiseB, 1);
            	
            
            	//worn edges
            	fixed edgeNoise = GetEdgeNoise(rgbaNoise, _4WornEdgesNoiseMix);
        		fixed wornEdges = GetWornEdges(ao, edgeMask, edgeNoise, _4WornEdgesContrast, _4WornEdgesAmount, _4WornEdgesOpacity); 
            	fixed wornEdgesCenter = GetWornEdgesCenter(ao, edgeMask, edgeNoise, _4WornEdgesContrast, _4WornEdgesAmount, _4WornEdgesOpacity, _4WornEdgesBorder); 

            
      		    //paint
				//1
				fixed4 basePaint1 = tex2D(_4Paint1MaskTex, i.texcoord_4Paint1MaskTex);
				fixed4 basePaint1Mask = tex2D(_4Paint1MaskTex, i.texcoord_4Paint1MaskTex).a;	
			
				fixed maskNoiseBorder1 = GetMaskNoiseBorder(rgbaNoise, _4Paint1NoiseMix);			
				fixed4 pMask1 = GetPMask(basePaint1Mask, objectMasks.r, _4Paint1Intensity, maskNoiseBorder1);							  		   
           		fixed4 paintCol1 = GetPaintCol(pMask1, basePaint1, _4Paint1Color);
				fixed paintMask1 =  pMask1.a;
			
			
				//2
				fixed4 basePaint2 = tex2D(_4Paint2MaskTex, i.texcoord_4Paint2MaskTex);
				fixed4 basePaint2Mask = tex2D(_4Paint2MaskTex, i.texcoord_4Paint2MaskTex).a;	
				
				fixed maskNoiseBorder2 = GetMaskNoiseBorder(rgbaNoise, _4Paint2NoiseMix);			
				fixed4 pMask2 = GetPMask(basePaint2Mask, objectMasks.r, _4Paint2Intensity, maskNoiseBorder2);							  		   
           		fixed4 paintCol2 = GetPaintCol(pMask2, basePaint2, _4Paint2Color);
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
            			
				//Specular
				fixed4 resultSpecularMap = 0;
			
				if (_UseSpecularMap4 > 0.5) {
					fixed4 objectTexture4specRGB = GetSpecMap(tex2D(_SpecularMap4, i.texcoord_BumpMap4 / _GlobalScale), _4SpecularContrast, _4SpecularBrightness); 
					resultSpecularMap = GetResultSpecularMapRGB(objectTexture4specRGB, _SpecularIntensity4, _SpecularTint4, paintMask1, paintMask2, _4Paint1Specular, _4Paint2Specular, objectLabelAlpha, _LabelSpecularIntensity, wornEdges, _4UnderlyingSpecular, _4UnderlyingSpecularTint, dirts1Mask, dirts2Mask, _specMin, _specMax, _4GlobalTransparency);
				}
				else {
				 	fixed objectTexture4spec = GetSpecMap(tex2D(_Texture4, i.texcoord_Texture4 / _GlobalScale), _4SpecularContrast, _4SpecularBrightness); 
					resultSpecularMap = GetResultSpecularMap(objectTexture4spec, _SpecularIntensity4, _SpecularTint4, paintMask1, paintMask2, _4Paint1Specular, _4Paint2Specular, objectLabelAlpha, _LabelSpecularIntensity, wornEdges, _4UnderlyingSpecular, _4UnderlyingSpecularTint, dirts1Mask, dirts2Mask, _specMin, _specMax, _4GlobalTransparency);		
				}
			

     			
   				return fixed4 (resultSpecularMap.r, resultSpecularMap.g, resultSpecularMap.b, 1) * objectMasks.r;
  			}
  			ENDCG
 		}
 		
 		
 		//Mask 5
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
   				
   				half2 texcoord_Texture5 : TEXCOORD1;
   				half2 texcoord_BumpMap5 : TEXCOORD2;
   				half2 texcoord_5Paint1MaskTex : TEXCOORD3;
   				half2 texcoord_5Paint2MaskTex : TEXCOORD4; 
   				half2 texroord_Noise : TEXCOORD5; 
  			};
   			
   			float _5MaterialRotation;
   			
			float4 _NormalMap_ST;
			float4 _Texture5_ST;
			float4 _BumpMap5_ST;	
			float4 _5Paint1MaskTex_ST;
			float4 _5Paint2MaskTex_ST;
			float4 _EmissionMask_ST; 

  			v2f vert (appdata_img v){
   				v2f o;
  				o.pos = UnityObjectToClipPos (v.vertex);
   				o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
   	
   				float rad = radians(_5MaterialRotation);
				float s = sin (rad);
				float c = cos (rad);
				float2x2 rotationMatrix = float2x2( c, -s, s, c);
				
   				o.texcoord_Texture5 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _Texture5) + 0.5;
				o.texcoord_BumpMap5 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _BumpMap5) + 0.5;
				o.texcoord_5Paint1MaskTex = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _5Paint1MaskTex) + 0.5;
				o.texcoord_5Paint2MaskTex = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _5Paint2MaskTex) + 0.5;
				o.texroord_Noise = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _EmissionMask) + 0.5;
   				
   				return o; 
  			}
    
  			sampler2D _Texture5;
  			sampler2D _BumpMap5;
			sampler2D _OcclusionMap5;	
			fixed4 _Tint5;
		
			fixed4 _SpecularTint5;
			half _SpecularIntensity5;
			float _5SpecularContrast;
			float _5SpecularBrightness;

			half _Glossiness5;	
			half _GlossinessIntensity5;
			float _5GlossinessContrast;			
			float _5GlossinessBrightness;
				
			float _NormalsStrength5;
			sampler2D _SpecularMap5;
			float _UseSpecularMap5;
			float _GlossinessFromAlpha5;
		
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
   			
   			half _5GlobalTransparency;
			half _5AlbedoTransparency;
			half _5Paint1Transparency;
			half _5Paint2Transparency;
 
  			fixed4 frag (v2f i) : COLOR{
  			
				fixed4 objectLabel = tex2D(_LabelTexture, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw) * _LabelTint;
				fixed4 objectLabelAlpha = tex2D(_LabelTexture, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).a;

				fixed ao = GetResultAoMap(tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).r);
          		fixed4 objectMasks = tex2D(_ObjectMasks2, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw); 
           		fixed4 objectTexture5 = tex2D(_Texture5, i.texcoord_Texture5 / _GlobalScale );
            	objectTexture5.a = 1;
            	fixed edgeMask = tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).g; 
            	fixed dirtMask = tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).b;

     
            	fixed noiseR = tex2D(_EmissionMask, i.texroord_Noise / _GlobalScale).b; 	
            	fixed noiseB = tex2D(_EmissionMask, i.texroord_Noise / _GlobalScale).a;     
            	fixed noiseG1 = tex2D(_EmissionMask, i.texroord_Noise / _GlobalScale + 0.5).b; 	
            	fixed noiseG2 = tex2D(_EmissionMask, 1 - i.texroord_Noise / _GlobalScale + 0.5).a;
            	fixed noiseG = clamp( (1 - noiseG1) * 0.5 + (1 - noiseG2) * 0.5, 0, 1);
            
            	fixed4 rgbaNoise = fixed4(noiseR, noiseG, noiseB, 1);
            	
            
            	//worn edges
            	fixed edgeNoise = GetEdgeNoise(rgbaNoise, _5WornEdgesNoiseMix);
        		fixed wornEdges = GetWornEdges(ao, edgeMask, edgeNoise, _5WornEdgesContrast, _5WornEdgesAmount, _5WornEdgesOpacity); 
            	fixed wornEdgesCenter = GetWornEdgesCenter(ao, edgeMask, edgeNoise, _5WornEdgesContrast, _5WornEdgesAmount, _5WornEdgesOpacity, _5WornEdgesBorder); 

            
      		    //paint
				//1
				fixed4 basePaint1 = tex2D(_5Paint1MaskTex, i.texcoord_5Paint1MaskTex);
				fixed4 basePaint1Mask = tex2D(_5Paint1MaskTex, i.texcoord_5Paint1MaskTex).a;	
			
				fixed maskNoiseBorder1 = GetMaskNoiseBorder(rgbaNoise, _5Paint1NoiseMix);			
				fixed4 pMask1 = GetPMask(basePaint1Mask, objectMasks.g, _5Paint1Intensity, maskNoiseBorder1);							  		   
           		fixed4 paintCol1 = GetPaintCol(pMask1, basePaint1, _5Paint1Color);
				fixed paintMask1 =  pMask1.a;
			
			
				//2
				fixed4 basePaint2 = tex2D(_5Paint2MaskTex, i.texcoord_5Paint2MaskTex);
				fixed4 basePaint2Mask = tex2D(_5Paint2MaskTex, i.texcoord_5Paint2MaskTex).a;	
				
				fixed maskNoiseBorder2 = GetMaskNoiseBorder(rgbaNoise, _5Paint2NoiseMix);			
				fixed4 pMask2 = GetPMask(basePaint2Mask, objectMasks.g, _5Paint2Intensity, maskNoiseBorder2);							  		   
           		fixed4 paintCol2 = GetPaintCol(pMask2, basePaint2, _5Paint2Color);
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
            			
				//Specular
				fixed4 resultSpecularMap = 0;
			
				if (_UseSpecularMap5 > 0.5) {
					fixed4 objectTexture5specRGB = GetSpecMap(tex2D(_SpecularMap5, i.texcoord_BumpMap5 / _GlobalScale), _5SpecularContrast, _5SpecularBrightness); 
					resultSpecularMap = GetResultSpecularMapRGB(objectTexture5specRGB, _SpecularIntensity5, _SpecularTint5, paintMask1, paintMask2, _5Paint1Specular, _5Paint2Specular, objectLabelAlpha, _LabelSpecularIntensity, wornEdges, _5UnderlyingSpecular, _5UnderlyingSpecularTint, dirts1Mask, dirts2Mask, _specMin, _specMax, _5GlobalTransparency);
				}
				else {
				 	fixed objectTexture5spec = GetSpecMap(tex2D(_Texture5, i.texcoord_Texture5 / _GlobalScale), _5SpecularContrast, _5SpecularBrightness); 
					resultSpecularMap = GetResultSpecularMap(objectTexture5spec, _SpecularIntensity5, _SpecularTint5, paintMask1, paintMask2, _5Paint1Specular, _5Paint2Specular, objectLabelAlpha, _LabelSpecularIntensity, wornEdges, _5UnderlyingSpecular, _5UnderlyingSpecularTint, dirts1Mask, dirts2Mask, _specMin, _specMax, _5GlobalTransparency);	
				}
			

     			
   				return fixed4 (resultSpecularMap.r, resultSpecularMap.g, resultSpecularMap.b, 1) * objectMasks.g;
  			}
  			ENDCG
 		}
 		
 		
 		//Mask 6
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
   				
   				half2 texcoord_Texture6 : TEXCOORD1;
   				half2 texcoord_BumpMap6 : TEXCOORD2;
   				half2 texcoord_6Paint1MaskTex : TEXCOORD3;
   				half2 texcoord_6Paint2MaskTex : TEXCOORD4; 
   				half2 texroord_Noise : TEXCOORD5; 
  			};
   			
   			float _6MaterialRotation;
   			
			float4 _NormalMap_ST;
			float4 _Texture6_ST;
			float4 _BumpMap6_ST;	
			float4 _6Paint1MaskTex_ST;
			float4 _6Paint2MaskTex_ST;
			float4 _EmissionMask_ST; 

  			v2f vert (appdata_img v){
   				v2f o;
  				o.pos = UnityObjectToClipPos (v.vertex);
   				o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
   	
   				float rad = radians(_6MaterialRotation);
				float s = sin (rad);
				float c = cos (rad);
				float2x2 rotationMatrix = float2x2( c, -s, s, c);
				
   				o.texcoord_Texture6 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _Texture6) + 0.5;
				o.texcoord_BumpMap6 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _BumpMap6) + 0.5;
				o.texcoord_6Paint1MaskTex = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _6Paint1MaskTex) + 0.5;
				o.texcoord_6Paint2MaskTex = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _6Paint2MaskTex) + 0.5;
				o.texroord_Noise = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _EmissionMask) + 0.5;
   				
   				return o; 
  			}
    
  			sampler2D _Texture6;
  			sampler2D _BumpMap6;
			sampler2D _OcclusionMap6;	
			fixed4 _Tint6;
		
			fixed4 _SpecularTint6;
			half _SpecularIntensity6;
			float _6SpecularContrast;
			float _6SpecularBrightness;

			half _Glossiness6;	
			half _GlossinessIntensity6;
			float _6GlossinessContrast;			
			float _6GlossinessBrightness;
				
			float _NormalsStrength6;
			sampler2D _SpecularMap6;
			float _UseSpecularMap6;
			float _GlossinessFromAlpha6;
		
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
 
   			half _6GlobalTransparency;
			half _6AlbedoTransparency;
			half _6Paint1Transparency;
			half _6Paint2Transparency;  
 
  			fixed4 frag (v2f i) : COLOR{
  			
				fixed4 objectLabel = tex2D(_LabelTexture, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw) * _LabelTint;
				fixed4 objectLabelAlpha = tex2D(_LabelTexture, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).a;

				fixed ao = GetResultAoMap(tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).r);
          		fixed4 objectMasks = tex2D(_ObjectMasks2, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw); 
           		fixed4 objectTexture6 = tex2D(_Texture6, i.texcoord_Texture6 / _GlobalScale );
            	objectTexture6.a = 1;
            	fixed edgeMask = tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).g; 
            	fixed dirtMask = tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).b;

     
            	fixed noiseR = tex2D(_EmissionMask, i.texroord_Noise / _GlobalScale).b; 	
            	fixed noiseB = tex2D(_EmissionMask, i.texroord_Noise / _GlobalScale).a;     
            	fixed noiseG1 = tex2D(_EmissionMask, i.texroord_Noise / _GlobalScale + 0.5).b; 	
            	fixed noiseG2 = tex2D(_EmissionMask, 1 - i.texroord_Noise / _GlobalScale + 0.5).a;
            	fixed noiseG = clamp( (1 - noiseG1) * 0.5 + (1 - noiseG2) * 0.5, 0, 1);
            
            	fixed4 rgbaNoise = fixed4(noiseR, noiseG, noiseB, 1);
            	
            
            	//worn edges
            	fixed edgeNoise = GetEdgeNoise(rgbaNoise, _6WornEdgesNoiseMix);
        		fixed wornEdges = GetWornEdges(ao, edgeMask, edgeNoise, _6WornEdgesContrast, _6WornEdgesAmount, _6WornEdgesOpacity); 
            	fixed wornEdgesCenter = GetWornEdgesCenter(ao, edgeMask, edgeNoise, _6WornEdgesContrast, _6WornEdgesAmount, _6WornEdgesOpacity, _6WornEdgesBorder); 

            
      		    //paint
				//1
				fixed4 basePaint1 = tex2D(_6Paint1MaskTex, i.texcoord_6Paint1MaskTex);
				fixed4 basePaint1Mask = tex2D(_6Paint1MaskTex, i.texcoord_6Paint1MaskTex).a;	
			
				fixed maskNoiseBorder1 = GetMaskNoiseBorder(rgbaNoise, _6Paint1NoiseMix);			
				fixed4 pMask1 = GetPMask(basePaint1Mask, objectMasks.b, _6Paint1Intensity, maskNoiseBorder1);							  		   
           		fixed4 paintCol1 = GetPaintCol(pMask1, basePaint1, _6Paint1Color);
				fixed paintMask1 =  pMask1.a;
			
			
				//2
				fixed4 basePaint2 = tex2D(_6Paint2MaskTex, i.texcoord_6Paint2MaskTex);
				fixed4 basePaint2Mask = tex2D(_6Paint2MaskTex, i.texcoord_6Paint2MaskTex).a;	
				
				fixed maskNoiseBorder2 = GetMaskNoiseBorder(rgbaNoise, _6Paint2NoiseMix);			
				fixed4 pMask2 = GetPMask(basePaint2Mask, objectMasks.b, _6Paint2Intensity, maskNoiseBorder2);							  		   
           		fixed4 paintCol2 = GetPaintCol(pMask2, basePaint2, _6Paint2Color);
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
            			
				//Specular
				fixed4 resultSpecularMap = 0;
			
				if (_UseSpecularMap6 > 0.5) {
					fixed4 objectTexture6specRGB = GetSpecMap(tex2D(_SpecularMap6, i.texcoord_BumpMap6 / _GlobalScale), _6SpecularContrast, _6SpecularBrightness); 
					resultSpecularMap = GetResultSpecularMapRGB(objectTexture6specRGB, _SpecularIntensity6, _SpecularTint6, paintMask1, paintMask2, _6Paint1Specular, _6Paint2Specular, objectLabelAlpha, _LabelSpecularIntensity, wornEdges, _6UnderlyingSpecular, _6UnderlyingSpecularTint, dirts1Mask, dirts2Mask, _specMin, _specMax, _6GlobalTransparency);
				}
				else {
				 	fixed objectTexture6spec = GetSpecMap(tex2D(_Texture6, i.texcoord_Texture6 / _GlobalScale), _6SpecularContrast, _6SpecularBrightness); 
					resultSpecularMap = GetResultSpecularMap(objectTexture6spec, _SpecularIntensity6, _SpecularTint6, paintMask1, paintMask2, _6Paint1Specular, _6Paint2Specular, objectLabelAlpha, _LabelSpecularIntensity, wornEdges, _6UnderlyingSpecular, _6UnderlyingSpecularTint, dirts1Mask, dirts2Mask, _specMin, _specMax, _6GlobalTransparency);	
				}
			

     			
   				return fixed4 (resultSpecularMap.r, resultSpecularMap.g, resultSpecularMap.b, 1) * objectMasks.b;
  			}
  			ENDCG
 		}
 		
 		
 		//Mask 7
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
   				
   				half2 texcoord_Texture7 : TEXCOORD1;
   				half2 texcoord_BumpMap7 : TEXCOORD2;
   				half2 texcoord_7Paint1MaskTex : TEXCOORD3;
   				half2 texcoord_7Paint2MaskTex : TEXCOORD4; 
   				half2 texroord_Noise : TEXCOORD5; 
  			};
   			
   			float _7MaterialRotation;
   			
			float4 _NormalMap_ST;
			float4 _Texture7_ST;
			float4 _BumpMap7_ST;	
			float4 _7Paint1MaskTex_ST;
			float4 _7Paint2MaskTex_ST;
			float4 _EmissionMask_ST; 

  			v2f vert (appdata_img v){
   				v2f o;
  				o.pos = UnityObjectToClipPos (v.vertex);
   				o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
   	
   				float rad = radians(_7MaterialRotation);
				float s = sin (rad);
				float c = cos (rad);
				float2x2 rotationMatrix = float2x2( c, -s, s, c);
				
   				o.texcoord_Texture7 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _Texture7) + 0.5;
				o.texcoord_BumpMap7 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _BumpMap7) + 0.5;
				o.texcoord_7Paint1MaskTex = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _7Paint1MaskTex) + 0.5;
				o.texcoord_7Paint2MaskTex = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _7Paint2MaskTex) + 0.5;
				o.texroord_Noise = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _EmissionMask) + 0.5;
   				
   				return o; 
  			}
    
  			sampler2D _Texture7;
  			sampler2D _BumpMap7;
			sampler2D _OcclusionMap7;	
			fixed4 _Tint7;
		
			fixed4 _SpecularTint7;
			half _SpecularIntensity7;
			float _7SpecularContrast;
			float _7SpecularBrightness;

			half _Glossiness7;	
			half _GlossinessIntensity7;
			float _7GlossinessContrast;			
			float _7GlossinessBrightness;
				
			float _NormalsStrength7;
			sampler2D _SpecularMap7;
			float _UseSpecularMap7;
			float _GlossinessFromAlpha7;
		
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
   			
   			half _7GlobalTransparency;
			half _7AlbedoTransparency;
			half _7Paint1Transparency;
			half _7Paint2Transparency;
 
  			fixed4 frag (v2f i) : COLOR{
  			
				fixed4 objectLabel = tex2D(_LabelTexture, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw) * _LabelTint;
				fixed4 objectLabelAlpha = tex2D(_LabelTexture, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).a;

				fixed ao = GetResultAoMap(tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).r);
          		fixed4 objectMasks = tex2D(_ObjectMasks2, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw); 
           		fixed4 objectTexture7 = tex2D(_Texture7, i.texcoord_Texture7 / _GlobalScale );
            	objectTexture7.a = 1;
            	fixed edgeMask = tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).g; 
            	fixed dirtMask = tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).b;

     
            	fixed noiseR = tex2D(_EmissionMask, i.texroord_Noise / _GlobalScale).b; 	
            	fixed noiseB = tex2D(_EmissionMask, i.texroord_Noise / _GlobalScale).a;     
            	fixed noiseG1 = tex2D(_EmissionMask, i.texroord_Noise / _GlobalScale + 0.5).b; 	
            	fixed noiseG2 = tex2D(_EmissionMask, 1 - i.texroord_Noise / _GlobalScale + 0.5).a;
            	fixed noiseG = clamp( (1 - noiseG1) * 0.5 + (1 - noiseG2) * 0.5, 0, 1);
            
            	fixed4 rgbaNoise = fixed4(noiseR, noiseG, noiseB, 1);
            	
            
            	//worn edges
            	fixed edgeNoise = GetEdgeNoise(rgbaNoise, _7WornEdgesNoiseMix);
        		fixed wornEdges = GetWornEdges(ao, edgeMask, edgeNoise, _7WornEdgesContrast, _7WornEdgesAmount, _7WornEdgesOpacity); 
            	fixed wornEdgesCenter = GetWornEdgesCenter(ao, edgeMask, edgeNoise, _7WornEdgesContrast, _7WornEdgesAmount, _7WornEdgesOpacity, _7WornEdgesBorder); 

            
      		    //paint
				//1
				fixed4 basePaint1 = tex2D(_7Paint1MaskTex, i.texcoord_7Paint1MaskTex);
				fixed4 basePaint1Mask = tex2D(_7Paint1MaskTex, i.texcoord_7Paint1MaskTex).a;	
			
				fixed maskNoiseBorder1 = GetMaskNoiseBorder(rgbaNoise, _7Paint1NoiseMix);			
				fixed4 pMask1 = GetPMask(basePaint1Mask, objectMasks.a, _7Paint1Intensity, maskNoiseBorder1);							  		   
           		fixed4 paintCol1 = GetPaintCol(pMask1, basePaint1, _7Paint1Color);
				fixed paintMask1 =  pMask1.a;
			
			
				//2
				fixed4 basePaint2 = tex2D(_7Paint2MaskTex, i.texcoord_7Paint2MaskTex);
				fixed4 basePaint2Mask = tex2D(_7Paint2MaskTex, i.texcoord_7Paint2MaskTex).a;	
				
				fixed maskNoiseBorder2 = GetMaskNoiseBorder(rgbaNoise, _7Paint2NoiseMix);			
				fixed4 pMask2 = GetPMask(basePaint2Mask, objectMasks.a, _7Paint2Intensity, maskNoiseBorder2);							  		   
           		fixed4 paintCol2 = GetPaintCol(pMask2, basePaint2, _7Paint2Color);
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
            			
				//Specular
				fixed4 resultSpecularMap = 0;
			
				if (_UseSpecularMap7 > 0.5) {
					fixed4 objectTexture7specRGB = GetSpecMap(tex2D(_SpecularMap7, i.texcoord_BumpMap7 / _GlobalScale), _7SpecularContrast, _7SpecularBrightness); 
					resultSpecularMap = GetResultSpecularMapRGB(objectTexture7specRGB, _SpecularIntensity7, _SpecularTint7, paintMask1, paintMask2, _7Paint1Specular, _7Paint2Specular, objectLabelAlpha, _LabelSpecularIntensity, wornEdges, _7UnderlyingSpecular, _7UnderlyingSpecularTint, dirts1Mask, dirts2Mask, _specMin, _specMax, _7GlobalTransparency);
				}
				else {
				 	fixed objectTexture7spec = GetSpecMap(tex2D(_Texture7, i.texcoord_Texture7 / _GlobalScale), _7SpecularContrast, _7SpecularBrightness); 
					resultSpecularMap = GetResultSpecularMap(objectTexture7spec, _SpecularIntensity7, _SpecularTint7, paintMask1, paintMask2, _7Paint1Specular, _7Paint2Specular, objectLabelAlpha, _LabelSpecularIntensity, wornEdges, _7UnderlyingSpecular, _7UnderlyingSpecularTint, dirts1Mask, dirts2Mask, _specMin, _specMax, _7GlobalTransparency);	
				}
			

     			
   				return fixed4 (resultSpecularMap.r, resultSpecularMap.g, resultSpecularMap.b, 1) * objectMasks.a;
  			}
  			ENDCG
 		}
	} 
 	FallBack "Diffuse"
}