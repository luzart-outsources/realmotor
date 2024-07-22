using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIResume : UIBase
{
    public Button btnResume;
    public Button btnRestart;
    public Button btnGarage;
    protected override void Setup()
    {
        base.Setup();
        GameUtil.ButtonOnClick(btnResume, Resume, true);
        GameUtil.ButtonOnClick(btnRestart, Restart, true);
        GameUtil.ButtonOnClick(btnGarage, GoToGarage, true);
    }
    public override void Show(Action onHideDone)
    {
        base.Show(onHideDone);
        Time.timeScale = 0;
    }
    private void Resume()
    {
        Hide();
    }
    private void Restart()
    {
        GameManager.Instance.Restart();
    }
    private void GoToGarage()
    {
        GameManager.Instance.DisableCurrentMode();
        UIManager.Instance.HideAll();
        UIManager.Instance.ShowGarage();
    }
    public override void Hide()
    {
        Time.timeScale = 1;

        base.Hide();
    }
}
