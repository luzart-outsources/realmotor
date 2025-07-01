namespace Luzart
{
    using UnityEngine;
    
    public class LookAtCamera : MonoBehaviour
    {
    
        void FixedUpdate()
        {
            Vector3 direction = CameraManager.Instance.helicopterCamera.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = rotation * Quaternion.Euler(0, 180, 0);
        }
    }
}
