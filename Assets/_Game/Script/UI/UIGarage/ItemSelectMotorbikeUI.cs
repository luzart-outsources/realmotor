using System;
using UnityEngine;
using UnityEngine.UI;
using AirFishLab.ScrollingList;
using AirFishLab.ScrollingList.ContentManagement;

public class ItemSelectMotorbikeUI : ListBox
{
    public Image imMotorBike;
    public Button btn;
    public GameObject obSelect;
    public BaseSelect baseSelectBg;
    public BaseSelect obLock;
    //public Action<ItemSelectMotorbikeUI> actionMotor;

    public DB_Motorbike db_Motorbike = null;
    public int currentIndex;
    private DB_ResourcesBuy resourcesBuy;
    private void Start()
    {
        //GameUtil.ButtonOnClick(btn, ClickMotor, true);
    }

    public void InitDB(DB_Motorbike dbMotorbike, Action<ItemSelectMotorbikeUI> onClick)
    {
        this.db_Motorbike = dbMotorbike;
        //this.actionMotor = onClick;
        SetLock(!dbMotorbike.isHas);
        resourcesBuy = DataManager.Instance.resourceBuySO.GetResourcesBuy(new DataTypeResource(RES_type.Bike,dbMotorbike.idMotor), PlaceBuy.Garage);
        imMotorBike.sprite = resourcesBuy.dataRes.spIcon;
    }
    public void SelectMotorBike(bool status)
    {
        obSelect.SetActive(status);
        baseSelectBg.Select(status);
        obLock.Select(status);
    }
    public void SetLock(bool isStatus)
    {
        if(obLock != null)
            obLock.gameObject.SetActive(isStatus);
    }

    protected override void UpdateDisplayContent(IListContent listContent)
    {
        IntListContentMotorbike intList = (IntListContentMotorbike)listContent;
        InitDB(intList.db_Motorbike, intList.onClick);
        currentIndex = intList.value;
    }
}
