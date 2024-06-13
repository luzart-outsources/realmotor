using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorVisual : MonoBehaviour
{
    [SerializeField]
    private Transform visualFrontWheel;
    [SerializeField]
    private Transform visualBackWheel;
    [SerializeField]
    private float tyreRotation = 1000f;
    [SerializeField]
    private float skidWidth = 0.05f;
    [SerializeField]
    private TrailRenderer skidMark1;
    [SerializeField]
    private TrailRenderer skidMark2;
    public void Initialize()
    {
        skidMark1.startWidth = skidWidth;
        skidMark2.startWidth = skidWidth;
    }
    public void RotateFrontWheel(float velocity)
    {
        visualFrontWheel.Rotate(Vector3.right, velocity * tyreRotation * Time.fixedTime);
    }
    public void RotateBackWheel(float velocity)
    {
        visualBackWheel.Rotate(Vector3.right, velocity * tyreRotation * Time.fixedTime);
    }
    public void RotateWheel(float velocity)
    {
        RotateFrontWheel(velocity);
        RotateBackWheel(velocity);
    }

    public void SkidMarks()
    {
        skidMark1.emitting = true;
        skidMark2.emitting = true;
    }
    public void BackSkidMarks()
    {
        skidMark1.emitting = false;
        skidMark2.emitting = false;
    }
}
