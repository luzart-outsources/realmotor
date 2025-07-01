namespace Luzart
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    
    public class RaycastFromHeadBike : RaycastLayer
    {
        public BoxCollider boxCollider;
        public override bool IsRaycast()
        {
            Vector3 originPos;
            originPos = transform.position + boxCollider.center - boxCollider.size/2;
            bool isRayHit = Physics.Raycast(originPos, Vector3.down, out hit, Mathf.Infinity, layerMask);
            return isRayHit;
        }
    }
}
