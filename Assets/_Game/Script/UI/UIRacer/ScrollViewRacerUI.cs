namespace Luzart
{
    //using AirFishLab.ScrollingList;
    //using AirFishLab.ScrollingList.ContentManagement;
    //using System;
    //using System.Collections;
    //using System.Collections.Generic;
    //using UnityEngine;
    
    //public class ScrollViewRacerUI : ListBank
    //{
    //    public UIRacer uiRacer;
    //    public List<DB_ResourcesBuy> listResourceBuy = new List<DB_ResourcesBuy>();
    //    private readonly IntListContentRaceUI _contentWrapper = new IntListContentRaceUI();
    //    public CircularScrollingList circular;
    //    public void InitListDB(List<DB_ResourcesBuy> list)
    //    {
    //        listResourceBuy = list;
    //        circular.Initialize();
    //        circular.ListSetting.AddOnFocusingBoxChangedCallback(uiRacer.ClickItem);
    //        circular.ListSetting.AddOnBoxSelectedCallback(uiRacer.ClickItem);
    //    }
    //    public override IListContent GetListContent(int index)
    //    {
    //        _contentWrapper.value = index;
    //        _contentWrapper.resBuy = listResourceBuy[index];
    //        return _contentWrapper;
    //    }
    
    //    public override int GetContentCount()
    //    {
    //        return listResourceBuy.Count;
    //    }
    //}
    //public class IntListContentRaceUI : IListContent
    //{
    //    public int value;
    //    public DB_ResourcesBuy resBuy;
    //}
}
