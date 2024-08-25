using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIReceiveRes : UIBase
{
    public Button btn;
    public ListResUI listResUI;
    private Action onDone;

    protected override void Setup()
    {
        base.Setup();
        GameUtil.ButtonOnClick(btn, ClickButton, true);
    }
    private void ClickButton()
    {
        onDone?.Invoke();
        Hide();
    }
    public void Initialize( Action onDone = null, params DataResource[] data)
    {
        this.onDone = onDone;
        listResUI.InitResUI(data);
    }

}
