using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelectMode : UIBase
{
    public Button btnSinglePlay;
    public Button btnChallenge;
    public Button btnPlayOther;

    protected override void Setup()
    {
        base.Setup();
        GameUtil.ButtonOnClick(btnSinglePlay, ClickSinglePlay, true);
    }
    private void ClickSinglePlay()
    {
        UIManager.Instance.ShowUI(UIName.SelectLevel);
    }
}
