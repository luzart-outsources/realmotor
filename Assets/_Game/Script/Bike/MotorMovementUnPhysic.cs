using System;
using UnityEngine;

public class MotorMovementUnPhysic : MotorMovement
{
    public Transform motorbikeTransform; // Transform của xe máy
    public InforMotorbike motorbikeInfo; // Thông tin của xe máy
    public Rigidbody rb;

    public float reverseSpeed = 10f;
    public float drag = 20f;

    public RaycastLayer[] raycastDowns;


    public override void Initialize(BaseMotor baseMotor)
    {
        base.Initialize(baseMotor);
        motorbikeInfo = baseMotor.baseMotorbike.inforMotorbike;
        raycastDowns = gameObject.GetComponents<RaycastLayer>();
        if(rb != null)
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            Destroy(rb);
        }
        IsDead = false;

        drag = Mathf.Clamp(drag, 0f, motorbikeInfo.maxSpeed/10);
    }
    protected override void UpdateMovement()
    {
        base.UpdateMovement();
        CalculatorPosition();
        CheckRaycast();
        OnRotate();

        OnChangePosition();
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
        // Xử lý tăng tốc và phanh
        if (moveInput > 0)
        {
            currentSpeed += motorbikeInfo.acceleration * Time.deltaTime * moveInput;
        }
        else if (moveInput < 0)
        {
            if (currentSpeed > 0)
            {
                // Phanh
                currentSpeed -= motorbikeInfo.brake * Time.deltaTime * Mathf.Abs(moveInput);
                ActionOnBrake?.Invoke(velocity);
                if (Mathf.Abs(currentSpeed) <= 0.5f)
                {
                    currentSpeed = 0;
                }
            }
            else if(IsBack)
            {
                // Lùi từ từ
                currentSpeed -= motorbikeInfo.acceleration * Time.deltaTime * Mathf.Abs(moveInput);
            }
        }
        else
        {
            // Giảm tốc từ từ khi không có đầu vào
            if (currentSpeed > 0)
            {
                currentSpeed -= drag * Time.deltaTime;
            }
            else if (currentSpeed < 0)
            {
                currentSpeed += drag * Time.deltaTime;
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
        ActionOnMove?.Invoke(trueVelocity);
        Vector3 pos = motorbikeTransform.position + velocity * Time.deltaTime;
        motorbikeTransform.position = pos;
    }
    protected override void FixedUpdateMovement()
    {
        //if(rb== null)
        //{
        //    return;
        //}
        //rb.velocity = velocity;
    }
    private void OnRotate()
    {
        // Xử lý quay xe
        if( Math.Abs(currentSpeed) >= 3f)
        {
            float turn = 0;
            if (currentSpeed >= 0)
            {
                turn = steerInput * motorbikeInfo.handling * Time.deltaTime;
            }
            else
            {
                turn = -1 * steerInput * motorbikeInfo.handling * Time.deltaTime;
            }

            motorbikeTransform.Rotate(0, turn, 0);
        }
        ActionTiltRotate?.Invoke(steerInput,currentSpeed);
    }
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
            currentSpeed = Mathf.Lerp(currentSpeed, velocityTarget,(motorbikeInfo.acceleration - acceTarget)*Time.deltaTime);
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
        if(IsDead) return;
        if(rb == null)
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
    private void Gravity(RaycastLayer.ResultRaycast result)
    {
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
}
