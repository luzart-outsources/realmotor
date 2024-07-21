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
    protected override void Setup()
    {
        base.Setup();

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
        if(btnSelectCache!= null)
        {
            btnSelectCache.Select(false);
        }
        btnSelectCache = btnSelect;
        btnSelectCache.Select(true);
        SpawnLevel(btnSelect.index);
    }
    private void SpawnLevel(int index)
    {
        ThemeLevel themeLevel = (ThemeLevel)index;
        List<DB_Level> list= levelSO.GetAllDBThemeLevel(themeLevel);
        int length = list.Count;
        MasterHelper.InitListObj<ItemSelectLevelUI>(length, itemSelectLevelPrefabs, listItemSelectLevel, parentSpawnLevel, (item, index) =>
        {
            item.gameObject.SetActive(true);
            item.InitItem(list[index], ClickLevel);
        });
    }
    private void ClickLevel(ItemSelectLevelUI item)
    {
        int level = item.db_Level.level;
        GameManager.Instance.PlayGameMode(EGameMode.Classic, level);
    }

}
