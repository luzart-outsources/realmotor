using BG_Library.IAP;
using BG_Library.NET;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;


public class ShopIAP : MonoBehaviour
{
    public GroupDataResources[] dataResource = new GroupDataResources[5];
    private void Awake()
    {
        IAPManager.PurchaseResultListener += OnPurchaseComplete;
    }

    private void OnDestroy()
    {
        IAPManager.PurchaseResultListener -= OnPurchaseComplete;
    }

    private void OnPurchaseComplete(IAPPurchaseResult iAPPurchaseResult)
    {
        switch (iAPPurchaseResult.Result)
        {
            case IAPPurchaseResult.EResult.Complete:
                var rew = iAPPurchaseResult.Product.Rewards;
                foreach (var iap in rew)
                    BuySuccess(iap);

                break;
            case IAPPurchaseResult.EResult.Restore:
                break;
            case IAPPurchaseResult.EResult.WrongProduct:
                // Purchase faield: can't find product with id 
                break;
            case IAPPurchaseResult.EResult.WrongInstance:
                // Purchase faield: IAP Manager instance null (Read Setup IAP)   
                break;
            case IAPPurchaseResult.EResult.WrongStoreController:
                // Purchase faield: IAP initialized faield
                break;
            case IAPPurchaseResult.EResult.PurchaseFailed:
                break;
            default:
                break;
        }
    }

    void BuySuccess(IAPProductStats.PurchaseReward product)
    {
        /*if (product.Reward == IAPProductStats.EReward.REMOVE_AD)
        {
            AdsManager.Ins.PurchaseRemoveAds();
            return;
        }*/
        var ui = UIManager.Instance.ShowUI<UIReceiveRes>(UIName.ReceiveRes);

        switch (product.atlas)
        {
            case "Cash":
                
                DataManager.Instance.ReceiveRes(new DataResource(new DataTypeResource(RES_type.Gold), product.PackRewardValue));
                ui.Initialize(() => {
                    var uiShop = UIManager.Instance.GetUiActive<UIShop>(UIName.Shop);
                    uiShop.RefreshUI();
                    UIManager.Instance.RefreshUI();
                }, new DataResource(new DataTypeResource(RES_type.Gold), product.PackRewardValue));
                Observer.Instance.Notify(ObserverKey.CoinObserverDontAuto, false);
                break;
            case "RemoveAds":
                AdsManager.Ins.PurchaseRemoveAds();
                Observer.Instance.Notify(ObserverKey.CoinObserverDontAuto, false);
                DataManager.Instance.ReceiveRes(dataResource[0].groupDataResources);
                ui.Initialize(() => {
                    var uiShop = UIManager.Instance.GetUiActive<UIShop>(UIName.Shop);
                    uiShop.RefreshUI();
                    UIManager.Instance.RefreshUI();
                }, dataResource[0].groupDataResources);

                break;
            case "Motor":
                DataManager.Instance.ReceiveRes(dataResource[1].groupDataResources);
                DataManager.Instance.GameData.isBeginnerBundle = true;
                DataManager.Instance.SaveGameData();
                Observer.Instance.Notify(ObserverKey.CoinObserverDontAuto, false);
                ui.Initialize(() => {
                    var uiGarage = UIManager.Instance.GetUiActive<UIGarage>(UIName.Shop);
                    UIManager.Instance.RefreshUI();
                }, dataResource[1].groupDataResources);
                break;
        }
    }


}

public enum ShopItemType
{
    None = 0,
    RemoveAds = 1,
    WatchAds = 2,
    BeginnerBundle = 3,
    BuyCoin = 4,
}
