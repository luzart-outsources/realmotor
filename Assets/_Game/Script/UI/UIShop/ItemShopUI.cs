using BG_Library.IAP;
using BG_Library.NET;
using IngameDebugConsole;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemShopUI : MonoBehaviour
{
    Button btnBuy;
    Button btnWatchAds;
    Button btnRemoveAds;
    [SerializeField] ShopItemType type;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private IAPProductStats _iapProductStats;

    private void OnEnable()
    {
        CheckItem();
    }

    private void Start()
    {
        IAPManager.CheckInitializedAndHandle(OnInitializedComplete);
    }
    private void OnInitializedComplete()
    {
        if (_iapProductStats == null) return;
        string priceString = IAPManager.GetPriceString(_iapProductStats.Id);
        _priceText.text = priceString;
        Debug.Log(priceString);
    }

    public void CheckItem()
    {
        if (type == ShopItemType.RemoveAds && AdsManager.IAP_RemoveAds)
        {
            gameObject.SetActive(false);
        }
        if (type == ShopItemType.BeginnerBundle && DataManager.Instance.GameData.isBeginnerBundle)
        {
            gameObject.SetActive(false);
        }
    }

    public void SelectBuy()
    {
        
    }

    public void SelectWatchAds()
    {
        AdsWrapperManager.Instance.ShowReward(KeyAds.ClickButtonWatchAds, OnShowDone, OnShowFailed);
    }

    public void SelectRemoveAds()
    {
        IAPManager.PurchaseProduct("REMOVE_AD", _iapProductStats.Id);
    }
    public void BuyBeginnerBundle()
    {
        IAPManager.PurchaseProduct("BEGINNER_BUNDLE", _iapProductStats.Id);
    }
    public void BuyCoin(string numCoin)
    {
        IAPManager.PurchaseProduct("BUY_COIN_" + numCoin, _iapProductStats.Id);
    }
    private void OnShowDone()
    {
        Observer.Instance.Notify(ObserverKey.CoinObserverDontAuto, true);
        DataManager.Instance.ReceiveRes(new DataResource(new DataTypeResource(RES_type.Gold), 500));
        UIManager.Instance.ShowCoinSpawn();

    }
    private void OnShowFailed()
    {
        UIManager.Instance.ShowToast(KeyToast.NoInternetLoadAds);
    }
}

