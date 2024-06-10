using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BikeController : MonoBehaviour
{
    private float moveInput, steelInput, rayLength,currentVelocityOffset;
    private Vector3 velocity;
    public float maxSpeed, acceleration, steelStrength;
    public float gravity;
    public float bikeXTiltIncrement = 0.09f, zTiltAngle = 45f;
    private RaycastHit hit;
    public LayerMask derivableSurface;
    [Range(1f, 10f)]
    public float brakingFactor;


    public Rigidbody sphereRB, bikeBody;
    // Start is called before the first frame update
    void Start()
    {
        moveInput = 0;
        sphereRB.transform.parent = null;
        bikeBody.transform.parent = null;
        rayLength = sphereRB.GetComponent<SphereCollider>().radius + 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        InputController();
        transform.position = sphereRB.transform.position;
        bikeBody.MoveRotation(transform.rotation);
        velocity = bikeBody.transform.InverseTransformDirection(bikeBody.velocity);
        currentVelocityOffset = velocity.z/maxSpeed;
    }
    private void InputController()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveInput = 1;
        }
         if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveInput = -1;
        }
         if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            moveInput = 0;
        }
         if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            moveInput = 0;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            steelInput = 1;
        }
         if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            steelInput = -1;
        }
         if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            steelInput = 0;
        }
         if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            steelInput = 0;
        }

    }
    private void FixedUpdate()
    {
        Movement();
    }
    private void Movement()
    {
        if (Grounded())
        {
            if (!Input.GetKey(KeyCode.Space))
            {
                Acceleration();
                Rotation();
            }
            Brake();
        }
        else
        {
            Gravity();
        }
        BikeTilt();
    }
    private void Acceleration()
    {
        sphereRB.velocity = Vector3.Lerp(sphereRB.velocity, moveInput * maxSpeed * transform.forward, Time.fixedDeltaTime * acceleration);
    }
    private void Rotation()
    {
        transform.Rotate(0, steelInput * moveInput * steelStrength * Time.fixedDeltaTime, 0, Space.World);
    }
    private void Brake()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            sphereRB.velocity *= brakingFactor / 10 ;
        }

    }
    private bool Grounded()
    {
        if(Physics.Raycast(sphereRB.position, Vector3.down, out hit, rayLength, derivableSurface))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void Gravity()
    {
        sphereRB.AddForce(gravity * Vector3.down, ForceMode.Acceleration);
    }
    private void BikeTilt()
    {
        float xRot = (Quaternion.FromToRotation(bikeBody.transform.up, hit.normal)*bikeBody.transform.rotation).eulerAngles.x;
        float zRot = 0;
        if(currentVelocityOffset > 0)
        {
            zRot = -zTiltAngle * steelInput * currentVelocityOffset;
        }

        Quaternion targetRot = Quaternion.Slerp(bikeBody.transform.rotation, Quaternion.Euler(xRot, transform.eulerAngles.y, zRot), bikeXTiltIncrement);
        Quaternion newRotation = Quaternion.Euler(targetRot.eulerAngles.x,transform.eulerAngles.y, targetRot.eulerAngles.z);
        bikeBody.MoveRotation(newRotation);
    }
}
