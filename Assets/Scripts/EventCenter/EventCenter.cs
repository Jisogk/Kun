using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 事件中心
/// 实现观察者模式
/// 用于分发事件
/// </summary>
public class EventCenter
{
    private static Dictionary<EventCode, Delegate> m_EventTable = new Dictionary<EventCode, Delegate>();

    /// <summary>
    /// 添加委托时检测是否有异常
    /// </summary>
    /// <param name="eventCode"></param>
    /// <param name="callBack"></param>
    private static void OnListnerAdding(EventCode eventCode, Delegate callBack)
    {
        if (!m_EventTable.ContainsKey(eventCode))
        {
            m_EventTable.Add(eventCode, null);
        }
        Delegate d = m_EventTable[eventCode];
        if (d != null && d.GetType() != callBack.GetType())
        {
            throw new Exception(string.Format("尝试为事件{0}添加不同类型的委托，当前事件对应的委托是{1},要添加的委托类型为{2}", eventCode, d.GetType(), callBack));
        }
    }

    /// <summary>
    /// 无参数的添加委托方法
    /// </summary>
    /// <param name="eventCode"></param>
    /// <param name="callBack"></param>
    public static void AddListener(EventCode eventCode, CallBack callBack)
    {
        OnListnerAdding(eventCode, callBack);
        m_EventTable[eventCode] = (CallBack)m_EventTable[eventCode] + callBack;
    }

    /// <summary>
    /// 有一个参数的添加委托方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eventCode"></param>
    /// <param name="callBack"></param>
    public static void AddListener<T>(EventCode eventCode, CallBack<T> callBack)
    {
        OnListnerAdding(eventCode, callBack);
        m_EventTable[eventCode] = (CallBack<T>)m_EventTable[eventCode] + callBack;
    }

    /// <summary>
    /// 有两个参数的添加委托方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="X"></typeparam>
    /// <param name="eventCode"></param>
    /// <param name="callBack"></param>
    public static void AddListener<T, X>(EventCode eventCode, CallBack<T, X> callBack)
    {
        OnListnerAdding(eventCode, callBack);
        m_EventTable[eventCode] = (CallBack<T, X>)m_EventTable[eventCode] + callBack;
    }

    /// <summary>
    /// 有三个参数的添加委托方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="X"></typeparam>
    /// <typeparam name="Y"></typeparam>
    /// <param name="eventCode"></param>
    /// <param name="callBack"></param>
    public static void AddListener<T, X, Y>(EventCode eventCode, CallBack<T, X, Y> callBack)
    {
        OnListnerAdding(eventCode, callBack);
        m_EventTable[eventCode] = (CallBack<T, X, Y>)m_EventTable[eventCode] + callBack;
    }

    /// <summary>
    /// 有四个参数的添加委托方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="X"></typeparam>
    /// <typeparam name="Y"></typeparam>
    /// <typeparam name="Z"></typeparam>
    /// <param name="eventCode"></param>
    /// <param name="callBack"></param>
    public static void AddListener<T, X, Y, Z>(EventCode eventCode, CallBack<T, X, Y, Z> callBack)
    {
        OnListnerAdding(eventCode, callBack);
        m_EventTable[eventCode] = (CallBack<T, X, Y, Z>)m_EventTable[eventCode] + callBack;
    }

    /// <summary>
    /// 移除委托时检测是否有异常
    /// </summary>
    /// <param name="eventCode"></param>
    /// <param name="callBack"></param>
    private static void OnListenerRemoving(EventCode eventCode, Delegate callBack)
    {
        if (m_EventTable.ContainsKey(eventCode))
        {
            Delegate d = m_EventTable[eventCode];
            if (d == null)
            {
                throw new Exception(string.Format("移除监听错误：事件{0}没有对应的委托", eventCode));
            }
            else if (d.GetType() != callBack.GetType())
            {
                throw new Exception(string.Format("移除监听错误：尝试为事件{0}移除不同类型的委托，当前事件对应的委托是{1},要移除的委托类型为{2}", eventCode, d.GetType(), callBack));
            }

        }
        else
        {
            throw new Exception(string.Format("移除监听错误：事件{0}不存在", eventCode));
        }
    }

    /// <summary>
    /// 移除委托后检测是否存在空事件码
    /// 若有则移除
    /// </summary>
    /// <param name="eventCode"></param>
    private static void OnListenerRemoved(EventCode eventCode)
    {
        if (m_EventTable[eventCode] == null)
        {
            m_EventTable.Remove(eventCode);
        }
    }

    /// <summary>
    /// 无参数的移除委托方法
    /// </summary>
    /// <param name="eventCode"></param>
    /// <param name="callBack"></param>
    public static void RemoveListener(EventCode eventCode, CallBack callBack)
    {
        OnListenerRemoving(eventCode, callBack);
        m_EventTable[eventCode] = (CallBack)m_EventTable[eventCode] - callBack;
        OnListenerRemoved(eventCode);
    }

    /// <summary>
    /// 有一个参数的移除委托方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eventCode"></param>
    /// <param name="callBack"></param>
    public static void RemoveListener<T>(EventCode eventCode, CallBack<T> callBack)
    {
        OnListenerRemoving(eventCode, callBack);
        m_EventTable[eventCode] = (CallBack<T>)m_EventTable[eventCode] - callBack;
        OnListenerRemoved(eventCode);
    }

    /// <summary>
    /// 有两个参数的移除委托方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="X"></typeparam>
    /// <param name="eventCode"></param>
    /// <param name="callBack"></param>
    public static void RemoveListener<T, X>(EventCode eventCode, CallBack<T, X> callBack)
    {
        OnListenerRemoving(eventCode, callBack);
        m_EventTable[eventCode] = (CallBack<T, X>)m_EventTable[eventCode] - callBack;
        OnListenerRemoved(eventCode);
    }

    /// <summary>
    /// 有三个参数的移除委托方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="X"></typeparam>
    /// <typeparam name="Y"></typeparam>
    /// <param name="eventCode"></param>
    /// <param name="callBack"></param>
    public static void RemoveListener<T, X, Y>(EventCode eventCode, CallBack<T, X, Y> callBack)
    {
        OnListenerRemoving(eventCode, callBack);
        m_EventTable[eventCode] = (CallBack<T, X, Y>)m_EventTable[eventCode] - callBack;
        OnListenerRemoved(eventCode);
    }

    /// <summary>
    /// 有四个参数的移除委托方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="X"></typeparam>
    /// <typeparam name="Y"></typeparam>
    /// <typeparam name="Z"></typeparam>
    /// <param name="eventCode"></param>
    /// <param name="callBack"></param>
    public static void RemoveListener<T, X, Y, Z>(EventCode eventCode, CallBack<T, X, Y, Z> callBack)
    {
        OnListenerRemoving(eventCode, callBack);
        m_EventTable[eventCode] = (CallBack<T, X, Y, Z>)m_EventTable[eventCode] - callBack;
        OnListenerRemoved(eventCode);
    }

    /// <summary>
    /// 无参数的广播方法
    /// </summary>
    /// <param name="eventCode"></param>
    public static void Broadcast(EventCode eventCode)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(eventCode, out d))
        {
            CallBack callBack = d as CallBack;
            if (callBack != null)
            {
                callBack();
            }
            else
            {
                throw new Exception(string.Format("广播类型错误：事件{0}对应的委托具有不同类型", eventCode));
            }
        }
    }

    /// <summary>
    /// 有一个参数的广播方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eventCode"></param>
    /// <param name="args"></param>
    public static void Broadcast<T>(EventCode eventCode, T args)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(eventCode, out d))
        {
            CallBack<T> callBack = d as CallBack<T>;
            if (callBack != null)
            {
                callBack(args);
            }
            else
            {
                throw new Exception(string.Format("广播类型错误：事件{0}对应的委托具有不同类型", eventCode));
            }
        }
    }

    /// <summary>
    /// 有两个参数的广播方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="X"></typeparam>
    /// <param name="eventCode"></param>
    /// <param name="args"></param>
    /// <param name="args1"></param>
    public static void Broadcast<T, X>(EventCode eventCode, T args, X args1)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(eventCode, out d))
        {
            CallBack<T, X> callBack = d as CallBack<T, X>;
            if (callBack != null)
            {
                callBack(args, args1);
            }
            else
            {
                throw new Exception(string.Format("广播类型错误：事件{0}对应的委托具有不同类型", eventCode));
            }
        }
    }

    /// <summary>
    /// 有三个参数的广播方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="X"></typeparam>
    /// <typeparam name="Y"></typeparam>
    /// <param name="eventCode"></param>
    /// <param name="args"></param>
    /// <param name="args1"></param>
    /// <param name="args2"></param>
    public static void Broadcast<T, X, Y>(EventCode eventCode, T args, X args1, Y args2)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(eventCode, out d))
        {
            CallBack<T, X, Y> callBack = d as CallBack<T, X, Y>;
            if (callBack != null)
            {
                callBack(args, args1, args2);
            }
            else
            {
                throw new Exception(string.Format("广播类型错误：事件{0}对应的委托具有不同类型", eventCode));
            }
        }
    }

    /// <summary>
    /// 有四个参数的广播方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="X"></typeparam>
    /// <typeparam name="Y"></typeparam>
    /// <typeparam name="Z"></typeparam>
    /// <param name="eventCode"></param>
    /// <param name="args"></param>
    /// <param name="args1"></param>
    /// <param name="args2"></param>
    /// <param name="args3"></param>
    public static void Broadcast<T, X, Y, Z>(EventCode eventCode, T args, X args1, Y args2, Z args3)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(eventCode, out d))
        {
            CallBack<T, X, Y, Z> callBack = d as CallBack<T, X, Y, Z>;
            if (callBack != null)
            {
                callBack(args, args1, args2, args3);
            }
            else
            {
                throw new Exception(string.Format("广播类型错误：事件{0}对应的委托具有不同类型", eventCode));
            }
        }
    }
}
