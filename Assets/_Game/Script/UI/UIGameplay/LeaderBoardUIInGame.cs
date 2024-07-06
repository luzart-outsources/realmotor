using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardUIInGame : MonoBehaviour
{
    public Transform parentSpawn;
    public ItemLeaderBoardUI itemLeaderBoardUIPf;
    public List<ItemLeaderBoardUI> listItemLeaderBoardUI = new List<ItemLeaderBoardUI>();
    private List<DB_LeaderBoardInGame> _listDBInGame = new List<DB_LeaderBoardInGame>();

    public void InitList(List<DB_LeaderBoardInGame> list)
    {
        this._listDBInGame = list;
        int length = list.Count;
        MasterHelper.InitListObj<ItemLeaderBoardUI>(length, itemLeaderBoardUIPf, listItemLeaderBoardUI, parentSpawn, (item, index) =>
        {
            item.gameObject.SetActive(true);
            var db = _listDBInGame[index];
            item.InitItem(db);
        });
    }

    public void UpdateList(List<DB_LeaderBoardInGame> list)
    {
        _listDBInGame = list;
        int length = _listDBInGame.Count;
        for (int i = 0; i < length; i++)
        {
            var item = listItemLeaderBoardUI[i];
            var db = _listDBInGame[i];
            item.InitItem(db);
        }
    }
    
}
