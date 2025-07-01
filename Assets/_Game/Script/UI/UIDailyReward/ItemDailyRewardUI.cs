namespace Luzart
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.SqlTypes;
    using TMPro;
    using Unity.VisualScripting;
    using UnityEngine;
    using UnityEngine.UI;
    
    public class ItemDailyRewardUI : MonoBehaviour
    {
        public Button btn;
    
        public TMP_Text txtDay;
        public TMP_Text txtGold;
        public GroupDataResources groupDataResources;
        private Action<ItemDailyRewardUI> actionClick;
        public EStateClaimReward eStateClaim;
        public int index;
    
        public GroupBaseSelect GroupBaseSelect;
        public GameObject obReceived;
        public GameObject obAds;
        private void Start()
        {
            GameUtil.ButtonOnClick(btn, ClickButton);
        }
        public void Initialize(int index,EStateClaimReward eState,GroupDataResources groupDataResources, Action<ItemDailyRewardUI> onClick)
        {
            this.index = index;
            this.actionClick = onClick;
            this.groupDataResources = groupDataResources;
            this.eStateClaim = eState;
            UpdateVisual();
            OnSetTextGold();
        }
    
        private void OnSetTextGold()
        {
            int length = groupDataResources.groupDataResources.Length;
            for (int i = 0; i < length; i++)
            {
                var item = groupDataResources.groupDataResources[i];
                if(item.type.type == RES_type.Gold)
                {
                    txtGold.text = $"+ {item.amount} Cash";
                    break;
                }
            }
            txtDay.text = $"Day {index + 1}";
        }
    
        private void UpdateVisual()
        {
            DisableAllState();
            switch(eStateClaim)
            {
                case EStateClaimReward.Received:
                    {
                        Received();
                        break;
                    }
                case EStateClaimReward.DontReceived:
                    {
                        DontReceive();
                        break;
                    }
                case EStateClaimReward.CanReceive:
                    {
                        CanReceive();
                        break;
                    }
                case EStateClaimReward.WillReceive:
                    {
                        WillReceive();
                        break;
                    }
            }
        }
        private void DisableAllState()
        {
            obReceived.SetActive(false);
            obAds.SetActive(false);
        }
        private void Received()
        {
            GroupBaseSelect.Select(false);
            obReceived.SetActive(true);
        }
        private void DontReceive()
        {
            obAds.SetActive(true);
        }
        private void CanReceive()
        {
            GroupBaseSelect.Select(true); 
        }
        private void WillReceive()
        {
            GroupBaseSelect.Select(false);
        }
        private void ClickButton()
        {
            actionClick?.Invoke(this);
        }
    }
    public enum EStateClaimReward
    {
        Received = 0,
        DontReceived = 1,
        CanReceive = 2,
        WillReceive = 3,
    }
}
