using System.Collections.Generic;
using System;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

public static class UIExtension
{
    public static void OnClickAnim(this Button btn, UnityAction action)
    {
        if (btn == null)
        {
            return;
        }
        var effect = btn.GetComponent<EffectButton>();
        if (effect == null)
        {
            effect = btn.gameObject.AddComponent<EffectButton>();
        }
        btn.onClick.AddListener(() =>
        {
            action?.Invoke();
        });
    }
    public static void FocusOnRectTransform(this ScrollRect scrollRect, RectTransform itemRectTransform)
    {
        Canvas.ForceUpdateCanvases();
        Vector3[] itemCorners = new Vector3[4];
        itemRectTransform.GetWorldCorners(itemCorners);
        Vector3[] viewCorners = new Vector3[4];
        scrollRect.viewport.GetWorldCorners(viewCorners);

        float difference = 0;

        if (scrollRect.horizontal)
        {
            // Tập trung theo chiều ngang
            if (itemCorners[2].x > viewCorners[2].x)
            {
                difference = itemCorners[2].x - viewCorners[2].x;
            }
            else if (itemCorners[0].x < viewCorners[0].x)
            {
                difference = itemCorners[0].x - viewCorners[0].x;
            }
            float width = viewCorners[2].x - viewCorners[0].x;
            float normalizedDifference = difference / width;
            Vector2 posCurrent = scrollRect.content.anchoredPosition;
            Vector2 size = scrollRect.content.sizeDelta;
            scrollRect.content.anchoredPosition = new Vector2(posCurrent.x - normalizedDifference * size.x, posCurrent.y);
        }
        else
        {
            // Tập trung theo chiều dọc
            if (itemCorners[1].y > viewCorners[1].y)
            {
                difference = itemCorners[1].y - viewCorners[1].y;
            }
            else if (itemCorners[0].y < viewCorners[0].y)
            {
                difference = itemCorners[0].y - viewCorners[0].y;
            }
            float height = viewCorners[1].y - viewCorners[0].y;
            float normalizedDifference = difference / height;
            Vector2 posCurrent = scrollRect.content.anchoredPosition;
            Vector2 size = scrollRect.content.sizeDelta;
            scrollRect.content.anchoredPosition = new Vector2(posCurrent.x, posCurrent.y - normalizedDifference * size.y);
        }
    }
    public static void FocusOnRectTransform(this ScrollRect scrollRect, RectTransform itemRectTransform, float elasticity)
    {
        scrollRect.elasticity = 0.5f;
        FocusOnRectTransform(scrollRect, itemRectTransform);
    }
    public static void SetInteractable(this Button bt, bool interactable)
    {
        var graphics = bt.GetComponentsInChildren<MaskableGraphic>();
        bt.interactable = interactable;
        for (int i = 0; i < graphics.Length; i++)
        {
            graphics[i].color = interactable ? bt.colors.normalColor : bt.colors.disabledColor;
        }
    }

    public static int SumRange(this IList<int> collection, int min, int max)
    {
        int num = 0;
        for (var i = min; i <= max && i < collection.Count; i++)
        {
            int obj = collection[i];
            num += obj;
        }

        return num;
    }

    public static int IndexOf<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
    {
        int num = 0;
        foreach (T obj in collection)
        {
            if (predicate(obj))
                return num;
            ++num;
        }
        return -1;
    }
}
