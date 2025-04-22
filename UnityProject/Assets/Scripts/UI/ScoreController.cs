using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private GameOverController _gameOverControllerSO;
    [SerializeField] private TMP_Text _player1Score;
    [SerializeField] private TMP_Text _player2Score;

    private void Start()
    {
        EventManager.OnUpdateUIEvent += UpdateScores;

        UpdateScores();
    }

    private void OnDestroy()
    {
        EventManager.OnUpdateUIEvent -= UpdateScores;
    }

    private void UpdateScores()
    {
        _player1Score.text = "Player 1\n" + _gameOverControllerSO.playerOneScore;
        _player2Score.text = "Player 2\n" + _gameOverControllerSO.playerTwoScore;
    }
}
