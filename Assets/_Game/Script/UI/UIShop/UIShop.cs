using BG_Library.IAP;
using BG_Library.NET;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class UIShop : UIBase
{
    public Button btnBack;
    public List<ItemShopUI> listItemShopUIs = new List<ItemShopUI>();

    protected override void Setup()
    {
        base.Setup();
        GameUtil.ButtonOnClick(btnBack, ClickBack, true, KeyAds.BtnShopBack);
    }
    public override void Show(Action onHideDone)
    {
        base.Show(onHideDone);
    }

    void ClickBack()
    {
        Hide();
        //UIManager.Instance.ShowGarage(UIName.Home);
        /*if (!AdsManager.IAP_RemoveAds)
        {
            UIManager.Instance.ShowUI(UIName.RemoveAds);
        }
        else
        {
            if(!DataManager.Instance.isBeginnerBundle) UIManager.Instance.ShowUI(UIName.BeginnerBundle);
        }*/
    }
   
}
