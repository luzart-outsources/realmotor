using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMotor : MonoBehaviour
{
    public MotorVisual visualMotor;
    public MotorMovement movementMotor;

    [SerializeField]
    private Transform transformVisualMotor;
    public BaseMotorbike baseMotorbike {  get; private set; }
    public ELayerRaycastMotorbike ELayerCurrent;
    private bool IsBrake = false;
    public float Speed => movementMotor.currentSpeed;

    public Action<Vector3> ActionCollisionWall = null;

    public void InitSpawn()
    {
        visualMotor.motorVisual = transformVisualMotor;
    }
    public void Initialize(BaseMotorbike baseMotorbike)
    {
        this.baseMotorbike = baseMotorbike;
        movementMotor.Initialize(this);
        visualMotor.Initialize(this);

        InitAction();
    }
    private void InitAction()
    {
        movementMotor.ActionTiltRotate = OnVisualTilt;
        movementMotor.ActionOnMove = OnVisualMove;
        movementMotor.ActionOnBrake = OnBrake;
        movementMotor.ActionCollisionWall = OnCollisionWall;
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
        IsBrake = false;
    }
    public void Drift()
    {
        visualMotor.SkidMarks();
        baseMotorbike.soundMotorbike.SoundDrifEnable(true);
    }
    public void UnDrift()
    {
        visualMotor.BackSkidMarks();
        baseMotorbike.soundMotorbike.SoundDrifEnable(false);
    }
    public void RotationWheel(float velocity)
    {
        visualMotor.RotateWheel(velocity);
    }
    public void ControlFXDustDrift(float velocity)
    {
        visualMotor.ControlFXDustDrift(velocity);
    }
    public void UnEnableFXDustDrift()
    {
        visualMotor.UnEnableFXDustDrift();
    }
    public void OnVisualTilt(int steerInput, float currentSpeed)
    {
        visualMotor.OnVisualTilt(steerInput, currentSpeed);
    }
    public void OnVisualDust(float velocity)
    {
        visualMotor.OnShowDustGround(velocity);
    }
    public void OnUnableVisualDust()
    {
        visualMotor.UnEnableFXDustGround();
    }
    public void OnBrake(Vector3 velocity)
    {
        IsBrake = true;
    }
    public void OnVisualMove(Vector3 velocity)
    {
        RotationWheel(velocity.z);
        if ((IsBrake /*||Mathf.Abs(velocity.x) > 0.1*/) && ELayerCurrent == ELayerRaycastMotorbike.Road)
        {
            Drift();
            ControlFXDustDrift(velocity.z);
        }
        else
        {
            UnDrift();
            UnEnableFXDustDrift();
        }
        if (ELayerCurrent == ELayerRaycastMotorbike.Ground)
        {
            OnVisualDust(velocity.z);
        }
        else
        {
            OnUnableVisualDust();
        }
    }
    public void ForceStop()
    {
        movementMotor.currentSpeed = 0;
    }
    private void OnCollisionWall(Vector3 velocity)
    {
        ActionCollisionWall?.Invoke(velocity);
    }
}
