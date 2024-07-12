using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettings : UIBase
{
    public SoundSettings soundSettings;
    public ButtonSelect[] buttonSelectsQuality;
    public ButtonSelect[] buttonSelectsControls;
    private ButtonSelect cacheQuality;
    private ButtonSelect cacheControls;
    public override void Show(Action onHideDone)
    {
        base.Show(onHideDone);
        soundSettings.Show();
        InitQuality();
        InitControls();
    }
    private void InitQuality()
    {
        int length = buttonSelectsQuality.Length;
        for (int i = 0; i < length; i++)
        {
            var btn = buttonSelectsQuality[i];
            if (btn != null)
            {
                int index = i;
                btn.InitAction( index, ClickQuality);
            }
        }
    }
    private void InitControls()
    {
        int length = buttonSelectsControls.Length;
        for (int i = 0; i < length; i++)
        {
            var btn = buttonSelectsControls[i];
            if (btn != null)
            {
                int index = i;
                btn.InitAction(index, ClickControls);
            }
        }
    }
    private void ClickQuality(ButtonSelect btn,int index)
    {
        if (cacheQuality != null)
        {
            cacheQuality.Select(false);
        }
        cacheQuality = btn;
        cacheQuality.Select(true);
    }
    private void ClickControls(ButtonSelect btn, int index)
    {
        if (cacheControls != null)
        {
            cacheControls.Select(false);
        }
        cacheControls = btn;
        cacheControls.Select(true);
    }
}
