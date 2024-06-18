using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavingPointGizmos : MonoBehaviour
{
    public Transform[] allWavePoint;
    private void OnDrawGizmos()
    {
        //allWavePoint = GetComponentsInChildren<Transform>();
        for (int i = 0; i < allWavePoint.Length; i++)
        {
            Gizmos.color = Color.yellow;
            if(i >= allWavePoint.Length - 1)
            {
                Gizmos.DrawLine(allWavePoint[i].position, allWavePoint[0].position);
            }
            else
            {
                Gizmos.DrawLine(allWavePoint[i].position, allWavePoint[i + 1].position);
            }
            
        }
    }
}
