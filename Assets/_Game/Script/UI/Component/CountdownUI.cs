using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class CountdownUI : MonoBehaviour
{
    public int count;
    public int targetCount;
    [ReadOnly]
    private int currentCount;    
    public Action onDone;
    public TMP_Text txt;
    public bool IsInitOnStart = false;
    private void Start()
    {
        if (IsInitOnStart)
        {
            StartCountDown();
        }
    }
    public void InitCountDown(int firstCount, int targetCount, Action onDone)
    {
        this.count = firstCount; 
        this.targetCount = targetCount;
        currentCount = firstCount;
        this.onDone = onDone;
        txt.text = currentCount.ToString();
        StartCountDown();

    }
    private Coroutine corIECountdown = null;
    private void StartCountDown()
    {
        if (corIECountdown != null)
        {
            StopCoroutine(corIECountdown);
        }
        corIECountdown = StartCoroutine(IECountdown());
    }
    private IEnumerator IECountdown()
    {
        WaitForSeconds wait = new WaitForSeconds(1);
        while(currentCount > targetCount)
        {
            yield return wait;
            currentCount--;
            txt.text = currentCount.ToString();
            if(currentCount == targetCount)
            {
                onDone?.Invoke();
            }
        }
    }
    private void OnDisable()
    {
        if (corIECountdown != null)
        {
            StopCoroutine(corIECountdown);
        }
    }
}
