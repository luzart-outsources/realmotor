//______________________________________________//
//________Realistic Car Shaders - Mobile________//
//______________________________________________//
//_______Copyright © 2024 Skril Studio__________//
//______________________________________________//
//__________ http://skrilstudio.com/ ___________//
//______________________________________________//
//________ http://fb.com/yugelmobile/ __________//
//______________________________________________//

Shader "Mobile/ Realistic Car Shaders/Light + Glass + Diffuse"
{
	Properties
	{
		// glass color
		_Color("Glass Color", Color) = (1, 1, 1, 1)

		// glass transparency
		_Transprnt("Glass Transparency", Range(0.05, 1)) = 0.5

		// glass texture for example: heater, decal, damage
		_MainTex("Diffuse", 2D) = "white" {}
	    _DiffuseUVScale("Diffuse UV Scale", Range(1, 100)) = 1
		_DiffuseBumpMap("Diffuse Bumpmap", 2D) = "bump" {}

		// reflection
	    _Cube("Reflection Cubemap", Cube) = "white" {}
	    _RefIntensity("Reflection Intensity", Range(0, 2)) = 0
		_RefVisibility("Reflection Visibility Scale", Range(0.1, 2)) = 0.1
		_RenderedTexture("Rendered Texture", 2D) = "white" {}

		_Emission("Emission", Range(0, 2)) = 1
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" }
		//Cull Off
		LOD 200
		CGPROGRAM
		#pragma surface surf BlinnPhong alpha
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
		// transparency
		struct v2f
		{
			float2 uv : TEXCOORD1;
		};
		v2f vert(inout appdata_full v)
		{
			v2f o;
			return o;
		}
		sampler2D _MainTex;
		sampler2D _DiffuseBumpMap;
		sampler2D _RenderedTexture;
		samplerCUBE _Cube;
		float4 _Color;
		float _DiffuseUVScale;
		float _Transprnt;
		float _RefIntensity;
		float _RefVisibility;
		float4 reflectionResult;
		float _Emission; // from 0 to 2
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
			float4 cubemapTexture = texCUBE(_Cube, worldVec); // user set cubemap
			float4 cubemapTexture2 = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, worldVec); // unity assigned cubemap

			// transparency
			float4 opacit = _Color.a * _Transprnt;
			// both
            #if Both_T
			reflectionResult = _RefIntensity * cubemapTexture * (_RenderedTxt - (-1 * cubemapTexture.a)) * pow((1.0 - dot(normalize(IN.viewDir), s.Normal * normalize(float3(0, 0, 1)))), _RefVisibility);
			s.Emission = _Emission * _Color * (1 - bodySpecularMask);
			s.Alpha = opacit;
            #endif
			// remdered texture only
            #if Rendered_Texture
			reflectionResult = _RefIntensity * _RenderedTxt * pow((1.0 - dot(normalize(IN.viewDir), s.Normal * normalize(float3(0, 0, 1)))), _RefVisibility);
			s.Emission = _Emission * _Color * (1 - bodySpecularMask);
			s.Alpha = opacit;
            #endif
			// cubemap only
            #if Cubemap_T
			reflectionResult = _RefIntensity * cubemapTexture * pow((1.0 - dot(normalize(IN.viewDir), s.Normal * normalize(float3(0, 0, 1)))), _RefVisibility);
			s.Emission = _Emission * _Color * (1 - bodySpecularMask);
			s.Alpha = opacit;
            #endif
			// assigned cubemap only
            #if Cubemap_Assigned
			reflectionResult = _RefIntensity * cubemapTexture2 * pow((1.0 - dot(normalize(IN.viewDir), s.Normal * normalize(float3(0, 0, 1)))), _RefVisibility);
			s.Emission = _Emission * _Color * (1 - bodySpecularMask);
			s.Alpha = opacit;
            #endif
			// reflection is off
            #if Off_T
			s.Emission = _Emission * _Color * (1 - bodySpecularMask);
			s.Alpha = opacit;
            #endif	

			// combine everything
			s.Albedo = _Color * (1 - bodySpecularMask) + BodyDiffuse * bodySpecularMask + reflectionResult;
		}
		ENDCG
	}
	FallBack "Standard"
    CustomEditor "SkrilStudio.VehicleLight_Editor"
}
