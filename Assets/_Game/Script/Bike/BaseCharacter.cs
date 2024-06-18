using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    [SerializeField]
    private CharacterVisual characterVisual;
    [SerializeField]
    private CharacterAnimation characterAnimation;
    [SerializeField]
    private Rigidbody rbRagdoll;

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
    private bool IsCollisionWall = false;
    public void OnCollisionWall(Vector3 velocity)
    {
        if (IsCollisionWall)
        {
            return;
        }
        IsCollisionWall= true;
        characterAnimation.gameObject.SetActive(false);
        rbRagdoll.transform.parent = null;
        rbRagdoll.gameObject.SetActive(true);
        Vector3 velocityUp = -transform.forward * velocity.magnitude;
        rbRagdoll.AddForce(velocityUp,ForceMode.Force);
    }
}
