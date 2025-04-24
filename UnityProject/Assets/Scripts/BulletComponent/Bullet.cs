using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Netcode;

public class Bullet : NetworkBehaviour
{
 
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private CircleCollider2D _circleCollider;
    [SerializeField] private ParticleSystem _bulletEffect;
    private BulletDetail _currentBulletConfig;
    private int _bulletDamage;
    private float _speed = 0;
    private bool _canMove = false;
   

    private void Start()
    {
        //LoadDefaultConfigBulletConfig();
    }
    private void OnEnable()
    {
        _canMove = true;
        _circleCollider.enabled = true;
    }

    private void OnDisable()
    {
        _canMove = false;
        _circleCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_canMove) return;
        transform.Translate(Vector3.right * Time.deltaTime * _speed);
    }

    public void LoadDefaultConfigBulletConfig(BulletDetail bulletDetail, float gunSpeed)
    {
        _currentBulletConfig = bulletDetail;
        _bulletDamage = bulletDetail.bulletDamage;
        
        if (IsServer)
        {
            _speed = gunSpeed;
            SetBulletStatusClientRpc(gunSpeed);
        }

        //_spriteRenderer.sprite = bulletDetail.bulletSprite;
        //_bulletEffect = bulletDetail.bulletCollisionEffect;
    }

    [ClientRpc]
    private void SetBulletStatusClientRpc(float speed)
    {
        _speed = speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (!IsServer || !NetworkObject.IsSpawned) return;

        if (other.gameObject.tag == "Enemy")
        {
            LifeController lifeController = other.gameObject.GetComponent<LifeController>();
            lifeController.GetDamage(_bulletDamage);
        }
        else
        {

            BulletDestroyClientRpc();
        }


        DestroyBullet();
    }

    [ClientRpc]
    private void BulletDestroyClientRpc()
    {
        Instantiate(_bulletEffect, transform.position, transform.rotation);
    }

    private void DestroyBullet()
    {
        _speed = 0;
        NetworkObject.Despawn(true);
    }
}
