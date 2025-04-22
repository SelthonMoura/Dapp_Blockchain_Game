using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonItem : MonoBehaviour
{
    [SerializeField] private GunSO _gunDetail;
    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
    }

    public void ButtonClick()
    {
        // if enought money
        //{
        _button.interactable = false;
        EventManager.OnBuyEventTrigger(_gunDetail);
        //}
    }
}
