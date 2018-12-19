sampler2D _NormalMap;	
sampler2D _AoEdgesDirtDepth;
sampler2D _ObjectMasks;
sampler2D _ObjectMasks2;
			
fixed4 _DirtNoise1Mix;
half _Dirt1Amount;
half _Dirt1Contrast;
half _Dirt1Opacity;
fixed4 _Dirt1Tint;
		
fixed4 _DirtNoise2Mix;
half _Dirt2Amount;
half _Dirt2Contrast;
half _Dirt2Opacity;
fixed4 _Dirt2Tint;
		
fixed4 _LabelTint;
sampler2D _LabelTexture; 
half _LabelSpecularIntensity;
half _LabelGlossiness;
		
fixed4 _0EmissionTint;
half _0EmissionIntensity;
fixed4 _1EmissionTint;
half _1EmissionIntensity;
sampler2D _EmissionMask;

half _specMin;
half _specMax;
		
half _glossMin;
half _glossMax;

half _gamma;
half _minInput;
half _maxInput;
half _minOutput;
half _maxOutput;

float _Hue;
float _Saturation; 
float _Brightness;
float _Contrast;

float _GlobalScale;

int _LinearColorSpace;

float _ShowMaps;

#define GammaCorrection(color, gamma)								pow(color, 1.0 / gamma)
#define LevelsControlInputRange(color, minInput, maxInput)				min(max(color - float3(minInput, minInput, minInput), float3(0, 0, 0)) / (float3(maxInput, maxInput, maxInput) - float3(minInput, minInput, minInput)), float3(1,1,1))
#define LevelsControlInput(color, minInput, gamma, maxInput)				GammaCorrection(LevelsControlInputRange(color, minInput, maxInput), gamma)
#define LevelsControlOutputRange(color, minOutput, maxOutput) 			lerp(float3(minOutput,minOutput,minOutput), float3(maxOutput,maxOutput,maxOutput), color)
#define LevelsControl(color, minInput, gamma, maxInput, minOutput, maxOutput) 	LevelsControlOutputRange(LevelsControlInput(color, minInput, gamma, maxInput), minOutput, maxOutput)

inline fixed4 GetSpecMap(fixed4 tex, float specularContrast, float specularBrightness) {
	float shift = 0;
	
	if (_LinearColorSpace != 0) {
		specularBrightness = pow (specularBrightness, 0.454545);
		shift = specularBrightness - pow (0.5, 0.454545);
		
		if (specularContrast > 0.5) {
			specularContrast =  1 - specularContrast;
			tex = pow(1 - pow(tex, 0.454545), 2.2);
		}
		specularContrast = pow (specularContrast, 2.2);
	}
	else {
		shift = specularBrightness - 0.5; 
		if (specularContrast > 0.5) {
			specularContrast =  1 - specularContrast;
			tex = 1-tex;
		}
	}
	
	float3 spec = LevelsControl(float3 (tex.r, tex.g, tex.b), 0+specularContrast+shift, 1.0, 1-specularContrast+shift, 0, 1);
	return fixed4(spec.r, spec.g, spec.b, 1);
}


inline fixed GetGlossMap(fixed tex, float glossinessContrast, float glossinessBrightness) {
	float shift = glossinessBrightness - 0.5;
	if (glossinessContrast > 0.5) {
		glossinessContrast =  1 - glossinessContrast;
		tex = 1-tex;
	}
	float3 gloss = LevelsControl(float3 (tex, tex, tex), 0+glossinessContrast+shift, 1.0, 1-glossinessContrast+shift, 0, 1);
	return fixed4(gloss.r, gloss.g, gloss.b, 1);
}

inline fixed GetEdgeNoise(fixed4 rgbaNoise, fixed4 wornEdgesNoiseMix) {
	return rgbaNoise.r * wornEdgesNoiseMix.r + rgbaNoise.g * wornEdgesNoiseMix.g + rgbaNoise.b * wornEdgesNoiseMix.b + rgbaNoise.a * wornEdgesNoiseMix.a;
}

inline fixed GetWornEdges(fixed ao, fixed edgeMask, fixed edgeNoise, half wornEdgesContrast, half wornEdgesAmount, half wornEdgesOpacity) {
	return clamp(pow (edgeMask * wornEdgesAmount * edgeNoise * pow (ao, 8), wornEdgesContrast), wornEdgesOpacity, 1.0) - wornEdgesOpacity;
}

inline fixed GetWornEdgesCenter(fixed ao, fixed edgeMask, fixed edgeNoise, half wornEdgesContrast, half wornEdgesAmount, half wornEdgesOpacity, half wornEdgesBorder) {
	fixed result = edgeMask * wornEdgesAmount - wornEdgesBorder; 
    if (result < 0) result = 0;
    return clamp (pow (result * edgeNoise * pow (ao, 8), wornEdgesContrast), wornEdgesOpacity, 1.0) - wornEdgesOpacity;
}

inline fixed4 GetPMask(fixed4 basePaintMask, fixed objectMask, half paintIntensity, fixed maskNoiseBorder) {
	return 1 - (basePaintMask * objectMask * paintIntensity * (1- maskNoiseBorder)); 
}

inline fixed GetMaskNoiseBorder(fixed4 rgbaNoise, fixed4 paintNoiseMix) {
	return clamp(rgbaNoise.r * paintNoiseMix.r + rgbaNoise.g * paintNoiseMix.g + rgbaNoise.b * paintNoiseMix.b + rgbaNoise.a * paintNoiseMix.a, 0, 1); 
}

inline fixed4 GetPaintCol(fixed4 pMask, fixed4 basePaint, fixed4 paintColor) {
	return (1 - pMask) * basePaint * paintColor * paintColor.a;
}

inline fixed GetBaseDirt(fixed dirtMask) {
	return clamp(dirtMask + 0.15, 0, 1.0);	
} 

inline fixed GetDirtNoise(fixed4 rgbaNoise, fixed4 dirtNoiseMix) {
	return rgbaNoise.r * dirtNoiseMix.r + rgbaNoise.g * dirtNoiseMix.g + rgbaNoise.b * dirtNoiseMix.b + rgbaNoise.a * dirtNoiseMix.a;
}

inline fixed GetDirtMask(fixed dirtNoise, fixed dirtMask, half dirtAmount, half dirtContrast, half dirtOpacity) {
	return clamp(pow((1.05  - dirtMask) * dirtNoise * dirtAmount, dirtContrast), 0, 1.0) * dirtOpacity;	
}

inline fixed4 GetDirt(fixed dirtsMask, fixed4 dirtTint) {
	return 1 * dirtsMask * dirtTint; 
} 

inline fixed4 GetResultSpecularMap(fixed objectTextureSpec, half specularIntensity, fixed4 specularTint, fixed paintMask1, fixed paintMask2, half paint1Specular, half paint2Specular, fixed4 objectLabelAlpha, half labelSpecularIntensity, fixed wornEdges, half underlyingSpecular, fixed4 underlyingSpecularTint, fixed dirts1Mask, fixed dirts2Mask, half specMin, half specMax, half globalTransparency) {
	//specularTint = pow (specularTint, 0.454545); //Specular tint as sRGB value, like in Unity standard shader
	
	if (_LinearColorSpace != 0) {
		paint1Specular = pow (paint1Specular, 2.2);
		paint2Specular = pow (paint2Specular, 2.2);
		
		labelSpecularIntensity = pow (labelSpecularIntensity, 2.2);
		
	}
	
	fixed specDetail = objectTextureSpec * specularIntensity + 0.5 * (1-specularIntensity);
	float twiceLuminance =  dot(specDetail, fixed4(0.2126, 0.7152, 0.0722, 0)) * 2;
	
	fixed4 spec = 0;
	if (twiceLuminance < 1) spec = lerp(fixed4(0, 0, 0, 0), specularTint, twiceLuminance);
	else spec = lerp(specularTint, fixed4(1, 1, 1, 1), twiceLuminance - 1);
    
	float3 result = LevelsControl(clamp (((((((spec ) * specularTint ) * paintMask1 + (1-paintMask1) * paint1Specular) * paintMask2 + (1-paintMask2) * paint2Specular ) * (1-objectLabelAlpha) + labelSpecularIntensity * objectLabelAlpha ) * (1-wornEdges)  + (spec + 1) * wornEdges * underlyingSpecular * underlyingSpecularTint) * ( 1 - dirts1Mask) * ( 1 - dirts2Mask), 0, 1.0), 0,1,1, specMin, specMax);
	return clamp(fixed4 (result.r, result.g, result.b, 1) * (1 - globalTransparency * (1-objectLabelAlpha)), 0, 1); 
} 

inline fixed4 GetResultSpecularMapRGB(fixed4 objectTextureSpec, half specularIntensity, fixed4 specularTint, fixed paintMask1, fixed paintMask2, half paint1Specular, half paint2Specular, fixed4 objectLabelAlpha, half labelSpecularIntensity, fixed wornEdges, half underlyingSpecular, fixed4 underlyingSpecularTint, fixed dirts1Mask, fixed dirts2Mask, half specMin, half specMax, half globalTransparency) {
	
	if (_LinearColorSpace != 0) {
		paint1Specular = pow (paint1Specular, 2.2);
		paint2Specular = pow (paint2Specular, 2.2);
		
		labelSpecularIntensity = pow (labelSpecularIntensity, 2.2);
		
	}
    
    fixed4 spec = objectTextureSpec * specularIntensity + 0.5 * (1-specularIntensity);
    
	float3 result = LevelsControl(clamp (((((((spec ) * specularTint ) * paintMask1 + (1-paintMask1) * paint1Specular) * paintMask2 + (1-paintMask2) * paint2Specular ) * (1-objectLabelAlpha) + labelSpecularIntensity * objectLabelAlpha ) * (1-wornEdges)  + (spec + 1) * wornEdges * underlyingSpecular * underlyingSpecularTint) * ( 1 - dirts1Mask) * ( 1 - dirts2Mask), 0, 1.0), 0,1,1, specMin, specMax);
	return clamp(fixed4 (result.r, result.g, result.b, 1) * (1 - globalTransparency * (1-objectLabelAlpha)), 0, 1); 
}


inline fixed GetResultGlossinessMap(fixed objectTextureGloss, half glossinessIntensity, half glossiness, fixed paintMask1, fixed paintMask2, half paint1Glossiness, half paint2Glossiness, fixed4 objectLabelAlpha, half labelGlossiness, fixed wornEdges, half underlyingGlossiness, half glossMin, half glossMax, half globalTransparency) {
	
	fixed4 glossinessTint = fixed4(glossiness.r, glossiness.r, glossiness.r, 1);
	fixed glossDetail = objectTextureGloss * glossinessIntensity + 0.5 * (1-glossinessIntensity);
	float twiceLuminance =  dot(glossDetail, fixed4(0.2126, 0.7152, 0.0722, 0)) * 2;
	
	fixed4 gloss = 0;
	if (twiceLuminance < 1) gloss = lerp(fixed4(0, 0, 0, 0), glossinessTint, twiceLuminance);
	else gloss = lerp(glossinessTint, fixed4(1, 1, 1, 1), twiceLuminance - 1);
	
	float3 result = LevelsControl(clamp((((((gloss ) * glossiness) * paintMask1 + (1-paintMask1) * paint1Glossiness) * paintMask2 + (1-paintMask2) * paint2Glossiness )  * (1-objectLabelAlpha) + labelGlossiness * objectLabelAlpha ) * (1-wornEdges) + (gloss + 1) * wornEdges * underlyingGlossiness, 0, 1.0), 0,1,1, glossMin, glossMax);
		
	if (globalTransparency == 1) {
		result = clamp(result - (1-objectLabelAlpha), 0, 1);
	}
	
	return fixed (result.r);  
}

inline fixed GetResultGlossinessMapFromAlpha(fixed objectTextureGloss, half glossinessIntensity, half glossiness, fixed paintMask1, fixed paintMask2, half paint1Glossiness, half paint2Glossiness, fixed4 objectLabelAlpha, half labelGlossiness, fixed wornEdges, half underlyingGlossiness, half glossMin, half glossMax, half globalTransparency) {
	
	fixed4 glossinessTint = fixed4(glossiness, glossiness, glossiness, 1);
	
	fixed4 gloss = lerp(glossinessTint, objectTextureGloss, glossinessIntensity);
		
	float3 result = LevelsControl(clamp((((((gloss ) * 1) * paintMask1 + (1-paintMask1) * paint1Glossiness) * paintMask2 + (1-paintMask2) * paint2Glossiness )  * (1-objectLabelAlpha) + labelGlossiness * objectLabelAlpha ) * (1-wornEdges) + (gloss + 1) * wornEdges * underlyingGlossiness, 0, 1.0), 0,1,1, glossMin, glossMax);

	if (globalTransparency == 1) {
		result = clamp(result - (1-objectLabelAlpha), 0, 1);
	}
	
	return fixed (result.r);  
}

inline fixed GetResultAoMap(fixed aochannel) {
	fixed resultAo = aochannel;
	if (_LinearColorSpace != 0) {
		resultAo = pow (aochannel, 2.2);
	}
	return clamp (resultAo, 0, 1.0);
}


inline fixed4 GetDiffuse(fixed4 objectTexture, fixed4 tint, fixed paintMask1, fixed paintMask2, fixed4 paint1Color, fixed4 paint2Color, fixed4 paintCol1, fixed4 paintCol2, fixed4 objectLabelAlpha, fixed4 objectLabel, fixed dirts1Mask, fixed dirts2Mask, fixed4 dirts1, fixed4 dirts2, fixed4 underlyingDiffuseTint, fixed wornEdges, fixed4 wornEdgesBorderTint, half underlyingDiffuse, fixed wornEdgesCenter) {
	fixed4 coloredTexture = objectTexture * tint;
	fixed4 	mainColor = ((((coloredTexture * paintMask1 + coloredTexture * (1 - paintMask1) * (1-paint1Color.a) + paintCol1) * paintMask2 + (coloredTexture * paintMask2 + coloredTexture * (1 - paintMask2) * (1-paint2Color.a) + paintCol2) * (1-paintMask2)) * (1-objectLabelAlpha) + objectLabel * objectLabelAlpha) * (1-dirts1Mask) + dirts1) * (1-dirts2Mask) + dirts2;	
	fixed4 wornEdgeColor = 1 * underlyingDiffuseTint;
	return (mainColor * (1 - wornEdges) + mainColor * wornEdgesBorderTint * (wornEdges - wornEdgesCenter)  + wornEdgeColor * wornEdgesCenter) * underlyingDiffuse + mainColor * (1 - underlyingDiffuse);
}	  


inline fixed4 GetEmission(fixed4 emissionMask, fixed4 emissionTint0, fixed4 emissionTint1, half emissionIntensity0, half emissionIntensity1, fixed4 emissionMap, fixed4 emissionMapTint, half emissionMapIntensity) {
	return clamp(emissionMask.r * emissionTint0 * emissionIntensity0 + emissionMask.g * emissionTint1 * emissionIntensity1 + emissionMap * emissionMapTint * emissionMapIntensity * clamp((1 - emissionMask.r - emissionMask.g), 0, 1), 0, 1);							  
} 
 

inline float3 applyHue(float3 aColor, float aHue) {
	float angle = radians(aHue);
    float3 k = float3(0.57735, 0.57735, 0.57735);
    float cosAngle = cos(angle);
    return aColor * cosAngle + cross(k, aColor) * sin(angle) + k * dot(k, aColor) * (1 - cosAngle);
}
 
 
inline float4 applyHSBEffect(float4 startColor, fixed4 hsbc) {
	float _Hue = 360 * hsbc.r;
	float _Brightness = hsbc.g * 2 - 1;
    float _Contrast = hsbc.b * 2;
    float _Saturation = hsbc.a * 2;
 
    float4 outputColor = startColor;
   	outputColor.rgb = applyHue(outputColor.rgb, _Hue);
    outputColor.rgb = (outputColor.rgb - 0.5f) * (_Contrast) + 0.5f;
    outputColor.rgb = outputColor.rgb + _Brightness;      
    float3 intensity = dot(outputColor.rgb, float3(0.299,0.587,0.114));
    outputColor.rgb = lerp(intensity, outputColor.rgb, _Saturation);
 
    return outputColor;
}

inline float3 GetFinalNormal(half4 normalMap, float me, float n, float s, float e, float w, fixed objectMask, float normalsStrength, half4 materialBumpMap, float bumpMapStrength, float normalsRotation) { 
			
			//UDN blending
			//float3 n1 = normalize(normalMap.xyz - 0.5);
			//float3 n2 = UnpackNormal (materialBumpMap).xyz;
			//float3 r = normalize(float3(n1.xy + n2.xy, n1.z));
			//float3 result = (r + 1) * 0.5;
			//return result;
			
			
			//RNM blending
			float3 n1 = normalize(normalMap.xyz - 0.5);
			float3 n2u = UnpackNormal (materialBumpMap).xyz;
			
			
			//rotate normals
			float rad = radians(normalsRotation);
			float rot_s = sin (rad);
			float rot_c = cos (rad);
			rot_s*=-1;
			float2x2 rotationMatrix = float2x2( rot_c, -rot_s, rot_s, rot_c);
			n2u = float3( mul (n2u.xy, rotationMatrix),1.0f);
			
			
			
			if (bumpMapStrength == 0) {
				bumpMapStrength = 0.00001;
			}
			
			n2u.z = n2u.z / abs(bumpMapStrength);
			n2u = normalize(n2u);
			
			
			float3 n2 = float3(n2u.x, n2u.y, n2u.z );
			if (bumpMapStrength < 0) {
				n2 = float3(n2u.x * -1, n2u.y * -1, n2u.z );
			}
			
			float a = 1/(1 + n1.z);
			float b = -n1.x*n1.y*a;

			float3 b1 = float3(1 - n1.x*n1.x*a, b, -n1.x);
			float3 b2 = float3(b, 1 - n1.y*n1.y*a, -n1.y);
			float3 b3 = n1;

			if (n1.z < -0.9999999) {
    			b1 = float3( 0, -1, 0);
   				b2 = float3(-1,  0, 0);
			}

			float3 r = n2.x*b1 + n2.y*b2 + n2.z*b3;

			return r*0.5 + 0.5;
			
			
			
			//Microdetail extraction from albedo
			//float3 normal = UnpackNormal(normalMap);

			//float3 norm = normal;
			//float3 temp = norm; 
			//if(norm.x==1)
			//	temp.y+=0.5;
			//else
			//	temp.x+=0.5;

			//float3 perp1 = normalize(cross(norm,temp));
			//float3 perp2 = normalize(cross(norm,perp1));

			//float3 normalOffset = -normalsStrength * 0.1 * objectMask * ( ( (n-me) - (s-me) ) * perp1 + ( ( e - me ) - ( w - me ) ) * perp2);

			//return float3(normalMap.r + normalOffset.r, normalMap.g + normalOffset.g, normalMap.b + normalOffset.b);
} 

inline fixed GetResultTransparency(half globalTransparency, half albedoTransparency, half paint1Transparency, half paint2Transparency, fixed paintMask1, fixed paintMask2, fixed4 objectLabelAlpha) {
	fixed transp = clamp(clamp ((1 - (1-paintMask1) - (1-paintMask2)), 0, 1) * albedoTransparency + (1-paintMask1) * paint1Transparency + (1-paintMask2) * paint2Transparency, 0, 1);
	fixed result = 1 - (1 * max(globalTransparency, transp) ) * (1-objectLabelAlpha);
	return clamp (result, 0, 1);
}

inline fixed GetMaterialAO(fixed materialAo, float occlusionMapStrength) {
	return clamp(pow(materialAo, occlusionMapStrength), 0, 1);
}

inline float GetScreenPart(float2 screenUV, float part, float tilt, int num) {
	float screenRatio = _ScreenParams.x / _ScreenParams.y;
	return  (1 - 1/screenRatio) * 0.5 + part / screenRatio * num - tilt * screenUV.y / screenRatio + tilt * 0.5 / screenRatio;
}




