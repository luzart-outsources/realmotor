using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using UnityEngine.Device;

public class UIManager : Singleton<UIManager>
{
    private const string PATH_UI = "UI/";
    //public UITop topUI;
    public Transform[] rootOb;
    public UIBase[] listSceneCache;

    public Canvas canvas;

    private List<UIBase> listScreenActive = new List<UIBase>();
    private Dictionary<UIName, UIBase> cacheScreen = new Dictionary<UIName, UIBase>();
    /// <summary>
    /// 0: UiController, shop, event, main, guild, free (on tab)
    /// 1: General (down top)
    /// 2: top, changeScene,login, ....
    /// 3: loading, NotiUpdate
    /// top: -1: not action || -2: Hide top || >=0: Show top
    /// </summary>
    private Dictionary<UIName, string> dir = new Dictionary<UIName, string>
    {
        // UIName, rootIdx,topIdx,loadPath
            {UIName.MainMenu,"0,0,UIMainMenu" },
            {UIName.Gameplay, "0,0,UIGameplay"},
            {UIName.Settings,"1,0,UISettings" },
            {UIName.WinClassic,"1,0,UIWinClassic" },
            {UIName.LoseClassic,"1,0,UILoseClassic" },
            {UIName.Splash,"1,0,UISplash" },
            {UIName.LoadScene,"2,0,UILoadScene" },
            {UIName.Garage,"0,0,UIGarage" },
            {UIName.Toast,"3,0,UIToast" },
            {UIName.AddCoin,"2,0,UIAddCoin" },
            {UIName.Resume,"2,0,UIResume" },
            {UIName.Racer,"0,0,UIRacer" },
            {UIName.Upgrade,"0,0,UIUpgrade" },
            {UIName.SelectMode,"0,0,UISelectMode" },
            {UIName.SelectLevel,"0,0,UISelect" },
    };

    private Dictionary<UIName, DataUIBase> dic2;

    public UIName CurrentName { get; private set; }
    public bool IsAction { get; set; }
    private void Awake()
    {
        dic2 = new Dictionary<UIName, DataUIBase>();
        foreach (var i in dir)
        {
            if (!dic2.ContainsKey(i.Key))
            {
                var t = i.Value.Split(',');
                dic2.Add(i.Key, new DataUIBase(int.Parse(t[0]), int.Parse(t[1]), t[2]));
            }
        }
        for (int i = 0; i < listSceneCache.Length; i++)
        {
            if (!cacheScreen.ContainsKey(listSceneCache[i].uiName))
            {
                cacheScreen.Add(listSceneCache[i].uiName, listSceneCache[i]);
            }
        }
        //if (SdkUtil.isiPad())
        //{
        //    GetComponent<CanvasScaler>().matchWidthOrHeight = 1f;
        //}
        //else
        //{
        //    GetComponent<CanvasScaler>().matchWidthOrHeight = 0f;
        //}
        IsAction = false;
    }
    public void ShowUI(UIName uIScreen, Action onHideDone = null)
    {
        ShowUI<UIBase>(uIScreen, onHideDone);
    }
    public void ShowGarage(UIName uiName = UIName.Garage)
    {
        ResourcesManager.Instance.LoadSceneGarage((garage) => ShowScreen(uiName,garage));
    }
    private void ShowScreen(UIName uiName, GarageManager garage)
    {
        HideAllUiActive();
        switch (uiName)
        {
            case UIName.Garage:
                {
                    UIGarage uiGarage = ShowUI<UIGarage>(UIName.Garage);
                    uiGarage.garageManager = garage;
                    garage.SetActiveMotorCharacter(true);
                    garage.OnInScreenUIGarage();
                    garage.ChangeCameraMotor();
                    uiGarage.RefreshUI();
                    break;
                }
            case UIName.Racer:
                {

                    UIRacer uiRacer = ShowUI<UIRacer>(UIName.Racer);
                    uiRacer.garageManager = garage;
                    garage.SetActiveMotorCharacter(true);
                    garage.OnInScreenUIRacer();
                    uiRacer.RefreshUIOnShow();
                    break;
                }
            case UIName.Upgrade:
                {
                    UIRacer uiRacer = ShowUI<UIRacer>(UIName.Upgrade);
                    garage.SetActiveMotorCharacter(false);
                    break;
                }
        }
        AudioManager.Instance.PlayMusicBg();
    }
    public T ShowUI<T>(UIName uIScreen, Action onHideDone = null) where T : UIBase
    {
        UIBase current = listScreenActive.Find(x => x.uiName == uIScreen);
        if (!current)
        {
            current = LoadUI(uIScreen);
            current.uiName = uIScreen;
            AddScreenActive(current, true);
        }
        current.transform.SetAsLastSibling();
        current.Show(onHideDone);
        CurrentName = uIScreen;
        return current as T;
    }

    private void AddScreenActive(UIBase current, bool isTop)
    {
        var idx = listScreenActive.FindIndex(x => x.uiName == current.uiName);
        if (isTop)
        {
            if (idx >= 0)
            {
                listScreenActive.RemoveAt(idx);
            }
            listScreenActive.Add(current);
        }
        else
        {
            if (idx < 0)
            {
                listScreenActive.Add(current);
            }
        }
    }
    public void LoadScene(Action onLoad, Action onDone)
    {
        UILoadScene uILoadScene = ShowUI<UILoadScene>(UIName.LoadScene);
        uILoadScene.LoadSceneCloud(onLoad, onDone);
    }


    public void RefreshUI()
    {
        var idx = 0;
        while (listScreenActive.Count > idx)
        {
            listScreenActive[idx].RefreshUI();
            idx++;
        }
        //topUI.RefreshUI();
        //GameManager.OnRefreshUI?.Invoke();
    }

    //private UIToast _uiToast;
    //public UIToast UIToast()
    //{
    //    if (_uiToast == null)
    //    {
    //        _uiToast = GetComponentInChildren<UIToast>();
    //    }
    //    return _uiToast;
    //}

    public T GetUI<T>(UIName uIScreen) where T : UIBase
    {
        var c = LoadUI(uIScreen);
        return c as T;
    }

    public UIBase GetUI(UIName uIScreen)
    {
        return LoadUI(uIScreen);
    }

    public UIBase GetUiActive(UIName uIScreen)
    {
        return listScreenActive.Find(x => x.uiName == uIScreen);
    }

    public T GetUiActive<T>(UIName uIScreen) where T : UIBase
    {
        var ui = listScreenActive.Find(x => x.uiName == uIScreen);
        if (ui)
        {
            return ui as T;
        }
        else
        {
            return default;
        }
    }

    private UIBase LoadUI(UIName uIScreen)
    {
        UIBase current = null;
        if (cacheScreen.ContainsKey(uIScreen))
        {
            current = cacheScreen[uIScreen];
            if (current == null)
            {
                var idx = dic2[uIScreen].rootIdx;
                current = Instantiate(Resources.Load<UIBase>(PATH_UI + dic2[uIScreen].loadPath), rootOb[idx]);
                cacheScreen[uIScreen] = current;
            }
        }
        else
        {
            var idx = dic2[uIScreen].rootIdx;
            current = Instantiate(Resources.Load<UIBase>(PATH_UI + dic2[uIScreen].loadPath), rootOb[idx]);
            cacheScreen.Add(uIScreen, current);
        }
        return current;
    }

    public void RemoveActiveUI(UIName uiName)
    {
        var idx = listScreenActive.FindIndex(x => x.uiName == uiName);
        if (idx >= 0)
        {
            var ui = listScreenActive[idx];
            listScreenActive.RemoveAt(idx);
            if (!ui.isCache && cacheScreen.ContainsKey(uiName))
            {
                cacheScreen[uiName] = null;
            }
        }
    }

    public void HideAllUIIgnore(UIName uiName)
    {
        int length = listScreenActive.Count;
        for (int i = 0; i < length; i++)
        {
            HideUIIgnore(listScreenActive[0]);
        }
        void HideUIIgnore(UIBase uiBase)
        {
            if(uiBase.uiName != uiName)
            {
                uiBase.Hide();
            }
        }
    }

    public void HideAll()
    {
        while (listScreenActive.Count > 0)
        {
            listScreenActive[0].Hide();
        }
        //topUI.Hide();
    }
    public void HideAllUiActive()
    {
        while (listScreenActive.Count > 0)
        {
            listScreenActive[0].Hide();
        }
    }

    public void HideAllUiActive(params UIName[] ignoreUI)
    {
        for (int i = 0; i < listScreenActive.Count; i++)
        {
            for (int j = 0; j < ignoreUI.Length; j++)
            {
                if (listScreenActive[i].uiName != ignoreUI[j])
                {
                    listScreenActive[i].Hide();
                }
            }
        }
    }

    public void HideUiActive(UIName uiName)
    {
        var ui = listScreenActive.Find(x => x.uiName == uiName);
        if (ui)
        {
            ui.Hide();
        }
    }

    public UIBase GetLastUiActive()
    {
        if (listScreenActive.Count == 0) return null;
        return listScreenActive.Last();
    }
}

public enum UIName
{
    None = 0,
    Gameplay = 1,
    Settings = 2,
    MainMenu = 3,
    WinClassic = 4,
    LoseClassic = 5,
    Splash = 6,
    Resume = 7,
    LoadScene = 8,
    Garage = 9,
    Toast = 10,
    AddCoin = 11,
    Racer = 12,
    Upgrade = 13,
    SelectMode = 14,
    SelectLevel = 15,
}
public class DataUIBase
{
    public int rootIdx;
    public int topIdx;
    public string loadPath;

    public DataUIBase(int rootIdx, int topIdx, string loadPath)
    {
        this.rootIdx = rootIdx;
        this.topIdx = topIdx;
        this.loadPath = loadPath;
    }
}
