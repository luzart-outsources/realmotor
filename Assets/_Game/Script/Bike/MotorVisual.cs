using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorVisual : MonoBehaviour
{
    public Transform motorVisual;
    [Space, Header("Handle")]
    [SerializeField] 
    private Transform Handle;
    [SerializeField]
    private float maxRotationHandle = 20f;

    [Space, Header("Tilt")]
    [SerializeField]
    private float maxTiltAngle = 50f;
    [SerializeField]
    private float deltaTilt = 60f;


    [Space, Header("Tail")]
    [SerializeField]
    private float maxTailAngle = 10f;
    [SerializeField]
    private float deltaTail = 10f;

    [Space, Header("Wheel")]
    [SerializeField]
    private Transform visualFrontWheel;
    [SerializeField]
    private Transform visualBackWheel;
    [SerializeField]
    private float tyreRotation = 1000f;

    [Space, Header("Drift")]
    [SerializeField]
    private float skidWidth = 0.05f;
    [SerializeField]
    private TrailRenderer skidMark1;
    [SerializeField]
    private TrailRenderer skidMark2;
    [SerializeField]
    private FXDust fxDustDriftFront;
    [SerializeField]
    private FXDust fxDustDriftBack;

    [Space, Header("Ground")]
    [SerializeField]
    private GameObject obDustGroundFront;
    [SerializeField]
    private GameObject obDustGroundBack;
    [SerializeField]
    private FXDust fxDustGroundFront;
    [SerializeField]
    private FXDust fxDustGroundBack;

    private float tiltAmount = 0f; // Góc nghiêng hiện tại của xe
    private float tailAmount = 0f; // Góc nghiêng hiện tại của xe
    private float maxVelocityRotate = 45f;

    public InforMotorbike motorbikeInfo; // Thông tin của xe máy
    public void Initialize(BaseMotor baseMotor)
    {
        motorbikeInfo = baseMotor.baseMotorbike.inforMotorbike;
        skidMark1.startWidth = skidWidth;
        skidMark2.startWidth = skidWidth;
    }
    public void OnShowDustGround(float velocity)
    {
        obDustGroundFront.SetActive(true);
        obDustGroundBack.SetActive(true);
        fxDustGroundFront.gameObject.SetActive(true);
        fxDustGroundBack.gameObject.SetActive(true);
        fxDustGroundFront.EmissionOnVelocity(velocity);
        fxDustGroundBack.EmissionOnVelocity(velocity);

    }
    public void UnEnableFXDustGround()
    {
        obDustGroundFront.SetActive(false);
        obDustGroundBack.SetActive(false);
        fxDustGroundFront.gameObject.SetActive(false);
        fxDustGroundBack.gameObject.SetActive(false);
    }
    public void ControlFXDustDrift(float velocity)
    {
        fxDustDriftFront.gameObject.SetActive(true);
        fxDustDriftBack.gameObject.SetActive(true);
        fxDustDriftFront.EmissionOnVelocity(velocity);
        fxDustDriftBack.EmissionOnVelocity(velocity);
    }
    public void UnEnableFXDustDrift()
    {
        fxDustDriftFront.gameObject.SetActive(false);
        fxDustDriftBack.gameObject.SetActive(false);
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

    public void OnVisualTilt(int steerInput, float currentSpeed)
    {
        // Tính toán góc tilt dựa trên vận tốc hiện tại
        float valueAngle = Mathf.Clamp01(currentSpeed/maxVelocityRotate);
        float _maxTiltAngle = -maxTiltAngle * steerInput * valueAngle;
        //float targetTiltAngle = Mathf.Lerp(0f, maxTiltAngle,Time.deltaTime);
        float factorTitl = 1f;
        if (steerInput == 0)
        {
            factorTitl = 1f;
        }else if(Mathf.Sign(tiltAmount) != Mathf.Sign(_maxTiltAngle))
        {
            factorTitl = 2f;
        }
        Handle.localRotation = Quaternion.Slerp(Handle.localRotation, Quaternion.Euler(Handle.localRotation.eulerAngles.x,
                               maxRotationHandle * steerInput, Handle.localRotation.eulerAngles.z), Time.deltaTime*deltaTilt);
        tiltAmount = Mathf.MoveTowards(tiltAmount, _maxTiltAngle, factorTitl* deltaTilt * Time.deltaTime);
        float _maxTailAngle = maxTailAngle * steerInput * valueAngle;
        tailAmount = Mathf.MoveTowards(tailAmount, _maxTailAngle, factorTitl* deltaTail * Time.deltaTime);
        //tiltAmount = Mathf.Lerp(tiltAmount, maxTiltAngle, deltaTilt * Time.deltaTime); // Sử dụng Lerp để làm mượt hành động nghiêng
        Quaternion targetTilt = Quaternion.Euler(0, tailAmount, tiltAmount);
        motorVisual.localRotation = targetTilt;
    }
}
