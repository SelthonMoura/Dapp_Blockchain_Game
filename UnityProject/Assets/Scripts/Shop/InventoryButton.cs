using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryButton : MonoBehaviour
{
    [SerializeField] GunSO _gun;
    [SerializeField] CurrentEquipedGunSO _equipedGun;
    public void SelectWeapon()
    {
        _equipedGun.currentEquipedGunSO = _gun;
        EventManager.OnNewGunSelectedTrigger(this.gameObject);
    }
}
