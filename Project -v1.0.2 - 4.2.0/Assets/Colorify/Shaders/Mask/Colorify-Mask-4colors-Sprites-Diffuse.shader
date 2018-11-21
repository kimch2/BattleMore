Shader "Colorify/Mask(baked)/4 colors/Sprites/Diffuse"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		_ColorifyMaskTex ("Colorify mask (RGB)", 2D) = "black" {}		
		_NewColor ("New Color", Color) = (1,1,1,1)
		_NewColor2 ("New Color 2", Color) = (1,1,1,1)	
		_NewColor3 ("New Color 3", Color) = (1,1,1,1)
		_NewColor4 ("New Color 4", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		CGPROGRAM
		#pragma surface surf Lambert vertex:vert nofog keepalpha
		#pragma multi_compile _ PIXELSNAP_ON

		sampler2D _MainTex;
		sampler2D _ColorifyMaskTex; 
		fixed4 _Color;				
		fixed4 _NewColor;
		fixed4 _NewColor2;
		fixed4 _NewColor3;
		fixed4 _NewColor4;

		struct Input
		{
			float2 uv_MainTex;
			fixed4 color;
		};
		
		void vert (inout appdata_full v, out Input o)
		{
			#if defined(PIXELSNAP_ON)
			v.vertex = UnityPixelSnap (v.vertex);
			#endif
			
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.color = v.color * _Color;
		}

		void surf (Input IN, inout SurfaceOutput o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
			fixed4 mask = tex2D(_ColorifyMaskTex, IN.uv_MainTex); 
			float lum = 0.299 * c.r + 0.587 * c.g + 0.114 * c.b;
			fixed3 col  = lerp(c.rgb,_NewColor.rgb * lum,mask.r);
			col = lerp(col,_NewColor2.rgb * lum,mask.g);
			col = lerp(col,_NewColor3.rgb * lum,mask.b);
			col = lerp(col,_NewColor4.rgb * lum,mask.a);
			o.Alpha = c.a;
			o.Albedo = col;
			//o.Albedo = mask;
		}
		ENDCG
	}

Fallback "Transparent/VertexLit"
}
