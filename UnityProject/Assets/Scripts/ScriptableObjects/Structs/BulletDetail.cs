using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletDetail", menuName = "ScriptableObjects/BulletDetail")]
public class BulletDetail : ScriptableObject
{
    public int bulletDamage;
    public Sprite bulletSprite;
    public ParticleSystem bulletCollisionEffect;
}
