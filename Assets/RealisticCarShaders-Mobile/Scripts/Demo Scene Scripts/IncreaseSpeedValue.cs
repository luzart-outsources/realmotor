//______________________________________________//
//________Realistic Car Shaders - Mobile________//
//______________________________________________//
//_______Copyright © 2022 Skril Studio__________//
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
    public class IncreaseSpeedValue : MonoBehaviour // this script is made for the drag racing demo scene, it will increase "currentSpeed" value of VehicleBluredWheel.cs script to simulate a vehicles current speed.
    {
        private WheelBlurringController _wheelBlurringCntrl;
        void Start()
        {
            _wheelBlurringCntrl = gameObject.GetComponent<WheelBlurringController>();
        }
        void Update()
        {
            _wheelBlurringCntrl.currentSpeed = Mathf.Clamp(_wheelBlurringCntrl.currentSpeed + (Time.deltaTime*4), 0, 250); // increase currentSpeed value
        }
    }
}
