using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCubeInGameObject : MonoBehaviour
{
    [SerializeField]
    private Vector3 size = Vector3.one;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position, size);
    }
}
