using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ClassicMode : BaseMode
{
    private UIGameplay uIGameplay;
    private DB_Level db_Level;
    private int currentLevel;
    private bool isInit { get; set; } = false;

    public override void StartLevel(int level)
    {
        base.StartLevel(level);
        InitData(level);
        UIManager.Instance.LoadScene(() =>
        {
            gameCoordinator.StartGame(db_Level);
        }, () =>
        {
            gameCoordinator.StartInGame();
        });

        gameCoordinator.ActionOnEndGame = OnEndGame;
    }
    private void InitData(int level)
    {
        currentLevel = level;
        db_Level = DataManager.Instance.levelSO.GetDB_Level(currentLevel);
    }
    public override void OnEndGame(bool isWin)
    {
        GameManager.Instance.gameCoordinator.EndGame(isWin);
        OnDataWin();
        OnReceiveRes();
        ShowPopUp(isWin);
        base.OnEndGame(isWin);
    }
    private void OnDataWin()
    {
        int count = GameManager.Instance.gameCoordinator.countLeaderBoard;
        int length = GameManager.Instance.gameCoordinator.listLeaderBoard.Count;
        int valueCountLeaderBoard = length - count;
        int level = db_Level.level;
        dataValueWin = new DataValueWin();
        dataValueWin.valuePos = DataManager.Instance.dB_ResourceSO.GetDataResourcePosition(valueCountLeaderBoard, level).amount;
        dataValueWin.valueLevel = DataManager.Instance.dB_ResourceSO.GetDataResourceBaseLevel(level).amount;
        dataValueWin.valueJoin = DataManager.Instance.dB_ResourceSO.dataResJoin.amount;
        dataValueWin.valueResult = DataManager.Instance.dB_ResourceSO.GetDataResResult().amount;
    }
    private void OnReceiveRes()
    {
        DataResource dataResource = new DataResource();
        dataResource.type = new DataTypeResource(RES_type.Gold);
        dataResource.amount = dataValueWin.valueLevel + dataValueWin.valuePos;
        DataManager.Instance.ReceiveRes(dataResource);
    }
    protected override void OnWinGame()
    {
        base.OnWinGame();
        if(DataManager.Instance.CurrentLevel <= db_Level.level)
        {
            DataManager.Instance.GameData.level++;
            DataManager.Instance.SaveGameData();
        }

    }
    private DataValueWin dataValueWin;
    private void ShowPopUp(bool isWin)
    {
        var ui = UIManager.Instance.ShowUI<UIWinClassic>(UIName.WinClassic);
        if(ui != null)
        {
            ui.InitDataRes(isWin, dataValueWin);
            ui.InitDataDashboard(GameManager.Instance.gameCoordinator.listDataItemWinLeaderBoard);
            ui.OnShowPopUp();
        }
    }
}