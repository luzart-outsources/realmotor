using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static AirFishLab.ScrollingList.ListBank;

public class LeaderBoardUIInGame : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Transform parentSpawn;
    public ItemLeaderBoardUI itemLeaderBoardUIPf;
    public List<ItemLeaderBoardUI> listItemLeaderBoardUI = new List<ItemLeaderBoardUI>();
    private List<DB_LeaderBoardInGame> _listDBInGame = new List<DB_LeaderBoardInGame>();
    private ItemLeaderBoardUI itemMe;
    public void InitList(List<DB_LeaderBoardInGame> list)
    {
        this._listDBInGame = list;
        int length = list.Count;
        MasterHelper.InitListObj<ItemLeaderBoardUI>(length, itemLeaderBoardUIPf, listItemLeaderBoardUI, parentSpawn, (item, index) =>
        {
            item.gameObject.SetActive(true);
            var db = _listDBInGame[index];
            item.InitItem(db);
            if(db.eTeam == ETeam.Player)
            {
                itemMe = item;
            }
        });

    }
    private int preIndex = -1;
    public void UpdateList(List<DB_LeaderBoardInGame> list)
    {
        int indexMe = -1;
        _listDBInGame = list;
        int length = _listDBInGame.Count;
        for (int i = 0; i < length; i++)
        {
            var item = listItemLeaderBoardUI[i];
            var db = _listDBInGame[i];
            item.InitItem(db);
            if(db.eTeam == ETeam.Player)
            {
                itemMe = item;
                indexMe = i;
            }
        }
        if (itemMe != null)
        {
            scrollRect.FocusOnRectTransform(itemMe.rectTransform,0f);
            preIndex = indexMe;
        }

    }
    public void UpdateDistance(List<DB_LeaderBoardInGame> list)
    {
        int length = list.Count;   
        for (int i = 0; i < length; i++)
        {
            var item = listItemLeaderBoardUI[i];
            var db = _listDBInGame[i];
            item.UpdateDistance(db.distance);
        }
    }

}
