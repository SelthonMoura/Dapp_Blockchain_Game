using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private PlayerCurrency _playerCurrency;
    [SerializeField] private TMP_Text _playerScore;
    [SerializeField] private GameObject _settingsPanel;
    public GameObject shopPanel;

    private void Start()
    {
        UpdatePlayerScoreText();
        EventManager.OnUpdateUIEvent += UpdatePlayerScoreText;
    }

    private void OnDestroy()
    {
        EventManager.OnUpdateUIEvent -= UpdatePlayerScoreText;
    }

    public void CloseShop()
    {
        Time.timeScale = 1.0f;
        shopPanel.SetActive(false);
    }

    private void UpdatePlayerScoreText()
    {
        //_playerScore.text = _playerCurrency.playerMoney.ToString();
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
