using AirFishLab.ScrollingList;
using DG.Tweening;
using Eco.TweenAnimation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGarage : UIBase
{
    public Button btnBack;

    public Button btnSettings;
    public Button btnUpgrade;
    public Button btnRacer;
    public PopUpUpgradeGarage popupUpgrade;
    public GarageManager garageManager;
    public ScrollRect scrollView;
    public ItemSelectMotorbikeUI itemSelectMotorbikeUIPf;
    public List<ItemSelectMotorbikeUI> listItemSelect = new List<ItemSelectMotorbikeUI>();

    [Space, Header("Buy")]
    public Button btnRacing;
    public Button btnBuy;
    public GameObject obBuyGold;
    public GameObject obBuyAds;
    public GameObject obBuyIAP;
    public GameObject obBuyOther;
    public TMP_Text txtValueGold;
    public TMP_Text txtValueAds;
    public TMP_Text txtValueIAP;
    public TMP_Text txtValueOther;

    private ItemSelectMotorbikeUI itemCache;
    private DB_Motor[] allDBMotor
    {
        get
        {
            return DataManager.Instance.motorSO.dB_Motors;
        }
    }
    private int currentItemClick;

    private DB_ResourcesBuy resourcesBuy = null;
    private DataTypeResource dataTypeResourceCurrent;
    private int GetIdMotorInAllMotor(int id)
    {
        int length = listItemSelect.Count;
        for (int i = 0; i < length; i++)
        {
            if (id == listItemSelect[i].db_Motorbike.idMotor)
            {
                return listItemSelect[i].currentIndex;
            }
        }
        return 0;
    }
    protected override void Setup()
    {
        base.Setup();
        GameUtil.ButtonOnClick(btnSettings, ClickSettings, true, KeyAds.BtnGarageSettings);
        GameUtil.ButtonOnClick(btnRacing, ClickRacing, true, KeyAds.BtnGarageRacing);
        GameUtil.ButtonOnClick(btnUpgrade, ClickUpgrade, true, KeyAds.BtnGarageUpgarde);
        GameUtil.ButtonOnClick(btnRacer, ClickRacer, true, KeyAds.BtnGarageRacer);
        GameUtil.ButtonOnClick(btnBuy, ClickBuy, true);
        GameUtil.ButtonOnClick(btnBuy, ClickBuy, true);
        GameUtil.ButtonOnClick(btnBack, ClickBack, true);
    }
    public void ClickBack()
    {
        UIManager.Instance.ShowGarage(UIName.Home);
    }
    public void ClickSettings()
    {
        UIManager.Instance.ShowUI(UIName.Settings);
    }
    public void ClickRacing()
    {
        UIManager.Instance.ShowUI(UIName.SelectMode);
    }
    public void ClickUpgrade()
    {
        UIManager.Instance.ShowGarage(UIName.Upgrade);
    }
    public void ClickRacer()
    {
        UIManager.Instance.ShowGarage(UIName.Racer);
    }
    public void InitGara(GarageManager gara)
    {
        
    }
    public override void Show(Action onHideDone)
    {
        base.Show(onHideDone);
        CameraManager.Instance.helicopterCamera.gameObject.SetActive(false);
        RefreshOnShow();
    }
    private void RefreshOnShow()
    {
        currentItemClick = DataManager.Instance.GameData.idCurMotor;
        SpawnList();
        currentItemClick = GetIdMotorInAllMotor(currentItemClick);
        itemCache = listItemSelect[currentItemClick];
        itemCache.SelectMotorBike(true);
    }
    private void SpawnList()
    {
        int length = allDBMotor.Length;
        List<DB_Motorbike> listDB = new List<DB_Motorbike> ();
        for (int i = 0; i < length; i++)
        {
            int index = i;
            DB_Motorbike db = new DB_Motorbike();
            int idMotor = allDBMotor[index].idMotor;
            int [] levelUpgrades = new int[4];
            db.idMotor = idMotor;
            db.isHas = DataManager.Instance.IsHasMotor(idMotor, ref levelUpgrades);
            db.levelUpgrades = levelUpgrades;
            listDB.Add(db);
        }
        MasterHelper.InitListObj(length, itemSelectMotorbikeUIPf, listItemSelect, scrollView.content, (item, index) =>
        {
            item.gameObject.SetActive(true);
            var data = listDB[index];
            item.InitDB(data, ClickItem);
            item.currentIndex = index;
        });
    }
    public void ClickItem(ItemSelectMotorbikeUI itemSelectMotor)
    {
        if (itemCache != null)
        {
            itemCache.SelectMotorBike(false);
        }
        itemCache = itemSelectMotor;
        CurrentItemClick();
    }
    private void CurrentItemClick()
    {
        itemCache.SelectMotorBike(true);
        currentItemClick = itemCache.currentIndex;
        RefreshUI();
    }
    public override void RefreshUI()
    {
        base.RefreshUI();
        DB_Motor dbGet = DataManager.Instance.motorSO.GetDBMotor(itemCache.db_Motorbike.idMotor).Clone();
        int[] levelUpgrade = new int[4];
        bool isHasData = DataManager.Instance.IsHasMotor(itemCache.db_Motorbike.idMotor, ref levelUpgrade);
        bool[] isMaxData = DataManager.Instance.IsMaxDataArray(itemCache.db_Motorbike.idMotor);
        if (isHasData)
        {
            dbGet.inforMotorbike = dbGet.GetInforMotorbike(levelUpgrade);
            DataManager.Instance.GameData.idCurMotor = itemCache.db_Motorbike.idMotor;
            DataManager.Instance.SaveGameData();
        }
        btnUpgrade.interactable = isHasData;
        popupUpgrade.SelectDB(dbGet, isMaxData);
        if (isHasData)
        {
            DataManager.Instance.GameData.idCurMotor = dbGet.idMotor;
        }
        itemCache.SetUnLock(isHasData);
        SetStatusButtonRace();
        if (garageManager != null)
        {
            garageManager.SpawnMotorVisual(itemCache.db_Motorbike.idMotor);
            garageManager.SetMyCharacter();
        }
        scrollView.FocusOnRectTransform(itemCache.rectTransform);
    }

    private void SetStatusButtonRace()
    {
        DisableAllButton();
        bool isHasData = DataManager.Instance.IsHasMotor(itemCache.db_Motorbike.idMotor);
        if (isHasData)
        {
            btnRacing.gameObject.SetActive(true);
        }
        else
        {
            SetStatusIfDontBuy();
        }

    }
    private void SetStatusIfDontBuy()
    {
        dataTypeResourceCurrent = new DataTypeResource(RES_type.Bike, itemCache.db_Motorbike.idMotor);
        resourcesBuy = DataManager.Instance.resourceBuySO.GetResourcesBuy(dataTypeResourceCurrent, PlaceBuy.Garage);
        switch (resourcesBuy.typeBuy)
        {
            case TypeBuy.None:
                {
                    break;
                }
            case TypeBuy.Gold:
                {
                    StatusBuyGold();
                    break;
                }
            case TypeBuy.IAP:
                {
                    StatusBuyIAP();
                    break;
                }
            case TypeBuy.Ads:
                {
                    StatusBuyAds();
                    break;
                }
            case TypeBuy.Other:
                {
                    StatusOther();
                    break;
                }
        }
    }
    private void DisableAllButton()
    {
        btnBuy.gameObject.SetActive(false);
        btnRacing.gameObject.SetActive(false);
        obBuyAds.gameObject.SetActive(false);
        obBuyGold.gameObject.SetActive(false);
        obBuyIAP.gameObject.SetActive(false);
        obBuyOther.gameObject.SetActive(false);
    }
    private void StatusBuyGold()
    {
        btnBuy.gameObject.SetActive(true);
        obBuyGold.gameObject.SetActive(true);
        txtValueGold.text = resourcesBuy.valueBuy.ToString();
    }
    private void StatusBuyIAP()
    {
        btnBuy.gameObject.SetActive(true);
        obBuyIAP.gameObject.SetActive(true);
        txtValueIAP.text = resourcesBuy.valueBuy.ToString();
    }
    private void StatusBuyAds()
    {
        btnBuy.gameObject.SetActive(true);
        obBuyAds.gameObject.SetActive(true);
        int adsCur = DataManager.Instance.GetAdsCurrentResource(dataTypeResourceCurrent);
        int totalAds = resourcesBuy.valueBuy;
        txtValueAds.text = $"{adsCur}/{totalAds}";
    }
    private void StatusOther()
    {
        btnBuy.gameObject.SetActive(true);
        obBuyOther.gameObject.SetActive(true);
        txtValueOther.text = "Comming Soon";
    }
    private void ClickBuy()
    {
        switch (resourcesBuy.typeBuy)
        {
            case TypeBuy.None:
                {
                    break;
                }
            case TypeBuy.Gold:
                {
                    BuyGold();
                    break;
                }
            case TypeBuy.IAP:
                {
                    BuyIAP();
                    break;
                }
            case TypeBuy.Ads:
                {
                    BuyAds();
                    break;
                }
            case TypeBuy.Other:
                {
                    BuyOther();
                    break;
                }
        }
    }

    private void BuyGold()
    {
        AdsWrapperManager.Instance.ShowInter(KeyAds.BtnGarageBuyBikeGold, OnBuyGold);
    }
    private void OnBuyGold()
    {
        DataResource dataRemove = new DataResource(new DataTypeResource(RES_type.Gold), -resourcesBuy.valueBuy);
        DataManager.Instance.AddRes(dataRemove, OnBuyGoldDone);
    }
    private void OnBuyGoldDone()
    {
        DataManager.Instance.ReceiveRes(resourcesBuy.dataRes);
        AudioManager.Instance.PlaySFXUnlockMotor();
        currentItemClick = itemCache.currentIndex;
        DataManager.Instance.GameData.idCurMotor = resourcesBuy.dataRes.type.id;
        RefreshOnShow();
        RefreshUI();
    }
    private void BuyAds()
    {
        AdsWrapperManager.Instance.ShowReward(KeyAds.ClickButtonWatchAdsGetBike, OnDoneShowAds, OnFailedShowAds);
    }
    private void OnDoneShowAds()
    {
        DataTypeResource dataType = new DataTypeResource(RES_type.Bike, itemCache.currentIndex);
        DataManager.Instance.AddGetAdsResource(dataType,1);
        int curIndex = DataManager.Instance.GetAdsCurrentResource(dataType);
        int maxIndex = resourcesBuy.valueBuy;
        if(curIndex >= maxIndex)
        {
            AudioManager.Instance.PlaySFXUnlockMotor();
            DataManager.Instance.ReceiveRes(resourcesBuy.dataRes);
        }
        DataManager.Instance.GameData.idCurMotor = resourcesBuy.dataRes.type.id;
        RefreshOnShow();
        RefreshUI();
    }
    private void OnFailedShowAds()
    {
        UIManager.Instance.ShowToast(KeyToast.NoInternetLoadAds);
    }
    private void BuyIAP()
    {

    }
    private void BuyOther()
    {

    }
    public TweenAnimation[] tws; 

}
