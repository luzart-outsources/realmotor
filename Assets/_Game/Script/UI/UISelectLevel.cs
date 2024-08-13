using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;

public class UISelectLevel : UIBase
{
    private LevelSO levelSO
    {
        get
        {
            return DataManager.Instance.levelSO;
        }
    }
    public Transform parentTitleSpawn;
    public ItemTitleSelectLevel itemTitleSelectLevelPrefab;
    private List<ItemTitleSelectLevel> listItemTitleSelectLevel = new List<ItemTitleSelectLevel>();

    public ScrollRect scrollRect;
    public Transform parentSpawnLevel;
    public ItemSelectLevelUI itemSelectLevelPrefabs;
    private List<ItemSelectLevelUI> listItemSelectLevel = new List<ItemSelectLevelUI>();
    private RectTransform _rtContent = null;
    public RectTransform rtContent
    {
        get
        {
            if(_rtContent == null)
            {
                _rtContent = parentSpawnLevel.GetComponent<RectTransform>();
            }
            return _rtContent;
        }

    }

    private ButtonSelect btnSelectCache;

    public Button btnBack;

    protected override void Setup()
    {
        base.Setup();
        GameUtil.ButtonOnClick(btnBack, ClickBack, true, KeyAds.BtnSelectLevelBack);

    }
    private void ClickBack()
    {
        Hide();
        UIManager.Instance.ShowUI(UIName.SelectMode);
    }
    public override void Show(Action onHideDone)
    {
        base.Show(onHideDone);
        int[] valueInt = GameUtil.GetArrayThemeLevel();
        int length = valueInt.Length;
        MasterHelper.InitListObj<ItemTitleSelectLevel>(length, itemTitleSelectLevelPrefab, listItemTitleSelectLevel, parentTitleSpawn, (item, index) =>
        {
            item.gameObject.SetActive(true);
            item.InitAction(index, OnClickTitle);
            string title = ((ThemeLevel)index).ToString();
            item.SetTextTitle(title);
        });
        int level = DataManager.Instance.CurrentLevel;
        int themeLevel = DataManager.Instance.levelSO.GetIndexThemeLevel(level);
        OnClickTitle(listItemTitleSelectLevel[themeLevel]);
        RectTransform posSelect = GetPosItemSelect(level);
        GameUtil.Instance.WaitAndDo(0, () =>
        {
            scrollRect.FocusOnRectTransform(posSelect);
        });
        //float x = PosXNew(posSelect);
        //rtContent.anchoredPosition = new Vector2(-x, rtContent.anchoredPosition.y);
    }
    private void OnClickTitle(ButtonSelect btnSelect)
    {
        if(btnSelect == btnSelectCache)
        {
            return;
        }
        if(btnSelectCache!= null)
        {
            btnSelectCache.Select(false);
        }
        btnSelectCache = btnSelect;
        btnSelectCache.Select(true);

        SpawnLevel(btnSelect.index);
    }
    public float timeShowLevelEach = 0.18f;
    private void SpawnLevel(int index)
    {
        ThemeLevel themeLevel = (ThemeLevel)index;
        List<DB_Level> list= levelSO.GetAllDBThemeLevel(themeLevel);
        int length = list.Count;
        if(listItemSelectLevel!=null && listItemSelectLevel.Count > 0)
        {
            int lengthList = listItemSelectLevel.Count;
            for (int i = 0; i < lengthList; i++)
            {
                listItemSelectLevel[i].gameObject.SetActive(false);
            }
        }
        MasterHelper.InitListObj<ItemSelectLevelUI>(length, itemSelectLevelPrefabs, listItemSelectLevel, parentSpawnLevel, (item, index) =>
        {
            item.gameObject.SetActive(true);
            item.InitItem(list[index], ClickLevel);
            item.OnHideCanvasG();
        });
        scrollRect.content.anchoredPosition = new Vector2(0, 0);
        int lengthLevel = listItemSelectLevel.Count;
        sequenceSpawn?.Kill(false);
        sequenceSpawn = DOTween.Sequence();
        for (int i = 0; i < lengthLevel; i++)
        {
            var item = listItemSelectLevel[i];
            sequenceSpawn.AppendCallback(() =>
            {
                item.OnShowCanvasG();
            });
            sequenceSpawn.AppendInterval(timeShowLevelEach);

        }
        sequenceSpawn.SetId(this);
    }

    private RectTransform GetPosItemSelect(int level)
    {
        int count = listItemSelectLevel.Count;
        RectTransform rtPos = listItemSelectLevel[0].GetComponent<RectTransform>();
        DB_Level db_level = DataManager.Instance.levelSO.GetDB_Level(level);
        if(db_level == null)
        {
            return listItemSelectLevel[count -1].GetComponent<RectTransform>(); ;
        }
        int length = listItemSelectLevel.Count;
        for (int i = 0; i < length; i++)
        {
            var item = listItemSelectLevel[i];
            if(db_level.level == item.db_Level.level)
            {
                rtPos = item.GetComponent<RectTransform>();
            }
        }
        return rtPos ;
    }
    private float PosXNew(RectTransform rt)
    {
        float initialAnchoredPositionX = rt.anchoredPosition.x;
        Vector2 initialAnchorMin = new Vector2(0.5f, 0.5f);
        Vector2 initialAnchorMax = new Vector2(0.5f, 0.5f);
        float parentWidth = rt.parent.GetComponent<RectTransform>().sizeDelta.x;

        // Tính toán vị trí hiện tại trong không gian parent
        float currentParentX = initialAnchorMin.x * parentWidth + initialAnchoredPositionX;

        // Giá trị neo mới
        Vector2 newAnchorMin = new Vector2(0f, 0f);
        Vector2 newAnchorMax = new Vector2(0f, 1f);

        // Tính toán anchoredPosition.x mới
        float newAnchoredPositionX = currentParentX - newAnchorMin.x * parentWidth;
        return newAnchoredPositionX;
    }
    private Sequence sequenceSpawn;
    private void ClickLevel(ItemSelectLevelUI item)
    {
        int level = item.db_Level.level;
        GameManager.Instance.PlayGameMode(EGameMode.Classic, level);
    }
    private void OnDisable()
    {
        this.DOKill(false);
    }
}
