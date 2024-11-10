#if UNITY_EDITOR
#define DEBUG_ENABLE
#define ASSERT
#endif

using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

public class DebugCustom
{
    #region DEBUG LOG
    public static void Log(object content)
    {
#if UNITY_EDITOR
        Debug.Log(content);
#endif
    }

    public static void LogFormat(string format, params object[] args)
    {
#if UNITY_EDITOR
        Debug.Log(string.Format(format, args));
#endif
    }

    public static void LogError(object content)
    {
#if UNITY_EDITOR
        Debug.LogError(content);
#endif
    }

    public static void LogWarning(object content)
    {
#if UNITY_EDITOR
        Debug.LogWarning(content);
#endif
    }

    public static void LogWarning(object message, Object context)
    {
#if UNITY_EDITOR
        Debug.LogWarning(message, context);
#endif
    }

    public static void LogWarning(Object context, string format, params object[] args)
    {
#if UNITY_EDITOR
        Debug.LogWarning(string.Format(format, args), context);
#endif
    }
    #endregion

    #region ASSERT
    /// Thown an exception if condition = false
    public static void Assert(bool condition)
    {
        if (!condition)
        {
            throw new UnityException();
        }
    }

    /// Thown an exception if condition = false, show message on console's log
    public static void Assert(bool condition, string message)
    {
        if (!condition)
        {
            throw new UnityException(message);
        }
    }

    /// Thown an exception if condition = false, show message on console's log
    public static void Assert(bool condition, string format, params object[] args)
    {
        if (!condition)
        {
            throw new UnityException(string.Format(format, args));
        }
    }

    #endregion
}