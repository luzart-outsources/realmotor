using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

public class BaseMotorbike : MonoBehaviour
{
    [SerializeField]
    private BaseMotor baseMotor;
    [SerializeField]
    private BaseCharacter baseCharacter;
    private BaseController baseController;
    public InforMotorbike inforMotorbike;



#if UNITY_EDITOR

    public InforMotorbike fakeData;
    private void Start()
    {
        var baseController =  this.gameObject.GetComponent<BaseController>();
        if (baseController == null)
        {
            baseController = this.gameObject.AddComponent<BaseController>();
        }
        baseController.Initialized(this);
        Initialize(fakeData, baseController);
        InitAction();
    }

#endif

    private void InitAction()
    {
        baseMotor.ActionCollisionWall = OnVisualCharacterCollisionWall;
    }

    public void Initialize(InforMotorbike inforMotorbike, BaseController baseController)
    {
        this.inforMotorbike = inforMotorbike;
        this.baseController = baseController;
        baseMotor.Initialize(this);
        baseCharacter.Initialize(this);

    }

    private void OnVisualCharacterCollisionWall (Vector3 velocity)
    {
        baseCharacter.OnCollisionWall(velocity);    
    }
    private void FixedUpdate()
    {
        FixedUpdateController();
    }
    private void Update()
    {
        UpdateController();
    }
    private void FixedUpdateController()
    {
        if (baseController != null)
        {
            baseController.FixedUpdateController();
        }

    }
    private void UpdateController()
    {
        if(baseController != null)
        {
            baseController.UpdateController();
        }
    }
    public void MoveLeft()
    {
        baseMotor.MoveLeft();
        baseCharacter.MoveLeft();
    }
    public void MoveRight()
    {
        baseMotor.MoveRight();
        baseCharacter.MoveRight();
    }
    public void MoveUp()
    {
        baseMotor.MoveUp();
        baseCharacter.MoveUp();
    }
    public void Brake()
    {
        baseMotor.Brake();
        baseCharacter.Brake();
    }
    public void UnHorizontal()
    {
        baseMotor.UnHorizontal();
        baseCharacter.UnHorizontal();
    }
    public void UnVerticle()
    {
        baseMotor.UnVerticle();
        baseCharacter.UnVerticle();
    }

    public void MoveVisual(Vector3 velocity)
    {
        baseMotor.OnVisualMove(velocity);
    }

    public void MoveSteerVisual(int steerInput, float currentSpeed)
    {
        baseMotor.OnVisualTilt(steerInput, currentSpeed);
    }

}
