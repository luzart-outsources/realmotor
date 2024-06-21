using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavingPoint : MonoBehaviour
{
    public int indexPoint;
    private void OnDrawGizmos()
    {
        indexPoint = transform.GetSiblingIndex();
    }
}
