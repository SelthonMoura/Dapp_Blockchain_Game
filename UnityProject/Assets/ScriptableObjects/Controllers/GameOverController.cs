using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Over Controller")]
public class GameOverController : ScriptableObject
{
    public int playerOneScore;
    public int playerTwoScore;

    public int maxHitsCount;
    public int currentHitsCount;

    public int totalPointsToWin;
}
