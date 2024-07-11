using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{
    public Button btn;
    public Image[] imSelect;
    public Sprite[] spSelect;
    public Sprite[] spUnSelect;
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
        int length = imSelect.Length;
        for (int i = 0; i < length; i++)
        {
            if (isSelect)
            {
                imSelect[i].sprite = spSelect[i];
            }
            else
            {
                imSelect[i].sprite = spUnSelect[i];
            }

        }
       
    }
}
