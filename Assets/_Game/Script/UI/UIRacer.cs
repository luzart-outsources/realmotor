using AirFishLab.ScrollingList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRacer : UIBase
{
    public GarageManager garageManager;
    public ButtonSelect btnRacer;
    public ButtonSelect btnClothes;
    public Button btnBack;

    public GameObject obScrollHelmet;
    public ScrollViewRacerUI scrollViewHelmet;
    public GameObject obScrollBody;
    public ScrollViewRacerUI scrollViewBody;
    private ItemScrollViewRacer itemCache;
    public Button btnEquip;
    public Button btnBuy;
    public GameObject obEquipped;
    public GameObject obBuyAds, obBuyGold, obBuyIAP, obBuyOther;
    public TMP_Text txtValueGold, txtValueAds,txtValueIAP, txtValueOther;
    protected override void Setup()
    {
        base.Setup();
        btnRacer.InitAction(0,ClickRacer);
        btnClothes.InitAction(1,ClickClothes);
        GameUtil.ButtonOnClick(btnBack,Hide);
        GameUtil.ButtonOnClick(btnEquip, ClickSelect, true);
        GameUtil.ButtonOnClick(btnBuy,ClickBuy, true);
    }
    public override void Hide()
    {
        base.Hide();
        UIManager.Instance.ShowGarage();
    }

    private bool isOnRacer = true;
    public override void Show(Action onHideDone)
    {
        base.Show(onHideDone);
    }
    public void RefreshUIOnShow() 
    {
        ClickRacer(null,0);
        garageManager.SpawnMotorVisual(DataManager.Instance.GameData.idCurMotor);
    }
    public void ClickRacer(ButtonSelect btn, int ia)
    {
        isOnRacer = true;
        btnRacer.Select(true);
        btnClothes.Select(false);
        obScrollBody.gameObject.SetActive(false);
        obScrollHelmet.gameObject.SetActive(true);
        var data = DataManager.Instance.resourceBuySO.dbResBuyHelmet
            .ToList();
        int length = data.Count;
        for (int i = 0; i < length; i++)
        {
            var item = data[i];
            item.isHas = DataManager.Instance.IsHasHelmet(item.dataRes.type.id);
        }
        scrollViewHelmet.InitListDB(data);
        int index = DataManager.Instance.GameData.curCharacter.idHelmet;
        scrollViewHelmet.circular.SelectContentID(index);
        itemCache = (ItemScrollViewRacer)scrollViewHelmet.circular.GetFocusingBox();
        RefreshUI();
    }
    public void ClickClothes(ButtonSelect btn,int a)
    {
        isOnRacer = false;
        btnRacer.Select(false);
        btnClothes.Select(true);
        obScrollHelmet.gameObject.SetActive(false);
        obScrollBody.gameObject.SetActive(true);
        var data = DataManager.Instance.resourceBuySO.dbResBuyBody
            .ToList();
        int length = data.Count;
        for (int i = 0; i < length; i++)
        {
            var item = data[i];
            item.isHas = DataManager.Instance.IsHasBody(item.dataRes.type.id);
        }
        scrollViewBody.InitListDB(data);
        int index = DataManager.Instance.GameData.curCharacter.idClothes;
        scrollViewBody.circular.SelectContentID(index);
        itemCache = (ItemScrollViewRacer)scrollViewBody.circular.GetFocusingBox();
        itemCache.SelectMotorBike(true);
        RefreshUI();
    }
    public void ClickItem(ListBox listBoxOld, ListBox listBoxNew)
    {
        ItemScrollViewRacer itemOld = (ItemScrollViewRacer)listBoxOld;
        if (itemOld != null)
        {
            itemOld.SelectMotorBike(false);
        }
        itemCache = (ItemScrollViewRacer)listBoxNew;
        SelectCurrent();
    }
    public void ClickItem(ListBox listBoxNew)
    {
        if (itemCache != null)
        {
            itemCache.SelectMotorBike(false);
        }
        ItemScrollViewRacer itemSelectMotor = (ItemScrollViewRacer)listBoxNew;
        itemCache = itemSelectMotor;
        SelectCurrent();
    }
    private void SelectCurrent()
    {
        itemCache.SelectMotorBike(true);
        RefreshUI();
    }
    public override void RefreshUI()
    {
        base.RefreshUI();
        itemCache.SelectMotorBike(true);
        Select(itemCache.resourcesBuy.dataRes.type);
        RefreshButton();
    }
    private void Select(DataTypeResource dataType)
    {
        //var dataType = item.resourcesBuy.dataRes.type;
        if (dataType.type == RES_type.Helmet)
        {
            garageManager.ChangeIdHelmet(dataType.id);
        }
        else
        {
            garageManager.ChangeIdBody(dataType.id);
        }
    }
    private EStateClaim eStateClaim;
    private void DisableAllButtonSelect()
    {
        btnBuy.gameObject.SetActive(false);
        btnEquip.gameObject.SetActive(false);
        obEquipped.gameObject.SetActive(false);
    }
    private void RefreshButton()
    {

        //
        resourcesBuy = itemCache.resourcesBuy;
        bool isHas = false;
        switch(resourcesBuy.dataRes.type.type)
        {
            case RES_type.Helmet:
                {
                    isHas = DataManager.Instance.IsHasHelmet(resourcesBuy.dataRes.type.id);

                    break;
                }
            case RES_type.Body:
                {
                    isHas = DataManager.Instance.IsHasBody(resourcesBuy.dataRes.type.id);
                    break;
                }
        }
        if(isHas)
        {
            eStateClaim = EStateClaim.CanEquip;
            switch (resourcesBuy.dataRes.type.type)
            {
                case RES_type.Helmet:
                    {
                        if (DataManager.Instance.GameData.curCharacter.idHelmet == resourcesBuy.dataRes.type.id)
                        {
                            eStateClaim = EStateClaim.None;
                        }
                        break;
                    }
                case RES_type.Body:
                    {
                        if (DataManager.Instance.GameData.curCharacter.idClothes == resourcesBuy.dataRes.type.id)
                        {
                            eStateClaim = EStateClaim.None;
                        }
                        break;
                    }
            }
        }
        else
        {
            eStateClaim = EStateClaim.Buy;
            SetStatusIfDontBuy();
        }
        SetAllStateClaim();
    }
    private void SetAllStateClaim()
    {
        DisableAllButtonSelect();
        switch (eStateClaim)
        {
            case EStateClaim.Buy:
                {
                    btnBuy.gameObject.SetActive(true);
                    break;
                }
            case EStateClaim.None:
                {
                    obEquipped.gameObject.SetActive(true);
                    break;
                }
            case EStateClaim.CanEquip:
                {
                    btnEquip.gameObject.SetActive(true);
                    break;
                }
        }
    }
    private DB_ResourcesBuy resourcesBuy;

    private void ClickSelect()
    {
        switch (resourcesBuy.dataRes.type.type)
        {
            case RES_type.Helmet:
                {
                    DataManager.Instance.GameData.curCharacter.idHelmet = resourcesBuy.dataRes.type.id;
                    break;
                }
            case RES_type.Body:
                {
                    DataManager.Instance.GameData.curCharacter.idClothes = resourcesBuy.dataRes.type.id;
                    break;
                }
        }
        RefreshUI();
    }
    private void SetStatusIfDontBuy()
    {
        DisableAllButton();
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
        btnEquip.gameObject.SetActive(false);
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
        int adsCur = DataManager.Instance.GetAdsCurrentResource(resourcesBuy.dataRes.type);
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
        DataResource dataRemove = new DataResource(new DataTypeResource(RES_type.Gold), -resourcesBuy.valueBuy);
        DataManager.Instance.AddRes(dataRemove, OnBuyGoldDone);
    }
    private void OnBuyGoldDone()
    {
        DataManager.Instance.ReceiveRes(resourcesBuy.dataRes);
        itemCache.SetLock(false);
        RefreshUI();
    }
    private void BuyAds()
    {
        switch (resourcesBuy.dataRes.type.type)
        {
            case RES_type.Helmet:
                {
                    AdsWrapperManager.Instance.ShowReward(KEYADS.ClickButtonWatchAdsGetHelmet, OnDoneShowAds, OnFailedShowAds);
                    break;
                }
            case RES_type.Body:
                {
                    AdsWrapperManager.Instance.ShowReward(KEYADS.ClickButtonWatchAdsGetBody, OnDoneShowAds, OnFailedShowAds);
                    break;
                }
        }
        
    }
    private void OnDoneShowAds()
    {

        DataTypeResource dataType = new DataTypeResource(resourcesBuy.dataRes.type.type, itemCache.currentIndex);
        DataManager.Instance.AddGetAdsResource(dataType, 1);
        int curIndex = DataManager.Instance.GetAdsCurrentResource(dataType);
        int maxIndex = resourcesBuy.valueBuy;
        if (curIndex >= maxIndex)
        {
            DataManager.Instance.ReceiveRes(resourcesBuy.dataRes);
            itemCache.SetLock(false);
        }
        RefreshUI();
    }
    private void OnFailedShowAds()
    {
        var ui = UIManager.Instance.ShowUI<UIToast>(UIName.Toast);
        ui.Init(KEYTOAST.NoInternetLoadAds);
    }
    private void BuyIAP()
    {

    }
    private void BuyOther()
    {

    }
    //private void InitScrollItemRacer()
    //{
    //    var data = DataManager.Instance.resourceBuySO.dbResBuyHelmet;
    //    int idHelmet = DataManager.Instance.GameData.curCharacter.idHelmet;
    //    ItemScrollViewRacer itemCurrent = null;
    //    MasterHelper.InitListObj<ItemScrollViewRacer>(data.Length, itemPf, listItemScrollViewRacer, parent, (item, index) =>
    //    {
    //        item.gameObject.SetActive(true);
    //        int id = data[index].dataRes.type.id;
    //        bool isBuy = DataManager.Instance.IsHasHelmet(id);
    //        item.Init(data[index], isBuy, ClickItem);
    //        if (id == idHelmet)
    //        {
    //            itemCurrent = item;
    //        }
    //    });
    //    UnAndSelectItem(itemCurrent);
    //}
    //private void InitScrollItemClothes()
    //{
    //    var data = DataManager.Instance.resourceBuySO.dbResBuyBody;
    //    int idBody = DataManager.Instance.GameData.curCharacter.idClothes;
    //    ItemScrollViewRacer itemCurrent = null;
    //    MasterHelper.InitListObj<ItemScrollViewRacer>(data.Length, itemPf, listItemScrollViewRacer, parent, (item, index) =>
    //    {
    //        item.gameObject.SetActive(true);
    //        int id = data[index].dataRes.type.id;
    //        bool isBuy = DataManager.Instance.IsHasBody(id);
    //        item.Init(data[index], isBuy, ClickItem);
    //        if (id == idBody)
    //        {
    //            itemCurrent = item;
    //        }
    //    });
    //    UnAndSelectItem(itemCurrent);
    //}
    //private void UnAndSelectItem(ItemScrollViewRacer item)
    //{
    //    if (itemCache != null)
    //    {
    //        itemCache.SelectedItem(false);
    //    }
    //    item.SelectedItem(true);
    //    if(item.dbResBuy.dataRes.type.type == RES_type.Helmet)
    //    {
    //        garageManager.ChangeIdHelmet(item.dbResBuy.dataRes.type.id);
    //    }
    //    else
    //    {
    //        garageManager.ChangeIdBody(item.dbResBuy.dataRes.type.id);
    //    }

    //}
    //private void RefreshUIBody()
    //{
    //    var data = DataManager.Instance.resourceBuySO.dbResBuyBody;
    //    for (int i = 0; i < listItemScrollViewRacer.Count; i++)
    //    {
    //        var index = i;
    //        var item = listItemScrollViewRacer[index];

    //        int id = data[index].dataRes.type.id;
    //        bool isBuy = DataManager.Instance.IsHasBody(id);
    //        //item.Init(data[index], isBuy, ClickItem);
    //    }
    //}
    //private void RefreshUIHelmet()
    //{
    //    var data = DataManager.Instance.resourceBuySO.dbResBuyHelmet;
    //    for (int i = 0; i < listItemScrollViewRacer.Count; i++)
    //    {
    //        var index = i;
    //        var item = listItemScrollViewRacer[index];

    //        int id = data[index].dataRes.type.id;
    //        bool isBuy = DataManager.Instance.IsHasHelmet(id);
    //        //item.Init(data[index], isBuy, ClickItem);
    //    }
    //}
    //public override void RefreshUI()
    //{
    //    base.RefreshUI();
    //    if (isOnRacer)
    //    {
    //        RefreshUIHelmet();
    //    }
    //    else
    //    {
    //        RefreshUIBody();
    //    }
    //}

}
public enum EStateClaim
{
    None = 0,
    CanEquip = 1,
    Buy = 2,
}
