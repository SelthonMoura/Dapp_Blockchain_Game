using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private Image itemImage;
    private GunDetail _gunDetail;

    public void ButtonClick()
    {
        EventManager.OnSendTransactionTrigger(_gunDetail.gunName, _gunDetail.price);
    }

    public GunDetail GetItem()
    {
        return _gunDetail;
    }

    internal void Setup(GunDetail gun)
    {
        _gunDetail = gun;
        itemName.text = gun.gunName;
        itemImage.sprite = gun.gunSprite;
    }
}
