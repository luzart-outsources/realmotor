using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastLayer : MonoBehaviour
{
    public LayerMask layerMask;
    public ELayerRaycastMotorbike eLayer;
    protected ResultRaycast resultRaycast = null;
    protected RaycastHit hit;

    public virtual bool IsRaycast()
    {
        bool isRayHit = Physics.Raycast(transform.position + Vector3.up*10f, Vector3.down, out hit, Mathf.Infinity, layerMask);
        return isRayHit;
    }

    public virtual ResultRaycast GetResultRaycast()
    {
        bool isRayHit = IsRaycast();
        IsCheckEditor(isRayHit);
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
#if UNITY_EDITOR
    public bool isRaycast = false;
#endif
    protected void IsCheckEditor(bool isCheck)
    {
#if UNITY_EDITOR
        if (isCheck)
        {
            isRaycast = true;
        }
        else
        {
            isRaycast = false;
        }
#endif
    }


    [System.Serializable]
    public class ResultRaycast
    {
        public ELayerRaycastMotorbike eLayer;
        public RaycastHit hit;
        public BaseMotorbike baseMotorbikes; 
    }
    [System.Serializable]
    public class ResultOverlapBool : ResultRaycast
    {
        public bool isFinishRaycast = false;
    }
}
public enum ELayerRaycastMotorbike
{
    None = 0,
    Road = 1,
    Ground = 2,
    Wall = 3,
    Bike = 4,
    FinishLine = 5,
}
