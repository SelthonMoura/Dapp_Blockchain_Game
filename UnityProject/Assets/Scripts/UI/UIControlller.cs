using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControlller : MonoBehaviour
{
    [SerializeField] private GameObject _storePanel;
    [SerializeField] private GameObject _gameOverPanel;

    private void Start()
    {
        EventManager.OnCallStorePanelEvent += ShowStorePanel;
        EventManager.OnCallGameOverEvent += ShowGameOverPanel;
    }

    private void OnDestroy()
    {
        EventManager.OnCallStorePanelEvent -= ShowStorePanel;
        EventManager.OnCallGameOverEvent -= ShowGameOverPanel;
    }

    private void ShowStorePanel()
    {
        Time.timeScale = 0;
        _storePanel.SetActive(true);
    }

    private void ShowGameOverPanel()
    {
        Time.timeScale = 0;
        _gameOverPanel.SetActive(true);
    }
}
