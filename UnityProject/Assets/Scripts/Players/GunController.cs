using System.Collections;
using UnityEngine;

public class GunController : MonoBehaviour
{
    // References
    [SerializeField] private CurrentEquipedGunSO _currentGun;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private SpriteRenderer _gunSpriteRenderer;
    [SerializeField] private Transform _bulletShooterPoint;
    [SerializeField] private GameObject _bullet;
    private GunSO _gunSO;

    // Variables
    private bool _canShoot;
    private bool _isPlayerOne;
    private Transform _playerTransform;

    private void Start()
    {
        _gunSO = _currentGun.currentEquipedGunSO;
        _playerTransform = _playerController.transform;

        _isPlayerOne = _playerController.GetControlScheme() == PlayerController.ControlScheme.WASD;
        _canShoot = true;
    }

    private void Update()
    {
        GunInputPressed();
    }

    private void GunInputPressed()
    {
        if (!_canShoot) return;

        bool shootKey = _isPlayerOne ? Input.GetKey(KeyCode.Space) : Input.GetKey(KeyCode.KeypadEnter);

        if (shootKey)
        {
            Shoot();
            StartCoroutine(GunCoolDown());
        }
    }

    private void Shoot()
    {
        float startAngle = -(_gunSO.bulletSpread * (_gunSO.bulletCount - 1) / 2f);

        // Determine facing direction
        float directionMultiplier = Mathf.Sign(_playerTransform.localScale.x); // 1 (right) or -1 (left)
        Vector3 baseRotation = transform.eulerAngles;

        for (int i = 0; i < _gunSO.bulletCount; i++)
        {
            float angleOffset = startAngle + _gunSO.bulletSpread * i;

            // Invert spread if facing left
            float angle = directionMultiplier > 0 ? angleOffset : -angleOffset;

            // Flip rotation if facing left
            Quaternion rotation = Quaternion.Euler(0, 0, baseRotation.z + angle);

            GameObject bullet = Instantiate(_bullet, _bulletShooterPoint.position, rotation);
            bullet.GetComponent<BulletController>().SetDamage(_gunSO.damage);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 shootDirection = bullet.transform.right * directionMultiplier;
                rb.velocity = shootDirection * _gunSO.bulletSpeed;
            }
            SetBulletOwner(bullet);
        }
    }

    private void SetBulletOwner(GameObject bullet)
    {
        if (_isPlayerOne)
            bullet.gameObject.tag = "Bullet_1";
        else
            bullet.gameObject.tag = "Bullet_2";
    }

    private IEnumerator GunCoolDown()
    {
        _canShoot = false;
        yield return new WaitForSeconds(_gunSO.cooldownTime);
        _canShoot = true;
    }
}
