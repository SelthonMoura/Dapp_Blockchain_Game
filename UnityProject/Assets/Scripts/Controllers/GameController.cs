
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using static EventManager;

public class GameController : NetworkBehaviour
{

    [SerializeField]
    private GameObject _gameOverScreen;

    [SerializeField]
    private GameObject _gamePausedScreen;

    [SerializeField]
    private GameObject _pauseButton;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        EventManager.OnPlayerDeath += OnPlayerDeath;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.OnPlayerDeath -= OnPlayerDeath;
    }

    public void OnPlayerDeath()
    {
        OpenGameOverScreen();
        OnGameEndClientRpc();
    }

    [ClientRpc]
    private void OnGameEndClientRpc()
    {
        //EventManager.OnGameEndTrigger(); // Não existe
        OpenGameOverScreen();
    }

    private void OpenGameOverScreen()
    {
        _gameOverScreen.SetActive(true);
        NetworkManager.Singleton.Shutdown();
        Time.timeScale = 0;
    }
    private IEnumerator DisablePauseMenu()
    {
        yield return new WaitForSeconds(1f);
        _gamePausedScreen.SetActive(false);
        _pauseButton.SetActive(true);
    }

    private IEnumerator EnablePauseMenu()
    {
        yield return new WaitForSeconds(1f);
        _gamePausedScreen.SetActive(true);
        _pauseButton.SetActive(false);
        Time.timeScale = 0;
    }
}



