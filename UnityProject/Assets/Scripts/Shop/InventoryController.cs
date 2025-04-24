using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.Progress;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private CurrentEquipedGunSO _currentGun;
    [SerializeField] private GunSO _gun;
    [SerializeField] List<GameObject> _buttonsList;

    private void Start()
    {
        //EventManager.OnBuyItemEvent += AddToBoughtItensList;
        EventManager.OnNewGunSelectedEvent += SelectWeapon;
    }

    private void OnDestroy()
    {
        //EventManager.OnBuyItemEvent -= AddToBoughtItensList;
        EventManager.OnNewGunSelectedEvent -= SelectWeapon;
    }

    private void AddToBoughtItensList(GameObject item)
    {
        for(int i = 0; i  < _buttonsList.Count; i++)
        {
            if(item == _buttonsList[i])
                _buttonsList[i].GetComponent<Button>().interactable = true;
        }
    }

    public void ButtonClicked()
    {
        _currentGun.currentEquipedGunSO = _gun;
        EventManager.OnNewGunSelectedTrigger(gameObject);
    }

    private void SelectWeapon(GameObject button)
    {
        for (int i = 0; i < _buttonsList.Count; i++)
        {
            if (button == _buttonsList[i])
                _buttonsList[i].GetComponent<Button>().interactable = false;
            else
                _buttonsList[i].GetComponent<Button>().interactable = true;
        }
    }
}
