using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public static TestManager Instance;
    private void Awake()
    {
        Instance = this;
    }

#if UNITY_EDITOR

    [Sirenix.OdinInspector.Button]
    public void CrossProduct(Vector3 vt1, Vector3 vt2)
    {
        Debug.Log(Vector3.Cross(vt1, vt2)); 
    }




#endif
}
