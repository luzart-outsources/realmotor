using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AirFishLab.ScrollingList;
using AirFishLab.ScrollingList.ContentManagement;
using System;

public class ScrollViewGarageUI : ListBank
{
    public UIGarage uiGarage;
    public List<DB_Motorbike> listMotorbike = new List<DB_Motorbike>();
    private readonly IntListContentMotorbike _contentWrapper = new IntListContentMotorbike();
    public CircularScrollingList circular;
    public void InitListDB(List<DB_Motorbike> list)
    {
        listMotorbike = list;
        circular.Initialize();
        circular.ListSetting.AddOnFocusingBoxChangedCallback(uiGarage.ClickItem);
        circular.ListSetting.AddOnBoxSelectedCallback(uiGarage.ClickItem);
    }
    public override IListContent GetListContent(int index)
    {
        _contentWrapper.value = index;
        _contentWrapper.db_Motorbike = listMotorbike[index];
        //_contentWrapper.onClick = uiGarage.ClickItem;
        return _contentWrapper;
    }

    public override int GetContentCount()
    {
        return listMotorbike.Count;
    }
}
public class IntListContentMotorbike : IListContent
{
    public int value;
    public DB_Motorbike db_Motorbike;
    public Action<ItemSelectMotorbikeUI> onClick;
}
