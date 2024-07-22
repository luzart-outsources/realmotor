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
    public GameObject obLeaderboard;
    public Transform parentDashboard;
    public TMP_Text txtIndex;
    public ItemWinDashboardUI itemDashboardPf;
    public List<ItemWinDashboardUI> listItemWinDashboard = new List<ItemWinDashboardUI>();
    public Button btnHome;
    public Button btnReplay;
    public Button btnNext;

    [Space, Header("PopupSuccess")]
    public GameObject obSuccess;

    public TMP_Text txt_Title;

    public RefInforWinEndLevel[] refText;

    public RewardSliderXValue rewardSliderXValue;
    public Button btnMissOut;
    private DataValueWin dataWin;

    protected override void Setup()
    {
        base.Setup();
        GameUtil.ButtonOnClick(btnMissOut, ClickMissOut, true);
        GameUtil.ButtonOnClick(btnHome, ClickHome, true);
        GameUtil.ButtonOnClick(btnReplay, ClickReplay, true);
        GameUtil.ButtonOnClick(btnNext, ClickNext, true);
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
    public void ClickNext()
    {
        obLeaderboard.SetActive(false);
        obSuccess.SetActive(true);
        rewardSliderXValue.Initialize(dataWin.Total, 1, ClickClaimReward);
    }
    private List<DataItemWinLeaderboardUI> listDataItemWinLeaderboardUI = new List<DataItemWinLeaderboardUI>();
    public void InitDataDashboard(List<DataItemWinLeaderboardUI> listData)
    {
        this.listDataItemWinLeaderboardUI = listData;
        int indexMe = GameManager.Instance.gameCoordinator.countLeaderBoard;
        txtIndex.text = GameUtil.ToOrdinal(indexMe+1);
        var list = listDataItemWinLeaderboardUI;
        int length = list.Count;
        MasterHelper.InitListObj(length, itemDashboardPf, listItemWinDashboard, parentDashboard, (item, index) =>
        {
            item.gameObject.SetActive(true);
            bool isMe = indexMe == index;
            item.InitData(isMe, list[index]);
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
        obLeaderboard.SetActive(true);
        obSuccess.SetActive(false);
    }
    private bool IsShowCoinInWin = false;
    public void OnClickClaimReward(float x)
    {
        DataManager.Instance.ReceiveRes(new DataResource(new DataTypeResource(RES_type.Gold), (int)((x - 1) * dataWin.Total)));
        UIManager.Instance.ShowCoinSpawn(null, OnHide);
    }
    private void ClickClaimReward(float x)
    {
        AdsWrapperManager.Instance.ShowReward(KEYADS.ClickButtonXRewardOnWin, () =>
        {
            IsShowCoinInWin = true;
            OnClickClaimReward(x);
        },
        () =>
        {
            UIManager.Instance.ShowToast(KEYTOAST.NoInternetLoadAds);
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
        UIManager.Instance.HideAll();
        UIManager.Instance.ShowGarage();
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
