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
        if (type == ShopItemType.RemoveAds && AdsManager.IAP_RemoveAds)
        {
            gameObject.SetActive(false);
        }
        if (type == ShopItemType.BeginnerBundle && DataManager.Instance.isBeginnerBundle)
        {
            gameObject.SetActive(false);
        }
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
        gameObject.SetActive(false);
    }
    public void BuyBeginnerBundle()
    {
        IAPManager.PurchaseProduct("BEGINNER_BUNDLE", _iapProductStats.Id);
        if (type == ShopItemType.BeginnerBundle)
        {
            gameObject.SetActive(false);
        }

    }
    public void BuyCoin(string numCoin)
    {
        IAPManager.PurchaseProduct("BUY_COIN_" + numCoin, _iapProductStats.Id);
    }
    private void OnShowDone()
    {
        Observer.Instance.Notify(ObserverKey.CoinObserverDontAuto, true);
        DataManager.Instance.ReceiveRes(new DataResource(new DataTypeResource(RES_type.Gold), 5000));
        UIManager.Instance.ShowCoinSpawn();

    }
    private void OnShowFailed()
    {
        UIManager.Instance.ShowToast(KeyToast.NoInternetLoadAds);
    }
}

