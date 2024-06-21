using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentMap : MonoBehaviour
{
    public WavingPointGizmos wavingPointGizmos;
    public Transform[] startPoint;

    public Transform GetStartPoint(int index)
    {
        return startPoint[index];
    }
}
