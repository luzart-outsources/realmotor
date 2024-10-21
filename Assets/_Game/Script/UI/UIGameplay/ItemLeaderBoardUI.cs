using BG_Library.Common;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemLeaderBoardUI : MonoBehaviour
{
    private RectTransform _rectTransform = null;
    public RectTransform rectTransform
    {
        get
        {
            if(_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }
            return _rectTransform;
        }
    }
    public TMP_Text txtIndex;
    public TMP_Text txtName;
    public GameObject obPlayer;
    public GameObject obBot;
    public Image imSpace;
    public TMP_Text txtDistance;
    public TMP_Text txtRound,txtPoint,txtDisIndex;
    public List<Color> listColor;
    public DB_LeaderBoardInGame data {  get; private set; }
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
        var random = new RandomNoRepeat<Color>(listColor);
        imSpace.color = random.Random();
        txtIndex.text = $"<color={color}>{db.index+1}</color>";
        txtName.text = $"<color={color}>{db.name}</color>";
        if(db.distance == 0)
        {
            txtDistance.text = "";
        }
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
    public void UpdateDistance(float distance)
    {
        if (distance == 0)
        {
            txtDistance.text = "";
        }
        txtDistance.text = $"{System.Math.Round(distance, 2)}";
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
