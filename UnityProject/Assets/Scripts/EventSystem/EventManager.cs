using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public delegate void OnBuyItem(GameObject item);
    public static event OnBuyItem OnBuyItemEvent;
    public static void OnBuyEventTrigger(GameObject item)
    {
        OnBuyItemEvent?.Invoke(item);
    }

    public delegate void OnUpdateUI();
    public static event OnUpdateUI OnUpdateUIEvent;
    public static void OnUpdateUITriger()
    {
        OnUpdateUIEvent?.Invoke();
    }
}
