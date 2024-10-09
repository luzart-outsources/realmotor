using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelectLevelUI : MonoBehaviour
{
    public Button btn;
    public TMP_Text txtLevel;
    public TMP_Text txtName;
    public TMP_Text txtCoin;
    public Image imIcon;
    public Image imLock;
    public Image imComplete;
    public Image imSelect;
    public GameObject obBlack;
    public GameObject obGift;
    public GroupBaseSelect groupBaseSelect;
    private Action<ItemSelectLevelUI> actionSelectUI;
    public DB_Level db_Level;
    private EStateSelectLevelUI eState;
    public CanvasGroup canvasGroup;
    public void InitItem(DB_Level db_Level, Action<ItemSelectLevelUI> actionSelectUI)
    {
        this.actionSelectUI = actionSelectUI;
        this.db_Level = db_Level;
        DataManager data = DataManager.Instance;
        int goldLv = data.dB_ResourceSO.GoldInLevel[db_Level.level];

        imIcon.sprite = db_Level.spIcon;
        if(txtCoin) txtCoin.text = $"+{goldLv}";
        txtLevel.text = $"Level {db_Level.level+1}";
        txtName.text = $"{db_Level.name}";
        var level = db_Level.level + 1;
        if(obGift && level == 7) obGift.SetActive(true);

        if(db_Level.level == data.CurrentLevel)
        {
            eState = EStateSelectLevelUI.Current;
        }
        else if(db_Level.level < data.CurrentLevel)
        {
            eState = EStateSelectLevelUI.Complete;
        }
        else
        {
            eState = EStateSelectLevelUI.UnOpen;
        }
        SelectStatus();
    }
    private void Start()
    {
        GameUtil.ButtonOnClick(btn, Click, true, KeyAds.BtnSelectLevelLevel);
    }
    private void Click()
    {
        actionSelectUI?.Invoke(this);
    }
    private Tweener twShow = null;
    public void OnHideCanvasG()
    {
        canvasGroup.alpha = 0;
    }
    public float timeShow = 0.5f;
    public float firstShow = 0.5f;
    public void OnShowCanvasG()
    {
        canvasGroup.alpha = 0;
        twShow?.Kill(true);
        twShow = DOVirtual.Float(firstShow, 1, timeShow, (x) =>
        {
            canvasGroup.alpha = x;
        }).SetEase(Ease.OutQuad).SetId(this);
    }
    public void SelectStatus()
    {
        SelectCurrentLine(false);
        SelectUnOpen(false);
        switch (eState)
        {
            case EStateSelectLevelUI.None:
                {
                    SelectUnOpen(true);
                    SelectCurrentLine(false);
                    SelectComplete(false);
                    break;
                }
            case EStateSelectLevelUI.Current:
                {
                    SelectUnOpen(true);
                    SelectCurrentLine(true);
                    SelectComplete(false);
                    break;
                }
            case EStateSelectLevelUI.UnOpen:
                {
                    SelectUnOpen(false);
                    SelectCurrentLine(false);
                    break;
                }
            case EStateSelectLevelUI.Complete:
                {
                    SelectUnOpen(true);
                    SelectCurrentLine(false);
                    SelectComplete(true);
                    break;
                }
        }
    }
    public void SelectCurrentLine(bool status)
    {
        groupBaseSelect.Select(status);
        imSelect.DOKill();
        imSelect.DOFade(0f, 1f).SetLoops(-1, LoopType.Yoyo);
    }
    public void SelectUnOpen(bool status)
    {
        obBlack.SetActive(!status);
        imLock.gameObject.SetActive(!status);
        imComplete.gameObject.SetActive(status);
        btn.interactable= status;
    }
    public void SelectComplete(bool status)
    {
        obBlack.SetActive(status);
        imComplete.gameObject.SetActive(status);
    }

    private void OnDisable()
    {
        this.DOKill(true);
    }

}
[System.Serializable]
public enum EStateSelectLevelUI
{
    None = 0,
    Current = 1,
    UnOpen = 2,
    Complete = 3
}
