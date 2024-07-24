using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTitleSelectLevel : ButtonSelect
{
    public TMP_Text txtTitle;
    protected override void Start()
    {
        base.Start();
        GameUtil.ButtonOnClick(btn, ClickAction, IsAnim, KeyAds.BtnSelectLevelTitle);
    }
    public void SetTextTitle(string str)
    {
        txtTitle.text = str;
    }
}
