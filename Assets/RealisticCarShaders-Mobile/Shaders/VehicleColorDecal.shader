//______________________________________________//
//________Realistic Car Shaders - Mobile________//
//______________________________________________//
//_______Copyright © 2024 Skril Studio__________//
//______________________________________________//
//__________ http://skrilstudio.com/ ___________//
//______________________________________________//
//________ http://fb.com/yugelmobile/ __________//
//______________________________________________//

Shader "Mobile/ Realistic Car Shaders/Body Color + Decal + Diffuse"
{
	Properties
	{
		// vehicle color
		_Color("Vehicle Color", Color) = (1, 1, 1, 1)

		// chassis texture for example: rust, damage or parts texture
		_MainTex("Diffuse", 2D) = "white" {}
	    _DiffuseUVScale("Diffuse UV Scale", Range(1, 100)) = 1
		_DiffuseBumpMap("Diffuse Bumpmap", 2D) = "bump" {}
		_RenderedTexture("Rendered Texture", 2D) = "white" {}

	    // decal
	    _DecalColor("Decal Color", Color) = (1, 1, 1,1)
	    _Decal("Decal", 2D) = "white" {}
		_DecalTransparency("Decal Transparency", Range(0.1, 1)) = 1
	    _DecalReflection("Decal Reflection", Range(0, 1)) = 0.5
		_DecalUVScale("Decal UV Scale", Range(1, 50)) = 1
		// metallicness
		_MetalicnessIntensity("Metalicness Intensity", Range(0, 2)) = 1
		_MetalicnessScale("Metalicness Scale", Range(100, 1000)) = 500

	    // reflection
	    _Cube("Reflection Cubemap", Cube) = "white" {}
	    _RefIntensity("Reflection Intensity", Range(0, 2)) = 0
	    _RefVisibility("Reflection Visibility Scale", Range(0.1, 2)) = 0.1
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
		#pragma multi_compile DiffuseReflectOn DiffuseReflectOff
		struct Input
		{
			float2 uv_Decal;
			float2 uv_MainTex;
			float3 worldRefl;
			float3 viewDir;
			INTERNAL_DATA
		};
		sampler2D _Decal;
		sampler2D _MainTex;
		sampler2D _DiffuseBumpMap;
		sampler2D _RenderedTexture;
		samplerCUBE _Cube;
		float4 _DecalColor;
		float4 _Color;
		float _DiffuseUVScale;
		float _DecalTransparency;
		float _DecalReflection;
		float _DecalUVScale;
		float _RefIntensity;
		float _RefVisibility;
		float _MetalicnessIntensity;
		float _MetalicnessScale;
		float3 body;
		//uniform float4x4 _Rotation;
		void surf(Input IN, inout SurfaceOutput s)
		{
            #if Bumped_Diffuse
			s.Normal = normalize(float3(0, 0, 1) + UnpackNormal(tex2D(_DiffuseBumpMap, IN.uv_MainTex*_DiffuseUVScale)));
            #else
			s.Normal = normalize(float3(0, 0, 1));
            #endif
			// decal and chassis texture
			float3 worldVec = WorldReflectionVector(IN, s.Normal);
			float4 DecalTexture = tex2D(_Decal, IN.uv_Decal*_DecalUVScale);
			float4 _RenderedTxt = tex2D(_RenderedTexture, worldVec);
			float4 BodyTexture = tex2D(_MainTex, IN.uv_MainTex*_DiffuseUVScale);
			float decalSpecularMask = DecalTexture.a* _DecalTransparency;
			float bodySpecularMask = BodyTexture.a;
            #if DiffuseReflectOff
			float bodySpecularMask2 = -.01f;
            #else
			float bodySpecularMask2 = BodyTexture.a;
            #endif
			float4 DecalDiffuse = ((_DecalColor * decalSpecularMask) * DecalTexture);
			float4 BodyColor = _Color;
			float4 BodyDiffuse = (bodySpecularMask * BodyTexture) + (BodyTexture * (1- bodySpecularMask));

			// reflection
			float4 cubemapTexture = texCUBE(_Cube, worldVec /*mul(_Rotation, worldVec)*/); // user set cubemap
			float4 cubemapTexture2 = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, worldVec); // unity assigned cubemap
			// metallicness
			float shininessResult2 = _MetalicnessIntensity * pow(abs(dot(normalize(IN.viewDir), normalize(float3(0, 0, 1)))), _MetalicnessScale) * (1 - bodySpecularMask2);

			// both
            #if Both_T
			float4 reflectionResult = _RefIntensity * cubemapTexture * (_RenderedTxt -(-1 * cubemapTexture.a)) * pow((1.0 - dot(normalize(IN.viewDir), s.Normal)), _RefVisibility);
			body = reflectionResult * (1 - bodySpecularMask2) * (1 - decalSpecularMask) + reflectionResult*decalSpecularMask*_DecalReflection* (1 - bodySpecularMask2) + BodyDiffuse * bodySpecularMask* (1 - decalSpecularMask);
            #endif
			// remdered texture only
            #if Rendered_Texture
			float4 reflectionResult = _RefIntensity * _RenderedTxt * pow((1.0 - dot(normalize(IN.viewDir), s.Normal)), _RefVisibility);
			body = reflectionResult * (1 - bodySpecularMask2) * (1 - decalSpecularMask) + reflectionResult*decalSpecularMask*_DecalReflection* (1 - bodySpecularMask2) + BodyDiffuse * bodySpecularMask* (1 - decalSpecularMask);
            #endif
			// cubemap only
            #if Cubemap_T
			float4 reflectionResult = _RefIntensity * cubemapTexture * pow((1.0 - dot(normalize(IN.viewDir), s.Normal)), _RefVisibility);
			body = reflectionResult * (1 - bodySpecularMask2) * (1 - decalSpecularMask) + reflectionResult*decalSpecularMask*_DecalReflection* (1 - bodySpecularMask2) + BodyDiffuse * bodySpecularMask* (1 - decalSpecularMask);
            #endif
			// assigned cubemap only
            #if Cubemap_Assigned
			float4 reflectionResult = _RefIntensity * cubemapTexture2 * pow((1.0 - dot(normalize(IN.viewDir), s.Normal)), _RefVisibility);
			body = reflectionResult * (1 - bodySpecularMask2) * (1 - decalSpecularMask) + reflectionResult * decalSpecularMask * _DecalReflection * (1 - bodySpecularMask2) + BodyDiffuse * bodySpecularMask * (1 - decalSpecularMask);
            #endif

			// combine everything
			s.Albedo = BodyColor * (1 - bodySpecularMask) * (1 - decalSpecularMask) + BodyDiffuse * bodySpecularMask + DecalDiffuse + body + shininessResult2;
		}
		ENDCG
	}
	FallBack "Standard"
	CustomEditor "SkrilStudio.VehicleDecal_Editor"
}
