//______________________________________________//
//________Realistic Car Shaders - Mobile________//
//______________________________________________//
//_______Copyright © 2024 Yugel Mobile__________//
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
    // This script is used to change reflection type on runtime of an RCS material that has reflection.
    // Good for that case when you want to use different reflection type for garage menu and for in game map. For example: cubemap reflection in garage and realtime reflection in game map.
    // Attach this script to that gameobject that contain the RCS material, set materialId to the order number of RCS material.
    public class ChangeReflectionType : MonoBehaviour
    {
        private Renderer _renderer;
        public int materialId = 0; // the order number of your RCS material
        void Start()
        {
            _renderer = gameObject.GetComponent<Renderer>();
        }
        private void OnApplicationQuit() // this part is only added because of the demo scene
        {
            RealtimeAndCubemapReflection(); // restoring materials to original their value
        }
        public void CubemapReflection()
        {
            // disable others
            _renderer.sharedMaterials[materialId].DisableKeyword("Cubemap_Assigned");
            _renderer.sharedMaterials[materialId].DisableKeyword("Rendered_Texture");
            _renderer.sharedMaterials[materialId].DisableKeyword("Both_T");
            _renderer.sharedMaterials[materialId].DisableKeyword("Off_T");
            // enable what is needed
            _renderer.sharedMaterials[materialId].EnableKeyword("Cubemap_T");
        }
        public void AssignedCubemapReflection()
        {
            // disable others
            _renderer.sharedMaterials[materialId].DisableKeyword("Cubemap_T");
            _renderer.sharedMaterials[materialId].DisableKeyword("Rendered_Texture");
            _renderer.sharedMaterials[materialId].DisableKeyword("Both_T");
            _renderer.sharedMaterials[materialId].DisableKeyword("Off_T");
            // enable what is needed
            _renderer.sharedMaterials[materialId].EnableKeyword("Cubemap_Assigned");
        }
        public void RealtimeReflection()
        {
            // disable others
            _renderer.sharedMaterials[materialId].DisableKeyword("Cubemap_Assigned");
            _renderer.sharedMaterials[materialId].DisableKeyword("Cubemap_T");
            _renderer.sharedMaterials[materialId].DisableKeyword("Both_T");
            _renderer.sharedMaterials[materialId].DisableKeyword("Off_T");
            // enable what is needed
            _renderer.sharedMaterials[materialId].EnableKeyword("Rendered_Texture");
        }
        public void RealtimeAndCubemapReflection()
        {
            // disable other
            _renderer.sharedMaterials[materialId].DisableKeyword("Cubemap_Assigned");
            _renderer.sharedMaterials[materialId].DisableKeyword("Rendered_Texture");
            _renderer.sharedMaterials[materialId].DisableKeyword("Cubemap_T");
            _renderer.sharedMaterials[materialId].DisableKeyword("Off_T");
            // enable what is needed
            _renderer.sharedMaterials[materialId].EnableKeyword("Both_T");
        }
        public void TurnOffReflections()
        {
            // disable others
            _renderer.sharedMaterials[materialId].DisableKeyword("Cubemap_Assigned");
            _renderer.sharedMaterials[materialId].DisableKeyword("Rendered_Texture");
            _renderer.sharedMaterials[materialId].DisableKeyword("Both_T");
            _renderer.sharedMaterials[materialId].DisableKeyword("Cubemap_T");
            // enable what is needed
            _renderer.sharedMaterials[materialId].EnableKeyword("Off_T");
        }
    }
}
