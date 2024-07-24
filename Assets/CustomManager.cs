using BG_Library.Common;
using BG_Library.NET;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomManager : MonoBehaviour
{
    [BoxGroup("Remote config")] public CustomConfig RemoteConfigCustom;

    public static CustomManager Ins;
    private void Awake()
    {
        if (Ins == null)
            Ins = this;
        else
            Destroy(gameObject);


        DontDestroyOnLoad(gameObject);

        RemoteConfig.OnFetchComplete += Event_OnFetchComplete;

    }
    private void OnDestroy()
    {
        RemoteConfig.OnFetchComplete += Event_OnFetchComplete;
    }

    private void Event_OnFetchComplete()
    {
        RemoteConfigCustom = JsonTool.DeserializeObject<CustomConfig>(RemoteConfig.Ins.custom_config);
        Debug.Log("RemoteConfigCustom" + RemoteConfigCustom.levelShowAds);
    }
}
public class CustomConfig
{
    public int levelShowAds;
    public int levelShowRate;
}

