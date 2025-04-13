using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory : MonoBehaviour
{
    private List<GameObject> _boughtItens;

    private void Start()
    {
        EventManager.OnBuyItemEvent += AddToBoughtItensList;
    }

    private void OnDestroy()
    {
        EventManager.OnBuyItemEvent -= AddToBoughtItensList;
    }

    private void AddToBoughtItensList(GameObject item)
    {
        //_boughtItens.Add(item);
        GameObject boughtItem = Instantiate(item);
        boughtItem.transform.parent = transform;
    }
}
