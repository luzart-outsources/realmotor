using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMotor : MonoBehaviour
{
    [SerializeField]
    private MotorVisual visualMotor;
    [SerializeField]
    private MotorMovement movementMotor;
    public BaseMotorbike baseMotorbike;
    public void Initialize(BaseMotorbike baseMotorbike)
    {
        this.baseMotorbike = baseMotorbike;
        movementMotor.Initialize(this);
    }
    public void MoveLeft()
    {
        movementMotor.MoveLeft();
    }
    public void MoveRight()
    {
        movementMotor.MoveRight();
    }
    public void MoveUp()
    {
        movementMotor.MoveUp();
    }
    public void Brake()
    {
        movementMotor.Brake();
    }
    public void UnHorizontal()
    {
        movementMotor.UnHorizontal();
    }
    public void UnVerticle()
    {
        movementMotor.UnVerticle();
    }
    public void Drift()
    {
        visualMotor.SkidMarks();
    }
    public void UnDrift()
    {
        visualMotor.BackSkidMarks();
    }
    public void RotationWheel(float velocity)
    {
        visualMotor.RotateWheel(velocity);
    }
}
