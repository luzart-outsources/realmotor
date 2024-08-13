using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequendCameraCinemachineTrackedDolly : MonoBehaviour
{
    public GroupPathCinemachineSmooth[] groupPathCinemachine;
    public CinemachineVirtualCamera virtualCamera;
    public SpriteRenderer spBlack;

    public Tweener FadeIn(float duration)
    {
        Color color = spBlack.color;
        return DOVirtual.Float(1, 0, duration, (x) =>
        {
            color.a = x;
            spBlack.color = color;
        });
    }
    public Tweener FadeOut(float duration)
    {
        Color color = spBlack.color;
        return DOVirtual.Float(0, 1, duration, (x) =>
        {
            color.a = x;
            spBlack.color = color;
        });
    }
    public void SetCinemachineCamera(int index)
    {
        var trackedDolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        if (trackedDolly != null)
        {
            trackedDolly.m_PathPosition = 0f;
            var group = groupPathCinemachine[index];
            trackedDolly.m_Path = group.cinemachineSmoothPath;

        }
    }
    public Tweener PlayCinemachineDolly(int index)
    {
        var trackedDolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        var group = groupPathCinemachine[index];
        return DOVirtual.Float(0, 1, group.timeMove, (x) =>
        {
            trackedDolly.m_PathPosition = x;
        });
    }
    public float PlayCinemachineDollyAndFade(int index, bool isTimeInterval = true)
    {
        var trackedDolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();

        var group = groupPathCinemachine[index];
        float timeInterval = isTimeInterval ? this.timeInterval : 0;
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() => SetCinemachineCamera(index));
        sequence.AppendInterval(timeInterval);
        float timeFadeIn = 0;
        if (!group.isStartFadeInBoth)
        {
            timeFadeIn = group.timeFadeIn;
            sequence.Append(FadeIn(timeFadeIn));
        }
        sequence.Append(PlayCinemachineDolly(index));
        if (group.isStartFadeInBoth)
        {
            sequence.Insert(timeInterval, FadeIn(group.timeFadeIn));
        }
        float timePlayAll = group.timeMove + timeFadeIn + timeInterval;
        float timeCanStartFadeOut = timePlayAll - group.timeFadeOut;
        float timeFadeOut = 0;
        if (group.isEndFadeOutBoth)
        {
            sequence.Insert(timeCanStartFadeOut, FadeOut(group.timeFadeOut));
        }
        else
        {
            timeFadeOut = group.timeFadeOut;
            sequence.Append(FadeOut(timeFadeOut));
        }
        float timePlay = timeFadeOut + timePlayAll;
        return timePlay;
    }
    public float timeInterval = 1f;
    private float TimeAnimCinemachineSmooth(int index, bool isTimeInterval = true)
    {
        float timeInterval = isTimeInterval ? this.timeInterval : 0;
        var group = groupPathCinemachine[index];
        float timeFadeIn = 0;
        float timeFadeOut = 0;
        if (!group.isStartFadeInBoth)
        {
            timeFadeIn = group.timeFadeIn;
        }
        if (!group.isEndFadeOutBoth)
        {
            timeFadeOut = group.timeFadeOut;
        }
        return group.timeMove + timeFadeIn + timeInterval + timeFadeOut;

    }
    public void StartAnimCamera()
    {
        int length = groupPathCinemachine.Length;
        float[] time = new float[length];
        for (int i = 0; i < length; i++)
        {
            int index = i;
            bool isTimeInterval = index != 0;
            time[index] = TimeAnimCinemachineSmooth(index, isTimeInterval);
        }
        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < length; i++)
        {
            int index = i;
            bool isTimeInterval = index != 0;
            sequence.AppendCallback(()=> PlayCinemachineDollyAndFade(index, isTimeInterval));
            sequence.AppendInterval(time[index]);
        }
        sequence.AppendCallback(() => OnCompletePathGame());
    }
    public Action ActionOnDoneCamera;
    public void OnCompletePathGame()
    {
        ActionOnDoneCamera?.Invoke();
    }
}
[System.Serializable]
public class GroupPathCinemachineSmooth
{
    public bool isStartFadeInBoth = false;
    public float timeFadeIn = 0.5f;
    public bool isEndFadeOutBoth = false;
    public float timeFadeOut = 0.2f;
    public float timeMove = 3f;
    public Ease ease = Ease.Unset;
    public CinemachineSmoothPath cinemachineSmoothPath;
    public Transform target;
}
