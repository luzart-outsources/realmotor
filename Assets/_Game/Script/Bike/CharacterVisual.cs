using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVisual : MonoBehaviour
{
    [SerializeField]
    private SkinnedMeshRenderer skinMeshAnim;
    [SerializeField]
    private SkinnedMeshRenderer skinMeshRagdoll;

    public void InitDBCharacter(DB_Character character)
    {
        ChangeHeader();
        ChangeBody();
    }
    public void ChangeHeader()
    {

    }
    public void ChangeBody()
    {

    }
}
