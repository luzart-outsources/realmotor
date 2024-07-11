using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinObserver : MonoBehaviour
{
    public TMP_Text txtGold;
    public Button btnAddCoin;
    private Action actionAddCoin;

    private void Awake()
    {
        if(btnAddCoin != null)
        {
            GameUtil.ButtonOnClick(btnAddCoin, ClickAddCoin, true);
        }

    }
    private void OnEnable()
    {
        Observer.Instance.AddObserver(ObserverKey.CoinObserverNormal, SetText);
        Observer.Instance.AddObserver(ObserverKey.CoinObserverTextRun, SetTextTimeRun);
        SetText(null);
    }
    private void OnDisable()
    {
        Observer.Instance.RemoveObserver(ObserverKey.CoinObserverNormal, SetText);
        Observer.Instance.RemoveObserver(ObserverKey.CoinObserverTextRun, SetTextTimeRun);
    }
    public void InitAddCoin(Action onAddCoin)
    {
        actionAddCoin = onAddCoin;
    }
    private void ClickAddCoin()
    {
        //actionAddCoin?.Invoke();
        UIManager.Instance.ShowUI(UIName.AddCoin);
    }
    private void SetText(object data)
    {
        txtGold.text = $"{DataManager.Instance.Gold}";
    }
    private void SetTextTimeRun(object data)
    {
        if (data == null)
        {
            return;
        }
        SetTextTimeRun dataRun = (SetTextTimeRun)data;
        GameUtil.Instance.StartLerpValue(this, dataRun.preGold, dataRun.targetGold, dataRun.timeRun, (x) =>
        {
            txtGold.text = $"{x}";
        });
    }
}
public struct SetTextTimeRun
{
    public int preGold;
    public int targetGold;
    public int timeRun;
}
