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
    public class WheelBlurringController : MonoBehaviour
    {
        public GameObject regularWheel;
        public GameObject blurredWheel;
        public float currentSpeed;
        private Renderer _renderer;

        void Start()
        {
            _renderer = blurredWheel.GetComponent<Renderer>();
            _renderer.material.SetFloat("_Transparency", 0);
        }

        // Update is called once per frame
        void Update()
        {
            // blurred wheel transparency change between 0 km/h and 20 km/h
            _renderer.sharedMaterials[1].SetFloat("_Transparency", Mathf.Clamp(currentSpeed, 0, 20) * 0.05f); // 20 km/h = full visibility, 1.0f / 20.0f = 0.05f
                                                                                                              // blurring level change between 50 km/h and 120 km/h
            _renderer.sharedMaterials[1].SetFloat("_BlurLevel", (Mathf.Clamp(currentSpeed, 20, 120) - 20) * 0.01f); // start increasing blur level above 20 km/h
            if (currentSpeed > 20)
                regularWheel.SetActive(false);
            else
                regularWheel.SetActive(true);
        }
    }
}
