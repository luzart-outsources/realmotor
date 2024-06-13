using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    [SerializeField]
    private CharacterVisual characterVisual;
    [SerializeField]
    private CharacterAnimation characterAnimation;

    public BaseMotorbike baseMotorbike;
    public void Initialize(BaseMotorbike baseMotorbike)
    {
        this.baseMotorbike = baseMotorbike;
    }

    public void MoveLeft()
    {
        characterAnimation.MoveLeft();
    }
    public void MoveRight()
    {
        characterAnimation.MoveRight();
    }
    public void MoveUp()
    {
        characterAnimation.MoveUp();
    }
    public void Brake()
    {
        characterAnimation.Brake();
    }
    public void UnHorizontal()
    {
        characterAnimation.UnHorizontal();
    }
    public void UnVerticle()
    {
        characterAnimation.UnVerticle();
    }
}
