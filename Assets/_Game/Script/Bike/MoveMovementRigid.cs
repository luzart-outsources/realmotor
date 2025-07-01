namespace Luzart
{
    using System;
    using Unity.VisualScripting;
    using UnityEngine;
    
    public class MoveMovementRigid : MotorMovement
    {
        public Transform motorbikeTransform
        {
            get
            {
                return transform;
            }
        }
        public InforMotorbike motorbikeInfo; // Thông tin của xe máy
        private Rigidbody _rb = null;
        public Rigidbody rb
        {
            get
            {
                if(_rb == null)
                {
                    _rb = GetComponent<Rigidbody>();
                }
                return _rb;
            }
        }
    
        public float reverseSpeed = 10f;
        public float drag = 20f;
    
        public RaycastLayer[] raycastDowns;
        public OnCollisionLayer[] onCollisionLayers; 
    
        private void Awake()
        {
            raycastDowns = GetComponents<RaycastLayer>();
            onCollisionLayers = GetComponents<OnCollisionLayer>();
        }
        public override void Initialize(BaseMotor baseMotor)
        {
            base.Initialize(baseMotor);
            motorbikeInfo = baseMotor.baseMotorbike.inforMotorbike;
            IsDead = false;
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
            drag = Mathf.Clamp(drag, 0f, motorbikeInfo.maxSpeed / 10);
            InitOnCollisionLayer();
    
        }
        private void InitOnCollisionLayer()
        {
            if(onCollisionLayers != null)
            {
                int length = onCollisionLayers.Length;
                for (int i = 0; i < length; i++)
                {
                    var colLayer = onCollisionLayers[i];
                    if (colLayer != null)
                    {
                        if(colLayer.actionOnCollisionLayer != null)
                        {
                            colLayer.actionOnCollisionLayer = null;
                        }
                        colLayer.actionOnCollisionLayer += ActionOnCollisionLayer;
                    }
                }
            }
        }
    
        protected override void UpdateMovement()
        {
            base.UpdateMovement();
    
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
        public float[] factorGear = new float []{ 1f , 0.7f, 0.3f };
        private void CalculatorPosition()
        {
            float deltaTime = Time.fixedDeltaTime;
            // Xử lý tăng tốc và phanh
            if (moveInput > 0)
            {
                ContrainRigidbody(RigidbodyConstraints.None);
                float acce = motorbikeInfo.acceleration;
                if(baseMotor.baseMotorbike.currentGear <= 1)
                {
                    acce = acce * factorGear[0];
                }
                else if(baseMotor.baseMotorbike.currentGear == 2)
                {
                    acce = acce * factorGear[1];
                }
                else
                {
                    acce = acce * factorGear[2];
                }
                //float targetSpeed = motorbikeInfo.maxSpeed * 0.3f;
                //if (currentSpeed < targetSpeed)
                //{
                //    acce = (3 - 2 * Mathf.Lerp(currentSpeed, 0, targetSpeed) / targetSpeed) * acce;
                //}
                currentSpeed += acce * deltaTime * moveInput;
                rb.interpolation = RigidbodyInterpolation.Interpolate;
            }
            else if (moveInput < 0)
            {
                ContrainRigidbody(RigidbodyConstraints.None);
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
                if (Mathf.Abs(currentSpeed) <= 5)
                {
                    ContrainRigidbody(RigidbodyConstraints.FreezeAll);
                }
            }
            if(curTimeSpeed >= 0.5f)
            {
                maxSpeedRandom = UnityEngine.Random.Range((int)motorbikeInfo.maxSpeed - 1, (int)motorbikeInfo.maxSpeed + 1);
                curTimeSpeed = 0;
            }
    
            // Giới hạn tốc độ của xe
            currentSpeed = Mathf.Clamp(currentSpeed, -reverseSpeed, maxSpeedRandom);
    
    
            // Di chuyển xe theo hướng phía trước
            velocity = motorbikeTransform.forward * currentSpeed;
            curTimeSpeed += Time.fixedDeltaTime;
    
        }
        private float maxSpeedRandom = 100;
        private float curTimeSpeed = 0;
        public void ContrainRigidbody(RigidbodyConstraints constraints)
        {
            if (isCollisionBike)
            {
                return;
            }
            rb.constraints = constraints;
            //if (freeze)
            //{
            //    rb.constraints = RigidbodyConstraints.FreezeAll;
            //}
            //else
            //{
            //    rb.constraints = RigidbodyConstraints.None;
            //}
        }
        public Vector3 trueVelocity;
        private void OnChangePosition()
        {
            //trueVelocity = transform.InverseTransformDirection(velocity);
            //trueVelocity.y = 0;
            //velocity = transform.TransformDirection(trueVelocity);
            velocity = transform.forward * currentSpeed;
            trueVelocity = velocity;
            ActionOnMove?.Invoke(trueVelocity);
            rb.velocity = velocity / 2;
            //Vector3 pos = motorbikeTransform.position + velocity * Time.fixedDeltaTime / 2;
            //motorbikeTransform.position = pos;
        }
        protected override void FixedUpdateMovement()
        {
            if (IsDead)
            {
                return;
            }
            CalculatorPosition();
            CheckRaycast();
            OnRotate();
            if (isOnGround)
            {
                OnChangePosition();
            }
    
        }
        public float forceRotate = 10f;
        public float sensor = 0.1f;
        public float percentSpeed = 0.8f;
        private void OnRotate()
        {
            // Xử lý quay xe
            if (Math.Abs(currentSpeed) >= 3f)
            {
                float handling = motorbikeInfo.handling;
                if(baseMotor.baseMotorbike.eTeam == ETeam.Player && baseMotor.baseMotorbike.eState == EStateMotorbike.None)
                {
                    if (currentSpeed / motorbikeInfo.maxSpeed >= percentSpeed)
                    {
                        handling = handling - sensor * (currentSpeed - percentSpeed * motorbikeInfo.maxSpeed)/ motorbikeInfo.maxSpeed * motorbikeInfo.handling;
                    }
                }
                float turn = 0;
                if (currentSpeed >= 0)
                {
                    turn = steerInput * handling * Time.fixedDeltaTime;
                }
                else
                {
                    turn = -1 * steerInput * handling * Time.fixedDeltaTime;
                }
                Vector3 deltaRotation = new Vector3(0, turn, 0);
    
                //rb.rotation = rb.rotation * deltaRotation;
                transform.Rotate(deltaRotation);
    
    
    
    
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
                //case ELayerRaycastMotorbike.Wall:
                //    {
                //        CheckWall(result);
                //        break;
                //    }
                //case ELayerRaycastMotorbike.Bike:
                //    {
                //        CheckBike(result);
                //        break;
                //    }
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
        private void CheckWall(Collision collision)
        {
            if (IsDead) return;
            if (currentSpeed >= 20)
            {
                CollisionDead();
            }
            else
            {
                var contactPoints = collision.contacts;
                int length = contactPoints.Length;
                float angleThreshold = 20f;
                Vector3 relativeVelocity = transform.forward;
                for (int i = 0; i < length; i++)
                {
                    var contact = contactPoints[i];
                    Vector3 contactNormal = contact.normal;
    
                    // Tính góc giữa vận tốc tương đối và pháp tuyến
                    float angle = Vector3.Angle(relativeVelocity, contactNormal);
    
    
                    // Kiểm tra xem góc có gần vuông góc hay không
                    if (Mathf.Abs(angle - (angle / 180)*180) < angleThreshold)
                    {
                        CollisionDead();
                    }
                }
            }
    
        }
        private void CollisionDead()
        {
    
            //if (rb == null)
            //{
            //    rb = gameObject.AddComponent<Rigidbody>();
            //}
            if (baseMotor.baseMotorbike.eTeam == ETeam.Player)
                AudioManager.Instance.PlaySFXCrashWall();
            rb.interpolation = RigidbodyInterpolation.None;
            rb.velocity = velocity;
            rb.mass = 100;
            rb.useGravity = true;
            ActionCollisionWall?.Invoke(velocity);
            currentSpeed = 0;
            velocity = Vector3.zero;
            IsDead = true;
        }
        private bool isOnGround = true;
        private float forceGravity = 5f;
        private void Gravity(RaycastLayer.ResultRaycast result)
        {
    
            //if (result.hit.distance <= 1.3f)
            //{
            //    // Vehicle is too high, We apply gravity force
            //    isOnGround = true;
            //    rb.AddForce(Vector3.down * forceGravity * Time.fixedDeltaTime, ForceMode.Acceleration);
            //}
            //else
            //{
    
            //    // we determine the distance between current vehicle height and wanted height
            //    float distanceVehicleToHoverPosition = 1.3f - result.hit.distance;
    
            //    float force = distanceVehicleToHoverPosition * 1000;
    
            //    // we add the hoverforce to the rigidbody
            //    rb.AddForce(Vector3.up * force * Time.fixedDeltaTime, ForceMode.Acceleration);
            //    isOnGround = false;
            //}
            Vector3 groundNormal = result.hit.normal;
            //Vector3 groundPhaptuyen = groundNormal + (Vector3.up - groundNormal)* delta;
            Vector3 forwardDirection = Vector3.Cross(motorbikeTransform.right, groundNormal).normalized;
            motorbikeTransform.rotation = Quaternion.LookRotation(forwardDirection, groundNormal);
            if (result.hit.distance > 1.3f)
            {
                float yPos = (result.hit.point + boxCol.size/2 - boxCol.center).y;
                Vector3 pos = motorbikeTransform.position;
                yPos = Mathf.Lerp(pos.y, yPos, Time.fixedDeltaTime * 10);
                motorbikeTransform.position = new Vector3(pos.x, yPos, pos.z);
            }
            // Nếu không có mặt đất dưới xe, di chuyển xe xuống dưới
    
        }
        private bool isRayBike = false;
        public float delta = 0.1f;
        private void CheckBike(Collision collision)
        {
            if (isCollisionBike)
            {
                return;
            }
            var motorPartner = collision.gameObject.GetComponent<MoveMovementRigid>();
            if(motorPartner!= null)
            {
                if (baseMotor.baseMotorbike.eTeam == ETeam.Player)
                AudioManager.Instance.PlaySFXCrashMotor();
                motorPartner.OnCollisionBike(this);
                OnCollisionBike(this);
            }
        }
        public void OnCollisionBike(MoveMovementRigid moveRigid)
        {
            currentSpeed = currentSpeed * 4 / 5;
            ContrainRigidbody(RigidbodyConstraints.FreezeRotation);
            isCollisionBike = true;
            //rb.AddForce(moveRigid.velocity/2);
            GameUtil.Instance.WaitAndDo(this, 0.5f, UnConstain);
        }
        public bool isCollisionBike = false;
        public void UnConstain()
        {
            isCollisionBike = false;
            ContrainRigidbody(RigidbodyConstraints.None);
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
        public void ActionOnCollisionLayer(ResultOnCollisionLayer result)
        {
            switch(result.layerCheck)
            {
                case LayerCheck.Wall:
                    {
                        CheckWall(result.collision);
                        break;
                    }
                case LayerCheck.Bike:
                    {
                        CheckBike(result.collision);
                        break;
                    }
            }
        }
        public BoxCollider boxCol;
        private void OnDestroy()
        {
            if(GameUtil.Instance != null)
            {
                GameUtil.Instance.StopAllCoroutinesForBehaviour(this);
            }
    
        }
    #if UNITY_EDITOR
        //[Space, Header ("Editor")]
    
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
}
