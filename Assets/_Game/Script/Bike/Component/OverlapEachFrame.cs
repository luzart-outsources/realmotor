namespace Luzart
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    
    public class OverlapEachFrame : RaycastLayer
    {
        [SerializeField]
        protected BoxCollider boxCollider;
        [SerializeField]
        protected float valueX = 1;
        public override ResultRaycast GetResultRaycast()
        {
            var colliders = Physics.OverlapBox(transform.position + boxCollider.center, valueX* boxCollider.size, Quaternion.identity, layerMask);
            if (colliders!=null)
            {
                for (int i = 0;i < colliders.Length; i++)
                {
                    if(colliders[i] != null)
                    {
                        var baseMotorbike = colliders[i].gameObject.GetComponentInParent<BaseMotorbike>();
                        if(baseMotorbike != null && baseMotorbike.gameObject!= gameObject)
                        {
                            resultRaycast = new ResultRaycast();
                            resultRaycast.hit = hit;
                            resultRaycast.eLayer = eLayer;
                            resultRaycast.baseMotorbikes = baseMotorbike;
                            IsCheckEditor(true);
                            return resultRaycast;
                        }
                    }
                    IsCheckEditor(false);
                }
    
            }
            return null;
        }
    }
}
