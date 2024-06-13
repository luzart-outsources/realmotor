using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCheckGround : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }
}
