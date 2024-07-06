using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
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
    [Button]
    public void AddWavingPointChildren()
    {
        int length = transform.childCount;
        for (int i = 0; i < length; i++)
        {
            var item = transform.GetChild(i);
            var component = item.GetComponent<WavingPoint>();
            if(component == null)
            {
                component = item.gameObject.AddComponent<WavingPoint>();
            }
            var col = item.GetComponent<Collider>();
            if(col == null)
            {
                col = item.gameObject.AddComponent<SphereCollider>();
            }
            col.isTrigger = true;
            item.gameObject.layer = LayerMask.NameToLayer("WavingPoint");
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
}
