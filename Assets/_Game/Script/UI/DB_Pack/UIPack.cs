//using BG_Library.IAP;
using BG_Library.NET;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public abstract class BaseUIPack: UIBase
{
    [SerializeField]
    protected DB_Pack db_Pack;
    public Button btnBuy;
    protected override void Setup()
    {
        isAnimBtnClose = true;
        base.Setup();
        GameUtil.ButtonOnClick(btnBuy, BuyIAP, true);

    }
    public override void Show(Action onHideDone)
    {
        base.Show(onHideDone);

        RefreshUI();
    }
    public override void RefreshUI()
    {
        base.RefreshUI();
        GetDBIAP();
        if (IsHasBuyPack())
        {
            Hide();
            return;
        }
        InitIAP();
    }
    public virtual bool IsHasBuyPack()
    {
        return DataManager.Instance.IsHasBuyPack(db_Pack.productId) && db_Pack.maxBuy <= DataManager.Instance.GetPackPurchaseCount(db_Pack.productId);
    }
    public virtual void GetDBIAP()
    {
        string where = db_Pack.where;
        db_Pack = DataManager.Instance.dbPackSO.GetDBPack(db_Pack.productId);
        db_Pack.where = where;
    }
    public virtual void InitIAP()
    {

    }
    public virtual void BuyIAP()
    {
        //IAPManager.PurchaseResultListener += OnPurchaseComplete;
        SetBuyProduct();
    }
    private void SetBuyProduct()
    {
        //IAPManager.PurchaseProduct(db_Pack.where, db_Pack.productId);
    }
    //private void OnPurchaseComplete(IAPPurchaseResult iappurchaseresult)
    //{
    //    IAPManager.PurchaseResultListener -= OnPurchaseComplete;
    //    switch (iappurchaseresult.Result)
    //    {
    //        case IAPPurchaseResult.EResult.Complete:
    //            OnCompleteBuy();
    //            DataManager.Instance.GameData.isUserIAP = true;
    //            // iappurchaseresult.Product.Reward - Reward setup in stats
    //            // iappurchaseresult.Product.Reward.PackRewardValue - give reward amount
    //            // iappurchaseresult.Product.Reward.Reward - Type Reward > REMOVE_AD, CURRENCY (CASH OR GOLD), CUSTOM (Item Or Tool)
    //            // iappurchaseresult.Product.Reward.atlas - Reward give Currency Id or Item, Tool Id (example: CASH, GOLD, TOOL_1...)
    //            // todo give product reward
    //            break;
    //        case IAPPurchaseResult.EResult.WrongInstance:
    //            // Purchase faield: IAP Manager instance null (Read Setup IAP)  
    //            break;
    //        case IAPPurchaseResult.EResult.WrongProduct:
    //            // Purchase faield: can't find product with id 
    //            break;
    //        case IAPPurchaseResult.EResult.WrongStoreController:
    //            // Purchase faield: IAP initialized faield
    //            break;
    //    }
    //}
    protected virtual void OnCompleteBuy()
    {
        DataManager.Instance.SaveBuyPack(db_Pack.productId);
        UIManager.Instance.RefreshUI();
    }
}
public class UIPack : BaseUIPack
{

    public ListResUI listResUI;
    public ResUI[] resOther;
    public override void RefreshUI()
    {
        base.RefreshUI();
    }
    public override void InitIAP()
    {
        base.InitIAP();
        if (db_Pack.gift.groupDataResources != null && db_Pack.gift.groupDataResources.Length > 0)
        {
            listResUI.InitResUI(db_Pack.gift.groupDataResources);
        }

        if (db_Pack.resourcesOther.groupDataResources != null && db_Pack.resourcesOther.groupDataResources.Length > 0 &&
            resOther != null && resOther.Length > 0)
        {
            int length = db_Pack.resourcesOther.groupDataResources.Length;
            int lengthRes = resOther.Length;
            int min = Mathf.Min(length, lengthRes);
            for (int i = 0; i < min; i++)
            {
                resOther[i].InitData(db_Pack.resourcesOther.groupDataResources[i]);
            }
        }
    }
    protected override void OnCompleteBuy()
    {
        base.OnCompleteBuy();
        List<DataResource> listReward = new List<DataResource>();
        if (db_Pack.gift.groupDataResources != null)
        {
            listReward.AddRange(db_Pack.gift.groupDataResources);
        }
        if (db_Pack.resourcesOther.groupDataResources != null)
        {
            listReward.AddRange(db_Pack.resourcesOther.groupDataResources);
        }
        DataManager.Instance.ReceiveRes( dataResource: listReward.ToArray());

        if (db_Pack.isRemoveAds)
        {
            AdsManager.Ins.PurchaseRemoveAds();
            DataManager.Instance.SaveBuyPack("seat.right.match.puzzle.removeads.100");
        }
    }
    private void OnCompleteBuyIAP()
    {
        UIManager.Instance.RefreshUI();
    }
    public override void Hide()
    {
        base.Hide();
    }

}

public class PackItem: MonoBehaviour
{
    [SerializeField]
    protected DB_Pack db_Pack;
    public Button btnBuy;
    protected virtual void Awake()
    {
        //GameUtil.ButtonOnClick(btnBuy, BuyIAP, true);
    }
    public virtual void Initialize()
    {
        GetDBIAP();
        if (IsHasBuyPack())
        {
            Hide();
            return;
        }
        InitIAP();
    }
    public virtual bool IsHasBuyPack()
    {
        return DataManager.Instance.IsHasBuyPack(db_Pack.productId) && db_Pack.maxBuy <= DataManager.Instance.GetPackPurchaseCount(db_Pack.productId);
    }
    public virtual void GetDBIAP()
    {
        db_Pack = DataManager.Instance.dbPackSO.GetDBPack(db_Pack.productId);
    }
    public virtual void InitIAP()
    {

    }
    //public virtual void BuyIAP()
    //{
    //    IAPManager.PurchaseResultListener += OnPurchaseComplete;
    //    SetBuyProduct();
    //}
    //private void SetBuyProduct()
    //{
    //    IAPManager.PurchaseProduct(db_Pack.where, db_Pack.productId);
    //}
    //private void OnPurchaseComplete(IAPPurchaseResult iappurchaseresult)
    //{
    //    IAPManager.PurchaseResultListener -= OnPurchaseComplete;
    //    switch (iappurchaseresult.Result)
    //    {
    //        case IAPPurchaseResult.EResult.Complete:
    //            OnCompleteBuy();
    //            DataManager.Instance.GameData.isUserIAP = true;
    //            // iappurchaseresult.Product.Reward - Reward setup in stats
    //            // iappurchaseresult.Product.Reward.PackRewardValue - give reward amount
    //            // iappurchaseresult.Product.Reward.Reward - Type Reward > REMOVE_AD, CURRENCY (CASH OR GOLD), CUSTOM (Item Or Tool)
    //            // iappurchaseresult.Product.Reward.atlas - Reward give Currency Id or Item, Tool Id (example: CASH, GOLD, TOOL_1...)
    //            // todo give product reward
    //            break;
    //        case IAPPurchaseResult.EResult.WrongInstance:
    //            // Purchase faield: IAP Manager instance null (Read Setup IAP)  
    //            break;
    //        case IAPPurchaseResult.EResult.WrongProduct:
    //            // Purchase faield: can't find product with id 
    //            break;
    //        case IAPPurchaseResult.EResult.WrongStoreController:
    //            // Purchase faield: IAP initialized faield
    //            break;
    //    }
    //}
    protected virtual void OnCompleteBuy()
    {

        UIManager.Instance.RefreshUI();
    }
    protected virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}

[System.Serializable]
public class DB_Pack
{
    public string productId;
    public int maxBuy = 1;
    public bool isRemoveAds = false;
    public GroupDataResources gift;
    public GroupDataResources resourcesOther;

    [ValueDropdown(nameof(GetPackWhereValues))]
    public string where;

    // Phương thức để tự động lấy tất cả các giá trị string từ PackWhere
    private static IEnumerable<string> GetPackWhereValues()
    {
        foreach (FieldInfo field in typeof(PackWhere).GetFields(BindingFlags.Static | BindingFlags.Public))
        {
            if (field.FieldType == typeof(string))
            {
                yield return (string)field.GetValue(null);
            }
        }
    }
}
public static class PackWhere
{
    public static string BattlePass = "BattlePass";
    public static string Ticket = "Ticket";
    public static string Shop = "Shop";
    public static string Home = "Home";
}
