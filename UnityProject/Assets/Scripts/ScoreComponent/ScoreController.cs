using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ScoreController : NetworkBehaviour
{
    [SerializeField]
    private TMP_Text _scoreTextUI;

    [SerializeField]
    private TMP_Text _scoreTextGameOverScreen;

    private NetworkVariable<int> _currentScore = new NetworkVariable<int>(
         0,
         NetworkVariableReadPermission.Everyone,
         NetworkVariableWritePermission.Owner);

    private void Start()
    {
        EventManager.OnCountScoreEvent += OnPlayerScore;
        EventManager.OnGameEndEvent += OnGameEndTrigger;
        _currentScore.OnValueChanged += UpdateScoreUI;

    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.OnCountScoreEvent -= OnPlayerScore;
        EventManager.OnGameEndEvent -= OnGameEndTrigger;

        _currentScore.OnValueChanged -= UpdateScoreUI;

    }


    public void OnGameEndTrigger()
    {
        _scoreTextUI.gameObject.SetActive(false);
    }

    public override void OnNetworkSpawn()
    {
        UpdateScoreUI(0, _currentScore.Value);
        base.OnNetworkSpawn();
    }

    private void OnPlayerScore(int scoreToAdd)
    {
        if (IsServer && NetworkObject.IsSpawned)
        _currentScore.Value += scoreToAdd;
    }

    private void UpdateScoreUI(int previousValue, int newValue)
    {
        _scoreTextUI.text = newValue.ToString();
        _scoreTextGameOverScreen.text = newValue.ToString();
    }


    public void OnPlayerDeath()
    {
        _scoreTextUI.gameObject.SetActive(false);
    }
}
