using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeGunControler : MonoBehaviour
{

    [SerializeField]
    private GunDetail[] _equipedGuns;

    private int _selectedGun = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeGun();
        }
    }

    private void ChangeGun()
    {
        GunDetail selectedGun = SelectGun();
        EventManager.OnChangeGunTrigger(selectedGun);
    }
    private GunDetail SelectGun()
    {
        if (_selectedGun == 0)
        {
            _selectedGun++;
            return _equipedGuns[1];
        }
        else
        {
            _selectedGun = 0;
            return _equipedGuns[0];
        }
    }
}
