// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/ExportNormals" {
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
		_BumpMap0("0 Normal Map", 2D) = "bump" {}
		_BumpMapStrength0 ("0 Normal Map Strength", Range (-2.0, 2)) = 0.0
		
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
		_BumpMap1("1 Normal Map", 2D) = "bump" {}
		_BumpMapStrength1 ("1 Normal Map Strength", Range (-2.0, 2)) = 0.0
		
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
		_BumpMap2("2 Normal Map", 2D) = "bump" {}
		_BumpMapStrength2 ("2 Normal Map Strength", Range (-2.0, 2)) = 0.0
		
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
		_BumpMap3("3 Normal Map", 2D) = "bump" {}
		_BumpMapStrength3 ("3 Normal Map Strength", Range (-2.0, 2)) = 0.0
		
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
		_BumpMap4("4 Normal Map", 2D) = "bump" {}
		_BumpMapStrength4 ("4 Normal Map Strength", Range (-2.0, 2)) = 0.0
		
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
		_BumpMap5("5 Normal Map", 2D) = "bump" {}
		_BumpMapStrength5 ("5 Normal Map Strength", Range (-2.0, 2)) = 0.0
		
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
		_BumpMap6("6 Normal Map", 2D) = "bump" {}
		_BumpMapStrength6 ("6 Normal Map Strength", Range (-2.0, 2)) = 0.0
		
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
		_BumpMap7("7 Normal Map", 2D) = "bump" {}
		_BumpMapStrength7 ("7 Normal Map Strength", Range (-2.0, 2)) = 0.0
		
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
   				
   				half2 texcoord_Texture0 : TEXCOORD1;
   				half2 texcoord_BumpMap0 : TEXCOORD2;
  			};
   			
   			float _0MaterialRotation;
   			
   			float4 _NormalMap_ST;
			float4 _Texture0_ST;
			float4 _BumpMap0_ST;


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
   				
   				return o; 
  			}
    
      		  		
    		sampler2D _Texture0;
    		sampler2D _BumpMap0;
    		float _NormalsStrength0;
    		float _BumpMapStrength0;	
		

  			fixed4 frag (v2f i) : COLOR{
  				
  				fixed4 objectMasks = tex2D(_ObjectMasks, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw); 
  	
  				//Normal detailing
				float me = tex2D(_Texture0, i.texcoord_Texture0 / _GlobalScale ).x;
				float n = tex2D(_Texture0,float2(i.texcoord_Texture0.x / _GlobalScale, i.texcoord_Texture0.y+1.0/2048 / _GlobalScale)).x;
				float s = tex2D(_Texture0,float2(i.texcoord_Texture0.x / _GlobalScale, i.texcoord_Texture0.y-1.0/2048 / _GlobalScale)).x;
				float e = tex2D(_Texture0,float2(i.texcoord_Texture0.x-1.0/2048 / _GlobalScale, i.texcoord_Texture0.y / _GlobalScale)).x;
				float w = tex2D(_Texture0,float2(i.texcoord_Texture0.x+1.0/2048 / _GlobalScale, i.texcoord_Texture0.y / _GlobalScale)).x;
			
				float3 finalNormal = GetFinalNormal(tex2D(_NormalMap, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw), me, n, s, e, w, objectMasks.r, _NormalsStrength0, tex2D(_BumpMap0, i.texcoord_BumpMap0 / _GlobalScale), _BumpMapStrength0, _0MaterialRotation);
  			
				fixed4 resultColor = fixed4(finalNormal.r, finalNormal.g, finalNormal.b, 1);
 			
   				return resultColor;
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
   				
   				half2 texcoord_Texture1 : TEXCOORD1;
   				half2 texcoord_BumpMap1 : TEXCOORD2;
  			};
   			
   			float _1MaterialRotation;
   			
   			float4 _NormalMap_ST;
			float4 _Texture1_ST;
			float4 _BumpMap1_ST;


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
   				
   				return o; 
  			}
    
      		  		
    		sampler2D _Texture1;
    		sampler2D _BumpMap1;
    		float _NormalsStrength1;
    		float _BumpMapStrength1;	
		

  			fixed4 frag (v2f i) : COLOR{
  				
  				fixed4 objectMasks = tex2D(_ObjectMasks, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw); 
  	
  				//Normal detailing
				float me = tex2D(_Texture1, i.texcoord_Texture1 / _GlobalScale ).x;
				float n = tex2D(_Texture1,float2(i.texcoord_Texture1.x / _GlobalScale, i.texcoord_Texture1.y+1.0/2048 / _GlobalScale)).x;
				float s = tex2D(_Texture1,float2(i.texcoord_Texture1.x / _GlobalScale, i.texcoord_Texture1.y-1.0/2048 / _GlobalScale)).x;
				float e = tex2D(_Texture1,float2(i.texcoord_Texture1.x-1.0/2048 / _GlobalScale, i.texcoord_Texture1.y / _GlobalScale)).x;
				float w = tex2D(_Texture1,float2(i.texcoord_Texture1.x+1.0/2048 / _GlobalScale, i.texcoord_Texture1.y / _GlobalScale)).x;
			
				float3 finalNormal = GetFinalNormal(tex2D(_NormalMap, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw), me, n, s, e, w, objectMasks.g, _NormalsStrength1, tex2D(_BumpMap1, i.texcoord_BumpMap1 / _GlobalScale ), _BumpMapStrength1, _1MaterialRotation);
  			
				fixed4 resultColor = fixed4(finalNormal.r, finalNormal.g, finalNormal.b, 1);
 			
   				return resultColor * objectMasks.g;
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
   				
   				half2 texcoord_Texture2 : TEXCOORD1;
   				half2 texcoord_BumpMap2 : TEXCOORD2;
  			};
   			
   			float _2MaterialRotation;
   			
   			float4 _NormalMap_ST;
			float4 _Texture2_ST;
			float4 _BumpMap2_ST;


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
   				
   				return o; 
  			}
    
      		  		
    		sampler2D _Texture2;
    		sampler2D _BumpMap2;
    		float _NormalsStrength2;
    		float _BumpMapStrength2;
		

  			fixed4 frag (v2f i) : COLOR{
  				
  				fixed4 objectMasks = tex2D(_ObjectMasks, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw); 
  	
  				//Normal detailing
				float me = tex2D(_Texture2, i.texcoord_Texture2 / _GlobalScale ).x;
				float n = tex2D(_Texture2,float2(i.texcoord_Texture2.x / _GlobalScale, i.texcoord_Texture2.y+1.0/2048 / _GlobalScale)).x;
				float s = tex2D(_Texture2,float2(i.texcoord_Texture2.x / _GlobalScale, i.texcoord_Texture2.y-1.0/2048 / _GlobalScale)).x;
				float e = tex2D(_Texture2,float2(i.texcoord_Texture2.x-1.0/2048 / _GlobalScale, i.texcoord_Texture2.y / _GlobalScale)).x;
				float w = tex2D(_Texture2,float2(i.texcoord_Texture2.x+1.0/2048 / _GlobalScale, i.texcoord_Texture2.y / _GlobalScale)).x;
				
				float3 finalNormal = GetFinalNormal(tex2D(_NormalMap, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw), me, n, s, e, w, objectMasks.b, _NormalsStrength2, tex2D(_BumpMap2, i.texcoord_BumpMap2 / _GlobalScale ), _BumpMapStrength2, _2MaterialRotation);
  			
				fixed4 resultColor = fixed4(finalNormal.r, finalNormal.g, finalNormal.b, 1);
 			
   				return resultColor * objectMasks.b;
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
   				
   				half2 texcoord_Texture3 : TEXCOORD1;
   				half2 texcoord_BumpMap3 : TEXCOORD2;
  			};
   			
   			float _3MaterialRotation;
   			
   			float4 _NormalMap_ST;
			float4 _Texture3_ST;
			float4 _BumpMap3_ST;


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
   				
   				return o; 
  			}
    
      		  		
    		sampler2D _Texture3;
    		sampler2D _BumpMap3;
    		float _NormalsStrength3;
    		float _BumpMapStrength3;	
		

  			fixed4 frag (v2f i) : COLOR{
  				
  				fixed4 objectMasks = tex2D(_ObjectMasks, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw); 
  	
  				//Normal detailing
				float me = tex2D(_Texture3, i.texcoord_Texture3 / _GlobalScale ).x;
				float n = tex2D(_Texture3,float2(i.texcoord_Texture3.x / _GlobalScale, i.texcoord_Texture3.y+1.0/2048 / _GlobalScale)).x;
				float s = tex2D(_Texture3,float2(i.texcoord_Texture3.x / _GlobalScale, i.texcoord_Texture3.y-1.0/2048 / _GlobalScale)).x;
				float e = tex2D(_Texture3,float2(i.texcoord_Texture3.x-1.0/2048 / _GlobalScale, i.texcoord_Texture3.y / _GlobalScale)).x;
				float w = tex2D(_Texture3,float2(i.texcoord_Texture3.x+1.0/2048 / _GlobalScale, i.texcoord_Texture3.y / _GlobalScale)).x;
			
				float3 finalNormal = GetFinalNormal(tex2D(_NormalMap, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw), me, n, s, e, w, objectMasks.a, _NormalsStrength3, tex2D(_BumpMap3, i.texcoord_BumpMap3 / _GlobalScale ), _BumpMapStrength3, _3MaterialRotation);
  			
				fixed4 resultColor = fixed4(finalNormal.r, finalNormal.g, finalNormal.b, 1);
 			
   				return resultColor * objectMasks.a;
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
   				
   				half2 texcoord_Texture4 : TEXCOORD1;
   				half2 texcoord_BumpMap4 : TEXCOORD2;
  			};
   			
   			float _4MaterialRotation;
   			
   			float4 _NormalMap_ST;
			float4 _Texture4_ST;
			float4 _BumpMap4_ST;


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
   				
   				return o; 
  			}
    
      		  		
    		sampler2D _Texture4;
    		sampler2D _BumpMap4;
    		float _NormalsStrength4;
    		float _BumpMapStrength4;	
		

  			fixed4 frag (v2f i) : COLOR{
  				
  				fixed4 objectMasks = tex2D(_ObjectMasks2, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw); 
  	
  				//Normal detailing
				float me = tex2D(_Texture4, i.texcoord_Texture4 / _GlobalScale ).x;
				float n = tex2D(_Texture4,float2(i.texcoord_Texture4.x / _GlobalScale, i.texcoord_Texture4.y+1.0/2048 / _GlobalScale)).x;
				float s = tex2D(_Texture4,float2(i.texcoord_Texture4.x / _GlobalScale, i.texcoord_Texture4.y-1.0/2048 / _GlobalScale)).x;
				float e = tex2D(_Texture4,float2(i.texcoord_Texture4.x-1.0/2048 / _GlobalScale, i.texcoord_Texture4.y / _GlobalScale)).x;
				float w = tex2D(_Texture4,float2(i.texcoord_Texture4.x+1.0/2048 / _GlobalScale, i.texcoord_Texture4.y / _GlobalScale)).x;
			
				float3 finalNormal = GetFinalNormal(tex2D(_NormalMap, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw), me, n, s, e, w, objectMasks.r, _NormalsStrength4, tex2D(_BumpMap4, i.texcoord_BumpMap4 / _GlobalScale ), _BumpMapStrength4, _4MaterialRotation);
  			
				fixed4 resultColor = fixed4(finalNormal.r, finalNormal.g, finalNormal.b, 1);
 			
   				return resultColor * objectMasks.r;
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
   				
   				half2 texcoord_Texture5 : TEXCOORD1;
   				half2 texcoord_BumpMap5 : TEXCOORD2;
  			};
   			
   			float _5MaterialRotation;
   			
   			float4 _NormalMap_ST;
			float4 _Texture5_ST;
			float4 _BumpMap5_ST;


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
   				
   				return o; 
  			}
    
      		  		
    		sampler2D _Texture5;
    		sampler2D _BumpMap5;
    		float _NormalsStrength5;
    		float _BumpMapStrength5;	
		

  			fixed4 frag (v2f i) : COLOR{
  				
  				fixed4 objectMasks = tex2D(_ObjectMasks2, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw); 
  	
  				//Normal detailing
				float me = tex2D(_Texture5, i.texcoord_Texture5 / _GlobalScale ).x;
				float n = tex2D(_Texture5,float2(i.texcoord_Texture5.x / _GlobalScale, i.texcoord_Texture5.y+1.0/2048 / _GlobalScale)).x;
				float s = tex2D(_Texture5,float2(i.texcoord_Texture5.x / _GlobalScale, i.texcoord_Texture5.y-1.0/2048 / _GlobalScale)).x;
				float e = tex2D(_Texture5,float2(i.texcoord_Texture5.x-1.0/2048 / _GlobalScale, i.texcoord_Texture5.y / _GlobalScale)).x;
				float w = tex2D(_Texture5,float2(i.texcoord_Texture5.x+1.0/2048 / _GlobalScale, i.texcoord_Texture5.y / _GlobalScale)).x;
			
				float3 finalNormal = GetFinalNormal(tex2D(_NormalMap, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw), me, n, s, e, w, objectMasks.g, _NormalsStrength5, tex2D(_BumpMap5, i.texcoord_BumpMap5 / _GlobalScale ), _BumpMapStrength5, _5MaterialRotation);
  			
				fixed4 resultColor = fixed4(finalNormal.r, finalNormal.g, finalNormal.b, 1);
 			
   				return resultColor * objectMasks.g;
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
   				
   				half2 texcoord_Texture6 : TEXCOORD1;
   				half2 texcoord_BumpMap6 : TEXCOORD2;
  			};
   			
   			float _6MaterialRotation;
   			
   			float4 _NormalMap_ST;
			float4 _Texture6_ST;
			float4 _BumpMap6_ST;


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
   				
   				return o; 
  			}
    
      		  		
    		sampler2D _Texture6;
    		sampler2D _BumpMap6;
    		float _NormalsStrength6;
    		float _BumpMapStrength6;	
		

  			fixed4 frag (v2f i) : COLOR{
  				
  				fixed4 objectMasks = tex2D(_ObjectMasks2, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw); 
  	
  				//Normal detailing
				float me = tex2D(_Texture6, i.texcoord_Texture6 / _GlobalScale ).x;
				float n = tex2D(_Texture6,float2(i.texcoord_Texture6.x / _GlobalScale, i.texcoord_Texture6.y+1.0/2048 / _GlobalScale)).x;
				float s = tex2D(_Texture6,float2(i.texcoord_Texture6.x / _GlobalScale, i.texcoord_Texture6.y-1.0/2048 / _GlobalScale)).x;
				float e = tex2D(_Texture6,float2(i.texcoord_Texture6.x-1.0/2048 / _GlobalScale, i.texcoord_Texture6.y / _GlobalScale)).x;
				float w = tex2D(_Texture6,float2(i.texcoord_Texture6.x+1.0/2048 / _GlobalScale, i.texcoord_Texture6.y / _GlobalScale)).x;
			
				float3 finalNormal = GetFinalNormal(tex2D(_NormalMap, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw), me, n, s, e, w, objectMasks.b, _NormalsStrength6, tex2D(_BumpMap6, i.texcoord_BumpMap6 / _GlobalScale ), _BumpMapStrength6, _6MaterialRotation);
  			
				fixed4 resultColor = fixed4(finalNormal.r, finalNormal.g, finalNormal.b, 1);
 			
   				return resultColor * objectMasks.b;
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
   				
   				half2 texcoord_Texture7 : TEXCOORD1;
   				half2 texcoord_BumpMap7 : TEXCOORD2;
  			};
   			
   			float _7MaterialRotation;
   			
   			float4 _NormalMap_ST;
			float4 _Texture7_ST;
			float4 _BumpMap7_ST;


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
   				
   				return o; 
  			}
    
      		  		
    		sampler2D _Texture7;
    		sampler2D _BumpMap7;
    		float _NormalsStrength7;
    		float _BumpMapStrength7;	
		

  			fixed4 frag (v2f i) : COLOR{
  				
  				fixed4 objectMasks = tex2D(_ObjectMasks2, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw); 
  	
  				//Normal detailing
				float me = tex2D(_Texture7, i.texcoord_Texture7 / _GlobalScale ).x;
				float n = tex2D(_Texture7,float2(i.texcoord_Texture7.x / _GlobalScale, i.texcoord_Texture7.y+1.0/2048 / _GlobalScale)).x;
				float s = tex2D(_Texture7,float2(i.texcoord_Texture7.x / _GlobalScale, i.texcoord_Texture7.y-1.0/2048 / _GlobalScale)).x;
				float e = tex2D(_Texture7,float2(i.texcoord_Texture7.x-1.0/2048 / _GlobalScale, i.texcoord_Texture7.y / _GlobalScale)).x;
				float w = tex2D(_Texture7,float2(i.texcoord_Texture7.x+1.0/2048 / _GlobalScale, i.texcoord_Texture7.y / _GlobalScale)).x;
			
				float3 finalNormal = GetFinalNormal(tex2D(_NormalMap, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw), me, n, s, e, w, objectMasks.a, _NormalsStrength7, tex2D(_BumpMap7, i.texcoord_BumpMap7 / _GlobalScale ), _BumpMapStrength7, _7MaterialRotation);
  			
				fixed4 resultColor = fixed4(finalNormal.r, finalNormal.g, finalNormal.b, 1);
 			
   				return resultColor * objectMasks.a;
  			}
  			ENDCG
 		}
 		
	} 
 	FallBack "Diffuse"
}