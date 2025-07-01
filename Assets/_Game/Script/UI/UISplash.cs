namespace Luzart
{
    using DG.Tweening;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    
    public class UISplash : UIBase
    {
        public Transform transload;
        public override void Show(Action onHideDone)
        {
            Sequence sq = DOTween.Sequence();
            sq.Append(transload.DORotate(new Vector3(0, 0, 720), 6, RotateMode.FastBeyond360));
            sq.AppendCallback(InitStartGame);
            //sq.Append(transload.DORotate(new Vector3(0, 0, 360), 1, RotateMode.FastBeyond360));
            //sq.AppendCallback(Hide);
    
        }
        private void InitStartGame()
        {
            UIManager.Instance.ShowGarage(UIName.Home);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                ShowDailyReward();
            });
        }
        private void ShowDailyReward()
        {
            int today = DataManager.Instance.dailyRewardManager.Today;
            if(today < DataManager.Instance.dailyRewardManager.dataDailyReward.isClaim.Length)
            {
                if (!DataManager.Instance.dailyRewardManager.IsClaimDay(today))
                {
                    UIManager.Instance.ShowUI(UIName.DailyReward);
                }
            }
            if (DataManager.Instance.dailyRewardManager.IsDontClaimAnyDay())
            {
                UIManager.Instance.ShowUI(UIName.DailyReward);
            }
        }
    }
}
