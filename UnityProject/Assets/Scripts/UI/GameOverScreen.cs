using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private PlayerDataSO playerDataSO;
    [SerializeField] private TMP_Text _totalPoints;

    private void OnEnable()
    {
        StartCoroutine(WaitATick());
    }

    private IEnumerator WaitATick()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        _totalPoints.text = "Total points: " + DataHolder.Instance.runtimePlayerDataSO.playerData;
        Debug.Log(DataHolder.Instance.runtimePlayerDataSO.playerData);
    }
}
