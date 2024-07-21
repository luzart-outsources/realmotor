using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWinClassic : UIBase
{
    [Space, Header("PopupDashboard")]
    public Transform parentDashboard;
    public TMP_Text txtIndex;
    public ItemWinDashboardUI itemDashboardPf;
    public List<ItemWinDashboardUI> listItemWinDashboard = new List<ItemWinDashboardUI>();

    [Space, Header("PopupSuccess")]

    public TMP_Text txt_Title;

    public RefInforWinEndLevel[] refText;

    public RewardSliderXValue rewardSliderXValue;
    public Button btnMissOut;
    private DataValueWin dataWin;

    protected override void Setup()
    {
        base.Setup();
        GameUtil.ButtonOnClick(btnMissOut, ClickMissOut, true);
    }
    public void InitDataDashboard()
    {
        int indexMe = 0;
        txtIndex.text = GameUtil.ToOrdinal(indexMe);
        var list = new List<DataItemWinLeaderboardUI>();
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
       
        rewardSliderXValue.Initialize(dataWin.Total, 1, ClickClaimReward);
    }
    public void OnClickClaimReward(float x)
    {
        DataManager.Instance.ReceiveRes(new DataResource(new DataTypeResource(RES_type.Gold), (int)((x - 1) * dataWin.Total)));
        ClickMissOut();
    }
    private void ClickClaimReward(float x)
    {
        AdsWrapperManager.Instance.ShowReward(KEYADS.ClickButtonXRewardOnWin, () =>
        {
            OnClickClaimReward(x);
        },
        () =>
        {
            UIManager.Instance.ShowToast(KEYTOAST.NoInternetLoadAds);
        });
    }
    private void ClickMissOut()
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
