using DG.Tweening;
using Eco.TweenAnimation;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIWinClassic : UIBase
{
    [Space, Header("PopupDashboard")]
    public TweenAnimation twDashboard;
    public Leaderboard leaderboard;
    public Transform parentDashboard;
    public TMP_Text txtIndex;
    public ItemWinDashboardUI itemDashboardPf;

    public Button btnHome;
    public Button btnReplay;
    public Button btnNext;

    [Space, Header("PopupSuccess")]
    public TweenAnimation twSuccess;
    public GameObject obSuccess;

    public TMP_Text txt_Title;

    public RefInforWinEndLevel[] refText;

    public RewardSliderXValue rewardSliderXValue;
    public Button btnMissOut;


    private DataValueWin dataWin;

    protected override void Setup()
    {
        base.Setup();
        GameUtil.ButtonOnClick(btnMissOut, ClickMissOut, true, KeyAds.BtnWinClassicMissOut);
        GameUtil.ButtonOnClick(btnHome, ClickHome, true);
        GameUtil.ButtonOnClick(btnReplay, ClickReplay, true);
        GameUtil.ButtonOnClick(btnNext, ClickNext, true, KeyAds.BtnWinClassicNext);
    }
    public override void Show(Action onHideDone)
    {
        base.Show(onHideDone);
        IsShowCoinInWin = false;
    }
    private void ClickHome()
    {
        UIManager.Instance.HideAll();
        UIManager.Instance.ShowGarage();
    }
    private void ClickReplay()
    {
        GameManager.Instance.Restart();
    }
    private Tween twMissOut;
    public void ClickNext()
    {
        leaderboard.gameObject.SetActive(false);
        obSuccess.SetActive(true);
        rewardSliderXValue.Initialize(dataWin.Total, 1, ClickClaimReward);
        twDashboard.Show();
        twSuccess.Show();
        twMissOut?.Kill();
        twMissOut = DOVirtual.DelayedCall(3f, () =>
        {
            btnMissOut.gameObject.SetActive(true);
        });
    }
    public List<ItemLeaderboard> listItemWinDashboard = new List<ItemLeaderboard>();
    private List<DataItemWinLeaderboardUI> listDataItemWinLeaderboardUI = new List<DataItemWinLeaderboardUI>();
    private int indexMe;
    public void InitDataDashboard(List<DataItemWinLeaderboardUI> listData)
    {
        this.listDataItemWinLeaderboardUI = listData;
        indexMe = GameManager.Instance.gameCoordinator.countLeaderBoard;
        txtIndex.text = GameUtil.ToOrdinal(indexMe+1);
        var list = listDataItemWinLeaderboardUI;
        var newItem = list[indexMe];
        int length = list.Count;
        listDataItemWinLeaderboardUI.Remove(newItem);
        newItem.index = (length).ToString();
        listDataItemWinLeaderboardUI.Add(newItem);
        leaderboard.gameObject.SetActive(true);
        leaderboard.InitSpawn(length, listItemWinDashboard, (itemLeaderboard, index) =>
        {
            var item = (ItemWinDashboardUI)itemLeaderboard;
            item.gameObject.SetActive(true);
            bool isMe = indexMe == length-1;
            var data = list[index];
            item.InitData(isMe, data);
        });

        obSuccess.SetActive(false);
        int indexCurrent = listDataItemWinLeaderboardUI.Count - 1;

        leaderboard.MoveItem(indexCurrent, indexMe, null, (item) =>
        {
            ClickNext();
        },
        (item, index) =>
        {
            var itemLeaderboard = (ItemWinDashboardUI)item;
            var data = listDataItemWinLeaderboardUI[indexCurrent];
            data.index = (index+1).ToString();
            itemLeaderboard.InitData(true, data);
        });
    }
    public void InitDataRes(bool isWin, DataValueWin db)
    {
        this.dataWin = db;
        if (isWin )
        {
            txt_Title.text = "Success";
        }
        else
        {
            txt_Title.text = "Failed";
        }
        refText[0].InitData("Position", $"{dataWin.valuePos}");
        refText[1].InitData("Result", $"{dataWin.valueResult}");
        refText[2].InitData("Level", $"{dataWin.valueLevel}");
        refText[3].InitData("Join", $"{dataWin.valueJoin}");
        refText[4].InitData("Total earning", $"{dataWin.Total}");
      
    }
    public void OnShowPopUp()
    {

    }
    private bool IsShowCoinInWin = false;
    public void OnClickClaimReward(float x)
    {
        DataManager.Instance.ReceiveRes(new DataResource(new DataTypeResource(RES_type.Gold), (int)((x - 1) * dataWin.Total)));
        UIManager.Instance.ShowCoinSpawn(null, OnHide);
    }
    private void ClickClaimReward(float x)
    {
        AdsWrapperManager.Instance.ShowReward(KeyAds.ClickButtonXRewardOnWin, () =>
        {
            IsShowCoinInWin = true;
            OnClickClaimReward(x);
        },
        () =>
        {
            UIManager.Instance.ShowToast(KeyToast.NoInternetLoadAds);
        });
    }
    private void ClickMissOut()
    {
        if (!IsShowCoinInWin)
        {
            UIManager.Instance.ShowCoinSpawn(null, OnHide);
        }

    }
    private void OnHide()
    {
        UIManager.Instance.LoadScene(() =>
        {
            UIManager.Instance.HideAllUIIgnore();
            UIManager.Instance.ShowGarage();
        }, null,1,1);
    }
}
[System.Serializable]
public class DataValueWin
{
    public int valuePos;
    public int valueResult;
    public int valueLevel;
    public int valueJoin;
    public int Total
    {
        get
        {
            return valuePos + valueResult + valueLevel + valueJoin;
        }
    }
}
[System.Serializable]
public class RefInforWinEndLevel
{
    public TMP_Text txtTitle;
    public TMP_Text txtValue;
    public void InitData(string title, string value)
    {
        txtTitle.text = title;
        txtValue.text = value;
    }
}
