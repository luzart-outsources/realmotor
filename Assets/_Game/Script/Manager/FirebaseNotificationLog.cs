namespace Luzart
{
    using BG_Library.NET;
    using Firebase.Analytics;
    using Microsoft.SqlServer.Server;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using UnityEngine;
    using static Cinemachine.DocumentationSortingAttribute;
    
    public static class FirebaseNotificationLog
    {
        public static void LogEvent(string nameEvent)
        {
            FirebaseEvent.LogEvent(nameEvent);
            
        }
        public static void LogStartLevel(int level)
        {
            
        }
        public static void LogWithLevelMax(string key, ParameterFirebaseCustom[] paramCustomOthers)
        {
            int levelMax = DataManager.Instance.CurrentLevel;
    
    
            int length = paramCustomOthers.Length;
            Parameter[] paramOther = new Parameter[length];
            for (int i = 0; i < length; i++)
            {
                var data = paramCustomOthers[i];
                paramOther[i] = new Parameter(data.Type, data.param);
            }
    
            Parameter[] param = new Parameter[length + 1];
            Parameter paramLevelMax = new Parameter(KeyTypeFirebase.LevelMax, levelMax.ToString());
            for (int i = 0;i < length; i++)
            {
                param[i] = paramOther[i];
            }
            param[length] = paramLevelMax;
    
    
            FirebaseEvent.LogEvent(key, param);
    
    #if DEBUG_DA
            List<ParameterFirebaseCustom> listParam = new List<ParameterFirebaseCustom>();
            listParam.AddRange(paramCustomOthers);
            listParam.Add(new ParameterFirebaseCustom(KeyTypeFirebase.LevelMax, levelMax.ToString()));
            LogLocal(key, listParam.ToArray());
    #endif
        }
        public static void LogWithLevelMax(string key, ParameterFirebaseCustom paramCustomOther)
        {
            int levelMax = DataManager.Instance.CurrentLevel;
            Parameter paramOther = new Parameter(paramCustomOther.Type, paramCustomOther.param);
            Parameter paramLevelMax = new Parameter(KeyTypeFirebase.LevelMax, levelMax.ToString());
            Parameter[] param = new Parameter[2];
            param[0] = paramOther;
            param[1] = paramLevelMax;
            FirebaseEvent.LogEvent(key, param);
            LogLocal(key, new ParameterFirebaseCustom[2] { paramCustomOther, new ParameterFirebaseCustom(KeyTypeFirebase.LevelMax, levelMax.ToString()) });
        }
        public static void LogLevel(string key ,int level)
        {
            ParameterFirebaseCustom paramLevel = new ParameterFirebaseCustom(KeyTypeFirebase.Level,level.ToString());
            LogWithLevelMax(key, paramLevel);
    
        }
        private static void LogLocal(string key, params ParameterFirebaseCustom[] param)
        {
    #if DEBUG_DA
            StringBuilder stringBuilder = new StringBuilder();
            int length = param.Length;
            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(" ( ");
                stringBuilder.Append(param[i].Type);
                stringBuilder.Append(" : ");
                stringBuilder.Append(param[i].param);
                stringBuilder.Append(" ) -");
            }
            LogLocal($"Log Firebase ( KEY: {key} ) on param ( {stringBuilder} )");
    #endif
        }
        private static void LogLocal(string key)
        {
            GameUtil.Log(key);
        }
    }
    public struct ParameterFirebaseCustom
    {
        public ParameterFirebaseCustom(string type, string param)
        {
            this.Type = type;
            this.param = param;
        }
        public string Type;
        public string param;
    }
    public static class KeyFirebase
    {
        public static string StartLevel = "start_level";
        public static string EndLevel = "end_level";
        public static string LevelFail = "level_fail";
        public static string ClickGoldFree = "click_goldfree";
        public static string ClickClaimAdsWin = "claim_ads_win";
        public static string ReceiveResource = "receive_resource";
        public static string ClickBtnAddCoin = "click_btn_addcoin";
    
        public static string ClickRestartResume = "click_restart_resume";
        public static string ClickHomeResume = "click_home_resume";
    
    
        public static string StepShowLeaderboard = "step_show_leaderboard";
        public static string StepShowInforGold = "step_show_inforgold";
        public static string StepShowUpgrade = "step_show_upgrade";
        public static string StepClickRace = "step_click_race";
        public static string StepBackUpgrade = "step_back_upgrade";
        public static string StepClickUpgradeUpgrade = "step_click_upgrade_upgrade";
        public static string StepClickBackGarage = "step_click_back_garage";
        public static string StepClickRacerGarage = "step_click_racer_garage";
        public static string StepClickRacingGarage = "step_click_racing_garage";
        public static string StepClickBackUILevel = "step_click_back_ui_level";
        public static string StepClickUILevelInGame = "step_click_ui_level_in";
    
    
        public static string StepUpgradeMotor(int indexStats)
        {
            return $"step_upgrade_motor_{indexStats}";
        }
        public static string UpgradeMotor(int indexStats)
        {
            return $"upgrade_motor_{indexStats}";
        }
    }
    public static class KeyTypeFirebase
    {
        public static string Level = "level";
        public static string LevelMax = "level_max";
        public static string Where = "where";
        public static string Res = "res";
        public static string Amount = "amount";
    }
}
