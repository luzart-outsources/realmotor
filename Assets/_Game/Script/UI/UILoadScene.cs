using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UILoadScene : UIBase
{
    public ProgressBarUI progressBarUI;
    private Action onLoad, onDone;
    protected override void Setup()
    {
        base.Setup();
    }
    public void LoadSceneCloud(Action onLoad, Action onDone)
    {
        this.onLoad = onLoad;
        this.onDone = onDone;
        progressBarUI.SetSlider(0, 0.2f, 1, LoadFake);
    }
    private void LoadFake()
    {
        onLoad?.Invoke();
        progressBarUI.SetSlider(0.2f, 1, 1, LoadDone);
    }
    private void LoadDone()
    {
        onDone?.Invoke();
        Hide();
    }
}
