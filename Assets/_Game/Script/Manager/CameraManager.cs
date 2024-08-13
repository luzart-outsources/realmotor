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
    //public void FadeOut(float time = 0.1f)
    //{
    //    Color color = spBlack.color;
    //    DOVirtual.Float(1, 0, time, (x) =>
    //    {
    //        color.a = x;
    //        spBlack.color = color;
    //    });
    //}
}
