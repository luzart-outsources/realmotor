using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class UIHome : UIBase
{
    public GarageManager garageManager { get;set; }
    public Button btnRacing, btnGarage, btnUpgrade,btnShop, btnSetting, btnDailyReward;
    protected override void Setup()
    {
        base.Setup();
        GameUtil.ButtonOnClick(btnRacing, ClickRacing, true);
        GameUtil.ButtonOnClick(btnGarage, ClickGarage, true);
        GameUtil.ButtonOnClick(btnUpgrade, ClickUpgrade, true);
        GameUtil.ButtonOnClick(btnShop, ClickShop, true);
        GameUtil.ButtonOnClick(btnSetting, ClickSettings, true);
        GameUtil.ButtonOnClick(btnDailyReward, ClickDailyReward, true);
    }
    private void ClickRacing()
    {
        UIManager.Instance.ShowUI(UIName.SelectMode);
    }
    private void ClickGarage()
    {
        UIManager.Instance.ShowGarage(UIName.Garage);
    }
    private void ClickUpgrade()
    {
        UIManager.Instance.ShowGarage(UIName.Upgrade);
    }
    private void ClickShop()
    {

    }
    private void ClickSettings()
    {
        UIManager.Instance.ShowUI(UIName.Settings);
    }
    private void ClickDailyReward()
    {

    }
    public override void RefreshUI()
    {
        base.RefreshUI(); 
        garageManager.SpawnMotorVisual(DataManager.Instance.GameData.idCurMotor);
    }
}
