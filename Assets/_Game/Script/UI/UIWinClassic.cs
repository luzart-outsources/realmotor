using DG.Tweening;
using Eco.TweenAnimation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWinClassic : UIBase
{
    public TMP_Text txtIndex;
    public TMP_Text txtOrdinal;
    public TMP_Text txt_Title;
    public BaseSelect baseSelect;

    public Animation animationFlag;
    public CanvasGroup canvasFade;
    public GameObject obScreen;

    [Space, Header("PopupDashboard")]
    public TweenAnimation twDashboard;
    public Leaderboard leaderboard;
    public Transform parentDashboard;

    public ItemWinDashboardUI itemDashboardPf;

    public Button btnHome;
    public Button btnReplay;
    public Button btnNext;

    [Space, Header("PopupSuccess")]
    public TweenAnimation twSuccess;
    public GameObject obSuccess;



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
        AudioManager.Instance.PlaySFXWin();
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
        btnMissOut.gameObject.SetActive(false);
        twMissOut = DOVirtual.DelayedCall(1.5f, () =>
        {
            btnMissOut.gameObject.SetActive(true);
        });
        PushFirebaseInforGold();
    }
    private void PushFirebaseInforGold()
    {
        ParameterFirebaseCustom param = new ParameterFirebaseCustom(KeyTypeFirebase.Level, level.ToString());
        FirebaseNotificationLog.LogWithLevelMax(KeyFirebase.StepShowInforGold, param);
    }


    public List<ItemLeaderboard> listItemWinLeaderboard = new List<ItemLeaderboard>();
    private List<DataItemWinLeaderboardUI> listDataItemWinLeaderboardUI = new List<DataItemWinLeaderboardUI>();
    private int indexMe;

    public void SetAnimationFlag()
    {
        float length = animationFlag.clip.length;
        animationFlag.gameObject.SetActive(true);
        //DOVirtual.DelayedCall(length, () =>
        //{
        //    animationFlag.gameObject.SetActive(false);
        //}).SetId(this);
    }
    public Tween SetCanvasGroupFade()
    {
        return DOVirtual.Float(0, 1, 0.5f, (x) =>
        {
            canvasFade.alpha = x;
        }).SetId(this);
    }
    public Transform targetFlag;
    private Sequence sqVisual;
    public Action OnShowCompleteAnim;
    public void OnSequenceVisual()
    {
        sqVisual = DOTween.Sequence();
        sqVisual.Append(SetCanvasGroupFade());
        sqVisual.InsertCallback(0.3f,SetAnimationFlag);
        sqVisual.InsertCallback(0.8f, ()=> animationFlag.transform.DOMove(targetFlag.position,0.3f));
        sqVisual.AppendInterval(0.5f);
        sqVisual.AppendCallback(() =>
        {
            obScreen.SetActive(true);
            OnMoveItem();
            OnShowCompleteAnim?.Invoke();
            PushFirebaseOnShowLeaderboard();
        });
    }
    private void PushFirebaseOnShowLeaderboard()
    {
       ParameterFirebaseCustom param = new ParameterFirebaseCustom(KeyTypeFirebase.Level, level.ToString());
       FirebaseNotificationLog.LogWithLevelMax(KeyFirebase.StepShowLeaderboard, param);
    }
    private int level;
    public void InitLevel(int level)
    {
        this.level = level;
    }

    public void InitDataDashboard(List<DataItemWinLeaderboardUI> listData)
    {
        this.listDataItemWinLeaderboardUI = listData;
        indexMe = GameManager.Instance.gameCoordinator.countLeaderBoard;
        txtIndex.text = (indexMe + 1).ToString();
        txtOrdinal.text = GameUtil.GetOrdinalSuffix(indexMe+1);
        var list = listDataItemWinLeaderboardUI;
        var newItem = list[indexMe];
        int length = list.Count;
        listDataItemWinLeaderboardUI.Remove(newItem);
        newItem.index = (length).ToString();
        listDataItemWinLeaderboardUI.Add(newItem);
        leaderboard.gameObject.SetActive(true);
        leaderboard.InitSpawn(length,ref listItemWinLeaderboard, (itemLeaderboard, index) =>
        {
            var item = (ItemWinDashboardUI)itemLeaderboard;
            item.gameObject.SetActive(true);
            bool isMe = (length-1) == index;
            var data = list[index];
            item.InitData(isMe, data);
        });

        obSuccess.SetActive(false);        
    }
    public void OnMoveItem()
    {
        int indexCurrent = listItemWinLeaderboard.Count - 1;
        leaderboard.scrollRect.FocusOnRectTransform(listItemWinLeaderboard[indexCurrent].GetComponent<RectTransform>());
        leaderboard.MoveItem(indexCurrent, indexMe, null, (item) =>
        {
            ClickNext();
        },
        (item, index) =>
        {
            var itemLeaderboard = (ItemWinDashboardUI)item;
            var data = listDataItemWinLeaderboardUI[indexCurrent];
            data.index = (index + 1).ToString();
            itemLeaderboard.InitData(true, data);
            leaderboard.scrollRect.FocusOnRectTransform(listItemWinLeaderboard[indexCurrent].GetComponent<RectTransform>());
        });
    }
    private bool isWin;
    public void InitDataRes(bool isWin, DataValueWin db)
    {
        this.isWin = isWin;
        this.dataWin = db;
        if (isWin )
        {
            txt_Title.text = "Finish";
        }
        else
        {
            txt_Title.text = "Defeat";
        }
        baseSelect.Select(isWin);
        refText[0].InitData("Launch", $"{dataWin.valuePos}");
        refText[1].InitData("Crash", $"{dataWin.valueResult}");
        refText[2].InitData("Result", $"{dataWin.valueLevel}");
        refText[3].InitData("Position", $"{dataWin.valueJoin}");
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
        //DataManager.Instance.
    }
    private void ClickClaimReward(float x)
    {
        FirebaseNotificationLog.LogLevel(KeyFirebase.ClickClaimAdsWin, level);
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
            GameManager.Instance.gameCoordinator.DestroyAllBike();
            UIManager.Instance.HideAllUIIgnore();
            UIManager.Instance.ShowGarage();
        }, () =>
        {
            if (DataManager.Instance.CurrentLevel == 7)
            {
                var data = DataManager.Instance.dB_ResourceSO.resGiftLevel;
                DataManager.Instance.ReceiveRes(data);
                DataManager.Instance.GameData.idCurMotor = 3;
                var uiRes = UIManager.Instance.ShowUI<UIReceiveRes>(UIName.ReceiveRes);
                uiRes.Initialize(() =>
                {
                    var uiGarage = UIManager.Instance.GetUiActive<UIGarage>(UIName.Garage);
                    uiGarage.RefreshOnShow();
                    UIManager.Instance.RefreshUI();
                }, data);
            }
            else
            {
                var ui = UIManager.Instance.ShowUI<UIUpgrade>(UIName.Upgrade);
                ui.isOutUIWin = true;
                ui.level = level;
                if (DataManager.Instance.CurrentLevel == 2)
                {
                    if (!DataManager.Instance.GameData.isBeginnerBundle) UIManager.Instance.ShowUI(UIName.BeginnerBundle);
                }
                PushShowStepFirebaseShowUpgrade();
            }


            
        },1,1);
    }
    private void PushShowStepFirebaseShowUpgrade()
    {
        ParameterFirebaseCustom param = new ParameterFirebaseCustom(KeyTypeFirebase.Level, level.ToString());
        FirebaseNotificationLog.LogWithLevelMax(KeyFirebase.StepShowUpgrade, param);
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
