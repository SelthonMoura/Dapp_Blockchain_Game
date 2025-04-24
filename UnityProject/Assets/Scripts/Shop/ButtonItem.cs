using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonItem : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryButton;
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
        _inventoryButton.GetComponent<Button>().interactable = true;
        //}
    }
}
