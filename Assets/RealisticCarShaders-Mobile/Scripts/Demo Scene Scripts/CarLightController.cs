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
using UnityEngine.UI;

namespace SkrilStudio
{
    // this script is made just for demo scenes, it is an example of how to change the emission of car light shader
    public class CarLightController : MonoBehaviour
    {
        public Slider emissionSlider;
        public Material[] lightMaterials;
        public void ChangeLightPower()
        {
            for (int i = 0; i < lightMaterials.Length; i++)
                lightMaterials[i].SetFloat("_Emission", emissionSlider.value);
        }
    }
}
