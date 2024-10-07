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
    public ItemTitleSelectLevel itemTitleSelectLevel;
    public int indexThemeLevel;
    public List<ItemChangeTitle> listchangeTitle = new List<ItemChangeTitle>();

    public ScrollRect scrollRect;
    public Transform parentSpawnLevel;
    public ItemSelectLevelUI itemSelectLevelPrefabs;
    public List<ItemSelectLevelUI> listItemSelectLevel = new List<ItemSelectLevelUI>();
    private RectTransform _rtContent = null;
    public RectTransform rtContent
    {
        get
        {
            if (_rtContent == null)
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
        UIManager.Instance.ShowGarage();



        if (isOutClickLevel)
        {
            FirebaseNotificationLog.LogLevel(KeyFirebase.StepClickBackUILevel, level);
        }
    }
    public override void Show(Action onHideDone)
    {
        base.Show(onHideDone);
        int[] valueInt = GameUtil.GetArrayThemeLevel();
        int length = valueInt.Length;

        int level = DataManager.Instance.CurrentLevel;
        int themeLevel = DataManager.Instance.levelSO.GetIndexThemeLevel(level);
        indexThemeLevel = themeLevel;
        string title = ((ThemeLevel)themeLevel).ToString();
        itemTitleSelectLevel.SetTextTitle(title);
        SpawnLevel(themeLevel);
        for (int i = 0; i < listchangeTitle.Count; i++)
        {
            listchangeTitle[i].InitAction(i, OnClickTitle);
        }

        /*RectTransform posSelect = GetPosItemSelect(level);
        GameUtil.Instance.WaitAndDo(0, () =>
        {
            scrollRect.FocusOnRectTransform(posSelect);
        });*/
        //float x = PosXNew(posSelect);
        //rtContent.anchoredPosition = new Vector2(-x, rtContent.anchoredPosition.y);
    }
    private void OnClickTitle(ButtonSelect btnSelect)
    {
        int temp = indexThemeLevel + (btnSelect.index > 0 ? 1 : -1);
        indexThemeLevel = Mathf.Max(0, temp);
        SpawnLevel(indexThemeLevel);
    }
    public float timeShowLevelEach = 0.18f;
    private void SpawnLevel(int index)
    {
        ThemeLevel themeLevel = (ThemeLevel)index;
        string title = themeLevel.ToString();
        itemTitleSelectLevel.SetTextTitle(title);

        List<DB_Level> list = levelSO.GetAllDBThemeLevel(themeLevel);
        int length = list.Count;
        if (listItemSelectLevel != null && listItemSelectLevel.Count > 0)
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
        //scrollRect.content.anchoredPosition = new Vector2(0, 0);
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
        if (db_level == null)
        {
            return listItemSelectLevel[count - 1].GetComponent<RectTransform>(); ;
        }
        int length = listItemSelectLevel.Count;
        for (int i = 0; i < length; i++)
        {
            var item = listItemSelectLevel[i];
            if (db_level.level == item.db_Level.level)
            {
                rtPos = item.GetComponent<RectTransform>();
            }
        }
        return rtPos;
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

        // 
        if (isOutClickLevel)
        {
            ParameterFirebaseCustom[] param = new ParameterFirebaseCustom[2];
            param[0] = new ParameterFirebaseCustom(KeyTypeFirebase.Amount, level.ToString());
            param[1] = new ParameterFirebaseCustom(KeyTypeFirebase.Level, this.level.ToString());
            FirebaseNotificationLog.LogWithLevelMax(KeyFirebase.StepClickUILevelInGame, param);
        }

    }
    private void OnDisable()
    {
        this.DOKill(false);
    }


    public int level;
    public bool isOutClickLevel = false;
}
