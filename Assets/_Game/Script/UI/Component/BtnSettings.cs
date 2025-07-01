namespace Luzart
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    
    public class BtnSettings : MonoBehaviour
    {
        public Button _btn;
        private Button btn
        {
            get
            {
                if(_btn == null)
                {
                    _btn = GetComponent<Button>();
                }
                return _btn;
            }
        }
        void Start()
        {
            GameUtil.ButtonOnClick(btn, ClickSettings,true);
        }
        private void ClickSettings()
        {
            UIManager.Instance.ShowUI(UIName.Settings);
        }
    
    }
}
