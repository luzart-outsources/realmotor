using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIDailyReward : UIBase
{
    private DailyRewardManager dailyRewardManager
    {
        get
        {
            return DataManager.Instance.dailyRewardManager;
        }
    }
    public ItemDailyRewardUI[] itemDailyRewardUIs;
    public Button btnX2Reward;
    protected override void Setup()
    {
        base.Setup();
        GameUtil.ButtonOnClick(btnX2Reward, ClickX2Reward, true);
    }
    private void ClickX2Reward()
    {
        AdsWrapperManager.Instance.ShowReward(KeyAds.ClickItemX2DailyReward, OnCompleteX2Reward, ShowToast);
    }
    private void OnCompleteX2Reward()
    {
        var itemDailyReward = itemDailyRewardUIs[dailyRewardManager.Today];
        var data = itemDailyReward.groupDataResources.groupDataResources;
        int length = data.Length;
        DataResource[] dataGold = new DataResource[length];
        for (int i = 0; i < length; i++)
        {
            dataGold[i] = data[i].Clone();
        }
        for (int i = 0; i < length; i++)
        {
            if (dataGold[i].type.type == RES_type.Gold)
            {
                dataGold[i].amount = dataGold[i].amount * 2;
            }
        }
        DataManager.Instance.ReceiveRes(dataGold);
        dailyRewardManager.ClaimReward(dailyRewardManager.Today);
        var ui = UIManager.Instance.ShowUI<UIReceiveRes>(UIName.ReceiveRes);
        ui.Initialize(RefreshUI, dataGold);

    }
    public override void Show(Action onHideDone)
    {
        base.Show(onHideDone);
        RefreshUI();
    }
    private void OnSetStatusButton()
    {

    }
    public override void RefreshUI()
    {
        base.RefreshUI();
        int length = itemDailyRewardUIs.Length;
        for (int i = 0; i < length; i++)
        {
            int index = i;
            var dataDailyRewardUI = dailyRewardManager.dataResourceDaily[index];
            var item = itemDailyRewardUIs[index];
            EStateClaimReward eState = GetEStateClaim(index);
            item.Initialize(index, eState, dataDailyRewardUI, ClickItem);
        }
        int today = dailyRewardManager.Today;
        if(today >= itemDailyRewardUIs.Length)
        {
            btnX2Reward.gameObject.SetActive(false);
            return;
        }
        bool isEnableX2 = itemDailyRewardUIs[today].eStateClaim == EStateClaimReward.CanReceive;
        btnX2Reward.gameObject.SetActive(isEnableX2);
    }
    private EStateClaimReward GetEStateClaim(int index)
    {
        if (index == dailyRewardManager.Today)
        {
            return dailyRewardManager.IsClaimDay(index) ? EStateClaimReward.Received : EStateClaimReward.CanReceive;
        }
        else if (index < dailyRewardManager.Today)
        {
            return dailyRewardManager.IsClaimDay(index) ? EStateClaimReward.Received : EStateClaimReward.DontReceived;
        }
        else
        {
            return EStateClaimReward.WillReceive;
        }
    }
    private void ClickItem(ItemDailyRewardUI itemDailyRewardUI)
    {
        dailyRewardManager.ClaimReward(itemDailyRewardUI.index);
        if(itemDailyRewardUI.eStateClaim == EStateClaimReward.DontReceived)
        {
            AdsWrapperManager.Instance.ShowReward(KeyAds.ClickItemDailyReward, ()=> OnDoneReceive(itemDailyRewardUI), ShowToast);
        }
        else if(itemDailyRewardUI.eStateClaim == EStateClaimReward.CanReceive)
        {
            OnDoneReceive(itemDailyRewardUI);
        }
        RefreshUI();
    }
    private void OnDoneReceive(ItemDailyRewardUI itemDailyRewardUI)
    {
        DataManager.Instance.ReceiveRes(itemDailyRewardUI.groupDataResources.groupDataResources);
        var ui = UIManager.Instance.ShowUI<UIReceiveRes>(UIName.ReceiveRes);
        ui.Initialize(Hide, itemDailyRewardUI.groupDataResources.groupDataResources);
        if(!DataManager.Instance.GameData.isBeginnerBundle) UIManager.Instance.ShowUI(UIName.BeginnerBundle);
    }
    private void ShowToast()
    {
        UIManager.Instance.ShowToast(KeyToast.NoInternetLoadAds);
    }
}
