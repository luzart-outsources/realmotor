using BG_Library.NET;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsWrapperManager : Singleton<AdsWrapperManager>
{
    public void ShowReward(string where, Action onDone, Action onFail)
    {
        GameUtil.Log(where);
#if UNITY_EDITOR
        if (UnityEngine.Random.Range(0,100) >= 30)
        {
            onDone?.Invoke();
            return;
        }
        if (TestManager.Instance != null)
        {
            onDone?.Invoke();
            return;
        }
#endif
        if (!AdsManager.IsRewardedReady())
        {
            onFail?.Invoke(); 
            return;
        }
        AdsManager.ShowRewardVideo(where, onDone);
    }
    public void ShowInter(string where, Action onDone)
    {
        if (TestManager.Instance != null)
        {
            onDone?.Invoke();
            return;
        }
        if (DataManager.Instance.CurrentLevel >= CustomManager.Ins.RemoteConfigCustom.levelShowAds)
        {
            GameUtil.Log(where);
            AdsManager.ShowInterstitial(where, onDone);
        }
        else
        {
            onDone?.Invoke();
        }

    }
}
public static class KeyAds
{
    public const string ClickButtonWatchAds = "click_btn_watch_ads";
    public const string ClickButtonWatchAdsGetBike = "click_btn_watch_ads_get_bike";
    public const string ClickButtonWatchAdsGetHelmet = "click_btn_watch_ads_get_helmet";
    public const string ClickButtonWatchAdsGetBody = "click_btn_watch_ads_get_body";
    public const string ClickButtonXRewardOnWin = "click_btn_x_reward_on_win";
    public const string ClickItemDailyReward = "click_btn_item_daily_reward";
    public const string ClickItemX2DailyReward = "click_btn_item_x2_daily_reward";

    // Button Inter
    public const string BtnGarageRacing = "btn_garage_racing";
    public const string BtnHomeRacing = "btn_home_racing";
    public const string BtnGarageRacer = "btn_garage_racer";
    public const string BtnGarageUpgarde = "btn_garage_upgrade";
    public const string BtnGarageSettings = "btn_garage_settings";
    public const string BtnGarageBuyBikeGold = "btn_garage_buy_bike_gold";
    public const string BtnSelectModeSinglePlayer = "btn_selectmode_singleplayer";
    public const string BtnSelectModeBack = "btn_select_mode_back";
    public const string BtnSelectLevelTitle = "btn_select_level_title";
    public const string BtnSelectLevelLevel = "btn_select_level_level";
    public const string BtnSelectLevelBack = "btn_select_level_back";
    public const string BtnRacerHelmet = "btn_racer_helmet";
    public const string BtnRacerClothes = "btn_racer_clothes";
    public const string BtnRacerEquip = "btn_racer_equip";
    public const string BtnRacerBack = "btn_racer_back";
    public const string BtnRacerBuyHelmet = "btn_racer_buy_helmet";
    public const string BtnRacerBuyClothes = "btn_racer_buy_clothes";
    public const string BtnUpgradeBack = "btn_upgrade_back";
    public const string BtnUpgradeUpgrade = "btn_upgrade_upgrade";
    public const string BtnSettingsBack = "btn_settings_back";
    public const string BtnWinClassicNext = "btn_winclassic_next";
    public const string BtnWinClassicMissOut = "btn_winclassic_missout";
    public const string BtnGameplayResume = "btn_gameplay_resume";
    public const string BtnResumeResume = "btn_resume_resume";
    public const string BtnResumeRestart = "btn_resume_restart";
    public const string BtnResumeHome = "btn_resume_home";

    // End Button

    //Inter
    public const string OnCollisionWall = "on_collision_wall";
    public const string OnEndRace = "on_end_race";
}
public enum PositionAds
{
    continue_win_1 = 0,
    continue_win_2 = 1,
    QuitHome = 1,


}