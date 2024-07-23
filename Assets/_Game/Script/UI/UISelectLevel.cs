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

    public Transform parentSpawnLevel;
    public ItemSelectLevelUI itemSelectLevelPrefabs;
    private List<ItemSelectLevelUI> listItemSelectLevel = new List<ItemSelectLevelUI>();

    private ButtonSelect btnSelectCache;

    public Button btnBack;

    protected override void Setup()
    {
        base.Setup();
        GameUtil.ButtonOnClick(btnBack, ClickBack, true);

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
        OnClickTitle(listItemTitleSelectLevel[0]);
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
        int level = DataManager.Instance.CurrentLevel;

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
