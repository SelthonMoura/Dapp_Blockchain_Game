using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreController : NetworkBehaviour
{
    [SerializeField] private PlayerDataSO _playerDataSO;
    [SerializeField] private TMP_Text _player1Score_txt;
    [SerializeField] private TMP_Text _player2Score_txt;
    [SerializeField] private int _maxScore;

    private NetworkVariable<int> _player1Score = new NetworkVariable<int>(
         0,
         NetworkVariableReadPermission.Everyone,
         NetworkVariableWritePermission.Owner);

    private NetworkVariable<int> _player2Score = new NetworkVariable<int>(
         0,
         NetworkVariableReadPermission.Everyone,
         NetworkVariableWritePermission.Owner);


    private void Start()
    {
        EventManager.OnPlayerScoreEvent += RegisterKill;

        _player1Score.OnValueChanged += UpdatePlayer1ScoreUI;
        _player2Score.OnValueChanged += UpdatePlayer2ScoreUI;
    }

    public override void OnDestroy()
    {
        EventManager.OnPlayerScoreEvent -= RegisterKill;

        _player1Score.OnValueChanged -= UpdatePlayer1ScoreUI;
        _player2Score.OnValueChanged -= UpdatePlayer2ScoreUI;

        base.OnDestroy();
    }

    public override void OnNetworkSpawn()
    {
        ResetMatchScores();

        UpdatePlayer1ScoreUI(0, _player1Score.Value);
        UpdatePlayer2ScoreUI(0, _player2Score.Value);

        base.OnNetworkSpawn();
    }

    public void RegisterKill(ulong killerClientId)
    {
        if (!IsServer) return;

        if (killerClientId == 0) // Assuming Host = Player 1
        {
            _player1Score.Value += 1;
        }
        else if (killerClientId == 1) // Assuming Client = Player 2
        {
            _player2Score.Value += 1;
        }

        if(_player1Score.Value >= _maxScore)
        {
            AddScoreToPlayerCurrency();
            EventManager.OnCallGameOverTrigger(1);
        }
        else if(_player2Score.Value >= _maxScore)
        {
            AddScoreToPlayerCurrency();
            EventManager.OnCallGameOverTrigger(2);
        }
    }

    private void UpdatePlayer1ScoreUI(int previousValue, int newValue)
    {
        _player1Score_txt.text = "Player 1\n" + newValue.ToString();
    }

    private void UpdatePlayer2ScoreUI(int previousValue, int newValue)
    {
        _player2Score_txt.text = "Player 2\n" + newValue.ToString();
    }

    private void AddScoreToPlayerCurrency()
    {
        if (!IsServer) return;

        UpdateScoreClientRpc(_player1Score.Value, _player2Score.Value);
        /*
        if (NetworkManager.Singleton.LocalClientId == 0)
        {
            DataHolder.Instance.runtimePlayerDataSO.playerData += _player1Score.Value;
            //_playerDataSO.playerData += _player1Score.Value;
        }
        else if(NetworkManager.Singleton.LocalClientId == 1)
        {
            DataHolder.Instance.runtimePlayerDataSO.playerData += _player2Score.Value;
            //_playerDataSO.playerData += _player2Score.Value;
        }
        */
    }

    [ClientRpc]
    private void UpdateScoreClientRpc(int player1Score, int player2Score)
    {
        ulong localId = NetworkManager.Singleton.LocalClientId;

        if (localId == 0)
        {
            DataHolder.Instance.runtimePlayerDataSO.playerData += player1Score;
        }
        else if (localId == 1)
        {
            DataHolder.Instance.runtimePlayerDataSO.playerData += player2Score;
        }
    }

    private void ResetMatchScores()
    {
        if (IsServer)
        {
            _player1Score.Value = 0;
            _player2Score.Value = 0;
        }
    }
}
