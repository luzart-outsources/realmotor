using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapFinishLine : OverlapEachFrame
{
    public override ResultRaycast GetResultRaycast()
    {
        bool isCollider = false;
        var resultRaycast = new ResultOverlapBool();
        resultRaycast.eLayer = eLayer;
        var colliders = Physics.OverlapBox(transform.position + boxCollider.center, boxCollider.size, Quaternion.identity, layerMask);
        if (colliders != null)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != null)
                {
                    isCollider = true;
                    break;
                }
            }
        }
        if (isCollider)
        {
            resultRaycast.isFinishRaycast = true;
            return resultRaycast;
        }
        else
        {
            resultRaycast.isFinishRaycast = false;
            return resultRaycast;
        }
    }
}
