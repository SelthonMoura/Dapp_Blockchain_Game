using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private Image itemImage;
    private int gunIndex;
    public void SelectWeapon()
    {
        EventManager.OnNewGunSelectedTrigger(gunIndex);
    }

    internal void Setup(GunDetail gun, int index)
    {
        gunIndex = index;
        itemName.text = gun.gunName;
        itemImage.sprite = gun.gunSprite;
    }
}
