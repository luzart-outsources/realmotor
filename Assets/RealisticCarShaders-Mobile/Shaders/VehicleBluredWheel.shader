//______________________________________________//
//________Realistic Car Shaders - Mobile________//
//______________________________________________//
//_______Copyright © 2022 Skril Studio__________//
//______________________________________________//
//__________ http://skrilstudio.com/ ___________//
//______________________________________________//
//________ http://fb.com/yugelmobile/ __________//
//______________________________________________//
Shader "Mobile/ Realistic Car Shaders/Blured Wheel"
{
	Properties
	{
		// wheel
		_WheelColor("Wheel Color", Color) = (1, 1, 1,1)
		_WheelTexture("Wheel Blured Texture", 2D) = "white" {}
		_Transparency("Transparency", Range(0, 1)) = 1
		_RefVisibility("Reflection Visibility", Range(0, 1)) = 0.5
		_BlurLevel("Blur Level", Range(0, 1)) = 0

		// reflection
		_Cube("Reflection Cubemap", Cube) = "white" {}
		_RefIntensity("Reflection Intensity", Range(0, 2)) = 1
		_RenderedTexture("Rendered Texture", 2D) = "white" {}
	}
		SubShader
	{
		Tags{ "Queue" = "Transparent" }
		LOD 100
		CGPROGRAM
		#pragma surface surf BlinnPhong alpha
		#pragma target 2.0	
		#pragma multi_compile Rendered_Texture Cubemap_T Cubemap_Assigned Both_T Off_T
		struct Input
	{
		float2 uv_WheelTexture;
		float3 worldRefl;
		float3 viewDir;
		INTERNAL_DATA
	};
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
	sampler2D _RenderedTexture;
	sampler2D _WheelTexture;
	samplerCUBE _Cube;
	float4 _WheelColor;
	float _DiffuseUVScale;
	float _Transparency;
	float _RefVisibility;
	float _Transprnt = 0;
	float _RefIntensity;
	float _BlurLevel;
	//uniform float4x4 _Rotation;
	void surf(Input IN, inout SurfaceOutput s)
	{
		s.Normal = normalize(float3(0, 0, 1));
		// texture
		float3 worldVec = WorldReflectionVector(IN, s.Normal);
		float4 WheelTexture = tex2D(_WheelTexture, IN.uv_WheelTexture);
		float4 WheelTexture2 = tex2D(_WheelTexture, IN.uv_WheelTexture + float2(0.5,0));
		float4 WheelTex = lerp(WheelTexture, WheelTexture2, _BlurLevel);

		float4 _RenderedTxt = tex2D(_RenderedTexture, worldVec);
		float wheelSpecularMask = WheelTex.a;
		float4 WheelDiffuse = ((_WheelColor * wheelSpecularMask)  * WheelTex);

		// reflection
		float4 cubemapTexture = texCUBE(_Cube, worldVec/*mul(_Rotation, worldVec)*/); // user set cubemap
		float4 cubemapTexture2 = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, worldVec); // unity assigned cubemap

		// both
		#if Both_T
		float4 reflectionResult = _RefIntensity * cubemapTexture * (_RenderedTxt - (-1 * cubemapTexture.a));
		s.Emission = reflectionResult * pow((1.0 - dot(normalize(IN.viewDir), s.Normal)), _RefVisibility);
		s.Alpha = (_Transprnt + wheelSpecularMask) * _Transparency;
		#endif
		// rendered texture only
		#if Rendered_Texture
		float4 reflectionResult = _RefIntensity * _RenderedTxt * wheelSpecularMask;
		s.Emission = reflectionResult * pow((1.0 - dot(normalize(IN.viewDir), s.Normal)), _RefVisibility);
		s.Alpha = (_Transprnt + wheelSpecularMask) * _Transparency;
		#endif
		// cubemap only
		#if Cubemap_T
		float4 reflectionResult = _RefIntensity * cubemapTexture * wheelSpecularMask;
		s.Emission = reflectionResult * pow((1.0 - dot(normalize(IN.viewDir), s.Normal)), _RefVisibility);
		s.Alpha = (_Transprnt + wheelSpecularMask )* _Transparency;
		#endif
		// assigned cubemap only
		#if Cubemap_Assigned
		float4 reflectionResult = _RefIntensity * cubemapTexture2 * wheelSpecularMask;
		s.Emission = reflectionResult * pow((1.0 - dot(normalize(IN.viewDir), s.Normal)), _RefVisibility);
		s.Alpha = (_Transprnt + wheelSpecularMask) * _Transparency;
		#endif
		// reflection is off
		#if Off_T
		s.Alpha = (_Transprnt + wheelSpecularMask )* _Transparency;
		#endif	

		// combine everything
		s.Albedo = (1 - wheelSpecularMask) + WheelDiffuse;
	}
	ENDCG
	}
		FallBack "Standard"
		CustomEditor "SkrilStudio.VehicleBluredWheel_Editor"
}
