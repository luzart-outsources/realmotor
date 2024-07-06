using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    public Animator animator;
    public const string KEY_NGHIENGXE = "NghiengXe";
    private float valueRotate = 0f;
    [SerializeField]
    private float factor = 1f;

    private ETypeRotate typeRotate = ETypeRotate.Default;
    public void MoveLeft()
    {
        typeRotate = ETypeRotate.LEFT;
    }
    public void MoveRight()
    {
        typeRotate = ETypeRotate.RIGHT;
    }
    public void MoveUp()
    {

    }
    public void Brake()
    {
    }
    public void UnHorizontal()
    {
        typeRotate = ETypeRotate.Default;
    }
    public void UnVerticle()
    {
    }
    private void FixedUpdate()
    {
        float step = Time.fixedDeltaTime* factor;
        switch (typeRotate)
        {
            case ETypeRotate.Default:
                {
                    valueRotate = (valueRotate > 0) ? Math.Max(0, valueRotate - step) : Math.Min(0, valueRotate + step);
                    break;
                }
            case ETypeRotate.LEFT:
                {
                    valueRotate = Math.Clamp(valueRotate - step, -0.5f, 0.5f);
                    break;
                }
            case ETypeRotate.RIGHT:
                {
                    valueRotate = Math.Clamp(valueRotate + step, -0.5f, 0.5f);
                    break ;
                }
        }
        animator.SetFloat(KEY_NGHIENGXE, valueRotate);
    }

    private enum ETypeRotate
    {
        Default = 0,
        LEFT = 1,
        RIGHT = 2,
    }
}
