namespace Luzart
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    
    public class UISelectMode : UIBase
    {
        public Button btnBack;
    
        public Button btnSinglePlay;
        public Button btnChallenge;
        public Button btnPlayOther;
    
        protected override void Setup()
        {
            base.Setup();
            GameUtil.ButtonOnClick(btnBack, ClickBack, true, KeyAds.BtnSelectModeBack);
            GameUtil.ButtonOnClick(btnSinglePlay, ClickSinglePlay, true, KeyAds.BtnSelectModeSinglePlayer);
    
        }
        private void ClickBack()
        {
            Hide(); 
            UIManager.Instance.ShowGarage();
        }
        private void ClickSinglePlay()
        {
            UIManager.Instance.ShowUI(UIName.SelectLevel);
        }
    }
}
