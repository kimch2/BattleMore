// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/ExportHeightmap" {
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

		_LinearColorSpace ("LinearColorSpace", Int) = 0
	}
 
	SubShader {
 
		ZTest Always Cull Off ZWrite Off Fog { Mode Off } 
 
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
   
  			};
   

  			v2f vert (appdata_img v){
   				v2f o;
  				o.pos = UnityObjectToClipPos (v.vertex);
   				o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
   				return o; 
  			}  
  		
			float4 _NormalMap_ST;


  			fixed4 frag (v2f i) : COLOR{
  			
				fixed heightmap = tex2D(_AoEdgesDirtDepth, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).a;
				fixed4 resultColor = fixed4(heightmap, heightmap, heightmap, 1);

   				return resultColor;
  			}
  			ENDCG
 		}
	} 
 	FallBack "Diffuse"
}