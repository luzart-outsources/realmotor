using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{
    public Button btn;
    public Image imSelect;
    public Sprite spSelect;
    public Sprite spUnSelect;
    public bool IsAnim = true;
    private Action ActionSelect;
    private void Start()
    {
        GameUtil.ButtonOnClick(btn, ClickAction, IsAnim);
    }
    public void InitAction(Action action)
    {
        this.ActionSelect = action;
    }
    private void ClickAction()
    {
        ActionSelect?.Invoke();
        Select(true);
    }
    public void Select(bool isSelect)
    {
        if (isSelect)
        {
            imSelect.sprite = spSelect;
        }
        else
        {
            imSelect.sprite= spUnSelect;
        }
    }
}
