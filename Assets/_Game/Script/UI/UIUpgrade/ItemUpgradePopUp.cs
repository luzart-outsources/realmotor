using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemUpgradePopUp : MonoBehaviour
{
    public TMP_Text txtTitle;
    public TMP_Text txtValueUpgrade;
    public LineUpgradeSlider lineUpgradeSlider;
    public Button btn;
    public Image imButton;
    public Action<ItemUpgradePopUp> ActionClick;
    public Sprite spClick, spUnclick;
    public StatsMotorbike stats;
    private string strTitle;
    private void Start()
    {
        GameUtil.ButtonOnClick(btn, Click, true);
    }
    public void Initialize(StatsMotorbike stats ,float value, int levelUpgrade ,Action<ItemUpgradePopUp> action )
    {
        this.ActionClick = action;
        this.stats = stats;
        this.txtValueUpgrade.text = value.ToString();
        this.lineUpgradeSlider.SetLevelUpgrade(levelUpgrade);
        SetText();
        SelectButton(false);
    }
    private void SetText()
    {
        switch (stats)
        {
            case StatsMotorbike.MaxSpeed:
                {
                    strTitle = "Top Speed";
                    break;
                }
            case StatsMotorbike.Acceleration:
                {
                    strTitle = "Acceleration";
                    break;
                }
            case StatsMotorbike.Handling:
                {
                    strTitle = "Handling";
                    break;
                }
            case StatsMotorbike.Brake:
                {
                    strTitle = "Brake";
                    break;
                }
        }
    }
    public void Click()
    {
        ActionClick?.Invoke(this);
    }
    private string colorBlack = "#000000";
    private string colorWhite = "#FFFFFF";
    public void SelectButton(bool isActive)
    {
        if(isActive)
        {
            imButton.sprite = spClick;
            txtTitle.text = $"<color={colorBlack}>{strTitle}</color>";
        }
        else
        {
            imButton.sprite= spUnclick;
            txtTitle.text = $"<color={colorWhite}>{strTitle}</color>";
        }
    }
}
