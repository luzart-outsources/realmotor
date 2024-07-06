using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgrade : UIBase
{
    public Button btnBack;
    public Button btnUpgrade;
    public Button btnSettings;
    public TMP_Text txtValue;
    public ItemUpgradePopUp[] itemUpgradePopups;
    private ItemUpgradePopUp itemCache;
    private StatsMotorbike myStats;
    private DB_ResourcesBuy db_Resbuy;
    protected override void Setup()
    {
        base.Setup();
        GameUtil.ButtonOnClick(btnUpgrade, ClickUpgrade, true);
        GameUtil.ButtonOnClick(btnBack, ClickBack, true);
        GameUtil.ButtonOnClick(btnSettings, ClickSettings, true);
    }
    public override void Show(Action onHideDone)
    {
        base.Show(onHideDone);
        RefreshUI();
    }
    public override void RefreshUI()
    {
        base.RefreshUI();
        int idCurMotor = DataManager.Instance.GameData.idCurMotor;
        int[] levelUpgrade = ConfigStats.GetLevelsUpgrade(idCurMotor, DataManager.Instance.GameData.motorbikeDatas);
        InforMotorbike infor = ConfigStats.GetInforMotorbike(idCurMotor, levelUpgrade);
        itemUpgradePopups[0].Initialize(StatsMotorbike.MaxSpeed, infor.maxSpeed, levelUpgrade[0], ClickItemUpgradePopUp);
        itemUpgradePopups[1].Initialize(StatsMotorbike.Acceleration, infor.acceleration, levelUpgrade[1], ClickItemUpgradePopUp);
        itemUpgradePopups[2].Initialize(StatsMotorbike.Handling, infor.handling, levelUpgrade[2], ClickItemUpgradePopUp);
        itemUpgradePopups[3].Initialize(StatsMotorbike.Brake, infor.brake, levelUpgrade[3], ClickItemUpgradePopUp);
        if(itemCache != null)
        {
            ClickItemUpgradePopUp(itemCache);
        }
        else
        {
            ClickItemUpgradePopUp(itemUpgradePopups[0]);
        }

    }
    private void ClickItemUpgradePopUp(ItemUpgradePopUp item)
    {
        if(itemCache != null)
        {
            itemCache.SelectButton(false);
        }
        itemCache = item;
        item.SelectButton(true);
        myStats = item.stats;
        SwitchStats();
    }
    private void ClickBack()
    {
        UIManager.Instance.ShowGarage(UIName.Garage);
    }
    private void ClickUpgrade()
    {
        DataResource data = new DataResource();
        data.type = new DataTypeResource(RES_type.Gold);
        data.amount = db_Resbuy.valueBuy;
        DataManager.Instance.AddRes(data, UpgradeComplete);
    }
    private void ClickSettings()
    {
        UIManager.Instance.ShowUI(UIName.Settings);
    }
    private void UpgradeComplete()
    {
        int indexStats = GameUtil.GetIndexStats(myStats);
        int idCurMotor = DataManager.Instance.GameData.idCurMotor;
        DataManager.Instance.UpgradeMotor(idCurMotor, indexStats);
        RefreshUI();
    }
    private void SwitchStats()
    {
        int idCurMotor = DataManager.Instance.GameData.idCurMotor;
        int[] levelUpgrades = ConfigStats.GetLevelsUpgrade(idCurMotor, DataManager.Instance.GameData.motorbikeDatas);
        int indexStats = GameUtil.GetIndexStats(myStats);
        int levelStats = 0;
        levelStats = levelUpgrades[indexStats];
        db_Resbuy = DataManager.Instance.resourceBuySO.GetResourcesBuyUpgradeMotor(idCurMotor,indexStats,levelStats);
        txtValue.text = db_Resbuy.valueBuy.ToString();
        bool isStatus = (levelStats < 5);
        btnUpgrade.gameObject.SetActive(isStatus);
    }
}
