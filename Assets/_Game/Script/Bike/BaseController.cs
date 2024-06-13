using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    private BaseMotorbike baseMotorBike;
    private bool IsInit = false;
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
    public void MoveLeft()
    {
        baseMotorBike.MoveLeft();
    }
    public void MoveRight()
    {
        baseMotorBike.MoveRight();
    }
    public void MoveUp()
    {
        baseMotorBike.MoveUp();
    }
    public void Brake()
    {
        baseMotorBike.Brake();
    }
    public void UnHorizontal()
    {
        baseMotorBike.UnHorizontal();
    }
    public void UnVerticle()
    {
        baseMotorBike.UnVerticle();
    }
}
