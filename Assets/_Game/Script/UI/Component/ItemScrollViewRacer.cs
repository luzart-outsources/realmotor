using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemScrollViewRacer : MonoBehaviour
{
    public Button btn;
    public Button btnBuy;
    private Action<ItemScrollViewRacer> action;
    public TMP_Text txtValue;
    public TMP_Text txtName;
    public Image imIcon;
    public GameObject obSelect;
    public GameObject obEquip;
    public GameObject obBuy;
    private bool isBuy;
    public DB_ResourcesBuy dbResBuy;


    private void Awake()
    {
        GameUtil.ButtonOnClick(btn, Click);
        GameUtil.ButtonOnClick(btnBuy, ClickBuy, true);
    }
    private void Click()
    {
        action?.Invoke(this);
    }
    private void ClickBuy()
    {
        DataManager.Instance.AddRes(dbResBuy.dataRes, ClaimRes);
        Click();
    }
    private void ClaimRes()
    {
        DataManager.Instance.ReceiveRes(dbResBuy.dataRes);
        UIManager.Instance.RefreshUI();
    }
    public void Init(DB_ResourcesBuy db,bool isBuy, Action<ItemScrollViewRacer> actionClick)
    {
        dbResBuy = db;
        this.isBuy = isBuy;
        this.action = actionClick;
        imIcon.sprite = db.dataRes.spIcon;
        txtName.text = db.titleBuy;
        SetEState();
    }
    private void DisableAll()
    {
        SelectedItem(false);
        obBuy.SetActive(false);
        obEquip.SetActive(false);
    }
    public void SelectedItem(bool isSelect)
    {
        obSelect.SetActive(isSelect);
        if (isBuy)
        {
            var type = dbResBuy.dataRes.type;
            int id = type.id;
            if(type.type == RES_type.Helmet)
            {
                DataManager.Instance.GameData.curCharacter.idHelmet = id;
            }
            else
            {
                DataManager.Instance.GameData.curCharacter.idClothes = id;
            }
        }
    }
    private void SetEState()
    {
        DisableAll();
        if(isBuy)
        {
            StateCanEquip();
        }
        else
        {
            StateDontBuy();
        }
    }
    private void SetStatusIfDontBuy()
    {
        obBuy.SetActive(true);
        switch (dbResBuy.typeBuy)
        {
            case TypeBuy.None:
                {
                    break;
                }
            case TypeBuy.Gold:
                {
                    StatusBuyGold();
                    break;
                }
            case TypeBuy.IAP:
                {
                    StatusBuyIAP();
                    break;
                }
            case TypeBuy.Ads:
                {
                    StatusBuyAds();
                    break;
                }
            case TypeBuy.Other:
                {
                    StatusOther();
                    break;
                }
        }
    }

    private void StatusOther()
    {

    }

    private void StatusBuyAds()
    {

    }

    private void StatusBuyIAP()
    {

    }

    private void StatusBuyGold()
    {

    }

    private void StateNone()
    {

    }
    private void StateCanEquip()
    {
        obEquip.SetActive(true);
    }
    private void StateEquipped()
    {

    }
    private void StateDontBuy()
    {
        SetStatusIfDontBuy();
        txtValue.text = dbResBuy.valueBuy.ToString();
    }

}
