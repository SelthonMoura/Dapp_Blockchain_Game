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
    public static void OnUpdateUITrigger()
    {
        OnUpdateUIEvent?.Invoke();
    }

    public delegate void OnCallStorePanel();
    public static event OnCallStorePanel OnCallStorePanelEvent;
    public static void OnCallStorePanelTrigger()
    {
        OnCallStorePanelEvent?.Invoke();
    }

    public delegate void OnCallGameOver();
    public static event OnCallGameOver OnCallGameOverEvent;
    public static void OnCallGameOverTrigger()
    {
        OnCallGameOverEvent?.Invoke();
    }

    public delegate void OnAddToHitsCount();
    public static event OnAddToHitsCount OnAddToHitsCountEvent;
    public static void OnAddToHitsCountTrigger()
    {
        OnAddToHitsCountEvent?.Invoke();
    }
}
