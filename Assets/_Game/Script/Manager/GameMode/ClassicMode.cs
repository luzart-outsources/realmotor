using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;
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
        gameCoordinator.StartGame(db_Level);
    }
    private void InitData(int level)
    {
        currentLevel = level;
        db_Level = DataManager.Instance.levelSO.GetDB_Level(currentLevel);
    }
    protected override void OnWinGame()
    {
        base.OnWinGame();
        DataManager.Instance.GameData.level++;
        DataManager.Instance.SaveGameData();
    }

    protected override void OnLoseGame()
    {
        base.OnLoseGame();
    }
}