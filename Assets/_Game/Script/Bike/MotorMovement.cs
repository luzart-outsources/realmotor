using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorMovement : MonoBehaviour
{
    private float bikeXTiltIncrement = 0.05f, zTiltAngle = 75f, gravity = 10f;
    private int moveInput, steerInput;
    private float currentVelocityOffset;
    [SerializeField]
    private float maxVelocityPer = 40;

    private BaseMotor baseMotor;
    [SerializeField]
    private Rigidbody rbData;
    [SerializeField]
    private float norDrag = 2f, driftDrag = 0.5f;

    private bool IsInit = false;

    public void Initialize(BaseMotor baseMotor)
    {
        this.baseMotor = baseMotor;
        rbData.transform.SetParent(baseMotor.baseMotorbike.transform.parent);
        IsInit = true;
        rayLength = rbData.GetComponent<SphereCollider>().radius + 0.5f;
    }

    public void MoveLeft()
    {
        steerInput = -1;
    }
    public void MoveRight()
    {
        steerInput = 1;
    }
    public void MoveUp()
    {
        moveInput = 1;
    }
    public void Brake()
    {
        moveInput = -1;
    }
    public void UnHorizontal()
    {
        steerInput = 0;
    }
    public void UnVerticle()
    {
        moveInput = 0;
    }
    public Vector3 velocity;
    private void FixedUpdate()
    {
        if (!IsInit)
        {
            return;
        }
        Grounded();
        if (IsGrounded)
        {
            Acceletation();
        }
        else
        {
            Gravity();
        }
        Rotation();
        BikeTilt();
        Drift();
        baseMotor.baseMotorbike.transform.position = rbData.transform.position;
        velocity = baseMotor.baseMotorbike.transform.InverseTransformDirection(rbData.velocity);
        currentVelocityOffset = velocity.z;
        baseMotor.RotationWheel(currentVelocityOffset);
    }
    private void Acceletation()
    {
        if (moveInput <= 0)
        {
            return;
        }
        rbData.velocity = Vector3.Lerp(rbData.velocity, baseMotor.baseMotorbike.inforMotorbike.maxSpeed * baseMotor.baseMotorbike.transform.forward, Time.fixedDeltaTime * baseMotor.baseMotorbike.inforMotorbike.acceleration);
    }
    private void Rotation()
    {
        baseMotor.baseMotorbike.transform.Rotate(0, Mathf.Clamp01(currentVelocityOffset / maxVelocityPer) * steerInput * baseMotor.baseMotorbike.inforMotorbike.handling * Time.fixedDeltaTime, 0, Space.World);
    }
    private void BikeTilt()
    {
        float xRot = (Quaternion.FromToRotation(baseMotor.baseMotorbike.transform.up, hit.normal) * baseMotor.baseMotorbike.transform.rotation).eulerAngles.x;
        float zRot = -zTiltAngle * steerInput * Mathf.Clamp01(Mathf.Abs(currentVelocityOffset) / maxVelocityPer);
        Quaternion targetRot = Quaternion.Slerp(baseMotor.baseMotorbike.transform.rotation, Quaternion.Euler(xRot, baseMotor.baseMotorbike.transform.eulerAngles.y, zRot), bikeXTiltIncrement);
        baseMotor.baseMotorbike.transform.rotation = targetRot;
    }
    private void Gravity()
    {
        rbData.AddForce(gravity * Vector3.down, ForceMode.Acceleration);
    }
    private RaycastHit hit;
    public float rayLength;
    [SerializeField]
    private LayerMask layerMask;
    public bool IsGrounded = false;
    private bool Grounded()
    {
        if (Physics.Raycast(rbData.position, Vector3.down, out hit, rayLength, layerMask))
        {
            IsGrounded = true;
        }
        else
        {
            IsGrounded = false;
        }
        return IsGrounded;
    }
    private void Drift()
    {
        if(moveInput == -1)
        {
            if (Mathf.Abs(velocity.z) <= 2f)
            {
                rbData.velocity = Vector3.Lerp(rbData.velocity, moveInput * baseMotor.baseMotorbike.inforMotorbike.maxSpeed * baseMotor.baseMotorbike.transform.forward, Time.fixedDeltaTime * baseMotor.baseMotorbike.inforMotorbike.acceleration);
            }
            else
            {
                rbData.velocity *= (baseMotor.baseMotorbike.inforMotorbike.brake / 100);
                rbData.drag = driftDrag;
                baseMotor.Drift();
            }

            
        }
        else
        {
            rbData.drag = norDrag;
            baseMotor.UnDrift();
        }
    }
}
