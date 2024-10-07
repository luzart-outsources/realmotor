using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class ItemChangeTitle : ButtonSelect
{
    protected override void Start()
    {
        base.Start();
        GameUtil.ButtonOnClick(btn, ClickAction, IsAnim);
    }
}
