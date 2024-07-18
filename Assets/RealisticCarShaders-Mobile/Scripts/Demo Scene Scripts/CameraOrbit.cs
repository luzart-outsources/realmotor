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
    public class CameraOrbit : MonoBehaviour
    {
        public float xRot = 0f;
        public float yRot = 0f;

        public float distance = 5f;
        public float sensitivity = 1000f;
        public Transform target;
        public Slider camDistanceSlider;

        private bool isTouching = false;
        private void Start()
        {
            xRot = -0.2f;
            yRot = 128;
            CalculateCamera();
        }

        void Update()
        {
            if (isTouching)
            {
                CalculateCamera();
            }
        }
        public void PointerDown()
        {
            isTouching = true;
        }
        public void PointerUp()
        {
            isTouching = false;
        }
        public void CamDistance()
        {
            distance = camDistanceSlider.value;
            transform.position = target.position + Quaternion.Euler(xRot, yRot, 0f) * (distance * -Vector3.back);
            transform.LookAt(target.position, Vector3.up);
        }
        private void CalculateCamera()
        {
            xRot += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
            yRot += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

            if (xRot > 90f)
            {
                xRot = 90f;
            }
            else if (xRot < -90f)
            {
                xRot = -90f;
            }

            transform.position = target.position + Quaternion.Euler(xRot, yRot, 0f) * (distance * -Vector3.back);
            transform.LookAt(target.position, Vector3.up);
        }
    }
}
