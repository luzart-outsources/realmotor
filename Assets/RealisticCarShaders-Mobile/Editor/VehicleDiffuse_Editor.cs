﻿//______________________________________________//
//________Realistic Car Shaders - Mobile________//
//______________________________________________//
//_______Copyright © 2024 Skril Studio__________//
//______________________________________________//
//__________ http://skrilstudio.com/ ___________//
//______________________________________________//
//________ http://fb.com/yugelmobile/ __________//
//______________________________________________//

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SkrilStudio
{
    public class VehicleDiffuse_Editor : ShaderGUI
    {
        private static readonly string _logoImagePath = "Assets/RealisticCarShaders-Mobile/Editor/logoDiffuse.png";
        MaterialProperty _Color;
        MaterialProperty _MainTex;
        MaterialProperty _DiffuseBumpMap;
        MaterialProperty _DiffuseUVScale;
        MaterialProperty _Cube;
        MaterialProperty _RenderedTexture;
        MaterialProperty _RefIntensity;
        MaterialProperty _RefVisibility;

        public enum ReflectionType
        {
            RenderedTextureReflection,
            CubemapReflection,
            AssignedCubemapReflection,
            BothReflections,
            TurnedOff
        }
        public ReflectionType reflectionType;

        private MaterialEditor materialEditor;
        private MaterialProperty[] materialProperties;
        private Material _material;
        private Color customUIColor;

        private bool firstApply = true,
        BodyUVFold, DiffuseBump, DiffuseReflect;

        // custom logo
        readonly Texture texLogo = AssetDatabase.LoadAssetAtPath<Texture>(_logoImagePath);
        public Rect logoRect;

        public override void OnGUI(MaterialEditor _materialEditor, MaterialProperty[] _materialProperties)
        {
            EditorGUI.BeginChangeCheck();
            if (firstApply)
            {
                materialEditor = _materialEditor;
                firstApply = false;
                customUIColor = new Color(.6f, .6f, .6f);
            }
            materialProperties = _materialProperties;
            _material = materialEditor.target as Material;
            if (!materialEditor.isVisible)
                return;
            GetValues();

            logoRect.height = texLogo.height;
            logoRect.width = texLogo.width;
            GUILayout.BeginHorizontal();
            GUILayout.Space((EditorGUIUtility.currentViewWidth - texLogo.width - 10f) / 2f);
            GUILayout.Label(texLogo, GUILayout.Width(logoRect.width - 25f), GUILayout.Height(logoRect.height));
            GUILayout.EndHorizontal();
            EditorGUI.indentLevel++;

            // material preview
            EditorGUILayout.BeginVertical(GUI.skin.textArea);
            materialEditor.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(200, 200), EditorStyles.toolbar);
            GUI.backgroundColor = customUIColor;
            EditorGUILayout.EndVertical();

            // material preview buttons
            GUILayout.BeginHorizontal(GUI.skin.button);
            materialEditor.DefaultPreviewSettingsGUI();
            GUI.backgroundColor = customUIColor;
            GUILayout.EndHorizontal();

            GUI.backgroundColor = new Color(.7f, .7f, .7f);

            EditorGUILayout.BeginVertical(GUI.skin.button);
            ShowProperties();
            EditorGUILayout.EndVertical();
        }

        void GetValues()
        {
            _Color = FindProperty("_Color", materialProperties);
            _MainTex = FindProperty("_MainTex", materialProperties);
            _DiffuseBumpMap = FindProperty("_DiffuseBumpMap", materialProperties);
            _DiffuseUVScale = FindProperty("_DiffuseUVScale", materialProperties);
            _Cube = FindProperty("_Cube", materialProperties);
            _RenderedTexture = FindProperty("_RenderedTexture", materialProperties);
            _RefIntensity = FindProperty("_RefIntensity", materialProperties);
            _RefVisibility = FindProperty("_RefVisibility", materialProperties);
            // enum
            if (_material.IsKeywordEnabled("Rendered_Texture"))
                reflectionType = ReflectionType.RenderedTextureReflection;
            if (_material.IsKeywordEnabled("Cubemap_T"))
                reflectionType = ReflectionType.CubemapReflection;
            if (_material.IsKeywordEnabled("Cubemap_Assigned"))
                reflectionType = ReflectionType.AssignedCubemapReflection;
            if (_material.IsKeywordEnabled("Both_T"))
                reflectionType = ReflectionType.BothReflections;
            if (_material.IsKeywordEnabled("Off_T"))
                reflectionType = ReflectionType.TurnedOff;
            // toggle
            if (_material.IsKeywordEnabled("Bumped_Diffuse"))
                DiffuseBump = true;
            else
                DiffuseBump = false;
            // diffuse reflection
            if (_material.IsKeywordEnabled("DiffuseReflectOff"))
                DiffuseReflect = true;
            else
                DiffuseReflect = false;
        }

        void ShowProperties()
        {
            GUI.backgroundColor = customUIColor;
            EditorGUILayout.Space();

            // reflection settings
            EditorGUILayout.HelpBox("Reflection", MessageType.None);
            EditorGUILayout.Space();
            reflectionType = (ReflectionType)EditorGUILayout.EnumPopup("Reflection Type", reflectionType);
            // enum
            if (reflectionType != ReflectionType.TurnedOff)
            {
                materialEditor.ShaderProperty(_RefIntensity, "Reflection Intensity");
                materialEditor.ShaderProperty(_RefVisibility, "Reflection Visibility Scale");
            }
            else
            {
                _material.DisableKeyword("Rendered_Texture");
                _material.DisableKeyword("Cubemap_T");
                _material.DisableKeyword("Cubemap_Assigned");
                _material.DisableKeyword("Both_T");
                _material.EnableKeyword("Off_T");
            }
            if (reflectionType == ReflectionType.RenderedTextureReflection)
            {
                _material.EnableKeyword("Rendered_Texture");
                _material.DisableKeyword("Cubemap_T");
                _material.DisableKeyword("Cubemap_Assigned");
                _material.DisableKeyword("Both_T");
                _material.DisableKeyword("Off_T");
                materialEditor.TexturePropertySingleLine(new GUIContent("Rendered Texture"), _RenderedTexture);
                DiffuseReflect = EditorGUILayout.Toggle("Diffuse Reflection", DiffuseReflect);
            }
            if (reflectionType == ReflectionType.CubemapReflection)
            {
                _material.DisableKeyword("Rendered_Texture");
                _material.EnableKeyword("Cubemap_T");
                _material.DisableKeyword("Cubemap_Assigned");
                _material.DisableKeyword("Both_T");
                _material.DisableKeyword("Off_T");
                materialEditor.TexturePropertySingleLine(new GUIContent("Reflection Cubemap"), _Cube);
                DiffuseReflect = EditorGUILayout.Toggle("Diffuse Reflection", DiffuseReflect);
            }
            if (reflectionType == ReflectionType.AssignedCubemapReflection)
            {
                _material.DisableKeyword("Rendered_Texture");
                _material.DisableKeyword("Cubemap_T");
                _material.EnableKeyword("Cubemap_Assigned");
                _material.DisableKeyword("Both_T");
                _material.DisableKeyword("Off_T");
                DiffuseReflect = EditorGUILayout.Toggle("Diffuse Reflection", DiffuseReflect);
            }
            if (reflectionType == ReflectionType.BothReflections)
            {
                _material.DisableKeyword("Rendered_Texture");
                _material.DisableKeyword("Cubemap_T");
                _material.DisableKeyword("Cubemap_Assigned");
                _material.EnableKeyword("Both_T");
                _material.DisableKeyword("Off_T");
                materialEditor.TexturePropertySingleLine(new GUIContent("Rendered Texture"), _RenderedTexture);
                materialEditor.TexturePropertySingleLine(new GUIContent("Reflection Cubemap"), _Cube);
                DiffuseReflect = EditorGUILayout.Toggle("Diffuse Reflection", DiffuseReflect);
            }
            EditorGUILayout.Space();

            // body settings
            EditorGUILayout.HelpBox("Body", MessageType.None);
            EditorGUILayout.Space();
            DiffuseBump = EditorGUILayout.Toggle("Bump Map", DiffuseBump);
            materialEditor.ShaderProperty(_Color, "Vehicle Color");
            materialEditor.TexturePropertySingleLine(new GUIContent("Diffuse Texture"), _MainTex);
            if (DiffuseBump)
            {
                _material.EnableKeyword("Bumped_Diffuse");
                materialEditor.TexturePropertySingleLine(new GUIContent("Diffuse Bump Map"), _DiffuseBumpMap);
            }
            else
            {
                _material.DisableKeyword("Bumped_Diffuse");
            }

            if (DiffuseReflect)
            {
                _material.EnableKeyword("DiffuseReflectOff");
            }
            else
            {
                _material.DisableKeyword("DiffuseReflectOff");
            }

            materialEditor.ShaderProperty(_DiffuseUVScale, "Diffuse UV Scale");
            BodyUVFold = EditorGUILayout.Foldout(BodyUVFold, "Diffuse UV");
            if (BodyUVFold)
                materialEditor.TextureScaleOffsetProperty(_MainTex);
            EditorGUILayout.Space();

            // render queue
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            materialEditor.RenderQueueField();
            EditorGUILayout.Space();
        }
    }
}