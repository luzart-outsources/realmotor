using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavingPointGizmos : MonoBehaviour
{
    public Transform[] allWavePoint;
    public List<Vector3> GetAllWavePoint()
    {
        List<Vector3> result = new List<Vector3>();
        for (int i = 0; i < allWavePoint.Length; i++)
        {
            if(i == 0)
            {
                continue;
            }
            result.Add(allWavePoint[i].position);
        }
        return result;
    }
    public List<Transform> GetAllTransPoint()
    {
        List<Transform> result = new List<Transform>();
        for (int i = 0; i < allWavePoint.Length; i++)
        {
            if (i == 0)
            {
                continue;
            }
            result.Add(allWavePoint[i]);
        }
        return result;
    }
    private void OnDrawGizmos()
    {
        allWavePoint = GetComponentsInChildren<Transform>();
        List<Transform> listWave = new List<Transform>();
        for (int i = 0; i < allWavePoint.Length; i++)
        {
            if(i == 0)
            {
                continue;
            }
            listWave.Add(allWavePoint[i]);
        }
        for (int i = 0; i < listWave.Count; i++)
        {
            Gizmos.color = Color.yellow;
            int nextIndex = i + 1;
            if (i >= listWave.Count - 1)
            {
                nextIndex = 0;
            }
            Gizmos.DrawLine(listWave[i].position, listWave[nextIndex].position);
            listWave[i].LookAt(listWave[nextIndex].position);
        }
    }
}
