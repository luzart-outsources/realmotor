//using BG_Library.IAP;
using BG_Library.NET;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRemoveAds : UIBase
{
    public GroupDataResources dataResource = new GroupDataResources();
    public CanvasGroup canvasGroup;
    public Button btnBack;

    //private void Awake()
    //{
    //    canvasGroup.alpha = 0f;
    //    IAPManager.PurchaseResultListener += OnPurchaseComplete;
    //}

    //private void OnDestroy()
    //{
    //    IAPManager.PurchaseResultListener -= OnPurchaseComplete;
    //}

    //protected override void Setup()
    //{
    //    canvasGroup.DOFade(1f, 0.4f);
    //    GameUtil.ButtonOnClick(btnBack, ClickBack, true);
    //}
    //public override void Show(Action onHideDone)
    //{
    //    base.Show(onHideDone);
    //}
    //public override void RefreshUI()
    //{
    //    base.RefreshUI();
    //}

    //public void ClickBack()
    //{
    //    canvasGroup.DOFade(0f, 0.3f).OnComplete(Hide);
    //    //UIManager.Instance.ShowUI(UIName.Home);
    //}
    //private void OnPurchaseComplete(IAPPurchaseResult iAPPurchaseResult)
    //{
    //    switch (iAPPurchaseResult.Result)
    //    {
    //        case IAPPurchaseResult.EResult.Complete:
    //            var rew = iAPPurchaseResult.Product.Rewards;
    //            foreach (var iap in rew)
    //                BuySuccess(iap);

    //            break;
    //        case IAPPurchaseResult.EResult.Restore:
    //            break;
    //        case IAPPurchaseResult.EResult.WrongProduct:
    //            // Purchase faield: can't find product with id 
    //            break;
    //        case IAPPurchaseResult.EResult.WrongInstance:
    //            // Purchase faield: IAP Manager instance null (Read Setup IAP)   
    //            break;
    //        case IAPPurchaseResult.EResult.WrongStoreController:
    //            // Purchase faield: IAP initialized faield
    //            break;
    //        case IAPPurchaseResult.EResult.PurchaseFailed:
    //            break;
    //        default:
    //            break;
    //    }
    //}

    //void BuySuccess(IAPProductStats.PurchaseReward product)
    //{
    //    /*if (product.Reward == IAPProductStats.EReward.REMOVE_AD)
    //    {
    //        AdsManager.Ins.PurchaseRemoveAds();
    //        return;
    //    }*/
    //    var ui = UIManager.Instance.ShowUI<UIReceiveRes>(UIName.ReceiveRes);

    //    switch (product.atlas)
    //    {
    //        case "RemoveAds":
    //            AdsManager.Ins.PurchaseRemoveAds();
    //            Observer.Instance.Notify(ObserverKey.CoinObserverDontAuto, false);
    //            DataManager.Instance.ReceiveRes(dataResource.groupDataResources);
    //            ui.Initialize(() => {
    //                Hide();
    //                var uiShop = UIManager.Instance.GetUiActive<UIShop>(UIName.Home);
    //                UIManager.Instance.RefreshUI();
    //            }, dataResource.groupDataResources);

    //            break;
            
    //    }
    //}

}
