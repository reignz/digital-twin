Shader "Terrain/SplatTerrainTesselation" {
	Properties {
		_Mask("Mask", 2D) = "black" {}

		_MainTex0("AlbedoSmoothness 0", 2D) = "gray" {}
		_MainTex1("AlbedoSmoothness 1", 2D) = "gray" {}
		_MainTex2("AlbedoSmoothness 2", 2D) = "gray" {}
		_MainTex3("AlbedoSmoothness 3", 2D) = "gray" {}
		
		_NormalMap0("NormalMapDisplacement 0", 2D) = "bump" {}
	    _BumpScale0("Scale", Float) = 1.0
		_NormalMap1("NormalMapDisplacement 1", 2D) = "bump" {}
	    _BumpScale1("Scale", Float) = 1.0
		_NormalMap2("NormalMapDisplacement 2", 2D) = "bump" {}
	    _BumpScale2("Scale", Float) = 1.0
		_NormalMap3("NormalMapDisplacement 3", 2D) = "bump" {}
	    _BumpScale3("Scale", Float) = 1.0

		_Displacement0 ("Displacement 0", Range(0, 1.0)) = 0.3
		_Displacement1 ("Displacement 1", Range(0, 1.0)) = 0.3
		_Displacement2 ("Displacement 2", Range(0, 1.0)) = 0.3
		_Displacement3 ("Displacement 3", Range(0, 1.0)) = 0.3

		_MinDist ("MinDistance", Range(0.0001,64)) = 4
		_MaxDist ("MaxDistance", Range(1,256)) = 25
		_Tess ("Tessellation", Range(1,256)) = 4
		_initialDepth ("Initial Depth", Range(0.0001,1)) = 0.2
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types with displacement and tesselation
		#pragma surface surf Standard fullforwardshadows vertex:disp tessellate:tessDistance 
		#pragma target 4.6
		#include "Tessellation.cginc"
		#include "TerrainSplatTesselation.cginc"

		sampler2D _Mask;

		sampler2D _MainTex0;
		sampler2D _MainTex1;
		sampler2D _MainTex2;
		sampler2D _MainTex3;

		sampler2D _NormalMap0;
	    half _BumpScale0;
		sampler2D _NormalMap1;
	    half _BumpScale1;
		sampler2D _NormalMap2;
	    half _BumpScale2;
		sampler2D _NormalMap3;
	    half _BumpScale3;

		half4 _PropertiesMap0_ST;

		half _Displacement0;
		half _Displacement1;
		half _Displacement2;
		half _Displacement3;

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

		float4 tessDistance(appdata v0, appdata v1, appdata v2) {
			float minDist = _MinDist;
			float maxDist = _MaxDist;
			return UnityDistanceBasedTess(v0.vertex, v1.vertex, v2.vertex, minDist, maxDist, _Tess);
		}
		

		void disp (inout appdata v)
		{
			fixed4 mask = tex2Dlod(_Mask, float4(v.texcoord.xy,0,0));
			float disp0 = tex2Dlod(_NormalMap0, float4(v.texcoord.xy * _PropertiesMap0_ST.xy,0,0)).g;
			float disp1 = tex2Dlod(_NormalMap1, float4(v.texcoord.xy * _PropertiesMap0_ST.xy,0,0)).g;
			float disp2 = tex2Dlod(_NormalMap2, float4(v.texcoord.xy * _PropertiesMap0_ST.xy,0,0)).g;
			float disp3 = tex2Dlod(_NormalMap3, float4(v.texcoord.xy * _PropertiesMap0_ST.xy,0,0)).g;

			float d = blend5 (disp0 * _Displacement0, disp1 * _Displacement1, disp2 * _Displacement2, disp3 * _Displacement3, mask);

			v.vertex.xyz += v.normal * d;
		}

		struct Input {
			float2 uv_Mask;
			float2 uv_MainTex0;
			float2 uv_MainTex1;
			float2 uv_MainTex2;
			float2 uv_MainTex3;
			float3 worldPos;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 mask = tex2D (_Mask, IN.uv_Mask);

			
			fixed4 albedo0 = tex2D(_MainTex0, IN.uv_MainTex0);
			fixed4 albedo1 = tex2D(_MainTex1, IN.uv_MainTex1);
			fixed4 albedo2 = tex2D(_MainTex2, IN.uv_MainTex2);
			fixed4 albedo3 = tex2D(_MainTex3, IN.uv_MainTex3);

			fixed properties0 = tex2D(_NormalMap0, IN.uv_MainTex0).a;
			fixed properties1 = tex2D(_NormalMap1, IN.uv_MainTex1).a;
			fixed properties2 = tex2D(_NormalMap2, IN.uv_MainTex2).a;
			fixed properties3 = tex2D(_NormalMap3, IN.uv_MainTex3).a;

			
			o.Albedo = blend(albedo0.rgb, albedo1.rgb, albedo2.rgb, albedo3.rgb, mask);
			

			// Metallic and smoothness come from slider variables
			o.Metallic = 0;
			
			o.Smoothness = 1 - blend5(albedo0.a, albedo1.a, albedo2.a, albedo3.a, mask);

			fixed3 normal0 = normalize(UnpackScaleNormal(tex2D(_NormalMap0, IN.uv_MainTex0), _BumpScale0));
			fixed3 normal1 = normalize(UnpackScaleNormal(tex2D(_NormalMap1, IN.uv_MainTex1), _BumpScale1));
			fixed3 normal2 = normalize(UnpackScaleNormal(tex2D(_NormalMap2, IN.uv_MainTex2), _BumpScale2));
			fixed3 normal3 = normalize(UnpackScaleNormal(tex2D(_NormalMap3, IN.uv_MainTex3), _BumpScale3));
	
			o.Normal = blend(normal0, normal1, normal2, normal3, mask);
		}
		ENDCG
	}
	FallBack "Diffuse"
	CustomEditor "TerrainSplatTesselationInspector"
}
