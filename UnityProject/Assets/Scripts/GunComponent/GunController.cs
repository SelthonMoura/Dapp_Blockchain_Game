using System;
using UnityEngine;
using Unity.Netcode;
using Unity.BossRoom.Infrastructure;

public class GunController : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _spawnBulletTransform;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private GunDetail _currentEquipedGun;
    private AudioClip _currentBulletShootSound;
    private float _shootVelocity;
    private BulletDetail _currentEquipedBullet;
    private PlayerInput _playerInput;

    private bool _loadingGun = false;

    // NetworkVariable para sincronizar o ângulo
    private NetworkVariable<float> angleVariable = new NetworkVariable<float>(
        0f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    private void Awake()
    {
        EventManager.OnChangeGunEvent += LoadGunComponents;
    }

    private void Start()
    {
        _playerInput = FindObjectOfType<PlayerInput>();
        _playerInput.OnShootAction += PlayerInput_OnShootAction;
        LoadGunComponents(_currentEquipedGun);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        _playerInput.OnShootAction -= PlayerInput_OnShootAction;
        EventManager.OnChangeGunEvent -= LoadGunComponents;
    }

    void Update()
    {
        Aim();
    }

    private void LoadGunComponents(GunDetail gunDetails)
    {
        _loadingGun = true;
        _currentEquipedGun = gunDetails;
        _spriteRenderer.sprite = _currentEquipedGun.gunSprite;
        _shootVelocity = _currentEquipedGun.gunShootVelocity;
        _currentEquipedBullet = _currentEquipedGun.defaultGunBullet;
        _currentBulletShootSound = _currentEquipedGun.gunSound;
        _loadingGun = false;
    }

    private void PlayerInput_OnShootAction(object sender, EventArgs e)
    {

        if (!IsLocalPlayer) return;
        if (_loadingGun) return;
        _audioSource.PlayOneShot(_currentBulletShootSound);
        ShootServerRpc();
        
    }

    [ServerRpc]
    private void ShootServerRpc()
    {
        NetworkObject bullet = NetworkObjectPool.Singleton.GetNetworkObject(_bulletPrefab, _spawnBulletTransform.position, transform.rotation);
        bullet.Spawn();
        bullet.GetComponent<Bullet>().LoadDefaultConfigBulletConfig(_currentEquipedBullet, _shootVelocity);

    }

    private void Aim()
    {
        if (IsOwner && Application.isFocused)
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

            if (Mathf.Abs(angleVariable.Value - angle) > 0.01f)
            {
                angleVariable.Value = angle;
            }
        }

        RotationInterpreter();
    }

    private void RotationInterpreter()
    {
        float angle = angleVariable.Value;

        transform.rotation = Quaternion.Euler(0, 0, angle);

        _spriteRenderer.flipY = angle > 90 || angle < -90;
    }
}
