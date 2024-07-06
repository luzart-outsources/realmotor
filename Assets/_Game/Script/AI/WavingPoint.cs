using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavingPoint : MonoBehaviour
{
    private int _indexPoint = -1;
    public int indexPoint
    {
        get
        {
            if(_indexPoint == -1)
            {
                _indexPoint = transform.GetSiblingIndex();
            }
            return _indexPoint;
        }
    }
    private void Awake()
    {
        _indexPoint = transform.GetSiblingIndex();
    }
}
