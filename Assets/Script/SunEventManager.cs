using System;
using UnityEngine;

/// <summary>
/// 太陽に関するグローバルイベントを管理するクラス
/// </summary>
public class SunEventManager : MonoBehaviour
{
    // 全ての天体が購読するためのシンプルな静的イベント
    public static event Action OnSunGravityCollapse;

    /// <summary>
    /// 太陽の重力が崩壊したことを通知するイベントを発行する
    /// </summary>
    public static void TriggerSunGravityCollapse()
    {
        Debug.Log("イベント発行: OnSunGravityCollapse");
        // 購読しているオブジェクトがあればイベントを実行する
        OnSunGravityCollapse?.Invoke();
    }
}