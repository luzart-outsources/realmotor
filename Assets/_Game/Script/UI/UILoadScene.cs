using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UILoadScene : UIBase
{
    public Transform transload;
    private Action onLoad, onDone;
    protected override void Setup()
    {
        base.Setup();
    }
    Sequence sequence;
    public void LoadSceneCloud(Action onLoad, Action onDone, float timeLoad = 2, float timeHide =1f)
    {
        this.onLoad = onLoad;
        this.onDone = onDone;
        sequence?.Kill();
        sequence = DOTween.Sequence();
        sequence.Append(transload.DORotate(new Vector3(0, 0, 720), timeLoad, RotateMode.FastBeyond360).SetEase(Ease.Linear));
        sequence.AppendCallback(() => onLoad?.Invoke());
        sequence.Append(transload.DORotate(new Vector3(0, 0, 360), timeHide, RotateMode.FastBeyond360).SetEase(Ease.Linear));
        sequence.AppendCallback(() => onDone?.Invoke());
        sequence.AppendCallback(Hide);
    }
    private void LoadFake()
    {
    }
    private void LoadDone()
    {

        Hide();
    }
}
