using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ClassicMode : BaseMode
{
    private UIGameplay uIGameplay;

    private bool isInit { get; set; } = false;

    public override void StartLevel(int level)
    {
        base.StartLevel(level);
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