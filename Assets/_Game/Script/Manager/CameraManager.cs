namespace Luzart
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    
    public class CameraManager : Singleton<CameraManager>
    {
        public Light lightMotor;
        public HelicopterCamera helicopterCamera;
        //public SpriteRenderer spBlack;
        public void SetFollowCamera(Transform target)
        {
            helicopterCamera.SetTargetFollow(target);
        }
        public void SetFollowCamera(Transform target, BaseMotorbike baseMotorbike)
        {
            helicopterCamera.SetTargetFollow(target);
            helicopterCamera.baseMotorbike = baseMotorbike;
        }
    }
}
