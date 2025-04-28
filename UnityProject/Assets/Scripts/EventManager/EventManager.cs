using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public  delegate void OnPlayerScore(ulong killerClientId);
    public static event OnPlayerScore OnPlayerScoreEvent;

    public static event Action OnPlayerDeath;


    public delegate void OnChangeGun(GunDetail bullet);
    public static event OnChangeGun OnChangeGunEvent;

    public delegate void OnNewGunSelected(int gunButton);
    public static event OnNewGunSelected OnNewGunSelectedEvent;


    public delegate void OnShakeCamera(float intensity, float frequency, float duration);
    public static event OnShakeCamera OnShakeCameraEvent;

    public delegate void OnPauseGame(bool pause);
    public static event OnPauseGame OnPauseGameEvent;

    public delegate void OnMenuButtonPressed();
    public static event OnMenuButtonPressed OnMenuButtonPressedEvent;

    public delegate void OnGameEnd();
    public static event OnGameEnd OnGameEndEvent;

    public delegate void OnUpdateUI();
    public static event OnUpdateUI OnUpdateUIEvent;

    public delegate void OnCallStorePanel();
    public static event OnCallStorePanel OnCallStorePanelEvent;

    public delegate void OnCallGameOver(int playerNum);
    public static event OnCallGameOver OnCallGameOverEvent;

    public delegate void OnAddToHitsCount();
    public static event OnAddToHitsCount OnAddToHitsCountEvent;

    public delegate void OnSendTransaction(string item, float amount);
    public static event OnSendTransaction OnSendTransactionEvent;

    public delegate void OnReceiveBlockchain(string text);
    public static event OnReceiveBlockchain OnReceiveBlockchainEvent;


    public void OnGameEndTrigger()
    {
        OnGameEndEvent.Invoke();
    }

    public static void OnPlayerScoreTrigger(ulong killerClientId)
    {
        OnPlayerScoreEvent?.Invoke(killerClientId);
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

    public static void OnUpdateUITrigger()
    {
        OnUpdateUIEvent?.Invoke();
    }

    public static void OnCallStorePanelTrigger()
    {
        OnCallStorePanelEvent?.Invoke();
    }
    public static void OnCallGameOverTrigger(int playerNum)
    {
        OnCallGameOverEvent?.Invoke(playerNum);
    }
    public static void OnAddToHitsCountTrigger()
    {
        OnAddToHitsCountEvent?.Invoke();
    }
    public static void OnNewGunSelectedTrigger(int gun)
    {
        OnNewGunSelectedEvent?.Invoke(gun);
    }

    public static void OnSendTransactionTrigger(string item, float amount)
    {
        OnSendTransactionEvent?.Invoke(item, amount);
    }

    public static void OnReceiveBlockchainTrigger(string text)
    {
        OnReceiveBlockchainEvent?.Invoke(text);
    }
}
