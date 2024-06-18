using UnityEngine;

public class OverlapBox : MonoBehaviour
{
    [SerializeField]
    private LayerMask layer;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("BikeBody"))
        {
            Debug.LogError("Call");
            //Vector3 pos = collision.contacts[0].point;
            //Vector3 dir = pos - transform.position;
            //var rb = collision.gameObject.GetComponent<Rigidbody>();
            //rb.AddForce(dir, ForceMode.Force);
        }
    }
}
