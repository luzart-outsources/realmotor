using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTitleSelectLevel : ButtonSelect
{
    public TMP_Text txtTitle;
    public void SetTextTitle(string str)
    {
        txtTitle.text = str;
    }
}
