namespace Luzart
{
    using DG.Tweening;
    using System;
    using System.Drawing.Drawing2D;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    
    public class ItemUpgradePopUp : MonoBehaviour
    {
        private RectTransform _rt;
        public RectTransform rtMe
        {
            get
            {
                if(_rt == null)
                {
                    _rt = GetComponent<RectTransform>();
    
                }
                return _rt;
            }
        }
        public TMP_Text txtTitle;
        public TMP_Text txtValueUpgrade;
        public LineUpgradeSlider lineUpgradeSlider;
        public Button btn;
        //public Image imButton;
        //public Image imIcon;
        public Action<ItemUpgradePopUp> ActionClick;
        //public Sprite spClick, spUnclick;
        //public Sprite spClickIcon, spUnclickIcon;
        public GroupBaseSelect baseSelect; 
        public StatsMotorbike stats;
        private string strTitle;
        private Vector2 size;
        public GameObject obUpdateDetail;
        private int levelUpgrade;
        public float valueStatsUpgrade;
        public float valueStatCurrent;
        private void Start()
        {
            GameUtil.ButtonOnClick(btn, Click, true);
            size = rtMe.sizeDelta;
    
        }
        public void Initialize(StatsMotorbike stats , float valueCurrent ,float valueUpgrade, int levelUpgrade ,Action<ItemUpgradePopUp> action )
        {
            this.ActionClick = action;
            this.stats = stats;
            this.valueStatsUpgrade = valueUpgrade;
            this.valueStatCurrent = valueCurrent;
            this.txtValueUpgrade.text = valueCurrent.ToString();
            this.levelUpgrade = levelUpgrade;
            this.lineUpgradeSlider.SetLevelUpgrade(levelUpgrade - 1);
            SetText();
            SelectButton(false);
    
        }
        private void SetText()
        {
            switch (stats)
            {
                case StatsMotorbike.MaxSpeed:
                    {
                        strTitle = "Top Speed";
                        break;
                    }
                case StatsMotorbike.Acceleration:
                    {
                        strTitle = "Acceleration";
                        break;
                    }
                case StatsMotorbike.Handling:
                    {
                        strTitle = "Handling";
                        break;
                    }
                case StatsMotorbike.Brake:
                    {
                        strTitle = "Brake";
                        break;
                    }
            }
        }
        public void Click()
        {
            ActionClick?.Invoke(this);
            //Canvas.ForceUpdateCanvases();
        }
        private string colorBlack = "#000000";
        private string colorWhite = "#FFFFFF";
        private Tween twScale = null;
        public void SelectButton(bool isActive)
        {
            Vector3 scale = Vector3.one;
            baseSelect.Select(isActive);
            txtTitle.text = strTitle;
            if(isActive)
            {
                txtValueUpgrade.text = valueStatsUpgrade.ToString();
                scale = Vector3.one * 1.03f;
            }
            else
            {
                txtValueUpgrade.text = valueStatCurrent.ToString();
                scale = Vector3.one;
            }
            twScale?.Kill(true);
            twScale = transform.DOScale(scale, 0.3f);
            obUpdateDetail.SetActive(isActive);
            if (levelUpgrade > 5)
            {
                obUpdateDetail.SetActive(false);
            }
    
        }
        private void SelectBaseSelect(bool isSelect)
        {
            
        }
    }
}
