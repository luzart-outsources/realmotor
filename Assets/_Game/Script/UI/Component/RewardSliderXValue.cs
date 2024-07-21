using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class RewardSliderXValue : MonoBehaviour
{
    public Button btStopPointer;
    public Action<float> action;
    public RectTransform rtPointer;
    public RectTransform rtLine;
    public float timeMove = 1.5f;
    private Sequence twLoop = null;
    public TMP_Text[] tx = new TMP_Text[5];
    public float[] valueX = new float[5];
    public GameObject[] obShadow; 
    private float widthLine;
    private float posLineX;
    private float widthPointer;
    private float startValueX;
    private float endValueX;
    private float widthEach;
    public int curCoins;
    public float anchorPos;
    public GameObject obPreShadow = null;
    public TMP_Text coinRewardTxt;

    public void Start()
    {
        GameUtil.ButtonOnClick(btStopPointer, OnClickButton, false);
    }
    public void Initialize(int coinCur, int xValue, Action<float> actionClick)
    {
        this.action = actionClick;
        curCoins = coinCur;
        for (int i = 0; i < tx.Length; i++)
        {
            valueX[i] = valueX[i] * xValue;
            tx[i].text = $"x{valueX[i]}";
        }
        SetSlider();
    }
    private void SetSlider()
    {
        widthLine = rtLine.rect.width;
        widthEach = widthLine/valueX.Length;
        widthPointer = rtPointer.rect.width;
        posLineX = rtLine.anchoredPosition.x;
        anchorPos = posLineX - widthLine / 2;
        startValueX = posLineX - widthLine/2 ;
        endValueX = posLineX + widthLine / 2 ;
        LoopMove();
    }
    private void LoopMove()
    {
        twLoop = DOTween.Sequence();

        twLoop.Append(rtPointer.DOAnchorPosX(endValueX - 30f, timeMove, false).SetEase(Ease.InOutQuad));
        twLoop.Append(rtPointer.DOAnchorPosX(startValueX + 30f, timeMove, false).SetEase(Ease.InOutQuad));

        twLoop.SetEase(Ease.Linear);
        
        twLoop.OnUpdate(UpdateInMove);
        twLoop.SetLoops(-1);
    }

    private void OnDisable()
    {
        twLoop.Kill();
    }

    private bool isSet = false;
    private void UpdateInMove()
    {
        float pointerPos = rtPointer.anchoredPosition.x;
        if (pointerPos <= anchorPos + widthEach)
        {
            SetActiveObLighting(obShadow[0]);
            coinRewardTxt.text = $"{curCoins * valueX[0]}";
        }
        else if (anchorPos + widthEach < pointerPos && pointerPos <= anchorPos + widthEach * 2)
        {
            SetActiveObLighting(obShadow[1]);
            coinRewardTxt.text = $"{curCoins * valueX[1]}";
        }
        else if (anchorPos + widthEach * 2 < pointerPos && pointerPos <= anchorPos + widthEach * 3)
        {
            SetActiveObLighting(obShadow[2]);
            coinRewardTxt.text = $"{curCoins * valueX[2]}";
        }
        else if (anchorPos + widthEach * 3 < pointerPos && pointerPos <= anchorPos + widthEach * 4)
        {
            SetActiveObLighting(obShadow[3]);
            coinRewardTxt.text = $"{curCoins * valueX[3]}";
        }
        else
        {
            SetActiveObLighting(obShadow[4]);
            coinRewardTxt.text = $"{curCoins * valueX[4]}";
        }
    }
    private void SetActiveObLighting(GameObject go)
    {
        if (!go.activeInHierarchy)
        {
            return;
        }
        if (obPreShadow != null)
        {
            obPreShadow.SetActive(true);
        }
        obPreShadow = go;
        obPreShadow.SetActive(false);
    }
    private void OnClickButton()
    {
        twLoop?.Pause();
        float value = OnClickStopPointer();
        action?.Invoke(value);
    }
    public float OnClickStopPointer()
    {
        float pointerPos = rtPointer.anchoredPosition.x;
        if (pointerPos <= anchorPos + widthEach)
        {
            return valueX[0];
        }
        else if(anchorPos + widthEach < pointerPos && pointerPos <= anchorPos + widthEach * 2)
        {
            return valueX[1];
        }
        else if (anchorPos + widthEach * 2 < pointerPos && pointerPos <= anchorPos + widthEach * 3)
        {
            return valueX[2];
        }
        else if (anchorPos + widthEach * 3 < pointerPos && pointerPos <= anchorPos + widthEach * 4)
        {
            return valueX[3];
        }
        else 
        {
            return valueX[4];
        }
    }

    public int GetCoins()
    {
        return curCoins;
    }
    private int Caculate(float a)
    {
        return (int)(curCoins * a);
    }
}
