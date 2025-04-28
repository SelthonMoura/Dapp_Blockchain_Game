using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHolder : MonoBehaviour
{
    public static DataHolder Instance { get; private set; }

    public PlayerDataSO runtimePlayerDataSO;
    public int equippedGun;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist between scenes

        EventManager.OnNewGunSelectedEvent += EquipGun;
    }

    private void EquipGun(int gun)
    {
        equippedGun = gun;
    }
}
