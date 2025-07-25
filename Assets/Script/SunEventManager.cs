using System;
using UnityEngine;

public class SunEventManager : MonoBehaviour
{
    public static event Action OnSunGravityCollapse;

    public static void TriggerSunGravityCollapse()
    {
        OnSunGravityCollapse?.Invoke();
    }
}