using Coffee.UIExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgrade : UIBase
{
    public Button btnBack;
    public Button btnUpgrade;
    //public Button btnSettings;
    public TMP_Text txtValue;
    public ItemUpgradePopUp[] itemUpgradePopups;
    private ItemUpgradePopUp itemCache;
    private StatsMotorbike myStats;
    private DB_ResourcesBuy db_Resbuy;
    public Image imIcon;
    public Sprite[] allSprite;
    public TMP_Text txtCur, txtUpgrade;
    public UIShiny uiShiny;
    protected override void Setup()
    {
        base.Setup();
        GameUtil.ButtonOnClick(btnUpgrade, ClickUpgrade, true);
        GameUtil.ButtonOnClick(btnBack, ClickBack, true);
        //GameUtil.ButtonOnClick(btnSettings, ClickSettings, true);
    }
    public override void Show(Action onHideDone)
    {
        base.Show(onHideDone);
        RefreshUI();
    }
    private bool IsMaxLevel = false;
    public override void RefreshUI()
    {
        base.RefreshUI();
        int idCurMotor = DataManager.Instance.GameData.idCurMotor;
        int[] levelUpgradeCur =  ConfigStats.GetLevelsUpgrade(idCurMotor, DataManager.Instance.GameData.motorbikeDatas);
        int[] levelUpgrade = levelUpgradeCur.ToArray();

        for (int i = 0; i < levelUpgrade.Length; i++)
        {
            levelUpgrade[i]++;
        }
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
        data.amount = -db_Resbuy.valueBuy;
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
        uiShiny.Play(false);
        RefreshUI();
    }
    private void SwitchStats()
    {
        int idCurMotor = DataManager.Instance.GameData.idCurMotor;
        int[] levelUpgrades = ConfigStats.GetLevelsUpgrade(idCurMotor, DataManager.Instance.GameData.motorbikeDatas);
        int[] infor = ConfigStats.GetInforMotorbike(idCurMotor, levelUpgrades).ToArray();
        int indexStats = GameUtil.GetIndexStats(myStats);
        imIcon.sprite = allSprite[indexStats];
        int levelStats = 0;
        levelStats = levelUpgrades[indexStats];
        bool isStatus = (levelStats < 5);
        btnUpgrade.gameObject.SetActive(isStatus);
        int pr = 0;
        IsMaxLevel = true;
        for (int i = 0; i < levelUpgrades.Length; i++)
        {
            if (levelUpgrades[i] < 5)
            {
                IsMaxLevel = false;
            }
            pr += infor[i];
        }

        txtCur.text = pr.ToString();
        if (IsMaxLevel)
        {
            txtUpgrade.text = "MAX";
        }
        else
        {
            pr = pr - infor[indexStats] + (int)itemCache.valueStats;
            txtUpgrade.text = pr.ToString();
        }


        if (!isStatus)
        {
            return;
        }
        db_Resbuy = DataManager.Instance.resourceBuySO.GetResourcesBuyUpgradeMotor(idCurMotor,indexStats,levelStats);
        txtValue.text = db_Resbuy.valueBuy.ToString();
        
    }
}
