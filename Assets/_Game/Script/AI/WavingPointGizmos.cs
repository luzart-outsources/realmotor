namespace Luzart
{
    using Sirenix.OdinInspector;
    using System.Collections.Generic;
    using System.Linq;
    using Unity.VisualScripting;
    using UnityEngine;
    
    public class WavingPointGizmos : MonoBehaviour
    {
        public WavingPoint[] allWavePoint;
        public List<Vector3> GetAllWavePoint()
        {
            return allWavePoint.Select(wave => wave.transform.position).ToList();
        }
        //public List<Transform> GetAllTransPoint()
        //{
        //    return allWavePoint.ToList();
        //}
        public Transform GetTransformIndex(int index)
        {
            index = index % allWavePoint.Length;
            return allWavePoint[index].transform;
        }
        public int GetLastIndex()
        {
            return allWavePoint.Length - 1;
        }
        public Transform GetLastTransform()
        {
            return allWavePoint[GetLastIndex()].transform;
        }
        [Button]
        public void AddWavingPointChildren()
        {
            int layer = LayerMask.NameToLayer("Road");
            LayerMask layerRoad = 1 << layer;
            int length = transform.childCount;
            for (int i = 0; i < length; i++)
            {
                var item = transform.GetChild(i);
                var component = item.GetComponent<WavingPoint>();
                if (component == null)
                {
                    component = item.gameObject.AddComponent<WavingPoint>();
                }
                var col = item.GetComponent<Collider>();
                if (col == null)
                {
                    col = item.gameObject.AddComponent<SphereCollider>();
                }
                col.isTrigger = true;
                item.gameObject.layer = LayerMask.NameToLayer("WavingPoint");
                RaycastHit rayUp, rayDown;
                bool isRayUp = Physics.Raycast(item.position, Vector3.up, out rayUp, Mathf.Infinity, layerRoad);
                bool isRayDown = Physics.Raycast(item.position + 100 * Vector3.up, Vector3.down, out rayDown, Mathf.Infinity, layerRoad);
                if (isRayUp)
                {
                    item.transform.position = rayUp.point;
                }
                else if (isRayDown)
                {
                    item.transform.position = rayDown.point;
                }
            }
        }
        [Button]
        public void GetAllWavePointEditor()
        {
            allWavePoint = transform.GetComponentsInChildren<WavingPoint>();
            //for (int i = 0; i < allWavePoint.Length; i++)
            //{
            //    allWavePoint[i].indexPoint = allWavePoint[i].transform.GetSiblingIndex();
            //}
        }
        private void OnDrawGizmos()
        {
            allWavePoint = transform.GetComponentsInChildren<WavingPoint>();
            List<Transform> listWave = allWavePoint.Select(item => item.transform).ToList();
            for (int i = 0; i < listWave.Count; i++)
            {
    
                int nextIndex = i + 1;
                if (nextIndex >= listWave.Count)
                {
                    nextIndex = 0;
                }
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(listWave[i].position, listWave[nextIndex].position);
    
                listWave[i].LookAt(listWave[nextIndex].position);
            }
            Gizmos.color = Color.yellow;
    
        }
    }
}
