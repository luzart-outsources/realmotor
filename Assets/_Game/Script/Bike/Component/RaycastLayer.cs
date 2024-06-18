using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastLayer : MonoBehaviour
{
    public LayerMask layerMask;
    public ELayerRaycastMotorbike eLayer;
    protected ResultRaycast resultRaycast = null;
    protected RaycastHit hit;

    public virtual ResultRaycast GetResultRaycast()
    {
        bool isRayHit =  Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, Mathf.Infinity, layerMask);
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
    [System.Serializable]
    public class ResultRaycast
    {
        public ELayerRaycastMotorbike eLayer;
        public RaycastHit hit;
        public BaseMotorbike baseMotorbikes; 
    }
}
public enum ELayerRaycastMotorbike
{
    None = 0,
    Road = 1,
    Ground = 2,
    Wall = 3,
    Bike = 4,
}
