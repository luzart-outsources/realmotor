using UnityEngine;

public class OverlapBox : OverlapMotor
{
    public override ResultRaycast GetResultRaycast()
    {
        var colliders = Physics.OverlapBox(transform.position + boxCollider.center, boxCollider.size, Quaternion.identity, layerMask);
        if (colliders != null)
        {
            bool isCollider = false;
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != null)
                {
                    isCollider = true;
                    break;
                }

            }
            if (isCollider)
            {
                resultRaycast = new ResultRaycast();
                resultRaycast.hit = hit;
                resultRaycast.eLayer = eLayer;
                return resultRaycast;
            }

        }
        return null;
    }
}
