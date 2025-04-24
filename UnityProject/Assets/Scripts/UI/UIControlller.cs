using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class UIControlller : NetworkBehaviour
{
    [SerializeField] private GameObject _storePanel;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TMP_Text _victoryText;

    private void Start()
    {
        EventManager.OnCallStorePanelEvent += ShowStorePanel;
        EventManager.OnCallGameOverEvent += CallShowGameOverServerRpc;
    }

    private void OnDestroy()
    {
        EventManager.OnCallStorePanelEvent -= ShowStorePanel;
        EventManager.OnCallGameOverEvent -= CallShowGameOverServerRpc;
    }

    private void ShowStorePanel()
    {
        Time.timeScale = 0;
        _storePanel.SetActive(true);
    }

    [ServerRpc]
    private void CallShowGameOverServerRpc(int playerNum)
    {
        ShowGameOverPanelClientRpc(playerNum);
    }

    [ClientRpc]
    private void ShowGameOverPanelClientRpc(int playerNum)
    {
        Time.timeScale = 0;
        _gameOverPanel.SetActive(true);
        _victoryText.text = "Congratulations!!!\r\n<size=55%> <color=#FFE900>* Player " + playerNum + " Wins *";
    }
}
