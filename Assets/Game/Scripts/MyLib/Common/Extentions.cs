using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using UnityEngine;

public static class Extentions
{
    #region GEOMETRY
    public static float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 right = Vector3.Cross(up, fwd);        // right vector
        float dir = Vector3.Dot(right, targetDir);

        if (dir > 0f)
        {
            return 1f;
        }
        else if (dir < 0f)
        {
            return -1f;
        }
        else
        {
            return 0f;
        }
    }
    #endregion GEOMETRY

    #region LIST

    public static IList<T> Shuffle<T>(this IList<T> list)
    {
        RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
        int n = list.Count;
        while (n > 1)
        {
            byte[] box = new byte[1];
            do provider.GetBytes(box);
            while (!(box[0] < n * (Byte.MaxValue / n)));
            int k = (box[0] % n);
            n--;
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }
    public static T GetRandomElement<T>(this IList<T> list)
    {
        int n = list.Count;
        int rd = UnityEngine.Random.Range(0, n);
        return list[rd];
    }
    #endregion

    #region COROUTINE

    public static void StartDelayAction(this MonoBehaviour mono, float time, Action callback)
    {
        mono.StartCoroutine(Delay(callback, time));
    }

    public static void StartActionEndOfFrame(this MonoBehaviour mono, Action callback)
    {
        mono.StartCoroutine(DelayEndOfFrame(callback));
    }

    private static IEnumerator Delay(Action callBack, float time)
    {
        yield return Yielder.Get(time);
        callBack.Invoke();
    }

    private static IEnumerator DelayEndOfFrame(Action callBack)
    {
        yield return new WaitForEndOfFrame();
        callBack.Invoke();
    }

    #endregion

    #region TRANSFORM
    public static Vector3 GetWorldPosition(this RectTransform rect)
    {
        return rect.TransformPoint(rect.transform.position);
    }
    public static void MoveTransformWithoutPhysic(this Transform transform, Vector3 direct, float speed)
    {
        transform.position = new Vector3(transform.position.x + speed * Time.deltaTime * direct.x,
            transform.position.y + speed * Time.deltaTime * direct.y,
            transform.position.z);
    }

    public static void MoveTransformWithPhysic(this Transform transform, Vector3 direct, float speed)
    {
        //transform.position += speed * Time.fixedDeltaTime * direct;
        transform.position = new Vector3(transform.position.x + speed * Time.fixedDeltaTime * direct.x,
            transform.position.y + speed * Time.fixedDeltaTime * direct.y,
            transform.position.z);
    }
    public static EdgeScreenType CheckTriggerEdgeScreen(this Transform transform)
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        if (pos.x < 0.07) return EdgeScreenType.LEFT;
        if (0.93 < pos.x) return EdgeScreenType.RIGHT;
        if (pos.y < 0.04) return EdgeScreenType.BOTTOM;
        if (0.96 < pos.y) return EdgeScreenType.TOP;
        return EdgeScreenType.NONE;
    }
    #endregion

    #region STRING
    static string STR_KEY = "k";
    public static string ToDictKey(this int value)
    {
        return string.Concat(STR_KEY, value);
    }
    public static int DictKeyToInt(this string dictKey)
    {
        return int.Parse(dictKey.Substring(1));
    }
    public static bool NotEquals(this string value, string pair)
    {
        return !value.Equals(pair);
    }
    public static T ToEnum<T>(this string value)
    {
        value.ToUpper();
        return (T)Enum.Parse(typeof(T), value, true);
    }


    //public static string ToStringRate(this float num, ColorText color = ColorText.GREEN)
    //{
    //    float percent = num * 100f;
    //    string s = string.Empty;

    //    if (color == ColorText.NONE)
    //    {
    //        s = string.Format("{0}%", percent);
    //    }
    //    else
    //    {
    //        string hexCode = string.Empty;

    //        switch (color)
    //        {
    //            case ColorText.WHITE:
    //                {
    //                    hexCode = GameUtils.GetColorHexCode(GameConfig.Instance.colorWhite);
    //                }
    //                break;

    //            default:
    //                {
    //                    hexCode = GameUtils.GetColorHexCode(GameConfig.Instance.colorGreen);
    //                }
    //                break;
    //        }

    //        s = string.Format("<color=#{0}>{1}%</color>", hexCode, percent);
    //    }

    //    return s;
    //}
    public static string ToShortStringK(this int num)
    {
        if (num < 1000)
        {
            return num.ToString("n0");

        }
        else if (num < 1000000)
        {
            int integer = (int)num / 1000;
            int decim = (int)num % 1000;

            if (decim >= 10)
            {
                return (num / (float)1000).ToString("f2") + "K";
            }
            else
            {
                return integer.ToString() + "K";
            }
        }
        else if (num < 1000000000)
        {
            int integer = (int)num / 1000000;
            int decim = (int)num % 1000000;

            if (decim >= 10000)
            {
                return (num / (float)1000000).ToString("f2") + "M";
            }
            else
            {
                return integer.ToString() + "M";
            }
        }
        else
        {
            return (num / (float)1000000000).ToString("f2") + "B";
        }
    }

    public static string ToShortStringK(this long num)
    {
        if (num < 1000)
        {
            return num.ToString("n0");

        }
        else if (num < 1000000)
        {
            int integer = (int)num / 1000;
            int decim = (int)num % 1000;

            if (decim >= 10)
            {
                return (num / (float)1000).ToString("f2") + "K";
            }
            else
            {
                return integer.ToString() + "K";
            }
        }
        else if (num < 1000000000)
        {
            int integer = (int)num / 1000000;
            int decim = (int)num % 1000000;

            if (decim >= 10000)
            {
                return (num / (float)1000000).ToString("f2") + "M";
            }
            else
            {
                return integer.ToString() + "M";
            }
        }
        else
        {
            return (num / (float)1000000000).ToString("f2") + "B";
        }
    }
    public static string ToShortString(this long num)
    {
        if (num < 1000000)
        {
            return num.ToString("n0");
        }
        else if (num < 1000000000)
        {
            int integer = (int)num / 1000000;
            int decim = (int)num % 1000000;

            if (decim >= 10000)
            {
                return (num / (float)1000000).ToString("f2") + "M";
            }
            else
            {
                return integer.ToString() + "M";
            }
        }
        else
        {
            return (num / (float)1000000000).ToString("f2") + "B";
        }
    }

    public static string ToShortString(this int num)
    {
        return ((long)num).ToShortString();
    }

    public static string ToShortString(this float num)
    {
        return ((long)num).ToShortString();
    }
    #endregion

    #region TIMESPAN
    public static string GetFormattedTimerShort(this TimeSpan ts)
    {
        return string.Format("{0:00}:{1:00}:{2:00}", ts.Days * 24 + ts.Hours, ts.Minutes, ts.Seconds);
    }
    public static int MillisecondsToMin(this long milliseconds)
    {
        return (int)( milliseconds / (1000 * 60));
    }
    public static long MinToMilliseconds(this int minutes)
    {
        return minutes * 1000 * 60;
    }
    public static string MinToHoursString(this int minutes)
    {
        int hours = minutes / 60;
        int remainingMinutes = minutes % 60;
        return $"{hours} hour(s) and {remainingMinutes} minute(s)";
    }
    #endregion
}
