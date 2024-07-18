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
    public class Rotate : MonoBehaviour
    {
        public float rotateZSpeed = 100f;
        public bool switchRotation = true;
        void Update()
        {
            if (switchRotation)
                gameObject.transform.Rotate(Vector3.up * Time.deltaTime * rotateZSpeed); // made for All Material Types scene
            else
                gameObject.transform.Rotate(Vector3.forward * Time.deltaTime * rotateZSpeed); // made for Realistic Reflection scene
        }
    }
}
