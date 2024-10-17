using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHome : UIBase
{
    public GarageManager garageManager { get;set; }
    public Button btnRacing, btnGarage, btnUpgrade,btnShop, btnSetting, btnDailyReward;
    public TMP_Text txtPR, txtName, txtRank;
    protected override void Setup()
    {
        base.Setup();
        GameUtil.ButtonOnClick(btnRacing, ClickRacing, true, KeyAds.BtnHomeRacing);
        GameUtil.ButtonOnClick(btnGarage, ClickGarage, true);
        GameUtil.ButtonOnClick(btnUpgrade, ClickUpgrade, true);
        GameUtil.ButtonOnClick(btnShop, ClickShop, true);
        GameUtil.ButtonOnClick(btnSetting, ClickSettings, true);
        GameUtil.ButtonOnClick(btnDailyReward, ClickDailyReward, true);
    }
    private void ClickRacing()
    {
        UIManager.Instance.ShowUI(UIName.SelectLevel);
    }
    private void ClickGarage()
    {
        UIManager.Instance.ShowGarage(UIName.Garage);
    }
    private void ClickUpgrade()
    {
        UIManager.Instance.ShowGarage(UIName.Upgrade);
    }
    public void ClickShop()
    {
        UIManager.Instance.ShowUI(UIName.Shop);
    }
    private void ClickSettings()
    {
        UIManager.Instance.ShowUI(UIName.Settings);
    }
    private void ClickDailyReward()
    {
        UIManager.Instance.ShowUI(UIName.DailyReward);
    }
    public override void RefreshUI()
    {
        base.RefreshUI();
        int idMotor = DataManager.Instance.GameData.idCurMotor;
        garageManager.SpawnMotorVisual(idMotor);
        DB_Motor db_Motor = DataManager.Instance.motorSO.GetDBMotor(idMotor);
        int[] levelsUpgrade = ConfigStats.GetLevelsUpgrade(idMotor);
        InforMotorbike infor = ConfigStats.GetInforMotorbike(idMotor, levelsUpgrade);
        txtName.text = db_Motor.nameModelMotor;
        txtRank.text = $"Rank {db_Motor.rank}";
        txtPR.text = $"{infor.PR}";
    }
}
