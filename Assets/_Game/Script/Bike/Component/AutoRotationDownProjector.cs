namespace Luzart
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    
    public class AutoRotationDownProjector : MonoBehaviour
    {
        private void FixedUpdate()
        {
            Vector3 euler = transform.eulerAngles;
            float x = Mathf.Clamp(euler.x, 20, 90);
            transform.eulerAngles = new Vector3(x, euler.y, euler.z);
        }
    }
}
