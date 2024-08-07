using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    public float timeMove = 3f;
    private Sequence sq;
    public ItemLeaderboard itemPrefab;
    public Transform parentItem;
    private List<ItemLeaderboard> items = new List<ItemLeaderboard>();
    private List<Transform> listRect = new List<Transform>();
    public ScrollRect scrollRect;
    public void InitSpawn(int num, ref List<ItemLeaderboard> list, Action<ItemLeaderboard, int> actionInit)
    {
        MasterHelper.InitListObj(num, itemPrefab, items, parentItem, (item, index) =>
        {
            item.gameObject.SetActive(true);
            actionInit?.Invoke(item, index);
            item.Initialize(index);
        });
        list = items;
        Canvas.ForceUpdateCanvases();
        listRect = items.Select(x => x.transform).ToList();
    }
#if UNITY_EDITOR
    [Sirenix.OdinInspector.Button]
    public void MoveItem(int fromIndex, int toIndex)
    {
        MoveItem(fromIndex, toIndex, null, null, null);
    }
    [Sirenix.OdinInspector.Button]
    public void Initialize()
    {
        InitSpawn(10, ref items, (item, index) =>
        {
            item.gameObject.SetActive(true);
        });
    }
#endif
    public void MoveItem(int fromIndex, int toIndex, Action<ItemLeaderboard> actionStart, Action<ItemLeaderboard> actionOnDone, Action<ItemLeaderboard, int> actionUpdate = null)
    {
        if (fromIndex < 0 || fromIndex >= items.Count || toIndex < 0 || toIndex >= items.Count)
        {
            Debug.LogError("Index out of range");
            return;
        }
        sq = DOTween.Sequence();
        var itemToMove = items[fromIndex];
        float timeEach = timeMove / (fromIndex - toIndex);
        sq.AppendCallback(()=> actionStart?.Invoke(itemToMove));
        sq.AppendInterval(0.5f);
        for (int i = fromIndex; i > toIndex; i--)
        {
            int firstIndex = i;
            int factor = 1;
            int indexToMove = firstIndex - factor;
            sq.Append(items[indexToMove].transform.DOLocalMoveY(listRect[firstIndex].transform.localPosition.y, timeEach).SetEase(Ease.Linear).OnUpdate(() =>
            {
                actionUpdate?.Invoke(itemToMove, indexToMove);
            }));
        }
        sq.AppendInterval(2f);
        sq.Insert(0.5f, itemToMove.transform.DOLocalMoveY(listRect[toIndex].transform.localPosition.y, timeMove).SetEase(Ease.Linear));
        sq.AppendCallback(()=> actionOnDone?.Invoke(itemToMove));
    }
}
