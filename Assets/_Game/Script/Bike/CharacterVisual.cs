namespace Luzart
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    
    public class CharacterVisual : MonoBehaviour
    {
        [Space, Header("Skin Mesh Animation")]
        [SerializeField]
        private SkinnedMeshRenderer[] skinMeshHeader;
        [SerializeField]
        private SkinnedMeshRenderer[] skinMeshBody;
        [SerializeField]
        private SkinnedMeshRenderer[] skinMeshBody1;
        [Space, Header("Skin Mesh Ragdoll")]
        [SerializeField]
        private SkinnedMeshRenderer[] skinMeshHeaderRd;
        [SerializeField]
        private SkinnedMeshRenderer[] skinMeshBodyRd;
        [SerializeField]
        private SkinnedMeshRenderer[] skinMeshBody1Rd;
        private DB_Character character;
        public void InitDBCharacter(DB_Character character)
        {
            this.character = character;
            ChangeHeader();
            ChangeBody();
        }
        public void ChangeHeader()
        {
            var tx2D = ResourcesManager.Instance.LoadHelmet(character.idHelmet);
            ChangeMainTexture(skinMeshHeader, 0, tx2D);
            ChangeMainTexture(skinMeshHeaderRd, 0, tx2D);
        }
        public void ChangeBody()
        {
            var array = ResourcesManager.Instance.LoadBody(character.idClothes);
            ChangeMainTexture(skinMeshBody, 0, array[0]);
            ChangeMainTexture(skinMeshBodyRd, 0, array[0]);
            ChangeMainTexture(skinMeshBody1, 0, array[1]);
            ChangeMainTexture(skinMeshBody1Rd, 0, array[1]);
        }
        private void ChangeMainTexture(SkinnedMeshRenderer[] skinMeshAnim,int index, Texture2D tx2D)
        {
            if (skinMeshAnim != null && skinMeshAnim.Length > 0 )
            {
                for (int i = 0; i < skinMeshAnim.Length; i++)
                {
                    int idx = i;
                    skinMeshAnim[idx].materials[index].mainTexture = tx2D;
                }
            }
        }
    }
}
