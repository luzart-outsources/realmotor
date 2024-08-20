using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceCameraCinemachineTrackedDolly : MonoBehaviour
{
    public Camera cameraMain;
    public bool isOneVirtual = false;
    public GroupPathCinemachineSmoothCamera[] groupPathCinemachine;
    public SpriteRenderer spBlack;

    public Tweener FadeIn(float duration)
    {
        Color color = spBlack.color;
        return DOVirtual.Float(1, 0, duration, (x) =>
        {
            color.a = x;
            spBlack.color = color;
        }).SetId(this);
    }
    public Tweener FadeOut(float duration)
    {
        Color color = spBlack.color;
        return DOVirtual.Float(0, 1, duration, (x) =>
        {
            color.a = x;
            spBlack.color = color;
        }).SetId(this);
    }
    private GroupPathCinemachineSmoothCamera preGroupPath = null;
    public void SetCinemachineCamera(int index)
    {
        var group = groupPathCinemachine[index];


        if (preGroupPath != null)
        {
            preGroupPath.virtualCamera.gameObject.SetActive(false);
        }
        preGroupPath = group;
        group.virtualCamera.gameObject.SetActive(true);
        group.virtualCamera.m_LookAt = group.lookAt;
        group.virtualCamera.m_Follow = group.follow;
        var trackedDolly = group.virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        if (trackedDolly != null)
        {
            trackedDolly.m_CameraUp = group.cameraUpMode;
            trackedDolly.m_PathPosition = 0f;
            trackedDolly.m_Path = group.cinemachineSmoothPath;
        }

    }
    public Tweener PlayCinemachineDolly(int index)
    {
        var group = groupPathCinemachine[index];
        var trackedDolly = group.virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        if(trackedDolly == null)
        {
            if(group.followOffset != new Vector3(-1, -1, -1))
            {
                var transposer = group.virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
                transposer.m_FollowOffset = group.followOffset;
            }
            return DOVirtual.Float(group.firstFOV, group.targetFOV, group.timeMove, (x) =>
            {
                group.virtualCamera.m_Lens.FieldOfView = x;
            }).SetEase(group.ease).SetId(this);
        }
        return DOVirtual.Float(0, 1, group.timeMove, (x) =>
        {
            trackedDolly.m_PathPosition = x;
        }).SetEase(group.ease).SetId(this);
    }
    public float PlayCinemachineDollyAndFade(int index)
    {
        var group = groupPathCinemachine[index];
        var trackedDolly = group.virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        float timeInterval = group.timeInterval;
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() => SetCinemachineCamera(index));
        sequence.AppendInterval(group.timeInterval);
        float timeFadeIn = 0;
        if (group.isFadeIn)
        {
            if (!group.isStartFadeInBoth)
            {
                timeFadeIn = group.timeFadeIn;
                sequence.Append(FadeIn(timeFadeIn));
            }
        }

        sequence.Append(PlayCinemachineDolly(index));

        if (group.isFadeIn)
        {
            if (group.isStartFadeInBoth)
            {
                sequence.Insert(timeInterval, FadeIn(group.timeFadeIn));
            }
        }

        float timePlayAll = group.timeMove + timeFadeIn + timeInterval;
        float timeCanStartFadeOut = timePlayAll - group.timeFadeOut;
        float timeFadeOut = 0;
        if (group.isFadeOut)
        {
            if (group.isEndFadeOutBoth)
            {
                sequence.Insert(timeCanStartFadeOut, FadeOut(group.timeFadeOut));
            }
            else
            {
                timeFadeOut = group.timeFadeOut;
                sequence.Append(FadeOut(timeFadeOut));
            }
        }

        float timePlay = timeFadeOut + timePlayAll;
        return timePlay;
    }
    private float TimeAnimCinemachineSmooth(int index)
    {
        var group = groupPathCinemachine[index];
        float timeFadeIn = 0;
        float timeFadeOut = 0;
        float timeInterval = group.timeInterval;
        if(group.isFadeIn)
        {
            if (!group.isStartFadeInBoth)
            {
                timeFadeIn = group.timeFadeIn;
            }
        }
        if(group.isFadeOut)
        {
            if (!group.isEndFadeOutBoth)
            {
                timeFadeOut = group.timeFadeOut;
            }
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
            time[index] = TimeAnimCinemachineSmooth(index);
        }
        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < length; i++)
        {
            int index = i;
            bool isTimeInterval = index != 0;
            sequence.AppendCallback(() => PlayCinemachineDollyAndFade(index));
            sequence.AppendInterval(time[index] + 0.3f);
        }
        sequence.AppendCallback(() => OnCompletePathGame());
        sequence.SetId(this);
    }
    public Action ActionOnDoneCamera;
    public void OnCompletePathGame()
    {
        ActionOnDoneCamera?.Invoke();
    }
    public void SetFollow(Transform target)
    {
        int length = groupPathCinemachine.Length;
        for (int i = 0; i < length; i++)
        {
            var group = groupPathCinemachine[i];
            group.follow = target;
        }
    }
    public void SetLookAt(Transform target)
    {
        int length = groupPathCinemachine.Length;
        for (int i = 0; i < length; i++)
        {
            var group = groupPathCinemachine[i];
            group.lookAt = target;
        }
    }
    private void OnDisable()
    {
        this.DOKill();
    }
}
[System.Serializable]
public class GroupPathCinemachineSmoothCamera
{
    public bool isFadeIn = true;
    public bool isStartFadeInBoth = false;
    public float timeFadeIn = 0.5f;

    public bool isFadeOut = true;
    public bool isEndFadeOutBoth = false;
    public float timeFadeOut = 0.2f;

    public float timeInterval = 0;

    public float timeMove = 3f;
    public Ease ease = Ease.Unset;

    public bool isChangeFOV = false;
    public float firstFOV = 60f;
    public float targetFOV = 30f;

    public Vector3 followOffset = new Vector3(-1,-1,-1);

    public Vector3 firstPosition;
    public Vector3 targetPosition;
    
    public CinemachineTrackedDolly.CameraUpMode cameraUpMode;
    public Transform follow;
    public Transform lookAt;
    public CinemachineSmoothPath cinemachineSmoothPath;
    public CinemachineVirtualCamera virtualCamera;
}
