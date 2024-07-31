using System;
using UnityEngine;

public class MoveMovementRigid : MotorMovement
{
    public Transform motorbikeTransform; // Transform của xe máy
    public InforMotorbike motorbikeInfo; // Thông tin của xe máy
    public Rigidbody rb;

    public float reverseSpeed = 10f;
    public float drag = 20f;

    public RaycastLayer[] raycastDowns;

    private void Awake()
    {
        raycastDowns = GetComponents<RaycastLayer>();
    }
    public override void Initialize(BaseMotor baseMotor)
    {
        base.Initialize(baseMotor);
        motorbikeInfo = baseMotor.baseMotorbike.inforMotorbike;
        IsDead = false;
        rb.velocity = Vector3.zero;
        drag = Mathf.Clamp(drag, 0f, motorbikeInfo.maxSpeed / 10);
    }
    protected override void UpdateMovement()
    {
        base.UpdateMovement();
        if (baseMotor.baseMotorbike.eTeam == ETeam.Player)
        {
            Debug.Log(rb.velocity);
        }

    }
    public override void Brake()
    {
        base.Brake();
        if (Mathf.Abs(currentSpeed) <= 1f)
        {
            IsBack = true;
        }
        else
        {
            IsBack = false;
        }
    }
    public bool IsBack = false;
    public override void UnVerticle()
    {
        base.UnVerticle();

    }
    private void CalculatorPosition()
    {
        float deltaTime = Time.fixedDeltaTime;
        // Xử lý tăng tốc và phanh
        if (moveInput > 0)
        {
            float acce = motorbikeInfo.acceleration;
            float targetSpeed = motorbikeInfo.maxSpeed * 0.3f;
            if (currentSpeed < targetSpeed)
            {
                acce = (3 - 2 * Mathf.Lerp(currentSpeed, 0, targetSpeed) / targetSpeed) * acce;
            }
            currentSpeed += acce * deltaTime * moveInput;
        }
        else if (moveInput < 0)
        {
            if (currentSpeed > 0)
            {
                // Phanh
                currentSpeed -= motorbikeInfo.brake * deltaTime * Mathf.Abs(moveInput);
                ActionOnBrake?.Invoke(velocity);
                if (Mathf.Abs(currentSpeed) <= 0.5f)
                {
                    currentSpeed = 0;
                }
            }
            else if (IsBack)
            {
                // Lùi từ từ
                currentSpeed -= motorbikeInfo.acceleration * deltaTime * Mathf.Abs(moveInput);
            }
        }
        else
        {
            // Giảm tốc từ từ khi không có đầu vào
            if (currentSpeed > 0)
            {
                currentSpeed -= drag * deltaTime;
            }
            else if (currentSpeed < 0)
            {
                currentSpeed += drag * deltaTime;
            }
            if (Mathf.Abs(currentSpeed) <= 0.5f)
            {
                currentSpeed = 0;
            }
        }

        // Giới hạn tốc độ của xe
        currentSpeed = Mathf.Clamp(currentSpeed, -reverseSpeed, motorbikeInfo.maxSpeed);


        // Di chuyển xe theo hướng phía trước
        velocity = motorbikeTransform.forward * currentSpeed;


    }
    public Vector3 trueVelocity;
    private void OnChangePosition()
    {
        trueVelocity = transform.InverseTransformDirection(velocity);
        trueVelocity.y = 0;
        velocity = transform.TransformDirection(trueVelocity);
        ActionOnMove?.Invoke(trueVelocity);
        rb.velocity = velocity / 2;
        //Vector3 pos = motorbikeTransform.position + velocity * Time.fixedDeltaTime / 2;
        //motorbikeTransform.position = pos;
    }
    protected override void FixedUpdateMovement()
    {
        CalculatorPosition();
        CheckRaycast();
        OnRotate();

        OnChangePosition();
    }
    public float forceRotate = 10f;
    private void OnRotate()
    {
        // Xử lý quay xe
        if (Math.Abs(currentSpeed) >= 3f)
        {
            float turn = 0;
            if (currentSpeed >= 0)
            {
                turn = steerInput * motorbikeInfo.handling * Time.fixedDeltaTime;
            }
            else
            {
                turn = -1 * steerInput * motorbikeInfo.handling * Time.fixedDeltaTime;
            }
            //rb.AddTorque(new Vector3(0, turn* factor, 0));
            motorbikeTransform.Rotate(0, turn, 0);
            rb.AddForce(turn * transform.right * forceRotate);
        }
        ActionTiltRotate?.Invoke(steerInput, currentSpeed);
    }
    public float factor = 10f;
    private bool IsGrounded = true;
    private void CheckRaycast()
    {
        IsGrounded = false;
        int length = raycastDowns.Length;
        for (int i = 0; i < length; i++)
        {
            var item = raycastDowns[i];
            var result = item.GetResultRaycast();
            if (result == null)
            {
                continue;
            }
            SwitchCheckModeRaycast(result);
        }
    }
    private void SwitchCheckModeRaycast(RaycastLayer.ResultRaycast result)
    {
        switch (result.eLayer)
        {
            case ELayerRaycastMotorbike.Road:
                {
                    CheckRoad(result);
                    IsGrounded = true;
                    break;
                }
            case ELayerRaycastMotorbike.Ground:
                {
                    if (!IsGrounded)
                    {
                        CheckGround(result);
                    }
                    break;
                }
            case ELayerRaycastMotorbike.Wall:
                {
                    CheckWall(result);
                    break;
                }
            case ELayerRaycastMotorbike.Bike:
                {
                    CheckBike(result);
                    break;
                }
            case ELayerRaycastMotorbike.FinishLine:
                {
                    CheckFinishLine(result);
                    break;
                }
        }

    }
    private void CheckRoad(RaycastLayer.ResultRaycast result)
    {
        Gravity(result);
        baseMotor.ELayerCurrent = ELayerRaycastMotorbike.Road;

    }
    private void CheckGround(RaycastLayer.ResultRaycast result)
    {
        Gravity(result);

        float velocityTarget = motorbikeInfo.maxSpeed * 0.8f;
        float acceTarget = motorbikeInfo.acceleration * 0.8f;
        currentSpeed += acceTarget * Time.deltaTime * moveInput;
        if (currentSpeed > velocityTarget)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, velocityTarget, (motorbikeInfo.acceleration - acceTarget) * Time.fixedDeltaTime);
        }
        else
        {
            currentSpeed = Mathf.Clamp(currentSpeed, -reverseSpeed, velocityTarget);
        }

        velocity = motorbikeTransform.forward * currentSpeed;
        baseMotor.ELayerCurrent = ELayerRaycastMotorbike.Ground;
    }

    private bool IsDead = false;
    private void CheckWall(RaycastLayer.ResultRaycast result)
    {
        if (IsDead) return;
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.velocity = velocity;
        rb.useGravity = true;
        rb.mass = 100;
        ActionCollisionWall?.Invoke(velocity);
        currentSpeed = 0;
        velocity = Vector3.zero;
        IsDead = true;
    }
    public float forceGravity = 30f;
    private void Gravity(RaycastLayer.ResultRaycast result)
    {
        if (result.hit.distance > 0.1f)
        {
            // Vehicle is too high, We apply gravity force
            rb.AddForce(Vector3.down * forceGravity * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
        //else
        //{

        //    // we determine the distance between current vehicle height and wanted height
        //    float distanceVehicleToHoverPosition = 0.5f - result.hit.distance;

        //    float force = distanceVehicleToHoverPosition * 10;

        //    // we add the hoverforce to the rigidbody
        //    rb.AddForce(Vector3.up * force * Time.fixedDeltaTime, ForceMode.Acceleration);
        //}
        return;
        // Điều chỉnh vị trí và hướng của xe dựa trên độ nghiêng của mặt đất
        Vector3 groundNormal = result.hit.normal;
        Vector3 forwardDirection = Vector3.Cross(motorbikeTransform.right, groundNormal).normalized;
        motorbikeTransform.rotation = Quaternion.LookRotation(forwardDirection, groundNormal);
        // Nếu không có mặt đất dưới xe, di chuyển xe xuống dưới
        motorbikeTransform.position = result.hit.point;
    }
    private bool isRayBike = false;
    private void CheckBike(RaycastLayer.ResultRaycast result)
    {
        //RaycastLayer.ResultOverlapBool resultFinish = result as RaycastLayer.ResultOverlapBool;
        //if (resultFinish == null)
        //{
        //    return;
        //}
        //bool isFinished = resultFinish.isFinishRaycast;
        //if (isRayBike == isFinished)
        //{
        //    return;
        //}
        //isRayBike = isFinished;
        //if (isFinished)
        //{
        //    if (rb == null)
        //    {
        //        rb = gameObject.AddComponent<Rigidbody>();
        //    }
        //}
        //else
        //{
        //    Destroy(rb);
        //}
    }

    private bool isRayFinish = false;
    private void CheckFinishLine(RaycastLayer.ResultRaycast result)
    {
        RaycastLayer.ResultOverlapBool resultFinish = result as RaycastLayer.ResultOverlapBool;
        if (resultFinish == null)
        {
            return;
        }
        bool isFinished = resultFinish.isFinishRaycast;
        if (isRayFinish == isFinished)
        {
            return;
        }
        isRayFinish = isFinished;
        if (isFinished)
        {
            baseMotor.baseMotorbike.OnFinishLine();
        }

    }
#if UNITY_EDITOR
    public BoxCollider boxCol;
    private void OnDrawGizmos()
    {
        if (boxCol == null)
        {
            return;
        }
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(boxCol.transform.position + boxCol.center, boxCol.size);
    }
#endif
}