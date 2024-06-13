using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public BaseMotorbike target; // Target to follow
    public Rigidbody rbTarget;
    public Transform obPosNew;
    public float minDistance = 5f; // Minimum distance to the target
    public float maxDistance = 10f; // Maximum distance from the target
    public float speedToDistanceFactor = 1f; // Factor to control how much speed affects the distance
    public float smoothTime = 10f; // Time lag for rotation in seconds
    public float factorToMoveStrange = 10f;
    public float factorToMoveTrue = 10f;

    private Vector3 offset; // Initial offset from the target
    private Vector3 currentVelocity = Vector3.zero;
    private Quaternion targetRotation;

    void Start()
    {
        // Calculate the initial offset
        offset = transform.position - target.transform.position;
        targetRotation = Quaternion.FromToRotation(transform.eulerAngles, target.transform.eulerAngles);
    }

    void LateUpdate()
    {
        float currentSpeed = target.transform.InverseTransformDirection(rbTarget.velocity).z;
        float distance = Mathf.Lerp(minDistance, maxDistance, currentSpeed * speedToDistanceFactor / maxDistance);
        Vector3 newOffset = offset/*.normalized * distance*/;
        Vector3 posNew = target.transform.rotation * newOffset + target.transform.position;
        transform.position = posNew;
        transform.LookAt(target.transform);
    }
}
