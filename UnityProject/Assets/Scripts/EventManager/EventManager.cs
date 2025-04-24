using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public  delegate void OnPlayerSocore(int scoreToAdd);
    public static event OnPlayerSocore OnCountScoreEvent;

    public delegate void OnChangeGun(GunDetail bullet);
    public static event OnChangeGun OnChangeGunEvent;

    public static event Action OnPlayerDeath;

    public delegate void OnShakeCamera(float intensity, float frequency, float duration);
    public static event OnShakeCamera OnShakeCameraEvent;

    public delegate void OnPauseGame(bool pause);
    public static event OnPauseGame OnPauseGameEvent;

    public delegate void OnMenuButtonPressed();
    public static event OnMenuButtonPressed OnMenuButtonPressedEvent;
    public delegate void OnGameEnd();
    public static event OnGameEnd OnGameEndEvent;
    public delegate void OnBuyItem(GunSO item);
    public static event OnBuyItem OnBuyItemEvent;
    public static void OnBuyEventTrigger(GunSO item)
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

    public void OnGameEndTrigger()
    {
        OnGameEndEvent.Invoke();
    }

    public static void OnCountScoreTrigger(int scoreToAdd)
    {
        OnCountScoreEvent?.Invoke(scoreToAdd);
    }

    public static void OnPlayerDeathTrigger() { 
        
        OnPlayerDeath?.Invoke();
    }

    public static void OnChangeGunTrigger(GunDetail gunDetail)
    {
        OnChangeGunEvent?.Invoke(gunDetail);
    }

    public static void OnShakeCameraTrigger(float intensity, float frequency, float duration)
    {
        OnShakeCameraEvent?.Invoke(intensity, frequency, duration);
    }

    public static void OnPauseGameTrigger(bool pause)
    {
        OnPauseGameEvent?.Invoke(pause);
    }

    public static void OnMenuButtonPressedTrigger()
    {
        OnMenuButtonPressedEvent?.Invoke();
    }
}
