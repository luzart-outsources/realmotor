using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{
    public Button btn;
    public BaseSelect[] baseSelects; 

    public bool IsAnim = true;
    public int index;
    private Action<ButtonSelect> ActionSelect;
    private void Start()
    {
        GameUtil.ButtonOnClick(btn, ClickAction, IsAnim);
    }
    public virtual void InitAction(int index ,Action<ButtonSelect> action)
    {
        this.index = index;
        this.ActionSelect = action;
        Select(false);
    }
    private void ClickAction()
    {
        ActionSelect?.Invoke(this);
        Select(true);
    }
    public void Select(bool isSelect)
    {
        if (baseSelects != null)
        {
            int length = baseSelects.Length;
            for (int i = 0; i < baseSelects.Length; i++)
            {
                if (baseSelects[i] != null)
                {
                    baseSelects[i].Select(isSelect);
                }
            }
        }
    }
}
