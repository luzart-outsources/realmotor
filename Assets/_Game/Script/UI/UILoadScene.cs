using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoadScene : UIBase
{
    protected override void Setup()
    {
        base.Setup();
    }
    public void LoadSceneCloud(Action onLoad)
    {
        onLoad?.Invoke();
    }
}
