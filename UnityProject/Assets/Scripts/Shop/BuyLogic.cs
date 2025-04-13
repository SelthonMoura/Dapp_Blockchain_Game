using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyLogic : MonoBehaviour
{
    public void BuyItem(GameObject item)
    {
        EventManager.OnBuyEventTrigger(item);
    }
}
