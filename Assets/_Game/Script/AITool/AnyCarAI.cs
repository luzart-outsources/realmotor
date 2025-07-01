namespace Luzart
{
    using System;
    using UnityEngine;
    
    #region CUSTOM DRIVING
    
    [Serializable]
    public enum CarDriveTypeAI
    {
        FrontWheelDrive,
        RearWheelDrive,
        FourWheelDrive
    }
    
    [Serializable]
    public enum SpeedTypeAI
    {
        MPH,
        KPH
    }
    
    #endregion
    
    #region WHEELS SETUP
    
    public enum AxlAI
    {
        Front,
        Rear
    }
    public enum TypeAI
    {
        Drive,
        Trailer
    }
    
    [Serializable]
    public struct CarWheelsAI
    {
        public GameObject model;
        public AxlAI axel;
        public TypeAI type;
    }
    
    [Serializable]
    public struct CarWheelsColsAI
    {
        public GameObject collider;
        public AxlAI axel;
        public TypeAI type;
    }
    
    #endregion
    
    #region COLLISION
    
    [Serializable]
    public struct OptionalMeshesAI
    {
        public MeshFilter modelMesh;
        public int loseAftCollisions;
    }
    
    #endregion
    
    #region AI ENUMS
    public enum BrakeCondition
    {
        NeverBrake,
        TargetDirectionDifference,
        TargetDistance,
    }
    
    public enum ProgressStyle
    {
        SmoothAlongRoute,
        PointToPoint,
    }
    
    #endregion
    
    public class AnyCarAI : BaseController
    {
        public InforMotorbike motorbikeInfo
        {
            get
            {
                return baseMotorBike.inforMotorbike;
            }
        }
    
    
        #region INPUT REFERENCES
    
        public float AccelInput { get; private set; }
        public float BrakeInput { get; private set; }
    
        #endregion
    
        #region CUSTOM CONTROLS
    
        public AnimationCurve enginePower;
    
        public float maximumSteerAngle;
    
    
        public int numberOfGears = 5;
    
    
        #endregion
    
        #region UTILITY
    
    
        public float currentSpeed;
        public Vector3 velocity
        {
            get
            {
                return transform.InverseTransformDirection(transform.forward * currentSpeed);
            }
        }
        private float rpmRange = 1f;
        public int currentGear;
        private float gearFactor;
        public bool reverseGearOn;
        public float RPM { get; private set; }
        #endregion
    
    
        #region AI CONTROLLER
    
        public CarAIInputs carAIInputs;
        public CarAIWaipointTracker carAIWaypointTracker;
    
        #region INPUTS
    
        [SerializeField] public BrakeCondition brakeCondition = BrakeCondition.TargetDistance;
    
        [SerializeField][Range(0, 1)] public float cautiousSpeedFactor = 0.05f;
        [SerializeField][Range(0, 180)] public float cautiousAngle = 50f;
        [SerializeField][Range(0, 200)] public float cautiousDistance = 100f;
        [SerializeField] public float cautiousAngularVelocityFactor = 30f;
        [SerializeField][Range(0, 0.1f)] public float steerSensitivity = 0.05f;
        [SerializeField][Range(0, 0.1f)] public float accelSensitivity = 0.04f;
        [SerializeField][Range(0, 1)] public float brakeSensitivity = 1f;
        [SerializeField][Range(0, 10)] public float lateralWander = 3f;
        [SerializeField] public float lateralWanderSpeed = 0.5f;
        [SerializeField][Range(0, 1)] public float wanderAmount = 0.1f;
        [SerializeField] public float accelWanderSpeed = 0.1f;
    
        [SerializeField] public bool isDriving;
        [SerializeField] public Transform carAItarget;
        private GameObject carAItargetObj;
        [SerializeField] public bool stopWhenTargetReached;
        [SerializeField] public float reachTargetThreshold = 2;
    
        #region SENSORS
    
        [SerializeField][Range(15, 50)] public float sensorsAngle;
        [SerializeField] public float avoidDistance = 10;
        [SerializeField] public float brakeDistance = 6;
        [SerializeField] public float reverseDistance = 3;
    
        #endregion
    
        #endregion
    
        #region PERSUIT AI
    
        public bool persuitAiOn;
        public GameObject persuitTarget;
        public float persuitDistance;
    
        #endregion
    
        #region PROGRESS TRACKER
    
        [SerializeField] public ProgressStyle progressStyle = ProgressStyle.SmoothAlongRoute;
        [SerializeField] public WaypointsPath AIcircuit;
        [SerializeField][Range(5, 50)] public float lookAheadForTarget = 5;
        [SerializeField] public float lookAheadForTargetFactor = .1f;
        [SerializeField] public float lookAheadForSpeedOffset = 10;
        [SerializeField] public float lookAheadForSpeedFactor = .2f;
        [SerializeField][Range(1, 10)] public float pointThreshold = 4;
        public Transform AItarget;
    
        #endregion
    
        #endregion
    
        void Start()
        {
            #region FEATURES SCRIPTS
    
            if (!gameObject.TryGetComponent<CarAIInputs>(out carAIInputs))
            {
                carAIInputs = gameObject.AddComponent<CarAIInputs>();
            }
            carAIWaypointTracker = gameObject.AddComponent<CarAIWaipointTracker>();
    
            #endregion
    
            #region AI REFERENCES
    
            carAItargetObj = new GameObject("WaypointsTarget");
            carAItargetObj.transform.parent = this.transform.GetChild(1);
            carAItarget = carAItargetObj.transform;
    
            #endregion
    
        }
    
        public override void UpdateController()
        {
            #region CURRENT SPEED
            //currentSpeed = motorbikeInfo.maxSpeed / 5;
            #endregion
        }
        public float curSpeed = 0;
        public Vector3 curVelocity;
        public void Move(float steering, float Accel, float footbrake, float handbrake)
        {
            //curSpeed += Accel * Time.deltaTime;
            //// Xử lý tăng tốc và phanh
    
            //if (footbrake > 0)
            //{
            //    if (curSpeed > 0)
            //    {
            //        // Phanh
            //        curSpeed -= motorbikeInfo.brake * Time.deltaTime;
            //        if (Mathf.Abs(curSpeed) <= 0.5f)
            //        {
            //            curSpeed = 0;
            //        }
            //    }
            //    else
            //    {
            //        // Lùi từ từ
            //        curSpeed -= Accel * Time.deltaTime ;
            //    }
            //}
            //else
            //{
            //    // Giảm tốc từ từ khi không có đầu vào
            //    if (curSpeed > 0)
            //    {
            //        curSpeed -= drag * Time.deltaTime;
            //    }
            //    else if (curSpeed < 0)
            //    {
            //        curSpeed += motorbikeInfo.drag * Time.deltaTime;
            //    }
            //    if (Mathf.Abs(curSpeed) <= 0.5f)
            //    {
            //        curSpeed = 0;
            //    }
            //}
            Debug.Log($"curSpeed {curSpeed}  steer {steering} accel {Accel}  footBrake {footbrake}  handBrake {handbrake}");
    
            curVelocity = transform.forward * curSpeed;
            MoveVisual(curVelocity);
            transform.Rotate(0, steering, 0);
            float steer = Mathf.Sign(steering);
            MoveSteerVisual((int)steer, curSpeed);
            transform.position += curVelocity;
    
            //#region GETINPUTS
    
            ////clamp input values
            //steering = Mathf.Clamp(steering, -1, 1);
            //AccelInput = Accel = Mathf.Clamp(Accel, 0, 1);
            //BrakeInput = footbrake = -1 * Mathf.Clamp(footbrake, -1, 0);
            //handbrake = Mathf.Clamp(handbrake, 0, 1);
    
            //#endregion
    
            //#region GEARS
    
            //CalculateRPM();
    
            //#region TRANSMISSION
    
            //AutoGearSystem();
    
            //#endregion
    
            //#endregion
    
            //#region DRIVING
    
            //Steering(steering);
    
            //ApplyDrive(Accel, footbrake);
    
            //MaxSpeedReached();
    
    
            //#endregion
    
            //#region AERODYNAMICS
    
            //AddDownForce();
            //#endregion       
        }
    
        #region DRIVING
        private void Steering(float steering)
        {
            var steerAngle = steering * maximumSteerAngle;
            float turn = steering * motorbikeInfo.handling * Time.deltaTime;
            transform.Rotate(0, turn, 0);
        }
    
        private void ApplyDrive(float Accel, float footbrake)
        {
            currentSpeed += Accel * Time.deltaTime;
    
            #region FOOTBRAKE
    
            if (footbrake > 0)
            {
                if (currentSpeed > 5 && Vector3.Angle(transform.forward, velocity) < 50f)
                {
                    reverseGearOn = false;
                }
                else
                {
                    reverseGearOn = true;
                }
            }
            else
            {
                reverseGearOn = false;
            }
            currentSpeed -= footbrake * Time.deltaTime;
    
            #endregion
            Vector3 pos = transform.position + velocity * Time.deltaTime;
            baseMotorBike.transform.position = pos;
    
        }
    
        private void MaxSpeedReached()
        {
            currentSpeed = motorbikeInfo.maxSpeed;
        }
    
        #endregion
    
        #region GEAR SYSTEM
        private void AutoGearSystem()
        {
            float gearRatio = Mathf.Abs(currentSpeed / motorbikeInfo.maxSpeed);
    
            float gearUp = (1 / (float)numberOfGears) * (currentGear + 1);
            float gearDown = (1 / (float)numberOfGears) * currentGear;
    
            if (currentGear > 0 && gearRatio < gearDown)
            {
                currentGear--;
            }
    
            if (gearRatio > gearUp && (currentGear < (numberOfGears - 1)))
            {
                if (!reverseGearOn)
                {
                    currentGear++;
                }
            }
        }
    
        // Curved Bias towards 1 for a value between 0-1
        private static float BiasCurve(float factor)
        {
            return 1 - (1 - factor) * (1 - factor);
        }
    
    
        // Smooth Lerp with no fixed Boundaries
        private static float SmoothLerp(float from, float to, float value)
        {
            return (1.0f - value) * from + value * to;
        }
    
    
        private void CalculateGearFactor()
        {
            // Smooth Gear Changing
            float f = (1 / (float)numberOfGears);
    
            var targetGearFactor = Mathf.InverseLerp(f * currentGear, f * (currentGear + 1), Mathf.Abs(currentSpeed / motorbikeInfo.maxSpeed));
            gearFactor = Mathf.Lerp(gearFactor, targetGearFactor, Time.deltaTime * 5f);
        }
    
    
        private void CalculateRPM()
        {
            // Calculate engine RPM
            CalculateGearFactor();
            float gearNumFactor;
    
            gearNumFactor = (currentGear / (float)numberOfGears);
    
            var minRPM = SmoothLerp(0f, rpmRange, BiasCurve(gearNumFactor));
            var maxRPM = SmoothLerp(rpmRange, 1f, gearNumFactor);
    
            RPM = SmoothLerp(minRPM, maxRPM, gearFactor);
        }
    
        #endregion
    
        #region AERODYNAMICS
        private void AddDownForce()
        {
            //if (currentSpeed > 0)
            //{
            //    currentSpeed -= motorbikeInfo.drag * Time.deltaTime;
            //}
        }
    
    
        #endregion
    }
}
