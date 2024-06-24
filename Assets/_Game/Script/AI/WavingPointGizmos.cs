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
    private void GetAllWavePointEditor()
    {
        allWavePoint = transform.GetComponentsInChildren<WavingPoint>();
    }
}
