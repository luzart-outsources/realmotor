using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWatchAdsUI : MonoBehaviour
{
    public Button btn;
    public DataResource _dataRes;
    public ResUI resUI;
    public bool isAnim;
    public void Awake()
    {
        GameUtil.ButtonOnClick(btn, ClickButton, isAnim);
        resUI.InitData(_dataRes);
    }
    public void InitData(DataResource dataResouse)
    {
        this._dataRes = dataResouse;
    }
    public void ClickButton()
    {
        AdsWrapperManager.Instance.ShowReward(KEYADS.ClickButtonWatchAds, OnShowDone, OnShowFailed);
    }
    private void OnShowDone()
    {
        DataManager.Instance.ReceiveRes(_dataRes);
    }
    private void OnShowFailed()
    {
        UIManager.Instance.ShowToast(KEYTOAST.NoInternetLoadAds);
    }
}
