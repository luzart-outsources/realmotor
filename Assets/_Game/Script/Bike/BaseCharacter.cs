using System;
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
    private Rigidbody _rbRagdoll;
    [SerializeField]
    private float factorRagdoll = 1f;

    public BaseMotorbike baseMotorbike;
    public void InitSpawn(DB_Character dbCharacter)
    {
        characterVisual.InitDBCharacter(dbCharacter);
    }
    public void Initialize(BaseMotorbike baseMotorbike)
    {
        this.baseMotorbike = baseMotorbike;
        if(_rbRagdoll != null)
        {
            Destroy(_rbRagdoll.gameObject);
        }
        characterAnimation.InitCharacter(baseMotorbike);
        characterAnimation.gameObject.SetActive(true);
        IsCollisionWall = false;
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
    public void OnVisualTilt(int teer)
    {
        if(teer > 0)
        {
            MoveRight();
        }
        else if(teer < 0)
        {
            MoveLeft();
        }
        else
        {
            UnHorizontal();
        }
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
        _rbRagdoll = Instantiate(this.rbRagdoll, rbRagdoll.transform.parent);
        _rbRagdoll.transform.position = rbRagdoll.transform.position;
        _rbRagdoll.transform.SetParent(null);
        if(baseMotorbike.eTeam == ETeam.Player)
        {
            CameraManager.Instance.SetFollowCamera(_rbRagdoll.transform.GetChild(0).GetChild(0));
        }

        _rbRagdoll.gameObject.SetActive(true);
        Vector3 velocityUp = -baseMotorbike.transform.forward * velocity.magnitude* factorRagdoll;
        _rbRagdoll.AddForce(velocityUp,ForceMode.Force);
    }
}
