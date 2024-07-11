using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsWrapperManager : Singleton<AdsWrapperManager>
{
    public void ShowReward(string where, Action onDone, Action onFail)
    {
        onDone?.Invoke();
    }
    public void ShowInter(string where, Action onDone, Action onFail)
    {
        onDone?.Invoke();
    }
}
public static class KEYADS
{
    public const string ClickButtonWatchAds = "click_btn_watch_ads";
    public const string ClickButtonWatchAdsGetBike = "click_btn_watch_ads_get_bike";
    public const string ClickButtonWatchAdsGetHelmet = "click_btn_watch_ads_get_helmet";
    public const string ClickButtonWatchAdsGetBody = "click_btn_watch_ads_get_body";
}
