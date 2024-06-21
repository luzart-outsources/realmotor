using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : UIBase
{
    public Button btnPlay;
    protected override void Setup()
    {
        base.Setup();
        GameUtil.ButtonOnClick(btnPlay, ClickPlay, true);
    }
    private void ClickPlay()
    {
        GameManager.Instance.PlayGameMode(EGameMode.Classic, 0);
    }
}
