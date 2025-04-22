using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private GameObject _settingsPanel;
    public GameObject shopPanel;

    private void Start()
    {

    }

    private void OnDestroy()
    {
    }

    public void CloseShop()
    {
        Time.timeScale = 1.0f;
        shopPanel.SetActive(false);
    }

    public void CloseSettings()
    {
        _settingsPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        _settingsPanel.SetActive(true);
    }
}
