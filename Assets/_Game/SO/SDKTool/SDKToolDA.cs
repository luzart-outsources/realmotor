namespace Luzart
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using BG_Library.NET;
    using static BG_Library.NET.AdsManager;
    
    [CreateAssetMenu(fileName = "SDKToolDA",menuName ="SDKtoolDA")]
    public class SDKToolDA : ScriptableObject
    {
        public StructAdsConfig.InterstitialInfor[] interstitial;
        public InterstitialDA inter;
    
        public string stringInter;
    
        [Sirenix.OdinInspector.Button]
        public void OnChangeStringInter()
        {
            inter.interstitial = interstitial;
            stringInter = JsonUtility.ToJson(inter);
        }
        [System.Serializable]
        public class InterstitialDA
        {
            public StructAdsConfig.InterstitialInfor[] interstitial;
        }
    }
}
