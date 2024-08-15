using UnityEngine;

public class HelicopterCamera : MonoBehaviour
{
    public Camera cameraMain;

    public Animator animator;
    public Transform PrimaryTarget;
    public GameObject SecondaryTarget;
    public float distance = 20.0f;
    public float height = 5.0f;
    public float heightDamping = 2.0f;

    public float lookAtHeight = 0.0f;

    public BaseMotorbike baseMotorbike;

    public float rotationSnapTime = 0.3F;

    public float distanceSnapTime;
    public float distanceMultiplier;
    float initialdistanceMultiplier;

    private Vector3 lookAtVector;

    private float usedDistance;

    float wantedRotationAngle;
    float wantedHeight;

    float currentRotationAngle;
    float currentHeight;

    Vector3 wantedPosition;

    private float yVelocity = 0.0F;
    private float zVelocity = 0.0F;
    PerfectMouseLook perfectMouseLook;
    float LateRot;
    public bool counterRotation;
    //MotorbikeController motorbikeController;
    bool changed, prevFallen;


    void Start()
    {
        //perfectMouseLook = GetComponent<PerfectMouseLook>();
        initialdistanceMultiplier = distanceMultiplier;

        //motorbikeController = FindObjectOfType<MotorbikeController>();
    }
    public void SetTargetFollow(Transform target)
    {
        PrimaryTarget = target;
        if (PrimaryTarget != null)
        {
            baseMotorbike = PrimaryTarget.GetComponent<BaseMotorbike>();
        }

        IsFirstTimeFinish = false;
        IsFirstTimeNone = false;
        IsFirstTimeStart = false;
    }

    void LateUpdate()
    {
        //if(motorbikeController.fallen!=prevFallen)
        //changed = false;
        //prevFallen = motorbikeController.fallen;

        //if (motorbikeController.fallen && changed == false)
        //{
        //    if(SecondaryTarget == null)
        //    target = GameObject.FindGameObjectWithTag("Ragdoll").transform.Find("Armature/Hips");
        //    parentRigidbody = target.gameObject.GetComponent<Rigidbody>();
        //    changed = true;
        //}

        //else if(motorbikeController.fallen == false && changed == false)
        //{
        //    target = PrimaryTarget.transform;
        //    parentRigidbody = PrimaryTarget.GetComponent<Rigidbody>();
        //    changed = true;
        //}

        if (PrimaryTarget == null)
        {
            return;
        }

        if (baseMotorbike == null)
        {
            CameraNone();
            return;
        }
        switch (baseMotorbike.eState)
        {
            case EStateMotorbike.None:
                {
                    CameraNone();
                    break;
                }
            case EStateMotorbike.Start:
                {
                    CameraStart();
                    break;
                }
            case EStateMotorbike.Finish:
                {
                    CameraNone();
                    break;
                }
        }


    }
    [SerializeField]
    private LayerMask _layerGround;
    private LayerMask layerGround
    {
        get
        {
            if (_layerGround == default)
            {
                LayerMask layerGround = LayerMask.NameToLayer("Ground");
                LayerMask layerRoad = LayerMask.NameToLayer("Road");
                _layerGround |= (1 << layerGround);
                _layerGround |= (1 << layerRoad);
            }
            return _layerGround;
        }
    }
    private float YPosCamera(Vector3 pos)
    {
        RaycastHit hit;
        bool isRay = Physics.Raycast(pos, Vector3.down, out hit, 100f, layerGround);
        if (isRay)
        {
            if (hit.distance <= 1f)
            {
                return hit.point.y + 1f;
            }
        }
        return pos.y;
    }
    public Vector3 GetTargetPosition(Transform target)
    {
        float wantedHeight = target.transform.position.y + height;
        float wantedRotationAngle = target.transform.eulerAngles.y;

        Vector3 wantedPosition = target.transform.position;
        wantedPosition.y = wantedHeight;

        float adjustedDistance = distance;

        wantedPosition += Quaternion.Euler(0, wantedRotationAngle, 0) * new Vector3(0, 0, -adjustedDistance);

        wantedPosition.y = YPosCamera(wantedPosition);

        return wantedPosition;
    }
    public Quaternion GetRotation(Transform target)
    {
        Vector3 lookAtVector = new Vector3(0, lookAtHeight, 0);
        Vector3 lookAtPosition = target.transform.position + lookAtVector;
        return Quaternion.LookRotation(lookAtPosition - wantedPosition);
    }
    private void CameraNone()
    {
        LerpCamera();
        LookAtFlash();
    }
    private void CameraStart()
    {
        LerpCamera();
        LookAtSmooth();
    }
    private void LerpCamera()
    {
        wantedHeight = PrimaryTarget.transform.position.y + height;
        currentHeight = transform.position.y;

        wantedRotationAngle = PrimaryTarget.transform.eulerAngles.y;
        currentRotationAngle = transform.eulerAngles.y;
        //if (counterRotation)
        //{
        //    if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        //        LateRot += 0.333f;
        //    else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        //        LateRot -= 0.333f;
        //    else
        //        LateRot = Mathf.Lerp(LateRot, 0, 0.1f);
        //    LateRot = Mathf.Clamp(LateRot, -50 / (parentRigidbody.velocity.magnitude / 20) + 1, 50 / (parentRigidbody.velocity.magnitude / 20) + 1);
        //}
        currentRotationAngle = Mathf.SmoothDampAngle(currentRotationAngle, wantedRotationAngle + LateRot, ref yVelocity, rotationSnapTime);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        wantedPosition = PrimaryTarget.transform.position;
        wantedPosition.y = currentHeight;
        float speed = 0;
        if (baseMotorbike != null)
        {
            speed = baseMotorbike.Speed / baseMotorbike.inforMotorbike.maxSpeed;
        }
        rotationSnapTime = maxRotationSnaptime - speed * (maxRotationSnaptime - mínRotationSnaptime);
        speed = Mathf.Clamp(speed, 0, 1) * 10;


        usedDistance = Mathf.SmoothDampAngle(usedDistance, distance + (speed * distanceMultiplier), ref zVelocity, distanceSnapTime);

        wantedPosition += Quaternion.Euler(0, currentRotationAngle, 0) * new Vector3(0, 0, -usedDistance);

        wantedPosition.y = YPosCamera(wantedPosition);

        transform.position = wantedPosition;
    }
    private void LookAtFlash()
    {
        lookAtVector = new Vector3(0, lookAtHeight, 0);

        transform.LookAt(PrimaryTarget.transform.position + lookAtVector);
    }
    private void LookAtSmooth()
    {
        // Tính toán vị trí nhìn của camera
        lookAtVector = new Vector3(0, lookAtHeight, 0);
        Vector3 lookAtPosition = PrimaryTarget.transform.position + lookAtVector;

        // Sử dụng Quaternion.Slerp để làm mượt góc quay của camera
        Quaternion targetRotation = Quaternion.LookRotation(lookAtPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSnapTime);
    }



    private bool IsFirstTimeNone = false;
    private bool IsFirstTimeStart = false;
    private bool IsFirstTimeFinish = false;
    public float mínRotationSnaptime = 0.5f;
    public float maxRotationSnaptime = 2f;
}