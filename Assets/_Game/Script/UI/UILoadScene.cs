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
    public void LoadSceneCloud(Action onLoad, Action onDone)
    {
        this.onLoad = onLoad;
        this.onDone = onDone;
        sequence?.Kill();
        sequence = DOTween.Sequence();
        sequence.Append(transload.DORotate(new Vector3(0, 0, 720), 2, RotateMode.FastBeyond360).SetEase(Ease.Linear));
        sequence.AppendCallback(() => onLoad?.Invoke());
        sequence.Append(transload.DORotate(new Vector3(0, 0, 360), 1, RotateMode.FastBeyond360).SetEase(Ease.Linear));
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
