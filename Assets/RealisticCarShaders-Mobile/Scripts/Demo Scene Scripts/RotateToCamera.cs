//______________________________________________//
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
using UnityEngine;

namespace SkrilStudio
{
    public class RotateToCamera : MonoBehaviour // This script rotates the reflection renderer camera to the correct orientation to simulate accurate looking reflections
    {
        public GameObject mainCamera;
        void Start()
        {
            if (mainCamera == null)
                mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        void LateUpdate()
        {
            if (mainCamera != null)
            {
                gameObject.transform.LookAt(mainCamera.transform);
            }
            else
            {
                Debug.LogError("Could not find Main Camra. Please add a Main Camera or set it manually for -Reflection Camera- gameobject.", mainCamera);
            }
        }
    }
}