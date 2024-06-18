using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerAI : BaseController
{
    public Transform[] allPoint;
    public Transform target;
    public float distance;
    private int index;
    public float deltaRotate = 10f;
    public override void Initialized(BaseMotorbike baseMotorbike)
    {
        base.Initialized(baseMotorbike);
        GetTransformNearest();
    }
    private void GetTransformNearest()
    {
        target = allPoint[0];
        float distance = 10000000;
        for (int i = 0; i < allPoint.Length; i++)
        {
            float dis = Vector3.Distance(transform.position, allPoint[i].position);
            if (dis < distance)
            {
                distance = dis;
                target = allPoint[i];
                index = i;
            }
        }
    }
    private void GetTransformTarget()
    {
        distance = Vector3.Distance(transform.position, target.position);
        if (distance <= 3f)
        {
            index++;
            if (index >= allPoint.Length)
            {
                index = 0;
            }
            target = allPoint[index];
        }
    }
    public override void UpdateController()
    {
        UpdateRotate();
        GetTransformTarget();
    }
    private void UpdateRotate()
    {
        int preIndex = index - 1;
        if(preIndex < 0)
        {
            preIndex = allPoint.Length - 1;
        }
        int afterIndex = index + 1;
        if(afterIndex > allPoint.Length - 1)
        {
            afterIndex = 0;
        }
        int currentIndex = index;
        Vector3 target = allPoint[currentIndex].position;
        Vector3 direction = target  - transform.position;
        Quaternion quaDir = Quaternion.LookRotation(direction, Vector3.up);
        Vector3 curDirection = transform.forward;
        float angle = Vector3.SignedAngle(direction, curDirection, transform.up);
        float steerInput = Mathf.Sign(angle);
        float currentSpeed = baseMotorBike.inforMotorbike.acceleration * Time.deltaTime*10f;

        Vector3 velocity = transform.forward * currentSpeed;
        var trueVelocity = transform.InverseTransformDirection(velocity);
        MoveSteerVisual((int)steerInput, currentSpeed);
        MoveVisual(trueVelocity);

        transform.position = Vector3.MoveTowards(transform.position, target, currentSpeed);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, quaDir, deltaRotate * Time.deltaTime);
    }
}
