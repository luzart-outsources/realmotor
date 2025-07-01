namespace Luzart
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    
    public class MiniMapPlayer : MonoBehaviour
    {
        [SerializeField]
        private GameObject obEnemy;
        [SerializeField]
        private GameObject obPlayer;
        private BaseMotorbike baseMotorbike;
        public void Initialize(BaseMotorbike baseMotorbike)
        {
            this.baseMotorbike = baseMotorbike;
            float scale = GameManager.Instance.gameCoordinator.miniMapEnvironment.cameraMiniMap.orthographicSize;
            float scaleEnemy = scale / obEnemy.transform.localScale.x * 1/6 ;
            float scalePlayer = scale / obPlayer.transform.localScale.x * 5/6;
            obEnemy.transform.localScale = obEnemy.transform.localScale * scaleEnemy;
            obPlayer.transform.localScale = obPlayer.transform.localScale * scalePlayer;
            obEnemy.SetActive(false);
            obPlayer.SetActive(false);
            if (baseMotorbike.eTeam == ETeam.AI)
            {
                obEnemy.SetActive(true);
            }
            else
            {
                obPlayer.SetActive(true);
            }
        }
    }
}
