using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    public Image imFill;
    public virtual void SetSlider(float prePercent, float targetPercent, float time, Action onDone)
    {
        GameUtil.Instance.StartLerpValue(this, prePercent, targetPercent, time, (x) =>
        {
            imFill.fillAmount = x;
        }, onDone);
    }
}
