namespace Luzart
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    
    public class UIResume : UIBase
    {
        public Button btnResume;
        public Button btnRestart;
        public Button btnGarage;
    
        private int level;
        protected override void Setup()
        {
            base.Setup();
            GameUtil.ButtonOnClick(btnResume, Resume, true, KeyAds.BtnResumeResume);
            GameUtil.ButtonOnClick(btnRestart, Restart, true, KeyAds.BtnResumeRestart);
            GameUtil.ButtonOnClick(btnGarage, GoToGarage, true, KeyAds.BtnResumeHome);
        }
        
        public override void Show(Action onHideDone)
        {
            base.Show(onHideDone);
            Time.timeScale = 0;
    
            level = GameManager.Instance.gameCoordinator.db_Level.level;
        }
        private void Resume()
        {
            Hide();
        }
        private void Restart()
        {
            GameManager.Instance.Restart();
            FirebaseNotificationLog.LogLevel(KeyFirebase.ClickRestartResume, level);
        }
        private void GoToGarage()
        {
            GameManager.Instance.DisableCurrentMode();
            UIManager.Instance.HideAll();
            UIManager.Instance.ShowGarage();
    
            FirebaseNotificationLog.LogLevel(KeyFirebase.ClickHomeResume, level);
        }
        public override void Hide()
        {
            Time.timeScale = 1;
    
            base.Hide();
        }
    }
}
