using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private Button _playBtn;
    [SerializeField] private Button _creditsBtn;
    [SerializeField] private GameObject _creditsPanel;
    [SerializeField] private Button _exitCreditsBtn;
    
    public void ChangeActivePanel(int panel)
    {
        StartCoroutine(WaitSeconds(1f, panel));
    }

    IEnumerator WaitSeconds(float seconds, int panel)
    {
        yield return new WaitForSeconds(seconds);
        
        if (panel == 1)
        {
            _creditsPanel.SetActive(true);
            _menuPanel.SetActive(false);
            _exitCreditsBtn.interactable = true;
        }
        else
        {
            _menuPanel.SetActive(true);
            _creditsPanel.SetActive(false);
            _creditsBtn.interactable = true;
            _playBtn.interactable = true;
        }
    }
}
