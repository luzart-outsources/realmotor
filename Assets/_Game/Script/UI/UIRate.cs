using BG_Library.IAR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.UI;

public class UIRate : UIBase
{
    public ButtonSelect[] btnSelects;
    private int indexStar = 0;
    public Button btnSubmit;

    protected override void Setup()
    {
        base.Setup();
        int length = btnSelects.Length;
        for (int i = 0; i < length; i++)
        {
            int index = i;
            btnSelects[i].InitAction(index, ClickButton);
        }
        GameUtil.ButtonOnClick(btnSubmit, ClickSubmit, true);
    }
    public override void Show(Action onHideDone)
    {
        base.Show(onHideDone);
        if (IsShowedRate)
        {
            Hide();
        }
    }
    private void ClickButton(ButtonSelect btn)
    {
        indexStar = btn.index;
        for (int i = 0;i < btnSelects.Length; i++)
        {
            if(i<= indexStar)
            {
                btnSelects[i].Select(true);
            }
            else
            {
                btnSelects[i].Select(false);
            }
        }
    }
    private void ClickSubmit()
    {
        if (indexStar>= 3)
        {
            IsShowedRate = true;
            InAppReviewManager.ShowReview();
        }
        Hide();
    }
    private string KEY_SHOW_RATE = "Show_Rate";
    private bool IsShowedRate
    {
        get
        {
            int boolean = PlayerPrefs.GetInt(KEY_SHOW_RATE, 0);
            return boolean != 0;
        }
        set
        {
            int boolean = value ? 1 : 0;
            PlayerPrefs.SetInt(KEY_SHOW_RATE, boolean);
            PlayerPrefs.Save();
        }
    }
}
