using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using AirFishLab.ScrollingList;
using AirFishLab.ScrollingList.ContentManagement;
using AirFishLab.ScrollingList.Demo;
using static AirFishLab.ScrollingList.ListBank;

public class ItemSelectMotorbikeUI : ListBox
{
    public Image imMotorBike;
    public Button btn;
    public GameObject obSelect;
    public GameObject obLock;
    public Action<ItemSelectMotorbikeUI> actionMotor;

    public DB_Motorbike db_Motorbike = null;
    public int currentIndex;
    private void Start()
    {
        //GameUtil.ButtonOnClick(btn, ClickMotor, true);
    }

    public void InitDB(DB_Motorbike dbMotorbike, Action<ItemSelectMotorbikeUI> onClick)
    {
        this.db_Motorbike = dbMotorbike;
        this.actionMotor = onClick;
        SetLock(!dbMotorbike.isHas);
    }

    public void SelectMotorBike(bool status)
    {
        if(status)
        {
            Selected();
        }
        else
        {
            UnSelected();
        }
    }
    public void SetLock(bool isStatus)
    {
        if(obLock != null)
        obLock.SetActive(isStatus);
    }
    private void Selected()
    {
        obSelect.SetActive(true);
    }
    private void UnSelected()
    {
        obSelect.SetActive(false);
    }
    public void ClickMotor()
    {
        actionMotor?.Invoke(this);
    }

    protected override void UpdateDisplayContent(IListContent listContent)
    {
        IntListContentMotorbike intList = (IntListContentMotorbike)listContent;
        InitDB(intList.db_Motorbike, intList.onClick);
        currentIndex = intList.value;
    }
}
