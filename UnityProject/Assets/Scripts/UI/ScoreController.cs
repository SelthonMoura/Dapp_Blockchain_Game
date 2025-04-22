using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private GameOverController _gameOverControllerSO;
    [SerializeField] private TMP_Text _player1Score;
    [SerializeField] private TMP_Text _player2Score;

    private void Awake()
    {
        _gameOverControllerSO.playerOneScore = 0;
        _gameOverControllerSO.playerTwoScore = 0;
    }

    private void Start()
    {
        EventManager.OnUpdateUIEvent += UpdateScores;
        EventManager.OnAddToHitsCountEvent += AddToHits;

        UpdateScores();
    }

    private void OnDestroy()
    {
        EventManager.OnUpdateUIEvent -= UpdateScores;
        EventManager.OnAddToHitsCountEvent -= AddToHits;
    }

    private void UpdateScores()
    {
        _player1Score.text = "Player 1\n" + _gameOverControllerSO.playerOneScore;
        _player2Score.text = "Player 2\n" + _gameOverControllerSO.playerTwoScore;
    }

    private void AddToHits()
    {
        _gameOverControllerSO.currentHitsCount += 1;

        if (_gameOverControllerSO.playerOneScore >= _gameOverControllerSO.totalPointsToWin
            || _gameOverControllerSO.playerTwoScore >= _gameOverControllerSO.totalPointsToWin)
        {
            ResetHits();
            EventManager.OnCallGameOverTrigger();
        }
        else if (_gameOverControllerSO.currentHitsCount >= _gameOverControllerSO.maxHitsCount)
        {
            ResetHits();
            EventManager.OnCallStorePanelTrigger();
        }
    }

    private void ResetHits()
    {
        _gameOverControllerSO.currentHitsCount = 0;
    }
}
