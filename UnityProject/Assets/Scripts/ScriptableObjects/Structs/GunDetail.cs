using UnityEngine;


[CreateAssetMenu(fileName = "GunDetail", menuName ="ScriptableObjects/GunDetail")]
public class GunDetail : ScriptableObject
{
    public string gunName;
    public Sprite gunSprite;
    public float gunShootVelocity;
    public AudioClip gunSound;
    public BulletDetail defaultGunBullet;
    public float price;
}
