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
