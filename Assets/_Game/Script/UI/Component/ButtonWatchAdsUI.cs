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
    public TypeWhereButtonAds typeWhere;
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
        AdsWrapperManager.Instance.ShowReward(KeyAds.ClickButtonWatchAds, OnShowDone, OnShowFailed);
        FirebaseNotificationLog.LogWithLevelMax(KeyFirebase.ClickGoldFree, new ParameterFirebaseCustom(KeyTypeFirebase.Where, typeWhere.ToString()));
    }
    private void OnShowDone()
    {
        Observer.Instance.Notify(ObserverKey.CoinObserverDontAuto, true);
        DataManager.Instance.ReceiveRes(_dataRes);
        UIManager.Instance.ShowCoinSpawn();

    }
    private void OnShowFailed()
    {
        UIManager.Instance.ShowToast(KeyToast.NoInternetLoadAds);
    }
    public enum TypeWhereButtonAds
    {
        None = 0,   
        home = 1,
        garage = 2,
        upgrade =3,
        racer = 4,
        popup_addcoin = 5
    }
}
