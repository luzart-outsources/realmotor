using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class UIRacer : UIBase
{
    public GarageManager garageManager;
    public ButtonSelect btnRacer;
    public ButtonSelect btnClothes;
    public Button btnBack;
    public ItemScrollViewRacer itemPf;
    public Transform parent;
    private List<ItemScrollViewRacer> listItemScrollViewRacer = new List<ItemScrollViewRacer>();
    protected override void Setup()
    {
        base.Setup();
        btnRacer.InitAction(ClickRacer);
        btnClothes.InitAction(ClickClothes);
        GameUtil.ButtonOnClick(btnBack,Hide);
    }
    public override void Hide()
    {
        base.Hide();
        UIManager.Instance.ShowGarage();
    }

    private bool isOnRacer = true;
    public override void Show(Action onHideDone)
    {
        base.Show(onHideDone);
    }
    public void RefreshUIOnShow() 
    {
        ClickRacer();
        garageManager.SpawnMotorVisual(DataManager.Instance.GameData.idCurMotor);
    }
    public void ClickRacer()
    {
        isOnRacer = true;
        btnRacer.Select(true);
        btnClothes.Select(false);
        InitScrollItemRacer();
    }
    public void ClickClothes()
    {
        isOnRacer = false;
        btnRacer.Select(false);
        btnClothes.Select(true);
        InitScrollItemClothes();
    }
    private void InitScrollItemRacer()
    {
        var data = DataManager.Instance.resourceBuySO.dbResBuyHelmet;
        int idHelmet = DataManager.Instance.GameData.curCharacter.idHelmet;
        ItemScrollViewRacer itemCurrent = null;
        MasterHelper.InitListObj<ItemScrollViewRacer>(data.Length, itemPf, listItemScrollViewRacer, parent, (item, index) =>
        {
            item.gameObject.SetActive(true);
            int id = data[index].dataRes.type.id;
            bool isBuy = DataManager.Instance.IsHasHelmet(id);
            item.Init(data[index], isBuy, ClickItem);
            if (id == idHelmet)
            {
                itemCurrent = item;
            }
        });
        UnAndSelectItem(itemCurrent);
    }
    private void InitScrollItemClothes()
    {
        var data = DataManager.Instance.resourceBuySO.dbResBuyBody;
        int idBody = DataManager.Instance.GameData.curCharacter.idClothes;
        ItemScrollViewRacer itemCurrent = null;
        MasterHelper.InitListObj<ItemScrollViewRacer>(data.Length, itemPf, listItemScrollViewRacer, parent, (item, index) =>
        {
            item.gameObject.SetActive(true);
            int id = data[index].dataRes.type.id;
            bool isBuy = DataManager.Instance.IsHasBody(id);
            item.Init(data[index], isBuy, ClickItem);
            if (id == idBody)
            {
                itemCurrent = item;
            }
        });
        UnAndSelectItem(itemCurrent);
    }
    private ItemScrollViewRacer itemCache;
    public void ClickItem(ItemScrollViewRacer item)
    {
        RefreshUI();
        UnAndSelectItem(item);
    }
    private void UnAndSelectItem(ItemScrollViewRacer item)
    {
        if (itemCache != null)
        {
            itemCache.SelectedItem(false);
        }
        item.SelectedItem(true);
        if(item.dbResBuy.dataRes.type.type == RES_type.Helmet)
        {
            garageManager.ChangeIdHelmet(item.dbResBuy.dataRes.type.id);
        }
        else
        {
            garageManager.ChangeIdBody(item.dbResBuy.dataRes.type.id);
        }

    }
    private void RefreshUIBody()
    {
        var data = DataManager.Instance.resourceBuySO.dbResBuyBody;
        for (int i = 0; i < listItemScrollViewRacer.Count; i++)
        {
            var index = i;
            var item = listItemScrollViewRacer[index];

            int id = data[index].dataRes.type.id;
            bool isBuy = DataManager.Instance.IsHasBody(id);
            item.Init(data[index], isBuy, ClickItem);
        }
    }
    private void RefreshUIHelmet()
    {
        var data = DataManager.Instance.resourceBuySO.dbResBuyHelmet;
        for (int i = 0; i < listItemScrollViewRacer.Count; i++)
        {
            var index = i;
            var item = listItemScrollViewRacer[index];

            int id = data[index].dataRes.type.id;
            bool isBuy = DataManager.Instance.IsHasHelmet(id);
            item.Init(data[index], isBuy, ClickItem);
        }
    }
    public override void RefreshUI()
    {
        base.RefreshUI();
        if (isOnRacer)
        {
            RefreshUIHelmet();
        }
        else
        {
            RefreshUIBody();
        }
    }

}
public enum EStateClaim
{
    None = 0,
    CanEquip = 1,
    Buy = 2,
}
