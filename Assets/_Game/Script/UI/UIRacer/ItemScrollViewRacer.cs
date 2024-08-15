using AirFishLab.ScrollingList;
using AirFishLab.ScrollingList.ContentManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemScrollViewRacer : MonoBehaviour
{
    private RectTransform _rectTransform;
    public RectTransform rectTransform
    {
        get
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }
            return _rectTransform;
        }
    }
    public Image imMotorBike;
    public TMP_Text txtName;
    public Button btn;
    public GameObject obSelect;
    public BaseSelect baseSelectBg;
    public BaseSelect obLock;
    public Action<ItemScrollViewRacer> actionMotor;
    public int currentIndex;
    public DB_ResourcesBuy resourcesBuy;
    private void Start()
    {
        GameUtil.ButtonOnClick(btn, ClickMotor, true);
    }

    public void InitDB(DB_ResourcesBuy dbResource, Action<ItemScrollViewRacer> onClick)
    {
        this.actionMotor = onClick;
        resourcesBuy = dbResource;
        SetUnLock(dbResource.isHas);
        //txtName.text = dbResource.nameMotor;
        imMotorBike.sprite = resourcesBuy.dataRes.spIcon;
        SelectMotorBike(false);
    }
    private void ClickMotor()
    {
        actionMotor?.Invoke(this);
    }
    public void SelectMotorBike(bool status)
    {
        obSelect.SetActive(status);
        if ((!status && !resourcesBuy.isHas) || status)
        {
            if (status && resourcesBuy.isHas)
            {
                baseSelectBg.Select(true);
            }
            else
            {
                baseSelectBg.Select(false);
            }

            obLock.gameObject.SetActive(true);
        }
        else
        {
            obLock.gameObject.SetActive(false);
            baseSelectBg.Select(false);
        }

    }
    public void SetUnLock(bool isStatus)
    {
        if (obLock != null)
            obLock.Select(isStatus);
    }
}
