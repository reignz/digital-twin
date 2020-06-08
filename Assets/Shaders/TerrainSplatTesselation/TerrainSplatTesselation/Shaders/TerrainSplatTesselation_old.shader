Shader "Terrain/SplatTerrainTesselation" {
	Properties {
		_Mask("Mask", 2D) = "black" {}

		_MainTex0("Albedo 0", 2D) = "gray" {}
		_MainTex1("Albedo 1", 2D) = "gray" {}
		_MainTex2("Albedo 2", 2D) = "gray" {}
		
		_NormalMap0("NormalMap 0", 2D) = "bump" {}
	    _BumpScale0("Scale", Float) = 1.0
		_NormalMap1("NormalMap 1", 2D) = "bump" {}
	    _BumpScale1("Scale", Float) = 1.0
		_NormalMap2("NormalMap 2", 2D) = "bump" {}
	    _BumpScale2("Scale", Float) = 1.0

		_PropertiesMap0("Properties 0", 2D) = "gray" {}
		_PropertiesMap1("Properties 1", 2D) = "gray" {}
		_PropertiesMap2("Properties 2", 2D) = "gray" {}

		_Displacement0 ("Displacement 0", Range(0, 1.0)) = 0.3
		_Displacement1 ("Displacement 1", Range(0, 1.0)) = 0.3
		_Displacement2 ("Displacement 2", Range(0, 1.0)) = 0.3

		_MinDist ("MinDistance", Range(0.0001,64)) = 4
		_MaxDist ("MaxDistance", Range(1,256)) = 25
		_Tess ("Tessellation", Range(1,256)) = 4
		_initialDepth ("Initial Depth", Range(0.0001,1)) = 0.2
      	_TintColor0 ("Tint Color 0", Color) = (0.98, 0.858, 0.694, 1)
      	_TintColor1 ("Tint Color 1", Color) = (0.98, 0.858, 0.694, 1)
      	_TintColor2 ("Tint Color 2", Color) = (0.98, 0.858, 0.694, 1)
		_TintScale ("Tint scale", Float) = 0.25
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types with displacement and tesselation
		#pragma surface surf Standard fullforwardshadows vertex:disp tessellate:tessDistance 
		#pragma shader_feature HEIGHT_BASED_ON
		#pragma shader_feature RANDOM_TINT_ON
		#pragma target 4.6
		#include "Tessellation.cginc"
		#include "TerrainSplatTesselation.cginc"

		sampler2D _Mask;

		#if RANDOM_TINT_ON
		fixed4 _TintColor0;
		fixed4 _TintColor1;
		fixed4 _TintColor2;
		fixed _TintScale;
		#endif

		sampler2D _MainTex0;
		sampler2D _MainTex1;
		sampler2D _MainTex2;

		half4 _MainTex1_ST;

		sampler2D _NormalMap0;
	    half _BumpScale0;
		sampler2D _NormalMap1;
	    half _BumpScale1;
		sampler2D _NormalMap2;
	    half _BumpScale2;

		sampler2D _PropertiesMap0;
		half4 _PropertiesMap0_ST;
		sampler2D _PropertiesMap1;
		sampler2D _PropertiesMap2;

		half _Displacement0;
		half _Displacement1;
		half _Displacement2;

		float _MinDist;
		float _MaxDist;
		float _Tess;
		float _isHeightBased;
		float _initialDepth;
		
		struct appdata {
			float4 vertex : POSITION;
			float4 tangent : TANGENT;
			float3 normal : NORMAL;
			float2 texcoord : TEXCOORD0;
			float2 texcoord1 : TEXCOORD1;
			float2 texcoord2 : TEXCOORD2;
		};

		float4 tessDistance (appdata v0, appdata v1, appdata v2) {
			float minDist = _MinDist;
			float maxDist = _MaxDist;
			return UnityDistanceBasedTess(v0.vertex, v1.vertex, v2.vertex, minDist, maxDist, _Tess);
		}
		
		// float _EdgeLength;

		// float4 tessEdge (appdata v0, appdata v1, appdata v2)
		// {
		// 	return UnityEdgeLengthBasedTessCull  (v0.vertex, v1.vertex, v2.vertex, _EdgeLength, _Tess);
		// }

		void disp (inout appdata v)
		{
			fixed3 mask = tex2Dlod(_Mask, float4(v.texcoord.xy,0,0)).rgb;
			float disp0 = tex2Dlod(_PropertiesMap0, float4(v.texcoord.xy * _PropertiesMap0_ST.xy,0,0)).g;
			float disp1 = tex2Dlod(_PropertiesMap1, float4(v.texcoord.xy * _PropertiesMap0_ST.xy,0,0)).g;
			float disp2 = tex2Dlod(_PropertiesMap2, float4(v.texcoord.xy * _PropertiesMap0_ST.xy,0,0)).g;

			#if HEIGHT_BASED_ON
			float d = blend1ByDepth (disp0 * _Displacement0, disp0, disp1 * _Displacement1, disp1, disp2 * _Displacement2, disp2, mask, _initialDepth);
			#else
			float d = blend1 (disp0 * _Displacement0, disp1 * _Displacement1, disp2 * _Displacement2, mask);
			#endif

			v.vertex.xyz += v.normal * d;
		}

		struct Input {
			float2 uv_Mask;
			float2 uv_MainTex0;
			float3 worldPos;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed3 mask = tex2D (_Mask, IN.uv_Mask).rgb;

			#if RANDOM_TINT_ON
				fixed tintFactor = noise(IN.worldPos.xz * _TintScale);

				fixed4 albedo0 = tex2D (_MainTex0, IN.uv_MainTex0) * lerp (fixed4(1, 1, 1, 1), _TintColor0, tintFactor);
				fixed4 albedo1 = tex2D (_MainTex1, IN.uv_MainTex0) * lerp (fixed4(1, 1, 1, 1), _TintColor1, tintFactor);
				fixed4 albedo2 = tex2D (_MainTex2, IN.uv_MainTex0) * lerp (fixed4(1, 1, 1, 1), _TintColor2, tintFactor);
			#else
				fixed4 albedo0 = tex2D (_MainTex0, IN.uv_MainTex0);
				fixed4 albedo1 = tex2D (_MainTex1, IN.uv_MainTex0);
				fixed4 albedo2 = tex2D (_MainTex2, IN.uv_MainTex0);
			#endif

			fixed2 properties0 = tex2D (_PropertiesMap0, IN.uv_MainTex0).rg;
			fixed2 properties1 = tex2D (_PropertiesMap1, IN.uv_MainTex0).rg;
			fixed2 properties2 = tex2D (_PropertiesMap2, IN.uv_MainTex0).rg;

			#if HEIGHT_BASED_ON
				o.Albedo = blendByDepth (albedo0.rgb, properties0.g, albedo1.rgb, properties1.g, albedo2.rgb, properties2.g, mask, _initialDepth);
			#else
				o.Albedo = blend (albedo0.rgb, albedo1.rgb, albedo2.rgb, mask);
			#endif

			// Metallic and smoothness come from slider variables
			o.Metallic = 0;
			#if HEIGHT_BASED_ON
			o.Smoothness = 1 - blend1ByDepth (properties0.r, properties0.g, properties1.r, properties1.g, properties2.r, properties2.g, mask, _initialDepth);
			#else
			o.Smoothness = 1 - blend1 (properties0.r, properties0.r, properties0.r, mask);
			#endif

			fixed3 normal0 = normalize(UnpackScaleNormal(tex2D(_NormalMap0, IN.uv_MainTex0), _BumpScale0));
			fixed3 normal1 = normalize(UnpackScaleNormal(tex2D(_NormalMap1, IN.uv_MainTex0), _BumpScale1));
			fixed3 normal2 = normalize(UnpackScaleNormal(tex2D(_NormalMap2, IN.uv_MainTex0), _BumpScale2));
			#if HEIGHT_BASED_ON
			o.Normal = blendByDepth (normal0, properties0.g, normal1, properties1.g, normal2.rgb, properties2.g, mask, _initialDepth);
			#else
			o.Normal = blend (normal0, normal1, normal2, mask);
			#endif
		}
		ENDCG
	}
	FallBack "Diffuse"
	CustomEditor "TerrainSplatTesselationInspector"
}
