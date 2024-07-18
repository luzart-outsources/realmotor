//______________________________________________//
//________Realistic Car Shaders - Mobile________//
//______________________________________________//
//_______Copyright © 2024 Skril Studio__________//
//______________________________________________//
//__________ http://skrilstudio.com/ ___________//
//______________________________________________//
//________ http://fb.com/yugelmobile/ __________//
//______________________________________________//

Shader "Mobile/ Realistic Car Shaders/Body Color + Decal + Pearlescent + Diffuse + Holographic Reflection"
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

		// pearlescent
		_PearlescentColor("Pearlescent Color", Color) = (1, 1, 1, 1)
		_MainTexPearl("Diffuse Pearl", 2D) = "white" {}
		_PearlBumpMap("Diffuse Bumpmap", 2D) = "bump" {}
		_PearlUVScale ("Texture UV Scale", Range(1, 100)) = 1
		_ShininessIntensity("Pearlescent Intensity", Range(0, 4)) = 0
		_ShininessScale("Pearlescent Scale", Range(1, 50)) = 1
		// metallicness
		_MetalicnessIntensity("Metalicness Intensity", Range(0, 2)) = 1
		_MetalicnessScale("Metalicness Scale", Range(100, 1000)) = 500

	    // reflection
		_Cube("Reflection Cubemap", Cube) = "white" {}
		_RefIntensity("Reflection Intensity", Range(0.5, 2)) = 0
	    _RefVisibility("Reflection Holographic Scale", Range(0.5, 3)) = 0.5
		_MetalBrightnessIntensity("Metal Brightness Intensity", Range(0.1, 2.0)) = 1
	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
		#pragma surface surf BlinnPhong
		#pragma target 3.0
        #pragma multi_compile Rendered_Texture Cubemap_T Cubemap_Assigned Both_T Off_T
        #pragma multi_compile Bumped_Diffuse_Off Bumped_Diffuse
        #pragma multi_compile Bumped_Pearl_Off Bumped_Pearl
        #pragma multi_compile DecalHighlightOff DecalHighlightOn
		#pragma multi_compile DiffuseReflectOn DiffuseReflectOff
		struct Input
		{
			float2 uv_Decal;
			float2 uv_MainTex;
			float2 uv_MainTexPearl;
			float3 worldRefl;
			float3 viewDir;
			INTERNAL_DATA
		};
		sampler2D _Decal;
		sampler2D _RenderedTexture;
		sampler2D _MainTex;
		sampler2D _DiffuseBumpMap;
		sampler2D _MainTexPearl;
		sampler2D _PearlBumpMap;
		samplerCUBE _Cube;
		float4 _DecalColor;
		float4 _Color;
		float4 _PearlescentColor;
		float _DiffuseUVScale;
		float _DecalTransparency;
		float _DecalReflection;
		float _DecalUVScale;
		float _PearlUVScale;
		float _RefIntensity;
		float _RefVisibility;
		float _MetalBrightnessIntensity;
		float _ShininessIntensity;
		float _ShininessScale;
		float _MetalicnessIntensity;
		float _MetalicnessScale;
		float3 body;
		//uniform float4x4 _Rotation;
		void surf(Input IN, inout SurfaceOutput s)
		{
            #if Bumped_Diffuse
			s.Normal = UnpackNormal(tex2D(_DiffuseBumpMap, IN.uv_MainTex*_DiffuseUVScale));
            #else
			s.Normal = normalize(float3(0, 0, 1));
            #endif
			// decal and chassis texture
			float3 worldVec = WorldReflectionVector(IN, s.Normal);
			float4 DecalTexture = tex2D(_Decal, IN.uv_Decal*_DecalUVScale);
			float4 _RenderedTxt = tex2D(_RenderedTexture, worldVec);
			float4 BodyTexture = tex2D(_MainTex, IN.uv_MainTex*_DiffuseUVScale);

			float4 BodyPearlTexture = tex2D(_MainTexPearl, IN.uv_MainTexPearl*_PearlUVScale);

			float decalSpecularMask = DecalTexture.a * _DecalTransparency;
			float bodySpecularMask = BodyTexture.a;
            #if DiffuseReflectOff
			float bodySpecularMask2 = -.01f;
            #else
			float bodySpecularMask2 = BodyTexture.a;
            #endif
            #if DecalHighlightOn
			float4 DecalDiffuse = (((_DecalColor * decalSpecularMask) * DecalTexture) +(DecalTexture * (1 - decalSpecularMask))) * decalSpecularMask * ((1 - bodySpecularMask) + decalSpecularMask) * (1 - bodySpecularMask);
            #else
			float4 DecalDiffuse = ((_DecalColor * decalSpecularMask) * DecalTexture);
            #endif
			float4 BodyColor= _Color/5;
			float4 BodyDiffuse = (bodySpecularMask * BodyTexture) + (BodyTexture * (1 - bodySpecularMask));

			// reflection
			float4 cubemapTexture = texCUBE(_Cube, worldVec/*mul(_Rotation, worldVec)*/); // user set cubemap
			float4 cubemapTexture2 = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, worldVec); // unity assigned cubemap

			// bumped pearl/flakes/carbon
            #if Bumped_Pearl
			float3 pearlBumpTexture = UnpackNormal(tex2D(_PearlBumpMap, IN.uv_MainTexPearl * _PearlUVScale));
			float shininessResult = (BodyPearlTexture * _ShininessIntensity) * pow(abs(dot(normalize(IN.viewDir), s.Normal * pearlBumpTexture)), _ShininessScale);
			s.Normal = pearlBumpTexture;
            #else
			float shininessResult = (BodyPearlTexture * _ShininessIntensity) * pow(abs(dot(normalize(IN.viewDir), s.Normal)), _ShininessScale);
            #endif
			// metallicness
			float shininessResult2 = _MetalicnessIntensity * pow(abs(dot(normalize(IN.viewDir), normalize(float3(0, 0, 1)))), _MetalicnessScale) * (1 - bodySpecularMask2);

			// both
            #if Both_T
			float3 reflectionResult = _RefIntensity * cubemapTexture * _RenderedTxt * pow((1.0 - WorldReflectionVector(IN, s.Normal)), _RefVisibility);
			body = reflectionResult * (1 - bodySpecularMask2) * (1 - decalSpecularMask) + reflectionResult * decalSpecularMask * _DecalReflection * (1 - bodySpecularMask2) + BodyDiffuse * bodySpecularMask * (1 - decalSpecularMask);
            #endif
			// remdered texture only
            #if Rendered_Texture
			float3 reflectionResult = _RefIntensity * _RenderedTxt * pow((1.0 - WorldReflectionVector(IN, s.Normal)), _RefVisibility);
			body = reflectionResult * (1 - bodySpecularMask2) * (1 - decalSpecularMask) + reflectionResult*decalSpecularMask*_DecalReflection* (1 - bodySpecularMask2) + BodyDiffuse * bodySpecularMask* (1 - decalSpecularMask);
            #endif
			// cubemap only
            #if Cubemap_T
			float3 reflectionResult = _RefIntensity * cubemapTexture *pow((1.0 - WorldReflectionVector(IN, s.Normal)), _RefVisibility);
			body = reflectionResult * (1 - bodySpecularMask2) * (1 - decalSpecularMask) + reflectionResult*decalSpecularMask*_DecalReflection* (1 - bodySpecularMask2) + BodyDiffuse * bodySpecularMask* (1 - decalSpecularMask);
            #endif
			// assigned cubemap only
            #if Cubemap_Assigned
			float3 reflectionResult = _RefIntensity * cubemapTexture2 * pow((1.0 - WorldReflectionVector(IN, s.Normal)), _RefVisibility);
			body = reflectionResult * (1 - bodySpecularMask2) * (1 - decalSpecularMask) + reflectionResult * decalSpecularMask * _DecalReflection * (1 - bodySpecularMask2) + BodyDiffuse * bodySpecularMask * (1 - decalSpecularMask);
            #endif
			// no reflection
            #if Off_T
			float3 reflectionResult = pow((1.0 - WorldReflectionVector(IN, s.Normal)), _RefVisibility);
			body = reflectionResult * (1 - bodySpecularMask2) * (1 - decalSpecularMask) + reflectionResult * decalSpecularMask * _DecalReflection * (1 - bodySpecularMask2) + BodyColor + BodyDiffuse * bodySpecularMask * (1 - decalSpecularMask);
            #endif
			
			// combine everything
			s.Albedo = BodyColor * (1 - bodySpecularMask) * (1 - decalSpecularMask) + (shininessResult * _PearlescentColor * (1 - bodySpecularMask) * (1 - decalSpecularMask)) + body * _MetalBrightnessIntensity + BodyDiffuse * bodySpecularMask + DecalDiffuse + shininessResult2;
		}
		ENDCG
	}
	FallBack "Standard"
	CustomEditor "SkrilStudio.VehicleDecalPearlRGB_Editor"
}
