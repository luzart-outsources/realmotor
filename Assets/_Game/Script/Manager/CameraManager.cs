using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    public HelicopterCamera helicopterCamera;
    public void SetFollowCamera(GameObject target)
    {
        helicopterCamera.SetTargetFollow(target.transform);
    }
}
