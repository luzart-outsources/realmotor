using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastForward : RaycastLayer
{
    public MotorMovementUnPhysic movementUnPhysic;
    public override ResultRaycast GetResultRaycast()
    {
        float distance = Time.deltaTime* movementUnPhysic.currentSpeed ;
        bool isRayHit = Physics.Raycast(transform.position , Vector3.forward, out hit, distance, layerMask);
        if (isRayHit)
        {
            resultRaycast = new ResultRaycast();
            resultRaycast.hit = hit;
            resultRaycast.eLayer = eLayer;
            return resultRaycast;
        }
        else
        {
            resultRaycast = null;
            return null;
        }
    }
    //private void OnDrawGizmos()
    //{
    //    Vector3 extends = boxCollider.size;
    //    extends += velocity * Time.deltaTime * Vector3.one;
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireCube(transform.position + boxCollider.center,extends);
    //}
}
