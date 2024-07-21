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
    public Image imIcon;
    public GameObject obBlack;
    public GroupBaseSelect groupBaseSelect;
    private Action<ItemSelectLevelUI> actionSelectUI;
    public DB_Level db_Level;
    private EStateSelectLevelUI eState;
    public void InitItem(DB_Level db_Level, Action<ItemSelectLevelUI> actionSelectUI)
    {
        this.actionSelectUI = actionSelectUI;
        this.db_Level = db_Level;
        imIcon.sprite = db_Level.spIcon;
        txtLevel.text = $"Level {db_Level.level}";
        if(db_Level.level == DataManager.Instance.CurrentLevel)
        {
            eState = EStateSelectLevelUI.Current;
        }
        else if(db_Level.level < DataManager.Instance.CurrentLevel)
        {
            eState = EStateSelectLevelUI.None;
        }
        else
        {
            eState = EStateSelectLevelUI.UnOpen;
        }
        SelectStatus();
    }
    private void Start()
    {
        GameUtil.ButtonOnClick(btn, Click, true);
    }
    private void Click()
    {
        actionSelectUI?.Invoke(this);
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
                    break;
                }
            case EStateSelectLevelUI.Current:
                {
                    SelectUnOpen(true);
                    SelectCurrentLine(true);
                    break;
                }
            case EStateSelectLevelUI.UnOpen:
                {
                    SelectUnOpen(false);
                    SelectCurrentLine(false);
                    break;
                }
        }
    }
    public void SelectCurrentLine(bool status)
    {
        groupBaseSelect.Select(status);
    }
    public void SelectUnOpen(bool status)
    {
        obBlack.SetActive(!status);
    }

}
[System.Serializable]
public enum EStateSelectLevelUI
{
    None = 0,
    Current = 1,
    UnOpen = 2,
}
