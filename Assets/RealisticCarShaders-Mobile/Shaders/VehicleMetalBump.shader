//______________________________________________//
//________Realistic Car Shaders - Mobile________//
//______________________________________________//
//_______Copyright � 2024 Skril Studio__________//
//______________________________________________//
//__________ http://skrilstudio.com/ ___________//
//______________________________________________//
//________ http://fb.com/yugelmobile/ __________//
//______________________________________________//

Shader "Mobile/ Realistic Car Shaders/Metal" 
{
	Properties
	{
		// metal texture
		_MainTex("Diffuse", 2D) = "white" {}
	    _DiffuseBumpMap("Diffuse Bumpmap", 2D) = "bump" {}
	    _DiffuseUVScale("Diffuse UV Scale", Range(1, 100)) = 1

	    // reflection
	    _Cube("Reflection Cubemap", Cube) = "white" {}
	    _RefIntensity("Reflection Intensity", Range(0,2)) = 0
		_RefVisibility("Reflection Visibility Scale", Range(0.1, 2)) = 0.1
		_RenderedTexture("Rendered Texture", 2D) = "white" {}

		// shininess settings
		_ShininessIntensity("Metal Brightness Intensity", Range(0.0, 2.0)) = 1
	}
	SubShader
	{
	Tags{ "RenderType" = "Opaque" }
	LOD 200
	CGPROGRAM
	#pragma surface surf BlinnPhong
	#pragma target 2.0
    #pragma multi_compile Rendered_Texture Cubemap_T Cubemap_Assigned Both_T Off_T
    #pragma multi_compile Bumped_Diffuse_Off Bumped_Diffuse
	struct Input
	{
		float2 uv_MainTex;
		float3 worldRefl;
		float3 viewDir;
		INTERNAL_DATA
	};
	sampler2D _MainTex;
	sampler2D _DiffuseBumpMap;
	sampler2D _RenderedTexture;
	samplerCUBE _Cube;
	float _DiffuseUVScale;
	float _RefIntensity;
	float _RefVisibility;
	float _ShininessIntensity;
	float3 body;
	//uniform float4x4 _Rotation;
	void surf(Input IN, inout SurfaceOutput s)
	{
        #if Bumped_Diffuse
		s.Normal = normalize(float3(0, 0, 1) + UnpackNormal(tex2D(_DiffuseBumpMap, IN.uv_MainTex*_DiffuseUVScale)));
        #else
		s.Normal = normalize(float3(0, 0, 1));
        #endif

		// texture
		float3 worldVec = WorldReflectionVector(IN, s.Normal);
		float4 BodyTexture = tex2D(_MainTex, IN.uv_MainTex*_DiffuseUVScale);
		float4 _RenderedTxt = tex2D(_RenderedTexture, worldVec);
		float bodySpecularMask = BodyTexture.a;
		float4 BodyDiffuse = (bodySpecularMask * BodyTexture) + (BodyTexture * (1 - bodySpecularMask));

		// reflection	
		float4 cubemapTexture = texCUBE(_Cube, worldVec/*mul(_Rotation, worldVec)*/); // user set cubemap
		float4 cubemapTexture2 = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, worldVec); // unity assigned cubemap

		// both
        #if Both_T
		float4 reflectionResult = _RefIntensity * cubemapTexture * (_RenderedTxt - (-1 * cubemapTexture.a)) * pow((1.0 - dot(normalize(IN.viewDir), s.Normal)), _RefVisibility);
		body = reflectionResult * reflectionResult + bodySpecularMask * reflectionResult * reflectionResult;
        #endif
		// remdered texture only
        #if Rendered_Texture
		float4 reflectionResult = _RefIntensity * _RenderedTxt * pow((1.0 - dot(normalize(IN.viewDir), s.Normal)), _RefVisibility);
		body = reflectionResult * reflectionResult + bodySpecularMask * reflectionResult * reflectionResult;
        #endif
		// cubemap only
        #if Cubemap_T
		float4 reflectionResult = _RefIntensity * cubemapTexture * pow((1.0 - dot(normalize(IN.viewDir), s.Normal)), _RefVisibility);
		body = reflectionResult * reflectionResult + bodySpecularMask * reflectionResult * reflectionResult;
        #endif
		// assigned cubemap only
        #if Cubemap_Assigned
		float4 reflectionResult = _RefIntensity * cubemapTexture2 * pow((1.0 - dot(normalize(IN.viewDir), s.Normal)), _RefVisibility);
		body = reflectionResult * reflectionResult + bodySpecularMask * reflectionResult * reflectionResult;
        #endif

		// combine everything
		s.Albedo = BodyDiffuse * bodySpecularMask * _ShininessIntensity + body;
	}
	ENDCG
	}
	FallBack "Standard"
	CustomEditor "SkrilStudio.VehicleMetalBump_Editor"
}
