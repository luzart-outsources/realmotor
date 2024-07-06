using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVisual : MonoBehaviour
{
    [SerializeField]
    private SkinnedMeshRenderer skinMeshAnim;
    [SerializeField]
    private SkinnedMeshRenderer skinMeshRagdoll;
    private DB_Character character;
    public void InitDBCharacter(DB_Character character)
    {
        this.character = character;
        ChangeHeader();
        ChangeBody();
    }
    public void ChangeHeader()
    {
        ChangeMainTexture(skinMeshAnim, 1, ResourcesManager.Instance.LoadHelmet(character.idHelmet));
        ChangeMainTexture(skinMeshRagdoll, 1, ResourcesManager.Instance.LoadHelmet(character.idHelmet));
    }
    public void ChangeBody()
    {
        ChangeMainTexture(skinMeshAnim, 0, ResourcesManager.Instance.LoadBody(character.idClothes));
        ChangeMainTexture(skinMeshRagdoll, 0, ResourcesManager.Instance.LoadBody(character.idClothes));
    }
    private void ChangeMainTexture(SkinnedMeshRenderer skinMeshAnim,int index, Texture2D tx2D)
    {
        if(skinMeshAnim != null)
        {
            skinMeshAnim.materials[index].mainTexture = tx2D;
        }
    }
}
