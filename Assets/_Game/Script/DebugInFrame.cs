namespace Luzart
{
    #if UNITY_EDITOR
    using Sirenix.OdinInspector;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    
    public class DebugInFrame : MonoBehaviour
    {
        public LayerMask layerRoad;
        //[Button]
        //public void SetLayerMask()
        //{
        //    layerRoad = LayerMask.Lay("Road");
        //}
        [Button]
        public void Raycast()
        {
            int layer = LayerMask.NameToLayer("Road");
            LayerMask layerRoad = 1 << layer;
            RaycastHit rayUp, rayDown;
            bool isRayUp = Physics.Raycast(transform.position, Vector3.up, out rayUp,1000, layerRoad);
            bool isRayDown = Physics.Raycast(transform.position + Vector3.up*1000, Vector3.down, out rayDown, Mathf.Infinity, layerRoad);
            Debug.Log($"RayUp {isRayUp}");
            Debug.Log($"RauDown {isRayDown}");
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, Vector3.up*1000);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Vector3.down);
        }
    }
    #endif
}
