using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gun Stats")]
public class GunSO : ScriptableObject
{
    public Sprite sprite;

    public int damage;
    public float cooldownTime;
    public float bulletCount;
    public float bulletSpeed;
    public float bulletSpread;
}
