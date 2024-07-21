using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;



public class GameUtil : Singleton<GameUtil>
{
    public static void ButtonOnClick(Button bt, UnityAction action, bool isAnim = false)
    {
        UnityAction _action = () =>
        {
            action?.Invoke();
            AudioManager.Instance.PlaySFXBtn();
        };
        if (isAnim)
        {
            bt.OnClickAnim(_action);
        }
        else
        {
            bt.onClick.AddListener(() =>
            {
                ClickNormal(_action);
            });
        }


        void ClickNormal(UnityAction action)
        {
            action?.Invoke();
        }
    }

    public static string LongTimeSecondToUnixTime(long unixTimeSeconds, bool isDoubleParam = false, string day = "D", string hour = "H", string minutes = "M", string second = "S")
    {
        TimeSpan dateTime = TimeSpan.FromSeconds(unixTimeSeconds);
        return TimeSpanToUnixTime(dateTime, isDoubleParam, day, hour, minutes, second);
    }
    public static string FloatTimeSecondToUnixTime(float unixTimeSeconds, bool isDoubleParam = false, string day = "D", string hour = "H", string minutes = "M", string second = "S")
    {
        TimeSpan dateTime = TimeSpan.FromSeconds(unixTimeSeconds);
        string strValue = TimeSpanToUnixTime(dateTime, isDoubleParam, day, hour, minutes, second);
        int milliseconds = dateTime.Milliseconds;
        if (milliseconds > 0)
        {
            strValue += $".{milliseconds:D3}";
        }
        return strValue;

    }
    public static string TimeSpanToUnixTime(TimeSpan dateTime, bool isDoubleParam = false, string day = "D", string hour = "H", string minutes = "M", string second = "S")
    {
        string strValue = "";
        if (dateTime.Days > 0)
        {
            if (dateTime.Hours == 0 && !isDoubleParam)
            {
                strValue = $"{dateTime.Days:D2}{day}";
            }
            else
            {
                strValue = $"{dateTime.Days:D2}{day}:{dateTime.Hours:D2}{hour}";
            }
        }

        else if (dateTime.Hours > 0 && !isDoubleParam)
        {
            if (dateTime.Minutes == 0)
            {
                strValue = $"{dateTime.Hours:D2}{hour}";
            }
            else
            {
                strValue = $"{dateTime.Hours:D2}{hour}:{dateTime.Minutes:D2}{minutes}";
            }
        }
        else
        {
            if (dateTime.Seconds == 0 && !isDoubleParam)
            {
                strValue = $"{dateTime.Minutes:D2}{minutes}";
            }
            else
            {
                strValue = $"{dateTime.Minutes:D2}{minutes}:{dateTime.Seconds:D2}{second}";
            }
        }
        return strValue;
    }
    public static void Log(string debug)
    {
#if DEBUG_DA
        Debug.Log($"DEBUG_DA: {debug}");
#endif
    }
    public static void LogError(string debug)
    {
#if DEBUG_DA
        Debug.LogError($"DEBUG_DA: {debug}");
#endif
    }
    [SerializeField]
    private bool isActionPerSecond = false;
    private void Start()
    {
        if(isActionPerSecond)
        {
            StartCount();
        }
    }
    private void StartCount()
    {
        StartCoroutine(IECountTime());
    }
    private IEnumerator IECountTime()
    {
        WaitForSeconds wait = new WaitForSeconds(1);
        while (true)
        {
            long timeCb = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            Observer.Instance.Notify(ObserverKey.TimeActionPerSecond, timeCb);
            yield return wait;
        }
    }
    public void WaitAndDo(float time, Action action)
    {
        StartCoroutine(ieWaitAndDo(time, action));
    }
    private IEnumerator ieWaitAndDo(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();
    }
    private Dictionary<MonoBehaviour, Coroutine> coroutineDictionary = new Dictionary<MonoBehaviour, Coroutine>();

    public void StartLerpValue(MonoBehaviour behaviour, float preValue, float value , float timeLerp = 1f, Action<float> action = null,  Action onComplete = null)
    {
        Coroutine coroutine;
        if (coroutineDictionary.TryGetValue(behaviour, out coroutine))
        {
            action?.Invoke(value);
            behaviour.StopCoroutine(coroutine);
            coroutineDictionary.Remove(behaviour); 
        }
        coroutine = behaviour.StartCoroutine(IEFloatLerp(behaviour, preValue, value, action, timeLerp, onComplete));
        coroutineDictionary.Add(behaviour, coroutine);
    }

    private IEnumerator IEFloatLerp(MonoBehaviour behaviour, float preValue, float value, Action<float> action = null, float timeLerp = 1f, Action onComplete = null)
    {
        float sub = timeLerp / Time.deltaTime;
        float delta = (preValue - value) / sub;
        bool isNegativeValue = preValue < value;
        while (true)
        {
            preValue -= delta;
            if ((!isNegativeValue && (preValue <= value)) || (isNegativeValue && (preValue >= value)))
            {
                action?.Invoke(value);
                onComplete?.Invoke();
                coroutineDictionary.Remove(behaviour);
                yield break;
            }
            action?.Invoke(preValue);
            yield return null;
        }
    }
    public void OnDisableLerpValue(MonoBehaviour behaviour)
    {
        Coroutine coroutine;
        if (coroutineDictionary.TryGetValue(behaviour, out coroutine))
        {
            behaviour.StopCoroutine(coroutine);
        }
    }
    public static float[] Vector2ToFloatArray(Vector2 value)
    {
        return new float[]
        {
            value.x,
            value.y,
        };
    }
    public static Vector2 FloatArrayToVector2(float[] value)
    {
        return new Vector2(value[0], value[1]);
    }

    public static bool IsLayerInLayerMask(int layer, LayerMask layerMask)
    {
        return ((layerMask.value & (1 << layer)) != 0);
    }
    public static int GetIndexStats(StatsMotorbike myStats)
    {
        int indexStats = 0;
        switch (myStats)
        {
            case StatsMotorbike.MaxSpeed:
                {
                    indexStats = 0;
                    break;
                }
            case StatsMotorbike.Acceleration:
                {
                    indexStats = 1;
                    break;
                }
            case StatsMotorbike.Handling:
                {
                    indexStats = 2;
                    break;
                }
            case StatsMotorbike.Brake:
                {
                    indexStats = 3;
                    break;
                }
        }
        return indexStats;
    }
    public static int[] GetArrayThemeLevel()
    {
        return (int[])Enum.GetValues(typeof(ThemeLevel));
    }
    public static string ToOrdinal(int number)
    {
        if (number <= 0) return number.ToString();

        int lastTwoDigits = number % 100;
        int lastDigit = number % 10;

        if (lastTwoDigits >= 11 && lastTwoDigits <= 13)
        {
            return number + "th";
        }

        switch (lastDigit)
        {
            case 1:
                return number + "st";
            case 2:
                return number + "nd";
            case 3:
                return number + "rd";
            default:
                return number + "th";
        }
    }
}


public static class HelperClass
{

    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}
