namespace Luzart
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    
    public class ProgressBarUISize : ProgressBarUI
    {
        [SerializeField]
        private RectTransform rtContain;
        private RectTransform rtFill
        {
            get
            {
                return imFill.rectTransform;
            }
        }
        private float width;
        private float height;
    
        private void Awake()
        {
            width = rtContain.sizeDelta.x;
            height = rtFill.sizeDelta.y;
        }
        public override void SetSlider(float prePercent, float targetPercent, float time ,Action onDone)
        {
            float preWidth = width*prePercent;
            float targetWidth = targetPercent*width;
            GameUtil.Instance.StartLerpValue(this, preWidth, targetWidth, time, (x) =>
            {
                rtFill.sizeDelta = new Vector2(x, height);
            }, onDone);
        }
    }
}
