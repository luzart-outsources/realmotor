using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorMovement : MonoBehaviour
{
    protected int moveInput, steerInput;
    protected BaseMotor baseMotor;
    private bool IsInit = false;


    public Action<int, float> ActionTiltRotate = null;
    public Action<Vector3> ActionOnMove = null;
    public Action ActionOnGround = null;
    public Action<Vector3> ActionOnBrake = null;
    public Action<Vector3> ActionCollisionWall = null;

    public float currentSpeed = 0f; // Tốc độ hiện tại của xe

    public Vector3 velocity; // Vận tốc hiện tại


    public virtual void Initialize(BaseMotor baseMotor)
    {
        this.baseMotor = baseMotor;
        IsInit = true;
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
    public virtual void Brake()
    {
        moveInput = -1;
    }
    public virtual void UnHorizontal()
    {
        steerInput = 0;
    }
    public virtual void UnVerticle()
    {
        moveInput = 0;
    }
    private void Update()
    {
        if(!IsInit)
        {
            return;
        }
        UpdateMovement();
    }
    protected virtual void UpdateMovement()
    {

    }
    //public enum groundCheck { rayCast, sphereCaste };
    //public enum MovementMode { Velocity, AngularVelocity };
    //public MovementMode movementMode;
    //public groundCheck GroundCheck;
    //public LayerMask drivableSurface;

    //public float MaxSpeed
    //{
    //    get
    //    {
    //        return baseMotor.baseMotorbike.inforMotorbike.maxSpeed;
    //    }
    //}
    //public float accelaration
    //{
    //    get
    //    {
    //        return baseMotor.baseMotorbike.inforMotorbike.acceleration;
    //    }
    //}
    //public float turn
    //{
    //    get
    //    {
    //        return baseMotor.baseMotorbike.inforMotorbike.handling;
    //    }
    //}

    //public Rigidbody rb, carBody;

    //[HideInInspector]
    //public RaycastHit hit;
    //public AnimationCurve frictionCurve;
    //public AnimationCurve turnCurve;
    //public AnimationCurve leanCurve;
    //public PhysicMaterial frictionMaterial;
    //[Header("Visuals")]
    //public Transform BodyMesh;
    //public Transform Handle;
    //[HideInInspector]
    //public Vector3 carVelocity;

    //[Range(-70, 70)]
    //public float BodyTilt;

    //private float radius;
    //private Vector3 origin;

    //private void Start()
    //{
    //    radius = rb.GetComponent<SphereCollider>().radius;
    //    if (movementMode == MovementMode.AngularVelocity)
    //    {
    //        Physics.defaultMaxAngularSpeed = 150;
    //    }
    //    rb.centerOfMass = Vector3.zero;
    //}
    //private void Update()
    //{
    //    if (!IsInit)
    //    {
    //        return;
    //    }
    //    Visuals();

    //}

    //void FixedUpdate()
    //{
    //    if (!IsInit)
    //    {
    //        return;
    //    }
    //    carVelocity = carBody.transform.InverseTransformDirection(carBody.velocity);

    //    if (Mathf.Abs(carVelocity.x) > 0)
    //    {
    //        //changes friction according to sideways speed of car
    //        frictionMaterial.dynamicFriction = frictionCurve.Evaluate(Mathf.Abs(carVelocity.x / 100));
    //    }


    //    if (grounded())
    //    {
    //        //turnlogic
    //        float sign = Mathf.Sign(carVelocity.z);
    //        float TurnMultiplyer = turnCurve.Evaluate(carVelocity.magnitude / MaxSpeed);
    //        if (moveInput > 0.1f || carVelocity.z > 1)
    //        {
    //            carBody.AddTorque(Vector3.up * steerInput * sign * turn * 10 * TurnMultiplyer);
    //        }
    //        else if (moveInput < -0.1f || carVelocity.z < -1)
    //        {
    //            carBody.AddTorque(Vector3.up * steerInput * sign * turn * 10 * TurnMultiplyer);
    //        }

    //        //brakelogic
    //        if (moveInput < 0)
    //        {
    //            rb.constraints = RigidbodyConstraints.FreezeRotationX;
    //        }
    //        else
    //        {
    //            rb.constraints = RigidbodyConstraints.None;
    //        }

    //        //accelaration logic

    //        if (movementMode == MovementMode.AngularVelocity)
    //        {
    //            if (Mathf.Abs(moveInput) > 0.1f)
    //            {
    //                rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, carBody.transform.right * moveInput * MaxSpeed / radius, accelaration * Time.fixedDeltaTime);
    //            }
    //        }
    //        else if (movementMode == MovementMode.Velocity)
    //        {
    //            if (Mathf.Abs(moveInput) > 0.1f)
    //            {
    //                rb.velocity = Vector3.Lerp(rb.velocity, carBody.transform.forward * moveInput * MaxSpeed, accelaration / 10 * Time.fixedDeltaTime);
    //            }
    //        }

    //        //body tilt
    //        carBody.MoveRotation(Quaternion.Slerp(carBody.rotation, Quaternion.FromToRotation(carBody.transform.up, hit.normal) * carBody.transform.rotation, 0.09f));
    //    }
    //    else
    //    {
    //        carBody.MoveRotation(Quaternion.Slerp(carBody.rotation, Quaternion.FromToRotation(carBody.transform.up, Vector3.up) * carBody.transform.rotation, 0.02f));
    //    }

    //}
    //public void Visuals()
    //{
    //    Handle.localRotation = Quaternion.Slerp(Handle.localRotation, Quaternion.Euler(Handle.localRotation.eulerAngles.x,
    //                           20 * steerInput, Handle.localRotation.eulerAngles.z), 0.1f);
    //    baseMotor.RotationWheel(carVelocity.z);

    //    //Body
    //    if (carVelocity.z > 1)
    //    {
    //        BodyMesh.localRotation = Quaternion.Slerp(BodyMesh.localRotation, Quaternion.Euler(0,
    //                           BodyMesh.localRotation.eulerAngles.y, BodyTilt * steerInput * leanCurve.Evaluate(carVelocity.z / MaxSpeed)), 0.02f);
    //    }
    //    else
    //    {
    //        BodyMesh.localRotation = Quaternion.Slerp(BodyMesh.localRotation, Quaternion.Euler(0, 0, 0), 0.02f);
    //    }
    //    //Visual
    //    if (IsGround)
    //    {
    //        if (Mathf.Abs(carVelocity.x) > 10)
    //        {
    //            baseMotor.Drift();
    //        }
    //        else
    //        {
    //            baseMotor.UnDrift();
    //        }
    //    }
    //    else
    //    {
    //        baseMotor.UnDrift();
    //    }



    //}
    //public bool IsGround = false;
    //public bool grounded() //checks for if vehicle is grounded or not
    //{
    //    origin = rb.position + rb.GetComponent<SphereCollider>().radius * Vector3.up;
    //    var direction = -transform.up;
    //    var maxdistance = rb.GetComponent<SphereCollider>().radius + 0.2f;

    //    if (GroundCheck == groundCheck.rayCast)
    //    {
    //        if (Physics.Raycast(rb.position, Vector3.down, out hit, maxdistance, drivableSurface))
    //        {
    //            IsGround =  true;
    //        }
    //        else
    //        {
    //            IsGround = false;
    //        }
    //    }

    //    else if (GroundCheck == groundCheck.sphereCaste)
    //    {
    //        if (Physics.SphereCast(origin, radius + 0.1f, direction, out hit, maxdistance, drivableSurface))
    //        {
    //            IsGround = true;

    //        }
    //        else
    //        {
    //            IsGround = false;
    //        }
    //    }
    //    else { IsGround = false; }

    //    return IsGround;
    //}
}
