using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private GameObject _baseButton;
    private List<GunSO> _boughtItens;

    private void Start()
    {
        EventManager.OnBuyItemEvent += AddToBoughtItensList;
    }

    private void OnDestroy()
    {
        EventManager.OnBuyItemEvent -= AddToBoughtItensList;
    }

    private void AddToBoughtItensList(GunSO item)
    {
        //_boughtItens.Add(item);
        GameObject boughtItem = Instantiate(_baseButton);
        boughtItem.transform.parent = transform;
    }
}
