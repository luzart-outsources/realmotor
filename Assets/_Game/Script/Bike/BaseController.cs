using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    protected BaseMotorbike baseMotorBike;
    protected bool IsInit = false;
    public virtual void Initialized(BaseMotorbike baseMotorbike)
    {
        this.baseMotorBike = baseMotorbike;
        IsInit = true;
    }
    public virtual void FixedUpdateController()
    {
        
    }
    public virtual void UpdateController()
    {
        if (!IsInit)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveUp();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Brake();
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            UnVerticle();
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            UnVerticle();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            UnHorizontal();
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            UnHorizontal();
        }
    }
    public virtual void MoveLeft()
    {
        baseMotorBike.MoveLeft();
    }
    public virtual void MoveRight()
    {
        baseMotorBike.MoveRight();
    }
    public virtual void MoveUp()
    {
        baseMotorBike.MoveUp();
    }
    public virtual void Brake()
    {
        baseMotorBike.Brake();
    }
    public virtual void UnHorizontal()
    {
        baseMotorBike.UnHorizontal();
    }
    public virtual void UnVerticle()
    {
        baseMotorBike.UnVerticle();
    }
    public void MoveVisual(Vector3 velocity)
    {
        baseMotorBike.MoveVisual(velocity);
    }

    public void MoveSteerVisual(int steerInput, float currentSpeed)
    {
        baseMotorBike.MoveSteerVisual(steerInput, currentSpeed);
    }
}
