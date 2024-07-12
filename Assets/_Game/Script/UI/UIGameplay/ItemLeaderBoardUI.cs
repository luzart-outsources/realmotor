using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemLeaderBoardUI : MonoBehaviour
{
    public TMP_Text txtIndex;
    public TMP_Text txtName;
    public GameObject obPlayer;
    public GameObject obBot;
    public TMP_Text txtDistance;
    public TMP_Text txtRound,txtPoint,txtDisIndex;

    private DB_LeaderBoardInGame data;
    private const string WhiteColor = "#FFFFFF";
    private const string BlackColor = "#000000";

    public void InitItem(DB_LeaderBoardInGame db)
    {
        this.data = db;
        bool isPlayer = db.eTeam == ETeam.Player;
        obBot.SetActive(!isPlayer);
        obPlayer.SetActive(isPlayer);
        string color = isPlayer ? BlackColor : WhiteColor;
        txtDistance.gameObject.SetActive(isPlayer);
        txtIndex.text = $"<color={color}>{db.index+1}</color>";
        txtName.text = $"<color={color}>{db.name}</color>";
        txtDistance.text = $"{System.Math.Round(db.distance, 2)}";
#if ENABLE_TEST_LEADERBOARD
        txtRound.gameObject.SetActive(true); 
        txtPoint.gameObject.SetActive(true); 
        txtDisIndex.gameObject.SetActive(true); 
        txtRound.text = db.round.ToString();
        txtPoint.text = db.curIndex.ToString();
        txtDisIndex.text = $"{System.Math.Round(db.disFromIndex, 2)}";
#endif
    }
}
[System.Serializable]
public class DB_LeaderBoardInGame
{
    public int index;
    public string name;
    public float distance;
    public int curIndex;
    public int round;
    public float disFromIndex;
    public ETeam eTeam;   
}
