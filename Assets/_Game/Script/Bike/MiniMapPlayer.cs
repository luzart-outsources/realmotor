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
